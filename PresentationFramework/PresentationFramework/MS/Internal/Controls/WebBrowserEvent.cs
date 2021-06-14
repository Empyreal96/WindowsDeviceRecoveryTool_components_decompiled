using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows.Controls;
using System.Windows.Navigation;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.IO.Packaging;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x0200075A RID: 1882
	[ClassInterface(ClassInterfaceType.None)]
	internal class WebBrowserEvent : InternalDispatchObject<UnsafeNativeMethods.DWebBrowserEvents2>, UnsafeNativeMethods.DWebBrowserEvents2
	{
		// Token: 0x060077C4 RID: 30660 RVA: 0x002233A8 File Offset: 0x002215A8
		[SecurityCritical]
		public WebBrowserEvent(WebBrowser parent)
		{
			this._parent = parent;
		}

		// Token: 0x060077C5 RID: 30661 RVA: 0x002233B8 File Offset: 0x002215B8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void BeforeNavigate2(object pDisp, ref object url, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				if (targetFrameName == null)
				{
					targetFrameName = "";
				}
				if (headers == null)
				{
					headers = "";
				}
				string text = (string)url;
				Uri uri = string.IsNullOrEmpty(text) ? null : new Uri(text);
				UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
				if (this._parent.AxIWebBrowser2 == webBrowser)
				{
					if (this._parent.NavigatingToAboutBlank && string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) != 0)
					{
						this._parent.NavigatingToAboutBlank = false;
					}
					if (!this._parent.NavigatingToAboutBlank && !SecurityHelper.CallerHasWebPermission(uri) && !WebBrowserEvent.IsAllowedScriptScheme(uri))
					{
						flag2 = true;
					}
					else
					{
						if (this._parent.NavigatingToAboutBlank)
						{
							uri = null;
						}
						NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(uri, null, null, null, NavigationMode.New, null, null, true);
						Guid lastNavigation = this._parent.LastNavigation;
						this._parent.OnNavigating(navigatingCancelEventArgs);
						if (this._parent.LastNavigation != lastNavigation)
						{
							flag = true;
						}
						flag2 = navigatingCancelEventArgs.Cancel;
					}
				}
			}
			catch
			{
				flag2 = true;
			}
			finally
			{
				if (flag2 && !flag)
				{
					this._parent.CleanInternalState();
				}
				if (flag2 || flag)
				{
					cancel = true;
				}
			}
		}

		// Token: 0x060077C6 RID: 30662 RVA: 0x002234F4 File Offset: 0x002216F4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool IsAllowedScriptScheme(Uri uri)
		{
			return uri != null && (uri.Scheme == "javascript" || uri.Scheme == "vbscript");
		}

		// Token: 0x060077C7 RID: 30663 RVA: 0x00223528 File Offset: 0x00221728
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void NavigateComplete2(object pDisp, ref object url)
		{
			UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
			if (this._parent.AxIWebBrowser2 == webBrowser)
			{
				if (this._parent.DocumentStream != null)
				{
					Invariant.Assert(this._parent.NavigatingToAboutBlank && string.Compare((string)url, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					try
					{
						UnsafeNativeMethods.IHTMLDocument nativeHTMLDocument = this._parent.NativeHTMLDocument;
						if (nativeHTMLDocument != null)
						{
							UnsafeNativeMethods.IPersistStreamInit persistStreamInit = nativeHTMLDocument as UnsafeNativeMethods.IPersistStreamInit;
							System.Runtime.InteropServices.ComTypes.IStream pstm = new ManagedIStream(this._parent.DocumentStream);
							persistStreamInit.Load(pstm);
						}
						return;
					}
					finally
					{
						this._parent.DocumentStream = null;
					}
				}
				string text = (string)url;
				if (this._parent.NavigatingToAboutBlank)
				{
					Invariant.Assert(string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					text = null;
				}
				Uri uri = string.IsNullOrEmpty(text) ? null : new Uri(text);
				NavigationEventArgs e = new NavigationEventArgs(uri, null, null, null, null, true);
				this._parent.OnNavigated(e);
			}
		}

		// Token: 0x060077C8 RID: 30664 RVA: 0x00223630 File Offset: 0x00221830
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void DocumentComplete(object pDisp, ref object url)
		{
			UnsafeNativeMethods.IWebBrowser2 webBrowser = (UnsafeNativeMethods.IWebBrowser2)pDisp;
			if (this._parent.AxIWebBrowser2 == webBrowser)
			{
				string text = (string)url;
				if (this._parent.NavigatingToAboutBlank)
				{
					Invariant.Assert(string.Compare(text, "about:blank", StringComparison.OrdinalIgnoreCase) == 0);
					text = null;
				}
				Uri uri = string.IsNullOrEmpty(text) ? null : new Uri(text);
				NavigationEventArgs e = new NavigationEventArgs(uri, null, null, null, null, true);
				this._parent.OnLoadCompleted(e);
			}
		}

		// Token: 0x060077C9 RID: 30665 RVA: 0x002236A7 File Offset: 0x002218A7
		[SecurityCritical]
		public void CommandStateChange(long command, bool enable)
		{
			if (command == 2L)
			{
				this._parent._canGoBack = enable;
				return;
			}
			if (command == 1L)
			{
				this._parent._canGoForward = enable;
			}
		}

		// Token: 0x060077CA RID: 30666 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void TitleChange(string text)
		{
		}

		// Token: 0x060077CB RID: 30667 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void SetSecureLockIcon(int secureLockIcon)
		{
		}

		// Token: 0x060077CC RID: 30668 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void NewWindow2(ref object ppDisp, ref bool cancel)
		{
		}

		// Token: 0x060077CD RID: 30669 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void ProgressChange(int progress, int progressMax)
		{
		}

		// Token: 0x060077CE RID: 30670 RVA: 0x002236CC File Offset: 0x002218CC
		[SecurityCritical]
		public void StatusTextChange(string text)
		{
			this._parent.RaiseEvent(new RequestSetStatusBarEventArgs(text));
		}

		// Token: 0x060077CF RID: 30671 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void DownloadBegin()
		{
		}

		// Token: 0x060077D0 RID: 30672 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void FileDownload(ref bool activeDocument, ref bool cancel)
		{
		}

		// Token: 0x060077D1 RID: 30673 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void PrivacyImpactedStateChange(bool bImpacted)
		{
		}

		// Token: 0x060077D2 RID: 30674 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
		{
		}

		// Token: 0x060077D3 RID: 30675 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void PrintTemplateTeardown(object pDisp)
		{
		}

		// Token: 0x060077D4 RID: 30676 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void PrintTemplateInstantiation(object pDisp)
		{
		}

		// Token: 0x060077D5 RID: 30677 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
		{
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void ClientToHostWindow(ref long cX, ref long cY)
		{
		}

		// Token: 0x060077D7 RID: 30679 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowClosing(bool isChildWindow, ref bool cancel)
		{
		}

		// Token: 0x060077D8 RID: 30680 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowSetHeight(int height)
		{
		}

		// Token: 0x060077D9 RID: 30681 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowSetWidth(int width)
		{
		}

		// Token: 0x060077DA RID: 30682 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowSetTop(int top)
		{
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowSetLeft(int left)
		{
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowSetResizable(bool resizable)
		{
		}

		// Token: 0x060077DD RID: 30685 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnTheaterMode(bool theaterMode)
		{
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnFullScreen(bool fullScreen)
		{
		}

		// Token: 0x060077DF RID: 30687 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnStatusBar(bool statusBar)
		{
		}

		// Token: 0x060077E0 RID: 30688 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnMenuBar(bool menuBar)
		{
		}

		// Token: 0x060077E1 RID: 30689 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnToolBar(bool toolBar)
		{
		}

		// Token: 0x060077E2 RID: 30690 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnVisible(bool visible)
		{
		}

		// Token: 0x060077E3 RID: 30691 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void OnQuit()
		{
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void PropertyChange(string szProperty)
		{
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void DownloadComplete()
		{
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void SetPhishingFilterStatus(uint phishingFilterStatus)
		{
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		public void WindowStateChanged(uint dwFlags, uint dwValidFlagsMask)
		{
		}

		// Token: 0x040038E0 RID: 14560
		private WebBrowser _parent;
	}
}
