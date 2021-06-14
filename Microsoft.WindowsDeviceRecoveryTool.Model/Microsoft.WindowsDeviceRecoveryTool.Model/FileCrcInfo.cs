using System;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000046 RID: 70
	public class FileCrcInfo
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00004E6C File Offset: 0x0000306C
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00004E83 File Offset: 0x00003083
		public string FileName { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00004E8C File Offset: 0x0000308C
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00004EA3 File Offset: 0x000030A3
		public bool Optional { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00004EAC File Offset: 0x000030AC
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00004EC3 File Offset: 0x000030C3
		public string Directory { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00004ECC File Offset: 0x000030CC
		public string FilePath
		{
			get
			{
				return Path.Combine(this.Directory, this.FileName);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00004EF0 File Offset: 0x000030F0
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00004F07 File Offset: 0x00003107
		public string Crc { get; set; }
	}
}
