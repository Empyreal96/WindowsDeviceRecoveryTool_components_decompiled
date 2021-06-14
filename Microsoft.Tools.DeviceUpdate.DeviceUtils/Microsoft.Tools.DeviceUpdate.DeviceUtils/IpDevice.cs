using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Deployment.Compression.Cab;
using Microsoft.Tools.Connectivity;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x0200000B RID: 11
	public class IpDevice : Disposable, IIpDevice, IUpdateableDevice, IDevicePropertyCollection, IDisposable
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600004B RID: 75 RVA: 0x00002890 File Offset: 0x00000A90
		// (remove) Token: 0x0600004C RID: 76 RVA: 0x000028C8 File Offset: 0x00000AC8
		public event MessageHandler NormalMessageEvent;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600004D RID: 77 RVA: 0x00002900 File Offset: 0x00000B00
		// (remove) Token: 0x0600004E RID: 78 RVA: 0x00002938 File Offset: 0x00000B38
		public event MessageHandler WarningMessageEvent;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600004F RID: 79 RVA: 0x00002970 File Offset: 0x00000B70
		// (remove) Token: 0x06000050 RID: 80 RVA: 0x000029A8 File Offset: 0x00000BA8
		public event MessageHandler ProgressEvent;

		// Token: 0x06000051 RID: 81 RVA: 0x000029E0 File Offset: 0x00000BE0
		public IpDevice(DiscoveredDeviceInfo deviceInfo, IpDeviceCommunicator deviceCommunicator)
		{
			this.deviceInfo = deviceInfo;
			this.DeviceCommunicator = deviceCommunicator;
			this.isServicingSupported = deviceCommunicator.IsServicingSupported();
			this.DeviceUniqueId = deviceInfo.UniqueId;
			this.Manufacturer = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_MANUFACTURER, null);
			this.Branch = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_BUILD_BRANCH, null);
			this.CoreSysBuildNumber = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_BUILD_NUMBER, null);
			this.BuildTimeStamp = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_BUILD_TIMESTAMP, null);
			this.OemDeviceName = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_OEM_DEVICE_NAME, null);
			this.UefiName = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_UEFI_NAME, null);
			this.FirmwareVersion = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_FIRMWARE_VERSION, null);
			try
			{
				this.CoreSysBuildRevision = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_BUILD_REVISION, null);
			}
			catch
			{
				this.CoreSysBuildRevision = "?";
			}
			try
			{
				this.SerialNumber = deviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_PROPERTY_SERIAL_NUMBER, null);
			}
			catch
			{
				this.SerialNumber = "";
			}
			this.ImageTargetingType = "";
			this.FeedbackId = "";
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002B30 File Offset: 0x00000D30
		public virtual string GetProperty(string name)
		{
			return PropertyDeviceCollection.GetPropertyString(this, name);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002B39 File Offset: 0x00000D39
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002B41 File Offset: 0x00000D41
		public Guid DeviceUniqueId { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002B4A File Offset: 0x00000D4A
		public string Model
		{
			get
			{
				return this.deviceInfo.Name;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002B57 File Offset: 0x00000D57
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002B5F File Offset: 0x00000D5F
		public IpDeviceCommunicator DeviceCommunicator { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002B68 File Offset: 0x00000D68
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002B70 File Offset: 0x00000D70
		public virtual string Branch { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002B79 File Offset: 0x00000D79
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002B81 File Offset: 0x00000D81
		public virtual string CoreSysBuildNumber { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002B8A File Offset: 0x00000D8A
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002B92 File Offset: 0x00000D92
		public virtual string CoreSysBuildRevision { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002B9B File Offset: 0x00000D9B
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002BA3 File Offset: 0x00000DA3
		public virtual string BuildTimeStamp { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002BAC File Offset: 0x00000DAC
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public virtual string ImageTargetingType { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002BBD File Offset: 0x00000DBD
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002BC5 File Offset: 0x00000DC5
		public virtual string FeedbackId { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002BCE File Offset: 0x00000DCE
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002BD6 File Offset: 0x00000DD6
		public virtual string FirmwareVersion { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public string BuildString
		{
			get
			{
				return string.Format("{0}.{1}.{2}.{3}", new object[]
				{
					this.CoreSysBuildNumber,
					this.CoreSysBuildRevision,
					this.Branch,
					this.BuildTimeStamp
				});
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002C23 File Offset: 0x00000E23
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002C2B File Offset: 0x00000E2B
		public string SerialNumber { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002C34 File Offset: 0x00000E34
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002C3C File Offset: 0x00000E3C
		public string Manufacturer { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002C45 File Offset: 0x00000E45
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002C4D File Offset: 0x00000E4D
		public string OemDeviceName { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002C56 File Offset: 0x00000E56
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002C5E File Offset: 0x00000E5E
		public string UefiName { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002C67 File Offset: 0x00000E67
		public string BatteryLevel
		{
			get
			{
				return this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_COMMAND_GET_BATTERY_LEVEL, null);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002C7C File Offset: 0x00000E7C
		public string UpdateState
		{
			get
			{
				try
				{
					this.UpdateStatus();
				}
				catch
				{
				}
				return this.updateState;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002CAC File Offset: 0x00000EAC
		public string DuResult
		{
			get
			{
				try
				{
					this.UpdateStatus();
				}
				catch
				{
				}
				return this.duResult;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002CDC File Offset: 0x00000EDC
		private void UpdateStatus()
		{
			string response = this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_STATUS, null);
			string text;
			bool flag = IpDeviceCommunicator.IpDeviceApplyUpdateCommand.ParseResponse(response, out text, out this.updateState, out this.updateProgress);
			if (flag)
			{
				this.duResult = text;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002D1A File Offset: 0x00000F1A
		private void ClearStatus()
		{
			this.updateState = "";
			this.updateProgress = "";
			this.duResult = "";
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002D40 File Offset: 0x00000F40
		public void RebootToUefi()
		{
			try
			{
				this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_COMMAND_REBOOT_TO_UEFI, null);
			}
			catch
			{
				this.OnWarningMessageEvent("Error communicating with the device. To flash, please manually boot to FFU mode by power cycling and holding volume up.");
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002D80 File Offset: 0x00000F80
		public void RebootToTarget(uint target)
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002D87 File Offset: 0x00000F87
		public InstalledPackageInfo[] InstalledPackages
		{
			get
			{
				if (this.installedPackages == null)
				{
					this.installedPackages = this.GetInstalledPackages();
				}
				return this.installedPackages;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002DA4 File Offset: 0x00000FA4
		private InstalledPackageInfo[] GetInstalledPackages()
		{
			this.OnNormalMessageEvent("Retrieving list of installed packages...");
			string text = this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_COMMAND_GET_INSTALLED_PACKAGES, null);
			string[] array = text.Split(new char[]
			{
				';'
			});
			List<InstalledPackageInfo> list = new List<InstalledPackageInfo>();
			foreach (string text2 in array)
			{
				string[] array3 = text2.Split(new char[]
				{
					','
				});
				if (3 != array3.Length)
				{
					throw new DeviceException(string.Format("Package string has invalid format: {0}", text2));
				}
				InstalledPackageInfo item = new InstalledPackageInfo(array3[0], array3[1], array3[2]);
				list.Add(item);
			}
			this.OnNormalMessageEvent("Retrieved list of installed packages");
			if (list.Count == 0)
			{
				throw new DeviceException("Device package count is 0");
			}
			return list.ToArray();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002E76 File Offset: 0x00001076
		public void StartDeviceUpdateScan(uint throttle)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002E80 File Offset: 0x00001080
		public void InitiateDuInstall()
		{
			if (!this.isServicingSupported)
			{
				throw new ServicingNotSupportedException();
			}
			this.ClearStatus();
			this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_COMMIT, null);
			try
			{
				while (this.updateState == "" || this.updateState == "UpdateStateIdle")
				{
					if (!(this.duResult == ""))
					{
						break;
					}
					Thread.Sleep(1000);
					this.UpdateStatus();
				}
				while (this.DeviceCommunicator.IsIpDevice() && this.updateState != "UpdateStateIdle" && this.updateState != "UpdateStateUpdateComplete" && this.duResult == "")
				{
					if (this.updateProgress == "100")
					{
						this.OnProgressEvent("Completing remaining tasks before rebooting. This will take several minutes.");
					}
					else if (this.updateProgress != "")
					{
						this.OnProgressEvent(string.Format("Install progress: {0}", this.updateProgress));
					}
					Thread.Sleep(1000);
					this.UpdateStatus();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002FB0 File Offset: 0x000011B0
		public void ClearDuStagingDirectory()
		{
			if (!this.isServicingSupported)
			{
				throw new ServicingNotSupportedException();
			}
			this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_CLEAR, null);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002FD4 File Offset: 0x000011D4
		public void GetDuDiagnostics(string path)
		{
			this.OnNormalMessageEvent("Collecting log files...");
			CabInfo cabInfo = new CabInfo(path);
			List<string> list = new List<string>();
			string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
			try
			{
				try
				{
					this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_COLLECT_LOGS, null);
				}
				catch
				{
					this.OnWarningMessageEvent("Device does not support log collection");
					return;
				}
				string text2 = Path.Combine(Path.GetTempPath(), Path.GetFileName("C:\\Data\\ProgramData\\USOShared\\UsoLogs.dudiag"));
				this.DeviceCommunicator.GetFile("C:\\Data\\ProgramData\\USOShared\\UsoLogs.dudiag", text2);
				try
				{
					CabInfo cabInfo2 = new CabInfo(text2);
					cabInfo2.Unpack(text);
				}
				finally
				{
					File.Delete(text2);
				}
				try
				{
					string localFilePath = Path.Combine(text, Path.GetFileName("C:\\Data\\ProgramData\\Update\\ApplyUpdate.log"));
					this.DeviceCommunicator.GetFile("C:\\Data\\ProgramData\\Update\\ApplyUpdate.log", localFilePath);
				}
				catch (DeviceException ex)
				{
					if (!(ex.InnerException is FileNotFoundException))
					{
						throw;
					}
				}
				using (StreamWriter streamWriter = new StreamWriter(Path.Combine(text, "DeviceProperties.csv")))
				{
					SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
					PropertyDeviceCollection.GetProperties(this, ref sortedDictionary);
					foreach (KeyValuePair<string, string> keyValuePair in sortedDictionary)
					{
						streamWriter.WriteLine(string.Format("{0},{1}", keyValuePair.Key, keyValuePair.Value));
					}
				}
				foreach (FileInfo fileInfo in directoryInfo.GetFiles())
				{
					list.Add(fileInfo.FullName);
				}
				if (list.Count > 0)
				{
					this.OnNormalMessageEvent(string.Format("Copying log files to {0}", path));
					cabInfo.PackFiles(null, list, null);
				}
				else
				{
					this.OnWarningMessageEvent("No log files found");
				}
			}
			finally
			{
				directoryInfo.Delete(true);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000322C File Offset: 0x0000142C
		public void GetPackageInfo(string path)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003234 File Offset: 0x00001434
		public void SendIuPackage(string path)
		{
			if (!this.isServicingSupported)
			{
				throw new ServicingNotSupportedException();
			}
			string text = string.Format("{0}\\{1}", "C:\\Data\\ProgramData\\Update", Path.GetFileName(path));
			Regex regex = new Regex("^\\\\\\\\[\\w]");
			if (regex.IsMatch(path))
			{
				path = path.Insert(2, "?\\UNC\\");
			}
			this.DeviceCommunicator.PutFile(text, path);
			try
			{
				this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.APPLY_UPDATE_COMMAND_STAGE, text);
			}
			finally
			{
				this.DeviceCommunicator.DeleteFile(text);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000032C8 File Offset: 0x000014C8
		public void SendIuPackage(Stream stream)
		{
			string path = null;
			try
			{
				path = Path.GetTempFileName();
				using (FileStream fileStream = File.OpenWrite(path))
				{
					stream.CopyTo(fileStream);
				}
				this.SendIuPackage(path);
			}
			finally
			{
				File.Delete(path);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003324 File Offset: 0x00001524
		public void SetTime(DateTime time)
		{
			string args = string.Format("{0} {1} {2} {3} {4} {5} {6}", new object[]
			{
				time.Year,
				time.Month,
				time.Day,
				time.Hour,
				time.Minute,
				time.Second,
				time.Millisecond
			});
			try
			{
				this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_COMMAND_SET_TIME, args);
			}
			catch (DeviceException ex)
			{
				if (!(ex.InnerException is ArgumentException))
				{
					throw;
				}
				args = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", new object[]
				{
					time.Year,
					time.Month,
					time.DayOfWeek,
					time.Day,
					time.Hour,
					time.Minute,
					time.Second,
					time.Millisecond
				});
				this.DeviceCommunicator.ExecuteCommand(IpDeviceCommunicator.DEVICE_UPDATE_COMMAND_SET_TIME, args);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003488 File Offset: 0x00001688
		protected override void DisposeManaged()
		{
			try
			{
				this.DeviceCommunicator.Dispose();
			}
			catch
			{
			}
			base.DisposeManaged();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000034BC File Offset: 0x000016BC
		protected void OnProgressEvent(string message)
		{
			if (this.ProgressEvent != null)
			{
				this.ProgressEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000034D8 File Offset: 0x000016D8
		protected void OnNormalMessageEvent(string message)
		{
			if (this.NormalMessageEvent != null)
			{
				this.NormalMessageEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000034F4 File Offset: 0x000016F4
		protected void OnWarningMessageEvent(string message)
		{
			if (this.WarningMessageEvent != null)
			{
				this.WarningMessageEvent(this, new MessageArgs(message));
			}
		}

		// Token: 0x0400000E RID: 14
		private DiscoveredDeviceInfo deviceInfo;

		// Token: 0x0400000F RID: 15
		private string updateState = "";

		// Token: 0x04000010 RID: 16
		private string updateProgress = "";

		// Token: 0x04000011 RID: 17
		private string duResult = "";

		// Token: 0x04000012 RID: 18
		private InstalledPackageInfo[] installedPackages;

		// Token: 0x04000013 RID: 19
		private bool isServicingSupported;
	}
}
