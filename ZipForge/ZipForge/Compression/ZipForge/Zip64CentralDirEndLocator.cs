using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A5 RID: 165
	internal struct Zip64CentralDirEndLocator
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002C55F File Offset: 0x0002B55F
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002C567 File Offset: 0x0002B567
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

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002C570 File Offset: 0x0002B570
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0002C578 File Offset: 0x0002B578
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002C581 File Offset: 0x0002B581
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002C589 File Offset: 0x0002B589
		public long OffsetStartDirEnd
		{
			get
			{
				return this._offsetStartDirEnd;
			}
			set
			{
				this._offsetStartDirEnd = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002C592 File Offset: 0x0002B592
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0002C59A File Offset: 0x0002B59A
		public uint TotalNumberOfDisks
		{
			get
			{
				return this._totalNumberOfDisks;
			}
			set
			{
				this._totalNumberOfDisks = value;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002C5A3 File Offset: 0x0002B5A3
		public void Reset()
		{
			this._signature = 0U;
			this._startDiskNumber = 0U;
			this._offsetStartDirEnd = 0L;
			this._totalNumberOfDisks = 0U;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002C5C2 File Offset: 0x0002B5C2
		public static int SizeOf()
		{
			return 20;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002C5C8 File Offset: 0x0002B5C8
		public void LoadFromByteArray(byte[] source, uint offset)
		{
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(source)
			{
				Position = (long)((ulong)offset)
			});
			this._signature = binaryReader.ReadUInt32();
			this._startDiskNumber = binaryReader.ReadUInt32();
			this._offsetStartDirEnd = binaryReader.ReadInt64();
			this._totalNumberOfDisks = binaryReader.ReadUInt32();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002C61C File Offset: 0x0002B61C
		public byte[] GetBytes()
		{
			byte[] array = new byte[Zip64CentralDirEndLocator.SizeOf()];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this._signature);
			binaryWriter.Write(this._startDiskNumber);
			binaryWriter.Write(this._offsetStartDirEnd);
			binaryWriter.Write(this._totalNumberOfDisks);
			return array;
		}

		// Token: 0x04000403 RID: 1027
		private uint _signature;

		// Token: 0x04000404 RID: 1028
		private uint _startDiskNumber;

		// Token: 0x04000405 RID: 1029
		private long _offsetStartDirEnd;

		// Token: 0x04000406 RID: 1030
		private uint _totalNumberOfDisks;
	}
}
