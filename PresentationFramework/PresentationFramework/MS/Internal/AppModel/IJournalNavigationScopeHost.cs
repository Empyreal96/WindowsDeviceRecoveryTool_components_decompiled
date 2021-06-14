using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000790 RID: 1936
	internal interface IJournalNavigationScopeHost : INavigatorBase
	{
		// Token: 0x17001CB9 RID: 7353
		// (get) Token: 0x060079B0 RID: 31152
		NavigationService NavigationService { get; }

		// Token: 0x060079B1 RID: 31153
		void VerifyContextAndObjectState();

		// Token: 0x060079B2 RID: 31154
		void OnJournalAvailable();

		// Token: 0x060079B3 RID: 31155
		bool GoBackOverride();

		// Token: 0x060079B4 RID: 31156
		bool GoForwardOverride();
	}
}
