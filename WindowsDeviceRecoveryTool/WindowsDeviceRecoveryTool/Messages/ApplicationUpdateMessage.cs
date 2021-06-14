using System;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200004F RID: 79
	public class ApplicationUpdateMessage
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0000FB0E File Offset: 0x0000DD0E
		public ApplicationUpdateMessage(ApplicationUpdate update)
		{
			this.Update = update;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000FB24 File Offset: 0x0000DD24
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000FB3B File Offset: 0x0000DD3B
		public ApplicationUpdate Update { get; set; }
	}
}
