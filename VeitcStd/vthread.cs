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
    public unsafe static class vthread
    {
        public static IntPtr TlsGetValue(IntPtr threaddata, IntPtr tlsvalue)
        {
#if VEITC_USE_WIN32
            voidptr td = (voidptr)threaddata.value;
            voidptr tv = (voidptr)tlsvalue.value;

            return libcs.tointptr((*(voidptr*)(td + tv * 4 + 0xE10)));
#else
            return default(IntPtr);
#endif
        }
        public static IntPtr GetMonoThreadContext()
        {
#if VEITC_USE_WIN32 && SIMS3VERSION_1672
            return TlsGetValue(vnativefunctions.veitc_native_ptr_fs_zero(0x18), libcs.tointptr((*(voidptr*)0x011F5160)));
#else
            return default(IntPtr);
#endif
        }
    }
}
