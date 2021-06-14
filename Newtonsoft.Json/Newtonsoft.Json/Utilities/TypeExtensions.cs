using System;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000104 RID: 260
	internal static class TypeExtensions
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x0003163C File Offset: 0x0002F83C
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00031644 File Offset: 0x0002F844
		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0003164C File Offset: 0x0002F84C
		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00031654 File Offset: 0x0002F854
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0003165C File Offset: 0x0002F85C
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00031664 File Offset: 0x0002F864
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003166C File Offset: 0x0002F86C
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00031674 File Offset: 0x0002F874
		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0003167C File Offset: 0x0002F87C
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00031684 File Offset: 0x0002F884
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003168C File Offset: 0x0002F88C
		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00031694 File Offset: 0x0002F894
		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003169C File Offset: 0x0002F89C
		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x000316A4 File Offset: 0x0002F8A4
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000316AC File Offset: 0x0002F8AC
		public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			foreach (Type type3 in type.GetInterfaces())
			{
				if (string.Equals(type3.Name, fullTypeName, StringComparison.Ordinal))
				{
					match = type;
					return true;
				}
			}
			match = null;
			return false;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003171C File Offset: 0x0002F91C
		public static bool AssignableToTypeName(this Type type, string fullTypeName)
		{
			Type type2;
			return type.AssignableToTypeName(fullTypeName, out type2);
		}
	}
}
