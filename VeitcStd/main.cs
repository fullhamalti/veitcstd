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

#if VETIC_HAVE_SIMIFACE
using Sims3.SimIFace;
#endif

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
    public class __main
    {
#if VETIC_HAVE_SIMIFACE
        [Tunable]
        public static bool loading;
#endif

        public static void RegisterMethods()
        {
            var cu = AppDomain.CurrentDomain;
            if (cu == null)
                libcs.assert("System.AppDomain.CurrentDomain == null");

            cu.SetData("libcs::abort",
                vmethod.GetGoodMethods(typeof(libcs),
                null,
                "abort",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData("libcs::dprintf",
               vmethod.GetGoodMethods(typeof(libcs),
               new Type[] { typeof(string) },
               "dprintf",
               BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData("libcs::readptr",
                vmethod.GetGoodMethods(typeof(libcs),
                new Type[] { typeof(voidptr) },
                "readptr",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData("libcs::writeptr",
                vmethod.GetGoodMethods(typeof(libcs),
                new Type[] { typeof(voidptr), typeof(voidptr) },
                "writeptr",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData(
                "veitcstd::vcmethod::GetNativeMonoMethod",
                vmethod.GetGoodMethods(typeof(vcmethod),
                new Type[] { typeof(ConstructorInfo) },
                "GetNativeMonoMethod",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData(
                "veitcstd::vmethod::GetNativeMonoMethod",
                vmethod.GetGoodMethods(typeof(vmethod),
                new Type[] { typeof(MethodInfo) },
                "GetNativeMonoMethod",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));

            cu.SetData(
                "veitcstd::vobject::GetObjectAdderess",
                vmethod.GetMethodNameFastS(typeof(vobject), "GetObjectAdderess")
            );

            cu.SetData(
                "veitcstd::vassembly::GetNativeMonoAssembly",
                vmethod.GetMethodNameFastS(typeof(vassembly), "GetNativeMonoAssembly")
            );

            cu.SetData(
                "veitcstd::vassembly::GetRawData",
                vmethod.GetMethodNameFastS(typeof(vassembly), "GetRawData")
            );

            cu.SetData("veitcstd::vassembly::GetBaseDirectory",
                vmethod.GetGoodMethods(typeof(vassembly),
                null,
                "GetBaseDirectory",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public
            ));
        }

        static __main()
        {
#if SIMS3VERSION_1672 && !VEITC_AUTO_INIT_CCTOR
            try
            {
               vdelegate.CallvFunction(vnativefunctions.init_class);
            }
            catch (Exception)
            {}
#endif
            try
            {
                vdelegate.CallvFunction(RegisterMethods);
            }
            catch (Exception)
            { }
        }
    }
}
