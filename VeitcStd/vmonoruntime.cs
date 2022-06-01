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
using System.Runtime.CompilerServices;

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
    public unsafe static class vmonoruntime
    {
        public static IntPtr MonoInterpreterTransformMethod(MethodBase method, bool safe)
        {
            IntPtr monoMethod;

            if (method is MonoCMethod)
            {
                monoMethod = ((MonoCMethod)method).mhandle;
            }
            else if (method is MonoMethod)
            {
                monoMethod = ((MonoMethod)method).mhandle;
            }
            else
            {
                throw new NotSupportedException();
            }

            IntPtr runtimeMethod = mono_interp_get_runtime_method(monoMethod);
            if (runtimeMethod.value != null && !safe && !MonoRuntimeMethod_IsInitialized(runtimeMethod))
            {
                var ex = mono_interp_transform_method(runtimeMethod, vthread.GetMonoThreadContext());
                if (ex != null)
                {
                    ex.message += "\nMethod: " + method.ToString();
                    throw ex;
                }
            }
            return runtimeMethod;
        }

        public static void DeleteTransformMethod(MethodBase method)
        {
            IntPtr monoMethod;

            if (method is MonoCMethod)
            {
                monoMethod = ((MonoCMethod)method).mhandle;
            }
            else if (method is MonoMethod)
            {
                monoMethod = ((MonoMethod)method).mhandle;
            }
            else
            {
                throw new NotSupportedException();
            }

            IntPtr runtimeMethod = mono_interp_get_runtime_method(monoMethod);
            if (!MonoRuntimeMethod_IsInitialized(runtimeMethod))
                return;

            voidptr ptr_a = (voidptr)runtimeMethod.value;

            *(voidptr*)(ptr_a) = 0;
            *(voidptr*)(ptr_a + 0x4) = 0;

            //*(voidptr*)(ptr_a + 0x8) = 0;
            //*(voidptr*)(ptr_a + 0xC) = 0;

            *(voidptr*)(ptr_a + 0x10) = 0;

            *(voidptr*)(ptr_a + 0x14) = 0;
            *(voidptr*)(ptr_a + 0x18) = 0;

            *(voidptr*)(ptr_a + 0x1C) = 0;
            *(voidptr*)(ptr_a + 0x20) = 0;

            *(voidptr*)(ptr_a + 0x24) = 0; 
            *(voidptr*)(ptr_a + 0x28) = 0; 
            *(voidptr*)(ptr_a + 0x2C) = 0;

            *(voidptr*)(ptr_a + 0x30) = 0;
            *(voidptr*)(ptr_a + 0x34) = 0;

            *(voidptr*)(ptr_a + 0x38) = 0;
            *(voidptr*)(ptr_a + 0x3C) = 0;
            *(voidptr*)(ptr_a + 0x40) = 0;
            *(voidptr*)(ptr_a + 0x44) = 0;
        }

        public static IntPtr MonoInterpreterTransformMethodB(MethodBase method, bool safe, IntPtr threadContext)
        {
            IntPtr monoMethod;

            if (method is MonoCMethod)
            {
                monoMethod = ((MonoCMethod)method).mhandle;
            }
            else if (method is MonoMethod)
            {
                monoMethod = ((MonoMethod)method).mhandle;
            }
            else
            {
                throw new NotSupportedException(method.GetType().ToString());
            }

            IntPtr runtimeMethod = mono_interp_get_runtime_method(monoMethod);
            if (runtimeMethod.value != null && !safe && !MonoRuntimeMethod_IsInitialized(runtimeMethod))
            {
                var ex = mono_interp_transform_method(runtimeMethod, threadContext);
                if (ex != null)
                {
                    ex.message += "\nMethod: " + method.Name;
                    throw ex;
                }
            }
            return runtimeMethod;
        }

        public static IntPtr MonoInterpreterTransformMethodPtr(IntPtr monoMethod, string methodName, bool safe)
        {
            IntPtr runtimeMethod = mono_interp_get_runtime_method(monoMethod);
            //MonoRuntimeMethod_IsTransformed(runtimeMethod);
            if (runtimeMethod.value != null && !safe && !MonoRuntimeMethod_IsInitialized(runtimeMethod))
            {
                var ex = mono_interp_transform_method(runtimeMethod, vthread.GetMonoThreadContext());
                if (ex != null)
                {
                    ex.message += "\nMethod: " + methodName;
                    throw ex;
                }
            }
            return runtimeMethod;
        }

        public static bool MonoRuntimeMethod_IsInitialized(IntPtr runtimeMethod)
        {
            if (runtimeMethod.value == null)
                throw new ArgumentNullException("runtimeMethod");
            return (voidptr)(*(voidptr*)runtimeMethod.value) != 0x00000000;
        }

        public static bool MonoRuntimeMethod_IsTransformed(IntPtr runtimeMethod)
        {
            // TODO
            return MonoRuntimeMethod_IsInitialized(runtimeMethod);
        }

        /* EAMono removed check InternalCall or DllImportAttribute

        StackTrace removed (wrapper managed-to-native) why?
        *
        //#0: 0x00000            in System.Reflection.System.Reflection.Assembly:GetTypes (bool) (12345678 [0] )
        //#1: 0x00002 call       in System.Reflection.System.Reflection.Assembly:GetTypes () ()
        //#2: 0x000df callvirt   in NRaas.Common+DerivativeSearch:FindOfType (System.Type,NRaas.Common/DerivativeSearch/Caching,NRaas.Common/DerivativeSearch/Scope) ([12345678] [0] [0] )
        //#3: 0x00010 call       in NRaas.Common+DerivativeSearch:Find (NRaas.Common/DerivativeSearch/Caching,NRaas.Common/DerivativeSearch/Scope) ([0] [0] )
        //#4: 0x00002 call       in NRaas.Common+DerivativeSearch:Find () ()
        //#5: 0x00017 call       in NRaas.NRaas.Common:OnStartupApp (object,System.EventArgs) ([12345678] [12345678] )
        *
       
        public const short METHOD_IMPL_ATTRIBUTE_RUNTIME = 0x0003;
        public const short METHOD_IMPL_ATTRIBUTE_INTERNAL_CALL = 0x1000;
        public const short METHOD_ATTRIBUTE_PINVOKE_IMPL = 0x2000;
        public static bool MonoMethodIsInternalCall(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
        
            var iflags = (short)(*(short*)monoMethod.value + 0x2);
            var flags = (short)(*(short*)monoMethod.value);
        
            return iflags == 0x98 && flags == 0x96;
        }
        
        public static bool MonoMethodIsDllImport(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
        
            var iflags = (short)(*(short*)monoMethod.value + 0x2);
            var flags = (short)(*(short*)monoMethod.value);
        
            return ((iflags == 0x93 || iflags == 0x2093)
                  && (flags == 0x91 || flags == 0x2091));
        }
        
        public static bool MonoMethodIsDllImportOrInternalCall(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
        
            var iflags = (short)(*(short*)monoMethod.value + 0x2);
            var flags = (short)(*(short*)monoMethod.value);
        
            return ((iflags == 0x93 || iflags == 0x2093) && (flags == 0x91 || flags == 0x2091))
                   || (iflags == 0x98 && flags == 0x96);
        }*/


        public static short MonoMethod_GetFlags(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
            return (short)(*(short*)monoMethod.value);
        }

        public static uint MonoMethod_GetFlags2(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
            return (voidptr)(*(voidptr*)monoMethod.value + 0x8);
        }

        public static short MonoMethod_GetFlags3(IntPtr monoMethod)
        {
            if (monoMethod.value == null)
                throw new ArgumentNullException("monoMethod");
            return (short)(*(short*)monoMethod.value + 0x2);
        }

        private static bool mono_interp_get_runtime_methodinited = false;


        /*
       ts3w.mono_interp_get_runtime_method:
         push edi
         mov edi,[esp+8]
         mov eax,[edi+14]
         test eax,eax
         jne ret
         push esi
         call <ts3w.mono_domain_get>
         mov eax,[eax+18]
         push 48  // (RuntimeMethod Size is 0x48)?
         push eax
         call <ts3w.g_malloc>
         push 48
         mov esi,eax
         push 0
         push esi
         call <JMP.&memset>
         push edi
         mov [esi+8],edi
         call <ts3w.mono_method_signature>
         movzx ecx,[eax+4]
         mov [esi+40],cl
         mov edx,[eax]
         shl edx,9
         xor edx,[esi+44]
         add esp,18
         and edx,200
         xor [esi+44],edx
         mov eax,[eax+C]
         movzx ecx,[eax+6]
         mov [esi+44],cl
         mov edx,[edi+8]
         mov eax,[edx+14]
         shl eax,7
         xor eax,[esi+44]
         and eax,400
         xor [esi+44],eax
         mov [edi+14],esi
         mov eax,esi
         pop esi
        ret:
         pop edi
         ret 
        */

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern IntPtr mono_interp_get_runtime_method_internal(IntPtr monoMethod);

        // Return MonoRuntimeMethod*
        public static IntPtr mono_interp_get_runtime_method(IntPtr monoMethod)
        {
            if (!mono_interp_get_runtime_methodinited)
            {
                var m = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vmonoruntime), "mono_interp_get_runtime_method_internal");

                if (m == null)
                {
                    return default(IntPtr);
                }

                if (!vmethod.SetNativeFunctionAdderss(m.mhandle, libcs.tointptr(0x00E52210)))
                    return default(IntPtr);

                mono_interp_get_runtime_methodinited = true;
            }

            if (monoMethod.value == null)
            {
                throw new ArgumentNullException("monoMethod");
            }

            return mono_interp_get_runtime_method_internal(monoMethod);
        }

        /////////////////////////////////////////////////////

        private static bool mono_interp_transform_methodinited = false;

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern Exception mono_interp_transform_method_internal(IntPtr runtime_method, IntPtr thread_context);

        // Return MonoException*
        public static Exception mono_interp_transform_method(IntPtr runtime_method, IntPtr thread_context)
        {
            if (!mono_interp_transform_methodinited)
            {
                var m = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vmonoruntime), "mono_interp_transform_method_internal");

                if (m == null)
                {
                    return new NotSupportedException("GetMethod mono_interp_transform_method_internal failed");
                }

                if (!vmethod.SetNativeFunctionAdderss(m.mhandle, libcs.tointptr(0x00E646A0)))
                    return new NotSupportedException("!vmethod.SetFunctionPointer(m.mhandle, libcs.toIntPointer(0x00E646A0))");

                mono_interp_transform_methodinited = true;
            }

            if (runtime_method.value == null)
            {
                return new ArgumentNullException("runtime_method");
            }

            if (thread_context.value == null)
            {
                return new ArgumentNullException("thread_context");
            }

            if (MonoRuntimeMethod_IsInitialized(runtime_method))
            {
                return new InvalidOperationException("Attempting to transforme target method. This a mono method have already transformed.");
            }

            return mono_interp_transform_method_internal(runtime_method, thread_context);

        }

        /////////////////////////////////////////////////////

        private static bool mono_arch_create_trampolineinited = false;

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern IntPtr mono_arch_create_trampoline_internal(IntPtr monoMethodSignature);

        // Return MonoPIFunc
        [Obsolete("TODO")]
        public static IntPtr mono_arch_create_trampoline(IntPtr monoMethodSignature) // TODO
        {
            if (!mono_arch_create_trampolineinited)
            {
                var m = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vmonoruntime), "mono_arch_create_trampoline_internal");

                if (m == null)
                {
                    return default(IntPtr);
                }

                if (!vmethod.SetNativeFunctionAdderss(m.mhandle, libcs.tointptr(0x00E516F0)))
                    return default(IntPtr);

                mono_arch_create_trampolineinited = true;
            }

            if (monoMethodSignature.value == null)
            {
                throw new ArgumentNullException("monoMethodSignature");
            }


            return mono_arch_create_trampoline_internal(monoMethodSignature);
        }

        /////////////////////////////////////////////////////


        private static bool mono_method_signatureinited = false;

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern IntPtr mono_method_signature_internal(IntPtr monoMethod);

        // Return MonoMethodSignature*
        public static IntPtr mono_method_signature(IntPtr monoMethod)
        {
            if (!mono_method_signatureinited)
            {
                var m = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vmonoruntime), "mono_method_signature_internal");

                if (m == null)
                {
                    return default(IntPtr);
                }

               if (!vmethod.SetNativeFunctionAdderss(m.mhandle, libcs.tointptr(0x00E85330)))
                    return default(IntPtr);

                mono_method_signatureinited = true;
            }

            if (monoMethod.value == null)
            {
                throw new ArgumentNullException("monoMethod");
            }

            return mono_method_signature_internal(monoMethod);
        }

        ///////////////////////////////////////////////////

        // mono_marshal_get_native_wrapper

        private static bool mono_marshal_get_native_wrapperinited = false;

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern IntPtr mono_marshal_get_native_wrapper_internal(IntPtr monoMethod);

        // Return MonoMethod*
        public static IntPtr mono_marshal_get_native_wrapper(IntPtr monoMethod)
        {
            if (!mono_marshal_get_native_wrapperinited)
            {
                var m = (MonoMethod)vmethod.GetMethodNameFastS(typeof(vmonoruntime), "mono_marshal_get_native_wrapper_internal");

                if (m == null)
                {
                    return default(IntPtr);
                }

                if (!vmethod.SetNativeFunctionAdderss(m.mhandle, libcs.tointptr(0x00E994C0)))
                    return default(IntPtr);

                mono_marshal_get_native_wrapperinited = true;
            }

            if (monoMethod.value == null)
            {
                throw new ArgumentNullException("monoMethod");
            }

            return mono_marshal_get_native_wrapper_internal(monoMethod);
        }

        public static void mono_runtime_install_handlers()
        {
            global::Mono.Runtime.mono_runtime_install_handlers();
        }
    }
#endif
}
