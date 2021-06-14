using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000279 RID: 633
	internal static class TypeUtils
	{
		// Token: 0x060014DD RID: 5341 RVA: 0x0004D670 File Offset: 0x0004B870
		internal static bool IsNullableType(Type type)
		{
			return type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004D691 File Offset: 0x0004B891
		internal static Type GetNonNullableType(Type type)
		{
			return Nullable.GetUnderlyingType(type) ?? type;
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004D6A0 File Offset: 0x0004B8A0
		internal static Type GetNullableType(Type type)
		{
			if (!TypeUtils.TypeAllowsNull(type))
			{
				type = typeof(Nullable<>).MakeGenericType(new Type[]
				{
					type
				});
			}
			return type;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004D6D3 File Offset: 0x0004B8D3
		internal static bool TypeAllowsNull(Type type)
		{
			return !type.IsValueType() || TypeUtils.IsNullableType(type);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004D6E5 File Offset: 0x0004B8E5
		internal static bool AreTypesEquivalent(Type typeA, Type typeB)
		{
			return !(typeA == null) && !(typeB == null) && typeA.IsEquivalentTo(typeB);
		}
	}
}
