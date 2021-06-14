using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public class MsrReport : IReport
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00007683 File Offset: 0x00005883
		private MsrReport()
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000768E File Offset: 0x0000588E
		public MsrReport(string sessionId)
		{
			this.SessionId = sessionId;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000076A4 File Offset: 0x000058A4
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000076BC File Offset: 0x000058BC
		public string DownloadedBytes
		{
			get
			{
				return this.downloadedBytes;
			}
			set
			{
				this.downloadedBytes = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000076CC File Offset: 0x000058CC
		// (set) Token: 0x06000142 RID: 322 RVA: 0x000076E4 File Offset: 0x000058E4
		public string PackageSizeOnServer
		{
			get
			{
				return this.packageSizeOnServer;
			}
			set
			{
				this.packageSizeOnServer = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000076F4 File Offset: 0x000058F4
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000770C File Offset: 0x0000590C
		public string ResumeCounter
		{
			get
			{
				return this.resumeCounter;
			}
			set
			{
				this.resumeCounter = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000771C File Offset: 0x0000591C
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00007734 File Offset: 0x00005934
		public string ActionDescription
		{
			get
			{
				return this.actionDescription;
			}
			set
			{
				this.actionDescription = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00007744 File Offset: 0x00005944
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000775C File Offset: 0x0000595C
		public string SystemInfo
		{
			get
			{
				return this.systemInfo;
			}
			set
			{
				this.systemInfo = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000776C File Offset: 0x0000596C
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00007784 File Offset: 0x00005984
		public string OsVersion
		{
			get
			{
				return this.osVersion;
			}
			set
			{
				this.osVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00007794 File Offset: 0x00005994
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000077AC File Offset: 0x000059AC
		public string OsPlatform
		{
			get
			{
				return this.osPlatform;
			}
			set
			{
				this.osPlatform = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000077BC File Offset: 0x000059BC
		// (set) Token: 0x0600014E RID: 334 RVA: 0x000077D4 File Offset: 0x000059D4
		public string ManufacturerHardwareModel
		{
			get
			{
				return this.productType;
			}
			set
			{
				this.productType = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000077E4 File Offset: 0x000059E4
		// (set) Token: 0x06000150 RID: 336 RVA: 0x000077FC File Offset: 0x000059FC
		public string ManufacturerHardwareVariant
		{
			get
			{
				return this.productCode;
			}
			set
			{
				this.productCode = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000780C File Offset: 0x00005A0C
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00007824 File Offset: 0x00005A24
		public string Imei
		{
			get
			{
				return this.imei;
			}
			set
			{
				this.imei = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00007834 File Offset: 0x00005A34
		// (set) Token: 0x06000154 RID: 340 RVA: 0x0000784C File Offset: 0x00005A4C
		public string Vid
		{
			get
			{
				return this.vid;
			}
			set
			{
				this.vid = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000785C File Offset: 0x00005A5C
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00007874 File Offset: 0x00005A74
		public string Pid
		{
			get
			{
				return this.pid;
			}
			set
			{
				this.pid = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00007884 File Offset: 0x00005A84
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000789C File Offset: 0x00005A9C
		public string Mid
		{
			get
			{
				return this.mid;
			}
			set
			{
				this.mid = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000078AC File Offset: 0x00005AAC
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000078C4 File Offset: 0x00005AC4
		public string Cid
		{
			get
			{
				return this.cid;
			}
			set
			{
				this.cid = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000078D4 File Offset: 0x00005AD4
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000078EC File Offset: 0x00005AEC
		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000078FC File Offset: 0x00005AFC
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00007914 File Offset: 0x00005B14
		public string SalesName
		{
			get
			{
				return this.salesName;
			}
			set
			{
				this.salesName = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007924 File Offset: 0x00005B24
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000793C File Offset: 0x00005B3C
		public string SwVersion
		{
			get
			{
				return this.swVersion;
			}
			set
			{
				this.swVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000794C File Offset: 0x00005B4C
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00007964 File Offset: 0x00005B64
		public string AkVersion
		{
			get
			{
				return this.akVersion;
			}
			set
			{
				this.akVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00007974 File Offset: 0x00005B74
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000798C File Offset: 0x00005B8C
		public string NewAkVersion
		{
			get
			{
				return this.newAkVersion;
			}
			set
			{
				this.newAkVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000799C File Offset: 0x00005B9C
		// (set) Token: 0x06000166 RID: 358 RVA: 0x000079B4 File Offset: 0x00005BB4
		public string NewSwVersion
		{
			get
			{
				return this.newSwVersion;
			}
			set
			{
				this.newSwVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000167 RID: 359 RVA: 0x000079C4 File Offset: 0x00005BC4
		// (set) Token: 0x06000168 RID: 360 RVA: 0x000079DC File Offset: 0x00005BDC
		public string LocalPath
		{
			get
			{
				return this.localPath;
			}
			set
			{
				this.localPath = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000079EC File Offset: 0x00005BEC
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00007A04 File Offset: 0x00005C04
		public string DebugField
		{
			get
			{
				return this.debugField;
			}
			set
			{
				string text = MsrReport.PrepareForCsvFormat(value);
				this.debugField = ((text.Length <= 2000) ? text : text.Substring(0, 2000));
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00007A3C File Offset: 0x00005C3C
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00007A54 File Offset: 0x00005C54
		public string ApiError
		{
			get
			{
				return this.apiError;
			}
			set
			{
				this.apiError = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00007A64 File Offset: 0x00005C64
		// (set) Token: 0x0600016E RID: 366 RVA: 0x00007A7C File Offset: 0x00005C7C
		public string ApiErrorMessage
		{
			get
			{
				return this.apiErrorMessage;
			}
			set
			{
				this.apiErrorMessage = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00007A8C File Offset: 0x00005C8C
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00007AA4 File Offset: 0x00005CA4
		public string ApplicationName
		{
			get
			{
				return this.applicationName;
			}
			set
			{
				this.applicationName = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00007AB4 File Offset: 0x00005CB4
		// (set) Token: 0x06000172 RID: 370 RVA: 0x00007ACC File Offset: 0x00005CCC
		public string ApplicationVendor
		{
			get
			{
				return this.applicationVendor;
			}
			set
			{
				this.applicationVendor = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00007ADC File Offset: 0x00005CDC
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public string ApplicationVersion
		{
			get
			{
				return this.applicationVersion;
			}
			set
			{
				this.applicationVersion = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00007B04 File Offset: 0x00005D04
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00007B1C File Offset: 0x00005D1C
		public string UserSiteLanguage
		{
			get
			{
				return this.userSiteLanguage;
			}
			set
			{
				this.userSiteLanguage = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007B2C File Offset: 0x00005D2C
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00007B44 File Offset: 0x00005D44
		public string CountryCode
		{
			get
			{
				return this.countryCode;
			}
			set
			{
				this.countryCode = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00007B54 File Offset: 0x00005D54
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00007B6C File Offset: 0x00005D6C
		public string FirmwareGrading
		{
			get
			{
				return this.firmwareGrading;
			}
			set
			{
				this.firmwareGrading = MsrReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007B7C File Offset: 0x00005D7C
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00007B94 File Offset: 0x00005D94
		public string ReportVersion
		{
			get
			{
				return this.reportVersion;
			}
			set
			{
				this.reportVersion = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007BA0 File Offset: 0x00005DA0
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00007BB8 File Offset: 0x00005DB8
		public long TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
			set
			{
				this.timeStamp = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007BC4 File Offset: 0x00005DC4
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00007BDB File Offset: 0x00005DDB
		public long DownloadDuration { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007BE4 File Offset: 0x00005DE4
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00007BFB File Offset: 0x00005DFB
		public long UpdateDuration { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007C04 File Offset: 0x00005E04
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00007C1B File Offset: 0x00005E1B
		public string SessionId { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00007C24 File Offset: 0x00005E24
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00007C3B File Offset: 0x00005E3B
		public UriData Uri { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00007C44 File Offset: 0x00005E44
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00007C5B File Offset: 0x00005E5B
		public PhoneTypes PhoneType { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00007C64 File Offset: 0x00005E64
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007C7B File Offset: 0x00005E7B
		public string ManufacturerName { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007C84 File Offset: 0x00005E84
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00007C9B File Offset: 0x00005E9B
		public string ManufacturerProductLine { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007CA4 File Offset: 0x00005EA4
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00007CBB File Offset: 0x00005EBB
		public string PlatformId { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007CC4 File Offset: 0x00005EC4
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00007CDB File Offset: 0x00005EDB
		public string PackageId { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007CE4 File Offset: 0x00005EE4
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00007CFB File Offset: 0x00005EFB
		public string LastErrorData { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007D04 File Offset: 0x00005F04
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00007D1B File Offset: 0x00005F1B
		public string Manufacturer { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007D24 File Offset: 0x00005F24
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007D3B File Offset: 0x00005F3B
		public bool Sent { get; private set; }

		// Token: 0x06000197 RID: 407 RVA: 0x00007D44 File Offset: 0x00005F44
		public void MarkAsSent()
		{
			this.Sent = true;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007D50 File Offset: 0x00005F50
		public string GetReportAsCsv()
		{
			List<string> values = new List<string>
			{
				this.SystemInfo,
				this.ActionDescription,
				this.ManufacturerHardwareModel,
				this.ManufacturerHardwareVariant,
				this.Imei,
				this.Vid,
				this.Pid,
				this.Mid,
				this.Cid,
				this.SerialNumber,
				this.SalesName,
				this.PhoneType.ToString(),
				this.SwVersion,
				this.AkVersion,
				this.NewAkVersion,
				this.NewSwVersion,
				this.DownloadDuration.ToString(),
				this.UpdateDuration.ToString(),
				this.ApiError,
				this.ApiErrorMessage,
				this.UriDataToUint(this.Uri).ToString(),
				this.ApplicationVersion,
				this.FirmwareGrading,
				this.LocalPath,
				this.PackageSizeOnServer,
				this.DownloadedBytes,
				this.ResumeCounter,
				this.ManufacturerName,
				this.ManufacturerProductLine
			};
			return string.Join(";", values);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007F18 File Offset: 0x00006118
		public string GetReportAsXml()
		{
			XDocument xdocument = new XDocument(new XDeclaration("1.0", "utf-8", "no"), new object[]
			{
				new XElement("Reports", new XElement("Report", new object[]
				{
					new XElement("reportVersion", "1.1"),
					new XElement("reportSessionId", this.SessionId),
					new XElement("reportTimeStamp", this.TimeStamp),
					new XElement("osSystemInfo", this.SystemInfo),
					new XElement("osVersion", this.OsVersion),
					new XElement("osPlatform", this.OsPlatform),
					new XElement("appApplicationName", this.ApplicationName),
					new XElement("appApplicationVendor", this.ApplicationVendor),
					new XElement("appApplicationVersion", this.ApplicationVersion),
					new XElement("appUserSiteLanguage", this.UserSiteLanguage),
					new XElement("appCountryCode", this.CountryCode),
					new XElement("flowUri", this.UriDataToUint(this.Uri).ToString()),
					new XElement("flowActionDescription", this.ActionDescription),
					new XElement("flowNewSwVersion", this.NewSwVersion),
					new XElement("flowNewAkVersion", this.NewAkVersion),
					new XElement("flowFirmwareGrading", this.FirmwareGrading),
					new XElement("flowLocalPath", this.LocalPath),
					new XElement("flowDownloadDuration", this.DownloadDuration),
					new XElement("flowUpdateDuration", this.UpdateDuration),
					new XElement("flowPackageSizeOnServer", this.PackageSizeOnServer),
					new XElement("flowDownloadedBytes", this.DownloadedBytes),
					new XElement("flowResumeCounter", this.ResumeCounter),
					new XElement("devManufacturer", this.Manufacturer),
					new XElement("devManufacturerName", this.ManufacturerName),
					new XElement("devManufacturerProductLine", this.ManufacturerProductLine),
					new XElement("devManufacturerHardwareModel", this.ManufacturerHardwareModel),
					new XElement("devManufacturerHardwareVariant", this.ManufacturerHardwareVariant),
					new XElement("devImei", this.Imei),
					new XElement("devVid", this.Vid),
					new XElement("devPid", this.Pid),
					new XElement("devMid", this.Mid),
					new XElement("devCid", this.Cid),
					new XElement("devSerialNumber", this.SerialNumber),
					new XElement("devSalesName", this.SalesName),
					new XElement("devPhoneType", this.PhoneType),
					new XElement("devSwVersion", this.SwVersion),
					new XElement("devAkVersion", this.AkVersion),
					new XElement("devPlatformId", this.PlatformId),
					new XElement("devPackageId", this.PackageId),
					new XElement("DebugField", this.DebugField),
					new XElement("ApiError", this.ApiError),
					new XElement("ApiErrorMessage", this.ApiErrorMessage),
					new XElement("LastErrorData", this.LastErrorData)
				}))
			});
			return xdocument.ToString();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000083C8 File Offset: 0x000065C8
		public ReportUpdateStatus4Parameters CreateReportStatusParameters()
		{
			string text = this.FormatString(this.Imei, 100);
			if (this.PhoneType == PhoneTypes.Htc)
			{
				text = this.FormatString(this.SerialNumber, 100);
			}
			return new ReportUpdateStatus4Parameters
			{
				SystemInfo = this.SystemInfo,
				ActionDescription = this.FormatString(this.ActionDescription, 200),
				Uri = (long)this.Uri,
				UriDescription = this.FormatString(UriDataArgument.Description(this.Uri), 200),
				ApplicationName = this.FormatString(this.ApplicationName, 200),
				ApplicationVendorName = "Microsoft",
				ApplicationVersion = this.ApplicationVersion,
				ProductType = (string.IsNullOrWhiteSpace(this.ManufacturerHardwareModel) ? this.FormatString(this.SalesName, 100) : this.FormatString(this.ManufacturerHardwareModel, 100)),
				ProductCode = this.FormatString(this.ManufacturerHardwareVariant, 100),
				Imei = text,
				FirmwareVersionOld = this.FormatString(this.SwVersion, 200),
				FirmwareVersionNew = this.FormatString(this.NewSwVersion, 200),
				FwGrading = this.FirmwareGrading,
				Duration = this.DownloadDuration + this.UpdateDuration,
				DownloadDuration = this.DownloadDuration,
				UpdateDuration = this.UpdateDuration,
				ApiError = this.ApiError,
				ApiErrorText = this.FormatString(this.ApiErrorMessage, 400),
				TimeStamp = TimeStampUtility.CreateTimeStamp(),
				Ext1 = ((string.IsNullOrWhiteSpace(this.Vid) && string.IsNullOrWhiteSpace(this.Pid)) ? string.Empty : string.Format("Vid: {0}; Pid: {1};", this.Vid, this.Pid)),
				Ext2 = ((string.IsNullOrWhiteSpace(this.Mid) && string.IsNullOrWhiteSpace(this.Cid)) ? string.Empty : string.Format("Mid: {0}; Cid: {1};", this.Mid, this.Cid)),
				Ext3 = this.FormatString(this.SalesName, 100),
				Ext4 = this.PhoneType.ToString(),
				Ext7 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", new object[]
				{
					string.Empty,
					this.AkVersion,
					this.NewAkVersion,
					this.DownloadedBytes,
					this.PackageSizeOnServer,
					this.ResumeCounter,
					text
				}),
				Ext8 = ApplicationInfo.CurrentLanguageInRegistry.EnglishName
			};
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008698 File Offset: 0x00006898
		private static string PrepareForCsvFormat(string field)
		{
			string result;
			if (!string.IsNullOrEmpty(field))
			{
				StringBuilder stringBuilder = new StringBuilder(field);
				stringBuilder.Replace(',', ';');
				stringBuilder.Replace("\r\n", " ");
				stringBuilder.Replace('\r', ' ');
				stringBuilder.Replace('\n', ' ');
				result = stringBuilder.ToString();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000086FC File Offset: 0x000068FC
		private uint UriDataToUint(UriData uriData)
		{
			return (uint)uriData;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008714 File Offset: 0x00006914
		private string FormatString(string source, int maxLength)
		{
			string result;
			if (string.IsNullOrEmpty(source))
			{
				result = "Unknown";
			}
			else
			{
				result = this.Truncate(source, maxLength);
			}
			return result;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008744 File Offset: 0x00006944
		private string Truncate(string source, int length)
		{
			if (source.Length > length)
			{
				source = source.Substring(0, length);
			}
			return source;
		}

		// Token: 0x04000098 RID: 152
		public const string REPORTVERSION = "1.1";

		// Token: 0x04000099 RID: 153
		private string actionDescription;

		// Token: 0x0400009A RID: 154
		private string akVersion;

		// Token: 0x0400009B RID: 155
		private string apiError;

		// Token: 0x0400009C RID: 156
		private string apiErrorMessage;

		// Token: 0x0400009D RID: 157
		private string applicationVersion;

		// Token: 0x0400009E RID: 158
		private string applicationVendor;

		// Token: 0x0400009F RID: 159
		private string applicationName;

		// Token: 0x040000A0 RID: 160
		private string cid;

		// Token: 0x040000A1 RID: 161
		private string debugField;

		// Token: 0x040000A2 RID: 162
		private string firmwareGrading;

		// Token: 0x040000A3 RID: 163
		private string imei;

		// Token: 0x040000A4 RID: 164
		private string localPath;

		// Token: 0x040000A5 RID: 165
		private string mid;

		// Token: 0x040000A6 RID: 166
		private string newAkVersion;

		// Token: 0x040000A7 RID: 167
		private string newSwVersion;

		// Token: 0x040000A8 RID: 168
		private string pid;

		// Token: 0x040000A9 RID: 169
		private string productCode;

		// Token: 0x040000AA RID: 170
		private string productType;

		// Token: 0x040000AB RID: 171
		private string reportVersion;

		// Token: 0x040000AC RID: 172
		private string salesName;

		// Token: 0x040000AD RID: 173
		private string serialNumber;

		// Token: 0x040000AE RID: 174
		private string systemInfo;

		// Token: 0x040000AF RID: 175
		private string osVersion;

		// Token: 0x040000B0 RID: 176
		private string osPlatform;

		// Token: 0x040000B1 RID: 177
		private long timeStamp;

		// Token: 0x040000B2 RID: 178
		private string userSiteLanguage;

		// Token: 0x040000B3 RID: 179
		private string countryCode;

		// Token: 0x040000B4 RID: 180
		private string vid;

		// Token: 0x040000B5 RID: 181
		private string downloadedBytes;

		// Token: 0x040000B6 RID: 182
		private string packageSizeOnServer;

		// Token: 0x040000B7 RID: 183
		private string resumeCounter;

		// Token: 0x040000B8 RID: 184
		private string swVersion;
	}
}
