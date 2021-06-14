using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Threading;
using Microsoft.Win32;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.IO.Packaging;
using MS.Internal.Telemetry;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;
using MS.Utility;
using MS.Win32;

namespace System.Windows
{
	/// <summary>Encapsulates a Windows Presentation Foundation (WPF) application. </summary>
	// Token: 0x0200009B RID: 155
	public class Application : DispatcherObject, IHaveResources, IQueryAmbient
	{
		// Token: 0x06000274 RID: 628 RVA: 0x00006362 File Offset: 0x00004562
		static Application()
		{
			Application.ApplicationInit();
			NetFxVersionTraceLogger.LogVersionDetails();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.</summary>
		/// <exception cref="T:System.InvalidOperationException">More than one instance of the <see cref="T:System.Windows.Application" /> class is created per <see cref="T:System.AppDomain" />.</exception>
		// Token: 0x06000275 RID: 629 RVA: 0x00006394 File Offset: 0x00004594
		[SecurityCritical]
		public Application()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppCtor);
			object globalLock = Application._globalLock;
			lock (globalLock)
			{
				if (Application._appCreatedInThisAppDomain)
				{
					throw new InvalidOperationException(SR.Get("MultiSingleton"));
				}
				Application._appInstance = this;
				Application.IsShuttingDown = false;
				Application._appCreatedInThisAppDomain = true;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.StartDispatcherInBrowser), null);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
			{
				if (Application.IsShuttingDown)
				{
					return null;
				}
				StartupEventArgs startupEventArgs = new StartupEventArgs();
				this.OnStartup(startupEventArgs);
				if (startupEventArgs.PerformDefaultAction)
				{
					this.DoStartup();
				}
				return null;
			}), null);
		}

		/// <summary>Starts a Windows Presentation Foundation (WPF) application.</summary>
		/// <returns>The <see cref="T:System.Int32" /> application exit code that is returned to the operating system when the application shuts down. By default, the exit code value is 0.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Application.Run" /> is called from a browser-hosted application (for example, an XAML browser application (XBAP)).</exception>
		// Token: 0x06000276 RID: 630 RVA: 0x0000643C File Offset: 0x0000463C
		public int Run()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppRun);
			return this.Run(null);
		}

		/// <summary>Starts a Windows Presentation Foundation (WPF) application and opens the specified window.</summary>
		/// <param name="window">A <see cref="T:System.Windows.Window" /> that opens automatically when an application starts.</param>
		/// <returns>The <see cref="T:System.Int32" /> application exit code that is returned to the operating system when the application shuts down. By default, the exit code value is 0.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="M:System.Windows.Application.Run" /> is called from a browser-hosted application (for example, an XAML browser application (XBAP)).</exception>
		// Token: 0x06000277 RID: 631 RVA: 0x0000644C File Offset: 0x0000464C
		[SecurityCritical]
		public int Run(Window window)
		{
			base.VerifyAccess();
			if (Application.InBrowserHostedApp())
			{
				throw new InvalidOperationException(SR.Get("CannotCallRunFromBrowserHostedApp"));
			}
			return this.RunInternal(window);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00006472 File Offset: 0x00004672
		internal static bool InBrowserHostedApp()
		{
			return BrowserInteropHelper.IsBrowserHosted && !(Application.Current is XappLauncherApp);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00006490 File Offset: 0x00004690
		internal object GetService(Type serviceType)
		{
			base.VerifyAccess();
			object result = null;
			if (this.ServiceProvider != null)
			{
				result = this.ServiceProvider.GetService(serviceType);
			}
			return result;
		}

		/// <summary>Shuts down an application.</summary>
		// Token: 0x0600027A RID: 634 RVA: 0x000064BB File Offset: 0x000046BB
		public void Shutdown()
		{
			this.Shutdown(0);
		}

		/// <summary>Shuts down an application that returns the specified exit code to the operating system.</summary>
		/// <param name="exitCode">An integer exit code for an application. The default exit code is 0.</param>
		// Token: 0x0600027B RID: 635 RVA: 0x000064C4 File Offset: 0x000046C4
		[SecurityCritical]
		public void Shutdown(int exitCode)
		{
			SecurityHelper.DemandUIWindowPermission();
			this.CriticalShutdown(exitCode);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000064D2 File Offset: 0x000046D2
		[SecurityCritical]
		internal void CriticalShutdown(int exitCode)
		{
			base.VerifyAccess();
			if (Application.IsShuttingDown)
			{
				return;
			}
			ControlsTraceLogger.LogUsedControlsDetails();
			this.SetExitCode(exitCode);
			Application.IsShuttingDown = true;
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.ShutdownCallback), null);
		}

		/// <summary>Searches for a user interface (UI) resource, such as a <see cref="T:System.Windows.Style" /> or <see cref="T:System.Windows.Media.Brush" />, with the specified key, and throws an exception if the requested resource is not found (see XAML Resources).</summary>
		/// <param name="resourceKey">The name of the resource to find.</param>
		/// <returns>The requested resource object. If the requested resource is not found, a <see cref="T:System.Windows.ResourceReferenceKeyNotFoundException" /> is thrown.</returns>
		/// <exception cref="T:System.Windows.ResourceReferenceKeyNotFoundException">The resource cannot be found.</exception>
		// Token: 0x0600027D RID: 637 RVA: 0x00006510 File Offset: 0x00004710
		public object FindResource(object resourceKey)
		{
			ResourceDictionary resources = this._resources;
			object obj = null;
			if (resources != null)
			{
				obj = resources[resourceKey];
			}
			if (obj == DependencyProperty.UnsetValue || obj == null)
			{
				obj = SystemResources.FindResourceInternal(resourceKey);
			}
			if (obj == null)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		/// <summary>Searches for the specified resource.</summary>
		/// <param name="resourceKey">The name of the resource to find.</param>
		/// <returns>The requested resource object. If the requested resource is not found, a null reference is returned.</returns>
		// Token: 0x0600027E RID: 638 RVA: 0x00006550 File Offset: 0x00004750
		public object TryFindResource(object resourceKey)
		{
			ResourceDictionary resources = this._resources;
			object obj = null;
			if (resources != null)
			{
				obj = resources[resourceKey];
			}
			if (obj == DependencyProperty.UnsetValue || obj == null)
			{
				obj = SystemResources.FindResourceInternal(resourceKey);
			}
			return obj;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00006584 File Offset: 0x00004784
		internal object FindResourceInternal(object resourceKey)
		{
			return this.FindResourceInternal(resourceKey, false, false);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00006590 File Offset: 0x00004790
		internal object FindResourceInternal(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resources = this._resources;
			if (resources == null)
			{
				return null;
			}
			bool flag;
			return resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
		}

		/// <summary>Loads a XAML file that is located at the specified uniform resource identifier (URI) and converts it to an instance of the object that is specified by the root element of the XAML file.</summary>
		/// <param name="component">An object of the same type as the root element of the XAML file.</param>
		/// <param name="resourceLocator">A <see cref="T:System.Uri" /> that maps to a relative XAML file.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="component" /> is null.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="resourceLocator" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Uri.OriginalString" /> property of the <paramref name="resourceLocator" /><see cref="T:System.Uri" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="resourceLocator" /> is an absolute URI.</exception>
		/// <exception cref="T:System.Exception">
		///         <paramref name="component" /> is of a type that does not match the root element of the XAML file.</exception>
		// Token: 0x06000281 RID: 641 RVA: 0x000065B4 File Offset: 0x000047B4
		[SecurityCritical]
		public static void LoadComponent(object component, Uri resourceLocator)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (resourceLocator == null)
			{
				throw new ArgumentNullException("resourceLocator");
			}
			if (resourceLocator.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"resourceLocator",
					"OriginalString"
				}));
			}
			if (resourceLocator.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriNotAllowed"));
			}
			Uri uri = new Uri(BaseUriHelper.PackAppBaseUri, resourceLocator);
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = uri;
			Stream stream;
			bool closeStream;
			if (Application.IsComponentBeingLoadedFromOuterLoadBaml(uri))
			{
				NestedBamlLoadInfo nestedBamlLoadInfo = Application.s_NestedBamlLoadInfo.Peek();
				stream = nestedBamlLoadInfo.BamlStream;
				stream.Seek(0L, SeekOrigin.Begin);
				parserContext.SkipJournaledProperties = nestedBamlLoadInfo.SkipJournaledProperties;
				nestedBamlLoadInfo.BamlUri = null;
				closeStream = false;
			}
			else
			{
				PackagePart resourceOrContentPart = Application.GetResourceOrContentPart(resourceLocator);
				ContentType contentType = new ContentType(resourceOrContentPart.ContentType);
				stream = resourceOrContentPart.GetStream();
				closeStream = true;
				if (!MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
				{
					throw new Exception(SR.Get("ContentTypeNotSupported", new object[]
					{
						contentType
					}));
				}
			}
			IStreamInfo streamInfo = stream as IStreamInfo;
			if (streamInfo == null || streamInfo.Assembly != component.GetType().Assembly)
			{
				throw new Exception(SR.Get("UriNotMatchWithRootType", new object[]
				{
					component.GetType(),
					resourceLocator
				}));
			}
			XamlReader.LoadBaml(stream, parserContext, component, closeStream);
		}

		/// <summary>Loads a XAML file that is located at the specified uniform resource identifier (URI), and converts it to an instance of the object that is specified by the root element of the XAML file.</summary>
		/// <param name="resourceLocator">A <see cref="T:System.Uri" /> that maps to a relative XAML file.</param>
		/// <returns>An instance of the root element specified by the XAML file loaded. </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="resourceLocator" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Uri.OriginalString" /> property of the <paramref name="resourceLocator" /><see cref="T:System.Uri" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="resourceLocator" /> is an absolute URI.</exception>
		/// <exception cref="T:System.Exception">The file is not a XAML file.</exception>
		// Token: 0x06000282 RID: 642 RVA: 0x00006720 File Offset: 0x00004920
		public static object LoadComponent(Uri resourceLocator)
		{
			if (resourceLocator == null)
			{
				throw new ArgumentNullException("resourceLocator");
			}
			if (resourceLocator.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"resourceLocator",
					"OriginalString"
				}));
			}
			if (resourceLocator.IsAbsoluteUri)
			{
				throw new ArgumentException(SR.Get("AbsoluteUriNotAllowed"));
			}
			return Application.LoadComponent(resourceLocator, false);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00006790 File Offset: 0x00004990
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static object LoadComponent(Uri resourceLocator, bool bSkipJournaledProperties)
		{
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.PackAppBaseUri, resourceLocator);
			PackagePart resourceOrContentPart = Application.GetResourceOrContentPart(resolvedUri);
			ContentType contentType = new ContentType(resourceOrContentPart.ContentType);
			Stream stream = resourceOrContentPart.GetStream();
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = resolvedUri;
			parserContext.SkipJournaledProperties = bSkipJournaledProperties;
			if (MimeTypeMapper.BamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				return Application.LoadBamlStreamWithSyncInfo(stream, parserContext);
			}
			if (MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(contentType))
			{
				return XamlReader.Load(stream, parserContext);
			}
			throw new Exception(SR.Get("ContentTypeNotSupported", new object[]
			{
				contentType.ToString()
			}));
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00006824 File Offset: 0x00004A24
		internal static object LoadBamlStreamWithSyncInfo(Stream stream, ParserContext pc)
		{
			object result = null;
			if (Application.s_NestedBamlLoadInfo == null)
			{
				Application.s_NestedBamlLoadInfo = new Stack<NestedBamlLoadInfo>();
			}
			NestedBamlLoadInfo item = new NestedBamlLoadInfo(pc.BaseUri, stream, pc.SkipJournaledProperties);
			Application.s_NestedBamlLoadInfo.Push(item);
			try
			{
				result = XamlReader.LoadBaml(stream, pc, null, true);
			}
			finally
			{
				Application.s_NestedBamlLoadInfo.Pop();
				if (Application.s_NestedBamlLoadInfo.Count == 0)
				{
					Application.s_NestedBamlLoadInfo = null;
				}
			}
			return result;
		}

		/// <summary>Returns a resource stream for a resource data file that is located at the specified <see cref="T:System.Uri" /> (see WPF Application Resource, Content, and Data Files).</summary>
		/// <param name="uriResource">The <see cref="T:System.Uri" /> that maps to an embedded resource.</param>
		/// <returns>A <see cref="T:System.Windows.Resources.StreamResourceInfo" /> that contains a resource stream for resource data file that is located at the specified <see cref="T:System.Uri" />. </returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetResourceStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Uri.OriginalString" /> property of the <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetResourceStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetResourceStream(System.Uri)" /> is either not relative, or is absolute but not in the pack://application:,,,/ form.</exception>
		/// <exception cref="T:System.IO.IOException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetResourceStream(System.Uri)" /> cannot be found.</exception>
		// Token: 0x06000285 RID: 645 RVA: 0x000068A0 File Offset: 0x00004AA0
		[SecurityCritical]
		public static StreamResourceInfo GetResourceStream(Uri uriResource)
		{
			if (uriResource == null)
			{
				throw new ArgumentNullException("uriResource");
			}
			if (uriResource.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriResource",
					"OriginalString"
				}));
			}
			if (uriResource.IsAbsoluteUri && !BaseUriHelper.IsPackApplicationUri(uriResource))
			{
				throw new ArgumentException(SR.Get("NonPackAppAbsoluteUriNotAllowed"));
			}
			ResourcePart resourcePart = Application.GetResourceOrContentPart(uriResource) as ResourcePart;
			if (resourcePart != null)
			{
				return new StreamResourceInfo(resourcePart.GetStream(), resourcePart.ContentType);
			}
			return null;
		}

		/// <summary>Returns a resource stream for a content data file that is located at the specified <see cref="T:System.Uri" /> (see WPF Application Resource, Content, and Data Files).</summary>
		/// <param name="uriContent">The relative <see cref="T:System.Uri" /> that maps to a loose resource.</param>
		/// <returns>A <see cref="T:System.Windows.Resources.StreamResourceInfo" /> that contains a content data file that is located at the specified <see cref="T:System.Uri" />. If a loose resource is not found, null is returned.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetContentStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Uri.OriginalString" /> property of the <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetContentStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetContentStream(System.Uri)" /> is an absolute <see cref="T:System.Uri" />.</exception>
		// Token: 0x06000286 RID: 646 RVA: 0x00006934 File Offset: 0x00004B34
		[SecurityCritical]
		public static StreamResourceInfo GetContentStream(Uri uriContent)
		{
			if (uriContent == null)
			{
				throw new ArgumentNullException("uriContent");
			}
			if (uriContent.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriContent",
					"OriginalString"
				}));
			}
			if (uriContent.IsAbsoluteUri && !BaseUriHelper.IsPackApplicationUri(uriContent))
			{
				throw new ArgumentException(SR.Get("NonPackAppAbsoluteUriNotAllowed"));
			}
			ContentFilePart contentFilePart = Application.GetResourceOrContentPart(uriContent) as ContentFilePart;
			if (contentFilePart != null)
			{
				return new StreamResourceInfo(contentFilePart.GetStream(), contentFilePart.ContentType);
			}
			return null;
		}

		/// <summary>Returns a resource stream for a site-of-origin data file that is located at the specified <see cref="T:System.Uri" /> (see WPF Application Resource, Content, and Data Files).</summary>
		/// <param name="uriRemote">The <see cref="T:System.Uri" /> that maps to a loose resource at the site of origin.</param>
		/// <returns>A <see cref="T:System.Windows.Resources.StreamResourceInfo" /> that contains a resource stream for a site-of-origin data file that is located at the specified <see cref="T:System.Uri" />. If the loose resource is not found, <see langword="null" /> is returned.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetRemoteStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Uri.OriginalString" /> property of the <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetRemoteStream(System.Uri)" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Uri" /> that is passed to <see cref="M:System.Windows.Application.GetRemoteStream(System.Uri)" /> is either not relative, or is absolute but not in the pack://siteoforigin:,,,/ form.</exception>
		// Token: 0x06000287 RID: 647 RVA: 0x000069C8 File Offset: 0x00004BC8
		[SecurityCritical]
		public static StreamResourceInfo GetRemoteStream(Uri uriRemote)
		{
			SiteOfOriginPart siteOfOriginPart = null;
			if (uriRemote == null)
			{
				throw new ArgumentNullException("uriRemote");
			}
			if (uriRemote.OriginalString == null)
			{
				throw new ArgumentException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"uriRemote",
					"OriginalString"
				}));
			}
			if (uriRemote.IsAbsoluteUri && !BaseUriHelper.SiteOfOriginBaseUri.IsBaseOf(uriRemote))
			{
				throw new ArgumentException(SR.Get("NonPackSooAbsoluteUriNotAllowed"));
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(BaseUriHelper.SiteOfOriginBaseUri, uriRemote);
			Uri packageUri;
			Uri partUri;
			PackUriHelper.ValidateAndGetPackUriComponents(resolvedUri, out packageUri, out partUri);
			SiteOfOriginContainer siteOfOriginContainer = (SiteOfOriginContainer)Application.GetResourcePackage(packageUri);
			SiteOfOriginContainer obj = siteOfOriginContainer;
			lock (obj)
			{
				siteOfOriginPart = (siteOfOriginContainer.GetPart(partUri) as SiteOfOriginPart);
			}
			Stream stream = null;
			if (siteOfOriginPart != null)
			{
				try
				{
					stream = siteOfOriginPart.GetStream();
					if (stream == null)
					{
						siteOfOriginPart = null;
					}
				}
				catch (WebException)
				{
					siteOfOriginPart = null;
				}
			}
			if (stream != null)
			{
				return new StreamResourceInfo(stream, siteOfOriginPart.ContentType);
			}
			return null;
		}

		/// <summary>Retrieves a cookie for the location specified by a <see cref="T:System.Uri" />.</summary>
		/// <param name="uri">The <see cref="T:System.Uri" /> that specifies the location for which a cookie was created.</param>
		/// <returns>A <see cref="T:System.String" /> value, if the cookie exists; otherwise, a <see cref="T:System.ComponentModel.Win32Exception" /> is thrown.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error is raised by the <see langword="InternetGetCookie" /> function (called by <see cref="M:System.Windows.Application.GetCookie(System.Uri)" />) if a problem occurs when attempting to retrieve the specified cookie.</exception>
		// Token: 0x06000288 RID: 648 RVA: 0x00006AD8 File Offset: 0x00004CD8
		public static string GetCookie(Uri uri)
		{
			return CookieHandler.GetCookie(uri, true);
		}

		/// <summary>Creates a cookie for the location specified by a <see cref="T:System.Uri" />.</summary>
		/// <param name="uri">The <see cref="T:System.Uri" /> that specifies the location for which the cookie should be created.</param>
		/// <param name="value">The <see cref="T:System.String" /> that contains the cookie data.</param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">A Win32 error is raised by the <see langword="InternetSetCookie" /> function (called by <see cref="M:System.Windows.Application.SetCookie(System.Uri,System.String)" />) if a problem occurs when attempting to create the specified cookie.</exception>
		// Token: 0x06000289 RID: 649 RVA: 0x00006AE1 File Offset: 0x00004CE1
		public static void SetCookie(Uri uri, string value)
		{
			CookieHandler.SetCookie(uri, value);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Application" /> object for the current <see cref="T:System.AppDomain" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Application" /> object for the current <see cref="T:System.AppDomain" />.</returns>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00006AEB File Offset: 0x00004CEB
		public static Application Current
		{
			get
			{
				return Application._appInstance;
			}
		}

		/// <summary>Gets the instantiated windows in an application. </summary>
		/// <returns>A <see cref="T:System.Windows.WindowCollection" /> that contains references to all window objects in the current <see cref="T:System.AppDomain" />.</returns>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00006AF2 File Offset: 0x00004CF2
		public WindowCollection Windows
		{
			get
			{
				base.VerifyAccess();
				return this.WindowsInternal.Clone();
			}
		}

		/// <summary>Gets or sets the main window of the application.</summary>
		/// <returns>A <see cref="T:System.Windows.Window" /> that is designated as the main application window.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Application.MainWindow" /> is set from an application that's hosted in a browser, such as an XAML browser applications (XBAPs).</exception>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00006B05 File Offset: 0x00004D05
		// (set) Token: 0x0600028D RID: 653 RVA: 0x00006B14 File Offset: 0x00004D14
		public Window MainWindow
		{
			get
			{
				base.VerifyAccess();
				return this._mainWindow;
			}
			set
			{
				base.VerifyAccess();
				if (this._mainWindow is RootBrowserWindow || (this.BrowserCallbackServices != null && this._mainWindow == null && !(value is RootBrowserWindow)))
				{
					throw new InvalidOperationException(SR.Get("CannotChangeMainWindowInBrowser"));
				}
				if (value != this._mainWindow)
				{
					this._mainWindow = value;
				}
			}
		}

		/// <summary>Gets or sets the condition that causes the <see cref="M:System.Windows.Application.Shutdown" /> method to be called.</summary>
		/// <returns>A <see cref="T:System.Windows.ShutdownMode" /> enumeration value. The default value is <see cref="F:System.Windows.ShutdownMode.OnLastWindowClose" />.</returns>
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00006B6C File Offset: 0x00004D6C
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00006B7C File Offset: 0x00004D7C
		public ShutdownMode ShutdownMode
		{
			get
			{
				base.VerifyAccess();
				return this._shutdownMode;
			}
			set
			{
				base.VerifyAccess();
				if (!Application.IsValidShutdownMode(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ShutdownMode));
				}
				if (Application.IsShuttingDown || this._appIsShutdown)
				{
					throw new InvalidOperationException(SR.Get("ShutdownModeWhenAppShutdown"));
				}
				this._shutdownMode = value;
			}
		}

		/// <summary>Gets or sets a collection of application-scope resources, such as styles and brushes.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceDictionary" /> object that contains zero or more application-scope resources.</returns>
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00006BD4 File Offset: 0x00004DD4
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00006C38 File Offset: 0x00004E38
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				bool flag = false;
				object globalLock = Application._globalLock;
				ResourceDictionary resources;
				lock (globalLock)
				{
					if (this._resources == null)
					{
						this._resources = new ResourceDictionary();
						flag = true;
					}
					resources = this._resources;
				}
				if (flag)
				{
					resources.AddOwner(this);
				}
				return resources;
			}
			set
			{
				bool flag = false;
				object globalLock = Application._globalLock;
				ResourceDictionary resources;
				lock (globalLock)
				{
					resources = this._resources;
					this._resources = value;
				}
				if (resources != null)
				{
					resources.RemoveOwner(this);
				}
				if (value != null && !value.ContainsOwner(this))
				{
					value.AddOwner(this);
				}
				if (resources != value)
				{
					flag = true;
				}
				if (flag)
				{
					this.InvalidateResourceReferences(new ResourcesChangeInfo(resources, value));
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00006CB4 File Offset: 0x00004EB4
		// (set) Token: 0x06000293 RID: 659 RVA: 0x00006CBC File Offset: 0x00004EBC
		ResourceDictionary IHaveResources.Resources
		{
			get
			{
				return this.Resources;
			}
			set
			{
				this.Resources = value;
			}
		}

		/// <summary>Queries for whether a specified ambient property is available in the current scope.</summary>
		/// <param name="propertyName">The name of the requested ambient property.</param>
		/// <returns>
		///     <see langword="true" /> if the requested ambient property is available; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000294 RID: 660 RVA: 0x00006CC5 File Offset: 0x00004EC5
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName == "Resources" && this._resources != null;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00006CDF File Offset: 0x00004EDF
		// (set) Token: 0x06000296 RID: 662 RVA: 0x00006CE7 File Offset: 0x00004EE7
		internal bool HasImplicitStylesInResources
		{
			get
			{
				return this._hasImplicitStylesInResources;
			}
			set
			{
				this._hasImplicitStylesInResources = value;
			}
		}

		/// <summary>Gets or sets a UI that is automatically shown when an application starts.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that refers to the UI that automatically opens when an application starts.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <see cref="P:System.Windows.Application.StartupUri" /> is set with a value of null.</exception>
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00006CF0 File Offset: 0x00004EF0
		// (set) Token: 0x06000298 RID: 664 RVA: 0x00006CF8 File Offset: 0x00004EF8
		public Uri StartupUri
		{
			get
			{
				return this._startupUri;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._startupUri = value;
			}
		}

		/// <summary>Gets a collection of application-scope properties.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> that contains the application-scope properties.</returns>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00006D1C File Offset: 0x00004F1C
		public IDictionary Properties
		{
			get
			{
				object globalLock = Application._globalLock;
				IDictionary htProps;
				lock (globalLock)
				{
					if (this._htProps == null)
					{
						this._htProps = new HybridDictionary(5);
					}
					htProps = this._htProps;
				}
				return htProps;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Reflection.Assembly" /> that provides the pack uniform resource identifiers (URIs) for resources in a WPF application.</summary>
		/// <returns>A reference to the <see cref="T:System.Reflection.Assembly" /> that provides the pack uniform resource identifiers (URIs) for resources in a WPF application.</returns>
		/// <exception cref="T:System.InvalidOperationException">A WPF application has an entry assembly, or <see cref="P:System.Windows.Application.ResourceAssembly" /> has already been set.</exception>
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00006D74 File Offset: 0x00004F74
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00006DCC File Offset: 0x00004FCC
		public static Assembly ResourceAssembly
		{
			get
			{
				if (Application._resourceAssembly == null)
				{
					object globalLock = Application._globalLock;
					lock (globalLock)
					{
						Application._resourceAssembly = Assembly.GetEntryAssembly();
					}
				}
				return Application._resourceAssembly;
			}
			set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					if (Application._resourceAssembly != value)
					{
						if (!(Application._resourceAssembly == null) || !(Assembly.GetEntryAssembly() == null))
						{
							throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
							{
								"ResourceAssembly",
								"Application"
							}));
						}
						Application._resourceAssembly = value;
						BaseUriHelper.ResourceAssembly = value;
					}
				}
			}
		}

		/// <summary>Occurs when the <see cref="M:System.Windows.Application.Run" /> method of the <see cref="T:System.Windows.Application" /> object is called.</summary>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600029C RID: 668 RVA: 0x00006E60 File Offset: 0x00005060
		// (remove) Token: 0x0600029D RID: 669 RVA: 0x00006E79 File Offset: 0x00005079
		public event StartupEventHandler Startup
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_STARTUP, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_STARTUP, value);
			}
		}

		/// <summary>Occurs just before an application shuts down, and cannot be canceled.</summary>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600029E RID: 670 RVA: 0x00006E92 File Offset: 0x00005092
		// (remove) Token: 0x0600029F RID: 671 RVA: 0x00006EAB File Offset: 0x000050AB
		public event ExitEventHandler Exit
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_EXIT, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_EXIT, value);
			}
		}

		/// <summary>Occurs when an application becomes the foreground application.</summary>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002A0 RID: 672 RVA: 0x00006EC4 File Offset: 0x000050C4
		// (remove) Token: 0x060002A1 RID: 673 RVA: 0x00006EFC File Offset: 0x000050FC
		public event EventHandler Activated;

		/// <summary>Occurs when an application stops being the foreground application.</summary>
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002A2 RID: 674 RVA: 0x00006F34 File Offset: 0x00005134
		// (remove) Token: 0x060002A3 RID: 675 RVA: 0x00006F6C File Offset: 0x0000516C
		public event EventHandler Deactivated;

		/// <summary>Occurs when the user ends the Windows session by logging off or shutting down the operating system.</summary>
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060002A4 RID: 676 RVA: 0x00006FA1 File Offset: 0x000051A1
		// (remove) Token: 0x060002A5 RID: 677 RVA: 0x00006FBA File Offset: 0x000051BA
		public event SessionEndingCancelEventHandler SessionEnding
		{
			add
			{
				base.VerifyAccess();
				this.Events.AddHandler(Application.EVENT_SESSIONENDING, value);
			}
			remove
			{
				base.VerifyAccess();
				this.Events.RemoveHandler(Application.EVENT_SESSIONENDING, value);
			}
		}

		/// <summary>Occurs when an exception is thrown by an application but not handled.</summary>
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060002A6 RID: 678 RVA: 0x00006FD4 File Offset: 0x000051D4
		// (remove) Token: 0x060002A7 RID: 679 RVA: 0x00007010 File Offset: 0x00005210
		public event DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException
		{
			add
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this.Dispatcher.UnhandledException += value;
					return null;
				}), null);
			}
			remove
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this.Dispatcher.UnhandledException -= value;
					return null;
				}), null);
			}
		}

		/// <summary>Occurs when a new navigation is requested by a navigator in the application.</summary>
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060002A8 RID: 680 RVA: 0x0000704C File Offset: 0x0000524C
		// (remove) Token: 0x060002A9 RID: 681 RVA: 0x00007084 File Offset: 0x00005284
		public event NavigatingCancelEventHandler Navigating;

		/// <summary>Occurs when the content that is being navigated to by a navigator in the application has been found, although it may not have completed loading.</summary>
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002AA RID: 682 RVA: 0x000070BC File Offset: 0x000052BC
		// (remove) Token: 0x060002AB RID: 683 RVA: 0x000070F4 File Offset: 0x000052F4
		public event NavigatedEventHandler Navigated;

		/// <summary>Occurs periodically during a download that is being managed by a navigator in the application to provide navigation progress information.</summary>
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002AC RID: 684 RVA: 0x0000712C File Offset: 0x0000532C
		// (remove) Token: 0x060002AD RID: 685 RVA: 0x00007164 File Offset: 0x00005364
		public event NavigationProgressEventHandler NavigationProgress;

		/// <summary>Occurs when an error occurs while a navigator in the application is navigating to the requested content.</summary>
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002AE RID: 686 RVA: 0x0000719C File Offset: 0x0000539C
		// (remove) Token: 0x060002AF RID: 687 RVA: 0x000071D4 File Offset: 0x000053D4
		public event NavigationFailedEventHandler NavigationFailed;

		/// <summary>Occurs when content that was navigated to by a navigator in the application has been loaded, parsed, and has begun rendering.</summary>
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002B0 RID: 688 RVA: 0x0000720C File Offset: 0x0000540C
		// (remove) Token: 0x060002B1 RID: 689 RVA: 0x00007244 File Offset: 0x00005444
		public event LoadCompletedEventHandler LoadCompleted;

		/// <summary>Occurs when the <see langword="StopLoading" /> method of a navigator in the application is called, or when a new navigation is requested by a navigator while a current navigation is in progress.</summary>
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002B2 RID: 690 RVA: 0x0000727C File Offset: 0x0000547C
		// (remove) Token: 0x060002B3 RID: 691 RVA: 0x000072B4 File Offset: 0x000054B4
		public event NavigationStoppedEventHandler NavigationStopped;

		/// <summary>Occurs when a navigator in the application begins navigation to a content fragment, Navigation occurs immediately if the desired fragment is in the current content, or after the source XAML content has been loaded if the desired fragment is in different content.</summary>
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002B4 RID: 692 RVA: 0x000072EC File Offset: 0x000054EC
		// (remove) Token: 0x060002B5 RID: 693 RVA: 0x00007324 File Offset: 0x00005524
		public event FragmentNavigationEventHandler FragmentNavigation;

		/// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
		// Token: 0x060002B6 RID: 694 RVA: 0x0000735C File Offset: 0x0000555C
		protected virtual void OnStartup(StartupEventArgs e)
		{
			base.VerifyAccess();
			StartupEventHandler startupEventHandler = (StartupEventHandler)this.Events[Application.EVENT_STARTUP];
			if (startupEventHandler != null)
			{
				startupEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.Exit" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
		// Token: 0x060002B7 RID: 695 RVA: 0x00007390 File Offset: 0x00005590
		protected virtual void OnExit(ExitEventArgs e)
		{
			base.VerifyAccess();
			ExitEventHandler exitEventHandler = (ExitEventHandler)this.Events[Application.EVENT_EXIT];
			if (exitEventHandler != null)
			{
				exitEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.Activated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060002B8 RID: 696 RVA: 0x000073C4 File Offset: 0x000055C4
		protected virtual void OnActivated(EventArgs e)
		{
			base.VerifyAccess();
			if (this.Activated != null)
			{
				this.Activated(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.Deactivated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060002B9 RID: 697 RVA: 0x000073E1 File Offset: 0x000055E1
		protected virtual void OnDeactivated(EventArgs e)
		{
			base.VerifyAccess();
			if (this.Deactivated != null)
			{
				this.Deactivated(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.SessionEnding" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.SessionEndingCancelEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BA RID: 698 RVA: 0x00007400 File Offset: 0x00005600
		protected virtual void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			base.VerifyAccess();
			SessionEndingCancelEventHandler sessionEndingCancelEventHandler = (SessionEndingCancelEventHandler)this.Events[Application.EVENT_SESSIONENDING];
			if (sessionEndingCancelEventHandler != null)
			{
				sessionEndingCancelEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.Navigating" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigatingCancelEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BB RID: 699 RVA: 0x00007434 File Offset: 0x00005634
		protected virtual void OnNavigating(NavigatingCancelEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.Navigated" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BC RID: 700 RVA: 0x00007451 File Offset: 0x00005651
		protected virtual void OnNavigated(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.NavigationProgress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationProgressEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BD RID: 701 RVA: 0x0000746E File Offset: 0x0000566E
		protected virtual void OnNavigationProgress(NavigationProgressEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationProgress != null)
			{
				this.NavigationProgress(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.NavigationFailed" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationFailedEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BE RID: 702 RVA: 0x0000748B File Offset: 0x0000568B
		protected virtual void OnNavigationFailed(NavigationFailedEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationFailed != null)
			{
				this.NavigationFailed(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.LoadCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationEventArgs" /> that contains the event data.</param>
		// Token: 0x060002BF RID: 703 RVA: 0x000074A8 File Offset: 0x000056A8
		protected virtual void OnLoadCompleted(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.LoadCompleted != null)
			{
				this.LoadCompleted(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.NavigationStopped" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.NavigationEventArgs" /> that contains the event data. </param>
		// Token: 0x060002C0 RID: 704 RVA: 0x000074C5 File Offset: 0x000056C5
		protected virtual void OnNavigationStopped(NavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.NavigationStopped != null)
			{
				this.NavigationStopped(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Application.FragmentNavigation" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Navigation.FragmentNavigationEventArgs" /> that contains the event data.</param>
		// Token: 0x060002C1 RID: 705 RVA: 0x000074E2 File Offset: 0x000056E2
		protected virtual void OnFragmentNavigation(FragmentNavigationEventArgs e)
		{
			base.VerifyAccess();
			if (this.FragmentNavigation != null)
			{
				this.FragmentNavigation(this, e);
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00007500 File Offset: 0x00005700
		internal virtual void PerformNavigationStateChangeTasks(bool isNavigationInitiator, bool playNavigatingSound, Application.NavigationStateChange state)
		{
			if (isNavigationInitiator)
			{
				switch (state)
				{
				case Application.NavigationStateChange.Navigating:
					this.ChangeBrowserDownloadState(true);
					if (playNavigatingSound)
					{
						this.PlaySound("Navigating");
						return;
					}
					break;
				case Application.NavigationStateChange.Completed:
					this.PlaySound("ActivatingDocument");
					this.ChangeBrowserDownloadState(false);
					this.UpdateBrowserCommands();
					return;
				case Application.NavigationStateChange.Stopped:
					this.ChangeBrowserDownloadState(false);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000755C File Offset: 0x0000575C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void UpdateBrowserCommands()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_UpdateBrowserCommandsStart);
			IBrowserCallbackServices browserCallbackServices = (IBrowserCallbackServices)this.GetService(typeof(IBrowserCallbackServices));
			if (browserCallbackServices != null)
			{
				browserCallbackServices.UpdateCommands();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_UpdateBrowserCommandsEnd);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000075AC File Offset: 0x000057AC
		internal void DoStartup()
		{
			if (this.StartupUri != null)
			{
				if (!this.StartupUri.IsAbsoluteUri)
				{
					this.StartupUri = new Uri(this.ApplicationMarkupBaseUri, this.StartupUri);
				}
				if (BaseUriHelper.IsPackApplicationUri(this.StartupUri))
				{
					Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(this.StartupUri);
					NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(uriRelativeToPackAppBase, null, null, null, NavigationMode.New, null, null, true);
					this.FireNavigating(navigatingCancelEventArgs, true);
					if (!navigatingCancelEventArgs.Cancel)
					{
						object root = Application.LoadComponent(this.StartupUri, false);
						this.ConfigAppWindowAndRootElement(root, this.StartupUri);
						return;
					}
				}
				else
				{
					this.NavService = new NavigationService(null);
					this.NavService.AllowWindowNavigation = true;
					this.NavService.PreBPReady += this.OnPreBPReady;
					this.NavService.Navigate(this.StartupUri);
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00007684 File Offset: 0x00005884
		[SecurityCritical]
		internal virtual void DoShutdown()
		{
			while (this.WindowsInternal.Count > 0)
			{
				if (!this.WindowsInternal[0].IsDisposed)
				{
					this.WindowsInternal[0].InternalClose(true, true);
				}
				else
				{
					this.WindowsInternal.RemoveAt(0);
				}
			}
			this.WindowsInternal = null;
			ExitEventArgs exitEventArgs = new ExitEventArgs(this._exitCode);
			try
			{
				this.OnExit(exitEventArgs);
			}
			finally
			{
				this.SetExitCode(exitEventArgs._exitCode);
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					Application._appInstance = null;
				}
				this._mainWindow = null;
				this._htProps = null;
				this.NonAppWindowsInternal = null;
				if (this._parkingHwnd != null)
				{
					this._parkingHwnd.Dispose();
				}
				if (this._events != null)
				{
					this._events.Dispose();
				}
				PreloadedPackages.Clear();
				AppSecurityManager.ClearSecurityManager();
				this._appIsShutdown = true;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00007788 File Offset: 0x00005988
		[SecurityCritical]
		internal int RunInternal(Window window)
		{
			base.VerifyAccess();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Event.WClientAppRun);
			if (this._appIsShutdown)
			{
				throw new InvalidOperationException(SR.Get("CannotCallRunMultipleTimes", new object[]
				{
					base.GetType().FullName
				}));
			}
			if (window != null)
			{
				if (!window.CheckAccess())
				{
					throw new ArgumentException(SR.Get("WindowPassedShouldBeOnApplicationThread", new object[]
					{
						window.GetType().FullName,
						base.GetType().FullName
					}));
				}
				if (!this.WindowsInternal.HasItem(window))
				{
					this.WindowsInternal.Add(window);
				}
				if (this.MainWindow == null)
				{
					this.MainWindow = window;
				}
				if (window.Visibility != Visibility.Visible)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object obj)
					{
						Window window2 = obj as Window;
						window2.Show();
						return null;
					}), window);
				}
			}
			this.EnsureHwndSource();
			if (!BrowserInteropHelper.IsBrowserHosted)
			{
				this.RunDispatcher(null);
			}
			return this._exitCode;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00007889 File Offset: 0x00005A89
		internal void InvalidateResourceReferences(ResourcesChangeInfo info)
		{
			this.InvalidateResourceReferenceOnWindowCollection(this.WindowsInternal.Clone(), info);
			this.InvalidateResourceReferenceOnWindowCollection(this.NonAppWindowsInternal.Clone(), info);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000078B0 File Offset: 0x00005AB0
		[SecurityCritical]
		internal NavigationWindow GetAppWindow()
		{
			NavigationWindow navigationWindow;
			if ((IBrowserCallbackServices)this.GetService(typeof(IBrowserCallbackServices)) == null)
			{
				navigationWindow = new NavigationWindow();
				new WindowInteropHelper(navigationWindow).EnsureHandle();
			}
			else
			{
				IHostService hostService = (IHostService)this.GetService(typeof(IHostService));
				navigationWindow = hostService.RootBrowserWindowProxy.RootBrowserWindow;
			}
			return navigationWindow;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000790F File Offset: 0x00005B0F
		internal void FireNavigating(NavigatingCancelEventArgs e, bool isInitialNavigation)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, !isInitialNavigation, Application.NavigationStateChange.Navigating);
			this.OnNavigating(e);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00007929 File Offset: 0x00005B29
		internal void FireNavigated(NavigationEventArgs e)
		{
			this.OnNavigated(e);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00007932 File Offset: 0x00005B32
		internal void FireNavigationProgress(NavigationProgressEventArgs e)
		{
			this.OnNavigationProgress(e);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000793B File Offset: 0x00005B3B
		internal void FireNavigationFailed(NavigationFailedEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(true, false, Application.NavigationStateChange.Stopped);
			this.OnNavigationFailed(e);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000794D File Offset: 0x00005B4D
		internal void FireLoadCompleted(NavigationEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, false, Application.NavigationStateChange.Completed);
			this.OnLoadCompleted(e);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00007964 File Offset: 0x00005B64
		internal void FireNavigationStopped(NavigationEventArgs e)
		{
			this.PerformNavigationStateChangeTasks(e.IsNavigationInitiator, false, Application.NavigationStateChange.Stopped);
			this.OnNavigationStopped(e);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000797B File Offset: 0x00005B7B
		internal void FireFragmentNavigation(FragmentNavigationEventArgs e)
		{
			this.OnFragmentNavigation(e);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00007984 File Offset: 0x00005B84
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x000079D8 File Offset: 0x00005BD8
		internal WindowCollection WindowsInternal
		{
			get
			{
				object globalLock = Application._globalLock;
				WindowCollection appWindowList;
				lock (globalLock)
				{
					if (this._appWindowList == null)
					{
						this._appWindowList = new WindowCollection();
					}
					appWindowList = this._appWindowList;
				}
				return appWindowList;
			}
			private set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					this._appWindowList = value;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00007A18 File Offset: 0x00005C18
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00007A6C File Offset: 0x00005C6C
		internal WindowCollection NonAppWindowsInternal
		{
			get
			{
				object globalLock = Application._globalLock;
				WindowCollection nonAppWindowList;
				lock (globalLock)
				{
					if (this._nonAppWindowList == null)
					{
						this._nonAppWindowList = new WindowCollection();
					}
					nonAppWindowList = this._nonAppWindowList;
				}
				return nonAppWindowList;
			}
			private set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					this._nonAppWindowList = value;
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00007AAC File Offset: 0x00005CAC
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00007AB9 File Offset: 0x00005CB9
		internal MimeType MimeType
		{
			get
			{
				return this._appMimeType.Value;
			}
			[SecurityCritical]
			set
			{
				this._appMimeType = new SecurityCriticalDataForSet<MimeType>(value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00007AC7 File Offset: 0x00005CC7
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00007AE0 File Offset: 0x00005CE0
		internal IServiceProvider ServiceProvider
		{
			private get
			{
				base.VerifyAccess();
				if (this._serviceProvider != null)
				{
					return this._serviceProvider;
				}
				return null;
			}
			set
			{
				base.VerifyAccess();
				this._serviceProvider = value;
				if (value != null)
				{
					this._browserCallbackServices = (IBrowserCallbackServices)this._serviceProvider.GetService(typeof(IBrowserCallbackServices));
					ILease lease = RemotingServices.GetLifetimeService(this._browserCallbackServices as MarshalByRefObject) as ILease;
					if (lease != null)
					{
						this._browserCallbackSponsor = new SponsorHelper(lease, new TimeSpan(0, 5, 0));
						this._browserCallbackSponsor.Register();
						return;
					}
				}
				else
				{
					this.CleanUpBrowserCallBackServices();
				}
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00007B5C File Offset: 0x00005D5C
		private void CleanUpBrowserCallBackServices()
		{
			if (this._browserCallbackServices != null)
			{
				if (this._browserCallbackSponsor != null)
				{
					this._browserCallbackSponsor.Unregister();
					this._browserCallbackSponsor = null;
				}
				this._browserCallbackServices = null;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00007B87 File Offset: 0x00005D87
		internal IBrowserCallbackServices BrowserCallbackServices
		{
			get
			{
				base.VerifyAccess();
				return this._browserCallbackServices;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00007B95 File Offset: 0x00005D95
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00007BA3 File Offset: 0x00005DA3
		internal NavigationService NavService
		{
			get
			{
				base.VerifyAccess();
				return this._navService;
			}
			set
			{
				base.VerifyAccess();
				this._navService = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00007BB4 File Offset: 0x00005DB4
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00007BFC File Offset: 0x00005DFC
		internal static bool IsShuttingDown
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (Application._isShuttingDown)
				{
					return Application._isShuttingDown;
				}
				if (BrowserInteropHelper.IsBrowserHosted)
				{
					Application application = Application.Current;
					if (application != null && application.CheckAccess())
					{
						IBrowserCallbackServices browserCallbackServices = application.BrowserCallbackServices;
						return browserCallbackServices != null && browserCallbackServices.IsShuttingDown();
					}
				}
				return false;
			}
			set
			{
				object globalLock = Application._globalLock;
				lock (globalLock)
				{
					Application._isShuttingDown = value;
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00007C3C File Offset: 0x00005E3C
		internal static bool IsApplicationObjectShuttingDown
		{
			get
			{
				return Application._isShuttingDown;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00007C43 File Offset: 0x00005E43
		internal IntPtr ParkingHwnd
		{
			[SecurityCritical]
			get
			{
				if (this._parkingHwnd != null)
				{
					return this._parkingHwnd.Handle;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00007C5E File Offset: 0x00005E5E
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x00007C7F File Offset: 0x00005E7F
		internal Uri ApplicationMarkupBaseUri
		{
			get
			{
				if (this._applicationMarkupBaseUri == null)
				{
					this._applicationMarkupBaseUri = BaseUriHelper.BaseUri;
				}
				return this._applicationMarkupBaseUri;
			}
			set
			{
				this._applicationMarkupBaseUri = value;
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00007C88 File Offset: 0x00005E88
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void ApplicationInit()
		{
			Application._globalLock = new object();
			PreloadedPackages.AddPackage(PackUriHelper.GetPackageUri(BaseUriHelper.PackAppBaseUri), new ResourceContainer(), true);
			MimeObjectFactory.Register(MimeTypeMapper.BamlMime, new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.BamlConverter));
			StreamToObjectFactoryDelegate method = new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.XamlConverter);
			MimeObjectFactory.Register(MimeTypeMapper.XamlMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedDocumentMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedDocumentSequenceMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.FixedPageMime, method);
			MimeObjectFactory.Register(MimeTypeMapper.ResourceDictionaryMime, method);
			StreamToObjectFactoryDelegate method2 = new StreamToObjectFactoryDelegate(AppModelKnownContentFactory.HtmlXappConverter);
			MimeObjectFactory.Register(MimeTypeMapper.HtmMime, method2);
			MimeObjectFactory.Register(MimeTypeMapper.HtmlMime, method2);
			MimeObjectFactory.Register(MimeTypeMapper.XbapMime, method2);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00007D3C File Offset: 0x00005F3C
		[SecurityCritical]
		private static PackagePart GetResourceOrContentPart(Uri uri)
		{
			Uri packAppBaseUri = BaseUriHelper.PackAppBaseUri;
			Uri resolvedUri = BindUriHelper.GetResolvedUri(packAppBaseUri, uri);
			Uri packageUri;
			Uri partUri;
			PackUriHelper.ValidateAndGetPackUriComponents(resolvedUri, out packageUri, out partUri);
			ResourceContainer resourceContainer = (ResourceContainer)Application.GetResourcePackage(packageUri);
			PackagePart result = null;
			ResourceContainer obj = resourceContainer;
			lock (obj)
			{
				result = resourceContainer.GetPart(partUri);
			}
			return result;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00007DAC File Offset: 0x00005FAC
		[SecurityCritical]
		private static Package GetResourcePackage(Uri packageUri)
		{
			Package package = PreloadedPackages.GetPackage(packageUri);
			if (package == null)
			{
				Uri uri = PackUriHelper.Create(packageUri);
				Invariant.Assert(uri == BaseUriHelper.PackAppBaseUri || uri == BaseUriHelper.SiteOfOriginBaseUri, "Unknown packageUri passed: " + packageUri);
				Invariant.Assert(Application.IsApplicationObjectShuttingDown);
				throw new InvalidOperationException(SR.Get("ApplicationShuttingDown"));
			}
			return package;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00007E10 File Offset: 0x00006010
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EnsureHwndSource()
		{
			if (this.BrowserCallbackServices == null && this._parkingHwnd == null)
			{
				this._appFilterHook = new HwndWrapperHook(this.AppFilterMessage);
				HwndWrapperHook[] hooks = new HwndWrapperHook[]
				{
					this._appFilterHook
				};
				this._parkingHwnd = new HwndWrapper(0, 0, 0, 0, 0, 0, 0, "", IntPtr.Zero, hooks);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00007E6C File Offset: 0x0000606C
		private IntPtr AppFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg != 17)
			{
				if (msg == 28)
				{
					handled = this.WmActivateApp(NativeMethods.IntPtrToInt32(wParam));
				}
				else
				{
					handled = false;
				}
			}
			else
			{
				handled = this.WmQueryEndSession(lParam, ref zero);
			}
			return zero;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00007EB0 File Offset: 0x000060B0
		private bool WmActivateApp(int wParam)
		{
			bool flag = wParam != 0;
			if (flag)
			{
				this.OnActivated(EventArgs.Empty);
			}
			else
			{
				this.OnDeactivated(EventArgs.Empty);
			}
			return false;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00007EE4 File Offset: 0x000060E4
		[SecuritySafeCritical]
		private bool WmQueryEndSession(IntPtr lParam, ref IntPtr refInt)
		{
			int num = NativeMethods.IntPtrToInt32(lParam);
			SessionEndingCancelEventArgs sessionEndingCancelEventArgs = new SessionEndingCancelEventArgs(((num & int.MinValue) != 0) ? ReasonSessionEnding.Logoff : ReasonSessionEnding.Shutdown);
			this.OnSessionEnding(sessionEndingCancelEventArgs);
			bool result;
			if (!sessionEndingCancelEventArgs.Cancel)
			{
				this.Shutdown();
				refInt = new IntPtr(1);
				result = false;
			}
			else
			{
				SecurityHelper.DemandUnmanagedCode();
				refInt = IntPtr.Zero;
				result = true;
			}
			return result;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00007F40 File Offset: 0x00006140
		private void InvalidateResourceReferenceOnWindowCollection(WindowCollection wc, ResourcesChangeInfo info)
		{
			bool hasImplicitStyles = info.IsResourceAddOperation && this.HasImplicitStylesInResources;
			DispatcherOperationCallback <>9__0;
			for (int i = 0; i < wc.Count; i++)
			{
				if (wc[i].CheckAccess())
				{
					if (hasImplicitStyles)
					{
						wc[i].ShouldLookupImplicitStyles = true;
					}
					TreeWalkHelper.InvalidateOnResourcesChange(wc[i], null, info);
				}
				else
				{
					Dispatcher dispatcher = wc[i].Dispatcher;
					DispatcherPriority priority = DispatcherPriority.Send;
					DispatcherOperationCallback method;
					if ((method = <>9__0) == null)
					{
						method = (<>9__0 = delegate(object obj)
						{
							object[] array = obj as object[];
							if (hasImplicitStyles)
							{
								((FrameworkElement)array[0]).ShouldLookupImplicitStyles = true;
							}
							TreeWalkHelper.InvalidateOnResourcesChange((FrameworkElement)array[0], null, (ResourcesChangeInfo)array[1]);
							return null;
						});
					}
					dispatcher.BeginInvoke(priority, method, new object[]
					{
						wc[i],
						info
					});
				}
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00008001 File Offset: 0x00006201
		private void SetExitCode(int exitCode)
		{
			if (this._exitCode != exitCode)
			{
				this._exitCode = exitCode;
				Environment.ExitCode = exitCode;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00008019 File Offset: 0x00006219
		[SecurityCritical]
		private object ShutdownCallback(object arg)
		{
			this.ShutdownImpl();
			return null;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00008024 File Offset: 0x00006224
		[SecurityCritical]
		private void ShutdownImpl()
		{
			try
			{
				this.DoShutdown();
			}
			finally
			{
				if (this._ownDispatcherStarted)
				{
					base.Dispatcher.CriticalInvokeShutdown();
				}
				this.ServiceProvider = null;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00008064 File Offset: 0x00006264
		private static bool IsValidShutdownMode(ShutdownMode value)
		{
			return value == ShutdownMode.OnExplicitShutdown || value == ShutdownMode.OnLastWindowClose || value == ShutdownMode.OnMainWindowClose;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00008074 File Offset: 0x00006274
		private void OnPreBPReady(object sender, BPReadyEventArgs e)
		{
			this.NavService.PreBPReady -= this.OnPreBPReady;
			this.NavService.AllowWindowNavigation = false;
			this.ConfigAppWindowAndRootElement(e.Content, e.Uri);
			this.NavService = null;
			e.Cancel = true;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000080C4 File Offset: 0x000062C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ConfigAppWindowAndRootElement(object root, Uri uri)
		{
			Window window2 = root as Window;
			if (window2 == null)
			{
				NavigationWindow appWindow = this.GetAppWindow();
				appWindow.Navigate(root, new NavigateInfo(uri));
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new SendOrPostCallback(delegate(object window)
				{
					if (!((Window)window).IsDisposed)
					{
						((Window)window).Show();
					}
				}), appWindow);
				return;
			}
			if (!window2.IsVisibilitySet && !window2.IsDisposed)
			{
				window2.Visibility = Visibility.Visible;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00008138 File Offset: 0x00006338
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ChangeBrowserDownloadState(bool newState)
		{
			IBrowserCallbackServices browserCallbackServices = (IBrowserCallbackServices)this.GetService(typeof(IBrowserCallbackServices));
			if (browserCallbackServices != null)
			{
				browserCallbackServices.ChangeDownloadState(newState);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00008168 File Offset: 0x00006368
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void PlaySound(string soundName)
		{
			string systemSound = this.GetSystemSound(soundName);
			if (!string.IsNullOrEmpty(systemSound))
			{
				UnsafeNativeMethods.PlaySound(systemSound, IntPtr.Zero, SafeNativeMethods.PlaySoundFlags.SND_ASYNC | SafeNativeMethods.PlaySoundFlags.SND_NODEFAULT | SafeNativeMethods.PlaySoundFlags.SND_NOSTOP | SafeNativeMethods.PlaySoundFlags.SND_FILENAME);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00008198 File Offset: 0x00006398
		[SecurityCritical]
		private string GetSystemSound(string soundName)
		{
			string result = null;
			string name = string.Format(CultureInfo.InvariantCulture, "AppEvents\\Schemes\\Apps\\Explorer\\{0}\\.current\\", new object[]
			{
				soundName
			});
			PermissionSet permissionSet = new PermissionSet(null);
			permissionSet.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_CURRENT_USER\\AppEvents\\Schemes\\Apps\\Explorer\\"));
			permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
			permissionSet.Assert();
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name))
				{
					if (registryKey != null)
					{
						result = (string)registryKey.GetValue("");
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00008250 File Offset: 0x00006450
		private EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000826C File Offset: 0x0000646C
		private static bool IsComponentBeingLoadedFromOuterLoadBaml(Uri curComponentUri)
		{
			bool result = false;
			Invariant.Assert(curComponentUri != null, "curComponentUri should not be null");
			if (Application.s_NestedBamlLoadInfo != null && Application.s_NestedBamlLoadInfo.Count > 0)
			{
				NestedBamlLoadInfo nestedBamlLoadInfo = Application.s_NestedBamlLoadInfo.Peek();
				if (nestedBamlLoadInfo != null && nestedBamlLoadInfo.BamlUri != null && nestedBamlLoadInfo.BamlStream != null && BindUriHelper.DoSchemeAndHostMatch(nestedBamlLoadInfo.BamlUri, curComponentUri))
				{
					string localPath = nestedBamlLoadInfo.BamlUri.LocalPath;
					string localPath2 = curComponentUri.LocalPath;
					Invariant.Assert(localPath != null, "fileInBamlConvert should not be null");
					Invariant.Assert(localPath2 != null, "fileCurrent should not be null");
					if (string.Compare(localPath, localPath2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						result = true;
					}
					else
					{
						string[] array = localPath.Split(new char[]
						{
							'/',
							'\\'
						});
						string[] array2 = localPath2.Split(new char[]
						{
							'/',
							'\\'
						});
						int num = array.Length;
						int num2 = array2.Length;
						Invariant.Assert(num >= 2 && num2 >= 2);
						int num3 = num - num2;
						if (Math.Abs(num3) == 1 && string.Compare(array[num - 1], array2[num2 - 1], StringComparison.OrdinalIgnoreCase) == 0)
						{
							string component = (num3 == 1) ? array[1] : array2[1];
							result = BaseUriHelper.IsComponentEntryAssembly(component);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000083B8 File Offset: 0x000065B8
		[SecurityCritical]
		[DebuggerNonUserCode]
		private object StartDispatcherInBrowser(object unused)
		{
			if (BrowserInteropHelper.IsBrowserHosted)
			{
				BrowserInteropHelper.InitializeHostFilterInput();
				try
				{
					this.RunDispatcher(null);
				}
				catch
				{
					throw;
				}
			}
			return null;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000083F0 File Offset: 0x000065F0
		[SecurityCritical]
		private object RunDispatcher(object ignore)
		{
			if (this._ownDispatcherStarted)
			{
				throw new InvalidOperationException(SR.Get("ApplicationAlreadyRunning"));
			}
			this._ownDispatcherStarted = true;
			Dispatcher.Run();
			return null;
		}

		// Token: 0x0400059B RID: 1435
		private static object _globalLock;

		// Token: 0x0400059C RID: 1436
		private static bool _isShuttingDown;

		// Token: 0x0400059D RID: 1437
		private static bool _appCreatedInThisAppDomain;

		// Token: 0x0400059E RID: 1438
		private static Application _appInstance;

		// Token: 0x0400059F RID: 1439
		private static Assembly _resourceAssembly;

		// Token: 0x040005A0 RID: 1440
		[ThreadStatic]
		private static Stack<NestedBamlLoadInfo> s_NestedBamlLoadInfo = null;

		// Token: 0x040005A1 RID: 1441
		private Uri _startupUri;

		// Token: 0x040005A2 RID: 1442
		private Uri _applicationMarkupBaseUri;

		// Token: 0x040005A3 RID: 1443
		private HybridDictionary _htProps;

		// Token: 0x040005A4 RID: 1444
		private WindowCollection _appWindowList;

		// Token: 0x040005A5 RID: 1445
		private WindowCollection _nonAppWindowList;

		// Token: 0x040005A6 RID: 1446
		private Window _mainWindow;

		// Token: 0x040005A7 RID: 1447
		private ResourceDictionary _resources;

		// Token: 0x040005A8 RID: 1448
		private bool _ownDispatcherStarted;

		// Token: 0x040005A9 RID: 1449
		private NavigationService _navService;

		// Token: 0x040005AA RID: 1450
		private SecurityCriticalDataForSet<MimeType> _appMimeType;

		// Token: 0x040005AB RID: 1451
		private IServiceProvider _serviceProvider;

		// Token: 0x040005AC RID: 1452
		private IBrowserCallbackServices _browserCallbackServices;

		// Token: 0x040005AD RID: 1453
		private SponsorHelper _browserCallbackSponsor;

		// Token: 0x040005AE RID: 1454
		private bool _appIsShutdown;

		// Token: 0x040005AF RID: 1455
		private int _exitCode;

		// Token: 0x040005B0 RID: 1456
		private ShutdownMode _shutdownMode;

		// Token: 0x040005B1 RID: 1457
		[SecurityCritical]
		private HwndWrapper _parkingHwnd;

		// Token: 0x040005B2 RID: 1458
		[SecurityCritical]
		private HwndWrapperHook _appFilterHook;

		// Token: 0x040005B3 RID: 1459
		private EventHandlerList _events;

		// Token: 0x040005B4 RID: 1460
		private bool _hasImplicitStylesInResources;

		// Token: 0x040005B5 RID: 1461
		private static readonly object EVENT_STARTUP = new object();

		// Token: 0x040005B6 RID: 1462
		private static readonly object EVENT_EXIT = new object();

		// Token: 0x040005B7 RID: 1463
		private static readonly object EVENT_SESSIONENDING = new object();

		// Token: 0x040005B8 RID: 1464
		private const SafeNativeMethods.PlaySoundFlags PLAYSOUND_FLAGS = SafeNativeMethods.PlaySoundFlags.SND_ASYNC | SafeNativeMethods.PlaySoundFlags.SND_NODEFAULT | SafeNativeMethods.PlaySoundFlags.SND_NOSTOP | SafeNativeMethods.PlaySoundFlags.SND_FILENAME;

		// Token: 0x040005B9 RID: 1465
		private const string SYSTEM_SOUNDS_REGISTRY_LOCATION = "AppEvents\\Schemes\\Apps\\Explorer\\{0}\\.current\\";

		// Token: 0x040005BA RID: 1466
		private const string SYSTEM_SOUNDS_REGISTRY_BASE = "HKEY_CURRENT_USER\\AppEvents\\Schemes\\Apps\\Explorer\\";

		// Token: 0x040005BB RID: 1467
		private const string SOUND_NAVIGATING = "Navigating";

		// Token: 0x040005BC RID: 1468
		private const string SOUND_COMPLETE_NAVIGATION = "ActivatingDocument";

		// Token: 0x0200080D RID: 2061
		internal enum NavigationStateChange : byte
		{
			// Token: 0x04003B9A RID: 15258
			Navigating,
			// Token: 0x04003B9B RID: 15259
			Completed,
			// Token: 0x04003B9C RID: 15260
			Stopped
		}
	}
}
