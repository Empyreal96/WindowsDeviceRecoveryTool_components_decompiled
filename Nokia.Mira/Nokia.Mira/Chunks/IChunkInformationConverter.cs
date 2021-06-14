using System;
using System.Collections.ObjectModel;

namespace Nokia.Mira.Chunks
{
	// Token: 0x02000002 RID: 2
	internal interface IChunkInformationConverter
	{
		// Token: 0x06000001 RID: 1
		bool TryConvertToChunkSize(long chunkSize, out ReadOnlyCollection<ChunkInformation> convertedInformations);

		// Token: 0x06000002 RID: 2
		ReadOnlyCollection<ChunkInformation> ConvertToChunkSize(long chunkSize);
	}
}
