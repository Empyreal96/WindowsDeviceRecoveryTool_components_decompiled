using System;
using System.IO;

namespace ComponentAce.Compression.GZip
{
	// Token: 0x02000041 RID: 65
	internal class GzipHeader
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00018084 File Offset: 0x00017084
		public void LoadFromByteArray(byte[] source)
		{
			MemoryStream input = new MemoryStream(source);
			BinaryReader binaryReader = new BinaryReader(input);
			this.Id1 = binaryReader.ReadByte();
			this.Id2 = binaryReader.ReadByte();
			this.CompressionMethod = binaryReader.ReadByte();
			this.Flag = binaryReader.ReadByte();
			this.ModificationTime = binaryReader.ReadUInt32();
			this.ExtraFlags = binaryReader.ReadByte();
			this.OperationSystem = binaryReader.ReadByte();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000180F3 File Offset: 0x000170F3
		public static int SizeOf()
		{
			return 10;
		}

		// Token: 0x040001C3 RID: 451
		public byte Id1;

		// Token: 0x040001C4 RID: 452
		public byte Id2;

		// Token: 0x040001C5 RID: 453
		public byte CompressionMethod;

		// Token: 0x040001C6 RID: 454
		public byte Flag;

		// Token: 0x040001C7 RID: 455
		public uint ModificationTime;

		// Token: 0x040001C8 RID: 456
		public byte ExtraFlags;

		// Token: 0x040001C9 RID: 457
		public byte OperationSystem;
	}
}
