using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000C1 RID: 193
	[Flags]
	public enum SharedAccessBlobPermissions
	{
		// Token: 0x04000462 RID: 1122
		None = 0,
		// Token: 0x04000463 RID: 1123
		Read = 1,
		// Token: 0x04000464 RID: 1124
		Write = 2,
		// Token: 0x04000465 RID: 1125
		Delete = 4,
		// Token: 0x04000466 RID: 1126
		List = 8
	}
}
