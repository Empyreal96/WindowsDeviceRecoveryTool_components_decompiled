using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.NodeMouseClick" /> and <see cref="E:System.Windows.Forms.TreeView.NodeMouseDoubleClick" /> events. </summary>
	// Token: 0x02000401 RID: 1025
	public class TreeNodeMouseClickEventArgs : MouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs" /> class. </summary>
		/// <param name="node">The node that was clicked.</param>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> members.</param>
		/// <param name="clicks">The number of clicks that occurred.</param>
		/// <param name="x">The x-coordinate where the click occurred.</param>
		/// <param name="y">The y-coordinate where the click occurred.</param>
		// Token: 0x0600463D RID: 17981 RVA: 0x0012C231 File Offset: 0x0012A431
		public TreeNodeMouseClickEventArgs(TreeNode node, MouseButtons button, int clicks, int x, int y) : base(button, clicks, x, y, 0)
		{
			this.node = node;
		}

		/// <summary>Gets the node that was clicked.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that was clicked.</returns>
		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x0012C247 File Offset: 0x0012A447
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x04002643 RID: 9795
		private TreeNode node;
	}
}
