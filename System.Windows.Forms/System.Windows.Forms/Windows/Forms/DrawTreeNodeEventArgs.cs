using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.DrawNode" /> event.</summary>
	// Token: 0x02000235 RID: 565
	public class DrawTreeNodeEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DrawTreeNodeEventArgs" /> class.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw. </param>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> to draw. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> within which to draw. </param>
		/// <param name="state">A bitwise combination of the <see cref="T:System.Windows.Forms.TreeNodeStates" /> values indicating the current state of the <see cref="T:System.Windows.Forms.TreeNode" /> to draw. </param>
		// Token: 0x060021AC RID: 8620 RVA: 0x000A5011 File Offset: 0x000A3211
		public DrawTreeNodeEventArgs(Graphics graphics, TreeNode node, Rectangle bounds, TreeNodeStates state)
		{
			this.graphics = graphics;
			this.node = node;
			this.bounds = bounds;
			this.state = state;
			this.drawDefault = false;
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.TreeNode" /> should be drawn by the operating system rather than being owner drawn.</summary>
		/// <returns>
		///     <see langword="true" /> if the node should be drawn by the operating system; <see langword="false" /> if the node will be drawn in the event handler. The default value is <see langword="false" />.</returns>
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x000A503D File Offset: 0x000A323D
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x000A5045 File Offset: 0x000A3245
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

		/// <summary>Gets the <see cref="T:System.Drawing.Graphics" /> object used to draw the <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> used to draw the <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x000A504E File Offset: 0x000A324E
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TreeNode" /> to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> to draw.</returns>
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x000A5056 File Offset: 0x000A3256
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		/// <summary>Gets the size and location of the <see cref="T:System.Windows.Forms.TreeNode" /> to draw.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.TreeNode" /> to draw.</returns>
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x000A505E File Offset: 0x000A325E
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the current state of the <see cref="T:System.Windows.Forms.TreeNode" /> to draw.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.TreeNodeStates" /> values indicating the current state of the <see cref="T:System.Windows.Forms.TreeNode" />.</returns>
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060021B2 RID: 8626 RVA: 0x000A5066 File Offset: 0x000A3266
		public TreeNodeStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04000EA4 RID: 3748
		private readonly Graphics graphics;

		// Token: 0x04000EA5 RID: 3749
		private readonly TreeNode node;

		// Token: 0x04000EA6 RID: 3750
		private readonly Rectangle bounds;

		// Token: 0x04000EA7 RID: 3751
		private readonly TreeNodeStates state;

		// Token: 0x04000EA8 RID: 3752
		private bool drawDefault;
	}
}
