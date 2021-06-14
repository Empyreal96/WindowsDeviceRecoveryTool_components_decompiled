using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E1 RID: 481
	internal class BamlDefAttributeRecord : BamlStringValueRecord
	{
		// Token: 0x06001F48 RID: 8008 RVA: 0x00094A0B File Offset: 0x00092C0B
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			this.NameId = bamlBinaryReader.ReadInt16();
			this.Name = null;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00094A2C File Offset: 0x00092C2C
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(this.NameId);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x00094A48 File Offset: 0x00092C48
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)record;
			bamlDefAttributeRecord._name = this._name;
			bamlDefAttributeRecord._nameId = this._nameId;
			bamlDefAttributeRecord._attributeUsage = this._attributeUsage;
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x00094A87 File Offset: 0x00092C87
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttribute;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x00094A8B File Offset: 0x00092C8B
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x00094A93 File Offset: 0x00092C93
		internal short NameId
		{
			get
			{
				return this._nameId;
			}
			set
			{
				this._nameId = value;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x00094A9C File Offset: 0x00092C9C
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x00094AA4 File Offset: 0x00092CA4
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x00094AAD File Offset: 0x00092CAD
		// (set) Token: 0x06001F51 RID: 8017 RVA: 0x00094AB5 File Offset: 0x00092CB5
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
			set
			{
				this._attributeUsage = value;
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x00094AC0 File Offset: 0x00092CC0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} nameId({1}) is '{2}' usage={3}", new object[]
			{
				this.RecordType,
				this.NameId,
				this.Name,
				this.AttributeUsage
			});
		}

		// Token: 0x04001514 RID: 5396
		private string _name;

		// Token: 0x04001515 RID: 5397
		private short _nameId;

		// Token: 0x04001516 RID: 5398
		private BamlAttributeUsage _attributeUsage;
	}
}
