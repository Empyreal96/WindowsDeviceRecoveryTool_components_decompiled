using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000060 RID: 96
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("027947E1-D731-11CE-A357-000000000001")]
	[ComImport]
	internal interface IEnumWbemClassObject
	{
		// Token: 0x060003E9 RID: 1001
		[PreserveSig]
		int Reset_();

		// Token: 0x060003EA RID: 1002
		[PreserveSig]
		int Next_([In] int lTimeout, [In] uint uCount, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] IWbemClassObject_DoNotMarshal[] apObjects, out uint puReturned);

		// Token: 0x060003EB RID: 1003
		[PreserveSig]
		int NextAsync_([In] uint uCount, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink);

		// Token: 0x060003EC RID: 1004
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003ED RID: 1005
		[PreserveSig]
		int Skip_([In] int lTimeout, [In] uint nCount);
	}
}
