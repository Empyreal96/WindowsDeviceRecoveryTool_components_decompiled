using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Annotations;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000175 RID: 373
	internal class CsdlSemanticsModel : EdmModelBase, IEdmCheckable
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x00015FB0 File Offset: 0x000141B0
		public CsdlSemanticsModel(CsdlModel astModel, EdmDirectValueAnnotationsManager annotationsManager, IEnumerable<IEdmModel> referencedModels) : base(referencedModels, annotationsManager)
		{
			this.astModel = astModel;
			foreach (CsdlSchema schema in this.astModel.Schemata)
			{
				this.AddSchema(schema);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x000164B8 File Offset: 0x000146B8
		public override IEnumerable<IEdmSchemaElement> SchemaElements
		{
			get
			{
				foreach (CsdlSemanticsSchema schema in this.schemata)
				{
					foreach (IEdmSchemaType type in schema.Types)
					{
						yield return type;
					}
					foreach (IEdmSchemaElement function in schema.Functions)
					{
						yield return function;
					}
					foreach (IEdmSchemaElement valueTerm in schema.ValueTerms)
					{
						yield return valueTerm;
					}
					foreach (IEdmEntityContainer entityContainer in schema.EntityContainers)
					{
						yield return entityContainer;
					}
				}
				yield break;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x000164D8 File Offset: 0x000146D8
		public override IEnumerable<IEdmVocabularyAnnotation> VocabularyAnnotations
		{
			get
			{
				List<IEdmVocabularyAnnotation> list = new List<IEdmVocabularyAnnotation>();
				foreach (CsdlSemanticsSchema csdlSemanticsSchema in this.schemata)
				{
					foreach (CsdlAnnotations csdlAnnotations in ((CsdlSchema)csdlSemanticsSchema.Element).OutOfLineAnnotations)
					{
						CsdlSemanticsAnnotations annotationsContext = new CsdlSemanticsAnnotations(csdlSemanticsSchema, csdlAnnotations);
						foreach (CsdlVocabularyAnnotationBase annotation in csdlAnnotations.Annotations)
						{
							IEdmVocabularyAnnotation edmVocabularyAnnotation = this.WrapVocabularyAnnotation(annotation, csdlSemanticsSchema, null, annotationsContext, csdlAnnotations.Qualifier);
							edmVocabularyAnnotation.SetSerializationLocation(this, new EdmVocabularyAnnotationSerializationLocation?(EdmVocabularyAnnotationSerializationLocation.OutOfLine));
							edmVocabularyAnnotation.SetSchemaNamespace(this, csdlSemanticsSchema.Namespace);
							list.Add(edmVocabularyAnnotation);
						}
					}
				}
				foreach (IEdmSchemaElement edmSchemaElement in this.SchemaElements)
				{
					list.AddRange(((CsdlSemanticsElement)edmSchemaElement).InlineVocabularyAnnotations);
					CsdlSemanticsStructuredTypeDefinition csdlSemanticsStructuredTypeDefinition = edmSchemaElement as CsdlSemanticsStructuredTypeDefinition;
					if (csdlSemanticsStructuredTypeDefinition != null)
					{
						foreach (IEdmProperty edmProperty in csdlSemanticsStructuredTypeDefinition.DeclaredProperties)
						{
							list.AddRange(((CsdlSemanticsElement)edmProperty).InlineVocabularyAnnotations);
						}
					}
					CsdlSemanticsFunction csdlSemanticsFunction = edmSchemaElement as CsdlSemanticsFunction;
					if (csdlSemanticsFunction != null)
					{
						foreach (IEdmFunctionParameter edmFunctionParameter in csdlSemanticsFunction.Parameters)
						{
							list.AddRange(((CsdlSemanticsElement)edmFunctionParameter).InlineVocabularyAnnotations);
						}
					}
					CsdlSemanticsEntityContainer csdlSemanticsEntityContainer = edmSchemaElement as CsdlSemanticsEntityContainer;
					if (csdlSemanticsEntityContainer != null)
					{
						foreach (IEdmEntityContainerElement edmEntityContainerElement in csdlSemanticsEntityContainer.Elements)
						{
							list.AddRange(((CsdlSemanticsElement)edmEntityContainerElement).InlineVocabularyAnnotations);
							CsdlSemanticsFunctionImport csdlSemanticsFunctionImport = edmEntityContainerElement as CsdlSemanticsFunctionImport;
							if (csdlSemanticsFunctionImport != null)
							{
								foreach (IEdmFunctionParameter edmFunctionParameter2 in csdlSemanticsFunctionImport.Parameters)
								{
									list.AddRange(((CsdlSemanticsElement)edmFunctionParameter2).InlineVocabularyAnnotations);
								}
							}
						}
					}
				}
				return list;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00016814 File Offset: 0x00014A14
		public IEnumerable<EdmError> Errors
		{
			get
			{
				List<EdmError> list = new List<EdmError>();
				foreach (IEdmAssociation edmAssociation in this.associationDictionary.Values)
				{
					string text = edmAssociation.Namespace + "." + edmAssociation.Name;
					if (edmAssociation.IsBad())
					{
						AmbiguousAssociationBinding ambiguousAssociationBinding = edmAssociation as AmbiguousAssociationBinding;
						if (ambiguousAssociationBinding != null)
						{
							bool flag = true;
							using (IEnumerator<IEdmAssociation> enumerator2 = ambiguousAssociationBinding.Bindings.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									IEdmAssociation item = enumerator2.Current;
									if (flag)
									{
										flag = false;
									}
									else
									{
										list.Add(new EdmError(item.Location(), EdmErrorCode.AlreadyDefined, Strings.EdmModel_Validator_Semantic_SchemaElementNameAlreadyDefined(text)));
									}
								}
								continue;
							}
						}
						list.AddRange(edmAssociation.Errors());
					}
					else
					{
						if (base.FindDeclaredType(text) != null || base.FindDeclaredValueTerm(text) != null || base.FindDeclaredFunctions(text).Count<IEdmFunction>() != 0)
						{
							list.Add(new EdmError(edmAssociation.Location(), EdmErrorCode.AlreadyDefined, Strings.EdmModel_Validator_Semantic_SchemaElementNameAlreadyDefined(text)));
						}
						list.AddRange(edmAssociation.End1.Errors());
						list.AddRange(edmAssociation.End2.Errors());
						if (edmAssociation.ReferentialConstraint != null)
						{
							list.AddRange(edmAssociation.ReferentialConstraint.Errors());
						}
					}
				}
				foreach (CsdlSemanticsSchema element in this.schemata)
				{
					list.AddRange(element.Errors());
				}
				return list;
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000169F0 File Offset: 0x00014BF0
		public IEdmAssociation FindAssociation(string qualifiedName)
		{
			IEdmAssociation result;
			this.associationDictionary.TryGetValue(qualifiedName, out result);
			return result;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00016A10 File Offset: 0x00014C10
		public override IEnumerable<IEdmVocabularyAnnotation> FindDeclaredVocabularyAnnotations(IEdmVocabularyAnnotatable element)
		{
			CsdlSemanticsElement csdlSemanticsElement = element as CsdlSemanticsElement;
			IEnumerable<IEdmVocabularyAnnotation> enumerable = (csdlSemanticsElement != null && csdlSemanticsElement.Model == this) ? csdlSemanticsElement.InlineVocabularyAnnotations : Enumerable.Empty<IEdmVocabularyAnnotation>();
			string text = EdmUtil.FullyQualifiedName(element);
			List<CsdlSemanticsAnnotations> list;
			if (text != null && this.outOfLineAnnotations.TryGetValue(text, out list))
			{
				List<IEdmVocabularyAnnotation> list2 = new List<IEdmVocabularyAnnotation>();
				foreach (CsdlSemanticsAnnotations csdlSemanticsAnnotations in list)
				{
					foreach (CsdlVocabularyAnnotationBase annotation in csdlSemanticsAnnotations.Annotations.Annotations)
					{
						list2.Add(this.WrapVocabularyAnnotation(annotation, csdlSemanticsAnnotations.Context, null, csdlSemanticsAnnotations, csdlSemanticsAnnotations.Annotations.Qualifier));
					}
				}
				return enumerable.Concat(list2);
			}
			return enumerable;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00016B2C File Offset: 0x00014D2C
		public override IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType)
		{
			List<IEdmStructuredType> source;
			if (this.derivedTypeMappings.TryGetValue(((IEdmSchemaType)baseType).Name, out source))
			{
				return from t in source
				where t.BaseType == baseType
				select t;
			}
			return Enumerable.Empty<IEdmStructuredType>();
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00016B84 File Offset: 0x00014D84
		internal static IEdmExpression WrapExpression(CsdlExpressionBase expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema)
		{
			if (expression != null)
			{
				switch (expression.ExpressionKind)
				{
				case EdmExpressionKind.BinaryConstant:
					return new CsdlSemanticsBinaryConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.BooleanConstant:
					return new CsdlSemanticsBooleanConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.DateTimeConstant:
					return new CsdlSemanticsDateTimeConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.DateTimeOffsetConstant:
					return new CsdlSemanticsDateTimeOffsetConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.DecimalConstant:
					return new CsdlSemanticsDecimalConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.FloatingConstant:
					return new CsdlSemanticsFloatingConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.GuidConstant:
					return new CsdlSemanticsGuidConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.IntegerConstant:
					return new CsdlSemanticsIntConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.StringConstant:
					return new CsdlSemanticsStringConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.TimeConstant:
					return new CsdlSemanticsTimeConstantExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.Null:
					return new CsdlSemanticsNullExpression((CsdlConstantExpression)expression, schema);
				case EdmExpressionKind.Record:
					return new CsdlSemanticsRecordExpression((CsdlRecordExpression)expression, bindingContext, schema);
				case EdmExpressionKind.Collection:
					return new CsdlSemanticsCollectionExpression((CsdlCollectionExpression)expression, bindingContext, schema);
				case EdmExpressionKind.Path:
					return new CsdlSemanticsPathExpression((CsdlPathExpression)expression, bindingContext, schema);
				case EdmExpressionKind.ParameterReference:
					return new CsdlSemanticsParameterReferenceExpression((CsdlParameterReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.FunctionReference:
					return new CsdlSemanticsFunctionReferenceExpression((CsdlFunctionReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.PropertyReference:
					return new CsdlSemanticsPropertyReferenceExpression((CsdlPropertyReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.EntitySetReference:
					return new CsdlSemanticsEntitySetReferenceExpression((CsdlEntitySetReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.EnumMemberReference:
					return new CsdlSemanticsEnumMemberReferenceExpression((CsdlEnumMemberReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.If:
					return new CsdlSemanticsIfExpression((CsdlIfExpression)expression, bindingContext, schema);
				case EdmExpressionKind.AssertType:
					return new CsdlSemanticsAssertTypeExpression((CsdlAssertTypeExpression)expression, bindingContext, schema);
				case EdmExpressionKind.IsType:
					return new CsdlSemanticsIsTypeExpression((CsdlIsTypeExpression)expression, bindingContext, schema);
				case EdmExpressionKind.FunctionApplication:
					return new CsdlSemanticsApplyExpression((CsdlApplyExpression)expression, bindingContext, schema);
				case EdmExpressionKind.LabeledExpressionReference:
					return new CsdlSemanticsLabeledExpressionReferenceExpression((CsdlLabeledExpressionReferenceExpression)expression, bindingContext, schema);
				case EdmExpressionKind.Labeled:
					return schema.WrapLabeledElement((CsdlLabeledExpression)expression, bindingContext);
				}
			}
			return null;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00016D68 File Offset: 0x00014F68
		internal static IEdmTypeReference WrapTypeReference(CsdlSemanticsSchema schema, CsdlTypeReference type)
		{
			CsdlNamedTypeReference csdlNamedTypeReference = type as CsdlNamedTypeReference;
			if (csdlNamedTypeReference != null)
			{
				CsdlPrimitiveTypeReference csdlPrimitiveTypeReference = csdlNamedTypeReference as CsdlPrimitiveTypeReference;
				if (csdlPrimitiveTypeReference != null)
				{
					switch (csdlPrimitiveTypeReference.Kind)
					{
					case EdmPrimitiveTypeKind.Binary:
						return new CsdlSemanticsBinaryTypeReference(schema, (CsdlBinaryTypeReference)csdlPrimitiveTypeReference);
					case EdmPrimitiveTypeKind.Boolean:
					case EdmPrimitiveTypeKind.Byte:
					case EdmPrimitiveTypeKind.Double:
					case EdmPrimitiveTypeKind.Guid:
					case EdmPrimitiveTypeKind.Int16:
					case EdmPrimitiveTypeKind.Int32:
					case EdmPrimitiveTypeKind.Int64:
					case EdmPrimitiveTypeKind.SByte:
					case EdmPrimitiveTypeKind.Single:
					case EdmPrimitiveTypeKind.Stream:
						return new CsdlSemanticsPrimitiveTypeReference(schema, csdlPrimitiveTypeReference);
					case EdmPrimitiveTypeKind.DateTime:
					case EdmPrimitiveTypeKind.DateTimeOffset:
					case EdmPrimitiveTypeKind.Time:
						return new CsdlSemanticsTemporalTypeReference(schema, (CsdlTemporalTypeReference)csdlPrimitiveTypeReference);
					case EdmPrimitiveTypeKind.Decimal:
						return new CsdlSemanticsDecimalTypeReference(schema, (CsdlDecimalTypeReference)csdlPrimitiveTypeReference);
					case EdmPrimitiveTypeKind.String:
						return new CsdlSemanticsStringTypeReference(schema, (CsdlStringTypeReference)csdlPrimitiveTypeReference);
					case EdmPrimitiveTypeKind.Geography:
					case EdmPrimitiveTypeKind.GeographyPoint:
					case EdmPrimitiveTypeKind.GeographyLineString:
					case EdmPrimitiveTypeKind.GeographyPolygon:
					case EdmPrimitiveTypeKind.GeographyCollection:
					case EdmPrimitiveTypeKind.GeographyMultiPolygon:
					case EdmPrimitiveTypeKind.GeographyMultiLineString:
					case EdmPrimitiveTypeKind.GeographyMultiPoint:
					case EdmPrimitiveTypeKind.Geometry:
					case EdmPrimitiveTypeKind.GeometryPoint:
					case EdmPrimitiveTypeKind.GeometryLineString:
					case EdmPrimitiveTypeKind.GeometryPolygon:
					case EdmPrimitiveTypeKind.GeometryCollection:
					case EdmPrimitiveTypeKind.GeometryMultiPolygon:
					case EdmPrimitiveTypeKind.GeometryMultiLineString:
					case EdmPrimitiveTypeKind.GeometryMultiPoint:
						return new CsdlSemanticsSpatialTypeReference(schema, (CsdlSpatialTypeReference)csdlPrimitiveTypeReference);
					}
				}
				return new CsdlSemanticsNamedTypeReference(schema, csdlNamedTypeReference);
			}
			CsdlExpressionTypeReference csdlExpressionTypeReference = type as CsdlExpressionTypeReference;
			if (csdlExpressionTypeReference != null)
			{
				CsdlRowType csdlRowType = csdlExpressionTypeReference.TypeExpression as CsdlRowType;
				if (csdlRowType != null)
				{
					return new CsdlSemanticsRowTypeExpression(csdlExpressionTypeReference, new CsdlSemanticsRowTypeDefinition(schema, csdlRowType));
				}
				CsdlCollectionType csdlCollectionType = csdlExpressionTypeReference.TypeExpression as CsdlCollectionType;
				if (csdlCollectionType != null)
				{
					return new CsdlSemanticsCollectionTypeExpression(csdlExpressionTypeReference, new CsdlSemanticsCollectionTypeDefinition(schema, csdlCollectionType));
				}
				CsdlEntityReferenceType csdlEntityReferenceType = csdlExpressionTypeReference.TypeExpression as CsdlEntityReferenceType;
				if (csdlEntityReferenceType != null)
				{
					return new CsdlSemanticsEntityReferenceTypeExpression(csdlExpressionTypeReference, new CsdlSemanticsEntityReferenceTypeDefinition(schema, csdlEntityReferenceType));
				}
			}
			return null;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00016EDC File Offset: 0x000150DC
		internal static IEdmAssociation CreateAmbiguousAssociationBinding(IEdmAssociation first, IEdmAssociation second)
		{
			AmbiguousAssociationBinding ambiguousAssociationBinding = first as AmbiguousAssociationBinding;
			if (ambiguousAssociationBinding != null)
			{
				ambiguousAssociationBinding.AddBinding(second);
				return ambiguousAssociationBinding;
			}
			return new AmbiguousAssociationBinding(first, second);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00016F04 File Offset: 0x00015104
		internal IEnumerable<IEdmVocabularyAnnotation> WrapInlineVocabularyAnnotations(CsdlSemanticsElement element, CsdlSemanticsSchema schema)
		{
			IEdmVocabularyAnnotatable edmVocabularyAnnotatable = element as IEdmVocabularyAnnotatable;
			if (edmVocabularyAnnotatable != null)
			{
				IEnumerable<CsdlVocabularyAnnotationBase> vocabularyAnnotations = element.Element.VocabularyAnnotations;
				if (vocabularyAnnotations.FirstOrDefault<CsdlVocabularyAnnotationBase>() != null)
				{
					List<IEdmVocabularyAnnotation> list = new List<IEdmVocabularyAnnotation>();
					foreach (CsdlVocabularyAnnotationBase csdlVocabularyAnnotationBase in vocabularyAnnotations)
					{
						IEdmVocabularyAnnotation edmVocabularyAnnotation = this.WrapVocabularyAnnotation(csdlVocabularyAnnotationBase, schema, edmVocabularyAnnotatable, null, csdlVocabularyAnnotationBase.Qualifier);
						edmVocabularyAnnotation.SetSerializationLocation(this, new EdmVocabularyAnnotationSerializationLocation?(EdmVocabularyAnnotationSerializationLocation.Inline));
						list.Add(edmVocabularyAnnotation);
					}
					return list;
				}
			}
			return Enumerable.Empty<IEdmVocabularyAnnotation>();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00016FA0 File Offset: 0x000151A0
		private IEdmVocabularyAnnotation WrapVocabularyAnnotation(CsdlVocabularyAnnotationBase annotation, CsdlSemanticsSchema schema, IEdmVocabularyAnnotatable targetContext, CsdlSemanticsAnnotations annotationsContext, string qualifier)
		{
			CsdlSemanticsVocabularyAnnotation csdlSemanticsVocabularyAnnotation;
			if (this.wrappedAnnotations.TryGetValue(annotation, out csdlSemanticsVocabularyAnnotation))
			{
				return csdlSemanticsVocabularyAnnotation;
			}
			CsdlValueAnnotation csdlValueAnnotation = annotation as CsdlValueAnnotation;
			csdlSemanticsVocabularyAnnotation = ((csdlValueAnnotation != null) ? new CsdlSemanticsValueAnnotation(schema, targetContext, annotationsContext, csdlValueAnnotation, qualifier) : new CsdlSemanticsTypeAnnotation(schema, targetContext, annotationsContext, (CsdlTypeAnnotation)annotation, qualifier));
			this.wrappedAnnotations[annotation] = csdlSemanticsVocabularyAnnotation;
			return csdlSemanticsVocabularyAnnotation;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00016FF8 File Offset: 0x000151F8
		private void AddSchema(CsdlSchema schema)
		{
			CsdlSemanticsSchema csdlSemanticsSchema = new CsdlSemanticsSchema(this, schema);
			this.schemata.Add(csdlSemanticsSchema);
			foreach (IEdmSchemaType edmSchemaType in csdlSemanticsSchema.Types)
			{
				CsdlSemanticsStructuredTypeDefinition csdlSemanticsStructuredTypeDefinition = edmSchemaType as CsdlSemanticsStructuredTypeDefinition;
				if (csdlSemanticsStructuredTypeDefinition != null)
				{
					string baseTypeName = ((CsdlNamedStructuredType)csdlSemanticsStructuredTypeDefinition.Element).BaseTypeName;
					if (baseTypeName != null)
					{
						string text;
						string text2;
						EdmUtil.TryGetNamespaceNameFromQualifiedName(baseTypeName, out text, out text2);
						if (text2 != null)
						{
							List<IEdmStructuredType> list;
							if (!this.derivedTypeMappings.TryGetValue(text2, out list))
							{
								list = new List<IEdmStructuredType>();
								this.derivedTypeMappings[text2] = list;
							}
							list.Add(csdlSemanticsStructuredTypeDefinition);
						}
					}
				}
				base.RegisterElement(edmSchemaType);
			}
			foreach (CsdlSemanticsAssociation csdlSemanticsAssociation in csdlSemanticsSchema.Associations)
			{
				RegistrationHelper.AddElement<IEdmAssociation>(csdlSemanticsAssociation, csdlSemanticsAssociation.Namespace + "." + csdlSemanticsAssociation.Name, this.associationDictionary, new Func<IEdmAssociation, IEdmAssociation, IEdmAssociation>(CsdlSemanticsModel.CreateAmbiguousAssociationBinding));
			}
			foreach (IEdmFunction element in csdlSemanticsSchema.Functions)
			{
				base.RegisterElement(element);
			}
			foreach (IEdmValueTerm element2 in csdlSemanticsSchema.ValueTerms)
			{
				base.RegisterElement(element2);
			}
			foreach (IEdmEntityContainer element3 in csdlSemanticsSchema.EntityContainers)
			{
				base.RegisterElement(element3);
			}
			foreach (CsdlAnnotations csdlAnnotations in schema.OutOfLineAnnotations)
			{
				string text3 = csdlAnnotations.Target;
				string text4 = csdlSemanticsSchema.ReplaceAlias(text3);
				if (text4 != null)
				{
					text3 = text4;
				}
				List<CsdlSemanticsAnnotations> list2;
				if (!this.outOfLineAnnotations.TryGetValue(text3, out list2))
				{
					list2 = new List<CsdlSemanticsAnnotations>();
					this.outOfLineAnnotations[text3] = list2;
				}
				list2.Add(new CsdlSemanticsAnnotations(csdlSemanticsSchema, csdlAnnotations));
			}
			foreach (CsdlUsing csdlUsing in schema.Usings)
			{
				this.SetNamespaceAlias(csdlUsing.Namespace, csdlUsing.Alias);
			}
			Version edmVersion = this.GetEdmVersion();
			if (edmVersion == null || edmVersion < schema.Version)
			{
				this.SetEdmVersion(schema.Version);
			}
		}

		// Token: 0x04000411 RID: 1041
		private readonly CsdlModel astModel;

		// Token: 0x04000412 RID: 1042
		private readonly List<CsdlSemanticsSchema> schemata = new List<CsdlSemanticsSchema>();

		// Token: 0x04000413 RID: 1043
		private readonly Dictionary<string, List<CsdlSemanticsAnnotations>> outOfLineAnnotations = new Dictionary<string, List<CsdlSemanticsAnnotations>>();

		// Token: 0x04000414 RID: 1044
		private readonly Dictionary<CsdlVocabularyAnnotationBase, CsdlSemanticsVocabularyAnnotation> wrappedAnnotations = new Dictionary<CsdlVocabularyAnnotationBase, CsdlSemanticsVocabularyAnnotation>();

		// Token: 0x04000415 RID: 1045
		private readonly Dictionary<string, IEdmAssociation> associationDictionary = new Dictionary<string, IEdmAssociation>();

		// Token: 0x04000416 RID: 1046
		private readonly Dictionary<string, List<IEdmStructuredType>> derivedTypeMappings = new Dictionary<string, List<IEdmStructuredType>>();
	}
}
