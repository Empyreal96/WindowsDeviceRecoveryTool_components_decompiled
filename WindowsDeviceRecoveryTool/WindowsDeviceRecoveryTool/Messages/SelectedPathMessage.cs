using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000068 RID: 104
	public class SelectedPathMessage
	{
		// Token: 0x06000318 RID: 792 RVA: 0x00010274 File Offset: 0x0000E474
		public SelectedPathMessage(string selectedPath)
		{
			this.SelectedPath = selectedPath;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00010288 File Offset: 0x0000E488
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0001029F File Offset: 0x0000E49F
		public string SelectedPath { get; private set; }
	}
}
