using System;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000DC RID: 220
	public sealed class FileSharePermissions
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x0004236C File Offset: 0x0004056C
		public FileSharePermissions()
		{
			this.SharedAccessPolicies = new SharedAccessFilePolicies();
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x0004237F File Offset: 0x0004057F
		// (set) Token: 0x060011BC RID: 4540 RVA: 0x00042387 File Offset: 0x00040587
		public SharedAccessFilePolicies SharedAccessPolicies { get; private set; }
	}
}
