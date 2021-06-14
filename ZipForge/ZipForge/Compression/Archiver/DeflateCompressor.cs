using System;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Libs.ZLib;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000030 RID: 48
	internal class DeflateCompressor : BaseCompressor
	{
		// Token: 0x060001FF RID: 511 RVA: 0x00015370 File Offset: 0x00014370
		public override void Init(CompressionDirection direction, byte compressionMode)
		{
			this.isInitialized = true;
			if (direction == CompressionDirection.Compress)
			{
				this.strm.next_out = null;
				this.strm.avail_out = 0;
				this.strm.total_out = 0L;
				try
				{
					if (this.strm.dstate == null)
					{
						this.strm.deflateInit((int)compressionMode);
					}
					else if (this.strm.dstate.level != (int)compressionMode)
					{
						this.strm.deflateEnd();
						this.strm.deflateInit((int)compressionMode);
					}
					else
					{
						this.strm.dstate.deflateReset(this.strm);
					}
				}
				catch
				{
					this.strm.deflateEnd();
					throw;
				}
				this.direction = CompressionDirection.Compress;
			}
			else
			{
				this.strm.next_out = null;
				this.strm.avail_out = 0;
				this.strm.total_out = 0L;
				this.strm.inflateInit();
				this.direction = CompressionDirection.Decompress;
			}
			this.compressionMode = compressionMode;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0001547C File Offset: 0x0001447C
		public override void Init(CompressionDirection direction, byte compressionMode, bool isRealTimeCompress)
		{
			this.Init(direction, compressionMode);
			if (isRealTimeCompress)
			{
				this.strm.inflateInit(-15);
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00015498 File Offset: 0x00014498
		public override bool CompressBlock(uint blockSize, long currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, ref long compressedDataSize, ref byte[] resultBuffer)
		{
			if (!this.isInitialized || this.direction != CompressionDirection.Compress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			int avail_out = (int)currentBlockSize + (int)(currentBlockSize / 10L) + 12 + 255 & -256;
			this.strm.next_in = sourceBuffer;
			this.strm.avail_in = (int)currentBlockSize;
			this.strm.next_out = resultBuffer;
			this.strm.avail_out = avail_out;
			this.strm.next_in_index = 0;
			this.strm.next_out_index = 0;
			long total_out = this.strm.total_out;
			if (currentBlockSize != (long)((ulong)blockSize))
			{
				if (this.strm.deflate(FlushStrategy.Z_FINISH) < 0)
				{
					return false;
				}
			}
			else if (this.strm.deflate(FlushStrategy.Z_SYNC_FLUSH) < 0)
			{
				return false;
			}
			if (compressedDataSize == 0L)
			{
				Array.Copy(resultBuffer, 2, resultBuffer, 0, resultBuffer.Length - 2);
				compressedDataSize = this.strm.total_out - total_out - 2L;
			}
			else
			{
				compressedDataSize = this.strm.total_out - total_out;
			}
			if (isFinalBlock && this.compressionMode > 0)
			{
				compressedDataSize -= 4L;
			}
			return true;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000155B0 File Offset: 0x000145B0
		public override bool DecompressBlock(int currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, out long decompressedDataSize)
		{
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			decompressedDataSize = 0L;
			if (currentBlockSize <= 0)
			{
				return false;
			}
			this.strm.next_in = sourceBuffer;
			this.strm.avail_in = currentBlockSize;
			int num = 1048576;
			byte[] next_out = new byte[num];
			this.strm.next_out = next_out;
			this.strm.avail_out = num;
			if (this.strm.total_in == 0L)
			{
				Array.Copy(sourceBuffer, 0, sourceBuffer, 2, currentBlockSize);
				ushort value = (ushort)this.GetZlibStreamHeader(1);
				byte[] bytes = BitConverter.GetBytes(value);
				Array.Copy(bytes, 0, sourceBuffer, 0, 2);
				this.strm.avail_in = currentBlockSize + 2;
			}
			try
			{
				this.strm.next_in_index = 0;
				this.strm.next_out_index = 0;
				for (;;)
				{
					int num2 = this.strm.inflate(FlushStrategy.Z_NO_FLUSH);
					if (num2 < 0 && num2 != -5)
					{
						break;
					}
					if (num2 == -5)
					{
						goto Block_7;
					}
					decompressedDataSize += (long)(this.strm.next_out.Length - this.strm.avail_out);
					bool flag;
					base.DoOnDecompressedBufferReady(this.strm.next_out, this.strm.next_out.Length - this.strm.avail_out, out flag);
					if (flag)
					{
						goto Block_8;
					}
					if (this.strm.avail_in == 0 && this.strm.avail_out > 0)
					{
						goto IL_182;
					}
					this.strm.next_out_index = 0;
					this.strm.avail_out = this.strm.next_out.Length;
				}
				return false;
				Block_7:
				return true;
				Block_8:
				return false;
				IL_182:;
			}
			catch
			{
				this.strm.inflateEnd();
				throw;
			}
			return true;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00015764 File Offset: 0x00014764
		public override bool DecompressBlock(int currentBlockSize, ref byte[] sourceBuffer, out long decompressedDataSize, out bool endOfFileDiscovered)
		{
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			decompressedDataSize = 0L;
			endOfFileDiscovered = false;
			if (currentBlockSize <= 0)
			{
				return false;
			}
			this.strm.next_in = sourceBuffer;
			this.strm.avail_in = currentBlockSize;
			int num = 1048576;
			byte[] next_out = new byte[num];
			this.strm.next_out = next_out;
			this.strm.avail_out = num;
			try
			{
				this.strm.next_in_index = 0;
				this.strm.next_out_index = 0;
				for (;;)
				{
					int num2 = this.strm.inflate(FlushStrategy.Z_NO_FLUSH);
					if (num2 < 0 && num2 != -5)
					{
						break;
					}
					if (num2 == -5)
					{
						goto Block_7;
					}
					decompressedDataSize += (long)(this.strm.next_out.Length - this.strm.avail_out);
					bool flag;
					base.DoOnDecompressedBufferReady(this.strm.next_out, this.strm.next_out.Length - this.strm.avail_out, out flag);
					if (flag)
					{
						goto Block_8;
					}
					if (this.strm.avail_in == 0 && this.strm.avail_out > 0)
					{
						goto Block_10;
					}
					if (num2 == 1)
					{
						goto Block_11;
					}
					this.strm.next_out_index = 0;
					this.strm.avail_out = this.strm.next_out.Length;
				}
				return false;
				Block_7:
				sourceBuffer = new byte[0];
				return true;
				Block_8:
				return false;
				Block_10:
				sourceBuffer = new byte[0];
				goto IL_191;
				Block_11:
				int num3 = currentBlockSize - this.strm.next_in_index;
				byte[] array = new byte[num3];
				Buffer.BlockCopy(sourceBuffer, this.strm.next_in_index, array, 0, num3);
				sourceBuffer = array;
				endOfFileDiscovered = true;
				IL_191:;
			}
			catch
			{
				this.strm.inflateEnd();
				throw;
			}
			return true;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00015934 File Offset: 0x00014934
		public override void Close()
		{
			if (this.direction == CompressionDirection.Decompress)
			{
				this.strm.inflateEnd();
			}
			this.isInitialized = false;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00015954 File Offset: 0x00014954
		private int EstimateBytesOut(long compressedSize, long uncompressedSize, long size)
		{
			double num = 1.0 / Math.Log(2.0);
			long num2 = (long)Math.Pow(2.0, Math.Round(Math.Log((double)((1L + uncompressedSize / compressedSize) * (size + size / 2L + 12L + 255L) & -256L)) * num));
			return (int)num2;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000159BC File Offset: 0x000149BC
		private int GetZlibStreamHeader(int CompMode)
		{
			switch (CompMode)
			{
			case 0:
			case 7:
			case 8:
			case 9:
				return 55928;
			case 1:
			case 2:
				return 376;
			case 3:
			case 4:
				return 24184;
			case 5:
			case 6:
				return 40056;
			default:
				return 0;
			}
		}

		// Token: 0x04000136 RID: 310
		private bool isInitialized;

		// Token: 0x04000137 RID: 311
		private CompressionDirection direction;

		// Token: 0x04000138 RID: 312
		private byte compressionMode;

		// Token: 0x04000139 RID: 313
		private ZStream strm = new ZStream();
	}
}
