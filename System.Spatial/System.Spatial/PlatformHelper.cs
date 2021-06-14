using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace System.Spatial
{
	// Token: 0x02000002 RID: 2
	internal static class PlatformHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		internal static Assembly GetAssembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		internal static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020E0 File Offset: 0x000002E0
		internal static bool IsGenericParameter(this Type type)
		{
			return type.IsGenericParameter;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E8 File Offset: 0x000002E8
		internal static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		internal static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F8 File Offset: 0x000002F8
		internal static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002100 File Offset: 0x00000300
		internal static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002108 File Offset: 0x00000308
		internal static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002110 File Offset: 0x00000310
		internal static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002118 File Offset: 0x00000318
		internal static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002120 File Offset: 0x00000320
		internal static Type GetBaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002128 File Offset: 0x00000328
		internal static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002130 File Offset: 0x00000330
		internal static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
		{
			return Array.AsReadOnly<T>(array);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002138 File Offset: 0x00000338
		internal static DateTime ConvertStringToDateTime(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002149 File Offset: 0x00000349
		internal static DateTimeOffset ConvertStringToDateTimeOffset(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTimeOffset(text);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000215C File Offset: 0x0000035C
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

		// Token: 0x06000011 RID: 17 RVA: 0x000021A4 File Offset: 0x000003A4
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

		// Token: 0x06000012 RID: 18 RVA: 0x000021ED File Offset: 0x000003ED
		internal static string ConvertDateTimeToString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021F6 File Offset: 0x000003F6
		internal static Type GetTypeOrThrow(string typeName)
		{
			return Type.GetType(typeName, true);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021FF File Offset: 0x000003FF
		internal static TypeCode GetTypeCode(Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002207 File Offset: 0x00000407
		internal static UnicodeCategory GetUnicodeCategory(char c)
		{
			return char.GetUnicodeCategory(c);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000220F File Offset: 0x0000040F
		internal static bool IsProperty(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Property;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000221B File Offset: 0x0000041B
		internal static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002223 File Offset: 0x00000423
		internal static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000222B File Offset: 0x0000042B
		internal static bool IsMethod(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Method;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002236 File Offset: 0x00000436
		internal static bool AreMembersEqual(MemberInfo member1, MemberInfo member2)
		{
			return member1.MetadataToken == member2.MetadataToken;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002246 File Offset: 0x00000446
		internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type, bool instanceOnly)
		{
			return type.GetPublicProperties(instanceOnly, false);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002250 File Offset: 0x00000450
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

		// Token: 0x0600001D RID: 29 RVA: 0x00002278 File Offset: 0x00000478
		internal static IEnumerable<ConstructorInfo> GetInstanceConstructors(this Type type, bool isPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructors(bindingFlags);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000229C File Offset: 0x0000049C
		internal static ConstructorInfo GetInstanceConstructor(this Type type, bool isPublic, Type[] argTypes)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructor(bindingFlags, null, argTypes, null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000022C4 File Offset: 0x000004C4
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

		// Token: 0x06000020 RID: 32 RVA: 0x00002300 File Offset: 0x00000500
		internal static MethodInfo GetMethod(this Type type, string name, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002330 File Offset: 0x00000530
		internal static MethodInfo GetMethod(this Type type, string name, Type[] types, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags, null, types, null);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002361 File Offset: 0x00000561
		internal static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000236B File Offset: 0x0000056B
		internal static IEnumerable<Type> GetNonPublicNestedTypes(this Type type)
		{
			return type.GetNestedTypes(BindingFlags.NonPublic);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002375 File Offset: 0x00000575
		public static Regex CreateCompiled(string pattern, RegexOptions options)
		{
			options |= RegexOptions.Compiled;
			return new Regex(pattern, options);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002383 File Offset: 0x00000583
		public static string[] GetSegments(this Uri uri)
		{
			return uri.Segments;
		}

		// Token: 0x04000001 RID: 1
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x04000002 RID: 2
		internal static readonly string UriSchemeHttp = Uri.UriSchemeHttp;

		// Token: 0x04000003 RID: 3
		internal static readonly string UriSchemeHttps = Uri.UriSchemeHttps;
	}
}
