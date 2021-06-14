using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A6 RID: 166
	internal struct ZipFileHeader
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x0002C674 File Offset: 0x0002B674
		public void Reset()
		{
			this.signature = 0U;
			this.extractVersion = 0;
			this.genPurposeFlag = 0;
			this.compMethod = 0;
			this.lastModTime = 0;
			this.lastModDate = 0;
			this.crc32 = 0U;
			this.compSize = 0U;
			this.uncompSize = 0U;
			this.nameLength = 0;
			this.extraLength = 0;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002C6CE File Offset: 0x0002B6CE
		public static int SizeOf()
		{
			return 30;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002C6D4 File Offset: 0x0002B6D4
		public void LoadFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this.signature = binaryReader.ReadUInt32();
			this.extractVersion = binaryReader.ReadUInt16();
			this.genPurposeFlag = binaryReader.ReadUInt16();
			this.compMethod = binaryReader.ReadUInt16();
			this.lastModTime = binaryReader.ReadUInt16();
			this.lastModDate = binaryReader.ReadUInt16();
			this.crc32 = binaryReader.ReadUInt32();
			this.compSize = binaryReader.ReadUInt32();
			this.uncompSize = binaryReader.ReadUInt32();
			this.nameLength = binaryReader.ReadUInt16();
			this.extraLength = binaryReader.ReadUInt16();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002C77C File Offset: 0x0002B77C
		public byte[] GetBytes()
		{
			byte[] array = new byte[ZipFileHeader.SizeOf()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.signature);
			binaryWriter.Write(this.extractVersion);
			binaryWriter.Write(this.genPurposeFlag);
			binaryWriter.Write(this.compMethod);
			binaryWriter.Write(this.lastModTime);
			binaryWriter.Write(this.lastModDate);
			binaryWriter.Write(this.crc32);
			binaryWriter.Write(this.compSize);
			binaryWriter.Write(this.uncompSize);
			binaryWriter.Write(this.nameLength);
			binaryWriter.Write(this.extraLength);
			return array;
		}

		// Token: 0x04000407 RID: 1031
		public uint signature;

		// Token: 0x04000408 RID: 1032
		public ushort extractVersion;

		// Token: 0x04000409 RID: 1033
		public ushort genPurposeFlag;

		// Token: 0x0400040A RID: 1034
		public ushort compMethod;

		// Token: 0x0400040B RID: 1035
		public ushort lastModTime;

		// Token: 0x0400040C RID: 1036
		public ushort lastModDate;

		// Token: 0x0400040D RID: 1037
		public uint crc32;

		// Token: 0x0400040E RID: 1038
		public uint compSize;

		// Token: 0x0400040F RID: 1039
		public uint uncompSize;

		// Token: 0x04000410 RID: 1040
		public ushort nameLength;

		// Token: 0x04000411 RID: 1041
		public ushort extraLength;
	}
}
