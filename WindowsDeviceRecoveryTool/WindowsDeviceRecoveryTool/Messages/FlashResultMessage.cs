using System;
using System.Collections.Generic;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000062 RID: 98
	public class FlashResultMessage
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000FF6C File Offset: 0x0000E16C
		public FlashResultMessage(bool result)
		{
			this.Result = result;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FF7F File Offset: 0x0000E17F
		public FlashResultMessage(bool result, List<string> extendedMessage)
		{
			this.Result = result;
			this.ExtendedMessage = extendedMessage;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000FF9A File Offset: 0x0000E19A
		public FlashResultMessage(bool result, List<string> extendedMessage, string argument)
		{
			this.Result = result;
			this.ExtendedMessage = extendedMessage;
			this.Argument = argument;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000FFD7 File Offset: 0x0000E1D7
		public bool Result { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000FFE0 File Offset: 0x0000E1E0
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000FFF7 File Offset: 0x0000E1F7
		public List<string> ExtendedMessage { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00010000 File Offset: 0x0000E200
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00010017 File Offset: 0x0000E217
		public string Argument { get; private set; }
	}
}
