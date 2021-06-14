using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x0200003C RID: 60
	[TypeIdentifier]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CompilerGenerated]
	[Guid("9B4ADD96-F6BF-4034-8708-ECA72BF10554")]
	[ComImport]
	public interface IPortableDeviceContent2 : IPortableDeviceContent
	{
		// Token: 0x06000179 RID: 377
		void _VtblGap1_1();

		// Token: 0x0600017A RID: 378
		void Properties([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceProperties ppProperties);
	}
}
