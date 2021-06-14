using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E6 RID: 486
	internal class BamlPropertyWithConverterRecord : BamlPropertyRecord
	{
		// Token: 0x06001F7A RID: 8058 RVA: 0x00094D78 File Offset: 0x00092F78
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00094D8D File Offset: 0x00092F8D
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00094DA4 File Offset: 0x00092FA4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyWithConverterRecord bamlPropertyWithConverterRecord = (BamlPropertyWithConverterRecord)record;
			bamlPropertyWithConverterRecord._converterTypeId = this._converterTypeId;
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x00094DCB File Offset: 0x00092FCB
		// (set) Token: 0x06001F7E RID: 8062 RVA: 0x00094DD3 File Offset: 0x00092FD3
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

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x00094DDC File Offset: 0x00092FDC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithConverter;
			}
		}

		// Token: 0x0400151C RID: 5404
		private short _converterTypeId;
	}
}
