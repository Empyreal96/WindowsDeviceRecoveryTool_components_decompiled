using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A2 RID: 162
	internal struct Zip64CentralDirEnd
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0002C148 File Offset: 0x0002B148
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0002C150 File Offset: 0x0002B150
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

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002C159 File Offset: 0x0002B159
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0002C161 File Offset: 0x0002B161
		public long CentralDirEndSize
		{
			get
			{
				return this._centralDirEndSize;
			}
			set
			{
				this._centralDirEndSize = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002C16A File Offset: 0x0002B16A
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0002C172 File Offset: 0x0002B172
		public ushort VersionMadeBy
		{
			get
			{
				return this._versionMadeBy;
			}
			set
			{
				this._versionMadeBy = value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002C17B File Offset: 0x0002B17B
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0002C183 File Offset: 0x0002B183
		public ushort VersionNeededToExtract
		{
			get
			{
				return this._versionNeededToExtract;
			}
			set
			{
				this._versionNeededToExtract = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002C18C File Offset: 0x0002B18C
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0002C194 File Offset: 0x0002B194
		public uint DiskNumber
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

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002C19D File Offset: 0x0002B19D
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0002C1A5 File Offset: 0x0002B1A5
		public uint StartDiskNumber
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

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002C1AE File Offset: 0x0002B1AE
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0002C1B6 File Offset: 0x0002B1B6
		public long EntriesOnDisk
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0002C1BF File Offset: 0x0002B1BF
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0002C1C7 File Offset: 0x0002B1C7
		public long EntriesCentralDir
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0002C1D0 File Offset: 0x0002B1D0
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0002C1D8 File Offset: 0x0002B1D8
		public long CentralDirSize
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

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0002C1E1 File Offset: 0x0002B1E1
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0002C1E9 File Offset: 0x0002B1E9
		public long OffsetStartDir
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

		// Token: 0x0600074E RID: 1870 RVA: 0x0002C1F4 File Offset: 0x0002B1F4
		public void Reset()
		{
			this._signature = 0U;
			this._centralDirEndSize = 0L;
			this._versionMadeBy = 0;
			this._versionNeededToExtract = 0;
			this._diskNumber = 0U;
			this._startDiskNumber = 0U;
			this._entriesOnDisk = 0L;
			this._entriesCentralDir = 0L;
			this._centralDirSize = 0L;
			this._offsetStartDir = 0L;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002C24C File Offset: 0x0002B24C
		public static int SizeOf()
		{
			return 56;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002C250 File Offset: 0x0002B250
		public byte[] GetBytes()
		{
			byte[] array = new byte[Zip64CentralDirEnd.SizeOf()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this._signature);
			binaryWriter.Write(this._centralDirEndSize);
			binaryWriter.Write(this._versionMadeBy);
			binaryWriter.Write(this._versionNeededToExtract);
			binaryWriter.Write(this._diskNumber);
			binaryWriter.Write(this._startDiskNumber);
			binaryWriter.Write(this._entriesOnDisk);
			binaryWriter.Write(this._entriesCentralDir);
			binaryWriter.Write(this._centralDirSize);
			binaryWriter.Write(this._offsetStartDir);
			return array;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002C2F0 File Offset: 0x0002B2F0
		public void LoadFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this._signature = binaryReader.ReadUInt32();
			this._centralDirEndSize = binaryReader.ReadInt64();
			this._versionMadeBy = binaryReader.ReadUInt16();
			this._versionNeededToExtract = binaryReader.ReadUInt16();
			this._diskNumber = binaryReader.ReadUInt32();
			this._startDiskNumber = binaryReader.ReadUInt32();
			this._entriesOnDisk = binaryReader.ReadInt64();
			this._entriesCentralDir = binaryReader.ReadInt64();
			this._centralDirSize = binaryReader.ReadInt64();
			this._offsetStartDir = binaryReader.ReadInt64();
		}

		// Token: 0x040003ED RID: 1005
		private uint _signature;

		// Token: 0x040003EE RID: 1006
		private long _centralDirEndSize;

		// Token: 0x040003EF RID: 1007
		private ushort _versionMadeBy;

		// Token: 0x040003F0 RID: 1008
		private ushort _versionNeededToExtract;

		// Token: 0x040003F1 RID: 1009
		private uint _diskNumber;

		// Token: 0x040003F2 RID: 1010
		private uint _startDiskNumber;

		// Token: 0x040003F3 RID: 1011
		private long _entriesOnDisk;

		// Token: 0x040003F4 RID: 1012
		private long _entriesCentralDir;

		// Token: 0x040003F5 RID: 1013
		private long _centralDirSize;

		// Token: 0x040003F6 RID: 1014
		private long _offsetStartDir;
	}
}
