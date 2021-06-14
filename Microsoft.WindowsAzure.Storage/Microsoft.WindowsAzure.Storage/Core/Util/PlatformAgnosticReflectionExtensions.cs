using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200009B RID: 155
	internal static class PlatformAgnosticReflectionExtensions
	{
		// Token: 0x06001021 RID: 4129 RVA: 0x0003D840 File Offset: 0x0003BA40
		public static IEnumerable<MethodInfo> FindStaticMethods(this Type type, string name)
		{
			return from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
			where m.Name == name
			select m;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003D873 File Offset: 0x0003BA73
		public static IEnumerable<MethodInfo> FindMethods(this Type type)
		{
			return type.GetMethods();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003D87B File Offset: 0x0003BA7B
		public static MethodInfo FindMethod(this Type type, string name, Type[] parameters)
		{
			return type.GetMethod(name, parameters);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003D885 File Offset: 0x0003BA85
		public static PropertyInfo FindProperty(this Type type, string name)
		{
			return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003D890 File Offset: 0x0003BA90
		public static MethodInfo FindGetProp(this PropertyInfo property)
		{
			return property.GetGetMethod();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003D898 File Offset: 0x0003BA98
		public static MethodInfo FindSetProp(this PropertyInfo property)
		{
			return property.GetSetMethod();
		}
	}
}
