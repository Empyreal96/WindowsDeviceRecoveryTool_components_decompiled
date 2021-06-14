using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for <see cref="E:System.Windows.Forms.Splitter.SplitterMoving" /> and the <see cref="E:System.Windows.Forms.Splitter.SplitterMoved" /> events.</summary>
	// Token: 0x02000360 RID: 864
	[ComVisible(true)]
	public class SplitterEventArgs : EventArgs
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.SplitterEventArgs" /> class with the specified coordinates of the mouse pointer and the coordinates of the upper-left corner of the <see cref="T:System.Windows.Forms.Splitter" /> control.</summary>
		/// <param name="x">The x-coordinate of the mouse pointer (in client coordinates). </param>
		/// <param name="y">The y-coordinate of the mouse pointer (in client coordinates). </param>
		/// <param name="splitX">The x-coordinate of the upper-left corner of the <see cref="T:System.Windows.Forms.Splitter" /> (in client coordinates). </param>
		/// <param name="splitY">The y-coordinate of the upper-left corner of the <see cref="T:System.Windows.Forms.Splitter" /> (in client coordinates). </param>
		// Token: 0x0600362E RID: 13870 RVA: 0x000F74B1 File Offset: 0x000F56B1
		public SplitterEventArgs(int x, int y, int splitX, int splitY)
		{
			this.x = x;
			this.y = y;
			this.splitX = splitX;
			this.splitY = splitY;
		}

		/// <summary>Gets the x-coordinate of the mouse pointer (in client coordinates).</summary>
		/// <returns>The x-coordinate of the mouse pointer.</returns>
		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x000F74D6 File Offset: 0x000F56D6
		public int X
		{
			get
			{
				return this.x;
			}
		}

		/// <summary>Gets the y-coordinate of the mouse pointer (in client coordinates).</summary>
		/// <returns>The y-coordinate of the mouse pointer.</returns>
		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x000F74DE File Offset: 0x000F56DE
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of the <see cref="T:System.Windows.Forms.Splitter" /> (in client coordinates).</summary>
		/// <returns>The x-coordinate of the upper-left corner of the control.</returns>
		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003631 RID: 13873 RVA: 0x000F74E6 File Offset: 0x000F56E6
		// (set) Token: 0x06003632 RID: 13874 RVA: 0x000F74EE File Offset: 0x000F56EE
		public int SplitX
		{
			get
			{
				return this.splitX;
			}
			set
			{
				this.splitX = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of the <see cref="T:System.Windows.Forms.Splitter" /> (in client coordinates).</summary>
		/// <returns>The y-coordinate of the upper-left corner of the control.</returns>
		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003633 RID: 13875 RVA: 0x000F74F7 File Offset: 0x000F56F7
		// (set) Token: 0x06003634 RID: 13876 RVA: 0x000F74FF File Offset: 0x000F56FF
		public int SplitY
		{
			get
			{
				return this.splitY;
			}
			set
			{
				this.splitY = value;
			}
		}

		// Token: 0x040021AE RID: 8622
		private readonly int x;

		// Token: 0x040021AF RID: 8623
		private readonly int y;

		// Token: 0x040021B0 RID: 8624
		private int splitX;

		// Token: 0x040021B1 RID: 8625
		private int splitY;
	}
}
