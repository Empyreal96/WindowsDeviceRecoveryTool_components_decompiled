using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.NodeMouseHover" /> event. </summary>
	// Token: 0x02000405 RID: 1029
	[ComVisible(true)]
	public class TreeNodeMouseHoverEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeNodeMouseHoverEventArgs" /> class. </summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> the mouse pointer is currently resting on.</param>
		// Token: 0x06004675 RID: 18037 RVA: 0x0012CD2A File Offset: 0x0012AF2A
		public TreeNodeMouseHoverEventArgs(TreeNode node)
		{
			this.node = node;
		}

		/// <summary>Gets the node the mouse pointer is currently resting on.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> the mouse pointer is currently resting on.</returns>
		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x0012CD39 File Offset: 0x0012AF39
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x04002647 RID: 9799
		private readonly TreeNode node;
	}
}
