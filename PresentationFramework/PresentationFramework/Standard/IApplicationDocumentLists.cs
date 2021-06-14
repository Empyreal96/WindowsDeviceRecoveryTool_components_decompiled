using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000086 RID: 134
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3c594f9f-9f30-47a1-979a-c9e83d3d0a06")]
	[ComImport]
	internal interface IApplicationDocumentLists
	{
		// Token: 0x0600017A RID: 378
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x0600017B RID: 379
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetList([In] APPDOCLISTTYPE listtype, [In] uint cItemsDesired, [In] ref Guid riid);
	}
}
