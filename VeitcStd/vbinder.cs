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
    public static class vbinder
    {
        public static object ChangeType(Binder binder, object value, Type type, System.Globalization.CultureInfo culture)
        {
            if (binder == null)
                throw new ArgumentNullException("binder");
            if (value == null)
                return null;
            Type vtype = value.GetType();
            if (type.IsByRef)
                type = type.GetElementType();
            if (vtype == type || type.IsInstanceOfType(value))
                return value;
            if (vtype.IsArray && type.IsArray)
            {
                if (Binder.Default.IsArrayAssignable(vtype.GetElementType(), type.GetElementType()))
                    return value;
            }

            if (Binder.Default.check_type(vtype, type))
                return Convert.ChangeType(value, type);

            return null;
        }
        public static bool ConvertArgs(Binder binder, object[] args, ParameterInfo[] pinfo, System.Globalization.CultureInfo culture)
        {
            if (binder == null)
                throw new ArgumentNullException("binder");
            if (pinfo == null)
                throw new ArgumentNullException("pinfo");

            if (args == null)
            {
                if (pinfo.Length == 0)
                    return true;
                else
                    throw new TargetParameterCountException();
            }
            if (pinfo.Length != args.Length)
                throw new TargetParameterCountException();
            for (int i = 0; i < args.Length; ++i)
            {
                object v = ChangeType(binder, args[i], pinfo[i].ParameterType, culture);
                if ((v == null) && (args[i] != null))
                    return false;
                args[i] = v;
            }
            return true;
        }
    }
}
