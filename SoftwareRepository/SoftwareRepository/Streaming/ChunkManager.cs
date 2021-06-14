using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace SoftwareRepository.Streaming
{
	// Token: 0x0200000A RID: 10
	internal class ChunkManager
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002227 File Offset: 0x00000427
		internal int TotalChunks
		{
			get
			{
				return this.ChunkStates.Length;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002231 File Offset: 0x00000431
		internal bool IsDownloaded
		{
			get
			{
				return this.DownloadedChunks == this.TotalChunks;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002244 File Offset: 0x00000444
		internal long ProgressBytes
		{
			get
			{
				object stateLock = this.StateLock;
				long result;
				lock (stateLock)
				{
					long num = (long)this.DownloadedChunks * this.ChunkSize;
					if (this.FileSize % this.ChunkSize != 0L && this.ChunkStates[this.ChunkStates.Length - 1] == ChunkState.Downlodaded)
					{
						num -= this.ChunkSize;
						num += this.FileSize % this.ChunkSize;
					}
					foreach (DownloadChunk downloadChunk in this.DownloadingChunks)
					{
						num += downloadChunk.BytesRead;
					}
					if (this.ResumedPartialProgress != null)
					{
						foreach (long num2 in this.ResumedPartialProgress.Values)
						{
							num += num2;
						}
					}
					result = num;
				}
				return result;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002364 File Offset: 0x00000564
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000236C File Offset: 0x0000056C
		internal HashSet<DownloadChunk> DownloadingChunks { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002375 File Offset: 0x00000575
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0000237D File Offset: 0x0000057D
		internal long ChunkSize { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002386 File Offset: 0x00000586
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000238E File Offset: 0x0000058E
		internal long FileSize { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002397 File Offset: 0x00000597
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000239F File Offset: 0x0000059F
		internal string FileName { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023A8 File Offset: 0x000005A8
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000023B0 File Offset: 0x000005B0
		internal string FileUrl { get; set; }

		// Token: 0x06000025 RID: 37 RVA: 0x000023B9 File Offset: 0x000005B9
		internal ChunkManager(long chunkSize, long fileSize, string filename, Streamer streamer, SemaphoreSlim syncLock, CancellationToken cancellationToken) : this(null, chunkSize, fileSize, filename, streamer, syncLock, cancellationToken)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000023CC File Offset: 0x000005CC
		internal ChunkManager(DownloadMetadata metadata, long chunkSize, long fileSize, string filename, Streamer streamer, SemaphoreSlim syncLock, CancellationToken cancellationToken)
		{
			long num = fileSize / chunkSize + ((fileSize % chunkSize == 0L) ? 0L : 1L);
			if (metadata == null || (long)metadata.ChunkStates.Length != num)
			{
				this.ChunkStates = new ChunkState[num];
			}
			else
			{
				this.ChunkStates = metadata.ChunkStates;
				this.ResumedPartialProgress = metadata.PartialProgress;
			}
			this.NextUndownloadedIndex = 0;
			this.DownloadedChunks = 0;
			while (this.NextUndownloadedIndex < this.ChunkStates.Length && this.ChunkStates[this.NextUndownloadedIndex] != ChunkState.Undownloaded)
			{
				if (this.ChunkStates[this.NextUndownloadedIndex] == ChunkState.Downlodaded)
				{
					this.DownloadedChunks++;
				}
				this.NextUndownloadedIndex++;
			}
			for (long num2 = (long)(this.NextUndownloadedIndex + 1); num2 < (long)this.ChunkStates.Length; num2 += 1L)
			{
				if (this.ChunkStates[(int)(checked((IntPtr)num2))] == ChunkState.Downlodaded)
				{
					this.DownloadedChunks++;
				}
			}
			this.ChunkSize = chunkSize;
			this.DownloadingChunks = new HashSet<DownloadChunk>();
			this.FileName = filename;
			this.FileSize = fileSize;
			this.ClientCancellationToken = cancellationToken;
			this.SyncLock = syncLock;
			this.Streamer = streamer;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024FC File Offset: 0x000006FC
		internal DownloadChunk GetNextChunk()
		{
			object stateLock = this.StateLock;
			DownloadChunk result;
			lock (stateLock)
			{
				DownloadChunk downloadChunk;
				if (this.ResumedPartialProgress != null && this.ResumedPartialProgress.Count > 0)
				{
					int num = this.ResumedPartialProgress.Keys.First<int>();
					long num2 = this.ResumedPartialProgress[num];
					long num3 = (long)num * this.ChunkSize + num2;
					long downloadLength = Math.Min(this.ChunkSize - num2, this.FileSize - num3);
					downloadChunk = this.MakeChunk(num, num3, downloadLength);
					this.ResumedPartialProgress.Remove(num);
					if (this.ResumedPartialProgress.Count == 0)
					{
						this.ResumedPartialProgress = null;
					}
				}
				else
				{
					while (this.NextUndownloadedIndex < this.ChunkStates.Length && this.ChunkStates[this.NextUndownloadedIndex] != ChunkState.Undownloaded)
					{
						this.NextUndownloadedIndex++;
					}
					if (this.NextUndownloadedIndex >= this.ChunkStates.Length)
					{
						return null;
					}
					long num4 = (long)this.NextUndownloadedIndex * this.ChunkSize;
					long downloadLength2 = Math.Min(this.ChunkSize, this.FileSize - num4);
					downloadChunk = this.MakeChunk(this.NextUndownloadedIndex, num4, downloadLength2);
					this.NextUndownloadedIndex++;
				}
				this.ChunkStates[downloadChunk.ChunkIndex] = ChunkState.PartiallyDownloaded;
				this.DownloadingChunks.Add(downloadChunk);
				result = downloadChunk;
			}
			return result;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002680 File Offset: 0x00000880
		internal DownloadChunk GetTestChunk()
		{
			object stateLock = this.StateLock;
			DownloadChunk result;
			lock (stateLock)
			{
				while (this.NextUndownloadedIndex < this.ChunkStates.Length && this.ChunkStates[this.NextUndownloadedIndex] != ChunkState.Undownloaded)
				{
					this.NextUndownloadedIndex++;
				}
				if (this.NextUndownloadedIndex < this.ChunkStates.Length - 1 || (this.NextUndownloadedIndex == this.ChunkStates.Length - 1 && this.FileSize % this.ChunkSize == 0L))
				{
					long startPosition = (long)this.NextUndownloadedIndex * this.ChunkSize;
					result = this.MakeChunk(this.NextUndownloadedIndex, startPosition, this.ChunkSize);
				}
				else
				{
					int num = -1;
					long num2 = -1L;
					long num3 = 0L;
					if (this.ChunkStates[this.ChunkStates.Length - 1] == ChunkState.Undownloaded)
					{
						num = this.ChunkStates.Length - 1;
						num2 = this.FileSize % this.ChunkSize;
					}
					if (this.ResumedPartialProgress != null)
					{
						foreach (int num4 in this.ResumedPartialProgress.Keys)
						{
							long num5 = ((num4 == this.ChunkStates.Length - 1) ? (this.FileSize % this.ChunkSize) : this.ChunkSize) - this.ResumedPartialProgress[num4];
							if (num5 > num2)
							{
								num = num4;
								num2 = num5;
								num3 = this.ResumedPartialProgress[num4];
							}
						}
					}
					if (num >= 0 && num2 > 0L)
					{
						int num6 = num;
						result = this.MakeChunk(num6, (long)num6 * this.ChunkSize + num3, num2);
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002854 File Offset: 0x00000A54
		internal void MarkDownloaded(DownloadChunk chunk)
		{
			object stateLock = this.StateLock;
			lock (stateLock)
			{
				if (this.ChunkStates[chunk.ChunkIndex] != ChunkState.Downlodaded)
				{
					this.DownloadedChunks++;
					this.ChunkStates[chunk.ChunkIndex] = ChunkState.Downlodaded;
				}
				this.DownloadingChunks.Remove(chunk);
				if (this.ResumedPartialProgress != null && this.ResumedPartialProgress.ContainsKey(chunk.ChunkIndex))
				{
					this.ResumedPartialProgress.Remove(chunk.ChunkIndex);
					if (this.ResumedPartialProgress.Count == 0)
					{
						this.ResumedPartialProgress = null;
					}
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002908 File Offset: 0x00000B08
		internal DownloadMetadata GetMetadata(bool includeInProgress)
		{
			object stateLock = this.StateLock;
			DownloadMetadata result;
			lock (stateLock)
			{
				if (includeInProgress)
				{
					DownloadMetadata downloadMetadata = new DownloadMetadata();
					downloadMetadata.ChunkStates = (this.ChunkStates.Clone() as ChunkState[]);
					downloadMetadata.PartialProgress = this.DownloadingChunks.ToDictionary((DownloadChunk d) => d.ChunkIndex, (DownloadChunk d) => d.BytesRead);
					result = downloadMetadata;
				}
				else
				{
					DownloadMetadata downloadMetadata2 = new DownloadMetadata();
					downloadMetadata2.ChunkStates = this.ChunkStates.Select(delegate(ChunkState s)
					{
						if (s != ChunkState.Downlodaded)
						{
							return ChunkState.Undownloaded;
						}
						return ChunkState.Downlodaded;
					}).ToArray<ChunkState>();
					downloadMetadata2.PartialProgress = null;
					result = downloadMetadata2;
				}
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000029F8 File Offset: 0x00000BF8
		internal void SaveMetadata(Streamer target, bool includeInProgress = false)
		{
			byte[] array = this.GetMetadata(includeInProgress).Serialize();
			if (array != null)
			{
				target.SetMetadata(array);
				return;
			}
			target.ClearMetadata();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A24 File Offset: 0x00000C24
		private DownloadChunk MakeChunk(int chunkIndex, long startPosition, long downloadLength)
		{
			DownloadChunk downloadChunk = new DownloadChunk(this.FileName, this.FileUrl, startPosition, downloadLength, this.ClientCancellationToken)
			{
				SyncLock = this.SyncLock,
				ChunkIndex = chunkIndex,
				OutStream = this.Streamer.GetStream(),
				FileSize = this.FileSize,
				AllowWindowsAuth = this.AllowWindowsAuth
			};
			if (this.ChunkTimeoutInMilliseconds != null)
			{
				downloadChunk.TimeoutInMilliseconds = this.ChunkTimeoutInMilliseconds.Value;
			}
			if (this.SoftwareRepositoryProxy != null)
			{
				downloadChunk.SoftwareRepositoryProxy = this.SoftwareRepositoryProxy;
			}
			return downloadChunk;
		}

		// Token: 0x04000018 RID: 24
		private object StateLock = new object();

		// Token: 0x04000019 RID: 25
		private Streamer Streamer;

		// Token: 0x0400001A RID: 26
		private SemaphoreSlim SyncLock;

		// Token: 0x0400001B RID: 27
		private ChunkState[] ChunkStates;

		// Token: 0x0400001C RID: 28
		private Dictionary<int, long> ResumedPartialProgress;

		// Token: 0x0400001D RID: 29
		private int NextUndownloadedIndex;

		// Token: 0x0400001E RID: 30
		private int DownloadedChunks;

		// Token: 0x04000024 RID: 36
		private CancellationToken ClientCancellationToken;

		// Token: 0x04000025 RID: 37
		internal int? ChunkTimeoutInMilliseconds;

		// Token: 0x04000026 RID: 38
		internal IWebProxy SoftwareRepositoryProxy;

		// Token: 0x04000027 RID: 39
		internal bool AllowWindowsAuth;
	}
}
