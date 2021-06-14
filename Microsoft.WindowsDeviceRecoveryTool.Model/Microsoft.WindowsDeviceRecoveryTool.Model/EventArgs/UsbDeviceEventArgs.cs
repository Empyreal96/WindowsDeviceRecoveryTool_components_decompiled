using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x02000032 RID: 50
	public class UsbDeviceEventArgs : EventArgs
	{
		// Token: 0x06000166 RID: 358 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public UsbDeviceEventArgs(UsbDevice usbDevice)
		{
			if (usbDevice == null)
			{
				throw new ArgumentNullException("usbDevice");
			}
			this.UsbDevice = usbDevice;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00004AE8 File Offset: 0x00002CE8
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00004AFF File Offset: 0x00002CFF
		public UsbDevice UsbDevice { get; private set; }
	}
}
