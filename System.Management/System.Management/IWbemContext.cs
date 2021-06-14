using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200005B RID: 91
	[Guid("44ACA674-E8FC-11D0-A07C-00C04FB68820")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemContext
	{
		// Token: 0x060003AC RID: 940
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemContext ppNewCopy);

		// Token: 0x060003AD RID: 941
		[PreserveSig]
		int GetNames_([In] int lFlags, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060003AE RID: 942
		[PreserveSig]
		int BeginEnumeration_([In] int lFlags);

		// Token: 0x060003AF RID: 943
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out object pValue);

		// Token: 0x060003B0 RID: 944
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x060003B1 RID: 945
		[PreserveSig]
		int SetValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pValue);

		// Token: 0x060003B2 RID: 946
		[PreserveSig]
		int GetValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, out object pValue);

		// Token: 0x060003B3 RID: 947
		[PreserveSig]
		int DeleteValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags);

		// Token: 0x060003B4 RID: 948
		[PreserveSig]
		int DeleteAll_();
	}
}
