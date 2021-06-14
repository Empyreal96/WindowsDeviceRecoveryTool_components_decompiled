using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Drawing.Text;
using System.Internal;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x0200003E RID: 62
	[SuppressUnmanagedCodeSecurity]
	internal class SafeNativeMethods
	{
		// Token: 0x0600060D RID: 1549
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleBitmap(HandleRef hDC, int width, int height);

		// Token: 0x0600060E RID: 1550 RVA: 0x0001A5FB File Offset: 0x000187FB
		public static IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateCompatibleBitmap(hDC, width, height), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600060F RID: 1551
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateBitmap(int width, int height, int planes, int bpp, IntPtr bitmapData);

		// Token: 0x06000610 RID: 1552 RVA: 0x0001A60F File Offset: 0x0001880F
		public static IntPtr CreateBitmap(int width, int height, int planes, int bpp, IntPtr bitmapData)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmap(width, height, planes, bpp, bitmapData), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000611 RID: 1553
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

		// Token: 0x06000612 RID: 1554
		[DllImport("gdi32.dll")]
		public static extern int GetDIBits(HandleRef hdc, HandleRef hbm, int arg1, int arg2, IntPtr arg3, ref NativeMethods.BITMAPINFO_FLAT bmi, int arg5);

		// Token: 0x06000613 RID: 1555
		[DllImport("gdi32.dll")]
		public static extern uint GetPaletteEntries(HandleRef hpal, int iStartIndex, int nEntries, byte[] lppe);

		// Token: 0x06000614 RID: 1556
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDIBSection", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateDIBSection(HandleRef hdc, ref NativeMethods.BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x06000615 RID: 1557 RVA: 0x0001A626 File Offset: 0x00018826
		public static IntPtr CreateDIBSection(HandleRef hdc, ref NativeMethods.BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateDIBSection(hdc, ref bmi, iUsage, ref ppvBits, hSection, dwOffset), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000616 RID: 1558
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GlobalFree(HandleRef handle);

		// Token: 0x06000617 RID: 1559
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int StartDoc(HandleRef hDC, SafeNativeMethods.DOCINFO lpDocInfo);

		// Token: 0x06000618 RID: 1560
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int StartPage(HandleRef hDC);

		// Token: 0x06000619 RID: 1561
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int EndPage(HandleRef hDC);

		// Token: 0x0600061A RID: 1562
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int AbortDoc(HandleRef hDC);

		// Token: 0x0600061B RID: 1563
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int EndDoc(HandleRef hDC);

		// Token: 0x0600061C RID: 1564
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PrintDlg([In] [Out] SafeNativeMethods.PRINTDLG lppd);

		// Token: 0x0600061D RID: 1565
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PrintDlg([In] [Out] SafeNativeMethods.PRINTDLGX86 lppd);

		// Token: 0x0600061E RID: 1566
		[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DeviceCapabilities(string pDevice, string pPort, short fwCapabilities, IntPtr pOutput, IntPtr pDevMode);

		// Token: 0x0600061F RID: 1567
		[DllImport("winspool.drv", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DocumentProperties(HandleRef hwnd, HandleRef hPrinter, string pDeviceName, IntPtr pDevModeOutput, HandleRef pDevModeInput, int fMode);

		// Token: 0x06000620 RID: 1568
		[DllImport("winspool.drv", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DocumentProperties(HandleRef hwnd, HandleRef hPrinter, string pDeviceName, IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

		// Token: 0x06000621 RID: 1569
		[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int EnumPrinters(int flags, string name, int level, IntPtr pPrinterEnum, int cbBuf, out int pcbNeeded, out int pcReturned);

		// Token: 0x06000622 RID: 1570
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GlobalLock(HandleRef handle);

		// Token: 0x06000623 RID: 1571
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ResetDC(HandleRef hDC, HandleRef lpDevMode);

		// Token: 0x06000624 RID: 1572
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GlobalUnlock(HandleRef handle);

		// Token: 0x06000625 RID: 1573
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x06000626 RID: 1574 RVA: 0x0001A63F File Offset: 0x0001883F
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000627 RID: 1575
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000628 RID: 1576
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000629 RID: 1577
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int AddFontResourceEx(string lpszFilename, int fl, IntPtr pdv);

		// Token: 0x0600062A RID: 1578 RVA: 0x0001A654 File Offset: 0x00018854
		public static int AddFontFile(string fileName)
		{
			if (Marshal.SystemDefaultCharSize == 1)
			{
				return 0;
			}
			return SafeNativeMethods.AddFontResourceEx(fileName, 16, IntPtr.Zero);
		}

		// Token: 0x0600062B RID: 1579
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int RemoveFontResourceEx(string lpszFilename, int fl, IntPtr pdv);

		// Token: 0x0600062C RID: 1580 RVA: 0x0001A66D File Offset: 0x0001886D
		public static int RemoveFontFile(string fileName)
		{
			return SafeNativeMethods.RemoveFontResourceEx(fileName, 16, IntPtr.Zero);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001A67C File Offset: 0x0001887C
		internal static IntPtr SaveClipRgn(IntPtr hDC)
		{
			IntPtr intPtr = SafeNativeMethods.CreateRectRgn(0, 0, 0, 0);
			IntPtr result = IntPtr.Zero;
			try
			{
				int clipRgn = SafeNativeMethods.GetClipRgn(new HandleRef(null, hDC), new HandleRef(null, intPtr));
				if (clipRgn > 0)
				{
					result = intPtr;
					intPtr = IntPtr.Zero;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}
			return result;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001A6E8 File Offset: 0x000188E8
		internal static void RestoreClipRgn(IntPtr hDC, IntPtr hRgn)
		{
			try
			{
				SafeNativeMethods.SelectClipRgn(new HandleRef(null, hDC), new HandleRef(null, hRgn));
			}
			finally
			{
				if (hRgn != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, hRgn));
				}
			}
		}

		// Token: 0x0600062F RID: 1583
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ExtEscape(HandleRef hDC, int nEscape, int cbInput, ref int inData, int cbOutput, out int outData);

		// Token: 0x06000630 RID: 1584
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ExtEscape(HandleRef hDC, int nEscape, int cbInput, byte[] inData, int cbOutput, out int outData);

		// Token: 0x06000631 RID: 1585
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int IntersectClipRect(HandleRef hDC, int x1, int y1, int x2, int y2);

		// Token: 0x06000632 RID: 1586
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GlobalAlloc", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGlobalAlloc(int uFlags, UIntPtr dwBytes);

		// Token: 0x06000633 RID: 1587 RVA: 0x0001A738 File Offset: 0x00018938
		public static IntPtr GlobalAlloc(int uFlags, uint dwBytes)
		{
			return SafeNativeMethods.IntGlobalAlloc(uFlags, new UIntPtr(dwBytes));
		}

		// Token: 0x06000634 RID: 1588
		[DllImport("kernel32.dll")]
		internal static extern void ZeroMemory(IntPtr destination, UIntPtr length);

		// Token: 0x06000635 RID: 1589
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		internal static extern int IntDeleteObject(HandleRef hObject);

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A746 File Offset: 0x00018946
		public static int DeleteObject(HandleRef hObject)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hObject, SafeNativeMethods.CommonHandles.GDI);
			return SafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06000637 RID: 1591
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(HandleRef hdc, HandleRef obj);

		// Token: 0x06000638 RID: 1592
		[DllImport("user32.dll", EntryPoint = "CreateIconFromResourceEx", SetLastError = true)]
		private unsafe static extern IntPtr IntCreateIconFromResourceEx(byte* pbIconBits, int cbIconBits, bool fIcon, int dwVersion, int csDesired, int cyDesired, int flags);

		// Token: 0x06000639 RID: 1593 RVA: 0x0001A75F File Offset: 0x0001895F
		public unsafe static IntPtr CreateIconFromResourceEx(byte* pbIconBits, int cbIconBits, bool fIcon, int dwVersion, int csDesired, int cyDesired, int flags)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateIconFromResourceEx(pbIconBits, cbIconBits, fIcon, dwVersion, csDesired, cyDesired, flags), SafeNativeMethods.CommonHandles.Icon);
		}

		// Token: 0x0600063A RID: 1594
		[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "ExtractAssociatedIcon")]
		public static extern IntPtr IntExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);

		// Token: 0x0600063B RID: 1595 RVA: 0x0001A77A File Offset: 0x0001897A
		public static IntPtr ExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index)
		{
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntExtractAssociatedIcon(hInst, iconPath, ref index), SafeNativeMethods.CommonHandles.Icon);
		}

		// Token: 0x0600063C RID: 1596
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadIcon", SetLastError = true)]
		private static extern IntPtr IntLoadIcon(HandleRef hInst, IntPtr iconId);

		// Token: 0x0600063D RID: 1597 RVA: 0x0001A78E File Offset: 0x0001898E
		public static IntPtr LoadIcon(HandleRef hInst, int iconId)
		{
			return SafeNativeMethods.IntLoadIcon(hInst, new IntPtr(iconId));
		}

		// Token: 0x0600063E RID: 1598
		[DllImport("comctl32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadIconWithScaleDown", SetLastError = true)]
		private static extern int IntLoadIconWithScaleDown(HandleRef hInst, IntPtr iconId, int cx, int cy, ref IntPtr phico);

		// Token: 0x0600063F RID: 1599 RVA: 0x0001A79C File Offset: 0x0001899C
		public static int LoadIconWithScaleDown(HandleRef hInst, int iconId, int cx, int cy, ref IntPtr phico)
		{
			return SafeNativeMethods.IntLoadIconWithScaleDown(hInst, new IntPtr(iconId), cx, cy, ref phico);
		}

		// Token: 0x06000640 RID: 1600
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyIcon", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntDestroyIcon(HandleRef hIcon);

		// Token: 0x06000641 RID: 1601 RVA: 0x0001A7AE File Offset: 0x000189AE
		public static bool DestroyIcon(HandleRef hIcon)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hIcon, SafeNativeMethods.CommonHandles.Icon);
			return SafeNativeMethods.IntDestroyIcon(hIcon);
		}

		// Token: 0x06000642 RID: 1602
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CopyImage", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags);

		// Token: 0x06000643 RID: 1603 RVA: 0x0001A7C8 File Offset: 0x000189C8
		public static IntPtr CopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			int type;
			if (uType == 1)
			{
				type = SafeNativeMethods.CommonHandles.Icon;
			}
			else
			{
				type = SafeNativeMethods.CommonHandles.GDI;
			}
			return System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), type);
		}

		// Token: 0x06000644 RID: 1604
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] SafeNativeMethods.BITMAP bm);

		// Token: 0x06000645 RID: 1605
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] SafeNativeMethods.LOGFONT lf);

		// Token: 0x06000646 RID: 1606 RVA: 0x0001A7F8 File Offset: 0x000189F8
		public static int GetObject(HandleRef hObject, SafeNativeMethods.LOGFONT lp)
		{
			return SafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(SafeNativeMethods.LOGFONT)), lp);
		}

		// Token: 0x06000647 RID: 1607
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GetIconInfo(HandleRef hIcon, [In] [Out] SafeNativeMethods.ICONINFO info);

		// Token: 0x06000648 RID: 1608
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetSysColor(int nIndex);

		// Token: 0x06000649 RID: 1609
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool DrawIconEx(HandleRef hDC, int x, int y, HandleRef hIcon, int width, int height, int iStepIfAniCursor, HandleRef hBrushFlickerFree, int diFlags);

		// Token: 0x0600064A RID: 1610
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern SafeNativeMethods.IPicture OleCreatePictureIndirect(SafeNativeMethods.PICTDESC pictdesc, [In] ref Guid refiid, bool fOwn);

		// Token: 0x0400034B RID: 843
		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		// Token: 0x0400034C RID: 844
		public const int ERROR_CANCELLED = 1223;

		// Token: 0x0400034D RID: 845
		public const int RASTERCAPS = 38;

		// Token: 0x0400034E RID: 846
		public const int RC_PALETTE = 256;

		// Token: 0x0400034F RID: 847
		public const int SIZEPALETTE = 104;

		// Token: 0x04000350 RID: 848
		public const int SYSPAL_STATIC = 1;

		// Token: 0x04000351 RID: 849
		public const int BS_SOLID = 0;

		// Token: 0x04000352 RID: 850
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x04000353 RID: 851
		public const int R2_BLACK = 1;

		// Token: 0x04000354 RID: 852
		public const int R2_NOTMERGEPEN = 2;

		// Token: 0x04000355 RID: 853
		public const int R2_MASKNOTPEN = 3;

		// Token: 0x04000356 RID: 854
		public const int R2_NOTCOPYPEN = 4;

		// Token: 0x04000357 RID: 855
		public const int R2_MASKPENNOT = 5;

		// Token: 0x04000358 RID: 856
		public const int R2_NOT = 6;

		// Token: 0x04000359 RID: 857
		public const int R2_XORPEN = 7;

		// Token: 0x0400035A RID: 858
		public const int R2_NOTMASKPEN = 8;

		// Token: 0x0400035B RID: 859
		public const int R2_MASKPEN = 9;

		// Token: 0x0400035C RID: 860
		public const int R2_NOTXORPEN = 10;

		// Token: 0x0400035D RID: 861
		public const int R2_NOP = 11;

		// Token: 0x0400035E RID: 862
		public const int R2_MERGENOTPEN = 12;

		// Token: 0x0400035F RID: 863
		public const int R2_COPYPEN = 13;

		// Token: 0x04000360 RID: 864
		public const int R2_MERGEPENNOT = 14;

		// Token: 0x04000361 RID: 865
		public const int R2_MERGEPEN = 15;

		// Token: 0x04000362 RID: 866
		public const int R2_WHITE = 16;

		// Token: 0x04000363 RID: 867
		public const int UOI_FLAGS = 1;

		// Token: 0x04000364 RID: 868
		public const int WSF_VISIBLE = 1;

		// Token: 0x04000365 RID: 869
		public const int E_UNEXPECTED = -2147418113;

		// Token: 0x04000366 RID: 870
		public const int E_NOTIMPL = -2147467263;

		// Token: 0x04000367 RID: 871
		public const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x04000368 RID: 872
		public const int E_INVALIDARG = -2147024809;

		// Token: 0x04000369 RID: 873
		public const int E_NOINTERFACE = -2147467262;

		// Token: 0x0400036A RID: 874
		public const int E_POINTER = -2147467261;

		// Token: 0x0400036B RID: 875
		public const int E_HANDLE = -2147024890;

		// Token: 0x0400036C RID: 876
		public const int E_ABORT = -2147467260;

		// Token: 0x0400036D RID: 877
		public const int E_FAIL = -2147467259;

		// Token: 0x0400036E RID: 878
		public const int E_ACCESSDENIED = -2147024891;

		// Token: 0x0400036F RID: 879
		public const int PM_NOREMOVE = 0;

		// Token: 0x04000370 RID: 880
		public const int PM_REMOVE = 1;

		// Token: 0x04000371 RID: 881
		public const int PM_NOYIELD = 2;

		// Token: 0x04000372 RID: 882
		public const int GMEM_FIXED = 0;

		// Token: 0x04000373 RID: 883
		public const int GMEM_MOVEABLE = 2;

		// Token: 0x04000374 RID: 884
		public const int GMEM_NOCOMPACT = 16;

		// Token: 0x04000375 RID: 885
		public const int GMEM_NODISCARD = 32;

		// Token: 0x04000376 RID: 886
		public const int GMEM_ZEROINIT = 64;

		// Token: 0x04000377 RID: 887
		public const int GMEM_MODIFY = 128;

		// Token: 0x04000378 RID: 888
		public const int GMEM_DISCARDABLE = 256;

		// Token: 0x04000379 RID: 889
		public const int GMEM_NOT_BANKED = 4096;

		// Token: 0x0400037A RID: 890
		public const int GMEM_SHARE = 8192;

		// Token: 0x0400037B RID: 891
		public const int GMEM_DDESHARE = 8192;

		// Token: 0x0400037C RID: 892
		public const int GMEM_NOTIFY = 16384;

		// Token: 0x0400037D RID: 893
		public const int GMEM_LOWER = 4096;

		// Token: 0x0400037E RID: 894
		public const int GMEM_VALID_FLAGS = 32626;

		// Token: 0x0400037F RID: 895
		public const int GMEM_INVALID_HANDLE = 32768;

		// Token: 0x04000380 RID: 896
		public const int DM_UPDATE = 1;

		// Token: 0x04000381 RID: 897
		public const int DM_COPY = 2;

		// Token: 0x04000382 RID: 898
		public const int DM_PROMPT = 4;

		// Token: 0x04000383 RID: 899
		public const int DM_MODIFY = 8;

		// Token: 0x04000384 RID: 900
		public const int DM_IN_BUFFER = 8;

		// Token: 0x04000385 RID: 901
		public const int DM_IN_PROMPT = 4;

		// Token: 0x04000386 RID: 902
		public const int DM_OUT_BUFFER = 2;

		// Token: 0x04000387 RID: 903
		public const int DM_OUT_DEFAULT = 1;

		// Token: 0x04000388 RID: 904
		public const int DT_PLOTTER = 0;

		// Token: 0x04000389 RID: 905
		public const int DT_RASDISPLAY = 1;

		// Token: 0x0400038A RID: 906
		public const int DT_RASPRINTER = 2;

		// Token: 0x0400038B RID: 907
		public const int DT_RASCAMERA = 3;

		// Token: 0x0400038C RID: 908
		public const int DT_CHARSTREAM = 4;

		// Token: 0x0400038D RID: 909
		public const int DT_METAFILE = 5;

		// Token: 0x0400038E RID: 910
		public const int DT_DISPFILE = 6;

		// Token: 0x0400038F RID: 911
		public const int TECHNOLOGY = 2;

		// Token: 0x04000390 RID: 912
		public const int DC_FIELDS = 1;

		// Token: 0x04000391 RID: 913
		public const int DC_PAPERS = 2;

		// Token: 0x04000392 RID: 914
		public const int DC_PAPERSIZE = 3;

		// Token: 0x04000393 RID: 915
		public const int DC_MINEXTENT = 4;

		// Token: 0x04000394 RID: 916
		public const int DC_MAXEXTENT = 5;

		// Token: 0x04000395 RID: 917
		public const int DC_BINS = 6;

		// Token: 0x04000396 RID: 918
		public const int DC_DUPLEX = 7;

		// Token: 0x04000397 RID: 919
		public const int DC_SIZE = 8;

		// Token: 0x04000398 RID: 920
		public const int DC_EXTRA = 9;

		// Token: 0x04000399 RID: 921
		public const int DC_VERSION = 10;

		// Token: 0x0400039A RID: 922
		public const int DC_DRIVER = 11;

		// Token: 0x0400039B RID: 923
		public const int DC_BINNAMES = 12;

		// Token: 0x0400039C RID: 924
		public const int DC_ENUMRESOLUTIONS = 13;

		// Token: 0x0400039D RID: 925
		public const int DC_FILEDEPENDENCIES = 14;

		// Token: 0x0400039E RID: 926
		public const int DC_TRUETYPE = 15;

		// Token: 0x0400039F RID: 927
		public const int DC_PAPERNAMES = 16;

		// Token: 0x040003A0 RID: 928
		public const int DC_ORIENTATION = 17;

		// Token: 0x040003A1 RID: 929
		public const int DC_COPIES = 18;

		// Token: 0x040003A2 RID: 930
		public const int PD_ALLPAGES = 0;

		// Token: 0x040003A3 RID: 931
		public const int PD_SELECTION = 1;

		// Token: 0x040003A4 RID: 932
		public const int PD_PAGENUMS = 2;

		// Token: 0x040003A5 RID: 933
		public const int PD_CURRENTPAGE = 4194304;

		// Token: 0x040003A6 RID: 934
		public const int PD_NOSELECTION = 4;

		// Token: 0x040003A7 RID: 935
		public const int PD_NOPAGENUMS = 8;

		// Token: 0x040003A8 RID: 936
		public const int PD_NOCURRENTPAGE = 8388608;

		// Token: 0x040003A9 RID: 937
		public const int PD_COLLATE = 16;

		// Token: 0x040003AA RID: 938
		public const int PD_PRINTTOFILE = 32;

		// Token: 0x040003AB RID: 939
		public const int PD_PRINTSETUP = 64;

		// Token: 0x040003AC RID: 940
		public const int PD_NOWARNING = 128;

		// Token: 0x040003AD RID: 941
		public const int PD_RETURNDC = 256;

		// Token: 0x040003AE RID: 942
		public const int PD_RETURNIC = 512;

		// Token: 0x040003AF RID: 943
		public const int PD_RETURNDEFAULT = 1024;

		// Token: 0x040003B0 RID: 944
		public const int PD_SHOWHELP = 2048;

		// Token: 0x040003B1 RID: 945
		public const int PD_ENABLEPRINTHOOK = 4096;

		// Token: 0x040003B2 RID: 946
		public const int PD_ENABLESETUPHOOK = 8192;

		// Token: 0x040003B3 RID: 947
		public const int PD_ENABLEPRINTTEMPLATE = 16384;

		// Token: 0x040003B4 RID: 948
		public const int PD_ENABLESETUPTEMPLATE = 32768;

		// Token: 0x040003B5 RID: 949
		public const int PD_ENABLEPRINTTEMPLATEHANDLE = 65536;

		// Token: 0x040003B6 RID: 950
		public const int PD_ENABLESETUPTEMPLATEHANDLE = 131072;

		// Token: 0x040003B7 RID: 951
		public const int PD_USEDEVMODECOPIES = 262144;

		// Token: 0x040003B8 RID: 952
		public const int PD_USEDEVMODECOPIESANDCOLLATE = 262144;

		// Token: 0x040003B9 RID: 953
		public const int PD_DISABLEPRINTTOFILE = 524288;

		// Token: 0x040003BA RID: 954
		public const int PD_HIDEPRINTTOFILE = 1048576;

		// Token: 0x040003BB RID: 955
		public const int PD_NONETWORKBUTTON = 2097152;

		// Token: 0x040003BC RID: 956
		public const int DI_MASK = 1;

		// Token: 0x040003BD RID: 957
		public const int DI_IMAGE = 2;

		// Token: 0x040003BE RID: 958
		public const int DI_NORMAL = 3;

		// Token: 0x040003BF RID: 959
		public const int DI_COMPAT = 4;

		// Token: 0x040003C0 RID: 960
		public const int DI_DEFAULTSIZE = 8;

		// Token: 0x040003C1 RID: 961
		public const int IDC_ARROW = 32512;

		// Token: 0x040003C2 RID: 962
		public const int IDC_IBEAM = 32513;

		// Token: 0x040003C3 RID: 963
		public const int IDC_WAIT = 32514;

		// Token: 0x040003C4 RID: 964
		public const int IDC_CROSS = 32515;

		// Token: 0x040003C5 RID: 965
		public const int IDC_UPARROW = 32516;

		// Token: 0x040003C6 RID: 966
		public const int IDC_SIZE = 32640;

		// Token: 0x040003C7 RID: 967
		public const int IDC_ICON = 32641;

		// Token: 0x040003C8 RID: 968
		public const int IDC_SIZENWSE = 32642;

		// Token: 0x040003C9 RID: 969
		public const int IDC_SIZENESW = 32643;

		// Token: 0x040003CA RID: 970
		public const int IDC_SIZEWE = 32644;

		// Token: 0x040003CB RID: 971
		public const int IDC_SIZENS = 32645;

		// Token: 0x040003CC RID: 972
		public const int IDC_SIZEALL = 32646;

		// Token: 0x040003CD RID: 973
		public const int IDC_NO = 32648;

		// Token: 0x040003CE RID: 974
		public const int IDC_APPSTARTING = 32650;

		// Token: 0x040003CF RID: 975
		public const int IDC_HELP = 32651;

		// Token: 0x040003D0 RID: 976
		public const int IMAGE_BITMAP = 0;

		// Token: 0x040003D1 RID: 977
		public const int IMAGE_ICON = 1;

		// Token: 0x040003D2 RID: 978
		public const int IMAGE_CURSOR = 2;

		// Token: 0x040003D3 RID: 979
		public const int IMAGE_ENHMETAFILE = 3;

		// Token: 0x040003D4 RID: 980
		public const int IDI_APPLICATION = 32512;

		// Token: 0x040003D5 RID: 981
		public const int IDI_HAND = 32513;

		// Token: 0x040003D6 RID: 982
		public const int IDI_QUESTION = 32514;

		// Token: 0x040003D7 RID: 983
		public const int IDI_EXCLAMATION = 32515;

		// Token: 0x040003D8 RID: 984
		public const int IDI_ASTERISK = 32516;

		// Token: 0x040003D9 RID: 985
		public const int IDI_WINLOGO = 32517;

		// Token: 0x040003DA RID: 986
		public const int IDI_WARNING = 32515;

		// Token: 0x040003DB RID: 987
		public const int IDI_ERROR = 32513;

		// Token: 0x040003DC RID: 988
		public const int IDI_INFORMATION = 32516;

		// Token: 0x040003DD RID: 989
		public const int IDI_SHIELD = 32518;

		// Token: 0x040003DE RID: 990
		public const int SRCCOPY = 13369376;

		// Token: 0x040003DF RID: 991
		public const int PLANES = 14;

		// Token: 0x040003E0 RID: 992
		public const int PS_SOLID = 0;

		// Token: 0x040003E1 RID: 993
		public const int PS_DASH = 1;

		// Token: 0x040003E2 RID: 994
		public const int PS_DOT = 2;

		// Token: 0x040003E3 RID: 995
		public const int PS_DASHDOT = 3;

		// Token: 0x040003E4 RID: 996
		public const int PS_DASHDOTDOT = 4;

		// Token: 0x040003E5 RID: 997
		public const int PS_NULL = 5;

		// Token: 0x040003E6 RID: 998
		public const int PS_INSIDEFRAME = 6;

		// Token: 0x040003E7 RID: 999
		public const int PS_USERSTYLE = 7;

		// Token: 0x040003E8 RID: 1000
		public const int PS_ALTERNATE = 8;

		// Token: 0x040003E9 RID: 1001
		public const int PS_STYLE_MASK = 15;

		// Token: 0x040003EA RID: 1002
		public const int PS_ENDCAP_ROUND = 0;

		// Token: 0x040003EB RID: 1003
		public const int PS_ENDCAP_SQUARE = 256;

		// Token: 0x040003EC RID: 1004
		public const int PS_ENDCAP_FLAT = 512;

		// Token: 0x040003ED RID: 1005
		public const int PS_ENDCAP_MASK = 3840;

		// Token: 0x040003EE RID: 1006
		public const int PS_JOIN_ROUND = 0;

		// Token: 0x040003EF RID: 1007
		public const int PS_JOIN_BEVEL = 4096;

		// Token: 0x040003F0 RID: 1008
		public const int PS_JOIN_MITER = 8192;

		// Token: 0x040003F1 RID: 1009
		public const int PS_JOIN_MASK = 61440;

		// Token: 0x040003F2 RID: 1010
		public const int PS_COSMETIC = 0;

		// Token: 0x040003F3 RID: 1011
		public const int PS_GEOMETRIC = 65536;

		// Token: 0x040003F4 RID: 1012
		public const int PS_TYPE_MASK = 983040;

		// Token: 0x040003F5 RID: 1013
		public const int BITSPIXEL = 12;

		// Token: 0x040003F6 RID: 1014
		public const int ALTERNATE = 1;

		// Token: 0x040003F7 RID: 1015
		public const int LOGPIXELSX = 88;

		// Token: 0x040003F8 RID: 1016
		public const int LOGPIXELSY = 90;

		// Token: 0x040003F9 RID: 1017
		public const int PHYSICALWIDTH = 110;

		// Token: 0x040003FA RID: 1018
		public const int PHYSICALHEIGHT = 111;

		// Token: 0x040003FB RID: 1019
		public const int PHYSICALOFFSETX = 112;

		// Token: 0x040003FC RID: 1020
		public const int PHYSICALOFFSETY = 113;

		// Token: 0x040003FD RID: 1021
		public const int WINDING = 2;

		// Token: 0x040003FE RID: 1022
		public const int VERTRES = 10;

		// Token: 0x040003FF RID: 1023
		public const int HORZRES = 8;

		// Token: 0x04000400 RID: 1024
		public const int DM_SPECVERSION = 1025;

		// Token: 0x04000401 RID: 1025
		public const int DM_ORIENTATION = 1;

		// Token: 0x04000402 RID: 1026
		public const int DM_PAPERSIZE = 2;

		// Token: 0x04000403 RID: 1027
		public const int DM_PAPERLENGTH = 4;

		// Token: 0x04000404 RID: 1028
		public const int DM_PAPERWIDTH = 8;

		// Token: 0x04000405 RID: 1029
		public const int DM_SCALE = 16;

		// Token: 0x04000406 RID: 1030
		public const int DM_COPIES = 256;

		// Token: 0x04000407 RID: 1031
		public const int DM_DEFAULTSOURCE = 512;

		// Token: 0x04000408 RID: 1032
		public const int DM_PRINTQUALITY = 1024;

		// Token: 0x04000409 RID: 1033
		public const int DM_COLOR = 2048;

		// Token: 0x0400040A RID: 1034
		public const int DM_DUPLEX = 4096;

		// Token: 0x0400040B RID: 1035
		public const int DM_YRESOLUTION = 8192;

		// Token: 0x0400040C RID: 1036
		public const int DM_TTOPTION = 16384;

		// Token: 0x0400040D RID: 1037
		public const int DM_COLLATE = 32768;

		// Token: 0x0400040E RID: 1038
		public const int DM_FORMNAME = 65536;

		// Token: 0x0400040F RID: 1039
		public const int DM_LOGPIXELS = 131072;

		// Token: 0x04000410 RID: 1040
		public const int DM_BITSPERPEL = 262144;

		// Token: 0x04000411 RID: 1041
		public const int DM_PELSWIDTH = 524288;

		// Token: 0x04000412 RID: 1042
		public const int DM_PELSHEIGHT = 1048576;

		// Token: 0x04000413 RID: 1043
		public const int DM_DISPLAYFLAGS = 2097152;

		// Token: 0x04000414 RID: 1044
		public const int DM_DISPLAYFREQUENCY = 4194304;

		// Token: 0x04000415 RID: 1045
		public const int DM_PANNINGWIDTH = 8388608;

		// Token: 0x04000416 RID: 1046
		public const int DM_PANNINGHEIGHT = 16777216;

		// Token: 0x04000417 RID: 1047
		public const int DM_ICMMETHOD = 33554432;

		// Token: 0x04000418 RID: 1048
		public const int DM_ICMINTENT = 67108864;

		// Token: 0x04000419 RID: 1049
		public const int DM_MEDIATYPE = 134217728;

		// Token: 0x0400041A RID: 1050
		public const int DM_DITHERTYPE = 268435456;

		// Token: 0x0400041B RID: 1051
		public const int DM_ICCMANUFACTURER = 536870912;

		// Token: 0x0400041C RID: 1052
		public const int DM_ICCMODEL = 1073741824;

		// Token: 0x0400041D RID: 1053
		public const int DMORIENT_PORTRAIT = 1;

		// Token: 0x0400041E RID: 1054
		public const int DMORIENT_LANDSCAPE = 2;

		// Token: 0x0400041F RID: 1055
		public const int DMPAPER_LETTER = 1;

		// Token: 0x04000420 RID: 1056
		public const int DMPAPER_LETTERSMALL = 2;

		// Token: 0x04000421 RID: 1057
		public const int DMPAPER_TABLOID = 3;

		// Token: 0x04000422 RID: 1058
		public const int DMPAPER_LEDGER = 4;

		// Token: 0x04000423 RID: 1059
		public const int DMPAPER_LEGAL = 5;

		// Token: 0x04000424 RID: 1060
		public const int DMPAPER_STATEMENT = 6;

		// Token: 0x04000425 RID: 1061
		public const int DMPAPER_EXECUTIVE = 7;

		// Token: 0x04000426 RID: 1062
		public const int DMPAPER_A3 = 8;

		// Token: 0x04000427 RID: 1063
		public const int DMPAPER_A4 = 9;

		// Token: 0x04000428 RID: 1064
		public const int DMPAPER_A4SMALL = 10;

		// Token: 0x04000429 RID: 1065
		public const int DMPAPER_A5 = 11;

		// Token: 0x0400042A RID: 1066
		public const int DMPAPER_B4 = 12;

		// Token: 0x0400042B RID: 1067
		public const int DMPAPER_B5 = 13;

		// Token: 0x0400042C RID: 1068
		public const int DMPAPER_FOLIO = 14;

		// Token: 0x0400042D RID: 1069
		public const int DMPAPER_QUARTO = 15;

		// Token: 0x0400042E RID: 1070
		public const int DMPAPER_10X14 = 16;

		// Token: 0x0400042F RID: 1071
		public const int DMPAPER_11X17 = 17;

		// Token: 0x04000430 RID: 1072
		public const int DMPAPER_NOTE = 18;

		// Token: 0x04000431 RID: 1073
		public const int DMPAPER_ENV_9 = 19;

		// Token: 0x04000432 RID: 1074
		public const int DMPAPER_ENV_10 = 20;

		// Token: 0x04000433 RID: 1075
		public const int DMPAPER_ENV_11 = 21;

		// Token: 0x04000434 RID: 1076
		public const int DMPAPER_ENV_12 = 22;

		// Token: 0x04000435 RID: 1077
		public const int DMPAPER_ENV_14 = 23;

		// Token: 0x04000436 RID: 1078
		public const int DMPAPER_CSHEET = 24;

		// Token: 0x04000437 RID: 1079
		public const int DMPAPER_DSHEET = 25;

		// Token: 0x04000438 RID: 1080
		public const int DMPAPER_ESHEET = 26;

		// Token: 0x04000439 RID: 1081
		public const int DMPAPER_ENV_DL = 27;

		// Token: 0x0400043A RID: 1082
		public const int DMPAPER_ENV_C5 = 28;

		// Token: 0x0400043B RID: 1083
		public const int DMPAPER_ENV_C3 = 29;

		// Token: 0x0400043C RID: 1084
		public const int DMPAPER_ENV_C4 = 30;

		// Token: 0x0400043D RID: 1085
		public const int DMPAPER_ENV_C6 = 31;

		// Token: 0x0400043E RID: 1086
		public const int DMPAPER_ENV_C65 = 32;

		// Token: 0x0400043F RID: 1087
		public const int DMPAPER_ENV_B4 = 33;

		// Token: 0x04000440 RID: 1088
		public const int DMPAPER_ENV_B5 = 34;

		// Token: 0x04000441 RID: 1089
		public const int DMPAPER_ENV_B6 = 35;

		// Token: 0x04000442 RID: 1090
		public const int DMPAPER_ENV_ITALY = 36;

		// Token: 0x04000443 RID: 1091
		public const int DMPAPER_ENV_MONARCH = 37;

		// Token: 0x04000444 RID: 1092
		public const int DMPAPER_ENV_PERSONAL = 38;

		// Token: 0x04000445 RID: 1093
		public const int DMPAPER_FANFOLD_US = 39;

		// Token: 0x04000446 RID: 1094
		public const int DMPAPER_FANFOLD_STD_GERMAN = 40;

		// Token: 0x04000447 RID: 1095
		public const int DMPAPER_FANFOLD_LGL_GERMAN = 41;

		// Token: 0x04000448 RID: 1096
		public const int DMPAPER_ISO_B4 = 42;

		// Token: 0x04000449 RID: 1097
		public const int DMPAPER_JAPANESE_POSTCARD = 43;

		// Token: 0x0400044A RID: 1098
		public const int DMPAPER_9X11 = 44;

		// Token: 0x0400044B RID: 1099
		public const int DMPAPER_10X11 = 45;

		// Token: 0x0400044C RID: 1100
		public const int DMPAPER_15X11 = 46;

		// Token: 0x0400044D RID: 1101
		public const int DMPAPER_ENV_INVITE = 47;

		// Token: 0x0400044E RID: 1102
		public const int DMPAPER_RESERVED_48 = 48;

		// Token: 0x0400044F RID: 1103
		public const int DMPAPER_RESERVED_49 = 49;

		// Token: 0x04000450 RID: 1104
		public const int DMPAPER_LETTER_EXTRA = 50;

		// Token: 0x04000451 RID: 1105
		public const int DMPAPER_LEGAL_EXTRA = 51;

		// Token: 0x04000452 RID: 1106
		public const int DMPAPER_TABLOID_EXTRA = 52;

		// Token: 0x04000453 RID: 1107
		public const int DMPAPER_A4_EXTRA = 53;

		// Token: 0x04000454 RID: 1108
		public const int DMPAPER_LETTER_TRANSVERSE = 54;

		// Token: 0x04000455 RID: 1109
		public const int DMPAPER_A4_TRANSVERSE = 55;

		// Token: 0x04000456 RID: 1110
		public const int DMPAPER_LETTER_EXTRA_TRANSVERSE = 56;

		// Token: 0x04000457 RID: 1111
		public const int DMPAPER_A_PLUS = 57;

		// Token: 0x04000458 RID: 1112
		public const int DMPAPER_B_PLUS = 58;

		// Token: 0x04000459 RID: 1113
		public const int DMPAPER_LETTER_PLUS = 59;

		// Token: 0x0400045A RID: 1114
		public const int DMPAPER_A4_PLUS = 60;

		// Token: 0x0400045B RID: 1115
		public const int DMPAPER_A5_TRANSVERSE = 61;

		// Token: 0x0400045C RID: 1116
		public const int DMPAPER_B5_TRANSVERSE = 62;

		// Token: 0x0400045D RID: 1117
		public const int DMPAPER_A3_EXTRA = 63;

		// Token: 0x0400045E RID: 1118
		public const int DMPAPER_A5_EXTRA = 64;

		// Token: 0x0400045F RID: 1119
		public const int DMPAPER_B5_EXTRA = 65;

		// Token: 0x04000460 RID: 1120
		public const int DMPAPER_A2 = 66;

		// Token: 0x04000461 RID: 1121
		public const int DMPAPER_A3_TRANSVERSE = 67;

		// Token: 0x04000462 RID: 1122
		public const int DMPAPER_A3_EXTRA_TRANSVERSE = 68;

		// Token: 0x04000463 RID: 1123
		public const int DMPAPER_DBL_JAPANESE_POSTCARD = 69;

		// Token: 0x04000464 RID: 1124
		public const int DMPAPER_A6 = 70;

		// Token: 0x04000465 RID: 1125
		public const int DMPAPER_JENV_KAKU2 = 71;

		// Token: 0x04000466 RID: 1126
		public const int DMPAPER_JENV_KAKU3 = 72;

		// Token: 0x04000467 RID: 1127
		public const int DMPAPER_JENV_CHOU3 = 73;

		// Token: 0x04000468 RID: 1128
		public const int DMPAPER_JENV_CHOU4 = 74;

		// Token: 0x04000469 RID: 1129
		public const int DMPAPER_LETTER_ROTATED = 75;

		// Token: 0x0400046A RID: 1130
		public const int DMPAPER_A3_ROTATED = 76;

		// Token: 0x0400046B RID: 1131
		public const int DMPAPER_A4_ROTATED = 77;

		// Token: 0x0400046C RID: 1132
		public const int DMPAPER_A5_ROTATED = 78;

		// Token: 0x0400046D RID: 1133
		public const int DMPAPER_B4_JIS_ROTATED = 79;

		// Token: 0x0400046E RID: 1134
		public const int DMPAPER_B5_JIS_ROTATED = 80;

		// Token: 0x0400046F RID: 1135
		public const int DMPAPER_JAPANESE_POSTCARD_ROTATED = 81;

		// Token: 0x04000470 RID: 1136
		public const int DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82;

		// Token: 0x04000471 RID: 1137
		public const int DMPAPER_A6_ROTATED = 83;

		// Token: 0x04000472 RID: 1138
		public const int DMPAPER_JENV_KAKU2_ROTATED = 84;

		// Token: 0x04000473 RID: 1139
		public const int DMPAPER_JENV_KAKU3_ROTATED = 85;

		// Token: 0x04000474 RID: 1140
		public const int DMPAPER_JENV_CHOU3_ROTATED = 86;

		// Token: 0x04000475 RID: 1141
		public const int DMPAPER_JENV_CHOU4_ROTATED = 87;

		// Token: 0x04000476 RID: 1142
		public const int DMPAPER_B6_JIS = 88;

		// Token: 0x04000477 RID: 1143
		public const int DMPAPER_B6_JIS_ROTATED = 89;

		// Token: 0x04000478 RID: 1144
		public const int DMPAPER_12X11 = 90;

		// Token: 0x04000479 RID: 1145
		public const int DMPAPER_JENV_YOU4 = 91;

		// Token: 0x0400047A RID: 1146
		public const int DMPAPER_JENV_YOU4_ROTATED = 92;

		// Token: 0x0400047B RID: 1147
		public const int DMPAPER_P16K = 93;

		// Token: 0x0400047C RID: 1148
		public const int DMPAPER_P32K = 94;

		// Token: 0x0400047D RID: 1149
		public const int DMPAPER_P32KBIG = 95;

		// Token: 0x0400047E RID: 1150
		public const int DMPAPER_PENV_1 = 96;

		// Token: 0x0400047F RID: 1151
		public const int DMPAPER_PENV_2 = 97;

		// Token: 0x04000480 RID: 1152
		public const int DMPAPER_PENV_3 = 98;

		// Token: 0x04000481 RID: 1153
		public const int DMPAPER_PENV_4 = 99;

		// Token: 0x04000482 RID: 1154
		public const int DMPAPER_PENV_5 = 100;

		// Token: 0x04000483 RID: 1155
		public const int DMPAPER_PENV_6 = 101;

		// Token: 0x04000484 RID: 1156
		public const int DMPAPER_PENV_7 = 102;

		// Token: 0x04000485 RID: 1157
		public const int DMPAPER_PENV_8 = 103;

		// Token: 0x04000486 RID: 1158
		public const int DMPAPER_PENV_9 = 104;

		// Token: 0x04000487 RID: 1159
		public const int DMPAPER_PENV_10 = 105;

		// Token: 0x04000488 RID: 1160
		public const int DMPAPER_P16K_ROTATED = 106;

		// Token: 0x04000489 RID: 1161
		public const int DMPAPER_P32K_ROTATED = 107;

		// Token: 0x0400048A RID: 1162
		public const int DMPAPER_P32KBIG_ROTATED = 108;

		// Token: 0x0400048B RID: 1163
		public const int DMPAPER_PENV_1_ROTATED = 109;

		// Token: 0x0400048C RID: 1164
		public const int DMPAPER_PENV_2_ROTATED = 110;

		// Token: 0x0400048D RID: 1165
		public const int DMPAPER_PENV_3_ROTATED = 111;

		// Token: 0x0400048E RID: 1166
		public const int DMPAPER_PENV_4_ROTATED = 112;

		// Token: 0x0400048F RID: 1167
		public const int DMPAPER_PENV_5_ROTATED = 113;

		// Token: 0x04000490 RID: 1168
		public const int DMPAPER_PENV_6_ROTATED = 114;

		// Token: 0x04000491 RID: 1169
		public const int DMPAPER_PENV_7_ROTATED = 115;

		// Token: 0x04000492 RID: 1170
		public const int DMPAPER_PENV_8_ROTATED = 116;

		// Token: 0x04000493 RID: 1171
		public const int DMPAPER_PENV_9_ROTATED = 117;

		// Token: 0x04000494 RID: 1172
		public const int DMPAPER_PENV_10_ROTATED = 118;

		// Token: 0x04000495 RID: 1173
		public const int DMPAPER_LAST = 118;

		// Token: 0x04000496 RID: 1174
		public const int DMPAPER_USER = 256;

		// Token: 0x04000497 RID: 1175
		public const int DMBIN_UPPER = 1;

		// Token: 0x04000498 RID: 1176
		public const int DMBIN_ONLYONE = 1;

		// Token: 0x04000499 RID: 1177
		public const int DMBIN_LOWER = 2;

		// Token: 0x0400049A RID: 1178
		public const int DMBIN_MIDDLE = 3;

		// Token: 0x0400049B RID: 1179
		public const int DMBIN_MANUAL = 4;

		// Token: 0x0400049C RID: 1180
		public const int DMBIN_ENVELOPE = 5;

		// Token: 0x0400049D RID: 1181
		public const int DMBIN_ENVMANUAL = 6;

		// Token: 0x0400049E RID: 1182
		public const int DMBIN_AUTO = 7;

		// Token: 0x0400049F RID: 1183
		public const int DMBIN_TRACTOR = 8;

		// Token: 0x040004A0 RID: 1184
		public const int DMBIN_SMALLFMT = 9;

		// Token: 0x040004A1 RID: 1185
		public const int DMBIN_LARGEFMT = 10;

		// Token: 0x040004A2 RID: 1186
		public const int DMBIN_LARGECAPACITY = 11;

		// Token: 0x040004A3 RID: 1187
		public const int DMBIN_CASSETTE = 14;

		// Token: 0x040004A4 RID: 1188
		public const int DMBIN_FORMSOURCE = 15;

		// Token: 0x040004A5 RID: 1189
		public const int DMBIN_LAST = 15;

		// Token: 0x040004A6 RID: 1190
		public const int DMBIN_USER = 256;

		// Token: 0x040004A7 RID: 1191
		public const int DMRES_DRAFT = -1;

		// Token: 0x040004A8 RID: 1192
		public const int DMRES_LOW = -2;

		// Token: 0x040004A9 RID: 1193
		public const int DMRES_MEDIUM = -3;

		// Token: 0x040004AA RID: 1194
		public const int DMRES_HIGH = -4;

		// Token: 0x040004AB RID: 1195
		public const int DMCOLOR_MONOCHROME = 1;

		// Token: 0x040004AC RID: 1196
		public const int DMCOLOR_COLOR = 2;

		// Token: 0x040004AD RID: 1197
		public const int DMDUP_SIMPLEX = 1;

		// Token: 0x040004AE RID: 1198
		public const int DMDUP_VERTICAL = 2;

		// Token: 0x040004AF RID: 1199
		public const int DMDUP_HORIZONTAL = 3;

		// Token: 0x040004B0 RID: 1200
		public const int DMTT_BITMAP = 1;

		// Token: 0x040004B1 RID: 1201
		public const int DMTT_DOWNLOAD = 2;

		// Token: 0x040004B2 RID: 1202
		public const int DMTT_SUBDEV = 3;

		// Token: 0x040004B3 RID: 1203
		public const int DMTT_DOWNLOAD_OUTLINE = 4;

		// Token: 0x040004B4 RID: 1204
		public const int DMCOLLATE_FALSE = 0;

		// Token: 0x040004B5 RID: 1205
		public const int DMCOLLATE_TRUE = 1;

		// Token: 0x040004B6 RID: 1206
		public const int DMDISPLAYFLAGS_TEXTMODE = 4;

		// Token: 0x040004B7 RID: 1207
		public const int DMICMMETHOD_NONE = 1;

		// Token: 0x040004B8 RID: 1208
		public const int DMICMMETHOD_SYSTEM = 2;

		// Token: 0x040004B9 RID: 1209
		public const int DMICMMETHOD_DRIVER = 3;

		// Token: 0x040004BA RID: 1210
		public const int DMICMMETHOD_DEVICE = 4;

		// Token: 0x040004BB RID: 1211
		public const int DMICMMETHOD_USER = 256;

		// Token: 0x040004BC RID: 1212
		public const int DMICM_SATURATE = 1;

		// Token: 0x040004BD RID: 1213
		public const int DMICM_CONTRAST = 2;

		// Token: 0x040004BE RID: 1214
		public const int DMICM_COLORMETRIC = 3;

		// Token: 0x040004BF RID: 1215
		public const int DMICM_USER = 256;

		// Token: 0x040004C0 RID: 1216
		public const int DMMEDIA_STANDARD = 1;

		// Token: 0x040004C1 RID: 1217
		public const int DMMEDIA_TRANSPARENCY = 2;

		// Token: 0x040004C2 RID: 1218
		public const int DMMEDIA_GLOSSY = 3;

		// Token: 0x040004C3 RID: 1219
		public const int DMMEDIA_USER = 256;

		// Token: 0x040004C4 RID: 1220
		public const int DMDITHER_NONE = 1;

		// Token: 0x040004C5 RID: 1221
		public const int DMDITHER_COARSE = 2;

		// Token: 0x040004C6 RID: 1222
		public const int DMDITHER_FINE = 3;

		// Token: 0x040004C7 RID: 1223
		public const int DMDITHER_LINEART = 4;

		// Token: 0x040004C8 RID: 1224
		public const int DMDITHER_GRAYSCALE = 5;

		// Token: 0x040004C9 RID: 1225
		public const int DMDITHER_USER = 256;

		// Token: 0x040004CA RID: 1226
		public const int PRINTER_ENUM_DEFAULT = 1;

		// Token: 0x040004CB RID: 1227
		public const int PRINTER_ENUM_LOCAL = 2;

		// Token: 0x040004CC RID: 1228
		public const int PRINTER_ENUM_CONNECTIONS = 4;

		// Token: 0x040004CD RID: 1229
		public const int PRINTER_ENUM_FAVORITE = 4;

		// Token: 0x040004CE RID: 1230
		public const int PRINTER_ENUM_NAME = 8;

		// Token: 0x040004CF RID: 1231
		public const int PRINTER_ENUM_REMOTE = 16;

		// Token: 0x040004D0 RID: 1232
		public const int PRINTER_ENUM_SHARED = 32;

		// Token: 0x040004D1 RID: 1233
		public const int PRINTER_ENUM_NETWORK = 64;

		// Token: 0x040004D2 RID: 1234
		public const int PRINTER_ENUM_EXPAND = 16384;

		// Token: 0x040004D3 RID: 1235
		public const int PRINTER_ENUM_CONTAINER = 32768;

		// Token: 0x040004D4 RID: 1236
		public const int PRINTER_ENUM_ICONMASK = 16711680;

		// Token: 0x040004D5 RID: 1237
		public const int PRINTER_ENUM_ICON1 = 65536;

		// Token: 0x040004D6 RID: 1238
		public const int PRINTER_ENUM_ICON2 = 131072;

		// Token: 0x040004D7 RID: 1239
		public const int PRINTER_ENUM_ICON3 = 262144;

		// Token: 0x040004D8 RID: 1240
		public const int PRINTER_ENUM_ICON4 = 524288;

		// Token: 0x040004D9 RID: 1241
		public const int PRINTER_ENUM_ICON5 = 1048576;

		// Token: 0x040004DA RID: 1242
		public const int PRINTER_ENUM_ICON6 = 2097152;

		// Token: 0x040004DB RID: 1243
		public const int PRINTER_ENUM_ICON7 = 4194304;

		// Token: 0x040004DC RID: 1244
		public const int PRINTER_ENUM_ICON8 = 8388608;

		// Token: 0x040004DD RID: 1245
		public const int DC_BINADJUST = 19;

		// Token: 0x040004DE RID: 1246
		public const int DC_EMF_COMPLIANT = 20;

		// Token: 0x040004DF RID: 1247
		public const int DC_DATATYPE_PRODUCED = 21;

		// Token: 0x040004E0 RID: 1248
		public const int DC_COLLATE = 22;

		// Token: 0x040004E1 RID: 1249
		public const int DCTT_BITMAP = 1;

		// Token: 0x040004E2 RID: 1250
		public const int DCTT_DOWNLOAD = 2;

		// Token: 0x040004E3 RID: 1251
		public const int DCTT_SUBDEV = 4;

		// Token: 0x040004E4 RID: 1252
		public const int DCTT_DOWNLOAD_OUTLINE = 8;

		// Token: 0x040004E5 RID: 1253
		public const int DCBA_FACEUPNONE = 0;

		// Token: 0x040004E6 RID: 1254
		public const int DCBA_FACEUPCENTER = 1;

		// Token: 0x040004E7 RID: 1255
		public const int DCBA_FACEUPLEFT = 2;

		// Token: 0x040004E8 RID: 1256
		public const int DCBA_FACEUPRIGHT = 3;

		// Token: 0x040004E9 RID: 1257
		public const int DCBA_FACEDOWNNONE = 256;

		// Token: 0x040004EA RID: 1258
		public const int DCBA_FACEDOWNCENTER = 257;

		// Token: 0x040004EB RID: 1259
		public const int DCBA_FACEDOWNLEFT = 258;

		// Token: 0x040004EC RID: 1260
		public const int DCBA_FACEDOWNRIGHT = 259;

		// Token: 0x040004ED RID: 1261
		public const int SRCPAINT = 15597702;

		// Token: 0x040004EE RID: 1262
		public const int SRCAND = 8913094;

		// Token: 0x040004EF RID: 1263
		public const int SRCINVERT = 6684742;

		// Token: 0x040004F0 RID: 1264
		public const int SRCERASE = 4457256;

		// Token: 0x040004F1 RID: 1265
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x040004F2 RID: 1266
		public const int NOTSRCERASE = 1114278;

		// Token: 0x040004F3 RID: 1267
		public const int MERGECOPY = 12583114;

		// Token: 0x040004F4 RID: 1268
		public const int MERGEPAINT = 12255782;

		// Token: 0x040004F5 RID: 1269
		public const int PATCOPY = 15728673;

		// Token: 0x040004F6 RID: 1270
		public const int PATPAINT = 16452105;

		// Token: 0x040004F7 RID: 1271
		public const int PATINVERT = 5898313;

		// Token: 0x040004F8 RID: 1272
		public const int DSTINVERT = 5570569;

		// Token: 0x040004F9 RID: 1273
		public const int BLACKNESS = 66;

		// Token: 0x040004FA RID: 1274
		public const int WHITENESS = 16711778;

		// Token: 0x040004FB RID: 1275
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x040004FC RID: 1276
		public const int SM_CXSCREEN = 0;

		// Token: 0x040004FD RID: 1277
		public const int SM_CYSCREEN = 1;

		// Token: 0x040004FE RID: 1278
		public const int SM_CXVSCROLL = 2;

		// Token: 0x040004FF RID: 1279
		public const int SM_CYHSCROLL = 3;

		// Token: 0x04000500 RID: 1280
		public const int SM_CYCAPTION = 4;

		// Token: 0x04000501 RID: 1281
		public const int SM_CXBORDER = 5;

		// Token: 0x04000502 RID: 1282
		public const int SM_CYBORDER = 6;

		// Token: 0x04000503 RID: 1283
		public const int SM_CXDLGFRAME = 7;

		// Token: 0x04000504 RID: 1284
		public const int SM_CYDLGFRAME = 8;

		// Token: 0x04000505 RID: 1285
		public const int SM_CYVTHUMB = 9;

		// Token: 0x04000506 RID: 1286
		public const int SM_CXHTHUMB = 10;

		// Token: 0x04000507 RID: 1287
		public const int SM_CXICON = 11;

		// Token: 0x04000508 RID: 1288
		public const int SM_CYICON = 12;

		// Token: 0x04000509 RID: 1289
		public const int SM_CXCURSOR = 13;

		// Token: 0x0400050A RID: 1290
		public const int SM_CYCURSOR = 14;

		// Token: 0x0400050B RID: 1291
		public const int SM_CYMENU = 15;

		// Token: 0x0400050C RID: 1292
		public const int SM_CXFULLSCREEN = 16;

		// Token: 0x0400050D RID: 1293
		public const int SM_CYFULLSCREEN = 17;

		// Token: 0x0400050E RID: 1294
		public const int SM_CYKANJIWINDOW = 18;

		// Token: 0x0400050F RID: 1295
		public const int SM_MOUSEPRESENT = 19;

		// Token: 0x04000510 RID: 1296
		public const int SM_CYVSCROLL = 20;

		// Token: 0x04000511 RID: 1297
		public const int SM_CXHSCROLL = 21;

		// Token: 0x04000512 RID: 1298
		public const int SM_DEBUG = 22;

		// Token: 0x04000513 RID: 1299
		public const int SM_SWAPBUTTON = 23;

		// Token: 0x04000514 RID: 1300
		public const int SM_RESERVED1 = 24;

		// Token: 0x04000515 RID: 1301
		public const int SM_RESERVED2 = 25;

		// Token: 0x04000516 RID: 1302
		public const int SM_RESERVED3 = 26;

		// Token: 0x04000517 RID: 1303
		public const int SM_RESERVED4 = 27;

		// Token: 0x04000518 RID: 1304
		public const int SM_CXMIN = 28;

		// Token: 0x04000519 RID: 1305
		public const int SM_CYMIN = 29;

		// Token: 0x0400051A RID: 1306
		public const int SM_CXSIZE = 30;

		// Token: 0x0400051B RID: 1307
		public const int SM_CYSIZE = 31;

		// Token: 0x0400051C RID: 1308
		public const int SM_CXFRAME = 32;

		// Token: 0x0400051D RID: 1309
		public const int SM_CYFRAME = 33;

		// Token: 0x0400051E RID: 1310
		public const int SM_CXMINTRACK = 34;

		// Token: 0x0400051F RID: 1311
		public const int SM_CYMINTRACK = 35;

		// Token: 0x04000520 RID: 1312
		public const int SM_CXDOUBLECLK = 36;

		// Token: 0x04000521 RID: 1313
		public const int SM_CYDOUBLECLK = 37;

		// Token: 0x04000522 RID: 1314
		public const int SM_CXICONSPACING = 38;

		// Token: 0x04000523 RID: 1315
		public const int SM_CYICONSPACING = 39;

		// Token: 0x04000524 RID: 1316
		public const int SM_MENUDROPALIGNMENT = 40;

		// Token: 0x04000525 RID: 1317
		public const int SM_PENWINDOWS = 41;

		// Token: 0x04000526 RID: 1318
		public const int SM_DBCSENABLED = 42;

		// Token: 0x04000527 RID: 1319
		public const int SM_CMOUSEBUTTONS = 43;

		// Token: 0x04000528 RID: 1320
		public const int SM_CXFIXEDFRAME = 7;

		// Token: 0x04000529 RID: 1321
		public const int SM_CYFIXEDFRAME = 8;

		// Token: 0x0400052A RID: 1322
		public const int SM_CXSIZEFRAME = 32;

		// Token: 0x0400052B RID: 1323
		public const int SM_CYSIZEFRAME = 33;

		// Token: 0x0400052C RID: 1324
		public const int SM_SECURE = 44;

		// Token: 0x0400052D RID: 1325
		public const int SM_CXEDGE = 45;

		// Token: 0x0400052E RID: 1326
		public const int SM_CYEDGE = 46;

		// Token: 0x0400052F RID: 1327
		public const int SM_CXMINSPACING = 47;

		// Token: 0x04000530 RID: 1328
		public const int SM_CYMINSPACING = 48;

		// Token: 0x04000531 RID: 1329
		public const int SM_CXSMICON = 49;

		// Token: 0x04000532 RID: 1330
		public const int SM_CYSMICON = 50;

		// Token: 0x04000533 RID: 1331
		public const int SM_CYSMCAPTION = 51;

		// Token: 0x04000534 RID: 1332
		public const int SM_CXSMSIZE = 52;

		// Token: 0x04000535 RID: 1333
		public const int SM_CYSMSIZE = 53;

		// Token: 0x04000536 RID: 1334
		public const int SM_CXMENUSIZE = 54;

		// Token: 0x04000537 RID: 1335
		public const int SM_CYMENUSIZE = 55;

		// Token: 0x04000538 RID: 1336
		public const int SM_ARRANGE = 56;

		// Token: 0x04000539 RID: 1337
		public const int SM_CXMINIMIZED = 57;

		// Token: 0x0400053A RID: 1338
		public const int SM_CYMINIMIZED = 58;

		// Token: 0x0400053B RID: 1339
		public const int SM_CXMAXTRACK = 59;

		// Token: 0x0400053C RID: 1340
		public const int SM_CYMAXTRACK = 60;

		// Token: 0x0400053D RID: 1341
		public const int SM_CXMAXIMIZED = 61;

		// Token: 0x0400053E RID: 1342
		public const int SM_CYMAXIMIZED = 62;

		// Token: 0x0400053F RID: 1343
		public const int SM_NETWORK = 63;

		// Token: 0x04000540 RID: 1344
		public const int SM_CLEANBOOT = 67;

		// Token: 0x04000541 RID: 1345
		public const int SM_CXDRAG = 68;

		// Token: 0x04000542 RID: 1346
		public const int SM_CYDRAG = 69;

		// Token: 0x04000543 RID: 1347
		public const int SM_SHOWSOUNDS = 70;

		// Token: 0x04000544 RID: 1348
		public const int SM_CXMENUCHECK = 71;

		// Token: 0x04000545 RID: 1349
		public const int SM_CYMENUCHECK = 72;

		// Token: 0x04000546 RID: 1350
		public const int SM_SLOWMACHINE = 73;

		// Token: 0x04000547 RID: 1351
		public const int SM_MIDEASTENABLED = 74;

		// Token: 0x04000548 RID: 1352
		public const int SM_MOUSEWHEELPRESENT = 75;

		// Token: 0x04000549 RID: 1353
		public const int SM_XVIRTUALSCREEN = 76;

		// Token: 0x0400054A RID: 1354
		public const int SM_YVIRTUALSCREEN = 77;

		// Token: 0x0400054B RID: 1355
		public const int SM_CXVIRTUALSCREEN = 78;

		// Token: 0x0400054C RID: 1356
		public const int SM_CYVIRTUALSCREEN = 79;

		// Token: 0x0400054D RID: 1357
		public const int SM_CMONITORS = 80;

		// Token: 0x0400054E RID: 1358
		public const int SM_SAMEDISPLAYFORMAT = 81;

		// Token: 0x0400054F RID: 1359
		public const int SM_CMETRICS = 83;

		// Token: 0x04000550 RID: 1360
		public const int GM_COMPATIBLE = 1;

		// Token: 0x04000551 RID: 1361
		public const int GM_ADVANCED = 2;

		// Token: 0x04000552 RID: 1362
		public const int MWT_IDENTITY = 1;

		// Token: 0x04000553 RID: 1363
		public const int FW_DONTCARE = 0;

		// Token: 0x04000554 RID: 1364
		public const int FW_NORMAL = 400;

		// Token: 0x04000555 RID: 1365
		public const int FW_BOLD = 700;

		// Token: 0x04000556 RID: 1366
		public const int ANSI_CHARSET = 0;

		// Token: 0x04000557 RID: 1367
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x04000558 RID: 1368
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x04000559 RID: 1369
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x0400055A RID: 1370
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x0400055B RID: 1371
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x0400055C RID: 1372
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x0400055D RID: 1373
		public const int MM_TEXT = 1;

		// Token: 0x0400055E RID: 1374
		public const int OBJ_FONT = 6;

		// Token: 0x0400055F RID: 1375
		public const int TA_DEFAULT = 0;

		// Token: 0x04000560 RID: 1376
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000561 RID: 1377
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000562 RID: 1378
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000563 RID: 1379
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x04000564 RID: 1380
		public const int NOMIRRORBITMAP = -2147483648;

		// Token: 0x04000565 RID: 1381
		public const int QUERYESCSUPPORT = 8;

		// Token: 0x04000566 RID: 1382
		public const int CHECKJPEGFORMAT = 4119;

		// Token: 0x04000567 RID: 1383
		public const int CHECKPNGFORMAT = 4120;

		// Token: 0x04000568 RID: 1384
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000569 RID: 1385
		public const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x0400056A RID: 1386
		public const int ERROR_PROC_NOT_FOUND = 127;

		// Token: 0x02000105 RID: 261
		[SuppressUnmanagedCodeSecurity]
		internal class Gdip
		{
			// Token: 0x06000CD8 RID: 3288 RVA: 0x0002CB8C File Offset: 0x0002AD8C
			static Gdip()
			{
				SafeNativeMethods.Gdip.Initialize();
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0002CBA7 File Offset: 0x0002ADA7
			private static bool Initialized
			{
				get
				{
					return SafeNativeMethods.Gdip.initToken != IntPtr.Zero;
				}
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0002CBB8 File Offset: 0x0002ADB8
			internal static IDictionary ThreadData
			{
				get
				{
					LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("system.drawing.threaddata");
					IDictionary dictionary = (IDictionary)Thread.GetData(namedDataSlot);
					if (dictionary == null)
					{
						dictionary = new Hashtable();
						Thread.SetData(namedDataSlot, dictionary);
					}
					return dictionary;
				}
			}

			// Token: 0x06000CDB RID: 3291 RVA: 0x0002CBF0 File Offset: 0x0002ADF0
			[MethodImpl(MethodImplOptions.NoInlining)]
			private static void ClearThreadData()
			{
				LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("system.drawing.threaddata");
				Thread.SetData(namedDataSlot, null);
			}

			// Token: 0x06000CDC RID: 3292 RVA: 0x0002CC10 File Offset: 0x0002AE10
			private static void Initialize()
			{
				SafeNativeMethods.Gdip.StartupInput @default = SafeNativeMethods.Gdip.StartupInput.GetDefault();
				SafeNativeMethods.Gdip.StartupOutput startupOutput;
				int num = SafeNativeMethods.Gdip.GdiplusStartup(out SafeNativeMethods.Gdip.initToken, ref @default, out startupOutput);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.ProcessExit += SafeNativeMethods.Gdip.OnProcessExit;
				if (!currentDomain.IsDefaultAppDomain())
				{
					currentDomain.DomainUnload += SafeNativeMethods.Gdip.OnProcessExit;
				}
			}

			// Token: 0x06000CDD RID: 3293 RVA: 0x0002CC70 File Offset: 0x0002AE70
			private static void Shutdown()
			{
				if (SafeNativeMethods.Gdip.Initialized)
				{
					SafeNativeMethods.Gdip.ClearThreadData();
					AppDomain currentDomain = AppDomain.CurrentDomain;
					currentDomain.ProcessExit -= SafeNativeMethods.Gdip.OnProcessExit;
					if (!currentDomain.IsDefaultAppDomain())
					{
						currentDomain.DomainUnload -= SafeNativeMethods.Gdip.OnProcessExit;
					}
				}
			}

			// Token: 0x06000CDE RID: 3294 RVA: 0x0002CCBB File Offset: 0x0002AEBB
			[PrePrepareMethod]
			private static void OnProcessExit(object sender, EventArgs e)
			{
				SafeNativeMethods.Gdip.Shutdown();
			}

			// Token: 0x06000CDF RID: 3295 RVA: 0x00015255 File Offset: 0x00013455
			internal static void DummyFunction()
			{
			}

			// Token: 0x06000CE0 RID: 3296
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			private static extern int GdiplusStartup(out IntPtr token, ref SafeNativeMethods.Gdip.StartupInput input, out SafeNativeMethods.Gdip.StartupOutput output);

			// Token: 0x06000CE1 RID: 3297
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			private static extern void GdiplusShutdown(HandleRef token);

			// Token: 0x06000CE2 RID: 3298
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath(int brushMode, out IntPtr path);

			// Token: 0x06000CE3 RID: 3299
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath2(HandleRef points, HandleRef types, int count, int brushMode, out IntPtr path);

			// Token: 0x06000CE4 RID: 3300
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath2I(HandleRef points, HandleRef types, int count, int brushMode, out IntPtr path);

			// Token: 0x06000CE5 RID: 3301
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClonePath(HandleRef path, out IntPtr clonepath);

			// Token: 0x06000CE6 RID: 3302
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePath", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePath(HandleRef path);

			// Token: 0x06000CE7 RID: 3303 RVA: 0x0002CCC4 File Offset: 0x0002AEC4
			internal static int GdipDeletePath(HandleRef path)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePath(path);
			}

			// Token: 0x06000CE8 RID: 3304
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPath(HandleRef path);

			// Token: 0x06000CE9 RID: 3305
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPointCount(HandleRef path, out int count);

			// Token: 0x06000CEA RID: 3306
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathTypes(HandleRef path, byte[] types, int count);

			// Token: 0x06000CEB RID: 3307
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathPoints(HandleRef path, HandleRef points, int count);

			// Token: 0x06000CEC RID: 3308
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathFillMode(HandleRef path, out int fillmode);

			// Token: 0x06000CED RID: 3309
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathFillMode(HandleRef path, int fillmode);

			// Token: 0x06000CEE RID: 3310
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathData(HandleRef path, IntPtr pathData);

			// Token: 0x06000CEF RID: 3311
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStartPathFigure(HandleRef path);

			// Token: 0x06000CF0 RID: 3312
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClosePathFigure(HandleRef path);

			// Token: 0x06000CF1 RID: 3313
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClosePathFigures(HandleRef path);

			// Token: 0x06000CF2 RID: 3314
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathMarker(HandleRef path);

			// Token: 0x06000CF3 RID: 3315
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClearPathMarkers(HandleRef path);

			// Token: 0x06000CF4 RID: 3316
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipReversePath(HandleRef path);

			// Token: 0x06000CF5 RID: 3317
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathLastPoint(HandleRef path, GPPOINTF lastPoint);

			// Token: 0x06000CF6 RID: 3318
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine(HandleRef path, float x1, float y1, float x2, float y2);

			// Token: 0x06000CF7 RID: 3319
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine2(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000CF8 RID: 3320
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathArc(HandleRef path, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000CF9 RID: 3321
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBezier(HandleRef path, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);

			// Token: 0x06000CFA RID: 3322
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBeziers(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000CFB RID: 3323
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000CFC RID: 3324
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve2(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x06000CFD RID: 3325
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve3(HandleRef path, HandleRef memorypts, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000CFE RID: 3326
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000CFF RID: 3327
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve2(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x06000D00 RID: 3328
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangle(HandleRef path, float x, float y, float width, float height);

			// Token: 0x06000D01 RID: 3329
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangles(HandleRef path, HandleRef rects, int count);

			// Token: 0x06000D02 RID: 3330
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathEllipse(HandleRef path, float x, float y, float width, float height);

			// Token: 0x06000D03 RID: 3331
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPie(HandleRef path, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000D04 RID: 3332
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPolygon(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D05 RID: 3333
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPath(HandleRef path, HandleRef addingPath, bool connect);

			// Token: 0x06000D06 RID: 3334
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathString(HandleRef path, string s, int length, HandleRef fontFamily, int style, float emSize, ref GPRECTF layoutRect, HandleRef format);

			// Token: 0x06000D07 RID: 3335
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathStringI(HandleRef path, string s, int length, HandleRef fontFamily, int style, float emSize, ref GPRECT layoutRect, HandleRef format);

			// Token: 0x06000D08 RID: 3336
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLineI(HandleRef path, int x1, int y1, int x2, int y2);

			// Token: 0x06000D09 RID: 3337
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine2I(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D0A RID: 3338
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathArcI(HandleRef path, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000D0B RID: 3339
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBezierI(HandleRef path, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);

			// Token: 0x06000D0C RID: 3340
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBeziersI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D0D RID: 3341
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurveI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D0E RID: 3342
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve2I(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x06000D0F RID: 3343
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve3I(HandleRef path, HandleRef memorypts, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000D10 RID: 3344
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurveI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D11 RID: 3345
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve2I(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x06000D12 RID: 3346
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangleI(HandleRef path, int x, int y, int width, int height);

			// Token: 0x06000D13 RID: 3347
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectanglesI(HandleRef path, HandleRef rects, int count);

			// Token: 0x06000D14 RID: 3348
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathEllipseI(HandleRef path, int x, int y, int width, int height);

			// Token: 0x06000D15 RID: 3349
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPieI(HandleRef path, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000D16 RID: 3350
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPolygonI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000D17 RID: 3351
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFlattenPath(HandleRef path, HandleRef matrixfloat, float flatness);

			// Token: 0x06000D18 RID: 3352
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipWidenPath(HandleRef path, HandleRef pen, HandleRef matrix, float flatness);

			// Token: 0x06000D19 RID: 3353
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipWarpPath(HandleRef path, HandleRef matrix, HandleRef points, int count, float srcX, float srcY, float srcWidth, float srcHeight, WarpMode warpMode, float flatness);

			// Token: 0x06000D1A RID: 3354
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPath(HandleRef path, HandleRef matrix);

			// Token: 0x06000D1B RID: 3355
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathWorldBounds(HandleRef path, ref GPRECTF gprectf, HandleRef matrix, HandleRef pen);

			// Token: 0x06000D1C RID: 3356
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePathPoint(HandleRef path, float x, float y, HandleRef graphics, out int boolean);

			// Token: 0x06000D1D RID: 3357
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePathPointI(HandleRef path, int x, int y, HandleRef graphics, out int boolean);

			// Token: 0x06000D1E RID: 3358
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsOutlineVisiblePathPoint(HandleRef path, float x, float y, HandleRef pen, HandleRef graphics, out int boolean);

			// Token: 0x06000D1F RID: 3359
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsOutlineVisiblePathPointI(HandleRef path, int x, int y, HandleRef pen, HandleRef graphics, out int boolean);

			// Token: 0x06000D20 RID: 3360
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathIter(out IntPtr pathIter, HandleRef path);

			// Token: 0x06000D21 RID: 3361
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePathIter", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePathIter(HandleRef pathIter);

			// Token: 0x06000D22 RID: 3362 RVA: 0x0002CCE4 File Offset: 0x0002AEE4
			internal static int GdipDeletePathIter(HandleRef pathIter)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePathIter(pathIter);
			}

			// Token: 0x06000D23 RID: 3363
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextSubpath(HandleRef pathIter, out int resultCount, out int startIndex, out int endIndex, out bool isClosed);

			// Token: 0x06000D24 RID: 3364
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextSubpathPath(HandleRef pathIter, out int resultCount, HandleRef path, out bool isClosed);

			// Token: 0x06000D25 RID: 3365
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextPathType(HandleRef pathIter, out int resultCount, out byte pathType, out int startIndex, out int endIndex);

			// Token: 0x06000D26 RID: 3366
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextMarker(HandleRef pathIter, out int resultCount, out int startIndex, out int endIndex);

			// Token: 0x06000D27 RID: 3367
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextMarkerPath(HandleRef pathIter, out int resultCount, HandleRef path);

			// Token: 0x06000D28 RID: 3368
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterGetCount(HandleRef pathIter, out int count);

			// Token: 0x06000D29 RID: 3369
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterGetSubpathCount(HandleRef pathIter, out int count);

			// Token: 0x06000D2A RID: 3370
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterHasCurve(HandleRef pathIter, out bool hasCurve);

			// Token: 0x06000D2B RID: 3371
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterRewind(HandleRef pathIter);

			// Token: 0x06000D2C RID: 3372
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterEnumerate(HandleRef pathIter, out int resultCount, IntPtr memoryPts, [In] [Out] byte[] types, int count);

			// Token: 0x06000D2D RID: 3373
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterCopyData(HandleRef pathIter, out int resultCount, IntPtr memoryPts, [In] [Out] byte[] types, int startIndex, int endIndex);

			// Token: 0x06000D2E RID: 3374
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix(out IntPtr matrix);

			// Token: 0x06000D2F RID: 3375
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix2(float m11, float m12, float m21, float m22, float dx, float dy, out IntPtr matrix);

			// Token: 0x06000D30 RID: 3376
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix3(ref GPRECTF rect, HandleRef dstplg, out IntPtr matrix);

			// Token: 0x06000D31 RID: 3377
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix3I(ref GPRECT rect, HandleRef dstplg, out IntPtr matrix);

			// Token: 0x06000D32 RID: 3378
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneMatrix(HandleRef matrix, out IntPtr cloneMatrix);

			// Token: 0x06000D33 RID: 3379
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteMatrix", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteMatrix(HandleRef matrix);

			// Token: 0x06000D34 RID: 3380 RVA: 0x0002CD04 File Offset: 0x0002AF04
			internal static int GdipDeleteMatrix(HandleRef matrix)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteMatrix(matrix);
			}

			// Token: 0x06000D35 RID: 3381
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetMatrixElements(HandleRef matrix, float m11, float m12, float m21, float m22, float dx, float dy);

			// Token: 0x06000D36 RID: 3382
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyMatrix(HandleRef matrix, HandleRef matrix2, MatrixOrder order);

			// Token: 0x06000D37 RID: 3383
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateMatrix(HandleRef matrix, float offsetX, float offsetY, MatrixOrder order);

			// Token: 0x06000D38 RID: 3384
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleMatrix(HandleRef matrix, float scaleX, float scaleY, MatrixOrder order);

			// Token: 0x06000D39 RID: 3385
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateMatrix(HandleRef matrix, float angle, MatrixOrder order);

			// Token: 0x06000D3A RID: 3386
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipShearMatrix(HandleRef matrix, float shearX, float shearY, MatrixOrder order);

			// Token: 0x06000D3B RID: 3387
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipInvertMatrix(HandleRef matrix);

			// Token: 0x06000D3C RID: 3388
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformMatrixPoints(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x06000D3D RID: 3389
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformMatrixPointsI(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x06000D3E RID: 3390
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipVectorTransformMatrixPoints(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x06000D3F RID: 3391
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipVectorTransformMatrixPointsI(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x06000D40 RID: 3392
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMatrixElements(HandleRef matrix, IntPtr m);

			// Token: 0x06000D41 RID: 3393
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixInvertible(HandleRef matrix, out int boolean);

			// Token: 0x06000D42 RID: 3394
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixIdentity(HandleRef matrix, out int boolean);

			// Token: 0x06000D43 RID: 3395
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixEqual(HandleRef matrix, HandleRef matrix2, out int boolean);

			// Token: 0x06000D44 RID: 3396
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegion(out IntPtr region);

			// Token: 0x06000D45 RID: 3397
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRect(ref GPRECTF gprectf, out IntPtr region);

			// Token: 0x06000D46 RID: 3398
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRectI(ref GPRECT gprect, out IntPtr region);

			// Token: 0x06000D47 RID: 3399
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionPath(HandleRef path, out IntPtr region);

			// Token: 0x06000D48 RID: 3400
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRgnData(byte[] rgndata, int size, out IntPtr region);

			// Token: 0x06000D49 RID: 3401
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionHrgn(HandleRef hRgn, out IntPtr region);

			// Token: 0x06000D4A RID: 3402
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneRegion(HandleRef region, out IntPtr cloneregion);

			// Token: 0x06000D4B RID: 3403
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteRegion", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteRegion(HandleRef region);

			// Token: 0x06000D4C RID: 3404 RVA: 0x0002CD24 File Offset: 0x0002AF24
			internal static int GdipDeleteRegion(HandleRef region)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteRegion(region);
			}

			// Token: 0x06000D4D RID: 3405
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetInfinite(HandleRef region);

			// Token: 0x06000D4E RID: 3406
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetEmpty(HandleRef region);

			// Token: 0x06000D4F RID: 3407
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRect(HandleRef region, ref GPRECTF gprectf, CombineMode mode);

			// Token: 0x06000D50 RID: 3408
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRectI(HandleRef region, ref GPRECT gprect, CombineMode mode);

			// Token: 0x06000D51 RID: 3409
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionPath(HandleRef region, HandleRef path, CombineMode mode);

			// Token: 0x06000D52 RID: 3410
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRegion(HandleRef region, HandleRef region2, CombineMode mode);

			// Token: 0x06000D53 RID: 3411
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateRegion(HandleRef region, float dx, float dy);

			// Token: 0x06000D54 RID: 3412
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateRegionI(HandleRef region, int dx, int dy);

			// Token: 0x06000D55 RID: 3413
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformRegion(HandleRef region, HandleRef matrix);

			// Token: 0x06000D56 RID: 3414
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionBounds(HandleRef region, HandleRef graphics, ref GPRECTF gprectf);

			// Token: 0x06000D57 RID: 3415
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionHRgn(HandleRef region, HandleRef graphics, out IntPtr hrgn);

			// Token: 0x06000D58 RID: 3416
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsEmptyRegion(HandleRef region, HandleRef graphics, out int boolean);

			// Token: 0x06000D59 RID: 3417
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsInfiniteRegion(HandleRef region, HandleRef graphics, out int boolean);

			// Token: 0x06000D5A RID: 3418
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsEqualRegion(HandleRef region, HandleRef region2, HandleRef graphics, out int boolean);

			// Token: 0x06000D5B RID: 3419
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionDataSize(HandleRef region, out int bufferSize);

			// Token: 0x06000D5C RID: 3420
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionData(HandleRef region, byte[] regionData, int bufferSize, out int sizeFilled);

			// Token: 0x06000D5D RID: 3421
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionPoint(HandleRef region, float X, float Y, HandleRef graphics, out int boolean);

			// Token: 0x06000D5E RID: 3422
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionPointI(HandleRef region, int X, int Y, HandleRef graphics, out int boolean);

			// Token: 0x06000D5F RID: 3423
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionRect(HandleRef region, float X, float Y, float width, float height, HandleRef graphics, out int boolean);

			// Token: 0x06000D60 RID: 3424
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionRectI(HandleRef region, int X, int Y, int width, int height, HandleRef graphics, out int boolean);

			// Token: 0x06000D61 RID: 3425
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionScansCount(HandleRef region, out int count, HandleRef matrix);

			// Token: 0x06000D62 RID: 3426
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionScans(HandleRef region, IntPtr rects, out int count, HandleRef matrix);

			// Token: 0x06000D63 RID: 3427
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBrush(HandleRef brush, out IntPtr clonebrush);

			// Token: 0x06000D64 RID: 3428
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteBrush", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteBrush(HandleRef brush);

			// Token: 0x06000D65 RID: 3429 RVA: 0x0002CD44 File Offset: 0x0002AF44
			internal static int GdipDeleteBrush(HandleRef brush)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteBrush(brush);
			}

			// Token: 0x06000D66 RID: 3430
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHatchBrush(int hatchstyle, int forecol, int backcol, out IntPtr brush);

			// Token: 0x06000D67 RID: 3431
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchStyle(HandleRef brush, out int hatchstyle);

			// Token: 0x06000D68 RID: 3432
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchForegroundColor(HandleRef brush, out int forecol);

			// Token: 0x06000D69 RID: 3433
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchBackgroundColor(HandleRef brush, out int backcol);

			// Token: 0x06000D6A RID: 3434
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture(HandleRef bitmap, int wrapmode, out IntPtr texture);

			// Token: 0x06000D6B RID: 3435
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture2(HandleRef bitmap, int wrapmode, float x, float y, float width, float height, out IntPtr texture);

			// Token: 0x06000D6C RID: 3436
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTextureIA(HandleRef bitmap, HandleRef imageAttrib, float x, float y, float width, float height, out IntPtr texture);

			// Token: 0x06000D6D RID: 3437
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture2I(HandleRef bitmap, int wrapmode, int x, int y, int width, int height, out IntPtr texture);

			// Token: 0x06000D6E RID: 3438
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTextureIAI(HandleRef bitmap, HandleRef imageAttrib, int x, int y, int width, int height, out IntPtr texture);

			// Token: 0x06000D6F RID: 3439
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextureTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000D70 RID: 3440
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000D71 RID: 3441
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetTextureTransform(HandleRef brush);

			// Token: 0x06000D72 RID: 3442
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyTextureTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000D73 RID: 3443
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateTextureTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000D74 RID: 3444
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleTextureTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000D75 RID: 3445
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateTextureTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000D76 RID: 3446
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextureWrapMode(HandleRef brush, int wrapMode);

			// Token: 0x06000D77 RID: 3447
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureWrapMode(HandleRef brush, out int wrapMode);

			// Token: 0x06000D78 RID: 3448
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureImage(HandleRef brush, out IntPtr image);

			// Token: 0x06000D79 RID: 3449
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateSolidFill(int color, out IntPtr brush);

			// Token: 0x06000D7A RID: 3450
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetSolidFillColor(HandleRef brush, int color);

			// Token: 0x06000D7B RID: 3451
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetSolidFillColor(HandleRef brush, out int color);

			// Token: 0x06000D7C RID: 3452
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrush(GPPOINTF point1, GPPOINTF point2, int color1, int color2, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D7D RID: 3453
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushI(GPPOINT point1, GPPOINT point2, int color1, int color2, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D7E RID: 3454
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRect(ref GPRECTF rect, int color1, int color2, int lineGradientMode, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D7F RID: 3455
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectI(ref GPRECT rect, int color1, int color2, int lineGradientMode, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D80 RID: 3456
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectWithAngle(ref GPRECTF rect, int color1, int color2, float angle, bool isAngleScaleable, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D81 RID: 3457
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectWithAngleI(ref GPRECT rect, int color1, int color2, float angle, bool isAngleScaleable, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000D82 RID: 3458
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineColors(HandleRef brush, int color1, int color2);

			// Token: 0x06000D83 RID: 3459
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineColors(HandleRef brush, int[] colors);

			// Token: 0x06000D84 RID: 3460
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineRect(HandleRef brush, ref GPRECTF gprectf);

			// Token: 0x06000D85 RID: 3461
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineGammaCorrection(HandleRef brush, out bool useGammaCorrection);

			// Token: 0x06000D86 RID: 3462
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineGammaCorrection(HandleRef brush, bool useGammaCorrection);

			// Token: 0x06000D87 RID: 3463
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineSigmaBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000D88 RID: 3464
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineLinearBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000D89 RID: 3465
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineBlendCount(HandleRef brush, out int count);

			// Token: 0x06000D8A RID: 3466
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000D8B RID: 3467
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000D8C RID: 3468
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLinePresetBlendCount(HandleRef brush, out int count);

			// Token: 0x06000D8D RID: 3469
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLinePresetBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000D8E RID: 3470
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLinePresetBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000D8F RID: 3471
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineWrapMode(HandleRef brush, int wrapMode);

			// Token: 0x06000D90 RID: 3472
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineWrapMode(HandleRef brush, out int wrapMode);

			// Token: 0x06000D91 RID: 3473
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetLineTransform(HandleRef brush);

			// Token: 0x06000D92 RID: 3474
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyLineTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000D93 RID: 3475
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000D94 RID: 3476
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000D95 RID: 3477
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateLineTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000D96 RID: 3478
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleLineTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000D97 RID: 3479
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateLineTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000D98 RID: 3480
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradient(HandleRef points, int count, int wrapMode, out IntPtr brush);

			// Token: 0x06000D99 RID: 3481
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradientI(HandleRef points, int count, int wrapMode, out IntPtr brush);

			// Token: 0x06000D9A RID: 3482
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradientFromPath(HandleRef path, out IntPtr brush);

			// Token: 0x06000D9B RID: 3483
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientCenterColor(HandleRef brush, out int color);

			// Token: 0x06000D9C RID: 3484
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientCenterColor(HandleRef brush, int color);

			// Token: 0x06000D9D RID: 3485
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientSurroundColorsWithCount(HandleRef brush, int[] color, ref int count);

			// Token: 0x06000D9E RID: 3486
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientSurroundColorsWithCount(HandleRef brush, int[] argb, ref int count);

			// Token: 0x06000D9F RID: 3487
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientCenterPoint(HandleRef brush, GPPOINTF point);

			// Token: 0x06000DA0 RID: 3488
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientCenterPoint(HandleRef brush, GPPOINTF point);

			// Token: 0x06000DA1 RID: 3489
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientRect(HandleRef brush, ref GPRECTF gprectf);

			// Token: 0x06000DA2 RID: 3490
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPointCount(HandleRef brush, out int count);

			// Token: 0x06000DA3 RID: 3491
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientSurroundColorCount(HandleRef brush, out int count);

			// Token: 0x06000DA4 RID: 3492
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientBlendCount(HandleRef brush, out int count);

			// Token: 0x06000DA5 RID: 3493
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000DA6 RID: 3494
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000DA7 RID: 3495
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPresetBlendCount(HandleRef brush, out int count);

			// Token: 0x06000DA8 RID: 3496
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPresetBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000DA9 RID: 3497
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientPresetBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000DAA RID: 3498
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientSigmaBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000DAB RID: 3499
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientLinearBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000DAC RID: 3500
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientWrapMode(HandleRef brush, int wrapmode);

			// Token: 0x06000DAD RID: 3501
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientWrapMode(HandleRef brush, out int wrapmode);

			// Token: 0x06000DAE RID: 3502
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000DAF RID: 3503
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000DB0 RID: 3504
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPathGradientTransform(HandleRef brush);

			// Token: 0x06000DB1 RID: 3505
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyPathGradientTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000DB2 RID: 3506
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslatePathGradientTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000DB3 RID: 3507
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScalePathGradientTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000DB4 RID: 3508
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotatePathGradientTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000DB5 RID: 3509
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientFocusScales(HandleRef brush, float[] xScale, float[] yScale);

			// Token: 0x06000DB6 RID: 3510
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientFocusScales(HandleRef brush, float xScale, float yScale);

			// Token: 0x06000DB7 RID: 3511
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePen1(int argb, float width, int unit, out IntPtr pen);

			// Token: 0x06000DB8 RID: 3512
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePen2(HandleRef brush, float width, int unit, out IntPtr pen);

			// Token: 0x06000DB9 RID: 3513
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClonePen(HandleRef pen, out IntPtr clonepen);

			// Token: 0x06000DBA RID: 3514
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePen", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePen(HandleRef Pen);

			// Token: 0x06000DBB RID: 3515 RVA: 0x0002CD64 File Offset: 0x0002AF64
			internal static int GdipDeletePen(HandleRef pen)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePen(pen);
			}

			// Token: 0x06000DBC RID: 3516
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenMode(HandleRef pen, PenAlignment penAlign);

			// Token: 0x06000DBD RID: 3517
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenMode(HandleRef pen, out PenAlignment penAlign);

			// Token: 0x06000DBE RID: 3518
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenWidth(HandleRef pen, float width);

			// Token: 0x06000DBF RID: 3519
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenWidth(HandleRef pen, float[] width);

			// Token: 0x06000DC0 RID: 3520
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenLineCap197819(HandleRef pen, int startCap, int endCap, int dashCap);

			// Token: 0x06000DC1 RID: 3521
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenStartCap(HandleRef pen, int startCap);

			// Token: 0x06000DC2 RID: 3522
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenEndCap(HandleRef pen, int endCap);

			// Token: 0x06000DC3 RID: 3523
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenStartCap(HandleRef pen, out int startCap);

			// Token: 0x06000DC4 RID: 3524
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenEndCap(HandleRef pen, out int endCap);

			// Token: 0x06000DC5 RID: 3525
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashCap197819(HandleRef pen, out int dashCap);

			// Token: 0x06000DC6 RID: 3526
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashCap197819(HandleRef pen, int dashCap);

			// Token: 0x06000DC7 RID: 3527
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenLineJoin(HandleRef pen, int lineJoin);

			// Token: 0x06000DC8 RID: 3528
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenLineJoin(HandleRef pen, out int lineJoin);

			// Token: 0x06000DC9 RID: 3529
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCustomStartCap(HandleRef pen, HandleRef customCap);

			// Token: 0x06000DCA RID: 3530
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCustomStartCap(HandleRef pen, out IntPtr customCap);

			// Token: 0x06000DCB RID: 3531
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCustomEndCap(HandleRef pen, HandleRef customCap);

			// Token: 0x06000DCC RID: 3532
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCustomEndCap(HandleRef pen, out IntPtr customCap);

			// Token: 0x06000DCD RID: 3533
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenMiterLimit(HandleRef pen, float miterLimit);

			// Token: 0x06000DCE RID: 3534
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenMiterLimit(HandleRef pen, float[] miterLimit);

			// Token: 0x06000DCF RID: 3535
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenTransform(HandleRef pen, HandleRef matrix);

			// Token: 0x06000DD0 RID: 3536
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenTransform(HandleRef pen, HandleRef matrix);

			// Token: 0x06000DD1 RID: 3537
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPenTransform(HandleRef brush);

			// Token: 0x06000DD2 RID: 3538
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyPenTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000DD3 RID: 3539
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslatePenTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000DD4 RID: 3540
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScalePenTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000DD5 RID: 3541
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotatePenTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000DD6 RID: 3542
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenColor(HandleRef pen, int argb);

			// Token: 0x06000DD7 RID: 3543
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenColor(HandleRef pen, out int argb);

			// Token: 0x06000DD8 RID: 3544
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenBrushFill(HandleRef pen, HandleRef brush);

			// Token: 0x06000DD9 RID: 3545
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenBrushFill(HandleRef pen, out IntPtr brush);

			// Token: 0x06000DDA RID: 3546
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenFillType(HandleRef pen, out int pentype);

			// Token: 0x06000DDB RID: 3547
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashStyle(HandleRef pen, out int dashstyle);

			// Token: 0x06000DDC RID: 3548
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashStyle(HandleRef pen, int dashstyle);

			// Token: 0x06000DDD RID: 3549
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashArray(HandleRef pen, HandleRef memorydash, int count);

			// Token: 0x06000DDE RID: 3550
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashOffset(HandleRef pen, float[] dashoffset);

			// Token: 0x06000DDF RID: 3551
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashOffset(HandleRef pen, float dashoffset);

			// Token: 0x06000DE0 RID: 3552
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashCount(HandleRef pen, out int dashcount);

			// Token: 0x06000DE1 RID: 3553
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashArray(HandleRef pen, IntPtr memorydash, int count);

			// Token: 0x06000DE2 RID: 3554
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCompoundCount(HandleRef pen, out int count);

			// Token: 0x06000DE3 RID: 3555
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCompoundArray(HandleRef pen, float[] array, int count);

			// Token: 0x06000DE4 RID: 3556
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCompoundArray(HandleRef pen, float[] array, int count);

			// Token: 0x06000DE5 RID: 3557
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateCustomLineCap(HandleRef fillpath, HandleRef strokepath, LineCap baseCap, float baseInset, out IntPtr customCap);

			// Token: 0x06000DE6 RID: 3558
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteCustomLineCap", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteCustomLineCap(HandleRef customCap);

			// Token: 0x06000DE7 RID: 3559 RVA: 0x0002CD84 File Offset: 0x0002AF84
			internal static int GdipDeleteCustomLineCap(HandleRef customCap)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteCustomLineCap(customCap);
			}

			// Token: 0x06000DE8 RID: 3560
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneCustomLineCap(HandleRef customCap, out IntPtr clonedCap);

			// Token: 0x06000DE9 RID: 3561
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapType(HandleRef customCap, out CustomLineCapType capType);

			// Token: 0x06000DEA RID: 3562
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapStrokeCaps(HandleRef customCap, LineCap startCap, LineCap endCap);

			// Token: 0x06000DEB RID: 3563
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapStrokeCaps(HandleRef customCap, out LineCap startCap, out LineCap endCap);

			// Token: 0x06000DEC RID: 3564
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapStrokeJoin(HandleRef customCap, LineJoin lineJoin);

			// Token: 0x06000DED RID: 3565
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapStrokeJoin(HandleRef customCap, out LineJoin lineJoin);

			// Token: 0x06000DEE RID: 3566
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapBaseCap(HandleRef customCap, LineCap baseCap);

			// Token: 0x06000DEF RID: 3567
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapBaseCap(HandleRef customCap, out LineCap baseCap);

			// Token: 0x06000DF0 RID: 3568
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapBaseInset(HandleRef customCap, float inset);

			// Token: 0x06000DF1 RID: 3569
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapBaseInset(HandleRef customCap, out float inset);

			// Token: 0x06000DF2 RID: 3570
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapWidthScale(HandleRef customCap, float widthScale);

			// Token: 0x06000DF3 RID: 3571
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapWidthScale(HandleRef customCap, out float widthScale);

			// Token: 0x06000DF4 RID: 3572
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateAdjustableArrowCap(float height, float width, bool isFilled, out IntPtr adjustableArrowCap);

			// Token: 0x06000DF5 RID: 3573
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapHeight(HandleRef adjustableArrowCap, float height);

			// Token: 0x06000DF6 RID: 3574
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapHeight(HandleRef adjustableArrowCap, out float height);

			// Token: 0x06000DF7 RID: 3575
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapWidth(HandleRef adjustableArrowCap, float width);

			// Token: 0x06000DF8 RID: 3576
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapWidth(HandleRef adjustableArrowCap, out float width);

			// Token: 0x06000DF9 RID: 3577
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapMiddleInset(HandleRef adjustableArrowCap, float middleInset);

			// Token: 0x06000DFA RID: 3578
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapMiddleInset(HandleRef adjustableArrowCap, out float middleInset);

			// Token: 0x06000DFB RID: 3579
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapFillState(HandleRef adjustableArrowCap, bool fillState);

			// Token: 0x06000DFC RID: 3580
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapFillState(HandleRef adjustableArrowCap, out bool fillState);

			// Token: 0x06000DFD RID: 3581
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromStream(UnsafeNativeMethods.IStream stream, out IntPtr image);

			// Token: 0x06000DFE RID: 3582
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromFile(string filename, out IntPtr image);

			// Token: 0x06000DFF RID: 3583
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromStreamICM(UnsafeNativeMethods.IStream stream, out IntPtr image);

			// Token: 0x06000E00 RID: 3584
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromFileICM(string filename, out IntPtr image);

			// Token: 0x06000E01 RID: 3585
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneImage(HandleRef image, out IntPtr cloneimage);

			// Token: 0x06000E02 RID: 3586
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDisposeImage", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDisposeImage(HandleRef image);

			// Token: 0x06000E03 RID: 3587 RVA: 0x0002CDA4 File Offset: 0x0002AFA4
			internal static int GdipDisposeImage(HandleRef image)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDisposeImage(image);
			}

			// Token: 0x06000E04 RID: 3588
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveImageToFile(HandleRef image, string filename, ref Guid classId, HandleRef encoderParams);

			// Token: 0x06000E05 RID: 3589
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveImageToStream(HandleRef image, UnsafeNativeMethods.IStream stream, ref Guid classId, HandleRef encoderParams);

			// Token: 0x06000E06 RID: 3590
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveAdd(HandleRef image, HandleRef encoderParams);

			// Token: 0x06000E07 RID: 3591
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveAddImage(HandleRef image, HandleRef newImage, HandleRef encoderParams);

			// Token: 0x06000E08 RID: 3592
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageGraphicsContext(HandleRef image, out IntPtr graphics);

			// Token: 0x06000E09 RID: 3593
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageBounds(HandleRef image, ref GPRECTF gprectf, out GraphicsUnit unit);

			// Token: 0x06000E0A RID: 3594
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDimension(HandleRef image, out float width, out float height);

			// Token: 0x06000E0B RID: 3595
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageType(HandleRef image, out int type);

			// Token: 0x06000E0C RID: 3596
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageWidth(HandleRef image, out int width);

			// Token: 0x06000E0D RID: 3597
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageHeight(HandleRef image, out int height);

			// Token: 0x06000E0E RID: 3598
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageHorizontalResolution(HandleRef image, out float horzRes);

			// Token: 0x06000E0F RID: 3599
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageVerticalResolution(HandleRef image, out float vertRes);

			// Token: 0x06000E10 RID: 3600
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageFlags(HandleRef image, out int flags);

			// Token: 0x06000E11 RID: 3601
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

			// Token: 0x06000E12 RID: 3602
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePixelFormat(HandleRef image, out int format);

			// Token: 0x06000E13 RID: 3603
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageThumbnail(HandleRef image, int thumbWidth, int thumbHeight, out IntPtr thumbImage, Image.GetThumbnailImageAbort callback, IntPtr callbackdata);

			// Token: 0x06000E14 RID: 3604
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEncoderParameterListSize(HandleRef image, ref Guid clsid, out int size);

			// Token: 0x06000E15 RID: 3605
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEncoderParameterList(HandleRef image, ref Guid clsid, int size, IntPtr buffer);

			// Token: 0x06000E16 RID: 3606
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);

			// Token: 0x06000E17 RID: 3607
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameDimensionsList(HandleRef image, IntPtr buffer, int count);

			// Token: 0x06000E18 RID: 3608
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameCount(HandleRef image, ref Guid dimensionID, int[] count);

			// Token: 0x06000E19 RID: 3609
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageSelectActiveFrame(HandleRef image, ref Guid dimensionID, int frameIndex);

			// Token: 0x06000E1A RID: 3610
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageRotateFlip(HandleRef image, int rotateFlipType);

			// Token: 0x06000E1B RID: 3611
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePalette(HandleRef image, IntPtr palette, int size);

			// Token: 0x06000E1C RID: 3612
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImagePalette(HandleRef image, IntPtr palette);

			// Token: 0x06000E1D RID: 3613
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePaletteSize(HandleRef image, out int size);

			// Token: 0x06000E1E RID: 3614
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyCount(HandleRef image, out int count);

			// Token: 0x06000E1F RID: 3615
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyIdList(HandleRef image, int count, int[] list);

			// Token: 0x06000E20 RID: 3616
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

			// Token: 0x06000E21 RID: 3617
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyItem(HandleRef image, int propid, int size, IntPtr buffer);

			// Token: 0x06000E22 RID: 3618
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertySize(HandleRef image, out int totalSize, ref int count);

			// Token: 0x06000E23 RID: 3619
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAllPropertyItems(HandleRef image, int totalSize, int count, IntPtr buffer);

			// Token: 0x06000E24 RID: 3620
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRemovePropertyItem(HandleRef image, int propid);

			// Token: 0x06000E25 RID: 3621
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPropertyItem(HandleRef image, PropertyItemInternal propitem);

			// Token: 0x06000E26 RID: 3622
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageForceValidation(HandleRef image);

			// Token: 0x06000E27 RID: 3623
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

			// Token: 0x06000E28 RID: 3624
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

			// Token: 0x06000E29 RID: 3625
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

			// Token: 0x06000E2A RID: 3626
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

			// Token: 0x06000E2B RID: 3627
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromStream(UnsafeNativeMethods.IStream stream, out IntPtr bitmap);

			// Token: 0x06000E2C RID: 3628
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

			// Token: 0x06000E2D RID: 3629
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromStreamICM(UnsafeNativeMethods.IStream stream, out IntPtr bitmap);

			// Token: 0x06000E2E RID: 3630
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromFileICM(string filename, out IntPtr bitmap);

			// Token: 0x06000E2F RID: 3631
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromScan0(int width, int height, int stride, int format, HandleRef scan0, out IntPtr bitmap);

			// Token: 0x06000E30 RID: 3632
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromGraphics(int width, int height, HandleRef graphics, out IntPtr bitmap);

			// Token: 0x06000E31 RID: 3633
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromHBITMAP(HandleRef hbitmap, HandleRef hpalette, out IntPtr bitmap);

			// Token: 0x06000E32 RID: 3634
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromHICON(HandleRef hicon, out IntPtr bitmap);

			// Token: 0x06000E33 RID: 3635
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromResource(HandleRef hresource, HandleRef name, out IntPtr bitmap);

			// Token: 0x06000E34 RID: 3636
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

			// Token: 0x06000E35 RID: 3637
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHICONFromBitmap(HandleRef nativeBitmap, out IntPtr hicon);

			// Token: 0x06000E36 RID: 3638
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBitmapArea(float x, float y, float width, float height, int format, HandleRef srcbitmap, out IntPtr dstbitmap);

			// Token: 0x06000E37 RID: 3639
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBitmapAreaI(int x, int y, int width, int height, int format, HandleRef srcbitmap, out IntPtr dstbitmap);

			// Token: 0x06000E38 RID: 3640
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapLockBits(HandleRef bitmap, ref GPRECT rect, ImageLockMode flags, PixelFormat format, [In] [Out] BitmapData lockedBitmapData);

			// Token: 0x06000E39 RID: 3641
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapUnlockBits(HandleRef bitmap, BitmapData lockedBitmapData);

			// Token: 0x06000E3A RID: 3642
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapGetPixel(HandleRef bitmap, int x, int y, out int argb);

			// Token: 0x06000E3B RID: 3643
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapSetPixel(HandleRef bitmap, int x, int y, int argb);

			// Token: 0x06000E3C RID: 3644
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapSetResolution(HandleRef bitmap, float dpix, float dpiy);

			// Token: 0x06000E3D RID: 3645
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateImageAttributes(out IntPtr imageattr);

			// Token: 0x06000E3E RID: 3646
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneImageAttributes(HandleRef imageattr, out IntPtr cloneImageattr);

			// Token: 0x06000E3F RID: 3647
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDisposeImageAttributes", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDisposeImageAttributes(HandleRef imageattr);

			// Token: 0x06000E40 RID: 3648 RVA: 0x0002CDC4 File Offset: 0x0002AFC4
			internal static int GdipDisposeImageAttributes(HandleRef imageattr)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDisposeImageAttributes(imageattr);
			}

			// Token: 0x06000E41 RID: 3649
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesColorMatrix(HandleRef imageattr, ColorAdjustType type, bool enableFlag, ColorMatrix colorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag flags);

			// Token: 0x06000E42 RID: 3650
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesThreshold(HandleRef imageattr, ColorAdjustType type, bool enableFlag, float threshold);

			// Token: 0x06000E43 RID: 3651
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesGamma(HandleRef imageattr, ColorAdjustType type, bool enableFlag, float gamma);

			// Token: 0x06000E44 RID: 3652
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesNoOp(HandleRef imageattr, ColorAdjustType type, bool enableFlag);

			// Token: 0x06000E45 RID: 3653
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesColorKeys(HandleRef imageattr, ColorAdjustType type, bool enableFlag, int colorLow, int colorHigh);

			// Token: 0x06000E46 RID: 3654
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesOutputChannel(HandleRef imageattr, ColorAdjustType type, bool enableFlag, ColorChannelFlag flags);

			// Token: 0x06000E47 RID: 3655
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesOutputChannelColorProfile(HandleRef imageattr, ColorAdjustType type, bool enableFlag, string colorProfileFilename);

			// Token: 0x06000E48 RID: 3656
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesRemapTable(HandleRef imageattr, ColorAdjustType type, bool enableFlag, int mapSize, HandleRef map);

			// Token: 0x06000E49 RID: 3657
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesWrapMode(HandleRef imageattr, int wrapmode, int argb, bool clamp);

			// Token: 0x06000E4A RID: 3658
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageAttributesAdjustedPalette(HandleRef imageattr, HandleRef palette, ColorAdjustType type);

			// Token: 0x06000E4B RID: 3659
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFlush(HandleRef graphics, FlushIntention intention);

			// Token: 0x06000E4C RID: 3660
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHDC(HandleRef hdc, out IntPtr graphics);

			// Token: 0x06000E4D RID: 3661
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHDC2(HandleRef hdc, HandleRef hdevice, out IntPtr graphics);

			// Token: 0x06000E4E RID: 3662
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHWND(HandleRef hwnd, out IntPtr graphics);

			// Token: 0x06000E4F RID: 3663
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteGraphics", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteGraphics(HandleRef graphics);

			// Token: 0x06000E50 RID: 3664 RVA: 0x0002CDE4 File Offset: 0x0002AFE4
			internal static int GdipDeleteGraphics(HandleRef graphics)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteGraphics(graphics);
			}

			// Token: 0x06000E51 RID: 3665
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDC(HandleRef graphics, out IntPtr hdc);

			// Token: 0x06000E52 RID: 3666
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipReleaseDC", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipReleaseDC(HandleRef graphics, HandleRef hdc);

			// Token: 0x06000E53 RID: 3667 RVA: 0x0002CE04 File Offset: 0x0002B004
			internal static int GdipReleaseDC(HandleRef graphics, HandleRef hdc)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipReleaseDC(graphics, hdc);
			}

			// Token: 0x06000E54 RID: 3668
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCompositingMode(HandleRef graphics, int compositeMode);

			// Token: 0x06000E55 RID: 3669
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextRenderingHint(HandleRef graphics, TextRenderingHint textRenderingHint);

			// Token: 0x06000E56 RID: 3670
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextContrast(HandleRef graphics, int textContrast);

			// Token: 0x06000E57 RID: 3671
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetInterpolationMode(HandleRef graphics, int mode);

			// Token: 0x06000E58 RID: 3672
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCompositingMode(HandleRef graphics, out int compositeMode);

			// Token: 0x06000E59 RID: 3673
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetRenderingOrigin(HandleRef graphics, int x, int y);

			// Token: 0x06000E5A RID: 3674
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRenderingOrigin(HandleRef graphics, out int x, out int y);

			// Token: 0x06000E5B RID: 3675
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCompositingQuality(HandleRef graphics, CompositingQuality quality);

			// Token: 0x06000E5C RID: 3676
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCompositingQuality(HandleRef graphics, out CompositingQuality quality);

			// Token: 0x06000E5D RID: 3677
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetSmoothingMode(HandleRef graphics, SmoothingMode smoothingMode);

			// Token: 0x06000E5E RID: 3678
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetSmoothingMode(HandleRef graphics, out SmoothingMode smoothingMode);

			// Token: 0x06000E5F RID: 3679
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPixelOffsetMode(HandleRef graphics, PixelOffsetMode pixelOffsetMode);

			// Token: 0x06000E60 RID: 3680
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPixelOffsetMode(HandleRef graphics, out PixelOffsetMode pixelOffsetMode);

			// Token: 0x06000E61 RID: 3681
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextRenderingHint(HandleRef graphics, out TextRenderingHint textRenderingHint);

			// Token: 0x06000E62 RID: 3682
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextContrast(HandleRef graphics, out int textContrast);

			// Token: 0x06000E63 RID: 3683
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetInterpolationMode(HandleRef graphics, out int mode);

			// Token: 0x06000E64 RID: 3684
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetWorldTransform(HandleRef graphics, HandleRef matrix);

			// Token: 0x06000E65 RID: 3685
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetWorldTransform(HandleRef graphics);

			// Token: 0x06000E66 RID: 3686
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyWorldTransform(HandleRef graphics, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000E67 RID: 3687
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateWorldTransform(HandleRef graphics, float dx, float dy, MatrixOrder order);

			// Token: 0x06000E68 RID: 3688
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleWorldTransform(HandleRef graphics, float sx, float sy, MatrixOrder order);

			// Token: 0x06000E69 RID: 3689
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateWorldTransform(HandleRef graphics, float angle, MatrixOrder order);

			// Token: 0x06000E6A RID: 3690
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetWorldTransform(HandleRef graphics, HandleRef matrix);

			// Token: 0x06000E6B RID: 3691
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPageUnit(HandleRef graphics, out int unit);

			// Token: 0x06000E6C RID: 3692
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPageScale(HandleRef graphics, float[] scale);

			// Token: 0x06000E6D RID: 3693
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPageUnit(HandleRef graphics, int unit);

			// Token: 0x06000E6E RID: 3694
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPageScale(HandleRef graphics, float scale);

			// Token: 0x06000E6F RID: 3695
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDpiX(HandleRef graphics, float[] dpi);

			// Token: 0x06000E70 RID: 3696
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDpiY(HandleRef graphics, float[] dpi);

			// Token: 0x06000E71 RID: 3697
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPoints(HandleRef graphics, int destSpace, int srcSpace, IntPtr points, int count);

			// Token: 0x06000E72 RID: 3698
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPointsI(HandleRef graphics, int destSpace, int srcSpace, IntPtr points, int count);

			// Token: 0x06000E73 RID: 3699
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetNearestColor(HandleRef graphics, ref int color);

			// Token: 0x06000E74 RID: 3700
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern IntPtr GdipCreateHalftonePalette();

			// Token: 0x06000E75 RID: 3701
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLine(HandleRef graphics, HandleRef pen, float x1, float y1, float x2, float y2);

			// Token: 0x06000E76 RID: 3702
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLineI(HandleRef graphics, HandleRef pen, int x1, int y1, int x2, int y2);

			// Token: 0x06000E77 RID: 3703
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLines(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E78 RID: 3704
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLinesI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E79 RID: 3705
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawArc(HandleRef graphics, HandleRef pen, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000E7A RID: 3706
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawArcI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000E7B RID: 3707
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBezier(HandleRef graphics, HandleRef pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);

			// Token: 0x06000E7C RID: 3708
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBeziers(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E7D RID: 3709
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBeziersI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E7E RID: 3710
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangle(HandleRef graphics, HandleRef pen, float x, float y, float width, float height);

			// Token: 0x06000E7F RID: 3711
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangleI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height);

			// Token: 0x06000E80 RID: 3712
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangles(HandleRef graphics, HandleRef pen, HandleRef rects, int count);

			// Token: 0x06000E81 RID: 3713
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectanglesI(HandleRef graphics, HandleRef pen, HandleRef rects, int count);

			// Token: 0x06000E82 RID: 3714
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawEllipse(HandleRef graphics, HandleRef pen, float x, float y, float width, float height);

			// Token: 0x06000E83 RID: 3715
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawEllipseI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height);

			// Token: 0x06000E84 RID: 3716
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPie(HandleRef graphics, HandleRef pen, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000E85 RID: 3717
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPieI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000E86 RID: 3718
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPolygon(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E87 RID: 3719
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPolygonI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E88 RID: 3720
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPath(HandleRef graphics, HandleRef pen, HandleRef path);

			// Token: 0x06000E89 RID: 3721
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E8A RID: 3722
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurveI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E8B RID: 3723
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve2(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000E8C RID: 3724
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve2I(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000E8D RID: 3725
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve3(HandleRef graphics, HandleRef pen, HandleRef points, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000E8E RID: 3726
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve3I(HandleRef graphics, HandleRef pen, HandleRef points, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000E8F RID: 3727
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E90 RID: 3728
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurveI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000E91 RID: 3729
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve2(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000E92 RID: 3730
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve2I(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000E93 RID: 3731
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGraphicsClear(HandleRef graphics, int argb);

			// Token: 0x06000E94 RID: 3732
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangle(HandleRef graphics, HandleRef brush, float x, float y, float width, float height);

			// Token: 0x06000E95 RID: 3733
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangleI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height);

			// Token: 0x06000E96 RID: 3734
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangles(HandleRef graphics, HandleRef brush, HandleRef rects, int count);

			// Token: 0x06000E97 RID: 3735
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectanglesI(HandleRef graphics, HandleRef brush, HandleRef rects, int count);

			// Token: 0x06000E98 RID: 3736
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPolygon(HandleRef graphics, HandleRef brush, HandleRef points, int count, int brushMode);

			// Token: 0x06000E99 RID: 3737
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPolygonI(HandleRef graphics, HandleRef brush, HandleRef points, int count, int brushMode);

			// Token: 0x06000E9A RID: 3738
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillEllipse(HandleRef graphics, HandleRef brush, float x, float y, float width, float height);

			// Token: 0x06000E9B RID: 3739
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillEllipseI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height);

			// Token: 0x06000E9C RID: 3740
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPie(HandleRef graphics, HandleRef brush, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000E9D RID: 3741
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPieI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000E9E RID: 3742
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPath(HandleRef graphics, HandleRef brush, HandleRef path);

			// Token: 0x06000E9F RID: 3743
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve(HandleRef graphics, HandleRef brush, HandleRef points, int count);

			// Token: 0x06000EA0 RID: 3744
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurveI(HandleRef graphics, HandleRef brush, HandleRef points, int count);

			// Token: 0x06000EA1 RID: 3745
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve2(HandleRef graphics, HandleRef brush, HandleRef points, int count, float tension, int mode);

			// Token: 0x06000EA2 RID: 3746
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve2I(HandleRef graphics, HandleRef brush, HandleRef points, int count, float tension, int mode);

			// Token: 0x06000EA3 RID: 3747
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRegion(HandleRef graphics, HandleRef brush, HandleRef region);

			// Token: 0x06000EA4 RID: 3748
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImage(HandleRef graphics, HandleRef image, float x, float y);

			// Token: 0x06000EA5 RID: 3749
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageI(HandleRef graphics, HandleRef image, int x, int y);

			// Token: 0x06000EA6 RID: 3750
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRect(HandleRef graphics, HandleRef image, float x, float y, float width, float height);

			// Token: 0x06000EA7 RID: 3751
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectI(HandleRef graphics, HandleRef image, int x, int y, int width, int height);

			// Token: 0x06000EA8 RID: 3752
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePoints(HandleRef graphics, HandleRef image, HandleRef points, int count);

			// Token: 0x06000EA9 RID: 3753
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsI(HandleRef graphics, HandleRef image, HandleRef points, int count);

			// Token: 0x06000EAA RID: 3754
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointRect(HandleRef graphics, HandleRef image, float x, float y, float srcx, float srcy, float srcwidth, float srcheight, int srcunit);

			// Token: 0x06000EAB RID: 3755
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointRectI(HandleRef graphics, HandleRef image, int x, int y, int srcx, int srcy, int srcwidth, int srcheight, int srcunit);

			// Token: 0x06000EAC RID: 3756
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectRect(HandleRef graphics, HandleRef image, float dstx, float dsty, float dstwidth, float dstheight, float srcx, float srcy, float srcwidth, float srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000EAD RID: 3757
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectRectI(HandleRef graphics, HandleRef image, int dstx, int dsty, int dstwidth, int dstheight, int srcx, int srcy, int srcwidth, int srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000EAE RID: 3758
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsRect(HandleRef graphics, HandleRef image, HandleRef points, int count, float srcx, float srcy, float srcwidth, float srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000EAF RID: 3759
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsRectI(HandleRef graphics, HandleRef image, HandleRef points, int count, int srcx, int srcy, int srcwidth, int srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000EB0 RID: 3760
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPoint(HandleRef graphics, HandleRef metafile, GPPOINTF destPoint, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB1 RID: 3761
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPointI(HandleRef graphics, HandleRef metafile, GPPOINT destPoint, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB2 RID: 3762
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestRect(HandleRef graphics, HandleRef metafile, ref GPRECTF destRect, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB3 RID: 3763
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestRectI(HandleRef graphics, HandleRef metafile, ref GPRECT destRect, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB4 RID: 3764
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPoints(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB5 RID: 3765
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPointsI(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB6 RID: 3766
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPoint(HandleRef graphics, HandleRef metafile, GPPOINTF destPoint, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB7 RID: 3767
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPointI(HandleRef graphics, HandleRef metafile, GPPOINT destPoint, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB8 RID: 3768
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestRect(HandleRef graphics, HandleRef metafile, ref GPRECTF destRect, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EB9 RID: 3769
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestRectI(HandleRef graphics, HandleRef metafile, ref GPRECT destRect, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EBA RID: 3770
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPoints(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EBB RID: 3771
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPointsI(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000EBC RID: 3772
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPlayMetafileRecord(HandleRef graphics, EmfPlusRecordType recordType, int flags, int dataSize, byte[] data);

			// Token: 0x06000EBD RID: 3773
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipGraphics(HandleRef graphics, HandleRef srcgraphics, CombineMode mode);

			// Token: 0x06000EBE RID: 3774
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRect(HandleRef graphics, float x, float y, float width, float height, CombineMode mode);

			// Token: 0x06000EBF RID: 3775
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRectI(HandleRef graphics, int x, int y, int width, int height, CombineMode mode);

			// Token: 0x06000EC0 RID: 3776
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipPath(HandleRef graphics, HandleRef path, CombineMode mode);

			// Token: 0x06000EC1 RID: 3777
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRegion(HandleRef graphics, HandleRef region, CombineMode mode);

			// Token: 0x06000EC2 RID: 3778
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetClip(HandleRef graphics);

			// Token: 0x06000EC3 RID: 3779
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateClip(HandleRef graphics, float dx, float dy);

			// Token: 0x06000EC4 RID: 3780
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetClip(HandleRef graphics, HandleRef region);

			// Token: 0x06000EC5 RID: 3781
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetClipBounds(HandleRef graphics, ref GPRECTF rect);

			// Token: 0x06000EC6 RID: 3782
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsClipEmpty(HandleRef graphics, out int boolean);

			// Token: 0x06000EC7 RID: 3783
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetVisibleClipBounds(HandleRef graphics, ref GPRECTF rect);

			// Token: 0x06000EC8 RID: 3784
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleClipEmpty(HandleRef graphics, out int boolean);

			// Token: 0x06000EC9 RID: 3785
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePoint(HandleRef graphics, float x, float y, out int boolean);

			// Token: 0x06000ECA RID: 3786
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePointI(HandleRef graphics, int x, int y, out int boolean);

			// Token: 0x06000ECB RID: 3787
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRect(HandleRef graphics, float x, float y, float width, float height, out int boolean);

			// Token: 0x06000ECC RID: 3788
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRectI(HandleRef graphics, int x, int y, int width, int height, out int boolean);

			// Token: 0x06000ECD RID: 3789
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveGraphics(HandleRef graphics, out int state);

			// Token: 0x06000ECE RID: 3790
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRestoreGraphics(HandleRef graphics, int state);

			// Token: 0x06000ECF RID: 3791
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainer(HandleRef graphics, ref GPRECTF dstRect, ref GPRECTF srcRect, int unit, out int state);

			// Token: 0x06000ED0 RID: 3792
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainer2(HandleRef graphics, out int state);

			// Token: 0x06000ED1 RID: 3793
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainerI(HandleRef graphics, ref GPRECT dstRect, ref GPRECT srcRect, int unit, out int state);

			// Token: 0x06000ED2 RID: 3794
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEndContainer(HandleRef graphics, int state);

			// Token: 0x06000ED3 RID: 3795
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromWmf(HandleRef hMetafile, WmfPlaceableFileHeader wmfplaceable, [In] [Out] MetafileHeaderWmf metafileHeaderWmf);

			// Token: 0x06000ED4 RID: 3796
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromEmf(HandleRef hEnhMetafile, [In] [Out] MetafileHeaderEmf metafileHeaderEmf);

			// Token: 0x06000ED5 RID: 3797
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromFile(string filename, IntPtr header);

			// Token: 0x06000ED6 RID: 3798
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromStream(UnsafeNativeMethods.IStream stream, IntPtr header);

			// Token: 0x06000ED7 RID: 3799
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromMetafile(HandleRef metafile, IntPtr header);

			// Token: 0x06000ED8 RID: 3800
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHemfFromMetafile(HandleRef metafile, out IntPtr hEnhMetafile);

			// Token: 0x06000ED9 RID: 3801
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromWmf(HandleRef hMetafile, [MarshalAs(UnmanagedType.Bool)] bool deleteWmf, WmfPlaceableFileHeader wmfplacealbeHeader, out IntPtr metafile);

			// Token: 0x06000EDA RID: 3802
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromEmf(HandleRef hEnhMetafile, bool deleteEmf, out IntPtr metafile);

			// Token: 0x06000EDB RID: 3803
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromFile(string file, out IntPtr metafile);

			// Token: 0x06000EDC RID: 3804
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromStream(UnsafeNativeMethods.IStream stream, out IntPtr metafile);

			// Token: 0x06000EDD RID: 3805
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafile(HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EDE RID: 3806
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafile(HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EDF RID: 3807
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileI(HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE0 RID: 3808
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileName(string fileName, HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE1 RID: 3809
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileName(string fileName, HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE2 RID: 3810
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileNameI(string fileName, HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE3 RID: 3811
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStream(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE4 RID: 3812
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStream(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE5 RID: 3813
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStreamI(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000EE6 RID: 3814
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipComment(HandleRef graphics, int sizeData, byte[] data);

			// Token: 0x06000EE7 RID: 3815
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipNewInstalledFontCollection(out IntPtr fontCollection);

			// Token: 0x06000EE8 RID: 3816
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipNewPrivateFontCollection(out IntPtr fontCollection);

			// Token: 0x06000EE9 RID: 3817
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePrivateFontCollection", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePrivateFontCollection(out IntPtr fontCollection);

			// Token: 0x06000EEA RID: 3818 RVA: 0x0002CE24 File Offset: 0x0002B024
			internal static int GdipDeletePrivateFontCollection(out IntPtr fontCollection)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					fontCollection = IntPtr.Zero;
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePrivateFontCollection(out fontCollection);
			}

			// Token: 0x06000EEB RID: 3819
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontCollectionFamilyCount(HandleRef fontCollection, out int numFound);

			// Token: 0x06000EEC RID: 3820
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontCollectionFamilyList(HandleRef fontCollection, int numSought, IntPtr[] gpfamilies, out int numFound);

			// Token: 0x06000EED RID: 3821
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPrivateAddFontFile(HandleRef fontCollection, string filename);

			// Token: 0x06000EEE RID: 3822
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPrivateAddMemoryFont(HandleRef fontCollection, HandleRef memory, int length);

			// Token: 0x06000EEF RID: 3823
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFamilyFromName(string name, HandleRef fontCollection, out IntPtr FontFamily);

			// Token: 0x06000EF0 RID: 3824
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilySansSerif(out IntPtr fontfamily);

			// Token: 0x06000EF1 RID: 3825
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilySerif(out IntPtr fontfamily);

			// Token: 0x06000EF2 RID: 3826
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilyMonospace(out IntPtr fontfamily);

			// Token: 0x06000EF3 RID: 3827
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteFontFamily", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteFontFamily(HandleRef fontFamily);

			// Token: 0x06000EF4 RID: 3828 RVA: 0x0002CE4C File Offset: 0x0002B04C
			internal static int GdipDeleteFontFamily(HandleRef fontFamily)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteFontFamily(fontFamily);
			}

			// Token: 0x06000EF5 RID: 3829
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneFontFamily(HandleRef fontfamily, out IntPtr clonefontfamily);

			// Token: 0x06000EF6 RID: 3830
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFamilyName(HandleRef family, StringBuilder name, int language);

			// Token: 0x06000EF7 RID: 3831
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsStyleAvailable(HandleRef family, FontStyle style, out int isStyleAvailable);

			// Token: 0x06000EF8 RID: 3832
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEmHeight(HandleRef family, FontStyle style, out int EmHeight);

			// Token: 0x06000EF9 RID: 3833
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCellAscent(HandleRef family, FontStyle style, out int CellAscent);

			// Token: 0x06000EFA RID: 3834
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCellDescent(HandleRef family, FontStyle style, out int CellDescent);

			// Token: 0x06000EFB RID: 3835
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineSpacing(HandleRef family, FontStyle style, out int LineSpaceing);

			// Token: 0x06000EFC RID: 3836
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromDC(HandleRef hdc, ref IntPtr font);

			// Token: 0x06000EFD RID: 3837
			[DllImport("gdiplus.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromLogfontA(HandleRef hdc, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf, out IntPtr font);

			// Token: 0x06000EFE RID: 3838
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromLogfontW(HandleRef hdc, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf, out IntPtr font);

			// Token: 0x06000EFF RID: 3839
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFont(HandleRef fontFamily, float emSize, FontStyle style, GraphicsUnit unit, out IntPtr font);

			// Token: 0x06000F00 RID: 3840
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLogFontW(HandleRef font, HandleRef graphics, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

			// Token: 0x06000F01 RID: 3841
			[DllImport("gdiplus.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLogFontA(HandleRef font, HandleRef graphics, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

			// Token: 0x06000F02 RID: 3842
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneFont(HandleRef font, out IntPtr cloneFont);

			// Token: 0x06000F03 RID: 3843
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteFont", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteFont(HandleRef font);

			// Token: 0x06000F04 RID: 3844 RVA: 0x0002CE6C File Offset: 0x0002B06C
			internal static int GdipDeleteFont(HandleRef font)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteFont(font);
			}

			// Token: 0x06000F05 RID: 3845
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFamily(HandleRef font, out IntPtr family);

			// Token: 0x06000F06 RID: 3846
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontStyle(HandleRef font, out FontStyle style);

			// Token: 0x06000F07 RID: 3847
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontSize(HandleRef font, out float size);

			// Token: 0x06000F08 RID: 3848
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontHeight(HandleRef font, HandleRef graphics, out float size);

			// Token: 0x06000F09 RID: 3849
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontHeightGivenDPI(HandleRef font, float dpi, out float size);

			// Token: 0x06000F0A RID: 3850
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontUnit(HandleRef font, out GraphicsUnit unit);

			// Token: 0x06000F0B RID: 3851
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawString(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, HandleRef brush);

			// Token: 0x06000F0C RID: 3852
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMeasureString(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, [In] [Out] ref GPRECTF boundingBox, out int codepointsFitted, out int linesFilled);

			// Token: 0x06000F0D RID: 3853
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMeasureCharacterRanges(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, int characterCount, [In] [Out] IntPtr[] region);

			// Token: 0x06000F0E RID: 3854
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatMeasurableCharacterRanges(HandleRef format, int rangeCount, [In] [Out] CharacterRange[] range);

			// Token: 0x06000F0F RID: 3855
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateStringFormat(StringFormatFlags options, int language, out IntPtr format);

			// Token: 0x06000F10 RID: 3856
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStringFormatGetGenericDefault(out IntPtr format);

			// Token: 0x06000F11 RID: 3857
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStringFormatGetGenericTypographic(out IntPtr format);

			// Token: 0x06000F12 RID: 3858
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteStringFormat", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteStringFormat(HandleRef format);

			// Token: 0x06000F13 RID: 3859 RVA: 0x0002CE8C File Offset: 0x0002B08C
			internal static int GdipDeleteStringFormat(HandleRef format)
			{
				if (!SafeNativeMethods.Gdip.Initialized)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteStringFormat(format);
			}

			// Token: 0x06000F14 RID: 3860
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneStringFormat(HandleRef format, out IntPtr newFormat);

			// Token: 0x06000F15 RID: 3861
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatFlags(HandleRef format, StringFormatFlags options);

			// Token: 0x06000F16 RID: 3862
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatFlags(HandleRef format, out StringFormatFlags result);

			// Token: 0x06000F17 RID: 3863
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatAlign(HandleRef format, StringAlignment align);

			// Token: 0x06000F18 RID: 3864
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatAlign(HandleRef format, out StringAlignment align);

			// Token: 0x06000F19 RID: 3865
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatLineAlign(HandleRef format, StringAlignment align);

			// Token: 0x06000F1A RID: 3866
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatLineAlign(HandleRef format, out StringAlignment align);

			// Token: 0x06000F1B RID: 3867
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatHotkeyPrefix(HandleRef format, HotkeyPrefix hotkeyPrefix);

			// Token: 0x06000F1C RID: 3868
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatHotkeyPrefix(HandleRef format, out HotkeyPrefix hotkeyPrefix);

			// Token: 0x06000F1D RID: 3869
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatTabStops(HandleRef format, float firstTabOffset, int count, float[] tabStops);

			// Token: 0x06000F1E RID: 3870
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTabStops(HandleRef format, int count, out float firstTabOffset, [In] [Out] float[] tabStops);

			// Token: 0x06000F1F RID: 3871
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTabStopCount(HandleRef format, out int count);

			// Token: 0x06000F20 RID: 3872
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatMeasurableCharacterRangeCount(HandleRef format, out int count);

			// Token: 0x06000F21 RID: 3873
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatTrimming(HandleRef format, StringTrimming trimming);

			// Token: 0x06000F22 RID: 3874
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTrimming(HandleRef format, out StringTrimming trimming);

			// Token: 0x06000F23 RID: 3875
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatDigitSubstitution(HandleRef format, int langID, StringDigitSubstitute sds);

			// Token: 0x06000F24 RID: 3876
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatDigitSubstitution(HandleRef format, out int langID, out StringDigitSubstitute sds);

			// Token: 0x06000F25 RID: 3877 RVA: 0x0002CEAC File Offset: 0x0002B0AC
			internal static Exception StatusException(int status)
			{
				switch (status)
				{
				case 1:
					return new ExternalException(SR.GetString("GdiplusGenericError"), -2147467259);
				case 2:
					return new ArgumentException(SR.GetString("GdiplusInvalidParameter"));
				case 3:
					return new OutOfMemoryException(SR.GetString("GdiplusOutOfMemory"));
				case 4:
					return new InvalidOperationException(SR.GetString("GdiplusObjectBusy"));
				case 5:
					return new OutOfMemoryException(SR.GetString("GdiplusInsufficientBuffer"));
				case 6:
					return new NotImplementedException(SR.GetString("GdiplusNotImplemented"));
				case 7:
					return new ExternalException(SR.GetString("GdiplusGenericError"), -2147467259);
				case 8:
					return new InvalidOperationException(SR.GetString("GdiplusWrongState"));
				case 9:
					return new ExternalException(SR.GetString("GdiplusAborted"), -2147467260);
				case 10:
					return new FileNotFoundException(SR.GetString("GdiplusFileNotFound"));
				case 11:
					return new OverflowException(SR.GetString("GdiplusOverflow"));
				case 12:
					return new ExternalException(SR.GetString("GdiplusAccessDenied"), -2147024891);
				case 13:
					return new ArgumentException(SR.GetString("GdiplusUnknownImageFormat"));
				case 14:
					return new ArgumentException(SR.GetString("GdiplusFontFamilyNotFound", new object[]
					{
						"?"
					}));
				case 15:
					return new ArgumentException(SR.GetString("GdiplusFontStyleNotFound", new object[]
					{
						"?",
						"?"
					}));
				case 16:
					return new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont_NoName"));
				case 17:
					return new ExternalException(SR.GetString("GdiplusUnsupportedGdiplusVersion"), -2147467259);
				case 18:
					return new ExternalException(SR.GetString("GdiplusNotInitialized"), -2147467259);
				case 19:
					return new ArgumentException(SR.GetString("GdiplusPropertyNotFoundError"));
				case 20:
					return new ArgumentException(SR.GetString("GdiplusPropertyNotSupportedError"));
				default:
					return new ExternalException(SR.GetString("GdiplusUnknown"), -2147418113);
				}
			}

			// Token: 0x06000F26 RID: 3878 RVA: 0x0002D0AC File Offset: 0x0002B2AC
			internal static PointF[] ConvertGPPOINTFArrayF(IntPtr memory, int count)
			{
				if (memory == IntPtr.Zero)
				{
					throw new ArgumentNullException("memory");
				}
				PointF[] array = new PointF[count];
				GPPOINTF gppointf = new GPPOINTF();
				int num = Marshal.SizeOf(gppointf.GetType());
				for (int i = 0; i < count; i++)
				{
					gppointf = (GPPOINTF)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)memory + (long)(i * num)), gppointf.GetType());
					array[i] = new PointF(gppointf.X, gppointf.Y);
				}
				return array;
			}

			// Token: 0x06000F27 RID: 3879 RVA: 0x0002D130 File Offset: 0x0002B330
			internal static Point[] ConvertGPPOINTArray(IntPtr memory, int count)
			{
				if (memory == IntPtr.Zero)
				{
					throw new ArgumentNullException("memory");
				}
				Point[] array = new Point[count];
				GPPOINT gppoint = new GPPOINT();
				int num = Marshal.SizeOf(gppoint.GetType());
				for (int i = 0; i < count; i++)
				{
					gppoint = (GPPOINT)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)memory + (long)(i * num)), gppoint.GetType());
					array[i] = new Point(gppoint.X, gppoint.Y);
				}
				return array;
			}

			// Token: 0x06000F28 RID: 3880 RVA: 0x0002D1B4 File Offset: 0x0002B3B4
			internal static IntPtr ConvertPointToMemory(PointF[] points)
			{
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				int num = Marshal.SizeOf(typeof(GPPOINTF));
				int num2 = points.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					Marshal.StructureToPtr(new GPPOINTF(points[i]), (IntPtr)(checked((long)intPtr + unchecked((long)(checked(i * num))))), false);
				}
				return intPtr;
			}

			// Token: 0x06000F29 RID: 3881 RVA: 0x0002D21C File Offset: 0x0002B41C
			internal static IntPtr ConvertPointToMemory(Point[] points)
			{
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				int num = Marshal.SizeOf(typeof(GPPOINT));
				int num2 = points.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					Marshal.StructureToPtr(new GPPOINT(points[i]), (IntPtr)(checked((long)intPtr + unchecked((long)(checked(i * num))))), false);
				}
				return intPtr;
			}

			// Token: 0x06000F2A RID: 3882 RVA: 0x0002D284 File Offset: 0x0002B484
			internal static IntPtr ConvertRectangleToMemory(RectangleF[] rect)
			{
				if (rect == null)
				{
					throw new ArgumentNullException("rect");
				}
				int num = Marshal.SizeOf(typeof(GPRECTF));
				int num2 = rect.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					Marshal.StructureToPtr(new GPRECTF(rect[i]), (IntPtr)(checked((long)intPtr + unchecked((long)(checked(i * num))))), false);
				}
				return intPtr;
			}

			// Token: 0x06000F2B RID: 3883 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
			internal static IntPtr ConvertRectangleToMemory(Rectangle[] rect)
			{
				if (rect == null)
				{
					throw new ArgumentNullException("rect");
				}
				int num = Marshal.SizeOf(typeof(GPRECT));
				int num2 = rect.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					Marshal.StructureToPtr(new GPRECT(rect[i]), (IntPtr)(checked((long)intPtr + unchecked((long)(checked(i * num))))), false);
				}
				return intPtr;
			}

			// Token: 0x04000B2D RID: 2861
			private static readonly TraceSwitch GdiPlusInitialization = new TraceSwitch("GdiPlusInitialization", "Tracks GDI+ initialization and teardown");

			// Token: 0x04000B2E RID: 2862
			private static IntPtr initToken;

			// Token: 0x04000B2F RID: 2863
			private const string ThreadDataSlotName = "system.drawing.threaddata";

			// Token: 0x04000B30 RID: 2864
			internal const int Ok = 0;

			// Token: 0x04000B31 RID: 2865
			internal const int GenericError = 1;

			// Token: 0x04000B32 RID: 2866
			internal const int InvalidParameter = 2;

			// Token: 0x04000B33 RID: 2867
			internal const int OutOfMemory = 3;

			// Token: 0x04000B34 RID: 2868
			internal const int ObjectBusy = 4;

			// Token: 0x04000B35 RID: 2869
			internal const int InsufficientBuffer = 5;

			// Token: 0x04000B36 RID: 2870
			internal const int NotImplemented = 6;

			// Token: 0x04000B37 RID: 2871
			internal const int Win32Error = 7;

			// Token: 0x04000B38 RID: 2872
			internal const int WrongState = 8;

			// Token: 0x04000B39 RID: 2873
			internal const int Aborted = 9;

			// Token: 0x04000B3A RID: 2874
			internal const int FileNotFound = 10;

			// Token: 0x04000B3B RID: 2875
			internal const int ValueOverflow = 11;

			// Token: 0x04000B3C RID: 2876
			internal const int AccessDenied = 12;

			// Token: 0x04000B3D RID: 2877
			internal const int UnknownImageFormat = 13;

			// Token: 0x04000B3E RID: 2878
			internal const int FontFamilyNotFound = 14;

			// Token: 0x04000B3F RID: 2879
			internal const int FontStyleNotFound = 15;

			// Token: 0x04000B40 RID: 2880
			internal const int NotTrueTypeFont = 16;

			// Token: 0x04000B41 RID: 2881
			internal const int UnsupportedGdiplusVersion = 17;

			// Token: 0x04000B42 RID: 2882
			internal const int GdiplusNotInitialized = 18;

			// Token: 0x04000B43 RID: 2883
			internal const int PropertyNotFound = 19;

			// Token: 0x04000B44 RID: 2884
			internal const int PropertyNotSupported = 20;

			// Token: 0x02000134 RID: 308
			private struct StartupInput
			{
				// Token: 0x06000FB4 RID: 4020 RVA: 0x0002E6B0 File Offset: 0x0002C8B0
				public static SafeNativeMethods.Gdip.StartupInput GetDefault()
				{
					return new SafeNativeMethods.Gdip.StartupInput
					{
						GdiplusVersion = 1,
						SuppressBackgroundThread = false,
						SuppressExternalCodecs = false
					};
				}

				// Token: 0x04000CE3 RID: 3299
				public int GdiplusVersion;

				// Token: 0x04000CE4 RID: 3300
				public IntPtr DebugEventCallback;

				// Token: 0x04000CE5 RID: 3301
				public bool SuppressBackgroundThread;

				// Token: 0x04000CE6 RID: 3302
				public bool SuppressExternalCodecs;
			}

			// Token: 0x02000135 RID: 309
			private struct StartupOutput
			{
				// Token: 0x04000CE7 RID: 3303
				public IntPtr hook;

				// Token: 0x04000CE8 RID: 3304
				public IntPtr unhook;
			}

			// Token: 0x02000136 RID: 310
			private enum DebugEventLevel
			{
				// Token: 0x04000CEA RID: 3306
				Fatal,
				// Token: 0x04000CEB RID: 3307
				Warning
			}
		}

		// Token: 0x02000106 RID: 262
		[StructLayout(LayoutKind.Sequential)]
		public class ENHMETAHEADER
		{
			// Token: 0x04000B45 RID: 2885
			public int iType;

			// Token: 0x04000B46 RID: 2886
			public int nSize = 40;

			// Token: 0x04000B47 RID: 2887
			public int rclBounds_left;

			// Token: 0x04000B48 RID: 2888
			public int rclBounds_top;

			// Token: 0x04000B49 RID: 2889
			public int rclBounds_right;

			// Token: 0x04000B4A RID: 2890
			public int rclBounds_bottom;

			// Token: 0x04000B4B RID: 2891
			public int rclFrame_left;

			// Token: 0x04000B4C RID: 2892
			public int rclFrame_top;

			// Token: 0x04000B4D RID: 2893
			public int rclFrame_right;

			// Token: 0x04000B4E RID: 2894
			public int rclFrame_bottom;

			// Token: 0x04000B4F RID: 2895
			public int dSignature;

			// Token: 0x04000B50 RID: 2896
			public int nVersion;

			// Token: 0x04000B51 RID: 2897
			public int nBytes;

			// Token: 0x04000B52 RID: 2898
			public int nRecords;

			// Token: 0x04000B53 RID: 2899
			public short nHandles;

			// Token: 0x04000B54 RID: 2900
			public short sReserved;

			// Token: 0x04000B55 RID: 2901
			public int nDescription;

			// Token: 0x04000B56 RID: 2902
			public int offDescription;

			// Token: 0x04000B57 RID: 2903
			public int nPalEntries;

			// Token: 0x04000B58 RID: 2904
			public int szlDevice_cx;

			// Token: 0x04000B59 RID: 2905
			public int szlDevice_cy;

			// Token: 0x04000B5A RID: 2906
			public int szlMillimeters_cx;

			// Token: 0x04000B5B RID: 2907
			public int szlMillimeters_cy;

			// Token: 0x04000B5C RID: 2908
			public int cbPixelFormat;

			// Token: 0x04000B5D RID: 2909
			public int offPixelFormat;

			// Token: 0x04000B5E RID: 2910
			public int bOpenGL;
		}

		// Token: 0x02000107 RID: 263
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class DOCINFO
		{
			// Token: 0x04000B5F RID: 2911
			public int cbSize = 20;

			// Token: 0x04000B60 RID: 2912
			public string lpszDocName;

			// Token: 0x04000B61 RID: 2913
			public string lpszOutput;

			// Token: 0x04000B62 RID: 2914
			public string lpszDatatype;

			// Token: 0x04000B63 RID: 2915
			public int fwType;
		}

		// Token: 0x02000108 RID: 264
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class PRINTDLG
		{
			// Token: 0x04000B64 RID: 2916
			public int lStructSize;

			// Token: 0x04000B65 RID: 2917
			public IntPtr hwndOwner;

			// Token: 0x04000B66 RID: 2918
			public IntPtr hDevMode;

			// Token: 0x04000B67 RID: 2919
			public IntPtr hDevNames;

			// Token: 0x04000B68 RID: 2920
			public IntPtr hDC;

			// Token: 0x04000B69 RID: 2921
			public int Flags;

			// Token: 0x04000B6A RID: 2922
			public short nFromPage;

			// Token: 0x04000B6B RID: 2923
			public short nToPage;

			// Token: 0x04000B6C RID: 2924
			public short nMinPage;

			// Token: 0x04000B6D RID: 2925
			public short nMaxPage;

			// Token: 0x04000B6E RID: 2926
			public short nCopies;

			// Token: 0x04000B6F RID: 2927
			public IntPtr hInstance;

			// Token: 0x04000B70 RID: 2928
			public IntPtr lCustData;

			// Token: 0x04000B71 RID: 2929
			public IntPtr lpfnPrintHook;

			// Token: 0x04000B72 RID: 2930
			public IntPtr lpfnSetupHook;

			// Token: 0x04000B73 RID: 2931
			public string lpPrintTemplateName;

			// Token: 0x04000B74 RID: 2932
			public string lpSetupTemplateName;

			// Token: 0x04000B75 RID: 2933
			public IntPtr hPrintTemplate;

			// Token: 0x04000B76 RID: 2934
			public IntPtr hSetupTemplate;
		}

		// Token: 0x02000109 RID: 265
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class PRINTDLGX86
		{
			// Token: 0x04000B77 RID: 2935
			public int lStructSize;

			// Token: 0x04000B78 RID: 2936
			public IntPtr hwndOwner;

			// Token: 0x04000B79 RID: 2937
			public IntPtr hDevMode;

			// Token: 0x04000B7A RID: 2938
			public IntPtr hDevNames;

			// Token: 0x04000B7B RID: 2939
			public IntPtr hDC;

			// Token: 0x04000B7C RID: 2940
			public int Flags;

			// Token: 0x04000B7D RID: 2941
			public short nFromPage;

			// Token: 0x04000B7E RID: 2942
			public short nToPage;

			// Token: 0x04000B7F RID: 2943
			public short nMinPage;

			// Token: 0x04000B80 RID: 2944
			public short nMaxPage;

			// Token: 0x04000B81 RID: 2945
			public short nCopies;

			// Token: 0x04000B82 RID: 2946
			public IntPtr hInstance;

			// Token: 0x04000B83 RID: 2947
			public IntPtr lCustData;

			// Token: 0x04000B84 RID: 2948
			public IntPtr lpfnPrintHook;

			// Token: 0x04000B85 RID: 2949
			public IntPtr lpfnSetupHook;

			// Token: 0x04000B86 RID: 2950
			public string lpPrintTemplateName;

			// Token: 0x04000B87 RID: 2951
			public string lpSetupTemplateName;

			// Token: 0x04000B88 RID: 2952
			public IntPtr hPrintTemplate;

			// Token: 0x04000B89 RID: 2953
			public IntPtr hSetupTemplate;
		}

		// Token: 0x0200010A RID: 266
		public enum StructFormat
		{
			// Token: 0x04000B8B RID: 2955
			Ansi = 1,
			// Token: 0x04000B8C RID: 2956
			Unicode,
			// Token: 0x04000B8D RID: 2957
			Auto
		}

		// Token: 0x0200010B RID: 267
		public struct RECT
		{
			// Token: 0x04000B8E RID: 2958
			public int left;

			// Token: 0x04000B8F RID: 2959
			public int top;

			// Token: 0x04000B90 RID: 2960
			public int right;

			// Token: 0x04000B91 RID: 2961
			public int bottom;
		}

		// Token: 0x0200010C RID: 268
		public struct MSG
		{
			// Token: 0x04000B92 RID: 2962
			public IntPtr hwnd;

			// Token: 0x04000B93 RID: 2963
			public int message;

			// Token: 0x04000B94 RID: 2964
			public IntPtr wParam;

			// Token: 0x04000B95 RID: 2965
			public IntPtr lParam;

			// Token: 0x04000B96 RID: 2966
			public int time;

			// Token: 0x04000B97 RID: 2967
			public int pt_x;

			// Token: 0x04000B98 RID: 2968
			public int pt_y;
		}

		// Token: 0x0200010D RID: 269
		[StructLayout(LayoutKind.Sequential)]
		public class ICONINFO
		{
			// Token: 0x04000B99 RID: 2969
			public int fIcon;

			// Token: 0x04000B9A RID: 2970
			public int xHotspot;

			// Token: 0x04000B9B RID: 2971
			public int yHotspot;

			// Token: 0x04000B9C RID: 2972
			public IntPtr hbmMask = IntPtr.Zero;

			// Token: 0x04000B9D RID: 2973
			public IntPtr hbmColor = IntPtr.Zero;
		}

		// Token: 0x0200010E RID: 270
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAP
		{
			// Token: 0x04000B9E RID: 2974
			public int bmType;

			// Token: 0x04000B9F RID: 2975
			public int bmWidth;

			// Token: 0x04000BA0 RID: 2976
			public int bmHeight;

			// Token: 0x04000BA1 RID: 2977
			public int bmWidthBytes;

			// Token: 0x04000BA2 RID: 2978
			public short bmPlanes;

			// Token: 0x04000BA3 RID: 2979
			public short bmBitsPixel;

			// Token: 0x04000BA4 RID: 2980
			public IntPtr bmBits = IntPtr.Zero;
		}

		// Token: 0x0200010F RID: 271
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAPINFOHEADER
		{
			// Token: 0x04000BA5 RID: 2981
			public int biSize = 40;

			// Token: 0x04000BA6 RID: 2982
			public int biWidth;

			// Token: 0x04000BA7 RID: 2983
			public int biHeight;

			// Token: 0x04000BA8 RID: 2984
			public short biPlanes;

			// Token: 0x04000BA9 RID: 2985
			public short biBitCount;

			// Token: 0x04000BAA RID: 2986
			public int biCompression;

			// Token: 0x04000BAB RID: 2987
			public int biSizeImage;

			// Token: 0x04000BAC RID: 2988
			public int biXPelsPerMeter;

			// Token: 0x04000BAD RID: 2989
			public int biYPelsPerMeter;

			// Token: 0x04000BAE RID: 2990
			public int biClrUsed;

			// Token: 0x04000BAF RID: 2991
			public int biClrImportant;
		}

		// Token: 0x02000110 RID: 272
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x06000F34 RID: 3892 RVA: 0x00003800 File Offset: 0x00001A00
			public LOGFONT()
			{
			}

			// Token: 0x06000F35 RID: 3893 RVA: 0x0002D3C0 File Offset: 0x0002B5C0
			public LOGFONT(SafeNativeMethods.LOGFONT lf)
			{
				this.lfHeight = lf.lfHeight;
				this.lfWidth = lf.lfWidth;
				this.lfEscapement = lf.lfEscapement;
				this.lfOrientation = lf.lfOrientation;
				this.lfWeight = lf.lfWeight;
				this.lfItalic = lf.lfItalic;
				this.lfUnderline = lf.lfUnderline;
				this.lfStrikeOut = lf.lfStrikeOut;
				this.lfCharSet = lf.lfCharSet;
				this.lfOutPrecision = lf.lfOutPrecision;
				this.lfClipPrecision = lf.lfClipPrecision;
				this.lfQuality = lf.lfQuality;
				this.lfPitchAndFamily = lf.lfPitchAndFamily;
				this.lfFaceName = lf.lfFaceName;
			}

			// Token: 0x06000F36 RID: 3894 RVA: 0x0002D47C File Offset: 0x0002B67C
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"lfHeight=",
					this.lfHeight,
					", lfWidth=",
					this.lfWidth,
					", lfEscapement=",
					this.lfEscapement,
					", lfOrientation=",
					this.lfOrientation,
					", lfWeight=",
					this.lfWeight,
					", lfItalic=",
					this.lfItalic,
					", lfUnderline=",
					this.lfUnderline,
					", lfStrikeOut=",
					this.lfStrikeOut,
					", lfCharSet=",
					this.lfCharSet,
					", lfOutPrecision=",
					this.lfOutPrecision,
					", lfClipPrecision=",
					this.lfClipPrecision,
					", lfQuality=",
					this.lfQuality,
					", lfPitchAndFamily=",
					this.lfPitchAndFamily,
					", lfFaceName=",
					this.lfFaceName
				});
			}

			// Token: 0x04000BB0 RID: 2992
			public int lfHeight;

			// Token: 0x04000BB1 RID: 2993
			public int lfWidth;

			// Token: 0x04000BB2 RID: 2994
			public int lfEscapement;

			// Token: 0x04000BB3 RID: 2995
			public int lfOrientation;

			// Token: 0x04000BB4 RID: 2996
			public int lfWeight;

			// Token: 0x04000BB5 RID: 2997
			public byte lfItalic;

			// Token: 0x04000BB6 RID: 2998
			public byte lfUnderline;

			// Token: 0x04000BB7 RID: 2999
			public byte lfStrikeOut;

			// Token: 0x04000BB8 RID: 3000
			public byte lfCharSet;

			// Token: 0x04000BB9 RID: 3001
			public byte lfOutPrecision;

			// Token: 0x04000BBA RID: 3002
			public byte lfClipPrecision;

			// Token: 0x04000BBB RID: 3003
			public byte lfQuality;

			// Token: 0x04000BBC RID: 3004
			public byte lfPitchAndFamily;

			// Token: 0x04000BBD RID: 3005
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x02000111 RID: 273
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04000BBE RID: 3006
			public int tmHeight;

			// Token: 0x04000BBF RID: 3007
			public int tmAscent;

			// Token: 0x04000BC0 RID: 3008
			public int tmDescent;

			// Token: 0x04000BC1 RID: 3009
			public int tmInternalLeading;

			// Token: 0x04000BC2 RID: 3010
			public int tmExternalLeading;

			// Token: 0x04000BC3 RID: 3011
			public int tmAveCharWidth;

			// Token: 0x04000BC4 RID: 3012
			public int tmMaxCharWidth;

			// Token: 0x04000BC5 RID: 3013
			public int tmWeight;

			// Token: 0x04000BC6 RID: 3014
			public int tmOverhang;

			// Token: 0x04000BC7 RID: 3015
			public int tmDigitizedAspectX;

			// Token: 0x04000BC8 RID: 3016
			public int tmDigitizedAspectY;

			// Token: 0x04000BC9 RID: 3017
			public char tmFirstChar;

			// Token: 0x04000BCA RID: 3018
			public char tmLastChar;

			// Token: 0x04000BCB RID: 3019
			public char tmDefaultChar;

			// Token: 0x04000BCC RID: 3020
			public char tmBreakChar;

			// Token: 0x04000BCD RID: 3021
			public byte tmItalic;

			// Token: 0x04000BCE RID: 3022
			public byte tmUnderlined;

			// Token: 0x04000BCF RID: 3023
			public byte tmStruckOut;

			// Token: 0x04000BD0 RID: 3024
			public byte tmPitchAndFamily;

			// Token: 0x04000BD1 RID: 3025
			public byte tmCharSet;
		}

		// Token: 0x02000112 RID: 274
		public struct TEXTMETRICA
		{
			// Token: 0x04000BD2 RID: 3026
			public int tmHeight;

			// Token: 0x04000BD3 RID: 3027
			public int tmAscent;

			// Token: 0x04000BD4 RID: 3028
			public int tmDescent;

			// Token: 0x04000BD5 RID: 3029
			public int tmInternalLeading;

			// Token: 0x04000BD6 RID: 3030
			public int tmExternalLeading;

			// Token: 0x04000BD7 RID: 3031
			public int tmAveCharWidth;

			// Token: 0x04000BD8 RID: 3032
			public int tmMaxCharWidth;

			// Token: 0x04000BD9 RID: 3033
			public int tmWeight;

			// Token: 0x04000BDA RID: 3034
			public int tmOverhang;

			// Token: 0x04000BDB RID: 3035
			public int tmDigitizedAspectX;

			// Token: 0x04000BDC RID: 3036
			public int tmDigitizedAspectY;

			// Token: 0x04000BDD RID: 3037
			public byte tmFirstChar;

			// Token: 0x04000BDE RID: 3038
			public byte tmLastChar;

			// Token: 0x04000BDF RID: 3039
			public byte tmDefaultChar;

			// Token: 0x04000BE0 RID: 3040
			public byte tmBreakChar;

			// Token: 0x04000BE1 RID: 3041
			public byte tmItalic;

			// Token: 0x04000BE2 RID: 3042
			public byte tmUnderlined;

			// Token: 0x04000BE3 RID: 3043
			public byte tmStruckOut;

			// Token: 0x04000BE4 RID: 3044
			public byte tmPitchAndFamily;

			// Token: 0x04000BE5 RID: 3045
			public byte tmCharSet;
		}

		// Token: 0x02000113 RID: 275
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct ICONDIR
		{
			// Token: 0x04000BE6 RID: 3046
			public short idReserved;

			// Token: 0x04000BE7 RID: 3047
			public short idType;

			// Token: 0x04000BE8 RID: 3048
			public short idCount;

			// Token: 0x04000BE9 RID: 3049
			public SafeNativeMethods.ICONDIRENTRY idEntries;
		}

		// Token: 0x02000114 RID: 276
		public struct ICONDIRENTRY
		{
			// Token: 0x04000BEA RID: 3050
			public byte bWidth;

			// Token: 0x04000BEB RID: 3051
			public byte bHeight;

			// Token: 0x04000BEC RID: 3052
			public byte bColorCount;

			// Token: 0x04000BED RID: 3053
			public byte bReserved;

			// Token: 0x04000BEE RID: 3054
			public short wPlanes;

			// Token: 0x04000BEF RID: 3055
			public short wBitCount;

			// Token: 0x04000BF0 RID: 3056
			public int dwBytesInRes;

			// Token: 0x04000BF1 RID: 3057
			public int dwImageOffset;
		}

		// Token: 0x02000115 RID: 277
		public class Ole
		{
			// Token: 0x04000BF2 RID: 3058
			public const int PICTYPE_UNINITIALIZED = -1;

			// Token: 0x04000BF3 RID: 3059
			public const int PICTYPE_NONE = 0;

			// Token: 0x04000BF4 RID: 3060
			public const int PICTYPE_BITMAP = 1;

			// Token: 0x04000BF5 RID: 3061
			public const int PICTYPE_METAFILE = 2;

			// Token: 0x04000BF6 RID: 3062
			public const int PICTYPE_ICON = 3;

			// Token: 0x04000BF7 RID: 3063
			public const int PICTYPE_ENHMETAFILE = 4;

			// Token: 0x04000BF8 RID: 3064
			public const int STATFLAG_DEFAULT = 0;

			// Token: 0x04000BF9 RID: 3065
			public const int STATFLAG_NONAME = 1;
		}

		// Token: 0x02000116 RID: 278
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESC
		{
			// Token: 0x06000F38 RID: 3896 RVA: 0x0002D5D8 File Offset: 0x0002B7D8
			public static SafeNativeMethods.PICTDESC CreateIconPICTDESC(IntPtr hicon)
			{
				return new SafeNativeMethods.PICTDESC
				{
					cbSizeOfStruct = 12,
					picType = 3,
					union1 = hicon
				};
			}

			// Token: 0x06000F39 RID: 3897 RVA: 0x0002D602 File Offset: 0x0002B802
			public virtual IntPtr GetHandle()
			{
				return this.union1;
			}

			// Token: 0x04000BFA RID: 3066
			internal int cbSizeOfStruct;

			// Token: 0x04000BFB RID: 3067
			public int picType;

			// Token: 0x04000BFC RID: 3068
			internal IntPtr union1;

			// Token: 0x04000BFD RID: 3069
			internal int union2;

			// Token: 0x04000BFE RID: 3070
			internal int union3;
		}

		// Token: 0x02000117 RID: 279
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class DEVMODE
		{
			// Token: 0x06000F3B RID: 3899 RVA: 0x0002D60C File Offset: 0x0002B80C
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"[DEVMODE: dmDeviceName=",
					this.dmDeviceName,
					", dmSpecVersion=",
					this.dmSpecVersion,
					", dmDriverVersion=",
					this.dmDriverVersion,
					", dmSize=",
					this.dmSize,
					", dmDriverExtra=",
					this.dmDriverExtra,
					", dmFields=",
					this.dmFields,
					", dmOrientation=",
					this.dmOrientation,
					", dmPaperSize=",
					this.dmPaperSize,
					", dmPaperLength=",
					this.dmPaperLength,
					", dmPaperWidth=",
					this.dmPaperWidth,
					", dmScale=",
					this.dmScale,
					", dmCopies=",
					this.dmCopies,
					", dmDefaultSource=",
					this.dmDefaultSource,
					", dmPrintQuality=",
					this.dmPrintQuality,
					", dmColor=",
					this.dmColor,
					", dmDuplex=",
					this.dmDuplex,
					", dmYResolution=",
					this.dmYResolution,
					", dmTTOption=",
					this.dmTTOption,
					", dmCollate=",
					this.dmCollate,
					", dmFormName=",
					this.dmFormName,
					", dmLogPixels=",
					this.dmLogPixels,
					", dmBitsPerPel=",
					this.dmBitsPerPel,
					", dmPelsWidth=",
					this.dmPelsWidth,
					", dmPelsHeight=",
					this.dmPelsHeight,
					", dmDisplayFlags=",
					this.dmDisplayFlags,
					", dmDisplayFrequency=",
					this.dmDisplayFrequency,
					", dmICMMethod=",
					this.dmICMMethod,
					", dmICMIntent=",
					this.dmICMIntent,
					", dmMediaType=",
					this.dmMediaType,
					", dmDitherType=",
					this.dmDitherType,
					", dmICCManufacturer=",
					this.dmICCManufacturer,
					", dmICCModel=",
					this.dmICCModel,
					", dmPanningWidth=",
					this.dmPanningWidth,
					", dmPanningHeight=",
					this.dmPanningHeight,
					"]"
				});
			}

			// Token: 0x04000BFF RID: 3071
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			// Token: 0x04000C00 RID: 3072
			public short dmSpecVersion;

			// Token: 0x04000C01 RID: 3073
			public short dmDriverVersion;

			// Token: 0x04000C02 RID: 3074
			public short dmSize;

			// Token: 0x04000C03 RID: 3075
			public short dmDriverExtra;

			// Token: 0x04000C04 RID: 3076
			public int dmFields;

			// Token: 0x04000C05 RID: 3077
			public short dmOrientation;

			// Token: 0x04000C06 RID: 3078
			public short dmPaperSize;

			// Token: 0x04000C07 RID: 3079
			public short dmPaperLength;

			// Token: 0x04000C08 RID: 3080
			public short dmPaperWidth;

			// Token: 0x04000C09 RID: 3081
			public short dmScale;

			// Token: 0x04000C0A RID: 3082
			public short dmCopies;

			// Token: 0x04000C0B RID: 3083
			public short dmDefaultSource;

			// Token: 0x04000C0C RID: 3084
			public short dmPrintQuality;

			// Token: 0x04000C0D RID: 3085
			public short dmColor;

			// Token: 0x04000C0E RID: 3086
			public short dmDuplex;

			// Token: 0x04000C0F RID: 3087
			public short dmYResolution;

			// Token: 0x04000C10 RID: 3088
			public short dmTTOption;

			// Token: 0x04000C11 RID: 3089
			public short dmCollate;

			// Token: 0x04000C12 RID: 3090
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			// Token: 0x04000C13 RID: 3091
			public short dmLogPixels;

			// Token: 0x04000C14 RID: 3092
			public int dmBitsPerPel;

			// Token: 0x04000C15 RID: 3093
			public int dmPelsWidth;

			// Token: 0x04000C16 RID: 3094
			public int dmPelsHeight;

			// Token: 0x04000C17 RID: 3095
			public int dmDisplayFlags;

			// Token: 0x04000C18 RID: 3096
			public int dmDisplayFrequency;

			// Token: 0x04000C19 RID: 3097
			public int dmICMMethod;

			// Token: 0x04000C1A RID: 3098
			public int dmICMIntent;

			// Token: 0x04000C1B RID: 3099
			public int dmMediaType;

			// Token: 0x04000C1C RID: 3100
			public int dmDitherType;

			// Token: 0x04000C1D RID: 3101
			public int dmICCManufacturer;

			// Token: 0x04000C1E RID: 3102
			public int dmICCModel;

			// Token: 0x04000C1F RID: 3103
			public int dmPanningWidth;

			// Token: 0x04000C20 RID: 3104
			public int dmPanningHeight;
		}

		// Token: 0x02000118 RID: 280
		public sealed class CommonHandles
		{
			// Token: 0x04000C21 RID: 3105
			public static readonly int Accelerator = System.Internal.HandleCollector.RegisterType("Accelerator", 80, 50);

			// Token: 0x04000C22 RID: 3106
			public static readonly int Cursor = System.Internal.HandleCollector.RegisterType("Cursor", 20, 500);

			// Token: 0x04000C23 RID: 3107
			public static readonly int EMF = System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x04000C24 RID: 3108
			public static readonly int Find = System.Internal.HandleCollector.RegisterType("Find", 0, 1000);

			// Token: 0x04000C25 RID: 3109
			public static readonly int GDI = System.Internal.HandleCollector.RegisterType("GDI", 50, 500);

			// Token: 0x04000C26 RID: 3110
			public static readonly int HDC = System.Internal.HandleCollector.RegisterType("HDC", 100, 2);

			// Token: 0x04000C27 RID: 3111
			public static readonly int Icon = System.Internal.HandleCollector.RegisterType("Icon", 20, 500);

			// Token: 0x04000C28 RID: 3112
			public static readonly int Kernel = System.Internal.HandleCollector.RegisterType("Kernel", 0, 1000);

			// Token: 0x04000C29 RID: 3113
			public static readonly int Menu = System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);

			// Token: 0x04000C2A RID: 3114
			public static readonly int Window = System.Internal.HandleCollector.RegisterType("Window", 5, 1000);
		}

		// Token: 0x02000119 RID: 281
		public class StreamConsts
		{
			// Token: 0x04000C2B RID: 3115
			public const int LOCK_WRITE = 1;

			// Token: 0x04000C2C RID: 3116
			public const int LOCK_EXCLUSIVE = 2;

			// Token: 0x04000C2D RID: 3117
			public const int LOCK_ONLYONCE = 4;

			// Token: 0x04000C2E RID: 3118
			public const int STATFLAG_DEFAULT = 0;

			// Token: 0x04000C2F RID: 3119
			public const int STATFLAG_NONAME = 1;

			// Token: 0x04000C30 RID: 3120
			public const int STATFLAG_NOOPEN = 2;

			// Token: 0x04000C31 RID: 3121
			public const int STGC_DEFAULT = 0;

			// Token: 0x04000C32 RID: 3122
			public const int STGC_OVERWRITE = 1;

			// Token: 0x04000C33 RID: 3123
			public const int STGC_ONLYIFCURRENT = 2;

			// Token: 0x04000C34 RID: 3124
			public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;

			// Token: 0x04000C35 RID: 3125
			public const int STREAM_SEEK_SET = 0;

			// Token: 0x04000C36 RID: 3126
			public const int STREAM_SEEK_CUR = 1;

			// Token: 0x04000C37 RID: 3127
			public const int STREAM_SEEK_END = 2;
		}

		// Token: 0x0200011A RID: 282
		[Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPicture
		{
			// Token: 0x06000F40 RID: 3904
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetHandle();

			// Token: 0x06000F41 RID: 3905
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetHPal();

			// Token: 0x06000F42 RID: 3906
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.I2)]
			short GetPictureType();

			// Token: 0x06000F43 RID: 3907
			[SuppressUnmanagedCodeSecurity]
			int GetWidth();

			// Token: 0x06000F44 RID: 3908
			[SuppressUnmanagedCodeSecurity]
			int GetHeight();

			// Token: 0x06000F45 RID: 3909
			[SuppressUnmanagedCodeSecurity]
			void Render();

			// Token: 0x06000F46 RID: 3910
			[SuppressUnmanagedCodeSecurity]
			void SetHPal([In] IntPtr phpal);

			// Token: 0x06000F47 RID: 3911
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetCurDC();

			// Token: 0x06000F48 RID: 3912
			[SuppressUnmanagedCodeSecurity]
			void SelectPicture([In] IntPtr hdcIn, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] phdcOut, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] phbmpOut);

			// Token: 0x06000F49 RID: 3913
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetKeepOriginalFormat();

			// Token: 0x06000F4A RID: 3914
			[SuppressUnmanagedCodeSecurity]
			void SetKeepOriginalFormat([MarshalAs(UnmanagedType.Bool)] [In] bool pfkeep);

			// Token: 0x06000F4B RID: 3915
			[SuppressUnmanagedCodeSecurity]
			void PictureChanged();

			// Token: 0x06000F4C RID: 3916
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int SaveAsFile([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [In] int fSaveMemCopy, out int pcbSize);

			// Token: 0x06000F4D RID: 3917
			[SuppressUnmanagedCodeSecurity]
			int GetAttributes();

			// Token: 0x06000F4E RID: 3918
			[SuppressUnmanagedCodeSecurity]
			void SetHdc([In] IntPtr hdc);
		}

		// Token: 0x0200011B RID: 283
		public struct OBJECTHEADER
		{
			// Token: 0x04000C38 RID: 3128
			public short signature;

			// Token: 0x04000C39 RID: 3129
			public short headersize;

			// Token: 0x04000C3A RID: 3130
			public short objectType;

			// Token: 0x04000C3B RID: 3131
			public short nameLen;

			// Token: 0x04000C3C RID: 3132
			public short classLen;

			// Token: 0x04000C3D RID: 3133
			public short nameOffset;

			// Token: 0x04000C3E RID: 3134
			public short classOffset;

			// Token: 0x04000C3F RID: 3135
			public short width;

			// Token: 0x04000C40 RID: 3136
			public short height;

			// Token: 0x04000C41 RID: 3137
			public IntPtr pInfo;
		}

		// Token: 0x0200011C RID: 284
		internal enum Win32SystemColors
		{
			// Token: 0x04000C43 RID: 3139
			ActiveBorder = 10,
			// Token: 0x04000C44 RID: 3140
			ActiveCaption = 2,
			// Token: 0x04000C45 RID: 3141
			ActiveCaptionText = 9,
			// Token: 0x04000C46 RID: 3142
			AppWorkspace = 12,
			// Token: 0x04000C47 RID: 3143
			ButtonFace = 15,
			// Token: 0x04000C48 RID: 3144
			ButtonHighlight = 20,
			// Token: 0x04000C49 RID: 3145
			ButtonShadow = 16,
			// Token: 0x04000C4A RID: 3146
			Control = 15,
			// Token: 0x04000C4B RID: 3147
			ControlDark,
			// Token: 0x04000C4C RID: 3148
			ControlDarkDark = 21,
			// Token: 0x04000C4D RID: 3149
			ControlLight,
			// Token: 0x04000C4E RID: 3150
			ControlLightLight = 20,
			// Token: 0x04000C4F RID: 3151
			ControlText = 18,
			// Token: 0x04000C50 RID: 3152
			Desktop = 1,
			// Token: 0x04000C51 RID: 3153
			GradientActiveCaption = 27,
			// Token: 0x04000C52 RID: 3154
			GradientInactiveCaption,
			// Token: 0x04000C53 RID: 3155
			GrayText = 17,
			// Token: 0x04000C54 RID: 3156
			Highlight = 13,
			// Token: 0x04000C55 RID: 3157
			HighlightText,
			// Token: 0x04000C56 RID: 3158
			HotTrack = 26,
			// Token: 0x04000C57 RID: 3159
			InactiveBorder = 11,
			// Token: 0x04000C58 RID: 3160
			InactiveCaption = 3,
			// Token: 0x04000C59 RID: 3161
			InactiveCaptionText = 19,
			// Token: 0x04000C5A RID: 3162
			Info = 24,
			// Token: 0x04000C5B RID: 3163
			InfoText = 23,
			// Token: 0x04000C5C RID: 3164
			Menu = 4,
			// Token: 0x04000C5D RID: 3165
			MenuBar = 30,
			// Token: 0x04000C5E RID: 3166
			MenuHighlight = 29,
			// Token: 0x04000C5F RID: 3167
			MenuText = 7,
			// Token: 0x04000C60 RID: 3168
			ScrollBar = 0,
			// Token: 0x04000C61 RID: 3169
			Window = 5,
			// Token: 0x04000C62 RID: 3170
			WindowFrame,
			// Token: 0x04000C63 RID: 3171
			WindowText = 8
		}

		// Token: 0x0200011D RID: 285
		public enum BackgroundMode
		{
			// Token: 0x04000C65 RID: 3173
			TRANSPARENT = 1,
			// Token: 0x04000C66 RID: 3174
			OPAQUE
		}
	}
}
