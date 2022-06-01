// Copyright © 2018-2022 Fullham Alfayet 
//
// veitcstd is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// veitcstd is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

using System;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if VEITCSTD_64BIT_MODE
using size_t = System.UInt64;
using voidptr = System.UInt64;
using ivoidptr = System.Int64;
#else
using size_t = System.UInt32;
using voidptr = System.UInt32;
using ivoidptr = System.Int32;
#endif

namespace veitcstd
{
    public unsafe static class vstring
    {
        private static readonly voidptr offsetData = (voidptr)RuntimeHelpers.OffsetToStringData;

        private static readonly char[] WhiteChars = { (char) 0x9, (char) 0xA, (char) 0xB, (char) 0xC, (char) 0xD,
#if NET_2_0
			(char) 0x85, (char) 0x1680, (char) 0x2028, (char) 0x2029,
#endif
			(char) 0x20, (char) 0xA0, (char) 0x2000, (char) 0x2001, (char) 0x2002, (char) 0x2003, (char) 0x2004,
			(char) 0x2005, (char) 0x2006, (char) 0x2007, (char) 0x2008, (char) 0x2009, (char) 0x200A, (char) 0x200B,
			(char) 0x3000, (char) 0xFEFF };

        // Mono v1.2.3.1 have string.WhiteChars
        public static char[] GetWhiteChars()
        {
            return WhiteChars; //string.WhiteChars;
        }

        public static string NewA(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static string NewU(IntPtr ptr)
        {
            return Marshal.PtrToStringUni(ptr);
        }

        public static byte[] ToUTF8(string instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            return Encoding.UTF8.GetBytes(instance);
        }

        public static string Clone(string instance)
        {
            if (instance == null)
                return instance;

            var stringLength = instance.Length;
            var adderess = (voidptr)instance.obj_address().value;
            return Marshal.PtrToStringUni (
                libcs.tointptr(adderess + offsetData),
                stringLength
            );
        }

        public static IntPtr GetStringData(string instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            var adderess = (voidptr)instance.obj_address().value;
            return libcs.tointptr(adderess + offsetData);
        }

        public static bool BeginsWith(string instance, string text)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (text == null)
                throw new ArgumentNullException("text");

            return instance.StartsWith(text);
        }

        public static int Find(string instance, string what, int from, bool caseSensitive)
        {
            return instance.IndexOf(what, from, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
        }

        public static int GetSliceCount(string instance, string splitter)
        {
            if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(splitter))
                return 0;

            int pos = 0;
            int slices = 1;

            while ((pos = Find(instance, splitter, pos, true)) >= 0)
            {
                slices++;
                pos += splitter.Length;
            }

            return slices;
        }

        public static string Capitalize(string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            string aux = instance.Replace("_", " ").ToLower();
            string cap = "";

            for (int i = 0; i < GetSliceCount(instance, " "); i++)
            {
                string slice = GetSliceCharacter(instance, ' ', i);
                if (slice.Length > 0)
                {
                    slice = char.ToUpper(slice[0]) + slice.Substring(1);
                    if (i > 0)
                        cap += " ";
                    cap += slice;
                }
            }

            return cap;
        }

        public static string GetSliceCharacter(string instance, char splitter, int slice)
        {
            if (!string.IsNullOrEmpty(instance) && slice >= 0)
            {
                int i = 0;
                int prev = 0;
                int count = 0;

                while (true)
                {
                    bool end = instance.Length <= i;

                    if (end || instance[i] == splitter)
                    {
                        if (slice == count)
                        {
                            return instance.Substring(prev, i - prev);
                        }
                        else if (end)
                        {
                            return "";
                        }

                        count++;
                        prev = i + 1;
                    }

                    i++;
                }
            }

            return "";
        }

        public static string BaseName(string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            int index = instance.LastIndexOf('.');

            if (index > 0)
                return instance.Substring(0, index);

            return instance;
        }

        public static string[] Bigrams(string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            string[] b = new string[instance.Length - 1];

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = instance.Substring(i, 2);
            }

            return b;
        }

        public static int BinToInt(string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (instance.Length == 0)
            {
                return 0;
            }

            int sign = 1;

            if (instance[0] == '-')
            {
                sign = -1;
                instance = instance.Substring(1);
            }

            if (instance.StartsWith("0b"))
            {
                instance = instance.Substring(2);
            }

            return sign * Convert.ToInt32(instance, 2);
        }

        public static int Count(string instance, string what)
        {
            return Count(instance, what, true, 0, 0);
        }

        public static int Count(string instance, string what, bool caseSensitive, int from, int to)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (what == null)
            {
                throw new ArgumentNullException("what");
            }

            if (what.Length == 0)
            {
                return 0;
            }

            int len = instance.Length;
            int slen = what.Length;

            if (len < slen)
            {
                return 0;
            }

            string str;

            if (from >= 0 && to >= 0)
            {
                if (to == 0)
                {
                    to = len;
                }
                else if (from >= to)
                {
                    return 0;
                }
                if (from == 0 && to == len)
                {
                    str = instance;
                }
                else
                {
                    str = instance.Substring(from, to - from);
                }
            }
            else
            {
                return 0;
            }
          
            int c = 0;
            int idx;

            do
            {
                idx = str.IndexOf(what, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                if (idx != -1)
                {
                    str = str.Substring(idx + slen);
                    ++c;
                }
            } while (idx != -1);

            return c;
        }

        public static string CEscape(string instance)
        {
            var sb = new StringBuilder(Clone(instance));

            sb.Replace("\\", "\\\\");
            sb.Replace("\a", "\\a");
            sb.Replace("\b", "\\b");
            sb.Replace("\f", "\\f");
            sb.Replace("\n", "\\n");
            sb.Replace("\r", "\\r");
            sb.Replace("\t", "\\t");
            sb.Replace("\v", "\\v");
            sb.Replace("\'", "\\'");
            sb.Replace("\"", "\\\"");
            sb.Replace("?", "\\?");

            return sb.ToString();
        }

        public static string CUnescape(string instance)
        {
            var sb = new StringBuilder(Clone(instance));

            sb.Replace("\\a", "\a");
            sb.Replace("\\b", "\b");
            sb.Replace("\\f", "\f");
            sb.Replace("\\n", "\n");
            sb.Replace("\\r", "\r");
            sb.Replace("\\t", "\t");
            sb.Replace("\\v", "\v");
            sb.Replace("\\'", "\'");
            sb.Replace("\\\"", "\"");
            sb.Replace("\\?", "?");
            sb.Replace("\\\\", "\\");

            return sb.ToString();
        }
    }
}
