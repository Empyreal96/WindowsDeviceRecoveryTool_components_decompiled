using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the border styles for a form.</summary>
	// Token: 0x0200024E RID: 590
	[ComVisible(true)]
	public enum FormBorderStyle
	{
		/// <summary>No border.</summary>
		// Token: 0x04000F83 RID: 3971
		None,
		/// <summary>A fixed, single-line border.</summary>
		// Token: 0x04000F84 RID: 3972
		FixedSingle,
		/// <summary>A fixed, three-dimensional border.</summary>
		// Token: 0x04000F85 RID: 3973
		Fixed3D,
		/// <summary>A thick, fixed dialog-style border.</summary>
		// Token: 0x04000F86 RID: 3974
		FixedDialog,
		/// <summary>A resizable border.</summary>
		// Token: 0x04000F87 RID: 3975
		Sizable,
		/// <summary>A tool window border that is not resizable. A tool window does not appear in the taskbar or in the window that appears when the user presses ALT+TAB. Although forms that specify <see cref="F:System.Windows.Forms.FormBorderStyle.FixedToolWindow" /> typically are not shown in the taskbar, you must also ensure that the <see cref="P:System.Windows.Forms.Form.ShowInTaskbar" /> property is set to <see langword="false" />, since its default value is <see langword="true" />.</summary>
		// Token: 0x04000F88 RID: 3976
		FixedToolWindow,
		/// <summary>A resizable tool window border. A tool window does not appear in the taskbar or in the window that appears when the user presses ALT+TAB.</summary>
		// Token: 0x04000F89 RID: 3977
		SizableToolWindow
	}
}
