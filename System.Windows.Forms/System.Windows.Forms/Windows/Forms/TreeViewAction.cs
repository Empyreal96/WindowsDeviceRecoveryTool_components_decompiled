using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the action that raised a <see cref="T:System.Windows.Forms.TreeViewEventArgs" /> event.</summary>
	// Token: 0x02000409 RID: 1033
	public enum TreeViewAction
	{
		/// <summary>The action that caused the event is unknown.</summary>
		// Token: 0x04002694 RID: 9876
		Unknown,
		/// <summary>The event was caused by a keystroke.</summary>
		// Token: 0x04002695 RID: 9877
		ByKeyboard,
		/// <summary>The event was caused by a mouse operation.</summary>
		// Token: 0x04002696 RID: 9878
		ByMouse,
		/// <summary>The event was caused by the <see cref="T:System.Windows.Forms.TreeNode" /> collapsing.</summary>
		// Token: 0x04002697 RID: 9879
		Collapse,
		/// <summary>The event was caused by the <see cref="T:System.Windows.Forms.TreeNode" /> expanding.</summary>
		// Token: 0x04002698 RID: 9880
		Expand
	}
}
