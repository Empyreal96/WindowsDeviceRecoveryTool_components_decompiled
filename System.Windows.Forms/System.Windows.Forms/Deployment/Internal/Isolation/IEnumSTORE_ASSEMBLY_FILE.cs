using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000030 RID: 48
	[Guid("a5c6aaa3-03e4-478d-b9f5-2e45908d5e4f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_FILE
	{
		// Token: 0x060000E6 RID: 230
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY_FILE[] rgelt);

		// Token: 0x060000E7 RID: 231
		[SecurityCritical]
		void Skip([In] uint celt);

		// Token: 0x060000E8 RID: 232
		[SecurityCritical]
		void Reset();

		// Token: 0x060000E9 RID: 233
		[SecurityCritical]
		IEnumSTORE_ASSEMBLY_FILE Clone();
	}
}
