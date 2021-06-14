using System;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000D0 RID: 208
	public sealed class ListBlobPrefixEntry : IListBlobEntry
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x0004070E File Offset: 0x0003E90E
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x00040716 File Offset: 0x0003E916
		public string Name { get; internal set; }
	}
}
