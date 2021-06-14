using System;

namespace System.Windows.Controls
{
	/// <summary> Defines the different roles that a <see cref="T:System.Windows.Controls.MenuItem" /> can have. </summary>
	// Token: 0x02000502 RID: 1282
	public enum MenuItemRole
	{
		/// <summary> Top-level menu item that can invoke commands. </summary>
		// Token: 0x04002C83 RID: 11395
		TopLevelItem,
		/// <summary> Header for top-level menus. </summary>
		// Token: 0x04002C84 RID: 11396
		TopLevelHeader,
		/// <summary> Menu item in a submenu that can invoke commands. </summary>
		// Token: 0x04002C85 RID: 11397
		SubmenuItem,
		/// <summary> Header for a submenu. </summary>
		// Token: 0x04002C86 RID: 11398
		SubmenuHeader
	}
}
