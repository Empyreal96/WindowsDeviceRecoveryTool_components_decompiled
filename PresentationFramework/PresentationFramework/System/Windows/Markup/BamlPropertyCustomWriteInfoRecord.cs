using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020001E9 RID: 489
	internal class BamlPropertyCustomWriteInfoRecord : BamlPropertyCustomRecord
	{
		// Token: 0x06001F9D RID: 8093 RVA: 0x000950F0 File Offset: 0x000932F0
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			int num = (int)bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			short serializerTypeId = base.SerializerTypeId;
			bamlBinaryWriter.Write(base.AttributeId);
			if (serializerTypeId == 137)
			{
				if (this.ValueMemberName != null)
				{
					bamlBinaryWriter.Write(serializerTypeId | BamlPropertyCustomRecord.TypeIdValueMask);
				}
				else
				{
					bamlBinaryWriter.Write(serializerTypeId);
				}
				bamlBinaryWriter.Write(this.ValueId);
				if (this.ValueMemberName != null)
				{
					bamlBinaryWriter.Write(this.ValueMemberName);
				}
				return;
			}
			bamlBinaryWriter.Write(serializerTypeId);
			bool flag = false;
			if (this.ValueType != null && this.ValueType.IsEnum)
			{
				uint num2 = 0U;
				string[] array = this.Value.Split(new char[]
				{
					','
				});
				foreach (string text in array)
				{
					FieldInfo field = this.ValueType.GetField(text.Trim(), BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
					if (!(field != null))
					{
						flag = false;
						break;
					}
					object rawConstantValue = field.GetRawConstantValue();
					num2 += (uint)Convert.ChangeType(rawConstantValue, typeof(uint), TypeConverterHelper.InvariantEnglishUS);
					flag = true;
				}
				if (flag)
				{
					bamlBinaryWriter.Write(num2);
				}
			}
			else if (this.ValueType == typeof(bool))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(bool));
				object value = converter.ConvertFromString(this.TypeContext, TypeConverterHelper.InvariantEnglishUS, this.Value);
				bamlBinaryWriter.Write((byte)Convert.ChangeType(value, typeof(byte), TypeConverterHelper.InvariantEnglishUS));
				flag = true;
			}
			else if (this.SerializerType == typeof(XamlBrushSerializer))
			{
				XamlSerializer xamlSerializer = new XamlBrushSerializer();
				flag = xamlSerializer.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPoint3DCollectionSerializer))
			{
				XamlSerializer xamlSerializer2 = new XamlPoint3DCollectionSerializer();
				flag = xamlSerializer2.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlVector3DCollectionSerializer))
			{
				XamlSerializer xamlSerializer3 = new XamlVector3DCollectionSerializer();
				flag = xamlSerializer3.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPointCollectionSerializer))
			{
				XamlSerializer xamlSerializer4 = new XamlPointCollectionSerializer();
				flag = xamlSerializer4.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlInt32CollectionSerializer))
			{
				XamlSerializer xamlSerializer5 = new XamlInt32CollectionSerializer();
				flag = xamlSerializer5.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			else if (this.SerializerType == typeof(XamlPathDataSerializer))
			{
				XamlSerializer xamlSerializer6 = new XamlPathDataSerializer();
				flag = xamlSerializer6.ConvertStringToCustomBinary(bamlBinaryWriter, this.Value);
			}
			if (!flag)
			{
				throw new XamlParseException(SR.Get("ParserBadString", new object[]
				{
					this.Value,
					this.ValueType.Name
				}));
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000953E0 File Offset: 0x000935E0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyCustomWriteInfoRecord bamlPropertyCustomWriteInfoRecord = (BamlPropertyCustomWriteInfoRecord)record;
			bamlPropertyCustomWriteInfoRecord._valueId = this._valueId;
			bamlPropertyCustomWriteInfoRecord._valueType = this._valueType;
			bamlPropertyCustomWriteInfoRecord._value = this._value;
			bamlPropertyCustomWriteInfoRecord._valueMemberName = this._valueMemberName;
			bamlPropertyCustomWriteInfoRecord._serializerType = this._serializerType;
			bamlPropertyCustomWriteInfoRecord._typeContext = this._typeContext;
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x00095443 File Offset: 0x00093643
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x0009544B File Offset: 0x0009364B
		internal short ValueId
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

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x00095454 File Offset: 0x00093654
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x0009545C File Offset: 0x0009365C
		internal string ValueMemberName
		{
			get
			{
				return this._valueMemberName;
			}
			set
			{
				this._valueMemberName = value;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x00095465 File Offset: 0x00093665
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x0009546D File Offset: 0x0009366D
		internal Type ValueType
		{
			get
			{
				return this._valueType;
			}
			set
			{
				this._valueType = value;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x00095476 File Offset: 0x00093676
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x0009547E File Offset: 0x0009367E
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x00095487 File Offset: 0x00093687
		// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x0009548F File Offset: 0x0009368F
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
			set
			{
				this._serializerType = value;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x00095498 File Offset: 0x00093698
		// (set) Token: 0x06001FAA RID: 8106 RVA: 0x000954A0 File Offset: 0x000936A0
		internal ITypeDescriptorContext TypeContext
		{
			get
			{
				return this._typeContext;
			}
			set
			{
				this._typeContext = value;
			}
		}

		// Token: 0x04001526 RID: 5414
		private short _valueId;

		// Token: 0x04001527 RID: 5415
		private Type _valueType;

		// Token: 0x04001528 RID: 5416
		private string _value;

		// Token: 0x04001529 RID: 5417
		private string _valueMemberName;

		// Token: 0x0400152A RID: 5418
		private Type _serializerType;

		// Token: 0x0400152B RID: 5419
		private ITypeDescriptorContext _typeContext;
	}
}
