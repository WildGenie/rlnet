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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNET.Sample
{
    public class Program
    {
        private static int playerX = 25;
        private static int playerY = 25;
        private static RLRootConsole rootConsole;

        public static void Main()
        {
            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = 60;
            settings.Height = 40;
            settings.Scale = 1f;
            settings.Title = "RLNET Sample";
            settings.WindowBorder = RLWindowBorder.Resizable;
            settings.ResizeType = RLResizeType.ResizeCells;
            settings.StartWindowState = RLWindowState.Normal;

            rootConsole = new RLRootConsole(settings);
            rootConsole.Update += rootConsole_Update;
            rootConsole.Render += rootConsole_Render;
            rootConsole.OnLoad += rootConsole_OnLoad;
            rootConsole.Run();

        }

        static void rootConsole_OnLoad(object sender, EventArgs e)
        {
            //rootConsole.SetWindowState(RLWindowState.Fullscreen);
        }

        static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up)
                    playerY--;
                else if (keyPress.Key == RLKey.Down)
                    playerY++;
                else if (keyPress.Key == RLKey.Left)
                    playerX--;
                else if (keyPress.Key == RLKey.Right)
                    playerX++;
                if (keyPress.Key == RLKey.Escape)
                    rootConsole.Close();
            }
        }

        static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();

            rootConsole.Print(1, 1, "Hello World!", RLColor.White);

            rootConsole.SetChar(playerX, playerY, '@');

            int color = 1;
            if (rootConsole.Mouse.LeftPressed)
            {
                color = 4;
            }
            rootConsole.SetBackColor(rootConsole.Mouse.X, rootConsole.Mouse.Y, RLColor.CGA[color]);

            rootConsole.Draw();
        }
    }
}
