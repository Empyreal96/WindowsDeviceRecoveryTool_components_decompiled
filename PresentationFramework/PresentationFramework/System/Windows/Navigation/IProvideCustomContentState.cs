using System;

namespace System.Windows.Navigation
{
	/// <summary>Implemented by a class that needs to add custom state to the navigation history entry for content before the content is navigated away from.</summary>
	// Token: 0x02000303 RID: 771
	public interface IProvideCustomContentState
	{
		/// <summary>Returns an instance of a custom state class that is to be associated with content in navigation history.</summary>
		/// <returns>An instance of a custom <see cref="T:System.Windows.Navigation.CustomContentState" /> class that is to be associated with content in navigation history.</returns>
		// Token: 0x060028F3 RID: 10483
		CustomContentState GetContentState();
	}
}
