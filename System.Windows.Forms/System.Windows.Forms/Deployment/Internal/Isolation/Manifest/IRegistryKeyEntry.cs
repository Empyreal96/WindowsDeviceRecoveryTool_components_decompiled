using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000D8 RID: 216
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("186685d1-6673-48c3-bc83-95859bb591df")]
	[ComImport]
	internal interface IRegistryKeyEntry
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000300 RID: 768
		RegistryKeyEntry AllData { [SecurityCritical] get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000301 RID: 769
		uint Flags { [SecurityCritical] get; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000302 RID: 770
		uint Protection { [SecurityCritical] get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000303 RID: 771
		string BuildFilter { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000304 RID: 772
		object SecurityDescriptor { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000305 RID: 773
		object Values { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000306 RID: 774
		object Keys { [SecurityCritical] [return: MarshalAs(UnmanagedType.Interface)] get; }
	}
}
