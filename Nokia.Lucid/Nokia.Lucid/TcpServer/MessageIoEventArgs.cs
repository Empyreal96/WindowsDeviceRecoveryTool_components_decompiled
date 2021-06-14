using System;

namespace Nokia.Lucid.TcpServer
{
	// Token: 0x02000038 RID: 56
	public class MessageIoEventArgs : EventArgs
	{
		// Token: 0x06000184 RID: 388 RVA: 0x0000B8F2 File Offset: 0x00009AF2
		public MessageIoEventArgs(ref byte[] data)
		{
			this.Data = data;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000B902 File Offset: 0x00009B02
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000B90A File Offset: 0x00009B0A
		public byte[] Data { get; private set; }
	}
}
