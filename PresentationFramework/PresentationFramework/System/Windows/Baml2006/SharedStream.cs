using System;
using System.ComponentModel;
using System.IO;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016A RID: 362
	internal class SharedStream : Stream
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x00041ABA File Offset: 0x0003FCBA
		public SharedStream(Stream baseStream)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.Initialize(baseStream, 0L, baseStream.Length);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00041ADF File Offset: 0x0003FCDF
		public SharedStream(Stream baseStream, long offset, long length)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.Initialize(baseStream, offset, length);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00041B00 File Offset: 0x0003FD00
		private void Initialize(Stream baseStream, long offset, long length)
		{
			if (!baseStream.CanSeek)
			{
				throw new ArgumentException("can’t seek on baseStream");
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			SharedStream sharedStream = baseStream as SharedStream;
			if (sharedStream != null)
			{
				this._baseStream = sharedStream.BaseStream;
				this._offset = offset + sharedStream._offset;
				this._length = length;
				this._refCount = sharedStream._refCount;
				this._refCount.Value++;
				return;
			}
			this._baseStream = baseStream;
			this._offset = offset;
			this._length = length;
			this._refCount = new SharedStream.RefCount();
			this._refCount.Value++;
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x00041BBE File Offset: 0x0003FDBE
		public virtual int SharedCount
		{
			get
			{
				return this._refCount.Value;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00041BCB File Offset: 0x0003FDCB
		public override bool CanRead
		{
			get
			{
				this.CheckDisposed();
				return this._baseStream.CanRead;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00041BDE File Offset: 0x0003FDDE
		public override void Flush()
		{
			this.CheckDisposed();
			this._baseStream.Flush();
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x00041BF1 File Offset: 0x0003FDF1
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x00041BF9 File Offset: 0x0003FDF9
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x00041C01 File Offset: 0x0003FE01
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value < 0L || value >= this._length)
				{
					throw new ArgumentOutOfRangeException("value", value, string.Empty);
				}
				this._position = value;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00041C2E File Offset: 0x0003FE2E
		public virtual bool IsDisposed
		{
			get
			{
				return this._baseStream == null;
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00041C3C File Offset: 0x0003FE3C
		public override int ReadByte()
		{
			this.CheckDisposed();
			long num = Math.Min(this._position + 1L, this._length);
			int result;
			if (this.Sync())
			{
				result = this._baseStream.ReadByte();
				this._position = this._baseStream.Position - this._offset;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00041C98 File Offset: 0x0003FE98
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckDisposed();
			long num = Math.Min(this._position + (long)count, this._length);
			int result = 0;
			if (this.Sync())
			{
				result = this._baseStream.Read(buffer, offset, (int)(num - this._position));
				this._position = this._baseStream.Position - this._offset;
			}
			return result;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00041D38 File Offset: 0x0003FF38
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this._position + offset;
				break;
			case SeekOrigin.End:
				num = this._length + offset;
				break;
			default:
				throw new InvalidEnumArgumentException("origin", (int)origin, typeof(SeekOrigin));
			}
			if (num < 0L || num >= this._length)
			{
				throw new ArgumentOutOfRangeException("offset", offset, string.Empty);
			}
			this.CheckDisposed();
			this._position = num;
			return this._position;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00041D30 File Offset: 0x0003FF30
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00041DC0 File Offset: 0x0003FFC0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._baseStream != null)
			{
				this._refCount.Value--;
				if (this._refCount.Value < 1)
				{
					this._baseStream.Close();
				}
				this._refCount = null;
				this._baseStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00041E19 File Offset: 0x00040019
		public Stream BaseStream
		{
			get
			{
				return this._baseStream;
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00041E21 File Offset: 0x00040021
		private void CheckDisposed()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("BaseStream");
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00041E38 File Offset: 0x00040038
		private bool Sync()
		{
			if (this._position >= 0L && this._position < this._length)
			{
				if (this._position + this._offset != this._baseStream.Position)
				{
					this._baseStream.Seek(this._offset + this._position, SeekOrigin.Begin);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04001232 RID: 4658
		private Stream _baseStream;

		// Token: 0x04001233 RID: 4659
		private long _offset;

		// Token: 0x04001234 RID: 4660
		private long _length;

		// Token: 0x04001235 RID: 4661
		private long _position;

		// Token: 0x04001236 RID: 4662
		private SharedStream.RefCount _refCount;

		// Token: 0x0200084C RID: 2124
		private class RefCount
		{
			// Token: 0x04003CFF RID: 15615
			public int Value;
		}
	}
}
