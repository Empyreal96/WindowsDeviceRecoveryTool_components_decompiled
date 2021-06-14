using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200007B RID: 123
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectArray
	{
		// Token: 0x06000125 RID: 293
		uint GetCount();

		// Token: 0x06000126 RID: 294
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);
	}
}
