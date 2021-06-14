using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderItemImage" /> event. </summary>
	// Token: 0x020003C5 RID: 965
	public class ToolStripItemImageRenderEventArgs : ToolStripItemRenderEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemImageRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStripItem" /> within the specified space and that has the specified properties.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to paint the image.</param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" />.</param>
		/// <param name="imageRectangle">The bounding area of the image.</param>
		// Token: 0x06004060 RID: 16480 RVA: 0x001163F0 File Offset: 0x001145F0
		public ToolStripItemImageRenderEventArgs(Graphics g, ToolStripItem item, Rectangle imageRectangle) : base(g, item)
		{
			this.image = ((item.RightToLeftAutoMirrorImage && item.RightToLeft == RightToLeft.Yes) ? item.MirroredImage : item.Image);
			this.imageRectangle = imageRectangle;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemImageRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStripItem" /> that displays an image within the specified space and that has the specified properties. </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to paint the image.</param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to draw the image.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to paint.</param>
		/// <param name="imageRectangle">The bounding area of the image.</param>
		// Token: 0x06004061 RID: 16481 RVA: 0x0011643C File Offset: 0x0011463C
		public ToolStripItemImageRenderEventArgs(Graphics g, ToolStripItem item, Image image, Rectangle imageRectangle) : base(g, item)
		{
			this.image = image;
			this.imageRectangle = imageRectangle;
		}

		/// <summary>Gets the image painted on the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> painted on the <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x00116460 File Offset: 0x00114660
		public Image Image
		{
			get
			{
				return this.image;
			}
		}

		/// <summary>Gets the rectangle that represents the bounding area of the image.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounding area of the image.</returns>
		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x00116468 File Offset: 0x00114668
		public Rectangle ImageRectangle
		{
			get
			{
				return this.imageRectangle;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06004064 RID: 16484 RVA: 0x00116470 File Offset: 0x00114670
		// (set) Token: 0x06004065 RID: 16485 RVA: 0x00116478 File Offset: 0x00114678
		internal bool ShiftOnPress
		{
			get
			{
				return this.shiftOnPress;
			}
			set
			{
				this.shiftOnPress = value;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x00116481 File Offset: 0x00114681
		// (set) Token: 0x06004067 RID: 16487 RVA: 0x00116489 File Offset: 0x00114689
		internal ImageAttributes ImageAttributes
		{
			get
			{
				return this.imageAttr;
			}
			set
			{
				this.imageAttr = value;
			}
		}

		// Token: 0x040024AA RID: 9386
		private Image image;

		// Token: 0x040024AB RID: 9387
		private Rectangle imageRectangle = Rectangle.Empty;

		// Token: 0x040024AC RID: 9388
		private bool shiftOnPress;

		// Token: 0x040024AD RID: 9389
		private ImageAttributes imageAttr;
	}
}
