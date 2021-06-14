using System;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.AppModel;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Utility;

namespace System.Windows.Navigation
{
	/// <summary>Represents a window that supports content navigation.</summary>
	// Token: 0x0200031E RID: 798
	[ContentProperty]
	[TemplatePart(Name = "PART_NavWinCP", Type = typeof(ContentPresenter))]
	public class NavigationWindow : Window, INavigator, INavigatorBase, INavigatorImpl, IDownloader, IJournalNavigationScopeHost, IUriContext
	{
		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Navigation.NavigationWindow" /> isolates external Extensible Application Markup Language (XAML) content within a partial trust security sandbox (with default Internet zone permission set).  </summary>
		/// <returns>
		///     <see langword="true" /> if content is isolated within a partial trust security sandbox; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">
		///         <see cref="P:System.Windows.Navigation.NavigationWindow.SandboxExternalContent" /> is set when an application is executing in partial trust.</exception>
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06002A0C RID: 10764 RVA: 0x000C1D4C File Offset: 0x000BFF4C
		// (set) Token: 0x06002A0D RID: 10765 RVA: 0x000C1D60 File Offset: 0x000BFF60
		public bool SandboxExternalContent
		{
			get
			{
				return (bool)base.GetValue(NavigationWindow.SandboxExternalContentProperty);
			}
			set
			{
				bool value2 = value;
				SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref value2);
				base.SetValue(NavigationWindow.SandboxExternalContentProperty, value2);
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000C1D84 File Offset: 0x000BFF84
		private static void OnSandboxExternalContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			bool flag = (bool)e.NewValue;
			SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref flag);
			if (flag && !(bool)e.OldValue)
			{
				navigationWindow.NavigationService.Refresh();
			}
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000C1DC8 File Offset: 0x000BFFC8
		private static object CoerceSandBoxExternalContentValue(DependencyObject d, object value)
		{
			bool flag = (bool)value;
			SecurityHelper.ThrowExceptionIfSettingTrueInPartialTrust(ref flag);
			return flag;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000C1DEC File Offset: 0x000BFFEC
		static NavigationWindow()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(typeof(NavigationWindow)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(NavigationWindow.CoerceContent)));
			NavigationWindow.SandboxExternalContentProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(new PropertyChangedCallback(NavigationWindow.OnSandboxExternalContentPropertyChanged), new CoerceValueCallback(NavigationWindow.CoerceSandBoxExternalContentValue)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseBack, new ExecutedRoutedEventHandler(NavigationWindow.OnGoBack), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryGoBack)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseForward, new ExecutedRoutedEventHandler(NavigationWindow.OnGoForward), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryGoForward)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.NavigateJournal, new ExecutedRoutedEventHandler(NavigationWindow.OnNavigateJournal)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.Refresh, new ExecutedRoutedEventHandler(NavigationWindow.OnRefresh), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryRefresh)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseStop, new ExecutedRoutedEventHandler(NavigationWindow.OnBrowseStop), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryBrowseStop)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Navigation.NavigationWindow" /> class. </summary>
		// Token: 0x06002A11 RID: 10769 RVA: 0x000C2045 File Offset: 0x000C0245
		public NavigationWindow()
		{
			this.Initialize();
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000C2053 File Offset: 0x000C0253
		[SecurityCritical]
		internal NavigationWindow(bool inRbw) : base(inRbw)
		{
			this.Initialize();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000C2062 File Offset: 0x000C0262
		private void Initialize()
		{
			this._navigationService = new NavigationService(this);
			this._navigationService.BPReady += this.OnBPReady;
			this._JNS = new JournalNavigationScope(this);
			this._fFramelet = false;
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06002A14 RID: 10772 RVA: 0x000C209A File Offset: 0x000C029A
		NavigationService IDownloader.Downloader
		{
			get
			{
				return this._navigationService;
			}
		}

		/// <summary>Navigates asynchronously to content that is specified by a uniform resource identifier (URI).</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A15 RID: 10773 RVA: 0x000C20A2 File Offset: 0x000C02A2
		public bool Navigate(Uri source)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(source);
		}

		/// <summary>Navigates asynchronously to source content located at a uniform resource identifier (URI), and pass an object that contains data to be used for processing during navigation.</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <param name="extraData">A <see cref="T:System.Object" /> that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A16 RID: 10774 RVA: 0x000C20B6 File Offset: 0x000C02B6
		public bool Navigate(Uri source, object extraData)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(source, extraData);
		}

		/// <summary>Navigates asynchronously to content that is contained by an object.</summary>
		/// <param name="content">An <see cref="T:System.Object" /> that contains the content to navigate to.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A17 RID: 10775 RVA: 0x000C20CB File Offset: 0x000C02CB
		public bool Navigate(object content)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(content);
		}

		/// <summary>Navigates asynchronously to content that is contained by an object, and passes an object that contains data to be used for processing during navigation.</summary>
		/// <param name="content">An <see cref="T:System.Object" /> that contains the content to navigate to.</param>
		/// <param name="extraData">A <see cref="T:System.Object" /> that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A18 RID: 10776 RVA: 0x000C20DF File Offset: 0x000C02DF
		public bool Navigate(object content, object extraData)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(content, extraData);
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000C20F4 File Offset: 0x000C02F4
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this._JNS;
		}

		/// <summary>Navigates to the most recent item in forward navigation history.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Navigation.NavigationWindow.GoForward" /> is called when there are no entries in forward navigation history.</exception>
		// Token: 0x06002A1A RID: 10778 RVA: 0x000C20FC File Offset: 0x000C02FC
		public void GoForward()
		{
			this._JNS.GoForward();
		}

		/// <summary>Navigates to the most recent item in back navigation history.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Navigation.NavigationWindow.GoBack" /> is called when there are no entries in back navigation history.</exception>
		// Token: 0x06002A1B RID: 10779 RVA: 0x000C2109 File Offset: 0x000C0309
		public void GoBack()
		{
			this._JNS.GoBack();
		}

		/// <summary>Stops further downloading of content for the current navigation request.</summary>
		// Token: 0x06002A1C RID: 10780 RVA: 0x000C2116 File Offset: 0x000C0316
		public void StopLoading()
		{
			base.VerifyContextAndObjectState();
			if (this.InAppShutdown)
			{
				return;
			}
			this.NavigationService.StopLoading();
		}

		/// <summary>Reloads the current content.</summary>
		// Token: 0x06002A1D RID: 10781 RVA: 0x000C2132 File Offset: 0x000C0332
		public void Refresh()
		{
			base.VerifyContextAndObjectState();
			if (this.InAppShutdown)
			{
				return;
			}
			this.NavigationService.Refresh();
		}

		/// <summary>Adds an entry to back navigation history that contains a <see cref="T:System.Windows.Navigation.CustomContentState" /> object.</summary>
		/// <param name="state">A <see cref="T:System.Windows.Navigation.CustomContentState" /> object that represents application-defined state that is associated with a specific piece of content.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="state" /> is <see langword="null" />, and a <see cref="T:System.Windows.Navigation.CustomContentState" /> object isn't returned from <see cref="M:System.Windows.Navigation.IProvideCustomContentState.GetContentState" />.</exception>
		// Token: 0x06002A1E RID: 10782 RVA: 0x000C214E File Offset: 0x000C034E
		public void AddBackEntry(CustomContentState state)
		{
			base.VerifyContextAndObjectState();
			this.NavigationService.AddBackEntry(state);
		}

		/// <summary>Removes the most recent journal entry from back history.</summary>
		/// <returns>The most recent <see cref="T:System.Windows.Navigation.JournalEntry" /> in back navigation history, if there is one.</returns>
		// Token: 0x06002A1F RID: 10783 RVA: 0x000C2162 File Offset: 0x000C0362
		public JournalEntry RemoveBackEntry()
		{
			return this._JNS.RemoveBackEntry();
		}

		/// <summary>Gets or sets the base uniform resource identifier (URI) of the current context.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06002A20 RID: 10784 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06002A21 RID: 10785 RVA: 0x000C2181 File Offset: 0x000C0381
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		/// <summary>Called when the template generation for the visual tree is created.</summary>
		// Token: 0x06002A22 RID: 10786 RVA: 0x000C2190 File Offset: 0x000C0390
		[SecurityCritical]
		public override void OnApplyTemplate()
		{
			base.VerifyContextAndObjectState();
			base.OnApplyTemplate();
			FrameworkElement frameworkElement = this.GetVisualChild(0) as FrameworkElement;
			if (this._navigationService != null)
			{
				this._navigationService.VisualTreeAvailable(frameworkElement);
			}
			if (frameworkElement != null && frameworkElement.Name == "NavigationBarRoot")
			{
				if (!this._fFramelet)
				{
					this._fFramelet = true;
					return;
				}
			}
			else if (this._fFramelet)
			{
				this._fFramelet = false;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002A23 RID: 10787 RVA: 0x000C21FE File Offset: 0x000C03FE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool ShouldSerializeContent()
		{
			return this._navigationService == null || !this._navigationService.CanReloadFromUri;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Navigation.NavigationService" /> that is used by this <see cref="T:System.Windows.Navigation.NavigationWindow" /> to provide navigation services to its content.</summary>
		/// <returns>The navigation service used by this <see cref="T:System.Windows.Navigation.NavigationWindow" />.</returns>
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06002A24 RID: 10788 RVA: 0x000C2218 File Offset: 0x000C0418
		public NavigationService NavigationService
		{
			get
			{
				base.VerifyContextAndObjectState();
				return this._navigationService;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IEnumerable" /> that you use to enumerate the entries in back navigation history for a <see cref="T:System.Windows.Navigation.NavigationWindow" />.  </summary>
		/// <returns>
		///     <see cref="T:System.Collections.IEnumerable" /> if at least one entry has been added to back navigation history, or <see langword="null" /> if there are not entries or the <see cref="T:System.Windows.Navigation.NavigationWindow" /> does not own its own navigation history.</returns>
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x000C2226 File Offset: 0x000C0426
		public IEnumerable BackStack
		{
			get
			{
				return this._JNS.BackStack;
			}
		}

		/// <summary>Gets an <see cref="T:System.Collections.IEnumerable" /> that you use to enumerate the entries in back navigation history for a <see cref="T:System.Windows.Navigation.NavigationWindow" />.  </summary>
		/// <returns>
		///     <see cref="T:System.Collections.IEnumerable" /> if at least one entry has been added to forward navigation history, or null if there are no entries or the <see cref="T:System.Windows.Navigation.NavigationWindow" /> does not own its own navigation history.</returns>
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06002A26 RID: 10790 RVA: 0x000C2233 File Offset: 0x000C0433
		public IEnumerable ForwardStack
		{
			get
			{
				return this._JNS.ForwardStack;
			}
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Navigation.NavigationWindow" /> shows its navigation UI.  </summary>
		/// <returns>
		///     <see langword="true" /> if the navigation UI is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x000C2240 File Offset: 0x000C0440
		// (set) Token: 0x06002A28 RID: 10792 RVA: 0x000C2258 File Offset: 0x000C0458
		public bool ShowsNavigationUI
		{
			get
			{
				base.VerifyContextAndObjectState();
				return (bool)base.GetValue(NavigationWindow.ShowsNavigationUIProperty);
			}
			set
			{
				base.VerifyContextAndObjectState();
				base.SetValue(NavigationWindow.ShowsNavigationUIProperty, value);
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000C226C File Offset: 0x000C046C
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			if (!navigationWindow._sourceUpdatedFromNavService)
			{
				Uri uriToNavigate = BindUriHelper.GetUriToNavigate(navigationWindow, d.GetValue(BaseUriHelper.BaseUriProperty) as Uri, (Uri)e.NewValue);
				navigationWindow._navigationService.Navigate(uriToNavigate, null, false, true);
			}
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000C22BC File Offset: 0x000C04BC
		void INavigatorImpl.OnSourceUpdatedFromNavService(bool journalOrCancel)
		{
			try
			{
				this._sourceUpdatedFromNavService = true;
				base.SetCurrentValueInternal(NavigationWindow.SourceProperty, this._navigationService.Source);
			}
			finally
			{
				this._sourceUpdatedFromNavService = false;
			}
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to.  </summary>
		/// <returns>The URI for the current content, or the content that is currently being navigated to.</returns>
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002A2B RID: 10795 RVA: 0x000C2300 File Offset: 0x000C0500
		// (set) Token: 0x06002A2C RID: 10796 RVA: 0x000C2312 File Offset: 0x000C0512
		[DefaultValue(null)]
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(NavigationWindow.SourceProperty);
			}
			set
			{
				base.SetValue(NavigationWindow.SourceProperty, value);
			}
		}

		/// <summary>Gets the uniform resource identifier (URI) of the content that was last navigated to.</summary>
		/// <returns>The URI for the content that was last navigated to, if navigated to by using a URI; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x000C2320 File Offset: 0x000C0520
		public Uri CurrentSource
		{
			get
			{
				base.VerifyContextAndObjectState();
				if (this._navigationService != null)
				{
					return this._navigationService.CurrentSource;
				}
				return null;
			}
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in forward navigation history.  </summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in forward navigation history; <see langword="false" /> if there are no entries in forward navigation history, or the <see cref="T:System.Windows.Navigation.NavigationWindow" /> does not own its own navigation history.</returns>
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000C233D File Offset: 0x000C053D
		public bool CanGoForward
		{
			get
			{
				return this._JNS.CanGoForward;
			}
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in back navigation history.  </summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in back navigation history; <see langword="false" /> if there are no entries in back navigation history or the <see cref="T:System.Windows.Navigation.NavigationWindow" /> does not own its own navigation history.</returns>
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000C234A File Offset: 0x000C054A
		public bool CanGoBack
		{
			get
			{
				return this._JNS.CanGoBack;
			}
		}

		/// <summary>Occurs when a new navigation is requested.</summary>
		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06002A30 RID: 10800 RVA: 0x000C2357 File Offset: 0x000C0557
		// (remove) Token: 0x06002A31 RID: 10801 RVA: 0x000C236B File Offset: 0x000C056B
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigating += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigating -= value;
			}
		}

		/// <summary>Occurs periodically during a download to provide navigation progress information.</summary>
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06002A32 RID: 10802 RVA: 0x000C237F File Offset: 0x000C057F
		// (remove) Token: 0x06002A33 RID: 10803 RVA: 0x000C2393 File Offset: 0x000C0593
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationProgress += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationProgress -= value;
			}
		}

		/// <summary>Occurs when an error is raised while navigating to the requested content.</summary>
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002A34 RID: 10804 RVA: 0x000C23A7 File Offset: 0x000C05A7
		// (remove) Token: 0x06002A35 RID: 10805 RVA: 0x000C23BB File Offset: 0x000C05BB
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationFailed += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationFailed -= value;
			}
		}

		/// <summary>Occurs when the content that is being navigated to has been found, and is available from the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property, although it may not have completed loading.</summary>
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06002A36 RID: 10806 RVA: 0x000C23CF File Offset: 0x000C05CF
		// (remove) Token: 0x06002A37 RID: 10807 RVA: 0x000C23E3 File Offset: 0x000C05E3
		public event NavigatedEventHandler Navigated
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigated += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigated -= value;
			}
		}

		/// <summary>Occurs when content that was navigated to has been loaded, parsed, and has begun rendering.</summary>
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06002A38 RID: 10808 RVA: 0x000C23F7 File Offset: 0x000C05F7
		// (remove) Token: 0x06002A39 RID: 10809 RVA: 0x000C240B File Offset: 0x000C060B
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.LoadCompleted += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.LoadCompleted -= value;
			}
		}

		/// <summary>Occurs when the <see cref="M:System.Windows.Navigation.NavigationWindow.StopLoading" /> method is called, or when a new navigation is requested while a current navigation is in progress.</summary>
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002A3A RID: 10810 RVA: 0x000C241F File Offset: 0x000C061F
		// (remove) Token: 0x06002A3B RID: 10811 RVA: 0x000C2433 File Offset: 0x000C0633
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationStopped += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationStopped -= value;
			}
		}

		/// <summary>Occurs when navigation to a content fragment begins, which occurs immediately, if the desired fragment is in the current content, or after the source XAML content has been loaded, if the desired fragment is in different content.</summary>
		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06002A3C RID: 10812 RVA: 0x000C2447 File Offset: 0x000C0647
		// (remove) Token: 0x06002A3D RID: 10813 RVA: 0x000C245B File Offset: 0x000C065B
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.FragmentNavigation += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.FragmentNavigation -= value;
			}
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.NavigationWindowAutomationPeer" /> object for this <see cref="T:System.Windows.Navigation.NavigationWindow" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.NavigationWindowAutomationPeer" /> object for this <see cref="T:System.Windows.Navigation.NavigationWindow" />.</returns>
		// Token: 0x06002A3E RID: 10814 RVA: 0x000C246F File Offset: 0x000C066F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new NavigationWindowAutomationPeer(this);
		}

		/// <summary>Adds a child object.</summary>
		/// <param name="value">The child object to add.</param>
		/// <exception cref="T:System.InvalidOperationException">when this method is called. This prevents content from being added to <see cref="T:System.Windows.Navigation.NavigationWindow" /> using XAML.</exception>
		// Token: 0x06002A3F RID: 10815 RVA: 0x000C2477 File Offset: 0x000C0677
		protected override void AddChild(object value)
		{
			throw new InvalidOperationException(SR.Get("NoAddChild"));
		}

		/// <summary>Adds text to the object.</summary>
		/// <param name="text">The text to add to the object.</param>
		/// <exception cref="T:System.ArgumentException">if the text parameter value contains non-whitespace characters.</exception>
		// Token: 0x06002A40 RID: 10816 RVA: 0x0000B31C File Offset: 0x0000951C
		protected override void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Window.Closed" /> event.</summary>
		/// <param name="args">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002A41 RID: 10817 RVA: 0x000C2488 File Offset: 0x000C0688
		protected override void OnClosed(EventArgs args)
		{
			base.VerifyContextAndObjectState();
			base.OnClosed(args);
			if (this._navigationService != null)
			{
				this._navigationService.Dispose();
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000C24AA File Offset: 0x000C06AA
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			this._JNS.EnsureJournal();
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000C24BD File Offset: 0x000C06BD
		void IJournalNavigationScopeHost.VerifyContextAndObjectState()
		{
			base.VerifyContextAndObjectState();
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x00002137 File Offset: 0x00000337
		void IJournalNavigationScopeHost.OnJournalAvailable()
		{
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IJournalNavigationScopeHost.GoBackOverride()
		{
			return false;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IJournalNavigationScopeHost.GoForwardOverride()
		{
			return false;
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002A47 RID: 10823 RVA: 0x000C209A File Offset: 0x000C029A
		NavigationService IJournalNavigationScopeHost.NavigationService
		{
			get
			{
				return this._navigationService;
			}
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000C24C5 File Offset: 0x000C06C5
		Visual INavigatorImpl.FindRootViewer()
		{
			return NavigationHelper.FindRootViewer(this, "PART_NavWinCP");
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002A49 RID: 10825 RVA: 0x000C24D2 File Offset: 0x000C06D2
		internal Journal Journal
		{
			get
			{
				return this._JNS.Journal;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000C20F4 File Offset: 0x000C02F4
		internal JournalNavigationScope JournalNavigationScope
		{
			get
			{
				return this._JNS;
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000C24E0 File Offset: 0x000C06E0
		private static object CoerceContent(DependencyObject d, object value)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			if (navigationWindow.NavigationService.Content == value)
			{
				return value;
			}
			navigationWindow.Navigate(value);
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000C2511 File Offset: 0x000C0711
		private void OnBPReady(object sender, BPReadyEventArgs e)
		{
			base.Content = e.Content;
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000C2520 File Offset: 0x000C0720
		private static void OnGoBack(object sender, ExecutedRoutedEventArgs args)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			navigationWindow.GoBack();
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000C253C File Offset: 0x000C073C
		private static void OnQueryGoBack(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = navigationWindow.CanGoBack;
			e.ContinueRouting = !navigationWindow.CanGoBack;
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000C256C File Offset: 0x000C076C
		private static void OnGoForward(object sender, ExecutedRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			navigationWindow.GoForward();
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000C2588 File Offset: 0x000C0788
		private static void OnQueryGoForward(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = navigationWindow.CanGoForward;
			e.ContinueRouting = !navigationWindow.CanGoForward;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000C25B8 File Offset: 0x000C07B8
		private static void OnRefresh(object sender, ExecutedRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			navigationWindow.Refresh();
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000C25D4 File Offset: 0x000C07D4
		private static void OnQueryRefresh(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = (navigationWindow.Content != null);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000C25F8 File Offset: 0x000C07F8
		private static void OnBrowseStop(object sender, ExecutedRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			navigationWindow.StopLoading();
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000C2612 File Offset: 0x000C0812
		private static void OnQueryBrowseStop(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000C261C File Offset: 0x000C081C
		private static void OnNavigateJournal(object sender, ExecutedRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			FrameworkElement frameworkElement = e.Parameter as FrameworkElement;
			if (frameworkElement != null)
			{
				JournalEntry journalEntry = frameworkElement.DataContext as JournalEntry;
				if (journalEntry != null)
				{
					navigationWindow.JournalNavigationScope.NavigateToEntry(journalEntry);
				}
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x000C265B File Offset: 0x000C085B
		private bool InAppShutdown
		{
			get
			{
				return Application.IsShuttingDown;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x000C2662 File Offset: 0x000C0862
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return NavigationWindow._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.SandboxExternalContent" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.SandboxExternalContent" /> dependency property.</returns>
		// Token: 0x04001C2C RID: 7212
		public static readonly DependencyProperty SandboxExternalContentProperty = Frame.SandboxExternalContentProperty.AddOwner(typeof(NavigationWindow));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.ShowsNavigationUI" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.ShowsNavigationUI" /> dependency property.</returns>
		// Token: 0x04001C2D RID: 7213
		public static readonly DependencyProperty ShowsNavigationUIProperty = DependencyProperty.Register("ShowsNavigationUI", typeof(bool), typeof(NavigationWindow), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.BackStack" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.BackStack" /> dependency property.</returns>
		// Token: 0x04001C2E RID: 7214
		public static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackProperty.AddOwner(typeof(NavigationWindow));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.ForwardStack" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.ForwardStack" /> dependency property.</returns>
		// Token: 0x04001C2F RID: 7215
		public static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackProperty.AddOwner(typeof(NavigationWindow));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.CanGoBack" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.CanGoBack" /> dependency property.</returns>
		// Token: 0x04001C30 RID: 7216
		public static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackProperty.AddOwner(typeof(NavigationWindow));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.CanGoForward" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.CanGoForward" /> dependency property.</returns>
		// Token: 0x04001C31 RID: 7217
		public static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardProperty.AddOwner(typeof(NavigationWindow));

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.NavigationWindow.Source" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.NavigationWindow.Source" /> dependency property.</returns>
		// Token: 0x04001C32 RID: 7218
		public static readonly DependencyProperty SourceProperty = Frame.SourceProperty.AddOwner(typeof(NavigationWindow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(NavigationWindow.OnSourcePropertyChanged)));

		// Token: 0x04001C33 RID: 7219
		private NavigationService _navigationService;

		// Token: 0x04001C34 RID: 7220
		private JournalNavigationScope _JNS;

		// Token: 0x04001C35 RID: 7221
		private bool _sourceUpdatedFromNavService;

		// Token: 0x04001C36 RID: 7222
		private bool _fFramelet;

		// Token: 0x04001C37 RID: 7223
		private static DependencyObjectType _dType = DependencyObjectType.FromSystemTypeInternal(typeof(NavigationWindow));
	}
}
