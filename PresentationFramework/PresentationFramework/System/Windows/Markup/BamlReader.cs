using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x020001CE RID: 462
	internal class BamlReader
	{
		// Token: 0x06001D7D RID: 7549 RVA: 0x000897F0 File Offset: 0x000879F0
		public BamlReader(Stream bamlStream)
		{
			this._parserContext = new ParserContext();
			this._parserContext.XamlTypeMapper = XmlParserDefaults.DefaultMapper;
			this._bamlRecordReader = new BamlRecordReader(bamlStream, this._parserContext, false);
			this._readState = ReadState.Initial;
			this._bamlNodeType = BamlNodeType.None;
			this._prefixDictionary = new XmlnsDictionary();
			this._value = string.Empty;
			this._assemblyName = string.Empty;
			this._prefix = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._properties = new ArrayList();
			this._haveUnprocessedRecord = false;
			this._deferableContentBlockDepth = -1;
			this._nodeStack = new Stack();
			this._reverseXmlnsTable = new Dictionary<string, List<string>>();
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x000898D1 File Offset: 0x00087AD1
		public int PropertyCount
		{
			get
			{
				return this._properties.Count;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x000898DE File Offset: 0x00087ADE
		public bool HasProperties
		{
			get
			{
				return this.PropertyCount > 0;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x000898E9 File Offset: 0x00087AE9
		public int ConnectionId
		{
			get
			{
				return this._connectionId;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x000898F1 File Offset: 0x00087AF1
		public BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000898F9 File Offset: 0x00087AF9
		public BamlNodeType NodeType
		{
			get
			{
				return this._bamlNodeType;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001D83 RID: 7555 RVA: 0x00089901 File Offset: 0x00087B01
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x00089909 File Offset: 0x00087B09
		public string LocalName
		{
			get
			{
				return this._localName;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x00089911 File Offset: 0x00087B11
		public string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x00089919 File Offset: 0x00087B19
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x00089921 File Offset: 0x00087B21
		public string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00089929 File Offset: 0x00087B29
		public string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x00089931 File Offset: 0x00087B31
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x00089939 File Offset: 0x00087B39
		public bool IsInjected
		{
			get
			{
				return this._isInjected;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x00089941 File Offset: 0x00087B41
		public bool CreateUsingTypeConverter
		{
			get
			{
				return this._useTypeConverter;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x00089949 File Offset: 0x00087B49
		public string TypeConverterName
		{
			get
			{
				return this._typeConverterName;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x00089951 File Offset: 0x00087B51
		public string TypeConverterAssemblyName
		{
			get
			{
				return this._typeConverterAssemblyName;
			}
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x00089959 File Offset: 0x00087B59
		public bool Read()
		{
			if (this._readState == ReadState.EndOfFile || this._readState == ReadState.Closed)
			{
				throw new InvalidOperationException(SR.Get("BamlReaderClosed"));
			}
			this.ReadNextRecord();
			return this._readState != ReadState.EndOfFile;
		}

		// Token: 0x170006F8 RID: 1784
		// (set) Token: 0x06001D8F RID: 7567 RVA: 0x0008998F File Offset: 0x00087B8F
		private BamlNodeType NodeTypeInternal
		{
			set
			{
				this._bamlNodeType = value;
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x00089998 File Offset: 0x00087B98
		private void AddToPropertyInfoCollection(object info)
		{
			this._properties.Add(info);
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000899A7 File Offset: 0x00087BA7
		public void Close()
		{
			if (this._readState != ReadState.Closed)
			{
				this._bamlRecordReader.Close();
				this._currentBamlRecord = null;
				this._bamlRecordReader = null;
				this._readState = ReadState.Closed;
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000899D2 File Offset: 0x00087BD2
		public bool MoveToFirstProperty()
		{
			if (this.HasProperties)
			{
				this._propertiesIndex = -1;
				return this.MoveToNextProperty();
			}
			return false;
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000899EC File Offset: 0x00087BEC
		public bool MoveToNextProperty()
		{
			if (this._propertiesIndex >= this._properties.Count - 1)
			{
				return false;
			}
			this._propertiesIndex++;
			object obj = this._properties[this._propertiesIndex];
			BamlReader.BamlPropertyInfo bamlPropertyInfo = obj as BamlReader.BamlPropertyInfo;
			if (bamlPropertyInfo != null)
			{
				this._name = bamlPropertyInfo.Name;
				this._localName = bamlPropertyInfo.LocalName;
				int num = bamlPropertyInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
				if (num > 0)
				{
					this._ownerTypeName = bamlPropertyInfo.Name.Substring(0, num);
				}
				else
				{
					this._ownerTypeName = string.Empty;
				}
				this._value = bamlPropertyInfo.Value;
				this._assemblyName = bamlPropertyInfo.AssemblyName;
				this._prefix = bamlPropertyInfo.Prefix;
				this._xmlNamespace = bamlPropertyInfo.XmlNamespace;
				this._clrNamespace = bamlPropertyInfo.ClrNamespace;
				this._connectionId = 0;
				this._contentPropertyName = string.Empty;
				this._attributeUsage = bamlPropertyInfo.AttributeUsage;
				if (bamlPropertyInfo.RecordType == BamlRecordType.XmlnsProperty)
				{
					this.NodeTypeInternal = BamlNodeType.XmlnsProperty;
				}
				else if (bamlPropertyInfo.RecordType == BamlRecordType.DefAttribute)
				{
					this.NodeTypeInternal = BamlNodeType.DefAttribute;
				}
				else if (bamlPropertyInfo.RecordType == BamlRecordType.PresentationOptionsAttribute)
				{
					this.NodeTypeInternal = BamlNodeType.PresentationOptionsAttribute;
				}
				else
				{
					this.NodeTypeInternal = BamlNodeType.Property;
				}
				return true;
			}
			BamlReader.BamlContentPropertyInfo bamlContentPropertyInfo = obj as BamlReader.BamlContentPropertyInfo;
			if (bamlContentPropertyInfo != null)
			{
				this._contentPropertyName = bamlContentPropertyInfo.LocalName;
				this._connectionId = 0;
				this._prefix = string.Empty;
				this._name = bamlContentPropertyInfo.Name;
				int num2 = bamlContentPropertyInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
				if (num2 > 0)
				{
					this._ownerTypeName = bamlContentPropertyInfo.Name.Substring(0, num2);
				}
				this._localName = bamlContentPropertyInfo.LocalName;
				this._ownerTypeName = string.Empty;
				this._assemblyName = bamlContentPropertyInfo.AssemblyName;
				this._xmlNamespace = string.Empty;
				this._clrNamespace = string.Empty;
				this._attributeUsage = BamlAttributeUsage.Default;
				this._value = bamlContentPropertyInfo.LocalName;
				this.NodeTypeInternal = BamlNodeType.ContentProperty;
				return true;
			}
			this._connectionId = (int)obj;
			this._contentPropertyName = string.Empty;
			this._prefix = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._assemblyName = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._attributeUsage = BamlAttributeUsage.Default;
			this._value = this._connectionId.ToString(CultureInfo.CurrentCulture);
			this.NodeTypeInternal = BamlNodeType.ConnectionId;
			return true;
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x00089C70 File Offset: 0x00087E70
		private void GetNextRecord()
		{
			if (this._currentStaticResourceRecords != null)
			{
				List<BamlRecord> currentStaticResourceRecords = this._currentStaticResourceRecords;
				int currentStaticResourceRecordIndex = this._currentStaticResourceRecordIndex;
				this._currentStaticResourceRecordIndex = currentStaticResourceRecordIndex + 1;
				this._currentBamlRecord = currentStaticResourceRecords[currentStaticResourceRecordIndex];
				if (this._currentStaticResourceRecordIndex == this._currentStaticResourceRecords.Count)
				{
					this._currentStaticResourceRecords = null;
					this._currentStaticResourceRecordIndex = -1;
					return;
				}
			}
			else
			{
				this._currentBamlRecord = this._bamlRecordReader.GetNextRecord();
			}
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00089CDC File Offset: 0x00087EDC
		private void ReadNextRecord()
		{
			if (this._readState == ReadState.Initial)
			{
				this._bamlRecordReader.ReadVersionHeader();
			}
			bool flag = true;
			while (flag)
			{
				if (this._haveUnprocessedRecord)
				{
					this._haveUnprocessedRecord = false;
				}
				else
				{
					this.GetNextRecord();
				}
				if (this._currentBamlRecord == null)
				{
					this.NodeTypeInternal = BamlNodeType.None;
					this._readState = ReadState.EndOfFile;
					this.ClearProperties();
					return;
				}
				this._readState = ReadState.Interactive;
				flag = false;
				switch (this._currentBamlRecord.RecordType)
				{
				case BamlRecordType.DocumentStart:
					this.ReadDocumentStartRecord();
					continue;
				case BamlRecordType.DocumentEnd:
					this.ReadDocumentEndRecord();
					continue;
				case BamlRecordType.ElementStart:
				case BamlRecordType.StaticResourceStart:
					this.ReadElementStartRecord();
					continue;
				case BamlRecordType.ElementEnd:
				case BamlRecordType.StaticResourceEnd:
					this.ReadElementEndRecord();
					continue;
				case BamlRecordType.PropertyComplexStart:
				case BamlRecordType.PropertyArrayStart:
				case BamlRecordType.PropertyIListStart:
				case BamlRecordType.PropertyIDictionaryStart:
					this.ReadPropertyComplexStartRecord();
					continue;
				case BamlRecordType.PropertyComplexEnd:
				case BamlRecordType.PropertyArrayEnd:
				case BamlRecordType.PropertyIListEnd:
				case BamlRecordType.PropertyIDictionaryEnd:
					this.ReadPropertyComplexEndRecord();
					continue;
				case BamlRecordType.LiteralContent:
					this.ReadLiteralContentRecord();
					continue;
				case BamlRecordType.Text:
				case BamlRecordType.TextWithConverter:
				case BamlRecordType.TextWithId:
					this.ReadTextRecord();
					continue;
				case BamlRecordType.PIMapping:
					this.ReadPIMappingRecord();
					continue;
				case BamlRecordType.AssemblyInfo:
					this.ReadAssemblyInfoRecord();
					flag = true;
					continue;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
					flag = true;
					continue;
				case BamlRecordType.DeferableContentStart:
					this.ReadDeferableContentRecord();
					flag = true;
					continue;
				case BamlRecordType.ConstructorParametersStart:
					this.ReadConstructorStart();
					continue;
				case BamlRecordType.ConstructorParametersEnd:
					this.ReadConstructorEnd();
					continue;
				case BamlRecordType.ConnectionId:
					this.ReadConnectionIdRecord();
					continue;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord();
					flag = true;
					continue;
				case BamlRecordType.StaticResourceId:
					this.ReadStaticResourceId();
					flag = true;
					continue;
				}
				throw new InvalidOperationException(SR.Get("ParserUnknownBaml", new object[]
				{
					((int)this._currentBamlRecord.RecordType).ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00089F4D File Offset: 0x0008814D
		private void ReadProperties()
		{
			while (!this._haveUnprocessedRecord)
			{
				this.GetNextRecord();
				this.ProcessPropertyRecord();
			}
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x00089F68 File Offset: 0x00088168
		private void ProcessPropertyRecord()
		{
			BamlRecordType recordType = this._currentBamlRecord.RecordType;
			if (recordType <= BamlRecordType.PropertyCustom)
			{
				if (recordType != BamlRecordType.Property)
				{
					if (recordType != BamlRecordType.PropertyCustom)
					{
						goto IL_182;
					}
					this.ReadPropertyCustomRecord();
					return;
				}
			}
			else
			{
				switch (recordType)
				{
				case BamlRecordType.RoutedEvent:
					this.ReadRoutedEventRecord();
					return;
				case BamlRecordType.ClrEvent:
					this.ReadClrEventRecord();
					return;
				case BamlRecordType.XmlnsProperty:
					this.ReadXmlnsPropertyRecord();
					return;
				case BamlRecordType.XmlAttribute:
				case BamlRecordType.ProcessingInstruction:
				case BamlRecordType.Comment:
				case BamlRecordType.DefTag:
				case BamlRecordType.EndAttributes:
				case BamlRecordType.PIMapping:
				case BamlRecordType.DeferableContentStart:
				case BamlRecordType.DefAttributeKeyString:
				case BamlRecordType.KeyElementEnd:
				case BamlRecordType.ConstructorParametersStart:
				case BamlRecordType.ConstructorParametersEnd:
				case BamlRecordType.ConstructorParameterType:
				case BamlRecordType.NamedElementStart:
				case BamlRecordType.StaticResourceStart:
				case BamlRecordType.StaticResourceEnd:
				case BamlRecordType.StaticResourceId:
				case BamlRecordType.TextWithId:
					goto IL_182;
				case BamlRecordType.DefAttribute:
					this.ReadDefAttributeRecord();
					return;
				case BamlRecordType.AssemblyInfo:
					this.ReadAssemblyInfoRecord();
					return;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
					return;
				case BamlRecordType.PropertyStringReference:
					this.ReadPropertyStringRecord();
					return;
				case BamlRecordType.PropertyTypeReference:
					this.ReadPropertyTypeRecord();
					return;
				case BamlRecordType.PropertyWithExtension:
					this.ReadPropertyWithExtensionRecord();
					return;
				case BamlRecordType.PropertyWithConverter:
					break;
				case BamlRecordType.DefAttributeKeyType:
					this.ReadDefAttributeKeyTypeRecord();
					return;
				case BamlRecordType.KeyElementStart:
				{
					BamlReader.BamlKeyInfo info = this.ProcessKeyTree();
					this.AddToPropertyInfoCollection(info);
					return;
				}
				case BamlRecordType.ConnectionId:
					this.ReadConnectionIdRecord();
					return;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord();
					return;
				case BamlRecordType.PresentationOptionsAttribute:
					this.ReadPresentationOptionsAttributeRecord();
					return;
				default:
					if (recordType != BamlRecordType.PropertyWithStaticResourceId)
					{
						goto IL_182;
					}
					this.ReadPropertyWithStaticResourceIdRecord();
					return;
				}
			}
			this.ReadPropertyRecord();
			return;
			IL_182:
			this._haveUnprocessedRecord = true;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0008A100 File Offset: 0x00088300
		private void ReadXmlnsPropertyRecord()
		{
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)this._currentBamlRecord;
			this._parserContext.XmlnsDictionary[bamlXmlnsPropertyRecord.Prefix] = bamlXmlnsPropertyRecord.XmlNamespace;
			this._prefixDictionary[bamlXmlnsPropertyRecord.XmlNamespace] = bamlXmlnsPropertyRecord.Prefix;
			this.AddToPropertyInfoCollection(new BamlReader.BamlPropertyInfo
			{
				Value = bamlXmlnsPropertyRecord.XmlNamespace,
				XmlNamespace = string.Empty,
				ClrNamespace = string.Empty,
				AssemblyName = string.Empty,
				Prefix = "xmlns",
				LocalName = ((bamlXmlnsPropertyRecord.Prefix == null) ? string.Empty : bamlXmlnsPropertyRecord.Prefix),
				Name = ((bamlXmlnsPropertyRecord.Prefix == null || bamlXmlnsPropertyRecord.Prefix == string.Empty) ? "xmlns" : ("xmlns:" + bamlXmlnsPropertyRecord.Prefix)),
				RecordType = BamlRecordType.XmlnsProperty
			});
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0008A1EC File Offset: 0x000883EC
		private void ReadPropertyRecord()
		{
			string text = ((BamlPropertyRecord)this._currentBamlRecord).Value;
			text = MarkupExtensionParser.AddEscapeToLiteralString(text);
			this.AddToPropertyInfoCollection(this.ReadPropertyRecordCore(text));
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x0008A220 File Offset: 0x00088420
		private void ReadContentPropertyRecord()
		{
			BamlReader.BamlContentPropertyInfo bamlContentPropertyInfo = new BamlReader.BamlContentPropertyInfo();
			BamlContentPropertyRecord bamlContentPropertyRecord = (BamlContentPropertyRecord)this._currentBamlRecord;
			this.SetCommonPropertyInfo(bamlContentPropertyInfo, bamlContentPropertyRecord.AttributeId);
			bamlContentPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			this.AddToPropertyInfoCollection(bamlContentPropertyInfo);
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x0008A268 File Offset: 0x00088468
		private void ReadPropertyStringRecord()
		{
			string stringFromStringId = this.MapTable.GetStringFromStringId((int)((BamlPropertyStringReferenceRecord)this._currentBamlRecord).StringId);
			this.AddToPropertyInfoCollection(this.ReadPropertyRecordCore(stringFromStringId));
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0008A2A0 File Offset: 0x000884A0
		private void ReadPropertyTypeRecord()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = this.GetTypeValueString(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).TypeId);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x0008A308 File Offset: 0x00088508
		private void ReadPropertyWithExtensionRecord()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyWithExtensionRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = this.GetExtensionValueString((IOptimizedMarkupExtension)this._currentBamlRecord);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0008A36C File Offset: 0x0008856C
		private void ReadPropertyWithStaticResourceIdRecord()
		{
			BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord = (BamlPropertyWithStaticResourceIdRecord)this._currentBamlRecord;
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, bamlPropertyWithStaticResourceIdRecord.AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			BamlOptimizedStaticResourceRecord optimizedMarkupExtensionRecord = (BamlOptimizedStaticResourceRecord)this._currentKeyInfo.StaticResources[(int)bamlPropertyWithStaticResourceIdRecord.StaticResourceId][0];
			bamlPropertyInfo.Value = this.GetExtensionValueString(optimizedMarkupExtensionRecord);
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0008A3E8 File Offset: 0x000885E8
		private BamlReader.BamlPropertyInfo ReadPropertyRecordCore(string value)
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.Value = value;
			return bamlPropertyInfo;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0008A42C File Offset: 0x0008862C
		private void ReadPropertyCustomRecord()
		{
			BamlReader.BamlPropertyInfo propertyCustomRecordInfo = this.GetPropertyCustomRecordInfo();
			this.AddToPropertyInfoCollection(propertyCustomRecordInfo);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0008A448 File Offset: 0x00088648
		private BamlReader.BamlPropertyInfo GetPropertyCustomRecordInfo()
		{
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			BamlAttributeInfoRecord bamlAttributeInfoRecord = this.SetCommonPropertyInfo(bamlPropertyInfo, ((BamlPropertyCustomRecord)this._currentBamlRecord).AttributeId);
			bamlPropertyInfo.RecordType = this._currentBamlRecord.RecordType;
			bamlPropertyInfo.AttributeUsage = BamlAttributeUsage.Default;
			BamlPropertyCustomRecord bamlPropertyCustomRecord = (BamlPropertyCustomRecord)this._currentBamlRecord;
			if (bamlAttributeInfoRecord.DP == null && bamlAttributeInfoRecord.PropInfo == null)
			{
				bamlAttributeInfoRecord.DP = this.MapTable.GetDependencyProperty(bamlAttributeInfoRecord);
				if (bamlAttributeInfoRecord.OwnerType == null)
				{
					throw new InvalidOperationException(SR.Get("BamlReaderNoOwnerType", new object[]
					{
						bamlAttributeInfoRecord.Name,
						this.AssemblyName
					}));
				}
				if (bamlAttributeInfoRecord.DP == null)
				{
					try
					{
						bamlAttributeInfoRecord.PropInfo = bamlAttributeInfoRecord.OwnerType.GetProperty(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public);
					}
					catch (AmbiguousMatchException)
					{
						PropertyInfo[] properties = bamlAttributeInfoRecord.OwnerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
						for (int i = 0; i < properties.Length; i++)
						{
							if (properties[i].Name == bamlAttributeInfoRecord.Name)
							{
								bamlAttributeInfoRecord.PropInfo = properties[i];
								break;
							}
						}
					}
					if (bamlAttributeInfoRecord.PropInfo == null)
					{
						throw new InvalidOperationException(SR.Get("ParserCantGetDPOrPi", new object[]
						{
							bamlPropertyInfo.Name
						}));
					}
				}
			}
			Type propertyType = bamlAttributeInfoRecord.GetPropertyType();
			string name = bamlAttributeInfoRecord.Name;
			short serializerTypeId = bamlPropertyCustomRecord.SerializerTypeId;
			if (serializerTypeId == 137)
			{
				Type type = null;
				this._propertyDP = this._bamlRecordReader.GetCustomDependencyPropertyValue(bamlPropertyCustomRecord, out type);
				type = ((type == null) ? this._propertyDP.OwnerType : type);
				bamlPropertyInfo.Value = type.Name + "." + this._propertyDP.Name;
				string xmlNamespace = this._parserContext.XamlTypeMapper.GetXmlNamespace(type.Namespace, type.Assembly.FullName);
				string xmlnsPrefix = this.GetXmlnsPrefix(xmlNamespace);
				if (xmlnsPrefix != string.Empty)
				{
					bamlPropertyInfo.Value = xmlnsPrefix + ":" + bamlPropertyInfo.Value;
				}
				if (!this._propertyDP.PropertyType.IsEnum)
				{
					this._propertyDP = null;
				}
			}
			else
			{
				if (this._propertyDP != null)
				{
					propertyType = this._propertyDP.PropertyType;
					name = this._propertyDP.Name;
					this._propertyDP = null;
				}
				object customValue = this._bamlRecordReader.GetCustomValue(bamlPropertyCustomRecord, propertyType, name);
				TypeConverter converter = TypeDescriptor.GetConverter(customValue.GetType());
				bamlPropertyInfo.Value = converter.ConvertToString(null, TypeConverterHelper.InvariantEnglishUS, customValue);
			}
			return bamlPropertyInfo;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0008A6EC File Offset: 0x000888EC
		private void ReadDefAttributeRecord()
		{
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)this._currentBamlRecord;
			bamlDefAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlDefAttributeRecord.NameId);
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = bamlDefAttributeRecord.Value;
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = bamlDefAttributeRecord.Name;
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0008A78C File Offset: 0x0008898C
		private void ReadPresentationOptionsAttributeRecord()
		{
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)this._currentBamlRecord;
			bamlPresentationOptionsAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlPresentationOptionsAttributeRecord.NameId);
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = bamlPresentationOptionsAttributeRecord.Value;
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation/options"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = bamlPresentationOptionsAttributeRecord.Name;
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.PresentationOptionsAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0008A82C File Offset: 0x00088A2C
		private void ReadDefAttributeKeyTypeRecord()
		{
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = (BamlDefAttributeKeyTypeRecord)this._currentBamlRecord;
			BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
			bamlPropertyInfo.Value = this.GetTypeValueString(bamlDefAttributeKeyTypeRecord.TypeId);
			bamlPropertyInfo.AssemblyName = string.Empty;
			bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlPropertyInfo.ClrNamespace = string.Empty;
			bamlPropertyInfo.Name = "Key";
			bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
			bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
			this.AddToPropertyInfoCollection(bamlPropertyInfo);
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0008A8BA File Offset: 0x00088ABA
		private void ReadDeferableContentRecord()
		{
			this._deferableContentBlockDepth = this._nodeStack.Count;
			this._deferableContentPosition = this.ReadDeferKeys();
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0008A8DC File Offset: 0x00088ADC
		private long ReadDeferKeys()
		{
			long result = -1L;
			this._deferKeys = new List<BamlReader.BamlKeyInfo>();
			while (!this._haveUnprocessedRecord)
			{
				this.GetNextRecord();
				this.ProcessDeferKey();
				if (!this._haveUnprocessedRecord)
				{
					result = this._bamlRecordReader.StreamPosition;
				}
			}
			return result;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0008A924 File Offset: 0x00088B24
		private void ProcessDeferKey()
		{
			BamlRecordType recordType = this._currentBamlRecord.RecordType;
			switch (recordType)
			{
			case BamlRecordType.DefAttributeKeyString:
			{
				BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = this._currentBamlRecord as BamlDefAttributeKeyStringRecord;
				if (bamlDefAttributeKeyStringRecord != null)
				{
					BamlReader.BamlKeyInfo bamlKeyInfo = this.CheckForSharedness();
					if (bamlKeyInfo != null)
					{
						this._deferKeys.Add(bamlKeyInfo);
					}
					bamlDefAttributeKeyStringRecord.Value = this.MapTable.GetStringFromStringId((int)bamlDefAttributeKeyStringRecord.ValueId);
					bamlKeyInfo = new BamlReader.BamlKeyInfo();
					bamlKeyInfo.Value = bamlDefAttributeKeyStringRecord.Value;
					bamlKeyInfo.AssemblyName = string.Empty;
					bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
					bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
					bamlKeyInfo.ClrNamespace = string.Empty;
					bamlKeyInfo.Name = "Key";
					bamlKeyInfo.LocalName = bamlKeyInfo.Name;
					bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
					bamlKeyInfo.Offset = ((IBamlDictionaryKey)bamlDefAttributeKeyStringRecord).ValuePosition;
					this._deferKeys.Add(bamlKeyInfo);
					return;
				}
				break;
			}
			case BamlRecordType.DefAttributeKeyType:
			{
				BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = this._currentBamlRecord as BamlDefAttributeKeyTypeRecord;
				if (bamlDefAttributeKeyTypeRecord != null)
				{
					string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
					string text2;
					if (text != string.Empty)
					{
						text2 = "{" + text + ":Type ";
					}
					else
					{
						text2 = "{Type ";
					}
					BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlDefAttributeKeyTypeRecord.TypeId);
					string text3 = typeInfoFromId.TypeFullName;
					text3 = text3.Substring(text3.LastIndexOf(".", StringComparison.Ordinal) + 1);
					string text4;
					string text5;
					string text6;
					this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text4, out text5, out text6);
					if (text5 != string.Empty)
					{
						text3 = string.Concat(new string[]
						{
							text2,
							text5,
							":",
							text3,
							"}"
						});
					}
					else
					{
						text3 = text2 + text3 + "}";
					}
					BamlReader.BamlKeyInfo bamlKeyInfo2 = new BamlReader.BamlKeyInfo();
					bamlKeyInfo2.Value = text3;
					bamlKeyInfo2.AssemblyName = string.Empty;
					bamlKeyInfo2.Prefix = text;
					bamlKeyInfo2.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
					bamlKeyInfo2.ClrNamespace = string.Empty;
					bamlKeyInfo2.Name = "Key";
					bamlKeyInfo2.LocalName = bamlKeyInfo2.Name;
					bamlKeyInfo2.RecordType = BamlRecordType.DefAttribute;
					bamlKeyInfo2.Offset = ((IBamlDictionaryKey)bamlDefAttributeKeyTypeRecord).ValuePosition;
					this._deferKeys.Add(bamlKeyInfo2);
					return;
				}
				break;
			}
			case BamlRecordType.KeyElementStart:
			{
				BamlReader.BamlKeyInfo bamlKeyInfo3 = this.CheckForSharedness();
				if (bamlKeyInfo3 != null)
				{
					this._deferKeys.Add(bamlKeyInfo3);
				}
				bamlKeyInfo3 = this.ProcessKeyTree();
				this._deferKeys.Add(bamlKeyInfo3);
				return;
			}
			default:
				if (recordType == BamlRecordType.StaticResourceStart || recordType == BamlRecordType.OptimizedStaticResource)
				{
					List<BamlRecord> list = new List<BamlRecord>();
					this._currentBamlRecord.Pin();
					list.Add(this._currentBamlRecord);
					if (this._currentBamlRecord.RecordType == BamlRecordType.StaticResourceStart)
					{
						this.ProcessStaticResourceTree(list);
					}
					BamlReader.BamlKeyInfo bamlKeyInfo4 = this._deferKeys[this._deferKeys.Count - 1];
					bamlKeyInfo4.StaticResources.Add(list);
					return;
				}
				this._haveUnprocessedRecord = true;
				break;
			}
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0008AC14 File Offset: 0x00088E14
		private BamlReader.BamlKeyInfo CheckForSharedness()
		{
			IBamlDictionaryKey bamlDictionaryKey = (IBamlDictionaryKey)this._currentBamlRecord;
			if (!bamlDictionaryKey.SharedSet)
			{
				return null;
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = new BamlReader.BamlKeyInfo();
			bamlKeyInfo.Value = bamlDictionaryKey.Shared.ToString();
			bamlKeyInfo.AssemblyName = string.Empty;
			bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlKeyInfo.ClrNamespace = string.Empty;
			bamlKeyInfo.Name = "Shared";
			bamlKeyInfo.LocalName = bamlKeyInfo.Name;
			bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
			bamlKeyInfo.Offset = bamlDictionaryKey.ValuePosition;
			return bamlKeyInfo;
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0008ACB4 File Offset: 0x00088EB4
		private BamlReader.BamlKeyInfo ProcessKeyTree()
		{
			BamlKeyElementStartRecord bamlKeyElementStartRecord = this._currentBamlRecord as BamlKeyElementStartRecord;
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlKeyElementStartRecord.TypeId);
			string text = typeInfoFromId.TypeFullName;
			text = text.Substring(text.LastIndexOf(".", StringComparison.Ordinal) + 1);
			string text2;
			string text3;
			string text4;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text2, out text3, out text4);
			if (text3 != string.Empty)
			{
				text = string.Concat(new string[]
				{
					"{",
					text3,
					":",
					text,
					" "
				});
			}
			else
			{
				text = "{" + text + " ";
			}
			bool flag = true;
			Stack stack = new Stack();
			Stack stack2 = new Stack();
			Stack stack3 = new Stack();
			stack.Push(false);
			stack2.Push(false);
			stack3.Push(false);
			while (flag)
			{
				if (!this._haveUnprocessedRecord)
				{
					this.GetNextRecord();
				}
				else
				{
					this._haveUnprocessedRecord = false;
				}
				BamlRecordType recordType = this._currentBamlRecord.RecordType;
				switch (recordType)
				{
				case BamlRecordType.ElementStart:
				{
					if ((bool)stack3.Peek())
					{
						text += ", ";
					}
					if ((bool)stack2.Peek())
					{
						stack3.Pop();
						stack3.Push(true);
					}
					stack.Push(false);
					stack2.Push(false);
					stack3.Push(false);
					BamlElementStartRecord bamlElementStartRecord = this._currentBamlRecord as BamlElementStartRecord;
					BamlTypeInfoRecord typeInfoFromId2 = this.MapTable.GetTypeInfoFromId(bamlElementStartRecord.TypeId);
					string text5 = typeInfoFromId2.TypeFullName;
					text5 = text5.Substring(text5.LastIndexOf(".", StringComparison.Ordinal) + 1);
					this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId2, out text2, out text3, out text4);
					if (text3 != string.Empty)
					{
						text = string.Concat(new string[]
						{
							text,
							"{",
							text3,
							":",
							text5,
							" "
						});
						continue;
					}
					text = "{" + text5 + " ";
					continue;
				}
				case BamlRecordType.ElementEnd:
					stack.Pop();
					stack2.Pop();
					stack3.Pop();
					text += "}";
					continue;
				case BamlRecordType.Property:
					break;
				case BamlRecordType.PropertyCustom:
				{
					BamlReader.BamlPropertyInfo propertyCustomRecordInfo = this.GetPropertyCustomRecordInfo();
					if ((bool)stack.Pop())
					{
						text += ", ";
					}
					text = text + propertyCustomRecordInfo.LocalName + "=" + propertyCustomRecordInfo.Value;
					stack.Push(true);
					continue;
				}
				case BamlRecordType.PropertyComplexStart:
				{
					this.ReadPropertyComplexStartRecord();
					BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
					if ((bool)stack.Pop())
					{
						text += ", ";
					}
					text = text + bamlNodeInfo.LocalName + "=";
					stack.Push(true);
					continue;
				}
				case BamlRecordType.PropertyComplexEnd:
					continue;
				default:
				{
					if (recordType != BamlRecordType.Text)
					{
						switch (recordType)
						{
						case BamlRecordType.AssemblyInfo:
							this.ReadAssemblyInfoRecord();
							continue;
						case BamlRecordType.TypeInfo:
						case BamlRecordType.TypeSerializerInfo:
							this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.AttributeInfo:
							this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.StringInfo:
							this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)this._currentBamlRecord);
							continue;
						case BamlRecordType.PropertyStringReference:
						{
							string stringFromStringId = this.MapTable.GetStringFromStringId((int)((BamlPropertyStringReferenceRecord)this._currentBamlRecord).StringId);
							BamlReader.BamlPropertyInfo bamlPropertyInfo = this.ReadPropertyRecordCore(stringFromStringId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + bamlPropertyInfo.LocalName + "=" + bamlPropertyInfo.Value;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyTypeReference:
						{
							string typeValueString = this.GetTypeValueString(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).TypeId);
							string attributeNameFromId = this.MapTable.GetAttributeNameFromId(((BamlPropertyTypeReferenceRecord)this._currentBamlRecord).AttributeId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + attributeNameFromId + "=" + typeValueString;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyWithExtension:
						{
							string extensionValueString = this.GetExtensionValueString((BamlPropertyWithExtensionRecord)this._currentBamlRecord);
							string attributeNameFromId2 = this.MapTable.GetAttributeNameFromId(((BamlPropertyWithExtensionRecord)this._currentBamlRecord).AttributeId);
							if ((bool)stack.Pop())
							{
								text += ", ";
							}
							text = text + attributeNameFromId2 + "=" + extensionValueString;
							stack.Push(true);
							continue;
						}
						case BamlRecordType.PropertyWithConverter:
							goto IL_4BE;
						case BamlRecordType.KeyElementEnd:
							text += "}";
							flag = false;
							this._haveUnprocessedRecord = false;
							continue;
						case BamlRecordType.ConstructorParametersStart:
							stack2.Pop();
							stack2.Push(true);
							continue;
						case BamlRecordType.ConstructorParametersEnd:
							stack2.Pop();
							stack2.Push(false);
							stack3.Pop();
							stack3.Push(false);
							continue;
						case BamlRecordType.ConstructorParameterType:
						{
							if ((bool)stack3.Peek())
							{
								text += ", ";
							}
							if ((bool)stack2.Peek())
							{
								stack3.Pop();
								stack3.Push(true);
							}
							BamlConstructorParameterTypeRecord bamlConstructorParameterTypeRecord = this._currentBamlRecord as BamlConstructorParameterTypeRecord;
							text += this.GetTypeValueString(bamlConstructorParameterTypeRecord.TypeId);
							continue;
						}
						case BamlRecordType.TextWithId:
							goto IL_249;
						}
						throw new InvalidOperationException(SR.Get("ParserUnknownBaml", new object[]
						{
							((int)this._currentBamlRecord.RecordType).ToString(CultureInfo.CurrentCulture)
						}));
					}
					IL_249:
					BamlTextWithIdRecord bamlTextWithIdRecord = this._currentBamlRecord as BamlTextWithIdRecord;
					if (bamlTextWithIdRecord != null)
					{
						bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
					}
					string str = this.EscapeString(((BamlTextRecord)this._currentBamlRecord).Value);
					if ((bool)stack3.Peek())
					{
						text += ", ";
					}
					text += str;
					if ((bool)stack2.Peek())
					{
						stack3.Pop();
						stack3.Push(true);
						continue;
					}
					continue;
				}
				}
				IL_4BE:
				string value = ((BamlPropertyRecord)this._currentBamlRecord).Value;
				BamlReader.BamlPropertyInfo bamlPropertyInfo2 = this.ReadPropertyRecordCore(value);
				if ((bool)stack.Pop())
				{
					text += ", ";
				}
				text = text + bamlPropertyInfo2.LocalName + "=" + bamlPropertyInfo2.Value;
				stack.Push(true);
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = new BamlReader.BamlKeyInfo();
			bamlKeyInfo.Value = text;
			bamlKeyInfo.AssemblyName = string.Empty;
			bamlKeyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			bamlKeyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
			bamlKeyInfo.ClrNamespace = string.Empty;
			bamlKeyInfo.Name = "Key";
			bamlKeyInfo.LocalName = bamlKeyInfo.Name;
			bamlKeyInfo.RecordType = BamlRecordType.DefAttribute;
			bamlKeyInfo.Offset = ((IBamlDictionaryKey)bamlKeyElementStartRecord).ValuePosition;
			return bamlKeyInfo;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0008B444 File Offset: 0x00089644
		private void ProcessStaticResourceTree(List<BamlRecord> srRecords)
		{
			bool flag = true;
			while (flag)
			{
				if (this._haveUnprocessedRecord)
				{
					this._haveUnprocessedRecord = false;
				}
				else
				{
					this.GetNextRecord();
				}
				this._currentBamlRecord.Pin();
				srRecords.Add(this._currentBamlRecord);
				if (this._currentBamlRecord.RecordType == BamlRecordType.StaticResourceEnd)
				{
					flag = false;
				}
			}
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0008B498 File Offset: 0x00089698
		private void ReadStaticResourceId()
		{
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this._currentBamlRecord;
			this._currentStaticResourceRecords = this._currentKeyInfo.StaticResources[(int)bamlStaticResourceIdRecord.StaticResourceId];
			this._currentStaticResourceRecordIndex = 0;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0008B4D4 File Offset: 0x000896D4
		private string EscapeString(string value)
		{
			StringBuilder stringBuilder = null;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '{' || value[i] == '}')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length + 2);
						stringBuilder.Append(value, 0, i);
					}
					stringBuilder.Append('\\');
				}
				if (stringBuilder != null)
				{
					stringBuilder.Append(value[i]);
				}
			}
			if (stringBuilder == null)
			{
				return value;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0008B54A File Offset: 0x0008974A
		private void ReadRoutedEventRecord()
		{
			throw new InvalidOperationException(SR.Get("ParserBamlEvent", new object[]
			{
				string.Empty
			}));
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0008B54A File Offset: 0x0008974A
		private void ReadClrEventRecord()
		{
			throw new InvalidOperationException(SR.Get("ParserBamlEvent", new object[]
			{
				string.Empty
			}));
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0008B56C File Offset: 0x0008976C
		private void ReadDocumentStartRecord()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.StartDocument;
			BamlDocumentStartRecord bamlDocumentStartRecord = (BamlDocumentStartRecord)this._currentBamlRecord;
			this._parserContext.IsDebugBamlStream = bamlDocumentStartRecord.DebugBaml;
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.RecordType = BamlRecordType.DocumentStart;
			this._nodeStack.Push(bamlNodeInfo);
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0008B5BC File Offset: 0x000897BC
		private void ReadDocumentEndRecord()
		{
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			if (bamlNodeInfo.RecordType != BamlRecordType.DocumentStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					bamlNodeInfo.RecordType.ToString(),
					"DocumentEnd"
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndDocument;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0008B628 File Offset: 0x00089828
		private void ReadAssemblyInfoRecord()
		{
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = (BamlAssemblyInfoRecord)this._currentBamlRecord;
			this.MapTable.LoadAssemblyInfoRecord(bamlAssemblyInfoRecord);
			Assembly assembly = Assembly.Load(bamlAssemblyInfoRecord.AssemblyFullName);
			foreach (XmlnsDefinitionAttribute xmlnsDefinitionAttribute in assembly.GetCustomAttributes(typeof(XmlnsDefinitionAttribute), true))
			{
				this.SetXmlNamespace(xmlnsDefinitionAttribute.ClrNamespace, assembly.FullName, xmlnsDefinitionAttribute.XmlNamespace);
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0008B6A0 File Offset: 0x000898A0
		private void ReadPIMappingRecord()
		{
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)this._currentBamlRecord;
			BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(bamlPIMappingRecord.AssemblyId);
			if (assemblyInfoFromId == null)
			{
				throw new InvalidOperationException(SR.Get("ParserMapPIMissingAssembly"));
			}
			if (!this._parserContext.XamlTypeMapper.PITable.Contains(bamlPIMappingRecord.XmlNamespace))
			{
				this._parserContext.XamlTypeMapper.AddMappingProcessingInstruction(bamlPIMappingRecord.XmlNamespace, bamlPIMappingRecord.ClrNamespace, assemblyInfoFromId.AssemblyFullName);
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.PIMapping;
			this._name = "Mapping";
			this._localName = this._name;
			this._ownerTypeName = string.Empty;
			this._xmlNamespace = bamlPIMappingRecord.XmlNamespace;
			this._clrNamespace = bamlPIMappingRecord.ClrNamespace;
			this._assemblyName = assemblyInfoFromId.AssemblyFullName;
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append("XmlNamespace=\"");
			stringBuilder.Append(this._xmlNamespace);
			stringBuilder.Append("\" ClrNamespace=\"");
			stringBuilder.Append(this._clrNamespace);
			stringBuilder.Append("\" Assembly=\"");
			stringBuilder.Append(this._assemblyName);
			stringBuilder.Append("\"");
			this._value = stringBuilder.ToString();
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0008B7DC File Offset: 0x000899DC
		private void ReadLiteralContentRecord()
		{
			this.ClearProperties();
			BamlLiteralContentRecord bamlLiteralContentRecord = (BamlLiteralContentRecord)this._currentBamlRecord;
			this.NodeTypeInternal = BamlNodeType.LiteralContent;
			this._value = bamlLiteralContentRecord.Value;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0008B810 File Offset: 0x00089A10
		private void ReadConnectionIdRecord()
		{
			BamlConnectionIdRecord bamlConnectionIdRecord = (BamlConnectionIdRecord)this._currentBamlRecord;
			this.AddToPropertyInfoCollection(bamlConnectionIdRecord.ConnectionId);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0008B83C File Offset: 0x00089A3C
		private void ReadElementStartRecord()
		{
			this.ClearProperties();
			this._propertyDP = null;
			this._parserContext.PushScope();
			this._prefixDictionary.PushScope();
			BamlElementStartRecord bamlElementStartRecord = (BamlElementStartRecord)this._currentBamlRecord;
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlElementStartRecord.TypeId);
			this.NodeTypeInternal = BamlNodeType.StartElement;
			this._name = typeInfoFromId.TypeFullName;
			this._localName = this._name.Substring(this._name.LastIndexOf(".", StringComparison.Ordinal) + 1);
			this._ownerTypeName = string.Empty;
			this._clrNamespace = typeInfoFromId.ClrNamespace;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out this._assemblyName, out this._prefix, out this._xmlNamespace);
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.Name = this._name;
			bamlNodeInfo.LocalName = this._localName;
			bamlNodeInfo.AssemblyName = this._assemblyName;
			bamlNodeInfo.Prefix = this._prefix;
			bamlNodeInfo.ClrNamespace = this._clrNamespace;
			bamlNodeInfo.XmlNamespace = this._xmlNamespace;
			bamlNodeInfo.RecordType = BamlRecordType.ElementStart;
			this._useTypeConverter = bamlElementStartRecord.CreateUsingTypeConverter;
			this._isInjected = bamlElementStartRecord.IsInjected;
			if (this._deferableContentBlockDepth == this._nodeStack.Count)
			{
				int num = (int)(this._bamlRecordReader.StreamPosition - this._deferableContentPosition);
				num -= bamlElementStartRecord.RecordSize + 1;
				if (BamlRecordHelper.HasDebugExtensionRecord(this._parserContext.IsDebugBamlStream, bamlElementStartRecord))
				{
					BamlRecord next = bamlElementStartRecord.Next;
					num -= next.RecordSize + 1;
				}
				this.InsertDeferedKey(num);
			}
			this._nodeStack.Push(bamlNodeInfo);
			this.ReadProperties();
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0008B9D0 File Offset: 0x00089BD0
		private void ReadElementEndRecord()
		{
			if (this._deferableContentBlockDepth == this._nodeStack.Count)
			{
				this._deferableContentBlockDepth = -1;
				this._deferableContentPosition = -1L;
			}
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			if (bamlNodeInfo.RecordType != BamlRecordType.ElementStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					BamlRecordType.ElementEnd.ToString()
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndElement;
			this._name = bamlNodeInfo.Name;
			this._localName = bamlNodeInfo.LocalName;
			this._ownerTypeName = string.Empty;
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._parserContext.PopScope();
			this._prefixDictionary.PopScope();
			this.ReadProperties();
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0008BADC File Offset: 0x00089CDC
		private void ReadPropertyComplexStartRecord()
		{
			this.ClearProperties();
			this._parserContext.PushScope();
			this._prefixDictionary.PushScope();
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			this.SetCommonPropertyInfo(bamlNodeInfo, ((BamlPropertyComplexStartRecord)this._currentBamlRecord).AttributeId);
			this.NodeTypeInternal = BamlNodeType.StartComplexProperty;
			this._localName = bamlNodeInfo.LocalName;
			int num = bamlNodeInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
			if (num > 0)
			{
				this._ownerTypeName = bamlNodeInfo.Name.Substring(0, num);
			}
			else
			{
				this._ownerTypeName = string.Empty;
			}
			this._name = bamlNodeInfo.Name;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			bamlNodeInfo.RecordType = this._currentBamlRecord.RecordType;
			this._nodeStack.Push(bamlNodeInfo);
			this.ReadProperties();
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0008BBCC File Offset: 0x00089DCC
		private void ReadPropertyComplexEndRecord()
		{
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			BamlRecordType bamlRecordType;
			switch (bamlNodeInfo.RecordType)
			{
			case BamlRecordType.PropertyComplexStart:
				bamlRecordType = BamlRecordType.PropertyComplexEnd;
				goto IL_53;
			case BamlRecordType.PropertyArrayStart:
				bamlRecordType = BamlRecordType.PropertyArrayEnd;
				goto IL_53;
			case BamlRecordType.PropertyIListStart:
				bamlRecordType = BamlRecordType.PropertyIListEnd;
				goto IL_53;
			case BamlRecordType.PropertyIDictionaryStart:
				bamlRecordType = BamlRecordType.PropertyIDictionaryEnd;
				goto IL_53;
			}
			bamlRecordType = BamlRecordType.Unknown;
			IL_53:
			if (this._currentBamlRecord.RecordType != bamlRecordType)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					bamlRecordType.ToString()
				}));
			}
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndComplexProperty;
			this._name = bamlNodeInfo.Name;
			this._localName = bamlNodeInfo.LocalName;
			int num = bamlNodeInfo.Name.LastIndexOf(".", StringComparison.Ordinal);
			if (num > 0)
			{
				this._ownerTypeName = bamlNodeInfo.Name.Substring(0, num);
			}
			else
			{
				this._ownerTypeName = string.Empty;
			}
			this._assemblyName = bamlNodeInfo.AssemblyName;
			this._prefix = bamlNodeInfo.Prefix;
			this._xmlNamespace = bamlNodeInfo.XmlNamespace;
			this._clrNamespace = bamlNodeInfo.ClrNamespace;
			this._parserContext.PopScope();
			this._prefixDictionary.PopScope();
			this.ReadProperties();
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0008BD24 File Offset: 0x00089F24
		private void ReadTextRecord()
		{
			this.ClearProperties();
			BamlTextWithIdRecord bamlTextWithIdRecord = this._currentBamlRecord as BamlTextWithIdRecord;
			if (bamlTextWithIdRecord != null)
			{
				bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
			}
			BamlTextWithConverterRecord bamlTextWithConverterRecord = this._currentBamlRecord as BamlTextWithConverterRecord;
			if (bamlTextWithConverterRecord != null)
			{
				short converterTypeId = bamlTextWithConverterRecord.ConverterTypeId;
				Type typeFromId = this.MapTable.GetTypeFromId(converterTypeId);
				this._typeConverterAssemblyName = typeFromId.Assembly.FullName;
				this._typeConverterName = typeFromId.FullName;
			}
			this.NodeTypeInternal = BamlNodeType.Text;
			this._prefix = string.Empty;
			this._value = ((BamlTextRecord)this._currentBamlRecord).Value;
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0008BDC8 File Offset: 0x00089FC8
		private void ReadConstructorStart()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.StartConstructor;
			BamlReader.BamlNodeInfo bamlNodeInfo = new BamlReader.BamlNodeInfo();
			bamlNodeInfo.RecordType = BamlRecordType.ConstructorParametersStart;
			this._nodeStack.Push(bamlNodeInfo);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0008BE00 File Offset: 0x0008A000
		private void ReadConstructorEnd()
		{
			this.ClearProperties();
			this.NodeTypeInternal = BamlNodeType.EndConstructor;
			BamlReader.BamlNodeInfo bamlNodeInfo = (BamlReader.BamlNodeInfo)this._nodeStack.Pop();
			if (bamlNodeInfo.RecordType != BamlRecordType.ConstructorParametersStart)
			{
				throw new InvalidOperationException(SR.Get("BamlScopeError", new object[]
				{
					this._currentBamlRecord.RecordType.ToString(),
					BamlRecordType.ConstructorParametersEnd.ToString()
				}));
			}
			this.ReadProperties();
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0008BE84 File Offset: 0x0008A084
		private void InsertDeferedKey(int valueOffset)
		{
			if (this._deferKeys == null)
			{
				return;
			}
			BamlReader.BamlKeyInfo bamlKeyInfo = this._deferKeys[0];
			while (bamlKeyInfo.Offset == valueOffset)
			{
				this._currentKeyInfo = bamlKeyInfo;
				BamlReader.BamlPropertyInfo bamlPropertyInfo = new BamlReader.BamlPropertyInfo();
				bamlPropertyInfo.Value = bamlKeyInfo.Value;
				bamlPropertyInfo.AssemblyName = string.Empty;
				bamlPropertyInfo.Prefix = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
				bamlPropertyInfo.XmlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
				bamlPropertyInfo.ClrNamespace = string.Empty;
				bamlPropertyInfo.Name = bamlKeyInfo.Name;
				bamlPropertyInfo.LocalName = bamlPropertyInfo.Name;
				bamlPropertyInfo.RecordType = BamlRecordType.DefAttribute;
				this.AddToPropertyInfoCollection(bamlPropertyInfo);
				this._deferKeys.RemoveAt(0);
				if (this._deferKeys.Count <= 0)
				{
					return;
				}
				bamlKeyInfo = this._deferKeys[0];
			}
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0008BF5C File Offset: 0x0008A15C
		private void ClearProperties()
		{
			this._value = string.Empty;
			this._prefix = string.Empty;
			this._name = string.Empty;
			this._localName = string.Empty;
			this._ownerTypeName = string.Empty;
			this._assemblyName = string.Empty;
			this._xmlNamespace = string.Empty;
			this._clrNamespace = string.Empty;
			this._connectionId = 0;
			this._contentPropertyName = string.Empty;
			this._attributeUsage = BamlAttributeUsage.Default;
			this._typeConverterAssemblyName = string.Empty;
			this._typeConverterName = string.Empty;
			this._properties.Clear();
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0008BFFC File Offset: 0x0008A1FC
		private BamlAttributeInfoRecord SetCommonPropertyInfo(BamlReader.BamlNodeInfo nodeInfo, short attrId)
		{
			BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(attrId);
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
			nodeInfo.LocalName = attributeInfoFromId.Name;
			nodeInfo.Name = typeInfoFromId.TypeFullName + "." + nodeInfo.LocalName;
			string assemblyName;
			string prefix;
			string xmlNamespace;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out assemblyName, out prefix, out xmlNamespace);
			nodeInfo.AssemblyName = assemblyName;
			nodeInfo.Prefix = prefix;
			nodeInfo.XmlNamespace = xmlNamespace;
			nodeInfo.ClrNamespace = typeInfoFromId.ClrNamespace;
			nodeInfo.AttributeUsage = attributeInfoFromId.AttributeUsage;
			return attributeInfoFromId;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0008C08C File Offset: 0x0008A28C
		private string GetTemplateBindingExtensionValueString(short memberId)
		{
			string str = string.Empty;
			string text = null;
			string text2;
			string name;
			if (memberId < 0)
			{
				memberId = -memberId;
				DependencyProperty dependencyProperty = null;
				if (memberId < 137)
				{
					KnownProperties knownProperty = (KnownProperties)memberId;
					dependencyProperty = KnownTypes.GetKnownDependencyPropertyFromId(knownProperty);
				}
				if (dependencyProperty == null)
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				text2 = dependencyProperty.OwnerType.Name;
				name = dependencyProperty.Name;
				object obj = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
				text = ((obj == null) ? string.Empty : ((string)obj));
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
				string text3;
				string text4;
				this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text3, out text, out text4);
				text2 = typeInfoFromId.TypeFullName;
				text2 = text2.Substring(text2.LastIndexOf(".", StringComparison.Ordinal) + 1);
				name = attributeInfoFromId.Name;
			}
			if (text == string.Empty)
			{
				str += text2;
			}
			else
			{
				str = str + text + ":" + text2;
			}
			return str + "." + name + "}";
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0008C1A4 File Offset: 0x0008A3A4
		private string GetStaticExtensionValueString(short memberId)
		{
			string str = string.Empty;
			string text = null;
			string text2 = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			if (text2 != string.Empty)
			{
				str = "{" + text2 + ":Static ";
			}
			else
			{
				str = "{Static ";
			}
			string text3;
			string str2;
			if (memberId < 0)
			{
				memberId = -memberId;
				bool flag = true;
				memberId = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(memberId, out flag);
				if (!Enum.IsDefined(typeof(SystemResourceKeyID), (int)memberId))
				{
					throw new InvalidOperationException(SR.Get("BamlBadExtensionValue"));
				}
				SystemResourceKeyID id = (SystemResourceKeyID)memberId;
				text3 = SystemKeyConverter.GetSystemClassName(id);
				if (flag)
				{
					str2 = SystemKeyConverter.GetSystemKeyName(id);
				}
				else
				{
					str2 = SystemKeyConverter.GetSystemPropertyName(id);
				}
				object obj = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
				text = ((obj == null) ? string.Empty : ((string)obj));
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(attributeInfoFromId.OwnerTypeId);
				string text4;
				string text5;
				this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text4, out text, out text5);
				text3 = typeInfoFromId.TypeFullName;
				text3 = text3.Substring(text3.LastIndexOf(".", StringComparison.Ordinal) + 1);
				str2 = attributeInfoFromId.Name;
			}
			if (text == string.Empty)
			{
				str += text3;
			}
			else
			{
				str = str + text + ":" + text3;
			}
			return str + "." + str2 + "}";
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0008C310 File Offset: 0x0008A510
		private string GetExtensionPrefixString(string extensionName)
		{
			string result = string.Empty;
			string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml/presentation"];
			if (!string.IsNullOrEmpty(text))
			{
				result = string.Concat(new string[]
				{
					"{",
					text,
					":",
					extensionName,
					" "
				});
			}
			else
			{
				result = "{" + extensionName + " ";
			}
			return result;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0008C37C File Offset: 0x0008A57C
		private string GetInnerExtensionValueString(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			string str = string.Empty;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			if (optimizedMarkupExtensionRecord.IsValueTypeExtension)
			{
				str = this.GetTypeValueString(valueId);
			}
			else if (optimizedMarkupExtensionRecord.IsValueStaticExtension)
			{
				str = this.GetStaticExtensionValueString(valueId);
			}
			else
			{
				str = this.MapTable.GetStringFromStringId((int)valueId);
			}
			return str + "}";
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0008C3D4 File Offset: 0x0008A5D4
		private string GetExtensionValueString(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			string text = string.Empty;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			short extensionTypeId = optimizedMarkupExtensionRecord.ExtensionTypeId;
			if (extensionTypeId <= 602)
			{
				if (extensionTypeId != 189)
				{
					if (extensionTypeId == 602)
					{
						text = this.GetStaticExtensionValueString(valueId);
					}
				}
				else
				{
					text = this.GetExtensionPrefixString("DynamicResource");
					text += this.GetInnerExtensionValueString(optimizedMarkupExtensionRecord);
				}
			}
			else if (extensionTypeId != 603)
			{
				if (extensionTypeId == 634)
				{
					text = this.GetExtensionPrefixString("TemplateBinding");
					text += this.GetTemplateBindingExtensionValueString(valueId);
				}
			}
			else
			{
				text = this.GetExtensionPrefixString("StaticResource");
				text += this.GetInnerExtensionValueString(optimizedMarkupExtensionRecord);
			}
			return text;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0008C480 File Offset: 0x0008A680
		private string GetTypeValueString(short typeId)
		{
			string text = this._prefixDictionary["http://schemas.microsoft.com/winfx/2006/xaml"];
			string str;
			if (text != string.Empty)
			{
				str = "{" + text + ":Type ";
			}
			else
			{
				str = "{Type ";
			}
			BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(typeId);
			string text2;
			string text3;
			string text4;
			this.GetAssemblyAndPrefixAndXmlns(typeInfoFromId, out text2, out text3, out text4);
			string text5 = typeInfoFromId.TypeFullName;
			text5 = text5.Substring(text5.LastIndexOf(".", StringComparison.Ordinal) + 1);
			if (text3 == string.Empty)
			{
				str += text5;
			}
			else
			{
				str = str + text3 + ":" + text5;
			}
			return str + "}";
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0008C534 File Offset: 0x0008A734
		private void GetAssemblyAndPrefixAndXmlns(BamlTypeInfoRecord typeInfo, out string assemblyFullName, out string prefix, out string xmlns)
		{
			if (typeInfo.AssemblyId >= 0 || typeInfo.Type == null)
			{
				BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(typeInfo.AssemblyId);
				assemblyFullName = assemblyInfoFromId.AssemblyFullName;
			}
			else
			{
				Assembly assembly = typeInfo.Type.Assembly;
				assemblyFullName = assembly.FullName;
			}
			if (typeInfo.ClrNamespace == "System.Windows.Markup" && (assemblyFullName.StartsWith("PresentationFramework", StringComparison.Ordinal) || assemblyFullName.StartsWith("System.Xaml", StringComparison.Ordinal)))
			{
				xmlns = "http://schemas.microsoft.com/winfx/2006/xaml";
			}
			else
			{
				xmlns = this._parserContext.XamlTypeMapper.GetXmlNamespace(typeInfo.ClrNamespace, assemblyFullName);
				if (string.IsNullOrEmpty(xmlns))
				{
					List<string> xmlNamespaceList = this.GetXmlNamespaceList(typeInfo.ClrNamespace, assemblyFullName);
					prefix = this.GetXmlnsPrefix(xmlNamespaceList);
					return;
				}
			}
			prefix = this.GetXmlnsPrefix(xmlns);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0008C60C File Offset: 0x0008A80C
		private void SetXmlNamespace(string clrNamespace, string assemblyFullName, string xmlNs)
		{
			string key = clrNamespace + "#" + assemblyFullName;
			List<string> list;
			if (this._reverseXmlnsTable.ContainsKey(key))
			{
				list = this._reverseXmlnsTable[key];
			}
			else
			{
				list = new List<string>();
				this._reverseXmlnsTable[key] = list;
			}
			list.Add(xmlNs);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0008C660 File Offset: 0x0008A860
		private List<string> GetXmlNamespaceList(string clrNamespace, string assemblyFullName)
		{
			string key = clrNamespace + "#" + assemblyFullName;
			List<string> result = null;
			if (this._reverseXmlnsTable.ContainsKey(key))
			{
				result = this._reverseXmlnsTable[key];
			}
			return result;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0008C698 File Offset: 0x0008A898
		internal string GetXmlnsPrefix(string xmlns)
		{
			string result = string.Empty;
			if (xmlns == string.Empty)
			{
				xmlns = this._parserContext.XmlnsDictionary[string.Empty];
			}
			else
			{
				object obj = this._prefixDictionary[xmlns];
				if (obj != null)
				{
					result = (string)obj;
				}
			}
			return result;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0008C6EC File Offset: 0x0008A8EC
		private string GetXmlnsPrefix(List<string> xmlnsList)
		{
			if (xmlnsList != null)
			{
				for (int i = 0; i < xmlnsList.Count; i++)
				{
					string prefix = xmlnsList[i];
					string text = this._prefixDictionary[prefix];
					if (text != null)
					{
						return text;
					}
				}
			}
			return string.Empty;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0008C72C File Offset: 0x0008A92C
		private BamlMapTable MapTable
		{
			get
			{
				return this._parserContext.MapTable;
			}
		}

		// Token: 0x04001448 RID: 5192
		private BamlRecordReader _bamlRecordReader;

		// Token: 0x04001449 RID: 5193
		private XmlnsDictionary _prefixDictionary;

		// Token: 0x0400144A RID: 5194
		private BamlRecord _currentBamlRecord;

		// Token: 0x0400144B RID: 5195
		private bool _haveUnprocessedRecord;

		// Token: 0x0400144C RID: 5196
		private int _deferableContentBlockDepth;

		// Token: 0x0400144D RID: 5197
		private long _deferableContentPosition;

		// Token: 0x0400144E RID: 5198
		private List<BamlReader.BamlKeyInfo> _deferKeys;

		// Token: 0x0400144F RID: 5199
		private BamlReader.BamlKeyInfo _currentKeyInfo;

		// Token: 0x04001450 RID: 5200
		private List<BamlRecord> _currentStaticResourceRecords;

		// Token: 0x04001451 RID: 5201
		private int _currentStaticResourceRecordIndex;

		// Token: 0x04001452 RID: 5202
		private BamlNodeType _bamlNodeType;

		// Token: 0x04001453 RID: 5203
		private ReadState _readState;

		// Token: 0x04001454 RID: 5204
		private string _assemblyName;

		// Token: 0x04001455 RID: 5205
		private string _prefix;

		// Token: 0x04001456 RID: 5206
		private string _xmlNamespace;

		// Token: 0x04001457 RID: 5207
		private string _clrNamespace;

		// Token: 0x04001458 RID: 5208
		private string _value;

		// Token: 0x04001459 RID: 5209
		private string _name;

		// Token: 0x0400145A RID: 5210
		private string _localName;

		// Token: 0x0400145B RID: 5211
		private string _ownerTypeName;

		// Token: 0x0400145C RID: 5212
		private ArrayList _properties;

		// Token: 0x0400145D RID: 5213
		private DependencyProperty _propertyDP;

		// Token: 0x0400145E RID: 5214
		private int _propertiesIndex;

		// Token: 0x0400145F RID: 5215
		private int _connectionId;

		// Token: 0x04001460 RID: 5216
		private string _contentPropertyName;

		// Token: 0x04001461 RID: 5217
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x04001462 RID: 5218
		private Stack _nodeStack;

		// Token: 0x04001463 RID: 5219
		private ParserContext _parserContext;

		// Token: 0x04001464 RID: 5220
		private bool _isInjected;

		// Token: 0x04001465 RID: 5221
		private bool _useTypeConverter;

		// Token: 0x04001466 RID: 5222
		private string _typeConverterAssemblyName;

		// Token: 0x04001467 RID: 5223
		private string _typeConverterName;

		// Token: 0x04001468 RID: 5224
		private Dictionary<string, List<string>> _reverseXmlnsTable;

		// Token: 0x02000887 RID: 2183
		internal class BamlNodeInfo
		{
			// Token: 0x06008340 RID: 33600 RVA: 0x0000326D File Offset: 0x0000146D
			internal BamlNodeInfo()
			{
			}

			// Token: 0x17001DB8 RID: 7608
			// (get) Token: 0x06008341 RID: 33601 RVA: 0x00244FC9 File Offset: 0x002431C9
			// (set) Token: 0x06008342 RID: 33602 RVA: 0x00244FD1 File Offset: 0x002431D1
			internal BamlRecordType RecordType
			{
				get
				{
					return this._recordType;
				}
				set
				{
					this._recordType = value;
				}
			}

			// Token: 0x17001DB9 RID: 7609
			// (get) Token: 0x06008343 RID: 33603 RVA: 0x00244FDA File Offset: 0x002431DA
			// (set) Token: 0x06008344 RID: 33604 RVA: 0x00244FE2 File Offset: 0x002431E2
			internal string AssemblyName
			{
				get
				{
					return this._assemblyName;
				}
				set
				{
					this._assemblyName = value;
				}
			}

			// Token: 0x17001DBA RID: 7610
			// (get) Token: 0x06008345 RID: 33605 RVA: 0x00244FEB File Offset: 0x002431EB
			// (set) Token: 0x06008346 RID: 33606 RVA: 0x00244FF3 File Offset: 0x002431F3
			internal string Prefix
			{
				get
				{
					return this._prefix;
				}
				set
				{
					this._prefix = value;
				}
			}

			// Token: 0x17001DBB RID: 7611
			// (get) Token: 0x06008347 RID: 33607 RVA: 0x00244FFC File Offset: 0x002431FC
			// (set) Token: 0x06008348 RID: 33608 RVA: 0x00245004 File Offset: 0x00243204
			internal string XmlNamespace
			{
				get
				{
					return this._xmlNamespace;
				}
				set
				{
					this._xmlNamespace = value;
				}
			}

			// Token: 0x17001DBC RID: 7612
			// (get) Token: 0x06008349 RID: 33609 RVA: 0x0024500D File Offset: 0x0024320D
			// (set) Token: 0x0600834A RID: 33610 RVA: 0x00245015 File Offset: 0x00243215
			internal string ClrNamespace
			{
				get
				{
					return this._clrNamespace;
				}
				set
				{
					this._clrNamespace = value;
				}
			}

			// Token: 0x17001DBD RID: 7613
			// (get) Token: 0x0600834B RID: 33611 RVA: 0x0024501E File Offset: 0x0024321E
			// (set) Token: 0x0600834C RID: 33612 RVA: 0x00245026 File Offset: 0x00243226
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

			// Token: 0x17001DBE RID: 7614
			// (get) Token: 0x0600834D RID: 33613 RVA: 0x0024502F File Offset: 0x0024322F
			// (set) Token: 0x0600834E RID: 33614 RVA: 0x00245037 File Offset: 0x00243237
			internal string LocalName
			{
				get
				{
					return this._localName;
				}
				set
				{
					this._localName = value;
				}
			}

			// Token: 0x17001DBF RID: 7615
			// (get) Token: 0x0600834F RID: 33615 RVA: 0x00245040 File Offset: 0x00243240
			// (set) Token: 0x06008350 RID: 33616 RVA: 0x00245048 File Offset: 0x00243248
			internal BamlAttributeUsage AttributeUsage
			{
				get
				{
					return this._attributeUsage;
				}
				set
				{
					this._attributeUsage = value;
				}
			}

			// Token: 0x04004162 RID: 16738
			private BamlRecordType _recordType;

			// Token: 0x04004163 RID: 16739
			private string _assemblyName;

			// Token: 0x04004164 RID: 16740
			private string _prefix;

			// Token: 0x04004165 RID: 16741
			private string _xmlNamespace;

			// Token: 0x04004166 RID: 16742
			private string _clrNamespace;

			// Token: 0x04004167 RID: 16743
			private string _name;

			// Token: 0x04004168 RID: 16744
			private string _localName;

			// Token: 0x04004169 RID: 16745
			private BamlAttributeUsage _attributeUsage;
		}

		// Token: 0x02000888 RID: 2184
		internal class BamlPropertyInfo : BamlReader.BamlNodeInfo
		{
			// Token: 0x06008351 RID: 33617 RVA: 0x00245051 File Offset: 0x00243251
			internal BamlPropertyInfo()
			{
			}

			// Token: 0x17001DC0 RID: 7616
			// (get) Token: 0x06008352 RID: 33618 RVA: 0x00245059 File Offset: 0x00243259
			// (set) Token: 0x06008353 RID: 33619 RVA: 0x00245061 File Offset: 0x00243261
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

			// Token: 0x0400416A RID: 16746
			private string _value;
		}

		// Token: 0x02000889 RID: 2185
		internal class BamlContentPropertyInfo : BamlReader.BamlNodeInfo
		{
		}

		// Token: 0x0200088A RID: 2186
		[DebuggerDisplay("{_offset}")]
		internal class BamlKeyInfo : BamlReader.BamlPropertyInfo
		{
			// Token: 0x06008355 RID: 33621 RVA: 0x0024506A File Offset: 0x0024326A
			internal BamlKeyInfo()
			{
			}

			// Token: 0x17001DC1 RID: 7617
			// (get) Token: 0x06008356 RID: 33622 RVA: 0x00245072 File Offset: 0x00243272
			// (set) Token: 0x06008357 RID: 33623 RVA: 0x0024507A File Offset: 0x0024327A
			internal int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001DC2 RID: 7618
			// (get) Token: 0x06008358 RID: 33624 RVA: 0x00245083 File Offset: 0x00243283
			internal List<List<BamlRecord>> StaticResources
			{
				get
				{
					if (this._staticResources == null)
					{
						this._staticResources = new List<List<BamlRecord>>();
					}
					return this._staticResources;
				}
			}

			// Token: 0x0400416B RID: 16747
			private int _offset;

			// Token: 0x0400416C RID: 16748
			private List<List<BamlRecord>> _staticResources;
		}
	}
}
