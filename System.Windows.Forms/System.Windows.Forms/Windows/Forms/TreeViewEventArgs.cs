using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.AfterCheck" />, <see cref="E:System.Windows.Forms.TreeView.AfterCollapse" />, <see cref="E:System.Windows.Forms.TreeView.AfterExpand" />, or <see cref="E:System.Windows.Forms.TreeView.AfterSelect" /> events of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
	// Token: 0x0200040D RID: 1037
	public class TreeViewEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> class for the specified tree node.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> that the event is responding to. </param>
		// Token: 0x0600474B RID: 18251 RVA: 0x001308E7 File Offset: 0x0012EAE7
		public TreeViewEventArgs(TreeNode node)
		{
			this.node = node;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> class for the specified tree node and with the specified type of action that raised the event.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> that the event is responding to. </param>
		/// <param name="action">The type of <see cref="T:System.Windows.Forms.TreeViewAction" /> that raised the event. </param>
		// Token: 0x0600474C RID: 18252 RVA: 0x001308F6 File Offset: 0x0012EAF6
		public TreeViewEventArgs(TreeNode node, TreeViewAction action)
		{
			this.node = node;
			this.action = action;
		}

		/// <summary>Gets the tree node that has been checked, expanded, collapsed, or selected.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> that has been checked, expanded, collapsed, or selected.</returns>
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600474D RID: 18253 RVA: 0x0013090C File Offset: 0x0012EB0C
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		/// <summary>Gets the type of action that raised the event.</summary>
		/// <returns>The type of <see cref="T:System.Windows.Forms.TreeViewAction" /> that raised the event.</returns>
		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600474E RID: 18254 RVA: 0x00130914 File Offset: 0x0012EB14
		public TreeViewAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x0400269F RID: 9887
		private TreeNode node;

		// Token: 0x040026A0 RID: 9888
		private TreeViewAction action;
	}
}
