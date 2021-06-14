using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E3 RID: 483
	internal class BamlPropertyComplexStartRecord : BamlRecord
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x00094BED File Offset: 0x00092DED
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x00094BFB File Offset: 0x00092DFB
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x00094C0C File Offset: 0x00092E0C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyComplexStartRecord bamlPropertyComplexStartRecord = (BamlPropertyComplexStartRecord)record;
			bamlPropertyComplexStartRecord._attributeId = this._attributeId;
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06001F61 RID: 8033 RVA: 0x0001321D File Offset: 0x0001141D
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyComplexStart;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x00094C33 File Offset: 0x00092E33
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x00094C3B File Offset: 0x00092E3B
		internal short AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x00094C44 File Offset: 0x00092E44
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 2;
			}
			set
			{
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00094C47 File Offset: 0x00092E47
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1})", new object[]
			{
				this.RecordType,
				this._attributeId
			});
		}

		// Token: 0x04001519 RID: 5401
		private short _attributeId = -1;
	}
}
