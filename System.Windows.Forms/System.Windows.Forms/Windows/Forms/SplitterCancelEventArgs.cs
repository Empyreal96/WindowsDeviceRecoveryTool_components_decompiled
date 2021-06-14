using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for splitter events.</summary>
	// Token: 0x0200035E RID: 862
	public class SplitterCancelEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.SplitterCancelEventArgs" /> class with the specified coordinates of the mouse pointer and the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" />.</summary>
		/// <param name="mouseCursorX">The X coordinate of the mouse pointer in client coordinates. </param>
		/// <param name="mouseCursorY">The Y coordinate of the mouse pointer in client coordinates. </param>
		/// <param name="splitX">The X coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" /> in client coordinates. </param>
		/// <param name="splitY">The Y coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" /> in client coordinates. </param>
		// Token: 0x06003623 RID: 13859 RVA: 0x000F7459 File Offset: 0x000F5659
		public SplitterCancelEventArgs(int mouseCursorX, int mouseCursorY, int splitX, int splitY) : base(false)
		{
			this.mouseCursorX = mouseCursorX;
			this.mouseCursorY = mouseCursorY;
			this.splitX = splitX;
			this.splitY = splitY;
		}

		/// <summary>Gets the X coordinate of the mouse pointer in client coordinates.</summary>
		/// <returns>An integer representing the X coordinate of the mouse pointer in client coordinates.</returns>
		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x000F747F File Offset: 0x000F567F
		public int MouseCursorX
		{
			get
			{
				return this.mouseCursorX;
			}
		}

		/// <summary>Gets the Y coordinate of the mouse pointer in client coordinates.</summary>
		/// <returns>An integer representing the Y coordinate of the mouse pointer in client coordinates.</returns>
		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x000F7487 File Offset: 0x000F5687
		public int MouseCursorY
		{
			get
			{
				return this.mouseCursorY;
			}
		}

		/// <summary>Gets or sets the X coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" /> in client coordinates.</summary>
		/// <returns>An integer representing the X coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003626 RID: 13862 RVA: 0x000F748F File Offset: 0x000F568F
		// (set) Token: 0x06003627 RID: 13863 RVA: 0x000F7497 File Offset: 0x000F5697
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

		/// <summary>Gets or sets the Y coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" /> in client coordinates.</summary>
		/// <returns>An integer representing the Y coordinate of the upper left corner of the <see cref="T:System.Windows.Forms.SplitContainer" />.</returns>
		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x000F74A0 File Offset: 0x000F56A0
		// (set) Token: 0x06003629 RID: 13865 RVA: 0x000F74A8 File Offset: 0x000F56A8
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

		// Token: 0x040021AA RID: 8618
		private readonly int mouseCursorX;

		// Token: 0x040021AB RID: 8619
		private readonly int mouseCursorY;

		// Token: 0x040021AC RID: 8620
		private int splitX;

		// Token: 0x040021AD RID: 8621
		private int splitY;
	}
}
