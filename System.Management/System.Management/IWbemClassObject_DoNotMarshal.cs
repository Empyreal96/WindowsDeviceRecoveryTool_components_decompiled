using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000058 RID: 88
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("DC12A681-737F-11CF-884D-00AA004B2E24")]
	[ComImport]
	internal interface IWbemClassObject_DoNotMarshal
	{
		// Token: 0x0600038C RID: 908
		[PreserveSig]
		int GetQualifierSet_([MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x0600038D RID: 909
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x0600038E RID: 910
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pVal, [In] int Type);

		// Token: 0x0600038F RID: 911
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x06000390 RID: 912
		[PreserveSig]
		int GetNames_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQualifierName, [In] int lFlags, [In] ref object pQualifierVal, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x06000391 RID: 913
		[PreserveSig]
		int BeginEnumeration_([In] int lEnumFlags);

		// Token: 0x06000392 RID: 914
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string strName, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x06000393 RID: 915
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x06000394 RID: 916
		[PreserveSig]
		int GetPropertyQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszProperty, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x06000395 RID: 917
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppCopy);

		// Token: 0x06000396 RID: 918
		[PreserveSig]
		int GetObjectText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrObjectText);

		// Token: 0x06000397 RID: 919
		[PreserveSig]
		int SpawnDerivedClass_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewClass);

		// Token: 0x06000398 RID: 920
		[PreserveSig]
		int SpawnInstance_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewInstance);

		// Token: 0x06000399 RID: 921
		[PreserveSig]
		int CompareTo_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pCompareTo);

		// Token: 0x0600039A RID: 922
		[PreserveSig]
		int GetPropertyOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x0600039B RID: 923
		[PreserveSig]
		int InheritsFrom_([MarshalAs(UnmanagedType.LPWStr)] [In] string strAncestor);

		// Token: 0x0600039C RID: 924
		[PreserveSig]
		int GetMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x0600039D RID: 925
		[PreserveSig]
		int PutMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInSignature, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pOutSignature);

		// Token: 0x0600039E RID: 926
		[PreserveSig]
		int DeleteMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x0600039F RID: 927
		[PreserveSig]
		int BeginMethodEnumeration_([In] int lEnumFlags);

		// Token: 0x060003A0 RID: 928
		[PreserveSig]
		int NextMethod_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x060003A1 RID: 929
		[PreserveSig]
		int EndMethodEnumeration_();

		// Token: 0x060003A2 RID: 930
		[PreserveSig]
		int GetMethodQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethod, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060003A3 RID: 931
		[PreserveSig]
		int GetMethodOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethodName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);
	}
}
