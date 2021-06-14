using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200000A RID: 10
	public class MultiCmdHandler
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003BFC File Offset: 0x00001DFC
		private void ShowUsage()
		{
			LogUtil.Message("Usage: {0} <command> <parameters>", new object[]
			{
				this.appName
			});
			LogUtil.Message("\t available command:");
			foreach (KeyValuePair<string, CmdHandler> keyValuePair in this._allHandlers)
			{
				LogUtil.Message("\t\t{0}:{1}", new object[]
				{
					keyValuePair.Value.Command,
					keyValuePair.Value.Description
				});
			}
			LogUtil.Message("\t Run {0} <command> /? to check command line parameters for each command", new object[]
			{
				this.appName
			});
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003CBC File Offset: 0x00001EBC
		public void AddCmdHandler(CmdHandler cmdHandler)
		{
			this._allHandlers.Add(cmdHandler.Command, cmdHandler);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public int Run(string[] args)
		{
			if (args.Length < 1)
			{
				this.ShowUsage();
				return -1;
			}
			int result = -1;
			string cmdParams = (args.Length > 1) ? string.Join(" ", args.Skip(1)) : string.Empty;
			CmdHandler cmdHandler = null;
			if (!this._allHandlers.TryGetValue(args[0], out cmdHandler))
			{
				this.ShowUsage();
			}
			else
			{
				result = cmdHandler.Execute(cmdParams, this.appName);
			}
			return result;
		}

		// Token: 0x04000026 RID: 38
		private string appName = new FileInfo(Environment.GetCommandLineArgs()[0]).Name;

		// Token: 0x04000027 RID: 39
		private Dictionary<string, CmdHandler> _allHandlers = new Dictionary<string, CmdHandler>(StringComparer.OrdinalIgnoreCase);
	}
}
