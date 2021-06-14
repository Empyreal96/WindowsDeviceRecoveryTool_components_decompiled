using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200006F RID: 111
	[InterfaceType(1)]
	[Guid("1005CBCF-E64F-4646-BCD3-3A089D8A84B4")]
	[ComImport]
	internal interface IWbemDecoupledRegistrar
	{
		// Token: 0x06000421 RID: 1057
		[PreserveSig]
		int Register_([In] int flags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext context, [MarshalAs(UnmanagedType.LPWStr)] [In] string user, [MarshalAs(UnmanagedType.LPWStr)] [In] string locale, [MarshalAs(UnmanagedType.LPWStr)] [In] string scope, [MarshalAs(UnmanagedType.LPWStr)] [In] string registration, [MarshalAs(UnmanagedType.IUnknown)] [In] object unknown);

		// Token: 0x06000422 RID: 1058
		[PreserveSig]
		int UnRegister_();
	}
}
