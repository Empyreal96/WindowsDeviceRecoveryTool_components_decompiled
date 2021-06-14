using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001E3 RID: 483
	public enum ODataParameterReaderState
	{
		// Token: 0x0400052B RID: 1323
		Start,
		// Token: 0x0400052C RID: 1324
		Value,
		// Token: 0x0400052D RID: 1325
		Collection,
		// Token: 0x0400052E RID: 1326
		Exception,
		// Token: 0x0400052F RID: 1327
		Completed
	}
}
