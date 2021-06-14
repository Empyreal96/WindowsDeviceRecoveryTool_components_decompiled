using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000056 RID: 86
	public class CompatibleFfuFilesMessage
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x0000FC8C File Offset: 0x0000DE8C
		public CompatibleFfuFilesMessage(List<PackageFileInfo> packages)
		{
			this.Packages = packages;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000FCB7 File Offset: 0x0000DEB7
		public List<PackageFileInfo> Packages { get; set; }
	}
}
