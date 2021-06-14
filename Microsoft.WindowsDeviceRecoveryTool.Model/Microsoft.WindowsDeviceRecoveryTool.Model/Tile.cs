using System;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200001B RID: 27
	public class Tile : NotificationObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000BC RID: 188 RVA: 0x000033E4 File Offset: 0x000015E4
		// (remove) Token: 0x060000BD RID: 189 RVA: 0x000033F4 File Offset: 0x000015F4
		public event EventHandler OnRemoveTimerElapsed
		{
			add
			{
				this.removeTimer.Tick += value;
			}
			remove
			{
				this.removeTimer.Stop();
				this.removeTimer.Tick -= value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003410 File Offset: 0x00001610
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003428 File Offset: 0x00001628
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				base.SetValue<string>(() => this.Title, ref this.title, value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003478 File Offset: 0x00001678
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000348F File Offset: 0x0000168F
		public PhoneTypes PhoneType { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003498 File Offset: 0x00001698
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000034B0 File Offset: 0x000016B0
		public ImageSource Image
		{
			get
			{
				return this.image;
			}
			set
			{
				base.SetValue<ImageSource>(() => this.Image, ref this.image, value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00003500 File Offset: 0x00001700
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00003517 File Offset: 0x00001717
		public Phone Phone { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003520 File Offset: 0x00001720
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00003538 File Offset: 0x00001738
		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				base.SetValue<bool>(() => this.IsEnabled, ref this.isEnabled, value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003588 File Offset: 0x00001788
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000035A0 File Offset: 0x000017A0
		public bool IsWaiting
		{
			get
			{
				return this.isWaiting;
			}
			set
			{
				base.SetValue<bool>(() => this.IsWaiting, ref this.isWaiting, value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000035F0 File Offset: 0x000017F0
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003607 File Offset: 0x00001807
		public bool ShowStartAnimation { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003610 File Offset: 0x00001810
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00003628 File Offset: 0x00001828
		public bool IsDeleted
		{
			get
			{
				return this.isDeleted;
			}
			set
			{
				base.SetValue<bool>(() => this.IsDeleted, ref this.isDeleted, value);
				this.StartRemoveTimer();
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003680 File Offset: 0x00001880
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003697 File Offset: 0x00001897
		public string DevicePath { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000036A0 File Offset: 0x000018A0
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x000036B7 File Offset: 0x000018B7
		public Guid SupportId { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000036C0 File Offset: 0x000018C0
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x000036D7 File Offset: 0x000018D7
		public object BasicDeviceInformation { get; set; }

		// Token: 0x060000D4 RID: 212 RVA: 0x000036E0 File Offset: 0x000018E0
		private void StartRemoveTimer()
		{
			this.removeTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
			this.removeTimer.Start();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000370C File Offset: 0x0000190C
		public override string ToString()
		{
			return this.Title;
		}

		// Token: 0x04000089 RID: 137
		private readonly DispatcherTimer removeTimer = new DispatcherTimer();

		// Token: 0x0400008A RID: 138
		private bool isDeleted;

		// Token: 0x0400008B RID: 139
		private string title;

		// Token: 0x0400008C RID: 140
		private ImageSource image;

		// Token: 0x0400008D RID: 141
		private bool isEnabled;

		// Token: 0x0400008E RID: 142
		private bool isWaiting;
	}
}
