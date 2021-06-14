using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000032 RID: 50
	[Guid("b840a2f5-a497-4a6d-9038-cd3ec2fbd222")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY
	{
		// Token: 0x060000F1 RID: 241
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY[] rgElements);

		// Token: 0x060000F2 RID: 242
		[SecurityCritical]
		void Skip([In] uint ulElements);

		// Token: 0x060000F3 RID: 243
		[SecurityCritical]
		void Reset();

		// Token: 0x060000F4 RID: 244
		[SecurityCritical]
		IEnumSTORE_CATEGORY Clone();
	}
}
