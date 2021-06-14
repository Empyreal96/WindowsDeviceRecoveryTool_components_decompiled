using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000079 RID: 121
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumIDList
	{
		// Token: 0x0600011D RID: 285
		[PreserveSig]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		// Token: 0x0600011E RID: 286
		[PreserveSig]
		HRESULT Skip(uint celt);

		// Token: 0x0600011F RID: 287
		void Reset();

		// Token: 0x06000120 RID: 288
		void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
	}
}
