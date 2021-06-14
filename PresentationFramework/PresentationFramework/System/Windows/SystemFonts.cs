using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace System.Windows
{
	/// <summary>Contains properties that expose the system resources that concern fonts. </summary>
	// Token: 0x0200010F RID: 271
	public static class SystemFonts
	{
		/// <summary>Gets the font size from the logical font information for the current icon-title font. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00021DAE File Offset: 0x0001FFAE
		public static double IconFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.IconMetrics.lfFont.lfHeight);
			}
		}

		/// <summary>Gets the font family from the logical font information for the current icon-title font.  </summary>
		/// <returns>A font family.</returns>
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00021DC4 File Offset: 0x0001FFC4
		public static FontFamily IconFontFamily
		{
			get
			{
				if (SystemFonts._iconFontFamily == null)
				{
					SystemFonts._iconFontFamily = new FontFamily(SystemParameters.IconMetrics.lfFont.lfFaceName);
				}
				return SystemFonts._iconFontFamily;
			}
		}

		/// <summary>Gets the font style from the logical font information for the current icon-title font. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00021DEB File Offset: 0x0001FFEB
		public static FontStyle IconFontStyle
		{
			get
			{
				if (SystemParameters.IconMetrics.lfFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the font weight from the logical font information for the current icon-title font. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x00021E09 File Offset: 0x00020009
		public static FontWeight IconFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.IconMetrics.lfFont.lfWeight);
			}
		}

		/// <summary>Gets the text decorations from the logical font information for the current icon-title font. </summary>
		/// <returns>A collection of text decorations.</returns>
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00021E20 File Offset: 0x00020020
		public static TextDecorationCollection IconFontTextDecorations
		{
			get
			{
				if (SystemFonts._iconFontTextDecorations == null)
				{
					SystemFonts._iconFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.IconMetrics.lfFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._iconFontTextDecorations);
					}
					if (SystemParameters.IconMetrics.lfFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._iconFontTextDecorations);
					}
					SystemFonts._iconFontTextDecorations.Freeze();
				}
				return SystemFonts._iconFontTextDecorations;
			}
		}

		/// <summary>Gets the metric that determines the caption font-size for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x00021E8D File Offset: 0x0002008D
		public static double CaptionFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfCaptionFont.lfHeight);
			}
		}

		/// <summary>Gets the metric that determines the font family of the caption of the nonclient area of a nonminimized window. </summary>
		/// <returns>A font family.</returns>
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00021EA3 File Offset: 0x000200A3
		public static FontFamily CaptionFontFamily
		{
			get
			{
				if (SystemFonts._captionFontFamily == null)
				{
					SystemFonts._captionFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfCaptionFont.lfFaceName);
				}
				return SystemFonts._captionFontFamily;
			}
		}

		/// <summary>Gets the metric that determines the caption font-style for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00021ECA File Offset: 0x000200CA
		public static FontStyle CaptionFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfCaptionFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the metric that determines the caption font-weight for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00021EE8 File Offset: 0x000200E8
		public static FontWeight CaptionFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfCaptionFont.lfWeight);
			}
		}

		/// <summary>Gets the metric that determines the caption text-decorations for the nonclient area of a nonminimized window. </summary>
		/// <returns>A collection of text decorations.</returns>
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00021F00 File Offset: 0x00020100
		public static TextDecorationCollection CaptionFontTextDecorations
		{
			get
			{
				if (SystemFonts._captionFontTextDecorations == null)
				{
					SystemFonts._captionFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfCaptionFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._captionFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfCaptionFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._captionFontTextDecorations);
					}
					SystemFonts._captionFontTextDecorations.Freeze();
				}
				return SystemFonts._captionFontTextDecorations;
			}
		}

		/// <summary>Gets the metric that determines the font size of the small-caption text for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00021F6D File Offset: 0x0002016D
		public static double SmallCaptionFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfHeight);
			}
		}

		/// <summary>Gets the metric that determines the font family of the small-caption text for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font family.</returns>
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00021F83 File Offset: 0x00020183
		public static FontFamily SmallCaptionFontFamily
		{
			get
			{
				if (SystemFonts._smallCaptionFontFamily == null)
				{
					SystemFonts._smallCaptionFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfFaceName);
				}
				return SystemFonts._smallCaptionFontFamily;
			}
		}

		/// <summary>Gets the metric that determines the font style of the small-caption text for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00021FAA File Offset: 0x000201AA
		public static FontStyle SmallCaptionFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the metric that determines the font weight of the small-caption text for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00021FC8 File Offset: 0x000201C8
		public static FontWeight SmallCaptionFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfSmCaptionFont.lfWeight);
			}
		}

		/// <summary>Gets the metric that determines the decorations of the small-caption text for the nonclient area of a nonminimized window. </summary>
		/// <returns>A collection of text decorations.</returns>
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00021FE0 File Offset: 0x000201E0
		public static TextDecorationCollection SmallCaptionFontTextDecorations
		{
			get
			{
				if (SystemFonts._smallCaptionFontTextDecorations == null)
				{
					SystemFonts._smallCaptionFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._smallCaptionFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfSmCaptionFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._smallCaptionFontTextDecorations);
					}
					SystemFonts._smallCaptionFontTextDecorations.Freeze();
				}
				return SystemFonts._smallCaptionFontTextDecorations;
			}
		}

		/// <summary>Gets the metric that determines the font size of menu text. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002204D File Offset: 0x0002024D
		public static double MenuFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfMenuFont.lfHeight);
			}
		}

		/// <summary>Gets the metric that determines the font family for menu text. </summary>
		/// <returns>A font family.</returns>
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00022063 File Offset: 0x00020263
		public static FontFamily MenuFontFamily
		{
			get
			{
				if (SystemFonts._menuFontFamily == null)
				{
					SystemFonts._menuFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfMenuFont.lfFaceName);
				}
				return SystemFonts._menuFontFamily;
			}
		}

		/// <summary>Gets the metric that determines the font style for menu text. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002208A File Offset: 0x0002028A
		public static FontStyle MenuFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfMenuFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the metric that determines the font weight for menu text. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x000220A8 File Offset: 0x000202A8
		public static FontWeight MenuFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfMenuFont.lfWeight);
			}
		}

		/// <summary>Gets the metric that determines the text decorations for menu text. </summary>
		/// <returns>A collection of text decorations.</returns>
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x000220C0 File Offset: 0x000202C0
		public static TextDecorationCollection MenuFontTextDecorations
		{
			get
			{
				if (SystemFonts._menuFontTextDecorations == null)
				{
					SystemFonts._menuFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfMenuFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._menuFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfMenuFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._menuFontTextDecorations);
					}
					SystemFonts._menuFontTextDecorations.Freeze();
				}
				return SystemFonts._menuFontTextDecorations;
			}
		}

		/// <summary>Gets the metric that determines the font size of the text used in status bars and ToolTips for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002212D File Offset: 0x0002032D
		public static double StatusFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfStatusFont.lfHeight);
			}
		}

		/// <summary>Gets the metric that determines the font family of the text used in status bars and ToolTips for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font family.</returns>
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00022143 File Offset: 0x00020343
		public static FontFamily StatusFontFamily
		{
			get
			{
				if (SystemFonts._statusFontFamily == null)
				{
					SystemFonts._statusFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfStatusFont.lfFaceName);
				}
				return SystemFonts._statusFontFamily;
			}
		}

		/// <summary>Gets the metric that determines the font style of the text used in status bars and ToolTips for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002216A File Offset: 0x0002036A
		public static FontStyle StatusFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfStatusFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the metric that determines the font weight of the text used in status bars and ToolTips for the nonclient area of a nonminimized window. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00022188 File Offset: 0x00020388
		public static FontWeight StatusFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfStatusFont.lfWeight);
			}
		}

		/// <summary>Gets the metric that determines the decorations of the text used in status bars and ToolTips for the nonclient area of a nonminimized window. </summary>
		/// <returns>A collection of text decoration.</returns>
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x000221A0 File Offset: 0x000203A0
		public static TextDecorationCollection StatusFontTextDecorations
		{
			get
			{
				if (SystemFonts._statusFontTextDecorations == null)
				{
					SystemFonts._statusFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfStatusFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._statusFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfStatusFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._statusFontTextDecorations);
					}
					SystemFonts._statusFontTextDecorations.Freeze();
				}
				return SystemFonts._statusFontTextDecorations;
			}
		}

		/// <summary>Gets the metric that determines the font size of message box text. </summary>
		/// <returns>A font size.</returns>
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0002220D File Offset: 0x0002040D
		public static double MessageFontSize
		{
			get
			{
				return SystemFonts.ConvertFontHeight(SystemParameters.NonClientMetrics.lfMessageFont.lfHeight);
			}
		}

		/// <summary>Gets the metric that determines the font family for message box text. </summary>
		/// <returns>A font family.</returns>
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00022223 File Offset: 0x00020423
		public static FontFamily MessageFontFamily
		{
			get
			{
				if (SystemFonts._messageFontFamily == null)
				{
					SystemFonts._messageFontFamily = new FontFamily(SystemParameters.NonClientMetrics.lfMessageFont.lfFaceName);
				}
				return SystemFonts._messageFontFamily;
			}
		}

		/// <summary>Gets the metric that determines the font style for message box text. </summary>
		/// <returns>A font style.</returns>
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0002224A File Offset: 0x0002044A
		public static FontStyle MessageFontStyle
		{
			get
			{
				if (SystemParameters.NonClientMetrics.lfMessageFont.lfItalic == 0)
				{
					return FontStyles.Normal;
				}
				return FontStyles.Italic;
			}
		}

		/// <summary>Gets the metric that determines the font weight for message box text. </summary>
		/// <returns>A font weight.</returns>
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00022268 File Offset: 0x00020468
		public static FontWeight MessageFontWeight
		{
			get
			{
				return FontWeight.FromOpenTypeWeight(SystemParameters.NonClientMetrics.lfMessageFont.lfWeight);
			}
		}

		/// <summary>Gets the metric that determines the decorations for message box text. </summary>
		/// <returns>A collection of text decorations.</returns>
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00022280 File Offset: 0x00020480
		public static TextDecorationCollection MessageFontTextDecorations
		{
			get
			{
				if (SystemFonts._messageFontTextDecorations == null)
				{
					SystemFonts._messageFontTextDecorations = new TextDecorationCollection();
					if (SystemParameters.NonClientMetrics.lfMessageFont.lfUnderline != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Underline, SystemFonts._messageFontTextDecorations);
					}
					if (SystemParameters.NonClientMetrics.lfMessageFont.lfStrikeOut != 0)
					{
						SystemFonts.CopyTextDecorationCollection(TextDecorations.Strikethrough, SystemFonts._messageFontTextDecorations);
					}
					SystemFonts._messageFontTextDecorations.Freeze();
				}
				return SystemFonts._messageFontTextDecorations;
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000222F0 File Offset: 0x000204F0
		private static void CopyTextDecorationCollection(TextDecorationCollection from, TextDecorationCollection to)
		{
			int count = from.Count;
			for (int i = 0; i < count; i++)
			{
				to.Add(from[i]);
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00021143 File Offset: 0x0001F343
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.IconFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002231D File Offset: 0x0002051D
		public static ResourceKey IconFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheIconFontSize == null)
				{
					SystemFonts._cacheIconFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontSize);
				}
				return SystemFonts._cacheIconFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.IconFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x00022337 File Offset: 0x00020537
		public static ResourceKey IconFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheIconFontFamily == null)
				{
					SystemFonts._cacheIconFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontFamily);
				}
				return SystemFonts._cacheIconFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.IconFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00022351 File Offset: 0x00020551
		public static ResourceKey IconFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheIconFontStyle == null)
				{
					SystemFonts._cacheIconFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontStyle);
				}
				return SystemFonts._cacheIconFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.IconFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0002236B File Offset: 0x0002056B
		public static ResourceKey IconFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheIconFontWeight == null)
				{
					SystemFonts._cacheIconFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontWeight);
				}
				return SystemFonts._cacheIconFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.IconFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00022385 File Offset: 0x00020585
		public static ResourceKey IconFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheIconFontTextDecorations == null)
				{
					SystemFonts._cacheIconFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.IconFontTextDecorations);
				}
				return SystemFonts._cacheIconFontTextDecorations;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.CaptionFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002239F File Offset: 0x0002059F
		public static ResourceKey CaptionFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontSize == null)
				{
					SystemFonts._cacheCaptionFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontSize);
				}
				return SystemFonts._cacheCaptionFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.CaptionFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x000223B9 File Offset: 0x000205B9
		public static ResourceKey CaptionFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontFamily == null)
				{
					SystemFonts._cacheCaptionFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontFamily);
				}
				return SystemFonts._cacheCaptionFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.CaptionFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x000223D3 File Offset: 0x000205D3
		public static ResourceKey CaptionFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontStyle == null)
				{
					SystemFonts._cacheCaptionFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontStyle);
				}
				return SystemFonts._cacheCaptionFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.CaptionFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x000223ED File Offset: 0x000205ED
		public static ResourceKey CaptionFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontWeight == null)
				{
					SystemFonts._cacheCaptionFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontWeight);
				}
				return SystemFonts._cacheCaptionFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.CaptionFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00022407 File Offset: 0x00020607
		public static ResourceKey CaptionFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheCaptionFontTextDecorations == null)
				{
					SystemFonts._cacheCaptionFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.CaptionFontTextDecorations);
				}
				return SystemFonts._cacheCaptionFontTextDecorations;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.SmallCaptionFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00022421 File Offset: 0x00020621
		public static ResourceKey SmallCaptionFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontSize == null)
				{
					SystemFonts._cacheSmallCaptionFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontSize);
				}
				return SystemFonts._cacheSmallCaptionFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.SmallCaptionFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0002243B File Offset: 0x0002063B
		public static ResourceKey SmallCaptionFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontFamily == null)
				{
					SystemFonts._cacheSmallCaptionFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontFamily);
				}
				return SystemFonts._cacheSmallCaptionFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.SmallCaptionFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00022455 File Offset: 0x00020655
		public static ResourceKey SmallCaptionFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontStyle == null)
				{
					SystemFonts._cacheSmallCaptionFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontStyle);
				}
				return SystemFonts._cacheSmallCaptionFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.SmallCaptionFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0002246F File Offset: 0x0002066F
		public static ResourceKey SmallCaptionFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontWeight == null)
				{
					SystemFonts._cacheSmallCaptionFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontWeight);
				}
				return SystemFonts._cacheSmallCaptionFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.SmallCaptionFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00022489 File Offset: 0x00020689
		public static ResourceKey SmallCaptionFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheSmallCaptionFontTextDecorations == null)
				{
					SystemFonts._cacheSmallCaptionFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.SmallCaptionFontTextDecorations);
				}
				return SystemFonts._cacheSmallCaptionFontTextDecorations;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MenuFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000224A3 File Offset: 0x000206A3
		public static ResourceKey MenuFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontSize == null)
				{
					SystemFonts._cacheMenuFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontSize);
				}
				return SystemFonts._cacheMenuFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MenuFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x000224BD File Offset: 0x000206BD
		public static ResourceKey MenuFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontFamily == null)
				{
					SystemFonts._cacheMenuFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontFamily);
				}
				return SystemFonts._cacheMenuFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MenuFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x000224D7 File Offset: 0x000206D7
		public static ResourceKey MenuFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontStyle == null)
				{
					SystemFonts._cacheMenuFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontStyle);
				}
				return SystemFonts._cacheMenuFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MenuFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x000224F1 File Offset: 0x000206F1
		public static ResourceKey MenuFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontWeight == null)
				{
					SystemFonts._cacheMenuFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontWeight);
				}
				return SystemFonts._cacheMenuFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MenuFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002250B File Offset: 0x0002070B
		public static ResourceKey MenuFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheMenuFontTextDecorations == null)
				{
					SystemFonts._cacheMenuFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.MenuFontTextDecorations);
				}
				return SystemFonts._cacheMenuFontTextDecorations;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.StatusFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00022525 File Offset: 0x00020725
		public static ResourceKey StatusFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontSize == null)
				{
					SystemFonts._cacheStatusFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontSize);
				}
				return SystemFonts._cacheStatusFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.StatusFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002253F File Offset: 0x0002073F
		public static ResourceKey StatusFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontFamily == null)
				{
					SystemFonts._cacheStatusFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontFamily);
				}
				return SystemFonts._cacheStatusFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.StatusFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00022559 File Offset: 0x00020759
		public static ResourceKey StatusFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontStyle == null)
				{
					SystemFonts._cacheStatusFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontStyle);
				}
				return SystemFonts._cacheStatusFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.StatusFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00022573 File Offset: 0x00020773
		public static ResourceKey StatusFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontWeight == null)
				{
					SystemFonts._cacheStatusFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontWeight);
				}
				return SystemFonts._cacheStatusFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.StatusFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0002258D File Offset: 0x0002078D
		public static ResourceKey StatusFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheStatusFontTextDecorations == null)
				{
					SystemFonts._cacheStatusFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.StatusFontTextDecorations);
				}
				return SystemFonts._cacheStatusFontTextDecorations;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MessageFontSize" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x000225A7 File Offset: 0x000207A7
		public static ResourceKey MessageFontSizeKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontSize == null)
				{
					SystemFonts._cacheMessageFontSize = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontSize);
				}
				return SystemFonts._cacheMessageFontSize;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MessageFontFamily" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x000225C1 File Offset: 0x000207C1
		public static ResourceKey MessageFontFamilyKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontFamily == null)
				{
					SystemFonts._cacheMessageFontFamily = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontFamily);
				}
				return SystemFonts._cacheMessageFontFamily;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MessageFontStyle" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x000225DB File Offset: 0x000207DB
		public static ResourceKey MessageFontStyleKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontStyle == null)
				{
					SystemFonts._cacheMessageFontStyle = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontStyle);
				}
				return SystemFonts._cacheMessageFontStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MessageFontWeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x000225F5 File Offset: 0x000207F5
		public static ResourceKey MessageFontWeightKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontWeight == null)
				{
					SystemFonts._cacheMessageFontWeight = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontWeight);
				}
				return SystemFonts._cacheMessageFontWeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemFonts.MessageFontTextDecorations" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0002260F File Offset: 0x0002080F
		public static ResourceKey MessageFontTextDecorationsKey
		{
			get
			{
				if (SystemFonts._cacheMessageFontTextDecorations == null)
				{
					SystemFonts._cacheMessageFontTextDecorations = SystemFonts.CreateInstance(SystemResourceKeyID.MessageFontTextDecorations);
				}
				return SystemFonts._cacheMessageFontTextDecorations;
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0002262C File Offset: 0x0002082C
		private static double ConvertFontHeight(int height)
		{
			int dpi = SystemParameters.Dpi;
			if (dpi != 0)
			{
				return (double)(Math.Abs(height) * 96 / dpi);
			}
			return 11.0;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00022658 File Offset: 0x00020858
		internal static void InvalidateIconMetrics()
		{
			SystemFonts._iconFontTextDecorations = null;
			SystemFonts._iconFontFamily = null;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00022666 File Offset: 0x00020866
		internal static void InvalidateNonClientMetrics()
		{
			SystemFonts._messageFontTextDecorations = null;
			SystemFonts._statusFontTextDecorations = null;
			SystemFonts._menuFontTextDecorations = null;
			SystemFonts._smallCaptionFontTextDecorations = null;
			SystemFonts._captionFontTextDecorations = null;
			SystemFonts._messageFontFamily = null;
			SystemFonts._statusFontFamily = null;
			SystemFonts._menuFontFamily = null;
			SystemFonts._smallCaptionFontFamily = null;
			SystemFonts._captionFontFamily = null;
		}

		// Token: 0x0400086D RID: 2157
		private const double FallbackFontSize = 11.0;

		// Token: 0x0400086E RID: 2158
		private static TextDecorationCollection _iconFontTextDecorations;

		// Token: 0x0400086F RID: 2159
		private static TextDecorationCollection _messageFontTextDecorations;

		// Token: 0x04000870 RID: 2160
		private static TextDecorationCollection _statusFontTextDecorations;

		// Token: 0x04000871 RID: 2161
		private static TextDecorationCollection _menuFontTextDecorations;

		// Token: 0x04000872 RID: 2162
		private static TextDecorationCollection _smallCaptionFontTextDecorations;

		// Token: 0x04000873 RID: 2163
		private static TextDecorationCollection _captionFontTextDecorations;

		// Token: 0x04000874 RID: 2164
		private static FontFamily _iconFontFamily;

		// Token: 0x04000875 RID: 2165
		private static FontFamily _messageFontFamily;

		// Token: 0x04000876 RID: 2166
		private static FontFamily _statusFontFamily;

		// Token: 0x04000877 RID: 2167
		private static FontFamily _menuFontFamily;

		// Token: 0x04000878 RID: 2168
		private static FontFamily _smallCaptionFontFamily;

		// Token: 0x04000879 RID: 2169
		private static FontFamily _captionFontFamily;

		// Token: 0x0400087A RID: 2170
		private static SystemResourceKey _cacheIconFontSize;

		// Token: 0x0400087B RID: 2171
		private static SystemResourceKey _cacheIconFontFamily;

		// Token: 0x0400087C RID: 2172
		private static SystemResourceKey _cacheIconFontStyle;

		// Token: 0x0400087D RID: 2173
		private static SystemResourceKey _cacheIconFontWeight;

		// Token: 0x0400087E RID: 2174
		private static SystemResourceKey _cacheIconFontTextDecorations;

		// Token: 0x0400087F RID: 2175
		private static SystemResourceKey _cacheCaptionFontSize;

		// Token: 0x04000880 RID: 2176
		private static SystemResourceKey _cacheCaptionFontFamily;

		// Token: 0x04000881 RID: 2177
		private static SystemResourceKey _cacheCaptionFontStyle;

		// Token: 0x04000882 RID: 2178
		private static SystemResourceKey _cacheCaptionFontWeight;

		// Token: 0x04000883 RID: 2179
		private static SystemResourceKey _cacheCaptionFontTextDecorations;

		// Token: 0x04000884 RID: 2180
		private static SystemResourceKey _cacheSmallCaptionFontSize;

		// Token: 0x04000885 RID: 2181
		private static SystemResourceKey _cacheSmallCaptionFontFamily;

		// Token: 0x04000886 RID: 2182
		private static SystemResourceKey _cacheSmallCaptionFontStyle;

		// Token: 0x04000887 RID: 2183
		private static SystemResourceKey _cacheSmallCaptionFontWeight;

		// Token: 0x04000888 RID: 2184
		private static SystemResourceKey _cacheSmallCaptionFontTextDecorations;

		// Token: 0x04000889 RID: 2185
		private static SystemResourceKey _cacheMenuFontSize;

		// Token: 0x0400088A RID: 2186
		private static SystemResourceKey _cacheMenuFontFamily;

		// Token: 0x0400088B RID: 2187
		private static SystemResourceKey _cacheMenuFontStyle;

		// Token: 0x0400088C RID: 2188
		private static SystemResourceKey _cacheMenuFontWeight;

		// Token: 0x0400088D RID: 2189
		private static SystemResourceKey _cacheMenuFontTextDecorations;

		// Token: 0x0400088E RID: 2190
		private static SystemResourceKey _cacheStatusFontSize;

		// Token: 0x0400088F RID: 2191
		private static SystemResourceKey _cacheStatusFontFamily;

		// Token: 0x04000890 RID: 2192
		private static SystemResourceKey _cacheStatusFontStyle;

		// Token: 0x04000891 RID: 2193
		private static SystemResourceKey _cacheStatusFontWeight;

		// Token: 0x04000892 RID: 2194
		private static SystemResourceKey _cacheStatusFontTextDecorations;

		// Token: 0x04000893 RID: 2195
		private static SystemResourceKey _cacheMessageFontSize;

		// Token: 0x04000894 RID: 2196
		private static SystemResourceKey _cacheMessageFontFamily;

		// Token: 0x04000895 RID: 2197
		private static SystemResourceKey _cacheMessageFontStyle;

		// Token: 0x04000896 RID: 2198
		private static SystemResourceKey _cacheMessageFontWeight;

		// Token: 0x04000897 RID: 2199
		private static SystemResourceKey _cacheMessageFontTextDecorations;
	}
}
