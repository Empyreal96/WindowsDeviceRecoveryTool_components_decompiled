using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	/// <summary>Provides information about the current system environment.</summary>
	// Token: 0x02000371 RID: 881
	public class SystemInformation
	{
		// Token: 0x06003735 RID: 14133 RVA: 0x000027DB File Offset: 0x000009DB
		private SystemInformation()
		{
		}

		/// <summary>Gets a value indicating whether the user has enabled full window drag.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled full window drag; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x000F9DDC File Offset: 0x000F7FDC
		public static bool DragFullWindows
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(38, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether the user has enabled the high-contrast mode accessibility feature.</summary>
		/// <returns>
		///     <see langword="true" /> if the user has enabled high-contrast mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x000F9DFC File Offset: 0x000F7FFC
		public static bool HighContrast
		{
			get
			{
				SystemInformation.EnsureSystemEvents();
				if (SystemInformation.systemEventsDirty)
				{
					NativeMethods.HIGHCONTRAST_I highcontrast_I = default(NativeMethods.HIGHCONTRAST_I);
					highcontrast_I.cbSize = Marshal.SizeOf(highcontrast_I);
					highcontrast_I.dwFlags = 0;
					highcontrast_I.lpszDefaultScheme = IntPtr.Zero;
					bool flag = UnsafeNativeMethods.SystemParametersInfo(66, highcontrast_I.cbSize, ref highcontrast_I, 0);
					if (flag)
					{
						SystemInformation.highContrast = ((highcontrast_I.dwFlags & 1) != 0);
					}
					else
					{
						SystemInformation.highContrast = false;
					}
					SystemInformation.systemEventsDirty = false;
				}
				return SystemInformation.highContrast;
			}
		}

		/// <summary>Gets the number of lines to scroll when the mouse wheel is rotated.</summary>
		/// <returns>The number of lines to scroll on a mouse wheel rotation, or -1 if the "One screen at a time" mouse option is selected.</returns>
		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x000F9E7C File Offset: 0x000F807C
		public static int MouseWheelScrollLines
		{
			get
			{
				if (SystemInformation.NativeMouseWheelSupport)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(104, 0, ref result, 0);
					return result;
				}
				IntPtr intPtr = IntPtr.Zero;
				intPtr = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
				if (intPtr != IntPtr.Zero)
				{
					int msg = SafeNativeMethods.RegisterWindowMessage("MSH_SCROLL_LINES_MSG");
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(null, intPtr), msg, 0, 0);
					if (num != 0)
					{
						return num;
					}
				}
				return 3;
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the current video mode of the primary display.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the dimensions, in pixels, of the current video mode of the primary display.</returns>
		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x000F9EE9 File Offset: 0x000F80E9
		public static Size PrimaryMonitorSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(0), UnsafeNativeMethods.GetSystemMetrics(1));
			}
		}

		/// <summary>Gets the default width, in pixels, of the vertical scroll bar.</summary>
		/// <returns>The default width, in pixels, of the vertical scroll bar.</returns>
		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x000F9EFC File Offset: 0x000F80FC
		public static int VerticalScrollBarWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(2);
			}
		}

		/// <summary>Gets the default height, in pixels, of the vertical scroll bar for a given DPI value.</summary>
		/// <param name="dpi">The DPI value for the current display device.</param>
		/// <returns>The default height, in pixels, of the vertical scroll bar.</returns>
		// Token: 0x0600373B RID: 14139 RVA: 0x000F9F04 File Offset: 0x000F8104
		public static int GetVerticalScrollBarWidthForDpi(int dpi)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				return UnsafeNativeMethods.TryGetSystemMetricsForDpi(2, (uint)dpi);
			}
			return UnsafeNativeMethods.GetSystemMetrics(2);
		}

		/// <summary>Gets the default height, in pixels, of the horizontal scroll bar.</summary>
		/// <returns>The default height, in pixels, of the horizontal scroll bar.</returns>
		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x000F9F1B File Offset: 0x000F811B
		public static int HorizontalScrollBarHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(3);
			}
		}

		/// <summary>Gets the default height, in pixels, of the horizontal scroll bar for a given DPI value.</summary>
		/// <param name="dpi">The DPI value for the current display device.</param>
		/// <returns>The default height, in pixels, of the horizontal scroll bar.</returns>
		// Token: 0x0600373D RID: 14141 RVA: 0x000F9F23 File Offset: 0x000F8123
		public static int GetHorizontalScrollBarHeightForDpi(int dpi)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				return UnsafeNativeMethods.TryGetSystemMetricsForDpi(3, (uint)dpi);
			}
			return UnsafeNativeMethods.GetSystemMetrics(3);
		}

		/// <summary>Gets the height, in pixels, of the standard title bar area of a window.</summary>
		/// <returns>The height, in pixels, of the standard title bar area of a window.</returns>
		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x0600373E RID: 14142 RVA: 0x000F9F3A File Offset: 0x000F813A
		public static int CaptionHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(4);
			}
		}

		/// <summary>Gets the thickness, in pixels, of a flat-style window or system control border.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the width, in pixels, of a vertical border, and the height, in pixels, of a horizontal border.</returns>
		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x0600373F RID: 14143 RVA: 0x000F9F42 File Offset: 0x000F8142
		public static Size BorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(5), UnsafeNativeMethods.GetSystemMetrics(6));
			}
		}

		/// <summary>Gets the thickness, in pixels, of a flat-style window or system control border for a given DPI value.</summary>
		/// <param name="dpi">The DPI value for the current display device.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the width, in pixels, of a vertical border, and the height, in pixels, of a horizontal border.</returns>
		// Token: 0x06003740 RID: 14144 RVA: 0x000F9F55 File Offset: 0x000F8155
		public static Size GetBorderSizeForDpi(int dpi)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				return new Size(UnsafeNativeMethods.TryGetSystemMetricsForDpi(5, (uint)dpi), UnsafeNativeMethods.TryGetSystemMetricsForDpi(6, (uint)dpi));
			}
			return SystemInformation.BorderSize;
		}

		/// <summary>Gets the thickness, in pixels, of the frame border of a window that has a caption and is not resizable.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the thickness, in pixels, of a fixed sized window border.</returns>
		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003741 RID: 14145 RVA: 0x000F9F77 File Offset: 0x000F8177
		public static Size FixedFrameBorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(7), UnsafeNativeMethods.GetSystemMetrics(8));
			}
		}

		/// <summary>Gets the height, in pixels, of the scroll box in a vertical scroll bar.</summary>
		/// <returns>The height, in pixels, of the scroll box in a vertical scroll bar.</returns>
		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003742 RID: 14146 RVA: 0x000F9F8A File Offset: 0x000F818A
		public static int VerticalScrollBarThumbHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(9);
			}
		}

		/// <summary>Gets the width, in pixels, of the scroll box in a horizontal scroll bar.</summary>
		/// <returns>The width, in pixels, of the scroll box in a horizontal scroll bar.</returns>
		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003743 RID: 14147 RVA: 0x000F9F93 File Offset: 0x000F8193
		public static int HorizontalScrollBarThumbWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(10);
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the Windows default program icon size.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the default dimensions, in pixels, for a program icon.</returns>
		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003744 RID: 14148 RVA: 0x000F9F9C File Offset: 0x000F819C
		public static Size IconSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(11), UnsafeNativeMethods.GetSystemMetrics(12));
			}
		}

		/// <summary>Gets the maximum size, in pixels, that a cursor can occupy.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the maximum dimensions of a cursor in pixels.</returns>
		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003745 RID: 14149 RVA: 0x000F9FB1 File Offset: 0x000F81B1
		public static Size CursorSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(13), UnsafeNativeMethods.GetSystemMetrics(14));
			}
		}

		/// <summary>Gets the font used to display text on menus.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> used to display text on menus.</returns>
		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003746 RID: 14150 RVA: 0x000F9FC6 File Offset: 0x000F81C6
		public static Font MenuFont
		{
			get
			{
				return SystemInformation.GetMenuFontHelper(0U, false);
			}
		}

		/// <summary>Gets the font used to display text on menus for use in changing the DPI for a given display device.</summary>
		/// <param name="dpi">The DPI value for the current display device.</param>
		/// <returns>The <see cref="T:System.Drawing.Font" /> used to display text on menus.</returns>
		// Token: 0x06003747 RID: 14151 RVA: 0x000F9FCF File Offset: 0x000F81CF
		public static Font GetMenuFontForDpi(int dpi)
		{
			return SystemInformation.GetMenuFontHelper((uint)dpi, DpiHelper.EnableDpiChangedMessageHandling);
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x000F9FDC File Offset: 0x000F81DC
		private static Font GetMenuFontHelper(uint dpi, bool useDpi)
		{
			Font result = null;
			NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
			bool flag;
			if (useDpi)
			{
				flag = UnsafeNativeMethods.TrySystemParametersInfoForDpi(41, nonclientmetrics.cbSize, nonclientmetrics, 0, dpi);
			}
			else
			{
				flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
			}
			if (flag && nonclientmetrics.lfMenuFont != null)
			{
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					result = Font.FromLogFont(nonclientmetrics.lfMenuFont);
				}
				catch
				{
					result = Control.DefaultFont;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return result;
		}

		/// <summary>Gets the height, in pixels, of one line of a menu.</summary>
		/// <returns>The height, in pixels, of one line of a menu.</returns>
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003749 RID: 14153 RVA: 0x000FA068 File Offset: 0x000F8268
		public static int MenuHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(15);
			}
		}

		/// <summary>Gets the current system power status.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.PowerStatus" /> that indicates the current system power status.</returns>
		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x000FA071 File Offset: 0x000F8271
		public static PowerStatus PowerStatus
		{
			get
			{
				if (SystemInformation.powerStatus == null)
				{
					SystemInformation.powerStatus = new PowerStatus();
				}
				return SystemInformation.powerStatus;
			}
		}

		/// <summary>Gets the size, in pixels, of the working area of the screen.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the size, in pixels, of the working area of the screen.</returns>
		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x000FA08C File Offset: 0x000F828C
		public static Rectangle WorkingArea
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.SystemParametersInfo(48, 0, ref rect, 0);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
		}

		/// <summary>Gets the height, in pixels, of the Kanji window at the bottom of the screen for double-byte character set (DBCS) versions of Windows.</summary>
		/// <returns>The height, in pixels, of the Kanji window.</returns>
		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x000FA0CA File Offset: 0x000F82CA
		public static int KanjiWindowHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(18);
			}
		}

		/// <summary>Gets a value indicating whether a pointing device is installed.</summary>
		/// <returns>
		///     <see langword="true" /> if a mouse is installed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x000FA0D3 File Offset: 0x000F82D3
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool MousePresent
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(19) != 0;
			}
		}

		/// <summary>Gets the height, in pixels, of the arrow bitmap on the vertical scroll bar.</summary>
		/// <returns>The height, in pixels, of the arrow bitmap on the vertical scroll bar.</returns>
		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000FA0DF File Offset: 0x000F82DF
		public static int VerticalScrollBarArrowHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(20);
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000FA0E8 File Offset: 0x000F82E8
		public static int VerticalScrollBarArrowHeightForDpi(int dpi)
		{
			return UnsafeNativeMethods.TryGetSystemMetricsForDpi(21, (uint)dpi);
		}

		/// <summary>Gets the width, in pixels, of the arrow bitmap on the horizontal scroll bar.</summary>
		/// <returns>The width, in pixels, of the arrow bitmap on the horizontal scroll bar.</returns>
		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x000FA0F2 File Offset: 0x000F82F2
		public static int HorizontalScrollBarArrowWidth
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(21);
			}
		}

		/// <summary>Gets the width of the horizontal scroll bar arrow bitmap in pixels. </summary>
		/// <param name="dpi">The DPI value for the current display device. </param>
		/// <returns>The default width, in pixels, of the horizontal scroll bar arrow. </returns>
		// Token: 0x06003751 RID: 14161 RVA: 0x000FA0FB File Offset: 0x000F82FB
		public static int GetHorizontalScrollBarArrowWidthForDpi(int dpi)
		{
			if (DpiHelper.EnableDpiChangedMessageHandling)
			{
				return UnsafeNativeMethods.TryGetSystemMetricsForDpi(21, (uint)dpi);
			}
			return UnsafeNativeMethods.GetSystemMetrics(21);
		}

		/// <summary>Gets a value indicating whether the debug version of USER.EXE is installed.</summary>
		/// <returns>
		///     <see langword="true" /> if the debugging version of USER.EXE is installed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x000FA114 File Offset: 0x000F8314
		public static bool DebugOS
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return UnsafeNativeMethods.GetSystemMetrics(22) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the functions of the left and right mouse buttons have been swapped.</summary>
		/// <returns>
		///     <see langword="true" /> if the functions of the left and right mouse buttons are swapped; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x000FA12A File Offset: 0x000F832A
		public static bool MouseButtonsSwapped
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(23) != 0;
			}
		}

		/// <summary>Gets the minimum width and height for a window, in pixels.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the minimum allowable dimensions of a window, in pixels.</returns>
		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x000FA136 File Offset: 0x000F8336
		public static Size MinimumWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(28), UnsafeNativeMethods.GetSystemMetrics(29));
			}
		}

		/// <summary>Gets the standard size, in pixels, of a button in a window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the standard dimensions, in pixels, of a button in a window's title bar.</returns>
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x000FA14B File Offset: 0x000F834B
		public static Size CaptionButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(30), UnsafeNativeMethods.GetSystemMetrics(31));
			}
		}

		/// <summary>Gets the thickness, in pixels, of the resizing border that is drawn around the perimeter of a window that is being drag resized.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the thickness, in pixels, of the width of a vertical resizing border and the height of a horizontal resizing border.</returns>
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x000FA160 File Offset: 0x000F8360
		public static Size FrameBorderSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(32), UnsafeNativeMethods.GetSystemMetrics(33));
			}
		}

		/// <summary>Gets the default minimum dimensions, in pixels, that a window may occupy during a drag resize.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the default minimum width and height of a window during resize, in pixels.</returns>
		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x000FA175 File Offset: 0x000F8375
		public static Size MinWindowTrackSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(34), UnsafeNativeMethods.GetSystemMetrics(35));
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the area within which the user must click twice for the operating system to consider the two clicks a double-click.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the dimensions, in pixels, of the area within which the user must click twice for the operating system to consider the two clicks a double-click.</returns>
		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x000FA18A File Offset: 0x000F838A
		public static Size DoubleClickSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(36), UnsafeNativeMethods.GetSystemMetrics(37));
			}
		}

		/// <summary>Gets the maximum number of milliseconds that can elapse between a first click and a second click for the OS to consider the mouse action a double-click.</summary>
		/// <returns>The maximum amount of time, in milliseconds, that can elapse between a first click and a second click for the OS to consider the mouse action a double-click.</returns>
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x000FA19F File Offset: 0x000F839F
		public static int DoubleClickTime
		{
			get
			{
				return SafeNativeMethods.GetDoubleClickTime();
			}
		}

		/// <summary>Gets the size, in pixels, of the grid square used to arrange icons in a large-icon view.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the dimensions, in pixels, of the grid square used to arrange icons in a large-icon view.</returns>
		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x000FA1A6 File Offset: 0x000F83A6
		public static Size IconSpacingSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(38), UnsafeNativeMethods.GetSystemMetrics(39));
			}
		}

		/// <summary>Gets a value indicating whether drop-down menus are right-aligned with the corresponding menu-bar item.</summary>
		/// <returns>
		///     <see langword="true" /> if drop-down menus are right-aligned with the corresponding menu-bar item; <see langword="false" /> if the menus are left-aligned.</returns>
		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x000FA1BB File Offset: 0x000F83BB
		public static bool RightAlignedMenus
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(40) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the Microsoft Windows for Pen Computing extensions are installed.</summary>
		/// <returns>
		///     <see langword="true" /> if the Windows for Pen Computing extensions are installed; <see langword="false" /> if Windows for Pen Computing extensions are not installed.</returns>
		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x000FA1C7 File Offset: 0x000F83C7
		public static bool PenWindows
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(41) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the operating system is capable of handling double-byte character set (DBCS) characters.</summary>
		/// <returns>
		///     <see langword="true" /> if the operating system supports DBCS; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x000FA1D3 File Offset: 0x000F83D3
		public static bool DbcsEnabled
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(42) != 0;
			}
		}

		/// <summary>Gets the number of buttons on the mouse.</summary>
		/// <returns>The number of buttons on the mouse, or zero if no mouse is installed.</returns>
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x000FA1DF File Offset: 0x000F83DF
		public static int MouseButtons
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(43);
			}
		}

		/// <summary>Gets a value indicating whether a Security Manager is present on this operating system.</summary>
		/// <returns>
		///     <see langword="true" /> if a Security Manager is present; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000FA1E8 File Offset: 0x000F83E8
		public static bool Secure
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return UnsafeNativeMethods.GetSystemMetrics(44) != 0;
			}
		}

		/// <summary>Gets the thickness, in pixels, of a three-dimensional (3-D) style window or system control border.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the width, in pixels, of a 3-D style vertical border, and the height, in pixels, of a 3-D style horizontal border.</returns>
		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x000FA1FE File Offset: 0x000F83FE
		public static Size Border3DSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(45), UnsafeNativeMethods.GetSystemMetrics(46));
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the area each minimized window is allocated when arranged.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the area each minimized window is allocated when arranged.</returns>
		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x000FA213 File Offset: 0x000F8413
		public static Size MinimizedWindowSpacingSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(47), UnsafeNativeMethods.GetSystemMetrics(48));
			}
		}

		/// <summary>Gets the dimensions, in pixels, of a small icon.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the dimensions, in pixels, of a small icon.</returns>
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x000FA228 File Offset: 0x000F8428
		public static Size SmallIconSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(49), UnsafeNativeMethods.GetSystemMetrics(50));
			}
		}

		/// <summary>Gets the height, in pixels, of a tool window caption.</summary>
		/// <returns>The height, in pixels, of a tool window caption in pixels.</returns>
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003763 RID: 14179 RVA: 0x000FA23D File Offset: 0x000F843D
		public static int ToolWindowCaptionHeight
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(51);
			}
		}

		/// <summary>Gets the dimensions, in pixels, of small caption buttons.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the dimensions, in pixels, of small caption buttons.</returns>
		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x000FA246 File Offset: 0x000F8446
		public static Size ToolWindowCaptionButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(52), UnsafeNativeMethods.GetSystemMetrics(53));
			}
		}

		/// <summary>Gets the default dimensions, in pixels, of menu-bar buttons.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the default dimensions, in pixels, of menu-bar buttons.</returns>
		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003765 RID: 14181 RVA: 0x000FA25B File Offset: 0x000F845B
		public static Size MenuButtonSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(54), UnsafeNativeMethods.GetSystemMetrics(55));
			}
		}

		/// <summary>Gets an <see cref="T:System.Windows.Forms.ArrangeStartingPosition" /> value that indicates the starting position from which the operating system arranges minimized windows.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ArrangeStartingPosition" /> values that indicates the starting position from which the operating system arranges minimized windows.</returns>
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003766 RID: 14182 RVA: 0x000FA270 File Offset: 0x000F8470
		public static ArrangeStartingPosition ArrangeStartingPosition
		{
			get
			{
				ArrangeStartingPosition arrangeStartingPosition = ArrangeStartingPosition.BottomRight | ArrangeStartingPosition.Hide | ArrangeStartingPosition.TopLeft;
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(56);
				return arrangeStartingPosition & (ArrangeStartingPosition)systemMetrics;
			}
		}

		/// <summary>Gets a value that indicates the direction in which the operating system arranges minimized windows.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ArrangeDirection" /> values that indicates the direction in which the operating system arranges minimized windows.</returns>
		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x000FA28C File Offset: 0x000F848C
		public static ArrangeDirection ArrangeDirection
		{
			get
			{
				ArrangeDirection arrangeDirection = ArrangeDirection.Down;
				int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(56);
				return arrangeDirection & (ArrangeDirection)systemMetrics;
			}
		}

		/// <summary>Gets the dimensions, in pixels, of a normal minimized window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the dimensions, in pixels, of a normal minimized window.</returns>
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x000FA2A6 File Offset: 0x000F84A6
		public static Size MinimizedWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(57), UnsafeNativeMethods.GetSystemMetrics(58));
			}
		}

		/// <summary>Gets the default maximum dimensions, in pixels, of a window that has a caption and sizing borders.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the maximum dimensions, in pixels, to which a window can be sized.</returns>
		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x000FA2BB File Offset: 0x000F84BB
		public static Size MaxWindowTrackSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(59), UnsafeNativeMethods.GetSystemMetrics(60));
			}
		}

		/// <summary>Gets the default dimensions, in pixels, of a maximized window on the primary display.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the dimensions, in pixels, of a maximized window on the primary display.</returns>
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x0600376A RID: 14186 RVA: 0x000FA2D0 File Offset: 0x000F84D0
		public static Size PrimaryMonitorMaximizedWindowSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(61), UnsafeNativeMethods.GetSystemMetrics(62));
			}
		}

		/// <summary>Gets a value indicating whether a network connection is present.</summary>
		/// <returns>
		///     <see langword="true" /> if a network connection is present; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x000FA2E5 File Offset: 0x000F84E5
		public static bool Network
		{
			get
			{
				return (UnsafeNativeMethods.GetSystemMetrics(63) & 1) != 0;
			}
		}

		/// <summary>Gets a value indicating whether the calling process is associated with a Terminal Services client session.</summary>
		/// <returns>
		///     <see langword="true" /> if the calling process is associated with a Terminal Services client session; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x000FA2F3 File Offset: 0x000F84F3
		public static bool TerminalServerSession
		{
			get
			{
				return (UnsafeNativeMethods.GetSystemMetrics(4096) & 1) != 0;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Forms.BootMode" /> value that indicates the boot mode the system was started in.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BootMode" /> values that indicates the boot mode the system was started in.</returns>
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x000FA304 File Offset: 0x000F8504
		public static BootMode BootMode
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				return (BootMode)UnsafeNativeMethods.GetSystemMetrics(67);
			}
		}

		/// <summary>Gets the width and height of a rectangle centered on the point the mouse button was pressed, within which a drag operation will not begin.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that specifies the area of a rectangle, in pixels, centered on the point the mouse button was pressed, within which a drag operation will not begin.</returns>
		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x0600376E RID: 14190 RVA: 0x000FA317 File Offset: 0x000F8517
		public static Size DragSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(68), UnsafeNativeMethods.GetSystemMetrics(69));
			}
		}

		/// <summary>Gets a value indicating whether the user prefers that an application present information in visual form in situations when it would present the information in audible form.</summary>
		/// <returns>
		///     <see langword="true" /> if the application should visually show information about audible output; <see langword="false" /> if the application does not need to provide extra visual cues for audio events.</returns>
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x000FA32C File Offset: 0x000F852C
		public static bool ShowSounds
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(70) != 0;
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the default size of a menu check mark area.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the default size, in pixels, of a menu check mark area.</returns>
		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06003770 RID: 14192 RVA: 0x000FA338 File Offset: 0x000F8538
		public static Size MenuCheckSize
		{
			get
			{
				return new Size(UnsafeNativeMethods.GetSystemMetrics(71), UnsafeNativeMethods.GetSystemMetrics(72));
			}
		}

		/// <summary>Gets a value indicating whether the operating system is enabled for the Hebrew and Arabic languages.</summary>
		/// <returns>
		///     <see langword="true" /> if the operating system is enabled for Hebrew or Arabic; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003771 RID: 14193 RVA: 0x000FA34D File Offset: 0x000F854D
		public static bool MidEastEnabled
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(74) != 0;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003772 RID: 14194 RVA: 0x000FA359 File Offset: 0x000F8559
		private static bool MultiMonitorSupport
		{
			get
			{
				if (!SystemInformation.checkMultiMonitorSupport)
				{
					SystemInformation.multiMonitorSupport = (UnsafeNativeMethods.GetSystemMetrics(80) != 0);
					SystemInformation.checkMultiMonitorSupport = true;
				}
				return SystemInformation.multiMonitorSupport;
			}
		}

		/// <summary>Gets a value indicating whether the operating system natively supports a mouse wheel.</summary>
		/// <returns>
		///     <see langword="true" /> if the operating system natively supports a mouse wheel; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003773 RID: 14195 RVA: 0x000FA37C File Offset: 0x000F857C
		public static bool NativeMouseWheelSupport
		{
			get
			{
				if (!SystemInformation.checkNativeMouseWheelSupport)
				{
					SystemInformation.nativeMouseWheelSupport = (UnsafeNativeMethods.GetSystemMetrics(75) != 0);
					SystemInformation.checkNativeMouseWheelSupport = true;
				}
				return SystemInformation.nativeMouseWheelSupport;
			}
		}

		/// <summary>Gets a value indicating whether a mouse with a mouse wheel is installed.</summary>
		/// <returns>
		///     <see langword="true" /> if a mouse with a mouse wheel is installed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003774 RID: 14196 RVA: 0x000FA3A0 File Offset: 0x000F85A0
		public static bool MouseWheelPresent
		{
			get
			{
				bool result = false;
				if (!SystemInformation.NativeMouseWheelSupport)
				{
					IntPtr value = IntPtr.Zero;
					value = UnsafeNativeMethods.FindWindow("MouseZ", "Magellan MSWHEEL");
					if (value != IntPtr.Zero)
					{
						result = true;
					}
				}
				else
				{
					result = (UnsafeNativeMethods.GetSystemMetrics(75) != 0);
				}
				return result;
			}
		}

		/// <summary>Gets the bounds of the virtual screen.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that specifies the bounding rectangle of the entire virtual screen.</returns>
		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003775 RID: 14197 RVA: 0x000FA3EC File Offset: 0x000F85EC
		public static Rectangle VirtualScreen
		{
			get
			{
				if (SystemInformation.MultiMonitorSupport)
				{
					return new Rectangle(UnsafeNativeMethods.GetSystemMetrics(76), UnsafeNativeMethods.GetSystemMetrics(77), UnsafeNativeMethods.GetSystemMetrics(78), UnsafeNativeMethods.GetSystemMetrics(79));
				}
				Size primaryMonitorSize = SystemInformation.PrimaryMonitorSize;
				return new Rectangle(0, 0, primaryMonitorSize.Width, primaryMonitorSize.Height);
			}
		}

		/// <summary>Gets the number of display monitors on the desktop.</summary>
		/// <returns>The number of monitors that make up the desktop.</returns>
		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06003776 RID: 14198 RVA: 0x000FA43D File Offset: 0x000F863D
		public static int MonitorCount
		{
			get
			{
				if (SystemInformation.MultiMonitorSupport)
				{
					return UnsafeNativeMethods.GetSystemMetrics(80);
				}
				return 1;
			}
		}

		/// <summary>Gets a value indicating whether all the display monitors are using the same pixel color format.</summary>
		/// <returns>
		///     <see langword="true" /> if all monitors are using the same pixel color format; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x000FA44F File Offset: 0x000F864F
		public static bool MonitorsSameDisplayFormat
		{
			get
			{
				return !SystemInformation.MultiMonitorSupport || UnsafeNativeMethods.GetSystemMetrics(81) != 0;
			}
		}

		/// <summary>Gets the NetBIOS computer name of the local computer.</summary>
		/// <returns>The name of this computer.</returns>
		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x000FA464 File Offset: 0x000F8664
		public static string ComputerName
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetComputerName(stringBuilder, new int[]
				{
					stringBuilder.Capacity
				});
				return stringBuilder.ToString();
			}
		}

		/// <summary>Gets the name of the domain the user belongs to.</summary>
		/// <returns>The name of the user domain. If a local user account exists with the same name as the user name, this property gets the computer name.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The operating system does not support this feature.</exception>
		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000FA4A2 File Offset: 0x000F86A2
		public static string UserDomainName
		{
			get
			{
				return Environment.UserDomainName;
			}
		}

		/// <summary>Gets a value indicating whether the current process is running in user-interactive mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the current process is running in user-interactive mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x0600377A RID: 14202 RVA: 0x000FA4AC File Offset: 0x000F86AC
		public static bool UserInteractive
		{
			get
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = UnsafeNativeMethods.GetProcessWindowStation();
					if (intPtr != IntPtr.Zero && SystemInformation.processWinStation != intPtr)
					{
						SystemInformation.isUserInteractive = true;
						int num = 0;
						NativeMethods.USEROBJECTFLAGS userobjectflags = new NativeMethods.USEROBJECTFLAGS();
						if (UnsafeNativeMethods.GetUserObjectInformation(new HandleRef(null, intPtr), 1, userobjectflags, Marshal.SizeOf(userobjectflags), ref num) && (userobjectflags.dwFlags & 1) == 0)
						{
							SystemInformation.isUserInteractive = false;
						}
						SystemInformation.processWinStation = intPtr;
					}
				}
				else
				{
					SystemInformation.isUserInteractive = true;
				}
				return SystemInformation.isUserInteractive;
			}
		}

		/// <summary>Gets the user name associated with the current thread.</summary>
		/// <returns>The user name of the user associated with the current thread.</returns>
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x0600377B RID: 14203 RVA: 0x000FA538 File Offset: 0x000F8738
		public static string UserName
		{
			get
			{
				IntSecurity.SensitiveSystemInformation.Demand();
				StringBuilder stringBuilder = new StringBuilder(256);
				UnsafeNativeMethods.GetUserName(stringBuilder, new int[]
				{
					stringBuilder.Capacity
				});
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x000FA576 File Offset: 0x000F8776
		private static void EnsureSystemEvents()
		{
			if (!SystemInformation.systemEventsAttached)
			{
				SystemEvents.UserPreferenceChanged += SystemInformation.OnUserPreferenceChanged;
				SystemInformation.systemEventsAttached = true;
			}
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000FA596 File Offset: 0x000F8796
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs pref)
		{
			SystemInformation.systemEventsDirty = true;
		}

		/// <summary>Gets a value indicating whether the drop shadow effect is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the drop shadow effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x000FA5A0 File Offset: 0x000F87A0
		public static bool IsDropShadowEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4132, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether native user menus have a flat menu appearance. </summary>
		/// <returns>This property is not used and always returns <see langword="false" />.</returns>
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x0600377F RID: 14207 RVA: 0x000FA5D0 File Offset: 0x000F87D0
		public static bool IsFlatMenuEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4130, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether font smoothing is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the font smoothing feature is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x000FA600 File Offset: 0x000F8800
		public static bool IsFontSmoothingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(74, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the font smoothing contrast value used in ClearType smoothing.</summary>
		/// <returns>The ClearType font smoothing contrast value.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system does not support this feature.</exception>
		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003781 RID: 14209 RVA: 0x000FA620 File Offset: 0x000F8820
		public static int FontSmoothingContrast
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8204, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		/// <summary>Gets the current type of font smoothing.</summary>
		/// <returns>A value that indicates the current type of font smoothing.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system does not support this feature.</exception>
		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x000FA65C File Offset: 0x000F885C
		public static int FontSmoothingType
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8202, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		/// <summary>Gets the width, in pixels, of an icon arrangement cell in large icon view.</summary>
		/// <returns>The width, in pixels, of an icon arrangement cell in large icon view.</returns>
		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x000FA698 File Offset: 0x000F8898
		public static int IconHorizontalSpacing
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(13, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets the height, in pixels, of an icon arrangement cell in large icon view.</summary>
		/// <returns>The height, in pixels, of an icon arrangement cell in large icon view.</returns>
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003784 RID: 14212 RVA: 0x000FA6B4 File Offset: 0x000F88B4
		public static int IconVerticalSpacing
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(24, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether icon-title wrapping is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the icon-title wrapping feature is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003785 RID: 14213 RVA: 0x000FA6D0 File Offset: 0x000F88D0
		public static bool IsIconTitleWrappingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(25, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether menu access keys are always underlined.</summary>
		/// <returns>
		///     <see langword="true" /> if menu access keys are always underlined; <see langword="false" /> if they are underlined only when the menu is activated or receives focus.</returns>
		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x000FA6F0 File Offset: 0x000F88F0
		public static bool MenuAccessKeysUnderlined
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4106, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the keyboard repeat-delay setting.</summary>
		/// <returns>The keyboard repeat-delay setting, from 0 (approximately 250 millisecond delay) through 3 (approximately 1 second delay).</returns>
		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x000FA714 File Offset: 0x000F8914
		public static int KeyboardDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(22, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the user relies on the keyboard instead of the mouse, and prefers applications to display keyboard interfaces that would otherwise be hidden.</summary>
		/// <returns>
		///     <see langword="true" /> if keyboard preferred mode is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x000FA730 File Offset: 0x000F8930
		public static bool IsKeyboardPreferred
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(68, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the keyboard repeat-speed setting.</summary>
		/// <returns>The keyboard repeat-speed setting, from 0 (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second).</returns>
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003789 RID: 14217 RVA: 0x000FA750 File Offset: 0x000F8950
		public static int KeyboardSpeed
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(10, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets the dimensions, in pixels, of the rectangle within which the mouse pointer has to stay for the mouse hover time before a mouse hover message is generated.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the dimensions, in pixels, of the rectangle within which the mouse pointer has to stay for the mouse hover time before a mouse hover message is generated.</returns>
		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x0600378A RID: 14218 RVA: 0x000FA76C File Offset: 0x000F896C
		public static Size MouseHoverSize
		{
			get
			{
				int height = 0;
				int width = 0;
				UnsafeNativeMethods.SystemParametersInfo(100, 0, ref height, 0);
				UnsafeNativeMethods.SystemParametersInfo(98, 0, ref width, 0);
				return new Size(width, height);
			}
		}

		/// <summary>Gets the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle before a mouse hover message is generated.</summary>
		/// <returns>The time, in milliseconds, that the mouse pointer has to stay in the hover rectangle before a mouse hover message is generated.</returns>
		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x0600378B RID: 14219 RVA: 0x000FA79C File Offset: 0x000F899C
		public static int MouseHoverTime
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(102, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets the current mouse speed.</summary>
		/// <returns>A mouse speed value between 1 (slowest) and 20 (fastest).</returns>
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x000FA7B8 File Offset: 0x000F89B8
		public static int MouseSpeed
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(112, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the snap-to-default-button feature is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the snap-to-default-button feature is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600378D RID: 14221 RVA: 0x000FA7D4 File Offset: 0x000F89D4
		public static bool IsSnapToDefaultEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(95, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the side of pop-up menus that are aligned to the corresponding menu-bar item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.LeftRightAlignment" /> that indicates whether pop-up menus are left-aligned or right-aligned, relative to the corresponding menu-bar item.</returns>
		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x0600378E RID: 14222 RVA: 0x000FA7F4 File Offset: 0x000F89F4
		public static LeftRightAlignment PopupMenuAlignment
		{
			get
			{
				bool flag = false;
				UnsafeNativeMethods.SystemParametersInfo(27, 0, ref flag, 0);
				if (flag)
				{
					return LeftRightAlignment.Left;
				}
				return LeftRightAlignment.Right;
			}
		}

		/// <summary>Gets a value indicating whether menu fade animation is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if fade animation is enabled; <see langword="false" /> if it is disabled.</returns>
		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x0600378F RID: 14223 RVA: 0x000FA818 File Offset: 0x000F8A18
		public static bool IsMenuFadeEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4114, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets the time, in milliseconds, that the system waits before displaying a cascaded shortcut menu when the mouse cursor is over a submenu item.</summary>
		/// <returns>The time, in milliseconds, that the system waits before displaying a cascaded shortcut menu when the mouse cursor is over a submenu item.</returns>
		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x000FA854 File Offset: 0x000F8A54
		public static int MenuShowDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(106, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether the slide-open effect for combo boxes is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the slide-open effect for combo boxes is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x000FA870 File Offset: 0x000F8A70
		public static bool IsComboBoxAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4100, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether the gradient effect for window title bars is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the gradient effect for window title bars is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x000FA894 File Offset: 0x000F8A94
		public static bool IsTitleBarGradientEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4104, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether hot tracking of user-interface elements, such as menu names on menu bars, is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if hot tracking of user-interface elements is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000FA8B8 File Offset: 0x000F8AB8
		public static bool IsHotTrackingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4110, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether the smooth-scrolling effect for list boxes is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if smooth-scrolling is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06003794 RID: 14228 RVA: 0x000FA8DC File Offset: 0x000F8ADC
		public static bool IsListBoxSmoothScrollingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4102, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether menu fade or slide animation features are enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if menu fade or slide animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x000FA900 File Offset: 0x000F8B00
		public static bool IsMenuAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4098, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets a value indicating whether the selection fade effect is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the selection fade effect is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x000FA924 File Offset: 0x000F8B24
		public static bool IsSelectionFadeEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4116, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether <see cref="T:System.Windows.Forms.ToolTip" /> animation is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Forms.ToolTip" /> animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000FA960 File Offset: 0x000F8B60
		public static bool IsToolTipAnimationEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4118, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether user interface (UI) effects are enabled or disabled.</summary>
		/// <returns>
		///     <see langword="true" /> if UI effects are enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003798 RID: 14232 RVA: 0x000FA99C File Offset: 0x000F8B9C
		public static bool UIEffectsEnabled
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int num = 0;
					UnsafeNativeMethods.SystemParametersInfo(4158, 0, ref num, 0);
					return num != 0;
				}
				return false;
			}
		}

		/// <summary>Gets a value indicating whether active window tracking is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if active window tracking is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x000FA9D8 File Offset: 0x000F8BD8
		public static bool IsActiveWindowTrackingEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(4096, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the active window tracking delay.</summary>
		/// <returns>The active window tracking delay, in milliseconds.</returns>
		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x0600379A RID: 14234 RVA: 0x000FA9FC File Offset: 0x000F8BFC
		public static int ActiveWindowTrackingDelay
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(8194, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets a value indicating whether window minimize and restore animation is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if window minimize and restore animation is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x000FAA1C File Offset: 0x000F8C1C
		public static bool IsMinimizeRestoreAnimationEnabled
		{
			get
			{
				int num = 0;
				UnsafeNativeMethods.SystemParametersInfo(72, 0, ref num, 0);
				return num != 0;
			}
		}

		/// <summary>Gets the border multiplier factor that is used when determining the thickness of a window's sizing border.</summary>
		/// <returns>The multiplier used to determine the thickness of a window's sizing border.</returns>
		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x0600379C RID: 14236 RVA: 0x000FAA3C File Offset: 0x000F8C3C
		public static int BorderMultiplierFactor
		{
			get
			{
				int result = 0;
				UnsafeNativeMethods.SystemParametersInfo(5, 0, ref result, 0);
				return result;
			}
		}

		/// <summary>Gets the caret blink time.</summary>
		/// <returns>The caret blink time.</returns>
		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x0600379D RID: 14237 RVA: 0x000FAA57 File Offset: 0x000F8C57
		public static int CaretBlinkTime
		{
			get
			{
				return (int)SafeNativeMethods.GetCaretBlinkTime();
			}
		}

		/// <summary>Gets the width, in pixels, of the caret in edit controls.</summary>
		/// <returns>The width, in pixels, of the caret in edit controls.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system does not support this feature.</exception>
		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x0600379E RID: 14238 RVA: 0x000FAA60 File Offset: 0x000F8C60
		public static int CaretWidth
		{
			get
			{
				if (OSFeature.Feature.OnXp || OSFeature.Feature.OnWin2k)
				{
					int result = 0;
					UnsafeNativeMethods.SystemParametersInfo(8198, 0, ref result, 0);
					return result;
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		/// <summary>Gets the amount of the delta value of a single mouse wheel rotation increment.</summary>
		/// <returns>The amount of the delta value of a single mouse wheel rotation increment.</returns>
		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x0600379F RID: 14239 RVA: 0x000FAAA7 File Offset: 0x000F8CA7
		public static int MouseWheelScrollDelta
		{
			get
			{
				return 120;
			}
		}

		/// <summary>Gets the thickness, in pixels, of the top and bottom edges of the system focus rectangle.</summary>
		/// <returns>The thickness, in pixels, of the top and bottom edges of the system focus rectangle.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system does not support this feature.</exception>
		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x000FAAAB File Offset: 0x000F8CAB
		public static int VerticalFocusThickness
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					return UnsafeNativeMethods.GetSystemMetrics(84);
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		/// <summary>Gets the thickness of the left and right edges of the system focus rectangle, in pixels.</summary>
		/// <returns>The thickness of the left and right edges of the system focus rectangle, in pixels.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system does not support this feature.</exception>
		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x000FAAD0 File Offset: 0x000F8CD0
		public static int HorizontalFocusThickness
		{
			get
			{
				if (OSFeature.Feature.OnXp)
				{
					return UnsafeNativeMethods.GetSystemMetrics(83);
				}
				throw new NotSupportedException(SR.GetString("SystemInformationFeatureNotSupported"));
			}
		}

		/// <summary>Gets the thickness, in pixels, of the top and bottom edges of the sizing border around the perimeter of a window being resized.</summary>
		/// <returns>The height, in pixels, of the top and bottom edges of the sizing border around the perimeter of a window being resized, in pixels.</returns>
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060037A2 RID: 14242 RVA: 0x000FAAF5 File Offset: 0x000F8CF5
		public static int VerticalResizeBorderThickness
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(33);
			}
		}

		/// <summary>Gets the thickness of the left and right edges of the sizing border around the perimeter of a window being resized, in pixels.</summary>
		/// <returns>The width of the left and right edges of the sizing border around the perimeter of a window being resized, in pixels.</returns>
		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x000FAAFE File Offset: 0x000F8CFE
		public static int HorizontalResizeBorderThickness
		{
			get
			{
				return UnsafeNativeMethods.GetSystemMetrics(32);
			}
		}

		/// <summary>Gets the orientation of the screen.</summary>
		/// <returns>The orientation of the screen, in degrees.</returns>
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060037A4 RID: 14244 RVA: 0x000FAB08 File Offset: 0x000F8D08
		public static ScreenOrientation ScreenOrientation
		{
			get
			{
				ScreenOrientation result = ScreenOrientation.Angle0;
				NativeMethods.DEVMODE devmode = default(NativeMethods.DEVMODE);
				devmode.dmSize = (short)Marshal.SizeOf(typeof(NativeMethods.DEVMODE));
				devmode.dmDriverExtra = 0;
				try
				{
					SafeNativeMethods.EnumDisplaySettings(null, -1, ref devmode);
					if ((devmode.dmFields & 128) > 0)
					{
						result = devmode.dmDisplayOrientation;
					}
				}
				catch
				{
				}
				return result;
			}
		}

		/// <summary>Gets the width, in pixels, of the sizing border drawn around the perimeter of a window being resized.</summary>
		/// <returns>The width, in pixels, of the window sizing border drawn around the perimeter of a window being resized.</returns>
		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x000FAB74 File Offset: 0x000F8D74
		public static int SizingBorderWidth
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iBorderWidth > 0)
				{
					return nonclientmetrics.iBorderWidth;
				}
				return 0;
			}
		}

		/// <summary>Gets the width, in pixels, of small caption buttons, and the height, in pixels, of small captions.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the width, in pixels, of small caption buttons, and the height, in pixels, of small captions.</returns>
		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x000FABAC File Offset: 0x000F8DAC
		public static Size SmallCaptionButtonSize
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iSmCaptionHeight > 0 && nonclientmetrics.iSmCaptionWidth > 0)
				{
					return new Size(nonclientmetrics.iSmCaptionWidth, nonclientmetrics.iSmCaptionHeight);
				}
				return Size.Empty;
			}
		}

		/// <summary>Gets the default width, in pixels, for menu-bar buttons and the height, in pixels, of a menu bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that indicates the default width for menu-bar buttons, in pixels, and the height of a menu bar, in pixels.</returns>
		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x000FABFC File Offset: 0x000F8DFC
		public static Size MenuBarButtonSize
		{
			get
			{
				NativeMethods.NONCLIENTMETRICS nonclientmetrics = new NativeMethods.NONCLIENTMETRICS();
				bool flag = UnsafeNativeMethods.SystemParametersInfo(41, nonclientmetrics.cbSize, nonclientmetrics, 0);
				if (flag && nonclientmetrics.iMenuHeight > 0 && nonclientmetrics.iMenuWidth > 0)
				{
					return new Size(nonclientmetrics.iMenuWidth, nonclientmetrics.iMenuHeight);
				}
				return Size.Empty;
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000FAC4C File Offset: 0x000F8E4C
		internal static bool InLockedTerminalSession()
		{
			bool result = false;
			if (SystemInformation.TerminalServerSession)
			{
				IntPtr intPtr = SafeNativeMethods.OpenInputDesktop(0, false, 256);
				if (intPtr == IntPtr.Zero)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					result = (lastWin32Error == 5);
				}
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.CloseDesktop(intPtr);
				}
			}
			return result;
		}

		// Token: 0x04002207 RID: 8711
		private static bool checkMultiMonitorSupport = false;

		// Token: 0x04002208 RID: 8712
		private static bool multiMonitorSupport = false;

		// Token: 0x04002209 RID: 8713
		private static bool checkNativeMouseWheelSupport = false;

		// Token: 0x0400220A RID: 8714
		private static bool nativeMouseWheelSupport = true;

		// Token: 0x0400220B RID: 8715
		private static bool highContrast = false;

		// Token: 0x0400220C RID: 8716
		private static bool systemEventsAttached = false;

		// Token: 0x0400220D RID: 8717
		private static bool systemEventsDirty = true;

		// Token: 0x0400220E RID: 8718
		private static IntPtr processWinStation = IntPtr.Zero;

		// Token: 0x0400220F RID: 8719
		private static bool isUserInteractive = false;

		// Token: 0x04002210 RID: 8720
		private static PowerStatus powerStatus = null;

		// Token: 0x04002211 RID: 8721
		private const int DefaultMouseWheelScrollLines = 3;
	}
}
