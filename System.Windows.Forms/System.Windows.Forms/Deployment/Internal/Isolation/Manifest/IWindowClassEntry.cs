using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AE RID: 174
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BA")]
	[ComImport]
	internal interface IWindowClassEntry
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000296 RID: 662
		WindowClassEntry AllData { [SecurityCritical] get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000297 RID: 663
		string ClassName { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000298 RID: 664
		string HostDll { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000299 RID: 665
		bool fVersioned { [SecurityCritical] get; }
	}
}
