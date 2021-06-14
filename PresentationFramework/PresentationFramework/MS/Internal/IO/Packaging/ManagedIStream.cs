using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000667 RID: 1639
	internal class ManagedIStream : IStream
	{
		// Token: 0x06006C7D RID: 27773 RVA: 0x001F3C2F File Offset: 0x001F1E2F
		internal ManagedIStream(Stream ioStream)
		{
			if (ioStream == null)
			{
				throw new ArgumentNullException("ioStream");
			}
			this._ioStream = ioStream;
		}

		// Token: 0x06006C7E RID: 27774 RVA: 0x001F3C4C File Offset: 0x001F1E4C
		[SecurityCritical]
		void IStream.Read(byte[] buffer, int bufferSize, IntPtr bytesReadPtr)
		{
			int val = this._ioStream.Read(buffer, 0, bufferSize);
			if (bytesReadPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesReadPtr, val);
			}
		}

		// Token: 0x06006C7F RID: 27775 RVA: 0x001F3C7C File Offset: 0x001F1E7C
		[SecurityCritical]
		void IStream.Seek(long offset, int origin, IntPtr newPositionPtr)
		{
			SeekOrigin origin2;
			switch (origin)
			{
			case 0:
				origin2 = SeekOrigin.Begin;
				break;
			case 1:
				origin2 = SeekOrigin.Current;
				break;
			case 2:
				origin2 = SeekOrigin.End;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin");
			}
			long val = this._ioStream.Seek(offset, origin2);
			if (newPositionPtr != IntPtr.Zero)
			{
				Marshal.WriteInt64(newPositionPtr, val);
			}
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x001F3CD6 File Offset: 0x001F1ED6
		void IStream.SetSize(long libNewSize)
		{
			this._ioStream.SetLength(libNewSize);
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x001F3CE4 File Offset: 0x001F1EE4
		void IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG streamStats, int grfStatFlag)
		{
			streamStats = default(System.Runtime.InteropServices.ComTypes.STATSTG);
			streamStats.type = 2;
			streamStats.cbSize = this._ioStream.Length;
			streamStats.grfMode = 0;
			if (this._ioStream.CanRead && this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 2;
				return;
			}
			if (this._ioStream.CanRead)
			{
				streamStats.grfMode |= 0;
				return;
			}
			if (this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 1;
				return;
			}
			throw new IOException(SR.Get("StreamObjectDisposed"));
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x001F3D7E File Offset: 0x001F1F7E
		[SecurityCritical]
		void IStream.Write(byte[] buffer, int bufferSize, IntPtr bytesWrittenPtr)
		{
			this._ioStream.Write(buffer, 0, bufferSize);
			if (bytesWrittenPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesWrittenPtr, bufferSize);
			}
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x001F3DA2 File Offset: 0x001F1FA2
		void IStream.Clone(out IStream streamCopy)
		{
			streamCopy = null;
			throw new NotSupportedException();
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x00041D30 File Offset: 0x0003FF30
		void IStream.CopyTo(IStream targetStream, long bufferSize, IntPtr buffer, IntPtr bytesWrittenPtr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x00041D30 File Offset: 0x0003FF30
		void IStream.Commit(int flags)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x00041D30 File Offset: 0x0003FF30
		void IStream.LockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x00041D30 File Offset: 0x0003FF30
		void IStream.Revert()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x00041D30 File Offset: 0x0003FF30
		void IStream.UnlockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04003547 RID: 13639
		private Stream _ioStream;
	}
}
