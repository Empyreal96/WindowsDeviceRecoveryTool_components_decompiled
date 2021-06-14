using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200015D RID: 349
	internal static class ODataValueUtils
	{
		// Token: 0x06000995 RID: 2453 RVA: 0x0001DCA4 File Offset: 0x0001BEA4
		internal static ODataValue ToODataValue(this object objectToConvert)
		{
			if (objectToConvert == null)
			{
				return new ODataNullValue();
			}
			ODataValue odataValue = objectToConvert as ODataValue;
			if (odataValue != null)
			{
				return odataValue;
			}
			return new ODataPrimitiveValue(objectToConvert);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001DCCC File Offset: 0x0001BECC
		internal static object FromODataValue(this ODataValue odataValue)
		{
			if (odataValue is ODataNullValue)
			{
				return null;
			}
			ODataPrimitiveValue odataPrimitiveValue = odataValue as ODataPrimitiveValue;
			if (odataPrimitiveValue != null)
			{
				return odataPrimitiveValue.Value;
			}
			return odataValue;
		}
	}
}
