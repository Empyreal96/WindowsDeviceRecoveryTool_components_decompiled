using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Printing
{
	// Token: 0x02000657 RID: 1623
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06006BCE RID: 27598
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
		internal static extern int PrintDlgEx(IntPtr pdex);

		// Token: 0x06006BCF RID: 27599
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalFree(IntPtr hMem);

		// Token: 0x06006BD0 RID: 27600
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalLock(IntPtr hMem);

		// Token: 0x06006BD1 RID: 27601
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern bool GlobalUnlock(IntPtr hMem);
	}
}
