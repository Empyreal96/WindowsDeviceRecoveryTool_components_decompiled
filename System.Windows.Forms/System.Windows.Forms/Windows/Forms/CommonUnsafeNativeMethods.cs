using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	// Token: 0x02000104 RID: 260
	[SuppressUnmanagedCodeSecurity]
	internal class CommonUnsafeNativeMethods
	{
		// Token: 0x0600043E RID: 1086
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(HandleRef hModule, string lpProcName);

		// Token: 0x0600043F RID: 1087
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string modName);

		// Token: 0x06000440 RID: 1088
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibraryEx(string lpModuleName, IntPtr hFile, uint dwFlags);

		// Token: 0x06000441 RID: 1089
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string libname);

		// Token: 0x06000442 RID: 1090
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool FreeLibrary(HandleRef hModule);

		// Token: 0x06000443 RID: 1091 RVA: 0x0000D854 File Offset: 0x0000BA54
		public static IntPtr LoadLibraryFromSystemPathIfAvailable(string libraryName)
		{
			IntPtr result = IntPtr.Zero;
			IntPtr moduleHandle = CommonUnsafeNativeMethods.GetModuleHandle("kernel32.dll");
			if (moduleHandle != IntPtr.Zero)
			{
				if (CommonUnsafeNativeMethods.GetProcAddress(new HandleRef(null, moduleHandle), "AddDllDirectory") != IntPtr.Zero)
				{
					result = CommonUnsafeNativeMethods.LoadLibraryEx(libraryName, IntPtr.Zero, 2048U);
				}
				else
				{
					result = CommonUnsafeNativeMethods.LoadLibrary(libraryName);
				}
			}
			return result;
		}

		// Token: 0x06000444 RID: 1092
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern DpiAwarenessContext GetThreadDpiAwarenessContext();

		// Token: 0x06000445 RID: 1093
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hWnd);

		// Token: 0x06000446 RID: 1094
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern CommonUnsafeNativeMethods.DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(IntPtr dpiAwarenessContext);

		// Token: 0x06000447 RID: 1095
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern DpiAwarenessContext SetThreadDpiAwarenessContext(DpiAwarenessContext dpiContext);

		// Token: 0x06000448 RID: 1096
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool AreDpiAwarenessContextsEqual(DpiAwarenessContext dpiContextA, DpiAwarenessContext dpiContextB);

		// Token: 0x06000449 RID: 1097 RVA: 0x0000D8B7 File Offset: 0x0000BAB7
		public static bool TryFindDpiAwarenessContextsEqual(DpiAwarenessContext dpiContextA, DpiAwarenessContext dpiContextB)
		{
			return (dpiContextA == DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED && dpiContextB == DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED) || (ApiHelper.IsApiAvailable("user32.dll", "AreDpiAwarenessContextsEqual") && CommonUnsafeNativeMethods.AreDpiAwarenessContextsEqual(dpiContextA, dpiContextB));
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000D8DB File Offset: 0x0000BADB
		public static DpiAwarenessContext TryGetThreadDpiAwarenessContext()
		{
			if (ApiHelper.IsApiAvailable("user32.dll", "GetThreadDpiAwarenessContext"))
			{
				return CommonUnsafeNativeMethods.GetThreadDpiAwarenessContext();
			}
			return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000D8F5 File Offset: 0x0000BAF5
		public static DpiAwarenessContext TrySetThreadDpiAwarenessContext(DpiAwarenessContext dpiCOntext)
		{
			if (ApiHelper.IsApiAvailable("user32.dll", "SetThreadDpiAwarenessContext"))
			{
				return CommonUnsafeNativeMethods.SetThreadDpiAwarenessContext(dpiCOntext);
			}
			return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000D910 File Offset: 0x0000BB10
		internal static DpiAwarenessContext TryGetDpiAwarenessContextForWindow(IntPtr hWnd)
		{
			DpiAwarenessContext result = DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNSPECIFIED;
			try
			{
				if (ApiHelper.IsApiAvailable("user32.dll", "GetWindowDpiAwarenessContext") && ApiHelper.IsApiAvailable("user32.dll", "GetAwarenessFromDpiAwarenessContext"))
				{
					IntPtr windowDpiAwarenessContext = CommonUnsafeNativeMethods.GetWindowDpiAwarenessContext(hWnd);
					CommonUnsafeNativeMethods.DPI_AWARENESS awarenessFromDpiAwarenessContext = CommonUnsafeNativeMethods.GetAwarenessFromDpiAwarenessContext(windowDpiAwarenessContext);
					result = CommonUnsafeNativeMethods.ConvertToDpiAwarenessContext(awarenessFromDpiAwarenessContext);
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000D96C File Offset: 0x0000BB6C
		private static DpiAwarenessContext ConvertToDpiAwarenessContext(CommonUnsafeNativeMethods.DPI_AWARENESS dpiAwareness)
		{
			switch (dpiAwareness)
			{
			case CommonUnsafeNativeMethods.DPI_AWARENESS.DPI_AWARENESS_UNAWARE:
				return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_UNAWARE;
			case CommonUnsafeNativeMethods.DPI_AWARENESS.DPI_AWARENESS_SYSTEM_AWARE:
				return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE;
			case CommonUnsafeNativeMethods.DPI_AWARENESS.DPI_AWARENESS_PER_MONITOR_AWARE:
				return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2;
			default:
				return DpiAwarenessContext.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE;
			}
		}

		// Token: 0x0400044E RID: 1102
		internal const int LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048;

		// Token: 0x02000541 RID: 1345
		internal enum DPI_AWARENESS
		{
			// Token: 0x04003764 RID: 14180
			DPI_AWARENESS_INVALID = -1,
			// Token: 0x04003765 RID: 14181
			DPI_AWARENESS_UNAWARE,
			// Token: 0x04003766 RID: 14182
			DPI_AWARENESS_SYSTEM_AWARE,
			// Token: 0x04003767 RID: 14183
			DPI_AWARENESS_PER_MONITOR_AWARE
		}
	}
}
