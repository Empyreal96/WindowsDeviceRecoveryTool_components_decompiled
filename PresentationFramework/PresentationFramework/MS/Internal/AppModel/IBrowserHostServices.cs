using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x02000776 RID: 1910
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a0aa9153-65b4-3b57-9f2b-126f9c76c9f5")]
	[ComImport]
	internal interface IBrowserHostServices
	{
		// Token: 0x060078FE RID: 30974
		[SecurityCritical]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Run([MarshalAs(UnmanagedType.LPWStr)] [In] string strUrl, [MarshalAs(UnmanagedType.LPWStr)] [In] string strFragment, MimeType mime, [MarshalAs(UnmanagedType.LPWStr)] [In] string strDebugSecurityZoneURL, [MarshalAs(UnmanagedType.LPWStr)] [In] string strApplicationId, [MarshalAs(UnmanagedType.Interface)] [In] object storageIUnknown, [MarshalAs(UnmanagedType.Interface)] [In] object loadByteArray, HostingFlags hostingFlags, INativeProgressPage nativeProgressPage, [MarshalAs(UnmanagedType.BStr)] [In] string bstrProgressAssemblyName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrProgressClassName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrErrorAssemblyName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrErrorClassName, IHostBrowser hostBrowser);

		// Token: 0x060078FF RID: 30975
		[SecurityCritical]
		void SetParent(IntPtr parentHandle);

		// Token: 0x06007900 RID: 30976
		void Show([MarshalAs(UnmanagedType.Bool)] bool showView);

		// Token: 0x06007901 RID: 30977
		void Move(int x, int y, int width, int height);

		// Token: 0x06007902 RID: 30978
		[SecurityCritical]
		void SetBrowserCallback([MarshalAs(UnmanagedType.Interface)] [In] object browserCallback);

		// Token: 0x06007903 RID: 30979
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsAppLoaded();

		// Token: 0x06007904 RID: 30980
		[PreserveSig]
		int GetApplicationExitCode();

		// Token: 0x06007905 RID: 30981
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool CanInvokeJournalEntry([MarshalAs(UnmanagedType.I4)] [In] int entryId);

		// Token: 0x06007906 RID: 30982
		void SaveHistory([MarshalAs(UnmanagedType.Interface)] object ucomIStream, [MarshalAs(UnmanagedType.Bool)] bool persistEntireJournal, [MarshalAs(UnmanagedType.I4)] out int entryIndex, [MarshalAs(UnmanagedType.LPWStr)] out string url, [MarshalAs(UnmanagedType.LPWStr)] out string title);

		// Token: 0x06007907 RID: 30983
		[SecurityCritical]
		void LoadHistory([MarshalAs(UnmanagedType.Interface)] object ucomIStream);

		// Token: 0x06007908 RID: 30984
		[PreserveSig]
		int QueryStatus([MarshalAs(UnmanagedType.LPStruct)] Guid guidCmdGroup, [In] uint command, out uint flags);

		// Token: 0x06007909 RID: 30985
		[SecurityCritical]
		[PreserveSig]
		int ExecCommand([MarshalAs(UnmanagedType.LPStruct)] Guid guidCmdGroup, uint command, object arg);

		// Token: 0x0600790A RID: 30986
		[SecurityCritical]
		void PostShutdown();

		// Token: 0x0600790B RID: 30987
		void Activate([MarshalAs(UnmanagedType.Bool)] bool fActivated);

		// Token: 0x0600790C RID: 30988
		void TabInto(bool forward);

		// Token: 0x0600790D RID: 30989
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool FocusedElementWantsBackspace();
	}
}
