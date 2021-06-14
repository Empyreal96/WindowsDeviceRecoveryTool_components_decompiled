using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000021 RID: 33
	public class DeviceBatteryStatusReadMessage
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00008428 File Offset: 0x00006628
		public DeviceBatteryStatusReadMessage(BatteryStatus status)
		{
			this.Status = status;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000843C File Offset: 0x0000663C
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00008453 File Offset: 0x00006653
		public BatteryStatus Status { get; private set; }
	}
}
