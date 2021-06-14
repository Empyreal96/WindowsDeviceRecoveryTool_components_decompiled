using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FFUComponents
{
	// Token: 0x02000055 RID: 85
	internal partial class UsbEventWatcherForm : Form
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00007DBD File Offset: 0x00005FBD
		public UsbEventWatcherForm(IUsbEventSink sink, Guid devClass, Guid devIf)
		{
			this.eventSink = sink;
			this.ifGuid = devIf;
			this.classGuid = devClass;
			this.notificationHandle = IntPtr.Zero;
			this.InitializeComponent();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007DEC File Offset: 0x00005FEC
		protected override void WndProc(ref Message message)
		{
			base.WndProc(ref message);
			int msg = message.Msg;
			if (msg != 537)
			{
				return;
			}
			this.HandleDeviceChangeMessage(ref message);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007E18 File Offset: 0x00006018
		protected override void OnHandleCreated(EventArgs e)
		{
			IntPtr intPtr = this.RegisterDeviceNotification(this.ifGuid);
			IntPtr value = Interlocked.CompareExchange(ref this.notificationHandle, intPtr, IntPtr.Zero);
			if (IntPtr.Zero != value)
			{
				NativeMethods.UnregisterDeviceNotification(intPtr);
			}
			else
			{
				this.DiscoverSimpleIODevices();
			}
			base.OnHandleCreated(e);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007E68 File Offset: 0x00006068
		protected override void OnClosed(EventArgs e)
		{
			IntPtr intPtr = Interlocked.CompareExchange(ref this.notificationHandle, IntPtr.Zero, this.notificationHandle);
			if (IntPtr.Zero != intPtr)
			{
				NativeMethods.UnregisterDeviceNotification(intPtr);
			}
			base.OnClosed(e);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007EA8 File Offset: 0x000060A8
		private void DiscoverSimpleIODevices()
		{
			string queryString = string.Format(CultureInfo.InvariantCulture, "SELECT PnPDeviceId FROM Win32_PnPEntity where ClassGuid ='{0:B}'", new object[]
			{
				this.classGuid
			});
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				string pnpId = managementBaseObject["PnPDeviceId"] as string;
				this.NotifyConnect(pnpId);
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007F3C File Offset: 0x0000613C
		private void NotifyConnect(string pnpId)
		{
			try
			{
				if (pnpId != null)
				{
					string usbDevicePath = this.GetUsbDevicePath(pnpId);
					this.eventSink.OnDeviceConnect(usbDevicePath);
				}
			}
			catch (Exception ex)
			{
				FFUManager.HostLogger.EventWriteInitNotifyException(pnpId, ex.ToString());
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007F88 File Offset: 0x00006188
		private unsafe string GetUsbDevicePath(string pnpId)
		{
			IntPtr intPtr = NativeMethods.SetupDiGetClassDevs(ref this.ifGuid, pnpId, IntPtr.Zero, 18);
			if (IntPtr.Zero == intPtr)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error);
			}
			int memberIndex = 0;
			int num = 0;
			DeviceInterfaceData deviceInterfaceData = new DeviceInterfaceData
			{
				Size = Marshal.SizeOf(typeof(DeviceInterfaceData))
			};
			if (!NativeMethods.SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref this.ifGuid, memberIndex, ref deviceInterfaceData))
			{
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				throw new Win32Exception(lastWin32Error2);
			}
			if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, IntPtr.Zero, 0, ref num, IntPtr.Zero))
			{
				int lastWin32Error3 = Marshal.GetLastWin32Error();
				if (lastWin32Error3 != 122)
				{
					throw new Win32Exception(lastWin32Error3);
				}
			}
			DeviceInterfaceDetailData* ptr = (DeviceInterfaceDetailData*)((void*)Marshal.AllocHGlobal(num));
			string result;
			try
			{
				if (IntPtr.Size == 4)
				{
					ptr->Size = 6;
				}
				else
				{
					ptr->Size = 8;
				}
				DeviceInformationData deviceInformationData = new DeviceInformationData
				{
					Size = Marshal.SizeOf(typeof(DeviceInformationData))
				};
				if (!NativeMethods.SetupDiGetDeviceInterfaceDetail(intPtr, ref deviceInterfaceData, ptr, num, ref num, ref deviceInformationData))
				{
					int lastWin32Error4 = Marshal.GetLastWin32Error();
					throw new Win32Exception(lastWin32Error4);
				}
				string text = Marshal.PtrToStringAuto(new IntPtr((void*)(&ptr->DevicePath)));
				result = text;
			}
			finally
			{
				Marshal.FreeHGlobal((IntPtr)((void*)ptr));
			}
			return result;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000080DC File Offset: 0x000062DC
		private void HandleDeviceChangeMessage(ref Message message)
		{
			int num = message.WParam.ToInt32();
			if (num == 32768)
			{
				int num2 = Marshal.ReadInt32(message.LParam, 4);
				if (num2 != 5)
				{
					return;
				}
				DevBroadcastDeviceInterface devBroadcastDeviceInterface = (DevBroadcastDeviceInterface)Marshal.PtrToStructure(message.LParam, typeof(DevBroadcastDeviceInterface));
				try
				{
					this.eventSink.OnDeviceConnect(devBroadcastDeviceInterface.Name);
					return;
				}
				catch (Exception ex)
				{
					FFUManager.HostLogger.EventWriteConnectNotifyException(devBroadcastDeviceInterface.Name, ex.ToString());
					return;
				}
			}
			if (num == 32772)
			{
				int num3 = Marshal.ReadInt32(message.LParam, 4);
				if (num3 == 5)
				{
					DevBroadcastDeviceInterface devBroadcastDeviceInterface2 = (DevBroadcastDeviceInterface)Marshal.PtrToStructure(message.LParam, typeof(DevBroadcastDeviceInterface));
					try
					{
						this.eventSink.OnDeviceDisconnect(devBroadcastDeviceInterface2.Name);
					}
					catch (Exception ex2)
					{
						FFUManager.HostLogger.EventWriteDisconnectNotifyException(devBroadcastDeviceInterface2.Name, ex2.ToString());
					}
				}
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000081E4 File Offset: 0x000063E4
		private IntPtr RegisterDeviceNotification(Guid ifGuid)
		{
			IntPtr result = IntPtr.Zero;
			DevBroadcastDeviceInterface devBroadcastDeviceInterface = new DevBroadcastDeviceInterface
			{
				Size = Marshal.SizeOf(typeof(DevBroadcastDeviceInterface)),
				DeviceType = 5,
				ClassGuid = ifGuid
			};
			IntPtr intPtr = Marshal.AllocHGlobal(devBroadcastDeviceInterface.Size);
			try
			{
				Marshal.StructureToPtr(devBroadcastDeviceInterface, intPtr, true);
				result = NativeMethods.RegisterDeviceNotification(base.Handle, intPtr, 0);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x04000171 RID: 369
		private IUsbEventSink eventSink;

		// Token: 0x04000172 RID: 370
		private Guid ifGuid;

		// Token: 0x04000173 RID: 371
		private Guid classGuid;

		// Token: 0x04000174 RID: 372
		private IntPtr notificationHandle;
	}
}
