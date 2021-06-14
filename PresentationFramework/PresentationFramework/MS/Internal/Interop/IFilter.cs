using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200067B RID: 1659
	[ComVisible(true)]
	[Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IFilter
	{
		// Token: 0x06006CE9 RID: 27881
		IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes);

		// Token: 0x06006CEA RID: 27882
		STAT_CHUNK GetChunk();

		// Token: 0x06006CEB RID: 27883
		void GetText([In] [Out] ref uint pcwcBuffer, [In] IntPtr pBuffer);

		// Token: 0x06006CEC RID: 27884
		IntPtr GetValue();

		// Token: 0x06006CED RID: 27885
		IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid);
	}
}
