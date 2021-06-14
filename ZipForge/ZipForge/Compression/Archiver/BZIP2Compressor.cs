using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Libs.bzip2;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000031 RID: 49
	internal class BZIP2Compressor : BaseCompressor
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00015A28 File Offset: 0x00014A28
		public override void Init(CompressionDirection direction, byte compressionMode)
		{
			this.isInitialized = true;
			this.direction = direction;
			this.compressionMode = compressionMode;
			if (direction == CompressionDirection.Compress)
			{
				this.resStream = new MemoryStream();
				this.resStream.Write(Encoding.ASCII.GetBytes("BZ"), 0, 2);
				this.compressStream = new CBZip2OutputStream(this.resStream, (int)(compressionMode * 100));
				return;
			}
			this.sourceStream = new MemoryStream();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00015A95 File Offset: 0x00014A95
		public override void Init(CompressionDirection direction, byte compressionMode, bool isRealTimeCompress)
		{
			this.Init(direction, compressionMode);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00015AA0 File Offset: 0x00014AA0
		public override bool CompressBlock(uint blockSize, long currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, ref long compressedDataSize, ref byte[] resultBuffer)
		{
			if (!this.isInitialized || this.direction != CompressionDirection.Compress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			MemoryStream input = new MemoryStream(sourceBuffer, 0, (int)currentBlockSize);
			this.CopyStream(input, this.compressStream);
			if (isFinalBlock)
			{
				this.compressStream.Close();
				resultBuffer = this.resStream.ToArray();
				compressedDataSize = (long)resultBuffer.Length;
			}
			else
			{
				resultBuffer = this.resStream.ToArray();
				this.resStream.SetLength(0L);
				compressedDataSize = (long)resultBuffer.Length;
			}
			return true;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00015B2C File Offset: 0x00014B2C
		public override bool DecompressBlock(int currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, out long decompressedDataSize)
		{
			decompressedDataSize = 0L;
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			if (this.decompressStream == null)
			{
				this.sourceStream.Write(sourceBuffer, 2, currentBlockSize - 2);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
				this.decompressStream = new CBZip2InputStream(this.sourceStream);
			}
			else
			{
				byte[] array = new byte[this.sourceStream.Length - this.sourceStream.Position];
				this.sourceStream.Read(array, 0, (int)(this.sourceStream.Length - this.sourceStream.Position));
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
				this.sourceStream.SetLength(0L);
				this.sourceStream.Write(array, 0, array.Length);
				this.sourceStream.Write(sourceBuffer, 0, currentBlockSize);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
			}
			byte[] array2;
			if (!isFinalBlock)
			{
				MemoryStream memoryStream = new MemoryStream();
				this.CopyStream(this.decompressStream, memoryStream, (int)this.decompressStream.Length);
				array2 = memoryStream.ToArray();
				decompressedDataSize = (long)array2.Length;
			}
			else
			{
				MemoryStream memoryStream2 = new MemoryStream();
				this.CopyStream(this.decompressStream, memoryStream2);
				array2 = memoryStream2.ToArray();
				decompressedDataSize = (long)array2.Length;
			}
			bool flag;
			base.DoOnDecompressedBufferReady(array2, array2.Length, out flag);
			return true;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00015C84 File Offset: 0x00014C84
		public override bool DecompressBlock(int currentBlockSize, ref byte[] sourceBuffer, out long decompressedDataSize, out bool endOfFileDiscovered)
		{
			decompressedDataSize = 0L;
			long num = 0L;
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			if (this.decompressStream == null)
			{
				this.sourceStream.Write(sourceBuffer, 2, currentBlockSize - 2);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
				this.decompressStream = new CBZip2InputStream(this.sourceStream);
				num += 2L;
			}
			else
			{
				this.sourceStream.SetLength(0L);
				this.sourceStream.Write(sourceBuffer, 0, currentBlockSize);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
			}
			MemoryStream memoryStream = new MemoryStream();
			long num2 = (long)this.CopyStream(this.decompressStream, memoryStream, (int)this.decompressStream.Length);
			num += this.decompressStream.Position;
			byte[] buffer = memoryStream.ToArray();
			decompressedDataSize = num2;
			bool flag;
			base.DoOnDecompressedBufferReady(buffer, (int)num2, out flag);
			endOfFileDiscovered = this.decompressStream.IsStreamEndReached;
			if (endOfFileDiscovered)
			{
				long num3 = (long)sourceBuffer.Length - num;
				byte[] array = new byte[num3];
				Buffer.BlockCopy(sourceBuffer, (int)num, array, 0, (int)num3);
				sourceBuffer = array;
			}
			else
			{
				byte[] array2 = new byte[this.sourceStream.Length - this.sourceStream.Position];
				this.sourceStream.Read(array2, 0, (int)(this.sourceStream.Length - this.sourceStream.Position));
				sourceBuffer = array2;
			}
			return true;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00015DE6 File Offset: 0x00014DE6
		public override void Close()
		{
			this.isInitialized = false;
			this.decompressStream = null;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00015DF8 File Offset: 0x00014DF8
		private int CopyStream(Stream input, Stream output)
		{
			int num = 0;
			int num2;
			while ((num2 = input.Read(this.tempBuffer, 0, 2048)) > 0)
			{
				num += num2;
				output.Write(this.tempBuffer, 0, num2);
			}
			return num;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00015E34 File Offset: 0x00014E34
		private int CopyStream(Stream input, Stream output, int readSize)
		{
			byte[] buffer;
			if (readSize > 2048)
			{
				buffer = new byte[readSize];
			}
			else
			{
				buffer = this.tempBuffer;
			}
			int result = input.Read(buffer, 0, readSize);
			output.Write(buffer, 0, readSize);
			return result;
		}

		// Token: 0x0400013A RID: 314
		private const int TransferBlockSize = 2048;

		// Token: 0x0400013B RID: 315
		private bool isInitialized;

		// Token: 0x0400013C RID: 316
		private byte compressionMode;

		// Token: 0x0400013D RID: 317
		private CompressionDirection direction;

		// Token: 0x0400013E RID: 318
		private CBZip2InputStream decompressStream;

		// Token: 0x0400013F RID: 319
		private CBZip2OutputStream compressStream;

		// Token: 0x04000140 RID: 320
		private MemoryStream resStream;

		// Token: 0x04000141 RID: 321
		private MemoryStream sourceStream;

		// Token: 0x04000142 RID: 322
		private byte[] tempBuffer = new byte[2048];
	}
}
