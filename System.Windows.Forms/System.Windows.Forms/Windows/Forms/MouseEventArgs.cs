using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.MouseUp" />, <see cref="E:System.Windows.Forms.Control.MouseDown" />, and <see cref="E:System.Windows.Forms.Control.MouseMove" /> events.</summary>
	// Token: 0x020002F2 RID: 754
	[ComVisible(true)]
	public class MouseEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.MouseEventArgs" /> class.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that indicate which mouse button was pressed. </param>
		/// <param name="clicks">The number of times a mouse button was pressed. </param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels. </param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		/// <param name="delta">A signed count of the number of detents the wheel has rotated. </param>
		// Token: 0x06002DC6 RID: 11718 RVA: 0x000D4FBC File Offset: 0x000D31BC
		public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
		{
			this.button = button;
			this.clicks = clicks;
			this.x = x;
			this.y = y;
			this.delta = delta;
		}

		/// <summary>Gets which mouse button was pressed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values.</returns>
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000D4FE9 File Offset: 0x000D31E9
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		/// <summary>Gets the number of times the mouse button was pressed and released.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the number of times the mouse button was pressed and released.</returns>
		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002DC8 RID: 11720 RVA: 0x000D4FF1 File Offset: 0x000D31F1
		public int Clicks
		{
			get
			{
				return this.clicks;
			}
		}

		/// <summary>Gets the x-coordinate of the mouse during the generating mouse event.</summary>
		/// <returns>The x-coordinate of the mouse, in pixels.</returns>
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x000D4FF9 File Offset: 0x000D31F9
		public int X
		{
			get
			{
				return this.x;
			}
		}

		/// <summary>Gets the y-coordinate of the mouse during the generating mouse event.</summary>
		/// <returns>The y-coordinate of the mouse, in pixels.</returns>
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002DCA RID: 11722 RVA: 0x000D5001 File Offset: 0x000D3201
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		/// <summary>Gets a signed count of the number of detents the mouse wheel has rotated, multiplied by the WHEEL_DELTA constant. A detent is one notch of the mouse wheel.</summary>
		/// <returns>A signed count of the number of detents the mouse wheel has rotated, multiplied by the WHEEL_DELTA constant.</returns>
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000D5009 File Offset: 0x000D3209
		public int Delta
		{
			get
			{
				return this.delta;
			}
		}

		/// <summary>Gets the location of the mouse during the generating mouse event.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that contains the x- and y- mouse coordinates, in pixels, relative to the upper-left corner of the form.</returns>
		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002DCC RID: 11724 RVA: 0x000D5011 File Offset: 0x000D3211
		public Point Location
		{
			get
			{
				return new Point(this.x, this.y);
			}
		}

		// Token: 0x04001388 RID: 5000
		private readonly MouseButtons button;

		// Token: 0x04001389 RID: 5001
		private readonly int clicks;

		// Token: 0x0400138A RID: 5002
		private readonly int x;

		// Token: 0x0400138B RID: 5003
		private readonly int y;

		// Token: 0x0400138C RID: 5004
		private readonly int delta;
	}
}
