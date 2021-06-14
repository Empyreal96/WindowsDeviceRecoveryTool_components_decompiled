using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BF RID: 1983
	internal static class NativeMethods2
	{
		// Token: 0x06007B55 RID: 31573
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocsString(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		// Token: 0x06007B56 RID: 31574
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		// Token: 0x06007B57 RID: 31575 RVA: 0x0022B234 File Offset: 0x00229434
		[SecurityCritical]
		internal static void SHAddToRecentDocs(string path)
		{
			NativeMethods2.SHAddToRecentDocsString(SHARD.PATHW, path);
		}

		// Token: 0x06007B58 RID: 31576 RVA: 0x0022B23D File Offset: 0x0022943D
		[SecurityCritical]
		internal static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			NativeMethods2.SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		// Token: 0x06007B59 RID: 31577
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06007B5A RID: 31578
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHGetFolderPathEx([In] ref Guid rfid, KF_FLAG dwFlags, [In] [Optional] IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszPath, uint cchPath);

		// Token: 0x06007B5B RID: 31579
		[DllImport("shell32.dll", PreserveSig = false)]
		internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		// Token: 0x06007B5C RID: 31580
		[DllImport("shell32.dll")]
		internal static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
