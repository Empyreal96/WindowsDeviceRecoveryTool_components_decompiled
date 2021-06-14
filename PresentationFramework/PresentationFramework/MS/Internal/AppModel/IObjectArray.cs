using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A8 RID: 1960
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectArray
	{
		// Token: 0x06007A72 RID: 31346
		uint GetCount();

		// Token: 0x06007A73 RID: 31347
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);
	}
}
