using System;
using System.IO;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000097 RID: 151
	internal class ExtraFieldData : ICloneable
	{
		// Token: 0x060006D1 RID: 1745 RVA: 0x0002AC62 File Offset: 0x00029C62
		public ExtraFieldData()
		{
			this.LocalHeaderExtraFieldsLength = -1;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002AC71 File Offset: 0x00029C71
		public ExtraFieldData(ushort id, ushort dataSize, byte[] data)
		{
			if (data.Length != (int)dataSize)
			{
				throw new ArgumentException("The actual size of data and the dataSize parameter are different");
			}
			this.HeaderId = id;
			this.DataSize = dataSize;
			this.Data = (byte[])data.Clone();
			this.LocalHeaderExtraFieldsLength = -1;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002ACB0 File Offset: 0x00029CB0
		public static ExtraFieldData LoadFromStream(ushort extraFieldId, Stream source, DirItem item)
		{
			BinaryReader binaryReader = new BinaryReader(source);
			ushort num = binaryReader.ReadUInt16();
			byte[] data = binaryReader.ReadBytes((int)num);
			return new ExtraFieldData(extraFieldId, num, data);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002ACDB File Offset: 0x00029CDB
		public void SetLocalHeaderExtraFieldSize(int size)
		{
			this.LocalHeaderExtraFieldsLength = size;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002ACE4 File Offset: 0x00029CE4
		public virtual void ResetLocalHeaderExtraFieldSize()
		{
			this.LocalHeaderExtraFieldsLength = -1;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002ACF0 File Offset: 0x00029CF0
		public virtual byte[] GetBytes(ExtraFieldsTarget target)
		{
			byte[] array = new byte[this.InternalGetExtraFieldSize(target)];
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(array));
			binaryWriter.Write(this.HeaderId);
			binaryWriter.Write(this.DataSize);
			if (this.Data != null)
			{
				binaryWriter.Write(this.Data);
			}
			if (target == ExtraFieldsTarget.LocalHeaderExtraFields)
			{
				this.LocalHeaderExtraFieldsLength = -1;
			}
			return array;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002AD50 File Offset: 0x00029D50
		public virtual void WriteToStream(Stream stream, long offset, ExtraFieldsTarget target)
		{
			if (offset != 0L)
			{
				stream.Position = offset;
			}
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(this.HeaderId);
			binaryWriter.Write(this.DataSize);
			if (this.Data != null)
			{
				binaryWriter.Write(this.Data);
			}
			if (target == ExtraFieldsTarget.LocalHeaderExtraFields)
			{
				this.LocalHeaderExtraFieldsLength = -1;
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002ADA6 File Offset: 0x00029DA6
		public virtual int GetExtraFieldSize(ExtraFieldsTarget target)
		{
			if (target == ExtraFieldsTarget.LocalHeaderExtraFields && this.LocalHeaderExtraFieldsLength >= 0)
			{
				return this.LocalHeaderExtraFieldsLength;
			}
			return this.InternalGetExtraFieldSize(target);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002ADC2 File Offset: 0x00029DC2
		internal int InternalGetExtraFieldSize(ExtraFieldsTarget target)
		{
			return (int)(4 + this.GetDataSize(target));
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002ADCD File Offset: 0x00029DCD
		public virtual ushort GetExtraFieldId()
		{
			return this.HeaderId;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002ADD5 File Offset: 0x00029DD5
		public virtual ushort GetDataSize(ExtraFieldsTarget target)
		{
			return this.DataSize;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002ADDD File Offset: 0x00029DDD
		public virtual byte[] GetData(ExtraFieldsTarget target)
		{
			return this.Data;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002ADE8 File Offset: 0x00029DE8
		public virtual object Clone()
		{
			return new ExtraFieldData(this.HeaderId, this.DataSize, this.Data)
			{
				LocalHeaderExtraFieldsLength = this.LocalHeaderExtraFieldsLength
			};
		}

		// Token: 0x040003B1 RID: 945
		protected ushort HeaderId;

		// Token: 0x040003B2 RID: 946
		protected ushort DataSize;

		// Token: 0x040003B3 RID: 947
		protected byte[] Data;

		// Token: 0x040003B4 RID: 948
		protected int LocalHeaderExtraFieldsLength;
	}
}
