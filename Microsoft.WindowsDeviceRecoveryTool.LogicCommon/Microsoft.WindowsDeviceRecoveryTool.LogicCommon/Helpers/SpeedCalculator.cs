using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000C RID: 12
	public class SpeedCalculator
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002E7C File Offset: 0x0000107C
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002E93 File Offset: 0x00001093
		public long TotalFilesSize { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002E9C File Offset: 0x0000109C
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002EB3 File Offset: 0x000010B3
		public long PreviousDownloadedSize { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002EBC File Offset: 0x000010BC
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002ED3 File Offset: 0x000010D3
		public long CurrentDownloadedSize { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002EDC File Offset: 0x000010DC
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002EF3 File Offset: 0x000010F3
		public long CurrentPartlyDownloadedSize { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002EFC File Offset: 0x000010FC
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002F13 File Offset: 0x00001113
		public bool IsResumed { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002F1C File Offset: 0x0000111C
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002F33 File Offset: 0x00001133
		public bool IsDownloadStarted { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002F3C File Offset: 0x0000113C
		public long TotalDownloadedSize
		{
			get
			{
				return this.PreviousDownloadedSize + this.CurrentDownloadedSize + this.CurrentPartlyDownloadedSize;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002F64 File Offset: 0x00001164
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00002F7B File Offset: 0x0000117B
		public long RemaingSeconds { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002F84 File Offset: 0x00001184
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00002F9B File Offset: 0x0000119B
		public double BytesPerSecond { get; private set; }

		// Token: 0x06000083 RID: 131 RVA: 0x00002FA4 File Offset: 0x000011A4
		public void Start(long totalFilesSize, long previousDownloadedSize = 0L)
		{
			this.Reset();
			this.packetsInOneSecond = new Queue<long>();
			this.TotalFilesSize = totalFilesSize;
			this.PreviousDownloadedSize = previousDownloadedSize;
			this.oneSecondTick = new Timer
			{
				Interval = 1000.0
			};
			this.queueCapacity = 100;
			this.smoothFactor = 0.0005;
			this.oneSecondTick.Start();
			this.oneSecondTick.Elapsed += this.OneSecondTickElapsed;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000302C File Offset: 0x0000122C
		public void Stop()
		{
			if (this.oneSecondTick != null)
			{
				this.oneSecondTick.Stop();
				this.oneSecondTick.Elapsed -= this.OneSecondTickElapsed;
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003070 File Offset: 0x00001270
		public void Reset()
		{
			this.Stop();
			this.PreviousDownloadedSize = 0L;
			this.CurrentDownloadedSize = 0L;
			this.CurrentPartlyDownloadedSize = 0L;
			this.TotalFilesSize = 0L;
			this.RemaingSeconds = 0L;
			this.BytesPerSecond = 0.0;
			this.IsDownloadStarted = false;
			if (this.packetsInOneSecond != null)
			{
				this.packetsInOneSecond.Clear();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000030E8 File Offset: 0x000012E8
		private void OneSecondTickElapsed(object sender, ElapsedEventArgs e)
		{
			this.packetsInOneSecond.Enqueue(this.CurrentDownloadedSize);
			if (this.packetsInOneSecond.Count > 5)
			{
				long num = (this.packetsInOneSecond.Count > this.queueCapacity) ? this.packetsInOneSecond.Dequeue() : this.packetsInOneSecond.First<long>();
				this.BytesPerSecond = this.smoothFactor * (double)(this.packetsInOneSecond.Last<long>() - this.packetsInOneSecond.ElementAt(this.packetsInOneSecond.Count - 2)) / 2.0 + (1.0 - this.smoothFactor) * (double)(this.packetsInOneSecond.Last<long>() - num) / (double)this.packetsInOneSecond.Count;
				if (this.BytesPerSecond > 1.0)
				{
					long num2 = this.TotalFilesSize - this.TotalDownloadedSize;
					this.RemaingSeconds = (long)((double)num2 / this.BytesPerSecond);
				}
			}
		}

		// Token: 0x0400001B RID: 27
		private Timer oneSecondTick;

		// Token: 0x0400001C RID: 28
		private int queueCapacity;

		// Token: 0x0400001D RID: 29
		private double smoothFactor;

		// Token: 0x0400001E RID: 30
		private Queue<long> packetsInOneSecond;
	}
}
