using System;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200013D RID: 317
	[Flags]
	internal enum EntityReadFlags
	{
		// Token: 0x040007ED RID: 2029
		PartitionKey = 1,
		// Token: 0x040007EE RID: 2030
		RowKey = 2,
		// Token: 0x040007EF RID: 2031
		Timestamp = 4,
		// Token: 0x040007F0 RID: 2032
		Etag = 8,
		// Token: 0x040007F1 RID: 2033
		Properties = 16,
		// Token: 0x040007F2 RID: 2034
		All = 31
	}
}
