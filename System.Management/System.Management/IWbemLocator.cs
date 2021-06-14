using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200005A RID: 90
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("DC12A687-737F-11CF-884D-00AA004B2E24")]
	[ComImport]
	internal interface IWbemLocator
	{
		// Token: 0x060003AB RID: 939
		[PreserveSig]
		int ConnectServer_([MarshalAs(UnmanagedType.BStr)] [In] string strNetworkResource, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strLocale, [In] int lSecurityFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IWbemServices ppNamespace);
	}
}
