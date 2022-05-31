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
    public static class vmethod
    {
        public static IntPtr GetNativeMonoMethod(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            return ((MonoMethod)method).mhandle;
        }

        public static MethodAttributes GetAttributes(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            var _this = (MonoMethod)method;
            {
                MonoMethodInfo info;
                MonoMethodInfo.get_method_info(_this.mhandle, out info);
                return info.attrs;
            }
        }

        public static Type GetDeclaringType(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            MonoMethod _this = (MonoMethod)method;
            {
                MonoMethodInfo info;
                MonoMethodInfo.get_method_info(_this.mhandle, out info);
                return info.parent;
            }
        }

        public static bool IsVirtual(MethodInfo method)
        {
            return (GetAttributes(method) & MethodAttributes.Virtual) != 0;
        }

        public static bool IsStatic(MethodInfo method)
        {
            return (GetAttributes(method) & MethodAttributes.Static) != 0;
        }

        public static ParameterInfo[] GetParameters(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            MonoMethod _this = (MonoMethod)method;
            return MonoMethodInfo.get_parameter_info(_this.mhandle);
        }

        public static MethodBase GetGoodMethods(Type monoType, Type[] types, string name, BindingFlags bindingAttr)
        {
            if (monoType == null)
                throw new ArgumentNullException("monoType");
            if (name == null)
                throw new ArgumentNullException("name");
            int count;
            if (types == null)
            {
                count = 0;
                types = varray<Type>.EmptyArray;
            }
            else
            {
                count = types.Length;
                for (int i = 0; i < count; i++)
                {
                    if (types[i] == null)
                    {
                        throw new ArgumentNullException("types[" + i + "] is null");
                    }
                }
            }

            var methods = ((MonoType)monoType).GetMethodsByName(name, bindingAttr, false, monoType);
            if (methods != null)
            {
                foreach (var methodBase in methods)
                {
                    if (methodBase == null) // EAMono bug?
                    {
                        continue;
                    }

                    var parameters =
#if USE_VMONOMETHOD
                    GetParameters(methodBase);
#else
                    methodBase.GetParameters();
#endif
                    if (parameters != null && parameters.Length == count && System.Reflection.Binder.Default.check_arguments(types, parameters))
                    {
                        return methodBase;
                    }
                }
            }
            return null;
        }

        public unsafe static IntPtr GetRuntimeMethod(MethodInfo method)
        {
#if !SIMS3VERSION_1672
            throw new NotSupportedException("This game version not supported.");
#else
            var ptr = GetNativeMonoMethod(method);
            if (ptr.value == null)
            {
                return default(IntPtr);
            }

            return libcs.tointptr((*(voidptr*)((voidptr)ptr.value + 0x14)));
#endif
        }

        [Obsolete]
        public unsafe static IntPtr GetNativeWrapperMethod(MethodInfo method)
        {
#if !SIMS3VERSION_1672
            throw new NotSupportedException("This game version not supported.");
#else
            var ptr = GetNativeMonoMethod(method);
            if (ptr.value == null)
            {
                return default(IntPtr);
            }
            var t = *(voidptr*)((voidptr)ptr.value + 0xC);
            if (t == 0)
            {
                return default(IntPtr);
            }
            return libcs.tointptr(((voidptr)t + 0x40));
#endif
        }

        public unsafe static voidptr GetNativeFunctionAdderss(IntPtr monoMethod)
        {
#if SIMS3VERSION_1672 && !VEITCSTD_64BIT_MODE
            if (monoMethod.value == null)
            {
                throw new ArgumentNullException("monoMethod");
            }
            return *(voidptr*)(((voidptr)monoMethod.value) + 0x20);
#else
            return 0;
#endif // SIMS3VERSION_1672
        }

        public unsafe static void LoadNativeAddress(MethodBase method, IntPtr threadMonoContext)
        {
            voidptr nativeadderss;
            LoadNativeAddress(method, threadMonoContext, null, out nativeadderss);
        }

        public static object InvokeA(MethodInfo monoMethod, object obj, object[] parameters)
        {
            return InvokeC(monoMethod, obj, BindingFlags.Default, null, parameters, null, false);
        }

        public static object InvokeB(MethodInfo monoMethod, object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
        {
            return InvokeC(monoMethod, obj, BindingFlags.Default, null, parameters, null, false);
        }

        public static object InvokeC(MethodInfo monoMethod, object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture, bool dontThrow)
        {
            if (monoMethod == null)
                throw new ArgumentNullException("monoMethod");
            if (monoMethod.ContainsGenericParameters)
                throw new InvalidOperationException
                    ("Late bound operations cannot be performed on types or methods for which ContainsGenericParameters is true.");

            if (binder == null)
            {
                if (Binder_Default is Binder)
                {
                    binder = (Binder)Binder_Default;
                }
                else
                {
                    var t = new vobject._object_offset4();
                    if (vobject.SetType<Binder.Default>(t))
                    {
                        binder = (Binder)(object)t;
                    }
                    else binder = new Binder.Default();

                    Binder_Default = binder;
                }
            }

            if (!vbinder.ConvertArgs(binder, parameters, GetParameters(monoMethod), culture))
            {
                throw new ArgumentException("parameters");
            }

            if (obj == null && !IsStatic(monoMethod))
                throw new ArgumentNullException("obj");

            try
            {
                return ((MonoMethod)monoMethod).InternalInvoke(obj, parameters);
            }
            catch (InvalidOperationException)
            {
                if (dontThrow)
                    return null;
                throw;
            }
            catch (TargetException)
            {
                throw;
            }
            catch (Exception inner)
            {
                if (dontThrow)
                    return null;
                throw new TargetInvocationException(inner);
            }
        }

        public unsafe static void LoadNativeAddress(MethodBase method, IntPtr threadMonoContext, string methodName, out voidptr nativeadderss)
        {
            if (method == null)
                throw new ArgumentNullException("method");
#if SIMS3VERSION_1672 && ENABLE_NATIVE_INJECT
            nativeadderss = 0;
            // Required 
            vnativeinject.AddMonoSIGSEGVHandler();
            vnativeinject.MonoAssertDontCallExitOrAbout();
            //

            IntPtr monoMethod = default(IntPtr);
            IntPtr monoMethodOrg = default(IntPtr);

            try
            {

                bool dontagian = false;

                if (method is MonoCMethod)
                {
                    monoMethod = vcmethod.GetNativeMonoMethod((MonoCMethod)method);
                }
                else if (method is MonoMethod)
                {
                    monoMethod = vmethod.GetNativeMonoMethod((MonoMethod)method);
                }
                else { return; }
                monoMethodOrg = monoMethod;
            agian:
                var runtime_method_ptr = vmonoruntime.mono_interp_get_runtime_method(monoMethod);
                bool r = vmonoruntime.MonoRuntimeMethod_IsInitialized(runtime_method_ptr);
                if (!dontagian && r)
                {
                    try
                    {
                        monoMethod = vmonoruntime.mono_marshal_get_native_wrapper(monoMethod);
                        if (monoMethod.value != null)
                        {
                            //return;
                        }
                    }
                    catch (Exception)
                    { }
                }


                if (runtime_method_ptr.value != null && !r)
                {
                    Exception ex = null;
                    bool dontexit = false;

                    try
                    {
                        ex = vmonoruntime.mono_interp_transform_method(runtime_method_ptr, threadMonoContext);
                    }
                    catch (Exception exe)
                    {
                        if (exe.message != null)
                            dontexit = exe.message.Contains("Null Reference (SIGSEGV)");

                        if (!dontexit)
                        {
                            exe.message += "\nMethod Name: " + methodName ?? method.Name;
                            throw;
                        }
                        else ex = exe;
                    }

                    if (dontexit)
                    {
                        if (ex != null)
                        {
                            ex.trace_ips = null;
                            ex.message = "";
                            ex.source = null;
                            ex.stack_trace = null;
                        }

                        if (!dontagian)
                        {
                            dontagian = true;
                            try
                            {
                                monoMethod = vmonoruntime.mono_marshal_get_native_wrapper(monoMethod);
                                if (monoMethod.value != null)
                                {
                                    ex = null;
                                    goto agian;
                                }
                            }
                            catch (Exception)
                            { }
                        }
                        //return;
                    }

                    if (ex != null)
                    {
                        ex.message += "\nMethod Name: " + methodName ?? method.Name;
                        ex.stack_trace = "(stack_trace null)\n";
                        if (!dontagian)
                        {
                            dontagian = true;
                            try
                            {
                                monoMethod = vmonoruntime.mono_marshal_get_native_wrapper(monoMethod);
                                if (monoMethod.value != null)
                                {
                                    ex = null;
                                    goto agian;
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
            }
            catch (Exception)
            { }
            nativeadderss = vmethod.GetNativeFunctionAdderss(monoMethodOrg);
#else
            nativeadderss = 0;
#endif
        }
        public static MethodInfo GetMethodNameFastI(Type type, string funcName)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (funcName == null || funcName.Length == 0)
            {
                throw new ArgumentException("funcName");
            }
            var methods = ((MonoType)type).GetMethodsByName(funcName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, false, type);
            if (methods == null || methods.Length == 0)
                return null;
            if (methods.Length > 1)
            {
                throw new AmbiguousMatchException();
            }
            return methods[0];
        }
        public static MethodInfo GetMethodNameFastS(Type type, string funcName)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (funcName == null || funcName.Length == 0)
            {
                throw new ArgumentException("funcName");
            }
            var methods = ((MonoType)type).GetMethodsByName(funcName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, false, type);
            if (methods == null || methods.Length == 0)
                return null;
            if (methods.Length > 1)
            {
                throw new AmbiguousMatchException();
            }
            return methods[0];
        }

        public unsafe static bool SetNativeFunctionAdderss(IntPtr monoMethod, IntPtr funcAddress)
        {
#if SIMS3VERSION_1672
            if (monoMethod.value == null)
            {
                return false;
            }

            voidptr func_address = ((voidptr)monoMethod.value) + 0x20u;
            *(voidptr*)func_address = (voidptr)funcAddress.value;

            return true;
#else
            return false;
#endif // SIMS3VERSION_1672
        }

        public static Binder Binder_Default { get; private set; }
    }
}
