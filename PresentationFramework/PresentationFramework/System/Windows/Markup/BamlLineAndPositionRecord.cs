using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200020E RID: 526
	internal class BamlLineAndPositionRecord : BamlRecord
	{
		// Token: 0x060020C5 RID: 8389 RVA: 0x00096BBF File Offset: 0x00094DBF
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LineNumber = (uint)bamlBinaryReader.ReadInt32();
			this.LinePosition = (uint)bamlBinaryReader.ReadInt32();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00096BD9 File Offset: 0x00094DD9
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LineNumber);
			bamlBinaryWriter.Write(this.LinePosition);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00096BF4 File Offset: 0x00094DF4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlLineAndPositionRecord bamlLineAndPositionRecord = (BamlLineAndPositionRecord)record;
			bamlLineAndPositionRecord._lineNumber = this._lineNumber;
			bamlLineAndPositionRecord._linePosition = this._linePosition;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x00096C27 File Offset: 0x00094E27
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LineNumberAndPosition;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00096C2B File Offset: 0x00094E2B
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x00096C33 File Offset: 0x00094E33
		internal uint LineNumber
		{
			get
			{
				return this._lineNumber;
			}
			set
			{
				this._lineNumber = value;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x00096C3C File Offset: 0x00094E3C
		// (set) Token: 0x060020CC RID: 8396 RVA: 0x00096C44 File Offset: 0x00094E44
		internal uint LinePosition
		{
			get
			{
				return this._linePosition;
			}
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0009580C File Offset: 0x00093A0C
		internal override int RecordSize
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00096C50 File Offset: 0x00094E50
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} LineNum={1} Pos={2}", new object[]
			{
				this.RecordType,
				this.LineNumber,
				this.LinePosition
			});
		}

		// Token: 0x0400156A RID: 5482
		private uint _lineNumber;

		// Token: 0x0400156B RID: 5483
		private uint _linePosition;
	}
}
