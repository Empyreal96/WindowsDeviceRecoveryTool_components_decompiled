using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B9 RID: 1977
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	[ComImport]
	internal interface IObjectWithProgId
	{
		// Token: 0x06007B20 RID: 31520
		void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

		// Token: 0x06007B21 RID: 31521
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetProgID();
	}
}
