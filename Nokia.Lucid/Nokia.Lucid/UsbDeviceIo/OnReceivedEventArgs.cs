using System;

namespace Nokia.Lucid.UsbDeviceIo
{
	// Token: 0x0200003B RID: 59
	public class OnReceivedEventArgs : EventArgs
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
		public OnReceivedEventArgs(byte[] data)
		{
			this.Data = data;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000C5B7 File Offset: 0x0000A7B7
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000C5BF File Offset: 0x0000A7BF
		public byte[] Data { get; private set; }
	}
}
