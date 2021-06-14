using System;

namespace FFUComponents
{
	// Token: 0x0200004D RID: 77
	internal class SimpleIoEventSink : IUsbEventSink
	{
		// Token: 0x06000149 RID: 329 RVA: 0x000077BC File Offset: 0x000059BC
		public SimpleIoEventSink(SimpleIoEventSink.ConnectHandler connect, SimpleIoEventSink.DisconnectHandler disconnect)
		{
			this.onConnect = connect;
			this.onDisconnect = disconnect;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000077D2 File Offset: 0x000059D2
		public void OnDeviceConnect(string usbDevicePath)
		{
			this.onConnect(usbDevicePath);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000077E0 File Offset: 0x000059E0
		public void OnDeviceDisconnect(string usbDevicePath)
		{
			this.onDisconnect(usbDevicePath);
		}

		// Token: 0x0400015D RID: 349
		private SimpleIoEventSink.ConnectHandler onConnect;

		// Token: 0x0400015E RID: 350
		private SimpleIoEventSink.DisconnectHandler onDisconnect;

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x0600014D RID: 333
		public delegate void ConnectHandler(string deviceName);

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x06000151 RID: 337
		public delegate void DisconnectHandler(string deviceName);
	}
}
