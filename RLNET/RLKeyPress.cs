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
        public RLKey Key { get; private set; }
        public bool Alt { get; private set; }
        public bool Shift { get; private set; }
        public bool Control { get; private set; }
        public bool Repeating { get; private set; }
        public bool NumLock { get; private set; }
        public bool CapsLock { get; private set; }
        public bool ScrollLock { get; private set; }
        public char? Char
        {
            get
            {
                switch (Key)
                {
                    case RLKey.A: return Shift ^ CapsLock ? 'A' : 'a';
                    case RLKey.B: return Shift ^ CapsLock ? 'B' : 'b';
                    case RLKey.C: return Shift ^ CapsLock ? 'C' : 'c';
                    case RLKey.D: return Shift ^ CapsLock ? 'D' : 'd';
                    case RLKey.E: return Shift ^ CapsLock ? 'E' : 'e';
                    case RLKey.F: return Shift ^ CapsLock ? 'F' : 'f';
                    case RLKey.G: return Shift ^ CapsLock ? 'G' : 'g';
                    case RLKey.H: return Shift ^ CapsLock ? 'H' : 'h';
                    case RLKey.I: return Shift ^ CapsLock ? 'I' : 'i';
                    case RLKey.J: return Shift ^ CapsLock ? 'J' : 'j';
                    case RLKey.K: return Shift ^ CapsLock ? 'K' : 'k';
                    case RLKey.L: return Shift ^ CapsLock ? 'L' : 'l';
                    case RLKey.M: return Shift ^ CapsLock ? 'M' : 'm';
                    case RLKey.N: return Shift ^ CapsLock ? 'N' : 'n';
                    case RLKey.O: return Shift ^ CapsLock ? 'O' : 'o';
                    case RLKey.P: return Shift ^ CapsLock ? 'P' : 'p';
                    case RLKey.Q: return Shift ^ CapsLock ? 'Q' : 'q';
                    case RLKey.R: return Shift ^ CapsLock ? 'R' : 'r';
                    case RLKey.S: return Shift ^ CapsLock ? 'S' : 's';
                    case RLKey.T: return Shift ^ CapsLock ? 'T' : 't';
                    case RLKey.U: return Shift ^ CapsLock ? 'U' : 'u';
                    case RLKey.V: return Shift ^ CapsLock ? 'V' : 'v';
                    case RLKey.W: return Shift ^ CapsLock ? 'W' : 'w';
                    case RLKey.X: return Shift ^ CapsLock ? 'X' : 'x';
                    case RLKey.Y: return Shift ^ CapsLock ? 'Y' : 'y';
                    case RLKey.Z: return Shift ^ CapsLock ? 'Z' : 'z';
                    case RLKey.BackSlash: return Shift ? '|' : '\\';
                    case RLKey.BracketLeft: return Shift ? '{' : '[';
                    case RLKey.BracketRight: return Shift ? '}' : ']';
                    case RLKey.Comma: return Shift ? '<' : ',';
                    case RLKey.Grave: return Shift ? '~' : '`';
                    case RLKey.Keypad0: return Shift ? (char?)null : '0';
                    case RLKey.Keypad1: return Shift ? (char?)null : '1';
                    case RLKey.Keypad2: return Shift ? (char?)null : '2';
                    case RLKey.Keypad3: return Shift ? (char?)null : '3';
                    case RLKey.Keypad4: return Shift ? (char?)null : '4';
                    case RLKey.Keypad5: return Shift ? (char?)null : '5';
                    case RLKey.Keypad6: return Shift ? (char?)null : '6';
                    case RLKey.Keypad7: return Shift ? (char?)null : '7';
                    case RLKey.Keypad8: return Shift ? (char?)null : '8';
                    case RLKey.Keypad9: return Shift ? (char?)null : '9';
                    case RLKey.KeypadPlus: return '+';
                    case RLKey.KeypadDecimal: return Shift ? (char?)null : '.';
                    case RLKey.KeypadDivide: return '/';
                    case RLKey.KeypadMinus: return '-';
                    case RLKey.KeypadMultiply: return '*';
                    case RLKey.Number0: return Shift ? ')' : '0';
                    case RLKey.Number1: return Shift ? '!' : '1';
                    case RLKey.Number2: return Shift ? '@' : '2';
                    case RLKey.Number3: return Shift ? '#' : '3';
                    case RLKey.Number4: return Shift ? '$' : '4';
                    case RLKey.Number5: return Shift ? '%' : '5';
                    case RLKey.Number6: return Shift ? '^' : '6';
                    case RLKey.Number7: return Shift ? '&' : '7';
                    case RLKey.Number8: return Shift ? '*' : '8';
                    case RLKey.Number9: return Shift ? '(' : '9';
                    case RLKey.Period: return Shift ? '>' : '.';
                    case RLKey.Plus: return Shift ? '+' : '=';
                    case RLKey.Minus: return Shift ? '_' : '-';
                    case RLKey.Quote: return Shift ? '"' : '\'';
                    case RLKey.Semicolon: return Shift ? ':' : ';';
                    case RLKey.Slash: return Shift ? '?' : '/';
                    case RLKey.Space: return Shift ? ' ' : ' ';
                    default: return null;
                }
            }
        }

        public RLKeyPress(RLKey key, bool alt, bool shift, bool control, bool repeating, bool numLock, bool capsLock, bool scrollLock)
        {
            Key = key;
            Alt = alt;
            Shift = shift;
            Control = control;
            Repeating = repeating;
            NumLock = numLock;
            CapsLock = capsLock;
            ScrollLock = scrollLock;
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
