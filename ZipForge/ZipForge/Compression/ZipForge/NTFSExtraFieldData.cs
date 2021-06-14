using System;
using System.Collections;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x0200009B RID: 155
	internal class NTFSExtraFieldData : ExtraFieldData
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0002B531 File Offset: 0x0002A531
		public NTFSExtraFieldData() : this(0L, 0L, 0L)
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002B540 File Offset: 0x0002A540
		public NTFSExtraFieldData(string fileName)
		{
			this.HeaderId = 10;
			FileInfo fileInfo = new FileInfo(fileName);
			this._fileModificationTime = fileInfo.LastWriteTime.ToFileTime();
			this._fileLastAccessTime = fileInfo.LastAccessTime.ToFileTime();
			this._fileCreationTime = fileInfo.CreationTime.ToFileTime();
			this._attributes = new NTFSExtraFieldData.NTFSAttributeTag[0];
			this.UpdateBaseClassFields();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002B5B0 File Offset: 0x0002A5B0
		public NTFSExtraFieldData(long fileModificationTime, long fileLastAccessTime, long fileCreationTime)
		{
			this.HeaderId = 10;
			this._fileModificationTime = fileModificationTime;
			this._fileLastAccessTime = fileLastAccessTime;
			this._fileCreationTime = fileCreationTime;
			this._attributes = new NTFSExtraFieldData.NTFSAttributeTag[0];
			this.UpdateBaseClassFields();
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002B5E8 File Offset: 0x0002A5E8
		public static NTFSExtraFieldData LoadFromStream(Stream source, DirItem item)
		{
			BinaryReader binaryReader = new BinaryReader(source);
			ushort num = binaryReader.ReadUInt16();
			ushort num2 = 0;
			binaryReader.ReadInt32();
			num2 += 4;
			long fileModificationTime = 0L;
			long fileLastAccessTime = 0L;
			long fileCreationTime = 0L;
			ArrayList arrayList = new ArrayList();
			while (num2 < num)
			{
				ushort num3 = binaryReader.ReadUInt16();
				num2 += 2;
				ushort num4 = binaryReader.ReadUInt16();
				num2 += 2;
				if (num3 == 1)
				{
					fileModificationTime = binaryReader.ReadInt64();
					num2 += 8;
					fileLastAccessTime = binaryReader.ReadInt64();
					num2 += 8;
					fileCreationTime = binaryReader.ReadInt64();
					num2 += 8;
				}
				else
				{
					NTFSExtraFieldData.NTFSAttributeTag ntfsattributeTag = default(NTFSExtraFieldData.NTFSAttributeTag);
					ntfsattributeTag.AttrTag = num3;
					ntfsattributeTag.AttrSize = num4;
					ntfsattributeTag.AttributeData = new byte[(int)num4];
					binaryReader.Read(ntfsattributeTag.AttributeData, 0, (int)num4);
					num2 += num4;
					arrayList.Add(ntfsattributeTag);
				}
			}
			return new NTFSExtraFieldData(fileModificationTime, fileLastAccessTime, fileCreationTime)
			{
				Tags = (NTFSExtraFieldData.NTFSAttributeTag[])arrayList.ToArray(typeof(NTFSExtraFieldData.NTFSAttributeTag))
			};
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002B6F0 File Offset: 0x0002A6F0
		public override object Clone()
		{
			return new NTFSExtraFieldData(this._fileModificationTime, this._fileLastAccessTime, this._fileCreationTime)
			{
				Tags = (NTFSExtraFieldData.NTFSAttributeTag[])this._attributes.Clone(),
				LocalHeaderExtraFieldsLength = this.LocalHeaderExtraFieldsLength
			};
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0002B738 File Offset: 0x0002A738
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0002B740 File Offset: 0x0002A740
		public long FileModificationTime
		{
			get
			{
				return this._fileModificationTime;
			}
			set
			{
				this._fileModificationTime = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0002B74F File Offset: 0x0002A74F
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0002B757 File Offset: 0x0002A757
		public long FileLastAccessTime
		{
			get
			{
				return this._fileLastAccessTime;
			}
			set
			{
				this._fileLastAccessTime = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0002B766 File Offset: 0x0002A766
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x0002B76E File Offset: 0x0002A76E
		public long FileCreationTime
		{
			get
			{
				return this._fileCreationTime;
			}
			set
			{
				this._fileCreationTime = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0002B77D File Offset: 0x0002A77D
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0002B785 File Offset: 0x0002A785
		protected NTFSExtraFieldData.NTFSAttributeTag[] Tags
		{
			get
			{
				return this._attributes;
			}
			set
			{
				this._attributes = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002B794 File Offset: 0x0002A794
		private ushort GetFieldDataBytesLength()
		{
			ushort num = 4;
			if (this._fileModificationTime != 0L)
			{
				num += 28;
			}
			if (this._attributes != null)
			{
				foreach (NTFSExtraFieldData.NTFSAttributeTag ntfsattributeTag in this._attributes)
				{
					num += 4 + ntfsattributeTag.AttrSize;
				}
			}
			return num;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002B7EC File Offset: 0x0002A7EC
		private byte[] GetFieldDataBytes()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			int value = 0;
			binaryWriter.Write(value);
			if (this._fileModificationTime != 0L)
			{
				binaryWriter.Write(1);
				binaryWriter.Write(24);
				binaryWriter.Write(this._fileModificationTime);
				binaryWriter.Write(this._fileLastAccessTime);
				binaryWriter.Write(this._fileCreationTime);
			}
			if (this._attributes != null)
			{
				foreach (NTFSExtraFieldData.NTFSAttributeTag ntfsattributeTag in this._attributes)
				{
					binaryWriter.Write(ntfsattributeTag.AttrTag);
					binaryWriter.Write(ntfsattributeTag.AttrSize);
					binaryWriter.Write(ntfsattributeTag.AttributeData);
				}
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002B8AB File Offset: 0x0002A8AB
		private void UpdateBaseClassFields()
		{
			this.DataSize = this.GetFieldDataBytesLength();
			this.Data = this.GetFieldDataBytes();
		}

		// Token: 0x040003C3 RID: 963
		private const ushort FieldId = 10;

		// Token: 0x040003C4 RID: 964
		private const ushort firstTag = 1;

		// Token: 0x040003C5 RID: 965
		private long _fileModificationTime;

		// Token: 0x040003C6 RID: 966
		private long _fileLastAccessTime;

		// Token: 0x040003C7 RID: 967
		private long _fileCreationTime;

		// Token: 0x040003C8 RID: 968
		private NTFSExtraFieldData.NTFSAttributeTag[] _attributes;

		// Token: 0x0200009C RID: 156
		protected struct NTFSAttributeTag
		{
			// Token: 0x040003C9 RID: 969
			public ushort AttrTag;

			// Token: 0x040003CA RID: 970
			public ushort AttrSize;

			// Token: 0x040003CB RID: 971
			public byte[] AttributeData;
		}
	}
}
