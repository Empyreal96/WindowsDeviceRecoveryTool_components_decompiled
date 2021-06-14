using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x020001A0 RID: 416
	internal sealed class ODataJsonLightServiceDocumentDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x06000CA8 RID: 3240 RVA: 0x0002B847 File Offset: 0x00029A47
		internal ODataJsonLightServiceDocumentDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002B850 File Offset: 0x00029A50
		internal ODataWorkspace ReadServiceDocument()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			base.ReadPayloadStart(ODataPayloadKind.ServiceDocument, duplicatePropertyNamesChecker, false, false);
			ODataWorkspace result = this.ReadServiceDocumentImplementation(duplicatePropertyNamesChecker);
			base.ReadPayloadEnd(false);
			return result;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002B8B4 File Offset: 0x00029AB4
		internal Task<ODataWorkspace> ReadServiceDocumentAsync()
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			return base.ReadPayloadStartAsync(ODataPayloadKind.ServiceDocument, duplicatePropertyNamesChecker, false, false).FollowOnSuccessWith(delegate(Task t)
			{
				ODataWorkspace result = this.ReadServiceDocumentImplementation(duplicatePropertyNamesChecker);
				this.ReadPayloadEnd(false);
				return result;
			});
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002BA28 File Offset: 0x00029C28
		private ODataWorkspace ReadServiceDocumentImplementation(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClass8 CS$<>8__locals1 = new ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClass8();
			CS$<>8__locals1.<>4__this = this;
			ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClass8 CS$<>8__locals2 = CS$<>8__locals1;
			List<ODataResourceCollectionInfo>[] collections = new List<ODataResourceCollectionInfo>[1];
			CS$<>8__locals2.collections = collections;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
				{
					throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInServiceDocument(annotationName, "value"));
				};
				base.ProcessProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
					{
						if (string.CompareOrdinal("value", propertyName) != 0)
						{
							throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInServiceDocument(propertyName, "value"));
						}
						if (CS$<>8__locals1.collections[0] != null)
						{
							throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInServiceDocument("value"));
						}
						CS$<>8__locals1.collections[0] = new List<ODataResourceCollectionInfo>();
						CS$<>8__locals1.<>4__this.JsonReader.ReadStartArray();
						DuplicatePropertyNamesChecker duplicatePropertyNamesChecker2 = CS$<>8__locals1.<>4__this.CreateDuplicatePropertyNamesChecker();
						while (CS$<>8__locals1.<>4__this.JsonReader.NodeType != JsonNodeType.EndArray)
						{
							ODataResourceCollectionInfo item = CS$<>8__locals1.<>4__this.ReadResourceCollection(duplicatePropertyNamesChecker2);
							CS$<>8__locals1.collections[0].Add(item);
							duplicatePropertyNamesChecker2.Clear();
						}
						CS$<>8__locals1.<>4__this.JsonReader.ReadEndArray();
						return;
					}
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInServiceDocument(propertyName, "value"));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						CS$<>8__locals1.<>4__this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						return;
					}
				});
			}
			if (CS$<>8__locals1.collections[0] == null)
			{
				throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_MissingValuePropertyInServiceDocument("value"));
			}
			base.JsonReader.ReadEndObject();
			return new ODataWorkspace
			{
				Collections = new ReadOnlyEnumerable<ODataResourceCollectionInfo>(CS$<>8__locals1.collections[0])
			};
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0002BBF4 File Offset: 0x00029DF4
		private ODataResourceCollectionInfo ReadResourceCollection(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClassf CS$<>8__locals1 = new ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClassf();
			CS$<>8__locals1.<>4__this = this;
			base.JsonReader.ReadStartObject();
			ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClassf CS$<>8__locals2 = CS$<>8__locals1;
			string[] entitySetName = new string[1];
			CS$<>8__locals2.entitySetName = entitySetName;
			ODataJsonLightServiceDocumentDeserializer.<>c__DisplayClassf CS$<>8__locals3 = CS$<>8__locals1;
			string[] entitySetUrl = new string[1];
			CS$<>8__locals3.entitySetUrl = entitySetUrl;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
				{
					throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationInResourceCollection(annotationName));
				};
				base.ProcessProperty(duplicatePropertyNamesChecker, readPropertyAnnotationValue, delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
						if (string.CompareOrdinal("name", propertyName) == 0)
						{
							if (CS$<>8__locals1.entitySetName[0] != null)
							{
								throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection("name"));
							}
							CS$<>8__locals1.entitySetName[0] = CS$<>8__locals1.<>4__this.JsonReader.ReadStringValue();
							return;
						}
						else
						{
							if (string.CompareOrdinal("url", propertyName) != 0)
							{
								throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_UnexpectedPropertyInResourceCollection(propertyName, "name", "url"));
							}
							if (CS$<>8__locals1.entitySetUrl[0] != null)
							{
								throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_DuplicatePropertiesInResourceCollection("url"));
							}
							CS$<>8__locals1.entitySetUrl[0] = CS$<>8__locals1.<>4__this.JsonReader.ReadStringValue();
							ValidationUtils.ValidateResourceCollectionInfoUrl(CS$<>8__locals1.entitySetUrl[0]);
							return;
						}
						break;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_PropertyAnnotationWithoutProperty(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_InstanceAnnotationInResourceCollection(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						CS$<>8__locals1.<>4__this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						return;
					}
				});
			}
			if (string.IsNullOrEmpty(CS$<>8__locals1.entitySetName[0]))
			{
				throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection("name"));
			}
			if (string.IsNullOrEmpty(CS$<>8__locals1.entitySetUrl[0]))
			{
				throw new ODataException(Strings.ODataJsonLightServiceDocumentDeserializer_MissingRequiredPropertyInResourceCollection("url"));
			}
			ODataResourceCollectionInfo result = new ODataResourceCollectionInfo
			{
				Url = base.ProcessUriFromPayload(CS$<>8__locals1.entitySetUrl[0]),
				Name = CS$<>8__locals1.entitySetName[0]
			};
			base.JsonReader.ReadEndObject();
			return result;
		}
	}
}
