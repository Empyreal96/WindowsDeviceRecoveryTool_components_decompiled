using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000062 RID: 98
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("BFBF883A-CAD7-11D3-A11B-00105A1F515A")]
	[ComImport]
	internal interface IWbemObjectTextSrc
	{
		// Token: 0x060003EF RID: 1007
		[PreserveSig]
		int GetText_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObj, [In] uint uObjTextFormat, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.BStr)] out string strText);

		// Token: 0x060003F0 RID: 1008
		[PreserveSig]
		int CreateFromText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strText, [In] uint uObjTextFormat, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal pNewObj);
	}
}
