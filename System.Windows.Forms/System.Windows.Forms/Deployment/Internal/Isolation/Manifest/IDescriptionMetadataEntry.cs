using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BD RID: 189
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CB73147E-5FC2-4c31-B4E6-58D13DBE1A08")]
	[ComImport]
	internal interface IDescriptionMetadataEntry
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002AE RID: 686
		DescriptionMetadataEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002AF RID: 687
		string Publisher { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002B0 RID: 688
		string Product { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002B1 RID: 689
		string SupportUrl { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002B2 RID: 690
		string IconFile { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002B3 RID: 691
		string ErrorReportUrl { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002B4 RID: 692
		string SuiteName { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
