using System;
using System.Diagnostics;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x0200002C RID: 44
	public class ReportData
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x0000989A File Offset: 0x00007A9A
		public ReportData(string description, string sessionId)
		{
			this.Description = description;
			this.ConnectionName = string.Empty;
			this.SystemInfo = Environment.OSVersion.ToString();
			this.SessionId = sessionId;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000098D4 File Offset: 0x00007AD4
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x000098EB File Offset: 0x00007AEB
		public Exception Exception { get; private set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000098F4 File Offset: 0x00007AF4
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000990B File Offset: 0x00007B0B
		public UriData UriData { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009914 File Offset: 0x00007B14
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000992B File Offset: 0x00007B2B
		public Phone PhoneInfo { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00009934 File Offset: 0x00007B34
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000994B File Offset: 0x00007B4B
		public long DownloadDuration { get; private set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00009954 File Offset: 0x00007B54
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000996B File Offset: 0x00007B6B
		public long UpdateDuration { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00009974 File Offset: 0x00007B74
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000998B File Offset: 0x00007B8B
		public string ConnectionName { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00009994 File Offset: 0x00007B94
		// (set) Token: 0x06000203 RID: 515 RVA: 0x000099AB File Offset: 0x00007BAB
		public string Description { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000099B4 File Offset: 0x00007BB4
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000099CB File Offset: 0x00007BCB
		public string SystemInfo { get; private set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000099D4 File Offset: 0x00007BD4
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000099EB File Offset: 0x00007BEB
		public string LocalPath { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000099F4 File Offset: 0x00007BF4
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00009A0B File Offset: 0x00007C0B
		public long PackageSize { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00009A14 File Offset: 0x00007C14
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00009A2B File Offset: 0x00007C2B
		public int ResumeCounter { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00009A34 File Offset: 0x00007C34
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00009A4B File Offset: 0x00007C4B
		public long DownloadedBytes { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00009A54 File Offset: 0x00007C54
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00009A6B File Offset: 0x00007C6B
		public string SessionId { get; private set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00009A74 File Offset: 0x00007C74
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00009A8B File Offset: 0x00007C8B
		public Exception LastError { get; set; }

		// Token: 0x06000212 RID: 530 RVA: 0x00009A94 File Offset: 0x00007C94
		public void SetPhoneInfo(Phone phone)
		{
			this.PhoneInfo = phone;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009AA0 File Offset: 0x00007CA0
		public void StartUpdateTimer()
		{
			if (this.updateTimer == null)
			{
				this.updateTimer = new Stopwatch();
			}
			if (this.updateTimer.IsRunning || this.updateTimer.ElapsedMilliseconds != 0L)
			{
				this.updateTimer.Restart();
			}
			else
			{
				this.updateTimer.Start();
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009B10 File Offset: 0x00007D10
		public void StopUpdateTimer()
		{
			if (this.updateTimer != null)
			{
				this.updateTimer.Stop();
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009B3C File Offset: 0x00007D3C
		public void StartDownloadTimer()
		{
			if (this.downloadTimer == null)
			{
				this.downloadTimer = new Stopwatch();
			}
			if (this.downloadTimer.IsRunning || this.downloadTimer.ElapsedMilliseconds != 0L)
			{
				this.downloadTimer.Restart();
			}
			else
			{
				this.downloadTimer.Start();
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009BAC File Offset: 0x00007DAC
		public void StopDownloadTimer()
		{
			if (this.downloadTimer != null)
			{
				this.downloadTimer.Stop();
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00009BD5 File Offset: 0x00007DD5
		public void SetResult(UriData uriData, Exception exception)
		{
			this.UriData = uriData;
			this.Exception = exception;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00009BE8 File Offset: 0x00007DE8
		public void EndDataCollecting()
		{
			this.StopUpdateTimer();
			this.StopDownloadTimer();
			if (this.updateTimer != null)
			{
				this.UpdateDuration = this.updateTimer.ElapsedMilliseconds;
			}
			if (this.downloadTimer != null)
			{
				this.DownloadDuration = this.downloadTimer.ElapsedMilliseconds;
			}
		}

		// Token: 0x040000FA RID: 250
		private Stopwatch updateTimer;

		// Token: 0x040000FB RID: 251
		private Stopwatch downloadTimer;
	}
}
