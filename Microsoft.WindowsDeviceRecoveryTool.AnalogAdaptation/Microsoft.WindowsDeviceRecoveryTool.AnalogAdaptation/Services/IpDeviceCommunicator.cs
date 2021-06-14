using System;
using Microsoft.Tools.Connectivity;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x0200000A RID: 10
	internal class IpDeviceCommunicator
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000053B5 File Offset: 0x000035B5
		public IpDeviceCommunicator(Guid id)
		{
			this.device = new RemoteDevice(id);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000053CC File Offset: 0x000035CC
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

		// Token: 0x06000063 RID: 99 RVA: 0x00005458 File Offset: 0x00003658
		public void Disconnect()
		{
			this.device.Disconnect();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005468 File Offset: 0x00003668
		public string ExecuteCommand(IpDeviceCommand command, string args = null)
		{
			return command.Execute(this.device, args);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005488 File Offset: 0x00003688
		public int ReadBatteryLevel()
		{
			string text = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyBatteryLevel, null);
			try
			{
				return int.Parse(text);
			}
			catch
			{
				Tracer<IpDeviceCommunicator>.WriteError("Uncorrect format of battery level parameter {0}", new object[]
				{
					text
				});
			}
			return -1;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000054E0 File Offset: 0x000036E0
		public bool IsIpDevice()
		{
			bool result;
			try
			{
				this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyFirmwareVersion, null);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005518 File Offset: 0x00003718
		public bool GetDeviceProperties(ref IpDeviceCommunicator.DeviceProperties deviceProperties)
		{
			bool result;
			try
			{
				deviceProperties.FirmwareVersion = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyFirmwareVersion, null);
				deviceProperties.Name = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyOemDeviceName, null);
				deviceProperties.UefiName = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyUefiName, null);
				deviceProperties.BatteryLevel = this.ReadBatteryLevel();
				result = true;
			}
			catch (Exception error)
			{
				Tracer<IpDeviceCommunicator>.WriteError(error);
				result = false;
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000558C File Offset: 0x0000378C
		public string GetOSVersion()
		{
			string str = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyBuildNumber, null);
			string str2 = this.ExecuteCommand(IpDeviceCommunicator.DeviceUpdatePropertyBuildRevision, null);
			return "10.0." + str + "." + str2;
		}

		// Token: 0x04000029 RID: 41
		public const string ApplyUpdateDirectory = "C:\\Data\\ProgramData\\Update";

		// Token: 0x0400002A RID: 42
		public const string ApplyUpdateLogFile = "C:\\Data\\ProgramData\\Update\\ApplyUpdate.log";

		// Token: 0x0400002B RID: 43
		public const string UsoLogFile = "C:\\Data\\ProgramData\\USOShared\\UsoLogs.dudiag";

		// Token: 0x0400002C RID: 44
		public static readonly IpDeviceCommand DeviceUpdatePropertyFirmwareVersion = new IpDeviceDeviceUpdateUtilCommand("firmwareversion", "1");

		// Token: 0x0400002D RID: 45
		public static readonly IpDeviceCommand DeviceUpdatePropertyManufacturer = new IpDeviceDeviceUpdateUtilCommand("manufacturer", "2");

		// Token: 0x0400002E RID: 46
		public static readonly IpDeviceCommand DeviceUpdatePropertySerialNumber = new IpDeviceDeviceUpdateUtilCommand("serialnumber", "3");

		// Token: 0x0400002F RID: 47
		public static readonly IpDeviceCommand DeviceUpdatePropertyBuildBranch = new IpDeviceDeviceUpdateUtilCommand("buildbranch", "4");

		// Token: 0x04000030 RID: 48
		public static readonly IpDeviceCommand DeviceUpdatePropertyBuildNumber = new IpDeviceDeviceUpdateUtilCommand("buildnumber", "5");

		// Token: 0x04000031 RID: 49
		public static readonly IpDeviceCommand DeviceUpdatePropertyBuildTimestamp = new IpDeviceDeviceUpdateUtilCommand("buildtimestamp", "6");

		// Token: 0x04000032 RID: 50
		public static readonly IpDeviceCommand DeviceUpdatePropertyOemDeviceName = new IpDeviceDeviceUpdateUtilCommand("oemdevicename", "7");

		// Token: 0x04000033 RID: 51
		public static readonly IpDeviceCommand DeviceUpdatePropertyUefiName = new IpDeviceDeviceUpdateUtilCommand("uefiname", "8");

		// Token: 0x04000034 RID: 52
		public static readonly IpDeviceCommand DeviceUpdatePropertyBuildRevision = new IpDeviceDeviceUpdateUtilCommand("buildrevision", null);

		// Token: 0x04000035 RID: 53
		public static readonly IpDeviceCommand DeviceUpdatePropertyBatteryLevel = new IpDeviceDeviceUpdateUtilCommand("getbatterylevel", null);

		// Token: 0x04000036 RID: 54
		public static readonly IpDeviceCommand DeviceUpdateCommandRebootToUefi = new IpDeviceDeviceUpdateUtilCommand("reboottouefi", "4097");

		// Token: 0x04000037 RID: 55
		public static readonly IpDeviceCommand DeviceUpdateCommandSetTime = new IpDeviceDeviceUpdateUtilCommand("settime", "4098");

		// Token: 0x04000038 RID: 56
		public static readonly IpDeviceCommand DeviceUpdateCommandGetInstalledPackages = new IpDeviceDeviceUpdateUtilCommand("getinstalledpackages", null);

		// Token: 0x04000039 RID: 57
		private readonly RemoteDevice device;

		// Token: 0x0200000B RID: 11
		public struct DeviceProperties
		{
			// Token: 0x0400003A RID: 58
			public string FirmwareVersion;

			// Token: 0x0400003B RID: 59
			public string Name;

			// Token: 0x0400003C RID: 60
			public string UefiName;

			// Token: 0x0400003D RID: 61
			public int BatteryLevel;
		}
	}
}
