using System;
using System.Collections.Generic;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000009 RID: 9
	public class MsrPackageInfo : PackageFileInfo
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002D48 File Offset: 0x00000F48
		public MsrPackageInfo(string path) : base(path)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002D54 File Offset: 0x00000F54
		public MsrPackageInfo(string packageId, string name, string softwareVersion) : base(string.Empty)
		{
			this.packageId = packageId;
			this.name = name;
			this.softwareVersion = softwareVersion;
			this.akVersion = null;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002D80 File Offset: 0x00000F80
		public MsrPackageInfo(string packageId, string name, string softwareVersion, string akVersion) : base(string.Empty)
		{
			this.packageId = packageId;
			this.name = name;
			this.softwareVersion = softwareVersion;
			this.akVersion = akVersion;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002DB0 File Offset: 0x00000FB0
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002DC7 File Offset: 0x00000FC7
		public string Id { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public override string PackageId
		{
			get
			{
				return this.packageId;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002E00 File Offset: 0x00001000
		public override string SoftwareVersion
		{
			get
			{
				return this.softwareVersion;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002E18 File Offset: 0x00001018
		public override string AkVersion
		{
			get
			{
				return this.akVersion;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002E30 File Offset: 0x00001030
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002E47 File Offset: 0x00001047
		public IEnumerable<MsrPackageInfo.MsrFileInfo> PackageFileData { get; set; }

		// Token: 0x04000029 RID: 41
		private readonly string packageId;

		// Token: 0x0400002A RID: 42
		private readonly string name;

		// Token: 0x0400002B RID: 43
		private readonly string softwareVersion;

		// Token: 0x0400002C RID: 44
		private readonly string akVersion;

		// Token: 0x0200000A RID: 10
		public class MsrFileInfo
		{
			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000068 RID: 104 RVA: 0x00002E50 File Offset: 0x00001050
			// (set) Token: 0x06000069 RID: 105 RVA: 0x00002E67 File Offset: 0x00001067
			public string FileName { get; set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x0600006A RID: 106 RVA: 0x00002E70 File Offset: 0x00001070
			// (set) Token: 0x0600006B RID: 107 RVA: 0x00002E87 File Offset: 0x00001087
			public string FileType { get; set; }

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x0600006C RID: 108 RVA: 0x00002E90 File Offset: 0x00001090
			// (set) Token: 0x0600006D RID: 109 RVA: 0x00002EA7 File Offset: 0x000010A7
			public string FileNameWithRevision { get; set; }

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x0600006E RID: 110 RVA: 0x00002EB0 File Offset: 0x000010B0
			// (set) Token: 0x0600006F RID: 111 RVA: 0x00002EC7 File Offset: 0x000010C7
			public string FileVersion { get; set; }
		}
	}
}
