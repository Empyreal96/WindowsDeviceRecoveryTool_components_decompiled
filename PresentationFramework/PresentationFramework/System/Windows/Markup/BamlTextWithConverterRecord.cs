using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000202 RID: 514
	internal class BamlTextWithConverterRecord : BamlTextRecord
	{
		// Token: 0x06002043 RID: 8259 RVA: 0x00096010 File Offset: 0x00094210
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00096025 File Offset: 0x00094225
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0009603C File Offset: 0x0009423C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTextWithConverterRecord bamlTextWithConverterRecord = (BamlTextWithConverterRecord)record;
			bamlTextWithConverterRecord._converterTypeId = this._converterTypeId;
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x00096063 File Offset: 0x00094263
		// (set) Token: 0x06002047 RID: 8263 RVA: 0x0009606B File Offset: 0x0009426B
		internal short ConverterTypeId
		{
			get
			{
				return this._converterTypeId;
			}
			set
			{
				this._converterTypeId = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x00096074 File Offset: 0x00094274
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithConverter;
			}
		}

		// Token: 0x04001548 RID: 5448
		private short _converterTypeId;
	}
}
