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

namespace veitcstd
{
    public static class vcmethod
    {
        public static IntPtr GetNativeMonoMethod(System.Reflection.ConstructorInfo ctor)
        {
            if (ctor == null)
                throw new ArgumentNullException("ctor");
            return ((MonoCMethod)ctor).mhandle;
        }

        public static MethodAttributes GetAttributes(ConstructorInfo ctor)
        {
            if (ctor == null)
                throw new ArgumentNullException("ctor");
            var _this = (MonoCMethod)ctor;
            {
                MonoMethodInfo info;
                MonoMethodInfo.get_method_info(_this.mhandle, out info);
                return info.attrs;
            }
        }

        public static bool IsVirtual(ConstructorInfo ctor)
        {
            return (GetAttributes(ctor) & MethodAttributes.Virtual) != 0;
        }
    }
}
