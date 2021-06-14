using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000068 RID: 104
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("E245105B-B06E-11D0-AD61-00C04FD8FDFF")]
	[ComImport]
	internal interface IWbemEventProvider
	{
		// Token: 0x06000419 RID: 1049
		[PreserveSig]
		int ProvideEvents_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink, [In] int lFlags);
	}
}
