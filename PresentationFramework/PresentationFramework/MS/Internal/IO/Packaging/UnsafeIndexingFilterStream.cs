using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000668 RID: 1640
	[SecurityCritical(SecurityCriticalScope.Everything)]
	internal class UnsafeIndexingFilterStream : Stream
	{
		// Token: 0x06006C89 RID: 27785 RVA: 0x001F3DAC File Offset: 0x001F1FAC
		internal UnsafeIndexingFilterStream(MS.Internal.Interop.IStream oleStream)
		{
			if (oleStream == null)
			{
				throw new ArgumentNullException("oleStream");
			}
			this._oleStream = oleStream;
			this._disposed = false;
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x001F3DD0 File Offset: 0x001F1FD0
		public unsafe override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			PackagingUtilities.VerifyStreamReadArgs(this, buffer, offset, count);
			if (count == 0)
			{
				return 0;
			}
			int result;
			IntPtr refToNumBytesRead = new IntPtr((void*)(&result));
			long position = this.Position;
			try
			{
				try
				{
					fixed (byte* ptr = &buffer[offset])
					{
						this._oleStream.Read(new IntPtr((void*)ptr), count, refToNumBytesRead);
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			catch (COMException innerException)
			{
				this.Position = position;
				throw new IOException("Read", innerException);
			}
			catch (IOException innerException2)
			{
				this.Position = position;
				throw new IOException("Read", innerException2);
			}
			return result;
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x001F3E7C File Offset: 0x001F207C
		public unsafe override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfStreamDisposed();
			long result = 0L;
			IntPtr refToNewOffsetNullAllowed = new IntPtr((void*)(&result));
			this._oleStream.Seek(offset, (int)origin, refToNewOffsetNullAllowed);
			return result;
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x001F3EAB File Offset: 0x001F20AB
		public override void SetLength(long newLength)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x001F3EAB File Offset: 0x001F20AB
		public override void Write(byte[] buf, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x001F3EC2 File Offset: 0x001F20C2
		public override void Flush()
		{
			this.ThrowIfStreamDisposed();
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x06006C8F RID: 27791 RVA: 0x001F3ECA File Offset: 0x001F20CA
		public override bool CanRead
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x06006C90 RID: 27792 RVA: 0x001F3ECA File Offset: 0x001F20CA
		public override bool CanSeek
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x06006C91 RID: 27793 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x06006C92 RID: 27794 RVA: 0x001F3ED5 File Offset: 0x001F20D5
		// (set) Token: 0x06006C93 RID: 27795 RVA: 0x001F3EE6 File Offset: 0x001F20E6
		public override long Position
		{
			get
			{
				this.ThrowIfStreamDisposed();
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.ThrowIfStreamDisposed();
				if (value < 0L)
				{
					throw new ArgumentException(SR.Get("CannotSetNegativePosition"));
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x06006C94 RID: 27796 RVA: 0x001F3F0C File Offset: 0x001F210C
		public override long Length
		{
			get
			{
				this.ThrowIfStreamDisposed();
				System.Runtime.InteropServices.ComTypes.STATSTG statstg;
				this._oleStream.Stat(out statstg, 1);
				return statstg.cbSize;
			}
		}

		// Token: 0x06006C95 RID: 27797 RVA: 0x001F3F34 File Offset: 0x001F2134
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._oleStream != null)
				{
					UnsafeNativeMethods.SafeReleaseComObject(this._oleStream);
				}
			}
			finally
			{
				this._oleStream = null;
				this._disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06006C96 RID: 27798 RVA: 0x001F3F80 File Offset: 0x001F2180
		private void ThrowIfStreamDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null, SR.Get("StreamObjectDisposed"));
			}
		}

		// Token: 0x04003548 RID: 13640
		private MS.Internal.Interop.IStream _oleStream;

		// Token: 0x04003549 RID: 13641
		private bool _disposed;
	}
}
