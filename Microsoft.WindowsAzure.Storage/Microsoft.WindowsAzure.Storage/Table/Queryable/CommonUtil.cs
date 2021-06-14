using System;
using System.Dynamic;
using System.Linq;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200010A RID: 266
	internal static class CommonUtil
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x0004638C File Offset: 0x0004458C
		internal static bool IsUnsupportedType(Type type)
		{
			if (type.IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			return CommonUtil.UnsupportedTypes.Any((Type t) => t.IsAssignableFrom(type));
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x000463DF File Offset: 0x000445DF
		internal static bool IsClientType(Type t)
		{
			return t.GetInterface(typeof(ITableEntity).FullName, false) != null;
		}

		// Token: 0x04000585 RID: 1413
		private static readonly Type[] UnsupportedTypes = new Type[]
		{
			typeof(IDynamicMetaObjectProvider),
			typeof(Tuple<>),
			typeof(Tuple<, >),
			typeof(Tuple<, , >),
			typeof(Tuple<, , , >),
			typeof(Tuple<, , , , >),
			typeof(Tuple<, , , , , >),
			typeof(Tuple<, , , , , , >),
			typeof(Tuple<, , , , , , , >)
		};
	}
}
