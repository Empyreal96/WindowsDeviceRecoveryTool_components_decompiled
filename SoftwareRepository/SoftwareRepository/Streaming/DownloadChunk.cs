using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000016 RID: 22
	internal class DownloadChunk
	{
		// Token: 0x06000083 RID: 131 RVA: 0x000034E4 File Offset: 0x000016E4
		internal DownloadChunk(string filename, string url, long bytesFrom, long byteCount, CancellationToken cancellationToken)
		{
			this.FileName = filename;
			this.Url = url;
			this.BytesFrom = bytesFrom;
			this.Bytes = byteCount;
			this.CancellationToken = cancellationToken;
			this.TimeoutInMilliseconds = 10000;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003523 File Offset: 0x00001723
		// (set) Token: 0x06000085 RID: 133 RVA: 0x0000352B File Offset: 0x0000172B
		internal string FileName { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003534 File Offset: 0x00001734
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000353C File Offset: 0x0000173C
		internal string Url { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003545 File Offset: 0x00001745
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000354D File Offset: 0x0000174D
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Unused for now but might prove useful later.")]
		internal long FileSize { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003556 File Offset: 0x00001756
		// (set) Token: 0x0600008B RID: 139 RVA: 0x0000355E File Offset: 0x0000175E
		internal IWebProxy SoftwareRepositoryProxy { get; set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600008C RID: 140 RVA: 0x00003568 File Offset: 0x00001768
		// (remove) Token: 0x0600008D RID: 141 RVA: 0x000035A0 File Offset: 0x000017A0
		internal event DownloadProgressEventHandler DownloadProgress;

		// Token: 0x0600008E RID: 142 RVA: 0x000035D5 File Offset: 0x000017D5
		private void OnDownloadProgress(EventArgs e)
		{
			if (this.DownloadProgress != null)
			{
				this.DownloadProgress(this, e);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000035EC File Offset: 0x000017EC
		internal DownloadChunk Clone()
		{
			return new DownloadChunk(this.FileName, this.Url, this.BytesFrom, this.Bytes, this.CancellationToken)
			{
				FileSize = this.FileSize,
				OutStream = this.OutStream,
				SoftwareRepositoryProxy = this.SoftwareRepositoryProxy,
				TimeoutInMilliseconds = this.TimeoutInMilliseconds,
				SyncLock = this.SyncLock,
				ChunkIndex = this.ChunkIndex,
				AllowSeek = this.AllowSeek,
				AllowWindowsAuth = this.AllowWindowsAuth
			};
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000367C File Offset: 0x0000187C
		internal async Task<int> Download()
		{
			int result;
			if (this.Bytes == 0L)
			{
				result = 200;
			}
			else
			{
				int ret = -1;
				DateTime downloadStartTime = DateTime.UtcNow;
				HttpClientHandler httpClientHandler = new HttpClientHandler
				{
					UseDefaultCredentials = this.AllowWindowsAuth
				};
				if (this.SoftwareRepositoryProxy != null)
				{
					httpClientHandler.Proxy = this.SoftwareRepositoryProxy;
					httpClientHandler.UseProxy = true;
				}
				HttpClient httpClient = new HttpClient(httpClientHandler);
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(this.Url));
				long rangeStart = this.BytesFrom + this.BytesRead;
				long rangeEnd = this.BytesFrom + this.Bytes - 1L;
				httpRequestMessage.Headers.Range = new RangeHeaderValue(new long?(rangeStart), new long?(rangeEnd));
				HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, this.CancellationToken);
				HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
				if (httpStatusCode != HttpStatusCode.OK && httpStatusCode != HttpStatusCode.PartialContent)
				{
					httpClient.Dispose();
					throw new DownloadException((int)httpStatusCode, "HTTP Response status code: " + (int)httpStatusCode);
				}
				HttpContentHeaders headers = httpResponseMessage.Content.Headers;
				if (headers.ContentLength == null)
				{
					Diagnostics.Log(LogLevel.Warning, "Missing Content-Length header", new object[0]);
				}
				else if (headers.ContentLength.Value != rangeEnd - rangeStart + 1L)
				{
					throw new DownloadException(0, "Content-Length does not match request range");
				}
				if (headers.ContentRange == null)
				{
					Diagnostics.Log(LogLevel.Warning, "Missing Content-Range header", new object[0]);
				}
				else if (headers.ContentRange.Unit.ToLowerInvariant() != "bytes" || headers.ContentRange.From != rangeStart || headers.ContentRange.To != rangeEnd)
				{
					throw new DownloadException(0, "Content-Range does not match request range");
				}
				using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync())
				{
					byte[] buffer = new byte[4096];
					int bytesRead = 0;
					while ((bytesRead = await DownloadChunk.WithTimeout<int>(stream.ReadAsync(buffer, 0, buffer.Length, this.CancellationToken), this.TimeoutInMilliseconds)) != 0)
					{
						this.CancellationToken.ThrowIfCancellationRequested();
						bytesRead = (int)Math.Min(this.Bytes - this.BytesRead, (long)bytesRead);
						if (this.SyncLock != null)
						{
							await this.SyncLock.WaitAsync();
						}
						try
						{
							if (this.AllowSeek)
							{
								this.OutStream.Seek(this.BytesFrom + this.BytesRead, SeekOrigin.Begin);
							}
							await this.OutStream.WriteAsync(buffer, 0, bytesRead, this.CancellationToken);
							this.BytesRead += (long)bytesRead;
							if (this.BytesRead >= this.Bytes)
							{
								await this.OutStream.FlushAsync();
							}
						}
						catch (OperationCanceledException ex)
						{
							httpClient.Dispose();
							throw ex;
						}
						catch (Exception)
						{
							httpClient.Dispose();
							throw new DownloadException(508, "Download interrupted because disk full, out of memory or other local storage reason prevents storing file locally.");
						}
						finally
						{
							if (this.SyncLock != null)
							{
								this.SyncLock.Release();
							}
						}
						this.DownloadSpeed = (double)this.BytesRead / (DateTime.UtcNow - downloadStartTime).TotalSeconds;
						this.OnDownloadProgress(null);
						if (this.BytesRead >= this.Bytes)
						{
							httpClient.Dispose();
							return (int)httpStatusCode;
						}
					}
					buffer = null;
				}
				Stream stream = null;
				result = ret;
			}
			return result;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000036C4 File Offset: 0x000018C4
		private static async Task<T> WithTimeout<T>(Task<T> task, int time)
		{
			Task delayTask = Task.Delay(time);
			if (await Task.WhenAny(new Task[]
			{
				task,
				delayTask
			}) == delayTask)
			{
				throw new DownloadException(408, "Download request or streaming timeout.");
			}
			return await task;
		}

		// Token: 0x0400004F RID: 79
		private const int DefaultTimeoutInMilliseconds = 10000;

		// Token: 0x04000050 RID: 80
		internal const int BufferSize = 4096;

		// Token: 0x04000053 RID: 83
		internal long BytesFrom;

		// Token: 0x04000054 RID: 84
		internal long Bytes;

		// Token: 0x04000055 RID: 85
		internal long BytesRead;

		// Token: 0x04000056 RID: 86
		internal double DownloadSpeed;

		// Token: 0x04000057 RID: 87
		internal CancellationToken CancellationToken;

		// Token: 0x04000059 RID: 89
		internal Stream OutStream;

		// Token: 0x0400005B RID: 91
		internal int TimeoutInMilliseconds;

		// Token: 0x0400005C RID: 92
		internal SemaphoreSlim SyncLock;

		// Token: 0x0400005D RID: 93
		internal int ChunkIndex;

		// Token: 0x0400005E RID: 94
		internal bool AllowSeek = true;

		// Token: 0x0400005F RID: 95
		internal bool AllowWindowsAuth;
	}
}
