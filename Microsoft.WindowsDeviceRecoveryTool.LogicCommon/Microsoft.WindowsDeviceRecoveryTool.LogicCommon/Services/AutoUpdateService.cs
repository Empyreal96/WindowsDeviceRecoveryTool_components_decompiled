using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Nokia.Mira;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000032 RID: 50
	[PartCreationPolicy(CreationPolicy.Shared)]
	[Export(typeof(IUseProxy))]
	[Export(typeof(AutoUpdateService))]
	public class AutoUpdateService : IUseProxy, IDisposable
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000AE26 File Offset: 0x00009026
		[ImportingConstructor]
		public AutoUpdateService()
		{
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002AD RID: 685 RVA: 0x0000AE34 File Offset: 0x00009034
		// (remove) Token: 0x060002AE RID: 686 RVA: 0x0000AE70 File Offset: 0x00009070
		public event Action<DownloadingProgressChangedEventArgs> DownloadProgressChanged;

		// Token: 0x060002AF RID: 687 RVA: 0x0000AEAC File Offset: 0x000090AC
		public ApplicationUpdate ReadLatestAppVersion(int appId, string currentVersion, bool useTestServer)
		{
			try
			{
				Tracer<AutoUpdateService>.LogEntry("ReadLatestAppVersion");
				Tracer<AutoUpdateService>.WriteInformation("Checking - appId: {0}, current version: {1}", new object[]
				{
					appId,
					currentVersion
				});
				ApplicationUpdate appVersion = this.GetAppVersion(appId, useTestServer);
				if (appVersion != null)
				{
					Tracer<AutoUpdateService>.WriteInformation("Latest package version found: {0}", new object[]
					{
						appVersion.Version
					});
					int num = VersionComparer.CompareVersions(appVersion.Version, currentVersion);
					if (num > 0)
					{
						Tracer<AutoUpdateService>.WriteInformation("Package on server is newer than installed!");
						this.CheckPackageSize(appVersion);
						return appVersion;
					}
					if (num == 0)
					{
						Tracer<AutoUpdateService>.WriteInformation("Package on server is same as installed.");
					}
					else
					{
						Tracer<AutoUpdateService>.WriteError("Package on server is older than installed!", new object[0]);
					}
				}
				else
				{
					Tracer<AutoUpdateService>.WriteInformation("Update package not found.");
				}
			}
			finally
			{
				Tracer<AutoUpdateService>.LogExit("ReadLatestAppVersion");
			}
			return null;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000AFB4 File Offset: 0x000091B4
		public string DownloadAppPacket(ApplicationUpdate packageToDownload, string downloadPath, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			if (packageToDownload == null || string.IsNullOrWhiteSpace(packageToDownload.PackageUri))
			{
				throw new InvalidOperationException("App update package is incorrect. It doesn't contain any file.");
			}
			this.Download(packageToDownload, downloadPath, token);
			return Path.Combine(downloadPath, packageToDownload.PackageFileName);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000B00C File Offset: 0x0000920C
		private void Download(ApplicationUpdate packageToDownload, string path, CancellationToken token)
		{
			this.lastProgressPercentage = 0;
			this.lastProgressMessage = "DownloadingFiles";
			this.speedCalculator = new SpeedCalculator();
			this.speedCalculator.Start(packageToDownload.Size, 0L);
			Action<DownloadProgressInfo> action = new Action<DownloadProgressInfo>(this.DownloadTaskProgress);
			Nokia.Mira.Progress<DownloadProgressInfo> progress = new Nokia.Mira.Progress<DownloadProgressInfo>(action);
			DownloadSettings downloadSettings = new DownloadSettings(5, 3145728L, true, true);
			Uri uri = new Uri(packageToDownload.PackageUri);
			HttpWebRequestFactory httpWebRequestFactory = new HttpWebRequestFactory(uri)
			{
				Proxy = this.Proxy()
			};
			WebFile webFile = new WebFile(httpWebRequestFactory);
			Task task = webFile.DownloadAsync(path + packageToDownload.PackageFileName, token, progress, downloadSettings);
			task.ContinueWith(new Action<Task>(this.DownloadTaskFinished), token);
			Task.WaitAll(new Task[]
			{
				task
			});
			token.ThrowIfCancellationRequested();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000B0E8 File Offset: 0x000092E8
		private void DownloadTaskProgress(DownloadProgressInfo info)
		{
			this.speedCalculator.CurrentDownloadedSize += info.BytesReceived;
			this.lastProgressPercentage = (int)(this.speedCalculator.TotalDownloadedSize * 100L / this.speedCalculator.TotalFilesSize);
			this.RaiseDownloadProgressChangedEvent();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B138 File Offset: 0x00009338
		private void RaiseDownloadProgressChangedEvent()
		{
			Action<DownloadingProgressChangedEventArgs> downloadProgressChanged = this.DownloadProgressChanged;
			if (downloadProgressChanged != null)
			{
				downloadProgressChanged(new DownloadingProgressChangedEventArgs(this.lastProgressPercentage, this.speedCalculator.TotalDownloadedSize, this.speedCalculator.TotalFilesSize, this.speedCalculator.BytesPerSecond, this.speedCalculator.RemaingSeconds, this.lastProgressMessage));
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B19C File Offset: 0x0000939C
		private void DownloadTaskFinished(Task task)
		{
			TaskStatus status = task.Status;
			if (status.Equals(TaskStatus.Faulted))
			{
				Tracer<AutoUpdateService>.WriteInformation("Downloading App update failed.");
			}
			else if (status.Equals(TaskStatus.Canceled))
			{
				Tracer<AutoUpdateService>.WriteInformation("Download cancelled on the App update.");
			}
			else
			{
				Tracer<AutoUpdateService>.WriteInformation("App update succesfully downloaded.");
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B208 File Offset: 0x00009408
		private void CheckPackageSize(ApplicationUpdate package)
		{
			WebRequest webRequest = WebRequest.Create(package.PackageUri);
			webRequest.Method = "HEAD";
			webRequest.Proxy = this.Proxy();
			using (WebResponse response = webRequest.GetResponse())
			{
				long size;
				if (long.TryParse(response.Headers.Get("Content-Length"), out size))
				{
					package.Size = size;
				}
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000B290 File Offset: 0x00009490
		private ApplicationUpdate GetAppVersion(int appId, bool useTestServer)
		{
			Tracer<AutoUpdateService>.LogEntry("GetAppVersion");
			string text = useTestServer ? "http://147.243.21.64/PackageSource/WDRT/AppUpdate/" : "https://repairavoidance.blob.core.windows.net/packages/WDRT/AppUpdate/";
			text = Path.Combine(text, "WDRT_Update.xml");
			try
			{
				Uri address = new Uri(text);
				Stream input = this.Download(address);
				XmlReader reader = XmlReader.Create(input);
				XmlDocument xmlDocument = new XmlDocument
				{
					XmlResolver = null
				};
				xmlDocument.Load(reader);
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Package");
				foreach (object obj in elementsByTagName)
				{
					XmlNode node = (XmlNode)obj;
					ApplicationUpdate autoUpdateNode = this.GetAutoUpdateNode(node);
					if (autoUpdateNode.AppId == appId)
					{
						return autoUpdateNode;
					}
				}
			}
			catch (Exception error)
			{
				Tracer<AutoUpdateService>.WriteError(error, "Reading app update failed", new object[0]);
				throw;
			}
			Tracer<AutoUpdateService>.LogExit("GetAppVersion");
			return null;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000B3C0 File Offset: 0x000095C0
		private ApplicationUpdate GetAutoUpdateNode(XmlNode node)
		{
			ApplicationUpdate applicationUpdate = new ApplicationUpdate();
			foreach (object obj in node.ChildNodes)
			{
				XmlElement xmlElement = (XmlElement)obj;
				if (xmlElement.Name == "Description")
				{
					applicationUpdate.Description = xmlElement.InnerText;
				}
				else if (xmlElement.Name == "PackagePath")
				{
					applicationUpdate.PackageUri = xmlElement.InnerText;
				}
				else if (xmlElement.Name == "AppVersion")
				{
					applicationUpdate.Version = xmlElement.InnerText;
				}
				else if (xmlElement.Name == "AppId")
				{
					applicationUpdate.AppId = int.Parse(xmlElement.InnerText);
				}
			}
			Tracer<AutoUpdateService>.WriteInformation("Found app update node: appId: {0} | appVersion {1}", new object[]
			{
				applicationUpdate.AppId,
				applicationUpdate.Version
			});
			return applicationUpdate;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000B514 File Offset: 0x00009714
		public void SetProxy(IWebProxy settings)
		{
			this.proxySettings = settings;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000B520 File Offset: 0x00009720
		private IWebProxy Proxy()
		{
			return this.proxySettings ?? WebRequest.GetSystemWebProxy();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000B544 File Offset: 0x00009744
		private Stream Download(Uri address)
		{
			Stream result;
			using (WebClientEx webClientEx = new WebClientEx(30000)
			{
				Proxy = this.Proxy()
			})
			{
				try
				{
					result = webClientEx.OpenRead(address);
				}
				catch (Exception error)
				{
					Tracer<AutoUpdateService>.WriteError(error);
					throw;
				}
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B5B4 File Offset: 0x000097B4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B5C8 File Offset: 0x000097C8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x04000159 RID: 345
		private const int DefaultChunkSize = 3145728;

		// Token: 0x0400015A RID: 346
		private const int DefaultMaxChunks = 5;

		// Token: 0x0400015B RID: 347
		private const string QaServerAddress = "http://147.243.21.64/PackageSource/WDRT/AppUpdate/";

		// Token: 0x0400015C RID: 348
		private const string ProductionServerAddress = "https://repairavoidance.blob.core.windows.net/packages/WDRT/AppUpdate/";

		// Token: 0x0400015D RID: 349
		private const string AutoUpdateXmlFileName = "WDRT_Update.xml";

		// Token: 0x0400015E RID: 350
		private bool disposed;

		// Token: 0x0400015F RID: 351
		private IWebProxy proxySettings;

		// Token: 0x04000160 RID: 352
		private string lastProgressMessage;

		// Token: 0x04000161 RID: 353
		private int lastProgressPercentage;

		// Token: 0x04000162 RID: 354
		private SpeedCalculator speedCalculator;
	}
}
