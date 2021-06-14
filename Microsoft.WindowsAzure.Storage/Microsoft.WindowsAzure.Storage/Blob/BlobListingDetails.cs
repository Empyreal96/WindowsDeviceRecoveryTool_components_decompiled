using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000AD RID: 173
	[Flags]
	public enum BlobListingDetails
	{
		// Token: 0x040003F5 RID: 1013
		None = 0,
		// Token: 0x040003F6 RID: 1014
		Snapshots = 1,
		// Token: 0x040003F7 RID: 1015
		Metadata = 2,
		// Token: 0x040003F8 RID: 1016
		UncommittedBlobs = 4,
		// Token: 0x040003F9 RID: 1017
		Copy = 8,
		// Token: 0x040003FA RID: 1018
		All = 15
	}
}
