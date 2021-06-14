using System;

namespace ClickerUtilityLibrary.Misc
{
	// Token: 0x02000008 RID: 8
	public interface ILogger
	{
		// Token: 0x0600003F RID: 63
		void LogDebug(string message);

		// Token: 0x06000040 RID: 64
		void LogInfo(string message);

		// Token: 0x06000041 RID: 65
		void LogError(string message);
	}
}
