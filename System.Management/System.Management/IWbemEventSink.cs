using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000070 RID: 112
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("3AE0080A-7E3A-4366-BF89-0FEEDC931659")]
	[ComImport]
	internal interface IWbemEventSink
	{
		// Token: 0x06000423 RID: 1059
		[PreserveSig]
		int Indicate_([In] int lObjectCount, [MarshalAs(UnmanagedType.Interface)] [In] ref IWbemClassObject_DoNotMarshal apObjArray);

		// Token: 0x06000424 RID: 1060
		[PreserveSig]
		int SetStatus_([In] int lFlags, [MarshalAs(UnmanagedType.Error)] [In] int hResult, [MarshalAs(UnmanagedType.BStr)] [In] string strParam, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObjParam);

		// Token: 0x06000425 RID: 1061
		[PreserveSig]
		int IndicateWithSD_([In] int lNumObjects, [MarshalAs(UnmanagedType.IUnknown)] [In] ref object apObjects, [In] int lSDLength, [In] ref byte pSD);

		// Token: 0x06000426 RID: 1062
		[PreserveSig]
		int SetSinkSecurity_([In] int lSDLength, [In] ref byte pSD);

		// Token: 0x06000427 RID: 1063
		[PreserveSig]
		int IsActive_();

		// Token: 0x06000428 RID: 1064
		[PreserveSig]
		int GetRestrictedSink_([In] int lNumQueries, [MarshalAs(UnmanagedType.LPWStr)] [In] ref string awszQueries, [MarshalAs(UnmanagedType.IUnknown)] [In] object pCallback, [MarshalAs(UnmanagedType.Interface)] out IWbemEventSink ppSink);

		// Token: 0x06000429 RID: 1065
		[PreserveSig]
		int SetBatchingParameters_([In] int lFlags, [In] uint dwMaxBufferSize, [In] uint dwMaxSendLatency);
	}
}
