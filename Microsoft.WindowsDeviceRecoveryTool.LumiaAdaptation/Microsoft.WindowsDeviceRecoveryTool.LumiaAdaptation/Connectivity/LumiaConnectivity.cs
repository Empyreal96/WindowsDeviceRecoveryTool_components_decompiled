using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Connectivity
{
	// Token: 0x02000003 RID: 3
	public class LumiaConnectivity
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000002 RID: 2 RVA: 0x000020A8 File Offset: 0x000002A8
		// (remove) Token: 0x06000003 RID: 3 RVA: 0x000020E4 File Offset: 0x000002E4
		public event EventHandler<DeviceConnectedEventArgs> DeviceConnected;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000004 RID: 4 RVA: 0x00002120 File Offset: 0x00000320
		// (remove) Token: 0x06000005 RID: 5 RVA: 0x0000215C File Offset: 0x0000035C
		public event EventHandler<DeviceConnectedEventArgs> DeviceDisconnected;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000006 RID: 6 RVA: 0x00002198 File Offset: 0x00000398
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x000021D4 File Offset: 0x000003D4
		public event EventHandler<DeviceReadyChangedEventArgs> DeviceReadyChanged;

		// Token: 0x06000008 RID: 8 RVA: 0x00002210 File Offset: 0x00000410
		public Collection<ConnectedDevice> GetAllConnectedDevices()
		{
			return new Collection<ConnectedDevice>(this.devices);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002230 File Offset: 0x00000430
		public void Start(IList<DeviceIdentifier> deviceIdentifiers)
		{
			if (this.usbDeviceDetector == null)
			{
				this.usbDeviceDetector = new UsbDeviceScanner(deviceIdentifiers);
				this.usbDeviceDetector.DeviceConnected += this.HandleDeviceConnected;
				this.usbDeviceDetector.DeviceDisconnected += this.HandleDeviceDisconnected;
				this.usbDeviceDetector.DeviceEndpointConnected += this.HandleDeviceEndpointConnected;
				this.usbDeviceDetector.Start();
				ReadOnlyCollection<UsbDevice> readOnlyCollection = this.usbDeviceDetector.GetDevices();
				foreach (UsbDevice usbDevice in readOnlyCollection)
				{
					this.devices.Add(LucidConnectivityHelper.GetConnectedDeviceFromUsbDevice(usbDevice));
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002314 File Offset: 0x00000514
		public void Stop()
		{
			if (this.usbDeviceDetector != null)
			{
				this.usbDeviceDetector.Stop();
				this.usbDeviceDetector.DeviceConnected -= this.HandleDeviceConnected;
				this.usbDeviceDetector.DeviceDisconnected -= this.HandleDeviceDisconnected;
				this.usbDeviceDetector.DeviceEndpointConnected -= this.HandleDeviceEndpointConnected;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002388 File Offset: 0x00000588
		private void HandleDeviceConnected(object sender, UsbDeviceEventArgs e)
		{
			if (e.UsbDevice != null)
			{
				Tracer<LumiaConnectivity>.WriteInformation("HandleDeviceConnected: portID: {0}, VID&PID: {1}", new object[]
				{
					e.UsbDevice.PortId,
					e.UsbDevice.Vid + "&" + e.UsbDevice.Pid
				});
				foreach (ConnectedDevice connectedDevice in this.devices)
				{
					if (connectedDevice.PortId == e.UsbDevice.PortId)
					{
						if (!connectedDevice.IsDeviceConnected)
						{
							this.devices[this.devices.IndexOf(connectedDevice)].IsDeviceConnected = true;
							this.devices[this.devices.IndexOf(connectedDevice)].DeviceReady = false;
							this.devices[this.devices.IndexOf(connectedDevice)].DevicePath = string.Empty;
							this.devices[this.devices.IndexOf(connectedDevice)].Vid = e.UsbDevice.Vid;
							this.devices[this.devices.IndexOf(connectedDevice)].Pid = e.UsbDevice.Pid;
							ConnectedDeviceMode mode = this.devices[this.devices.IndexOf(connectedDevice)].Mode;
							ConnectedDeviceMode deviceMode = LucidConnectivityHelper.GetDeviceMode(e.UsbDevice.Vid, e.UsbDevice.Pid);
							this.devices[this.devices.IndexOf(connectedDevice)].Mode = deviceMode;
							this.devices[this.devices.IndexOf(connectedDevice)].TypeDesignator = e.UsbDevice.TypeDesignator;
							this.devices[this.devices.IndexOf(connectedDevice)].SalesName = e.UsbDevice.SalesName;
							if (deviceMode != mode)
							{
								this.SendDeviceConnectedEvent(this.devices[this.devices.IndexOf(connectedDevice)]);
							}
							else
							{
								this.SendDeviceConnectedEvent(this.devices[this.devices.IndexOf(connectedDevice)]);
							}
						}
						return;
					}
				}
				ConnectedDevice connectedDevice2 = new ConnectedDevice(e.UsbDevice.PortId, e.UsbDevice.Vid, e.UsbDevice.Pid, LucidConnectivityHelper.GetDeviceMode(e.UsbDevice.Vid, e.UsbDevice.Pid), true, e.UsbDevice.TypeDesignator, e.UsbDevice.SalesName, e.UsbDevice.Path, e.UsbDevice.InstanceId);
				this.devices.Add(connectedDevice2);
				this.SendDeviceConnectedEvent(connectedDevice2);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000026B4 File Offset: 0x000008B4
		private void HandleDeviceEndpointConnected(object sender, UsbDeviceEventArgs e)
		{
			if (e.UsbDevice != null)
			{
				Tracer<LumiaConnectivity>.WriteInformation("HandleDeviceEndpointConnected: portID: {0}, VID&PID: {1}", new object[]
				{
					e.UsbDevice.PortId,
					e.UsbDevice.Vid + "&" + e.UsbDevice.Pid
				});
				foreach (ConnectedDevice connectedDevice in this.devices)
				{
					if (connectedDevice.PortId == e.UsbDevice.PortId)
					{
						switch (this.devices[this.devices.IndexOf(connectedDevice)].Mode)
						{
						case ConnectedDeviceMode.Normal:
						case ConnectedDeviceMode.Uefi:
							if (e.UsbDevice.Interfaces.Count > 0)
							{
								this.devices[this.devices.IndexOf(connectedDevice)].DeviceReady = true;
								this.devices[this.devices.IndexOf(connectedDevice)].DevicePath = e.UsbDevice.Interfaces[0].DevicePath;
								this.SendDeviceReadyChangedEvent(this.devices[this.devices.IndexOf(connectedDevice)]);
							}
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002858 File Offset: 0x00000A58
		private void HandleDeviceDisconnected(object sender, UsbDeviceEventArgs e)
		{
			if (e.UsbDevice != null)
			{
				Tracer<LumiaConnectivity>.WriteInformation("HandleDeviceDisconnected: portID: {0}, VID&PID: {1}", new object[]
				{
					e.UsbDevice.PortId,
					e.UsbDevice.Vid + "&" + e.UsbDevice.Pid
				});
				foreach (ConnectedDevice connectedDevice in this.devices)
				{
					if (connectedDevice.PortId == e.UsbDevice.PortId)
					{
						this.devices[this.devices.IndexOf(connectedDevice)].IsDeviceConnected = false;
						this.devices[this.devices.IndexOf(connectedDevice)].DeviceReady = false;
						this.devices[this.devices.IndexOf(connectedDevice)].DevicePath = string.Empty;
						this.SendDeviceDisconnectedEvent(this.devices[this.devices.IndexOf(connectedDevice)]);
						this.devices.Remove(connectedDevice);
						break;
					}
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000029B4 File Offset: 0x00000BB4
		private void SendDeviceConnectedEvent(ConnectedDevice device)
		{
			if (!device.SuppressConnectedDisconnectedEvents)
			{
				Tracer<LumiaConnectivity>.WriteInformation("SendDeviceConnectedEvent: portID: {0}, VID&PID: {1}, mode: {2}, connected: {3}, typeDesignator: {4}", new object[]
				{
					device.PortId,
					device.Vid + "&" + device.Pid,
					device.Mode,
					device.IsDeviceConnected,
					device.TypeDesignator
				});
				EventHandler<DeviceConnectedEventArgs> deviceConnected = this.DeviceConnected;
				if (deviceConnected != null)
				{
					deviceConnected(this, new DeviceConnectedEventArgs(device));
				}
			}
			else
			{
				Tracer<LumiaConnectivity>.WriteInformation("SendDeviceConnectedEvent: event suppressed. portID: {0}, VID&PID: {1}, mode: {2}, connected: {3}, typeDesignator: {4}", new object[]
				{
					device.PortId,
					device.Vid + "&" + device.Pid,
					device.Mode,
					device.IsDeviceConnected,
					device.TypeDesignator
				});
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002AA8 File Offset: 0x00000CA8
		private void SendDeviceDisconnectedEvent(ConnectedDevice device)
		{
			if (!device.SuppressConnectedDisconnectedEvents)
			{
				Tracer<LumiaConnectivity>.WriteInformation("SendDeviceDisconnectedEvent: portID: {0}, VID&PID: {1}, mode: {2}, connected: {3}, typeDesignator: {4}", new object[]
				{
					device.PortId,
					device.Vid + "&" + device.Pid,
					device.Mode,
					device.IsDeviceConnected,
					device.TypeDesignator
				});
				EventHandler<DeviceConnectedEventArgs> deviceDisconnected = this.DeviceDisconnected;
				if (deviceDisconnected != null)
				{
					deviceDisconnected(this, new DeviceConnectedEventArgs(device));
				}
			}
			else
			{
				Tracer<LumiaConnectivity>.WriteInformation("SendDeviceDisconnectedEvent: event suppressed. portID: {0}, VID&PID: {1}, mode: {2}, connected: {3}, typeDesignator: {4}", new object[]
				{
					device.PortId,
					device.Vid + "&" + device.Pid,
					device.Mode,
					device.IsDeviceConnected,
					device.TypeDesignator
				});
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002B9C File Offset: 0x00000D9C
		private void SendDeviceReadyChangedEvent(ConnectedDevice device)
		{
			Tracer<LumiaConnectivity>.WriteInformation("SendDeviceReadyChangedEvent: portID: {0}, VID&PID: {1}, mode: {2}, connected: {3}, typeDesignator: {4}", new object[]
			{
				device.PortId,
				device.Vid + "&" + device.Pid,
				device.Mode,
				device.IsDeviceConnected,
				device.TypeDesignator
			});
			EventHandler<DeviceReadyChangedEventArgs> deviceReadyChanged = this.DeviceReadyChanged;
			if (deviceReadyChanged != null)
			{
				deviceReadyChanged(this, new DeviceReadyChangedEventArgs(device, device.DeviceReady, device.Mode));
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002C2F File Offset: 0x00000E2F
		public void FillLumiaDeviceInfo(Phone phone, CancellationToken token)
		{
			this.usbDeviceDetector.FillDeviceInfo(phone, token);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002C40 File Offset: 0x00000E40
		public int ReadBatteryLevel(Phone phone)
		{
			return this.usbDeviceDetector.ReadBatteryLevel(phone);
		}

		// Token: 0x04000001 RID: 1
		private readonly Collection<ConnectedDevice> devices = new Collection<ConnectedDevice>();

		// Token: 0x04000002 RID: 2
		private UsbDeviceScanner usbDeviceDetector;
	}
}
