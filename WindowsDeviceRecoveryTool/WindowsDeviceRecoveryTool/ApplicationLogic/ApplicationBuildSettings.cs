using System;

namespace Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic
{
	// Token: 0x02000006 RID: 6
	internal static class ApplicationBuildSettings
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003314 File Offset: 0x00001514
		public static DateTime ExpirationDate
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000332C File Offset: 0x0000152C
		public static bool SkipApplicationUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003340 File Offset: 0x00001540
		public static int ApplicationId
		{
			get
			{
				return 3900;
			}
		}
	}
}
