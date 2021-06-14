using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x0200002D RID: 45
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A1567595-4C2F-4574-A6FA-ECEF917B9A40")]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDeviceManager
	{
		// Token: 0x06000151 RID: 337
		void GetDevices([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] [In] [Out] string[] pPnPDeviceIDs, [In] [Out] ref uint pcPnPDeviceIDs);

		// Token: 0x06000152 RID: 338
		void RefreshDeviceList();
	}
}
