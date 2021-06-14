using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000D RID: 13
	internal static class TimeStampUtility
	{
		// Token: 0x06000088 RID: 136 RVA: 0x000031FC File Offset: 0x000013FC
		public static long CreateTimeStamp(DateTime date)
		{
			return (long)(date - TimeStampUtility.UnixTimeStampRefTime).TotalMilliseconds;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003224 File Offset: 0x00001424
		public static long CreateTimeStamp()
		{
			return TimeStampUtility.CreateTimeStamp(DateTime.UtcNow);
		}

		// Token: 0x04000027 RID: 39
		private static readonly DateTime UnixTimeStampRefTime = new DateTime(1970, 1, 1, 0, 0, 0);
	}
}
