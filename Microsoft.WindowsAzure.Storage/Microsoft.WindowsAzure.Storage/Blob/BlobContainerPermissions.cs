using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000A7 RID: 167
	public sealed class BlobContainerPermissions
	{
		// Token: 0x06001066 RID: 4198 RVA: 0x0003E45A File Offset: 0x0003C65A
		public BlobContainerPermissions()
		{
			this.PublicAccess = BlobContainerPublicAccessType.Off;
			this.SharedAccessPolicies = new SharedAccessBlobPolicies();
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x0003E474 File Offset: 0x0003C674
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x0003E47C File Offset: 0x0003C67C
		public BlobContainerPublicAccessType PublicAccess { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x0003E485 File Offset: 0x0003C685
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x0003E48D File Offset: 0x0003C68D
		public SharedAccessBlobPolicies SharedAccessPolicies { get; private set; }
	}
}
