using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B1 RID: 177
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("70A4ECEE-B195-4c59-85BF-44B6ACA83F07")]
	[ComImport]
	internal interface IResourceTableMappingEntry
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600029B RID: 667
		ResourceTableMappingEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600029C RID: 668
		string id { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600029D RID: 669
		string FinalStringMapped { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
