using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000063 RID: 99
	[Guid("49353C9A-516B-11D1-AEA6-00C04FB68820")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemObjectAccess
	{
		// Token: 0x060003F1 RID: 1009
		[PreserveSig]
		int GetQualifierSet_([MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060003F2 RID: 1010
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060003F3 RID: 1011
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pVal, [In] int Type);

		// Token: 0x060003F4 RID: 1012
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060003F5 RID: 1013
		[PreserveSig]
		int GetNames_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQualifierName, [In] int lFlags, [In] ref object pQualifierVal, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060003F6 RID: 1014
		[PreserveSig]
		int BeginEnumeration_([In] int lEnumFlags);

		// Token: 0x060003F7 RID: 1015
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string strName, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060003F8 RID: 1016
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x060003F9 RID: 1017
		[PreserveSig]
		int GetPropertyQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszProperty, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060003FA RID: 1018
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppCopy);

		// Token: 0x060003FB RID: 1019
		[PreserveSig]
		int GetObjectText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrObjectText);

		// Token: 0x060003FC RID: 1020
		[PreserveSig]
		int SpawnDerivedClass_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewClass);

		// Token: 0x060003FD RID: 1021
		[PreserveSig]
		int SpawnInstance_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewInstance);

		// Token: 0x060003FE RID: 1022
		[PreserveSig]
		int CompareTo_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pCompareTo);

		// Token: 0x060003FF RID: 1023
		[PreserveSig]
		int GetPropertyOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x06000400 RID: 1024
		[PreserveSig]
		int InheritsFrom_([MarshalAs(UnmanagedType.LPWStr)] [In] string strAncestor);

		// Token: 0x06000401 RID: 1025
		[PreserveSig]
		int GetMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x06000402 RID: 1026
		[PreserveSig]
		int PutMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInSignature, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pOutSignature);

		// Token: 0x06000403 RID: 1027
		[PreserveSig]
		int DeleteMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x06000404 RID: 1028
		[PreserveSig]
		int BeginMethodEnumeration_([In] int lEnumFlags);

		// Token: 0x06000405 RID: 1029
		[PreserveSig]
		int NextMethod_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x06000406 RID: 1030
		[PreserveSig]
		int EndMethodEnumeration_();

		// Token: 0x06000407 RID: 1031
		[PreserveSig]
		int GetMethodQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethod, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x06000408 RID: 1032
		[PreserveSig]
		int GetMethodOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethodName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x06000409 RID: 1033
		[PreserveSig]
		int GetPropertyHandle_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszPropertyName, out int pType, out int plHandle);

		// Token: 0x0600040A RID: 1034
		[PreserveSig]
		int WritePropertyValue_([In] int lHandle, [In] int lNumBytes, [In] ref byte aData);

		// Token: 0x0600040B RID: 1035
		[PreserveSig]
		int ReadPropertyValue_([In] int lHandle, [In] int lBufferSize, out int plNumBytes, out byte aData);

		// Token: 0x0600040C RID: 1036
		[PreserveSig]
		int ReadDWORD_([In] int lHandle, out uint pdw);

		// Token: 0x0600040D RID: 1037
		[PreserveSig]
		int WriteDWORD_([In] int lHandle, [In] uint dw);

		// Token: 0x0600040E RID: 1038
		[PreserveSig]
		int ReadQWORD_([In] int lHandle, out ulong pqw);

		// Token: 0x0600040F RID: 1039
		[PreserveSig]
		int WriteQWORD_([In] int lHandle, [In] ulong pw);

		// Token: 0x06000410 RID: 1040
		[PreserveSig]
		int GetPropertyInfoByHandle_([In] int lHandle, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out int pType);

		// Token: 0x06000411 RID: 1041
		[PreserveSig]
		int Lock_([In] int lFlags);

		// Token: 0x06000412 RID: 1042
		[PreserveSig]
		int Unlock_([In] int lFlags);
	}
}
