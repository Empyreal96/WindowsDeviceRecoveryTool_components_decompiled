using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x02000080 RID: 128
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	[ComImport]
	internal interface IShellItemArray
	{
		// Token: 0x06000141 RID: 321
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

		// Token: 0x06000142 RID: 322
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(int flags, [In] ref Guid riid);

		// Token: 0x06000143 RID: 323
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

		// Token: 0x06000144 RID: 324
		uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

		// Token: 0x06000145 RID: 325
		uint GetCount();

		// Token: 0x06000146 RID: 326
		IShellItem GetItemAt(uint dwIndex);

		// Token: 0x06000147 RID: 327
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumItems();
	}
}
