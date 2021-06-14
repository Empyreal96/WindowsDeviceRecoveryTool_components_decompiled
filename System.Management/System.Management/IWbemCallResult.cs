using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200005E RID: 94
	[Guid("44ACA675-E8FC-11D0-A07C-00C04FB68820")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemCallResult
	{
		// Token: 0x060003E3 RID: 995
		[PreserveSig]
		int GetResultObject_([In] int lTimeout, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Management.MarshalWbemObject)] out IWbemClassObjectFreeThreaded ppResultObject);

		// Token: 0x060003E4 RID: 996
		[PreserveSig]
		int GetResultString_([In] int lTimeout, [MarshalAs(UnmanagedType.BStr)] out string pstrResultString);

		// Token: 0x060003E5 RID: 997
		[PreserveSig]
		int GetResultServices_([In] int lTimeout, [MarshalAs(UnmanagedType.Interface)] out IWbemServices ppServices);

		// Token: 0x060003E6 RID: 998
		[PreserveSig]
		int GetCallStatus_([In] int lTimeout, out int plStatus);
	}
}
