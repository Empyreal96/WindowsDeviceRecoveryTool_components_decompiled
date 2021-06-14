using System;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000028 RID: 40
	public sealed class ReportStatusAsyncState
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x000090AD File Offset: 0x000072AD
		public ReportStatusAsyncState(ReportingOperation operation, IReport report = null, object userState = null)
		{
			this.ReportingOperation = operation;
			this.Report = report;
			this.UserState = userState;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000090D0 File Offset: 0x000072D0
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x000090E7 File Offset: 0x000072E7
		public ReportingOperation ReportingOperation { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000090F0 File Offset: 0x000072F0
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00009107 File Offset: 0x00007307
		public IReport Report { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00009110 File Offset: 0x00007310
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00009127 File Offset: 0x00007327
		public object UserState { get; private set; }
	}
}
