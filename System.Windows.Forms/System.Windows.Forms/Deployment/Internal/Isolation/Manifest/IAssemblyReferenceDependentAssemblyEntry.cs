using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A8 RID: 168
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C31FF59E-CD25-47b8-9EF3-CF4433EB97CC")]
	[ComImport]
	internal interface IAssemblyReferenceDependentAssemblyEntry
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000285 RID: 645
		AssemblyReferenceDependentAssemblyEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000286 RID: 646
		string Group { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000287 RID: 647
		string Codebase { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000288 RID: 648
		ulong Size { [SecurityCritical] get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000289 RID: 649
		object HashValue { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600028A RID: 650
		uint HashAlgorithm { [SecurityCritical] get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600028B RID: 651
		uint Flags { [SecurityCritical] get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600028C RID: 652
		string ResourceFallbackCulture { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600028D RID: 653
		string Description { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600028E RID: 654
		string SupportUrl { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600028F RID: 655
		ISection HashElements { [SecurityCritical] get; }
	}
}
