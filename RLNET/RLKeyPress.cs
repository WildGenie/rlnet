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

namespace RLNET
{
    public class RLKeyPress
    {
        public RLKey Key { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public bool Control { get; set; }
        public bool Repeating { get; set; }

        public RLKeyPress(RLKey key, bool alt, bool shift, bool control, bool repeating)
        {
            Key = key;
            Alt = alt;
            Shift = shift;
            Control = control;
            Repeating = repeating;
        }

        public static bool operator ==(RLKeyPress A, RLKeyPress B)
        {
            if (object.ReferenceEquals(A, B)) return true;
            if (((object)A == null) || ((object)B == null))
            {
                return false;
            }
            else return (A.Key == B.Key && A.Alt == B.Alt && A.Shift == B.Shift && A.Control == B.Control && A.Repeating == B.Repeating);
        }

        public static bool operator !=(RLKeyPress A, RLKeyPress B)
        {
            return !(A == B);
        }

    }
}
