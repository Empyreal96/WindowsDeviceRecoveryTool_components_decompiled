using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005E RID: 94
	public class FfuIntegrityCheckMessage
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000FE2C File Offset: 0x0000E02C
		public FfuIntegrityCheckMessage(bool result)
		{
			this.Result = result;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000FE40 File Offset: 0x0000E040
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000FE57 File Offset: 0x0000E057
		public bool Result { get; private set; }
	}
}
