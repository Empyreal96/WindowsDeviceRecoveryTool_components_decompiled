using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using MS.Internal.WindowsBase;

namespace Standard
{
	// Token: 0x02000068 RID: 104
	internal static class NativeMethods
	{
		// Token: 0x06000098 RID: 152
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _AdjustWindowRectEx(ref RECT lpRect, WS dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu, WS_EX dwExStyle);

		// Token: 0x06000099 RID: 153 RVA: 0x000035EE File Offset: 0x000017EE
		[SecurityCritical]
		public static RECT AdjustWindowRectEx(RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle)
		{
			if (!NativeMethods._AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		// Token: 0x0600009A RID: 154
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilter", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilter(WM message, MSGFLT dwFlag);

		// Token: 0x0600009B RID: 155
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilterEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, [In] [Out] [Optional] ref CHANGEFILTERSTRUCT pChangeFilterStruct);

		// Token: 0x0600009C RID: 156 RVA: 0x00003604 File Offset: 0x00001804
		[SecurityCritical]
		public static HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, out MSGFLTINFO filterInfo)
		{
			filterInfo = MSGFLTINFO.NONE;
			if (!Utility.IsOSVistaOrNewer)
			{
				return HRESULT.S_FALSE;
			}
			if (!Utility.IsOSWindows7OrNewer)
			{
				if (!NativeMethods._ChangeWindowMessageFilter(message, action))
				{
					return (HRESULT)Win32Error.GetLastError();
				}
				return HRESULT.S_OK;
			}
			else
			{
				CHANGEFILTERSTRUCT changefilterstruct = new CHANGEFILTERSTRUCT
				{
					cbSize = (uint)Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT))
				};
				if (!NativeMethods._ChangeWindowMessageFilterEx(hwnd, message, action, ref changefilterstruct))
				{
					return (HRESULT)Win32Error.GetLastError();
				}
				filterInfo = changefilterstruct.ExtStatus;
				return HRESULT.S_OK;
			}
		}

		// Token: 0x0600009D RID: 157
		[SecurityCritical]
		[DllImport("gdi32.dll")]
		public static extern CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, RGN fnCombineMode);

		// Token: 0x0600009E RID: 158
		[SecurityCritical]
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "CommandLineToArgvW")]
		private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);

		// Token: 0x0600009F RID: 159 RVA: 0x0000368C File Offset: 0x0000188C
		[SecurityCritical]
		public static string[] CommandLineToArgvW(string cmdLine)
		{
			IntPtr intPtr = IntPtr.Zero;
			string[] result;
			try
			{
				int num = 0;
				intPtr = NativeMethods._CommandLineToArgvW(cmdLine, out num);
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					IntPtr ptr = Marshal.ReadIntPtr(intPtr, i * Marshal.SizeOf(typeof(IntPtr)));
					array[i] = Marshal.PtrToStringUni(ptr);
				}
				result = array;
			}
			finally
			{
				IntPtr intPtr2 = NativeMethods._LocalFree(intPtr);
			}
			return result;
		}

		// Token: 0x060000A0 RID: 160
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSection(SafeDC hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x060000A1 RID: 161
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSectionIntPtr(IntPtr hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x060000A2 RID: 162 RVA: 0x00003714 File Offset: 0x00001914
		[SecurityCritical]
		public static SafeHBITMAP CreateDIBSection(SafeDC hdc, ref BITMAPINFO bitmapInfo, out IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			SafeHBITMAP safeHBITMAP;
			if (hdc == null)
			{
				safeHBITMAP = NativeMethods._CreateDIBSectionIntPtr(IntPtr.Zero, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
			}
			else
			{
				safeHBITMAP = NativeMethods._CreateDIBSection(hdc, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset);
			}
			if (safeHBITMAP.IsInvalid)
			{
				HRESULT.ThrowLastError();
			}
			return safeHBITMAP;
		}

		// Token: 0x060000A3 RID: 163
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		// Token: 0x060000A4 RID: 164 RVA: 0x00003754 File Offset: 0x00001954
		[SecurityCritical]
		public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
		{
			IntPtr intPtr = NativeMethods._CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x060000A5 RID: 165
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		// Token: 0x060000A6 RID: 166 RVA: 0x00003784 File Offset: 0x00001984
		[SecurityCritical]
		public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			IntPtr intPtr = NativeMethods._CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x060000A7 RID: 167
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgnIndirect", SetLastError = true)]
		private static extern IntPtr _CreateRectRgnIndirect([In] ref RECT lprc);

		// Token: 0x060000A8 RID: 168 RVA: 0x000037B0 File Offset: 0x000019B0
		[SecurityCritical]
		public static IntPtr CreateRectRgnIndirect(RECT lprc)
		{
			IntPtr intPtr = NativeMethods._CreateRectRgnIndirect(ref lprc);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x060000A9 RID: 169
		[SecurityCritical]
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(int crColor);

		// Token: 0x060000AA RID: 170
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", SetLastError = true)]
		private static extern IntPtr _CreateWindowEx(WS_EX dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		// Token: 0x060000AB RID: 171 RVA: 0x000037DC File Offset: 0x000019DC
		[SecurityCritical]
		public static IntPtr CreateWindowEx(WS_EX dwExStyle, string lpClassName, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
		{
			IntPtr intPtr = NativeMethods._CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
			if (IntPtr.Zero == intPtr)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x060000AC RID: 172
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x060000AD RID: 173
		[SecurityCritical]
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		// Token: 0x060000AE RID: 174
		[SecurityCritical]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(IntPtr handle);

		// Token: 0x060000AF RID: 175
		[SecurityCritical]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		// Token: 0x060000B0 RID: 176
		[SecurityCritical]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindow(IntPtr hwnd);

		// Token: 0x060000B1 RID: 177
		[SecurityCritical]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

		// Token: 0x060000B2 RID: 178
		[SecurityCritical]
		[DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DwmIsCompositionEnabled();

		// Token: 0x060000B3 RID: 179
		[SecurityCritical]
		[DllImport("dwmapi.dll", EntryPoint = "DwmGetColorizationColor")]
		private static extern HRESULT _DwmGetColorizationColor(out uint pcrColorization, [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

		// Token: 0x060000B4 RID: 180 RVA: 0x00003818 File Offset: 0x00001A18
		[SecurityCritical]
		public static bool DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend)
		{
			if (Utility.IsOSVistaOrNewer && NativeMethods.IsThemeActive() && NativeMethods._DwmGetColorizationColor(out pcrColorization, out pfOpaqueBlend).Succeeded)
			{
				return true;
			}
			pcrColorization = 4278190080U;
			pfOpaqueBlend = true;
			return false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003851 File Offset: 0x00001A51
		[SecurityCritical]
		public static bool DwmIsCompositionEnabled()
		{
			return Utility.IsOSVistaOrNewer && NativeMethods._DwmIsCompositionEnabled();
		}

		// Token: 0x060000B6 RID: 182
		[SecurityCritical]
		[DllImport("dwmapi.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DwmDefWindowProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

		// Token: 0x060000B7 RID: 183
		[SecurityCritical]
		[DllImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
		private static extern void _DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);

		// Token: 0x060000B8 RID: 184 RVA: 0x00003864 File Offset: 0x00001A64
		[SecurityCritical]
		public static void DwmSetWindowAttributeFlip3DPolicy(IntPtr hwnd, DWMFLIP3D flip3dPolicy)
		{
			int num = (int)flip3dPolicy;
			NativeMethods._DwmSetWindowAttribute(hwnd, DWMWA.FLIP3D_POLICY, ref num, 4);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003880 File Offset: 0x00001A80
		[SecurityCritical]
		public static void DwmSetWindowAttributeDisallowPeek(IntPtr hwnd, bool disallowPeek)
		{
			int num = disallowPeek ? 1 : 0;
			NativeMethods._DwmSetWindowAttribute(hwnd, DWMWA.DISALLOW_PEEK, ref num, 4);
		}

		// Token: 0x060000BA RID: 186
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "EnableMenuItem")]
		private static extern int _EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable);

		// Token: 0x060000BB RID: 187 RVA: 0x000038A0 File Offset: 0x00001AA0
		[SecurityCritical]
		public static MF EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable)
		{
			return (MF)NativeMethods._EnableMenuItem(hMenu, uIDEnableItem, uEnable);
		}

		// Token: 0x060000BC RID: 188
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "RemoveMenu", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

		// Token: 0x060000BD RID: 189 RVA: 0x000038B7 File Offset: 0x00001AB7
		[SecurityCritical]
		public static void RemoveMenu(IntPtr hMenu, SC uPosition, MF uFlags)
		{
			if (!NativeMethods._RemoveMenu(hMenu, (uint)uPosition, (uint)uFlags))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060000BE RID: 190
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "DrawMenuBar", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DrawMenuBar(IntPtr hWnd);

		// Token: 0x060000BF RID: 191 RVA: 0x000038C9 File Offset: 0x00001AC9
		[SecurityCritical]
		public static void DrawMenuBar(IntPtr hWnd)
		{
			if (!NativeMethods._DrawMenuBar(hWnd))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060000C0 RID: 192
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindClose(IntPtr handle);

		// Token: 0x060000C1 RID: 193
		[SecurityCritical]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFindHandle FindFirstFileW(string lpFileName, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] WIN32_FIND_DATAW lpFindFileData);

		// Token: 0x060000C2 RID: 194
		[SecurityCritical]
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindNextFileW(SafeFindHandle hndFindFile, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] WIN32_FIND_DATAW lpFindFileData);

		// Token: 0x060000C3 RID: 195
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetClientRect(IntPtr hwnd, out RECT lpRect);

		// Token: 0x060000C4 RID: 196 RVA: 0x000038DC File Offset: 0x00001ADC
		[SecurityCritical]
		public static RECT GetClientRect(IntPtr hwnd)
		{
			RECT result;
			if (!NativeMethods._GetClientRect(hwnd, out result))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x060000C5 RID: 197
		[SecurityCritical]
		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "GetCurrentThemeName")]
		private static extern HRESULT _GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		// Token: 0x060000C6 RID: 198 RVA: 0x000038FC File Offset: 0x00001AFC
		[SecurityCritical]
		public static void GetCurrentThemeName(out string themeFileName, out string color, out string size)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			StringBuilder stringBuilder3 = new StringBuilder(260);
			NativeMethods._GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity, stringBuilder3, stringBuilder3.Capacity).ThrowIfFailed();
			themeFileName = stringBuilder.ToString();
			color = stringBuilder2.ToString();
			size = stringBuilder3.ToString();
		}

		// Token: 0x060000C7 RID: 199
		[SecurityCritical]
		[DllImport("uxtheme.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsThemeActive();

		// Token: 0x060000C8 RID: 200
		[SecurityCritical]
		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(SafeDC hdc, DeviceCap nIndex);

		// Token: 0x060000C9 RID: 201
		[SecurityCritical]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleFileName", SetLastError = true)]
		private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

		// Token: 0x060000CA RID: 202 RVA: 0x00003964 File Offset: 0x00001B64
		[SecurityCritical]
		public static string GetModuleFileName(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			for (;;)
			{
				int num = NativeMethods._GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity);
				if (num == 0)
				{
					HRESULT.ThrowLastError();
				}
				if (num != stringBuilder.Capacity)
				{
					break;
				}
				stringBuilder.EnsureCapacity(stringBuilder.Capacity * 2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000CB RID: 203
		[SecurityCritical]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", SetLastError = true)]
		private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

		// Token: 0x060000CC RID: 204 RVA: 0x000039B4 File Offset: 0x00001BB4
		[SecurityCritical]
		public static IntPtr GetModuleHandle(string lpModuleName)
		{
			IntPtr intPtr = NativeMethods._GetModuleHandle(lpModuleName);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x060000CD RID: 205
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "GetMonitorInfo", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In] [Out] MONITORINFO lpmi);

		// Token: 0x060000CE RID: 206 RVA: 0x000039DC File Offset: 0x00001BDC
		[SecurityCritical]
		public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
		{
			MONITORINFO monitorinfo = new MONITORINFO();
			if (!NativeMethods._GetMonitorInfo(hMonitor, monitorinfo))
			{
				throw new Win32Exception();
			}
			return monitorinfo;
		}

		// Token: 0x060000CF RID: 207
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "GetStockObject", SetLastError = true)]
		private static extern IntPtr _GetStockObject(StockObject fnObject);

		// Token: 0x060000D0 RID: 208 RVA: 0x00003A00 File Offset: 0x00001C00
		[SecurityCritical]
		public static IntPtr GetStockObject(StockObject fnObject)
		{
			return NativeMethods._GetStockObject(fnObject);
		}

		// Token: 0x060000D1 RID: 209
		[SecurityCritical]
		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

		// Token: 0x060000D2 RID: 210
		[SecurityCritical]
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(SM nIndex);

		// Token: 0x060000D3 RID: 211 RVA: 0x00003A18 File Offset: 0x00001C18
		[SecurityCritical]
		public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (8 == IntPtr.Size)
			{
				intPtr = NativeMethodsSetLastError.GetWindowLongPtr(hwnd, (int)nIndex);
			}
			else
			{
				intPtr = new IntPtr(NativeMethodsSetLastError.GetWindowLong(hwnd, (int)nIndex));
			}
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x060000D4 RID: 212
		[SecurityCritical]
		[DllImport("uxtheme.dll", PreserveSig = false)]
		public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

		// Token: 0x060000D5 RID: 213
		[SecurityCritical]
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hwnd, WINDOWPLACEMENT lpwndpl);

		// Token: 0x060000D6 RID: 214 RVA: 0x00003A60 File Offset: 0x00001C60
		[SecurityCritical]
		public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT windowplacement = new WINDOWPLACEMENT();
			if (NativeMethods.GetWindowPlacement(hwnd, windowplacement))
			{
				return windowplacement;
			}
			throw new Win32Exception();
		}

		// Token: 0x060000D7 RID: 215
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetWindowRect(IntPtr hWnd, out RECT lpRect);

		// Token: 0x060000D8 RID: 216 RVA: 0x00003A84 File Offset: 0x00001C84
		[SecurityCritical]
		public static RECT GetWindowRect(IntPtr hwnd)
		{
			RECT result;
			if (!NativeMethods._GetWindowRect(hwnd, out result))
			{
				HRESULT.ThrowLastError();
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);

		// Token: 0x060000DA RID: 218
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHBITMAPFromBitmap(IntPtr bitmap, out IntPtr hbmReturn, int background);

		// Token: 0x060000DB RID: 219
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHICONFromBitmap(IntPtr bitmap, out IntPtr hbmReturn);

		// Token: 0x060000DC RID: 220
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipDisposeImage(IntPtr image);

		// Token: 0x060000DD RID: 221
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdipImageForceValidation(IntPtr image);

		// Token: 0x060000DE RID: 222
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusStartup(out IntPtr token, StartupInput input, out StartupOutput output);

		// Token: 0x060000DF RID: 223
		[SecurityCritical]
		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusShutdown(IntPtr token);

		// Token: 0x060000E0 RID: 224
		[SecurityCritical]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		// Token: 0x060000E1 RID: 225
		[SecurityCritical]
		[DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
		private static extern IntPtr _LocalFree(IntPtr hMem);

		// Token: 0x060000E2 RID: 226
		[SecurityCritical]
		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		// Token: 0x060000E3 RID: 227
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x060000E4 RID: 228 RVA: 0x00003AA1 File Offset: 0x00001CA1
		[SecurityCritical]
		public static void PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!NativeMethods._PostMessage(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060000E5 RID: 229
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true)]
		private static extern short _RegisterClassEx([In] ref WNDCLASSEX lpwcx);

		// Token: 0x060000E6 RID: 230 RVA: 0x00003AB4 File Offset: 0x00001CB4
		[SecurityCritical]
		public static short RegisterClassEx(ref WNDCLASSEX lpwcx)
		{
			short num = NativeMethods._RegisterClassEx(ref lpwcx);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		// Token: 0x060000E7 RID: 231
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegisterWindowMessage", SetLastError = true)]
		private static extern uint _RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

		// Token: 0x060000E8 RID: 232 RVA: 0x00003AD4 File Offset: 0x00001CD4
		[SecurityCritical]
		public static WM RegisterWindowMessage(string lpString)
		{
			uint num = NativeMethods._RegisterWindowMessage(lpString);
			if (num == 0U)
			{
				HRESULT.ThrowLastError();
			}
			return (WM)num;
		}

		// Token: 0x060000E9 RID: 233
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SetActiveWindow", SetLastError = true)]
		private static extern IntPtr _SetActiveWindow(IntPtr hWnd);

		// Token: 0x060000EA RID: 234 RVA: 0x00003AF4 File Offset: 0x00001CF4
		[SecurityCritical]
		public static IntPtr SetActiveWindow(IntPtr hwnd)
		{
			Verify.IsNotDefault<IntPtr>(hwnd, "hwnd");
			IntPtr intPtr = NativeMethods._SetActiveWindow(hwnd);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003B26 File Offset: 0x00001D26
		[SecurityCritical]
		public static IntPtr SetClassLongPtr(IntPtr hwnd, GCLP nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return NativeMethods.SetClassLongPtr64(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(NativeMethods.SetClassLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		// Token: 0x060000EC RID: 236
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SetClassLong", SetLastError = true)]
		private static extern int SetClassLongPtr32(IntPtr hWnd, GCLP nIndex, int dwNewLong);

		// Token: 0x060000ED RID: 237
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SetClassLongPtr", SetLastError = true)]
		private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, GCLP nIndex, IntPtr dwNewLong);

		// Token: 0x060000EE RID: 238
		[SecurityCritical]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern ErrorModes SetErrorMode(ErrorModes newMode);

		// Token: 0x060000EF RID: 239
		[SecurityCritical]
		[DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimiumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

		// Token: 0x060000F0 RID: 240 RVA: 0x00003B4C File Offset: 0x00001D4C
		[SecurityCritical]
		public static void SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize)
		{
			if (!NativeMethods._SetProcessWorkingSetSize(hProcess, new IntPtr(dwMinimumWorkingSetSize), new IntPtr(dwMaximumWorkingSetSize)))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00003B68 File Offset: 0x00001D68
		[SecurityCritical]
		public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return NativeMethodsSetLastError.SetWindowLongPtr(hwnd, (int)nIndex, dwNewLong);
			}
			return new IntPtr(NativeMethodsSetLastError.SetWindowLong(hwnd, (int)nIndex, dwNewLong.ToInt32()));
		}

		// Token: 0x060000F2 RID: 242
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SetWindowRgn", SetLastError = true)]
		private static extern int _SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

		// Token: 0x060000F3 RID: 243 RVA: 0x00003B90 File Offset: 0x00001D90
		[SecurityCritical]
		public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
		{
			if (NativeMethods._SetWindowRgn(hWnd, hRgn, bRedraw) == 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060000F4 RID: 244
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

		// Token: 0x060000F5 RID: 245 RVA: 0x00003BAF File Offset: 0x00001DAF
		[SecurityCritical]
		public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
		{
			return NativeMethods._SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);
		}

		// Token: 0x060000F6 RID: 246
		[SecurityCritical]
		[DllImport("shell32.dll")]
		public static extern Win32Error SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

		// Token: 0x060000F7 RID: 247
		[SecurityCritical]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

		// Token: 0x060000F8 RID: 248
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_String(SPI uiAction, int uiParam, [MarshalAs(UnmanagedType.LPWStr)] string pvParam, SPIF fWinIni);

		// Token: 0x060000F9 RID: 249
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_NONCLIENTMETRICS(SPI uiAction, int uiParam, [In] [Out] ref NONCLIENTMETRICS pvParam, SPIF fWinIni);

		// Token: 0x060000FA RID: 250
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_HIGHCONTRAST(SPI uiAction, int uiParam, [In] [Out] ref HIGHCONTRAST pvParam, SPIF fWinIni);

		// Token: 0x060000FB RID: 251 RVA: 0x00003BC5 File Offset: 0x00001DC5
		[SecurityCritical]
		public static void SystemParametersInfo(SPI uiAction, int uiParam, string pvParam, SPIF fWinIni)
		{
			if (!NativeMethods._SystemParametersInfo_String(uiAction, uiParam, pvParam, fWinIni))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00003BD8 File Offset: 0x00001DD8
		[SecurityCritical]
		public static NONCLIENTMETRICS SystemParameterInfo_GetNONCLIENTMETRICS()
		{
			NONCLIENTMETRICS nonclientmetrics = Utility.IsOSVistaOrNewer ? NONCLIENTMETRICS.VistaMetricsStruct : NONCLIENTMETRICS.XPMetricsStruct;
			if (!NativeMethods._SystemParametersInfo_NONCLIENTMETRICS(SPI.GETNONCLIENTMETRICS, nonclientmetrics.cbSize, ref nonclientmetrics, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return nonclientmetrics;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003C14 File Offset: 0x00001E14
		[SecurityCritical]
		public static HIGHCONTRAST SystemParameterInfo_GetHIGHCONTRAST()
		{
			HIGHCONTRAST highcontrast = new HIGHCONTRAST
			{
				cbSize = Marshal.SizeOf(typeof(HIGHCONTRAST))
			};
			if (!NativeMethods._SystemParametersInfo_HIGHCONTRAST(SPI.GETHIGHCONTRAST, highcontrast.cbSize, ref highcontrast, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return highcontrast;
		}

		// Token: 0x060000FE RID: 254
		[SecurityCritical]
		[DllImport("user32.dll")]
		public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		// Token: 0x060000FF RID: 255
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObject(SafeDC hdc, IntPtr hgdiobj);

		// Token: 0x06000100 RID: 256 RVA: 0x00003C5C File Offset: 0x00001E5C
		[SecurityCritical]
		public static IntPtr SelectObject(SafeDC hdc, IntPtr hgdiobj)
		{
			IntPtr intPtr = NativeMethods._SelectObject(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x06000101 RID: 257
		[SecurityCritical]
		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObjectSafeHBITMAP(SafeDC hdc, SafeHBITMAP hgdiobj);

		// Token: 0x06000102 RID: 258 RVA: 0x00003C84 File Offset: 0x00001E84
		[SecurityCritical]
		public static IntPtr SelectObject(SafeDC hdc, SafeHBITMAP hgdiobj)
		{
			IntPtr intPtr = NativeMethods._SelectObjectSafeHBITMAP(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		// Token: 0x06000103 RID: 259
		[SecurityCritical]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

		// Token: 0x06000104 RID: 260
		[SecurityCritical]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000105 RID: 261
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassAtom(IntPtr lpClassName, IntPtr hInstance);

		// Token: 0x06000106 RID: 262
		[SecurityCritical]
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassName(string lpClassName, IntPtr hInstance);

		// Token: 0x06000107 RID: 263 RVA: 0x00003CAC File Offset: 0x00001EAC
		[SecurityCritical]
		public static void UnregisterClass(short atom, IntPtr hinstance)
		{
			if (!NativeMethods._UnregisterClassAtom(new IntPtr((int)atom), hinstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003CC1 File Offset: 0x00001EC1
		[SecurityCritical]
		public static void UnregisterClass(string lpClassName, IntPtr hInstance)
		{
			if (!NativeMethods._UnregisterClassName(lpClassName, hInstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x06000109 RID: 265
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, [In] ref POINT pptDst, [In] ref SIZE psize, SafeDC hdcSrc, [In] ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		// Token: 0x0600010A RID: 266
		[SecurityCritical]
		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindowIntPtr(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		// Token: 0x0600010B RID: 267 RVA: 0x00003CD4 File Offset: 0x00001ED4
		[SecurityCritical]
		public static void UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, ref POINT pptDst, ref SIZE psize, SafeDC hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!NativeMethods._UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00003CFC File Offset: 0x00001EFC
		[SecurityCritical]
		public static void UpdateLayeredWindow(IntPtr hwnd, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!NativeMethods._UpdateLayeredWindowIntPtr(hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		// Token: 0x0600010D RID: 269
		[SecurityCritical]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_String(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		// Token: 0x0600010E RID: 270
		[SecurityCritical]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		// Token: 0x0600010F RID: 271 RVA: 0x00003D32 File Offset: 0x00001F32
		[SecurityCritical]
		public static void SHAddToRecentDocs(string path)
		{
			NativeMethods._SHAddToRecentDocs_String(SHARD.PATHW, path);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D3B File Offset: 0x00001F3B
		[SecurityCritical]
		public static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			NativeMethods._SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		// Token: 0x06000111 RID: 273
		[SecurityCritical]
		[DllImport("dwmapi.dll", EntryPoint = "DwmGetCompositionTimingInfo")]
		private static extern HRESULT _DwmGetCompositionTimingInfo(IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo);

		// Token: 0x06000112 RID: 274 RVA: 0x00003D44 File Offset: 0x00001F44
		[SecurityCritical]
		public static DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return null;
			}
			DWM_TIMING_INFO value = new DWM_TIMING_INFO
			{
				cbSize = Marshal.SizeOf(typeof(DWM_TIMING_INFO))
			};
			HRESULT hrLeft = NativeMethods._DwmGetCompositionTimingInfo(hwnd, ref value);
			if (hrLeft == HRESULT.E_PENDING)
			{
				return null;
			}
			hrLeft.ThrowIfFailed();
			return new DWM_TIMING_INFO?(value);
		}

		// Token: 0x06000113 RID: 275
		[SecurityCritical]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmInvalidateIconicBitmaps(IntPtr hwnd);

		// Token: 0x06000114 RID: 276
		[SecurityCritical]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, DWM_SIT dwSITFlags);

		// Token: 0x06000115 RID: 277
		[SecurityCritical]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, RefPOINT pptClient, DWM_SIT dwSITFlags);

		// Token: 0x06000116 RID: 278
		[SecurityCritical]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06000117 RID: 279
		[SecurityCritical]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06000118 RID: 280
		[SecurityCritical]
		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool Shell_NotifyIcon(NIM dwMessage, [In] NOTIFYICONDATA lpdata);

		// Token: 0x06000119 RID: 281
		[SecurityCritical]
		[DllImport("shell32.dll", PreserveSig = false)]
		public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		// Token: 0x0600011A RID: 282
		[SecurityCritical]
		[DllImport("shell32.dll")]
		public static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
