using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000062 RID: 98
	[Guid("ace1b703-1aac-4956-ab87-90cac8b93ce6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IManifestParseErrorCallback
	{
		// Token: 0x060001D7 RID: 471
		[SecurityCritical]
		void OnError([In] uint StartLine, [In] uint nStartColumn, [In] uint cCharacterCount, [In] int hr, [MarshalAs(UnmanagedType.LPWStr)] [In] string ErrorStatusHostFile, [In] uint ParameterCount, [MarshalAs(UnmanagedType.LPArray)] [In] string[] Parameters);
	}
}
