using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005D RID: 93
	public class FirmwareVersionsCompareMessage
	{
		// Token: 0x060002DA RID: 730 RVA: 0x0000FDF8 File Offset: 0x0000DFF8
		public FirmwareVersionsCompareMessage(SwVersionComparisonResult status)
		{
			this.Status = status;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000FE0C File Offset: 0x0000E00C
		// (set) Token: 0x060002DC RID: 732 RVA: 0x0000FE23 File Offset: 0x0000E023
		public SwVersionComparisonResult Status { get; set; }
	}
}
