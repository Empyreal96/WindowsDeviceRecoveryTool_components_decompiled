using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000085 RID: 133
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
	[ComImport]
	internal interface IApplicationDestinations
	{
		// Token: 0x06000177 RID: 375
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] [In] string pszAppID);

		// Token: 0x06000178 RID: 376
		void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x06000179 RID: 377
		void RemoveAllDestinations();
	}
}
