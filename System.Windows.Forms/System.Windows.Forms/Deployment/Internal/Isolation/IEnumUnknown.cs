using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200001B RID: 27
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumUnknown
	{
		// Token: 0x060000B7 RID: 183
		[PreserveSig]
		int Next(uint celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] [Out] object[] rgelt, ref uint celtFetched);

		// Token: 0x060000B8 RID: 184
		[PreserveSig]
		int Skip(uint celt);

		// Token: 0x060000B9 RID: 185
		[PreserveSig]
		int Reset();

		// Token: 0x060000BA RID: 186
		[PreserveSig]
		int Clone(out IEnumUnknown enumUnknown);
	}
}
