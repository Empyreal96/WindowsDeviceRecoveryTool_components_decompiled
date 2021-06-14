using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.DrawSubItem" /> event.</summary>
	// Token: 0x02000230 RID: 560
	public class DrawListViewSubItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawListViewSubItemEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> within which to draw. </param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> parent of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw. </param>
		/// <param name="subItem">The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</param>
		/// <param name="itemIndex">The index of the parent <see cref="T:System.Windows.Forms.ListViewItem" /> within the <see cref="P:System.Windows.Forms.ListView.Items" /> collection. </param>
		/// <param name="columnIndex">The index of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> column within the <see cref="P:System.Windows.Forms.ListView.Columns" /> collection. </param>
		/// <param name="header">The <see cref="T:System.Windows.Forms.ColumnHeader" /> for the column in which the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> is displayed. </param>
		/// <param name="itemState">A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the <see cref="T:System.Windows.Forms.ListViewItem" /> parent of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw. </param>
		// Token: 0x0600218A RID: 8586 RVA: 0x000A4CF0 File Offset: 0x000A2EF0
		public DrawListViewSubItemEventArgs(Graphics graphics, Rectangle bounds, ListViewItem item, ListViewItem.ListViewSubItem subItem, int itemIndex, int columnIndex, ColumnHeader header, ListViewItemStates itemState)
		{
			this.graphics = graphics;
			this.bounds = bounds;
			this.item = item;
			this.subItem = subItem;
			this.itemIndex = itemIndex;
			this.columnIndex = columnIndex;
			this.header = header;
			this.itemState = itemState;
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> should be drawn by the operating system instead of owner-drawn.</summary>
		/// <returns>
		///     <see langword="true" /> if the subitem should be drawn by the operating system; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000A4D40 File Offset: 0x000A2F40
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x000A4D48 File Offset: 0x000A2F48
		public bool DrawDefault
		{
			get
			{
				return this.drawDefault;
			}
			set
			{
				this.drawDefault = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to draw the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> used to draw the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" />.</returns>
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000A4D51 File Offset: 0x000A2F51
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the size and location of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</returns>
		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x000A4D59 File Offset: 0x000A2F59
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the parent <see cref="T:System.Windows.Forms.ListViewItem" /> of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ListViewItem" /> that represents the parent of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</returns>
		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000A4D61 File Offset: 0x000A2F61
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</returns>
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000A4D69 File Offset: 0x000A2F69
		public ListViewItem.ListViewSubItem SubItem
		{
			get
			{
				return this.subItem;
			}
		}

		/// <summary>Gets the index of the parent <see cref="T:System.Windows.Forms.ListViewItem" /> of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</summary>
		/// <returns>The index of the parent <see cref="T:System.Windows.Forms.ListViewItem" /> within the <see cref="P:System.Windows.Forms.ListView.Items" /> collection.</returns>
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000A4D71 File Offset: 0x000A2F71
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		/// <summary>Gets the index of the <see cref="T:System.Windows.Forms.ListView" /> column in which the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> is displayed.</summary>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> column within the <see cref="P:System.Windows.Forms.ListView.Columns" /> collection.</returns>
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000A4D79 File Offset: 0x000A2F79
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets the header of the <see cref="T:System.Windows.Forms.ListView" /> column in which the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> is displayed.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> for the column in which the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> is displayed.</returns>
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000A4D81 File Offset: 0x000A2F81
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		/// <summary>Gets the current state of the parent <see cref="T:System.Windows.Forms.ListViewItem" /> of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> to draw.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the parent <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x000A4D89 File Offset: 0x000A2F89
		public ListViewItemStates ItemState
		{
			get
			{
				return this.itemState;
			}
		}

		/// <summary>Draws the background of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> using its current background color.</summary>
		// Token: 0x06002195 RID: 8597 RVA: 0x000A4D94 File Offset: 0x000A2F94
		public void DrawBackground()
		{
			Color color = (this.itemIndex == -1) ? this.item.BackColor : this.subItem.BackColor;
			using (Brush brush = new SolidBrush(color))
			{
				this.Graphics.FillRectangle(brush, this.bounds);
			}
		}

		/// <summary>Draws a focus rectangle for the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> if the parent <see cref="T:System.Windows.Forms.ListViewItem" /> has focus.</summary>
		/// <param name="bounds">The area within which to draw the focus rectangle.</param>
		// Token: 0x06002196 RID: 8598 RVA: 0x000A4DF8 File Offset: 0x000A2FF8
		public void DrawFocusRectangle(Rectangle bounds)
		{
			if ((this.itemState & ListViewItemStates.Focused) == ListViewItemStates.Focused)
			{
				ControlPaint.DrawFocusRectangle(this.graphics, Rectangle.Inflate(bounds, -1, -1), this.item.ForeColor, this.item.BackColor);
			}
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> using its current foreground color.</summary>
		// Token: 0x06002197 RID: 8599 RVA: 0x000A4E30 File Offset: 0x000A3030
		public void DrawText()
		{
			HorizontalAlignment textAlign = this.header.TextAlign;
			TextFormatFlags textFormatFlags = (textAlign == HorizontalAlignment.Left) ? TextFormatFlags.Default : ((textAlign == HorizontalAlignment.Center) ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Right);
			textFormatFlags |= TextFormatFlags.WordEllipsis;
			this.DrawText(textFormatFlags);
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ListViewItem.ListViewSubItem" /> using its current foreground color and formatting it with the specified <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</summary>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" /> values. </param>
		// Token: 0x06002198 RID: 8600 RVA: 0x000A4E68 File Offset: 0x000A3068
		public void DrawText(TextFormatFlags flags)
		{
			string text = (this.itemIndex == -1) ? this.item.Text : this.subItem.Text;
			Font font = (this.itemIndex == -1) ? this.item.Font : this.subItem.Font;
			Color foreColor = (this.itemIndex == -1) ? this.item.ForeColor : this.subItem.ForeColor;
			int width = TextRenderer.MeasureText(" ", font).Width;
			Rectangle rectangle = Rectangle.Inflate(this.bounds, -width, 0);
			TextRenderer.DrawText(this.graphics, text, font, rectangle, foreColor, flags);
		}

		// Token: 0x04000E8F RID: 3727
		private readonly Graphics graphics;

		// Token: 0x04000E90 RID: 3728
		private readonly Rectangle bounds;

		// Token: 0x04000E91 RID: 3729
		private readonly ListViewItem item;

		// Token: 0x04000E92 RID: 3730
		private readonly ListViewItem.ListViewSubItem subItem;

		// Token: 0x04000E93 RID: 3731
		private readonly int itemIndex;

		// Token: 0x04000E94 RID: 3732
		private readonly int columnIndex;

		// Token: 0x04000E95 RID: 3733
		private readonly ColumnHeader header;

		// Token: 0x04000E96 RID: 3734
		private readonly ListViewItemStates itemState;

		// Token: 0x04000E97 RID: 3735
		private bool drawDefault;
	}
}
