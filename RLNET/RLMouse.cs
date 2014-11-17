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

using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLNET
{
    public class RLMouse
    {
        /// <summary>
        /// Gets the X coordinate of the mouse, in characters.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Gets the Y coordinate of the mouse, in characters.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Indicator whether the left mouse button is currently pressed down.
        /// </summary>
        public bool LeftPressed { get; private set; }

        /// <summary>
        /// Indicator whether the right mouse button is currently pressed down.
        /// </summary>
        public bool RightPressed { get; private set; }

        private bool rightClick;
        private bool leftClick;
        private int charWidth;
        private int charHeight;
        private float scale;

        internal RLMouse(GameWindow window, int charWidth, int charHeight, float scale)
        {
            this.charWidth = charWidth;
            this.charHeight = charHeight;
            this.scale = scale;

            window.MouseMove += window_MouseMove;
            window.MouseDown += window_MouseDown;
            window.MouseUp += window_MouseUp;
        }

        private void window_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                LeftPressed = false;
                leftClick = true;
            }
            else if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                RightPressed = false;
                rightClick = true;
            }
        }

        private void window_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                LeftPressed = true;
                leftClick = false;
            }
            else if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                RightPressed = true;
                rightClick = false;
            }
        }

        private void window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            X = (int)(e.X / (charWidth * scale));
            Y = (int)(e.Y / (charHeight * scale));
        }

        /// <summary>
        /// Gets whether the right mouse button was clicked.
        /// </summary>
        /// <returns></returns>
        public bool GetRightClick()
        {
            if (rightClick)
            {
                rightClick = false;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Gets whether the left mouse button was clicked.
        /// </summary>
        /// <returns></returns>
        public bool GetLeftClick()
        {
            if (leftClick)
            {
                leftClick = false;
                return true;
            }
            else return false;
        }
    }
}
