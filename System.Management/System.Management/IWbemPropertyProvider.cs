using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000067 RID: 103
	[InterfaceType(1)]
	[Guid("CE61E841-65BC-11D0-B6BD-00AA003240C7")]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemPropertyProvider
	{
		// Token: 0x06000417 RID: 1047
		[PreserveSig]
		int GetProperty_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strLocale, [MarshalAs(UnmanagedType.BStr)] [In] string strClassMapping, [MarshalAs(UnmanagedType.BStr)] [In] string strInstMapping, [MarshalAs(UnmanagedType.BStr)] [In] string strPropMapping, out object pvValue);

		// Token: 0x06000418 RID: 1048
		[PreserveSig]
		int PutProperty_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strLocale, [MarshalAs(UnmanagedType.BStr)] [In] string strClassMapping, [MarshalAs(UnmanagedType.BStr)] [In] string strInstMapping, [MarshalAs(UnmanagedType.BStr)] [In] string strPropMapping, [In] ref object pvValue);
	}
}
