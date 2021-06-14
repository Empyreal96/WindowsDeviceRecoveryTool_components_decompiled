using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A4 RID: 164
	internal struct ZipCentralDirEnd
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x0002C38B File Offset: 0x0002B38B
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x0002C393 File Offset: 0x0002B393
		public uint Signature
		{
			get
			{
				return this._signature;
			}
			set
			{
				this._signature = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000754 RID: 1876 RVA: 0x0002C39C File Offset: 0x0002B39C
		// (set) Token: 0x06000755 RID: 1877 RVA: 0x0002C3A4 File Offset: 0x0002B3A4
		public ushort DiskNumber
		{
			get
			{
				return this._diskNumber;
			}
			set
			{
				this._diskNumber = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0002C3AD File Offset: 0x0002B3AD
		// (set) Token: 0x06000757 RID: 1879 RVA: 0x0002C3B5 File Offset: 0x0002B3B5
		public ushort StartDiskNumber
		{
			get
			{
				return this._startDiskNumber;
			}
			set
			{
				this._startDiskNumber = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002C3BE File Offset: 0x0002B3BE
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0002C3C6 File Offset: 0x0002B3C6
		public ushort EntriesOnDisk
		{
			get
			{
				return this._entriesOnDisk;
			}
			set
			{
				this._entriesOnDisk = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002C3CF File Offset: 0x0002B3CF
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0002C3D7 File Offset: 0x0002B3D7
		public ushort EntriesCentralDir
		{
			get
			{
				return this._entriesCentralDir;
			}
			set
			{
				this._entriesCentralDir = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0002C3E0 File Offset: 0x0002B3E0
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0002C3E8 File Offset: 0x0002B3E8
		public uint CentralDirSize
		{
			get
			{
				return this._centralDirSize;
			}
			set
			{
				this._centralDirSize = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002C3F1 File Offset: 0x0002B3F1
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002C3F9 File Offset: 0x0002B3F9
		public uint OffsetStartDir
		{
			get
			{
				return this._offsetStartDir;
			}
			set
			{
				this._offsetStartDir = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002C402 File Offset: 0x0002B402
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0002C40A File Offset: 0x0002B40A
		public ushort CommentLength
		{
			get
			{
				return this._commentLength;
			}
			set
			{
				this._commentLength = value;
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002C413 File Offset: 0x0002B413
		public void Reset()
		{
			this._signature = 0U;
			this._diskNumber = 0;
			this._startDiskNumber = 0;
			this._entriesOnDisk = 0;
			this._entriesCentralDir = 0;
			this._centralDirSize = 0U;
			this._offsetStartDir = 0U;
			this._commentLength = 0;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002C44D File Offset: 0x0002B44D
		public static int SizeOf()
		{
			return 22;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0002C454 File Offset: 0x0002B454
		public byte[] GetBytes()
		{
			byte[] array = new byte[ZipCentralDirEnd.SizeOf()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this._signature);
			binaryWriter.Write(this._diskNumber);
			binaryWriter.Write(this._startDiskNumber);
			binaryWriter.Write(this._entriesOnDisk);
			binaryWriter.Write(this._entriesCentralDir);
			binaryWriter.Write(this._centralDirSize);
			binaryWriter.Write(this._offsetStartDir);
			binaryWriter.Write(this._commentLength);
			return array;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002C4DC File Offset: 0x0002B4DC
		public void LoadFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this._signature = binaryReader.ReadUInt32();
			this._diskNumber = binaryReader.ReadUInt16();
			this._startDiskNumber = binaryReader.ReadUInt16();
			this._entriesOnDisk = binaryReader.ReadUInt16();
			this._entriesCentralDir = binaryReader.ReadUInt16();
			this._centralDirSize = binaryReader.ReadUInt32();
			this._offsetStartDir = binaryReader.ReadUInt32();
			this._commentLength = binaryReader.ReadUInt16();
		}

		// Token: 0x040003FB RID: 1019
		private uint _signature;

		// Token: 0x040003FC RID: 1020
		private ushort _diskNumber;

		// Token: 0x040003FD RID: 1021
		private ushort _startDiskNumber;

		// Token: 0x040003FE RID: 1022
		private ushort _entriesOnDisk;

		// Token: 0x040003FF RID: 1023
		private ushort _entriesCentralDir;

		// Token: 0x04000400 RID: 1024
		private uint _centralDirSize;

		// Token: 0x04000401 RID: 1025
		private uint _offsetStartDir;

		// Token: 0x04000402 RID: 1026
		private ushort _commentLength;
	}
}
