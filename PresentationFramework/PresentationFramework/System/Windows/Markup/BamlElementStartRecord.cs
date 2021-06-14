using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F8 RID: 504
	internal class BamlElementStartRecord : BamlRecord
	{
		// Token: 0x06001FEE RID: 8174 RVA: 0x00095948 File Offset: 0x00093B48
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
			byte b = bamlBinaryReader.ReadByte();
			this.CreateUsingTypeConverter = ((b & 1) > 0);
			this.IsInjected = ((b & 2) > 0);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x00095980 File Offset: 0x00093B80
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
			byte value = (byte)((this.CreateUsingTypeConverter ? 1 : 0) | (this.IsInjected ? 2 : 0));
			bamlBinaryWriter.Write(value);
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x000800F2 File Offset: 0x0007E2F2
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ElementStart;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x000959BC File Offset: 0x00093BBC
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x000959F3 File Offset: 0x00093BF3
		internal short TypeId
		{
			get
			{
				short num = (short)this._flags[BamlElementStartRecord._typeIdLowSection];
				return num | (short)(this._flags[BamlElementStartRecord._typeIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlElementStartRecord._typeIdLowSection] = (int)(value & 255);
				this._flags[BamlElementStartRecord._typeIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x00095A27 File Offset: 0x00093C27
		// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x00095A3F File Offset: 0x00093C3F
		internal bool CreateUsingTypeConverter
		{
			get
			{
				return this._flags[BamlElementStartRecord._useTypeConverter] == 1;
			}
			set
			{
				this._flags[BamlElementStartRecord._useTypeConverter] = (value ? 1 : 0);
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x00095A58 File Offset: 0x00093C58
		// (set) Token: 0x06001FF6 RID: 8182 RVA: 0x00095A70 File Offset: 0x00093C70
		internal bool IsInjected
		{
			get
			{
				return this._flags[BamlElementStartRecord._isInjected] == 1;
			}
			set
			{
				this._flags[BamlElementStartRecord._isInjected] = (value ? 1 : 0);
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x000800F2 File Offset: 0x0007E2F2
		// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 3;
			}
			set
			{
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00095A89 File Offset: 0x00093C89
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} typeId={1}", new object[]
			{
				this.RecordType,
				BamlRecord.GetTypeName((int)this.TypeId)
			});
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x00095ABC File Offset: 0x00093CBC
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlElementStartRecord._isInjected;
			}
		}

		// Token: 0x04001536 RID: 5430
		private static BitVector32.Section _typeIdLowSection = BitVector32.CreateSection(255, BamlRecord.LastFlagsSection);

		// Token: 0x04001537 RID: 5431
		private static BitVector32.Section _typeIdHighSection = BitVector32.CreateSection(255, BamlElementStartRecord._typeIdLowSection);

		// Token: 0x04001538 RID: 5432
		private static BitVector32.Section _useTypeConverter = BitVector32.CreateSection(1, BamlElementStartRecord._typeIdHighSection);

		// Token: 0x04001539 RID: 5433
		private static BitVector32.Section _isInjected = BitVector32.CreateSection(1, BamlElementStartRecord._useTypeConverter);
	}
}
