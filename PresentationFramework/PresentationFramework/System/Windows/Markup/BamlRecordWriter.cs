using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001D4 RID: 468
	internal class BamlRecordWriter
	{
		// Token: 0x06001E8D RID: 7821 RVA: 0x00091D54 File Offset: 0x0008FF54
		public BamlRecordWriter(Stream stream, ParserContext parserContext, bool deferLoadingSupport)
		{
			this._bamlStream = stream;
			this._xamlTypeMapper = parserContext.XamlTypeMapper;
			this._deferLoadingSupport = deferLoadingSupport;
			this._bamlMapTable = parserContext.MapTable;
			this._parserContext = parserContext;
			this._debugBamlStream = false;
			this._lineNumber = -1;
			this._linePosition = -1;
			this._bamlBinaryWriter = new BamlBinaryWriter(stream, new UTF8Encoding());
			this._bamlRecordManager = new BamlRecordManager();
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x00091DC8 File Offset: 0x0008FFC8
		internal virtual void WriteBamlRecord(BamlRecord bamlRecord, int lineNumber, int linePosition)
		{
			try
			{
				bamlRecord.Write(this.BinaryWriter);
				if (this.DebugBamlStream && BamlRecordHelper.DoesRecordTypeHaveDebugExtension(bamlRecord.RecordType))
				{
					this.WriteDebugExtensionRecord(lineNumber, linePosition);
				}
			}
			catch (XamlParseException ex)
			{
				this._xamlTypeMapper.ThrowExceptionWithLine(ex.Message, ex.InnerException);
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool UpdateParentNodes
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x00091E2C File Offset: 0x0009002C
		internal void SetParseMode(XamlParseMode xamlParseMode)
		{
			if (this.UpdateParentNodes && xamlParseMode == XamlParseMode.Asynchronous && this.DocumentStartRecord != null)
			{
				this.DocumentStartRecord.LoadAsync = true;
				this.DocumentStartRecord.UpdateWrite(this.BinaryWriter);
			}
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x00091E5F File Offset: 0x0009005F
		internal virtual void SetMaxAsyncRecords(int maxAsyncRecords)
		{
			if (this.UpdateParentNodes && this.DocumentStartRecord != null)
			{
				this.DocumentStartRecord.MaxAsyncRecords = maxAsyncRecords;
				this.DocumentStartRecord.UpdateWrite(this.BinaryWriter);
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x00091E8E File Offset: 0x0009008E
		public bool DebugBamlStream
		{
			get
			{
				return this._debugBamlStream;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x00091E96 File Offset: 0x00090096
		internal BamlLineAndPositionRecord LineAndPositionRecord
		{
			get
			{
				if (this._bamlLineAndPositionRecord == null)
				{
					this._bamlLineAndPositionRecord = new BamlLineAndPositionRecord();
				}
				return this._bamlLineAndPositionRecord;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x00091EB1 File Offset: 0x000900B1
		internal BamlLinePositionRecord LinePositionRecord
		{
			get
			{
				if (this._bamlLinePositionRecord == null)
				{
					this._bamlLinePositionRecord = new BamlLinePositionRecord();
				}
				return this._bamlLinePositionRecord;
			}
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00091ECC File Offset: 0x000900CC
		internal void WriteDebugExtensionRecord(int lineNumber, int linePosition)
		{
			if (lineNumber != this._lineNumber)
			{
				BamlLineAndPositionRecord lineAndPositionRecord = this.LineAndPositionRecord;
				this._lineNumber = lineNumber;
				lineAndPositionRecord.LineNumber = (uint)lineNumber;
				this._linePosition = linePosition;
				lineAndPositionRecord.LinePosition = (uint)linePosition;
				lineAndPositionRecord.Write(this.BinaryWriter);
				return;
			}
			if (linePosition != this._linePosition)
			{
				this._linePosition = linePosition;
				BamlLinePositionRecord linePositionRecord = this.LinePositionRecord;
				linePositionRecord.LinePosition = (uint)linePosition;
				linePositionRecord.Write(this.BinaryWriter);
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00091F3C File Offset: 0x0009013C
		internal void WriteDocumentStart(XamlDocumentStartNode xamlDocumentNode)
		{
			BamlVersionHeader bamlVersionHeader = new BamlVersionHeader();
			bamlVersionHeader.WriteVersion(this.BinaryWriter);
			this.DocumentStartRecord = (BamlDocumentStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DocumentStart);
			this.DocumentStartRecord.DebugBaml = this.DebugBamlStream;
			this.WriteBamlRecord(this.DocumentStartRecord, xamlDocumentNode.LineNumber, xamlDocumentNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(this.DocumentStartRecord);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00091FAC File Offset: 0x000901AC
		internal void WriteDocumentEnd(XamlDocumentEndNode xamlDocumentEndNode)
		{
			BamlDocumentEndRecord bamlDocumentEndRecord = (BamlDocumentEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DocumentEnd);
			this.WriteBamlRecord(bamlDocumentEndRecord, xamlDocumentEndNode.LineNumber, xamlDocumentEndNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlDocumentEndRecord);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00091FEC File Offset: 0x000901EC
		internal void WriteConnectionId(int connectionId)
		{
			BamlConnectionIdRecord bamlConnectionIdRecord = (BamlConnectionIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConnectionId);
			bamlConnectionIdRecord.ConnectionId = connectionId;
			this.WriteAndReleaseRecord(bamlConnectionIdRecord, null);
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0009201C File Offset: 0x0009021C
		internal void WriteElementStart(XamlElementStartNode xamlElementNode)
		{
			BamlElementStartRecord bamlElementStartRecord = (BamlElementStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementStart);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlElementNode.AssemblyName, xamlElementNode.TypeFullName, out typeId))
			{
				string serializerAssemblyFullName = string.Empty;
				if (xamlElementNode.SerializerType != null)
				{
					serializerAssemblyFullName = xamlElementNode.SerializerType.Assembly.FullName;
				}
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlElementNode.AssemblyName, xamlElementNode.TypeFullName, xamlElementNode.ElementType, serializerAssemblyFullName, xamlElementNode.SerializerTypeFullName);
			}
			bamlElementStartRecord.TypeId = typeId;
			bamlElementStartRecord.CreateUsingTypeConverter = xamlElementNode.CreateUsingTypeConverter;
			bamlElementStartRecord.IsInjected = xamlElementNode.IsInjected;
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferElementDepth++;
				if (this.InStaticResourceSection)
				{
					this._staticResourceElementDepth += 1;
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				if (this.CollectingValues && KnownTypes.Types[603] == xamlElementNode.ElementType)
				{
					this._staticResourceElementDepth = 1;
					this._staticResourceRecordList = new List<BamlRecordWriter.ValueDeferRecord>(5);
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				if (this.InDynamicResourceSection)
				{
					this._dynamicResourceElementDepth += 1;
				}
				else if (this.CollectingValues && KnownTypes.Types[189] == xamlElementNode.ElementType)
				{
					this._dynamicResourceElementDepth = 1;
				}
				BamlRecordWriter.ValueDeferRecord valueDeferRecord = new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				if (this._deferComplexPropertyDepth > 0)
				{
					this._deferElement.Add(valueDeferRecord);
					return;
				}
				if (this._deferElementDepth == 2)
				{
					this._deferKeys.Add(new BamlRecordWriter.KeyDeferRecord(xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					valueDeferRecord.UpdateOffset = true;
					this._deferValues.Add(valueDeferRecord);
					return;
				}
				if (!this._deferKeyCollecting)
				{
					this._deferValues.Add(valueDeferRecord);
					return;
				}
				if (typeof(string).IsAssignableFrom(xamlElementNode.ElementType) || KnownTypes.Types[602].IsAssignableFrom(xamlElementNode.ElementType) || KnownTypes.Types[691].IsAssignableFrom(xamlElementNode.ElementType))
				{
					((BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1]).RecordList.Add(valueDeferRecord);
					return;
				}
				XamlParser.ThrowException("ParserBadKey", xamlElementNode.TypeFullName, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				return;
			}
			else
			{
				if (this._deferLoadingSupport && KnownTypes.Types[524].IsAssignableFrom(xamlElementNode.ElementType))
				{
					this._deferElementDepth = 1;
					this._deferEndOfStartReached = false;
					this._deferElement = new ArrayList(2);
					this._deferKeys = new ArrayList(10);
					this._deferValues = new ArrayList(100);
					this._deferElement.Add(new BamlRecordWriter.ValueDeferRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition));
					return;
				}
				this.WriteBamlRecord(bamlElementStartRecord, xamlElementNode.LineNumber, xamlElementNode.LinePosition);
				this.BamlRecordManager.ReleaseWriteRecord(bamlElementStartRecord);
				return;
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00092368 File Offset: 0x00090568
		internal void WriteElementEnd(XamlElementEndNode xamlElementEndNode)
		{
			BamlElementEndRecord bamlRecord = (BamlElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementEnd);
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				int deferElementDepth = this._deferElementDepth;
				this._deferElementDepth = deferElementDepth - 1;
				if (deferElementDepth == 1)
				{
					this.WriteDeferableContent(xamlElementEndNode);
					this._deferKeys = null;
					this._deferValues = null;
					this._deferElement = null;
					return;
				}
			}
			this.WriteAndReleaseRecord(bamlRecord, xamlElementEndNode);
			if (this._deferLoadingSupport && this._staticResourceElementDepth > 0)
			{
				short num = this._staticResourceElementDepth;
				this._staticResourceElementDepth = num - 1;
				if (num == 1)
				{
					this.WriteStaticResource();
					this._staticResourceRecordList = null;
					return;
				}
			}
			if (this._deferLoadingSupport && this._dynamicResourceElementDepth > 0)
			{
				short num = this._dynamicResourceElementDepth;
				this._dynamicResourceElementDepth = num - 1;
			}
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0009242B File Offset: 0x0009062B
		internal void WriteEndAttributes(XamlEndAttributesNode xamlEndAttributesNode)
		{
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferEndOfStartReached = true;
			}
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00092448 File Offset: 0x00090648
		internal void WriteLiteralContent(XamlLiteralContentNode xamlLiteralContentNode)
		{
			BamlLiteralContentRecord bamlLiteralContentRecord = (BamlLiteralContentRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.LiteralContent);
			bamlLiteralContentRecord.Value = xamlLiteralContentNode.Content;
			this.WriteAndReleaseRecord(bamlLiteralContentRecord, xamlLiteralContentNode);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0009247C File Offset: 0x0009067C
		internal void WriteDefAttributeKeyType(XamlDefAttributeKeyTypeNode xamlDefNode)
		{
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlDefNode.AssemblyName, xamlDefNode.Value, out typeId))
			{
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlDefNode.AssemblyName, xamlDefNode.Value, xamlDefNode.ValueType, string.Empty, string.Empty);
			}
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyType) as BamlDefAttributeKeyTypeRecord;
			bamlDefAttributeKeyTypeRecord.TypeId = typeId;
			((IBamlDictionaryKey)bamlDefAttributeKeyTypeRecord).KeyObject = xamlDefNode.ValueType;
			if (this._deferLoadingSupport && this._deferElementDepth == 2)
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlDefAttributeKeyTypeRecord);
				keyDeferRecord.Record = bamlDefAttributeKeyTypeRecord;
				keyDeferRecord.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlDefAttributeKeyTypeRecord, xamlDefNode.LineNumber, xamlDefNode.LinePosition));
				return;
			}
			this.WriteBamlRecord(bamlDefAttributeKeyTypeRecord, xamlDefNode.LineNumber, xamlDefNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlDefAttributeKeyTypeRecord);
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000925AE File Offset: 0x000907AE
		private void TransferOldSharedData(IBamlDictionaryKey oldRecord, IBamlDictionaryKey newRecord)
		{
			if (oldRecord != null && newRecord != null)
			{
				newRecord.Shared = oldRecord.Shared;
				newRecord.SharedSet = oldRecord.SharedSet;
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000925D0 File Offset: 0x000907D0
		private IBamlDictionaryKey FindBamlDictionaryKey(BamlRecordWriter.KeyDeferRecord record)
		{
			if (record != null)
			{
				if (record.RecordList != null)
				{
					for (int i = 0; i < record.RecordList.Count; i++)
					{
						BamlRecordWriter.ValueDeferRecord valueDeferRecord = (BamlRecordWriter.ValueDeferRecord)record.RecordList[i];
						IBamlDictionaryKey bamlDictionaryKey = valueDeferRecord.Record as IBamlDictionaryKey;
						if (bamlDictionaryKey != null)
						{
							return bamlDictionaryKey;
						}
					}
				}
				return record.Record as IBamlDictionaryKey;
			}
			return null;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00092630 File Offset: 0x00090830
		internal void WriteDefAttribute(XamlDefAttributeNode xamlDefNode)
		{
			if (this._deferLoadingSupport && this._deferElementDepth == 2 && xamlDefNode.Name == "Key")
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = keyDeferRecord.Record as BamlDefAttributeKeyStringRecord;
				if (bamlDefAttributeKeyStringRecord == null)
				{
					bamlDefAttributeKeyStringRecord = (BamlDefAttributeKeyStringRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyString);
					this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlDefAttributeKeyStringRecord);
					keyDeferRecord.Record = bamlDefAttributeKeyStringRecord;
				}
				short valueId;
				if (!this.MapTable.GetStringInfoId(xamlDefNode.Value, out valueId))
				{
					valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlDefNode.Value);
				}
				bamlDefAttributeKeyStringRecord.Value = xamlDefNode.Value;
				bamlDefAttributeKeyStringRecord.ValueId = valueId;
				keyDeferRecord.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			if (this._deferLoadingSupport && this._deferElementDepth == 2 && xamlDefNode.Name == "Shared")
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord2 = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				IBamlDictionaryKey bamlDictionaryKey = this.FindBamlDictionaryKey(keyDeferRecord2);
				if (bamlDictionaryKey == null)
				{
					BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord2 = (BamlDefAttributeKeyStringRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttributeKeyString);
					keyDeferRecord2.Record = bamlDefAttributeKeyStringRecord2;
					bamlDictionaryKey = bamlDefAttributeKeyStringRecord2;
				}
				bamlDictionaryKey.Shared = bool.Parse(xamlDefNode.Value);
				bamlDictionaryKey.SharedSet = true;
				keyDeferRecord2.LineNumber = xamlDefNode.LineNumber;
				keyDeferRecord2.LinePosition = xamlDefNode.LinePosition;
				return;
			}
			short nameId;
			if (!this.MapTable.GetStringInfoId(xamlDefNode.Name, out nameId))
			{
				nameId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlDefNode.Name);
			}
			BamlDefAttributeRecord bamlDefAttributeRecord = (BamlDefAttributeRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DefAttribute);
			bamlDefAttributeRecord.Value = xamlDefNode.Value;
			bamlDefAttributeRecord.Name = xamlDefNode.Name;
			bamlDefAttributeRecord.AttributeUsage = xamlDefNode.AttributeUsage;
			bamlDefAttributeRecord.NameId = nameId;
			this.WriteAndReleaseRecord(bamlDefAttributeRecord, xamlDefNode);
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00092840 File Offset: 0x00090A40
		internal void WritePresentationOptionsAttribute(XamlPresentationOptionsAttributeNode xamlPresentationOptionsNode)
		{
			short nameId;
			if (!this.MapTable.GetStringInfoId(xamlPresentationOptionsNode.Name, out nameId))
			{
				nameId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlPresentationOptionsNode.Name);
			}
			BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord = (BamlPresentationOptionsAttributeRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PresentationOptionsAttribute);
			bamlPresentationOptionsAttributeRecord.Value = xamlPresentationOptionsNode.Value;
			bamlPresentationOptionsAttributeRecord.Name = xamlPresentationOptionsNode.Name;
			bamlPresentationOptionsAttributeRecord.NameId = nameId;
			this.WriteAndReleaseRecord(bamlPresentationOptionsAttributeRecord, xamlPresentationOptionsNode);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000928B4 File Offset: 0x00090AB4
		internal void WriteNamespacePrefix(XamlXmlnsPropertyNode xamlXmlnsPropertyNode)
		{
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.XmlnsProperty);
			bamlXmlnsPropertyRecord.Prefix = xamlXmlnsPropertyNode.Prefix;
			bamlXmlnsPropertyRecord.XmlNamespace = xamlXmlnsPropertyNode.XmlNamespace;
			this.WriteAndReleaseRecord(bamlXmlnsPropertyRecord, xamlXmlnsPropertyNode);
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000928F4 File Offset: 0x00090AF4
		internal void WritePIMapping(XamlPIMappingNode xamlPIMappingNode)
		{
			BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PIMapping);
			BamlAssemblyInfoRecord bamlAssemblyInfoRecord = this.MapTable.AddAssemblyMap(this.BinaryWriter, xamlPIMappingNode.AssemblyName);
			bamlPIMappingRecord.XmlNamespace = xamlPIMappingNode.XmlNamespace;
			bamlPIMappingRecord.ClrNamespace = xamlPIMappingNode.ClrNamespace;
			bamlPIMappingRecord.AssemblyId = bamlAssemblyInfoRecord.AssemblyId;
			this.WriteBamlRecord(bamlPIMappingRecord, xamlPIMappingNode.LineNumber, xamlPIMappingNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlPIMappingRecord);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x00092970 File Offset: 0x00090B70
		internal void WritePropertyComplexStart(XamlPropertyComplexStartNode xamlComplexPropertyNode)
		{
			BamlPropertyComplexStartRecord bamlPropertyComplexStartRecord = (BamlPropertyComplexStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyComplexStart);
			bamlPropertyComplexStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlComplexPropertyNode.AssemblyName, xamlComplexPropertyNode.TypeFullName, xamlComplexPropertyNode.PropDeclaringType, xamlComplexPropertyNode.PropName, xamlComplexPropertyNode.PropValidType, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyComplexStartRecord, xamlComplexPropertyNode);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000929D0 File Offset: 0x00090BD0
		internal void WritePropertyComplexEnd(XamlPropertyComplexEndNode xamlPropertyComplexEnd)
		{
			BamlPropertyComplexEndRecord bamlRecord = (BamlPropertyComplexEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyComplexEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyComplexEnd);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000929F8 File Offset: 0x00090BF8
		public void WriteKeyElementStart(XamlElementStartNode xamlKeyElementNode)
		{
			if (!typeof(string).IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[602].IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[691].IsAssignableFrom(xamlKeyElementNode.ElementType) && !KnownTypes.Types[525].IsAssignableFrom(xamlKeyElementNode.ElementType))
			{
				XamlParser.ThrowException("ParserBadKey", xamlKeyElementNode.TypeFullName, xamlKeyElementNode.LineNumber, xamlKeyElementNode.LinePosition);
			}
			BamlKeyElementStartRecord bamlKeyElementStartRecord = (BamlKeyElementStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.KeyElementStart);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlKeyElementNode.AssemblyName, xamlKeyElementNode.TypeFullName, out typeId))
			{
				string serializerAssemblyFullName = string.Empty;
				if (xamlKeyElementNode.SerializerType != null)
				{
					serializerAssemblyFullName = xamlKeyElementNode.SerializerType.Assembly.FullName;
				}
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlKeyElementNode.AssemblyName, xamlKeyElementNode.TypeFullName, xamlKeyElementNode.ElementType, serializerAssemblyFullName, xamlKeyElementNode.SerializerTypeFullName);
			}
			bamlKeyElementStartRecord.TypeId = typeId;
			if (this._deferLoadingSupport && this._deferElementDepth == 2)
			{
				this._deferElementDepth++;
				this._deferKeyCollecting = true;
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				keyDeferRecord.RecordList = new ArrayList(5);
				keyDeferRecord.RecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlKeyElementStartRecord, xamlKeyElementNode.LineNumber, xamlKeyElementNode.LinePosition));
				if (keyDeferRecord.Record != null)
				{
					this.TransferOldSharedData(keyDeferRecord.Record as IBamlDictionaryKey, bamlKeyElementStartRecord);
					keyDeferRecord.Record = null;
				}
				keyDeferRecord.LineNumber = xamlKeyElementNode.LineNumber;
				keyDeferRecord.LinePosition = xamlKeyElementNode.LinePosition;
				return;
			}
			this.WriteAndReleaseRecord(bamlKeyElementStartRecord, xamlKeyElementNode);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x00092BD0 File Offset: 0x00090DD0
		internal void WriteKeyElementEnd(XamlElementEndNode xamlKeyElementEnd)
		{
			BamlKeyElementEndRecord bamlRecord = (BamlKeyElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.KeyElementEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlKeyElementEnd);
			if (this._deferLoadingSupport && this._deferKeyCollecting)
			{
				this._deferKeyCollecting = false;
				this._deferElementDepth--;
			}
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00092C20 File Offset: 0x00090E20
		internal void WriteConstructorParametersStart(XamlConstructorParametersStartNode xamlConstructorParametersStartNode)
		{
			BamlConstructorParametersStartRecord bamlRecord = (BamlConstructorParametersStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConstructorParametersStart);
			this.WriteAndReleaseRecord(bamlRecord, xamlConstructorParametersStartNode);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x00092C48 File Offset: 0x00090E48
		internal void WriteConstructorParametersEnd(XamlConstructorParametersEndNode xamlConstructorParametersEndNode)
		{
			BamlConstructorParametersEndRecord bamlRecord = (BamlConstructorParametersEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ConstructorParametersEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlConstructorParametersEndNode);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00092C70 File Offset: 0x00090E70
		internal virtual void WriteContentProperty(XamlContentPropertyNode xamlContentPropertyNode)
		{
			BamlContentPropertyRecord bamlContentPropertyRecord = (BamlContentPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ContentProperty);
			bamlContentPropertyRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlContentPropertyNode.AssemblyName, xamlContentPropertyNode.TypeFullName, xamlContentPropertyNode.PropDeclaringType, xamlContentPropertyNode.PropName, xamlContentPropertyNode.PropValidType, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlContentPropertyRecord, xamlContentPropertyNode);
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00092CD0 File Offset: 0x00090ED0
		internal virtual void WriteProperty(XamlPropertyNode xamlProperty)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlProperty.AssemblyName, xamlProperty.TypeFullName, xamlProperty.PropDeclaringType, xamlProperty.PropName, xamlProperty.PropValidType, xamlProperty.AttributeUsage);
			if (xamlProperty.AssemblyName != string.Empty && xamlProperty.TypeFullName != string.Empty)
			{
				short num;
				Type type;
				bool customSerializerOrConverter = this.MapTable.GetCustomSerializerOrConverter(this.BinaryWriter, xamlProperty.ValueDeclaringType, xamlProperty.ValuePropertyType, xamlProperty.ValuePropertyMember, xamlProperty.ValuePropertyName, out num, out type);
				if (type != null)
				{
					if (customSerializerOrConverter)
					{
						BamlPropertyCustomWriteInfoRecord bamlPropertyCustomWriteInfoRecord = (BamlPropertyCustomWriteInfoRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyCustom);
						bamlPropertyCustomWriteInfoRecord.AttributeId = attributeId;
						bamlPropertyCustomWriteInfoRecord.Value = xamlProperty.Value;
						bamlPropertyCustomWriteInfoRecord.ValueType = xamlProperty.ValuePropertyType;
						bamlPropertyCustomWriteInfoRecord.SerializerTypeId = num;
						bamlPropertyCustomWriteInfoRecord.SerializerType = type;
						bamlPropertyCustomWriteInfoRecord.TypeContext = this.TypeConvertContext;
						if (num == 137)
						{
							if (xamlProperty.HasValueId)
							{
								bamlPropertyCustomWriteInfoRecord.ValueId = xamlProperty.ValueId;
								bamlPropertyCustomWriteInfoRecord.ValueMemberName = xamlProperty.MemberName;
							}
							else
							{
								string text;
								Type dependencyPropertyOwnerAndName = this._xamlTypeMapper.GetDependencyPropertyOwnerAndName(xamlProperty.Value, this.ParserContext, xamlProperty.DefaultTargetType, out text);
								short valueId;
								short attributeOrTypeId = this.MapTable.GetAttributeOrTypeId(this.BinaryWriter, dependencyPropertyOwnerAndName, text, out valueId);
								if (attributeOrTypeId < 0)
								{
									bamlPropertyCustomWriteInfoRecord.ValueId = attributeOrTypeId;
									bamlPropertyCustomWriteInfoRecord.ValueMemberName = null;
								}
								else
								{
									bamlPropertyCustomWriteInfoRecord.ValueId = valueId;
									bamlPropertyCustomWriteInfoRecord.ValueMemberName = text;
								}
							}
						}
						this.WriteAndReleaseRecord(bamlPropertyCustomWriteInfoRecord, xamlProperty);
						return;
					}
					BamlPropertyWithConverterRecord bamlPropertyWithConverterRecord = (BamlPropertyWithConverterRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithConverter);
					bamlPropertyWithConverterRecord.AttributeId = attributeId;
					bamlPropertyWithConverterRecord.Value = xamlProperty.Value;
					bamlPropertyWithConverterRecord.ConverterTypeId = num;
					this.WriteAndReleaseRecord(bamlPropertyWithConverterRecord, xamlProperty);
					return;
				}
			}
			this.BaseWriteProperty(xamlProperty);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00092EB0 File Offset: 0x000910B0
		internal virtual void WritePropertyWithExtension(XamlPropertyWithExtensionNode xamlPropertyNode)
		{
			short valueId = 0;
			short extensionTypeId = xamlPropertyNode.ExtensionTypeId;
			bool isValueTypeExtension = false;
			bool isValueStaticExtension = false;
			if (extensionTypeId == 189 || extensionTypeId == 603)
			{
				if (xamlPropertyNode.IsValueNestedExtension)
				{
					if (xamlPropertyNode.IsValueTypeExtension)
					{
						Type typeFromBaseString = this._xamlTypeMapper.GetTypeFromBaseString(xamlPropertyNode.Value, this.ParserContext, true);
						if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, typeFromBaseString.Assembly.FullName, typeFromBaseString.FullName, out valueId))
						{
							valueId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, typeFromBaseString.Assembly.FullName, typeFromBaseString.FullName, typeFromBaseString, string.Empty, string.Empty);
						}
						isValueTypeExtension = true;
					}
					else
					{
						valueId = this.MapTable.GetStaticMemberId(this.BinaryWriter, this.ParserContext, 602, xamlPropertyNode.Value, xamlPropertyNode.DefaultTargetType);
						isValueStaticExtension = true;
					}
				}
				else if (!this.MapTable.GetStringInfoId(xamlPropertyNode.Value, out valueId))
				{
					valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlPropertyNode.Value);
				}
			}
			else
			{
				valueId = this.MapTable.GetStaticMemberId(this.BinaryWriter, this.ParserContext, extensionTypeId, xamlPropertyNode.Value, xamlPropertyNode.DefaultTargetType);
			}
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyNode.AssemblyName, xamlPropertyNode.TypeFullName, xamlPropertyNode.PropDeclaringType, xamlPropertyNode.PropName, xamlPropertyNode.PropValidType, BamlAttributeUsage.Default);
			if (this._deferLoadingSupport && this._deferElementDepth > 0 && this.CollectingValues && extensionTypeId == 603)
			{
				BamlOptimizedStaticResourceRecord bamlOptimizedStaticResourceRecord = (BamlOptimizedStaticResourceRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.OptimizedStaticResource);
				bamlOptimizedStaticResourceRecord.IsValueTypeExtension = isValueTypeExtension;
				bamlOptimizedStaticResourceRecord.IsValueStaticExtension = isValueStaticExtension;
				bamlOptimizedStaticResourceRecord.ValueId = valueId;
				this._staticResourceRecordList = new List<BamlRecordWriter.ValueDeferRecord>(1);
				this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlOptimizedStaticResourceRecord, xamlPropertyNode.LineNumber, xamlPropertyNode.LinePosition));
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
				keyDeferRecord.StaticResourceRecordList.Add(this._staticResourceRecordList);
				BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord = (BamlPropertyWithStaticResourceIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithStaticResourceId);
				bamlPropertyWithStaticResourceIdRecord.AttributeId = attributeId;
				bamlPropertyWithStaticResourceIdRecord.StaticResourceId = (short)(keyDeferRecord.StaticResourceRecordList.Count - 1);
				this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlPropertyWithStaticResourceIdRecord, xamlPropertyNode.LineNumber, xamlPropertyNode.LinePosition));
				this._staticResourceRecordList = null;
				return;
			}
			BamlPropertyWithExtensionRecord bamlPropertyWithExtensionRecord = (BamlPropertyWithExtensionRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyWithExtension);
			bamlPropertyWithExtensionRecord.AttributeId = attributeId;
			bamlPropertyWithExtensionRecord.ExtensionTypeId = extensionTypeId;
			bamlPropertyWithExtensionRecord.IsValueTypeExtension = isValueTypeExtension;
			bamlPropertyWithExtensionRecord.IsValueStaticExtension = isValueStaticExtension;
			bamlPropertyWithExtensionRecord.ValueId = valueId;
			this.WriteAndReleaseRecord(bamlPropertyWithExtensionRecord, xamlPropertyNode);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00093174 File Offset: 0x00091374
		internal virtual void WritePropertyWithType(XamlPropertyWithTypeNode xamlPropertyWithType)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyWithType.AssemblyName, xamlPropertyWithType.TypeFullName, xamlPropertyWithType.PropDeclaringType, xamlPropertyWithType.PropName, xamlPropertyWithType.PropValidType, BamlAttributeUsage.Default);
			short typeId;
			if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, xamlPropertyWithType.ValueTypeAssemblyName, xamlPropertyWithType.ValueTypeFullName, out typeId))
			{
				typeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, xamlPropertyWithType.ValueTypeAssemblyName, xamlPropertyWithType.ValueTypeFullName, xamlPropertyWithType.ValueElementType, xamlPropertyWithType.ValueSerializerTypeAssemblyName, xamlPropertyWithType.ValueSerializerTypeFullName);
			}
			BamlPropertyTypeReferenceRecord bamlPropertyTypeReferenceRecord = this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyTypeReference) as BamlPropertyTypeReferenceRecord;
			bamlPropertyTypeReferenceRecord.AttributeId = attributeId;
			bamlPropertyTypeReferenceRecord.TypeId = typeId;
			this.WriteAndReleaseRecord(bamlPropertyTypeReferenceRecord, xamlPropertyWithType);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0009322C File Offset: 0x0009142C
		internal void BaseWriteProperty(XamlPropertyNode xamlProperty)
		{
			short attributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlProperty.AssemblyName, xamlProperty.TypeFullName, xamlProperty.PropDeclaringType, xamlProperty.PropName, xamlProperty.PropValidType, xamlProperty.AttributeUsage);
			BamlPropertyRecord bamlPropertyRecord = (BamlPropertyRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.Property);
			bamlPropertyRecord.AttributeId = attributeId;
			bamlPropertyRecord.Value = xamlProperty.Value;
			this.WriteAndReleaseRecord(bamlPropertyRecord, xamlProperty);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00002137 File Offset: 0x00000337
		internal void WriteClrEvent(XamlClrEventNode xamlClrEventNode)
		{
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0009329C File Offset: 0x0009149C
		internal void WritePropertyArrayStart(XamlPropertyArrayStartNode xamlPropertyArrayStartNode)
		{
			BamlPropertyArrayStartRecord bamlPropertyArrayStartRecord = (BamlPropertyArrayStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyArrayStart);
			bamlPropertyArrayStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyArrayStartNode.AssemblyName, xamlPropertyArrayStartNode.TypeFullName, xamlPropertyArrayStartNode.PropDeclaringType, xamlPropertyArrayStartNode.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyArrayStartRecord, xamlPropertyArrayStartNode);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000932F8 File Offset: 0x000914F8
		internal virtual void WritePropertyArrayEnd(XamlPropertyArrayEndNode xamlPropertyArrayEndNode)
		{
			BamlPropertyArrayEndRecord bamlRecord = (BamlPropertyArrayEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyArrayEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyArrayEndNode);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00093320 File Offset: 0x00091520
		internal void WritePropertyIListStart(XamlPropertyIListStartNode xamlPropertyIListStart)
		{
			BamlPropertyIListStartRecord bamlPropertyIListStartRecord = (BamlPropertyIListStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIListStart);
			bamlPropertyIListStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyIListStart.AssemblyName, xamlPropertyIListStart.TypeFullName, xamlPropertyIListStart.PropDeclaringType, xamlPropertyIListStart.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyIListStartRecord, xamlPropertyIListStart);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0009337C File Offset: 0x0009157C
		internal virtual void WritePropertyIListEnd(XamlPropertyIListEndNode xamlPropertyIListEndNode)
		{
			BamlPropertyIListEndRecord bamlRecord = (BamlPropertyIListEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIListEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyIListEndNode);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000933A4 File Offset: 0x000915A4
		internal void WritePropertyIDictionaryStart(XamlPropertyIDictionaryStartNode xamlPropertyIDictionaryStartNode)
		{
			BamlPropertyIDictionaryStartRecord bamlPropertyIDictionaryStartRecord = (BamlPropertyIDictionaryStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIDictionaryStart);
			bamlPropertyIDictionaryStartRecord.AttributeId = this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlPropertyIDictionaryStartNode.AssemblyName, xamlPropertyIDictionaryStartNode.TypeFullName, xamlPropertyIDictionaryStartNode.PropDeclaringType, xamlPropertyIDictionaryStartNode.PropName, null, BamlAttributeUsage.Default);
			this.WriteAndReleaseRecord(bamlPropertyIDictionaryStartRecord, xamlPropertyIDictionaryStartNode);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00093400 File Offset: 0x00091600
		internal virtual void WritePropertyIDictionaryEnd(XamlPropertyIDictionaryEndNode xamlPropertyIDictionaryEndNode)
		{
			BamlPropertyIDictionaryEndRecord bamlRecord = (BamlPropertyIDictionaryEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.PropertyIDictionaryEnd);
			this.WriteAndReleaseRecord(bamlRecord, xamlPropertyIDictionaryEndNode);
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x00093428 File Offset: 0x00091628
		internal void WriteRoutedEvent(XamlRoutedEventNode xamlRoutedEventNode)
		{
			BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.RoutedEvent);
			BamlAttributeInfoRecord bamlAttributeInfoRecord;
			this.MapTable.AddAttributeInfoMap(this.BinaryWriter, xamlRoutedEventNode.AssemblyName, xamlRoutedEventNode.TypeFullName, null, xamlRoutedEventNode.EventName, null, BamlAttributeUsage.Default, out bamlAttributeInfoRecord);
			bamlAttributeInfoRecord.Event = xamlRoutedEventNode.Event;
			bamlRoutedEventRecord.AttributeId = bamlAttributeInfoRecord.AttributeId;
			bamlRoutedEventRecord.Value = xamlRoutedEventNode.Value;
			this.WriteAndReleaseRecord(bamlRoutedEventRecord, xamlRoutedEventNode);
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x000934A0 File Offset: 0x000916A0
		internal void WriteText(XamlTextNode xamlTextNode)
		{
			BamlTextRecord bamlTextRecord;
			if (xamlTextNode.ConverterType == null)
			{
				if (!this.InStaticResourceSection && !this.InDynamicResourceSection)
				{
					bamlTextRecord = (BamlTextRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.Text);
				}
				else
				{
					bamlTextRecord = (BamlTextWithIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.TextWithId);
					short valueId;
					if (!this.MapTable.GetStringInfoId(xamlTextNode.Text, out valueId))
					{
						valueId = this.MapTable.AddStringInfoMap(this.BinaryWriter, xamlTextNode.Text);
					}
					((BamlTextWithIdRecord)bamlTextRecord).ValueId = valueId;
				}
			}
			else
			{
				bamlTextRecord = (BamlTextWithConverterRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.TextWithConverter);
				string fullName = xamlTextNode.ConverterType.Assembly.FullName;
				string fullName2 = xamlTextNode.ConverterType.FullName;
				short converterTypeId;
				if (!this.MapTable.GetTypeInfoId(this.BinaryWriter, fullName, fullName2, out converterTypeId))
				{
					converterTypeId = this.MapTable.AddTypeInfoMap(this.BinaryWriter, fullName, fullName2, xamlTextNode.ConverterType, string.Empty, string.Empty);
				}
				((BamlTextWithConverterRecord)bamlTextRecord).ConverterTypeId = converterTypeId;
			}
			bamlTextRecord.Value = xamlTextNode.Text;
			this.WriteAndReleaseRecord(bamlTextRecord, xamlTextNode);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000935C0 File Offset: 0x000917C0
		private void WriteAndReleaseRecord(BamlRecord bamlRecord, XamlNode xamlNode)
		{
			int lineNumber = (xamlNode != null) ? xamlNode.LineNumber : 0;
			int linePosition = (xamlNode != null) ? xamlNode.LinePosition : 0;
			if (this._deferLoadingSupport && this._deferElementDepth > 0)
			{
				if (this.InStaticResourceSection)
				{
					this._staticResourceRecordList.Add(new BamlRecordWriter.ValueDeferRecord(bamlRecord, lineNumber, linePosition));
					return;
				}
				BamlRecordWriter.ValueDeferRecord value = new BamlRecordWriter.ValueDeferRecord(bamlRecord, lineNumber, linePosition);
				if (!this._deferEndOfStartReached)
				{
					this._deferElement.Add(value);
					return;
				}
				if (this._deferElementDepth == 1 && xamlNode is XamlPropertyComplexStartNode)
				{
					this._deferComplexPropertyDepth++;
				}
				if (this._deferComplexPropertyDepth > 0)
				{
					this._deferElement.Add(value);
					if (this._deferElementDepth == 1 && xamlNode is XamlPropertyComplexEndNode)
					{
						this._deferComplexPropertyDepth--;
						return;
					}
				}
				else
				{
					if (this._deferKeyCollecting)
					{
						((BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1]).RecordList.Add(value);
						return;
					}
					this._deferValues.Add(value);
					return;
				}
			}
			else
			{
				this.WriteBamlRecord(bamlRecord, lineNumber, linePosition);
				this.BamlRecordManager.ReleaseWriteRecord(bamlRecord);
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000936E8 File Offset: 0x000918E8
		private void WriteDeferableContent(XamlElementEndNode xamlNode)
		{
			for (int i = 0; i < this._deferElement.Count; i++)
			{
				BamlRecordWriter.ValueDeferRecord valueDeferRecord = (BamlRecordWriter.ValueDeferRecord)this._deferElement[i];
				this.WriteBamlRecord(valueDeferRecord.Record, valueDeferRecord.LineNumber, valueDeferRecord.LinePosition);
			}
			BamlDeferableContentStartRecord bamlDeferableContentStartRecord = (BamlDeferableContentStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.DeferableContentStart);
			this.WriteBamlRecord(bamlDeferableContentStartRecord, xamlNode.LineNumber, xamlNode.LinePosition);
			long num = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			for (int j = 0; j < this._deferKeys.Count; j++)
			{
				BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[j];
				if (keyDeferRecord.RecordList != null && keyDeferRecord.RecordList.Count > 0)
				{
					for (int k = 0; k < keyDeferRecord.RecordList.Count; k++)
					{
						BamlRecordWriter.ValueDeferRecord valueDeferRecord2 = (BamlRecordWriter.ValueDeferRecord)keyDeferRecord.RecordList[k];
						this.WriteBamlRecord(valueDeferRecord2.Record, valueDeferRecord2.LineNumber, valueDeferRecord2.LinePosition);
					}
				}
				else if (keyDeferRecord.Record == null)
				{
					XamlParser.ThrowException("ParserNoDictionaryKey", keyDeferRecord.LineNumber, keyDeferRecord.LinePosition);
				}
				else
				{
					this.WriteBamlRecord(keyDeferRecord.Record, keyDeferRecord.LineNumber, keyDeferRecord.LinePosition);
				}
				List<List<BamlRecordWriter.ValueDeferRecord>> staticResourceRecordList = keyDeferRecord.StaticResourceRecordList;
				if (staticResourceRecordList.Count > 0)
				{
					for (int l = 0; l < staticResourceRecordList.Count; l++)
					{
						List<BamlRecordWriter.ValueDeferRecord> list = staticResourceRecordList[l];
						for (int m = 0; m < list.Count; m++)
						{
							BamlRecordWriter.ValueDeferRecord valueDeferRecord3 = list[m];
							this.WriteBamlRecord(valueDeferRecord3.Record, valueDeferRecord3.LineNumber, valueDeferRecord3.LinePosition);
						}
					}
				}
			}
			long num2 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			int num3 = 0;
			for (int n = 0; n < this._deferValues.Count; n++)
			{
				BamlRecordWriter.ValueDeferRecord valueDeferRecord4 = (BamlRecordWriter.ValueDeferRecord)this._deferValues[n];
				if (valueDeferRecord4.UpdateOffset)
				{
					BamlRecordWriter.KeyDeferRecord keyDeferRecord2 = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[num3++];
					long num4 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
					IBamlDictionaryKey bamlDictionaryKey;
					if (keyDeferRecord2.RecordList != null && keyDeferRecord2.RecordList.Count > 0)
					{
						BamlRecordWriter.ValueDeferRecord valueDeferRecord5 = (BamlRecordWriter.ValueDeferRecord)keyDeferRecord2.RecordList[0];
						bamlDictionaryKey = (IBamlDictionaryKey)valueDeferRecord5.Record;
					}
					else
					{
						bamlDictionaryKey = (IBamlDictionaryKey)keyDeferRecord2.Record;
					}
					if (bamlDictionaryKey != null)
					{
						bamlDictionaryKey.UpdateValuePosition((int)(num4 - num2), this.BinaryWriter);
					}
				}
				this.WriteBamlRecord(valueDeferRecord4.Record, valueDeferRecord4.LineNumber, valueDeferRecord4.LinePosition);
			}
			long num5 = this.BinaryWriter.Seek(0, SeekOrigin.Current);
			bamlDeferableContentStartRecord.UpdateContentSize((int)(num5 - num), this.BinaryWriter);
			BamlElementEndRecord bamlElementEndRecord = (BamlElementEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.ElementEnd);
			this.WriteBamlRecord(bamlElementEndRecord, xamlNode.LineNumber, xamlNode.LinePosition);
			this.BamlRecordManager.ReleaseWriteRecord(bamlElementEndRecord);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x00093A00 File Offset: 0x00091C00
		private void WriteStaticResource()
		{
			BamlRecordWriter.ValueDeferRecord valueDeferRecord = this._staticResourceRecordList[0];
			int lineNumber = valueDeferRecord.LineNumber;
			int linePosition = valueDeferRecord.LinePosition;
			BamlStaticResourceStartRecord bamlStaticResourceStartRecord = (BamlStaticResourceStartRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceStart);
			bamlStaticResourceStartRecord.TypeId = ((BamlElementStartRecord)valueDeferRecord.Record).TypeId;
			valueDeferRecord.Record = bamlStaticResourceStartRecord;
			valueDeferRecord = this._staticResourceRecordList[this._staticResourceRecordList.Count - 1];
			BamlStaticResourceEndRecord record = (BamlStaticResourceEndRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceEnd);
			valueDeferRecord.Record = record;
			BamlRecordWriter.KeyDeferRecord keyDeferRecord = (BamlRecordWriter.KeyDeferRecord)this._deferKeys[this._deferKeys.Count - 1];
			keyDeferRecord.StaticResourceRecordList.Add(this._staticResourceRecordList);
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this.BamlRecordManager.GetWriteRecord(BamlRecordType.StaticResourceId);
			bamlStaticResourceIdRecord.StaticResourceId = (short)(keyDeferRecord.StaticResourceRecordList.Count - 1);
			this._deferValues.Add(new BamlRecordWriter.ValueDeferRecord(bamlStaticResourceIdRecord, lineNumber, linePosition));
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x00093AFD File Offset: 0x00091CFD
		public Stream BamlStream
		{
			get
			{
				return this._bamlStream;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x00093B05 File Offset: 0x00091D05
		internal BamlBinaryWriter BinaryWriter
		{
			get
			{
				return this._bamlBinaryWriter;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x00093B0D File Offset: 0x00091D0D
		internal BamlMapTable MapTable
		{
			get
			{
				return this._bamlMapTable;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x00093B15 File Offset: 0x00091D15
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x00093B1D File Offset: 0x00091D1D
		internal virtual BamlRecordManager BamlRecordManager
		{
			get
			{
				return this._bamlRecordManager;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x00093B25 File Offset: 0x00091D25
		// (set) Token: 0x06001EC1 RID: 7873 RVA: 0x00093B2D File Offset: 0x00091D2D
		private BamlDocumentStartRecord DocumentStartRecord
		{
			get
			{
				return this._startDocumentRecord;
			}
			set
			{
				this._startDocumentRecord = value;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x00093B36 File Offset: 0x00091D36
		private bool CollectingValues
		{
			get
			{
				return this._deferEndOfStartReached && !this._deferKeyCollecting && this._deferComplexPropertyDepth <= 0;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x00093B56 File Offset: 0x00091D56
		private ITypeDescriptorContext TypeConvertContext
		{
			get
			{
				if (this._typeConvertContext == null)
				{
					this._typeConvertContext = new TypeConvertContext(this._parserContext);
				}
				return this._typeConvertContext;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x00093B77 File Offset: 0x00091D77
		private bool InStaticResourceSection
		{
			get
			{
				return this._staticResourceElementDepth > 0;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x00093B82 File Offset: 0x00091D82
		private bool InDynamicResourceSection
		{
			get
			{
				return this._dynamicResourceElementDepth > 0;
			}
		}

		// Token: 0x0400149D RID: 5277
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x0400149E RID: 5278
		private Stream _bamlStream;

		// Token: 0x0400149F RID: 5279
		private BamlBinaryWriter _bamlBinaryWriter;

		// Token: 0x040014A0 RID: 5280
		private BamlDocumentStartRecord _startDocumentRecord;

		// Token: 0x040014A1 RID: 5281
		private ParserContext _parserContext;

		// Token: 0x040014A2 RID: 5282
		private BamlMapTable _bamlMapTable;

		// Token: 0x040014A3 RID: 5283
		private BamlRecordManager _bamlRecordManager;

		// Token: 0x040014A4 RID: 5284
		private ITypeDescriptorContext _typeConvertContext;

		// Token: 0x040014A5 RID: 5285
		private bool _deferLoadingSupport;

		// Token: 0x040014A6 RID: 5286
		private int _deferElementDepth;

		// Token: 0x040014A7 RID: 5287
		private bool _deferEndOfStartReached;

		// Token: 0x040014A8 RID: 5288
		private int _deferComplexPropertyDepth;

		// Token: 0x040014A9 RID: 5289
		private bool _deferKeyCollecting;

		// Token: 0x040014AA RID: 5290
		private ArrayList _deferKeys;

		// Token: 0x040014AB RID: 5291
		private ArrayList _deferValues;

		// Token: 0x040014AC RID: 5292
		private ArrayList _deferElement;

		// Token: 0x040014AD RID: 5293
		private short _staticResourceElementDepth;

		// Token: 0x040014AE RID: 5294
		private short _dynamicResourceElementDepth;

		// Token: 0x040014AF RID: 5295
		private List<BamlRecordWriter.ValueDeferRecord> _staticResourceRecordList;

		// Token: 0x040014B0 RID: 5296
		private bool _debugBamlStream;

		// Token: 0x040014B1 RID: 5297
		private int _lineNumber;

		// Token: 0x040014B2 RID: 5298
		private int _linePosition;

		// Token: 0x040014B3 RID: 5299
		private BamlLineAndPositionRecord _bamlLineAndPositionRecord;

		// Token: 0x040014B4 RID: 5300
		private BamlLinePositionRecord _bamlLinePositionRecord;

		// Token: 0x0200088B RID: 2187
		private class DeferRecord
		{
			// Token: 0x06008359 RID: 33625 RVA: 0x0024509E File Offset: 0x0024329E
			internal DeferRecord(int lineNumber, int linePosition)
			{
				this._lineNumber = lineNumber;
				this._linePosition = linePosition;
			}

			// Token: 0x17001DC3 RID: 7619
			// (get) Token: 0x0600835A RID: 33626 RVA: 0x002450B4 File Offset: 0x002432B4
			// (set) Token: 0x0600835B RID: 33627 RVA: 0x002450BC File Offset: 0x002432BC
			internal int LineNumber
			{
				get
				{
					return this._lineNumber;
				}
				set
				{
					this._lineNumber = value;
				}
			}

			// Token: 0x17001DC4 RID: 7620
			// (get) Token: 0x0600835C RID: 33628 RVA: 0x002450C5 File Offset: 0x002432C5
			// (set) Token: 0x0600835D RID: 33629 RVA: 0x002450CD File Offset: 0x002432CD
			internal int LinePosition
			{
				get
				{
					return this._linePosition;
				}
				set
				{
					this._linePosition = value;
				}
			}

			// Token: 0x0400416D RID: 16749
			private int _lineNumber;

			// Token: 0x0400416E RID: 16750
			private int _linePosition;
		}

		// Token: 0x0200088C RID: 2188
		private class ValueDeferRecord : BamlRecordWriter.DeferRecord
		{
			// Token: 0x0600835E RID: 33630 RVA: 0x002450D6 File Offset: 0x002432D6
			internal ValueDeferRecord(BamlRecord record, int lineNumber, int linePosition) : base(lineNumber, linePosition)
			{
				this._record = record;
				this._updateOffset = false;
			}

			// Token: 0x17001DC5 RID: 7621
			// (get) Token: 0x0600835F RID: 33631 RVA: 0x002450EE File Offset: 0x002432EE
			// (set) Token: 0x06008360 RID: 33632 RVA: 0x002450F6 File Offset: 0x002432F6
			internal BamlRecord Record
			{
				get
				{
					return this._record;
				}
				set
				{
					this._record = value;
				}
			}

			// Token: 0x17001DC6 RID: 7622
			// (get) Token: 0x06008361 RID: 33633 RVA: 0x002450FF File Offset: 0x002432FF
			// (set) Token: 0x06008362 RID: 33634 RVA: 0x00245107 File Offset: 0x00243307
			internal bool UpdateOffset
			{
				get
				{
					return this._updateOffset;
				}
				set
				{
					this._updateOffset = value;
				}
			}

			// Token: 0x0400416F RID: 16751
			private bool _updateOffset;

			// Token: 0x04004170 RID: 16752
			private BamlRecord _record;
		}

		// Token: 0x0200088D RID: 2189
		private class KeyDeferRecord : BamlRecordWriter.DeferRecord
		{
			// Token: 0x06008363 RID: 33635 RVA: 0x00245110 File Offset: 0x00243310
			internal KeyDeferRecord(int lineNumber, int linePosition) : base(lineNumber, linePosition)
			{
			}

			// Token: 0x17001DC7 RID: 7623
			// (get) Token: 0x06008364 RID: 33636 RVA: 0x0024511A File Offset: 0x0024331A
			// (set) Token: 0x06008365 RID: 33637 RVA: 0x00245122 File Offset: 0x00243322
			internal BamlRecord Record
			{
				get
				{
					return this._record;
				}
				set
				{
					this._record = value;
				}
			}

			// Token: 0x17001DC8 RID: 7624
			// (get) Token: 0x06008366 RID: 33638 RVA: 0x0024512B File Offset: 0x0024332B
			// (set) Token: 0x06008367 RID: 33639 RVA: 0x00245133 File Offset: 0x00243333
			internal ArrayList RecordList
			{
				get
				{
					return this._recordList;
				}
				set
				{
					this._recordList = value;
				}
			}

			// Token: 0x17001DC9 RID: 7625
			// (get) Token: 0x06008368 RID: 33640 RVA: 0x0024513C File Offset: 0x0024333C
			internal List<List<BamlRecordWriter.ValueDeferRecord>> StaticResourceRecordList
			{
				get
				{
					if (this._staticResourceRecordList == null)
					{
						this._staticResourceRecordList = new List<List<BamlRecordWriter.ValueDeferRecord>>(1);
					}
					return this._staticResourceRecordList;
				}
			}

			// Token: 0x04004171 RID: 16753
			private BamlRecord _record;

			// Token: 0x04004172 RID: 16754
			private ArrayList _recordList;

			// Token: 0x04004173 RID: 16755
			private List<List<BamlRecordWriter.ValueDeferRecord>> _staticResourceRecordList;
		}
	}
}
