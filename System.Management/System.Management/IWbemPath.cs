using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000072 RID: 114
	[Guid("3BC15AF2-736C-477E-9E51-238AF8667DCC")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemPath
	{
		// Token: 0x06000434 RID: 1076
		[PreserveSig]
		int SetText_([In] uint uMode, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszPath);

		// Token: 0x06000435 RID: 1077
		[PreserveSig]
		int GetText_([In] int lFlags, [In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);

		// Token: 0x06000436 RID: 1078
		[PreserveSig]
		int GetInfo_([In] uint uRequestedInfo, out ulong puResponse);

		// Token: 0x06000437 RID: 1079
		[PreserveSig]
		int SetServer_([MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000438 RID: 1080
		[PreserveSig]
		int GetServer_([In] [Out] ref uint puNameBufLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pName);

		// Token: 0x06000439 RID: 1081
		[PreserveSig]
		int GetNamespaceCount_(out uint puCount);

		// Token: 0x0600043A RID: 1082
		[PreserveSig]
		int SetNamespaceAt_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

		// Token: 0x0600043B RID: 1083
		[PreserveSig]
		int GetNamespaceAt_([In] uint uIndex, [In] [Out] ref uint puNameBufLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pName);

		// Token: 0x0600043C RID: 1084
		[PreserveSig]
		int RemoveNamespaceAt_([In] uint uIndex);

		// Token: 0x0600043D RID: 1085
		[PreserveSig]
		int RemoveAllNamespaces_();

		// Token: 0x0600043E RID: 1086
		[PreserveSig]
		int GetScopeCount_(out uint puCount);

		// Token: 0x0600043F RID: 1087
		[PreserveSig]
		int SetScope_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszClass);

		// Token: 0x06000440 RID: 1088
		[PreserveSig]
		int SetScopeFromText_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

		// Token: 0x06000441 RID: 1089
		[PreserveSig]
		int GetScope_([In] uint uIndex, [In] [Out] ref uint puClassNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszClass, [MarshalAs(UnmanagedType.Interface)] out IWbemPathKeyList pKeyList);

		// Token: 0x06000442 RID: 1090
		[PreserveSig]
		int GetScopeAsText_([In] uint uIndex, [In] [Out] ref uint puTextBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);

		// Token: 0x06000443 RID: 1091
		[PreserveSig]
		int RemoveScope_([In] uint uIndex);

		// Token: 0x06000444 RID: 1092
		[PreserveSig]
		int RemoveAllScopes_();

		// Token: 0x06000445 RID: 1093
		[PreserveSig]
		int SetClassName_([MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000446 RID: 1094
		[PreserveSig]
		int GetClassName_([In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszName);

		// Token: 0x06000447 RID: 1095
		[PreserveSig]
		int GetKeyList_([MarshalAs(UnmanagedType.Interface)] out IWbemPathKeyList pOut);

		// Token: 0x06000448 RID: 1096
		[PreserveSig]
		int CreateClassPart_([In] int lFlags, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000449 RID: 1097
		[PreserveSig]
		int DeleteClassPart_([In] int lFlags);

		// Token: 0x0600044A RID: 1098
		[PreserveSig]
		int IsRelative_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace);

		// Token: 0x0600044B RID: 1099
		[PreserveSig]
		int IsRelativeOrChild_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace, [In] int lFlags);

		// Token: 0x0600044C RID: 1100
		[PreserveSig]
		int IsLocal_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine);

		// Token: 0x0600044D RID: 1101
		[PreserveSig]
		int IsSameClassName_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszClass);
	}
}
