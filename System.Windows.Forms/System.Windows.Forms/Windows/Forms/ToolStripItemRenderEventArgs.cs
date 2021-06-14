using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the events that render the background of objects derived from <see cref="T:System.Windows.Forms.ToolStripItem" /> in the <see cref="T:System.Windows.Forms.ToolStripRenderer" /> class. </summary>
	// Token: 0x020003CA RID: 970
	public class ToolStripItemRenderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStripItem" /> and using the specified <see cref="T:System.Drawing.Graphics" />. </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> object used to draw the item.</param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> to be drawn.</param>
		// Token: 0x0600406C RID: 16492 RVA: 0x00116492 File Offset: 0x00114692
		public ToolStripItemRenderEventArgs(Graphics g, ToolStripItem item)
		{
			this.item = item;
			this.graphics = g;
		}

		/// <summary>Gets the graphics used to paint the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x001164A8 File Offset: 0x001146A8
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripItem" /> to paint.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripItem" /> to paint.</returns>
		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x001164B0 File Offset: 0x001146B0
		public ToolStripItem Item
		{
			get
			{
				return this.item;
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Forms.ToolStripItem.Owner" /> property for the <see cref="T:System.Windows.Forms.ToolStripItem" /> to paint.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStrip" /> that is the owner of the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x0600406F RID: 16495 RVA: 0x001164B8 File Offset: 0x001146B8
		public ToolStrip ToolStrip
		{
			get
			{
				return this.item.ParentInternal;
			}
		}

		// Token: 0x040024B9 RID: 9401
		private ToolStripItem item;

		// Token: 0x040024BA RID: 9402
		private Graphics graphics;
	}
}
