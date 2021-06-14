using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200006D RID: 109
	[Guid("1BE41571-91DD-11D1-AEB2-00C04FB68820")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemProviderInitSink
	{
		// Token: 0x0600041F RID: 1055
		[PreserveSig]
		int SetStatus_([In] int lStatus, [In] int lFlags);
	}
}
