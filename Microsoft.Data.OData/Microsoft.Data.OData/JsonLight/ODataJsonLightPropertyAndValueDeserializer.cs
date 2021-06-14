using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000163 RID: 355
	internal class ODataJsonLightPropertyAndValueDeserializer : ODataJsonLightDeserializer
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x0001F101 File Offset: 0x0001D301
		internal ODataJsonLightPropertyAndValueDeserializer(ODataJsonLightInputContext jsonLightInputContext) : base(jsonLightInputContext)
		{
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0001F10C File Offset: 0x0001D30C
		internal ODataProperty ReadTopLevelProperty(IEdmTypeReference expectedPropertyTypeReference)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			base.ReadPayloadStart(ODataPayloadKind.Property, duplicatePropertyNamesChecker, false, false);
			ODataProperty result = this.ReadTopLevelPropertyImplementation(expectedPropertyTypeReference, duplicatePropertyNamesChecker);
			base.ReadPayloadEnd(false);
			return result;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0001F178 File Offset: 0x0001D378
		internal Task<ODataProperty> ReadTopLevelPropertyAsync(IEdmTypeReference expectedPropertyTypeReference)
		{
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			return base.ReadPayloadStartAsync(ODataPayloadKind.Property, duplicatePropertyNamesChecker, false, false).FollowOnSuccessWith(delegate(Task t)
			{
				ODataProperty result = this.ReadTopLevelPropertyImplementation(expectedPropertyTypeReference, duplicatePropertyNamesChecker);
				this.ReadPayloadEnd(false);
				return result;
			});
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0001F1C8 File Offset: 0x0001D3C8
		internal object ReadNonEntityValue(string payloadTypeName, IEdmTypeReference expectedValueTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, bool isTopLevelPropertyValue, bool insideComplexValue, string propertyName)
		{
			return this.ReadNonEntityValue(payloadTypeName, expectedValueTypeReference, duplicatePropertyNamesChecker, collectionValidator, validateNullValue, isTopLevelPropertyValue, insideComplexValue, propertyName, false);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001F1EC File Offset: 0x0001D3EC
		internal object ReadNonEntityValue(string payloadTypeName, IEdmTypeReference expectedValueTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, bool isTopLevelPropertyValue, bool insideComplexValue, string propertyName, bool readRawValueEvenIfNoTypeFound)
		{
			return this.ReadNonEntityValueImplementation(payloadTypeName, expectedValueTypeReference, duplicatePropertyNamesChecker, collectionValidator, validateNullValue, isTopLevelPropertyValue, insideComplexValue, propertyName, readRawValueEvenIfNoTypeFound);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0001F210 File Offset: 0x0001D410
		protected static string ValidateDataPropertyTypeNameAnnotation(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, string propertyName)
		{
			Dictionary<string, object> odataPropertyAnnotations = duplicatePropertyNamesChecker.GetODataPropertyAnnotations(propertyName);
			string result = null;
			if (odataPropertyAnnotations != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in odataPropertyAnnotations)
				{
					if (string.CompareOrdinal(keyValuePair.Key, "odata.type") != 0)
					{
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedDataPropertyAnnotation(propertyName, keyValuePair.Key));
					}
					result = (string)keyValuePair.Value;
				}
			}
			return result;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0001F298 File Offset: 0x0001D498
		protected bool TryReadODataTypeAnnotationValue(string annotationName, out string value)
		{
			if (string.CompareOrdinal(annotationName, "odata.type") == 0)
			{
				value = this.ReadODataTypeAnnotationValue();
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
		protected string ReadODataTypeAnnotationValue()
		{
			string text = base.JsonReader.ReadStringValue();
			if (text == null)
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_InvalidTypeName(text));
			}
			return text;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0001F2E4 File Offset: 0x0001D4E4
		protected object ReadTypePropertyAnnotationValue(string propertyAnnotationName)
		{
			string result;
			if (this.TryReadODataTypeAnnotationValue(propertyAnnotationName, out result))
			{
				return result;
			}
			throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(propertyAnnotationName));
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0001F30C File Offset: 0x0001D50C
		private bool TryReadODataTypeAnnotation(out string payloadTypeName)
		{
			payloadTypeName = null;
			bool result = false;
			string propertyName = base.JsonReader.GetPropertyName();
			if (string.CompareOrdinal(propertyName, "odata.type") == 0)
			{
				base.JsonReader.ReadNext();
				payloadTypeName = this.ReadODataTypeAnnotationValue();
				result = true;
			}
			return result;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0001F4A8 File Offset: 0x0001D6A8
		private ODataProperty ReadTopLevelPropertyImplementation(IEdmTypeReference expectedPropertyTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			expectedPropertyTypeReference = this.UpdateExpectedTypeBasedOnMetadataUri(expectedPropertyTypeReference);
			object propertyValue = ODataJsonLightPropertyAndValueDeserializer.missingPropertyValue;
			if (this.IsTopLevelNullValue())
			{
				ReaderValidationUtils.ValidateNullValue(base.Model, expectedPropertyTypeReference, base.MessageReaderSettings, true, base.Version, null);
				this.ValidateNoPropertyInNullPayload(duplicatePropertyNamesChecker);
				propertyValue = null;
			}
			else
			{
				string payloadTypeName = null;
				if (this.ReadingComplexProperty(duplicatePropertyNamesChecker, expectedPropertyTypeReference, out payloadTypeName))
				{
					propertyValue = this.ReadNonEntityValue(payloadTypeName, expectedPropertyTypeReference, duplicatePropertyNamesChecker, null, true, true, true, null);
				}
				else
				{
					bool isReordering = base.JsonReader is ReorderingJsonReader;
					Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
					{
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation(annotationName));
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
								if (string.CompareOrdinal("value", propertyName) == 0)
								{
									propertyValue = this.ReadNonEntityValue(payloadTypeName, expectedPropertyTypeReference, null, null, true, true, false, propertyName);
									return;
								}
								throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyName(propertyName, "value"));
							case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
								throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty(propertyName));
							case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
								if (string.CompareOrdinal("odata.type", propertyName) != 0)
								{
									throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(propertyName));
								}
								if (isReordering)
								{
									this.JsonReader.SkipValue();
									return;
								}
								if (!object.ReferenceEquals(ODataJsonLightPropertyAndValueDeserializer.missingPropertyValue, propertyValue))
								{
									throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_TypePropertyAfterValueProperty("odata.type", "value"));
								}
								payloadTypeName = this.ReadODataTypeAnnotationValue();
								return;
							case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
								this.JsonReader.SkipValue();
								return;
							case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
								throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
							default:
								return;
							}
						});
					}
					if (object.ReferenceEquals(ODataJsonLightPropertyAndValueDeserializer.missingPropertyValue, propertyValue))
					{
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_InvalidTopLevelPropertyPayload);
					}
				}
			}
			ODataProperty result = new ODataProperty
			{
				Name = null,
				Value = propertyValue
			};
			base.JsonReader.Read();
			return result;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0001F630 File Offset: 0x0001D830
		private IEdmTypeReference UpdateExpectedTypeBasedOnMetadataUri(IEdmTypeReference expectedPropertyTypeReference)
		{
			if (base.MetadataUriParseResult == null || base.MetadataUriParseResult.EdmType == null)
			{
				return expectedPropertyTypeReference;
			}
			IEdmType edmType = base.MetadataUriParseResult.EdmType;
			if (expectedPropertyTypeReference != null && !expectedPropertyTypeReference.Definition.IsAssignableFrom(edmType))
			{
				throw new ODataException(Strings.ReaderValidationUtils_TypeInMetadataUriDoesNotMatchExpectedType(UriUtilsCommon.UriToString(base.MetadataUriParseResult.MetadataUri), edmType.ODataFullName(), expectedPropertyTypeReference.ODataFullName()));
			}
			bool nullable = true;
			if (expectedPropertyTypeReference != null)
			{
				nullable = expectedPropertyTypeReference.IsNullable;
			}
			return edmType.ToTypeReference(nullable);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001F6AC File Offset: 0x0001D8AC
		private ODataCollectionValue ReadCollectionValue(IEdmCollectionTypeReference collectionValueTypeReference, string payloadTypeName, SerializationTypeNameAnnotation serializationTypeNameAnnotation)
		{
			ODataVersionChecker.CheckCollectionValue(base.Version);
			this.IncreaseRecursionDepth();
			base.JsonReader.ReadStartArray();
			ODataCollectionValue odataCollectionValue = new ODataCollectionValue();
			odataCollectionValue.TypeName = ((collectionValueTypeReference != null) ? collectionValueTypeReference.ODataFullName() : payloadTypeName);
			if (serializationTypeNameAnnotation != null)
			{
				odataCollectionValue.SetAnnotation<SerializationTypeNameAnnotation>(serializationTypeNameAnnotation);
			}
			if (collectionValueTypeReference != null)
			{
				odataCollectionValue.SetAnnotation<ODataTypeAnnotation>(new ODataTypeAnnotation(collectionValueTypeReference));
			}
			List<object> list = new List<object>();
			DuplicatePropertyNamesChecker duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
			IEdmTypeReference expectedTypeReference = null;
			if (collectionValueTypeReference != null)
			{
				expectedTypeReference = collectionValueTypeReference.CollectionDefinition().ElementType;
			}
			CollectionWithoutExpectedTypeValidator collectionValidator = null;
			while (base.JsonReader.NodeType != JsonNodeType.EndArray)
			{
				object item = this.ReadNonEntityValueImplementation(null, expectedTypeReference, duplicatePropertyNamesChecker, collectionValidator, true, false, false, null);
				ValidationUtils.ValidateCollectionItem(item, false);
				list.Add(item);
			}
			base.JsonReader.ReadEndArray();
			odataCollectionValue.Items = new ReadOnlyEnumerable(list);
			this.DecreaseRecursionDepth();
			return odataCollectionValue;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001F778 File Offset: 0x0001D978
		private object ReadPrimitiveValue(bool insideJsonObjectValue, IEdmPrimitiveTypeReference expectedValueTypeReference, bool validateNullValue, string propertyName)
		{
			object obj;
			if (expectedValueTypeReference != null && expectedValueTypeReference.IsSpatial())
			{
				obj = ODataJsonReaderCoreUtils.ReadSpatialValue(base.JsonReader, insideJsonObjectValue, base.JsonLightInputContext, expectedValueTypeReference, validateNullValue, this.recursionDepth, propertyName);
			}
			else
			{
				if (insideJsonObjectValue)
				{
					throw new ODataException(Strings.JsonReaderExtensions_UnexpectedNodeDetected(JsonNodeType.PrimitiveValue, JsonNodeType.StartObject));
				}
				obj = base.JsonReader.ReadPrimitiveValue();
				if (expectedValueTypeReference != null)
				{
					obj = ODataJsonLightReaderUtils.ConvertValue(obj, expectedValueTypeReference, base.MessageReaderSettings, base.Version, validateNullValue, propertyName);
				}
			}
			return obj;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001F964 File Offset: 0x0001DB64
		private ODataComplexValue ReadComplexValue(IEdmComplexTypeReference complexValueTypeReference, string payloadTypeName, SerializationTypeNameAnnotation serializationTypeNameAnnotation, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			this.IncreaseRecursionDepth();
			ODataComplexValue odataComplexValue = new ODataComplexValue();
			odataComplexValue.TypeName = ((complexValueTypeReference != null) ? complexValueTypeReference.ODataFullName() : payloadTypeName);
			if (serializationTypeNameAnnotation != null)
			{
				odataComplexValue.SetAnnotation<SerializationTypeNameAnnotation>(serializationTypeNameAnnotation);
			}
			if (complexValueTypeReference != null)
			{
				odataComplexValue.SetAnnotation<ODataTypeAnnotation>(new ODataTypeAnnotation(complexValueTypeReference));
			}
			List<ODataProperty> properties = new List<ODataProperty>();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				base.ProcessProperty(duplicatePropertyNamesChecker, new Func<string, object>(this.ReadTypePropertyAnnotationValue), delegate(ODataJsonLightDeserializer.PropertyParsingResult propertyParsingResult, string propertyName)
				{
					switch (propertyParsingResult)
					{
					case ODataJsonLightDeserializer.PropertyParsingResult.EndOfObject:
						break;
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithValue:
					{
						ODataProperty odataProperty = new ODataProperty();
						odataProperty.Name = propertyName;
						IEdmProperty edmProperty = null;
						bool flag = false;
						if (complexValueTypeReference != null)
						{
							edmProperty = ReaderValidationUtils.ValidateValuePropertyDefined(propertyName, complexValueTypeReference.ComplexDefinition(), this.MessageReaderSettings, out flag);
						}
						if (flag && (this.JsonReader.NodeType == JsonNodeType.StartObject || this.JsonReader.NodeType == JsonNodeType.StartArray))
						{
							this.JsonReader.SkipValue();
							return;
						}
						ODataNullValueBehaviorKind odataNullValueBehaviorKind = (this.ReadingResponse || edmProperty == null) ? ODataNullValueBehaviorKind.Default : this.Model.NullValueReadBehaviorKind(edmProperty);
						object obj = this.ReadNonEntityValueImplementation(ODataJsonLightPropertyAndValueDeserializer.ValidateDataPropertyTypeNameAnnotation(duplicatePropertyNamesChecker, propertyName), (edmProperty == null) ? null : edmProperty.Type, null, null, odataNullValueBehaviorKind == ODataNullValueBehaviorKind.Default, false, false, propertyName);
						if (odataNullValueBehaviorKind != ODataNullValueBehaviorKind.IgnoreValue || obj != null)
						{
							duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(odataProperty);
							odataProperty.Value = obj;
							properties.Add(odataProperty);
							return;
						}
						break;
					}
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_ComplexValuePropertyAnnotationWithoutProperty(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						if (string.CompareOrdinal("odata.type", propertyName) == 0)
						{
							throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_ComplexTypeAnnotationNotFirst);
						}
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						this.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						return;
					}
				});
			}
			base.JsonReader.ReadEndObject();
			odataComplexValue.Properties = new ReadOnlyEnumerable<ODataProperty>(properties);
			this.DecreaseRecursionDepth();
			return odataComplexValue;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001FA44 File Offset: 0x0001DC44
		private object ReadNonEntityValueImplementation(string payloadTypeName, IEdmTypeReference expectedTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, bool isTopLevelPropertyValue, bool insideComplexValue, string propertyName)
		{
			return this.ReadNonEntityValueImplementation(payloadTypeName, expectedTypeReference, duplicatePropertyNamesChecker, collectionValidator, validateNullValue, isTopLevelPropertyValue, insideComplexValue, propertyName, false);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001FA68 File Offset: 0x0001DC68
		private object ReadNonEntityValueImplementation(string payloadTypeName, IEdmTypeReference expectedTypeReference, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, CollectionWithoutExpectedTypeValidator collectionValidator, bool validateNullValue, bool isTopLevelPropertyValue, bool insideComplexValue, string propertyName, bool readRawValueEvenIfNoTypeFound)
		{
			bool flag = base.JsonReader.NodeType == JsonNodeType.StartObject;
			bool flag2 = !insideComplexValue && payloadTypeName != null;
			bool flag3 = false;
			if (flag || insideComplexValue)
			{
				if (duplicatePropertyNamesChecker == null)
				{
					duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
				}
				else
				{
					duplicatePropertyNamesChecker.Clear();
				}
				if (!insideComplexValue)
				{
					string text;
					flag3 = this.TryReadPayloadTypeFromObject(duplicatePropertyNamesChecker, insideComplexValue, out text);
					if (flag3)
					{
						payloadTypeName = text;
					}
				}
			}
			if (string.IsNullOrEmpty(payloadTypeName) && expectedTypeReference == null && readRawValueEvenIfNoTypeFound)
			{
				ODataJsonLightGeneralDeserializer odataJsonLightGeneralDeserializer = new ODataJsonLightGeneralDeserializer(base.JsonLightInputContext);
				return odataJsonLightGeneralDeserializer.ReadValue();
			}
			EdmTypeKind collectionItemTypeKind;
			SerializationTypeNameAnnotation serializationTypeNameAnnotation;
			IEdmTypeReference edmTypeReference = ReaderValidationUtils.ResolvePayloadTypeNameAndComputeTargetType(EdmTypeKind.None, null, expectedTypeReference, payloadTypeName, base.Model, base.MessageReaderSettings, base.Version, new Func<EdmTypeKind>(this.GetNonEntityValueKind), out collectionItemTypeKind, out serializationTypeNameAnnotation);
			object obj;
			if (ODataJsonReaderCoreUtils.TryReadNullValue(base.JsonReader, base.JsonLightInputContext, edmTypeReference, validateNullValue, propertyName))
			{
				if (isTopLevelPropertyValue)
				{
					throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyWithPrimitiveNullValue("odata.null", "true"));
				}
				obj = null;
			}
			else
			{
				switch (collectionItemTypeKind)
				{
				case EdmTypeKind.Primitive:
				{
					IEdmPrimitiveTypeReference expectedValueTypeReference = (edmTypeReference == null) ? null : edmTypeReference.AsPrimitive();
					if (flag3)
					{
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_ODataTypeAnnotationInPrimitiveValue("odata.type"));
					}
					obj = this.ReadPrimitiveValue(flag, expectedValueTypeReference, validateNullValue, propertyName);
					goto IL_1BD;
				}
				case EdmTypeKind.Complex:
					if (flag2)
					{
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_ComplexValueWithPropertyTypeAnnotation("odata.type"));
					}
					if (!flag && !insideComplexValue)
					{
						base.JsonReader.ReadStartObject();
					}
					obj = this.ReadComplexValue((edmTypeReference == null) ? null : edmTypeReference.AsComplex(), payloadTypeName, serializationTypeNameAnnotation, duplicatePropertyNamesChecker);
					goto IL_1BD;
				case EdmTypeKind.Collection:
				{
					IEdmCollectionTypeReference collectionValueTypeReference = ValidationUtils.ValidateCollectionType(edmTypeReference);
					if (flag)
					{
						throw new ODataException(Strings.JsonReaderExtensions_UnexpectedNodeDetected(JsonNodeType.StartArray, JsonNodeType.StartObject));
					}
					obj = this.ReadCollectionValue(collectionValueTypeReference, payloadTypeName, serializationTypeNameAnnotation);
					goto IL_1BD;
				}
				}
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataJsonLightPropertyAndValueDeserializer_ReadPropertyValue));
				IL_1BD:
				if (collectionValidator != null)
				{
					string payloadTypeName2 = ODataJsonLightReaderUtils.GetPayloadTypeName(obj);
					collectionValidator.ValidateCollectionItem(payloadTypeName2, collectionItemTypeKind);
				}
			}
			return obj;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0001FC4C File Offset: 0x0001DE4C
		private bool TryReadPayloadTypeFromObject(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, bool insideComplexValue, out string payloadTypeName)
		{
			bool flag = false;
			payloadTypeName = null;
			if (!insideComplexValue)
			{
				base.JsonReader.ReadStartObject();
			}
			if (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				flag = this.TryReadODataTypeAnnotation(out payloadTypeName);
				if (flag)
				{
					duplicatePropertyNamesChecker.MarkPropertyAsProcessed("odata.type");
				}
			}
			return flag;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0001FC94 File Offset: 0x0001DE94
		private bool ReadingComplexProperty(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, IEdmTypeReference expectedPropertyTypeReference, out string payloadTypeName)
		{
			payloadTypeName = null;
			bool result = false;
			if (expectedPropertyTypeReference != null)
			{
				result = expectedPropertyTypeReference.IsComplex();
			}
			if (base.JsonReader.NodeType == JsonNodeType.Property && this.TryReadODataTypeAnnotation(out payloadTypeName))
			{
				duplicatePropertyNamesChecker.MarkPropertyAsProcessed("odata.type");
				IEdmType expectedType = null;
				if (expectedPropertyTypeReference != null)
				{
					expectedType = expectedPropertyTypeReference.Definition;
				}
				EdmTypeKind edmTypeKind = EdmTypeKind.None;
				IEdmType edmType = MetadataUtils.ResolveTypeNameForRead(base.Model, expectedType, payloadTypeName, base.MessageReaderSettings.ReaderBehavior, base.MessageReaderSettings.MaxProtocolVersion, out edmTypeKind);
				if (edmType != null)
				{
					result = edmType.IsODataComplexTypeKind();
				}
			}
			return result;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0001FD14 File Offset: 0x0001DF14
		private bool IsTopLevelNullValue()
		{
			bool flag = base.MetadataUriParseResult != null && base.MetadataUriParseResult.IsNullProperty;
			bool flag2 = base.JsonReader.NodeType == JsonNodeType.Property && string.CompareOrdinal("odata.null", base.JsonReader.GetPropertyName()) == 0;
			if (flag2)
			{
				base.JsonReader.ReadNext();
				object obj = base.JsonReader.ReadPrimitiveValue();
				if (!(obj is bool) || !(bool)obj)
				{
					throw new ODataException(Strings.ODataJsonLightReaderUtils_InvalidValueForODataNullAnnotation("odata.null", "true"));
				}
			}
			return flag || flag2;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001FE24 File Offset: 0x0001E024
		private void ValidateNoPropertyInNullPayload(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker)
		{
			Func<string, object> readPropertyAnnotationValue = delegate(string annotationName)
			{
				throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedODataPropertyAnnotation(annotationName));
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
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_NoPropertyAndAnnotationAllowedInNullPayload(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.PropertyWithoutValue:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_TopLevelPropertyAnnotationWithoutProperty(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.ODataInstanceAnnotation:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedAnnotationProperties(propertyName));
					case ODataJsonLightDeserializer.PropertyParsingResult.CustomInstanceAnnotation:
						base.JsonReader.SkipValue();
						return;
					case ODataJsonLightDeserializer.PropertyParsingResult.MetadataReferenceProperty:
						throw new ODataException(Strings.ODataJsonLightPropertyAndValueDeserializer_UnexpectedMetadataReferenceProperty(propertyName));
					default:
						return;
					}
				});
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001FE7C File Offset: 0x0001E07C
		private EdmTypeKind GetNonEntityValueKind()
		{
			JsonNodeType nodeType = base.JsonReader.NodeType;
			if (nodeType == JsonNodeType.StartArray)
			{
				return EdmTypeKind.Collection;
			}
			if (nodeType == JsonNodeType.PrimitiveValue)
			{
				return EdmTypeKind.Primitive;
			}
			return EdmTypeKind.Complex;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001FEA2 File Offset: 0x0001E0A2
		private void IncreaseRecursionDepth()
		{
			ValidationUtils.IncreaseAndValidateRecursionDepth(ref this.recursionDepth, base.MessageReaderSettings.MessageQuotas.MaxNestingDepth);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001FEBF File Offset: 0x0001E0BF
		private void DecreaseRecursionDepth()
		{
			this.recursionDepth--;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001FECF File Offset: 0x0001E0CF
		[Conditional("DEBUG")]
		private void AssertRecursionDepthIsZero()
		{
		}

		// Token: 0x0400039C RID: 924
		private static readonly object missingPropertyValue = new object();

		// Token: 0x0400039D RID: 925
		private int recursionDepth;
	}
}
