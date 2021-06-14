using System;

namespace Nokia.Lucid.GenericStream
{
	// Token: 0x02000004 RID: 4
	public class GenericStreamReceiveEventArgs : EventArgs
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002264 File Offset: 0x00000464
		public GenericStreamReceiveEventArgs(GenericMessage message)
		{
			this.Message = message;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002273 File Offset: 0x00000473
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000227B File Offset: 0x0000047B
		public GenericMessage Message { get; private set; }
	}
}
