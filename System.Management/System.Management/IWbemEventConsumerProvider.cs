using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200006C RID: 108
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("E246107A-B06E-11D0-AD61-00C04FD8FDFF")]
	[ComImport]
	internal interface IWbemEventConsumerProvider
	{
		// Token: 0x0600041E RID: 1054
		[PreserveSig]
		int FindConsumer_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pLogicalConsumer, [MarshalAs(UnmanagedType.Interface)] out IWbemUnboundObjectSink ppConsumer);
	}
}
