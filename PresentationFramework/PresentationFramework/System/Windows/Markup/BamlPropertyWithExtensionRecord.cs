using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E8 RID: 488
	internal class BamlPropertyWithExtensionRecord : BamlRecord, IOptimizedMarkupExtension
	{
		// Token: 0x06001F89 RID: 8073 RVA: 0x00094EA4 File Offset: 0x000930A4
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			short num = bamlBinaryReader.ReadInt16();
			this.ValueId = bamlBinaryReader.ReadInt16();
			this._extensionTypeId = (num & BamlPropertyWithExtensionRecord.ExtensionIdMask);
			this.IsValueTypeExtension = ((num & BamlPropertyWithExtensionRecord.TypeExtensionValueMask) == BamlPropertyWithExtensionRecord.TypeExtensionValueMask);
			this.IsValueStaticExtension = ((num & BamlPropertyWithExtensionRecord.StaticExtensionValueMask) == BamlPropertyWithExtensionRecord.StaticExtensionValueMask);
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00094F08 File Offset: 0x00093108
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			short num = this.ExtensionTypeId;
			if (this.IsValueTypeExtension)
			{
				num |= BamlPropertyWithExtensionRecord.TypeExtensionValueMask;
			}
			else if (this.IsValueStaticExtension)
			{
				num |= BamlPropertyWithExtensionRecord.StaticExtensionValueMask;
			}
			bamlBinaryWriter.Write(num);
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00094F60 File Offset: 0x00093160
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyWithExtensionRecord bamlPropertyWithExtensionRecord = (BamlPropertyWithExtensionRecord)record;
			bamlPropertyWithExtensionRecord._attributeId = this._attributeId;
			bamlPropertyWithExtensionRecord._extensionTypeId = this._extensionTypeId;
			bamlPropertyWithExtensionRecord._valueId = this._valueId;
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x00094F9F File Offset: 0x0009319F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithExtension;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x00094FA3 File Offset: 0x000931A3
		// (set) Token: 0x06001F8E RID: 8078 RVA: 0x00094FAB File Offset: 0x000931AB
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

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x00094FB4 File Offset: 0x000931B4
		// (set) Token: 0x06001F90 RID: 8080 RVA: 0x00094FBC File Offset: 0x000931BC
		public short ExtensionTypeId
		{
			get
			{
				return this._extensionTypeId;
			}
			set
			{
				this._extensionTypeId = value;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00094FC5 File Offset: 0x000931C5
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x00094FCD File Offset: 0x000931CD
		public short ValueId
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

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x00094FD6 File Offset: 0x000931D6
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 6;
			}
			set
			{
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x00094FD9 File Offset: 0x000931D9
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x00094FF1 File Offset: 0x000931F1
		public bool IsValueTypeExtension
		{
			get
			{
				return this._flags[BamlPropertyWithExtensionRecord._isValueTypeExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyWithExtensionRecord._isValueTypeExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0009500A File Offset: 0x0009320A
		// (set) Token: 0x06001F98 RID: 8088 RVA: 0x00095022 File Offset: 0x00093222
		public bool IsValueStaticExtension
		{
			get
			{
				return this._flags[BamlPropertyWithExtensionRecord._isValueStaticExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyWithExtensionRecord._isValueStaticExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0009503B File Offset: 0x0009323B
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPropertyWithExtensionRecord._isValueStaticExtensionSection;
			}
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x00095044 File Offset: 0x00093244
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) extn({2}) valueId({3})", new object[]
			{
				this.RecordType,
				this._attributeId,
				this._extensionTypeId,
				this._valueId
			});
		}

		// Token: 0x0400151E RID: 5406
		private static BitVector32.Section _isValueTypeExtensionSection = BitVector32.CreateSection(1, BamlRecord.LastFlagsSection);

		// Token: 0x0400151F RID: 5407
		private static BitVector32.Section _isValueStaticExtensionSection = BitVector32.CreateSection(1, BamlPropertyWithExtensionRecord._isValueTypeExtensionSection);

		// Token: 0x04001520 RID: 5408
		private short _attributeId = -1;

		// Token: 0x04001521 RID: 5409
		private short _extensionTypeId;

		// Token: 0x04001522 RID: 5410
		private short _valueId;

		// Token: 0x04001523 RID: 5411
		private static readonly short ExtensionIdMask = 4095;

		// Token: 0x04001524 RID: 5412
		private static readonly short TypeExtensionValueMask = 16384;

		// Token: 0x04001525 RID: 5413
		private static readonly short StaticExtensionValueMask = 8192;
	}
}
