using System;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200002F RID: 47
	internal class StoreCompressor : BaseCompressor
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x000152F6 File Offset: 0x000142F6
		public override void Init(CompressionDirection direction, byte compressionMode)
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000152F8 File Offset: 0x000142F8
		public override void Init(CompressionDirection direction, byte compressionMode, bool isRealTimeCompress)
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x000152FA File Offset: 0x000142FA
		public override bool CompressBlock(uint blockSize, long currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, ref long compressedDataSize, ref byte[] resultBuffer)
		{
			resultBuffer = new byte[currentBlockSize];
			Array.Copy(sourceBuffer, 0, resultBuffer, 0, (int)currentBlockSize);
			compressedDataSize = currentBlockSize;
			return true;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001531C File Offset: 0x0001431C
		public override bool DecompressBlock(int currentBlockSize, bool isFinalBlock, byte[] sourceBuffer, out long decompressedDataSize)
		{
			bool flag;
			base.DoOnDecompressedBufferReady(sourceBuffer, currentBlockSize, out flag);
			decompressedDataSize = (long)currentBlockSize;
			return true;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0001533C File Offset: 0x0001433C
		public override bool DecompressBlock(int currentBlockSize, ref byte[] sourceBuffer, out long decompressedDataSize, out bool endOfFileDiscovered)
		{
			bool flag;
			base.DoOnDecompressedBufferReady(sourceBuffer, currentBlockSize, out flag);
			decompressedDataSize = (long)currentBlockSize;
			endOfFileDiscovered = false;
			sourceBuffer = new byte[0];
			return true;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00015365 File Offset: 0x00014365
		public override void Close()
		{
		}
	}
}
