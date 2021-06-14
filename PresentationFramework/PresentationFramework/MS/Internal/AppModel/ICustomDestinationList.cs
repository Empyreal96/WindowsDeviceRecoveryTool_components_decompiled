using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B7 RID: 1975
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[ComImport]
	internal interface ICustomDestinationList
	{
		// Token: 0x06007B15 RID: 31509
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B16 RID: 31510
		[return: MarshalAs(UnmanagedType.Interface)]
		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		// Token: 0x06007B17 RID: 31511
		[PreserveSig]
		HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

		// Token: 0x06007B18 RID: 31512
		void AppendKnownCategory(KDC category);

		// Token: 0x06007B19 RID: 31513
		[PreserveSig]
		HRESULT AddUserTasks(IObjectArray poa);

		// Token: 0x06007B1A RID: 31514
		void CommitList();

		// Token: 0x06007B1B RID: 31515
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetRemovedDestinations([In] ref Guid riid);

		// Token: 0x06007B1C RID: 31516
		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B1D RID: 31517
		void AbortList();
	}
}
