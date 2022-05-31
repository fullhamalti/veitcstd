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
using System.Collections;

namespace veitcstd
{
    public static class vcustomattributes
    {
        // EAMono removed check InternalCall or DllImportAttribute
        public static bool force_if_pinvoke_impl = false;

        static ICustomAttributeProvider GetBase(ICustomAttributeProvider obj)
        {
            if (obj == null)
                return null;

            if (obj is Type)
                return ((Type)obj).BaseType;

            MethodInfo method = null;
            if (obj is MonoProperty)
            {
                MonoProperty prop = (MonoProperty)obj;
                method = prop.GetGetMethod(true);
                if (method == null)
                    method = prop.GetSetMethod(true);
            }
            else if (obj is MonoMethod)
            {
                method = (MethodInfo)obj;
            }

            /**
             * ParameterInfo -> null
             * Assembly -> null
             * MonoEvent -> null
             * MonoField -> null
             */
            if (method == null || !vmethod.IsVirtual(method))
                return null;

            MethodInfo baseMethod = method.GetBaseDefinition();
            if (baseMethod == method)
                return null;

            return baseMethod;
        }

        // EAMono does not exists System.Reflection.MonoMethod::GetPseudoCustomAttributes()
        public static object[] GetMPseudoCustomAttributes(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            int count = 0;
            MonoMethod _this = (MonoMethod)method;
            /* MS.NET doesn't report MethodImplAttribute */

            MonoMethodInfo info;
            MonoMethodInfo.get_method_info(_this.mhandle, out info);
            if ((info.iattrs & MethodImplAttributes.PreserveSig) != 0)
                count++;
            if (force_if_pinvoke_impl || (info.attrs & MethodAttributes.PinvokeImpl) != 0)
                count++;

            if (count == 0)
                return null;
            object[] attrs = new object[count];
            count = 0;

            if ((info.iattrs & MethodImplAttributes.PreserveSig) != 0)
                attrs[count++] = new System.Runtime.InteropServices.PreserveSigAttribute();
            if (force_if_pinvoke_impl || (info.attrs & MethodAttributes.PinvokeImpl) != 0)
            {
#if ENABLE_NATIVE_INJECT && SIMS3VERSION_1672
                vnativeinject.FixGetDllImportAttribute();
#endif
                System.Runtime.InteropServices.DllImportAttribute attr = MonoMethod.GetDllImportAttribute(_this.mhandle);
                if ((info.iattrs & MethodImplAttributes.PreserveSig) != 0)
                    attr.PreserveSig = true;
                attrs[count++] = attr;
            }

            return attrs;
        }

        // EAMono does not exists System.Reflection.FieldInfo::GetPseudoCustomAttributes()
        public static object[] GetFPseudoCustomAttributes(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            int count = 0;

            if (field.IsNotSerialized)
                count++;

            if (field.DeclaringType.IsExplicitLayout)
                count++;

            // EAMono does not exists
            //UnmanagedMarshal marshalAs = field.UMarshal
            //if (marshalAs != null)
            //    count++;

            if (count == 0)
                return null;
            object[] attrs = new object[count];
            count = 0;

            if (field.IsNotSerialized)
                attrs[count++] = new NonSerializedAttribute();
            if (field.DeclaringType.IsExplicitLayout)
                attrs[count++] = new System.Runtime.InteropServices.FieldOffsetAttribute(field.GetFieldOffset());

            return attrs;
        }

        // EAMono does not exists System.Reflection.ParameterInfo::GetPseudoCustomAttributes()
        public static object[] GetPPseudoCustomAttributes(ParameterInfo p)
        {
            if (p == null)
                throw new ArgumentNullException("p");
            int count = 0;

            if (p.IsIn)
                count++;
            if (p.IsOut)
                count++;
            if (p.IsOptional)
                count++;

            // EAMono does not exists 
            //if (p.marshalAs != null)
            //    count++;

            if (count == 0)
                return null;
            object[] attrs = new object[count];
            count = 0;

            if (p.IsIn)
                attrs[count++] = new System.Runtime.InteropServices.InAttribute();
            if (p.IsOptional)
                attrs[count++] = new System.Runtime.InteropServices.OptionalAttribute();
            if (p.IsOut)
                attrs[count++] = new System.Runtime.InteropServices.OutAttribute();

            // EAMono does not exists 
            //if (p.marshalAs != null)
            //    attrs[count++] = p.marshalAs.ToMarshalAsAttribute();

            return attrs;
        }

        // EAMono does not exists System.Type::GetPseudoCustomAttributes()
        public static object[] GetTPseudoCustomAttributes(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            int count = 0;

            /* IsSerializable returns true for delegates/enums as well */
            if ((vtype.GetTypeAttributeFlags(type) & TypeAttributes.Serializable) != 0)
                count++;
            if ((vtype.GetTypeAttributeFlags(type) & TypeAttributes.Import) != 0)
                count++;

            if (count == 0)
                return null;
            object[] attrs = new object[count];
            count = 0;

            if ((vtype.GetTypeAttributeFlags(type) & TypeAttributes.Serializable) != 0)
                attrs[count++] = new SerializableAttribute();
            if ((vtype.GetTypeAttributeFlags(type) & TypeAttributes.Import) != 0)
                attrs[count++] = new System.Runtime.InteropServices.ComImportAttribute();

            return attrs;
        }

        // EAMono does not exists System.MonoCustomAttrs::GetPseudoCustomAttributes(ICustomAttributeProvider obj, Type attributeType)
        public static object[] GetPseudoCustomAttributes(ICustomAttributeProvider obj, Type attributeType)
        {
#if NET_2_0 || BOOTSTRAP_NET_2_0
			object[] pseudoAttrs = null;

			/* FIXME: Add other types */
			if (obj is MonoMethod)
                pseudoAttrs = GetMPseudoCustomAttributes ((MonoMethod)obj);
			else if (obj is FieldInfo)
                pseudoAttrs = GetFPseudoCustomAttributes ((FieldInfo)obj);
			else if (obj is ParameterInfo)
				pseudoAttrs = GetPPseudoCustomAttributes ((ParameterInfo) obj);
			else if (obj is Type)
				pseudoAttrs = GetTPseudoCustomAttributes ((Type)obj);

			if ((attributeType != null) && (pseudoAttrs != null)) {
				for (int i = 0; i < pseudoAttrs.Length; ++i)
					if (attributeType.IsAssignableFrom (pseudoAttrs [i].GetType ()))
						if (pseudoAttrs.Length == 1)
							return pseudoAttrs;
						else
							return new object [] { pseudoAttrs [i] };
				return new object [0];
			}
			else
				return pseudoAttrs;
#else
            return null;
#endif
        }

        public static object[] GetCustomAttributesBase(ICustomAttributeProvider obj, Type attributeType)
        {
            object[] attrs = MonoCustomAttrs.GetCustomAttributesInternal(obj, attributeType, false);

            object[] pseudoAttrs = GetPseudoCustomAttributes(obj, attributeType);
            if (pseudoAttrs != null)
            {
                object[] res = new object[attrs.Length + pseudoAttrs.Length];
                System.Array.Copy(attrs, res, attrs.Length);
                System.Array.Copy(pseudoAttrs, 0, res, attrs.Length, pseudoAttrs.Length);
                return res;
            }
            else
                return attrs;
        }

        public static object[] GetCustomAttributes(ICustomAttributeProvider obj, Type attributeType, bool inherit)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            object[] r;
            object[] res = GetCustomAttributesBase(obj, attributeType);
            // shortcut
            if (!inherit && res.Length == 1)
            {
                if (attributeType != null)
                {
                    if (attributeType.IsAssignableFrom(res[0].GetType()))
                    {
                        r = (object[])Array.CreateInstance(attributeType, 1);
                        r[0] = res[0];
                    }
                    else
                    {
                        r = (object[])Array.CreateInstance(attributeType, 0);
                    }
                }
                else
                {
                    r = (object[])Array.CreateInstance(res[0].GetType(), 1);
                    r[0] = res[0];
                }
                return r;
            }

            // if AttributeType is sealed, and Inherited is set to false, then 
            // there's no use in scanning base types 
            if ((attributeType != null && attributeType.IsSealed) && inherit)
            {
                AttributeUsageAttribute usageAttribute = RetrieveAttributeUsage(
                    attributeType);
                if (!usageAttribute.Inherited)
                {
                    inherit = false;
                }
            }

            int initialSize = res.Length < 16 ? res.Length : 16;

            Hashtable attributeInfos = new Hashtable(initialSize);
            ArrayList a = new ArrayList(initialSize);
            ICustomAttributeProvider btype = obj;

            int inheritanceLevel = 0;

            do
            {
                foreach (object attr in res)
                {
                    AttributeUsageAttribute usage;

                    Type attrType = attr.GetType();
                    if (attributeType != null)
                    {
                        if (!attributeType.IsAssignableFrom(attrType))
                        {
                            continue;
                        }
                    }

                    var firstAttribute = (System.MonoCustomAttrs.AttributeInfo)attributeInfos[attrType];
                    if (firstAttribute != null)
                    {
                        usage = firstAttribute.Usage;
                    }
                    else
                    {
                        usage = RetrieveAttributeUsage(attrType);
                    }

                    // only add attribute to the list of attributes if 
                    // - we are on the first inheritance level, or the attribute can be inherited anyway
                    // and (
                    // - multiple attributes of the type are allowed
                    // or (
                    // - this is the first attribute we've discovered
                    // or
                    // - the attribute is on same inheritance level than the first 
                    //   attribute that was discovered for this attribute type ))
                    if ((inheritanceLevel == 0 || usage.Inherited) && (usage.AllowMultiple ||
                        (firstAttribute == null || (firstAttribute != null
                            && firstAttribute.InheritanceLevel == inheritanceLevel))))
                    {
                        a.Add(attr);
                    }

                    if (firstAttribute == null)
                    {
                        attributeInfos.Add(attrType, new System.MonoCustomAttrs.AttributeInfo(usage, inheritanceLevel));
                    }
                }

                if ((btype = GetBase(btype)) != null)
                {
                    inheritanceLevel++;
                    res = GetCustomAttributesBase(btype, attributeType);
                }
            } while (inherit && btype != null);

            object[] array = null;
            if (attributeType == null || attributeType.IsValueType)
            {
                array = (object[])Array.CreateInstance(typeof(Attribute), a.Count);
            }
            else
            {
                array = Array.CreateInstance(attributeType, a.Count) as object[];
            }

            // copy attributes to array
            a.CopyTo(array, 0);

            return array;
        }

        public static AttributeUsageAttribute RetrieveAttributeUsage(Type attributeType)
        {
            if (attributeType == typeof(AttributeUsageAttribute))
                /* Avoid endless recursion */
                return new AttributeUsageAttribute(AttributeTargets.Class);

            AttributeUsageAttribute usageAttribute = null;
            object[] attribs = GetCustomAttributes(attributeType,
                MonoCustomAttrs.AttributeUsageType, false);
            if (attribs.Length == 0)
            {
                // if no AttributeUsage was defined on the attribute level, then
                // try to retrieve if from its base type
                if (attributeType.BaseType != null)
                {
                    usageAttribute = RetrieveAttributeUsage(attributeType.BaseType);

                }
                if (usageAttribute != null)
                {
                    // return AttributeUsage of base class
                    return usageAttribute;

                }
                // return default AttributeUsageAttribute if no AttributeUsage 
                // was defined on attribute, or its base class
                return MonoCustomAttrs.DefaultAttributeUsage;
            }
            // check if more than one AttributeUsageAttribute has been specified 
            // on the type
            // NOTE: compilers should prevent this, but that doesn't prevent
            // anyone from using IL ofcourse
            if (attribs.Length > 1)
            {
                //throw new FormatException("Duplicate AttributeUsageAttribute cannot be specified on an attribute type.");
            }

            return ((AttributeUsageAttribute)attribs[0]);
        }
    }
}
