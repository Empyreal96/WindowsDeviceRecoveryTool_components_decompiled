using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000071 RID: 113
	[Guid("9AE62877-7544-4BB0-AA26-A13824659ED6")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemPathKeyList
	{
		// Token: 0x0600042A RID: 1066
		[PreserveSig]
		int GetCount_(out uint puKeyCount);

		// Token: 0x0600042B RID: 1067
		[PreserveSig]
		int SetKey_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags, [In] uint uCimType, [In] IntPtr pKeyVal);

		// Token: 0x0600042C RID: 1068
		[PreserveSig]
		int SetKey2_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags, [In] uint uCimType, [In] ref object pKeyVal);

		// Token: 0x0600042D RID: 1069
		[PreserveSig]
		int GetKey_([In] uint uKeyIx, [In] uint uFlags, [In] [Out] ref uint puNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszKeyName, [In] [Out] ref uint puKeyValBufSize, [In] [Out] IntPtr pKeyVal, out uint puApparentCimType);

		// Token: 0x0600042E RID: 1070
		[PreserveSig]
		int GetKey2_([In] uint uKeyIx, [In] uint uFlags, [In] [Out] ref uint puNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszKeyName, [In] [Out] ref object pKeyValue, out uint puApparentCimType);

		// Token: 0x0600042F RID: 1071
		[PreserveSig]
		int RemoveKey_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags);

		// Token: 0x06000430 RID: 1072
		[PreserveSig]
		int RemoveAllKeys_([In] uint uFlags);

		// Token: 0x06000431 RID: 1073
		[PreserveSig]
		int MakeSingleton_([In] sbyte bSet);

		// Token: 0x06000432 RID: 1074
		[PreserveSig]
		int GetInfo_([In] uint uRequestedInfo, out ulong puResponse);

		// Token: 0x06000433 RID: 1075
		[PreserveSig]
		int GetText_([In] int lFlags, [In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);
	}
}
