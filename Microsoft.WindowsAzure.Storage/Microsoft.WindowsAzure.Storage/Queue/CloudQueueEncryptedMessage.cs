using System;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x02000035 RID: 53
	internal class CloudQueueEncryptedMessage
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00025E15 File Offset: 0x00024015
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00025E1D File Offset: 0x0002401D
		public string EncryptedMessageContents { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00025E26 File Offset: 0x00024026
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x00025E2E File Offset: 0x0002402E
		public EncryptionData EncryptionData { get; set; }
	}
}
