using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x02000033 RID: 51
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7F6D695C-03DF-4439-A809-59266BEEE3A6")]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDeviceProperties
	{
		// Token: 0x06000174 RID: 372
		void _VtblGap1_2();

		// Token: 0x06000175 RID: 373
		void GetValues([MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjectID, [MarshalAs(UnmanagedType.Interface)] [In] IPortableDeviceKeyCollection pKeys, [MarshalAs(UnmanagedType.Interface)] out IPortableDeviceValues ppValues);
	}
}
