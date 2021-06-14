using System;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005A RID: 90
	public class DeviceDisconnectedMessage
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000FD5C File Offset: 0x0000DF5C
		public DeviceDisconnectedMessage(Phone phone)
		{
			this.Phone = phone;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000FD70 File Offset: 0x0000DF70
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000FD87 File Offset: 0x0000DF87
		public Phone Phone { get; private set; }
	}
}
