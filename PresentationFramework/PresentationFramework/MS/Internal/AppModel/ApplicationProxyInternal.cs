using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal.Documents.Application;
using MS.Internal.IO.Packaging;
using MS.Internal.Utility;
using MS.Utility;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x0200076D RID: 1901
	internal sealed class ApplicationProxyInternal : MarshalByRefObject
	{
		// Token: 0x06007888 RID: 30856 RVA: 0x00225504 File Offset: 0x00223704
		[SecurityCritical]
		internal ApplicationProxyInternal()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AppProxyCtor);
			if (ApplicationProxyInternal._proxyInstance != null)
			{
				throw new InvalidOperationException(SR.Get("MultiSingleton", new object[]
				{
					base.GetType().FullName
				}));
			}
			BrowserInteropHelper.SetBrowserHosted(true);
			ApplicationProxyInternal._proxyInstance = this;
		}

		// Token: 0x06007889 RID: 30857 RVA: 0x0000C238 File Offset: 0x0000A438
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x0600788A RID: 30858 RVA: 0x00225582 File Offset: 0x00223782
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CreateRootBrowserWindow()
		{
			if (this._rbw.Value == null)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._CreateRootBrowserWindowCallback), null);
			}
		}

		// Token: 0x0600788B RID: 30859 RVA: 0x002255B0 File Offset: 0x002237B0
		internal bool FocusedElementWantsBackspace()
		{
			TextBoxBase textBoxBase = Keyboard.FocusedElement as TextBoxBase;
			return textBoxBase != null || Keyboard.FocusedElement is PasswordBox;
		}

		// Token: 0x0600788C RID: 30860 RVA: 0x002255DE File Offset: 0x002237DE
		[SecurityCritical]
		private object _CreateRootBrowserWindowCallback(object unused)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_RootBrowserWindowSetupStart);
			this.RootBrowserWindow = RootBrowserWindow.CreateAndInitialize();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_RootBrowserWindowSetupEnd);
			return null;
		}

		// Token: 0x0600788D RID: 30861 RVA: 0x0022560C File Offset: 0x0022380C
		[SecurityCritical]
		internal int Run(ApplicationProxyInternal.InitData initData)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AppProxyRunStart);
			if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				IntPtr iunknownForObject = Marshal.GetIUnknownForObject(initData.HostBrowser);
				try
				{
					initData.HostBrowser = (IHostBrowser)Marshal.GetObjectForIUnknown(iunknownForObject);
				}
				finally
				{
					Marshal.Release(iunknownForObject);
				}
			}
			BrowserInteropHelper.HostBrowser = initData.HostBrowser;
			MimeType value = initData.MimeType.Value;
			this._mimeType.Value = value;
			this.Uri = initData.ActivationUri.Value;
			WpfWebRequestHelper.DefaultUserAgent = initData.UserAgentString;
			BrowserInteropHelper.HostingFlags = initData.HostingFlags;
			this.Move(initData.WindowRect);
			this.Show(initData.ShowWindow);
			switch (value)
			{
			case MimeType.Document:
				throw new NotImplementedException();
			case MimeType.Application:
				goto IL_F6;
			case MimeType.Markup:
			{
				Invariant.Assert(AppDomain.CurrentDomain.FriendlyName == "XamlViewer");
				Application application = new Application();
				application.StartupUri = this.Uri;
				goto IL_F6;
			}
			}
			throw new InvalidOperationException();
			IL_F6:
			Application.Current.MimeType = value;
			this.ServiceProvider = initData.ServiceProvider;
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._RunDelegate), initData);
			int result = Application.Current.RunInternal(null);
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AppProxyRunEnd);
			return result;
		}

		// Token: 0x0600788E RID: 30862 RVA: 0x00225774 File Offset: 0x00223974
		[SecurityCritical]
		private object _RunDelegate(object args)
		{
			ApplicationProxyInternal.InitData initData = (ApplicationProxyInternal.InitData)args;
			Application application = Application.Current;
			if (application != null && !(application is XappLauncherApp))
			{
				string text = initData.Fragment;
				if (!string.IsNullOrEmpty(text) && application.StartupUri != null)
				{
					Uri uri = application.StartupUri;
					if (!application.StartupUri.IsAbsoluteUri)
					{
						uri = new Uri(BindUriHelper.BaseUri, application.StartupUri);
					}
					UriBuilder uriBuilder = new UriBuilder(uri);
					if (text.StartsWith("#", StringComparison.Ordinal))
					{
						text = text.Substring("#".Length);
					}
					uriBuilder.Fragment = text;
					application.StartupUri = uriBuilder.Uri;
				}
				this.CreateRootBrowserWindow();
			}
			if (initData.UcomLoadIStream != null && initData.HandleHistoryLoad)
			{
				this.LoadHistoryStream(DocObjHost.ExtractComStream(initData.UcomLoadIStream), true);
			}
			return null;
		}

		// Token: 0x0600788F RID: 30863 RVA: 0x00225846 File Offset: 0x00223A46
		internal void Show(bool show)
		{
			this._show = show;
			if (Application.Current != null && this.RootBrowserWindow != null)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._ShowDelegate), null);
			}
		}

		// Token: 0x06007890 RID: 30864 RVA: 0x00225880 File Offset: 0x00223A80
		private object _ShowDelegate(object ignore)
		{
			if (this.RootBrowserWindow == null || Application.IsShuttingDown)
			{
				return null;
			}
			if (this._show)
			{
				this._rbw.Value.Visibility = Visibility.Visible;
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this._FocusDelegate), null);
			}
			else
			{
				this._rbw.Value.Visibility = Visibility.Hidden;
			}
			return null;
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x002258EC File Offset: 0x00223AEC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object _FocusDelegate(object unused)
		{
			if (this._rbw.Value != null)
			{
				try
				{
					UnsafeNativeMethods.SetFocus(new HandleRef(this._rbw.Value, this._rbw.Value.CriticalHandle));
				}
				catch (Win32Exception)
				{
				}
			}
			return null;
		}

		// Token: 0x06007892 RID: 30866 RVA: 0x00225944 File Offset: 0x00223B44
		internal void Move(Rect windowRect)
		{
			if (Application.Current != null && this.RootBrowserWindow != null)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._MoveDelegate), windowRect);
				return;
			}
			this._windowRect = windowRect;
			this._rectset = true;
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x00225994 File Offset: 0x00223B94
		private object _MoveDelegate(object moveArgs)
		{
			if (this._rbw.Value != null && !Application.IsShuttingDown)
			{
				Rect rect = (Rect)moveArgs;
				this._rbw.Value.ResizeMove((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
			}
			return null;
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x002259F0 File Offset: 0x00223BF0
		[SecurityCritical]
		internal void PostShutdown()
		{
			this.Cleanup();
			ApplicationProxyInternal._proxyInstance = null;
			Application application = Application.Current;
			if (application != null)
			{
				XappLauncherApp xappLauncherApp = application as XappLauncherApp;
				if (xappLauncherApp != null)
				{
					xappLauncherApp.AbortActivation();
					return;
				}
				application.CriticalShutdown(0);
				application.Dispatcher.Invoke(DispatcherPriority.Normal, new DispatcherOperationCallback((object unused) => null), null);
			}
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x00225A58 File Offset: 0x00223C58
		internal void Activate(bool fActivate)
		{
			if (Application.Current != null && this.RootBrowserWindow != null)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._ActivateDelegate), fActivate);
			}
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x00225A90 File Offset: 0x00223C90
		private object _ActivateDelegate(object arg)
		{
			if (this.RootBrowserWindow != null)
			{
				bool flag = (bool)arg;
				this._rbw.Value.HandleActivate(flag);
				if (flag)
				{
					this._FocusDelegate(null);
				}
			}
			return null;
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x00225ACC File Offset: 0x00223CCC
		internal bool CanInvokeJournalEntry(int entryId)
		{
			return Application.Current != null && (bool)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
			{
				NavigationWindow navigationWindow = Application.Current.MainWindow as NavigationWindow;
				if (navigationWindow == null)
				{
					return false;
				}
				return navigationWindow.JournalNavigationScope.CanInvokeJournalEntry(entryId);
			}), null);
		}

		// Token: 0x06007898 RID: 30872 RVA: 0x00225B14 File Offset: 0x00223D14
		[SecurityCritical]
		private object _GetSaveHistoryBytesDelegate(object arg)
		{
			bool flag = (bool)arg;
			ApplicationProxyInternal.SaveHistoryReturnInfo saveHistoryReturnInfo = new ApplicationProxyInternal.SaveHistoryReturnInfo();
			if (this._serviceProvider == null)
			{
				return null;
			}
			if (Application.IsApplicationObjectShuttingDown)
			{
				return null;
			}
			Invariant.Assert(this._rbw.Value != null, "BrowserJournalingError: _rbw should not be null");
			Journal journal = this._rbw.Value.Journal;
			Invariant.Assert(journal != null, "BrowserJournalingError: Could not get internal journal for the window");
			JournalEntry journalEntry;
			if (flag)
			{
				NavigationService navigationService = this._rbw.Value.NavigationService;
				try
				{
					navigationService.RequestCustomContentStateOnAppShutdown();
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
				}
				journal.PruneKeepAliveEntries();
				journalEntry = navigationService.MakeJournalEntry(JournalReason.NewContentNavigation);
				if (journalEntry != null && !journalEntry.IsAlive())
				{
					if (journalEntry.JEGroupState.JournalDataStreams != null)
					{
						journalEntry.JEGroupState.JournalDataStreams.PrepareForSerialization();
					}
					journal.UpdateCurrentEntry(journalEntry);
				}
				else
				{
					journalEntry = journal.GetGoBackEntry();
				}
			}
			else
			{
				journalEntry = journal.CurrentEntry;
			}
			if (journalEntry != null)
			{
				saveHistoryReturnInfo.title = journalEntry.Name;
				saveHistoryReturnInfo.entryId = journalEntry.Id;
			}
			else
			{
				saveHistoryReturnInfo.title = this._rbw.Value.Title;
			}
			saveHistoryReturnInfo.uri = BindUriHelper.UriToString(this.Uri);
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			if (flag)
			{
				ApplicationProxyInternal.BrowserJournal browserJournal = new ApplicationProxyInternal.BrowserJournal(journal, BindUriHelper.BaseUri);
				new SecurityPermission(SecurityPermissionFlag.SerializationFormatter).Assert();
				try
				{
					memoryStream.WriteByte(2);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					binaryFormatter.Serialize(memoryStream, browserJournal);
					goto IL_1A6;
				}
				catch (Exception ex2)
				{
					if (CriticalExceptions.IsCriticalException(ex2))
					{
						throw;
					}
					Invariant.Assert(false, "Failed to serialize the navigation journal: " + ex2);
					goto IL_1A6;
				}
				finally
				{
					CodeAccessPermission.RevertAll();
				}
			}
			memoryStream.WriteByte(1);
			ApplicationProxyInternal.WriteInt32(memoryStream, saveHistoryReturnInfo.entryId);
			IL_1A6:
			saveHistoryReturnInfo.saveByteArray = memoryStream.ToArray();
			((IDisposable)memoryStream).Dispose();
			return saveHistoryReturnInfo;
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x00225D04 File Offset: 0x00223F04
		[SecurityCritical]
		internal byte[] GetSaveHistoryBytes(bool persistEntireJournal, out int journalEntryId, out string uriString, out string titleString)
		{
			ApplicationProxyInternal.SaveHistoryReturnInfo saveHistoryReturnInfo = null;
			if (Application.Current != null)
			{
				saveHistoryReturnInfo = (ApplicationProxyInternal.SaveHistoryReturnInfo)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._GetSaveHistoryBytesDelegate), persistEntireJournal);
			}
			if (saveHistoryReturnInfo != null)
			{
				journalEntryId = saveHistoryReturnInfo.entryId;
				uriString = saveHistoryReturnInfo.uri;
				titleString = saveHistoryReturnInfo.title;
				return saveHistoryReturnInfo.saveByteArray;
			}
			journalEntryId = 0;
			uriString = null;
			titleString = null;
			return null;
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x00225D74 File Offset: 0x00223F74
		[SecurityCritical]
		internal void LoadHistoryStream(MemoryStream loadStream, bool firstLoadFromHistory)
		{
			if (Application.Current == null)
			{
				return;
			}
			ApplicationProxyInternal.LoadHistoryStreamInfo loadHistoryStreamInfo = new ApplicationProxyInternal.LoadHistoryStreamInfo();
			loadHistoryStreamInfo.loadStream = loadStream;
			loadHistoryStreamInfo.firstLoadFromHistory = firstLoadFromHistory;
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this._LoadHistoryStreamDelegate), loadHistoryStreamInfo);
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x00225DBC File Offset: 0x00223FBC
		[SecurityCritical]
		private object _LoadHistoryStreamDelegate(object arg)
		{
			ApplicationProxyInternal.LoadHistoryStreamInfo loadHistoryStreamInfo = (ApplicationProxyInternal.LoadHistoryStreamInfo)arg;
			if (this.IsShutdown())
			{
				return null;
			}
			loadHistoryStreamInfo.loadStream.Seek(0L, SeekOrigin.Begin);
			object obj = this.DeserializeJournaledObject(loadHistoryStreamInfo.loadStream);
			if (loadHistoryStreamInfo.firstLoadFromHistory)
			{
				if (!(obj is ApplicationProxyInternal.BrowserJournal))
				{
					return null;
				}
				ApplicationProxyInternal.BrowserJournal browserJournal = (ApplicationProxyInternal.BrowserJournal)obj;
				Journal journal = browserJournal.Journal;
				JournalEntry journalEntry = journal.CurrentEntry;
				if (journalEntry == null)
				{
					journalEntry = journal.GetGoBackEntry();
				}
				NavigationService navigationService = this._rbw.Value.NavigationService;
				this._rbw.Value.SetJournalForBrowserInterop(journal);
				if (BindUriHelper.BaseUri == null)
				{
					BindUriHelper.BaseUri = browserJournal.BaseUri;
				}
				if (journalEntry != null)
				{
					Application.Current.Startup += this.OnStartup;
					this._rbw.Value.JournalNavigationScope.NavigateToEntry(journalEntry);
				}
			}
			else
			{
				if (!(obj is int))
				{
					return null;
				}
				Journal journal = this._rbw.Value.Journal;
				int index = journal.FindIndexForEntryWithId((int)obj);
				if (!this._rbw.Value.JournalNavigationScope.NavigateToEntry(index))
				{
					throw new OperationCanceledException();
				}
			}
			return null;
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x00225EEC File Offset: 0x002240EC
		[SecurityCritical]
		private object DeserializeJournaledObject(MemoryStream inputStream)
		{
			object result = null;
			int num = inputStream.ReadByte();
			if (num >= 0)
			{
				byte b = (byte)num;
				if (b != 1)
				{
					if (b == 2)
					{
						try
						{
							new SecurityPermission(SecurityPermissionFlag.SerializationFormatter).Demand();
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							return binaryFormatter.Deserialize(inputStream);
						}
						catch (SecurityException)
						{
							return null;
						}
					}
					throw new FormatException();
				}
				result = ApplicationProxyInternal.ReadInt32(inputStream);
			}
			return result;
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x00225F60 File Offset: 0x00224160
		internal bool IsAppLoaded()
		{
			return Application.Current != null;
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x000C265B File Offset: 0x000C085B
		internal bool IsShutdown()
		{
			return Application.IsShuttingDown;
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x00225F6C File Offset: 0x0022416C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Cleanup()
		{
			if (Application.Current != null)
			{
				IBrowserCallbackServices browserCallbackServices = Application.Current.BrowserCallbackServices;
				if (browserCallbackServices != null)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.ReleaseBrowserCallback), browserCallbackServices);
				}
			}
			this.ServiceProvider = null;
			this.ClearRootBrowserWindow();
			if (this._storageRoot != null && this._storageRoot.Value != null)
			{
				this._storageRoot.Value.Close();
			}
			if (this._document.Value is PackageDocument)
			{
				PreloadedPackages.RemovePackage(PackUriHelper.GetPackageUri(PackUriHelper.Create(this.Uri)));
				((PackageDocument)this._document.Value).Dispose();
				this._document.Value = null;
			}
			if (this._mimeType.Value == MimeType.Document)
			{
				DocumentManager.CleanUp();
			}
			if (this._packageStream.Value != null)
			{
				this._packageStream.Value.Close();
			}
			if (this._unmanagedStream.Value != null)
			{
				Marshal.ReleaseComObject(this._unmanagedStream.Value);
				this._unmanagedStream = new SecurityCriticalData<object>(null);
			}
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x00226081 File Offset: 0x00224281
		[SecurityCritical]
		private object ReleaseBrowserCallback(object browserCallback)
		{
			Marshal.ReleaseComObject(browserCallback);
			BrowserInteropHelper.ReleaseBrowserInterfaces();
			return null;
		}

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x060078A1 RID: 30881 RVA: 0x00226090 File Offset: 0x00224290
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal RootBrowserWindowProxy RootBrowserWindowProxy
		{
			get
			{
				if (this._rbwProxy.Value == null)
				{
					this.CreateRootBrowserWindow();
				}
				return this._rbwProxy.Value;
			}
		}

		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x060078A2 RID: 30882 RVA: 0x002260B0 File Offset: 0x002242B0
		// (set) Token: 0x060078A3 RID: 30883 RVA: 0x002260C0 File Offset: 0x002242C0
		internal RootBrowserWindow RootBrowserWindow
		{
			get
			{
				return this._rbw.Value;
			}
			[SecurityCritical]
			private set
			{
				this._rbw.Value = value;
				if (value == null)
				{
					this._rbwProxy.Value = null;
					return;
				}
				this._rbwProxy.Value = new RootBrowserWindowProxy(value);
				if (this._rectset)
				{
					this.Move(this._windowRect);
					this._rectset = false;
				}
				this.Show(this._show);
			}
		}

		// Token: 0x17001C8D RID: 7309
		// (get) Token: 0x060078A4 RID: 30884 RVA: 0x00226121 File Offset: 0x00224321
		internal bool RootBrowserWindowCreated
		{
			get
			{
				return this._rbw.Value != null;
			}
		}

		// Token: 0x17001C8E RID: 7310
		// (get) Token: 0x060078A5 RID: 30885 RVA: 0x00226131 File Offset: 0x00224331
		internal OleCmdHelper OleCmdHelper
		{
			get
			{
				if (Application.Current == null)
				{
					return null;
				}
				return (OleCmdHelper)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					if (Application.IsApplicationObjectShuttingDown)
					{
						return null;
					}
					if (this._oleCmdHelper == null)
					{
						this._oleCmdHelper = new OleCmdHelper();
					}
					return this._oleCmdHelper;
				}), null);
			}
		}

		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x060078A6 RID: 30886 RVA: 0x0022615F File Offset: 0x0022435F
		internal static ApplicationProxyInternal Current
		{
			[SecurityCritical]
			get
			{
				return ApplicationProxyInternal._proxyInstance;
			}
		}

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x060078A7 RID: 30887 RVA: 0x00226166 File Offset: 0x00224366
		// (set) Token: 0x060078A8 RID: 30888 RVA: 0x00226173 File Offset: 0x00224373
		internal Uri Uri
		{
			get
			{
				return this._criticalUri.Value;
			}
			[SecurityCritical]
			private set
			{
				this._criticalUri.Value = value;
				if (SiteOfOriginContainer.BrowserSource == null)
				{
					SiteOfOriginContainer.BrowserSource = value;
				}
			}
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x00226194 File Offset: 0x00224394
		[SecurityCritical]
		internal void SetDebugSecurityZoneURL(Uri debugSecurityZoneURL)
		{
			SiteOfOriginContainer.BrowserSource = debugSecurityZoneURL;
		}

		// Token: 0x17001C91 RID: 7313
		// (set) Token: 0x060078AA RID: 30890 RVA: 0x0022619C File Offset: 0x0022439C
		internal object StreamContainer
		{
			[SecurityCritical]
			set
			{
				this._unmanagedStream = new SecurityCriticalData<object>(Marshal.GetObjectForIUnknown((IntPtr)value));
			}
		}

		// Token: 0x060078AB RID: 30891 RVA: 0x002261B4 File Offset: 0x002243B4
		private void OnStartup(object sender, StartupEventArgs e)
		{
			e.PerformDefaultAction = false;
			Application.Current.Startup -= this.OnStartup;
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x002261D3 File Offset: 0x002243D3
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ClearRootBrowserWindow()
		{
			this.RootBrowserWindow = null;
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x002261DC File Offset: 0x002243DC
		private static void WriteInt32(Stream stream, int value)
		{
			stream.WriteByte((byte)(((long)value & (long)((ulong)-16777216)) >> 24));
			stream.WriteByte((byte)((value & 16711680) >> 16));
			stream.WriteByte((byte)((value & 65280) >> 8));
			stream.WriteByte((byte)(value & 255));
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x0022622C File Offset: 0x0022442C
		private static int ReadInt32(Stream stream)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				int num2 = stream.ReadByte();
				if (num2 < 0)
				{
					throw new EndOfStreamException();
				}
				num = (num << 8 | num2);
			}
			return num;
		}

		// Token: 0x17001C92 RID: 7314
		// (set) Token: 0x060078AF RID: 30895 RVA: 0x0022625F File Offset: 0x0022445F
		private IServiceProvider ServiceProvider
		{
			set
			{
				this._serviceProvider = value;
				if (Application.Current != null)
				{
					Application.Current.ServiceProvider = value;
				}
			}
		}

		// Token: 0x0400391B RID: 14619
		private SecurityCriticalDataForSet<RootBrowserWindow> _rbw;

		// Token: 0x0400391C RID: 14620
		private SecurityCriticalDataForSet<RootBrowserWindowProxy> _rbwProxy;

		// Token: 0x0400391D RID: 14621
		private bool _show;

		// Token: 0x0400391E RID: 14622
		private OleCmdHelper _oleCmdHelper;

		// Token: 0x0400391F RID: 14623
		private Rect _windowRect;

		// Token: 0x04003920 RID: 14624
		private bool _rectset;

		// Token: 0x04003921 RID: 14625
		private SecurityCriticalDataForSet<Uri> _criticalUri;

		// Token: 0x04003922 RID: 14626
		private SecurityCriticalDataClass<StorageRoot> _storageRoot = new SecurityCriticalDataClass<StorageRoot>(null);

		// Token: 0x04003923 RID: 14627
		private SecurityCriticalDataForSet<MimeType> _mimeType;

		// Token: 0x04003924 RID: 14628
		private IServiceProvider _serviceProvider;

		// Token: 0x04003925 RID: 14629
		[SecurityCritical]
		private static ApplicationProxyInternal _proxyInstance;

		// Token: 0x04003926 RID: 14630
		private const string FRAGMENT_MARKER = "#";

		// Token: 0x04003927 RID: 14631
		private const byte JournalIdHeader = 1;

		// Token: 0x04003928 RID: 14632
		private const byte BrowserJournalHeader = 2;

		// Token: 0x04003929 RID: 14633
		private SecurityCriticalData<object> _unmanagedStream = new SecurityCriticalData<object>(null);

		// Token: 0x0400392A RID: 14634
		private SecurityCriticalData<Stream> _packageStream = new SecurityCriticalData<Stream>(null);

		// Token: 0x0400392B RID: 14635
		private SecurityCriticalDataForSet<object> _document;

		// Token: 0x02000B6F RID: 2927
		[Serializable]
		internal class InitData
		{
			// Token: 0x04004B4C RID: 19276
			internal IServiceProvider ServiceProvider;

			// Token: 0x04004B4D RID: 19277
			[SecurityCritical]
			internal IHostBrowser HostBrowser;

			// Token: 0x04004B4E RID: 19278
			internal SecurityCriticalDataForSet<MimeType> MimeType;

			// Token: 0x04004B4F RID: 19279
			internal SecurityCriticalDataForSet<Uri> ActivationUri;

			// Token: 0x04004B50 RID: 19280
			internal string Fragment;

			// Token: 0x04004B51 RID: 19281
			internal object UcomLoadIStream;

			// Token: 0x04004B52 RID: 19282
			internal bool HandleHistoryLoad;

			// Token: 0x04004B53 RID: 19283
			internal string UserAgentString;

			// Token: 0x04004B54 RID: 19284
			internal HostingFlags HostingFlags;

			// Token: 0x04004B55 RID: 19285
			internal Rect WindowRect;

			// Token: 0x04004B56 RID: 19286
			internal bool ShowWindow;
		}

		// Token: 0x02000B70 RID: 2928
		private class SaveHistoryReturnInfo
		{
			// Token: 0x04004B57 RID: 19287
			internal string uri;

			// Token: 0x04004B58 RID: 19288
			internal string title;

			// Token: 0x04004B59 RID: 19289
			internal int entryId;

			// Token: 0x04004B5A RID: 19290
			internal byte[] saveByteArray;
		}

		// Token: 0x02000B71 RID: 2929
		private class LoadHistoryStreamInfo
		{
			// Token: 0x04004B5B RID: 19291
			internal MemoryStream loadStream;

			// Token: 0x04004B5C RID: 19292
			internal bool firstLoadFromHistory;
		}

		// Token: 0x02000B72 RID: 2930
		[Serializable]
		private struct BrowserJournal
		{
			// Token: 0x06008E2B RID: 36395 RVA: 0x0025B85E File Offset: 0x00259A5E
			internal BrowserJournal(Journal journal, Uri baseUri)
			{
				this._journal = journal;
				this._baseUri = baseUri;
			}

			// Token: 0x17001F9E RID: 8094
			// (get) Token: 0x06008E2C RID: 36396 RVA: 0x0025B86E File Offset: 0x00259A6E
			internal Journal Journal
			{
				get
				{
					return this._journal;
				}
			}

			// Token: 0x17001F9F RID: 8095
			// (get) Token: 0x06008E2D RID: 36397 RVA: 0x0025B876 File Offset: 0x00259A76
			internal Uri BaseUri
			{
				get
				{
					return this._baseUri;
				}
			}

			// Token: 0x04004B5D RID: 19293
			private Journal _journal;

			// Token: 0x04004B5E RID: 19294
			private Uri _baseUri;
		}
	}
}
