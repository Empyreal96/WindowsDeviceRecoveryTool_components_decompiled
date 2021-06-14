using System;
using System.Threading;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000008 RID: 8
	internal static class IsiUtidManager
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000282A File Offset: 0x00000A2A
		public static int UtidMax
		{
			get
			{
				return 255;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002831 File Offset: 0x00000A31
		public static int UtidMin
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002834 File Offset: 0x00000A34
		public static byte GetUtid()
		{
			IsiUtidManager.utid = ((IsiUtidManager.utid < IsiUtidManager.UtidMax) ? Interlocked.Increment(ref IsiUtidManager.utid) : IsiUtidManager.UtidMin);
			return (byte)IsiUtidManager.utid;
		}

		// Token: 0x0400001D RID: 29
		private static int utid;
	}
}
