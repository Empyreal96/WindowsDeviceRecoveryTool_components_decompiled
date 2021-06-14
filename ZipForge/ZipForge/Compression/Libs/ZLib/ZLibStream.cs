using System;
using System.IO;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000C1 RID: 193
	public sealed class ZLibStream : Stream
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00035C4A File Offset: 0x00034C4A
		public override bool CanRead
		{
			get
			{
				return this.compressionDirection == CompressionDirection.Decompression && this._stream.CanRead;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00035C62 File Offset: 0x00034C62
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00035C65 File Offset: 0x00034C65
		public override bool CanWrite
		{
			get
			{
				return this.compressionDirection == CompressionDirection.Compression && this._stream.CanWrite;
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00035C7C File Offset: 0x00034C7C
		public override void Flush()
		{
			if (this.compressionDirection == CompressionDirection.Compression)
			{
				this._stream.Flush();
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00035C91 File Offset: 0x00034C91
		public override long Length
		{
			get
			{
				throw new NotSupportedException("ZLibStream doesn't support the Length property");
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00035C9D File Offset: 0x00034C9D
		// (set) Token: 0x06000833 RID: 2099 RVA: 0x00035CA9 File Offset: 0x00034CA9
		public override long Position
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00035CB5 File Offset: 0x00034CB5
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.compressionDirection == CompressionDirection.Compression)
			{
				throw new NotSupportedException("You cannot read from the compression stream");
			}
			return this.decompressionStream.Read(buffer, offset, count);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00035CD8 File Offset: 0x00034CD8
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("ZLibStream doesn't support the Seek operation");
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00035CE4 File Offset: 0x00034CE4
		public override void SetLength(long value)
		{
			throw new NotSupportedException("ZLibStream doesn't support the SetLength operation");
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00035CF0 File Offset: 0x00034CF0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.compressionDirection == CompressionDirection.Decompression)
			{
				throw new NotSupportedException("You cannot write to the decompression stream");
			}
			this.compressionStream.Write(buffer, offset, count);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00035D14 File Offset: 0x00034D14
		public override void Close()
		{
			if (this.compressionDirection == CompressionDirection.Compression)
			{
				this.compressionStream.Close();
			}
			else
			{
				this.decompressionStream.Close();
			}
			if (!this.leaveOpen)
			{
				this._stream.Close();
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00035D4C File Offset: 0x00034D4C
		public ZLibStream(Stream stream, CompressionDirection dir, bool leaveOpen)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("Stream to decompression cannot be null", "stream");
			}
			this.compressionDirection = dir;
			this._stream = stream;
			this.leaveOpen = leaveOpen;
			if (dir == CompressionDirection.Compression)
			{
				if (!this._stream.CanWrite)
				{
					throw new ArgumentException("The stream is not writable", "stream");
				}
				this.compressionStream = new ZOutputStream(this._stream, this.compLevel);
				return;
			}
			else
			{
				if (!this._stream.CanRead)
				{
					throw new ArgumentException("The stream is not readable", "stream");
				}
				this.decompressionStream = new ZInputStream(this._stream);
				return;
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00035DFB File Offset: 0x00034DFB
		public CompressionDirection GetCompressionDirection()
		{
			return this.compressionDirection;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00035E04 File Offset: 0x00034E04
		public void SetCompressionLevel(int level)
		{
			if (level < -1 || level > 9)
			{
				throw new ArgumentException("Invalid compression level is specified", "level");
			}
			if (this.compressionDirection == CompressionDirection.Decompression)
			{
				throw new NotSupportedException("The compression level cannot be set for decompression stream");
			}
			this.compLevel = level;
			this.compressionStream = new ZOutputStream(this._stream, this.compLevel);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00035E5C File Offset: 0x00034E5C
		public Stream BaseStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x0400054D RID: 1357
		private CompressionDirection compressionDirection;

		// Token: 0x0400054E RID: 1358
		private Stream _stream;

		// Token: 0x0400054F RID: 1359
		private ZInputStream decompressionStream;

		// Token: 0x04000550 RID: 1360
		private ZOutputStream compressionStream;

		// Token: 0x04000551 RID: 1361
		private int compLevel = -1;

		// Token: 0x04000552 RID: 1362
		private bool leaveOpen = true;
	}
}
