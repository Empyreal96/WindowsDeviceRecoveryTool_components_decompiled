using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000242 RID: 578
	internal static class Strings
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0002A2E3 File Offset: 0x000284E3
		internal static string EdmPrimitive_UnexpectedKind
		{
			get
			{
				return EntityRes.GetString("EdmPrimitive_UnexpectedKind");
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002A2F0 File Offset: 0x000284F0
		internal static string Annotations_DocumentationPun(object p0)
		{
			return EntityRes.GetString("Annotations_DocumentationPun", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0002A314 File Offset: 0x00028514
		internal static string Annotations_TypeMismatch(object p0, object p1)
		{
			return EntityRes.GetString("Annotations_TypeMismatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0002A33B File Offset: 0x0002853B
		internal static string Constructable_VocabularyAnnotationMustHaveTarget
		{
			get
			{
				return EntityRes.GetString("Constructable_VocabularyAnnotationMustHaveTarget");
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0002A347 File Offset: 0x00028547
		internal static string Constructable_EntityTypeOrCollectionOfEntityTypeExpected
		{
			get
			{
				return EntityRes.GetString("Constructable_EntityTypeOrCollectionOfEntityTypeExpected");
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0002A354 File Offset: 0x00028554
		internal static string Constructable_TargetMustBeStock(object p0)
		{
			return EntityRes.GetString("Constructable_TargetMustBeStock", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0002A378 File Offset: 0x00028578
		internal static string TypeSemantics_CouldNotConvertTypeReference(object p0, object p1)
		{
			return EntityRes.GetString("TypeSemantics_CouldNotConvertTypeReference", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x0002A39F File Offset: 0x0002859F
		internal static string EdmModel_CannotUseElementWithTypeNone
		{
			get
			{
				return EntityRes.GetString("EdmModel_CannotUseElementWithTypeNone");
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0002A3AB File Offset: 0x000285AB
		internal static string EdmEntityContainer_CannotUseElementWithTypeNone
		{
			get
			{
				return EntityRes.GetString("EdmEntityContainer_CannotUseElementWithTypeNone");
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0002A3B8 File Offset: 0x000285B8
		internal static string ValueWriter_NonSerializableValue(object p0)
		{
			return EntityRes.GetString("ValueWriter_NonSerializableValue", new object[]
			{
				p0
			});
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0002A3DB File Offset: 0x000285DB
		internal static string ValueHasAlreadyBeenSet
		{
			get
			{
				return EntityRes.GetString("ValueHasAlreadyBeenSet");
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0002A3E7 File Offset: 0x000285E7
		internal static string PathSegmentMustNotContainSlash
		{
			get
			{
				return EntityRes.GetString("PathSegmentMustNotContainSlash");
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0002A3F4 File Offset: 0x000285F4
		internal static string Edm_Evaluator_NoTermTypeAnnotationOnType(object p0, object p1)
		{
			return EntityRes.GetString("Edm_Evaluator_NoTermTypeAnnotationOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0002A41C File Offset: 0x0002861C
		internal static string Edm_Evaluator_NoValueAnnotationOnType(object p0, object p1)
		{
			return EntityRes.GetString("Edm_Evaluator_NoValueAnnotationOnType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0002A444 File Offset: 0x00028644
		internal static string Edm_Evaluator_NoValueAnnotationOnElement(object p0)
		{
			return EntityRes.GetString("Edm_Evaluator_NoValueAnnotationOnElement", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0002A468 File Offset: 0x00028668
		internal static string Edm_Evaluator_UnrecognizedExpressionKind(object p0)
		{
			return EntityRes.GetString("Edm_Evaluator_UnrecognizedExpressionKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0002A48C File Offset: 0x0002868C
		internal static string Edm_Evaluator_UnboundFunction(object p0)
		{
			return EntityRes.GetString("Edm_Evaluator_UnboundFunction", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0002A4B0 File Offset: 0x000286B0
		internal static string Edm_Evaluator_UnboundPath(object p0)
		{
			return EntityRes.GetString("Edm_Evaluator_UnboundPath", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0002A4D4 File Offset: 0x000286D4
		internal static string Edm_Evaluator_FailedTypeAssertion(object p0)
		{
			return EntityRes.GetString("Edm_Evaluator_FailedTypeAssertion", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0002A4F8 File Offset: 0x000286F8
		internal static string EdmModel_Validator_Semantic_SystemNamespaceEncountered(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_SystemNamespaceEncountered", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0002A51C File Offset: 0x0002871C
		internal static string EdmModel_Validator_Semantic_EntitySetTypeHasNoKeys(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntitySetTypeHasNoKeys", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0002A544 File Offset: 0x00028744
		internal static string EdmModel_Validator_Semantic_DuplicateEndName(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateEndName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0002A568 File Offset: 0x00028768
		internal static string EdmModel_Validator_Semantic_DuplicatePropertyNameSpecifiedInEntityKey(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicatePropertyNameSpecifiedInEntityKey", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0002A590 File Offset: 0x00028790
		internal static string EdmModel_Validator_Semantic_InvalidComplexTypeAbstract(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidComplexTypeAbstract", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0002A5B4 File Offset: 0x000287B4
		internal static string EdmModel_Validator_Semantic_InvalidComplexTypePolymorphic(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidComplexTypePolymorphic", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0002A5D8 File Offset: 0x000287D8
		internal static string EdmModel_Validator_Semantic_InvalidKeyNullablePart(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidKeyNullablePart", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0002A600 File Offset: 0x00028800
		internal static string EdmModel_Validator_Semantic_EntityKeyMustBeScalar(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntityKeyMustBeScalar", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002A628 File Offset: 0x00028828
		internal static string EdmModel_Validator_Semantic_InvalidKeyKeyDefinedInBaseClass(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidKeyKeyDefinedInBaseClass", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002A650 File Offset: 0x00028850
		internal static string EdmModel_Validator_Semantic_KeyMissingOnEntityType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_KeyMissingOnEntityType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002A674 File Offset: 0x00028874
		internal static string EdmModel_Validator_Semantic_BadNavigationPropertyUndefinedRole(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_BadNavigationPropertyUndefinedRole", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0002A6A0 File Offset: 0x000288A0
		internal static string EdmModel_Validator_Semantic_BadNavigationPropertyRolesCannotBeTheSame(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_BadNavigationPropertyRolesCannotBeTheSame", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002A6C4 File Offset: 0x000288C4
		internal static string EdmModel_Validator_Semantic_BadNavigationPropertyCouldNotDetermineType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_BadNavigationPropertyCouldNotDetermineType", new object[]
			{
				p0
			});
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0002A6E7 File Offset: 0x000288E7
		internal static string EdmModel_Validator_Semantic_InvalidOperationMultipleEndsInAssociation
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidOperationMultipleEndsInAssociation");
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0002A6F4 File Offset: 0x000288F4
		internal static string EdmModel_Validator_Semantic_EndWithManyMultiplicityCannotHaveOperationsSpecified(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EndWithManyMultiplicityCannotHaveOperationsSpecified", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0002A718 File Offset: 0x00028918
		internal static string EdmModel_Validator_Semantic_EndNameAlreadyDefinedDuplicate(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EndNameAlreadyDefinedDuplicate", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0002A73C File Offset: 0x0002893C
		internal static string EdmModel_Validator_Semantic_SameRoleReferredInReferentialConstraint(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_SameRoleReferredInReferentialConstraint", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0002A760 File Offset: 0x00028960
		internal static string EdmModel_Validator_Semantic_NavigationPropertyPrincipalEndMultiplicityUpperBoundMustBeOne(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyPrincipalEndMultiplicityUpperBoundMustBeOne", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0002A784 File Offset: 0x00028984
		internal static string EdmModel_Validator_Semantic_InvalidMultiplicityOfPrincipalEndDependentPropertiesAllNonnullable(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidMultiplicityOfPrincipalEndDependentPropertiesAllNonnullable", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002A7AC File Offset: 0x000289AC
		internal static string EdmModel_Validator_Semantic_InvalidMultiplicityOfPrincipalEndDependentPropertiesAllNullable(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidMultiplicityOfPrincipalEndDependentPropertiesAllNullable", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002A7D4 File Offset: 0x000289D4
		internal static string EdmModel_Validator_Semantic_InvalidMultiplicityOfDependentEndMustBeZeroOneOrOne(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidMultiplicityOfDependentEndMustBeZeroOneOrOne", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0002A7F8 File Offset: 0x000289F8
		internal static string EdmModel_Validator_Semantic_InvalidMultiplicityOfDependentEndMustBeMany(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidMultiplicityOfDependentEndMustBeMany", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002A81C File Offset: 0x00028A1C
		internal static string EdmModel_Validator_Semantic_InvalidToPropertyInRelationshipConstraint(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidToPropertyInRelationshipConstraint", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0002A843 File Offset: 0x00028A43
		internal static string EdmModel_Validator_Semantic_MismatchNumberOfPropertiesinRelationshipConstraint
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_MismatchNumberOfPropertiesinRelationshipConstraint");
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002A850 File Offset: 0x00028A50
		internal static string EdmModel_Validator_Semantic_TypeMismatchRelationshipConstraint(object p0, object p1, object p2, object p3, object p4)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_TypeMismatchRelationshipConstraint", new object[]
			{
				p0,
				p1,
				p2,
				p3,
				p4
			});
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002A884 File Offset: 0x00028A84
		internal static string EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintDependentEnd(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintDependentEnd", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002A8AC File Offset: 0x00028AAC
		internal static string EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintPrimaryEnd(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintPrimaryEnd", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002A8D4 File Offset: 0x00028AD4
		internal static string EdmModel_Validator_Semantic_NullableComplexTypeProperty(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NullableComplexTypeProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0002A8F8 File Offset: 0x00028AF8
		internal static string EdmModel_Validator_Semantic_InvalidPropertyType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidPropertyType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002A91C File Offset: 0x00028B1C
		internal static string EdmModel_Validator_Semantic_ComposableFunctionImportCannotBeSideEffecting(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ComposableFunctionImportCannotBeSideEffecting", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002A940 File Offset: 0x00028B40
		internal static string EdmModel_Validator_Semantic_BindableFunctionImportMustHaveParameters(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_BindableFunctionImportMustHaveParameters", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002A964 File Offset: 0x00028B64
		internal static string EdmModel_Validator_Semantic_FunctionImportWithUnsupportedReturnTypeV1(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportWithUnsupportedReturnTypeV1", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002A988 File Offset: 0x00028B88
		internal static string EdmModel_Validator_Semantic_FunctionImportWithUnsupportedReturnTypeAfterV1(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportWithUnsupportedReturnTypeAfterV1", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002A9AC File Offset: 0x00028BAC
		internal static string EdmModel_Validator_Semantic_FunctionImportReturnEntitiesButDoesNotSpecifyEntitySet(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportReturnEntitiesButDoesNotSpecifyEntitySet", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002A9D0 File Offset: 0x00028BD0
		internal static string EdmModel_Validator_Semantic_FunctionImportEntityTypeDoesNotMatchEntitySet(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportEntityTypeDoesNotMatchEntitySet", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002A9FC File Offset: 0x00028BFC
		internal static string EdmModel_Validator_Semantic_FunctionImportEntityTypeDoesNotMatchEntitySet2(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportEntityTypeDoesNotMatchEntitySet2", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002AA24 File Offset: 0x00028C24
		internal static string EdmModel_Validator_Semantic_FunctionImportEntitySetExpressionKindIsInvalid(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportEntitySetExpressionKindIsInvalid", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002AA4C File Offset: 0x00028C4C
		internal static string EdmModel_Validator_Semantic_FunctionImportEntitySetExpressionIsInvalid(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportEntitySetExpressionIsInvalid", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002AA70 File Offset: 0x00028C70
		internal static string EdmModel_Validator_Semantic_FunctionImportSpecifiesEntitySetButNotEntityType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportSpecifiesEntitySetButNotEntityType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002AA94 File Offset: 0x00028C94
		internal static string EdmModel_Validator_Semantic_ComposableFunctionImportMustHaveReturnType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ComposableFunctionImportMustHaveReturnType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002AAB8 File Offset: 0x00028CB8
		internal static string EdmModel_Validator_Semantic_ParameterNameAlreadyDefinedDuplicate(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ParameterNameAlreadyDefinedDuplicate", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002AADC File Offset: 0x00028CDC
		internal static string EdmModel_Validator_Semantic_DuplicateEntityContainerMemberName(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateEntityContainerMemberName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0002AB00 File Offset: 0x00028D00
		internal static string EdmModel_Validator_Semantic_SchemaElementNameAlreadyDefined(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_SchemaElementNameAlreadyDefined", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0002AB24 File Offset: 0x00028D24
		internal static string EdmModel_Validator_Semantic_InvalidMemberNameMatchesTypeName(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidMemberNameMatchesTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0002AB48 File Offset: 0x00028D48
		internal static string EdmModel_Validator_Semantic_PropertyNameAlreadyDefined(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_PropertyNameAlreadyDefined", new object[]
			{
				p0
			});
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002AB6B File Offset: 0x00028D6B
		internal static string EdmModel_Validator_Semantic_BaseTypeMustHaveSameTypeKind
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_BaseTypeMustHaveSameTypeKind");
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0002AB77 File Offset: 0x00028D77
		internal static string EdmModel_Validator_Semantic_RowTypeMustNotHaveBaseType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_RowTypeMustNotHaveBaseType");
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0002AB83 File Offset: 0x00028D83
		internal static string EdmModel_Validator_Semantic_FunctionsNotSupportedBeforeV2
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionsNotSupportedBeforeV2");
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x0002AB8F File Offset: 0x00028D8F
		internal static string EdmModel_Validator_Semantic_FunctionImportSideEffectingNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportSideEffectingNotSupportedBeforeV3");
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x0002AB9B File Offset: 0x00028D9B
		internal static string EdmModel_Validator_Semantic_FunctionImportComposableNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportComposableNotSupportedBeforeV3");
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0002ABA7 File Offset: 0x00028DA7
		internal static string EdmModel_Validator_Semantic_FunctionImportBindableNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportBindableNotSupportedBeforeV3");
			}
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002ABB4 File Offset: 0x00028DB4
		internal static string EdmModel_Validator_Semantic_KeyPropertyMustBelongToEntity(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_KeyPropertyMustBelongToEntity", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002ABDC File Offset: 0x00028DDC
		internal static string EdmModel_Validator_Semantic_DependentPropertiesMustBelongToDependentEntity(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DependentPropertiesMustBelongToDependentEntity", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0002AC04 File Offset: 0x00028E04
		internal static string EdmModel_Validator_Semantic_DeclaringTypeMustBeCorrect(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DeclaringTypeMustBeCorrect", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002AC28 File Offset: 0x00028E28
		internal static string EdmModel_Validator_Semantic_InaccessibleType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InaccessibleType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0002AC4C File Offset: 0x00028E4C
		internal static string EdmModel_Validator_Semantic_AmbiguousType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_AmbiguousType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002AC70 File Offset: 0x00028E70
		internal static string EdmModel_Validator_Semantic_InvalidNavigationPropertyType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidNavigationPropertyType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002AC94 File Offset: 0x00028E94
		internal static string EdmModel_Validator_Semantic_NavigationPropertyWithRecursiveContainmentTargetMustBeOptional(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyWithRecursiveContainmentTargetMustBeOptional", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002ACB8 File Offset: 0x00028EB8
		internal static string EdmModel_Validator_Semantic_NavigationPropertyWithRecursiveContainmentSourceMustBeFromZeroOrOne(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyWithRecursiveContainmentSourceMustBeFromZeroOrOne", new object[]
			{
				p0
			});
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002ACDC File Offset: 0x00028EDC
		internal static string EdmModel_Validator_Semantic_NavigationPropertyWithNonRecursiveContainmentSourceMustBeFromOne(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyWithNonRecursiveContainmentSourceMustBeFromOne", new object[]
			{
				p0
			});
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x0002ACFF File Offset: 0x00028EFF
		internal static string EdmModel_Validator_Semantic_NavigationPropertyContainsTargetNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyContainsTargetNotSupportedBeforeV3");
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002AD0C File Offset: 0x00028F0C
		internal static string EdmModel_Validator_Semantic_OnlyInputParametersAllowedInFunctions(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_OnlyInputParametersAllowedInFunctions", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002AD34 File Offset: 0x00028F34
		internal static string EdmModel_Validator_Semantic_InvalidFunctionImportParameterMode(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidFunctionImportParameterMode", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002AD5C File Offset: 0x00028F5C
		internal static string EdmModel_Validator_Semantic_FunctionImportParameterIncorrectType(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_FunctionImportParameterIncorrectType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0002AD83 File Offset: 0x00028F83
		internal static string EdmModel_Validator_Semantic_RowTypeMustHaveProperties
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_RowTypeMustHaveProperties");
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002AD90 File Offset: 0x00028F90
		internal static string EdmModel_Validator_Semantic_ComplexTypeMustHaveProperties(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ComplexTypeMustHaveProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002ADB4 File Offset: 0x00028FB4
		internal static string EdmModel_Validator_Semantic_DuplicateDependentProperty(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateDependentProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0002ADDB File Offset: 0x00028FDB
		internal static string EdmModel_Validator_Semantic_ScaleOutOfRange
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_ScaleOutOfRange");
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0002ADE7 File Offset: 0x00028FE7
		internal static string EdmModel_Validator_Semantic_PrecisionOutOfRange
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_PrecisionOutOfRange");
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x0002ADF3 File Offset: 0x00028FF3
		internal static string EdmModel_Validator_Semantic_StringMaxLengthOutOfRange
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_StringMaxLengthOutOfRange");
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0002ADFF File Offset: 0x00028FFF
		internal static string EdmModel_Validator_Semantic_MaxLengthOutOfRange
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_MaxLengthOutOfRange");
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002AE0C File Offset: 0x0002900C
		internal static string EdmModel_Validator_Semantic_InvalidPropertyTypeConcurrencyMode(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidPropertyTypeConcurrencyMode", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002AE30 File Offset: 0x00029030
		internal static string EdmModel_Validator_Semantic_EntityKeyMustNotBeBinaryBeforeV2(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntityKeyMustNotBeBinaryBeforeV2", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0002AE57 File Offset: 0x00029057
		internal static string EdmModel_Validator_Semantic_EnumsNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_EnumsNotSupportedBeforeV3");
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002AE64 File Offset: 0x00029064
		internal static string EdmModel_Validator_Semantic_EnumMemberTypeMustMatchEnumUnderlyingType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EnumMemberTypeMustMatchEnumUnderlyingType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002AE88 File Offset: 0x00029088
		internal static string EdmModel_Validator_Semantic_EnumMemberNameAlreadyDefined(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EnumMemberNameAlreadyDefined", new object[]
			{
				p0
			});
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0002AEAB File Offset: 0x000290AB
		internal static string EdmModel_Validator_Semantic_ValueTermsNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_ValueTermsNotSupportedBeforeV3");
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0002AEB7 File Offset: 0x000290B7
		internal static string EdmModel_Validator_Semantic_VocabularyAnnotationsNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_VocabularyAnnotationsNotSupportedBeforeV3");
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0002AEC3 File Offset: 0x000290C3
		internal static string EdmModel_Validator_Semantic_OpenTypesSupportedOnlyInV12AndAfterV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_OpenTypesSupportedOnlyInV12AndAfterV3");
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0002AECF File Offset: 0x000290CF
		internal static string EdmModel_Validator_Semantic_OpenTypesSupportedForEntityTypesOnly
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_OpenTypesSupportedForEntityTypesOnly");
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0002AEDB File Offset: 0x000290DB
		internal static string EdmModel_Validator_Semantic_IsUnboundedCannotBeTrueWhileMaxLengthIsNotNull
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_IsUnboundedCannotBeTrueWhileMaxLengthIsNotNull");
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0002AEE7 File Offset: 0x000290E7
		internal static string EdmModel_Validator_Semantic_InvalidElementAnnotationMismatchedTerm
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidElementAnnotationMismatchedTerm");
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0002AEF3 File Offset: 0x000290F3
		internal static string EdmModel_Validator_Semantic_InvalidElementAnnotationValueInvalidXml
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidElementAnnotationValueInvalidXml");
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0002AEFF File Offset: 0x000290FF
		internal static string EdmModel_Validator_Semantic_InvalidElementAnnotationNotIEdmStringValue
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidElementAnnotationNotIEdmStringValue");
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0002AF0B File Offset: 0x0002910B
		internal static string EdmModel_Validator_Semantic_InvalidElementAnnotationNullNamespaceOrName
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_InvalidElementAnnotationNullNamespaceOrName");
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002AF18 File Offset: 0x00029118
		internal static string EdmModel_Validator_Semantic_CannotAssertNullableTypeAsNonNullableType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_CannotAssertNullableTypeAsNonNullableType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002AF3C File Offset: 0x0002913C
		internal static string EdmModel_Validator_Semantic_ExpressionPrimitiveKindCannotPromoteToAssertedType(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ExpressionPrimitiveKindCannotPromoteToAssertedType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0002AF63 File Offset: 0x00029163
		internal static string EdmModel_Validator_Semantic_NullCannotBeAssertedToBeANonNullableType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_NullCannotBeAssertedToBeANonNullableType");
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0002AF6F File Offset: 0x0002916F
		internal static string EdmModel_Validator_Semantic_ExpressionNotValidForTheAssertedType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_ExpressionNotValidForTheAssertedType");
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x0002AF7B File Offset: 0x0002917B
		internal static string EdmModel_Validator_Semantic_CollectionExpressionNotValidForNonCollectionType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_CollectionExpressionNotValidForNonCollectionType");
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0002AF87 File Offset: 0x00029187
		internal static string EdmModel_Validator_Semantic_PrimitiveConstantExpressionNotValidForNonPrimitiveType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_PrimitiveConstantExpressionNotValidForNonPrimitiveType");
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0002AF93 File Offset: 0x00029193
		internal static string EdmModel_Validator_Semantic_RecordExpressionNotValidForNonStructuredType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_RecordExpressionNotValidForNonStructuredType");
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0002AFA0 File Offset: 0x000291A0
		internal static string EdmModel_Validator_Semantic_RecordExpressionMissingProperty(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_RecordExpressionMissingProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0002AFC4 File Offset: 0x000291C4
		internal static string EdmModel_Validator_Semantic_RecordExpressionHasExtraProperties(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_RecordExpressionHasExtraProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0002AFE8 File Offset: 0x000291E8
		internal static string EdmModel_Validator_Semantic_DuplicateAnnotation(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateAnnotation", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0002B014 File Offset: 0x00029214
		internal static string EdmModel_Validator_Semantic_IncorrectNumberOfArguments(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_IncorrectNumberOfArguments", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x0002B03F File Offset: 0x0002923F
		internal static string EdmModel_Validator_Semantic_StreamTypeReferencesNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_StreamTypeReferencesNotSupportedBeforeV3");
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002B04B File Offset: 0x0002924B
		internal static string EdmModel_Validator_Semantic_SpatialTypeReferencesNotSupportedBeforeV3
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_SpatialTypeReferencesNotSupportedBeforeV3");
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0002B058 File Offset: 0x00029258
		internal static string EdmModel_Validator_Semantic_DuplicateEntityContainerName(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateEntityContainerName", new object[]
			{
				p0
			});
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0002B07B File Offset: 0x0002927B
		internal static string EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_ExpressionPrimitiveKindNotValidForAssertedType");
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0002B087 File Offset: 0x00029287
		internal static string EdmModel_Validator_Semantic_IntegerConstantValueOutOfRange
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_IntegerConstantValueOutOfRange");
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0002B094 File Offset: 0x00029294
		internal static string EdmModel_Validator_Semantic_StringConstantLengthOutOfRange(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_StringConstantLengthOutOfRange", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002B0BC File Offset: 0x000292BC
		internal static string EdmModel_Validator_Semantic_BinaryConstantLengthOutOfRange(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_BinaryConstantLengthOutOfRange", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0002B0E3 File Offset: 0x000292E3
		internal static string EdmModel_Validator_Semantic_TypeMustNotHaveKindOfNone
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Semantic_TypeMustNotHaveKindOfNone");
			}
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0002B0F0 File Offset: 0x000292F0
		internal static string EdmModel_Validator_Semantic_TermMustNotHaveKindOfNone(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_TermMustNotHaveKindOfNone", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0002B114 File Offset: 0x00029314
		internal static string EdmModel_Validator_Semantic_SchemaElementMustNotHaveKindOfNone(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_SchemaElementMustNotHaveKindOfNone", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002B138 File Offset: 0x00029338
		internal static string EdmModel_Validator_Semantic_PropertyMustNotHaveKindOfNone(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_PropertyMustNotHaveKindOfNone", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0002B15C File Offset: 0x0002935C
		internal static string EdmModel_Validator_Semantic_PrimitiveTypeMustNotHaveKindOfNone(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_PrimitiveTypeMustNotHaveKindOfNone", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0002B180 File Offset: 0x00029380
		internal static string EdmModel_Validator_Semantic_EntityContainerElementMustNotHaveKindOfNone(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntityContainerElementMustNotHaveKindOfNone", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0002B1A4 File Offset: 0x000293A4
		internal static string EdmModel_Validator_Semantic_DuplicateNavigationPropertyMapping(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_DuplicateNavigationPropertyMapping", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0002B1CC File Offset: 0x000293CC
		internal static string EdmModel_Validator_Semantic_EntitySetNavigationMappingMustBeBidirectional(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntitySetNavigationMappingMustBeBidirectional", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0002B1F4 File Offset: 0x000293F4
		internal static string EdmModel_Validator_Semantic_EntitySetCanOnlyBeContainedByASingleNavigationProperty(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntitySetCanOnlyBeContainedByASingleNavigationProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0002B218 File Offset: 0x00029418
		internal static string EdmModel_Validator_Semantic_TypeAnnotationMissingRequiredProperty(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_TypeAnnotationMissingRequiredProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002B23C File Offset: 0x0002943C
		internal static string EdmModel_Validator_Semantic_TypeAnnotationHasExtraProperties(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_TypeAnnotationHasExtraProperties", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0002B260 File Offset: 0x00029460
		internal static string EdmModel_Validator_Semantic_EnumMustHaveIntegralUnderlyingType(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EnumMustHaveIntegralUnderlyingType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0002B284 File Offset: 0x00029484
		internal static string EdmModel_Validator_Semantic_InaccessibleTerm(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InaccessibleTerm", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0002B2A8 File Offset: 0x000294A8
		internal static string EdmModel_Validator_Semantic_InaccessibleTarget(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_InaccessibleTarget", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0002B2CC File Offset: 0x000294CC
		internal static string EdmModel_Validator_Semantic_ElementDirectValueAnnotationFullNameMustBeUnique(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_ElementDirectValueAnnotationFullNameMustBeUnique", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0002B2F4 File Offset: 0x000294F4
		internal static string EdmModel_Validator_Semantic_NoEntitySetsFoundForType(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NoEntitySetsFoundForType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0002B320 File Offset: 0x00029520
		internal static string EdmModel_Validator_Semantic_CannotInferEntitySetWithMultipleSetsPerType(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_CannotInferEntitySetWithMultipleSetsPerType", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0002B34C File Offset: 0x0002954C
		internal static string EdmModel_Validator_Semantic_EntitySetRecursiveNavigationPropertyMappingsMustPointBackToSourceEntitySet(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntitySetRecursiveNavigationPropertyMappingsMustPointBackToSourceEntitySet", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0002B374 File Offset: 0x00029574
		internal static string EdmModel_Validator_Semantic_NavigationPropertyEntityMustNotIndirectlyContainItself(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_NavigationPropertyEntityMustNotIndirectlyContainItself", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0002B398 File Offset: 0x00029598
		internal static string EdmModel_Validator_Semantic_PathIsNotValidForTheGivenContext(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_PathIsNotValidForTheGivenContext", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0002B3BC File Offset: 0x000295BC
		internal static string EdmModel_Validator_Semantic_EntitySetNavigationPropertyMappingMustPointToValidTargetForProperty(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Semantic_EntitySetNavigationPropertyMappingMustPointToValidTargetForProperty", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0002B3E3 File Offset: 0x000295E3
		internal static string EdmModel_Validator_Syntactic_MissingName
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Syntactic_MissingName");
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002B3F0 File Offset: 0x000295F0
		internal static string EdmModel_Validator_Syntactic_EdmModel_NameIsTooLong(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EdmModel_NameIsTooLong", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002B414 File Offset: 0x00029614
		internal static string EdmModel_Validator_Syntactic_EdmModel_NameIsNotAllowed(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EdmModel_NameIsNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x0002B437 File Offset: 0x00029637
		internal static string EdmModel_Validator_Syntactic_MissingNamespaceName
		{
			get
			{
				return EntityRes.GetString("EdmModel_Validator_Syntactic_MissingNamespaceName");
			}
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002B444 File Offset: 0x00029644
		internal static string EdmModel_Validator_Syntactic_EdmModel_NamespaceNameIsTooLong(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EdmModel_NamespaceNameIsTooLong", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002B468 File Offset: 0x00029668
		internal static string EdmModel_Validator_Syntactic_EdmModel_NamespaceNameIsNotAllowed(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EdmModel_NamespaceNameIsNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002B48C File Offset: 0x0002968C
		internal static string EdmModel_Validator_Syntactic_PropertyMustNotBeNull(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_PropertyMustNotBeNull", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002B4B4 File Offset: 0x000296B4
		internal static string EdmModel_Validator_Syntactic_EnumPropertyValueOutOfRange(object p0, object p1, object p2, object p3)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EnumPropertyValueOutOfRange", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002B4E4 File Offset: 0x000296E4
		internal static string EdmModel_Validator_Syntactic_InterfaceKindValueMismatch(object p0, object p1, object p2, object p3)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_InterfaceKindValueMismatch", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002B514 File Offset: 0x00029714
		internal static string EdmModel_Validator_Syntactic_TypeRefInterfaceTypeKindValueMismatch(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_TypeRefInterfaceTypeKindValueMismatch", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002B53C File Offset: 0x0002973C
		internal static string EdmModel_Validator_Syntactic_InterfaceKindValueUnexpected(object p0, object p1, object p2)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_InterfaceKindValueUnexpected", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0002B568 File Offset: 0x00029768
		internal static string EdmModel_Validator_Syntactic_EnumerableMustNotHaveNullElements(object p0, object p1)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_EnumerableMustNotHaveNullElements", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002B590 File Offset: 0x00029790
		internal static string EdmModel_Validator_Syntactic_NavigationPartnerInvalid(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_NavigationPartnerInvalid", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002B5B4 File Offset: 0x000297B4
		internal static string EdmModel_Validator_Syntactic_InterfaceCriticalCycleInTypeHierarchy(object p0)
		{
			return EntityRes.GetString("EdmModel_Validator_Syntactic_InterfaceCriticalCycleInTypeHierarchy", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x0002B5D7 File Offset: 0x000297D7
		internal static string Serializer_SingleFileExpected
		{
			get
			{
				return EntityRes.GetString("Serializer_SingleFileExpected");
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0002B5E3 File Offset: 0x000297E3
		internal static string Serializer_UnknownEdmVersion
		{
			get
			{
				return EntityRes.GetString("Serializer_UnknownEdmVersion");
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x0002B5EF File Offset: 0x000297EF
		internal static string Serializer_UnknownEdmxVersion
		{
			get
			{
				return EntityRes.GetString("Serializer_UnknownEdmxVersion");
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002B5FC File Offset: 0x000297FC
		internal static string Serializer_NonInlineFunctionImportReturnType(object p0)
		{
			return EntityRes.GetString("Serializer_NonInlineFunctionImportReturnType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002B620 File Offset: 0x00029820
		internal static string Serializer_ReferencedTypeMustHaveValidName(object p0)
		{
			return EntityRes.GetString("Serializer_ReferencedTypeMustHaveValidName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002B644 File Offset: 0x00029844
		internal static string Serializer_OutOfLineAnnotationTargetMustHaveValidName(object p0)
		{
			return EntityRes.GetString("Serializer_OutOfLineAnnotationTargetMustHaveValidName", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0002B667 File Offset: 0x00029867
		internal static string Serializer_NoSchemasProduced
		{
			get
			{
				return EntityRes.GetString("Serializer_NoSchemasProduced");
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002B674 File Offset: 0x00029874
		internal static string XmlParser_EmptyFile(object p0)
		{
			return EntityRes.GetString("XmlParser_EmptyFile", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x0002B697 File Offset: 0x00029897
		internal static string XmlParser_EmptySchemaTextReader
		{
			get
			{
				return EntityRes.GetString("XmlParser_EmptySchemaTextReader");
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002B6A4 File Offset: 0x000298A4
		internal static string XmlParser_MissingAttribute(object p0, object p1)
		{
			return EntityRes.GetString("XmlParser_MissingAttribute", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0002B6CC File Offset: 0x000298CC
		internal static string XmlParser_TextNotAllowed(object p0)
		{
			return EntityRes.GetString("XmlParser_TextNotAllowed", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002B6F0 File Offset: 0x000298F0
		internal static string XmlParser_UnexpectedAttribute(object p0)
		{
			return EntityRes.GetString("XmlParser_UnexpectedAttribute", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002B714 File Offset: 0x00029914
		internal static string XmlParser_UnexpectedElement(object p0)
		{
			return EntityRes.GetString("XmlParser_UnexpectedElement", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002B738 File Offset: 0x00029938
		internal static string XmlParser_UnusedElement(object p0)
		{
			return EntityRes.GetString("XmlParser_UnusedElement", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002B75C File Offset: 0x0002995C
		internal static string XmlParser_UnexpectedNodeType(object p0)
		{
			return EntityRes.GetString("XmlParser_UnexpectedNodeType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002B780 File Offset: 0x00029980
		internal static string XmlParser_UnexpectedRootElement(object p0, object p1)
		{
			return EntityRes.GetString("XmlParser_UnexpectedRootElement", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002B7A8 File Offset: 0x000299A8
		internal static string XmlParser_UnexpectedRootElementWrongNamespace(object p0, object p1)
		{
			return EntityRes.GetString("XmlParser_UnexpectedRootElementWrongNamespace", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002B7D0 File Offset: 0x000299D0
		internal static string XmlParser_UnexpectedRootElementNoNamespace(object p0)
		{
			return EntityRes.GetString("XmlParser_UnexpectedRootElementNoNamespace", new object[]
			{
				p0
			});
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002B7F4 File Offset: 0x000299F4
		internal static string CsdlParser_InvalidAlias(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidAlias", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0002B817 File Offset: 0x00029A17
		internal static string CsdlParser_AssociationHasAtMostOneConstraint
		{
			get
			{
				return EntityRes.GetString("CsdlParser_AssociationHasAtMostOneConstraint");
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002B824 File Offset: 0x00029A24
		internal static string CsdlParser_InvalidDeleteAction(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidDeleteAction", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0002B847 File Offset: 0x00029A47
		internal static string CsdlParser_MissingTypeAttributeOrElement
		{
			get
			{
				return EntityRes.GetString("CsdlParser_MissingTypeAttributeOrElement");
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002B854 File Offset: 0x00029A54
		internal static string CsdlParser_InvalidAssociationIncorrectNumberOfEnds(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidAssociationIncorrectNumberOfEnds", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002B878 File Offset: 0x00029A78
		internal static string CsdlParser_InvalidAssociationSetIncorrectNumberOfEnds(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidAssociationSetIncorrectNumberOfEnds", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002B89C File Offset: 0x00029A9C
		internal static string CsdlParser_InvalidConcurrencyMode(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidConcurrencyMode", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002B8C0 File Offset: 0x00029AC0
		internal static string CsdlParser_InvalidParameterMode(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidParameterMode", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002B8E4 File Offset: 0x00029AE4
		internal static string CsdlParser_InvalidEndRoleInRelationshipConstraint(object p0, object p1)
		{
			return EntityRes.GetString("CsdlParser_InvalidEndRoleInRelationshipConstraint", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002B90C File Offset: 0x00029B0C
		internal static string CsdlParser_InvalidMultiplicity(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidMultiplicity", new object[]
			{
				p0
			});
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0002B92F File Offset: 0x00029B2F
		internal static string CsdlParser_ReferentialConstraintRequiresOneDependent
		{
			get
			{
				return EntityRes.GetString("CsdlParser_ReferentialConstraintRequiresOneDependent");
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0002B93B File Offset: 0x00029B3B
		internal static string CsdlParser_ReferentialConstraintRequiresOnePrincipal
		{
			get
			{
				return EntityRes.GetString("CsdlParser_ReferentialConstraintRequiresOnePrincipal");
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0002B947 File Offset: 0x00029B47
		internal static string CsdlParser_InvalidIfExpressionIncorrectNumberOfOperands
		{
			get
			{
				return EntityRes.GetString("CsdlParser_InvalidIfExpressionIncorrectNumberOfOperands");
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0002B953 File Offset: 0x00029B53
		internal static string CsdlParser_InvalidIsTypeExpressionIncorrectNumberOfOperands
		{
			get
			{
				return EntityRes.GetString("CsdlParser_InvalidIsTypeExpressionIncorrectNumberOfOperands");
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0002B95F File Offset: 0x00029B5F
		internal static string CsdlParser_InvalidAssertTypeExpressionIncorrectNumberOfOperands
		{
			get
			{
				return EntityRes.GetString("CsdlParser_InvalidAssertTypeExpressionIncorrectNumberOfOperands");
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0002B96B File Offset: 0x00029B6B
		internal static string CsdlParser_InvalidLabeledElementExpressionIncorrectNumberOfOperands
		{
			get
			{
				return EntityRes.GetString("CsdlParser_InvalidLabeledElementExpressionIncorrectNumberOfOperands");
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0002B978 File Offset: 0x00029B78
		internal static string CsdlParser_InvalidTypeName(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0002B99C File Offset: 0x00029B9C
		internal static string CsdlParser_InvalidQualifiedName(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidQualifiedName", new object[]
			{
				p0
			});
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x0002B9BF File Offset: 0x00029BBF
		internal static string CsdlParser_NoReadersProvided
		{
			get
			{
				return EntityRes.GetString("CsdlParser_NoReadersProvided");
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002B9CB File Offset: 0x00029BCB
		internal static string CsdlParser_NullXmlReader
		{
			get
			{
				return EntityRes.GetString("CsdlParser_NullXmlReader");
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0002B9D8 File Offset: 0x00029BD8
		internal static string CsdlParser_InvalidEntitySetPath(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidEntitySetPath", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0002B9FC File Offset: 0x00029BFC
		internal static string CsdlParser_InvalidEnumMemberPath(object p0)
		{
			return EntityRes.GetString("CsdlParser_InvalidEnumMemberPath", new object[]
			{
				p0
			});
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06000E13 RID: 3603 RVA: 0x0002BA1F File Offset: 0x00029C1F
		internal static string CsdlSemantics_ReferentialConstraintMismatch
		{
			get
			{
				return EntityRes.GetString("CsdlSemantics_ReferentialConstraintMismatch");
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0002BA2B File Offset: 0x00029C2B
		internal static string CsdlSemantics_EnumMemberValueOutOfRange
		{
			get
			{
				return EntityRes.GetString("CsdlSemantics_EnumMemberValueOutOfRange");
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002BA38 File Offset: 0x00029C38
		internal static string CsdlSemantics_ImpossibleAnnotationsTarget(object p0)
		{
			return EntityRes.GetString("CsdlSemantics_ImpossibleAnnotationsTarget", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0002BA5C File Offset: 0x00029C5C
		internal static string CsdlSemantics_DuplicateAlias(object p0, object p1)
		{
			return EntityRes.GetString("CsdlSemantics_DuplicateAlias", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0002BA83 File Offset: 0x00029C83
		internal static string EdmxParser_EdmxVersionMismatch
		{
			get
			{
				return EntityRes.GetString("EdmxParser_EdmxVersionMismatch");
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0002BA8F File Offset: 0x00029C8F
		internal static string EdmxParser_EdmxDataServiceVersionInvalid
		{
			get
			{
				return EntityRes.GetString("EdmxParser_EdmxDataServiceVersionInvalid");
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0002BA9B File Offset: 0x00029C9B
		internal static string EdmxParser_EdmxMaxDataServiceVersionInvalid
		{
			get
			{
				return EntityRes.GetString("EdmxParser_EdmxMaxDataServiceVersionInvalid");
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0002BAA8 File Offset: 0x00029CA8
		internal static string EdmxParser_BodyElement(object p0)
		{
			return EntityRes.GetString("EdmxParser_BodyElement", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0002BACC File Offset: 0x00029CCC
		internal static string EdmParseException_ErrorsEncounteredInEdmx(object p0)
		{
			return EntityRes.GetString("EdmParseException_ErrorsEncounteredInEdmx", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0002BAF0 File Offset: 0x00029CF0
		internal static string ValueParser_InvalidBoolean(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidBoolean", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0002BB14 File Offset: 0x00029D14
		internal static string ValueParser_InvalidInteger(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidInteger", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0002BB38 File Offset: 0x00029D38
		internal static string ValueParser_InvalidLong(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidLong", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0002BB5C File Offset: 0x00029D5C
		internal static string ValueParser_InvalidFloatingPoint(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidFloatingPoint", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002BB80 File Offset: 0x00029D80
		internal static string ValueParser_InvalidMaxLength(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidMaxLength", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0002BBA4 File Offset: 0x00029DA4
		internal static string ValueParser_InvalidSrid(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidSrid", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0002BBC8 File Offset: 0x00029DC8
		internal static string ValueParser_InvalidGuid(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidGuid", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0002BBEC File Offset: 0x00029DEC
		internal static string ValueParser_InvalidDecimal(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidDecimal", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0002BC10 File Offset: 0x00029E10
		internal static string ValueParser_InvalidDateTimeOffset(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidDateTimeOffset", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0002BC34 File Offset: 0x00029E34
		internal static string ValueParser_InvalidDateTime(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidDateTime", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0002BC58 File Offset: 0x00029E58
		internal static string ValueParser_InvalidTime(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidTime", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0002BC7C File Offset: 0x00029E7C
		internal static string ValueParser_InvalidBinary(object p0)
		{
			return EntityRes.GetString("ValueParser_InvalidBinary", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0002BCA0 File Offset: 0x00029EA0
		internal static string UnknownEnumVal_Multiplicity(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_Multiplicity", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0002BCC4 File Offset: 0x00029EC4
		internal static string UnknownEnumVal_SchemaElementKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_SchemaElementKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0002BCE8 File Offset: 0x00029EE8
		internal static string UnknownEnumVal_TypeKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_TypeKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0002BD0C File Offset: 0x00029F0C
		internal static string UnknownEnumVal_PrimitiveKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_PrimitiveKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0002BD30 File Offset: 0x00029F30
		internal static string UnknownEnumVal_ContainerElementKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_ContainerElementKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0002BD54 File Offset: 0x00029F54
		internal static string UnknownEnumVal_EdmxTarget(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_EdmxTarget", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0002BD78 File Offset: 0x00029F78
		internal static string UnknownEnumVal_FunctionParameterMode(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_FunctionParameterMode", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0002BD9C File Offset: 0x00029F9C
		internal static string UnknownEnumVal_ConcurrencyMode(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_ConcurrencyMode", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0002BDC0 File Offset: 0x00029FC0
		internal static string UnknownEnumVal_PropertyKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_PropertyKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0002BDE4 File Offset: 0x00029FE4
		internal static string UnknownEnumVal_TermKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_TermKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0002BE08 File Offset: 0x0002A008
		internal static string UnknownEnumVal_ExpressionKind(object p0)
		{
			return EntityRes.GetString("UnknownEnumVal_ExpressionKind", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0002BE2C File Offset: 0x0002A02C
		internal static string Bad_AmbiguousElementBinding(object p0)
		{
			return EntityRes.GetString("Bad_AmbiguousElementBinding", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0002BE50 File Offset: 0x0002A050
		internal static string Bad_UnresolvedType(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0002BE74 File Offset: 0x0002A074
		internal static string Bad_UnresolvedComplexType(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedComplexType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0002BE98 File Offset: 0x0002A098
		internal static string Bad_UnresolvedEntityType(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedEntityType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0002BEBC File Offset: 0x0002A0BC
		internal static string Bad_UnresolvedPrimitiveType(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedPrimitiveType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0002BEE0 File Offset: 0x0002A0E0
		internal static string Bad_UnresolvedFunction(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedFunction", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0002BF04 File Offset: 0x0002A104
		internal static string Bad_AmbiguousFunction(object p0)
		{
			return EntityRes.GetString("Bad_AmbiguousFunction", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0002BF28 File Offset: 0x0002A128
		internal static string Bad_FunctionParametersDontMatch(object p0)
		{
			return EntityRes.GetString("Bad_FunctionParametersDontMatch", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0002BF4C File Offset: 0x0002A14C
		internal static string Bad_UnresolvedEntitySet(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedEntitySet", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002BF70 File Offset: 0x0002A170
		internal static string Bad_UnresolvedEntityContainer(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedEntityContainer", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0002BF94 File Offset: 0x0002A194
		internal static string Bad_UnresolvedEnumType(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedEnumType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		internal static string Bad_UnresolvedEnumMember(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedEnumMember", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0002BFDC File Offset: 0x0002A1DC
		internal static string Bad_UnresolvedProperty(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedProperty", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0002C000 File Offset: 0x0002A200
		internal static string Bad_UnresolvedParameter(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedParameter", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0002C024 File Offset: 0x0002A224
		internal static string Bad_UnresolvedLabeledElement(object p0)
		{
			return EntityRes.GetString("Bad_UnresolvedLabeledElement", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0002C048 File Offset: 0x0002A248
		internal static string Bad_CyclicEntity(object p0)
		{
			return EntityRes.GetString("Bad_CyclicEntity", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0002C06C File Offset: 0x0002A26C
		internal static string Bad_CyclicComplex(object p0)
		{
			return EntityRes.GetString("Bad_CyclicComplex", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0002C090 File Offset: 0x0002A290
		internal static string Bad_CyclicEntityContainer(object p0)
		{
			return EntityRes.GetString("Bad_CyclicEntityContainer", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0002C0B4 File Offset: 0x0002A2B4
		internal static string Bad_UncomputableAssociationEnd(object p0)
		{
			return EntityRes.GetString("Bad_UncomputableAssociationEnd", new object[]
			{
				p0
			});
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0002C0D7 File Offset: 0x0002A2D7
		internal static string RuleSet_DuplicateRulesExistInRuleSet
		{
			get
			{
				return EntityRes.GetString("RuleSet_DuplicateRulesExistInRuleSet");
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
		internal static string EdmToClr_UnsupportedTypeCode(object p0)
		{
			return EntityRes.GetString("EdmToClr_UnsupportedTypeCode", new object[]
			{
				p0
			});
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0002C107 File Offset: 0x0002A307
		internal static string EdmToClr_StructuredValueMappedToNonClass
		{
			get
			{
				return EntityRes.GetString("EdmToClr_StructuredValueMappedToNonClass");
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0002C114 File Offset: 0x0002A314
		internal static string EdmToClr_IEnumerableOfTPropertyAlreadyHasValue(object p0, object p1)
		{
			return EntityRes.GetString("EdmToClr_IEnumerableOfTPropertyAlreadyHasValue", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0002C13C File Offset: 0x0002A33C
		internal static string EdmToClr_StructuredPropertyDuplicateValue(object p0)
		{
			return EntityRes.GetString("EdmToClr_StructuredPropertyDuplicateValue", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002C160 File Offset: 0x0002A360
		internal static string EdmToClr_CannotConvertEdmValueToClrType(object p0, object p1)
		{
			return EntityRes.GetString("EdmToClr_CannotConvertEdmValueToClrType", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002C188 File Offset: 0x0002A388
		internal static string EdmToClr_CannotConvertEdmCollectionValueToClrType(object p0)
		{
			return EntityRes.GetString("EdmToClr_CannotConvertEdmCollectionValueToClrType", new object[]
			{
				p0
			});
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002C1AC File Offset: 0x0002A3AC
		internal static string EdmToClr_TryCreateObjectInstanceReturnedWrongObject(object p0, object p1)
		{
			return EntityRes.GetString("EdmToClr_TryCreateObjectInstanceReturnedWrongObject", new object[]
			{
				p0,
				p1
			});
		}
	}
}
