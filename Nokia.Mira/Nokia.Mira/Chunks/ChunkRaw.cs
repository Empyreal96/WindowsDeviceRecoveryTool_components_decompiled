using System;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000008 RID: 8
	internal sealed class ChunkRaw
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002430 File Offset: 0x00000630
		public ChunkRaw(long begin, long current)
		{
			this.begin = begin;
			this.current = current;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002446 File Offset: 0x00000646
		public long Begin
		{
			get
			{
				return this.begin;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000244E File Offset: 0x0000064E
		public long Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x04000007 RID: 7
		private readonly long begin;

		// Token: 0x04000008 RID: 8
		private readonly long current;
	}
}
