using System;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C4 RID: 196
	[Guid("809c652e-7396-11d2-9771-00a0c9b4d50c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibType(TypeLibTypeFlags.FRestricted)]
	[ComImport]
	internal interface IMetaDataDispenser
	{
		// Token: 0x0600056D RID: 1389
		[return: MarshalAs(UnmanagedType.Interface)]
		object DefineScope([In] ref Guid rclsid, [In] uint dwCreateFlags, [In] ref Guid riid);

		// Token: 0x0600056E RID: 1390
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScope([MarshalAs(UnmanagedType.LPWStr)] [In] string szScope, [In] uint dwOpenFlags, [In] ref Guid riid);

		// Token: 0x0600056F RID: 1391
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScopeOnMemory([In] IntPtr pData, [In] uint cbData, [In] uint dwOpenFlags, [In] ref Guid riid);
	}
}
