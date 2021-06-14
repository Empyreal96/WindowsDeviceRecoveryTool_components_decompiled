using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000BB RID: 187
	public enum LeaseState
	{
		// Token: 0x04000449 RID: 1097
		Unspecified,
		// Token: 0x0400044A RID: 1098
		Available,
		// Token: 0x0400044B RID: 1099
		Leased,
		// Token: 0x0400044C RID: 1100
		Expired,
		// Token: 0x0400044D RID: 1101
		Breaking,
		// Token: 0x0400044E RID: 1102
		Broken
	}
}
