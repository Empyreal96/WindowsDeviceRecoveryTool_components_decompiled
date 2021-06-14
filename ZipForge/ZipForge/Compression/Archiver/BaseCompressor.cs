using System;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200002D RID: 45
	internal abstract class BaseCompressor
	{
		// Token: 0x060001EA RID: 490
		public abstract void Init(CompressionDirection direction, byte compressionMode);

		// Token: 0x060001EB RID: 491
		public abstract void Init(CompressionDirection direction, byte compressionMode, bool isRealTimeCompress);

		// Token: 0x060001EC RID: 492
		public abstract bool CompressBlock(uint blockSize, long currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, ref long compressedDataSize, ref byte[] resultBuffer);

		// Token: 0x060001ED RID: 493
		public abstract bool DecompressBlock(int currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, out long decompressedDataSize);

		// Token: 0x060001EE RID: 494
		public abstract bool DecompressBlock(int currentBlockSize, ref byte[] sourceBuffer, out long decompressedDataSize, out bool endOfFileDiscovered);

		// Token: 0x060001EF RID: 495
		public abstract void Close();

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001F0 RID: 496 RVA: 0x000152AC File Offset: 0x000142AC
		// (remove) Token: 0x060001F1 RID: 497 RVA: 0x000152C5 File Offset: 0x000142C5
		public event BaseCompressor.DecompressedBufferReady OnDecompressedBufferReady;

		// Token: 0x060001F2 RID: 498 RVA: 0x000152DE File Offset: 0x000142DE
		protected void DoOnDecompressedBufferReady(byte[] buffer, int outBytes, out bool stopDecompression)
		{
			this.OnDecompressedBufferReady(buffer, outBytes, out stopDecompression);
		}

		// Token: 0x0200002E RID: 46
		// (Invoke) Token: 0x060001F5 RID: 501
		public delegate void DecompressedBufferReady(byte[] buffer, int outBytes, out bool stopDecompression);
	}
}
