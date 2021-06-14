using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BB RID: 1979
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[ComImport]
	internal interface ITaskbarList2 : ITaskbarList
	{
		// Token: 0x06007B27 RID: 31527
		void HrInit();

		// Token: 0x06007B28 RID: 31528
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B29 RID: 31529
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B2A RID: 31530
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B2B RID: 31531
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06007B2C RID: 31532
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
	}
}
