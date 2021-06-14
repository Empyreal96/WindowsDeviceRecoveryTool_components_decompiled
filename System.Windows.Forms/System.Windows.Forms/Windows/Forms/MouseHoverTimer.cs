using System;

namespace System.Windows.Forms
{
	// Token: 0x020003A1 RID: 929
	internal class MouseHoverTimer : IDisposable
	{
		// Token: 0x06003BEF RID: 15343 RVA: 0x00109B9C File Offset: 0x00107D9C
		public MouseHoverTimer()
		{
			int num = SystemInformation.MouseHoverTime;
			if (num == 0)
			{
				num = 400;
			}
			this.mouseHoverTimer.Interval = num;
			this.mouseHoverTimer.Tick += this.OnTick;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00109BEC File Offset: 0x00107DEC
		public void Start(ToolStripItem item)
		{
			if (item != this.currentItem)
			{
				this.Cancel(this.currentItem);
			}
			this.currentItem = item;
			if (this.currentItem != null)
			{
				this.mouseHoverTimer.Enabled = true;
			}
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00109C1E File Offset: 0x00107E1E
		public void Cancel()
		{
			this.mouseHoverTimer.Enabled = false;
			this.currentItem = null;
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00109C33 File Offset: 0x00107E33
		public void Cancel(ToolStripItem item)
		{
			if (item == this.currentItem)
			{
				this.Cancel();
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00109C44 File Offset: 0x00107E44
		public void Dispose()
		{
			if (this.mouseHoverTimer != null)
			{
				this.Cancel();
				this.mouseHoverTimer.Dispose();
				this.mouseHoverTimer = null;
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00109C66 File Offset: 0x00107E66
		private void OnTick(object sender, EventArgs e)
		{
			this.mouseHoverTimer.Enabled = false;
			if (this.currentItem != null && !this.currentItem.IsDisposed)
			{
				this.currentItem.FireEvent(EventArgs.Empty, ToolStripItemEventType.MouseHover);
			}
		}

		// Token: 0x04002388 RID: 9096
		private Timer mouseHoverTimer = new Timer();

		// Token: 0x04002389 RID: 9097
		private const int SPI_GETMOUSEHOVERTIME_WIN9X = 400;

		// Token: 0x0400238A RID: 9098
		private ToolStripItem currentItem;
	}
}
