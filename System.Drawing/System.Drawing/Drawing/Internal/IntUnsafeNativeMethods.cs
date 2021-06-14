using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Internal
{
	// Token: 0x020000E4 RID: 228
	[SuppressUnmanagedCodeSecurity]
	internal static class IntUnsafeNativeMethods
	{
		// Token: 0x06000BC6 RID: 3014
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002AD34 File Offset: 0x00028F34
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntGetDC(hWnd), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000BC8 RID: 3016
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002AD54 File Offset: 0x00028F54
		public static bool DeleteDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002AD7C File Offset: 0x00028F7C
		public static bool DeleteHDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000BCB RID: 3019
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002ADA2 File Offset: 0x00028FA2
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x06000BCD RID: 3021
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDC", SetLastError = true)]
		public static extern IntPtr IntCreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002ADBC File Offset: 0x00028FBC
		public static IntPtr CreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateDC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000BCF RID: 3023
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateIC", SetLastError = true)]
		public static extern IntPtr IntCreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002ADE0 File Offset: 0x00028FE0
		public static IntPtr CreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateIC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000BD1 RID: 3025
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002AE04 File Offset: 0x00029004
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateCompatibleDC(hDC), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BD3 RID: 3027
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SaveDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSaveDC(HandleRef hDC);

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002AE24 File Offset: 0x00029024
		public static int SaveDC(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntSaveDC(hDC);
		}

		// Token: 0x06000BD5 RID: 3029
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "RestoreDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRestoreDC(HandleRef hDC, int nSavedDC);

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002AE3C File Offset: 0x0002903C
		public static bool RestoreDC(HandleRef hDC, int nSavedDC)
		{
			return IntUnsafeNativeMethods.IntRestoreDC(hDC, nSavedDC);
		}

		// Token: 0x06000BD7 RID: 3031
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr WindowFromDC(HandleRef hDC);

		// Token: 0x06000BD8 RID: 3032
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x06000BD9 RID: 3033
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "OffsetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntOffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x06000BDA RID: 3034 RVA: 0x0002AE54 File Offset: 0x00029054
		public static bool OffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntOffsetViewportOrgEx(hDC, nXOffset, nYOffset, point);
		}

		// Token: 0x06000BDB RID: 3035
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetGraphicsMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetGraphicsMode(HandleRef hDC, int iMode);

		// Token: 0x06000BDC RID: 3036 RVA: 0x0002AE6C File Offset: 0x0002906C
		public static int SetGraphicsMode(HandleRef hDC, int iMode)
		{
			iMode = IntUnsafeNativeMethods.IntSetGraphicsMode(hDC, iMode);
			return iMode;
		}

		// Token: 0x06000BDD RID: 3037
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetGraphicsMode(HandleRef hDC);

		// Token: 0x06000BDE RID: 3038
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int GetROP2(HandleRef hdc);

		// Token: 0x06000BDF RID: 3039
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x06000BE0 RID: 3040
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CombineRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntCombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode);

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002AE78 File Offset: 0x00029078
		public static IntNativeMethods.RegionFlags CombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode)
		{
			if (hRgnDest.Wrapper == null || hRgnSrc1.Wrapper == null || hRgnSrc2.Wrapper == null)
			{
				return IntNativeMethods.RegionFlags.ERROR;
			}
			return IntUnsafeNativeMethods.IntCombineRgn(hRgnDest, hRgnSrc1, hRgnSrc2, combineMode);
		}

		// Token: 0x06000BE2 RID: 3042
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002AEA0 File Offset: 0x000290A0
		public static int GetClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntGetClipRgn(hDC, hRgn);
		}

		// Token: 0x06000BE4 RID: 3044
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntSelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002AEB8 File Offset: 0x000290B8
		public static IntNativeMethods.RegionFlags SelectClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntSelectClipRgn(hDC, hRgn);
		}

		// Token: 0x06000BE6 RID: 3046
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetRgnBox", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntGetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect);

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002AED0 File Offset: 0x000290D0
		public static IntNativeMethods.RegionFlags GetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect)
		{
			return IntUnsafeNativeMethods.IntGetRgnBox(hRgn, ref clipRect);
		}

		// Token: 0x06000BE8 RID: 3048
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateFontIndirect", SetLastError = true)]
		public static extern IntPtr IntCreateFontIndirect([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002AEE8 File Offset: 0x000290E8
		public static IntPtr CreateFontIndirect(object lf)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateFontIndirect(lf), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000BEA RID: 3050
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteObject(HandleRef hObject);

		// Token: 0x06000BEB RID: 3051 RVA: 0x0002AF08 File Offset: 0x00029108
		public static bool DeleteObject(HandleRef hObject)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hObject, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06000BEC RID: 3052
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hBrush, int nSize, [In] [Out] IntNativeMethods.LOGBRUSH lb);

		// Token: 0x06000BED RID: 3053 RVA: 0x0002AF30 File Offset: 0x00029130
		public static int GetObject(HandleRef hBrush, IntNativeMethods.LOGBRUSH lb)
		{
			return IntUnsafeNativeMethods.IntGetObject(hBrush, Marshal.SizeOf(typeof(IntNativeMethods.LOGBRUSH)), lb);
		}

		// Token: 0x06000BEE RID: 3054
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hFont, int nSize, [In] [Out] IntNativeMethods.LOGFONT lf);

		// Token: 0x06000BEF RID: 3055 RVA: 0x0002AF58 File Offset: 0x00029158
		public static int GetObject(HandleRef hFont, IntNativeMethods.LOGFONT lp)
		{
			return IntUnsafeNativeMethods.IntGetObject(hFont, Marshal.SizeOf(typeof(IntNativeMethods.LOGFONT)), lp);
		}

		// Token: 0x06000BF0 RID: 3056
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntSelectObject(HandleRef hdc, HandleRef obj);

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002AF80 File Offset: 0x00029180
		public static IntPtr SelectObject(HandleRef hdc, HandleRef obj)
		{
			return IntUnsafeNativeMethods.IntSelectObject(hdc, obj);
		}

		// Token: 0x06000BF2 RID: 3058
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetCurrentObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetCurrentObject(HandleRef hDC, int uObjectType);

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002AF98 File Offset: 0x00029198
		public static IntPtr GetCurrentObject(HandleRef hDC, int uObjectType)
		{
			return IntUnsafeNativeMethods.IntGetCurrentObject(hDC, uObjectType);
		}

		// Token: 0x06000BF4 RID: 3060
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetStockObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetStockObject(int nIndex);

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002AFB0 File Offset: 0x000291B0
		public static IntPtr GetStockObject(int nIndex)
		{
			return IntUnsafeNativeMethods.IntGetStockObject(nIndex);
		}

		// Token: 0x06000BF6 RID: 3062
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetNearestColor(HandleRef hDC, int color);

		// Token: 0x06000BF7 RID: 3063
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextColor(HandleRef hDC, int crColor);

		// Token: 0x06000BF8 RID: 3064
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextAlign(HandleRef hdc);

		// Token: 0x06000BF9 RID: 3065
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextColor(HandleRef hDC);

		// Token: 0x06000BFA RID: 3066
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);

		// Token: 0x06000BFB RID: 3067
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetBkMode(HandleRef hDC, int nBkMode);

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002AFC8 File Offset: 0x000291C8
		public static int SetBkMode(HandleRef hDC, int nBkMode)
		{
			return IntUnsafeNativeMethods.IntSetBkMode(hDC, nBkMode);
		}

		// Token: 0x06000BFD RID: 3069
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetBkMode(HandleRef hDC);

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002AFE0 File Offset: 0x000291E0
		public static int GetBkMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetBkMode(hDC);
		}

		// Token: 0x06000BFF RID: 3071
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBkColor(HandleRef hDC);

		// Token: 0x06000C00 RID: 3072
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x06000C01 RID: 3073
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x06000C02 RID: 3074 RVA: 0x0002AFF8 File Offset: 0x000291F8
		public static int DrawText(HandleRef hDC, string text, ref IntNativeMethods.RECT lpRect, int nFormat)
		{
			int result;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				lpRect.top = Math.Min(32767, lpRect.top);
				lpRect.left = Math.Min(32767, lpRect.left);
				lpRect.right = Math.Min(32767, lpRect.right);
				lpRect.bottom = Math.Min(32767, lpRect.bottom);
				int num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(num, 8192);
				result = IntUnsafeNativeMethods.DrawTextA(hDC, array, num, ref lpRect, nFormat);
			}
			else
			{
				result = IntUnsafeNativeMethods.DrawTextW(hDC, text, text.Length, ref lpRect, nFormat);
			}
			return result;
		}

		// Token: 0x06000C03 RID: 3075
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int DrawTextExW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x06000C04 RID: 3076
		[DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int DrawTextExA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002B0D4 File Offset: 0x000292D4
		public static int DrawTextEx(HandleRef hDC, string text, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams)
		{
			int result;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				lpRect.top = Math.Min(32767, lpRect.top);
				lpRect.left = Math.Min(32767, lpRect.left);
				lpRect.right = Math.Min(32767, lpRect.right);
				lpRect.bottom = Math.Min(32767, lpRect.bottom);
				int num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(num, 8192);
				result = IntUnsafeNativeMethods.DrawTextExA(hDC, array, num, ref lpRect, nFormat, lpDTParams);
			}
			else
			{
				result = IntUnsafeNativeMethods.DrawTextExW(hDC, text, text.Length, ref lpRect, nFormat, lpDTParams);
			}
			return result;
		}

		// Token: 0x06000C06 RID: 3078
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32W(HandleRef hDC, string text, int len, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x06000C07 RID: 3079
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32A(HandleRef hDC, byte[] lpszString, int byteCount, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x06000C08 RID: 3080 RVA: 0x0002B1B4 File Offset: 0x000293B4
		public static int GetTextExtentPoint32(HandleRef hDC, string text, [In] [Out] IntNativeMethods.SIZE size)
		{
			int num = text.Length;
			int result;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(text.Length, 8192);
				result = IntUnsafeNativeMethods.GetTextExtentPoint32A(hDC, array, num, size);
			}
			else
			{
				result = IntUnsafeNativeMethods.GetTextExtentPoint32W(hDC, text, text.Length, size);
			}
			return result;
		}

		// Token: 0x06000C09 RID: 3081
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ExtTextOut(HandleRef hdc, int x, int y, int options, ref IntNativeMethods.RECT rect, string str, int length, int[] spacing);

		// Token: 0x06000C0A RID: 3082
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "LineTo", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntLineTo(HandleRef hdc, int x, int y);

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002B23C File Offset: 0x0002943C
		public static bool LineTo(HandleRef hdc, int x, int y)
		{
			return IntUnsafeNativeMethods.IntLineTo(hdc, x, y);
		}

		// Token: 0x06000C0C RID: 3084
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "MoveToEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntMoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt);

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002B254 File Offset: 0x00029454
		public static bool MoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt)
		{
			return IntUnsafeNativeMethods.IntMoveToEx(hdc, x, y, pt);
		}

		// Token: 0x06000C0E RID: 3086
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Rectangle", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002B26C File Offset: 0x0002946C
		public static bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom)
		{
			return IntUnsafeNativeMethods.IntRectangle(hdc, left, top, right, bottom);
		}

		// Token: 0x06000C10 RID: 3088
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "FillRect", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntFillRect(HandleRef hdc, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush);

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002B288 File Offset: 0x00029488
		public static bool FillRect(HandleRef hDC, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush)
		{
			return IntUnsafeNativeMethods.IntFillRect(hDC, ref rect, hbrush);
		}

		// Token: 0x06000C12 RID: 3090
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetMapMode(HandleRef hDC, int nMapMode);

		// Token: 0x06000C13 RID: 3091 RVA: 0x0002B2A0 File Offset: 0x000294A0
		public static int SetMapMode(HandleRef hDC, int nMapMode)
		{
			return IntUnsafeNativeMethods.IntSetMapMode(hDC, nMapMode);
		}

		// Token: 0x06000C14 RID: 3092
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetMapMode(HandleRef hDC);

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002B2B8 File Offset: 0x000294B8
		public static int GetMapMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetMapMode(hDC);
		}

		// Token: 0x06000C16 RID: 3094
		[DllImport("gdi32.dll", EntryPoint = "GetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize);

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002B2D0 File Offset: 0x000294D0
		public static bool GetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize)
		{
			return IntUnsafeNativeMethods.IntGetViewportExtEx(hdc, lpSize);
		}

		// Token: 0x06000C18 RID: 3096
		[DllImport("gdi32.dll", EntryPoint = "GetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint);

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002B2E8 File Offset: 0x000294E8
		public static bool GetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint)
		{
			return IntUnsafeNativeMethods.IntGetViewportOrgEx(hdc, lpPoint);
		}

		// Token: 0x06000C1A RID: 3098
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x06000C1B RID: 3099 RVA: 0x0002B300 File Offset: 0x00029500
		public static bool SetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size)
		{
			return IntUnsafeNativeMethods.IntSetViewportExtEx(hDC, x, y, size);
		}

		// Token: 0x06000C1C RID: 3100
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x06000C1D RID: 3101 RVA: 0x0002B318 File Offset: 0x00029518
		public static bool SetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntSetViewportOrgEx(hDC, x, y, point);
		}

		// Token: 0x06000C1E RID: 3102
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRIC lptm);

		// Token: 0x06000C1F RID: 3103
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRICA lptm);

		// Token: 0x06000C20 RID: 3104 RVA: 0x0002B330 File Offset: 0x00029530
		public static int GetTextMetrics(HandleRef hDC, ref IntNativeMethods.TEXTMETRIC lptm)
		{
			int result;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				IntNativeMethods.TEXTMETRICA textmetrica = default(IntNativeMethods.TEXTMETRICA);
				result = IntUnsafeNativeMethods.GetTextMetricsA(hDC, ref textmetrica);
				lptm.tmHeight = textmetrica.tmHeight;
				lptm.tmAscent = textmetrica.tmAscent;
				lptm.tmDescent = textmetrica.tmDescent;
				lptm.tmInternalLeading = textmetrica.tmInternalLeading;
				lptm.tmExternalLeading = textmetrica.tmExternalLeading;
				lptm.tmAveCharWidth = textmetrica.tmAveCharWidth;
				lptm.tmMaxCharWidth = textmetrica.tmMaxCharWidth;
				lptm.tmWeight = textmetrica.tmWeight;
				lptm.tmOverhang = textmetrica.tmOverhang;
				lptm.tmDigitizedAspectX = textmetrica.tmDigitizedAspectX;
				lptm.tmDigitizedAspectY = textmetrica.tmDigitizedAspectY;
				lptm.tmFirstChar = (char)textmetrica.tmFirstChar;
				lptm.tmLastChar = (char)textmetrica.tmLastChar;
				lptm.tmDefaultChar = (char)textmetrica.tmDefaultChar;
				lptm.tmBreakChar = (char)textmetrica.tmBreakChar;
				lptm.tmItalic = textmetrica.tmItalic;
				lptm.tmUnderlined = textmetrica.tmUnderlined;
				lptm.tmStruckOut = textmetrica.tmStruckOut;
				lptm.tmPitchAndFamily = textmetrica.tmPitchAndFamily;
				lptm.tmCharSet = textmetrica.tmCharSet;
			}
			else
			{
				result = IntUnsafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
			}
			return result;
		}

		// Token: 0x06000C21 RID: 3105
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "BeginPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntBeginPath(HandleRef hDC);

		// Token: 0x06000C22 RID: 3106 RVA: 0x0002B454 File Offset: 0x00029654
		public static bool BeginPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntBeginPath(hDC);
		}

		// Token: 0x06000C23 RID: 3107
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "EndPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEndPath(HandleRef hDC);

		// Token: 0x06000C24 RID: 3108 RVA: 0x0002B46C File Offset: 0x0002966C
		public static bool EndPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntEndPath(hDC);
		}

		// Token: 0x06000C25 RID: 3109
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "StrokePath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntStrokePath(HandleRef hDC);

		// Token: 0x06000C26 RID: 3110 RVA: 0x0002B484 File Offset: 0x00029684
		public static bool StrokePath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntStrokePath(hDC);
		}

		// Token: 0x06000C27 RID: 3111
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "AngleArc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntAngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle);

		// Token: 0x06000C28 RID: 3112 RVA: 0x0002B49C File Offset: 0x0002969C
		public static bool AngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle)
		{
			return IntUnsafeNativeMethods.IntAngleArc(hDC, x, y, radius, startAngle, endAngle);
		}

		// Token: 0x06000C29 RID: 3113
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Arc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntArc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc);

		// Token: 0x06000C2A RID: 3114 RVA: 0x0002B4B8 File Offset: 0x000296B8
		public static bool Arc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc)
		{
			return IntUnsafeNativeMethods.IntArc(hDC, nLeftRect, nTopRect, nRightRect, nBottomRect, nXStartArc, nYStartArc, nXEndArc, nYEndArc);
		}

		// Token: 0x06000C2B RID: 3115
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextAlign(HandleRef hDC, int nMode);

		// Token: 0x06000C2C RID: 3116
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Ellipse", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEllipse(HandleRef hDc, int x1, int y1, int x2, int y2);

		// Token: 0x06000C2D RID: 3117 RVA: 0x0002B4DC File Offset: 0x000296DC
		public static bool Ellipse(HandleRef hDc, int x1, int y1, int x2, int y2)
		{
			return IntUnsafeNativeMethods.IntEllipse(hDc, x1, y1, x2, y2);
		}

		// Token: 0x06000C2E RID: 3118
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);
	}
}
