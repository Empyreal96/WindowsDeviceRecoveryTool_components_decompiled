using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A5 RID: 165
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1E0422A1-F0D2-44ae-914B-8A2DECCFD22B")]
	[ComImport]
	internal interface ICLRSurrogateEntry
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027D RID: 637
		CLRSurrogateEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027E RID: 638
		Guid Clsid { [SecurityCritical] get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600027F RID: 639
		string RuntimeVersion { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000280 RID: 640
		string ClassName { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
