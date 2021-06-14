using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001EE RID: 494
	internal class BamlConstructorParameterTypeRecord : BamlRecord
	{
		// Token: 0x06001FC5 RID: 8133 RVA: 0x000957AC File Offset: 0x000939AC
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000957BA File Offset: 0x000939BA
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000957C8 File Offset: 0x000939C8
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlConstructorParameterTypeRecord bamlConstructorParameterTypeRecord = (BamlConstructorParameterTypeRecord)record;
			bamlConstructorParameterTypeRecord._typeId = this._typeId;
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x000957EF File Offset: 0x000939EF
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ConstructorParameterType;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x000957F3 File Offset: 0x000939F3
		// (set) Token: 0x06001FCA RID: 8138 RVA: 0x000957FB File Offset: 0x000939FB
		internal short TypeId
		{
			get
			{
				return this._typeId;
			}
			set
			{
				this._typeId = value;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x00094C44 File Offset: 0x00092E44
		// (set) Token: 0x06001FCC RID: 8140 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x04001533 RID: 5427
		private short _typeId;
	}
}
