using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000083 RID: 131
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[ComImport]
	internal interface ITaskbarList
	{
		// Token: 0x0600016C RID: 364
		void HrInit();

		// Token: 0x0600016D RID: 365
		void AddTab(IntPtr hwnd);

		// Token: 0x0600016E RID: 366
		void DeleteTab(IntPtr hwnd);

		// Token: 0x0600016F RID: 367
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06000170 RID: 368
		void SetActiveAlt(IntPtr hwnd);
	}
}
