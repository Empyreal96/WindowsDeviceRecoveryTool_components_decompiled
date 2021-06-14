using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000069 RID: 105
	[Guid("580ACAF8-FA1C-11D0-AD72-00C04FD8FDFF")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemEventProviderQuerySink
	{
		// Token: 0x0600041A RID: 1050
		[PreserveSig]
		int NewQuery_([In] uint dwId, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQueryLanguage, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszQuery);

		// Token: 0x0600041B RID: 1051
		[PreserveSig]
		int CancelQuery_([In] uint dwId);
	}
}
