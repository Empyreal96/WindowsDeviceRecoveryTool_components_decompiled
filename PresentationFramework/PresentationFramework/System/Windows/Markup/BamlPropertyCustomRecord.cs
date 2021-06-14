using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x020001EA RID: 490
	internal class BamlPropertyCustomRecord : BamlVariableSizedRecord
	{
		// Token: 0x06001FAC RID: 8108 RVA: 0x000954B4 File Offset: 0x000936B4
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			short num = bamlBinaryReader.ReadInt16();
			this.IsValueTypeId = ((num & BamlPropertyCustomRecord.TypeIdValueMask) == BamlPropertyCustomRecord.TypeIdValueMask);
			if (this.IsValueTypeId)
			{
				num &= ~BamlPropertyCustomRecord.TypeIdValueMask;
			}
			this.SerializerTypeId = num;
			this.ValueObjectSet = false;
			this.IsRawEnumValueSet = false;
			this._valueObject = null;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x00095518 File Offset: 0x00093718
		internal object GetCustomValue(BinaryReader reader, Type propertyType, short serializerId, BamlRecordReader bamlRecordReader)
		{
			if (serializerId != 46)
			{
				if (serializerId != 195)
				{
					switch (serializerId)
					{
					case 744:
						this._valueObject = SolidColorBrush.DeserializeFrom(reader, bamlRecordReader.TypeConvertContext);
						goto IL_11D;
					case 745:
						this._valueObject = XamlInt32CollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 746:
						this._valueObject = XamlPathDataSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 747:
						this._valueObject = XamlPoint3DCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 748:
						this._valueObject = XamlPointCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					case 752:
						this._valueObject = XamlVector3DCollectionSerializer.StaticConvertCustomBinaryToObject(reader);
						goto IL_11D;
					}
					return null;
				}
				uint num;
				if (this._valueObject == null)
				{
					num = reader.ReadUInt32();
				}
				else
				{
					num = (uint)this._valueObject;
				}
				if (propertyType.IsEnum)
				{
					this._valueObject = Enum.ToObject(propertyType, num);
					this.ValueObjectSet = true;
					this.IsRawEnumValueSet = false;
				}
				else
				{
					this._valueObject = num;
					this.ValueObjectSet = false;
					this.IsRawEnumValueSet = true;
				}
				return this._valueObject;
			}
			else
			{
				byte b = reader.ReadByte();
				this._valueObject = (b == 1);
			}
			IL_11D:
			this.ValueObjectSet = true;
			return this._valueObject;
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00095650 File Offset: 0x00093850
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyCustomRecord bamlPropertyCustomRecord = (BamlPropertyCustomRecord)record;
			bamlPropertyCustomRecord._valueObject = this._valueObject;
			bamlPropertyCustomRecord._attributeId = this._attributeId;
			bamlPropertyCustomRecord._serializerTypeId = this._serializerTypeId;
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x00094FD6 File Offset: 0x000931D6
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyCustom;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0009568F File Offset: 0x0009388F
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x00095697 File Offset: 0x00093897
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

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x000956A0 File Offset: 0x000938A0
		// (set) Token: 0x06001FB3 RID: 8115 RVA: 0x000956A8 File Offset: 0x000938A8
		internal short SerializerTypeId
		{
			get
			{
				return this._serializerTypeId;
			}
			set
			{
				this._serializerTypeId = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000956B1 File Offset: 0x000938B1
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x000956B9 File Offset: 0x000938B9
		internal object ValueObject
		{
			get
			{
				return this._valueObject;
			}
			set
			{
				this._valueObject = value;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x000956C2 File Offset: 0x000938C2
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x000956DA File Offset: 0x000938DA
		internal bool ValueObjectSet
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isValueSetSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isValueSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x000956F3 File Offset: 0x000938F3
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x0009570B File Offset: 0x0009390B
		internal bool IsValueTypeId
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isValueTypeIdSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isValueTypeIdSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x00095724 File Offset: 0x00093924
		// (set) Token: 0x06001FBB RID: 8123 RVA: 0x0009573C File Offset: 0x0009393C
		internal bool IsRawEnumValueSet
		{
			get
			{
				return this._flags[BamlPropertyCustomRecord._isRawEnumValueSetSection] == 1;
			}
			set
			{
				this._flags[BamlPropertyCustomRecord._isRawEnumValueSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x00095755 File Offset: 0x00093955
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlPropertyCustomRecord._isRawEnumValueSetSection;
			}
		}

		// Token: 0x0400152C RID: 5420
		private object _valueObject;

		// Token: 0x0400152D RID: 5421
		private static BitVector32.Section _isValueSetSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x0400152E RID: 5422
		private static BitVector32.Section _isValueTypeIdSection = BitVector32.CreateSection(1, BamlPropertyCustomRecord._isValueSetSection);

		// Token: 0x0400152F RID: 5423
		private static BitVector32.Section _isRawEnumValueSetSection = BitVector32.CreateSection(1, BamlPropertyCustomRecord._isValueTypeIdSection);

		// Token: 0x04001530 RID: 5424
		internal static readonly short TypeIdValueMask = 16384;

		// Token: 0x04001531 RID: 5425
		private short _attributeId;

		// Token: 0x04001532 RID: 5426
		private short _serializerTypeId;
	}
}
