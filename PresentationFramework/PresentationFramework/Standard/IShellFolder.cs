using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200007E RID: 126
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellFolder
	{
		// Token: 0x06000132 RID: 306
		void ParseDisplayName([In] IntPtr hwnd, [In] IBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszDisplayName, [In] [Out] ref int pchEaten, out IntPtr ppidl, [In] [Out] ref uint pdwAttributes);

		// Token: 0x06000133 RID: 307
		IEnumIDList EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags);

		// Token: 0x06000134 RID: 308
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToObject([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06000135 RID: 309
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToStorage([In] IntPtr pidl, [In] IBindCtx pbc, [In] ref Guid riid);

		// Token: 0x06000136 RID: 310
		[PreserveSig]
		HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);

		// Token: 0x06000137 RID: 311
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid);

		// Token: 0x06000138 RID: 312
		void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In] [Out] ref SFGAO rgfInOut);

		// Token: 0x06000139 RID: 313
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 2)] [In] IntPtr apidl, [In] ref Guid riid, [In] [Out] ref uint rgfReserved);

		// Token: 0x0600013A RID: 314
		void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDN uFlags, out IntPtr pName);

		// Token: 0x0600013B RID: 315
		void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName, [In] SHGDN uFlags, out IntPtr ppidlOut);
	}
}
