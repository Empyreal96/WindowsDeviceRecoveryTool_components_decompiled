using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000145 RID: 325
	internal static class JsonSharedUtils
	{
		// Token: 0x060008CB RID: 2251 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		internal static bool IsDoubleValueSerializedAsString(double value)
		{
			return double.IsInfinity(value) || double.IsNaN(value);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001C5F4 File Offset: 0x0001A7F4
		internal static bool ValueTypeMatchesJsonType(ODataPrimitiveValue primitiveValue, IEdmPrimitiveTypeReference valueTypeReference)
		{
			EdmPrimitiveTypeKind edmPrimitiveTypeKind = valueTypeReference.PrimitiveKind();
			if (edmPrimitiveTypeKind <= EdmPrimitiveTypeKind.Double)
			{
				if (edmPrimitiveTypeKind != EdmPrimitiveTypeKind.Boolean)
				{
					if (edmPrimitiveTypeKind != EdmPrimitiveTypeKind.Double)
					{
						return false;
					}
					double value = (double)primitiveValue.Value;
					return !JsonSharedUtils.IsDoubleValueSerializedAsString(value);
				}
			}
			else if (edmPrimitiveTypeKind != EdmPrimitiveTypeKind.Int32 && edmPrimitiveTypeKind != EdmPrimitiveTypeKind.String)
			{
				return false;
			}
			return true;
		}
	}
}
