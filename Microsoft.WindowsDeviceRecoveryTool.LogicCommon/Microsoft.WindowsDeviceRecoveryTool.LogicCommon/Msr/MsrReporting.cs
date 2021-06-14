using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr
{
	// Token: 0x0200001A RID: 26
	public sealed class MsrReporting
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000F3 RID: 243 RVA: 0x000061F0 File Offset: 0x000043F0
		// (remove) Token: 0x060000F4 RID: 244 RVA: 0x0000622C File Offset: 0x0000442C
		public event Action<ReportSendCompletedEventArgs> SendCompleted;

		// Token: 0x060000F5 RID: 245 RVA: 0x00006268 File Offset: 0x00004468
		public MsrReporting(MsrReportingService msrReportingService)
		{
			this.environmentInfo = new EnvironmentInfo(new ApplicationInfo());
			this.msrReportingService = msrReportingService;
			this.actionQueue = new Queue<ReportStatusAsyncState>();
			this.processingThreadStarted = false;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000629C File Offset: 0x0000449C
		public void SendAsync(IReport report)
		{
			ReportStatusAsyncState asyncState = new ReportStatusAsyncState(ReportingOperation.Send, report, null);
			this.AddToReportQueue(asyncState);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000062BC File Offset: 0x000044BC
		private void AddToReportQueue(ReportStatusAsyncState asyncState)
		{
			lock (((ICollection)this.actionQueue).SyncRoot)
			{
				this.actionQueue.Enqueue(asyncState);
				if (!this.processingThreadStarted)
				{
					this.processingThreadStarted = true;
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessReportQueueWork));
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006338 File Offset: 0x00004538
		private void ProcessReportQueueWork(object obj)
		{
			do
			{
				ReportStatusAsyncState reportStatusAsyncState;
				lock (((ICollection)this.actionQueue).SyncRoot)
				{
					if (this.actionQueue.Count <= 0)
					{
						this.processingThreadStarted = false;
						break;
					}
					reportStatusAsyncState = this.actionQueue.Dequeue();
				}
				if (reportStatusAsyncState.ReportingOperation == ReportingOperation.Send)
				{
					this.HandleSendRequest(reportStatusAsyncState);
				}
			}
			while (this.processingThreadStarted);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000063DC File Offset: 0x000045DC
		private void HandleSendRequest(ReportStatusAsyncState statusAsyncState)
		{
			Exception error = null;
			ReportUpdateStatus4Parameters parameters = null;
			try
			{
				parameters = statusAsyncState.Report.CreateReportStatusParameters();
				this.msrReportingService.SendReportAsync(statusAsyncState.Report).Wait();
			}
			catch (Exception ex)
			{
				error = ex;
			}
			finally
			{
				this.OnSendCompleted(new ReportSendCompletedEventArgs(parameters, statusAsyncState.Report, error, null));
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006454 File Offset: 0x00004654
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

		// Token: 0x060000FB RID: 251 RVA: 0x00006484 File Offset: 0x00004684
		private string Truncate(string source, int length)
		{
			if (source.Length > length)
			{
				source = source.Substring(0, length);
			}
			return source;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000064B4 File Offset: 0x000046B4
		private void OnSendCompleted(ReportSendCompletedEventArgs e)
		{
			Action<ReportSendCompletedEventArgs> sendCompleted = this.SendCompleted;
			if (sendCompleted != null)
			{
				sendCompleted(e);
			}
		}

		// Token: 0x0400007F RID: 127
		private readonly Queue<ReportStatusAsyncState> actionQueue;

		// Token: 0x04000080 RID: 128
		private MsrReportingService msrReportingService;

		// Token: 0x04000081 RID: 129
		private bool processingThreadStarted;

		// Token: 0x04000082 RID: 130
		private readonly IEnvironmentInfo environmentInfo;
	}
}
