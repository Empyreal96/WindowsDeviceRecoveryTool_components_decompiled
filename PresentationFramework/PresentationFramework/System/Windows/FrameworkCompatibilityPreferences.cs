using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Contains properties that specify how an application should behave relative to WPF features that are in the PresentationFramework assembly.</summary>
	// Token: 0x020000A1 RID: 161
	public static class FrameworkCompatibilityPreferences
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00008D1C File Offset: 0x00006F1C
		static FrameworkCompatibilityPreferences()
		{
			FrameworkCompatibilityPreferences._targetsDesktop_V4_0 = (BinaryCompatibility.AppWasBuiltForFramework == TargetFrameworkId.NetFramework && !BinaryCompatibility.TargetsAtLeast_Desktop_V4_5);
			NameValueCollection nameValueCollection = null;
			try
			{
				nameValueCollection = ConfigurationManager.AppSettings;
			}
			catch (ConfigurationErrorsException)
			{
			}
			if (nameValueCollection != null)
			{
				FrameworkCompatibilityPreferences.SetHandleTwoWayBindingToPropertyWithNonPublicSetterFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetUseSetWindowPosForTopmostWindowsFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetVSP45CompatFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetScrollingTraceFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetShouldThrowOnCopyOrCutFailuresFromAppSettings(nameValueCollection);
				FrameworkCompatibilityPreferences.SetDisableLegacyDangerousXamlDeserializationMode(nameValueCollection);
				FrameworkCompatibilityPreferences.SetShouldThrowOnDataGridCopyOrCutFailuresFromAppSettings(nameValueCollection);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00008E00 File Offset: 0x00007000
		internal static bool TargetsDesktop_V4_0
		{
			get
			{
				return FrameworkCompatibilityPreferences._targetsDesktop_V4_0;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the application should use the <see cref="P:System.Windows.SystemColors.InactiveSelectionHighlightBrush" /> and <see cref="P:System.Windows.SystemColors.InactiveSelectionHighlightTextBrush" /> properties for the colors of inactive selected items.</summary>
		/// <returns>
		///     <see langword="true" /> if the application should use the <see cref="P:System.Windows.SystemColors.InactiveSelectionHighlightBrush" /> and <see cref="P:System.Windows.SystemColors.InactiveSelectionHighlightTextBrush" /> properties for the colors of inactive selected items; otherwise, <see langword="false" /></returns>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00008E07 File Offset: 0x00007007
		// (set) Token: 0x0600031F RID: 799 RVA: 0x00008E10 File Offset: 0x00007010
		public static bool AreInactiveSelectionHighlightBrushKeysSupported
		{
			get
			{
				return FrameworkCompatibilityPreferences._areInactiveSelectionHighlightBrushKeysSupported;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"AreInactiveSelectionHighlightBrushKeysSupported",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._areInactiveSelectionHighlightBrushKeysSupported = value;
				}
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00008E7C File Offset: 0x0000707C
		internal static bool GetAreInactiveSelectionHighlightBrushKeysSupported()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.AreInactiveSelectionHighlightBrushKeysSupported;
		}

		/// <summary>Gets or sets a value that indicates whether a data-bound <see cref="T:System.Windows.Controls.TextBox" /> should display a string that is identical to the value of the source its <see cref="P:System.Windows.Controls.TextBox.Text" /> property</summary>
		/// <returns>
		///     <see langword="true" /> if a data-bound <see cref="T:System.Windows.Controls.TextBox" /> should display a string that is identical to the value of the source its <see cref="P:System.Windows.Controls.TextBox.Text" /> property; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00008E88 File Offset: 0x00007088
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00008E90 File Offset: 0x00007090
		public static bool KeepTextBoxDisplaySynchronizedWithTextProperty
		{
			get
			{
				return FrameworkCompatibilityPreferences._keepTextBoxDisplaySynchronizedWithTextProperty;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"AextBoxDisplaysText",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._keepTextBoxDisplaySynchronizedWithTextProperty = value;
				}
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00008EFC File Offset: 0x000070FC
		internal static bool GetKeepTextBoxDisplaySynchronizedWithTextProperty()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00008F08 File Offset: 0x00007108
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00008F10 File Offset: 0x00007110
		internal static FrameworkCompatibilityPreferences.HandleBindingOptions HandleTwoWayBindingToPropertyWithNonPublicSetter
		{
			get
			{
				return FrameworkCompatibilityPreferences._handleTwoWayBindingToPropertyWithNonPublicSetter;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"HandleTwoWayBindingToPropertyWithNonPublicSetter",
							"FrameworkCompatibilityPreferences"
						}));
					}
					if (value.CompareTo(FrameworkCompatibilityPreferences._handleTwoWayBindingToPropertyWithNonPublicSetter) > 0)
					{
						throw new ArgumentException();
					}
					FrameworkCompatibilityPreferences._handleTwoWayBindingToPropertyWithNonPublicSetter = value;
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00008F9C File Offset: 0x0000719C
		internal static FrameworkCompatibilityPreferences.HandleBindingOptions GetHandleTwoWayBindingToPropertyWithNonPublicSetter()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.HandleTwoWayBindingToPropertyWithNonPublicSetter;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00008FA8 File Offset: 0x000071A8
		private static void SetHandleTwoWayBindingToPropertyWithNonPublicSetterFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["HandleTwoWayBindingToPropertyWithNonPublicSetter"];
			FrameworkCompatibilityPreferences.HandleBindingOptions handleTwoWayBindingToPropertyWithNonPublicSetter;
			if (Enum.TryParse<FrameworkCompatibilityPreferences.HandleBindingOptions>(value, true, out handleTwoWayBindingToPropertyWithNonPublicSetter) && handleTwoWayBindingToPropertyWithNonPublicSetter.CompareTo(FrameworkCompatibilityPreferences.HandleTwoWayBindingToPropertyWithNonPublicSetter) <= 0)
			{
				FrameworkCompatibilityPreferences.HandleTwoWayBindingToPropertyWithNonPublicSetter = handleTwoWayBindingToPropertyWithNonPublicSetter;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00008FEC File Offset: 0x000071EC
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00008FF4 File Offset: 0x000071F4
		internal static bool UseSetWindowPosForTopmostWindows
		{
			get
			{
				return FrameworkCompatibilityPreferences._useSetWindowPosForTopmostWindows;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"UseSetWindowPosForTopmostWindows",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._useSetWindowPosForTopmostWindows = value;
				}
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00009060 File Offset: 0x00007260
		internal static bool GetUseSetWindowPosForTopmostWindows()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.UseSetWindowPosForTopmostWindows;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000906C File Offset: 0x0000726C
		private static void SetUseSetWindowPosForTopmostWindowsFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["UseSetWindowPosForTopmostWindows"];
			bool useSetWindowPosForTopmostWindows;
			if (bool.TryParse(value, out useSetWindowPosForTopmostWindows))
			{
				FrameworkCompatibilityPreferences.UseSetWindowPosForTopmostWindows = useSetWindowPosForTopmostWindows;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00009095 File Offset: 0x00007295
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000909C File Offset: 0x0000729C
		internal static bool VSP45Compat
		{
			get
			{
				return FrameworkCompatibilityPreferences._vsp45Compat;
			}
			set
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					if (FrameworkCompatibilityPreferences._isSealed)
					{
						throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
						{
							"IsVirtualizingStackPanel_45Compatible",
							"FrameworkCompatibilityPreferences"
						}));
					}
					FrameworkCompatibilityPreferences._vsp45Compat = value;
				}
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00009108 File Offset: 0x00007308
		internal static bool GetVSP45Compat()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.VSP45Compat;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00009114 File Offset: 0x00007314
		private static void SetVSP45CompatFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["IsVirtualizingStackPanel_45Compatible"];
			bool vsp45Compat;
			if (bool.TryParse(value, out vsp45Compat))
			{
				FrameworkCompatibilityPreferences.VSP45Compat = vsp45Compat;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000913D File Offset: 0x0000733D
		internal static string GetScrollingTraceTarget()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences._scrollingTraceTarget;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00009149 File Offset: 0x00007349
		internal static string GetScrollingTraceFile()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences._scrollingTraceFile;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00009155 File Offset: 0x00007355
		private static void SetScrollingTraceFromAppSettings(NameValueCollection appSettings)
		{
			FrameworkCompatibilityPreferences._scrollingTraceTarget = appSettings["ScrollingTraceTarget"];
			FrameworkCompatibilityPreferences._scrollingTraceFile = appSettings["ScrollingTraceFile"];
		}

		/// <summary>Gets or sets a value that indicates whether a failed copy or cut operation in a <see cref="T:System.Windows.Controls.Primitives.TextBoxBase" /> instance results in a <see cref="T:System.Runtime.InteropServices.ExternalException" />.</summary>
		/// <returns>
		///   <see langword="true" /> if a failed copy or cut operation in a <see cref="T:System.Windows.Controls.Primitives.TextBoxBase" /> instance results in a <see cref="T:System.Runtime.InteropServices.ExternalException" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000333 RID: 819 RVA: 0x00009177 File Offset: 0x00007377
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000917E File Offset: 0x0000737E
		public static bool ShouldThrowOnCopyOrCutFailure
		{
			get
			{
				return FrameworkCompatibilityPreferences._shouldThrowOnCopyOrCutFailure;
			}
			set
			{
				if (FrameworkCompatibilityPreferences._isSealed)
				{
					throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
					{
						"ShouldThrowOnCopyOrCutFailure",
						"FrameworkCompatibilityPreferences"
					}));
				}
				FrameworkCompatibilityPreferences._shouldThrowOnCopyOrCutFailure = value;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000091B3 File Offset: 0x000073B3
		internal static bool GetShouldThrowOnCopyOrCutFailure()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000091C0 File Offset: 0x000073C0
		private static void SetShouldThrowOnCopyOrCutFailuresFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["ShouldThrowOnCopyOrCutFailure"];
			bool shouldThrowOnCopyOrCutFailure;
			if (bool.TryParse(value, out shouldThrowOnCopyOrCutFailure))
			{
				FrameworkCompatibilityPreferences.ShouldThrowOnCopyOrCutFailure = shouldThrowOnCopyOrCutFailure;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000337 RID: 823 RVA: 0x000091E9 File Offset: 0x000073E9
		// (set) Token: 0x06000338 RID: 824 RVA: 0x000091F0 File Offset: 0x000073F0
		internal static bool DisableLegacyDangerousXamlDeserializationMode
		{
			get
			{
				return FrameworkCompatibilityPreferences._disableLegacyDangerousXamlDeserializationMode;
			}
			set
			{
				FrameworkCompatibilityPreferences._disableLegacyDangerousXamlDeserializationMode = value;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000091F8 File Offset: 0x000073F8
		private static void SetDisableLegacyDangerousXamlDeserializationMode(NameValueCollection appSettings)
		{
			if (appSettings == null || !FrameworkCompatibilityPreferences.SetDisableLegacyDangerousXamlDeserializationModeFromAppSettings(appSettings))
			{
				FrameworkCompatibilityPreferences.SetDisableLegacyDangerousXamlDeserializationModeFromRegistry();
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000920C File Offset: 0x0000740C
		private static bool SetDisableLegacyDangerousXamlDeserializationModeFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["DisableLegacyDangerousXamlDeserializationMode"];
			bool disableLegacyDangerousXamlDeserializationMode;
			if (bool.TryParse(value, out disableLegacyDangerousXamlDeserializationMode))
			{
				FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode = disableLegacyDangerousXamlDeserializationMode;
				return true;
			}
			return false;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00009238 File Offset: 0x00007438
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_CURRENT_USER\\Software\\Microsoft\\Avalon.Xaml\\", Unrestricted = true)]
		private static void SetDisableLegacyDangerousXamlDeserializationModeFromRegistry()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Avalon.Xaml\\", RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (registryKey != null && registryKey.GetValueKind("DisableLegacyDangerousXamlDeserializationMode") == RegistryValueKind.DWord)
					{
						object value = registryKey.GetValue("DisableLegacyDangerousXamlDeserializationMode");
						if (value != null)
						{
							FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode = ((int)value != 0);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000092B4 File Offset: 0x000074B4
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000092BB File Offset: 0x000074BB
		internal static bool ShouldThrowOnDataGridCopyOrCutFailure
		{
			get
			{
				return FrameworkCompatibilityPreferences._shouldThrowOnDataGridCopyOrCutFailure;
			}
			set
			{
				if (FrameworkCompatibilityPreferences._isSealed)
				{
					throw new InvalidOperationException(SR.Get("CompatibilityPreferencesSealed", new object[]
					{
						"ShouldThrowOnDataGridCopyOrCutFailure",
						"FrameworkCompatibilityPreferences"
					}));
				}
				FrameworkCompatibilityPreferences._shouldThrowOnDataGridCopyOrCutFailure = value;
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000092F0 File Offset: 0x000074F0
		internal static bool GetShouldThrowOnDataGridCopyOrCutFailure()
		{
			FrameworkCompatibilityPreferences.Seal();
			return FrameworkCompatibilityPreferences.ShouldThrowOnDataGridCopyOrCutFailure;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000092FC File Offset: 0x000074FC
		private static void SetShouldThrowOnDataGridCopyOrCutFailuresFromAppSettings(NameValueCollection appSettings)
		{
			string value = appSettings["ShouldThrowOnDataGridCopyOrCutFailure"];
			bool shouldThrowOnDataGridCopyOrCutFailure;
			if (bool.TryParse(value, out shouldThrowOnDataGridCopyOrCutFailure))
			{
				FrameworkCompatibilityPreferences.ShouldThrowOnDataGridCopyOrCutFailure = shouldThrowOnDataGridCopyOrCutFailure;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00009328 File Offset: 0x00007528
		private static void Seal()
		{
			if (!FrameworkCompatibilityPreferences._isSealed)
			{
				object lockObject = FrameworkCompatibilityPreferences._lockObject;
				lock (lockObject)
				{
					FrameworkCompatibilityPreferences._isSealed = true;
				}
			}
		}

		// Token: 0x040005C9 RID: 1481
		private static bool _targetsDesktop_V4_0;

		// Token: 0x040005CA RID: 1482
		private static bool _areInactiveSelectionHighlightBrushKeysSupported = BinaryCompatibility.TargetsAtLeast_Desktop_V4_5;

		// Token: 0x040005CB RID: 1483
		private static bool _keepTextBoxDisplaySynchronizedWithTextProperty = BinaryCompatibility.TargetsAtLeast_Desktop_V4_5;

		// Token: 0x040005CC RID: 1484
		private static FrameworkCompatibilityPreferences.HandleBindingOptions _handleTwoWayBindingToPropertyWithNonPublicSetter = (!SecurityHelper.IsFullTrustCaller()) ? FrameworkCompatibilityPreferences.HandleBindingOptions.Throw : ((BinaryCompatibility.AppWasBuiltForFramework != TargetFrameworkId.NetFramework) ? FrameworkCompatibilityPreferences.HandleBindingOptions.Disallow : ((BinaryCompatibility.AppWasBuiltForVersion == 40500) ? FrameworkCompatibilityPreferences.HandleBindingOptions.Allow : FrameworkCompatibilityPreferences.HandleBindingOptions.Throw));

		// Token: 0x040005CD RID: 1485
		private static bool _useSetWindowPosForTopmostWindows = false;

		// Token: 0x040005CE RID: 1486
		private static bool _vsp45Compat = false;

		// Token: 0x040005CF RID: 1487
		private static string _scrollingTraceTarget;

		// Token: 0x040005D0 RID: 1488
		private static string _scrollingTraceFile;

		// Token: 0x040005D1 RID: 1489
		private static bool _shouldThrowOnCopyOrCutFailure = false;

		// Token: 0x040005D2 RID: 1490
		private static bool _disableLegacyDangerousXamlDeserializationMode = true;

		// Token: 0x040005D3 RID: 1491
		private const string WpfXamlKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Avalon.Xaml\\";

		// Token: 0x040005D4 RID: 1492
		private const string WpfXamlSubKeyPath = "Software\\Microsoft\\Avalon.Xaml\\";

		// Token: 0x040005D5 RID: 1493
		private const string DisableLegacyDangerousXamlDeserializationModeName = "DisableLegacyDangerousXamlDeserializationMode";

		// Token: 0x040005D6 RID: 1494
		private static bool _shouldThrowOnDataGridCopyOrCutFailure = false;

		// Token: 0x040005D7 RID: 1495
		private static bool _isSealed;

		// Token: 0x040005D8 RID: 1496
		private static object _lockObject = new object();

		// Token: 0x02000813 RID: 2067
		internal enum HandleBindingOptions
		{
			// Token: 0x04003BAA RID: 15274
			Throw,
			// Token: 0x04003BAB RID: 15275
			Disallow,
			// Token: 0x04003BAC RID: 15276
			Allow
		}
	}
}
