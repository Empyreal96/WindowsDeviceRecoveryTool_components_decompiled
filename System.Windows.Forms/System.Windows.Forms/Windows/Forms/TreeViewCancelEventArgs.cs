using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.BeforeCheck" />, <see cref="E:System.Windows.Forms.TreeView.BeforeCollapse" />, <see cref="E:System.Windows.Forms.TreeView.BeforeExpand" />, and <see cref="E:System.Windows.Forms.TreeView.BeforeSelect" /> events of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
	// Token: 0x0200040A RID: 1034
	public class TreeViewCancelEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs" /> class with the specified tree node, a value specifying whether the event is to be canceled, and the type of tree view action that raised the event.</summary>
		/// <param name="node">The <see cref="T:System.Windows.Forms.TreeNode" /> that the event is responding to. </param>
		/// <param name="cancel">
		///       <see langword="true" /> to cancel the event; otherwise, <see langword="false" />. </param>
		/// <param name="action">One of the <see cref="T:System.Windows.Forms.TreeViewAction" /> values indicating the type of action that raised the event. </param>
		// Token: 0x06004744 RID: 18244 RVA: 0x001308C0 File Offset: 0x0012EAC0
		public TreeViewCancelEventArgs(TreeNode node, bool cancel, TreeViewAction action) : base(cancel)
		{
			this.node = node;
			this.action = action;
		}

		/// <summary>Gets the tree node to be checked, expanded, collapsed, or selected.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TreeNode" /> to be checked, expanded, collapsed, or selected.</returns>
		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06004745 RID: 18245 RVA: 0x001308D7 File Offset: 0x0012EAD7
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		/// <summary>Gets the type of <see cref="T:System.Windows.Forms.TreeView" /> action that raised the event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TreeViewAction" /> values.</returns>
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x001308DF File Offset: 0x0012EADF
		public TreeViewAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04002699 RID: 9881
		private TreeNode node;

		// Token: 0x0400269A RID: 9882
		private TreeViewAction action;
	}
}
