using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000093 RID: 147
	internal class DirItemDataDescriptor
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x0002A9ED File Offset: 0x000299ED
		public DirItemDataDescriptor(uint crc32, int compressedSize, int uncompressedSize)
		{
			this._isZip64 = false;
			this._crc32 = crc32;
			this._compressedSize = (long)compressedSize;
			this._uncompressedSize = (long)uncompressedSize;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0002AA1A File Offset: 0x00029A1A
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0002AA22 File Offset: 0x00029A22
		public bool IsZip64
		{
			get
			{
				return this._isZip64;
			}
			set
			{
				this._isZip64 = value;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002AA2B File Offset: 0x00029A2B
		public DirItemDataDescriptor(uint crc32, long compressedSize, long uncompressedSize)
		{
			this._isZip64 = true;
			this._crc32 = crc32;
			this._compressedSize = compressedSize;
			this._uncompressedSize = uncompressedSize;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002AA56 File Offset: 0x00029A56
		public DirItemDataDescriptor(uint crc32, long compressedSize, long uncompressedSize, bool isZip64, bool includeSignature)
		{
			this._wasSignature = includeSignature;
			this._isZip64 = isZip64;
			this._crc32 = crc32;
			this._compressedSize = compressedSize;
			this._uncompressedSize = uncompressedSize;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002AA8C File Offset: 0x00029A8C
		public static DirItemDataDescriptor LoadFromStream(Stream stream, long offset, bool isZip64)
		{
			if (offset != 0L)
			{
				stream.Seek(offset, SeekOrigin.Current);
			}
			bool includeSignature = true;
			BinaryReader binaryReader = new BinaryReader(stream);
			int num = binaryReader.ReadInt32();
			if (num == 134695760)
			{
				num = binaryReader.ReadInt32();
			}
			else
			{
				includeSignature = false;
			}
			uint crc = (uint)num;
			long compressedSize;
			long uncompressedSize;
			if (isZip64)
			{
				compressedSize = binaryReader.ReadInt64();
				uncompressedSize = binaryReader.ReadInt64();
			}
			else
			{
				compressedSize = (long)binaryReader.ReadInt32();
				uncompressedSize = (long)binaryReader.ReadInt32();
			}
			return new DirItemDataDescriptor(crc, compressedSize, uncompressedSize, isZip64, includeSignature);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0002AAFE File Offset: 0x00029AFE
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0002AB06 File Offset: 0x00029B06
		public uint CRC32
		{
			get
			{
				return this._crc32;
			}
			set
			{
				this._crc32 = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0002AB0F File Offset: 0x00029B0F
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0002AB17 File Offset: 0x00029B17
		public long CompressedSize
		{
			get
			{
				return this._compressedSize;
			}
			set
			{
				if (value > 2147483647L)
				{
					this._isZip64 = true;
				}
				this._compressedSize = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0002AB30 File Offset: 0x00029B30
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0002AB38 File Offset: 0x00029B38
		public long UncompressedSize
		{
			get
			{
				return this._uncompressedSize;
			}
			set
			{
				if (value > 2147483647L)
				{
					this._isZip64 = true;
				}
				this._uncompressedSize = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0002AB51 File Offset: 0x00029B51
		public long Signature
		{
			get
			{
				return 134695760L;
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002AB5C File Offset: 0x00029B5C
		public void WriteToStream(Stream stream, long offset)
		{
			if (offset != 0L)
			{
				stream.Seek(offset, SeekOrigin.Current);
			}
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			if (this._wasSignature)
			{
				binaryWriter.Write(134695760);
			}
			binaryWriter.Write(this._crc32);
			if (this._isZip64)
			{
				binaryWriter.Write(this._compressedSize);
				binaryWriter.Write(this._uncompressedSize);
				return;
			}
			binaryWriter.Write((int)this._compressedSize);
			binaryWriter.Write((int)this._uncompressedSize);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002ABD8 File Offset: 0x00029BD8
		public int GetSize()
		{
			int num = 4;
			if (this._wasSignature)
			{
				num += 4;
			}
			if (this._isZip64)
			{
				num += 16;
			}
			else
			{
				num += 8;
			}
			return num;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002AC07 File Offset: 0x00029C07
		public object Clone()
		{
			return new DirItemDataDescriptor(this._crc32, this._compressedSize, this._uncompressedSize);
		}

		// Token: 0x04000397 RID: 919
		private const int signature = 134695760;

		// Token: 0x04000398 RID: 920
		private uint _crc32;

		// Token: 0x04000399 RID: 921
		private long _compressedSize;

		// Token: 0x0400039A RID: 922
		private long _uncompressedSize;

		// Token: 0x0400039B RID: 923
		private bool _isZip64;

		// Token: 0x0400039C RID: 924
		private readonly bool _wasSignature = true;
	}
}
