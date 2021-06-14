using System;
using System.IO;
using System.Text;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.Exception;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000098 RID: 152
	internal class UnicodeExtraFieldData : ExtraFieldData
	{
		// Token: 0x060006DE RID: 1758 RVA: 0x0002AE1A File Offset: 0x00029E1A
		public UnicodeExtraFieldData() : this("")
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002AE27 File Offset: 0x00029E27
		public UnicodeExtraFieldData(string fileName)
		{
			this.HeaderId = 21838;
			this._fileName = fileName;
			this._fileNameLength = fileName.Length;
			this.UpdateBaseClassFields();
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002AE54 File Offset: 0x00029E54
		public static UnicodeExtraFieldData LoadFromStream(Stream source, DirItem item)
		{
			BinaryReader binaryReader = new BinaryReader(source);
			binaryReader.ReadUInt16();
			int num = binaryReader.ReadInt32();
			if (num != 1480807758)
			{
				throw ExceptionBuilder.Exception(ErrorCode.InvalidUnicodeExtraFieldSignature);
			}
			int num2 = binaryReader.ReadInt32();
			byte[] array = binaryReader.ReadBytes(num2 * 2);
			string @string = Encoding.Unicode.GetString(array, 0, array.Length);
			return new UnicodeExtraFieldData(@string);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002AEB0 File Offset: 0x00029EB0
		public override object Clone()
		{
			return new UnicodeExtraFieldData(this._fileName)
			{
				LocalHeaderExtraFieldsLength = this.LocalHeaderExtraFieldsLength
			};
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002AED6 File Offset: 0x00029ED6
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0002AEDE File Offset: 0x00029EDE
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				this._fileNameLength = value.Length;
				this._fileName = value;
				this.UpdateBaseClassFields();
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002AEF9 File Offset: 0x00029EF9
		public int FileNameLength
		{
			get
			{
				return this._fileNameLength;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002AF01 File Offset: 0x00029F01
		private ushort GetFieldDataBytesLength()
		{
			return (ushort)(8 + this._fileNameLength * 2);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002AF10 File Offset: 0x00029F10
		private byte[] GetFieldDataBytes()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(1480807758);
			binaryWriter.Write(this._fileNameLength);
			binaryWriter.Write(Encoding.Unicode.GetBytes(this._fileName));
			return memoryStream.ToArray();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002AF5D File Offset: 0x00029F5D
		private void UpdateBaseClassFields()
		{
			this.DataSize = this.GetFieldDataBytesLength();
			this.Data = this.GetFieldDataBytes();
		}

		// Token: 0x040003B5 RID: 949
		private const int UnicodeSignature = 1480807758;

		// Token: 0x040003B6 RID: 950
		private const ushort FieldId = 21838;

		// Token: 0x040003B7 RID: 951
		private string _fileName;

		// Token: 0x040003B8 RID: 952
		private int _fileNameLength;
	}
}
