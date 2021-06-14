using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000099 RID: 153
	internal class Zip64ExtraFieldData : ExtraFieldData
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x0002AF77 File Offset: 0x00029F77
		public Zip64ExtraFieldData(long uncompSize, long compSize, long localHeaderOffset, uint diskNumberStart)
		{
			this.HeaderId = 1;
			this._compSize = compSize;
			this._uncompSize = uncompSize;
			this._relOffsetLh = localHeaderOffset;
			this._diskNumberStart = diskNumberStart;
			this.UpdateBaseClassFields();
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002AFA9 File Offset: 0x00029FA9
		public Zip64ExtraFieldData() : this(0L, 0L, 0L, 0U)
		{
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002AFB8 File Offset: 0x00029FB8
		public static Zip64ExtraFieldData LoadFromStream(Stream source, DirItem item)
		{
			BinaryReader binaryReader = new BinaryReader(source);
			ushort num = binaryReader.ReadUInt16();
			ushort num2 = 0;
			long uncompSize = -1L;
			long compSize = -1L;
			long localHeaderOffset = -1L;
			uint diskNumberStart = uint.MaxValue;
			if (item.UncompressedSize == (long)((ulong)-1))
			{
				uncompSize = binaryReader.ReadInt64();
				num2 += 8;
			}
			if (item.CompressedSize == (long)((ulong)-1))
			{
				compSize = binaryReader.ReadInt64();
				num2 += 8;
			}
			if (item.RelativeLocalHeaderOffset == (long)((ulong)-1))
			{
				localHeaderOffset = binaryReader.ReadInt64();
				num2 += 8;
			}
			if (num2 < num)
			{
				diskNumberStart = binaryReader.ReadUInt32();
			}
			return new Zip64ExtraFieldData(uncompSize, compSize, localHeaderOffset, diskNumberStart);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002B03F File Offset: 0x0002A03F
		public override int GetExtraFieldSize(ExtraFieldsTarget target)
		{
			if (target == ExtraFieldsTarget.LocalHeaderExtraFields && this._compSize == -1L && this._uncompSize == -1L)
			{
				return 0;
			}
			return base.GetExtraFieldSize(target);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002B064 File Offset: 0x0002A064
		public override byte[] GetBytes(ExtraFieldsTarget target)
		{
			if (target == ExtraFieldsTarget.CentralDirectoryExtraFields)
			{
				return base.GetBytes(target);
			}
			byte[] array = new byte[this.GetExtraFieldSize(target)];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.HeaderId);
			binaryWriter.Write(this.GetDataSize(target));
			binaryWriter.Write(this.GetData(target));
			return array;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002B0BD File Offset: 0x0002A0BD
		public override byte[] GetData(ExtraFieldsTarget target)
		{
			return this.GetFieldData(target);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002B0C6 File Offset: 0x0002A0C6
		public override ushort GetDataSize(ExtraFieldsTarget target)
		{
			return this.GetFieldDataSize(target);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002B0D0 File Offset: 0x0002A0D0
		public override void WriteToStream(Stream stream, long offset, ExtraFieldsTarget target)
		{
			if (target == ExtraFieldsTarget.CentralDirectoryExtraFields)
			{
				base.WriteToStream(stream, offset, target);
				return;
			}
			if (this.GetFieldDataSize(target) >= 0)
			{
				if (offset != 0L)
				{
					stream.Position = offset;
					BinaryWriter binaryWriter = new BinaryWriter(stream);
					binaryWriter.Write(this.HeaderId);
					binaryWriter.Write(this.GetFieldDataSize(target));
					binaryWriter.Write(this.GetFieldData(target));
					return;
				}
				if (target == ExtraFieldsTarget.LocalHeaderExtraFields)
				{
					BinaryWriter binaryWriter2 = new BinaryWriter(stream);
					binaryWriter2.Write(this.HeaderId);
					binaryWriter2.Write(this.GetFieldDataSize(target));
					binaryWriter2.Write(this.GetFieldData(target));
				}
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002B160 File Offset: 0x0002A160
		public override object Clone()
		{
			return new Zip64ExtraFieldData(this._uncompSize, this._compSize, this._relOffsetLh, this._diskNumberStart)
			{
				LocalHeaderExtraFieldsLength = this.LocalHeaderExtraFieldsLength
			};
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002B198 File Offset: 0x0002A198
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0002B1A0 File Offset: 0x0002A1A0
		public long UncompressedFileSize
		{
			get
			{
				return this._uncompSize;
			}
			set
			{
				this._uncompSize = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0002B1AF File Offset: 0x0002A1AF
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0002B1B7 File Offset: 0x0002A1B7
		public long CompressedFileSize
		{
			get
			{
				return this._compSize;
			}
			set
			{
				this._compSize = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002B1C6 File Offset: 0x0002A1C6
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0002B1CE File Offset: 0x0002A1CE
		public long LocalHeaderOffset
		{
			get
			{
				return this._relOffsetLh;
			}
			set
			{
				this._relOffsetLh = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002B1DD File Offset: 0x0002A1DD
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0002B1E5 File Offset: 0x0002A1E5
		public uint StartDiskNumber
		{
			get
			{
				return this._diskNumberStart;
			}
			set
			{
				this._diskNumberStart = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002B1F4 File Offset: 0x0002A1F4
		private void UpdateBaseClassFields()
		{
			this.DataSize = this.GetFieldDataSize(ExtraFieldsTarget.CentralDirectoryExtraFields);
			this.Data = this.GetFieldData(ExtraFieldsTarget.CentralDirectoryExtraFields);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002B210 File Offset: 0x0002A210
		private ushort GetFieldDataSize(ExtraFieldsTarget target)
		{
			if (target != ExtraFieldsTarget.LocalHeaderExtraFields)
			{
				ushort num = 0;
				if (this._uncompSize > -1L)
				{
					num += 8;
				}
				if (this._compSize > -1L)
				{
					num += 8;
				}
				if (this._relOffsetLh > -1L)
				{
					num += 8;
				}
				if (this._diskNumberStart < 4294967295U)
				{
					num += 4;
				}
				return num;
			}
			if (this._uncompSize > -1L || this._compSize > -1L)
			{
				return 16;
			}
			return 0;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002B278 File Offset: 0x0002A278
		private byte[] GetFieldData(ExtraFieldsTarget target)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			if (target == ExtraFieldsTarget.LocalHeaderExtraFields)
			{
				if (this._uncompSize >= 0L || this._compSize >= 0L)
				{
					binaryWriter.Write(this._uncompSize);
					binaryWriter.Write(this._compSize);
				}
			}
			else
			{
				if (this._uncompSize > -1L)
				{
					binaryWriter.Write(this._uncompSize);
				}
				if (this._compSize > -1L)
				{
					binaryWriter.Write(this._compSize);
				}
				if (this._relOffsetLh > -1L)
				{
					binaryWriter.Write(this._relOffsetLh);
				}
				if (this._diskNumberStart < 4294967295U)
				{
					binaryWriter.Write(this._diskNumberStart);
				}
			}
			return memoryStream.ToArray();
		}

		// Token: 0x040003B9 RID: 953
		private const ushort FieldId = 1;

		// Token: 0x040003BA RID: 954
		private long _uncompSize;

		// Token: 0x040003BB RID: 955
		private long _compSize;

		// Token: 0x040003BC RID: 956
		private long _relOffsetLh;

		// Token: 0x040003BD RID: 957
		private uint _diskNumberStart;
	}
}
