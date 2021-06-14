using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Management
{
	// Token: 0x0200005F RID: 95
	[TypeLibType(512)]
	[Guid("7C857801-7381-11CF-884D-00AA004B2E24")]
	[InterfaceType(1)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IWbemObjectSink
	{
		// Token: 0x060003E7 RID: 999
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		int Indicate_([In] int lObjectCount, [MarshalAs(UnmanagedType.LPArray)] [In] IntPtr[] apObjArray);

		// Token: 0x060003E8 RID: 1000
		[PreserveSig]
		int SetStatus_([In] int lFlags, [MarshalAs(UnmanagedType.Error)] [In] int hResult, [MarshalAs(UnmanagedType.BStr)] [In] string strParam, [In] IntPtr pObjParam);
	}
}
