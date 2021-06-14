using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions.HTC;

namespace Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation.Services
{
	// Token: 0x02000008 RID: 8
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Htc)]
	public class HtcAdaptation : BaseAdaptation
	{
		// Token: 0x0600001F RID: 31 RVA: 0x0000278C File Offset: 0x0000098C
		[ImportingConstructor]
		internal HtcAdaptation(MsrService msrService, ReportingService reportingService, HtcModelsCatalog htcModelsCatalog)
		{
			this.salesNameProvider = new SalesNameProvider();
			this.reportingService = reportingService;
			this.htcModelsCatalog = htcModelsCatalog;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027DC File Offset: 0x000009DC
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Htc;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027F0 File Offset: 0x000009F0
		public override string PackageExtension
		{
			get
			{
				return "nbh";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002808 File Offset: 0x00000A08
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000281C File Offset: 0x00000A1C
		public override string ManufacturerName
		{
			get
			{
				return "HTC";
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002834 File Offset: 0x00000A34
		public override string ReportManufacturerName
		{
			get
			{
				return "HTC";
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000284C File Offset: 0x00000A4C
		public override ISalesNameProvider SalesNameProvider()
		{
			return this.salesNameProvider;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000287C File Offset: 0x00000A7C
		protected override void FillSupportedDeviceIdentifiers()
		{
			foreach (VidPidPair vidPidPair in this.htcModelsCatalog.Models.SelectMany((ModelInfo m) => m.VidPidPairs))
			{
				this.SupportedNormalModeIds.Add(new DeviceIdentifier(vidPidPair.Vid, vidPidPair.Pid));
			}
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("0BB4", "00CE"));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002930 File Offset: 0x00000B30
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002938 File Offset: 0x00000B38
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			Tracer<HtcAdaptation>.LogEntry("CompareFirmwareVersions");
			Tracer<HtcAdaptation>.LogExit("CompareFirmwareVersions");
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002964 File Offset: 0x00000B64
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			int num = 15;
			HtcDeviceInfo htcDeviceInfo;
			do
			{
				htcDeviceInfo = this.ReadDeviceInfo(phone.InstanceId);
				if (htcDeviceInfo == null || string.IsNullOrEmpty(htcDeviceInfo.Mid) || string.IsNullOrEmpty(htcDeviceInfo.Cid))
				{
					Tracer<HtcAdaptation>.WriteInformation("Unable to read MID and CID from HTCDeviceInfo.exe");
					Thread.Sleep(1000);
				}
			}
			while (!cancellationToken.IsCancellationRequested && (htcDeviceInfo == null || string.IsNullOrEmpty(htcDeviceInfo.Mid)) && num-- > 0);
			return htcDeviceInfo != null;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029F8 File Offset: 0x00000BF8
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HtcAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				QueryParameters queryParameters = HtcMsrQuery.CreateQueryParameters(phone.Mid, phone.Cid);
				string htcProductsPath = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetHtcProductsPath(string.Format("{0}-{1}", queryParameters.ManufacturerHardwareModel, queryParameters.ManufacturerHardwareVariant));
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = queryParameters,
					DestinationFolder = htcProductsPath,
					FilesVersioned = true
				};
				Tracer<HtcAdaptation>.WriteInformation("Download Params: {0}", new object[]
				{
					downloadParameters
				});
				phone.PackageFiles = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken);
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationSucceded(phone, ReportOperationType.DownloadPackage);
			}
			catch (Exception ex)
			{
				bool flag = true;
				UriData resultUriData;
				if (ex is OperationCanceledException || ex.GetBaseException() is TaskCanceledException)
				{
					resultUriData = UriData.DownloadVariantPackageAbortedByUser;
					flag = false;
				}
				else
				{
					resultUriData = UriData.FailedToDownloadVariantPackage;
				}
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationFailed(phone, ReportOperationType.DownloadPackage, resultUriData, ex);
				Tracer<HtcAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<HtcAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002BC4 File Offset: 0x00000DC4
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BF4 File Offset: 0x00000DF4
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HtcAdaptation>.LogEntry("FlashDevice");
			base.RaiseProgressPercentageChanged(-1, null);
			this.lastProgressPercentage = 0;
			Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckAndCreatePath(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.HtcProductsPath);
			if (string.IsNullOrEmpty(phone.Mid) && (string.IsNullOrEmpty(phone.Cid) || !ApplicationInfo.IsInternal()))
			{
				throw new FlashException("Device data is missing (Mid and/or Cid)");
			}
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (string text3 in phone.PackageFiles)
			{
				string fileName = Path.GetFileName(text3);
				if (fileName != null && fileName.ToLower().StartsWith("uefi"))
				{
					text = text3;
				}
				else if (fileName != null && fileName.ToLower().StartsWith("ruu"))
				{
					text2 = text3;
				}
			}
			this.CheckForMissingFiles(new string[]
			{
				text,
				text2,
				Path.Combine(this.GetWorkingDirectoryPath(), "HTCRomUpdater.exe")
			});
			try
			{
				this.FlashDevice("HTCRomUpdater.exe", text, text2, phone.Path);
			}
			finally
			{
				Tracer<HtcAdaptation>.LogExit("FlashDevice");
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D6C File Offset: 0x00000F6C
		private void CheckForMissingFiles(params string[] files)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in files)
			{
				if (!Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckIfFileIsValid(text))
				{
					stringBuilder.Append(text + "\n");
				}
			}
			if (stringBuilder.Length > 0)
			{
				throw new FileNotFoundException(stringBuilder.ToString());
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DDC File Offset: 0x00000FDC
		private void FlashDevice(string appName, string uefiFile, string ruuFile, string phonePath)
		{
			this.flashingResult = null;
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				FileName = appName,
				Arguments = string.Format("\"{0}\" \"{1}\" -s \"{2}\"", uefiFile, ruuFile, phonePath),
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				WorkingDirectory = this.GetWorkingDirectoryPath()
			};
			Tracer<HtcAdaptation>.WriteInformation("filename: {0} | working directory: {1} | arguments: {2}", new object[]
			{
				appName,
				processStartInfo.WorkingDirectory,
				processStartInfo.Arguments
			});
			ProcessHelper processHelper = new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = processStartInfo
			};
			processHelper.OutputDataReceived += this.FlashOnOutputDataReceived;
			Tracer<HtcAdaptation>.WriteInformation("Starting flash process");
			processHelper.Start();
			processHelper.BeginOutputReadLine();
			processHelper.WaitForExit();
			Tracer<HtcAdaptation>.WriteInformation("Flash process finished");
			this.CheckForPossibleFlashErrors();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002EC8 File Offset: 0x000010C8
		private void CheckForPossibleFlashErrors()
		{
			if (string.IsNullOrWhiteSpace(this.flashingResult) || this.flashingResult == "0")
			{
				return;
			}
			if (this.flashingResult.StartsWith("8007"))
			{
				throw new HtcUsbPortOpenException();
			}
			if (this.flashingResult.StartsWith("A001"))
			{
				throw new HtcDeviceCommunicationException();
			}
			if (this.flashingResult.StartsWith("A002"))
			{
				throw new HtcServiceControlException();
			}
			if (this.flashingResult.StartsWith("A003"))
			{
				throw new HtcUsbCommunicationException();
			}
			if (this.flashingResult.StartsWith("A011"))
			{
				throw new HtcDeviceHandshakingException();
			}
			if (this.flashingResult.StartsWith("A012"))
			{
				throw new HtcPackageFileCheckException();
			}
			throw new FlashException(this.lastProgressMessage);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002FC4 File Offset: 0x000011C4
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HtcAdaptation>.LogEntry("CheckPackageIntegrity");
			Tracer<HtcAdaptation>.WriteWarning("NOT IMPLEMENTED!!!", new object[0]);
			Tracer<HtcAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002FF0 File Offset: 0x000011F0
		private string GetWorkingDirectoryPath()
		{
			string directoryName = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
			if (string.IsNullOrWhiteSpace(directoryName))
			{
				Tracer<HtcAdaptation>.WriteError("Could not find working directory path", new object[0]);
				throw new Exception("Could not find working directory path");
			}
			return directoryName;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000304C File Offset: 0x0000124C
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003054 File Offset: 0x00001254
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<HtcAdaptation>.LogEntry("CheckLatestPackage");
				Task<PackageFileInfo> task = this.msrService.CheckLatestPackage(this.DeviceQueryParameters(phone), cancellationToken);
				task.Wait(cancellationToken);
				result = task.Result;
			}
			catch (Exception ex)
			{
				if (ex.InnerException is PackageNotFoundException)
				{
					throw ex.InnerException;
				}
				if (ex.InnerException != null && ex.InnerException.GetBaseException() is WebException)
				{
					throw new WebException();
				}
				if (ex is OperationCanceledException || ex.InnerException is TaskCanceledException)
				{
					throw;
				}
				throw;
			}
			finally
			{
				Tracer<HtcAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003134 File Offset: 0x00001334
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			Tracer<HtcAdaptation>.LogEntry("FindPackage");
			Tracer<HtcAdaptation>.LogExit("FindPackage");
			return new List<PackageFileInfo>();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003164 File Offset: 0x00001364
		public override void ReadDeviceInfo(Phone phone, CancellationToken token)
		{
			HtcDeviceInfo htcDeviceInfo;
			do
			{
				htcDeviceInfo = this.ReadDeviceInfo(phone.InstanceId);
				if (htcDeviceInfo == null || string.IsNullOrEmpty(htcDeviceInfo.Mid) || string.IsNullOrEmpty(htcDeviceInfo.Cid))
				{
					Tracer<HtcAdaptation>.WriteInformation("Unable to read MID and CID from HTCDeviceInfo.exe");
					Thread.Sleep(1000);
				}
				else
				{
					base.RaiseDeviceInfoRead(new Phone(phone.PortId, phone.Vid, phone.Pid, phone.LocationPath, phone.HardwareModel, phone.HardwareVariant, phone.SalesName, phone.SoftwareVersion, phone.Path, phone.Type, phone.InstanceId, phone.SalesNameProvider, true, htcDeviceInfo.Mid, htcDeviceInfo.Cid));
				}
			}
			while (!token.IsCancellationRequested && (htcDeviceInfo == null || string.IsNullOrEmpty(htcDeviceInfo.Mid)));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003278 File Offset: 0x00001478
		private HtcDeviceInfo ReadDeviceInfo(string instanceId)
		{
			Tracer<HtcAdaptation>.LogEntry("ReadDeviceInfo");
			this.deviceInfos = new Dictionary<string, HtcDeviceInfo>();
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "HTCDeviceInfo.exe",
				Arguments = "all",
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				WorkingDirectory = this.GetWorkingDirectoryPath()
			};
			ProcessHelper processHelper = new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = startInfo
			};
			processHelper.OutputDataReceived += this.ReadDeviceInfoOnOutputDataReceived;
			processHelper.Start();
			processHelper.BeginOutputReadLine();
			processHelper.WaitForExit();
			Tracer<HtcAdaptation>.LogExit("ReadDeviceInfo");
			string deviceId = instanceId.Substring(instanceId.LastIndexOf('\\') + 2).ToLower();
			return this.deviceInfos.Values.FirstOrDefault((HtcDeviceInfo di) => di.Path.ToLower().Contains(deviceId));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000033CC File Offset: 0x000015CC
		private void ReadDeviceInfoOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			try
			{
				string data = dataReceivedEventArgs.Data;
				if (!string.IsNullOrEmpty(data))
				{
					if (data.ToLower().Contains("error"))
					{
						Tracer<HtcAdaptation>.WriteError("Unable to read device info.", new object[0]);
					}
					else
					{
						string text;
						string text2;
						this.TryReadNumberValue(data, out text, out text2);
						if (text2.ToLower().Contains("usb#"))
						{
							this.deviceInfos.Add(text, new HtcDeviceInfo(text2));
							Tracer<HtcAdaptation>.WriteInformation("{0} usb Path found: {1}.", new object[]
							{
								text,
								text2
							});
						}
						else if (text2.ToLower().Contains("mid"))
						{
							int num = text2.Trim().IndexOf(" ", StringComparison.Ordinal);
							string text3 = text2.Trim().Substring(num + 1, 8);
							string text4 = Path.GetInvalidFileNameChars().Aggregate(text3, (string current, char c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), "0"));
							this.deviceInfos[text].Mid = text4;
							Tracer<HtcAdaptation>.WriteInformation("{0} mid found: {1}, replaced with {2}.", new object[]
							{
								text,
								text3,
								text4
							});
						}
						else if (text2.ToLower().Contains("cid"))
						{
							int num = text2.Trim().IndexOf(" ", StringComparison.Ordinal);
							string text5 = text2.Trim().Substring(num + 1);
							string text6 = Path.GetInvalidFileNameChars().Aggregate(text5, (string current, char c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), "0"));
							this.deviceInfos[text].Cid = text6;
							Tracer<HtcAdaptation>.WriteInformation("{0} cid found: {1}, replaced with {2}.", new object[]
							{
								text,
								text5,
								text6
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Tracer<HtcAdaptation>.WriteWarning("Error parsing HTCDeviceInfo output. Unable to parse string: {0}, exception {1}", new object[]
				{
					dataReceivedEventArgs.Data,
					ex
				});
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003628 File Offset: 0x00001828
		private void TryReadNumberValue(string input, out string number, out string value)
		{
			number = null;
			value = null;
			try
			{
				number = input.Substring(0, 3);
				value = input.Substring(4);
			}
			catch (Exception ex)
			{
				Tracer<HtcAdaptation>.WriteWarning("Error parsing HTCDeviceInfo output. Unable to parse string: {0}, exception {1}", new object[]
				{
					input,
					ex
				});
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003684 File Offset: 0x00001884
		private void FlashOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			try
			{
				string data = dataReceivedEventArgs.Data;
				Tracer<HtcAdaptation>.WriteInformation(data);
				if (data.Contains("[MESSAGE]"))
				{
					this.lastProgressMessage = data.Substring(9, data.IndexOf("[/MESSAGE]", StringComparison.Ordinal) - 9);
				}
				if (data.Contains("[PERCENTAGE]"))
				{
					string s = data.Substring(12, data.IndexOf("[/PERCENTAGE]", StringComparison.Ordinal) - 12);
					this.lastProgressPercentage = int.Parse(s);
				}
				if (data.Contains("[RESULT]"))
				{
					this.flashingResult = data.Substring(8, data.IndexOf("[/RESULT]", StringComparison.Ordinal) - 8);
				}
				base.RaiseProgressPercentageChanged(this.lastProgressPercentage, this.lastProgressMessage);
			}
			catch (Exception ex)
			{
				Tracer<HtcAdaptation>.WriteWarning("Error parsing HTCRomUpdater output. Unable to parse string: {0}, exception {1}", new object[]
				{
					dataReceivedEventArgs.Data,
					ex
				});
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000037C4 File Offset: 0x000019C4
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.Vid.ToUpper() == "0BB4" && (phone.Pid.ToUpper() == "0BAC" || phone.Pid.ToUpper() == "0BAD" || phone.Pid.ToUpper() == "0BAE"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("HTCOne.png"));
			}
			else if (phone.Vid.ToUpper() == "0BB4")
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("HTC8X.png"));
			}
			Stream result;
			if (!string.IsNullOrEmpty(text))
			{
				result = executingAssembly.GetManifestResourceStream(text);
			}
			else
			{
				result = base.GetImageDataStream(phone);
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000038E4 File Offset: 0x00001AE4
		private QueryParameters DeviceQueryParameters(Phone phone)
		{
			return new QueryParameters
			{
				ManufacturerName = "HTC",
				ManufacturerHardwareModel = phone.Mid,
				ManufacturerHardwareVariant = phone.Cid
			};
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003940 File Offset: 0x00001B40
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("HtcLogo.png"));
			Stream result;
			if (!string.IsNullOrEmpty(text))
			{
				result = executingAssembly.GetManifestResourceStream(text);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000012 RID: 18
		private const string RuuExeFile = "HTCRomUpdater.exe";

		// Token: 0x04000013 RID: 19
		private const int NumberOfTrials = 15;

		// Token: 0x04000014 RID: 20
		private readonly MsrService msrService;

		// Token: 0x04000015 RID: 21
		private readonly ReportingService reportingService;

		// Token: 0x04000016 RID: 22
		private readonly HtcModelsCatalog htcModelsCatalog;

		// Token: 0x04000017 RID: 23
		private readonly SalesNameProvider salesNameProvider;

		// Token: 0x04000018 RID: 24
		private Dictionary<string, HtcDeviceInfo> deviceInfos;

		// Token: 0x04000019 RID: 25
		private string lastProgressMessage;

		// Token: 0x0400001A RID: 26
		private int lastProgressPercentage;

		// Token: 0x0400001B RID: 27
		private string flashingResult;
	}
}
