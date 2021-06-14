using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065C RID: 1628
	internal sealed class ByteStream : Stream
	{
		// Token: 0x06006C0D RID: 27661 RVA: 0x001F1A28 File Offset: 0x001EFC28
		[SecurityCritical]
		internal ByteStream(object underlyingStream, FileAccess openAccess)
		{
			ByteStream.SecuritySuppressedIStream value = underlyingStream as ByteStream.SecuritySuppressedIStream;
			this._securitySuppressedIStream = new SecurityCriticalDataForSet<ByteStream.SecuritySuppressedIStream>(value);
			this._access = openAccess;
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x06006C0E RID: 27662 RVA: 0x001F1A55 File Offset: 0x001EFC55
		public override bool CanRead
		{
			get
			{
				return !this.StreamDisposed && (FileAccess.Read == (this._access & FileAccess.Read) || FileAccess.ReadWrite == (this._access & FileAccess.ReadWrite));
			}
		}

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x06006C0F RID: 27663 RVA: 0x001F1A79 File Offset: 0x001EFC79
		public override bool CanSeek
		{
			get
			{
				return !this.StreamDisposed;
			}
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x06006C10 RID: 27664 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x06006C11 RID: 27665 RVA: 0x001F1A84 File Offset: 0x001EFC84
		public override long Length
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				this.CheckDisposedStatus();
				if (!this._isLengthInitialized)
				{
					System.Runtime.InteropServices.ComTypes.STATSTG statstg;
					this._securitySuppressedIStream.Value.Stat(out statstg, 1);
					this._isLengthInitialized = true;
					this._length = statstg.cbSize;
				}
				return this._length;
			}
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x06006C12 RID: 27666 RVA: 0x001F1ACC File Offset: 0x001EFCCC
		// (set) Token: 0x06006C13 RID: 27667 RVA: 0x001F1AF8 File Offset: 0x001EFCF8
		public override long Position
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				this.CheckDisposedStatus();
				long result = 0L;
				this._securitySuppressedIStream.Value.Seek(0L, 1, out result);
				return result;
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				this.CheckDisposedStatus();
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.Get("SetPositionNotSupported"));
				}
				long num = 0L;
				this._securitySuppressedIStream.Value.Seek(value, 0, out num);
				if (value != num)
				{
					throw new IOException(SR.Get("SeekFailed"));
				}
			}
		}

		// Token: 0x06006C14 RID: 27668 RVA: 0x00002137 File Offset: 0x00000337
		public override void Flush()
		{
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x001F1B50 File Offset: 0x001EFD50
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckDisposedStatus();
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.Get("SeekNotSupported"));
			}
			long result = 0L;
			int dwOrigin;
			switch (origin)
			{
			case SeekOrigin.Begin:
				dwOrigin = 0;
				if (0L > offset)
				{
					throw new ArgumentOutOfRangeException("offset", SR.Get("SeekNegative"));
				}
				break;
			case SeekOrigin.Current:
				dwOrigin = 1;
				break;
			case SeekOrigin.End:
				dwOrigin = 2;
				break;
			default:
				throw new InvalidEnumArgumentException("origin", (int)origin, typeof(SeekOrigin));
			}
			this._securitySuppressedIStream.Value.Seek(offset, dwOrigin, out result);
			return result;
		}

		// Token: 0x06006C16 RID: 27670 RVA: 0x001F1BE3 File Offset: 0x001EFDE3
		public override void SetLength(long newLength)
		{
			throw new NotSupportedException(SR.Get("SetLengthNotSupported"));
		}

		// Token: 0x06006C17 RID: 27671 RVA: 0x001F1BF4 File Offset: 0x001EFDF4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckDisposedStatus();
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.Get("ReadNotSupported"));
			}
			int num = 0;
			if (count == 0)
			{
				return num;
			}
			if (0 > count)
			{
				throw new ArgumentOutOfRangeException("count", SR.Get("ReadCountNegative"));
			}
			if (0 > offset)
			{
				throw new ArgumentOutOfRangeException("offset", SR.Get("BufferOffsetNegative"));
			}
			if (buffer.Length == 0 || buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.Get("BufferTooSmall"), "buffer");
			}
			if (offset == 0)
			{
				this._securitySuppressedIStream.Value.Read(buffer, count, out num);
			}
			else if (0 < offset)
			{
				byte[] array = new byte[count];
				this._securitySuppressedIStream.Value.Read(array, count, out num);
				if (num > 0)
				{
					Array.Copy(array, 0, buffer, offset, num);
				}
			}
			return num;
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x001F1CC0 File Offset: 0x001EFEC0
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.Get("WriteNotSupported"));
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x001F1CD1 File Offset: 0x001EFED1
		public override void Close()
		{
			this._disposed = true;
		}

		// Token: 0x06006C1A RID: 27674 RVA: 0x001F1CDA File Offset: 0x001EFEDA
		internal void CheckDisposedStatus()
		{
			if (this.StreamDisposed)
			{
				throw new ObjectDisposedException(null, SR.Get("StreamObjectDisposed"));
			}
		}

		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x06006C1B RID: 27675 RVA: 0x001F1CF5 File Offset: 0x001EFEF5
		private bool StreamDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x04003508 RID: 13576
		[SecurityCritical]
		private SecurityCriticalDataForSet<ByteStream.SecuritySuppressedIStream> _securitySuppressedIStream;

		// Token: 0x04003509 RID: 13577
		private FileAccess _access;

		// Token: 0x0400350A RID: 13578
		private long _length;

		// Token: 0x0400350B RID: 13579
		private bool _isLengthInitialized;

		// Token: 0x0400350C RID: 13580
		private bool _disposed;

		// Token: 0x02000B1C RID: 2844
		[Guid("0000000c-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical(SecurityCriticalScope.Everything)]
		[ComImport]
		public interface SecuritySuppressedIStream
		{
			// Token: 0x06008D23 RID: 36131
			void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, out int pcbRead);

			// Token: 0x06008D24 RID: 36132
			void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, out int pcbWritten);

			// Token: 0x06008D25 RID: 36133
			void Seek(long dlibMove, int dwOrigin, out long plibNewPosition);

			// Token: 0x06008D26 RID: 36134
			void SetSize(long libNewSize);

			// Token: 0x06008D27 RID: 36135
			void CopyTo(ByteStream.SecuritySuppressedIStream pstm, long cb, out long pcbRead, out long pcbWritten);

			// Token: 0x06008D28 RID: 36136
			void Commit(int grfCommitFlags);

			// Token: 0x06008D29 RID: 36137
			void Revert();

			// Token: 0x06008D2A RID: 36138
			void LockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x06008D2B RID: 36139
			void UnlockRegion(long libOffset, long cb, int dwLockType);

			// Token: 0x06008D2C RID: 36140
			void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

			// Token: 0x06008D2D RID: 36141
			void Clone(out ByteStream.SecuritySuppressedIStream ppstm);
		}
	}
}
