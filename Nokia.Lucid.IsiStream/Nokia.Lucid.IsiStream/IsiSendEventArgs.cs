using System;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000004 RID: 4
	public class IsiSendEventArgs : EventArgs
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002481 File Offset: 0x00000681
		public IsiSendEventArgs(IsiMessage message)
		{
			this.Message = message;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002490 File Offset: 0x00000690
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002498 File Offset: 0x00000698
		public IsiMessage Message { get; private set; }
	}
}
