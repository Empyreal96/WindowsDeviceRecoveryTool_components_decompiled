using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200067D RID: 1661
	[ComVisible(true)]
	[Guid("0000000C-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IStream
	{
		// Token: 0x06006CF3 RID: 27891
		void Read(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesRead);

		// Token: 0x06006CF4 RID: 27892
		void Write(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesWritten);

		// Token: 0x06006CF5 RID: 27893
		void Seek(long offset, int origin, IntPtr refToNewOffsetNullAllowed);

		// Token: 0x06006CF6 RID: 27894
		void SetSize(long newSize);

		// Token: 0x06006CF7 RID: 27895
		void CopyTo(IStream targetStream, long bytesToCopy, IntPtr refToNumBytesRead, IntPtr refToNumBytesWritten);

		// Token: 0x06006CF8 RID: 27896
		void Commit(int commitFlags);

		// Token: 0x06006CF9 RID: 27897
		void Revert();

		// Token: 0x06006CFA RID: 27898
		void LockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06006CFB RID: 27899
		void UnlockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06006CFC RID: 27900
		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG statStructure, int statFlag);

		// Token: 0x06006CFD RID: 27901
		void Clone(out IStream newStream);
	}
}
