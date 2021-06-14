using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums
{
	// Token: 0x0200002D RID: 45
	public enum ReportOperationType
	{
		// Token: 0x0400010B RID: 267
		Flashing,
		// Token: 0x0400010C RID: 268
		Recovery,
		// Token: 0x0400010D RID: 269
		ReadDeviceInfo,
		// Token: 0x0400010E RID: 270
		ReadDeviceInfoWithThor,
		// Token: 0x0400010F RID: 271
		DownloadPackage,
		// Token: 0x04000110 RID: 272
		CheckPackage,
		// Token: 0x04000111 RID: 273
		EmergencyFlashing,
		// Token: 0x04000112 RID: 274
		RecoveryAfterEmergencyFlashing,
		// Token: 0x04000113 RID: 275
		ReadInfoAfterEmergencyFlashing,
		// Token: 0x04000114 RID: 276
		DownloadEmergencyPackage
	}
}
