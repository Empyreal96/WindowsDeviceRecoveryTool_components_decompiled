using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AF RID: 1967
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellLinkW
	{
		// Token: 0x06007AA7 RID: 31399
		void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, [In] [Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		// Token: 0x06007AA8 RID: 31400
		IntPtr GetIDList();

		// Token: 0x06007AA9 RID: 31401
		void SetIDList(IntPtr pidl);

		// Token: 0x06007AAA RID: 31402
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxName);

		// Token: 0x06007AAB RID: 31403
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06007AAC RID: 31404
		void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

		// Token: 0x06007AAD RID: 31405
		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		// Token: 0x06007AAE RID: 31406
		void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

		// Token: 0x06007AAF RID: 31407
		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		// Token: 0x06007AB0 RID: 31408
		short GetHotKey();

		// Token: 0x06007AB1 RID: 31409
		void SetHotKey(short wHotKey);

		// Token: 0x06007AB2 RID: 31410
		uint GetShowCmd();

		// Token: 0x06007AB3 RID: 31411
		void SetShowCmd(uint iShowCmd);

		// Token: 0x06007AB4 RID: 31412
		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		// Token: 0x06007AB5 RID: 31413
		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		// Token: 0x06007AB6 RID: 31414
		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		// Token: 0x06007AB7 RID: 31415
		void Resolve(IntPtr hwnd, uint fFlags);

		// Token: 0x06007AB8 RID: 31416
		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
