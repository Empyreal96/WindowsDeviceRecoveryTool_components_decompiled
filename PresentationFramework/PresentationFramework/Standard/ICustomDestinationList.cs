using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000087 RID: 135
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[ComImport]
	internal interface ICustomDestinationList
	{
		// Token: 0x0600017C RID: 380
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] [In] string pszAppID);

		// Token: 0x0600017D RID: 381
		[return: MarshalAs(UnmanagedType.Interface)]
		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		// Token: 0x0600017E RID: 382
		[PreserveSig]
		HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

		// Token: 0x0600017F RID: 383
		void AppendKnownCategory(KDC category);

		// Token: 0x06000180 RID: 384
		[PreserveSig]
		HRESULT AddUserTasks(IObjectArray poa);

		// Token: 0x06000181 RID: 385
		void CommitList();

		// Token: 0x06000182 RID: 386
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetRemovedDestinations([In] ref Guid riid);

		// Token: 0x06000183 RID: 387
		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06000184 RID: 388
		void AbortList();
	}
}
