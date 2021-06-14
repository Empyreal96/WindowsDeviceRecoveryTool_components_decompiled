using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr
{
	// Token: 0x0200001C RID: 28
	internal class MsrServiceData
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006CC8 File Offset: 0x00004EC8
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00006CDF File Offset: 0x00004EDF
		public string ApiUrl { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006CE8 File Offset: 0x00004EE8
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00006CFF File Offset: 0x00004EFF
		public string UploadApiUrl { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006D08 File Offset: 0x00004F08
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00006D1F File Offset: 0x00004F1F
		public string UserAgent { get; set; }

		// Token: 0x06000113 RID: 275 RVA: 0x00006D28 File Offset: 0x00004F28
		public static MsrServiceData CreateServiceData()
		{
			return new MsrServiceData
			{
				ApiUrl = "https://api.swrepository.com/rest-api/",
				UploadApiUrl = "https://api.swrepository.com/rest-api/report/1/uploadlocation",
				UserAgent = "Microsoft-Windows Device Recovery Tool"
			};
		}
	}
}
