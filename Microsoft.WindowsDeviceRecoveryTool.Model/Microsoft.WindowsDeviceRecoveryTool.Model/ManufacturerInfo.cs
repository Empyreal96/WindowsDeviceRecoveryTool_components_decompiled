using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200004A RID: 74
	public class ManufacturerInfo
	{
		// Token: 0x06000203 RID: 515 RVA: 0x0000605C File Offset: 0x0000425C
		public ManufacturerInfo(PhoneTypes type, bool recoverySupport, string manufacturerName, byte[] imageData, string reportName, string reportProductLine)
		{
			this.Type = type;
			this.RecoverySupport = recoverySupport;
			this.Name = manufacturerName;
			this.ImageData = imageData;
			this.ReportManufacturerName = reportName;
			this.ReportProductLine = reportProductLine;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000609C File Offset: 0x0000429C
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000060B3 File Offset: 0x000042B3
		public PhoneTypes Type { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000060BC File Offset: 0x000042BC
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000060D3 File Offset: 0x000042D3
		public bool RecoverySupport { get; private set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000060DC File Offset: 0x000042DC
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000060F3 File Offset: 0x000042F3
		public string Name { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000060FC File Offset: 0x000042FC
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00006113 File Offset: 0x00004313
		public byte[] ImageData { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000611C File Offset: 0x0000431C
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00006133 File Offset: 0x00004333
		public string ReportManufacturerName { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000613C File Offset: 0x0000433C
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00006153 File Offset: 0x00004353
		public string ReportProductLine { get; set; }
	}
}
