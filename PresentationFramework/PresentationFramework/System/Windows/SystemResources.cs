using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Baml2006;
using System.Windows.Controls.Primitives;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xaml;
using System.Xaml.Permissions;
using MS.Internal;
using MS.Internal.Ink;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Internal.WindowsBase;
using MS.Utility;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x02000115 RID: 277
	internal static class SystemResources
	{
		// Token: 0x06000B77 RID: 2935 RVA: 0x00029CD8 File Offset: 0x00027ED8
		internal static object FindThemeStyle(DependencyObjectType key)
		{
			object obj = SystemResources._themeStyleCache[key];
			if (obj == null)
			{
				obj = SystemResources.FindResourceInternal(key.SystemType);
				object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
				lock (themeDictionaryLock)
				{
					if (obj != null)
					{
						SystemResources._themeStyleCache[key] = obj;
					}
					else
					{
						SystemResources._themeStyleCache[key] = SystemResources._specialNull;
					}
				}
				return obj;
			}
			if (obj == SystemResources._specialNull)
			{
				return null;
			}
			return obj;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00029D5C File Offset: 0x00027F5C
		internal static object FindResourceInternal(object key)
		{
			return SystemResources.FindResourceInternal(key, false, false);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00029D68 File Offset: 0x00027F68
		internal static object FindResourceInternal(object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			SystemResources.EnsureResourceChangeListener();
			object obj = null;
			bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
			Type type = key as Type;
			ResourceKey resourceKey = (type == null) ? (key as ResourceKey) : null;
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindBegin, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, (key == null) ? "null" : key.ToString());
			}
			if (type == null && resourceKey == null)
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindEnd, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
				return null;
			}
			if (!SystemResources.FindCachedResource(key, ref obj, mustReturnDeferredResourceReference))
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheMiss, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
				object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
				lock (themeDictionaryLock)
				{
					bool flag3 = true;
					SystemResourceKey systemResourceKey = (resourceKey != null) ? (resourceKey as SystemResourceKey) : null;
					if (systemResourceKey != null)
					{
						if (!mustReturnDeferredResourceReference)
						{
							obj = systemResourceKey.Resource;
						}
						else
						{
							obj = new DeferredResourceReferenceHolder(systemResourceKey, systemResourceKey.Resource);
						}
						if (flag)
						{
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceStock, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, systemResourceKey.ToString());
						}
					}
					else
					{
						obj = SystemResources.FindDictionaryResource(key, type, resourceKey, flag, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag3);
					}
					if ((flag3 && !allowDeferredResourceReference) || obj == null)
					{
						SystemResources.CacheResource(key, obj, flag);
					}
				}
			}
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceFindEnd, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
			}
			return obj;
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x00029ECC File Offset: 0x000280CC
		internal static ReadOnlyCollection<ResourceDictionaryInfo> ThemedResourceDictionaries
		{
			get
			{
				List<ResourceDictionaryInfo> list = new List<ResourceDictionaryInfo>();
				if (SystemResources._dictionaries != null)
				{
					object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
					lock (themeDictionaryLock)
					{
						if (SystemResources._dictionaries != null)
						{
							foreach (KeyValuePair<Assembly, SystemResources.ResourceDictionaries> keyValuePair in SystemResources._dictionaries)
							{
								ResourceDictionaryInfo themedDictionaryInfo = keyValuePair.Value.ThemedDictionaryInfo;
								if (themedDictionaryInfo.ResourceDictionary != null)
								{
									list.Add(themedDictionaryInfo);
								}
							}
						}
					}
				}
				return list.AsReadOnly();
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00029F7C File Offset: 0x0002817C
		internal static ReadOnlyCollection<ResourceDictionaryInfo> GenericResourceDictionaries
		{
			get
			{
				List<ResourceDictionaryInfo> list = new List<ResourceDictionaryInfo>();
				if (SystemResources._dictionaries != null)
				{
					object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
					lock (themeDictionaryLock)
					{
						if (SystemResources._dictionaries != null)
						{
							foreach (KeyValuePair<Assembly, SystemResources.ResourceDictionaries> keyValuePair in SystemResources._dictionaries)
							{
								ResourceDictionaryInfo genericDictionaryInfo = keyValuePair.Value.GenericDictionaryInfo;
								if (genericDictionaryInfo.ResourceDictionary != null)
								{
									list.Add(genericDictionaryInfo);
								}
							}
						}
					}
				}
				return list.AsReadOnly();
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002A02C File Offset: 0x0002822C
		internal static void CacheResource(object key, object resource, bool isTraceEnabled)
		{
			if (resource != null)
			{
				SystemResources._resourceCache[key] = resource;
				if (isTraceEnabled)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheValue, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
					return;
				}
			}
			else
			{
				SystemResources._resourceCache[key] = SystemResources._specialNull;
				if (isTraceEnabled)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceCacheNull, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose);
				}
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002A088 File Offset: 0x00028288
		private static bool FindCachedResource(object key, ref object resource, bool mustReturnDeferredResourceReference)
		{
			resource = SystemResources._resourceCache[key];
			bool flag = resource != null;
			if (resource == SystemResources._specialNull)
			{
				resource = null;
			}
			else
			{
				DispatcherObject dispatcherObject = resource as DispatcherObject;
				if (dispatcherObject != null)
				{
					dispatcherObject.VerifyAccess();
				}
			}
			if (flag && mustReturnDeferredResourceReference)
			{
				resource = new DeferredResourceReferenceHolder(key, resource);
			}
			return flag;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002A0D8 File Offset: 0x000282D8
		private static object FindDictionaryResource(object key, Type typeKey, ResourceKey resourceKey, bool isTraceEnabled, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			canCache = true;
			object obj = null;
			Assembly assembly = (typeKey != null) ? typeKey.Assembly : resourceKey.Assembly;
			if (assembly == null || SystemResources.IgnoreAssembly(assembly))
			{
				return null;
			}
			SystemResources.ResourceDictionaries resourceDictionaries = SystemResources.EnsureDictionarySlot(assembly);
			ResourceDictionary resourceDictionary = resourceDictionaries.LoadThemedDictionary(isTraceEnabled);
			if (resourceDictionary != null)
			{
				obj = SystemResources.LookupResourceInDictionary(resourceDictionary, key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
			}
			if (obj == null)
			{
				resourceDictionary = resourceDictionaries.LoadGenericDictionary(isTraceEnabled);
				if (resourceDictionary != null)
				{
					obj = SystemResources.LookupResourceInDictionary(resourceDictionary, key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
				}
			}
			if (obj != null)
			{
				SystemResources.Freeze(obj);
			}
			return obj;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002A15C File Offset: 0x0002835C
		private static object LookupResourceInDictionary(ResourceDictionary dictionary, object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, out bool canCache)
		{
			object result = null;
			SystemResources.IsSystemResourcesParsing = true;
			try
			{
				result = dictionary.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out canCache);
			}
			finally
			{
				SystemResources.IsSystemResourcesParsing = false;
			}
			return result;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002A198 File Offset: 0x00028398
		private static void Freeze(object resource)
		{
			Freezable freezable = resource as Freezable;
			if (freezable != null && !freezable.IsFrozen)
			{
				freezable.Freeze();
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002A1C0 File Offset: 0x000283C0
		private static SystemResources.ResourceDictionaries EnsureDictionarySlot(Assembly assembly)
		{
			SystemResources.ResourceDictionaries resourceDictionaries = null;
			if (SystemResources._dictionaries != null)
			{
				SystemResources._dictionaries.TryGetValue(assembly, out resourceDictionaries);
			}
			else
			{
				SystemResources._dictionaries = new Dictionary<Assembly, SystemResources.ResourceDictionaries>(1);
			}
			if (resourceDictionaries == null)
			{
				resourceDictionaries = new SystemResources.ResourceDictionaries(assembly);
				SystemResources._dictionaries.Add(assembly, resourceDictionaries);
			}
			return resourceDictionaries;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002A208 File Offset: 0x00028408
		private static bool IgnoreAssembly(Assembly assembly)
		{
			return assembly == SystemResources.MsCorLib || assembly == SystemResources.PresentationCore || assembly == SystemResources.WindowsBase;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x0002A231 File Offset: 0x00028431
		private static Assembly MsCorLib
		{
			get
			{
				if (SystemResources._mscorlib == null)
				{
					SystemResources._mscorlib = typeof(string).Assembly;
				}
				return SystemResources._mscorlib;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0002A259 File Offset: 0x00028459
		private static Assembly PresentationFramework
		{
			get
			{
				if (SystemResources._presentationFramework == null)
				{
					SystemResources._presentationFramework = typeof(FrameworkElement).Assembly;
				}
				return SystemResources._presentationFramework;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0002A281 File Offset: 0x00028481
		private static Assembly PresentationCore
		{
			get
			{
				if (SystemResources._presentationCore == null)
				{
					SystemResources._presentationCore = typeof(UIElement).Assembly;
				}
				return SystemResources._presentationCore;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0002A2A9 File Offset: 0x000284A9
		private static Assembly WindowsBase
		{
			get
			{
				if (SystemResources._windowsBase == null)
				{
					SystemResources._windowsBase = typeof(DependencyObject).Assembly;
				}
				return SystemResources._windowsBase;
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002A2D4 File Offset: 0x000284D4
		[SecuritySafeCritical]
		private static void EnsureResourceChangeListener()
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotifyHook != null && SystemResources._hwndNotify.Count != 0)
			{
				if (SystemResources._hwndNotify.Keys.FirstOrDefault((DpiUtil.HwndDpiInfo hwndDpiContext) => hwndDpiContext.DpiAwarenessContextValue == SystemResources.ProcessDpiAwarenessContextValue) != null)
				{
					return;
				}
			}
			SystemResources._hwndNotify = new Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>>();
			SystemResources._hwndNotifyHook = new Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook>();
			SystemResources._dpiAwarenessContextAndDpis = new List<DpiUtil.HwndDpiInfo>();
			DpiUtil.HwndDpiInfo item = SystemResources.CreateResourceChangeListenerWindow(SystemResources.ProcessDpiAwarenessContextValue, 0, 0, "EnsureResourceChangeListener");
			SystemResources._dpiAwarenessContextAndDpis.Add(item);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002A368 File Offset: 0x00028568
		[SecuritySafeCritical]
		private static bool EnsureResourceChangeListener(DpiUtil.HwndDpiInfo hwndDpiInfo)
		{
			SystemResources.EnsureResourceChangeListener();
			if (hwndDpiInfo.DpiAwarenessContextValue == DpiAwarenessContextValue.Invalid)
			{
				return false;
			}
			if (!SystemResources._hwndNotify.ContainsKey(hwndDpiInfo))
			{
				DpiUtil.HwndDpiInfo hwndDpiInfo2 = SystemResources.CreateResourceChangeListenerWindow(hwndDpiInfo.DpiAwarenessContextValue, hwndDpiInfo.ContainingMonitorScreenRect.left, hwndDpiInfo.ContainingMonitorScreenRect.top, "EnsureResourceChangeListener");
				if (hwndDpiInfo2 == hwndDpiInfo && !SystemResources._dpiAwarenessContextAndDpis.Contains(hwndDpiInfo))
				{
					SystemResources._dpiAwarenessContextAndDpis.Add(hwndDpiInfo);
				}
			}
			return SystemResources._hwndNotify.ContainsKey(hwndDpiInfo);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002A3E0 File Offset: 0x000285E0
		[SecurityCritical]
		private static DpiUtil.HwndDpiInfo CreateResourceChangeListenerWindow(DpiAwarenessContextValue dpiContextValue, int x = 0, int y = 0, [CallerMemberName] string callerName = "")
		{
			DpiUtil.HwndDpiInfo result;
			using (DpiUtil.WithDpiAwarenessContext(dpiContextValue))
			{
				HwndWrapper hwndWrapper = new HwndWrapper(0, -2013265920, 0, x, y, 0, 0, "SystemResourceNotifyWindow", IntPtr.Zero, null);
				DpiUtil.HwndDpiInfo hwndDpiInfo = SystemResources.IsPerMonitorDpiScalingActive ? DpiUtil.GetExtendedDpiInfoForWindow(hwndWrapper.Handle) : new DpiUtil.HwndDpiInfo(dpiContextValue, SystemResources.GetDpiScaleForUnawareOrSystemAwareContext(dpiContextValue));
				SystemResources._hwndNotify[hwndDpiInfo] = new SecurityCriticalDataClass<HwndWrapper>(hwndWrapper);
				SystemResources._hwndNotify[hwndDpiInfo].Value.Dispatcher.ShutdownFinished += SystemResources.OnShutdownFinished;
				SystemResources._hwndNotifyHook[hwndDpiInfo] = new HwndWrapperHook(SystemResources.SystemThemeFilterMessage);
				SystemResources._hwndNotify[hwndDpiInfo].Value.AddHook(SystemResources._hwndNotifyHook[hwndDpiInfo]);
				result = hwndDpiInfo;
			}
			return result;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002A4C0 File Offset: 0x000286C0
		[SecuritySafeCritical]
		private static void OnShutdownFinished(object sender, EventArgs args)
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				foreach (DpiUtil.HwndDpiInfo key in SystemResources._dpiAwarenessContextAndDpis)
				{
					SystemResources._hwndNotify[key].Value.Dispose();
					SystemResources._hwndNotifyHook[key] = null;
				}
			}
			Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>> hwndNotify = SystemResources._hwndNotify;
			if (hwndNotify != null)
			{
				hwndNotify.Clear();
			}
			SystemResources._hwndNotify = null;
			Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook> hwndNotifyHook = SystemResources._hwndNotifyHook;
			if (hwndNotifyHook != null)
			{
				hwndNotifyHook.Clear();
			}
			SystemResources._hwndNotifyHook = null;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002A56C File Offset: 0x0002876C
		private static DpiScale2 GetDpiScaleForUnawareOrSystemAwareContext(DpiAwarenessContextValue dpiContextValue)
		{
			DpiScale2 result;
			if (dpiContextValue != DpiAwarenessContextValue.SystemAware && dpiContextValue == DpiAwarenessContextValue.Unaware)
			{
				result = DpiScale2.FromPixelsPerInch(96.0, 96.0);
			}
			else
			{
				result = DpiUtil.GetSystemDpi();
			}
			return result;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002A5A8 File Offset: 0x000287A8
		private static void OnThemeChanged()
		{
			SystemResources.ResourceDictionaries.OnThemeChanged();
			UxThemeWrapper.OnThemeChanged();
			ThemeDictionaryExtension.OnThemeChanged();
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			lock (themeDictionaryLock)
			{
				SystemResources._resourceCache.Clear();
				SystemResources._themeStyleCache.Clear();
				if (SystemResources._dictionaries != null)
				{
					foreach (SystemResources.ResourceDictionaries resourceDictionaries in SystemResources._dictionaries.Values)
					{
						resourceDictionaries.ClearThemedDictionary();
					}
				}
			}
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002A650 File Offset: 0x00028850
		private static void OnSystemValueChanged()
		{
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			lock (themeDictionaryLock)
			{
				List<SystemResourceKey> list = new List<SystemResourceKey>();
				foreach (object obj in SystemResources._resourceCache.Keys)
				{
					SystemResourceKey systemResourceKey = obj as SystemResourceKey;
					if (systemResourceKey != null)
					{
						list.Add(systemResourceKey);
					}
				}
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					SystemResources._resourceCache.Remove(list[i]);
				}
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002A718 File Offset: 0x00028918
		private static object InvalidateTreeResources(object args)
		{
			object[] array = (object[])args;
			PresentationSource presentationSource = (PresentationSource)array[0];
			if (!presentationSource.IsDisposed)
			{
				FrameworkElement frameworkElement = presentationSource.RootVisual as FrameworkElement;
				if (frameworkElement != null)
				{
					bool flag = (bool)array[1];
					if (flag)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, ResourcesChangeInfo.SysColorsOrSettingsChangeInfo);
					}
					else
					{
						TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					KeyboardNavigation.AlwaysShowFocusVisual = SystemParameters.KeyboardCues;
					frameworkElement.CoerceValue(KeyboardNavigation.ShowKeyboardCuesProperty);
					SystemResources.SystemResourcesAreChanging = true;
					frameworkElement.CoerceValue(TextElement.FontFamilyProperty);
					frameworkElement.CoerceValue(TextElement.FontSizeProperty);
					frameworkElement.CoerceValue(TextElement.FontStyleProperty);
					frameworkElement.CoerceValue(TextElement.FontWeightProperty);
					SystemResources.SystemResourcesAreChanging = false;
					PopupRoot popupRoot = frameworkElement as PopupRoot;
					if (popupRoot != null && popupRoot.Parent != null)
					{
						popupRoot.Parent.CoerceValue(Popup.HasDropShadowProperty);
					}
				}
			}
			return null;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002A7EC File Offset: 0x000289EC
		[SecurityCritical]
		private static void InvalidateTabletDevices(WindowMessage msg, IntPtr wParam, IntPtr lParam)
		{
			if (StylusLogic.IsStylusAndTouchSupportEnabled && StylusLogic.IsInstantiated && SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				Dispatcher dispatcher = SystemResources.Hwnd.Dispatcher;
				if (dispatcher != null && dispatcher.InputManager != null)
				{
					StylusLogic.CurrentStylusLogic.HandleMessage(msg, wParam, lParam);
				}
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002A840 File Offset: 0x00028A40
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void InvalidateResources(bool isSysColorsOrSettingsChange)
		{
			SystemResources.SystemResourcesHaveChanged = true;
			Dispatcher dispatcher = isSysColorsOrSettingsChange ? null : Dispatcher.FromThread(Thread.CurrentThread);
			if (dispatcher != null || isSysColorsOrSettingsChange)
			{
				foreach (object obj in PresentationSource.CriticalCurrentSources)
				{
					PresentationSource presentationSource = (PresentationSource)obj;
					if (!presentationSource.IsDisposed && (isSysColorsOrSettingsChange || presentationSource.Dispatcher == dispatcher))
					{
						presentationSource.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(SystemResources.InvalidateTreeResources), new object[]
						{
							presentationSource,
							isSysColorsOrSettingsChange
						});
					}
				}
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002A8F4 File Offset: 0x00028AF4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static IntPtr SystemThemeFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg <= 536)
			{
				if (msg <= 26)
				{
					if (msg != 21)
					{
						if (msg == 26)
						{
							SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
							if (SystemParameters.InvalidateCache((int)wParam))
							{
								SystemResources.OnSystemValueChanged();
								SystemResources.InvalidateResources(true);
								HighContrastHelper.OnSettingChanged();
							}
							SystemParameters.InvalidateWindowFrameThicknessProperties();
						}
					}
					else if (SystemColors.InvalidateCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
				else if (msg != 126)
				{
					if (msg == 536)
					{
						if (NativeMethods.IntPtrToInt32(wParam) == 10 && SystemParameters.InvalidatePowerDependentCache())
						{
							SystemResources.OnSystemValueChanged();
							SystemResources.InvalidateResources(true);
						}
					}
				}
				else
				{
					SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					if (SystemParameters.InvalidateDisplayDependentCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
			}
			else if (msg <= 712)
			{
				if (msg != 537)
				{
					if (msg == 712)
					{
						SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					}
				}
				else
				{
					SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
					if (SystemParameters.InvalidateDeviceDependentCache())
					{
						SystemResources.OnSystemValueChanged();
						SystemResources.InvalidateResources(true);
					}
				}
			}
			else if (msg != 713)
			{
				switch (msg)
				{
				case 794:
					SystemColors.InvalidateCache();
					SystemParameters.InvalidateCache();
					SystemParameters.InvalidateDerivedThemeRelatedProperties();
					SystemResources.OnThemeChanged();
					SystemResources.InvalidateResources(false);
					break;
				case 798:
				case 799:
					SystemParameters.InvalidateIsGlassEnabled();
					break;
				case 800:
					SystemParameters.InvalidateWindowGlassColorizationProperties();
					break;
				}
			}
			else
			{
				SystemResources.InvalidateTabletDevices((WindowMessage)msg, wParam, lParam);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002AA80 File Offset: 0x00028C80
		internal static bool ClearBitArray(BitArray cacheValid)
		{
			bool result = false;
			for (int i = 0; i < cacheValid.Count; i++)
			{
				if (SystemResources.ClearSlot(cacheValid, i))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002AAAC File Offset: 0x00028CAC
		internal static bool ClearSlot(BitArray cacheValid, int slot)
		{
			if (cacheValid[slot])
			{
				cacheValid[slot] = false;
				return true;
			}
			return false;
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002AAC2 File Offset: 0x00028CC2
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0002AACC File Offset: 0x00028CCC
		internal static bool IsSystemResourcesParsing
		{
			get
			{
				return SystemResources._parsing > 0;
			}
			set
			{
				if (value)
				{
					SystemResources._parsing++;
					return;
				}
				SystemResources._parsing--;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0002AAEA File Offset: 0x00028CEA
		internal static object ThemeDictionaryLock
		{
			get
			{
				return SystemResources._resourceCache.SyncRoot;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x0002AAF8 File Offset: 0x00028CF8
		private static DpiAwarenessContextValue ProcessDpiAwarenessContextValue
		{
			get
			{
				if (HwndTarget.IsProcessUnaware == true)
				{
					return DpiAwarenessContextValue.Unaware;
				}
				if (HwndTarget.IsProcessSystemAware == true)
				{
					return DpiAwarenessContextValue.SystemAware;
				}
				if (HwndTarget.IsProcessPerMonitorDpiAware == true)
				{
					return DpiAwarenessContextValue.PerMonitorAware;
				}
				return DpiUtil.GetProcessDpiAwarenessContextValue(IntPtr.Zero);
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0002AB71 File Offset: 0x00028D71
		private static bool IsPerMonitorDpiScalingActive
		{
			get
			{
				return HwndTarget.IsPerMonitorDpiScalingEnabled && (SystemResources.ProcessDpiAwarenessContextValue == DpiAwarenessContextValue.PerMonitorAware || SystemResources.ProcessDpiAwarenessContextValue == DpiAwarenessContextValue.PerMonitorAwareVersion2);
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000B99 RID: 2969 RVA: 0x0002AB90 File Offset: 0x00028D90
		private static HwndWrapper Hwnd
		{
			[SecurityCritical]
			get
			{
				SystemResources.EnsureResourceChangeListener();
				DpiUtil.HwndDpiInfo key = SystemResources._hwndNotify.Keys.FirstOrDefault((DpiUtil.HwndDpiInfo hwndDpiContext) => hwndDpiContext.DpiAwarenessContextValue == SystemResources.ProcessDpiAwarenessContextValue);
				return SystemResources._hwndNotify[key].Value;
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002ABE4 File Offset: 0x00028DE4
		[SecurityCritical]
		internal static HwndWrapper GetDpiAwarenessCompatibleNotificationWindow(HandleRef hwnd)
		{
			DpiAwarenessContextValue processDpiAwarenessContextValue = SystemResources.ProcessDpiAwarenessContextValue;
			DpiUtil.HwndDpiInfo hwndDpiInfo = SystemResources.IsPerMonitorDpiScalingActive ? DpiUtil.GetExtendedDpiInfoForWindow(hwnd.Handle, true) : new DpiUtil.HwndDpiInfo(processDpiAwarenessContextValue, SystemResources.GetDpiScaleForUnawareOrSystemAwareContext(processDpiAwarenessContextValue));
			if (SystemResources.EnsureResourceChangeListener(hwndDpiInfo))
			{
				return SystemResources._hwndNotify[hwndDpiInfo].Value;
			}
			return null;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002AC34 File Offset: 0x00028E34
		internal static void DelayHwndShutdown()
		{
			if (SystemResources._hwndNotify != null && SystemResources._hwndNotify.Count != 0)
			{
				Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;
				currentDispatcher.ShutdownFinished -= SystemResources.OnShutdownFinished;
				currentDispatcher.ShutdownFinished += SystemResources.OnShutdownFinished;
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000B9C RID: 2972 RVA: 0x0002AC80 File Offset: 0x00028E80
		// (remove) Token: 0x06000B9D RID: 2973 RVA: 0x0002ACB4 File Offset: 0x00028EB4
		internal static event EventHandler<ResourceDictionaryLoadedEventArgs> ThemedDictionaryLoaded;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000B9E RID: 2974 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		// (remove) Token: 0x06000B9F RID: 2975 RVA: 0x0002AD1C File Offset: 0x00028F1C
		internal static event EventHandler<ResourceDictionaryUnloadedEventArgs> ThemedDictionaryUnloaded;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000BA0 RID: 2976 RVA: 0x0002AD50 File Offset: 0x00028F50
		// (remove) Token: 0x06000BA1 RID: 2977 RVA: 0x0002AD84 File Offset: 0x00028F84
		internal static event EventHandler<ResourceDictionaryLoadedEventArgs> GenericDictionaryLoaded;

		// Token: 0x04000A9E RID: 2718
		[ThreadStatic]
		private static int _parsing;

		// Token: 0x04000A9F RID: 2719
		[ThreadStatic]
		private static List<DpiUtil.HwndDpiInfo> _dpiAwarenessContextAndDpis;

		// Token: 0x04000AA0 RID: 2720
		[ThreadStatic]
		private static Dictionary<DpiUtil.HwndDpiInfo, SecurityCriticalDataClass<HwndWrapper>> _hwndNotify;

		// Token: 0x04000AA1 RID: 2721
		[ThreadStatic]
		[SecurityCritical]
		private static Dictionary<DpiUtil.HwndDpiInfo, HwndWrapperHook> _hwndNotifyHook;

		// Token: 0x04000AA2 RID: 2722
		private static Hashtable _resourceCache = new Hashtable();

		// Token: 0x04000AA3 RID: 2723
		private static DTypeMap _themeStyleCache = new DTypeMap(100);

		// Token: 0x04000AA4 RID: 2724
		private static Dictionary<Assembly, SystemResources.ResourceDictionaries> _dictionaries;

		// Token: 0x04000AA5 RID: 2725
		private static object _specialNull = new object();

		// Token: 0x04000AA6 RID: 2726
		internal const string GenericResourceName = "themes/generic";

		// Token: 0x04000AA7 RID: 2727
		internal const string ClassicResourceName = "themes/classic";

		// Token: 0x04000AA8 RID: 2728
		private static Assembly _mscorlib;

		// Token: 0x04000AA9 RID: 2729
		private static Assembly _presentationFramework;

		// Token: 0x04000AAA RID: 2730
		private static Assembly _presentationCore;

		// Token: 0x04000AAB RID: 2731
		private static Assembly _windowsBase;

		// Token: 0x04000AAC RID: 2732
		internal const string PresentationFrameworkName = "PresentationFramework";

		// Token: 0x04000AAD RID: 2733
		internal static bool SystemResourcesHaveChanged;

		// Token: 0x04000AAE RID: 2734
		[ThreadStatic]
		internal static bool SystemResourcesAreChanging;

		// Token: 0x0200082B RID: 2091
		internal class ResourceDictionaries
		{
			// Token: 0x06007E72 RID: 32370 RVA: 0x00235C4C File Offset: 0x00233E4C
			internal ResourceDictionaries(Assembly assembly)
			{
				this._assembly = assembly;
				this._themedDictionaryAssembly = null;
				this._themedDictionarySourceUri = null;
				this._genericDictionaryAssembly = null;
				this._genericDictionarySourceUri = null;
				if (assembly == SystemResources.PresentationFramework)
				{
					this._assemblyName = "PresentationFramework";
					this._genericDictionary = null;
					this._genericLoaded = true;
					this._genericLocation = ResourceDictionaryLocation.None;
					this._themedLocation = ResourceDictionaryLocation.ExternalAssembly;
					this._locationsLoaded = true;
					return;
				}
				this._assemblyName = SafeSecurityHelper.GetAssemblyPartialName(assembly);
			}

			// Token: 0x06007E73 RID: 32371 RVA: 0x00235CCC File Offset: 0x00233ECC
			internal void ClearThemedDictionary()
			{
				ResourceDictionaryInfo themedDictionaryInfo = this.ThemedDictionaryInfo;
				this._themedLoaded = false;
				this._themedDictionary = null;
				this._themedDictionaryAssembly = null;
				this._themedDictionarySourceUri = null;
				if (themedDictionaryInfo.ResourceDictionary != null)
				{
					EventHandler<ResourceDictionaryUnloadedEventArgs> themedDictionaryUnloaded = SystemResources.ThemedDictionaryUnloaded;
					if (themedDictionaryUnloaded == null)
					{
						return;
					}
					themedDictionaryUnloaded(null, new ResourceDictionaryUnloadedEventArgs(themedDictionaryInfo));
				}
			}

			// Token: 0x06007E74 RID: 32372 RVA: 0x00235D1C File Offset: 0x00233F1C
			internal ResourceDictionary LoadThemedDictionary(bool isTraceEnabled)
			{
				if (!this._themedLoaded)
				{
					this.LoadDictionaryLocations();
					if (this._preventReEnter || this._themedLocation == ResourceDictionaryLocation.None)
					{
						return null;
					}
					SystemResources.IsSystemResourcesParsing = true;
					this._preventReEnter = true;
					try
					{
						ResourceDictionary resourceDictionary = null;
						bool flag = this._themedLocation == ResourceDictionaryLocation.ExternalAssembly;
						string assemblyName;
						if (flag)
						{
							this.LoadExternalAssembly(false, false, out this._themedDictionaryAssembly, out assemblyName);
						}
						else
						{
							this._themedDictionaryAssembly = this._assembly;
							assemblyName = this._assemblyName;
						}
						if (this._themedDictionaryAssembly != null)
						{
							resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, isTraceEnabled, out this._themedDictionarySourceUri);
							if (resourceDictionary == null && !flag)
							{
								this.LoadExternalAssembly(false, false, out this._themedDictionaryAssembly, out assemblyName);
								if (this._themedDictionaryAssembly != null)
								{
									resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, SystemResources.ResourceDictionaries.ThemedResourceName, isTraceEnabled, out this._themedDictionarySourceUri);
								}
							}
						}
						if (resourceDictionary == null && UxThemeWrapper.IsActive)
						{
							if (flag)
							{
								this.LoadExternalAssembly(true, false, out this._themedDictionaryAssembly, out assemblyName);
							}
							else
							{
								this._themedDictionaryAssembly = this._assembly;
								assemblyName = this._assemblyName;
							}
							if (this._themedDictionaryAssembly != null)
							{
								resourceDictionary = this.LoadDictionary(this._themedDictionaryAssembly, assemblyName, "themes/classic", isTraceEnabled, out this._themedDictionarySourceUri);
							}
						}
						this._themedDictionary = resourceDictionary;
						this._themedLoaded = true;
						if (this._themedDictionary != null)
						{
							EventHandler<ResourceDictionaryLoadedEventArgs> themedDictionaryLoaded = SystemResources.ThemedDictionaryLoaded;
							if (themedDictionaryLoaded != null)
							{
								themedDictionaryLoaded(null, new ResourceDictionaryLoadedEventArgs(this.ThemedDictionaryInfo));
							}
						}
					}
					finally
					{
						this._preventReEnter = false;
						SystemResources.IsSystemResourcesParsing = false;
					}
				}
				return this._themedDictionary;
			}

			// Token: 0x06007E75 RID: 32373 RVA: 0x00235EB4 File Offset: 0x002340B4
			internal ResourceDictionary LoadGenericDictionary(bool isTraceEnabled)
			{
				if (!this._genericLoaded)
				{
					this.LoadDictionaryLocations();
					if (this._preventReEnter || this._genericLocation == ResourceDictionaryLocation.None)
					{
						return null;
					}
					SystemResources.IsSystemResourcesParsing = true;
					this._preventReEnter = true;
					try
					{
						ResourceDictionary genericDictionary = null;
						string assemblyName;
						if (this._genericLocation == ResourceDictionaryLocation.ExternalAssembly)
						{
							this.LoadExternalAssembly(false, true, out this._genericDictionaryAssembly, out assemblyName);
						}
						else
						{
							this._genericDictionaryAssembly = this._assembly;
							assemblyName = this._assemblyName;
						}
						if (this._genericDictionaryAssembly != null)
						{
							genericDictionary = this.LoadDictionary(this._genericDictionaryAssembly, assemblyName, "themes/generic", isTraceEnabled, out this._genericDictionarySourceUri);
						}
						this._genericDictionary = genericDictionary;
						this._genericLoaded = true;
						if (this._genericDictionary != null)
						{
							EventHandler<ResourceDictionaryLoadedEventArgs> genericDictionaryLoaded = SystemResources.GenericDictionaryLoaded;
							if (genericDictionaryLoaded != null)
							{
								genericDictionaryLoaded(null, new ResourceDictionaryLoadedEventArgs(this.GenericDictionaryInfo));
							}
						}
					}
					finally
					{
						this._preventReEnter = false;
						SystemResources.IsSystemResourcesParsing = false;
					}
				}
				return this._genericDictionary;
			}

			// Token: 0x06007E76 RID: 32374 RVA: 0x00235FA4 File Offset: 0x002341A4
			private void LoadDictionaryLocations()
			{
				if (!this._locationsLoaded)
				{
					ThemeInfoAttribute themeInfoAttribute = ThemeInfoAttribute.FromAssembly(this._assembly);
					if (themeInfoAttribute != null)
					{
						this._themedLocation = themeInfoAttribute.ThemeDictionaryLocation;
						this._genericLocation = themeInfoAttribute.GenericDictionaryLocation;
					}
					else
					{
						this._themedLocation = ResourceDictionaryLocation.None;
						this._genericLocation = ResourceDictionaryLocation.None;
					}
					this._locationsLoaded = true;
				}
			}

			// Token: 0x06007E77 RID: 32375 RVA: 0x00235FF8 File Offset: 0x002341F8
			private void LoadExternalAssembly(bool classic, bool generic, out Assembly assembly, out string assemblyName)
			{
				StringBuilder stringBuilder = new StringBuilder(this._assemblyName.Length + 10);
				stringBuilder.Append(this._assemblyName);
				stringBuilder.Append(".");
				if (generic)
				{
					stringBuilder.Append("generic");
				}
				else if (classic)
				{
					stringBuilder.Append("classic");
				}
				else
				{
					stringBuilder.Append(UxThemeWrapper.ThemeName);
				}
				assemblyName = stringBuilder.ToString();
				string fullAssemblyNameFromPartialName = SafeSecurityHelper.GetFullAssemblyNameFromPartialName(this._assembly, assemblyName);
				assembly = null;
				try
				{
					assembly = Assembly.Load(fullAssemblyNameFromPartialName);
				}
				catch (FileNotFoundException)
				{
				}
				catch (BadImageFormatException)
				{
				}
				if (this._assemblyName == "PresentationFramework" && assembly != null)
				{
					Type type = assembly.GetType("Microsoft.Windows.Themes.KnownTypeHelper");
					if (type != null)
					{
						SecurityHelper.RunClassConstructor(type);
					}
				}
			}

			// Token: 0x17001D5B RID: 7515
			// (get) Token: 0x06007E78 RID: 32376 RVA: 0x002360E0 File Offset: 0x002342E0
			internal static string ThemedResourceName
			{
				get
				{
					string text = SystemResources.ResourceDictionaries._themedResourceName;
					while (text == null)
					{
						text = UxThemeWrapper.ThemedResourceName;
						string text2 = Interlocked.CompareExchange<string>(ref SystemResources.ResourceDictionaries._themedResourceName, text, null);
						if (text2 != null && text2 != text)
						{
							SystemResources.ResourceDictionaries._themedResourceName = null;
							text = null;
						}
					}
					return text;
				}
			}

			// Token: 0x17001D5C RID: 7516
			// (get) Token: 0x06007E79 RID: 32377 RVA: 0x00236120 File Offset: 0x00234320
			internal ResourceDictionaryInfo GenericDictionaryInfo
			{
				get
				{
					return new ResourceDictionaryInfo(this._assembly, this._genericDictionaryAssembly, this._genericDictionary, this._genericDictionarySourceUri);
				}
			}

			// Token: 0x17001D5D RID: 7517
			// (get) Token: 0x06007E7A RID: 32378 RVA: 0x0023613F File Offset: 0x0023433F
			internal ResourceDictionaryInfo ThemedDictionaryInfo
			{
				get
				{
					return new ResourceDictionaryInfo(this._assembly, this._themedDictionaryAssembly, this._themedDictionary, this._themedDictionarySourceUri);
				}
			}

			// Token: 0x06007E7B RID: 32379 RVA: 0x00236160 File Offset: 0x00234360
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private ResourceDictionary LoadDictionary(Assembly assembly, string assemblyName, string resourceName, bool isTraceEnabled, out Uri dictionarySourceUri)
			{
				ResourceDictionary resourceDictionary = null;
				dictionarySourceUri = null;
				ResourceManager resourceManager = new ResourceManager(assemblyName + ".g", assembly);
				resourceName += ".baml";
				Stream stream = null;
				try
				{
					stream = resourceManager.GetStream(resourceName, CultureInfo.CurrentUICulture);
				}
				catch (MissingManifestResourceException)
				{
				}
				catch (MissingSatelliteAssemblyException)
				{
				}
				catch (InvalidOperationException)
				{
				}
				if (stream != null)
				{
					Baml2006ReaderSettings baml2006ReaderSettings = new Baml2006ReaderSettings();
					baml2006ReaderSettings.OwnsStream = true;
					baml2006ReaderSettings.LocalAssembly = assembly;
					Baml2006Reader baml2006Reader = new Baml2006ReaderInternal(stream, new Baml2006SchemaContext(baml2006ReaderSettings.LocalAssembly), baml2006ReaderSettings);
					XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettingsForBaml();
					if (assembly != null)
					{
						xamlObjectWriterSettings.AccessLevel = XamlAccessLevel.AssemblyAccessTo(assembly);
						AssemblyName assemblyName2 = new AssemblyName(assembly.FullName);
						Uri uri = null;
						string uriString = string.Format("pack://application:,,,/{0};v{1};component/{2}", assemblyName2.Name, assemblyName2.Version.ToString(), resourceName);
						if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
						{
							if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
							{
								xamlObjectWriterSettings.SourceBamlUri = uri;
							}
							dictionarySourceUri = uri;
						}
					}
					XamlObjectWriter xamlObjectWriter = new XamlObjectWriter(baml2006Reader.SchemaContext, xamlObjectWriterSettings);
					if (xamlObjectWriterSettings.AccessLevel != null)
					{
						XamlLoadPermission xamlLoadPermission = new XamlLoadPermission(xamlObjectWriterSettings.AccessLevel);
						xamlLoadPermission.Assert();
						try
						{
							XamlServices.Transform(baml2006Reader, xamlObjectWriter);
							goto IL_123;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					XamlServices.Transform(baml2006Reader, xamlObjectWriter);
					IL_123:
					resourceDictionary = (ResourceDictionary)xamlObjectWriter.Result;
					if (isTraceEnabled && resourceDictionary != null)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientResourceBamlAssembly, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, assemblyName);
					}
				}
				return resourceDictionary;
			}

			// Token: 0x06007E7C RID: 32380 RVA: 0x002362F0 File Offset: 0x002344F0
			internal static void OnThemeChanged()
			{
				SystemResources.ResourceDictionaries._themedResourceName = null;
			}

			// Token: 0x04003C8C RID: 15500
			private ResourceDictionary _genericDictionary;

			// Token: 0x04003C8D RID: 15501
			private ResourceDictionary _themedDictionary;

			// Token: 0x04003C8E RID: 15502
			private bool _genericLoaded;

			// Token: 0x04003C8F RID: 15503
			private bool _themedLoaded;

			// Token: 0x04003C90 RID: 15504
			private bool _preventReEnter;

			// Token: 0x04003C91 RID: 15505
			private bool _locationsLoaded;

			// Token: 0x04003C92 RID: 15506
			private string _assemblyName;

			// Token: 0x04003C93 RID: 15507
			private Assembly _assembly;

			// Token: 0x04003C94 RID: 15508
			private ResourceDictionaryLocation _genericLocation;

			// Token: 0x04003C95 RID: 15509
			private ResourceDictionaryLocation _themedLocation;

			// Token: 0x04003C96 RID: 15510
			private static string _themedResourceName;

			// Token: 0x04003C97 RID: 15511
			private Assembly _themedDictionaryAssembly;

			// Token: 0x04003C98 RID: 15512
			private Assembly _genericDictionaryAssembly;

			// Token: 0x04003C99 RID: 15513
			private Uri _themedDictionarySourceUri;

			// Token: 0x04003C9A RID: 15514
			private Uri _genericDictionarySourceUri;
		}
	}
}
