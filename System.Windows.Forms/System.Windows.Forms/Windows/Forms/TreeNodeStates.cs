using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that represent the possible states of a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
	// Token: 0x02000407 RID: 1031
	[Flags]
	public enum TreeNodeStates
	{
		/// <summary>The node is checked.</summary>
		// Token: 0x04002649 RID: 9801
		Checked = 8,
		/// <summary>The node is in its default state.</summary>
		// Token: 0x0400264A RID: 9802
		Default = 32,
		/// <summary>The node has focus.</summary>
		// Token: 0x0400264B RID: 9803
		Focused = 16,
		/// <summary>The node is disabled.</summary>
		// Token: 0x0400264C RID: 9804
		Grayed = 2,
		/// <summary>The node is hot. This state occurs when the <see cref="P:System.Windows.Forms.TreeView.HotTracking" /> property is set to <see langword="true" /> and the mouse pointer is over the node.</summary>
		// Token: 0x0400264D RID: 9805
		Hot = 64,
		/// <summary>The node in an indeterminate state.</summary>
		// Token: 0x0400264E RID: 9806
		Indeterminate = 256,
		/// <summary>The node is marked.</summary>
		// Token: 0x0400264F RID: 9807
		Marked = 128,
		/// <summary>The node is selected.</summary>
		// Token: 0x04002650 RID: 9808
		Selected = 1,
		/// <summary>The node should indicate a keyboard shortcut.</summary>
		// Token: 0x04002651 RID: 9809
		ShowKeyboardCues = 512
	}
}
