using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the direction in which a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control is displayed relative to its parent control.</summary>
	// Token: 0x020003AE RID: 942
	public enum ToolStripDropDownDirection
	{
		/// <summary>Uses the mouse position to specify that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed above and to the left of its parent control.</summary>
		// Token: 0x040023D8 RID: 9176
		AboveLeft,
		/// <summary>Uses the mouse position to specify that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed above and to the right of its parent control.</summary>
		// Token: 0x040023D9 RID: 9177
		AboveRight,
		/// <summary>Uses the mouse position to specify that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed below and to the left of its parent control.</summary>
		// Token: 0x040023DA RID: 9178
		BelowLeft,
		/// <summary>Uses the mouse position to specify that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed below and to the right of its parent control.</summary>
		// Token: 0x040023DB RID: 9179
		BelowRight,
		/// <summary>Compensates for nested drop-down controls and specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed to the left of its parent control.</summary>
		// Token: 0x040023DC RID: 9180
		Left,
		/// <summary>Compensates for nested drop-down controls and specifies that the <see cref="T:System.Windows.Forms.ToolStripDropDown" /> is displayed to the right of its parent control.</summary>
		// Token: 0x040023DD RID: 9181
		Right,
		/// <summary>Compensates for nested drop-down controls and responds to the <see cref="T:System.Windows.Forms.RightToLeft" /> setting, specifying either <see cref="F:System.Windows.Forms.ToolStripDropDownDirection.Left" /> or <see cref="F:System.Windows.Forms.ToolStripDropDownDirection.Right" /> accordingly.</summary>
		// Token: 0x040023DE RID: 9182
		Default = 7
	}
}
