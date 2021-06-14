using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F6 RID: 1270
	[SuppressUnmanagedCodeSecurity]
	internal static class IntUnsafeNativeMethods
	{
		// Token: 0x0600533B RID: 21307
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x0600533C RID: 21308 RVA: 0x0015D078 File Offset: 0x0015B278
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntGetDC(hWnd), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600533D RID: 21309
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x0600533E RID: 21310 RVA: 0x0015D098 File Offset: 0x0015B298
		public static bool DeleteDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x0015D0C0 File Offset: 0x0015B2C0
		public static bool DeleteHDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06005340 RID: 21312
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x06005341 RID: 21313 RVA: 0x0015D0E6 File Offset: 0x0015B2E6
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x06005342 RID: 21314
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDC", SetLastError = true)]
		public static extern IntPtr IntCreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06005343 RID: 21315 RVA: 0x0015D100 File Offset: 0x0015B300
		public static IntPtr CreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateDC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06005344 RID: 21316
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateIC", SetLastError = true)]
		public static extern IntPtr IntCreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06005345 RID: 21317 RVA: 0x0015D124 File Offset: 0x0015B324
		public static IntPtr CreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateIC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06005346 RID: 21318
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x06005347 RID: 21319 RVA: 0x0015D148 File Offset: 0x0015B348
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateCompatibleDC(hDC), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06005348 RID: 21320
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SaveDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSaveDC(HandleRef hDC);

		// Token: 0x06005349 RID: 21321 RVA: 0x0015D168 File Offset: 0x0015B368
		public static int SaveDC(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntSaveDC(hDC);
		}

		// Token: 0x0600534A RID: 21322
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "RestoreDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRestoreDC(HandleRef hDC, int nSavedDC);

		// Token: 0x0600534B RID: 21323 RVA: 0x0015D180 File Offset: 0x0015B380
		public static bool RestoreDC(HandleRef hDC, int nSavedDC)
		{
			return IntUnsafeNativeMethods.IntRestoreDC(hDC, nSavedDC);
		}

		// Token: 0x0600534C RID: 21324
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr WindowFromDC(HandleRef hDC);

		// Token: 0x0600534D RID: 21325
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x0600534E RID: 21326
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "OffsetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntOffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x0600534F RID: 21327 RVA: 0x0015D198 File Offset: 0x0015B398
		public static bool OffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntOffsetViewportOrgEx(hDC, nXOffset, nYOffset, point);
		}

		// Token: 0x06005350 RID: 21328
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetGraphicsMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetGraphicsMode(HandleRef hDC, int iMode);

		// Token: 0x06005351 RID: 21329 RVA: 0x0015D1B0 File Offset: 0x0015B3B0
		public static int SetGraphicsMode(HandleRef hDC, int iMode)
		{
			iMode = IntUnsafeNativeMethods.IntSetGraphicsMode(hDC, iMode);
			return iMode;
		}

		// Token: 0x06005352 RID: 21330
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetGraphicsMode(HandleRef hDC);

		// Token: 0x06005353 RID: 21331
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int GetROP2(HandleRef hdc);

		// Token: 0x06005354 RID: 21332
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x06005355 RID: 21333
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CombineRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntCombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode);

		// Token: 0x06005356 RID: 21334 RVA: 0x0015D1BC File Offset: 0x0015B3BC
		public static IntNativeMethods.RegionFlags CombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode)
		{
			if (hRgnDest.Wrapper == null || hRgnSrc1.Wrapper == null || hRgnSrc2.Wrapper == null)
			{
				return IntNativeMethods.RegionFlags.ERROR;
			}
			return IntUnsafeNativeMethods.IntCombineRgn(hRgnDest, hRgnSrc1, hRgnSrc2, combineMode);
		}

		// Token: 0x06005357 RID: 21335
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06005358 RID: 21336 RVA: 0x0015D1E4 File Offset: 0x0015B3E4
		public static int GetClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntGetClipRgn(hDC, hRgn);
		}

		// Token: 0x06005359 RID: 21337
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntSelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x0600535A RID: 21338 RVA: 0x0015D1FC File Offset: 0x0015B3FC
		public static IntNativeMethods.RegionFlags SelectClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntSelectClipRgn(hDC, hRgn);
		}

		// Token: 0x0600535B RID: 21339
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetRgnBox", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntGetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect);

		// Token: 0x0600535C RID: 21340 RVA: 0x0015D214 File Offset: 0x0015B414
		public static IntNativeMethods.RegionFlags GetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect)
		{
			return IntUnsafeNativeMethods.IntGetRgnBox(hRgn, ref clipRect);
		}

		// Token: 0x0600535D RID: 21341
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateFontIndirect", SetLastError = true)]
		public static extern IntPtr IntCreateFontIndirect([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

		// Token: 0x0600535E RID: 21342 RVA: 0x0015D22C File Offset: 0x0015B42C
		public static IntPtr CreateFontIndirect(object lf)
		{
			return System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateFontIndirect(lf), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600535F RID: 21343
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteObject(HandleRef hObject);

		// Token: 0x06005360 RID: 21344 RVA: 0x0015D24C File Offset: 0x0015B44C
		public static bool DeleteObject(HandleRef hObject)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hObject, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06005361 RID: 21345
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hBrush, int nSize, [In] [Out] IntNativeMethods.LOGBRUSH lb);

		// Token: 0x06005362 RID: 21346 RVA: 0x0015D274 File Offset: 0x0015B474
		public static int GetObject(HandleRef hBrush, IntNativeMethods.LOGBRUSH lb)
		{
			return IntUnsafeNativeMethods.IntGetObject(hBrush, Marshal.SizeOf(typeof(IntNativeMethods.LOGBRUSH)), lb);
		}

		// Token: 0x06005363 RID: 21347
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hFont, int nSize, [In] [Out] IntNativeMethods.LOGFONT lf);

		// Token: 0x06005364 RID: 21348 RVA: 0x0015D29C File Offset: 0x0015B49C
		public static int GetObject(HandleRef hFont, IntNativeMethods.LOGFONT lp)
		{
			return IntUnsafeNativeMethods.IntGetObject(hFont, Marshal.SizeOf(typeof(IntNativeMethods.LOGFONT)), lp);
		}

		// Token: 0x06005365 RID: 21349
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntSelectObject(HandleRef hdc, HandleRef obj);

		// Token: 0x06005366 RID: 21350 RVA: 0x0015D2C4 File Offset: 0x0015B4C4
		public static IntPtr SelectObject(HandleRef hdc, HandleRef obj)
		{
			return IntUnsafeNativeMethods.IntSelectObject(hdc, obj);
		}

		// Token: 0x06005367 RID: 21351
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetCurrentObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetCurrentObject(HandleRef hDC, int uObjectType);

		// Token: 0x06005368 RID: 21352 RVA: 0x0015D2DC File Offset: 0x0015B4DC
		public static IntPtr GetCurrentObject(HandleRef hDC, int uObjectType)
		{
			return IntUnsafeNativeMethods.IntGetCurrentObject(hDC, uObjectType);
		}

		// Token: 0x06005369 RID: 21353
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetStockObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetStockObject(int nIndex);

		// Token: 0x0600536A RID: 21354 RVA: 0x0015D2F4 File Offset: 0x0015B4F4
		public static IntPtr GetStockObject(int nIndex)
		{
			return IntUnsafeNativeMethods.IntGetStockObject(nIndex);
		}

		// Token: 0x0600536B RID: 21355
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetNearestColor(HandleRef hDC, int color);

		// Token: 0x0600536C RID: 21356
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextColor(HandleRef hDC, int crColor);

		// Token: 0x0600536D RID: 21357
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextAlign(HandleRef hdc);

		// Token: 0x0600536E RID: 21358
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextColor(HandleRef hDC);

		// Token: 0x0600536F RID: 21359
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);

		// Token: 0x06005370 RID: 21360
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetBkMode(HandleRef hDC, int nBkMode);

		// Token: 0x06005371 RID: 21361 RVA: 0x0015D30C File Offset: 0x0015B50C
		public static int SetBkMode(HandleRef hDC, int nBkMode)
		{
			return IntUnsafeNativeMethods.IntSetBkMode(hDC, nBkMode);
		}

		// Token: 0x06005372 RID: 21362
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetBkMode(HandleRef hDC);

		// Token: 0x06005373 RID: 21363 RVA: 0x0015D324 File Offset: 0x0015B524
		public static int GetBkMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetBkMode(hDC);
		}

		// Token: 0x06005374 RID: 21364
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBkColor(HandleRef hDC);

		// Token: 0x06005375 RID: 21365
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x06005376 RID: 21366
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x06005377 RID: 21367 RVA: 0x0015D33C File Offset: 0x0015B53C
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

		// Token: 0x06005378 RID: 21368
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int DrawTextExW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x06005379 RID: 21369
		[DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int DrawTextExA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x0600537A RID: 21370 RVA: 0x0015D418 File Offset: 0x0015B618
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

		// Token: 0x0600537B RID: 21371
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32W(HandleRef hDC, string text, int len, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x0600537C RID: 21372
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32A(HandleRef hDC, byte[] lpszString, int byteCount, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x0600537D RID: 21373 RVA: 0x0015D4F8 File Offset: 0x0015B6F8
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

		// Token: 0x0600537E RID: 21374
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ExtTextOut(HandleRef hdc, int x, int y, int options, ref IntNativeMethods.RECT rect, string str, int length, int[] spacing);

		// Token: 0x0600537F RID: 21375
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "LineTo", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntLineTo(HandleRef hdc, int x, int y);

		// Token: 0x06005380 RID: 21376 RVA: 0x0015D580 File Offset: 0x0015B780
		public static bool LineTo(HandleRef hdc, int x, int y)
		{
			return IntUnsafeNativeMethods.IntLineTo(hdc, x, y);
		}

		// Token: 0x06005381 RID: 21377
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "MoveToEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntMoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt);

		// Token: 0x06005382 RID: 21378 RVA: 0x0015D598 File Offset: 0x0015B798
		public static bool MoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt)
		{
			return IntUnsafeNativeMethods.IntMoveToEx(hdc, x, y, pt);
		}

		// Token: 0x06005383 RID: 21379
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Rectangle", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x06005384 RID: 21380 RVA: 0x0015D5B0 File Offset: 0x0015B7B0
		public static bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom)
		{
			return IntUnsafeNativeMethods.IntRectangle(hdc, left, top, right, bottom);
		}

		// Token: 0x06005385 RID: 21381
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "FillRect", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntFillRect(HandleRef hdc, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush);

		// Token: 0x06005386 RID: 21382 RVA: 0x0015D5CC File Offset: 0x0015B7CC
		public static bool FillRect(HandleRef hDC, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush)
		{
			return IntUnsafeNativeMethods.IntFillRect(hDC, ref rect, hbrush);
		}

		// Token: 0x06005387 RID: 21383
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetMapMode(HandleRef hDC, int nMapMode);

		// Token: 0x06005388 RID: 21384 RVA: 0x0015D5E4 File Offset: 0x0015B7E4
		public static int SetMapMode(HandleRef hDC, int nMapMode)
		{
			return IntUnsafeNativeMethods.IntSetMapMode(hDC, nMapMode);
		}

		// Token: 0x06005389 RID: 21385
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetMapMode(HandleRef hDC);

		// Token: 0x0600538A RID: 21386 RVA: 0x0015D5FC File Offset: 0x0015B7FC
		public static int GetMapMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetMapMode(hDC);
		}

		// Token: 0x0600538B RID: 21387
		[DllImport("gdi32.dll", EntryPoint = "GetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize);

		// Token: 0x0600538C RID: 21388 RVA: 0x0015D614 File Offset: 0x0015B814
		public static bool GetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize)
		{
			return IntUnsafeNativeMethods.IntGetViewportExtEx(hdc, lpSize);
		}

		// Token: 0x0600538D RID: 21389
		[DllImport("gdi32.dll", EntryPoint = "GetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint);

		// Token: 0x0600538E RID: 21390 RVA: 0x0015D62C File Offset: 0x0015B82C
		public static bool GetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint)
		{
			return IntUnsafeNativeMethods.IntGetViewportOrgEx(hdc, lpPoint);
		}

		// Token: 0x0600538F RID: 21391
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x06005390 RID: 21392 RVA: 0x0015D644 File Offset: 0x0015B844
		public static bool SetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size)
		{
			return IntUnsafeNativeMethods.IntSetViewportExtEx(hDC, x, y, size);
		}

		// Token: 0x06005391 RID: 21393
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x06005392 RID: 21394 RVA: 0x0015D65C File Offset: 0x0015B85C
		public static bool SetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntSetViewportOrgEx(hDC, x, y, point);
		}

		// Token: 0x06005393 RID: 21395
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRIC lptm);

		// Token: 0x06005394 RID: 21396
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRICA lptm);

		// Token: 0x06005395 RID: 21397 RVA: 0x0015D674 File Offset: 0x0015B874
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

		// Token: 0x06005396 RID: 21398
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "BeginPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntBeginPath(HandleRef hDC);

		// Token: 0x06005397 RID: 21399 RVA: 0x0015D798 File Offset: 0x0015B998
		public static bool BeginPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntBeginPath(hDC);
		}

		// Token: 0x06005398 RID: 21400
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "EndPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEndPath(HandleRef hDC);

		// Token: 0x06005399 RID: 21401 RVA: 0x0015D7B0 File Offset: 0x0015B9B0
		public static bool EndPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntEndPath(hDC);
		}

		// Token: 0x0600539A RID: 21402
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "StrokePath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntStrokePath(HandleRef hDC);

		// Token: 0x0600539B RID: 21403 RVA: 0x0015D7C8 File Offset: 0x0015B9C8
		public static bool StrokePath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntStrokePath(hDC);
		}

		// Token: 0x0600539C RID: 21404
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "AngleArc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntAngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle);

		// Token: 0x0600539D RID: 21405 RVA: 0x0015D7E0 File Offset: 0x0015B9E0
		public static bool AngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle)
		{
			return IntUnsafeNativeMethods.IntAngleArc(hDC, x, y, radius, startAngle, endAngle);
		}

		// Token: 0x0600539E RID: 21406
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Arc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntArc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc);

		// Token: 0x0600539F RID: 21407 RVA: 0x0015D7FC File Offset: 0x0015B9FC
		public static bool Arc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc)
		{
			return IntUnsafeNativeMethods.IntArc(hDC, nLeftRect, nTopRect, nRightRect, nBottomRect, nXStartArc, nYStartArc, nXEndArc, nYEndArc);
		}

		// Token: 0x060053A0 RID: 21408
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextAlign(HandleRef hDC, int nMode);

		// Token: 0x060053A1 RID: 21409
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Ellipse", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEllipse(HandleRef hDc, int x1, int y1, int x2, int y2);

		// Token: 0x060053A2 RID: 21410 RVA: 0x0015D820 File Offset: 0x0015BA20
		public static bool Ellipse(HandleRef hDc, int x1, int y1, int x2, int y2)
		{
			return IntUnsafeNativeMethods.IntEllipse(hDc, x1, y1, x2, y2);
		}

		// Token: 0x060053A3 RID: 21411
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);
	}
}
