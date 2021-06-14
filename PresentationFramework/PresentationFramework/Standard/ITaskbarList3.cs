using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200008A RID: 138
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList3 : ITaskbarList2, ITaskbarList
	{
		// Token: 0x06000189 RID: 393
		void HrInit();

		// Token: 0x0600018A RID: 394
		void AddTab(IntPtr hwnd);

		// Token: 0x0600018B RID: 395
		void DeleteTab(IntPtr hwnd);

		// Token: 0x0600018C RID: 396
		void ActivateTab(IntPtr hwnd);

		// Token: 0x0600018D RID: 397
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x0600018E RID: 398
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x0600018F RID: 399
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x06000190 RID: 400
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x06000191 RID: 401
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x06000192 RID: 402
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x06000193 RID: 403
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x06000194 RID: 404
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x06000195 RID: 405
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06000196 RID: 406
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06000197 RID: 407
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x06000198 RID: 408
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x06000199 RID: 409
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x0600019A RID: 410
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);
	}
}
