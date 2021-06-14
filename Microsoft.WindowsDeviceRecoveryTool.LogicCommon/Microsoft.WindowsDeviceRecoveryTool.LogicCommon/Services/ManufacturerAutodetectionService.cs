using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003B RID: 59
	[Export]
	public class ManufacturerAutodetectionService : IDisposable
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000C870 File Offset: 0x0000AA70
		[ImportingConstructor]
		public ManufacturerAutodetectionService()
		{
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000303 RID: 771 RVA: 0x0000C87C File Offset: 0x0000AA7C
		// (remove) Token: 0x06000304 RID: 772 RVA: 0x0000C8B8 File Offset: 0x0000AAB8
		public event Action<UsbDeviceEventArgs> DeviceConnected;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000305 RID: 773 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		// (remove) Token: 0x06000306 RID: 774 RVA: 0x0000C930 File Offset: 0x0000AB30
		public event Action<UsbDeviceEventArgs> DeviceDisconnected;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000307 RID: 775 RVA: 0x0000C96C File Offset: 0x0000AB6C
		// (remove) Token: 0x06000308 RID: 776 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		public event Action<UsbDeviceEventArgs> DeviceEndpointConnected;

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000C9FB File Offset: 0x0000ABFB
		private bool Disposed { get; set; }

		// Token: 0x0600030B RID: 779 RVA: 0x0000CA04 File Offset: 0x0000AC04
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000CA18 File Offset: 0x0000AC18
		protected virtual void Dispose(bool disposing)
		{
			if (!this.Disposed)
			{
				if (disposing)
				{
					this.ReleaseManagedObjects();
				}
				this.ReleaseUnmanagedObjects();
				this.Disposed = true;
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000CA56 File Offset: 0x0000AC56
		protected virtual void ReleaseManagedObjects()
		{
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000CA59 File Offset: 0x0000AC59
		protected virtual void ReleaseUnmanagedObjects()
		{
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000CA5C File Offset: 0x0000AC5C
		public void Start(Collection<DeviceIdentifier> deviceIdentifiers)
		{
			Tracer<ManufacturerAutodetectionService>.LogEntry("Start");
			Tracer<ManufacturerAutodetectionService>.WriteInformation("Creating UsbDeviceScanner");
			this.usbDeviceDetector = new UsbDeviceScanner(deviceIdentifiers);
			Tracer<ManufacturerAutodetectionService>.WriteInformation("Starting UsbDeviceDetection");
			this.usbDeviceDetector.DeviceConnected += this.UsbDeviceDetectorOnDeviceConnected;
			this.usbDeviceDetector.DeviceDisconnected += this.UsbDeviceDetectorOnDeviceDisconnected;
			this.usbDeviceDetector.DeviceEndpointConnected += this.UsbDeviceDetectorOnDeviceEndpointConnected;
			this.usbDeviceDetector.Start();
			Tracer<ManufacturerAutodetectionService>.LogExit("Start");
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000CAF8 File Offset: 0x0000ACF8
		public ReadOnlyCollection<UsbDevice> GetConnectedDevices()
		{
			return this.usbDeviceDetector.GetDevices();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000CB15 File Offset: 0x0000AD15
		private void UsbDeviceDetectorOnDeviceConnected(object sender, UsbDeviceEventArgs usbDeviceEventArgs)
		{
			this.RaiseDeviceConnected(usbDeviceEventArgs);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000CB20 File Offset: 0x0000AD20
		private void UsbDeviceDetectorOnDeviceDisconnected(object sender, UsbDeviceEventArgs args)
		{
			this.RaiseDeviceDisconnected(args);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000CB2B File Offset: 0x0000AD2B
		private void UsbDeviceDetectorOnDeviceEndpointConnected(object sender, UsbDeviceEventArgs args)
		{
			this.RaiseDeviceEndpointConnected(args);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000CB38 File Offset: 0x0000AD38
		private void RaiseDeviceConnected(UsbDeviceEventArgs usbDeviceEventArgs)
		{
			Action<UsbDeviceEventArgs> deviceConnected = this.DeviceConnected;
			if (deviceConnected != null)
			{
				deviceConnected(usbDeviceEventArgs);
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000CB60 File Offset: 0x0000AD60
		private void RaiseDeviceDisconnected(UsbDeviceEventArgs usbDeviceEventArgs)
		{
			Action<UsbDeviceEventArgs> deviceDisconnected = this.DeviceDisconnected;
			if (deviceDisconnected != null)
			{
				deviceDisconnected(usbDeviceEventArgs);
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000CB88 File Offset: 0x0000AD88
		private void RaiseDeviceEndpointConnected(UsbDeviceEventArgs usbDeviceEventArgs)
		{
			Action<UsbDeviceEventArgs> deviceEndpointConnected = this.DeviceEndpointConnected;
			if (deviceEndpointConnected != null)
			{
				deviceEndpointConnected(usbDeviceEventArgs);
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		public void Stop()
		{
			Tracer<ManufacturerAutodetectionService>.LogEntry("Stop");
			if (this.usbDeviceDetector != null)
			{
				this.usbDeviceDetector.DeviceConnected -= this.UsbDeviceDetectorOnDeviceConnected;
				this.usbDeviceDetector.DeviceDisconnected -= this.UsbDeviceDetectorOnDeviceDisconnected;
				this.usbDeviceDetector.DeviceEndpointConnected -= this.UsbDeviceDetectorOnDeviceEndpointConnected;
				this.usbDeviceDetector.Stop();
				this.usbDeviceDetector = null;
			}
			Tracer<ManufacturerAutodetectionService>.LogExit("Stop");
		}

		// Token: 0x04000179 RID: 377
		private UsbDeviceScanner usbDeviceDetector;
	}
}
