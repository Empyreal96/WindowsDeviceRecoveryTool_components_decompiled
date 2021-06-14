using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Validation.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.Serialization
{
	// Token: 0x020000A8 RID: 168
	internal static class SerializationValidator
	{
		// Token: 0x060002C4 RID: 708 RVA: 0x00006D84 File Offset: 0x00004F84
		public static IEnumerable<EdmError> GetSerializationErrors(this IEdmModel root)
		{
			IEnumerable<EdmError> enumerable;
			root.Validate(SerializationValidator.serializationRuleSet, out enumerable);
			enumerable = enumerable.Where(new Func<EdmError, bool>(SerializationValidator.SignificantToSerialization));
			return enumerable;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00006DB4 File Offset: 0x00004FB4
		internal static bool SignificantToSerialization(EdmError error)
		{
			if (ValidationHelper.IsInterfaceCritical(error))
			{
				return true;
			}
			EdmErrorCode errorCode = error.ErrorCode;
			if (errorCode <= EdmErrorCode.RowTypeMustNotHaveBaseType)
			{
				if (errorCode <= EdmErrorCode.NameTooLong)
				{
					if (errorCode != EdmErrorCode.InvalidName && errorCode != EdmErrorCode.NameTooLong)
					{
						return false;
					}
				}
				else if (errorCode != EdmErrorCode.FunctionImportEntitySetExpressionIsInvalid)
				{
					switch (errorCode)
					{
					case EdmErrorCode.SystemNamespaceEncountered:
					case EdmErrorCode.InvalidNamespaceName:
						break;
					case (EdmErrorCode)162:
						return false;
					default:
						if (errorCode != EdmErrorCode.RowTypeMustNotHaveBaseType)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (errorCode <= EdmErrorCode.EnumMemberTypeMustMatchEnumUnderlyingType)
			{
				switch (errorCode)
				{
				case EdmErrorCode.OnlyInputParametersAllowedInFunctions:
				case EdmErrorCode.FunctionImportParameterIncorrectType:
					break;
				case EdmErrorCode.ComplexTypeMustHaveProperties:
					return false;
				default:
					if (errorCode != EdmErrorCode.EnumMemberTypeMustMatchEnumUnderlyingType)
					{
						return false;
					}
					break;
				}
			}
			else if (errorCode != EdmErrorCode.ReferencedTypeMustHaveValidName)
			{
				switch (errorCode)
				{
				case EdmErrorCode.InvalidFunctionImportParameterMode:
				case EdmErrorCode.TypeMustNotHaveKindOfNone:
				case EdmErrorCode.PrimitiveTypeMustNotHaveKindOfNone:
				case EdmErrorCode.PropertyMustNotHaveKindOfNone:
				case EdmErrorCode.TermMustNotHaveKindOfNone:
				case EdmErrorCode.SchemaElementMustNotHaveKindOfNone:
				case EdmErrorCode.EntityContainerElementMustNotHaveKindOfNone:
				case EdmErrorCode.BinaryValueCannotHaveEmptyValue:
					break;
				default:
					if (errorCode != EdmErrorCode.EnumMustHaveIntegerUnderlyingType)
					{
						return false;
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x0400013A RID: 314
		private static readonly ValidationRule<IEdmTypeReference> TypeReferenceTargetMustHaveValidName = new ValidationRule<IEdmTypeReference>(delegate(ValidationContext context, IEdmTypeReference typeReference)
		{
			IEdmSchemaType edmSchemaType = typeReference.Definition as IEdmSchemaType;
			if (edmSchemaType != null && !EdmUtil.IsQualifiedName(edmSchemaType.FullName()))
			{
				context.AddError(typeReference.Location(), EdmErrorCode.ReferencedTypeMustHaveValidName, Strings.Serializer_ReferencedTypeMustHaveValidName(edmSchemaType.FullName()));
			}
		});

		// Token: 0x0400013B RID: 315
		private static readonly ValidationRule<IEdmEntityReferenceType> EntityReferenceTargetMustHaveValidName = new ValidationRule<IEdmEntityReferenceType>(delegate(ValidationContext context, IEdmEntityReferenceType entityReference)
		{
			if (!EdmUtil.IsQualifiedName(entityReference.EntityType.FullName()))
			{
				context.AddError(entityReference.Location(), EdmErrorCode.ReferencedTypeMustHaveValidName, Strings.Serializer_ReferencedTypeMustHaveValidName(entityReference.EntityType.FullName()));
			}
		});

		// Token: 0x0400013C RID: 316
		private static readonly ValidationRule<IEdmEntitySet> EntitySetTypeMustHaveValidName = new ValidationRule<IEdmEntitySet>(delegate(ValidationContext context, IEdmEntitySet set)
		{
			if (!EdmUtil.IsQualifiedName(set.ElementType.FullName()))
			{
				context.AddError(set.Location(), EdmErrorCode.ReferencedTypeMustHaveValidName, Strings.Serializer_ReferencedTypeMustHaveValidName(set.ElementType.FullName()));
			}
		});

		// Token: 0x0400013D RID: 317
		private static readonly ValidationRule<IEdmStructuredType> StructuredTypeBaseTypeMustHaveValidName = new ValidationRule<IEdmStructuredType>(delegate(ValidationContext context, IEdmStructuredType type)
		{
			IEdmSchemaType edmSchemaType = type.BaseType as IEdmSchemaType;
			if (edmSchemaType != null && !EdmUtil.IsQualifiedName(edmSchemaType.FullName()))
			{
				context.AddError(type.Location(), EdmErrorCode.ReferencedTypeMustHaveValidName, Strings.Serializer_ReferencedTypeMustHaveValidName(edmSchemaType.FullName()));
			}
		});

		// Token: 0x0400013E RID: 318
		private static readonly ValidationRule<IEdmNavigationProperty> NavigationPropertyVerifyAssociationName = new ValidationRule<IEdmNavigationProperty>(delegate(ValidationContext context, IEdmNavigationProperty property)
		{
			if (!EdmUtil.IsQualifiedName(context.Model.GetAssociationFullName(property)))
			{
				context.AddError(property.Location(), EdmErrorCode.ReferencedTypeMustHaveValidName, Strings.Serializer_ReferencedTypeMustHaveValidName(context.Model.GetAssociationFullName(property)));
			}
		});

		// Token: 0x0400013F RID: 319
		private static readonly ValidationRule<IEdmVocabularyAnnotation> VocabularyAnnotationOutOfLineMustHaveValidTargetName = new ValidationRule<IEdmVocabularyAnnotation>(delegate(ValidationContext context, IEdmVocabularyAnnotation annotation)
		{
			if (annotation.GetSerializationLocation(context.Model) == EdmVocabularyAnnotationSerializationLocation.OutOfLine && !EdmUtil.IsQualifiedName(annotation.TargetString()))
			{
				context.AddError(annotation.Location(), EdmErrorCode.InvalidName, Strings.Serializer_OutOfLineAnnotationTargetMustHaveValidName(EdmUtil.FullyQualifiedName(annotation.Target)));
			}
		});

		// Token: 0x04000140 RID: 320
		private static readonly ValidationRule<IEdmVocabularyAnnotation> VocabularyAnnotationMustHaveValidTermName = new ValidationRule<IEdmVocabularyAnnotation>(delegate(ValidationContext context, IEdmVocabularyAnnotation annotation)
		{
			if (!EdmUtil.IsQualifiedName(annotation.Term.FullName()))
			{
				context.AddError(annotation.Location(), EdmErrorCode.InvalidName, Strings.Serializer_OutOfLineAnnotationTargetMustHaveValidName(annotation.Term.FullName()));
			}
		});

		// Token: 0x04000141 RID: 321
		private static ValidationRuleSet serializationRuleSet = new ValidationRuleSet(new ValidationRule[]
		{
			SerializationValidator.TypeReferenceTargetMustHaveValidName,
			SerializationValidator.EntityReferenceTargetMustHaveValidName,
			SerializationValidator.EntitySetTypeMustHaveValidName,
			SerializationValidator.StructuredTypeBaseTypeMustHaveValidName,
			SerializationValidator.VocabularyAnnotationOutOfLineMustHaveValidTargetName,
			SerializationValidator.VocabularyAnnotationMustHaveValidTermName,
			SerializationValidator.NavigationPropertyVerifyAssociationName,
			ValidationRules.FunctionImportEntitySetExpressionIsInvalid,
			ValidationRules.FunctionImportParametersCannotHaveModeOfNone,
			ValidationRules.FunctionOnlyInputParametersAllowedInFunctions,
			ValidationRules.TypeMustNotHaveKindOfNone,
			ValidationRules.PrimitiveTypeMustNotHaveKindOfNone,
			ValidationRules.PropertyMustNotHaveKindOfNone,
			ValidationRules.TermMustNotHaveKindOfNone,
			ValidationRules.SchemaElementMustNotHaveKindOfNone,
			ValidationRules.EntityContainerElementMustNotHaveKindOfNone,
			ValidationRules.EnumMustHaveIntegerUnderlyingType,
			ValidationRules.EnumMemberValueMustHaveSameTypeAsUnderlyingType
		});
	}
}
