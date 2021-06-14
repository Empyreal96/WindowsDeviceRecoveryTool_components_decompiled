using System;
using System.Text;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.JsonLight;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000241 RID: 577
	internal static class ReaderValidationUtils
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0004507C File Offset: 0x0004327C
		internal static void ValidateMessageReaderSettings(ODataMessageReaderSettings messageReaderSettings, bool readingResponse)
		{
			if (messageReaderSettings.BaseUri != null && !messageReaderSettings.BaseUri.IsAbsoluteUri)
			{
				throw new ODataException(Strings.ReaderValidationUtils_MessageReaderSettingsBaseUriMustBeNullOrAbsolute(UriUtilsCommon.UriToString(messageReaderSettings.BaseUri)));
			}
			if (!readingResponse && messageReaderSettings.UndeclaredPropertyBehaviorKinds != ODataUndeclaredPropertyBehaviorKinds.None)
			{
				throw new ODataException(Strings.ReaderValidationUtils_UndeclaredPropertyBehaviorKindSpecifiedOnRequest);
			}
			if (!string.IsNullOrEmpty(messageReaderSettings.ReaderBehavior.ODataTypeScheme) && !string.Equals(messageReaderSettings.ReaderBehavior.ODataTypeScheme, "http://schemas.microsoft.com/ado/2007/08/dataservices/scheme"))
			{
				ODataVersionChecker.CheckCustomTypeScheme(messageReaderSettings.MaxProtocolVersion);
			}
			if (!string.IsNullOrEmpty(messageReaderSettings.ReaderBehavior.ODataNamespace) && !string.Equals(messageReaderSettings.ReaderBehavior.ODataNamespace, "http://schemas.microsoft.com/ado/2007/08/dataservices"))
			{
				ODataVersionChecker.CheckCustomDataNamespace(messageReaderSettings.MaxProtocolVersion);
			}
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00045138 File Offset: 0x00043338
		internal static void ValidateEntityReferenceLink(ODataEntityReferenceLink link)
		{
			if (link.Url == null)
			{
				throw new ODataException(Strings.ReaderValidationUtils_EntityReferenceLinkMissingUri);
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00045153 File Offset: 0x00043353
		internal static void ValidateStreamReferenceProperty(ODataProperty streamProperty, IEdmStructuredType structuredType, IEdmProperty streamEdmProperty, ODataMessageReaderSettings messageReaderSettings)
		{
			ValidationUtils.ValidateStreamReferenceProperty(streamProperty, streamEdmProperty);
			if (structuredType != null && structuredType.IsOpen && streamEdmProperty == null && !messageReaderSettings.ReportUndeclaredLinkProperties)
			{
				ValidationUtils.ValidateOpenPropertyValue(streamProperty.Name, streamProperty.Value);
			}
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00045183 File Offset: 0x00043383
		internal static void ValidateNullValue(IEdmModel model, IEdmTypeReference expectedTypeReference, ODataMessageReaderSettings messageReaderSettings, bool validateNullValue, ODataVersion version, string propertyName)
		{
			if (expectedTypeReference != null)
			{
				ReaderValidationUtils.ValidateTypeSupported(expectedTypeReference, version);
				if (!messageReaderSettings.DisablePrimitiveTypeConversion || expectedTypeReference.TypeKind() != EdmTypeKind.Primitive)
				{
					ReaderValidationUtils.ValidateNullValueAllowed(expectedTypeReference, validateNullValue, model, propertyName);
				}
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000451AB File Offset: 0x000433AB
		internal static void ValidateEntry(ODataEntry entry)
		{
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x000451B0 File Offset: 0x000433B0
		internal static IEdmProperty FindDefinedProperty(string propertyName, IEdmStructuredType owningStructuredType)
		{
			if (owningStructuredType == null)
			{
				return null;
			}
			return owningStructuredType.FindProperty(propertyName);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000451CC File Offset: 0x000433CC
		internal static IEdmProperty ValidateValuePropertyDefined(string propertyName, IEdmStructuredType owningStructuredType, ODataMessageReaderSettings messageReaderSettings, out bool ignoreProperty)
		{
			ignoreProperty = false;
			if (owningStructuredType == null)
			{
				return null;
			}
			IEdmProperty edmProperty = ReaderValidationUtils.FindDefinedProperty(propertyName, owningStructuredType);
			if (edmProperty == null && !owningStructuredType.IsOpen)
			{
				if (!messageReaderSettings.IgnoreUndeclaredValueProperties)
				{
					throw new ODataException(Strings.ValidationUtils_PropertyDoesNotExistOnType(propertyName, owningStructuredType.ODataFullName()));
				}
				ignoreProperty = true;
			}
			return edmProperty;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00045214 File Offset: 0x00043414
		internal static void ValidateExpectedPropertyName(string expectedPropertyName, string payloadPropertyName)
		{
			if (expectedPropertyName != null && string.CompareOrdinal(expectedPropertyName, payloadPropertyName) != 0)
			{
				throw new ODataException(Strings.ReaderValidationUtils_NonMatchingPropertyNames(payloadPropertyName, expectedPropertyName));
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x00045230 File Offset: 0x00043430
		internal static IEdmProperty ValidateLinkPropertyDefined(string propertyName, IEdmStructuredType owningStructuredType, ODataMessageReaderSettings messageReaderSettings)
		{
			if (owningStructuredType == null)
			{
				return null;
			}
			IEdmProperty edmProperty = ReaderValidationUtils.FindDefinedProperty(propertyName, owningStructuredType);
			if (edmProperty == null && !owningStructuredType.IsOpen && !messageReaderSettings.ReportUndeclaredLinkProperties)
			{
				throw new ODataException(Strings.ValidationUtils_PropertyDoesNotExistOnType(propertyName, owningStructuredType.ODataFullName()));
			}
			return edmProperty;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00045270 File Offset: 0x00043470
		internal static IEdmNavigationProperty ValidateNavigationPropertyDefined(string propertyName, IEdmEntityType owningEntityType, ODataMessageReaderSettings messageReaderSettings)
		{
			if (owningEntityType == null)
			{
				return null;
			}
			IEdmProperty edmProperty = ReaderValidationUtils.ValidateLinkPropertyDefined(propertyName, owningEntityType, messageReaderSettings);
			if (edmProperty == null)
			{
				if (owningEntityType.IsOpen && !messageReaderSettings.ReportUndeclaredLinkProperties)
				{
					throw new ODataException(Strings.ValidationUtils_OpenNavigationProperty(propertyName, owningEntityType.ODataFullName()));
				}
			}
			else if (edmProperty.PropertyKind != EdmPropertyKind.Navigation)
			{
				throw new ODataException(Strings.ValidationUtils_NavigationPropertyExpected(propertyName, owningEntityType.ODataFullName(), edmProperty.PropertyKind.ToString()));
			}
			return (IEdmNavigationProperty)edmProperty;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x000452E1 File Offset: 0x000434E1
		internal static ODataException GetPrimitiveTypeConversionException(IEdmPrimitiveTypeReference targetTypeReference, Exception innerException)
		{
			return new ODataException(Strings.ReaderValidationUtils_CannotConvertPrimitiveValue(targetTypeReference.ODataFullName()), innerException);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000452F4 File Offset: 0x000434F4
		internal static IEdmType ResolvePayloadTypeName(IEdmModel model, IEdmTypeReference expectedTypeReference, string payloadTypeName, EdmTypeKind expectedTypeKind, ODataReaderBehavior readerBehavior, ODataVersion version, out EdmTypeKind payloadTypeKind)
		{
			if (payloadTypeName == null)
			{
				payloadTypeKind = EdmTypeKind.None;
				return null;
			}
			if (payloadTypeName.Length == 0)
			{
				payloadTypeKind = expectedTypeKind;
				return null;
			}
			IEdmType result = MetadataUtils.ResolveTypeNameForRead(model, (expectedTypeReference == null) ? null : expectedTypeReference.Definition, payloadTypeName, readerBehavior, version, out payloadTypeKind);
			if (payloadTypeKind == EdmTypeKind.None)
			{
				payloadTypeKind = expectedTypeKind;
			}
			return result;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004533C File Offset: 0x0004353C
		internal static IEdmTypeReference ResolvePayloadTypeNameAndComputeTargetType(EdmTypeKind expectedTypeKind, IEdmType defaultPrimitivePayloadType, IEdmTypeReference expectedTypeReference, string payloadTypeName, IEdmModel model, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, Func<EdmTypeKind> typeKindPeekedFromPayloadFunc, out EdmTypeKind targetTypeKind, out SerializationTypeNameAnnotation serializationTypeNameAnnotation)
		{
			serializationTypeNameAnnotation = null;
			EdmTypeKind payloadTypeKind;
			IEdmType payloadType = ReaderValidationUtils.ResolvePayloadTypeName(model, expectedTypeReference, payloadTypeName, EdmTypeKind.Complex, messageReaderSettings.ReaderBehavior, version, out payloadTypeKind);
			targetTypeKind = ReaderValidationUtils.ComputeTargetTypeKind(expectedTypeReference, expectedTypeKind == EdmTypeKind.Entity, payloadTypeName, payloadTypeKind, messageReaderSettings, typeKindPeekedFromPayloadFunc);
			IEdmTypeReference edmTypeReference;
			if (targetTypeKind == EdmTypeKind.Primitive)
			{
				edmTypeReference = ReaderValidationUtils.ResolveAndValidatePrimitiveTargetType(expectedTypeReference, payloadTypeKind, payloadType, payloadTypeName, defaultPrimitivePayloadType, model, messageReaderSettings, version);
			}
			else
			{
				edmTypeReference = ReaderValidationUtils.ResolveAndValidateNonPrimitiveTargetType(targetTypeKind, expectedTypeReference, payloadTypeKind, payloadType, payloadTypeName, model, messageReaderSettings, version);
				if (edmTypeReference != null)
				{
					serializationTypeNameAnnotation = ReaderValidationUtils.CreateSerializationTypeNameAnnotation(payloadTypeName, edmTypeReference);
				}
			}
			if (expectedTypeKind != EdmTypeKind.None && edmTypeReference != null)
			{
				ValidationUtils.ValidateTypeKind(targetTypeKind, expectedTypeKind, payloadTypeName);
			}
			return edmTypeReference;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000453C0 File Offset: 0x000435C0
		internal static IEdmTypeReference ResolveAndValidatePrimitiveTargetType(IEdmTypeReference expectedTypeReference, EdmTypeKind payloadTypeKind, IEdmType payloadType, string payloadTypeName, IEdmType defaultPayloadType, IEdmModel model, ODataMessageReaderSettings messageReaderSettings, ODataVersion version)
		{
			bool flag = messageReaderSettings.ReaderBehavior.TypeResolver != null && payloadType != null;
			if (expectedTypeReference != null && !flag)
			{
				ReaderValidationUtils.ValidateTypeSupported(expectedTypeReference, version);
			}
			if (payloadTypeKind != EdmTypeKind.None && (messageReaderSettings.DisablePrimitiveTypeConversion || !messageReaderSettings.DisableStrictMetadataValidation))
			{
				ValidationUtils.ValidateTypeKind(payloadTypeKind, EdmTypeKind.Primitive, payloadTypeName);
			}
			if (!model.IsUserModel())
			{
				return MetadataUtils.GetNullablePayloadTypeReference(payloadType ?? defaultPayloadType);
			}
			if (expectedTypeReference == null || flag || messageReaderSettings.DisablePrimitiveTypeConversion)
			{
				return MetadataUtils.GetNullablePayloadTypeReference(payloadType ?? defaultPayloadType);
			}
			if (messageReaderSettings.DisableStrictMetadataValidation)
			{
				return expectedTypeReference;
			}
			if (payloadType != null && !MetadataUtilsCommon.CanConvertPrimitiveTypeTo((IEdmPrimitiveType)payloadType, (IEdmPrimitiveType)expectedTypeReference.Definition))
			{
				throw new ODataException(Strings.ValidationUtils_IncompatibleType(payloadTypeName, expectedTypeReference.ODataFullName()));
			}
			return expectedTypeReference;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x0004547C File Offset: 0x0004367C
		internal static IEdmTypeReference ResolveAndValidateNonPrimitiveTargetType(EdmTypeKind expectedTypeKind, IEdmTypeReference expectedTypeReference, EdmTypeKind payloadTypeKind, IEdmType payloadType, string payloadTypeName, IEdmModel model, ODataMessageReaderSettings messageReaderSettings, ODataVersion version)
		{
			bool flag = messageReaderSettings.ReaderBehavior.TypeResolver != null && payloadType != null;
			if (!flag)
			{
				ReaderValidationUtils.ValidateTypeSupported(expectedTypeReference, version);
				if (model.IsUserModel() && (expectedTypeReference == null || !messageReaderSettings.DisableStrictMetadataValidation))
				{
					ReaderValidationUtils.VerifyPayloadTypeDefined(payloadTypeName, payloadType);
				}
			}
			else
			{
				ReaderValidationUtils.ValidateTypeSupported((payloadType == null) ? null : payloadType.ToTypeReference(true), version);
			}
			if (payloadTypeKind != EdmTypeKind.None && (!messageReaderSettings.DisableStrictMetadataValidation || expectedTypeReference == null))
			{
				ValidationUtils.ValidateTypeKind(payloadTypeKind, expectedTypeKind, payloadTypeName);
			}
			if (!model.IsUserModel())
			{
				return null;
			}
			if (expectedTypeReference == null || flag)
			{
				return ReaderValidationUtils.ResolveAndValidateTargetTypeWithNoExpectedType(expectedTypeKind, payloadType);
			}
			if (messageReaderSettings.DisableStrictMetadataValidation)
			{
				return ReaderValidationUtils.ResolveAndValidateTargetTypeStrictValidationDisabled(expectedTypeKind, expectedTypeReference, payloadType);
			}
			return ReaderValidationUtils.ResolveAndValidateTargetTypeStrictValidationEnabled(expectedTypeKind, expectedTypeReference, payloadType);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0004552B File Offset: 0x0004372B
		internal static void ValidateEncodingSupportedInBatch(Encoding encoding)
		{
			if (!encoding.IsSingleByte && Encoding.UTF8.CodePage != encoding.CodePage)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_MultiByteEncodingsNotSupported(encoding.WebName));
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00045558 File Offset: 0x00043758
		internal static void ValidateTypeSupported(IEdmTypeReference typeReference, ODataVersion version)
		{
			if (typeReference != null)
			{
				if (typeReference.IsNonEntityCollectionType())
				{
					ODataVersionChecker.CheckCollectionValue(version);
					return;
				}
				if (typeReference.IsSpatial())
				{
					ODataVersionChecker.CheckSpatialValue(version);
				}
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0004557C File Offset: 0x0004377C
		internal static void ValidateFeedOrEntryMetadataUri(ODataJsonLightMetadataUriParseResult metadataUriParseResult, ODataReaderCore.Scope scope)
		{
			if (scope.EntitySet == null)
			{
				scope.EntitySet = metadataUriParseResult.EntitySet;
			}
			else if (string.CompareOrdinal(scope.EntitySet.FullName(), metadataUriParseResult.EntitySet.FullName()) != 0)
			{
				throw new ODataException(Strings.ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntitySet(UriUtilsCommon.UriToString(metadataUriParseResult.MetadataUri), metadataUriParseResult.EntitySet.FullName(), scope.EntitySet.FullName()));
			}
			IEdmEntityType edmEntityType = (IEdmEntityType)metadataUriParseResult.EdmType;
			if (scope.EntityType == null)
			{
				scope.EntityType = edmEntityType;
				return;
			}
			if (scope.EntityType.IsAssignableFrom(edmEntityType))
			{
				scope.EntityType = edmEntityType;
				return;
			}
			if (!edmEntityType.IsAssignableFrom(scope.EntityType))
			{
				throw new ODataException(Strings.ReaderValidationUtils_MetadataUriValidationInvalidExpectedEntityType(UriUtilsCommon.UriToString(metadataUriParseResult.MetadataUri), metadataUriParseResult.EdmType.ODataFullName(), scope.EntityType.FullName()));
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00045654 File Offset: 0x00043854
		internal static void ValidateEntityReferenceLinkMetadataUri(ODataJsonLightMetadataUriParseResult metadataUriParseResult, IEdmNavigationProperty navigationProperty)
		{
			if (navigationProperty == null)
			{
				return;
			}
			IEdmNavigationProperty navigationProperty2 = metadataUriParseResult.NavigationProperty;
			if (string.CompareOrdinal(navigationProperty.Name, navigationProperty2.Name) != 0)
			{
				throw new ODataException(Strings.ReaderValidationUtils_MetadataUriValidationNonMatchingPropertyNames(UriUtilsCommon.UriToString(metadataUriParseResult.MetadataUri), navigationProperty2.Name, navigationProperty2.DeclaringEntityType().FullName(), navigationProperty.Name));
			}
			if (!navigationProperty.DeclaringType.IsEquivalentTo(navigationProperty2.DeclaringType))
			{
				throw new ODataException(Strings.ReaderValidationUtils_MetadataUriValidationNonMatchingDeclaringTypes(UriUtilsCommon.UriToString(metadataUriParseResult.MetadataUri), navigationProperty2.Name, navigationProperty2.DeclaringEntityType().FullName(), navigationProperty.DeclaringEntityType().FullName()));
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000456F4 File Offset: 0x000438F4
		internal static IEdmTypeReference ValidateCollectionMetadataUriAndGetPayloadItemTypeReference(ODataJsonLightMetadataUriParseResult metadataUriParseResult, IEdmTypeReference expectedItemTypeReference)
		{
			if (metadataUriParseResult == null)
			{
				return expectedItemTypeReference;
			}
			IEdmCollectionType edmCollectionType = (IEdmCollectionType)metadataUriParseResult.EdmType;
			if (expectedItemTypeReference != null && !expectedItemTypeReference.IsAssignableFrom(edmCollectionType.ElementType))
			{
				throw new ODataException(Strings.ReaderValidationUtils_MetadataUriDoesNotReferTypeAssignableToExpectedType(UriUtilsCommon.UriToString(metadataUriParseResult.MetadataUri), edmCollectionType.ElementType.ODataFullName(), expectedItemTypeReference.ODataFullName()));
			}
			return edmCollectionType.ElementType;
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00045750 File Offset: 0x00043950
		internal static void ValidateOperationProperty(object propertyValue, string propertyName, string metadata, string operationsHeader)
		{
			if (propertyValue == null)
			{
				throw new ODataException(Strings.ODataJsonOperationsDeserializerUtils_OperationPropertyCannotBeNull(propertyName, metadata, operationsHeader));
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00045764 File Offset: 0x00043964
		private static IEdmTypeReference ResolveAndValidateTargetTypeWithNoExpectedType(EdmTypeKind expectedTypeKind, IEdmType payloadType)
		{
			if (payloadType != null)
			{
				return payloadType.ToTypeReference(true);
			}
			if (expectedTypeKind == EdmTypeKind.Entity)
			{
				throw new ODataException(Strings.ReaderValidationUtils_EntryWithoutType);
			}
			return null;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x00045790 File Offset: 0x00043990
		private static IEdmTypeReference ResolveAndValidateTargetTypeStrictValidationDisabled(EdmTypeKind expectedTypeKind, IEdmTypeReference expectedTypeReference, IEdmType payloadType)
		{
			switch (expectedTypeKind)
			{
			case EdmTypeKind.Entity:
				if (payloadType != null && expectedTypeKind == payloadType.TypeKind && expectedTypeReference.AsEntity().EntityDefinition().IsAssignableFrom((IEdmEntityType)payloadType))
				{
					return payloadType.ToTypeReference(true);
				}
				return expectedTypeReference;
			case EdmTypeKind.Complex:
				if (payloadType != null && expectedTypeKind == payloadType.TypeKind)
				{
					ReaderValidationUtils.VerifyComplexType(expectedTypeReference, payloadType, false);
					return expectedTypeReference;
				}
				return expectedTypeReference;
			case EdmTypeKind.Collection:
				if (payloadType != null && expectedTypeKind == payloadType.TypeKind)
				{
					ReaderValidationUtils.VerifyCollectionComplexItemType(expectedTypeReference, payloadType);
					return expectedTypeReference;
				}
				return expectedTypeReference;
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ReaderValidationUtils_ResolveAndValidateTypeName_Strict_TypeKind));
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00045828 File Offset: 0x00043A28
		private static IEdmTypeReference ResolveAndValidateTargetTypeStrictValidationEnabled(EdmTypeKind expectedTypeKind, IEdmTypeReference expectedTypeReference, IEdmType payloadType)
		{
			switch (expectedTypeKind)
			{
			case EdmTypeKind.Entity:
				if (payloadType != null)
				{
					IEdmTypeReference edmTypeReference = payloadType.ToTypeReference(true);
					ValidationUtils.ValidateEntityTypeIsAssignable((IEdmEntityTypeReference)expectedTypeReference, (IEdmEntityTypeReference)edmTypeReference);
					return edmTypeReference;
				}
				return expectedTypeReference;
			case EdmTypeKind.Complex:
				if (payloadType != null)
				{
					ReaderValidationUtils.VerifyComplexType(expectedTypeReference, payloadType, true);
					return expectedTypeReference;
				}
				return expectedTypeReference;
			case EdmTypeKind.Collection:
				if (payloadType != null && string.CompareOrdinal(payloadType.ODataFullName(), expectedTypeReference.ODataFullName()) != 0)
				{
					ReaderValidationUtils.VerifyCollectionComplexItemType(expectedTypeReference, payloadType);
					throw new ODataException(Strings.ValidationUtils_IncompatibleType(payloadType.ODataFullName(), expectedTypeReference.ODataFullName()));
				}
				return expectedTypeReference;
			}
			throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ReaderValidationUtils_ResolveAndValidateTypeName_Strict_TypeKind));
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x000458C3 File Offset: 0x00043AC3
		private static void VerifyPayloadTypeDefined(string payloadTypeName, IEdmType payloadType)
		{
			if (payloadTypeName != null && payloadType == null)
			{
				throw new ODataException(Strings.ValidationUtils_UnrecognizedTypeName(payloadTypeName));
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x000458D8 File Offset: 0x00043AD8
		private static void VerifyComplexType(IEdmTypeReference expectedTypeReference, IEdmType payloadType, bool failIfNotRelated)
		{
			IEdmStructuredType edmStructuredType = expectedTypeReference.AsStructured().StructuredDefinition();
			IEdmStructuredType edmStructuredType2 = (IEdmStructuredType)payloadType;
			if (!edmStructuredType.IsEquivalentTo(edmStructuredType2))
			{
				if (edmStructuredType.IsAssignableFrom(edmStructuredType2))
				{
					throw new ODataException(Strings.ReaderValidationUtils_DerivedComplexTypesAreNotAllowed(edmStructuredType.ODataFullName(), edmStructuredType2.ODataFullName()));
				}
				if (failIfNotRelated)
				{
					throw new ODataException(Strings.ValidationUtils_IncompatibleType(edmStructuredType2.ODataFullName(), edmStructuredType.ODataFullName()));
				}
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004593C File Offset: 0x00043B3C
		private static void VerifyCollectionComplexItemType(IEdmTypeReference expectedTypeReference, IEdmType payloadType)
		{
			IEdmCollectionTypeReference typeReference = ValidationUtils.ValidateCollectionType(expectedTypeReference);
			IEdmTypeReference collectionItemType = typeReference.GetCollectionItemType();
			if (collectionItemType != null && collectionItemType.IsODataComplexTypeKind())
			{
				IEdmCollectionTypeReference typeReference2 = ValidationUtils.ValidateCollectionType(payloadType.ToTypeReference());
				IEdmTypeReference collectionItemType2 = typeReference2.GetCollectionItemType();
				if (collectionItemType2 != null && collectionItemType2.IsODataComplexTypeKind())
				{
					ReaderValidationUtils.VerifyComplexType(collectionItemType, collectionItemType2.Definition, false);
				}
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00045990 File Offset: 0x00043B90
		private static SerializationTypeNameAnnotation CreateSerializationTypeNameAnnotation(string payloadTypeName, IEdmTypeReference targetTypeReference)
		{
			if (payloadTypeName != null && string.CompareOrdinal(payloadTypeName, targetTypeReference.ODataFullName()) != 0)
			{
				return new SerializationTypeNameAnnotation
				{
					TypeName = payloadTypeName
				};
			}
			if (payloadTypeName == null)
			{
				return new SerializationTypeNameAnnotation
				{
					TypeName = null
				};
			}
			return null;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x000459D0 File Offset: 0x00043BD0
		private static EdmTypeKind ComputeTargetTypeKind(IEdmTypeReference expectedTypeReference, bool forEntityValue, string payloadTypeName, EdmTypeKind payloadTypeKind, ODataMessageReaderSettings messageReaderSettings, Func<EdmTypeKind> typeKindFromPayloadFunc)
		{
			bool flag = messageReaderSettings.ReaderBehavior.TypeResolver != null && payloadTypeKind != EdmTypeKind.None;
			EdmTypeKind edmTypeKind = EdmTypeKind.None;
			if (!flag)
			{
				edmTypeKind = ReaderValidationUtils.GetExpectedTypeKind(expectedTypeReference, messageReaderSettings);
			}
			EdmTypeKind edmTypeKind2;
			if (edmTypeKind != EdmTypeKind.None)
			{
				edmTypeKind2 = edmTypeKind;
			}
			else if (payloadTypeKind != EdmTypeKind.None)
			{
				if (!forEntityValue)
				{
					ValidationUtils.ValidateValueTypeKind(payloadTypeKind, payloadTypeName);
				}
				edmTypeKind2 = payloadTypeKind;
			}
			else
			{
				edmTypeKind2 = typeKindFromPayloadFunc();
			}
			if (ReaderValidationUtils.ShouldValidatePayloadTypeKind(messageReaderSettings, expectedTypeReference, payloadTypeKind))
			{
				ValidationUtils.ValidateTypeKind(edmTypeKind2, expectedTypeReference.TypeKind(), payloadTypeName);
			}
			return edmTypeKind2;
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00045A40 File Offset: 0x00043C40
		private static EdmTypeKind GetExpectedTypeKind(IEdmTypeReference expectedTypeReference, ODataMessageReaderSettings messageReaderSettings)
		{
			IEdmType definition;
			if (expectedTypeReference == null || (definition = expectedTypeReference.Definition) == null)
			{
				return EdmTypeKind.None;
			}
			EdmTypeKind typeKind = definition.TypeKind;
			if (messageReaderSettings.DisablePrimitiveTypeConversion && typeKind == EdmTypeKind.Primitive && !definition.IsStream())
			{
				return EdmTypeKind.None;
			}
			return typeKind;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00045A7C File Offset: 0x00043C7C
		private static bool ShouldValidatePayloadTypeKind(ODataMessageReaderSettings messageReaderSettings, IEdmTypeReference expectedValueTypeReference, EdmTypeKind payloadTypeKind)
		{
			bool flag = messageReaderSettings.ReaderBehavior.TypeResolver != null && payloadTypeKind != EdmTypeKind.None;
			return expectedValueTypeReference != null && (!messageReaderSettings.DisableStrictMetadataValidation || flag || (expectedValueTypeReference.IsODataPrimitiveTypeKind() && messageReaderSettings.DisablePrimitiveTypeConversion));
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00045AC4 File Offset: 0x00043CC4
		private static void ValidateNullValueAllowed(IEdmTypeReference expectedValueTypeReference, bool validateNullValue, IEdmModel model, string propertyName)
		{
			if (validateNullValue && expectedValueTypeReference != null)
			{
				if (expectedValueTypeReference.IsODataPrimitiveTypeKind())
				{
					if (!expectedValueTypeReference.IsNullable)
					{
						ReaderValidationUtils.ThrowNullValueForNonNullableTypeException(expectedValueTypeReference, propertyName);
						return;
					}
				}
				else
				{
					if (expectedValueTypeReference.IsNonEntityCollectionType())
					{
						ReaderValidationUtils.ThrowNullValueForNonNullableTypeException(expectedValueTypeReference, propertyName);
						return;
					}
					if (expectedValueTypeReference.IsODataComplexTypeKind() && ValidationUtils.ShouldValidateComplexPropertyNullValue(model))
					{
						IEdmComplexTypeReference edmComplexTypeReference = expectedValueTypeReference.AsComplex();
						if (!edmComplexTypeReference.IsNullable)
						{
							ReaderValidationUtils.ThrowNullValueForNonNullableTypeException(expectedValueTypeReference, propertyName);
						}
					}
				}
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00045B25 File Offset: 0x00043D25
		private static void ThrowNullValueForNonNullableTypeException(IEdmTypeReference expectedValueTypeReference, string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ODataException(Strings.ReaderValidationUtils_NullValueForNonNullableType(expectedValueTypeReference.ODataFullName()));
			}
			throw new ODataException(Strings.ReaderValidationUtils_NullNamedValueForNonNullableType(propertyName, expectedValueTypeReference.ODataFullName()));
		}
	}
}
