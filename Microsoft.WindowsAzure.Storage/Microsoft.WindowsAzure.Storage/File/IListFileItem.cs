using System;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000023 RID: 35
	public interface IListFileItem
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060006A7 RID: 1703
		Uri Uri { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060006A8 RID: 1704
		StorageUri StorageUri { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060006A9 RID: 1705
		CloudFileDirectory Parent { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060006AA RID: 1706
		CloudFileShare Share { get; }
	}
}
