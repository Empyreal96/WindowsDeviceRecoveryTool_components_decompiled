using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BA RID: 1978
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[ComImport]
	internal interface ITaskbarList
	{
		// Token: 0x06007B22 RID: 31522
		void HrInit();

		// Token: 0x06007B23 RID: 31523
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B24 RID: 31524
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B25 RID: 31525
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B26 RID: 31526
		void SetActiveAlt(IntPtr hwnd);
	}
}
