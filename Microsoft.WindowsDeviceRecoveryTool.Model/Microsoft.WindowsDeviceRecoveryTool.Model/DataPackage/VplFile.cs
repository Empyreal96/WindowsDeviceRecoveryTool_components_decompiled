using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage
{
	// Token: 0x02000023 RID: 35
	public class VplFile
	{
		// Token: 0x0600011B RID: 283 RVA: 0x000042AB File Offset: 0x000024AB
		public VplFile(string name, string fileType, string fileSubtype, bool signed, bool optional, string crc)
		{
			this.Name = name;
			this.FileType = fileType;
			this.FileSubtype = fileSubtype;
			this.Signed = signed;
			this.Optional = optional;
			this.Crc = crc;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000042EC File Offset: 0x000024EC
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00004303 File Offset: 0x00002503
		public string Name { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000430C File Offset: 0x0000250C
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00004323 File Offset: 0x00002523
		public string FileType { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000432C File Offset: 0x0000252C
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00004343 File Offset: 0x00002543
		public string FileSubtype { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000434C File Offset: 0x0000254C
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00004363 File Offset: 0x00002563
		public bool Signed { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000436C File Offset: 0x0000256C
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00004383 File Offset: 0x00002583
		public bool Optional { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000438C File Offset: 0x0000258C
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000043A3 File Offset: 0x000025A3
		public string Crc { get; private set; }
	}
}
