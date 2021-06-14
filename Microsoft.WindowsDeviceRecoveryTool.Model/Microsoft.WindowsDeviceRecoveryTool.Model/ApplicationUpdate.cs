using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000003 RID: 3
	public class ApplicationUpdate
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000273C File Offset: 0x0000093C
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002753 File Offset: 0x00000953
		public int AppId { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000275C File Offset: 0x0000095C
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002773 File Offset: 0x00000973
		public string Description { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000277C File Offset: 0x0000097C
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002793 File Offset: 0x00000993
		public string Version { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000279C File Offset: 0x0000099C
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000027B3 File Offset: 0x000009B3
		public string PackageUri { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027BC File Offset: 0x000009BC
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000027D3 File Offset: 0x000009D3
		public long Size { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000027DC File Offset: 0x000009DC
		public string PackageFileName
		{
			get
			{
				string result;
				try
				{
					Uri uri = new Uri(this.PackageUri);
					result = uri.Segments[uri.Segments.Length - 1];
				}
				catch
				{
					result = null;
				}
				return result;
			}
		}
	}
}
