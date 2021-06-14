using System;
using System.Collections.Generic;
using System.Linq;

namespace Nokia.Mira.Chunks
{
	// Token: 0x0200002B RID: 43
	internal class ChunkInformationCollector : IChunkInformationCollector
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00003834 File Offset: 0x00001A34
		public ChunkInformationCollector(IChunkInformationWriter writer, IEnumerable<ChunkInformation> initialInformations)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (initialInformations == null)
			{
				initialInformations = Enumerable.Empty<ChunkInformation>();
			}
			this.writer = writer;
			this.internalInformations = new List<ChunkInformation>(initialInformations);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003888 File Offset: 0x00001A88
		public void Add(ChunkInformation chunkInformation)
		{
			lock (this.syncRoot)
			{
				this.MergeWithCurrentInformations(chunkInformation);
				this.writer.Write(from ch in this.internalInformations
				select new ChunkRaw(ch.Begin, ch.Current));
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000391C File Offset: 0x00001B1C
		private void MergeWithCurrentInformations(ChunkInformation chunkInformation)
		{
			ChunkInformation chunkInformation2 = this.internalInformations.FirstOrDefault((ChunkInformation ch) => ch.Begin == chunkInformation.Begin);
			if (chunkInformation2 == null)
			{
				this.internalInformations.Add(chunkInformation);
				return;
			}
			this.internalInformations[this.internalInformations.IndexOf(chunkInformation2)] = this.CoerceChunks(chunkInformation2, chunkInformation);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003987 File Offset: 0x00001B87
		private ChunkInformation CoerceChunks(ChunkInformation old, ChunkInformation @new)
		{
			return new ChunkInformation(old.Begin, @new.Current, @new.End);
		}

		// Token: 0x04000056 RID: 86
		private readonly IChunkInformationWriter writer;

		// Token: 0x04000057 RID: 87
		private readonly List<ChunkInformation> internalInformations;

		// Token: 0x04000058 RID: 88
		private readonly object syncRoot = new object();
	}
}
