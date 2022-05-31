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
    public unsafe static class vobject
    {
        public class _object_offset4
        {
            public object unused0 = null;
            public object unused1 = null;
            public object unused2 = null;
            public object unused3 = null;
        }

        public unsafe static IntPtr GetNativeMonoVTable(object obj)
        {
            if (obj == null)
                return default(IntPtr);

            return libcs.tointptr(*(voidptr*)obj.obj_address().value);
        }

        public unsafe static IntPtr GetNativeMonoVTable<T>()
        {
#if SIMS3VERSION_1672
            var type = typeof(T);
            if (type == null || type._impl.value == null)
                return default(IntPtr);

            voidptr unknown = *(voidptr*)((voidptr)type._impl.value.value);
            if (unknown == 0x0)
                return default(IntPtr);
            voidptr unknown1 = *(voidptr*)(unknown + 0xB8);
            if (unknown1 == 0x0)
                return default(IntPtr);
            voidptr monoVTable = *(voidptr*)(unknown1 + 0x4);
            if (monoVTable == 0x0) 
                return default(IntPtr);

            return libcs.tointptr(monoVTable); 
#else
            return default(IntPtr);
#endif
        }

        public static bool SetType<T>(object obj)
        {
            if (obj == null)
                return false;

            libcs.unusedtype<T>();
            var vtable = GetNativeMonoVTable<T>();
            if (vtable.value == null)
                return false;

            *(voidptr*)obj.obj_address().value = (voidptr)vtable.value;
            return true;
        }
    }
}
