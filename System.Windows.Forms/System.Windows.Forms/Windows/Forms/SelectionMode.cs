using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the selection behavior of a list box.</summary>
	// Token: 0x02000354 RID: 852
	[ComVisible(true)]
	public enum SelectionMode
	{
		/// <summary>No items can be selected.</summary>
		// Token: 0x040020AD RID: 8365
		None,
		/// <summary>Only one item can be selected.</summary>
		// Token: 0x040020AE RID: 8366
		One,
		/// <summary>Multiple items can be selected.</summary>
		// Token: 0x040020AF RID: 8367
		MultiSimple,
		/// <summary>Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections </summary>
		// Token: 0x040020B0 RID: 8368
		MultiExtended
	}
}
