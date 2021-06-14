using System;

namespace FFUComponents
{
	// Token: 0x02000021 RID: 33
	internal interface IUsbEventSink
	{
		// Token: 0x060000BD RID: 189
		void OnDeviceConnect(string usbDevicePath);

		// Token: 0x060000BE RID: 190
		void OnDeviceDisconnect(string usbDevicePath);
	}
}
