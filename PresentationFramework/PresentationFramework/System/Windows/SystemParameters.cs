using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.FontCache;
using MS.Internal.Interop;
using MS.Utility;
using MS.Win32;
using Standard;

namespace System.Windows
{
	/// <summary>Contains properties that you can use to query system settings. </summary>
	// Token: 0x02000111 RID: 273
	public static class SystemParameters
	{
		/// <summary>
		///     Occurs when one of the properties changes.</summary>
		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000A45 RID: 2629 RVA: 0x000226A4 File Offset: 0x000208A4
		// (remove) Token: 0x06000A46 RID: 2630 RVA: 0x000226D8 File Offset: 0x000208D8
		public static event PropertyChangedEventHandler StaticPropertyChanged;

		// Token: 0x06000A47 RID: 2631 RVA: 0x0002270C File Offset: 0x0002090C
		private static void OnPropertiesChanged(params string[] propertyNames)
		{
			PropertyChangedEventHandler staticPropertyChanged = SystemParameters.StaticPropertyChanged;
			if (staticPropertyChanged != null)
			{
				for (int i = 0; i < propertyNames.Length; i++)
				{
					staticPropertyChanged(null, new PropertyChangedEventArgs(propertyNames[i]));
				}
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002273F File Offset: 0x0002093F
		private static bool InvalidateProperty(int slot, string name)
		{
			if (!SystemResources.ClearSlot(SystemParameters._cacheValid, slot))
			{
				return false;
			}
			SystemParameters.OnPropertiesChanged(new string[]
			{
				name
			});
			return true;
		}

		/// <summary>Gets the width, in pixels, of the left and right edges of the focus rectangle.  </summary>
		/// <returns>The edge width.</returns>
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00022760 File Offset: 0x00020960
		public static double FocusBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[1])
					{
						SystemParameters._cacheValid[1] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(8206, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[1] = false;
							throw new Win32Exception();
						}
						SystemParameters._focusBorderWidth = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._focusBorderWidth;
			}
		}

		/// <summary>Gets the height, in pixels, of the upper and lower edges of the focus rectangle.  </summary>
		/// <returns>The edge height.</returns>
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x000227EC File Offset: 0x000209EC
		public static double FocusBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[2])
					{
						SystemParameters._cacheValid[2] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(8208, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[2] = false;
							throw new Win32Exception();
						}
						SystemParameters._focusBorderHeight = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._focusBorderHeight;
			}
		}

		/// <summary>Gets information about the High Contrast accessibility feature. </summary>
		/// <returns>
		///     <see langword="true" /> if the HIGHCONTRASTON option is selected; otherwise,<see langword=" false" />.</returns>
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00022878 File Offset: 0x00020A78
		public static bool HighContrast
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[3])
					{
						SystemParameters._cacheValid[3] = true;
						NativeMethods.HIGHCONTRAST_I highcontrast_I = default(NativeMethods.HIGHCONTRAST_I);
						highcontrast_I.cbSize = Marshal.SizeOf(typeof(NativeMethods.HIGHCONTRAST_I));
						if (!UnsafeNativeMethods.SystemParametersInfo(66, highcontrast_I.cbSize, ref highcontrast_I, 0))
						{
							SystemParameters._cacheValid[3] = false;
							throw new Win32Exception();
						}
						SystemParameters._highContrast = ((highcontrast_I.dwFlags & 1) == 1);
					}
				}
				return SystemParameters._highContrast;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00022928 File Offset: 0x00020B28
		internal static bool MouseVanish
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[4])
					{
						SystemParameters._cacheValid[4] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4128, 0, ref SystemParameters._mouseVanish, 0))
						{
							SystemParameters._cacheValid[4] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._mouseVanish;
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00021143 File Offset: 0x0001F343
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
		{
			return new SystemResourceKey(KeyId);
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FocusBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x000229A8 File Offset: 0x00020BA8
		public static ResourceKey FocusBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFocusBorderWidth == null)
				{
					SystemParameters._cacheFocusBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FocusBorderWidth);
				}
				return SystemParameters._cacheFocusBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FocusBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x000229C5 File Offset: 0x00020BC5
		public static ResourceKey FocusBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFocusBorderHeight == null)
				{
					SystemParameters._cacheFocusBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FocusBorderHeight);
				}
				return SystemParameters._cacheFocusBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.HighContrast" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x000229E2 File Offset: 0x00020BE2
		public static ResourceKey HighContrastKey
		{
			get
			{
				if (SystemParameters._cacheHighContrast == null)
				{
					SystemParameters._cacheHighContrast = SystemParameters.CreateInstance(SystemResourceKeyID.HighContrast);
				}
				return SystemParameters._cacheHighContrast;
			}
		}

		/// <summary>Gets a value indicating whether the drop shadow effect is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the drop shadow effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00022A00 File Offset: 0x00020C00
		public static bool DropShadow
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[5])
					{
						SystemParameters._cacheValid[5] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4132, 0, ref SystemParameters._dropShadow, 0))
						{
							SystemParameters._cacheValid[5] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._dropShadow;
			}
		}

		/// <summary>Gets a value indicating whether native menus appear as a flat menu.  </summary>
		/// <returns>
		///     <see langword="true" /> if the flat menu appearance is set; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00022A80 File Offset: 0x00020C80
		public static bool FlatMenu
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[6])
					{
						SystemParameters._cacheValid[6] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4130, 0, ref SystemParameters._flatMenu, 0))
						{
							SystemParameters._cacheValid[6] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._flatMenu;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x00022B00 File Offset: 0x00020D00
		internal static NativeMethods.RECT WorkAreaInternal
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[7])
					{
						SystemParameters._cacheValid[7] = true;
						SystemParameters._workAreaInternal = default(NativeMethods.RECT);
						if (!UnsafeNativeMethods.SystemParametersInfo(48, 0, ref SystemParameters._workAreaInternal, 0))
						{
							SystemParameters._cacheValid[7] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._workAreaInternal;
			}
		}

		/// <summary>Gets the size of the work area on the primary display monitor. </summary>
		/// <returns>A <see langword="RECT" /> structure that receives the work area coordinates, expressed as virtual screen coordinates.</returns>
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00022B88 File Offset: 0x00020D88
		public static Rect WorkArea
		{
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[8])
					{
						SystemParameters._cacheValid[8] = true;
						NativeMethods.RECT workAreaInternal = SystemParameters.WorkAreaInternal;
						SystemParameters._workArea = new Rect(SystemParameters.ConvertPixel(workAreaInternal.left), SystemParameters.ConvertPixel(workAreaInternal.top), SystemParameters.ConvertPixel(workAreaInternal.Width), SystemParameters.ConvertPixel(workAreaInternal.Height));
					}
				}
				return SystemParameters._workArea;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.DropShadow" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00022C20 File Offset: 0x00020E20
		public static ResourceKey DropShadowKey
		{
			get
			{
				if (SystemParameters._cacheDropShadow == null)
				{
					SystemParameters._cacheDropShadow = SystemParameters.CreateInstance(SystemResourceKeyID.DropShadow);
				}
				return SystemParameters._cacheDropShadow;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FlatMenu" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00022C3D File Offset: 0x00020E3D
		public static ResourceKey FlatMenuKey
		{
			get
			{
				if (SystemParameters._cacheFlatMenu == null)
				{
					SystemParameters._cacheFlatMenu = SystemParameters.CreateInstance(SystemResourceKeyID.FlatMenu);
				}
				return SystemParameters._cacheFlatMenu;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.WorkArea" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00022C5A File Offset: 0x00020E5A
		public static ResourceKey WorkAreaKey
		{
			get
			{
				if (SystemParameters._cacheWorkArea == null)
				{
					SystemParameters._cacheWorkArea = SystemParameters.CreateInstance(SystemResourceKeyID.WorkArea);
				}
				return SystemParameters._cacheWorkArea;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00022C78 File Offset: 0x00020E78
		internal static NativeMethods.ICONMETRICS IconMetrics
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[9])
					{
						SystemParameters._cacheValid[9] = true;
						SystemParameters._iconMetrics = new NativeMethods.ICONMETRICS();
						if (!UnsafeNativeMethods.SystemParametersInfo(45, SystemParameters._iconMetrics.cbSize, SystemParameters._iconMetrics, 0))
						{
							SystemParameters._cacheValid[9] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._iconMetrics;
			}
		}

		/// <summary>Gets the width, in pixels, of an icon cell. The system uses this rectangle to arrange icons in large icon view. </summary>
		/// <returns>The width of an icon cell.</returns>
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00022D0C File Offset: 0x00020F0C
		public static double IconHorizontalSpacing
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.IconMetrics.iHorzSpacing);
			}
		}

		/// <summary>Gets the height, in pixels, of an icon cell. The system uses this rectangle to arrange icons in large icon view. </summary>
		/// <returns>The height of an icon cell.</returns>
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00022D1D File Offset: 0x00020F1D
		public static double IconVerticalSpacing
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.IconMetrics.iVertSpacing);
			}
		}

		/// <summary>Gets a value indicating whether icon-title wrapping is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if icon-title wrapping is enabled; otherwise <see langword="false" />.</returns>
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00022D2E File Offset: 0x00020F2E
		public static bool IconTitleWrap
		{
			get
			{
				return SystemParameters.IconMetrics.iTitleWrap != 0;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconHorizontalSpacing" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00022D3D File Offset: 0x00020F3D
		public static ResourceKey IconHorizontalSpacingKey
		{
			get
			{
				if (SystemParameters._cacheIconHorizontalSpacing == null)
				{
					SystemParameters._cacheIconHorizontalSpacing = SystemParameters.CreateInstance(SystemResourceKeyID.IconHorizontalSpacing);
				}
				return SystemParameters._cacheIconHorizontalSpacing;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconVerticalSpacing" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x00022D5A File Offset: 0x00020F5A
		public static ResourceKey IconVerticalSpacingKey
		{
			get
			{
				if (SystemParameters._cacheIconVerticalSpacing == null)
				{
					SystemParameters._cacheIconVerticalSpacing = SystemParameters.CreateInstance(SystemResourceKeyID.IconVerticalSpacing);
				}
				return SystemParameters._cacheIconVerticalSpacing;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconTitleWrap" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00022D77 File Offset: 0x00020F77
		public static ResourceKey IconTitleWrapKey
		{
			get
			{
				if (SystemParameters._cacheIconTitleWrap == null)
				{
					SystemParameters._cacheIconTitleWrap = SystemParameters.CreateInstance(SystemResourceKeyID.IconTitleWrap);
				}
				return SystemParameters._cacheIconTitleWrap;
			}
		}

		/// <summary>Gets a value indicating whether menu access keys are always underlined. </summary>
		/// <returns>
		///     <see langword="true" /> if menu access keys are always underlined; <see langword="false" /> if they are underlined only when the menu is activated by the keyboard.</returns>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x00022D94 File Offset: 0x00020F94
		public static bool KeyboardCues
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[10])
					{
						SystemParameters._cacheValid[10] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4106, 0, ref SystemParameters._keyboardCues, 0))
						{
							SystemParameters._cacheValid[10] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardCues;
			}
		}

		/// <summary>Gets the keyboard repeat-delay setting, which is a value in the range from 0 (approximately 250 milliseconds delay) through 3 (approximately 1 second delay). </summary>
		/// <returns>The keyboard repeat-delay setting.</returns>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00022E18 File Offset: 0x00021018
		public static int KeyboardDelay
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[11])
					{
						SystemParameters._cacheValid[11] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(22, 0, ref SystemParameters._keyboardDelay, 0))
						{
							SystemParameters._cacheValid[11] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardDelay;
			}
		}

		/// <summary>Gets a value indicating whether the user relies on the keyboard instead of the mouse, and whether the user wants applications to display keyboard interfaces that are typically hidden. </summary>
		/// <returns>
		///     <see langword="true" /> if the user relies on the keyboard; otherwise,<see langword=" false" />.</returns>
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x00022E98 File Offset: 0x00021098
		public static bool KeyboardPreference
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[12])
					{
						SystemParameters._cacheValid[12] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(68, 0, ref SystemParameters._keyboardPref, 0))
						{
							SystemParameters._cacheValid[12] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardPref;
			}
		}

		/// <summary>Gets the keyboard repeat-speed setting, which is a value in the range from 0 (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second). </summary>
		/// <returns>The keyboard repeat-speed setting.</returns>
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00022F18 File Offset: 0x00021118
		public static int KeyboardSpeed
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[13])
					{
						SystemParameters._cacheValid[13] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(10, 0, ref SystemParameters._keyboardSpeed, 0))
						{
							SystemParameters._cacheValid[13] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._keyboardSpeed;
			}
		}

		/// <summary>Gets a value indicating whether the snap-to-default button is enabled. If enabled, the mouse cursor automatically moves to the default button of a dialog box, such as OK or Apply.  </summary>
		/// <returns>
		///     <see langword="true" /> when the feature is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00022F98 File Offset: 0x00021198
		public static bool SnapToDefaultButton
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[14])
					{
						SystemParameters._cacheValid[14] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(95, 0, ref SystemParameters._snapToDefButton, 0))
						{
							SystemParameters._cacheValid[14] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._snapToDefButton;
			}
		}

		/// <summary>Gets a value that indicates the number of lines to scroll when the mouse wheel is rotated. </summary>
		/// <returns>The number of lines.</returns>
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00023018 File Offset: 0x00021218
		public static int WheelScrollLines
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[15])
					{
						SystemParameters._cacheValid[15] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(104, 0, ref SystemParameters._wheelScrollLines, 0))
						{
							SystemParameters._cacheValid[15] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._wheelScrollLines;
			}
		}

		/// <summary>Gets the time, in milliseconds, that the mouse pointer must remain in the hover rectangle to generate a mouse-hover event.  </summary>
		/// <returns>The time, in milliseconds, that the mouse must be in the hover rectangle to generate a mouse-hover event.</returns>
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00023098 File Offset: 0x00021298
		public static TimeSpan MouseHoverTime
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)SystemParameters.MouseHoverTimeMilliseconds);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x000230A8 File Offset: 0x000212A8
		internal static int MouseHoverTimeMilliseconds
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[16])
					{
						SystemParameters._cacheValid[16] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(102, 0, ref SystemParameters._mouseHoverTime, 0))
						{
							SystemParameters._cacheValid[16] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._mouseHoverTime;
			}
		}

		/// <summary>Gets the height, in pixels, of the rectangle within which the mouse pointer has to stay to generate a mouse-hover event. </summary>
		/// <returns>The height of a rectangle used for a mouse-hover event.</returns>
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00023128 File Offset: 0x00021328
		public static double MouseHoverHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[17])
					{
						SystemParameters._cacheValid[17] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(100, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[17] = false;
							throw new Win32Exception();
						}
						SystemParameters._mouseHoverHeight = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._mouseHoverHeight;
			}
		}

		/// <summary>Gets the width, in pixels, of the rectangle within which the mouse pointer has to stay to generate a mouse-hover event.  </summary>
		/// <returns>The width of a rectangle used for a mouse-hover event.</returns>
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x000231B4 File Offset: 0x000213B4
		public static double MouseHoverWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[18])
					{
						SystemParameters._cacheValid[18] = true;
						int pixel = 0;
						if (!UnsafeNativeMethods.SystemParametersInfo(98, 0, ref pixel, 0))
						{
							SystemParameters._cacheValid[18] = false;
							throw new Win32Exception();
						}
						SystemParameters._mouseHoverWidth = SystemParameters.ConvertPixel(pixel);
					}
				}
				return SystemParameters._mouseHoverWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.KeyboardCues" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00023240 File Offset: 0x00021440
		public static ResourceKey KeyboardCuesKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardCues == null)
				{
					SystemParameters._cacheKeyboardCues = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardCues);
				}
				return SystemParameters._cacheKeyboardCues;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.KeyboardDelay" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0002325D File Offset: 0x0002145D
		public static ResourceKey KeyboardDelayKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardDelay == null)
				{
					SystemParameters._cacheKeyboardDelay = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardDelay);
				}
				return SystemParameters._cacheKeyboardDelay;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.KeyboardPreference" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0002327A File Offset: 0x0002147A
		public static ResourceKey KeyboardPreferenceKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardPreference == null)
				{
					SystemParameters._cacheKeyboardPreference = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardPreference);
				}
				return SystemParameters._cacheKeyboardPreference;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.KeyboardSpeed" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00023297 File Offset: 0x00021497
		public static ResourceKey KeyboardSpeedKey
		{
			get
			{
				if (SystemParameters._cacheKeyboardSpeed == null)
				{
					SystemParameters._cacheKeyboardSpeed = SystemParameters.CreateInstance(SystemResourceKeyID.KeyboardSpeed);
				}
				return SystemParameters._cacheKeyboardSpeed;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SnapToDefaultButton" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x000232B4 File Offset: 0x000214B4
		public static ResourceKey SnapToDefaultButtonKey
		{
			get
			{
				if (SystemParameters._cacheSnapToDefaultButton == null)
				{
					SystemParameters._cacheSnapToDefaultButton = SystemParameters.CreateInstance(SystemResourceKeyID.SnapToDefaultButton);
				}
				return SystemParameters._cacheSnapToDefaultButton;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.WheelScrollLines" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000232D1 File Offset: 0x000214D1
		public static ResourceKey WheelScrollLinesKey
		{
			get
			{
				if (SystemParameters._cacheWheelScrollLines == null)
				{
					SystemParameters._cacheWheelScrollLines = SystemParameters.CreateInstance(SystemResourceKeyID.WheelScrollLines);
				}
				return SystemParameters._cacheWheelScrollLines;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MouseHoverTime" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000232EE File Offset: 0x000214EE
		public static ResourceKey MouseHoverTimeKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverTime == null)
				{
					SystemParameters._cacheMouseHoverTime = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverTime);
				}
				return SystemParameters._cacheMouseHoverTime;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MouseHoverHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002330B File Offset: 0x0002150B
		public static ResourceKey MouseHoverHeightKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverHeight == null)
				{
					SystemParameters._cacheMouseHoverHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverHeight);
				}
				return SystemParameters._cacheMouseHoverHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MouseHoverWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00023328 File Offset: 0x00021528
		public static ResourceKey MouseHoverWidthKey
		{
			get
			{
				if (SystemParameters._cacheMouseHoverWidth == null)
				{
					SystemParameters._cacheMouseHoverWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MouseHoverWidth);
				}
				return SystemParameters._cacheMouseHoverWidth;
			}
		}

		/// <summary>Gets a value indicating whether pop-up menus are left-aligned or right-aligned, relative to the corresponding menu item. </summary>
		/// <returns>
		///     <see langword="true" /> if left-aligned; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00023348 File Offset: 0x00021548
		public static bool MenuDropAlignment
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[19])
					{
						SystemParameters._cacheValid[19] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(27, 0, ref SystemParameters._menuDropAlignment, 0))
						{
							SystemParameters._cacheValid[19] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuDropAlignment;
			}
		}

		/// <summary>Gets a value indicating whether menu fade animation is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> when fade animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000233C8 File Offset: 0x000215C8
		public static bool MenuFade
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[20])
					{
						SystemParameters._cacheValid[20] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4114, 0, ref SystemParameters._menuFade, 0))
						{
							SystemParameters._cacheValid[20] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuFade;
			}
		}

		/// <summary>Gets the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor is over a submenu item.  </summary>
		/// <returns>The delay time.</returns>
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002344C File Offset: 0x0002164C
		public static int MenuShowDelay
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[21])
					{
						SystemParameters._cacheValid[21] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(106, 0, ref SystemParameters._menuShowDelay, 0))
						{
							SystemParameters._cacheValid[21] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuShowDelay;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuDropAlignment" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000234CC File Offset: 0x000216CC
		public static ResourceKey MenuDropAlignmentKey
		{
			get
			{
				if (SystemParameters._cacheMenuDropAlignment == null)
				{
					SystemParameters._cacheMenuDropAlignment = SystemParameters.CreateInstance(SystemResourceKeyID.MenuDropAlignment);
				}
				return SystemParameters._cacheMenuDropAlignment;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuFade" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x000234E9 File Offset: 0x000216E9
		public static ResourceKey MenuFadeKey
		{
			get
			{
				if (SystemParameters._cacheMenuFade == null)
				{
					SystemParameters._cacheMenuFade = SystemParameters.CreateInstance(SystemResourceKeyID.MenuFade);
				}
				return SystemParameters._cacheMenuFade;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuShowDelay" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00023506 File Offset: 0x00021706
		public static ResourceKey MenuShowDelayKey
		{
			get
			{
				if (SystemParameters._cacheMenuShowDelay == null)
				{
					SystemParameters._cacheMenuShowDelay = SystemParameters.CreateInstance(SystemResourceKeyID.MenuShowDelay);
				}
				return SystemParameters._cacheMenuShowDelay;
			}
		}

		/// <summary>Gets the system value of the <see cref="P:System.Windows.Controls.Primitives.Popup.PopupAnimation" /> property for combo boxes. </summary>
		/// <returns>A pop-up animation value.</returns>
		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00023523 File Offset: 0x00021723
		public static PopupAnimation ComboBoxPopupAnimation
		{
			get
			{
				if (SystemParameters.ComboBoxAnimation)
				{
					return PopupAnimation.Slide;
				}
				return PopupAnimation.None;
			}
		}

		/// <summary>Gets a value indicating whether the slide-open effect for combo boxes is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> for enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00023530 File Offset: 0x00021730
		public static bool ComboBoxAnimation
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[22])
					{
						SystemParameters._cacheValid[22] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4100, 0, ref SystemParameters._comboBoxAnimation, 0))
						{
							SystemParameters._cacheValid[22] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._comboBoxAnimation;
			}
		}

		/// <summary>Gets a value indicating whether the client area animation feature is enabled.</summary>
		/// <returns>A Boolean value; true if client area animation is enabled, false otherwise.</returns>
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x000235B4 File Offset: 0x000217B4
		public static bool ClientAreaAnimation
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[23])
					{
						SystemParameters._cacheValid[23] = true;
						if (Environment.OSVersion.Version.Major >= 6)
						{
							if (!UnsafeNativeMethods.SystemParametersInfo(4162, 0, ref SystemParameters._clientAreaAnimation, 0))
							{
								SystemParameters._cacheValid[23] = false;
								throw new Win32Exception();
							}
						}
						else
						{
							SystemParameters._clientAreaAnimation = true;
						}
					}
				}
				return SystemParameters._clientAreaAnimation;
			}
		}

		/// <summary>Gets a value indicating whether the cursor has a shadow around it. </summary>
		/// <returns>
		///     <see langword="true" /> if the shadow is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00023650 File Offset: 0x00021850
		public static bool CursorShadow
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[24])
					{
						SystemParameters._cacheValid[24] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4122, 0, ref SystemParameters._cursorShadow, 0))
						{
							SystemParameters._cacheValid[24] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._cursorShadow;
			}
		}

		/// <summary>Gets a value indicating whether the gradient effect for window title bars is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the gradient effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x000236D4 File Offset: 0x000218D4
		public static bool GradientCaptions
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[25])
					{
						SystemParameters._cacheValid[25] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4104, 0, ref SystemParameters._gradientCaptions, 0))
						{
							SystemParameters._cacheValid[25] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._gradientCaptions;
			}
		}

		/// <summary>Gets a value indicating whether hot tracking of user-interface elements, such as menu names on menu bars, is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if hot tracking is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00023758 File Offset: 0x00021958
		public static bool HotTracking
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[26])
					{
						SystemParameters._cacheValid[26] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4110, 0, ref SystemParameters._hotTracking, 0))
						{
							SystemParameters._cacheValid[26] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._hotTracking;
			}
		}

		/// <summary>Gets a value indicating whether the smooth-scrolling effect for list boxes is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the smooth-scrolling effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x000237DC File Offset: 0x000219DC
		public static bool ListBoxSmoothScrolling
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[27])
					{
						SystemParameters._cacheValid[27] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4102, 0, ref SystemParameters._listBoxSmoothScrolling, 0))
						{
							SystemParameters._cacheValid[27] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._listBoxSmoothScrolling;
			}
		}

		/// <summary>Gets the system value of the <see cref="P:System.Windows.Controls.Primitives.Popup.PopupAnimation" /> property for menus. </summary>
		/// <returns>The pop-up animation property.</returns>
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00023860 File Offset: 0x00021A60
		public static PopupAnimation MenuPopupAnimation
		{
			get
			{
				if (!SystemParameters.MenuAnimation)
				{
					return PopupAnimation.None;
				}
				if (SystemParameters.MenuFade)
				{
					return PopupAnimation.Fade;
				}
				return PopupAnimation.Scroll;
			}
		}

		/// <summary>Gets a value indicating whether the menu animation feature is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if menu animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00023878 File Offset: 0x00021A78
		public static bool MenuAnimation
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[28])
					{
						SystemParameters._cacheValid[28] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4098, 0, ref SystemParameters._menuAnimation, 0))
						{
							SystemParameters._cacheValid[28] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._menuAnimation;
			}
		}

		/// <summary>Gets a value indicating whether the selection fade effect is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if the fade effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x000238FC File Offset: 0x00021AFC
		public static bool SelectionFade
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[29])
					{
						SystemParameters._cacheValid[29] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4116, 0, ref SystemParameters._selectionFade, 0))
						{
							SystemParameters._cacheValid[29] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._selectionFade;
			}
		}

		/// <summary>Gets a value indicating whether hot tracking of a stylus is enabled.  </summary>
		/// <returns>
		///     <see langword="true" /> if hot tracking of a stylus is enabled; otherwise <see langword="false" />.</returns>
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00023980 File Offset: 0x00021B80
		public static bool StylusHotTracking
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[30])
					{
						SystemParameters._cacheValid[30] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4112, 0, ref SystemParameters._stylusHotTracking, 0))
						{
							SystemParameters._cacheValid[30] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._stylusHotTracking;
			}
		}

		/// <summary>Gets the system value of the <see cref="P:System.Windows.Controls.Primitives.Popup.PopupAnimation" /> property for ToolTips. </summary>
		/// <returns>A system value for the pop-up animation property.</returns>
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00023A04 File Offset: 0x00021C04
		public static PopupAnimation ToolTipPopupAnimation
		{
			get
			{
				if (SystemParameters.ToolTipAnimation && SystemParameters.ToolTipFade)
				{
					return PopupAnimation.Fade;
				}
				return PopupAnimation.None;
			}
		}

		/// <summary>Gets a value indicating whether <see cref="T:System.Windows.Controls.ToolTip" /> animation is enabled.  </summary>
		/// <returns>
		///     <see langword="true" /> if ToolTip animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00023A18 File Offset: 0x00021C18
		public static bool ToolTipAnimation
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[31])
					{
						SystemParameters._cacheValid[31] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4118, 0, ref SystemParameters._toolTipAnimation, 0))
						{
							SystemParameters._cacheValid[31] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._toolTipAnimation;
			}
		}

		/// <summary>Gets a value indicating whether ToolTip animation uses a fade effect or a slide effect.  </summary>
		/// <returns>
		///     <see langword="true" /> if a fade effect is used; <see langword="false" /> if a slide effect is used.</returns>
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00023A9C File Offset: 0x00021C9C
		public static bool ToolTipFade
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[32])
					{
						SystemParameters._cacheValid[32] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4120, 0, ref SystemParameters._tooltipFade, 0))
						{
							SystemParameters._cacheValid[32] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._tooltipFade;
			}
		}

		/// <summary>Gets a value that indicates whether all user interface (UI) effects are enabled.   </summary>
		/// <returns>
		///     <see langword="true" /> if all UI effects are enabled; <see langword="false" /> if they are disabled.</returns>
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00023B20 File Offset: 0x00021D20
		public static bool UIEffects
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[33])
					{
						SystemParameters._cacheValid[33] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(4158, 0, ref SystemParameters._uiEffects, 0))
						{
							SystemParameters._cacheValid[33] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._uiEffects;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ComboBoxAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00023BA4 File Offset: 0x00021DA4
		public static ResourceKey ComboBoxAnimationKey
		{
			get
			{
				if (SystemParameters._cacheComboBoxAnimation == null)
				{
					SystemParameters._cacheComboBoxAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ComboBoxAnimation);
				}
				return SystemParameters._cacheComboBoxAnimation;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ClientAreaAnimation" /> property.</summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00023BC1 File Offset: 0x00021DC1
		public static ResourceKey ClientAreaAnimationKey
		{
			get
			{
				if (SystemParameters._cacheClientAreaAnimation == null)
				{
					SystemParameters._cacheClientAreaAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ClientAreaAnimation);
				}
				return SystemParameters._cacheClientAreaAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CursorShadow" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00023BDE File Offset: 0x00021DDE
		public static ResourceKey CursorShadowKey
		{
			get
			{
				if (SystemParameters._cacheCursorShadow == null)
				{
					SystemParameters._cacheCursorShadow = SystemParameters.CreateInstance(SystemResourceKeyID.CursorShadow);
				}
				return SystemParameters._cacheCursorShadow;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.GradientCaptions" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00023BFB File Offset: 0x00021DFB
		public static ResourceKey GradientCaptionsKey
		{
			get
			{
				if (SystemParameters._cacheGradientCaptions == null)
				{
					SystemParameters._cacheGradientCaptions = SystemParameters.CreateInstance(SystemResourceKeyID.GradientCaptions);
				}
				return SystemParameters._cacheGradientCaptions;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.HotTracking" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x00023C18 File Offset: 0x00021E18
		public static ResourceKey HotTrackingKey
		{
			get
			{
				if (SystemParameters._cacheHotTracking == null)
				{
					SystemParameters._cacheHotTracking = SystemParameters.CreateInstance(SystemResourceKeyID.HotTracking);
				}
				return SystemParameters._cacheHotTracking;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ListBoxSmoothScrolling" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00023C35 File Offset: 0x00021E35
		public static ResourceKey ListBoxSmoothScrollingKey
		{
			get
			{
				if (SystemParameters._cacheListBoxSmoothScrolling == null)
				{
					SystemParameters._cacheListBoxSmoothScrolling = SystemParameters.CreateInstance(SystemResourceKeyID.ListBoxSmoothScrolling);
				}
				return SystemParameters._cacheListBoxSmoothScrolling;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00023C52 File Offset: 0x00021E52
		public static ResourceKey MenuAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMenuAnimation == null)
				{
					SystemParameters._cacheMenuAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MenuAnimation);
				}
				return SystemParameters._cacheMenuAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SelectionFade" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00023C6F File Offset: 0x00021E6F
		public static ResourceKey SelectionFadeKey
		{
			get
			{
				if (SystemParameters._cacheSelectionFade == null)
				{
					SystemParameters._cacheSelectionFade = SystemParameters.CreateInstance(SystemResourceKeyID.SelectionFade);
				}
				return SystemParameters._cacheSelectionFade;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.StylusHotTracking" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00023C8C File Offset: 0x00021E8C
		public static ResourceKey StylusHotTrackingKey
		{
			get
			{
				if (SystemParameters._cacheStylusHotTracking == null)
				{
					SystemParameters._cacheStylusHotTracking = SystemParameters.CreateInstance(SystemResourceKeyID.StylusHotTracking);
				}
				return SystemParameters._cacheStylusHotTracking;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ToolTipAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00023CA9 File Offset: 0x00021EA9
		public static ResourceKey ToolTipAnimationKey
		{
			get
			{
				if (SystemParameters._cacheToolTipAnimation == null)
				{
					SystemParameters._cacheToolTipAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipAnimation);
				}
				return SystemParameters._cacheToolTipAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ToolTipFade" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x00023CC6 File Offset: 0x00021EC6
		public static ResourceKey ToolTipFadeKey
		{
			get
			{
				if (SystemParameters._cacheToolTipFade == null)
				{
					SystemParameters._cacheToolTipFade = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipFade);
				}
				return SystemParameters._cacheToolTipFade;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.UIEffects" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00023CE3 File Offset: 0x00021EE3
		public static ResourceKey UIEffectsKey
		{
			get
			{
				if (SystemParameters._cacheUIEffects == null)
				{
					SystemParameters._cacheUIEffects = SystemParameters.CreateInstance(SystemResourceKeyID.UIEffects);
				}
				return SystemParameters._cacheUIEffects;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ComboBoxPopupAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00023D00 File Offset: 0x00021F00
		public static ResourceKey ComboBoxPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheComboBoxPopupAnimation == null)
				{
					SystemParameters._cacheComboBoxPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ComboBoxPopupAnimation);
				}
				return SystemParameters._cacheComboBoxPopupAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuPopupAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00023D1D File Offset: 0x00021F1D
		public static ResourceKey MenuPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMenuPopupAnimation == null)
				{
					SystemParameters._cacheMenuPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MenuPopupAnimation);
				}
				return SystemParameters._cacheMenuPopupAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ToolTipPopupAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00023D3A File Offset: 0x00021F3A
		public static ResourceKey ToolTipPopupAnimationKey
		{
			get
			{
				if (SystemParameters._cacheToolTipPopupAnimation == null)
				{
					SystemParameters._cacheToolTipPopupAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.ToolTipPopupAnimation);
				}
				return SystemParameters._cacheToolTipPopupAnimation;
			}
		}

		/// <summary>Gets the animation effects associated with user actions. </summary>
		/// <returns>
		///     <see langword="true" /> if the minimize window animations feature is enabled; otherwise,<see langword=" false" />.</returns>
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00023D58 File Offset: 0x00021F58
		public static bool MinimizeAnimation
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[34])
					{
						SystemParameters._cacheValid[34] = true;
						NativeMethods.ANIMATIONINFO animationinfo = new NativeMethods.ANIMATIONINFO();
						if (!UnsafeNativeMethods.SystemParametersInfo(72, animationinfo.cbSize, animationinfo, 0))
						{
							SystemParameters._cacheValid[34] = false;
							throw new Win32Exception();
						}
						SystemParameters._minAnimation = (animationinfo.iMinAnimate != 0);
					}
				}
				return SystemParameters._minAnimation;
			}
		}

		/// <summary>Gets the border multiplier factor that determines the width of a window's sizing border. </summary>
		/// <returns>A multiplier.</returns>
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00023DF0 File Offset: 0x00021FF0
		public static int Border
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[35])
					{
						SystemParameters._cacheValid[35] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(5, 0, ref SystemParameters._border, 0))
						{
							SystemParameters._cacheValid[35] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._border;
			}
		}

		/// <summary>Gets the caret width, in pixels, for edit controls. </summary>
		/// <returns>The caret width.</returns>
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00023E70 File Offset: 0x00022070
		public static double CaretWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[36])
					{
						SystemParameters._cacheValid[36] = true;
						int num = 0;
						using (DpiUtil.WithDpiAwarenessContext(DpiAwarenessContextValue.Unaware))
						{
							if (!UnsafeNativeMethods.SystemParametersInfo(8198, 0, ref num, 0))
							{
								SystemParameters._cacheValid[36] = false;
								throw new Win32Exception();
							}
							SystemParameters._caretWidth = (double)num;
						}
					}
				}
				return SystemParameters._caretWidth;
			}
		}

		/// <summary>Gets a value indicating whether dragging of full windows is enabled. </summary>
		/// <returns>
		///     <see langword="true" /> if dragging of full windows is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00023F18 File Offset: 0x00022118
		public static bool DragFullWindows
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[38])
					{
						SystemParameters._cacheValid[38] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(38, 0, ref SystemParameters._dragFullWindows, 0))
						{
							SystemParameters._cacheValid[38] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._dragFullWindows;
			}
		}

		/// <summary>Gets the number of times the Set Foreground Window flashes the taskbar button when rejecting a foreground switch request.</summary>
		/// <returns>A flash count.</returns>
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x00023F98 File Offset: 0x00022198
		public static int ForegroundFlashCount
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[37])
					{
						SystemParameters._cacheValid[37] = true;
						if (!UnsafeNativeMethods.SystemParametersInfo(8196, 0, ref SystemParameters._foregroundFlashCount, 0))
						{
							SystemParameters._cacheValid[37] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._foregroundFlashCount;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002401C File Offset: 0x0002221C
		internal static NativeMethods.NONCLIENTMETRICS NonClientMetrics
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[39])
					{
						SystemParameters._cacheValid[39] = true;
						SystemParameters._ncm = new NativeMethods.NONCLIENTMETRICS();
						if (!UnsafeNativeMethods.SystemParametersInfo(41, SystemParameters._ncm.cbSize, SystemParameters._ncm, 0))
						{
							SystemParameters._cacheValid[39] = false;
							throw new Win32Exception();
						}
					}
				}
				return SystemParameters._ncm;
			}
		}

		/// <summary>Gets the metric that determines the border width of the nonclient area of a nonminimized window. </summary>
		/// <returns>A border width.</returns>
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x000240B0 File Offset: 0x000222B0
		public static double BorderWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iBorderWidth);
			}
		}

		/// <summary>Gets the metric that determines the scroll width of the nonclient area of a nonminimized window. </summary>
		/// <returns>The scroll width, in pixels.</returns>
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000240C1 File Offset: 0x000222C1
		public static double ScrollWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iScrollWidth);
			}
		}

		/// <summary>Gets the metric that determines the scroll height of the nonclient area of a nonminimized window. </summary>
		/// <returns>The scroll height, in pixels.</returns>
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x000240D2 File Offset: 0x000222D2
		public static double ScrollHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iScrollHeight);
			}
		}

		/// <summary>Gets the metric that determines the caption width for the nonclient area of a nonminimized window. </summary>
		/// <returns>The caption width.</returns>
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x000240E3 File Offset: 0x000222E3
		public static double CaptionWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iCaptionWidth);
			}
		}

		/// <summary>Gets the metric that determines the caption height for the nonclient area of a nonminimized window. </summary>
		/// <returns>The caption height.</returns>
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x000240F4 File Offset: 0x000222F4
		public static double CaptionHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iCaptionHeight);
			}
		}

		/// <summary>Gets the metric that determines the width of the small caption of the nonclient area of a nonminimized window. </summary>
		/// <returns>The caption width, in pixels.</returns>
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00024105 File Offset: 0x00022305
		public static double SmallCaptionWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iSmCaptionWidth);
			}
		}

		/// <summary>Gets the metric that determines the height of the small caption of the nonclient area of a nonminimized window. </summary>
		/// <returns>The caption height, in pixels.</returns>
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x00024116 File Offset: 0x00022316
		public static double SmallCaptionHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iSmCaptionHeight);
			}
		}

		/// <summary>Gets the metric that determines the width of the menu. </summary>
		/// <returns>The menu width, in pixels.</returns>
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00024127 File Offset: 0x00022327
		public static double MenuWidth
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iMenuWidth);
			}
		}

		/// <summary>Gets the metric that determines the height of the menu. </summary>
		/// <returns>The menu height.</returns>
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00024138 File Offset: 0x00022338
		public static double MenuHeight
		{
			get
			{
				return SystemParameters.ConvertPixel(SystemParameters.NonClientMetrics.iMenuHeight);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimizeAnimation" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00024149 File Offset: 0x00022349
		public static ResourceKey MinimizeAnimationKey
		{
			get
			{
				if (SystemParameters._cacheMinimizeAnimation == null)
				{
					SystemParameters._cacheMinimizeAnimation = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizeAnimation);
				}
				return SystemParameters._cacheMinimizeAnimation;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.Border" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00024166 File Offset: 0x00022366
		public static ResourceKey BorderKey
		{
			get
			{
				if (SystemParameters._cacheBorder == null)
				{
					SystemParameters._cacheBorder = SystemParameters.CreateInstance(SystemResourceKeyID.Border);
				}
				return SystemParameters._cacheBorder;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CaretWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00024183 File Offset: 0x00022383
		public static ResourceKey CaretWidthKey
		{
			get
			{
				if (SystemParameters._cacheCaretWidth == null)
				{
					SystemParameters._cacheCaretWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CaretWidth);
				}
				return SystemParameters._cacheCaretWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ForegroundFlashCount" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x000241A0 File Offset: 0x000223A0
		public static ResourceKey ForegroundFlashCountKey
		{
			get
			{
				if (SystemParameters._cacheForegroundFlashCount == null)
				{
					SystemParameters._cacheForegroundFlashCount = SystemParameters.CreateInstance(SystemResourceKeyID.ForegroundFlashCount);
				}
				return SystemParameters._cacheForegroundFlashCount;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.DragFullWindows" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x000241BD File Offset: 0x000223BD
		public static ResourceKey DragFullWindowsKey
		{
			get
			{
				if (SystemParameters._cacheDragFullWindows == null)
				{
					SystemParameters._cacheDragFullWindows = SystemParameters.CreateInstance(SystemResourceKeyID.DragFullWindows);
				}
				return SystemParameters._cacheDragFullWindows;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.BorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x000241DA File Offset: 0x000223DA
		public static ResourceKey BorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheBorderWidth == null)
				{
					SystemParameters._cacheBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.BorderWidth);
				}
				return SystemParameters._cacheBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ScrollWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x000241F7 File Offset: 0x000223F7
		public static ResourceKey ScrollWidthKey
		{
			get
			{
				if (SystemParameters._cacheScrollWidth == null)
				{
					SystemParameters._cacheScrollWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ScrollWidth);
				}
				return SystemParameters._cacheScrollWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ScrollHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00024214 File Offset: 0x00022414
		public static ResourceKey ScrollHeightKey
		{
			get
			{
				if (SystemParameters._cacheScrollHeight == null)
				{
					SystemParameters._cacheScrollHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ScrollHeight);
				}
				return SystemParameters._cacheScrollHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CaptionWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00024231 File Offset: 0x00022431
		public static ResourceKey CaptionWidthKey
		{
			get
			{
				if (SystemParameters._cacheCaptionWidth == null)
				{
					SystemParameters._cacheCaptionWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CaptionWidth);
				}
				return SystemParameters._cacheCaptionWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CaptionHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0002424E File Offset: 0x0002244E
		public static ResourceKey CaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheCaptionHeight == null)
				{
					SystemParameters._cacheCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.CaptionHeight);
				}
				return SystemParameters._cacheCaptionHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallCaptionWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0002426B File Offset: 0x0002246B
		public static ResourceKey SmallCaptionWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallCaptionWidth == null)
				{
					SystemParameters._cacheSmallCaptionWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallCaptionWidth);
				}
				return SystemParameters._cacheSmallCaptionWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00024288 File Offset: 0x00022488
		public static ResourceKey MenuWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuWidth == null)
				{
					SystemParameters._cacheMenuWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuWidth);
				}
				return SystemParameters._cacheMenuWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x000242A5 File Offset: 0x000224A5
		public static ResourceKey MenuHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuHeight == null)
				{
					SystemParameters._cacheMenuHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuHeight);
				}
				return SystemParameters._cacheMenuHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a horizontal window border. </summary>
		/// <returns>The height of a border.</returns>
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000242C4 File Offset: 0x000224C4
		public static double ThinHorizontalBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[40])
					{
						SystemParameters._cacheValid[40] = true;
						SystemParameters._thinHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXBORDER));
					}
				}
				return SystemParameters._thinHorizontalBorderHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a vertical window border. </summary>
		/// <returns>The width of a border.</returns>
		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00024330 File Offset: 0x00022530
		public static double ThinVerticalBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[41])
					{
						SystemParameters._cacheValid[41] = true;
						SystemParameters._thinVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYBORDER));
					}
				}
				return SystemParameters._thinVerticalBorderWidth;
			}
		}

		/// <summary>Gets the width, in pixels, of a cursor. </summary>
		/// <returns>The cursor width.</returns>
		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0002439C File Offset: 0x0002259C
		public static double CursorWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[42])
					{
						SystemParameters._cacheValid[42] = true;
						SystemParameters._cursorWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXCURSOR));
					}
				}
				return SystemParameters._cursorWidth;
			}
		}

		/// <summary>Gets the height, in pixels, of a cursor. </summary>
		/// <returns>The cursor height.</returns>
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00024408 File Offset: 0x00022608
		public static double CursorHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[43])
					{
						SystemParameters._cacheValid[43] = true;
						SystemParameters._cursorHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYCURSOR));
					}
				}
				return SystemParameters._cursorHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a 3-D border.   </summary>
		/// <returns>The height of a border.</returns>
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00024474 File Offset: 0x00022674
		public static double ThickHorizontalBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[44])
					{
						SystemParameters._cacheValid[44] = true;
						SystemParameters._thickHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXEDGE));
					}
				}
				return SystemParameters._thickHorizontalBorderHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a 3-D border.   </summary>
		/// <returns>The width of a border.</returns>
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x000244E0 File Offset: 0x000226E0
		public static double ThickVerticalBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[45])
					{
						SystemParameters._cacheValid[45] = true;
						SystemParameters._thickVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYEDGE));
					}
				}
				return SystemParameters._thickVerticalBorderWidth;
			}
		}

		/// <summary>Gets the width of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins.  </summary>
		/// <returns>The width of the rectangle, in pixels.</returns>
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0002454C File Offset: 0x0002274C
		public static double MinimumHorizontalDragDistance
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[46])
					{
						SystemParameters._cacheValid[46] = true;
						SystemParameters._minimumHorizontalDragDistance = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXDRAG));
					}
				}
				return SystemParameters._minimumHorizontalDragDistance;
			}
		}

		/// <summary>Gets the height of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins.  </summary>
		/// <returns>The height of the rectangle, in pixels.</returns>
		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x000245B8 File Offset: 0x000227B8
		public static double MinimumVerticalDragDistance
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[47])
					{
						SystemParameters._cacheValid[47] = true;
						SystemParameters._minimumVerticalDragDistance = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYDRAG));
					}
				}
				return SystemParameters._minimumVerticalDragDistance;
			}
		}

		/// <summary>Gets the height of the horizontal border of the frame around a window. </summary>
		/// <returns>The border height.</returns>
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00024624 File Offset: 0x00022824
		public static double FixedFrameHorizontalBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[48])
					{
						SystemParameters._cacheValid[48] = true;
						SystemParameters._fixedFrameHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFIXEDFRAME));
					}
				}
				return SystemParameters._fixedFrameHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the width of the vertical border of the frame around a window. </summary>
		/// <returns>The border width.</returns>
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00024690 File Offset: 0x00022890
		public static double FixedFrameVerticalBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[49])
					{
						SystemParameters._cacheValid[49] = true;
						SystemParameters._fixedFrameVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFIXEDFRAME));
					}
				}
				return SystemParameters._fixedFrameVerticalBorderWidth;
			}
		}

		/// <summary>Gets the height of the upper and lower edges of the focus rectangle.  </summary>
		/// <returns>The edge height.</returns>
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000246FC File Offset: 0x000228FC
		public static double FocusHorizontalBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[50])
					{
						SystemParameters._cacheValid[50] = true;
						SystemParameters._focusHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFOCUSBORDER));
					}
				}
				return SystemParameters._focusHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the width of the left and right edges of the focus rectangle.  </summary>
		/// <returns>The edge width.</returns>
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00024768 File Offset: 0x00022968
		public static double FocusVerticalBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[51])
					{
						SystemParameters._cacheValid[51] = true;
						SystemParameters._focusVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFOCUSBORDER));
					}
				}
				return SystemParameters._focusVerticalBorderWidth;
			}
		}

		/// <summary>Gets the width, in pixels, of the client area for a full-screen window on the primary display monitor.  </summary>
		/// <returns>The width of the client area.</returns>
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000247D4 File Offset: 0x000229D4
		public static double FullPrimaryScreenWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[52])
					{
						SystemParameters._cacheValid[52] = true;
						SystemParameters._fullPrimaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFULLSCREEN));
					}
				}
				return SystemParameters._fullPrimaryScreenWidth;
			}
		}

		/// <summary>Gets the height, in pixels, of the client area for a full-screen window on the primary display monitor.  </summary>
		/// <returns>The height of the client area.</returns>
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00024848 File Offset: 0x00022A48
		public static double FullPrimaryScreenHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[53])
					{
						SystemParameters._cacheValid[53] = true;
						SystemParameters._fullPrimaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFULLSCREEN));
					}
				}
				return SystemParameters._fullPrimaryScreenHeight;
			}
		}

		/// <summary>Gets the width, in pixels, of the arrow bitmap on a horizontal scroll bar. </summary>
		/// <returns>The width of the arrow bitmap.</returns>
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000248BC File Offset: 0x00022ABC
		public static double HorizontalScrollBarButtonWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[54])
					{
						SystemParameters._cacheValid[54] = true;
						SystemParameters._horizontalScrollBarButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXHSCROLL));
					}
				}
				return SystemParameters._horizontalScrollBarButtonWidth;
			}
		}

		/// <summary>Gets the height of a horizontal scroll bar, in pixels. </summary>
		/// <returns>The height of the scroll bar.</returns>
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00024928 File Offset: 0x00022B28
		public static double HorizontalScrollBarHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[55])
					{
						SystemParameters._cacheValid[55] = true;
						SystemParameters._horizontalScrollBarHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYHSCROLL));
					}
				}
				return SystemParameters._horizontalScrollBarHeight;
			}
		}

		/// <summary>Gets the width, in pixels, of the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> in a horizontal scroll bar. </summary>
		/// <returns>The width of the thumb.</returns>
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00024994 File Offset: 0x00022B94
		public static double HorizontalScrollBarThumbWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[56])
					{
						SystemParameters._cacheValid[56] = true;
						SystemParameters._horizontalScrollBarThumbWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXHTHUMB));
					}
				}
				return SystemParameters._horizontalScrollBarThumbWidth;
			}
		}

		/// <summary>Gets the default width of an icon. </summary>
		/// <returns>The icon width.</returns>
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00024A00 File Offset: 0x00022C00
		public static double IconWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[57])
					{
						SystemParameters._cacheValid[57] = true;
						SystemParameters._iconWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXICON));
					}
				}
				return SystemParameters._iconWidth;
			}
		}

		/// <summary>Gets the default height of an icon. </summary>
		/// <returns>The icon height.</returns>
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00024A6C File Offset: 0x00022C6C
		public static double IconHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[58])
					{
						SystemParameters._cacheValid[58] = true;
						SystemParameters._iconHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYICON));
					}
				}
				return SystemParameters._iconHeight;
			}
		}

		/// <summary>Gets the width of a grid that a large icon will fit into. </summary>
		/// <returns>The grid width.</returns>
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00024AD8 File Offset: 0x00022CD8
		public static double IconGridWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[59])
					{
						SystemParameters._cacheValid[59] = true;
						SystemParameters._iconGridWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXICONSPACING));
					}
				}
				return SystemParameters._iconGridWidth;
			}
		}

		/// <summary>Gets the height of a grid in which a large icon will fit. </summary>
		/// <returns>The grid height.</returns>
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00024B44 File Offset: 0x00022D44
		public static double IconGridHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[60])
					{
						SystemParameters._cacheValid[60] = true;
						SystemParameters._iconGridHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYICONSPACING));
					}
				}
				return SystemParameters._iconGridHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a maximized top-level window on the primary display monitor.  </summary>
		/// <returns>The window width.</returns>
		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00024BB0 File Offset: 0x00022DB0
		public static double MaximizedPrimaryScreenWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[61])
					{
						SystemParameters._cacheValid[61] = true;
						SystemParameters._maximizedPrimaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMAXIMIZED));
					}
				}
				return SystemParameters._maximizedPrimaryScreenWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a maximized top-level window on the primary display monitor.  </summary>
		/// <returns>The window height.</returns>
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00024C24 File Offset: 0x00022E24
		public static double MaximizedPrimaryScreenHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[62])
					{
						SystemParameters._cacheValid[62] = true;
						SystemParameters._maximizedPrimaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMAXIMIZED));
					}
				}
				return SystemParameters._maximizedPrimaryScreenHeight;
			}
		}

		/// <summary>Gets a value that indicates the maximum width, in pixels, of a window that has a caption and sizing borders.  </summary>
		/// <returns>The maximum window width.</returns>
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00024C98 File Offset: 0x00022E98
		public static double MaximumWindowTrackWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[63])
					{
						SystemParameters._cacheValid[63] = true;
						SystemParameters._maximumWindowTrackWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMAXTRACK));
					}
				}
				return SystemParameters._maximumWindowTrackWidth;
			}
		}

		/// <summary>Gets a value that indicates the maximum height, in pixels, of a window that has a caption and sizing borders.  </summary>
		/// <returns>The maximum window height.</returns>
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00024D0C File Offset: 0x00022F0C
		public static double MaximumWindowTrackHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[64])
					{
						SystemParameters._cacheValid[64] = true;
						SystemParameters._maximumWindowTrackHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMAXTRACK));
					}
				}
				return SystemParameters._maximumWindowTrackHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of the default menu check-mark bitmap.  </summary>
		/// <returns>The width of the bitmap.</returns>
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00024D80 File Offset: 0x00022F80
		public static double MenuCheckmarkWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[65])
					{
						SystemParameters._cacheValid[65] = true;
						SystemParameters._menuCheckmarkWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMENUCHECK));
					}
				}
				return SystemParameters._menuCheckmarkWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of the default menu check-mark bitmap.  </summary>
		/// <returns>The height of a bitmap.</returns>
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00024DEC File Offset: 0x00022FEC
		public static double MenuCheckmarkHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[66])
					{
						SystemParameters._cacheValid[66] = true;
						SystemParameters._menuCheckmarkHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENUCHECK));
					}
				}
				return SystemParameters._menuCheckmarkHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a menu bar button.  </summary>
		/// <returns>The width of a menu bar button.</returns>
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00024E58 File Offset: 0x00023058
		public static double MenuButtonWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[67])
					{
						SystemParameters._cacheValid[67] = true;
						SystemParameters._menuButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMENUSIZE));
					}
				}
				return SystemParameters._menuButtonWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a menu bar button.  </summary>
		/// <returns>The height of a menu bar button.</returns>
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00024EC4 File Offset: 0x000230C4
		public static double MenuButtonHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[68])
					{
						SystemParameters._cacheValid[68] = true;
						SystemParameters._menuButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENUSIZE));
					}
				}
				return SystemParameters._menuButtonHeight;
			}
		}

		/// <summary>Gets a value that indicates the minimum width, in pixels, of a window.  </summary>
		/// <returns>The minimum width of a window.</returns>
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00024F30 File Offset: 0x00023130
		public static double MinimumWindowWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[69])
					{
						SystemParameters._cacheValid[69] = true;
						SystemParameters._minimumWindowWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMIN));
					}
				}
				return SystemParameters._minimumWindowWidth;
			}
		}

		/// <summary>Gets a value that indicates the minimum height, in pixels, of a window.  </summary>
		/// <returns>The minimum height of a window.</returns>
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00024FA4 File Offset: 0x000231A4
		public static double MinimumWindowHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[70])
					{
						SystemParameters._cacheValid[70] = true;
						SystemParameters._minimumWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMIN));
					}
				}
				return SystemParameters._minimumWindowHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a minimized window.  </summary>
		/// <returns>The width of a minimized window.</returns>
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00025018 File Offset: 0x00023218
		public static double MinimizedWindowWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[71])
					{
						SystemParameters._cacheValid[71] = true;
						SystemParameters._minimizedWindowWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINIMIZED));
					}
				}
				return SystemParameters._minimizedWindowWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a minimized window.  </summary>
		/// <returns>The height of a minimized window.</returns>
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0002508C File Offset: 0x0002328C
		public static double MinimizedWindowHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[72])
					{
						SystemParameters._cacheValid[72] = true;
						SystemParameters._minimizedWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINIMIZED));
					}
				}
				return SystemParameters._minimizedWindowHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a grid cell for a minimized window.  </summary>
		/// <returns>The width of a grid cell for a minimized window.</returns>
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00025100 File Offset: 0x00023300
		public static double MinimizedGridWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[73])
					{
						SystemParameters._cacheValid[73] = true;
						SystemParameters._minimizedGridWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINSPACING));
					}
				}
				return SystemParameters._minimizedGridWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a grid cell for a minimized window.  </summary>
		/// <returns>The height of a grid cell for a minimized window.</returns>
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0002516C File Offset: 0x0002336C
		public static double MinimizedGridHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[74])
					{
						SystemParameters._cacheValid[74] = true;
						SystemParameters._minimizedGridHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINSPACING));
					}
				}
				return SystemParameters._minimizedGridHeight;
			}
		}

		/// <summary>Gets a value that indicates the minimum tracking width of a window, in pixels.   </summary>
		/// <returns>The minimum tracking width of a window.</returns>
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000251D8 File Offset: 0x000233D8
		public static double MinimumWindowTrackWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[75])
					{
						SystemParameters._cacheValid[75] = true;
						SystemParameters._minimumWindowTrackWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXMINTRACK));
					}
				}
				return SystemParameters._minimumWindowTrackWidth;
			}
		}

		/// <summary>Gets a value that indicates the minimum tracking height of a window, in pixels.   </summary>
		/// <returns>The minimun tracking height of a window.</returns>
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0002524C File Offset: 0x0002344C
		public static double MinimumWindowTrackHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[76])
					{
						SystemParameters._cacheValid[76] = true;
						SystemParameters._minimumWindowTrackHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMINTRACK));
					}
				}
				return SystemParameters._minimumWindowTrackHeight;
			}
		}

		/// <summary>Gets a value that indicates the screen width, in pixels, of the primary display monitor.   </summary>
		/// <returns>The width of the screen.</returns>
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x000252C0 File Offset: 0x000234C0
		public static double PrimaryScreenWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[77])
					{
						SystemParameters._cacheValid[77] = true;
						SystemParameters._primaryScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSCREEN));
					}
				}
				return SystemParameters._primaryScreenWidth;
			}
		}

		/// <summary>Gets a value that indicates the screen height, in pixels, of the primary display monitor.   </summary>
		/// <returns>The height of the screen.</returns>
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0002532C File Offset: 0x0002352C
		public static double PrimaryScreenHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[78])
					{
						SystemParameters._cacheValid[78] = true;
						SystemParameters._primaryScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSCREEN));
					}
				}
				return SystemParameters._primaryScreenHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a button in the title bar of a window.  </summary>
		/// <returns>The width of a caption button.</returns>
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00025398 File Offset: 0x00023598
		public static double WindowCaptionButtonWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[79])
					{
						SystemParameters._cacheValid[79] = true;
						SystemParameters._windowCaptionButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSIZE));
					}
				}
				return SystemParameters._windowCaptionButtonWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a button in the title bar of a window.  </summary>
		/// <returns>The height of a caption button.</returns>
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0002540C File Offset: 0x0002360C
		public static double WindowCaptionButtonHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[80])
					{
						SystemParameters._cacheValid[80] = true;
						SystemParameters._windowCaptionButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSIZE));
					}
				}
				return SystemParameters._windowCaptionButtonHeight;
			}
		}

		/// <summary>Gets a value that indicates the height (thickness), in pixels, of the horizontal sizing border around the perimeter of a window that can be resized.   </summary>
		/// <returns>The height of the border.</returns>
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00025478 File Offset: 0x00023678
		public static double ResizeFrameHorizontalBorderHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[81])
					{
						SystemParameters._cacheValid[81] = true;
						SystemParameters._resizeFrameHorizontalBorderHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXFRAME));
					}
				}
				return SystemParameters._resizeFrameHorizontalBorderHeight;
			}
		}

		/// <summary>Gets a value that indicates the width (thickness), in pixels, of the vertical sizing border around the perimeter of a window that can be resized.   </summary>
		/// <returns>The width of the border.</returns>
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000254E4 File Offset: 0x000236E4
		public static double ResizeFrameVerticalBorderWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[82])
					{
						SystemParameters._cacheValid[82] = true;
						SystemParameters._resizeFrameVerticalBorderWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYFRAME));
					}
				}
				return SystemParameters._resizeFrameVerticalBorderWidth;
			}
		}

		/// <summary>Gets a value that indicates the recommended width, in pixels, of a small icon. </summary>
		/// <returns>The width of the icon.</returns>
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00025550 File Offset: 0x00023750
		public static double SmallIconWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[83])
					{
						SystemParameters._cacheValid[83] = true;
						SystemParameters._smallIconWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON));
					}
				}
				return SystemParameters._smallIconWidth;
			}
		}

		/// <summary>Gets a value that indicates the recommended height, in pixels, of a small icon. </summary>
		/// <returns>The icon height.</returns>
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x000255BC File Offset: 0x000237BC
		public static double SmallIconHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[84])
					{
						SystemParameters._cacheValid[84] = true;
						SystemParameters._smallIconHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON));
					}
				}
				return SystemParameters._smallIconHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of small caption buttons.  </summary>
		/// <returns>The width of the caption button.</returns>
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00025628 File Offset: 0x00023828
		public static double SmallWindowCaptionButtonWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[85])
					{
						SystemParameters._cacheValid[85] = true;
						SystemParameters._smallWindowCaptionButtonWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXSMSIZE));
					}
				}
				return SystemParameters._smallWindowCaptionButtonWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of small caption buttons.  </summary>
		/// <returns>The height of the caption button.</returns>
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00025694 File Offset: 0x00023894
		public static double SmallWindowCaptionButtonHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[86])
					{
						SystemParameters._cacheValid[86] = true;
						SystemParameters._smallWindowCaptionButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYSMSIZE));
					}
				}
				return SystemParameters._smallWindowCaptionButtonHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of the virtual screen.   </summary>
		/// <returns>The width of the virtual screen.</returns>
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00025700 File Offset: 0x00023900
		public static double VirtualScreenWidth
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[87])
					{
						SystemParameters._cacheValid[87] = true;
						SystemParameters._virtualScreenWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of the virtual screen.   </summary>
		/// <returns>The height of the virtual screen.</returns>
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00025774 File Offset: 0x00023974
		public static double VirtualScreenHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[88])
					{
						SystemParameters._cacheValid[88] = true;
						SystemParameters._virtualScreenHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenHeight;
			}
		}

		/// <summary>Gets a value that indicates the width, in pixels, of a vertical scroll bar.  </summary>
		/// <returns>The width of a scroll bar.</returns>
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x000257E8 File Offset: 0x000239E8
		public static double VerticalScrollBarWidth
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[89])
					{
						SystemParameters._cacheValid[89] = true;
						SystemParameters._verticalScrollBarWidth = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CXVSCROLL));
					}
				}
				return SystemParameters._verticalScrollBarWidth;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of the arrow bitmap on a vertical scroll bar.  </summary>
		/// <returns>The height of a bitmap.</returns>
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00025854 File Offset: 0x00023A54
		public static double VerticalScrollBarButtonHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[90])
					{
						SystemParameters._cacheValid[90] = true;
						SystemParameters._verticalScrollBarButtonHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVSCROLL));
					}
				}
				return SystemParameters._verticalScrollBarButtonHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a caption area.  </summary>
		/// <returns>The height of a caption area.</returns>
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x000258C0 File Offset: 0x00023AC0
		public static double WindowCaptionHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[91])
					{
						SystemParameters._cacheValid[91] = true;
						SystemParameters._windowCaptionHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYCAPTION));
					}
				}
				return SystemParameters._windowCaptionHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of the kanji window at the bottom of the screen for systems that use double-byte characters.  </summary>
		/// <returns>The window height.</returns>
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00025930 File Offset: 0x00023B30
		public static double KanjiWindowHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[92])
					{
						SystemParameters._cacheValid[92] = true;
						SystemParameters._kanjiWindowHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYKANJIWINDOW));
					}
				}
				return SystemParameters._kanjiWindowHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of a single-line menu bar.  </summary>
		/// <returns>The height of the menu bar.</returns>
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x000259A4 File Offset: 0x00023BA4
		public static double MenuBarHeight
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[93])
					{
						SystemParameters._cacheValid[93] = true;
						SystemParameters._menuBarHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYMENU));
					}
				}
				return SystemParameters._menuBarHeight;
			}
		}

		/// <summary>Gets a value that indicates the height, in pixels, of the thumb in a vertical scroll bar.  </summary>
		/// <returns>The height of the thumb.</returns>
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00025A18 File Offset: 0x00023C18
		public static double VerticalScrollBarThumbHeight
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[94])
					{
						SystemParameters._cacheValid[94] = true;
						SystemParameters._verticalScrollBarThumbHeight = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.CYVTHUMB));
					}
				}
				return SystemParameters._verticalScrollBarThumbHeight;
			}
		}

		/// <summary>Gets a value that indicates whether the system is ready to use a Unicode-based Input Method Editor (IME) on a Unicode application.  </summary>
		/// <returns>
		///     <see langword="true" /> if the Input Method Manager/Input Method Editor features are enabled; otherwise, <see langword="false" />.<see langword="" /></returns>
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00025A84 File Offset: 0x00023C84
		public static bool IsImmEnabled
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[95])
					{
						SystemParameters._cacheValid[95] = true;
						SystemParameters._isImmEnabled = (UnsafeNativeMethods.GetSystemMetrics(SM.IMMENABLED) != 0);
					}
				}
				return SystemParameters._isImmEnabled;
			}
		}

		/// <summary>Gets a value that indicates whether the current operating system is the Microsoft Windows XP Media Center Edition. </summary>
		/// <returns>
		///     <see langword="true" /> if the current operating system is Windows XP Media Center Edition; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00025AF4 File Offset: 0x00023CF4
		public static bool IsMediaCenter
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[96])
					{
						SystemParameters._cacheValid[96] = true;
						SystemParameters._isMediaCenter = (UnsafeNativeMethods.GetSystemMetrics(SM.MEDIACENTER) != 0);
					}
				}
				return SystemParameters._isMediaCenter;
			}
		}

		/// <summary>Gets a value that indicates whether drop-down menus are right-aligned with the corresponding menu item. </summary>
		/// <returns>
		///     <see langword="true" /> if drop-down menus are right-aligned; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00025B64 File Offset: 0x00023D64
		public static bool IsMenuDropRightAligned
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[97])
					{
						SystemParameters._cacheValid[97] = true;
						SystemParameters._isMenuDropRightAligned = (UnsafeNativeMethods.GetSystemMetrics(SM.MENUDROPALIGNMENT) != 0);
					}
				}
				return SystemParameters._isMenuDropRightAligned;
			}
		}

		/// <summary>Gets a value that indicates whether the system is enabled for Hebrew and Arabic languages. </summary>
		/// <returns>
		///     <see langword="true" /> if the system is enabled for Hebrew and Arabic languages; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00025BD0 File Offset: 0x00023DD0
		public static bool IsMiddleEastEnabled
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[98])
					{
						SystemParameters._cacheValid[98] = true;
						SystemParameters._isMiddleEastEnabled = (UnsafeNativeMethods.GetSystemMetrics(SM.MIDEASTENABLED) != 0);
					}
				}
				return SystemParameters._isMiddleEastEnabled;
			}
		}

		/// <summary>Gets a value that indicates whether a mouse is installed. </summary>
		/// <returns>
		///     <see langword="true" /> if a mouse is installed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00025C40 File Offset: 0x00023E40
		public static bool IsMousePresent
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[99])
					{
						SystemParameters._cacheValid[99] = true;
						SystemParameters._isMousePresent = (UnsafeNativeMethods.GetSystemMetrics(SM.MOUSEPRESENT) != 0);
					}
				}
				return SystemParameters._isMousePresent;
			}
		}

		/// <summary>Gets a value that indicates whether the installed mouse has a vertical scroll wheel. </summary>
		/// <returns>
		///     <see langword="true" /> if the installed mouse has a vertical scroll wheel; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00025CAC File Offset: 0x00023EAC
		public static bool IsMouseWheelPresent
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[100])
					{
						SystemParameters._cacheValid[100] = true;
						SystemParameters._isMouseWheelPresent = (UnsafeNativeMethods.GetSystemMetrics(SM.MOUSEWHEELPRESENT) != 0);
					}
				}
				return SystemParameters._isMouseWheelPresent;
			}
		}

		/// <summary>Gets a value that indicates whether Microsoft Windows for Pen Computing extensions are installed. </summary>
		/// <returns>
		///     <see langword="true" /> if Pen Computing extensions are installed; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00025D18 File Offset: 0x00023F18
		public static bool IsPenWindows
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[101])
					{
						SystemParameters._cacheValid[101] = true;
						SystemParameters._isPenWindows = (UnsafeNativeMethods.GetSystemMetrics(SM.PENWINDOWS) != 0);
					}
				}
				return SystemParameters._isPenWindows;
			}
		}

		/// <summary>Gets a value that indicates whether the current session is remotely controlled. </summary>
		/// <returns>
		///     <see langword="true" /> if the current session is remotely controlled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00025D88 File Offset: 0x00023F88
		public static bool IsRemotelyControlled
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[102])
					{
						SystemParameters._cacheValid[102] = true;
						SystemParameters._isRemotelyControlled = (UnsafeNativeMethods.GetSystemMetrics(SM.REMOTECONTROL) != 0);
					}
				}
				return SystemParameters._isRemotelyControlled;
			}
		}

		/// <summary>Gets a value that indicates whether the calling process is associated with a Terminal Services client session. </summary>
		/// <returns>
		///     <see langword="true" /> if the calling process is associated with a Terminal Services client session; <see langword="false" /> if the calling process is associated with the Terminal Server console session.</returns>
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00025DFC File Offset: 0x00023FFC
		public static bool IsRemoteSession
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[103])
					{
						SystemParameters._cacheValid[103] = true;
						SystemParameters._isRemoteSession = (UnsafeNativeMethods.GetSystemMetrics(SM.REMOTESESSION) != 0);
					}
				}
				return SystemParameters._isRemoteSession;
			}
		}

		/// <summary>Gets a value that indicates whether the user requires information in visual format. </summary>
		/// <returns>
		///     <see langword="true" /> if the user requires an application to present information visually where it typically presents the information only in audible form; otherwise <see langword="false" />.</returns>
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00025E70 File Offset: 0x00024070
		public static bool ShowSounds
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[104])
					{
						SystemParameters._cacheValid[104] = true;
						SystemParameters._showSounds = (UnsafeNativeMethods.GetSystemMetrics(SM.SHOWSOUNDS) != 0);
					}
				}
				return SystemParameters._showSounds;
			}
		}

		/// <summary>Gets a value that indicates whether the computer has a low-end (slow) processor. </summary>
		/// <returns>
		///     <see langword="true" /> if the computer has a low-end (slow) processor; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00025EE0 File Offset: 0x000240E0
		public static bool IsSlowMachine
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[105])
					{
						SystemParameters._cacheValid[105] = true;
						SystemParameters._isSlowMachine = (UnsafeNativeMethods.GetSystemMetrics(SM.SLOWMACHINE) != 0);
					}
				}
				return SystemParameters._isSlowMachine;
			}
		}

		/// <summary>Gets a value that indicates whether the functionality of the left and right mouse buttons are swapped.  </summary>
		/// <returns>
		///     <see langword="true" /> if the functionality of the left and right mouse buttons are swapped; otherwise <see langword="false" />.</returns>
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00025F50 File Offset: 0x00024150
		public static bool SwapButtons
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[106])
					{
						SystemParameters._cacheValid[106] = true;
						SystemParameters._swapButtons = (UnsafeNativeMethods.GetSystemMetrics(SM.SWAPBUTTON) != 0);
					}
				}
				return SystemParameters._swapButtons;
			}
		}

		/// <summary>Gets a value that indicates whether the current operating system is Microsoft Windows XP Tablet PC Edition. </summary>
		/// <returns>
		///     <see langword="true" /> if the current operating system is Windows XP Tablet PC Edition; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00025FC0 File Offset: 0x000241C0
		public static bool IsTabletPC
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[107])
					{
						SystemParameters._cacheValid[107] = true;
						SystemParameters._isTabletPC = (UnsafeNativeMethods.GetSystemMetrics(SM.TABLETPC) != 0);
					}
				}
				return SystemParameters._isTabletPC;
			}
		}

		/// <summary>Gets a value that indicates the coordinate for the left side of the virtual screen.   </summary>
		/// <returns>A screen coordinate, in pixels.</returns>
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00026030 File Offset: 0x00024230
		public static double VirtualScreenLeft
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[108])
					{
						SystemParameters._cacheValid[108] = true;
						SystemParameters._virtualScreenLeft = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.XVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenLeft;
			}
		}

		/// <summary>Gets a value that indicates the upper coordinate of the virtual screen. </summary>
		/// <returns>A screen coordinate, in pixels.</returns>
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000260A4 File Offset: 0x000242A4
		public static double VirtualScreenTop
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[109])
					{
						SystemParameters._cacheValid[109] = true;
						SystemParameters._virtualScreenTop = SystemParameters.ConvertPixel(UnsafeNativeMethods.GetSystemMetrics(SM.YVIRTUALSCREEN));
					}
				}
				return SystemParameters._virtualScreenTop;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ThinHorizontalBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00026118 File Offset: 0x00024318
		public static ResourceKey ThinHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheThinHorizontalBorderHeight == null)
				{
					SystemParameters._cacheThinHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ThinHorizontalBorderHeight);
				}
				return SystemParameters._cacheThinHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ThinVerticalBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00026132 File Offset: 0x00024332
		public static ResourceKey ThinVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheThinVerticalBorderWidth == null)
				{
					SystemParameters._cacheThinVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ThinVerticalBorderWidth);
				}
				return SystemParameters._cacheThinVerticalBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CursorWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x0002614C File Offset: 0x0002434C
		public static ResourceKey CursorWidthKey
		{
			get
			{
				if (SystemParameters._cacheCursorWidth == null)
				{
					SystemParameters._cacheCursorWidth = SystemParameters.CreateInstance(SystemResourceKeyID.CursorWidth);
				}
				return SystemParameters._cacheCursorWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.CursorHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00026166 File Offset: 0x00024366
		public static ResourceKey CursorHeightKey
		{
			get
			{
				if (SystemParameters._cacheCursorHeight == null)
				{
					SystemParameters._cacheCursorHeight = SystemParameters.CreateInstance(SystemResourceKeyID.CursorHeight);
				}
				return SystemParameters._cacheCursorHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ThickHorizontalBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00026180 File Offset: 0x00024380
		public static ResourceKey ThickHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheThickHorizontalBorderHeight == null)
				{
					SystemParameters._cacheThickHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ThickHorizontalBorderHeight);
				}
				return SystemParameters._cacheThickHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ThickVerticalBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0002619A File Offset: 0x0002439A
		public static ResourceKey ThickVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheThickVerticalBorderWidth == null)
				{
					SystemParameters._cacheThickVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ThickVerticalBorderWidth);
				}
				return SystemParameters._cacheThickVerticalBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FixedFrameHorizontalBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x000261B4 File Offset: 0x000243B4
		public static ResourceKey FixedFrameHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFixedFrameHorizontalBorderHeight == null)
				{
					SystemParameters._cacheFixedFrameHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FixedFrameHorizontalBorderHeight);
				}
				return SystemParameters._cacheFixedFrameHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FixedFrameVerticalBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x000261CE File Offset: 0x000243CE
		public static ResourceKey FixedFrameVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFixedFrameVerticalBorderWidth == null)
				{
					SystemParameters._cacheFixedFrameVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FixedFrameVerticalBorderWidth);
				}
				return SystemParameters._cacheFixedFrameVerticalBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FocusHorizontalBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x000261E8 File Offset: 0x000243E8
		public static ResourceKey FocusHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheFocusHorizontalBorderHeight == null)
				{
					SystemParameters._cacheFocusHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FocusHorizontalBorderHeight);
				}
				return SystemParameters._cacheFocusHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FocusVerticalBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00026202 File Offset: 0x00024402
		public static ResourceKey FocusVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheFocusVerticalBorderWidth == null)
				{
					SystemParameters._cacheFocusVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FocusVerticalBorderWidth);
				}
				return SystemParameters._cacheFocusVerticalBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FullPrimaryScreenWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0002621C File Offset: 0x0002441C
		public static ResourceKey FullPrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheFullPrimaryScreenWidth == null)
				{
					SystemParameters._cacheFullPrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.FullPrimaryScreenWidth);
				}
				return SystemParameters._cacheFullPrimaryScreenWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.FullPrimaryScreenHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00026236 File Offset: 0x00024436
		public static ResourceKey FullPrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheFullPrimaryScreenHeight == null)
				{
					SystemParameters._cacheFullPrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.FullPrimaryScreenHeight);
				}
				return SystemParameters._cacheFullPrimaryScreenHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.HorizontalScrollBarButtonWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00026250 File Offset: 0x00024450
		public static ResourceKey HorizontalScrollBarButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarButtonWidth == null)
				{
					SystemParameters._cacheHorizontalScrollBarButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarButtonWidth);
				}
				return SystemParameters._cacheHorizontalScrollBarButtonWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.HorizontalScrollBarHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0002626A File Offset: 0x0002446A
		public static ResourceKey HorizontalScrollBarHeightKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarHeight == null)
				{
					SystemParameters._cacheHorizontalScrollBarHeight = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarHeight);
				}
				return SystemParameters._cacheHorizontalScrollBarHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.HorizontalScrollBarThumbWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00026284 File Offset: 0x00024484
		public static ResourceKey HorizontalScrollBarThumbWidthKey
		{
			get
			{
				if (SystemParameters._cacheHorizontalScrollBarThumbWidth == null)
				{
					SystemParameters._cacheHorizontalScrollBarThumbWidth = SystemParameters.CreateInstance(SystemResourceKeyID.HorizontalScrollBarThumbWidth);
				}
				return SystemParameters._cacheHorizontalScrollBarThumbWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0002629E File Offset: 0x0002449E
		public static ResourceKey IconWidthKey
		{
			get
			{
				if (SystemParameters._cacheIconWidth == null)
				{
					SystemParameters._cacheIconWidth = SystemParameters.CreateInstance(SystemResourceKeyID.IconWidth);
				}
				return SystemParameters._cacheIconWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x000262B8 File Offset: 0x000244B8
		public static ResourceKey IconHeightKey
		{
			get
			{
				if (SystemParameters._cacheIconHeight == null)
				{
					SystemParameters._cacheIconHeight = SystemParameters.CreateInstance(SystemResourceKeyID.IconHeight);
				}
				return SystemParameters._cacheIconHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconGridWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x000262D2 File Offset: 0x000244D2
		public static ResourceKey IconGridWidthKey
		{
			get
			{
				if (SystemParameters._cacheIconGridWidth == null)
				{
					SystemParameters._cacheIconGridWidth = SystemParameters.CreateInstance(SystemResourceKeyID.IconGridWidth);
				}
				return SystemParameters._cacheIconGridWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IconGridHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x000262EC File Offset: 0x000244EC
		public static ResourceKey IconGridHeightKey
		{
			get
			{
				if (SystemParameters._cacheIconGridHeight == null)
				{
					SystemParameters._cacheIconGridHeight = SystemParameters.CreateInstance(SystemResourceKeyID.IconGridHeight);
				}
				return SystemParameters._cacheIconGridHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MaximizedPrimaryScreenWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00026306 File Offset: 0x00024506
		public static ResourceKey MaximizedPrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheMaximizedPrimaryScreenWidth == null)
				{
					SystemParameters._cacheMaximizedPrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenWidth);
				}
				return SystemParameters._cacheMaximizedPrimaryScreenWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MaximizedPrimaryScreenHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00026320 File Offset: 0x00024520
		public static ResourceKey MaximizedPrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheMaximizedPrimaryScreenHeight == null)
				{
					SystemParameters._cacheMaximizedPrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenHeight);
				}
				return SystemParameters._cacheMaximizedPrimaryScreenHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MaximumWindowTrackWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0002633A File Offset: 0x0002453A
		public static ResourceKey MaximumWindowTrackWidthKey
		{
			get
			{
				if (SystemParameters._cacheMaximumWindowTrackWidth == null)
				{
					SystemParameters._cacheMaximumWindowTrackWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MaximumWindowTrackWidth);
				}
				return SystemParameters._cacheMaximumWindowTrackWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MaximumWindowTrackHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00026354 File Offset: 0x00024554
		public static ResourceKey MaximumWindowTrackHeightKey
		{
			get
			{
				if (SystemParameters._cacheMaximumWindowTrackHeight == null)
				{
					SystemParameters._cacheMaximumWindowTrackHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MaximumWindowTrackHeight);
				}
				return SystemParameters._cacheMaximumWindowTrackHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuCheckmarkWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0002636E File Offset: 0x0002456E
		public static ResourceKey MenuCheckmarkWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuCheckmarkWidth == null)
				{
					SystemParameters._cacheMenuCheckmarkWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuCheckmarkWidth);
				}
				return SystemParameters._cacheMenuCheckmarkWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuCheckmarkHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00026388 File Offset: 0x00024588
		public static ResourceKey MenuCheckmarkHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuCheckmarkHeight == null)
				{
					SystemParameters._cacheMenuCheckmarkHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuCheckmarkHeight);
				}
				return SystemParameters._cacheMenuCheckmarkHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuButtonWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x000263A2 File Offset: 0x000245A2
		public static ResourceKey MenuButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheMenuButtonWidth == null)
				{
					SystemParameters._cacheMenuButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MenuButtonWidth);
				}
				return SystemParameters._cacheMenuButtonWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuButtonHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x000263BC File Offset: 0x000245BC
		public static ResourceKey MenuButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuButtonHeight == null)
				{
					SystemParameters._cacheMenuButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuButtonHeight);
				}
				return SystemParameters._cacheMenuButtonHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimumWindowWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000263D6 File Offset: 0x000245D6
		public static ResourceKey MinimumWindowWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowWidth == null)
				{
					SystemParameters._cacheMinimumWindowWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowWidth);
				}
				return SystemParameters._cacheMinimumWindowWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimumWindowHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x000263F0 File Offset: 0x000245F0
		public static ResourceKey MinimumWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowHeight == null)
				{
					SystemParameters._cacheMinimumWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowHeight);
				}
				return SystemParameters._cacheMinimumWindowHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimizedWindowWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002640A File Offset: 0x0002460A
		public static ResourceKey MinimizedWindowWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedWindowWidth == null)
				{
					SystemParameters._cacheMinimizedWindowWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedWindowWidth);
				}
				return SystemParameters._cacheMinimizedWindowWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimizedWindowHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00026424 File Offset: 0x00024624
		public static ResourceKey MinimizedWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedWindowHeight == null)
				{
					SystemParameters._cacheMinimizedWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedWindowHeight);
				}
				return SystemParameters._cacheMinimizedWindowHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimizedGridWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002643E File Offset: 0x0002463E
		public static ResourceKey MinimizedGridWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedGridWidth == null)
				{
					SystemParameters._cacheMinimizedGridWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedGridWidth);
				}
				return SystemParameters._cacheMinimizedGridWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimizedGridHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00026458 File Offset: 0x00024658
		public static ResourceKey MinimizedGridHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimizedGridHeight == null)
				{
					SystemParameters._cacheMinimizedGridHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimizedGridHeight);
				}
				return SystemParameters._cacheMinimizedGridHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimumWindowTrackWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00026472 File Offset: 0x00024672
		public static ResourceKey MinimumWindowTrackWidthKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowTrackWidth == null)
				{
					SystemParameters._cacheMinimumWindowTrackWidth = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowTrackWidth);
				}
				return SystemParameters._cacheMinimumWindowTrackWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MinimumWindowTrackHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0002648F File Offset: 0x0002468F
		public static ResourceKey MinimumWindowTrackHeightKey
		{
			get
			{
				if (SystemParameters._cacheMinimumWindowTrackHeight == null)
				{
					SystemParameters._cacheMinimumWindowTrackHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MinimumWindowTrackHeight);
				}
				return SystemParameters._cacheMinimumWindowTrackHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.PrimaryScreenWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x000264AC File Offset: 0x000246AC
		public static ResourceKey PrimaryScreenWidthKey
		{
			get
			{
				if (SystemParameters._cachePrimaryScreenWidth == null)
				{
					SystemParameters._cachePrimaryScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.PrimaryScreenWidth);
				}
				return SystemParameters._cachePrimaryScreenWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.PrimaryScreenHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x000264C9 File Offset: 0x000246C9
		public static ResourceKey PrimaryScreenHeightKey
		{
			get
			{
				if (SystemParameters._cachePrimaryScreenHeight == null)
				{
					SystemParameters._cachePrimaryScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.PrimaryScreenHeight);
				}
				return SystemParameters._cachePrimaryScreenHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.WindowCaptionButtonWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x000264E6 File Offset: 0x000246E6
		public static ResourceKey WindowCaptionButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionButtonWidth == null)
				{
					SystemParameters._cacheWindowCaptionButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionButtonWidth);
				}
				return SystemParameters._cacheWindowCaptionButtonWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.WindowCaptionButtonHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00026503 File Offset: 0x00024703
		public static ResourceKey WindowCaptionButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionButtonHeight == null)
				{
					SystemParameters._cacheWindowCaptionButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionButtonHeight);
				}
				return SystemParameters._cacheWindowCaptionButtonHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ResizeFrameHorizontalBorderHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00026520 File Offset: 0x00024720
		public static ResourceKey ResizeFrameHorizontalBorderHeightKey
		{
			get
			{
				if (SystemParameters._cacheResizeFrameHorizontalBorderHeight == null)
				{
					SystemParameters._cacheResizeFrameHorizontalBorderHeight = SystemParameters.CreateInstance(SystemResourceKeyID.ResizeFrameHorizontalBorderHeight);
				}
				return SystemParameters._cacheResizeFrameHorizontalBorderHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ResizeFrameVerticalBorderWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0002653D File Offset: 0x0002473D
		public static ResourceKey ResizeFrameVerticalBorderWidthKey
		{
			get
			{
				if (SystemParameters._cacheResizeFrameVerticalBorderWidth == null)
				{
					SystemParameters._cacheResizeFrameVerticalBorderWidth = SystemParameters.CreateInstance(SystemResourceKeyID.ResizeFrameVerticalBorderWidth);
				}
				return SystemParameters._cacheResizeFrameVerticalBorderWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallIconWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0002655A File Offset: 0x0002475A
		public static ResourceKey SmallIconWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallIconWidth == null)
				{
					SystemParameters._cacheSmallIconWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallIconWidth);
				}
				return SystemParameters._cacheSmallIconWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallIconHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00026577 File Offset: 0x00024777
		public static ResourceKey SmallIconHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallIconHeight == null)
				{
					SystemParameters._cacheSmallIconHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallIconHeight);
				}
				return SystemParameters._cacheSmallIconHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallWindowCaptionButtonWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00026594 File Offset: 0x00024794
		public static ResourceKey SmallWindowCaptionButtonWidthKey
		{
			get
			{
				if (SystemParameters._cacheSmallWindowCaptionButtonWidth == null)
				{
					SystemParameters._cacheSmallWindowCaptionButtonWidth = SystemParameters.CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonWidth);
				}
				return SystemParameters._cacheSmallWindowCaptionButtonWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallWindowCaptionButtonHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000265B1 File Offset: 0x000247B1
		public static ResourceKey SmallWindowCaptionButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallWindowCaptionButtonHeight == null)
				{
					SystemParameters._cacheSmallWindowCaptionButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonHeight);
				}
				return SystemParameters._cacheSmallWindowCaptionButtonHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VirtualScreenWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x000265CE File Offset: 0x000247CE
		public static ResourceKey VirtualScreenWidthKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenWidth == null)
				{
					SystemParameters._cacheVirtualScreenWidth = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenWidth);
				}
				return SystemParameters._cacheVirtualScreenWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VirtualScreenHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x000265EB File Offset: 0x000247EB
		public static ResourceKey VirtualScreenHeightKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenHeight == null)
				{
					SystemParameters._cacheVirtualScreenHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenHeight);
				}
				return SystemParameters._cacheVirtualScreenHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VerticalScrollBarWidth" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00026608 File Offset: 0x00024808
		public static ResourceKey VerticalScrollBarWidthKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarWidth == null)
				{
					SystemParameters._cacheVerticalScrollBarWidth = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarWidth);
				}
				return SystemParameters._cacheVerticalScrollBarWidth;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VerticalScrollBarButtonHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00026625 File Offset: 0x00024825
		public static ResourceKey VerticalScrollBarButtonHeightKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarButtonHeight == null)
				{
					SystemParameters._cacheVerticalScrollBarButtonHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarButtonHeight);
				}
				return SystemParameters._cacheVerticalScrollBarButtonHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.WindowCaptionHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00026642 File Offset: 0x00024842
		public static ResourceKey WindowCaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheWindowCaptionHeight == null)
				{
					SystemParameters._cacheWindowCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.WindowCaptionHeight);
				}
				return SystemParameters._cacheWindowCaptionHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.KanjiWindowHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0002665F File Offset: 0x0002485F
		public static ResourceKey KanjiWindowHeightKey
		{
			get
			{
				if (SystemParameters._cacheKanjiWindowHeight == null)
				{
					SystemParameters._cacheKanjiWindowHeight = SystemParameters.CreateInstance(SystemResourceKeyID.KanjiWindowHeight);
				}
				return SystemParameters._cacheKanjiWindowHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.MenuBarHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0002667C File Offset: 0x0002487C
		public static ResourceKey MenuBarHeightKey
		{
			get
			{
				if (SystemParameters._cacheMenuBarHeight == null)
				{
					SystemParameters._cacheMenuBarHeight = SystemParameters.CreateInstance(SystemResourceKeyID.MenuBarHeight);
				}
				return SystemParameters._cacheMenuBarHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SmallCaptionHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00026699 File Offset: 0x00024899
		public static ResourceKey SmallCaptionHeightKey
		{
			get
			{
				if (SystemParameters._cacheSmallCaptionHeight == null)
				{
					SystemParameters._cacheSmallCaptionHeight = SystemParameters.CreateInstance(SystemResourceKeyID.SmallCaptionHeight);
				}
				return SystemParameters._cacheSmallCaptionHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VerticalScrollBarThumbHeight" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x000266B6 File Offset: 0x000248B6
		public static ResourceKey VerticalScrollBarThumbHeightKey
		{
			get
			{
				if (SystemParameters._cacheVerticalScrollBarThumbHeight == null)
				{
					SystemParameters._cacheVerticalScrollBarThumbHeight = SystemParameters.CreateInstance(SystemResourceKeyID.VerticalScrollBarThumbHeight);
				}
				return SystemParameters._cacheVerticalScrollBarThumbHeight;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsImmEnabled" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x000266D3 File Offset: 0x000248D3
		public static ResourceKey IsImmEnabledKey
		{
			get
			{
				if (SystemParameters._cacheIsImmEnabled == null)
				{
					SystemParameters._cacheIsImmEnabled = SystemParameters.CreateInstance(SystemResourceKeyID.IsImmEnabled);
				}
				return SystemParameters._cacheIsImmEnabled;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsMediaCenter" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x000266F0 File Offset: 0x000248F0
		public static ResourceKey IsMediaCenterKey
		{
			get
			{
				if (SystemParameters._cacheIsMediaCenter == null)
				{
					SystemParameters._cacheIsMediaCenter = SystemParameters.CreateInstance(SystemResourceKeyID.IsMediaCenter);
				}
				return SystemParameters._cacheIsMediaCenter;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsMenuDropRightAligned" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002670D File Offset: 0x0002490D
		public static ResourceKey IsMenuDropRightAlignedKey
		{
			get
			{
				if (SystemParameters._cacheIsMenuDropRightAligned == null)
				{
					SystemParameters._cacheIsMenuDropRightAligned = SystemParameters.CreateInstance(SystemResourceKeyID.IsMenuDropRightAligned);
				}
				return SystemParameters._cacheIsMenuDropRightAligned;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsMiddleEastEnabled" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0002672A File Offset: 0x0002492A
		public static ResourceKey IsMiddleEastEnabledKey
		{
			get
			{
				if (SystemParameters._cacheIsMiddleEastEnabled == null)
				{
					SystemParameters._cacheIsMiddleEastEnabled = SystemParameters.CreateInstance(SystemResourceKeyID.IsMiddleEastEnabled);
				}
				return SystemParameters._cacheIsMiddleEastEnabled;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsMousePresent" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00026747 File Offset: 0x00024947
		public static ResourceKey IsMousePresentKey
		{
			get
			{
				if (SystemParameters._cacheIsMousePresent == null)
				{
					SystemParameters._cacheIsMousePresent = SystemParameters.CreateInstance(SystemResourceKeyID.IsMousePresent);
				}
				return SystemParameters._cacheIsMousePresent;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsMouseWheelPresent" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00026764 File Offset: 0x00024964
		public static ResourceKey IsMouseWheelPresentKey
		{
			get
			{
				if (SystemParameters._cacheIsMouseWheelPresent == null)
				{
					SystemParameters._cacheIsMouseWheelPresent = SystemParameters.CreateInstance(SystemResourceKeyID.IsMouseWheelPresent);
				}
				return SystemParameters._cacheIsMouseWheelPresent;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsPenWindows" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00026781 File Offset: 0x00024981
		public static ResourceKey IsPenWindowsKey
		{
			get
			{
				if (SystemParameters._cacheIsPenWindows == null)
				{
					SystemParameters._cacheIsPenWindows = SystemParameters.CreateInstance(SystemResourceKeyID.IsPenWindows);
				}
				return SystemParameters._cacheIsPenWindows;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsRemotelyControlled" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0002679E File Offset: 0x0002499E
		public static ResourceKey IsRemotelyControlledKey
		{
			get
			{
				if (SystemParameters._cacheIsRemotelyControlled == null)
				{
					SystemParameters._cacheIsRemotelyControlled = SystemParameters.CreateInstance(SystemResourceKeyID.IsRemotelyControlled);
				}
				return SystemParameters._cacheIsRemotelyControlled;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsRemoteSession" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x000267BB File Offset: 0x000249BB
		public static ResourceKey IsRemoteSessionKey
		{
			get
			{
				if (SystemParameters._cacheIsRemoteSession == null)
				{
					SystemParameters._cacheIsRemoteSession = SystemParameters.CreateInstance(SystemResourceKeyID.IsRemoteSession);
				}
				return SystemParameters._cacheIsRemoteSession;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.ShowSounds" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x000267D8 File Offset: 0x000249D8
		public static ResourceKey ShowSoundsKey
		{
			get
			{
				if (SystemParameters._cacheShowSounds == null)
				{
					SystemParameters._cacheShowSounds = SystemParameters.CreateInstance(SystemResourceKeyID.ShowSounds);
				}
				return SystemParameters._cacheShowSounds;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsSlowMachine" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x000267F5 File Offset: 0x000249F5
		public static ResourceKey IsSlowMachineKey
		{
			get
			{
				if (SystemParameters._cacheIsSlowMachine == null)
				{
					SystemParameters._cacheIsSlowMachine = SystemParameters.CreateInstance(SystemResourceKeyID.IsSlowMachine);
				}
				return SystemParameters._cacheIsSlowMachine;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.SwapButtons" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00026812 File Offset: 0x00024A12
		public static ResourceKey SwapButtonsKey
		{
			get
			{
				if (SystemParameters._cacheSwapButtons == null)
				{
					SystemParameters._cacheSwapButtons = SystemParameters.CreateInstance(SystemResourceKeyID.SwapButtons);
				}
				return SystemParameters._cacheSwapButtons;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.IsTabletPC" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0002682F File Offset: 0x00024A2F
		public static ResourceKey IsTabletPCKey
		{
			get
			{
				if (SystemParameters._cacheIsTabletPC == null)
				{
					SystemParameters._cacheIsTabletPC = SystemParameters.CreateInstance(SystemResourceKeyID.IsTabletPC);
				}
				return SystemParameters._cacheIsTabletPC;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VirtualScreenLeft" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x0002684C File Offset: 0x00024A4C
		public static ResourceKey VirtualScreenLeftKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenLeft == null)
				{
					SystemParameters._cacheVirtualScreenLeft = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenLeft);
				}
				return SystemParameters._cacheVirtualScreenLeft;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.VirtualScreenTop" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00026869 File Offset: 0x00024A69
		public static ResourceKey VirtualScreenTopKey
		{
			get
			{
				if (SystemParameters._cacheVirtualScreenTop == null)
				{
					SystemParameters._cacheVirtualScreenTop = SystemParameters.CreateInstance(SystemResourceKeyID.VirtualScreenTop);
				}
				return SystemParameters._cacheVirtualScreenTop;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see langword="FocusVisualStyle" /> property. </summary>
		/// <returns>The resource key.</returns>
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00026886 File Offset: 0x00024A86
		public static ResourceKey FocusVisualStyleKey
		{
			get
			{
				if (SystemParameters._cacheFocusVisualStyle == null)
				{
					SystemParameters._cacheFocusVisualStyle = new SystemThemeKey(SystemResourceKeyID.FocusVisualStyle);
				}
				return SystemParameters._cacheFocusVisualStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.NavigationChromeStyleKey" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x000268A3 File Offset: 0x00024AA3
		public static ResourceKey NavigationChromeStyleKey
		{
			get
			{
				if (SystemParameters._cacheNavigationChromeStyle == null)
				{
					SystemParameters._cacheNavigationChromeStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeStyle);
				}
				return SystemParameters._cacheNavigationChromeStyle;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.NavigationChromeDownLevelStyleKey" /> property. </summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x000268C0 File Offset: 0x00024AC0
		public static ResourceKey NavigationChromeDownLevelStyleKey
		{
			get
			{
				if (SystemParameters._cacheNavigationChromeDownLevelStyle == null)
				{
					SystemParameters._cacheNavigationChromeDownLevelStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeDownLevelStyle);
				}
				return SystemParameters._cacheNavigationChromeDownLevelStyle;
			}
		}

		/// <summary>Gets a value indicating whether the system power is online, or that the system power status is unknown.</summary>
		/// <returns>A value in the enumeration.</returns>
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x000268E0 File Offset: 0x00024AE0
		public static PowerLineStatus PowerLineStatus
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[110])
					{
						SystemParameters._cacheValid[110] = true;
						NativeMethods.SYSTEM_POWER_STATUS system_POWER_STATUS = default(NativeMethods.SYSTEM_POWER_STATUS);
						if (!UnsafeNativeMethods.GetSystemPowerStatus(ref system_POWER_STATUS))
						{
							SystemParameters._cacheValid[110] = false;
							throw new Win32Exception();
						}
						SystemParameters._powerLineStatus = (PowerLineStatus)system_POWER_STATUS.ACLineStatus;
					}
				}
				return SystemParameters._powerLineStatus;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.ResourceKey" /> for the <see cref="P:System.Windows.SystemParameters.PowerLineStatus" /> property.</summary>
		/// <returns>A resource key.</returns>
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0002696C File Offset: 0x00024B6C
		public static ResourceKey PowerLineStatusKey
		{
			get
			{
				if (SystemParameters._cachePowerLineStatus == null)
				{
					SystemParameters._cachePowerLineStatus = SystemParameters.CreateInstance(SystemResourceKeyID.PowerLineStatus);
				}
				return SystemParameters._cachePowerLineStatus;
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002698C File Offset: 0x00024B8C
		internal static void InvalidateCache()
		{
			int[] array = new int[]
			{
				8207,
				8209,
				67,
				4129,
				4133,
				4131,
				47,
				46,
				4107,
				23,
				69,
				11,
				96,
				105,
				103,
				101,
				99,
				28,
				4115,
				107,
				4101,
				4163,
				4123,
				4105,
				4111,
				4103,
				4099,
				4117,
				4113,
				4119,
				4121,
				4159,
				73,
				8199,
				8197,
				37,
				6,
				42,
				76,
				77,
				49,
				57,
				33
			};
			for (int i = 0; i < array.Length; i++)
			{
				SystemParameters.InvalidateCache(array[i]);
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x000269C4 File Offset: 0x00024BC4
		internal static bool InvalidateDeviceDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 99))
			{
				flag |= (SystemParameters._isMousePresent != SystemParameters.IsMousePresent);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 100))
			{
				flag |= (SystemParameters._isMouseWheelPresent != SystemParameters.IsMouseWheelPresent);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"IsMousePresent",
					"IsMouseWheelPresent"
				});
			}
			return flag;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00026A34 File Offset: 0x00024C34
		internal static bool InvalidateDisplayDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 7))
			{
				NativeMethods.RECT workAreaInternal = SystemParameters._workAreaInternal;
				NativeMethods.RECT workAreaInternal2 = SystemParameters.WorkAreaInternal;
				flag |= (workAreaInternal.left != workAreaInternal2.left);
				flag |= (workAreaInternal.top != workAreaInternal2.top);
				flag |= (workAreaInternal.right != workAreaInternal2.right);
				flag |= (workAreaInternal.bottom != workAreaInternal2.bottom);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 8))
			{
				flag |= (SystemParameters._workArea != SystemParameters.WorkArea);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 52))
			{
				flag |= (SystemParameters._fullPrimaryScreenWidth != SystemParameters.FullPrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 53))
			{
				flag |= (SystemParameters._fullPrimaryScreenHeight != SystemParameters.FullPrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 61))
			{
				flag |= (SystemParameters._maximizedPrimaryScreenWidth != SystemParameters.MaximizedPrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 62))
			{
				flag |= (SystemParameters._maximizedPrimaryScreenHeight != SystemParameters.MaximizedPrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 77))
			{
				flag |= (SystemParameters._primaryScreenWidth != SystemParameters.PrimaryScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 78))
			{
				flag |= (SystemParameters._primaryScreenHeight != SystemParameters.PrimaryScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 87))
			{
				flag |= (SystemParameters._virtualScreenWidth != SystemParameters.VirtualScreenWidth);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 88))
			{
				flag |= (SystemParameters._virtualScreenHeight != SystemParameters.VirtualScreenHeight);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 108))
			{
				flag |= (SystemParameters._virtualScreenLeft != SystemParameters.VirtualScreenLeft);
			}
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 109))
			{
				flag |= (SystemParameters._virtualScreenTop != SystemParameters.VirtualScreenTop);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"WorkArea",
					"FullPrimaryScreenWidth",
					"FullPrimaryScreenHeight",
					"MaximizedPrimaryScreenWidth",
					"MaximizedPrimaryScreenHeight",
					"PrimaryScreenWidth",
					"PrimaryScreenHeight",
					"VirtualScreenWidth",
					"VirtualScreenHeight",
					"VirtualScreenLeft",
					"VirtualScreenTop"
				});
			}
			return flag;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00026C78 File Offset: 0x00024E78
		internal static bool InvalidatePowerDependentCache()
		{
			bool flag = false;
			if (SystemResources.ClearSlot(SystemParameters._cacheValid, 110))
			{
				flag |= (SystemParameters._powerLineStatus != SystemParameters.PowerLineStatus);
			}
			if (flag)
			{
				SystemParameters.OnPropertiesChanged(new string[]
				{
					"PowerLineStatus"
				});
			}
			return flag;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00026CC0 File Offset: 0x00024EC0
		internal static bool InvalidateCache(int param)
		{
			if (param <= 69)
			{
				if (param <= 33)
				{
					if (param <= 11)
					{
						if (param != 6)
						{
							if (param != 11)
							{
								return false;
							}
							return SystemParameters.InvalidateProperty(13, "KeyboardSpeed");
						}
					}
					else
					{
						if (param == 23)
						{
							return SystemParameters.InvalidateProperty(11, "KeyboardDelay");
						}
						if (param == 28)
						{
							bool flag = SystemResources.ClearSlot(SystemParameters._cacheValid, 19);
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 97))
							{
								flag |= (SystemParameters._isMenuDropRightAligned != SystemParameters.IsMenuDropRightAligned);
							}
							if (flag)
							{
								SystemParameters.OnPropertiesChanged(new string[]
								{
									"MenuDropAlignment",
									"IsMenuDropRightAligned"
								});
							}
							return flag;
						}
						if (param != 33)
						{
							return false;
						}
						return SystemParameters.InvalidateProperty(106, "SwapButtons");
					}
				}
				else if (param <= 49)
				{
					if (param == 37)
					{
						return SystemParameters.InvalidateProperty(38, "DragFullWindows");
					}
					if (param != 42)
					{
						switch (param)
						{
						case 46:
						{
							bool flag2 = SystemResources.ClearSlot(SystemParameters._cacheValid, 9);
							if (flag2)
							{
								SystemFonts.InvalidateIconMetrics();
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 57))
							{
								flag2 |= (SystemParameters._iconWidth != SystemParameters.IconWidth);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 58))
							{
								flag2 |= (SystemParameters._iconHeight != SystemParameters.IconHeight);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 59))
							{
								flag2 |= (SystemParameters._iconGridWidth != SystemParameters.IconGridWidth);
							}
							if (SystemResources.ClearSlot(SystemParameters._cacheValid, 60))
							{
								flag2 |= (SystemParameters._iconGridHeight != SystemParameters.IconGridHeight);
							}
							if (flag2)
							{
								SystemParameters.OnPropertiesChanged(new string[]
								{
									"IconMetrics",
									"IconWidth",
									"IconHeight",
									"IconGridWidth",
									"IconGridHeight"
								});
							}
							return flag2;
						}
						case 47:
							return SystemParameters.InvalidateDisplayDependentCache();
						case 48:
							return false;
						case 49:
							return SystemParameters.InvalidateProperty(101, "IsPenWindows");
						default:
							return false;
						}
					}
				}
				else
				{
					if (param == 57)
					{
						return SystemParameters.InvalidateProperty(104, "ShowSounds");
					}
					if (param == 67)
					{
						return SystemParameters.InvalidateProperty(3, "HighContrast");
					}
					if (param != 69)
					{
						return false;
					}
					return SystemParameters.InvalidateProperty(12, "KeyboardPreference");
				}
				bool flag3 = SystemResources.ClearSlot(SystemParameters._cacheValid, 39);
				if (flag3)
				{
					SystemFonts.InvalidateNonClientMetrics();
				}
				flag3 |= SystemResources.ClearSlot(SystemParameters._cacheValid, 35);
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 40))
				{
					flag3 |= (SystemParameters._thinHorizontalBorderHeight != SystemParameters.ThinHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 41))
				{
					flag3 |= (SystemParameters._thinVerticalBorderWidth != SystemParameters.ThinVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 42))
				{
					flag3 |= (SystemParameters._cursorWidth != SystemParameters.CursorWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 43))
				{
					flag3 |= (SystemParameters._cursorHeight != SystemParameters.CursorHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 44))
				{
					flag3 |= (SystemParameters._thickHorizontalBorderHeight != SystemParameters.ThickHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 45))
				{
					flag3 |= (SystemParameters._thickVerticalBorderWidth != SystemParameters.ThickVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 48))
				{
					flag3 |= (SystemParameters._fixedFrameHorizontalBorderHeight != SystemParameters.FixedFrameHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 49))
				{
					flag3 |= (SystemParameters._fixedFrameVerticalBorderWidth != SystemParameters.FixedFrameVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 54))
				{
					flag3 |= (SystemParameters._horizontalScrollBarButtonWidth != SystemParameters.HorizontalScrollBarButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 55))
				{
					flag3 |= (SystemParameters._horizontalScrollBarHeight != SystemParameters.HorizontalScrollBarHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 56))
				{
					flag3 |= (SystemParameters._horizontalScrollBarThumbWidth != SystemParameters.HorizontalScrollBarThumbWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 57))
				{
					flag3 |= (SystemParameters._iconWidth != SystemParameters.IconWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 58))
				{
					flag3 |= (SystemParameters._iconHeight != SystemParameters.IconHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 59))
				{
					flag3 |= (SystemParameters._iconGridWidth != SystemParameters.IconGridWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 60))
				{
					flag3 |= (SystemParameters._iconGridHeight != SystemParameters.IconGridHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 63))
				{
					flag3 |= (SystemParameters._maximumWindowTrackWidth != SystemParameters.MaximumWindowTrackWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 64))
				{
					flag3 |= (SystemParameters._maximumWindowTrackHeight != SystemParameters.MaximumWindowTrackHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 65))
				{
					flag3 |= (SystemParameters._menuCheckmarkWidth != SystemParameters.MenuCheckmarkWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 66))
				{
					flag3 |= (SystemParameters._menuCheckmarkHeight != SystemParameters.MenuCheckmarkHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 67))
				{
					flag3 |= (SystemParameters._menuButtonWidth != SystemParameters.MenuButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 68))
				{
					flag3 |= (SystemParameters._menuButtonHeight != SystemParameters.MenuButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 69))
				{
					flag3 |= (SystemParameters._minimumWindowWidth != SystemParameters.MinimumWindowWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 70))
				{
					flag3 |= (SystemParameters._minimumWindowHeight != SystemParameters.MinimumWindowHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 71))
				{
					flag3 |= (SystemParameters._minimizedWindowWidth != SystemParameters.MinimizedWindowWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 72))
				{
					flag3 |= (SystemParameters._minimizedWindowHeight != SystemParameters.MinimizedWindowHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 73))
				{
					flag3 |= (SystemParameters._minimizedGridWidth != SystemParameters.MinimizedGridWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 74))
				{
					flag3 |= (SystemParameters._minimizedGridHeight != SystemParameters.MinimizedGridHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 75))
				{
					flag3 |= (SystemParameters._minimumWindowTrackWidth != SystemParameters.MinimumWindowTrackWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 76))
				{
					flag3 |= (SystemParameters._minimumWindowTrackHeight != SystemParameters.MinimumWindowTrackHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 79))
				{
					flag3 |= (SystemParameters._windowCaptionButtonWidth != SystemParameters.WindowCaptionButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 80))
				{
					flag3 |= (SystemParameters._windowCaptionButtonHeight != SystemParameters.WindowCaptionButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 81))
				{
					flag3 |= (SystemParameters._resizeFrameHorizontalBorderHeight != SystemParameters.ResizeFrameHorizontalBorderHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 82))
				{
					flag3 |= (SystemParameters._resizeFrameVerticalBorderWidth != SystemParameters.ResizeFrameVerticalBorderWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 83))
				{
					flag3 |= (SystemParameters._smallIconWidth != SystemParameters.SmallIconWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 84))
				{
					flag3 |= (SystemParameters._smallIconHeight != SystemParameters.SmallIconHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 85))
				{
					flag3 |= (SystemParameters._smallWindowCaptionButtonWidth != SystemParameters.SmallWindowCaptionButtonWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 86))
				{
					flag3 |= (SystemParameters._smallWindowCaptionButtonHeight != SystemParameters.SmallWindowCaptionButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 89))
				{
					flag3 |= (SystemParameters._verticalScrollBarWidth != SystemParameters.VerticalScrollBarWidth);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 90))
				{
					flag3 |= (SystemParameters._verticalScrollBarButtonHeight != SystemParameters.VerticalScrollBarButtonHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 91))
				{
					flag3 |= (SystemParameters._windowCaptionHeight != SystemParameters.WindowCaptionHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 93))
				{
					flag3 |= (SystemParameters._menuBarHeight != SystemParameters.MenuBarHeight);
				}
				if (SystemResources.ClearSlot(SystemParameters._cacheValid, 94))
				{
					flag3 |= (SystemParameters._verticalScrollBarThumbHeight != SystemParameters.VerticalScrollBarThumbHeight);
				}
				if (flag3)
				{
					SystemParameters.OnPropertiesChanged(new string[]
					{
						"NonClientMetrics",
						"Border",
						"ThinHorizontalBorderHeight",
						"ThinVerticalBorderWidth",
						"CursorWidth",
						"CursorHeight",
						"ThickHorizontalBorderHeight",
						"ThickVerticalBorderWidth",
						"FixedFrameHorizontalBorderHeight",
						"FixedFrameVerticalBorderWidth",
						"HorizontalScrollBarButtonWidth",
						"HorizontalScrollBarHeight",
						"HorizontalScrollBarThumbWidth",
						"IconWidth",
						"IconHeight",
						"IconGridWidth",
						"IconGridHeight",
						"MaximumWindowTrackWidth",
						"MaximumWindowTrackHeight",
						"MenuCheckmarkWidth",
						"MenuCheckmarkHeight",
						"MenuButtonWidth",
						"MenuButtonHeight",
						"MinimumWindowWidth",
						"MinimumWindowHeight",
						"MinimizedWindowWidth",
						"MinimizedWindowHeight",
						"MinimizedGridWidth",
						"MinimizedGridHeight",
						"MinimumWindowTrackWidth",
						"MinimumWindowTrackHeight",
						"WindowCaptionButtonWidth",
						"WindowCaptionButtonHeight",
						"ResizeFrameHorizontalBorderHeight",
						"ResizeFrameVerticalBorderWidth",
						"SmallIconWidth",
						"SmallIconHeight",
						"SmallWindowCaptionButtonWidth",
						"SmallWindowCaptionButtonHeight",
						"VerticalScrollBarWidth",
						"VerticalScrollBarButtonHeight",
						"MenuBarHeight",
						"VerticalScrollBarThumbHeight"
					});
				}
				return flag3 | SystemParameters.InvalidateDisplayDependentCache();
			}
			if (param <= 4133)
			{
				if (param <= 107)
				{
					switch (param)
					{
					case 73:
						return SystemParameters.InvalidateProperty(34, "MinimizeAnimation");
					case 74:
					case 75:
						break;
					case 76:
						return SystemParameters.InvalidateProperty(46, "MinimumHorizontalDragDistance");
					case 77:
						return SystemParameters.InvalidateProperty(47, "MinimumVerticalDragDistance");
					default:
						if (param == 96)
						{
							return SystemParameters.InvalidateProperty(14, "SnapToDefaultButton");
						}
						switch (param)
						{
						case 99:
							return SystemParameters.InvalidateProperty(18, "MouseHoverWidth");
						case 101:
							return SystemParameters.InvalidateProperty(17, "MouseHoverHeight");
						case 103:
							return SystemParameters.InvalidateProperty(16, "MouseHoverTime");
						case 105:
							return SystemParameters.InvalidateProperty(15, "WheelScrollLines");
						case 107:
							return SystemParameters.InvalidateProperty(21, "MenuShowDelay");
						}
						break;
					}
				}
				else
				{
					switch (param)
					{
					case 4099:
						return SystemParameters.InvalidateProperty(28, "MenuAnimation");
					case 4100:
					case 4102:
					case 4104:
					case 4106:
						break;
					case 4101:
						return SystemParameters.InvalidateProperty(22, "ComboBoxAnimation");
					case 4103:
						return SystemParameters.InvalidateProperty(27, "ListBoxSmoothScrolling");
					case 4105:
						return SystemParameters.InvalidateProperty(25, "GradientCaptions");
					case 4107:
						return SystemParameters.InvalidateProperty(10, "KeyboardCues");
					default:
						switch (param)
						{
						case 4111:
							return SystemParameters.InvalidateProperty(26, "HotTracking");
						case 4112:
						case 4114:
						case 4116:
						case 4118:
						case 4120:
						case 4122:
							break;
						case 4113:
							return SystemParameters.InvalidateProperty(30, "StylusHotTracking");
						case 4115:
							return SystemParameters.InvalidateProperty(20, "MenuFade");
						case 4117:
							return SystemParameters.InvalidateProperty(29, "SelectionFade");
						case 4119:
							return SystemParameters.InvalidateProperty(31, "ToolTipAnimation");
						case 4121:
							return SystemParameters.InvalidateProperty(32, "ToolTipFade");
						case 4123:
							return SystemParameters.InvalidateProperty(24, "CursorShadow");
						default:
							switch (param)
							{
							case 4129:
								return SystemParameters.InvalidateProperty(4, "MouseVanish");
							case 4131:
								return SystemParameters.InvalidateProperty(6, "FlatMenu");
							case 4133:
								return SystemParameters.InvalidateProperty(5, "DropShadow");
							}
							break;
						}
						break;
					}
				}
			}
			else if (param <= 8197)
			{
				if (param == 4159)
				{
					return SystemParameters.InvalidateProperty(33, "UIEffects");
				}
				if (param == 4163)
				{
					return SystemParameters.InvalidateProperty(23, "ClientAreaAnimation");
				}
				if (param == 8197)
				{
					return SystemParameters.InvalidateProperty(37, "ForegroundFlashCount");
				}
			}
			else
			{
				if (param == 8199)
				{
					return SystemParameters.InvalidateProperty(36, "CaretWidth");
				}
				if (param == 8207)
				{
					bool flag4 = SystemResources.ClearSlot(SystemParameters._cacheValid, 1);
					if (SystemResources.ClearSlot(SystemParameters._cacheValid, 50))
					{
						flag4 |= (SystemParameters._focusHorizontalBorderHeight != SystemParameters.FocusHorizontalBorderHeight);
					}
					if (SystemResources.ClearSlot(SystemParameters._cacheValid, 51))
					{
						flag4 |= (SystemParameters._focusVerticalBorderWidth != SystemParameters.FocusVerticalBorderWidth);
					}
					if (flag4)
					{
						SystemParameters.OnPropertiesChanged(new string[]
						{
							"FocusBorderWidth",
							"FocusHorizontalBorderHeight",
							"FocusVerticalBorderWidth"
						});
					}
					return flag4;
				}
				if (param == 8209)
				{
					return SystemParameters.InvalidateProperty(2, "FocusBorderHeight");
				}
			}
			return false;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00027915 File Offset: 0x00025B15
		internal static bool InvalidateIsGlassEnabled()
		{
			return SystemParameters.InvalidateProperty(111, "IsGlassEnabled");
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00027923 File Offset: 0x00025B23
		internal static void InvalidateDerivedThemeRelatedProperties()
		{
			SystemParameters.InvalidateProperty(112, "UxThemeName");
			SystemParameters.InvalidateProperty(113, "UxThemeColor");
			SystemParameters.InvalidateProperty(114, "WindowCornerRadius");
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002794C File Offset: 0x00025B4C
		internal static void InvalidateWindowGlassColorizationProperties()
		{
			SystemParameters.InvalidateProperty(115, "WindowGlassColor");
			SystemParameters.InvalidateProperty(116, "WindowGlassBrush");
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00027968 File Offset: 0x00025B68
		internal static void InvalidateWindowFrameThicknessProperties()
		{
			SystemParameters.InvalidateProperty(117, "WindowNonClientFrameThickness");
			SystemParameters.InvalidateProperty(118, "WindowResizeBorderThickness");
		}

		/// <summary>Gets a value that indicates whether glass window frames are being used.</summary>
		/// <returns>
		///     <see langword="true" /> if glass window frames are being used; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00027984 File Offset: 0x00025B84
		public static bool IsGlassEnabled
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[111])
					{
						SystemParameters._cacheValid[111] = true;
						SystemParameters._isGlassEnabled = NativeMethods.DwmIsCompositionEnabled();
					}
				}
				return SystemParameters._isGlassEnabled;
			}
		}

		/// <summary>Gets the theme name.</summary>
		/// <returns>The theme name.</returns>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000279EC File Offset: 0x00025BEC
		public static string UxThemeName
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[112])
					{
						SystemParameters._cacheValid[112] = true;
						if (!NativeMethods.IsThemeActive())
						{
							SystemParameters._uxThemeName = "Classic";
						}
						else
						{
							string path;
							string text;
							string text2;
							NativeMethods.GetCurrentThemeName(out path, out text, out text2);
							SystemParameters._uxThemeName = Path.GetFileNameWithoutExtension(path);
						}
					}
				}
				return SystemParameters._uxThemeName;
			}
		}

		/// <summary>Gets the color theme name.</summary>
		/// <returns>The color theme name.</returns>
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00027A70 File Offset: 0x00025C70
		public static string UxThemeColor
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[113])
					{
						SystemParameters._cacheValid[113] = true;
						if (!NativeMethods.IsThemeActive())
						{
							SystemParameters._uxThemeColor = "";
						}
						else
						{
							string text;
							string uxThemeColor;
							string text2;
							NativeMethods.GetCurrentThemeName(out text, out uxThemeColor, out text2);
							SystemParameters._uxThemeColor = uxThemeColor;
						}
					}
				}
				return SystemParameters._uxThemeColor;
			}
		}

		/// <summary>Gets the radius of the corners for a window.</summary>
		/// <returns>The degree to which the corners of a window are rounded.</returns>
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00027AF0 File Offset: 0x00025CF0
		public static CornerRadius WindowCornerRadius
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[114])
					{
						SystemParameters._cacheValid[114] = true;
						CornerRadius windowCornerRadius = default(CornerRadius);
						string a = SystemParameters.UxThemeName.ToUpperInvariant();
						if (!(a == "LUNA"))
						{
							if (!(a == "AERO"))
							{
								if (!(a == "CLASSIC") && !(a == "ZUNE") && !(a == "ROYALE"))
								{
								}
								windowCornerRadius = new CornerRadius(0.0);
							}
							else if (NativeMethods.DwmIsCompositionEnabled())
							{
								windowCornerRadius = new CornerRadius(8.0);
							}
							else
							{
								windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
							}
						}
						else
						{
							windowCornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
						}
						SystemParameters._windowCornerRadius = windowCornerRadius;
					}
				}
				return SystemParameters._windowCornerRadius;
			}
		}

		/// <summary>Gets the color that is used to paint the glass window frame.</summary>
		/// <returns>The color that is used to paint the glass window frame.</returns>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00027C44 File Offset: 0x00025E44
		public static Color WindowGlassColor
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[115])
					{
						SystemParameters._cacheValid[115] = true;
						uint num;
						bool flag2;
						NativeMethods.DwmGetColorizationColor(out num, out flag2);
						num |= (flag2 ? 4278190080U : 0U);
						SystemParameters._windowGlassColor = Utility.ColorFromArgbDword(num);
					}
				}
				return SystemParameters._windowGlassColor;
			}
		}

		/// <summary>Gets the brush that paints the glass window frame.</summary>
		/// <returns>The brush that paints the glass window frame.</returns>
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00027CC4 File Offset: 0x00025EC4
		public static Brush WindowGlassBrush
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[116])
					{
						SystemParameters._cacheValid[116] = true;
						SolidColorBrush solidColorBrush = new SolidColorBrush(SystemParameters.WindowGlassColor);
						solidColorBrush.Freeze();
						SystemParameters._windowGlassBrush = solidColorBrush;
					}
				}
				return SystemParameters._windowGlassBrush;
			}
		}

		/// <summary>Gets the size of the resizing border around the window.</summary>
		/// <returns>The size of the resizing border around the window, in device-independent units (1/96th of an inch).</returns>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00027D38 File Offset: 0x00025F38
		public static Thickness WindowResizeBorderThickness
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[118])
					{
						SystemParameters._cacheValid[118] = true;
						Size deviceSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME));
						Size size = DpiHelper.DeviceSizeToLogical(deviceSize, (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0);
						SystemParameters._windowResizeBorderThickness = new Thickness(size.Width, size.Height, size.Width, size.Height);
					}
				}
				return SystemParameters._windowResizeBorderThickness;
			}
		}

		/// <summary>Gets the size of the non-client area of the window.</summary>
		/// <returns>The size of the non-client area of the window, in device-independent units (1/96th of an inch).</returns>
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00027DF8 File Offset: 0x00025FF8
		public static Thickness WindowNonClientFrameThickness
		{
			[SecurityCritical]
			get
			{
				BitArray cacheValid = SystemParameters._cacheValid;
				lock (cacheValid)
				{
					while (!SystemParameters._cacheValid[117])
					{
						SystemParameters._cacheValid[117] = true;
						Size deviceSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME));
						Size size = DpiHelper.DeviceSizeToLogical(deviceSize, (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0);
						int systemMetrics = NativeMethods.GetSystemMetrics(SM.CYCAPTION);
						double y = DpiHelper.DevicePixelsToLogical(new Point(0.0, (double)systemMetrics), (double)SystemParameters.DpiX / 96.0, (double)SystemParameters.Dpi / 96.0).Y;
						SystemParameters._windowNonClientFrameThickness = new Thickness(size.Width, size.Height + y, size.Width, size.Height);
					}
				}
				return SystemParameters._windowNonClientFrameThickness;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00027F0C File Offset: 0x0002610C
		internal static int Dpi
		{
			get
			{
				return Util.Dpi;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00027F14 File Offset: 0x00026114
		internal static int DpiX
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (SystemParameters._setDpiX)
				{
					BitArray cacheValid = SystemParameters._cacheValid;
					lock (cacheValid)
					{
						if (SystemParameters._setDpiX)
						{
							SystemParameters._setDpiX = false;
							HandleRef hWnd = new HandleRef(null, IntPtr.Zero);
							IntPtr dc = UnsafeNativeMethods.GetDC(hWnd);
							if (dc == IntPtr.Zero)
							{
								throw new Win32Exception();
							}
							try
							{
								SystemParameters._dpiX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
								SystemParameters._cacheValid[0] = true;
							}
							finally
							{
								UnsafeNativeMethods.ReleaseDC(hWnd, new HandleRef(null, dc));
							}
						}
					}
				}
				return SystemParameters._dpiX;
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00027FD0 File Offset: 0x000261D0
		internal static double ConvertPixel(int pixel)
		{
			int dpi = SystemParameters.Dpi;
			if (dpi != 0)
			{
				return (double)pixel * 96.0 / (double)dpi;
			}
			return (double)pixel;
		}

		// Token: 0x0400089D RID: 2205
		private static BitArray _cacheValid = new BitArray(119);

		// Token: 0x0400089E RID: 2206
		private static bool _isGlassEnabled;

		// Token: 0x0400089F RID: 2207
		private static string _uxThemeName;

		// Token: 0x040008A0 RID: 2208
		private static string _uxThemeColor;

		// Token: 0x040008A1 RID: 2209
		private static CornerRadius _windowCornerRadius;

		// Token: 0x040008A2 RID: 2210
		private static Color _windowGlassColor;

		// Token: 0x040008A3 RID: 2211
		private static Brush _windowGlassBrush;

		// Token: 0x040008A4 RID: 2212
		private static Thickness _windowNonClientFrameThickness;

		// Token: 0x040008A5 RID: 2213
		private static Thickness _windowResizeBorderThickness;

		// Token: 0x040008A6 RID: 2214
		private static int _dpiX;

		// Token: 0x040008A7 RID: 2215
		private static bool _setDpiX = true;

		// Token: 0x040008A8 RID: 2216
		private static double _focusBorderWidth;

		// Token: 0x040008A9 RID: 2217
		private static double _focusBorderHeight;

		// Token: 0x040008AA RID: 2218
		private static bool _highContrast;

		// Token: 0x040008AB RID: 2219
		private static bool _mouseVanish;

		// Token: 0x040008AC RID: 2220
		private static bool _dropShadow;

		// Token: 0x040008AD RID: 2221
		private static bool _flatMenu;

		// Token: 0x040008AE RID: 2222
		private static NativeMethods.RECT _workAreaInternal;

		// Token: 0x040008AF RID: 2223
		private static Rect _workArea;

		// Token: 0x040008B0 RID: 2224
		private static NativeMethods.ICONMETRICS _iconMetrics;

		// Token: 0x040008B1 RID: 2225
		private static bool _keyboardCues;

		// Token: 0x040008B2 RID: 2226
		private static int _keyboardDelay;

		// Token: 0x040008B3 RID: 2227
		private static bool _keyboardPref;

		// Token: 0x040008B4 RID: 2228
		private static int _keyboardSpeed;

		// Token: 0x040008B5 RID: 2229
		private static bool _snapToDefButton;

		// Token: 0x040008B6 RID: 2230
		private static int _wheelScrollLines;

		// Token: 0x040008B7 RID: 2231
		private static int _mouseHoverTime;

		// Token: 0x040008B8 RID: 2232
		private static double _mouseHoverHeight;

		// Token: 0x040008B9 RID: 2233
		private static double _mouseHoverWidth;

		// Token: 0x040008BA RID: 2234
		private static bool _menuDropAlignment;

		// Token: 0x040008BB RID: 2235
		private static bool _menuFade;

		// Token: 0x040008BC RID: 2236
		private static int _menuShowDelay;

		// Token: 0x040008BD RID: 2237
		private static bool _comboBoxAnimation;

		// Token: 0x040008BE RID: 2238
		private static bool _clientAreaAnimation;

		// Token: 0x040008BF RID: 2239
		private static bool _cursorShadow;

		// Token: 0x040008C0 RID: 2240
		private static bool _gradientCaptions;

		// Token: 0x040008C1 RID: 2241
		private static bool _hotTracking;

		// Token: 0x040008C2 RID: 2242
		private static bool _listBoxSmoothScrolling;

		// Token: 0x040008C3 RID: 2243
		private static bool _menuAnimation;

		// Token: 0x040008C4 RID: 2244
		private static bool _selectionFade;

		// Token: 0x040008C5 RID: 2245
		private static bool _stylusHotTracking;

		// Token: 0x040008C6 RID: 2246
		private static bool _toolTipAnimation;

		// Token: 0x040008C7 RID: 2247
		private static bool _tooltipFade;

		// Token: 0x040008C8 RID: 2248
		private static bool _uiEffects;

		// Token: 0x040008C9 RID: 2249
		private static bool _minAnimation;

		// Token: 0x040008CA RID: 2250
		private static int _border;

		// Token: 0x040008CB RID: 2251
		private static double _caretWidth;

		// Token: 0x040008CC RID: 2252
		private static bool _dragFullWindows;

		// Token: 0x040008CD RID: 2253
		private static int _foregroundFlashCount;

		// Token: 0x040008CE RID: 2254
		private static NativeMethods.NONCLIENTMETRICS _ncm;

		// Token: 0x040008CF RID: 2255
		private static double _thinHorizontalBorderHeight;

		// Token: 0x040008D0 RID: 2256
		private static double _thinVerticalBorderWidth;

		// Token: 0x040008D1 RID: 2257
		private static double _cursorWidth;

		// Token: 0x040008D2 RID: 2258
		private static double _cursorHeight;

		// Token: 0x040008D3 RID: 2259
		private static double _thickHorizontalBorderHeight;

		// Token: 0x040008D4 RID: 2260
		private static double _thickVerticalBorderWidth;

		// Token: 0x040008D5 RID: 2261
		private static double _minimumHorizontalDragDistance;

		// Token: 0x040008D6 RID: 2262
		private static double _minimumVerticalDragDistance;

		// Token: 0x040008D7 RID: 2263
		private static double _fixedFrameHorizontalBorderHeight;

		// Token: 0x040008D8 RID: 2264
		private static double _fixedFrameVerticalBorderWidth;

		// Token: 0x040008D9 RID: 2265
		private static double _focusHorizontalBorderHeight;

		// Token: 0x040008DA RID: 2266
		private static double _focusVerticalBorderWidth;

		// Token: 0x040008DB RID: 2267
		private static double _fullPrimaryScreenHeight;

		// Token: 0x040008DC RID: 2268
		private static double _fullPrimaryScreenWidth;

		// Token: 0x040008DD RID: 2269
		private static double _horizontalScrollBarHeight;

		// Token: 0x040008DE RID: 2270
		private static double _horizontalScrollBarButtonWidth;

		// Token: 0x040008DF RID: 2271
		private static double _horizontalScrollBarThumbWidth;

		// Token: 0x040008E0 RID: 2272
		private static double _iconWidth;

		// Token: 0x040008E1 RID: 2273
		private static double _iconHeight;

		// Token: 0x040008E2 RID: 2274
		private static double _iconGridWidth;

		// Token: 0x040008E3 RID: 2275
		private static double _iconGridHeight;

		// Token: 0x040008E4 RID: 2276
		private static double _maximizedPrimaryScreenWidth;

		// Token: 0x040008E5 RID: 2277
		private static double _maximizedPrimaryScreenHeight;

		// Token: 0x040008E6 RID: 2278
		private static double _maximumWindowTrackWidth;

		// Token: 0x040008E7 RID: 2279
		private static double _maximumWindowTrackHeight;

		// Token: 0x040008E8 RID: 2280
		private static double _menuCheckmarkWidth;

		// Token: 0x040008E9 RID: 2281
		private static double _menuCheckmarkHeight;

		// Token: 0x040008EA RID: 2282
		private static double _menuButtonWidth;

		// Token: 0x040008EB RID: 2283
		private static double _menuButtonHeight;

		// Token: 0x040008EC RID: 2284
		private static double _minimumWindowWidth;

		// Token: 0x040008ED RID: 2285
		private static double _minimumWindowHeight;

		// Token: 0x040008EE RID: 2286
		private static double _minimizedWindowWidth;

		// Token: 0x040008EF RID: 2287
		private static double _minimizedWindowHeight;

		// Token: 0x040008F0 RID: 2288
		private static double _minimizedGridWidth;

		// Token: 0x040008F1 RID: 2289
		private static double _minimizedGridHeight;

		// Token: 0x040008F2 RID: 2290
		private static double _minimumWindowTrackWidth;

		// Token: 0x040008F3 RID: 2291
		private static double _minimumWindowTrackHeight;

		// Token: 0x040008F4 RID: 2292
		private static double _primaryScreenWidth;

		// Token: 0x040008F5 RID: 2293
		private static double _primaryScreenHeight;

		// Token: 0x040008F6 RID: 2294
		private static double _windowCaptionButtonWidth;

		// Token: 0x040008F7 RID: 2295
		private static double _windowCaptionButtonHeight;

		// Token: 0x040008F8 RID: 2296
		private static double _resizeFrameHorizontalBorderHeight;

		// Token: 0x040008F9 RID: 2297
		private static double _resizeFrameVerticalBorderWidth;

		// Token: 0x040008FA RID: 2298
		private static double _smallIconWidth;

		// Token: 0x040008FB RID: 2299
		private static double _smallIconHeight;

		// Token: 0x040008FC RID: 2300
		private static double _smallWindowCaptionButtonWidth;

		// Token: 0x040008FD RID: 2301
		private static double _smallWindowCaptionButtonHeight;

		// Token: 0x040008FE RID: 2302
		private static double _virtualScreenWidth;

		// Token: 0x040008FF RID: 2303
		private static double _virtualScreenHeight;

		// Token: 0x04000900 RID: 2304
		private static double _verticalScrollBarWidth;

		// Token: 0x04000901 RID: 2305
		private static double _verticalScrollBarButtonHeight;

		// Token: 0x04000902 RID: 2306
		private static double _windowCaptionHeight;

		// Token: 0x04000903 RID: 2307
		private static double _kanjiWindowHeight;

		// Token: 0x04000904 RID: 2308
		private static double _menuBarHeight;

		// Token: 0x04000905 RID: 2309
		private static double _verticalScrollBarThumbHeight;

		// Token: 0x04000906 RID: 2310
		private static bool _isImmEnabled;

		// Token: 0x04000907 RID: 2311
		private static bool _isMediaCenter;

		// Token: 0x04000908 RID: 2312
		private static bool _isMenuDropRightAligned;

		// Token: 0x04000909 RID: 2313
		private static bool _isMiddleEastEnabled;

		// Token: 0x0400090A RID: 2314
		private static bool _isMousePresent;

		// Token: 0x0400090B RID: 2315
		private static bool _isMouseWheelPresent;

		// Token: 0x0400090C RID: 2316
		private static bool _isPenWindows;

		// Token: 0x0400090D RID: 2317
		private static bool _isRemotelyControlled;

		// Token: 0x0400090E RID: 2318
		private static bool _isRemoteSession;

		// Token: 0x0400090F RID: 2319
		private static bool _showSounds;

		// Token: 0x04000910 RID: 2320
		private static bool _isSlowMachine;

		// Token: 0x04000911 RID: 2321
		private static bool _swapButtons;

		// Token: 0x04000912 RID: 2322
		private static bool _isTabletPC;

		// Token: 0x04000913 RID: 2323
		private static double _virtualScreenLeft;

		// Token: 0x04000914 RID: 2324
		private static double _virtualScreenTop;

		// Token: 0x04000915 RID: 2325
		private static PowerLineStatus _powerLineStatus;

		// Token: 0x04000916 RID: 2326
		private static SystemResourceKey _cacheThinHorizontalBorderHeight;

		// Token: 0x04000917 RID: 2327
		private static SystemResourceKey _cacheThinVerticalBorderWidth;

		// Token: 0x04000918 RID: 2328
		private static SystemResourceKey _cacheCursorWidth;

		// Token: 0x04000919 RID: 2329
		private static SystemResourceKey _cacheCursorHeight;

		// Token: 0x0400091A RID: 2330
		private static SystemResourceKey _cacheThickHorizontalBorderHeight;

		// Token: 0x0400091B RID: 2331
		private static SystemResourceKey _cacheThickVerticalBorderWidth;

		// Token: 0x0400091C RID: 2332
		private static SystemResourceKey _cacheFixedFrameHorizontalBorderHeight;

		// Token: 0x0400091D RID: 2333
		private static SystemResourceKey _cacheFixedFrameVerticalBorderWidth;

		// Token: 0x0400091E RID: 2334
		private static SystemResourceKey _cacheFocusHorizontalBorderHeight;

		// Token: 0x0400091F RID: 2335
		private static SystemResourceKey _cacheFocusVerticalBorderWidth;

		// Token: 0x04000920 RID: 2336
		private static SystemResourceKey _cacheFullPrimaryScreenWidth;

		// Token: 0x04000921 RID: 2337
		private static SystemResourceKey _cacheFullPrimaryScreenHeight;

		// Token: 0x04000922 RID: 2338
		private static SystemResourceKey _cacheHorizontalScrollBarButtonWidth;

		// Token: 0x04000923 RID: 2339
		private static SystemResourceKey _cacheHorizontalScrollBarHeight;

		// Token: 0x04000924 RID: 2340
		private static SystemResourceKey _cacheHorizontalScrollBarThumbWidth;

		// Token: 0x04000925 RID: 2341
		private static SystemResourceKey _cacheIconWidth;

		// Token: 0x04000926 RID: 2342
		private static SystemResourceKey _cacheIconHeight;

		// Token: 0x04000927 RID: 2343
		private static SystemResourceKey _cacheIconGridWidth;

		// Token: 0x04000928 RID: 2344
		private static SystemResourceKey _cacheIconGridHeight;

		// Token: 0x04000929 RID: 2345
		private static SystemResourceKey _cacheMaximizedPrimaryScreenWidth;

		// Token: 0x0400092A RID: 2346
		private static SystemResourceKey _cacheMaximizedPrimaryScreenHeight;

		// Token: 0x0400092B RID: 2347
		private static SystemResourceKey _cacheMaximumWindowTrackWidth;

		// Token: 0x0400092C RID: 2348
		private static SystemResourceKey _cacheMaximumWindowTrackHeight;

		// Token: 0x0400092D RID: 2349
		private static SystemResourceKey _cacheMenuCheckmarkWidth;

		// Token: 0x0400092E RID: 2350
		private static SystemResourceKey _cacheMenuCheckmarkHeight;

		// Token: 0x0400092F RID: 2351
		private static SystemResourceKey _cacheMenuButtonWidth;

		// Token: 0x04000930 RID: 2352
		private static SystemResourceKey _cacheMenuButtonHeight;

		// Token: 0x04000931 RID: 2353
		private static SystemResourceKey _cacheMinimumWindowWidth;

		// Token: 0x04000932 RID: 2354
		private static SystemResourceKey _cacheMinimumWindowHeight;

		// Token: 0x04000933 RID: 2355
		private static SystemResourceKey _cacheMinimizedWindowWidth;

		// Token: 0x04000934 RID: 2356
		private static SystemResourceKey _cacheMinimizedWindowHeight;

		// Token: 0x04000935 RID: 2357
		private static SystemResourceKey _cacheMinimizedGridWidth;

		// Token: 0x04000936 RID: 2358
		private static SystemResourceKey _cacheMinimizedGridHeight;

		// Token: 0x04000937 RID: 2359
		private static SystemResourceKey _cacheMinimumWindowTrackWidth;

		// Token: 0x04000938 RID: 2360
		private static SystemResourceKey _cacheMinimumWindowTrackHeight;

		// Token: 0x04000939 RID: 2361
		private static SystemResourceKey _cachePrimaryScreenWidth;

		// Token: 0x0400093A RID: 2362
		private static SystemResourceKey _cachePrimaryScreenHeight;

		// Token: 0x0400093B RID: 2363
		private static SystemResourceKey _cacheWindowCaptionButtonWidth;

		// Token: 0x0400093C RID: 2364
		private static SystemResourceKey _cacheWindowCaptionButtonHeight;

		// Token: 0x0400093D RID: 2365
		private static SystemResourceKey _cacheResizeFrameHorizontalBorderHeight;

		// Token: 0x0400093E RID: 2366
		private static SystemResourceKey _cacheResizeFrameVerticalBorderWidth;

		// Token: 0x0400093F RID: 2367
		private static SystemResourceKey _cacheSmallIconWidth;

		// Token: 0x04000940 RID: 2368
		private static SystemResourceKey _cacheSmallIconHeight;

		// Token: 0x04000941 RID: 2369
		private static SystemResourceKey _cacheSmallWindowCaptionButtonWidth;

		// Token: 0x04000942 RID: 2370
		private static SystemResourceKey _cacheSmallWindowCaptionButtonHeight;

		// Token: 0x04000943 RID: 2371
		private static SystemResourceKey _cacheVirtualScreenWidth;

		// Token: 0x04000944 RID: 2372
		private static SystemResourceKey _cacheVirtualScreenHeight;

		// Token: 0x04000945 RID: 2373
		private static SystemResourceKey _cacheVerticalScrollBarWidth;

		// Token: 0x04000946 RID: 2374
		private static SystemResourceKey _cacheVerticalScrollBarButtonHeight;

		// Token: 0x04000947 RID: 2375
		private static SystemResourceKey _cacheWindowCaptionHeight;

		// Token: 0x04000948 RID: 2376
		private static SystemResourceKey _cacheKanjiWindowHeight;

		// Token: 0x04000949 RID: 2377
		private static SystemResourceKey _cacheMenuBarHeight;

		// Token: 0x0400094A RID: 2378
		private static SystemResourceKey _cacheSmallCaptionHeight;

		// Token: 0x0400094B RID: 2379
		private static SystemResourceKey _cacheVerticalScrollBarThumbHeight;

		// Token: 0x0400094C RID: 2380
		private static SystemResourceKey _cacheIsImmEnabled;

		// Token: 0x0400094D RID: 2381
		private static SystemResourceKey _cacheIsMediaCenter;

		// Token: 0x0400094E RID: 2382
		private static SystemResourceKey _cacheIsMenuDropRightAligned;

		// Token: 0x0400094F RID: 2383
		private static SystemResourceKey _cacheIsMiddleEastEnabled;

		// Token: 0x04000950 RID: 2384
		private static SystemResourceKey _cacheIsMousePresent;

		// Token: 0x04000951 RID: 2385
		private static SystemResourceKey _cacheIsMouseWheelPresent;

		// Token: 0x04000952 RID: 2386
		private static SystemResourceKey _cacheIsPenWindows;

		// Token: 0x04000953 RID: 2387
		private static SystemResourceKey _cacheIsRemotelyControlled;

		// Token: 0x04000954 RID: 2388
		private static SystemResourceKey _cacheIsRemoteSession;

		// Token: 0x04000955 RID: 2389
		private static SystemResourceKey _cacheShowSounds;

		// Token: 0x04000956 RID: 2390
		private static SystemResourceKey _cacheIsSlowMachine;

		// Token: 0x04000957 RID: 2391
		private static SystemResourceKey _cacheSwapButtons;

		// Token: 0x04000958 RID: 2392
		private static SystemResourceKey _cacheIsTabletPC;

		// Token: 0x04000959 RID: 2393
		private static SystemResourceKey _cacheVirtualScreenLeft;

		// Token: 0x0400095A RID: 2394
		private static SystemResourceKey _cacheVirtualScreenTop;

		// Token: 0x0400095B RID: 2395
		private static SystemResourceKey _cacheFocusBorderWidth;

		// Token: 0x0400095C RID: 2396
		private static SystemResourceKey _cacheFocusBorderHeight;

		// Token: 0x0400095D RID: 2397
		private static SystemResourceKey _cacheHighContrast;

		// Token: 0x0400095E RID: 2398
		private static SystemResourceKey _cacheDropShadow;

		// Token: 0x0400095F RID: 2399
		private static SystemResourceKey _cacheFlatMenu;

		// Token: 0x04000960 RID: 2400
		private static SystemResourceKey _cacheWorkArea;

		// Token: 0x04000961 RID: 2401
		private static SystemResourceKey _cacheIconHorizontalSpacing;

		// Token: 0x04000962 RID: 2402
		private static SystemResourceKey _cacheIconVerticalSpacing;

		// Token: 0x04000963 RID: 2403
		private static SystemResourceKey _cacheIconTitleWrap;

		// Token: 0x04000964 RID: 2404
		private static SystemResourceKey _cacheKeyboardCues;

		// Token: 0x04000965 RID: 2405
		private static SystemResourceKey _cacheKeyboardDelay;

		// Token: 0x04000966 RID: 2406
		private static SystemResourceKey _cacheKeyboardPreference;

		// Token: 0x04000967 RID: 2407
		private static SystemResourceKey _cacheKeyboardSpeed;

		// Token: 0x04000968 RID: 2408
		private static SystemResourceKey _cacheSnapToDefaultButton;

		// Token: 0x04000969 RID: 2409
		private static SystemResourceKey _cacheWheelScrollLines;

		// Token: 0x0400096A RID: 2410
		private static SystemResourceKey _cacheMouseHoverTime;

		// Token: 0x0400096B RID: 2411
		private static SystemResourceKey _cacheMouseHoverHeight;

		// Token: 0x0400096C RID: 2412
		private static SystemResourceKey _cacheMouseHoverWidth;

		// Token: 0x0400096D RID: 2413
		private static SystemResourceKey _cacheMenuDropAlignment;

		// Token: 0x0400096E RID: 2414
		private static SystemResourceKey _cacheMenuFade;

		// Token: 0x0400096F RID: 2415
		private static SystemResourceKey _cacheMenuShowDelay;

		// Token: 0x04000970 RID: 2416
		private static SystemResourceKey _cacheComboBoxAnimation;

		// Token: 0x04000971 RID: 2417
		private static SystemResourceKey _cacheClientAreaAnimation;

		// Token: 0x04000972 RID: 2418
		private static SystemResourceKey _cacheCursorShadow;

		// Token: 0x04000973 RID: 2419
		private static SystemResourceKey _cacheGradientCaptions;

		// Token: 0x04000974 RID: 2420
		private static SystemResourceKey _cacheHotTracking;

		// Token: 0x04000975 RID: 2421
		private static SystemResourceKey _cacheListBoxSmoothScrolling;

		// Token: 0x04000976 RID: 2422
		private static SystemResourceKey _cacheMenuAnimation;

		// Token: 0x04000977 RID: 2423
		private static SystemResourceKey _cacheSelectionFade;

		// Token: 0x04000978 RID: 2424
		private static SystemResourceKey _cacheStylusHotTracking;

		// Token: 0x04000979 RID: 2425
		private static SystemResourceKey _cacheToolTipAnimation;

		// Token: 0x0400097A RID: 2426
		private static SystemResourceKey _cacheToolTipFade;

		// Token: 0x0400097B RID: 2427
		private static SystemResourceKey _cacheUIEffects;

		// Token: 0x0400097C RID: 2428
		private static SystemResourceKey _cacheMinimizeAnimation;

		// Token: 0x0400097D RID: 2429
		private static SystemResourceKey _cacheBorder;

		// Token: 0x0400097E RID: 2430
		private static SystemResourceKey _cacheCaretWidth;

		// Token: 0x0400097F RID: 2431
		private static SystemResourceKey _cacheForegroundFlashCount;

		// Token: 0x04000980 RID: 2432
		private static SystemResourceKey _cacheDragFullWindows;

		// Token: 0x04000981 RID: 2433
		private static SystemResourceKey _cacheBorderWidth;

		// Token: 0x04000982 RID: 2434
		private static SystemResourceKey _cacheScrollWidth;

		// Token: 0x04000983 RID: 2435
		private static SystemResourceKey _cacheScrollHeight;

		// Token: 0x04000984 RID: 2436
		private static SystemResourceKey _cacheCaptionWidth;

		// Token: 0x04000985 RID: 2437
		private static SystemResourceKey _cacheCaptionHeight;

		// Token: 0x04000986 RID: 2438
		private static SystemResourceKey _cacheSmallCaptionWidth;

		// Token: 0x04000987 RID: 2439
		private static SystemResourceKey _cacheMenuWidth;

		// Token: 0x04000988 RID: 2440
		private static SystemResourceKey _cacheMenuHeight;

		// Token: 0x04000989 RID: 2441
		private static SystemResourceKey _cacheComboBoxPopupAnimation;

		// Token: 0x0400098A RID: 2442
		private static SystemResourceKey _cacheMenuPopupAnimation;

		// Token: 0x0400098B RID: 2443
		private static SystemResourceKey _cacheToolTipPopupAnimation;

		// Token: 0x0400098C RID: 2444
		private static SystemResourceKey _cachePowerLineStatus;

		// Token: 0x0400098D RID: 2445
		private static SystemThemeKey _cacheFocusVisualStyle;

		// Token: 0x0400098E RID: 2446
		private static SystemThemeKey _cacheNavigationChromeStyle;

		// Token: 0x0400098F RID: 2447
		private static SystemThemeKey _cacheNavigationChromeDownLevelStyle;

		// Token: 0x0200082A RID: 2090
		private enum CacheSlot
		{
			// Token: 0x04003C14 RID: 15380
			DpiX,
			// Token: 0x04003C15 RID: 15381
			FocusBorderWidth,
			// Token: 0x04003C16 RID: 15382
			FocusBorderHeight,
			// Token: 0x04003C17 RID: 15383
			HighContrast,
			// Token: 0x04003C18 RID: 15384
			MouseVanish,
			// Token: 0x04003C19 RID: 15385
			DropShadow,
			// Token: 0x04003C1A RID: 15386
			FlatMenu,
			// Token: 0x04003C1B RID: 15387
			WorkAreaInternal,
			// Token: 0x04003C1C RID: 15388
			WorkArea,
			// Token: 0x04003C1D RID: 15389
			IconMetrics,
			// Token: 0x04003C1E RID: 15390
			KeyboardCues,
			// Token: 0x04003C1F RID: 15391
			KeyboardDelay,
			// Token: 0x04003C20 RID: 15392
			KeyboardPreference,
			// Token: 0x04003C21 RID: 15393
			KeyboardSpeed,
			// Token: 0x04003C22 RID: 15394
			SnapToDefaultButton,
			// Token: 0x04003C23 RID: 15395
			WheelScrollLines,
			// Token: 0x04003C24 RID: 15396
			MouseHoverTime,
			// Token: 0x04003C25 RID: 15397
			MouseHoverHeight,
			// Token: 0x04003C26 RID: 15398
			MouseHoverWidth,
			// Token: 0x04003C27 RID: 15399
			MenuDropAlignment,
			// Token: 0x04003C28 RID: 15400
			MenuFade,
			// Token: 0x04003C29 RID: 15401
			MenuShowDelay,
			// Token: 0x04003C2A RID: 15402
			ComboBoxAnimation,
			// Token: 0x04003C2B RID: 15403
			ClientAreaAnimation,
			// Token: 0x04003C2C RID: 15404
			CursorShadow,
			// Token: 0x04003C2D RID: 15405
			GradientCaptions,
			// Token: 0x04003C2E RID: 15406
			HotTracking,
			// Token: 0x04003C2F RID: 15407
			ListBoxSmoothScrolling,
			// Token: 0x04003C30 RID: 15408
			MenuAnimation,
			// Token: 0x04003C31 RID: 15409
			SelectionFade,
			// Token: 0x04003C32 RID: 15410
			StylusHotTracking,
			// Token: 0x04003C33 RID: 15411
			ToolTipAnimation,
			// Token: 0x04003C34 RID: 15412
			ToolTipFade,
			// Token: 0x04003C35 RID: 15413
			UIEffects,
			// Token: 0x04003C36 RID: 15414
			MinimizeAnimation,
			// Token: 0x04003C37 RID: 15415
			Border,
			// Token: 0x04003C38 RID: 15416
			CaretWidth,
			// Token: 0x04003C39 RID: 15417
			ForegroundFlashCount,
			// Token: 0x04003C3A RID: 15418
			DragFullWindows,
			// Token: 0x04003C3B RID: 15419
			NonClientMetrics,
			// Token: 0x04003C3C RID: 15420
			ThinHorizontalBorderHeight,
			// Token: 0x04003C3D RID: 15421
			ThinVerticalBorderWidth,
			// Token: 0x04003C3E RID: 15422
			CursorWidth,
			// Token: 0x04003C3F RID: 15423
			CursorHeight,
			// Token: 0x04003C40 RID: 15424
			ThickHorizontalBorderHeight,
			// Token: 0x04003C41 RID: 15425
			ThickVerticalBorderWidth,
			// Token: 0x04003C42 RID: 15426
			MinimumHorizontalDragDistance,
			// Token: 0x04003C43 RID: 15427
			MinimumVerticalDragDistance,
			// Token: 0x04003C44 RID: 15428
			FixedFrameHorizontalBorderHeight,
			// Token: 0x04003C45 RID: 15429
			FixedFrameVerticalBorderWidth,
			// Token: 0x04003C46 RID: 15430
			FocusHorizontalBorderHeight,
			// Token: 0x04003C47 RID: 15431
			FocusVerticalBorderWidth,
			// Token: 0x04003C48 RID: 15432
			FullPrimaryScreenWidth,
			// Token: 0x04003C49 RID: 15433
			FullPrimaryScreenHeight,
			// Token: 0x04003C4A RID: 15434
			HorizontalScrollBarButtonWidth,
			// Token: 0x04003C4B RID: 15435
			HorizontalScrollBarHeight,
			// Token: 0x04003C4C RID: 15436
			HorizontalScrollBarThumbWidth,
			// Token: 0x04003C4D RID: 15437
			IconWidth,
			// Token: 0x04003C4E RID: 15438
			IconHeight,
			// Token: 0x04003C4F RID: 15439
			IconGridWidth,
			// Token: 0x04003C50 RID: 15440
			IconGridHeight,
			// Token: 0x04003C51 RID: 15441
			MaximizedPrimaryScreenWidth,
			// Token: 0x04003C52 RID: 15442
			MaximizedPrimaryScreenHeight,
			// Token: 0x04003C53 RID: 15443
			MaximumWindowTrackWidth,
			// Token: 0x04003C54 RID: 15444
			MaximumWindowTrackHeight,
			// Token: 0x04003C55 RID: 15445
			MenuCheckmarkWidth,
			// Token: 0x04003C56 RID: 15446
			MenuCheckmarkHeight,
			// Token: 0x04003C57 RID: 15447
			MenuButtonWidth,
			// Token: 0x04003C58 RID: 15448
			MenuButtonHeight,
			// Token: 0x04003C59 RID: 15449
			MinimumWindowWidth,
			// Token: 0x04003C5A RID: 15450
			MinimumWindowHeight,
			// Token: 0x04003C5B RID: 15451
			MinimizedWindowWidth,
			// Token: 0x04003C5C RID: 15452
			MinimizedWindowHeight,
			// Token: 0x04003C5D RID: 15453
			MinimizedGridWidth,
			// Token: 0x04003C5E RID: 15454
			MinimizedGridHeight,
			// Token: 0x04003C5F RID: 15455
			MinimumWindowTrackWidth,
			// Token: 0x04003C60 RID: 15456
			MinimumWindowTrackHeight,
			// Token: 0x04003C61 RID: 15457
			PrimaryScreenWidth,
			// Token: 0x04003C62 RID: 15458
			PrimaryScreenHeight,
			// Token: 0x04003C63 RID: 15459
			WindowCaptionButtonWidth,
			// Token: 0x04003C64 RID: 15460
			WindowCaptionButtonHeight,
			// Token: 0x04003C65 RID: 15461
			ResizeFrameHorizontalBorderHeight,
			// Token: 0x04003C66 RID: 15462
			ResizeFrameVerticalBorderWidth,
			// Token: 0x04003C67 RID: 15463
			SmallIconWidth,
			// Token: 0x04003C68 RID: 15464
			SmallIconHeight,
			// Token: 0x04003C69 RID: 15465
			SmallWindowCaptionButtonWidth,
			// Token: 0x04003C6A RID: 15466
			SmallWindowCaptionButtonHeight,
			// Token: 0x04003C6B RID: 15467
			VirtualScreenWidth,
			// Token: 0x04003C6C RID: 15468
			VirtualScreenHeight,
			// Token: 0x04003C6D RID: 15469
			VerticalScrollBarWidth,
			// Token: 0x04003C6E RID: 15470
			VerticalScrollBarButtonHeight,
			// Token: 0x04003C6F RID: 15471
			WindowCaptionHeight,
			// Token: 0x04003C70 RID: 15472
			KanjiWindowHeight,
			// Token: 0x04003C71 RID: 15473
			MenuBarHeight,
			// Token: 0x04003C72 RID: 15474
			VerticalScrollBarThumbHeight,
			// Token: 0x04003C73 RID: 15475
			IsImmEnabled,
			// Token: 0x04003C74 RID: 15476
			IsMediaCenter,
			// Token: 0x04003C75 RID: 15477
			IsMenuDropRightAligned,
			// Token: 0x04003C76 RID: 15478
			IsMiddleEastEnabled,
			// Token: 0x04003C77 RID: 15479
			IsMousePresent,
			// Token: 0x04003C78 RID: 15480
			IsMouseWheelPresent,
			// Token: 0x04003C79 RID: 15481
			IsPenWindows,
			// Token: 0x04003C7A RID: 15482
			IsRemotelyControlled,
			// Token: 0x04003C7B RID: 15483
			IsRemoteSession,
			// Token: 0x04003C7C RID: 15484
			ShowSounds,
			// Token: 0x04003C7D RID: 15485
			IsSlowMachine,
			// Token: 0x04003C7E RID: 15486
			SwapButtons,
			// Token: 0x04003C7F RID: 15487
			IsTabletPC,
			// Token: 0x04003C80 RID: 15488
			VirtualScreenLeft,
			// Token: 0x04003C81 RID: 15489
			VirtualScreenTop,
			// Token: 0x04003C82 RID: 15490
			PowerLineStatus,
			// Token: 0x04003C83 RID: 15491
			IsGlassEnabled,
			// Token: 0x04003C84 RID: 15492
			UxThemeName,
			// Token: 0x04003C85 RID: 15493
			UxThemeColor,
			// Token: 0x04003C86 RID: 15494
			WindowCornerRadius,
			// Token: 0x04003C87 RID: 15495
			WindowGlassColor,
			// Token: 0x04003C88 RID: 15496
			WindowGlassBrush,
			// Token: 0x04003C89 RID: 15497
			WindowNonClientFrameThickness,
			// Token: 0x04003C8A RID: 15498
			WindowResizeBorderThickness,
			// Token: 0x04003C8B RID: 15499
			NumSlots
		}
	}
}
