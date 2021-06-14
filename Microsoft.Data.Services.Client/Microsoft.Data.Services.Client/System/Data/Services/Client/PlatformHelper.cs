using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x02000137 RID: 311
	internal static class PlatformHelper
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x0002C48B File Offset: 0x0002A68B
		internal static Assembly GetAssembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0002C493 File Offset: 0x0002A693
		internal static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0002C49B File Offset: 0x0002A69B
		internal static bool IsGenericParameter(this Type type)
		{
			return type.IsGenericParameter;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002C4A3 File Offset: 0x0002A6A3
		internal static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002C4AB File Offset: 0x0002A6AB
		internal static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002C4B3 File Offset: 0x0002A6B3
		internal static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002C4BB File Offset: 0x0002A6BB
		internal static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002C4C3 File Offset: 0x0002A6C3
		internal static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002C4CB File Offset: 0x0002A6CB
		internal static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002C4D3 File Offset: 0x0002A6D3
		internal static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002C4DB File Offset: 0x0002A6DB
		internal static Type GetBaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002C4E3 File Offset: 0x0002A6E3
		internal static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002C4EB File Offset: 0x0002A6EB
		internal static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
		{
			return Array.AsReadOnly<T>(array);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002C4F3 File Offset: 0x0002A6F3
		internal static DateTime ConvertStringToDateTime(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002C504 File Offset: 0x0002A704
		internal static DateTimeOffset ConvertStringToDateTimeOffset(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTimeOffset(text);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002C514 File Offset: 0x0002A714
		internal static string AddSecondsPaddingIfMissing(string text)
		{
			int num = text.IndexOf("T", StringComparison.Ordinal);
			int num2 = num + 6;
			if (num > 0 && (text.Length <= num2 || text[num2] != ':'))
			{
				text = text.Insert(num2, ":00");
			}
			return text;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002C55C File Offset: 0x0002A75C
		internal static string ConvertDateTimeToStringInternal(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Unspecified)
			{
				DateTimeOffset value = new DateTimeOffset(dateTime, TimeSpan.Zero);
				string text = XmlConvert.ToString(value);
				return text.TrimEnd(new char[]
				{
					'Z'
				});
			}
			return XmlConvert.ToString(dateTime);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002C5A5 File Offset: 0x0002A7A5
		internal static string ConvertDateTimeToString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0002C5AE File Offset: 0x0002A7AE
		internal static Type GetTypeOrThrow(string typeName)
		{
			return Type.GetType(typeName, true);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002C5B7 File Offset: 0x0002A7B7
		internal static TypeCode GetTypeCode(Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002C5BF File Offset: 0x0002A7BF
		internal static UnicodeCategory GetUnicodeCategory(char c)
		{
			return char.GetUnicodeCategory(c);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002C5C7 File Offset: 0x0002A7C7
		internal static bool IsProperty(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Property;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002C5D3 File Offset: 0x0002A7D3
		internal static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002C5DB File Offset: 0x0002A7DB
		internal static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002C5E3 File Offset: 0x0002A7E3
		internal static bool IsMethod(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Method;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002C5EE File Offset: 0x0002A7EE
		internal static bool AreMembersEqual(MemberInfo member1, MemberInfo member2)
		{
			return member1.MetadataToken == member2.MetadataToken;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002C5FE File Offset: 0x0002A7FE
		internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type, bool instanceOnly)
		{
			return type.GetPublicProperties(instanceOnly, false);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002C608 File Offset: 0x0002A808
		internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type, bool instanceOnly, bool declaredOnly)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (!instanceOnly)
			{
				bindingFlags |= BindingFlags.Static;
			}
			if (declaredOnly)
			{
				bindingFlags |= BindingFlags.DeclaredOnly;
			}
			return type.GetProperties(bindingFlags);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002C630 File Offset: 0x0002A830
		internal static IEnumerable<ConstructorInfo> GetInstanceConstructors(this Type type, bool isPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructors(bindingFlags);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002C654 File Offset: 0x0002A854
		internal static ConstructorInfo GetInstanceConstructor(this Type type, bool isPublic, Type[] argTypes)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructor(bindingFlags, null, argTypes, null);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002C67C File Offset: 0x0002A87C
		internal static bool TryGetMethod(this Type type, string name, Type[] parameterTypes, out MethodInfo foundMethod)
		{
			foundMethod = null;
			bool result;
			try
			{
				foundMethod = type.GetMethod(name, parameterTypes);
				result = (foundMethod != null);
			}
			catch (ArgumentNullException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002C6B8 File Offset: 0x0002A8B8
		internal static MethodInfo GetMethod(this Type type, string name, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002C6E8 File Offset: 0x0002A8E8
		internal static MethodInfo GetMethod(this Type type, string name, Type[] types, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags, null, types, null);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002C719 File Offset: 0x0002A919
		internal static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0002C723 File Offset: 0x0002A923
		internal static IEnumerable<Type> GetNonPublicNestedTypes(this Type type)
		{
			return type.GetNestedTypes(BindingFlags.NonPublic);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002C72D File Offset: 0x0002A92D
		public static Regex CreateCompiled(string pattern, RegexOptions options)
		{
			options |= RegexOptions.Compiled;
			return new Regex(pattern, options);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002C73B File Offset: 0x0002A93B
		public static string[] GetSegments(this Uri uri)
		{
			return uri.Segments;
		}

		// Token: 0x04000604 RID: 1540
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x04000605 RID: 1541
		internal static readonly string UriSchemeHttp = Uri.UriSchemeHttp;

		// Token: 0x04000606 RID: 1542
		internal static readonly string UriSchemeHttps = Uri.UriSchemeHttps;
	}
}
