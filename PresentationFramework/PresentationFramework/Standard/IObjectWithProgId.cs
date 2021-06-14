using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000089 RID: 137
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	[ComImport]
	internal interface IObjectWithProgId
	{
		// Token: 0x06000187 RID: 391
		void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

		// Token: 0x06000188 RID: 392
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetProgID();
	}
}
