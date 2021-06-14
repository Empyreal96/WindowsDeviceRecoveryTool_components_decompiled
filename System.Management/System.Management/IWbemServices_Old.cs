using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200005D RID: 93
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("9556DC99-828C-11CF-A37E-00AA003240C7")]
	[ComImport]
	internal interface IWbemServices_Old
	{
		// Token: 0x060003CC RID: 972
		[PreserveSig]
		int OpenNamespace_([MarshalAs(UnmanagedType.BStr)] [In] string strNamespace, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemServices ppWorkingNamespace, [In] IntPtr ppCallResult);

		// Token: 0x060003CD RID: 973
		[PreserveSig]
		int CancelAsyncCall_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink);

		// Token: 0x060003CE RID: 974
		[PreserveSig]
		int QueryObjectSink_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemObjectSink ppResponseHandler);

		// Token: 0x060003CF RID: 975
		[PreserveSig]
		int GetObject_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppObject, [In] IntPtr ppCallResult);

		// Token: 0x060003D0 RID: 976
		[PreserveSig]
		int GetObjectAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003D1 RID: 977
		[PreserveSig]
		int PutClass_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003D2 RID: 978
		[PreserveSig]
		int PutClassAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003D3 RID: 979
		[PreserveSig]
		int DeleteClass_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003D4 RID: 980
		[PreserveSig]
		int DeleteClassAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003D5 RID: 981
		[PreserveSig]
		int CreateClassEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003D6 RID: 982
		[PreserveSig]
		int CreateClassEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003D7 RID: 983
		[PreserveSig]
		int PutInstance_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003D8 RID: 984
		[PreserveSig]
		int PutInstanceAsync_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003D9 RID: 985
		[PreserveSig]
		int DeleteInstance_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003DA RID: 986
		[PreserveSig]
		int DeleteInstanceAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003DB RID: 987
		[PreserveSig]
		int CreateInstanceEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003DC RID: 988
		[PreserveSig]
		int CreateInstanceEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003DD RID: 989
		[PreserveSig]
		int ExecQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003DE RID: 990
		[PreserveSig]
		int ExecQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003DF RID: 991
		[PreserveSig]
		int ExecNotificationQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003E0 RID: 992
		[PreserveSig]
		int ExecNotificationQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003E1 RID: 993
		[PreserveSig]
		int ExecMethod_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutParams, [In] IntPtr ppCallResult);

		// Token: 0x060003E2 RID: 994
		[PreserveSig]
		int ExecMethodAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInParams, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);
	}
}
