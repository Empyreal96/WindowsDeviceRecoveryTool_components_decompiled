using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x0200002E RID: 46
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("625E2DF8-6392-4CF0-9AD1-3CFA5F17775C")]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDevice
	{
		// Token: 0x06000153 RID: 339
		void Open([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPnPDeviceID, [MarshalAs(UnmanagedType.Interface)] [In] IPortableDeviceValues pClientInfo);

		// Token: 0x06000154 RID: 340
		void SendCommand([In] uint dwFlags, [MarshalAs(UnmanagedType.Interface)] [In] IPortableDeviceValues pParameters, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppResults);

		// Token: 0x06000155 RID: 341
		void Content([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceContent ppContent);

		// Token: 0x06000156 RID: 342
		void _VtblGap1_2();

		// Token: 0x06000157 RID: 343
		void Close();
	}
}
