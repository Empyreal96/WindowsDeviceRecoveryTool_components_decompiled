using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Nokia.Mira.Primitives;

namespace Nokia.Mira
{
	// Token: 0x02000022 RID: 34
	public class DownloadPool : IDownloadPool
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00002FA8 File Offset: 0x000011A8
		public DownloadPool(int maxDownloads = 8)
		{
			this.maxDownloads = maxDownloads;
			this.availableWorkerCount = maxDownloads;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002FD4 File Offset: 0x000011D4
		public static IDownloadPool DefaultInstance
		{
			get
			{
				return DownloadPool.LazyDownloadPool.Value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002FE0 File Offset: 0x000011E0
		public int MaxDownloads
		{
			get
			{
				return this.maxDownloads;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003050 File Offset: 0x00001250
		public void QueueDownloadTask(DownloadTask task)
		{
			Task.Factory.StartNew(delegate()
			{
				lock (this.syncRoot)
				{
					this.tasks.Add(task);
					this.DispatchNextTask();
				}
			}, TaskCreationOptions.LongRunning);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000310C File Offset: 0x0000130C
		private void DispatchNextTask()
		{
			if (this.availableWorkerCount <= 0 || this.tasks.Count == 0)
			{
				return;
			}
			DownloadTask downloadTask;
			Lazy<Task> lazy;
			if (!this.TryGetNextChunk(out lazy, out downloadTask))
			{
				return;
			}
			this.availableWorkerCount--;
			Task value = lazy.Value;
			value.ContinueWith(delegate(Task t)
			{
				lock (this.syncRoot)
				{
					if (!downloadTask.IsRunningToCompletion && (t.IsFaulted || t.IsCanceled))
					{
						this.OnDownloadTaskCompleted(downloadTask);
					}
					this.OnDownloadChunkCompleted();
				}
			}, TaskContinuationOptions.LongRunning);
			this.DispatchNextTask();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003180 File Offset: 0x00001380
		private bool TryGetNextChunk(out Lazy<Task> chunk, out DownloadTask downloadTask)
		{
			chunk = null;
			downloadTask = null;
			int i = 0;
			while (i < this.tasks.Count)
			{
				downloadTask = this.tasks[i];
				try
				{
					switch (downloadTask.GetNextDownloadChunk(out chunk))
					{
					case ChunkStatus.WaitingToRun:
						return true;
					case ChunkStatus.WaitingToCompletion:
						this.OnTaskCompleted(downloadTask);
						return true;
					case ChunkStatus.WaitingForResources:
						i++;
						break;
					case ChunkStatus.RanToCompletion:
						this.OnTaskCompleted(downloadTask);
						break;
					}
				}
				catch (WebException ex)
				{
					if (ex.Status == WebExceptionStatus.Timeout && this.availableWorkerCount < this.maxDownloads)
					{
						return false;
					}
					this.OnTaskFaulted(downloadTask, ex);
				}
				catch (OperationCanceledException)
				{
					this.OnTaskCanceled(downloadTask);
				}
				catch (Exception error)
				{
					this.OnTaskFaulted(downloadTask, error);
				}
			}
			return false;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000326C File Offset: 0x0000146C
		private void OnTaskCanceled(DownloadTask task)
		{
			task.SetCanceled();
			this.tasks.Remove(task);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003281 File Offset: 0x00001481
		private void OnTaskCompleted(DownloadTask task)
		{
			task.SetCompleted();
			this.tasks.Remove(task);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003296 File Offset: 0x00001496
		private void OnTaskFaulted(DownloadTask task, Exception error)
		{
			task.SetFaulted(error);
			this.tasks.Remove(task);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000032AC File Offset: 0x000014AC
		private void OnDownloadTaskCompleted(DownloadTask task)
		{
			task.SetCompleted();
			this.tasks.Remove(task);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000032C1 File Offset: 0x000014C1
		private void OnDownloadChunkCompleted()
		{
			this.availableWorkerCount++;
			this.DispatchNextTask();
		}

		// Token: 0x0400003E RID: 62
		private static readonly Lazy<IDownloadPool> LazyDownloadPool = new Lazy<IDownloadPool>(() => new DownloadPool(8));

		// Token: 0x0400003F RID: 63
		private readonly object syncRoot = new object();

		// Token: 0x04000040 RID: 64
		private readonly List<DownloadTask> tasks = new List<DownloadTask>();

		// Token: 0x04000041 RID: 65
		private readonly int maxDownloads;

		// Token: 0x04000042 RID: 66
		private int availableWorkerCount;
	}
}
