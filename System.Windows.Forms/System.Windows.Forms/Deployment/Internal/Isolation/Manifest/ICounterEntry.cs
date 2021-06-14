using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000E4 RID: 228
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8CD3FC86-AFD3-477a-8FD5-146C291195BB")]
	[ComImport]
	internal interface ICounterEntry
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600031C RID: 796
		CounterEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600031D RID: 797
		Guid CounterSetGuid { [SecurityCritical] get; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600031E RID: 798
		uint CounterId { [SecurityCritical] get; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600031F RID: 799
		string Name { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000320 RID: 800
		string Description { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000321 RID: 801
		uint CounterType { [SecurityCritical] get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000322 RID: 802
		ulong Attributes { [SecurityCritical] get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000323 RID: 803
		uint BaseId { [SecurityCritical] get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000324 RID: 804
		uint DefaultScale { [SecurityCritical] get; }
	}
}
