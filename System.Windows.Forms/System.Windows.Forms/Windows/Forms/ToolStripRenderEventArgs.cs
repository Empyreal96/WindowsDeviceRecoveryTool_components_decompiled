using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="M:System.Windows.Forms.ToolStripRenderer.OnRenderImageMargin(System.Windows.Forms.ToolStripRenderEventArgs)" />, <see cref="M:System.Windows.Forms.ToolStripRenderer.OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs)" />, and <see cref="M:System.Windows.Forms.ToolStripRenderer.OnRenderToolStripBackground(System.Windows.Forms.ToolStripRenderEventArgs)" /> methods. </summary>
	// Token: 0x020003EA RID: 1002
	public class ToolStripRenderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStrip" /> and using the specified <see cref="T:System.Drawing.Graphics" />. </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to use for painting.</param>
		/// <param name="toolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> to paint.</param>
		// Token: 0x0600438C RID: 17292 RVA: 0x00121AA4 File Offset: 0x0011FCA4
		public ToolStripRenderEventArgs(Graphics g, ToolStrip toolStrip)
		{
			this.toolStrip = toolStrip;
			this.graphics = g;
			this.affectedBounds = new Rectangle(Point.Empty, toolStrip.Size);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStrip" />, using the specified <see cref="T:System.Drawing.Graphics" /> to paint the specified bounds with the specified <see cref="T:System.Drawing.Color" />.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to use for painting.</param>
		/// <param name="toolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> to paint.</param>
		/// <param name="affectedBounds">The <see cref="T:System.Drawing.Rectangle" /> representing the bounds of the area to be painted.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> that the background of the <see cref="T:System.Windows.Forms.ToolStrip" /> is painted with.</param>
		// Token: 0x0600438D RID: 17293 RVA: 0x00121AF1 File Offset: 0x0011FCF1
		public ToolStripRenderEventArgs(Graphics g, ToolStrip toolStrip, Rectangle affectedBounds, Color backColor)
		{
			this.toolStrip = toolStrip;
			this.affectedBounds = affectedBounds;
			this.graphics = g;
			this.backColor = backColor;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Rectangle" /> representing the bounds of the area to be painted. </summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> representing the bounds of the area to be painted.</returns>
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x0600438E RID: 17294 RVA: 0x00121B2C File Offset: 0x0011FD2C
		public Rectangle AffectedBounds
		{
			get
			{
				return this.affectedBounds;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Color" /> that the background of the <see cref="T:System.Windows.Forms.ToolStrip" /> is painted with.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that the background of the <see cref="T:System.Windows.Forms.ToolStrip" /> is painted with.</returns>
		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x0600438F RID: 17295 RVA: 0x00121B34 File Offset: 0x0011FD34
		public Color BackColor
		{
			get
			{
				if (this.backColor == Color.Empty)
				{
					this.backColor = this.toolStrip.RawBackColor;
					if (this.backColor == Color.Empty)
					{
						if (this.toolStrip is ToolStripDropDown)
						{
							this.backColor = SystemColors.Menu;
						}
						else if (this.toolStrip is MenuStrip)
						{
							this.backColor = SystemColors.MenuBar;
						}
						else
						{
							this.backColor = SystemColors.Control;
						}
					}
				}
				return this.backColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> used to paint.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint.</returns>
		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06004390 RID: 17296 RVA: 0x00121BBB File Offset: 0x0011FDBB
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStrip" /> to be painted.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStrip" /> to be painted.</returns>
		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06004391 RID: 17297 RVA: 0x00121BC3 File Offset: 0x0011FDC3
		public ToolStrip ToolStrip
		{
			get
			{
				return this.toolStrip;
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Rectangle" /> representing the overlap area between a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> and its <see cref="P:System.Windows.Forms.ToolStripDropDown.OwnerItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> representing the overlap area between a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> and its <see cref="P:System.Windows.Forms.ToolStripDropDown.OwnerItem" />.</returns>
		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x06004392 RID: 17298 RVA: 0x00121BCC File Offset: 0x0011FDCC
		public Rectangle ConnectedArea
		{
			get
			{
				ToolStripDropDown toolStripDropDown = this.toolStrip as ToolStripDropDown;
				if (toolStripDropDown != null)
				{
					ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
					if (toolStripDropDownItem is MdiControlStrip.SystemMenuItem)
					{
						return Rectangle.Empty;
					}
					if (toolStripDropDownItem != null && toolStripDropDownItem.ParentInternal != null && !toolStripDropDownItem.IsOnDropDown)
					{
						Rectangle rect = new Rectangle(this.toolStrip.PointToClient(toolStripDropDownItem.TranslatePoint(Point.Empty, ToolStripPointType.ToolStripItemCoords, ToolStripPointType.ScreenCoords)), toolStripDropDownItem.Size);
						Rectangle bounds = this.ToolStrip.Bounds;
						Rectangle clientRectangle = this.ToolStrip.ClientRectangle;
						clientRectangle.Inflate(1, 1);
						if (clientRectangle.IntersectsWith(rect))
						{
							switch (toolStripDropDownItem.DropDownDirection)
							{
							case ToolStripDropDownDirection.AboveLeft:
							case ToolStripDropDownDirection.AboveRight:
								return Rectangle.Empty;
							case ToolStripDropDownDirection.BelowLeft:
							case ToolStripDropDownDirection.BelowRight:
								clientRectangle.Intersect(rect);
								if (clientRectangle.Height == 2)
								{
									return new Rectangle(rect.X + 1, 0, rect.Width - 2, 2);
								}
								return Rectangle.Empty;
							case ToolStripDropDownDirection.Left:
							case ToolStripDropDownDirection.Right:
								return Rectangle.Empty;
							}
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x0400259F RID: 9631
		private ToolStrip toolStrip;

		// Token: 0x040025A0 RID: 9632
		private Graphics graphics;

		// Token: 0x040025A1 RID: 9633
		private Rectangle affectedBounds = Rectangle.Empty;

		// Token: 0x040025A2 RID: 9634
		private Color backColor = Color.Empty;
	}
}
