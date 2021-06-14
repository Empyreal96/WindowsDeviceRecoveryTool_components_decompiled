using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001F8 RID: 504
	internal sealed class ODataAtomEntityReferenceLinkDeserializer : ODataAtomDeserializer
	{
		// Token: 0x06000F5C RID: 3932 RVA: 0x00036DB8 File Offset: 0x00034FB8
		internal ODataAtomEntityReferenceLinkDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.ODataLinksElementName = nameTable.Add("links");
			this.ODataCountElementName = nameTable.Add("count");
			this.ODataNextElementName = nameTable.Add("next");
			this.ODataUriElementName = nameTable.Add("uri");
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00036E1C File Offset: 0x0003501C
		internal ODataEntityReferenceLinks ReadEntityReferenceLinks()
		{
			base.ReadPayloadStart();
			if (!base.XmlReader.NamespaceEquals(base.XmlReader.ODataNamespace) || !base.XmlReader.LocalNameEquals(this.ODataLinksElementName))
			{
				throw new ODataException(Strings.ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksStartElement(base.XmlReader.LocalName, base.XmlReader.NamespaceURI));
			}
			ODataEntityReferenceLinks result = this.ReadLinksElement();
			base.ReadPayloadEnd();
			return result;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00036E8C File Offset: 0x0003508C
		internal ODataEntityReferenceLink ReadEntityReferenceLink()
		{
			base.ReadPayloadStart();
			if ((!base.XmlReader.NamespaceEquals(base.XmlReader.ODataNamespace) && !base.XmlReader.NamespaceEquals(base.XmlReader.ODataMetadataNamespace)) || !base.XmlReader.LocalNameEquals(this.ODataUriElementName))
			{
				throw new ODataException(Strings.ODataAtomEntityReferenceLinkDeserializer_InvalidEntityReferenceLinkStartElement(base.XmlReader.LocalName, base.XmlReader.NamespaceURI));
			}
			ODataEntityReferenceLink result = this.ReadUriElement();
			base.ReadPayloadEnd();
			return result;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00036F11 File Offset: 0x00035111
		private static void VerifyEntityReferenceLinksElementNotFound(ref ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask elementsFoundBitField, ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask elementFoundBitMask, string elementNamespace, string elementName)
		{
			if ((elementsFoundBitField & elementFoundBitMask) == elementFoundBitMask)
			{
				throw new ODataException(Strings.ODataAtomEntityReferenceLinkDeserializer_MultipleEntityReferenceLinksElementsWithSameName(elementNamespace, elementName));
			}
			elementsFoundBitField |= elementFoundBitMask;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00036F30 File Offset: 0x00035130
		private ODataEntityReferenceLinks ReadLinksElement()
		{
			ODataEntityReferenceLinks odataEntityReferenceLinks = new ODataEntityReferenceLinks();
			List<ODataEntityReferenceLink> list = new List<ODataEntityReferenceLink>();
			ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask duplicateEntityReferenceLinksElementBitMask = ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask.None;
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.Read();
				for (;;)
				{
					XmlNodeType nodeType = base.XmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							goto IL_16F;
						}
					}
					else if (base.XmlReader.NamespaceEquals(base.XmlReader.ODataMetadataNamespace) && base.XmlReader.LocalNameEquals(this.ODataCountElementName) && base.Version >= ODataVersion.V2)
					{
						ODataAtomEntityReferenceLinkDeserializer.VerifyEntityReferenceLinksElementNotFound(ref duplicateEntityReferenceLinksElementBitMask, ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask.Count, base.XmlReader.ODataMetadataNamespace, "count");
						long value = (long)AtomValueUtils.ReadPrimitiveValue(base.XmlReader, EdmCoreModel.Instance.GetInt64(false));
						odataEntityReferenceLinks.Count = new long?(value);
						base.XmlReader.Read();
					}
					else
					{
						if (!base.XmlReader.NamespaceEquals(base.XmlReader.ODataNamespace))
						{
							goto IL_16F;
						}
						if (base.XmlReader.LocalNameEquals(this.ODataUriElementName))
						{
							ODataEntityReferenceLink item = this.ReadUriElement();
							list.Add(item);
						}
						else
						{
							if (!base.XmlReader.LocalNameEquals(this.ODataNextElementName) || base.Version < ODataVersion.V2)
							{
								goto IL_16F;
							}
							ODataAtomEntityReferenceLinkDeserializer.VerifyEntityReferenceLinksElementNotFound(ref duplicateEntityReferenceLinksElementBitMask, ODataAtomEntityReferenceLinkDeserializer.DuplicateEntityReferenceLinksElementBitMask.NextLink, base.XmlReader.ODataNamespace, "next");
							Uri xmlBaseUri = base.XmlReader.XmlBaseUri;
							string uriFromPayload = base.XmlReader.ReadElementValue();
							odataEntityReferenceLinks.NextPageLink = base.ProcessUriFromPayload(uriFromPayload, xmlBaseUri);
						}
					}
					IL_17A:
					if (base.XmlReader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					continue;
					IL_16F:
					base.XmlReader.Skip();
					goto IL_17A;
				}
			}
			base.XmlReader.Read();
			odataEntityReferenceLinks.Links = new ReadOnlyEnumerable<ODataEntityReferenceLink>(list);
			return odataEntityReferenceLinks;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x000370E4 File Offset: 0x000352E4
		private ODataEntityReferenceLink ReadUriElement()
		{
			ODataEntityReferenceLink odataEntityReferenceLink = new ODataEntityReferenceLink();
			Uri xmlBaseUri = base.XmlReader.XmlBaseUri;
			string uriFromPayload = base.XmlReader.ReadElementValue();
			Uri url = base.ProcessUriFromPayload(uriFromPayload, xmlBaseUri);
			odataEntityReferenceLink.Url = url;
			ReaderValidationUtils.ValidateEntityReferenceLink(odataEntityReferenceLink);
			return odataEntityReferenceLink;
		}

		// Token: 0x0400056F RID: 1391
		private readonly string ODataLinksElementName;

		// Token: 0x04000570 RID: 1392
		private readonly string ODataCountElementName;

		// Token: 0x04000571 RID: 1393
		private readonly string ODataNextElementName;

		// Token: 0x04000572 RID: 1394
		private readonly string ODataUriElementName;

		// Token: 0x020001F9 RID: 505
		[Flags]
		private enum DuplicateEntityReferenceLinksElementBitMask
		{
			// Token: 0x04000574 RID: 1396
			None = 0,
			// Token: 0x04000575 RID: 1397
			Count = 1,
			// Token: 0x04000576 RID: 1398
			NextLink = 2
		}
	}
}
