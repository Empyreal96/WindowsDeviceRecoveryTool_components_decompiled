using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.DrawColumnHeader" /> event. </summary>
	// Token: 0x0200022C RID: 556
	public class DrawListViewColumnHeaderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawListViewColumnHeaderEventArgs" /> class. </summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> within which to draw.</param>
		/// <param name="columnIndex">The index of the header's column within the <see cref="P:System.Windows.Forms.ListView.Columns" /> collection.</param>
		/// <param name="header">The <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the header to draw.</param>
		/// <param name="state">A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the column header.</param>
		/// <param name="foreColor">The foreground <see cref="T:System.Drawing.Color" /> of the header.</param>
		/// <param name="backColor">The background <see cref="T:System.Drawing.Color" /> of the header.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> used for the header text.</param>
		// Token: 0x06002167 RID: 8551 RVA: 0x000A4828 File Offset: 0x000A2A28
		public DrawListViewColumnHeaderEventArgs(Graphics graphics, Rectangle bounds, int columnIndex, ColumnHeader header, ListViewItemStates state, Color foreColor, Color backColor, Font font)
		{
			this.graphics = graphics;
			this.bounds = bounds;
			this.columnIndex = columnIndex;
			this.header = header;
			this.state = state;
			this.foreColor = foreColor;
			this.backColor = backColor;
			this.font = font;
		}

		/// <summary>Gets or sets a value indicating whether the column header should be drawn by the operating system instead of owner-drawn.</summary>
		/// <returns>
		///     <see langword="true" /> if the header should be drawn by the operating system; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000A4878 File Offset: 0x000A2A78
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000A4880 File Offset: 0x000A2A80
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

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to draw the column header.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> used to draw the column header.</returns>
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000A4889 File Offset: 0x000A2A89
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the size and location of the column header to draw.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the column header.</returns>
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600216B RID: 8555 RVA: 0x000A4891 File Offset: 0x000A2A91
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the index of the <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the header to draw.</summary>
		/// <returns>The index of the column header within the <see cref="P:System.Windows.Forms.ListView.Columns" /> collection.</returns>
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000A4899 File Offset: 0x000A2A99
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ColumnHeader" /> representing the column header.</returns>
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000A48A1 File Offset: 0x000A2AA1
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		/// <summary>Gets the current state of the column header.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the column header.</returns>
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000A48A9 File Offset: 0x000A2AA9
		public ListViewItemStates State
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Gets the foreground color of the header.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing the foreground color of the header.</returns>
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600216F RID: 8559 RVA: 0x000A48B1 File Offset: 0x000A2AB1
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
		}

		/// <summary>Gets the background color of the header.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing the background color of the header.</returns>
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000A48B9 File Offset: 0x000A2AB9
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
		}

		/// <summary>Gets the font used to draw the column header text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> representing the font of the header text.</returns>
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x000A48C1 File Offset: 0x000A2AC1
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		/// <summary>Draws the background of the column header.</summary>
		// Token: 0x06002172 RID: 8562 RVA: 0x000A48CC File Offset: 0x000A2ACC
		public void DrawBackground()
		{
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
				visualStyleRenderer.DrawBackground(this.graphics, this.bounds);
				return;
			}
			using (Brush brush = new SolidBrush(this.backColor))
			{
				this.graphics.FillRectangle(brush, this.bounds);
			}
			Rectangle rect = this.bounds;
			rect.Width--;
			rect.Height--;
			this.graphics.DrawRectangle(SystemPens.ControlDarkDark, rect);
			rect.Width--;
			rect.Height--;
			this.graphics.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Y, rect.Right, rect.Y);
			this.graphics.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Y, rect.X, rect.Bottom);
			this.graphics.DrawLine(SystemPens.ControlDark, rect.X + 1, rect.Bottom, rect.Right, rect.Bottom);
			this.graphics.DrawLine(SystemPens.ControlDark, rect.Right, rect.Y + 1, rect.Right, rect.Bottom);
		}

		/// <summary>Draws the column header text using the default formatting.</summary>
		// Token: 0x06002173 RID: 8563 RVA: 0x000A4A40 File Offset: 0x000A2C40
		public void DrawText()
		{
			HorizontalAlignment textAlign = this.header.TextAlign;
			TextFormatFlags textFormatFlags = (textAlign == HorizontalAlignment.Left) ? TextFormatFlags.Default : ((textAlign == HorizontalAlignment.Center) ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Right);
			textFormatFlags |= TextFormatFlags.WordEllipsis;
			this.DrawText(textFormatFlags);
		}

		/// <summary>Draws the column header text, formatting it with the specified <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</summary>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" /> values. </param>
		// Token: 0x06002174 RID: 8564 RVA: 0x000A4A78 File Offset: 0x000A2C78
		public void DrawText(TextFormatFlags flags)
		{
			string text = this.header.Text;
			int width = TextRenderer.MeasureText(" ", this.font).Width;
			Rectangle rectangle = Rectangle.Inflate(this.bounds, -width, 0);
			TextRenderer.DrawText(this.graphics, text, this.font, rectangle, this.foreColor, flags);
		}

		// Token: 0x04000E80 RID: 3712
		private readonly Graphics graphics;

		// Token: 0x04000E81 RID: 3713
		private readonly Rectangle bounds;

		// Token: 0x04000E82 RID: 3714
		private readonly int columnIndex;

		// Token: 0x04000E83 RID: 3715
		private readonly ColumnHeader header;

		// Token: 0x04000E84 RID: 3716
		private readonly ListViewItemStates state;

		// Token: 0x04000E85 RID: 3717
		private readonly Color foreColor;

		// Token: 0x04000E86 RID: 3718
		private readonly Color backColor;

		// Token: 0x04000E87 RID: 3719
		private readonly Font font;

		// Token: 0x04000E88 RID: 3720
		private bool drawDefault;
	}
}
