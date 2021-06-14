using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see langword="DrawItem" /> event.</summary>
	// Token: 0x02000229 RID: 553
	public class DrawItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> class for the specified control with the specified font, state, surface to draw on, and the bounds to draw within.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to use, usually the parent control's <see cref="T:System.Drawing.Font" /> property. </param>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> bounds to draw within. </param>
		/// <param name="index">The <see cref="T:System.Windows.Forms.Control.ControlCollection" /> index value of the item that is being drawn. </param>
		/// <param name="state">The control's <see cref="T:System.Windows.Forms.DrawItemState" /> information. </param>
		// Token: 0x06002158 RID: 8536 RVA: 0x000A46D0 File Offset: 0x000A28D0
		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state)
		{
			this.graphics = graphics;
			this.font = font;
			this.rect = rect;
			this.index = index;
			this.state = state;
			this.foreColor = SystemColors.WindowText;
			this.backColor = SystemColors.Window;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawItemEventArgs" /> class for the specified control with the specified font, state, foreground color, background color, surface to draw on, and the bounds to draw within.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to use, usually the parent control's <see cref="T:System.Drawing.Font" /> property. </param>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> bounds to draw within. </param>
		/// <param name="index">The <see cref="T:System.Windows.Forms.Control.ControlCollection" /> index value of the item that is being drawn. </param>
		/// <param name="state">The control's <see cref="T:System.Windows.Forms.DrawItemState" /> information. </param>
		/// <param name="foreColor">The foreground <see cref="T:System.Drawing.Color" /> to draw the control with. </param>
		/// <param name="backColor">The background <see cref="T:System.Drawing.Color" /> to draw the control with. </param>
		// Token: 0x06002159 RID: 8537 RVA: 0x000A471E File Offset: 0x000A291E
		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state, Color foreColor, Color backColor)
		{
			this.graphics = graphics;
			this.font = font;
			this.rect = rect;
			this.index = index;
			this.state = state;
			this.foreColor = foreColor;
			this.backColor = backColor;
		}

		/// <summary>Gets the background color of the item that is being drawn.</summary>
		/// <returns>The background <see cref="T:System.Drawing.Color" /> of the item that is being drawn.</returns>
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600215A RID: 8538 RVA: 0x000A475B File Offset: 0x000A295B
		public Color BackColor
		{
			get
			{
				if ((this.state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					return SystemColors.Highlight;
				}
				return this.backColor;
			}
		}

		/// <summary>Gets the rectangle that represents the bounds of the item that is being drawn.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the item that is being drawn.</returns>
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x000A4774 File Offset: 0x000A2974
		public Rectangle Bounds
		{
			get
			{
				return this.rect;
			}
		}

		/// <summary>Gets the font that is assigned to the item being drawn.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> that is assigned to the item being drawn.</returns>
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x000A477C File Offset: 0x000A297C
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		/// <summary>Gets the foreground color of the of the item being drawn.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the item being drawn.</returns>
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600215D RID: 8541 RVA: 0x000A4784 File Offset: 0x000A2984
		public Color ForeColor
		{
			get
			{
				if ((this.state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					return SystemColors.HighlightText;
				}
				return this.foreColor;
			}
		}

		/// <summary>Gets the graphics surface to draw the item on.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> surface to draw the item on.</returns>
		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x000A479D File Offset: 0x000A299D
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the index value of the item that is being drawn.</summary>
		/// <returns>The numeric value that represents the <see cref="P:System.Windows.Forms.Control.ControlCollection.Item(System.Int32)" /> value of the item being drawn.</returns>
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600215F RID: 8543 RVA: 0x000A47A5 File Offset: 0x000A29A5
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>Gets the state of the item being drawn.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DrawItemState" /> that represents the state of the item being drawn.</returns>
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x000A47AD File Offset: 0x000A29AD
		public DrawItemState State
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Draws the background within the bounds specified in the <see cref="Overload:System.Windows.Forms.DrawItemEventArgs.#ctor" /> constructor and with the appropriate color.</summary>
		// Token: 0x06002161 RID: 8545 RVA: 0x000A47B8 File Offset: 0x000A29B8
		public virtual void DrawBackground()
		{
			Brush brush = new SolidBrush(this.BackColor);
			this.Graphics.FillRectangle(brush, this.rect);
			brush.Dispose();
		}

		/// <summary>Draws a focus rectangle within the bounds specified in the <see cref="Overload:System.Windows.Forms.DrawItemEventArgs.#ctor" /> constructor.</summary>
		// Token: 0x06002162 RID: 8546 RVA: 0x000A47E9 File Offset: 0x000A29E9
		public virtual void DrawFocusRectangle()
		{
			if ((this.state & DrawItemState.Focus) == DrawItemState.Focus && (this.state & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
			{
				ControlPaint.DrawFocusRectangle(this.Graphics, this.rect, this.ForeColor, this.BackColor);
			}
		}

		// Token: 0x04000E6C RID: 3692
		private Color backColor;

		// Token: 0x04000E6D RID: 3693
		private Color foreColor;

		// Token: 0x04000E6E RID: 3694
		private Font font;

		// Token: 0x04000E6F RID: 3695
		private readonly Graphics graphics;

		// Token: 0x04000E70 RID: 3696
		private readonly int index;

		// Token: 0x04000E71 RID: 3697
		private readonly Rectangle rect;

		// Token: 0x04000E72 RID: 3698
		private readonly DrawItemState state;
	}
}
