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
using System.Reflection;

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
    public unsafe static class vassembly
    {
        public static bool Equals(Assembly a, Assembly b)
        {
            if (a == b)
                return true;
            if (a == null)
                return false;
            if (b == null)
                return false;
            return a._mono_assembly.value == b._mono_assembly.value;
        }

        public static string GetBaseDirectory()
        {
            // like "C:\\Users\\User\\Desktop\\"
#if SIMS3VERSION_1672
            return veamonoassembly.GetBaseDirectory(typeof(void).Assembly._mono_assembly);
#else 
            return null;
#endif
        }

        public static string GetBaseDirectory(Assembly assembly)
        {
#if SIMS3VERSION_1672
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return veamonoassembly.GetBaseDirectory(assembly._mono_assembly);
#else
            return null;
#endif
        }
        public static IntPtr GetNativeMonoAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return assembly._mono_assembly;
        }
        public static byte[] GetRawData(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
#if SIMS3VERSION_1672
            return veamonoassembly.GetRawData(assembly._mono_assembly);
#else
            return null;
#endif
        }
    }
}
