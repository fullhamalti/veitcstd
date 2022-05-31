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
        veamonoassembly
    {
        //[FieldOffset(0)]
        public static IntPtr ref_count(IntPtr eaMonoassembly)
        {
            if (eaMonoassembly.value == null)
                throw new ArgumentNullException("eaMonoassembly");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoassembly.value));
        }

        //[FieldOffset(libcs.voidsize * 0x1)]
        public static IntPtr basedir(IntPtr eaMonoassembly)
        {
            if (eaMonoassembly.value == null)
                throw new ArgumentNullException("eaMonoassembly");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoassembly.value + (libcs.voidsize * 0x1)));
        }

        //[FieldOffset(libcs.voidsize * 0x2)]
        public static IntPtr aname(IntPtr eaMonoassembly)
        {
            if (eaMonoassembly.value == null)
                throw new ArgumentNullException("eaMonoassembly");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoassembly.value + (libcs.voidsize * 0x2)));
        }

        //[FieldOffset(libcs.voidsize * 0x11)]
        public static IntPtr image(IntPtr eaMonoassembly)
        {
            if (eaMonoassembly.value == null)
                throw new ArgumentNullException("eaMonoassembly");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoassembly.value + (libcs.voidsize * 0x11)));
        }

        public static int GetReferenceCount(IntPtr eaMonoassembly)
        {
            return (int)ref_count(eaMonoassembly);
        }

        public static string GetName(IntPtr eaMonoassembly)
        {
            return Marshal.PtrToStringAnsi(aname(eaMonoassembly));
        }

        public static string GetBaseDirectory(IntPtr eaMonoassembly)
        {
            return Marshal.PtrToStringAnsi(basedir(eaMonoassembly));
        }

        public static byte[] GetRawData(IntPtr eaMonoassembly)
        {
            return veamonoimage.GetRawData(image(eaMonoassembly));
        }
    }
#endif
}
