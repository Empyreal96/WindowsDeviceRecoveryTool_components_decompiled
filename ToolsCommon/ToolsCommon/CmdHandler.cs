using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000008 RID: 8
	public abstract class CmdHandler
	{
		// Token: 0x06000054 RID: 84
		protected abstract int DoExecution();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000055 RID: 85
		public abstract string Command { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000056 RID: 86
		public abstract string Description { get; }

		// Token: 0x06000057 RID: 87 RVA: 0x00003B5C File Offset: 0x00001D5C
		public int Execute(string cmdParams, string applicationName)
		{
			if (!this._cmdLineParser.ParseString("appName " + cmdParams, true))
			{
				string appName = applicationName + " " + this.Command;
				LogUtil.Message(this._cmdLineParser.UsageString(appName));
				return -1;
			}
			return this.DoExecution();
		}

		// Token: 0x04000024 RID: 36
		protected CommandLineParser _cmdLineParser = new CommandLineParser();
	}
}
