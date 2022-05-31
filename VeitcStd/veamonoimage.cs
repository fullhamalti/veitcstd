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
#if SIMS3VERSION_1672
    //[StructLayout(LayoutKind.Explicit)]
    //public unsafe struct
    public unsafe static class 
        veamonoimage
    {
        //[FieldOffset(0x0)]
        public static IntPtr ref_count(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value));
        }

        //[FieldOffset(libcs.voidsize * 0x1)]
        public static IntPtr file_descr(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x1)));
        }

        //[FieldOffset(libcs.voidsize * 0x2)]
        public static IntPtr raw_data(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x2)));
        }

        //[FieldOffset(libcs.voidsize * 0x3)]
        public static IntPtr raw_data_len(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x3)));
        }

        public static byte[] GetRawData(IntPtr eaMonoImage)
        {
            var rawlen = (int)raw_data_len(eaMonoImage);
            byte[] rawBytes = new byte[rawlen];
            Marshal.Copy(raw_data(eaMonoImage), rawBytes, 0, rawlen);
            return rawBytes;
        }

        public static string GetAssemblyName(IntPtr eaMonoImage)
        {
            return Marshal.PtrToStringAnsi(assembly_name(eaMonoImage));
        }

        public static string GetName(IntPtr eaMonoImage)
        {
            return Marshal.PtrToStringAnsi(name(eaMonoImage));
        }

        public static string GetModuleName(IntPtr eaMonoImage)
        {
            return Marshal.PtrToStringAnsi(module_name(eaMonoImage));
        }

        // ...... //

        //[FieldOffset(libcs.voidsize * 0x5)]
        public static IntPtr name(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x5)));
        }

        //[FieldOffset(libcs.voidsize * 0x6)]
        public static IntPtr assembly_name(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x6)));
        }

        //[FieldOffset(libcs.voidsize * 0x7)]
        public static IntPtr module_name(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x7)));
        }

        //[FieldOffset(libcs.voidsize * 0x8)]
        public static IntPtr version(IntPtr eaMonoImage)
        {
            if (eaMonoImage.value == null)
                throw new ArgumentNullException("eaMonoImage");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoImage.value + (libcs.voidsize * 0x8)));
        }
    }
#endif
}
