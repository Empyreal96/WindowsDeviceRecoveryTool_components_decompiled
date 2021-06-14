using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000021 RID: 33
	public class FfuPackageFileInfo : PackageFileInfo
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00003B1C File Offset: 0x00001D1C
		public FfuPackageFileInfo(string path, PlatformId platformId, string softwareVersion) : base(path)
		{
			this.softwareVersion = softwareVersion;
			this.PlatformId = platformId;
			this.akVersion = null;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00003B3E File Offset: 0x00001D3E
		public FfuPackageFileInfo(string path, PlatformId platformId, string softwareVersion, string akVersion) : base(path)
		{
			this.softwareVersion = softwareVersion;
			this.PlatformId = platformId;
			this.akVersion = akVersion;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00003B64 File Offset: 0x00001D64
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00003B7B File Offset: 0x00001D7B
		public PlatformId PlatformId { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00003B84 File Offset: 0x00001D84
		public override string PackageId
		{
			get
			{
				return this.PlatformId.ToString();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public override string Name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public override string SoftwareVersion
		{
			get
			{
				return this.softwareVersion;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public override string AkVersion
		{
			get
			{
				return this.akVersion;
			}
		}

		// Token: 0x040000AE RID: 174
		private readonly string softwareVersion;

		// Token: 0x040000AF RID: 175
		private readonly string akVersion;
	}
}
