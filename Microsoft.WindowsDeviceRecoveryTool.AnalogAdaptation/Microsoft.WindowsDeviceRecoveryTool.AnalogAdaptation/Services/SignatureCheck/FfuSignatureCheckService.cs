using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsPhone.Imaging;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services.SignatureCheck
{
	// Token: 0x02000008 RID: 8
	internal sealed class FfuSignatureCheckService
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00004708 File Offset: 0x00002908
		private FfuSignatureCheckService(FfuFileInfoService ffuFileInfo)
		{
			if (ffuFileInfo == null)
			{
				throw new ArgumentNullException("ffuFileInfo");
			}
			this.ffuFileInfo = ffuFileInfo;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000473C File Offset: 0x0000293C
		public static void RunSignatureCheck(FfuFileInfoService ffuFileInfoService, CancellationToken cancellationToken)
		{
			Tracer<FfuSignatureCheckService>.LogEntry("RunSignatureCheck");
			FfuSignatureCheckService ffuSignatureCheckService = new FfuSignatureCheckService(ffuFileInfoService);
			try
			{
				ffuSignatureCheckService.ValidateComponents(cancellationToken);
				ffuSignatureCheckService.RunGetCatalog(cancellationToken);
				ffuSignatureCheckService.ValidateExtractedCatalog(cancellationToken);
				ffuSignatureCheckService.ValidateExtractedCatalogCorrespondsToTheImage(cancellationToken);
			}
			finally
			{
				ffuSignatureCheckService.Cleanup();
				Tracer<FfuSignatureCheckService>.LogExit("RunSignatureCheck");
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000047A8 File Offset: 0x000029A8
		private void Cleanup()
		{
			if (File.Exists(this.extractedCatalogFilePath))
			{
				File.Delete(this.extractedCatalogFilePath);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000047D8 File Offset: 0x000029D8
		private void ValidateExtractedCatalogCorrespondsToTheImage(CancellationToken cancellationToken)
		{
			try
			{
				FullFlashUpdateImage fullFlashUpdateImage = new FullFlashUpdateImage();
				fullFlashUpdateImage.Initialize(this.ffuFileInfo.FullName);
				ImageSigner imageSigner = new ImageSigner();
				imageSigner.Initialize(fullFlashUpdateImage, this.extractedCatalogFilePath, null);
				imageSigner.VerifyCatalog();
			}
			catch (Exception ex)
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Verification failed: {0}", new object[]
				{
					ex
				});
				throw new FfuSignatureCheckException("FFU Signature check fail - Verification of extracted catalog against image failed", ex);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004878 File Offset: 0x00002A78
		private void SignFfuFileWithCatalog(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string text = string.Format("SIGN \"{0}\" \"{1}\"", this.ffuFileInfo.FullName, this.extractedCatalogFilePath);
			ProcessHelper imageSignerProcess = new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = new ProcessStartInfo(this.imageSignerPath, text)
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					WorkingDirectory = this.workingDirectory
				}
			};
			try
			{
				imageSignerProcess.ErrorDataReceived += this.ImageSignerProcessOnErrorDataReceived;
				imageSignerProcess.OutputDataReceived += this.ImageSignerProcessOnOutputDataReceived;
				Task task = new Task(delegate()
				{
					this.CancellationMonitor(cancellationToken, imageSignerProcess);
				});
				task.Start();
				imageSignerProcess.Start();
				Tracer<FfuSignatureCheckService>.WriteVerbose("Running process ID={0}: {1} {2}", new object[]
				{
					imageSignerProcess.Id,
					"imagesigner.exe",
					text
				});
				imageSignerProcess.BeginOutputReadLine();
				imageSignerProcess.WaitForExit();
			}
			catch (Exception innerException)
			{
				throw new FfuSignatureCheckException("FFU Signature fail - Signing ffu with catalog failed", innerException);
			}
			finally
			{
				imageSignerProcess.ErrorDataReceived -= this.ImageSignerProcessOnErrorDataReceived;
				imageSignerProcess.OutputDataReceived -= this.ImageSignerProcessOnOutputDataReceived;
			}
			if (imageSignerProcess.ExitCode != 0)
			{
				throw new FfuSignatureCheckException(string.Format("FFU Signature check fail - Signing ffu with catalog failed with exit code: {0}", imageSignerProcess.ExitCode), imageSignerProcess.ExitCode);
			}
			Tracer<FfuSignatureCheckService>.WriteInformation("Signature check - signing ffu file success");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004AB8 File Offset: 0x00002CB8
		private void ValidateExtractedCatalog(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (!File.Exists(this.extractedCatalogFilePath))
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - FFU extracted cat not present: {0}", new object[]
				{
					this.extractedCatalogFilePath
				});
				throw new FileNotFoundException("FFU Signature check fail - FFU image catalog extracted but not present on given location", this.extractedCatalogFilePath);
			}
			string text = string.Format("VERIFY /r \"{0}\" /ca {1} /u {2} \"{3}\"", new object[]
			{
				"Microsoft Root Certificate Authority 2010",
				"C01386A907496404F276C3C1853ABF4A5274AF88",
				"1.3.6.1.4.1.311.10.3.6",
				this.extractedCatalogFilePath
			});
			ProcessHelper signToolProcess = new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = new ProcessStartInfo(this.signToolPath, text)
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					WorkingDirectory = this.workingDirectory
				}
			};
			try
			{
				signToolProcess.OutputDataReceived += this.SignToolProcessOnOutputDataReceived;
				signToolProcess.ErrorDataReceived += this.SignToolProcessOnErrorDataReceived;
				Task task = new Task(delegate()
				{
					this.CancellationMonitor(cancellationToken, signToolProcess);
				});
				task.Start();
				signToolProcess.Start();
				Tracer<FfuSignatureCheckService>.WriteVerbose("Running process ID={0}: {1} {2}", new object[]
				{
					signToolProcess.Id,
					"signtool.exe",
					text
				});
				signToolProcess.BeginOutputReadLine();
				signToolProcess.WaitForExit();
			}
			catch (Exception ex)
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Verification failed: {0}", new object[]
				{
					ex
				});
				throw new FfuSignatureCheckException("FFU Signature check fail - Verification of extracted catalog failed", ex);
			}
			finally
			{
				signToolProcess.OutputDataReceived -= this.SignToolProcessOnOutputDataReceived;
				signToolProcess.ErrorDataReceived -= this.SignToolProcessOnErrorDataReceived;
			}
			if (signToolProcess.ExitCode != 0)
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Verification failed", new object[0]);
				throw new FfuSignatureCheckException(string.Format("FFU Signature check fail - Verification of extracted catalog failed exit code: {0}", signToolProcess.ExitCode), signToolProcess.ExitCode);
			}
			Tracer<FfuSignatureCheckService>.WriteInformation("Signature check - extracted cat file verification success");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004D84 File Offset: 0x00002F84
		private void RunGetCatalog(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string path = string.Format("{0}.cat", Path.GetFileNameWithoutExtension(this.ffuFileInfo.Name));
			this.extractedCatalogFilePath = Path.Combine(Path.GetTempPath(), path);
			string text = string.Format("GETCATALOG \"{0}\" \"{1}\"", this.ffuFileInfo.FullName, this.extractedCatalogFilePath);
			ProcessHelper imageSignerProcess = new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = new ProcessStartInfo(this.imageSignerPath, text)
				{
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					WorkingDirectory = this.workingDirectory
				}
			};
			try
			{
				imageSignerProcess.ErrorDataReceived += this.ImageSignerProcessOnErrorDataReceived;
				imageSignerProcess.OutputDataReceived += this.ImageSignerProcessOnOutputDataReceived;
				Task task = new Task(delegate()
				{
					this.CancellationMonitor(cancellationToken, imageSignerProcess);
				});
				task.Start();
				imageSignerProcess.Start();
				Tracer<FfuSignatureCheckService>.WriteVerbose("Running process ID={0}: {1} {2}", new object[]
				{
					imageSignerProcess.Id,
					"imagesigner.exe",
					text
				});
				imageSignerProcess.BeginOutputReadLine();
				imageSignerProcess.WaitForExit();
			}
			catch (Exception ex)
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Extracting catalog failed: {0}", new object[]
				{
					ex
				});
				throw new FfuSignatureCheckException("FFU Signature check fail - Extracting ffu image catalog failed", ex);
			}
			finally
			{
				imageSignerProcess.ErrorDataReceived -= this.ImageSignerProcessOnErrorDataReceived;
				imageSignerProcess.OutputDataReceived -= this.ImageSignerProcessOnOutputDataReceived;
			}
			if (imageSignerProcess.ExitCode != 0)
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Extracting catalog failed with exit code: {0}", new object[]
				{
					imageSignerProcess.ExitCode
				});
				throw new FfuSignatureCheckException(string.Format("FFU Signature check fail - Extracting ffu image catalog failed with exit code: {0}", imageSignerProcess.ExitCode), imageSignerProcess.ExitCode);
			}
			Tracer<FfuSignatureCheckService>.WriteInformation("Signature check - Extracting catalog success, file path = {0}", new object[]
			{
				this.extractedCatalogFilePath
			});
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005028 File Offset: 0x00003228
		private void ValidateComponents(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (!File.Exists(this.ffuFileInfo.FullName))
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Ffu file not found: {0}", new object[]
				{
					this.ffuFileInfo.FullName
				});
				throw new FileNotFoundException("FFU file was not found on given location", this.ffuFileInfo.FullName);
			}
			string workingDirectoryPath = this.GetWorkingDirectoryPath();
			this.signToolPath = Path.Combine(workingDirectoryPath, "signtool.exe");
			this.imageSignerPath = Path.Combine(workingDirectoryPath, "imagesigner.exe");
			if (!File.Exists(this.signToolPath))
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Signing tool not found: {0}", new object[]
				{
					this.signToolPath
				});
				throw new FileNotFoundException("Signing tool was not found on given location", this.signToolPath);
			}
			if (!File.Exists(this.imageSignerPath))
			{
				Tracer<FfuSignatureCheckService>.WriteError("Signature check - Image signer not found: {0}", new object[]
				{
					this.imageSignerPath
				});
				throw new FileNotFoundException("Image signer was not found on given location", this.imageSignerPath);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005130 File Offset: 0x00003330
		private string GetWorkingDirectoryPath()
		{
			string result;
			if (!string.IsNullOrEmpty(this.workingDirectory))
			{
				result = this.workingDirectory;
			}
			else
			{
				string directoryName = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
				if (string.IsNullOrWhiteSpace(directoryName))
				{
					Tracer<FfuSignatureCheckService>.WriteError("Signature check - Could not read working directory", new object[0]);
					throw new Exception("Could not find working directory path");
				}
				Tracer<FfuSignatureCheckService>.WriteVerbose("Working directory set to: {0}", new object[]
				{
					directoryName
				});
				result = (this.workingDirectory = directoryName);
			}
			return result;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000051C8 File Offset: 0x000033C8
		private void CancellationMonitor(CancellationToken token, ProcessHelper helper)
		{
			while (!helper.HasExited)
			{
				Thread.Sleep(500);
				if (token.IsCancellationRequested)
				{
					if (!helper.HasExited)
					{
						Tracer<FfuSignatureCheckService>.WriteInformation("Cancellation requested. Process still running. Need to manually kill process.");
						helper.Kill();
					}
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005220 File Offset: 0x00003420
		private void SignToolProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			Tracer<FfuSignatureCheckService>.WriteError("{0}: {1}", new object[]
			{
				"signtool.exe",
				dataReceivedEventArgs.Data
			});
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005254 File Offset: 0x00003454
		private void SignToolProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			Tracer<FfuSignatureCheckService>.WriteVerbose("{0}: {1}", new object[]
			{
				"signtool.exe",
				dataReceivedEventArgs.Data
			});
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005288 File Offset: 0x00003488
		private void ImageSignerProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			Tracer<FfuSignatureCheckService>.WriteVerbose("{0}: {1}", new object[]
			{
				"imagesigner.exe",
				dataReceivedEventArgs.Data
			});
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000052BC File Offset: 0x000034BC
		private void ImageSignerProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			Tracer<FfuSignatureCheckService>.WriteVerbose("{0}: {1}", new object[]
			{
				"imagesigner.exe",
				dataReceivedEventArgs.Data
			});
		}

		// Token: 0x0400001C RID: 28
		private const string SignToolFileName = "signtool.exe";

		// Token: 0x0400001D RID: 29
		private const string ImageSignerFileName = "imagesigner.exe";

		// Token: 0x0400001E RID: 30
		private const string CertificateAuthorityName = "Microsoft Root Certificate Authority 2010";

		// Token: 0x0400001F RID: 31
		private const string CertificateKey = "C01386A907496404F276C3C1853ABF4A5274AF88";

		// Token: 0x04000020 RID: 32
		private const string CertificateVersion = "1.3.6.1.4.1.311.10.3.6";

		// Token: 0x04000021 RID: 33
		private readonly FfuFileInfoService ffuFileInfo;

		// Token: 0x04000022 RID: 34
		private string signToolPath;

		// Token: 0x04000023 RID: 35
		private string imageSignerPath;

		// Token: 0x04000024 RID: 36
		private string extractedCatalogFilePath;

		// Token: 0x04000025 RID: 37
		private string workingDirectory;
	}
}
