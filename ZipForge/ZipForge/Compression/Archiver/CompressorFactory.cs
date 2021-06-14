using System;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.Archiver
{
	// Token: 0x0200002B RID: 43
	internal class CompressorFactory
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0001525C File Offset: 0x0001425C
		public static BaseCompressor GetCompressor(CompressionMethod method, byte compressionMode)
		{
			if (method <= CompressionMethod.Deflate)
			{
				if (method == CompressionMethod.None)
				{
					return new StoreCompressor();
				}
				if (method == CompressionMethod.Deflate)
				{
					return new DeflateCompressor();
				}
			}
			else
			{
				if (method == CompressionMethod.BZIP2)
				{
					return new BZIP2Compressor();
				}
				if (method == CompressionMethod.PPMd)
				{
					return new PPMdCompressor();
				}
			}
			throw ExceptionBuilder.Exception(ErrorCode.UnknownCompressionMethod);
		}
	}
}
