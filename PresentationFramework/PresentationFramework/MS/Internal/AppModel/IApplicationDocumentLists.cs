using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B6 RID: 1974
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
	[ComImport]
	internal interface IApplicationDocumentLists
	{
		// Token: 0x06007B13 RID: 31507
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B14 RID: 31508
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetList(ADLT listtype, uint cItemsDesired, [In] ref Guid riid);
	}
}
