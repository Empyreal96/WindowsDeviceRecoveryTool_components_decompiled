using System;
using System.Linq;
using System.Threading;
using Nokia.Mira.Chunks;
using Nokia.Mira.IO;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Strategies
{
	// Token: 0x02000031 RID: 49
	internal sealed class DownloadStrategyFactory : IDownloadStrategyFactory
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00004358 File Offset: 0x00002558
		public IDownloadStrategy Create(string temporaryFileName, DownloadSettings downloadSettings, IHttpWebRequestFactory httpWebRequestFactory, IChunkInformationWriter chunkInformationWriter, ChunkInformation[] informations, IChunkInformationProvider provider, IWebResponse initialResponse, ChunkInformation initialChunkInformation, CancellationToken token, Action<DownloadProgressInfo> reportProgress)
		{
			long num = informations.Sum((ChunkInformation i) => i.Current - i.Begin);
			Action<long> action = delegate(long bytesDownloaded)
			{
				reportProgress(new DownloadProgressInfo(bytesDownloaded));
			};
			if (initialResponse.IsContentRangeSupported())
			{
				long totalLength;
				if (initialResponse.TryGetLengthFromRange(out totalLength) && totalLength > 0L)
				{
					action = delegate(long bytesDownloaded)
					{
						reportProgress(new DownloadProgressInfo(bytesDownloaded, new long?(totalLength)));
					};
				}
				action(num);
				IChunkInformationCollector @object = new ChunkInformationCollector(chunkInformationWriter, informations);
				SynchronizedFileStreamFactory fileStreamFactory = new SynchronizedFileStreamFactory(temporaryFileName);
				return new ChunkedDownloadStrategy(initialResponse, initialChunkInformation, provider, httpWebRequestFactory, fileStreamFactory, downloadSettings.MaxChunks, token, action, new Action<ChunkInformation>(@object.Add));
			}
			long num2;
			if (initialResponse.TryGetContentLength(out num2) && num2 == num)
			{
				initialResponse.Close();
				return new CompletedDownloadStrategy();
			}
			Action<long> reportBytesWritten = delegate(long v)
			{
				chunkInformationWriter.Write(new ChunkRaw[]
				{
					new ChunkRaw(0L, v)
				});
			};
			DirectFileStreamFactory fileStreamFactory2 = new DirectFileStreamFactory(temporaryFileName);
			return new ChunklessDownloadStrategy(initialResponse, fileStreamFactory2, token, action, reportBytesWritten);
		}
	}
}
