using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000189 RID: 393
	internal sealed class ODataJsonLightEntityReferenceLinkDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x06000B1F RID: 2847 RVA: 0x00024D90 File Offset: 0x00022F90
		internal ODataJsonLightEntityReferenceLinkDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00024D9C File Offset: 0x00022F9C
		internal ODataEntityReferenceLinks ReadEntityReferenceLinks(IEdmNavigationProperty navigationProperty)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			base.ReadPayloadStart(ODataPayloadKind.EntityReferenceLinks, duplicatePropertyNamesChecker, false, false);
			ODataEntityReferenceLinks result = this.ReadEntityReferenceLinksImplementation(navigationProperty, duplicatePropertyNamesChecker);
			base.ReadPayloadEnd(false);
			return result;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00024E08 File Offset: 0x00023008
		internal Task<ODataEntityReferenceLinks> ReadEntityReferenceLinksAsync(IEdmNavigationProperty navigationProperty)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			return base.ReadPayloadStartAsync(ODataPayloadKind.EntityReferenceLinks, duplicatePropertyNamesChecker, false, false).FollowOnSuccessWith(delegate(Task t)
			{
				ODataEntityReferenceLinks result = this.ReadEntityReferenceLinksImplementation(navigationProperty, duplicatePropertyNamesChecker);
				this.ReadPayloadEnd(false);
				return result;
			});
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00024E58 File Offset: 0x00023058
		internal ODataEntityReferenceLink ReadEntityReferenceLink(IEdmNavigationProperty navigationProperty)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			base.ReadPayloadStart(ODataPayloadKind.EntityReferenceLink, duplicatePropertyNamesChecker, false, false);
			ODataEntityReferenceLink result = this.ReadEntityReferenceLinkImplementation(navigationProperty, duplicatePropertyNamesChecker);
			base.ReadPayloadEnd(false);
			return result;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00024EC4 File Offset: 0x000230C4
		internal Task<ODataEntityReferenceLink> ReadEntityReferenceLinkAsync(IEdmNavigationProperty navigationProperty)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			return base.ReadPayloadStartAsync(ODataPayloadKind.EntityReferenceLink, duplicatePropertyNamesChecker, false, false).FollowOnSuccessWith(delegate(Task t)
			{
				ODataEntityReferenceLink result = this.ReadEntityReferenceLinkImplementation(navigationProperty, duplicatePropertyNamesChecker);
				this.ReadPayloadEnd(false);
				return result;
			});
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00024F14 File Offset: 0x00023114
		private ODataEntityReferenceLinks ReadEntityReferenceLinksImplementation(IEdmNavigationProperty navigationProperty, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			ODataEntityReferenceLinks odataEntityReferenceLinks = new ODataEntityReferenceLinks();
			if (base.JsonLightInputContext.ReadingResponse)
			{
				ReaderValidationUtils.ValidateEntityReferenceLinkMetadataUri(base.MetadataUriParseResult, navigationProperty);
			}
			this.ReadEntityReferenceLinksAnnotations(odataEntityReferenceLinks, duplicatePropertyNamesChecker, true);
			base.JsonReader.ReadStartArray();
			List<ODataEntityReferenceLink> list = new List<ODataEntityReferenceLink>();
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker2 = base.JsonLightInputContext.CreateDuplicatePropertyNamesChecker();
			while (base.JsonReader.NodeType != JsonNodeType.EndArray)
			{
				ODataEntityReferenceLink item = this.ReadSingleEntityReferenceLink(duplicatePropertyNamesChecker2, false);
				list.Add(item);
				duplicatePropertyNamesChecker2.Clear();
			}
			base.JsonReader.ReadEndArray();
			this.ReadEntityReferenceLinksAnnotations(odataEntityReferenceLinks, duplicatePropertyNamesChecker, false);
			base.JsonReader.ReadEndObject();
			odataEntityReferenceLinks.Links = new ReadOnlyEnumerable<ODataEntityReferenceLink>(list);
			return odataEntityReferenceLinks;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00024FB8 File Offset: 0x000231B8
		private ODataEntityReferenceLink ReadEntityReferenceLinkImplementation(IEdmNavigationProperty navigationProperty, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			if (base.JsonLightInputContext.ReadingResponse)
			{
				ReaderValidationUtils.ValidateEntityReferenceLinkMetadataUri(base.MetadataUriParseResult, navigationProperty);
			}
			return this.ReadSingleEntityReferenceLink(duplicatePropertyNamesChecker, true);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000250F0 File Offset: 0x000232F0
		private void ReadEntityReferenceLinksAnnotations(ODataEntityReferenceLinks links, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool forLinksStart)
		{
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
				{
					throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLinks);
				};
				bool foundValueProperty = false;
				base.ProcessProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParseResult, string propertyName)
				{
					switch (propertyParseResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
						if (string.CompareOrdinal("value", propertyName) != 0)
						{
							throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_InvalidEntityReferenceLinksPropertyFound(propertyName, "value"));
						}
						foundValueProperty = true;
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyAnnotationInEntityReferenceLinks(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						if (string.CompareOrdinal("odata.nextLink", propertyName) == 0)
						{
							this.ReadEntityReferenceLinksNextLinkAnnotationValue(links);
							return;
						}
						if (string.CompareOrdinal("odata.count", propertyName) == 0)
						{
							this.ReadEntityReferenceCountAnnotationValue(links);
							return;
						}
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataJsonLightEntityReferenceLinkDeserializer_ReadEntityReferenceLinksAnnotations));
					}
				});
				if (foundValueProperty)
				{
					return;
				}
			}
			if (forLinksStart)
			{
				throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_ExpectedEntityReferenceLinksPropertyNotFound("value"));
			}
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00025184 File Offset: 0x00023384
		private void ReadEntityReferenceLinksNextLinkAnnotationValue(ODataEntityReferenceLinks links)
		{
			Uri nextPageLink = base.ReadAndValidateAnnotationStringValueAsUri("odata.nextLink");
			links.NextPageLink = nextPageLink;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000251A4 File Offset: 0x000233A4
		private void ReadEntityReferenceCountAnnotationValue(ODataEntityReferenceLinks links)
		{
			links.Count = new long?(base.ReadAndValidateAnnotationStringValueAsLong("odata.count"));
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000252E0 File Offset: 0x000234E0
		private ODataEntityReferenceLink ReadSingleEntityReferenceLink(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool topLevel)
		{
			ODataJsonLightEntityReferenceLinkDeserializer.<>c__DisplayClass12 CS$<>8__locals1 = new ODataJsonLightEntityReferenceLinkDeserializer.<>c__DisplayClass12();
			CS$<>8__locals1.<>4__this = this;
			if (!topLevel)
			{
				if (base.JsonReader.NodeType != JsonNodeType.StartObject)
				{
					throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkMustBeObjectValue(base.JsonReader.NodeType));
				}
				base.JsonReader.ReadStartObject();
			}
			ODataJsonLightEntityReferenceLinkDeserializer.<>c__DisplayClass12 CS$<>8__locals2 = CS$<>8__locals1;
			ODataEntityReferenceLink[] entityReferenceLink = new ODataEntityReferenceLink[1];
			CS$<>8__locals2.entityReferenceLink = entityReferenceLink;
			Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
			{
				throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_PropertyAnnotationForEntityReferenceLink(annotationName));
			};
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				base.ProcessProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
					{
						if (string.CompareOrdinal("url", propertyName) != 0)
						{
							throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_InvalidPropertyInEntityReferenceLink(propertyName, "url"));
						}
						if (CS$<>8__locals1.entityReferenceLink[0] != null)
						{
							throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_MultipleUriPropertiesInEntityReferenceLink("url"));
						}
						string text = CS$<>8__locals1.<>4__this.JsonReader.ReadStringValue("url");
						if (text == null)
						{
							throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_EntityReferenceLinkUrlCannotBeNull("url"));
						}
						CS$<>8__locals1.entityReferenceLink[0] = new ODataEntityReferenceLink
						{
							Url = CS$<>8__locals1.<>4__this.ProcessUriFromPayload(text)
						};
						ReaderValidationUtils.ValidateEntityReferenceLink(CS$<>8__locals1.entityReferenceLink[0]);
						return;
					}
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_InvalidAnnotationInEntityReferenceLink(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						CS$<>8__locals1.<>4__this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataJsonLightEntityReferenceLinkDeserializer_ReadSingleEntityReferenceLink));
					}
				});
			}
			if (CS$<>8__locals1.entityReferenceLink[0] == null)
			{
				throw new ODataException(Strings.ODataJsonLightEntityReferenceLinkDeserializer_MissingEntityReferenceLinkProperty("url"));
			}
			base.JsonReader.ReadEndObject();
			return CS$<>8__locals1.entityReferenceLink[0];
		}
	}
}
