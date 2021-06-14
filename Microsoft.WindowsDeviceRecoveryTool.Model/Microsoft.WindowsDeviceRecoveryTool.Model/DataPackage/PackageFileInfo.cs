using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000008 RID: 8
	public abstract class PackageFileInfo
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00002CB4 File Offset: 0x00000EB4
		protected PackageFileInfo(string path)
		{
			this.Path = path;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002CC8 File Offset: 0x00000EC8
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002CDF File Offset: 0x00000EDF
		public string Path { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002CE8 File Offset: 0x00000EE8
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002CFF File Offset: 0x00000EFF
		public string SalesName { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002D08 File Offset: 0x00000F08
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002D1F File Offset: 0x00000F1F
		public string ManufacturerModelName { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002D28 File Offset: 0x00000F28
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002D3F File Offset: 0x00000F3F
		public bool OfflinePackage { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000059 RID: 89
		public abstract string PackageId { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600005A RID: 90
		public abstract string Name { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600005B RID: 91
		public abstract string SoftwareVersion { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600005C RID: 92
		public abstract string AkVersion { get; }
	}
}
