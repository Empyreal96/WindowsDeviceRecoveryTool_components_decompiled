using System;

namespace System.Windows.Navigation
{
	/// <summary>Specifies the position in navigation history of a piece of content with respect to current content. <see cref="T:System.Windows.Navigation.JournalEntryPosition" /> is used by <see cref="T:System.Windows.Navigation.JournalEntryUnifiedViewConverter" />.</summary>
	// Token: 0x02000306 RID: 774
	public enum JournalEntryPosition
	{
		/// <summary>Content is in back navigation history relative to current content.</summary>
		// Token: 0x04001BC8 RID: 7112
		Back,
		/// <summary>Content is the current content.</summary>
		// Token: 0x04001BC9 RID: 7113
		Current,
		/// <summary>Content is in forward navigation history with respect to current content.</summary>
		// Token: 0x04001BCA RID: 7114
		Forward
	}
}
