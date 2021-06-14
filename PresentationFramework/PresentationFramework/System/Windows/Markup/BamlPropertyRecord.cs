using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E7 RID: 487
	internal class BamlPropertyRecord : BamlStringValueRecord
	{
		// Token: 0x06001F81 RID: 8065 RVA: 0x00094DE8 File Offset: 0x00092FE8
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x00094E02 File Offset: 0x00093002
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.Value);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00094E1C File Offset: 0x0009301C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyRecord bamlPropertyRecord = (BamlPropertyRecord)record;
			bamlPropertyRecord._attributeId = this._attributeId;
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x00094E43 File Offset: 0x00093043
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Property;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x00094E46 File Offset: 0x00093046
		// (set) Token: 0x06001F86 RID: 8070 RVA: 0x00094E4E File Offset: 0x0009304E
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

		// Token: 0x06001F87 RID: 8071 RVA: 0x00094E57 File Offset: 0x00093057
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) <== '{2}'", new object[]
			{
				this.RecordType,
				this._attributeId,
				base.Value
			});
		}

		// Token: 0x0400151D RID: 5405
		private short _attributeId = -1;
	}
}
