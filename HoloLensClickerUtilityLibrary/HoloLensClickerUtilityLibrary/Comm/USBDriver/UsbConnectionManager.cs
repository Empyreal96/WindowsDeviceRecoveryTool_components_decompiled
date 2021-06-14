using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000038 RID: 56
	internal class UsbConnectionManager
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007DC7 File Offset: 0x00005FC7
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00007DCF File Offset: 0x00005FCF
		public UsbDevice Device { get; private set; }

		// Token: 0x06000148 RID: 328 RVA: 0x00007DD8 File Offset: 0x00005FD8
		public UsbConnectionManager(OnDeviceConnect deviceConnectCallback, OnDeviceDisconnect deviceDisconnectCallback, UsbDevice device = null)
		{
			bool flag = deviceConnectCallback == null;
			if (flag)
			{
				throw new ArgumentNullException("deviceConnectCallback");
			}
			bool flag2 = deviceDisconnectCallback == null;
			if (flag2)
			{
				throw new ArgumentNullException("deviceDisconnectCallback");
			}
			this.deviceConnectCallback = deviceConnectCallback;
			this.deviceDisconnectCallback = deviceDisconnectCallback;
			this.Device = device;
			this.mDeviceState = UsbConnectionManager.DeviceState.Disconnected;
			this.mCmNotifyCallback = new CfgMgr.CM_NOTIFY_CALLBACK(this.UsbNotificationCallback);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007E44 File Offset: 0x00006044
		~UsbConnectionManager()
		{
			this.Dispose(false);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007E78 File Offset: 0x00006078
		private void Dispose(bool Disposing)
		{
			if (Disposing)
			{
				this.Stop();
				this.mCmNotifyCallback = null;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007E9B File Offset: 0x0000609B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007EA8 File Offset: 0x000060A8
		public void Start()
		{
			bool flag = !this.mConnectionManagerStarted;
			if (flag)
			{
				this.DiscoverUsbDevices();
				CfgMgr.CM_NOTIFY_FILTER cm_NOTIFY_FILTER = new CfgMgr.CM_NOTIFY_FILTER();
				cm_NOTIFY_FILTER.cbSize = (uint)Marshal.SizeOf(cm_NOTIFY_FILTER);
				cm_NOTIFY_FILTER.FilterType = CfgMgr.CM_NOTIFY_FILTER_TYPE.CM_NOTIFY_FILTER_TYPE_DEVICEINTERFACE;
				cm_NOTIFY_FILTER.Flags = (CfgMgr.CM_NOTIFY_FILTER_FLAGS)0U;
				cm_NOTIFY_FILTER.Reserved = 0U;
				cm_NOTIFY_FILTER.u.DeviceHandle.hTarget = IntPtr.Zero;
				cm_NOTIFY_FILTER.u.DeviceInterface.ClassGuid = UsbConnectionManager.UsbIfGuid;
				uint num = CfgMgr.CM_Register_Notification(cm_NOTIFY_FILTER, IntPtr.Zero, this.mCmNotifyCallback, out this.mNotifyContext);
				bool flag2 = num > 0U;
				if (flag2)
				{
					throw new Exception("Unable to register for USB connection and disconnection notifications.");
				}
				this.mConnectionManagerStarted = true;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007F54 File Offset: 0x00006154
		public void Stop()
		{
			bool flag = this.mConnectionManagerStarted;
			if (flag)
			{
				bool flag2 = this.mNotifyContext != UIntPtr.Zero;
				if (flag2)
				{
					uint num = CfgMgr.CM_Unregister_Notification(this.mNotifyContext);
					bool flag3 = num > 0U;
					if (flag3)
					{
						throw new Exception("Unable to unregister for USB connection and disconnection notifications.");
					}
				}
				this.mConnectionManagerStarted = false;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007FAC File Offset: 0x000061AC
		private void DiscoverUsbDevices()
		{
			bool flag = this.Device == null;
			if (flag)
			{
				UsbDevices usbDevices = new UsbDevices();
				bool flag2 = usbDevices.Devices.Count > 0;
				if (flag2)
				{
					this.Device = usbDevices.Devices.First<KeyValuePair<string, UsbDevice>>().Value;
					string deviceInterfaceSymbolicLinkName = this.Device.DeviceInterfaceSymbolicLinkName;
					this.OnUsbDeviceConnect(deviceInterfaceSymbolicLinkName);
				}
			}
			else
			{
				this.OnUsbDeviceConnect(this.Device.DeviceInterfaceSymbolicLinkName);
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008028 File Offset: 0x00006228
		private void OnUsbDeviceConnect(string deviceInterfaceSymbolicLinkName)
		{
			bool flag = this.Device == null;
			if (flag)
			{
				UsbDevices usbDevices = new UsbDevices();
				string key = deviceInterfaceSymbolicLinkName.ToUpper(CultureInfo.InvariantCulture);
				bool flag2 = usbDevices.Devices.ContainsKey(key);
				if (!flag2)
				{
					return;
				}
				this.Device = usbDevices.Devices[key];
			}
			bool flag3 = string.Equals(deviceInterfaceSymbolicLinkName, this.Device.DeviceInterfaceSymbolicLinkName, StringComparison.CurrentCultureIgnoreCase);
			bool flag4 = flag3 && this.mDeviceState == UsbConnectionManager.DeviceState.Disconnected;
			if (flag4)
			{
				this.mDeviceState = UsbConnectionManager.DeviceState.Connected;
				this.deviceConnectCallback(this.Device.DeviceInterfaceSymbolicLinkName);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000080CC File Offset: 0x000062CC
		private void OnUsbDeviceDisconnect(string deviceInterfaceSymbolicLinkName)
		{
			bool flag = this.Device == null;
			if (!flag)
			{
				bool flag2 = string.Equals(deviceInterfaceSymbolicLinkName, this.Device.DeviceInterfaceSymbolicLinkName, StringComparison.CurrentCultureIgnoreCase);
				bool flag3 = flag2 && this.mDeviceState == UsbConnectionManager.DeviceState.Connected;
				if (flag3)
				{
					this.deviceDisconnectCallback(deviceInterfaceSymbolicLinkName);
					this.Device = null;
					this.mDeviceState = UsbConnectionManager.DeviceState.Disconnected;
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008130 File Offset: 0x00006330
		public unsafe uint UsbNotificationCallback(IntPtr hNotify, IntPtr Context, CfgMgr.CM_NOTIFY_ACTION Action, ref CfgMgr.CM_NOTIFY_EVENT_DATA EventData, uint EventDataSize)
		{
			if (Action != CfgMgr.CM_NOTIFY_ACTION.CM_NOTIFY_ACTION_DEVICEINTERFACEARRIVAL)
			{
				if (Action != CfgMgr.CM_NOTIFY_ACTION.CM_NOTIFY_ACTION_DEVICEINTERFACEREMOVAL)
				{
					Console.WriteLine("Unknown action\n");
				}
				else
				{
					string deviceInterfaceSymbolicLinkName;
					fixed (ushort* ptr = &EventData.u.DeviceInterface.SymbolicLink.FixedElementField)
					{
						deviceInterfaceSymbolicLinkName = Marshal.PtrToStringUni(new IntPtr((void*)ptr));
					}
					this.OnUsbDeviceDisconnect(deviceInterfaceSymbolicLinkName);
				}
			}
			else
			{
				string deviceInterfaceSymbolicLinkName;
				fixed (ushort* ptr2 = &EventData.u.DeviceInterface.SymbolicLink.FixedElementField)
				{
					deviceInterfaceSymbolicLinkName = Marshal.PtrToStringUni(new IntPtr((void*)ptr2));
				}
				this.OnUsbDeviceConnect(deviceInterfaceSymbolicLinkName);
			}
			return 0U;
		}

		// Token: 0x04000151 RID: 337
		private static readonly string UsbIfGuidString = "{875d47fc-d331-4663-b339-624001a2dc5e}";

		// Token: 0x04000152 RID: 338
		private static readonly Guid UsbIfGuid = new Guid(UsbConnectionManager.UsbIfGuidString);

		// Token: 0x04000153 RID: 339
		private readonly OnDeviceConnect deviceConnectCallback;

		// Token: 0x04000154 RID: 340
		private readonly OnDeviceDisconnect deviceDisconnectCallback;

		// Token: 0x04000155 RID: 341
		private bool mConnectionManagerStarted;

		// Token: 0x04000157 RID: 343
		private UsbConnectionManager.DeviceState mDeviceState;

		// Token: 0x04000158 RID: 344
		private UIntPtr mNotifyContext;

		// Token: 0x04000159 RID: 345
		private CfgMgr.CM_NOTIFY_CALLBACK mCmNotifyCallback;

		// Token: 0x0200005A RID: 90
		private enum DeviceState
		{
			// Token: 0x040001E0 RID: 480
			Connected,
			// Token: 0x040001E1 RID: 481
			Disconnected
		}
	}
}
