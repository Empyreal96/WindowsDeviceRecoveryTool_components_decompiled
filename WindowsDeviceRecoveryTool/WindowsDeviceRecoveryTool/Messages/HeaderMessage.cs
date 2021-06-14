using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000063 RID: 99
	public class HeaderMessage
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x00010020 File Offset: 0x0000E220
		public HeaderMessage(string header, string subheader = "")
		{
			this.Header = header;
			this.Subheader = subheader;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0001003C File Offset: 0x0000E23C
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00010053 File Offset: 0x0000E253
		public string Header { get; private set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0001005C File Offset: 0x0000E25C
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00010073 File Offset: 0x0000E273
		public string Subheader { get; private set; }
	}
}
