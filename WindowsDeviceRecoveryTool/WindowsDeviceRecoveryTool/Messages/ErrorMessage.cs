using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006E RID: 110
	public class ErrorMessage
	{
		// Token: 0x06000330 RID: 816 RVA: 0x00010424 File Offset: 0x0000E624
		public ErrorMessage(Exception exception)
		{
			this.Exception = exception;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00010438 File Offset: 0x0000E638
		// (set) Token: 0x06000332 RID: 818 RVA: 0x0001044F File Offset: 0x0000E64F
		public Exception Exception { get; set; }
	}
}
