using System;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000005 RID: 5
	public class IsiReceiveEventArgs : EventArgs
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000024A1 File Offset: 0x000006A1
		public IsiReceiveEventArgs(IsiMessage message)
		{
			this.Message = message;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000024B0 File Offset: 0x000006B0
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000024B8 File Offset: 0x000006B8
		public IsiMessage Message { get; private set; }
	}
}
