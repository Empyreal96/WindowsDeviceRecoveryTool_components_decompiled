using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Standard
{
	// Token: 0x02000082 RID: 130
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellLinkW
	{
		// Token: 0x0600015A RID: 346
		void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, [In] [Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		// Token: 0x0600015B RID: 347
		void GetIDList(out IntPtr ppidl);

		// Token: 0x0600015C RID: 348
		void SetIDList(IntPtr pidl);

		// Token: 0x0600015D RID: 349
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxName);

		// Token: 0x0600015E RID: 350
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x0600015F RID: 351
		void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

		// Token: 0x06000160 RID: 352
		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		// Token: 0x06000161 RID: 353
		void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

		// Token: 0x06000162 RID: 354
		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		// Token: 0x06000163 RID: 355
		short GetHotKey();

		// Token: 0x06000164 RID: 356
		void SetHotKey(short wHotKey);

		// Token: 0x06000165 RID: 357
		uint GetShowCmd();

		// Token: 0x06000166 RID: 358
		void SetShowCmd(uint iShowCmd);

		// Token: 0x06000167 RID: 359
		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		// Token: 0x06000168 RID: 360
		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		// Token: 0x06000169 RID: 361
		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		// Token: 0x0600016A RID: 362
		void Resolve(IntPtr hwnd, uint fFlags);

		// Token: 0x0600016B RID: 363
		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
