using System;

namespace System.Windows.Navigation
{
	/// <summary>Specifies whether a <see cref="T:System.Windows.Controls.Frame" /> displays its navigation chrome. <see cref="T:System.Windows.Navigation.NavigationUIVisibility" /> is used by the <see cref="P:System.Windows.Controls.Frame.NavigationUIVisibility" /> property.</summary>
	// Token: 0x020002FE RID: 766
	public enum NavigationUIVisibility
	{
		/// <summary>The navigation chrome is visible when a <see cref="T:System.Windows.Controls.Frame" /> uses its own journal (see <see cref="P:System.Windows.Controls.Frame.JournalOwnership" />).</summary>
		// Token: 0x04001BB0 RID: 7088
		Automatic,
		/// <summary>The navigation chrome is visible.</summary>
		// Token: 0x04001BB1 RID: 7089
		Visible,
		/// <summary>The navigation chrome is not visible.</summary>
		// Token: 0x04001BB2 RID: 7090
		Hidden
	}
}
