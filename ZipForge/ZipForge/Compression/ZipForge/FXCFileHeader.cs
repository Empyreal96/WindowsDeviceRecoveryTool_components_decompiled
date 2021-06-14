using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x0200009E RID: 158
	internal class FXCFileHeader
	{
		// Token: 0x06000729 RID: 1833 RVA: 0x0002BDE0 File Offset: 0x0002ADE0
		public void Reset()
		{
			this.BlockSize = 0;
			this.CompSize = 0;
			this.UncompSize = 0;
			this.FileCrc32 = 0U;
			this.NumBlocks = 0;
			this.CompressionAlgorithm = 0;
			this.CompressionMode = 0;
			this.CryptoAlgorithm = 0;
			this.ControlBlockCrc32 = 0U;
			this.ControlBlock = new byte[16];
			this.extraLength = 0;
			this.reserved = 0;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002BE47 File Offset: 0x0002AE47
		public static int SizeOf()
		{
			return 50;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002BE4C File Offset: 0x0002AE4C
		public byte[] GetBytes()
		{
			byte[] array = new byte[FXCFileHeader.SizeOf()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.BlockSize);
			binaryWriter.Write(this.CompSize);
			binaryWriter.Write(this.UncompSize);
			binaryWriter.Write(this.FileCrc32);
			binaryWriter.Write(this.NumBlocks);
			binaryWriter.Write(this.CompressionAlgorithm);
			binaryWriter.Write(this.CompressionMode);
			binaryWriter.Write(this.CryptoAlgorithm);
			binaryWriter.Write(this.ControlBlockCrc32);
			binaryWriter.Write(this.ControlBlock);
			binaryWriter.Write(this.extraLength);
			binaryWriter.Write(this.reserved);
			return array;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002BF04 File Offset: 0x0002AF04
		public void LoadFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this.BlockSize = binaryReader.ReadInt32();
			this.CompSize = binaryReader.ReadInt32();
			this.UncompSize = binaryReader.ReadInt32();
			this.FileCrc32 = binaryReader.ReadUInt32();
			this.NumBlocks = binaryReader.ReadInt32();
			this.CompressionAlgorithm = binaryReader.ReadByte();
			this.CompressionMode = binaryReader.ReadByte();
			this.CryptoAlgorithm = binaryReader.ReadUInt16();
			this.ControlBlockCrc32 = binaryReader.ReadUInt32();
			this.ControlBlock = binaryReader.ReadBytes(16);
			this.extraLength = binaryReader.ReadUInt16();
			this.reserved = binaryReader.ReadInt32();
		}

		// Token: 0x040003CE RID: 974
		public int BlockSize;

		// Token: 0x040003CF RID: 975
		public int CompSize;

		// Token: 0x040003D0 RID: 976
		public int UncompSize;

		// Token: 0x040003D1 RID: 977
		public uint FileCrc32;

		// Token: 0x040003D2 RID: 978
		public int NumBlocks;

		// Token: 0x040003D3 RID: 979
		public byte CompressionAlgorithm;

		// Token: 0x040003D4 RID: 980
		public byte CompressionMode;

		// Token: 0x040003D5 RID: 981
		public ushort CryptoAlgorithm;

		// Token: 0x040003D6 RID: 982
		public uint ControlBlockCrc32;

		// Token: 0x040003D7 RID: 983
		public byte[] ControlBlock = new byte[16];

		// Token: 0x040003D8 RID: 984
		public ushort extraLength;

		// Token: 0x040003D9 RID: 985
		public int reserved;
	}
}
