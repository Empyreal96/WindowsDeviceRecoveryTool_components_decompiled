using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000066 RID: 102
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("E246107B-B06E-11D0-AD61-00C04FD8FDFF")]
	[ComImport]
	internal interface IWbemUnboundObjectSink
	{
		// Token: 0x06000416 RID: 1046
		[PreserveSig]
		int IndicateToConsumer_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pLogicalConsumer, [In] int lNumObjects, [MarshalAs(UnmanagedType.Interface)] [In] ref IWbemClassObject_DoNotMarshal apObjects);
	}
}
