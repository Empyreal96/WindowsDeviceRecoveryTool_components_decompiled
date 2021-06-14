using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AE RID: 1966
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	[ComImport]
	internal interface IShellItemArray
	{
		// Token: 0x06007AA0 RID: 31392
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid rbhid, [In] ref Guid riid);

		// Token: 0x06007AA1 RID: 31393
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(int flags, [In] ref Guid riid);

		// Token: 0x06007AA2 RID: 31394
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList([In] ref PKEY keyType, [In] ref Guid riid);

		// Token: 0x06007AA3 RID: 31395
		uint GetAttributes(SIATTRIBFLAGS dwAttribFlags, uint sfgaoMask);

		// Token: 0x06007AA4 RID: 31396
		uint GetCount();

		// Token: 0x06007AA5 RID: 31397
		IShellItem GetItemAt(uint dwIndex);

		// Token: 0x06007AA6 RID: 31398
		[return: MarshalAs(UnmanagedType.Interface)]
		object EnumItems();
	}
}
