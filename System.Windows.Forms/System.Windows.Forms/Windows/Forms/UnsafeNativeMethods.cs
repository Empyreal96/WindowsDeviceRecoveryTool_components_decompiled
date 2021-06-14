using System;
using System.Drawing;
using System.Internal;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Automation;
using Accessibility;

namespace System.Windows.Forms
{
	// Token: 0x0200041A RID: 1050
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06004775 RID: 18293
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal static extern uint SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, uint cchOutBuf, IntPtr ppvReserved);

		// Token: 0x06004776 RID: 18294
		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(HandleRef pStg, [In] [Out] ref Guid pclsid);

		// Token: 0x06004777 RID: 18295
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern void CoTaskMemFree(IntPtr pv);

		// Token: 0x06004778 RID: 18296
		[DllImport("user32.dll")]
		public static extern int GetClassName(HandleRef hwnd, StringBuilder lpClassName, int nMaxCount);

		// Token: 0x06004779 RID: 18297 RVA: 0x00130EDB File Offset: 0x0012F0DB
		public static IntPtr SetClassLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetClassLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetClassLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x0600477A RID: 18298
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
		public static extern IntPtr SetClassLongPtr32(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x0600477B RID: 18299
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
		public static extern IntPtr SetClassLongPtr64(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x0600477C RID: 18300
		[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IClassFactory2 CoGetClassObject([In] ref Guid clsid, int dwContext, int serverInfo, [In] ref Guid refiid);

		// Token: 0x0600477D RID: 18301
		[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public static extern object CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.Interface)] object punkOuter, int context, [In] ref Guid iid);

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x0600477E RID: 18302 RVA: 0x00130EF8 File Offset: 0x0012F0F8
		internal static bool IsVista
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion != null && osversion.Platform == PlatformID.Win32NT && osversion.Version.CompareTo(UnsafeNativeMethods.VistaOSVersion) >= 0;
			}
		}

		// Token: 0x0600477F RID: 18303
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetLocaleInfo(int Locale, int LCType, StringBuilder lpLCData, int cchData);

		// Token: 0x06004780 RID: 18304
		[DllImport("ole32.dll")]
		public static extern int WriteClassStm(UnsafeNativeMethods.IStream pStream, ref Guid clsid);

		// Token: 0x06004781 RID: 18305
		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(UnsafeNativeMethods.IStorage pStorage, out Guid clsid);

		// Token: 0x06004782 RID: 18306
		[DllImport("ole32.dll")]
		public static extern int ReadClassStm(UnsafeNativeMethods.IStream pStream, out Guid clsid);

		// Token: 0x06004783 RID: 18307
		[DllImport("ole32.dll")]
		public static extern int OleLoadFromStream(UnsafeNativeMethods.IStream pStorage, ref Guid iid, out UnsafeNativeMethods.IOleObject pObject);

		// Token: 0x06004784 RID: 18308
		[DllImport("ole32.dll")]
		public static extern int OleSaveToStream(UnsafeNativeMethods.IPersistStream pPersistStream, UnsafeNativeMethods.IStream pStream);

		// Token: 0x06004785 RID: 18309
		[DllImport("ole32.dll")]
		public static extern int CoGetMalloc(int dwReserved, out UnsafeNativeMethods.IMalloc pMalloc);

		// Token: 0x06004786 RID: 18310
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PageSetupDlg([In] [Out] NativeMethods.PAGESETUPDLG lppsd);

		// Token: 0x06004787 RID: 18311
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, EntryPoint = "PrintDlg", SetLastError = true)]
		public static extern bool PrintDlg_32([In] [Out] NativeMethods.PRINTDLG_32 lppd);

		// Token: 0x06004788 RID: 18312
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, EntryPoint = "PrintDlg", SetLastError = true)]
		public static extern bool PrintDlg_64([In] [Out] NativeMethods.PRINTDLG_64 lppd);

		// Token: 0x06004789 RID: 18313 RVA: 0x00130F34 File Offset: 0x0012F134
		public static bool PrintDlg([In] [Out] NativeMethods.PRINTDLG lppd)
		{
			if (IntPtr.Size == 4)
			{
				NativeMethods.PRINTDLG_32 printdlg_ = lppd as NativeMethods.PRINTDLG_32;
				if (printdlg_ == null)
				{
					throw new NullReferenceException("PRINTDLG data is null");
				}
				return UnsafeNativeMethods.PrintDlg_32(printdlg_);
			}
			else
			{
				NativeMethods.PRINTDLG_64 printdlg_2 = lppd as NativeMethods.PRINTDLG_64;
				if (printdlg_2 == null)
				{
					throw new NullReferenceException("PRINTDLG data is null");
				}
				return UnsafeNativeMethods.PrintDlg_64(printdlg_2);
			}
		}

		// Token: 0x0600478A RID: 18314
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int PrintDlgEx([In] [Out] NativeMethods.PRINTDLGEX lppdex);

		// Token: 0x0600478B RID: 18315
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleGetClipboard(ref IDataObject data);

		// Token: 0x0600478C RID: 18316
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleSetClipboard(IDataObject pDataObj);

		// Token: 0x0600478D RID: 18317
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleFlushClipboard();

		// Token: 0x0600478E RID: 18318
		[DllImport("oleaut32.dll", ExactSpelling = true)]
		public static extern void OleCreatePropertyFrameIndirect(NativeMethods.OCPFIPARAMS p);

		// Token: 0x0600478F RID: 18319
		[DllImport("oleaut32.dll", EntryPoint = "OleCreateFontIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IFont OleCreateIFontIndirect(NativeMethods.FONTDESC fd, ref Guid iid);

		// Token: 0x06004790 RID: 18320
		[DllImport("oleaut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPicture OleCreateIPictureIndirect([MarshalAs(UnmanagedType.AsAny)] object pictdesc, ref Guid iid, bool fOwn);

		// Token: 0x06004791 RID: 18321
		[DllImport("oleaut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPictureDisp OleCreateIPictureDispIndirect([MarshalAs(UnmanagedType.AsAny)] object pictdesc, ref Guid iid, bool fOwn);

		// Token: 0x06004792 RID: 18322
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPicture OleCreatePictureIndirect(NativeMethods.PICTDESC pictdesc, [In] ref Guid refiid, bool fOwn);

		// Token: 0x06004793 RID: 18323
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IFont OleCreateFontIndirect(NativeMethods.tagFONTDESC fontdesc, [In] ref Guid refiid);

		// Token: 0x06004794 RID: 18324
		[DllImport("oleaut32.dll", ExactSpelling = true)]
		public static extern int VarFormat(ref object pvarIn, HandleRef pstrFormat, int iFirstDay, int iFirstWeek, uint dwFlags, [In] [Out] ref IntPtr pbstr);

		// Token: 0x06004795 RID: 18325
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern int DragQueryFile(HandleRef hDrop, int iFile, StringBuilder lpszFile, int cch);

		// Token: 0x06004796 RID: 18326 RVA: 0x00130F80 File Offset: 0x0012F180
		public static int DragQueryFileLongPath(HandleRef hDrop, int iFile, StringBuilder lpszFile)
		{
			if (lpszFile != null && lpszFile.Capacity != 0 && iFile != -1)
			{
				int num;
				if ((num = UnsafeNativeMethods.DragQueryFile(hDrop, iFile, lpszFile, lpszFile.Capacity)) == lpszFile.Capacity)
				{
					int num2 = UnsafeNativeMethods.DragQueryFile(hDrop, iFile, null, 0);
					if (num2 < 32767)
					{
						lpszFile.EnsureCapacity(num2);
						num = UnsafeNativeMethods.DragQueryFile(hDrop, iFile, lpszFile, num2);
					}
					else
					{
						num = 0;
					}
				}
				lpszFile.Length = num;
				return num;
			}
			return UnsafeNativeMethods.DragQueryFile(hDrop, iFile, lpszFile, lpszFile.Capacity);
		}

		// Token: 0x06004797 RID: 18327
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EnumChildWindows(HandleRef hwndParent, NativeMethods.EnumChildrenCallback lpEnumFunc, HandleRef lParam);

		// Token: 0x06004798 RID: 18328
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ShellExecute(HandleRef hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		// Token: 0x06004799 RID: 18329
		[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "ShellExecute")]
		public static extern IntPtr ShellExecute_NoBFM(HandleRef hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		// Token: 0x0600479A RID: 18330
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetScrollPos(HandleRef hWnd, int nBar, int nPos, bool bRedraw);

		// Token: 0x0600479B RID: 18331
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool EnableScrollBar(HandleRef hWnd, int nBar, int value);

		// Token: 0x0600479C RID: 18332
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern int Shell_NotifyIcon(int message, NativeMethods.NOTIFYICONDATA pnid);

		// Token: 0x0600479D RID: 18333
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool InsertMenuItem(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x0600479E RID: 18334
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetMenu(HandleRef hWnd);

		// Token: 0x0600479F RID: 18335
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, [In] [Out] NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x060047A0 RID: 18336
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, [In] [Out] NativeMethods.MENUITEMINFO_T_RW lpmii);

		// Token: 0x060047A1 RID: 18337
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x060047A2 RID: 18338
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateMenu", ExactSpelling = true)]
		private static extern IntPtr IntCreateMenu();

		// Token: 0x060047A3 RID: 18339 RVA: 0x00130FF5 File Offset: 0x0012F1F5
		public static IntPtr CreateMenu()
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateMenu(), NativeMethods.CommonHandles.Menu);
		}

		// Token: 0x060047A4 RID: 18340
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetOpenFileName([In] [Out] NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x060047A5 RID: 18341
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EndDialog(HandleRef hWnd, IntPtr result);

		// Token: 0x060047A6 RID: 18342
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int MultiByteToWideChar(int CodePage, int dwFlags, byte[] lpMultiByteStr, int cchMultiByte, char[] lpWideCharStr, int cchWideChar);

		// Token: 0x060047A7 RID: 18343
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);

		// Token: 0x060047A8 RID: 18344
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "RtlMoveMemory", ExactSpelling = true, SetLastError = true)]
		public static extern void CopyMemory(HandleRef destData, HandleRef srcData, int size);

		// Token: 0x060047A9 RID: 18345
		[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemory(IntPtr pdst, byte[] psrc, int cb);

		// Token: 0x060047AA RID: 18346
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryW(IntPtr pdst, string psrc, int cb);

		// Token: 0x060047AB RID: 18347
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryW(IntPtr pdst, char[] psrc, int cb);

		// Token: 0x060047AC RID: 18348
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryA(IntPtr pdst, string psrc, int cb);

		// Token: 0x060047AD RID: 18349
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryA(IntPtr pdst, char[] psrc, int cb);

		// Token: 0x060047AE RID: 18350
		[DllImport("kernel32.dll", EntryPoint = "DuplicateHandle", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntDuplicateHandle(HandleRef processSource, HandleRef handleSource, HandleRef processTarget, ref IntPtr handleTarget, int desiredAccess, bool inheritHandle, int options);

		// Token: 0x060047AF RID: 18351 RVA: 0x00131008 File Offset: 0x0012F208
		public static IntPtr DuplicateHandle(HandleRef processSource, HandleRef handleSource, HandleRef processTarget, ref IntPtr handleTarget, int desiredAccess, bool inheritHandle, int options)
		{
			IntPtr result = UnsafeNativeMethods.IntDuplicateHandle(processSource, handleSource, processTarget, ref handleTarget, desiredAccess, inheritHandle, options);
			System.Internal.HandleCollector.Add(handleTarget, NativeMethods.CommonHandles.Kernel);
			return result;
		}

		// Token: 0x060047B0 RID: 18352
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgOpenStorageOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, UnsafeNativeMethods.IStorage pStgPriority, int grfMode, int sndExcluded, int reserved);

		// Token: 0x060047B1 RID: 18353
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern IntPtr GetHGlobalFromILockBytes(UnsafeNativeMethods.ILockBytes pLkbyt);

		// Token: 0x060047B2 RID: 18354
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);

		// Token: 0x060047B3 RID: 18355
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetKeyboardState(byte[] keystate);

		// Token: 0x060047B4 RID: 18356
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "keybd_event", ExactSpelling = true)]
		public static extern void Keybd_event(byte vk, byte scan, int flags, int extrainfo);

		// Token: 0x060047B5 RID: 18357
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int SetKeyboardState(byte[] keystate);

		// Token: 0x060047B6 RID: 18358
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UnhookWindowsHookEx(HandleRef hhook);

		// Token: 0x060047B7 RID: 18359
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetAsyncKeyState(int vkey);

		// Token: 0x060047B8 RID: 18360
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);

		// Token: 0x060047B9 RID: 18361
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ScreenToClient(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x060047BA RID: 18362
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

		// Token: 0x060047BB RID: 18363 RVA: 0x00131034 File Offset: 0x0012F234
		public static StringBuilder GetModuleFileNameLongPath(HandleRef hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			int num = 1;
			int moduleFileName;
			while ((moduleFileName = UnsafeNativeMethods.GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity)) == stringBuilder.Capacity && Marshal.GetLastWin32Error() == 122 && stringBuilder.Capacity < 32767)
			{
				num += 2;
				int capacity = (num * 260 < 32767) ? (num * 260) : 32767;
				stringBuilder.EnsureCapacity(capacity);
			}
			stringBuilder.Length = moduleFileName;
			return stringBuilder;
		}

		// Token: 0x060047BC RID: 18364
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool IsDialogMessage(HandleRef hWndDlg, [In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x060047BD RID: 18365
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMessage([In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x060047BE RID: 18366
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DispatchMessage([In] ref NativeMethods.MSG msg);

		// Token: 0x060047BF RID: 18367
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr DispatchMessageA([In] ref NativeMethods.MSG msg);

		// Token: 0x060047C0 RID: 18368
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr DispatchMessageW([In] ref NativeMethods.MSG msg);

		// Token: 0x060047C1 RID: 18369
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostThreadMessage(int id, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x060047C2 RID: 18370
		[DllImport("ole32.dll", ExactSpelling = true)]
		public static extern int CoRegisterMessageFilter(HandleRef newFilter, ref IntPtr oldMsgFilter);

		// Token: 0x060047C3 RID: 18371
		[DllImport("ole32.dll", EntryPoint = "OleInitialize", ExactSpelling = true, SetLastError = true)]
		private static extern int IntOleInitialize(int val);

		// Token: 0x060047C4 RID: 18372 RVA: 0x001310B0 File Offset: 0x0012F2B0
		public static int OleInitialize()
		{
			return UnsafeNativeMethods.IntOleInitialize(0);
		}

		// Token: 0x060047C5 RID: 18373
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnumThreadWindows(int dwThreadId, NativeMethods.EnumThreadWindowsCallback lpfn, HandleRef lParam);

		// Token: 0x060047C6 RID: 18374
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

		// Token: 0x060047C7 RID: 18375
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendDlgItemMessage(HandleRef hDlg, int nIDDlgItem, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x060047C8 RID: 18376
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int OleUninitialize();

		// Token: 0x060047C9 RID: 18377
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetSaveFileName([In] [Out] NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x060047CA RID: 18378
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ChildWindowFromPointEx", ExactSpelling = true)]
		private static extern IntPtr _ChildWindowFromPointEx(HandleRef hwndParent, UnsafeNativeMethods.POINTSTRUCT pt, int uFlags);

		// Token: 0x060047CB RID: 18379 RVA: 0x001310B8 File Offset: 0x0012F2B8
		public static IntPtr ChildWindowFromPointEx(HandleRef hwndParent, int x, int y, int uFlags)
		{
			UnsafeNativeMethods.POINTSTRUCT pt = new UnsafeNativeMethods.POINTSTRUCT(x, y);
			return UnsafeNativeMethods._ChildWindowFromPointEx(hwndParent, pt, uFlags);
		}

		// Token: 0x060047CC RID: 18380
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "CloseHandle", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntCloseHandle(HandleRef handle);

		// Token: 0x060047CD RID: 18381 RVA: 0x001310D6 File Offset: 0x0012F2D6
		public static bool CloseHandle(HandleRef handle)
		{
			System.Internal.HandleCollector.Remove((IntPtr)handle, NativeMethods.CommonHandles.Kernel);
			return UnsafeNativeMethods.IntCloseHandle(handle);
		}

		// Token: 0x060047CE RID: 18382
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x060047CF RID: 18383 RVA: 0x001310EF File Offset: 0x0012F2EF
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateCompatibleDC(hDC), NativeMethods.CommonHandles.CompatibleHDC);
		}

		// Token: 0x060047D0 RID: 18384
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] [In] bool fBlockIt);

		// Token: 0x060047D1 RID: 18385
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern uint SendInput(uint nInputs, NativeMethods.INPUT[] pInputs, int cbSize);

		// Token: 0x060047D2 RID: 18386
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "MapViewOfFile", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntMapViewOfFile(HandleRef hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap);

		// Token: 0x060047D3 RID: 18387 RVA: 0x00131101 File Offset: 0x0012F301
		public static IntPtr MapViewOfFile(HandleRef hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntMapViewOfFile(hFileMapping, dwDesiredAccess, dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap), NativeMethods.CommonHandles.Kernel);
		}

		// Token: 0x060047D4 RID: 18388
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "UnmapViewOfFile", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntUnmapViewOfFile(HandleRef pvBaseAddress);

		// Token: 0x060047D5 RID: 18389 RVA: 0x00131118 File Offset: 0x0012F318
		public static bool UnmapViewOfFile(HandleRef pvBaseAddress)
		{
			System.Internal.HandleCollector.Remove((IntPtr)pvBaseAddress, NativeMethods.CommonHandles.Kernel);
			return UnsafeNativeMethods.IntUnmapViewOfFile(pvBaseAddress);
		}

		// Token: 0x060047D6 RID: 18390
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDCEx", ExactSpelling = true)]
		private static extern IntPtr IntGetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags);

		// Token: 0x060047D7 RID: 18391 RVA: 0x00131131 File Offset: 0x0012F331
		public static IntPtr GetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDCEx(hWnd, hrgnClip, flags), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x060047D8 RID: 18392
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.BITMAP bm);

		// Token: 0x060047D9 RID: 18393
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGPEN lp);

		// Token: 0x060047DA RID: 18394 RVA: 0x00131145 File Offset: 0x0012F345
		public static int GetObject(HandleRef hObject, NativeMethods.LOGPEN lp)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGPEN)), lp);
		}

		// Token: 0x060047DB RID: 18395
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGBRUSH lb);

		// Token: 0x060047DC RID: 18396 RVA: 0x0013115D File Offset: 0x0012F35D
		public static int GetObject(HandleRef hObject, NativeMethods.LOGBRUSH lb)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGBRUSH)), lb);
		}

		// Token: 0x060047DD RID: 18397
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGFONT lf);

		// Token: 0x060047DE RID: 18398 RVA: 0x00131175 File Offset: 0x0012F375
		public static int GetObject(HandleRef hObject, NativeMethods.LOGFONT lp)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGFONT)), lp);
		}

		// Token: 0x060047DF RID: 18399
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, ref int nEntries);

		// Token: 0x060047E0 RID: 18400
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, int[] nEntries);

		// Token: 0x060047E1 RID: 18401
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetObjectType(HandleRef hObject);

		// Token: 0x060047E2 RID: 18402
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateAcceleratorTable")]
		private static extern IntPtr IntCreateAcceleratorTable(HandleRef pentries, int cCount);

		// Token: 0x060047E3 RID: 18403 RVA: 0x0013118D File Offset: 0x0012F38D
		public static IntPtr CreateAcceleratorTable(HandleRef pentries, int cCount)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateAcceleratorTable(pentries, cCount), NativeMethods.CommonHandles.Accelerator);
		}

		// Token: 0x060047E4 RID: 18404
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyAcceleratorTable", ExactSpelling = true)]
		private static extern bool IntDestroyAcceleratorTable(HandleRef hAccel);

		// Token: 0x060047E5 RID: 18405 RVA: 0x001311A0 File Offset: 0x0012F3A0
		public static bool DestroyAcceleratorTable(HandleRef hAccel)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hAccel, NativeMethods.CommonHandles.Accelerator);
			return UnsafeNativeMethods.IntDestroyAcceleratorTable(hAccel);
		}

		// Token: 0x060047E6 RID: 18406
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern short VkKeyScan(char key);

		// Token: 0x060047E7 RID: 18407
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCapture();

		// Token: 0x060047E8 RID: 18408
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetCapture(HandleRef hwnd);

		// Token: 0x060047E9 RID: 18409
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		// Token: 0x060047EA RID: 18410
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetCursorPos([In] [Out] NativeMethods.POINT pt);

		// Token: 0x060047EB RID: 18411
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetKeyState(int keyCode);

		// Token: 0x060047EC RID: 18412
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, uint cchBuffer);

		// Token: 0x060047ED RID: 18413
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowRgn", ExactSpelling = true)]
		private static extern int IntSetWindowRgn(HandleRef hwnd, HandleRef hrgn, bool fRedraw);

		// Token: 0x060047EE RID: 18414 RVA: 0x001311BC File Offset: 0x0012F3BC
		public static int SetWindowRgn(HandleRef hwnd, HandleRef hrgn, bool fRedraw)
		{
			int num = UnsafeNativeMethods.IntSetWindowRgn(hwnd, hrgn, fRedraw);
			if (num != 0)
			{
				System.Internal.HandleCollector.Remove((IntPtr)hrgn, NativeMethods.CommonHandles.GDI);
			}
			return num;
		}

		// Token: 0x060047EF RID: 18415
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x060047F0 RID: 18416
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern void GetTempFileName(string tempDirName, string prefixName, int unique, StringBuilder sb);

		// Token: 0x060047F1 RID: 18417
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetWindowText(HandleRef hWnd, string text);

		// Token: 0x060047F2 RID: 18418
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalAlloc(int uFlags, int dwBytes);

		// Token: 0x060047F3 RID: 18419
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalReAlloc(HandleRef handle, int bytes, int flags);

		// Token: 0x060047F4 RID: 18420
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalLock(HandleRef handle);

		// Token: 0x060047F5 RID: 18421
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GlobalUnlock(HandleRef handle);

		// Token: 0x060047F6 RID: 18422
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalFree(HandleRef handle);

		// Token: 0x060047F7 RID: 18423
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GlobalSize(HandleRef handle);

		// Token: 0x060047F8 RID: 18424
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmSetConversionStatus(HandleRef hIMC, int conversion, int sentence);

		// Token: 0x060047F9 RID: 18425
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmGetConversionStatus(HandleRef hIMC, ref int conversion, ref int sentence);

		// Token: 0x060047FA RID: 18426
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmGetContext(HandleRef hWnd);

		// Token: 0x060047FB RID: 18427
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmReleaseContext(HandleRef hWnd, HandleRef hIMC);

		// Token: 0x060047FC RID: 18428
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmAssociateContext(HandleRef hWnd, HandleRef hIMC);

		// Token: 0x060047FD RID: 18429
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmDestroyContext(HandleRef hIMC);

		// Token: 0x060047FE RID: 18430
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmCreateContext();

		// Token: 0x060047FF RID: 18431
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmSetOpenStatus(HandleRef hIMC, bool open);

		// Token: 0x06004800 RID: 18432
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmGetOpenStatus(HandleRef hIMC);

		// Token: 0x06004801 RID: 18433
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmNotifyIME(HandleRef hIMC, int dwAction, int dwIndex, int dwValue);

		// Token: 0x06004802 RID: 18434
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(HandleRef hWnd);

		// Token: 0x06004803 RID: 18435
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetParent(HandleRef hWnd);

		// Token: 0x06004804 RID: 18436
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetAncestor(HandleRef hWnd, int flags);

		// Token: 0x06004805 RID: 18437
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

		// Token: 0x06004806 RID: 18438
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsZoomed(HandleRef hWnd);

		// Token: 0x06004807 RID: 18439
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string className, string windowName);

		// Token: 0x06004808 RID: 18440
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In] [Out] ref NativeMethods.RECT rect, int cPoints);

		// Token: 0x06004809 RID: 18441
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In] [Out] NativeMethods.POINT pt, int cPoints);

		// Token: 0x0600480A RID: 18442
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, bool wParam, int lParam);

		// Token: 0x0600480B RID: 18443
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int[] lParam);

		// Token: 0x0600480C RID: 18444
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int[] wParam, int[] lParam);

		// Token: 0x0600480D RID: 18445
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, ref int wParam, ref int lParam);

		// Token: 0x0600480E RID: 18446
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

		// Token: 0x0600480F RID: 18447
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, string lParam);

		// Token: 0x06004810 RID: 18448
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, StringBuilder lParam);

		// Token: 0x06004811 RID: 18449
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TOOLINFO_T lParam);

		// Token: 0x06004812 RID: 18450
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TOOLINFO_TOOLTIP lParam);

		// Token: 0x06004813 RID: 18451
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TBBUTTON lParam);

		// Token: 0x06004814 RID: 18452
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TBBUTTONINFO lParam);

		// Token: 0x06004815 RID: 18453
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TV_ITEM lParam);

		// Token: 0x06004816 RID: 18454
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TV_INSERTSTRUCT lParam);

		// Token: 0x06004817 RID: 18455
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TV_HITTESTINFO lParam);

		// Token: 0x06004818 RID: 18456
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVBKIMAGE lParam);

		// Token: 0x06004819 RID: 18457
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.LVHITTESTINFO lParam);

		// Token: 0x0600481A RID: 18458
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TCITEM_T lParam);

		// Token: 0x0600481B RID: 18459
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.HDLAYOUT hdlayout);

		// Token: 0x0600481C RID: 18460
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, HandleRef wParam, int lParam);

		// Token: 0x0600481D RID: 18461
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, HandleRef lParam);

		// Token: 0x0600481E RID: 18462
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.PARAFORMAT lParam);

		// Token: 0x0600481F RID: 18463
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMATA lParam);

		// Token: 0x06004820 RID: 18464
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMAT2A lParam);

		// Token: 0x06004821 RID: 18465
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMATW lParam);

		// Token: 0x06004822 RID: 18466
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.IUnknown)] out object editOle);

		// Token: 0x06004823 RID: 18467
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.CHARRANGE lParam);

		// Token: 0x06004824 RID: 18468
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.FINDTEXT lParam);

		// Token: 0x06004825 RID: 18469
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TEXTRANGE lParam);

		// Token: 0x06004826 RID: 18470
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.POINT lParam);

		// Token: 0x06004827 RID: 18471
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.POINT wParam, int lParam);

		// Token: 0x06004828 RID: 18472
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.REPASTESPECIAL lParam);

		// Token: 0x06004829 RID: 18473
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.EDITSTREAM lParam);

		// Token: 0x0600482A RID: 18474
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.EDITSTREAM64 lParam);

		// Token: 0x0600482B RID: 18475
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.GETTEXTLENGTHEX wParam, int lParam);

		// Token: 0x0600482C RID: 18476
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.SIZE lParam);

		// Token: 0x0600482D RID: 18477
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] ref NativeMethods.LVFINDINFO lParam);

		// Token: 0x0600482E RID: 18478
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVHITTESTINFO lParam);

		// Token: 0x0600482F RID: 18479
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVCOLUMN_T lParam);

		// Token: 0x06004830 RID: 18480
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] ref NativeMethods.LVITEM lParam);

		// Token: 0x06004831 RID: 18481
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVCOLUMN lParam);

		// Token: 0x06004832 RID: 18482
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVGROUP lParam);

		// Token: 0x06004833 RID: 18483
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.POINT wParam, [In] [Out] NativeMethods.LVINSERTMARK lParam);

		// Token: 0x06004834 RID: 18484
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVINSERTMARK lParam);

		// Token: 0x06004835 RID: 18485
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.LVTILEVIEWINFO lParam);

		// Token: 0x06004836 RID: 18486
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.MCHITTESTINFO lParam);

		// Token: 0x06004837 RID: 18487
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.SYSTEMTIME lParam);

		// Token: 0x06004838 RID: 18488
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.SYSTEMTIMEARRAY lParam);

		// Token: 0x06004839 RID: 18489
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.LOGFONT lParam);

		// Token: 0x0600483A RID: 18490
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.MSG lParam);

		// Token: 0x0600483B RID: 18491
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

		// Token: 0x0600483C RID: 18492
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600483D RID: 18493
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, [In] [Out] ref NativeMethods.RECT lParam);

		// Token: 0x0600483E RID: 18494
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, ref short wParam, ref short lParam);

		// Token: 0x0600483F RID: 18495
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, [MarshalAs(UnmanagedType.Bool)] [In] [Out] ref bool wParam, IntPtr lParam);

		// Token: 0x06004840 RID: 18496
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, IntPtr lParam);

		// Token: 0x06004841 RID: 18497
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, [In] [Out] ref NativeMethods.RECT lParam);

		// Token: 0x06004842 RID: 18498
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, [In] [Out] ref Rectangle lParam);

		// Token: 0x06004843 RID: 18499
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, NativeMethods.ListViewCompareCallback pfnCompare);

		// Token: 0x06004844 RID: 18500
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);

		// Token: 0x06004845 RID: 18501
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);

		// Token: 0x06004846 RID: 18502
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetWindowRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x06004847 RID: 18503
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

		// Token: 0x06004848 RID: 18504
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDlgItem(HandleRef hWnd, int nIDDlgItem);

		// Token: 0x06004849 RID: 18505
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string modName);

		// Token: 0x0600484A RID: 18506
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600484B RID: 18507
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefMDIChildProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600484C RID: 18508
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600484D RID: 18509
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GlobalDeleteAtom(short atom);

		// Token: 0x0600484E RID: 18510
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr GetProcAddress(HandleRef hModule, string lpProcName);

		// Token: 0x0600484F RID: 18511
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetClassInfo(HandleRef hInst, string lpszClass, [In] [Out] NativeMethods.WNDCLASS_I wc);

		// Token: 0x06004850 RID: 18512
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetClassInfo(HandleRef hInst, string lpszClass, IntPtr h);

		// Token: 0x06004851 RID: 18513
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetSystemMetrics(int nIndex);

		// Token: 0x06004852 RID: 18514
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetSystemMetricsForDpi(int nIndex, uint dpi);

		// Token: 0x06004853 RID: 18515 RVA: 0x001311E7 File Offset: 0x0012F3E7
		public static int TryGetSystemMetricsForDpi(int nIndex, uint dpi)
		{
			if (ApiHelper.IsApiAvailable("user32.dll", "GetSystemMetricsForDpi"))
			{
				return UnsafeNativeMethods.GetSystemMetricsForDpi(nIndex, dpi);
			}
			return UnsafeNativeMethods.GetSystemMetrics(nIndex);
		}

		// Token: 0x06004854 RID: 18516
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.RECT rc, int nUpdate);

		// Token: 0x06004855 RID: 18517
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref int value, int ignore);

		// Token: 0x06004856 RID: 18518
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref bool value, int ignore);

		// Token: 0x06004857 RID: 18519
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.HIGHCONTRAST_I rc, int nUpdate);

		// Token: 0x06004858 RID: 18520
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] NativeMethods.NONCLIENTMETRICS metrics, int nUpdate);

		// Token: 0x06004859 RID: 18521
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SystemParametersInfoForDpi(int nAction, int nParam, [In] [Out] NativeMethods.NONCLIENTMETRICS metrics, int nUpdate, uint dpi);

		// Token: 0x0600485A RID: 18522 RVA: 0x00131208 File Offset: 0x0012F408
		public static bool TrySystemParametersInfoForDpi(int nAction, int nParam, [In] [Out] NativeMethods.NONCLIENTMETRICS metrics, int nUpdate, uint dpi)
		{
			if (ApiHelper.IsApiAvailable("user32.dll", "SystemParametersInfoForDpi"))
			{
				return UnsafeNativeMethods.SystemParametersInfoForDpi(nAction, nParam, metrics, nUpdate, dpi);
			}
			return UnsafeNativeMethods.SystemParametersInfo(nAction, nParam, metrics, nUpdate);
		}

		// Token: 0x0600485B RID: 18523
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] NativeMethods.LOGFONT font, int nUpdate);

		// Token: 0x0600485C RID: 18524
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, bool[] flag, bool nUpdate);

		// Token: 0x0600485D RID: 18525
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);

		// Token: 0x0600485E RID: 18526
		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetUserName(StringBuilder lpBuffer, int[] nSize);

		// Token: 0x0600485F RID: 18527
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr GetProcessWindowStation();

		// Token: 0x06004860 RID: 18528
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetUserObjectInformation(HandleRef hObj, int nIndex, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.USEROBJECTFLAGS pvBuffer, int nLength, ref int lpnLengthNeeded);

		// Token: 0x06004861 RID: 18529
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x06004862 RID: 18530
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06004863 RID: 18531
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		// Token: 0x06004864 RID: 18532
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06004865 RID: 18533
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RegisterDragDrop(HandleRef hwnd, UnsafeNativeMethods.IOleDropTarget target);

		// Token: 0x06004866 RID: 18534
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RevokeDragDrop(HandleRef hwnd);

		// Token: 0x06004867 RID: 18535
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x06004868 RID: 18536
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool PeekMessageW([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x06004869 RID: 18537
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		public static extern bool PeekMessageA([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x0600486A RID: 18538
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600486B RID: 18539
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short GlobalAddAtom(string atomName);

		// Token: 0x0600486C RID: 18540
		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr LresultFromObject(ref Guid refiid, IntPtr wParam, HandleRef pAcc);

		// Token: 0x0600486D RID: 18541
		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int CreateStdAccessibleObject(HandleRef hWnd, int objID, ref Guid refiid, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref object pAcc);

		// Token: 0x0600486E RID: 18542
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

		// Token: 0x0600486F RID: 18543
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMenuItemID(HandleRef hMenu, int nPos);

		// Token: 0x06004870 RID: 18544
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSubMenu(HandleRef hwnd, int index);

		// Token: 0x06004871 RID: 18545
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMenuItemCount(HandleRef hMenu);

		// Token: 0x06004872 RID: 18546
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void GetErrorInfo(int reserved, [In] [Out] ref UnsafeNativeMethods.IErrorInfo errorInfo);

		// Token: 0x06004873 RID: 18547
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "BeginPaint", ExactSpelling = true)]
		private static extern IntPtr IntBeginPaint(HandleRef hWnd, [In] [Out] ref NativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x06004874 RID: 18548 RVA: 0x00131230 File Offset: 0x0012F430
		public static IntPtr BeginPaint(HandleRef hWnd, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] ref NativeMethods.PAINTSTRUCT lpPaint)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntBeginPaint(hWnd, ref lpPaint), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06004875 RID: 18549
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "EndPaint", ExactSpelling = true)]
		private static extern bool IntEndPaint(HandleRef hWnd, ref NativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x06004876 RID: 18550 RVA: 0x00131243 File Offset: 0x0012F443
		public static bool EndPaint(HandleRef hWnd, [MarshalAs(UnmanagedType.LPStruct)] [In] ref NativeMethods.PAINTSTRUCT lpPaint)
		{
			System.Internal.HandleCollector.Remove(lpPaint.hdc, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntEndPaint(hWnd, ref lpPaint);
		}

		// Token: 0x06004877 RID: 18551
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
		private static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x06004878 RID: 18552 RVA: 0x0013125D File Offset: 0x0012F45D
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06004879 RID: 18553
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowDC", ExactSpelling = true)]
		private static extern IntPtr IntGetWindowDC(HandleRef hWnd);

		// Token: 0x0600487A RID: 18554 RVA: 0x0013126F File Offset: 0x0012F46F
		public static IntPtr GetWindowDC(HandleRef hWnd)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetWindowDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600487B RID: 18555
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x0600487C RID: 18556 RVA: 0x00131281 File Offset: 0x0012F481
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x0600487D RID: 18557
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDC", SetLastError = true)]
		private static extern IntPtr IntCreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, HandleRef devMode);

		// Token: 0x0600487E RID: 18558 RVA: 0x0013129B File Offset: 0x0012F49B
		public static IntPtr CreateDC(string lpszDriver)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateDC(lpszDriver, null, null, NativeMethods.NullHandleRef), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x001312B4 File Offset: 0x0012F4B4
		public static IntPtr CreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateDC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06004880 RID: 18560
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] IntPtr[] rc, int nUpdate);

		// Token: 0x06004881 RID: 18561
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
		public static extern IntPtr SendCallbackMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06004882 RID: 18562
		[DllImport("shell32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern void DragAcceptFiles(HandleRef hWnd, bool fAccept);

		// Token: 0x06004883 RID: 18563
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x06004884 RID: 18564
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, NativeMethods.SCROLLINFO si);

		// Token: 0x06004885 RID: 18565
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int SetScrollInfo(HandleRef hWnd, int fnBar, NativeMethods.SCROLLINFO si, bool redraw);

		// Token: 0x06004886 RID: 18566
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();

		// Token: 0x06004887 RID: 18567
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string libname);

		// Token: 0x06004888 RID: 18568
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibraryEx(string lpModuleName, IntPtr hFile, uint dwFlags);

		// Token: 0x06004889 RID: 18569
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool FreeLibrary(HandleRef hModule);

		// Token: 0x0600488A RID: 18570 RVA: 0x001312C9 File Offset: 0x0012F4C9
		public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.GetWindowLong32(hWnd, nIndex);
			}
			return UnsafeNativeMethods.GetWindowLongPtr64(hWnd, nIndex);
		}

		// Token: 0x0600488B RID: 18571
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		public static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);

		// Token: 0x0600488C RID: 18572
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
		public static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

		// Token: 0x0600488D RID: 18573 RVA: 0x001312E2 File Offset: 0x0012F4E2
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x0600488E RID: 18574
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x0600488F RID: 18575
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x06004890 RID: 18576 RVA: 0x001312FD File Offset: 0x0012F4FD
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, wndproc);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, wndproc);
		}

		// Token: 0x06004891 RID: 18577
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc);

		// Token: 0x06004892 RID: 18578
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc);

		// Token: 0x06004893 RID: 18579
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.ILockBytes CreateILockBytesOnHGlobal(HandleRef hGlobal, bool fDeleteOnRelease);

		// Token: 0x06004894 RID: 18580
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgCreateDocfileOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, int grfMode, int reserved);

		// Token: 0x06004895 RID: 18581
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePopupMenu", ExactSpelling = true)]
		private static extern IntPtr IntCreatePopupMenu();

		// Token: 0x06004896 RID: 18582 RVA: 0x00131318 File Offset: 0x0012F518
		public static IntPtr CreatePopupMenu()
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreatePopupMenu(), NativeMethods.CommonHandles.Menu);
		}

		// Token: 0x06004897 RID: 18583
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RemoveMenu(HandleRef hMenu, int uPosition, int uFlags);

		// Token: 0x06004898 RID: 18584
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyMenu", ExactSpelling = true)]
		private static extern bool IntDestroyMenu(HandleRef hMenu);

		// Token: 0x06004899 RID: 18585 RVA: 0x00131329 File Offset: 0x0012F529
		public static bool DestroyMenu(HandleRef hMenu)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hMenu, NativeMethods.CommonHandles.Menu);
			return UnsafeNativeMethods.IntDestroyMenu(hMenu);
		}

		// Token: 0x0600489A RID: 18586
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetForegroundWindow(HandleRef hWnd);

		// Token: 0x0600489B RID: 18587
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

		// Token: 0x0600489C RID: 18588
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefFrameProc(IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600489D RID: 18589
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMDISysAccel(IntPtr hWndClient, [In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x0600489E RID: 18590
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetLayeredWindowAttributes(HandleRef hwnd, int crKey, byte bAlpha, int dwFlags);

		// Token: 0x0600489F RID: 18591
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetMenu(HandleRef hWnd, HandleRef hMenu);

		// Token: 0x060048A0 RID: 18592
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowPlacement(HandleRef hWnd, ref NativeMethods.WINDOWPLACEMENT placement);

		// Token: 0x060048A1 RID: 18593
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern void GetStartupInfo([In] [Out] NativeMethods.STARTUPINFO_I startupinfo_i);

		// Token: 0x060048A2 RID: 18594
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetMenuDefaultItem(HandleRef hwnd, int nIndex, bool pos);

		// Token: 0x060048A3 RID: 18595
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnableMenuItem(HandleRef hMenu, int UIDEnabledItem, int uEnable);

		// Token: 0x060048A4 RID: 18596
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(HandleRef hWnd);

		// Token: 0x060048A5 RID: 18597
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateIC", SetLastError = true)]
		private static extern IntPtr IntCreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x060048A6 RID: 18598 RVA: 0x00131342 File Offset: 0x0012F542
		public static IntPtr CreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateIC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x060048A7 RID: 18599
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(ref NativeMethods.RECT rcClip);

		// Token: 0x060048A8 RID: 18600
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(NativeMethods.COMRECT rcClip);

		// Token: 0x060048A9 RID: 18601
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetCursor(HandleRef hcursor);

		// Token: 0x060048AA RID: 18602
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetCursorPos(int x, int y);

		// Token: 0x060048AB RID: 18603
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ShowCursor(bool bShow);

		// Token: 0x060048AC RID: 18604
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyCursor", ExactSpelling = true)]
		private static extern bool IntDestroyCursor(HandleRef hCurs);

		// Token: 0x060048AD RID: 18605 RVA: 0x00131357 File Offset: 0x0012F557
		public static bool DestroyCursor(HandleRef hCurs)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hCurs, NativeMethods.CommonHandles.Cursor);
			return UnsafeNativeMethods.IntDestroyCursor(hCurs);
		}

		// Token: 0x060048AE RID: 18606
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindow(HandleRef hWnd);

		// Token: 0x060048AF RID: 18607
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x060048B0 RID: 18608 RVA: 0x00131370 File Offset: 0x0012F570
		public static bool DeleteDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x00131389 File Offset: 0x0012F589
		public static bool DeleteCompatibleDC(HandleRef hDC)
		{
			System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.CompatibleHDC);
			return UnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x060048B2 RID: 18610
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern bool GetMessageA([In] [Out] ref NativeMethods.MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax);

		// Token: 0x060048B3 RID: 18611
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern bool GetMessageW([In] [Out] ref NativeMethods.MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax);

		// Token: 0x060048B4 RID: 18612
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);

		// Token: 0x060048B5 RID: 18613
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, IntPtr lparam);

		// Token: 0x060048B6 RID: 18614
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x060048B7 RID: 18615
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, IntPtr rect);

		// Token: 0x060048B8 RID: 18616
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "WindowFromPoint", ExactSpelling = true)]
		private static extern IntPtr _WindowFromPoint(UnsafeNativeMethods.POINTSTRUCT pt);

		// Token: 0x060048B9 RID: 18617 RVA: 0x001313A4 File Offset: 0x0012F5A4
		public static IntPtr WindowFromPoint(int x, int y)
		{
			UnsafeNativeMethods.POINTSTRUCT pt = new UnsafeNativeMethods.POINTSTRUCT(x, y);
			return UnsafeNativeMethods._WindowFromPoint(pt);
		}

		// Token: 0x060048BA RID: 18618
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr WindowFromDC(HandleRef hDC);

		// Token: 0x060048BB RID: 18619
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateWindowEx", SetLastError = true)]
		public static extern IntPtr IntCreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);

		// Token: 0x060048BC RID: 18620 RVA: 0x001313C0 File Offset: 0x0012F5C0
		public static IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam)
		{
			return UnsafeNativeMethods.IntCreateWindowEx(dwExStyle, lpszClassName, lpszWindowName, style, x, y, width, height, hWndParent, hMenu, hInst, pvParam);
		}

		// Token: 0x060048BD RID: 18621
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyWindow", ExactSpelling = true)]
		public static extern bool IntDestroyWindow(HandleRef hWnd);

		// Token: 0x060048BE RID: 18622 RVA: 0x001313E6 File Offset: 0x0012F5E6
		public static bool DestroyWindow(HandleRef hWnd)
		{
			return UnsafeNativeMethods.IntDestroyWindow(hWnd);
		}

		// Token: 0x060048BF RID: 18623
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool UnregisterClass(string className, HandleRef hInstance);

		// Token: 0x060048C0 RID: 18624
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetStockObject(int nIndex);

		// Token: 0x060048C1 RID: 18625
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short RegisterClass(NativeMethods.WNDCLASS_D wc);

		// Token: 0x060048C2 RID: 18626
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void PostQuitMessage(int nExitCode);

		// Token: 0x060048C3 RID: 18627
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void WaitMessage();

		// Token: 0x060048C4 RID: 18628
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPlacement(HandleRef hWnd, [In] ref NativeMethods.WINDOWPLACEMENT placement);

		// Token: 0x060048C5 RID: 18629
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern uint GetDpiForWindow(HandleRef hWnd);

		// Token: 0x060048C6 RID: 18630
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetSystemPowerStatus([In] [Out] ref NativeMethods.SYSTEM_POWER_STATUS systemPowerStatus);

		// Token: 0x060048C7 RID: 18631
		[DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

		// Token: 0x060048C8 RID: 18632
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetRegionData(HandleRef hRgn, int size, IntPtr lpRgnData);

		// Token: 0x060048C9 RID: 18633 RVA: 0x001313F0 File Offset: 0x0012F5F0
		public unsafe static NativeMethods.RECT[] GetRectsFromRegion(IntPtr hRgn)
		{
			NativeMethods.RECT[] array = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int regionData = UnsafeNativeMethods.GetRegionData(new HandleRef(null, hRgn), 0, IntPtr.Zero);
				if (regionData != 0)
				{
					intPtr = Marshal.AllocCoTaskMem(regionData);
					int regionData2 = UnsafeNativeMethods.GetRegionData(new HandleRef(null, hRgn), regionData, intPtr);
					if (regionData2 == regionData)
					{
						NativeMethods.RGNDATAHEADER* ptr = (NativeMethods.RGNDATAHEADER*)((void*)intPtr);
						if (ptr->iType == 1)
						{
							array = new NativeMethods.RECT[ptr->nCount];
							int cbSizeOfStruct = ptr->cbSizeOfStruct;
							for (int i = 0; i < ptr->nCount; i++)
							{
								array[i] = *(NativeMethods.RECT*)((byte*)((byte*)((void*)intPtr) + cbSizeOfStruct) + Marshal.SizeOf(typeof(NativeMethods.RECT)) * i);
							}
						}
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return array;
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x001314C8 File Offset: 0x0012F6C8
		internal static bool IsComObject(object o)
		{
			return Marshal.IsComObject(o);
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x001314D0 File Offset: 0x0012F6D0
		internal static int ReleaseComObject(object objToRelease)
		{
			return Marshal.ReleaseComObject(objToRelease);
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x001314D8 File Offset: 0x0012F6D8
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static object PtrToStructure(IntPtr lparam, Type cls)
		{
			return Marshal.PtrToStructure(lparam, cls);
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x001314E1 File Offset: 0x0012F6E1
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static void PtrToStructure(IntPtr lparam, object data)
		{
			Marshal.PtrToStructure(lparam, data);
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x001314EA File Offset: 0x0012F6EA
		internal static int SizeOf(Type t)
		{
			return Marshal.SizeOf(t);
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x001314F2 File Offset: 0x0012F6F2
		internal static void ThrowExceptionForHR(int errorCode)
		{
			Marshal.ThrowExceptionForHR(errorCode);
		}

		// Token: 0x060048D0 RID: 18640
		[DllImport("clr.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		internal static extern void CorLaunchApplication(uint hostType, string applicationFullName, int manifestPathsCount, string[] manifestPaths, int activationDataCount, string[] activationData, UnsafeNativeMethods.PROCESS_INFORMATION processInformation);

		// Token: 0x060048D1 RID: 18641
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode)]
		internal static extern int UiaHostProviderFromHwnd(HandleRef hwnd, out UnsafeNativeMethods.IRawElementProviderSimple provider);

		// Token: 0x060048D2 RID: 18642
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr UiaReturnRawElementProvider(HandleRef hwnd, IntPtr wParam, IntPtr lParam, UnsafeNativeMethods.IRawElementProviderSimple el);

		// Token: 0x060048D3 RID: 18643
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode)]
		internal static extern bool UiaClientsAreListening();

		// Token: 0x060048D4 RID: 18644
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int UiaRaiseAutomationEvent(UnsafeNativeMethods.IRawElementProviderSimple provider, int id);

		// Token: 0x060048D5 RID: 18645
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int UiaRaiseAutomationPropertyChangedEvent(UnsafeNativeMethods.IRawElementProviderSimple provider, int id, object oldValue, object newValue);

		// Token: 0x060048D6 RID: 18646
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int UiaRaiseNotificationEvent(UnsafeNativeMethods.IRawElementProviderSimple provider, AutomationNotificationKind notificationKind, AutomationNotificationProcessing notificationProcessing, string notificationText, string activityId);

		// Token: 0x060048D7 RID: 18647
		[DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode)]
		internal static extern int UiaRaiseStructureChangedEvent(UnsafeNativeMethods.IRawElementProviderSimple provider, UnsafeNativeMethods.StructureChangeType structureChangeType, int[] runtimeId, int runtimeIdLen);

		// Token: 0x060048D8 RID: 18648 RVA: 0x001314FC File Offset: 0x0012F6FC
		public static IntPtr LoadLibraryFromSystemPathIfAvailable(string libraryName)
		{
			IntPtr result = IntPtr.Zero;
			IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle("kernel32.dll");
			if (moduleHandle != IntPtr.Zero)
			{
				if (UnsafeNativeMethods.GetProcAddress(new HandleRef(null, moduleHandle), "AddDllDirectory") != IntPtr.Zero)
				{
					result = UnsafeNativeMethods.LoadLibraryEx(libraryName, IntPtr.Zero, 2048U);
				}
				else
				{
					result = UnsafeNativeMethods.LoadLibrary(libraryName);
				}
			}
			return result;
		}

		// Token: 0x060048D9 RID: 18649
		[DllImport("wldp.dll", ExactSpelling = true)]
		private static extern int WldpIsDynamicCodePolicyEnabled(out int enabled);

		// Token: 0x060048DA RID: 18650 RVA: 0x00131560 File Offset: 0x0012F760
		internal static bool IsDynamicCodePolicyEnabled()
		{
			if (!ApiHelper.IsApiAvailable("wldp.dll", "WldpIsDynamicCodePolicyEnabled"))
			{
				return false;
			}
			int num = 0;
			return UnsafeNativeMethods.WldpIsDynamicCodePolicyEnabled(out num) == 0 && num != 0;
		}

		// Token: 0x040026C4 RID: 9924
		private static readonly Version VistaOSVersion = new Version(6, 0);

		// Token: 0x040026C5 RID: 9925
		public const int MB_PRECOMPOSED = 1;

		// Token: 0x040026C6 RID: 9926
		public const int SMTO_ABORTIFHUNG = 2;

		// Token: 0x040026C7 RID: 9927
		public const int LAYOUT_RTL = 1;

		// Token: 0x040026C8 RID: 9928
		public const int LAYOUT_BITMAPORIENTATIONPRESERVED = 8;

		// Token: 0x040026C9 RID: 9929
		public static readonly Guid guid_IAccessibleEx = new Guid("{F8B80ADA-2C44-48D0-89BE-5FF23C9CD875}");

		// Token: 0x0200075E RID: 1886
		internal struct POINTSTRUCT
		{
			// Token: 0x06006245 RID: 25157 RVA: 0x00191FC4 File Offset: 0x001901C4
			public POINTSTRUCT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x040041C3 RID: 16835
			public int x;

			// Token: 0x040041C4 RID: 16836
			public int y;
		}

		// Token: 0x0200075F RID: 1887
		[Guid("00000122-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDropTarget
		{
			// Token: 0x06006246 RID: 25158
			[PreserveSig]
			int OleDragEnter([MarshalAs(UnmanagedType.Interface)] [In] object pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [In] UnsafeNativeMethods.POINTSTRUCT pt, [In] [Out] ref int pdwEffect);

			// Token: 0x06006247 RID: 25159
			[PreserveSig]
			int OleDragOver([MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [In] UnsafeNativeMethods.POINTSTRUCT pt, [In] [Out] ref int pdwEffect);

			// Token: 0x06006248 RID: 25160
			[PreserveSig]
			int OleDragLeave();

			// Token: 0x06006249 RID: 25161
			[PreserveSig]
			int OleDrop([MarshalAs(UnmanagedType.Interface)] [In] object pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [In] UnsafeNativeMethods.POINTSTRUCT pt, [In] [Out] ref int pdwEffect);
		}

		// Token: 0x02000760 RID: 1888
		[Guid("00000121-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDropSource
		{
			// Token: 0x0600624A RID: 25162
			[PreserveSig]
			int OleQueryContinueDrag(int fEscapePressed, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState);

			// Token: 0x0600624B RID: 25163
			[PreserveSig]
			int OleGiveFeedback([MarshalAs(UnmanagedType.U4)] [In] int dwEffect);
		}

		// Token: 0x02000761 RID: 1889
		[Guid("00000016-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleMessageFilter
		{
			// Token: 0x0600624C RID: 25164
			[PreserveSig]
			int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);

			// Token: 0x0600624D RID: 25165
			[PreserveSig]
			int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);

			// Token: 0x0600624E RID: 25166
			[PreserveSig]
			int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
		}

		// Token: 0x02000762 RID: 1890
		[Guid("B196B289-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleControlSite
		{
			// Token: 0x0600624F RID: 25167
			[PreserveSig]
			int OnControlInfoChanged();

			// Token: 0x06006250 RID: 25168
			[PreserveSig]
			int LockInPlaceActive(int fLock);

			// Token: 0x06006251 RID: 25169
			[PreserveSig]
			int GetExtendedControl([MarshalAs(UnmanagedType.IDispatch)] out object ppDisp);

			// Token: 0x06006252 RID: 25170
			[PreserveSig]
			int TransformCoords([In] [Out] NativeMethods._POINTL pPtlHimetric, [In] [Out] NativeMethods.tagPOINTF pPtfContainer, [MarshalAs(UnmanagedType.U4)] [In] int dwFlags);

			// Token: 0x06006253 RID: 25171
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG pMsg, [MarshalAs(UnmanagedType.U4)] [In] int grfModifiers);

			// Token: 0x06006254 RID: 25172
			[PreserveSig]
			int OnFocus(int fGotFocus);

			// Token: 0x06006255 RID: 25173
			[PreserveSig]
			int ShowPropertyFrame();
		}

		// Token: 0x02000763 RID: 1891
		[Guid("00000118-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleClientSite
		{
			// Token: 0x06006256 RID: 25174
			[PreserveSig]
			int SaveObject();

			// Token: 0x06006257 RID: 25175
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x06006258 RID: 25176
			[PreserveSig]
			int GetContainer(out UnsafeNativeMethods.IOleContainer container);

			// Token: 0x06006259 RID: 25177
			[PreserveSig]
			int ShowObject();

			// Token: 0x0600625A RID: 25178
			[PreserveSig]
			int OnShowWindow(int fShow);

			// Token: 0x0600625B RID: 25179
			[PreserveSig]
			int RequestNewObjectLayout();
		}

		// Token: 0x02000764 RID: 1892
		[Guid("00000119-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceSite
		{
			// Token: 0x0600625C RID: 25180
			IntPtr GetWindow();

			// Token: 0x0600625D RID: 25181
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x0600625E RID: 25182
			[PreserveSig]
			int CanInPlaceActivate();

			// Token: 0x0600625F RID: 25183
			[PreserveSig]
			int OnInPlaceActivate();

			// Token: 0x06006260 RID: 25184
			[PreserveSig]
			int OnUIActivate();

			// Token: 0x06006261 RID: 25185
			[PreserveSig]
			int GetWindowContext([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, [Out] NativeMethods.COMRECT lprcPosRect, [Out] NativeMethods.COMRECT lprcClipRect, [In] [Out] NativeMethods.tagOIFI lpFrameInfo);

			// Token: 0x06006262 RID: 25186
			[PreserveSig]
			int Scroll(NativeMethods.tagSIZE scrollExtant);

			// Token: 0x06006263 RID: 25187
			[PreserveSig]
			int OnUIDeactivate(int fUndoable);

			// Token: 0x06006264 RID: 25188
			[PreserveSig]
			int OnInPlaceDeactivate();

			// Token: 0x06006265 RID: 25189
			[PreserveSig]
			int DiscardUndoState();

			// Token: 0x06006266 RID: 25190
			[PreserveSig]
			int DeactivateAndUndo();

			// Token: 0x06006267 RID: 25191
			[PreserveSig]
			int OnPosRectChange([In] NativeMethods.COMRECT lprcPosRect);
		}

		// Token: 0x02000765 RID: 1893
		[Guid("742B0E01-14E6-101B-914E-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ISimpleFrameSite
		{
			// Token: 0x06006268 RID: 25192
			[PreserveSig]
			int PreMessageFilter(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] [In] int msg, IntPtr wp, IntPtr lp, [In] [Out] ref IntPtr plResult, [MarshalAs(UnmanagedType.U4)] [In] [Out] ref int pdwCookie);

			// Token: 0x06006269 RID: 25193
			[PreserveSig]
			int PostMessageFilter(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] [In] int msg, IntPtr wp, IntPtr lp, [In] [Out] ref IntPtr plResult, [MarshalAs(UnmanagedType.U4)] [In] int dwCookie);
		}

		// Token: 0x02000766 RID: 1894
		[Guid("40A050A0-3C31-101B-A82E-08002B2B2337")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IVBGetControl
		{
			// Token: 0x0600626A RID: 25194
			[PreserveSig]
			int EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum);
		}

		// Token: 0x02000767 RID: 1895
		[Guid("91733A60-3F4C-101B-A3F6-00AA0034E4E9")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IGetVBAObject
		{
			// Token: 0x0600626B RID: 25195
			[PreserveSig]
			int GetObject([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IVBFormat[] rval, int dwReserved);
		}

		// Token: 0x02000768 RID: 1896
		[Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPropertyNotifySink
		{
			// Token: 0x0600626C RID: 25196
			void OnChanged(int dispID);

			// Token: 0x0600626D RID: 25197
			[PreserveSig]
			int OnRequestEdit(int dispID);
		}

		// Token: 0x02000769 RID: 1897
		[Guid("9849FD60-3768-101B-8D72-AE6164FFE3CF")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IVBFormat
		{
			// Token: 0x0600626E RID: 25198
			[PreserveSig]
			int Format([In] ref object var, IntPtr pszFormat, IntPtr lpBuffer, short cpBuffer, int lcid, short firstD, short firstW, [MarshalAs(UnmanagedType.LPArray)] [Out] short[] result);
		}

		// Token: 0x0200076A RID: 1898
		[Guid("00000100-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumUnknown
		{
			// Token: 0x0600626F RID: 25199
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] IntPtr rgelt, IntPtr pceltFetched);

			// Token: 0x06006270 RID: 25200
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x06006271 RID: 25201
			void Reset();

			// Token: 0x06006272 RID: 25202
			void Clone(out UnsafeNativeMethods.IEnumUnknown ppenum);
		}

		// Token: 0x0200076B RID: 1899
		[Guid("0000011B-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleContainer
		{
			// Token: 0x06006273 RID: 25203
			[PreserveSig]
			int ParseDisplayName([MarshalAs(UnmanagedType.Interface)] [In] object pbc, [MarshalAs(UnmanagedType.BStr)] [In] string pszDisplayName, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pchEaten, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppmkOut);

			// Token: 0x06006274 RID: 25204
			[PreserveSig]
			int EnumObjects([MarshalAs(UnmanagedType.U4)] [In] int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum);

			// Token: 0x06006275 RID: 25205
			[PreserveSig]
			int LockContainer(bool fLock);
		}

		// Token: 0x0200076C RID: 1900
		[Guid("00000116-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceFrame
		{
			// Token: 0x06006276 RID: 25206
			IntPtr GetWindow();

			// Token: 0x06006277 RID: 25207
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06006278 RID: 25208
			[PreserveSig]
			int GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x06006279 RID: 25209
			[PreserveSig]
			int RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x0600627A RID: 25210
			[PreserveSig]
			int SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x0600627B RID: 25211
			[PreserveSig]
			int SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);

			// Token: 0x0600627C RID: 25212
			[PreserveSig]
			int InsertMenus([In] IntPtr hmenuShared, [In] [Out] NativeMethods.tagOleMenuGroupWidths lpMenuWidths);

			// Token: 0x0600627D RID: 25213
			[PreserveSig]
			int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

			// Token: 0x0600627E RID: 25214
			[PreserveSig]
			int RemoveMenus([In] IntPtr hmenuShared);

			// Token: 0x0600627F RID: 25215
			[PreserveSig]
			int SetStatusText([MarshalAs(UnmanagedType.LPWStr)] [In] string pszStatusText);

			// Token: 0x06006280 RID: 25216
			[PreserveSig]
			int EnableModeless(bool fEnable);

			// Token: 0x06006281 RID: 25217
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg, [MarshalAs(UnmanagedType.U2)] [In] short wID);
		}

		// Token: 0x0200076D RID: 1901
		[ComVisible(true)]
		[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDocHostUIHandler
		{
			// Token: 0x06006282 RID: 25218
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowContextMenu([MarshalAs(UnmanagedType.U4)] [In] int dwID, [In] NativeMethods.POINT pt, [MarshalAs(UnmanagedType.Interface)] [In] object pcmdtReserved, [MarshalAs(UnmanagedType.Interface)] [In] object pdispReserved);

			// Token: 0x06006283 RID: 25219
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetHostInfo([In] [Out] NativeMethods.DOCHOSTUIINFO info);

			// Token: 0x06006284 RID: 25220
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowUI([MarshalAs(UnmanagedType.I4)] [In] int dwID, [In] UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, [In] NativeMethods.IOleCommandTarget commandTarget, [In] UnsafeNativeMethods.IOleInPlaceFrame frame, [In] UnsafeNativeMethods.IOleInPlaceUIWindow doc);

			// Token: 0x06006285 RID: 25221
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int HideUI();

			// Token: 0x06006286 RID: 25222
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int UpdateUI();

			// Token: 0x06006287 RID: 25223
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int EnableModeless([MarshalAs(UnmanagedType.Bool)] [In] bool fEnable);

			// Token: 0x06006288 RID: 25224
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnDocWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x06006289 RID: 25225
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnFrameWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x0600628A RID: 25226
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ResizeBorder([In] NativeMethods.COMRECT rect, [In] UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow);

			// Token: 0x0600628B RID: 25227
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG msg, [In] ref Guid group, [MarshalAs(UnmanagedType.I4)] [In] int nCmdID);

			// Token: 0x0600628C RID: 25228
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOptionKeyPath([MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrKey, [MarshalAs(UnmanagedType.U4)] [In] int dw);

			// Token: 0x0600628D RID: 25229
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleDropTarget ppDropTarget);

			// Token: 0x0600628E RID: 25230
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

			// Token: 0x0600628F RID: 25231
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateUrl([MarshalAs(UnmanagedType.U4)] [In] int dwTranslate, [MarshalAs(UnmanagedType.LPWStr)] [In] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

			// Token: 0x06006290 RID: 25232
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
		}

		// Token: 0x0200076E RID: 1902
		[SuppressUnmanagedCodeSecurity]
		[Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
		[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FOleAutomation)]
		[ComImport]
		public interface IWebBrowser2
		{
			// Token: 0x06006291 RID: 25233
			[DispId(100)]
			void GoBack();

			// Token: 0x06006292 RID: 25234
			[DispId(101)]
			void GoForward();

			// Token: 0x06006293 RID: 25235
			[DispId(102)]
			void GoHome();

			// Token: 0x06006294 RID: 25236
			[DispId(103)]
			void GoSearch();

			// Token: 0x06006295 RID: 25237
			[DispId(104)]
			void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			// Token: 0x06006296 RID: 25238
			[DispId(-550)]
			void Refresh();

			// Token: 0x06006297 RID: 25239
			[DispId(105)]
			void Refresh2([In] ref object level);

			// Token: 0x06006298 RID: 25240
			[DispId(106)]
			void Stop();

			// Token: 0x1700177B RID: 6011
			// (get) Token: 0x06006299 RID: 25241
			[DispId(200)]
			object Application { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

			// Token: 0x1700177C RID: 6012
			// (get) Token: 0x0600629A RID: 25242
			[DispId(201)]
			object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

			// Token: 0x1700177D RID: 6013
			// (get) Token: 0x0600629B RID: 25243
			[DispId(202)]
			object Container { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

			// Token: 0x1700177E RID: 6014
			// (get) Token: 0x0600629C RID: 25244
			[DispId(203)]
			object Document { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

			// Token: 0x1700177F RID: 6015
			// (get) Token: 0x0600629D RID: 25245
			[DispId(204)]
			bool TopLevelContainer { get; }

			// Token: 0x17001780 RID: 6016
			// (get) Token: 0x0600629E RID: 25246
			[DispId(205)]
			string Type { get; }

			// Token: 0x17001781 RID: 6017
			// (get) Token: 0x0600629F RID: 25247
			// (set) Token: 0x060062A0 RID: 25248
			[DispId(206)]
			int Left { get; set; }

			// Token: 0x17001782 RID: 6018
			// (get) Token: 0x060062A1 RID: 25249
			// (set) Token: 0x060062A2 RID: 25250
			[DispId(207)]
			int Top { get; set; }

			// Token: 0x17001783 RID: 6019
			// (get) Token: 0x060062A3 RID: 25251
			// (set) Token: 0x060062A4 RID: 25252
			[DispId(208)]
			int Width { get; set; }

			// Token: 0x17001784 RID: 6020
			// (get) Token: 0x060062A5 RID: 25253
			// (set) Token: 0x060062A6 RID: 25254
			[DispId(209)]
			int Height { get; set; }

			// Token: 0x17001785 RID: 6021
			// (get) Token: 0x060062A7 RID: 25255
			[DispId(210)]
			string LocationName { get; }

			// Token: 0x17001786 RID: 6022
			// (get) Token: 0x060062A8 RID: 25256
			[DispId(211)]
			string LocationURL { get; }

			// Token: 0x17001787 RID: 6023
			// (get) Token: 0x060062A9 RID: 25257
			[DispId(212)]
			bool Busy { get; }

			// Token: 0x060062AA RID: 25258
			[DispId(300)]
			void Quit();

			// Token: 0x060062AB RID: 25259
			[DispId(301)]
			void ClientToWindow(out int pcx, out int pcy);

			// Token: 0x060062AC RID: 25260
			[DispId(302)]
			void PutProperty([In] string property, [In] object vtValue);

			// Token: 0x060062AD RID: 25261
			[DispId(303)]
			object GetProperty([In] string property);

			// Token: 0x17001788 RID: 6024
			// (get) Token: 0x060062AE RID: 25262
			[DispId(0)]
			string Name { get; }

			// Token: 0x17001789 RID: 6025
			// (get) Token: 0x060062AF RID: 25263
			[DispId(-515)]
			int HWND { get; }

			// Token: 0x1700178A RID: 6026
			// (get) Token: 0x060062B0 RID: 25264
			[DispId(400)]
			string FullName { get; }

			// Token: 0x1700178B RID: 6027
			// (get) Token: 0x060062B1 RID: 25265
			[DispId(401)]
			string Path { get; }

			// Token: 0x1700178C RID: 6028
			// (get) Token: 0x060062B2 RID: 25266
			// (set) Token: 0x060062B3 RID: 25267
			[DispId(402)]
			bool Visible { get; set; }

			// Token: 0x1700178D RID: 6029
			// (get) Token: 0x060062B4 RID: 25268
			// (set) Token: 0x060062B5 RID: 25269
			[DispId(403)]
			bool StatusBar { get; set; }

			// Token: 0x1700178E RID: 6030
			// (get) Token: 0x060062B6 RID: 25270
			// (set) Token: 0x060062B7 RID: 25271
			[DispId(404)]
			string StatusText { get; set; }

			// Token: 0x1700178F RID: 6031
			// (get) Token: 0x060062B8 RID: 25272
			// (set) Token: 0x060062B9 RID: 25273
			[DispId(405)]
			int ToolBar { get; set; }

			// Token: 0x17001790 RID: 6032
			// (get) Token: 0x060062BA RID: 25274
			// (set) Token: 0x060062BB RID: 25275
			[DispId(406)]
			bool MenuBar { get; set; }

			// Token: 0x17001791 RID: 6033
			// (get) Token: 0x060062BC RID: 25276
			// (set) Token: 0x060062BD RID: 25277
			[DispId(407)]
			bool FullScreen { get; set; }

			// Token: 0x060062BE RID: 25278
			[DispId(500)]
			void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			// Token: 0x060062BF RID: 25279
			[DispId(501)]
			NativeMethods.OLECMDF QueryStatusWB([In] NativeMethods.OLECMDID cmdID);

			// Token: 0x060062C0 RID: 25280
			[DispId(502)]
			void ExecWB([In] NativeMethods.OLECMDID cmdID, [In] NativeMethods.OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);

			// Token: 0x060062C1 RID: 25281
			[DispId(503)]
			void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);

			// Token: 0x17001792 RID: 6034
			// (get) Token: 0x060062C2 RID: 25282
			[DispId(-525)]
			WebBrowserReadyState ReadyState { get; }

			// Token: 0x17001793 RID: 6035
			// (get) Token: 0x060062C3 RID: 25283
			// (set) Token: 0x060062C4 RID: 25284
			[DispId(550)]
			bool Offline { get; set; }

			// Token: 0x17001794 RID: 6036
			// (get) Token: 0x060062C5 RID: 25285
			// (set) Token: 0x060062C6 RID: 25286
			[DispId(551)]
			bool Silent { get; set; }

			// Token: 0x17001795 RID: 6037
			// (get) Token: 0x060062C7 RID: 25287
			// (set) Token: 0x060062C8 RID: 25288
			[DispId(552)]
			bool RegisterAsBrowser { get; set; }

			// Token: 0x17001796 RID: 6038
			// (get) Token: 0x060062C9 RID: 25289
			// (set) Token: 0x060062CA RID: 25290
			[DispId(553)]
			bool RegisterAsDropTarget { get; set; }

			// Token: 0x17001797 RID: 6039
			// (get) Token: 0x060062CB RID: 25291
			// (set) Token: 0x060062CC RID: 25292
			[DispId(554)]
			bool TheaterMode { get; set; }

			// Token: 0x17001798 RID: 6040
			// (get) Token: 0x060062CD RID: 25293
			// (set) Token: 0x060062CE RID: 25294
			[DispId(555)]
			bool AddressBar { get; set; }

			// Token: 0x17001799 RID: 6041
			// (get) Token: 0x060062CF RID: 25295
			// (set) Token: 0x060062D0 RID: 25296
			[DispId(556)]
			bool Resizable { get; set; }
		}

		// Token: 0x0200076F RID: 1903
		[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DWebBrowserEvents2
		{
			// Token: 0x060062D1 RID: 25297
			[DispId(102)]
			void StatusTextChange([In] string text);

			// Token: 0x060062D2 RID: 25298
			[DispId(108)]
			void ProgressChange([In] int progress, [In] int progressMax);

			// Token: 0x060062D3 RID: 25299
			[DispId(105)]
			void CommandStateChange([In] long command, [In] bool enable);

			// Token: 0x060062D4 RID: 25300
			[DispId(106)]
			void DownloadBegin();

			// Token: 0x060062D5 RID: 25301
			[DispId(104)]
			void DownloadComplete();

			// Token: 0x060062D6 RID: 25302
			[DispId(113)]
			void TitleChange([In] string text);

			// Token: 0x060062D7 RID: 25303
			[DispId(112)]
			void PropertyChange([In] string szProperty);

			// Token: 0x060062D8 RID: 25304
			[DispId(250)]
			void BeforeNavigate2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In] [Out] ref bool cancel);

			// Token: 0x060062D9 RID: 25305
			[DispId(251)]
			void NewWindow2([MarshalAs(UnmanagedType.IDispatch)] [In] [Out] ref object pDisp, [In] [Out] ref bool cancel);

			// Token: 0x060062DA RID: 25306
			[DispId(252)]
			void NavigateComplete2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL);

			// Token: 0x060062DB RID: 25307
			[DispId(259)]
			void DocumentComplete([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL);

			// Token: 0x060062DC RID: 25308
			[DispId(253)]
			void OnQuit();

			// Token: 0x060062DD RID: 25309
			[DispId(254)]
			void OnVisible([In] bool visible);

			// Token: 0x060062DE RID: 25310
			[DispId(255)]
			void OnToolBar([In] bool toolBar);

			// Token: 0x060062DF RID: 25311
			[DispId(256)]
			void OnMenuBar([In] bool menuBar);

			// Token: 0x060062E0 RID: 25312
			[DispId(257)]
			void OnStatusBar([In] bool statusBar);

			// Token: 0x060062E1 RID: 25313
			[DispId(258)]
			void OnFullScreen([In] bool fullScreen);

			// Token: 0x060062E2 RID: 25314
			[DispId(260)]
			void OnTheaterMode([In] bool theaterMode);

			// Token: 0x060062E3 RID: 25315
			[DispId(262)]
			void WindowSetResizable([In] bool resizable);

			// Token: 0x060062E4 RID: 25316
			[DispId(264)]
			void WindowSetLeft([In] int left);

			// Token: 0x060062E5 RID: 25317
			[DispId(265)]
			void WindowSetTop([In] int top);

			// Token: 0x060062E6 RID: 25318
			[DispId(266)]
			void WindowSetWidth([In] int width);

			// Token: 0x060062E7 RID: 25319
			[DispId(267)]
			void WindowSetHeight([In] int height);

			// Token: 0x060062E8 RID: 25320
			[DispId(263)]
			void WindowClosing([In] bool isChildWindow, [In] [Out] ref bool cancel);

			// Token: 0x060062E9 RID: 25321
			[DispId(268)]
			void ClientToHostWindow([In] [Out] ref long cx, [In] [Out] ref long cy);

			// Token: 0x060062EA RID: 25322
			[DispId(269)]
			void SetSecureLockIcon([In] int secureLockIcon);

			// Token: 0x060062EB RID: 25323
			[DispId(270)]
			void FileDownload([In] [Out] ref bool cancel);

			// Token: 0x060062EC RID: 25324
			[DispId(271)]
			void NavigateError([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In] [Out] ref bool cancel);

			// Token: 0x060062ED RID: 25325
			[DispId(225)]
			void PrintTemplateInstantiation([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			// Token: 0x060062EE RID: 25326
			[DispId(226)]
			void PrintTemplateTeardown([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			// Token: 0x060062EF RID: 25327
			[DispId(227)]
			void UpdatePageStatus([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object nPage, [In] ref object fDone);

			// Token: 0x060062F0 RID: 25328
			[DispId(272)]
			void PrivacyImpactedStateChange([In] bool bImpacted);
		}

		// Token: 0x02000770 RID: 1904
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("626FC520-A41E-11cf-A731-00A0C9082637")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLDocument
		{
			// Token: 0x060062F1 RID: 25329
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetScript();
		}

		// Token: 0x02000771 RID: 1905
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLDocument2
		{
			// Token: 0x060062F2 RID: 25330
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetScript();

			// Token: 0x060062F3 RID: 25331
			UnsafeNativeMethods.IHTMLElementCollection GetAll();

			// Token: 0x060062F4 RID: 25332
			UnsafeNativeMethods.IHTMLElement GetBody();

			// Token: 0x060062F5 RID: 25333
			UnsafeNativeMethods.IHTMLElement GetActiveElement();

			// Token: 0x060062F6 RID: 25334
			UnsafeNativeMethods.IHTMLElementCollection GetImages();

			// Token: 0x060062F7 RID: 25335
			UnsafeNativeMethods.IHTMLElementCollection GetApplets();

			// Token: 0x060062F8 RID: 25336
			UnsafeNativeMethods.IHTMLElementCollection GetLinks();

			// Token: 0x060062F9 RID: 25337
			UnsafeNativeMethods.IHTMLElementCollection GetForms();

			// Token: 0x060062FA RID: 25338
			UnsafeNativeMethods.IHTMLElementCollection GetAnchors();

			// Token: 0x060062FB RID: 25339
			void SetTitle(string p);

			// Token: 0x060062FC RID: 25340
			string GetTitle();

			// Token: 0x060062FD RID: 25341
			UnsafeNativeMethods.IHTMLElementCollection GetScripts();

			// Token: 0x060062FE RID: 25342
			void SetDesignMode(string p);

			// Token: 0x060062FF RID: 25343
			string GetDesignMode();

			// Token: 0x06006300 RID: 25344
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetSelection();

			// Token: 0x06006301 RID: 25345
			string GetReadyState();

			// Token: 0x06006302 RID: 25346
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();

			// Token: 0x06006303 RID: 25347
			UnsafeNativeMethods.IHTMLElementCollection GetEmbeds();

			// Token: 0x06006304 RID: 25348
			UnsafeNativeMethods.IHTMLElementCollection GetPlugins();

			// Token: 0x06006305 RID: 25349
			void SetAlinkColor(object c);

			// Token: 0x06006306 RID: 25350
			object GetAlinkColor();

			// Token: 0x06006307 RID: 25351
			void SetBgColor(object c);

			// Token: 0x06006308 RID: 25352
			object GetBgColor();

			// Token: 0x06006309 RID: 25353
			void SetFgColor(object c);

			// Token: 0x0600630A RID: 25354
			object GetFgColor();

			// Token: 0x0600630B RID: 25355
			void SetLinkColor(object c);

			// Token: 0x0600630C RID: 25356
			object GetLinkColor();

			// Token: 0x0600630D RID: 25357
			void SetVlinkColor(object c);

			// Token: 0x0600630E RID: 25358
			object GetVlinkColor();

			// Token: 0x0600630F RID: 25359
			string GetReferrer();

			// Token: 0x06006310 RID: 25360
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLLocation GetLocation();

			// Token: 0x06006311 RID: 25361
			string GetLastModified();

			// Token: 0x06006312 RID: 25362
			void SetUrl(string p);

			// Token: 0x06006313 RID: 25363
			string GetUrl();

			// Token: 0x06006314 RID: 25364
			void SetDomain(string p);

			// Token: 0x06006315 RID: 25365
			string GetDomain();

			// Token: 0x06006316 RID: 25366
			void SetCookie(string p);

			// Token: 0x06006317 RID: 25367
			string GetCookie();

			// Token: 0x06006318 RID: 25368
			void SetExpando(bool p);

			// Token: 0x06006319 RID: 25369
			bool GetExpando();

			// Token: 0x0600631A RID: 25370
			void SetCharset(string p);

			// Token: 0x0600631B RID: 25371
			string GetCharset();

			// Token: 0x0600631C RID: 25372
			void SetDefaultCharset(string p);

			// Token: 0x0600631D RID: 25373
			string GetDefaultCharset();

			// Token: 0x0600631E RID: 25374
			string GetMimeType();

			// Token: 0x0600631F RID: 25375
			string GetFileSize();

			// Token: 0x06006320 RID: 25376
			string GetFileCreatedDate();

			// Token: 0x06006321 RID: 25377
			string GetFileModifiedDate();

			// Token: 0x06006322 RID: 25378
			string GetFileUpdatedDate();

			// Token: 0x06006323 RID: 25379
			string GetSecurity();

			// Token: 0x06006324 RID: 25380
			string GetProtocol();

			// Token: 0x06006325 RID: 25381
			string GetNameProp();

			// Token: 0x06006326 RID: 25382
			int Write([MarshalAs(UnmanagedType.SafeArray)] [In] object[] psarray);

			// Token: 0x06006327 RID: 25383
			int WriteLine([MarshalAs(UnmanagedType.SafeArray)] [In] object[] psarray);

			// Token: 0x06006328 RID: 25384
			[return: MarshalAs(UnmanagedType.Interface)]
			object Open(string mimeExtension, object name, object features, object replace);

			// Token: 0x06006329 RID: 25385
			void Close();

			// Token: 0x0600632A RID: 25386
			void Clear();

			// Token: 0x0600632B RID: 25387
			bool QueryCommandSupported(string cmdID);

			// Token: 0x0600632C RID: 25388
			bool QueryCommandEnabled(string cmdID);

			// Token: 0x0600632D RID: 25389
			bool QueryCommandState(string cmdID);

			// Token: 0x0600632E RID: 25390
			bool QueryCommandIndeterm(string cmdID);

			// Token: 0x0600632F RID: 25391
			string QueryCommandText(string cmdID);

			// Token: 0x06006330 RID: 25392
			object QueryCommandValue(string cmdID);

			// Token: 0x06006331 RID: 25393
			bool ExecCommand(string cmdID, bool showUI, object value);

			// Token: 0x06006332 RID: 25394
			bool ExecCommandShowHelp(string cmdID);

			// Token: 0x06006333 RID: 25395
			UnsafeNativeMethods.IHTMLElement CreateElement(string eTag);

			// Token: 0x06006334 RID: 25396
			void SetOnhelp(object p);

			// Token: 0x06006335 RID: 25397
			object GetOnhelp();

			// Token: 0x06006336 RID: 25398
			void SetOnclick(object p);

			// Token: 0x06006337 RID: 25399
			object GetOnclick();

			// Token: 0x06006338 RID: 25400
			void SetOndblclick(object p);

			// Token: 0x06006339 RID: 25401
			object GetOndblclick();

			// Token: 0x0600633A RID: 25402
			void SetOnkeyup(object p);

			// Token: 0x0600633B RID: 25403
			object GetOnkeyup();

			// Token: 0x0600633C RID: 25404
			void SetOnkeydown(object p);

			// Token: 0x0600633D RID: 25405
			object GetOnkeydown();

			// Token: 0x0600633E RID: 25406
			void SetOnkeypress(object p);

			// Token: 0x0600633F RID: 25407
			object GetOnkeypress();

			// Token: 0x06006340 RID: 25408
			void SetOnmouseup(object p);

			// Token: 0x06006341 RID: 25409
			object GetOnmouseup();

			// Token: 0x06006342 RID: 25410
			void SetOnmousedown(object p);

			// Token: 0x06006343 RID: 25411
			object GetOnmousedown();

			// Token: 0x06006344 RID: 25412
			void SetOnmousemove(object p);

			// Token: 0x06006345 RID: 25413
			object GetOnmousemove();

			// Token: 0x06006346 RID: 25414
			void SetOnmouseout(object p);

			// Token: 0x06006347 RID: 25415
			object GetOnmouseout();

			// Token: 0x06006348 RID: 25416
			void SetOnmouseover(object p);

			// Token: 0x06006349 RID: 25417
			object GetOnmouseover();

			// Token: 0x0600634A RID: 25418
			void SetOnreadystatechange(object p);

			// Token: 0x0600634B RID: 25419
			object GetOnreadystatechange();

			// Token: 0x0600634C RID: 25420
			void SetOnafterupdate(object p);

			// Token: 0x0600634D RID: 25421
			object GetOnafterupdate();

			// Token: 0x0600634E RID: 25422
			void SetOnrowexit(object p);

			// Token: 0x0600634F RID: 25423
			object GetOnrowexit();

			// Token: 0x06006350 RID: 25424
			void SetOnrowenter(object p);

			// Token: 0x06006351 RID: 25425
			object GetOnrowenter();

			// Token: 0x06006352 RID: 25426
			void SetOndragstart(object p);

			// Token: 0x06006353 RID: 25427
			object GetOndragstart();

			// Token: 0x06006354 RID: 25428
			void SetOnselectstart(object p);

			// Token: 0x06006355 RID: 25429
			object GetOnselectstart();

			// Token: 0x06006356 RID: 25430
			UnsafeNativeMethods.IHTMLElement ElementFromPoint(int x, int y);

			// Token: 0x06006357 RID: 25431
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 GetParentWindow();

			// Token: 0x06006358 RID: 25432
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetStyleSheets();

			// Token: 0x06006359 RID: 25433
			void SetOnbeforeupdate(object p);

			// Token: 0x0600635A RID: 25434
			object GetOnbeforeupdate();

			// Token: 0x0600635B RID: 25435
			void SetOnerrorupdate(object p);

			// Token: 0x0600635C RID: 25436
			object GetOnerrorupdate();

			// Token: 0x0600635D RID: 25437
			string toString();

			// Token: 0x0600635E RID: 25438
			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateStyleSheet(string bstrHref, int lIndex);
		}

		// Token: 0x02000772 RID: 1906
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F485-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLDocument3
		{
			// Token: 0x0600635F RID: 25439
			void ReleaseCapture();

			// Token: 0x06006360 RID: 25440
			void Recalc([In] bool fForce);

			// Token: 0x06006361 RID: 25441
			object CreateTextNode([In] string text);

			// Token: 0x06006362 RID: 25442
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetDocumentElement();

			// Token: 0x06006363 RID: 25443
			string GetUniqueID();

			// Token: 0x06006364 RID: 25444
			bool AttachEvent([In] string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006365 RID: 25445
			void DetachEvent([In] string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006366 RID: 25446
			void SetOnrowsdelete([In] object p);

			// Token: 0x06006367 RID: 25447
			object GetOnrowsdelete();

			// Token: 0x06006368 RID: 25448
			void SetOnrowsinserted([In] object p);

			// Token: 0x06006369 RID: 25449
			object GetOnrowsinserted();

			// Token: 0x0600636A RID: 25450
			void SetOncellchange([In] object p);

			// Token: 0x0600636B RID: 25451
			object GetOncellchange();

			// Token: 0x0600636C RID: 25452
			void SetOndatasetchanged([In] object p);

			// Token: 0x0600636D RID: 25453
			object GetOndatasetchanged();

			// Token: 0x0600636E RID: 25454
			void SetOndataavailable([In] object p);

			// Token: 0x0600636F RID: 25455
			object GetOndataavailable();

			// Token: 0x06006370 RID: 25456
			void SetOndatasetcomplete([In] object p);

			// Token: 0x06006371 RID: 25457
			object GetOndatasetcomplete();

			// Token: 0x06006372 RID: 25458
			void SetOnpropertychange([In] object p);

			// Token: 0x06006373 RID: 25459
			object GetOnpropertychange();

			// Token: 0x06006374 RID: 25460
			void SetDir([In] string p);

			// Token: 0x06006375 RID: 25461
			string GetDir();

			// Token: 0x06006376 RID: 25462
			void SetOncontextmenu([In] object p);

			// Token: 0x06006377 RID: 25463
			object GetOncontextmenu();

			// Token: 0x06006378 RID: 25464
			void SetOnstop([In] object p);

			// Token: 0x06006379 RID: 25465
			object GetOnstop();

			// Token: 0x0600637A RID: 25466
			object CreateDocumentFragment();

			// Token: 0x0600637B RID: 25467
			object GetParentDocument();

			// Token: 0x0600637C RID: 25468
			void SetEnableDownload([In] bool p);

			// Token: 0x0600637D RID: 25469
			bool GetEnableDownload();

			// Token: 0x0600637E RID: 25470
			void SetBaseUrl([In] string p);

			// Token: 0x0600637F RID: 25471
			string GetBaseUrl();

			// Token: 0x06006380 RID: 25472
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetChildNodes();

			// Token: 0x06006381 RID: 25473
			void SetInheritStyleSheets([In] bool p);

			// Token: 0x06006382 RID: 25474
			bool GetInheritStyleSheets();

			// Token: 0x06006383 RID: 25475
			void SetOnbeforeeditfocus([In] object p);

			// Token: 0x06006384 RID: 25476
			object GetOnbeforeeditfocus();

			// Token: 0x06006385 RID: 25477
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByName([In] string v);

			// Token: 0x06006386 RID: 25478
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetElementById([In] string v);

			// Token: 0x06006387 RID: 25479
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByTagName([In] string v);
		}

		// Token: 0x02000773 RID: 1907
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F69A-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLDocument4
		{
			// Token: 0x06006388 RID: 25480
			void Focus();

			// Token: 0x06006389 RID: 25481
			bool HasFocus();

			// Token: 0x0600638A RID: 25482
			void SetOnselectionchange(object p);

			// Token: 0x0600638B RID: 25483
			object GetOnselectionchange();

			// Token: 0x0600638C RID: 25484
			object GetNamespaces();

			// Token: 0x0600638D RID: 25485
			object createDocumentFromUrl(string bstrUrl, string bstrOptions);

			// Token: 0x0600638E RID: 25486
			void SetMedia(string bstrMedia);

			// Token: 0x0600638F RID: 25487
			string GetMedia();

			// Token: 0x06006390 RID: 25488
			object CreateEventObject([In] [Optional] ref object eventObject);

			// Token: 0x06006391 RID: 25489
			bool FireEvent(string eventName);

			// Token: 0x06006392 RID: 25490
			object CreateRenderStyle(string bstr);

			// Token: 0x06006393 RID: 25491
			void SetOncontrolselect(object p);

			// Token: 0x06006394 RID: 25492
			object GetOncontrolselect();

			// Token: 0x06006395 RID: 25493
			string GetURLUnencoded();
		}

		// Token: 0x02000774 RID: 1908
		[Guid("3050f613-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLDocumentEvents2
		{
			// Token: 0x06006396 RID: 25494
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006397 RID: 25495
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006398 RID: 25496
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006399 RID: 25497
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639A RID: 25498
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639B RID: 25499
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639C RID: 25500
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639D RID: 25501
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639E RID: 25502
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600639F RID: 25503
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A0 RID: 25504
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A1 RID: 25505
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A2 RID: 25506
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A3 RID: 25507
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A4 RID: 25508
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A5 RID: 25509
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A6 RID: 25510
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A7 RID: 25511
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A8 RID: 25512
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063A9 RID: 25513
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AA RID: 25514
			[DispId(1026)]
			bool onstop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AB RID: 25515
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AC RID: 25516
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AD RID: 25517
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AE RID: 25518
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063AF RID: 25519
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B0 RID: 25520
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B1 RID: 25521
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B2 RID: 25522
			[DispId(1027)]
			void onbeforeeditfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B3 RID: 25523
			[DispId(1037)]
			void onselectionchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B4 RID: 25524
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B5 RID: 25525
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B6 RID: 25526
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B7 RID: 25527
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B8 RID: 25528
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063B9 RID: 25529
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063BA RID: 25530
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060063BB RID: 25531
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000775 RID: 1909
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("332C4426-26CB-11D0-B483-00C04FD90119")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLFramesCollection2
		{
			// Token: 0x060063BC RID: 25532
			object Item(ref object idOrName);

			// Token: 0x060063BD RID: 25533
			int GetLength();
		}

		// Token: 0x02000776 RID: 1910
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("332C4427-26CB-11D0-B483-00C04FD90119")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLWindow2
		{
			// Token: 0x060063BE RID: 25534
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item([In] ref object pvarIndex);

			// Token: 0x060063BF RID: 25535
			int GetLength();

			// Token: 0x060063C0 RID: 25536
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLFramesCollection2 GetFrames();

			// Token: 0x060063C1 RID: 25537
			void SetDefaultStatus([In] string p);

			// Token: 0x060063C2 RID: 25538
			string GetDefaultStatus();

			// Token: 0x060063C3 RID: 25539
			void SetStatus([In] string p);

			// Token: 0x060063C4 RID: 25540
			string GetStatus();

			// Token: 0x060063C5 RID: 25541
			int SetTimeout([In] string expression, [In] int msec, [In] ref object language);

			// Token: 0x060063C6 RID: 25542
			void ClearTimeout([In] int timerID);

			// Token: 0x060063C7 RID: 25543
			void Alert([In] string message);

			// Token: 0x060063C8 RID: 25544
			bool Confirm([In] string message);

			// Token: 0x060063C9 RID: 25545
			[return: MarshalAs(UnmanagedType.Struct)]
			object Prompt([In] string message, [In] string defstr);

			// Token: 0x060063CA RID: 25546
			object GetImage();

			// Token: 0x060063CB RID: 25547
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLLocation GetLocation();

			// Token: 0x060063CC RID: 25548
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOmHistory GetHistory();

			// Token: 0x060063CD RID: 25549
			void Close();

			// Token: 0x060063CE RID: 25550
			void SetOpener([In] object p);

			// Token: 0x060063CF RID: 25551
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetOpener();

			// Token: 0x060063D0 RID: 25552
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOmNavigator GetNavigator();

			// Token: 0x060063D1 RID: 25553
			void SetName([In] string p);

			// Token: 0x060063D2 RID: 25554
			string GetName();

			// Token: 0x060063D3 RID: 25555
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 GetParent();

			// Token: 0x060063D4 RID: 25556
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 Open([In] string URL, [In] string name, [In] string features, [In] bool replace);

			// Token: 0x060063D5 RID: 25557
			object GetSelf();

			// Token: 0x060063D6 RID: 25558
			object GetTop();

			// Token: 0x060063D7 RID: 25559
			object GetWindow();

			// Token: 0x060063D8 RID: 25560
			void Navigate([In] string URL);

			// Token: 0x060063D9 RID: 25561
			void SetOnfocus([In] object p);

			// Token: 0x060063DA RID: 25562
			object GetOnfocus();

			// Token: 0x060063DB RID: 25563
			void SetOnblur([In] object p);

			// Token: 0x060063DC RID: 25564
			object GetOnblur();

			// Token: 0x060063DD RID: 25565
			void SetOnload([In] object p);

			// Token: 0x060063DE RID: 25566
			object GetOnload();

			// Token: 0x060063DF RID: 25567
			void SetOnbeforeunload(object p);

			// Token: 0x060063E0 RID: 25568
			object GetOnbeforeunload();

			// Token: 0x060063E1 RID: 25569
			void SetOnunload([In] object p);

			// Token: 0x060063E2 RID: 25570
			object GetOnunload();

			// Token: 0x060063E3 RID: 25571
			void SetOnhelp(object p);

			// Token: 0x060063E4 RID: 25572
			object GetOnhelp();

			// Token: 0x060063E5 RID: 25573
			void SetOnerror([In] object p);

			// Token: 0x060063E6 RID: 25574
			object GetOnerror();

			// Token: 0x060063E7 RID: 25575
			void SetOnresize([In] object p);

			// Token: 0x060063E8 RID: 25576
			object GetOnresize();

			// Token: 0x060063E9 RID: 25577
			void SetOnscroll([In] object p);

			// Token: 0x060063EA RID: 25578
			object GetOnscroll();

			// Token: 0x060063EB RID: 25579
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument2 GetDocument();

			// Token: 0x060063EC RID: 25580
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLEventObj GetEvent();

			// Token: 0x060063ED RID: 25581
			object Get_newEnum();

			// Token: 0x060063EE RID: 25582
			object ShowModalDialog([In] string dialog, [In] ref object varArgIn, [In] ref object varOptions);

			// Token: 0x060063EF RID: 25583
			void ShowHelp([In] string helpURL, [In] object helpArg, [In] string features);

			// Token: 0x060063F0 RID: 25584
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLScreen GetScreen();

			// Token: 0x060063F1 RID: 25585
			object GetOption();

			// Token: 0x060063F2 RID: 25586
			void Focus();

			// Token: 0x060063F3 RID: 25587
			bool GetClosed();

			// Token: 0x060063F4 RID: 25588
			void Blur();

			// Token: 0x060063F5 RID: 25589
			void Scroll([In] int x, [In] int y);

			// Token: 0x060063F6 RID: 25590
			object GetClientInformation();

			// Token: 0x060063F7 RID: 25591
			int SetInterval([In] string expression, [In] int msec, [In] ref object language);

			// Token: 0x060063F8 RID: 25592
			void ClearInterval([In] int timerID);

			// Token: 0x060063F9 RID: 25593
			void SetOffscreenBuffering([In] object p);

			// Token: 0x060063FA RID: 25594
			object GetOffscreenBuffering();

			// Token: 0x060063FB RID: 25595
			[return: MarshalAs(UnmanagedType.Struct)]
			object ExecScript([In] string code, [In] string language);

			// Token: 0x060063FC RID: 25596
			string toString();

			// Token: 0x060063FD RID: 25597
			void ScrollBy([In] int x, [In] int y);

			// Token: 0x060063FE RID: 25598
			void ScrollTo([In] int x, [In] int y);

			// Token: 0x060063FF RID: 25599
			void MoveTo([In] int x, [In] int y);

			// Token: 0x06006400 RID: 25600
			void MoveBy([In] int x, [In] int y);

			// Token: 0x06006401 RID: 25601
			void ResizeTo([In] int x, [In] int y);

			// Token: 0x06006402 RID: 25602
			void ResizeBy([In] int x, [In] int y);

			// Token: 0x06006403 RID: 25603
			object GetExternal();
		}

		// Token: 0x02000777 RID: 1911
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f4ae-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLWindow3
		{
			// Token: 0x06006404 RID: 25604
			int GetScreenLeft();

			// Token: 0x06006405 RID: 25605
			int GetScreenTop();

			// Token: 0x06006406 RID: 25606
			bool AttachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006407 RID: 25607
			void DetachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006408 RID: 25608
			int SetTimeout([In] ref object expression, int msec, [In] ref object language);

			// Token: 0x06006409 RID: 25609
			int SetInterval([In] ref object expression, int msec, [In] ref object language);

			// Token: 0x0600640A RID: 25610
			void Print();

			// Token: 0x0600640B RID: 25611
			void SetBeforePrint(object o);

			// Token: 0x0600640C RID: 25612
			object GetBeforePrint();

			// Token: 0x0600640D RID: 25613
			void SetAfterPrint(object o);

			// Token: 0x0600640E RID: 25614
			object GetAfterPrint();

			// Token: 0x0600640F RID: 25615
			object GetClipboardData();

			// Token: 0x06006410 RID: 25616
			object ShowModelessDialog(string url, object varArgIn, object options);
		}

		// Token: 0x02000778 RID: 1912
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f6cf-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLWindow4
		{
			// Token: 0x06006411 RID: 25617
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object CreatePopup([In] ref object reserved);

			// Token: 0x06006412 RID: 25618
			[return: MarshalAs(UnmanagedType.Interface)]
			object frameElement();
		}

		// Token: 0x02000779 RID: 1913
		[Guid("3050f625-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLWindowEvents2
		{
			// Token: 0x06006413 RID: 25619
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006414 RID: 25620
			[DispId(1008)]
			void onunload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006415 RID: 25621
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006416 RID: 25622
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006417 RID: 25623
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006418 RID: 25624
			[DispId(1002)]
			bool onerror(string description, string url, int line);

			// Token: 0x06006419 RID: 25625
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600641A RID: 25626
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600641B RID: 25627
			[DispId(1017)]
			void onbeforeunload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600641C RID: 25628
			[DispId(1024)]
			void onbeforeprint(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600641D RID: 25629
			[DispId(1025)]
			void onafterprint(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200077A RID: 1914
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f666-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLPopup
		{
			// Token: 0x0600641E RID: 25630
			void show(int x, int y, int w, int h, ref object element);

			// Token: 0x0600641F RID: 25631
			void hide();

			// Token: 0x06006420 RID: 25632
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument GetDocument();

			// Token: 0x06006421 RID: 25633
			bool IsOpen();
		}

		// Token: 0x0200077B RID: 1915
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f35c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLScreen
		{
			// Token: 0x06006422 RID: 25634
			int GetColorDepth();

			// Token: 0x06006423 RID: 25635
			void SetBufferDepth(int d);

			// Token: 0x06006424 RID: 25636
			int GetBufferDepth();

			// Token: 0x06006425 RID: 25637
			int GetWidth();

			// Token: 0x06006426 RID: 25638
			int GetHeight();

			// Token: 0x06006427 RID: 25639
			void SetUpdateInterval(int i);

			// Token: 0x06006428 RID: 25640
			int GetUpdateInterval();

			// Token: 0x06006429 RID: 25641
			int GetAvailHeight();

			// Token: 0x0600642A RID: 25642
			int GetAvailWidth();

			// Token: 0x0600642B RID: 25643
			bool GetFontSmoothingEnabled();
		}

		// Token: 0x0200077C RID: 1916
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("163BB1E0-6E00-11CF-837A-48DC04C10000")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLLocation
		{
			// Token: 0x0600642C RID: 25644
			void SetHref([In] string p);

			// Token: 0x0600642D RID: 25645
			string GetHref();

			// Token: 0x0600642E RID: 25646
			void SetProtocol([In] string p);

			// Token: 0x0600642F RID: 25647
			string GetProtocol();

			// Token: 0x06006430 RID: 25648
			void SetHost([In] string p);

			// Token: 0x06006431 RID: 25649
			string GetHost();

			// Token: 0x06006432 RID: 25650
			void SetHostname([In] string p);

			// Token: 0x06006433 RID: 25651
			string GetHostname();

			// Token: 0x06006434 RID: 25652
			void SetPort([In] string p);

			// Token: 0x06006435 RID: 25653
			string GetPort();

			// Token: 0x06006436 RID: 25654
			void SetPathname([In] string p);

			// Token: 0x06006437 RID: 25655
			string GetPathname();

			// Token: 0x06006438 RID: 25656
			void SetSearch([In] string p);

			// Token: 0x06006439 RID: 25657
			string GetSearch();

			// Token: 0x0600643A RID: 25658
			void SetHash([In] string p);

			// Token: 0x0600643B RID: 25659
			string GetHash();

			// Token: 0x0600643C RID: 25660
			void Reload([In] bool flag);

			// Token: 0x0600643D RID: 25661
			void Replace([In] string bstr);

			// Token: 0x0600643E RID: 25662
			void Assign([In] string bstr);
		}

		// Token: 0x0200077D RID: 1917
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("FECEAAA2-8405-11CF-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IOmHistory
		{
			// Token: 0x0600643F RID: 25663
			short GetLength();

			// Token: 0x06006440 RID: 25664
			void Back();

			// Token: 0x06006441 RID: 25665
			void Forward();

			// Token: 0x06006442 RID: 25666
			void Go([In] ref object pvargdistance);
		}

		// Token: 0x0200077E RID: 1918
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("FECEAAA5-8405-11CF-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IOmNavigator
		{
			// Token: 0x06006443 RID: 25667
			string GetAppCodeName();

			// Token: 0x06006444 RID: 25668
			string GetAppName();

			// Token: 0x06006445 RID: 25669
			string GetAppVersion();

			// Token: 0x06006446 RID: 25670
			string GetUserAgent();

			// Token: 0x06006447 RID: 25671
			bool JavaEnabled();

			// Token: 0x06006448 RID: 25672
			bool TaintEnabled();

			// Token: 0x06006449 RID: 25673
			object GetMimeTypes();

			// Token: 0x0600644A RID: 25674
			object GetPlugins();

			// Token: 0x0600644B RID: 25675
			bool GetCookieEnabled();

			// Token: 0x0600644C RID: 25676
			object GetOpsProfile();

			// Token: 0x0600644D RID: 25677
			string GetCpuClass();

			// Token: 0x0600644E RID: 25678
			string GetSystemLanguage();

			// Token: 0x0600644F RID: 25679
			string GetBrowserLanguage();

			// Token: 0x06006450 RID: 25680
			string GetUserLanguage();

			// Token: 0x06006451 RID: 25681
			string GetPlatform();

			// Token: 0x06006452 RID: 25682
			string GetAppMinorVersion();

			// Token: 0x06006453 RID: 25683
			int GetConnectionSpeed();

			// Token: 0x06006454 RID: 25684
			bool GetOnLine();

			// Token: 0x06006455 RID: 25685
			object GetUserProfile();
		}

		// Token: 0x0200077F RID: 1919
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLEventObj
		{
			// Token: 0x06006456 RID: 25686
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetSrcElement();

			// Token: 0x06006457 RID: 25687
			bool GetAltKey();

			// Token: 0x06006458 RID: 25688
			bool GetCtrlKey();

			// Token: 0x06006459 RID: 25689
			bool GetShiftKey();

			// Token: 0x0600645A RID: 25690
			void SetReturnValue(object p);

			// Token: 0x0600645B RID: 25691
			object GetReturnValue();

			// Token: 0x0600645C RID: 25692
			void SetCancelBubble(bool p);

			// Token: 0x0600645D RID: 25693
			bool GetCancelBubble();

			// Token: 0x0600645E RID: 25694
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetFromElement();

			// Token: 0x0600645F RID: 25695
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetToElement();

			// Token: 0x06006460 RID: 25696
			void SetKeyCode([In] int p);

			// Token: 0x06006461 RID: 25697
			int GetKeyCode();

			// Token: 0x06006462 RID: 25698
			int GetButton();

			// Token: 0x06006463 RID: 25699
			string GetEventType();

			// Token: 0x06006464 RID: 25700
			string GetQualifier();

			// Token: 0x06006465 RID: 25701
			int GetReason();

			// Token: 0x06006466 RID: 25702
			int GetX();

			// Token: 0x06006467 RID: 25703
			int GetY();

			// Token: 0x06006468 RID: 25704
			int GetClientX();

			// Token: 0x06006469 RID: 25705
			int GetClientY();

			// Token: 0x0600646A RID: 25706
			int GetOffsetX();

			// Token: 0x0600646B RID: 25707
			int GetOffsetY();

			// Token: 0x0600646C RID: 25708
			int GetScreenX();

			// Token: 0x0600646D RID: 25709
			int GetScreenY();

			// Token: 0x0600646E RID: 25710
			object GetSrcFilter();
		}

		// Token: 0x02000780 RID: 1920
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f48B-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLEventObj2
		{
			// Token: 0x0600646F RID: 25711
			void SetAttribute(string attributeName, object attributeValue, int lFlags);

			// Token: 0x06006470 RID: 25712
			object GetAttribute(string attributeName, int lFlags);

			// Token: 0x06006471 RID: 25713
			bool RemoveAttribute(string attributeName, int lFlags);

			// Token: 0x06006472 RID: 25714
			void SetPropertyName(string name);

			// Token: 0x06006473 RID: 25715
			string GetPropertyName();

			// Token: 0x06006474 RID: 25716
			void SetBookmarks(ref object bm);

			// Token: 0x06006475 RID: 25717
			object GetBookmarks();

			// Token: 0x06006476 RID: 25718
			void SetRecordset(ref object rs);

			// Token: 0x06006477 RID: 25719
			object GetRecordset();

			// Token: 0x06006478 RID: 25720
			void SetDataFld(string df);

			// Token: 0x06006479 RID: 25721
			string GetDataFld();

			// Token: 0x0600647A RID: 25722
			void SetBoundElements(ref object be);

			// Token: 0x0600647B RID: 25723
			object GetBoundElements();

			// Token: 0x0600647C RID: 25724
			void SetRepeat(bool r);

			// Token: 0x0600647D RID: 25725
			bool GetRepeat();

			// Token: 0x0600647E RID: 25726
			void SetSrcUrn(string urn);

			// Token: 0x0600647F RID: 25727
			string GetSrcUrn();

			// Token: 0x06006480 RID: 25728
			void SetSrcElement(ref object se);

			// Token: 0x06006481 RID: 25729
			object GetSrcElement();

			// Token: 0x06006482 RID: 25730
			void SetAltKey(bool alt);

			// Token: 0x06006483 RID: 25731
			bool GetAltKey();

			// Token: 0x06006484 RID: 25732
			void SetCtrlKey(bool ctrl);

			// Token: 0x06006485 RID: 25733
			bool GetCtrlKey();

			// Token: 0x06006486 RID: 25734
			void SetShiftKey(bool shift);

			// Token: 0x06006487 RID: 25735
			bool GetShiftKey();

			// Token: 0x06006488 RID: 25736
			void SetFromElement(ref object element);

			// Token: 0x06006489 RID: 25737
			object GetFromElement();

			// Token: 0x0600648A RID: 25738
			void SetToElement(ref object element);

			// Token: 0x0600648B RID: 25739
			object GetToElement();

			// Token: 0x0600648C RID: 25740
			void SetButton(int b);

			// Token: 0x0600648D RID: 25741
			int GetButton();

			// Token: 0x0600648E RID: 25742
			void SetType(string type);

			// Token: 0x0600648F RID: 25743
			string GetType();

			// Token: 0x06006490 RID: 25744
			void SetQualifier(string q);

			// Token: 0x06006491 RID: 25745
			string GetQualifier();

			// Token: 0x06006492 RID: 25746
			void SetReason(int r);

			// Token: 0x06006493 RID: 25747
			int GetReason();

			// Token: 0x06006494 RID: 25748
			void SetX(int x);

			// Token: 0x06006495 RID: 25749
			int GetX();

			// Token: 0x06006496 RID: 25750
			void SetY(int y);

			// Token: 0x06006497 RID: 25751
			int GetY();

			// Token: 0x06006498 RID: 25752
			void SetClientX(int x);

			// Token: 0x06006499 RID: 25753
			int GetClientX();

			// Token: 0x0600649A RID: 25754
			void SetClientY(int y);

			// Token: 0x0600649B RID: 25755
			int GetClientY();

			// Token: 0x0600649C RID: 25756
			void SetOffsetX(int x);

			// Token: 0x0600649D RID: 25757
			int GetOffsetX();

			// Token: 0x0600649E RID: 25758
			void SetOffsetY(int y);

			// Token: 0x0600649F RID: 25759
			int GetOffsetY();

			// Token: 0x060064A0 RID: 25760
			void SetScreenX(int x);

			// Token: 0x060064A1 RID: 25761
			int GetScreenX();

			// Token: 0x060064A2 RID: 25762
			void SetScreenY(int y);

			// Token: 0x060064A3 RID: 25763
			int GetScreenY();

			// Token: 0x060064A4 RID: 25764
			void SetSrcFilter(ref object filter);

			// Token: 0x060064A5 RID: 25765
			object GetSrcFilter();

			// Token: 0x060064A6 RID: 25766
			object GetDataTransfer();
		}

		// Token: 0x02000781 RID: 1921
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f814-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLEventObj4
		{
			// Token: 0x060064A7 RID: 25767
			int GetWheelDelta();
		}

		// Token: 0x02000782 RID: 1922
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLElementCollection
		{
			// Token: 0x060064A8 RID: 25768
			string toString();

			// Token: 0x060064A9 RID: 25769
			void SetLength(int p);

			// Token: 0x060064AA RID: 25770
			int GetLength();

			// Token: 0x060064AB RID: 25771
			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			// Token: 0x060064AC RID: 25772
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item(object idOrName, object index);

			// Token: 0x060064AD RID: 25773
			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags(object tagName);
		}

		// Token: 0x02000783 RID: 1923
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLElement
		{
			// Token: 0x060064AE RID: 25774
			void SetAttribute(string attributeName, object attributeValue, int lFlags);

			// Token: 0x060064AF RID: 25775
			object GetAttribute(string attributeName, int lFlags);

			// Token: 0x060064B0 RID: 25776
			bool RemoveAttribute(string strAttributeName, int lFlags);

			// Token: 0x060064B1 RID: 25777
			void SetClassName(string p);

			// Token: 0x060064B2 RID: 25778
			string GetClassName();

			// Token: 0x060064B3 RID: 25779
			void SetId(string p);

			// Token: 0x060064B4 RID: 25780
			string GetId();

			// Token: 0x060064B5 RID: 25781
			string GetTagName();

			// Token: 0x060064B6 RID: 25782
			UnsafeNativeMethods.IHTMLElement GetParentElement();

			// Token: 0x060064B7 RID: 25783
			UnsafeNativeMethods.IHTMLStyle GetStyle();

			// Token: 0x060064B8 RID: 25784
			void SetOnhelp(object p);

			// Token: 0x060064B9 RID: 25785
			object GetOnhelp();

			// Token: 0x060064BA RID: 25786
			void SetOnclick(object p);

			// Token: 0x060064BB RID: 25787
			object GetOnclick();

			// Token: 0x060064BC RID: 25788
			void SetOndblclick(object p);

			// Token: 0x060064BD RID: 25789
			object GetOndblclick();

			// Token: 0x060064BE RID: 25790
			void SetOnkeydown(object p);

			// Token: 0x060064BF RID: 25791
			object GetOnkeydown();

			// Token: 0x060064C0 RID: 25792
			void SetOnkeyup(object p);

			// Token: 0x060064C1 RID: 25793
			object GetOnkeyup();

			// Token: 0x060064C2 RID: 25794
			void SetOnkeypress(object p);

			// Token: 0x060064C3 RID: 25795
			object GetOnkeypress();

			// Token: 0x060064C4 RID: 25796
			void SetOnmouseout(object p);

			// Token: 0x060064C5 RID: 25797
			object GetOnmouseout();

			// Token: 0x060064C6 RID: 25798
			void SetOnmouseover(object p);

			// Token: 0x060064C7 RID: 25799
			object GetOnmouseover();

			// Token: 0x060064C8 RID: 25800
			void SetOnmousemove(object p);

			// Token: 0x060064C9 RID: 25801
			object GetOnmousemove();

			// Token: 0x060064CA RID: 25802
			void SetOnmousedown(object p);

			// Token: 0x060064CB RID: 25803
			object GetOnmousedown();

			// Token: 0x060064CC RID: 25804
			void SetOnmouseup(object p);

			// Token: 0x060064CD RID: 25805
			object GetOnmouseup();

			// Token: 0x060064CE RID: 25806
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument2 GetDocument();

			// Token: 0x060064CF RID: 25807
			void SetTitle(string p);

			// Token: 0x060064D0 RID: 25808
			string GetTitle();

			// Token: 0x060064D1 RID: 25809
			void SetLanguage(string p);

			// Token: 0x060064D2 RID: 25810
			string GetLanguage();

			// Token: 0x060064D3 RID: 25811
			void SetOnselectstart(object p);

			// Token: 0x060064D4 RID: 25812
			object GetOnselectstart();

			// Token: 0x060064D5 RID: 25813
			void ScrollIntoView(object varargStart);

			// Token: 0x060064D6 RID: 25814
			bool Contains(UnsafeNativeMethods.IHTMLElement pChild);

			// Token: 0x060064D7 RID: 25815
			int GetSourceIndex();

			// Token: 0x060064D8 RID: 25816
			object GetRecordNumber();

			// Token: 0x060064D9 RID: 25817
			void SetLang(string p);

			// Token: 0x060064DA RID: 25818
			string GetLang();

			// Token: 0x060064DB RID: 25819
			int GetOffsetLeft();

			// Token: 0x060064DC RID: 25820
			int GetOffsetTop();

			// Token: 0x060064DD RID: 25821
			int GetOffsetWidth();

			// Token: 0x060064DE RID: 25822
			int GetOffsetHeight();

			// Token: 0x060064DF RID: 25823
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetOffsetParent();

			// Token: 0x060064E0 RID: 25824
			void SetInnerHTML(string p);

			// Token: 0x060064E1 RID: 25825
			string GetInnerHTML();

			// Token: 0x060064E2 RID: 25826
			void SetInnerText(string p);

			// Token: 0x060064E3 RID: 25827
			string GetInnerText();

			// Token: 0x060064E4 RID: 25828
			void SetOuterHTML(string p);

			// Token: 0x060064E5 RID: 25829
			string GetOuterHTML();

			// Token: 0x060064E6 RID: 25830
			void SetOuterText(string p);

			// Token: 0x060064E7 RID: 25831
			string GetOuterText();

			// Token: 0x060064E8 RID: 25832
			void InsertAdjacentHTML(string where, string html);

			// Token: 0x060064E9 RID: 25833
			void InsertAdjacentText(string where, string text);

			// Token: 0x060064EA RID: 25834
			UnsafeNativeMethods.IHTMLElement GetParentTextEdit();

			// Token: 0x060064EB RID: 25835
			bool GetIsTextEdit();

			// Token: 0x060064EC RID: 25836
			void Click();

			// Token: 0x060064ED RID: 25837
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFilters();

			// Token: 0x060064EE RID: 25838
			void SetOndragstart(object p);

			// Token: 0x060064EF RID: 25839
			object GetOndragstart();

			// Token: 0x060064F0 RID: 25840
			string toString();

			// Token: 0x060064F1 RID: 25841
			void SetOnbeforeupdate(object p);

			// Token: 0x060064F2 RID: 25842
			object GetOnbeforeupdate();

			// Token: 0x060064F3 RID: 25843
			void SetOnafterupdate(object p);

			// Token: 0x060064F4 RID: 25844
			object GetOnafterupdate();

			// Token: 0x060064F5 RID: 25845
			void SetOnerrorupdate(object p);

			// Token: 0x060064F6 RID: 25846
			object GetOnerrorupdate();

			// Token: 0x060064F7 RID: 25847
			void SetOnrowexit(object p);

			// Token: 0x060064F8 RID: 25848
			object GetOnrowexit();

			// Token: 0x060064F9 RID: 25849
			void SetOnrowenter(object p);

			// Token: 0x060064FA RID: 25850
			object GetOnrowenter();

			// Token: 0x060064FB RID: 25851
			void SetOndatasetchanged(object p);

			// Token: 0x060064FC RID: 25852
			object GetOndatasetchanged();

			// Token: 0x060064FD RID: 25853
			void SetOndataavailable(object p);

			// Token: 0x060064FE RID: 25854
			object GetOndataavailable();

			// Token: 0x060064FF RID: 25855
			void SetOndatasetcomplete(object p);

			// Token: 0x06006500 RID: 25856
			object GetOndatasetcomplete();

			// Token: 0x06006501 RID: 25857
			void SetOnfilterchange(object p);

			// Token: 0x06006502 RID: 25858
			object GetOnfilterchange();

			// Token: 0x06006503 RID: 25859
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetChildren();

			// Token: 0x06006504 RID: 25860
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetAll();
		}

		// Token: 0x02000784 RID: 1924
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f434-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLElement2
		{
			// Token: 0x06006505 RID: 25861
			string ScopeName();

			// Token: 0x06006506 RID: 25862
			void SetCapture(bool containerCapture);

			// Token: 0x06006507 RID: 25863
			void ReleaseCapture();

			// Token: 0x06006508 RID: 25864
			void SetOnLoseCapture(object v);

			// Token: 0x06006509 RID: 25865
			object GetOnLoseCapture();

			// Token: 0x0600650A RID: 25866
			string GetComponentFromPoint(int x, int y);

			// Token: 0x0600650B RID: 25867
			void DoScroll(object component);

			// Token: 0x0600650C RID: 25868
			void SetOnScroll(object v);

			// Token: 0x0600650D RID: 25869
			object GetOnScroll();

			// Token: 0x0600650E RID: 25870
			void SetOnDrag(object v);

			// Token: 0x0600650F RID: 25871
			object GetOnDrag();

			// Token: 0x06006510 RID: 25872
			void SetOnDragEnd(object v);

			// Token: 0x06006511 RID: 25873
			object GetOnDragEnd();

			// Token: 0x06006512 RID: 25874
			void SetOnDragEnter(object v);

			// Token: 0x06006513 RID: 25875
			object GetOnDragEnter();

			// Token: 0x06006514 RID: 25876
			void SetOnDragOver(object v);

			// Token: 0x06006515 RID: 25877
			object GetOnDragOver();

			// Token: 0x06006516 RID: 25878
			void SetOnDragleave(object v);

			// Token: 0x06006517 RID: 25879
			object GetOnDragLeave();

			// Token: 0x06006518 RID: 25880
			void SetOnDrop(object v);

			// Token: 0x06006519 RID: 25881
			object GetOnDrop();

			// Token: 0x0600651A RID: 25882
			void SetOnBeforeCut(object v);

			// Token: 0x0600651B RID: 25883
			object GetOnBeforeCut();

			// Token: 0x0600651C RID: 25884
			void SetOnCut(object v);

			// Token: 0x0600651D RID: 25885
			object GetOnCut();

			// Token: 0x0600651E RID: 25886
			void SetOnBeforeCopy(object v);

			// Token: 0x0600651F RID: 25887
			object GetOnBeforeCopy();

			// Token: 0x06006520 RID: 25888
			void SetOnCopy(object v);

			// Token: 0x06006521 RID: 25889
			object GetOnCopy(object p);

			// Token: 0x06006522 RID: 25890
			void SetOnBeforePaste(object v);

			// Token: 0x06006523 RID: 25891
			object GetOnBeforePaste(object p);

			// Token: 0x06006524 RID: 25892
			void SetOnPaste(object v);

			// Token: 0x06006525 RID: 25893
			object GetOnPaste(object p);

			// Token: 0x06006526 RID: 25894
			object GetCurrentStyle();

			// Token: 0x06006527 RID: 25895
			void SetOnPropertyChange(object v);

			// Token: 0x06006528 RID: 25896
			object GetOnPropertyChange(object p);

			// Token: 0x06006529 RID: 25897
			object GetClientRects();

			// Token: 0x0600652A RID: 25898
			object GetBoundingClientRect();

			// Token: 0x0600652B RID: 25899
			void SetExpression(string propName, string expression, string language);

			// Token: 0x0600652C RID: 25900
			object GetExpression(string propName);

			// Token: 0x0600652D RID: 25901
			bool RemoveExpression(string propName);

			// Token: 0x0600652E RID: 25902
			void SetTabIndex(int v);

			// Token: 0x0600652F RID: 25903
			short GetTabIndex();

			// Token: 0x06006530 RID: 25904
			void Focus();

			// Token: 0x06006531 RID: 25905
			void SetAccessKey(string v);

			// Token: 0x06006532 RID: 25906
			string GetAccessKey();

			// Token: 0x06006533 RID: 25907
			void SetOnBlur(object v);

			// Token: 0x06006534 RID: 25908
			object GetOnBlur();

			// Token: 0x06006535 RID: 25909
			void SetOnFocus(object v);

			// Token: 0x06006536 RID: 25910
			object GetOnFocus();

			// Token: 0x06006537 RID: 25911
			void SetOnResize(object v);

			// Token: 0x06006538 RID: 25912
			object GetOnResize();

			// Token: 0x06006539 RID: 25913
			void Blur();

			// Token: 0x0600653A RID: 25914
			void AddFilter(object pUnk);

			// Token: 0x0600653B RID: 25915
			void RemoveFilter(object pUnk);

			// Token: 0x0600653C RID: 25916
			int ClientHeight();

			// Token: 0x0600653D RID: 25917
			int ClientWidth();

			// Token: 0x0600653E RID: 25918
			int ClientTop();

			// Token: 0x0600653F RID: 25919
			int ClientLeft();

			// Token: 0x06006540 RID: 25920
			bool AttachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006541 RID: 25921
			void DetachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06006542 RID: 25922
			object ReadyState();

			// Token: 0x06006543 RID: 25923
			void SetOnReadyStateChange(object v);

			// Token: 0x06006544 RID: 25924
			object GetOnReadyStateChange();

			// Token: 0x06006545 RID: 25925
			void SetOnRowsDelete(object v);

			// Token: 0x06006546 RID: 25926
			object GetOnRowsDelete();

			// Token: 0x06006547 RID: 25927
			void SetOnRowsInserted(object v);

			// Token: 0x06006548 RID: 25928
			object GetOnRowsInserted();

			// Token: 0x06006549 RID: 25929
			void SetOnCellChange(object v);

			// Token: 0x0600654A RID: 25930
			object GetOnCellChange();

			// Token: 0x0600654B RID: 25931
			void SetDir(string v);

			// Token: 0x0600654C RID: 25932
			string GetDir();

			// Token: 0x0600654D RID: 25933
			object CreateControlRange();

			// Token: 0x0600654E RID: 25934
			int GetScrollHeight();

			// Token: 0x0600654F RID: 25935
			int GetScrollWidth();

			// Token: 0x06006550 RID: 25936
			void SetScrollTop(int v);

			// Token: 0x06006551 RID: 25937
			int GetScrollTop();

			// Token: 0x06006552 RID: 25938
			void SetScrollLeft(int v);

			// Token: 0x06006553 RID: 25939
			int GetScrollLeft();

			// Token: 0x06006554 RID: 25940
			void ClearAttributes();

			// Token: 0x06006555 RID: 25941
			void MergeAttributes(object mergeThis);

			// Token: 0x06006556 RID: 25942
			void SetOnContextMenu(object v);

			// Token: 0x06006557 RID: 25943
			object GetOnContextMenu();

			// Token: 0x06006558 RID: 25944
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement InsertAdjacentElement(string where, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IHTMLElement insertedElement);

			// Token: 0x06006559 RID: 25945
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement applyElement([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IHTMLElement apply, string where);

			// Token: 0x0600655A RID: 25946
			string GetAdjacentText(string where);

			// Token: 0x0600655B RID: 25947
			string ReplaceAdjacentText(string where, string newText);

			// Token: 0x0600655C RID: 25948
			bool CanHaveChildren();

			// Token: 0x0600655D RID: 25949
			int AddBehavior(string url, ref object oFactory);

			// Token: 0x0600655E RID: 25950
			bool RemoveBehavior(int cookie);

			// Token: 0x0600655F RID: 25951
			object GetRuntimeStyle();

			// Token: 0x06006560 RID: 25952
			object GetBehaviorUrns();

			// Token: 0x06006561 RID: 25953
			void SetTagUrn(string v);

			// Token: 0x06006562 RID: 25954
			string GetTagUrn();

			// Token: 0x06006563 RID: 25955
			void SetOnBeforeEditFocus(object v);

			// Token: 0x06006564 RID: 25956
			object GetOnBeforeEditFocus();

			// Token: 0x06006565 RID: 25957
			int GetReadyStateValue();

			// Token: 0x06006566 RID: 25958
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByTagName(string v);
		}

		// Token: 0x02000785 RID: 1925
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f673-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLElement3
		{
			// Token: 0x06006567 RID: 25959
			void MergeAttributes(object mergeThis, object pvarFlags);

			// Token: 0x06006568 RID: 25960
			bool IsMultiLine();

			// Token: 0x06006569 RID: 25961
			bool CanHaveHTML();

			// Token: 0x0600656A RID: 25962
			void SetOnLayoutComplete(object v);

			// Token: 0x0600656B RID: 25963
			object GetOnLayoutComplete();

			// Token: 0x0600656C RID: 25964
			void SetOnPage(object v);

			// Token: 0x0600656D RID: 25965
			object GetOnPage();

			// Token: 0x0600656E RID: 25966
			void SetInflateBlock(bool v);

			// Token: 0x0600656F RID: 25967
			bool GetInflateBlock();

			// Token: 0x06006570 RID: 25968
			void SetOnBeforeDeactivate(object v);

			// Token: 0x06006571 RID: 25969
			object GetOnBeforeDeactivate();

			// Token: 0x06006572 RID: 25970
			void SetActive();

			// Token: 0x06006573 RID: 25971
			void SetContentEditable(string v);

			// Token: 0x06006574 RID: 25972
			string GetContentEditable();

			// Token: 0x06006575 RID: 25973
			bool IsContentEditable();

			// Token: 0x06006576 RID: 25974
			void SetHideFocus(bool v);

			// Token: 0x06006577 RID: 25975
			bool GetHideFocus();

			// Token: 0x06006578 RID: 25976
			void SetDisabled(bool v);

			// Token: 0x06006579 RID: 25977
			bool GetDisabled();

			// Token: 0x0600657A RID: 25978
			bool IsDisabled();

			// Token: 0x0600657B RID: 25979
			void SetOnMove(object v);

			// Token: 0x0600657C RID: 25980
			object GetOnMove();

			// Token: 0x0600657D RID: 25981
			void SetOnControlSelect(object v);

			// Token: 0x0600657E RID: 25982
			object GetOnControlSelect();

			// Token: 0x0600657F RID: 25983
			bool FireEvent(string bstrEventName, IntPtr pvarEventObject);

			// Token: 0x06006580 RID: 25984
			void SetOnResizeStart(object v);

			// Token: 0x06006581 RID: 25985
			object GetOnResizeStart();

			// Token: 0x06006582 RID: 25986
			void SetOnResizeEnd(object v);

			// Token: 0x06006583 RID: 25987
			object GetOnResizeEnd();

			// Token: 0x06006584 RID: 25988
			void SetOnMoveStart(object v);

			// Token: 0x06006585 RID: 25989
			object GetOnMoveStart();

			// Token: 0x06006586 RID: 25990
			void SetOnMoveEnd(object v);

			// Token: 0x06006587 RID: 25991
			object GetOnMoveEnd();

			// Token: 0x06006588 RID: 25992
			void SetOnMouseEnter(object v);

			// Token: 0x06006589 RID: 25993
			object GetOnMouseEnter();

			// Token: 0x0600658A RID: 25994
			void SetOnMouseLeave(object v);

			// Token: 0x0600658B RID: 25995
			object GetOnMouseLeave();

			// Token: 0x0600658C RID: 25996
			void SetOnActivate(object v);

			// Token: 0x0600658D RID: 25997
			object GetOnActivate();

			// Token: 0x0600658E RID: 25998
			void SetOnDeactivate(object v);

			// Token: 0x0600658F RID: 25999
			object GetOnDeactivate();

			// Token: 0x06006590 RID: 26000
			bool DragDrop();

			// Token: 0x06006591 RID: 26001
			int GlyphMode();
		}

		// Token: 0x02000786 RID: 1926
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050f5da-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLDOMNode
		{
			// Token: 0x06006592 RID: 26002
			long GetNodeType();

			// Token: 0x06006593 RID: 26003
			UnsafeNativeMethods.IHTMLDOMNode GetParentNode();

			// Token: 0x06006594 RID: 26004
			bool HasChildNodes();

			// Token: 0x06006595 RID: 26005
			object GetChildNodes();

			// Token: 0x06006596 RID: 26006
			object GetAttributes();

			// Token: 0x06006597 RID: 26007
			UnsafeNativeMethods.IHTMLDOMNode InsertBefore(UnsafeNativeMethods.IHTMLDOMNode newChild, object refChild);

			// Token: 0x06006598 RID: 26008
			UnsafeNativeMethods.IHTMLDOMNode RemoveChild(UnsafeNativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x06006599 RID: 26009
			UnsafeNativeMethods.IHTMLDOMNode ReplaceChild(UnsafeNativeMethods.IHTMLDOMNode newChild, UnsafeNativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x0600659A RID: 26010
			UnsafeNativeMethods.IHTMLDOMNode CloneNode(bool fDeep);

			// Token: 0x0600659B RID: 26011
			UnsafeNativeMethods.IHTMLDOMNode RemoveNode(bool fDeep);

			// Token: 0x0600659C RID: 26012
			UnsafeNativeMethods.IHTMLDOMNode SwapNode(UnsafeNativeMethods.IHTMLDOMNode otherNode);

			// Token: 0x0600659D RID: 26013
			UnsafeNativeMethods.IHTMLDOMNode ReplaceNode(UnsafeNativeMethods.IHTMLDOMNode replacement);

			// Token: 0x0600659E RID: 26014
			UnsafeNativeMethods.IHTMLDOMNode AppendChild(UnsafeNativeMethods.IHTMLDOMNode newChild);

			// Token: 0x0600659F RID: 26015
			string NodeName();

			// Token: 0x060065A0 RID: 26016
			void SetNodeValue(object v);

			// Token: 0x060065A1 RID: 26017
			object GetNodeValue();

			// Token: 0x060065A2 RID: 26018
			UnsafeNativeMethods.IHTMLDOMNode FirstChild();

			// Token: 0x060065A3 RID: 26019
			UnsafeNativeMethods.IHTMLDOMNode LastChild();

			// Token: 0x060065A4 RID: 26020
			UnsafeNativeMethods.IHTMLDOMNode PreviousSibling();

			// Token: 0x060065A5 RID: 26021
			UnsafeNativeMethods.IHTMLDOMNode NextSibling();
		}

		// Token: 0x02000787 RID: 1927
		[Guid("3050f60f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLElementEvents2
		{
			// Token: 0x060065A6 RID: 26022
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065A7 RID: 26023
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065A8 RID: 26024
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065A9 RID: 26025
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AA RID: 26026
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AB RID: 26027
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AC RID: 26028
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AD RID: 26029
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AE RID: 26030
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065AF RID: 26031
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B0 RID: 26032
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B1 RID: 26033
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B2 RID: 26034
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B3 RID: 26035
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B4 RID: 26036
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B5 RID: 26037
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B6 RID: 26038
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B7 RID: 26039
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B8 RID: 26040
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065B9 RID: 26041
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BA RID: 26042
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BB RID: 26043
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BC RID: 26044
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BD RID: 26045
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BE RID: 26046
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065BF RID: 26047
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C0 RID: 26048
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C1 RID: 26049
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C2 RID: 26050
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C3 RID: 26051
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C4 RID: 26052
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C5 RID: 26053
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C6 RID: 26054
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C7 RID: 26055
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C8 RID: 26056
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065C9 RID: 26057
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CA RID: 26058
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CB RID: 26059
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CC RID: 26060
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CD RID: 26061
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CE RID: 26062
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065CF RID: 26063
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D0 RID: 26064
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D1 RID: 26065
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D2 RID: 26066
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D3 RID: 26067
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D4 RID: 26068
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D5 RID: 26069
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D6 RID: 26070
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D7 RID: 26071
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D8 RID: 26072
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065D9 RID: 26073
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DA RID: 26074
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DB RID: 26075
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DC RID: 26076
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DD RID: 26077
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DE RID: 26078
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065DF RID: 26079
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E0 RID: 26080
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E1 RID: 26081
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E2 RID: 26082
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E3 RID: 26083
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000788 RID: 1928
		[Guid("3050f610-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLAnchorEvents2
		{
			// Token: 0x060065E4 RID: 26084
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E5 RID: 26085
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E6 RID: 26086
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E7 RID: 26087
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E8 RID: 26088
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065E9 RID: 26089
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065EA RID: 26090
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065EB RID: 26091
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065EC RID: 26092
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065ED RID: 26093
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065EE RID: 26094
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065EF RID: 26095
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F0 RID: 26096
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F1 RID: 26097
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F2 RID: 26098
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F3 RID: 26099
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F4 RID: 26100
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F5 RID: 26101
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F6 RID: 26102
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F7 RID: 26103
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F8 RID: 26104
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065F9 RID: 26105
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FA RID: 26106
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FB RID: 26107
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FC RID: 26108
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FD RID: 26109
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FE RID: 26110
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060065FF RID: 26111
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006600 RID: 26112
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006601 RID: 26113
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006602 RID: 26114
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006603 RID: 26115
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006604 RID: 26116
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006605 RID: 26117
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006606 RID: 26118
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006607 RID: 26119
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006608 RID: 26120
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006609 RID: 26121
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660A RID: 26122
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660B RID: 26123
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660C RID: 26124
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660D RID: 26125
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660E RID: 26126
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600660F RID: 26127
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006610 RID: 26128
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006611 RID: 26129
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006612 RID: 26130
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006613 RID: 26131
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006614 RID: 26132
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006615 RID: 26133
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006616 RID: 26134
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006617 RID: 26135
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006618 RID: 26136
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006619 RID: 26137
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661A RID: 26138
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661B RID: 26139
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661C RID: 26140
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661D RID: 26141
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661E RID: 26142
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600661F RID: 26143
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006620 RID: 26144
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006621 RID: 26145
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000789 RID: 1929
		[Guid("3050f611-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLAreaEvents2
		{
			// Token: 0x06006622 RID: 26146
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006623 RID: 26147
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006624 RID: 26148
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006625 RID: 26149
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006626 RID: 26150
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006627 RID: 26151
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006628 RID: 26152
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006629 RID: 26153
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662A RID: 26154
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662B RID: 26155
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662C RID: 26156
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662D RID: 26157
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662E RID: 26158
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600662F RID: 26159
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006630 RID: 26160
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006631 RID: 26161
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006632 RID: 26162
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006633 RID: 26163
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006634 RID: 26164
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006635 RID: 26165
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006636 RID: 26166
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006637 RID: 26167
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006638 RID: 26168
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006639 RID: 26169
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663A RID: 26170
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663B RID: 26171
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663C RID: 26172
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663D RID: 26173
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663E RID: 26174
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600663F RID: 26175
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006640 RID: 26176
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006641 RID: 26177
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006642 RID: 26178
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006643 RID: 26179
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006644 RID: 26180
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006645 RID: 26181
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006646 RID: 26182
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006647 RID: 26183
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006648 RID: 26184
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006649 RID: 26185
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664A RID: 26186
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664B RID: 26187
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664C RID: 26188
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664D RID: 26189
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664E RID: 26190
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600664F RID: 26191
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006650 RID: 26192
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006651 RID: 26193
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006652 RID: 26194
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006653 RID: 26195
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006654 RID: 26196
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006655 RID: 26197
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006656 RID: 26198
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006657 RID: 26199
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006658 RID: 26200
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006659 RID: 26201
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665A RID: 26202
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665B RID: 26203
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665C RID: 26204
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665D RID: 26205
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665E RID: 26206
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600665F RID: 26207
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078A RID: 1930
		[Guid("3050f617-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLButtonElementEvents2
		{
			// Token: 0x06006660 RID: 26208
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006661 RID: 26209
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006662 RID: 26210
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006663 RID: 26211
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006664 RID: 26212
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006665 RID: 26213
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006666 RID: 26214
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006667 RID: 26215
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006668 RID: 26216
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006669 RID: 26217
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666A RID: 26218
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666B RID: 26219
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666C RID: 26220
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666D RID: 26221
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666E RID: 26222
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600666F RID: 26223
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006670 RID: 26224
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006671 RID: 26225
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006672 RID: 26226
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006673 RID: 26227
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006674 RID: 26228
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006675 RID: 26229
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006676 RID: 26230
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006677 RID: 26231
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006678 RID: 26232
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006679 RID: 26233
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667A RID: 26234
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667B RID: 26235
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667C RID: 26236
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667D RID: 26237
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667E RID: 26238
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600667F RID: 26239
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006680 RID: 26240
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006681 RID: 26241
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006682 RID: 26242
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006683 RID: 26243
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006684 RID: 26244
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006685 RID: 26245
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006686 RID: 26246
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006687 RID: 26247
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006688 RID: 26248
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006689 RID: 26249
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668A RID: 26250
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668B RID: 26251
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668C RID: 26252
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668D RID: 26253
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668E RID: 26254
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600668F RID: 26255
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006690 RID: 26256
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006691 RID: 26257
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006692 RID: 26258
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006693 RID: 26259
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006694 RID: 26260
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006695 RID: 26261
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006696 RID: 26262
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006697 RID: 26263
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006698 RID: 26264
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006699 RID: 26265
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600669A RID: 26266
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600669B RID: 26267
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600669C RID: 26268
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600669D RID: 26269
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078B RID: 1931
		[Guid("3050f612-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLControlElementEvents2
		{
			// Token: 0x0600669E RID: 26270
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600669F RID: 26271
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A0 RID: 26272
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A1 RID: 26273
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A2 RID: 26274
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A3 RID: 26275
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A4 RID: 26276
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A5 RID: 26277
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A6 RID: 26278
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A7 RID: 26279
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A8 RID: 26280
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066A9 RID: 26281
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AA RID: 26282
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AB RID: 26283
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AC RID: 26284
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AD RID: 26285
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AE RID: 26286
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066AF RID: 26287
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B0 RID: 26288
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B1 RID: 26289
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B2 RID: 26290
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B3 RID: 26291
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B4 RID: 26292
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B5 RID: 26293
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B6 RID: 26294
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B7 RID: 26295
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B8 RID: 26296
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066B9 RID: 26297
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BA RID: 26298
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BB RID: 26299
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BC RID: 26300
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BD RID: 26301
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BE RID: 26302
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066BF RID: 26303
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C0 RID: 26304
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C1 RID: 26305
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C2 RID: 26306
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C3 RID: 26307
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C4 RID: 26308
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C5 RID: 26309
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C6 RID: 26310
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C7 RID: 26311
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C8 RID: 26312
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066C9 RID: 26313
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CA RID: 26314
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CB RID: 26315
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CC RID: 26316
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CD RID: 26317
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CE RID: 26318
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066CF RID: 26319
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D0 RID: 26320
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D1 RID: 26321
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D2 RID: 26322
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D3 RID: 26323
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D4 RID: 26324
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D5 RID: 26325
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D6 RID: 26326
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D7 RID: 26327
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D8 RID: 26328
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066D9 RID: 26329
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066DA RID: 26330
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066DB RID: 26331
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078C RID: 1932
		[Guid("3050f614-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLFormElementEvents2
		{
			// Token: 0x060066DC RID: 26332
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066DD RID: 26333
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066DE RID: 26334
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066DF RID: 26335
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E0 RID: 26336
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E1 RID: 26337
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E2 RID: 26338
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E3 RID: 26339
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E4 RID: 26340
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E5 RID: 26341
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E6 RID: 26342
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E7 RID: 26343
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E8 RID: 26344
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066E9 RID: 26345
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066EA RID: 26346
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066EB RID: 26347
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066EC RID: 26348
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066ED RID: 26349
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066EE RID: 26350
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066EF RID: 26351
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F0 RID: 26352
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F1 RID: 26353
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F2 RID: 26354
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F3 RID: 26355
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F4 RID: 26356
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F5 RID: 26357
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F6 RID: 26358
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F7 RID: 26359
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F8 RID: 26360
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066F9 RID: 26361
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FA RID: 26362
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FB RID: 26363
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FC RID: 26364
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FD RID: 26365
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FE RID: 26366
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060066FF RID: 26367
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006700 RID: 26368
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006701 RID: 26369
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006702 RID: 26370
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006703 RID: 26371
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006704 RID: 26372
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006705 RID: 26373
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006706 RID: 26374
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006707 RID: 26375
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006708 RID: 26376
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006709 RID: 26377
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670A RID: 26378
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670B RID: 26379
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670C RID: 26380
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670D RID: 26381
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670E RID: 26382
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600670F RID: 26383
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006710 RID: 26384
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006711 RID: 26385
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006712 RID: 26386
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006713 RID: 26387
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006714 RID: 26388
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006715 RID: 26389
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006716 RID: 26390
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006717 RID: 26391
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006718 RID: 26392
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006719 RID: 26393
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600671A RID: 26394
			[DispId(1007)]
			bool onsubmit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600671B RID: 26395
			[DispId(1015)]
			bool onreset(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078D RID: 1933
		[Guid("3050f7ff-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLFrameSiteEvents2
		{
			// Token: 0x0600671C RID: 26396
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600671D RID: 26397
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600671E RID: 26398
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600671F RID: 26399
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006720 RID: 26400
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006721 RID: 26401
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006722 RID: 26402
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006723 RID: 26403
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006724 RID: 26404
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006725 RID: 26405
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006726 RID: 26406
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006727 RID: 26407
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006728 RID: 26408
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006729 RID: 26409
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672A RID: 26410
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672B RID: 26411
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672C RID: 26412
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672D RID: 26413
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672E RID: 26414
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600672F RID: 26415
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006730 RID: 26416
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006731 RID: 26417
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006732 RID: 26418
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006733 RID: 26419
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006734 RID: 26420
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006735 RID: 26421
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006736 RID: 26422
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006737 RID: 26423
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006738 RID: 26424
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006739 RID: 26425
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673A RID: 26426
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673B RID: 26427
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673C RID: 26428
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673D RID: 26429
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673E RID: 26430
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600673F RID: 26431
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006740 RID: 26432
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006741 RID: 26433
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006742 RID: 26434
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006743 RID: 26435
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006744 RID: 26436
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006745 RID: 26437
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006746 RID: 26438
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006747 RID: 26439
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006748 RID: 26440
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006749 RID: 26441
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674A RID: 26442
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674B RID: 26443
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674C RID: 26444
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674D RID: 26445
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674E RID: 26446
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600674F RID: 26447
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006750 RID: 26448
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006751 RID: 26449
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006752 RID: 26450
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006753 RID: 26451
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006754 RID: 26452
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006755 RID: 26453
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006756 RID: 26454
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006757 RID: 26455
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006758 RID: 26456
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006759 RID: 26457
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600675A RID: 26458
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078E RID: 1934
		[Guid("3050f616-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLImgEvents2
		{
			// Token: 0x0600675B RID: 26459
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600675C RID: 26460
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600675D RID: 26461
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600675E RID: 26462
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600675F RID: 26463
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006760 RID: 26464
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006761 RID: 26465
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006762 RID: 26466
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006763 RID: 26467
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006764 RID: 26468
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006765 RID: 26469
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006766 RID: 26470
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006767 RID: 26471
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006768 RID: 26472
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006769 RID: 26473
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676A RID: 26474
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676B RID: 26475
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676C RID: 26476
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676D RID: 26477
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676E RID: 26478
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600676F RID: 26479
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006770 RID: 26480
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006771 RID: 26481
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006772 RID: 26482
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006773 RID: 26483
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006774 RID: 26484
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006775 RID: 26485
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006776 RID: 26486
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006777 RID: 26487
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006778 RID: 26488
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006779 RID: 26489
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677A RID: 26490
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677B RID: 26491
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677C RID: 26492
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677D RID: 26493
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677E RID: 26494
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600677F RID: 26495
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006780 RID: 26496
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006781 RID: 26497
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006782 RID: 26498
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006783 RID: 26499
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006784 RID: 26500
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006785 RID: 26501
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006786 RID: 26502
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006787 RID: 26503
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006788 RID: 26504
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006789 RID: 26505
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678A RID: 26506
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678B RID: 26507
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678C RID: 26508
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678D RID: 26509
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678E RID: 26510
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600678F RID: 26511
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006790 RID: 26512
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006791 RID: 26513
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006792 RID: 26514
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006793 RID: 26515
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006794 RID: 26516
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006795 RID: 26517
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006796 RID: 26518
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006797 RID: 26519
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006798 RID: 26520
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006799 RID: 26521
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600679A RID: 26522
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600679B RID: 26523
			[DispId(1000)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200078F RID: 1935
		[Guid("3050f61a-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLInputFileElementEvents2
		{
			// Token: 0x0600679C RID: 26524
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600679D RID: 26525
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600679E RID: 26526
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600679F RID: 26527
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A0 RID: 26528
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A1 RID: 26529
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A2 RID: 26530
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A3 RID: 26531
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A4 RID: 26532
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A5 RID: 26533
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A6 RID: 26534
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A7 RID: 26535
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A8 RID: 26536
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067A9 RID: 26537
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AA RID: 26538
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AB RID: 26539
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AC RID: 26540
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AD RID: 26541
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AE RID: 26542
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067AF RID: 26543
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B0 RID: 26544
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B1 RID: 26545
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B2 RID: 26546
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B3 RID: 26547
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B4 RID: 26548
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B5 RID: 26549
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B6 RID: 26550
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B7 RID: 26551
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B8 RID: 26552
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067B9 RID: 26553
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BA RID: 26554
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BB RID: 26555
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BC RID: 26556
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BD RID: 26557
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BE RID: 26558
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067BF RID: 26559
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C0 RID: 26560
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C1 RID: 26561
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C2 RID: 26562
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C3 RID: 26563
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C4 RID: 26564
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C5 RID: 26565
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C6 RID: 26566
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C7 RID: 26567
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C8 RID: 26568
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067C9 RID: 26569
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CA RID: 26570
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CB RID: 26571
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CC RID: 26572
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CD RID: 26573
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CE RID: 26574
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067CF RID: 26575
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D0 RID: 26576
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D1 RID: 26577
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D2 RID: 26578
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D3 RID: 26579
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D4 RID: 26580
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D5 RID: 26581
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D6 RID: 26582
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D7 RID: 26583
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D8 RID: 26584
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067D9 RID: 26585
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067DA RID: 26586
			[DispId(-2147412082)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067DB RID: 26587
			[DispId(-2147412102)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067DC RID: 26588
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067DD RID: 26589
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067DE RID: 26590
			[DispId(1000)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000790 RID: 1936
		[Guid("3050f61b-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLInputImageEvents2
		{
			// Token: 0x060067DF RID: 26591
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E0 RID: 26592
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E1 RID: 26593
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E2 RID: 26594
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E3 RID: 26595
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E4 RID: 26596
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E5 RID: 26597
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E6 RID: 26598
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E7 RID: 26599
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E8 RID: 26600
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067E9 RID: 26601
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067EA RID: 26602
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067EB RID: 26603
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067EC RID: 26604
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067ED RID: 26605
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067EE RID: 26606
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067EF RID: 26607
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F0 RID: 26608
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F1 RID: 26609
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F2 RID: 26610
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F3 RID: 26611
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F4 RID: 26612
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F5 RID: 26613
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F6 RID: 26614
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F7 RID: 26615
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F8 RID: 26616
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067F9 RID: 26617
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FA RID: 26618
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FB RID: 26619
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FC RID: 26620
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FD RID: 26621
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FE RID: 26622
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060067FF RID: 26623
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006800 RID: 26624
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006801 RID: 26625
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006802 RID: 26626
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006803 RID: 26627
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006804 RID: 26628
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006805 RID: 26629
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006806 RID: 26630
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006807 RID: 26631
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006808 RID: 26632
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006809 RID: 26633
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680A RID: 26634
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680B RID: 26635
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680C RID: 26636
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680D RID: 26637
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680E RID: 26638
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600680F RID: 26639
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006810 RID: 26640
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006811 RID: 26641
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006812 RID: 26642
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006813 RID: 26643
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006814 RID: 26644
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006815 RID: 26645
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006816 RID: 26646
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006817 RID: 26647
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006818 RID: 26648
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006819 RID: 26649
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681A RID: 26650
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681B RID: 26651
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681C RID: 26652
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681D RID: 26653
			[DispId(-2147412080)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681E RID: 26654
			[DispId(-2147412083)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600681F RID: 26655
			[DispId(-2147412084)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000791 RID: 1937
		[Guid("3050f618-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLInputTextElementEvents2
		{
			// Token: 0x06006820 RID: 26656
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006821 RID: 26657
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006822 RID: 26658
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006823 RID: 26659
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006824 RID: 26660
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006825 RID: 26661
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006826 RID: 26662
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006827 RID: 26663
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006828 RID: 26664
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006829 RID: 26665
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682A RID: 26666
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682B RID: 26667
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682C RID: 26668
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682D RID: 26669
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682E RID: 26670
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600682F RID: 26671
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006830 RID: 26672
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006831 RID: 26673
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006832 RID: 26674
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006833 RID: 26675
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006834 RID: 26676
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006835 RID: 26677
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006836 RID: 26678
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006837 RID: 26679
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006838 RID: 26680
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006839 RID: 26681
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683A RID: 26682
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683B RID: 26683
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683C RID: 26684
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683D RID: 26685
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683E RID: 26686
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600683F RID: 26687
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006840 RID: 26688
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006841 RID: 26689
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006842 RID: 26690
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006843 RID: 26691
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006844 RID: 26692
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006845 RID: 26693
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006846 RID: 26694
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006847 RID: 26695
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006848 RID: 26696
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006849 RID: 26697
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684A RID: 26698
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684B RID: 26699
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684C RID: 26700
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684D RID: 26701
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684E RID: 26702
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600684F RID: 26703
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006850 RID: 26704
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006851 RID: 26705
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006852 RID: 26706
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006853 RID: 26707
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006854 RID: 26708
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006855 RID: 26709
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006856 RID: 26710
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006857 RID: 26711
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006858 RID: 26712
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006859 RID: 26713
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685A RID: 26714
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685B RID: 26715
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685C RID: 26716
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685D RID: 26717
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685E RID: 26718
			[DispId(1001)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600685F RID: 26719
			[DispId(1006)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006860 RID: 26720
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006861 RID: 26721
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006862 RID: 26722
			[DispId(1001)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000792 RID: 1938
		[Guid("3050f61c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLLabelEvents2
		{
			// Token: 0x06006863 RID: 26723
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006864 RID: 26724
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006865 RID: 26725
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006866 RID: 26726
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006867 RID: 26727
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006868 RID: 26728
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006869 RID: 26729
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686A RID: 26730
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686B RID: 26731
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686C RID: 26732
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686D RID: 26733
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686E RID: 26734
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600686F RID: 26735
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006870 RID: 26736
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006871 RID: 26737
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006872 RID: 26738
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006873 RID: 26739
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006874 RID: 26740
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006875 RID: 26741
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006876 RID: 26742
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006877 RID: 26743
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006878 RID: 26744
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006879 RID: 26745
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687A RID: 26746
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687B RID: 26747
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687C RID: 26748
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687D RID: 26749
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687E RID: 26750
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600687F RID: 26751
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006880 RID: 26752
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006881 RID: 26753
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006882 RID: 26754
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006883 RID: 26755
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006884 RID: 26756
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006885 RID: 26757
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006886 RID: 26758
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006887 RID: 26759
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006888 RID: 26760
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006889 RID: 26761
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688A RID: 26762
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688B RID: 26763
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688C RID: 26764
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688D RID: 26765
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688E RID: 26766
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600688F RID: 26767
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006890 RID: 26768
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006891 RID: 26769
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006892 RID: 26770
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006893 RID: 26771
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006894 RID: 26772
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006895 RID: 26773
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006896 RID: 26774
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006897 RID: 26775
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006898 RID: 26776
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006899 RID: 26777
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689A RID: 26778
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689B RID: 26779
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689C RID: 26780
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689D RID: 26781
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689E RID: 26782
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600689F RID: 26783
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A0 RID: 26784
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000793 RID: 1939
		[Guid("3050f61d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLLinkElementEvents2
		{
			// Token: 0x060068A1 RID: 26785
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A2 RID: 26786
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A3 RID: 26787
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A4 RID: 26788
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A5 RID: 26789
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A6 RID: 26790
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A7 RID: 26791
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A8 RID: 26792
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068A9 RID: 26793
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AA RID: 26794
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AB RID: 26795
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AC RID: 26796
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AD RID: 26797
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AE RID: 26798
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068AF RID: 26799
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B0 RID: 26800
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B1 RID: 26801
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B2 RID: 26802
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B3 RID: 26803
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B4 RID: 26804
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B5 RID: 26805
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B6 RID: 26806
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B7 RID: 26807
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B8 RID: 26808
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068B9 RID: 26809
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BA RID: 26810
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BB RID: 26811
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BC RID: 26812
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BD RID: 26813
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BE RID: 26814
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068BF RID: 26815
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C0 RID: 26816
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C1 RID: 26817
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C2 RID: 26818
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C3 RID: 26819
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C4 RID: 26820
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C5 RID: 26821
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C6 RID: 26822
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C7 RID: 26823
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C8 RID: 26824
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068C9 RID: 26825
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CA RID: 26826
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CB RID: 26827
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CC RID: 26828
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CD RID: 26829
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CE RID: 26830
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068CF RID: 26831
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D0 RID: 26832
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D1 RID: 26833
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D2 RID: 26834
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D3 RID: 26835
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D4 RID: 26836
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D5 RID: 26837
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D6 RID: 26838
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D7 RID: 26839
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D8 RID: 26840
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068D9 RID: 26841
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DA RID: 26842
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DB RID: 26843
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DC RID: 26844
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DD RID: 26845
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DE RID: 26846
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068DF RID: 26847
			[DispId(-2147412080)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E0 RID: 26848
			[DispId(-2147412083)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000794 RID: 1940
		[Guid("3050f61e-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLMapEvents2
		{
			// Token: 0x060068E1 RID: 26849
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E2 RID: 26850
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E3 RID: 26851
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E4 RID: 26852
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E5 RID: 26853
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E6 RID: 26854
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E7 RID: 26855
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E8 RID: 26856
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068E9 RID: 26857
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068EA RID: 26858
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068EB RID: 26859
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068EC RID: 26860
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068ED RID: 26861
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068EE RID: 26862
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068EF RID: 26863
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F0 RID: 26864
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F1 RID: 26865
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F2 RID: 26866
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F3 RID: 26867
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F4 RID: 26868
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F5 RID: 26869
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F6 RID: 26870
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F7 RID: 26871
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F8 RID: 26872
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068F9 RID: 26873
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FA RID: 26874
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FB RID: 26875
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FC RID: 26876
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FD RID: 26877
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FE RID: 26878
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060068FF RID: 26879
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006900 RID: 26880
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006901 RID: 26881
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006902 RID: 26882
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006903 RID: 26883
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006904 RID: 26884
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006905 RID: 26885
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006906 RID: 26886
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006907 RID: 26887
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006908 RID: 26888
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006909 RID: 26889
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690A RID: 26890
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690B RID: 26891
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690C RID: 26892
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690D RID: 26893
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690E RID: 26894
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600690F RID: 26895
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006910 RID: 26896
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006911 RID: 26897
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006912 RID: 26898
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006913 RID: 26899
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006914 RID: 26900
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006915 RID: 26901
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006916 RID: 26902
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006917 RID: 26903
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006918 RID: 26904
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006919 RID: 26905
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600691A RID: 26906
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600691B RID: 26907
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600691C RID: 26908
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600691D RID: 26909
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600691E RID: 26910
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000795 RID: 1941
		[Guid("3050f61f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLMarqueeElementEvents2
		{
			// Token: 0x0600691F RID: 26911
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006920 RID: 26912
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006921 RID: 26913
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006922 RID: 26914
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006923 RID: 26915
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006924 RID: 26916
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006925 RID: 26917
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006926 RID: 26918
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006927 RID: 26919
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006928 RID: 26920
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006929 RID: 26921
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692A RID: 26922
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692B RID: 26923
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692C RID: 26924
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692D RID: 26925
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692E RID: 26926
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600692F RID: 26927
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006930 RID: 26928
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006931 RID: 26929
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006932 RID: 26930
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006933 RID: 26931
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006934 RID: 26932
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006935 RID: 26933
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006936 RID: 26934
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006937 RID: 26935
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006938 RID: 26936
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006939 RID: 26937
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693A RID: 26938
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693B RID: 26939
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693C RID: 26940
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693D RID: 26941
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693E RID: 26942
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600693F RID: 26943
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006940 RID: 26944
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006941 RID: 26945
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006942 RID: 26946
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006943 RID: 26947
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006944 RID: 26948
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006945 RID: 26949
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006946 RID: 26950
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006947 RID: 26951
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006948 RID: 26952
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006949 RID: 26953
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694A RID: 26954
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694B RID: 26955
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694C RID: 26956
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694D RID: 26957
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694E RID: 26958
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600694F RID: 26959
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006950 RID: 26960
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006951 RID: 26961
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006952 RID: 26962
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006953 RID: 26963
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006954 RID: 26964
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006955 RID: 26965
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006956 RID: 26966
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006957 RID: 26967
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006958 RID: 26968
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006959 RID: 26969
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695A RID: 26970
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695B RID: 26971
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695C RID: 26972
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695D RID: 26973
			[DispId(-2147412092)]
			void onbounce(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695E RID: 26974
			[DispId(-2147412086)]
			void onfinish(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600695F RID: 26975
			[DispId(-2147412085)]
			void onstart(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000796 RID: 1942
		[Guid("3050f619-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLOptionButtonElementEvents2
		{
			// Token: 0x06006960 RID: 26976
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006961 RID: 26977
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006962 RID: 26978
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006963 RID: 26979
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006964 RID: 26980
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006965 RID: 26981
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006966 RID: 26982
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006967 RID: 26983
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006968 RID: 26984
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006969 RID: 26985
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696A RID: 26986
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696B RID: 26987
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696C RID: 26988
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696D RID: 26989
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696E RID: 26990
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600696F RID: 26991
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006970 RID: 26992
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006971 RID: 26993
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006972 RID: 26994
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006973 RID: 26995
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006974 RID: 26996
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006975 RID: 26997
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006976 RID: 26998
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006977 RID: 26999
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006978 RID: 27000
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006979 RID: 27001
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697A RID: 27002
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697B RID: 27003
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697C RID: 27004
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697D RID: 27005
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697E RID: 27006
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600697F RID: 27007
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006980 RID: 27008
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006981 RID: 27009
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006982 RID: 27010
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006983 RID: 27011
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006984 RID: 27012
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006985 RID: 27013
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006986 RID: 27014
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006987 RID: 27015
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006988 RID: 27016
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006989 RID: 27017
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698A RID: 27018
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698B RID: 27019
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698C RID: 27020
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698D RID: 27021
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698E RID: 27022
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600698F RID: 27023
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006990 RID: 27024
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006991 RID: 27025
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006992 RID: 27026
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006993 RID: 27027
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006994 RID: 27028
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006995 RID: 27029
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006996 RID: 27030
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006997 RID: 27031
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006998 RID: 27032
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006999 RID: 27033
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600699A RID: 27034
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600699B RID: 27035
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600699C RID: 27036
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600699D RID: 27037
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600699E RID: 27038
			[DispId(-2147412082)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000797 RID: 1943
		[Guid("3050f622-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLSelectElementEvents2
		{
			// Token: 0x0600699F RID: 27039
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A0 RID: 27040
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A1 RID: 27041
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A2 RID: 27042
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A3 RID: 27043
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A4 RID: 27044
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A5 RID: 27045
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A6 RID: 27046
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A7 RID: 27047
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A8 RID: 27048
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069A9 RID: 27049
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AA RID: 27050
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AB RID: 27051
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AC RID: 27052
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AD RID: 27053
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AE RID: 27054
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069AF RID: 27055
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B0 RID: 27056
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B1 RID: 27057
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B2 RID: 27058
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B3 RID: 27059
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B4 RID: 27060
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B5 RID: 27061
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B6 RID: 27062
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B7 RID: 27063
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B8 RID: 27064
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069B9 RID: 27065
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BA RID: 27066
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BB RID: 27067
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BC RID: 27068
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BD RID: 27069
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BE RID: 27070
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069BF RID: 27071
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C0 RID: 27072
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C1 RID: 27073
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C2 RID: 27074
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C3 RID: 27075
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C4 RID: 27076
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C5 RID: 27077
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C6 RID: 27078
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C7 RID: 27079
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C8 RID: 27080
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069C9 RID: 27081
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CA RID: 27082
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CB RID: 27083
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CC RID: 27084
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CD RID: 27085
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CE RID: 27086
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069CF RID: 27087
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D0 RID: 27088
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D1 RID: 27089
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D2 RID: 27090
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D3 RID: 27091
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D4 RID: 27092
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D5 RID: 27093
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D6 RID: 27094
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D7 RID: 27095
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D8 RID: 27096
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069D9 RID: 27097
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069DA RID: 27098
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069DB RID: 27099
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069DC RID: 27100
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069DD RID: 27101
			[DispId(-2147412082)]
			void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000798 RID: 1944
		[Guid("3050f615-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLStyleElementEvents2
		{
			// Token: 0x060069DE RID: 27102
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069DF RID: 27103
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E0 RID: 27104
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E1 RID: 27105
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E2 RID: 27106
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E3 RID: 27107
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E4 RID: 27108
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E5 RID: 27109
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E6 RID: 27110
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E7 RID: 27111
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E8 RID: 27112
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069E9 RID: 27113
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069EA RID: 27114
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069EB RID: 27115
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069EC RID: 27116
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069ED RID: 27117
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069EE RID: 27118
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069EF RID: 27119
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F0 RID: 27120
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F1 RID: 27121
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F2 RID: 27122
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F3 RID: 27123
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F4 RID: 27124
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F5 RID: 27125
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F6 RID: 27126
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F7 RID: 27127
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F8 RID: 27128
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069F9 RID: 27129
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FA RID: 27130
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FB RID: 27131
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FC RID: 27132
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FD RID: 27133
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FE RID: 27134
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060069FF RID: 27135
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A00 RID: 27136
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A01 RID: 27137
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A02 RID: 27138
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A03 RID: 27139
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A04 RID: 27140
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A05 RID: 27141
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A06 RID: 27142
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A07 RID: 27143
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A08 RID: 27144
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A09 RID: 27145
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0A RID: 27146
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0B RID: 27147
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0C RID: 27148
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0D RID: 27149
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0E RID: 27150
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A0F RID: 27151
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A10 RID: 27152
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A11 RID: 27153
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A12 RID: 27154
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A13 RID: 27155
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A14 RID: 27156
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A15 RID: 27157
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A16 RID: 27158
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A17 RID: 27159
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A18 RID: 27160
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A19 RID: 27161
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A1A RID: 27162
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A1B RID: 27163
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A1C RID: 27164
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A1D RID: 27165
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000799 RID: 1945
		[Guid("3050f623-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLTableEvents2
		{
			// Token: 0x06006A1E RID: 27166
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A1F RID: 27167
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A20 RID: 27168
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A21 RID: 27169
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A22 RID: 27170
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A23 RID: 27171
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A24 RID: 27172
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A25 RID: 27173
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A26 RID: 27174
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A27 RID: 27175
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A28 RID: 27176
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A29 RID: 27177
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2A RID: 27178
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2B RID: 27179
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2C RID: 27180
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2D RID: 27181
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2E RID: 27182
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A2F RID: 27183
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A30 RID: 27184
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A31 RID: 27185
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A32 RID: 27186
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A33 RID: 27187
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A34 RID: 27188
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A35 RID: 27189
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A36 RID: 27190
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A37 RID: 27191
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A38 RID: 27192
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A39 RID: 27193
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3A RID: 27194
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3B RID: 27195
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3C RID: 27196
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3D RID: 27197
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3E RID: 27198
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A3F RID: 27199
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A40 RID: 27200
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A41 RID: 27201
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A42 RID: 27202
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A43 RID: 27203
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A44 RID: 27204
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A45 RID: 27205
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A46 RID: 27206
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A47 RID: 27207
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A48 RID: 27208
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A49 RID: 27209
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4A RID: 27210
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4B RID: 27211
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4C RID: 27212
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4D RID: 27213
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4E RID: 27214
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A4F RID: 27215
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A50 RID: 27216
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A51 RID: 27217
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A52 RID: 27218
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A53 RID: 27219
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A54 RID: 27220
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A55 RID: 27221
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A56 RID: 27222
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A57 RID: 27223
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A58 RID: 27224
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A59 RID: 27225
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A5A RID: 27226
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A5B RID: 27227
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200079A RID: 1946
		[Guid("3050f624-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLTextContainerEvents2
		{
			// Token: 0x06006A5C RID: 27228
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A5D RID: 27229
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A5E RID: 27230
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A5F RID: 27231
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A60 RID: 27232
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A61 RID: 27233
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A62 RID: 27234
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A63 RID: 27235
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A64 RID: 27236
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A65 RID: 27237
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A66 RID: 27238
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A67 RID: 27239
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A68 RID: 27240
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A69 RID: 27241
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6A RID: 27242
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6B RID: 27243
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6C RID: 27244
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6D RID: 27245
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6E RID: 27246
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A6F RID: 27247
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A70 RID: 27248
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A71 RID: 27249
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A72 RID: 27250
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A73 RID: 27251
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A74 RID: 27252
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A75 RID: 27253
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A76 RID: 27254
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A77 RID: 27255
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A78 RID: 27256
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A79 RID: 27257
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7A RID: 27258
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7B RID: 27259
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7C RID: 27260
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7D RID: 27261
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7E RID: 27262
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A7F RID: 27263
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A80 RID: 27264
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A81 RID: 27265
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A82 RID: 27266
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A83 RID: 27267
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A84 RID: 27268
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A85 RID: 27269
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A86 RID: 27270
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A87 RID: 27271
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A88 RID: 27272
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A89 RID: 27273
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8A RID: 27274
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8B RID: 27275
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8C RID: 27276
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8D RID: 27277
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8E RID: 27278
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A8F RID: 27279
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A90 RID: 27280
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A91 RID: 27281
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A92 RID: 27282
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A93 RID: 27283
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A94 RID: 27284
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A95 RID: 27285
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A96 RID: 27286
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A97 RID: 27287
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A98 RID: 27288
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A99 RID: 27289
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A9A RID: 27290
			[DispId(1001)]
			void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A9B RID: 27291
			[DispId(1006)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200079B RID: 1947
		[Guid("3050f621-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLScriptEvents2
		{
			// Token: 0x06006A9C RID: 27292
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A9D RID: 27293
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A9E RID: 27294
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006A9F RID: 27295
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA0 RID: 27296
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA1 RID: 27297
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA2 RID: 27298
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA3 RID: 27299
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA4 RID: 27300
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA5 RID: 27301
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA6 RID: 27302
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA7 RID: 27303
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA8 RID: 27304
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AA9 RID: 27305
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAA RID: 27306
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAB RID: 27307
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAC RID: 27308
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAD RID: 27309
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAE RID: 27310
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AAF RID: 27311
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB0 RID: 27312
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB1 RID: 27313
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB2 RID: 27314
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB3 RID: 27315
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB4 RID: 27316
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB5 RID: 27317
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB6 RID: 27318
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB7 RID: 27319
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB8 RID: 27320
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AB9 RID: 27321
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABA RID: 27322
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABB RID: 27323
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABC RID: 27324
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABD RID: 27325
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABE RID: 27326
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ABF RID: 27327
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC0 RID: 27328
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC1 RID: 27329
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC2 RID: 27330
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC3 RID: 27331
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC4 RID: 27332
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC5 RID: 27333
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC6 RID: 27334
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC7 RID: 27335
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC8 RID: 27336
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AC9 RID: 27337
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACA RID: 27338
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACB RID: 27339
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACC RID: 27340
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACD RID: 27341
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACE RID: 27342
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ACF RID: 27343
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD0 RID: 27344
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD1 RID: 27345
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD2 RID: 27346
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD3 RID: 27347
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD4 RID: 27348
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD5 RID: 27349
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD6 RID: 27350
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD7 RID: 27351
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD8 RID: 27352
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006AD9 RID: 27353
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06006ADA RID: 27354
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200079C RID: 1948
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLStyle
		{
			// Token: 0x06006ADB RID: 27355
			void SetFontFamily(string p);

			// Token: 0x06006ADC RID: 27356
			string GetFontFamily();

			// Token: 0x06006ADD RID: 27357
			void SetFontStyle(string p);

			// Token: 0x06006ADE RID: 27358
			string GetFontStyle();

			// Token: 0x06006ADF RID: 27359
			void SetFontObject(string p);

			// Token: 0x06006AE0 RID: 27360
			string GetFontObject();

			// Token: 0x06006AE1 RID: 27361
			void SetFontWeight(string p);

			// Token: 0x06006AE2 RID: 27362
			string GetFontWeight();

			// Token: 0x06006AE3 RID: 27363
			void SetFontSize(object p);

			// Token: 0x06006AE4 RID: 27364
			object GetFontSize();

			// Token: 0x06006AE5 RID: 27365
			void SetFont(string p);

			// Token: 0x06006AE6 RID: 27366
			string GetFont();

			// Token: 0x06006AE7 RID: 27367
			void SetColor(object p);

			// Token: 0x06006AE8 RID: 27368
			object GetColor();

			// Token: 0x06006AE9 RID: 27369
			void SetBackground(string p);

			// Token: 0x06006AEA RID: 27370
			string GetBackground();

			// Token: 0x06006AEB RID: 27371
			void SetBackgroundColor(object p);

			// Token: 0x06006AEC RID: 27372
			object GetBackgroundColor();

			// Token: 0x06006AED RID: 27373
			void SetBackgroundImage(string p);

			// Token: 0x06006AEE RID: 27374
			string GetBackgroundImage();

			// Token: 0x06006AEF RID: 27375
			void SetBackgroundRepeat(string p);

			// Token: 0x06006AF0 RID: 27376
			string GetBackgroundRepeat();

			// Token: 0x06006AF1 RID: 27377
			void SetBackgroundAttachment(string p);

			// Token: 0x06006AF2 RID: 27378
			string GetBackgroundAttachment();

			// Token: 0x06006AF3 RID: 27379
			void SetBackgroundPosition(string p);

			// Token: 0x06006AF4 RID: 27380
			string GetBackgroundPosition();

			// Token: 0x06006AF5 RID: 27381
			void SetBackgroundPositionX(object p);

			// Token: 0x06006AF6 RID: 27382
			object GetBackgroundPositionX();

			// Token: 0x06006AF7 RID: 27383
			void SetBackgroundPositionY(object p);

			// Token: 0x06006AF8 RID: 27384
			object GetBackgroundPositionY();

			// Token: 0x06006AF9 RID: 27385
			void SetWordSpacing(object p);

			// Token: 0x06006AFA RID: 27386
			object GetWordSpacing();

			// Token: 0x06006AFB RID: 27387
			void SetLetterSpacing(object p);

			// Token: 0x06006AFC RID: 27388
			object GetLetterSpacing();

			// Token: 0x06006AFD RID: 27389
			void SetTextDecoration(string p);

			// Token: 0x06006AFE RID: 27390
			string GetTextDecoration();

			// Token: 0x06006AFF RID: 27391
			void SetTextDecorationNone(bool p);

			// Token: 0x06006B00 RID: 27392
			bool GetTextDecorationNone();

			// Token: 0x06006B01 RID: 27393
			void SetTextDecorationUnderline(bool p);

			// Token: 0x06006B02 RID: 27394
			bool GetTextDecorationUnderline();

			// Token: 0x06006B03 RID: 27395
			void SetTextDecorationOverline(bool p);

			// Token: 0x06006B04 RID: 27396
			bool GetTextDecorationOverline();

			// Token: 0x06006B05 RID: 27397
			void SetTextDecorationLineThrough(bool p);

			// Token: 0x06006B06 RID: 27398
			bool GetTextDecorationLineThrough();

			// Token: 0x06006B07 RID: 27399
			void SetTextDecorationBlink(bool p);

			// Token: 0x06006B08 RID: 27400
			bool GetTextDecorationBlink();

			// Token: 0x06006B09 RID: 27401
			void SetVerticalAlign(object p);

			// Token: 0x06006B0A RID: 27402
			object GetVerticalAlign();

			// Token: 0x06006B0B RID: 27403
			void SetTextTransform(string p);

			// Token: 0x06006B0C RID: 27404
			string GetTextTransform();

			// Token: 0x06006B0D RID: 27405
			void SetTextAlign(string p);

			// Token: 0x06006B0E RID: 27406
			string GetTextAlign();

			// Token: 0x06006B0F RID: 27407
			void SetTextIndent(object p);

			// Token: 0x06006B10 RID: 27408
			object GetTextIndent();

			// Token: 0x06006B11 RID: 27409
			void SetLineHeight(object p);

			// Token: 0x06006B12 RID: 27410
			object GetLineHeight();

			// Token: 0x06006B13 RID: 27411
			void SetMarginTop(object p);

			// Token: 0x06006B14 RID: 27412
			object GetMarginTop();

			// Token: 0x06006B15 RID: 27413
			void SetMarginRight(object p);

			// Token: 0x06006B16 RID: 27414
			object GetMarginRight();

			// Token: 0x06006B17 RID: 27415
			void SetMarginBottom(object p);

			// Token: 0x06006B18 RID: 27416
			object GetMarginBottom();

			// Token: 0x06006B19 RID: 27417
			void SetMarginLeft(object p);

			// Token: 0x06006B1A RID: 27418
			object GetMarginLeft();

			// Token: 0x06006B1B RID: 27419
			void SetMargin(string p);

			// Token: 0x06006B1C RID: 27420
			string GetMargin();

			// Token: 0x06006B1D RID: 27421
			void SetPaddingTop(object p);

			// Token: 0x06006B1E RID: 27422
			object GetPaddingTop();

			// Token: 0x06006B1F RID: 27423
			void SetPaddingRight(object p);

			// Token: 0x06006B20 RID: 27424
			object GetPaddingRight();

			// Token: 0x06006B21 RID: 27425
			void SetPaddingBottom(object p);

			// Token: 0x06006B22 RID: 27426
			object GetPaddingBottom();

			// Token: 0x06006B23 RID: 27427
			void SetPaddingLeft(object p);

			// Token: 0x06006B24 RID: 27428
			object GetPaddingLeft();

			// Token: 0x06006B25 RID: 27429
			void SetPadding(string p);

			// Token: 0x06006B26 RID: 27430
			string GetPadding();

			// Token: 0x06006B27 RID: 27431
			void SetBorder(string p);

			// Token: 0x06006B28 RID: 27432
			string GetBorder();

			// Token: 0x06006B29 RID: 27433
			void SetBorderTop(string p);

			// Token: 0x06006B2A RID: 27434
			string GetBorderTop();

			// Token: 0x06006B2B RID: 27435
			void SetBorderRight(string p);

			// Token: 0x06006B2C RID: 27436
			string GetBorderRight();

			// Token: 0x06006B2D RID: 27437
			void SetBorderBottom(string p);

			// Token: 0x06006B2E RID: 27438
			string GetBorderBottom();

			// Token: 0x06006B2F RID: 27439
			void SetBorderLeft(string p);

			// Token: 0x06006B30 RID: 27440
			string GetBorderLeft();

			// Token: 0x06006B31 RID: 27441
			void SetBorderColor(string p);

			// Token: 0x06006B32 RID: 27442
			string GetBorderColor();

			// Token: 0x06006B33 RID: 27443
			void SetBorderTopColor(object p);

			// Token: 0x06006B34 RID: 27444
			object GetBorderTopColor();

			// Token: 0x06006B35 RID: 27445
			void SetBorderRightColor(object p);

			// Token: 0x06006B36 RID: 27446
			object GetBorderRightColor();

			// Token: 0x06006B37 RID: 27447
			void SetBorderBottomColor(object p);

			// Token: 0x06006B38 RID: 27448
			object GetBorderBottomColor();

			// Token: 0x06006B39 RID: 27449
			void SetBorderLeftColor(object p);

			// Token: 0x06006B3A RID: 27450
			object GetBorderLeftColor();

			// Token: 0x06006B3B RID: 27451
			void SetBorderWidth(string p);

			// Token: 0x06006B3C RID: 27452
			string GetBorderWidth();

			// Token: 0x06006B3D RID: 27453
			void SetBorderTopWidth(object p);

			// Token: 0x06006B3E RID: 27454
			object GetBorderTopWidth();

			// Token: 0x06006B3F RID: 27455
			void SetBorderRightWidth(object p);

			// Token: 0x06006B40 RID: 27456
			object GetBorderRightWidth();

			// Token: 0x06006B41 RID: 27457
			void SetBorderBottomWidth(object p);

			// Token: 0x06006B42 RID: 27458
			object GetBorderBottomWidth();

			// Token: 0x06006B43 RID: 27459
			void SetBorderLeftWidth(object p);

			// Token: 0x06006B44 RID: 27460
			object GetBorderLeftWidth();

			// Token: 0x06006B45 RID: 27461
			void SetBorderStyle(string p);

			// Token: 0x06006B46 RID: 27462
			string GetBorderStyle();

			// Token: 0x06006B47 RID: 27463
			void SetBorderTopStyle(string p);

			// Token: 0x06006B48 RID: 27464
			string GetBorderTopStyle();

			// Token: 0x06006B49 RID: 27465
			void SetBorderRightStyle(string p);

			// Token: 0x06006B4A RID: 27466
			string GetBorderRightStyle();

			// Token: 0x06006B4B RID: 27467
			void SetBorderBottomStyle(string p);

			// Token: 0x06006B4C RID: 27468
			string GetBorderBottomStyle();

			// Token: 0x06006B4D RID: 27469
			void SetBorderLeftStyle(string p);

			// Token: 0x06006B4E RID: 27470
			string GetBorderLeftStyle();

			// Token: 0x06006B4F RID: 27471
			void SetWidth(object p);

			// Token: 0x06006B50 RID: 27472
			object GetWidth();

			// Token: 0x06006B51 RID: 27473
			void SetHeight(object p);

			// Token: 0x06006B52 RID: 27474
			object GetHeight();

			// Token: 0x06006B53 RID: 27475
			void SetStyleFloat(string p);

			// Token: 0x06006B54 RID: 27476
			string GetStyleFloat();

			// Token: 0x06006B55 RID: 27477
			void SetClear(string p);

			// Token: 0x06006B56 RID: 27478
			string GetClear();

			// Token: 0x06006B57 RID: 27479
			void SetDisplay(string p);

			// Token: 0x06006B58 RID: 27480
			string GetDisplay();

			// Token: 0x06006B59 RID: 27481
			void SetVisibility(string p);

			// Token: 0x06006B5A RID: 27482
			string GetVisibility();

			// Token: 0x06006B5B RID: 27483
			void SetListStyleType(string p);

			// Token: 0x06006B5C RID: 27484
			string GetListStyleType();

			// Token: 0x06006B5D RID: 27485
			void SetListStylePosition(string p);

			// Token: 0x06006B5E RID: 27486
			string GetListStylePosition();

			// Token: 0x06006B5F RID: 27487
			void SetListStyleImage(string p);

			// Token: 0x06006B60 RID: 27488
			string GetListStyleImage();

			// Token: 0x06006B61 RID: 27489
			void SetListStyle(string p);

			// Token: 0x06006B62 RID: 27490
			string GetListStyle();

			// Token: 0x06006B63 RID: 27491
			void SetWhiteSpace(string p);

			// Token: 0x06006B64 RID: 27492
			string GetWhiteSpace();

			// Token: 0x06006B65 RID: 27493
			void SetTop(object p);

			// Token: 0x06006B66 RID: 27494
			object GetTop();

			// Token: 0x06006B67 RID: 27495
			void SetLeft(object p);

			// Token: 0x06006B68 RID: 27496
			object GetLeft();

			// Token: 0x06006B69 RID: 27497
			string GetPosition();

			// Token: 0x06006B6A RID: 27498
			void SetZIndex(object p);

			// Token: 0x06006B6B RID: 27499
			object GetZIndex();

			// Token: 0x06006B6C RID: 27500
			void SetOverflow(string p);

			// Token: 0x06006B6D RID: 27501
			string GetOverflow();

			// Token: 0x06006B6E RID: 27502
			void SetPageBreakBefore(string p);

			// Token: 0x06006B6F RID: 27503
			string GetPageBreakBefore();

			// Token: 0x06006B70 RID: 27504
			void SetPageBreakAfter(string p);

			// Token: 0x06006B71 RID: 27505
			string GetPageBreakAfter();

			// Token: 0x06006B72 RID: 27506
			void SetCssText(string p);

			// Token: 0x06006B73 RID: 27507
			string GetCssText();

			// Token: 0x06006B74 RID: 27508
			void SetPixelTop(int p);

			// Token: 0x06006B75 RID: 27509
			int GetPixelTop();

			// Token: 0x06006B76 RID: 27510
			void SetPixelLeft(int p);

			// Token: 0x06006B77 RID: 27511
			int GetPixelLeft();

			// Token: 0x06006B78 RID: 27512
			void SetPixelWidth(int p);

			// Token: 0x06006B79 RID: 27513
			int GetPixelWidth();

			// Token: 0x06006B7A RID: 27514
			void SetPixelHeight(int p);

			// Token: 0x06006B7B RID: 27515
			int GetPixelHeight();

			// Token: 0x06006B7C RID: 27516
			void SetPosTop(float p);

			// Token: 0x06006B7D RID: 27517
			float GetPosTop();

			// Token: 0x06006B7E RID: 27518
			void SetPosLeft(float p);

			// Token: 0x06006B7F RID: 27519
			float GetPosLeft();

			// Token: 0x06006B80 RID: 27520
			void SetPosWidth(float p);

			// Token: 0x06006B81 RID: 27521
			float GetPosWidth();

			// Token: 0x06006B82 RID: 27522
			void SetPosHeight(float p);

			// Token: 0x06006B83 RID: 27523
			float GetPosHeight();

			// Token: 0x06006B84 RID: 27524
			void SetCursor(string p);

			// Token: 0x06006B85 RID: 27525
			string GetCursor();

			// Token: 0x06006B86 RID: 27526
			void SetClip(string p);

			// Token: 0x06006B87 RID: 27527
			string GetClip();

			// Token: 0x06006B88 RID: 27528
			void SetFilter(string p);

			// Token: 0x06006B89 RID: 27529
			string GetFilter();

			// Token: 0x06006B8A RID: 27530
			void SetAttribute(string strAttributeName, object AttributeValue, int lFlags);

			// Token: 0x06006B8B RID: 27531
			object GetAttribute(string strAttributeName, int lFlags);

			// Token: 0x06006B8C RID: 27532
			bool RemoveAttribute(string strAttributeName, int lFlags);
		}

		// Token: 0x0200079D RID: 1949
		[Guid("39088D7E-B71E-11D1-8F39-00C04FD946D0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IExtender
		{
			// Token: 0x1700179A RID: 6042
			// (get) Token: 0x06006B8D RID: 27533
			// (set) Token: 0x06006B8E RID: 27534
			int Align { get; set; }

			// Token: 0x1700179B RID: 6043
			// (get) Token: 0x06006B8F RID: 27535
			// (set) Token: 0x06006B90 RID: 27536
			bool Enabled { get; set; }

			// Token: 0x1700179C RID: 6044
			// (get) Token: 0x06006B91 RID: 27537
			// (set) Token: 0x06006B92 RID: 27538
			int Height { get; set; }

			// Token: 0x1700179D RID: 6045
			// (get) Token: 0x06006B93 RID: 27539
			// (set) Token: 0x06006B94 RID: 27540
			int Left { get; set; }

			// Token: 0x1700179E RID: 6046
			// (get) Token: 0x06006B95 RID: 27541
			// (set) Token: 0x06006B96 RID: 27542
			bool TabStop { get; set; }

			// Token: 0x1700179F RID: 6047
			// (get) Token: 0x06006B97 RID: 27543
			// (set) Token: 0x06006B98 RID: 27544
			int Top { get; set; }

			// Token: 0x170017A0 RID: 6048
			// (get) Token: 0x06006B99 RID: 27545
			// (set) Token: 0x06006B9A RID: 27546
			bool Visible { get; set; }

			// Token: 0x170017A1 RID: 6049
			// (get) Token: 0x06006B9B RID: 27547
			// (set) Token: 0x06006B9C RID: 27548
			int Width { get; set; }

			// Token: 0x170017A2 RID: 6050
			// (get) Token: 0x06006B9D RID: 27549
			string Name { [return: MarshalAs(UnmanagedType.BStr)] get; }

			// Token: 0x170017A3 RID: 6051
			// (get) Token: 0x06006B9E RID: 27550
			object Parent { [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x170017A4 RID: 6052
			// (get) Token: 0x06006B9F RID: 27551
			IntPtr Hwnd { get; }

			// Token: 0x170017A5 RID: 6053
			// (get) Token: 0x06006BA0 RID: 27552
			object Container { [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x06006BA1 RID: 27553
			void Move([MarshalAs(UnmanagedType.Interface)] [In] object left, [MarshalAs(UnmanagedType.Interface)] [In] object top, [MarshalAs(UnmanagedType.Interface)] [In] object width, [MarshalAs(UnmanagedType.Interface)] [In] object height);
		}

		// Token: 0x0200079E RID: 1950
		[Guid("8A701DA0-4FEB-101B-A82E-08002B2B2337")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IGetOleObject
		{
			// Token: 0x06006BA2 RID: 27554
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetOleObject(ref Guid riid);
		}

		// Token: 0x0200079F RID: 1951
		[Guid("CB2F6722-AB3A-11d2-9C40-00C04FA30A3E")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ICorRuntimeHost
		{
			// Token: 0x06006BA3 RID: 27555
			[PreserveSig]
			int CreateLogicalThreadState();

			// Token: 0x06006BA4 RID: 27556
			[PreserveSig]
			int DeleteLogicalThreadState();

			// Token: 0x06006BA5 RID: 27557
			[PreserveSig]
			int SwitchInLogicalThreadState([In] ref uint pFiberCookie);

			// Token: 0x06006BA6 RID: 27558
			[PreserveSig]
			int SwitchOutLogicalThreadState(out uint FiberCookie);

			// Token: 0x06006BA7 RID: 27559
			[PreserveSig]
			int LocksHeldByLogicalThread(out uint pCount);

			// Token: 0x06006BA8 RID: 27560
			[PreserveSig]
			int MapFile(IntPtr hFile, out IntPtr hMapAddress);

			// Token: 0x06006BA9 RID: 27561
			[PreserveSig]
			int GetConfiguration([MarshalAs(UnmanagedType.IUnknown)] out object pConfiguration);

			// Token: 0x06006BAA RID: 27562
			[PreserveSig]
			int Start();

			// Token: 0x06006BAB RID: 27563
			[PreserveSig]
			int Stop();

			// Token: 0x06006BAC RID: 27564
			[PreserveSig]
			int CreateDomain(string pwzFriendlyName, [MarshalAs(UnmanagedType.IUnknown)] object pIdentityArray, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x06006BAD RID: 27565
			[PreserveSig]
			int GetDefaultDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x06006BAE RID: 27566
			[PreserveSig]
			int EnumDomains(out IntPtr hEnum);

			// Token: 0x06006BAF RID: 27567
			[PreserveSig]
			int NextDomain(IntPtr hEnum, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x06006BB0 RID: 27568
			[PreserveSig]
			int CloseEnum(IntPtr hEnum);

			// Token: 0x06006BB1 RID: 27569
			[PreserveSig]
			int CreateDomainEx(string pwzFriendlyName, [MarshalAs(UnmanagedType.IUnknown)] object pSetup, [MarshalAs(UnmanagedType.IUnknown)] object pEvidence, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x06006BB2 RID: 27570
			[PreserveSig]
			int CreateDomainSetup([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomainSetup);

			// Token: 0x06006BB3 RID: 27571
			[PreserveSig]
			int CreateEvidence([MarshalAs(UnmanagedType.IUnknown)] out object pEvidence);

			// Token: 0x06006BB4 RID: 27572
			[PreserveSig]
			int UnloadDomain([MarshalAs(UnmanagedType.IUnknown)] object pAppDomain);

			// Token: 0x06006BB5 RID: 27573
			[PreserveSig]
			int CurrentDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);
		}

		// Token: 0x020007A0 RID: 1952
		[Guid("CB2F6723-AB3A-11d2-9C40-00C04FA30A3E")]
		[ComImport]
		internal class CorRuntimeHost
		{
			// Token: 0x06006BB6 RID: 27574
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern CorRuntimeHost();
		}

		// Token: 0x020007A1 RID: 1953
		[Guid("000C0601-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IMsoComponentManager
		{
			// Token: 0x06006BB7 RID: 27575
			[PreserveSig]
			int QueryService(ref Guid guidService, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

			// Token: 0x06006BB8 RID: 27576
			[PreserveSig]
			bool FDebugMessage(IntPtr hInst, int msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x06006BB9 RID: 27577
			[PreserveSig]
			bool FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out IntPtr dwComponentID);

			// Token: 0x06006BBA RID: 27578
			[PreserveSig]
			bool FRevokeComponent(IntPtr dwComponentID);

			// Token: 0x06006BBB RID: 27579
			[PreserveSig]
			bool FUpdateComponentRegistration(IntPtr dwComponentID, NativeMethods.MSOCRINFOSTRUCT pcrinfo);

			// Token: 0x06006BBC RID: 27580
			[PreserveSig]
			bool FOnComponentActivate(IntPtr dwComponentID);

			// Token: 0x06006BBD RID: 27581
			[PreserveSig]
			bool FSetTrackingComponent(IntPtr dwComponentID, [MarshalAs(UnmanagedType.Bool)] [In] bool fTrack);

			// Token: 0x06006BBE RID: 27582
			[PreserveSig]
			void OnComponentEnterState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved);

			// Token: 0x06006BBF RID: 27583
			[PreserveSig]
			bool FOnComponentExitState(IntPtr dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude);

			// Token: 0x06006BC0 RID: 27584
			[PreserveSig]
			bool FInState(int uStateID, IntPtr pvoid);

			// Token: 0x06006BC1 RID: 27585
			[PreserveSig]
			bool FContinueIdle();

			// Token: 0x06006BC2 RID: 27586
			[PreserveSig]
			bool FPushMessageLoop(IntPtr dwComponentID, int uReason, int pvLoopData);

			// Token: 0x06006BC3 RID: 27587
			[PreserveSig]
			bool FCreateSubComponentManager([MarshalAs(UnmanagedType.Interface)] object punkOuter, [MarshalAs(UnmanagedType.Interface)] object punkServProv, ref Guid riid, out IntPtr ppvObj);

			// Token: 0x06006BC4 RID: 27588
			[PreserveSig]
			bool FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm);

			// Token: 0x06006BC5 RID: 27589
			[PreserveSig]
			bool FGetActiveComponent(int dwgac, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT pcrinfo, int dwReserved);
		}

		// Token: 0x020007A2 RID: 1954
		[Guid("000C0600-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IMsoComponent
		{
			// Token: 0x06006BC6 RID: 27590
			[PreserveSig]
			bool FDebugMessage(IntPtr hInst, int msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x06006BC7 RID: 27591
			[PreserveSig]
			bool FPreTranslateMessage(ref NativeMethods.MSG msg);

			// Token: 0x06006BC8 RID: 27592
			[PreserveSig]
			void OnEnterState(int uStateID, bool fEnter);

			// Token: 0x06006BC9 RID: 27593
			[PreserveSig]
			void OnAppActivate(bool fActive, int dwOtherThreadID);

			// Token: 0x06006BCA RID: 27594
			[PreserveSig]
			void OnLoseActivation();

			// Token: 0x06006BCB RID: 27595
			[PreserveSig]
			void OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved);

			// Token: 0x06006BCC RID: 27596
			[PreserveSig]
			bool FDoIdle(int grfidlef);

			// Token: 0x06006BCD RID: 27597
			[PreserveSig]
			bool FContinueMessageLoop(int uReason, int pvLoopData, [MarshalAs(UnmanagedType.LPArray)] NativeMethods.MSG[] pMsgPeeked);

			// Token: 0x06006BCE RID: 27598
			[PreserveSig]
			bool FQueryTerminate(bool fPromptUser);

			// Token: 0x06006BCF RID: 27599
			[PreserveSig]
			void Terminate();

			// Token: 0x06006BD0 RID: 27600
			[PreserveSig]
			IntPtr HwndGetWindow(int dwWhich, int dwReserved);
		}

		// Token: 0x020007A3 RID: 1955
		[ComVisible(true)]
		[Guid("8CC497C0-A1DF-11ce-8098-00AA0047BE5D")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		public interface ITextDocument
		{
			// Token: 0x06006BD1 RID: 27601
			string GetName();

			// Token: 0x06006BD2 RID: 27602
			object GetSelection();

			// Token: 0x06006BD3 RID: 27603
			int GetStoryCount();

			// Token: 0x06006BD4 RID: 27604
			object GetStoryRanges();

			// Token: 0x06006BD5 RID: 27605
			int GetSaved();

			// Token: 0x06006BD6 RID: 27606
			void SetSaved(int value);

			// Token: 0x06006BD7 RID: 27607
			object GetDefaultTabStop();

			// Token: 0x06006BD8 RID: 27608
			void SetDefaultTabStop(object value);

			// Token: 0x06006BD9 RID: 27609
			void New();

			// Token: 0x06006BDA RID: 27610
			void Open(object pVar, int flags, int codePage);

			// Token: 0x06006BDB RID: 27611
			void Save(object pVar, int flags, int codePage);

			// Token: 0x06006BDC RID: 27612
			int Freeze();

			// Token: 0x06006BDD RID: 27613
			int Unfreeze();

			// Token: 0x06006BDE RID: 27614
			void BeginEditCollection();

			// Token: 0x06006BDF RID: 27615
			void EndEditCollection();

			// Token: 0x06006BE0 RID: 27616
			int Undo(int count);

			// Token: 0x06006BE1 RID: 27617
			int Redo(int count);

			// Token: 0x06006BE2 RID: 27618
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange Range(int cp1, int cp2);

			// Token: 0x06006BE3 RID: 27619
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange RangeFromPoint(int x, int y);
		}

		// Token: 0x020007A4 RID: 1956
		[ComVisible(true)]
		[Guid("8CC497C2-A1DF-11ce-8098-00AA0047BE5D")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		public interface ITextRange
		{
			// Token: 0x06006BE4 RID: 27620
			string GetText();

			// Token: 0x06006BE5 RID: 27621
			void SetText(string text);

			// Token: 0x06006BE6 RID: 27622
			object GetChar();

			// Token: 0x06006BE7 RID: 27623
			void SetChar(object ch);

			// Token: 0x06006BE8 RID: 27624
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange GetDuplicate();

			// Token: 0x06006BE9 RID: 27625
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange GetFormattedText();

			// Token: 0x06006BEA RID: 27626
			void SetFormattedText([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06006BEB RID: 27627
			int GetStart();

			// Token: 0x06006BEC RID: 27628
			void SetStart(int cpFirst);

			// Token: 0x06006BED RID: 27629
			int GetEnd();

			// Token: 0x06006BEE RID: 27630
			void SetEnd(int cpLim);

			// Token: 0x06006BEF RID: 27631
			object GetFont();

			// Token: 0x06006BF0 RID: 27632
			void SetFont(object font);

			// Token: 0x06006BF1 RID: 27633
			object GetPara();

			// Token: 0x06006BF2 RID: 27634
			void SetPara(object para);

			// Token: 0x06006BF3 RID: 27635
			int GetStoryLength();

			// Token: 0x06006BF4 RID: 27636
			int GetStoryType();

			// Token: 0x06006BF5 RID: 27637
			void Collapse(int start);

			// Token: 0x06006BF6 RID: 27638
			int Expand(int unit);

			// Token: 0x06006BF7 RID: 27639
			int GetIndex(int unit);

			// Token: 0x06006BF8 RID: 27640
			void SetIndex(int unit, int index, int extend);

			// Token: 0x06006BF9 RID: 27641
			void SetRange(int cpActive, int cpOther);

			// Token: 0x06006BFA RID: 27642
			int InRange([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06006BFB RID: 27643
			int InStory([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06006BFC RID: 27644
			int IsEqual([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06006BFD RID: 27645
			void Select();

			// Token: 0x06006BFE RID: 27646
			int StartOf(int unit, int extend);

			// Token: 0x06006BFF RID: 27647
			int EndOf(int unit, int extend);

			// Token: 0x06006C00 RID: 27648
			int Move(int unit, int count);

			// Token: 0x06006C01 RID: 27649
			int MoveStart(int unit, int count);

			// Token: 0x06006C02 RID: 27650
			int MoveEnd(int unit, int count);

			// Token: 0x06006C03 RID: 27651
			int MoveWhile(object cset, int count);

			// Token: 0x06006C04 RID: 27652
			int MoveStartWhile(object cset, int count);

			// Token: 0x06006C05 RID: 27653
			int MoveEndWhile(object cset, int count);

			// Token: 0x06006C06 RID: 27654
			int MoveUntil(object cset, int count);

			// Token: 0x06006C07 RID: 27655
			int MoveStartUntil(object cset, int count);

			// Token: 0x06006C08 RID: 27656
			int MoveEndUntil(object cset, int count);

			// Token: 0x06006C09 RID: 27657
			int FindText(string text, int cch, int flags);

			// Token: 0x06006C0A RID: 27658
			int FindTextStart(string text, int cch, int flags);

			// Token: 0x06006C0B RID: 27659
			int FindTextEnd(string text, int cch, int flags);

			// Token: 0x06006C0C RID: 27660
			int Delete(int unit, int count);

			// Token: 0x06006C0D RID: 27661
			void Cut(out object pVar);

			// Token: 0x06006C0E RID: 27662
			void Copy(out object pVar);

			// Token: 0x06006C0F RID: 27663
			void Paste(object pVar, int format);

			// Token: 0x06006C10 RID: 27664
			int CanPaste(object pVar, int format);

			// Token: 0x06006C11 RID: 27665
			int CanEdit();

			// Token: 0x06006C12 RID: 27666
			void ChangeCase(int type);

			// Token: 0x06006C13 RID: 27667
			void GetPoint(int type, out int x, out int y);

			// Token: 0x06006C14 RID: 27668
			void SetPoint(int x, int y, int type, int extend);

			// Token: 0x06006C15 RID: 27669
			void ScrollIntoView(int value);

			// Token: 0x06006C16 RID: 27670
			object GetEmbeddedObject();
		}

		// Token: 0x020007A5 RID: 1957
		[Guid("00020D03-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRichEditOleCallback
		{
			// Token: 0x06006C17 RID: 27671
			[PreserveSig]
			int GetNewStorage(out UnsafeNativeMethods.IStorage ret);

			// Token: 0x06006C18 RID: 27672
			[PreserveSig]
			int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);

			// Token: 0x06006C19 RID: 27673
			[PreserveSig]
			int ShowContainerUI(int fShow);

			// Token: 0x06006C1A RID: 27674
			[PreserveSig]
			int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);

			// Token: 0x06006C1B RID: 27675
			[PreserveSig]
			int DeleteObject(IntPtr lpoleobj);

			// Token: 0x06006C1C RID: 27676
			[PreserveSig]
			int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict);

			// Token: 0x06006C1D RID: 27677
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06006C1E RID: 27678
			[PreserveSig]
			int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj);

			// Token: 0x06006C1F RID: 27679
			[PreserveSig]
			int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);

			// Token: 0x06006C20 RID: 27680
			[PreserveSig]
			int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu);
		}

		// Token: 0x020007A6 RID: 1958
		[Guid("00000115-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceUIWindow
		{
			// Token: 0x06006C21 RID: 27681
			IntPtr GetWindow();

			// Token: 0x06006C22 RID: 27682
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06006C23 RID: 27683
			[PreserveSig]
			int GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x06006C24 RID: 27684
			[PreserveSig]
			int RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06006C25 RID: 27685
			[PreserveSig]
			int SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06006C26 RID: 27686
			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);
		}

		// Token: 0x020007A7 RID: 1959
		[SuppressUnmanagedCodeSecurity]
		[Guid("00000117-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceActiveObject
		{
			// Token: 0x06006C27 RID: 27687
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x06006C28 RID: 27688
			void ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06006C29 RID: 27689
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg);

			// Token: 0x06006C2A RID: 27690
			void OnFrameWindowActivate(bool fActivate);

			// Token: 0x06006C2B RID: 27691
			void OnDocWindowActivate(int fActivate);

			// Token: 0x06006C2C RID: 27692
			void ResizeBorder([In] NativeMethods.COMRECT prcBorder, [In] UnsafeNativeMethods.IOleInPlaceUIWindow pUIWindow, bool fFrameWindow);

			// Token: 0x06006C2D RID: 27693
			void EnableModeless(int fEnable);
		}

		// Token: 0x020007A8 RID: 1960
		[Guid("00000114-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleWindow
		{
			// Token: 0x06006C2E RID: 27694
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x06006C2F RID: 27695
			void ContextSensitiveHelp(int fEnterMode);
		}

		// Token: 0x020007A9 RID: 1961
		[SuppressUnmanagedCodeSecurity]
		[Guid("00000113-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceObject
		{
			// Token: 0x06006C30 RID: 27696
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x06006C31 RID: 27697
			void ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06006C32 RID: 27698
			void InPlaceDeactivate();

			// Token: 0x06006C33 RID: 27699
			[PreserveSig]
			int UIDeactivate();

			// Token: 0x06006C34 RID: 27700
			void SetObjectRects([In] NativeMethods.COMRECT lprcPosRect, [In] NativeMethods.COMRECT lprcClipRect);

			// Token: 0x06006C35 RID: 27701
			void ReactivateAndUndo();
		}

		// Token: 0x020007AA RID: 1962
		[SuppressUnmanagedCodeSecurity]
		[Guid("00000112-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleObject
		{
			// Token: 0x06006C36 RID: 27702
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pClientSite);

			// Token: 0x06006C37 RID: 27703
			UnsafeNativeMethods.IOleClientSite GetClientSite();

			// Token: 0x06006C38 RID: 27704
			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			// Token: 0x06006C39 RID: 27705
			[PreserveSig]
			int Close(int dwSaveOption);

			// Token: 0x06006C3A RID: 27706
			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x06006C3B RID: 27707
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x06006C3C RID: 27708
			[PreserveSig]
			int InitFromData([MarshalAs(UnmanagedType.Interface)] [In] IDataObject pDataObject, int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x06006C3D RID: 27709
			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			// Token: 0x06006C3E RID: 27710
			[PreserveSig]
			int DoVerb(int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			// Token: 0x06006C3F RID: 27711
			[PreserveSig]
			int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e);

			// Token: 0x06006C40 RID: 27712
			[PreserveSig]
			int OleUpdate();

			// Token: 0x06006C41 RID: 27713
			[PreserveSig]
			int IsUpToDate();

			// Token: 0x06006C42 RID: 27714
			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			// Token: 0x06006C43 RID: 27715
			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			// Token: 0x06006C44 RID: 27716
			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x06006C45 RID: 27717
			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			// Token: 0x06006C46 RID: 27718
			[PreserveSig]
			int Advise(IAdviseSink pAdvSink, out int cookie);

			// Token: 0x06006C47 RID: 27719
			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			// Token: 0x06006C48 RID: 27720
			[PreserveSig]
			int EnumAdvise(out IEnumSTATDATA e);

			// Token: 0x06006C49 RID: 27721
			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			// Token: 0x06006C4A RID: 27722
			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);
		}

		// Token: 0x020007AB RID: 1963
		[Guid("1C2056CC-5EF4-101B-8BC8-00AA003E3B29")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceObjectWindowless
		{
			// Token: 0x06006C4B RID: 27723
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pClientSite);

			// Token: 0x06006C4C RID: 27724
			[PreserveSig]
			int GetClientSite(out UnsafeNativeMethods.IOleClientSite site);

			// Token: 0x06006C4D RID: 27725
			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			// Token: 0x06006C4E RID: 27726
			[PreserveSig]
			int Close(int dwSaveOption);

			// Token: 0x06006C4F RID: 27727
			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x06006C50 RID: 27728
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x06006C51 RID: 27729
			[PreserveSig]
			int InitFromData([MarshalAs(UnmanagedType.Interface)] [In] IDataObject pDataObject, int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x06006C52 RID: 27730
			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			// Token: 0x06006C53 RID: 27731
			[PreserveSig]
			int DoVerb(int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			// Token: 0x06006C54 RID: 27732
			[PreserveSig]
			int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e);

			// Token: 0x06006C55 RID: 27733
			[PreserveSig]
			int OleUpdate();

			// Token: 0x06006C56 RID: 27734
			[PreserveSig]
			int IsUpToDate();

			// Token: 0x06006C57 RID: 27735
			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			// Token: 0x06006C58 RID: 27736
			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			// Token: 0x06006C59 RID: 27737
			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x06006C5A RID: 27738
			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			// Token: 0x06006C5B RID: 27739
			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink, out int cookie);

			// Token: 0x06006C5C RID: 27740
			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			// Token: 0x06006C5D RID: 27741
			[PreserveSig]
			int EnumAdvise(out IEnumSTATDATA e);

			// Token: 0x06006C5E RID: 27742
			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			// Token: 0x06006C5F RID: 27743
			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);

			// Token: 0x06006C60 RID: 27744
			[PreserveSig]
			int OnWindowMessage([MarshalAs(UnmanagedType.U4)] [In] int msg, [MarshalAs(UnmanagedType.U4)] [In] int wParam, [MarshalAs(UnmanagedType.U4)] [In] int lParam, [MarshalAs(UnmanagedType.U4)] [Out] int plResult);

			// Token: 0x06006C61 RID: 27745
			[PreserveSig]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [Out] object ppDropTarget);
		}

		// Token: 0x020007AC RID: 1964
		[SuppressUnmanagedCodeSecurity]
		[Guid("B196B288-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleControl
		{
			// Token: 0x06006C62 RID: 27746
			[PreserveSig]
			int GetControlInfo([Out] NativeMethods.tagCONTROLINFO pCI);

			// Token: 0x06006C63 RID: 27747
			[PreserveSig]
			int OnMnemonic([In] ref NativeMethods.MSG pMsg);

			// Token: 0x06006C64 RID: 27748
			[PreserveSig]
			int OnAmbientPropertyChange(int dispID);

			// Token: 0x06006C65 RID: 27749
			[PreserveSig]
			int FreezeEvents(int bFreeze);
		}

		// Token: 0x020007AD RID: 1965
		[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleServiceProvider
		{
			// Token: 0x06006C66 RID: 27750
			[PreserveSig]
			int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
		}

		// Token: 0x020007AE RID: 1966
		[Guid("0000010d-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IViewObject
		{
			// Token: 0x06006C67 RID: 27751
			[PreserveSig]
			int Draw([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] NativeMethods.COMRECT lprcBounds, [In] NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, [In] int dwContinue);

			// Token: 0x06006C68 RID: 27752
			[PreserveSig]
			int GetColorSet([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, [Out] NativeMethods.tagLOGPALETTE ppColorSet);

			// Token: 0x06006C69 RID: 27753
			[PreserveSig]
			int Freeze([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

			// Token: 0x06006C6A RID: 27754
			[PreserveSig]
			int Unfreeze([MarshalAs(UnmanagedType.U4)] [In] int dwFreeze);

			// Token: 0x06006C6B RID: 27755
			void SetAdvise([MarshalAs(UnmanagedType.U4)] [In] int aspects, [MarshalAs(UnmanagedType.U4)] [In] int advf, [MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink);

			// Token: 0x06006C6C RID: 27756
			void GetAdvise([MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] paspects, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] advf, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] IAdviseSink[] pAdvSink);
		}

		// Token: 0x020007AF RID: 1967
		[Guid("00000127-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IViewObject2
		{
			// Token: 0x06006C6D RID: 27757
			void Draw([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] NativeMethods.COMRECT lprcBounds, [In] NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, [In] int dwContinue);

			// Token: 0x06006C6E RID: 27758
			[PreserveSig]
			int GetColorSet([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, [Out] NativeMethods.tagLOGPALETTE ppColorSet);

			// Token: 0x06006C6F RID: 27759
			[PreserveSig]
			int Freeze([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

			// Token: 0x06006C70 RID: 27760
			[PreserveSig]
			int Unfreeze([MarshalAs(UnmanagedType.U4)] [In] int dwFreeze);

			// Token: 0x06006C71 RID: 27761
			void SetAdvise([MarshalAs(UnmanagedType.U4)] [In] int aspects, [MarshalAs(UnmanagedType.U4)] [In] int advf, [MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink);

			// Token: 0x06006C72 RID: 27762
			void GetAdvise([MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] paspects, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] advf, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] IAdviseSink[] pAdvSink);

			// Token: 0x06006C73 RID: 27763
			void GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, [In] NativeMethods.tagDVTARGETDEVICE ptd, [Out] NativeMethods.tagSIZEL lpsizel);
		}

		// Token: 0x020007B0 RID: 1968
		[Guid("0000010C-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersist
		{
			// Token: 0x06006C74 RID: 27764
			[SuppressUnmanagedCodeSecurity]
			void GetClassID(out Guid pClassID);
		}

		// Token: 0x020007B1 RID: 1969
		[Guid("37D84F60-42CB-11CE-8135-00AA004BB851")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersistPropertyBag
		{
			// Token: 0x06006C75 RID: 27765
			void GetClassID(out Guid pClassID);

			// Token: 0x06006C76 RID: 27766
			void InitNew();

			// Token: 0x06006C77 RID: 27767
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IPropertyBag pPropBag, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IErrorLog pErrorLog);

			// Token: 0x06006C78 RID: 27768
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IPropertyBag pPropBag, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty, [MarshalAs(UnmanagedType.Bool)] [In] bool fSaveAllProperties);
		}

		// Token: 0x020007B2 RID: 1970
		[Guid("CF51ED10-62FE-11CF-BF86-00A0C9034836")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IQuickActivate
		{
			// Token: 0x06006C79 RID: 27769
			void QuickActivate([In] UnsafeNativeMethods.tagQACONTAINER pQaContainer, [Out] UnsafeNativeMethods.tagQACONTROL pQaControl);

			// Token: 0x06006C7A RID: 27770
			void SetContentExtent([In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x06006C7B RID: 27771
			void GetContentExtent([Out] NativeMethods.tagSIZEL pSizel);
		}

		// Token: 0x020007B3 RID: 1971
		[Guid("000C060B-0000-0000-C000-000000000046")]
		[ComImport]
		public class SMsoComponentManager
		{
			// Token: 0x06006C7C RID: 27772
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern SMsoComponentManager();
		}

		// Token: 0x020007B4 RID: 1972
		[Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPropertyBag
		{
			// Token: 0x06006C7D RID: 27773
			[PreserveSig]
			int Read([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName, [In] [Out] ref object pVar, [In] UnsafeNativeMethods.IErrorLog pErrorLog);

			// Token: 0x06006C7E RID: 27774
			[PreserveSig]
			int Write([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName, [In] ref object pVar);
		}

		// Token: 0x020007B5 RID: 1973
		[Guid("3127CA40-446E-11CE-8135-00AA004BB851")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IErrorLog
		{
			// Token: 0x06006C7F RID: 27775
			void AddError([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName_p0, [MarshalAs(UnmanagedType.Struct)] [In] NativeMethods.tagEXCEPINFO pExcepInfo_p1);
		}

		// Token: 0x020007B6 RID: 1974
		[Guid("00000109-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersistStream
		{
			// Token: 0x06006C80 RID: 27776
			void GetClassID(out Guid pClassId);

			// Token: 0x06006C81 RID: 27777
			[PreserveSig]
			int IsDirty();

			// Token: 0x06006C82 RID: 27778
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x06006C83 RID: 27779
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			// Token: 0x06006C84 RID: 27780
			long GetSizeMax();
		}

		// Token: 0x020007B7 RID: 1975
		[SuppressUnmanagedCodeSecurity]
		[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersistStreamInit
		{
			// Token: 0x06006C85 RID: 27781
			void GetClassID(out Guid pClassID);

			// Token: 0x06006C86 RID: 27782
			[PreserveSig]
			int IsDirty();

			// Token: 0x06006C87 RID: 27783
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x06006C88 RID: 27784
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			// Token: 0x06006C89 RID: 27785
			void GetSizeMax([MarshalAs(UnmanagedType.LPArray)] [Out] long pcbSize);

			// Token: 0x06006C8A RID: 27786
			void InitNew();
		}

		// Token: 0x020007B8 RID: 1976
		[SuppressUnmanagedCodeSecurity]
		[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IConnectionPoint
		{
			// Token: 0x06006C8B RID: 27787
			[PreserveSig]
			int GetConnectionInterface(out Guid iid);

			// Token: 0x06006C8C RID: 27788
			[PreserveSig]
			int GetConnectionPointContainer([MarshalAs(UnmanagedType.Interface)] ref UnsafeNativeMethods.IConnectionPointContainer pContainer);

			// Token: 0x06006C8D RID: 27789
			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] object pUnkSink, ref int cookie);

			// Token: 0x06006C8E RID: 27790
			[PreserveSig]
			int Unadvise(int cookie);

			// Token: 0x06006C8F RID: 27791
			[PreserveSig]
			int EnumConnections(out object pEnum);
		}

		// Token: 0x020007B9 RID: 1977
		[Guid("0000010A-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersistStorage
		{
			// Token: 0x06006C90 RID: 27792
			void GetClassID(out Guid pClassID);

			// Token: 0x06006C91 RID: 27793
			[PreserveSig]
			int IsDirty();

			// Token: 0x06006C92 RID: 27794
			void InitNew(UnsafeNativeMethods.IStorage pstg);

			// Token: 0x06006C93 RID: 27795
			[PreserveSig]
			int Load(UnsafeNativeMethods.IStorage pstg);

			// Token: 0x06006C94 RID: 27796
			void Save(UnsafeNativeMethods.IStorage pStgSave, bool fSameAsLoad);

			// Token: 0x06006C95 RID: 27797
			void SaveCompleted(UnsafeNativeMethods.IStorage pStgNew);

			// Token: 0x06006C96 RID: 27798
			void HandsOffStorage();
		}

		// Token: 0x020007BA RID: 1978
		[Guid("00020404-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumVariant
		{
			// Token: 0x06006C97 RID: 27799
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [In] [Out] IntPtr rgvar, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x06006C98 RID: 27800
			void Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x06006C99 RID: 27801
			void Reset();

			// Token: 0x06006C9A RID: 27802
			void Clone([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IEnumVariant[] ppenum);
		}

		// Token: 0x020007BB RID: 1979
		[Guid("00000104-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumOLEVERB
		{
			// Token: 0x06006C9B RID: 27803
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] NativeMethods.tagOLEVERB rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x06006C9C RID: 27804
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x06006C9D RID: 27805
			void Reset();

			// Token: 0x06006C9E RID: 27806
			void Clone(out UnsafeNativeMethods.IEnumOLEVERB ppenum);
		}

		// Token: 0x020007BC RID: 1980
		[SuppressUnmanagedCodeSecurity]
		[Guid("00bb2762-6a77-11d0-a535-00c04fd7d062")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IAutoComplete
		{
			// Token: 0x06006C9F RID: 27807
			int Init([In] HandleRef hwndEdit, [In] IEnumString punkACL, [In] string pwszRegKeyPath, [In] string pwszQuickComplete);

			// Token: 0x06006CA0 RID: 27808
			void Enable([In] bool fEnable);
		}

		// Token: 0x020007BD RID: 1981
		[SuppressUnmanagedCodeSecurity]
		[Guid("EAC04BC0-3791-11d2-BB95-0060977B464C")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IAutoComplete2
		{
			// Token: 0x06006CA1 RID: 27809
			int Init([In] HandleRef hwndEdit, [In] IEnumString punkACL, [In] string pwszRegKeyPath, [In] string pwszQuickComplete);

			// Token: 0x06006CA2 RID: 27810
			void Enable([In] bool fEnable);

			// Token: 0x06006CA3 RID: 27811
			int SetOptions([In] int dwFlag);

			// Token: 0x06006CA4 RID: 27812
			void GetOptions([Out] IntPtr pdwFlag);
		}

		// Token: 0x020007BE RID: 1982
		[SuppressUnmanagedCodeSecurity]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IStream
		{
			// Token: 0x06006CA5 RID: 27813
			int Read(IntPtr buf, int len);

			// Token: 0x06006CA6 RID: 27814
			int Write(IntPtr buf, int len);

			// Token: 0x06006CA7 RID: 27815
			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, int dwOrigin);

			// Token: 0x06006CA8 RID: 27816
			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			// Token: 0x06006CA9 RID: 27817
			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			// Token: 0x06006CAA RID: 27818
			void Commit(int grfCommitFlags);

			// Token: 0x06006CAB RID: 27819
			void Revert();

			// Token: 0x06006CAC RID: 27820
			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x06006CAD RID: 27821
			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x06006CAE RID: 27822
			void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

			// Token: 0x06006CAF RID: 27823
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream Clone();
		}

		// Token: 0x020007BF RID: 1983
		public abstract class CharBuffer
		{
			// Token: 0x06006CB0 RID: 27824 RVA: 0x00191FD4 File Offset: 0x001901D4
			public static UnsafeNativeMethods.CharBuffer CreateBuffer(int size)
			{
				if (Marshal.SystemDefaultCharSize == 1)
				{
					return new UnsafeNativeMethods.AnsiCharBuffer(size);
				}
				return new UnsafeNativeMethods.UnicodeCharBuffer(size);
			}

			// Token: 0x06006CB1 RID: 27825
			public abstract IntPtr AllocCoTaskMem();

			// Token: 0x06006CB2 RID: 27826
			public abstract string GetString();

			// Token: 0x06006CB3 RID: 27827
			public abstract void PutCoTaskMem(IntPtr ptr);

			// Token: 0x06006CB4 RID: 27828
			public abstract void PutString(string s);
		}

		// Token: 0x020007C0 RID: 1984
		public class AnsiCharBuffer : UnsafeNativeMethods.CharBuffer
		{
			// Token: 0x06006CB6 RID: 27830 RVA: 0x00191FEB File Offset: 0x001901EB
			public AnsiCharBuffer(int size)
			{
				this.buffer = new byte[size];
			}

			// Token: 0x06006CB7 RID: 27831 RVA: 0x00192000 File Offset: 0x00190200
			public override IntPtr AllocCoTaskMem()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(this.buffer.Length);
				Marshal.Copy(this.buffer, 0, intPtr, this.buffer.Length);
				return intPtr;
			}

			// Token: 0x06006CB8 RID: 27832 RVA: 0x00192034 File Offset: 0x00190234
			public override string GetString()
			{
				int num = this.offset;
				while (num < this.buffer.Length && this.buffer[num] != 0)
				{
					num++;
				}
				string @string = Encoding.Default.GetString(this.buffer, this.offset, num - this.offset);
				if (num < this.buffer.Length)
				{
					num++;
				}
				this.offset = num;
				return @string;
			}

			// Token: 0x06006CB9 RID: 27833 RVA: 0x00192099 File Offset: 0x00190299
			public override void PutCoTaskMem(IntPtr ptr)
			{
				Marshal.Copy(ptr, this.buffer, 0, this.buffer.Length);
				this.offset = 0;
			}

			// Token: 0x06006CBA RID: 27834 RVA: 0x001920B8 File Offset: 0x001902B8
			public override void PutString(string s)
			{
				byte[] bytes = Encoding.Default.GetBytes(s);
				int num = Math.Min(bytes.Length, this.buffer.Length - this.offset);
				Array.Copy(bytes, 0, this.buffer, this.offset, num);
				this.offset += num;
				if (this.offset < this.buffer.Length)
				{
					byte[] array = this.buffer;
					int num2 = this.offset;
					this.offset = num2 + 1;
					array[num2] = 0;
				}
			}

			// Token: 0x040041C5 RID: 16837
			internal byte[] buffer;

			// Token: 0x040041C6 RID: 16838
			internal int offset;
		}

		// Token: 0x020007C1 RID: 1985
		public class UnicodeCharBuffer : UnsafeNativeMethods.CharBuffer
		{
			// Token: 0x06006CBB RID: 27835 RVA: 0x00192134 File Offset: 0x00190334
			public UnicodeCharBuffer(int size)
			{
				this.buffer = new char[size];
			}

			// Token: 0x06006CBC RID: 27836 RVA: 0x00192148 File Offset: 0x00190348
			public override IntPtr AllocCoTaskMem()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(this.buffer.Length * 2);
				Marshal.Copy(this.buffer, 0, intPtr, this.buffer.Length);
				return intPtr;
			}

			// Token: 0x06006CBD RID: 27837 RVA: 0x0019217C File Offset: 0x0019037C
			public override string GetString()
			{
				int num = this.offset;
				while (num < this.buffer.Length && this.buffer[num] != '\0')
				{
					num++;
				}
				string result = new string(this.buffer, this.offset, num - this.offset);
				if (num < this.buffer.Length)
				{
					num++;
				}
				this.offset = num;
				return result;
			}

			// Token: 0x06006CBE RID: 27838 RVA: 0x001921DC File Offset: 0x001903DC
			public override void PutCoTaskMem(IntPtr ptr)
			{
				Marshal.Copy(ptr, this.buffer, 0, this.buffer.Length);
				this.offset = 0;
			}

			// Token: 0x06006CBF RID: 27839 RVA: 0x001921FC File Offset: 0x001903FC
			public override void PutString(string s)
			{
				int num = Math.Min(s.Length, this.buffer.Length - this.offset);
				s.CopyTo(0, this.buffer, this.offset, num);
				this.offset += num;
				if (this.offset < this.buffer.Length)
				{
					char[] array = this.buffer;
					int num2 = this.offset;
					this.offset = num2 + 1;
					array[num2] = 0;
				}
			}

			// Token: 0x040041C7 RID: 16839
			internal char[] buffer;

			// Token: 0x040041C8 RID: 16840
			internal int offset;
		}

		// Token: 0x020007C2 RID: 1986
		public class ComStreamFromDataStream : UnsafeNativeMethods.IStream
		{
			// Token: 0x06006CC0 RID: 27840 RVA: 0x0019226F File Offset: 0x0019046F
			public ComStreamFromDataStream(Stream dataStream)
			{
				if (dataStream == null)
				{
					throw new ArgumentNullException("dataStream");
				}
				this.dataStream = dataStream;
			}

			// Token: 0x06006CC1 RID: 27841 RVA: 0x00192294 File Offset: 0x00190494
			private void ActualizeVirtualPosition()
			{
				if (this.virtualPosition == -1L)
				{
					return;
				}
				if (this.virtualPosition > this.dataStream.Length)
				{
					this.dataStream.SetLength(this.virtualPosition);
				}
				this.dataStream.Position = this.virtualPosition;
				this.virtualPosition = -1L;
			}

			// Token: 0x06006CC2 RID: 27842 RVA: 0x001922E9 File Offset: 0x001904E9
			public UnsafeNativeMethods.IStream Clone()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
				return null;
			}

			// Token: 0x06006CC3 RID: 27843 RVA: 0x001922F1 File Offset: 0x001904F1
			public void Commit(int grfCommitFlags)
			{
				this.dataStream.Flush();
				this.ActualizeVirtualPosition();
			}

			// Token: 0x06006CC4 RID: 27844 RVA: 0x00192304 File Offset: 0x00190504
			public long CopyTo(UnsafeNativeMethods.IStream pstm, long cb, long[] pcbRead)
			{
				int num = 4096;
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				long num2 = 0L;
				try
				{
					while (num2 < cb)
					{
						int num3 = num;
						if (num2 + (long)num3 > cb)
						{
							num3 = (int)(cb - num2);
						}
						int num4 = this.Read(intPtr, num3);
						if (num4 == 0)
						{
							break;
						}
						if (pstm.Write(intPtr, num4) != num4)
						{
							throw UnsafeNativeMethods.ComStreamFromDataStream.EFail("Wrote an incorrect number of bytes");
						}
						num2 += (long)num4;
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (pcbRead != null && pcbRead.Length != 0)
				{
					pcbRead[0] = num2;
				}
				return num2;
			}

			// Token: 0x06006CC5 RID: 27845 RVA: 0x0019239C File Offset: 0x0019059C
			public Stream GetDataStream()
			{
				return this.dataStream;
			}

			// Token: 0x06006CC6 RID: 27846 RVA: 0x0000701A File Offset: 0x0000521A
			public void LockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x06006CC7 RID: 27847 RVA: 0x001923A4 File Offset: 0x001905A4
			protected static ExternalException EFail(string msg)
			{
				ExternalException ex = new ExternalException(msg, -2147467259);
				throw ex;
			}

			// Token: 0x06006CC8 RID: 27848 RVA: 0x001923C0 File Offset: 0x001905C0
			protected static void NotImplemented()
			{
				ExternalException ex = new ExternalException(SR.GetString("UnsafeNativeMethodsNotImplemented"), -2147467263);
				throw ex;
			}

			// Token: 0x06006CC9 RID: 27849 RVA: 0x001923E4 File Offset: 0x001905E4
			public int Read(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				int num = this.Read(array, length);
				Marshal.Copy(array, 0, buf, num);
				return num;
			}

			// Token: 0x06006CCA RID: 27850 RVA: 0x0019240B File Offset: 0x0019060B
			public int Read(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				return this.dataStream.Read(buffer, 0, length);
			}

			// Token: 0x06006CCB RID: 27851 RVA: 0x00192421 File Offset: 0x00190621
			public void Revert()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
			}

			// Token: 0x06006CCC RID: 27852 RVA: 0x00192428 File Offset: 0x00190628
			public long Seek(long offset, int origin)
			{
				long position = this.virtualPosition;
				if (this.virtualPosition == -1L)
				{
					position = this.dataStream.Position;
				}
				long length = this.dataStream.Length;
				switch (origin)
				{
				case 0:
					if (offset <= length)
					{
						this.dataStream.Position = offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset;
					}
					break;
				case 1:
					if (offset + position <= length)
					{
						this.dataStream.Position = position + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset + position;
					}
					break;
				case 2:
					if (offset <= 0L)
					{
						this.dataStream.Position = length + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = length + offset;
					}
					break;
				}
				if (this.virtualPosition != -1L)
				{
					return this.virtualPosition;
				}
				return this.dataStream.Position;
			}

			// Token: 0x06006CCD RID: 27853 RVA: 0x00192500 File Offset: 0x00190700
			public void SetSize(long value)
			{
				this.dataStream.SetLength(value);
			}

			// Token: 0x06006CCE RID: 27854 RVA: 0x0019250E File Offset: 0x0019070E
			public void Stat(NativeMethods.STATSTG pstatstg, int grfStatFlag)
			{
				pstatstg.type = 2;
				pstatstg.cbSize = this.dataStream.Length;
				pstatstg.grfLocksSupported = 2;
			}

			// Token: 0x06006CCF RID: 27855 RVA: 0x0000701A File Offset: 0x0000521A
			public void UnlockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x06006CD0 RID: 27856 RVA: 0x00192530 File Offset: 0x00190730
			public int Write(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				Marshal.Copy(buf, array, 0, length);
				return this.Write(array, length);
			}

			// Token: 0x06006CD1 RID: 27857 RVA: 0x00192555 File Offset: 0x00190755
			public int Write(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				this.dataStream.Write(buffer, 0, length);
				return length;
			}

			// Token: 0x040041C9 RID: 16841
			protected Stream dataStream;

			// Token: 0x040041CA RID: 16842
			private long virtualPosition = -1L;
		}

		// Token: 0x020007C3 RID: 1987
		[Guid("0000000B-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IStorage
		{
			// Token: 0x06006CD2 RID: 27858
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream CreateStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06006CD3 RID: 27859
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream OpenStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr reserved1, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06006CD4 RID: 27860
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage CreateStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06006CD5 RID: 27861
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage OpenStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr pstgPriority, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, IntPtr snbExclude, [MarshalAs(UnmanagedType.U4)] [In] int reserved);

			// Token: 0x06006CD6 RID: 27862
			void CopyTo(int ciidExclude, [MarshalAs(UnmanagedType.LPArray)] [In] Guid[] pIIDExclude, IntPtr snbExclude, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest);

			// Token: 0x06006CD7 RID: 27863
			void MoveElementTo([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName, [MarshalAs(UnmanagedType.U4)] [In] int grfFlags);

			// Token: 0x06006CD8 RID: 27864
			void Commit(int grfCommitFlags);

			// Token: 0x06006CD9 RID: 27865
			void Revert();

			// Token: 0x06006CDA RID: 27866
			void EnumElements([MarshalAs(UnmanagedType.U4)] [In] int reserved1, IntPtr reserved2, [MarshalAs(UnmanagedType.U4)] [In] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);

			// Token: 0x06006CDB RID: 27867
			void DestroyElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName);

			// Token: 0x06006CDC RID: 27868
			void RenameElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsOldName, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName);

			// Token: 0x06006CDD RID: 27869
			void SetElementTimes([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [In] NativeMethods.FILETIME pctime, [In] NativeMethods.FILETIME patime, [In] NativeMethods.FILETIME pmtime);

			// Token: 0x06006CDE RID: 27870
			void SetClass([In] ref Guid clsid);

			// Token: 0x06006CDF RID: 27871
			void SetStateBits(int grfStateBits, int grfMask);

			// Token: 0x06006CE0 RID: 27872
			void Stat([Out] NativeMethods.STATSTG pStatStg, int grfStatFlag);
		}

		// Token: 0x020007C4 RID: 1988
		[Guid("B196B28F-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IClassFactory2
		{
			// Token: 0x06006CE1 RID: 27873
			void CreateInstance([MarshalAs(UnmanagedType.Interface)] [In] object unused, [In] ref Guid refiid, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppunk);

			// Token: 0x06006CE2 RID: 27874
			void LockServer(int fLock);

			// Token: 0x06006CE3 RID: 27875
			void GetLicInfo([Out] NativeMethods.tagLICINFO licInfo);

			// Token: 0x06006CE4 RID: 27876
			void RequestLicKey([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrKey);

			// Token: 0x06006CE5 RID: 27877
			void CreateInstanceLic([MarshalAs(UnmanagedType.Interface)] [In] object pUnkOuter, [MarshalAs(UnmanagedType.Interface)] [In] object pUnkReserved, [In] ref Guid riid, [MarshalAs(UnmanagedType.BStr)] [In] string bstrKey, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
		}

		// Token: 0x020007C5 RID: 1989
		[SuppressUnmanagedCodeSecurity]
		[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IConnectionPointContainer
		{
			// Token: 0x06006CE6 RID: 27878
			[return: MarshalAs(UnmanagedType.Interface)]
			object EnumConnectionPoints();

			// Token: 0x06006CE7 RID: 27879
			[PreserveSig]
			int FindConnectionPoint([In] ref Guid guid, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IConnectionPoint ppCP);
		}

		// Token: 0x020007C6 RID: 1990
		[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumConnectionPoints
		{
			// Token: 0x06006CE8 RID: 27880
			[PreserveSig]
			int Next(int cConnections, out UnsafeNativeMethods.IConnectionPoint pCp, out int pcFetched);

			// Token: 0x06006CE9 RID: 27881
			[PreserveSig]
			int Skip(int cSkip);

			// Token: 0x06006CEA RID: 27882
			void Reset();

			// Token: 0x06006CEB RID: 27883
			UnsafeNativeMethods.IEnumConnectionPoints Clone();
		}

		// Token: 0x020007C7 RID: 1991
		[Guid("00020400-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDispatch
		{
			// Token: 0x06006CEC RID: 27884
			int GetTypeInfoCount();

			// Token: 0x06006CED RID: 27885
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITypeInfo GetTypeInfo([MarshalAs(UnmanagedType.U4)] [In] int iTInfo, [MarshalAs(UnmanagedType.U4)] [In] int lcid);

			// Token: 0x06006CEE RID: 27886
			[PreserveSig]
			int GetIDsOfNames([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [In] string[] rgszNames, [MarshalAs(UnmanagedType.U4)] [In] int cNames, [MarshalAs(UnmanagedType.U4)] [In] int lcid, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgDispId);

			// Token: 0x06006CEF RID: 27887
			[PreserveSig]
			int Invoke(int dispIdMember, [In] ref Guid riid, [MarshalAs(UnmanagedType.U4)] [In] int lcid, [MarshalAs(UnmanagedType.U4)] [In] int dwFlags, [In] [Out] NativeMethods.tagDISPPARAMS pDispParams, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] pVarResult, [In] [Out] NativeMethods.tagEXCEPINFO pExcepInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] pArgErr);
		}

		// Token: 0x020007C8 RID: 1992
		[Guid("00020401-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITypeInfo
		{
			// Token: 0x06006CF0 RID: 27888
			[PreserveSig]
			int GetTypeAttr(ref IntPtr pTypeAttr);

			// Token: 0x06006CF1 RID: 27889
			[PreserveSig]
			int GetTypeComp([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTComp);

			// Token: 0x06006CF2 RID: 27890
			[PreserveSig]
			int GetFuncDesc([MarshalAs(UnmanagedType.U4)] [In] int index, ref IntPtr pFuncDesc);

			// Token: 0x06006CF3 RID: 27891
			[PreserveSig]
			int GetVarDesc([MarshalAs(UnmanagedType.U4)] [In] int index, ref IntPtr pVarDesc);

			// Token: 0x06006CF4 RID: 27892
			[PreserveSig]
			int GetNames(int memid, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] rgBstrNames, [MarshalAs(UnmanagedType.U4)] [In] int cMaxNames, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcNames);

			// Token: 0x06006CF5 RID: 27893
			[PreserveSig]
			int GetRefTypeOfImplType([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pRefType);

			// Token: 0x06006CF6 RID: 27894
			[PreserveSig]
			int GetImplTypeFlags([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pImplTypeFlags);

			// Token: 0x06006CF7 RID: 27895
			[PreserveSig]
			int GetIDsOfNames(IntPtr rgszNames, int cNames, IntPtr pMemId);

			// Token: 0x06006CF8 RID: 27896
			[PreserveSig]
			int Invoke();

			// Token: 0x06006CF9 RID: 27897
			[PreserveSig]
			int GetDocumentation(int memid, ref string pBstrName, ref string pBstrDocString, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pdwHelpContext, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrHelpFile);

			// Token: 0x06006CFA RID: 27898
			[PreserveSig]
			int GetDllEntry(int memid, NativeMethods.tagINVOKEKIND invkind, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrDllName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrName, [MarshalAs(UnmanagedType.LPArray)] [Out] short[] pwOrdinal);

			// Token: 0x06006CFB RID: 27899
			[PreserveSig]
			int GetRefTypeInfo(IntPtr hreftype, ref UnsafeNativeMethods.ITypeInfo pTypeInfo);

			// Token: 0x06006CFC RID: 27900
			[PreserveSig]
			int AddressOfMember();

			// Token: 0x06006CFD RID: 27901
			[PreserveSig]
			int CreateInstance([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppvObj);

			// Token: 0x06006CFE RID: 27902
			[PreserveSig]
			int GetMops(int memid, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrMops);

			// Token: 0x06006CFF RID: 27903
			[PreserveSig]
			int GetContainingTypeLib([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeLib[] ppTLib, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pIndex);

			// Token: 0x06006D00 RID: 27904
			[PreserveSig]
			void ReleaseTypeAttr(IntPtr typeAttr);

			// Token: 0x06006D01 RID: 27905
			[PreserveSig]
			void ReleaseFuncDesc(IntPtr funcDesc);

			// Token: 0x06006D02 RID: 27906
			[PreserveSig]
			void ReleaseVarDesc(IntPtr varDesc);
		}

		// Token: 0x020007C9 RID: 1993
		[Guid("00020403-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITypeComp
		{
			// Token: 0x06006D03 RID: 27907
			void RemoteBind([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.U2)] [In] short wFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagDESCKIND[] pDescKind, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagFUNCDESC[] ppFuncDesc, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagVARDESC[] ppVarDesc, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTypeComp, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pDummy);

			// Token: 0x06006D04 RID: 27908
			void RemoteBindType([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);
		}

		// Token: 0x020007CA RID: 1994
		[Guid("00020402-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITypeLib
		{
			// Token: 0x06006D05 RID: 27909
			void RemoteGetTypeInfoCount([MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcTInfo);

			// Token: 0x06006D06 RID: 27910
			void GetTypeInfo([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);

			// Token: 0x06006D07 RID: 27911
			void GetTypeInfoType([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagTYPEKIND[] pTKind);

			// Token: 0x06006D08 RID: 27912
			void GetTypeInfoOfGuid([In] ref Guid guid, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);

			// Token: 0x06006D09 RID: 27913
			void RemoteGetLibAttr(IntPtr ppTLibAttr, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pDummy);

			// Token: 0x06006D0A RID: 27914
			void GetTypeComp([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTComp);

			// Token: 0x06006D0B RID: 27915
			void RemoteGetDocumentation(int index, [MarshalAs(UnmanagedType.U4)] [In] int refPtrFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrDocString, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pdwHelpContext, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrHelpFile);

			// Token: 0x06006D0C RID: 27916
			void RemoteIsName([MarshalAs(UnmanagedType.LPWStr)] [In] string szNameBuf, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] pfName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrLibName);

			// Token: 0x06006D0D RID: 27917
			void RemoteFindName([MarshalAs(UnmanagedType.LPWStr)] [In] string szNameBuf, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] short[] pcFound, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrLibName);

			// Token: 0x06006D0E RID: 27918
			void LocalReleaseTLibAttr();
		}

		// Token: 0x020007CB RID: 1995
		[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ISupportErrorInfo
		{
			// Token: 0x06006D0F RID: 27919
			int InterfaceSupportsErrorInfo([In] ref Guid riid);
		}

		// Token: 0x020007CC RID: 1996
		[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IErrorInfo
		{
			// Token: 0x06006D10 RID: 27920
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetGUID(out Guid pguid);

			// Token: 0x06006D11 RID: 27921
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetSource([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrSource);

			// Token: 0x06006D12 RID: 27922
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetDescription([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrDescription);

			// Token: 0x06006D13 RID: 27923
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetHelpFile([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrHelpFile);

			// Token: 0x06006D14 RID: 27924
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetHelpContext([MarshalAs(UnmanagedType.U4)] [In] [Out] ref int pdwHelpContext);
		}

		// Token: 0x020007CD RID: 1997
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagQACONTAINER
		{
			// Token: 0x040041CB RID: 16843
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.tagQACONTAINER));

			// Token: 0x040041CC RID: 16844
			public UnsafeNativeMethods.IOleClientSite pClientSite;

			// Token: 0x040041CD RID: 16845
			[MarshalAs(UnmanagedType.Interface)]
			public object pAdviseSink;

			// Token: 0x040041CE RID: 16846
			public UnsafeNativeMethods.IPropertyNotifySink pPropertyNotifySink;

			// Token: 0x040041CF RID: 16847
			[MarshalAs(UnmanagedType.Interface)]
			public object pUnkEventSink;

			// Token: 0x040041D0 RID: 16848
			[MarshalAs(UnmanagedType.U4)]
			public int dwAmbientFlags;

			// Token: 0x040041D1 RID: 16849
			[MarshalAs(UnmanagedType.U4)]
			public uint colorFore;

			// Token: 0x040041D2 RID: 16850
			[MarshalAs(UnmanagedType.U4)]
			public uint colorBack;

			// Token: 0x040041D3 RID: 16851
			[MarshalAs(UnmanagedType.Interface)]
			public object pFont;

			// Token: 0x040041D4 RID: 16852
			[MarshalAs(UnmanagedType.Interface)]
			public object pUndoMgr;

			// Token: 0x040041D5 RID: 16853
			[MarshalAs(UnmanagedType.U4)]
			public int dwAppearance;

			// Token: 0x040041D6 RID: 16854
			public int lcid;

			// Token: 0x040041D7 RID: 16855
			public IntPtr hpal = IntPtr.Zero;

			// Token: 0x040041D8 RID: 16856
			[MarshalAs(UnmanagedType.Interface)]
			public object pBindHost;
		}

		// Token: 0x020007CE RID: 1998
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagQACONTROL
		{
			// Token: 0x040041D9 RID: 16857
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.tagQACONTROL));

			// Token: 0x040041DA RID: 16858
			[MarshalAs(UnmanagedType.U4)]
			public int dwMiscStatus;

			// Token: 0x040041DB RID: 16859
			[MarshalAs(UnmanagedType.U4)]
			public int dwViewStatus;

			// Token: 0x040041DC RID: 16860
			[MarshalAs(UnmanagedType.U4)]
			public int dwEventCookie;

			// Token: 0x040041DD RID: 16861
			[MarshalAs(UnmanagedType.U4)]
			public int dwPropNotifyCookie;

			// Token: 0x040041DE RID: 16862
			[MarshalAs(UnmanagedType.U4)]
			public int dwPointerActivationPolicy;
		}

		// Token: 0x020007CF RID: 1999
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("E44C3566-915D-4070-99C6-047BFF5A08F5")]
		[ComVisible(true)]
		[ComImport]
		public interface ILegacyIAccessibleProvider
		{
			// Token: 0x06006D17 RID: 27927
			void Select(int flagsSelect);

			// Token: 0x06006D18 RID: 27928
			void DoDefaultAction();

			// Token: 0x06006D19 RID: 27929
			void SetValue([MarshalAs(UnmanagedType.LPWStr)] string szValue);

			// Token: 0x06006D1A RID: 27930
			[return: MarshalAs(UnmanagedType.Interface)]
			IAccessible GetIAccessible();

			// Token: 0x170017A6 RID: 6054
			// (get) Token: 0x06006D1B RID: 27931
			int ChildId { get; }

			// Token: 0x170017A7 RID: 6055
			// (get) Token: 0x06006D1C RID: 27932
			string Name { get; }

			// Token: 0x170017A8 RID: 6056
			// (get) Token: 0x06006D1D RID: 27933
			string Value { get; }

			// Token: 0x170017A9 RID: 6057
			// (get) Token: 0x06006D1E RID: 27934
			string Description { get; }

			// Token: 0x170017AA RID: 6058
			// (get) Token: 0x06006D1F RID: 27935
			uint Role { get; }

			// Token: 0x170017AB RID: 6059
			// (get) Token: 0x06006D20 RID: 27936
			uint State { get; }

			// Token: 0x170017AC RID: 6060
			// (get) Token: 0x06006D21 RID: 27937
			string Help { get; }

			// Token: 0x170017AD RID: 6061
			// (get) Token: 0x06006D22 RID: 27938
			string KeyboardShortcut { get; }

			// Token: 0x06006D23 RID: 27939
			object[] GetSelection();

			// Token: 0x170017AE RID: 6062
			// (get) Token: 0x06006D24 RID: 27940
			string DefaultAction { get; }
		}

		// Token: 0x020007D0 RID: 2000
		[Guid("0000000A-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ILockBytes
		{
			// Token: 0x06006D25 RID: 27941
			void ReadAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, [Out] IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbRead);

			// Token: 0x06006D26 RID: 27942
			void WriteAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbWritten);

			// Token: 0x06006D27 RID: 27943
			void Flush();

			// Token: 0x06006D28 RID: 27944
			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long cb);

			// Token: 0x06006D29 RID: 27945
			void LockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x06006D2A RID: 27946
			void UnlockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x06006D2B RID: 27947
			void Stat([Out] NativeMethods.STATSTG pstatstg, [MarshalAs(UnmanagedType.U4)] [In] int grfStatFlag);
		}

		// Token: 0x020007D1 RID: 2001
		[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential)]
		public class OFNOTIFY
		{
			// Token: 0x040041DF RID: 16863
			public IntPtr hdr_hwndFrom = IntPtr.Zero;

			// Token: 0x040041E0 RID: 16864
			public IntPtr hdr_idFrom = IntPtr.Zero;

			// Token: 0x040041E1 RID: 16865
			public int hdr_code;

			// Token: 0x040041E2 RID: 16866
			public IntPtr lpOFN = IntPtr.Zero;

			// Token: 0x040041E3 RID: 16867
			public IntPtr pszFile = IntPtr.Zero;
		}

		// Token: 0x020007D2 RID: 2002
		// (Invoke) Token: 0x06006D2E RID: 27950
		public delegate int BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

		// Token: 0x020007D3 RID: 2003
		[Flags]
		public enum BrowseInfos
		{
			// Token: 0x040041E5 RID: 16869
			NewDialogStyle = 64,
			// Token: 0x040041E6 RID: 16870
			HideNewFolderButton = 512
		}

		// Token: 0x020007D4 RID: 2004
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class BROWSEINFO
		{
			// Token: 0x040041E7 RID: 16871
			public IntPtr hwndOwner;

			// Token: 0x040041E8 RID: 16872
			public IntPtr pidlRoot;

			// Token: 0x040041E9 RID: 16873
			public IntPtr pszDisplayName;

			// Token: 0x040041EA RID: 16874
			public string lpszTitle;

			// Token: 0x040041EB RID: 16875
			public int ulFlags;

			// Token: 0x040041EC RID: 16876
			public UnsafeNativeMethods.BrowseCallbackProc lpfn;

			// Token: 0x040041ED RID: 16877
			public IntPtr lParam;

			// Token: 0x040041EE RID: 16878
			public int iImage;
		}

		// Token: 0x020007D5 RID: 2005
		[SuppressUnmanagedCodeSecurity]
		internal class Shell32
		{
			// Token: 0x06006D32 RID: 27954
			[DllImport("shell32.dll")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

			// Token: 0x06006D33 RID: 27955
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			private static extern bool SHGetPathFromIDListEx(IntPtr pidl, IntPtr pszPath, int cchPath, int flags);

			// Token: 0x06006D34 RID: 27956 RVA: 0x001925E8 File Offset: 0x001907E8
			public static bool SHGetPathFromIDListLongPath(IntPtr pidl, ref IntPtr pszPath)
			{
				int num = 1;
				int num2 = 260 * Marshal.SystemDefaultCharSize;
				int num3 = 260;
				bool result;
				while (!(result = UnsafeNativeMethods.Shell32.SHGetPathFromIDListEx(pidl, pszPath, num3, 0)) && num3 < 32767)
				{
					string text = Marshal.PtrToStringAuto(pszPath);
					if (text.Length != 0 && text.Length < num3)
					{
						break;
					}
					num += 2;
					num3 = ((num * num3 >= 32767) ? 32767 : (num * num3));
					pszPath = Marshal.ReAllocHGlobal(pszPath, (IntPtr)((num3 + 1) * Marshal.SystemDefaultCharSize));
				}
				return result;
			}

			// Token: 0x06006D35 RID: 27957
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder([In] UnsafeNativeMethods.BROWSEINFO lpbi);

			// Token: 0x06006D36 RID: 27958
			[DllImport("shell32.dll")]
			public static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMalloc[] ppMalloc);

			// Token: 0x06006D37 RID: 27959
			[DllImport("shell32.dll")]
			private static extern int SHGetKnownFolderPath(ref Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);

			// Token: 0x06006D38 RID: 27960 RVA: 0x00192670 File Offset: 0x00190870
			public static int SHGetFolderPathEx(ref Guid rfid, uint dwFlags, IntPtr hToken, StringBuilder pszPath)
			{
				if (UnsafeNativeMethods.IsVista)
				{
					IntPtr zero = IntPtr.Zero;
					int result;
					if ((result = UnsafeNativeMethods.Shell32.SHGetKnownFolderPath(ref rfid, dwFlags, hToken, out zero)) == 0)
					{
						pszPath.Append(Marshal.PtrToStringAuto(zero));
						UnsafeNativeMethods.CoTaskMemFree(zero);
					}
					return result;
				}
				throw new NotSupportedException();
			}

			// Token: 0x06006D39 RID: 27961
			[DllImport("shell32.dll")]
			public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out FileDialogNative.IShellItem ppsi);

			// Token: 0x06006D3A RID: 27962
			[DllImport("shell32.dll")]
			public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);
		}

		// Token: 0x020007D6 RID: 2006
		[Guid("00000002-0000-0000-c000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IMalloc
		{
			// Token: 0x06006D3C RID: 27964
			[PreserveSig]
			IntPtr Alloc(int cb);

			// Token: 0x06006D3D RID: 27965
			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);

			// Token: 0x06006D3E RID: 27966
			[PreserveSig]
			void Free(IntPtr pv);

			// Token: 0x06006D3F RID: 27967
			[PreserveSig]
			int GetSize(IntPtr pv);

			// Token: 0x06006D40 RID: 27968
			[PreserveSig]
			int DidAlloc(IntPtr pv);

			// Token: 0x06006D41 RID: 27969
			[PreserveSig]
			void HeapMinimize();
		}

		// Token: 0x020007D7 RID: 2007
		[Guid("00000126-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRunnableObject
		{
			// Token: 0x06006D42 RID: 27970
			void GetRunningClass(out Guid guid);

			// Token: 0x06006D43 RID: 27971
			[PreserveSig]
			int Run(IntPtr lpBindContext);

			// Token: 0x06006D44 RID: 27972
			bool IsRunning();

			// Token: 0x06006D45 RID: 27973
			void LockRunning(bool fLock, bool fLastUnlockCloses);

			// Token: 0x06006D46 RID: 27974
			void SetContainedObject(bool fContained);
		}

		// Token: 0x020007D8 RID: 2008
		[ComVisible(true)]
		[Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDocumentSite
		{
			// Token: 0x06006D47 RID: 27975
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ActivateMe([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleDocumentView pViewToActivate);
		}

		// Token: 0x020007D9 RID: 2009
		[ComVisible(true)]
		[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleDocumentView
		{
			// Token: 0x06006D48 RID: 27976
			void SetInPlaceSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceSite pIPSite);

			// Token: 0x06006D49 RID: 27977
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOleInPlaceSite GetInPlaceSite();

			// Token: 0x06006D4A RID: 27978
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();

			// Token: 0x06006D4B RID: 27979
			void SetRect([In] ref NativeMethods.RECT prcView);

			// Token: 0x06006D4C RID: 27980
			void GetRect([In] [Out] ref NativeMethods.RECT prcView);

			// Token: 0x06006D4D RID: 27981
			void SetRectComplex([In] NativeMethods.RECT prcView, [In] NativeMethods.RECT prcHScroll, [In] NativeMethods.RECT prcVScroll, [In] NativeMethods.RECT prcSizeBox);

			// Token: 0x06006D4E RID: 27982
			void Show(bool fShow);

			// Token: 0x06006D4F RID: 27983
			[PreserveSig]
			int UIActivate(bool fUIActivate);

			// Token: 0x06006D50 RID: 27984
			void Open();

			// Token: 0x06006D51 RID: 27985
			[PreserveSig]
			int Close([MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x06006D52 RID: 27986
			void SaveViewState([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x06006D53 RID: 27987
			void ApplyViewState([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x06006D54 RID: 27988
			void Clone([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceSite pIPSiteNew, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IOleDocumentView[] ppViewNew);
		}

		// Token: 0x020007DA RID: 2010
		[Guid("b722bcc5-4e68-101b-a2bc-00aa00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDocument
		{
			// Token: 0x06006D55 RID: 27989
			[PreserveSig]
			int CreateView(UnsafeNativeMethods.IOleInPlaceSite pIPSite, UnsafeNativeMethods.IStream pstm, int dwReserved, out UnsafeNativeMethods.IOleDocumentView ppView);

			// Token: 0x06006D56 RID: 27990
			[PreserveSig]
			int GetDocMiscStatus(out int pdwStatus);

			// Token: 0x06006D57 RID: 27991
			int EnumViews(out object ppEnum, out UnsafeNativeMethods.IOleDocumentView ppView);
		}

		// Token: 0x020007DB RID: 2011
		[Guid("0000011e-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleCache
		{
			// Token: 0x06006D58 RID: 27992
			int Cache(ref FORMATETC pformatetc, int advf);

			// Token: 0x06006D59 RID: 27993
			void Uncache(int dwConnection);

			// Token: 0x06006D5A RID: 27994
			object EnumCache();

			// Token: 0x06006D5B RID: 27995
			void InitCache(IDataObject pDataObject);

			// Token: 0x06006D5C RID: 27996
			void SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, bool fRelease);
		}

		// Token: 0x020007DC RID: 2012
		[TypeLibType(4176)]
		[Guid("618736E0-3C3D-11CF-810C-00AA00389B71")]
		[ComImport]
		public interface IAccessibleInternal
		{
			// Token: 0x06006D5D RID: 27997
			[DispId(-5000)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object get_accParent();

			// Token: 0x06006D5E RID: 27998
			[DispId(-5001)]
			[TypeLibFunc(64)]
			int get_accChildCount();

			// Token: 0x06006D5F RID: 27999
			[TypeLibFunc(64)]
			[DispId(-5002)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object get_accChild([MarshalAs(UnmanagedType.Struct)] [In] object varChild);

			// Token: 0x06006D60 RID: 28000
			[DispId(-5003)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accName([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D61 RID: 28001
			[TypeLibFunc(64)]
			[DispId(-5004)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accValue([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D62 RID: 28002
			[DispId(-5005)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accDescription([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D63 RID: 28003
			[DispId(-5006)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accRole([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D64 RID: 28004
			[TypeLibFunc(64)]
			[DispId(-5007)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accState([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D65 RID: 28005
			[TypeLibFunc(64)]
			[DispId(-5008)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accHelp([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D66 RID: 28006
			[DispId(-5009)]
			[TypeLibFunc(64)]
			int get_accHelpTopic([MarshalAs(UnmanagedType.BStr)] out string pszHelpFile, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D67 RID: 28007
			[DispId(-5010)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accKeyboardShortcut([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D68 RID: 28008
			[DispId(-5011)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accFocus();

			// Token: 0x06006D69 RID: 28009
			[DispId(-5012)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accSelection();

			// Token: 0x06006D6A RID: 28010
			[TypeLibFunc(64)]
			[DispId(-5013)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accDefaultAction([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D6B RID: 28011
			[DispId(-5014)]
			[TypeLibFunc(64)]
			void accSelect([In] int flagsSelect, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D6C RID: 28012
			[DispId(-5015)]
			[TypeLibFunc(64)]
			void accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D6D RID: 28013
			[TypeLibFunc(64)]
			[DispId(-5016)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object accNavigate([In] int navDir, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varStart);

			// Token: 0x06006D6E RID: 28014
			[TypeLibFunc(64)]
			[DispId(-5017)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object accHitTest([In] int xLeft, [In] int yTop);

			// Token: 0x06006D6F RID: 28015
			[TypeLibFunc(64)]
			[DispId(-5018)]
			void accDoDefaultAction([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x06006D70 RID: 28016
			[TypeLibFunc(64)]
			[DispId(-5003)]
			void set_accName([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild, [MarshalAs(UnmanagedType.BStr)] [In] string pszName);

			// Token: 0x06006D71 RID: 28017
			[TypeLibFunc(64)]
			[DispId(-5004)]
			void set_accValue([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild, [MarshalAs(UnmanagedType.BStr)] [In] string pszValue);
		}

		// Token: 0x020007DD RID: 2013
		[Guid("BEF6E002-A874-101A-8BBA-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IFont
		{
			// Token: 0x06006D72 RID: 28018
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetName();

			// Token: 0x06006D73 RID: 28019
			void SetName([MarshalAs(UnmanagedType.BStr)] [In] string pname);

			// Token: 0x06006D74 RID: 28020
			[return: MarshalAs(UnmanagedType.U8)]
			long GetSize();

			// Token: 0x06006D75 RID: 28021
			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long psize);

			// Token: 0x06006D76 RID: 28022
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetBold();

			// Token: 0x06006D77 RID: 28023
			void SetBold([MarshalAs(UnmanagedType.Bool)] [In] bool pbold);

			// Token: 0x06006D78 RID: 28024
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetItalic();

			// Token: 0x06006D79 RID: 28025
			void SetItalic([MarshalAs(UnmanagedType.Bool)] [In] bool pitalic);

			// Token: 0x06006D7A RID: 28026
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetUnderline();

			// Token: 0x06006D7B RID: 28027
			void SetUnderline([MarshalAs(UnmanagedType.Bool)] [In] bool punderline);

			// Token: 0x06006D7C RID: 28028
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetStrikethrough();

			// Token: 0x06006D7D RID: 28029
			void SetStrikethrough([MarshalAs(UnmanagedType.Bool)] [In] bool pstrikethrough);

			// Token: 0x06006D7E RID: 28030
			[return: MarshalAs(UnmanagedType.I2)]
			short GetWeight();

			// Token: 0x06006D7F RID: 28031
			void SetWeight([MarshalAs(UnmanagedType.I2)] [In] short pweight);

			// Token: 0x06006D80 RID: 28032
			[return: MarshalAs(UnmanagedType.I2)]
			short GetCharset();

			// Token: 0x06006D81 RID: 28033
			void SetCharset([MarshalAs(UnmanagedType.I2)] [In] short pcharset);

			// Token: 0x06006D82 RID: 28034
			IntPtr GetHFont();

			// Token: 0x06006D83 RID: 28035
			void Clone(out UnsafeNativeMethods.IFont ppfont);

			// Token: 0x06006D84 RID: 28036
			[PreserveSig]
			int IsEqual([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IFont pfontOther);

			// Token: 0x06006D85 RID: 28037
			void SetRatio(int cyLogical, int cyHimetric);

			// Token: 0x06006D86 RID: 28038
			void QueryTextMetrics(out IntPtr ptm);

			// Token: 0x06006D87 RID: 28039
			void AddRefHfont(IntPtr hFont);

			// Token: 0x06006D88 RID: 28040
			void ReleaseHfont(IntPtr hFont);

			// Token: 0x06006D89 RID: 28041
			void SetHdc(IntPtr hdc);
		}

		// Token: 0x020007DE RID: 2014
		[Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPicture
		{
			// Token: 0x06006D8A RID: 28042
			IntPtr GetHandle();

			// Token: 0x06006D8B RID: 28043
			IntPtr GetHPal();

			// Token: 0x06006D8C RID: 28044
			[return: MarshalAs(UnmanagedType.I2)]
			short GetPictureType();

			// Token: 0x06006D8D RID: 28045
			int GetWidth();

			// Token: 0x06006D8E RID: 28046
			int GetHeight();

			// Token: 0x06006D8F RID: 28047
			void Render(IntPtr hDC, int x, int y, int cx, int cy, int xSrc, int ySrc, int cxSrc, int cySrc, IntPtr rcBounds);

			// Token: 0x06006D90 RID: 28048
			void SetHPal(IntPtr phpal);

			// Token: 0x06006D91 RID: 28049
			IntPtr GetCurDC();

			// Token: 0x06006D92 RID: 28050
			void SelectPicture(IntPtr hdcIn, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] phdcOut, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] phbmpOut);

			// Token: 0x06006D93 RID: 28051
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetKeepOriginalFormat();

			// Token: 0x06006D94 RID: 28052
			void SetKeepOriginalFormat([MarshalAs(UnmanagedType.Bool)] [In] bool pfkeep);

			// Token: 0x06006D95 RID: 28053
			void PictureChanged();

			// Token: 0x06006D96 RID: 28054
			[PreserveSig]
			int SaveAsFile([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, int fSaveMemCopy, out int pcbSize);

			// Token: 0x06006D97 RID: 28055
			int GetAttributes();
		}

		// Token: 0x020007DF RID: 2015
		[Guid("7BF80981-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[ComImport]
		public interface IPictureDisp
		{
			// Token: 0x170017AF RID: 6063
			// (get) Token: 0x06006D98 RID: 28056
			IntPtr Handle { get; }

			// Token: 0x170017B0 RID: 6064
			// (get) Token: 0x06006D99 RID: 28057
			IntPtr HPal { get; }

			// Token: 0x170017B1 RID: 6065
			// (get) Token: 0x06006D9A RID: 28058
			short PictureType { get; }

			// Token: 0x170017B2 RID: 6066
			// (get) Token: 0x06006D9B RID: 28059
			int Width { get; }

			// Token: 0x170017B3 RID: 6067
			// (get) Token: 0x06006D9C RID: 28060
			int Height { get; }

			// Token: 0x06006D9D RID: 28061
			void Render(IntPtr hdc, int x, int y, int cx, int cy, int xSrc, int ySrc, int cxSrc, int cySrc);
		}

		// Token: 0x020007E0 RID: 2016
		[SuppressUnmanagedCodeSecurity]
		internal class ThemingScope
		{
			// Token: 0x06006D9E RID: 28062 RVA: 0x001926B4 File Offset: 0x001908B4
			private static bool IsContextActive()
			{
				IntPtr zero = IntPtr.Zero;
				return UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && UnsafeNativeMethods.ThemingScope.GetCurrentActCtx(out zero) && zero == UnsafeNativeMethods.ThemingScope.hActCtx;
			}

			// Token: 0x06006D9F RID: 28063 RVA: 0x001926E4 File Offset: 0x001908E4
			public static IntPtr Activate()
			{
				IntPtr zero = IntPtr.Zero;
				if (Application.UseVisualStyles && UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes) && !UnsafeNativeMethods.ThemingScope.IsContextActive() && !UnsafeNativeMethods.ThemingScope.ActivateActCtx(UnsafeNativeMethods.ThemingScope.hActCtx, out zero))
				{
					zero = IntPtr.Zero;
				}
				return zero;
			}

			// Token: 0x06006DA0 RID: 28064 RVA: 0x00192732 File Offset: 0x00190932
			public static IntPtr Deactivate(IntPtr userCookie)
			{
				if (userCookie != IntPtr.Zero && OSFeature.Feature.IsPresent(OSFeature.Themes) && UnsafeNativeMethods.ThemingScope.DeactivateActCtx(0, userCookie))
				{
					userCookie = IntPtr.Zero;
				}
				return userCookie;
			}

			// Token: 0x06006DA1 RID: 28065 RVA: 0x00192764 File Offset: 0x00190964
			public static bool CreateActivationContext(string dllPath, int nativeResourceManifestID)
			{
				Type typeFromHandle = typeof(UnsafeNativeMethods.ThemingScope);
				bool result;
				lock (typeFromHandle)
				{
					if (!UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes))
					{
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext = default(UnsafeNativeMethods.ThemingScope.ACTCTX);
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.ThemingScope.ACTCTX));
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.lpSource = dllPath;
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.lpResourceName = (IntPtr)nativeResourceManifestID;
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.dwFlags = 8U;
						UnsafeNativeMethods.ThemingScope.hActCtx = UnsafeNativeMethods.ThemingScope.CreateActCtx(ref UnsafeNativeMethods.ThemingScope.enableThemingActivationContext);
						UnsafeNativeMethods.ThemingScope.contextCreationSucceeded = (UnsafeNativeMethods.ThemingScope.hActCtx != new IntPtr(-1));
					}
					result = UnsafeNativeMethods.ThemingScope.contextCreationSucceeded;
				}
				return result;
			}

			// Token: 0x06006DA2 RID: 28066
			[DllImport("kernel32.dll")]
			private static extern IntPtr CreateActCtx(ref UnsafeNativeMethods.ThemingScope.ACTCTX actctx);

			// Token: 0x06006DA3 RID: 28067
			[DllImport("kernel32.dll")]
			private static extern bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);

			// Token: 0x06006DA4 RID: 28068
			[DllImport("kernel32.dll")]
			private static extern bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);

			// Token: 0x06006DA5 RID: 28069
			[DllImport("kernel32.dll")]
			private static extern bool GetCurrentActCtx(out IntPtr handle);

			// Token: 0x040041EF RID: 16879
			private static UnsafeNativeMethods.ThemingScope.ACTCTX enableThemingActivationContext;

			// Token: 0x040041F0 RID: 16880
			private static IntPtr hActCtx;

			// Token: 0x040041F1 RID: 16881
			private static bool contextCreationSucceeded;

			// Token: 0x040041F2 RID: 16882
			private const int ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 4;

			// Token: 0x040041F3 RID: 16883
			private const int ACTCTX_FLAG_RESOURCE_NAME_VALID = 8;

			// Token: 0x020008A7 RID: 2215
			private struct ACTCTX
			{
				// Token: 0x04004414 RID: 17428
				public int cbSize;

				// Token: 0x04004415 RID: 17429
				public uint dwFlags;

				// Token: 0x04004416 RID: 17430
				public string lpSource;

				// Token: 0x04004417 RID: 17431
				public ushort wProcessorArchitecture;

				// Token: 0x04004418 RID: 17432
				public ushort wLangId;

				// Token: 0x04004419 RID: 17433
				public string lpAssemblyDirectory;

				// Token: 0x0400441A RID: 17434
				public IntPtr lpResourceName;

				// Token: 0x0400441B RID: 17435
				public string lpApplicationName;
			}
		}

		// Token: 0x020007E1 RID: 2017
		[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential)]
		internal class PROCESS_INFORMATION
		{
			// Token: 0x06006DA7 RID: 28071 RVA: 0x00192830 File Offset: 0x00190A30
			~PROCESS_INFORMATION()
			{
				this.Close();
			}

			// Token: 0x06006DA8 RID: 28072 RVA: 0x0019285C File Offset: 0x00190A5C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void Close()
			{
				if (this.hProcess != (IntPtr)0 && this.hProcess != UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE)
				{
					UnsafeNativeMethods.PROCESS_INFORMATION.CloseHandle(new HandleRef(this, this.hProcess));
					this.hProcess = UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE;
				}
				if (this.hThread != (IntPtr)0 && this.hThread != UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE)
				{
					UnsafeNativeMethods.PROCESS_INFORMATION.CloseHandle(new HandleRef(this, this.hThread));
					this.hThread = UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE;
				}
			}

			// Token: 0x06006DA9 RID: 28073
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			private static extern bool CloseHandle(HandleRef handle);

			// Token: 0x040041F4 RID: 16884
			public IntPtr hProcess = IntPtr.Zero;

			// Token: 0x040041F5 RID: 16885
			public IntPtr hThread = IntPtr.Zero;

			// Token: 0x040041F6 RID: 16886
			public int dwProcessId;

			// Token: 0x040041F7 RID: 16887
			public int dwThreadId;

			// Token: 0x040041F8 RID: 16888
			private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
		}

		// Token: 0x020007E2 RID: 2018
		[ComVisible(true)]
		[Guid("e4cfef41-071d-472c-a65c-c14f59ea81eb")]
		public enum StructureChangeType
		{
			// Token: 0x040041FA RID: 16890
			ChildAdded,
			// Token: 0x040041FB RID: 16891
			ChildRemoved,
			// Token: 0x040041FC RID: 16892
			ChildrenInvalidated,
			// Token: 0x040041FD RID: 16893
			ChildrenBulkAdded,
			// Token: 0x040041FE RID: 16894
			ChildrenBulkRemoved,
			// Token: 0x040041FF RID: 16895
			ChildrenReordered
		}

		// Token: 0x020007E3 RID: 2019
		[ComVisible(true)]
		[Guid("76d12d7e-b227-4417-9ce2-42642ffa896a")]
		public enum ExpandCollapseState
		{
			// Token: 0x04004201 RID: 16897
			Collapsed,
			// Token: 0x04004202 RID: 16898
			Expanded,
			// Token: 0x04004203 RID: 16899
			PartiallyExpanded,
			// Token: 0x04004204 RID: 16900
			LeafNode
		}

		// Token: 0x020007E4 RID: 2020
		[Flags]
		public enum ProviderOptions
		{
			// Token: 0x04004206 RID: 16902
			ClientSideProvider = 1,
			// Token: 0x04004207 RID: 16903
			ServerSideProvider = 2,
			// Token: 0x04004208 RID: 16904
			NonClientAreaProvider = 4,
			// Token: 0x04004209 RID: 16905
			OverrideProvider = 8,
			// Token: 0x0400420A RID: 16906
			ProviderOwnsSetFocus = 16,
			// Token: 0x0400420B RID: 16907
			UseComThreading = 32
		}

		// Token: 0x020007E5 RID: 2021
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("fb8b03af-3bdf-48d4-bd36-1a65793be168")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ISelectionProvider
		{
			// Token: 0x06006DAC RID: 28076
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetSelection();

			// Token: 0x170017B4 RID: 6068
			// (get) Token: 0x06006DAD RID: 28077
			bool CanSelectMultiple { [return: MarshalAs(UnmanagedType.Bool)] get; }

			// Token: 0x170017B5 RID: 6069
			// (get) Token: 0x06006DAE RID: 28078
			bool IsSelectionRequired { [return: MarshalAs(UnmanagedType.Bool)] get; }
		}

		// Token: 0x020007E6 RID: 2022
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("2acad808-b2d4-452d-a407-91ff1ad167b2")]
		[ComImport]
		public interface ISelectionItemProvider
		{
			// Token: 0x06006DAF RID: 28079
			void Select();

			// Token: 0x06006DB0 RID: 28080
			void AddToSelection();

			// Token: 0x06006DB1 RID: 28081
			void RemoveFromSelection();

			// Token: 0x170017B6 RID: 6070
			// (get) Token: 0x06006DB2 RID: 28082
			bool IsSelected { [return: MarshalAs(UnmanagedType.Bool)] get; }

			// Token: 0x170017B7 RID: 6071
			// (get) Token: 0x06006DB3 RID: 28083
			UnsafeNativeMethods.IRawElementProviderSimple SelectionContainer { [return: MarshalAs(UnmanagedType.Interface)] get; }
		}

		// Token: 0x020007E7 RID: 2023
		[ComVisible(true)]
		[Guid("1d5df27c-8947-4425-b8d9-79787bb460b8")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IRawElementProviderHwndOverride : UnsafeNativeMethods.IRawElementProviderSimple
		{
			// Token: 0x06006DB4 RID: 28084
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IRawElementProviderSimple GetOverrideProviderForHwnd(IntPtr hwnd);
		}

		// Token: 0x020007E8 RID: 2024
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IServiceProvider
		{
			// Token: 0x06006DB5 RID: 28085
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[PreserveSig]
			int QueryService(ref Guid service, ref Guid riid, out IntPtr ppvObj);
		}

		// Token: 0x020007E9 RID: 2025
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[Guid("F8B80ADA-2C44-48D0-89BE-5FF23C9CD875")]
		[ComImport]
		internal interface IAccessibleEx
		{
			// Token: 0x06006DB6 RID: 28086
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object GetObjectForChild(int idChild);

			// Token: 0x06006DB7 RID: 28087
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetIAccessiblePair([MarshalAs(UnmanagedType.Interface)] out object ppAcc, out int pidChild);

			// Token: 0x06006DB8 RID: 28088
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)]
			int[] GetRuntimeId();

			// Token: 0x06006DB9 RID: 28089
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ConvertReturnedElement([MarshalAs(UnmanagedType.Interface)] [In] object pIn, [MarshalAs(UnmanagedType.Interface)] out object ppRetValOut);
		}

		// Token: 0x020007EA RID: 2026
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("d847d3a5-cab0-4a98-8c32-ecb45c59ad24")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IExpandCollapseProvider
		{
			// Token: 0x06006DBA RID: 28090
			void Expand();

			// Token: 0x06006DBB RID: 28091
			void Collapse();

			// Token: 0x170017B8 RID: 6072
			// (get) Token: 0x06006DBC RID: 28092
			UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState { get; }
		}

		// Token: 0x020007EB RID: 2027
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("c7935180-6fb3-4201-b174-7df73adbf64a")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IValueProvider
		{
			// Token: 0x06006DBD RID: 28093
			void SetValue([MarshalAs(UnmanagedType.LPWStr)] string value);

			// Token: 0x170017B9 RID: 6073
			// (get) Token: 0x06006DBE RID: 28094
			string Value { get; }

			// Token: 0x170017BA RID: 6074
			// (get) Token: 0x06006DBF RID: 28095
			bool IsReadOnly { [return: MarshalAs(UnmanagedType.Bool)] get; }
		}

		// Token: 0x020007EC RID: 2028
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("36dc7aef-33e6-4691-afe1-2be7274b3d33")]
		[ComImport]
		public interface IRangeValueProvider
		{
			// Token: 0x06006DC0 RID: 28096
			void SetValue(double value);

			// Token: 0x170017BB RID: 6075
			// (get) Token: 0x06006DC1 RID: 28097
			double Value { get; }

			// Token: 0x170017BC RID: 6076
			// (get) Token: 0x06006DC2 RID: 28098
			bool IsReadOnly { [return: MarshalAs(UnmanagedType.Bool)] get; }

			// Token: 0x170017BD RID: 6077
			// (get) Token: 0x06006DC3 RID: 28099
			double Maximum { get; }

			// Token: 0x170017BE RID: 6078
			// (get) Token: 0x06006DC4 RID: 28100
			double Minimum { get; }

			// Token: 0x170017BF RID: 6079
			// (get) Token: 0x06006DC5 RID: 28101
			double LargeChange { get; }

			// Token: 0x170017C0 RID: 6080
			// (get) Token: 0x06006DC6 RID: 28102
			double SmallChange { get; }
		}

		// Token: 0x020007ED RID: 2029
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("D6DD68D1-86FD-4332-8666-9ABEDEA2D24C")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IRawElementProviderSimple
		{
			// Token: 0x170017C1 RID: 6081
			// (get) Token: 0x06006DC7 RID: 28103
			UnsafeNativeMethods.ProviderOptions ProviderOptions { get; }

			// Token: 0x06006DC8 RID: 28104
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object GetPatternProvider(int patternId);

			// Token: 0x06006DC9 RID: 28105
			object GetPropertyValue(int propertyId);

			// Token: 0x170017C2 RID: 6082
			// (get) Token: 0x06006DCA RID: 28106
			UnsafeNativeMethods.IRawElementProviderSimple HostRawElementProvider { get; }
		}

		// Token: 0x020007EE RID: 2030
		[ComVisible(true)]
		[Guid("670c3006-bf4c-428b-8534-e1848f645122")]
		public enum NavigateDirection
		{
			// Token: 0x0400420D RID: 16909
			Parent,
			// Token: 0x0400420E RID: 16910
			NextSibling,
			// Token: 0x0400420F RID: 16911
			PreviousSibling,
			// Token: 0x04004210 RID: 16912
			FirstChild,
			// Token: 0x04004211 RID: 16913
			LastChild
		}

		// Token: 0x020007EF RID: 2031
		[ComVisible(true)]
		[Guid("f7063da8-8359-439c-9297-bbc5299a7d87")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IRawElementProviderFragment : UnsafeNativeMethods.IRawElementProviderSimple
		{
			// Token: 0x06006DCB RID: 28107
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object Navigate(UnsafeNativeMethods.NavigateDirection direction);

			// Token: 0x06006DCC RID: 28108
			int[] GetRuntimeId();

			// Token: 0x170017C3 RID: 6083
			// (get) Token: 0x06006DCD RID: 28109
			NativeMethods.UiaRect BoundingRectangle { get; }

			// Token: 0x06006DCE RID: 28110
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetEmbeddedFragmentRoots();

			// Token: 0x06006DCF RID: 28111
			void SetFocus();

			// Token: 0x170017C4 RID: 6084
			// (get) Token: 0x06006DD0 RID: 28112
			UnsafeNativeMethods.IRawElementProviderFragmentRoot FragmentRoot { [return: MarshalAs(UnmanagedType.Interface)] get; }
		}

		// Token: 0x020007F0 RID: 2032
		[ComVisible(true)]
		[Guid("620ce2a5-ab8f-40a9-86cb-de3c75599b58")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IRawElementProviderFragmentRoot : UnsafeNativeMethods.IRawElementProviderFragment, UnsafeNativeMethods.IRawElementProviderSimple
		{
			// Token: 0x06006DD1 RID: 28113
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object ElementProviderFromPoint(double x, double y);

			// Token: 0x06006DD2 RID: 28114
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object GetFocus();
		}

		// Token: 0x020007F1 RID: 2033
		[Flags]
		public enum ToggleState
		{
			// Token: 0x04004213 RID: 16915
			ToggleState_Off = 0,
			// Token: 0x04004214 RID: 16916
			ToggleState_On = 1,
			// Token: 0x04004215 RID: 16917
			ToggleState_Indeterminate = 2
		}

		// Token: 0x020007F2 RID: 2034
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("56D00BD0-C4F4-433C-A836-1A52A57E0892")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IToggleProvider
		{
			// Token: 0x06006DD3 RID: 28115
			void Toggle();

			// Token: 0x170017C5 RID: 6085
			// (get) Token: 0x06006DD4 RID: 28116
			UnsafeNativeMethods.ToggleState ToggleState { get; }
		}

		// Token: 0x020007F3 RID: 2035
		[Flags]
		public enum RowOrColumnMajor
		{
			// Token: 0x04004217 RID: 16919
			RowOrColumnMajor_RowMajor = 0,
			// Token: 0x04004218 RID: 16920
			RowOrColumnMajor_ColumnMajor = 1,
			// Token: 0x04004219 RID: 16921
			RowOrColumnMajor_Indeterminate = 2
		}

		// Token: 0x020007F4 RID: 2036
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("9c860395-97b3-490a-b52a-858cc22af166")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface ITableProvider
		{
			// Token: 0x06006DD5 RID: 28117
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetRowHeaders();

			// Token: 0x06006DD6 RID: 28118
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetColumnHeaders();

			// Token: 0x170017C6 RID: 6086
			// (get) Token: 0x06006DD7 RID: 28119
			UnsafeNativeMethods.RowOrColumnMajor RowOrColumnMajor { get; }
		}

		// Token: 0x020007F5 RID: 2037
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("b9734fa6-771f-4d78-9c90-2517999349cd")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface ITableItemProvider
		{
			// Token: 0x06006DD8 RID: 28120
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetRowHeaderItems();

			// Token: 0x06006DD9 RID: 28121
			[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)]
			object[] GetColumnHeaderItems();
		}

		// Token: 0x020007F6 RID: 2038
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("b17d6187-0907-464b-a168-0ef17a1572b1")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IGridProvider
		{
			// Token: 0x06006DDA RID: 28122
			[return: MarshalAs(UnmanagedType.IUnknown)]
			object GetItem(int row, int column);

			// Token: 0x170017C7 RID: 6087
			// (get) Token: 0x06006DDB RID: 28123
			int RowCount { [return: MarshalAs(UnmanagedType.I4)] get; }

			// Token: 0x170017C8 RID: 6088
			// (get) Token: 0x06006DDC RID: 28124
			int ColumnCount { [return: MarshalAs(UnmanagedType.I4)] get; }
		}

		// Token: 0x020007F7 RID: 2039
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("d02541f1-fb81-4d64-ae32-f520f8a6dbd1")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IGridItemProvider
		{
			// Token: 0x170017C9 RID: 6089
			// (get) Token: 0x06006DDD RID: 28125
			int Row { [return: MarshalAs(UnmanagedType.I4)] get; }

			// Token: 0x170017CA RID: 6090
			// (get) Token: 0x06006DDE RID: 28126
			int Column { [return: MarshalAs(UnmanagedType.I4)] get; }

			// Token: 0x170017CB RID: 6091
			// (get) Token: 0x06006DDF RID: 28127
			int RowSpan { [return: MarshalAs(UnmanagedType.I4)] get; }

			// Token: 0x170017CC RID: 6092
			// (get) Token: 0x06006DE0 RID: 28128
			int ColumnSpan { [return: MarshalAs(UnmanagedType.I4)] get; }

			// Token: 0x170017CD RID: 6093
			// (get) Token: 0x06006DE1 RID: 28129
			UnsafeNativeMethods.IRawElementProviderSimple ContainingGrid { [return: MarshalAs(UnmanagedType.Interface)] get; }
		}

		// Token: 0x020007F8 RID: 2040
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("54fcb24b-e18e-47a2-b4d3-eccbe77599a2")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IInvokeProvider
		{
			// Token: 0x06006DE2 RID: 28130
			void Invoke();
		}

		// Token: 0x020007F9 RID: 2041
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComVisible(true)]
		[Guid("2360c714-4bf1-4b26-ba65-9b21316127eb")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[CLSCompliant(false)]
		[ComImport]
		public interface IScrollItemProvider
		{
			// Token: 0x06006DE3 RID: 28131
			void ScrollIntoView();
		}
	}
}
