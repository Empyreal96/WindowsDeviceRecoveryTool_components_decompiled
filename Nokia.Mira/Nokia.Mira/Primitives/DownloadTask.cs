using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nokia.Mira.Chunks;
using Nokia.Mira.Extensions;
using Nokia.Mira.Metadata;
using Nokia.Mira.Strategies;

namespace Nokia.Mira.Primitives
{
	// Token: 0x02000030 RID: 48
	public sealed class DownloadTask
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003AB8 File Offset: 0x00001CB8
		internal DownloadTask(IHttpWebRequestFactory httpWebRequestFactory, string fileName, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings, IDownloadStrategyFactory downloadStrategyFactory, IDownloadPool downloadPool) : this(httpWebRequestFactory, fileName, DownloadTask.GetTemporaryFileName(fileName), DownloadTask.GetMetadataFileName(fileName), new EnvironmentSetup(DownloadTask.GetTemporaryFileName(fileName)), cancellationToken, progress, downloadSettings, downloadStrategyFactory, downloadPool)
		{
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003C58 File Offset: 0x00001E58
		internal DownloadTask(IHttpWebRequestFactory httpWebRequestFactory, string fileName, string temporaryFileName, string metadataFileName, EnvironmentSetup environmentSetup, CancellationToken cancellationToken, IProgress<DownloadProgressInfo> progress, DownloadSettings downloadSettings, IDownloadStrategyFactory downloadStrategyFactory, IDownloadPool downloadPool)
		{
			DownloadTask <>4__this = this;
			this.fileName = fileName;
			this.temporaryFileName = temporaryFileName;
			this.cancellationToken = cancellationToken;
			this.downloadSettings = downloadSettings;
			this.downloadPool = downloadPool;
			this.progress = progress;
			this.blockingDispatcher = new BlockingDispatcher<DownloadProgressInfo>(new Action<DownloadProgressInfo>(this.OnProgressDispatched));
			this.AddEmptyTask();
			this.downloadStrategyLazy = new Lazy<IDownloadStrategy>(delegate()
			{
				<>4__this.cancellationToken.ThrowIfCancellationRequested();
				if (!<>4__this.downloadSettings.OverwriteExistingFile && File.Exists(<>4__this.fileName))
				{
					throw new InvalidOperationException("Target file already exists.");
				}
				environmentSetup.EnsureTargetDirectory();
				<>4__this.streamContainer = new MetadataStreamContainer(metadataFileName);
				IChunkInformationReader reader = new ChunkInformationXmlReader(<>4__this.streamContainer);
				ChunkInformation[] array;
				if (downloadSettings.ResumeDownload)
				{
					array = environmentSetup.PrepareForResumedDownload(downloadSettings.ChunkSize, reader).ToArray<ChunkInformation>();
				}
				else
				{
					array = new ChunkInformation[0];
					environmentSetup.PrepareForNewDownload();
				}
				HttpWebRequest request = httpWebRequestFactory.Create();
				IChunkInformationProvider chunkInformationProvider = new CollectionBasedChunkInformationProvider(array, downloadSettings.ChunkSize);
				ChunkInformation chunkInformation = chunkInformationProvider.Current;
				IWebResponse response = request.GetResponse(chunkInformation.Current, chunkInformation.End, cancellationToken);
				IChunkInformationWriter chunkInformationWriter = new ChunkInformationXmlWriter(<>4__this.streamContainer);
				chunkInformationProvider.MoveNext();
				return downloadStrategyFactory.Create(<>4__this.temporaryFileName, downloadSettings, httpWebRequestFactory, chunkInformationWriter, array, chunkInformationProvider, response, chunkInformation, cancellationToken, new Action<DownloadProgressInfo>(<>4__this.blockingDispatcher.Dispatch));
			});
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003D2E File Offset: 0x00001F2E
		public bool IsRunningToCompletion
		{
			get
			{
				return this.isRunningToCompletion;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003D38 File Offset: 0x00001F38
		public ChunkStatus GetNextDownloadChunk(out Lazy<Task> chunk)
		{
			IDownloadStrategy downloadStrategy = this.GetDownloadStrategy();
			Lazy<Task> lazy;
			ChunkStatus nextChunk = downloadStrategy.GetNextChunk(out lazy);
			if (nextChunk == ChunkStatus.WaitingToRun || nextChunk == ChunkStatus.WaitingToCompletion)
			{
				this.chunks.Add(lazy);
			}
			chunk = lazy;
			return nextChunk;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003D6C File Offset: 0x00001F6C
		public void SetCompleted()
		{
			this.OnSetCompleted();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003D74 File Offset: 0x00001F74
		public void SetCanceled()
		{
			this.OnSetCanceled();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003D7C File Offset: 0x00001F7C
		public void SetFaulted(Exception error)
		{
			this.OnSetFaulted(error);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003DD0 File Offset: 0x00001FD0
		internal Task StartAsync()
		{
			this.downloadPool.QueueDownloadTask(this);
			return this.completionSource.Task.ContinueWith<Task<object>>(delegate(Task<object> t)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				if (t.IsCanceled)
				{
					taskCompletionSource.SetCanceled();
				}
				else if (t.IsFaulted)
				{
					taskCompletionSource.SetException(t.Exception);
				}
				else
				{
					taskCompletionSource.SetResult(null);
				}
				return taskCompletionSource.Task;
			}, CancellationToken.None).Unwrap<object>();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003E20 File Offset: 0x00002020
		private static string GetMetadataFileName(string fileName)
		{
			string path = Path.GetDirectoryName(fileName) ?? string.Empty;
			string str = Path.GetFileName(fileName);
			return Path.Combine(path, str + ".metadata");
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003E58 File Offset: 0x00002058
		private static string GetTemporaryFileName(string fileName)
		{
			string path = Path.GetDirectoryName(fileName) ?? string.Empty;
			string str = Path.GetFileName(fileName);
			return Path.Combine(path, str + ".tmp");
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003EA4 File Offset: 0x000020A4
		private void AddEmptyTask()
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			source.SetResult(null);
			this.chunks.Add(new Lazy<Task>(() => source.Task));
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003F00 File Offset: 0x00002100
		private void OnSetFaulted(Exception ex)
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			source.SetException(ex);
			this.chunks.Add(new Lazy<Task>(() => source.Task));
			this.OnLastChunkStarted();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003F64 File Offset: 0x00002164
		private void OnSetCanceled()
		{
			TaskCompletionSource<object> source = new TaskCompletionSource<object>();
			source.SetCanceled();
			this.chunks.Add(new Lazy<Task>(() => source.Task));
			this.OnLastChunkStarted();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003FAF File Offset: 0x000021AF
		private void OnSetCompleted()
		{
			this.OnLastChunkStarted();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004174 File Offset: 0x00002374
		private void OnLastChunkStarted()
		{
			this.isRunningToCompletion = true;
			Task.Factory.ContinueWhenAll((from ch in this.chunks
			select ch.Value).ToArray<Task>(), delegate(Task[] tasks)
			{
				IEnumerable<AggregateException> source = (from t in tasks
				where t.IsFaulted && t.Exception != null
				select t.Exception).ToArray<AggregateException>();
				if (source.Any<AggregateException>())
				{
					try
					{
						this.blockingDispatcher.Dispose();
					}
					catch (Exception)
					{
					}
					try
					{
						this.streamContainer.Dispose();
					}
					catch (Exception)
					{
					}
					finally
					{
						this.completionSource.SetException(source.SelectMany((AggregateException ex) => ex.InnerExceptions));
					}
					return;
				}
				if (tasks.Any((Task t) => t.IsCanceled))
				{
					try
					{
						this.blockingDispatcher.Dispose();
					}
					catch (Exception)
					{
					}
					try
					{
						this.streamContainer.Dispose();
					}
					catch (Exception)
					{
					}
					finally
					{
						this.completionSource.SetCanceled();
					}
					return;
				}
				try
				{
					this.OnCompleted();
				}
				catch (Exception exception)
				{
					this.completionSource.SetException(exception);
					return;
				}
				this.completionSource.SetResult(null);
			});
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000041CC File Offset: 0x000023CC
		private void InternalTaskAction()
		{
			try
			{
				this.completionSource.Task.Wait();
			}
			catch (Exception)
			{
				if (this.completionSource.Task.IsCanceled)
				{
					throw new OperationCanceledException(this.cancellationToken);
				}
				throw;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000421C File Offset: 0x0000241C
		private void OnProgressDispatched(DownloadProgressInfo downloadProgressInfo)
		{
			this.progress.Report(downloadProgressInfo);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000422A File Offset: 0x0000242A
		private IDownloadStrategy GetDownloadStrategy()
		{
			return this.downloadStrategyLazy.Value;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004238 File Offset: 0x00002438
		private void OnCompleted()
		{
			bool flag = false;
			try
			{
				if (File.Exists(this.fileName))
				{
					if (!this.downloadSettings.OverwriteExistingFile)
					{
						throw new InvalidOperationException("Target file already exists.");
					}
					File.Delete(this.fileName);
				}
				File.Move(this.temporaryFileName, this.fileName);
				flag = true;
			}
			finally
			{
				if (this.blockingDispatcher != null)
				{
					this.blockingDispatcher.Dispose();
				}
				if (this.streamContainer != null)
				{
					if (flag)
					{
						this.streamContainer.RemoveMetadataOnDispose = true;
					}
					this.streamContainer.Dispose();
				}
			}
		}

		// Token: 0x0400005F RID: 95
		private readonly string fileName;

		// Token: 0x04000060 RID: 96
		private readonly CancellationToken cancellationToken;

		// Token: 0x04000061 RID: 97
		private readonly DownloadSettings downloadSettings;

		// Token: 0x04000062 RID: 98
		private readonly IDownloadPool downloadPool;

		// Token: 0x04000063 RID: 99
		private readonly TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();

		// Token: 0x04000064 RID: 100
		private readonly Lazy<IDownloadStrategy> downloadStrategyLazy;

		// Token: 0x04000065 RID: 101
		private readonly List<Lazy<Task>> chunks = new List<Lazy<Task>>();

		// Token: 0x04000066 RID: 102
		private readonly string temporaryFileName;

		// Token: 0x04000067 RID: 103
		private readonly BlockingDispatcher<DownloadProgressInfo> blockingDispatcher;

		// Token: 0x04000068 RID: 104
		private readonly IProgress<DownloadProgressInfo> progress;

		// Token: 0x04000069 RID: 105
		private MetadataStreamContainer streamContainer;

		// Token: 0x0400006A RID: 106
		private bool isRunningToCompletion;
	}
}
