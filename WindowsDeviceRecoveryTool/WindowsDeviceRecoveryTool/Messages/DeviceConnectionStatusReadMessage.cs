using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000020 RID: 32
	public class DeviceConnectionStatusReadMessage
	{
		// Token: 0x060000FA RID: 250 RVA: 0x000083F2 File Offset: 0x000065F2
		public DeviceConnectionStatusReadMessage(bool status)
		{
			this.Status = status;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00008408 File Offset: 0x00006608
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000841F File Offset: 0x0000661F
		public bool Status { get; private set; }
	}
}
