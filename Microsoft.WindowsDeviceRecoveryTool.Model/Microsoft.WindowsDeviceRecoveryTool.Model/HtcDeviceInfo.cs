using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200001D RID: 29
	public class HtcDeviceInfo
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00003790 File Offset: 0x00001990
		public HtcDeviceInfo(string mid, string cid, string path)
		{
			this.Mid = mid;
			this.Cid = cid;
			this.Path = path;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000037B3 File Offset: 0x000019B3
		public HtcDeviceInfo(string path)
		{
			this.Path = path;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000037C8 File Offset: 0x000019C8
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000037DF File Offset: 0x000019DF
		public string Mid { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000037E8 File Offset: 0x000019E8
		// (set) Token: 0x060000DF RID: 223 RVA: 0x000037FF File Offset: 0x000019FF
		public string Cid { get; set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00003808 File Offset: 0x00001A08
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000381F File Offset: 0x00001A1F
		public string Path { get; set; }
	}
}
