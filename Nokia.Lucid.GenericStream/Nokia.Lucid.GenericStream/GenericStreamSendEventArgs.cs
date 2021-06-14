using System;

namespace Nokia.Lucid.GenericStream
{
	// Token: 0x02000003 RID: 3
	public class GenericStreamSendEventArgs : EventArgs
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002244 File Offset: 0x00000444
		public GenericStreamSendEventArgs(GenericMessage message)
		{
			this.Message = message;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002253 File Offset: 0x00000453
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000225B File Offset: 0x0000045B
		public GenericMessage Message { get; private set; }
	}
}
