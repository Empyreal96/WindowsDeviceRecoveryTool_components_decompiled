using System;
using System.Collections;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MS.Internal.KnownBoxes;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078F RID: 1935
	internal class JournalNavigationScope : DependencyObject, INavigator, INavigatorBase
	{
		// Token: 0x0600797E RID: 31102 RVA: 0x002276EF File Offset: 0x002258EF
		internal JournalNavigationScope(IJournalNavigationScopeHost host)
		{
			this._host = host;
			this._rootNavSvc = host.NavigationService;
		}

		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x0600797F RID: 31103 RVA: 0x0022770A File Offset: 0x0022590A
		// (set) Token: 0x06007980 RID: 31104 RVA: 0x00227717 File Offset: 0x00225917
		public Uri Source
		{
			get
			{
				return this._host.Source;
			}
			set
			{
				this._host.Source = value;
			}
		}

		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x06007981 RID: 31105 RVA: 0x00227725 File Offset: 0x00225925
		public Uri CurrentSource
		{
			get
			{
				return this._host.CurrentSource;
			}
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x06007982 RID: 31106 RVA: 0x00227732 File Offset: 0x00225932
		// (set) Token: 0x06007983 RID: 31107 RVA: 0x0022773F File Offset: 0x0022593F
		public object Content
		{
			get
			{
				return this._host.Content;
			}
			set
			{
				this._host.Content = value;
			}
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x0022774D File Offset: 0x0022594D
		public bool Navigate(Uri source)
		{
			return this._host.Navigate(source);
		}

		// Token: 0x06007985 RID: 31109 RVA: 0x0022775B File Offset: 0x0022595B
		public bool Navigate(Uri source, object extraData)
		{
			return this._host.Navigate(source, extraData);
		}

		// Token: 0x06007986 RID: 31110 RVA: 0x0022776A File Offset: 0x0022596A
		public bool Navigate(object content)
		{
			return this._host.Navigate(content);
		}

		// Token: 0x06007987 RID: 31111 RVA: 0x00227778 File Offset: 0x00225978
		public bool Navigate(object content, object extraData)
		{
			return this._host.Navigate(content, extraData);
		}

		// Token: 0x06007988 RID: 31112 RVA: 0x00227787 File Offset: 0x00225987
		public void StopLoading()
		{
			this._host.StopLoading();
		}

		// Token: 0x06007989 RID: 31113 RVA: 0x00227794 File Offset: 0x00225994
		public void Refresh()
		{
			this._host.Refresh();
		}

		// Token: 0x14000165 RID: 357
		// (add) Token: 0x0600798A RID: 31114 RVA: 0x002277A1 File Offset: 0x002259A1
		// (remove) Token: 0x0600798B RID: 31115 RVA: 0x002277AF File Offset: 0x002259AF
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._host.Navigating += value;
			}
			remove
			{
				this._host.Navigating -= value;
			}
		}

		// Token: 0x14000166 RID: 358
		// (add) Token: 0x0600798C RID: 31116 RVA: 0x002277BD File Offset: 0x002259BD
		// (remove) Token: 0x0600798D RID: 31117 RVA: 0x002277CB File Offset: 0x002259CB
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._host.NavigationProgress += value;
			}
			remove
			{
				this._host.NavigationProgress -= value;
			}
		}

		// Token: 0x14000167 RID: 359
		// (add) Token: 0x0600798E RID: 31118 RVA: 0x002277D9 File Offset: 0x002259D9
		// (remove) Token: 0x0600798F RID: 31119 RVA: 0x002277E7 File Offset: 0x002259E7
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				this._host.NavigationFailed += value;
			}
			remove
			{
				this._host.NavigationFailed -= value;
			}
		}

		// Token: 0x14000168 RID: 360
		// (add) Token: 0x06007990 RID: 31120 RVA: 0x002277F5 File Offset: 0x002259F5
		// (remove) Token: 0x06007991 RID: 31121 RVA: 0x00227803 File Offset: 0x00225A03
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._host.Navigated += value;
			}
			remove
			{
				this._host.Navigated -= value;
			}
		}

		// Token: 0x14000169 RID: 361
		// (add) Token: 0x06007992 RID: 31122 RVA: 0x00227811 File Offset: 0x00225A11
		// (remove) Token: 0x06007993 RID: 31123 RVA: 0x0022781F File Offset: 0x00225A1F
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._host.LoadCompleted += value;
			}
			remove
			{
				this._host.LoadCompleted -= value;
			}
		}

		// Token: 0x1400016A RID: 362
		// (add) Token: 0x06007994 RID: 31124 RVA: 0x0022782D File Offset: 0x00225A2D
		// (remove) Token: 0x06007995 RID: 31125 RVA: 0x0022783B File Offset: 0x00225A3B
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._host.NavigationStopped += value;
			}
			remove
			{
				this._host.NavigationStopped -= value;
			}
		}

		// Token: 0x1400016B RID: 363
		// (add) Token: 0x06007996 RID: 31126 RVA: 0x00227849 File Offset: 0x00225A49
		// (remove) Token: 0x06007997 RID: 31127 RVA: 0x00227857 File Offset: 0x00225A57
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._host.FragmentNavigation += value;
			}
			remove
			{
				this._host.FragmentNavigation -= value;
			}
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x06007998 RID: 31128 RVA: 0x00227865 File Offset: 0x00225A65
		public bool CanGoForward
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoForward;
			}
		}

		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x06007999 RID: 31129 RVA: 0x0022788F File Offset: 0x00225A8F
		public bool CanGoBack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoBack;
			}
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x002278BC File Offset: 0x00225ABC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GoForward()
		{
			if (!this.CanGoForward)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			if (!this._host.GoForwardOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginForwardNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x00227914 File Offset: 0x00225B14
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void GoBack()
		{
			if (!this.CanGoBack)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			if (!this._host.GoBackOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginBackNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00227969 File Offset: 0x00225B69
		public void AddBackEntry(CustomContentState state)
		{
			this._host.VerifyContextAndObjectState();
			this._rootNavSvc.AddBackEntry(state);
		}

		// Token: 0x0600799D RID: 31133 RVA: 0x00227982 File Offset: 0x00225B82
		public JournalEntry RemoveBackEntry()
		{
			this._host.VerifyContextAndObjectState();
			if (this._journal != null)
			{
				return this._journal.RemoveBackEntry();
			}
			return null;
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x0600799E RID: 31134 RVA: 0x002279A4 File Offset: 0x00225BA4
		public IEnumerable BackStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.BackStack;
			}
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x0600799F RID: 31135 RVA: 0x002279BC File Offset: 0x00225BBC
		public IEnumerable ForwardStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.ForwardStack;
			}
		}

		// Token: 0x060079A0 RID: 31136 RVA: 0x0001B7E3 File Offset: 0x000199E3
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this;
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x002279D4 File Offset: 0x00225BD4
		internal void EnsureJournal()
		{
			Journal journal = this.Journal;
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x002279E8 File Offset: 0x00225BE8
		internal bool CanInvokeJournalEntry(int entryId)
		{
			if (this._journal == null)
			{
				return false;
			}
			int num = this._journal.FindIndexForEntryWithId(entryId);
			if (num == -1)
			{
				return false;
			}
			JournalEntry entry = this._journal[num];
			return this._journal.IsNavigable(entry);
		}

		// Token: 0x060079A3 RID: 31139 RVA: 0x00227A2C File Offset: 0x00225C2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal bool NavigateToEntry(int index)
		{
			JournalEntry entry = this.Journal[index];
			return this.NavigateToEntry(entry);
		}

		// Token: 0x060079A4 RID: 31140 RVA: 0x00227A50 File Offset: 0x00225C50
		internal bool NavigateToEntry(JournalEntry entry)
		{
			if (entry == null)
			{
				return false;
			}
			if (!this.Journal.IsNavigable(entry))
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			NavigationMode navigationMode = this.Journal.GetNavigationMode(entry);
			bool flag = false;
			try
			{
				flag = entry.Navigate(navigationService.INavigatorHost, navigationMode);
			}
			finally
			{
				if (!flag)
				{
					this.AbortJournalNavigation();
				}
			}
			return flag;
		}

		// Token: 0x060079A5 RID: 31141 RVA: 0x00227AC0 File Offset: 0x00225CC0
		internal void AbortJournalNavigation()
		{
			if (this._journal != null)
			{
				this._journal.AbortJournalNavigation();
			}
		}

		// Token: 0x060079A6 RID: 31142 RVA: 0x00227AD5 File Offset: 0x00225CD5
		internal INavigatorBase FindTarget(string name)
		{
			return this._rootNavSvc.FindTarget(name);
		}

		// Token: 0x060079A7 RID: 31143 RVA: 0x00227AE3 File Offset: 0x00225CE3
		internal static void ClearDPValues(DependencyObject navigator)
		{
			navigator.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.BackStackPropertyKey, null);
			navigator.SetValue(JournalNavigationScope.ForwardStackPropertyKey, null);
		}

		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x060079A8 RID: 31144 RVA: 0x00227B1D File Offset: 0x00225D1D
		// (set) Token: 0x060079A9 RID: 31145 RVA: 0x00227B38 File Offset: 0x00225D38
		internal Journal Journal
		{
			get
			{
				if (this._journal == null)
				{
					this.Journal = new Journal();
				}
				return this._journal;
			}
			set
			{
				this._journal = value;
				this._journal.Filter = new JournalEntryFilter(this.IsEntryNavigable);
				this._journal.BackForwardStateChange += this.OnBackForwardStateChange;
				DependencyObject dependencyObject = (DependencyObject)this._host;
				dependencyObject.SetValue(JournalNavigationScope.BackStackPropertyKey, this._journal.BackStack);
				dependencyObject.SetValue(JournalNavigationScope.ForwardStackPropertyKey, this._journal.ForwardStack);
				this._host.OnJournalAvailable();
			}
		}

		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x060079AA RID: 31146 RVA: 0x00227BBD File Offset: 0x00225DBD
		internal NavigationService RootNavigationService
		{
			get
			{
				return this._rootNavSvc;
			}
		}

		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x060079AB RID: 31147 RVA: 0x00227BC5 File Offset: 0x00225DC5
		internal INavigatorBase NavigatorHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x060079AC RID: 31148 RVA: 0x00227BD0 File Offset: 0x00225DD0
		private void OnBackForwardStateChange(object sender, EventArgs e)
		{
			DependencyObject dependencyObject = (DependencyObject)this._host;
			bool flag = false;
			bool flag2 = this._journal.CanGoBack;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoBackProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			flag2 = this._journal.CanGoForward;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoForwardProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x00227C58 File Offset: 0x00225E58
		private bool IsEntryNavigable(JournalEntry entry)
		{
			if (entry == null || !entry.IsNavigable())
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			return navigationService != null && (navigationService.ContentId == entry.ContentId || entry.JEGroupState.GroupExitEntry == entry);
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x060079AE RID: 31150 RVA: 0x000C265B File Offset: 0x000C085B
		private bool InAppShutdown
		{
			get
			{
				return Application.IsShuttingDown;
			}
		}

		// Token: 0x04003987 RID: 14727
		private static readonly DependencyPropertyKey CanGoBackPropertyKey = DependencyProperty.RegisterReadOnly("CanGoBack", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003988 RID: 14728
		internal static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackPropertyKey.DependencyProperty;

		// Token: 0x04003989 RID: 14729
		private static readonly DependencyPropertyKey CanGoForwardPropertyKey = DependencyProperty.RegisterReadOnly("CanGoForward", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400398A RID: 14730
		internal static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardPropertyKey.DependencyProperty;

		// Token: 0x0400398B RID: 14731
		private static readonly DependencyPropertyKey BackStackPropertyKey = DependencyProperty.RegisterReadOnly("BackStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x0400398C RID: 14732
		internal static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackPropertyKey.DependencyProperty;

		// Token: 0x0400398D RID: 14733
		private static readonly DependencyPropertyKey ForwardStackPropertyKey = DependencyProperty.RegisterReadOnly("ForwardStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x0400398E RID: 14734
		internal static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackPropertyKey.DependencyProperty;

		// Token: 0x0400398F RID: 14735
		private IJournalNavigationScopeHost _host;

		// Token: 0x04003990 RID: 14736
		private NavigationService _rootNavSvc;

		// Token: 0x04003991 RID: 14737
		private Journal _journal;
	}
}
