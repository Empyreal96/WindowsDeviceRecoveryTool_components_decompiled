using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000058 RID: 88
	public class DeviceInfoReadMessage
	{
		// Token: 0x060002CB RID: 715 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		public DeviceInfoReadMessage(bool result)
		{
			this.Result = result;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000FD08 File Offset: 0x0000DF08
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000FD1F File Offset: 0x0000DF1F
		public bool Result { get; private set; }
	}
}
