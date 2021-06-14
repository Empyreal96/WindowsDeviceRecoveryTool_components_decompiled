using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x0200002F RID: 47
	[CompilerGenerated]
	[Guid("D3BD3A44-D7B5-40A9-98B7-2FA4D01DEC08")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDeviceService
	{
		// Token: 0x06000158 RID: 344
		void Open([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPnPServiceID, [MarshalAs(UnmanagedType.Interface)] [In] IPortableDeviceValues pClientInfo);

		// Token: 0x06000159 RID: 345
		void _VtblGap1_1();

		// Token: 0x0600015A RID: 346
		void Content([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceContent2 ppContent);

		// Token: 0x0600015B RID: 347
		void _VtblGap2_2();

		// Token: 0x0600015C RID: 348
		void Close();

		// Token: 0x0600015D RID: 349
		void GetServiceObjectID([MarshalAs(UnmanagedType.LPWStr)] out string ppszServiceObjectID);
	}
}
