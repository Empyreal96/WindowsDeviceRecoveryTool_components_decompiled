using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200003F RID: 63
	[Guid("b30352cf-23da-4577-9b3f-b4e6573be53b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumReferenceIdentity
	{
		// Token: 0x06000138 RID: 312
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IReferenceIdentity[] ReferenceIdentity);

		// Token: 0x06000139 RID: 313
		[SecurityCritical]
		void Skip(uint celt);

		// Token: 0x0600013A RID: 314
		[SecurityCritical]
		void Reset();

		// Token: 0x0600013B RID: 315
		[SecurityCritical]
		IEnumReferenceIdentity Clone();
	}
}
