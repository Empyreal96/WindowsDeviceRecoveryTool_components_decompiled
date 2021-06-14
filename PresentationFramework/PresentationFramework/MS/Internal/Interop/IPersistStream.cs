using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200067C RID: 1660
	[ComVisible(true)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistStream
	{
		// Token: 0x06006CEE RID: 27886
		void GetClassID(out Guid pClassID);

		// Token: 0x06006CEF RID: 27887
		[PreserveSig]
		int IsDirty();

		// Token: 0x06006CF0 RID: 27888
		void Load(IStream pstm);

		// Token: 0x06006CF1 RID: 27889
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06006CF2 RID: 27890
		void GetSizeMax(out long pcbSize);
	}
}
