using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AD RID: 1965
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[ComImport]
	internal interface IShellItem2 : IShellItem
	{
		// Token: 0x06007A8E RID: 31374
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x06007A8F RID: 31375
		IShellItem GetParent();

		// Token: 0x06007A90 RID: 31376
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x06007A91 RID: 31377
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x06007A92 RID: 31378
		int Compare(IShellItem psi, SICHINT hint);

		// Token: 0x06007A93 RID: 31379
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		// Token: 0x06007A94 RID: 31380
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

		// Token: 0x06007A95 RID: 31381
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		// Token: 0x06007A96 RID: 31382
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		// Token: 0x06007A97 RID: 31383
		void Update(IBindCtx pbc);

		// Token: 0x06007A98 RID: 31384
		[SecurityCritical]
		void GetProperty(IntPtr key, [In] [Out] PROPVARIANT pv);

		// Token: 0x06007A99 RID: 31385
		Guid GetCLSID(IntPtr key);

		// Token: 0x06007A9A RID: 31386
		System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

		// Token: 0x06007A9B RID: 31387
		int GetInt32(IntPtr key);

		// Token: 0x06007A9C RID: 31388
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetString(IntPtr key);

		// Token: 0x06007A9D RID: 31389
		uint GetUInt32(IntPtr key);

		// Token: 0x06007A9E RID: 31390
		ulong GetUInt64(IntPtr key);

		// Token: 0x06007A9F RID: 31391
		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetBool(IntPtr key);
	}
}
