using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x02000774 RID: 1908
	[Guid("AD5D6F03-0002-4D77-9FC0-381981317144")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface IHostBrowser2
	{
		// Token: 0x060078FA RID: 30970
		void TabOut_DoNotUse(bool forward);

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x060078FB RID: 30971
		object HostScriptObject { [return: MarshalAs(UnmanagedType.IDispatch)] get; }

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x060078FC RID: 30972
		string PluginName { get; }

		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x060078FD RID: 30973
		string PluginVersion { get; }
	}
}
