using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that represent areas of a <see cref="T:System.Windows.Forms.TreeView" /> or <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
	// Token: 0x02000410 RID: 1040
	[Flags]
	[ComVisible(true)]
	public enum TreeViewHitTestLocations
	{
		/// <summary>A position in the client area of the <see cref="T:System.Windows.Forms.TreeView" /> control, but not on a node or a portion of a node.</summary>
		// Token: 0x040026A4 RID: 9892
		None = 1,
		/// <summary>A position within the bounds of an image contained on a <see cref="T:System.Windows.Forms.TreeView" /> or <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026A5 RID: 9893
		Image = 2,
		/// <summary>A position on the text portion of a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026A6 RID: 9894
		Label = 4,
		/// <summary>A position in the indentation area for a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026A7 RID: 9895
		Indent = 8,
		/// <summary>A position above the client portion of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		// Token: 0x040026A8 RID: 9896
		AboveClientArea = 256,
		/// <summary>A position below the client portion of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		// Token: 0x040026A9 RID: 9897
		BelowClientArea = 512,
		/// <summary>A position to the left of the client area of a <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		// Token: 0x040026AA RID: 9898
		LeftOfClientArea = 2048,
		/// <summary>A position to the right of the client area of the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
		// Token: 0x040026AB RID: 9899
		RightOfClientArea = 1024,
		/// <summary>A position to the right of the text area of a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026AC RID: 9900
		RightOfLabel = 32,
		/// <summary>A position within the bounds of a state image for a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026AD RID: 9901
		StateImage = 64,
		/// <summary>A position on the plus/minus area of a <see cref="T:System.Windows.Forms.TreeNode" />.</summary>
		// Token: 0x040026AE RID: 9902
		PlusMinus = 16
	}
}
