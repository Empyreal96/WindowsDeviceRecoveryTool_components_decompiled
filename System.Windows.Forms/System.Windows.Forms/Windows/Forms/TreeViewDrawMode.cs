using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that represent the ways a <see cref="T:System.Windows.Forms.TreeView" /> can be drawn.</summary>
	// Token: 0x0200040C RID: 1036
	public enum TreeViewDrawMode
	{
		/// <summary>The <see cref="T:System.Windows.Forms.TreeView" /> is drawn by the operating system.</summary>
		// Token: 0x0400269C RID: 9884
		Normal,
		/// <summary>The label portion of the <see cref="T:System.Windows.Forms.TreeView" /> nodes are drawn manually. Other node elements are drawn by the operating system, including icons, checkboxes, plus and minus signs, and lines connecting the nodes.</summary>
		// Token: 0x0400269D RID: 9885
		OwnerDrawText,
		/// <summary>All elements of a <see cref="T:System.Windows.Forms.TreeView" /> node are drawn manually, including icons, checkboxes, plus and minus signs, and lines connecting the nodes.</summary>
		// Token: 0x0400269E RID: 9886
		OwnerDrawAll
	}
}
