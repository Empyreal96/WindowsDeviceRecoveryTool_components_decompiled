using System;
using System.Collections.Generic;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000006 RID: 6
	internal interface IChunkInformationWriter
	{
		// Token: 0x0600000F RID: 15
		void Write(IEnumerable<ChunkRaw> chunkInformations);
	}
}
