using System;
using Microsoft.WindowsDeviceRecoveryTool.States.Help;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005B RID: 91
	public class HelpScreenChangedMessage
	{
		// Token: 0x060002D4 RID: 724 RVA: 0x0000FD90 File Offset: 0x0000DF90
		public HelpScreenChangedMessage(HelpTabs tab)
		{
			this.SelectedTab = tab;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000FDBB File Offset: 0x0000DFBB
		public HelpTabs SelectedTab { get; private set; }
	}
}
