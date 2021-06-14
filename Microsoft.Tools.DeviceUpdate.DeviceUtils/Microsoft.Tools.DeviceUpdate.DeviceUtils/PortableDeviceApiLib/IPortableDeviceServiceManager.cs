using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x0200003A RID: 58
	[Guid("A8ABC4E9-A84A-47A9-80B3-C5D9B172A961")]
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDeviceServiceManager
	{
		// Token: 0x06000178 RID: 376
		void GetDeviceServices([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPnPDeviceID, [In] ref Guid guidServiceCategory, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] [In] [Out] string[] pServices, [In] [Out] ref uint pcServices);
	}
}
