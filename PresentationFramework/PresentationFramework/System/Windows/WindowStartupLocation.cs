using System;

namespace System.Windows
{
	/// <summary>Specifies the position that a <see cref="T:System.Windows.Window" /> will be shown in when it is first opened. Used by the <see cref="P:System.Windows.Window.WindowStartupLocation" /> property.</summary>
	// Token: 0x0200013E RID: 318
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public enum WindowStartupLocation
	{
		/// <summary>The startup location of a <see cref="T:System.Windows.Window" /> is set from code, or defers to the default Windows location.</summary>
		// Token: 0x04000B9A RID: 2970
		Manual,
		/// <summary>The startup location of a <see cref="T:System.Windows.Window" /> is the center of the screen that contains the mouse cursor.</summary>
		// Token: 0x04000B9B RID: 2971
		CenterScreen,
		/// <summary>The startup location of a <see cref="T:System.Windows.Window" /> is the center of the <see cref="T:System.Windows.Window" /> that owns it, as specified by the <see cref="P:System.Windows.Window.Owner" /> property.</summary>
		// Token: 0x04000B9C RID: 2972
		CenterOwner
	}
}
