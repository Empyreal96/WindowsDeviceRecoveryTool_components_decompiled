using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ListView.DrawItem" /> event.</summary>
	// Token: 0x0200022E RID: 558
	public class DrawListViewItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawListViewItemEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ListViewItem" /> to draw. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> within which to draw. </param>
		/// <param name="itemIndex">The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> within the <see cref="P:System.Windows.Forms.ListView.Items" /> collection. </param>
		/// <param name="state">A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the <see cref="T:System.Windows.Forms.ListViewItem" /> to draw. </param>
		// Token: 0x06002179 RID: 8569 RVA: 0x000A4AD3 File Offset: 0x000A2CD3
		public DrawListViewItemEventArgs(Graphics graphics, ListViewItem item, Rectangle bounds, int itemIndex, ListViewItemStates state)
		{
			this.graphics = graphics;
			this.item = item;
			this.bounds = bounds;
			this.itemIndex = itemIndex;
			this.state = state;
			this.drawDefault = false;
		}

		/// <summary>Gets or sets a property indicating whether the <see cref="T:System.Windows.Forms.ListView" /> control will use the default drawing for the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the system draws the item; <see langword="false" /> if the event handler draws the item. The default value is <see langword="false" />.</returns>
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000A4B07 File Offset: 0x000A2D07
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x000A4B0F File Offset: 0x000A2D0F
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

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to draw the <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> used to draw the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000A4B18 File Offset: 0x000A2D18
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ListViewItem" /> to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ListViewItem" /> to draw.</returns>
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x000A4B20 File Offset: 0x000A2D20
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets the size and location of the <see cref="T:System.Windows.Forms.ListViewItem" /> to draw.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.ListViewItem" /> to draw.</returns>
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000A4B28 File Offset: 0x000A2D28
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the index of the <see cref="T:System.Windows.Forms.ListViewItem" /> within the <see cref="P:System.Windows.Forms.ListView.Items" /> collection of the containing <see cref="T:System.Windows.Forms.ListView" />.</summary>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.ListViewItem" /> within the <see cref="P:System.Windows.Forms.ListView.Items" /> collection.</returns>
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x000A4B30 File Offset: 0x000A2D30
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		/// <summary>Gets the current state of the <see cref="T:System.Windows.Forms.ListViewItem" /> to draw.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.ListViewItemStates" /> values indicating the current state of the <see cref="T:System.Windows.Forms.ListViewItem" />.</returns>
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000A4B38 File Offset: 0x000A2D38
		public ListViewItemStates State
		{
			get
			{
				return this.state;
			}
		}

		/// <summary>Draws the background of the <see cref="T:System.Windows.Forms.ListViewItem" /> using its current background color.</summary>
		// Token: 0x06002181 RID: 8577 RVA: 0x000A4B40 File Offset: 0x000A2D40
		public void DrawBackground()
		{
			Brush brush = new SolidBrush(this.item.BackColor);
			this.Graphics.FillRectangle(brush, this.bounds);
			brush.Dispose();
		}

		/// <summary>Draws a focus rectangle for the <see cref="T:System.Windows.Forms.ListViewItem" /> if it has focus.</summary>
		// Token: 0x06002182 RID: 8578 RVA: 0x000A4B78 File Offset: 0x000A2D78
		public void DrawFocusRectangle()
		{
			if ((this.state & ListViewItemStates.Focused) == ListViewItemStates.Focused)
			{
				Rectangle originalBounds = this.bounds;
				ControlPaint.DrawFocusRectangle(this.graphics, this.UpdateBounds(originalBounds, false), this.item.ForeColor, this.item.BackColor);
			}
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ListViewItem" /> using its current foreground color.</summary>
		// Token: 0x06002183 RID: 8579 RVA: 0x000A4BC2 File Offset: 0x000A2DC2
		public void DrawText()
		{
			this.DrawText(TextFormatFlags.Default);
		}

		/// <summary>Draws the text of the <see cref="T:System.Windows.Forms.ListViewItem" /> using its current foreground color and formatting it with the specified <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</summary>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Windows.Forms.TextFormatFlags" /> values. </param>
		// Token: 0x06002184 RID: 8580 RVA: 0x000A4BCB File Offset: 0x000A2DCB
		public void DrawText(TextFormatFlags flags)
		{
			TextRenderer.DrawText(this.graphics, this.item.Text, this.item.Font, this.UpdateBounds(this.bounds, true), this.item.ForeColor, flags);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000A4C08 File Offset: 0x000A2E08
		private Rectangle UpdateBounds(Rectangle originalBounds, bool drawText)
		{
			Rectangle result = originalBounds;
			if (this.item.ListView.View == View.Details)
			{
				if (!this.item.ListView.FullRowSelect && this.item.SubItems.Count > 0)
				{
					ListViewItem.ListViewSubItem listViewSubItem = this.item.SubItems[0];
					Size size = TextRenderer.MeasureText(listViewSubItem.Text, listViewSubItem.Font);
					result = new Rectangle(originalBounds.X, originalBounds.Y, size.Width, size.Height);
					result.X += 4;
					int num = result.Width;
					result.Width = num + 1;
				}
				else
				{
					result.X += 4;
					result.Width -= 4;
				}
				if (drawText)
				{
					int num = result.X;
					result.X = num - 1;
				}
			}
			return result;
		}

		// Token: 0x04000E89 RID: 3721
		private readonly Graphics graphics;

		// Token: 0x04000E8A RID: 3722
		private readonly ListViewItem item;

		// Token: 0x04000E8B RID: 3723
		private readonly Rectangle bounds;

		// Token: 0x04000E8C RID: 3724
		private readonly int itemIndex;

		// Token: 0x04000E8D RID: 3725
		private readonly ListViewItemStates state;

		// Token: 0x04000E8E RID: 3726
		private bool drawDefault;
	}
}
