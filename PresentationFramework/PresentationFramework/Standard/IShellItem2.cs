using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace Standard
{
	// Token: 0x02000081 RID: 129
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[ComImport]
	internal interface IShellItem2 : IShellItem
	{
		// Token: 0x06000148 RID: 328
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler([In] IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x06000149 RID: 329
		IShellItem GetParent();

		// Token: 0x0600014A RID: 330
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x0600014B RID: 331
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x0600014C RID: 332
		int Compare(IShellItem psi, SICHINT hint);

		// Token: 0x0600014D RID: 333
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		// Token: 0x0600014E RID: 334
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

		// Token: 0x0600014F RID: 335
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		// Token: 0x06000150 RID: 336
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		// Token: 0x06000151 RID: 337
		void Update(IBindCtx pbc);

		// Token: 0x06000152 RID: 338
		[SecurityCritical]
		PROPVARIANT GetProperty(IntPtr key);

		// Token: 0x06000153 RID: 339
		Guid GetCLSID(IntPtr key);

		// Token: 0x06000154 RID: 340
		System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

		// Token: 0x06000155 RID: 341
		int GetInt32(IntPtr key);

		// Token: 0x06000156 RID: 342
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetString(IntPtr key);

		// Token: 0x06000157 RID: 343
		uint GetUInt32(IntPtr key);

		// Token: 0x06000158 RID: 344
		ulong GetUInt64(IntPtr key);

		// Token: 0x06000159 RID: 345
		[return: MarshalAs(UnmanagedType.Bool)]
		void GetBool(IntPtr key);
	}
}
