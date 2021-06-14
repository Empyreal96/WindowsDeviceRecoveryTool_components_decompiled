using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200067F RID: 1663
	[ComVisible(true)]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistFile
	{
		// Token: 0x06006D03 RID: 27907
		void GetClassID(out Guid pClassID);

		// Token: 0x06006D04 RID: 27908
		[PreserveSig]
		int IsDirty();

		// Token: 0x06006D05 RID: 27909
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06006D06 RID: 27910
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06006D07 RID: 27911
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x06006D08 RID: 27912
		[PreserveSig]
		int GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}
