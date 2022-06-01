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
#if ENABLE_NATIVE_INJECT
    public unsafe static class vnativeinject
    {
        private static bool FixGetDllImportAttributeDone;
        private static bool inited;

        private static void init()
        {
            if (inited)
                return;

            inited = true;

            try
            {
                
#if VEITC_USE_WIN32 && SIMS3VERSION_1672
                vdelegate.CallvFunction(vnativefunctions.init_class);
                bool ok = vnativefunctions.RunExeUnProtectNativeLibrary();
                var pp = vnativefunctions.RunExeUnProtect();
#else
                var pp = 0;
                bool ok = false;
#endif
                if (ok || pp == 0x2 || pp == 0x80 || pp == 0x40)
                {
                    return;
                }
                else
                {
                    inited = false;
                    throw new InvalidOperationException();
                }
            }
            catch
            { inited = false; throw; }

        }

#if SIMS3VERSION_1672
        /*
        sub esp,0x14
        push ebx
        push ebp
        push esi
        push edi
        call <ts3w.mono_domain_get>
        mov esi,[esp+0x28]
        xor ebp,ebp
        xor ecx,ecx
        cmp word ptr ds:[esi],bp
        mov [esp+0x10],eax
        mov eax,[esi+0x8]
        mov edi,[eax]
        sete cl
        lea ebx,[edi+0x1B4]
        mov [esp+0x28],ebp
        test ecx,0x2000
        je ts3w.E7CD5F <--- if (true)
        pop edi
        pop esi
        pop ebp
        xor eax,eax
        pop ebx
        add esp,0x14
        ret 
        cmp [0x11F5358],ebp
        jne ts3w.E7CDA5
        mov edx,[0x1221E40]
        push ts3w.10B75F4
        push ts3w.1094178
        push edx
        call <ts3w.mono_class_from_name>
        */
        public static void FixGetDllImportAttribute()
        {
            if (FixGetDllImportAttributeDone)
                return;
            MonoMethod obj = null;
            if (obj != null)
            {
                MonoMethod.GetDllImportAttribute(obj.mhandle);
            }
            var method = (MonoMethod)vmethod.GetGoodMethods(typeof(MonoMethod), new Type[] { typeof(IntPtr) }, "GetDllImportAttribute", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            if (method == null)
            {
                libcs.assert("FixGetDllImportAttribute failed");
            }

            voidptr nativeaddress = method == null ? 0 : vmethod.GetNativeFunctionAdderss(method.mhandle);
            if (nativeaddress == 0)
                vmethod.LoadNativeAddress(method, vthread.GetMonoThreadContext(), "GetDllImportAttribute", out nativeaddress);

            if (nativeaddress != 0)
            {
                if (*(byte*)(nativeaddress + 0x33) == 0xEB)
                    return;
                if (!inited)
                    init();
                *(byte*)(nativeaddress + 0x33) = 0xEB; // je to jmp
                FixGetDllImportAttributeDone = true;
            }
            else
            {
                libcs.assert("FixGetDllImportAttribute failed2\nMonoMethod: 0x" + libcs.fprintptr((voidptr)method.mhandle.value));
            }
        }


        public static void AddMonoSIGSEGVHandler()
        {
            if (!inited)
                init();

            vmonoruntime.mono_runtime_install_handlers();

            *(uint*)(0x00E53724 + 0x0) = libcs.swap32(0x60E80600);
            *(uint*)(0x00E53724 + 0x4) = libcs.swap32(0x00006131);
            *(uint*)(0x00E53724 + 0x8) = libcs.swap32(0xC0C3CCCC);

            *(uint*)(0x00E52D2C + 0x0) = libcs.swap32(0xF4090000);

        }

        public static void DontOptimizeDuringTransformMethod()
        {
            if (!inited)
                init();

            // in mono_interp_transform_method(...)
            *(uint*)(0x00E5E0A2 + 0x0) = 0x000665E9;
            *(uint*)(0x00E5E0A2 + 0x4) = 0x7D839000;

            // in TaskControl_GetMethodChecksum(...)
            *(uint*)(0x00D83B77 + 0x0) = 0x5113F669;
        }

        public static void MonoAssertDontCallExitOrAbout()
        {
            if (!inited)
                init();

            byte[] x86_inject = {
            	0x68, 0x3C, 0x1E, 0x09, 0x01, 0xE8, 0x36, 0x09, 0x03, 0x00, 0x50, 0xE8,
	            0xD0, 0xAD, 0x05, 0x00, 0xEB, 0xFE, 0x90, 0x90, 0x90
            };

            // 0x00E46E00 in mono_assert(...)
            for (int i = 0; i < x86_inject.Length; i++)
            {
                *(byte*)(0x00E46E00 + i) = x86_inject[i];
            }
        }
#endif
    }
#endif
}
