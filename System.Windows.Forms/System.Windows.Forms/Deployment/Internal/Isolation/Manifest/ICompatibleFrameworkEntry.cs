using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000E7 RID: 231
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C98BFE2A-62C9-40AD-ADCE-A9037BE2BE6C")]
	[ComImport]
	internal interface ICompatibleFrameworkEntry
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000326 RID: 806
		CompatibleFrameworkEntry AllData { [SecurityCritical] get; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000327 RID: 807
		uint index { [SecurityCritical] get; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000328 RID: 808
		string TargetVersion { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000329 RID: 809
		string Profile { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600032A RID: 810
		string SupportedRuntime { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
