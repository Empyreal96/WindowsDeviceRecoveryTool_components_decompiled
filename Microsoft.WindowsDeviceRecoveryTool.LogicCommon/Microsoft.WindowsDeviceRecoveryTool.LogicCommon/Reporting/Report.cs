using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class Report
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00008774 File Offset: 0x00006974
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000878C File Offset: 0x0000698C
		public string DownloadedBytes
		{
			get
			{
				return this.downloadedBytes;
			}
			set
			{
				this.downloadedBytes = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000879C File Offset: 0x0000699C
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000087B4 File Offset: 0x000069B4
		public string PackageSizeOnServer
		{
			get
			{
				return this.packageSizeOnServer;
			}
			set
			{
				this.packageSizeOnServer = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x000087C4 File Offset: 0x000069C4
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x000087DC File Offset: 0x000069DC
		public string ResumeCounter
		{
			get
			{
				return this.resumeCounter;
			}
			set
			{
				this.resumeCounter = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000087EC File Offset: 0x000069EC
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00008804 File Offset: 0x00006A04
		public string ActionDescription
		{
			get
			{
				return this.actionDescription;
			}
			set
			{
				this.actionDescription = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00008814 File Offset: 0x00006A14
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000882C File Offset: 0x00006A2C
		public string SystemInfo
		{
			get
			{
				return this.systemInfo;
			}
			set
			{
				this.systemInfo = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000883C File Offset: 0x00006A3C
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00008854 File Offset: 0x00006A54
		public string ProductType
		{
			get
			{
				return this.productType;
			}
			set
			{
				this.productType = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00008864 File Offset: 0x00006A64
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000887C File Offset: 0x00006A7C
		public string ProductCode
		{
			get
			{
				return this.productCode;
			}
			set
			{
				this.productCode = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000888C File Offset: 0x00006A8C
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000088A4 File Offset: 0x00006AA4
		public string Imei
		{
			get
			{
				return this.imei;
			}
			set
			{
				this.imei = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001AF RID: 431 RVA: 0x000088B4 File Offset: 0x00006AB4
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x000088CC File Offset: 0x00006ACC
		public string Vid
		{
			get
			{
				return this.vid;
			}
			set
			{
				this.vid = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000088DC File Offset: 0x00006ADC
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000088F4 File Offset: 0x00006AF4
		public string Pid
		{
			get
			{
				return this.pid;
			}
			set
			{
				this.pid = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00008904 File Offset: 0x00006B04
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000891C File Offset: 0x00006B1C
		public string Mid
		{
			get
			{
				return this.mid;
			}
			set
			{
				this.mid = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000892C File Offset: 0x00006B2C
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00008944 File Offset: 0x00006B44
		public string Cid
		{
			get
			{
				return this.cid;
			}
			set
			{
				this.cid = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00008954 File Offset: 0x00006B54
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000896C File Offset: 0x00006B6C
		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000897C File Offset: 0x00006B7C
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00008994 File Offset: 0x00006B94
		public string SalesName
		{
			get
			{
				return this.salesName;
			}
			set
			{
				this.salesName = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000089A4 File Offset: 0x00006BA4
		// (set) Token: 0x060001BC RID: 444 RVA: 0x000089BC File Offset: 0x00006BBC
		public string SwVersion
		{
			get
			{
				return this.swVersion;
			}
			set
			{
				this.swVersion = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000089CC File Offset: 0x00006BCC
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000089E4 File Offset: 0x00006BE4
		public string AkVersion
		{
			get
			{
				return this.akVersion;
			}
			set
			{
				this.akVersion = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000089F4 File Offset: 0x00006BF4
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00008A0C File Offset: 0x00006C0C
		public string NewAkVersion
		{
			get
			{
				return this.newAkVersion;
			}
			set
			{
				this.newAkVersion = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00008A1C File Offset: 0x00006C1C
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00008A34 File Offset: 0x00006C34
		public string NewSwVersion
		{
			get
			{
				return this.newSwVersion;
			}
			set
			{
				this.newSwVersion = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00008A44 File Offset: 0x00006C44
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00008A5C File Offset: 0x00006C5C
		public string LocalPath
		{
			get
			{
				return this.localPath;
			}
			set
			{
				this.localPath = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00008A6C File Offset: 0x00006C6C
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008A84 File Offset: 0x00006C84
		public string DebugField
		{
			get
			{
				return this.debugField;
			}
			set
			{
				string text = Report.PrepareForCsvFormat(value);
				this.debugField = ((text.Length <= 2000) ? text : text.Substring(0, 2000));
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00008ABC File Offset: 0x00006CBC
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public string ApiError
		{
			get
			{
				return this.apiError;
			}
			set
			{
				this.apiError = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00008AE4 File Offset: 0x00006CE4
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00008AFC File Offset: 0x00006CFC
		public string ApiErrorMessage
		{
			get
			{
				return this.apiErrorMessage;
			}
			set
			{
				this.apiErrorMessage = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00008B0C File Offset: 0x00006D0C
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00008B24 File Offset: 0x00006D24
		public string ApplicationVersion
		{
			get
			{
				return this.applicationVersion;
			}
			set
			{
				this.applicationVersion = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00008B34 File Offset: 0x00006D34
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00008B4C File Offset: 0x00006D4C
		public string FirmwareGrading
		{
			get
			{
				return this.firmwareGrading;
			}
			set
			{
				this.firmwareGrading = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00008B5C File Offset: 0x00006D5C
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00008B74 File Offset: 0x00006D74
		public string ReportManufacturerName
		{
			get
			{
				return this.reportManufacturerName;
			}
			set
			{
				this.reportManufacturerName = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00008B84 File Offset: 0x00006D84
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00008B9C File Offset: 0x00006D9C
		public string ReportManufacturerProductLine
		{
			get
			{
				return this.reportManufacturerProductLine;
			}
			set
			{
				this.reportManufacturerProductLine = Report.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00008BAC File Offset: 0x00006DAC
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00008BC3 File Offset: 0x00006DC3
		public long DownloadDuration { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00008BCC File Offset: 0x00006DCC
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00008BE3 File Offset: 0x00006DE3
		public long UpdateDuration { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00008BEC File Offset: 0x00006DEC
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00008C03 File Offset: 0x00006E03
		public Guid SessionId { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00008C0C File Offset: 0x00006E0C
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00008C23 File Offset: 0x00006E23
		public UriData Uri { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008C2C File Offset: 0x00006E2C
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00008C43 File Offset: 0x00006E43
		public PhoneTypes PhoneType { get; set; }

		// Token: 0x060001DD RID: 477 RVA: 0x00008C4C File Offset: 0x00006E4C
		public string GetReportAsCsv()
		{
			List<string> values = new List<string>
			{
				this.SystemInfo,
				this.ActionDescription,
				this.ProductType,
				this.ProductCode,
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
				this.ReportManufacturerName,
				this.ReportManufacturerProductLine
			};
			return string.Join(";", values);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008E14 File Offset: 0x00007014
		public MsrReport ConvertToMsrReport()
		{
			return new MsrReport(string.Format("{0}-{1}", this.Imei, Guid.NewGuid()))
			{
				ActionDescription = this.ActionDescription,
				AkVersion = this.AkVersion,
				ApiError = this.ApiError,
				ApiErrorMessage = this.ApiErrorMessage,
				ApplicationName = string.Empty,
				ApplicationVendor = string.Empty,
				ApplicationVersion = this.ApplicationVersion,
				Cid = this.Cid,
				DebugField = this.DebugField,
				DownloadDuration = this.DownloadDuration,
				DownloadedBytes = this.DownloadedBytes,
				FirmwareGrading = this.FirmwareGrading,
				Imei = this.Imei,
				LastErrorData = string.Empty,
				LocalPath = this.LocalPath,
				Manufacturer = string.Empty,
				ManufacturerHardwareModel = this.ProductType,
				ManufacturerHardwareVariant = this.ProductCode,
				ManufacturerName = this.ReportManufacturerName,
				Mid = this.Mid,
				NewAkVersion = this.NewAkVersion,
				NewSwVersion = this.NewSwVersion,
				PackageId = string.Empty,
				PackageSizeOnServer = this.PackageSizeOnServer,
				PhoneType = this.PhoneType,
				Pid = this.Pid,
				PlatformId = string.Empty,
				ManufacturerProductLine = this.ReportManufacturerProductLine,
				ReportVersion = "1.1",
				ResumeCounter = this.ResumeCounter,
				SalesName = this.SalesName,
				SerialNumber = this.SerialNumber,
				SwVersion = this.SwVersion,
				SystemInfo = this.SystemInfo,
				TimeStamp = 0L,
				UpdateDuration = this.UpdateDuration,
				Uri = this.Uri,
				UserSiteLanguage = string.Empty
			};
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000902C File Offset: 0x0000722C
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

		// Token: 0x060001E0 RID: 480 RVA: 0x00009090 File Offset: 0x00007290
		private uint UriDataToUint(UriData uriData)
		{
			return (uint)uriData;
		}

		// Token: 0x040000C5 RID: 197
		private string actionDescription;

		// Token: 0x040000C6 RID: 198
		private string akVersion;

		// Token: 0x040000C7 RID: 199
		private string apiError;

		// Token: 0x040000C8 RID: 200
		private string apiErrorMessage;

		// Token: 0x040000C9 RID: 201
		private string applicationVersion;

		// Token: 0x040000CA RID: 202
		private string cid;

		// Token: 0x040000CB RID: 203
		private string debugField;

		// Token: 0x040000CC RID: 204
		private string firmwareGrading;

		// Token: 0x040000CD RID: 205
		private string imei;

		// Token: 0x040000CE RID: 206
		private string localPath;

		// Token: 0x040000CF RID: 207
		private string mid;

		// Token: 0x040000D0 RID: 208
		private string newAkVersion;

		// Token: 0x040000D1 RID: 209
		private string newSwVersion;

		// Token: 0x040000D2 RID: 210
		private string pid;

		// Token: 0x040000D3 RID: 211
		private string productCode;

		// Token: 0x040000D4 RID: 212
		private string productType;

		// Token: 0x040000D5 RID: 213
		private string salesName;

		// Token: 0x040000D6 RID: 214
		private string serialNumber;

		// Token: 0x040000D7 RID: 215
		private string systemInfo;

		// Token: 0x040000D8 RID: 216
		private string vid;

		// Token: 0x040000D9 RID: 217
		private string reportManufacturerName;

		// Token: 0x040000DA RID: 218
		private string reportManufacturerProductLine;

		// Token: 0x040000DB RID: 219
		private string downloadedBytes;

		// Token: 0x040000DC RID: 220
		private string packageSizeOnServer;

		// Token: 0x040000DD RID: 221
		private string resumeCounter;

		// Token: 0x040000DE RID: 222
		private string swVersion;
	}
}
