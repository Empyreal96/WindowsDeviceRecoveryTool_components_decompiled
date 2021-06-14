using System;
using System.ComponentModel;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x0200002E RID: 46
	public sealed class ReportSendCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00009C48 File Offset: 0x00007E48
		public ReportSendCompletedEventArgs(IReport report, Exception error, object userState) : base(error, false, userState)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}
			this.Report = report;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009C80 File Offset: 0x00007E80
		public ReportSendCompletedEventArgs(ReportUpdateStatus4Parameters parameters, IReport report, Exception error, object userState = null) : base(error, false, userState)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}
			this.Parameters = parameters;
			this.Report = report;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00009CC4 File Offset: 0x00007EC4
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00009CDB File Offset: 0x00007EDB
		public IReport Report { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00009CE4 File Offset: 0x00007EE4
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00009CFB File Offset: 0x00007EFB
		public ReportUpdateStatus4Parameters Parameters { get; private set; }
	}
}
