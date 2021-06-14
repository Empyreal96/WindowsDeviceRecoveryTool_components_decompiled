using System;
using System.Collections.ObjectModel;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000004 RID: 4
	internal interface IChunkInformationReader
	{
		// Token: 0x06000008 RID: 8
		ReadOnlyCollection<ChunkRaw> Read();
	}
}
