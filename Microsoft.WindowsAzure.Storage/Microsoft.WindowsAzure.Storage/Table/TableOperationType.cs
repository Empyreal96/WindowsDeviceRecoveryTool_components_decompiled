using System;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000146 RID: 326
	public enum TableOperationType
	{
		// Token: 0x0400080B RID: 2059
		Insert,
		// Token: 0x0400080C RID: 2060
		Delete,
		// Token: 0x0400080D RID: 2061
		Replace,
		// Token: 0x0400080E RID: 2062
		Merge,
		// Token: 0x0400080F RID: 2063
		InsertOrReplace,
		// Token: 0x04000810 RID: 2064
		InsertOrMerge,
		// Token: 0x04000811 RID: 2065
		Retrieve
	}
}
