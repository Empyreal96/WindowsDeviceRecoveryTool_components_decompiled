using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000CF RID: 207
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BC")]
	[ComImport]
	internal interface IEventMapEntry
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002EC RID: 748
		EventMapEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002ED RID: 749
		string MapName { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002EE RID: 750
		string Name { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002EF RID: 751
		uint Value { [SecurityCritical] get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002F0 RID: 752
		bool IsValueMap { [SecurityCritical] get; }
	}
}
