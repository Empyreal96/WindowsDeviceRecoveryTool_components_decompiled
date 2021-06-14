using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies whether the tabs in a tab control are owner-drawn (drawn by the parent window), or drawn by the operating system.</summary>
	// Token: 0x0200037B RID: 891
	public enum TabDrawMode
	{
		/// <summary>The tabs are drawn by the operating system, and are of the same size.</summary>
		// Token: 0x0400225A RID: 8794
		Normal,
		/// <summary>The tabs are drawn by the parent window, and are of the same size.</summary>
		// Token: 0x0400225B RID: 8795
		OwnerDrawFixed
	}
}
