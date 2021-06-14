using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftwareRepository.Discovery;
using SoftwareRepository.Reporting;

namespace SoftwareRepository.Streaming
{
	// Token: 0x0200000E RID: 14
	public class Downloader
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002BEB File Offset: 0x00000DEB
		public Downloader()
		{
			this.TimeoutInMilliseconds = 10000;
			this.MaxParallelConnections = 4;
			this.ChunkSize = this.DefaultChunkSize;
			this.AllowWindowsAuth = false;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002C24 File Offset: 0x00000E24
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002C2C File Offset: 0x00000E2C
		public long ChunkSize { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002C35 File Offset: 0x00000E35
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002C3D File Offset: 0x00000E3D
		public int MaxParallelConnections { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002C46 File Offset: 0x00000E46
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002C4E File Offset: 0x00000E4E
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string SoftwareRepositoryAlternativeBaseUrl { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002C57 File Offset: 0x00000E57
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002C5F File Offset: 0x00000E5F
		public string SoftwareRepositoryAuthenticationToken { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002C68 File Offset: 0x00000E68
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002C70 File Offset: 0x00000E70
		public IWebProxy SoftwareRepositoryProxy { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002C79 File Offset: 0x00000E79
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002C81 File Offset: 0x00000E81
		public string SoftwareRepositoryUserAgent { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002C8A File Offset: 0x00000E8A
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00002C92 File Offset: 0x00000E92
		public int TimeoutInMilliseconds { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002C9B File Offset: 0x00000E9B
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002CA3 File Offset: 0x00000EA3
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Auth", Justification = "Acceptable abbreviation")]
		public bool AllowWindowsAuth { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600004A RID: 74 RVA: 0x00002CAC File Offset: 0x00000EAC
		// (remove) Token: 0x0600004B RID: 75 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public event DownloadReadyEventHandler DownloadReady;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600004C RID: 76 RVA: 0x00002D1C File Offset: 0x00000F1C
		// (remove) Token: 0x0600004D RID: 77 RVA: 0x00002D54 File Offset: 0x00000F54
		[SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
		public event BestUrlSelectionEventHandler OnUrlSelection;

		// Token: 0x0600004E RID: 78 RVA: 0x00002D8C File Offset: 0x00000F8C
		public async Task GetFileAsync(string packageId, string filename, Streamer streamer)
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			DownloadReadyEventArgs downloadReadyEvent = null;
			DownloadReadyEventHandler value = delegate(object _, DownloadReadyEventArgs e)
			{
				downloadReadyEvent = e;
			};
			this.DownloadReady += value;
			Action<DownloadProgressInfo> action = delegate(DownloadProgressInfo info)
			{
				if (info.BytesReceived >= info.TotalBytes)
				{
					cts.Cancel();
				}
			};
			await this.GetFileAsync(packageId, filename, streamer, cts.Token, new DownloadProgress<DownloadProgressInfo>(action));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002DEC File Offset: 0x00000FEC
		public async Task GetFileAsync(string packageId, string filename, Streamer streamer, DownloadProgress<DownloadProgressInfo> progress)
		{
			await this.GetFileAsync(packageId, filename, streamer, CancellationToken.None, progress);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002E54 File Offset: 0x00001054
		public async Task GetFileAsync(string packageId, string filename, Streamer streamer, CancellationToken cancellationToken)
		{
			await this.GetFileAsync(packageId, filename, streamer, cancellationToken, null);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002EBC File Offset: 0x000010BC
		public async Task GetFileAsync(string packageId, string filename, Streamer streamer, CancellationToken cancellationToken, DownloadProgress<DownloadProgressInfo> progress)
		{
			Exception exception = null;
			int reportStatus = -1;
			DateTime reportTimeBegin = DateTime.Now;
			FileUrlResult fileUrlResult = null;
			ChunkManager chunkManager = null;
			SemaphoreSlim syncLock = new SemaphoreSlim(1, 1);
			Downloader.ProgressWrapper progressWrapper = new Downloader.ProgressWrapper(progress);
			UrlSelectionResult urlSelectionResult = new UrlSelectionResult();
			try
			{
				try
				{
					FileUrlResult fileUrlResult2 = await this.GetFileUrlAsync(packageId, filename, cancellationToken);
					fileUrlResult = fileUrlResult2;
					List<string> fileUrls = fileUrlResult.GetFileUrls();
					if (fileUrlResult.StatusCode != HttpStatusCode.OK || fileUrls.Count <= 0)
					{
						throw new DownloadException(404, "File not found (" + filename + ").");
					}
					long streamSize = streamer.GetStream().Length;
					if (streamSize > fileUrlResult.FileSize)
					{
						throw new DownloadException(412, "Incorrect file size, can't resume download. Stream contains more data than expected.");
					}
					bool flag = streamSize == fileUrlResult.FileSize;
					if (flag)
					{
						flag = await Downloader.FileIntegrityPreservedAsync(fileUrlResult.Checksum, streamer.GetStream());
					}
					if (flag)
					{
						this.ReportCompleted(progressWrapper, packageId, filename, fileUrlResult.FileSize);
						return;
					}
					byte[] array = streamer.GetMetadata();
					if (array != null)
					{
						chunkManager = new ChunkManager(DownloadMetadata.Deserialize(array), this.ChunkSize, fileUrlResult.FileSize, filename, streamer, syncLock, cancellationToken);
						if (streamSize < chunkManager.ProgressBytes)
						{
							array = null;
							streamer.ClearMetadata();
						}
					}
					if (array == null)
					{
						chunkManager = new ChunkManager(this.ChunkSize, fileUrlResult.FileSize, filename, streamer, syncLock, cancellationToken);
					}
					chunkManager.SoftwareRepositoryProxy = this.SoftwareRepositoryProxy;
					chunkManager.ChunkTimeoutInMilliseconds = new int?(this.TimeoutInMilliseconds);
					chunkManager.AllowWindowsAuth = this.AllowWindowsAuth;
					this.OnUrlSelection += delegate(UrlSelectionResult result)
					{
						urlSelectionResult = result;
					};
					string fileUrl = await this.SelectBestUrlAsync(chunkManager.GetTestChunk(), fileUrlResult, streamer, cancellationToken, progressWrapper, chunkManager);
					chunkManager.FileUrl = fileUrl;
					if (chunkManager.IsDownloaded)
					{
						reportStatus = 200;
					}
					else
					{
						reportStatus = await this.DownloadAsync(chunkManager, fileUrlResult, streamer, progressWrapper);
					}
				}
				catch (OperationCanceledException ex)
				{
					reportStatus = 206;
					exception = ex;
				}
				catch (HttpRequestException innerException)
				{
					reportStatus = 999;
					exception = new DownloadException(reportStatus, "HttpRequestException.", innerException);
				}
				catch (DownloadException ex2)
				{
					reportStatus = ex2.StatusCode;
					exception = ex2;
				}
				if (chunkManager != null)
				{
					if (chunkManager.IsDownloaded)
					{
						streamer.ClearMetadata();
					}
					else
					{
						chunkManager.SaveMetadata(streamer, true);
					}
				}
				await streamer.GetStream().FlushAsync();
			}
			catch (Exception innerException2)
			{
				if (reportStatus == -1)
				{
					reportStatus = 999;
				}
				exception = new DownloadException(999, "Unknown exception.", innerException2);
			}
			if (exception == null)
			{
				if (!(await Downloader.FileIntegrityPreservedAsync(fileUrlResult.Checksum, streamer.GetStream())))
				{
					reportStatus = 417;
					exception = new DownloadException(reportStatus, "File integrity error. MD5 checksum of the file does not match with data received from server.");
				}
				else
				{
					reportStatus = 200;
				}
			}
			try
			{
				long num = (long)((int)(DateTime.Now - reportTimeBegin).TotalSeconds);
				if (num <= 0L)
				{
					num = 1L;
				}
				List<string> list = new List<string>();
				list.Add(urlSelectionResult.ToJson());
				await new Reporter
				{
					SoftwareRepositoryAlternativeBaseUrl = this.SoftwareRepositoryAlternativeBaseUrl,
					SoftwareRepositoryUserAgent = this.SoftwareRepositoryUserAgent,
					SoftwareRepositoryProxy = this.SoftwareRepositoryProxy
				}.SendDownloadReport(packageId, filename, list, reportStatus, num, fileUrlResult.FileSize, Math.Min(this.MaxParallelConnections, chunkManager.TotalChunks), cancellationToken);
			}
			catch (Exception)
			{
			}
			if (exception != null)
			{
				throw exception;
			}
			this.ReportCompleted(progressWrapper, packageId, filename, fileUrlResult.FileSize);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002F2C File Offset: 0x0000112C
		private static async Task<bool> FileIntegrityPreservedAsync(List<SoftwareFileChecksum> checksums, Stream stream)
		{
			Downloader.<>c__DisplayClass53_1 CS$<>8__locals1 = new Downloader.<>c__DisplayClass53_1();
			CS$<>8__locals1.stream = stream;
			bool result;
			if (checksums == null)
			{
				result = false;
			}
			else
			{
				bool ret = false;
				foreach (SoftwareFileChecksum checksum in checksums)
				{
					if (checksum != null && checksum.ChecksumType != null && checksum.Value != null)
					{
						if (checksum.ChecksumType.ToUpperInvariant() == "MD5")
						{
							Downloader.<>c__DisplayClass53_0 CS$<>8__locals2 = new Downloader.<>c__DisplayClass53_0();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							CS$<>8__locals2.md5 = MD5.Create();
							try
							{
								CS$<>8__locals2.CS$<>8__locals1.stream.Seek(0L, SeekOrigin.Begin);
								byte[] array = await Task.Run<byte[]>(() => CS$<>8__locals2.md5.ComputeHash(CS$<>8__locals2.CS$<>8__locals1.stream));
								string value = Convert.ToBase64String(array);
								string value2 = BitConverter.ToString(array).Replace("-", string.Empty).ToLowerInvariant();
								if (!checksum.Value.Equals(value) && !checksum.Value.ToLowerInvariant().Equals(value2))
								{
									return false;
								}
								ret = true;
							}
							finally
							{
								if (CS$<>8__locals2.md5 != null)
								{
									((IDisposable)CS$<>8__locals2.md5).Dispose();
								}
							}
							CS$<>8__locals2 = null;
						}
						checksum = null;
					}
				}
				List<SoftwareFileChecksum>.Enumerator enumerator = default(List<SoftwareFileChecksum>.Enumerator);
				result = ret;
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002F7C File Offset: 0x0000117C
		private async Task<FileUrlResult> GetFileUrlAsync(string packageId, string filename, CancellationToken cancellationToken)
		{
			FileUrlResult ret = new FileUrlResult();
			try
			{
				string text = "https://api.swrepository.com";
				if (this.SoftwareRepositoryAlternativeBaseUrl != null)
				{
					text = this.SoftwareRepositoryAlternativeBaseUrl;
				}
				Uri requestUri = new Uri(string.Concat(new string[]
				{
					text,
					"/rest-api/discovery/1/package/",
					packageId,
					"/file/",
					filename,
					"/urls"
				}));
				HttpClient httpClient = null;
				if (this.SoftwareRepositoryProxy != null)
				{
					httpClient = new HttpClient(new HttpClientHandler
					{
						Proxy = this.SoftwareRepositoryProxy,
						UseProxy = true
					});
				}
				else
				{
					httpClient = new HttpClient();
				}
				string input = "SoftwareRepository";
				if (this.SoftwareRepositoryUserAgent != null)
				{
					input = this.SoftwareRepositoryUserAgent;
				}
				httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(input);
				if (this.SoftwareRepositoryAuthenticationToken != null)
				{
					httpClient.DefaultRequestHeaders.Add("X-Authentication", this.SoftwareRepositoryAuthenticationToken);
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.SoftwareRepositoryAuthenticationToken);
				}
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri, cancellationToken);
				HttpResponseMessage responseMsg = httpResponseMessage;
				if (responseMsg.StatusCode == HttpStatusCode.OK)
				{
					string s = await responseMsg.Content.ReadAsStringAsync();
					ret = (FileUrlResult)new DataContractJsonSerializer(typeof(FileUrlResult)).ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(s)));
				}
				ret.StatusCode = responseMsg.StatusCode;
				httpClient.Dispose();
				httpClient = null;
				responseMsg = null;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new DownloadException(999, "Cannot get file url.", innerException);
			}
			return ret;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FDC File Offset: 0x000011DC
		private async Task<string> SelectBestUrlAsync(DownloadChunk testChunk, FileUrlResult fileUrlResult, Streamer streamer, CancellationToken cancellationToken, Downloader.ProgressWrapper progress, ChunkManager chunkManager)
		{
			Downloader.<>c__DisplayClass55_0 CS$<>8__locals1 = new Downloader.<>c__DisplayClass55_0();
			CS$<>8__locals1.progress = progress;
			CS$<>8__locals1.chunkManager = chunkManager;
			CS$<>8__locals1.urlSelectionResult = new UrlSelectionResult();
			CS$<>8__locals1.urlSelectionResult.TestBytes = testChunk.Bytes;
			List<string> fileUrls = fileUrlResult.GetFileUrls();
			string result;
			if (fileUrls.Count == 1)
			{
				CS$<>8__locals1.urlSelectionResult.UrlResults.Add(new UrlResult
				{
					FileUrl = fileUrls.First<string>(),
					IsSelected = true
				});
				if (this.OnUrlSelection != null)
				{
					this.OnUrlSelection(CS$<>8__locals1.urlSelectionResult);
				}
				result = fileUrls.First<string>();
			}
			else
			{
				CS$<>8__locals1.urlSelectionResult.UrlResults.AddRange(from x in fileUrlResult.GetFileUrls()
				select new UrlResult
				{
					FileUrl = x
				});
				CS$<>8__locals1.ret = null;
				CS$<>8__locals1.exception = null;
				Task<int> winner = null;
				int statusCode = -1;
				List<DownloadChunk> chunks = new List<DownloadChunk>();
				List<Task<int>> tasks = new List<Task<int>>();
				CS$<>8__locals1.currentDownloaded = CS$<>8__locals1.chunkManager.ProgressBytes;
				CS$<>8__locals1.bestChunk = 0L;
				DownloadChunk chunk;
				DownloadProgressEventHandler progressHandler = delegate(object sender, EventArgs args)
				{
					DownloadChunk chunk = sender as DownloadChunk;
					(from x in CS$<>8__locals1.urlSelectionResult.UrlResults
					where x.FileUrl.Equals(chunk.Url, StringComparison.OrdinalIgnoreCase)
					select x).ToList<UrlResult>().ForEach(delegate(UrlResult x)
					{
						x.TestSpeed = chunk.DownloadSpeed;
						x.BytesRead = chunk.BytesRead;
					});
					if (chunk.BytesRead > CS$<>8__locals1.bestChunk)
					{
						CS$<>8__locals1.bestChunk = chunk.BytesRead;
						CS$<>8__locals1.progress.Report(new DownloadProgressInfo(CS$<>8__locals1.currentDownloaded + CS$<>8__locals1.bestChunk, CS$<>8__locals1.chunkManager.FileSize, CS$<>8__locals1.chunkManager.FileName));
					}
				};
				Downloader.<>c__DisplayClass55_2 CS$<>8__locals2 = new Downloader.<>c__DisplayClass55_2();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.cts = new CancellationTokenSource();
				try
				{
					using (cancellationToken.Register(delegate()
					{
						CS$<>8__locals2.cts.Cancel();
					}))
					{
						using (List<string>.Enumerator enumerator = fileUrls.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string url = enumerator.Current;
								DownloadChunk downloadChunk = testChunk.Clone();
								downloadChunk.Url = url;
								downloadChunk.OutStream = new MemoryStream();
								downloadChunk.AllowSeek = false;
								downloadChunk.DownloadProgress += progressHandler;
								chunks.Add(downloadChunk);
								tasks.Add(downloadChunk.Download());
							}
							goto IL_4DF;
						}
						IL_2AD:
						try
						{
							Task<int> task = await Task.WhenAny<int>(tasks);
							winner = task;
							statusCode = await winner;
						}
						catch (OperationCanceledException exception)
						{
							CS$<>8__locals2.CS$<>8__locals1.exception = exception;
						}
						catch (Exception exception2)
						{
							if (!(CS$<>8__locals2.CS$<>8__locals1.exception is OperationCanceledException))
							{
								CS$<>8__locals2.CS$<>8__locals1.exception = exception2;
							}
						}
						finally
						{
							if (statusCode == -1)
							{
								int index = tasks.IndexOf(winner);
								tasks.Remove(winner);
								fileUrls.RemoveAt(index);
								DownloadChunk chunk = chunks[index];
								Func<Exception, string> walkException = null;
								walkException = delegate(Exception e)
								{
									if (e.InnerException != null)
									{
										return string.Format(CultureInfo.InvariantCulture, "{0}: {1} ({2})", new object[]
										{
											e.GetType().Name,
											e.Message,
											walkException(e.InnerException)
										});
									}
									return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", new object[]
									{
										e.GetType().Name,
										e.Message
									});
								};
								(from x in CS$<>8__locals2.CS$<>8__locals1.urlSelectionResult.UrlResults
								where x.FileUrl.Equals(chunk.Url, StringComparison.OrdinalIgnoreCase)
								select x).ToList<UrlResult>().ForEach(delegate(UrlResult x)
								{
									x.Error = walkException(CS$<>8__locals2.CS$<>8__locals1.exception);
								});
								chunk.OutStream.Dispose();
								chunk.DownloadProgress -= progressHandler;
								chunks.RemoveAt(index);
							}
						}
						IL_4DF:
						if (statusCode == -1 && tasks.Count > 0)
						{
							goto IL_2AD;
						}
					}
					CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
					if (!CS$<>8__locals2.cts.IsCancellationRequested)
					{
						CS$<>8__locals2.cts.Cancel();
					}
				}
				finally
				{
					if (CS$<>8__locals2.cts != null)
					{
						((IDisposable)CS$<>8__locals2.cts).Dispose();
					}
				}
				CS$<>8__locals2 = null;
				if (statusCode != -1)
				{
					int index2 = tasks.IndexOf(winner);
					CS$<>8__locals1.ret = fileUrls[index2];
					(from x in CS$<>8__locals1.urlSelectionResult.UrlResults
					where x.FileUrl.Equals(CS$<>8__locals1.ret, StringComparison.OrdinalIgnoreCase)
					select x).ToList<UrlResult>().ForEach(delegate(UrlResult x)
					{
						x.IsSelected = true;
					});
					chunk = chunks[index2];
					chunk.OutStream.Seek(0L, SeekOrigin.Begin);
					streamer.GetStream().Seek(chunk.BytesFrom, SeekOrigin.Begin);
					await chunk.OutStream.CopyToAsync(streamer.GetStream());
					await streamer.GetStream().FlushAsync();
					CS$<>8__locals1.chunkManager.MarkDownloaded(chunk);
					CS$<>8__locals1.chunkManager.SaveMetadata(streamer, false);
					chunk = null;
				}
				foreach (DownloadChunk downloadChunk2 in chunks)
				{
					downloadChunk2.OutStream.Dispose();
					downloadChunk2.DownloadProgress -= progressHandler;
				}
				if (statusCode == -1 && CS$<>8__locals1.exception != null)
				{
					throw CS$<>8__locals1.exception;
				}
				if (this.OnUrlSelection != null)
				{
					this.OnUrlSelection(CS$<>8__locals1.urlSelectionResult);
				}
				result = CS$<>8__locals1.ret;
			}
			return result;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003054 File Offset: 0x00001254
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fileUrlResult")]
		private async Task<int> DownloadAsync(ChunkManager chunkManager, FileUrlResult fileUrlResult, Streamer streamer, Downloader.ProgressWrapper progress)
		{
			int maxParallelConnections = this.MaxParallelConnections;
			DownloadProgressEventHandler progressHandler = delegate(object sender, EventArgs args)
			{
				progress.Report(new DownloadProgressInfo(chunkManager.ProgressBytes, chunkManager.FileSize, chunkManager.FileName));
			};
			List<DownloadChunk> chunks = new List<DownloadChunk>();
			List<Task<int>> tasks = new List<Task<int>>();
			for (int i = 0; i < maxParallelConnections; i++)
			{
				DownloadChunk nextChunk = chunkManager.GetNextChunk();
				if (nextChunk == null)
				{
					break;
				}
				nextChunk.DownloadProgress += progressHandler;
				chunks.Add(nextChunk);
				tasks.Add(nextChunk.Download());
			}
			int ret = 200;
			while (tasks.Count > 0)
			{
				Task<int> task = await Task.WhenAny<int>(tasks);
				int taskIndex = tasks.IndexOf(task);
				DownloadChunk chunk = chunks[taskIndex];
				chunk.DownloadProgress -= progressHandler;
				int num = await task;
				if (num != 200 && num != 206)
				{
					return num;
				}
				chunkManager.MarkDownloaded(chunk);
				chunkManager.SaveMetadata(streamer, false);
				chunk = chunkManager.GetNextChunk();
				if (chunk == null)
				{
					tasks.RemoveAt(taskIndex);
					chunks.RemoveAt(taskIndex);
				}
				else
				{
					chunk.OutStream = streamer.GetStream();
					chunk.DownloadProgress += progressHandler;
					chunks[taskIndex] = chunk;
					tasks[taskIndex] = chunk.Download();
				}
				chunk = null;
			}
			streamer.ClearMetadata();
			return ret;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000030B2 File Offset: 0x000012B2
		private void ReportCompleted(Downloader.ProgressWrapper progressWrapper, string packageId, string fileName, long fileSize)
		{
			progressWrapper.Report(new DownloadProgressInfo(fileSize, fileSize, fileName));
			if (this.DownloadReady != null)
			{
				this.DownloadReady(this, new DownloadReadyEventArgs(packageId, fileName));
			}
		}

		// Token: 0x0400002A RID: 42
		private const int Canceled = 206;

		// Token: 0x0400002B RID: 43
		private const int Completed = 200;

		// Token: 0x0400002C RID: 44
		private long DefaultChunkSize = 1048576L;

		// Token: 0x0400002D RID: 45
		private const int DefaultParallelConnections = 4;

		// Token: 0x0400002E RID: 46
		private const string DefaultSoftwareRepositoryBaseUrl = "https://api.swrepository.com";

		// Token: 0x0400002F RID: 47
		private const string DefaultSoftwareRepositoryFileUrl = "/rest-api/discovery";

		// Token: 0x04000030 RID: 48
		private const string DefaultSoftwareRepositoryFileUrlApiVersion = "/1";

		// Token: 0x04000031 RID: 49
		private const string DefaultSoftwareRepositoryUserAgent = "SoftwareRepository";

		// Token: 0x04000032 RID: 50
		private const int DefaultTimeoutInMilliseconds = 10000;

		// Token: 0x02000029 RID: 41
		private class ProgressWrapper
		{
			// Token: 0x06000160 RID: 352 RVA: 0x00004501 File Offset: 0x00002701
			internal ProgressWrapper(DownloadProgress<DownloadProgressInfo> listener)
			{
				this.Listener = listener;
			}

			// Token: 0x06000161 RID: 353 RVA: 0x00004518 File Offset: 0x00002718
			internal void Report(DownloadProgressInfo report)
			{
				if (this.Listener != null)
				{
					lock (this)
					{
						if (this.LargestReported < report.BytesReceived)
						{
							this.Listener.Report(report);
							this.LargestReported = report.BytesReceived;
						}
					}
				}
			}

			// Token: 0x040000C5 RID: 197
			private long LargestReported = -1L;

			// Token: 0x040000C6 RID: 198
			private DownloadProgress<DownloadProgressInfo> Listener;
		}
	}
}
