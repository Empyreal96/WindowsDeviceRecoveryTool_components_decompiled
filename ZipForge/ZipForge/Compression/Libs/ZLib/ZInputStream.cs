using System;
using System.IO;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B7 RID: 183
	public class ZInputStream : Stream
	{
		// Token: 0x06000806 RID: 2054 RVA: 0x00034F1E File Offset: 0x00033F1E
		private void InitBlock()
		{
			this.flush = FlushStrategy.Z_NO_FLUSH;
			this.buf = new byte[1048576];
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x00034F37 File Offset: 0x00033F37
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x00034F3F File Offset: 0x00033F3F
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

		// Token: 0x06000809 RID: 2057 RVA: 0x00034F48 File Offset: 0x00033F48
		public ZInputStream(Stream stream)
		{
			this.InitBlock();
			this._stream = stream;
			this.z.inflateInit();
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00034FB4 File Offset: 0x00033FB4
		public override int ReadByte()
		{
			if (this.Read(this.buf1, 0, 1) == -1)
			{
				return -1;
			}
			return (int)(this.buf1[0] & byte.MaxValue);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00034FD8 File Offset: 0x00033FD8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return 0;
			}
			if (this.needCopyArrays && ZLibUtil.CopyLargeArrayToSmall.GetRemainingDataSize() > 0)
			{
				return ZLibUtil.CopyLargeArrayToSmall.CopyData();
			}
			this.needCopyArrays = false;
			bool flag = false;
			this.z.next_out = buffer;
			this.z.next_out_index = offset;
			this.z.avail_out = count;
			for (;;)
			{
				if (this.z.avail_in == 0 && !this.nomoreinput)
				{
					this.z.next_in_index = 0;
					this.z.avail_in = ZLibUtil.ReadInput(this._stream, this.buf, 0, 1048576);
					if (this.z.avail_in == -1)
					{
						this.z.avail_in = 0;
						this.nomoreinput = true;
					}
				}
				if (this.z.avail_in == 0 && this.nomoreinput)
				{
					break;
				}
				int num = this.z.inflate(this.flush);
				if (this.nomoreinput && num == -5)
				{
					return -1;
				}
				if (num != 0 && num != 1)
				{
					goto Block_12;
				}
				if (this.nomoreinput && this.z.avail_out == count)
				{
					return -1;
				}
				if (this.z.avail_out != count || num != 0)
				{
					goto IL_134;
				}
			}
			flag = true;
			goto IL_134;
			Block_12:
			throw new ZStreamException("inflating: " + this.z.msg);
			IL_134:
			if (flag)
			{
				return this.Finish(buffer, offset, count);
			}
			return count - this.z.avail_out;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00035133 File Offset: 0x00034133
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("ZInputStream doesn't support writing");
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00035140 File Offset: 0x00034140
		public long Skip(long n)
		{
			int num = 512;
			if (n < (long)num)
			{
				num = (int)n;
			}
			byte[] array = new byte[num];
			return (long)ZLibUtil.ReadInput(this, array, 0, array.Length);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00035170 File Offset: 0x00034170
		public virtual int Finish(byte[] buffer, int offset, int count)
		{
			int num = 0;
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = 1048576;
				int num2 = this.z.inflate(FlushStrategy.Z_FINISH);
				if (num2 != 1 && num2 != 0)
				{
					break;
				}
				if (1048576 - this.z.avail_out > 0)
				{
					this.needCopyArrays = true;
					ZLibUtil.CopyLargeArrayToSmall.Initialize(this.buf, 0, 1048576 - this.z.avail_out, buffer, offset + num, count);
					int result = ZLibUtil.CopyLargeArrayToSmall.CopyData();
					if (ZLibUtil.CopyLargeArrayToSmall.GetRemainingDataSize() > 0)
					{
						return result;
					}
					num += 1048576 - this.z.avail_out;
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					goto Block_6;
				}
			}
			throw new ZStreamException("inflating: " + this.z.msg);
			Block_6:
			try
			{
				this.Flush();
			}
			catch
			{
			}
			return num;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0003527C File Offset: 0x0003427C
		public virtual void End()
		{
			this.z.inflateEnd();
			this.z.free();
			this.z = null;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0003529C File Offset: 0x0003429C
		public override void Close()
		{
			this.End();
			this._stream.Close();
			this._stream = null;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000352B6 File Offset: 0x000342B6
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000352C3 File Offset: 0x000342C3
		public override void SetLength(long value)
		{
			throw new NotSupportedException("ZInputStream doesn't support SetLength");
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000352CF File Offset: 0x000342CF
		public override long Length
		{
			get
			{
				throw new NotSupportedException("ZLibStream doesn't support the Length property");
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x000352DB File Offset: 0x000342DB
		// (set) Token: 0x06000815 RID: 2069 RVA: 0x000352E7 File Offset: 0x000342E7
		public override long Position
		{
			get
			{
				throw new NotSupportedException("ZInputStream doesn't support the Position property");
			}
			set
			{
				throw new NotSupportedException("ZInputStream doesn't support seeking");
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x000352F3 File Offset: 0x000342F3
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("ZInputStream doesn't support seeking");
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000352FF File Offset: 0x000342FF
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00035302 File Offset: 0x00034302
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00035305 File Offset: 0x00034305
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040004FF RID: 1279
		private ZStream z = new ZStream();

		// Token: 0x04000500 RID: 1280
		private FlushStrategy flush;

		// Token: 0x04000501 RID: 1281
		private byte[] buf;

		// Token: 0x04000502 RID: 1282
		private byte[] buf1 = new byte[1];

		// Token: 0x04000503 RID: 1283
		private Stream _stream;

		// Token: 0x04000504 RID: 1284
		private bool nomoreinput;

		// Token: 0x04000505 RID: 1285
		private bool needCopyArrays;
	}
}
