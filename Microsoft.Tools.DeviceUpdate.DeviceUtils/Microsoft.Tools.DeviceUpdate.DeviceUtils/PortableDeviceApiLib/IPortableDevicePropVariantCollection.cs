using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x02000031 RID: 49
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("89B2E422-4F1B-4316-BCEF-A44AFEA83EB3")]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDevicePropVariantCollection
	{
		// Token: 0x06000170 RID: 368
		void _VtblGap1_2();

		// Token: 0x06000171 RID: 369
		void Add([In] ref tag_inner_PROPVARIANT pValue);
	}
}
