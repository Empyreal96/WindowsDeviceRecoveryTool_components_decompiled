using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000003 RID: 3
	internal class ChunkInformationConverter : IChunkInformationConverter
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
		public ChunkInformationConverter(IEnumerable<ChunkRaw> chunkInformations)
		{
			if (chunkInformations == null)
			{
				this.internalInformations = new List<ChunkRaw>();
				return;
			}
			this.internalInformations = new List<ChunkRaw>(chunkInformations);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002073 File Offset: 0x00000273
		public bool TryConvertToChunkSize(long chunkSize, out ReadOnlyCollection<ChunkInformation> convertedInformations)
		{
			convertedInformations = this.ConvertToChunkSize(chunkSize);
			return true;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002080 File Offset: 0x00000280
		public ReadOnlyCollection<ChunkInformation> ConvertToChunkSize(long chunkSize)
		{
			long num = 0L;
			List<ChunkInformation> list = new List<ChunkInformation>();
			while (this.CanContinue(num))
			{
				ChunkRaw chunkRaw = this.FindMatchingChunk(num);
				if (chunkRaw != null)
				{
					long current = (chunkRaw.Current < num + chunkSize) ? chunkRaw.Current : (num + chunkSize);
					ChunkInformation item = new ChunkInformation(num, current, num + chunkSize - 1L);
					list.Add(item);
				}
				num += chunkSize;
			}
			return list.AsReadOnly();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000210C File Offset: 0x0000030C
		private ChunkRaw FindMatchingChunk(long position)
		{
			return this.internalInformations.FirstOrDefault((ChunkRaw ch) => ch.Begin <= position && ch.Current > position);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002158 File Offset: 0x00000358
		private bool CanContinue(long position)
		{
			return this.internalInformations.Any((ChunkRaw ch) => ch.Current > position);
		}

		// Token: 0x04000001 RID: 1
		private readonly List<ChunkRaw> internalInformations;
	}
}
