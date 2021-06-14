using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C2 RID: 450
	internal class ODataVerboseJsonPropertyAndValueSerializer : ODataVerboseJsonSerializer
	{
		// Token: 0x06000DE6 RID: 3558 RVA: 0x000307A4 File Offset: 0x0002E9A4
		internal ODataVerboseJsonPropertyAndValueSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00030804 File Offset: 0x0002EA04
		internal void WriteTopLevelProperty(ODataProperty property)
		{
			base.WriteTopLevelPayload(delegate()
			{
				this.JsonWriter.StartObjectScope();
				this.WriteProperty(property, null, false, this.CreateDuplicatePropertyNamesChecker(), null);
				this.JsonWriter.EndObjectScope();
			});
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00030838 File Offset: 0x0002EA38
		internal void WriteProperties(IEdmStructuredType owningType, IEnumerable<ODataProperty> properties, bool isComplexValue, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			if (properties == null)
			{
				return;
			}
			foreach (ODataProperty property in properties)
			{
				this.WriteProperty(property, owningType, !isComplexValue, duplicatePropertyNamesChecker, projectedProperties);
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00030890 File Offset: 0x0002EA90
		internal void WritePrimitiveValue(object value, CollectionWithoutExpectedTypeValidator collectionValidator, IEdmTypeReference expectedTypeReference)
		{
			IEdmPrimitiveTypeReference primitiveTypeReference = EdmLibraryExtensions.GetPrimitiveTypeReference(value.GetType());
			if (collectionValidator != null)
			{
				if (primitiveTypeReference == null)
				{
					throw new ODataException(Strings.ValidationUtils_UnsupportedPrimitiveType(value.GetType().FullName));
				}
				collectionValidator.ValidateCollectionItem(primitiveTypeReference.FullName(), EdmTypeKind.Primitive);
			}
			if (expectedTypeReference != null)
			{
				ValidationUtils.ValidateIsExpectedPrimitiveType(value, primitiveTypeReference, expectedTypeReference);
			}
			if (primitiveTypeReference != null && primitiveTypeReference.IsSpatial())
			{
				string typeName = primitiveTypeReference.FullName();
				PrimitiveConverter.Instance.WriteVerboseJson(value, base.JsonWriter, typeName, base.Version);
				return;
			}
			base.JsonWriter.WritePrimitiveValue(value, base.Version);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0003091C File Offset: 0x0002EB1C
		internal void WriteComplexValue(ODataComplexValue complexValue, IEdmTypeReference propertyTypeReference, bool isOpenPropertyType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator)
		{
			this.IncreaseRecursionDepth();
			base.JsonWriter.StartObjectScope();
			string text = complexValue.TypeName;
			if (collectionValidator != null)
			{
				collectionValidator.ValidateCollectionItem(text, EdmTypeKind.Complex);
			}
			IEdmComplexTypeReference edmComplexTypeReference = TypeNameOracle.ResolveAndValidateTypeNameForValue(base.Model, propertyTypeReference, complexValue, isOpenPropertyType).AsComplexOrNull();
			string text2;
			text = base.VerboseJsonOutputContext.TypeNameOracle.GetValueTypeNameForWriting(complexValue, edmComplexTypeReference, complexValue.GetAnnotation<SerializationTypeNameAnnotation>(), collectionValidator, out text2);
			if (text != null)
			{
				ODataJsonWriterUtils.WriteMetadataWithTypeName(base.JsonWriter, text);
			}
			this.WriteProperties((edmComplexTypeReference == null) ? null : edmComplexTypeReference.ComplexDefinition(), complexValue.Properties, true, duplicatePropertyNamesChecker, null);
			base.JsonWriter.EndObjectScope();
			this.DecreaseRecursionDepth();
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000309BC File Offset: 0x0002EBBC
		internal void WriteCollectionValue(ODataCollectionValue collectionValue, IEdmTypeReference metadataTypeReference, bool isOpenPropertyType)
		{
			this.IncreaseRecursionDepth();
			base.JsonWriter.StartObjectScope();
			IEdmCollectionTypeReference edmCollectionTypeReference = (IEdmCollectionTypeReference)TypeNameOracle.ResolveAndValidateTypeNameForValue(base.Model, metadataTypeReference, collectionValue, isOpenPropertyType);
			string itemTypeNameFromCollection;
			string valueTypeNameForWriting = base.VerboseJsonOutputContext.TypeNameOracle.GetValueTypeNameForWriting(collectionValue, edmCollectionTypeReference, collectionValue.GetAnnotation<SerializationTypeNameAnnotation>(), null, out itemTypeNameFromCollection);
			if (valueTypeNameForWriting != null)
			{
				ODataJsonWriterUtils.WriteMetadataWithTypeName(base.JsonWriter, valueTypeNameForWriting);
			}
			base.JsonWriter.WriteDataArrayName();
			base.JsonWriter.StartArrayScope();
			IEnumerable items = collectionValue.Items;
			if (items != null)
			{
				IEdmTypeReference edmTypeReference = (edmCollectionTypeReference == null) ? null : edmCollectionTypeReference.ElementType();
				CollectionWithoutExpectedTypeValidator collectionValidator = new CollectionWithoutExpectedTypeValidator(itemTypeNameFromCollection);
				DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = null;
				foreach (object obj in items)
				{
					ValidationUtils.ValidateCollectionItem(obj, false);
					ODataComplexValue odataComplexValue = obj as ODataComplexValue;
					if (odataComplexValue != null)
					{
						if (duplicatePropertyNamesChecker == null)
						{
							duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
						}
						this.WriteComplexValue(odataComplexValue, edmTypeReference, false, duplicatePropertyNamesChecker, collectionValidator);
						duplicatePropertyNamesChecker.Clear();
					}
					else
					{
						this.WritePrimitiveValue(obj, collectionValidator, edmTypeReference);
					}
				}
			}
			base.JsonWriter.EndArrayScope();
			base.JsonWriter.EndObjectScope();
			this.DecreaseRecursionDepth();
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00030AFC File Offset: 0x0002ECFC
		internal void WriteStreamReferenceValueContent(ODataStreamReferenceValue streamReferenceValue)
		{
			Uri editLink = streamReferenceValue.EditLink;
			if (editLink != null)
			{
				base.JsonWriter.WriteName("edit_media");
				base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(editLink));
			}
			if (streamReferenceValue.ReadLink != null)
			{
				base.JsonWriter.WriteName("media_src");
				base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(streamReferenceValue.ReadLink));
			}
			if (streamReferenceValue.ContentType != null)
			{
				base.JsonWriter.WriteName("content_type");
				base.JsonWriter.WriteValue(streamReferenceValue.ContentType);
			}
			string etag = streamReferenceValue.ETag;
			if (etag != null)
			{
				this.WriteETag("media_etag", etag);
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00030BAF File Offset: 0x0002EDAF
		internal void WriteETag(string etagName, string etagValue)
		{
			base.JsonWriter.WriteName(etagName);
			base.JsonWriter.WriteValue(etagValue);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00030BC9 File Offset: 0x0002EDC9
		[Conditional("DEBUG")]
		internal void AssertRecursionDepthIsZero()
		{
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00030BCC File Offset: 0x0002EDCC
		private void WriteProperty(ODataProperty property, IEdmStructuredType owningType, bool allowStreamProperty, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ProjectedPropertiesAnnotation projectedProperties)
		{
			WriterValidationUtils.ValidatePropertyNotNull(property);
			string name = property.Name;
			object value = property.Value;
			if (projectedProperties.ShouldSkipProperty(name))
			{
				return;
			}
			WriterValidationUtils.ValidatePropertyName(name);
			duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(property);
			IEdmProperty edmProperty = WriterValidationUtils.ValidatePropertyDefined(name, owningType);
			IEdmTypeReference edmTypeReference = (edmProperty == null) ? null : edmProperty.Type;
			if ((edmTypeReference != null && edmTypeReference.IsSpatial()) || (edmTypeReference == null && value is ISpatial))
			{
				ODataVersionChecker.CheckSpatialValue(base.Version);
			}
			base.JsonWriter.WriteName(name);
			if (value == null)
			{
				WriterValidationUtils.ValidateNullPropertyValue(edmTypeReference, name, base.MessageWriterSettings.WriterBehavior, base.Model);
				base.JsonWriter.WriteValue(null);
				return;
			}
			bool flag = owningType != null && owningType.IsOpen && edmTypeReference == null;
			if (flag)
			{
				ValidationUtils.ValidateOpenPropertyValue(name, value);
			}
			ODataComplexValue odataComplexValue = value as ODataComplexValue;
			if (odataComplexValue != null)
			{
				this.WriteComplexValue(odataComplexValue, edmTypeReference, flag, base.CreateDuplicatePropertyNamesChecker(), null);
				return;
			}
			ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
			if (odataCollectionValue != null)
			{
				ODataVersionChecker.CheckCollectionValueProperties(base.Version, name);
				this.WriteCollectionValue(odataCollectionValue, edmTypeReference, flag);
				return;
			}
			ODataStreamReferenceValue odataStreamReferenceValue = value as ODataStreamReferenceValue;
			if (odataStreamReferenceValue == null)
			{
				this.WritePrimitiveValue(value, null, edmTypeReference);
				return;
			}
			if (!allowStreamProperty)
			{
				throw new ODataException(Strings.ODataWriter_StreamPropertiesMustBePropertiesOfODataEntry(name));
			}
			WriterValidationUtils.ValidateStreamReferenceProperty(property, edmProperty, base.Version, base.WritingResponse);
			WriterValidationUtils.ValidateStreamReferenceValue(odataStreamReferenceValue, false);
			this.WriteStreamReferenceValue(odataStreamReferenceValue);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00030D1C File Offset: 0x0002EF1C
		private void WriteStreamReferenceValue(ODataStreamReferenceValue streamReferenceValue)
		{
			base.JsonWriter.StartObjectScope();
			base.JsonWriter.WriteName("__mediaresource");
			base.JsonWriter.StartObjectScope();
			this.WriteStreamReferenceValueContent(streamReferenceValue);
			base.JsonWriter.EndObjectScope();
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00030D6C File Offset: 0x0002EF6C
		private void IncreaseRecursionDepth()
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref this.recursionDepth, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00030D89 File Offset: 0x0002EF89
		private void DecreaseRecursionDepth()
		{
			this.recursionDepth--;
		}

		// Token: 0x040004AB RID: 1195
		private int recursionDepth;
	}
}
