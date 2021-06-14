using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000052 RID: 82
	[Guid("87A5AD68-A38A-43ef-ACA9-EFE910E5D24C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWmiEventSource
	{
		// Token: 0x06000348 RID: 840
		void Indicate(IntPtr pIWbemClassObject);

		// Token: 0x06000349 RID: 841
		void SetStatus(int lFlags, int hResult, [MarshalAs(UnmanagedType.BStr)] string strParam, IntPtr pObjParam);
	}
}
