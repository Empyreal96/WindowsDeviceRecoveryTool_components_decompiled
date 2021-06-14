using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using MS.Internal.Xaml.Parser;

namespace System.Windows.Markup
{
	// Token: 0x02000222 RID: 546
	internal class MarkupExtensionParser
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x000A7D15 File Offset: 0x000A5F15
		internal MarkupExtensionParser(IParserHelper parserHelper, ParserContext parserContext)
		{
			this._parserHelper = parserHelper;
			this._parserContext = parserContext;
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000A7D2C File Offset: 0x000A5F2C
		internal AttributeData IsMarkupExtensionAttribute(Type declaringType, string propIdName, ref string attrValue, int lineNumber, int linePosition, int depth, object info)
		{
			string typename;
			string args;
			if (!MarkupExtensionParser.GetMarkupExtensionTypeAndArgs(ref attrValue, out typename, out args))
			{
				return null;
			}
			return this.FillAttributeData(declaringType, propIdName, typename, args, attrValue, lineNumber, linePosition, depth, info);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000A7D5C File Offset: 0x000A5F5C
		internal DefAttributeData IsMarkupExtensionDefAttribute(Type declaringType, ref string attrValue, int lineNumber, int linePosition, int depth)
		{
			string typename;
			string args;
			if (!MarkupExtensionParser.GetMarkupExtensionTypeAndArgs(ref attrValue, out typename, out args))
			{
				return null;
			}
			return this.FillDefAttributeData(declaringType, typename, args, attrValue, lineNumber, linePosition, depth);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000A7D87 File Offset: 0x000A5F87
		internal static bool LooksLikeAMarkupExtension(string attrValue)
		{
			return attrValue.Length >= 2 && attrValue[0] == '{' && attrValue[1] != '}';
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000A7DB0 File Offset: 0x000A5FB0
		internal static string AddEscapeToLiteralString(string literalString)
		{
			string text = literalString;
			if (!string.IsNullOrEmpty(text) && text[0] == '{')
			{
				text = "{}" + text;
			}
			return text;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000A7DE0 File Offset: 0x000A5FE0
		private KnownElements GetKnownExtensionFromType(Type extensionType, out string propName)
		{
			if (KnownTypes.Types[691] == extensionType)
			{
				propName = "TypeName";
				return KnownElements.TypeExtension;
			}
			if (KnownTypes.Types[602] == extensionType)
			{
				propName = "Member";
				return KnownElements.StaticExtension;
			}
			if (KnownTypes.Types[634] == extensionType)
			{
				propName = "Property";
				return KnownElements.TemplateBindingExtension;
			}
			if (KnownTypes.Types[189] == extensionType)
			{
				propName = "ResourceKey";
				return KnownElements.DynamicResourceExtension;
			}
			if (KnownTypes.Types[603] == extensionType)
			{
				propName = "ResourceKey";
				return KnownElements.StaticResourceExtension;
			}
			propName = string.Empty;
			return KnownElements.UnknownElement;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000A7EA9 File Offset: 0x000A60A9
		private bool IsSimpleTypeExtensionArgs(Type extensionType, int lineNumber, int linePosition, ref string args)
		{
			return KnownTypes.Types[691] == extensionType && this.IsSimpleExtensionArgs(lineNumber, linePosition, "TypeName", ref args, extensionType);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000A7ED4 File Offset: 0x000A60D4
		private bool IsSimpleExtension(Type extensionType, int lineNumber, int linePosition, int depth, out short extensionTypeId, out bool isValueNestedExtension, out bool isValueTypeExtension, ref string args)
		{
			bool flag = false;
			extensionTypeId = 0;
			isValueNestedExtension = false;
			isValueTypeExtension = false;
			string propName;
			KnownElements knownExtensionFromType = this.GetKnownExtensionFromType(extensionType, out propName);
			if (knownExtensionFromType != KnownElements.UnknownElement)
			{
				flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, propName, ref args, extensionType);
			}
			if (flag)
			{
				if ((knownExtensionFromType == KnownElements.DynamicResourceExtension || knownExtensionFromType == KnownElements.StaticResourceExtension) && MarkupExtensionParser.LooksLikeAMarkupExtension(args))
				{
					AttributeData attributeData = this.IsMarkupExtensionAttribute(extensionType, null, ref args, lineNumber, linePosition, depth, null);
					isValueTypeExtension = attributeData.IsTypeExtension;
					flag = (isValueTypeExtension || attributeData.IsStaticExtension);
					isValueNestedExtension = flag;
					if (flag)
					{
						args = attributeData.Args;
					}
					else
					{
						args += "}";
					}
				}
				if (flag)
				{
					extensionTypeId = (short)knownExtensionFromType;
				}
			}
			return flag;
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000A7F78 File Offset: 0x000A6178
		private bool IsSimpleExtensionArgs(int lineNumber, int linePosition, string propName, ref string args, Type targetType)
		{
			ArrayList arrayList = this.TokenizeAttributes(args, lineNumber, linePosition, targetType);
			if (arrayList == null)
			{
				return false;
			}
			if (arrayList.Count == 1)
			{
				args = (string)arrayList[0];
				return true;
			}
			if (arrayList.Count == 3 && (string)arrayList[0] == propName)
			{
				args = (string)arrayList[2];
				return true;
			}
			return false;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000A7FE4 File Offset: 0x000A61E4
		internal static bool GetMarkupExtensionTypeAndArgs(ref string attrValue, out string typeName, out string args)
		{
			int length = attrValue.Length;
			typeName = string.Empty;
			args = string.Empty;
			if (length < 1 || attrValue[0] != '{')
			{
				return false;
			}
			bool flag = false;
			StringBuilder stringBuilder = null;
			int i;
			for (i = 1; i < length; i++)
			{
				if (char.IsWhiteSpace(attrValue[i]))
				{
					if (stringBuilder != null)
					{
						break;
					}
				}
				else if (stringBuilder == null)
				{
					if (!flag && attrValue[i] == '\\')
					{
						flag = true;
					}
					else if (attrValue[i] == '}')
					{
						if (i == 1)
						{
							attrValue = attrValue.Substring(2);
							return false;
						}
					}
					else
					{
						stringBuilder = new StringBuilder(length - i);
						stringBuilder.Append(attrValue[i]);
						flag = false;
					}
				}
				else if (!flag && attrValue[i] == '\\')
				{
					flag = true;
				}
				else
				{
					if (attrValue[i] == '}')
					{
						break;
					}
					stringBuilder.Append(attrValue[i]);
					flag = false;
				}
			}
			if (stringBuilder != null)
			{
				typeName = stringBuilder.ToString();
			}
			if (i < length - 1)
			{
				args = attrValue.Substring(i, length - i);
			}
			else if (attrValue[length - 1] == '}')
			{
				args = "}";
			}
			return true;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000A8100 File Offset: 0x000A6300
		private DefAttributeData FillDefAttributeData(Type declaringType, string typename, string args, string attributeValue, int lineNumber, int linePosition, int depth)
		{
			bool isSimple = false;
			string targetNamespaceUri;
			string targetAssemblyName;
			string targetFullName;
			Type type;
			Type type2;
			bool extensionType = this.GetExtensionType(typename, attributeValue, lineNumber, linePosition, out targetNamespaceUri, out targetAssemblyName, out targetFullName, out type, out type2);
			if (extensionType)
			{
				isSimple = this.IsSimpleTypeExtensionArgs(type, lineNumber, linePosition, ref args);
			}
			return new DefAttributeData(targetAssemblyName, targetFullName, type, args, declaringType, targetNamespaceUri, lineNumber, linePosition, depth, isSimple);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000A8150 File Offset: 0x000A6350
		private AttributeData FillAttributeData(Type declaringType, string propIdName, string typename, string args, string attributeValue, int lineNumber, int linePosition, int depth, object info)
		{
			bool flag = false;
			short extensionTypeId = 0;
			bool isValueNestedExtension = false;
			bool isValueTypeExtension = false;
			string targetNamespaceUri;
			string targetAssemblyName;
			string targetFullName;
			Type type;
			Type serializerType;
			bool extensionType = this.GetExtensionType(typename, attributeValue, lineNumber, linePosition, out targetNamespaceUri, out targetAssemblyName, out targetFullName, out type, out serializerType);
			if (extensionType && propIdName != string.Empty)
			{
				if (propIdName == null)
				{
					if (KnownTypes.Types[691] == type)
					{
						flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, "TypeName", ref args, type);
						isValueNestedExtension = flag;
						isValueTypeExtension = flag;
						extensionTypeId = 691;
					}
					else if (KnownTypes.Types[602] == type)
					{
						flag = this.IsSimpleExtensionArgs(lineNumber, linePosition, "Member", ref args, type);
						isValueNestedExtension = flag;
						extensionTypeId = 602;
					}
				}
				else
				{
					propIdName = propIdName.Trim();
					flag = this.IsSimpleExtension(type, lineNumber, linePosition, depth, out extensionTypeId, out isValueNestedExtension, out isValueTypeExtension, ref args);
				}
			}
			return new AttributeData(targetAssemblyName, targetFullName, type, args, declaringType, propIdName, info, serializerType, lineNumber, linePosition, depth, targetNamespaceUri, extensionTypeId, isValueNestedExtension, isValueTypeExtension, flag);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000A8250 File Offset: 0x000A6450
		private bool GetExtensionType(string typename, string attributeValue, int lineNumber, int linePosition, out string namespaceURI, out string targetAssemblyName, out string targetFullName, out Type targetType, out Type serializerType)
		{
			targetAssemblyName = null;
			targetFullName = null;
			targetType = null;
			serializerType = null;
			string text = typename;
			string prefix = string.Empty;
			int num = typename.IndexOf(':');
			if (num >= 0)
			{
				prefix = typename.Substring(0, num);
				typename = typename.Substring(num + 1);
			}
			namespaceURI = this._parserHelper.LookupNamespace(prefix);
			bool elementType = this._parserHelper.GetElementType(true, typename, namespaceURI, ref targetAssemblyName, ref targetFullName, ref targetType, ref serializerType);
			if (!elementType)
			{
				if (this._parserHelper.CanResolveLocalAssemblies())
				{
					this.ThrowException("ParserNotMarkupExtension", attributeValue, typename, namespaceURI, lineNumber, linePosition);
				}
				else
				{
					targetFullName = text;
					targetType = typeof(MarkupExtensionParser.UnknownMarkupExtension);
				}
			}
			else if (!KnownTypes.Types[381].IsAssignableFrom(targetType))
			{
				this.ThrowException("ParserNotMarkupExtension", attributeValue, typename, namespaceURI, lineNumber, linePosition);
			}
			return elementType;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000A8324 File Offset: 0x000A6524
		internal ArrayList CompileAttributes(ArrayList markupExtensionList, int startingDepth)
		{
			ArrayList arrayList = new ArrayList(markupExtensionList.Count * 5);
			for (int i = 0; i < markupExtensionList.Count; i++)
			{
				AttributeData data = (AttributeData)markupExtensionList[i];
				this.CompileAttribute(arrayList, data);
			}
			return arrayList;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000A8368 File Offset: 0x000A6568
		internal void CompileAttribute(ArrayList xamlNodes, AttributeData data)
		{
			string fullName = data.DeclaringType.Assembly.FullName;
			string fullName2 = data.DeclaringType.FullName;
			Type propertyType;
			bool propertyCanWrite;
			XamlTypeMapper.GetPropertyType(data.Info, out propertyType, out propertyCanWrite);
			XamlNode value;
			XamlNode value2;
			switch (BamlRecordManager.GetPropertyStartRecordType(propertyType, propertyCanWrite))
			{
			case BamlRecordType.PropertyArrayStart:
				value = new XamlPropertyArrayStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyArrayEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			case BamlRecordType.PropertyIListStart:
				value = new XamlPropertyIListStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyIListEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			case BamlRecordType.PropertyIDictionaryStart:
				value = new XamlPropertyIDictionaryStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
				value2 = new XamlPropertyIDictionaryEndNode(data.LineNumber, data.LinePosition, data.Depth);
				goto IL_164;
			}
			value = new XamlPropertyComplexStartNode(data.LineNumber, data.LinePosition, data.Depth, data.Info, fullName, fullName2, data.PropertyName);
			value2 = new XamlPropertyComplexEndNode(data.LineNumber, data.LinePosition, data.Depth);
			IL_164:
			xamlNodes.Add(value);
			this.CompileAttributeCore(xamlNodes, data);
			xamlNodes.Add(value2);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000A84F4 File Offset: 0x000A66F4
		internal void CompileAttributeCore(ArrayList xamlNodes, AttributeData data)
		{
			string text = null;
			string xmlNamespace = null;
			ArrayList arrayList = this.TokenizeAttributes(data.Args, data.LineNumber, data.LinePosition, data.TargetType);
			int num2;
			if (data.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				text = data.TargetFullName;
				string prefix = string.Empty;
				int num = text.IndexOf(':');
				if (num >= 0)
				{
					prefix = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				xmlNamespace = this._parserHelper.LookupNamespace(prefix);
				int lineNumber = data.LineNumber;
				int linePosition = data.LinePosition;
				num2 = data.Depth + 1;
				data.Depth = num2;
				xamlNodes.Add(new XamlUnknownTagStartNode(lineNumber, linePosition, num2, xmlNamespace, text));
			}
			else
			{
				int lineNumber2 = data.LineNumber;
				int linePosition2 = data.LinePosition;
				num2 = data.Depth + 1;
				data.Depth = num2;
				xamlNodes.Add(new XamlElementStartNode(lineNumber2, linePosition2, num2, data.TargetAssemblyName, data.TargetFullName, data.TargetType, data.SerializerType));
			}
			xamlNodes.Add(new XamlEndAttributesNode(data.LineNumber, data.LinePosition, data.Depth, true));
			int listIndex = 0;
			if (arrayList != null && (arrayList.Count == 1 || (arrayList.Count > 1 && !(arrayList[1] is string) && (char)arrayList[1] == ',')))
			{
				this.WriteConstructorParams(xamlNodes, arrayList, data, ref listIndex);
			}
			this.WriteProperties(xamlNodes, arrayList, listIndex, data);
			if (data.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				int lineNumber3 = data.LineNumber;
				int linePosition3 = data.LinePosition;
				num2 = data.Depth;
				data.Depth = num2 - 1;
				xamlNodes.Add(new XamlUnknownTagEndNode(lineNumber3, linePosition3, num2, text, xmlNamespace));
				return;
			}
			int lineNumber4 = data.LineNumber;
			int linePosition4 = data.LinePosition;
			num2 = data.Depth;
			data.Depth = num2 - 1;
			xamlNodes.Add(new XamlElementEndNode(lineNumber4, linePosition4, num2));
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000A86CC File Offset: 0x000A68CC
		internal ArrayList CompileDictionaryKeys(ArrayList complexDefAttributesList, int startingDepth)
		{
			ArrayList arrayList = new ArrayList(complexDefAttributesList.Count * 5);
			for (int i = 0; i < complexDefAttributesList.Count; i++)
			{
				DefAttributeData data = (DefAttributeData)complexDefAttributesList[i];
				this.CompileDictionaryKey(arrayList, data);
			}
			return arrayList;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000A8710 File Offset: 0x000A6910
		internal void CompileDictionaryKey(ArrayList xamlNodes, DefAttributeData data)
		{
			ArrayList arrayList = this.TokenizeAttributes(data.Args, data.LineNumber, data.LinePosition, data.TargetType);
			int lineNumber = data.LineNumber;
			int linePosition = data.LinePosition;
			int num = data.Depth + 1;
			data.Depth = num;
			xamlNodes.Add(new XamlKeyElementStartNode(lineNumber, linePosition, num, data.TargetAssemblyName, data.TargetFullName, data.TargetType, null));
			xamlNodes.Add(new XamlEndAttributesNode(data.LineNumber, data.LinePosition, data.Depth, true));
			int listIndex = 0;
			if (arrayList != null && (arrayList.Count == 1 || (arrayList.Count > 1 && !(arrayList[1] is string) && (char)arrayList[1] == ',')))
			{
				this.WriteConstructorParams(xamlNodes, arrayList, data, ref listIndex);
			}
			this.WriteProperties(xamlNodes, arrayList, listIndex, data);
			int lineNumber2 = data.LineNumber;
			int linePosition2 = data.LinePosition;
			num = data.Depth;
			data.Depth = num - 1;
			xamlNodes.Add(new XamlKeyElementEndNode(lineNumber2, linePosition2, num));
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000A880C File Offset: 0x000A6A0C
		private ArrayList TokenizeAttributes(string args, int lineNumber, int linePosition, Type extensionType)
		{
			if (extensionType == typeof(MarkupExtensionParser.UnknownMarkupExtension))
			{
				return null;
			}
			int num = 0;
			ParameterInfo[] array = this.FindLongestConstructor(extensionType, out num);
			Dictionary<string, SpecialBracketCharacters> dictionary = this._parserContext.InitBracketCharacterCacheForType(extensionType);
			Stack<char> stack = new Stack<char>();
			int num2 = 0;
			bool flag = array != null && num > 0;
			bool flag2 = false;
			ArrayList arrayList = null;
			int length = args.Length;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			char c = '\'';
			int num3 = 0;
			StringBuilder stringBuilder = null;
			SpecialBracketCharacters specialBracketCharacters = null;
			if (flag && dictionary != null)
			{
				string text = (num > 0) ? array[num2].Name : null;
				if (!string.IsNullOrEmpty(text))
				{
					specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
				}
			}
			int num4 = 0;
			while (num4 < length && !flag6)
			{
				if (!flag4 && args[num4] == '\\')
				{
					flag4 = true;
				}
				else
				{
					if (!flag5 && !char.IsWhiteSpace(args[num4]))
					{
						flag5 = true;
					}
					if (flag3 || num3 > 0 || flag5)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(length);
							arrayList = new ArrayList(1);
						}
						if (flag4)
						{
							stringBuilder.Append('\\');
							stringBuilder.Append(args[num4]);
							flag4 = false;
						}
						else if (flag3 || num3 > 0)
						{
							if (flag3 && args[num4] == c)
							{
								flag3 = false;
								flag5 = false;
								MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, false);
							}
							else
							{
								if (num3 > 0 && args[num4] == '}')
								{
									num3--;
								}
								else if (args[num4] == '{')
								{
									num3++;
								}
								stringBuilder.Append(args[num4]);
							}
						}
						else if (flag2)
						{
							stringBuilder.Append(args[num4]);
							if (specialBracketCharacters.StartsEscapeSequence(args[num4]))
							{
								stack.Push(args[num4]);
							}
							else if (specialBracketCharacters.EndsEscapeSequence(args[num4]))
							{
								if (specialBracketCharacters.Match(stack.Peek(), args[num4]))
								{
									stack.Pop();
								}
								else
								{
									this.ThrowException("ParserMarkupExtensionInvalidClosingBracketCharacers", args[num4].ToString(), lineNumber, linePosition);
								}
							}
							if (stack.Count == 0)
							{
								flag2 = false;
							}
						}
						else
						{
							char c2 = args[num4];
							if (c2 <= ',')
							{
								if (c2 == '"' || c2 == '\'')
								{
									if (stringBuilder.Length != 0)
									{
										this.ThrowException("ParserMarkupExtensionNoQuotesInName", args, lineNumber, linePosition);
									}
									flag3 = true;
									c = args[num4];
									goto IL_441;
								}
								if (c2 != ',')
								{
									goto IL_405;
								}
							}
							else if (c2 != '=')
							{
								if (c2 == '{')
								{
									num3++;
									stringBuilder.Append(args[num4]);
									goto IL_441;
								}
								if (c2 != '}')
								{
									goto IL_405;
								}
								flag6 = true;
								if (stringBuilder == null)
								{
									goto IL_441;
								}
								if (stringBuilder.Length > 0)
								{
									MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, true);
									goto IL_441;
								}
								if (arrayList.Count > 0 && arrayList[arrayList.Count - 1] is char)
								{
									this.ThrowException("ParserMarkupExtensionBadDelimiter", args, lineNumber, linePosition);
									goto IL_441;
								}
								goto IL_441;
							}
							if (flag && args[num4] == ',')
							{
								flag = (++num2 < num);
								if (flag)
								{
									string text = array[num2].Name;
									specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
								}
							}
							if (stringBuilder != null && stringBuilder.Length > 0)
							{
								MarkupExtensionParser.AddToTokenList(arrayList, stringBuilder, true);
								if (stack.Count != 0)
								{
									this.ThrowException("ParserMarkupExtensionMalformedBracketCharacers", stack.Peek().ToString(), lineNumber, linePosition);
								}
							}
							else if (arrayList.Count == 0)
							{
								this.ThrowException("ParserMarkupExtensionDelimiterBeforeFirstAttribute", args, lineNumber, linePosition);
							}
							else if (arrayList[arrayList.Count - 1] is char)
							{
								this.ThrowException("ParserMarkupExtensionBadDelimiter", args, lineNumber, linePosition);
							}
							if (args[num4] == '=')
							{
								flag = false;
								string text = (string)arrayList[arrayList.Count - 1];
								specialBracketCharacters = this.GetBracketCharacterForProperty(text, dictionary);
							}
							arrayList.Add(args[num4]);
							flag5 = false;
							goto IL_441;
							IL_405:
							if (specialBracketCharacters != null && specialBracketCharacters.StartsEscapeSequence(args[num4]))
							{
								stack.Clear();
								stack.Push(args[num4]);
								flag2 = true;
							}
							stringBuilder.Append(args[num4]);
						}
					}
				}
				IL_441:
				num4++;
			}
			if (!flag6)
			{
				this.ThrowException("ParserMarkupExtensionNoEndCurlie", "}", lineNumber, linePosition);
			}
			else if (num4 < length)
			{
				this.ThrowException("ParserMarkupExtensionTrailingGarbage", "}", args.Substring(num4, length - num4), lineNumber, linePosition);
			}
			return arrayList;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000A8CAC File Offset: 0x000A6EAC
		private static void AddToTokenList(ArrayList list, StringBuilder sb, bool trim)
		{
			if (trim)
			{
				int num = sb.Length - 1;
				while (char.IsWhiteSpace(sb[num]))
				{
					num--;
				}
				sb.Length = num + 1;
			}
			list.Add(sb.ToString());
			sb.Length = 0;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000A8CF8 File Offset: 0x000A6EF8
		private ParameterInfo[] FindLongestConstructor(Type extensionType, out int maxConstructorArguments)
		{
			ParameterInfo[] result = null;
			ConstructorInfo[] constructors = extensionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			maxConstructorArguments = 0;
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (parameters.Length >= maxConstructorArguments)
				{
					maxConstructorArguments = parameters.Length;
					result = parameters;
				}
			}
			return result;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000A8D44 File Offset: 0x000A6F44
		private void WriteConstructorParams(ArrayList xamlNodes, ArrayList list, DefAttributeData data, ref int listIndex)
		{
			if (list != null && listIndex < list.Count)
			{
				int lineNumber = data.LineNumber;
				int linePosition = data.LinePosition;
				int num = data.Depth + 1;
				data.Depth = num;
				xamlNodes.Add(new XamlConstructorParametersStartNode(lineNumber, linePosition, num));
				while (listIndex < list.Count)
				{
					if (!(list[listIndex] is string))
					{
						this.ThrowException("ParserMarkupExtensionBadConstructorParam", data.Args, data.LineNumber, data.LinePosition);
					}
					if (list.Count > listIndex + 1 && list[listIndex + 1] is char && (char)list[listIndex + 1] == '=')
					{
						break;
					}
					string textContent = (string)list[listIndex];
					AttributeData attributeData = this.IsMarkupExtensionAttribute(data.DeclaringType, string.Empty, ref textContent, data.LineNumber, data.LinePosition, data.Depth, null);
					if (attributeData == null)
					{
						MarkupExtensionParser.RemoveEscapes(ref textContent);
						xamlNodes.Add(new XamlTextNode(data.LineNumber, data.LinePosition, data.Depth, textContent, null));
					}
					else
					{
						this.CompileAttributeCore(xamlNodes, attributeData);
					}
					listIndex += 2;
				}
				int lineNumber2 = data.LineNumber;
				int linePosition2 = data.LinePosition;
				num = data.Depth;
				data.Depth = num - 1;
				xamlNodes.Add(new XamlConstructorParametersEndNode(lineNumber2, linePosition2, num));
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000A8EA0 File Offset: 0x000A70A0
		private void WriteProperties(ArrayList xamlNodes, ArrayList list, int listIndex, DefAttributeData data)
		{
			if (list != null && listIndex < list.Count)
			{
				ArrayList arrayList = new ArrayList(list.Count / 4);
				for (int i = listIndex; i < list.Count; i += 4)
				{
					if (i > list.Count - 3 || list[i + 1] is string || (char)list[i + 1] != '=')
					{
						this.ThrowException("ParserMarkupExtensionNoNameValue", data.Args, data.LineNumber, data.LinePosition);
					}
					string text = list[i] as string;
					text = text.Trim();
					if (arrayList.Contains(text))
					{
						this.ThrowException("ParserDuplicateMarkupExtensionProperty", text, data.LineNumber, data.LinePosition);
					}
					arrayList.Add(text);
					int num = text.IndexOf(':');
					string text2 = (num < 0) ? text : text.Substring(num + 1);
					string prefix = (num < 0) ? string.Empty : text.Substring(0, num);
					string attributeNamespaceUri = this.ResolveAttributeNamespaceURI(prefix, text2, data.TargetNamespaceUri);
					object info;
					string text3;
					string text4;
					Type type;
					string text5;
					AttributeContext attributeContext = this.GetAttributeContext(data.TargetType, data.TargetNamespaceUri, attributeNamespaceUri, text2, out info, out text3, out text4, out type, out text5);
					string value = list[i + 2] as string;
					AttributeData attributeData = this.IsMarkupExtensionAttribute(data.TargetType, text, ref value, data.LineNumber, data.LinePosition, data.Depth, info);
					list[i + 2] = value;
					if (data.IsUnknownExtension)
					{
						return;
					}
					if (attributeData != null)
					{
						if (attributeData.IsSimple)
						{
							this.CompileProperty(xamlNodes, text, attributeData.Args, data.TargetType, data.TargetNamespaceUri, attributeData, attributeData.LineNumber, attributeData.LinePosition, attributeData.Depth);
						}
						else
						{
							this.CompileAttribute(xamlNodes, attributeData);
						}
					}
					else
					{
						this.CompileProperty(xamlNodes, text, (string)list[i + 2], data.TargetType, data.TargetNamespaceUri, null, data.LineNumber, data.LinePosition, data.Depth);
					}
				}
			}
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000A90B0 File Offset: 0x000A72B0
		private string ResolveAttributeNamespaceURI(string prefix, string name, string parentURI)
		{
			string result;
			if (!string.IsNullOrEmpty(prefix))
			{
				result = this._parserHelper.LookupNamespace(prefix);
			}
			else
			{
				int num = name.IndexOf('.');
				if (-1 == num)
				{
					result = parentURI;
				}
				else
				{
					result = this._parserHelper.LookupNamespace("");
				}
			}
			return result;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000A90F8 File Offset: 0x000A72F8
		private SpecialBracketCharacters GetBracketCharacterForProperty(string propertyName, Dictionary<string, SpecialBracketCharacters> bracketCharacterCache)
		{
			SpecialBracketCharacters result = null;
			if (bracketCharacterCache != null && bracketCharacterCache.ContainsKey(propertyName))
			{
				result = bracketCharacterCache[propertyName];
			}
			return result;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000A911C File Offset: 0x000A731C
		private void CompileProperty(ArrayList xamlNodes, string name, string value, Type parentType, string parentTypeNamespaceUri, AttributeData data, int lineNumber, int linePosition, int depth)
		{
			MarkupExtensionParser.RemoveEscapes(ref name);
			MarkupExtensionParser.RemoveEscapes(ref value);
			int num = name.IndexOf(':');
			string text = (num < 0) ? name : name.Substring(num + 1);
			string text2 = (num < 0) ? string.Empty : name.Substring(0, num);
			string text3 = this.ResolveAttributeNamespaceURI(text2, text, parentTypeNamespaceUri);
			if (string.IsNullOrEmpty(text3))
			{
				this.ThrowException("ParserPrefixNSProperty", text2, name, lineNumber, linePosition);
			}
			object obj;
			string assemblyName;
			string typeFullName;
			Type type;
			string propertyName;
			AttributeContext attributeContext = this.GetAttributeContext(parentType, parentTypeNamespaceUri, text3, text, out obj, out assemblyName, out typeFullName, out type, out propertyName);
			if (attributeContext != AttributeContext.Property)
			{
				this.ThrowException("ParserMarkupExtensionUnknownAttr", text, parentType.FullName, lineNumber, linePosition);
			}
			MemberInfo memberInfo = obj as MemberInfo;
			if (data == null || !data.IsSimple)
			{
				XamlPropertyNode value2 = new XamlPropertyNode(lineNumber, linePosition, depth, obj, assemblyName, typeFullName, propertyName, value, BamlAttributeUsage.Default, true);
				xamlNodes.Add(value2);
				return;
			}
			if (data.IsTypeExtension)
			{
				string valueTypeFullName = value;
				string valueAssemblyName = null;
				Type typeFromBaseString = this._parserContext.XamlTypeMapper.GetTypeFromBaseString(value, this._parserContext, true);
				if (typeFromBaseString != null)
				{
					valueTypeFullName = typeFromBaseString.FullName;
					valueAssemblyName = typeFromBaseString.Assembly.FullName;
				}
				XamlPropertyWithTypeNode value3 = new XamlPropertyWithTypeNode(data.LineNumber, data.LinePosition, data.Depth, obj, assemblyName, typeFullName, text, valueTypeFullName, valueAssemblyName, typeFromBaseString, string.Empty, string.Empty);
				xamlNodes.Add(value3);
				return;
			}
			XamlPropertyWithExtensionNode value4 = new XamlPropertyWithExtensionNode(data.LineNumber, data.LinePosition, data.Depth, obj, assemblyName, typeFullName, text, value, data.ExtensionTypeId, data.IsValueNestedExtension, data.IsValueTypeExtension);
			xamlNodes.Add(value4);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000A92C4 File Offset: 0x000A74C4
		internal static void RemoveEscapes(ref string value)
		{
			StringBuilder stringBuilder = null;
			bool flag = true;
			for (int i = 0; i < value.Length; i++)
			{
				if (flag && value[i] == '\\')
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(value.Length);
						stringBuilder.Append(value.Substring(0, i));
					}
					flag = false;
				}
				else if (stringBuilder != null)
				{
					stringBuilder.Append(value[i]);
					flag = true;
				}
			}
			if (stringBuilder != null)
			{
				value = stringBuilder.ToString();
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000A933C File Offset: 0x000A753C
		private AttributeContext GetAttributeContext(Type elementBaseType, string elementBaseTypeNamespaceUri, string attributeNamespaceUri, string attributeLocalName, out object dynamicObject, out string assemblyName, out string typeFullName, out Type declaringType, out string dynamicObjectName)
		{
			AttributeContext result = AttributeContext.Unknown;
			dynamicObject = null;
			assemblyName = null;
			typeFullName = null;
			declaringType = null;
			dynamicObjectName = null;
			MemberInfo clrInfo = this._parserContext.XamlTypeMapper.GetClrInfo(false, elementBaseType, attributeNamespaceUri, attributeLocalName, ref dynamicObjectName);
			if (null != clrInfo)
			{
				result = AttributeContext.Property;
				dynamicObject = clrInfo;
				declaringType = clrInfo.DeclaringType;
				typeFullName = declaringType.FullName;
				assemblyName = declaringType.Assembly.FullName;
			}
			return result;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000A93AC File Offset: 0x000A75AC
		private void ThrowException(string id, string parameter1, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000A93D4 File Offset: 0x000A75D4
		private void ThrowException(string id, string parameter1, string parameter2, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1,
				parameter2
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000A9404 File Offset: 0x000A7604
		private void ThrowException(string id, string parameter1, string parameter2, string parameter3, int lineNumber, int linePosition)
		{
			string message = SR.Get(id, new object[]
			{
				parameter1,
				parameter2,
				parameter3
			});
			this.ThrowExceptionWithLine(message, lineNumber, linePosition);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000A9438 File Offset: 0x000A7638
		private void ThrowExceptionWithLine(string message, int lineNumber, int linePosition)
		{
			message += " ";
			message += SR.Get("ParserLineAndOffset", new object[]
			{
				lineNumber.ToString(CultureInfo.CurrentCulture),
				linePosition.ToString(CultureInfo.CurrentCulture)
			});
			XamlParseException ex = new XamlParseException(message, lineNumber, linePosition);
			throw ex;
		}

		// Token: 0x040019AF RID: 6575
		private IParserHelper _parserHelper;

		// Token: 0x040019B0 RID: 6576
		private ParserContext _parserContext;

		// Token: 0x02000895 RID: 2197
		internal class UnknownMarkupExtension
		{
		}
	}
}
