using System;

namespace System.Windows.Forms
{
	/// <summary>Contains information about an area of a <see cref="T:System.Windows.Forms.TreeView" /> control or a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
	// Token: 0x0200040F RID: 1039
	public class TreeViewHitTestInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeViewHitTestInfo" /> class. </summary>
		/// <param name="hitNode">The tree node located at the position indicated by the hit test.</param>
		/// <param name="hitLocation">One of the <see cref="T:System.Windows.Forms.TreeViewHitTestLocations" /> values.</param>
		// Token: 0x06004753 RID: 18259 RVA: 0x0013091C File Offset: 0x0012EB1C
		public TreeViewHitTestInfo(TreeNode hitNode, TreeViewHitTestLocations hitLocation)
		{
			this.node = hitNode;
			this.loc = hitLocation;
		}

		/// <summary>Gets the location of a hit test on a <see cref="T:System.Windows.Forms.TreeView" /> control, in relation to the <see cref="T:System.Windows.Forms.TreeView" /> and the nodes it contains.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TreeViewHitTestLocations" /> values.</returns>
		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x06004754 RID: 18260 RVA: 0x00130932 File Offset: 0x0012EB32
		public TreeViewHitTestLocations Location
		{
			get
			{
				return this.loc;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TreeNode" /> at the position indicated by a hit test of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> at the position indicated by a hit test of a <see cref="T:System.Windows.Forms.TreeView" /> control.</returns>
		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x06004755 RID: 18261 RVA: 0x0013093A File Offset: 0x0012EB3A
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x040026A1 RID: 9889
		private TreeViewHitTestLocations loc;

		// Token: 0x040026A2 RID: 9890
		private TreeNode node;
	}
}
