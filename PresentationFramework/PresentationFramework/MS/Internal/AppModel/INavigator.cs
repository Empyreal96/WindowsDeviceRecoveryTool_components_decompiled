using System;
using System.Collections;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000781 RID: 1921
	internal interface INavigator : INavigatorBase
	{
		// Token: 0x0600793B RID: 31035
		JournalNavigationScope GetJournal(bool create);

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x0600793C RID: 31036
		bool CanGoForward { get; }

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x0600793D RID: 31037
		bool CanGoBack { get; }

		// Token: 0x0600793E RID: 31038
		void GoForward();

		// Token: 0x0600793F RID: 31039
		void GoBack();

		// Token: 0x06007940 RID: 31040
		void AddBackEntry(CustomContentState state);

		// Token: 0x06007941 RID: 31041
		JournalEntry RemoveBackEntry();

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x06007942 RID: 31042
		IEnumerable BackStack { get; }

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x06007943 RID: 31043
		IEnumerable ForwardStack { get; }
	}
}
