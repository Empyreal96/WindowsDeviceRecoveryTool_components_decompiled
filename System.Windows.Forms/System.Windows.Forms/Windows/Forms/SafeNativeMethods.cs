using System;
using System.Drawing;
using System.Internal;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000340 RID: 832
	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		// Token: 0x0600334B RID: 13131
		[DllImport("shlwapi.dll")]
		public static extern int SHAutoComplete(HandleRef hwndEdit, int flags);

		// Token: 0x0600334C RID: 13132
		[DllImport("user32.dll")]
		public static extern int OemKeyScan(short wAsciiVal);

		// Token: 0x0600334D RID: 13133
		[DllImport("gdi32.dll")]
		public static extern int GetSystemPaletteEntries(HandleRef hdc, int iStartIndex, int nEntries, byte[] lppe);

		// Token: 0x0600334E RID: 13134
		[DllImport("gdi32.dll")]
		public static extern int GetDIBits(HandleRef hdc, HandleRef hbm, int uStartScan, int cScanLines, byte[] lpvBits, ref NativeMethods.BITMAPINFO_FLAT bmi, int uUsage);

		// Token: 0x0600334F RID: 13135
		[DllImport("gdi32.dll")]
		public static extern int StretchDIBits(HandleRef hdc, int XDest, int YDest, int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth, int nSrcHeight, byte[] lpBits, ref NativeMethods.BITMAPINFO_FLAT lpBitsInfo, int iUsage, int dwRop);

		// Token: 0x06003350 RID: 13136
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleBitmap(HandleRef hDC, int width, int height);

		// Token: 0x06003351 RID: 13137 RVA: 0x000EEE27 File Offset: 0x000ED027
		public static IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateCompatibleBitmap(hDC, width, height), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06003352 RID: 13138
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, [In] [Out] NativeMethods.SCROLLINFO si);

		// Token: 0x06003353 RID: 13139
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsAccelerator(HandleRef hAccel, int cAccelEntries, [In] ref NativeMethods.MSG lpMsg, short[] lpwCmd);

		// Token: 0x06003354 RID: 13140
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ChooseFont([In] [Out] NativeMethods.CHOOSEFONT cf);

		// Token: 0x06003355 RID: 13141
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBitmapBits(HandleRef hbmp, int cbBuffer, byte[] lpvBits);

		// Token: 0x06003356 RID: 13142
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int CommDlgExtendedError();

		// Token: 0x06003357 RID: 13143
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern void SysFreeString(HandleRef bstr);

		// Token: 0x06003358 RID: 13144
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, [MarshalAs(UnmanagedType.Interface)] ref object pobjs, int pages, HandleRef pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x06003359 RID: 13145
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, [MarshalAs(UnmanagedType.Interface)] ref object pobjs, int pages, Guid[] pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x0600335A RID: 13146
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void OleCreatePropertyFrame(HandleRef hwndOwner, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string caption, int objects, HandleRef lplpobjs, int pages, HandleRef pClsid, int locale, int reserved1, IntPtr reserved2);

		// Token: 0x0600335B RID: 13147
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, int dwData);

		// Token: 0x0600335C RID: 13148
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, string dwData);

		// Token: 0x0600335D RID: 13149
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_POPUP dwData);

		// Token: 0x0600335E RID: 13150
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_FTS_QUERY dwData);

		// Token: 0x0600335F RID: 13151
		[DllImport("hhctrl.ocx", CharSet = CharSet.Auto)]
		public static extern int HtmlHelp(HandleRef hwndCaller, [MarshalAs(UnmanagedType.LPTStr)] string pszFile, int uCommand, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.HH_AKLINK dwData);

		// Token: 0x06003360 RID: 13152
		[DllImport("oleaut32.dll")]
		public static extern void VariantInit(HandleRef pObject);

		// Token: 0x06003361 RID: 13153
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void VariantClear(HandleRef pObject);

		// Token: 0x06003362 RID: 13154
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool LineTo(HandleRef hdc, int x, int y);

		// Token: 0x06003363 RID: 13155
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool MoveToEx(HandleRef hdc, int x, int y, NativeMethods.POINT pt);

		// Token: 0x06003364 RID: 13156
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x06003365 RID: 13157
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool PatBlt(HandleRef hdc, int left, int top, int width, int height, int rop);

		// Token: 0x06003366 RID: 13158
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetThreadLocale")]
		public static extern int GetThreadLCID();

		// Token: 0x06003367 RID: 13159
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessagePos();

		// Token: 0x06003368 RID: 13160
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int RegisterClipboardFormat(string format);

		// Token: 0x06003369 RID: 13161
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetClipboardFormatName(int format, StringBuilder lpString, int cchMax);

		// Token: 0x0600336A RID: 13162
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ChooseColor([In] [Out] NativeMethods.CHOOSECOLOR cc);

		// Token: 0x0600336B RID: 13163
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int RegisterWindowMessage(string msg);

		// Token: 0x0600336C RID: 13164
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		public static extern bool ExternalDeleteObject(HandleRef hObject);

		// Token: 0x0600336D RID: 13165
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		internal static extern bool IntDeleteObject(HandleRef hObject);

		// Token: 0x0600336E RID: 13166 RVA: 0x000EEE3B File Offset: 0x000ED03B
		public static bool DeleteObject(HandleRef hObject)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hObject, NativeMethods.CommonHandles.GDI);
			return SafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x0600336F RID: 13167
		[DllImport("oleaut32.dll", EntryPoint = "OleCreateFontIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern SafeNativeMethods.IFontDisp OleCreateIFontDispIndirect(NativeMethods.FONTDESC fd, ref Guid iid);

		// Token: 0x06003370 RID: 13168
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateSolidBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateSolidBrush(int crColor);

		// Token: 0x06003371 RID: 13169 RVA: 0x000EEE54 File Offset: 0x000ED054
		public static IntPtr CreateSolidBrush(int crColor)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateSolidBrush(crColor), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06003372 RID: 13170
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowExtEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.SIZE size);

		// Token: 0x06003373 RID: 13171
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, HandleRef arguments);

		// Token: 0x06003374 RID: 13172
		[DllImport("comctl32.dll")]
		public static extern void InitCommonControls();

		// Token: 0x06003375 RID: 13173
		[DllImport("comctl32.dll")]
		public static extern bool InitCommonControlsEx(NativeMethods.INITCOMMONCONTROLSEX icc);

		// Token: 0x06003376 RID: 13174
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Create(int cx, int cy, int flags, int cInitial, int cGrow);

		// Token: 0x06003377 RID: 13175
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Destroy(HandleRef himl);

		// Token: 0x06003378 RID: 13176
		[DllImport("comctl32.dll", EntryPoint = "ImageList_Destroy")]
		public static extern bool ImageList_Destroy_Native(HandleRef himl);

		// Token: 0x06003379 RID: 13177
		[DllImport("comctl32.dll")]
		public static extern int ImageList_GetImageCount(HandleRef himl);

		// Token: 0x0600337A RID: 13178
		[DllImport("comctl32.dll")]
		public static extern int ImageList_Add(HandleRef himl, HandleRef hbmImage, HandleRef hbmMask);

		// Token: 0x0600337B RID: 13179
		[DllImport("comctl32.dll")]
		public static extern int ImageList_ReplaceIcon(HandleRef himl, int index, HandleRef hicon);

		// Token: 0x0600337C RID: 13180
		[DllImport("comctl32.dll")]
		public static extern int ImageList_SetBkColor(HandleRef himl, int clrBk);

		// Token: 0x0600337D RID: 13181
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Draw(HandleRef himl, int i, HandleRef hdcDst, int x, int y, int fStyle);

		// Token: 0x0600337E RID: 13182
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Replace(HandleRef himl, int i, HandleRef hbmImage, HandleRef hbmMask);

		// Token: 0x0600337F RID: 13183
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_DrawEx(HandleRef himl, int i, HandleRef hdcDst, int x, int y, int dx, int dy, int rgbBk, int rgbFg, int fStyle);

		// Token: 0x06003380 RID: 13184
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_GetIconSize(HandleRef himl, out int x, out int y);

		// Token: 0x06003381 RID: 13185
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Duplicate(HandleRef himl);

		// Token: 0x06003382 RID: 13186
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Remove(HandleRef himl, int i);

		// Token: 0x06003383 RID: 13187
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_GetImageInfo(HandleRef himl, int i, NativeMethods.IMAGEINFO pImageInfo);

		// Token: 0x06003384 RID: 13188
		[DllImport("comctl32.dll")]
		public static extern IntPtr ImageList_Read(UnsafeNativeMethods.IStream pstm);

		// Token: 0x06003385 RID: 13189
		[DllImport("comctl32.dll")]
		public static extern bool ImageList_Write(HandleRef himl, UnsafeNativeMethods.IStream pstm);

		// Token: 0x06003386 RID: 13190
		[DllImport("comctl32.dll")]
		public static extern int ImageList_WriteEx(HandleRef himl, int dwFlags, UnsafeNativeMethods.IStream pstm);

		// Token: 0x06003387 RID: 13191
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TrackPopupMenuEx(HandleRef hmenu, int fuFlags, int x, int y, HandleRef hwnd, NativeMethods.TPMPARAMS tpm);

		// Token: 0x06003388 RID: 13192
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetKeyboardLayout(int dwLayout);

		// Token: 0x06003389 RID: 13193
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr ActivateKeyboardLayout(HandleRef hkl, int uFlags);

		// Token: 0x0600338A RID: 13194
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetKeyboardLayoutList(int size, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] hkls);

		// Token: 0x0600338B RID: 13195
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref NativeMethods.DEVMODE lpDevMode);

		// Token: 0x0600338C RID: 13196
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(HandleRef hmonitor, [In] [Out] NativeMethods.MONITORINFOEX info);

		// Token: 0x0600338D RID: 13197
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromPoint(NativeMethods.POINTSTRUCT pt, int flags);

		// Token: 0x0600338E RID: 13198
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromRect(ref NativeMethods.RECT rect, int flags);

		// Token: 0x0600338F RID: 13199
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

		// Token: 0x06003390 RID: 13200
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EnumDisplayMonitors(HandleRef hdc, NativeMethods.COMRECT rcClip, NativeMethods.MonitorEnumProc lpfnEnum, IntPtr dwData);

		// Token: 0x06003391 RID: 13201
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateHalftonePalette", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateHalftonePalette(HandleRef hdc);

		// Token: 0x06003392 RID: 13202 RVA: 0x000EEE66 File Offset: 0x000ED066
		public static IntPtr CreateHalftonePalette(HandleRef hdc)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateHalftonePalette(hdc), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06003393 RID: 13203
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetPaletteEntries(HandleRef hpal, int iStartIndex, int nEntries, int[] lppe);

		// Token: 0x06003394 RID: 13204
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref NativeMethods.TEXTMETRIC lptm);

		// Token: 0x06003395 RID: 13205
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref NativeMethods.TEXTMETRICA lptm);

		// Token: 0x06003396 RID: 13206 RVA: 0x000EEE78 File Offset: 0x000ED078
		public static int GetTextMetrics(HandleRef hDC, ref NativeMethods.TEXTMETRIC lptm)
		{
			if (Marshal.SystemDefaultCharSize == 1)
			{
				NativeMethods.TEXTMETRICA textmetrica = default(NativeMethods.TEXTMETRICA);
				int textMetricsA = SafeNativeMethods.GetTextMetricsA(hDC, ref textmetrica);
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
				return textMetricsA;
			}
			return SafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
		}

		// Token: 0x06003397 RID: 13207
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDIBSection", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateDIBSection(HandleRef hdc, HandleRef pbmi, int iUsage, byte[] ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x06003398 RID: 13208 RVA: 0x000EEF9A File Offset: 0x000ED19A
		public static IntPtr CreateDIBSection(HandleRef hdc, HandleRef pbmi, int iUsage, byte[] ppvBits, IntPtr hSection, int dwOffset)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateDIBSection(hdc, pbmi, iUsage, ppvBits, hSection, dwOffset), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06003399 RID: 13209
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, IntPtr lpvBits);

		// Token: 0x0600339A RID: 13210 RVA: 0x000EEFB3 File Offset: 0x000ED1B3
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, IntPtr lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmap(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600339B RID: 13211
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmapShort(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits);

		// Token: 0x0600339C RID: 13212 RVA: 0x000EEFCA File Offset: 0x000ED1CA
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, short[] lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmapShort(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600339D RID: 13213
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBitmapByte(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, byte[] lpvBits);

		// Token: 0x0600339E RID: 13214 RVA: 0x000EEFE1 File Offset: 0x000ED1E1
		public static IntPtr CreateBitmap(int nWidth, int nHeight, int nPlanes, int nBitsPerPixel, byte[] lpvBits)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmapByte(nWidth, nHeight, nPlanes, nBitsPerPixel, lpvBits), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600339F RID: 13215
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePatternBrush", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePatternBrush(HandleRef hbmp);

		// Token: 0x060033A0 RID: 13216 RVA: 0x000EEFF8 File Offset: 0x000ED1F8
		public static IntPtr CreatePatternBrush(HandleRef hbmp)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePatternBrush(hbmp), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060033A1 RID: 13217
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBrushIndirect", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateBrushIndirect(NativeMethods.LOGBRUSH lb);

		// Token: 0x060033A2 RID: 13218 RVA: 0x000EF00A File Offset: 0x000ED20A
		public static IntPtr CreateBrushIndirect(NativeMethods.LOGBRUSH lb)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBrushIndirect(lb), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060033A3 RID: 13219
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreatePen(int nStyle, int nWidth, int crColor);

		// Token: 0x060033A4 RID: 13220 RVA: 0x000EF01C File Offset: 0x000ED21C
		public static IntPtr CreatePen(int nStyle, int nWidth, int crColor)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePen(nStyle, nWidth, crColor), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060033A5 RID: 13221
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetViewportExtEx(HandleRef hDC, int x, int y, NativeMethods.SIZE size);

		// Token: 0x060033A6 RID: 13222
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr LoadCursor(HandleRef hInst, int iconId);

		// Token: 0x060033A7 RID: 13223
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClipCursor([In] [Out] ref NativeMethods.RECT lpRect);

		// Token: 0x060033A8 RID: 13224
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCursor();

		// Token: 0x060033A9 RID: 13225
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetIconInfo(HandleRef hIcon, [In] [Out] NativeMethods.ICONINFO info);

		// Token: 0x060033AA RID: 13226
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int IntersectClipRect(HandleRef hDC, int x1, int y1, int x2, int y2);

		// Token: 0x060033AB RID: 13227
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CopyImage", ExactSpelling = true)]
		private static extern IntPtr IntCopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags);

		// Token: 0x060033AC RID: 13228 RVA: 0x000EF030 File Offset: 0x000ED230
		public static IntPtr CopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000EF047 File Offset: 0x000ED247
		public static IntPtr CopyImageAsCursor(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), NativeMethods.CommonHandles.Cursor);
		}

		// Token: 0x060033AE RID: 13230
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool AdjustWindowRectEx(ref NativeMethods.RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

		// Token: 0x060033AF RID: 13231
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool AdjustWindowRectExForDpi(ref NativeMethods.RECT lpRect, int dwStyle, bool bMenu, int dwExStyle, uint dpi);

		// Token: 0x060033B0 RID: 13232
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int DoDragDrop(IDataObject dataObject, UnsafeNativeMethods.IOleDropSource dropSource, int allowedEffects, int[] finalEffect);

		// Token: 0x060033B1 RID: 13233
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSysColorBrush(int nIndex);

		// Token: 0x060033B2 RID: 13234
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnableWindow(HandleRef hWnd, bool enable);

		// Token: 0x060033B3 RID: 13235
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x060033B4 RID: 13236
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetDoubleClickTime();

		// Token: 0x060033B5 RID: 13237
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetUpdateRgn(HandleRef hwnd, HandleRef hrgn, bool fErase);

		// Token: 0x060033B6 RID: 13238
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ValidateRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x060033B7 RID: 13239
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int FillRect(HandleRef hdc, [In] ref NativeMethods.RECT rect, HandleRef hbrush);

		// Token: 0x060033B8 RID: 13240
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTextColor(HandleRef hDC);

		// Token: 0x060033B9 RID: 13241
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBkColor(HandleRef hDC);

		// Token: 0x060033BA RID: 13242
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextColor(HandleRef hDC, int crColor);

		// Token: 0x060033BB RID: 13243
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);

		// Token: 0x060033BC RID: 13244
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectPalette(HandleRef hdc, HandleRef hpal, int bForceBackground);

		// Token: 0x060033BD RID: 13245
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.POINT point);

		// Token: 0x060033BE RID: 13246
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x060033BF RID: 13247 RVA: 0x000EF05E File Offset: 0x000ED25E
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060033C0 RID: 13248
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int CombineRgn(HandleRef hRgn, HandleRef hRgn1, HandleRef hRgn2, int nCombineMode);

		// Token: 0x060033C1 RID: 13249
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int RealizePalette(HandleRef hDC);

		// Token: 0x060033C2 RID: 13250
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool LPtoDP(HandleRef hDC, [In] [Out] ref NativeMethods.RECT lpRect, int nCount);

		// Token: 0x060033C3 RID: 13251
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetWindowOrgEx(HandleRef hDC, int x, int y, [In] [Out] NativeMethods.POINT point);

		// Token: 0x060033C4 RID: 13252
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GetViewportOrgEx(HandleRef hDC, [In] [Out] NativeMethods.POINT point);

		// Token: 0x060033C5 RID: 13253
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetMapMode(HandleRef hDC, int nMapMode);

		// Token: 0x060033C6 RID: 13254
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowEnabled(HandleRef hWnd);

		// Token: 0x060033C7 RID: 13255
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		// Token: 0x060033C8 RID: 13256
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ReleaseCapture();

		// Token: 0x060033C9 RID: 13257
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentThreadId();

		// Token: 0x060033CA RID: 13258
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumWindows(SafeNativeMethods.EnumThreadWindowsCallback callback, IntPtr extraData);

		// Token: 0x060033CB RID: 13259
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

		// Token: 0x060033CC RID: 13260
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(HandleRef hWnd, out int lpdwExitCode);

		// Token: 0x060033CD RID: 13261
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

		// Token: 0x060033CE RID: 13262
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		// Token: 0x060033CF RID: 13263
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		// Token: 0x060033D0 RID: 13264
		[CLSCompliant(false)]
		[DllImport("comctl32.dll", ExactSpelling = true)]
		private static extern bool _TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme);

		// Token: 0x060033D1 RID: 13265 RVA: 0x000EF073 File Offset: 0x000ED273
		public static bool TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme)
		{
			return SafeNativeMethods._TrackMouseEvent(tme);
		}

		// Token: 0x060033D2 RID: 13266
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(HandleRef hwnd, ref NativeMethods.RECT rcUpdate, HandleRef hrgnUpdate, int flags);

		// Token: 0x060033D3 RID: 13267
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(HandleRef hwnd, NativeMethods.COMRECT rcUpdate, HandleRef hrgnUpdate, int flags);

		// Token: 0x060033D4 RID: 13268
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRect(HandleRef hWnd, ref NativeMethods.RECT rect, bool erase);

		// Token: 0x060033D5 RID: 13269
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRect(HandleRef hWnd, NativeMethods.COMRECT rect, bool erase);

		// Token: 0x060033D6 RID: 13270
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool InvalidateRgn(HandleRef hWnd, HandleRef hrgn, bool erase);

		// Token: 0x060033D7 RID: 13271
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UpdateWindow(HandleRef hWnd);

		// Token: 0x060033D8 RID: 13272
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentProcessId();

		// Token: 0x060033D9 RID: 13273
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ScrollWindowEx(HandleRef hWnd, int nXAmount, int nYAmount, NativeMethods.COMRECT rectScrollRegion, ref NativeMethods.RECT rectClip, HandleRef hrgnUpdate, ref NativeMethods.RECT prcUpdate, int flags);

		// Token: 0x060033DA RID: 13274
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetThreadLocale();

		// Token: 0x060033DB RID: 13275
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool MessageBeep(int type);

		// Token: 0x060033DC RID: 13276
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawMenuBar(HandleRef hWnd);

		// Token: 0x060033DD RID: 13277
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef parent, HandleRef child);

		// Token: 0x060033DE RID: 13278
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetTimer(HandleRef hWnd, int nIDEvent, int uElapse, IntPtr lpTimerFunc);

		// Token: 0x060033DF RID: 13279
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool KillTimer(HandleRef hwnd, int idEvent);

		// Token: 0x060033E0 RID: 13280
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int MessageBox(HandleRef hWnd, string text, string caption, int type);

		// Token: 0x060033E1 RID: 13281
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		// Token: 0x060033E2 RID: 13282
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTickCount();

		// Token: 0x060033E3 RID: 13283
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ScrollWindow(HandleRef hWnd, int nXAmount, int nYAmount, ref NativeMethods.RECT rectScrollRegion, ref NativeMethods.RECT rectClip);

		// Token: 0x060033E4 RID: 13284
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCurrentProcess();

		// Token: 0x060033E5 RID: 13285
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCurrentThread();

		// Token: 0x060033E6 RID: 13286
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetThreadLocale(int Locale);

		// Token: 0x060033E7 RID: 13287
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowUnicode(HandleRef hWnd);

		// Token: 0x060033E8 RID: 13288
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawEdge(HandleRef hDC, ref NativeMethods.RECT rect, int edge, int flags);

		// Token: 0x060033E9 RID: 13289
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawFrameControl(HandleRef hDC, ref NativeMethods.RECT rect, int type, int state);

		// Token: 0x060033EA RID: 13290
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x060033EB RID: 13291
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetRgnBox(HandleRef hRegion, ref NativeMethods.RECT clipRect);

		// Token: 0x060033EC RID: 13292
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x060033ED RID: 13293
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x060033EE RID: 13294
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawIcon(HandleRef hDC, int x, int y, HandleRef hIcon);

		// Token: 0x060033EF RID: 13295
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool DrawIconEx(HandleRef hDC, int x, int y, HandleRef hIcon, int width, int height, int iStepIfAniCursor, HandleRef hBrushFlickerFree, int diFlags);

		// Token: 0x060033F0 RID: 13296
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkMode(HandleRef hDC, int nBkMode);

		// Token: 0x060033F1 RID: 13297
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

		// Token: 0x060033F2 RID: 13298
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ShowCaret(HandleRef hWnd);

		// Token: 0x060033F3 RID: 13299
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool HideCaret(HandleRef hWnd);

		// Token: 0x060033F4 RID: 13300
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint GetCaretBlinkTime();

		// Token: 0x060033F5 RID: 13301
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsAppThemed();

		// Token: 0x060033F6 RID: 13302
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeAppProperties();

		// Token: 0x060033F7 RID: 13303
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern void SetThemeAppProperties(int Flags);

		// Token: 0x060033F8 RID: 13304
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr OpenThemeData(HandleRef hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszClassList);

		// Token: 0x060033F9 RID: 13305
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int CloseThemeData(HandleRef hTheme);

		// Token: 0x060033FA RID: 13306
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		// Token: 0x060033FB RID: 13307
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsThemePartDefined(HandleRef hTheme, int iPartId, int iStateId);

		// Token: 0x060033FC RID: 13308
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeBackground(HandleRef hTheme, HandleRef hdc, int partId, int stateId, [In] NativeMethods.COMRECT pRect, [In] NativeMethods.COMRECT pClipRect);

		// Token: 0x060033FD RID: 13309
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeEdge(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pDestRect, int uEdge, int uFlags, [Out] NativeMethods.COMRECT pContentRect);

		// Token: 0x060033FE RID: 13310
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeParentBackground(HandleRef hwnd, HandleRef hdc, [In] NativeMethods.COMRECT prc);

		// Token: 0x060033FF RID: 13311
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int DrawThemeText(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [MarshalAs(UnmanagedType.LPWStr)] string pszText, int iCharCount, int dwTextFlags, int dwTextFlags2, [In] NativeMethods.COMRECT pRect);

		// Token: 0x06003400 RID: 13312
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundContentRect(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pBoundingRect, [Out] NativeMethods.COMRECT pContentRect);

		// Token: 0x06003401 RID: 13313
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundExtent(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pContentRect, [Out] NativeMethods.COMRECT pExtentRect);

		// Token: 0x06003402 RID: 13314
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBackgroundRegion(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT pRect, ref IntPtr pRegion);

		// Token: 0x06003403 RID: 13315
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeBool(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref bool pfVal);

		// Token: 0x06003404 RID: 13316
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeColor(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int pColor);

		// Token: 0x06003405 RID: 13317
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeEnumValue(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

		// Token: 0x06003406 RID: 13318
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeFilename(HandleRef hTheme, int iPartId, int iStateId, int iPropId, StringBuilder pszThemeFilename, int cchMaxBuffChars);

		// Token: 0x06003407 RID: 13319
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeFont(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, int iPropId, NativeMethods.LOGFONT pFont);

		// Token: 0x06003408 RID: 13320
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeInt(HandleRef hTheme, int iPartId, int iStateId, int iPropId, ref int piVal);

		// Token: 0x06003409 RID: 13321
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemePartSize(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [In] NativeMethods.COMRECT prc, ThemeSizeType eSize, [Out] NativeMethods.SIZE psz);

		// Token: 0x0600340A RID: 13322
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemePosition(HandleRef hTheme, int iPartId, int iStateId, int iPropId, [Out] NativeMethods.POINT pPoint);

		// Token: 0x0600340B RID: 13323
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeMargins(HandleRef hTheme, HandleRef hDC, int iPartId, int iStateId, int iPropId, ref NativeMethods.MARGINS margins);

		// Token: 0x0600340C RID: 13324
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeString(HandleRef hTheme, int iPartId, int iStateId, int iPropId, StringBuilder pszBuff, int cchMaxBuffChars);

		// Token: 0x0600340D RID: 13325
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeDocumentationProperty([MarshalAs(UnmanagedType.LPWStr)] string pszThemeName, [MarshalAs(UnmanagedType.LPWStr)] string pszPropertyName, StringBuilder pszValueBuff, int cchMaxValChars);

		// Token: 0x0600340E RID: 13326
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeTextExtent(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, [MarshalAs(UnmanagedType.LPWStr)] string pszText, int iCharCount, int dwTextFlags, [In] NativeMethods.COMRECT pBoundingRect, [Out] NativeMethods.COMRECT pExtentRect);

		// Token: 0x0600340F RID: 13327
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeTextMetrics(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, ref TextMetrics ptm);

		// Token: 0x06003410 RID: 13328
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int HitTestThemeBackground(HandleRef hTheme, HandleRef hdc, int iPartId, int iStateId, int dwOptions, [In] NativeMethods.COMRECT pRect, HandleRef hrgn, [In] NativeMethods.POINTSTRUCT ptTest, ref int pwHitTestCode);

		// Token: 0x06003411 RID: 13329
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool IsThemeBackgroundPartiallyTransparent(HandleRef hTheme, int iPartId, int iStateId);

		// Token: 0x06003412 RID: 13330
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern bool GetThemeSysBool(HandleRef hTheme, int iBoolId);

		// Token: 0x06003413 RID: 13331
		[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
		public static extern int GetThemeSysInt(HandleRef hTheme, int iIntId, ref int piValue);

		// Token: 0x06003414 RID: 13332
		[DllImport("user32.dll")]
		public static extern IntPtr OpenInputDesktop(int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess);

		// Token: 0x06003415 RID: 13333
		[DllImport("user32.dll")]
		public static extern bool CloseDesktop(IntPtr hDesktop);

		// Token: 0x06003416 RID: 13334
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetProcessDPIAware();

		// Token: 0x06003417 RID: 13335
		[DllImport("SHCore.dll", SetLastError = true)]
		public static extern int SetProcessDpiAwareness(NativeMethods.PROCESS_DPI_AWARENESS awareness);

		// Token: 0x06003418 RID: 13336
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

		// Token: 0x06003419 RID: 13337
		[DllImport("user32.dll")]
		public static extern int GetThreadDpiAwarenessContext();

		// Token: 0x0600341A RID: 13338
		[DllImport("user32.dll")]
		public static extern bool AreDpiAwarenessContextsEqual(int dpiContextA, int dpiContextB);

		// Token: 0x0600341B RID: 13339 RVA: 0x000EF07C File Offset: 0x000ED27C
		public static int RGBToCOLORREF(int rgbValue)
		{
			int num = (rgbValue & 255) << 16;
			rgbValue &= 16776960;
			rgbValue |= (rgbValue >> 16 & 255);
			rgbValue &= 65535;
			rgbValue |= num;
			return rgbValue;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000EF0BC File Offset: 0x000ED2BC
		public static Color ColorFromCOLORREF(int colorref)
		{
			int red = colorref & 255;
			int green = colorref >> 8 & 255;
			int blue = colorref >> 16 & 255;
			return Color.FromArgb(red, green, blue);
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000EF0EE File Offset: 0x000ED2EE
		public static int ColorToCOLORREF(Color color)
		{
			return (int)color.R | (int)color.G << 8 | (int)color.B << 16;
		}

		// Token: 0x02000710 RID: 1808
		// (Invoke) Token: 0x06005FE9 RID: 24553
		internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		// Token: 0x02000711 RID: 1809
		[Guid("BEF6E003-A874-101A-8BBA-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[ComImport]
		public interface IFontDisp
		{
			// Token: 0x170016EC RID: 5868
			// (get) Token: 0x06005FEC RID: 24556
			// (set) Token: 0x06005FED RID: 24557
			string Name { get; set; }

			// Token: 0x170016ED RID: 5869
			// (get) Token: 0x06005FEE RID: 24558
			// (set) Token: 0x06005FEF RID: 24559
			long Size { get; set; }

			// Token: 0x170016EE RID: 5870
			// (get) Token: 0x06005FF0 RID: 24560
			// (set) Token: 0x06005FF1 RID: 24561
			bool Bold { get; set; }

			// Token: 0x170016EF RID: 5871
			// (get) Token: 0x06005FF2 RID: 24562
			// (set) Token: 0x06005FF3 RID: 24563
			bool Italic { get; set; }

			// Token: 0x170016F0 RID: 5872
			// (get) Token: 0x06005FF4 RID: 24564
			// (set) Token: 0x06005FF5 RID: 24565
			bool Underline { get; set; }

			// Token: 0x170016F1 RID: 5873
			// (get) Token: 0x06005FF6 RID: 24566
			// (set) Token: 0x06005FF7 RID: 24567
			bool Strikethrough { get; set; }

			// Token: 0x170016F2 RID: 5874
			// (get) Token: 0x06005FF8 RID: 24568
			// (set) Token: 0x06005FF9 RID: 24569
			short Weight { get; set; }

			// Token: 0x170016F3 RID: 5875
			// (get) Token: 0x06005FFA RID: 24570
			// (set) Token: 0x06005FFB RID: 24571
			short Charset { get; set; }
		}
	}
}
