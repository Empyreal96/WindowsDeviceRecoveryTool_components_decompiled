using System;
using Microsoft.Tools.Connectivity;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x0200000C RID: 12
	public class IpDeviceDeviceUpdateUtilCommand : IpDeviceCommand
	{
		// Token: 0x0600006A RID: 106 RVA: 0x000056D1 File Offset: 0x000038D1
		public IpDeviceDeviceUpdateUtilCommand(string args, string secondaryArgs = null) : base("C:\\Windows\\System32\\DeviceUpdateUtil.exe", "DeviceUpdateUtil.exe", args)
		{
			this.secondaryArgs = secondaryArgs;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000056F0 File Offset: 0x000038F0
		private bool HasSecondaryArgs()
		{
			return this.secondaryArgs != null;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005710 File Offset: 0x00003910
		private string SecondaryArgs(string additionalArgs = null)
		{
			string result;
			if (string.IsNullOrEmpty(additionalArgs))
			{
				result = this.secondaryArgs;
			}
			else
			{
				result = string.Format("{0} {1}", this.secondaryArgs, additionalArgs);
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000574C File Offset: 0x0000394C
		public override string Execute(RemoteDevice device, string additionalArgs)
		{
			string result;
			try
			{
				result = this.Execute(device, additionalArgs, false);
			}
			catch (DeviceException ex)
			{
				if (!(ex.InnerException is InvalidOperationException))
				{
					throw;
				}
				result = this.Execute(device, additionalArgs, true);
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000057A0 File Offset: 0x000039A0
		private string Execute(RemoteDevice device, string additionalArgs, bool useSecondaryArgs)
		{
			string fullCommandString = base.GetFullCommandString(additionalArgs);
			string text;
			try
			{
				try
				{
					text = device.RunCommand(base.Command, useSecondaryArgs ? this.SecondaryArgs(additionalArgs) : base.Args(additionalArgs));
				}
				catch (OperationFailedException)
				{
					text = device.RunCommand(base.AlternateCommand, useSecondaryArgs ? this.SecondaryArgs(additionalArgs) : base.Args(additionalArgs));
				}
			}
			catch (Exception innerException)
			{
				throw new DeviceException(string.Format("Unexpected failure for command \"{0}\"", fullCommandString), innerException);
			}
			if (!text.Contains(";"))
			{
				throw new DeviceException(string.Format("Unexpected device response for command \"{0}\": {1}", fullCommandString, text));
			}
			int num;
			try
			{
				num = int.Parse(text.Substring(text.LastIndexOf(';') + 1));
			}
			catch (Exception innerException)
			{
				throw new DeviceException(string.Format("Unexpected status for command \"{0}\"\n{1}", fullCommandString), innerException);
			}
			if (num == 4317)
			{
				Exception ex = new InvalidOperationException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
				throw new DeviceException(ex.Message, ex);
			}
			if (num == 87)
			{
				Exception ex = new ArgumentException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
				throw new DeviceException(ex.Message, ex);
			}
			if (num != 0)
			{
				throw new DeviceException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
			}
			return text.Substring(0, text.LastIndexOf(';'));
		}

		// Token: 0x0400003E RID: 62
		private const string DeviceUpdateUtilPath = "C:\\Windows\\System32\\DeviceUpdateUtil.exe";

		// Token: 0x0400003F RID: 63
		private const string DeviceUpdateUtilAlternatePath = "DeviceUpdateUtil.exe";

		// Token: 0x04000040 RID: 64
		private const int DeviceUpdateStatusSuccess = 0;

		// Token: 0x04000041 RID: 65
		private const int DeviceUpdateStatusInvalidParameter = 87;

		// Token: 0x04000042 RID: 66
		private const int DeviceUpdateStatusInvalidOperation = 4317;

		// Token: 0x04000043 RID: 67
		private readonly string secondaryArgs;
	}
}
