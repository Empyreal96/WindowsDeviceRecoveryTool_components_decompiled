using System;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000059 RID: 89
	public class DeviceConnectedMessage
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000FD28 File Offset: 0x0000DF28
		public DeviceConnectedMessage(Phone phone)
		{
			this.Phone = phone;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000FD53 File Offset: 0x0000DF53
		public Phone Phone { get; private set; }
	}
}
