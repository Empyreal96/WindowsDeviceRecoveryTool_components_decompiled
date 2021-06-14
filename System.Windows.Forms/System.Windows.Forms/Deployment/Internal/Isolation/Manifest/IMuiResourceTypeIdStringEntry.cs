using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000084 RID: 132
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("11df5cad-c183-479b-9a44-3842b71639ce")]
	[ComImport]
	internal interface IMuiResourceTypeIdStringEntry
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600022B RID: 555
		MuiResourceTypeIdStringEntry AllData { [SecurityCritical] get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600022C RID: 556
		object StringIds { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600022D RID: 557
		object IntegerIds { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
