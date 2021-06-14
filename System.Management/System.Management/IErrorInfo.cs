using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000054 RID: 84
	[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IErrorInfo
	{
		// Token: 0x0600034C RID: 844
		Guid GetGUID();

		// Token: 0x0600034D RID: 845
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSource();

		// Token: 0x0600034E RID: 846
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDescription();

		// Token: 0x0600034F RID: 847
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetHelpFile();

		// Token: 0x06000350 RID: 848
		uint GetHelpContext();
	}
}
