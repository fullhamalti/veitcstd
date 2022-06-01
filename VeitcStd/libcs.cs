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
    public static class libcs
    {
        public const int voidsize =
#if VEITCSTD_64BIT_MODE
            0x8;
#else
            0x4;
#endif
            
        public unsafe static long dtol(double value)
        {
            return *(long*)(&value);
        }

        public unsafe static double ltod(long value)
        {
            return *(double*)(&value);
        }

        public unsafe static int ftoi(float value)
        {
            return *(int*)(&value);
        }

        public unsafe static float itof(int value)
        {
            return *(float*)(&value);
        }

        public unsafe static bool littleendian()
        {
            double num = 1.0;
            return *(byte*)(&num) == 0;
        }

        public static ulong swap64(ulong ix)
        {
            return      (ix >> 56) |
                        ((ix << 40) & 0x00FF000000000000) |
                        ((ix << 24) & 0x0000FF0000000000) |
                        ((ix << 8)  & 0x000000FF00000000) |
                        ((ix >> 8)  & 0x00000000FF000000) |
                        ((ix >> 24) & 0x0000000000FF0000) |
                        ((ix >> 40) & 0x000000000000FF00) |
                        (ix << 56);
        }

        public static uint swap32(uint ix)
        {
            return      (ix >> 24) |
                        ((ix << 8) & 0x00FF0000) |
                        ((ix >> 8) & 0x0000FF00) |
                        (ix << 24);
        }

        public static ushort swap16(ushort ix)
        {
            return       (ushort)(((ix >> 8) |
                         (ix << 8)));
        }


        public unsafe static string dumpmemtostr(string nativeClassName, IntPtr ptr, size_t lenHex, bool num)
        {
#if VEITCSTD_64BIT_MODE
            return "";
#else
            if (ptr.value == null)
                return "----DumpMemory----";

            var sb = new System.Text.StringBuilder();
            AppDomain.CurrentDomain.SetData("libcs.dumpmemtostr", sb);

            try
            {
                if (!string.IsNullOrEmpty(nativeClassName))
                {
                    sb.Append("----DumpMemory----\n");
                    sb.Append("Native Class Name: " + nativeClassName + "\n");
                    sb.Append("\n");
                }
                else
                {
                    sb.Append("----DumpMemory----\n");
                    sb.Append("\n");
                }

                for (size_t i = 0; i < lenHex; i += 0x4)
                {
                    sb.Append("[" + fprintptr((voidptr)ptr.value + (voidptr)i) + "]: 0x" + fprintptr((*(voidptr*)((voidptr)ptr.value + i))) + "\n");
                }

                if (num)
                {
                    sb.Append("\n");
                    sb.Append("----------------------------------" + "\n");
                    sb.Append("\n");

                    for (size_t i = 0; i < lenHex; i += 0x4)
                    {
                        sb.Append("[eax+" + i.ToString("X") + "]: 0x" + fprintptr((*(voidptr*)((voidptr)ptr.value + i))) + "\n");
                    }
                }
            }
            catch (Exception)
            { }

            return sb.ToString();
#endif
        }

        public unsafe static IntPtr tointptr(voidptr native)
        {
            var d = default(IntPtr);
            d.value = (void*)native;
            return d;
        }


        public unsafe static size_t compact_non_zero(size_t n, ivoidptr* _in, ivoidptr* _out)
        {
            size_t i;
            ivoidptr* p = _out;

            for (i = 0; i < n; i++)
            {
                ivoidptr v = *_in++;
                if (v != 0)
                    *p++ = v;
            }

            return (size_t)(p - _out);
        }

        public unsafe static voidptr readptr(voidptr ptr)
        {
            if (ptr == 0)
                throw new ArgumentNullException();
            return *(voidptr*)ptr;
        }

        public unsafe static void writeptr(voidptr ptr, voidptr value)
        {
            if (ptr == 0)
                throw new ArgumentNullException();
            *(voidptr*)ptr = value;
        }

        public unsafe static void reduce(ivoidptr* x, ivoidptr* y, ivoidptr num, ivoidptr den)
        {
            ivoidptr n = num, d = den;
            while (d != 0)
            {
                int t = d;
                d = n % d;
                n = t;
            }
            if (n != 0)
            {
                *x = num / n;
                *y = den / n;
            }
            else
            {
                *x = num;
                *y = den;
            }
        }

        public static int yuv2rgb(int yuv)
        {
            double y, Cr, Cb;
            int r, g, b;

            y = (yuv >> 16) & 0xff;
            Cr = (yuv >> 8) & 0xff;
            Cb = (yuv) & 0xff;

            r = (int)(1.164 * (y - 16) + 1.596 * (Cr - 128));
            g = (int)(1.164 * (y - 16) - 0.392 * (Cb - 128) - 0.813 * (Cr - 128));
            b = (int)(1.164 * (y - 16) + 2.017 * (Cb - 128));

            r = (r < 0) ? 0 : r;
            g = (g < 0) ? 0 : g;
            b = (b < 0) ? 0 : b;

            r = (r > 255) ? 255 : r;
            g = (g > 255) ? 255 : g;
            b = (b > 255) ? 255 : b;

            return (r << 16) | (g << 8) | b;
        }

        public static int rgb2yuv(int rgb)
        {
            double r, g, b;
            int y, Cr, Cb;

            r = (rgb >> 16) & 0xff;
            g = (rgb >> 8) & 0xff;
            b = (rgb) & 0xff;

            y = (int)(16.0 + (0.257 * r) + (0.504 * g) + (0.098 * b));
            Cb = (int)(128.0 + (-0.148 * r) - (0.291 * g) + (0.439 * b));
            Cr = (int)(128.0 + (0.439 * r) - (0.368 * g) - (0.071 * b));

            y = (y < 0) ? 0 : y;
            Cb = (Cb < 0) ? 0 : Cb;
            Cr = (Cr < 0) ? 0 : Cr;

            y = (y > 255) ? 255 : y;
            Cb = (Cb > 255) ? 255 : Cb;
            Cr = (Cr > 255) ? 255 : Cr;

            return (y << 16) | (Cr << 8) | Cb;
        }

        public unsafe static bool is_valid_handle(IntPtr handle)
        {
            var h = (uint)handle.value;
            if (h == 0 || h == 0xFFFFFFFF)
                return false;
            return true;
        }

        public static void checkf<T>(T obj)
        {
            if (obj == null)
            {
                try
                {
                    throw new InvalidOperationException("checkf failed obj == null Type: " + typeof(T).ToString());
                }
                catch (Exception ex)
                {
                    assert(ex.ToString());
                }
            }
        }

        // TODO vlist class
        public static bool listremove<T>(System.Collections.Generic.List<T> list, T item) // fast code.  EAMono Interpreter only
        {
            int index = varray<T>.arrayindexof(list._items, item, list._size);
            if (index != -1)
            {
                if (index < 0 || (uint)index > (uint)list._size)
                {
                    throw new ArgumentOutOfRangeException("array_indexof() is out of range.");
                }

                int start = index;
                int delta = -1;

                if (delta < 0)
                    start -= delta;

                varray<T>.arraycpy(list._items, start, list._items, start + delta, list._size - start);

                list._size += delta;
                list._items[list._size] = default(T);
                list._version++;
            }
            return index != -1;
        }

        public static bool runningintelcorecpu()
        {
#if SIMS3VERSION_1672
            if (!vnativefunctions.cache_done_veitc_native_cpuid)
                return false;
            uint eax = 1, ebx = 0, ecx = 0, edx = 0, family;

            vnativefunctions.veitc_native_cpuid(ref eax, ref ebx, ref ecx, ref edx);
            family = ((eax >> 8) & 0xf) + ((eax >> 20) & 0xff);
            if (family == 0x06)
                return true;
#endif
            return false;
        }

        public static string intelgetpname()
        {
#if SIMS3VERSION_1672
            if (!vnativefunctions.cache_done_veitc_native_cpuid)
                return "unknown";
            uint eax = 1, ebx = 0, ecx = 0, edx = 0, family, model;

            vnativefunctions.veitc_native_cpuid(ref eax, ref ebx, ref ecx, ref edx);
            family = ((eax >> 8) & 0xf) + ((eax >> 20) & 0xff);
            model = ((eax >> 4) & 0xf) + ((eax >> 12) & 0xf0);

            switch (family)
            {
            case 0x06:
                {
              switch (model)
              {
                 case 0x1C:
                 case 0x26:
                 case 0x27:
                 case 0x35:
                 case 0x36:
                     return "Bonnell";
                 case 0x2A:
                 case 0x2D:
                     return "Sandy Bridge";
                 case 0x3A:
                 case 0x3E:
                     return "Ivy Bridge";
                 case 0x37:
                 case 0x4A:
                 case 0x4D:
                 case 0x5A:
                 case 0x5D:
                     return "Silvermont";
                 case 0x3C:
                 case 0x3F:
                 case 0x45:
                 case 0x46:
                     return "Haswell";
                 case 0x3D:
                 case 0x4F:
                 case 0x56:
                     return "Broadwell";
                 case 0x4C:
                     return "Skylake";
                 case 0x4E:
                 case 0x5E:
                     return "Airmont";
                 case 0x8E:
                 case 0x9E:
                     return "Kaby Lake";
                 case 0xA5:
                 case 0xA6:
                     return "Comet Lake";
                 case 0x7D:
                 case 0x7E:
                     return "Ice Lake";
                 case 0x8C:
                 case 0x8D:
                     return "Tiger Lake";
                 default:
                     break;
                  }
              } break;
            
            default:
                break;
            }
#endif
            return "unknown";
        }

        public static void dprintf(string str) // dprintf = debug print function
        {
#if SIMS3VERSION_1672
            vnativefunctions.OutputDebugString(str);
#endif
        }

        public static void abort()
        {
#if SIMS3VERSION_1672
            vnativefunctions.veitc_native_force_gamecrash_noexhandle_init(false, true);
#else
            RuntimeMethodHandle.GetFunctionPointer(tointptr(1)); // call mono_assert
#endif
        }

        public static void assert(bool c, string p)
        {
            if (!c)
            {
                while (p != null) { }
            }
        }

        public static void assert(string p)
        {
            while (p != null) { }
        }

        private static char[] fprintptrfFormatHexadecimaldigitLowerTable = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static char[] fprintptrfFormatHexadecimaldigitUpperTable = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public unsafe static string fprintptrf(ulong value, bool positive, int byteSize, int precision, bool upper)
        {
            if (!positive)
            {
                switch (byteSize)
                {
                    case 1:
                        value = (ulong)(256UL - value);
                        break;
                    case 2:
                        value = (ulong)(65536UL - value);
                        break;
                    case 4:
                        value = (ulong)(4294967296UL - value);
                        break;
                }
            }

            char[] digits = (upper ? fprintptrfFormatHexadecimaldigitUpperTable : fprintptrfFormatHexadecimaldigitLowerTable);
            int size = precision > 16 ? precision : 16;
            char* buffer = stackalloc char[size];
            char* last = buffer + size;
            char* ptr = last;

            while (value > 0)
            {
                *--ptr = digits[value & 0xF];
                value >>= 4;
            }

            while (ptr == last || last - ptr < precision)
                *--ptr = '0';

            return new string(ptr, 0, (int)(last - ptr));
        }

        public static string fprintptr(voidptr value)
        {
            return fprintptrf(value, true, 4, 8, true);
        }

        public static void breakf()
        {
            while (true) { }
        }

        public static void unused(params object[] parms) { }

        public static T unusedtype<T>() { return default(T); }
    }
}
