using System;
using System.Collections.Generic;
using System.Spatial;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200018D RID: 397
	internal class ODataJsonLightPropertySerializer : ODataJsonLightSerializer
	{
		// Token: 0x06000B5C RID: 2908 RVA: 0x0002726E File Offset: 0x0002546E
		internal ODataJsonLightPropertySerializer(ODataJsonLightOutputContext jsonLightOutputContext) : base(jsonLightOutputContext)
		{
			this.jsonLightValueSerializer = new ODataJsonLightValueSerializer(this);
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00027283 File Offset: 0x00025483
		internal ODataJsonLightValueSerializer JsonLightValueSerializer
		{
			get
			{
				return this.jsonLightValueSerializer;
			}
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00027310 File Offset: 0x00025510
		internal void WriteTopLevelProperty(ODataProperty property)
		{
			base.WriteTopLevelPayload(delegate
			{
				this.JsonWriter.StartObjectScope();
				ODataJsonLightMetadataUriBuilder odataJsonLightMetadataUriBuilder = this.JsonLightOutputContext.CreateMetadataUriBuilder();
				Uri metadataUri;
				if (odataJsonLightMetadataUriBuilder.TryBuildMetadataUriForValue(property, out metadataUri))
				{
					this.WriteMetadataUriProperty(metadataUri);
				}
				this.WriteProperty(property, null, true, false, this.CreateDuplicatePropertyNamesChecker(), null);
				this.JsonWriter.EndObjectScope();
			});
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00027344 File Offset: 0x00025544
		internal void WriteProperties(IEdmStructuredType owningType, IEnumerable<ODataProperty> properties, bool isComplexValue, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			if (properties == null)
			{
				return;
			}
			foreach (ODataProperty property in properties)
			{
				this.WriteProperty(property, owningType, false, !isComplexValue, duplicatePropertyNamesChecker, projectedProperties);
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0002739C File Offset: 0x0002559C
		private static bool IsOpenPropertyType(ODataProperty property, IEdmStructuredType owningType, IEdmProperty edmProperty)
		{
			if (property.SerializationInfo != null)
			{
				return property.SerializationInfo.PropertyKind == ODataPropertyKind.Open;
			}
			return owningType != null && owningType.IsOpen && edmProperty == null;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000273C8 File Offset: 0x000255C8
		private void WriteProperty(ODataProperty property, IEdmStructuredType owningType, bool isTopLevel, bool allowStreamProperty, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			WriterValidationUtils.ValidatePropertyNotNull(property);
			string name = property.Name;
			if (projectedProperties.ShouldSkipProperty(name))
			{
				return;
			}
			WriterValidationUtils.ValidatePropertyName(name);
			duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(property);
			IEdmProperty edmProperty = WriterValidationUtils.ValidatePropertyDefined(name, owningType);
			IEdmTypeReference edmTypeReference = (edmProperty == null) ? null : edmProperty.Type;
			ODataValue odataValue = property.ODataValue;
			ODataPrimitiveValue odataPrimitiveValue = odataValue as ODataPrimitiveValue;
			bool flag = (edmTypeReference != null && edmTypeReference.IsSpatial()) || (edmTypeReference == null && odataPrimitiveValue != null && odataPrimitiveValue.Value is ISpatial);
			if (flag)
			{
				ODataVersionChecker.CheckSpatialValue(base.Version);
			}
			ODataStreamReferenceValue odataStreamReferenceValue = odataValue as ODataStreamReferenceValue;
			if (odataStreamReferenceValue != null)
			{
				if (!allowStreamProperty)
				{
					throw new ODataException(Strings.ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry(name));
				}
				WriterValidationUtils.ValidateStreamReferenceProperty(property, edmProperty, base.Version, base.WritingResponse);
				this.WriteStreamReferenceProperty(name, odataStreamReferenceValue);
				return;
			}
			else
			{
				string text = isTopLevel ? "value" : name;
				if (odataValue is ODataNullValue || odataValue == null)
				{
					WriterValidationUtils.ValidateNullPropertyValue(edmTypeReference, name, base.MessageWriterSettings.WriterBehavior, base.Model);
					if (isTopLevel)
					{
						base.JsonWriter.WriteName("odata.null");
						base.JsonWriter.WriteValue(true);
						return;
					}
					base.JsonWriter.WriteName(text);
					this.JsonLightValueSerializer.WriteNullValue();
					return;
				}
				else
				{
					bool flag2 = ODataJsonLightPropertySerializer.IsOpenPropertyType(property, owningType, edmProperty);
					if (flag2)
					{
						ValidationUtils.ValidateOpenPropertyValue(name, odataValue);
					}
					ODataComplexValue odataComplexValue = odataValue as ODataComplexValue;
					if (odataComplexValue != null)
					{
						if (!isTopLevel)
						{
							base.JsonWriter.WriteName(text);
						}
						this.JsonLightValueSerializer.WriteComplexValue(odataComplexValue, edmTypeReference, isTopLevel, flag2, base.CreateDuplicatePropertyNamesChecker());
						return;
					}
					IEdmTypeReference typeReferenceFromValue = TypeNameOracle.ResolveAndValidateTypeNameForValue(base.Model, edmTypeReference, odataValue, flag2);
					ODataCollectionValue odataCollectionValue = odataValue as ODataCollectionValue;
					if (odataCollectionValue != null)
					{
						string valueTypeNameForWriting = base.JsonLightOutputContext.TypeNameOracle.GetValueTypeNameForWriting(odataCollectionValue, edmTypeReference, typeReferenceFromValue, flag2);
						this.WritePropertyTypeName(text, valueTypeNameForWriting, isTopLevel);
						base.JsonWriter.WriteName(text);
						ODataVersionChecker.CheckCollectionValueProperties(base.Version, name);
						this.JsonLightValueSerializer.WriteCollectionValue(odataCollectionValue, edmTypeReference, isTopLevel, false, flag2);
						return;
					}
					string valueTypeNameForWriting2 = base.JsonLightOutputContext.TypeNameOracle.GetValueTypeNameForWriting(odataPrimitiveValue, edmTypeReference, typeReferenceFromValue, flag2);
					this.WritePropertyTypeName(text, valueTypeNameForWriting2, isTopLevel);
					base.JsonWriter.WriteName(text);
					this.JsonLightValueSerializer.WritePrimitiveValue(odataPrimitiveValue.Value, edmTypeReference);
					return;
				}
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x000275F4 File Offset: 0x000257F4
		private void WriteStreamReferenceProperty(string propertyName, ODataStreamReferenceValue streamReferenceValue)
		{
			Uri editLink = streamReferenceValue.EditLink;
			if (editLink != null)
			{
				base.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.mediaEditLink");
				base.JsonWriter.WriteValue(base.UriToString(editLink));
			}
			Uri readLink = streamReferenceValue.ReadLink;
			if (readLink != null)
			{
				base.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.mediaReadLink");
				base.JsonWriter.WriteValue(base.UriToString(readLink));
			}
			string contentType = streamReferenceValue.ContentType;
			if (contentType != null)
			{
				base.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.mediaContentType");
				base.JsonWriter.WriteValue(contentType);
			}
			string etag = streamReferenceValue.ETag;
			if (etag != null)
			{
				base.JsonWriter.WritePropertyAnnotationName(propertyName, "odata.mediaETag");
				base.JsonWriter.WriteValue(etag);
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x000276B5 File Offset: 0x000258B5
		private void WritePropertyTypeName(string propertyName, string typeNameToWrite, bool isTopLevel)
		{
			if (typeNameToWrite != null)
			{
				if (isTopLevel)
				{
					base.JsonWriter.WriteName("odata.type");
					base.JsonWriter.WriteValue(typeNameToWrite);
					return;
				}
				ODataJsonLightWriterUtils.WriteODataTypePropertyAnnotation(base.JsonWriter, propertyName, typeNameToWrite);
			}
		}

		// Token: 0x04000418 RID: 1048
		private readonly ODataJsonLightValueSerializer jsonLightValueSerializer;
	}
}
