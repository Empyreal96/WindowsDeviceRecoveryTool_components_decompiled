using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x02000772 RID: 1906
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5FFAD804-61C8-445c-8C31-A2101C64C510")]
	[ComImport]
	internal interface IBrowserCallbackServices
	{
		// Token: 0x060078DA RID: 30938
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void OnBeforeShowNavigationWindow();

		// Token: 0x060078DB RID: 30939
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void PostReadyStateChange([MarshalAs(UnmanagedType.I4)] [In] int readyState);

		// Token: 0x060078DC RID: 30940
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void DelegateNavigation([MarshalAs(UnmanagedType.BStr)] [In] string url, [MarshalAs(UnmanagedType.BStr)] [In] string targetName, [MarshalAs(UnmanagedType.BStr)] [In] string headers);

		// Token: 0x060078DD RID: 30941
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool UpdateAddressBar([MarshalAs(UnmanagedType.BStr)] [In] string url);

		// Token: 0x060078DE RID: 30942
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		void UpdateBackForwardState();

		// Token: 0x060078DF RID: 30943
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		void UpdateTravelLog([MarshalAs(UnmanagedType.Bool)] [In] bool addNewEntry);

		// Token: 0x060078E0 RID: 30944
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool UpdateProgress([MarshalAs(UnmanagedType.I8)] [In] long cBytesCompleted, [MarshalAs(UnmanagedType.I8)] [In] long cBytesTotal);

		// Token: 0x060078E1 RID: 30945
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool ChangeDownloadState([In] bool fIsDownloading);

		// Token: 0x060078E2 RID: 30946
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsDownlevelPlatform();

		// Token: 0x060078E3 RID: 30947
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsShuttingDown();

		// Token: 0x060078E4 RID: 30948
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		bool TabOut(bool forward);

		// Token: 0x060078E5 RID: 30949
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		void ProcessUnhandledException([MarshalAs(UnmanagedType.BStr)] [In] string pErrorMsg);

		// Token: 0x060078E6 RID: 30950
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		int GetOleClientSite([MarshalAs(UnmanagedType.IUnknown)] out object oleClientSite);

		// Token: 0x060078E7 RID: 30951
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		int UpdateCommands();

		// Token: 0x060078E8 RID: 30952
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		IntPtr CreateWebBrowserControlInBrowserProcess();
	}
}
