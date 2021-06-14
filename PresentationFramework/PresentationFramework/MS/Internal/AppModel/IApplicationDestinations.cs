using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B5 RID: 1973
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
	[ComImport]
	internal interface IApplicationDestinations
	{
		// Token: 0x06007B10 RID: 31504
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B11 RID: 31505
		void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x06007B12 RID: 31506
		void RemoveAllDestinations();
	}
}
