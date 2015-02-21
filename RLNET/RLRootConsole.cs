#region license
/* 
 * Released under the MIT License (MIT)
 * Copyright (c) 2014 Travis M. Clark
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 */
#endregion

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;

namespace RLNET
{
    public class RLRootConsole : RLConsole
    {
        private GameWindow window;
        private bool closed = false;
        private int texId;
        private int vboId; //position bo
        private int iboId; //index bo
        private int tcboId; //tex coord bo
        private int foreColorId; //color3 bo
        private int backColorId; //color3 bo
        private Vector2[] texVertices;
        private Vector3[] colorVertices;
        private Vector3[] backColorVertices;
        private float uRatio;
        private float vRatio;
        private int charsPerRow;

        private float scale;
        private Vector3 scale3;
        private int charWidth;
        private int charHeight;
        private RLResizeType resizeType;
        private int offsetX;
        private int offsetY;

        public event UpdateEventHandler Render;
        public event UpdateEventHandler Update;
        public event EventHandler<EventArgs> OnLoad;
        public event EventHandler<System.ComponentModel.CancelEventArgs> OnClosing;
        public event ResizeEventHandler OnResize;

        public RLKeyboard Keyboard { get; private set; }
        public RLMouse Mouse { get; private set; }

        /// <summary>
        /// Creates the root console.
        /// </summary>
        /// <param name="bitmapFile">Path to bitmap file.</param>
        /// <param name="width">Width in characters of the console.</param>
        /// <param name="height">Height in characters of the console.</param>
        /// <param name="charWidth">Width of the characters.</param>
        /// <param name="charHeight">Height of the characters.</param>
        /// <param name="scale">Scale the window by this value.</param>
        /// <param name="title">Title of the window.</param>
        public RLRootConsole(string bitmapFile, int width, int height, int charWidth, int charHeight, float scale = 1f, string title = "RLNET Console")
            : base(width, height)
        {
            RLSettings settings = new RLSettings();
            settings.BitmapFile = bitmapFile;
            settings.Width = width;
            settings.Height = height;
            settings.CharWidth = charWidth;
            settings.CharHeight = charHeight;
            settings.Scale = scale;
            settings.Title = title;

            Init(settings);
        }

        /// <summary>
        /// Creates the root console.
        /// </summary>
        /// <param name="settings">Settings for the RLRootConsole.</param>
        public RLRootConsole(RLSettings settings)
            : base(settings.Width, settings.Height)
        {
            Init(settings);
        }

        private void Init(RLSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (settings.BitmapFile == null)
                throw new ArgumentNullException("BitmapFile");
            if (settings.Title == null)
                throw new ArgumentNullException("Title");
            if (settings.Width <= 0)
                throw new ArgumentException("Width cannot be zero or less");
            if (settings.Height <= 0)
                throw new ArgumentException("Height cannot be zero or less");
            if (settings.CharWidth <= 0)
                throw new ArgumentException("CharWidth cannot be zero or less");
            if (settings.CharHeight <= 0)
                throw new ArgumentException("CharHeight cannot be zero or less");
            if (settings.Scale <= 0)
                throw new ArgumentException("Scale cannot be zero or less");
            if (System.IO.File.Exists(settings.BitmapFile) == false)
                throw new System.IO.FileNotFoundException("cannot find bitmapFile");



            this.scale = settings.Scale;
            this.scale3 = new Vector3(scale, scale, 1f);
            this.charWidth = settings.CharWidth;
            this.charHeight = settings.CharHeight;
            this.resizeType = settings.ResizeType;

            window = new GameWindow((int)(settings.Width * charWidth * scale), (int)(settings.Height * charHeight * scale), GraphicsMode.Default);
            window.WindowBorder = (WindowBorder)settings.WindowBorder;
            if (settings.StartWindowState == RLWindowState.Fullscreen || settings.StartWindowState == RLWindowState.Maximized)
            {
                window.WindowState = (WindowState)settings.StartWindowState;
            }

            window.Title = settings.Title;
            window.RenderFrame += window_RenderFrame;
            window.UpdateFrame += window_UpdateFrame;
            window.Load += window_Load;
            window.Resize += window_Resize;
            window.Closed += window_Closed;
            window.Closing += window_Closing;
            Mouse = new RLMouse(window);
            Keyboard = new RLKeyboard(window);
            LoadTexture2d(settings.BitmapFile);

            vboId = GL.GenBuffer();
            iboId = GL.GenBuffer();
            tcboId = GL.GenBuffer();
            foreColorId = GL.GenBuffer();
            backColorId = GL.GenBuffer();
            CalcWindow(true);
        }

        void window_Load(object sender, EventArgs e)
        {
            window.VSync = VSyncMode.On;
            if (OnLoad != null) OnLoad(this, e);
        }

        void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (OnClosing != null) OnClosing(this, e);
        }

        /// <summary>
        /// Resizes the window.
        /// </summary>
        /// <param name="width">The new width of the window, in pixels.</param>
        /// <param name="height">The new height of the window, in pixels.</param>
        public void ResizeWindow(int width, int height)
        {
            window.Width = width;
            window.Height = height;
        }

        public void SetWindowState(RLWindowState windowState)
        {
            window.WindowState = (WindowState)windowState;
        }

        public void LoadBitmap(string bitmapFile, int charWidth, int charHeight)
        {
            if (bitmapFile == null)
                throw new ArgumentNullException("bitmapFile");
            if (charWidth <= 0)
                throw new ArgumentException("charWidth cannot be zero or less");
            if (charHeight <= 0)
                throw new ArgumentException("charHeight cannot be zero or less");

            if (this.charWidth != charWidth || this.charHeight != charHeight)
            {
                this.charWidth = charWidth;
                this.charHeight = charHeight;

                if (resizeType == RLResizeType.ResizeCells || window.WindowState == WindowState.Fullscreen || window.WindowState == WindowState.Maximized)
                {
                    CalcWindow(false);
                }
                else
                {
                    int viewWidth = (int)(Width * charWidth * scale);
                    int viewHeight = (int)(Height * charHeight * scale);

                    if (viewWidth != window.Width || viewHeight != window.Height)
                    {
                        ResizeWindow(viewWidth, viewHeight);
                    }
                    else
                    {
                        CalcWindow(false);
                    }
                }
            }
            LoadTexture2d(bitmapFile);
        }

        public void Close()
        {
            window.Close();
        }

        void window_Resize(object sender, EventArgs e)
        {
            CalcWindow(false);
        }

        private void CalcWindow(bool startup)
        {
            if (resizeType == RLResizeType.None)
            {
                int viewWidth = (int)(Width * charWidth * scale);
                int viewHeight = (int)(Height * charHeight * scale);
                int newOffsetX = (window.Width - viewWidth) / 2;
                int newOffsetY = (window.Height - viewHeight) / 2;

                if (startup || offsetX != newOffsetX || offsetY != newOffsetY)
                {
                    offsetX = newOffsetX;
                    offsetY = newOffsetY;
                    Mouse.Calibrate(charWidth, charHeight, offsetX, offsetY, scale);
                    GL.Viewport(offsetX, offsetY, viewWidth, viewHeight);
                }

                if (startup)
                {
                    CreateBuffers(Width, Height);
                }
            }
            else if (resizeType == RLResizeType.ResizeCells)
            {
                int width = window.Width / charWidth;
                int height = window.Height / charHeight;

                if (startup || width != Width || height != Height)
                {
                    Resize(width, height);
                    CreateBuffers(width, height);
                    GL.Viewport(0, 0, (int)(Width * charWidth * scale), (int)(Height * charHeight * scale));
                    Mouse.Calibrate(charWidth, charHeight, offsetX, offsetY, scale);
                    if (OnResize != null) OnResize(this, new ResizeEventArgs(width, height));
                }
            }
            else if (resizeType == RLResizeType.ResizeScale)
            {
                float newScale = Math.Min(window.Width / (charWidth * Width), window.Height / (charHeight * Height));
                int viewWidth = (int)(Width * charWidth * newScale);
                int viewHeight = (int)(Height * charHeight * newScale);
                int newOffsetX = (window.Width - viewWidth) / 2;
                int newOffsetY = (window.Height - viewHeight) / 2;

                if (startup || newScale != scale || offsetX != newOffsetX || offsetY != newOffsetY)
                {
                    offsetX = newOffsetX;
                    offsetY = newOffsetY;
                    scale = newScale;
                    scale3 = new Vector3(scale, scale, 1);
                    Mouse.Calibrate(charWidth, charHeight, offsetX, offsetY, scale);
                    GL.Viewport(offsetX, offsetY, viewWidth, viewHeight);
                }

                if (startup)
                {
                    CreateBuffers(Width, Height);
                }
            }
        }

        ~RLRootConsole()
        {
            if (window != null)
                window.Dispose();

            if (!closed)
            {
                GL.DeleteBuffer(vboId);
                GL.DeleteBuffer(iboId);
                GL.DeleteBuffer(tcboId);
                GL.DeleteBuffer(foreColorId);
                GL.DeleteBuffer(backColorId);
                GL.DeleteTexture(texId);
            }
        }

        private void CreateBuffers(int width, int height)
        {
            Vector2[] vertices = CreateVertices(width, height, charWidth, charHeight);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * 2 * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            texVertices = new Vector2[width * height * 4];
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texVertices.Length * 2 * sizeof(float)), texVertices, BufferUsageHint.DynamicDraw);

            colorVertices = new Vector3[width * height * 4];
            GL.BindBuffer(BufferTarget.ArrayBuffer, foreColorId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorVertices.Length * 3 * sizeof(float)), colorVertices, BufferUsageHint.DynamicDraw);

            backColorVertices = new Vector3[width * height * 4];
            GL.BindBuffer(BufferTarget.ArrayBuffer, backColorId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(backColorVertices.Length * 3 * sizeof(float)), backColorVertices, BufferUsageHint.DynamicDraw);

            uint[] indices = CreateIndices(width * height);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Opens the window and begins the game loop.
        /// </summary>
        public void Run(double fps = 30d)
        {
            window.Run(fps);
        }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            set
            {
                window.Title = value;
            }
            get
            {
                return window.Title;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse cursor is visible.
        /// </summary>
        public bool CursorVisible
        {
            set
            {
                window.CursorVisible = value;
            }
            get
            {
                return window.CursorVisible;
            }
        }

        /// <summary>
        /// Returns whether the window has been closed.
        /// </summary>
        /// <returns></returns>
        public bool IsWindowClosed()
        {
            return closed;
        }

        private void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            if (Update != null)
                Update(this, new UpdateEventArgs(e.Time));
        }

        private void window_RenderFrame(object sender, FrameEventArgs e)
        {
            if (Render != null)
                Render(this, new UpdateEventArgs(e.Time));
        }

        private void window_Closed(object sender, EventArgs e)
        {
            closed = true;

            GL.DeleteBuffer(vboId);
            GL.DeleteBuffer(iboId);
            GL.DeleteBuffer(tcboId);
            GL.DeleteBuffer(foreColorId);
            GL.DeleteBuffer(backColorId);
            GL.DeleteTexture(texId);
        }

        /// <summary>
        /// Draws the root console to the window.
        /// </summary>
        public void Draw()
        {
            float charU = (float)charWidth * uRatio;
            float charV = (float)charHeight * vRatio;

            //Convert cells to vertices
            for (int iy = 0; iy < Height; iy++)
            {
                for (int ix = 0; ix < Width; ix++)
                {
                    //1 -- 0
                    //2 -- 3
                    int i = (ix + (iy * Width)) * 4;

                    int character = cells[ix, iy].character;

                    float charX = (float)(character % charsPerRow) * charU;
                    float charY = (float)(character / charsPerRow) * charV;

                    texVertices[i].X = charX + charU;
                    texVertices[i].Y = charY;
                    texVertices[i + 1].X = charX;
                    texVertices[i + 1].Y = charY;
                    texVertices[i + 2].X = charX;
                    texVertices[i + 2].Y = charY + charV;
                    texVertices[i + 3].X = charX + charU;
                    texVertices[i + 3].Y = charY + charV;

                    Vector3 backColor = cells[ix, iy].backColor.ToVector3();
                    backColorVertices[i] = backColor;
                    backColorVertices[i + 1] = backColor;
                    backColorVertices[i + 2] = backColor;
                    backColorVertices[i + 3] = backColor;

                    Vector3 color = cells[ix, iy].color.ToVector3();
                    colorVertices[i] = color;
                    colorVertices[i + 1] = color;
                    colorVertices[i + 2] = color;
                    colorVertices[i + 3] = color;
                }
            }

            //Clear
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            //Set Projection
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width * charWidth * scale, Height * charHeight * scale, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            //Setup States
            GL.Enable(EnableCap.VertexArray);
            GL.Enable(EnableCap.IndexArray);
            GL.Enable(EnableCap.ColorArray);
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, texId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.IndexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.Scale(scale3);

            //VBO
            //Vertex Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.VertexPointer(2, VertexPointerType.Float, 2 * sizeof(float), 0);
            //Index Buffer
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboId);

            //Back Color Draw
            //Color Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, backColorId);
            GL.ColorPointer(3, ColorPointerType.Float, 3 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(backColorVertices.Length * 3 * sizeof(float)), backColorVertices, BufferUsageHint.DynamicDraw);
            //Draw Back Color
            GL.DrawElements(PrimitiveType.Triangles, Width * Height * 6, DrawElementsType.UnsignedInt, 0);

            //Fore Color / Texture Draw
            //Texture Coord Buffer
            GL.Enable(EnableCap.Texture2D);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, tcboId);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 2 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texVertices.Length * 2 * sizeof(float)), texVertices, BufferUsageHint.DynamicDraw);
            //Color Buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, foreColorId);
            GL.ColorPointer(3, ColorPointerType.Float, 3 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colorVertices.Length * 3 * sizeof(float)), colorVertices, BufferUsageHint.DynamicDraw);
            //Draw
            GL.DrawElements(PrimitiveType.Triangles, Width * Height * 6, DrawElementsType.UnsignedInt, 0);

            //Clean Up
            GL.Disable(EnableCap.Texture2D);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.DisableClientState(ArrayCap.IndexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            window.SwapBuffers();
        }

        private uint[] CreateIndices(int cellCount)
        {
            uint[] indices = new uint[cellCount * 6];

            for (uint i = 0; i < cellCount; i++)
            {
                uint iv = (uint)(i * 4);
                uint ii = (uint)(i * 6);
                indices[ii] = iv;
                indices[ii + 1] = (uint)(iv + 1);
                indices[ii + 2] = (uint)(iv + 2);
                indices[ii + 3] = (uint)(iv + 2);
                indices[ii + 4] = (uint)(iv + 3);
                indices[ii + 5] = iv;
            }

            return indices;
        }

        private Vector2[] CreateVertices(int width, int height, int charWidth, int charHeight)
        {
            Vector2[] vertices = new Vector2[width * height * 4];
            for (int iy = 0; iy < height; iy++)
            {
                for (int ix = 0; ix < width; ix++)
                {
                    int i = (ix + (iy * width)) * 4;

                    //1 -- 0
                    //2 -- 3

                    int x = ix * charWidth;
                    int y = iy * charHeight;

                    vertices[i].X = x + charWidth;
                    vertices[i].Y = y;

                    vertices[i + 1].X = x;
                    vertices[i + 1].Y = y;

                    vertices[i + 2].X = x;
                    vertices[i + 2].Y = y + charHeight;

                    vertices[i + 3].X = x + charWidth;
                    vertices[i + 3].Y = y + charHeight;
                }
            }
            return vertices;
        }

        private void LoadTexture2d(string filename)
        {
            if (texId != 0) GL.DeleteTexture(texId);
            texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmpData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            charsPerRow = bmp.Width / charWidth;
            uRatio = 1f / (float)bmp.Width;
            vRatio = 1f / (float)bmp.Height;

            //Set Alpha
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            int r = rgbValues[0];
            int b = rgbValues[1];
            int g = rgbValues[2];
            for (int i = 0; i < rgbValues.Length; i += 4)
            {
                if (rgbValues[i] == r && rgbValues[i + 1] == b && rgbValues[i + 2] == g)
                    rgbValues[i + 3] = 0;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            //Create Texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bmp.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }
    }
}
