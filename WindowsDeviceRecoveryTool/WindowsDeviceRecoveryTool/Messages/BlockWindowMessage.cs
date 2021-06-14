using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000055 RID: 85
	public class BlockWindowMessage
	{
		// Token: 0x060002BE RID: 702 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public BlockWindowMessage(bool block, string message = null, string title = null)
		{
			this.Block = block;
			this.Message = message;
			this.Title = title;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000FC43 File Offset: 0x0000DE43
		public bool Block { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000FC63 File Offset: 0x0000DE63
		public string Message { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000FC83 File Offset: 0x0000DE83
		public string Title { get; private set; }
	}
}
