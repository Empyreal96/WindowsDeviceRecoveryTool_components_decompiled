using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200008A RID: 138
	public abstract class ODataValue : ODataAnnotatable
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000B63E File Offset: 0x0000983E
		internal virtual bool IsNullValue
		{
			get
			{
				return false;
			}
		}
	}
}
