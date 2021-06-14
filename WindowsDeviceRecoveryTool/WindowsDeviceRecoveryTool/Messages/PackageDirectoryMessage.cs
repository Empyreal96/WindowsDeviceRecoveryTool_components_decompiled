using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000060 RID: 96
	public class PackageDirectoryMessage
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		public PackageDirectoryMessage(string directory)
		{
			this.Directory = directory;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000FEE7 File Offset: 0x0000E0E7
		public string Directory { get; private set; }
	}
}
