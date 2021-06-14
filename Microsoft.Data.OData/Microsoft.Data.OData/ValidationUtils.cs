using System;
using System.Globalization;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x02000255 RID: 597
	internal static class ValidationUtils
	{
		// Token: 0x06001397 RID: 5015 RVA: 0x000497FC File Offset: 0x000479FC
		internal static void ValidateOpenPropertyValue(string propertyName, object value)
		{
			if (value is ODataStreamReferenceValue)
			{
				throw new ODataException(Strings.ValidationUtils_OpenStreamProperty(propertyName));
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00049812 File Offset: 0x00047A12
		internal static void ValidateValueTypeKind(EdmTypeKind typeKind, string typeName)
		{
			if (typeKind != EdmTypeKind.Primitive && typeKind != EdmTypeKind.Complex && typeKind != EdmTypeKind.Collection)
			{
				throw new ODataException(Strings.ValidationUtils_IncorrectValueTypeKind(typeName, typeKind.ToString()));
			}
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00049838 File Offset: 0x00047A38
		internal static string ValidateCollectionTypeName(string collectionTypeName)
		{
			string collectionItemTypeName = EdmLibraryExtensions.GetCollectionItemTypeName(collectionTypeName);
			if (collectionItemTypeName == null)
			{
				throw new ODataException(Strings.ValidationUtils_InvalidCollectionTypeName(collectionTypeName));
			}
			return collectionItemTypeName;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0004985C File Offset: 0x00047A5C
		internal static void ValidateEntityTypeIsAssignable(IEdmEntityTypeReference expectedEntityTypeReference, IEdmEntityTypeReference payloadEntityTypeReference)
		{
			if (!expectedEntityTypeReference.EntityDefinition().IsAssignableFrom(payloadEntityTypeReference.EntityDefinition()))
			{
				throw new ODataException(Strings.ValidationUtils_EntryTypeNotAssignableToExpectedType(payloadEntityTypeReference.ODataFullName(), expectedEntityTypeReference.ODataFullName()));
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00049888 File Offset: 0x00047A88
		internal static IEdmCollectionTypeReference ValidateCollectionType(IEdmTypeReference typeReference)
		{
			IEdmCollectionTypeReference edmCollectionTypeReference = typeReference.AsCollectionOrNull();
			if (edmCollectionTypeReference != null && !typeReference.IsNonEntityCollectionType())
			{
				throw new ODataException(Strings.ValidationUtils_InvalidCollectionTypeReference(typeReference.TypeKind()));
			}
			return edmCollectionTypeReference;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000498BE File Offset: 0x00047ABE
		internal static void ValidateCollectionItem(object item, bool isStreamable)
		{
			if (!isStreamable && item == null)
			{
				throw new ODataException(Strings.ValidationUtils_NonStreamingCollectionElementsMustNotBeNull);
			}
			if (item is ODataCollectionValue)
			{
				throw new ODataException(Strings.ValidationUtils_NestedCollectionsAreNotSupported);
			}
			if (item is ODataStreamReferenceValue)
			{
				throw new ODataException(Strings.ValidationUtils_StreamReferenceValuesNotSupportedInCollections);
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x000498F7 File Offset: 0x00047AF7
		internal static void ValidateNullCollectionItem(IEdmTypeReference expectedItemType, ODataWriterBehavior writerBehavior)
		{
			if (expectedItemType != null && expectedItemType.IsODataPrimitiveTypeKind() && !expectedItemType.IsNullable && !writerBehavior.AllowNullValuesForNonNullablePrimitiveTypes)
			{
				throw new ODataException(Strings.ValidationUtils_NullCollectionItemForNonNullableType(expectedItemType.ODataFullName()));
			}
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00049925 File Offset: 0x00047B25
		internal static void ValidateStreamReferenceProperty(ODataProperty streamProperty, IEdmProperty edmProperty)
		{
			if (edmProperty != null && !edmProperty.Type.IsStream())
			{
				throw new ODataException(Strings.ValidationUtils_MismatchPropertyKindForStreamProperty(streamProperty.Name));
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00049948 File Offset: 0x00047B48
		internal static void ValidateAssociationLinkNotNull(ODataAssociationLink associationLink)
		{
			if (associationLink == null)
			{
				throw new ODataException(Strings.ValidationUtils_EnumerableContainsANullItem("ODataEntry.AssociationLinks"));
			}
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004995D File Offset: 0x00047B5D
		internal static void ValidateAssociationLinkName(string associationLinkName)
		{
			if (string.IsNullOrEmpty(associationLinkName))
			{
				throw new ODataException(Strings.ValidationUtils_AssociationLinkMustSpecifyName);
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00049972 File Offset: 0x00047B72
		internal static void ValidateAssociationLink(ODataAssociationLink associationLink)
		{
			ValidationUtils.ValidateAssociationLinkName(associationLink.Name);
			if (associationLink.Url == null)
			{
				throw new ODataException(Strings.ValidationUtils_AssociationLinkMustSpecifyUrl);
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00049998 File Offset: 0x00047B98
		internal static void IncreaseAndValidateRecursionDepth(ref int recursionDepth, int maxDepth)
		{
			recursionDepth++;
			if (recursionDepth > maxDepth)
			{
				throw new ODataException(Strings.ValidationUtils_RecursionDepthLimitReached(maxDepth));
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000499B8 File Offset: 0x00047BB8
		internal static void ValidateOperationNotNull(ODataOperation operation, bool isAction)
		{
			if (operation == null)
			{
				string p = isAction ? "ODataEntry.Actions" : "ODataEntry.Functions";
				throw new ODataException(Strings.ValidationUtils_EnumerableContainsANullItem(p));
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000499E4 File Offset: 0x00047BE4
		internal static void ValidateOperationMetadataNotNull(ODataOperation operation)
		{
			if (operation.Metadata == null)
			{
				throw new ODataException(Strings.ValidationUtils_ActionsAndFunctionsMustSpecifyMetadata(operation.GetType().Name));
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00049A0A File Offset: 0x00047C0A
		internal static void ValidateOperationTargetNotNull(ODataOperation operation)
		{
			if (operation.Target == null)
			{
				throw new ODataException(Strings.ValidationUtils_ActionsAndFunctionsMustSpecifyTarget(operation.GetType().Name));
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00049A30 File Offset: 0x00047C30
		internal static void ValidateEntryMetadataResource(ODataEntry entry, IEdmEntityType entityType, IEdmModel model, bool validateMediaResource)
		{
			if (entityType != null && validateMediaResource)
			{
				bool flag = model.HasDefaultStream(entityType);
				if (entry.MediaResource == null)
				{
					if (flag)
					{
						throw new ODataException(Strings.ValidationUtils_EntryWithoutMediaResourceAndMLEType(entityType.ODataFullName()));
					}
				}
				else if (!flag)
				{
					throw new ODataException(Strings.ValidationUtils_EntryWithMediaResourceAndNonMLEType(entityType.ODataFullName()));
				}
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00049A7C File Offset: 0x00047C7C
		internal static void ValidateIsExpectedPrimitiveType(object value, IEdmTypeReference expectedTypeReference)
		{
			Type type = value.GetType();
			IEdmPrimitiveTypeReference primitiveTypeReference = EdmLibraryExtensions.GetPrimitiveTypeReference(type);
			ValidationUtils.ValidateIsExpectedPrimitiveType(value, primitiveTypeReference, expectedTypeReference);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00049A9F File Offset: 0x00047C9F
		internal static void ValidateIsExpectedPrimitiveType(object value, IEdmPrimitiveTypeReference valuePrimitiveTypeReference, IEdmTypeReference expectedTypeReference)
		{
			if (valuePrimitiveTypeReference == null)
			{
				throw new ODataException(Strings.ValidationUtils_UnsupportedPrimitiveType(value.GetType().FullName));
			}
			if (!expectedTypeReference.IsODataPrimitiveTypeKind())
			{
				throw new ODataException(Strings.ValidationUtils_NonPrimitiveTypeForPrimitiveValue(expectedTypeReference.ODataFullName()));
			}
			ValidationUtils.ValidateMetadataPrimitiveType(expectedTypeReference, valuePrimitiveTypeReference);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00049ADC File Offset: 0x00047CDC
		internal static void ValidateMetadataPrimitiveType(IEdmTypeReference expectedTypeReference, IEdmTypeReference typeReferenceFromValue)
		{
			IEdmPrimitiveType baseType = (IEdmPrimitiveType)expectedTypeReference.Definition;
			IEdmPrimitiveType subtype = (IEdmPrimitiveType)typeReferenceFromValue.Definition;
			bool flag = expectedTypeReference.IsNullable == typeReferenceFromValue.IsNullable || (expectedTypeReference.IsNullable && !typeReferenceFromValue.IsNullable) || !typeReferenceFromValue.IsODataValueType();
			bool flag2 = baseType.IsAssignableFrom(subtype);
			if (!flag || !flag2)
			{
				throw new ODataException(Strings.ValidationUtils_IncompatiblePrimitiveItemType(typeReferenceFromValue.ODataFullName(), typeReferenceFromValue.IsNullable, expectedTypeReference.ODataFullName(), expectedTypeReference.IsNullable));
			}
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00049B67 File Offset: 0x00047D67
		internal static void ValidateResourceCollectionInfo(ODataResourceCollectionInfo collectionInfo)
		{
			if (collectionInfo == null)
			{
				throw new ODataException(Strings.ValidationUtils_WorkspaceCollectionsMustNotContainNullItem);
			}
			if (collectionInfo.Url == null)
			{
				throw new ODataException(Strings.ValidationUtils_ResourceCollectionMustSpecifyUrl);
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00049B90 File Offset: 0x00047D90
		internal static void ValidateResourceCollectionInfoUrl(string collectionInfoUrl)
		{
			if (collectionInfoUrl == null)
			{
				throw new ODataException(Strings.ValidationUtils_ResourceCollectionUrlMustNotBeNull);
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00049BA0 File Offset: 0x00047DA0
		internal static void ValidateTypeKind(EdmTypeKind actualTypeKind, EdmTypeKind expectedTypeKind, string typeName)
		{
			if (actualTypeKind == expectedTypeKind)
			{
				return;
			}
			if (typeName == null)
			{
				throw new ODataException(Strings.ValidationUtils_IncorrectTypeKindNoTypeName(actualTypeKind.ToString(), expectedTypeKind.ToString()));
			}
			throw new ODataException(Strings.ValidationUtils_IncorrectTypeKind(typeName, expectedTypeKind.ToString(), actualTypeKind.ToString()));
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00049BF7 File Offset: 0x00047DF7
		internal static void ValidateBoundaryString(string boundary)
		{
			if (boundary == null || boundary.Length == 0 || boundary.Length > 70)
			{
				throw new ODataException(Strings.ValidationUtils_InvalidBatchBoundaryDelimiterLength(boundary, 70));
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00049C24 File Offset: 0x00047E24
		internal static bool ShouldValidateComplexPropertyNullValue(IEdmModel model)
		{
			Version edmVersion = model.GetEdmVersion();
			Version dataServiceVersion = model.GetDataServiceVersion();
			return !(edmVersion != null) || !(dataServiceVersion != null) || !(edmVersion < ODataVersion.V3.ToDataServiceVersion());
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00049C62 File Offset: 0x00047E62
		internal static bool IsValidPropertyName(string propertyName)
		{
			return propertyName.IndexOfAny(ValidationUtils.InvalidCharactersInPropertyNames) < 0;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00049CA4 File Offset: 0x00047EA4
		internal static void ValidatePropertyName(string propertyName)
		{
			if (!ValidationUtils.IsValidPropertyName(propertyName))
			{
				string p = string.Join(", ", (from c in ValidationUtils.InvalidCharactersInPropertyNames
				select string.Format(CultureInfo.InvariantCulture, "'{0}'", new object[]
				{
					c
				})).ToArray<string>());
				throw new ODataException(Strings.ValidationUtils_PropertiesMustNotContainReservedChars(propertyName, p));
			}
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00049D00 File Offset: 0x00047F00
		internal static int ValidateTotalEntityPropertyMappingCount(ODataEntityPropertyMappingCache baseCache, ODataEntityPropertyMappingCollection mappings, int maxMappingCount)
		{
			int num = (baseCache == null) ? 0 : baseCache.TotalMappingCount;
			int num2 = (mappings == null) ? 0 : mappings.Count;
			int num3 = num + num2;
			if (num3 > maxMappingCount)
			{
				throw new ODataException(Strings.ValidationUtils_MaxNumberOfEntityPropertyMappingsExceeded(num3, maxMappingCount));
			}
			return num3;
		}

		// Token: 0x040006FF RID: 1791
		private const int MaxBoundaryLength = 70;

		// Token: 0x04000700 RID: 1792
		internal static readonly char[] InvalidCharactersInPropertyNames = new char[]
		{
			':',
			'.',
			'@'
		};
	}
}
