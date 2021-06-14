using System;

namespace Nokia.Lucid.UsbDeviceIo
{
	// Token: 0x0200003C RID: 60
	public class OnSendingEventArgs : EventArgs
	{
		// Token: 0x060001AC RID: 428 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		public OnSendingEventArgs(byte[] data)
		{
			this.Data = data;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000C5D7 File Offset: 0x0000A7D7
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000C5DF File Offset: 0x0000A7DF
		public byte[] Data { get; private set; }
	}
}
