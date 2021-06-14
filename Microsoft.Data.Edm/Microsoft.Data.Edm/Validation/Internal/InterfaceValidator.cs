using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Validation.Internal
{
	// Token: 0x020001EF RID: 495
	internal class InterfaceValidator
	{
		// Token: 0x06000BE4 RID: 3044 RVA: 0x00022980 File Offset: 0x00020B80
		private InterfaceValidator(HashSetInternal<object> skipVisitation, IEdmModel model, bool validateDirectValueAnnotations)
		{
			this.skipVisitation = skipVisitation;
			this.model = model;
			this.validateDirectValueAnnotations = validateDirectValueAnnotations;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x000229F0 File Offset: 0x00020BF0
		public static IEnumerable<EdmError> ValidateModelStructureAndSemantics(IEdmModel model, ValidationRuleSet semanticRuleSet)
		{
			InterfaceValidator modelValidator = new InterfaceValidator(null, model, true);
			List<EdmError> list = new List<EdmError>(modelValidator.ValidateStructure(model));
			InterfaceValidator referencesValidator = new InterfaceValidator(modelValidator.visited, model, false);
			IEnumerable<object> enumerable = modelValidator.danglingReferences;
			while (enumerable.FirstOrDefault<object>() != null)
			{
				foreach (object item2 in enumerable)
				{
					list.AddRange(referencesValidator.ValidateStructure(item2));
				}
				enumerable = referencesValidator.danglingReferences.ToArray<object>();
			}
			if (list.Any(new Func<EdmError, bool>(ValidationHelper.IsInterfaceCritical)))
			{
				return list;
			}
			ValidationContext validationContext = new ValidationContext(model, (object item) => modelValidator.visitedBad.Contains(item) || referencesValidator.visitedBad.Contains(item));
			Dictionary<Type, List<ValidationRule>> concreteTypeSemanticInterfaceVisitors = new Dictionary<Type, List<ValidationRule>>();
			foreach (object obj in modelValidator.visited)
			{
				if (!modelValidator.visitedBad.Contains(obj))
				{
					foreach (ValidationRule validationRule in InterfaceValidator.GetSemanticInterfaceVisitorsForObject(obj.GetType(), semanticRuleSet, concreteTypeSemanticInterfaceVisitors))
					{
						validationRule.Evaluate(validationContext, obj);
					}
				}
			}
			list.AddRange(validationContext.Errors);
			return list;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00022B98 File Offset: 0x00020D98
		public static IEnumerable<EdmError> GetStructuralErrors(IEdmElement item)
		{
			IEdmModel edmModel = item as IEdmModel;
			InterfaceValidator interfaceValidator = new InterfaceValidator(null, edmModel, edmModel != null);
			return interfaceValidator.ValidateStructure(item);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00022BC4 File Offset: 0x00020DC4
		private static Dictionary<Type, InterfaceValidator.VisitorBase> CreateInterfaceVisitorsMap()
		{
			Dictionary<Type, InterfaceValidator.VisitorBase> dictionary = new Dictionary<Type, InterfaceValidator.VisitorBase>();
			foreach (Type type in typeof(InterfaceValidator).GetNonPublicNestedTypes())
			{
				if (type.IsClass())
				{
					Type baseType = type.GetBaseType();
					if (baseType.IsGenericType() && baseType.GetBaseType() == typeof(InterfaceValidator.VisitorBase))
					{
						dictionary.Add(baseType.GetGenericArguments()[0], (InterfaceValidator.VisitorBase)Activator.CreateInstance(type));
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00022C64 File Offset: 0x00020E64
		private static IEnumerable<InterfaceValidator.VisitorBase> ComputeInterfaceVisitorsForObject(Type objectType)
		{
			List<InterfaceValidator.VisitorBase> list = new List<InterfaceValidator.VisitorBase>();
			foreach (Type key in objectType.GetInterfaces())
			{
				InterfaceValidator.VisitorBase item;
				if (InterfaceValidator.InterfaceVisitors.TryGetValue(key, out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00022CAC File Offset: 0x00020EAC
		private static EdmError CreatePropertyMustNotBeNullError<T>(T item, string propertyName)
		{
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalPropertyValueMustNotBeNull, Strings.EdmModel_Validator_Syntactic_PropertyMustNotBeNull(typeof(T).Name, propertyName));
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00022CD5 File Offset: 0x00020ED5
		private static EdmError CreateEnumPropertyOutOfRangeError<T, E>(T item, E enumValue, string propertyName)
		{
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalEnumPropertyValueOutOfRange, Strings.EdmModel_Validator_Syntactic_EnumPropertyValueOutOfRange(typeof(T).Name, propertyName, typeof(E).Name, enumValue));
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00022D14 File Offset: 0x00020F14
		private static EdmError CheckForInterfaceKindValueMismatchError<T, K, I>(T item, K kind, string propertyName)
		{
			if (item is I)
			{
				return null;
			}
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalKindValueMismatch, Strings.EdmModel_Validator_Syntactic_InterfaceKindValueMismatch(kind, typeof(T).Name, propertyName, typeof(I).Name));
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00022D6C File Offset: 0x00020F6C
		private static EdmError CreateInterfaceKindValueUnexpectedError<T, K>(T item, K kind, string propertyName)
		{
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalKindValueUnexpected, Strings.EdmModel_Validator_Syntactic_InterfaceKindValueUnexpected(kind, typeof(T).Name, propertyName));
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00022D9B File Offset: 0x00020F9B
		private static EdmError CreateTypeRefInterfaceTypeKindValueMismatchError<T>(T item) where T : IEdmTypeReference
		{
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalKindValueMismatch, Strings.EdmModel_Validator_Syntactic_TypeRefInterfaceTypeKindValueMismatch(typeof(T).Name, item.Definition.TypeKind));
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00022DDC File Offset: 0x00020FDC
		private static EdmError CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<T>(T item) where T : IEdmPrimitiveTypeReference
		{
			return new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalKindValueMismatch, Strings.EdmModel_Validator_Syntactic_TypeRefInterfaceTypeKindValueMismatch(typeof(T).Name, ((IEdmPrimitiveType)item.Definition).PrimitiveKind));
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00022E2C File Offset: 0x0002102C
		private static void ProcessEnumerable<T, E>(T item, IEnumerable<E> enumerable, string propertyName, IList targetList, ref List<EdmError> errors)
		{
			if (enumerable == null)
			{
				InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<T>(item, propertyName), ref errors);
				return;
			}
			foreach (E e in enumerable)
			{
				if (e == null)
				{
					InterfaceValidator.CollectErrors(new EdmError(InterfaceValidator.GetLocation(item), EdmErrorCode.InterfaceCriticalEnumerableMustNotHaveNullElements, Strings.EdmModel_Validator_Syntactic_EnumerableMustNotHaveNullElements(typeof(T).Name, propertyName)), ref errors);
					break;
				}
				targetList.Add(e);
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00022EC8 File Offset: 0x000210C8
		private static void CollectErrors(EdmError newError, ref List<EdmError> errors)
		{
			if (newError != null)
			{
				if (errors == null)
				{
					errors = new List<EdmError>();
				}
				errors.Add(newError);
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00022EE0 File Offset: 0x000210E0
		private static bool IsCheckableBad(object element)
		{
			IEdmCheckable edmCheckable = element as IEdmCheckable;
			return edmCheckable != null && edmCheckable.Errors != null && edmCheckable.Errors.Count<EdmError>() > 0;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00022F10 File Offset: 0x00021110
		private static EdmLocation GetLocation(object item)
		{
			IEdmLocatable edmLocatable = item as IEdmLocatable;
			if (edmLocatable == null || edmLocatable.Location == null)
			{
				return new ObjectLocation(item);
			}
			return edmLocatable.Location;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00022F3C File Offset: 0x0002113C
		private static IEnumerable<ValidationRule> GetSemanticInterfaceVisitorsForObject(Type objectType, ValidationRuleSet ruleSet, Dictionary<Type, List<ValidationRule>> concreteTypeSemanticInterfaceVisitors)
		{
			List<ValidationRule> list;
			if (!concreteTypeSemanticInterfaceVisitors.TryGetValue(objectType, out list))
			{
				list = new List<ValidationRule>();
				foreach (Type t in objectType.GetInterfaces())
				{
					list.AddRange(ruleSet.GetRules(t));
				}
				concreteTypeSemanticInterfaceVisitors.Add(objectType, list);
			}
			return list;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00022F8C File Offset: 0x0002118C
		private IEnumerable<EdmError> ValidateStructure(object item)
		{
			if (item is IEdmValidCoreModelElement || this.visited.Contains(item) || (this.skipVisitation != null && this.skipVisitation.Contains(item)))
			{
				return Enumerable.Empty<EdmError>();
			}
			this.visited.Add(item);
			if (this.danglingReferences.Contains(item))
			{
				this.danglingReferences.Remove(item);
			}
			List<EdmError> list = null;
			List<object> list2 = new List<object>();
			List<object> list3 = new List<object>();
			IEnumerable<InterfaceValidator.VisitorBase> enumerable = InterfaceValidator.ConcreteTypeInterfaceVisitors.Evaluate(item.GetType());
			foreach (InterfaceValidator.VisitorBase visitorBase in enumerable)
			{
				IEnumerable<EdmError> enumerable2 = visitorBase.Visit(item, list2, list3);
				if (enumerable2 != null)
				{
					foreach (EdmError item2 in enumerable2)
					{
						if (list == null)
						{
							list = new List<EdmError>();
						}
						list.Add(item2);
					}
				}
			}
			if (list != null)
			{
				this.visitedBad.Add(item);
				return list;
			}
			List<EdmError> list4 = new List<EdmError>();
			if (this.validateDirectValueAnnotations)
			{
				IEdmElement edmElement = item as IEdmElement;
				if (edmElement != null)
				{
					foreach (IEdmDirectValueAnnotation item3 in this.model.DirectValueAnnotations(edmElement))
					{
						list4.AddRange(this.ValidateStructure(item3));
					}
				}
			}
			foreach (object item4 in list2)
			{
				list4.AddRange(this.ValidateStructure(item4));
			}
			foreach (object reference in list3)
			{
				this.CollectReference(reference);
			}
			return list4;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000231AC File Offset: 0x000213AC
		private void CollectReference(object reference)
		{
			if (!(reference is IEdmValidCoreModelElement) && !this.visited.Contains(reference) && (this.skipVisitation == null || !this.skipVisitation.Contains(reference)))
			{
				this.danglingReferences.Add(reference);
			}
		}

		// Token: 0x0400058F RID: 1423
		private static readonly Dictionary<Type, InterfaceValidator.VisitorBase> InterfaceVisitors = InterfaceValidator.CreateInterfaceVisitorsMap();

		// Token: 0x04000590 RID: 1424
		private static readonly Memoizer<Type, IEnumerable<InterfaceValidator.VisitorBase>> ConcreteTypeInterfaceVisitors = new Memoizer<Type, IEnumerable<InterfaceValidator.VisitorBase>>(new Func<Type, IEnumerable<InterfaceValidator.VisitorBase>>(InterfaceValidator.ComputeInterfaceVisitorsForObject), null);

		// Token: 0x04000591 RID: 1425
		private readonly HashSetInternal<object> visited = new HashSetInternal<object>();

		// Token: 0x04000592 RID: 1426
		private readonly HashSetInternal<object> visitedBad = new HashSetInternal<object>();

		// Token: 0x04000593 RID: 1427
		private readonly HashSetInternal<object> danglingReferences = new HashSetInternal<object>();

		// Token: 0x04000594 RID: 1428
		private readonly HashSetInternal<object> skipVisitation;

		// Token: 0x04000595 RID: 1429
		private readonly bool validateDirectValueAnnotations;

		// Token: 0x04000596 RID: 1430
		private readonly IEdmModel model;

		// Token: 0x020001F0 RID: 496
		private abstract class VisitorBase
		{
			// Token: 0x06000BF7 RID: 3063
			public abstract IEnumerable<EdmError> Visit(object item, List<object> followup, List<object> references);
		}

		// Token: 0x020001F1 RID: 497
		private abstract class VisitorOfT<T> : InterfaceValidator.VisitorBase
		{
			// Token: 0x06000BF9 RID: 3065 RVA: 0x00023212 File Offset: 0x00021412
			public override IEnumerable<EdmError> Visit(object item, List<object> followup, List<object> references)
			{
				return this.VisitT((T)((object)item), followup, references);
			}

			// Token: 0x06000BFA RID: 3066
			protected abstract IEnumerable<EdmError> VisitT(T item, List<object> followup, List<object> references);
		}

		// Token: 0x020001F2 RID: 498
		private sealed class VisitorOfIEdmCheckable : InterfaceValidator.VisitorOfT<IEdmCheckable>
		{
			// Token: 0x06000BFC RID: 3068 RVA: 0x0002322C File Offset: 0x0002142C
			protected override IEnumerable<EdmError> VisitT(IEdmCheckable checkable, List<object> followup, List<object> references)
			{
				List<EdmError> list = new List<EdmError>();
				List<EdmError> list2 = null;
				InterfaceValidator.ProcessEnumerable<IEdmCheckable, EdmError>(checkable, checkable.Errors, "Errors", list, ref list2);
				return list2 ?? list;
			}
		}

		// Token: 0x020001F3 RID: 499
		private sealed class VisitorOfIEdmElement : InterfaceValidator.VisitorOfT<IEdmElement>
		{
			// Token: 0x06000BFE RID: 3070 RVA: 0x00023263 File Offset: 0x00021463
			protected override IEnumerable<EdmError> VisitT(IEdmElement element, List<object> followup, List<object> references)
			{
				return null;
			}
		}

		// Token: 0x020001F4 RID: 500
		private sealed class VisitorOfIEdmNamedElement : InterfaceValidator.VisitorOfT<IEdmNamedElement>
		{
			// Token: 0x06000C00 RID: 3072 RVA: 0x00023270 File Offset: 0x00021470
			protected override IEnumerable<EdmError> VisitT(IEdmNamedElement element, List<object> followup, List<object> references)
			{
				if (element.Name == null)
				{
					return new EdmError[]
					{
						InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmNamedElement>(element, "Name")
					};
				}
				return null;
			}
		}

		// Token: 0x020001F5 RID: 501
		private sealed class VisitorOfIEdmSchemaElement : InterfaceValidator.VisitorOfT<IEdmSchemaElement>
		{
			// Token: 0x06000C02 RID: 3074 RVA: 0x000232A8 File Offset: 0x000214A8
			protected override IEnumerable<EdmError> VisitT(IEdmSchemaElement element, List<object> followup, List<object> references)
			{
				List<EdmError> result = new List<EdmError>();
				switch (element.SchemaElementKind)
				{
				case EdmSchemaElementKind.None:
					break;
				case EdmSchemaElementKind.TypeDefinition:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmSchemaElement, EdmSchemaElementKind, IEdmSchemaType>(element, element.SchemaElementKind, "SchemaElementKind"), ref result);
					break;
				case EdmSchemaElementKind.Function:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmSchemaElement, EdmSchemaElementKind, IEdmFunction>(element, element.SchemaElementKind, "SchemaElementKind"), ref result);
					break;
				case EdmSchemaElementKind.ValueTerm:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmSchemaElement, EdmSchemaElementKind, IEdmValueTerm>(element, element.SchemaElementKind, "SchemaElementKind"), ref result);
					break;
				case EdmSchemaElementKind.EntityContainer:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmSchemaElement, EdmSchemaElementKind, IEdmEntityContainer>(element, element.SchemaElementKind, "SchemaElementKind"), ref result);
					break;
				default:
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateEnumPropertyOutOfRangeError<IEdmSchemaElement, EdmSchemaElementKind>(element, element.SchemaElementKind, "SchemaElementKind"), ref result);
					break;
				}
				if (element.Namespace == null)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmSchemaElement>(element, "Namespace"), ref result);
				}
				return result;
			}
		}

		// Token: 0x020001F6 RID: 502
		private sealed class VisitorOfIEdmModel : InterfaceValidator.VisitorOfT<IEdmModel>
		{
			// Token: 0x06000C04 RID: 3076 RVA: 0x00023384 File Offset: 0x00021584
			protected override IEnumerable<EdmError> VisitT(IEdmModel model, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmModel, IEdmSchemaElement>(model, model.SchemaElements, "SchemaElements", followup, ref result);
				InterfaceValidator.ProcessEnumerable<IEdmModel, IEdmVocabularyAnnotation>(model, model.VocabularyAnnotations, "VocabularyAnnotations", followup, ref result);
				return result;
			}
		}

		// Token: 0x020001F7 RID: 503
		private sealed class VisitorOfIEdmEntityContainer : InterfaceValidator.VisitorOfT<IEdmEntityContainer>
		{
			// Token: 0x06000C06 RID: 3078 RVA: 0x000233C4 File Offset: 0x000215C4
			protected override IEnumerable<EdmError> VisitT(IEdmEntityContainer container, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmEntityContainer, IEdmEntityContainerElement>(container, container.Elements, "Elements", followup, ref result);
				return result;
			}
		}

		// Token: 0x020001F8 RID: 504
		private sealed class VisitorOfIEdmEntityContainerElement : InterfaceValidator.VisitorOfT<IEdmEntityContainerElement>
		{
			// Token: 0x06000C08 RID: 3080 RVA: 0x000233F0 File Offset: 0x000215F0
			protected override IEnumerable<EdmError> VisitT(IEdmEntityContainerElement element, List<object> followup, List<object> references)
			{
				EdmError edmError = null;
				switch (element.ContainerElementKind)
				{
				case EdmContainerElementKind.None:
					break;
				case EdmContainerElementKind.EntitySet:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmEntityContainerElement, EdmContainerElementKind, IEdmEntitySet>(element, element.ContainerElementKind, "ContainerElementKind");
					break;
				case EdmContainerElementKind.FunctionImport:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmEntityContainerElement, EdmContainerElementKind, IEdmFunctionImport>(element, element.ContainerElementKind, "ContainerElementKind");
					break;
				default:
					edmError = InterfaceValidator.CreateEnumPropertyOutOfRangeError<IEdmEntityContainerElement, EdmContainerElementKind>(element, element.ContainerElementKind, "ContainerElementKind");
					break;
				}
				if (edmError == null)
				{
					return null;
				}
				return new EdmError[]
				{
					edmError
				};
			}
		}

		// Token: 0x020001F9 RID: 505
		private sealed class VisitorOfIEdmEntitySet : InterfaceValidator.VisitorOfT<IEdmEntitySet>
		{
			// Token: 0x06000C0A RID: 3082 RVA: 0x00023470 File Offset: 0x00021670
			protected override IEnumerable<EdmError> VisitT(IEdmEntitySet set, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (set.ElementType != null)
				{
					references.Add(set.ElementType);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEntitySet>(set, "ElementType"), ref result);
				}
				List<IEdmNavigationTargetMapping> list = new List<IEdmNavigationTargetMapping>();
				InterfaceValidator.ProcessEnumerable<IEdmEntitySet, IEdmNavigationTargetMapping>(set, set.NavigationTargets, "NavigationTargets", list, ref result);
				foreach (IEdmNavigationTargetMapping edmNavigationTargetMapping in list)
				{
					if (edmNavigationTargetMapping.NavigationProperty != null)
					{
						references.Add(edmNavigationTargetMapping.NavigationProperty);
					}
					else
					{
						InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmNavigationTargetMapping>(edmNavigationTargetMapping, "NavigationProperty"), ref result);
					}
					if (edmNavigationTargetMapping.TargetEntitySet != null)
					{
						references.Add(edmNavigationTargetMapping.TargetEntitySet);
					}
					else
					{
						InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmNavigationTargetMapping>(edmNavigationTargetMapping, "TargetEntitySet"), ref result);
					}
				}
				return result;
			}
		}

		// Token: 0x020001FA RID: 506
		private sealed class VisitorOfIEdmTypeReference : InterfaceValidator.VisitorOfT<IEdmTypeReference>
		{
			// Token: 0x06000C0C RID: 3084 RVA: 0x00023554 File Offset: 0x00021754
			protected override IEnumerable<EdmError> VisitT(IEdmTypeReference type, List<object> followup, List<object> references)
			{
				if (type.Definition != null)
				{
					if (type.Definition is IEdmSchemaType)
					{
						references.Add(type.Definition);
					}
					else
					{
						followup.Add(type.Definition);
					}
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmTypeReference>(type, "Definition")
				};
			}
		}

		// Token: 0x020001FB RID: 507
		private sealed class VisitorOfIEdmType : InterfaceValidator.VisitorOfT<IEdmType>
		{
			// Token: 0x06000C0E RID: 3086 RVA: 0x000235B0 File Offset: 0x000217B0
			protected override IEnumerable<EdmError> VisitT(IEdmType type, List<object> followup, List<object> references)
			{
				EdmError edmError = null;
				switch (type.TypeKind)
				{
				case EdmTypeKind.None:
					break;
				case EdmTypeKind.Primitive:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmPrimitiveType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.Entity:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmEntityType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.Complex:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmComplexType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.Row:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmRowType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.Collection:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmCollectionType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.EntityReference:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmEntityReferenceType>(type, type.TypeKind, "TypeKind");
					break;
				case EdmTypeKind.Enum:
					edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmType, EdmTypeKind, IEdmEnumType>(type, type.TypeKind, "TypeKind");
					break;
				default:
					edmError = InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmType, EdmTypeKind>(type, type.TypeKind, "TypeKind");
					break;
				}
				if (edmError == null)
				{
					return null;
				}
				return new EdmError[]
				{
					edmError
				};
			}
		}

		// Token: 0x020001FC RID: 508
		private sealed class VisitorOfIEdmPrimitiveType : InterfaceValidator.VisitorOfT<IEdmPrimitiveType>
		{
			// Token: 0x06000C10 RID: 3088 RVA: 0x000236AC File Offset: 0x000218AC
			protected override IEnumerable<EdmError> VisitT(IEdmPrimitiveType type, List<object> followup, List<object> references)
			{
				if (!InterfaceValidator.IsCheckableBad(type) && (type.PrimitiveKind < EdmPrimitiveTypeKind.None || type.PrimitiveKind > EdmPrimitiveTypeKind.GeometryMultiPoint))
				{
					return new EdmError[]
					{
						InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmPrimitiveType, EdmPrimitiveTypeKind>(type, type.PrimitiveKind, "PrimitiveKind")
					};
				}
				return null;
			}
		}

		// Token: 0x020001FD RID: 509
		private sealed class VisitorOfIEdmStructuredType : InterfaceValidator.VisitorOfT<IEdmStructuredType>
		{
			// Token: 0x06000C12 RID: 3090 RVA: 0x000236FC File Offset: 0x000218FC
			protected override IEnumerable<EdmError> VisitT(IEdmStructuredType type, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmStructuredType, IEdmProperty>(type, type.DeclaredProperties, "DeclaredProperties", followup, ref result);
				if (type.BaseType != null)
				{
					HashSetInternal<IEdmStructuredType> hashSetInternal = new HashSetInternal<IEdmStructuredType>();
					hashSetInternal.Add(type);
					for (IEdmStructuredType baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
					{
						if (hashSetInternal.Contains(baseType))
						{
							IEdmSchemaType edmSchemaType = type as IEdmSchemaType;
							string p = (edmSchemaType != null) ? edmSchemaType.FullName() : typeof(Type).Name;
							InterfaceValidator.CollectErrors(new EdmError(InterfaceValidator.GetLocation(type), EdmErrorCode.InterfaceCriticalCycleInTypeHierarchy, Strings.EdmModel_Validator_Syntactic_InterfaceCriticalCycleInTypeHierarchy(p)), ref result);
							break;
						}
					}
					references.Add(type.BaseType);
				}
				return result;
			}
		}

		// Token: 0x020001FE RID: 510
		private sealed class VisitorOfIEdmEntityType : InterfaceValidator.VisitorOfT<IEdmEntityType>
		{
			// Token: 0x06000C14 RID: 3092 RVA: 0x000237A8 File Offset: 0x000219A8
			protected override IEnumerable<EdmError> VisitT(IEdmEntityType type, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (type.DeclaredKey != null)
				{
					InterfaceValidator.ProcessEnumerable<IEdmEntityType, IEdmStructuralProperty>(type, type.DeclaredKey, "DeclaredKey", references, ref result);
				}
				return result;
			}
		}

		// Token: 0x020001FF RID: 511
		private sealed class VisitorOfIEdmEntityReferenceType : InterfaceValidator.VisitorOfT<IEdmEntityReferenceType>
		{
			// Token: 0x06000C16 RID: 3094 RVA: 0x000237DC File Offset: 0x000219DC
			protected override IEnumerable<EdmError> VisitT(IEdmEntityReferenceType type, List<object> followup, List<object> references)
			{
				if (type.EntityType != null)
				{
					references.Add(type.EntityType);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEntityReferenceType>(type, "EntityType")
				};
			}
		}

		// Token: 0x02000200 RID: 512
		private sealed class VisitorOfIEdmEnumType : InterfaceValidator.VisitorOfT<IEdmEnumType>
		{
			// Token: 0x06000C18 RID: 3096 RVA: 0x00023820 File Offset: 0x00021A20
			protected override IEnumerable<EdmError> VisitT(IEdmEnumType type, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmEnumType, IEdmEnumMember>(type, type.Members, "Members", followup, ref result);
				if (type.UnderlyingType != null)
				{
					references.Add(type.UnderlyingType);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEnumType>(type, "UnderlyingType"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000201 RID: 513
		private sealed class VisitorOfIEdmTerm : InterfaceValidator.VisitorOfT<IEdmTerm>
		{
			// Token: 0x06000C1A RID: 3098 RVA: 0x00023874 File Offset: 0x00021A74
			protected override IEnumerable<EdmError> VisitT(IEdmTerm term, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				switch (term.TermKind)
				{
				case EdmTermKind.None:
					break;
				case EdmTermKind.Type:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmTerm, EdmTermKind, IEdmSchemaType>(term, term.TermKind, "TermKind"), ref result);
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmTerm, EdmTermKind, IEdmStructuredType>(term, term.TermKind, "TermKind"), ref result);
					break;
				case EdmTermKind.Value:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmTerm, EdmTermKind, IEdmValueTerm>(term, term.TermKind, "TermKind"), ref result);
					break;
				default:
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmTerm, EdmTermKind>(term, term.TermKind, "TermKind"), ref result);
					break;
				}
				return result;
			}
		}

		// Token: 0x02000202 RID: 514
		private sealed class VisitorOfIEdmValueTerm : InterfaceValidator.VisitorOfT<IEdmValueTerm>
		{
			// Token: 0x06000C1C RID: 3100 RVA: 0x0002390C File Offset: 0x00021B0C
			protected override IEnumerable<EdmError> VisitT(IEdmValueTerm term, List<object> followup, List<object> references)
			{
				if (term.Type != null)
				{
					followup.Add(term.Type);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmValueTerm>(term, "Type")
				};
			}
		}

		// Token: 0x02000203 RID: 515
		private sealed class VisitorOfIEdmCollectionType : InterfaceValidator.VisitorOfT<IEdmCollectionType>
		{
			// Token: 0x06000C1E RID: 3102 RVA: 0x00023950 File Offset: 0x00021B50
			protected override IEnumerable<EdmError> VisitT(IEdmCollectionType type, List<object> followup, List<object> references)
			{
				if (type.ElementType != null)
				{
					followup.Add(type.ElementType);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmCollectionType>(type, "ElementType")
				};
			}
		}

		// Token: 0x02000204 RID: 516
		private sealed class VisitorOfIEdmProperty : InterfaceValidator.VisitorOfT<IEdmProperty>
		{
			// Token: 0x06000C20 RID: 3104 RVA: 0x00023994 File Offset: 0x00021B94
			protected override IEnumerable<EdmError> VisitT(IEdmProperty property, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				switch (property.PropertyKind)
				{
				case EdmPropertyKind.Structural:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmProperty, EdmPropertyKind, IEdmStructuralProperty>(property, property.PropertyKind, "PropertyKind"), ref result);
					break;
				case EdmPropertyKind.Navigation:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmProperty, EdmPropertyKind, IEdmNavigationProperty>(property, property.PropertyKind, "PropertyKind"), ref result);
					break;
				case EdmPropertyKind.None:
					break;
				default:
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmProperty, EdmPropertyKind>(property, property.PropertyKind, "PropertyKind"), ref result);
					break;
				}
				if (property.Type != null)
				{
					followup.Add(property.Type);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmProperty>(property, "Type"), ref result);
				}
				if (property.DeclaringType != null)
				{
					references.Add(property.DeclaringType);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmProperty>(property, "DeclaringType"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000205 RID: 517
		private sealed class VisitorOfIEdmStructuralProperty : InterfaceValidator.VisitorOfT<IEdmStructuralProperty>
		{
			// Token: 0x06000C22 RID: 3106 RVA: 0x00023A64 File Offset: 0x00021C64
			protected override IEnumerable<EdmError> VisitT(IEdmStructuralProperty property, List<object> followup, List<object> references)
			{
				if (property.ConcurrencyMode < EdmConcurrencyMode.None || property.ConcurrencyMode > EdmConcurrencyMode.Fixed)
				{
					return new EdmError[]
					{
						InterfaceValidator.CreateEnumPropertyOutOfRangeError<IEdmStructuralProperty, EdmConcurrencyMode>(property, property.ConcurrencyMode, "ConcurrencyMode")
					};
				}
				return null;
			}
		}

		// Token: 0x02000206 RID: 518
		private sealed class VisitorOfIEdmNavigationProperty : InterfaceValidator.VisitorOfT<IEdmNavigationProperty>
		{
			// Token: 0x06000C24 RID: 3108 RVA: 0x00023AAC File Offset: 0x00021CAC
			protected override IEnumerable<EdmError> VisitT(IEdmNavigationProperty property, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (property.Partner != null)
				{
					if (!property.Partner.DeclaringType.DeclaredProperties.Contains(property.Partner))
					{
						followup.Add(property.Partner);
					}
					else
					{
						references.Add(property.Partner);
					}
					if (property.Partner.Partner != property || property.Partner == property)
					{
						InterfaceValidator.CollectErrors(new EdmError(InterfaceValidator.GetLocation(property), EdmErrorCode.InterfaceCriticalNavigationPartnerInvalid, Strings.EdmModel_Validator_Syntactic_NavigationPartnerInvalid(property.Name)), ref result);
					}
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmNavigationProperty>(property, "Partner"), ref result);
				}
				if (property.DependentProperties != null)
				{
					InterfaceValidator.ProcessEnumerable<IEdmNavigationProperty, IEdmStructuralProperty>(property, property.DependentProperties, "DependentProperties", references, ref result);
				}
				if (property.OnDelete < EdmOnDeleteAction.None || property.OnDelete > EdmOnDeleteAction.Cascade)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateEnumPropertyOutOfRangeError<IEdmNavigationProperty, EdmOnDeleteAction>(property, property.OnDelete, "OnDelete"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000207 RID: 519
		private sealed class VisitorOfIEdmEnumMember : InterfaceValidator.VisitorOfT<IEdmEnumMember>
		{
			// Token: 0x06000C26 RID: 3110 RVA: 0x00023B94 File Offset: 0x00021D94
			protected override IEnumerable<EdmError> VisitT(IEdmEnumMember member, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (member.DeclaringType != null)
				{
					references.Add(member.DeclaringType);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEnumMember>(member, "DeclaringType"), ref result);
				}
				if (member.Value != null)
				{
					followup.Add(member.Value);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEnumMember>(member, "Value"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000208 RID: 520
		private sealed class VisitorOfIEdmFunctionBase : InterfaceValidator.VisitorOfT<IEdmFunctionBase>
		{
			// Token: 0x06000C28 RID: 3112 RVA: 0x00023BFC File Offset: 0x00021DFC
			protected override IEnumerable<EdmError> VisitT(IEdmFunctionBase function, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmFunctionBase, IEdmFunctionParameter>(function, function.Parameters, "Parameters", followup, ref result);
				if (function.ReturnType != null)
				{
					followup.Add(function.ReturnType);
				}
				return result;
			}
		}

		// Token: 0x02000209 RID: 521
		private sealed class VisitorOfIEdmFunction : InterfaceValidator.VisitorOfT<IEdmFunction>
		{
			// Token: 0x06000C2A RID: 3114 RVA: 0x00023C3C File Offset: 0x00021E3C
			protected override IEnumerable<EdmError> VisitT(IEdmFunction function, List<object> followup, List<object> references)
			{
				if (function.ReturnType != null)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmFunction>(function, "ReturnType")
				};
			}
		}

		// Token: 0x0200020A RID: 522
		private sealed class VisitorOfIEdmFunctionImport : InterfaceValidator.VisitorOfT<IEdmFunctionImport>
		{
			// Token: 0x06000C2C RID: 3116 RVA: 0x00023C71 File Offset: 0x00021E71
			protected override IEnumerable<EdmError> VisitT(IEdmFunctionImport functionImport, List<object> followup, List<object> references)
			{
				if (functionImport.EntitySet != null)
				{
					followup.Add(functionImport.EntitySet);
				}
				return null;
			}
		}

		// Token: 0x0200020B RID: 523
		private sealed class VisitorOfIEdmFunctionParameter : InterfaceValidator.VisitorOfT<IEdmFunctionParameter>
		{
			// Token: 0x06000C2E RID: 3118 RVA: 0x00023C90 File Offset: 0x00021E90
			protected override IEnumerable<EdmError> VisitT(IEdmFunctionParameter parameter, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (parameter.Type != null)
				{
					followup.Add(parameter.Type);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmFunctionParameter>(parameter, "Type"), ref result);
				}
				if (parameter.DeclaringFunction != null)
				{
					references.Add(parameter.DeclaringFunction);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmFunctionParameter>(parameter, "DeclaringFunction"), ref result);
				}
				if (parameter.Mode < EdmFunctionParameterMode.None || parameter.Mode > EdmFunctionParameterMode.InOut)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateEnumPropertyOutOfRangeError<IEdmFunctionParameter, EdmFunctionParameterMode>(parameter, parameter.Mode, "Mode"), ref result);
				}
				return result;
			}
		}

		// Token: 0x0200020C RID: 524
		private sealed class VisitorOfIEdmCollectionTypeReference : InterfaceValidator.VisitorOfT<IEdmCollectionTypeReference>
		{
			// Token: 0x06000C30 RID: 3120 RVA: 0x00023D24 File Offset: 0x00021F24
			protected override IEnumerable<EdmError> VisitT(IEdmCollectionTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Collection)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmCollectionTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x0200020D RID: 525
		private sealed class VisitorOfIEdmEntityReferenceTypeReference : InterfaceValidator.VisitorOfT<IEdmEntityReferenceTypeReference>
		{
			// Token: 0x06000C32 RID: 3122 RVA: 0x00023D64 File Offset: 0x00021F64
			protected override IEnumerable<EdmError> VisitT(IEdmEntityReferenceTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.EntityReference)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmEntityReferenceTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x0200020E RID: 526
		private sealed class VisitorOfIEdmStructuredTypeReference : InterfaceValidator.VisitorOfT<IEdmStructuredTypeReference>
		{
			// Token: 0x06000C34 RID: 3124 RVA: 0x00023DA4 File Offset: 0x00021FA4
			protected override IEnumerable<EdmError> VisitT(IEdmStructuredTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind.IsStructured())
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmStructuredTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x0200020F RID: 527
		private sealed class VisitorOfIEdmEntityTypeReference : InterfaceValidator.VisitorOfT<IEdmEntityTypeReference>
		{
			// Token: 0x06000C36 RID: 3126 RVA: 0x00023DE8 File Offset: 0x00021FE8
			protected override IEnumerable<EdmError> VisitT(IEdmEntityTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Entity)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmEntityTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000210 RID: 528
		private sealed class VisitorOfIEdmComplexTypeReference : InterfaceValidator.VisitorOfT<IEdmComplexTypeReference>
		{
			// Token: 0x06000C38 RID: 3128 RVA: 0x00023E28 File Offset: 0x00022028
			protected override IEnumerable<EdmError> VisitT(IEdmComplexTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Complex)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmComplexTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000211 RID: 529
		private sealed class VisitorOfIEdmRowTypeReference : InterfaceValidator.VisitorOfT<IEdmRowTypeReference>
		{
			// Token: 0x06000C3A RID: 3130 RVA: 0x00023E68 File Offset: 0x00022068
			protected override IEnumerable<EdmError> VisitT(IEdmRowTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Row)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmRowTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000212 RID: 530
		private sealed class VisitorOfIEdmEnumTypeReference : InterfaceValidator.VisitorOfT<IEdmEnumTypeReference>
		{
			// Token: 0x06000C3C RID: 3132 RVA: 0x00023EA8 File Offset: 0x000220A8
			protected override IEnumerable<EdmError> VisitT(IEdmEnumTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Enum)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmEnumTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000213 RID: 531
		private sealed class VisitorOfIEdmPrimitiveTypeReference : InterfaceValidator.VisitorOfT<IEdmPrimitiveTypeReference>
		{
			// Token: 0x06000C3E RID: 3134 RVA: 0x00023EE8 File Offset: 0x000220E8
			protected override IEnumerable<EdmError> VisitT(IEdmPrimitiveTypeReference typeRef, List<object> followup, List<object> references)
			{
				if (typeRef.Definition == null || typeRef.Definition.TypeKind == EdmTypeKind.Primitive)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreateTypeRefInterfaceTypeKindValueMismatchError<IEdmPrimitiveTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000214 RID: 532
		private sealed class VisitorOfIEdmBinaryTypeReference : InterfaceValidator.VisitorOfT<IEdmBinaryTypeReference>
		{
			// Token: 0x06000C40 RID: 3136 RVA: 0x00023F28 File Offset: 0x00022128
			protected override IEnumerable<EdmError> VisitT(IEdmBinaryTypeReference typeRef, List<object> followup, List<object> references)
			{
				IEdmPrimitiveType edmPrimitiveType = typeRef.Definition as IEdmPrimitiveType;
				if (edmPrimitiveType == null || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Binary)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<IEdmBinaryTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000215 RID: 533
		private sealed class VisitorOfIEdmDecimalTypeReference : InterfaceValidator.VisitorOfT<IEdmDecimalTypeReference>
		{
			// Token: 0x06000C42 RID: 3138 RVA: 0x00023F68 File Offset: 0x00022168
			protected override IEnumerable<EdmError> VisitT(IEdmDecimalTypeReference typeRef, List<object> followup, List<object> references)
			{
				IEdmPrimitiveType edmPrimitiveType = typeRef.Definition as IEdmPrimitiveType;
				if (edmPrimitiveType == null || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.Decimal)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<IEdmDecimalTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000216 RID: 534
		private sealed class VisitorOfIEdmStringTypeReference : InterfaceValidator.VisitorOfT<IEdmStringTypeReference>
		{
			// Token: 0x06000C44 RID: 3140 RVA: 0x00023FA8 File Offset: 0x000221A8
			protected override IEnumerable<EdmError> VisitT(IEdmStringTypeReference typeRef, List<object> followup, List<object> references)
			{
				IEdmPrimitiveType edmPrimitiveType = typeRef.Definition as IEdmPrimitiveType;
				if (edmPrimitiveType == null || edmPrimitiveType.PrimitiveKind == EdmPrimitiveTypeKind.String)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<IEdmStringTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000217 RID: 535
		private sealed class VisitorOfIEdmTemporalTypeReference : InterfaceValidator.VisitorOfT<IEdmTemporalTypeReference>
		{
			// Token: 0x06000C46 RID: 3142 RVA: 0x00023FEC File Offset: 0x000221EC
			protected override IEnumerable<EdmError> VisitT(IEdmTemporalTypeReference typeRef, List<object> followup, List<object> references)
			{
				IEdmPrimitiveType edmPrimitiveType = typeRef.Definition as IEdmPrimitiveType;
				if (edmPrimitiveType == null || edmPrimitiveType.PrimitiveKind.IsTemporal())
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<IEdmTemporalTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000218 RID: 536
		private sealed class VisitorOfIEdmSpatialTypeReference : InterfaceValidator.VisitorOfT<IEdmSpatialTypeReference>
		{
			// Token: 0x06000C48 RID: 3144 RVA: 0x00024030 File Offset: 0x00022230
			protected override IEnumerable<EdmError> VisitT(IEdmSpatialTypeReference typeRef, List<object> followup, List<object> references)
			{
				IEdmPrimitiveType edmPrimitiveType = typeRef.Definition as IEdmPrimitiveType;
				if (edmPrimitiveType == null || edmPrimitiveType.PrimitiveKind.IsSpatial())
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePrimitiveTypeRefInterfaceTypeKindValueMismatchError<IEdmSpatialTypeReference>(typeRef)
				};
			}
		}

		// Token: 0x02000219 RID: 537
		private sealed class VisitorOfIEdmExpression : InterfaceValidator.VisitorOfT<IEdmExpression>
		{
			// Token: 0x06000C4A RID: 3146 RVA: 0x00024074 File Offset: 0x00022274
			protected override IEnumerable<EdmError> VisitT(IEdmExpression expression, List<object> followup, List<object> references)
			{
				EdmError edmError = null;
				if (!InterfaceValidator.IsCheckableBad(expression))
				{
					switch (expression.ExpressionKind)
					{
					case EdmExpressionKind.BinaryConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmBinaryConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.BooleanConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmBooleanConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.DateTimeConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmDateTimeConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.DateTimeOffsetConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmDateTimeOffsetConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.DecimalConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmDecimalConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.FloatingConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmFloatingConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.GuidConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmGuidConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.IntegerConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmIntegerConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.StringConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmStringConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.TimeConstant:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmTimeConstantExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.Null:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmNullExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.Record:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmRecordExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.Collection:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmCollectionExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.Path:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmPathExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.ParameterReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmParameterReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.FunctionReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmFunctionReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.PropertyReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmPropertyReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.ValueTermReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmValueTermReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.EntitySetReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmEntitySetReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.EnumMemberReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmEnumMemberReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.If:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmIfExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.AssertType:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmAssertTypeExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.IsType:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmIsTypeExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.FunctionApplication:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmApplyExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.LabeledExpressionReference:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmLabeledExpressionReferenceExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					case EdmExpressionKind.Labeled:
						edmError = InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmExpression, EdmExpressionKind, IEdmLabeledExpression>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					default:
						edmError = InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmExpression, EdmExpressionKind>(expression, expression.ExpressionKind, "ExpressionKind");
						break;
					}
				}
				if (edmError == null)
				{
					return null;
				}
				return new EdmError[]
				{
					edmError
				};
			}
		}

		// Token: 0x0200021A RID: 538
		private sealed class VisitorOfIEdmRecordExpression : InterfaceValidator.VisitorOfT<IEdmRecordExpression>
		{
			// Token: 0x06000C4C RID: 3148 RVA: 0x0002437C File Offset: 0x0002257C
			protected override IEnumerable<EdmError> VisitT(IEdmRecordExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmRecordExpression, IEdmPropertyConstructor>(expression, expression.Properties, "Properties", followup, ref result);
				if (expression.DeclaredType != null)
				{
					followup.Add(expression.DeclaredType);
				}
				return result;
			}
		}

		// Token: 0x0200021B RID: 539
		private sealed class VisitorOfIEdmPropertyConstructor : InterfaceValidator.VisitorOfT<IEdmPropertyConstructor>
		{
			// Token: 0x06000C4E RID: 3150 RVA: 0x000243BC File Offset: 0x000225BC
			protected override IEnumerable<EdmError> VisitT(IEdmPropertyConstructor expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.Name == null)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyConstructor>(expression, "Name"), ref result);
				}
				if (expression.Value != null)
				{
					followup.Add(expression.Value);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyConstructor>(expression, "Value"), ref result);
				}
				return result;
			}
		}

		// Token: 0x0200021C RID: 540
		private sealed class VisitorOfIEdmCollectionExpression : InterfaceValidator.VisitorOfT<IEdmCollectionExpression>
		{
			// Token: 0x06000C50 RID: 3152 RVA: 0x00024418 File Offset: 0x00022618
			protected override IEnumerable<EdmError> VisitT(IEdmCollectionExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmCollectionExpression, IEdmExpression>(expression, expression.Elements, "Elements", followup, ref result);
				if (expression.DeclaredType != null)
				{
					followup.Add(expression.DeclaredType);
				}
				return result;
			}
		}

		// Token: 0x0200021D RID: 541
		private sealed class VisitorOfIEdmLabeledElement : InterfaceValidator.VisitorOfT<IEdmLabeledExpression>
		{
			// Token: 0x06000C52 RID: 3154 RVA: 0x00024458 File Offset: 0x00022658
			protected override IEnumerable<EdmError> VisitT(IEdmLabeledExpression expression, List<object> followup, List<object> references)
			{
				if (expression.Expression != null)
				{
					followup.Add(expression.Expression);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmLabeledExpression>(expression, "Expression")
				};
			}
		}

		// Token: 0x0200021E RID: 542
		private sealed class VisitorOfIEdmPathExpression : InterfaceValidator.VisitorOfT<IEdmPathExpression>
		{
			// Token: 0x06000C54 RID: 3156 RVA: 0x0002449C File Offset: 0x0002269C
			protected override IEnumerable<EdmError> VisitT(IEdmPathExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				List<string> targetList = new List<string>();
				InterfaceValidator.ProcessEnumerable<IEdmPathExpression, string>(expression, expression.Path, "Path", targetList, ref result);
				return result;
			}
		}

		// Token: 0x0200021F RID: 543
		private sealed class VisitorOfIEdmParameterReferenceExpression : InterfaceValidator.VisitorOfT<IEdmParameterReferenceExpression>
		{
			// Token: 0x06000C56 RID: 3158 RVA: 0x000244D0 File Offset: 0x000226D0
			protected override IEnumerable<EdmError> VisitT(IEdmParameterReferenceExpression expression, List<object> followup, List<object> references)
			{
				if (expression.ReferencedParameter != null)
				{
					references.Add(expression.ReferencedParameter);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmParameterReferenceExpression>(expression, "ReferencedParameter")
				};
			}
		}

		// Token: 0x02000220 RID: 544
		private sealed class VisitorOfIEdmFunctionReferenceExpression : InterfaceValidator.VisitorOfT<IEdmFunctionReferenceExpression>
		{
			// Token: 0x06000C58 RID: 3160 RVA: 0x00024514 File Offset: 0x00022714
			protected override IEnumerable<EdmError> VisitT(IEdmFunctionReferenceExpression expression, List<object> followup, List<object> references)
			{
				if (expression.ReferencedFunction != null)
				{
					references.Add(expression.ReferencedFunction);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmFunctionReferenceExpression>(expression, "ReferencedFunction")
				};
			}
		}

		// Token: 0x02000221 RID: 545
		private sealed class VisitorOfIEdmPropertyReferenceExpression : InterfaceValidator.VisitorOfT<IEdmPropertyReferenceExpression>
		{
			// Token: 0x06000C5A RID: 3162 RVA: 0x00024558 File Offset: 0x00022758
			protected override IEnumerable<EdmError> VisitT(IEdmPropertyReferenceExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.Base != null)
				{
					followup.Add(expression.Base);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyReferenceExpression>(expression, "Base"), ref result);
				}
				if (expression.ReferencedProperty != null)
				{
					references.Add(expression.ReferencedProperty);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyReferenceExpression>(expression, "ReferencedProperty"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000222 RID: 546
		private sealed class VisitorOfIEdmValueTermReferenceExpression : InterfaceValidator.VisitorOfT<IEdmValueTermReferenceExpression>
		{
			// Token: 0x06000C5C RID: 3164 RVA: 0x000245C0 File Offset: 0x000227C0
			protected override IEnumerable<EdmError> VisitT(IEdmValueTermReferenceExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.Base != null)
				{
					followup.Add(expression.Base);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmValueTermReferenceExpression>(expression, "Base"), ref result);
				}
				if (expression.Term != null)
				{
					references.Add(expression.Term);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmValueTermReferenceExpression>(expression, "Term"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000223 RID: 547
		private sealed class VistorOfIEdmEntitySetReferenceExpression : InterfaceValidator.VisitorOfT<IEdmEntitySetReferenceExpression>
		{
			// Token: 0x06000C5E RID: 3166 RVA: 0x00024628 File Offset: 0x00022828
			protected override IEnumerable<EdmError> VisitT(IEdmEntitySetReferenceExpression expression, List<object> followup, List<object> references)
			{
				if (expression.ReferencedEntitySet != null)
				{
					references.Add(expression.ReferencedEntitySet);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEntitySetReferenceExpression>(expression, "ReferencedEntitySet")
				};
			}
		}

		// Token: 0x02000224 RID: 548
		private sealed class VistorOfIEdmEnumMemberReferenceExpression : InterfaceValidator.VisitorOfT<IEdmEnumMemberReferenceExpression>
		{
			// Token: 0x06000C60 RID: 3168 RVA: 0x0002466C File Offset: 0x0002286C
			protected override IEnumerable<EdmError> VisitT(IEdmEnumMemberReferenceExpression expression, List<object> followup, List<object> references)
			{
				if (expression.ReferencedEnumMember != null)
				{
					references.Add(expression.ReferencedEnumMember);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEnumMemberReferenceExpression>(expression, "ReferencedEnumMember")
				};
			}
		}

		// Token: 0x02000225 RID: 549
		private sealed class VistorOfIEdmIfExpression : InterfaceValidator.VisitorOfT<IEdmIfExpression>
		{
			// Token: 0x06000C62 RID: 3170 RVA: 0x000246B0 File Offset: 0x000228B0
			protected override IEnumerable<EdmError> VisitT(IEdmIfExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.TestExpression != null)
				{
					followup.Add(expression.TestExpression);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmIfExpression>(expression, "TestExpression"), ref result);
				}
				if (expression.TrueExpression != null)
				{
					followup.Add(expression.TrueExpression);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmIfExpression>(expression, "TrueExpression"), ref result);
				}
				if (expression.FalseExpression != null)
				{
					followup.Add(expression.FalseExpression);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmIfExpression>(expression, "FalseExpression"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000226 RID: 550
		private sealed class VistorOfIEdmAssertTypeExpression : InterfaceValidator.VisitorOfT<IEdmAssertTypeExpression>
		{
			// Token: 0x06000C64 RID: 3172 RVA: 0x00024740 File Offset: 0x00022940
			protected override IEnumerable<EdmError> VisitT(IEdmAssertTypeExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.Operand != null)
				{
					followup.Add(expression.Operand);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmAssertTypeExpression>(expression, "Operand"), ref result);
				}
				if (expression.Type != null)
				{
					followup.Add(expression.Type);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmAssertTypeExpression>(expression, "Type"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000227 RID: 551
		private sealed class VistorOfIEdmIsTypeExpression : InterfaceValidator.VisitorOfT<IEdmIsTypeExpression>
		{
			// Token: 0x06000C66 RID: 3174 RVA: 0x000247A8 File Offset: 0x000229A8
			protected override IEnumerable<EdmError> VisitT(IEdmIsTypeExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.Operand != null)
				{
					followup.Add(expression.Operand);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmIsTypeExpression>(expression, "Operand"), ref result);
				}
				if (expression.Type != null)
				{
					followup.Add(expression.Type);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmIsTypeExpression>(expression, "Type"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000228 RID: 552
		private sealed class VistorOfIEdmFunctionApplicationExpression : InterfaceValidator.VisitorOfT<IEdmApplyExpression>
		{
			// Token: 0x06000C68 RID: 3176 RVA: 0x00024810 File Offset: 0x00022A10
			protected override IEnumerable<EdmError> VisitT(IEdmApplyExpression expression, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (expression.AppliedFunction != null)
				{
					followup.Add(expression.AppliedFunction);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmApplyExpression>(expression, "AppliedFunction"), ref result);
				}
				InterfaceValidator.ProcessEnumerable<IEdmApplyExpression, IEdmExpression>(expression, expression.Arguments, "Arguments", followup, ref result);
				return result;
			}
		}

		// Token: 0x02000229 RID: 553
		private sealed class VistorOfIEdmLabeledElementReferenceExpression : InterfaceValidator.VisitorOfT<IEdmLabeledExpressionReferenceExpression>
		{
			// Token: 0x06000C6A RID: 3178 RVA: 0x00024864 File Offset: 0x00022A64
			protected override IEnumerable<EdmError> VisitT(IEdmLabeledExpressionReferenceExpression expression, List<object> followup, List<object> references)
			{
				if (expression.ReferencedLabeledExpression != null)
				{
					references.Add(expression.ReferencedLabeledExpression);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmLabeledExpressionReferenceExpression>(expression, "ReferencedLabeledExpression")
				};
			}
		}

		// Token: 0x0200022A RID: 554
		private sealed class VisitorOfIEdmValue : InterfaceValidator.VisitorOfT<IEdmValue>
		{
			// Token: 0x06000C6C RID: 3180 RVA: 0x000248A8 File Offset: 0x00022AA8
			protected override IEnumerable<EdmError> VisitT(IEdmValue value, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (value.Type != null)
				{
					followup.Add(value.Type);
				}
				switch (value.ValueKind)
				{
				case EdmValueKind.Binary:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmBinaryValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Boolean:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmBooleanValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Collection:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmCollectionValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.DateTimeOffset:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmDateTimeOffsetValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.DateTime:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmDateTimeValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Decimal:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmDecimalValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Enum:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmEnumValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Floating:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmFloatingValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Guid:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmGuidValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Integer:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmIntegerValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Null:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmNullValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.String:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmStringValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Structured:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmStructuredValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.Time:
					InterfaceValidator.CollectErrors(InterfaceValidator.CheckForInterfaceKindValueMismatchError<IEdmValue, EdmValueKind, IEdmTimeValue>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				case EdmValueKind.None:
					break;
				default:
					InterfaceValidator.CollectErrors(InterfaceValidator.CreateInterfaceKindValueUnexpectedError<IEdmValue, EdmValueKind>(value, value.ValueKind, "ValueKind"), ref result);
					break;
				}
				return result;
			}
		}

		// Token: 0x0200022B RID: 555
		private sealed class VisitorOfIEdmDelayedValue : InterfaceValidator.VisitorOfT<IEdmDelayedValue>
		{
			// Token: 0x06000C6E RID: 3182 RVA: 0x00024AC4 File Offset: 0x00022CC4
			protected override IEnumerable<EdmError> VisitT(IEdmDelayedValue value, List<object> followup, List<object> references)
			{
				if (value.Value != null)
				{
					followup.Add(value.Value);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmDelayedValue>(value, "Value")
				};
			}
		}

		// Token: 0x0200022C RID: 556
		private sealed class VisitorOfIEdmPropertyValue : InterfaceValidator.VisitorOfT<IEdmPropertyValue>
		{
			// Token: 0x06000C70 RID: 3184 RVA: 0x00024B08 File Offset: 0x00022D08
			protected override IEnumerable<EdmError> VisitT(IEdmPropertyValue value, List<object> followup, List<object> references)
			{
				if (value.Name != null)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyValue>(value, "Name")
				};
			}
		}

		// Token: 0x0200022D RID: 557
		private sealed class VisitorOfIEdmEnumValue : InterfaceValidator.VisitorOfT<IEdmEnumValue>
		{
			// Token: 0x06000C72 RID: 3186 RVA: 0x00024B40 File Offset: 0x00022D40
			protected override IEnumerable<EdmError> VisitT(IEdmEnumValue value, List<object> followup, List<object> references)
			{
				if (value.Value != null)
				{
					followup.Add(value.Value);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmEnumValue>(value, "Value")
				};
			}
		}

		// Token: 0x0200022E RID: 558
		private sealed class VisitorOfIEdmCollectionValue : InterfaceValidator.VisitorOfT<IEdmCollectionValue>
		{
			// Token: 0x06000C74 RID: 3188 RVA: 0x00024B84 File Offset: 0x00022D84
			protected override IEnumerable<EdmError> VisitT(IEdmCollectionValue value, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmCollectionValue, IEdmDelayedValue>(value, value.Elements, "Elements", followup, ref result);
				return result;
			}
		}

		// Token: 0x0200022F RID: 559
		private sealed class VisitorOfIEdmStructuredValue : InterfaceValidator.VisitorOfT<IEdmStructuredValue>
		{
			// Token: 0x06000C76 RID: 3190 RVA: 0x00024BB0 File Offset: 0x00022DB0
			protected override IEnumerable<EdmError> VisitT(IEdmStructuredValue value, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmStructuredValue, IEdmPropertyValue>(value, value.PropertyValues, "PropertyValues", followup, ref result);
				return result;
			}
		}

		// Token: 0x02000230 RID: 560
		private sealed class VisitorOfIEdmBinaryValue : InterfaceValidator.VisitorOfT<IEdmBinaryValue>
		{
			// Token: 0x06000C78 RID: 3192 RVA: 0x00024BDC File Offset: 0x00022DDC
			protected override IEnumerable<EdmError> VisitT(IEdmBinaryValue value, List<object> followup, List<object> references)
			{
				if (value.Value != null)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmBinaryValue>(value, "Value")
				};
			}
		}

		// Token: 0x02000231 RID: 561
		private sealed class VisitorOfIEdmStringValue : InterfaceValidator.VisitorOfT<IEdmStringValue>
		{
			// Token: 0x06000C7A RID: 3194 RVA: 0x00024C14 File Offset: 0x00022E14
			protected override IEnumerable<EdmError> VisitT(IEdmStringValue value, List<object> followup, List<object> references)
			{
				if (value.Value != null)
				{
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmStringValue>(value, "Value")
				};
			}
		}

		// Token: 0x02000232 RID: 562
		private sealed class VisitorOfIEdmVocabularyAnnotation : InterfaceValidator.VisitorOfT<IEdmVocabularyAnnotation>
		{
			// Token: 0x06000C7C RID: 3196 RVA: 0x00024C4C File Offset: 0x00022E4C
			protected override IEnumerable<EdmError> VisitT(IEdmVocabularyAnnotation annotation, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (annotation.Term != null)
				{
					references.Add(annotation.Term);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmVocabularyAnnotation>(annotation, "Term"), ref result);
				}
				if (annotation.Target != null)
				{
					references.Add(annotation.Target);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmVocabularyAnnotation>(annotation, "Target"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000233 RID: 563
		private sealed class VisitorOfIEdmValueAnnotation : InterfaceValidator.VisitorOfT<IEdmValueAnnotation>
		{
			// Token: 0x06000C7E RID: 3198 RVA: 0x00024CB4 File Offset: 0x00022EB4
			protected override IEnumerable<EdmError> VisitT(IEdmValueAnnotation annotation, List<object> followup, List<object> references)
			{
				if (annotation.Value != null)
				{
					followup.Add(annotation.Value);
					return null;
				}
				return new EdmError[]
				{
					InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmValueAnnotation>(annotation, "Value")
				};
			}
		}

		// Token: 0x02000234 RID: 564
		private sealed class VisitorOfIEdmTypeAnnotation : InterfaceValidator.VisitorOfT<IEdmTypeAnnotation>
		{
			// Token: 0x06000C80 RID: 3200 RVA: 0x00024CF8 File Offset: 0x00022EF8
			protected override IEnumerable<EdmError> VisitT(IEdmTypeAnnotation annotation, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				InterfaceValidator.ProcessEnumerable<IEdmTypeAnnotation, IEdmPropertyValueBinding>(annotation, annotation.PropertyValueBindings, "PropertyValueBindings", followup, ref result);
				return result;
			}
		}

		// Token: 0x02000235 RID: 565
		private sealed class VisitorOfIEdmPropertyValueBinding : InterfaceValidator.VisitorOfT<IEdmPropertyValueBinding>
		{
			// Token: 0x06000C82 RID: 3202 RVA: 0x00024D24 File Offset: 0x00022F24
			protected override IEnumerable<EdmError> VisitT(IEdmPropertyValueBinding binding, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (binding.Value != null)
				{
					followup.Add(binding.Value);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyValueBinding>(binding, "Value"), ref result);
				}
				if (binding.BoundProperty != null)
				{
					references.Add(binding.BoundProperty);
				}
				else
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmPropertyValueBinding>(binding, "BoundProperty"), ref result);
				}
				return result;
			}
		}

		// Token: 0x02000236 RID: 566
		private sealed class VisitorOfIEdmDirectValueAnnotation : InterfaceValidator.VisitorOfT<IEdmDirectValueAnnotation>
		{
			// Token: 0x06000C84 RID: 3204 RVA: 0x00024D8C File Offset: 0x00022F8C
			protected override IEnumerable<EdmError> VisitT(IEdmDirectValueAnnotation annotation, List<object> followup, List<object> references)
			{
				List<EdmError> result = null;
				if (annotation.NamespaceUri == null)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmDirectValueAnnotation>(annotation, "NamespaceUri"), ref result);
				}
				if (annotation.Value == null)
				{
					InterfaceValidator.CollectErrors(InterfaceValidator.CreatePropertyMustNotBeNullError<IEdmDirectValueAnnotation>(annotation, "Value"), ref result);
				}
				return result;
			}
		}
	}
}
