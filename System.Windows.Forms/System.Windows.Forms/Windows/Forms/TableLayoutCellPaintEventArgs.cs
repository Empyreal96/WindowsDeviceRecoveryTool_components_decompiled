using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TableLayoutPanel.CellPaint" /> event.</summary>
	// Token: 0x02000388 RID: 904
	public class TableLayoutCellPaintEventArgs : PaintEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TableLayoutCellPaintEventArgs" /> class.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to paint the item.</param>
		/// <param name="clipRectangle">The <see cref="T:System.Drawing.Rectangle" /> that represents the rectangle in which to paint.</param>
		/// <param name="cellBounds">The bounds of the cell.</param>
		/// <param name="column">The column of the cell.</param>
		/// <param name="row">The row of the cell.</param>
		// Token: 0x060038DA RID: 14554 RVA: 0x000FE7DB File Offset: 0x000FC9DB
		public TableLayoutCellPaintEventArgs(Graphics g, Rectangle clipRectangle, Rectangle cellBounds, int column, int row) : base(g, clipRectangle)
		{
			this.bounds = cellBounds;
			this.row = row;
			this.column = column;
		}

		/// <summary>Gets the size and location of the cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the size and location of the cell.</returns>
		// Token: 0x17000E32 RID: 3634
		// (get) Token: 0x060038DB RID: 14555 RVA: 0x000FE7FC File Offset: 0x000FC9FC
		public Rectangle CellBounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the row of the cell.</summary>
		/// <returns>The row position of the cell.</returns>
		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x060038DC RID: 14556 RVA: 0x000FE804 File Offset: 0x000FCA04
		public int Row
		{
			get
			{
				return this.row;
			}
		}

		/// <summary>Gets the column of the cell.</summary>
		/// <returns>The column position of the cell.</returns>
		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x060038DD RID: 14557 RVA: 0x000FE80C File Offset: 0x000FCA0C
		public int Column
		{
			get
			{
				return this.column;
			}
		}

		// Token: 0x04002279 RID: 8825
		private Rectangle bounds;

		// Token: 0x0400227A RID: 8826
		private int row;

		// Token: 0x0400227B RID: 8827
		private int column;
	}
}
