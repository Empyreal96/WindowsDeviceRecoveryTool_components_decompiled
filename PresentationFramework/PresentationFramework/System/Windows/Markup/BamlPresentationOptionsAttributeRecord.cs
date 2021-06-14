using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E2 RID: 482
	internal class BamlPresentationOptionsAttributeRecord : BamlStringValueRecord
	{
		// Token: 0x06001F54 RID: 8020 RVA: 0x00094B1D File Offset: 0x00092D1D
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			this.NameId = bamlBinaryReader.ReadInt16();
			this.Name = null;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x00094B3E File Offset: 0x00092D3E
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(this.NameId);
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x00094B58 File Offset: 0x00092D58
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)record;
			bamlPresentationOptionsAttributeRecord._name = this._name;
			bamlPresentationOptionsAttributeRecord._nameId = this._nameId;
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x00094B8B File Offset: 0x00092D8B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PresentationOptionsAttribute;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x00094B8F File Offset: 0x00092D8F
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x00094B97 File Offset: 0x00092D97
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

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x00094BA0 File Offset: 0x00092DA0
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x00094BA8 File Offset: 0x00092DA8
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

		// Token: 0x06001F5C RID: 8028 RVA: 0x00094BB1 File Offset: 0x00092DB1
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} nameId({1}) is '{2}' ", new object[]
			{
				this.RecordType,
				this.NameId,
				this.Name
			});
		}

		// Token: 0x04001517 RID: 5399
		private string _name;

		// Token: 0x04001518 RID: 5400
		private short _nameId;
	}
}
