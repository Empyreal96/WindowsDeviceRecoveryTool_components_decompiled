using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200014F RID: 335
	public sealed class ODataNullValue : ODataValue
	{
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0001CCE6 File Offset: 0x0001AEE6
		internal override bool IsNullValue
		{
			get
			{
				return true;
			}
		}
	}
}
