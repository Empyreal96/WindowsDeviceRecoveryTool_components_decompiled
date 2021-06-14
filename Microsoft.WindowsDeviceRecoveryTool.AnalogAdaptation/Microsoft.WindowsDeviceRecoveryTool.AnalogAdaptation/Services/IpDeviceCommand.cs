using System;
using Microsoft.Tools.Connectivity;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x02000009 RID: 9
	public abstract class IpDeviceCommand
	{
		// Token: 0x06000059 RID: 89 RVA: 0x000052EE File Offset: 0x000034EE
		protected IpDeviceCommand(string command, string alternateCommand, string args)
		{
			this.Command = command;
			this.AlternateCommand = alternateCommand;
			this.args = args;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00005310 File Offset: 0x00003510
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00005327 File Offset: 0x00003527
		private protected string Command { protected get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00005330 File Offset: 0x00003530
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00005347 File Offset: 0x00003547
		private protected string AlternateCommand { protected get; private set; }

		// Token: 0x0600005E RID: 94 RVA: 0x00005350 File Offset: 0x00003550
		protected string Args(string additionalArgs = null)
		{
			string result;
			if (string.IsNullOrEmpty(additionalArgs))
			{
				result = this.args;
			}
			else
			{
				result = string.Format("{0} {1}", this.args, additionalArgs);
			}
			return result;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000538C File Offset: 0x0000358C
		protected string GetFullCommandString(string additionalArgs = null)
		{
			return string.Format("{0} {1}", this.Command, this.Args(additionalArgs));
		}

		// Token: 0x06000060 RID: 96
		public abstract string Execute(RemoteDevice device, string additionalArgs);

		// Token: 0x04000026 RID: 38
		private readonly string args;
	}
}
