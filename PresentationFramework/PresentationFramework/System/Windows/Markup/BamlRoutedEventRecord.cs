using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F5 RID: 501
	internal class BamlRoutedEventRecord : BamlStringValueRecord
	{
		// Token: 0x06001FDA RID: 8154 RVA: 0x00095817 File Offset: 0x00093A17
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00095831 File Offset: 0x00093A31
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.Value);
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0009584C File Offset: 0x00093A4C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)record;
			bamlRoutedEventRecord._attributeId = this._attributeId;
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x00095873 File Offset: 0x00093A73
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.RoutedEvent;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x00095877 File Offset: 0x00093A77
		// (set) Token: 0x06001FDF RID: 8159 RVA: 0x0009587F File Offset: 0x00093A7F
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

		// Token: 0x04001534 RID: 5428
		private short _attributeId = -1;
	}
}
