using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x0200023D RID: 573
	public sealed class ValidationRuleSet : IEnumerable<ValidationRule>, IEnumerable
	{
		// Token: 0x06000D15 RID: 3349 RVA: 0x000294F9 File Offset: 0x000276F9
		public ValidationRuleSet(IEnumerable<ValidationRule> baseSet, IEnumerable<ValidationRule> newRules) : this(EdmUtil.CheckArgumentNull<IEnumerable<ValidationRule>>(baseSet, "baseSet").Concat(EdmUtil.CheckArgumentNull<IEnumerable<ValidationRule>>(newRules, "newRules")))
		{
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0002951C File Offset: 0x0002771C
		public ValidationRuleSet(IEnumerable<ValidationRule> rules)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<ValidationRule>>(rules, "rules");
			this.rules = new Dictionary<Type, List<ValidationRule>>();
			foreach (ValidationRule rule in rules)
			{
				this.AddRule(rule);
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00029584 File Offset: 0x00027784
		public static ValidationRuleSet GetEdmModelRuleSet(Version version)
		{
			if (version == EdmConstants.EdmVersion1)
			{
				return ValidationRuleSet.V1RuleSet;
			}
			if (version == EdmConstants.EdmVersion1_1)
			{
				return ValidationRuleSet.V1_1RuleSet;
			}
			if (version == EdmConstants.EdmVersion1_2)
			{
				return ValidationRuleSet.V1_2RuleSet;
			}
			if (version == EdmConstants.EdmVersion2)
			{
				return ValidationRuleSet.V2RuleSet;
			}
			if (version == EdmConstants.EdmVersion3)
			{
				return ValidationRuleSet.V3RuleSet;
			}
			throw new InvalidOperationException(Strings.Serializer_UnknownEdmVersion);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000297B4 File Offset: 0x000279B4
		public IEnumerator<ValidationRule> GetEnumerator()
		{
			foreach (List<ValidationRule> ruleList in this.rules.Values)
			{
				foreach (ValidationRule rule in ruleList)
				{
					yield return rule;
				}
			}
			yield break;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000297D0 File Offset: 0x000279D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x000297D8 File Offset: 0x000279D8
		internal IEnumerable<ValidationRule> GetRules(Type t)
		{
			List<ValidationRule> result;
			if (!this.rules.TryGetValue(t, out result))
			{
				return Enumerable.Empty<ValidationRule>();
			}
			return result;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000297FC File Offset: 0x000279FC
		private void AddRule(ValidationRule rule)
		{
			List<ValidationRule> list;
			if (!this.rules.TryGetValue(rule.ValidatedType, out list))
			{
				list = new List<ValidationRule>();
				this.rules[rule.ValidatedType] = list;
			}
			if (list.Contains(rule))
			{
				throw new InvalidOperationException(Strings.RuleSet_DuplicateRulesExistInRuleSet);
			}
			list.Add(rule);
		}

		// Token: 0x0400067D RID: 1661
		private readonly Dictionary<Type, List<ValidationRule>> rules;

		// Token: 0x0400067E RID: 1662
		private static readonly ValidationRuleSet BaseRuleSet = new ValidationRuleSet(new ValidationRule[]
		{
			ValidationRules.EntityTypeKeyPropertyMustBelongToEntity,
			ValidationRules.StructuredTypePropertiesDeclaringTypeMustBeCorrect,
			ValidationRules.NamedElementNameMustNotBeEmptyOrWhiteSpace,
			ValidationRules.NamedElementNameIsTooLong,
			ValidationRules.NamedElementNameIsNotAllowed,
			ValidationRules.SchemaElementNamespaceIsNotAllowed,
			ValidationRules.SchemaElementNamespaceIsTooLong,
			ValidationRules.SchemaElementNamespaceMustNotBeEmptyOrWhiteSpace,
			ValidationRules.SchemaElementSystemNamespaceEncountered,
			ValidationRules.EntityContainerDuplicateEntityContainerMemberName,
			ValidationRules.EntityTypeDuplicatePropertyNameSpecifiedInEntityKey,
			ValidationRules.EntityTypeInvalidKeyNullablePart,
			ValidationRules.EntityTypeEntityKeyMustBeScalar,
			ValidationRules.EntityTypeInvalidKeyKeyDefinedInBaseClass,
			ValidationRules.EntityTypeKeyMissingOnEntityType,
			ValidationRules.StructuredTypeInvalidMemberNameMatchesTypeName,
			ValidationRules.StructuredTypePropertyNameAlreadyDefined,
			ValidationRules.StructuralPropertyInvalidPropertyType,
			ValidationRules.ComplexTypeInvalidAbstractComplexType,
			ValidationRules.ComplexTypeInvalidPolymorphicComplexType,
			ValidationRules.FunctionBaseParameterNameAlreadyDefinedDuplicate,
			ValidationRules.FunctionImportReturnEntitiesButDoesNotSpecifyEntitySet,
			ValidationRules.FunctionImportEntityTypeDoesNotMatchEntitySet,
			ValidationRules.ComposableFunctionImportMustHaveReturnType,
			ValidationRules.StructuredTypeBaseTypeMustBeSameKindAsDerivedKind,
			ValidationRules.RowTypeBaseTypeMustBeNull,
			ValidationRules.NavigationPropertyWithRecursiveContainmentTargetMustBeOptional,
			ValidationRules.NavigationPropertyWithRecursiveContainmentSourceMustBeFromZeroOrOne,
			ValidationRules.NavigationPropertyWithNonRecursiveContainmentSourceMustBeFromOne,
			ValidationRules.EntitySetInaccessibleEntityType,
			ValidationRules.StructuredTypeInaccessibleBaseType,
			ValidationRules.EntityReferenceTypeInaccessibleEntityType,
			ValidationRules.TypeReferenceInaccessibleSchemaType,
			ValidationRules.EntitySetTypeHasNoKeys,
			ValidationRules.FunctionOnlyInputParametersAllowedInFunctions,
			ValidationRules.RowTypeMustContainProperties,
			ValidationRules.DecimalTypeReferenceScaleOutOfRange,
			ValidationRules.BinaryTypeReferenceBinaryMaxLengthNegative,
			ValidationRules.StringTypeReferenceStringMaxLengthNegative,
			ValidationRules.StructuralPropertyInvalidPropertyTypeConcurrencyMode,
			ValidationRules.EnumMemberValueMustHaveSameTypeAsUnderlyingType,
			ValidationRules.EnumTypeEnumMemberNameAlreadyDefined,
			ValidationRules.FunctionImportBindableFunctionImportMustHaveParameters,
			ValidationRules.FunctionImportComposableFunctionImportCannotBeSideEffecting,
			ValidationRules.FunctionImportEntitySetExpressionIsInvalid,
			ValidationRules.BinaryTypeReferenceBinaryUnboundedNotValidForMaxLength,
			ValidationRules.StringTypeReferenceStringUnboundedNotValidForMaxLength,
			ValidationRules.ImmediateValueAnnotationElementAnnotationIsValid,
			ValidationRules.ValueAnnotationAssertCorrectExpressionType,
			ValidationRules.IfExpressionAssertCorrectTestType,
			ValidationRules.CollectionExpressionAllElementsCorrectType,
			ValidationRules.RecordExpressionPropertiesMatchType,
			ValidationRules.NavigationPropertyDependentPropertiesMustBelongToDependentEntity,
			ValidationRules.NavigationPropertyInvalidOperationMultipleEndsInAssociation,
			ValidationRules.NavigationPropertyEndWithManyMultiplicityCannotHaveOperationsSpecified,
			ValidationRules.NavigationPropertyTypeMismatchRelationshipConstraint,
			ValidationRules.NavigationPropertyDuplicateDependentProperty,
			ValidationRules.NavigationPropertyPrincipalEndMultiplicity,
			ValidationRules.NavigationPropertyDependentEndMultiplicity,
			ValidationRules.NavigationPropertyCorrectType,
			ValidationRules.ImmediateValueAnnotationElementAnnotationHasNameAndNamespace,
			ValidationRules.FunctionApplicationExpressionParametersMatchAppliedFunction,
			ValidationRules.VocabularyAnnotatableNoDuplicateAnnotations,
			ValidationRules.TemporalTypeReferencePrecisionOutOfRange,
			ValidationRules.DecimalTypeReferencePrecisionOutOfRange,
			ValidationRules.ModelDuplicateEntityContainerName,
			ValidationRules.FunctionImportParametersCannotHaveModeOfNone,
			ValidationRules.TypeMustNotHaveKindOfNone,
			ValidationRules.PrimitiveTypeMustNotHaveKindOfNone,
			ValidationRules.PropertyMustNotHaveKindOfNone,
			ValidationRules.TermMustNotHaveKindOfNone,
			ValidationRules.SchemaElementMustNotHaveKindOfNone,
			ValidationRules.EntityContainerElementMustNotHaveKindOfNone,
			ValidationRules.PrimitiveValueValidForType,
			ValidationRules.EntitySetCanOnlyBeContainedByASingleNavigationProperty,
			ValidationRules.EntitySetNavigationMappingMustBeBidirectional,
			ValidationRules.EntitySetNavigationPropertyMappingsMustBeUnique,
			ValidationRules.TypeAnnotationAssertMatchesTermType,
			ValidationRules.TypeAnnotationInaccessibleTerm,
			ValidationRules.PropertyValueBindingValueIsCorrectType,
			ValidationRules.EnumMustHaveIntegerUnderlyingType,
			ValidationRules.ValueAnnotationInaccessibleTerm,
			ValidationRules.ElementDirectValueAnnotationFullNameMustBeUnique,
			ValidationRules.VocabularyAnnotationInaccessibleTarget,
			ValidationRules.ComplexTypeMustContainProperties,
			ValidationRules.EntitySetAssociationSetNameMustBeValid,
			ValidationRules.NavigationPropertyAssociationEndNameIsValid,
			ValidationRules.NavigationPropertyAssociationNameIsValid,
			ValidationRules.OnlyEntityTypesCanBeOpen,
			ValidationRules.NavigationPropertyEntityMustNotIndirectlyContainItself,
			ValidationRules.EntitySetRecursiveNavigationPropertyMappingsMustPointBackToSourceEntitySet,
			ValidationRules.EntitySetNavigationPropertyMappingMustPointToValidTargetForProperty,
			ValidationRules.DirectValueAnnotationHasXmlSerializableName
		});

		// Token: 0x0400067F RID: 1663
		private static readonly ValidationRuleSet V1RuleSet = new ValidationRuleSet(ValidationRuleSet.BaseRuleSet, new ValidationRule[]
		{
			ValidationRules.NavigationPropertyInvalidToPropertyInRelationshipConstraintBeforeV2,
			ValidationRules.FunctionsNotSupportedBeforeV2,
			ValidationRules.FunctionImportUnsupportedReturnTypeV1,
			ValidationRules.FunctionImportParametersIncorrectTypeBeforeV3,
			ValidationRules.FunctionImportIsSideEffectingNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsComposableNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsBindableNotSupportedBeforeV3,
			ValidationRules.EntityTypeEntityKeyMustNotBeBinaryBeforeV2,
			ValidationRules.EnumTypeEnumsNotSupportedBeforeV3,
			ValidationRules.NavigationPropertyContainsTargetNotSupportedBeforeV3,
			ValidationRules.StructuralPropertyNullableComplexType,
			ValidationRules.ValueTermsNotSupportedBeforeV3,
			ValidationRules.VocabularyAnnotationsNotSupportedBeforeV3,
			ValidationRules.OpenTypesNotSupported,
			ValidationRules.StreamTypeReferencesNotSupportedBeforeV3,
			ValidationRules.SpatialTypeReferencesNotSupportedBeforeV3,
			ValidationRules.ModelDuplicateSchemaElementNameBeforeV3
		});

		// Token: 0x04000680 RID: 1664
		private static readonly ValidationRuleSet V1_1RuleSet = new ValidationRuleSet(from r in ValidationRuleSet.BaseRuleSet
		where r != ValidationRules.ComplexTypeInvalidAbstractComplexType && r != ValidationRules.ComplexTypeInvalidPolymorphicComplexType
		select r, new ValidationRule[]
		{
			ValidationRules.NavigationPropertyInvalidToPropertyInRelationshipConstraintBeforeV2,
			ValidationRules.FunctionsNotSupportedBeforeV2,
			ValidationRules.FunctionImportUnsupportedReturnTypeAfterV1,
			ValidationRules.FunctionImportIsSideEffectingNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsComposableNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsBindableNotSupportedBeforeV3,
			ValidationRules.EntityTypeEntityKeyMustNotBeBinaryBeforeV2,
			ValidationRules.FunctionImportParametersIncorrectTypeBeforeV3,
			ValidationRules.EnumTypeEnumsNotSupportedBeforeV3,
			ValidationRules.NavigationPropertyContainsTargetNotSupportedBeforeV3,
			ValidationRules.ValueTermsNotSupportedBeforeV3,
			ValidationRules.VocabularyAnnotationsNotSupportedBeforeV3,
			ValidationRules.OpenTypesNotSupported,
			ValidationRules.StreamTypeReferencesNotSupportedBeforeV3,
			ValidationRules.SpatialTypeReferencesNotSupportedBeforeV3,
			ValidationRules.ModelDuplicateSchemaElementNameBeforeV3
		});

		// Token: 0x04000681 RID: 1665
		private static readonly ValidationRuleSet V1_2RuleSet = new ValidationRuleSet(from r in ValidationRuleSet.BaseRuleSet
		where r != ValidationRules.ComplexTypeInvalidAbstractComplexType && r != ValidationRules.ComplexTypeInvalidPolymorphicComplexType
		select r, new ValidationRule[]
		{
			ValidationRules.NavigationPropertyInvalidToPropertyInRelationshipConstraintBeforeV2,
			ValidationRules.FunctionsNotSupportedBeforeV2,
			ValidationRules.FunctionImportUnsupportedReturnTypeAfterV1,
			ValidationRules.FunctionImportParametersIncorrectTypeBeforeV3,
			ValidationRules.FunctionImportIsSideEffectingNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsComposableNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsBindableNotSupportedBeforeV3,
			ValidationRules.EntityTypeEntityKeyMustNotBeBinaryBeforeV2,
			ValidationRules.EnumTypeEnumsNotSupportedBeforeV3,
			ValidationRules.NavigationPropertyContainsTargetNotSupportedBeforeV3,
			ValidationRules.ValueTermsNotSupportedBeforeV3,
			ValidationRules.VocabularyAnnotationsNotSupportedBeforeV3,
			ValidationRules.StreamTypeReferencesNotSupportedBeforeV3,
			ValidationRules.SpatialTypeReferencesNotSupportedBeforeV3,
			ValidationRules.ModelDuplicateSchemaElementNameBeforeV3
		});

		// Token: 0x04000682 RID: 1666
		private static readonly ValidationRuleSet V2RuleSet = new ValidationRuleSet(ValidationRuleSet.BaseRuleSet, new ValidationRule[]
		{
			ValidationRules.FunctionImportParametersIncorrectTypeBeforeV3,
			ValidationRules.FunctionImportUnsupportedReturnTypeAfterV1,
			ValidationRules.FunctionImportIsSideEffectingNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsComposableNotSupportedBeforeV3,
			ValidationRules.FunctionImportIsBindableNotSupportedBeforeV3,
			ValidationRules.EnumTypeEnumsNotSupportedBeforeV3,
			ValidationRules.NavigationPropertyContainsTargetNotSupportedBeforeV3,
			ValidationRules.StructuralPropertyNullableComplexType,
			ValidationRules.ValueTermsNotSupportedBeforeV3,
			ValidationRules.VocabularyAnnotationsNotSupportedBeforeV3,
			ValidationRules.OpenTypesNotSupported,
			ValidationRules.StreamTypeReferencesNotSupportedBeforeV3,
			ValidationRules.SpatialTypeReferencesNotSupportedBeforeV3,
			ValidationRules.ModelDuplicateSchemaElementNameBeforeV3
		});

		// Token: 0x04000683 RID: 1667
		private static readonly ValidationRuleSet V3RuleSet = new ValidationRuleSet(ValidationRuleSet.BaseRuleSet, new ValidationRule[]
		{
			ValidationRules.FunctionImportUnsupportedReturnTypeAfterV1,
			ValidationRules.ModelDuplicateSchemaElementName
		});
	}
}
