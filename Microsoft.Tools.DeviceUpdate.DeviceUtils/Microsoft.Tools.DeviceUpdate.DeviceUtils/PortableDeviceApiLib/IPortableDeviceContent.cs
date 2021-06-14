using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x02000032 RID: 50
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeIdentifier]
	[Guid("6A96ED84-7C73-4480-9938-BF5AF477D426")]
	[CompilerGenerated]
	[ComImport]
	public interface IPortableDeviceContent
	{
		// Token: 0x06000172 RID: 370
		void _VtblGap1_1();

		// Token: 0x06000173 RID: 371
		void Properties([MarshalAs(UnmanagedType.Interface)] out IPortableDeviceProperties ppProperties);
	}
}
