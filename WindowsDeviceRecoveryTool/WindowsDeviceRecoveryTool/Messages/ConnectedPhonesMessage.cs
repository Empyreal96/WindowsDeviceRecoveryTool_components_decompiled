using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000057 RID: 87
	public class ConnectedPhonesMessage
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000FCC0 File Offset: 0x0000DEC0
		public ConnectedPhonesMessage(List<Phone> phones)
		{
			this.Phones = phones;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		public List<Phone> Phones { get; private set; }
	}
}
