using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ComponentAce.Compression.ZipForge.Encryption
{
	// Token: 0x0200008C RID: 140
	internal class PkzipClassicCryptoTransform : BaseZipForgeCryptoTransform
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x00029FD8 File Offset: 0x00028FD8
		public override void GenerateKey(string password)
		{
			this.password = password;
			int oemcodePage = CultureInfo.CurrentCulture.TextInfo.OEMCodePage;
			byte[] bytes = Encoding.GetEncoding(oemcodePage).GetBytes(password);
			this.key[0] = 305419896L;
			this.key[1] = 591751049L;
			this.key[2] = 878082192L;
			for (int i = 0; i < bytes.Length; i++)
			{
				this.ZipUpdateKeys(bytes[i]);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002A058 File Offset: 0x00029058
		public override bool CheckPassword(string password, DirItem item)
		{
			if (this.zipKeyHeaderBlock == null)
			{
				throw new Exception("The file storage start block is empty");
			}
			byte b;
			if (item.IsGeneralPurposeFlagBitSet(3))
			{
				b = ZipUtil.HIBYTE(ZipUtil.LOWORD((uint)item.LastModificationTime));
			}
			else
			{
				b = ZipUtil.HIBYTE(ZipUtil.HIWORD(item.CRC32));
			}
			return this.zipKeyHeaderBlock[11] == b;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002A0B8 File Offset: 0x000290B8
		public override byte[] GetKey()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			foreach (long value in this.key.fArray)
			{
				binaryWriter.Write(value);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002A102 File Offset: 0x00029102
		public override string GetPassword()
		{
			return this.password;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002A10A File Offset: 0x0002910A
		public override void Initialize(CryptoTransformMode mode, DirItem item)
		{
			base.Initialize(mode, item);
			if (mode == CryptoTransformMode.Encryption)
			{
				this.fileCRCValue = item.CRC32;
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002A123 File Offset: 0x00029123
		public override void Reset()
		{
			this.key = new PkzipClassicCryptoTransform.ZipKey();
			this.password = "";
			this.fileCRCValue = 0U;
			this.zipKeyHeaderBlock = new PkzipClassicCryptoTransform.ZipKeyHeader();
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002A150 File Offset: 0x00029150
		public override int EncryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			for (long num = (long)inputOffset; num < (long)(inputOffset + inputCount); num += 1L)
			{
				binaryWriter.Write(this.ZipEncryptByte(inputBuffer[(int)(checked((IntPtr)num))]));
			}
			Array.Copy(memoryStream.ToArray(), 0, outputBuffer, outputOffset, inputCount);
			return inputCount;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002A1A0 File Offset: 0x000291A0
		public override int DecryptBuffer(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			for (long num = (long)inputOffset; num < (long)(inputOffset + inputCount); num += 1L)
			{
				byte b = inputBuffer[(int)(checked((IntPtr)num))];
				byte b2 = b ^ this.ZipDecryptByte(this.key);
				this.ZipUpdateKeys(b2);
				binaryWriter.Write(b2);
			}
			Array.Copy(memoryStream.ToArray(), 0, outputBuffer, outputOffset, inputCount);
			return inputCount;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002A208 File Offset: 0x00029208
		public override byte[] GetFileStorageStartBlock()
		{
			if (this.zipKeyHeaderBlock == null)
			{
				this.zipKeyHeaderBlock = new PkzipClassicCryptoTransform.ZipKeyHeader();
			}
			MemoryStream memoryStream = new MemoryStream();
			int num = PkzipClassicCryptoTransform.ZipKeyHeader.SizeOf() - 3;
			if (this.zipKeyHeaderBlock == null)
			{
				throw new Exception("The file storage start block is empty");
			}
			for (int i = 0; i <= num; i++)
			{
				this.zipKeyHeaderBlock[i] = (byte)(new Random(DateTime.Now.Millisecond).Next(int.MaxValue) % 256);
			}
			this.zipKeyHeaderBlock[num + 1] = (byte)(this.fileCRCValue >> 16);
			this.zipKeyHeaderBlock[num + 2] = (byte)(this.fileCRCValue >> 24);
			for (int j = 0; j <= num + 2; j++)
			{
				byte value = this.ZipEncryptByte(this.zipKeyHeaderBlock[j]);
				memoryStream.WriteByte(value);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002A2E8 File Offset: 0x000292E8
		public override void LoadFileStorageStartBlock(Stream stream, long offset)
		{
			stream.Position = offset;
			this.zipKeyHeaderBlock = new PkzipClassicCryptoTransform.ZipKeyHeader();
			stream.Read(this.zipKeyHeaderBlock.fArray, 0, PkzipClassicCryptoTransform.ZipKeyHeader.SizeOf());
			byte[] bytes = this.zipKeyHeaderBlock.GetBytes();
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i] ^ this.ZipDecryptByte(this.key);
				this.ZipUpdateKeys(b);
				bytes[i] = b;
			}
			this.zipKeyHeaderBlock.LoadFromByteArray(bytes, 0U);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002A363 File Offset: 0x00029363
		public override int GetFileStorageStartBlockSize()
		{
			return PkzipClassicCryptoTransform.ZipKeyHeader.SizeOf();
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002A36A File Offset: 0x0002936A
		public override byte[] GetFileStorageEndBlock()
		{
			return new byte[0];
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002A372 File Offset: 0x00029372
		public override ExtraFieldData GetExtraFieldData()
		{
			return null;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002A375 File Offset: 0x00029375
		public override void LoadFileStorageEndBlock(Stream stream, long offset)
		{
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002A377 File Offset: 0x00029377
		public override int GetFileStorageEndBlockSize()
		{
			return 0;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002A37A File Offset: 0x0002937A
		public override bool IsDirItemCorrupted(DirItem item, uint crc32)
		{
			return crc32 != item.CRC32;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002A388 File Offset: 0x00029388
		private byte ZipEncryptByte(byte c)
		{
			byte b = this.ZipDecryptByte(this.key);
			this.ZipUpdateKeys(c);
			return b ^ c;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002A3B0 File Offset: 0x000293B0
		private void ZipUpdateKeys(byte symbol)
		{
			this.key[0] = (long)((ulong)ZipUtil.UpdCRC(symbol, (uint)this.key[0]));
			this.key[1] = this.key[1] + (this.key[0] & 255L);
			this.key[1] = this.key[1] * 134775813L + 1L;
			this.key[2] = (long)((ulong)ZipUtil.UpdCRC((byte)(this.key[1] >> 24), (uint)this.key[2]));
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002A45C File Offset: 0x0002945C
		private byte ZipDecryptByte(PkzipClassicCryptoTransform.ZipKey Key)
		{
			long num = this.key[2] | 2L;
			num = (long)((ulong)((ushort)(num * (num ^ 1L))));
			return (byte)(num >> 8);
		}

		// Token: 0x04000371 RID: 881
		private PkzipClassicCryptoTransform.ZipKey key = new PkzipClassicCryptoTransform.ZipKey();

		// Token: 0x04000372 RID: 882
		private string password;

		// Token: 0x04000373 RID: 883
		private PkzipClassicCryptoTransform.ZipKeyHeader zipKeyHeaderBlock;

		// Token: 0x04000374 RID: 884
		private uint fileCRCValue;

		// Token: 0x0200008D RID: 141
		internal class ZipKey
		{
			// Token: 0x1700011D RID: 285
			public long this[int index]
			{
				get
				{
					return this.fArray[index];
				}
				set
				{
					this.fArray[index] = value;
				}
			}

			// Token: 0x04000375 RID: 885
			internal long[] fArray = new long[3];
		}

		// Token: 0x0200008E RID: 142
		internal class ZipKeyHeader
		{
			// Token: 0x1700011E RID: 286
			public byte this[int index]
			{
				get
				{
					return this.fArray[index];
				}
				set
				{
					this.fArray[index] = value;
				}
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x0002A4D7 File Offset: 0x000294D7
			public static int SizeOf()
			{
				return 12;
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x0002A4DB File Offset: 0x000294DB
			public void LoadFromByteArray(byte[] tb, uint offset)
			{
				Array.Copy(tb, (int)offset, this.fArray, 0, 12);
			}

			// Token: 0x060006A8 RID: 1704 RVA: 0x0002A4F0 File Offset: 0x000294F0
			public byte[] GetBytes()
			{
				byte[] array = new byte[12];
				Array.Copy(this.fArray, 0, array, 0, 12);
				return array;
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x0002A516 File Offset: 0x00029516
			public void Reset()
			{
				this.fArray = new byte[12];
			}

			// Token: 0x04000376 RID: 886
			internal byte[] fArray = new byte[12];
		}
	}
}
