using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200005C RID: 92
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("9556DC99-828C-11CF-A37E-00AA003240C7")]
	[ComImport]
	internal interface IWbemServices
	{
		// Token: 0x060003B5 RID: 949
		[PreserveSig]
		int OpenNamespace_([MarshalAs(UnmanagedType.BStr)] [In] string strNamespace, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemServices ppWorkingNamespace, [In] IntPtr ppCallResult);

		// Token: 0x060003B6 RID: 950
		[PreserveSig]
		int CancelAsyncCall_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink);

		// Token: 0x060003B7 RID: 951
		[PreserveSig]
		int QueryObjectSink_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemObjectSink ppResponseHandler);

		// Token: 0x060003B8 RID: 952
		[PreserveSig]
		int GetObject_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Management.MarshalWbemObject)] out IWbemClassObjectFreeThreaded ppObject, [In] IntPtr ppCallResult);

		// Token: 0x060003B9 RID: 953
		[PreserveSig]
		int GetObjectAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003BA RID: 954
		[PreserveSig]
		int PutClass_([In] IntPtr pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003BB RID: 955
		[PreserveSig]
		int PutClassAsync_([In] IntPtr pObject, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003BC RID: 956
		[PreserveSig]
		int DeleteClass_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003BD RID: 957
		[PreserveSig]
		int DeleteClassAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strClass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003BE RID: 958
		[PreserveSig]
		int CreateClassEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003BF RID: 959
		[PreserveSig]
		int CreateClassEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strSuperclass, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003C0 RID: 960
		[PreserveSig]
		int PutInstance_([In] IntPtr pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003C1 RID: 961
		[PreserveSig]
		int PutInstanceAsync_([In] IntPtr pInst, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003C2 RID: 962
		[PreserveSig]
		int DeleteInstance_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr ppCallResult);

		// Token: 0x060003C3 RID: 963
		[PreserveSig]
		int DeleteInstanceAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003C4 RID: 964
		[PreserveSig]
		int CreateInstanceEnum_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003C5 RID: 965
		[PreserveSig]
		int CreateInstanceEnumAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strFilter, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003C6 RID: 966
		[PreserveSig]
		int ExecQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003C7 RID: 967
		[PreserveSig]
		int ExecQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003C8 RID: 968
		[PreserveSig]
		int ExecNotificationQuery_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060003C9 RID: 969
		[PreserveSig]
		int ExecNotificationQueryAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strQueryLanguage, [MarshalAs(UnmanagedType.BStr)] [In] string strQuery, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);

		// Token: 0x060003CA RID: 970
		[PreserveSig]
		int ExecMethod_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr pInParams, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Management.MarshalWbemObject)] out IWbemClassObjectFreeThreaded ppOutParams, [In] IntPtr ppCallResult);

		// Token: 0x060003CB RID: 971
		[PreserveSig]
		int ExecMethodAsync_([MarshalAs(UnmanagedType.BStr)] [In] string strObjectPath, [MarshalAs(UnmanagedType.BStr)] [In] string strMethodName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [In] IntPtr pInParams, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pResponseHandler);
	}
}
