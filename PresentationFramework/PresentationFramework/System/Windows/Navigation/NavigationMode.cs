using System;

namespace System.Windows.Navigation
{
	/// <summary>Specifies the type of navigation that is taking place <see cref="T:System.Windows.Navigation.NavigationMode" /> is used by the <see cref="P:System.Windows.Navigation.NavigatingCancelEventArgs.NavigationMode" /> property.</summary>
	// Token: 0x0200030C RID: 780
	public enum NavigationMode : byte
	{
		/// <summary>Navigating to new content. This occurs when the Navigate method is called, or when Source property is set.</summary>
		// Token: 0x04001BE7 RID: 7143
		New,
		/// <summary>Navigating back to the most recent content in back navigation history. This occurs when the GoBack method is called.</summary>
		// Token: 0x04001BE8 RID: 7144
		Back,
		/// <summary>Navigating to the most recent content on forward navigation history. This occurs when the GoForward method is called.</summary>
		// Token: 0x04001BE9 RID: 7145
		Forward,
		/// <summary>Reloading the current content. This occurs when the Refresh method is called.</summary>
		// Token: 0x04001BEA RID: 7146
		Refresh
	}
}
