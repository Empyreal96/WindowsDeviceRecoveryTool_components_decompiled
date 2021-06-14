using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Microsoft.Data.Edm
{
	// Token: 0x0200023F RID: 575
	internal static class PlatformHelper
	{
		// Token: 0x06000D1F RID: 3359 RVA: 0x00029EC5 File Offset: 0x000280C5
		internal static Assembly GetAssembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00029ECD File Offset: 0x000280CD
		internal static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00029ED5 File Offset: 0x000280D5
		internal static bool IsGenericParameter(this Type type)
		{
			return type.IsGenericParameter;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00029EDD File Offset: 0x000280DD
		internal static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00029EE5 File Offset: 0x000280E5
		internal static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00029EED File Offset: 0x000280ED
		internal static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00029EF5 File Offset: 0x000280F5
		internal static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00029EFD File Offset: 0x000280FD
		internal static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00029F05 File Offset: 0x00028105
		internal static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00029F0D File Offset: 0x0002810D
		internal static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00029F15 File Offset: 0x00028115
		internal static Type GetBaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00029F1D File Offset: 0x0002811D
		internal static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00029F25 File Offset: 0x00028125
		internal static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
		{
			return Array.AsReadOnly<T>(array);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00029F2D File Offset: 0x0002812D
		internal static DateTime ConvertStringToDateTime(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00029F3E File Offset: 0x0002813E
		internal static DateTimeOffset ConvertStringToDateTimeOffset(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTimeOffset(text);
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00029F50 File Offset: 0x00028150
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

		// Token: 0x06000D2F RID: 3375 RVA: 0x00029F98 File Offset: 0x00028198
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

		// Token: 0x06000D30 RID: 3376 RVA: 0x00029FE1 File Offset: 0x000281E1
		internal static string ConvertDateTimeToString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00029FEA File Offset: 0x000281EA
		internal static Type GetTypeOrThrow(string typeName)
		{
			return Type.GetType(typeName, true);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00029FF3 File Offset: 0x000281F3
		internal static TypeCode GetTypeCode(Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00029FFB File Offset: 0x000281FB
		internal static UnicodeCategory GetUnicodeCategory(char c)
		{
			return char.GetUnicodeCategory(c);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0002A003 File Offset: 0x00028203
		internal static bool IsProperty(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Property;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0002A00F File Offset: 0x0002820F
		internal static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0002A017 File Offset: 0x00028217
		internal static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002A01F File Offset: 0x0002821F
		internal static bool IsMethod(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Method;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0002A02A File Offset: 0x0002822A
		internal static bool AreMembersEqual(MemberInfo member1, MemberInfo member2)
		{
			return member1.MetadataToken == member2.MetadataToken;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0002A03A File Offset: 0x0002823A
		internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type, bool instanceOnly)
		{
			return type.GetPublicProperties(instanceOnly, false);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0002A044 File Offset: 0x00028244
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

		// Token: 0x06000D3B RID: 3387 RVA: 0x0002A06C File Offset: 0x0002826C
		internal static IEnumerable<ConstructorInfo> GetInstanceConstructors(this Type type, bool isPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructors(bindingFlags);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0002A090 File Offset: 0x00028290
		internal static ConstructorInfo GetInstanceConstructor(this Type type, bool isPublic, Type[] argTypes)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructor(bindingFlags, null, argTypes, null);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0002A0B8 File Offset: 0x000282B8
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

		// Token: 0x06000D3E RID: 3390 RVA: 0x0002A0F4 File Offset: 0x000282F4
		internal static MethodInfo GetMethod(this Type type, string name, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0002A124 File Offset: 0x00028324
		internal static MethodInfo GetMethod(this Type type, string name, Type[] types, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags, null, types, null);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0002A155 File Offset: 0x00028355
		internal static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0002A15F File Offset: 0x0002835F
		internal static IEnumerable<Type> GetNonPublicNestedTypes(this Type type)
		{
			return type.GetNestedTypes(BindingFlags.NonPublic);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0002A169 File Offset: 0x00028369
		public static Regex CreateCompiled(string pattern, RegexOptions options)
		{
			options |= RegexOptions.Compiled;
			return new Regex(pattern, options);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0002A177 File Offset: 0x00028377
		public static string[] GetSegments(this Uri uri)
		{
			return uri.Segments;
		}

		// Token: 0x0400074B RID: 1867
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x0400074C RID: 1868
		internal static readonly string UriSchemeHttp = Uri.UriSchemeHttp;

		// Token: 0x0400074D RID: 1869
		internal static readonly string UriSchemeHttps = Uri.UriSchemeHttps;
	}
}
