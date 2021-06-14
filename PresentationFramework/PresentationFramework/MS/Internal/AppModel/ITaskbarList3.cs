using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BC RID: 1980
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList3 : ITaskbarList2, ITaskbarList
	{
		// Token: 0x06007B2D RID: 31533
		void HrInit();

		// Token: 0x06007B2E RID: 31534
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B2F RID: 31535
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B30 RID: 31536
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B31 RID: 31537
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06007B32 RID: 31538
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x06007B33 RID: 31539
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x06007B34 RID: 31540
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x06007B35 RID: 31541
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x06007B36 RID: 31542
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x06007B37 RID: 31543
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x06007B38 RID: 31544
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x06007B39 RID: 31545
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06007B3A RID: 31546
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06007B3B RID: 31547
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x06007B3C RID: 31548
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, NativeMethods.IconHandle hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x06007B3D RID: 31549
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x06007B3E RID: 31550
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, NativeMethods.RefRECT prcClip);
	}
}
