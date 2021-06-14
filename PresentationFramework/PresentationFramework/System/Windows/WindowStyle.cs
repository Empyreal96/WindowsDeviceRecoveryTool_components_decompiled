using System;

namespace System.Windows
{
	/// <summary>Specifies the type of border that a <see cref="T:System.Windows.Window" /> has. Used by the <see cref="P:System.Windows.Window.WindowStyle" /> property.</summary>
	// Token: 0x0200013C RID: 316
	public enum WindowStyle
	{
		/// <summary>Only the client area is visible - the title bar and border are not shown. A <see cref="T:System.Windows.Navigation.NavigationWindow" /> with a <see cref="P:System.Windows.Window.WindowStyle" /> of <see cref="F:System.Windows.WindowStyle.None" /> will still display the navigation user interface (UI).</summary>
		// Token: 0x04000B91 RID: 2961
		None,
		/// <summary>A window with a single border. This is the default value.</summary>
		// Token: 0x04000B92 RID: 2962
		SingleBorderWindow,
		/// <summary>A window with a 3-D border.</summary>
		// Token: 0x04000B93 RID: 2963
		ThreeDBorderWindow,
		/// <summary>A fixed tool window.</summary>
		// Token: 0x04000B94 RID: 2964
		ToolWindow
	}
}
