using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000061 RID: 97
	public class FfuFilePlatformIdMessage
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public FfuFilePlatformIdMessage(PlatformId platformId, string version)
		{
			this.PlatformId = platformId;
			this.Version = version;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000FF0C File Offset: 0x0000E10C
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000FF23 File Offset: 0x0000E123
		public PlatformId PlatformId { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000FF2C File Offset: 0x0000E12C
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000FF43 File Offset: 0x0000E143
		public string Version { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000FF4C File Offset: 0x0000E14C
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000FF63 File Offset: 0x0000E163
		public IEnumerable<PlatformId> AllPlatformIds { get; set; }
	}
}
