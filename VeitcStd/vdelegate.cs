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
    public static class vdelegate
    {
        public static IntPtr GetFunctionPointer(Delegate func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            return func.method_ptr;
        }

        public static MethodInfo GetFunction(Delegate func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            return func.method_info;
        }

        public static object GetTarget(Delegate func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            return func.m_target;
        }

        // TODO: check args
        public static Delegate CreateDelegateStatic<Type>(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (!method.IsStatic)
            {
                throw new ArgumentException("The method is not static", "method");
            }
            return System.Delegate.CreateDelegate_internal(typeof(Type), null, method);
        }

        // TODO: check args
        public static Delegate CreateDelegateStaticNoGeneric(Type monoType, MethodInfo method)
        {
            if (method == null || monoType == null)
            {
                throw new ArgumentNullException("method or monoType is null");
            }
            if (!method.IsStatic)
            {
                throw new ArgumentException("The method is Static", "method");
            }
            if (!monoType.IsSubclassOf(typeof(MulticastDelegate) ?? Type.GetType("System.MulticastDelegate, mscorlib", true)))
            {
                throw new ArgumentException("Type is not a subclass of Multicastdelegate");
            }
            return System.Delegate.CreateDelegate_internal(monoType, null, method);
        }

        public static MethodInfo GetInvokeMethod<T>()
        {
            return ((MonoType)typeof(T)).GetMethodsByName("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, false, typeof(T))[0];
        }

        public static MethodInfo GetInvokeMethodcNoGeneric(Type monoType)
        {
            if (monoType == null)
            {
                throw new ArgumentNullException("monoType");
            }
            if (!monoType.IsSubclassOf(typeof(MulticastDelegate) ?? Type.GetType("System.MulticastDelegate, mscorlib", true)))
            {
                throw new ArgumentException("Type is not a subclass of Multicastdelegate");
            }
            return ((MonoType)monoType).GetMethodsByName("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, false, monoType)[0];
        }

        // TODO: check args
        public static Delegate CreateDelegateObject<Type>(MethodInfo method, object obj)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method is null");
            }
            if (obj == null)
            {
                throw new ArgumentNullException("obj is null");
            }
            if (method.IsStatic)
            {
                throw new ArgumentException("The method is Static", "method");
            }
            return System.Delegate.CreateDelegate_internal(typeof(Type), obj, method);
        }

        // ---------------------------------------Call*Function----------------------------------------- //
        
        public unsafe static void CallvFunction(vfunction func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                 libcs.assert
                     ("CallvFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return;
            }
            else
            {
                vmethod.InvokeA(func.method_info, func.m_target, null);
            }
        }

        public unsafe static bool CallbFunction(bfunction func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallbFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return false;
            }
            else
            {
                return (bool)vmethod.InvokeA(func.method_info, func.m_target, null);
            }
        }

        public unsafe static TR CallgFunction<TR>(fgeneric<TR> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, null);
            }
        }

        public unsafe static TR CallgFunction<TR, T1>(fgeneric<TR, T1> func, T1 param1)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1 });
            }
        }

        public unsafe static TR CallgFunction<TR, T1, T2>(fgeneric<TR, T1, T2> func, T1 param1, T2 param2)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2 });
            }
        }

        public unsafe static TR CallgFunction<TR, T1, T2, T3>(fgeneric<TR, T1, T2, T3> func, T1 param1, T2 param2, T3 param3)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2, param3 });
            }
        }

        public unsafe static TR CallgFunction<TR, T1, T2, T3, T4>(fgeneric<TR, T1, T2, T3, T4> func, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2, param3, param4 });
            }
        }

        public unsafe static TR CallgFunction<TR, T1, T2, T3, T4, T5>(fgeneric<TR, T1, T2, T3, T4, T5> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2, param3, param4, param5 });
            }
        }
        public unsafe static TR CallgFunction<TR, T1, T2, T3, T4, T5, T6>(fgeneric<TR, T1, T2, T3, T4, T5, T6> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2, param3, param4, param5, param6 });
            }
        }
        public unsafe static TR CallgFunction<TR, T1, T2, T3, T4, T5, T6, T7>(fgeneric<TR, T1, T2, T3, T4, T5, T6, T7> func, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func is null");
            }
            if (func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null))
            {
                libcs.assert
                    ("CallgFunction: func.method_info == null || func.method_ptr.value == null || (!func.Method.IsStatic && func.Target == null)");
                return default(TR);
            }
            else
            {
                return (TR)vmethod.InvokeA(func.method_info, func.m_target, new object[] { param1, param2, param3, param4, param5, param6, param7 });
            }
        }

        public delegate void vfunction();
        public delegate bool bfunction();

        public delegate TR fgeneric<TR>();
        public delegate TR fgeneric<TR, T1>(T1 param1);
        public delegate TR fgeneric<TR, T1, T2>(T1 param1, T2 param2);
        public delegate TR fgeneric<TR, T1, T2, T3>(T1 param1, T2 param2, T3 param3);
        public delegate TR fgeneric<TR, T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
        public delegate TR fgeneric<TR, T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
        public delegate TR fgeneric<TR, T1, T2, T3, T4, T5, T6>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);
        public delegate TR fgeneric<TR, T1, T2, T3, T4, T5, T6, T7>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);
    }
}
