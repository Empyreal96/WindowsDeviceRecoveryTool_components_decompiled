using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000050 RID: 80
	public class ApplicationDataSizeMessage
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public ApplicationDataSizeMessage(ApplicationDataSizeMessage.DataType type, long filesSize)
		{
			this.Type = type;
			this.FilesSize = filesSize;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000FB60 File Offset: 0x0000DD60
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000FB77 File Offset: 0x0000DD77
		public long FilesSize { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000FB80 File Offset: 0x0000DD80
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000FB97 File Offset: 0x0000DD97
		public ApplicationDataSizeMessage.DataType Type { get; private set; }

		// Token: 0x02000051 RID: 81
		public enum DataType
		{
			// Token: 0x0400010A RID: 266
			Logs,
			// Token: 0x0400010B RID: 267
			Reports,
			// Token: 0x0400010C RID: 268
			Packages
		}
	}
}
