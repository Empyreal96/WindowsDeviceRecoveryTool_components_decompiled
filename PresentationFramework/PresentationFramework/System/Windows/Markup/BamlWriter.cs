using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000211 RID: 529
	internal class BamlWriter : IParserHelper
	{
		// Token: 0x060020E0 RID: 8416 RVA: 0x00096E48 File Offset: 0x00095048
		public BamlWriter(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(SR.Get("BamlWriterBadStream"));
			}
			this._parserContext = new ParserContext();
			if (this._parserContext.XamlTypeMapper == null)
			{
				this._parserContext.XamlTypeMapper = new BamlWriterXamlTypeMapper(XmlParserDefaults.GetDefaultAssemblyNames(), XmlParserDefaults.GetDefaultNamespaceMaps());
			}
			this._xamlTypeMapper = this._parserContext.XamlTypeMapper;
			this._bamlRecordWriter = new BamlRecordWriter(stream, this._parserContext, true);
			this._startDocumentWritten = false;
			this._depth = 0;
			this._closed = false;
			this._nodeTypeStack = new ParserStack();
			this._assemblies = new Hashtable(7);
			this._extensionParser = new MarkupExtensionParser(this, this._parserContext);
			this._markupExtensionNodes = new ArrayList();
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00096F20 File Offset: 0x00095120
		public void Close()
		{
			this._bamlRecordWriter.BamlStream.Close();
			this._closed = true;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00096F39 File Offset: 0x00095139
		string IParserHelper.LookupNamespace(string prefix)
		{
			return this._parserContext.XmlnsDictionary[prefix];
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00096F4C File Offset: 0x0009514C
		bool IParserHelper.GetElementType(bool extensionFirst, string localName, string namespaceURI, ref string assemblyName, ref string typeFullName, ref Type baseType, ref Type serializerType)
		{
			bool result = false;
			assemblyName = string.Empty;
			typeFullName = string.Empty;
			serializerType = null;
			baseType = null;
			if (namespaceURI == null || localName == null)
			{
				return false;
			}
			TypeAndSerializer typeAndSerializer = this._xamlTypeMapper.GetTypeAndSerializer(namespaceURI, localName, null);
			if (typeAndSerializer == null)
			{
				typeAndSerializer = this._xamlTypeMapper.GetTypeAndSerializer(namespaceURI, localName + "Extension", null);
			}
			if (typeAndSerializer != null && typeAndSerializer.ObjectType != null)
			{
				serializerType = typeAndSerializer.SerializerType;
				baseType = typeAndSerializer.ObjectType;
				typeFullName = baseType.FullName;
				assemblyName = baseType.Assembly.FullName;
				result = true;
			}
			return result;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IParserHelper.CanResolveLocalAssemblies()
		{
			return false;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00096FE8 File Offset: 0x000951E8
		public void WriteStartDocument()
		{
			if (this._closed)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterClosed"));
			}
			if (this._startDocumentWritten)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterStartDoc"));
			}
			XamlDocumentStartNode xamlDocumentNode = new XamlDocumentStartNode(0, 0, this._depth);
			this._bamlRecordWriter.WriteDocumentStart(xamlDocumentNode);
			this._startDocumentWritten = true;
			this.Push(BamlRecordType.DocumentStart);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00097050 File Offset: 0x00095250
		public void WriteEndDocument()
		{
			this.VerifyEndTagState(BamlRecordType.DocumentStart, BamlRecordType.DocumentEnd);
			XamlDocumentEndNode xamlDocumentEndNode = new XamlDocumentEndNode(0, 0, this._depth);
			this._bamlRecordWriter.WriteDocumentEnd(xamlDocumentEndNode);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0009707F File Offset: 0x0009527F
		public void WriteConnectionId(int connectionId)
		{
			this.VerifyWriteState();
			this._bamlRecordWriter.WriteConnectionId(connectionId);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00097094 File Offset: 0x00095294
		public void WriteStartElement(string assemblyName, string typeFullName, bool isInjected, bool useTypeConverter)
		{
			this.VerifyWriteState();
			this._dpProperty = null;
			this._parserContext.PushScope();
			this.ProcessMarkupExtensionNodes();
			Type type = this.GetType(assemblyName, typeFullName);
			Type xamlSerializerForType = this._xamlTypeMapper.GetXamlSerializerForType(type);
			this.Push(BamlRecordType.ElementStart, type);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth;
			this._depth = depth + 1;
			XamlElementStartNode xamlElementStartNode = new XamlElementStartNode(lineNumber, linePosition, depth, assemblyName, typeFullName, type, xamlSerializerForType);
			xamlElementStartNode.IsInjected = isInjected;
			xamlElementStartNode.CreateUsingTypeConverter = useTypeConverter;
			this._bamlRecordWriter.WriteElementStart(xamlElementStartNode);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00097118 File Offset: 0x00095318
		public void WriteEndElement()
		{
			this.VerifyEndTagState(BamlRecordType.ElementStart, BamlRecordType.ElementEnd);
			this.ProcessMarkupExtensionNodes();
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlElementEndNode xamlElementEndNode = new XamlElementEndNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteElementEnd(xamlElementEndNode);
			this._parserContext.PopScope();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00097164 File Offset: 0x00095364
		public void WriteStartConstructor()
		{
			this.VerifyWriteState();
			this.Push(BamlRecordType.ConstructorParametersStart);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlConstructorParametersStartNode xamlConstructorParametersStartNode = new XamlConstructorParametersStartNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteConstructorParametersStart(xamlConstructorParametersStartNode);
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000971A4 File Offset: 0x000953A4
		public void WriteEndConstructor()
		{
			this.VerifyEndTagState(BamlRecordType.ConstructorParametersStart, BamlRecordType.ConstructorParametersEnd);
			int lineNumber = 0;
			int linePosition = 0;
			int depth = this._depth - 1;
			this._depth = depth;
			XamlConstructorParametersEndNode xamlConstructorParametersEndNode = new XamlConstructorParametersEndNode(lineNumber, linePosition, depth);
			this._bamlRecordWriter.WriteConstructorParametersEnd(xamlConstructorParametersEndNode);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000971E0 File Offset: 0x000953E0
		public void WriteProperty(string assemblyName, string ownerTypeFullName, string propName, string value, BamlAttributeUsage propUsage)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterNoInElement", new object[]
				{
					"WriteProperty",
					bamlRecordType.ToString()
				}));
			}
			object obj;
			Type declaringType;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out obj, out declaringType);
			AttributeData attributeData = this._extensionParser.IsMarkupExtensionAttribute(declaringType, propName, ref value, 0, 0, 0, obj);
			if (attributeData == null)
			{
				XamlPropertyNode xamlPropertyNode = new XamlPropertyNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, value, propUsage, false);
				Type propertyType = XamlTypeMapper.GetPropertyType(obj);
				if (propertyType == typeof(DependencyProperty))
				{
					Type declaringType2 = null;
					this._dpProperty = XamlTypeMapper.ParsePropertyName(this._parserContext, value, ref declaringType2);
					if (this._bamlRecordWriter != null && this._dpProperty != null)
					{
						short valueId;
						short attributeOrTypeId = this._parserContext.MapTable.GetAttributeOrTypeId(this._bamlRecordWriter.BinaryWriter, declaringType2, this._dpProperty.Name, out valueId);
						if (attributeOrTypeId < 0)
						{
							xamlPropertyNode.ValueId = attributeOrTypeId;
							xamlPropertyNode.MemberName = null;
						}
						else
						{
							xamlPropertyNode.ValueId = valueId;
							xamlPropertyNode.MemberName = this._dpProperty.Name;
						}
					}
				}
				else if (this._dpProperty != null)
				{
					xamlPropertyNode.ValuePropertyType = this._dpProperty.PropertyType;
					xamlPropertyNode.ValuePropertyMember = this._dpProperty;
					xamlPropertyNode.ValuePropertyName = this._dpProperty.Name;
					xamlPropertyNode.ValueDeclaringType = this._dpProperty.OwnerType;
					string fullName = this._dpProperty.OwnerType.Assembly.FullName;
					this._dpProperty = null;
				}
				this._bamlRecordWriter.WriteProperty(xamlPropertyNode);
				return;
			}
			if (!attributeData.IsSimple)
			{
				this._extensionParser.CompileAttribute(this._markupExtensionNodes, attributeData);
				return;
			}
			if (attributeData.IsTypeExtension)
			{
				Type typeFromBaseString = this._xamlTypeMapper.GetTypeFromBaseString(attributeData.Args, this._parserContext, true);
				XamlPropertyWithTypeNode xamlPropertyWithType = new XamlPropertyWithTypeNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, typeFromBaseString.FullName, typeFromBaseString.Assembly.FullName, typeFromBaseString, string.Empty, string.Empty);
				this._bamlRecordWriter.WritePropertyWithType(xamlPropertyWithType);
				return;
			}
			XamlPropertyWithExtensionNode xamlPropertyNode2 = new XamlPropertyWithExtensionNode(0, 0, this._depth, obj, assemblyName, ownerTypeFullName, propName, attributeData.Args, attributeData.ExtensionTypeId, attributeData.IsValueNestedExtension, attributeData.IsValueTypeExtension);
			this._bamlRecordWriter.WritePropertyWithExtension(xamlPropertyNode2);
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x0009744C File Offset: 0x0009564C
		public void WriteContentProperty(string assemblyName, string ownerTypeFullName, string propName)
		{
			object propertyMember;
			Type type;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out propertyMember, out type);
			XamlContentPropertyNode xamlContentPropertyNode = new XamlContentPropertyNode(0, 0, this._depth, propertyMember, assemblyName, ownerTypeFullName, propName);
			this._bamlRecordWriter.WriteContentProperty(xamlContentPropertyNode);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00097484 File Offset: 0x00095684
		public void WriteXmlnsProperty(string localName, string xmlNamespace)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart && bamlRecordType != BamlRecordType.PropertyComplexStart && bamlRecordType != BamlRecordType.PropertyArrayStart && bamlRecordType != BamlRecordType.PropertyIListStart && bamlRecordType != BamlRecordType.PropertyIDictionaryStart)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterBadXmlns", new object[]
				{
					"WriteXmlnsProperty",
					bamlRecordType.ToString()
				}));
			}
			XamlXmlnsPropertyNode xamlXmlnsPropertyNode = new XamlXmlnsPropertyNode(0, 0, this._depth, localName, xmlNamespace);
			this._bamlRecordWriter.WriteNamespacePrefix(xamlXmlnsPropertyNode);
			this._parserContext.XmlnsDictionary[localName] = xmlNamespace;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00097514 File Offset: 0x00095714
		public void WriteDefAttribute(string name, string value)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.PeekRecordType();
			if (bamlRecordType != BamlRecordType.ElementStart && name != "Uid")
			{
				throw new InvalidOperationException(SR.Get("BamlWriterNoInElement", new object[]
				{
					"WriteDefAttribute",
					bamlRecordType.ToString()
				}));
			}
			if (name == "Key")
			{
				DefAttributeData defAttributeData = this._extensionParser.IsMarkupExtensionDefAttribute(this.PeekElementType(), ref value, 0, 0, 0);
				if (defAttributeData != null)
				{
					if (name != "Key")
					{
						defAttributeData.IsSimple = false;
					}
					if (defAttributeData.IsSimple)
					{
						int num = defAttributeData.Args.IndexOf(':');
						string prefix = string.Empty;
						string localName = defAttributeData.Args;
						if (num > 0)
						{
							prefix = defAttributeData.Args.Substring(0, num);
							localName = defAttributeData.Args.Substring(num + 1);
						}
						string namespaceURI = this._parserContext.XmlnsDictionary[prefix];
						string empty = string.Empty;
						string empty2 = string.Empty;
						Type type = null;
						Type type2 = null;
						bool elementType = ((IParserHelper)this).GetElementType(false, localName, namespaceURI, ref empty, ref empty2, ref type, ref type2);
						if (elementType)
						{
							XamlDefAttributeKeyTypeNode xamlDefNode = new XamlDefAttributeKeyTypeNode(0, 0, this._depth, empty2, type.Assembly.FullName, type);
							this._bamlRecordWriter.WriteDefAttributeKeyType(xamlDefNode);
						}
						else
						{
							defAttributeData.IsSimple = false;
							DefAttributeData defAttributeData2 = defAttributeData;
							defAttributeData2.Args += "}";
						}
					}
					if (!defAttributeData.IsSimple)
					{
						this._extensionParser.CompileDictionaryKey(this._markupExtensionNodes, defAttributeData);
					}
					return;
				}
			}
			XamlDefAttributeNode xamlDefNode2 = new XamlDefAttributeNode(0, 0, this._depth, name, value);
			this._bamlRecordWriter.WriteDefAttribute(xamlDefNode2);
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000976BC File Offset: 0x000958BC
		public void WritePresentationOptionsAttribute(string name, string value)
		{
			this.VerifyWriteState();
			XamlPresentationOptionsAttributeNode xamlPresentationOptionsNode = new XamlPresentationOptionsAttributeNode(0, 0, this._depth, name, value);
			this._bamlRecordWriter.WritePresentationOptionsAttribute(xamlPresentationOptionsNode);
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000976EC File Offset: 0x000958EC
		public void WriteStartComplexProperty(string assemblyName, string ownerTypeFullName, string propName)
		{
			this.VerifyWriteState();
			this._parserContext.PushScope();
			this.ProcessMarkupExtensionNodes();
			Type type = null;
			bool propertyCanWrite = true;
			object obj;
			Type type2;
			this.GetDpOrPi(assemblyName, ownerTypeFullName, propName, out obj, out type2);
			if (obj == null)
			{
				MethodInfo mi = this.GetMi(assemblyName, ownerTypeFullName, propName, out type2);
				if (mi != null)
				{
					XamlTypeMapper.GetPropertyType(mi, out type, out propertyCanWrite);
				}
			}
			else
			{
				type = XamlTypeMapper.GetPropertyType(obj);
				PropertyInfo propertyInfo = obj as PropertyInfo;
				if (propertyInfo != null)
				{
					propertyCanWrite = propertyInfo.CanWrite;
				}
				else
				{
					DependencyProperty dependencyProperty = obj as DependencyProperty;
					if (dependencyProperty != null)
					{
						propertyCanWrite = !dependencyProperty.ReadOnly;
					}
				}
			}
			int depth;
			if (type == null)
			{
				this.Push(BamlRecordType.PropertyComplexStart);
				int lineNumber = 0;
				int linePosition = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyComplexStartNode xamlComplexPropertyNode = new XamlPropertyComplexStartNode(lineNumber, linePosition, depth, null, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyComplexStart(xamlComplexPropertyNode);
				return;
			}
			BamlRecordType propertyStartRecordType = BamlRecordManager.GetPropertyStartRecordType(type, propertyCanWrite);
			this.Push(propertyStartRecordType);
			switch (propertyStartRecordType)
			{
			case BamlRecordType.PropertyArrayStart:
			{
				int lineNumber2 = 0;
				int linePosition2 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyArrayStartNode xamlPropertyArrayStartNode = new XamlPropertyArrayStartNode(lineNumber2, linePosition2, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyArrayStart(xamlPropertyArrayStartNode);
				return;
			}
			case BamlRecordType.PropertyIListStart:
			{
				int lineNumber3 = 0;
				int linePosition3 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyIListStartNode xamlPropertyIListStart = new XamlPropertyIListStartNode(lineNumber3, linePosition3, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyIListStart(xamlPropertyIListStart);
				return;
			}
			case BamlRecordType.PropertyIDictionaryStart:
			{
				int lineNumber4 = 0;
				int linePosition4 = 0;
				depth = this._depth;
				this._depth = depth + 1;
				XamlPropertyIDictionaryStartNode xamlPropertyIDictionaryStartNode = new XamlPropertyIDictionaryStartNode(lineNumber4, linePosition4, depth, obj, assemblyName, ownerTypeFullName, propName);
				this._bamlRecordWriter.WritePropertyIDictionaryStart(xamlPropertyIDictionaryStartNode);
				return;
			}
			}
			int lineNumber5 = 0;
			int linePosition5 = 0;
			depth = this._depth;
			this._depth = depth + 1;
			XamlPropertyComplexStartNode xamlComplexPropertyNode2 = new XamlPropertyComplexStartNode(lineNumber5, linePosition5, depth, obj, assemblyName, ownerTypeFullName, propName);
			this._bamlRecordWriter.WritePropertyComplexStart(xamlComplexPropertyNode2);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000978B0 File Offset: 0x00095AB0
		public void WriteEndComplexProperty()
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.Pop();
			switch (bamlRecordType)
			{
			case BamlRecordType.PropertyComplexStart:
			{
				int lineNumber = 0;
				int linePosition = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyComplexEndNode xamlPropertyComplexEnd = new XamlPropertyComplexEndNode(lineNumber, linePosition, depth);
				this._bamlRecordWriter.WritePropertyComplexEnd(xamlPropertyComplexEnd);
				goto IL_11F;
			}
			case BamlRecordType.PropertyArrayStart:
			{
				int lineNumber2 = 0;
				int linePosition2 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyArrayEndNode xamlPropertyArrayEndNode = new XamlPropertyArrayEndNode(lineNumber2, linePosition2, depth);
				this._bamlRecordWriter.WritePropertyArrayEnd(xamlPropertyArrayEndNode);
				goto IL_11F;
			}
			case BamlRecordType.PropertyIListStart:
			{
				int lineNumber3 = 0;
				int linePosition3 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyIListEndNode xamlPropertyIListEndNode = new XamlPropertyIListEndNode(lineNumber3, linePosition3, depth);
				this._bamlRecordWriter.WritePropertyIListEnd(xamlPropertyIListEndNode);
				goto IL_11F;
			}
			case BamlRecordType.PropertyIDictionaryStart:
			{
				int lineNumber4 = 0;
				int linePosition4 = 0;
				int depth = this._depth - 1;
				this._depth = depth;
				XamlPropertyIDictionaryEndNode xamlPropertyIDictionaryEndNode = new XamlPropertyIDictionaryEndNode(lineNumber4, linePosition4, depth);
				this._bamlRecordWriter.WritePropertyIDictionaryEnd(xamlPropertyIDictionaryEndNode);
				goto IL_11F;
			}
			}
			throw new InvalidOperationException(SR.Get("BamlWriterBadScope", new object[]
			{
				bamlRecordType.ToString(),
				BamlRecordType.PropertyComplexEnd.ToString()
			}));
			IL_11F:
			this._parserContext.PopScope();
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000979E8 File Offset: 0x00095BE8
		public void WriteLiteralContent(string contents)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			XamlLiteralContentNode xamlLiteralContentNode = new XamlLiteralContentNode(0, 0, this._depth, contents);
			this._bamlRecordWriter.WriteLiteralContent(xamlLiteralContentNode);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00097A1C File Offset: 0x00095C1C
		public void WritePIMapping(string xmlNamespace, string clrNamespace, string assemblyName)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			XamlPIMappingNode xamlPIMappingNode = new XamlPIMappingNode(0, 0, this._depth, xmlNamespace, clrNamespace, assemblyName);
			if (!this._xamlTypeMapper.PITable.Contains(xmlNamespace))
			{
				ClrNamespaceAssemblyPair clrNamespaceAssemblyPair = new ClrNamespaceAssemblyPair(clrNamespace, assemblyName);
				this._xamlTypeMapper.PITable.Add(xmlNamespace, clrNamespaceAssemblyPair);
			}
			this._bamlRecordWriter.WritePIMapping(xamlPIMappingNode);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00097A88 File Offset: 0x00095C88
		public void WriteText(string textContent, string typeConverterAssemblyName, string typeConverterName)
		{
			this.VerifyWriteState();
			this.ProcessMarkupExtensionNodes();
			Type converterType = null;
			if (!string.IsNullOrEmpty(typeConverterName))
			{
				converterType = this.GetType(typeConverterAssemblyName, typeConverterName);
			}
			XamlTextNode xamlTextNode = new XamlTextNode(0, 0, this._depth, textContent, converterType);
			this._bamlRecordWriter.WriteText(xamlTextNode);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00097AD0 File Offset: 0x00095CD0
		public void WriteRoutedEvent(string assemblyName, string ownerTypeFullName, string eventIdName, string handlerName)
		{
			throw new NotSupportedException(SR.Get("ParserBamlEvent", new object[]
			{
				eventIdName
			}));
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00097AEB File Offset: 0x00095CEB
		public void WriteEvent(string eventName, string handlerName)
		{
			throw new NotSupportedException(SR.Get("ParserBamlEvent", new object[]
			{
				eventName
			}));
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00097B08 File Offset: 0x00095D08
		private void ProcessMarkupExtensionNodes()
		{
			int i = 0;
			while (i < this._markupExtensionNodes.Count)
			{
				XamlNode xamlNode = this._markupExtensionNodes[i] as XamlNode;
				XamlNodeType tokenType = xamlNode.TokenType;
				switch (tokenType)
				{
				case XamlNodeType.ElementStart:
					this._bamlRecordWriter.WriteElementStart((XamlElementStartNode)xamlNode);
					break;
				case XamlNodeType.ElementEnd:
					this._bamlRecordWriter.WriteElementEnd((XamlElementEndNode)xamlNode);
					break;
				case XamlNodeType.Property:
					this._bamlRecordWriter.WriteProperty((XamlPropertyNode)xamlNode);
					break;
				case XamlNodeType.PropertyComplexStart:
					this._bamlRecordWriter.WritePropertyComplexStart((XamlPropertyComplexStartNode)xamlNode);
					break;
				case XamlNodeType.PropertyComplexEnd:
					this._bamlRecordWriter.WritePropertyComplexEnd((XamlPropertyComplexEndNode)xamlNode);
					break;
				case XamlNodeType.PropertyArrayStart:
				case XamlNodeType.PropertyArrayEnd:
				case XamlNodeType.PropertyIListStart:
				case XamlNodeType.PropertyIListEnd:
				case XamlNodeType.PropertyIDictionaryStart:
				case XamlNodeType.PropertyIDictionaryEnd:
				case XamlNodeType.LiteralContent:
					goto IL_1A2;
				case XamlNodeType.PropertyWithExtension:
					this._bamlRecordWriter.WritePropertyWithExtension((XamlPropertyWithExtensionNode)xamlNode);
					break;
				case XamlNodeType.PropertyWithType:
					this._bamlRecordWriter.WritePropertyWithType((XamlPropertyWithTypeNode)xamlNode);
					break;
				case XamlNodeType.Text:
					this._bamlRecordWriter.WriteText((XamlTextNode)xamlNode);
					break;
				default:
					switch (tokenType)
					{
					case XamlNodeType.EndAttributes:
						this._bamlRecordWriter.WriteEndAttributes((XamlEndAttributesNode)xamlNode);
						break;
					case XamlNodeType.PIMapping:
					case XamlNodeType.UnknownTagStart:
					case XamlNodeType.UnknownTagEnd:
					case XamlNodeType.UnknownAttribute:
						goto IL_1A2;
					case XamlNodeType.KeyElementStart:
						this._bamlRecordWriter.WriteKeyElementStart((XamlKeyElementStartNode)xamlNode);
						break;
					case XamlNodeType.KeyElementEnd:
						this._bamlRecordWriter.WriteKeyElementEnd((XamlKeyElementEndNode)xamlNode);
						break;
					case XamlNodeType.ConstructorParametersStart:
						this._bamlRecordWriter.WriteConstructorParametersStart((XamlConstructorParametersStartNode)xamlNode);
						break;
					case XamlNodeType.ConstructorParametersEnd:
						this._bamlRecordWriter.WriteConstructorParametersEnd((XamlConstructorParametersEndNode)xamlNode);
						break;
					default:
						goto IL_1A2;
					}
					break;
				}
				i++;
				continue;
				IL_1A2:
				throw new InvalidOperationException(SR.Get("BamlWriterUnknownMarkupExtension"));
			}
			this._markupExtensionNodes.Clear();
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00097CE7 File Offset: 0x00095EE7
		private void VerifyWriteState()
		{
			if (this._closed)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterClosed"));
			}
			if (!this._startDocumentWritten)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterStartDoc"));
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00097D1C File Offset: 0x00095F1C
		private void VerifyEndTagState(BamlRecordType expectedStartTag, BamlRecordType endTagBeingWritten)
		{
			this.VerifyWriteState();
			BamlRecordType bamlRecordType = this.Pop();
			if (bamlRecordType != expectedStartTag)
			{
				throw new InvalidOperationException(SR.Get("BamlWriterBadScope", new object[]
				{
					bamlRecordType.ToString(),
					endTagBeingWritten.ToString()
				}));
			}
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00097D70 File Offset: 0x00095F70
		private Assembly GetAssembly(string assemblyName)
		{
			Assembly assembly = this._assemblies[assemblyName] as Assembly;
			if (assembly == null)
			{
				assembly = ReflectionHelper.LoadAssembly(assemblyName, null);
				if (assembly == null)
				{
					throw new ArgumentException(SR.Get("BamlWriterBadAssembly", new object[]
					{
						assemblyName
					}));
				}
				this._assemblies[assemblyName] = assembly;
			}
			return assembly;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x00097DD4 File Offset: 0x00095FD4
		private Type GetType(string assemblyName, string typeFullName)
		{
			Assembly assembly = this.GetAssembly(assemblyName);
			return assembly.GetType(typeFullName);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00097DF4 File Offset: 0x00095FF4
		private object GetDpOrPi(Type ownerType, string propName)
		{
			object obj = null;
			if (ownerType != null)
			{
				obj = DependencyProperty.FromName(propName, ownerType);
				if (obj == null)
				{
					PropertyInfo propertyInfo = null;
					MemberInfo[] member = ownerType.GetMember(propName, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
					foreach (PropertyInfo propertyInfo2 in member)
					{
						if (propertyInfo2.GetIndexParameters().Length == 0 && (propertyInfo == null || propertyInfo.DeclaringType.IsAssignableFrom(propertyInfo2.DeclaringType)))
						{
							propertyInfo = propertyInfo2;
						}
					}
					obj = propertyInfo;
				}
			}
			return obj;
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00097E71 File Offset: 0x00096071
		private void GetDpOrPi(string assemblyName, string ownerTypeFullName, string propName, out object dpOrPi, out Type ownerType)
		{
			if (assemblyName == string.Empty || ownerTypeFullName == string.Empty)
			{
				dpOrPi = null;
				ownerType = null;
				return;
			}
			ownerType = this.GetType(assemblyName, ownerTypeFullName);
			dpOrPi = this.GetDpOrPi(ownerType, propName);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00097EB0 File Offset: 0x000960B0
		private MethodInfo GetMi(Type ownerType, string propName)
		{
			MethodInfo methodInfo = ownerType.GetMethod("Set" + propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (methodInfo != null && methodInfo.GetParameters().Length != 2)
			{
				methodInfo = null;
			}
			if (methodInfo == null)
			{
				methodInfo = ownerType.GetMethod("Get" + propName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (methodInfo != null && methodInfo.GetParameters().Length != 1)
				{
					methodInfo = null;
				}
			}
			return methodInfo;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00097F20 File Offset: 0x00096120
		private MethodInfo GetMi(string assemblyName, string ownerTypeFullName, string propName, out Type ownerType)
		{
			MethodInfo result;
			if (assemblyName == string.Empty || ownerTypeFullName == string.Empty)
			{
				result = null;
				ownerType = null;
			}
			else
			{
				ownerType = this.GetType(assemblyName, ownerTypeFullName);
				result = this.GetMi(ownerType, propName);
			}
			return result;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00097F68 File Offset: 0x00096168
		private void Push(BamlRecordType recordType)
		{
			this.CheckEndAttributes();
			this._nodeTypeStack.Push(new BamlWriter.WriteStackNode(recordType));
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00097F81 File Offset: 0x00096181
		private void Push(BamlRecordType recordType, Type elementType)
		{
			this.CheckEndAttributes();
			this._nodeTypeStack.Push(new BamlWriter.WriteStackNode(recordType, elementType));
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00097F9C File Offset: 0x0009619C
		private BamlRecordType Pop()
		{
			BamlWriter.WriteStackNode writeStackNode = this._nodeTypeStack.Pop() as BamlWriter.WriteStackNode;
			return writeStackNode.RecordType;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00097FC0 File Offset: 0x000961C0
		private BamlRecordType PeekRecordType()
		{
			BamlWriter.WriteStackNode writeStackNode = this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode;
			return writeStackNode.RecordType;
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00097FE4 File Offset: 0x000961E4
		private Type PeekElementType()
		{
			BamlWriter.WriteStackNode writeStackNode = this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode;
			return writeStackNode.ElementType;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00098008 File Offset: 0x00096208
		private void CheckEndAttributes()
		{
			if (this._nodeTypeStack.Count > 0)
			{
				BamlWriter.WriteStackNode writeStackNode = this._nodeTypeStack.Peek() as BamlWriter.WriteStackNode;
				if (!writeStackNode.EndAttributesReached && writeStackNode.RecordType == BamlRecordType.ElementStart)
				{
					XamlEndAttributesNode xamlEndAttributesNode = new XamlEndAttributesNode(0, 0, this._depth, false);
					this._bamlRecordWriter.WriteEndAttributes(xamlEndAttributesNode);
				}
				writeStackNode.EndAttributesReached = true;
			}
		}

		// Token: 0x0400156F RID: 5487
		private BamlRecordWriter _bamlRecordWriter;

		// Token: 0x04001570 RID: 5488
		private bool _startDocumentWritten;

		// Token: 0x04001571 RID: 5489
		private int _depth;

		// Token: 0x04001572 RID: 5490
		private bool _closed;

		// Token: 0x04001573 RID: 5491
		private DependencyProperty _dpProperty;

		// Token: 0x04001574 RID: 5492
		private ParserStack _nodeTypeStack;

		// Token: 0x04001575 RID: 5493
		private Hashtable _assemblies;

		// Token: 0x04001576 RID: 5494
		private XamlTypeMapper _xamlTypeMapper;

		// Token: 0x04001577 RID: 5495
		private ParserContext _parserContext;

		// Token: 0x04001578 RID: 5496
		private MarkupExtensionParser _extensionParser;

		// Token: 0x04001579 RID: 5497
		private ArrayList _markupExtensionNodes;

		// Token: 0x0200088F RID: 2191
		private class WriteStackNode
		{
			// Token: 0x06008369 RID: 33641 RVA: 0x00245158 File Offset: 0x00243358
			public WriteStackNode(BamlRecordType recordType)
			{
				this._recordType = recordType;
				this._endAttributesReached = false;
			}

			// Token: 0x0600836A RID: 33642 RVA: 0x0024516E File Offset: 0x0024336E
			public WriteStackNode(BamlRecordType recordType, Type elementType) : this(recordType)
			{
				this._elementType = elementType;
			}

			// Token: 0x17001DCA RID: 7626
			// (get) Token: 0x0600836B RID: 33643 RVA: 0x0024517E File Offset: 0x0024337E
			public BamlRecordType RecordType
			{
				get
				{
					return this._recordType;
				}
			}

			// Token: 0x17001DCB RID: 7627
			// (get) Token: 0x0600836C RID: 33644 RVA: 0x00245186 File Offset: 0x00243386
			// (set) Token: 0x0600836D RID: 33645 RVA: 0x0024518E File Offset: 0x0024338E
			public bool EndAttributesReached
			{
				get
				{
					return this._endAttributesReached;
				}
				set
				{
					this._endAttributesReached = value;
				}
			}

			// Token: 0x17001DCC RID: 7628
			// (get) Token: 0x0600836E RID: 33646 RVA: 0x00245197 File Offset: 0x00243397
			public Type ElementType
			{
				get
				{
					return this._elementType;
				}
			}

			// Token: 0x04004178 RID: 16760
			private bool _endAttributesReached;

			// Token: 0x04004179 RID: 16761
			private BamlRecordType _recordType;

			// Token: 0x0400417A RID: 16762
			private Type _elementType;
		}
	}
}
