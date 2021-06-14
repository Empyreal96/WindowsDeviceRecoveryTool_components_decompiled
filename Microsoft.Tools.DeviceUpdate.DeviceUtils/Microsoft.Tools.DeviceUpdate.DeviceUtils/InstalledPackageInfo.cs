using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x0200000A RID: 10
	public class InstalledPackageInfo
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002801 File Offset: 0x00000A01
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002809 File Offset: 0x00000A09
		public string Partition { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002812 File Offset: 0x00000A12
		// (set) Token: 0x06000046 RID: 70 RVA: 0x0000281A File Offset: 0x00000A1A
		public string Package { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002823 File Offset: 0x00000A23
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000282B File Offset: 0x00000A2B
		public Version Version { get; set; }

		// Token: 0x06000049 RID: 73 RVA: 0x00002834 File Offset: 0x00000A34
		public InstalledPackageInfo()
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000283C File Offset: 0x00000A3C
		public InstalledPackageInfo(string partition, string package, string versionString)
		{
			this.Partition = partition;
			this.Package = package;
			try
			{
				this.Version = Version.Parse(versionString);
			}
			catch
			{
				throw new DeviceException(string.Format("Invalid package version: {0} {1}", package, versionString));
			}
		}
	}
}
