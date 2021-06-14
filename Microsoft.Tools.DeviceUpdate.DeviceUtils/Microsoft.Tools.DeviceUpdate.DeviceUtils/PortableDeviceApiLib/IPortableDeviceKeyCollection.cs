using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x02000034 RID: 52
	[Guid("DADA2357-E0AD-492E-98DB-DD61C53BA353")]
	[TypeIdentifier]
	[CompilerGenerated]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IPortableDeviceKeyCollection
	{
		// Token: 0x06000176 RID: 374
		void _VtblGap1_2();

		// Token: 0x06000177 RID: 375
		void Add([In] ref _tagpropertykey key);
	}
}
