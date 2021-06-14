using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Navigation;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Navigation
{
	/// <summary>Contains methods, properties, and events to support navigation.</summary>
	// Token: 0x0200030E RID: 782
	public sealed class NavigationService : IContentContainer
	{
		// Token: 0x06002941 RID: 10561 RVA: 0x000BE474 File Offset: 0x000BC674
		internal NavigationService(INavigator nav)
		{
			this.INavigatorHost = nav;
			if (!(nav is NavigationWindow))
			{
				this.GuidId = Guid.NewGuid();
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
		private void ResetPendingNavigationState(NavigationStatus newState)
		{
			JournalNavigationScope journalScope = this.JournalScope;
			if (journalScope != null && journalScope.RootNavigationService != this)
			{
				journalScope.RootNavigationService.BytesRead -= this._bytesRead;
				journalScope.RootNavigationService.MaxBytes -= this._maxBytes;
			}
			this._navStatus = newState;
			this._bytesRead = 0L;
			this._maxBytes = 0L;
			this._navigateQueueItem = null;
			this._request = null;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000BE548 File Offset: 0x000BC748
		private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			string target = e.Target;
			Uri bpu = e.Uri;
			if (bpu != null && !bpu.IsAbsoluteUri)
			{
				DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
				IUriContext uriContext = dependencyObject as IUriContext;
				if (uriContext == null)
				{
					throw new Exception(SR.Get("MustImplementIUriContext", new object[]
					{
						typeof(IUriContext)
					}));
				}
				bpu = BindUriHelper.GetUriToNavigate(dependencyObject, uriContext.BaseUri, e.Uri);
			}
			INavigatorBase navigator = null;
			bool flag = true;
			if (!string.IsNullOrEmpty(target))
			{
				navigator = this.FindTarget(target);
				if (navigator == null && this.JournalScope != null)
				{
					navigator = this.JournalScope.FindTarget(target);
				}
				if (navigator == null)
				{
					NavigationWindow navigationWindow = this.FindNavigationWindow();
					if (navigationWindow != null)
					{
						navigator = NavigationService.FindTargetInNavigationWindow(navigationWindow, target);
					}
					if (navigator == null)
					{
						navigator = NavigationService.FindTargetInApplication(target);
						if (navigator != null)
						{
							flag = ((DispatcherObject)navigator).CheckAccess();
						}
					}
				}
			}
			else
			{
				navigator = this.INavigatorHost;
			}
			if (navigator != null)
			{
				if (flag)
				{
					navigator.Navigate(bpu);
					return;
				}
				((DispatcherObject)navigator).Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback((object unused) => navigator.Navigate(bpu)), null);
				return;
			}
			else
			{
				if (!Application.InBrowserHostedApp())
				{
					throw new ArgumentException(SR.Get("HyperLinkTargetNotFound"));
				}
				if (SecurityHelper.AreStringTypesEqual(bpu.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
				{
					bpu = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(bpu);
				}
				LaunchResult launchResult = AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(this.CurrentSource, bpu, target, true);
				if (launchResult == LaunchResult.NotLaunched)
				{
					throw new Exception(SR.Get("FailToNavigateUsingHyperlinkTarget"));
				}
				return;
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000BE73C File Offset: 0x000BC93C
		private static bool IsSameUri(Uri baseUri, Uri a, Uri b, bool withFragment)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(baseUri, a);
			Uri resolvedUri2 = BindUriHelper.GetResolvedUri(baseUri, b);
			bool flag = resolvedUri.Equals(resolvedUri2);
			if (flag && withFragment)
			{
				flag = (flag && string.Compare(resolvedUri.Fragment, resolvedUri2.Fragment, StringComparison.OrdinalIgnoreCase) == 0);
			}
			return flag;
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000BE7A0 File Offset: 0x000BC9A0
		private void NavigateToFragmentOrCustomContentState(Uri uri, object navState)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			JournalEntry journalEntry = null;
			if (navigateInfo != null)
			{
				journalEntry = navigateInfo.JournalEntry;
			}
			NavigationMode navigationMode = (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode;
			CustomJournalStateInternal rootViewerState = this.GetRootViewerState(JournalReason.FragmentNavigation);
			string elementId = (uri != null) ? BindUriHelper.GetFragment(uri) : null;
			bool flag = journalEntry != null && journalEntry.CustomContentState != null;
			bool flag2 = this.NavigateToFragment(elementId, !flag);
			if (navigationMode == NavigationMode.Back || navigationMode == NavigationMode.Forward || (flag2 && !NavigationService.IsSameUri(null, this._currentSource, uri, true)))
			{
				try
				{
					this._rootViewerStateToSave = rootViewerState;
					this.UpdateJournal(navigationMode, JournalReason.FragmentNavigation, journalEntry);
				}
				finally
				{
					this._rootViewerStateToSave = null;
				}
				Uri resolvedUri = BindUriHelper.GetResolvedUri(this._currentSource, uri);
				this._currentSource = resolvedUri;
				this._currentCleanSource = BindUriHelper.GetUriRelativeToPackAppBase(uri);
			}
			this._navStatus = NavigationStatus.Navigated;
			this.HandleNavigated(navState, false);
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000BE884 File Offset: 0x000BCA84
		private bool NavigateToFragment(string elementId, bool scrollToTopOnEmptyFragment)
		{
			if (this.FireFragmentNavigation(elementId))
			{
				return true;
			}
			if (!string.IsNullOrEmpty(elementId))
			{
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode((DependencyObject)this._bp, elementId);
				NavigationService.BringIntoView(dependencyObject);
				return dependencyObject != null;
			}
			if (!scrollToTopOnEmptyFragment)
			{
				return false;
			}
			this.ScrollContentToTop();
			return true;
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000BE8D0 File Offset: 0x000BCAD0
		private void ScrollContentToTop()
		{
			if (this._bp != null)
			{
				FrameworkElement frameworkElement = this._bp as FrameworkElement;
				if (frameworkElement != null)
				{
					IEnumerator logicalChildren = frameworkElement.LogicalChildren;
					if (logicalChildren != null && logicalChildren.MoveNext())
					{
						ScrollViewer scrollViewer = logicalChildren.Current as ScrollViewer;
						if (scrollViewer != null)
						{
							scrollViewer.ScrollToTop();
							return;
						}
					}
				}
				IInputElement inputElement = this._bp as IInputElement;
				if (inputElement != null && ScrollBar.ScrollToTopCommand.CanExecute(null, inputElement))
				{
					ScrollBar.ScrollToTopCommand.Execute(null, inputElement);
					return;
				}
				NavigationService.BringIntoView(this._bp as DependencyObject);
			}
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000BE958 File Offset: 0x000BCB58
		private static void BringIntoView(DependencyObject elem)
		{
			FrameworkElement frameworkElement = elem as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
				return;
			}
			FrameworkContentElement frameworkContentElement = elem as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.BringIntoView();
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000BE986 File Offset: 0x000BCB86
		private JournalNavigationScope EnsureJournal()
		{
			if (this._journalScope == null && this._navigatorHost != null)
			{
				this._journalScope = this._navigatorHost.GetJournal(true);
			}
			return this._journalScope;
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000BE9B0 File Offset: 0x000BCBB0
		private bool IsParentedByBrowserWindow()
		{
			return this.Application != null && this.Application.CheckAccess() && ((this.JournalScope != null && this.JournalScope.NavigatorHost is RootBrowserWindow) || (this.JournalScope == null && this.Application.BrowserCallbackServices != null));
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000BEA04 File Offset: 0x000BCC04
		private bool IsConsistent(NavigateInfo navInfo)
		{
			return navInfo == null || (navInfo.IsConsistent && (navInfo.JournalEntry == null || navInfo.JournalEntry.NavigationServiceId == this._guidId));
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000BEA35 File Offset: 0x000BCC35
		private bool IsJournalNavigation(NavigateInfo navInfo)
		{
			return navInfo != null && (navInfo.NavigationMode == NavigationMode.Back || navInfo.NavigationMode == NavigationMode.Forward);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000BEA50 File Offset: 0x000BCC50
		private CustomJournalStateInternal GetRootViewerState(JournalReason journalReason)
		{
			if (this._navigatorHostImpl != null && !(this._bp is Visual))
			{
				Visual visual = this._navigatorHostImpl.FindRootViewer();
				IJournalState journalState = visual as IJournalState;
				if (journalState != null)
				{
					return journalState.GetJournalState(journalReason);
				}
			}
			return null;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000BEA94 File Offset: 0x000BCC94
		private bool RestoreRootViewerState(CustomJournalStateInternal rvs)
		{
			Visual visual = this._navigatorHostImpl.FindRootViewer();
			if (visual == null)
			{
				return false;
			}
			IJournalState journalState = visual as IJournalState;
			if (journalState != null)
			{
				journalState.RestoreJournalState(rvs);
			}
			return true;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000BEAC4 File Offset: 0x000BCCC4
		private bool ShouldDelegateXamlViewerNavigationToBrowser(NavigateInfo navigateInfo, Uri resolvedUri)
		{
			bool result = false;
			if (BrowserInteropHelper.IsViewer)
			{
				Invariant.Assert(resolvedUri != null && resolvedUri.IsAbsoluteUri);
				result = (!BrowserInteropHelper.IsInitialViewerNavigation && (navigateInfo == null || navigateInfo.NavigationMode != NavigationMode.Refresh) && this.IsTopLevelContainer && BrowserInteropHelper.IsAvalonTopLevel && !this.HasTravelLogIntegration);
			}
			return result;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000BEB24 File Offset: 0x000BCD24
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateAddressBarForLooseXaml()
		{
			if (BrowserInteropHelper.IsViewer && !BrowserInteropHelper.IsInitialViewerNavigation && this.IsTopLevelContainer)
			{
				Uri uri = this._currentSource;
				if (PackUriHelper.IsPackUri(uri))
				{
					uri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(uri);
				}
				Invariant.Assert(this._navigatorHost != null && this._navigatorHost == this.Application.MainWindow && uri.IsAbsoluteUri && !PackUriHelper.IsPackUri(uri));
				SecurityHelper.DemandWebPermission(uri);
				this.Application.BrowserCallbackServices.UpdateAddressBar(uri.ToString());
			}
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000BEBB0 File Offset: 0x000BCDB0
		internal static INavigatorBase FindTargetInApplication(string targetName)
		{
			if (Application.Current == null)
			{
				return null;
			}
			INavigatorBase navigatorBase = NavigationService.FindTargetInWindowCollection(Application.Current.WindowsInternal.Clone(), targetName);
			if (navigatorBase == null)
			{
				navigatorBase = NavigationService.FindTargetInWindowCollection(Application.Current.NonAppWindowsInternal.Clone(), targetName);
			}
			return navigatorBase;
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000BEBF8 File Offset: 0x000BCDF8
		private static INavigatorBase FindTargetInWindowCollection(WindowCollection wc, string targetName)
		{
			NavigationWindow nw = null;
			DispatcherOperationCallback <>9__0;
			for (int i = 0; i < wc.Count; i++)
			{
				nw = (wc[i] as NavigationWindow);
				if (nw != null)
				{
					INavigatorBase navigatorBase;
					if (nw.CheckAccess())
					{
						navigatorBase = NavigationService.FindTargetInNavigationWindow(nw, targetName);
					}
					else
					{
						Dispatcher dispatcher = nw.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Send;
						DispatcherOperationCallback method;
						if ((method = <>9__0) == null)
						{
							method = (<>9__0 = ((object unused) => NavigationService.FindTargetInNavigationWindow(nw, targetName)));
						}
						navigatorBase = (INavigator)dispatcher.Invoke(priority, method, null);
					}
					if (navigatorBase != null)
					{
						return navigatorBase;
					}
				}
			}
			return null;
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000BECA8 File Offset: 0x000BCEA8
		private static INavigatorBase FindTargetInNavigationWindow(NavigationWindow navigationWindow, string navigatorId)
		{
			if (navigationWindow != null)
			{
				return navigationWindow.NavigationService.FindTarget(navigatorId);
			}
			return null;
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000BECBC File Offset: 0x000BCEBC
		internal void InvalidateJournalNavigationScope()
		{
			if (this._journalScope != null && this._journalScope.Journal.HasUncommittedNavigation)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_CantChangeJournalOwnership"));
			}
			this._journalScope = null;
			for (int i = this.ChildNavigationServices.Count - 1; i >= 0; i--)
			{
				((NavigationService)this.ChildNavigationServices[i]).InvalidateJournalNavigationScope();
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000BED28 File Offset: 0x000BCF28
		internal void OnParentNavigationServiceChanged()
		{
			NavigationService parentNavigationService = this._parentNavigationService;
			NavigationService navigationService = ((DependencyObject)this.INavigatorHost).GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
			if (navigationService == parentNavigationService)
			{
				return;
			}
			if (parentNavigationService != null)
			{
				parentNavigationService.RemoveChild(this);
			}
			if (navigationService != null)
			{
				navigationService.AddChild(this);
			}
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000BED70 File Offset: 0x000BCF70
		internal void AddChild(NavigationService ncChild)
		{
			if (ncChild == this)
			{
				throw new Exception(SR.Get("LoopDetected", new object[]
				{
					this._currentCleanSource
				}));
			}
			Invariant.Assert(ncChild.ParentNavigationService == null);
			Invariant.Assert(ncChild.JournalScope == null || ncChild.IsJournalLevelContainer, "Parentless NavigationService has a reference to a JournalNavigationScope its host navigator doesn't own.");
			this.ChildNavigationServices.Add(ncChild);
			ncChild._parentNavigationService = this;
			if (this.JournalScope != null)
			{
				this.JournalScope.Journal.UpdateView();
			}
			if (this.NavStatus == NavigationStatus.Stopped)
			{
				ncChild.INavigatorHost.StopLoading();
				return;
			}
			if (ncChild.NavStatus != NavigationStatus.Idle && ncChild.NavStatus != NavigationStatus.Stopped && this.NavStatus != NavigationStatus.Idle && this.NavStatus != NavigationStatus.Stopped)
			{
				this.PendingNavigationList.Add(ncChild);
			}
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000BEE3C File Offset: 0x000BD03C
		internal void RemoveChild(NavigationService ncChild)
		{
			this.ChildNavigationServices.Remove(ncChild);
			ncChild._parentNavigationService = null;
			if (!ncChild.IsJournalLevelContainer)
			{
				ncChild.InvalidateJournalNavigationScope();
			}
			if (this.JournalScope != null)
			{
				this.JournalScope.Journal.UpdateView();
			}
			if (this.PendingNavigationList.Contains(ncChild))
			{
				this.PendingNavigationList.Remove(ncChild);
				this.HandleLoadCompleted(null);
			}
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000BEEA4 File Offset: 0x000BD0A4
		internal NavigationService FindTarget(Guid navigationServiceId)
		{
			if (this.GuidId == navigationServiceId)
			{
				return this;
			}
			foreach (object obj in this.ChildNavigationServices)
			{
				NavigationService navigationService = (NavigationService)obj;
				NavigationService navigationService2 = navigationService.FindTarget(navigationServiceId);
				if (navigationService2 != null)
				{
					return navigationService2;
				}
			}
			return null;
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000BEF20 File Offset: 0x000BD120
		internal INavigatorBase FindTarget(string name)
		{
			FrameworkElement frameworkElement = this.INavigatorHost as FrameworkElement;
			if (string.Compare(name, frameworkElement.Name, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.INavigatorHost;
			}
			INavigatorBase navigatorBase = null;
			foreach (object obj in this.ChildNavigationServices)
			{
				NavigationService navigationService = (NavigationService)obj;
				navigatorBase = navigationService.FindTarget(name);
				if (navigatorBase != null)
				{
					return navigatorBase;
				}
			}
			return navigatorBase;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000BEFB0 File Offset: 0x000BD1B0
		internal bool IsContentKeepAlive()
		{
			bool flag = true;
			DependencyObject dependencyObject = this._bp as DependencyObject;
			if (dependencyObject != null)
			{
				flag = JournalEntry.GetKeepAlive(dependencyObject);
				if (!flag)
				{
					PageFunctionBase pageFunctionBase = dependencyObject as PageFunctionBase;
					bool flag2 = !this.CanReloadFromUri;
					if (pageFunctionBase == null && flag2)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000BEFF4 File Offset: 0x000BD1F4
		private void SetBaseUri(DependencyObject dobj, Uri fullUri)
		{
			Invariant.Assert(dobj != null && !dobj.IsSealed);
			Uri uri = (Uri)dobj.GetValue(BaseUriHelper.BaseUriProperty);
			if (uri == null && fullUri != null)
			{
				dobj.SetValue(BaseUriHelper.BaseUriProperty, fullUri);
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000BF048 File Offset: 0x000BD248
		private bool UnhookOldTree(object oldTree)
		{
			DependencyObject dependencyObject = oldTree as DependencyObject;
			if (dependencyObject != null && !dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(NavigationService.NavigationServiceProperty, null);
			}
			IInputElement inputElement = oldTree as IInputElement;
			if (inputElement != null && inputElement.IsKeyboardFocusWithin)
			{
				if (dependencyObject != null && this.JournalScope != null)
				{
					DependencyObject dependencyObject2 = (DependencyObject)this.INavigatorHost;
					if (!(bool)dependencyObject2.GetValue(FocusManager.IsFocusScopeProperty))
					{
						dependencyObject2 = FocusManager.GetFocusScope(dependencyObject2);
					}
					FocusManager.SetFocusedElement(dependencyObject2, null);
				}
				Keyboard.PrimaryDevice.Focus(null);
			}
			PageFunctionBase pageFunctionBase = oldTree as PageFunctionBase;
			if (pageFunctionBase != null)
			{
				pageFunctionBase.FinishHandler = null;
			}
			bool result = true;
			if (this.IsContentKeepAlive())
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000BF0EC File Offset: 0x000BD2EC
		private bool HookupNewTree(object newTree, NavigateInfo navInfo, Uri newUri)
		{
			if (newTree != null && this.IsJournalNavigation(navInfo))
			{
				navInfo.JournalEntry.RestoreState(newTree);
			}
			PageFunctionReturnInfo pageFunctionReturnInfo = navInfo as PageFunctionReturnInfo;
			PageFunctionBase pageFunctionBase = (pageFunctionReturnInfo != null) ? pageFunctionReturnInfo.FinishingChildPageFunction : null;
			if (pageFunctionBase != null)
			{
				object returnEventArgs = (pageFunctionReturnInfo != null) ? pageFunctionReturnInfo.ReturnEventArgs : null;
				if (newTree != null)
				{
					this.FireChildPageFunctionReturnEvent(newTree, pageFunctionBase, returnEventArgs);
					if (this._navigateQueueItem != null)
					{
						if (pageFunctionReturnInfo.JournalEntry != null)
						{
							pageFunctionReturnInfo.JournalEntry.SaveState(newTree);
						}
						return false;
					}
				}
			}
			if (NavigationService.IsPageFunction(newTree))
			{
				this.SetupPageFunctionHandlers(newTree);
				if ((navInfo == null || navInfo.NavigationMode == NavigationMode.New) && !this._doNotJournalCurrentContent)
				{
					PageFunctionBase pageFunctionBase2 = (PageFunctionBase)newTree;
					if (!pageFunctionBase2._Resume && pageFunctionBase2.ParentPageFunctionId == Guid.Empty && this._bp is PageFunctionBase)
					{
						pageFunctionBase2.ParentPageFunctionId = ((PageFunctionBase)this._bp).PageFunctionId;
					}
				}
			}
			DependencyObject dependencyObject = newTree as DependencyObject;
			if (dependencyObject != null && !dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(NavigationService.NavigationServiceProperty, this);
				if (newUri != null && !BindUriHelper.StartWithFragment(newUri))
				{
					this.SetBaseUri(dependencyObject, newUri);
				}
			}
			this._webBrowser = (newTree as WebBrowser);
			return true;
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000BF210 File Offset: 0x000BD410
		private bool OnBeforeSwitchContent(object newBP, NavigateInfo navInfo, Uri newUri)
		{
			if (newBP != null && !this.HookupNewTree(newBP, navInfo, newUri))
			{
				return false;
			}
			if (this.HasTravelLogIntegration)
			{
				this.DispatchPendingCallFromBrowser();
				if (this._navigateQueueItem != null)
				{
					return false;
				}
			}
			if (navInfo == null)
			{
				this.UpdateJournal(NavigationMode.New, JournalReason.NewContentNavigation, null);
			}
			else if (navInfo.NavigationMode != NavigationMode.Refresh)
			{
				this.UpdateJournal(navInfo.NavigationMode, JournalReason.NewContentNavigation, navInfo.JournalEntry);
			}
			if (this._navigateQueueItem != null)
			{
				return false;
			}
			bool flag = this.UnhookOldTree(this._bp);
			if (flag)
			{
				DisposeTreeQueueItem @object = new DisposeTreeQueueItem(this._bp);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(@object.Dispatch), null);
			}
			return true;
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000BF2B1 File Offset: 0x000BD4B1
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DispatchPendingCallFromBrowser()
		{
			BrowserInteropHelper.HostBrowser.GetTop();
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000BF2BE File Offset: 0x000BD4BE
		internal void VisualTreeAvailable(Visual v)
		{
			if (v != this._oldRootVisual)
			{
				if (this._oldRootVisual != null)
				{
					this._oldRootVisual.SetValue(NavigationService.NavigationServiceProperty, null);
				}
				if (v != null)
				{
					v.SetValue(NavigationService.NavigationServiceProperty, this);
				}
				this._oldRootVisual = v;
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000BF2F8 File Offset: 0x000BD4F8
		void IContentContainer.OnContentReady(ContentType contentType, object bp, Uri bpu, object navState)
		{
			Invariant.Assert(bpu == null || bpu.IsAbsoluteUri, "Content URI must be absolute.");
			if (this.IsDisposed)
			{
				return;
			}
			if (!this.IsValidRootElement(bp))
			{
				throw new InvalidOperationException(SR.Get("WrongNavigateRootElement", new object[]
				{
					bp.ToString()
				}));
			}
			this.ResetPendingNavigationState(NavigationStatus.Navigated);
			NavigateInfo navigateInfo = navState as NavigateInfo;
			NavigationMode navigationMode = (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode;
			if (bpu == null)
			{
				bpu = ((navigateInfo == null) ? null : navigateInfo.Source);
			}
			Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(bpu);
			if (this.PreBPReady != null)
			{
				BPReadyEventArgs bpreadyEventArgs = new BPReadyEventArgs(bp, bpu);
				this.PreBPReady(this, bpreadyEventArgs);
				if (bpreadyEventArgs.Cancel)
				{
					this._navStatus = NavigationStatus.Idle;
					return;
				}
			}
			bool flag = false;
			if (bp == this._bp)
			{
				flag = true;
				this._bp = null;
				if (this.BPReady != null)
				{
					this.BPReady(this, new BPReadyEventArgs(null, null));
				}
			}
			else
			{
				if (!this.OnBeforeSwitchContent(bp, navigateInfo, bpu))
				{
					return;
				}
				if (navigationMode != NavigationMode.Refresh)
				{
					if (navigateInfo == null || navigateInfo.JournalEntry == null)
					{
						this._contentId += 1U;
						this._journalEntryGroupState = null;
					}
					else
					{
						this._contentId = navigateInfo.JournalEntry.ContentId;
						this._journalEntryGroupState = navigateInfo.JournalEntry.JEGroupState;
					}
					this._currentSource = bpu;
					this._currentCleanSource = uriRelativeToPackAppBase;
				}
			}
			this._bp = bp;
			if (this.BPReady != null)
			{
				this.BPReady(this, new BPReadyEventArgs(this._bp, bpu));
			}
			this.HandleNavigated(navState, !flag);
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000BF484 File Offset: 0x000BD684
		void IContentContainer.OnNavigationProgress(Uri sourceUri, long bytesRead, long maxBytes)
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (!sourceUri.Equals(this.Source))
			{
				return;
			}
			NavigationService navigationService = null;
			if (this.JournalScope != null && this.JournalScope.RootNavigationService != this)
			{
				navigationService = this.JournalScope.RootNavigationService;
				navigationService.BytesRead += bytesRead - this._bytesRead;
				navigationService.MaxBytes += maxBytes - this._maxBytes;
			}
			this._bytesRead = bytesRead;
			this._maxBytes = maxBytes;
			this.FireNavigationProgress(sourceUri);
			if (navigationService == null)
			{
				return;
			}
			navigationService.FireNavigationProgress(sourceUri);
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000BF517 File Offset: 0x000BD717
		void IContentContainer.OnStreamClosed(Uri sourceUri)
		{
			if (!sourceUri.Equals(this.Source))
			{
				return;
			}
			this._asyncObjectConverter = null;
			this.HandleLoadCompleted(null);
		}

		/// <summary>Gets a reference to the <see cref="T:System.Windows.Navigation.NavigationService" /> for the navigator whose content contains the specified <see cref="T:System.Windows.DependencyObject" />.</summary>
		/// <param name="dependencyObject">The <see cref="T:System.Windows.DependencyObject" /> in content that is hosted by a navigator.</param>
		/// <returns>A reference to the <see cref="T:System.Windows.Navigation.NavigationService" /> for the navigator whose content contains the specified <see cref="T:System.Windows.DependencyObject" />; can be <see langword="null" /> in some cases (see Remarks).</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dependencyObject" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06002964 RID: 10596 RVA: 0x000BF536 File Offset: 0x000BD736
		public static NavigationService GetNavigationService(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return dependencyObject.GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
		}

		/// <summary>Gets or sets the uniform resource identifier (URI) of the current content, or the URI of new content that is currently being navigated to.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI for the current content, or the content that is currently being navigated to.</returns>
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06002965 RID: 10597 RVA: 0x000BF558 File Offset: 0x000BD758
		// (set) Token: 0x06002966 RID: 10598 RVA: 0x000BF5C4 File Offset: 0x000BD7C4
		public Uri Source
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				if (this._recursiveNavigateList.Count > 0)
				{
					return BindUriHelper.GetUriRelativeToPackAppBase((this._recursiveNavigateList[this._recursiveNavigateList.Count - 1] as NavigateQueueItem).Source);
				}
				if (this._navigateQueueItem != null)
				{
					return BindUriHelper.GetUriRelativeToPackAppBase(this._navigateQueueItem.Source);
				}
				return this._currentCleanSource;
			}
			set
			{
				this.Navigate(value);
			}
		}

		/// <summary>Gets the uniform resource identifier (URI) of the content that was last navigated to.</summary>
		/// <returns>A <see cref="T:System.Uri" /> for the content that was last navigated to, if navigated to by using a URI; otherwise, <see langword="null" />.</returns>
		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x000BF5CE File Offset: 0x000BD7CE
		public Uri CurrentSource
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				return this._currentCleanSource;
			}
		}

		/// <summary>Gets or sets a reference to the object that contains the current content.</summary>
		/// <returns>An object that is a reference to the object that contains the current content.</returns>
		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x000BF5E0 File Offset: 0x000BD7E0
		// (set) Token: 0x06002969 RID: 10601 RVA: 0x000BF5F2 File Offset: 0x000BD7F2
		public object Content
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				return this._bp;
			}
			set
			{
				this.Navigate(value);
			}
		}

		/// <summary>Adds an entry to back navigation history that contains a <see cref="T:System.Windows.Navigation.CustomContentState" /> object.</summary>
		/// <param name="state">A <see cref="T:System.Windows.Navigation.CustomContentState" /> object that represents application-defined state that is associated with a specific piece of content.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="state" /> is <see langword="null" />, and a <see cref="T:System.Windows.Navigation.CustomContentState" /> object isn't returned from <see cref="M:System.Windows.Navigation.IProvideCustomContentState.GetContentState" />.</exception>
		// Token: 0x0600296A RID: 10602 RVA: 0x000BF5FC File Offset: 0x000BD7FC
		public void AddBackEntry(CustomContentState state)
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this._bp == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_AddBackEntryNoContent"));
			}
			this._customContentStateToSave = state;
			JournalEntry journalEntry = this.UpdateJournal(NavigationMode.New, JournalReason.AddBackEntry, null);
			this._customContentStateToSave = null;
			if (journalEntry != null && journalEntry.CustomContentState == null)
			{
				this.RemoveBackEntry();
				throw new InvalidOperationException(SR.Get("InvalidOperation_MustImplementIPCCSOrHandleNavigating", new object[]
				{
					(this._bp != null) ? this._bp.GetType().ToString() : "null"
				}));
			}
		}

		/// <summary>Removes the most recent journal entry from back history.</summary>
		/// <returns>The most recent <see cref="T:System.Windows.Navigation.JournalEntry" /> in back navigation history, if there is one.</returns>
		// Token: 0x0600296B RID: 10603 RVA: 0x000BF68C File Offset: 0x000BD88C
		public JournalEntry RemoveBackEntry()
		{
			if (this.IsDisposed)
			{
				return null;
			}
			if (this.JournalScope == null)
			{
				return null;
			}
			return this.JournalScope.RemoveBackEntry();
		}

		/// <summary>Navigate asynchronously to content that is specified by a uniform resource identifier (URI).</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600296C RID: 10604 RVA: 0x000BF6AD File Offset: 0x000BD8AD
		public bool Navigate(Uri source)
		{
			return this.Navigate(source, null, false, false);
		}

		/// <summary>Navigate asynchronously to content that is contained by an object.</summary>
		/// <param name="root">An object that contains the content to navigate to.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600296D RID: 10605 RVA: 0x000BF6B9 File Offset: 0x000BD8B9
		public bool Navigate(object root)
		{
			return this.Navigate(root, null);
		}

		/// <summary>Navigate asynchronously to source content located at a uniform resource identifier (URI), and pass an object that contains data to be used for processing during navigation.</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600296E RID: 10606 RVA: 0x000BF6C3 File Offset: 0x000BD8C3
		public bool Navigate(Uri source, object navigationState)
		{
			return this.Navigate(source, navigationState, false, false);
		}

		/// <summary>Navigate asynchronously to source content located at a uniform resource identifier (URI), pass an object containing navigation state for processing during navigation, and sandbox the content.</summary>
		/// <param name="source">A <see cref="T:System.Uri" /> object initialized with the URI for the desired content.</param>
		/// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
		/// <param name="sandboxExternalContent">Download content into a partial trust security sandbox (with the default Internet zone set of permissions, if <see langword="true" />. The default is <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600296F RID: 10607 RVA: 0x000BF6CF File Offset: 0x000BD8CF
		public bool Navigate(Uri source, object navigationState, bool sandboxExternalContent)
		{
			return this.Navigate(source, navigationState, sandboxExternalContent, false);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000BF6DC File Offset: 0x000BD8DC
		internal bool Navigate(Uri source, object navigationState, bool sandboxExternalContent, bool navigateOnSourceChanged)
		{
			if (this.IsDisposed)
			{
				return false;
			}
			NavigateInfo navigateInfo = navigationState as NavigateInfo;
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationStart, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, new object[]
				{
					(navigateInfo != null) ? navigateInfo.NavigationMode.ToString() : NavigationMode.New.ToString(),
					(source != null) ? ("\"" + source.ToString() + "\"") : "(null)"
				});
			}
			Invariant.Assert(this.IsConsistent(navigateInfo));
			WebRequest webRequest = null;
			bool flag = false;
			Uri uri = null;
			if (source != null)
			{
				if (BindUriHelper.StartWithFragment(source) || BindUriHelper.StartWithFragment(BindUriHelper.GetUriRelativeToPackAppBase(source)))
				{
					uri = BindUriHelper.GetResolvedUri(this._currentSource, source);
					flag = true;
				}
				else
				{
					uri = BindUriHelper.GetResolvedUri(source);
					flag = ((navigateInfo == null || navigateInfo.JournalEntry == null || navigateInfo.JournalEntry.ContentId == this._contentId) && NavigationService.IsSameUri(null, uri, this._currentSource, false));
				}
				if (navigateInfo != null && navigateInfo.NavigationMode == NavigationMode.Refresh)
				{
					flag = false;
				}
				if (!flag)
				{
					webRequest = this.CreateWebRequest(uri, navigateInfo);
					if (webRequest == null)
					{
						return false;
					}
				}
			}
			if (!this.HandleNavigating(uri, null, navigationState, webRequest, navigateOnSourceChanged))
			{
				return false;
			}
			if (source == null && this._bp == null)
			{
				this.ResetPendingNavigationState(NavigationStatus.Idle);
				return true;
			}
			if (flag)
			{
				this.NavigateToFragmentOrCustomContentState(uri, navigationState);
				return true;
			}
			if (this.ShouldDelegateXamlViewerNavigationToBrowser(navigateInfo, uri))
			{
				try
				{
					this.DelegateToBrowser(webRequest is PackWebRequest, uri);
					return true;
				}
				finally
				{
					this.ResetPendingNavigationState(NavigationStatus.Idle);
				}
			}
			this._navigateQueueItem.PostNavigation();
			return true;
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000BF888 File Offset: 0x000BDA88
		private void InformBrowserAboutStoppedNavigation()
		{
			if (this.Application != null && this.Application.CheckAccess())
			{
				this.Application.PerformNavigationStateChangeTasks(true, false, Application.NavigationStateChange.Stopped);
			}
		}

		/// <summary>Navigate asynchronously to content that is contained by an object, and pass an object that contains data to be used for processing during navigation.</summary>
		/// <param name="root">An object that contains the content to navigate to.</param>
		/// <param name="navigationState">An object that contains data to be used for processing during navigation.</param>
		/// <returns>
		///     <see langword="true" /> if a navigation is not canceled; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002972 RID: 10610 RVA: 0x000BF8B0 File Offset: 0x000BDAB0
		public bool Navigate(object root, object navigationState)
		{
			if (this.IsDisposed)
			{
				return false;
			}
			NavigateInfo navigateInfo = navigationState as NavigateInfo;
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationStart, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, new object[]
				{
					(navigateInfo != null) ? navigateInfo.NavigationMode.ToString() : NavigationMode.New.ToString(),
					(root != null) ? root.ToString() : "(null)"
				});
			}
			Invariant.Assert(this.IsConsistent(navigateInfo));
			if (navigateInfo == null)
			{
				PageFunctionBase pageFunctionBase = root as PageFunctionBase;
				if (pageFunctionBase != null && (pageFunctionBase._Resume || pageFunctionBase._Saver != null))
				{
					throw new InvalidOperationException(SR.Get("InvalidOperation_CannotReenterPageFunction"));
				}
			}
			Uri uri = (navigateInfo == null) ? null : navigateInfo.Source;
			if (!this.HandleNavigating(uri, root, navigationState, null, false))
			{
				return false;
			}
			if (root == this._bp && (navigateInfo == null || navigateInfo.NavigationMode != NavigationMode.Refresh))
			{
				this.NavigateToFragmentOrCustomContentState(uri, navigationState);
				if (this.IsJournalNavigation(navigateInfo))
				{
					this._journalEntryGroupState = navigateInfo.JournalEntry.JEGroupState;
					this._contentId = this._journalEntryGroupState.ContentId;
					this._journalScope.Journal.UpdateView();
				}
				return true;
			}
			this._navigateQueueItem.PostNavigation();
			return true;
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in forward navigation history.</summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in forward navigation history; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x000BF9F0 File Offset: 0x000BDBF0
		public bool CanGoForward
		{
			get
			{
				return this.JournalScope != null && this.JournalScope.CanGoForward;
			}
		}

		/// <summary>Gets a value that indicates whether there is at least one entry in back navigation history.</summary>
		/// <returns>
		///     <see langword="true" /> if there is at least one entry in back navigation history; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x000BFA07 File Offset: 0x000BDC07
		public bool CanGoBack
		{
			get
			{
				return this.JournalScope != null && this.JournalScope.CanGoBack;
			}
		}

		/// <summary>Navigate to the most recent entry in forward navigation history, if there is one.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Navigation.NavigationService.GoForward" /> is called when there are no entries in forward navigation history.</exception>
		// Token: 0x06002975 RID: 10613 RVA: 0x000BFA1E File Offset: 0x000BDC1E
		public void GoForward()
		{
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			this.JournalScope.GoForward();
		}

		/// <summary>Navigates to the most recent entry in back navigation history, if there is one.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Navigation.NavigationService.GoBack" /> is called when there are no entries in back navigation history.</exception>
		// Token: 0x06002976 RID: 10614 RVA: 0x000BFA43 File Offset: 0x000BDC43
		public void GoBack()
		{
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			this.JournalScope.GoBack();
		}

		/// <summary>Stops further downloading of content for the current navigation request.</summary>
		// Token: 0x06002977 RID: 10615 RVA: 0x000BFA68 File Offset: 0x000BDC68
		public void StopLoading()
		{
			this.DoStopLoading(true, true);
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000BFA74 File Offset: 0x000BDC74
		private void DoStopLoading(bool clearRecursiveNavigations, bool fireEvents)
		{
			bool flag = false;
			object navState = null;
			if (this._asyncObjectConverter != null)
			{
				this._asyncObjectConverter.CancelAsync();
				this._asyncObjectConverter = null;
				Invariant.Assert(this._webResponse != null);
				this._webResponse.Close();
				this._webResponse = null;
			}
			else if (this._navStatus != NavigationStatus.Navigating && this._webResponse != null)
			{
				this._webResponse.Close();
				this._webResponse = null;
			}
			this._navStatus = NavigationStatus.Stopped;
			if (this._navigateQueueItem != null)
			{
				this._navigateQueueItem.Stop();
				if (this.JournalScope != null && clearRecursiveNavigations)
				{
					this.JournalScope.AbortJournalNavigation();
				}
				if (this._request != null)
				{
					try
					{
						WebRequest request = this._request;
						this._request = null;
						request.Abort();
					}
					catch (NotSupportedException)
					{
					}
					catch (NotImplementedException)
					{
					}
				}
				navState = this._navigateQueueItem.NavState;
				this.ResetPendingNavigationState(NavigationStatus.Stopped);
				flag = true;
			}
			if (clearRecursiveNavigations && this._recursiveNavigateList.Count > 0)
			{
				this._recursiveNavigateList.Clear();
				flag = true;
			}
			if (this._navigatorHostImpl != null)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(true);
			}
			bool fireEvents2 = false;
			try
			{
				if (fireEvents && flag)
				{
					this.FireNavigationStopped(navState);
				}
				fireEvents2 = true;
			}
			finally
			{
				int i = 0;
				try
				{
					while (i < this._childNavigationServices.Count)
					{
						((NavigationService)this._childNavigationServices[i]).DoStopLoading(true, fireEvents2);
						i++;
					}
				}
				finally
				{
					if (++i < this._childNavigationServices.Count)
					{
						while (i < this._childNavigationServices.Count)
						{
							((NavigationService)this._childNavigationServices[i]).DoStopLoading(true, false);
							i++;
						}
					}
					this.PendingNavigationList.Clear();
					if (this._parentNavigationService != null && this._parentNavigationService.PendingNavigationList.Contains(this))
					{
						this._parentNavigationService.PendingNavigationList.Remove(this);
						if (fireEvents)
						{
							this._parentNavigationService.HandleLoadCompleted(null);
						}
					}
				}
			}
		}

		/// <summary>Reloads the current content.</summary>
		// Token: 0x06002979 RID: 10617 RVA: 0x000BFC8C File Offset: 0x000BDE8C
		public void Refresh()
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this.CanReloadFromUri)
			{
				this.Navigate(this._currentSource, new NavigateInfo(this._currentSource, NavigationMode.Refresh));
				return;
			}
			if (this._bp != null)
			{
				this.Navigate(this._bp, new NavigateInfo(this._currentSource, NavigationMode.Refresh));
			}
		}

		/// <summary>Occurs when an error occurs while navigating to the requested content.</summary>
		// Token: 0x1400005C RID: 92
		// (add) Token: 0x0600297A RID: 10618 RVA: 0x000BFCE8 File Offset: 0x000BDEE8
		// (remove) Token: 0x0600297B RID: 10619 RVA: 0x000BFD20 File Offset: 0x000BDF20
		public event NavigationFailedEventHandler NavigationFailed;

		/// <summary>Occurs when a new navigation is requested.</summary>
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x0600297C RID: 10620 RVA: 0x000BFD55 File Offset: 0x000BDF55
		// (remove) Token: 0x0600297D RID: 10621 RVA: 0x000BFD6E File Offset: 0x000BDF6E
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._navigating = (NavigatingCancelEventHandler)Delegate.Combine(this._navigating, value);
			}
			remove
			{
				this._navigating = (NavigatingCancelEventHandler)Delegate.Remove(this._navigating, value);
			}
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000BFD88 File Offset: 0x000BDF88
		private bool FireNavigating(Uri source, object bp, object navState, WebRequest request)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(source);
			if (bp != null && navigateInfo != null && !(navigateInfo is PageFunctionReturnInfo) && (!(bp is PageFunctionBase) || !(bp as PageFunctionBase)._Resume) && navigateInfo.Source != null && navigateInfo.NavigationMode == NavigationMode.New)
			{
				return this._navigateQueueItem == null;
			}
			CustomContentState customContentState = (navigateInfo != null && navigateInfo.JournalEntry != null) ? navigateInfo.JournalEntry.CustomContentState : null;
			object extraData = (navigateInfo == null) ? navState : null;
			NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(uriRelativeToPackAppBase, bp, customContentState, extraData, (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode, request, this.INavigatorHost, this.IsNavigationInitiator);
			if (this._navigating != null)
			{
				this._navigating(this.INavigatorHost, navigatingCancelEventArgs);
			}
			if (!navigatingCancelEventArgs.Cancel && this.Application != null && this.Application.CheckAccess())
			{
				this.Application.FireNavigating(navigatingCancelEventArgs, this._bp == null);
			}
			this._customContentStateToSave = navigatingCancelEventArgs.ContentStateToSave;
			if (navigatingCancelEventArgs.Cancel && this.JournalScope != null)
			{
				this.JournalScope.AbortJournalNavigation();
			}
			return !navigatingCancelEventArgs.Cancel && !this.IsDisposed;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000BFEBC File Offset: 0x000BE0BC
		private bool HandleNavigating(Uri source, object content, object navState, WebRequest newRequest, bool navigateOnSourceChanged)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			if (navigateInfo != null && source == null)
			{
				source = navigateInfo.Source;
			}
			NavigateQueueItem navigateQueueItem = new NavigateQueueItem(source, content, (navigateInfo != null) ? navigateInfo.NavigationMode : NavigationMode.New, navState, this);
			this._recursiveNavigateList.Add(navigateQueueItem);
			this._isNavInitiatorValid = false;
			if (this._navigatorHostImpl != null && !navigateOnSourceChanged)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(this.IsJournalNavigation(navigateInfo));
			}
			bool flag = false;
			try
			{
				flag = this.FireNavigating(source, content, navState, newRequest);
			}
			catch
			{
				this.CleanupAfterNavigationCancelled(navigateQueueItem);
				throw;
			}
			if (flag)
			{
				this.DoStopLoading(false, true);
				if (!this._recursiveNavigateList.Contains(navigateQueueItem))
				{
					return false;
				}
				this._recursiveNavigateList.Clear();
				this._navigateQueueItem = navigateQueueItem;
				this._request = newRequest;
				this._navStatus = NavigationStatus.Navigating;
			}
			else
			{
				this.CleanupAfterNavigationCancelled(navigateQueueItem);
			}
			return flag;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000BFF9C File Offset: 0x000BE19C
		private void CleanupAfterNavigationCancelled(NavigateQueueItem localNavigateQueueItem)
		{
			if (this.JournalScope != null)
			{
				this.JournalScope.AbortJournalNavigation();
			}
			this._recursiveNavigateList.Remove(localNavigateQueueItem);
			if (this._navigatorHostImpl != null)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(true);
			}
			this.InformBrowserAboutStoppedNavigation();
		}

		/// <summary>Occurs when the content that is being navigated to has been found, and is available from the <see cref="P:System.Windows.Navigation.NavigationService.Content" /> property, although it may not have completed loading.</summary>
		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06002981 RID: 10625 RVA: 0x000BFFD7 File Offset: 0x000BE1D7
		// (remove) Token: 0x06002982 RID: 10626 RVA: 0x000BFFF0 File Offset: 0x000BE1F0
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._navigated = (NavigatedEventHandler)Delegate.Combine(this._navigated, value);
			}
			remove
			{
				this._navigated = (NavigatedEventHandler)Delegate.Remove(this._navigated, value);
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000C000C File Offset: 0x000BE20C
		private void FireNavigated(object navState)
		{
			object extraData = (navState is NavigateInfo) ? null : navState;
			try
			{
				NavigationEventArgs e = new NavigationEventArgs(this.CurrentSource, this.Content, extraData, this._webResponse, this.INavigatorHost, this.IsNavigationInitiator);
				if (this._navigated != null)
				{
					this._navigated(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigated(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000C00A4 File Offset: 0x000BE2A4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void HandleNavigated(object navState, bool navigatedToNewContent)
		{
			this.UpdateAddressBarForLooseXaml();
			BrowserInteropHelper.IsInitialViewerNavigation = false;
			NavigateInfo navigateInfo = navState as NavigateInfo;
			bool flag = false;
			if (navigatedToNewContent && this._currentSource != null)
			{
				string fragment = BindUriHelper.GetFragment(this._currentSource);
				flag = !string.IsNullOrEmpty(fragment);
			}
			if (navigateInfo != null && navigateInfo.JournalEntry != null)
			{
				JournalEntry journalEntry = navigateInfo.JournalEntry;
				if (journalEntry.CustomContentState != null)
				{
					journalEntry.CustomContentState.Replay(this, navigateInfo.NavigationMode);
					journalEntry.CustomContentState = null;
					if (this._navStatus != NavigationStatus.Navigated)
					{
						return;
					}
				}
				if (journalEntry.RootViewerState != null && this._navigatorHostImpl != null)
				{
					if (!navigatedToNewContent)
					{
						this.RestoreRootViewerState(journalEntry.RootViewerState);
						journalEntry.RootViewerState = null;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				FrameworkContentElement frameworkContentElement = this._bp as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.Loaded += this.OnContentLoaded;
				}
				else
				{
					FrameworkElement frameworkElement = this._bp as FrameworkElement;
					if (frameworkElement != null)
					{
						frameworkElement.Loaded += this.OnContentLoaded;
					}
				}
				this._cancelContentRenderedHandling = false;
			}
			if (this.JournalScope != null)
			{
				NavigateQueueItem navigateQueueItem = this._navigateQueueItem;
				this.JournalScope.Journal.UpdateView();
				if (this._navigateQueueItem != navigateQueueItem)
				{
					return;
				}
			}
			this.ResetPendingNavigationState(NavigationStatus.Navigated);
			this.FireNavigated(navState);
			if (navigatedToNewContent && NavigationService.IsPageFunction(this._bp))
			{
				this.HandlePageFunction(navigateInfo);
			}
			this.HandleLoadCompleted(navState);
		}

		/// <summary>Occurs periodically during a download to provide navigation progress information.</summary>
		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06002985 RID: 10629 RVA: 0x000C0200 File Offset: 0x000BE400
		// (remove) Token: 0x06002986 RID: 10630 RVA: 0x000C0219 File Offset: 0x000BE419
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._navigationProgress = (NavigationProgressEventHandler)Delegate.Combine(this._navigationProgress, value);
			}
			remove
			{
				this._navigationProgress = (NavigationProgressEventHandler)Delegate.Remove(this._navigationProgress, value);
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000C0234 File Offset: 0x000BE434
		private void FireNavigationProgress(Uri source)
		{
			UIElement uielement = this.INavigatorHost as UIElement;
			if (uielement != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(uielement);
				if (automationPeer != null)
				{
					NavigationWindowAutomationPeer.RaiseAsyncContentLoadedEvent(automationPeer, this.BytesRead, this.MaxBytes);
				}
			}
			NavigationProgressEventArgs e = new NavigationProgressEventArgs(source, this.BytesRead, this.MaxBytes, this.INavigatorHost);
			try
			{
				if (this._navigationProgress != null)
				{
					this._navigationProgress(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigationProgress(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		/// <summary>Occurs when content that was navigated to has been loaded, parsed, and has begun rendering.</summary>
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06002988 RID: 10632 RVA: 0x000C02E0 File Offset: 0x000BE4E0
		// (remove) Token: 0x06002989 RID: 10633 RVA: 0x000C02F9 File Offset: 0x000BE4F9
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._loadCompleted = (LoadCompletedEventHandler)Delegate.Combine(this._loadCompleted, value);
			}
			remove
			{
				this._loadCompleted = (LoadCompletedEventHandler)Delegate.Remove(this._loadCompleted, value);
			}
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000C0314 File Offset: 0x000BE514
		private void FireLoadCompleted(bool isNavInitiator, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationEnd);
			object extraData = (navState is NavigateInfo) ? null : navState;
			NavigationEventArgs e = new NavigationEventArgs(this.CurrentSource, this.Content, extraData, this._webResponse, this.INavigatorHost, isNavInitiator);
			try
			{
				if (this._loadCompleted != null)
				{
					this._loadCompleted(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireLoadCompleted(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		/// <summary>Occurs when navigation to a content fragment begins, which occurs immediately, if the desired fragment is in the current content, or after the source XAML content has been loaded, if the desired fragment is in different content.</summary>
		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600298B RID: 10635 RVA: 0x000C03B8 File Offset: 0x000BE5B8
		// (remove) Token: 0x0600298C RID: 10636 RVA: 0x000C03D1 File Offset: 0x000BE5D1
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._fragmentNavigation = (FragmentNavigationEventHandler)Delegate.Combine(this._fragmentNavigation, value);
			}
			remove
			{
				this._fragmentNavigation = (FragmentNavigationEventHandler)Delegate.Remove(this._fragmentNavigation, value);
			}
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000C03EC File Offset: 0x000BE5EC
		private bool FireFragmentNavigation(string fragment)
		{
			if (string.IsNullOrEmpty(fragment))
			{
				return false;
			}
			FragmentNavigationEventArgs fragmentNavigationEventArgs = new FragmentNavigationEventArgs(fragment, this.INavigatorHost);
			try
			{
				if (this._fragmentNavigation != null)
				{
					this._fragmentNavigation(this, fragmentNavigationEventArgs);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireFragmentNavigation(fragmentNavigationEventArgs);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
			return fragmentNavigationEventArgs.Handled;
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000C046C File Offset: 0x000BE66C
		private void HandleLoadCompleted(object navState)
		{
			if (navState != null)
			{
				this._navState = navState;
			}
			if (this._asyncObjectConverter != null)
			{
				return;
			}
			if (this.PendingNavigationList.Count != 0 || this._navStatus != NavigationStatus.Navigated)
			{
				return;
			}
			NavigationService parentNavigationService = this.ParentNavigationService;
			this._navStatus = NavigationStatus.Idle;
			bool isNavigationInitiator = this.IsNavigationInitiator;
			this.FireLoadCompleted(isNavigationInitiator, this._navState);
			this._navState = null;
			if (this._webResponse != null)
			{
				this._webResponse.Close();
				this._webResponse = null;
			}
			if (!isNavigationInitiator && parentNavigationService != null)
			{
				parentNavigationService.PendingNavigationList.Remove(this);
				parentNavigationService.HandleLoadCompleted(null);
			}
		}

		/// <summary>Occurs when the <see cref="M:System.Windows.Navigation.NavigationService.StopLoading" /> method is called, or when a new navigation is requested while a current navigation is in progress.</summary>
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x0600298F RID: 10639 RVA: 0x000C04FF File Offset: 0x000BE6FF
		// (remove) Token: 0x06002990 RID: 10640 RVA: 0x000C0518 File Offset: 0x000BE718
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._stopped = (NavigationStoppedEventHandler)Delegate.Combine(this._stopped, value);
			}
			remove
			{
				this._stopped = (NavigationStoppedEventHandler)Delegate.Remove(this._stopped, value);
			}
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000C0534 File Offset: 0x000BE734
		private void FireNavigationStopped(object navState)
		{
			object extraData = (navState is NavigateInfo) ? null : navState;
			NavigationEventArgs e = new NavigationEventArgs(this.Source, this.Content, extraData, null, this.INavigatorHost, this.IsNavigationInitiator);
			if (this._stopped != null)
			{
				this._stopped(this.INavigatorHost, e);
			}
			if (this.Application != null && this.Application.CheckAccess())
			{
				this.Application.FireNavigationStopped(e);
			}
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000C05AC File Offset: 0x000BE7AC
		private void OnContentLoaded(object sender, RoutedEventArgs args)
		{
			FrameworkContentElement frameworkContentElement = this._bp as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.Loaded -= this.OnContentLoaded;
			}
			else
			{
				((FrameworkElement)this._bp).Loaded -= this.OnContentLoaded;
			}
			this.OnFirstContentLayout();
			this._cancelContentRenderedHandling = true;
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000C0605 File Offset: 0x000BE805
		private void ContentRenderedHandler(object sender, EventArgs args)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationContentRendered);
			if (this._cancelContentRenderedHandling)
			{
				this._cancelContentRenderedHandling = false;
				return;
			}
			this.OnFirstContentLayout();
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000C0630 File Offset: 0x000BE830
		private void OnFirstContentLayout()
		{
			if (this.CurrentSource != null)
			{
				string fragment = BindUriHelper.GetFragment(this.CurrentSource);
				if (!string.IsNullOrEmpty(fragment))
				{
					this.NavigateToFragment(fragment, false);
				}
			}
			if (this._journalScope != null)
			{
				JournalEntry currentEntry = this._journalScope.Journal.CurrentEntry;
				if (currentEntry != null && currentEntry.RootViewerState != null)
				{
					this.RestoreRootViewerState(currentEntry.RootViewerState);
					currentEntry.RootViewerState = null;
				}
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000C06A0 File Offset: 0x000BE8A0
		internal void DoNavigate(Uri source, NavigationMode f, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationAsyncWorkItem);
			if (this.IsDisposed)
			{
				return;
			}
			WebResponse webResponse = null;
			try
			{
				if (this._request is PackWebRequest)
				{
					webResponse = WpfWebRequestHelper.GetResponse(this._request);
					if (webResponse == null)
					{
						Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(this._request.RequestUri);
						throw new Exception(SR.Get("GetResponseFailed", new object[]
						{
							uriRelativeToPackAppBase.ToString()
						}));
					}
					this.GetObjectFromResponse(this._request, webResponse, source, navState);
				}
				else
				{
					RequestState state = new RequestState(this._request, source, navState, Dispatcher.CurrentDispatcher);
					this._request.BeginGetResponse(new AsyncCallback(this.HandleWebResponseOnRightDispatcher), state);
				}
			}
			catch (WebException e)
			{
				object extraData = (navState is NavigateInfo) ? null : navState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(source, extraData, this.INavigatorHost, this._request, webResponse, e)))
				{
					throw;
				}
			}
			catch (IOException e2)
			{
				object extraData2 = (navState is NavigateInfo) ? null : navState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(source, extraData2, this.INavigatorHost, this._request, webResponse, e2)))
				{
					throw;
				}
			}
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000C07D4 File Offset: 0x000BE9D4
		private bool FireNavigationFailed(NavigationFailedEventArgs e)
		{
			this._navStatus = NavigationStatus.NavigationFailed;
			try
			{
				if (this.NavigationFailed != null)
				{
					this.NavigationFailed(this.INavigatorHost, e);
				}
				if (!e.Handled)
				{
					NavigationWindow navigationWindow = this.FindNavigationWindow();
					if (navigationWindow != null && navigationWindow.NavigationService != this)
					{
						navigationWindow.NavigationService.FireNavigationFailed(e);
					}
				}
				if (!e.Handled && this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigationFailed(e);
				}
			}
			finally
			{
				if (this._navStatus == NavigationStatus.NavigationFailed)
				{
					this.DoStopLoading(true, false);
				}
			}
			return e.Handled;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000C0880 File Offset: 0x000BEA80
		private WebRequest CreateWebRequest(Uri resolvedDestinationUri, NavigateInfo navInfo)
		{
			WebRequest webRequest = null;
			try
			{
				webRequest = PackWebRequestFactory.CreateWebRequest(resolvedDestinationUri);
			}
			catch (NotSupportedException)
			{
				LaunchResult launchResult = AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(this.CurrentSource, resolvedDestinationUri, this.IsTopLevelContainer);
				if (launchResult == LaunchResult.NotLaunched)
				{
					throw;
				}
			}
			catch (SecurityException ex)
			{
				LaunchResult launchResult2 = LaunchResult.NotLaunched;
				if (ex.PermissionType == typeof(WebPermission))
				{
					launchResult2 = AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(this.CurrentSource, resolvedDestinationUri, this.IsTopLevelContainer);
				}
				if (launchResult2 == LaunchResult.NotLaunched)
				{
					throw;
				}
			}
			bool isRefresh = navInfo != null && navInfo.NavigationMode == NavigationMode.Refresh;
			WpfWebRequestHelper.ConfigCachePolicy(webRequest, isRefresh);
			return webRequest;
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000C0924 File Offset: 0x000BEB24
		private void HandleWebResponseOnRightDispatcher(IAsyncResult ar)
		{
			if (this.IsDisposed)
			{
				return;
			}
			Dispatcher callbackDispatcher = ((RequestState)ar.AsyncState).CallbackDispatcher;
			if (Dispatcher.CurrentDispatcher != callbackDispatcher)
			{
				callbackDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object unused)
				{
					this.HandleWebResponse(ar);
					return null;
				}), null);
				return;
			}
			callbackDispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
			{
				this.HandleWebResponse(ar);
				return null;
			}), null);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000C099C File Offset: 0x000BEB9C
		private void HandleWebResponse(IAsyncResult ar)
		{
			if (this.IsDisposed)
			{
				return;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationWebResponseReceived);
			RequestState requestState = (RequestState)ar.AsyncState;
			if (requestState.Request != this._request)
			{
				return;
			}
			WebResponse response = null;
			try
			{
				try
				{
					response = WpfWebRequestHelper.EndGetResponse(this._request, ar);
				}
				catch (WebException ex)
				{
					LaunchResult launchResult = LaunchResult.NotLaunched;
					SecurityException ex2 = ex.GetBaseException() as SecurityException;
					if (this._request.RequestUri.IsUnc && this._request.RequestUri.IsFile && ex2 != null && ex2.PermissionType == typeof(FileIOPermission))
					{
						launchResult = AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(this.CurrentSource, this._request.RequestUri, this.IsTopLevelContainer);
					}
					if (launchResult == LaunchResult.NotLaunched)
					{
						throw;
					}
					return;
				}
				this.GetObjectFromResponse(this._request, response, requestState.Source, requestState.NavState);
			}
			catch (WebException e)
			{
				object extraData = (requestState.NavState is NavigateInfo) ? null : requestState.NavState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(requestState.Source, extraData, this.INavigatorHost, this._request, response, e)))
				{
					throw;
				}
			}
			catch (IOException e2)
			{
				object extraData2 = (requestState.NavState is NavigateInfo) ? null : requestState.NavState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(requestState.Source, extraData2, this.INavigatorHost, this._request, response, e2)))
				{
					throw;
				}
			}
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000C0B30 File Offset: 0x000BED30
		private bool CanUseTopLevelBrowserForHTMLRendering()
		{
			return this.IsTopLevelContainer && this.IsParentedByBrowserWindow();
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000C0B44 File Offset: 0x000BED44
		private void GetObjectFromResponse(WebRequest request, WebResponse response, Uri destinationUri, object navState)
		{
			bool flag = false;
			ContentType contentType = WpfWebRequestHelper.GetContentType(response);
			try
			{
				Stream responseStream = response.GetResponseStream();
				if (responseStream == null)
				{
					Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(this._request.RequestUri);
					throw new Exception(SR.Get("GetStreamFailed", new object[]
					{
						uriRelativeToPackAppBase.ToString()
					}));
				}
				long contentLength = response.ContentLength;
				Uri uriRelativeToPackAppBase2 = BindUriHelper.GetUriRelativeToPackAppBase(destinationUri);
				NavigateInfo navigateInfo = navState as NavigateInfo;
				bool flag2 = this.SandboxExternalContent && !BaseUriHelper.IsPackApplicationUri(destinationUri) && MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(contentType);
				if (flag2 && !SecurityHelper.CheckUnmanagedCodePermission())
				{
					flag2 = false;
				}
				BindStream s = new BindStream(responseStream, contentLength, uriRelativeToPackAppBase2, this, Dispatcher.CurrentDispatcher);
				Invariant.Assert(this._webResponse == null && this._asyncObjectConverter == null);
				this._webResponse = response;
				this._asyncObjectConverter = null;
				Invariant.Assert(!this.ShouldDelegateXamlViewerNavigationToBrowser(navigateInfo, destinationUri), "TopLevel navigation away from loose xaml is already delageted to browser. It should never reach here.");
				object objectAndCloseStream = MimeObjectFactory.GetObjectAndCloseStream(s, contentType, destinationUri, this.CanUseTopLevelBrowserForHTMLRendering(), flag2, true, this.IsJournalNavigation(navigateInfo), out this._asyncObjectConverter);
				if (objectAndCloseStream != null)
				{
					if (this._request == request)
					{
						((IContentContainer)this).OnContentReady(contentType, objectAndCloseStream, destinationUri, navState);
						flag = true;
					}
				}
				else
				{
					try
					{
						if (!this.IsTopLevelContainer || BrowserInteropHelper.IsInitialViewerNavigation)
						{
							throw new InvalidOperationException(SR.Get("FailedToConvertResource"));
						}
						this.DelegateToBrowser(response is PackWebResponse, destinationUri);
					}
					finally
					{
						this.DrainResponseStreamForPartialCacheFileBug(responseStream);
						responseStream.Close();
						this.ResetPendingNavigationState(this._navStatus);
					}
				}
			}
			finally
			{
				if (!flag)
				{
					response.Close();
					this._webResponse = null;
					if (this._asyncObjectConverter != null)
					{
						this._asyncObjectConverter.CancelAsync();
						this._asyncObjectConverter = null;
					}
				}
			}
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000C0D18 File Offset: 0x000BEF18
		private void DelegateToBrowser(bool isPack, Uri destinationUri)
		{
			try
			{
				if (isPack)
				{
					destinationUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(destinationUri);
				}
				if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationLaunchBrowser, EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, destinationUri.ToString());
				}
				AppSecurityManager.SafeLaunchBrowserDemandWhenUnsafe(this.CurrentSource, destinationUri, this.IsTopLevelContainer);
			}
			finally
			{
				this.InformBrowserAboutStoppedNavigation();
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000C0D84 File Offset: 0x000BEF84
		private void DrainResponseStreamForPartialCacheFileBug(Stream s)
		{
			if (this._request is HttpWebRequest && HttpWebRequest.DefaultCachePolicy != null && HttpWebRequest.DefaultCachePolicy is HttpRequestCachePolicy)
			{
				StreamReader streamReader = new StreamReader(s);
				streamReader.ReadToEnd();
				streamReader.Close();
			}
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000C0DC8 File Offset: 0x000BEFC8
		internal void DoNavigate(object bp, NavigationMode navFlags, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationAsyncWorkItem);
			if (this.IsDisposed)
			{
				return;
			}
			NavigateInfo navigateInfo = navState as NavigateInfo;
			Invariant.Assert(navFlags != NavigationMode.Refresh ^ bp == this._bp, "Navigating to the same object should be handled as fragment navigation, except for Refresh.");
			Uri orgUri = (navigateInfo == null) ? null : navigateInfo.Source;
			Uri resolvedUri = BindUriHelper.GetResolvedUri(null, orgUri);
			((IContentContainer)this).OnContentReady(null, bp, resolvedUri, navState);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000C0E30 File Offset: 0x000BF030
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private JournalEntry UpdateJournal(NavigationMode navigationMode, JournalReason journalReason, JournalEntry destinationJournalEntry)
		{
			JournalEntry journalEntry = null;
			if (!this._doNotJournalCurrentContent)
			{
				journalEntry = this.MakeJournalEntry(journalReason);
			}
			if (journalEntry == null)
			{
				this._doNotJournalCurrentContent = false;
				if ((navigationMode == NavigationMode.Back || navigationMode == NavigationMode.Forward) && this.JournalScope != null)
				{
					this.JournalScope.Journal.CommitJournalNavigation(destinationJournalEntry);
				}
				return null;
			}
			JournalNavigationScope journalNavigationScope = this.EnsureJournal();
			if (journalNavigationScope == null)
			{
				return null;
			}
			PageFunctionBase pageFunctionBase = this._bp as PageFunctionBase;
			if (pageFunctionBase != null && navigationMode == NavigationMode.New && pageFunctionBase.Content == null)
			{
				journalEntry.EntryType = JournalEntryType.UiLess;
			}
			journalNavigationScope.Journal.UpdateCurrentEntry(journalEntry);
			if (journalEntry.IsNavigable())
			{
				this.CallUpdateTravelLog(navigationMode == NavigationMode.New);
			}
			if (navigationMode == NavigationMode.New)
			{
				journalNavigationScope.Journal.RecordNewNavigation();
			}
			else
			{
				journalNavigationScope.Journal.CommitJournalNavigation(destinationJournalEntry);
			}
			this._customContentStateToSave = null;
			return journalEntry;
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000C0EEC File Offset: 0x000BF0EC
		internal JournalEntry MakeJournalEntry(JournalReason journalReason)
		{
			if (this._bp == null)
			{
				return null;
			}
			if (this._journalEntryGroupState == null)
			{
				this._journalEntryGroupState = new JournalEntryGroupState(this._guidId, this._contentId);
			}
			bool flag = this.IsContentKeepAlive();
			PageFunctionBase pageFunctionBase = this._bp as PageFunctionBase;
			JournalEntry journalEntry;
			if (pageFunctionBase != null)
			{
				if (flag)
				{
					journalEntry = new JournalEntryPageFunctionKeepAlive(this._journalEntryGroupState, pageFunctionBase);
				}
				else
				{
					Uri uri = pageFunctionBase.GetValue(BaseUriHelper.BaseUriProperty) as Uri;
					if (uri != null)
					{
						Invariant.Assert(uri.IsAbsoluteUri, "BaseUri for root element should be absolute.");
						Uri markupUri;
						if (this._currentCleanSource != null && !BindUriHelper.StartWithFragment(this._currentCleanSource))
						{
							markupUri = this._currentSource;
						}
						else
						{
							markupUri = uri;
						}
						journalEntry = new JournalEntryPageFunctionUri(this._journalEntryGroupState, pageFunctionBase, markupUri);
					}
					else
					{
						journalEntry = new JournalEntryPageFunctionType(this._journalEntryGroupState, pageFunctionBase);
					}
				}
				journalEntry.Source = this._currentCleanSource;
			}
			else if (flag)
			{
				journalEntry = new JournalEntryKeepAlive(this._journalEntryGroupState, this._currentCleanSource, this._bp);
			}
			else
			{
				journalEntry = new JournalEntryUri(this._journalEntryGroupState, this._currentCleanSource);
			}
			CustomContentState customContentState = this._customContentStateToSave;
			if (customContentState == null)
			{
				IProvideCustomContentState provideCustomContentState = this._bp as IProvideCustomContentState;
				if (provideCustomContentState != null)
				{
					customContentState = provideCustomContentState.GetContentState();
				}
			}
			if (customContentState != null)
			{
				Type type = customContentState.GetType();
				if (!type.IsSerializable)
				{
					throw new SystemException(SR.Get("CustomContentStateMustBeSerializable", new object[]
					{
						type
					}));
				}
				journalEntry.CustomContentState = customContentState;
			}
			if (this._rootViewerStateToSave != null)
			{
				journalEntry.RootViewerState = this._rootViewerStateToSave;
				this._rootViewerStateToSave = null;
			}
			else
			{
				journalEntry.RootViewerState = this.GetRootViewerState(journalReason);
			}
			string text = null;
			if (journalEntry.CustomContentState != null)
			{
				text = journalEntry.CustomContentState.JournalEntryName;
			}
			if (string.IsNullOrEmpty(text))
			{
				DependencyObject dependencyObject = this._bp as DependencyObject;
				if (dependencyObject != null)
				{
					text = (string)dependencyObject.GetValue(JournalEntry.NameProperty);
					if (string.IsNullOrEmpty(text) && dependencyObject is Page)
					{
						text = (dependencyObject as Page).Title;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (this._currentSource != null)
					{
						string fragment = BindUriHelper.GetFragment(this._currentSource);
						if (!string.IsNullOrEmpty(fragment))
						{
							text = text + "#" + fragment;
						}
					}
				}
				else
				{
					NavigationWindow navigationWindow = (this.JournalScope == null) ? null : (this.JournalScope.NavigatorHost as NavigationWindow);
					if (navigationWindow != null && this == navigationWindow.NavigationService && !string.IsNullOrEmpty(navigationWindow.Title))
					{
						if (this.CurrentSource != null)
						{
							text = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
							{
								navigationWindow.Title,
								JournalEntry.GetDisplayName(this._currentSource, SiteOfOriginContainer.SiteOfOrigin)
							});
						}
						else
						{
							text = navigationWindow.Title;
						}
					}
					else if (this.CurrentSource != null)
					{
						text = JournalEntry.GetDisplayName(this._currentSource, SiteOfOriginContainer.SiteOfOrigin);
					}
					else
					{
						text = SR.Get("Untitled");
					}
				}
			}
			journalEntry.Name = text;
			if (journalReason == JournalReason.NewContentNavigation)
			{
				journalEntry.SaveState(this._bp);
			}
			return journalEntry;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000C1200 File Offset: 0x000BF400
		internal void RequestCustomContentStateOnAppShutdown()
		{
			this._isNavInitiator = false;
			this._isNavInitiatorValid = true;
			this.FireNavigating(null, null, null, null);
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000C121B File Offset: 0x000BF41B
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CallUpdateTravelLog(bool addNewEntry)
		{
			if (this.HasTravelLogIntegration)
			{
				this.Application.BrowserCallbackServices.UpdateTravelLog(addNewEntry);
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x00036A24 File Offset: 0x00034C24
		internal Application Application
		{
			get
			{
				return Application.Current;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x000C1236 File Offset: 0x000BF436
		// (set) Token: 0x060029A5 RID: 10661 RVA: 0x000C123E File Offset: 0x000BF43E
		internal bool AllowWindowNavigation
		{
			private get
			{
				return this._allowWindowNavigation;
			}
			set
			{
				this._allowWindowNavigation = value;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060029A6 RID: 10662 RVA: 0x000C1247 File Offset: 0x000BF447
		// (set) Token: 0x060029A7 RID: 10663 RVA: 0x000C124F File Offset: 0x000BF44F
		internal long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
			set
			{
				this._bytesRead = value;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x000C1258 File Offset: 0x000BF458
		// (set) Token: 0x060029A9 RID: 10665 RVA: 0x000C1260 File Offset: 0x000BF460
		internal long MaxBytes
		{
			get
			{
				return this._maxBytes;
			}
			set
			{
				this._maxBytes = value;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x000C1269 File Offset: 0x000BF469
		internal uint ContentId
		{
			get
			{
				return this._contentId;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000C1271 File Offset: 0x000BF471
		// (set) Token: 0x060029AC RID: 10668 RVA: 0x000C1279 File Offset: 0x000BF479
		internal Guid GuidId
		{
			get
			{
				return this._guidId;
			}
			set
			{
				this._guidId = value;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x000C1282 File Offset: 0x000BF482
		internal NavigationService ParentNavigationService
		{
			get
			{
				return this._parentNavigationService;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060029AE RID: 10670 RVA: 0x000C128A File Offset: 0x000BF48A
		internal bool CanReloadFromUri
		{
			get
			{
				return !(this._currentCleanSource == null) && !BindUriHelper.StartWithFragment(this._currentCleanSource) && !BindUriHelper.StartWithFragment(BindUriHelper.GetUriRelativeToPackAppBase(this._currentCleanSource));
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x000C12BC File Offset: 0x000BF4BC
		internal ArrayList ChildNavigationServices
		{
			get
			{
				return this._childNavigationServices;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x000C12C4 File Offset: 0x000BF4C4
		private FinishEventHandler FinishHandler
		{
			get
			{
				if (this._finishHandler == null)
				{
					this._finishHandler = new FinishEventHandler(this.HandleFinish);
				}
				return this._finishHandler;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000C12E6 File Offset: 0x000BF4E6
		private bool HasTravelLogIntegration
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this.IsParentedByBrowserWindow() && ApplicationProxyInternal.Current.RootBrowserWindow.HasTravelLogIntegration;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000C1301 File Offset: 0x000BF501
		private bool IsTopLevelContainer
		{
			get
			{
				return this.INavigatorHost is NavigationWindow || (this.Application != null && this.Application.CheckAccess() && this.Application.NavService == this);
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060029B3 RID: 10675 RVA: 0x000C1338 File Offset: 0x000BF538
		private bool IsJournalLevelContainer
		{
			get
			{
				JournalNavigationScope journalScope = this.JournalScope;
				return journalScope != null && journalScope.RootNavigationService == this;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000C135C File Offset: 0x000BF55C
		private bool SandboxExternalContent
		{
			get
			{
				DependencyObject dependencyObject = this.INavigatorHost as DependencyObject;
				return dependencyObject != null && (bool)dependencyObject.GetValue(Frame.SandboxExternalContentProperty);
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000C138A File Offset: 0x000BF58A
		// (set) Token: 0x060029B6 RID: 10678 RVA: 0x000C1394 File Offset: 0x000BF594
		internal INavigator INavigatorHost
		{
			get
			{
				return this._navigatorHost;
			}
			set
			{
				RequestNavigateEventHandler handler = new RequestNavigateEventHandler(this.OnRequestNavigate);
				if (this._navigatorHost != null)
				{
					IInputElement inputElement = this._navigatorHost as IInputElement;
					if (inputElement != null)
					{
						inputElement.RemoveHandler(Hyperlink.RequestNavigateEvent, handler);
					}
					IDownloader downloader = this._navigatorHost as IDownloader;
					if (downloader != null)
					{
						downloader.ContentRendered -= this.ContentRenderedHandler;
					}
				}
				if (value != null)
				{
					IInputElement inputElement2 = value as IInputElement;
					if (inputElement2 != null)
					{
						inputElement2.AddHandler(Hyperlink.RequestNavigateEvent, handler);
					}
					IDownloader downloader2 = value as IDownloader;
					if (downloader2 != null)
					{
						downloader2.ContentRendered += this.ContentRenderedHandler;
					}
				}
				this._navigatorHost = value;
				this._navigatorHostImpl = (value as INavigatorImpl);
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000C143D File Offset: 0x000BF63D
		// (set) Token: 0x060029B8 RID: 10680 RVA: 0x000C1445 File Offset: 0x000BF645
		internal NavigationStatus NavStatus
		{
			get
			{
				return this._navStatus;
			}
			set
			{
				this._navStatus = value;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x000C144E File Offset: 0x000BF64E
		internal ArrayList PendingNavigationList
		{
			get
			{
				return this._pendingNavigationList;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000C1456 File Offset: 0x000BF656
		internal WebBrowser WebBrowser
		{
			get
			{
				return this._webBrowser;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x000C1460 File Offset: 0x000BF660
		internal bool IsDisposed
		{
			get
			{
				bool flag = false;
				if (this.Application != null && this.Application.CheckAccess() && Application.IsShuttingDown)
				{
					flag = true;
				}
				return this._disposed || flag;
			}
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000C1498 File Offset: 0x000BF698
		internal void Dispose()
		{
			this._disposed = true;
			this.StopLoading();
			foreach (object obj in this.ChildNavigationServices)
			{
				NavigationService navigationService = (NavigationService)obj;
				navigationService.Dispose();
			}
			this._journalScope = null;
			this._bp = null;
			this._currentSource = null;
			this._currentCleanSource = null;
			this._oldRootVisual = null;
			this._childNavigationServices.Clear();
			this._parentNavigationService = null;
			this._webBrowser = null;
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000C153C File Offset: 0x000BF73C
		private NavigationWindow FindNavigationWindow()
		{
			NavigationService navigationService = this;
			while (navigationService != null && navigationService.INavigatorHost != null)
			{
				NavigationWindow navigationWindow = navigationService.INavigatorHost as NavigationWindow;
				if (navigationWindow != null)
				{
					return navigationWindow;
				}
				navigationService = navigationService.ParentNavigationService;
			}
			return null;
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000C1571 File Offset: 0x000BF771
		internal static bool IsPageFunction(object content)
		{
			return content is PageFunctionBase;
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000C1580 File Offset: 0x000BF780
		private void SetupPageFunctionHandlers(object bp)
		{
			PageFunctionBase pageFunctionBase = bp as PageFunctionBase;
			if (bp == null)
			{
				return;
			}
			pageFunctionBase.FinishHandler = this.FinishHandler;
			ReturnEventSaver returnEventSaver = new ReturnEventSaver();
			returnEventSaver._Detach(pageFunctionBase);
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000C15B4 File Offset: 0x000BF7B4
		private void HandlePageFunction(NavigateInfo navInfo)
		{
			PageFunctionBase pageFunctionBase = (PageFunctionBase)this._bp;
			if (this.IsJournalNavigation(navInfo))
			{
				pageFunctionBase._Resume = true;
			}
			if (!pageFunctionBase._Resume)
			{
				pageFunctionBase.CallStart();
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000C15EC File Offset: 0x000BF7EC
		private void HandleFinish(PageFunctionBase endingPF, object ReturnEventArgs)
		{
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationPageFunctionReturn, EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, endingPF.ToString());
			}
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("WindowAlreadyClosed"));
			}
			Journal journal = this.JournalScope.Journal;
			PageFunctionBase pageFunctionBase = null;
			int parentPageJournalIndex = JournalEntryPageFunction.GetParentPageJournalIndex(this, journal, endingPF);
			if (endingPF.RemoveFromJournal)
			{
				this.DoRemoveFromJournal(endingPF, parentPageJournalIndex);
			}
			if (parentPageJournalIndex != -1)
			{
				JournalEntryPageFunction journalEntryPageFunction = journal[parentPageJournalIndex] as JournalEntryPageFunction;
				if (journalEntryPageFunction != null)
				{
					pageFunctionBase = journalEntryPageFunction.ResumePageFunction();
					pageFunctionBase.FinishHandler = this.FinishHandler;
					this.FireChildPageFunctionReturnEvent(pageFunctionBase, endingPF, ReturnEventArgs);
				}
			}
			if (this._navigateQueueItem == null)
			{
				if (parentPageJournalIndex != -1 && parentPageJournalIndex < journal.TotalCount && !this.IsDisposed)
				{
					this.NavigateToParentPage(endingPF, pageFunctionBase, ReturnEventArgs, parentPageJournalIndex);
					return;
				}
			}
			else
			{
				if (parentPageJournalIndex < journal.TotalCount)
				{
					JournalEntryPageFunction journalEntryPageFunction2 = (JournalEntryPageFunction)journal[parentPageJournalIndex];
					journalEntryPageFunction2.SaveState(pageFunctionBase);
				}
				pageFunctionBase.FinishHandler = null;
			}
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000C16E0 File Offset: 0x000BF8E0
		private void FireChildPageFunctionReturnEvent(object parentElem, PageFunctionBase childPF, object ReturnEventArgs)
		{
			ReturnEventSaver saver = childPF._Saver;
			if (saver != null)
			{
				saver._Attach(parentElem, childPF);
				Window window = null;
				DependencyObject dependencyObject = parentElem as DependencyObject;
				if (dependencyObject != null && !dependencyObject.IsSealed)
				{
					dependencyObject.SetValue(NavigationService.NavigationServiceProperty, this);
					DependencyObject dependencyObject2 = this.INavigatorHost as DependencyObject;
					if (dependencyObject2 != null && (window = Window.GetWindow(dependencyObject2)) != null)
					{
						dependencyObject.SetValue(Window.IWindowServiceProperty, window);
					}
				}
				try
				{
					childPF._OnFinish(ReturnEventArgs);
				}
				catch
				{
					this.DoStopLoading(true, false);
					throw;
				}
				finally
				{
					saver._Detach(childPF);
					if (dependencyObject != null && !dependencyObject.IsSealed)
					{
						dependencyObject.ClearValue(NavigationService.NavigationServiceProperty);
						if (window != null)
						{
							dependencyObject.ClearValue(Window.IWindowServiceProperty);
						}
					}
				}
			}
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000C17A4 File Offset: 0x000BF9A4
		private void DoRemoveFromJournal(PageFunctionBase finishingChildPageFunction, int parentEntryIndex)
		{
			if (!finishingChildPageFunction.RemoveFromJournal)
			{
				return;
			}
			bool flag = false;
			Journal journal = this.JournalScope.Journal;
			int i = parentEntryIndex + 1;
			while (i < journal.TotalCount)
			{
				if (!flag)
				{
					JournalEntryPageFunction journalEntryPageFunction = journal[i] as JournalEntryPageFunction;
					flag = (journalEntryPageFunction != null && journalEntryPageFunction.PageFunctionId == finishingChildPageFunction.PageFunctionId);
				}
				if (flag)
				{
					journal.RemoveEntryInternal(i);
				}
				else
				{
					i++;
				}
			}
			if (flag)
			{
				journal.UpdateView();
			}
			else if (this._bp == finishingChildPageFunction)
			{
				journal.ClearForwardStack();
			}
			this._doNotJournalCurrentContent = true;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000C1834 File Offset: 0x000BFA34
		private void NavigateToParentPage(PageFunctionBase finishingChildPageFunction, PageFunctionBase parentPF, object returnEventArgs, int parentIndex)
		{
			JournalEntry journalEntry = this.JournalScope.Journal[parentIndex];
			if (parentPF != null)
			{
				if (journalEntry.EntryType == JournalEntryType.UiLess)
				{
					throw new InvalidOperationException(SR.Get("UiLessPageFunctionNotCallingOnReturn"));
				}
				NavigateInfo navigationState = finishingChildPageFunction.RemoveFromJournal ? new NavigateInfo(journalEntry.Source, NavigationMode.Back, journalEntry) : new NavigateInfo(journalEntry.Source, NavigationMode.New);
				this.Navigate(parentPF, navigationState);
				return;
			}
			else
			{
				PageFunctionReturnInfo navigationState2 = finishingChildPageFunction.RemoveFromJournal ? new PageFunctionReturnInfo(finishingChildPageFunction, journalEntry.Source, NavigationMode.Back, journalEntry, returnEventArgs) : new PageFunctionReturnInfo(finishingChildPageFunction, journalEntry.Source, NavigationMode.New, null, returnEventArgs);
				if (journalEntry is JournalEntryUri)
				{
					this.Navigate(journalEntry.Source, navigationState2);
					return;
				}
				if (journalEntry is JournalEntryKeepAlive)
				{
					object keepAliveRoot = ((JournalEntryKeepAlive)journalEntry).KeepAliveRoot;
					this.Navigate(keepAliveRoot, navigationState2);
				}
				return;
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000C18FC File Offset: 0x000BFAFC
		private bool IsValidRootElement(object bp)
		{
			bool result = true;
			if (!this.AllowWindowNavigation && bp != null && bp is Window)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060029C6 RID: 10694 RVA: 0x000C1924 File Offset: 0x000BFB24
		// (remove) Token: 0x060029C7 RID: 10695 RVA: 0x000C195C File Offset: 0x000BFB5C
		internal event BPReadyEventHandler BPReady;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x060029C8 RID: 10696 RVA: 0x000C1994 File Offset: 0x000BFB94
		// (remove) Token: 0x060029C9 RID: 10697 RVA: 0x000C19CC File Offset: 0x000BFBCC
		internal event BPReadyEventHandler PreBPReady;

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000C1A01 File Offset: 0x000BFC01
		private JournalNavigationScope JournalScope
		{
			get
			{
				if (this._journalScope == null && this._navigatorHost != null)
				{
					this._journalScope = this._navigatorHost.GetJournal(false);
				}
				return this._journalScope;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000C1A2C File Offset: 0x000BFC2C
		private bool IsNavigationInitiator
		{
			get
			{
				if (!this._isNavInitiatorValid)
				{
					this._isNavInitiator = this.IsTopLevelContainer;
					if (this._parentNavigationService != null)
					{
						if (!this._parentNavigationService.PendingNavigationList.Contains(this))
						{
							this._isNavInitiator = true;
						}
					}
					else if (this.IsJournalLevelContainer)
					{
						this._isNavInitiator = true;
					}
					this._isNavInitiatorValid = true;
				}
				return this._isNavInitiator;
			}
		}

		// Token: 0x04001BEF RID: 7151
		internal static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.RegisterAttached("NavigationService", typeof(NavigationService), typeof(NavigationService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001BF1 RID: 7153
		private NavigatingCancelEventHandler _navigating;

		// Token: 0x04001BF2 RID: 7154
		private NavigatedEventHandler _navigated;

		// Token: 0x04001BF3 RID: 7155
		private NavigationProgressEventHandler _navigationProgress;

		// Token: 0x04001BF4 RID: 7156
		private LoadCompletedEventHandler _loadCompleted;

		// Token: 0x04001BF5 RID: 7157
		private FragmentNavigationEventHandler _fragmentNavigation;

		// Token: 0x04001BF6 RID: 7158
		private NavigationStoppedEventHandler _stopped;

		// Token: 0x04001BF9 RID: 7161
		private object _bp;

		// Token: 0x04001BFA RID: 7162
		private uint _contentId;

		// Token: 0x04001BFB RID: 7163
		private Uri _currentSource;

		// Token: 0x04001BFC RID: 7164
		private Uri _currentCleanSource;

		// Token: 0x04001BFD RID: 7165
		private JournalEntryGroupState _journalEntryGroupState;

		// Token: 0x04001BFE RID: 7166
		private bool _doNotJournalCurrentContent;

		// Token: 0x04001BFF RID: 7167
		private bool _cancelContentRenderedHandling;

		// Token: 0x04001C00 RID: 7168
		private CustomContentState _customContentStateToSave;

		// Token: 0x04001C01 RID: 7169
		private CustomJournalStateInternal _rootViewerStateToSave;

		// Token: 0x04001C02 RID: 7170
		private WebRequest _request;

		// Token: 0x04001C03 RID: 7171
		private object _navState;

		// Token: 0x04001C04 RID: 7172
		private WebResponse _webResponse;

		// Token: 0x04001C05 RID: 7173
		private XamlReader _asyncObjectConverter;

		// Token: 0x04001C06 RID: 7174
		private bool _isNavInitiator;

		// Token: 0x04001C07 RID: 7175
		private bool _isNavInitiatorValid;

		// Token: 0x04001C08 RID: 7176
		private bool _allowWindowNavigation;

		// Token: 0x04001C09 RID: 7177
		private Guid _guidId = Guid.Empty;

		// Token: 0x04001C0A RID: 7178
		private INavigator _navigatorHost;

		// Token: 0x04001C0B RID: 7179
		private INavigatorImpl _navigatorHostImpl;

		// Token: 0x04001C0C RID: 7180
		private JournalNavigationScope _journalScope;

		// Token: 0x04001C0D RID: 7181
		private ArrayList _childNavigationServices = new ArrayList(2);

		// Token: 0x04001C0E RID: 7182
		private NavigationService _parentNavigationService;

		// Token: 0x04001C0F RID: 7183
		private bool _disposed;

		// Token: 0x04001C10 RID: 7184
		private FinishEventHandler _finishHandler;

		// Token: 0x04001C11 RID: 7185
		private NavigationStatus _navStatus;

		// Token: 0x04001C12 RID: 7186
		private ArrayList _pendingNavigationList = new ArrayList(2);

		// Token: 0x04001C13 RID: 7187
		private ArrayList _recursiveNavigateList = new ArrayList(2);

		// Token: 0x04001C14 RID: 7188
		private NavigateQueueItem _navigateQueueItem;

		// Token: 0x04001C15 RID: 7189
		private long _bytesRead;

		// Token: 0x04001C16 RID: 7190
		private long _maxBytes;

		// Token: 0x04001C17 RID: 7191
		private Visual _oldRootVisual;

		// Token: 0x04001C18 RID: 7192
		private const int _noParentPage = -1;

		// Token: 0x04001C19 RID: 7193
		private WebBrowser _webBrowser;
	}
}
