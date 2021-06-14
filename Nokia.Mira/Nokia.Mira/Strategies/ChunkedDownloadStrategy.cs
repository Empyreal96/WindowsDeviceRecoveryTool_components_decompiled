using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nokia.Mira.Chunks;
using Nokia.Mira.Extensions;
using Nokia.Mira.IO;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Strategies
{
	// Token: 0x02000029 RID: 41
	internal sealed class ChunkedDownloadStrategy : IDownloadStrategy
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00003494 File Offset: 0x00001694
		public ChunkedDownloadStrategy(IWebResponse initialWebResponse, ChunkInformation initialChunkInformation, IChunkInformationProvider chunkInformationProvider, IHttpWebRequestFactory httpWebRequestFactory, IFileStreamFactory fileStreamFactory, int maxChunks, CancellationToken token, Action<long> reportBytesDownloaded, Action<ChunkInformation> reportBytesWritten)
		{
			this.initialWebResponse = initialWebResponse;
			this.initialChunkInformation = initialChunkInformation;
			this.chunkInformationProvider = chunkInformationProvider;
			this.httpWebRequestFactory = httpWebRequestFactory;
			this.fileStreamFactory = fileStreamFactory;
			this.token = token;
			this.reportBytesDownloaded = reportBytesDownloaded;
			this.reportBytesWritten = reportBytesWritten;
			this.semaphore = new SemaphoreSlim(maxChunks, maxChunks);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000354C File Offset: 0x0000174C
		public ChunkStatus GetNextChunk(out Lazy<Task> chunk)
		{
			chunk = null;
			if (!this.semaphore.Wait(TimeSpan.Zero))
			{
				return ChunkStatus.WaitingForResources;
			}
			IWebResponse currentResponse;
			try
			{
				currentResponse = this.GetWebResponse();
			}
			catch (Exception)
			{
				this.semaphore.Release();
				throw;
			}
			ChunkInformation currentChunk;
			try
			{
				currentChunk = this.GetChunkInformation();
			}
			catch (Exception)
			{
				currentResponse.Close();
				this.semaphore.Release();
				throw;
			}
			if (currentResponse.StatusCode == HttpStatusCode.OK || currentResponse.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
			{
				currentResponse.Close();
				this.semaphore.Release();
				return ChunkStatus.RanToCompletion;
			}
			ChunkStatus result = ChunkStatus.WaitingToRun;
			if (currentResponse.ContentLength < currentChunk.End - currentChunk.Current)
			{
				result = ChunkStatus.WaitingToCompletion;
				currentChunk = new ChunkInformation(currentChunk.Begin, currentChunk.Current, currentChunk.Current + currentResponse.ContentLength);
			}
			chunk = new Lazy<Task>(() => this.StartDownloadAsync(currentResponse, currentChunk).ContinueWith<Task>(delegate(Task t)
			{
				currentResponse.Close();
				this.semaphore.Release();
				return t;
			}).Unwrap());
			return result;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003690 File Offset: 0x00001890
		private IWebResponse GetWebResponse()
		{
			if (this.initialWebResponse != null)
			{
				IWebResponse result = this.initialWebResponse;
				this.initialWebResponse = null;
				return result;
			}
			return this.CreateWebResponse(this.chunkInformationProvider.Current);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000036C8 File Offset: 0x000018C8
		private ChunkInformation GetChunkInformation()
		{
			ChunkInformation result;
			if (this.initialChunkInformation != null)
			{
				result = this.initialChunkInformation;
				this.initialChunkInformation = null;
				return result;
			}
			result = this.chunkInformationProvider.Current;
			this.chunkInformationProvider.MoveNext();
			return result;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003705 File Offset: 0x00001905
		private IWebResponse CreateWebResponse(ChunkInformation chunk)
		{
			return this.httpWebRequestFactory.Create().GetResponse(chunk.Current, chunk.End, this.token);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000037D4 File Offset: 0x000019D4
		private Task StartDownloadAsync(IWebResponse webResponse, ChunkInformation chunkInformation)
		{
			long totalBytesWritten = 0L;
			return Task.Factory.StartNew(delegate()
			{
				webResponse.DownloadResponseStream(chunkInformation.Current, this.token, this.fileStreamFactory, delegate(long v)
				{
					totalBytesWritten += v;
					this.OnBytesDownloaded(v);
					this.reportBytesWritten(new ChunkInformation(chunkInformation.Begin, chunkInformation.Current + totalBytesWritten, chunkInformation.End));
				});
			}, this.token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003826 File Offset: 0x00001A26
		private void OnBytesDownloaded(long bytesCount)
		{
			this.reportBytesDownloaded(bytesCount);
		}

		// Token: 0x0400004D RID: 77
		private readonly CancellationToken token;

		// Token: 0x0400004E RID: 78
		private readonly Action<long> reportBytesDownloaded;

		// Token: 0x0400004F RID: 79
		private readonly Action<ChunkInformation> reportBytesWritten;

		// Token: 0x04000050 RID: 80
		private readonly SemaphoreSlim semaphore;

		// Token: 0x04000051 RID: 81
		private readonly IChunkInformationProvider chunkInformationProvider;

		// Token: 0x04000052 RID: 82
		private readonly IHttpWebRequestFactory httpWebRequestFactory;

		// Token: 0x04000053 RID: 83
		private readonly IFileStreamFactory fileStreamFactory;

		// Token: 0x04000054 RID: 84
		private IWebResponse initialWebResponse;

		// Token: 0x04000055 RID: 85
		private ChunkInformation initialChunkInformation;
	}
}
