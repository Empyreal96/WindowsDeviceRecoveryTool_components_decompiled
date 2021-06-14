using System;
using System.IO;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000C2 RID: 194
	public class ZOutputStream : Stream
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x00035E64 File Offset: 0x00034E64
		private void InitBlock()
		{
			this.flush = FlushStrategy.Z_NO_FLUSH;
			this.buf = new byte[1048576];
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00035E7D File Offset: 0x00034E7D
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00035E85 File Offset: 0x00034E85
		public FlushStrategy FlushMode
		{
			get
			{
				return this.flush;
			}
			set
			{
				this.flush = value;
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00035E8E File Offset: 0x00034E8E
		public ZOutputStream(Stream stream, int level)
		{
			this.InitBlock();
			this._stream = stream;
			this.z.deflateInit(level);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00035EC8 File Offset: 0x00034EC8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[buffer.Length];
			Array.Copy(buffer, 0, array, 0, buffer.Length);
			this.z.next_in = array;
			this.z.next_in_index = offset;
			this.z.avail_in = count;
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = 1048576;
				int num = this.z.deflate(this.flush);
				if (num != 0 && num != 1)
				{
					break;
				}
				this._stream.Write(this.buf, 0, 1048576 - this.z.avail_out);
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					return;
				}
			}
			throw new ZStreamException("deflating: " + this.z.msg);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00035FC8 File Offset: 0x00034FC8
		public void Finish()
		{
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = 1048576;
				int num = this.z.deflate(FlushStrategy.Z_FINISH);
				if (num != 1 && num != 0)
				{
					break;
				}
				if (1048576 - this.z.avail_out > 0)
				{
					this._stream.Write(this.buf, 0, 1048576 - this.z.avail_out);
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					goto Block_4;
				}
			}
			throw new ZStreamException("deflating: " + this.z.msg);
			Block_4:
			try
			{
				this.Flush();
			}
			catch
			{
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000360A4 File Offset: 0x000350A4
		public void End()
		{
			this.z.deflateEnd();
			this.z.free();
			this.z = null;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000360C4 File Offset: 0x000350C4
		public override void Close()
		{
			try
			{
				this.Finish();
			}
			catch
			{
			}
			finally
			{
				this.End();
				this._stream.Close();
				this._stream = null;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00036114 File Offset: 0x00035114
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00036121 File Offset: 0x00035121
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("ZOutputStream doesn't support reading");
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0003612D File Offset: 0x0003512D
		public override void SetLength(long value)
		{
			throw new NotSupportedException("ZOutputStream doesn't support seeking");
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00036139 File Offset: 0x00035139
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("ZOutputStream doesn't support seeking");
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00036145 File Offset: 0x00035145
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x00036148 File Offset: 0x00035148
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0003614B File Offset: 0x0003514B
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0003614E File Offset: 0x0003514E
		public override long Length
		{
			get
			{
				throw new NotSupportedException("ZLibStream doesn't support the Length property");
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0003615A File Offset: 0x0003515A
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x00036166 File Offset: 0x00035166
		public override long Position
		{
			get
			{
				throw new NotSupportedException("ZOutputStream doesn't support the Position property");
			}
			set
			{
				throw new NotSupportedException("ZOutputStream doesn't support seeking");
			}
		}

		// Token: 0x04000553 RID: 1363
		private ZStream z = new ZStream();

		// Token: 0x04000554 RID: 1364
		private FlushStrategy flush;

		// Token: 0x04000555 RID: 1365
		private byte[] buf;

		// Token: 0x04000556 RID: 1366
		private byte[] buf1 = new byte[1];

		// Token: 0x04000557 RID: 1367
		private Stream _stream;
	}
}
