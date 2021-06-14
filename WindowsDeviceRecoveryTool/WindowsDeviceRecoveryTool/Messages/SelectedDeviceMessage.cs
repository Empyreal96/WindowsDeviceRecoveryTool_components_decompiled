using System;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000067 RID: 103
	public class SelectedDeviceMessage
	{
		// Token: 0x06000315 RID: 789 RVA: 0x00010240 File Offset: 0x0000E440
		public SelectedDeviceMessage(Phone phone)
		{
			this.SelectedPhone = phone;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00010254 File Offset: 0x0000E454
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0001026B File Offset: 0x0000E46B
		public Phone SelectedPhone { get; private set; }
	}
}
