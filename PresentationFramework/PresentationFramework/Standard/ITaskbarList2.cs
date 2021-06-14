using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000084 RID: 132
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[ComImport]
	internal interface ITaskbarList2 : ITaskbarList
	{
		// Token: 0x06000171 RID: 369
		void HrInit();

		// Token: 0x06000172 RID: 370
		void AddTab(IntPtr hwnd);

		// Token: 0x06000173 RID: 371
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06000174 RID: 372
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06000175 RID: 373
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06000176 RID: 374
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
	}
}
