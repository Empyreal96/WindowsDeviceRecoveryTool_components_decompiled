using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000031 RID: 49
	[Serializable]
	public class SurveyReport : IReport
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000A39A File Offset: 0x0000859A
		public SurveyReport()
		{
			this.ActionDescription = "Survey";
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000A3B4 File Offset: 0x000085B4
		// (set) Token: 0x06000283 RID: 643 RVA: 0x0000A3CB File Offset: 0x000085CB
		public string SessionId { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000A3D4 File Offset: 0x000085D4
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000A3EB File Offset: 0x000085EB
		public string LocalPath { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000A3F4 File Offset: 0x000085F4
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000A40B File Offset: 0x0000860B
		public bool Question1 { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000A414 File Offset: 0x00008614
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000A42B File Offset: 0x0000862B
		public bool Question2 { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000A434 File Offset: 0x00008634
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000A44B File Offset: 0x0000864B
		public bool Question3 { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000A454 File Offset: 0x00008654
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000A46B File Offset: 0x0000866B
		public bool Question4 { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000A474 File Offset: 0x00008674
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000A48B File Offset: 0x0000868B
		public bool Question5 { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000A494 File Offset: 0x00008694
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000A4AC File Offset: 0x000086AC
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A4BC File Offset: 0x000086BC
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000A4D3 File Offset: 0x000086D3
		public bool InsiderProgramQuestion { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000A4DC File Offset: 0x000086DC
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000A4F4 File Offset: 0x000086F4
		public string ManufacturerHardwareModel
		{
			get
			{
				return this.productType;
			}
			set
			{
				this.productType = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000A504 File Offset: 0x00008704
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000A51C File Offset: 0x0000871C
		public string ManufacturerHardwareVariant
		{
			get
			{
				return this.productCode;
			}
			set
			{
				this.productCode = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000A52C File Offset: 0x0000872C
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000A544 File Offset: 0x00008744
		public string Imei
		{
			get
			{
				return this.imei;
			}
			set
			{
				this.imei = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000A554 File Offset: 0x00008754
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000A56C File Offset: 0x0000876C
		public string ManufacturerName
		{
			get
			{
				return this.surveyManufacturerName;
			}
			set
			{
				this.surveyManufacturerName = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000A57C File Offset: 0x0000877C
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000A594 File Offset: 0x00008794
		public string ManufacturerProductLine
		{
			get
			{
				return this.surveyManufacturerProductLine;
			}
			set
			{
				this.surveyManufacturerProductLine = SurveyReport.PrepareForCsvFormat(value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000A5A4 File Offset: 0x000087A4
		// (set) Token: 0x0600029F RID: 671 RVA: 0x0000A5BB File Offset: 0x000087BB
		public string ActionDescription { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000A5C4 File Offset: 0x000087C4
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000A5DB File Offset: 0x000087DB
		public PhoneTypes PhoneType { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000A5E4 File Offset: 0x000087E4
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000A5FB File Offset: 0x000087FB
		public bool Sent { get; private set; }

		// Token: 0x060002A4 RID: 676 RVA: 0x0000A604 File Offset: 0x00008804
		public void MarkAsSent()
		{
			this.Sent = true;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000A610 File Offset: 0x00008810
		public string GetReportAsXml()
		{
			XDocument xdocument = new XDocument(new XDeclaration("1.0", "utf-8", "no"), new object[]
			{
				new XElement("Survey", new object[]
				{
					new XElement("reportSessionId", this.SessionId),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q1"),
						new XAttribute("id", "q1_1"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Header1")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_AppsNotWorking1")),
						this.Question1
					}),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q1"),
						new XAttribute("id", "q1_2"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Header1")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_PerformanceIssues1")),
						this.Question2
					}),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q1"),
						new XAttribute("id", "q1_3"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Header1")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_PrevVersionFaster1")),
						this.Question3
					}),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q1"),
						new XAttribute("id", "q1_4"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Header1")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_PrevVersionMoreReliable1")),
						this.Question4
					}),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q1"),
						new XAttribute("id", "q1_5"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Header1")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_DeviceNotWorking1")),
						this.Question5
					}),
					new XElement("surveyOpenAnswer", new object[]
					{
						new XAttribute("questionId", "tellUsMore"),
						new XAttribute("id", "tellUsMore_userMessage"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_TellUsMore1")),
						new XAttribute("answerContent", "q1"),
						this.Description
					}),
					new XElement("surveyCheckAnswer", new object[]
					{
						new XAttribute("questionId", "q2"),
						new XAttribute("id", "q2_insiderProgram"),
						new XAttribute("questionContent", SurveyReport.ReadEnglishResource("Survey_Choice_MoreAbout_QuestionContent")),
						new XAttribute("answerContent", SurveyReport.ReadEnglishResource("Survey_Choice_InsiderProgram_AnswerContent")),
						this.InsiderProgramQuestion
					})
				})
			});
			return xdocument.ToString();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AA88 File Offset: 0x00008C88
		public string GetReportAsCsv()
		{
			List<string> values = new List<string>
			{
				this.Question1.ToString(),
				this.Question2.ToString(),
				this.Question3.ToString(),
				this.Question4.ToString(),
				this.Question5.ToString(),
				this.Description,
				this.ManufacturerHardwareModel,
				this.ManufacturerHardwareVariant,
				this.Imei,
				this.ManufacturerName,
				this.ManufacturerProductLine
			};
			return string.Join(";", values);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000AB64 File Offset: 0x00008D64
		public ReportUpdateStatus4Parameters CreateReportStatusParameters()
		{
			string text = this.FormatString(this.Imei, 100);
			return new ReportUpdateStatus4Parameters
			{
				SystemInfo = string.Empty,
				ActionDescription = this.FormatString(this.ActionDescription, 200),
				Uri = 0L,
				UriDescription = string.Empty,
				ApplicationName = string.Empty,
				ApplicationVendorName = "Microsoft",
				ApplicationVersion = string.Empty,
				ProductType = this.FormatString(this.ManufacturerHardwareModel, 100),
				ProductCode = this.FormatString(this.ManufacturerHardwareVariant, 100),
				Imei = text,
				FirmwareVersionOld = string.Empty,
				FirmwareVersionNew = string.Empty,
				FwGrading = string.Empty,
				Duration = 0L,
				DownloadDuration = 0L,
				UpdateDuration = 0L,
				ApiError = string.Empty,
				ApiErrorText = string.Empty,
				TimeStamp = TimeStampUtility.CreateTimeStamp(),
				Ext1 = this.FormatString(this.ActionDescription, 200),
				Ext2 = string.Empty,
				Ext3 = string.Empty,
				Ext4 = string.Format("{0}|{1}|{2}|{3}|{4}", new object[]
				{
					this.Question1,
					this.Question2,
					this.Question3,
					this.Question4,
					this.Question5
				}),
				Ext7 = this.FormatString(this.Description, 200),
				Ext8 = ApplicationInfo.CurrentLanguageInRegistry.EnglishName
			};
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AD40 File Offset: 0x00008F40
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

		// Token: 0x060002A9 RID: 681 RVA: 0x0000ADA4 File Offset: 0x00008FA4
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

		// Token: 0x060002AA RID: 682 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		private string Truncate(string source, int length)
		{
			if (source.Length > length)
			{
				source = source.Substring(0, length);
			}
			return source;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AE04 File Offset: 0x00009004
		private static string ReadEnglishResource(string resourceKey)
		{
			return LocalizationManager.Instance().EnglishResource(resourceKey) as string;
		}

		// Token: 0x04000147 RID: 327
		private const string Survey = "Survey";

		// Token: 0x04000148 RID: 328
		private string description;

		// Token: 0x04000149 RID: 329
		private string productType;

		// Token: 0x0400014A RID: 330
		private string productCode;

		// Token: 0x0400014B RID: 331
		private string imei;

		// Token: 0x0400014C RID: 332
		private string surveyManufacturerName;

		// Token: 0x0400014D RID: 333
		private string surveyManufacturerProductLine;
	}
}
