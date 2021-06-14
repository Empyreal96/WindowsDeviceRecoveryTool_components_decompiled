using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Tools.Connectivity;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x0200000C RID: 12
	public class IpDeviceCommunicator : Disposable
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00003510 File Offset: 0x00001710
		public IpDeviceCommunicator(Guid id)
		{
			this.device = new RemoteDevice(id);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003524 File Offset: 0x00001724
		public void Connect()
		{
			try
			{
				this.device.UserName = "UpdateUser";
				this.device.Connect();
			}
			catch (AccessDeniedException)
			{
				this.device.UserName = "SshRecoveryUser";
				this.device.Connect();
			}
			try
			{
				this.device.CreateDirectory("C:\\Data\\ProgramData\\Update");
			}
			catch
			{
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000035A0 File Offset: 0x000017A0
		protected override void DisposeManaged()
		{
			try
			{
				this.device.Disconnect();
			}
			catch
			{
			}
			base.DisposeManaged();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000035D4 File Offset: 0x000017D4
		public string ExecuteCommand(IpDeviceCommunicator.IpDeviceCommand command, string args = null)
		{
			return command.Execute(this.device, args);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000035E4 File Offset: 0x000017E4
		public void PutFile(string remoteFilePath, string localFilePath)
		{
			try
			{
				this.device.PutFile(remoteFilePath, localFilePath, true);
			}
			catch (Exception innerException)
			{
				throw new DeviceException(string.Format("Unexpected failure when copying {0} to {1}", localFilePath, remoteFilePath), innerException);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003628 File Offset: 0x00001828
		public void GetFile(string remoteFilePath, string localFilePath)
		{
			try
			{
				this.device.GetFile(remoteFilePath, localFilePath, true);
			}
			catch (Exception innerException)
			{
				throw new DeviceException(string.Format("Unexpected failure when copying {0} to {1}", remoteFilePath, localFilePath), innerException);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000366C File Offset: 0x0000186C
		public void DeleteFile(string remoteFilePath)
		{
			try
			{
				try
				{
					this.device.DeleteFile(remoteFilePath);
				}
				catch (NotImplementedException)
				{
					this.device.RunCommand("cmd.exe", string.Format("/c del {0}", remoteFilePath));
				}
			}
			catch (Exception innerException)
			{
				throw new DeviceException(string.Format("Unexpected failure when deleting {0}", remoteFilePath), innerException);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000036D8 File Offset: 0x000018D8
		public bool IsIpDevice()
		{
			bool result;
			try
			{
				this.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_FIRMWARE_VERSION, null);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000370C File Offset: 0x0000190C
		public bool IsServicingSupported()
		{
			bool result;
			try
			{
				this.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_STATUS, null);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000024 RID: 36
		public const string APPLY_UPDATE_DIRECTORY = "C:\\Data\\ProgramData\\Update";

		// Token: 0x04000025 RID: 37
		public const string APPLY_UPDATE_LOG_FILE = "C:\\Data\\ProgramData\\Update\\ApplyUpdate.log";

		// Token: 0x04000026 RID: 38
		public const string USO_LOG_FILE = "C:\\Data\\ProgramData\\USOShared\\UsoLogs.dudiag";

		// Token: 0x04000027 RID: 39
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_FIRMWARE_VERSION = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("firmwareversion", "1");

		// Token: 0x04000028 RID: 40
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_MANUFACTURER = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("manufacturer", "2");

		// Token: 0x04000029 RID: 41
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_SERIAL_NUMBER = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("serialnumber", "3");

		// Token: 0x0400002A RID: 42
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_BUILD_BRANCH = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("buildbranch", "4");

		// Token: 0x0400002B RID: 43
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_BUILD_NUMBER = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("buildnumber", "5");

		// Token: 0x0400002C RID: 44
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_BUILD_TIMESTAMP = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("buildtimestamp", "6");

		// Token: 0x0400002D RID: 45
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_OEM_DEVICE_NAME = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("oemdevicename", "7");

		// Token: 0x0400002E RID: 46
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_UEFI_NAME = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("uefiname", "8");

		// Token: 0x0400002F RID: 47
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_PROPERTY_BUILD_REVISION = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("buildrevision", null);

		// Token: 0x04000030 RID: 48
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_COMMAND_REBOOT_TO_UEFI = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("reboottouefi", "4097");

		// Token: 0x04000031 RID: 49
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_COMMAND_SET_TIME = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("settime", "4098");

		// Token: 0x04000032 RID: 50
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_COMMAND_GET_INSTALLED_PACKAGES = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("getinstalledpackages", null);

		// Token: 0x04000033 RID: 51
		public static IpDeviceCommunicator.IpDeviceCommand DEVICE_UPDATE_COMMAND_GET_BATTERY_LEVEL = new IpDeviceCommunicator.IpDeviceDeviceUpdateUtilCommand("getbatterylevel", null);

		// Token: 0x04000034 RID: 52
		public static IpDeviceCommunicator.IpDeviceCommand APPLY_UPDATE_COMMAND_STAGE = new IpDeviceCommunicator.IpDeviceApplyUpdateCommand("-stage", true, false, null);

		// Token: 0x04000035 RID: 53
		public static IpDeviceCommunicator.IpDeviceCommand APPLY_UPDATE_COMMAND_COMMIT = new IpDeviceCommunicator.IpDeviceApplyUpdateCommand("-commit -timeout 1", true, false, null);

		// Token: 0x04000036 RID: 54
		public static IpDeviceCommunicator.IpDeviceCommand APPLY_UPDATE_COMMAND_CLEAR = new IpDeviceCommunicator.IpDeviceApplyUpdateCommand("-clear", true, false, null);

		// Token: 0x04000037 RID: 55
		public static IpDeviceCommunicator.IpDeviceCommand APPLY_UPDATE_COMMAND_STATUS = new IpDeviceCommunicator.IpDeviceApplyUpdateCommand("-status", false, true, new int?(5));

		// Token: 0x04000038 RID: 56
		public static IpDeviceCommunicator.IpDeviceCommand APPLY_UPDATE_COMMAND_COLLECT_LOGS = new IpDeviceCommunicator.IpDeviceApplyUpdateCommand("-collectlogs", true, false, null);

		// Token: 0x04000039 RID: 57
		private RemoteDevice device;

		// Token: 0x0200000D RID: 13
		public abstract class IpDeviceCommand
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600008E RID: 142 RVA: 0x000038C4 File Offset: 0x00001AC4
			// (set) Token: 0x0600008F RID: 143 RVA: 0x000038CC File Offset: 0x00001ACC
			private protected string Command { protected get; private set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x06000090 RID: 144 RVA: 0x000038D5 File Offset: 0x00001AD5
			// (set) Token: 0x06000091 RID: 145 RVA: 0x000038DD File Offset: 0x00001ADD
			private protected string AlternateCommand { protected get; private set; }

			// Token: 0x06000092 RID: 146 RVA: 0x000038E6 File Offset: 0x00001AE6
			public IpDeviceCommand(string command, string alternateCommand, string args)
			{
				this.Command = command;
				this.AlternateCommand = alternateCommand;
				this.args = args;
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003903 File Offset: 0x00001B03
			protected string Args(string additionalArgs = null)
			{
				if (string.IsNullOrEmpty(additionalArgs))
				{
					return this.args;
				}
				return string.Format("{0} {1}", this.args, additionalArgs);
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003925 File Offset: 0x00001B25
			protected string GetFullCommandString(string additionalArgs = null)
			{
				return string.Format("{0} {1}", this.Command, this.Args(additionalArgs));
			}

			// Token: 0x06000095 RID: 149
			public abstract string Execute(RemoteDevice device, string additionalArgs);

			// Token: 0x0400003A RID: 58
			private string args;
		}

		// Token: 0x0200000E RID: 14
		public class IpDeviceDeviceUpdateUtilCommand : IpDeviceCommunicator.IpDeviceCommand
		{
			// Token: 0x06000096 RID: 150 RVA: 0x0000393E File Offset: 0x00001B3E
			public IpDeviceDeviceUpdateUtilCommand(string args, string secondaryArgs = null) : base("C:\\Windows\\System32\\DeviceUpdateUtil.exe", "DeviceUpdateUtil.exe", args)
			{
				this.secondaryArgs = secondaryArgs;
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00003958 File Offset: 0x00001B58
			private bool HasSecondaryArgs()
			{
				return this.secondaryArgs != null;
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00003966 File Offset: 0x00001B66
			private string SecondaryArgs(string additionalArgs = null)
			{
				if (string.IsNullOrEmpty(additionalArgs))
				{
					return this.secondaryArgs;
				}
				return string.Format("{0} {1}", this.secondaryArgs, additionalArgs);
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00003988 File Offset: 0x00001B88
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

			// Token: 0x0600009A RID: 154 RVA: 0x000039D0 File Offset: 0x00001BD0
			private string Execute(RemoteDevice device, string additionalArgs, bool useSecondaryArgs)
			{
				string fullCommandString = base.GetFullCommandString(additionalArgs);
				string text = null;
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
				if (!text.Contains(';'))
				{
					throw new DeviceException(string.Format("Unexpected device response for command \"{0}\": {1}", fullCommandString, text));
				}
				int num;
				try
				{
					num = int.Parse(text.Substring(text.LastIndexOf(';') + 1));
				}
				catch (Exception innerException2)
				{
					throw new DeviceException(string.Format("Unexpected status for command \"{0}\"\n{1}", fullCommandString), innerException2);
				}
				if (num == 4317)
				{
					Exception ex = new InvalidOperationException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
					throw new DeviceException(ex.Message, ex);
				}
				if (num == 87)
				{
					Exception ex2 = new ArgumentException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
					throw new DeviceException(ex2.Message, ex2);
				}
				if (num != 0)
				{
					throw new DeviceException(string.Format("Command \"{0}\" failed with status {1}", fullCommandString, num));
				}
				return text.Substring(0, text.LastIndexOf(';'));
			}

			// Token: 0x0400003D RID: 61
			private const string DEVICE_UPDATE_UTIL_PATH = "C:\\Windows\\System32\\DeviceUpdateUtil.exe";

			// Token: 0x0400003E RID: 62
			private const string DEVICE_UPDATE_UTIL_ALTERNATE_PATH = "DeviceUpdateUtil.exe";

			// Token: 0x0400003F RID: 63
			private const int DEVICE_UPDATE_STATUS_SUCCESS = 0;

			// Token: 0x04000040 RID: 64
			private const int DEVICE_UPDATE_STATUS_INVALID_PARAMETER = 87;

			// Token: 0x04000041 RID: 65
			private const int DEVICE_UPDATE_STATUS_INVALID_OPERATION = 4317;

			// Token: 0x04000042 RID: 66
			private string secondaryArgs;
		}

		// Token: 0x0200000F RID: 15
		public class IpDeviceApplyUpdateCommand : IpDeviceCommunicator.IpDeviceCommand
		{
			// Token: 0x0600009B RID: 155 RVA: 0x00003B38 File Offset: 0x00001D38
			public IpDeviceApplyUpdateCommand(string args, bool log = true, bool ignoreFailure = false, int? timeoutSeconds = null) : base("C:\\Windows\\System32\\ApplyUpdate.exe", "ApplyUpdate.exe", log ? string.Format("-log {0} {1}", "C:\\Data\\ProgramData\\Update\\ApplyUpdate.log", args) : args)
			{
				this.ignoreFailure = ignoreFailure;
				this.timeoutSeconds = timeoutSeconds;
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00003B70 File Offset: 0x00001D70
			public override string Execute(RemoteDevice device, string additionalArgs)
			{
				string fullCommandString = base.GetFullCommandString(additionalArgs);
				string text = null;
				int num = 0;
				try
				{
					try
					{
						RemoteCommand remoteCommand = device.Command(base.Command, base.Args(additionalArgs));
						remoteCommand.CaptureOutput = true;
						if (this.timeoutSeconds != null)
						{
							remoteCommand.Timeout = TimeSpan.FromSeconds((double)this.timeoutSeconds.Value);
						}
						num = remoteCommand.Execute();
						text = remoteCommand.Output;
					}
					catch (OperationFailedException)
					{
						RemoteCommand remoteCommand2 = device.Command(base.AlternateCommand, base.Args(additionalArgs));
						remoteCommand2.CaptureOutput = true;
						if (this.timeoutSeconds != null)
						{
							remoteCommand2.Timeout = TimeSpan.FromSeconds((double)this.timeoutSeconds.Value);
						}
						num = remoteCommand2.Execute();
						text = remoteCommand2.Output;
					}
				}
				catch (Exception innerException)
				{
					throw new DeviceException(string.Format("Unexpected failure for command \"{0}\"", fullCommandString), innerException);
				}
				if (!this.ignoreFailure && num != 0)
				{
					string arg;
					string text2;
					string text3;
					bool flag = IpDeviceCommunicator.IpDeviceApplyUpdateCommand.ParseResponse(text, out arg, out text2, out text3);
					if (flag)
					{
						throw new DeviceException(string.Format("Command \"{0}\" failed\n{1}", fullCommandString, arg));
					}
				}
				return text;
			}

			// Token: 0x0600009D RID: 157 RVA: 0x00003C94 File Offset: 0x00001E94
			public static bool ParseResponse(string response, out string errorLine, out string updateState, out string updateProgress)
			{
				string[] array = response.Split(new string[]
				{
					"\r\n",
					"\n"
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = "Unknown failure";
				bool flag = false;
				errorLine = "";
				updateState = "";
				updateProgress = "";
				foreach (string text2 in array)
				{
					if (text2.StartsWith("INFO: UpdateState"))
					{
						updateState = text2.Split(new string[]
						{
							"INFO: "
						}, StringSplitOptions.RemoveEmptyEntries)[0];
					}
					else if (text2.StartsWith("INFO: ProgressStateInstall"))
					{
						Regex regex = new Regex("\\d+", RegexOptions.IgnoreCase);
						Match match = regex.Match(text2);
						if (match.Success)
						{
							updateProgress = match.Value;
						}
					}
					else if (text2.StartsWith("ERROR:"))
					{
						if (IpDeviceCommunicator.IpDeviceApplyUpdateCommand.StatusFailed(text2))
						{
							flag = true;
						}
						else
						{
							text = text2.Split(new string[]
							{
								"ERROR: "
							}, StringSplitOptions.RemoveEmptyEntries)[0];
						}
					}
				}
				if (flag)
				{
					errorLine = text;
				}
				return flag;
			}

			// Token: 0x0600009E RID: 158 RVA: 0x00003DA8 File Offset: 0x00001FA8
			private static bool StatusFailed(string line)
			{
				return line.Contains("Cannot proceed with updates at this time") || line.Contains("Staging Failed") || line.Contains("Commit Failed") || line.Contains("Clear Failed") || line.Contains("Incorrect arguments");
			}

			// Token: 0x04000043 RID: 67
			private const string APPLY_UPDATE_PATH = "C:\\Windows\\System32\\ApplyUpdate.exe";

			// Token: 0x04000044 RID: 68
			private const string APPLY_UPDATE_ALTERNATE_PATH = "ApplyUpdate.exe";

			// Token: 0x04000045 RID: 69
			private const int APPLY_UPDATE_STATUS_SUCCESS = 0;

			// Token: 0x04000046 RID: 70
			private bool ignoreFailure;

			// Token: 0x04000047 RID: 71
			private int? timeoutSeconds;
		}
	}
}
