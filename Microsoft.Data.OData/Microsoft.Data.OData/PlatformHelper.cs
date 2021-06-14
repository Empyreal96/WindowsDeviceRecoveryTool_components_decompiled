using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Microsoft.Data.OData
{
	// Token: 0x020002AF RID: 687
	internal static class PlatformHelper
	{
		// Token: 0x06001764 RID: 5988 RVA: 0x00053FA8 File Offset: 0x000521A8
		internal static Assembly GetAssembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00053FB0 File Offset: 0x000521B0
		internal static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00053FB8 File Offset: 0x000521B8
		internal static bool IsGenericParameter(this Type type)
		{
			return type.IsGenericParameter;
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00053FC0 File Offset: 0x000521C0
		internal static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00053FC8 File Offset: 0x000521C8
		internal static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00053FD0 File Offset: 0x000521D0
		internal static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00053FD8 File Offset: 0x000521D8
		internal static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00053FE0 File Offset: 0x000521E0
		internal static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00053FE8 File Offset: 0x000521E8
		internal static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00053FF0 File Offset: 0x000521F0
		internal static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00053FF8 File Offset: 0x000521F8
		internal static Type GetBaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00054000 File Offset: 0x00052200
		internal static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00054008 File Offset: 0x00052208
		internal static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
		{
			return Array.AsReadOnly<T>(array);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00054010 File Offset: 0x00052210
		internal static DateTime ConvertStringToDateTime(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTime(text, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00054021 File Offset: 0x00052221
		internal static DateTimeOffset ConvertStringToDateTimeOffset(string text)
		{
			text = PlatformHelper.AddSecondsPaddingIfMissing(text);
			return XmlConvert.ToDateTimeOffset(text);
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00054034 File Offset: 0x00052234
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

		// Token: 0x06001774 RID: 6004 RVA: 0x0005407C File Offset: 0x0005227C
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

		// Token: 0x06001775 RID: 6005 RVA: 0x000540C5 File Offset: 0x000522C5
		internal static string ConvertDateTimeToString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000540CE File Offset: 0x000522CE
		internal static Type GetTypeOrThrow(string typeName)
		{
			return Type.GetType(typeName, true);
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000540D7 File Offset: 0x000522D7
		internal static TypeCode GetTypeCode(Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000540DF File Offset: 0x000522DF
		internal static UnicodeCategory GetUnicodeCategory(char c)
		{
			return char.GetUnicodeCategory(c);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000540E7 File Offset: 0x000522E7
		internal static bool IsProperty(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Property;
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000540F3 File Offset: 0x000522F3
		internal static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000540FB File Offset: 0x000522FB
		internal static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00054103 File Offset: 0x00052303
		internal static bool IsMethod(MemberInfo member)
		{
			return member.MemberType == MemberTypes.Method;
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x0005410E File Offset: 0x0005230E
		internal static bool AreMembersEqual(MemberInfo member1, MemberInfo member2)
		{
			return member1.MetadataToken == member2.MetadataToken;
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0005411E File Offset: 0x0005231E
		internal static IEnumerable<PropertyInfo> GetPublicProperties(this Type type, bool instanceOnly)
		{
			return type.GetPublicProperties(instanceOnly, false);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00054128 File Offset: 0x00052328
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

		// Token: 0x06001780 RID: 6016 RVA: 0x00054150 File Offset: 0x00052350
		internal static IEnumerable<ConstructorInfo> GetInstanceConstructors(this Type type, bool isPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructors(bindingFlags);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00054174 File Offset: 0x00052374
		internal static ConstructorInfo GetInstanceConstructor(this Type type, bool isPublic, Type[] argTypes)
		{
			BindingFlags bindingFlags = BindingFlags.Instance;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			return type.GetConstructor(bindingFlags, null, argTypes, null);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0005419C File Offset: 0x0005239C
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

		// Token: 0x06001783 RID: 6019 RVA: 0x000541D8 File Offset: 0x000523D8
		internal static MethodInfo GetMethod(this Type type, string name, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags);
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00054208 File Offset: 0x00052408
		internal static MethodInfo GetMethod(this Type type, string name, Type[] types, bool isPublic, bool isStatic)
		{
			BindingFlags bindingFlags = BindingFlags.Default;
			bindingFlags |= (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
			bindingFlags |= (isStatic ? BindingFlags.Static : BindingFlags.Instance);
			return type.GetMethod(name, bindingFlags, null, types, null);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00054239 File Offset: 0x00052439
		internal static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00054243 File Offset: 0x00052443
		internal static IEnumerable<Type> GetNonPublicNestedTypes(this Type type)
		{
			return type.GetNestedTypes(BindingFlags.NonPublic);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0005424D File Offset: 0x0005244D
		public static Regex CreateCompiled(string pattern, RegexOptions options)
		{
			options |= RegexOptions.Compiled;
			return new Regex(pattern, options);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0005425B File Offset: 0x0005245B
		public static string[] GetSegments(this Uri uri)
		{
			return uri.Segments;
		}

		// Token: 0x040009A6 RID: 2470
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x040009A7 RID: 2471
		internal static readonly string UriSchemeHttp = Uri.UriSchemeHttp;

		// Token: 0x040009A8 RID: 2472
		internal static readonly string UriSchemeHttps = Uri.UriSchemeHttps;
	}
}
