using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000009 RID: 9
	public abstract class QuietCmdHandler : CmdHandler
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003BC0 File Offset: 0x00001DC0
		protected void SetLoggingVerbosity(IULogger logger)
		{
			if (this._cmdLineParser.GetSwitchAsBoolean("quiet"))
			{
				logger.SetLoggingLevel(LoggingLevel.Warning);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003BDB File Offset: 0x00001DDB
		protected void SetQuietCommand()
		{
			this._cmdLineParser.SetOptionalSwitchBoolean("quiet", "When set only errors and warnings will be logged.", false);
		}

		// Token: 0x04000025 RID: 37
		private const string QUIET_SWITCH_NAME = "quiet";
	}
}
