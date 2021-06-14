using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E4 RID: 484
	internal class BamlPropertyStringReferenceRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x06001F68 RID: 8040 RVA: 0x00094C89 File Offset: 0x00092E89
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.AttributeId = bamlBinaryReader.ReadInt16();
			this.StringId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00094CA3 File Offset: 0x00092EA3
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.AttributeId);
			bamlBinaryWriter.Write(this.StringId);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x00094CC0 File Offset: 0x00092EC0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyStringReferenceRecord bamlPropertyStringReferenceRecord = (BamlPropertyStringReferenceRecord)record;
			bamlPropertyStringReferenceRecord._stringId = this._stringId;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x00094CE7 File Offset: 0x00092EE7
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyStringReference;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x00094CEB File Offset: 0x00092EEB
		// (set) Token: 0x06001F6D RID: 8045 RVA: 0x00094CF3 File Offset: 0x00092EF3
		internal short StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				this._stringId = value;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x00094CFC File Offset: 0x00092EFC
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x0400151A RID: 5402
		private short _stringId;
	}
}
