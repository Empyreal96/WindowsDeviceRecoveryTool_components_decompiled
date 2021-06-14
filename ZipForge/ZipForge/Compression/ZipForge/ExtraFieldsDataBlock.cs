using System;
using System.Collections;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x0200009D RID: 157
	internal class ExtraFieldsDataBlock : ICloneable
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002B8C5 File Offset: 0x0002A8C5
		public int Count
		{
			get
			{
				return this._extraFieldsArray.Count;
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002B8D4 File Offset: 0x0002A8D4
		public ushort GetBytesLength(ExtraFieldsTarget target)
		{
			ushort num = 0;
			for (int i = 0; i < this.Count; i++)
			{
				num += (ushort)this[i].GetExtraFieldSize(target);
			}
			return num;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002B908 File Offset: 0x0002A908
		public void WriteToStream(Stream source, long offset, ExtraFieldsTarget target)
		{
			if (offset != 0L)
			{
				source.Seek(offset, SeekOrigin.Current);
			}
			foreach (object obj in this._extraFieldsArray)
			{
				ExtraFieldData extraFieldData = (ExtraFieldData)obj;
				extraFieldData.WriteToStream(source, source.CanSeek ? source.Position : 0L, target);
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002B984 File Offset: 0x0002A984
		public byte[] GetBytes(ExtraFieldsTarget target)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in this._extraFieldsArray)
			{
				ExtraFieldData extraFieldData = (ExtraFieldData)obj;
				extraFieldData.WriteToStream(memoryStream, memoryStream.Position, target);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x17000132 RID: 306
		public ExtraFieldData this[int index]
		{
			get
			{
				return (ExtraFieldData)this._extraFieldsArray[index];
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002BA0C File Offset: 0x0002AA0C
		public ExtraFieldData GetExtraFieldById(ushort headerId)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].GetExtraFieldId() == headerId)
				{
					return this[i];
				}
			}
			return null;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002BA44 File Offset: 0x0002AA44
		public void AddExtraField(ExtraFieldData extraField, DirItem item)
		{
			if (extraField.GetExtraFieldId() == 1)
			{
				this._zip64ExtraField = (Zip64ExtraFieldData)extraField;
				if (item != null)
				{
					item.IsHugeFile = true;
				}
			}
			if (extraField.GetExtraFieldId() == 21838 && item != null)
			{
				item.Name = ((UnicodeExtraFieldData)extraField).FileName;
			}
			if (this.GetExtraFieldById(extraField.GetExtraFieldId()) != null)
			{
				this.RemoveExtraFieldById(extraField.GetExtraFieldId());
			}
			this._extraFieldsArray.Add(extraField);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002BAB8 File Offset: 0x0002AAB8
		public void RemoveExtraField(ExtraFieldData extraField)
		{
			this._extraFieldsArray.Remove(extraField);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002BAC6 File Offset: 0x0002AAC6
		public void RemoveExtraFieldById(ushort extraFieldId)
		{
			this.RemoveExtraField(this.GetExtraFieldById(extraFieldId));
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002BAD5 File Offset: 0x0002AAD5
		public ExtraFieldData[] GetExtraFieldsArray()
		{
			return (ExtraFieldData[])this._extraFieldsArray.ToArray(typeof(ExtraFieldData));
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0002BAF1 File Offset: 0x0002AAF1
		public Zip64ExtraFieldData Zip64ExtraField
		{
			get
			{
				return this._zip64ExtraField;
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0002BAFC File Offset: 0x0002AAFC
		public object Clone()
		{
			ExtraFieldsDataBlock extraFieldsDataBlock = new ExtraFieldsDataBlock();
			foreach (object obj in this._extraFieldsArray)
			{
				ExtraFieldData extraFieldData = (ExtraFieldData)obj;
				extraFieldsDataBlock.AddExtraField((ExtraFieldData)extraFieldData.Clone(), null);
			}
			return extraFieldsDataBlock;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002BB6C File Offset: 0x0002AB6C
		public void Reset()
		{
			this._extraFieldsArray.Clear();
			this._zip64ExtraField = null;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0002BB80 File Offset: 0x0002AB80
		public static ExtraFieldsDataBlock LoadFromStream(Stream source, ushort extraFieldsLength, DirItem item)
		{
			ExtraFieldsDataBlock extraFieldsDataBlock = new ExtraFieldsDataBlock();
			long num = (long)((ulong)extraFieldsLength);
			while (num > 0L)
			{
				long position = source.Position;
				byte[] array = new byte[2];
				source.Read(array, 0, 2);
				ushort num2 = BitConverter.ToUInt16(array, 0);
				ushort num3 = num2;
				if (num3 <= 10)
				{
					if (num3 != 1)
					{
						if (num3 != 10)
						{
							goto IL_AA;
						}
						NTFSExtraFieldData extraField = NTFSExtraFieldData.LoadFromStream(source, item);
						extraFieldsDataBlock.AddExtraField(extraField, item);
					}
					else
					{
						Zip64ExtraFieldData extraField2 = Zip64ExtraFieldData.LoadFromStream(source, item);
						extraFieldsDataBlock.AddExtraField(extraField2, item);
					}
				}
				else if (num3 != 21838)
				{
					if (num3 != 39169)
					{
						goto IL_AA;
					}
					AESExtraFieldData extraField3 = AESExtraFieldData.LoadFromStream(source, item);
					extraFieldsDataBlock.AddExtraField(extraField3, item);
				}
				else
				{
					UnicodeExtraFieldData extraField4 = UnicodeExtraFieldData.LoadFromStream(source, item);
					extraFieldsDataBlock.AddExtraField(extraField4, item);
				}
				IL_BA:
				num -= source.Position - position;
				continue;
				IL_AA:
				extraFieldsDataBlock.AddExtraField(ExtraFieldData.LoadFromStream(num2, source, item), item);
				goto IL_BA;
			}
			return extraFieldsDataBlock;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0002BC5C File Offset: 0x0002AC5C
		public void LoadLocalHeaderExtraFieldsFromStream(Stream source, ushort extraFieldsLength, DirItem item)
		{
			long num = (long)((ulong)extraFieldsLength);
			BinaryReader binaryReader = new BinaryReader(source);
			ArrayList arrayList = new ArrayList();
			while (num > 0L)
			{
				long position = source.Position;
				ushort num2 = binaryReader.ReadUInt16();
				arrayList.Add(num2);
				ExtraFieldData extraFieldById = this.GetExtraFieldById(num2);
				if (extraFieldById != null)
				{
					ushort num3 = binaryReader.ReadUInt16();
					if (extraFieldById.GetExtraFieldSize(ExtraFieldsTarget.LocalHeaderExtraFields) != (int)(num3 + 4))
					{
						extraFieldById.SetLocalHeaderExtraFieldSize((int)(num3 + 4));
					}
					source.Seek((long)((ulong)num3), SeekOrigin.Current);
				}
				else
				{
					ushort num4 = num2;
					if (num4 != 1)
					{
						if (num4 != 21838)
						{
							if (num4 != 39169)
							{
								this.AddExtraField(ExtraFieldData.LoadFromStream(num2, source, item), item);
							}
							else
							{
								AESExtraFieldData extraField = AESExtraFieldData.LoadFromStream(source, item);
								this.AddExtraField(extraField, item);
							}
						}
						else
						{
							UnicodeExtraFieldData extraField2 = UnicodeExtraFieldData.LoadFromStream(source, item);
							this.AddExtraField(extraField2, item);
						}
					}
					else
					{
						Zip64ExtraFieldData extraField3 = Zip64ExtraFieldData.LoadFromStream(source, item);
						this.AddExtraField(extraField3, item);
					}
				}
				num -= source.Position - position;
			}
			if (num == 0L)
			{
				using (IEnumerator enumerator = this._extraFieldsArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						ExtraFieldData extraFieldData = (ExtraFieldData)obj;
						if (!arrayList.Contains(extraFieldData.GetExtraFieldId()))
						{
							extraFieldData.SetLocalHeaderExtraFieldSize(0);
						}
					}
					return;
				}
			}
			item.InformOnLocalHeaderExtraFieldCorruption((int)extraFieldsLength);
		}

		// Token: 0x040003CC RID: 972
		private readonly ArrayList _extraFieldsArray = new ArrayList();

		// Token: 0x040003CD RID: 973
		private Zip64ExtraFieldData _zip64ExtraField;
	}
}
