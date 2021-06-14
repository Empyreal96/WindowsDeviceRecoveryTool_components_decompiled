using System;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001F1 RID: 497
	internal sealed class ODataAtomErrorDeserializer : ODataAtomDeserializer
	{
		// Token: 0x06000F43 RID: 3907 RVA: 0x00036422 File Offset: 0x00034622
		internal ODataAtomErrorDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0003642C File Offset: 0x0003462C
		internal static ODataError ReadErrorElement(BufferingXmlReader xmlReader, int maxInnerErrorDepth)
		{
			ODataError odataError = new ODataError();
			ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask duplicateErrorElementPropertyBitMask = ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask.None;
			if (!xmlReader.IsEmptyElement)
			{
				xmlReader.Read();
				for (;;)
				{
					XmlNodeType nodeType = xmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							goto IL_E4;
						}
					}
					else
					{
						string localName;
						if (!xmlReader.NamespaceEquals(xmlReader.ODataMetadataNamespace) || (localName = xmlReader.LocalName) == null)
						{
							goto IL_E4;
						}
						if (!(localName == "code"))
						{
							if (!(localName == "message"))
							{
								if (!(localName == "innererror"))
								{
									goto IL_E4;
								}
								ODataAtomErrorDeserializer.VerifyErrorElementNotFound(ref duplicateErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask.InnerError, "innererror");
								odataError.InnerError = ODataAtomErrorDeserializer.ReadInnerErrorElement(xmlReader, 0, maxInnerErrorDepth);
							}
							else
							{
								ODataAtomErrorDeserializer.VerifyErrorElementNotFound(ref duplicateErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask.Message, "message");
								odataError.MessageLanguage = xmlReader.GetAttribute(xmlReader.XmlLangAttributeName, xmlReader.XmlNamespace);
								odataError.Message = xmlReader.ReadElementValue();
							}
						}
						else
						{
							ODataAtomErrorDeserializer.VerifyErrorElementNotFound(ref duplicateErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask.Code, "code");
							odataError.ErrorCode = xmlReader.ReadElementValue();
						}
					}
					IL_EA:
					if (xmlReader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					continue;
					IL_E4:
					xmlReader.Skip();
					goto IL_EA;
				}
			}
			return odataError;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00036534 File Offset: 0x00034734
		internal ODataError ReadTopLevelError()
		{
			ODataError result;
			try
			{
				base.XmlReader.DisableInStreamErrorDetection = true;
				base.ReadPayloadStart();
				if (!base.XmlReader.NamespaceEquals(base.XmlReader.ODataMetadataNamespace) || !base.XmlReader.LocalNameEquals(base.XmlReader.ODataErrorElementName))
				{
					throw new ODataErrorException(Strings.ODataAtomErrorDeserializer_InvalidRootElement(base.XmlReader.Name, base.XmlReader.NamespaceURI));
				}
				ODataError odataError = ODataAtomErrorDeserializer.ReadErrorElement(base.XmlReader, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
				base.XmlReader.Read();
				base.ReadPayloadEnd();
				result = odataError;
			}
			finally
			{
				base.XmlReader.DisableInStreamErrorDetection = false;
			}
			return result;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x000365F4 File Offset: 0x000347F4
		private static void VerifyErrorElementNotFound(ref ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask elementsFoundBitField, ODataAtomErrorDeserializer.DuplicateErrorElementPropertyBitMask elementFoundBitMask, string elementName)
		{
			if ((elementsFoundBitField & elementFoundBitMask) == elementFoundBitMask)
			{
				throw new ODataException(Strings.ODataAtomErrorDeserializer_MultipleErrorElementsWithSameName(elementName));
			}
			elementsFoundBitField |= elementFoundBitMask;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0003660F File Offset: 0x0003480F
		private static void VerifyInnerErrorElementNotFound(ref ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask elementsFoundBitField, ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask elementFoundBitMask, string elementName)
		{
			if ((elementsFoundBitField & elementFoundBitMask) == elementFoundBitMask)
			{
				throw new ODataException(Strings.ODataAtomErrorDeserializer_MultipleInnerErrorElementsWithSameName(elementName));
			}
			elementsFoundBitField |= elementFoundBitMask;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0003662C File Offset: 0x0003482C
		private static ODataInnerError ReadInnerErrorElement(BufferingXmlReader xmlReader, int recursionDepth, int maxInnerErrorDepth)
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref recursionDepth, maxInnerErrorDepth);
			ODataInnerError odataInnerError = new ODataInnerError();
			ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask duplicateInnerErrorElementPropertyBitMask = ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask.None;
			if (!xmlReader.IsEmptyElement)
			{
				xmlReader.Read();
				for (;;)
				{
					XmlNodeType nodeType = xmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							goto IL_FC;
						}
					}
					else
					{
						string localName;
						if (!xmlReader.NamespaceEquals(xmlReader.ODataMetadataNamespace) || (localName = xmlReader.LocalName) == null)
						{
							goto IL_FC;
						}
						if (!(localName == "message"))
						{
							if (!(localName == "type"))
							{
								if (!(localName == "stacktrace"))
								{
									if (!(localName == "internalexception"))
									{
										goto IL_FC;
									}
									ODataAtomErrorDeserializer.VerifyInnerErrorElementNotFound(ref duplicateInnerErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask.InternalException, "internalexception");
									odataInnerError.InnerError = ODataAtomErrorDeserializer.ReadInnerErrorElement(xmlReader, recursionDepth, maxInnerErrorDepth);
								}
								else
								{
									ODataAtomErrorDeserializer.VerifyInnerErrorElementNotFound(ref duplicateInnerErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask.StackTrace, "stacktrace");
									odataInnerError.StackTrace = xmlReader.ReadElementValue();
								}
							}
							else
							{
								ODataAtomErrorDeserializer.VerifyInnerErrorElementNotFound(ref duplicateInnerErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask.TypeName, "type");
								odataInnerError.TypeName = xmlReader.ReadElementValue();
							}
						}
						else
						{
							ODataAtomErrorDeserializer.VerifyInnerErrorElementNotFound(ref duplicateInnerErrorElementPropertyBitMask, ODataAtomErrorDeserializer.DuplicateInnerErrorElementPropertyBitMask.Message, "message");
							odataInnerError.Message = xmlReader.ReadElementValue();
						}
					}
					IL_102:
					if (xmlReader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					continue;
					IL_FC:
					xmlReader.Skip();
					goto IL_102;
				}
			}
			xmlReader.Read();
			return odataInnerError;
		}

		// Token: 0x020001F2 RID: 498
		[Flags]
		private enum DuplicateErrorElementPropertyBitMask
		{
			// Token: 0x04000562 RID: 1378
			None = 0,
			// Token: 0x04000563 RID: 1379
			Code = 1,
			// Token: 0x04000564 RID: 1380
			Message = 2,
			// Token: 0x04000565 RID: 1381
			InnerError = 4
		}

		// Token: 0x020001F3 RID: 499
		[Flags]
		private enum DuplicateInnerErrorElementPropertyBitMask
		{
			// Token: 0x04000567 RID: 1383
			None = 0,
			// Token: 0x04000568 RID: 1384
			Message = 1,
			// Token: 0x04000569 RID: 1385
			TypeName = 2,
			// Token: 0x0400056A RID: 1386
			StackTrace = 4,
			// Token: 0x0400056B RID: 1387
			InternalException = 8
		}
	}
}
