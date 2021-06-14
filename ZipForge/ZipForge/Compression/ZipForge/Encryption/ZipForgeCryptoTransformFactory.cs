using System;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.ZipForge.Encryption
{
	// Token: 0x02000089 RID: 137
	internal class ZipForgeCryptoTransformFactory
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x00029B50 File Offset: 0x00028B50
		public static BaseZipForgeCryptoTransform GetCryptoTransform(EncryptionAlgorithm alg, DirItem item)
		{
			if (alg == EncryptionAlgorithm.PkzipClassic)
			{
				return new PkzipClassicCryptoTransform();
			}
			switch (alg)
			{
			case EncryptionAlgorithm.Aes128:
				return new AESCryptoTransform(128, 2, item.ActualCompressionMethod);
			case EncryptionAlgorithm.Aes192:
				return new AESCryptoTransform(192, 2, item.ActualCompressionMethod);
			case EncryptionAlgorithm.Aes256:
				return new AESCryptoTransform(256, 2, item.ActualCompressionMethod);
			default:
				throw ExceptionBuilder.Exception(ErrorCode.UnknownEncryptionMethod);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00029BC0 File Offset: 0x00028BC0
		public static EncryptionAlgorithm GetEncryptionAlgorithm(DirItem item)
		{
			if (!item.IsGeneralPurposeFlagBitSet(0))
			{
				return EncryptionAlgorithm.None;
			}
			AESExtraFieldData aesextraFieldData = item.ExtraFields.GetExtraFieldById(39169) as AESExtraFieldData;
			if (aesextraFieldData != null)
			{
				int keyLengthBits = aesextraFieldData.KeyLengthBits;
				if (keyLengthBits == 128)
				{
					return EncryptionAlgorithm.Aes128;
				}
				if (keyLengthBits == 192)
				{
					return EncryptionAlgorithm.Aes192;
				}
				if (keyLengthBits != 256)
				{
					throw new Exception("AES encryption strength specified in extra field is incorrect");
				}
				return EncryptionAlgorithm.Aes256;
			}
			else
			{
				if (item.ExtraFields.GetExtraFieldById(23) != null)
				{
					return EncryptionAlgorithm.Unknown;
				}
				return EncryptionAlgorithm.PkzipClassic;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00029C45 File Offset: 0x00028C45
		public static bool IsAESEncryption(EncryptionAlgorithm alg)
		{
			return alg == EncryptionAlgorithm.Aes128 || alg == EncryptionAlgorithm.Aes192 || alg == EncryptionAlgorithm.Aes256;
		}
	}
}
