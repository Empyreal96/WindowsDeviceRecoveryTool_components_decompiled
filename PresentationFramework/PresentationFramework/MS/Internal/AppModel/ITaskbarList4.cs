using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BD RID: 1981
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	[ComImport]
	internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
	{
		// Token: 0x06007B3F RID: 31551
		void HrInit();

		// Token: 0x06007B40 RID: 31552
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B41 RID: 31553
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B42 RID: 31554
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B43 RID: 31555
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06007B44 RID: 31556
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		// Token: 0x06007B45 RID: 31557
		[PreserveSig]
		HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		// Token: 0x06007B46 RID: 31558
		[PreserveSig]
		HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		// Token: 0x06007B47 RID: 31559
		[PreserveSig]
		HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		// Token: 0x06007B48 RID: 31560
		[PreserveSig]
		HRESULT UnregisterTab(IntPtr hwndTab);

		// Token: 0x06007B49 RID: 31561
		[PreserveSig]
		HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		// Token: 0x06007B4A RID: 31562
		[PreserveSig]
		HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		// Token: 0x06007B4B RID: 31563
		[PreserveSig]
		HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06007B4C RID: 31564
		[PreserveSig]
		HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		// Token: 0x06007B4D RID: 31565
		[PreserveSig]
		HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		// Token: 0x06007B4E RID: 31566
		[PreserveSig]
		HRESULT SetOverlayIcon(IntPtr hwnd, NativeMethods.IconHandle hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		// Token: 0x06007B4F RID: 31567
		[PreserveSig]
		HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		// Token: 0x06007B50 RID: 31568
		[PreserveSig]
		HRESULT SetThumbnailClip(IntPtr hwnd, NativeMethods.RefRECT prcClip);

		// Token: 0x06007B51 RID: 31569
		[PreserveSig]
		HRESULT SetTabProperties(IntPtr hwndTab, STPF stpFlags);
	}
}
