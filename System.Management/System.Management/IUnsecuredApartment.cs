using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000064 RID: 100
	[Guid("1CFABA8C-1523-11D1-AD79-00C04FD8FDFF")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IUnsecuredApartment
	{
		// Token: 0x06000413 RID: 1043
		[PreserveSig]
		int CreateObjectStub_([MarshalAs(UnmanagedType.IUnknown)] [In] object pObject, [MarshalAs(UnmanagedType.IUnknown)] out object ppStub);
	}
}
