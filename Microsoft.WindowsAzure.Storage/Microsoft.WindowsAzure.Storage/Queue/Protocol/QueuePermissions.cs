using System;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000103 RID: 259
	public sealed class QueuePermissions
	{
		// Token: 0x06001298 RID: 4760 RVA: 0x00044F86 File Offset: 0x00043186
		public QueuePermissions()
		{
			this.SharedAccessPolicies = new SharedAccessQueuePolicies();
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00044F99 File Offset: 0x00043199
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x00044FA1 File Offset: 0x000431A1
		public SharedAccessQueuePolicies SharedAccessPolicies { get; private set; }
	}
}
