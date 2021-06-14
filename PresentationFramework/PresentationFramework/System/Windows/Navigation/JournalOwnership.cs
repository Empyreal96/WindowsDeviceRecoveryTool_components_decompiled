using System;

namespace System.Windows.Navigation
{
	/// <summary>Specifies whether a <see cref="T:System.Windows.Controls.Frame" /> uses its own journal. <see cref="T:System.Windows.Navigation.JournalOwnership" /> is used by the <see cref="P:System.Windows.Controls.Frame.JournalOwnership" /> property.</summary>
	// Token: 0x020002FD RID: 765
	[Serializable]
	public enum JournalOwnership
	{
		/// <summary>Whether or not this <see cref="T:System.Windows.Controls.Frame" /> will create and use its own journal depends on its parent.</summary>
		// Token: 0x04001BAC RID: 7084
		Automatic,
		/// <summary>The <see cref="T:System.Windows.Controls.Frame" /> maintains its own journal.</summary>
		// Token: 0x04001BAD RID: 7085
		OwnsJournal,
		/// <summary>The <see cref="T:System.Windows.Controls.Frame" /> uses the journal of the next available navigation host up the content tree, if available. Otherwise, navigation history is not maintained for the <see cref="T:System.Windows.Controls.Frame" />.</summary>
		// Token: 0x04001BAE RID: 7086
		UsesParentJournal
	}
}
