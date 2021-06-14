using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003D RID: 61
	[Export(typeof(MsrReportingService))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	[Export(typeof(IUseProxy))]
	public class MsrReportingService : IUseProxy
	{
		// Token: 0x06000323 RID: 803 RVA: 0x0000CED6 File Offset: 0x0000B0D6
		[ImportingConstructor]
		public MsrReportingService()
		{
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000CEEC File Offset: 0x0000B0EC
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000CF03 File Offset: 0x0000B103
		public IManufacturerDataProvider ManufacturerDataProvider { get; set; }

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000326 RID: 806 RVA: 0x0000CF0C File Offset: 0x0000B10C
		// (remove) Token: 0x06000327 RID: 807 RVA: 0x0000CF48 File Offset: 0x0000B148
		public event Action SessionReportsSendingCompleted;

		// Token: 0x06000328 RID: 808 RVA: 0x0000CF84 File Offset: 0x0000B184
		private void Initialize()
		{
			this.msrServiceData = MsrServiceData.CreateServiceData();
			MsrReporting msrReporting = new MsrReporting(this);
			this.msrReportSender = new MsrReportSender(msrReporting);
			this.msrReportSender.SendOldReports();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000D1AC File Offset: 0x0000B3AC
		public async Task SendReportAsync(IReport report)
		{
			if (!report.Sent)
			{
				string uploadUrl = await this.ProvideReportUploadUrlAsync(report, null);
				if (!string.IsNullOrWhiteSpace(uploadUrl))
				{
					if (await this.UploadWithHttpClientAsync(uploadUrl, report))
					{
						report.MarkAsSent();
					}
				}
				else
				{
					Tracer<MsrReportingService>.WriteWarning("MSR Reporting bad request parameters, error 400", new object[0]);
				}
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000D200 File Offset: 0x0000B400
		public void OperationStarted(Phone phone, ReportOperationType reportOperationType)
		{
			ReportData reportData = this.GetReportData(phone, reportOperationType, reportOperationType == ReportOperationType.DownloadPackage);
			switch (reportOperationType)
			{
			case ReportOperationType.Flashing:
			case ReportOperationType.Recovery:
				break;
			case ReportOperationType.ReadDeviceInfo:
			case ReportOperationType.ReadDeviceInfoWithThor:
				return;
			case ReportOperationType.DownloadPackage:
				reportData.StartDownloadTimer();
				return;
			default:
				if (reportOperationType != ReportOperationType.RecoveryAfterEmergencyFlashing)
				{
					return;
				}
				break;
			}
			reportData.StartUpdateTimer();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D250 File Offset: 0x0000B450
		public void OperationSucceded(Phone phone, ReportOperationType reportOperationType, UriData uriData)
		{
			Tracer<MsrReportingService>.LogEntry("OperationSucceded");
			Tracer<MsrReportingService>.WriteInformation("Operation: {0} succeeded", new object[]
			{
				reportOperationType
			});
			ReportData reportData = this.GetReportData(phone, reportOperationType, false);
			reportData.SetPhoneInfo(phone);
			this.SendReport(reportData);
			this.RemoveReportData(phone, reportOperationType);
			Tracer<MsrReportingService>.LogExit("OperationSucceded");
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		public void OperationSucceded(Phone phone, ReportOperationType reportOperationType)
		{
			Tracer<MsrReportingService>.LogEntry("OperationSucceded");
			Tracer<MsrReportingService>.WriteInformation("Operation: {0} succeeded", new object[]
			{
				reportOperationType
			});
			ReportData reportData = this.GetReportData(phone, reportOperationType, false);
			switch (reportOperationType)
			{
			case ReportOperationType.Flashing:
				this.SendReport(reportData, UriData.FirmwareUpdateSuccessful);
				goto IL_E6;
			case ReportOperationType.Recovery:
				this.SendReport(reportData, UriData.DeadPhoneRecovered);
				goto IL_E6;
			case ReportOperationType.DownloadPackage:
				if (reportData.DownloadedBytes != 0L)
				{
					this.SendReport(reportData, UriData.DownloadPackageSuccess);
				}
				else
				{
					this.msrReportSender.RemoveLocalReport(reportData.LocalPath);
				}
				goto IL_E6;
			case ReportOperationType.EmergencyFlashing:
				reportData.SetPhoneInfo(phone);
				this.SendReport(reportData, UriData.EmergencyFlashingSuccesfullyFinished);
				goto IL_E6;
			case ReportOperationType.RecoveryAfterEmergencyFlashing:
				this.SendReport(reportData, UriData.DeadPhoneRecoveredAfterEmergencyFlashing);
				goto IL_E6;
			}
			this.msrReportSender.RemoveLocalReport(reportData.LocalPath);
			IL_E6:
			this.RemoveReportData(phone, reportOperationType);
			Tracer<MsrReportingService>.LogExit("OperationSucceded");
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D3BB File Offset: 0x0000B5BB
		public void SurveySucceded(SurveyReport survey)
		{
			survey.SessionId = this.GetSessionId();
			this.msrReportSender.SendReport(survey, ApplicationInfo.IsInternal());
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
		public void PartialOperationSucceded(Phone phone, ReportOperationType reportOperationType, UriData uriData)
		{
			Tracer<MsrReportingService>.LogEntry("PartialOperationSucceded");
			Tracer<MsrReportingService>.WriteInformation("Operation: {0} succeeded", new object[]
			{
				reportOperationType
			});
			if (reportOperationType == ReportOperationType.EmergencyFlashing)
			{
				this.UpdateReportWithImeiNumber(phone, reportOperationType);
			}
			ReportData reportData = this.GetReportData(phone, reportOperationType, false);
			reportData.SetPhoneInfo(phone);
			reportData.SetResult(uriData, null);
			Tracer<MsrReportingService>.LogExit("PartialOperationSucceded");
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D454 File Offset: 0x0000B654
		public void OperationFailed(Phone phone, ReportOperationType reportOperationType, UriData resultUriData, Exception ex)
		{
			ReportData reportData = this.GetReportData(phone, reportOperationType, false);
			if (ex != null)
			{
				ex = ex.GetBaseException();
			}
			reportData.SetResult(resultUriData, ex);
			this.msrReportSender.SaveLocalReport(reportData);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D498 File Offset: 0x0000B698
		public void SetDownloadByteInformation(Phone phone, ReportOperationType reportOperationType, long currentDownloadedSize, long packageSize, bool isResumed)
		{
			ReportData reportData = this.GetReportData(phone, reportOperationType, isResumed);
			reportData.PackageSize = packageSize;
			reportData.DownloadedBytes = currentDownloadedSize;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D4C4 File Offset: 0x0000B6C4
		public void SendSessionReports()
		{
			this.msrReportSender.SessionReportsSendingCompleted += this.SessionReportsSendingCompleted;
			lock (this.msrReports)
			{
				this.msrReportSender.SendSessionReports(this.msrReports.Values.ToList<ReportData>(), ApplicationInfo.IsInternal());
				this.msrReports.Clear();
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000D548 File Offset: 0x0000B748
		public void StartFlowSession()
		{
			this.currentSessionId = Guid.NewGuid().ToString();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		private async Task<string> ProvideReportUploadUrlAsync(IReport report, ManufacturerInfo manufacturerInfo = null)
		{
			string result = null;
			HttpClientHandler httpClientHandler = new HttpClientHandler
			{
				Proxy = this.Proxy(),
				UseDefaultCredentials = false
			};
			using (HttpClient client = new HttpClient(httpClientHandler))
			{
				this.AddDefaultHeaders(client);
				DateTime time = DateTime.UtcNow;
				RequestBody requestBody = new RequestBody
				{
					ManufacturerName = ((manufacturerInfo != null) ? manufacturerInfo.ReportManufacturerName : report.ManufacturerName),
					ManufacturerProductLine = ((manufacturerInfo != null) ? manufacturerInfo.ReportProductLine : report.ManufacturerProductLine),
					ReportClassification = "Public",
					FileName = string.Format("{0:yyyyMMdd}{0:HHmmss_ff}_{1}_{2}_{3}{4}.xml", new object[]
					{
						time,
						report.ManufacturerHardwareModel,
						report.ManufacturerHardwareVariant,
						report.Imei,
						(report is SurveyReport) ? "_Survey" : string.Empty
					})
				};
				string postBody = requestBody.ToJsonString();
				using (HttpResponseMessage response = await client.PostAsync(new Uri(this.msrServiceData.UploadApiUrl), new StringContent(postBody, Encoding.UTF8, "application/json"), CancellationToken.None))
				{
					if (response.IsSuccessStatusCode)
					{
						result = response.Headers.Location.AbsoluteUri;
					}
					else if (manufacturerInfo == null)
					{
						if (this.ManufacturerDataProvider == null)
						{
							Tracer<MsrReportingService>.WriteWarning("ManufacturerDataProvider value was not set", new object[0]);
							throw new HttpRequestException(string.Format("Could not provide reporting upload url. Status: {0}", response.StatusCode));
						}
						Tracer<MsrReportingService>.WriteWarning("Could not provide report upload url using params: {0}", new object[]
						{
							postBody
						});
						Tracer<MsrReportingService>.WriteInformation("Try use manufacturer extracted data for getting upload url");
						ManufacturerInfo minfo = this.ManufacturerDataProvider.GetAdaptationsData().FirstOrDefault((ManufacturerInfo mi) => mi.Type == report.PhoneType);
						if (minfo != null)
						{
							result = await this.ProvideReportUploadUrlAsync(report, minfo);
						}
						else
						{
							Tracer<MsrReportingService>.WriteWarning("No manufacturer for reported type '{0}' found", new object[]
							{
								report.PhoneType
							});
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000DDD0 File Offset: 0x0000BFD0
		private async Task<bool> UploadWithHttpClientAsync(string reportFileUri, IReport report)
		{
			HttpClientHandler httpClientHandler = new HttpClientHandler
			{
				Proxy = this.Proxy(),
				UseDefaultCredentials = false
			};
			bool isSuccessStatusCode;
			using (HttpClient client = new HttpClient(httpClientHandler))
			{
				string dateInRfc1123Format = DateTime.UtcNow.ToString("R");
				StringContent content = new StringContent(report.GetReportAsXml());
				client.Timeout = Timeout.InfiniteTimeSpan;
				client.DefaultRequestHeaders.Add("x-ms-blob-type", BlobType.BlockBlob.ToString());
				client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
				HttpResponseMessage reqResult = await client.PutAsync(reportFileUri, content);
				isSuccessStatusCode = reqResult.IsSuccessStatusCode;
			}
			return isSuccessStatusCode;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E048 File Offset: 0x0000C248
		private async Task UploadWithHttpClientAsync(string reportFileUri, string content)
		{
			HttpClientHandler httpClientHandler = new HttpClientHandler
			{
				Proxy = this.Proxy(),
				UseDefaultCredentials = false
			};
			using (HttpClient client = new HttpClient(httpClientHandler))
			{
				string dateInRfc1123Format = DateTime.UtcNow.ToString("R");
				StringContent stringContent = new StringContent(content);
				client.Timeout = Timeout.InfiniteTimeSpan;
				client.DefaultRequestHeaders.Add("x-ms-blob-type", BlobType.BlockBlob.ToString());
				client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
				await client.PutAsync(reportFileUri, stringContent);
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E0A2 File Offset: 0x0000C2A2
		private void SendReport(ReportData reportData, UriData reportResultUriData)
		{
			reportData.SetResult(reportResultUriData, null);
			this.SendReport(reportData);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E0B6 File Offset: 0x0000C2B6
		private void SendReport(ReportData reportData)
		{
			reportData.EndDataCollecting();
			this.msrReportSender.SendReport(reportData, ApplicationInfo.IsInternal());
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		private ReportData StartDataCollecting(string description, Phone phone)
		{
			ReportData reportData = new ReportData(description, this.GetSessionId());
			reportData.SetPhoneInfo(phone);
			return reportData;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E0FC File Offset: 0x0000C2FC
		private ReportData GetReportData(Phone phone, ReportOperationType reportOperationType, bool resumeCounter = false)
		{
			string key = string.Format("{0}_{1}", phone.Imei, reportOperationType);
			ReportData result;
			lock (this.msrReports)
			{
				if (this.msrReports.ContainsKey(key))
				{
					Tracer<MsrReportingService>.WriteInformation("Getting existing report from dictionary");
					if (resumeCounter)
					{
						this.msrReports[key].ResumeCounter++;
						if (this.msrReports[key].Exception != null)
						{
							if (this.msrReports[key].LastError == null)
							{
								this.msrReports[key].LastError = this.msrReports[key].Exception;
							}
						}
					}
					result = this.msrReports[key];
				}
				else
				{
					Tracer<MsrReportingService>.WriteInformation("Create new report and add it to dictionary");
					ReportData value = this.StartDataCollecting(reportOperationType.ToString(), phone);
					this.msrReports.Add(key, value);
					result = this.msrReports[key];
				}
			}
			return result;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E258 File Offset: 0x0000C458
		private void RemoveReportData(Phone phone, ReportOperationType reportOperationType)
		{
			string key = string.Format("{0}_{1}", phone.Imei, reportOperationType);
			lock (this.msrReports)
			{
				if (this.msrReports.ContainsKey(key))
				{
					this.msrReports.Remove(key);
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		private void UpdateReportWithImeiNumber(Phone phone, ReportOperationType reportOperationType)
		{
			string key = string.Format("{0}_{1}", null, reportOperationType);
			string key2 = string.Format("{0}_{1}", phone.Imei, reportOperationType);
			lock (this.msrReports)
			{
				if (this.msrReports.ContainsKey(key))
				{
					ReportData value = this.msrReports[key];
					this.msrReports[key2] = value;
					this.msrReports.Remove(key);
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E38C File Offset: 0x0000C58C
		private UriData GetDefaultFailUriData(ReportOperationType reportOperationType)
		{
			switch (reportOperationType)
			{
			case ReportOperationType.Flashing:
				return UriData.ProgrammingPhoneFailed;
			case ReportOperationType.Recovery:
				return UriData.DeadPhoneRecoveryFailed;
			case ReportOperationType.ReadDeviceInfo:
			case ReportOperationType.ReadDeviceInfoWithThor:
				break;
			case ReportOperationType.DownloadPackage:
				return UriData.FailedToDownloadVariantPackage;
			default:
				if (reportOperationType == ReportOperationType.RecoveryAfterEmergencyFlashing)
				{
					return UriData.RecoveryAfterEmergencyFlashingFailed;
				}
				break;
			}
			return UriData.InvalidUriCode;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000E3DC File Offset: 0x0000C5DC
		private void AddDefaultHeaders(HttpClient client)
		{
			client.Timeout = Timeout.InfiniteTimeSpan;
			client.DefaultRequestHeaders.UserAgent.TryParseAdd(this.msrServiceData.UserAgent);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000E430 File Offset: 0x0000C630
		private string GetSessionId()
		{
			if (string.IsNullOrEmpty(this.currentSessionId) || string.Equals(this.currentSessionId, Guid.Empty.ToString()))
			{
				this.StartFlowSession();
			}
			return this.currentSessionId;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000E487 File Offset: 0x0000C687
		public void SetProxy(IWebProxy settings)
		{
			this.proxySettings = settings;
			this.Initialize();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000E498 File Offset: 0x0000C698
		private IWebProxy Proxy()
		{
			return this.proxySettings ?? WebRequest.GetSystemWebProxy();
		}

		// Token: 0x04000181 RID: 385
		private const string JsonContentType = "application/json";

		// Token: 0x04000182 RID: 386
		private readonly Dictionary<string, ReportData> msrReports = new Dictionary<string, ReportData>();

		// Token: 0x04000183 RID: 387
		private MsrReportSender msrReportSender;

		// Token: 0x04000184 RID: 388
		private MsrServiceData msrServiceData;

		// Token: 0x04000185 RID: 389
		private IWebProxy proxySettings;

		// Token: 0x04000186 RID: 390
		private string currentSessionId;
	}
}
