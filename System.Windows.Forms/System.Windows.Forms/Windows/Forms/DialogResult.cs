using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies identifiers to indicate the return value of a dialog box.</summary>
	// Token: 0x0200021D RID: 541
	[ComVisible(true)]
	public enum DialogResult
	{
		/// <summary>
		///     <see langword="Nothing" /> is returned from the dialog box. This means that the modal dialog continues running.</summary>
		// Token: 0x04000E2F RID: 3631
		None,
		/// <summary>The dialog box return value is <see langword="OK" /> (usually sent from a button labeled OK).</summary>
		// Token: 0x04000E30 RID: 3632
		OK,
		/// <summary>The dialog box return value is <see langword="Cancel" /> (usually sent from a button labeled Cancel).</summary>
		// Token: 0x04000E31 RID: 3633
		Cancel,
		/// <summary>The dialog box return value is <see langword="Abort" /> (usually sent from a button labeled Abort).</summary>
		// Token: 0x04000E32 RID: 3634
		Abort,
		/// <summary>The dialog box return value is <see langword="Retry" /> (usually sent from a button labeled Retry).</summary>
		// Token: 0x04000E33 RID: 3635
		Retry,
		/// <summary>The dialog box return value is <see langword="Ignore" /> (usually sent from a button labeled Ignore).</summary>
		// Token: 0x04000E34 RID: 3636
		Ignore,
		/// <summary>The dialog box return value is <see langword="Yes" /> (usually sent from a button labeled Yes).</summary>
		// Token: 0x04000E35 RID: 3637
		Yes,
		/// <summary>The dialog box return value is <see langword="No" /> (usually sent from a button labeled No).</summary>
		// Token: 0x04000E36 RID: 3638
		No
	}
}
