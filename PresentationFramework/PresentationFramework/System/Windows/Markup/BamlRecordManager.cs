using System;
using System.Collections;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001D7 RID: 471
	internal class BamlRecordManager
	{
		// Token: 0x06001EC6 RID: 7878 RVA: 0x00093B90 File Offset: 0x00091D90
		internal BamlRecord ReadNextRecord(BinaryReader bamlBinaryReader, long bytesAvailable, BamlRecordType recordType)
		{
			BamlRecord bamlRecord;
			switch (recordType)
			{
			case BamlRecordType.AssemblyInfo:
				bamlRecord = new BamlAssemblyInfoRecord();
				goto IL_A6;
			case BamlRecordType.TypeInfo:
				bamlRecord = new BamlTypeInfoRecord();
				goto IL_A6;
			case BamlRecordType.TypeSerializerInfo:
				bamlRecord = new BamlTypeInfoWithSerializerRecord();
				goto IL_A6;
			case BamlRecordType.AttributeInfo:
				bamlRecord = new BamlAttributeInfoRecord();
				goto IL_A6;
			case BamlRecordType.StringInfo:
				bamlRecord = new BamlStringInfoRecord();
				goto IL_A6;
			case BamlRecordType.DefAttributeKeyString:
				bamlRecord = new BamlDefAttributeKeyStringRecord();
				goto IL_A6;
			case BamlRecordType.DefAttributeKeyType:
				bamlRecord = new BamlDefAttributeKeyTypeRecord();
				goto IL_A6;
			case BamlRecordType.KeyElementStart:
				bamlRecord = new BamlKeyElementStartRecord();
				goto IL_A6;
			}
			bamlRecord = this._readCache[(int)recordType];
			if (bamlRecord == null || bamlRecord.IsPinned)
			{
				bamlRecord = (this._readCache[(int)recordType] = this.AllocateRecord(recordType));
			}
			IL_A6:
			bamlRecord.Next = null;
			if (bamlRecord != null)
			{
				if (bamlRecord.LoadRecordSize(bamlBinaryReader, bytesAvailable) && bytesAvailable >= (long)bamlRecord.RecordSize)
				{
					bamlRecord.LoadRecordData(bamlBinaryReader);
				}
				else
				{
					bamlRecord = null;
				}
			}
			return bamlRecord;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00093C70 File Offset: 0x00091E70
		internal static IAddChild AsIAddChild(object obj)
		{
			return obj as IAddChildInternal;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00093C85 File Offset: 0x00091E85
		internal static bool TreatAsIAddChild(Type parentObjectType)
		{
			return KnownTypes.Types[275].IsAssignableFrom(parentObjectType);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00093C9C File Offset: 0x00091E9C
		internal static BamlRecordType GetPropertyStartRecordType(Type propertyType, bool propertyCanWrite)
		{
			BamlRecordType result;
			if (propertyType.IsArray)
			{
				result = BamlRecordType.PropertyArrayStart;
			}
			else if (typeof(IDictionary).IsAssignableFrom(propertyType))
			{
				result = BamlRecordType.PropertyIDictionaryStart;
			}
			else if (typeof(IList).IsAssignableFrom(propertyType) || BamlRecordManager.TreatAsIAddChild(propertyType) || (typeof(IEnumerable).IsAssignableFrom(propertyType) && !propertyCanWrite))
			{
				result = BamlRecordType.PropertyIListStart;
			}
			else
			{
				result = BamlRecordType.PropertyComplexStart;
			}
			return result;
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00093D04 File Offset: 0x00091F04
		internal BamlRecord CloneRecord(BamlRecord record)
		{
			BamlRecordType recordType = record.RecordType;
			BamlRecord bamlRecord;
			if (recordType != BamlRecordType.ElementStart)
			{
				if (recordType != BamlRecordType.PropertyCustom)
				{
					bamlRecord = this.AllocateRecord(record.RecordType);
				}
				else if (record is BamlPropertyCustomWriteInfoRecord)
				{
					bamlRecord = new BamlPropertyCustomWriteInfoRecord();
				}
				else
				{
					bamlRecord = new BamlPropertyCustomRecord();
				}
			}
			else if (record is BamlNamedElementStartRecord)
			{
				bamlRecord = new BamlNamedElementStartRecord();
			}
			else
			{
				bamlRecord = new BamlElementStartRecord();
			}
			record.Copy(bamlRecord);
			return bamlRecord;
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00093D68 File Offset: 0x00091F68
		private BamlRecord AllocateWriteRecord(BamlRecordType recordType)
		{
			BamlRecord result;
			if (recordType == BamlRecordType.PropertyCustom)
			{
				result = new BamlPropertyCustomWriteInfoRecord();
			}
			else
			{
				result = this.AllocateRecord(recordType);
			}
			return result;
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00093D8C File Offset: 0x00091F8C
		private BamlRecord AllocateRecord(BamlRecordType recordType)
		{
			switch (recordType)
			{
			case BamlRecordType.DocumentStart:
				return new BamlDocumentStartRecord();
			case BamlRecordType.DocumentEnd:
				return new BamlDocumentEndRecord();
			case BamlRecordType.ElementStart:
				return new BamlElementStartRecord();
			case BamlRecordType.ElementEnd:
				return new BamlElementEndRecord();
			case BamlRecordType.Property:
				return new BamlPropertyRecord();
			case BamlRecordType.PropertyCustom:
				return new BamlPropertyCustomRecord();
			case BamlRecordType.PropertyComplexStart:
				return new BamlPropertyComplexStartRecord();
			case BamlRecordType.PropertyComplexEnd:
				return new BamlPropertyComplexEndRecord();
			case BamlRecordType.PropertyArrayStart:
				return new BamlPropertyArrayStartRecord();
			case BamlRecordType.PropertyArrayEnd:
				return new BamlPropertyArrayEndRecord();
			case BamlRecordType.PropertyIListStart:
				return new BamlPropertyIListStartRecord();
			case BamlRecordType.PropertyIListEnd:
				return new BamlPropertyIListEndRecord();
			case BamlRecordType.PropertyIDictionaryStart:
				return new BamlPropertyIDictionaryStartRecord();
			case BamlRecordType.PropertyIDictionaryEnd:
				return new BamlPropertyIDictionaryEndRecord();
			case BamlRecordType.LiteralContent:
				return new BamlLiteralContentRecord();
			case BamlRecordType.Text:
				return new BamlTextRecord();
			case BamlRecordType.TextWithConverter:
				return new BamlTextWithConverterRecord();
			case BamlRecordType.RoutedEvent:
				return new BamlRoutedEventRecord();
			case BamlRecordType.XmlnsProperty:
				return new BamlXmlnsPropertyRecord();
			case BamlRecordType.DefAttribute:
				return new BamlDefAttributeRecord();
			case BamlRecordType.PIMapping:
				return new BamlPIMappingRecord();
			case BamlRecordType.AssemblyInfo:
			case BamlRecordType.TypeInfo:
			case BamlRecordType.TypeSerializerInfo:
			case BamlRecordType.AttributeInfo:
			case BamlRecordType.StringInfo:
				return null;
			case BamlRecordType.PropertyStringReference:
				return new BamlPropertyStringReferenceRecord();
			case BamlRecordType.PropertyTypeReference:
				return new BamlPropertyTypeReferenceRecord();
			case BamlRecordType.PropertyWithExtension:
				return new BamlPropertyWithExtensionRecord();
			case BamlRecordType.PropertyWithConverter:
				return new BamlPropertyWithConverterRecord();
			case BamlRecordType.DeferableContentStart:
				return new BamlDeferableContentStartRecord();
			case BamlRecordType.DefAttributeKeyString:
				return new BamlDefAttributeKeyStringRecord();
			case BamlRecordType.DefAttributeKeyType:
				return new BamlDefAttributeKeyTypeRecord();
			case BamlRecordType.KeyElementStart:
				return new BamlKeyElementStartRecord();
			case BamlRecordType.KeyElementEnd:
				return new BamlKeyElementEndRecord();
			case BamlRecordType.ConstructorParametersStart:
				return new BamlConstructorParametersStartRecord();
			case BamlRecordType.ConstructorParametersEnd:
				return new BamlConstructorParametersEndRecord();
			case BamlRecordType.ConstructorParameterType:
				return new BamlConstructorParameterTypeRecord();
			case BamlRecordType.ConnectionId:
				return new BamlConnectionIdRecord();
			case BamlRecordType.ContentProperty:
				return new BamlContentPropertyRecord();
			case BamlRecordType.StaticResourceStart:
				return new BamlStaticResourceStartRecord();
			case BamlRecordType.StaticResourceEnd:
				return new BamlStaticResourceEndRecord();
			case BamlRecordType.StaticResourceId:
				return new BamlStaticResourceIdRecord();
			case BamlRecordType.TextWithId:
				return new BamlTextWithIdRecord();
			case BamlRecordType.PresentationOptionsAttribute:
				return new BamlPresentationOptionsAttributeRecord();
			case BamlRecordType.LineNumberAndPosition:
				return new BamlLineAndPositionRecord();
			case BamlRecordType.LinePosition:
				return new BamlLinePositionRecord();
			case BamlRecordType.OptimizedStaticResource:
				return new BamlOptimizedStaticResourceRecord();
			case BamlRecordType.PropertyWithStaticResourceId:
				return new BamlPropertyWithStaticResourceIdRecord();
			}
			return null;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00094044 File Offset: 0x00092244
		internal BamlRecord GetWriteRecord(BamlRecordType recordType)
		{
			if (this._writeCache == null)
			{
				this._writeCache = new BamlRecord[57];
			}
			BamlRecord bamlRecord = this._writeCache[(int)recordType];
			if (bamlRecord == null)
			{
				bamlRecord = this.AllocateWriteRecord(recordType);
			}
			else
			{
				this._writeCache[(int)recordType] = null;
			}
			bamlRecord.RecordSize = -1;
			return bamlRecord;
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0009408D File Offset: 0x0009228D
		internal void ReleaseWriteRecord(BamlRecord record)
		{
			if (!record.IsPinned)
			{
				if (this._writeCache[(int)record.RecordType] != null)
				{
					throw new InvalidOperationException(SR.Get("ParserMultiBamls"));
				}
				this._writeCache[(int)record.RecordType] = record;
			}
		}

		// Token: 0x040014F5 RID: 5365
		private BamlRecord[] _readCache = new BamlRecord[57];

		// Token: 0x040014F6 RID: 5366
		private BamlRecord[] _writeCache;
	}
}
