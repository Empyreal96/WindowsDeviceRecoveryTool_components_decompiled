using System;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077E RID: 1918
	internal interface IHostService
	{
		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x0600791D RID: 31005
		RootBrowserWindowProxy RootBrowserWindowProxy { get; }

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x0600791E RID: 31006
		IntPtr HostWindowHandle { [SecurityCritical] [UIPermission(SecurityAction.InheritanceDemand, Unrestricted = true)] get; }
	}
}
