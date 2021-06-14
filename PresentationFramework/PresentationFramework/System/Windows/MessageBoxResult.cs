using System;

namespace System.Windows
{
	/// <summary>Specifies which message box button that a user clicks. <see cref="T:System.Windows.MessageBoxResult" /> is returned by the <see cref="Overload:System.Windows.MessageBox.Show" /> method.</summary>
	// Token: 0x020000D9 RID: 217
	public enum MessageBoxResult
	{
		/// <summary>The message box returns no result.</summary>
		// Token: 0x04000746 RID: 1862
		None,
		/// <summary>The result value of the message box is OK.</summary>
		// Token: 0x04000747 RID: 1863
		OK,
		/// <summary>The result value of the message box is Cancel.</summary>
		// Token: 0x04000748 RID: 1864
		Cancel,
		/// <summary>The result value of the message box is Yes.</summary>
		// Token: 0x04000749 RID: 1865
		Yes = 6,
		/// <summary>The result value of the message box is No.</summary>
		// Token: 0x0400074A RID: 1866
		No
	}
}
