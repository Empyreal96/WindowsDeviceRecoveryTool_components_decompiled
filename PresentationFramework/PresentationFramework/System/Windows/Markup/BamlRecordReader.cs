using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.IO.Packaging;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Markup
{
	// Token: 0x020001D1 RID: 465
	internal class BamlRecordReader
	{
		// Token: 0x06001DCF RID: 7631 RVA: 0x0008C874 File Offset: 0x0008AA74
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext) : this(bamlStream, parserContext, true)
		{
			this.XamlParseMode = XamlParseMode.Synchronous;
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0008C888 File Offset: 0x0008AA88
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext, object root)
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
			this.ParserContext = parserContext;
			this._rootElement = root;
			this._bamlAsForest = (root != null);
			if (this._bamlAsForest)
			{
				this.ParserContext.RootElement = this._rootElement;
			}
			this._rootList = new ArrayList(1);
			this.BamlStream = bamlStream;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0008C8F8 File Offset: 0x0008AAF8
		internal BamlRecordReader(Stream bamlStream, ParserContext parserContext, bool loadMapper)
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
			this.ParserContext = parserContext;
			this._rootList = new ArrayList(1);
			this.BamlStream = bamlStream;
			if (loadMapper)
			{
				this.ParserContext.XamlTypeMapper = this.XamlTypeMapper;
			}
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0008C952 File Offset: 0x0008AB52
		protected internal BamlRecordReader()
		{
			this._contextStack = new ParserStack();
			this._parseMode = XamlParseMode.Synchronous;
			this._buildTopDown = true;
			base..ctor();
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0008C973 File Offset: 0x0008AB73
		internal void Initialize()
		{
			this.MapTable.Initialize();
			this.XamlTypeMapper.Initialize();
			this.ParserContext.Initialize();
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x0008C996 File Offset: 0x0008AB96
		// (set) Token: 0x06001DD5 RID: 7637 RVA: 0x0008C99E File Offset: 0x0008AB9E
		internal ArrayList RootList
		{
			get
			{
				return this._rootList;
			}
			set
			{
				this._rootList = value;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0008C9A7 File Offset: 0x0008ABA7
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x0008C9AF File Offset: 0x0008ABAF
		internal bool BuildTopDown
		{
			get
			{
				return this._buildTopDown;
			}
			set
			{
				this._buildTopDown = value;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0008C9B8 File Offset: 0x0008ABB8
		internal int BytesAvailible
		{
			get
			{
				Stream baseStream = this.BinaryReader.BaseStream;
				return (int)(baseStream.Length - baseStream.Position);
			}
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0008C9E0 File Offset: 0x0008ABE0
		internal BamlRecord GetNextRecord()
		{
			BamlRecord bamlRecord = null;
			if (this.PreParsedRecordsStart == null)
			{
				Stream baseStream = this.BinaryReader.BaseStream;
				if (this.XamlReaderStream != null)
				{
					long position = baseStream.Position;
					long num = baseStream.Length - position;
					if (1L > num)
					{
						return null;
					}
					BamlRecordType recordType = (BamlRecordType)this.BinaryReader.ReadByte();
					num -= 1L;
					bamlRecord = this.ReadNextRecordWithDebugExtension(num, recordType);
					if (bamlRecord == null)
					{
						baseStream.Seek(position, SeekOrigin.Begin);
						return null;
					}
					this.XamlReaderStream.ReaderDoneWithFileUpToPosition(baseStream.Position - 1L);
				}
				else
				{
					bool flag = true;
					while (flag)
					{
						if (this.BinaryReader.BaseStream.Length > this.BinaryReader.BaseStream.Position)
						{
							BamlRecordType recordType2 = (BamlRecordType)this.BinaryReader.ReadByte();
							bamlRecord = this.ReadNextRecordWithDebugExtension(long.MaxValue, recordType2);
							flag = false;
						}
						else
						{
							flag = false;
						}
					}
				}
			}
			else if (this.PreParsedCurrentRecord != null)
			{
				bamlRecord = this.PreParsedCurrentRecord;
				this.PreParsedCurrentRecord = this.PreParsedCurrentRecord.Next;
				if (BamlRecordHelper.HasDebugExtensionRecord(this.ParserContext.IsDebugBamlStream, bamlRecord))
				{
					this.ProcessDebugBamlRecord(this.PreParsedCurrentRecord);
					this.PreParsedCurrentRecord = this.PreParsedCurrentRecord.Next;
				}
			}
			return bamlRecord;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0008CB10 File Offset: 0x0008AD10
		internal BamlRecord ReadNextRecordWithDebugExtension(long bytesAvailable, BamlRecordType recordType)
		{
			BamlRecord bamlRecord = this.BamlRecordManager.ReadNextRecord(this.BinaryReader, bytesAvailable, recordType);
			if (this.IsDebugBamlStream && BamlRecordHelper.DoesRecordTypeHaveDebugExtension(bamlRecord.RecordType))
			{
				BamlRecord next = this.ReadDebugExtensionRecord();
				bamlRecord.Next = next;
			}
			return bamlRecord;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0008CB58 File Offset: 0x0008AD58
		internal BamlRecord ReadDebugExtensionRecord()
		{
			Stream baseStream = this.BinaryReader.BaseStream;
			long num = baseStream.Length - baseStream.Position;
			if (num == 0L)
			{
				return null;
			}
			BamlRecordType recordType = (BamlRecordType)this.BinaryReader.ReadByte();
			if (BamlRecordHelper.IsDebugBamlRecordType(recordType))
			{
				BamlRecord bamlRecord = this.BamlRecordManager.ReadNextRecord(this.BinaryReader, num, recordType);
				this.ProcessDebugBamlRecord(bamlRecord);
				return bamlRecord;
			}
			baseStream.Seek(-1L, SeekOrigin.Current);
			return null;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0008CBC0 File Offset: 0x0008ADC0
		internal void ProcessDebugBamlRecord(BamlRecord bamlRecord)
		{
			if (bamlRecord.RecordType == BamlRecordType.LineNumberAndPosition)
			{
				BamlLineAndPositionRecord bamlLineAndPositionRecord = (BamlLineAndPositionRecord)bamlRecord;
				this.LineNumber = (int)bamlLineAndPositionRecord.LineNumber;
				this.LinePosition = (int)bamlLineAndPositionRecord.LinePosition;
				return;
			}
			BamlLinePositionRecord bamlLinePositionRecord = (BamlLinePositionRecord)bamlRecord;
			this.LinePosition = (int)bamlLinePositionRecord.LinePosition;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0008CC0C File Offset: 0x0008AE0C
		internal BamlRecordType GetNextRecordType()
		{
			BamlRecordType result;
			if (this.PreParsedRecordsStart == null)
			{
				result = (BamlRecordType)this.BinaryReader.PeekChar();
			}
			else
			{
				result = this.PreParsedCurrentRecord.RecordType;
			}
			return result;
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0008CC3D File Offset: 0x0008AE3D
		internal void Close()
		{
			if (this.BamlStream != null)
			{
				this.BamlStream.Close();
			}
			this.EndOfDocument = true;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0008CC5C File Offset: 0x0008AE5C
		internal bool Read(bool singleRecord)
		{
			BamlRecord bamlRecord = null;
			bool flag = true;
			while (flag && (bamlRecord = this.GetNextRecord()) != null)
			{
				flag = this.ReadRecord(bamlRecord);
				if (singleRecord)
				{
					break;
				}
			}
			if (bamlRecord == null)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0008CC8D File Offset: 0x0008AE8D
		internal bool Read()
		{
			return this.Read(false);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0008CC96 File Offset: 0x0008AE96
		internal bool Read(BamlRecord bamlRecord, int lineNumber, int linePosition)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
			return this.ReadRecord(bamlRecord);
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0008CCB0 File Offset: 0x0008AEB0
		internal void ReadVersionHeader()
		{
			BamlVersionHeader bamlVersionHeader = new BamlVersionHeader();
			bamlVersionHeader.LoadVersion(this.BinaryReader);
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0008CCD0 File Offset: 0x0008AED0
		internal object ReadElement(long startPosition, XamlObjectIds contextXamlObjectIds, object dictionaryKey)
		{
			bool flag = true;
			this.BinaryReader.BaseStream.Position = startPosition;
			int num = 0;
			bool flag2 = false;
			this.PushContext(ReaderFlags.RealizeDeferContent, null, null, 0);
			this.CurrentContext.ElementNameOrPropertyName = contextXamlObjectIds.Name;
			this.CurrentContext.Uid = contextXamlObjectIds.Uid;
			this.CurrentContext.Key = dictionaryKey;
			BamlRecord nextRecord;
			while (flag && (nextRecord = this.GetNextRecord()) != null)
			{
				BamlElementStartRecord bamlElementStartRecord = nextRecord as BamlElementStartRecord;
				if (bamlElementStartRecord != null)
				{
					if (!this.MapTable.HasSerializerForTypeId(bamlElementStartRecord.TypeId))
					{
						num++;
					}
				}
				else if (nextRecord is BamlElementEndRecord)
				{
					num--;
				}
				flag = this.ReadRecord(nextRecord);
				if (!flag2)
				{
					this.CurrentContext.Key = dictionaryKey;
					flag2 = true;
				}
				if (num == 0)
				{
					break;
				}
			}
			object objectData = this.CurrentContext.ObjectData;
			this.CurrentContext.ObjectData = null;
			this.PopContext();
			this.MapTable.ClearConverterCache();
			return objectData;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0008CDC0 File Offset: 0x0008AFC0
		protected virtual void ReadConnectionId(BamlConnectionIdRecord bamlConnectionIdRecord)
		{
			if (this._componentConnector != null)
			{
				object currentObjectData = this.GetCurrentObjectData();
				this._componentConnector.Connect(bamlConnectionIdRecord.ConnectionId, currentObjectData);
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0008CDEE File Offset: 0x0008AFEE
		private void ReadDocumentStartRecord(BamlDocumentStartRecord documentStartRecord)
		{
			this.IsDebugBamlStream = documentStartRecord.DebugBaml;
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0008CDFC File Offset: 0x0008AFFC
		private void ReadDocumentEndRecord()
		{
			this.SetPropertyValueToParent(false);
			this.ParserContext.RootElement = null;
			this.MapTable.ClearConverterCache();
			this.EndOfDocument = true;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0008CE24 File Offset: 0x0008B024
		internal virtual bool ReadRecord(BamlRecord bamlRecord)
		{
			bool result = true;
			try
			{
				switch (bamlRecord.RecordType)
				{
				case BamlRecordType.DocumentStart:
					this.ReadDocumentStartRecord((BamlDocumentStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.DocumentEnd:
					this.ReadDocumentEndRecord();
					result = false;
					goto IL_434;
				case BamlRecordType.ElementStart:
				case BamlRecordType.StaticResourceStart:
					if (((BamlElementStartRecord)bamlRecord).IsInjected)
					{
						this.CurrentContext.SetFlag(ReaderFlags.InjectedElement);
						goto IL_434;
					}
					this.ReadElementStartRecord((BamlElementStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.ElementEnd:
				case BamlRecordType.StaticResourceEnd:
					if (this.CurrentContext.CheckFlag(ReaderFlags.InjectedElement))
					{
						this.CurrentContext.ClearFlag(ReaderFlags.InjectedElement);
						goto IL_434;
					}
					this.ReadElementEndRecord(false);
					goto IL_434;
				case BamlRecordType.Property:
					this.ReadPropertyRecord((BamlPropertyRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyCustom:
					this.ReadPropertyCustomRecord((BamlPropertyCustomRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyComplexStart:
					this.ReadPropertyComplexStartRecord((BamlPropertyComplexStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyComplexEnd:
					this.ReadPropertyComplexEndRecord();
					goto IL_434;
				case BamlRecordType.PropertyArrayStart:
					this.ReadPropertyArrayStartRecord((BamlPropertyArrayStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyArrayEnd:
					this.ReadPropertyArrayEndRecord();
					goto IL_434;
				case BamlRecordType.PropertyIListStart:
					this.ReadPropertyIListStartRecord((BamlPropertyIListStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyIListEnd:
					this.ReadPropertyIListEndRecord();
					goto IL_434;
				case BamlRecordType.PropertyIDictionaryStart:
					this.ReadPropertyIDictionaryStartRecord((BamlPropertyIDictionaryStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyIDictionaryEnd:
					this.ReadPropertyIDictionaryEndRecord();
					goto IL_434;
				case BamlRecordType.LiteralContent:
					this.ReadLiteralContentRecord((BamlLiteralContentRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.Text:
				case BamlRecordType.TextWithConverter:
				case BamlRecordType.TextWithId:
					this.ReadTextRecord((BamlTextRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.RoutedEvent:
				{
					DependencyObject dependencyObject = this.GetCurrentObjectData() as DependencyObject;
					BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)bamlRecord;
					this.ThrowException("ParserBamlEvent", bamlRoutedEventRecord.Value);
					goto IL_434;
				}
				case BamlRecordType.XmlnsProperty:
					this.ReadXmlnsPropertyRecord((BamlXmlnsPropertyRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.DefAttribute:
					this.ReadDefAttributeRecord((BamlDefAttributeRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PIMapping:
				{
					BamlPIMappingRecord bamlPIMappingRecord = (BamlPIMappingRecord)bamlRecord;
					if (!this.XamlTypeMapper.PITable.Contains(bamlPIMappingRecord.XmlNamespace))
					{
						BamlAssemblyInfoRecord assemblyInfoFromId = this.MapTable.GetAssemblyInfoFromId(bamlPIMappingRecord.AssemblyId);
						ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(bamlPIMappingRecord.ClrNamespace, assemblyInfoFromId.AssemblyFullName);
						this.XamlTypeMapper.PITable.Add(bamlPIMappingRecord.XmlNamespace, clrNamespaceAssemblyPair);
						goto IL_434;
					}
					goto IL_434;
				}
				case BamlRecordType.AssemblyInfo:
					this.MapTable.LoadAssemblyInfoRecord((BamlAssemblyInfoRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.TypeInfo:
				case BamlRecordType.TypeSerializerInfo:
					this.MapTable.LoadTypeInfoRecord((BamlTypeInfoRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.AttributeInfo:
					this.MapTable.LoadAttributeInfoRecord((BamlAttributeInfoRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.StringInfo:
					this.MapTable.LoadStringInfoRecord((BamlStringInfoRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyStringReference:
					this.ReadPropertyStringRecord((BamlPropertyStringReferenceRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyTypeReference:
					this.ReadPropertyTypeRecord((BamlPropertyTypeReferenceRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyWithExtension:
					this.ReadPropertyWithExtensionRecord((BamlPropertyWithExtensionRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyWithConverter:
					this.ReadPropertyConverterRecord((BamlPropertyWithConverterRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.DeferableContentStart:
					this.ReadDeferableContentStart((BamlDeferableContentStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.DefAttributeKeyType:
					this.ReadDefAttributeKeyTypeRecord((BamlDefAttributeKeyTypeRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.KeyElementStart:
					this.ReadKeyElementStartRecord((BamlKeyElementStartRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.KeyElementEnd:
					this.ReadKeyElementEndRecord();
					goto IL_434;
				case BamlRecordType.ConstructorParametersStart:
					this.ReadConstructorParametersStartRecord();
					goto IL_434;
				case BamlRecordType.ConstructorParametersEnd:
					this.ReadConstructorParametersEndRecord();
					goto IL_434;
				case BamlRecordType.ConstructorParameterType:
					this.ReadConstructorParameterTypeRecord((BamlConstructorParameterTypeRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.ConnectionId:
					this.ReadConnectionId((BamlConnectionIdRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.ContentProperty:
					this.ReadContentPropertyRecord((BamlContentPropertyRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.NamedElementStart:
					goto IL_434;
				case BamlRecordType.StaticResourceId:
					this.ReadStaticResourceIdRecord((BamlStaticResourceIdRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PresentationOptionsAttribute:
					this.ReadPresentationOptionsAttributeRecord((BamlPresentationOptionsAttributeRecord)bamlRecord);
					goto IL_434;
				case BamlRecordType.PropertyWithStaticResourceId:
					this.ReadPropertyWithStaticResourceIdRecord((BamlPropertyWithStaticResourceIdRecord)bamlRecord);
					goto IL_434;
				}
				this.ThrowException("ParserUnknownBaml", ((int)bamlRecord.RecordType).ToString(CultureInfo.CurrentCulture));
				IL_434:;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				XamlParseException.ThrowException(this.ParserContext, this.LineNumber, this.LinePosition, string.Empty, ex);
			}
			return result;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0008D2BC File Offset: 0x0008B4BC
		protected virtual void ReadXmlnsPropertyRecord(BamlXmlnsPropertyRecord xmlnsRecord)
		{
			if (ReaderFlags.DependencyObject == this.CurrentContext.ContextType || ReaderFlags.ClrObject == this.CurrentContext.ContextType || ReaderFlags.PropertyComplexClr == this.CurrentContext.ContextType || ReaderFlags.PropertyComplexDP == this.CurrentContext.ContextType)
			{
				this.XmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
				this.XamlTypeMapper.SetUriToAssemblyNameMapping(xmlnsRecord.XmlNamespace, xmlnsRecord.AssemblyIds);
				if (ReaderFlags.DependencyObject == this.CurrentContext.ContextType)
				{
					this.SetXmlnsOnCurrentObject(xmlnsRecord);
				}
			}
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0008D358 File Offset: 0x0008B558
		private void GetElementAndFlags(BamlElementStartRecord bamlElementStartRecord, out object element, out ReaderFlags flags, out Type delayCreatedType, out short delayCreatedTypeId)
		{
			short typeId = bamlElementStartRecord.TypeId;
			Type typeFromId = this.MapTable.GetTypeFromId(typeId);
			element = null;
			delayCreatedType = null;
			delayCreatedTypeId = 0;
			flags = ReaderFlags.Unknown;
			if (null != typeFromId)
			{
				if (bamlElementStartRecord.CreateUsingTypeConverter || typeof(MarkupExtension).IsAssignableFrom(typeFromId))
				{
					delayCreatedType = typeFromId;
					delayCreatedTypeId = typeId;
				}
				else
				{
					element = this.CreateInstanceFromType(typeFromId, typeId, false);
					if (element == null)
					{
						this.ThrowException("ParserNoElementCreate2", typeFromId.FullName);
					}
				}
				flags = this.GetFlagsFromType(typeFromId);
			}
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0008D3E0 File Offset: 0x0008B5E0
		protected ReaderFlags GetFlagsFromType(Type elementType)
		{
			ReaderFlags readerFlags = typeof(DependencyObject).IsAssignableFrom(elementType) ? ReaderFlags.DependencyObject : ReaderFlags.ClrObject;
			if (typeof(IDictionary).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.IDictionary;
			}
			else if (typeof(IList).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.IList;
			}
			else if (typeof(ArrayExtension).IsAssignableFrom(elementType))
			{
				readerFlags |= ReaderFlags.ArrayExt;
			}
			else if (BamlRecordManager.TreatAsIAddChild(elementType))
			{
				readerFlags |= ReaderFlags.IAddChild;
			}
			return readerFlags;
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0008D468 File Offset: 0x0008B668
		internal static void CheckForTreeAdd(ref ReaderFlags flags, ReaderContextStackData context)
		{
			if (context == null || (context.ContextType != ReaderFlags.ConstructorParams && context.ContextType != ReaderFlags.RealizeDeferContent))
			{
				flags |= ReaderFlags.NeedToAddToTree;
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0008D490 File Offset: 0x0008B690
		internal void SetDependencyValue(DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = (this.ParserContext != null && this.ParserContext.SkipJournaledProperties) ? (dependencyProperty.GetMetadata(dependencyObject.DependencyObjectType) as FrameworkPropertyMetadata) : null;
			if (frameworkPropertyMetadata == null || !frameworkPropertyMetadata.Journal || value is Expression)
			{
				this.SetDependencyValueCore(dependencyObject, dependencyProperty, value);
			}
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0008D4E3 File Offset: 0x0008B6E3
		internal virtual void SetDependencyValueCore(DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
		{
			dependencyObject.SetValue(dependencyProperty, value);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0008D4F0 File Offset: 0x0008B6F0
		internal object ProvideValueFromMarkupExtension(MarkupExtension markupExtension, object obj, object member)
		{
			object obj2 = null;
			ProvideValueServiceProvider provideValueProvider = this.ParserContext.ProvideValueProvider;
			provideValueProvider.SetData(obj, member);
			try
			{
				obj2 = markupExtension.ProvideValue(provideValueProvider);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.TraceActivityItem(TraceMarkup.ProvideValue, new object[]
					{
						markupExtension,
						obj,
						member,
						obj2
					});
				}
			}
			finally
			{
				provideValueProvider.ClearData();
			}
			return obj2;
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0008D55C File Offset: 0x0008B75C
		internal void BaseReadElementStartRecord(BamlElementStartRecord bamlElementRecord)
		{
			object obj = null;
			Type expectedType = null;
			short expectedTypeId = 0;
			ReaderFlags contextFlags = ReaderFlags.Unknown;
			ReaderContextStackData currentContext = this.CurrentContext;
			if (this._bamlAsForest && currentContext == null)
			{
				obj = this._rootElement;
				contextFlags = this.GetFlagsFromType(obj.GetType());
			}
			else
			{
				if (currentContext != null && (ReaderFlags.PropertyComplexClr == currentContext.ContextType || ReaderFlags.PropertyComplexDP == currentContext.ContextType) && null == currentContext.ExpectedType)
				{
					string propNameFrom = this.GetPropNameFrom(currentContext.ObjectData);
					this.ThrowException("ParserNoComplexMulti", propNameFrom);
				}
				if (this.ParentContext == null)
				{
					this.SetPropertyValueToParent(true);
				}
				this.GetElementAndFlags(bamlElementRecord, out obj, out contextFlags, out expectedType, out expectedTypeId);
			}
			Stream bamlStream = this.BamlStream;
			if (!this._bamlAsForest && currentContext == null && obj != null && bamlStream != null && !(bamlStream is ReaderStream) && this.StreamPosition == this.StreamLength)
			{
				this.ReadDocumentEndRecord();
				if (this.RootList.Count == 0)
				{
					this.RootList.Add(obj);
				}
				this.IsRootAlreadyLoaded = true;
				return;
			}
			if (obj != null)
			{
				string name = null;
				if (bamlElementRecord is BamlNamedElementStartRecord)
				{
					BamlNamedElementStartRecord bamlNamedElementStartRecord = bamlElementRecord as BamlNamedElementStartRecord;
					name = bamlNamedElementStartRecord.RuntimeName;
				}
				this.ElementInitialize(obj, name);
			}
			BamlRecordReader.CheckForTreeAdd(ref contextFlags, currentContext);
			this.PushContext(contextFlags, obj, expectedType, expectedTypeId, bamlElementRecord.CreateUsingTypeConverter);
			if (this.BuildTopDown && obj != null && (obj is UIElement || obj is ContentElement || obj is UIElement3D))
			{
				this.SetPropertyValueToParent(true);
				return;
			}
			if (this.CurrentContext.CheckFlag(ReaderFlags.IDictionary))
			{
				bool flag = false;
				if (this.CheckExplicitCollectionTag(ref flag))
				{
					this.CurrentContext.MarkAddedToTree();
					if (obj is ResourceDictionary)
					{
						this.SetCollectionPropertyValue(this.ParentContext);
					}
				}
			}
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0008D708 File Offset: 0x0008B908
		protected virtual bool ReadElementStartRecord(BamlElementStartRecord bamlElementRecord)
		{
			bool flag = this.MapTable.HasSerializerForTypeId(bamlElementRecord.TypeId);
			if (flag)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInstBegin);
				try
				{
					BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(bamlElementRecord.TypeId);
					XamlSerializer xamlSerializer = this.CreateSerializer((BamlTypeInfoWithSerializerRecord)typeInfoFromId);
					if (this.ParserContext.RootElement == null)
					{
						this.ParserContext.RootElement = this._rootElement;
					}
					if (this.ParserContext.StyleConnector == null)
					{
						this.ParserContext.StyleConnector = (this._rootElement as IStyleConnector);
					}
					xamlSerializer.ConvertBamlToObject(this, bamlElementRecord, this.ParserContext);
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInstEnd);
				}
				return true;
			}
			this.BaseReadElementStartRecord(bamlElementRecord);
			return false;
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0008D7D4 File Offset: 0x0008B9D4
		protected internal virtual void ReadElementEndRecord(bool fromNestedBamlRecordReader)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpectedEndEle");
			}
			object currentObjectData = this.GetCurrentObjectData();
			this.ElementEndInit(ref currentObjectData);
			this.SetPropertyValueToParent(false);
			ReaderFlags contextFlags = this.CurrentContext.ContextFlags;
			this.FreezeIfRequired(currentObjectData);
			this.PopContext();
			if ((contextFlags & ReaderFlags.AddedToTree) == ReaderFlags.Unknown && this.CurrentContext != null)
			{
				ReaderFlags contextType = this.CurrentContext.ContextType;
				if (contextType == ReaderFlags.RealizeDeferContent)
				{
					this.CurrentContext.ObjectData = currentObjectData;
					return;
				}
				if (contextType != ReaderFlags.ConstructorParams)
				{
					return;
				}
				this.SetConstructorParameter(currentObjectData);
			}
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0008D888 File Offset: 0x0008BA88
		internal virtual void ReadKeyElementStartRecord(BamlKeyElementStartRecord bamlElementRecord)
		{
			Type typeFromId = this.MapTable.GetTypeFromId(bamlElementRecord.TypeId);
			ReaderFlags contextFlags = (typeFromId.IsAssignableFrom(typeof(DependencyObject)) ? ReaderFlags.DependencyObject : ReaderFlags.ClrObject) | ReaderFlags.NeedToAddToTree;
			this.PushContext(contextFlags, null, typeFromId, bamlElementRecord.TypeId);
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0008D8D8 File Offset: 0x0008BAD8
		internal virtual void ReadKeyElementEndRecord()
		{
			object key = this.ProvideValueFromMarkupExtension((MarkupExtension)this.GetCurrentObjectData(), this.ParentObjectData, null);
			this.SetKeyOnContext(key, "Key", this.ParentContext, this.GrandParentContext);
			this.PopContext();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0008D91C File Offset: 0x0008BB1C
		internal virtual void ReadConstructorParameterTypeRecord(BamlConstructorParameterTypeRecord constructorParameterType)
		{
			this.SetConstructorParameter(this.MapTable.GetTypeFromId(constructorParameterType.TypeId));
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0008D938 File Offset: 0x0008BB38
		internal virtual void ReadContentPropertyRecord(BamlContentPropertyRecord bamlContentPropertyRecord)
		{
			object obj = null;
			short attributeId = bamlContentPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			if (currentObjectData != null)
			{
				short knownTypeIdFromType = BamlMapTable.GetKnownTypeIdFromType(currentObjectData.GetType());
				if (knownTypeIdFromType < 0)
				{
					obj = KnownTypes.GetCollectionForCPA(currentObjectData, (KnownElements)(-(KnownElements)knownTypeIdFromType));
				}
			}
			if (obj == null)
			{
				WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
				if (wpfPropertyDefinition.DependencyProperty != null)
				{
					if (typeof(IList).IsAssignableFrom(wpfPropertyDefinition.PropertyType))
					{
						obj = (((DependencyObject)currentObjectData).GetValue(wpfPropertyDefinition.DependencyProperty) as IList);
					}
					else
					{
						obj = wpfPropertyDefinition.DependencyProperty;
					}
				}
				if (obj == null && wpfPropertyDefinition.PropertyInfo != null)
				{
					if (wpfPropertyDefinition.IsInternal)
					{
						obj = (XamlTypeMapper.GetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, wpfPropertyDefinition.PropertyInfo, currentObjectData) as IList);
						if (obj == null)
						{
							bool allowProtected = this.ParserContext.RootElement is IComponentConnector && this.ParserContext.RootElement == currentObjectData;
							bool flag;
							if (!XamlTypeMapper.IsAllowedPropertySet(wpfPropertyDefinition.PropertyInfo, allowProtected, out flag))
							{
								this.ThrowException("ParserCantSetContentProperty", wpfPropertyDefinition.Name, wpfPropertyDefinition.PropertyInfo.ReflectedType.Name);
							}
						}
					}
					else
					{
						obj = (wpfPropertyDefinition.PropertyInfo.GetValue(currentObjectData, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, null, TypeConverterHelper.InvariantEnglishUS) as IList);
					}
					if (obj == null)
					{
						obj = wpfPropertyDefinition.PropertyInfo;
					}
				}
			}
			if (obj == null)
			{
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.CurrentContext.ContentProperty = obj;
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0008DABC File Offset: 0x0008BCBC
		internal virtual void ReadConstructorParametersStartRecord()
		{
			this.PushContext(ReaderFlags.ConstructorParams, null, null, 0);
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0008DACC File Offset: 0x0008BCCC
		internal virtual void ReadConstructorParametersEndRecord()
		{
			Type expectedType = this.ParentContext.ExpectedType;
			short num = -this.ParentContext.ExpectedTypeId;
			object obj = null;
			ArrayList arrayList = null;
			object obj2 = null;
			bool flag = false;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.CreateMarkupExtension, expectedType);
			}
			int num2;
			if (this.CurrentContext.CheckFlag(ReaderFlags.SingletonConstructorParam))
			{
				obj = this.CurrentContext.ObjectData;
				num2 = 1;
				if (num <= 602)
				{
					if (num != 189)
					{
						if (num == 602)
						{
							obj2 = new StaticExtension((string)obj);
							flag = true;
						}
					}
					else
					{
						obj2 = new DynamicResourceExtension(obj);
						flag = true;
					}
				}
				else if (num != 603)
				{
					if (num != 634)
					{
						if (num == 691)
						{
							Type type = obj as Type;
							if (type != null)
							{
								obj2 = new TypeExtension(type);
							}
							else
							{
								obj2 = new TypeExtension((string)obj);
							}
							flag = true;
						}
					}
					else
					{
						DependencyProperty dependencyProperty = obj as DependencyProperty;
						if (dependencyProperty == null)
						{
							string text = obj as string;
							Type targetType = this.ParserContext.TargetType;
							dependencyProperty = XamlTypeMapper.ParsePropertyName(this.ParserContext, text.Trim(), ref targetType);
							if (dependencyProperty == null)
							{
								this.ThrowException("ParserNoDPOnOwner", text, targetType.FullName);
							}
						}
						obj2 = new TemplateBindingExtension(dependencyProperty);
						flag = true;
					}
				}
				else
				{
					obj2 = new StaticResourceExtension(obj);
					flag = true;
				}
			}
			else
			{
				arrayList = (ArrayList)this.CurrentContext.ObjectData;
				num2 = arrayList.Count;
			}
			if (!flag)
			{
				XamlTypeMapper.ConstructorData constructors = this.XamlTypeMapper.GetConstructors(expectedType);
				ConstructorInfo[] constructors2 = constructors.Constructors;
				for (int i = 0; i < constructors2.Length; i++)
				{
					ConstructorInfo constructorInfo = constructors2[i];
					ParameterInfo[] parameters = constructors.GetParameters(i);
					if (parameters.Length == num2)
					{
						object[] array = new object[parameters.Length];
						if (num2 == 1)
						{
							this.ProcessConstructorParameter(parameters[0], obj, ref array[0]);
							if (num == 516)
							{
								obj2 = new RelativeSource((RelativeSourceMode)array[0]);
								flag = true;
							}
						}
						else
						{
							for (int j = 0; j < parameters.Length; j++)
							{
								this.ProcessConstructorParameter(parameters[j], arrayList[j], ref array[j]);
							}
						}
						if (!flag)
						{
							try
							{
								obj2 = constructorInfo.Invoke(array);
								flag = true;
							}
							catch (Exception innerException)
							{
								if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
								{
									throw;
								}
								TargetInvocationException ex = innerException as TargetInvocationException;
								if (ex != null)
								{
									innerException = ex.InnerException;
								}
								this.ThrowExceptionWithLine(SR.Get("ParserFailedToCreateFromConstructor", new object[]
								{
									constructorInfo.DeclaringType.Name
								}), innerException);
							}
						}
					}
				}
			}
			if (flag)
			{
				this.ParentContext.ObjectData = obj2;
				this.ParentContext.ExpectedType = null;
				this.PopContext();
			}
			else
			{
				this.ThrowException("ParserBadConstructorParams", expectedType.Name, num2.ToString(CultureInfo.CurrentCulture));
			}
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.CreateMarkupExtension, expectedType, obj2);
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0008DDE4 File Offset: 0x0008BFE4
		private void ProcessConstructorParameter(ParameterInfo paramInfo, object param, ref object paramArrayItem)
		{
			MarkupExtension markupExtension = param as MarkupExtension;
			if (markupExtension != null)
			{
				param = this.ProvideValueFromMarkupExtension(markupExtension, null, null);
			}
			if (param != null && paramInfo.ParameterType != typeof(object) && paramInfo.ParameterType != param.GetType())
			{
				TypeConverter typeConverter = this.XamlTypeMapper.GetTypeConverter(paramInfo.ParameterType);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.ProcessConstructorParameter, paramInfo.ParameterType, typeConverter.GetType(), param);
				}
				try
				{
					if (param is string)
					{
						object obj = typeConverter.ConvertFromString(this.TypeConvertContext, TypeConverterHelper.InvariantEnglishUS, param as string);
						param = obj;
					}
					else if (!paramInfo.ParameterType.IsAssignableFrom(param.GetType()))
					{
						object obj = typeConverter.ConvertTo(this.TypeConvertContext, TypeConverterHelper.InvariantEnglishUS, param, paramInfo.ParameterType);
						param = obj;
					}
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					this.ThrowExceptionWithLine(SR.Get("ParserCannotConvertString", new object[]
					{
						param.ToString(),
						paramInfo.ParameterType.FullName
					}), ex);
				}
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.ProcessConstructorParameter, paramInfo.ParameterType, typeConverter.GetType(), param);
				}
			}
			paramArrayItem = param;
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0008DF44 File Offset: 0x0008C144
		internal virtual void ReadDeferableContentStart(BamlDeferableContentStartRecord bamlRecord)
		{
			ResourceDictionary resourceDictionary = this.GetDictionaryFromContext(this.CurrentContext, true) as ResourceDictionary;
			if (resourceDictionary == null)
			{
				return;
			}
			Stream baseStream = this.BinaryReader.BaseStream;
			long position = baseStream.Position;
			long num = baseStream.Length - position;
			if (num < (long)bamlRecord.ContentSize)
			{
				this.ThrowException("ParserDeferContentAsync");
			}
			ArrayList arrayList;
			List<object[]> list;
			this.BaseReadDeferableContentStart(bamlRecord, out arrayList, out list);
			long position2 = baseStream.Position;
			int num2 = (int)((long)bamlRecord.ContentSize - position2 + position);
			if (!this.ParserContext.OwnsBamlStream)
			{
				byte[] buffer = new byte[num2];
				if (num2 > 0)
				{
					PackagingUtilities.ReliableRead(this.BinaryReader, buffer, 0, num2);
				}
				throw new NotImplementedException();
			}
			throw new NotImplementedException();
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0008DFF8 File Offset: 0x0008C1F8
		internal void BaseReadDeferableContentStart(BamlDeferableContentStartRecord bamlRecord, out ArrayList defKeyList, out List<object[]> staticResourceValuesList)
		{
			defKeyList = new ArrayList(Math.Max(5, bamlRecord.ContentSize / 400));
			staticResourceValuesList = new List<object[]>(defKeyList.Capacity);
			ArrayList arrayList = new ArrayList();
			BamlRecordType nextRecordType = this.GetNextRecordType();
			while (nextRecordType == BamlRecordType.DefAttributeKeyString || nextRecordType == BamlRecordType.DefAttributeKeyType || nextRecordType == BamlRecordType.KeyElementStart)
			{
				BamlRecord nextRecord = this.GetNextRecord();
				IBamlDictionaryKey bamlDictionaryKey = nextRecord as IBamlDictionaryKey;
				if (nextRecordType == BamlRecordType.KeyElementStart)
				{
					this.ReadKeyElementStartRecord((BamlKeyElementStartRecord)nextRecord);
					defKeyList.Add(nextRecord);
					bool flag = true;
					while (flag)
					{
						BamlRecord nextRecord2;
						if ((nextRecord2 = this.GetNextRecord()) == null)
						{
							break;
						}
						if (nextRecord2 is BamlKeyElementEndRecord)
						{
							object obj = this.GetCurrentObjectData();
							MarkupExtension markupExtension = obj as MarkupExtension;
							if (markupExtension != null)
							{
								obj = this.ProvideValueFromMarkupExtension(markupExtension, this.GetParentObjectData(), null);
							}
							bamlDictionaryKey.KeyObject = obj;
							this.PopContext();
							flag = false;
						}
						else
						{
							flag = this.ReadRecord(nextRecord2);
						}
					}
				}
				else
				{
					BamlDefAttributeKeyStringRecord bamlDefAttributeKeyStringRecord = nextRecord as BamlDefAttributeKeyStringRecord;
					if (bamlDefAttributeKeyStringRecord != null)
					{
						bamlDefAttributeKeyStringRecord.Value = this.MapTable.GetStringFromStringId((int)bamlDefAttributeKeyStringRecord.ValueId);
						bamlDictionaryKey.KeyObject = this.XamlTypeMapper.GetDictionaryKey(bamlDefAttributeKeyStringRecord.Value, this.ParserContext);
						defKeyList.Add(bamlDefAttributeKeyStringRecord);
					}
					else
					{
						BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = nextRecord as BamlDefAttributeKeyTypeRecord;
						if (bamlDefAttributeKeyTypeRecord != null)
						{
							bamlDictionaryKey.KeyObject = this.MapTable.GetTypeFromId(bamlDefAttributeKeyTypeRecord.TypeId);
							defKeyList.Add(bamlDefAttributeKeyTypeRecord);
						}
						else
						{
							this.ThrowException("ParserUnexpInBAML", nextRecord.RecordType.ToString(CultureInfo.CurrentCulture));
						}
					}
				}
				nextRecordType = this.GetNextRecordType();
				if (!this.ParserContext.InDeferredSection)
				{
					while (nextRecordType == BamlRecordType.StaticResourceStart || nextRecordType == BamlRecordType.OptimizedStaticResource)
					{
						BamlRecord nextRecord3 = this.GetNextRecord();
						if (nextRecordType == BamlRecordType.StaticResourceStart)
						{
							BamlStaticResourceStartRecord bamlElementRecord = (BamlStaticResourceStartRecord)nextRecord3;
							this.BaseReadElementStartRecord(bamlElementRecord);
							bool flag2 = true;
							while (flag2)
							{
								BamlRecord nextRecord4;
								if ((nextRecord4 = this.GetNextRecord()) == null)
								{
									break;
								}
								if (nextRecord4.RecordType == BamlRecordType.StaticResourceEnd)
								{
									StaticResourceExtension value = (StaticResourceExtension)this.GetCurrentObjectData();
									arrayList.Add(value);
									this.PopContext();
									flag2 = false;
								}
								else
								{
									flag2 = this.ReadRecord(nextRecord4);
								}
							}
						}
						else
						{
							StaticResourceExtension value2 = (StaticResourceExtension)this.GetExtensionValue((IOptimizedMarkupExtension)nextRecord3, null);
							arrayList.Add(value2);
						}
						nextRecordType = this.GetNextRecordType();
					}
				}
				else
				{
					object[] array = this.ParserContext.StaticResourcesStack[this.ParserContext.StaticResourcesStack.Count - 1];
					while (nextRecordType == BamlRecordType.StaticResourceId)
					{
						BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)this.GetNextRecord();
						DeferredResourceReference deferredResourceReference = (DeferredResourceReference)array[(int)bamlStaticResourceIdRecord.StaticResourceId];
						arrayList.Add(new StaticResourceHolder(deferredResourceReference.Key, deferredResourceReference));
						nextRecordType = this.GetNextRecordType();
					}
				}
				staticResourceValuesList.Add(arrayList.ToArray());
				arrayList.Clear();
				nextRecordType = this.GetNextRecordType();
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0008E2C8 File Offset: 0x0008C4C8
		protected virtual void ReadStaticResourceIdRecord(BamlStaticResourceIdRecord bamlStaticResourceIdRecord)
		{
			object staticResourceFromId = this.GetStaticResourceFromId(bamlStaticResourceIdRecord.StaticResourceId);
			this.PushContext((ReaderFlags)8193, staticResourceFromId, null, 0);
			this.ReadElementEndRecord(true);
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0008E2F8 File Offset: 0x0008C4F8
		protected virtual void ReadPropertyWithStaticResourceIdRecord(BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyWithStaticResourceIdRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			object staticResourceFromId = this.GetStaticResourceFromId(bamlPropertyWithStaticResourceIdRecord.StaticResourceId);
			this.BaseReadOptimizedMarkupExtension(currentObjectData, attributeId, propertyDefinition, staticResourceFromId);
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0008E378 File Offset: 0x0008C578
		internal StaticResourceHolder GetStaticResourceFromId(short staticResourceId)
		{
			object[] array = this.ParserContext.StaticResourcesStack[this.ParserContext.StaticResourcesStack.Count - 1];
			DeferredResourceReference deferredResourceReference = (DeferredResourceReference)array[(int)staticResourceId];
			return new StaticResourceHolder(deferredResourceReference.Key, deferredResourceReference);
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0008E3C0 File Offset: 0x0008C5C0
		internal virtual void ReadLiteralContentRecord(BamlLiteralContentRecord bamlLiteralContentRecord)
		{
			if (this.CurrentContext != null)
			{
				object obj = null;
				object obj2 = null;
				if (this.CurrentContext.ContentProperty != null)
				{
					obj = this.CurrentContext.ContentProperty;
					obj2 = this.CurrentContext.ObjectData;
				}
				else if (this.CurrentContext.ContextType == ReaderFlags.PropertyComplexClr || this.CurrentContext.ContextType == ReaderFlags.PropertyComplexDP)
				{
					obj = this.CurrentContext.ObjectData;
					obj2 = this.ParentContext.ObjectData;
				}
				IXmlSerializable xmlSerializable = null;
				PropertyInfo propertyInfo = obj as PropertyInfo;
				if (propertyInfo != null)
				{
					if (typeof(IXmlSerializable).IsAssignableFrom(propertyInfo.PropertyType))
					{
						xmlSerializable = (propertyInfo.GetValue(obj2, null) as IXmlSerializable);
					}
				}
				else
				{
					DependencyProperty dependencyProperty = obj as DependencyProperty;
					if (dependencyProperty != null && typeof(IXmlSerializable).IsAssignableFrom(dependencyProperty.PropertyType))
					{
						xmlSerializable = (((DependencyObject)obj2).GetValue(dependencyProperty) as IXmlSerializable);
					}
				}
				if (xmlSerializable != null)
				{
					FilteredXmlReader reader = new FilteredXmlReader(bamlLiteralContentRecord.Value, XmlNodeType.Element, this.ParserContext);
					xmlSerializable.ReadXml(reader);
					return;
				}
			}
			this.ThrowException("ParserUnexpInBAML", "BamlLiteralContent");
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0008E4E4 File Offset: 0x0008C6E4
		protected virtual void ReadPropertyComplexStartRecord(BamlPropertyComplexStartRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.ClrObject != this.CurrentContext.ContextType && ReaderFlags.DependencyObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "PropertyComplexStart");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, ReaderFlags.DependencyObject == this.CurrentContext.ContextType);
			if (wpfPropertyDefinition.DependencyProperty != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexDP, wpfPropertyDefinition.AttributeInfo, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.PropertyInfo != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.PropertyInfo, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.AttachedPropertySetter != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.AttachedPropertySetter, wpfPropertyDefinition.PropertyType, 0);
			}
			else if (wpfPropertyDefinition.AttachedPropertyGetter != null)
			{
				this.PushContext(ReaderFlags.PropertyComplexClr, wpfPropertyDefinition.AttachedPropertyGetter, wpfPropertyDefinition.PropertyType, 0);
			}
			else
			{
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.CurrentContext.ElementNameOrPropertyName = wpfPropertyDefinition.Name;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0008E61C File Offset: 0x0008C81C
		protected virtual void ReadPropertyComplexEndRecord()
		{
			this.PopContext();
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0008E624 File Offset: 0x0008C824
		internal DependencyProperty GetCustomDependencyPropertyValue(BamlPropertyCustomRecord bamlPropertyRecord)
		{
			Type type = null;
			return this.GetCustomDependencyPropertyValue(bamlPropertyRecord, out type);
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0008E63C File Offset: 0x0008C83C
		internal DependencyProperty GetCustomDependencyPropertyValue(BamlPropertyCustomRecord bamlPropertyRecord, out Type declaringType)
		{
			declaringType = null;
			short serializerTypeId = bamlPropertyRecord.SerializerTypeId;
			DependencyProperty dependencyProperty;
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				short memberId = this.BinaryReader.ReadInt16();
				string memberName = null;
				if (bamlPropertyRecord.IsValueTypeId)
				{
					memberName = this.BinaryReader.ReadString();
				}
				dependencyProperty = this.MapTable.GetDependencyPropertyValueFromId(memberId, memberName, out declaringType);
				if (dependencyProperty == null)
				{
					this.ThrowException("ParserCannotConvertPropertyValue", "Property", typeof(DependencyProperty).FullName);
				}
				bamlPropertyRecord.ValueObject = dependencyProperty;
				bamlPropertyRecord.ValueObjectSet = true;
			}
			else
			{
				dependencyProperty = (DependencyProperty)bamlPropertyRecord.ValueObject;
			}
			return dependencyProperty;
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0008E6D0 File Offset: 0x0008C8D0
		internal object GetCustomValue(BamlPropertyCustomRecord bamlPropertyRecord, Type propertyType, string propertyName)
		{
			object result = null;
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				Exception innerException = null;
				short serializerTypeId = bamlPropertyRecord.SerializerTypeId;
				try
				{
					if (serializerTypeId == 137)
					{
						result = this.GetCustomDependencyPropertyValue(bamlPropertyRecord);
					}
					else
					{
						result = bamlPropertyRecord.GetCustomValue(this.BinaryReader, propertyType, serializerTypeId, this);
					}
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					innerException = ex;
				}
				if (!bamlPropertyRecord.ValueObjectSet && !bamlPropertyRecord.IsRawEnumValueSet)
				{
					string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						propertyName,
						propertyType.FullName
					});
					this.ThrowExceptionWithLine(message, innerException);
				}
			}
			else
			{
				result = bamlPropertyRecord.ValueObject;
			}
			return result;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0008E780 File Offset: 0x0008C980
		protected virtual void ReadPropertyCustomRecord(BamlPropertyCustomRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "PropertyCustom");
			}
			object obj = null;
			object currentObjectData = this.GetCurrentObjectData();
			short attributeId = bamlPropertyRecord.AttributeId;
			WpfPropertyDefinition wpfPropertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			if (!bamlPropertyRecord.ValueObjectSet)
			{
				try
				{
					obj = this.GetCustomValue(bamlPropertyRecord, wpfPropertyDefinition.PropertyType, wpfPropertyDefinition.Name);
					goto IL_D2;
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
					{
						throw;
					}
					string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						wpfPropertyDefinition.Name,
						wpfPropertyDefinition.PropertyType.FullName
					});
					this.ThrowExceptionWithLine(message, ex);
					goto IL_D2;
				}
			}
			obj = bamlPropertyRecord.ValueObject;
			IL_D2:
			this.FreezeIfRequired(obj);
			if (wpfPropertyDefinition.DependencyProperty != null)
			{
				this.SetDependencyValue((DependencyObject)currentObjectData, wpfPropertyDefinition.DependencyProperty, obj);
				return;
			}
			if (wpfPropertyDefinition.PropertyInfo != null)
			{
				if (!wpfPropertyDefinition.IsInternal)
				{
					wpfPropertyDefinition.PropertyInfo.SetValue(currentObjectData, obj, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
					return;
				}
				if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, wpfPropertyDefinition.PropertyInfo, currentObjectData, obj))
				{
					this.ThrowException("ParserCantSetAttribute", "property", wpfPropertyDefinition.Name, "set");
					return;
				}
			}
			else
			{
				if (wpfPropertyDefinition.AttachedPropertySetter != null)
				{
					wpfPropertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
					{
						currentObjectData,
						obj
					});
					return;
				}
				this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
			}
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0008E944 File Offset: 0x0008CB44
		protected virtual void ReadPropertyRecord(BamlPropertyRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			this.ReadPropertyRecordBase(bamlPropertyRecord.Value, bamlPropertyRecord.AttributeId, 0);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0008E9A0 File Offset: 0x0008CBA0
		protected virtual void ReadPropertyConverterRecord(BamlPropertyWithConverterRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			this.ReadPropertyRecordBase(bamlPropertyRecord.Value, bamlPropertyRecord.AttributeId, bamlPropertyRecord.ConverterTypeId);
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x0008EA04 File Offset: 0x0008CC04
		protected virtual void ReadPropertyStringRecord(BamlPropertyStringReferenceRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			string propertyValueFromStringId = this.GetPropertyValueFromStringId(bamlPropertyRecord.StringId);
			this.ReadPropertyRecordBase(propertyValueFromStringId, bamlPropertyRecord.AttributeId, 0);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0008EA68 File Offset: 0x0008CC68
		private object GetInnerExtensionValue(IOptimizedMarkupExtension optimizedMarkupExtensionRecord)
		{
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			object result;
			if (optimizedMarkupExtensionRecord.IsValueTypeExtension)
			{
				result = this.MapTable.GetTypeFromId(valueId);
			}
			else if (optimizedMarkupExtensionRecord.IsValueStaticExtension)
			{
				result = this.GetStaticExtensionValue(valueId);
			}
			else
			{
				result = this.MapTable.GetStringFromStringId((int)valueId);
			}
			return result;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0008EAB8 File Offset: 0x0008CCB8
		private object GetStaticExtensionValue(short memberId)
		{
			object result = null;
			if (memberId < 0)
			{
				short num = -memberId;
				bool flag;
				num = SystemResourceKey.GetSystemResourceKeyIdFromBamlId(num, out flag);
				if (flag)
				{
					result = SystemResourceKey.GetResourceKey(num);
				}
				else
				{
					result = SystemResourceKey.GetResource(num);
				}
			}
			else
			{
				BamlAttributeInfoRecord attributeInfoFromId = this.MapTable.GetAttributeInfoFromId(memberId);
				if (attributeInfoFromId != null)
				{
					result = new StaticExtension
					{
						MemberType = this.MapTable.GetTypeFromId(attributeInfoFromId.OwnerTypeId),
						Member = attributeInfoFromId.Name
					}.ProvideValue(null);
				}
			}
			return result;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0008EB34 File Offset: 0x0008CD34
		internal virtual object GetExtensionValue(IOptimizedMarkupExtension optimizedMarkupExtensionRecord, string propertyName)
		{
			object obj = null;
			short valueId = optimizedMarkupExtensionRecord.ValueId;
			short extensionTypeId = optimizedMarkupExtensionRecord.ExtensionTypeId;
			if (extensionTypeId != 189)
			{
				if (extensionTypeId != 602)
				{
					if (extensionTypeId == 603)
					{
						object innerExtensionValue = this.GetInnerExtensionValue(optimizedMarkupExtensionRecord);
						obj = new StaticResourceExtension(innerExtensionValue);
					}
				}
				else
				{
					obj = this.GetStaticExtensionValue(valueId);
				}
			}
			else
			{
				object innerExtensionValue = this.GetInnerExtensionValue(optimizedMarkupExtensionRecord);
				obj = new DynamicResourceExtension(innerExtensionValue);
			}
			if (obj == null)
			{
				string parameter = string.Empty;
				if (extensionTypeId != 189)
				{
					if (extensionTypeId != 602)
					{
						if (extensionTypeId == 603)
						{
							parameter = typeof(StaticResourceExtension).FullName;
						}
					}
					else
					{
						parameter = typeof(StaticExtension).FullName;
					}
				}
				else
				{
					parameter = typeof(DynamicResourceExtension).FullName;
				}
				this.ThrowException("ParserCannotConvertPropertyValue", propertyName, parameter);
			}
			return obj;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x0008EC04 File Offset: 0x0008CE04
		protected virtual void ReadPropertyWithExtensionRecord(BamlPropertyWithExtensionRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			object extensionValue = this.GetExtensionValue(bamlPropertyRecord, propertyDefinition.Name);
			this.BaseReadOptimizedMarkupExtension(currentObjectData, attributeId, propertyDefinition, extensionValue);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0008EC88 File Offset: 0x0008CE88
		private void BaseReadOptimizedMarkupExtension(object element, short attributeId, WpfPropertyDefinition propertyDefinition, object value)
		{
			try
			{
				MarkupExtension markupExtension = value as MarkupExtension;
				if (markupExtension != null)
				{
					value = this.ProvideValueFromMarkupExtension(markupExtension, element, propertyDefinition.DpOrPiOrMi);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.TraceActivityItem(TraceMarkup.ProvideValue, new object[]
						{
							markupExtension,
							element,
							propertyDefinition.DpOrPiOrMi,
							value
						});
					}
				}
				if (!this.SetPropertyValue(element, propertyDefinition, value))
				{
					this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				string message = SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					propertyDefinition.Name,
					propertyDefinition.PropertyType.FullName
				});
				this.ThrowExceptionWithLine(message, innerException);
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0008ED68 File Offset: 0x0008CF68
		private bool SetPropertyValue(object o, WpfPropertyDefinition propertyDefinition, object value)
		{
			bool result = true;
			if (propertyDefinition.DependencyProperty != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.DependencyProperty.Name, value);
				}
				this.SetDependencyValue((DependencyObject)o, propertyDefinition.DependencyProperty, value);
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.DependencyProperty.Name, value);
				}
			}
			else if (propertyDefinition.PropertyInfo != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.PropertyInfo.Name, value);
				}
				if (propertyDefinition.IsInternal)
				{
					if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, propertyDefinition.PropertyInfo, o, value))
					{
						this.ThrowException("ParserCantSetAttribute", "property", propertyDefinition.Name, "set");
					}
				}
				else
				{
					propertyDefinition.PropertyInfo.SetValue(o, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				}
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.PropertyInfo.Name, value);
				}
			}
			else if (propertyDefinition.AttachedPropertySetter != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, o, propertyDefinition.AttachedPropertySetter.Name, value);
				}
				propertyDefinition.AttachedPropertySetter.Invoke(null, new object[]
				{
					o,
					value
				});
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, o, propertyDefinition.AttachedPropertySetter.Name, value);
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0008EF1C File Offset: 0x0008D11C
		protected virtual void ReadPropertyTypeRecord(BamlPropertyTypeReferenceRecord bamlPropertyRecord)
		{
			if (this.CurrentContext == null || (ReaderFlags.DependencyObject != this.CurrentContext.ContextType && ReaderFlags.ClrObject != this.CurrentContext.ContextType))
			{
				this.ThrowException("ParserUnexpInBAML", "Property");
			}
			short attributeId = bamlPropertyRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			Type typeFromId = this.MapTable.GetTypeFromId(bamlPropertyRecord.TypeId);
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			try
			{
				if (!this.SetPropertyValue(currentObjectData, propertyDefinition, typeFromId))
				{
					this.ThrowException("ParserCantGetDPOrPi", this.GetPropertyNameFromAttributeId(attributeId));
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentObjectData.GetType().FullName,
					propertyDefinition.Name,
					typeFromId.Name
				}), innerException);
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0008F030 File Offset: 0x0008D230
		private void ReadPropertyRecordBase(string attribValue, short attributeId, short converterTypeId)
		{
			if (this.CurrentContext.CreateUsingTypeConverter)
			{
				this.ParserContext.XmlSpace = attribValue;
				return;
			}
			object currentObjectData = this.GetCurrentObjectData();
			WpfPropertyDefinition propertyDefinition = new WpfPropertyDefinition(this, attributeId, currentObjectData is DependencyObject);
			try
			{
				switch (propertyDefinition.AttributeUsage)
				{
				case BamlAttributeUsage.XmlLang:
					this.ParserContext.XmlLang = attribValue;
					break;
				case BamlAttributeUsage.XmlSpace:
					this.ParserContext.XmlSpace = attribValue;
					break;
				case BamlAttributeUsage.RuntimeName:
					this.DoRegisterName(attribValue, currentObjectData);
					break;
				}
				if (propertyDefinition.DependencyProperty != null)
				{
					object obj = this.ParseProperty((DependencyObject)currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.DependencyProperty, attribValue, converterTypeId);
					if (obj != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj);
					}
				}
				else if (propertyDefinition.PropertyInfo != null)
				{
					object obj2 = this.ParseProperty(currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.PropertyInfo, attribValue, converterTypeId);
					if (obj2 != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj2);
					}
				}
				else if (propertyDefinition.AttachedPropertySetter != null)
				{
					object obj3 = this.ParseProperty(currentObjectData, propertyDefinition.PropertyType, propertyDefinition.Name, propertyDefinition.AttachedPropertySetter, attribValue, converterTypeId);
					if (obj3 != DependencyProperty.UnsetValue)
					{
						this.SetPropertyValue(currentObjectData, propertyDefinition, obj3);
					}
				}
				else
				{
					bool flag = false;
					object obj4 = null;
					bool flag2 = false;
					if (this._componentConnector != null && this._rootElement != null)
					{
						obj4 = this.GetREOrEiFromAttributeId(attributeId, out flag2, out flag);
					}
					if (obj4 != null)
					{
						if (flag)
						{
							RoutedEvent routedEvent = obj4 as RoutedEvent;
							Delegate @delegate = XamlTypeMapper.CreateDelegate(this.ParserContext, routedEvent.HandlerType, this.ParserContext.RootElement, attribValue);
							if (@delegate == null)
							{
								this.ThrowException("ParserCantCreateDelegate", routedEvent.HandlerType.Name, attribValue);
							}
							UIElement uielement = currentObjectData as UIElement;
							if (uielement != null)
							{
								uielement.AddHandler(routedEvent, @delegate);
							}
							else
							{
								ContentElement contentElement = currentObjectData as ContentElement;
								if (contentElement != null)
								{
									contentElement.AddHandler(routedEvent, @delegate);
								}
								else
								{
									UIElement3D uielement3D = currentObjectData as UIElement3D;
									uielement3D.AddHandler(routedEvent, @delegate);
								}
							}
						}
						else
						{
							EventInfo eventInfo = obj4 as EventInfo;
							Delegate @delegate = XamlTypeMapper.CreateDelegate(this.ParserContext, eventInfo.EventHandlerType, this.ParserContext.RootElement, attribValue);
							if (@delegate == null)
							{
								this.ThrowException("ParserCantCreateDelegate", eventInfo.EventHandlerType.Name, attribValue);
							}
							if (flag2)
							{
								if (!XamlTypeMapper.AddInternalEventHandler(this.ParserContext, this.ParserContext.RootElement, eventInfo, currentObjectData, @delegate))
								{
									this.ThrowException("ParserCantSetAttribute", "event", eventInfo.Name, "add");
								}
							}
							else
							{
								eventInfo.AddEventHandler(currentObjectData, @delegate);
							}
						}
					}
					else
					{
						this.ThrowException("ParserCantGetDPOrPi", propertyDefinition.Name);
					}
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentObjectData.GetType().FullName,
					propertyDefinition.AttributeInfo.Name,
					attribValue
				}), innerException);
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0008F384 File Offset: 0x0008D584
		private void DoRegisterName(string name, object element)
		{
			if (this.CurrentContext != null)
			{
				this.CurrentContext.ElementNameOrPropertyName = name;
			}
			if (this.ParserContext != null && this.ParserContext.NameScopeStack != null && this.ParserContext.NameScopeStack.Count != 0)
			{
				INameScope nameScope = this.ParserContext.NameScopeStack.Pop() as INameScope;
				if (NameScope.NameScopeFromObject(element) != null && this.ParserContext.NameScopeStack.Count != 0)
				{
					INameScope nameScope2 = this.ParserContext.NameScopeStack.Peek() as INameScope;
					if (nameScope2 != null)
					{
						nameScope2.RegisterName(name, element);
					}
				}
				else
				{
					nameScope.RegisterName(name, element);
				}
				this.ParserContext.NameScopeStack.Push(nameScope);
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0008F43C File Offset: 0x0008D63C
		protected void ReadPropertyArrayStartRecord(BamlPropertyArrayStartRecord bamlPropertyArrayStartRecord)
		{
			short attributeId = bamlPropertyArrayStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId, false);
			if (!bamlCollectionHolder.PropertyType.IsArray)
			{
				this.ThrowException("ParserNoMatchingArray", this.GetPropertyNameFromAttributeId(attributeId));
			}
			this.PushContext((ReaderFlags)20488, bamlCollectionHolder, bamlCollectionHolder.PropertyType, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0008F4A4 File Offset: 0x0008D6A4
		protected virtual void ReadPropertyArrayEndRecord()
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)this.GetCurrentObjectData();
			if (bamlCollectionHolder.Collection == null)
			{
				this.InitPropertyCollection(bamlCollectionHolder, this.CurrentContext);
			}
			ArrayExtension arrayExt = bamlCollectionHolder.ArrayExt;
			bamlCollectionHolder.Collection = this.ProvideValueFromMarkupExtension(arrayExt, bamlCollectionHolder, null);
			bamlCollectionHolder.SetPropertyValue();
			this.PopContext();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0008F4F4 File Offset: 0x0008D6F4
		protected virtual void ReadPropertyIListStartRecord(BamlPropertyIListStartRecord bamlPropertyIListStartRecord)
		{
			short attributeId = bamlPropertyIListStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId);
			Type type = bamlCollectionHolder.PropertyType;
			ReaderFlags readerFlags = ReaderFlags.Unknown;
			if (typeof(IList).IsAssignableFrom(type))
			{
				readerFlags = ReaderFlags.PropertyIList;
			}
			else if (BamlRecordManager.TreatAsIAddChild(type))
			{
				readerFlags = ReaderFlags.PropertyIAddChild;
				bamlCollectionHolder.Collection = bamlCollectionHolder.DefaultCollection;
				bamlCollectionHolder.ReadOnly = true;
			}
			else if (typeof(IEnumerable).IsAssignableFrom(type) && BamlRecordManager.AsIAddChild(this.GetCurrentObjectData()) != null)
			{
				readerFlags = ReaderFlags.PropertyIAddChild;
				bamlCollectionHolder.Collection = this.CurrentContext.ObjectData;
				bamlCollectionHolder.ReadOnly = true;
				type = this.CurrentContext.ObjectData.GetType();
			}
			else
			{
				this.ThrowException("ParserReadOnlyProp", bamlCollectionHolder.PropertyDefinition.Name);
			}
			this.PushContext(readerFlags | ReaderFlags.CollectionHolder, bamlCollectionHolder, type, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0008F5ED File Offset: 0x0008D7ED
		protected virtual void ReadPropertyIListEndRecord()
		{
			this.SetCollectionPropertyValue(this.CurrentContext);
			this.PopContext();
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0008F604 File Offset: 0x0008D804
		protected virtual void ReadPropertyIDictionaryStartRecord(BamlPropertyIDictionaryStartRecord bamlPropertyIDictionaryStartRecord)
		{
			short attributeId = bamlPropertyIDictionaryStartRecord.AttributeId;
			object currentObjectData = this.GetCurrentObjectData();
			BamlCollectionHolder bamlCollectionHolder = new BamlCollectionHolder(this, currentObjectData, attributeId);
			this.PushContext((ReaderFlags)28680, bamlCollectionHolder, bamlCollectionHolder.PropertyType, 0);
			this.CurrentContext.ElementNameOrPropertyName = bamlCollectionHolder.AttributeName;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0008F5ED File Offset: 0x0008D7ED
		protected virtual void ReadPropertyIDictionaryEndRecord()
		{
			this.SetCollectionPropertyValue(this.CurrentContext);
			this.PopContext();
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0008F64C File Offset: 0x0008D84C
		private void SetCollectionPropertyValue(ReaderContextStackData context)
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)context.ObjectData;
			if (bamlCollectionHolder.Collection == null)
			{
				this.InitPropertyCollection(bamlCollectionHolder, context);
			}
			if (!bamlCollectionHolder.ReadOnly && bamlCollectionHolder.Collection != bamlCollectionHolder.DefaultCollection)
			{
				bamlCollectionHolder.SetPropertyValue();
			}
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0008F694 File Offset: 0x0008D894
		private void InitPropertyCollection(BamlCollectionHolder holder, ReaderContextStackData context)
		{
			if (context.ContextType == ReaderFlags.PropertyArray)
			{
				holder.Collection = new ArrayExtension
				{
					Type = context.ExpectedType.GetElementType()
				};
			}
			else if (holder.DefaultCollection != null)
			{
				holder.Collection = holder.DefaultCollection;
			}
			else
			{
				this.ThrowException("ParserNullPropertyCollection", holder.PropertyDefinition.Name);
			}
			context.ExpectedType = null;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0008F704 File Offset: 0x0008D904
		private BamlCollectionHolder GetCollectionHolderFromContext(ReaderContextStackData context, bool toInsert)
		{
			BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)context.ObjectData;
			if (bamlCollectionHolder.Collection == null && toInsert)
			{
				this.InitPropertyCollection(bamlCollectionHolder, context);
			}
			if (toInsert && bamlCollectionHolder.IsClosed)
			{
				this.ThrowException("ParserPropertyCollectionClosed", bamlCollectionHolder.PropertyDefinition.Name);
			}
			return bamlCollectionHolder;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0008F758 File Offset: 0x0008D958
		protected IDictionary GetDictionaryFromContext(ReaderContextStackData context, bool toInsert)
		{
			IDictionary result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IDictionary))
				{
					result = (IDictionary)this.GetObjectDataFromContext(context);
				}
				else if (context.ContextType == ReaderFlags.PropertyIDictionary)
				{
					BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(context, toInsert);
					result = collectionHolderFromContext.Dictionary;
				}
			}
			return result;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0008F7A4 File Offset: 0x0008D9A4
		private IList GetListFromContext(ReaderContextStackData context)
		{
			IList result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IList))
				{
					result = (IList)this.GetObjectDataFromContext(context);
				}
				else if (context.ContextType == ReaderFlags.PropertyIList)
				{
					BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(context, true);
					result = collectionHolderFromContext.List;
				}
			}
			return result;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0008F7F0 File Offset: 0x0008D9F0
		private IAddChild GetIAddChildFromContext(ReaderContextStackData context)
		{
			IAddChild result = null;
			if (context != null)
			{
				if (context.CheckFlag(ReaderFlags.IAddChild))
				{
					result = BamlRecordManager.AsIAddChild(context.ObjectData);
				}
				else if (context.ContextType == ReaderFlags.PropertyIAddChild)
				{
					BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(context, false);
					result = BamlRecordManager.AsIAddChild(collectionHolderFromContext.Collection);
				}
			}
			return result;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0008F840 File Offset: 0x0008DA40
		private ArrayExtension GetArrayExtensionFromContext(ReaderContextStackData context)
		{
			ArrayExtension result = null;
			if (context != null)
			{
				result = (context.ObjectData as ArrayExtension);
				if (context.CheckFlag(ReaderFlags.ArrayExt))
				{
					result = (ArrayExtension)context.ObjectData;
				}
				else if (context.ContextType == ReaderFlags.PropertyArray)
				{
					BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(context, true);
					result = collectionHolderFromContext.ArrayExt;
				}
			}
			return result;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0008F894 File Offset: 0x0008DA94
		protected virtual void ReadDefAttributeRecord(BamlDefAttributeRecord bamlDefAttributeRecord)
		{
			bamlDefAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlDefAttributeRecord.NameId);
			if (bamlDefAttributeRecord.Name == "Key")
			{
				object dictionaryKey = this.XamlTypeMapper.GetDictionaryKey(bamlDefAttributeRecord.Value, this.ParserContext);
				if (dictionaryKey == null)
				{
					this.ThrowException("ParserNoResource", bamlDefAttributeRecord.Value);
				}
				this.SetKeyOnContext(dictionaryKey, bamlDefAttributeRecord.Value, this.CurrentContext, this.ParentContext);
				return;
			}
			if (bamlDefAttributeRecord.Name == "Uid" || bamlDefAttributeRecord.NameId == BamlMapTable.UidStringId)
			{
				if (this.CurrentContext == null)
				{
					return;
				}
				this.CurrentContext.Uid = bamlDefAttributeRecord.Value;
				UIElement uielement = this.CurrentContext.ObjectData as UIElement;
				if (uielement != null)
				{
					this.SetDependencyValue(uielement, UIElement.UidProperty, bamlDefAttributeRecord.Value);
					return;
				}
			}
			else
			{
				if (bamlDefAttributeRecord.Name == "Shared")
				{
					this.ThrowException("ParserDefSharedOnlyInCompiled");
					return;
				}
				if (bamlDefAttributeRecord.Name == "Name")
				{
					object currentObjectData = this.GetCurrentObjectData();
					if (currentObjectData != null)
					{
						this.DoRegisterName(bamlDefAttributeRecord.Value, currentObjectData);
						return;
					}
				}
				else
				{
					this.ThrowException("ParserUnknownDefAttribute", bamlDefAttributeRecord.Name);
				}
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0008F9CC File Offset: 0x0008DBCC
		protected virtual void ReadDefAttributeKeyTypeRecord(BamlDefAttributeKeyTypeRecord bamlDefAttributeRecord)
		{
			Type typeFromId = this.MapTable.GetTypeFromId(bamlDefAttributeRecord.TypeId);
			if (typeFromId == null)
			{
				this.ThrowException("ParserNoResource", "Key");
			}
			this.SetKeyOnContext(typeFromId, "Key", this.CurrentContext, this.ParentContext);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0008FA1C File Offset: 0x0008DC1C
		private void SetKeyOnContext(object key, string attributeName, ReaderContextStackData context, ReaderContextStackData parentContext)
		{
			try
			{
				this.GetDictionaryFromContext(parentContext, true);
			}
			catch (XamlParseException innerException)
			{
				if (parentContext.CheckFlag(ReaderFlags.CollectionHolder))
				{
					BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)parentContext.ObjectData;
					object objectData = context.ObjectData;
					if (objectData != null && objectData == bamlCollectionHolder.Dictionary)
					{
						this.ThrowExceptionWithLine(SR.Get("ParserKeyOnExplicitDictionary", new object[]
						{
							attributeName,
							objectData.GetType().ToString(),
							bamlCollectionHolder.PropertyDefinition.Name
						}), innerException);
					}
				}
				this.ThrowExceptionWithLine(SR.Get("ParserNoMatchingIDictionary", new object[]
				{
					attributeName
				}), innerException);
			}
			context.Key = key;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0008FAD4 File Offset: 0x0008DCD4
		protected virtual void ReadTextRecord(BamlTextRecord bamlTextRecord)
		{
			BamlTextWithIdRecord bamlTextWithIdRecord = bamlTextRecord as BamlTextWithIdRecord;
			if (bamlTextWithIdRecord != null)
			{
				bamlTextWithIdRecord.Value = this.MapTable.GetStringFromStringId((int)bamlTextWithIdRecord.ValueId);
			}
			if (this.CurrentContext == null)
			{
				this._componentConnector = null;
				this._rootElement = null;
				this.RootList.Add(bamlTextRecord.Value);
				return;
			}
			short converterTypeId = 0;
			BamlTextWithConverterRecord bamlTextWithConverterRecord = bamlTextRecord as BamlTextWithConverterRecord;
			if (bamlTextWithConverterRecord != null)
			{
				converterTypeId = bamlTextWithConverterRecord.ConverterTypeId;
			}
			ReaderFlags contextType = this.CurrentContext.ContextType;
			if (contextType <= ReaderFlags.PropertyComplexClr)
			{
				if (contextType != ReaderFlags.DependencyObject && contextType != ReaderFlags.ClrObject)
				{
					if (contextType == ReaderFlags.PropertyComplexClr)
					{
						if (null == this.CurrentContext.ExpectedType)
						{
							this.ThrowException("ParserNoComplexMulti", this.GetPropNameFrom(this.CurrentContext.ObjectData));
						}
						object objectFromString = this.GetObjectFromString(this.CurrentContext.ExpectedType, bamlTextRecord.Value, converterTypeId);
						if (DependencyProperty.UnsetValue != objectFromString)
						{
							this.SetClrComplexProperty(objectFromString);
							return;
						}
						this.ThrowException("ParserCantCreateTextComplexProp", this.CurrentContext.ExpectedType.FullName, bamlTextRecord.Value);
						return;
					}
				}
				else if (this.CurrentContext.CreateUsingTypeConverter)
				{
					object objectFromString2 = this.GetObjectFromString(this.CurrentContext.ExpectedType, bamlTextRecord.Value, converterTypeId);
					if (DependencyProperty.UnsetValue != objectFromString2)
					{
						this.CurrentContext.ObjectData = objectFromString2;
						this.CurrentContext.ExpectedType = null;
						return;
					}
					this.ThrowException("ParserCannotConvertString", bamlTextRecord.Value, this.CurrentContext.ExpectedType.FullName);
					return;
				}
				else
				{
					object currentObjectData = this.GetCurrentObjectData();
					if (currentObjectData == null)
					{
						this.ThrowException("ParserCantCreateInstanceType", this.CurrentContext.ExpectedType.FullName);
					}
					IAddChild iaddChildFromContext = this.GetIAddChildFromContext(this.CurrentContext);
					if (iaddChildFromContext != null)
					{
						iaddChildFromContext.AddText(bamlTextRecord.Value);
						return;
					}
					if (this.CurrentContext.ContentProperty != null)
					{
						this.AddToContentProperty(currentObjectData, this.CurrentContext.ContentProperty, bamlTextRecord.Value);
						return;
					}
					this.ThrowException("ParserIAddChildText", currentObjectData.GetType().FullName, bamlTextRecord.Value);
					return;
				}
			}
			else if (contextType <= ReaderFlags.PropertyIList)
			{
				if (contextType != ReaderFlags.PropertyComplexDP)
				{
					if (contextType == ReaderFlags.PropertyIList)
					{
						BamlCollectionHolder collectionHolderFromContext = this.GetCollectionHolderFromContext(this.CurrentContext, true);
						if (collectionHolderFromContext.List == null)
						{
							this.ThrowException("ParserNoMatchingIList", "?");
						}
						collectionHolderFromContext.List.Add(bamlTextRecord.Value);
						return;
					}
				}
				else
				{
					if (null == this.CurrentContext.ExpectedType)
					{
						this.ThrowException("ParserNoComplexMulti", this.GetPropNameFrom(this.CurrentContext.ObjectData));
					}
					BamlAttributeInfoRecord bamlAttributeInfoRecord = this.CurrentContext.ObjectData as BamlAttributeInfoRecord;
					object obj = this.ParseProperty((DependencyObject)this.GetParentObjectData(), bamlAttributeInfoRecord.DP.PropertyType, bamlAttributeInfoRecord.DP.Name, bamlAttributeInfoRecord.DP, bamlTextRecord.Value, converterTypeId);
					if (DependencyProperty.UnsetValue != obj)
					{
						this.SetDependencyComplexProperty(obj);
						return;
					}
					this.ThrowException("ParserCantCreateTextComplexProp", bamlAttributeInfoRecord.OwnerType.FullName, bamlTextRecord.Value);
					return;
				}
			}
			else
			{
				if (contextType == ReaderFlags.PropertyIAddChild)
				{
					BamlCollectionHolder collectionHolderFromContext2 = this.GetCollectionHolderFromContext(this.CurrentContext, true);
					IAddChild addChild = BamlRecordManager.AsIAddChild(collectionHolderFromContext2.Collection);
					if (addChild == null)
					{
						this.ThrowException("ParserNoMatchingIList", "?");
					}
					addChild.AddText(bamlTextRecord.Value);
					return;
				}
				if (contextType == ReaderFlags.ConstructorParams)
				{
					this.SetConstructorParameter(bamlTextRecord.Value);
					return;
				}
			}
			this.ThrowException("ParserUnexpInBAML", "Text");
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0008FE68 File Offset: 0x0008E068
		protected virtual void ReadPresentationOptionsAttributeRecord(BamlPresentationOptionsAttributeRecord bamlPresentationOptionsAttributeRecord)
		{
			bamlPresentationOptionsAttributeRecord.Name = this.MapTable.GetStringFromStringId((int)bamlPresentationOptionsAttributeRecord.NameId);
			if (bamlPresentationOptionsAttributeRecord.Name == "Freeze")
			{
				bool freezeFreezables = bool.Parse(bamlPresentationOptionsAttributeRecord.Value);
				this._parserContext.FreezeFreezables = freezeFreezables;
				return;
			}
			this.ThrowException("ParserUnknownPresentationOptionsAttribute", bamlPresentationOptionsAttributeRecord.Name);
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0008FEC8 File Offset: 0x0008E0C8
		private void SetDependencyComplexProperty(object o)
		{
			object parentObjectData = this.GetParentObjectData();
			BamlAttributeInfoRecord attribInfo = (BamlAttributeInfoRecord)this.GetCurrentObjectData();
			this.SetDependencyComplexProperty(parentObjectData, attribInfo, o);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0008FEF4 File Offset: 0x0008E0F4
		private void SetDependencyComplexProperty(object currentTarget, BamlAttributeInfoRecord attribInfo, object o)
		{
			DependencyProperty dependencyProperty = (currentTarget is DependencyObject) ? attribInfo.DP : null;
			PropertyInfo propInfo = attribInfo.PropInfo;
			MethodInfo methodInfo = null;
			try
			{
				MarkupExtension markupExtension = o as MarkupExtension;
				if (markupExtension != null)
				{
					o = this.ProvideValueFromMarkupExtension(markupExtension, currentTarget, dependencyProperty);
				}
				Type propertyType = null;
				if (dependencyProperty != null)
				{
					propertyType = dependencyProperty.PropertyType;
				}
				else if (propInfo != null)
				{
					propertyType = propInfo.PropertyType;
				}
				else
				{
					if (attribInfo.AttachedPropertySetter == null)
					{
						this.XamlTypeMapper.UpdateAttachedPropertySetter(attribInfo);
					}
					methodInfo = attribInfo.AttachedPropertySetter;
					if (methodInfo != null)
					{
						propertyType = methodInfo.GetParameters()[1].ParameterType;
					}
				}
				o = this.OptionallyMakeNullable(propertyType, o, attribInfo.Name);
				if (dependencyProperty != null)
				{
					this.SetDependencyValue((DependencyObject)currentTarget, dependencyProperty, o);
				}
				else if (propInfo != null)
				{
					propInfo.SetValue(currentTarget, o, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				}
				else if (methodInfo != null)
				{
					methodInfo.Invoke(null, new object[]
					{
						currentTarget,
						o
					});
				}
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					currentTarget.GetType().FullName,
					attribInfo.Name,
					o
				}), innerException);
			}
			this.CurrentContext.ExpectedType = null;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0009006C File Offset: 0x0008E26C
		internal static bool IsNullable(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == BamlRecordReader.NullableType;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00090088 File Offset: 0x0008E288
		internal object OptionallyMakeNullable(Type propertyType, object o, string propName)
		{
			object result = o;
			if (!BamlRecordReader.TryOptionallyMakeNullable(propertyType, propName, ref result))
			{
				this.ThrowException("ParserBadNullableType", propName, propertyType.GetGenericArguments()[0].Name, o.GetType().FullName);
			}
			return result;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000900C8 File Offset: 0x0008E2C8
		internal static bool TryOptionallyMakeNullable(Type propertyType, string propName, ref object o)
		{
			if (o != null && BamlRecordReader.IsNullable(propertyType) && !(o is Expression) && !(o is MarkupExtension))
			{
				Type left = propertyType.GetGenericArguments()[0];
				if (left != o.GetType())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00090110 File Offset: 0x0008E310
		internal virtual void SetClrComplexPropertyCore(object parentObject, object value, MemberInfo memberInfo)
		{
			MarkupExtension markupExtension = value as MarkupExtension;
			if (markupExtension != null)
			{
				value = this.ProvideValueFromMarkupExtension(markupExtension, parentObject, memberInfo);
			}
			if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				value = this.OptionallyMakeNullable(propertyInfo.PropertyType, value, propertyInfo.Name);
				propertyInfo.SetValue(parentObject, value, BindingFlags.Default, null, null, TypeConverterHelper.InvariantEnglishUS);
				return;
			}
			MethodInfo methodInfo = (MethodInfo)memberInfo;
			value = this.OptionallyMakeNullable(methodInfo.GetParameters()[1].ParameterType, value, methodInfo.Name.Substring("Set".Length));
			methodInfo.Invoke(null, new object[]
			{
				parentObject,
				value
			});
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000901B0 File Offset: 0x0008E3B0
		private void SetClrComplexProperty(object o)
		{
			MemberInfo memberInfo = (MemberInfo)this.GetCurrentObjectData();
			object parentObjectData = this.GetParentObjectData();
			this.SetClrComplexProperty(parentObjectData, memberInfo, o);
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000901DC File Offset: 0x0008E3DC
		private void SetClrComplexProperty(object parentObject, MemberInfo memberInfo, object o)
		{
			try
			{
				this.SetClrComplexPropertyCore(parentObject, o, memberInfo);
			}
			catch (Exception innerException)
			{
				if (CriticalExceptions.IsCriticalException(innerException) || innerException is XamlParseException)
				{
					throw;
				}
				TargetInvocationException ex = innerException as TargetInvocationException;
				if (ex != null)
				{
					innerException = ex.InnerException;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotSetValue", new object[]
				{
					parentObject.GetType().FullName,
					memberInfo.Name,
					o
				}), innerException);
			}
			this.CurrentContext.ExpectedType = null;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x00090268 File Offset: 0x0008E468
		private void SetConstructorParameter(object o)
		{
			MarkupExtension markupExtension = o as MarkupExtension;
			if (markupExtension != null)
			{
				o = this.ProvideValueFromMarkupExtension(markupExtension, null, null);
			}
			if (this.CurrentContext.ObjectData == null)
			{
				this.CurrentContext.ObjectData = o;
				this.CurrentContext.SetFlag(ReaderFlags.SingletonConstructorParam);
				return;
			}
			if (this.CurrentContext.CheckFlag(ReaderFlags.SingletonConstructorParam))
			{
				ArrayList arrayList = new ArrayList(2);
				arrayList.Add(this.CurrentContext.ObjectData);
				arrayList.Add(o);
				this.CurrentContext.ObjectData = arrayList;
				this.CurrentContext.ClearFlag(ReaderFlags.SingletonConstructorParam);
				return;
			}
			ArrayList arrayList2 = (ArrayList)this.CurrentContext.ObjectData;
			arrayList2.Add(o);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x00090320 File Offset: 0x0008E520
		protected void SetXmlnsOnCurrentObject(BamlXmlnsPropertyRecord xmlnsRecord)
		{
			DependencyObject dependencyObject = this.CurrentContext.ObjectData as DependencyObject;
			if (dependencyObject != null)
			{
				XmlnsDictionary xmlnsDictionary = XmlAttributeProperties.GetXmlnsDictionary(dependencyObject);
				if (xmlnsDictionary != null)
				{
					xmlnsDictionary.Unseal();
					xmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
					xmlnsDictionary.Seal();
					return;
				}
				xmlnsDictionary = new XmlnsDictionary();
				xmlnsDictionary[xmlnsRecord.Prefix] = xmlnsRecord.XmlNamespace;
				xmlnsDictionary.Seal();
				XmlAttributeProperties.SetXmlnsDictionary(dependencyObject, xmlnsDictionary);
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00090390 File Offset: 0x0008E590
		internal object ParseProperty(object element, Type propertyType, string propertyName, object dpOrPi, string attribValue, short converterTypeId)
		{
			object obj = null;
			try
			{
				obj = this.XamlTypeMapper.ParseProperty(element, propertyType, propertyName, dpOrPi, this.TypeConvertContext, this.ParserContext, attribValue, converterTypeId);
				this.FreezeIfRequired(obj);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowPropertyParseError(ex, propertyName, attribValue, element, propertyType);
			}
			if (DependencyProperty.UnsetValue == obj)
			{
				this.ThrowException("ParserNullReturned", propertyName, attribValue);
			}
			return obj;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00090414 File Offset: 0x0008E614
		private void ThrowPropertyParseError(Exception e, string propertyName, string attribValue, object element, Type propertyType)
		{
			string message = string.Empty;
			if (this.FindResourceInParserStack(attribValue.Trim(), false, false) == DependencyProperty.UnsetValue)
			{
				if (propertyType == typeof(Type))
				{
					message = SR.Get("ParserErrorParsingAttribType", new object[]
					{
						propertyName,
						attribValue
					});
				}
				else
				{
					message = SR.Get("ParserErrorParsingAttrib", new object[]
					{
						propertyName,
						attribValue,
						propertyType.Name
					});
				}
			}
			else
			{
				message = SR.Get("ParserErrorParsingAttribType", new object[]
				{
					propertyName,
					attribValue
				});
			}
			this.ThrowExceptionWithLine(message, e);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000904B0 File Offset: 0x0008E6B0
		private object GetObjectFromString(Type type, string s, short converterTypeId)
		{
			object unsetValue = DependencyProperty.UnsetValue;
			return this.ParserContext.XamlTypeMapper.ParseProperty(null, type, string.Empty, null, this.TypeConvertContext, this.ParserContext, s, converterTypeId);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000904EC File Offset: 0x0008E6EC
		private static object Lookup(IDictionary dictionary, object key, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary resourceDictionary;
			if (allowDeferredResourceReference && (resourceDictionary = (dictionary as ResourceDictionary)) != null)
			{
				bool flag;
				return resourceDictionary.FetchResource(key, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			if (!mustReturnDeferredResourceReference)
			{
				return dictionary[key];
			}
			return new DeferredResourceReferenceHolder(key, dictionary[key]);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0009052C File Offset: 0x0008E72C
		internal object FindResourceInParserStack(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj = DependencyProperty.UnsetValue;
			ParserStack parserStack = this.ReaderContextStack;
			BamlRecordReader bamlRecordReader = this;
			while (parserStack != null)
			{
				for (int i = parserStack.Count - 1; i >= 0; i--)
				{
					ReaderContextStackData readerContextStackData = (ReaderContextStackData)parserStack[i];
					IDictionary dictionaryFromContext = this.GetDictionaryFromContext(readerContextStackData, false);
					if (dictionaryFromContext != null && dictionaryFromContext.Contains(resourceNameObject))
					{
						obj = BamlRecordReader.Lookup(dictionaryFromContext, resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					else if (readerContextStackData.ContextType == ReaderFlags.DependencyObject)
					{
						DependencyObject d = (DependencyObject)readerContextStackData.ObjectData;
						FrameworkElement frameworkElement;
						FrameworkContentElement frameworkContentElement;
						Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
						if (frameworkElement != null)
						{
							obj = frameworkElement.FindResourceOnSelf(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
						}
						else if (frameworkContentElement != null)
						{
							obj = frameworkContentElement.FindResourceOnSelf(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
						}
					}
					else if (readerContextStackData.CheckFlag(ReaderFlags.StyleObject))
					{
						Style style = (Style)readerContextStackData.ObjectData;
						obj = style.FindResource(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					else if (readerContextStackData.CheckFlag(ReaderFlags.FrameworkTemplateObject))
					{
						FrameworkTemplate frameworkTemplate = (FrameworkTemplate)readerContextStackData.ObjectData;
						obj = frameworkTemplate.FindResource(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					}
					if (obj != DependencyProperty.UnsetValue)
					{
						return obj;
					}
				}
				bool flag = false;
				while (bamlRecordReader._previousBamlRecordReader != null)
				{
					bamlRecordReader = bamlRecordReader._previousBamlRecordReader;
					if (bamlRecordReader.ReaderContextStack != parserStack)
					{
						parserStack = bamlRecordReader.ReaderContextStack;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					parserStack = null;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00090680 File Offset: 0x0008E880
		private object FindResourceInRootOrAppOrTheme(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj;
			if (!SystemResources.IsSystemResourcesParsing)
			{
				object obj2;
				obj = FrameworkElement.FindResourceFromAppOrSystem(resourceNameObject, out obj2, false, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			else
			{
				obj = SystemResources.FindResourceInternal(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			if (obj != null)
			{
				return obj;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000906B8 File Offset: 0x0008E8B8
		internal object FindResourceInParentChain(object resourceNameObject, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			object obj = this.FindResourceInParserStack(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = this.FindResourceInRootOrAppOrTheme(resourceNameObject, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			if (obj == DependencyProperty.UnsetValue && mustReturnDeferredResourceReference)
			{
				obj = new DeferredResourceReferenceHolder(resourceNameObject, DependencyProperty.UnsetValue);
			}
			return obj;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000906FC File Offset: 0x0008E8FC
		internal object LoadResource(string resourceNameString)
		{
			string keyString = resourceNameString.Substring(1, resourceNameString.Length - 2);
			object dictionaryKey = this.XamlTypeMapper.GetDictionaryKey(keyString, this.ParserContext);
			if (dictionaryKey == null)
			{
				this.ThrowException("ParserNoResource", resourceNameString);
			}
			object obj = this.FindResourceInParentChain(dictionaryKey, false, false);
			if (obj == DependencyProperty.UnsetValue)
			{
				this.ThrowException("ParserNoResource", "{" + dictionaryKey.ToString() + "}");
			}
			return obj;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x00090770 File Offset: 0x0008E970
		private object GetObjectDataFromContext(ReaderContextStackData context)
		{
			if (context.ObjectData == null && null != context.ExpectedType)
			{
				context.ObjectData = this.CreateInstanceFromType(context.ExpectedType, context.ExpectedTypeId, true);
				if (context.ObjectData == null)
				{
					this.ThrowException("ParserCantCreateInstanceType", context.ExpectedType.FullName);
				}
				context.ExpectedType = null;
				this.ElementInitialize(context.ObjectData, null);
			}
			return context.ObjectData;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000907E5 File Offset: 0x0008E9E5
		internal object GetCurrentObjectData()
		{
			return this.GetObjectDataFromContext(this.CurrentContext);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000907F3 File Offset: 0x0008E9F3
		protected object GetParentObjectData()
		{
			return this.GetObjectDataFromContext(this.ParentContext);
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x00090801 File Offset: 0x0008EA01
		internal void PushContext(ReaderFlags contextFlags, object contextData, Type expectedType, short expectedTypeId)
		{
			this.PushContext(contextFlags, contextData, expectedType, expectedTypeId, false);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x00090810 File Offset: 0x0008EA10
		internal void PushContext(ReaderFlags contextFlags, object contextData, Type expectedType, short expectedTypeId, bool createUsingTypeConverter)
		{
			List<ReaderContextStackData> stackDataFactoryCache = BamlRecordReader._stackDataFactoryCache;
			ReaderContextStackData readerContextStackData;
			lock (stackDataFactoryCache)
			{
				if (BamlRecordReader._stackDataFactoryCache.Count == 0)
				{
					readerContextStackData = new ReaderContextStackData();
				}
				else
				{
					readerContextStackData = BamlRecordReader._stackDataFactoryCache[BamlRecordReader._stackDataFactoryCache.Count - 1];
					BamlRecordReader._stackDataFactoryCache.RemoveAt(BamlRecordReader._stackDataFactoryCache.Count - 1);
				}
			}
			readerContextStackData.ContextFlags = contextFlags;
			readerContextStackData.ObjectData = contextData;
			readerContextStackData.ExpectedType = expectedType;
			readerContextStackData.ExpectedTypeId = expectedTypeId;
			readerContextStackData.CreateUsingTypeConverter = createUsingTypeConverter;
			this.ReaderContextStack.Push(readerContextStackData);
			this.ParserContext.PushScope();
			INameScope nameScope = NameScope.NameScopeFromObject(contextData);
			if (nameScope != null)
			{
				this.ParserContext.NameScopeStack.Push(nameScope);
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000908E4 File Offset: 0x0008EAE4
		internal void PopContext()
		{
			ReaderContextStackData readerContextStackData = (ReaderContextStackData)this.ReaderContextStack.Pop();
			INameScope nameScope = NameScope.NameScopeFromObject(readerContextStackData.ObjectData);
			if (nameScope != null)
			{
				this.ParserContext.NameScopeStack.Pop();
			}
			this.ParserContext.PopScope();
			readerContextStackData.ClearData();
			List<ReaderContextStackData> stackDataFactoryCache = BamlRecordReader._stackDataFactoryCache;
			lock (stackDataFactoryCache)
			{
				BamlRecordReader._stackDataFactoryCache.Add(readerContextStackData);
			}
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0009096C File Offset: 0x0008EB6C
		private Uri GetBaseUri()
		{
			Uri uri = this.ParserContext.BaseUri;
			if (uri == null)
			{
				uri = BindUriHelper.BaseUri;
			}
			else if (!uri.IsAbsoluteUri)
			{
				uri = new Uri(BindUriHelper.BaseUri, uri);
			}
			return uri;
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000909AC File Offset: 0x0008EBAC
		private bool ElementInitialize(object element, string name)
		{
			bool result = false;
			ISupportInitialize supportInitialize = element as ISupportInitialize;
			if (supportInitialize != null)
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.BeginInit, supportInitialize);
				}
				supportInitialize.BeginInit();
				result = true;
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.BeginInit, supportInitialize);
				}
			}
			if (name != null)
			{
				this.DoRegisterName(name, element);
			}
			IUriContext uriContext = element as IUriContext;
			if (uriContext != null)
			{
				uriContext.BaseUri = this.GetBaseUri();
			}
			else if (element is Application)
			{
				((Application)element).ApplicationMarkupBaseUri = this.GetBaseUri();
			}
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				UIElement uielement2 = uielement;
				int persistId = this._persistId + 1;
				this._persistId = persistId;
				uielement2.SetPersistId(persistId);
			}
			if (this.CurrentContext == null)
			{
				IComponentConnector componentConnector = null;
				if (this._componentConnector == null)
				{
					componentConnector = (this._componentConnector = (element as IComponentConnector));
					if (this._componentConnector != null)
					{
						if (this.ParserContext.RootElement == null)
						{
							this.ParserContext.RootElement = element;
						}
						this._componentConnector.Connect(0, element);
					}
				}
				this._rootElement = element;
				DependencyObject dependencyObject = element as DependencyObject;
				if (!(element is INameScope) && this.ParserContext.NameScopeStack.Count == 0 && dependencyObject != null)
				{
					NameScope nameScope = null;
					if (componentConnector != null)
					{
						nameScope = (NameScope.GetNameScope(dependencyObject) as NameScope);
					}
					if (nameScope == null)
					{
						nameScope = new NameScope();
						NameScope.SetNameScope(dependencyObject, nameScope);
					}
				}
				if (dependencyObject != null)
				{
					Uri baseUri = this.GetBaseUri();
					this.SetDependencyValue(dependencyObject, BaseUriHelper.BaseUriProperty, baseUri);
				}
			}
			return result;
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x00090B20 File Offset: 0x0008ED20
		private void ElementEndInit(ref object element)
		{
			try
			{
				ISupportInitialize supportInitialize = element as ISupportInitialize;
				if (supportInitialize != null)
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.EndInit, supportInitialize);
					}
					supportInitialize.EndInit();
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.EndInit, supportInitialize);
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				ReaderContextStackData parentContext = this.ParentContext;
				ReaderFlags readerFlags = (parentContext != null) ? parentContext.ContextType : ReaderFlags.Unknown;
				if (readerFlags == ReaderFlags.PropertyComplexClr || readerFlags == ReaderFlags.PropertyComplexDP || readerFlags == ReaderFlags.PropertyIList || readerFlags == ReaderFlags.PropertyIDictionary || readerFlags == ReaderFlags.PropertyArray || readerFlags == ReaderFlags.PropertyIAddChild)
				{
					IProvidePropertyFallback providePropertyFallback = this.GrandParentObjectData as IProvidePropertyFallback;
					if (providePropertyFallback != null)
					{
						string elementNameOrPropertyName = parentContext.ElementNameOrPropertyName;
						if (providePropertyFallback.CanProvidePropertyFallback(elementNameOrPropertyName))
						{
							element = providePropertyFallback.ProvidePropertyFallback(elementNameOrPropertyName, ex);
							this.CurrentContext.ObjectData = element;
							return;
						}
					}
				}
				this.ThrowExceptionWithLine(SR.Get("ParserFailedEndInit"), ex);
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x00090C2C File Offset: 0x0008EE2C
		private void SetPropertyValueToParent(bool fromStartTag)
		{
			bool flag;
			this.SetPropertyValueToParent(fromStartTag, out flag);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00090C44 File Offset: 0x0008EE44
		private void SetPropertyValueToParent(bool fromStartTag, out bool isMarkupExtension)
		{
			isMarkupExtension = false;
			object p = null;
			ReaderContextStackData currentContext = this.CurrentContext;
			ReaderContextStackData parentContext = this.ParentContext;
			if (currentContext == null || !currentContext.NeedToAddToTree || (ReaderFlags.DependencyObject != currentContext.ContextType && ReaderFlags.ClrObject != currentContext.ContextType))
			{
				return;
			}
			object obj = null;
			try
			{
				obj = this.GetCurrentObjectData();
				this.FreezeIfRequired(obj);
				if (parentContext == null)
				{
					if (this.RootList.Count == 0)
					{
						this.RootList.Add(obj);
					}
					currentContext.MarkAddedToTree();
				}
				else if (this.CheckExplicitCollectionTag(ref isMarkupExtension))
				{
					currentContext.MarkAddedToTree();
				}
				else
				{
					object parentObjectData = this.GetParentObjectData();
					IDictionary dictionaryFromContext = this.GetDictionaryFromContext(parentContext, true);
					if (dictionaryFromContext != null)
					{
						if (!fromStartTag)
						{
							obj = this.GetElementValue(obj, dictionaryFromContext, null, ref isMarkupExtension);
							if (currentContext.Key == null)
							{
								this.ThrowException("ParserNoDictionaryKey");
							}
							dictionaryFromContext.Add(currentContext.Key, obj);
							currentContext.MarkAddedToTree();
						}
					}
					else
					{
						IList listFromContext = this.GetListFromContext(parentContext);
						if (listFromContext != null)
						{
							obj = this.GetElementValue(obj, listFromContext, null, ref isMarkupExtension);
							listFromContext.Add(obj);
							currentContext.MarkAddedToTree();
						}
						else
						{
							ArrayExtension arrayExtensionFromContext = this.GetArrayExtensionFromContext(parentContext);
							if (arrayExtensionFromContext != null)
							{
								obj = this.GetElementValue(obj, arrayExtensionFromContext, null, ref isMarkupExtension);
								arrayExtensionFromContext.AddChild(obj);
								if (TraceMarkup.IsEnabled)
								{
									TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToArray, p, parentContext.ElementNameOrPropertyName, obj);
								}
								currentContext.MarkAddedToTree();
							}
							else
							{
								IAddChild iaddChildFromContext = this.GetIAddChildFromContext(parentContext);
								if (iaddChildFromContext != null)
								{
									obj = this.GetElementValue(obj, iaddChildFromContext, null, ref isMarkupExtension);
									string text = obj as string;
									if (text != null)
									{
										iaddChildFromContext.AddText(text);
									}
									else
									{
										iaddChildFromContext.AddChild(obj);
									}
									if (TraceMarkup.IsEnabled)
									{
										TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToAddChild, p, obj);
									}
									currentContext.MarkAddedToTree();
								}
								else
								{
									object contentProperty = parentContext.ContentProperty;
									if (contentProperty != null)
									{
										obj = this.GetElementValue(obj, parentContext.ObjectData, contentProperty, ref isMarkupExtension);
										this.AddToContentProperty(parentObjectData, contentProperty, obj);
										currentContext.MarkAddedToTree();
									}
									else if (parentContext.ContextType == ReaderFlags.PropertyComplexClr)
									{
										object objectDataFromContext = this.GetObjectDataFromContext(this.GrandParentContext);
										MemberInfo memberInfo = (MemberInfo)this.GetParentObjectData();
										this.SetClrComplexProperty(objectDataFromContext, memberInfo, obj);
										currentContext.MarkAddedToTree();
									}
									else if (parentContext.ContextType == ReaderFlags.PropertyComplexDP)
									{
										object objectDataFromContext2 = this.GetObjectDataFromContext(this.GrandParentContext);
										BamlAttributeInfoRecord attribInfo = (BamlAttributeInfoRecord)this.GetParentObjectData();
										this.SetDependencyComplexProperty(objectDataFromContext2, attribInfo, obj);
										currentContext.MarkAddedToTree();
									}
									else
									{
										Type parentType = this.GetParentType();
										string text2 = (parentType == null) ? string.Empty : parentType.FullName;
										if (obj == null)
										{
											this.ThrowException("ParserCannotAddAnyChildren", text2);
										}
										else
										{
											this.ThrowException("ParserCannotAddAnyChildren2", text2, obj.GetType().FullName);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				Type parentType2 = this.GetParentType();
				string text3 = (parentType2 == null) ? string.Empty : parentType2.FullName;
				if (obj == null)
				{
					this.ThrowException("ParserCannotAddAnyChildren", text3);
				}
				else
				{
					this.ThrowException("ParserCannotAddAnyChildren2", text3, obj.GetType().FullName);
				}
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00090F78 File Offset: 0x0008F178
		private Type GetParentType()
		{
			ReaderContextStackData parentContext = this.ParentContext;
			object obj = this.GetParentObjectData();
			if (parentContext.CheckFlag(ReaderFlags.CollectionHolder))
			{
				obj = ((BamlCollectionHolder)obj).Collection;
			}
			if (obj != null)
			{
				return obj.GetType();
			}
			if (parentContext.ExpectedType != null)
			{
				return parentContext.ExpectedType;
			}
			return null;
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00090FC8 File Offset: 0x0008F1C8
		private object GetElementValue(object element, object parent, object contentProperty, ref bool isMarkupExtension)
		{
			MarkupExtension markupExtension = element as MarkupExtension;
			if (markupExtension != null)
			{
				isMarkupExtension = true;
				element = this.ProvideValueFromMarkupExtension(markupExtension, parent, contentProperty);
				this.CurrentContext.ObjectData = element;
			}
			return element;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00090FFC File Offset: 0x0008F1FC
		private bool CheckExplicitCollectionTag(ref bool isMarkupExtension)
		{
			bool result = false;
			ReaderContextStackData parentContext = this.ParentContext;
			if (parentContext != null && parentContext.CheckFlag(ReaderFlags.CollectionHolder) && parentContext.ExpectedType != null)
			{
				BamlCollectionHolder bamlCollectionHolder = (BamlCollectionHolder)parentContext.ObjectData;
				if (!bamlCollectionHolder.IsClosed && !bamlCollectionHolder.ReadOnly)
				{
					ReaderContextStackData currentContext = this.CurrentContext;
					object obj = currentContext.ObjectData;
					Type c;
					if (currentContext.CheckFlag(ReaderFlags.ArrayExt))
					{
						c = ((ArrayExtension)obj).Type.MakeArrayType();
						isMarkupExtension = false;
					}
					else
					{
						obj = this.GetElementValue(obj, this.GrandParentObjectData, bamlCollectionHolder.PropertyDefinition.DependencyProperty, ref isMarkupExtension);
						c = ((obj == null) ? null : obj.GetType());
					}
					if (isMarkupExtension || parentContext.ExpectedType.IsAssignableFrom(c))
					{
						bamlCollectionHolder.Collection = obj;
						bamlCollectionHolder.IsClosed = true;
						parentContext.ExpectedType = null;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000910E4 File Offset: 0x0008F2E4
		private void AddToContentProperty(object container, object contentProperty, object value)
		{
			IList list = contentProperty as IList;
			object p = null;
			try
			{
				if (list != null)
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.AddValueToList, p, string.Empty, value);
					}
					list.Add(value);
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.AddValueToList, p, string.Empty, value);
					}
				}
				else
				{
					DependencyProperty dependencyProperty = contentProperty as DependencyProperty;
					if (dependencyProperty != null)
					{
						DependencyObject dependencyObject = container as DependencyObject;
						if (dependencyObject == null)
						{
							this.ThrowException("ParserParentDO", value.ToString());
						}
						if (TraceMarkup.IsEnabled)
						{
							TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, p, dependencyProperty.Name, value);
						}
						this.SetDependencyValue(dependencyObject, dependencyProperty, value);
						if (TraceMarkup.IsEnabled)
						{
							TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, p, dependencyProperty.Name, value);
						}
					}
					else
					{
						PropertyInfo propertyInfo = contentProperty as PropertyInfo;
						if (propertyInfo != null)
						{
							if (TraceMarkup.IsEnabled)
							{
								TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.SetPropertyValue, p, propertyInfo.Name, value);
							}
							if (!XamlTypeMapper.SetInternalPropertyValue(this.ParserContext, this.ParserContext.RootElement, propertyInfo, container, value))
							{
								this.ThrowException("ParserCantSetContentProperty", propertyInfo.Name, propertyInfo.ReflectedType.Name);
							}
							if (TraceMarkup.IsEnabled)
							{
								TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetPropertyValue, p, propertyInfo.Name, value);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserCannotAddChild", new object[]
				{
					value.GetType().Name,
					container.GetType().Name
				}), ex);
			}
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.SetCPA, p, value);
			}
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000912CC File Offset: 0x0008F4CC
		internal string GetPropertyNameFromAttributeId(short id)
		{
			if (this.MapTable != null)
			{
				return this.MapTable.GetAttributeNameFromId(id);
			}
			return null;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000912E4 File Offset: 0x0008F4E4
		internal string GetPropertyValueFromStringId(short id)
		{
			string result = null;
			if (this.MapTable != null)
			{
				result = this.MapTable.GetStringFromStringId((int)id);
			}
			return result;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0009130C File Offset: 0x0008F50C
		private XamlSerializer CreateSerializer(BamlTypeInfoWithSerializerRecord typeWithSerializerInfo)
		{
			if (typeWithSerializerInfo.SerializerTypeId < 0)
			{
				return (XamlSerializer)this.MapTable.CreateKnownTypeFromId(typeWithSerializerInfo.SerializerTypeId);
			}
			if (typeWithSerializerInfo.SerializerType == null)
			{
				typeWithSerializerInfo.SerializerType = this.MapTable.GetTypeFromId(typeWithSerializerInfo.SerializerTypeId);
			}
			return (XamlSerializer)this.CreateInstanceFromType(typeWithSerializerInfo.SerializerType, typeWithSerializerInfo.SerializerTypeId, false);
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x00091378 File Offset: 0x0008F578
		internal object GetREOrEiFromAttributeId(short id, out bool isInternal, out bool isRE)
		{
			object obj = null;
			isRE = true;
			isInternal = false;
			BamlAttributeInfoRecord bamlAttributeInfoRecord = null;
			if (this.MapTable != null)
			{
				bamlAttributeInfoRecord = this.MapTable.GetAttributeInfoFromId(id);
				if (bamlAttributeInfoRecord != null)
				{
					obj = bamlAttributeInfoRecord.Event;
					if (obj == null)
					{
						obj = bamlAttributeInfoRecord.EventInfo;
						if (obj == null)
						{
							bamlAttributeInfoRecord.Event = this.MapTable.GetRoutedEvent(bamlAttributeInfoRecord);
							obj = bamlAttributeInfoRecord.Event;
							if (obj == null)
							{
								object currentObjectData = this.GetCurrentObjectData();
								Type type = currentObjectData.GetType();
								if (ReflectionHelper.IsPublicType(type))
								{
									bamlAttributeInfoRecord.EventInfo = this.ParserContext.XamlTypeMapper.GetClrEventInfo(type, bamlAttributeInfoRecord.Name);
								}
								if (bamlAttributeInfoRecord.EventInfo == null)
								{
									bamlAttributeInfoRecord.EventInfo = type.GetEvent(bamlAttributeInfoRecord.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									if (bamlAttributeInfoRecord.EventInfo != null)
									{
										bamlAttributeInfoRecord.IsInternal = true;
									}
								}
								obj = bamlAttributeInfoRecord.EventInfo;
								isRE = false;
							}
						}
						else
						{
							isRE = false;
						}
					}
				}
			}
			if (bamlAttributeInfoRecord != null)
			{
				isInternal = bamlAttributeInfoRecord.IsInternal;
			}
			return obj;
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x00091468 File Offset: 0x0008F668
		private string GetPropNameFrom(object PiOrAttribInfo)
		{
			BamlAttributeInfoRecord bamlAttributeInfoRecord = PiOrAttribInfo as BamlAttributeInfoRecord;
			if (bamlAttributeInfoRecord != null)
			{
				return bamlAttributeInfoRecord.OwnerType.Name + "." + bamlAttributeInfoRecord.Name;
			}
			PropertyInfo propertyInfo = PiOrAttribInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.DeclaringType.Name + "." + propertyInfo.Name;
			}
			return string.Empty;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000914CC File Offset: 0x0008F6CC
		protected void ThrowException(string id)
		{
			this.ThrowExceptionWithLine(SR.Get(id), null);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x000914DB File Offset: 0x0008F6DB
		protected internal void ThrowException(string id, string parameter)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter
			}), null);
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000914F4 File Offset: 0x0008F6F4
		protected void ThrowException(string id, string parameter1, string parameter2)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			}), null);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x00091511 File Offset: 0x0008F711
		protected void ThrowException(string id, string parameter1, string parameter2, string parameter3)
		{
			this.ThrowExceptionWithLine(SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			}), null);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00091533 File Offset: 0x0008F733
		internal void ThrowExceptionWithLine(string message, Exception innerException)
		{
			XamlParseException.ThrowException(this.ParserContext, this.LineNumber, this.LinePosition, message, innerException);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00091550 File Offset: 0x0008F750
		internal object CreateInstanceFromType(Type type, short typeId, bool throwOnFail)
		{
			bool flag = true;
			if (typeId >= 0)
			{
				BamlTypeInfoRecord typeInfoFromId = this.MapTable.GetTypeInfoFromId(typeId);
				if (typeInfoFromId != null)
				{
					flag = !typeInfoFromId.IsInternalType;
				}
			}
			if (flag)
			{
				if (!ReflectionHelper.IsPublicType(type))
				{
					this.ThrowException("ParserNotMarkedPublic", type.Name);
				}
			}
			else if (!ReflectionHelper.IsInternalType(type))
			{
				this.ThrowException("ParserNotAllowedInternalType", type.Name);
			}
			object result;
			try
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInFTypBegin);
				object obj = null;
				try
				{
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.CreateObject, type);
					}
					if (type != typeof(string))
					{
						if (typeId < 0)
						{
							obj = this.MapTable.CreateKnownTypeFromId(typeId);
						}
						else if (flag)
						{
							obj = Activator.CreateInstance(type);
						}
						else
						{
							obj = XamlTypeMapper.CreateInternalInstance(this.ParserContext, type);
							if (obj == null && throwOnFail)
							{
								this.ThrowException("ParserNotAllowedInternalType", type.Name);
							}
						}
					}
					if (TraceMarkup.IsEnabled)
					{
						TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.CreateObject, type, obj);
					}
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseRdrCrInFTypEnd);
				}
				result = obj;
			}
			catch (MissingMethodException innerException)
			{
				if (throwOnFail)
				{
					if (this.ParentContext != null && this.ParentContext.ContextType == ReaderFlags.PropertyComplexDP)
					{
						BamlAttributeInfoRecord bamlAttributeInfoRecord = this.GetParentObjectData() as BamlAttributeInfoRecord;
						this.ThrowException("ParserNoDefaultPropConstructor", type.Name, bamlAttributeInfoRecord.DP.Name);
					}
					else
					{
						this.ThrowExceptionWithLine(SR.Get("ParserNoDefaultConstructor", new object[]
						{
							type.Name
						}), innerException);
					}
				}
				result = null;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is XamlParseException)
				{
					throw;
				}
				this.ThrowExceptionWithLine(SR.Get("ParserErrorCreatingInstance", new object[]
				{
					type.Name,
					type.Assembly.FullName
				}), ex);
				result = null;
			}
			return result;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00091748 File Offset: 0x0008F948
		internal void FreezeIfRequired(object element)
		{
			if (this._parserContext.FreezeFreezables)
			{
				Freezable freezable = element as Freezable;
				if (freezable != null)
				{
					freezable.Freeze();
				}
			}
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00091772 File Offset: 0x0008F972
		internal void PreParsedBamlReset()
		{
			this.PreParsedCurrentRecord = this.PreParsedRecordsStart;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00091780 File Offset: 0x0008F980
		protected internal void SetPreviousBamlRecordReader(BamlRecordReader previousBamlRecordReader)
		{
			this._previousBamlRecordReader = previousBamlRecordReader;
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x00091789 File Offset: 0x0008F989
		// (set) Token: 0x06001E53 RID: 7763 RVA: 0x00091791 File Offset: 0x0008F991
		internal BamlRecord PreParsedRecordsStart
		{
			get
			{
				return this._preParsedBamlRecordsStart;
			}
			set
			{
				this._preParsedBamlRecordsStart = value;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0009179A File Offset: 0x0008F99A
		// (set) Token: 0x06001E55 RID: 7765 RVA: 0x000917A2 File Offset: 0x0008F9A2
		internal BamlRecord PreParsedCurrentRecord
		{
			get
			{
				return this._preParsedIndexRecord;
			}
			set
			{
				this._preParsedIndexRecord = value;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x000917AB File Offset: 0x0008F9AB
		// (set) Token: 0x06001E57 RID: 7767 RVA: 0x000917B4 File Offset: 0x0008F9B4
		internal Stream BamlStream
		{
			get
			{
				return this._bamlStream;
			}
			set
			{
				this._bamlStream = value;
				if (this._bamlStream is ReaderStream)
				{
					this._xamlReaderStream = (ReaderStream)this._bamlStream;
				}
				else
				{
					this._xamlReaderStream = null;
				}
				if (this.BamlStream != null)
				{
					this._binaryReader = new BamlBinaryReader(this.BamlStream, new UTF8Encoding());
				}
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x0009180D File Offset: 0x0008FA0D
		internal BamlBinaryReader BinaryReader
		{
			get
			{
				return this._binaryReader;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x00091815 File Offset: 0x0008FA15
		internal XamlTypeMapper XamlTypeMapper
		{
			get
			{
				return this.ParserContext.XamlTypeMapper;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x00091822 File Offset: 0x0008FA22
		// (set) Token: 0x06001E5B RID: 7771 RVA: 0x0009182A File Offset: 0x0008FA2A
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
			set
			{
				this._parserContext = value;
				this._typeConvertContext = null;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0009183A File Offset: 0x0008FA3A
		internal TypeConvertContext TypeConvertContext
		{
			get
			{
				if (this._typeConvertContext == null)
				{
					this._typeConvertContext = new TypeConvertContext(this.ParserContext);
				}
				return this._typeConvertContext;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0009185B File Offset: 0x0008FA5B
		// (set) Token: 0x06001E5E RID: 7774 RVA: 0x00091863 File Offset: 0x0008FA63
		internal XamlParseMode XamlParseMode
		{
			get
			{
				return this._parseMode;
			}
			set
			{
				this._parseMode = value;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0009186C File Offset: 0x0008FA6C
		// (set) Token: 0x06001E60 RID: 7776 RVA: 0x00091874 File Offset: 0x0008FA74
		internal int MaxAsyncRecords
		{
			get
			{
				return this._maxAsyncRecords;
			}
			set
			{
				this._maxAsyncRecords = value;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0009187D File Offset: 0x0008FA7D
		internal BamlMapTable MapTable
		{
			get
			{
				return this.ParserContext.MapTable;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0009188A File Offset: 0x0008FA8A
		internal XmlnsDictionary XmlnsDictionary
		{
			get
			{
				return this.ParserContext.XmlnsDictionary;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x00091897 File Offset: 0x0008FA97
		internal ReaderContextStackData CurrentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.CurrentContext;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x000918A9 File Offset: 0x0008FAA9
		internal ReaderContextStackData ParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.ParentContext;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x000918BC File Offset: 0x0008FABC
		internal object ParentObjectData
		{
			get
			{
				ReaderContextStackData parentContext = this.ParentContext;
				if (parentContext != null)
				{
					return parentContext.ObjectData;
				}
				return null;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x000918DB File Offset: 0x0008FADB
		internal ReaderContextStackData GrandParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.GrandParentContext;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000918F0 File Offset: 0x0008FAF0
		internal object GrandParentObjectData
		{
			get
			{
				ReaderContextStackData grandParentContext = this.GrandParentContext;
				if (grandParentContext != null)
				{
					return grandParentContext.ObjectData;
				}
				return null;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x0009190F File Offset: 0x0008FB0F
		internal ReaderContextStackData GreatGrandParentContext
		{
			get
			{
				return (ReaderContextStackData)this.ReaderContextStack.GreatGrandParentContext;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x00091921 File Offset: 0x0008FB21
		internal ParserStack ReaderContextStack
		{
			get
			{
				return this._contextStack;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x00091929 File Offset: 0x0008FB29
		internal BamlRecordManager BamlRecordManager
		{
			get
			{
				if (this._bamlRecordManager == null)
				{
					this._bamlRecordManager = new BamlRecordManager();
				}
				return this._bamlRecordManager;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x00091944 File Offset: 0x0008FB44
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x0009194C File Offset: 0x0008FB4C
		internal bool EndOfDocument
		{
			get
			{
				return this._endOfDocument;
			}
			set
			{
				this._endOfDocument = value;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x00091955 File Offset: 0x0008FB55
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x0009195D File Offset: 0x0008FB5D
		internal object RootElement
		{
			get
			{
				return this._rootElement;
			}
			set
			{
				this._rootElement = value;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x00091966 File Offset: 0x0008FB66
		// (set) Token: 0x06001E70 RID: 7792 RVA: 0x0009196E File Offset: 0x0008FB6E
		internal IComponentConnector ComponentConnector
		{
			get
			{
				return this._componentConnector;
			}
			set
			{
				this._componentConnector = value;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x00091977 File Offset: 0x0008FB77
		private ReaderStream XamlReaderStream
		{
			get
			{
				return this._xamlReaderStream;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x00091921 File Offset: 0x0008FB21
		// (set) Token: 0x06001E73 RID: 7795 RVA: 0x0009197F File Offset: 0x0008FB7F
		internal ParserStack ContextStack
		{
			get
			{
				return this._contextStack;
			}
			set
			{
				this._contextStack = value;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x00091988 File Offset: 0x0008FB88
		// (set) Token: 0x06001E75 RID: 7797 RVA: 0x00091995 File Offset: 0x0008FB95
		internal int LineNumber
		{
			get
			{
				return this.ParserContext.LineNumber;
			}
			set
			{
				this.ParserContext.LineNumber = value;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000919A3 File Offset: 0x0008FBA3
		// (set) Token: 0x06001E77 RID: 7799 RVA: 0x000919B0 File Offset: 0x0008FBB0
		internal int LinePosition
		{
			get
			{
				return this.ParserContext.LinePosition;
			}
			set
			{
				this.ParserContext.LinePosition = value;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x000919BE File Offset: 0x0008FBBE
		// (set) Token: 0x06001E79 RID: 7801 RVA: 0x000919CB File Offset: 0x0008FBCB
		internal bool IsDebugBamlStream
		{
			get
			{
				return this.ParserContext.IsDebugBamlStream;
			}
			set
			{
				this.ParserContext.IsDebugBamlStream = value;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001E7A RID: 7802 RVA: 0x000919D9 File Offset: 0x0008FBD9
		internal long StreamPosition
		{
			get
			{
				return this._bamlStream.Position;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x000919E6 File Offset: 0x0008FBE6
		private long StreamLength
		{
			get
			{
				return this._bamlStream.Length;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001E7C RID: 7804 RVA: 0x000919F3 File Offset: 0x0008FBF3
		// (set) Token: 0x06001E7D RID: 7805 RVA: 0x000919FB File Offset: 0x0008FBFB
		internal bool IsRootAlreadyLoaded
		{
			get
			{
				return this._isRootAlreadyLoaded;
			}
			set
			{
				this._isRootAlreadyLoaded = value;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x00091A04 File Offset: 0x0008FC04
		internal BamlRecordReader PreviousBamlRecordReader
		{
			get
			{
				return this._previousBamlRecordReader;
			}
		}

		// Token: 0x04001482 RID: 5250
		private static Type NullableType = typeof(Nullable<>);

		// Token: 0x04001483 RID: 5251
		private IComponentConnector _componentConnector;

		// Token: 0x04001484 RID: 5252
		private object _rootElement;

		// Token: 0x04001485 RID: 5253
		private bool _bamlAsForest;

		// Token: 0x04001486 RID: 5254
		private bool _isRootAlreadyLoaded;

		// Token: 0x04001487 RID: 5255
		private ArrayList _rootList;

		// Token: 0x04001488 RID: 5256
		private ParserContext _parserContext;

		// Token: 0x04001489 RID: 5257
		private TypeConvertContext _typeConvertContext;

		// Token: 0x0400148A RID: 5258
		private int _persistId;

		// Token: 0x0400148B RID: 5259
		private ParserStack _contextStack;

		// Token: 0x0400148C RID: 5260
		private XamlParseMode _parseMode;

		// Token: 0x0400148D RID: 5261
		private int _maxAsyncRecords;

		// Token: 0x0400148E RID: 5262
		private Stream _bamlStream;

		// Token: 0x0400148F RID: 5263
		private ReaderStream _xamlReaderStream;

		// Token: 0x04001490 RID: 5264
		private BamlBinaryReader _binaryReader;

		// Token: 0x04001491 RID: 5265
		private BamlRecordManager _bamlRecordManager;

		// Token: 0x04001492 RID: 5266
		private BamlRecord _preParsedBamlRecordsStart;

		// Token: 0x04001493 RID: 5267
		private BamlRecord _preParsedIndexRecord;

		// Token: 0x04001494 RID: 5268
		private bool _endOfDocument;

		// Token: 0x04001495 RID: 5269
		private bool _buildTopDown;

		// Token: 0x04001496 RID: 5270
		private BamlRecordReader _previousBamlRecordReader;

		// Token: 0x04001497 RID: 5271
		private static List<ReaderContextStackData> _stackDataFactoryCache = new List<ReaderContextStackData>();
	}
}
