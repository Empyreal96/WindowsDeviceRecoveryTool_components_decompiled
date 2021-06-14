using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000087 RID: 135
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55b2dec1-d0f6-4bf4-91b1-30f73ad8e4df")]
	[ComImport]
	internal interface IMuiResourceTypeIdIntEntry
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000232 RID: 562
		MuiResourceTypeIdIntEntry AllData { [SecurityCritical] get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000233 RID: 563
		object StringIds { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000234 RID: 564
		object IntegerIds { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
