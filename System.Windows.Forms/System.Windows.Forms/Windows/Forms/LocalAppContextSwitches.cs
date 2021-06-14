using System;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
	// Token: 0x020002D4 RID: 724
	internal static class LocalAppContextSwitches
	{
		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000CAEB6 File Offset: 0x000C90B6
		public static bool DontSupportReentrantFilterMessage
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.DontSupportReentrantFilterMessage", ref LocalAppContextSwitches._dontSupportReentrantFilterMessage);
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06002B5A RID: 11098 RVA: 0x000CAEC7 File Offset: 0x000C90C7
		public static bool DoNotSupportSelectAllShortcutInMultilineTextBox
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.DoNotSupportSelectAllShortcutInMultilineTextBox", ref LocalAppContextSwitches._doNotSupportSelectAllShortcutInMultilineTextBox);
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x000CAED8 File Offset: 0x000C90D8
		public static bool DoNotLoadLatestRichEditControl
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.DoNotLoadLatestRichEditControl", ref LocalAppContextSwitches._doNotLoadLatestRichEditControl);
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000CAEE9 File Offset: 0x000C90E9
		public static bool UseLegacyContextMenuStripSourceControlValue
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.UseLegacyContextMenuStripSourceControlValue", ref LocalAppContextSwitches._useLegacyContextMenuStripSourceControlValue);
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x000CAEFA File Offset: 0x000C90FA
		public static bool UseLegacyDomainUpDownControlScrolling
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.DomainUpDown.UseLegacyScrolling", ref LocalAppContextSwitches._useLegacyDomainUpDownScrolling);
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000CAF0B File Offset: 0x000C910B
		public static bool AllowUpdateChildControlIndexForTabControls
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.AllowUpdateChildControlIndexForTabControls", ref LocalAppContextSwitches._allowUpdateChildControlIndexForTabControls);
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x000CAF1C File Offset: 0x000C911C
		public static bool UseLegacyImages
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.UseLegacyImages", ref LocalAppContextSwitches._useLegacyImages);
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x000CAF2D File Offset: 0x000C912D
		public static bool EnableVisualStyleValidation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.EnableVisualStyleValidation", ref LocalAppContextSwitches._enableVisualStyleValidation);
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000CAF3E File Offset: 0x000C913E
		public static bool EnableLegacyDangerousClipboardDeserializationMode
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (LocalAppContextSwitches._enableLegacyDangerousClipboardDeserializationMode < 0)
				{
					return false;
				}
				if (LocalAppContextSwitches._enableLegacyDangerousClipboardDeserializationMode > 0)
				{
					return true;
				}
				if (UnsafeNativeMethods.IsDynamicCodePolicyEnabled())
				{
					LocalAppContextSwitches._enableLegacyDangerousClipboardDeserializationMode = -1;
				}
				else
				{
					LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.EnableLegacyDangerousClipboardDeserializationMode", ref LocalAppContextSwitches._enableLegacyDangerousClipboardDeserializationMode);
				}
				return LocalAppContextSwitches._enableLegacyDangerousClipboardDeserializationMode > 0;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000CAF7B File Offset: 0x000C917B
		public static bool EnableLegacyChineseIMEIndicator
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.EnableLegacyChineseIMEIndicator", ref LocalAppContextSwitches._enableLegacyChineseIMEIndicator);
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x000CAF8C File Offset: 0x000C918C
		public static bool EnableLegacyIMEFocusInComboBox
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Forms.EnableLegacyIMEFocusInComboBox", ref LocalAppContextSwitches._enableLegacyIMEFocusInComboBox);
			}
		}

		// Token: 0x04001294 RID: 4756
		internal const string DontSupportReentrantFilterMessageSwitchName = "Switch.System.Windows.Forms.DontSupportReentrantFilterMessage";

		// Token: 0x04001295 RID: 4757
		internal const string DoNotSupportSelectAllShortcutInMultilineTextBoxSwitchName = "Switch.System.Windows.Forms.DoNotSupportSelectAllShortcutInMultilineTextBox";

		// Token: 0x04001296 RID: 4758
		internal const string DoNotLoadLatestRichEditControlSwitchName = "Switch.System.Windows.Forms.DoNotLoadLatestRichEditControl";

		// Token: 0x04001297 RID: 4759
		internal const string UseLegacyContextMenuStripSourceControlValueSwitchName = "Switch.System.Windows.Forms.UseLegacyContextMenuStripSourceControlValue";

		// Token: 0x04001298 RID: 4760
		internal const string DomainUpDownUseLegacyScrollingSwitchName = "Switch.System.Windows.Forms.DomainUpDown.UseLegacyScrolling";

		// Token: 0x04001299 RID: 4761
		internal const string AllowUpdateChildControlIndexForTabControlsSwitchName = "Switch.System.Windows.Forms.AllowUpdateChildControlIndexForTabControls";

		// Token: 0x0400129A RID: 4762
		internal const string UseLegacyImagesSwitchName = "Switch.System.Windows.Forms.UseLegacyImages";

		// Token: 0x0400129B RID: 4763
		internal const string EnableVisualStyleValidationSwitchName = "Switch.System.Windows.Forms.EnableVisualStyleValidation";

		// Token: 0x0400129C RID: 4764
		internal const string EnableLegacyDangerousClipboardDeserializationModeSwitchName = "Switch.System.Windows.Forms.EnableLegacyDangerousClipboardDeserializationMode";

		// Token: 0x0400129D RID: 4765
		internal const string EnableLegacyChineseIMEIndicatorSwitchName = "Switch.System.Windows.Forms.EnableLegacyChineseIMEIndicator";

		// Token: 0x0400129E RID: 4766
		internal const string EnableLegacyIMEFocusInComboBoxSwitchName = "Switch.System.Windows.Forms.EnableLegacyIMEFocusInComboBox";

		// Token: 0x0400129F RID: 4767
		private static int _dontSupportReentrantFilterMessage;

		// Token: 0x040012A0 RID: 4768
		private static int _doNotSupportSelectAllShortcutInMultilineTextBox;

		// Token: 0x040012A1 RID: 4769
		private static int _doNotLoadLatestRichEditControl;

		// Token: 0x040012A2 RID: 4770
		private static int _useLegacyContextMenuStripSourceControlValue;

		// Token: 0x040012A3 RID: 4771
		private static int _useLegacyDomainUpDownScrolling;

		// Token: 0x040012A4 RID: 4772
		private static int _allowUpdateChildControlIndexForTabControls;

		// Token: 0x040012A5 RID: 4773
		private static int _useLegacyImages;

		// Token: 0x040012A6 RID: 4774
		private static int _enableVisualStyleValidation;

		// Token: 0x040012A7 RID: 4775
		private static int _enableLegacyDangerousClipboardDeserializationMode;

		// Token: 0x040012A8 RID: 4776
		private static int _enableLegacyChineseIMEIndicator;

		// Token: 0x040012A9 RID: 4777
		private static int _enableLegacyIMEFocusInComboBox;
	}
}
