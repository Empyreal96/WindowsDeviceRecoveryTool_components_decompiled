using System;
using System.Collections.Generic;
using System.Linq;

namespace Nokia.Mira.Chunks
{
	// Token: 0x0200000A RID: 10
	internal class CollectionBasedChunkInformationProvider : IChunkInformationProvider
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002456 File Offset: 0x00000656
		public CollectionBasedChunkInformationProvider(IEnumerable<ChunkInformation> chunkInformations, long preferredChunkSize)
		{
			this.preferredSize = preferredChunkSize;
			if (chunkInformations == null)
			{
				chunkInformations = Enumerable.Empty<ChunkInformation>();
			}
			this.chunkInformations = new List<ChunkInformation>(chunkInformations);
			this.currentChunk = this.GetNextChunk();
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002487 File Offset: 0x00000687
		public ChunkInformation Current
		{
			get
			{
				return this.currentChunk;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002490 File Offset: 0x00000690
		public void MoveNext()
		{
			if (this.nextChunk != null)
			{
				this.prevChunk = this.currentChunk;
				this.currentChunk = this.nextChunk;
				this.nextChunk = null;
				return;
			}
			this.prevChunk = this.currentChunk;
			this.currentChunk = this.GetNextChunk();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000024DD File Offset: 0x000006DD
		public void MoveBack()
		{
			if (this.prevChunk == null)
			{
				throw new InvalidOperationException("MoveBack can be called only once in a row.");
			}
			this.nextChunk = this.currentChunk;
			this.currentChunk = this.prevChunk;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002524 File Offset: 0x00000724
		private ChunkInformation GetNextChunk()
		{
			long begin = this.currentBegin;
			this.currentBegin += this.preferredSize;
			ChunkInformation chunkInformation = this.chunkInformations.FirstOrDefault((ChunkInformation ch) => ch.Begin == begin);
			if (chunkInformation == null)
			{
				return new ChunkInformation(begin, begin, begin + this.preferredSize - 1L);
			}
			if (chunkInformation.Current <= chunkInformation.End)
			{
				return chunkInformation;
			}
			return this.GetNextChunk();
		}

		// Token: 0x04000009 RID: 9
		private readonly long preferredSize;

		// Token: 0x0400000A RID: 10
		private readonly List<ChunkInformation> chunkInformations;

		// Token: 0x0400000B RID: 11
		private ChunkInformation currentChunk;

		// Token: 0x0400000C RID: 12
		private ChunkInformation prevChunk;

		// Token: 0x0400000D RID: 13
		private ChunkInformation nextChunk;

		// Token: 0x0400000E RID: 14
		private long currentBegin;
	}
}
