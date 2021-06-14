using System;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces
{
	// Token: 0x02000024 RID: 36
	public interface IReport
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000128 RID: 296
		string SessionId { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000129 RID: 297
		// (set) Token: 0x0600012A RID: 298
		string LocalPath { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600012B RID: 299
		// (set) Token: 0x0600012C RID: 300
		string ManufacturerName { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600012D RID: 301
		// (set) Token: 0x0600012E RID: 302
		string ManufacturerProductLine { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600012F RID: 303
		// (set) Token: 0x06000130 RID: 304
		string ManufacturerHardwareModel { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000131 RID: 305
		// (set) Token: 0x06000132 RID: 306
		string ManufacturerHardwareVariant { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000133 RID: 307
		// (set) Token: 0x06000134 RID: 308
		string Imei { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000135 RID: 309
		// (set) Token: 0x06000136 RID: 310
		string ActionDescription { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000137 RID: 311
		PhoneTypes PhoneType { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000138 RID: 312
		bool Sent { get; }

		// Token: 0x06000139 RID: 313
		void MarkAsSent();

		// Token: 0x0600013A RID: 314
		string GetReportAsXml();

		// Token: 0x0600013B RID: 315
		string GetReportAsCsv();

		// Token: 0x0600013C RID: 316
		ReportUpdateStatus4Parameters CreateReportStatusParameters();
	}
}
