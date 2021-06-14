using System;
using System.IO;
using ComponentAce.Compression.Exception;
using ComponentAce.Compression.Libs.PPMd;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x02000032 RID: 50
	internal class PPMdCompressor : BaseCompressor
	{
		// Token: 0x06000211 RID: 529 RVA: 0x00015E85 File Offset: 0x00014E85
		public override void Init(CompressionDirection direction, byte compressionMode)
		{
			this.isInitialized = true;
			this.direction = direction;
			this.compressionMode = compressionMode;
			this.modelOrder = 8;
			this.allocatorSize = 50;
			this.modelRestorationMethod = ModelRestorationMethod.Restart;
			if (direction == CompressionDirection.Compress)
			{
				Allocator.Start(this.allocatorSize);
			}
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00015EC0 File Offset: 0x00014EC0
		public override void Init(CompressionDirection direction, byte compressionMode, bool isRealTimeCompress)
		{
			this.Init(direction, compressionMode);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00015ECC File Offset: 0x00014ECC
		public override bool CompressBlock(uint blockSize, long currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, ref long compressedDataSize, ref byte[] resultBuffer)
		{
			if (!this.isInitialized || this.direction != CompressionDirection.Compress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			MemoryStream memoryStream = new MemoryStream();
			if (this.sourceStream == null)
			{
				ushort value = (ushort)((ModelRestorationMethod)(this.modelOrder - 1) | (ModelRestorationMethod)(this.allocatorSize - 1 << 4) | this.modelRestorationMethod << 12);
				memoryStream.Write(BitConverter.GetBytes(value), 0, 2);
				Model.StartEncoding(this.modelRestorationMethod, this.modelOrder);
				this.sourceStream = new MemoryStream();
			}
			this.sourceStream.SetLength(0L);
			this.sourceStream.Write(sourceBuffer, 0, (int)currentBlockSize);
			this.sourceStream.Seek(0L, SeekOrigin.Begin);
			Model.EncodeBuffer(memoryStream, this.sourceStream, isFinalBlock);
			if (isFinalBlock)
			{
				Model.StopEncoding(memoryStream);
				Allocator.Stop();
			}
			resultBuffer = memoryStream.ToArray();
			compressedDataSize = memoryStream.Length;
			return true;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00015FA8 File Offset: 0x00014FA8
		public override bool DecompressBlock(int currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, out long decompressedDataSize)
		{
			decompressedDataSize = 0L;
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			if (this.sourceStream == null)
			{
				this.sourceStream = new MemoryStream();
				this.sourceStream.Write(sourceBuffer, 0, currentBlockSize);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
				ushort num = new BinaryReader(this.sourceStream).ReadUInt16();
				this.modelRestorationMethod = (ModelRestorationMethod)(num >> 12);
				this.allocatorSize = (num << 4 >> 8) + 1;
				this.modelOrder = ((ushort)(num << 12) >> 12) + 1;
				Model.StartDecoding(this.modelOrder, this.modelRestorationMethod);
				Allocator.Start(this.allocatorSize);
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
			MemoryStream memoryStream = new MemoryStream();
			Model.DecodeBuffer(memoryStream, this.sourceStream, isFinalBlock);
			byte[] array2 = memoryStream.ToArray();
			decompressedDataSize = (long)array2.Length;
			bool flag;
			base.DoOnDecompressedBufferReady(array2, array2.Length, out flag);
			if (isFinalBlock)
			{
				Allocator.Stop();
			}
			return true;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00016124 File Offset: 0x00015124
		public override bool DecompressBlock(int currentBlockSize, ref byte[] sourceBuffer, out long decompressedDataSize, out bool endOfFileDiscovered)
		{
			decompressedDataSize = 0L;
			if (!this.isInitialized || this.direction != CompressionDirection.Decompress)
			{
				throw ExceptionBuilder.Exception(ErrorCode.CompressionEngineIsNotInitialized);
			}
			if (this.sourceStream == null)
			{
				this.sourceStream = new MemoryStream();
				this.sourceStream.Write(sourceBuffer, 0, currentBlockSize);
				this.sourceStream.Seek(0L, SeekOrigin.Begin);
				ushort num = new BinaryReader(this.sourceStream).ReadUInt16();
				this.modelRestorationMethod = (ModelRestorationMethod)(num >> 12);
				this.allocatorSize = (num << 4 >> 8) + 1;
				this.modelOrder = ((ushort)(num << 12) >> 12) + 1;
				Model.StartDecoding(this.modelOrder, this.modelRestorationMethod);
				Allocator.Start(this.allocatorSize);
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
			MemoryStream memoryStream = new MemoryStream();
			Model.DecodeBuffer(memoryStream, this.sourceStream, false);
			byte[] array2 = memoryStream.ToArray();
			decompressedDataSize = (long)array2.Length;
			bool flag;
			base.DoOnDecompressedBufferReady(array2, array2.Length, out flag);
			endOfFileDiscovered = (Model.EntryPoint == 0);
			if (endOfFileDiscovered)
			{
				Allocator.Stop();
				int num2 = (int)this.sourceStream.Length - (int)this.sourceStream.Position;
				byte[] array3 = new byte[num2];
				Buffer.BlockCopy(sourceBuffer, (int)this.sourceStream.Position, array3, 0, num2);
				sourceBuffer = array3;
			}
			else
			{
				sourceBuffer = new byte[0];
			}
			return true;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000162F7 File Offset: 0x000152F7
		public override void Close()
		{
			this.isInitialized = false;
			this.sourceStream = null;
		}

		// Token: 0x04000143 RID: 323
		private bool isInitialized;

		// Token: 0x04000144 RID: 324
		private byte compressionMode;

		// Token: 0x04000145 RID: 325
		private CompressionDirection direction;

		// Token: 0x04000146 RID: 326
		private int allocatorSize;

		// Token: 0x04000147 RID: 327
		private int modelOrder;

		// Token: 0x04000148 RID: 328
		private ModelRestorationMethod modelRestorationMethod;

		// Token: 0x04000149 RID: 329
		private MemoryStream sourceStream;
	}
}
