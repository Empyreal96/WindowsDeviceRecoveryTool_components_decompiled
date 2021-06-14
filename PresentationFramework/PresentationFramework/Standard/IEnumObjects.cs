using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200007A RID: 122
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("2c1c7e2e-2d0e-4059-831e-1e6f82335c2e")]
	[ComImport]
	internal interface IEnumObjects
	{
		// Token: 0x06000121 RID: 289
		void Next(uint celt, [In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown, SizeParamIndex = 0)] [Out] object[] rgelt, out uint pceltFetched);

		// Token: 0x06000122 RID: 290
		void Skip(uint celt);

		// Token: 0x06000123 RID: 291
		void Reset();

		// Token: 0x06000124 RID: 292
		IEnumObjects Clone();
	}
}
