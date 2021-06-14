using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000024 RID: 36
	public class VplPackageFileInfo : PackageFileInfo
	{
		// Token: 0x06000128 RID: 296 RVA: 0x000043AC File Offset: 0x000025AC
		public VplPackageFileInfo(string path, VariantInfo variantInfo) : base(path)
		{
			base.Path = path;
			this.packageId = variantInfo.ProductCode;
			this.name = variantInfo.Name;
			this.softwareVersion = variantInfo.SoftwareVersion;
			this.akVersion = variantInfo.AkVersion;
			this.FfuFilePath = variantInfo.FfuFilePath;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00004408 File Offset: 0x00002608
		public override string PackageId
		{
			get
			{
				return this.packageId;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00004420 File Offset: 0x00002620
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00004438 File Offset: 0x00002638
		public override string SoftwareVersion
		{
			get
			{
				return this.softwareVersion;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00004450 File Offset: 0x00002650
		public override string AkVersion
		{
			get
			{
				return this.akVersion;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004468 File Offset: 0x00002668
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000447F File Offset: 0x0000267F
		public string FfuFilePath { get; private set; }

		// Token: 0x040000C2 RID: 194
		private readonly string packageId;

		// Token: 0x040000C3 RID: 195
		private readonly string name;

		// Token: 0x040000C4 RID: 196
		private readonly string softwareVersion;

		// Token: 0x040000C5 RID: 197
		private readonly string akVersion;
	}
}
