using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200005F RID: 95
	public class FoundSoftwareVersionMessage
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x0000FE60 File Offset: 0x0000E060
		public FoundSoftwareVersionMessage(bool status, PackageFileInfo packageFileInfo)
		{
			this.Status = status;
			this.PackageFileInfo = packageFileInfo;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000FE7C File Offset: 0x0000E07C
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000FE93 File Offset: 0x0000E093
		public PackageFileInfo PackageFileInfo { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000FE9C File Offset: 0x0000E09C
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000FEB3 File Offset: 0x0000E0B3
		public bool Status { get; set; }
	}
}
