using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200003C RID: 60
	[Guid("9cdaae75-246e-4b00-a26d-b9aec137a3eb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumIDENTITY_ATTRIBUTE
	{
		// Token: 0x06000128 RID: 296
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDENTITY_ATTRIBUTE[] rgAttributes);

		// Token: 0x06000129 RID: 297
		[SecurityCritical]
		IntPtr CurrentIntoBuffer([In] IntPtr Available, [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] Data);

		// Token: 0x0600012A RID: 298
		[SecurityCritical]
		void Skip([In] uint celt);

		// Token: 0x0600012B RID: 299
		[SecurityCritical]
		void Reset();

		// Token: 0x0600012C RID: 300
		[SecurityCritical]
		IEnumIDENTITY_ATTRIBUTE Clone();
	}
}
