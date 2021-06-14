using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000059 RID: 89
	[InterfaceType(1)]
	[Guid("DC12A680-737F-11CF-884D-00AA004B2E24")]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemQualifierSet_DoNotMarshal
	{
		// Token: 0x060003A4 RID: 932
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int plFlavor);

		// Token: 0x060003A5 RID: 933
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] ref object pVal, [In] int lFlavor);

		// Token: 0x060003A6 RID: 934
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060003A7 RID: 935
		[PreserveSig]
		int GetNames_([In] int lFlags, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060003A8 RID: 936
		[PreserveSig]
		int BeginEnumeration_([In] int lFlags);

		// Token: 0x060003A9 RID: 937
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [In] [Out] ref object pVal, [In] [Out] ref int plFlavor);

		// Token: 0x060003AA RID: 938
		[PreserveSig]
		int EndEnumeration_();
	}
}
