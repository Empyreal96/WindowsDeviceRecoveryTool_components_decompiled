using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000065 RID: 101
	[Guid("EB87E1BC-3233-11D2-AEC9-00C04FB68820")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemStatusCodeText
	{
		// Token: 0x06000414 RID: 1044
		[PreserveSig]
		int GetErrorCodeText_([MarshalAs(UnmanagedType.Error)] [In] int hRes, [In] uint LocaleId, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string MessageText);

		// Token: 0x06000415 RID: 1045
		[PreserveSig]
		int GetFacilityCodeText_([MarshalAs(UnmanagedType.Error)] [In] int hRes, [In] uint LocaleId, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string MessageText);
	}
}
