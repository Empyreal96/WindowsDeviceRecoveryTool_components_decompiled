using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200006E RID: 110
	[Guid("1BE41572-91DD-11D1-AEB2-00C04FB68820")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemProviderInit
	{
		// Token: 0x06000420 RID: 1056
		[PreserveSig]
		int Initialize_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszUser, [In] int lFlags, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszLocale, [MarshalAs(UnmanagedType.Interface)] [In] IWbemServices pNamespace, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemProviderInitSink pInitSink);
	}
}
