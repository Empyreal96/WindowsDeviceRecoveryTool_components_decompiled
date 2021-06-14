using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000231 RID: 561
	internal sealed class ODataVerboseJsonEntityReferenceLinkDeserializer : ODataVerboseJsonDeserializer
	{
		// Token: 0x060011D5 RID: 4565 RVA: 0x00042230 File Offset: 0x00040430
		internal ODataVerboseJsonEntityReferenceLinkDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004223C File Offset: 0x0004043C
		internal ODataEntityReferenceLinks ReadEntityReferenceLinks()
		{
			bool flag = base.Version >= ODataVersion.V2 && base.ReadingResponse;
			ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask entityReferenceLinksWrapperPropertyBitMask = ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask.None;
			ODataEntityReferenceLinks odataEntityReferenceLinks = new ODataEntityReferenceLinks();
			base.ReadPayloadStart(false);
			if (flag)
			{
				base.JsonReader.ReadStartObject();
				if (!this.ReadEntityReferenceLinkProperties(odataEntityReferenceLinks, ref entityReferenceLinksWrapperPropertyBitMask))
				{
					throw new ODataException(Strings.ODataJsonEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksResultsPropertyNotFound);
				}
			}
			base.JsonReader.ReadStartArray();
			List<ODataEntityReferenceLink> list = new List<ODataEntityReferenceLink>();
			while (base.JsonReader.NodeType != JsonNodeType.EndArray)
			{
				ODataEntityReferenceLink item = this.ReadSingleEntityReferenceLink();
				list.Add(item);
			}
			base.JsonReader.ReadEndArray();
			if (flag)
			{
				this.ReadEntityReferenceLinkProperties(odataEntityReferenceLinks, ref entityReferenceLinksWrapperPropertyBitMask);
				base.JsonReader.ReadEndObject();
			}
			odataEntityReferenceLinks.Links = new ReadOnlyEnumerable<ODataEntityReferenceLink>(list);
			base.ReadPayloadEnd(false);
			return odataEntityReferenceLinks;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000422FC File Offset: 0x000404FC
		internal ODataEntityReferenceLink ReadEntityReferenceLink()
		{
			base.ReadPayloadStart(false);
			ODataEntityReferenceLink result = this.ReadSingleEntityReferenceLink();
			base.ReadPayloadEnd(false);
			return result;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00042320 File Offset: 0x00040520
		private bool ReadEntityReferenceLinkProperties(ODataEntityReferenceLinks entityReferenceLinks, ref ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask propertiesFoundBitField)
		{
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				string a;
				if ((a = text) != null)
				{
					if (a == "results")
					{
						ODataVerboseJsonReaderUtils.VerifyEntityReferenceLinksWrapperPropertyNotFound(ref propertiesFoundBitField, ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask.Results, "results");
						return true;
					}
					if (a == "__count")
					{
						ODataVerboseJsonReaderUtils.VerifyEntityReferenceLinksWrapperPropertyNotFound(ref propertiesFoundBitField, ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask.Count, "__count");
						object value = base.JsonReader.ReadPrimitiveValue();
						long? num = (long?)ODataVerboseJsonReaderUtils.ConvertValue(value, EdmCoreModel.Instance.GetInt64(true), base.MessageReaderSettings, base.Version, true, text);
						ODataVerboseJsonReaderUtils.ValidateCountPropertyInEntityReferenceLinks(num);
						entityReferenceLinks.Count = num;
						continue;
					}
					if (a == "__next")
					{
						ODataVerboseJsonReaderUtils.VerifyEntityReferenceLinksWrapperPropertyNotFound(ref propertiesFoundBitField, ODataVerboseJsonReaderUtils.EntityReferenceLinksWrapperPropertyBitMask.NextPageLink, "__next");
						string text2 = base.JsonReader.ReadStringValue("__next");
						ODataVerboseJsonReaderUtils.ValidateEntityReferenceLinksStringProperty(text2, "__next");
						entityReferenceLinks.NextPageLink = base.ProcessUriFromPayload(text2);
						continue;
					}
				}
				base.JsonReader.SkipValue();
			}
			return false;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00042424 File Offset: 0x00040624
		private ODataEntityReferenceLink ReadSingleEntityReferenceLink()
		{
			if (base.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue(base.JsonReader.NodeType));
			}
			base.JsonReader.ReadStartObject();
			ODataEntityReferenceLink odataEntityReferenceLink = new ODataEntityReferenceLink();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("uri", strB) == 0)
				{
					if (odataEntityReferenceLink.Url != null)
					{
						throw new ODataException(Strings.ODataJsonEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink);
					}
					string text = base.JsonReader.ReadStringValue("uri");
					if (text == null)
					{
						throw new ODataException(Strings.ODataJsonEntityReferenceLinkDeserializer_EntityReferenceLinkUriCannotBeNull);
					}
					odataEntityReferenceLink.Url = base.ProcessUriFromPayload(text);
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			ReaderValidationUtils.ValidateEntityReferenceLink(odataEntityReferenceLink);
			base.JsonReader.ReadEndObject();
			return odataEntityReferenceLink;
		}
	}
}
