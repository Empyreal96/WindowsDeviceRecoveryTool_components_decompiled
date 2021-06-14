using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200006A RID: 106
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("631F7D96-D993-11D2-B339-00105A1F4AAF")]
	[ComImport]
	internal interface IWbemEventProviderSecurity
	{
		// Token: 0x0600041C RID: 1052
		[PreserveSig]
		int AccessCheck_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery, [In] int lSidLength, [In] ref byte pSid);
	}
}
