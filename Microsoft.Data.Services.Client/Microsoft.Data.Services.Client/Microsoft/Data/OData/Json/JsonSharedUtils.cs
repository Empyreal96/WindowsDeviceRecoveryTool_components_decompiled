using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000006 RID: 6
	internal static class JsonSharedUtils
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002686 File Offset: 0x00000886
		internal static bool IsDoubleValueSerializedAsString(double value)
		{
			return double.IsInfinity(value) || double.IsNaN(value);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002698 File Offset: 0x00000898
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
