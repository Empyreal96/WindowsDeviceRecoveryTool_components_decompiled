using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TreeView.BeforeLabelEdit" /> and <see cref="E:System.Windows.Forms.TreeView.AfterLabelEdit" /> events.</summary>
	// Token: 0x020002F9 RID: 761
	public class NodeLabelEditEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NodeLabelEditEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		/// <param name="node">The tree node containing the text to edit. </param>
		// Token: 0x06002E09 RID: 11785 RVA: 0x000D6FB6 File Offset: 0x000D51B6
		public NodeLabelEditEventArgs(TreeNode node)
		{
			this.node = node;
			this.label = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NodeLabelEditEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.TreeNode" /> and the specified text with which to update the tree node label.</summary>
		/// <param name="node">The tree node containing the text to edit. </param>
		/// <param name="label">The new text to associate with the tree node. </param>
		// Token: 0x06002E0A RID: 11786 RVA: 0x000D6FCC File Offset: 0x000D51CC
		public NodeLabelEditEventArgs(TreeNode node, string label)
		{
			this.node = node;
			this.label = label;
		}

		/// <summary>Gets or sets a value indicating whether the edit has been canceled.</summary>
		/// <returns>
		///     <see langword="true" /> if the edit has been canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000D6FE2 File Offset: 0x000D51E2
		// (set) Token: 0x06002E0C RID: 11788 RVA: 0x000D6FEA File Offset: 0x000D51EA
		public bool CancelEdit
		{
			get
			{
				return this.cancelEdit;
			}
			set
			{
				this.cancelEdit = value;
			}
		}

		/// <summary>Gets the new text to associate with the tree node.</summary>
		/// <returns>The string value that represents the new <see cref="T:System.Windows.Forms.TreeNode" /> label or <see langword="null" /> if the user cancels the edit. </returns>
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002E0D RID: 11789 RVA: 0x000D6FF3 File Offset: 0x000D51F3
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		/// <summary>Gets the tree node containing the text to edit.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TreeNode" /> that represents the tree node containing the text to edit.</returns>
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000D6FFB File Offset: 0x000D51FB
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x04001D07 RID: 7431
		private readonly string label;

		// Token: 0x04001D08 RID: 7432
		private readonly TreeNode node;

		// Token: 0x04001D09 RID: 7433
		private bool cancelEdit;
	}
}
