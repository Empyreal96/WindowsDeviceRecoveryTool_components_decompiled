using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the initial position of a form.</summary>
	// Token: 0x02000254 RID: 596
	[ComVisible(true)]
	public enum FormStartPosition
	{
		/// <summary>The position of the form is determined by the <see cref="P:System.Windows.Forms.Control.Location" /> property.</summary>
		// Token: 0x04000F8E RID: 3982
		Manual,
		/// <summary>The form is centered on the current display, and has the dimensions specified in the form's size.</summary>
		// Token: 0x04000F8F RID: 3983
		CenterScreen,
		/// <summary>The form is positioned at the Windows default location and has the dimensions specified in the form's size.</summary>
		// Token: 0x04000F90 RID: 3984
		WindowsDefaultLocation,
		/// <summary>The form is positioned at the Windows default location and has the bounds determined by Windows default.</summary>
		// Token: 0x04000F91 RID: 3985
		WindowsDefaultBounds,
		/// <summary>The form is centered within the bounds of its parent form.</summary>
		// Token: 0x04000F92 RID: 3986
		CenterParent
	}
}
