using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000201 RID: 513
	internal class BamlTextWithIdRecord : BamlTextRecord
	{
		// Token: 0x0600203C RID: 8252 RVA: 0x00095FAF File Offset: 0x000941AF
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ValueId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00095FBD File Offset: 0x000941BD
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00095FCC File Offset: 0x000941CC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTextWithIdRecord bamlTextWithIdRecord = (BamlTextWithIdRecord)record;
			bamlTextWithIdRecord._valueId = this._valueId;
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00095FF3 File Offset: 0x000941F3
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithId;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x00095FF7 File Offset: 0x000941F7
		// (set) Token: 0x06002041 RID: 8257 RVA: 0x00095FFF File Offset: 0x000941FF
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
			}
		}

		// Token: 0x04001547 RID: 5447
		private short _valueId;
	}
}
