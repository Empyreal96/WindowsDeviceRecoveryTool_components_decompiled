using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200007F RID: 127
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	[ComImport]
	internal interface IShellItem
	{
		// Token: 0x0600013C RID: 316
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x0600013D RID: 317
		IShellItem GetParent();

		// Token: 0x0600013E RID: 318
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x0600013F RID: 319
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x06000140 RID: 320
		int Compare(IShellItem psi, SICHINT hint);
	}
}
