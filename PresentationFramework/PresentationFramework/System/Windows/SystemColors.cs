using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using MS.Win32;

namespace System.Windows
{
	/// <summary>Contains system colors, system brushes, and system resource keys that correspond to system display elements. </summary>
	// Token: 0x0200010D RID: 269
	public static class SystemColors
	{
		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the active window's border. </summary>
		/// <returns>The color of the active window's border.</returns>
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0002103E File Offset: 0x0001F23E
		public static Color ActiveBorderColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color of the active window's title bar. </summary>
		/// <returns>The background color of the active window's title bar.</returns>
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00021046 File Offset: 0x0001F246
		public static Color ActiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the text in the active window's title bar. </summary>
		/// <returns>The color of the active window's title bar.</returns>
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0002104E File Offset: 0x0001F24E
		public static Color ActiveCaptionTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ActiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the application workspace. </summary>
		/// <returns>The color of the application workspace.</returns>
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00021056 File Offset: 0x0001F256
		public static Color AppWorkspaceColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.AppWorkspace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the face color of a three-dimensional display element. </summary>
		/// <returns>The face color of a three-dimensional display element.</returns>
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0002105E File Offset: 0x0001F25E
		public static Color ControlColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Control);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the shadow color of a three-dimensional display element. </summary>
		/// <returns>The shadow color of a three-dimensional display element.</returns>
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00021066 File Offset: 0x0001F266
		public static Color ControlDarkColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the dark shadow color of a three-dimensional display element. </summary>
		/// <returns>The dark shadow color of a three-dimensional display element.</returns>
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0002106E File Offset: 0x0001F26E
		public static Color ControlDarkDarkColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlDarkDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the light color of a three-dimensional display element. </summary>
		/// <returns>The light color of a three-dimensional display element.</returns>
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00021076 File Offset: 0x0001F276
		public static Color ControlLightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the highlight color of a three-dimensional display element. </summary>
		/// <returns>The highlight color of a three-dimensional display element.</returns>
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0002107E File Offset: 0x0001F27E
		public static Color ControlLightLightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlLightLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of text in a three-dimensional display element. </summary>
		/// <returns>The color of text in a three-dimensional display element.</returns>
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00021086 File Offset: 0x0001F286
		public static Color ControlTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ControlText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the desktop. </summary>
		/// <returns>The color of the desktop.</returns>
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0002108F File Offset: 0x0001F28F
		public static Color DesktopColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Desktop);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the right side color in the gradient of an active window's title bar. </summary>
		/// <returns>The right side color in the gradient.</returns>
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00021098 File Offset: 0x0001F298
		public static Color GradientActiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GradientActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the right side color in the gradient of an inactive window's title bar. </summary>
		/// <returns>The right side color in the gradient.</returns>
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x000210A1 File Offset: 0x0001F2A1
		public static Color GradientInactiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GradientInactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of disabled text. </summary>
		/// <returns>The color of disabled text.</returns>
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x000210AA File Offset: 0x0001F2AA
		public static Color GrayTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.GrayText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color of selected items. </summary>
		/// <returns>The background color of selected items.</returns>
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x000210B3 File Offset: 0x0001F2B3
		public static Color HighlightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Highlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the text of selected items. </summary>
		/// <returns>The color of the text of selected items.</returns>
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x000210BC File Offset: 0x0001F2BC
		public static Color HighlightTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.HighlightText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color used to designate a hot-tracked item. </summary>
		/// <returns>The color used to designate a hot-tracked item.</returns>
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x000210C5 File Offset: 0x0001F2C5
		public static Color HotTrackColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.HotTrack);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of an inactive window's border. </summary>
		/// <returns>The color of an inactive window's border.</returns>
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x000210CE File Offset: 0x0001F2CE
		public static Color InactiveBorderColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color of an inactive window's title bar. </summary>
		/// <returns>The background color of an inactive window's title bar.</returns>
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x000210D7 File Offset: 0x0001F2D7
		public static Color InactiveCaptionColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the text of an inactive window's title bar. </summary>
		/// <returns>The color of the text of an inactive window's title bar.</returns>
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x000210E0 File Offset: 0x0001F2E0
		public static Color InactiveCaptionTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InactiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color for the <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The background color for the <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x000210E9 File Offset: 0x0001F2E9
		public static Color InfoColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Info);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the text color for the <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The text color for the <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x000210F2 File Offset: 0x0001F2F2
		public static Color InfoTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.InfoText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of a menu's background. </summary>
		/// <returns>The color of a menu's background.</returns>
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x000210FB File Offset: 0x0001F2FB
		public static Color MenuColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Menu);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color for a menu bar. </summary>
		/// <returns>The background color for a menu bar.</returns>
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x00021104 File Offset: 0x0001F304
		public static Color MenuBarColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color used to highlight a menu item. </summary>
		/// <returns>The color used to highlight a menu item.</returns>
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0002110D File Offset: 0x0001F30D
		public static Color MenuHighlightColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of a menu's text. </summary>
		/// <returns>The color of a menu's text.</returns>
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x00021116 File Offset: 0x0001F316
		public static Color MenuTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.MenuText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color of a scroll bar. </summary>
		/// <returns>The background color of a scroll bar.</returns>
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0002111F File Offset: 0x0001F31F
		public static Color ScrollBarColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.ScrollBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the background color in the client area of a window. </summary>
		/// <returns>The background color in the client area of a window.</returns>
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00021128 File Offset: 0x0001F328
		public static Color WindowColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.Window);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of a window frame. </summary>
		/// <returns>The color of a window frame.</returns>
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00021131 File Offset: 0x0001F331
		public static Color WindowFrameColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.WindowFrame);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Color" /> structure that is the color of the text in the client area of a window. </summary>
		/// <returns>The color of the text in the client area of a window.</returns>
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0002113A File Offset: 0x0001F33A
		public static Color WindowTextColor
		{
			get
			{
				return SystemColors.GetSystemColor(SystemColors.CacheSlot.WindowText);
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00021143 File Offset: 0x0001F343
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the active window's border. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the active window's border.</returns>
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0002114B File Offset: 0x0001F34B
		public static ResourceKey ActiveBorderColorKey
		{
			get
			{
				if (SystemColors._cacheActiveBorderColor == null)
				{
					SystemColors._cacheActiveBorderColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveBorderColor);
				}
				return SystemColors._cacheActiveBorderColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of the active window's title bar.</summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of the active window's title bar.</returns>
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00021165 File Offset: 0x0001F365
		public static ResourceKey ActiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionColor == null)
				{
					SystemColors._cacheActiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionColor);
				}
				return SystemColors._cacheActiveCaptionColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the text in the active window's title bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the text in the active window's title bar.</returns>
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0002117F File Offset: 0x0001F37F
		public static ResourceKey ActiveCaptionTextColorKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionTextColor == null)
				{
					SystemColors._cacheActiveCaptionTextColor = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionTextColor);
				}
				return SystemColors._cacheActiveCaptionTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the application workspace. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the application workspace.</returns>
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00021199 File Offset: 0x0001F399
		public static ResourceKey AppWorkspaceColorKey
		{
			get
			{
				if (SystemColors._cacheAppWorkspaceColor == null)
				{
					SystemColors._cacheAppWorkspaceColor = SystemColors.CreateInstance(SystemResourceKeyID.AppWorkspaceColor);
				}
				return SystemColors._cacheAppWorkspaceColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the face <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element. </summary>
		/// <returns>The resource key for the face <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element.</returns>
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x000211B3 File Offset: 0x0001F3B3
		public static ResourceKey ControlColorKey
		{
			get
			{
				if (SystemColors._cacheControlColor == null)
				{
					SystemColors._cacheControlColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlColor);
				}
				return SystemColors._cacheControlColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the shadow <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element. </summary>
		/// <returns>The resource key for the shadow <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element.</returns>
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x000211CD File Offset: 0x0001F3CD
		public static ResourceKey ControlDarkColorKey
		{
			get
			{
				if (SystemColors._cacheControlDarkColor == null)
				{
					SystemColors._cacheControlDarkColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkColor);
				}
				return SystemColors._cacheControlDarkColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the dark shadow <see cref="T:System.Windows.Media.Color" /> of the highlight color of a three-dimensional display element. </summary>
		/// <returns>The resource key for the dark shadow <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element.</returns>
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x000211E7 File Offset: 0x0001F3E7
		public static ResourceKey ControlDarkDarkColorKey
		{
			get
			{
				if (SystemColors._cacheControlDarkDarkColor == null)
				{
					SystemColors._cacheControlDarkDarkColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkDarkColor);
				}
				return SystemColors._cacheControlDarkDarkColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the highlight <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element. </summary>
		/// <returns>The resource key for the highlight <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element.</returns>
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00021201 File Offset: 0x0001F401
		public static ResourceKey ControlLightColorKey
		{
			get
			{
				if (SystemColors._cacheControlLightColor == null)
				{
					SystemColors._cacheControlLightColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightColor);
				}
				return SystemColors._cacheControlLightColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the highlight <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element. </summary>
		/// <returns>The resource key for the highlight <see cref="T:System.Windows.Media.Color" /> of a three-dimensional display element.</returns>
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0002121B File Offset: 0x0001F41B
		public static ResourceKey ControlLightLightColorKey
		{
			get
			{
				if (SystemColors._cacheControlLightLightColor == null)
				{
					SystemColors._cacheControlLightLightColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightLightColor);
				}
				return SystemColors._cacheControlLightLightColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of text in a three-dimensional display element.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of text in a three-dimensional display element.</returns>
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00021235 File Offset: 0x0001F435
		public static ResourceKey ControlTextColorKey
		{
			get
			{
				if (SystemColors._cacheControlTextColor == null)
				{
					SystemColors._cacheControlTextColor = SystemColors.CreateInstance(SystemResourceKeyID.ControlTextColor);
				}
				return SystemColors._cacheControlTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the desktop. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the desktop.</returns>
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002124F File Offset: 0x0001F44F
		public static ResourceKey DesktopColorKey
		{
			get
			{
				if (SystemColors._cacheDesktopColor == null)
				{
					SystemColors._cacheDesktopColor = SystemColors.CreateInstance(SystemResourceKeyID.DesktopColor);
				}
				return SystemColors._cacheDesktopColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the right-side <see cref="T:System.Windows.Media.Color" /> in the gradient of an active window's title bar. </summary>
		/// <returns>The resource key for the right-side <see cref="T:System.Windows.Media.Color" /> in the gradient of an active window's title bar.</returns>
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00021269 File Offset: 0x0001F469
		public static ResourceKey GradientActiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheGradientActiveCaptionColor == null)
				{
					SystemColors._cacheGradientActiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.GradientActiveCaptionColor);
				}
				return SystemColors._cacheGradientActiveCaptionColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the right-side <see cref="T:System.Windows.Media.Color" /> in the gradient of an inactive window's title bar. </summary>
		/// <returns>The resource key for the right-side <see cref="T:System.Windows.Media.Color" /> in the gradient of an inactive window's title bar.</returns>
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x00021283 File Offset: 0x0001F483
		public static ResourceKey GradientInactiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheGradientInactiveCaptionColor == null)
				{
					SystemColors._cacheGradientInactiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.GradientInactiveCaptionColor);
				}
				return SystemColors._cacheGradientInactiveCaptionColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of disabled text. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of disabled text.</returns>
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0002129D File Offset: 0x0001F49D
		public static ResourceKey GrayTextColorKey
		{
			get
			{
				if (SystemColors._cacheGrayTextColor == null)
				{
					SystemColors._cacheGrayTextColor = SystemColors.CreateInstance(SystemResourceKeyID.GrayTextColor);
				}
				return SystemColors._cacheGrayTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of selected items. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of selected items.</returns>
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x000212B7 File Offset: 0x0001F4B7
		public static ResourceKey HighlightColorKey
		{
			get
			{
				if (SystemColors._cacheHighlightColor == null)
				{
					SystemColors._cacheHighlightColor = SystemColors.CreateInstance(SystemResourceKeyID.HighlightColor);
				}
				return SystemColors._cacheHighlightColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of a selected item's text. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of a selected item's text.</returns>
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x000212D1 File Offset: 0x0001F4D1
		public static ResourceKey HighlightTextColorKey
		{
			get
			{
				if (SystemColors._cacheHighlightTextColor == null)
				{
					SystemColors._cacheHighlightTextColor = SystemColors.CreateInstance(SystemResourceKeyID.HighlightTextColor);
				}
				return SystemColors._cacheHighlightTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> that designates a hot-tracked item. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> that designates a hot-tracked item.</returns>
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x000212EB File Offset: 0x0001F4EB
		public static ResourceKey HotTrackColorKey
		{
			get
			{
				if (SystemColors._cacheHotTrackColor == null)
				{
					SystemColors._cacheHotTrackColor = SystemColors.CreateInstance(SystemResourceKeyID.HotTrackColor);
				}
				return SystemColors._cacheHotTrackColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of an inactive window's border.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of an inactive window's border.</returns>
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00021305 File Offset: 0x0001F505
		public static ResourceKey InactiveBorderColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveBorderColor == null)
				{
					SystemColors._cacheInactiveBorderColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveBorderColor);
				}
				return SystemColors._cacheInactiveBorderColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of an inactive window's title bar. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of an inactive window's title bar.</returns>
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0002131F File Offset: 0x0001F51F
		public static ResourceKey InactiveCaptionColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionColor == null)
				{
					SystemColors._cacheInactiveCaptionColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionColor);
				}
				return SystemColors._cacheInactiveCaptionColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the text of an inactive window's title bar.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the text of an inactive window's title bar.</returns>
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00021339 File Offset: 0x0001F539
		public static ResourceKey InactiveCaptionTextColorKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionTextColor == null)
				{
					SystemColors._cacheInactiveCaptionTextColor = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionTextColor);
				}
				return SystemColors._cacheInactiveCaptionTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of the <see cref="T:System.Windows.Controls.ToolTip" /> control.</summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of the <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00021353 File Offset: 0x0001F553
		public static ResourceKey InfoColorKey
		{
			get
			{
				if (SystemColors._cacheInfoColor == null)
				{
					SystemColors._cacheInfoColor = SystemColors.CreateInstance(SystemResourceKeyID.InfoColor);
				}
				return SystemColors._cacheInfoColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of the text in a <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of the text in a <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0002136D File Offset: 0x0001F56D
		public static ResourceKey InfoTextColorKey
		{
			get
			{
				if (SystemColors._cacheInfoTextColor == null)
				{
					SystemColors._cacheInfoTextColor = SystemColors.CreateInstance(SystemResourceKeyID.InfoTextColor);
				}
				return SystemColors._cacheInfoTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of a menu. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of a menu.</returns>
		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00021387 File Offset: 0x0001F587
		public static ResourceKey MenuColorKey
		{
			get
			{
				if (SystemColors._cacheMenuColor == null)
				{
					SystemColors._cacheMenuColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuColor);
				}
				return SystemColors._cacheMenuColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of a menu bar. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of a menu bar.</returns>
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x000213A1 File Offset: 0x0001F5A1
		public static ResourceKey MenuBarColorKey
		{
			get
			{
				if (SystemColors._cacheMenuBarColor == null)
				{
					SystemColors._cacheMenuBarColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuBarColor);
				}
				return SystemColors._cacheMenuBarColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of a highlighted menu item. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of a highlighted menu item.</returns>
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x000213BB File Offset: 0x0001F5BB
		public static ResourceKey MenuHighlightColorKey
		{
			get
			{
				if (SystemColors._cacheMenuHighlightColor == null)
				{
					SystemColors._cacheMenuHighlightColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuHighlightColor);
				}
				return SystemColors._cacheMenuHighlightColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of a menu's text. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of a menu's text.</returns>
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000213D5 File Offset: 0x0001F5D5
		public static ResourceKey MenuTextColorKey
		{
			get
			{
				if (SystemColors._cacheMenuTextColor == null)
				{
					SystemColors._cacheMenuTextColor = SystemColors.CreateInstance(SystemResourceKeyID.MenuTextColor);
				}
				return SystemColors._cacheMenuTextColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of a scroll bar. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of a scroll bar.</returns>
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x000213EF File Offset: 0x0001F5EF
		public static ResourceKey ScrollBarColorKey
		{
			get
			{
				if (SystemColors._cacheScrollBarColor == null)
				{
					SystemColors._cacheScrollBarColor = SystemColors.CreateInstance(SystemResourceKeyID.ScrollBarColor);
				}
				return SystemColors._cacheScrollBarColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the background <see cref="T:System.Windows.Media.Color" /> of a window's client area. </summary>
		/// <returns>The resource key for the background <see cref="T:System.Windows.Media.Color" /> of a window's client area.</returns>
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x00021409 File Offset: 0x0001F609
		public static ResourceKey WindowColorKey
		{
			get
			{
				if (SystemColors._cacheWindowColor == null)
				{
					SystemColors._cacheWindowColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowColor);
				}
				return SystemColors._cacheWindowColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of a window frame. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of a window frame.</returns>
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00021423 File Offset: 0x0001F623
		public static ResourceKey WindowFrameColorKey
		{
			get
			{
				if (SystemColors._cacheWindowFrameColor == null)
				{
					SystemColors._cacheWindowFrameColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowFrameColor);
				}
				return SystemColors._cacheWindowFrameColor;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.Color" /> of text in a window's client area. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.Color" /> of text in a window's client area.</returns>
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0002143D File Offset: 0x0001F63D
		public static ResourceKey WindowTextColorKey
		{
			get
			{
				if (SystemColors._cacheWindowTextColor == null)
				{
					SystemColors._cacheWindowTextColor = SystemColors.CreateInstance(SystemResourceKeyID.WindowTextColor);
				}
				return SystemColors._cacheWindowTextColor;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the active window's border. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the color of the active window's border. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00021457 File Offset: 0x0001F657
		public static SolidColorBrush ActiveBorderBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the background of the active window's title bar. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the background color of the active window's title bar. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0002145F File Offset: 0x0001F65F
		public static SolidColorBrush ActiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the text in the active window's title bar. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the background color of the color of the text in the active window's title bar. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00021467 File Offset: 0x0001F667
		public static SolidColorBrush ActiveCaptionTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ActiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the application workspace. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the color of the application workspace. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0002146F File Offset: 0x0001F66F
		public static SolidColorBrush AppWorkspaceBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.AppWorkspace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the face color of a three-dimensional display element. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the face color of a three-dimensional display element. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00021477 File Offset: 0x0001F677
		public static SolidColorBrush ControlBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Control);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the shadow color of a three-dimensional display element. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the shadow color of a three-dimensional display element. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0002147F File Offset: 0x0001F67F
		public static SolidColorBrush ControlDarkBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the dark shadow color of a three-dimensional display element. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the dark shadow color of a three-dimensional display element. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00021487 File Offset: 0x0001F687
		public static SolidColorBrush ControlDarkDarkBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlDarkDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the light color of a three-dimensional display element. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.SolidColorBrush" /> with its <see cref="P:System.Windows.Media.SolidColorBrush.Color" /> set to the light color of a three-dimensional display element. The returned brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0002148F File Offset: 0x0001F68F
		public static SolidColorBrush ControlLightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the highlight color of a three-dimensional display element. </summary>
		/// <returns>The highlight color of a three-dimensional display element.</returns>
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00021497 File Offset: 0x0001F697
		public static SolidColorBrush ControlLightLightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlLightLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of text in a three-dimensional display element. </summary>
		/// <returns>The color of text in a three-dimensional display element.</returns>
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0002149F File Offset: 0x0001F69F
		public static SolidColorBrush ControlTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ControlText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the desktop. </summary>
		/// <returns>The color of the desktop.</returns>
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x000214A8 File Offset: 0x0001F6A8
		public static SolidColorBrush DesktopBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Desktop);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the right side color in the gradient of an active window's title bar. </summary>
		/// <returns>The right side color in the gradient.</returns>
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x000214B1 File Offset: 0x0001F6B1
		public static SolidColorBrush GradientActiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GradientActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the right side color in the gradient of an inactive window's title bar. </summary>
		/// <returns>The right side color in the gradient.</returns>
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x000214BA File Offset: 0x0001F6BA
		public static SolidColorBrush GradientInactiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GradientInactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of disabled text. </summary>
		/// <returns>The color of disabled text.</returns>
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000214C3 File Offset: 0x0001F6C3
		public static SolidColorBrush GrayTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.GrayText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of selected items. </summary>
		/// <returns>The background color of selected items.</returns>
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x000214CC File Offset: 0x0001F6CC
		public static SolidColorBrush HighlightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Highlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the text of selected items. </summary>
		/// <returns>The color of the text of selected items.</returns>
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x000214D5 File Offset: 0x0001F6D5
		public static SolidColorBrush HighlightTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.HighlightText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color used to designate a hot-tracked item. </summary>
		/// <returns>The color used to designate a hot-tracked item.</returns>
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x000214DE File Offset: 0x0001F6DE
		public static SolidColorBrush HotTrackBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.HotTrack);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of an inactive window's border. </summary>
		/// <returns>The color of an inactive window's border.</returns>
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x000214E7 File Offset: 0x0001F6E7
		public static SolidColorBrush InactiveBorderBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the background color of an inactive window's title bar. </summary>
		/// <returns>The background color of an inactive window's title bar.</returns>
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x000214F0 File Offset: 0x0001F6F0
		public static SolidColorBrush InactiveCaptionBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the text of an inactive window's title bar. </summary>
		/// <returns>The color of the text of an inactive window's title bar.</returns>
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x000214F9 File Offset: 0x0001F6F9
		public static SolidColorBrush InactiveCaptionTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InactiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the background color for the <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The background color for the <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x00021502 File Offset: 0x0001F702
		public static SolidColorBrush InfoBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Info);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the text color for the <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The text color for the <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0002150B File Offset: 0x0001F70B
		public static SolidColorBrush InfoTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.InfoText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of a menu's background. </summary>
		/// <returns>The color of a menu's background.</returns>
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x00021514 File Offset: 0x0001F714
		public static SolidColorBrush MenuBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Menu);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the background color for a menu bar. </summary>
		/// <returns>The background color for a menu bar.</returns>
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002151D File Offset: 0x0001F71D
		public static SolidColorBrush MenuBarBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color used to highlight a menu item. </summary>
		/// <returns>The color used to highlight a menu item.</returns>
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00021526 File Offset: 0x0001F726
		public static SolidColorBrush MenuHighlightBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of a menu's text. </summary>
		/// <returns>The color of a menu's text.</returns>
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0002152F File Offset: 0x0001F72F
		public static SolidColorBrush MenuTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.MenuText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the background color of a scroll bar. </summary>
		/// <returns>The background color of a scroll bar.</returns>
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00021538 File Offset: 0x0001F738
		public static SolidColorBrush ScrollBarBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.ScrollBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the background color in the client area of a window. </summary>
		/// <returns>The background color in the client area of a window.</returns>
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00021541 File Offset: 0x0001F741
		public static SolidColorBrush WindowBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.Window);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of a window frame. </summary>
		/// <returns>The color of a window frame.</returns>
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002154A File Offset: 0x0001F74A
		public static SolidColorBrush WindowFrameBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.WindowFrame);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the text in the client area of a window. </summary>
		/// <returns>The color of the text in the client area of a window.</returns>
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00021553 File Offset: 0x0001F753
		public static SolidColorBrush WindowTextBrush
		{
			get
			{
				return SystemColors.MakeBrush(SystemColors.CacheSlot.WindowText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color used to highlight a selected item that is inactive.</summary>
		/// <returns>The color used to highlight an inactive selected item.</returns>
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002155C File Offset: 0x0001F75C
		public static SolidColorBrush InactiveSelectionHighlightBrush
		{
			get
			{
				if (SystemParameters.HighContrast)
				{
					return SystemColors.HighlightBrush;
				}
				return SystemColors.ControlBrush;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of an inactive selected item’s text.</summary>
		/// <returns>The color of an inactive selected item’s text.</returns>
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00021570 File Offset: 0x0001F770
		public static SolidColorBrush InactiveSelectionHighlightTextBrush
		{
			get
			{
				if (SystemParameters.HighContrast)
				{
					return SystemColors.HighlightTextBrush;
				}
				return SystemColors.ControlTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> used to paint the active window's border. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> used to paint the active window's border. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060009CB RID: 2507 RVA: 0x00021584 File Offset: 0x0001F784
		public static ResourceKey ActiveBorderBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveBorderBrush == null)
				{
					SystemColors._cacheActiveBorderBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveBorderBrush);
				}
				return SystemColors._cacheActiveBorderBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> used to paint the background of the active window's title bar.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> used to paint the background of the active window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002159D File Offset: 0x0001F79D
		public static ResourceKey ActiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionBrush == null)
				{
					SystemColors._cacheActiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionBrush);
				}
				return SystemColors._cacheActiveCaptionBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in the active window's title bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in the active window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x000215B6 File Offset: 0x0001F7B6
		public static ResourceKey ActiveCaptionTextBrushKey
		{
			get
			{
				if (SystemColors._cacheActiveCaptionTextBrush == null)
				{
					SystemColors._cacheActiveCaptionTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.ActiveCaptionTextBrush);
				}
				return SystemColors._cacheActiveCaptionTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the application workspace.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the application workspace. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x000215CF File Offset: 0x0001F7CF
		public static ResourceKey AppWorkspaceBrushKey
		{
			get
			{
				if (SystemColors._cacheAppWorkspaceBrush == null)
				{
					SystemColors._cacheAppWorkspaceBrush = SystemColors.CreateInstance(SystemResourceKeyID.AppWorkspaceBrush);
				}
				return SystemColors._cacheAppWorkspaceBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the face of a three-dimensional display element.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the face of a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060009CF RID: 2511 RVA: 0x000215E8 File Offset: 0x0001F7E8
		public static ResourceKey ControlBrushKey
		{
			get
			{
				if (SystemColors._cacheControlBrush == null)
				{
					SystemColors._cacheControlBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlBrush);
				}
				return SystemColors._cacheControlBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the shadow of a three-dimensional display element.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the shadow of a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00021601 File Offset: 0x0001F801
		public static ResourceKey ControlDarkBrushKey
		{
			get
			{
				if (SystemColors._cacheControlDarkBrush == null)
				{
					SystemColors._cacheControlDarkBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkBrush);
				}
				return SystemColors._cacheControlDarkBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the dark shadow of a three-dimensional display element. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the dark shadow of a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0002161A File Offset: 0x0001F81A
		public static ResourceKey ControlDarkDarkBrushKey
		{
			get
			{
				if (SystemColors._cacheControlDarkDarkBrush == null)
				{
					SystemColors._cacheControlDarkDarkBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlDarkDarkBrush);
				}
				return SystemColors._cacheControlDarkDarkBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the light area of a three-dimensional display element. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the light area of a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00021633 File Offset: 0x0001F833
		public static ResourceKey ControlLightBrushKey
		{
			get
			{
				if (SystemColors._cacheControlLightBrush == null)
				{
					SystemColors._cacheControlLightBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightBrush);
				}
				return SystemColors._cacheControlLightBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the highlight of a three-dimensional display element. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the highlight of a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0002164C File Offset: 0x0001F84C
		public static ResourceKey ControlLightLightBrushKey
		{
			get
			{
				if (SystemColors._cacheControlLightLightBrush == null)
				{
					SystemColors._cacheControlLightLightBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlLightLightBrush);
				}
				return SystemColors._cacheControlLightLightBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints text in a three-dimensional display element. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints text in a three-dimensional display element. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x00021666 File Offset: 0x0001F866
		public static ResourceKey ControlTextBrushKey
		{
			get
			{
				if (SystemColors._cacheControlTextBrush == null)
				{
					SystemColors._cacheControlTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.ControlTextBrush);
				}
				return SystemColors._cacheControlTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the desktop. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the desktop. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00021680 File Offset: 0x0001F880
		public static ResourceKey DesktopBrushKey
		{
			get
			{
				if (SystemColors._cacheDesktopBrush == null)
				{
					SystemColors._cacheDesktopBrush = SystemColors.CreateInstance(SystemResourceKeyID.DesktopBrush);
				}
				return SystemColors._cacheDesktopBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the right side of the gradient of an active window's title bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the right side of the gradient of an active window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0002169A File Offset: 0x0001F89A
		public static ResourceKey GradientActiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheGradientActiveCaptionBrush == null)
				{
					SystemColors._cacheGradientActiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.GradientActiveCaptionBrush);
				}
				return SystemColors._cacheGradientActiveCaptionBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that is the color of the right side of the gradient of an inactive window's title bar.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> used to paint the background of the inactive window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000216B4 File Offset: 0x0001F8B4
		public static ResourceKey GradientInactiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheGradientInactiveCaptionBrush == null)
				{
					SystemColors._cacheGradientInactiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.GradientInactiveCaptionBrush);
				}
				return SystemColors._cacheGradientInactiveCaptionBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints disabled text. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints disabled text. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x000216CE File Offset: 0x0001F8CE
		public static ResourceKey GrayTextBrushKey
		{
			get
			{
				if (SystemColors._cacheGrayTextBrush == null)
				{
					SystemColors._cacheGrayTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.GrayTextBrush);
				}
				return SystemColors._cacheGrayTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of selected items. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of selected items. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x000216E8 File Offset: 0x0001F8E8
		public static ResourceKey HighlightBrushKey
		{
			get
			{
				if (SystemColors._cacheHighlightBrush == null)
				{
					SystemColors._cacheHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.HighlightBrush);
				}
				return SystemColors._cacheHighlightBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text of selected items. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text of selected items. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00021702 File Offset: 0x0001F902
		public static ResourceKey HighlightTextBrushKey
		{
			get
			{
				if (SystemColors._cacheHighlightTextBrush == null)
				{
					SystemColors._cacheHighlightTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.HighlightTextBrush);
				}
				return SystemColors._cacheHighlightTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints hot-tracked items. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints hot-tracked items. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0002171C File Offset: 0x0001F91C
		public static ResourceKey HotTrackBrushKey
		{
			get
			{
				if (SystemColors._cacheHotTrackBrush == null)
				{
					SystemColors._cacheHotTrackBrush = SystemColors.CreateInstance(SystemResourceKeyID.HotTrackBrush);
				}
				return SystemColors._cacheHotTrackBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the border of an inactive window. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the border of an inactive window. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00021736 File Offset: 0x0001F936
		public static ResourceKey InactiveBorderBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveBorderBrush == null)
				{
					SystemColors._cacheInactiveBorderBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveBorderBrush);
				}
				return SystemColors._cacheInactiveBorderBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of an inactive window's title bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of an inactive window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00021750 File Offset: 0x0001F950
		public static ResourceKey InactiveCaptionBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionBrush == null)
				{
					SystemColors._cacheInactiveCaptionBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionBrush);
				}
				return SystemColors._cacheInactiveCaptionBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text of an inactive window's title bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text of an inactive window's title bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0002176A File Offset: 0x0001F96A
		public static ResourceKey InactiveCaptionTextBrushKey
		{
			get
			{
				if (SystemColors._cacheInactiveCaptionTextBrush == null)
				{
					SystemColors._cacheInactiveCaptionTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveCaptionTextBrush);
				}
				return SystemColors._cacheInactiveCaptionTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of the <see cref="T:System.Windows.Controls.ToolTip" /> control.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of the <see cref="T:System.Windows.Controls.ToolTip" /> control. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x00021784 File Offset: 0x0001F984
		public static ResourceKey InfoBrushKey
		{
			get
			{
				if (SystemColors._cacheInfoBrush == null)
				{
					SystemColors._cacheInfoBrush = SystemColors.CreateInstance(SystemResourceKeyID.InfoBrush);
				}
				return SystemColors._cacheInfoBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in a <see cref="T:System.Windows.Controls.ToolTip" /> control. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in a <see cref="T:System.Windows.Controls.ToolTip" /> control. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0002179E File Offset: 0x0001F99E
		public static ResourceKey InfoTextBrushKey
		{
			get
			{
				if (SystemColors._cacheInfoTextBrush == null)
				{
					SystemColors._cacheInfoTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InfoTextBrush);
				}
				return SystemColors._cacheInfoTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a menu. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a menu. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x000217B8 File Offset: 0x0001F9B8
		public static ResourceKey MenuBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuBrush == null)
				{
					SystemColors._cacheMenuBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuBrush);
				}
				return SystemColors._cacheMenuBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a menu bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a menu bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x000217D2 File Offset: 0x0001F9D2
		public static ResourceKey MenuBarBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuBarBrush == null)
				{
					SystemColors._cacheMenuBarBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuBarBrush);
				}
				return SystemColors._cacheMenuBarBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a highlighted menu item. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a highlighted menu item. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x000217EC File Offset: 0x0001F9EC
		public static ResourceKey MenuHighlightBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuHighlightBrush == null)
				{
					SystemColors._cacheMenuHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuHighlightBrush);
				}
				return SystemColors._cacheMenuHighlightBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a menu's text. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a menu's text. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00021806 File Offset: 0x0001FA06
		public static ResourceKey MenuTextBrushKey
		{
			get
			{
				if (SystemColors._cacheMenuTextBrush == null)
				{
					SystemColors._cacheMenuTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.MenuTextBrush);
				}
				return SystemColors._cacheMenuTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a scroll bar. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a scroll bar. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00021820 File Offset: 0x0001FA20
		public static ResourceKey ScrollBarBrushKey
		{
			get
			{
				if (SystemColors._cacheScrollBarBrush == null)
				{
					SystemColors._cacheScrollBarBrush = SystemColors.CreateInstance(SystemResourceKeyID.ScrollBarBrush);
				}
				return SystemColors._cacheScrollBarBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a window's client area. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of a window's client area. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002183A File Offset: 0x0001FA3A
		public static ResourceKey WindowBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowBrush == null)
				{
					SystemColors._cacheWindowBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowBrush);
				}
				return SystemColors._cacheWindowBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a window frame. </summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a window frame. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x00021854 File Offset: 0x0001FA54
		public static ResourceKey WindowFrameBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowFrameBrush == null)
				{
					SystemColors._cacheWindowFrameBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowFrameBrush);
				}
				return SystemColors._cacheWindowFrameBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in the client area of a window.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the text in the client area of a window. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0002186E File Offset: 0x0001FA6E
		public static ResourceKey WindowTextBrushKey
		{
			get
			{
				if (SystemColors._cacheWindowTextBrush == null)
				{
					SystemColors._cacheWindowTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.WindowTextBrush);
				}
				return SystemColors._cacheWindowTextBrush;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of an inactive selected item.</summary>
		/// <returns>The <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints the background of an inactive selected item. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00021888 File Offset: 0x0001FA88
		public static ResourceKey InactiveSelectionHighlightBrushKey
		{
			get
			{
				if (FrameworkCompatibilityPreferences.GetAreInactiveSelectionHighlightBrushKeysSupported())
				{
					if (SystemColors._cacheInactiveSelectionHighlightBrush == null)
					{
						SystemColors._cacheInactiveSelectionHighlightBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightBrush);
					}
					return SystemColors._cacheInactiveSelectionHighlightBrush;
				}
				return SystemColors.ControlBrushKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a an inactive selected item’s text.</summary>
		/// <returns>The <see cref="T:System.Windows.ResourceKey" /> for the <see cref="T:System.Windows.Media.SolidColorBrush" /> that paints a an inactive selected item’s text. This brush's <see cref="P:System.Windows.Freezable.IsFrozen" /> property is <see langword="true" />, so it cannot be modified.</returns>
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x000218B2 File Offset: 0x0001FAB2
		public static ResourceKey InactiveSelectionHighlightTextBrushKey
		{
			get
			{
				if (FrameworkCompatibilityPreferences.GetAreInactiveSelectionHighlightBrushKeysSupported())
				{
					if (SystemColors._cacheInactiveSelectionHighlightTextBrush == null)
					{
						SystemColors._cacheInactiveSelectionHighlightTextBrush = SystemColors.CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightTextBrush);
					}
					return SystemColors._cacheInactiveSelectionHighlightTextBrush;
				}
				return SystemColors.ControlTextBrushKey;
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000218DC File Offset: 0x0001FADC
		internal static bool InvalidateCache()
		{
			bool flag = SystemResources.ClearBitArray(SystemColors._colorCacheValid);
			bool flag2 = SystemResources.ClearBitArray(SystemColors._brushCacheValid);
			return flag || flag2;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00021902 File Offset: 0x0001FB02
		private static int Encode(int alpha, int red, int green, int blue)
		{
			return red << 16 | green << 8 | blue | alpha << 24;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00021913 File Offset: 0x0001FB13
		private static int FromWin32Value(int value)
		{
			return SystemColors.Encode(255, value & 255, value >> 8 & 255, value >> 16 & 255);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002193C File Offset: 0x0001FB3C
		private static Color GetSystemColor(SystemColors.CacheSlot slot)
		{
			BitArray colorCacheValid = SystemColors._colorCacheValid;
			Color color;
			lock (colorCacheValid)
			{
				while (!SystemColors._colorCacheValid[(int)slot])
				{
					SystemColors._colorCacheValid[(int)slot] = true;
					int sysColor = SafeNativeMethods.GetSysColor(SystemColors.SlotToFlag(slot));
					uint num = (uint)SystemColors.FromWin32Value(sysColor);
					color = Color.FromArgb((byte)((num & 4278190080U) >> 24), (byte)((num & 16711680U) >> 16), (byte)((num & 65280U) >> 8), (byte)(num & 255U));
					SystemColors._colorCache[(int)slot] = color;
				}
				color = SystemColors._colorCache[(int)slot];
			}
			return color;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000219EC File Offset: 0x0001FBEC
		private static SolidColorBrush MakeBrush(SystemColors.CacheSlot slot)
		{
			BitArray brushCacheValid = SystemColors._brushCacheValid;
			SolidColorBrush solidColorBrush;
			lock (brushCacheValid)
			{
				while (!SystemColors._brushCacheValid[(int)slot])
				{
					SystemColors._brushCacheValid[(int)slot] = true;
					solidColorBrush = new SolidColorBrush(SystemColors.GetSystemColor(slot));
					solidColorBrush.Freeze();
					SystemColors._brushCache[(int)slot] = solidColorBrush;
				}
				solidColorBrush = SystemColors._brushCache[(int)slot];
			}
			return solidColorBrush;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00021A64 File Offset: 0x0001FC64
		private static int SlotToFlag(SystemColors.CacheSlot slot)
		{
			switch (slot)
			{
			case SystemColors.CacheSlot.ActiveBorder:
				return 10;
			case SystemColors.CacheSlot.ActiveCaption:
				return 2;
			case SystemColors.CacheSlot.ActiveCaptionText:
				return 9;
			case SystemColors.CacheSlot.AppWorkspace:
				return 12;
			case SystemColors.CacheSlot.Control:
				return 15;
			case SystemColors.CacheSlot.ControlDark:
				return 16;
			case SystemColors.CacheSlot.ControlDarkDark:
				return 21;
			case SystemColors.CacheSlot.ControlLight:
				return 22;
			case SystemColors.CacheSlot.ControlLightLight:
				return 20;
			case SystemColors.CacheSlot.ControlText:
				return 18;
			case SystemColors.CacheSlot.Desktop:
				return 1;
			case SystemColors.CacheSlot.GradientActiveCaption:
				return 27;
			case SystemColors.CacheSlot.GradientInactiveCaption:
				return 28;
			case SystemColors.CacheSlot.GrayText:
				return 17;
			case SystemColors.CacheSlot.Highlight:
				return 13;
			case SystemColors.CacheSlot.HighlightText:
				return 14;
			case SystemColors.CacheSlot.HotTrack:
				return 26;
			case SystemColors.CacheSlot.InactiveBorder:
				return 11;
			case SystemColors.CacheSlot.InactiveCaption:
				return 3;
			case SystemColors.CacheSlot.InactiveCaptionText:
				return 19;
			case SystemColors.CacheSlot.Info:
				return 24;
			case SystemColors.CacheSlot.InfoText:
				return 23;
			case SystemColors.CacheSlot.Menu:
				return 4;
			case SystemColors.CacheSlot.MenuBar:
				return 30;
			case SystemColors.CacheSlot.MenuHighlight:
				return 29;
			case SystemColors.CacheSlot.MenuText:
				return 7;
			case SystemColors.CacheSlot.ScrollBar:
				return 0;
			case SystemColors.CacheSlot.Window:
				return 5;
			case SystemColors.CacheSlot.WindowFrame:
				return 6;
			case SystemColors.CacheSlot.WindowText:
				return 8;
			default:
				return 0;
			}
		}

		// Token: 0x0400081F RID: 2079
		private const int AlphaShift = 24;

		// Token: 0x04000820 RID: 2080
		private const int RedShift = 16;

		// Token: 0x04000821 RID: 2081
		private const int GreenShift = 8;

		// Token: 0x04000822 RID: 2082
		private const int BlueShift = 0;

		// Token: 0x04000823 RID: 2083
		private const int Win32RedShift = 0;

		// Token: 0x04000824 RID: 2084
		private const int Win32GreenShift = 8;

		// Token: 0x04000825 RID: 2085
		private const int Win32BlueShift = 16;

		// Token: 0x04000826 RID: 2086
		private static BitArray _colorCacheValid = new BitArray(30);

		// Token: 0x04000827 RID: 2087
		private static Color[] _colorCache = new Color[30];

		// Token: 0x04000828 RID: 2088
		private static BitArray _brushCacheValid = new BitArray(30);

		// Token: 0x04000829 RID: 2089
		private static SolidColorBrush[] _brushCache = new SolidColorBrush[30];

		// Token: 0x0400082A RID: 2090
		private static SystemResourceKey _cacheActiveBorderBrush;

		// Token: 0x0400082B RID: 2091
		private static SystemResourceKey _cacheActiveCaptionBrush;

		// Token: 0x0400082C RID: 2092
		private static SystemResourceKey _cacheActiveCaptionTextBrush;

		// Token: 0x0400082D RID: 2093
		private static SystemResourceKey _cacheAppWorkspaceBrush;

		// Token: 0x0400082E RID: 2094
		private static SystemResourceKey _cacheControlBrush;

		// Token: 0x0400082F RID: 2095
		private static SystemResourceKey _cacheControlDarkBrush;

		// Token: 0x04000830 RID: 2096
		private static SystemResourceKey _cacheControlDarkDarkBrush;

		// Token: 0x04000831 RID: 2097
		private static SystemResourceKey _cacheControlLightBrush;

		// Token: 0x04000832 RID: 2098
		private static SystemResourceKey _cacheControlLightLightBrush;

		// Token: 0x04000833 RID: 2099
		private static SystemResourceKey _cacheControlTextBrush;

		// Token: 0x04000834 RID: 2100
		private static SystemResourceKey _cacheDesktopBrush;

		// Token: 0x04000835 RID: 2101
		private static SystemResourceKey _cacheGradientActiveCaptionBrush;

		// Token: 0x04000836 RID: 2102
		private static SystemResourceKey _cacheGradientInactiveCaptionBrush;

		// Token: 0x04000837 RID: 2103
		private static SystemResourceKey _cacheGrayTextBrush;

		// Token: 0x04000838 RID: 2104
		private static SystemResourceKey _cacheHighlightBrush;

		// Token: 0x04000839 RID: 2105
		private static SystemResourceKey _cacheHighlightTextBrush;

		// Token: 0x0400083A RID: 2106
		private static SystemResourceKey _cacheHotTrackBrush;

		// Token: 0x0400083B RID: 2107
		private static SystemResourceKey _cacheInactiveBorderBrush;

		// Token: 0x0400083C RID: 2108
		private static SystemResourceKey _cacheInactiveCaptionBrush;

		// Token: 0x0400083D RID: 2109
		private static SystemResourceKey _cacheInactiveCaptionTextBrush;

		// Token: 0x0400083E RID: 2110
		private static SystemResourceKey _cacheInfoBrush;

		// Token: 0x0400083F RID: 2111
		private static SystemResourceKey _cacheInfoTextBrush;

		// Token: 0x04000840 RID: 2112
		private static SystemResourceKey _cacheMenuBrush;

		// Token: 0x04000841 RID: 2113
		private static SystemResourceKey _cacheMenuBarBrush;

		// Token: 0x04000842 RID: 2114
		private static SystemResourceKey _cacheMenuHighlightBrush;

		// Token: 0x04000843 RID: 2115
		private static SystemResourceKey _cacheMenuTextBrush;

		// Token: 0x04000844 RID: 2116
		private static SystemResourceKey _cacheScrollBarBrush;

		// Token: 0x04000845 RID: 2117
		private static SystemResourceKey _cacheWindowBrush;

		// Token: 0x04000846 RID: 2118
		private static SystemResourceKey _cacheWindowFrameBrush;

		// Token: 0x04000847 RID: 2119
		private static SystemResourceKey _cacheWindowTextBrush;

		// Token: 0x04000848 RID: 2120
		private static SystemResourceKey _cacheInactiveSelectionHighlightBrush;

		// Token: 0x04000849 RID: 2121
		private static SystemResourceKey _cacheInactiveSelectionHighlightTextBrush;

		// Token: 0x0400084A RID: 2122
		private static SystemResourceKey _cacheActiveBorderColor;

		// Token: 0x0400084B RID: 2123
		private static SystemResourceKey _cacheActiveCaptionColor;

		// Token: 0x0400084C RID: 2124
		private static SystemResourceKey _cacheActiveCaptionTextColor;

		// Token: 0x0400084D RID: 2125
		private static SystemResourceKey _cacheAppWorkspaceColor;

		// Token: 0x0400084E RID: 2126
		private static SystemResourceKey _cacheControlColor;

		// Token: 0x0400084F RID: 2127
		private static SystemResourceKey _cacheControlDarkColor;

		// Token: 0x04000850 RID: 2128
		private static SystemResourceKey _cacheControlDarkDarkColor;

		// Token: 0x04000851 RID: 2129
		private static SystemResourceKey _cacheControlLightColor;

		// Token: 0x04000852 RID: 2130
		private static SystemResourceKey _cacheControlLightLightColor;

		// Token: 0x04000853 RID: 2131
		private static SystemResourceKey _cacheControlTextColor;

		// Token: 0x04000854 RID: 2132
		private static SystemResourceKey _cacheDesktopColor;

		// Token: 0x04000855 RID: 2133
		private static SystemResourceKey _cacheGradientActiveCaptionColor;

		// Token: 0x04000856 RID: 2134
		private static SystemResourceKey _cacheGradientInactiveCaptionColor;

		// Token: 0x04000857 RID: 2135
		private static SystemResourceKey _cacheGrayTextColor;

		// Token: 0x04000858 RID: 2136
		private static SystemResourceKey _cacheHighlightColor;

		// Token: 0x04000859 RID: 2137
		private static SystemResourceKey _cacheHighlightTextColor;

		// Token: 0x0400085A RID: 2138
		private static SystemResourceKey _cacheHotTrackColor;

		// Token: 0x0400085B RID: 2139
		private static SystemResourceKey _cacheInactiveBorderColor;

		// Token: 0x0400085C RID: 2140
		private static SystemResourceKey _cacheInactiveCaptionColor;

		// Token: 0x0400085D RID: 2141
		private static SystemResourceKey _cacheInactiveCaptionTextColor;

		// Token: 0x0400085E RID: 2142
		private static SystemResourceKey _cacheInfoColor;

		// Token: 0x0400085F RID: 2143
		private static SystemResourceKey _cacheInfoTextColor;

		// Token: 0x04000860 RID: 2144
		private static SystemResourceKey _cacheMenuColor;

		// Token: 0x04000861 RID: 2145
		private static SystemResourceKey _cacheMenuBarColor;

		// Token: 0x04000862 RID: 2146
		private static SystemResourceKey _cacheMenuHighlightColor;

		// Token: 0x04000863 RID: 2147
		private static SystemResourceKey _cacheMenuTextColor;

		// Token: 0x04000864 RID: 2148
		private static SystemResourceKey _cacheScrollBarColor;

		// Token: 0x04000865 RID: 2149
		private static SystemResourceKey _cacheWindowColor;

		// Token: 0x04000866 RID: 2150
		private static SystemResourceKey _cacheWindowFrameColor;

		// Token: 0x04000867 RID: 2151
		private static SystemResourceKey _cacheWindowTextColor;

		// Token: 0x02000829 RID: 2089
		private enum CacheSlot
		{
			// Token: 0x04003BF4 RID: 15348
			ActiveBorder,
			// Token: 0x04003BF5 RID: 15349
			ActiveCaption,
			// Token: 0x04003BF6 RID: 15350
			ActiveCaptionText,
			// Token: 0x04003BF7 RID: 15351
			AppWorkspace,
			// Token: 0x04003BF8 RID: 15352
			Control,
			// Token: 0x04003BF9 RID: 15353
			ControlDark,
			// Token: 0x04003BFA RID: 15354
			ControlDarkDark,
			// Token: 0x04003BFB RID: 15355
			ControlLight,
			// Token: 0x04003BFC RID: 15356
			ControlLightLight,
			// Token: 0x04003BFD RID: 15357
			ControlText,
			// Token: 0x04003BFE RID: 15358
			Desktop,
			// Token: 0x04003BFF RID: 15359
			GradientActiveCaption,
			// Token: 0x04003C00 RID: 15360
			GradientInactiveCaption,
			// Token: 0x04003C01 RID: 15361
			GrayText,
			// Token: 0x04003C02 RID: 15362
			Highlight,
			// Token: 0x04003C03 RID: 15363
			HighlightText,
			// Token: 0x04003C04 RID: 15364
			HotTrack,
			// Token: 0x04003C05 RID: 15365
			InactiveBorder,
			// Token: 0x04003C06 RID: 15366
			InactiveCaption,
			// Token: 0x04003C07 RID: 15367
			InactiveCaptionText,
			// Token: 0x04003C08 RID: 15368
			Info,
			// Token: 0x04003C09 RID: 15369
			InfoText,
			// Token: 0x04003C0A RID: 15370
			Menu,
			// Token: 0x04003C0B RID: 15371
			MenuBar,
			// Token: 0x04003C0C RID: 15372
			MenuHighlight,
			// Token: 0x04003C0D RID: 15373
			MenuText,
			// Token: 0x04003C0E RID: 15374
			ScrollBar,
			// Token: 0x04003C0F RID: 15375
			Window,
			// Token: 0x04003C10 RID: 15376
			WindowFrame,
			// Token: 0x04003C11 RID: 15377
			WindowText,
			// Token: 0x04003C12 RID: 15378
			NumSlots
		}
	}
}
