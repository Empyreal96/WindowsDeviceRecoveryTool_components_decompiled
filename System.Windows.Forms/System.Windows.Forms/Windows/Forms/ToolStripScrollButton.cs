using System;
using System.Drawing;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020003ED RID: 1005
	internal class ToolStripScrollButton : ToolStripControlHost
	{
		// Token: 0x06004397 RID: 17303 RVA: 0x00121CDE File Offset: 0x0011FEDE
		public ToolStripScrollButton(bool up) : base(ToolStripScrollButton.CreateControlInstance(up))
		{
			this.up = up;
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x00121CFC File Offset: 0x0011FEFC
		private static Control CreateControlInstance(bool up)
		{
			return new ToolStripScrollButton.StickyLabel
			{
				ImageAlign = ContentAlignment.MiddleCenter,
				Image = (up ? ToolStripScrollButton.UpImage : ToolStripScrollButton.DownImage)
			};
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06004399 RID: 17305 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600439A RID: 17306 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected override Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x0600439B RID: 17307 RVA: 0x00121D2D File Offset: 0x0011FF2D
		private static Image DownImage
		{
			get
			{
				if (ToolStripScrollButton.downScrollImage == null)
				{
					ToolStripScrollButton.downScrollImage = new Bitmap(typeof(ToolStripScrollButton), "ScrollButtonDown.bmp");
					ToolStripScrollButton.downScrollImage.MakeTransparent(Color.White);
				}
				return ToolStripScrollButton.downScrollImage;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x0600439C RID: 17308 RVA: 0x00121D63 File Offset: 0x0011FF63
		internal ToolStripScrollButton.StickyLabel Label
		{
			get
			{
				return base.Control as ToolStripScrollButton.StickyLabel;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x0600439D RID: 17309 RVA: 0x00121D70 File Offset: 0x0011FF70
		private static Image UpImage
		{
			get
			{
				if (ToolStripScrollButton.upScrollImage == null)
				{
					ToolStripScrollButton.upScrollImage = new Bitmap(typeof(ToolStripScrollButton), "ScrollButtonUp.bmp");
					ToolStripScrollButton.upScrollImage.MakeTransparent(Color.White);
				}
				return ToolStripScrollButton.upScrollImage;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x0600439E RID: 17310 RVA: 0x00121DA6 File Offset: 0x0011FFA6
		private Timer MouseDownTimer
		{
			get
			{
				if (this.mouseDownTimer == null)
				{
					this.mouseDownTimer = new Timer();
				}
				return this.mouseDownTimer;
			}
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x00121DC1 File Offset: 0x0011FFC1
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.mouseDownTimer != null)
			{
				this.mouseDownTimer.Enabled = false;
				this.mouseDownTimer.Dispose();
				this.mouseDownTimer = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x00121DF4 File Offset: 0x0011FFF4
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.UnsubscribeAll();
			base.OnMouseDown(e);
			this.Scroll();
			this.MouseDownTimer.Interval = ToolStripScrollButton.AUTOSCROLL_PAUSE;
			this.MouseDownTimer.Tick += this.OnInitialAutoScrollMouseDown;
			this.MouseDownTimer.Enabled = true;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x00121E47 File Offset: 0x00120047
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.UnsubscribeAll();
			base.OnMouseUp(e);
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x00121E56 File Offset: 0x00120056
		protected override void OnMouseLeave(EventArgs e)
		{
			this.UnsubscribeAll();
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x00121E5E File Offset: 0x0012005E
		private void UnsubscribeAll()
		{
			this.MouseDownTimer.Enabled = false;
			this.MouseDownTimer.Tick -= this.OnInitialAutoScrollMouseDown;
			this.MouseDownTimer.Tick -= this.OnAutoScrollAccellerate;
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x00121E9A File Offset: 0x0012009A
		private void OnAutoScrollAccellerate(object sender, EventArgs e)
		{
			this.Scroll();
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x00121EA4 File Offset: 0x001200A4
		private void OnInitialAutoScrollMouseDown(object sender, EventArgs e)
		{
			this.MouseDownTimer.Tick -= this.OnInitialAutoScrollMouseDown;
			this.Scroll();
			this.MouseDownTimer.Interval = 50;
			this.MouseDownTimer.Tick += this.OnAutoScrollAccellerate;
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x00121EF4 File Offset: 0x001200F4
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			empty.Height = ((this.Label.Image != null) ? (this.Label.Image.Height + 4) : 0);
			empty.Width = ((base.ParentInternal != null) ? (base.ParentInternal.Width - 2) : empty.Width);
			return empty;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x00121F58 File Offset: 0x00120158
		private void Scroll()
		{
			ToolStripDropDownMenu toolStripDropDownMenu = base.ParentInternal as ToolStripDropDownMenu;
			if (toolStripDropDownMenu != null && this.Label.Enabled)
			{
				toolStripDropDownMenu.ScrollInternal(this.up);
			}
		}

		// Token: 0x040025A8 RID: 9640
		private bool up = true;

		// Token: 0x040025A9 RID: 9641
		[ThreadStatic]
		private static Bitmap upScrollImage;

		// Token: 0x040025AA RID: 9642
		[ThreadStatic]
		private static Bitmap downScrollImage;

		// Token: 0x040025AB RID: 9643
		private const int AUTOSCROLL_UPDATE = 50;

		// Token: 0x040025AC RID: 9644
		private static readonly int AUTOSCROLL_PAUSE = SystemInformation.DoubleClickTime;

		// Token: 0x040025AD RID: 9645
		private Timer mouseDownTimer;

		// Token: 0x0200074D RID: 1869
		internal class StickyLabel : Label
		{
			// Token: 0x1700175E RID: 5982
			// (get) Token: 0x060061E8 RID: 25064 RVA: 0x0019126E File Offset: 0x0018F46E
			public bool FreezeLocationChange
			{
				get
				{
					return this.freezeLocationChange;
				}
			}

			// Token: 0x060061E9 RID: 25065 RVA: 0x00191276 File Offset: 0x0018F476
			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				if ((specified & BoundsSpecified.Location) != BoundsSpecified.None && this.FreezeLocationChange)
				{
					return;
				}
				base.SetBoundsCore(x, y, width, height, specified);
			}

			// Token: 0x060061EA RID: 25066 RVA: 0x00191294 File Offset: 0x0018F494
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg >= 256 && m.Msg <= 264)
				{
					this.DefWndProc(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x040041A6 RID: 16806
			private bool freezeLocationChange;
		}
	}
}
