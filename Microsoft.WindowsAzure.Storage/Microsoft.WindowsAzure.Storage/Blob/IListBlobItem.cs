using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000015 RID: 21
	public interface IListBlobItem
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000185 RID: 389
		Uri Uri { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000186 RID: 390
		StorageUri StorageUri { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000187 RID: 391
		CloudBlobDirectory Parent { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000188 RID: 392
		CloudBlobContainer Container { get; }
	}
}
