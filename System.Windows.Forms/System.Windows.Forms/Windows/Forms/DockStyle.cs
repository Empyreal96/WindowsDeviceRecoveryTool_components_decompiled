using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	/// <summary>Specifies the position and manner in which a control is docked.</summary>
	// Token: 0x02000221 RID: 545
	[Editor("System.Windows.Forms.Design.DockEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum DockStyle
	{
		/// <summary>The control is not docked.</summary>
		// Token: 0x04000E49 RID: 3657
		None,
		/// <summary>The control's top edge is docked to the top of its containing control.</summary>
		// Token: 0x04000E4A RID: 3658
		Top,
		/// <summary>The control's bottom edge is docked to the bottom of its containing control.</summary>
		// Token: 0x04000E4B RID: 3659
		Bottom,
		/// <summary>The control's left edge is docked to the left edge of its containing control.</summary>
		// Token: 0x04000E4C RID: 3660
		Left,
		/// <summary>The control's right edge is docked to the right edge of its containing control.</summary>
		// Token: 0x04000E4D RID: 3661
		Right,
		/// <summary>All the control's edges are docked to the all edges of its containing control and sized appropriately.</summary>
		// Token: 0x04000E4E RID: 3662
		Fill
	}
}
