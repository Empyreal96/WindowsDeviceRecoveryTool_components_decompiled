using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001B7 RID: 439
	internal class ODataVerboseJsonPropertyAndValueDeserializer : ODataVerboseJsonDeserializer
	{
		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002F029 File Offset: 0x0002D229
		internal ODataVerboseJsonPropertyAndValueDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002F034 File Offset: 0x0002D234
		internal ODataProperty ReadTopLevelProperty(IEdmStructuralProperty expectedProperty, IEdmTypeReference expectedPropertyTypeReference)
		{
			if (!base.Model.IsUserModel())
			{
				throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_TopLevelPropertyWithoutMetadata);
			}
			base.ReadPayloadStart(false);
			string expectedPropertyName = ReaderUtils.GetExpectedPropertyName(expectedProperty);
			object value = null;
			string text;
			if (this.ShouldReadTopLevelPropertyValueWithoutPropertyWrapper(expectedPropertyTypeReference))
			{
				text = (expectedPropertyName ?? string.Empty);
				value = this.ReadNonEntityValue(expectedPropertyTypeReference, null, null, true, text);
			}
			else
			{
				base.JsonReader.ReadStartObject();
				bool flag = false;
				string text2 = null;
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					text = base.JsonReader.ReadPropertyName();
					if (!ValidationUtils.IsValidPropertyName(text))
					{
						base.JsonReader.SkipValue();
					}
					else
					{
						if (flag)
						{
							throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload);
						}
						flag = true;
						text2 = text;
						value = this.ReadNonEntityValue(expectedPropertyTypeReference, null, null, true, text);
					}
				}
				if (!flag)
				{
					throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload);
				}
				ReaderValidationUtils.ValidateExpectedPropertyName(expectedPropertyName, text2);
				text = text2;
				base.JsonReader.Read();
			}
			base.ReadPayloadEnd(false);
			return new ODataProperty
			{
				Name = text,
				Value = value
			};
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0002F138 File Offset: 0x0002D338
		internal string FindTypeNameInPayload()
		{
			if (base.JsonReader.NodeType == JsonNodeType.PrimitiveValue)
			{
				return null;
			}
			base.JsonReader.StartBuffering();
			base.JsonReader.ReadStartObject();
			string result = null;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strA = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal(strA, "__metadata") == 0)
				{
					result = this.ReadTypeNameFromMetadataPropertyValue();
					break;
				}
				base.JsonReader.SkipValue();
			}
			base.JsonReader.StopBuffering();
			return result;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0002F1B8 File Offset: 0x0002D3B8
		internal object ReadNonEntityValue(IEdmTypeReference expectedValueTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, string propertyName)
		{
			return this.ReadNonEntityValueImplementation(expectedValueTypeReference, duplicatePropertyNamesChecker, collectionValidator, validateNullValue, propertyName);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002F1D4 File Offset: 0x0002D3D4
		internal string ReadTypeNameFromMetadataPropertyValue()
		{
			string text = null;
			if (base.JsonReader.NodeType != JsonNodeType.StartObject)
			{
				throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_MetadataPropertyMustHaveObjectValue(base.JsonReader.NodeType));
			}
			base.JsonReader.ReadStartObject();
			ODataVerboseJsonReaderUtils.MetadataPropertyBitMask metadataPropertyBitMask = ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.None;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("type", strB) == 0)
				{
					ODataVerboseJsonReaderUtils.VerifyMetadataPropertyNotFound(ref metadataPropertyBitMask, ODataVerboseJsonReaderUtils.MetadataPropertyBitMask.Type, "type");
					object obj = base.JsonReader.ReadPrimitiveValue();
					text = (obj as string);
					if (text == null)
					{
						throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_InvalidTypeName(obj));
					}
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			base.JsonReader.ReadEndObject();
			return text;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002F288 File Offset: 0x0002D488
		internal object ReadPrimitiveValue(IEdmPrimitiveTypeReference expectedValueTypeReference, bool validateNullValue, string propertyName)
		{
			object obj;
			if (expectedValueTypeReference != null && expectedValueTypeReference.IsSpatial())
			{
				obj = ODataJsonReaderCoreUtils.ReadSpatialValue(base.JsonReader, false, base.VerboseJsonInputContext, expectedValueTypeReference, validateNullValue, this.recursionDepth, propertyName);
			}
			else
			{
				obj = base.JsonReader.ReadPrimitiveValue();
				if (expectedValueTypeReference != null && !base.MessageReaderSettings.DisablePrimitiveTypeConversion)
				{
					obj = ODataVerboseJsonReaderUtils.ConvertValue(obj, expectedValueTypeReference, base.MessageReaderSettings, base.Version, validateNullValue, propertyName);
				}
			}
			return obj;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		private ODataCollectionValue ReadCollectionValueImplementation(IEdmCollectionTypeReference collectionValueTypeReference, string payloadTypeName, SerializationTypeNameAnnotation serializationTypeNameAnnotation)
		{
			ODataVersionChecker.CheckCollectionValue(base.Version);
			this.IncreaseRecursionDepth();
			base.JsonReader.ReadStartObject();
			ODataCollectionValue odataCollectionValue = new ODataCollectionValue();
			odataCollectionValue.TypeName = ((collectionValueTypeReference != null) ? collectionValueTypeReference.ODataFullName() : payloadTypeName);
			if (serializationTypeNameAnnotation != null)
			{
				odataCollectionValue.SetAnnotation<SerializationTypeNameAnnotation>(serializationTypeNameAnnotation);
			}
			List<object> list = null;
			bool flag = false;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("__metadata", strB) == 0)
				{
					if (flag)
					{
						throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper("__metadata"));
					}
					flag = true;
					base.JsonReader.SkipValue();
				}
				else if (string.CompareOrdinal("results", strB) == 0)
				{
					if (list != null)
					{
						throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_MultiplePropertiesInCollectionWrapper("results"));
					}
					list = new List<object>();
					base.JsonReader.ReadStartArray();
					DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
					IEdmTypeReference expectedTypeReference = null;
					if (collectionValueTypeReference != null)
					{
						expectedTypeReference = collectionValueTypeReference.CollectionDefinition().ElementType;
					}
					CollectionWithoutExpectedTypeValidator collectionValidator = null;
					while (base.JsonReader.NodeType != JsonNodeType.EndArray)
					{
						object item = this.ReadNonEntityValueImplementation(expectedTypeReference, duplicatePropertyNamesChecker, collectionValidator, true, null);
						ValidationUtils.ValidateCollectionItem(item, false);
						list.Add(item);
					}
					base.JsonReader.ReadEndArray();
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			base.JsonReader.ReadEndObject();
			if (list == null)
			{
				throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_CollectionWithoutResults);
			}
			odataCollectionValue.Items = new ReadOnlyEnumerable(list);
			this.DecreaseRecursionDepth();
			return odataCollectionValue;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002F45C File Offset: 0x0002D65C
		private ODataComplexValue ReadComplexValueImplementation(IEdmComplexTypeReference complexValueTypeReference, string payloadTypeName, SerializationTypeNameAnnotation serializationTypeNameAnnotation, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			this.IncreaseRecursionDepth();
			base.JsonReader.ReadStartObject();
			ODataComplexValue odataComplexValue = new ODataComplexValue();
			odataComplexValue.TypeName = ((complexValueTypeReference != null) ? complexValueTypeReference.ODataFullName() : payloadTypeName);
			if (serializationTypeNameAnnotation != null)
			{
				odataComplexValue.SetAnnotation<SerializationTypeNameAnnotation>(serializationTypeNameAnnotation);
			}
			if (duplicatePropertyNamesChecker == null)
			{
				duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			}
			else
			{
				duplicatePropertyNamesChecker.Clear();
			}
			List<ODataProperty> list = new List<ODataProperty>();
			bool flag = false;
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string text = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("__metadata", text) == 0)
				{
					if (flag)
					{
						throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_MultipleMetadataPropertiesInComplexValue);
					}
					flag = true;
					base.JsonReader.SkipValue();
				}
				else if (!ValidationUtils.IsValidPropertyName(text))
				{
					base.JsonReader.SkipValue();
				}
				else
				{
					ODataProperty odataProperty = new ODataProperty();
					odataProperty.Name = text;
					IEdmProperty edmProperty = null;
					bool flag2 = false;
					if (complexValueTypeReference != null)
					{
						edmProperty = ReaderValidationUtils.ValidateValuePropertyDefined(text, complexValueTypeReference.ComplexDefinition(), base.MessageReaderSettings, out flag2);
					}
					if (flag2 && (base.JsonReader.NodeType == JsonNodeType.StartObject || base.JsonReader.NodeType == JsonNodeType.StartArray))
					{
						base.JsonReader.SkipValue();
					}
					else
					{
						ODataNullValueBehaviorKind odataNullValueBehaviorKind = (base.ReadingResponse || edmProperty == null) ? ODataNullValueBehaviorKind.Default : base.Model.NullValueReadBehaviorKind(edmProperty);
						object obj = this.ReadNonEntityValueImplementation((edmProperty == null) ? null : edmProperty.Type, null, null, odataNullValueBehaviorKind == ODataNullValueBehaviorKind.Default, text);
						if (odataNullValueBehaviorKind != ODataNullValueBehaviorKind.IgnoreValue || obj != null)
						{
							duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(odataProperty);
							odataProperty.Value = obj;
							list.Add(odataProperty);
						}
					}
				}
			}
			base.JsonReader.ReadEndObject();
			odataComplexValue.Properties = new ReadOnlyEnumerable<ODataProperty>(list);
			this.DecreaseRecursionDepth();
			return odataComplexValue;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0002F5F8 File Offset: 0x0002D7F8
		private object ReadNonEntityValueImplementation(IEdmTypeReference expectedTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, string propertyName)
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType == JsonNodeType.StartArray)
			{
				throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_CannotReadPropertyValue(nodeType));
			}
			object obj;
			if (ODataJsonReaderCoreUtils.TryReadNullValue(base.JsonReader, base.VerboseJsonInputContext, expectedTypeReference, validateNullValue, propertyName))
			{
				obj = null;
			}
			else
			{
				string text = this.FindTypeNameInPayload();
				EdmTypeKind collectionItemTypeKind;
				SerializationTypeNameAnnotation serializationTypeNameAnnotation;
				IEdmTypeReference edmTypeReference = ReaderValidationUtils.ResolvePayloadTypeNameAndComputeTargetType(EdmTypeKind.None, null, expectedTypeReference, text, base.Model, base.MessageReaderSettings, base.Version, new Func<EdmTypeKind>(this.GetNonEntityValueKind), out collectionItemTypeKind, out serializationTypeNameAnnotation);
				switch (collectionItemTypeKind)
				{
				case EdmTypeKind.Primitive:
				{
					IEdmPrimitiveTypeReference edmPrimitiveTypeReference = (edmTypeReference == null) ? null : edmTypeReference.AsPrimitive();
					if (text != null && !edmPrimitiveTypeReference.IsSpatial())
					{
						throw new ODataException(Strings.ODataJsonPropertyAndValueDeserializer_InvalidPrimitiveTypeName(text));
					}
					obj = this.ReadPrimitiveValue(edmPrimitiveTypeReference, validateNullValue, propertyName);
					goto IL_110;
				}
				case EdmTypeKind.Complex:
					obj = this.ReadComplexValueImplementation((edmTypeReference == null) ? null : edmTypeReference.AsComplex(), text, serializationTypeNameAnnotation, duplicatePropertyNamesChecker);
					goto IL_110;
				case EdmTypeKind.Collection:
				{
					IEdmCollectionTypeReference collectionValueTypeReference = ValidationUtils.ValidateCollectionType(edmTypeReference);
					obj = this.ReadCollectionValueImplementation(collectionValueTypeReference, text, serializationTypeNameAnnotation);
					goto IL_110;
				}
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataVerboseJsonPropertyAndValueDeserializer_ReadPropertyValue));
				IL_110:
				if (collectionValidator != null)
				{
					string payloadTypeName = ODataVerboseJsonReaderUtils.GetPayloadTypeName(obj);
					collectionValidator.ValidateCollectionItem(payloadTypeName, collectionItemTypeKind);
				}
			}
			return obj;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002F72C File Offset: 0x0002D92C
		private EdmTypeKind GetNonEntityValueKind()
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType == JsonNodeType.PrimitiveValue)
			{
				return EdmTypeKind.Primitive;
			}
			base.JsonReader.StartBuffering();
			EdmTypeKind result;
			try
			{
				base.JsonReader.ReadNext();
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string b = base.JsonReader.ReadPropertyName();
					if (string.Equals("results", b, StringComparison.Ordinal))
					{
						if (base.JsonReader.NodeType == JsonNodeType.StartArray && base.Version >= ODataVersion.V3)
						{
							return EdmTypeKind.Collection;
						}
						return EdmTypeKind.Complex;
					}
					else
					{
						base.JsonReader.SkipValue();
					}
				}
				result = EdmTypeKind.Complex;
			}
			finally
			{
				base.JsonReader.StopBuffering();
			}
			return result;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002F7D8 File Offset: 0x0002D9D8
		private bool ShouldReadTopLevelPropertyValueWithoutPropertyWrapper(IEdmTypeReference expectedPropertyTypeReference)
		{
			if (base.UseServerFormatBehavior && expectedPropertyTypeReference == null)
			{
				base.JsonReader.StartBuffering();
				try
				{
					base.JsonReader.ReadStartObject();
					if (base.JsonReader.NodeType == JsonNodeType.EndObject)
					{
						return false;
					}
					string strA = base.JsonReader.ReadPropertyName();
					base.JsonReader.SkipValue();
					if (base.JsonReader.NodeType != JsonNodeType.EndObject)
					{
						return true;
					}
					if (string.CompareOrdinal(strA, "__metadata") == 0)
					{
						return true;
					}
				}
				finally
				{
					base.JsonReader.StopBuffering();
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002F874 File Offset: 0x0002DA74
		private void IncreaseRecursionDepth()
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref this.recursionDepth, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002F891 File Offset: 0x0002DA91
		private void DecreaseRecursionDepth()
		{
			this.recursionDepth--;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002F8A1 File Offset: 0x0002DAA1
		[Conditional("DEBUG")]
		private void AssertRecursionDepthIsZero()
		{
		}

		// Token: 0x04000490 RID: 1168
		private int recursionDepth;
	}
}
