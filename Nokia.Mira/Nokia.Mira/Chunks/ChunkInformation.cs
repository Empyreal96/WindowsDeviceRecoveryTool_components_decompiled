using System;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000024 RID: 36
	internal class ChunkInformation
	{
		// Token: 0x0600009C RID: 156 RVA: 0x0000334B File Offset: 0x0000154B
		public ChunkInformation(long begin, long current, long end)
		{
			this.begin = begin;
			this.current = current;
			this.end = end;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003368 File Offset: 0x00001568
		public long Begin
		{
			get
			{
				return this.begin;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003370 File Offset: 0x00001570
		public long Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003378 File Offset: 0x00001578
		public long End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x04000046 RID: 70
		private readonly long begin;

		// Token: 0x04000047 RID: 71
		private readonly long current;

		// Token: 0x04000048 RID: 72
		private readonly long end;
	}
}
