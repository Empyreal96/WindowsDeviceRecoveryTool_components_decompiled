using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Nokia.Lucid.Interop.Win32Types;

namespace Nokia.Lucid.Interop
{
	// Token: 0x02000033 RID: 51
	internal static class User32NativeMethods
	{
		// Token: 0x0600014D RID: 333
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		// Token: 0x0600014E RID: 334
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern int GetQueueStatus(int flags);

		// Token: 0x0600014F RID: 335
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000150 RID: 336
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PeekMessage(out MSG lpMsg, HandleRef hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);

		// Token: 0x06000151 RID: 337
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PostMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000152 RID: 338
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern void PostQuitMessage(int nExitCode);

		// Token: 0x06000153 RID: 339
		[DllImport("user32.dll", ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool TranslateMessage(ref MSG lpMsg);

		// Token: 0x06000154 RID: 340
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		public static extern IntPtr DispatchMessage(ref MSG lpmsg);

		// Token: 0x06000155 RID: 341
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		// Token: 0x06000156 RID: 342
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(HandleRef hWnd);

		// Token: 0x06000157 RID: 343
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern WNDPROC SetWindowLong(IntPtr hWnd, int nIndex, WNDPROC dwNewLong);

		// Token: 0x06000158 RID: 344
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern WNDPROC SetWindowLongPtr(IntPtr hWnd, int nIndex, WNDPROC dwNewLong);

		// Token: 0x06000159 RID: 345
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern IntPtr RegisterDeviceNotification(HandleRef hRecipient, ref DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, int Flags);

		// Token: 0x0600015A RID: 346
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterDeviceNotification(IntPtr Handle);

		// Token: 0x0600015B RID: 347
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern short RegisterClassEx(ref WNDCLASSEX lpwcx);

		// Token: 0x0600015C RID: 348
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

		// Token: 0x040000C7 RID: 199
		private const string User32DllName = "user32.dll";
	}
}
