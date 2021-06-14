using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services.DataPackageRules;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using SoftwareRepository.Discovery;
using SoftwareRepository.Streaming;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003E RID: 62
	[PartCreationPolicy(CreationPolicy.Shared)]
	[Export(typeof(MsrService))]
	[Export(typeof(IUseProxy))]
	public class MsrService : BaseRemoteRepository, IUseProxy
	{
		// Token: 0x06000341 RID: 833 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
		[ImportingConstructor]
		public MsrService(FileChecker fileChecker)
		{
			this.fileChecker = fileChecker;
			this.fileChecker.SetProgressHandler(new Action<double>(this.IntegrityCheckProgressChanged));
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000342 RID: 834 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		// (remove) Token: 0x06000343 RID: 835 RVA: 0x0000E52C File Offset: 0x0000C72C
		public event Action<int> IntegrityCheckProgressEvent;

		// Token: 0x06000344 RID: 836 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		public override async Task<PackageFileInfo> CheckLatestPackage(QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			SoftwarePackage package = await this.CheckLatestPackageInternal(queryParameters, cancellationToken);
			return new MsrPackageInfo(string.Empty, package.PackageTitle, package.PackageRevision)
			{
				ManufacturerModelName = package.ManufacturerModelName.FirstOrDefault<string>(),
				PackageFileData = this.ExtractMsrPackageFiles(package)
			};
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		private IEnumerable<MsrPackageInfo.MsrFileInfo> ExtractMsrPackageFiles(SoftwarePackage package)
		{
			return from f in package.Files
			select new MsrPackageInfo.MsrFileInfo
			{
				FileName = f.FileName,
				FileType = (this.ReadFileTypeFromPackage(f, package) ?? f.FileType),
				FileNameWithRevision = this.GenerateSoftwareVersionFile(f.FileName, package.PackageRevision, true),
				FileVersion = this.ReadFileVersionFromPackage(f, package)
			};
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000E840 File Offset: 0x0000CA40
		private string ReadFileVersionFromPackage(SoftwareFile softwareFile, SoftwarePackage originPackage)
		{
			string result = null;
			if (originPackage.ExtendedAttributes != null)
			{
				Dictionary<string, string> dictionary = originPackage.ExtendedAttributes.Dictionary;
				if (dictionary != null)
				{
					KeyValuePair<string, string> keyValuePair = dictionary.FirstOrDefault((KeyValuePair<string, string> kv) => string.Equals(softwareFile.FileName, kv.Value, StringComparison.InvariantCultureIgnoreCase));
					string pattern = "Component(?<component_number>\\d+)_FileName";
					if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && Regex.IsMatch(keyValuePair.Key, pattern))
					{
						Match match = Regex.Match(keyValuePair.Key, pattern);
						string value = match.Groups["component_number"].Value;
						string key = string.Format("Component{0}_Version", value);
						if (dictionary.ContainsKey(key))
						{
							result = dictionary[key];
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E96C File Offset: 0x0000CB6C
		private string ReadFileTypeFromPackage(SoftwareFile softwareFile, SoftwarePackage originPackage)
		{
			string result = null;
			if (originPackage.ExtendedAttributes != null)
			{
				Dictionary<string, string> dictionary = originPackage.ExtendedAttributes.Dictionary;
				if (dictionary != null)
				{
					KeyValuePair<string, string> keyValuePair = dictionary.FirstOrDefault((KeyValuePair<string, string> kv) => string.Equals(softwareFile.FileName, kv.Value, StringComparison.InvariantCultureIgnoreCase));
					string pattern = "Component(?<component_number>\\d+)_FileName";
					if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && Regex.IsMatch(keyValuePair.Key, pattern))
					{
						Match match = Regex.Match(keyValuePair.Key, pattern);
						string value = match.Groups["component_number"].Value;
						string key = string.Format("Component{0}_Type", value);
						if (dictionary.ContainsKey(key))
						{
							result = dictionary[key];
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000EA64 File Offset: 0x0000CC64
		public Tuple<long, long, bool> GetDownloadPackageInformation()
		{
			return new Tuple<long, long, bool>(base.SpeedCalculator.CurrentDownloadedSize, base.TotalFilesSize, base.SpeedCalculator.IsResumed);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000EE80 File Offset: 0x0000D080
		private async Task<SoftwarePackage> CheckLatestPackageInternal(QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			DiscoveryQueryParameters discoveryQueryParameters = new DiscoveryQueryParameters
			{
				ManufacturerName = queryParameters.ManufacturerName,
				ManufacturerModelName = queryParameters.ManufacturerModelName,
				ManufacturerProductLine = queryParameters.ManufacturerProductLine,
				PackageType = queryParameters.PackageType,
				PackageClass = queryParameters.PackageClass,
				ManufacturerHardwareModel = queryParameters.ManufacturerHardwareModel,
				ManufacturerHardwareVariant = queryParameters.ManufacturerHardwareVariant
			};
			if (queryParameters.ExtendedAttributes != null && queryParameters.ExtendedAttributes.Count > 0)
			{
				discoveryQueryParameters.ExtendedAttributes = new ExtendedAttributes
				{
					Dictionary = queryParameters.ExtendedAttributes
				};
			}
			Tracer<MsrService>.WriteInformation("MSR Query: {0}", new object[]
			{
				queryParameters
			});
			DiscoveryParameters discovererParams = new DiscoveryParameters
			{
				Query = discoveryQueryParameters
			};
			Discoverer discoverer = new Discoverer
			{
				SoftwareRepositoryAlternativeBaseUrl = (ApplicationInfo.UseTestServer() ? "https://pvprepo.azurewebsites.net" : "https://api.swrepository.com"),
				SoftwareRepositoryAuthenticationToken = null,
				SoftwareRepositoryProxy = base.Proxy()
			};
			DiscoveryJsonResult result = await discoverer.DiscoverJsonAsync(discovererParams, cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();
			if (result.StatusCode != HttpStatusCode.OK)
			{
				throw new PackageNotFoundException();
			}
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SoftwarePackages));
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result.Result)))
			{
				SoftwarePackages softwarePackages = (SoftwarePackages)serializer.ReadObject(memoryStream);
				if (softwarePackages != null)
				{
					Tracer<MsrService>.WriteInformation("Packages found: {0}", new object[]
					{
						softwarePackages.SoftwarePackageList.Count
					});
					SoftwarePackage softwarePackage = softwarePackages.SoftwarePackageList.FirstOrDefault<SoftwarePackage>();
					if (softwarePackage != null)
					{
						Tracer<MsrService>.WriteInformation("Version: {0}", new object[]
						{
							softwarePackage.PackageRevision
						});
						return softwarePackage;
					}
				}
			}
			throw new PackageNotFoundException();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000EEDC File Offset: 0x0000D0DC
		public override List<string> DownloadLatestPackage(DownloadParameters downloadParameters, CancellationToken cancellationToken)
		{
			List<string> result;
			try
			{
				this.progressUpdateResetTimer = new IntervalResetAccessTimer(MsrDownloadConfig.Instance.ReportingProgressIntervalMillis, true);
				this.progressUpdateResetTimer.StartTimer();
				result = this.DownloadPackage(downloadParameters, cancellationToken);
			}
			catch (DownloadPackageException)
			{
				throw;
			}
			catch (NotEnoughSpaceException)
			{
				throw;
			}
			catch (AggregateException ex)
			{
				if (ex.GetBaseException() is PackageNotFoundException)
				{
					throw ex.GetBaseException();
				}
				if (ex.GetBaseException() is DownloadPackageException)
				{
					throw ex.GetBaseException();
				}
				Exception ex2 = ex;
				while (ex2.InnerException != null)
				{
					ex2 = ex2.InnerException;
					if (ex2 is IOException && ((long)ex2.HResult == 39L || (long)ex2.HResult == 112L))
					{
						throw ex2;
					}
					if (ex2.InnerException is IOException)
					{
						throw new NotEnoughSpaceException(ex2.InnerException.Message, ex2.InnerException);
					}
				}
				throw new DownloadPackageException("Downloading variant file failed.", ex);
			}
			catch (Exception ex3)
			{
				if (ex3 is OperationCanceledException || ex3 is CannotAccessDirectoryException || ex3.InnerException is TaskCanceledException || ex3.InnerException is OperationCanceledException)
				{
					throw;
				}
				throw new DownloadPackageException("Downloading variant file failed.", ex3);
			}
			finally
			{
				base.SpeedCalculator.Stop();
				this.progressUpdateResetTimer.StopTimer();
			}
			return result;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000F188 File Offset: 0x0000D388
		private List<string> DownloadPackage(DownloadParameters downloadParameters, CancellationToken cancellationToken)
		{
			base.RaiseProgressChangedEvent(0, "CheckingAlreadyDownloadedFiles");
			Task<SoftwarePackage> task = this.CheckLatestPackageInternal(downloadParameters.DiscoveryParameters, cancellationToken);
			task.Wait(cancellationToken);
			cancellationToken.ThrowIfCancellationRequested();
			SoftwarePackage package = task.Result;
			List<SoftwareFile> list = package.Files;
			this.lastDownloadedSize = 0L;
			if (!string.IsNullOrEmpty(downloadParameters.FileExtension))
			{
				list = (from file in package.Files
				where file.FileName.ToLower().EndsWith(downloadParameters.FileExtension.ToLower())
				select file).ToList<SoftwareFile>();
			}
			base.TotalFilesSize = list.Sum((SoftwareFile f) => f.FileSize);
			List<SoftwareFile> notDownloadedFiles = this.GetNotDownloadedFiles(downloadParameters.DestinationFolder, list, package.PackageRevision, downloadParameters.FilesVersioned, cancellationToken);
			if (notDownloadedFiles.Count > 0)
			{
				long num = notDownloadedFiles.Sum((SoftwareFile file) => file.FileSize);
				FileChecker.ValidateSpaceAvailability(downloadParameters.DestinationFolder, num);
				base.RaiseProgressChangedEvent(0, "DownloadingFiles");
				base.SpeedCalculator.Start(base.TotalFilesSize, base.TotalFilesSize - num);
				Dictionary<string, long> dictionary = new Dictionary<string, long>();
				List<Task> list2 = new List<Task>();
				foreach (SoftwareFile softwareFile in notDownloadedFiles)
				{
					string path = this.GenerateSoftwareVersionFile(softwareFile.FileName, package.PackageRevision, downloadParameters.FilesVersioned);
					string downloadPath = Path.Combine(downloadParameters.DestinationFolder, path);
					Task task2 = this.DownloadAsync(package.Id, softwareFile.FileName, downloadPath, downloadParameters.DestinationFolder, dictionary, cancellationToken);
					dictionary.Add(softwareFile.FileName, 0L);
					task2.ContinueWith(new Action<Task>(this.DownloadTaskFinished), cancellationToken);
					list2.Add(task2);
				}
				Task.WaitAll(list2.ToArray(), cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
			}
			base.RaiseProgressChangedEvent(95, "VerifyingDownloadedFiles");
			this.CheckFilesCorrectness(downloadParameters.DestinationFolder, notDownloadedFiles, package.PackageRevision, downloadParameters.FilesVersioned, cancellationToken);
			base.RaiseProgressChangedEvent(100, null);
			return (from file in list
			select Path.Combine(downloadParameters.DestinationFolder, this.GenerateSoftwareVersionFile(file.FileName, package.PackageRevision, downloadParameters.FilesVersioned))).ToList<string>();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F750 File Offset: 0x0000D950
		private async Task DownloadAsync(string packageId, string fileName, string downloadPath, string destinationFolder, Dictionary<string, long> filesBeingDownloaded, CancellationToken cancellationToken)
		{
			Downloader download = new Downloader
			{
				SoftwareRepositoryAlternativeBaseUrl = (ApplicationInfo.UseTestServer() ? "https://pvprepo.azurewebsites.net" : "https://api.swrepository.com"),
				SoftwareRepositoryProxy = base.Proxy(),
				ChunkSize = (long)MsrDownloadConfig.Instance.ChunkSizeBytes,
				MaxParallelConnections = MsrDownloadConfig.Instance.MaxNumberOfParallelDownloads
			};
			using (FileStreamer streamer = new FileStreamer(downloadPath, packageId, destinationFolder, false))
			{
				try
				{
					await download.GetFileAsync(packageId, fileName, streamer, cancellationToken, new DownloadProgress<DownloadProgressInfo>(delegate(DownloadProgressInfo dpi)
					{
						this.OnProgress(dpi, filesBeingDownloaded);
					}));
				}
				catch (Exception innerException)
				{
					throw new DownloadPackageException(string.Format("Downloading file {0} failed.", fileName), innerException);
				}
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000F7D0 File Offset: 0x0000D9D0
		private void DownloadTaskFinished(Task task)
		{
			TaskStatus status = task.Status;
			if (status.Equals(TaskStatus.Faulted))
			{
				Tracer<MsrService>.WriteInformation("Downloading file failed.");
			}
			else if (status.Equals(TaskStatus.Canceled))
			{
				Tracer<MsrService>.WriteInformation("Download cancelled on the file.");
			}
			else
			{
				Tracer<MsrService>.WriteInformation("File succesfully downloaded.");
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000FB84 File Offset: 0x0000DD84
		private async void OnProgress(DownloadProgressInfo progressInfo, Dictionary<string, long> filesBeingDownloaded)
		{
			if (await this.progressUpdateResetTimer.TryAccessSectionAndSetAsync(CancellationToken.None))
			{
				try
				{
					await this.downloadProgressRaiseSemaphoreSlim.WaitAsync();
					if (filesBeingDownloaded[progressInfo.FileName] == 0L)
					{
						base.SpeedCalculator.CurrentPartlyDownloadedSize += progressInfo.BytesReceived;
					}
					else
					{
						base.SpeedCalculator.CurrentDownloadedSize = filesBeingDownloaded.Sum((KeyValuePair<string, long> c) => c.Value) - base.SpeedCalculator.CurrentPartlyDownloadedSize;
					}
					filesBeingDownloaded[progressInfo.FileName] = progressInfo.BytesReceived;
					int percentage = (int)(base.SpeedCalculator.TotalDownloadedSize * 95L / base.TotalFilesSize);
					long downloadSizeRequiredDeltaBytes = (long)MsrDownloadConfig.Instance.MinimalReportedDownloadedBytesIncrease;
					if (base.SpeedCalculator.TotalDownloadedSize - this.lastDownloadedSize >= downloadSizeRequiredDeltaBytes)
					{
						this.lastDownloadedSize = base.SpeedCalculator.TotalDownloadedSize;
						base.RaiseProgressChangedEvent(percentage, null);
					}
				}
				finally
				{
					this.downloadProgressRaiseSemaphoreSlim.Release();
				}
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		private List<SoftwareFile> GetNotDownloadedFiles(string targetFolder, IEnumerable<SoftwareFile> files, string softwareVersion, bool filesVersioned, CancellationToken cancellationToken)
		{
			List<SoftwareFile> list = (from file in files
			select new SoftwareFile
			{
				Checksum = file.Checksum,
				FileName = file.FileName,
				FileSize = file.FileSize,
				FileType = file.FileType
			}).ToList<SoftwareFile>();
			List<SoftwareFile> list2 = new List<SoftwareFile>(list);
			foreach (SoftwareFile softwareFile in list)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string path = this.GenerateSoftwareVersionFile(softwareFile.FileName, softwareVersion, filesVersioned);
				string text = Path.Combine(targetFolder, path);
				if (File.Exists(text))
				{
					SoftwareFileChecksum softwareFileChecksum = softwareFile.Checksum.First<SoftwareFileChecksum>();
					byte[] array = this.fileChecker.CheckFile(softwareFileChecksum.ChecksumType, text, cancellationToken);
					if (array != null && Convert.ToBase64String(array) == softwareFileChecksum.Value)
					{
						list2.Remove(softwareFile);
					}
				}
			}
			return list2;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000FD34 File Offset: 0x0000DF34
		private string GenerateSoftwareVersionFile(string fileName, string softwareVersion, bool appendVersion)
		{
			string result;
			if (appendVersion)
			{
				string str = fileName.Substring(0, fileName.LastIndexOf('.'));
				string str2 = fileName.Substring(fileName.LastIndexOf('.'));
				string text = str + "_" + softwareVersion + str2;
				result = text;
			}
			else
			{
				result = fileName;
			}
			return result;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
		private void IntegrityCheckProgressChanged(double progress)
		{
			Action<int> handle = this.IntegrityCheckProgressEvent;
			if (handle != null)
			{
				AppDispatcher.Execute(delegate
				{
					handle((int)progress);
				}, false);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		public void CheckFilesCorrectness(string targetFolder, IEnumerable<SoftwareFile> files, string softwareVersion, bool filesVersioned, CancellationToken cancellationToken)
		{
			foreach (SoftwareFile softwareFile in files)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string path = this.GenerateSoftwareVersionFile(softwareFile.FileName, softwareVersion, filesVersioned);
				SoftwareFileChecksum softwareFileChecksum = softwareFile.Checksum.First<SoftwareFileChecksum>();
				byte[] array = this.fileChecker.CheckFile(softwareFileChecksum.ChecksumType, Path.Combine(targetFolder, path), cancellationToken);
				if (array != null && Convert.ToBase64String(array) != softwareFileChecksum.Value)
				{
					throw new Crc32Exception(targetFolder + softwareFile.FileName);
				}
			}
		}

		// Token: 0x04000189 RID: 393
		private const string RepositoryBaseUri = "https://api.swrepository.com";

		// Token: 0x0400018A RID: 394
		private const string TestRepositoryBaseUri = "https://pvprepo.azurewebsites.net";

		// Token: 0x0400018B RID: 395
		private readonly SemaphoreSlim downloadProgressRaiseSemaphoreSlim = new SemaphoreSlim(1, 1);

		// Token: 0x0400018C RID: 396
		private long lastDownloadedSize;

		// Token: 0x0400018D RID: 397
		private readonly FileChecker fileChecker;

		// Token: 0x0400018E RID: 398
		private IntervalResetAccessTimer progressUpdateResetTimer;
	}
}
