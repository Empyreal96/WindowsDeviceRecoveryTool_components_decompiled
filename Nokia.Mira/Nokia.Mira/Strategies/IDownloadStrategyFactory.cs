using System;
using System.Threading;
using Nokia.Mira.Chunks;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Strategies
{
	// Token: 0x0200002E RID: 46
	internal interface IDownloadStrategyFactory
	{
		// Token: 0x060000BA RID: 186
		IDownloadStrategy Create(string temporaryFileName, DownloadSettings downloadSettings, IHttpWebRequestFactory httpWebRequestFactory, IChunkInformationWriter chunkInformationWriter, ChunkInformation[] informations, IChunkInformationProvider provider, IWebResponse initialResponse, ChunkInformation initialChunkInformation, CancellationToken token, Action<DownloadProgressInfo> reportProgress);
	}
}
