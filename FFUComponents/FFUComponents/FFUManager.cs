using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Windows.Flashing.Platform;

namespace FFUComponents
{
	// Token: 0x0200001B RID: 27
	public static class FFUManager
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000081 RID: 129 RVA: 0x00003278 File Offset: 0x00001478
		// (remove) Token: 0x06000082 RID: 130 RVA: 0x000032AC File Offset: 0x000014AC
		public static event EventHandler<ConnectEventArgs> DeviceConnectEvent;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000083 RID: 131 RVA: 0x000032E0 File Offset: 0x000014E0
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x00003314 File Offset: 0x00001514
		public static event EventHandler<DisconnectEventArgs> DeviceDisconnectEvent;

		// Token: 0x06000085 RID: 133 RVA: 0x00003348 File Offset: 0x00001548
		internal static void DisconnectDevice(Guid id)
		{
			List<IFFUDeviceInternal> list = new List<IFFUDeviceInternal>(FFUManager.activeFFUDevices.Count);
			lock (FFUManager.activeFFUDevices)
			{
				for (int i = 0; i < FFUManager.activeFFUDevices.Count; i++)
				{
					if (FFUManager.activeFFUDevices[i].DeviceUniqueID == id)
					{
						list.Add(FFUManager.activeFFUDevices[i]);
						FFUManager.activeFFUDevices.RemoveAt(i);
					}
				}
			}
			foreach (IFFUDeviceInternal device in list)
			{
				FFUManager.disconnectTimer.StopTimer(device);
				FFUManager.OnDisconnect(device);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003428 File Offset: 0x00001628
		internal static void DisconnectDevice(SimpleIODevice deviceToRemove)
		{
			IFFUDeviceInternal iffudeviceInternal = null;
			lock (FFUManager.activeFFUDevices)
			{
				if (FFUManager.activeFFUDevices.Remove(deviceToRemove))
				{
					iffudeviceInternal = deviceToRemove;
				}
			}
			if (iffudeviceInternal != null)
			{
				FFUManager.disconnectTimer.StopTimer(iffudeviceInternal);
				FFUManager.OnDisconnect(iffudeviceInternal);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003488 File Offset: 0x00001688
		internal static void DisconnectDevice(ThorDevice deviceToRemove)
		{
			ThorDevice thorDevice = null;
			lock (FFUManager.activeFFUDevices)
			{
				if (FFUManager.activeFFUDevices.Remove(deviceToRemove))
				{
					thorDevice = deviceToRemove;
				}
			}
			if (thorDevice != null)
			{
				FFUManager.OnDisconnect(thorDevice);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000034DC File Offset: 0x000016DC
		private static bool DevicePresent(Guid id)
		{
			bool result = false;
			lock (FFUManager.activeFFUDevices)
			{
				for (int i = 0; i < FFUManager.activeFFUDevices.Count; i++)
				{
					if (FFUManager.activeFFUDevices[i].DeviceUniqueID == id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000354C File Offset: 0x0000174C
		private static void StartTimerIfNecessary(IFFUDeviceInternal device)
		{
			if (device.NeedsTimer())
			{
				DisconnectTimer disconnectTimer = FFUManager.disconnectTimer;
				if (disconnectTimer != null)
				{
					disconnectTimer.StartTimer(device);
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003571 File Offset: 0x00001771
		private static void OnConnect(IFFUDeviceInternal device)
		{
			if (device != null)
			{
				if (FFUManager.DeviceConnectEvent != null)
				{
					FFUManager.DeviceConnectEvent(null, new ConnectEventArgs(device));
				}
				FFUManager.HostLogger.EventWriteDevice_Attach(device.DeviceUniqueID, device.DeviceFriendlyName);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000035A8 File Offset: 0x000017A8
		private static void OnDisconnect(IFFUDeviceInternal device)
		{
			if (device != null && !FFUManager.DevicePresent(device.DeviceUniqueID))
			{
				if (FFUManager.DeviceDisconnectEvent != null)
				{
					FFUManager.DeviceDisconnectEvent(null, new DisconnectEventArgs(device.DeviceUniqueID));
				}
				FFUManager.HostLogger.EventWriteDevice_Remove(device.DeviceUniqueID, device.DeviceFriendlyName);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000361C File Offset: 0x0000181C
		private static void OnSimpleIoConnect(string usbDevicePath)
		{
			SimpleIODevice device = new SimpleIODevice(usbDevicePath);
			if (device.OnConnect(device))
			{
				IFFUDeviceInternal device3 = null;
				IFFUDeviceInternal device2 = null;
				lock (FFUManager.activeFFUDevices)
				{
					IFFUDeviceInternal iffudeviceInternal = FFUManager.activeFFUDevices.SingleOrDefault((IFFUDeviceInternal deviceInstance) => deviceInstance.DeviceUniqueID == device.DeviceUniqueID);
					IFFUDeviceInternal iffudeviceInternal2 = FFUManager.disconnectTimer.StopTimer(device);
					if (iffudeviceInternal == null && iffudeviceInternal2 != null)
					{
						FFUManager.activeFFUDevices.Add(iffudeviceInternal2);
						iffudeviceInternal = iffudeviceInternal2;
						device2 = iffudeviceInternal2;
					}
					if (iffudeviceInternal != null && !((SimpleIODevice)iffudeviceInternal).OnConnect(device))
					{
						FFUManager.activeFFUDevices.Remove(iffudeviceInternal);
						device3 = iffudeviceInternal;
						iffudeviceInternal = null;
					}
					if (iffudeviceInternal == null)
					{
						device2 = device;
						FFUManager.activeFFUDevices.Add(device);
					}
				}
				FFUManager.OnDisconnect(device3);
				FFUManager.OnConnect(device2);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003740 File Offset: 0x00001940
		private static void OnSimpleIoDisconnect(string usbDevicePath)
		{
			List<IFFUDeviceInternal> list = new List<IFFUDeviceInternal>();
			lock (FFUManager.activeFFUDevices)
			{
				if (usbDevicePath != null)
				{
					using (IEnumerator<IFFUDeviceInternal> enumerator = (from d in FFUManager.activeFFUDevices
					where d.UsbDevicePath.Equals(usbDevicePath, StringComparison.OrdinalIgnoreCase)
					select d).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IFFUDeviceInternal iffudeviceInternal = enumerator.Current;
							SimpleIODevice simpleIODevice = (SimpleIODevice)iffudeviceInternal;
							if (simpleIODevice != null && !simpleIODevice.IsConnected())
							{
								list.Add(simpleIODevice);
							}
						}
						goto IL_D7;
					}
				}
				foreach (IFFUDeviceInternal iffudeviceInternal2 in FFUManager.activeFFUDevices)
				{
					SimpleIODevice simpleIODevice2 = iffudeviceInternal2 as SimpleIODevice;
					if (simpleIODevice2 != null && !simpleIODevice2.IsConnected())
					{
						list.Add(iffudeviceInternal2);
					}
				}
				IL_D7:
				foreach (IFFUDeviceInternal iffudeviceInternal3 in list)
				{
					FFUManager.activeFFUDevices.Remove(iffudeviceInternal3);
					FFUManager.StartTimerIfNecessary(iffudeviceInternal3);
				}
			}
			foreach (IFFUDeviceInternal device in list)
			{
				FFUManager.OnDisconnect(device);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000038E8 File Offset: 0x00001AE8
		public static void OnThorConnect(string devicePath)
		{
			lock (FFUManager.activeFFUDevices)
			{
				string[] thorDevicePids = FFUManager.ThorDevicePids;
				int i = 0;
				while (i < thorDevicePids.Length)
				{
					string value = thorDevicePids[i];
					if (devicePath.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						ConnectedDevice connectedDevice = FFUManager.flashingPlatform.CreateConnectedDevice(devicePath);
						bool flag2 = connectedDevice.CanFlash();
						if (flag2)
						{
							FlashingDevice device = connectedDevice.CreateFlashingDevice();
							ThorDevice thorDevice = new ThorDevice(device, devicePath);
							FFUManager.activeFFUDevices.Add(thorDevice);
							FFUManager.OnConnect(thorDevice);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000039A8 File Offset: 0x00001BA8
		public static void OnThorDisconnect(string devicePath)
		{
			List<ThorDevice> list = new List<ThorDevice>();
			lock (FFUManager.activeFFUDevices)
			{
				if (devicePath != null)
				{
					using (IEnumerator<IFFUDeviceInternal> enumerator = (from d in FFUManager.activeFFUDevices
					where d.UsbDevicePath.Equals(devicePath, StringComparison.OrdinalIgnoreCase)
					select d).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IFFUDeviceInternal iffudeviceInternal = enumerator.Current;
							ThorDevice thorDevice = (ThorDevice)iffudeviceInternal;
							if (thorDevice != null)
							{
								list.Add(thorDevice);
							}
						}
						goto IL_C7;
					}
				}
				foreach (IFFUDeviceInternal iffudeviceInternal2 in FFUManager.activeFFUDevices)
				{
					ThorDevice thorDevice2 = iffudeviceInternal2 as ThorDevice;
					if (thorDevice2 != null)
					{
						list.Add(thorDevice2);
					}
				}
				IL_C7:
				foreach (ThorDevice item in list)
				{
					FFUManager.activeFFUDevices.Remove(item);
				}
			}
			foreach (ThorDevice device in list)
			{
				FFUManager.OnDisconnect(device);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003B38 File Offset: 0x00001D38
		static FFUManager()
		{
			FFUManager.activeFFUDevices = new List<IFFUDeviceInternal>();
			FFUManager.eventWatchers = new List<UsbEventWatcher>();
			FFUManager.HostLogger = new FlashingHostLogger();
			FFUManager.DeviceLogger = new FlashingDeviceLogger();
			string str = Process.GetCurrentProcess().ProcessName + Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture);
			string logFile = Path.Combine(Path.GetTempPath(), str + ".log");
			FFUManager.flashingPlatform = new FlashingPlatform(logFile);
			FFUManager.deviceNotification = null;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003C03 File Offset: 0x00001E03
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003C0A File Offset: 0x00001E0A
		internal static FlashingHostLogger HostLogger { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003C12 File Offset: 0x00001E12
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003C19 File Offset: 0x00001E19
		internal static FlashingDeviceLogger DeviceLogger { get; private set; }

		// Token: 0x06000095 RID: 149 RVA: 0x00003C24 File Offset: 0x00001E24
		public static void Start()
		{
			lock (FFUManager.eventWatchers)
			{
				if (!FFUManager.isStarted)
				{
					DeviceNotificationCallback deviceNotificationCallback = null;
					NotificationCallback callback = new NotificationCallback();
					FFUManager.flashingPlatform.RegisterDeviceNotificationCallback(callback, ref deviceNotificationCallback);
					FFUManager.deviceNotification = callback;
					FFUManager.disconnectTimer = new DisconnectTimer();
					if (FFUManager.eventWatchers.Count <= 0)
					{
						IUsbEventSink eventSink = new SimpleIoEventSink(new SimpleIoEventSink.ConnectHandler(FFUManager.OnSimpleIoConnect), new SimpleIoEventSink.DisconnectHandler(FFUManager.OnSimpleIoDisconnect));
						FFUManager.eventWatchers.Add(new UsbEventWatcher(eventSink, FFUManager.SimpleIOGuid, FFUManager.SimpleIOGuid));
						FFUManager.eventWatchers.Add(new UsbEventWatcher(eventSink, FFUManager.WinUSBClassGuid, FFUManager.WinUSBFlashingIfGuid));
					}
					FFUManager.isStarted = true;
				}
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003CFC File Offset: 0x00001EFC
		public static void Stop()
		{
			lock (FFUManager.eventWatchers)
			{
				if (FFUManager.isStarted)
				{
					FFUManager.eventWatchers.ForEach(delegate(UsbEventWatcher m)
					{
						m.Dispose();
					});
					FFUManager.eventWatchers.Clear();
					lock (FFUManager.activeFFUDevices)
					{
						FFUManager.activeFFUDevices.Clear();
					}
					DisconnectTimer disconnectTimer = Interlocked.Exchange<DisconnectTimer>(ref FFUManager.disconnectTimer, null);
					disconnectTimer.StopAllTimers();
					DeviceNotificationCallback deviceNotificationCallback = null;
					NotificationCallback callback = null;
					FFUManager.flashingPlatform.RegisterDeviceNotificationCallback(callback, ref deviceNotificationCallback);
					FFUManager.deviceNotification = callback;
					FFUManager.isStarted = false;
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003DD8 File Offset: 0x00001FD8
		public static ICollection<IFFUDevice> FlashableDevices
		{
			get
			{
				ICollection<IFFUDevice> result = new List<IFFUDevice>();
				FFUManager.GetFlashableDevices(ref result);
				return result;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public static void GetFlashableDevices(ref ICollection<IFFUDevice> devices)
		{
			lock (FFUManager.eventWatchers)
			{
				if (!FFUManager.isStarted)
				{
					throw new FFUManagerException(Resources.ERROR_FFUMANAGER_NOT_STARTED);
				}
				devices.Clear();
				lock (FFUManager.activeFFUDevices)
				{
					foreach (IFFUDeviceInternal item in FFUManager.activeFFUDevices)
					{
						devices.Add(item);
					}
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003EB4 File Offset: 0x000020B4
		public static IFFUDevice GetFlashableDevice(string instancePath, bool enableFallback)
		{
			SimpleIODevice simpleIODevice = new SimpleIODevice(instancePath);
			if (simpleIODevice.OnConnect(simpleIODevice))
			{
				return simpleIODevice;
			}
			if (enableFallback)
			{
				string fallbackInstancePath = FFUManager.GetFallbackInstancePath(instancePath);
				if (!string.IsNullOrEmpty(fallbackInstancePath))
				{
					simpleIODevice = new SimpleIODevice(fallbackInstancePath);
					if (simpleIODevice.OnConnect(simpleIODevice))
					{
						return simpleIODevice;
					}
				}
			}
			return null;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003EF8 File Offset: 0x000020F8
		private static string ReplaceUsbSerial(Match match)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = match.Groups["serial"].Value;
			if (Regex.IsMatch(value, "[a-f0-9]{32}", RegexOptions.IgnoreCase))
			{
				for (int i = 0; i < 8; i++)
				{
					stringBuilder.AppendFormat("{0}{1}", value.ElementAt(i + 1), value.ElementAt(i));
					i++;
				}
				stringBuilder.Append("-");
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 4; k++)
					{
						stringBuilder.AppendFormat("{0}{1}", value.ElementAt(8 + 4 * j + k + 1), value.ElementAt(8 + 4 * j + k));
						k++;
					}
					stringBuilder.Append("-");
				}
				for (int l = 0; l < 12; l++)
				{
					stringBuilder.AppendFormat("{0}{1}", value.ElementAt(20 + l + 1), value.ElementAt(20 + l));
					l++;
				}
				return match.ToString().Replace(value, stringBuilder.ToString());
			}
			if (Regex.IsMatch(value, "[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}", RegexOptions.IgnoreCase))
			{
				for (int m = 0; m < value.Length - 1; m++)
				{
					if (value.ElementAt(m) != '-')
					{
						stringBuilder.AppendFormat("{0}{1}", value.ElementAt(m + 1), value.ElementAt(m));
						m++;
					}
				}
				return match.ToString().Replace(value, stringBuilder.ToString());
			}
			return null;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000040A4 File Offset: 0x000022A4
		private static string GetFallbackInstancePath(string instancePath)
		{
			MatchEvaluator evaluator = new MatchEvaluator(FFUManager.ReplaceUsbSerial);
			return Regex.Replace(instancePath, "\\\\\\?\\\\usb#vid_[a-zA-Z0-9]{4}&pid_[a-zA-Z0-9]{4}#(?<serial>.+)#{[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}}\\z", evaluator, RegexOptions.IgnoreCase);
		}

		// Token: 0x04000035 RID: 53
		public static FlashingPlatform flashingPlatform;

		// Token: 0x04000036 RID: 54
		public static NotificationCallback deviceNotification;

		// Token: 0x04000039 RID: 57
		private static List<UsbEventWatcher> eventWatchers;

		// Token: 0x0400003A RID: 58
		private static IList<IFFUDeviceInternal> activeFFUDevices;

		// Token: 0x0400003B RID: 59
		private static DisconnectTimer disconnectTimer;

		// Token: 0x0400003C RID: 60
		private static bool isStarted = false;

		// Token: 0x0400003D RID: 61
		public static readonly Guid SimpleIOGuid = new Guid("{67EA0A90-FF06-417D-AB66-6676DCE879CD}");

		// Token: 0x0400003E RID: 62
		public static readonly Guid WinUSBClassGuid = new Guid("{88BAE032-5A81-49F0-BC3D-A4FF138216D6}");

		// Token: 0x0400003F RID: 63
		public static readonly Guid WinUSBFlashingIfGuid = new Guid("{82809DD0-51F5-11E1-B86C-0800200C9A66}");

		// Token: 0x04000040 RID: 64
		private static readonly string[] ThorDevicePids = new string[]
		{
			"pid_0658"
		};
	}
}
