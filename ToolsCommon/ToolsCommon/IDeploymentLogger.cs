using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000063 RID: 99
	public interface IDeploymentLogger
	{
		// Token: 0x06000263 RID: 611
		void Log(LoggingLevel level, string format, params object[] list);

		// Token: 0x06000264 RID: 612
		void LogException(Exception exp);

		// Token: 0x06000265 RID: 613
		void LogException(Exception exp, LoggingLevel level);

		// Token: 0x06000266 RID: 614
		void LogDebug(string format, params object[] list);

		// Token: 0x06000267 RID: 615
		void LogInfo(string format, params object[] list);

		// Token: 0x06000268 RID: 616
		void LogWarning(string format, params object[] list);

		// Token: 0x06000269 RID: 617
		void LogError(string format, params object[] list);
	}
}
