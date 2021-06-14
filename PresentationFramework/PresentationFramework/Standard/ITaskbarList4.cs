using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200008B RID: 139
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
	{
		// Token: 0x0600019B RID: 411
		void HrInit();

		// Token: 0x0600019C RID: 412
		void AddTab(IntPtr hwnd);

		// Token: 0x0600019D RID: 413
		void DeleteTab(IntPtr hwnd);

		// Token: 0x0600019E RID: 414
		void ActivateTab(IntPtr hwnd);

		// Token: 0x0600019F RID: 415
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x060001A0 RID: 416
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x060001A1 RID: 417
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x060001A2 RID: 418
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x060001A3 RID: 419
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x060001A4 RID: 420
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x060001A5 RID: 421
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x060001A6 RID: 422
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x060001A7 RID: 423
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060001A8 RID: 424
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x060001A9 RID: 425
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x060001AA RID: 426
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x060001AB RID: 427
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x060001AC RID: 428
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);

		// Token: 0x060001AD RID: 429
		void SetTabProperties(IntPtr hwndTab, STPF stpFlags);
	}
}
