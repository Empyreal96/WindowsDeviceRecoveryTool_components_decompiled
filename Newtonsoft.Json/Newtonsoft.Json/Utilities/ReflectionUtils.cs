using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000FF RID: 255
	internal static class ReflectionUtils
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002FF10 File Offset: 0x0002E110
		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo methodInfo = propertyInfo.GetGetMethod();
			if (methodInfo != null && methodInfo.IsVirtual)
			{
				return true;
			}
			methodInfo = propertyInfo.GetSetMethod();
			return methodInfo != null && methodInfo.IsVirtual;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002FF60 File Offset: 0x0002E160
		public static MethodInfo GetBaseDefinition(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo methodInfo = propertyInfo.GetGetMethod();
			if (methodInfo != null)
			{
				return methodInfo.GetBaseDefinition();
			}
			methodInfo = propertyInfo.GetSetMethod();
			if (methodInfo != null)
			{
				return methodInfo.GetBaseDefinition();
			}
			return null;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002FFA7 File Offset: 0x0002E1A7
		public static bool IsPublic(PropertyInfo property)
		{
			return (property.GetGetMethod() != null && property.GetGetMethod().IsPublic) || (property.GetSetMethod() != null && property.GetSetMethod().IsPublic);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002FFE4 File Offset: 0x0002E1E4
		public static Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
		{
			string text2;
			if (binder != null)
			{
				string text;
				string str;
				binder.BindToName(t, out text, out str);
				text2 = str + ((text == null) ? "" : (", " + text));
			}
			else
			{
				text2 = t.AssemblyQualifiedName;
			}
			switch (assemblyFormat)
			{
			case FormatterAssemblyStyle.Simple:
				return ReflectionUtils.RemoveAssemblyDetails(text2);
			case FormatterAssemblyStyle.Full:
				return text2;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00030054 File Offset: 0x0002E254
		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			foreach (char c in fullyQualifiedTypeName)
			{
				char c2 = c;
				if (c2 != ',')
				{
					switch (c2)
					{
					case '[':
						flag = false;
						flag2 = false;
						stringBuilder.Append(c);
						goto IL_77;
					case ']':
						flag = false;
						flag2 = false;
						stringBuilder.Append(c);
						goto IL_77;
					}
					if (!flag2)
					{
						stringBuilder.Append(c);
					}
				}
				else if (!flag)
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else
				{
					flag2 = true;
				}
				IL_77:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000300EB File Offset: 0x0002E2EB
		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsValueType() || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003010F File Offset: 0x0002E30F
		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00030128 File Offset: 0x0002E328
		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return t.GetConstructors(bindingFlags).SingleOrDefault((ConstructorInfo c) => !c.GetParameters().Any<ParameterInfo>());
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00030169 File Offset: 0x0002E369
		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsValueType() || ReflectionUtils.IsNullableType(t);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00030186 File Offset: 0x0002E386
		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsGenericType() && t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000301B2 File Offset: 0x0002E3B2
		public static Type EnsureNotNullableType(Type t)
		{
			if (!ReflectionUtils.IsNullableType(t))
			{
				return t;
			}
			return Nullable.GetUnderlyingType(t);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000301C4 File Offset: 0x0002E3C4
		public static bool IsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			if (!type.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == genericInterfaceDefinition;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000301EC File Offset: 0x0002E3EC
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type2;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00030204 File Offset: 0x0002E404
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface() || !genericInterfaceDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, genericInterfaceDefinition));
			}
			if (type.IsInterface() && type.IsGenericType())
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.IsGenericType())
				{
					Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000302B8 File Offset: 0x0002E4B8
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type2;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000302D0 File Offset: 0x0002E4D0
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass() || !genericClassDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, genericClassDefinition));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00030324 File Offset: 0x0002E524
		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			if (currentType.IsGenericType())
			{
				Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.BaseType() == null)
			{
				implementingType = null;
				return false;
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(currentType.BaseType(), genericClassDefinition, out implementingType);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00030370 File Offset: 0x0002E570
		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable<>), out type2))
			{
				if (type2.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				return type2.GetGenericArguments()[0];
			}
			else
			{
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					return null;
				}
				throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000303FC File Offset: 0x0002E5FC
		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			Type type;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<, >), out type))
			{
				if (type.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				Type[] genericArguments = type.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
				return;
			}
			else
			{
				if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					keyType = null;
					valueType = null;
					return;
				}
				throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00030488 File Offset: 0x0002E688
		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberTypes = member.MemberType();
			switch (memberTypes)
			{
			case MemberTypes.Event:
				return ((EventInfo)member).EventHandlerType;
			case MemberTypes.Constructor | MemberTypes.Event:
				break;
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			default:
				if (memberTypes == MemberTypes.Method)
				{
					return ((MethodInfo)member).ReturnType;
				}
				if (memberTypes == MemberTypes.Property)
				{
					return ((PropertyInfo)member).PropertyType;
				}
				break;
			}
			throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo, EventInfo or MethodInfo", "member");
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00030508 File Offset: 0x0002E708
		public static bool IsIndexedProperty(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			PropertyInfo propertyInfo = member as PropertyInfo;
			return propertyInfo != null && ReflectionUtils.IsIndexedProperty(propertyInfo);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00030538 File Offset: 0x0002E738
		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00030550 File Offset: 0x0002E750
		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes != MemberTypes.Field)
			{
				if (memberTypes == MemberTypes.Property)
				{
					try
					{
						return ((PropertyInfo)member).GetValue(target, null);
					}
					catch (TargetParameterCountException innerException)
					{
						throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, member.Name), innerException);
					}
				}
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture, member.Name), "member");
			}
			return ((FieldInfo)member).GetValue(target);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000305F8 File Offset: 0x0002E7F8
		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				((FieldInfo)member).SetValue(target, value);
				return;
			}
			if (memberTypes != MemberTypes.Property)
			{
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
			((PropertyInfo)member).SetValue(target, value, null);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0003066C File Offset: 0x0002E86C
		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return nonPublic || fieldInfo.IsPublic;
			}
			if (memberTypes != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanRead && (nonPublic || propertyInfo.GetGetMethod(nonPublic) != null);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000306C8 File Offset: 0x0002E8C8
		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			MemberTypes memberTypes = member.MemberType();
			if (memberTypes == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				return !fieldInfo.IsLiteral && (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
			}
			if (memberTypes != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)member;
			return propertyInfo.CanWrite && (nonPublic || propertyInfo.GetSetMethod(nonPublic) != null);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00030744 File Offset: 0x0002E944
		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(ReflectionUtils.GetFields(type, bindingAttr));
			list.AddRange(ReflectionUtils.GetProperties(type, bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.Count);
			foreach (IGrouping<string, MemberInfo> source in from m in list
			group m by m.Name)
			{
				int num = source.Count<MemberInfo>();
				IList<MemberInfo> list3 = source.ToList<MemberInfo>();
				if (num == 1)
				{
					list2.Add(list3.First<MemberInfo>());
				}
				else
				{
					IList<MemberInfo> list4 = new List<MemberInfo>();
					foreach (MemberInfo memberInfo in list3)
					{
						if (list4.Count == 0)
						{
							list4.Add(memberInfo);
						}
						else if (!ReflectionUtils.IsOverridenGenericMember(memberInfo, bindingAttr) || memberInfo.Name == "Item")
						{
							list4.Add(memberInfo);
						}
					}
					list2.AddRange(list4);
				}
			}
			return list2;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00030884 File Offset: 0x0002EA84
		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType() != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			if (!propertyInfo.IsVirtual())
			{
				return false;
			}
			Type declaringType = propertyInfo.DeclaringType;
			if (!declaringType.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(propertyInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member[0]);
			return memberUnderlyingType.IsGenericParameter;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x000308FC File Offset: 0x0002EAFC
		public static T GetAttribute<T>(object attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00030908 File Offset: 0x0002EB08
		public static T GetAttribute<T>(object attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			if (attributes == null)
			{
				return default(T);
			}
			return attributes.FirstOrDefault<T>();
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00030930 File Offset: 0x0002EB30
		public static T[] GetAttributes<T>(object attributeProvider, bool inherit) where T : Attribute
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(attributeProvider, typeof(T), inherit);
			T[] array = attributes as T[];
			if (array != null)
			{
				return array;
			}
			return attributes.Cast<T>().ToArray<T>();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00030968 File Offset: 0x0002EB68
		public static Attribute[] GetAttributes(object attributeProvider, Type attributeType, bool inherit)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			if (attributeProvider is Type)
			{
				Type type = (Type)attributeProvider;
				object[] source = (attributeType != null) ? type.GetCustomAttributes(attributeType, inherit) : type.GetCustomAttributes(inherit);
				return source.Cast<Attribute>().ToArray<Attribute>();
			}
			if (attributeProvider is Assembly)
			{
				Assembly element = (Assembly)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element);
				}
				return Attribute.GetCustomAttributes(element, attributeType);
			}
			else if (attributeProvider is MemberInfo)
			{
				MemberInfo element2 = (MemberInfo)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element2, inherit);
				}
				return Attribute.GetCustomAttributes(element2, attributeType, inherit);
			}
			else if (attributeProvider is Module)
			{
				Module element3 = (Module)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element3, inherit);
				}
				return Attribute.GetCustomAttributes(element3, attributeType, inherit);
			}
			else
			{
				if (!(attributeProvider is ParameterInfo))
				{
					ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
					object[] array = (attributeType != null) ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit);
					return (Attribute[])array;
				}
				ParameterInfo element4 = (ParameterInfo)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element4, inherit);
				}
				return Attribute.GetCustomAttributes(element4, attributeType, inherit);
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00030A98 File Offset: 0x0002EC98
		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex != null)
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
				return;
			}
			typeName = fullyQualifiedTypeName;
			assemblyName = null;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00030AF8 File Offset: 0x0002ECF8
		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char c = fullyQualifiedTypeName[i];
				char c2 = c;
				if (c2 != ',')
				{
					switch (c2)
					{
					case '[':
						num++;
						break;
					case ']':
						num--;
						break;
					}
				}
				else if (num == 0)
				{
					return new int?(i);
				}
			}
			return null;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00030B68 File Offset: 0x0002ED68
		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			MemberTypes memberTypes = memberInfo.MemberType();
			if (memberTypes == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				Type[] types = (from p in propertyInfo.GetIndexParameters()
				select p.ParameterType).ToArray<Type>();
				return targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, propertyInfo.PropertyType, types, null);
			}
			return targetType.GetMember(memberInfo.Name, memberInfo.MemberType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault<MemberInfo>();
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00030BE8 File Offset: 0x0002EDE8
		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			ReflectionUtils.GetChildPrivateFields(list, targetType, bindingAttr);
			return list.Cast<FieldInfo>();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00030C24 File Offset: 0x0002EE24
		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(BindingFlags.Public);
				while ((targetType = targetType.BaseType()) != null)
				{
					IEnumerable<MemberInfo> collection = (from f in targetType.GetFields(bindingAttr2)
					where f.IsPrivate
					select f).Cast<MemberInfo>();
					initialFields.AddRange(collection);
				}
			}
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00030C8C File Offset: 0x0002EE8C
		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> list = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			ReflectionUtils.GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int i = 0; i < list.Count; i++)
			{
				PropertyInfo propertyInfo = list[i];
				if (propertyInfo.DeclaringType != targetType)
				{
					PropertyInfo value = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(propertyInfo.DeclaringType, propertyInfo);
					list[i] = value;
				}
			}
			return list;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00030CFB File Offset: 0x0002EEFB
		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00030DB4 File Offset: 0x0002EFB4
		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			while ((targetType = targetType.BaseType()) != null)
			{
				PropertyInfo[] properties = targetType.GetProperties(bindingAttr);
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo subTypeProperty2 = properties[i];
					PropertyInfo subTypeProperty = subTypeProperty2;
					if (!ReflectionUtils.IsPublic(subTypeProperty))
					{
						int num = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name);
						if (num == -1)
						{
							initialProperties.Add(subTypeProperty);
						}
						else
						{
							PropertyInfo property = initialProperties[num];
							if (!ReflectionUtils.IsPublic(property))
							{
								initialProperties[num] = subTypeProperty;
							}
						}
					}
					else if (!subTypeProperty.IsVirtual())
					{
						int num2 = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name && p.DeclaringType == subTypeProperty.DeclaringType);
						if (num2 == -1)
						{
							initialProperties.Add(subTypeProperty);
						}
					}
					else
					{
						int num3 = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name && p.IsVirtual() && p.GetBaseDefinition() != null && p.GetBaseDefinition().DeclaringType.IsAssignableFrom(subTypeProperty.DeclaringType));
						if (num3 == -1)
						{
							initialProperties.Add(subTypeProperty);
						}
					}
				}
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00030F30 File Offset: 0x0002F130
		public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
		{
			return currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any((MethodInfo info) => info.Name == method && info.DeclaringType != methodDeclaringType && info.GetBaseDefinition().DeclaringType == methodDeclaringType);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00030F6C File Offset: 0x0002F16C
		public static object GetDefaultValue(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
			case PrimitiveTypeCode.Char:
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
				return 0;
			case PrimitiveTypeCode.CharNullable:
			case PrimitiveTypeCode.BooleanNullable:
			case PrimitiveTypeCode.SByteNullable:
			case PrimitiveTypeCode.Int16Nullable:
			case PrimitiveTypeCode.UInt16Nullable:
			case PrimitiveTypeCode.Int32Nullable:
			case PrimitiveTypeCode.ByteNullable:
			case PrimitiveTypeCode.UInt32Nullable:
			case PrimitiveTypeCode.Int64Nullable:
			case PrimitiveTypeCode.UInt64Nullable:
			case PrimitiveTypeCode.SingleNullable:
			case PrimitiveTypeCode.DoubleNullable:
			case PrimitiveTypeCode.DateTimeNullable:
			case PrimitiveTypeCode.DateTimeOffsetNullable:
			case PrimitiveTypeCode.DecimalNullable:
				break;
			case PrimitiveTypeCode.Boolean:
				return false;
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
				return 0L;
			case PrimitiveTypeCode.Single:
				return 0f;
			case PrimitiveTypeCode.Double:
				return 0.0;
			case PrimitiveTypeCode.DateTime:
				return default(DateTime);
			case PrimitiveTypeCode.DateTimeOffset:
				return default(DateTimeOffset);
			case PrimitiveTypeCode.Decimal:
				return 0m;
			case PrimitiveTypeCode.Guid:
				return default(Guid);
			default:
				if (typeCode == PrimitiveTypeCode.BigInteger)
				{
					return default(BigInteger);
				}
				break;
			}
			if (ReflectionUtils.IsNullable(type))
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x04000450 RID: 1104
		public static readonly Type[] EmptyTypes = Type.EmptyTypes;
	}
}
