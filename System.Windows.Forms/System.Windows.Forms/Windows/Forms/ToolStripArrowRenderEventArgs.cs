using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderArrow" /> event. </summary>
	// Token: 0x020003A3 RID: 931
	public class ToolStripArrowRenderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripArrowRenderEventArgs" /> class. </summary>
		/// <param name="g">The graphics used to paint the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</param>
		/// <param name="toolStripItem">The <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to paint the arrow.</param>
		/// <param name="arrowRectangle">The bounding area of the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</param>
		/// <param name="arrowColor">The color of the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</param>
		/// <param name="arrowDirection">The direction in which the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow points.</param>
		// Token: 0x06003C00 RID: 15360 RVA: 0x0010A318 File Offset: 0x00108518
		public ToolStripArrowRenderEventArgs(Graphics g, ToolStripItem toolStripItem, Rectangle arrowRectangle, Color arrowColor, ArrowDirection arrowDirection)
		{
			this.item = toolStripItem;
			this.graphics = g;
			this.arrowRect = arrowRectangle;
			this.defaultArrowColor = arrowColor;
			this.arrowDirection = arrowDirection;
		}

		/// <summary>Gets or sets the bounding area of the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding area.</returns>
		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x0010A379 File Offset: 0x00108579
		// (set) Token: 0x06003C02 RID: 15362 RVA: 0x0010A381 File Offset: 0x00108581
		public Rectangle ArrowRectangle
		{
			get
			{
				return this.arrowRect;
			}
			set
			{
				this.arrowRect = value;
			}
		}

		/// <summary>Gets or sets the color of the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of the arrow.</returns>
		// Token: 0x17000F07 RID: 3847
		// (get) Token: 0x06003C03 RID: 15363 RVA: 0x0010A38A File Offset: 0x0010858A
		// (set) Token: 0x06003C04 RID: 15364 RVA: 0x0010A3A1 File Offset: 0x001085A1
		public Color ArrowColor
		{
			get
			{
				if (this.arrowColorChanged)
				{
					return this.arrowColor;
				}
				return this.DefaultArrowColor;
			}
			set
			{
				this.arrowColor = value;
				this.arrowColorChanged = true;
			}
		}

		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06003C05 RID: 15365 RVA: 0x0010A3B1 File Offset: 0x001085B1
		// (set) Token: 0x06003C06 RID: 15366 RVA: 0x0010A3B9 File Offset: 0x001085B9
		internal Color DefaultArrowColor
		{
			get
			{
				return this.defaultArrowColor;
			}
			set
			{
				this.defaultArrowColor = value;
			}
		}

		/// <summary>Gets or sets the direction in which the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow points.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ArrowDirection" /> values.</returns>
		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06003C07 RID: 15367 RVA: 0x0010A3C2 File Offset: 0x001085C2
		// (set) Token: 0x06003C08 RID: 15368 RVA: 0x0010A3CA File Offset: 0x001085CA
		public ArrowDirection Direction
		{
			get
			{
				return this.arrowDirection;
			}
			set
			{
				this.arrowDirection = value;
			}
		}

		/// <summary>Gets the graphics used to paint the <see cref="T:System.Windows.Forms.ToolStrip" /> arrow.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint. </returns>
		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06003C09 RID: 15369 RVA: 0x0010A3D3 File Offset: 0x001085D3
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to paint the arrow.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to paint the arrow.</returns>
		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06003C0A RID: 15370 RVA: 0x0010A3DB File Offset: 0x001085DB
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x0400238C RID: 9100
		private Graphics graphics;

		// Token: 0x0400238D RID: 9101
		private Rectangle arrowRect = Rectangle.Empty;

		// Token: 0x0400238E RID: 9102
		private Color arrowColor = Color.Empty;

		// Token: 0x0400238F RID: 9103
		private Color defaultArrowColor = Color.Empty;

		// Token: 0x04002390 RID: 9104
		private ArrowDirection arrowDirection = ArrowDirection.Down;

		// Token: 0x04002391 RID: 9105
		private ToolStripItem item;

		// Token: 0x04002392 RID: 9106
		private bool arrowColorChanged;
	}
}
