using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services.DataPackageRules;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Nokia.Mira;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[Export(typeof(DataPackageService))]
	[Export(typeof(IUseProxy))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class DataPackageService : IUseProxy, IDisposable
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000031A1 File Offset: 0x000013A1
		[ImportingConstructor]
		public DataPackageService(FileChecker fileChecker)
		{
			this.fileChecker = fileChecker;
			this.speedCalculator = new SpeedCalculator();
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000023 RID: 35 RVA: 0x000031C0 File Offset: 0x000013C0
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x000031FC File Offset: 0x000013FC
		public event Action<int> IntegrityCheckProgressEvent;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00003238 File Offset: 0x00001438
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x00003274 File Offset: 0x00001474
		public event Action<DownloadingProgressChangedEventArgs> DownloadProgressChanged;

		// Token: 0x06000027 RID: 39 RVA: 0x000032B0 File Offset: 0x000014B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000032E0 File Offset: 0x000014E0
		public void IntegrityCheckProgressChanged(double progress)
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

		// Token: 0x06000029 RID: 41 RVA: 0x00003333 File Offset: 0x00001533
		public void IntegrityCheckDuringDownloadProgressChanged(double progress)
		{
			Tracer<DataPackageService>.WriteInformation(string.Format("Current integrity check progress is {0} ", progress));
			this.lastProgressPercentage = 95 + (int)(progress / 20.0);
			this.RaiseDownloadProgressChangedEvent();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003368 File Offset: 0x00001568
		public List<VariantInfo> FindLocalVariants()
		{
			List<VariantInfo> list = new List<VariantInfo>();
			this.FindLocalVariants(list, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultProductsPath);
			this.FindLocalVariants(list, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.NokiaProductsPath);
			return list;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000339C File Offset: 0x0000159C
		public void FindLocalVariants(List<VariantInfo> allVariants, string localPath)
		{
			Tracer<DataPackageService>.LogEntry("FindLocalVariants");
			if (!Directory.Exists(localPath))
			{
				Tracer<DataPackageService>.WriteInformation("Directory doesn't exist: {0}", new object[]
				{
					localPath
				});
			}
			else
			{
				string[] directories = Directory.GetDirectories(localPath, "rm-*", SearchOption.AllDirectories);
				foreach (string path in directories)
				{
					string[] files = Directory.GetFiles(Path.Combine(localPath, path), "*.vpl");
					allVariants.AddRange(files.Select(new Func<string, VariantInfo>(VariantInfo.GetVariantInfo)));
				}
				Tracer<DataPackageService>.LogExit("FindLocalVariants");
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003488 File Offset: 0x00001688
		public void CheckVariantIntegrity(string vplPath, CancellationToken cancellationToken)
		{
			Tracer<DataPackageService>.LogEntry("CheckVariantIntegrity");
			Tracer<DataPackageService>.WriteInformation("Checking: {0}", new object[]
			{
				vplPath
			});
			this.fileChecker.SetProgressHandler(new Action<double>(this.IntegrityCheckProgressChanged));
			VplContent vplContent = new VplContent();
			vplContent.ParseVplFile(vplPath);
			string directoryName = Path.GetDirectoryName(vplPath);
			if (string.IsNullOrEmpty(directoryName))
			{
				throw new DirectoryNotFoundException("Vpl directory not found");
			}
			List<string> list = (from file in vplContent.FileList
			where !string.IsNullOrEmpty(file.Name) && !file.Optional
			select file.Name).ToList<string>();
			foreach (string path in list)
			{
				if (!File.Exists(Path.Combine(directoryName, path)))
				{
					throw new FirmwareFileNotFoundException();
				}
			}
			List<FileCrcInfo> list2 = new List<FileCrcInfo>();
			foreach (VplFile vplFile in vplContent.FileList)
			{
				if (!string.IsNullOrEmpty(vplFile.Name) && !string.IsNullOrEmpty(vplFile.Crc) && list.Contains(vplFile.Name, StringComparer.OrdinalIgnoreCase))
				{
					list2.Add(new FileCrcInfo
					{
						FileName = vplFile.Name,
						Directory = directoryName,
						Crc = vplFile.Crc,
						Optional = vplFile.Optional
					});
				}
			}
			this.fileChecker.CheckFiles(list2, cancellationToken);
			Tracer<DataPackageService>.LogExit("CheckVariantIntegrity");
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000036B0 File Offset: 0x000018B0
		public ReadOnlyCollection<string> FindLocalVariantPaths(string productType, string productCode, string searchPath, CancellationToken token)
		{
			ReadOnlyCollection<string> result;
			if (!Directory.Exists(searchPath))
			{
				Tracer<DataPackageService>.WriteInformation("Directory doesn't exist: {0}", new object[]
				{
					searchPath
				});
				result = new ReadOnlyCollection<string>(new List<string>());
			}
			else
			{
				result = this.fileChecker.FindLocalVplFilePaths(productType, productCode, searchPath);
			}
			return result;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003700 File Offset: 0x00001900
		public Tuple<long, long, bool> GetDownloadPackageInformation()
		{
			return new Tuple<long, long, bool>(this.speedCalculator.CurrentDownloadedSize, this.totalFilesSize, this.speedCalculator.IsResumed);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003734 File Offset: 0x00001934
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000375C File Offset: 0x0000195C
		private void Download(List<File4> filesToDownload, string path, CancellationToken token, bool verifyChecksum = true)
		{
			this.lastProgressMessage = "DownloadingFiles";
			Action<DownloadProgressInfo> action = new Action<DownloadProgressInfo>(this.DownloadTaskProgress);
			Nokia.Mira.Progress<DownloadProgressInfo> progress = new Nokia.Mira.Progress<DownloadProgressInfo>(action);
			List<Task> list = new List<Task>();
			DownloadSettings downloadSettings = new DownloadSettings(5, 3145728L, true, true);
			foreach (File4 file in filesToDownload)
			{
				Uri uri = new Uri(file.DownloadUrl);
				HttpWebRequestFactory httpWebRequestFactory = new HttpWebRequestFactory(uri)
				{
					Proxy = this.Proxy()
				};
				WebFile webFile = new WebFile(httpWebRequestFactory);
				Task task = webFile.DownloadAsync(Path.Combine(path, file.FileName), token, progress, downloadSettings);
				task.ContinueWith(new Action<Task>(this.DownloadTaskFinished), token);
				list.Add(task);
			}
			try
			{
				Task.WaitAll(list.ToArray());
			}
			catch (Exception ex)
			{
				if (!(ex.InnerException is AggregateException) || !(ex.InnerException.InnerException is IOException) || (long)ex.InnerException.InnerException.HResult != -2147024784L)
				{
					throw;
				}
				string pathRoot = Path.GetPathRoot(path);
				if (pathRoot == null)
				{
					throw;
				}
				long availableFreeSpace = new DriveInfo(pathRoot).AvailableFreeSpace;
				throw new NotEnoughSpaceException
				{
					Available = availableFreeSpace,
					Needed = this.speedCalculator.TotalFilesSize - this.speedCalculator.TotalDownloadedSize,
					Disk = pathRoot
				};
			}
			token.ThrowIfCancellationRequested();
			if (verifyChecksum)
			{
				this.lastProgressPercentage = 95;
				this.lastProgressMessage = "VerifyingDownloadedFiles";
				this.RaiseDownloadProgressChangedEvent();
				this.fileChecker.SetProgressHandler(new Action<double>(this.IntegrityCheckDuringDownloadProgressChanged));
				this.fileChecker.CheckFilesCorrectness(path, filesToDownload, token);
			}
			this.lastProgressPercentage = 100;
			this.lastProgressMessage = string.Empty;
			this.RaiseDownloadProgressChangedEvent();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003994 File Offset: 0x00001B94
		private void DownloadTaskFinished(Task task)
		{
			TaskStatus status = task.Status;
			if (status.Equals(TaskStatus.Faulted))
			{
				Tracer<DataPackageService>.WriteError("Downloading variant file failed.", new object[]
				{
					task.Exception
				});
			}
			else if (status.Equals(TaskStatus.Canceled))
			{
				Tracer<DataPackageService>.WriteInformation("Download cancelled on the variant file.");
			}
			else
			{
				Tracer<DataPackageService>.WriteInformation("Variant file successfully downloaded.");
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003A14 File Offset: 0x00001C14
		private void DownloadTaskProgress(DownloadProgressInfo info)
		{
			if (!this.speedCalculator.IsDownloadStarted)
			{
				this.speedCalculator.PreviousDownloadedSize += info.BytesReceived;
				this.speedCalculator.IsDownloadStarted = true;
			}
			else
			{
				this.speedCalculator.CurrentDownloadedSize += info.BytesReceived;
			}
			if (this.totalFilesSize > 0L)
			{
				this.lastProgressPercentage = (int)(this.speedCalculator.TotalDownloadedSize * 95L / this.totalFilesSize);
			}
			this.RaiseDownloadProgressChangedEvent();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003AB0 File Offset: 0x00001CB0
		private void RaiseDownloadProgressChangedEvent()
		{
			Action<DownloadingProgressChangedEventArgs> downloadProgressChanged = this.DownloadProgressChanged;
			if (downloadProgressChanged != null)
			{
				downloadProgressChanged(new DownloadingProgressChangedEventArgs(this.lastProgressPercentage, this.speedCalculator.TotalDownloadedSize, this.speedCalculator.TotalFilesSize, this.speedCalculator.BytesPerSecond, this.speedCalculator.RemaingSeconds, this.lastProgressMessage));
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003B14 File Offset: 0x00001D14
		public EmergencyPackageInfo DownloadEmergencyPackage(string typeDesignator, string destinationPath, CancellationToken cancellationToken)
		{
			EmergencyPackageInfo result;
			try
			{
				result = this.TryDownloadEmergencyPackage(typeDesignator, destinationPath, cancellationToken);
			}
			catch (AggregateException ex)
			{
				Exception ex2 = ex;
				while (ex2.InnerException != null)
				{
					ex2 = ex2.InnerException;
					if ((ex2 is IOException || ex2.InnerException is IOException) && ((long)ex2.HResult == 39L || (long)ex2.HResult == 112L))
					{
						throw ex2;
					}
					if (ex2 is AggregateException && ex2.InnerException is IOException && ((long)ex2.InnerException.HResult == -2146233088L || (long)ex2.InnerException.HResult == -2146232800L))
					{
						throw new WebException(ex2.GetBaseException().Message);
					}
				}
				throw new DownloadPackageException(ex2.GetBaseException().Message, ex);
			}
			catch (Exception ex3)
			{
				if (ex3 is OperationCanceledException || ex3 is NotEnoughSpaceException || ex3 is PlannedServiceBreakException || ex3 is CannotAccessDirectoryException || ex3.InnerException is TaskCanceledException || ex3.InnerException is OperationCanceledException)
				{
					throw;
				}
				throw new DownloadPackageException(ex3.GetBaseException().Message, ex3);
			}
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003CEC File Offset: 0x00001EEC
		private EmergencyPackageInfo TryDownloadEmergencyPackage(string typeDesignator, string destinationPath, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string downloadUrl = string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, "emergency_flash_config.xml");
			File4 item = new File4("emergency_flash_config.xml", string.Empty, this.QueryFileLength(downloadUrl), downloadUrl, string.Empty, 0);
			List<File4> list = new List<File4>
			{
				item
			};
			this.totalFilesSize = 0L;
			this.speedCalculator.Reset();
			FileChecker.ValidateSpaceAvailability(destinationPath, list.Sum((File4 f) => f.FileSize));
			this.Download(list, destinationPath, cancellationToken, false);
			EmergencyPackageInfo result = new EmergencyPackageInfo
			{
				ConfigFilePath = Path.Combine(destinationPath, "emergency_flash_config.xml")
			};
			list = this.GetEmergencyFilesList(Path.Combine(destinationPath, "emergency_flash_config.xml"), typeDesignator, destinationPath, ref result);
			this.totalFilesSize = list.Sum((File4 f) => f.FileSize);
			FileChecker.ValidateSpaceAvailability(destinationPath, list.Sum((File4 f) => f.FileSize));
			try
			{
				this.speedCalculator.Start(this.totalFilesSize, this.totalFilesSize - list.Sum((File4 f) => f.FileSize));
				this.Download(list, destinationPath, cancellationToken, false);
			}
			finally
			{
				this.speedCalculator.Stop();
			}
			cancellationToken.ThrowIfCancellationRequested();
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000040D0 File Offset: 0x000022D0
		private List<File4> GetEmergencyFilesList(string emergencyConfigFilePath, string typeDesignator, string destinationPath, ref EmergencyPackageInfo emergencyPackageInfo)
		{
			List<File4> list = new List<File4>();
			using (FileStream fileStream = new FileStream(emergencyConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				XElement xelement = XElement.Load(fileStream);
				List<File4> list2 = (from file in xelement.Descendants("first_boot_image")
				select new File4(file.Attribute("image_path").Value, string.Empty, this.QueryFileLength(string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value)), string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value), string.Empty, 0)).ToList<File4>();
				list.AddRange(list2);
				if (list2.Any<File4>())
				{
					emergencyPackageInfo.HexFlasherFilePath = Path.Combine(destinationPath, list2.First<File4>().FileName);
				}
				List<File4> list3 = (from file in xelement.Descendants("firehose_image")
				select new File4(file.Attribute("image_path").Value, string.Empty, this.QueryFileLength(string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value)), string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value), string.Empty, 0)).ToList<File4>();
				list.AddRange(list3);
				if (list3.Any<File4>())
				{
					emergencyPackageInfo.EdpImageFilePath = Path.Combine(destinationPath, list3.First<File4>().FileName);
				}
				List<File4> list4 = (from file in xelement.Descendants("hex_flasher")
				select new File4(file.Attribute("image_path").Value, string.Empty, this.QueryFileLength(string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value)), string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value), string.Empty, 0)).ToList<File4>();
				list.AddRange(list4);
				if (list4.Any<File4>())
				{
					emergencyPackageInfo.HexFlasherFilePath = Path.Combine(destinationPath, list4.First<File4>().FileName);
				}
				List<File4> list5 = (from file in xelement.Descendants("mbn_image")
				select new File4(file.Attribute("image_path").Value, string.Empty, this.QueryFileLength(string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value)), string.Format("https://repairavoidance.blob.core.windows.net/packages/EmergencyFlash/{0}/{1}", typeDesignator, file.Attribute("image_path").Value), string.Empty, 0)).ToList<File4>();
				list.AddRange(list5);
				if (list5.Any<File4>())
				{
					emergencyPackageInfo.MbnImageFilePath = Path.Combine(destinationPath, list5.First<File4>().FileName);
				}
			}
			return list;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000042F4 File Offset: 0x000024F4
		public void SetProxy(IWebProxy settings)
		{
			this.proxySettings = settings;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004300 File Offset: 0x00002500
		private IWebProxy Proxy()
		{
			return this.proxySettings ?? WebRequest.GetSystemWebProxy();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004324 File Offset: 0x00002524
		private long QueryFileLength(string downloadUrl)
		{
			WebRequest webRequest = WebRequest.Create(downloadUrl);
			webRequest.Proxy = this.Proxy();
			webRequest.Method = "HEAD";
			using (WebResponse response = webRequest.GetResponse())
			{
				long result;
				if (long.TryParse(response.Headers.Get("Content-Length"), out result))
				{
					return result;
				}
			}
			return 0L;
		}

		// Token: 0x0400000F RID: 15
		private const int DefaultChunkSize = 3145728;

		// Token: 0x04000010 RID: 16
		private const int DefaultMaxChunks = 5;

		// Token: 0x04000011 RID: 17
		private const long ErrorHandleDiskFull = 39L;

		// Token: 0x04000012 RID: 18
		private const long ErrorEmptyDiskSpace = -2147024784L;

		// Token: 0x04000013 RID: 19
		private const long ErrorInternetConnection = -2146233088L;

		// Token: 0x04000014 RID: 20
		private const long ErrorServerConnection = -2146232800L;

		// Token: 0x04000015 RID: 21
		private const long ErrorDiskFull = 112L;

		// Token: 0x04000016 RID: 22
		private readonly FileChecker fileChecker;

		// Token: 0x04000017 RID: 23
		private readonly SpeedCalculator speedCalculator;

		// Token: 0x04000018 RID: 24
		private bool disposed;

		// Token: 0x04000019 RID: 25
		private int lastProgressPercentage;

		// Token: 0x0400001A RID: 26
		private string lastProgressMessage;

		// Token: 0x0400001B RID: 27
		private long totalFilesSize;

		// Token: 0x0400001C RID: 28
		private IWebProxy proxySettings;
	}
}
