using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000074 RID: 116
	internal class EncryptionData
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00038129 File Offset: 0x00036329
		// (set) Token: 0x06000E78 RID: 3704 RVA: 0x00038131 File Offset: 0x00036331
		public WrappedKey WrappedContentKey { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0003813A File Offset: 0x0003633A
		// (set) Token: 0x06000E7A RID: 3706 RVA: 0x00038142 File Offset: 0x00036342
		public EncryptionAgent EncryptionAgent { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0003814B File Offset: 0x0003634B
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x00038153 File Offset: 0x00036353
		public byte[] ContentEncryptionIV { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0003815C File Offset: 0x0003635C
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x00038164 File Offset: 0x00036364
		public IDictionary<string, string> KeyWrappingMetadata { get; set; }
	}
}
