using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B1 RID: 1969
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
	[ComImport]
	internal interface IModalWindow
	{
		// Token: 0x06007AC0 RID: 31424
		[PreserveSig]
		HRESULT Show(IntPtr parent);
	}
}
