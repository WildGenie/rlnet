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

namespace RLNET
{
    /// <summary>
    /// Represents a color to be drawn
    /// </summary>
    public struct RLColor
    {
        public float r;
        public float g;
        public float b;

        public static RLColor Black { get; private set; }
        public static RLColor Blue { get; private set; }
        public static RLColor Green { get; private set; }
        public static RLColor Cyan { get; private set; }
        public static RLColor Red { get; private set; }
        public static RLColor Magenta { get; private set; }
        public static RLColor Brown { get; private set; }
        public static RLColor LightGray { get; private set; }
        public static RLColor Gray { get; private set; }
        public static RLColor LightBlue { get; private set; }
        public static RLColor LightGreen { get; private set; }
        public static RLColor LightCyan { get; private set; }
        public static RLColor LightRed { get; private set; }
        public static RLColor LightMagenta { get; private set; }
        public static RLColor Yellow { get; private set; }
        public static RLColor White { get; private set; }
        public static RLColor[] CGA { get; private set; }

        static RLColor()
        {
            CGA = new RLColor[16];
            CGA[0] = Black = new RLColor(0, 0, 0);
            CGA[1] = Blue = new RLColor(0, 0, 255);
            CGA[2] = Green = new RLColor(0, 170, 0);
            CGA[3] = Cyan = new RLColor(0, 170, 170);
            CGA[4] = Red = new RLColor(170, 0, 0);
            CGA[5] = Magenta = new RLColor(170, 0, 170);
            CGA[6] = Brown = new RLColor(170, 85, 0);
            CGA[7] = LightGray = new RLColor(170, 170, 170);
            CGA[8] = Gray = new RLColor(85, 85, 85);
            CGA[9] = LightBlue = new RLColor(85, 85, 255);
            CGA[10] = LightGreen = new RLColor(85, 255, 85);
            CGA[11] = LightCyan = new RLColor(85, 255, 255);
            CGA[12] = LightRed = new RLColor(255, 85, 85);
            CGA[13] = LightMagenta = new RLColor(255, 85, 255);
            CGA[14] = Yellow = new RLColor(255, 255, 85);
            CGA[15] = White = new RLColor(255, 255, 255);
        }

        public RLColor(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public RLColor(byte r, byte g, byte b)
        {
            this.r = (float)r / 255f;
            this.g = (float)g / 255f;
            this.b = (float)b / 255f;
        }

        internal OpenTK.Vector3 ToVector3()
        {
            return new OpenTK.Vector3(r, g, b);
        }

        /// <summary>
        /// Blends the two colors
        /// </summary>
        /// <param name="ColorA">Primary Color</param>
        /// <param name="ColorB">Secondary Color</param>
        /// <param name="Ratio">Ratio of the colors that are blended (255 - Full Primary, 0 - Full Secondary)</param>
        /// <returns>New Blended Color</returns>
        public static RLColor Blend(RLColor primary, RLColor secondary, byte ratio)
        {
            return Blend(primary, secondary, (float)ratio / 255f);
        }

        /// <summary>
        /// Evenly blends two colors
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>New Blended Color</returns>
        public static RLColor Blend(RLColor color1, RLColor color2)
        {
            return Blend(color1, color2, .5f);
        }

        /// <summary>
        /// Blends the two colors
        /// </summary>
        /// <param name="ColorA">Primary Color</param>
        /// <param name="ColorB">Secondary Color</param>
        /// <param name="Ratio">Ratio of the colors that are blended (1f - Full Primary, 0f - Full Secondary)</param>
        /// <returns>New Blended Color</returns>
        public static RLColor Blend(RLColor primary, RLColor secondary, float ratio)
        {
            return secondary - ((secondary - primary) * ratio);
        }

        public static RLColor operator +(RLColor color, float f)
        {
            return new RLColor(color.r + f, color.g + f, color.b + f);
        }

        public static RLColor operator -(RLColor color, float f)
        {
            return new RLColor(color.r - f, color.g - f, color.b - f);
        }

        public static RLColor operator *(RLColor color, float f)
        {
            return new RLColor(color.r * f, color.g * f, color.b * f);
        }

        public static RLColor operator /(RLColor color, float f)
        {
            return new RLColor(color.r / f, color.g / f, color.b / f);
        }

        public static RLColor operator +(RLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new RLColor(color.r + f, color.g + f, color.b + f);
        }

        public static RLColor operator -(RLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new RLColor(color.r - f, color.g - f, color.b - f);
        }

        public static RLColor operator *(RLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new RLColor(color.r * f, color.g * f, color.b * f);
        }

        public static RLColor operator /(RLColor color, byte b)
        {
            float f = (byte)b / 255f;
            return new RLColor(color.r / f, color.g / f, color.b / f);
        }

        public static RLColor operator +(RLColor colorA, RLColor colorB)
        {
            return new RLColor(colorA.r + colorB.r, colorA.g + colorB.g, colorA.b + colorB.b);
        }

        public static RLColor operator -(RLColor colorA, RLColor colorB)
        {
            return new RLColor(colorA.r - colorB.r, colorA.g - colorB.g, colorA.b - colorB.b);
        }

        public static RLColor operator *(RLColor colorA, RLColor colorB)
        {
            return new RLColor(colorA.r * colorB.r, colorA.g * colorB.g, colorA.b * colorB.b);
        }

        public static RLColor operator /(RLColor colorA, RLColor colorB)
        {
            return new RLColor(colorA.r / colorB.r, colorA.g / colorB.g, colorA.b / colorB.b);
        }

        public override string ToString()
        {
            return string.Format("R:{0}, B:{1}, G:{2}", r, g, b);
        }
    }
}
