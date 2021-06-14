using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderGrip" /> event. </summary>
	// Token: 0x020003B7 RID: 951
	public class ToolStripGripRenderEventArgs : ToolStripRenderEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripGripRenderEventArgs" /> class.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> object used to paint the move handle.</param>
		/// <param name="toolStrip">The <see cref="T:System.Windows.Forms.ToolStrip" /> the move handle is to be drawn on.</param>
		// Token: 0x06003EAF RID: 16047 RVA: 0x00111F3B File Offset: 0x0011013B
		public ToolStripGripRenderEventArgs(Graphics g, ToolStrip toolStrip) : base(g, toolStrip)
		{
		}

		/// <summary>Gets the rectangle representing the area in which to paint the move handle.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the area in which to paint the move handle.</returns>
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x00111F45 File Offset: 0x00110145
		public Rectangle GripBounds
		{
			get
			{
				return base.ToolStrip.GripRectangle;
			}
		}

		/// <summary>Gets the style that indicates whether the move handle is displayed vertically or horizontally.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripDisplayStyle" /> values.</returns>
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x00111F52 File Offset: 0x00110152
		public ToolStripGripDisplayStyle GripDisplayStyle
		{
			get
			{
				return base.ToolStrip.GripDisplayStyle;
			}
		}

		/// <summary>Gets the style that indicates whether or not the move handle is visible.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripGripDisplayStyle" /> values.</returns>
		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x00111F5F File Offset: 0x0011015F
		public ToolStripGripStyle GripStyle
		{
			get
			{
				return base.ToolStrip.GripStyle;
			}
		}
	}
}
