using System;

namespace Microsoft.Data.OData
{
	// Token: 0x020001DA RID: 474
	public enum ODataBatchReaderState
	{
		// Token: 0x04000513 RID: 1299
		Initial,
		// Token: 0x04000514 RID: 1300
		Operation,
		// Token: 0x04000515 RID: 1301
		ChangesetStart,
		// Token: 0x04000516 RID: 1302
		ChangesetEnd,
		// Token: 0x04000517 RID: 1303
		Completed,
		// Token: 0x04000518 RID: 1304
		Exception
	}
}
