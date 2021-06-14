using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AB RID: 1963
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellFolder
	{
		// Token: 0x06007A7F RID: 31359
		void ParseDisplayName(IntPtr hwnd, IBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In] [Out] ref int pchEaten, out IntPtr ppidl, [In] [Out] ref uint pdwAttributes);

		// Token: 0x06007A80 RID: 31360
		IEnumIDList EnumObjects(IntPtr hwnd, SHCONTF grfFlags);

		// Token: 0x06007A81 RID: 31361
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToObject(IntPtr pidl, IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06007A82 RID: 31362
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToStorage(IntPtr pidl, IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06007A83 RID: 31363
		[PreserveSig]
		HRESULT CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

		// Token: 0x06007A84 RID: 31364
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid);

		// Token: 0x06007A85 RID: 31365
		void GetAttributesOf(uint cidl, IntPtr apidl, [In] [Out] ref SFGAO rgfInOut);

		// Token: 0x06007A86 RID: 31366
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 1)] IntPtr apidl, [In] ref Guid riid, [In] [Out] ref uint rgfReserved);

		// Token: 0x06007A87 RID: 31367
		void GetDisplayNameOf(IntPtr pidl, SHGDN uFlags, out IntPtr pName);

		// Token: 0x06007A88 RID: 31368
		void SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, SHGDN uFlags, out IntPtr ppidlOut);
	}
}
