using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200002E RID: 46
	[Guid("a5c637bf-6eaa-4e5f-b535-55299657e33e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY
	{
		// Token: 0x060000DB RID: 219
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY[] rgelt);

		// Token: 0x060000DC RID: 220
		[SecurityCritical]
		void Skip([In] uint celt);

		// Token: 0x060000DD RID: 221
		[SecurityCritical]
		void Reset();

		// Token: 0x060000DE RID: 222
		[SecurityCritical]
		IEnumSTORE_ASSEMBLY Clone();
	}
}
