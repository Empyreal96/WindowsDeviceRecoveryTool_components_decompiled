using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003F RID: 63
	[Export(typeof(ReportingService))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ReportingService
	{
		// Token: 0x06000357 RID: 855 RVA: 0x0000FEC4 File Offset: 0x0000E0C4
		[ImportingConstructor]
		public ReportingService(MsrReportingService msrReportingService)
		{
			this.msrReportingService = msrReportingService;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		public MsrReportingService MsrReportingService
		{
			get
			{
				return this.msrReportingService;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public void SetDownloadByteInformation(Phone phone, ReportOperationType reportOperationType, long currentDownloadedSize, long packageSize, bool isResumed)
		{
			this.msrReportingService.SetDownloadByteInformation(phone, reportOperationType, currentDownloadedSize, packageSize, isResumed);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000FF06 File Offset: 0x0000E106
		public void OperationStarted(Phone phone, ReportOperationType reportOperationType)
		{
			this.msrReportingService.OperationStarted(phone, reportOperationType);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000FF17 File Offset: 0x0000E117
		public void OperationSucceded(Phone phone, ReportOperationType reportOperationType)
		{
			this.msrReportingService.OperationSucceded(phone, reportOperationType);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000FF28 File Offset: 0x0000E128
		public void PartialOperationSucceded(Phone phone, ReportOperationType reportOperationType, UriData uriData)
		{
			this.msrReportingService.PartialOperationSucceded(phone, reportOperationType, uriData);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000FF3A File Offset: 0x0000E13A
		public void OperationFailed(Phone phone, ReportOperationType reportOperationType, UriData resultUriData, Exception ex)
		{
			this.msrReportingService.OperationFailed(phone, reportOperationType, resultUriData, ex);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000FF4E File Offset: 0x0000E14E
		public void SendSessionReports()
		{
			this.msrReportingService.SendSessionReports();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000FF5D File Offset: 0x0000E15D
		public void SurveySucceded(SurveyReport survey)
		{
			this.msrReportingService.SurveySucceded(survey);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FF6D File Offset: 0x0000E16D
		public void StartFlowSession()
		{
			this.msrReportingService.StartFlowSession();
		}

		// Token: 0x04000194 RID: 404
		private readonly MsrReportingService msrReportingService;
	}
}
