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
#if SIMS3VERSION_1672
    //[StructLayout(LayoutKind.Explicit)]
    //public unsafe struct
    public unsafe static class
        veamonovtable
    {
        //[FieldOffset(0)]
        public static IntPtr klass(IntPtr eaMonoVTable)
        {
            if (eaMonoVTable.value == null)
                throw new ArgumentNullException("eaMonoVTable");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoVTable.value));
        }
        //[FieldOffset(libcs.voidsize * 0x1)]
        public static IntPtr gc_descr(IntPtr eaMonoVTable)
        {
            if (eaMonoVTable.value == null)
                throw new ArgumentNullException("eaMonoVTable");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoVTable.value + (libcs.voidsize * 0x1)));
        }
        //[FieldOffset(libcs.voidsize * 0x2)]
        public static IntPtr domain(IntPtr eaMonoVTable)
        {
            if (eaMonoVTable.value == null)
                throw new ArgumentNullException("eaMonoVTable");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoVTable.value + (libcs.voidsize * 0x2)));
        }
        //[FieldOffset(libcs.voidsize * 0x3)]
        public static IntPtr data(IntPtr eaMonoVTable)
        {
            if (eaMonoVTable.value == null)
                throw new ArgumentNullException("eaMonoVTable");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoVTable.value + (libcs.voidsize * 0x3)));
        }
        //[FieldOffset(libcs.voidsize * 0x4)]
        public static IntPtr type(IntPtr eaMonoVTable)
        {
            if (eaMonoVTable.value == null)
                throw new ArgumentNullException("eaMonoVTable");
            return libcs.tointptr(*(voidptr*)((voidptr)eaMonoVTable.value + (libcs.voidsize * 0x4)));
        }
    }
#endif
}
