using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000052 RID: 82
	public class ApplicationInvalidateSizeMessage
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x0000FBA0 File Offset: 0x0000DDA0
		public ApplicationInvalidateSizeMessage(ApplicationInvalidateSizeMessage.DataType type)
		{
			this.Type = type;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000FBCB File Offset: 0x0000DDCB
		public ApplicationInvalidateSizeMessage.DataType Type { get; private set; }

		// Token: 0x02000053 RID: 83
		public enum DataType
		{
			// Token: 0x0400010F RID: 271
			Logs,
			// Token: 0x04000110 RID: 272
			Reports,
			// Token: 0x04000111 RID: 273
			Packages
		}
	}
}
