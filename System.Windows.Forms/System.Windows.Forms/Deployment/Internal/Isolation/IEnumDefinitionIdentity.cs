using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200003D RID: 61
	[Guid("f3549d9c-fc73-4793-9c00-1cd204254c0c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumDefinitionIdentity
	{
		// Token: 0x0600012D RID: 301
		[SecurityCritical]
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionIdentity[] DefinitionIdentity);

		// Token: 0x0600012E RID: 302
		[SecurityCritical]
		void Skip([In] uint celt);

		// Token: 0x0600012F RID: 303
		[SecurityCritical]
		void Reset();

		// Token: 0x06000130 RID: 304
		[SecurityCritical]
		IEnumDefinitionIdentity Clone();
	}
}
