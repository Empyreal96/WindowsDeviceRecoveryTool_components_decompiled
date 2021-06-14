using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200067E RID: 1662
	[ComVisible(true)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistStreamWithArrays
	{
		// Token: 0x06006CFE RID: 27902
		void GetClassID(out Guid pClassID);

		// Token: 0x06006CFF RID: 27903
		[PreserveSig]
		int IsDirty();

		// Token: 0x06006D00 RID: 27904
		void Load(IStream pstm);

		// Token: 0x06006D01 RID: 27905
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06006D02 RID: 27906
		void GetSizeMax(out long pcbSize);
	}
}
