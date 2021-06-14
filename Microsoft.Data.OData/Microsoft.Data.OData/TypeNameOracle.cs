using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x020000E4 RID: 228
	internal class TypeNameOracle
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x000139E4 File Offset: 0x00011BE4
		internal static IEdmType ResolveAndValidateTypeName(IEdmModel model, string typeName, EdmTypeKind expectedTypeKind)
		{
			if (typeName == null)
			{
				if (model.IsUserModel())
				{
					throw new ODataException(Strings.WriterValidationUtils_MissingTypeNameWithMetadata);
				}
				return null;
			}
			else
			{
				if (typeName.Length == 0)
				{
					throw new ODataException(Strings.ValidationUtils_TypeNameMustNotBeEmpty);
				}
				if (!model.IsUserModel())
				{
					return null;
				}
				IEdmType edmType = MetadataUtils.ResolveTypeNameForWrite(model, typeName);
				if (edmType == null)
				{
					throw new ODataException(Strings.ValidationUtils_UnrecognizedTypeName(typeName));
				}
				ValidationUtils.ValidateTypeKind(edmType.TypeKind, expectedTypeKind, edmType.ODataFullName());
				return edmType;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00013A50 File Offset: 0x00011C50
		internal static IEdmTypeReference ResolveAndValidateTypeNameForValue(IEdmModel model, IEdmTypeReference typeReferenceFromMetadata, ODataValue value, bool isOpenProperty)
		{
			ODataPrimitiveValue odataPrimitiveValue = value as ODataPrimitiveValue;
			if (odataPrimitiveValue != null)
			{
				return EdmLibraryExtensions.GetPrimitiveTypeReference(odataPrimitiveValue.Value.GetType());
			}
			ODataComplexValue odataComplexValue = value as ODataComplexValue;
			if (odataComplexValue != null)
			{
				return TypeNameOracle.ResolveAndValidateTypeFromNameAndMetadata(model, typeReferenceFromMetadata, odataComplexValue.TypeName, EdmTypeKind.Complex, isOpenProperty);
			}
			ODataCollectionValue odataCollectionValue = (ODataCollectionValue)value;
			return TypeNameOracle.ResolveAndValidateTypeFromNameAndMetadata(model, typeReferenceFromMetadata, odataCollectionValue.TypeName, EdmTypeKind.Collection, isOpenProperty);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00013AA8 File Offset: 0x00011CA8
		protected static string GetTypeNameFromValue(object value)
		{
			ODataPrimitiveValue odataPrimitiveValue = value as ODataPrimitiveValue;
			if (odataPrimitiveValue != null)
			{
				return EdmLibraryExtensions.GetPrimitiveTypeReference(odataPrimitiveValue.Value.GetType()).ODataFullName();
			}
			ODataComplexValue odataComplexValue = value as ODataComplexValue;
			if (odataComplexValue != null)
			{
				return odataComplexValue.TypeName;
			}
			ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
			if (odataCollectionValue != null)
			{
				return odataCollectionValue.TypeName;
			}
			IEdmPrimitiveTypeReference primitiveTypeReference = EdmLibraryExtensions.GetPrimitiveTypeReference(value.GetType());
			if (primitiveTypeReference == null)
			{
				throw new ODataException(Strings.ValidationUtils_UnsupportedPrimitiveType(value.GetType().FullName));
			}
			return primitiveTypeReference.ODataFullName();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00013B24 File Offset: 0x00011D24
		private static IEdmTypeReference ResolveAndValidateTypeFromNameAndMetadata(IEdmModel model, IEdmTypeReference typeReferenceFromMetadata, string typeName, EdmTypeKind typeKindFromValue, bool isOpenPropertyType)
		{
			if (typeName == null && model.IsUserModel() && isOpenPropertyType)
			{
				throw new ODataException(Strings.WriterValidationUtils_MissingTypeNameWithMetadata);
			}
			IEdmType edmType = (typeName == null) ? null : TypeNameOracle.ResolveAndValidateTypeName(model, typeName, typeKindFromValue);
			if (typeReferenceFromMetadata != null)
			{
				ValidationUtils.ValidateTypeKind(typeKindFromValue, typeReferenceFromMetadata.TypeKind(), (edmType == null) ? null : edmType.ODataFullName());
			}
			IEdmTypeReference edmTypeReference = TypeNameOracle.ValidateMetadataType(typeReferenceFromMetadata, (edmType == null) ? null : edmType.ToTypeReference());
			if (typeKindFromValue == EdmTypeKind.Collection && edmTypeReference != null)
			{
				edmTypeReference = ValidationUtils.ValidateCollectionType(edmTypeReference);
			}
			return edmTypeReference;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00013B98 File Offset: 0x00011D98
		private static IEdmTypeReference ValidateMetadataType(IEdmTypeReference typeReferenceFromMetadata, IEdmTypeReference typeReferenceFromValue)
		{
			if (typeReferenceFromMetadata == null)
			{
				return typeReferenceFromValue;
			}
			if (typeReferenceFromValue == null)
			{
				return typeReferenceFromMetadata;
			}
			if (typeReferenceFromValue.IsODataPrimitiveTypeKind())
			{
				ValidationUtils.ValidateMetadataPrimitiveType(typeReferenceFromMetadata, typeReferenceFromValue);
			}
			else if (typeReferenceFromMetadata.IsEntity())
			{
				ValidationUtils.ValidateEntityTypeIsAssignable((IEdmEntityTypeReference)typeReferenceFromMetadata, (IEdmEntityTypeReference)typeReferenceFromValue);
			}
			else if (typeReferenceFromMetadata.ODataFullName() != typeReferenceFromValue.ODataFullName())
			{
				throw new ODataException(Strings.ValidationUtils_IncompatibleType(typeReferenceFromValue.ODataFullName(), typeReferenceFromMetadata.ODataFullName()));
			}
			return typeReferenceFromValue;
		}
	}
}
