using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using Microsoft.Internal.DeploymentUI;
using MS.Internal.KnownBoxes;
using MS.Utility;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079C RID: 1948
	internal class XappLauncherApp : Application
	{
		// Token: 0x06007A1D RID: 31261 RVA: 0x00229800 File Offset: 0x00227A00
		[SecurityCritical]
		internal XappLauncherApp(Uri deploymentManifest, string applicationId, IBrowserCallbackServices browser, DocObjHost.ApplicationRunnerCallback applicationRunner, INativeProgressPage nativeProgressPage, string progressPageAssembly, string progressPageClass, string errorPageAssembly, string errorPageClass)
		{
			this._deploymentManifest = deploymentManifest;
			this._applicationId = applicationId;
			this._browser = browser;
			this._applicationRunnerCallback = applicationRunner;
			this._fwlinkUri = null;
			this._requiredCLRVersion = null;
			base.Startup += this.XappLauncherApp_Startup;
			base.Exit += this.XappLauncherApp_Exit;
			base.Navigated += this.XappLauncherApp_Navigated;
			this._nativeProgressPage = nativeProgressPage;
			this._progressPageAssembly = progressPageAssembly;
			this._progressPageClass = progressPageClass;
			this._errorPageAssembly = errorPageAssembly;
			this._errorPageClass = errorPageClass;
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x002298A7 File Offset: 0x00227AA7
		private void OnCommandRefresh(object sender, RoutedEventArgs e)
		{
			this.HandleRefresh();
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x002298AF File Offset: 0x00227AAF
		private void OnCommandStop(object sender, RoutedEventArgs e)
		{
			this.UserStop();
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x002298B7 File Offset: 0x00227AB7
		private void XappLauncherApp_Startup(object sender, StartupEventArgs e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_XappLauncherAppStartup);
			this.CreateApplicationIdentity();
			if (this._identity != null)
			{
				this.TryApplicationIdActivation();
				return;
			}
			this.TryUriActivation();
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x002298E4 File Offset: 0x00227AE4
		private void XappLauncherApp_Exit(object sender, ExitEventArgs e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_XappLauncherAppExit, this._runApplication);
			Invariant.Assert(!this._isInAsynchronousOperation, "Async downloading should have been canceled before XappLauncherApp exits.");
			this.RunApplicationAsyncCallback(null);
			this._browser = null;
			this._applicationRunner = null;
			this._applicationRunnerCallback = null;
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x0022993C File Offset: 0x00227B3C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void XappLauncherApp_Navigated(object sender, NavigationEventArgs e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_XappLauncherAppNavigated);
			if (Application.IsShuttingDown)
			{
				return;
			}
			if (!this._commandBindingsRegistered)
			{
				this._commandBindingsRegistered = true;
				base.MainWindow.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseStop, new ExecutedRoutedEventHandler(this.OnCommandStop)));
				base.MainWindow.CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, new ExecutedRoutedEventHandler(this.OnCommandRefresh)));
			}
			SecurityHelper.DemandUIWindowPermission();
			NavigationWindow appWindow = base.GetAppWindow();
			Invariant.Assert(appWindow != null, "A RootBrowserWindow should have been created.");
			while (appWindow.CanGoBack)
			{
				appWindow.RemoveBackEntry();
			}
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x002299E9 File Offset: 0x00227BE9
		private void StartAsynchronousOperation()
		{
			this._isInAsynchronousOperation = true;
			this.ChangeBrowserDownloadState(this._isInAsynchronousOperation);
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x002299FE File Offset: 0x00227BFE
		private void ClearAsynchronousOperationStatus()
		{
			this._isInAsynchronousOperation = false;
			this.ChangeBrowserDownloadState(this._isInAsynchronousOperation);
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x00229A13 File Offset: 0x00227C13
		private object UserRefresh(object unused)
		{
			this.HandleRefresh();
			return null;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x00229A1C File Offset: 0x00227C1C
		internal override void PerformNavigationStateChangeTasks(bool isNavigationInitiator, bool playNavigatingSound, Application.NavigationStateChange state)
		{
			if (isNavigationInitiator && state == Application.NavigationStateChange.Completed)
			{
				base.UpdateBrowserCommands();
			}
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x00229A2C File Offset: 0x00227C2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void HandleRefresh()
		{
			object lockObject = this._lockObject;
			lock (lockObject)
			{
				if (!this._refreshing)
				{
					this._refreshing = true;
					base.BrowserCallbackServices.DelegateNavigation(this._deploymentManifest.ToString(), null, null);
				}
			}
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x00229A90 File Offset: 0x00227C90
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ChangeBrowserDownloadState(bool newState)
		{
			try
			{
				this._browser.ChangeDownloadState(newState);
			}
			catch (Exception ex)
			{
				if ((!(ex is InvalidComObjectException) && !(ex is COMException) && !(ex is InvalidOperationException)) || newState || !Application.IsShuttingDown)
				{
					throw;
				}
			}
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x00229AEC File Offset: 0x00227CEC
		private void TryApplicationIdActivation()
		{
			base.Dispatcher.Invoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.DoDirectActivation), null);
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x00229B08 File Offset: 0x00227D08
		private void TryUriActivation()
		{
			if (this._hasTriedUriActivation)
			{
				base.Shutdown();
				return;
			}
			this._hasTriedUriActivation = true;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.WpfHost_FirstTimeActivation);
			ThreadStart threadStart = delegate()
			{
				this._hostingManager = new InPlaceHostingManager(this._deploymentManifest);
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new SendOrPostCallback(delegate(object <p0>)
				{
					this.DoGetManifestAsync();
					this.DoDownloadUI();
				}), null);
			};
			threadStart.BeginInvoke(null, null);
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x00229B54 File Offset: 0x00227D54
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object DoDirectActivation(object unused)
		{
			if (this.IsCanceledOrShuttingDown)
			{
				return null;
			}
			try
			{
				if (ApplicationSecurityManager.UserApplicationTrusts[this._identity.ToString()] != null)
				{
					this._context = ActivationContext.CreatePartialActivationContext(this._identity);
					this.RunApplicationAsync(new DocObjHost.ApplicationRunner(this.ExecuteDirectApplication));
				}
				else
				{
					this.TryUriActivation();
				}
			}
			catch (Exception ex)
			{
				this.DeleteCachedApplicationTrust(this._identity);
				if (ex is NullReferenceException || ex is SEHException)
				{
					throw;
				}
				this.TryUriActivation();
			}
			return null;
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x00229BE8 File Offset: 0x00227DE8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool ExecuteDirectApplication()
		{
			SecurityHelper.DemandUnmanagedCode();
			this._runApplication = false;
			try
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_ClickOnceActivationStart, BooleanBoxes.TrueBox);
				ObjectHandle oh = Activator.CreateInstance(this._context);
				this.GotNewAppDomain(oh);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_ClickOnceActivationEnd);
				base.Shutdown();
				return true;
			}
			catch (Exception ex)
			{
				this.DeleteCachedApplicationTrust(this._identity);
				if (ex is NullReferenceException || ex is SEHException)
				{
					throw;
				}
				this.TryUriActivation();
			}
			return false;
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x00229C7C File Offset: 0x00227E7C
		private void GotNewAppDomain(ObjectHandle oh)
		{
			Invariant.Assert(PresentationAppDomainManager.SaveAppDomain);
			if (oh != null)
			{
				AppDomain appDomain = (AppDomain)oh.Unwrap();
				if (appDomain != null)
				{
					PresentationAppDomainManager.NewAppDomain = appDomain;
					return;
				}
			}
			throw new ApplicationException(SR.Get("AppActivationException"));
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x00229CBC File Offset: 0x00227EBC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DoGetManifestAsync()
		{
			if (this.IsCanceledOrShuttingDown)
			{
				return;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_DownloadDeplManifestStart);
			this.StartAsynchronousOperation();
			this.SetStatusText(SR.Get("HostingStatusDownloadAppInfo"));
			this._hostingManager.GetManifestCompleted += this.GetManifestCompleted;
			if (!this.IsCanceledOrShuttingDown)
			{
				this._hostingManager.GetManifestAsync();
			}
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x00229D24 File Offset: 0x00227F24
		private object GetCustomPage(string pageAssemblyName, string pageClassName)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_GetDownloadPageStart, pageClassName);
			object result;
			try
			{
				Assembly assembly = string.IsNullOrEmpty(pageAssemblyName) ? typeof(TenFeetInstallationProgress).Assembly : Assembly.Load(pageAssemblyName);
				result = assembly.CreateInstance(pageClassName);
			}
			catch
			{
				result = null;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_GetDownloadPageEnd);
			return result;
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x00229D94 File Offset: 0x00227F94
		private void GetManifestCompleted(object sender, GetManifestCompletedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.DoGetManifestCompleted), e);
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x00229DB0 File Offset: 0x00227FB0
		private object DoGetManifestCompleted(object e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_DownloadDeplManifestEnd);
			GetManifestCompletedEventArgs getManifestCompletedEventArgs = (GetManifestCompletedEventArgs)e;
			this._getManifestCompletedEventArgs = getManifestCompletedEventArgs;
			bool flag = this._canceled || getManifestCompletedEventArgs.Cancelled;
			this.ClearAsynchronousOperationStatus();
			if (Application.IsShuttingDown)
			{
				return null;
			}
			if (getManifestCompletedEventArgs.Error != null)
			{
				this.HandleError(getManifestCompletedEventArgs.Error, getManifestCompletedEventArgs.LogFilePath, null, null);
				return null;
			}
			if (flag)
			{
				this.HandleCancel();
				return null;
			}
			this._identity = getManifestCompletedEventArgs.ApplicationIdentity;
			if (this._progressPage != null)
			{
				this._progressPage.ApplicationName = getManifestCompletedEventArgs.ProductName;
				XmlReader deploymentManifest = getManifestCompletedEventArgs.DeploymentManifest;
				deploymentManifest.MoveToContent();
				if (deploymentManifest.LocalName == "assembly")
				{
					deploymentManifest.ReadStartElement();
					while (deploymentManifest.NodeType != XmlNodeType.EndElement)
					{
						if (deploymentManifest.LocalName == "description")
						{
							string attribute = deploymentManifest.GetAttribute("publisher", "urn:schemas-microsoft-com:asm.v2");
							if (!string.IsNullOrEmpty(attribute))
							{
								this._progressPage.PublisherName = attribute;
								break;
							}
							break;
						}
						else
						{
							deploymentManifest.Skip();
						}
					}
				}
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_DownloadApplicationStart);
			this.ShowDownloadingStatusMessage();
			this.StartAsynchronousOperation();
			if (this.IsCanceledOrShuttingDown)
			{
				return null;
			}
			this._hostingManager.DownloadProgressChanged += this.DownloadProgressChanged;
			this._hostingManager.DownloadApplicationCompleted += this.DownloadApplicationCompleted;
			this._hostingManager.DownloadApplicationAsync();
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.AssertApplicationRequirementsAsync), null);
			this._assertAppRequirementsEvent = new ManualResetEvent(false);
			return null;
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x00229F44 File Offset: 0x00228144
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ShowDownloadingStatusMessage()
		{
			this.SetStatusText(SR.Get("HostingStatusDownloadApp"));
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x00229F58 File Offset: 0x00228158
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object AssertApplicationRequirementsAsync(object unused)
		{
			if (this.IsCanceledOrShuttingDown)
			{
				return null;
			}
			if (base.CheckAccess())
			{
				this.SetStatusText(SR.Get("HostingStatusVerifying"));
				Thread thread = new Thread(delegate()
				{
					this.AssertApplicationRequirementsAsync(null);
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Name = "IPHM.AAR thread";
				thread.Start();
			}
			else
			{
				try
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AssertAppRequirementsStart);
					this._hostingManager.AssertApplicationRequirements();
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AssertAppRequirementsEnd);
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object unused3)
					{
						if (this._isInAsynchronousOperation)
						{
							this.ShowDownloadingStatusMessage();
						}
						return null;
					}), null);
				}
				catch (Exception ex)
				{
					if (!(ex.GetType() == typeof(InvalidOperationException)) || !(ex.Source == "System.Deployment"))
					{
						this._assertAppRequirementsFailed = true;
						base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object exceptionObj)
						{
							Exception ex2 = (Exception)exceptionObj;
							if (CriticalExceptions.IsCriticalException(ex2))
							{
								throw new DeploymentException(SR.Get("UnknownErrorText"), ex2);
							}
							GetManifestCompletedEventArgs getManifestCompletedEventArgs = this._getManifestCompletedEventArgs;
							string text = null;
							if (ex2 is TrustNotGrantedException)
							{
								text = this.GetMissingCustomPermissionVersion(getManifestCompletedEventArgs.ApplicationManifest);
								if (!string.IsNullOrEmpty(text))
								{
									ex2 = new DependentPlatformMissingException();
								}
							}
							this.HandleError(ex2, getManifestCompletedEventArgs.LogFilePath, getManifestCompletedEventArgs.SupportUri, text);
							return null;
						}), ex);
					}
				}
				finally
				{
					this._assertAppRequirementsEvent.Set();
				}
			}
			return null;
		}

		// Token: 0x06007A34 RID: 31284 RVA: 0x0022A07C File Offset: 0x0022827C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DoDownloadUI()
		{
			SecurityHelper.DemandUIWindowPermission();
			bool flag = true;
			if (this._progressPageClass != null)
			{
				this._progressPage = (this.GetCustomPage(this._progressPageAssembly, this._progressPageClass) as IProgressPage);
			}
			if (this._progressPage == null)
			{
				flag = false;
				Invariant.Assert(this._nativeProgressPage != null);
				this._progressPage = new NativeProgressPageProxy(this._nativeProgressPage);
			}
			this._progressPage.DeploymentPath = this._deploymentManifest;
			this._progressPage.StopCallback = new DispatcherOperationCallback(this.UserStop);
			this._progressPage.RefreshCallback = new DispatcherOperationCallback(this.UserRefresh);
			if (flag)
			{
				this.BrowserWindow.Navigate(this._progressPage);
				return;
			}
			this._nativeProgressPage.Show();
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x0022A140 File Offset: 0x00228340
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void HandleError(Exception exception, string logFilePath, Uri supportUri, string requiredWpfVersion)
		{
			SecurityHelper.DemandUIWindowPermission();
			this.ClearAsynchronousOperationStatus();
			this.DeleteCachedApplicationTrust(this._identity);
			if (Application.IsShuttingDown)
			{
				this.AbortActivation();
				return;
			}
			this.SetStatusText(SR.Get("HostingStatusFailed"));
			string text = string.Empty;
			MissingDependencyType missingDependencyType = MissingDependencyType.Others;
			if (exception is DependentPlatformMissingException)
			{
				if (requiredWpfVersion != null)
				{
					missingDependencyType = MissingDependencyType.WinFX;
					text = requiredWpfVersion;
					DeploymentExceptionMapper.ConstructFwlinkUrl(text, out this._fwlinkUri);
					this._requiredCLRVersion = XappLauncherApp.ClrVersionFromWinFXVersion(text);
				}
				else
				{
					missingDependencyType = DeploymentExceptionMapper.GetWinFXRequirement(exception, this._hostingManager, out text, out this._fwlinkUri);
					if (missingDependencyType != MissingDependencyType.WinFX)
					{
						if (missingDependencyType == MissingDependencyType.CLR)
						{
							this._requiredCLRVersion = text;
						}
					}
					else
					{
						this._requiredCLRVersion = XappLauncherApp.ClrVersionFromWinFXVersion(text);
					}
				}
			}
			if (string.IsNullOrEmpty(this._requiredCLRVersion))
			{
				missingDependencyType = MissingDependencyType.Others;
			}
			string errorTitle;
			string errorText;
			if (missingDependencyType != MissingDependencyType.WinFX)
			{
				if (missingDependencyType != MissingDependencyType.CLR)
				{
					DeploymentExceptionMapper.GetErrorTextFromException(exception, out errorTitle, out errorText);
				}
				else
				{
					errorTitle = SR.Get("PlatformRequirementTitle");
					errorText = SR.Get("IncompatibleCLRText", new object[]
					{
						text
					});
				}
			}
			else
			{
				errorTitle = SR.Get("PlatformRequirementTitle");
				errorText = SR.Get("IncompatibleWinFXText", new object[]
				{
					text
				});
			}
			IErrorPage errorPage = null;
			if (this._errorPageClass != null)
			{
				errorPage = (this.GetCustomPage(this._errorPageAssembly, this._errorPageClass) as IErrorPage);
			}
			if (errorPage == null)
			{
				errorPage = new InstallationErrorPage();
			}
			errorPage.DeploymentPath = this._deploymentManifest;
			errorPage.ErrorTitle = errorTitle;
			errorPage.ErrorText = errorText;
			errorPage.SupportUri = supportUri;
			errorPage.LogFilePath = logFilePath;
			errorPage.RefreshCallback = new DispatcherOperationCallback(this.UserRefresh);
			errorPage.GetWinFxCallback = ((missingDependencyType != MissingDependencyType.Others) ? new DispatcherOperationCallback(this.GetWinFX) : null);
			errorPage.ErrorFlag = true;
			this.BrowserWindow.Navigate(errorPage);
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x0022A2F0 File Offset: 0x002284F0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void HandleCancel()
		{
			SecurityHelper.DemandUIWindowPermission();
			if (this._cancelHandled || this._runApplication)
			{
				return;
			}
			this._cancelHandled = (this._canceled = true);
			this.DeleteCachedApplicationTrust(this._identity);
			if (Application.IsShuttingDown)
			{
				this.AbortActivation();
				return;
			}
			this.CancelAsynchronousOperation();
			this.SetStatusText(SR.Get("HostingStatusCancelled"));
			string errorTitle;
			string errorText;
			DeploymentExceptionMapper.GetErrorTextFromException(null, out errorTitle, out errorText);
			IErrorPage errorPage = null;
			if (this._errorPageAssembly != null || this._errorPageClass != null)
			{
				errorPage = (this.GetCustomPage(this._errorPageAssembly, this._errorPageClass) as IErrorPage);
			}
			if (errorPage == null)
			{
				errorPage = new InstallationErrorPage();
			}
			errorPage.DeploymentPath = this._deploymentManifest;
			errorPage.ErrorTitle = errorTitle;
			errorPage.ErrorText = errorText;
			errorPage.SupportUri = null;
			errorPage.LogFilePath = null;
			errorPage.ErrorFlag = false;
			errorPage.RefreshCallback = new DispatcherOperationCallback(this.UserRefresh);
			errorPage.GetWinFxCallback = null;
			this.BrowserWindow.Navigate(errorPage);
		}

		// Token: 0x06007A37 RID: 31287 RVA: 0x0022A3E8 File Offset: 0x002285E8
		private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this._bytesDownloaded = e.BytesDownloaded;
			this._bytesTotal = e.TotalBytesToDownload;
			if (!this._updatePending)
			{
				this._updatePending = true;
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.DoDownloadProgressChanged), null);
			}
		}

		// Token: 0x06007A38 RID: 31288 RVA: 0x0022A438 File Offset: 0x00228638
		[SecuritySafeCritical]
		private object DoDownloadProgressChanged(object unused)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_DownloadProgressUpdate, this._bytesDownloaded, this._bytesTotal);
			if (!this._isInAsynchronousOperation || Application.IsShuttingDown)
			{
				return null;
			}
			SecurityHelper.DemandUIWindowPermission();
			if (this._progressPage != null)
			{
				this._progressPage.UpdateProgress(this._bytesDownloaded, this._bytesTotal);
			}
			this._updatePending = false;
			return null;
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x0022A4A8 File Offset: 0x002286A8
		private void DownloadApplicationCompleted(object sender, DownloadApplicationCompletedEventArgs e)
		{
			this._hostingManager.DownloadProgressChanged -= this.DownloadProgressChanged;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.DoDownloadApplicationCompleted), e);
		}

		// Token: 0x06007A3A RID: 31290 RVA: 0x0022A4DC File Offset: 0x002286DC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object DoDownloadApplicationCompleted(object e)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_DownloadApplicationEnd);
			DownloadApplicationCompletedEventArgs downloadApplicationCompletedEventArgs = (DownloadApplicationCompletedEventArgs)e;
			this.ClearAsynchronousOperationStatus();
			if (Application.IsShuttingDown)
			{
				return null;
			}
			if (downloadApplicationCompletedEventArgs.Error != null)
			{
				this._assertAppRequirementsEvent.WaitOne();
				if (!this._assertAppRequirementsFailed)
				{
					this.HandleError(downloadApplicationCompletedEventArgs.Error, downloadApplicationCompletedEventArgs.LogFilePath, this._getManifestCompletedEventArgs.SupportUri, null);
				}
				return null;
			}
			bool flag = this._canceled || downloadApplicationCompletedEventArgs.Cancelled;
			if (flag)
			{
				this.HandleCancel();
				return null;
			}
			this.SetStatusText(SR.Get("HostingStatusPreparingToRun"));
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object unused)
			{
				if (!this.IsCanceledOrShuttingDown)
				{
					this.RunApplicationAsync(new DocObjHost.ApplicationRunner(this.ExecuteDownloadedApplication));
				}
				return null;
			}), null);
			return null;
		}

		// Token: 0x06007A3B RID: 31291 RVA: 0x0022A598 File Offset: 0x00228798
		private bool ExecuteDownloadedApplication()
		{
			this._runApplication = false;
			bool result;
			try
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_ClickOnceActivationStart, BooleanBoxes.FalseBox);
				ObjectHandle oh = this._hostingManager.Execute();
				this.GotNewAppDomain(oh);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_ClickOnceActivationEnd);
				result = true;
			}
			finally
			{
				base.Shutdown();
			}
			return result;
		}

		// Token: 0x06007A3C RID: 31292 RVA: 0x0022A600 File Offset: 0x00228800
		private object UserStop(object unused)
		{
			this.UserStop();
			return null;
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x0022A609 File Offset: 0x00228809
		internal void UserStop()
		{
			if (this._isInAsynchronousOperation)
			{
				this.CancelAsynchronousOperation();
				return;
			}
			this.HandleCancel();
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x0022A620 File Offset: 0x00228820
		internal void AbortActivation()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordHosting, EventTrace.Event.WpfHost_AbortingActivation);
			this.CancelAsynchronousOperation();
			this._runApplication = false;
			base.Shutdown(30);
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x0022A648 File Offset: 0x00228848
		private void CancelAsynchronousOperation()
		{
			object lockObject = this._lockObject;
			lock (lockObject)
			{
				if (this._isInAsynchronousOperation)
				{
					this._canceled = true;
					Invariant.Assert(this._hostingManager != null, "_hostingManager should not be null if _isInAsynchronousOperation is true");
					this._hostingManager.CancelAsync();
					this.ClearAsynchronousOperationStatus();
				}
			}
		}

		// Token: 0x17001CC7 RID: 7367
		// (get) Token: 0x06007A40 RID: 31296 RVA: 0x0022A6B8 File Offset: 0x002288B8
		private bool IsCanceledOrShuttingDown
		{
			get
			{
				return this._canceled || Application.IsShuttingDown;
			}
		}

		// Token: 0x17001CC8 RID: 7368
		// (get) Token: 0x06007A41 RID: 31297 RVA: 0x0022A6CC File Offset: 0x002288CC
		private RootBrowserWindow BrowserWindow
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				RootBrowserWindow rootBrowserWindow = (RootBrowserWindow)base.GetAppWindow();
				Invariant.Assert(rootBrowserWindow != null, "Should have instantiated RBW if it wasn't already there");
				rootBrowserWindow.ShowsNavigationUI = false;
				return rootBrowserWindow;
			}
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x0022A700 File Offset: 0x00228900
		private void CreateApplicationIdentity()
		{
			this._identity = null;
			if (this._applicationId != null)
			{
				try
				{
					this._identity = new ApplicationIdentity(this._applicationId);
				}
				catch (NullReferenceException)
				{
					throw;
				}
				catch (SEHException)
				{
					throw;
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06007A43 RID: 31299 RVA: 0x0022A760 File Offset: 0x00228960
		[SecurityCritical]
		private void DeleteCachedApplicationTrust(ApplicationIdentity identity)
		{
			if (identity != null)
			{
				ApplicationTrust trust = new ApplicationTrust(identity);
				ApplicationSecurityManager.UserApplicationTrusts.Remove(trust);
			}
		}

		// Token: 0x06007A44 RID: 31300 RVA: 0x0022A784 File Offset: 0x00228984
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object GetWinFX(object unused)
		{
			bool flag = false;
			SecurityHelper.DemandUnmanagedCode();
			if (OperatingSystemVersionCheck.IsVersionOrLater(OperatingSystemVersion.Windows8))
			{
				Invariant.Assert(!string.IsNullOrEmpty(this._requiredCLRVersion));
				XappLauncherApp.UnsafeNativeMethods.TryGetRequestedCLRRuntime(this._requiredCLRVersion);
				flag = true;
			}
			else if (OperatingSystemVersionCheck.IsVersionOrLater(OperatingSystemVersion.WindowsVista))
			{
				string requiredCLRVersion = this._requiredCLRVersion;
				if (requiredCLRVersion == "v2.0.50727")
				{
					string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "ocsetup.exe");
					ProcessStartInfo startInfo = new ProcessStartInfo(fileName, "NetFx3")
					{
						Verb = "runas"
					};
					Process.Start(startInfo);
					flag = true;
				}
				else
				{
					Invariant.Assert(this._fwlinkUri != null);
					AppSecurityManager.ShellExecuteDefaultBrowser(this._fwlinkUri);
					flag = true;
				}
			}
			if (!flag && this._fwlinkUri != null)
			{
				Invariant.Assert(this._fwlinkUri != null);
				AppSecurityManager.ShellExecuteDefaultBrowser(this._fwlinkUri);
			}
			return null;
		}

		// Token: 0x06007A45 RID: 31301 RVA: 0x0022A860 File Offset: 0x00228A60
		private static string ClrVersionFromWinFXVersion(string winfxVersion)
		{
			string result = null;
			if (!(winfxVersion == "3.0") && !(winfxVersion == "3.5"))
			{
				if (winfxVersion == "4.0")
				{
					result = "v4.0.30319";
				}
			}
			else
			{
				result = "v2.0.50727";
			}
			return result;
		}

		// Token: 0x06007A46 RID: 31302 RVA: 0x0022A8A8 File Offset: 0x00228AA8
		[SecurityCritical]
		private void SetStatusText(string newStatusText)
		{
			IProgressPage2 progressPage = this._progressPage as IProgressPage2;
			if (progressPage != null)
			{
				progressPage.ShowProgressMessage(newStatusText);
			}
			BrowserInteropHelper.HostBrowser.SetStatusText(newStatusText);
		}

		// Token: 0x06007A47 RID: 31303 RVA: 0x0022A8D8 File Offset: 0x00228AD8
		private string GetMissingCustomPermissionVersion(XmlReader reader)
		{
			string result = null;
			while (reader.ReadToFollowing("IPermission", "urn:schemas-microsoft-com:asm.v2"))
			{
				string attribute = reader.GetAttribute("class");
				AssemblyName assemblyName = new AssemblyName(attribute.Substring(attribute.IndexOf(",", StringComparison.OrdinalIgnoreCase) + 1));
				if (assemblyName.Name.Equals("WindowsBase", StringComparison.OrdinalIgnoreCase))
				{
					try
					{
						Assembly assembly = Assembly.Load(assemblyName);
					}
					catch (Exception ex)
					{
						if (ex is FileNotFoundException || ex is FileLoadException)
						{
							result = assemblyName.Version.ToString();
							break;
						}
						throw;
					}
				}
			}
			reader.Close();
			return result;
		}

		// Token: 0x06007A48 RID: 31304 RVA: 0x0022A978 File Offset: 0x00228B78
		private void RunApplicationAsync(DocObjHost.ApplicationRunner applicationRunner)
		{
			Invariant.Assert(applicationRunner != null);
			this._applicationRunner = applicationRunner;
			this._runApplication = true;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.RunApplicationAsyncCallback), null);
		}

		// Token: 0x06007A49 RID: 31305 RVA: 0x0022A9AC File Offset: 0x00228BAC
		private object RunApplicationAsyncCallback(object unused)
		{
			if (this._runApplication)
			{
				this._applicationRunnerCallback(new DocObjHost.ApplicationRunner(this._applicationRunner.Invoke));
			}
			return null;
		}

		// Token: 0x040039B7 RID: 14775
		private InPlaceHostingManager _hostingManager;

		// Token: 0x040039B8 RID: 14776
		private IBrowserCallbackServices _browser;

		// Token: 0x040039B9 RID: 14777
		private ApplicationIdentity _identity;

		// Token: 0x040039BA RID: 14778
		private Uri _deploymentManifest;

		// Token: 0x040039BB RID: 14779
		private Uri _fwlinkUri;

		// Token: 0x040039BC RID: 14780
		private string _requiredCLRVersion;

		// Token: 0x040039BD RID: 14781
		private string _applicationId;

		// Token: 0x040039BE RID: 14782
		private ActivationContext _context;

		// Token: 0x040039BF RID: 14783
		private DocObjHost.ApplicationRunnerCallback _applicationRunnerCallback;

		// Token: 0x040039C0 RID: 14784
		private DocObjHost.ApplicationRunner _applicationRunner;

		// Token: 0x040039C1 RID: 14785
		[SecurityCritical]
		private INativeProgressPage _nativeProgressPage;

		// Token: 0x040039C2 RID: 14786
		private IProgressPage _progressPage;

		// Token: 0x040039C3 RID: 14787
		private bool _runApplication;

		// Token: 0x040039C4 RID: 14788
		private GetManifestCompletedEventArgs _getManifestCompletedEventArgs;

		// Token: 0x040039C5 RID: 14789
		private const int ERROR_ACTIVATION_ABORTED = 30;

		// Token: 0x040039C6 RID: 14790
		private object _lockObject = new object();

		// Token: 0x040039C7 RID: 14791
		private ManualResetEvent _assertAppRequirementsEvent;

		// Token: 0x040039C8 RID: 14792
		private long _bytesDownloaded;

		// Token: 0x040039C9 RID: 14793
		private long _bytesTotal;

		// Token: 0x040039CA RID: 14794
		private bool _updatePending;

		// Token: 0x040039CB RID: 14795
		private string _progressPageAssembly;

		// Token: 0x040039CC RID: 14796
		private string _progressPageClass;

		// Token: 0x040039CD RID: 14797
		private string _errorPageAssembly;

		// Token: 0x040039CE RID: 14798
		private string _errorPageClass;

		// Token: 0x040039CF RID: 14799
		private bool _commandBindingsRegistered;

		// Token: 0x040039D0 RID: 14800
		private bool _isInAsynchronousOperation;

		// Token: 0x040039D1 RID: 14801
		private volatile bool _canceled;

		// Token: 0x040039D2 RID: 14802
		private bool _cancelHandled;

		// Token: 0x040039D3 RID: 14803
		private volatile bool _assertAppRequirementsFailed;

		// Token: 0x040039D4 RID: 14804
		private bool _refreshing;

		// Token: 0x040039D5 RID: 14805
		private bool _hasTriedUriActivation;

		// Token: 0x02000B78 RID: 2936
		[SecurityCritical]
		private static class UnsafeNativeMethods
		{
			// Token: 0x06008E39 RID: 36409
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", CharSet = CharSet.Unicode)]
			public static extern int TryGetRequestedCLRRuntime(string versionString);
		}
	}
}
