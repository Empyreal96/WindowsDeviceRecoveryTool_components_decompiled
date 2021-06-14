using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Interfaces;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr
{
	// Token: 0x0200001B RID: 27
	public sealed class MsrReportSender
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000FD RID: 253 RVA: 0x000064DC File Offset: 0x000046DC
		// (remove) Token: 0x060000FE RID: 254 RVA: 0x00006518 File Offset: 0x00004718
		public event Action SessionReportsSendingCompleted;

		// Token: 0x060000FF RID: 255 RVA: 0x00006554 File Offset: 0x00004754
		public MsrReportSender(MsrReporting msrReporting)
		{
			if (msrReporting == null)
			{
				throw new ArgumentNullException("msrReporting");
			}
			this.ioHelper = new IOHelper();
			this.workerHelper = new WorkerHelper(new DoWorkEventHandler(this.SendOldReports));
			this.msrReporting = msrReporting;
			this.msrReporting.SendCompleted += this.MsrReportSendCompleted;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000065C4 File Offset: 0x000047C4
		public void SendReport(ReportData reportData, bool isInternal)
		{
			Tracer<MsrReportSender>.WriteInformation("Sending report {0} | {1}", new object[]
			{
				reportData.Description,
				reportData.UriData
			});
			try
			{
				bool flag = false;
				MsrReport report = ReportBuilder.Build(reportData, isInternal);
				if (!flag)
				{
					this.msrReporting.SendAsync(report);
				}
			}
			catch (Exception error)
			{
				Tracer<MsrReportSender>.WriteError(error, "Sending report failed.", new object[0]);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006648 File Offset: 0x00004848
		public void SendReport(SurveyReport report, bool isInternal)
		{
			Tracer<MsrReportSender>.WriteInformation("Sending report {0} | {1}", new object[]
			{
				report.ManufacturerHardwareModel,
				report.ManufacturerHardwareVariant
			});
			try
			{
				if (!false)
				{
					this.msrReporting.SendAsync(report);
				}
			}
			catch (Exception error)
			{
				Tracer<MsrReportSender>.WriteError(error, "Sending report failed.", new object[0]);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000066C0 File Offset: 0x000048C0
		public void SaveLocalReport(ReportData reportData)
		{
			Tracer<MsrReportSender>.WriteInformation("Saving report locally: {0}", new object[]
			{
				reportData.PhoneInfo
			});
			try
			{
				MsrReport report = ReportBuilder.Build(reportData, ApplicationInfo.IsInternal());
				if (string.IsNullOrWhiteSpace(reportData.LocalPath))
				{
					reportData.LocalPath = this.BuildReportFilename("Not sent\\", reportData.PhoneInfo.Imei, reportData.Description, ReportFileType.Binary);
				}
				this.ioHelper.SerializeReport(reportData.LocalPath, report);
			}
			catch (Exception error)
			{
				Tracer<MsrReportSender>.WriteError(error, "Saving local report failed.", new object[0]);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006770 File Offset: 0x00004970
		public void SendOldReports()
		{
			if (!this.workerHelper.IsBusy)
			{
				this.workerHelper.RunWorkerAsync();
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000679C File Offset: 0x0000499C
		public void RemoveLocalReport(string localPath)
		{
			if (!string.IsNullOrWhiteSpace(localPath))
			{
				this.ioHelper.DeleteFile(localPath);
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000067C4 File Offset: 0x000049C4
		public void SendSessionReports(List<ReportData> reportDataList, bool isInternal)
		{
			this.sendingReportQueue = new List<IReport>();
			lock (this.sendingReportQueue)
			{
				try
				{
					foreach (ReportData reportData in reportDataList)
					{
						reportData.EndDataCollecting();
						bool flag2 = false;
						MsrReport msrReport = ReportBuilder.Build(reportData, isInternal);
						if (!flag2)
						{
							this.sendingReportQueue.Add(msrReport);
							this.msrReporting.SendAsync(msrReport);
						}
					}
				}
				catch (Exception error)
				{
					Tracer<MsrReportSender>.WriteError(error, "Sending report failed.", new object[0]);
				}
			}
			this.TrySendSessionReportsCompletedEvent();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000068C0 File Offset: 0x00004AC0
		private void SaveSentReport(ReportSendCompletedEventArgs e)
		{
			string reportPath = this.BuildReportFilename("Sent\\", e.Parameters.Ext1.Replace(":", string.Empty).Replace(" ", string.Empty), e.Parameters.Ext2, ReportFileType.Xml);
			this.ioHelper.SaveReport(reportPath, e.Parameters);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006924 File Offset: 0x00004B24
		private void SaveNotSentReport(ReportSendCompletedEventArgs e)
		{
			IReport report = e.Report;
			if (string.IsNullOrWhiteSpace(report.LocalPath))
			{
				report.LocalPath = this.BuildReportFilename("Not sent\\", report.Imei, report.ActionDescription, ReportFileType.Binary);
			}
			this.ioHelper.SerializeReport(report.LocalPath, report);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006980 File Offset: 0x00004B80
		private void SendOldReports(object sender, DoWorkEventArgs e)
		{
			string text = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Reports) + "Not sent\\";
			this.ioHelper.CreateDir(text);
			string[] msrFiles = this.ioHelper.GetMsrFiles(text);
			string[] array = msrFiles;
			int i = 0;
			while (i < array.Length)
			{
				string text2 = array[i];
				IReport report;
				try
				{
					report = this.ioHelper.DeserializeReport(text2);
				}
				catch (Exception)
				{
					try
					{
						Report report2 = this.ioHelper.DeserializeFireReport(text2);
						report = report2.ConvertToMsrReport();
					}
					catch (Exception error)
					{
						Tracer<MsrReportSender>.WriteError(error, "Old report deserialization failed", new object[0]);
						this.ioHelper.DeleteFile(text2);
						goto IL_A1;
					}
				}
				goto IL_92;
				IL_A1:
				i++;
				continue;
				IL_92:
				this.msrReporting.SendAsync(report);
				goto IL_A1;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006A64 File Offset: 0x00004C64
		[Conditional("DEBUG")]
		private void SaveInternalReport(IReport report, ref bool handled)
		{
			Tracer<MsrReportSender>.LogEntry("SaveInternalReport");
			try
			{
				string reportPath = this.BuildReportFilename("Internal\\", report.Imei, report.ActionDescription, ReportFileType.Csv);
				this.ioHelper.SaveReportAsCsv(reportPath, report);
				this.RemoveLocalReport(report.LocalPath);
				handled = true;
			}
			catch (IOException)
			{
			}
			Tracer<MsrReportSender>.LogExit("SaveInternalReport");
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006ADC File Offset: 0x00004CDC
		private void MsrReportSendCompleted(ReportSendCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				try
				{
					this.SaveNotSentReport(e);
				}
				catch (Exception ex)
				{
					Tracer<MsrReportSender>.WriteInformation("SaveNotSentReport failed: {0}", new object[]
					{
						ex.Message
					});
				}
			}
			else
			{
				try
				{
					Tracer<MsrReportSender>.WriteInformation("Msr report was sent successfully.");
					this.SaveSentReport(e);
					this.RemoveLocalReport(e.Report.LocalPath);
				}
				catch (Exception ex)
				{
					Tracer<MsrReportSender>.WriteInformation("SaveSentReport failed: {0}", new object[]
					{
						ex.Message
					});
				}
			}
			if (this.sendingReportQueue != null && this.sendingReportQueue.Contains(e.Report))
			{
				lock (this.sendingReportQueue)
				{
					this.sendingReportQueue.Remove(e.Report);
				}
				this.TrySendSessionReportsCompletedEvent();
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006C08 File Offset: 0x00004E08
		private void TrySendSessionReportsCompletedEvent()
		{
			if (this.sendingReportQueue != null && !this.sendingReportQueue.Any<IReport>())
			{
				Action sessionReportsSendingCompleted = this.SessionReportsSendingCompleted;
				if (sessionReportsSendingCompleted != null)
				{
					sessionReportsSendingCompleted();
				}
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006C50 File Offset: 0x00004E50
		private string BuildReportFilename(string subDirectory, string imei, string reportDescription, ReportFileType reportFileType)
		{
			string text = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Reports) + subDirectory;
			this.ioHelper.CreateDir(text);
			DateTime utcNow = DateTime.UtcNow;
			string reportFileExtension = this.ioHelper.GetReportFileExtension(reportFileType);
			string str = string.Format("msr_{0:yyyyMMdd}_{0:HHmmss_ff}_{1}_{2}.{3}", new object[]
			{
				utcNow,
				imei,
				reportDescription,
				reportFileExtension
			});
			return text + str;
		}

		// Token: 0x04000084 RID: 132
		private MsrReporting msrReporting;

		// Token: 0x04000085 RID: 133
		private WorkerHelper workerHelper;

		// Token: 0x04000086 RID: 134
		private IOHelper ioHelper;

		// Token: 0x04000087 RID: 135
		private List<IReport> sendingReportQueue;
	}
}
