using System;
using System.IO;
using System.Security.Cryptography;
using ComponentAce.Encryption;

namespace ComponentAce.Compression.ZipForge.Encryption
{
	// Token: 0x0200008F RID: 143
	internal class AESCryptoTransform : BaseZipForgeCryptoTransform
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x0002A53A File Offset: 0x0002953A
		public AESCryptoTransform(int strength, ushort version, ushort actualCompressionMethod)
		{
			this.keyLengthBytes = (ushort)(strength / 8);
			this.version = version;
			this.actualCompressionMethod = actualCompressionMethod;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002A55A File Offset: 0x0002955A
		public static AESCryptoTransform CreateAESEncryption(AESExtraFieldData extraField)
		{
			return new AESCryptoTransform(extraField.KeyLengthBits / 8, extraField.VersionNumber, extraField.CompressionMethod);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002A578 File Offset: 0x00029578
		public override void GenerateKey(string password)
		{
			this.password = password;
			if (base.TransformMode == CryptoTransformMode.Decryption && this.saltValue != null)
			{
				this.keyGenerator = new PBKDF2(this.password, this.saltValue, 1000);
			}
			else
			{
				this.keyGenerator = new PBKDF2(this.password, (int)(this.keyLengthBytes / 2), 1000);
				this.saltValue = this.keyGenerator.Salt;
			}
			this.UpdateKeys();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002A5F0 File Offset: 0x000295F0
		private void UpdateKeys()
		{
			this.keyGenerator = new PBKDF2(this.password, this.saltValue, 1000);
			this.encryptionKey = this.keyGenerator.GetKeyBytes((int)this.keyLengthBytes);
			this.hashKey = this.keyGenerator.GetKeyBytes((int)this.keyLengthBytes);
			this.hmacSHA1 = new HMACSHA1(this.hashKey);
			this.cryptor = new RijndaelCipher(this.encryptionKey);
			this.cryptor.Mode = CipherBlockMode.CTR;
			this.passwordVerificationValue = this.keyGenerator.GetKeyBytes(2);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002A688 File Offset: 0x00029688
		public override bool CheckPassword(string password, DirItem item)
		{
			PBKDF2 pbkdf = new PBKDF2(password, this.saltValue, 1000);
			pbkdf.GetKeyBytes((int)this.keyLengthBytes);
			pbkdf.GetKeyBytes((int)this.keyLengthBytes);
			byte[] keyBytes = pbkdf.GetKeyBytes(2);
			return this.passwordVerificationValue[0] == keyBytes[0] && this.passwordVerificationValue[0] == keyBytes[0];
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002A6E8 File Offset: 0x000296E8
		public override bool IsDirItemCorrupted(DirItem item, uint crc32)
		{
			if (base.TransformMode != CryptoTransformMode.Decryption)
			{
				throw new Exception("The directory item corruption test is only possible in the decryption mode");
			}
			if (this.version == 1)
			{
				return crc32 != item.CRC32;
			}
			this.hmacSHA1.TransformFinalBlock(new byte[0], 0, 0);
			for (int i = 0; i < 10; i++)
			{
				if (this.hmacSHA1.Hash[i] != this.authenticationCode[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002A75A File Offset: 0x0002975A
		public override byte[] GetKey()
		{
			return (byte[])this.encryptionKey.Clone();
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002A76C File Offset: 0x0002976C
		public override string GetPassword()
		{
			return this.password;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002A774 File Offset: 0x00029774
		public override void Initialize(CryptoTransformMode mode, DirItem item)
		{
			base.Initialize(mode, item);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002A77E File Offset: 0x0002977E
		public override void Reset()
		{
			this.password = null;
			this.keyLengthBytes = 0;
			this.encryptionKey = null;
			this.hashKey = null;
			this.keyGenerator = null;
			this.passwordVerificationValue = null;
			this.saltValue = null;
			this.cryptor = null;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002A7B8 File Offset: 0x000297B8
		public override int EncryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			byte[] array = new byte[inputCount];
			Array.Copy(inputBuffer, inputOffset, array, 0, inputCount);
			this.cryptor.EncodeBuffer(array, array, inputCount);
			Array.Copy(array, 0, outputBuffer, outputOffset, inputCount);
			this.hmacSHA1.TransformBlock(array, 0, inputCount, outputBuffer, outputOffset);
			return inputCount;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002A804 File Offset: 0x00029804
		public override int DecryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			this.hmacSHA1.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
			byte[] array = new byte[inputCount];
			Array.Copy(outputBuffer, outputOffset, array, 0, inputCount);
			this.cryptor.DecodeBuffer(array, array, inputCount);
			Array.Copy(array, 0, outputBuffer, outputOffset, inputCount);
			return inputCount;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002A854 File Offset: 0x00029854
		public override ExtraFieldData GetExtraFieldData()
		{
			byte strength = 0;
			ushort num = this.keyLengthBytes;
			if (num != 16)
			{
				if (num != 24)
				{
					if (num == 32)
					{
						strength = 3;
					}
				}
				else
				{
					strength = 2;
				}
			}
			else
			{
				strength = 1;
			}
			return new AESExtraFieldData(this.version, strength, this.actualCompressionMethod);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002A898 File Offset: 0x00029898
		public override byte[] GetFileStorageStartBlock()
		{
			if (this.saltValue == null || this.passwordVerificationValue == null)
			{
				throw new Exception("The encryption key hasn'newDirItem been generated yet");
			}
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(this.saltValue);
			binaryWriter.Write(this.passwordVerificationValue);
			return memoryStream.ToArray();
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002A8EC File Offset: 0x000298EC
		public override void LoadFileStorageStartBlock(Stream stream, long offset)
		{
			int count = (int)(this.keyLengthBytes / 2);
			stream.Position = offset;
			BinaryReader binaryReader = new BinaryReader(stream);
			this.saltValue = binaryReader.ReadBytes(count);
			this.UpdateKeys();
			this.passwordVerificationValue = binaryReader.ReadBytes(2);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002A930 File Offset: 0x00029930
		public override int GetFileStorageStartBlockSize()
		{
			return (int)(this.keyLengthBytes / 2 + 2);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002A93C File Offset: 0x0002993C
		public override byte[] GetFileStorageEndBlock()
		{
			if (this.version != 1)
			{
				this.hmacSHA1.TransformFinalBlock(new byte[0], 0, 0);
				this.authenticationCode = new byte[10];
				Array.Copy(this.hmacSHA1.Hash, 0, this.authenticationCode, 0, 10);
				return (byte[])this.authenticationCode.Clone();
			}
			return new byte[0];
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002A9A4 File Offset: 0x000299A4
		public override void LoadFileStorageEndBlock(Stream stream, long offset)
		{
			if (this.version != 1)
			{
				stream.Position = offset;
				BinaryReader binaryReader = new BinaryReader(stream);
				this.authenticationCode = binaryReader.ReadBytes(10);
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002A9D6 File Offset: 0x000299D6
		public override int GetFileStorageEndBlockSize()
		{
			if (this.version != 1)
			{
				return 10;
			}
			return 0;
		}

		// Token: 0x04000377 RID: 887
		public const int AuthenticationCodeSize = 10;

		// Token: 0x04000378 RID: 888
		private PBKDF2 keyGenerator;

		// Token: 0x04000379 RID: 889
		private HMACSHA1 hmacSHA1;

		// Token: 0x0400037A RID: 890
		private RijndaelCipher cryptor;

		// Token: 0x0400037B RID: 891
		private ushort keyLengthBytes;

		// Token: 0x0400037C RID: 892
		private byte[] hashKey;

		// Token: 0x0400037D RID: 893
		private byte[] encryptionKey;

		// Token: 0x0400037E RID: 894
		private byte[] passwordVerificationValue;

		// Token: 0x0400037F RID: 895
		private string password;

		// Token: 0x04000380 RID: 896
		private byte[] saltValue;

		// Token: 0x04000381 RID: 897
		private byte[] authenticationCode;

		// Token: 0x04000382 RID: 898
		private ushort version;

		// Token: 0x04000383 RID: 899
		private ushort actualCompressionMethod;
	}
}
