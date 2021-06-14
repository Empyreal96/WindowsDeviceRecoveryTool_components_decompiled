using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.IO.Packaging;
using MS.Internal.Progressivity;
using MS.Utility;
using MS.Win32;

namespace System.Windows.Interop
{
	/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
	// Token: 0x020005BB RID: 1467
	public sealed class DocObjHost : MarshalByRefObject, IServiceProvider, IHostService, IBrowserHostServices, IByteRangeDownloaderService
	{
		/// <summary> This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
		// Token: 0x060061BA RID: 25018 RVA: 0x001B6588 File Offset: 0x001B4788
		[SecurityCritical]
		public DocObjHost()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_DocObjHostCreated);
			SecurityHelper.DemandUnmanagedCode();
			this._mainThread = Thread.CurrentThread;
			this._initData.Value.ServiceProvider = this;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
		/// <returns>A new <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> object. </returns>
		// Token: 0x060061BB RID: 25019 RVA: 0x001B65DC File Offset: 0x001B47DC
		[SecurityCritical]
		public override object InitializeLifetimeService()
		{
			ILease lease = (ILease)base.InitializeLifetimeService();
			new SecurityPermission(PermissionState.Unrestricted).Assert();
			try
			{
				lease.InitialLeaseTime = TimeSpan.Zero;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return lease;
		}

		/// <summary>For a description of this member, see <see cref="M:System.IServiceProvider.GetService(System.Type)" />.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType" />.-or- 
		///     <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x060061BC RID: 25020 RVA: 0x001B6624 File Offset: 0x001B4824
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(IHostService))
			{
				return this;
			}
			if (serviceType == typeof(IBrowserCallbackServices))
			{
				return this._browserCallbackServices;
			}
			return null;
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x060061BD RID: 25021 RVA: 0x001B6654 File Offset: 0x001B4854
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		RootBrowserWindowProxy IHostService.RootBrowserWindowProxy
		{
			get
			{
				if (this._appProxyInternal != null)
				{
					return this._appProxyInternal.RootBrowserWindowProxy;
				}
				return null;
			}
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x060061BE RID: 25022 RVA: 0x001B666B File Offset: 0x001B486B
		IntPtr IHostService.HostWindowHandle
		{
			[SecurityCritical]
			get
			{
				return this._parent;
			}
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x001B6674 File Offset: 0x001B4874
		[SecurityCritical]
		int IBrowserHostServices.Run(string path, string fragment, MimeType mime, string debugSecurityZoneURL, string applicationId, object streamContainer, object ucomLoadIStream, HostingFlags hostingFlags, INativeProgressPage nativeProgressPage, string progressAssemblyName, string progressClassName, string errorAssemblyName, string errorClassName, IHostBrowser hostBrowser)
		{
			Invariant.Assert(!string.IsNullOrEmpty(path), "path string should not be null or empty when Run method is called.");
			Invariant.Assert(mime > MimeType.Unknown, "Unknown mime type");
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_IBHSRunStart, "\"" + path + "\"", "\"" + applicationId + "\"");
			int num = 0;
			try
			{
				ApplicationProxyInternal.InitData value = this._initData.Value;
				value.HostBrowser = hostBrowser;
				value.Fragment = fragment;
				value.UcomLoadIStream = ucomLoadIStream;
				value.HandleHistoryLoad = true;
				value.MimeType.Value = mime;
				string userAgentString = null;
				HRESULT hrLeft = hostBrowser.GetUserAgentString(out userAgentString);
				if (hrLeft == HRESULT.E_OUTOFMEMORY && (hostingFlags & HostingFlags.hfHostedInIEorWebOC) != (HostingFlags)0)
				{
					userAgentString = UnsafeNativeMethods.ObtainUserAgentString();
					hrLeft = HRESULT.S_OK;
				}
				hrLeft.ThrowIfFailed();
				value.UserAgentString = userAgentString;
				value.HostingFlags = hostingFlags;
				Uri uri = new UriBuilder(path).Uri;
				value.ActivationUri.Value = uri;
				PresentationAppDomainManager.ActivationUri = uri;
				BrowserInteropHelper.SetBrowserHosted(true);
				if ((hostingFlags & HostingFlags.hfInDebugMode) != (HostingFlags)0)
				{
					this._browserCallbackServices.ChangeDownloadState(false);
					this._browserCallbackServices.UpdateProgress(-1L, 0L);
					this.EnableErrorPage();
					this._appProxyInternal = new ApplicationLauncherXappDebug(path, debugSecurityZoneURL).Initialize();
				}
				else
				{
					switch (mime)
					{
					case MimeType.Document:
						this._appProxyInternal = this.CreateAppDomainForXpsDocument();
						if (this._appProxyInternal == null)
						{
							num = -1;
						}
						else if (streamContainer != null)
						{
							IntPtr iunknownForObject = Marshal.GetIUnknownForObject(streamContainer);
							this._appProxyInternal.StreamContainer = iunknownForObject;
							Marshal.Release(iunknownForObject);
						}
						this._initData.Value = null;
						break;
					case MimeType.Application:
					{
						XappLauncherApp xappLauncherApp = new XappLauncherApp(uri, applicationId, this._browserCallbackServices, new DocObjHost.ApplicationRunnerCallback(this.RunApplication), nativeProgressPage, progressAssemblyName, progressClassName, errorAssemblyName, errorClassName);
						value.HandleHistoryLoad = false;
						this._appProxyInternal = new ApplicationProxyInternal();
						break;
					}
					case MimeType.Markup:
						this._appProxyInternal = this.CreateAppDomainForLooseXaml(uri);
						this._initData.Value = null;
						break;
					default:
						num = -1;
						break;
					}
				}
				if (num != -1)
				{
					if (mime == MimeType.Document || mime == MimeType.Markup)
					{
						this.EnableErrorPage();
					}
					if (this.IsAffectedByCtfIssue())
					{
						num = -1;
						this._browserCallbackServices.ProcessUnhandledException(string.Format(CultureInfo.CurrentCulture, SR.Get("AffectedByMsCtfIssue"), new object[]
						{
							"http://support.microsoft.com/kb/954494"
						}));
					}
					else
					{
						num = this._appProxyInternal.Run(value);
					}
				}
			}
			catch (Exception ex)
			{
				num = -1;
				this._browserCallbackServices.ProcessUnhandledException(ex.ToString());
				throw;
			}
			catch
			{
				num = -1;
				this._browserCallbackServices.ProcessUnhandledException(SR.Get("NonClsActivationException"));
				throw;
			}
			finally
			{
				this.Cleanup(num);
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_IBHSRunEnd, num);
			return num;
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x001B696C File Offset: 0x001B4B6C
		private bool IsAffectedByCtfIssue()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Version.Major == 5 && osversion.Version.Minor == 1 && (TextServicesLoader.ServicesInstalled || InputLanguageManager.IsMultipleKeyboardLayout))
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Lsa"))
				{
					if ((int)registryKey.GetValue("NoDefaultAdminOwner", 1) == 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x001B69F4 File Offset: 0x001B4BF4
		[SecurityCritical]
		internal void RunApplication(DocObjHost.ApplicationRunner runner)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_DocObjHostRunApplicationStart);
			PresentationAppDomainManager.SaveAppDomain = true;
			this.EnableErrorPage();
			if (runner())
			{
				Invariant.Assert(PresentationAppDomainManager.NewAppDomain != null, "Failed to start the application in a new AppDomain");
				Invariant.Assert(ApplicationProxyInternal.Current != null, "Unexpected reentrant PostShutdown?");
				PresentationAppDomainManager presentationAppDomainManager = PresentationAppDomainManager.NewAppDomain.DomainManager as PresentationAppDomainManager;
				Invariant.Assert(ApplicationProxyInternal.Current == this._appProxyInternal, "AppProxyInternal has shut down unexpectedly.");
				this._appProxyInternal = presentationAppDomainManager.CreateApplicationProxyInternal();
				PresentationAppDomainManager.SaveAppDomain = false;
				ApplicationProxyInternal.InitData value = this._initData.Value;
				value.HandleHistoryLoad = true;
				this._appProxyInternal.Run(value);
				this._initData.Value = null;
			}
			else
			{
				PresentationAppDomainManager.SaveAppDomain = false;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_DocObjHostRunApplicationEnd);
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x001B6AC9 File Offset: 0x001B4CC9
		[SecurityCritical]
		void IBrowserHostServices.SetBrowserCallback(object browserCallbackServices)
		{
			Invariant.Assert(browserCallbackServices != null, "Browser interop interface passed in should not be null");
			this._browserCallbackServices = (IBrowserCallbackServices)browserCallbackServices;
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x001B6AE5 File Offset: 0x001B4CE5
		[SecurityCritical]
		void IBrowserHostServices.SetParent(IntPtr hParent)
		{
			this._parent = hParent;
			PresentationHostSecurityManager.ElevationPromptOwnerWindow = hParent;
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x001B6AF4 File Offset: 0x001B4CF4
		void IBrowserHostServices.Show(bool show)
		{
			if (this._initData.Value != null)
			{
				this._initData.Value.ShowWindow = show;
			}
			if (this._appProxyInternal != null)
			{
				this._appProxyInternal.Show(show);
			}
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x001B6B28 File Offset: 0x001B4D28
		bool IBrowserHostServices.IsAppLoaded()
		{
			return this._appProxyInternal != null && this._appProxyInternal.IsAppLoaded();
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x001B6B3F File Offset: 0x001B4D3F
		int IBrowserHostServices.GetApplicationExitCode()
		{
			return Environment.ExitCode;
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x001B6B48 File Offset: 0x001B4D48
		bool IBrowserHostServices.CanInvokeJournalEntry(int entryId)
		{
			return ((IBrowserHostServices)this).IsAppLoaded() && this._appProxyInternal.CanInvokeJournalEntry(entryId);
		}

		// Token: 0x060061C8 RID: 25032 RVA: 0x001B6B71 File Offset: 0x001B4D71
		[SecurityCritical]
		void IBrowserHostServices.SaveHistory(object comIStream, bool persistEntireJournal, out int entryIndex, out string uri, out string title)
		{
			if (this._appProxyInternal != null)
			{
				this.SaveHistoryHelper(comIStream, persistEntireJournal, out entryIndex, out uri, out title);
				return;
			}
			entryIndex = -1;
			uri = null;
			title = null;
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x001B6B94 File Offset: 0x001B4D94
		[SecurityCritical]
		void IBrowserHostServices.LoadHistory(object ucomIStream)
		{
			if (this._appProxyInternal != null)
			{
				this.LoadHistoryHelper(ucomIStream, false);
			}
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x001B6BA8 File Offset: 0x001B4DA8
		[SecurityCritical]
		int IBrowserHostServices.QueryStatus(Guid guidCmdGroup, uint command, out uint flags)
		{
			flags = 0U;
			if (this._appProxyInternal != null)
			{
				OleCmdHelper oleCmdHelper = this._appProxyInternal.OleCmdHelper;
				if (oleCmdHelper != null)
				{
					oleCmdHelper.QueryStatus(guidCmdGroup, command, ref flags);
					return 0;
				}
			}
			return -2147221244;
		}

		// Token: 0x060061CB RID: 25035 RVA: 0x001B6BE0 File Offset: 0x001B4DE0
		[SecurityCritical]
		int IBrowserHostServices.ExecCommand(Guid guidCommandGroup, uint command, object arg)
		{
			XappLauncherApp xappLauncherApp = Application.Current as XappLauncherApp;
			if (xappLauncherApp != null && guidCommandGroup == Guid.Empty)
			{
				if (command == 22U)
				{
					xappLauncherApp.HandleRefresh();
					return 0;
				}
				if (command == 23U)
				{
					xappLauncherApp.UserStop();
					return 0;
				}
			}
			if (this._appProxyInternal != null)
			{
				OleCmdHelper oleCmdHelper = this._appProxyInternal.OleCmdHelper;
				if (oleCmdHelper != null)
				{
					oleCmdHelper.ExecCommand(guidCommandGroup, command, arg);
					return 0;
				}
			}
			return -2147221244;
		}

		// Token: 0x060061CC RID: 25036 RVA: 0x001B6C50 File Offset: 0x001B4E50
		[SecurityCritical]
		void IBrowserHostServices.Move(int x, int y, int width, int height)
		{
			Rect windowRect = new Rect((double)x, (double)y, (double)width, (double)height);
			if (this._initData.Value != null)
			{
				this._initData.Value.WindowRect = windowRect;
			}
			if (this._appProxyInternal != null)
			{
				this._appProxyInternal.Move(windowRect);
			}
		}

		// Token: 0x060061CD RID: 25037 RVA: 0x001B6C9F File Offset: 0x001B4E9F
		[SecurityCritical]
		void IBrowserHostServices.PostShutdown()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_PostShutdown);
			if (this._appProxyInternal != null)
			{
				this._appProxyInternal.PostShutdown();
			}
			BrowserInteropHelper.ReleaseBrowserInterfaces();
		}

		// Token: 0x060061CE RID: 25038 RVA: 0x001B6CC8 File Offset: 0x001B4EC8
		[SecurityCritical]
		void IBrowserHostServices.Activate(bool fActivate)
		{
			if (this._appProxyInternal != null)
			{
				this._appProxyInternal.Activate(fActivate);
			}
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x001B6CDE File Offset: 0x001B4EDE
		void IBrowserHostServices.TabInto(bool forward)
		{
			if (!this._appProxyInternal.RootBrowserWindowCreated)
			{
				return;
			}
			this._appProxyInternal.RootBrowserWindowProxy.TabInto(forward);
		}

		// Token: 0x060061D0 RID: 25040 RVA: 0x001B6CFF File Offset: 0x001B4EFF
		bool IBrowserHostServices.FocusedElementWantsBackspace()
		{
			return this._appProxyInternal != null && this._appProxyInternal.FocusedElementWantsBackspace();
		}

		// Token: 0x060061D1 RID: 25041 RVA: 0x001B6D18 File Offset: 0x001B4F18
		[SecurityCritical]
		void IByteRangeDownloaderService.InitializeByteRangeDownloader(string url, string tempFile, SafeWaitHandle eventHandle)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (tempFile == null)
			{
				throw new ArgumentNullException("tempFile");
			}
			if (eventHandle == null)
			{
				throw new ArgumentNullException("eventHandle");
			}
			if (eventHandle.IsInvalid || eventHandle.IsClosed)
			{
				throw new ArgumentException(SR.Get("InvalidEventHandle"), "eventHandle");
			}
			Uri requestedUri = new Uri(url, UriKind.Absolute);
			if (tempFile.Length <= 0)
			{
				throw new ArgumentException(SR.Get("InvalidTempFileName"), "tempFile");
			}
			ByteRangeDownloader downloader = new ByteRangeDownloader(requestedUri, tempFile, eventHandle);
			this._downloader = downloader;
		}

		// Token: 0x060061D2 RID: 25042 RVA: 0x001B6DAC File Offset: 0x001B4FAC
		void IByteRangeDownloaderService.RequestDownloadByteRanges(int[] byteRanges, int size)
		{
			if (byteRanges == null)
			{
				throw new ArgumentNullException("byteRanges");
			}
			if (byteRanges.Length == 0 || byteRanges.Length % 2 != 0)
			{
				throw new ArgumentException(SR.Get("InvalidByteRanges", new object[]
				{
					"byteRanges"
				}));
			}
			if (this._downloader == null)
			{
				throw new InvalidOperationException(SR.Get("ByteRangeDownloaderNotInitialized"));
			}
			((ByteRangeDownloader)this._downloader).RequestByteRanges(ByteRangeDownloader.ConvertByteRanges(byteRanges));
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x001B6E20 File Offset: 0x001B5020
		void IByteRangeDownloaderService.GetDownloadedByteRanges(out int[] byteRanges, out int size)
		{
			size = 0;
			byteRanges = null;
			if (this._downloader == null)
			{
				throw new InvalidOperationException(SR.Get("ByteRangeDownloaderNotInitialized"));
			}
			int[,] downloadedByteRanges = ((ByteRangeDownloader)this._downloader).GetDownloadedByteRanges();
			byteRanges = ByteRangeDownloader.ConvertByteRanges(downloadedByteRanges);
			size = byteRanges.Length;
		}

		// Token: 0x060061D4 RID: 25044 RVA: 0x001B6E6A File Offset: 0x001B506A
		void IByteRangeDownloaderService.ReleaseByteRangeDownloader()
		{
			if (this._downloader == null)
			{
				throw new InvalidOperationException(SR.Get("ByteRangeDownloaderNotInitialized"));
			}
			((IDisposable)this._downloader).Dispose();
			this._downloader = null;
		}

		// Token: 0x060061D5 RID: 25045 RVA: 0x001B6E9C File Offset: 0x001B509C
		[SecurityCritical]
		private ApplicationProxyInternal CreateAppDomainForXpsDocument()
		{
			PermissionSet permissionSet = new PermissionSet(null);
			permissionSet.AddPermission(new FileDialogPermission(FileDialogPermissionAccess.Open));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			permissionSet.AddPermission(new UIPermission(UIPermissionWindow.SafeTopLevelWindows));
			permissionSet.AddPermission(new UIPermission(UIPermissionClipboard.OwnClipboard));
			permissionSet.AddPermission(new MediaPermission(MediaPermissionImage.SafeImage));
			permissionSet.AddPermission(new IsolatedStorageFilePermission(PermissionState.Unrestricted)
			{
				UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser,
				UserQuota = (long)this.GetXpsViewerIsolatedStorageUserQuota()
			});
			return this.CreateAppDomainAndAppProxy("WCP_Hosted_Application", this.GetXPSViewerPath(), permissionSet);
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x001B6F28 File Offset: 0x001B5128
		[SecurityCritical]
		private ApplicationProxyInternal CreateAppDomainForLooseXaml(Uri uri)
		{
			PermissionSet permissionSet = new PermissionSet(null);
			permissionSet.AddPermission(new FileDialogPermission(FileDialogPermissionAccess.Open));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			permissionSet.AddPermission(new UIPermission(UIPermissionWindow.SafeTopLevelWindows));
			permissionSet.AddPermission(new UIPermission(UIPermissionClipboard.OwnClipboard));
			permissionSet.AddPermission(SystemDrawingHelper.NewSafePrintingPermission());
			permissionSet.AddPermission(new MediaPermission(MediaPermissionAudio.SafeAudio, MediaPermissionVideo.SafeVideo, MediaPermissionImage.SafeImage));
			permissionSet.AddPermission(new WebBrowserPermission(WebBrowserPermissionLevel.Safe));
			permissionSet = PresentationHostSecurityManager.AddPermissionForUri(permissionSet, uri);
			string text = "XamlViewer";
			string moduleFileName = UnsafeNativeMethods.GetModuleFileName(new HandleRef(null, UnsafeNativeMethods.GetModuleHandle("PresentationHost_v0400.dll")));
			string appBasePath = Path.GetDirectoryName(moduleFileName) + "\\" + text;
			return this.CreateAppDomainAndAppProxy(text, appBasePath, permissionSet);
		}

		// Token: 0x060061D7 RID: 25047 RVA: 0x001B6FD8 File Offset: 0x001B51D8
		[SecurityCritical]
		private ApplicationProxyInternal CreateAppDomainAndAppProxy(string domainName, string appBasePath, PermissionSet grantSet)
		{
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			Invariant.Assert(!string.IsNullOrEmpty(appBasePath));
			appDomainSetup.ApplicationBase = appBasePath;
			AppDomain appDomain = AppDomain.CreateDomain(domainName, null, appDomainSetup, grantSet, null);
			return ((PresentationAppDomainManager)appDomain.DomainManager).CreateApplicationProxyInternal();
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x001B701B File Offset: 0x001B521B
		[SecurityCritical]
		private void Cleanup(int exitCode)
		{
			if (exitCode == 0)
			{
				return;
			}
			if (this._appProxyInternal != null)
			{
				this._appProxyInternal.Cleanup();
				return;
			}
			if (this._browserCallbackServices != null)
			{
				Marshal.ReleaseComObject(this._browserCallbackServices);
				this._browserCallbackServices = null;
			}
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x001B7050 File Offset: 0x001B5250
		[SecurityCritical]
		private void SaveHistoryHelper(object comIStream, bool persistEntireJournal, out int entryIndex, out string uri, out string title)
		{
			string text;
			title = (text = null);
			uri = text;
			entryIndex = -1;
			if (this._appProxyInternal == null || !RemotingServices.IsTransparentProxy(this._appProxyInternal))
			{
				return;
			}
			DocObjHost.SecuritySuppressedIStream securitySuppressedIStream = comIStream as DocObjHost.SecuritySuppressedIStream;
			if (securitySuppressedIStream == null)
			{
				return;
			}
			byte[] saveHistoryBytes = this._appProxyInternal.GetSaveHistoryBytes(persistEntireJournal, out entryIndex, out uri, out title);
			if (saveHistoryBytes == null)
			{
				return;
			}
			int num = saveHistoryBytes.Length;
			int num2 = 0;
			securitySuppressedIStream.Write(saveHistoryBytes, num, out num2);
			Invariant.Assert(num2 == num, "Error saving journal stream to native IStream");
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x001B70C1 File Offset: 0x001B52C1
		[SecurityCritical]
		private void LoadHistoryHelper(object comIStream, bool firstLoadFromHistory)
		{
			if (this._appProxyInternal == null)
			{
				return;
			}
			this._appProxyInternal.LoadHistoryStream(DocObjHost.ExtractComStream(comIStream), firstLoadFromHistory);
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x001B70E0 File Offset: 0x001B52E0
		[SecurityCritical]
		internal static MemoryStream ExtractComStream(object comIStream)
		{
			DocObjHost.SecuritySuppressedIStream securitySuppressedIStream = comIStream as DocObjHost.SecuritySuppressedIStream;
			if (securitySuppressedIStream == null)
			{
				throw new ArgumentNullException("comIStream");
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[1024];
			int num = 0;
			do
			{
				num = 0;
				securitySuppressedIStream.Read(array, 1024, out num);
				memoryStream.Write(array, 0, num);
			}
			while (num > 0);
			Invariant.Assert(memoryStream.Length > 0L, "Error reading journal stream from native IStream");
			return memoryStream;
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x001B7148 File Offset: 0x001B5348
		private bool IsXbapErrorPageDisabled()
		{
			object value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Hosting", "DisableXbapErrorPage", null);
			if (value == null)
			{
				value = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETFramework\\Windows Presentation Foundation\\Hosting", "DisableXbapErrorPage", null);
			}
			return value is int && (int)value != 0;
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x001B718D File Offset: 0x001B538D
		[SecuritySafeCritical]
		private void EnableErrorPage()
		{
			if (!this.IsXbapErrorPageDisabled())
			{
				AppDomain.CurrentDomain.UnhandledException += this.ProcessUnhandledException;
			}
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x001B71B0 File Offset: 0x001B53B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[MethodImpl(MethodImplOptions.Synchronized)]
		private void ProcessUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				string text = e.ExceptionObject.ToString();
				text = text.Replace(" --->", "\n--->");
				Invariant.Assert(this._browserCallbackServices != null);
				if (!Debugger.IsAttached)
				{
					if (Thread.CurrentThread == this._mainThread)
					{
						this._browserCallbackServices.ProcessUnhandledException(text);
					}
					else
					{
						UnsafeNativeMethods.ProcessUnhandledException_DLL(text);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x001B7228 File Offset: 0x001B5428
		private string GetXPSViewerPath()
		{
			string text = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\CLSID\\{7DDA204B-2097-47C9-8323-C40BB840AE44}\\LocalServer32", null, null) as string;
			if (text == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, SR.Get("DocumentApplicationRegistryKeyNotFound"), new object[]
				{
					"HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\CLSID\\{7DDA204B-2097-47C9-8323-C40BB840AE44}\\LocalServer32"
				}));
			}
			return Path.GetDirectoryName(text);
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x001B727C File Offset: 0x001B547C
		private int GetXpsViewerIsolatedStorageUserQuota()
		{
			int result = 512000000;
			object value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\XPSViewer", "IsolatedStorageUserQuota", null);
			if (value is int)
			{
				result = (int)value;
			}
			return result;
		}

		// Token: 0x0400316D RID: 12653
		private Thread _mainThread;

		// Token: 0x0400316E RID: 12654
		private ApplicationProxyInternal _appProxyInternal;

		// Token: 0x0400316F RID: 12655
		private SecurityCriticalDataForSet<ApplicationProxyInternal.InitData> _initData = new SecurityCriticalDataForSet<ApplicationProxyInternal.InitData>(new ApplicationProxyInternal.InitData());

		// Token: 0x04003170 RID: 12656
		private IntPtr _parent;

		// Token: 0x04003171 RID: 12657
		private IBrowserCallbackServices _browserCallbackServices;

		// Token: 0x04003172 RID: 12658
		private object _downloader;

		// Token: 0x04003173 RID: 12659
		private const int _defaultXpsIsolatedStorageUserQuota = 512000000;

		// Token: 0x020009FA RID: 2554
		// (Invoke) Token: 0x060089CE RID: 35278
		internal delegate bool ApplicationRunner();

		// Token: 0x020009FB RID: 2555
		// (Invoke) Token: 0x060089D2 RID: 35282
		internal delegate void ApplicationRunnerCallback(DocObjHost.ApplicationRunner runner);

		// Token: 0x020009FC RID: 2556
		[Guid("0000000c-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface SecuritySuppressedIStream
		{
			// Token: 0x060089D5 RID: 35285
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, out int pcbRead);

			// Token: 0x060089D6 RID: 35286
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out int pcbWritten);

			// Token: 0x060089D7 RID: 35287
			void Seek(long dlibMove, int dwOrigin, out long plibNewPosition);

			// Token: 0x060089D8 RID: 35288
			void SetSize(long libNewSize);

			// Token: 0x060089D9 RID: 35289
			void CopyTo(DocObjHost.SecuritySuppressedIStream pstm, long cb, out long pcbRead, out long pcbWritten);

			// Token: 0x060089DA RID: 35290
			void Commit(int grfCommitFlags);

			// Token: 0x060089DB RID: 35291
			void Revert();

			// Token: 0x060089DC RID: 35292
			void LockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x060089DD RID: 35293
			void UnlockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x060089DE RID: 35294
			void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

			// Token: 0x060089DF RID: 35295
			void Clone(out DocObjHost.SecuritySuppressedIStream ppstm);
		}
	}
}
