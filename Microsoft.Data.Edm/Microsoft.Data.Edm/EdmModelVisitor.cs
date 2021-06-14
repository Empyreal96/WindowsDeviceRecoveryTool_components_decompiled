using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm
{
	// Token: 0x020000BF RID: 191
	internal abstract class EdmModelVisitor
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00008B50 File Offset: 0x00006D50
		protected EdmModelVisitor(IEdmModel model)
		{
			this.Model = model;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00008B5F File Offset: 0x00006D5F
		public void VisitEdmModel()
		{
			this.ProcessModel(this.Model);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00008B6D File Offset: 0x00006D6D
		public void VisitSchemaElements(IEnumerable<IEdmSchemaElement> elements)
		{
			EdmModelVisitor.VisitCollection<IEdmSchemaElement>(elements, new Action<IEdmSchemaElement>(this.VisitSchemaElement));
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00008B84 File Offset: 0x00006D84
		public void VisitSchemaElement(IEdmSchemaElement element)
		{
			switch (element.SchemaElementKind)
			{
			case EdmSchemaElementKind.None:
				this.ProcessSchemaElement(element);
				return;
			case EdmSchemaElementKind.TypeDefinition:
				this.VisitSchemaType((IEdmType)element);
				return;
			case EdmSchemaElementKind.Function:
				this.ProcessFunction((IEdmFunction)element);
				return;
			case EdmSchemaElementKind.ValueTerm:
				this.ProcessValueTerm((IEdmValueTerm)element);
				return;
			case EdmSchemaElementKind.EntityContainer:
				this.ProcessEntityContainer((IEdmEntityContainer)element);
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_SchemaElementKind(element.SchemaElementKind));
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00008C05 File Offset: 0x00006E05
		public void VisitAnnotations(IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			EdmModelVisitor.VisitCollection<IEdmDirectValueAnnotation>(annotations, new Action<IEdmDirectValueAnnotation>(this.VisitAnnotation));
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00008C19 File Offset: 0x00006E19
		public void VisitVocabularyAnnotations(IEnumerable<IEdmVocabularyAnnotation> annotations)
		{
			EdmModelVisitor.VisitCollection<IEdmVocabularyAnnotation>(annotations, new Action<IEdmVocabularyAnnotation>(this.VisitVocabularyAnnotation));
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00008C2D File Offset: 0x00006E2D
		public void VisitAnnotation(IEdmDirectValueAnnotation annotation)
		{
			this.ProcessImmediateValueAnnotation(annotation);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00008C38 File Offset: 0x00006E38
		public void VisitVocabularyAnnotation(IEdmVocabularyAnnotation annotation)
		{
			if (annotation.Term == null)
			{
				this.ProcessVocabularyAnnotation(annotation);
				return;
			}
			switch (annotation.Term.TermKind)
			{
			case EdmTermKind.None:
				this.ProcessVocabularyAnnotation(annotation);
				return;
			case EdmTermKind.Type:
				this.ProcessTypeAnnotation((IEdmTypeAnnotation)annotation);
				return;
			case EdmTermKind.Value:
				this.ProcessValueAnnotation((IEdmValueAnnotation)annotation);
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_TermKind(annotation.Term.TermKind));
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00008CB1 File Offset: 0x00006EB1
		public void VisitPropertyValueBindings(IEnumerable<IEdmPropertyValueBinding> bindings)
		{
			EdmModelVisitor.VisitCollection<IEdmPropertyValueBinding>(bindings, new Action<IEdmPropertyValueBinding>(this.ProcessPropertyValueBinding));
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00008CC6 File Offset: 0x00006EC6
		public void VisitExpressions(IEnumerable<IEdmExpression> expressions)
		{
			EdmModelVisitor.VisitCollection<IEdmExpression>(expressions, new Action<IEdmExpression>(this.VisitExpression));
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00008CDC File Offset: 0x00006EDC
		public void VisitExpression(IEdmExpression expression)
		{
			switch (expression.ExpressionKind)
			{
			case EdmExpressionKind.None:
				this.ProcessExpression(expression);
				return;
			case EdmExpressionKind.BinaryConstant:
				this.ProcessBinaryConstantExpression((IEdmBinaryConstantExpression)expression);
				return;
			case EdmExpressionKind.BooleanConstant:
				this.ProcessBooleanConstantExpression((IEdmBooleanConstantExpression)expression);
				return;
			case EdmExpressionKind.DateTimeConstant:
				this.ProcessDateTimeConstantExpression((IEdmDateTimeConstantExpression)expression);
				return;
			case EdmExpressionKind.DateTimeOffsetConstant:
				this.ProcessDateTimeOffsetConstantExpression((IEdmDateTimeOffsetConstantExpression)expression);
				return;
			case EdmExpressionKind.DecimalConstant:
				this.ProcessDecimalConstantExpression((IEdmDecimalConstantExpression)expression);
				return;
			case EdmExpressionKind.FloatingConstant:
				this.ProcessFloatingConstantExpression((IEdmFloatingConstantExpression)expression);
				return;
			case EdmExpressionKind.GuidConstant:
				this.ProcessGuidConstantExpression((IEdmGuidConstantExpression)expression);
				return;
			case EdmExpressionKind.IntegerConstant:
				this.ProcessIntegerConstantExpression((IEdmIntegerConstantExpression)expression);
				return;
			case EdmExpressionKind.StringConstant:
				this.ProcessStringConstantExpression((IEdmStringConstantExpression)expression);
				return;
			case EdmExpressionKind.TimeConstant:
				this.ProcessTimeConstantExpression((IEdmTimeConstantExpression)expression);
				return;
			case EdmExpressionKind.Null:
				this.ProcessNullConstantExpression((IEdmNullExpression)expression);
				return;
			case EdmExpressionKind.Record:
				this.ProcessRecordExpression((IEdmRecordExpression)expression);
				return;
			case EdmExpressionKind.Collection:
				this.ProcessCollectionExpression((IEdmCollectionExpression)expression);
				return;
			case EdmExpressionKind.Path:
				this.ProcessPathExpression((IEdmPathExpression)expression);
				return;
			case EdmExpressionKind.ParameterReference:
				this.ProcessParameterReferenceExpression((IEdmParameterReferenceExpression)expression);
				return;
			case EdmExpressionKind.FunctionReference:
				this.ProcessFunctionReferenceExpression((IEdmFunctionReferenceExpression)expression);
				return;
			case EdmExpressionKind.PropertyReference:
				this.ProcessPropertyReferenceExpression((IEdmPropertyReferenceExpression)expression);
				return;
			case EdmExpressionKind.ValueTermReference:
				this.ProcessPropertyReferenceExpression((IEdmPropertyReferenceExpression)expression);
				return;
			case EdmExpressionKind.EntitySetReference:
				this.ProcessEntitySetReferenceExpression((IEdmEntitySetReferenceExpression)expression);
				return;
			case EdmExpressionKind.EnumMemberReference:
				this.ProcessEnumMemberReferenceExpression((IEdmEnumMemberReferenceExpression)expression);
				return;
			case EdmExpressionKind.If:
				this.ProcessIfExpression((IEdmIfExpression)expression);
				return;
			case EdmExpressionKind.AssertType:
				this.ProcessAssertTypeExpression((IEdmAssertTypeExpression)expression);
				return;
			case EdmExpressionKind.IsType:
				this.ProcessIsTypeExpression((IEdmIsTypeExpression)expression);
				return;
			case EdmExpressionKind.FunctionApplication:
				this.ProcessFunctionApplicationExpression((IEdmApplyExpression)expression);
				return;
			case EdmExpressionKind.LabeledExpressionReference:
				this.ProcessLabeledExpressionReferenceExpression((IEdmLabeledExpressionReferenceExpression)expression);
				return;
			case EdmExpressionKind.Labeled:
				this.ProcessLabeledExpression((IEdmLabeledExpression)expression);
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_ExpressionKind(expression.ExpressionKind));
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00008ED6 File Offset: 0x000070D6
		public void VisitPropertyConstructors(IEnumerable<IEdmPropertyConstructor> constructor)
		{
			EdmModelVisitor.VisitCollection<IEdmPropertyConstructor>(constructor, new Action<IEdmPropertyConstructor>(this.ProcessPropertyConstructor));
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00008EEC File Offset: 0x000070EC
		public void VisitEntityContainerElements(IEnumerable<IEdmEntityContainerElement> elements)
		{
			foreach (IEdmEntityContainerElement edmEntityContainerElement in elements)
			{
				switch (edmEntityContainerElement.ContainerElementKind)
				{
				case EdmContainerElementKind.None:
					this.ProcessEntityContainerElement(edmEntityContainerElement);
					break;
				case EdmContainerElementKind.EntitySet:
					this.ProcessEntitySet((IEdmEntitySet)edmEntityContainerElement);
					break;
				case EdmContainerElementKind.FunctionImport:
					this.ProcessFunctionImport((IEdmFunctionImport)edmEntityContainerElement);
					break;
				default:
					throw new InvalidOperationException(Strings.UnknownEnumVal_ContainerElementKind(edmEntityContainerElement.ContainerElementKind.ToString()));
				}
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00008F88 File Offset: 0x00007188
		public void VisitTypeReference(IEdmTypeReference reference)
		{
			switch (reference.TypeKind())
			{
			case EdmTypeKind.None:
				this.ProcessTypeReference(reference);
				return;
			case EdmTypeKind.Primitive:
				this.VisitPrimitiveTypeReference(reference.AsPrimitive());
				return;
			case EdmTypeKind.Entity:
				this.ProcessEntityTypeReference(reference.AsEntity());
				return;
			case EdmTypeKind.Complex:
				this.ProcessComplexTypeReference(reference.AsComplex());
				return;
			case EdmTypeKind.Row:
				this.ProcessRowTypeReference(reference.AsRow());
				return;
			case EdmTypeKind.Collection:
				this.ProcessCollectionTypeReference(reference.AsCollection());
				return;
			case EdmTypeKind.EntityReference:
				this.ProcessEntityReferenceTypeReference(reference.AsEntityReference());
				return;
			case EdmTypeKind.Enum:
				this.ProcessEnumTypeReference(reference.AsEnum());
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_TypeKind(reference.TypeKind().ToString()));
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00009044 File Offset: 0x00007244
		public void VisitPrimitiveTypeReference(IEdmPrimitiveTypeReference reference)
		{
			switch (reference.PrimitiveKind())
			{
			case EdmPrimitiveTypeKind.None:
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
				this.ProcessPrimitiveTypeReference(reference);
				return;
			case EdmPrimitiveTypeKind.Binary:
				this.ProcessBinaryTypeReference(reference.AsBinary());
				return;
			case EdmPrimitiveTypeKind.DateTime:
			case EdmPrimitiveTypeKind.DateTimeOffset:
			case EdmPrimitiveTypeKind.Time:
				this.ProcessTemporalTypeReference(reference.AsTemporal());
				return;
			case EdmPrimitiveTypeKind.Decimal:
				this.ProcessDecimalTypeReference(reference.AsDecimal());
				return;
			case EdmPrimitiveTypeKind.String:
				this.ProcessStringTypeReference(reference.AsString());
				return;
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
				this.ProcessSpatialTypeReference(reference.AsSpatial());
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_PrimitiveKind(reference.PrimitiveKind().ToString()));
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00009148 File Offset: 0x00007348
		public void VisitSchemaType(IEdmType definition)
		{
			EdmTypeKind typeKind = definition.TypeKind;
			switch (typeKind)
			{
			case EdmTypeKind.None:
				this.VisitSchemaType(definition);
				return;
			case EdmTypeKind.Primitive:
				break;
			case EdmTypeKind.Entity:
				this.ProcessEntityType((IEdmEntityType)definition);
				return;
			case EdmTypeKind.Complex:
				this.ProcessComplexType((IEdmComplexType)definition);
				return;
			default:
				if (typeKind == EdmTypeKind.Enum)
				{
					this.ProcessEnumType((IEdmEnumType)definition);
					return;
				}
				break;
			}
			throw new InvalidOperationException(Strings.UnknownEnumVal_TypeKind(definition.TypeKind));
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000091BC File Offset: 0x000073BC
		public void VisitProperties(IEnumerable<IEdmProperty> properties)
		{
			EdmModelVisitor.VisitCollection<IEdmProperty>(properties, new Action<IEdmProperty>(this.VisitProperty));
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000091D0 File Offset: 0x000073D0
		public void VisitProperty(IEdmProperty property)
		{
			switch (property.PropertyKind)
			{
			case EdmPropertyKind.Structural:
				this.ProcessStructuralProperty((IEdmStructuralProperty)property);
				return;
			case EdmPropertyKind.Navigation:
				this.ProcessNavigationProperty((IEdmNavigationProperty)property);
				return;
			case EdmPropertyKind.None:
				this.ProcessProperty(property);
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_PropertyKind(property.PropertyKind.ToString()));
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00009234 File Offset: 0x00007434
		public void VisitEnumMembers(IEnumerable<IEdmEnumMember> enumMembers)
		{
			EdmModelVisitor.VisitCollection<IEdmEnumMember>(enumMembers, new Action<IEdmEnumMember>(this.VisitEnumMember));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00009248 File Offset: 0x00007448
		public void VisitEnumMember(IEdmEnumMember enumMember)
		{
			this.ProcessEnumMember(enumMember);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00009251 File Offset: 0x00007451
		public void VisitFunctionParameters(IEnumerable<IEdmFunctionParameter> parameters)
		{
			EdmModelVisitor.VisitCollection<IEdmFunctionParameter>(parameters, new Action<IEdmFunctionParameter>(this.ProcessFunctionParameter));
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00009268 File Offset: 0x00007468
		protected static void VisitCollection<T>(IEnumerable<T> collection, Action<T> visitMethod)
		{
			foreach (T obj in collection)
			{
				visitMethod(obj);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000092B0 File Offset: 0x000074B0
		protected virtual void ProcessModel(IEdmModel model)
		{
			this.ProcessElement(model);
			this.VisitSchemaElements(model.SchemaElements);
			this.VisitVocabularyAnnotations(model.VocabularyAnnotations);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000092D1 File Offset: 0x000074D1
		protected virtual void ProcessElement(IEdmElement element)
		{
			this.VisitAnnotations(this.Model.DirectValueAnnotations(element));
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000092E5 File Offset: 0x000074E5
		protected virtual void ProcessNamedElement(IEdmNamedElement element)
		{
			this.ProcessElement(element);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000092EE File Offset: 0x000074EE
		protected virtual void ProcessSchemaElement(IEdmSchemaElement element)
		{
			this.ProcessVocabularyAnnotatable(element);
			this.ProcessNamedElement(element);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000092FE File Offset: 0x000074FE
		protected virtual void ProcessVocabularyAnnotatable(IEdmVocabularyAnnotatable annotatable)
		{
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00009300 File Offset: 0x00007500
		protected virtual void ProcessComplexTypeReference(IEdmComplexTypeReference reference)
		{
			this.ProcessStructuredTypeReference(reference);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00009309 File Offset: 0x00007509
		protected virtual void ProcessEntityTypeReference(IEdmEntityTypeReference reference)
		{
			this.ProcessStructuredTypeReference(reference);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00009312 File Offset: 0x00007512
		protected virtual void ProcessEntityReferenceTypeReference(IEdmEntityReferenceTypeReference reference)
		{
			this.ProcessTypeReference(reference);
			this.ProcessEntityReferenceType(reference.EntityReferenceDefinition());
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00009327 File Offset: 0x00007527
		protected virtual void ProcessRowTypeReference(IEdmRowTypeReference reference)
		{
			this.ProcessStructuredTypeReference(reference);
			this.ProcessRowType(reference.RowDefinition());
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000933C File Offset: 0x0000753C
		protected virtual void ProcessCollectionTypeReference(IEdmCollectionTypeReference reference)
		{
			this.ProcessTypeReference(reference);
			this.ProcessCollectionType(reference.CollectionDefinition());
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00009351 File Offset: 0x00007551
		protected virtual void ProcessEnumTypeReference(IEdmEnumTypeReference reference)
		{
			this.ProcessTypeReference(reference);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000935A File Offset: 0x0000755A
		protected virtual void ProcessBinaryTypeReference(IEdmBinaryTypeReference reference)
		{
			this.ProcessPrimitiveTypeReference(reference);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00009363 File Offset: 0x00007563
		protected virtual void ProcessDecimalTypeReference(IEdmDecimalTypeReference reference)
		{
			this.ProcessPrimitiveTypeReference(reference);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000936C File Offset: 0x0000756C
		protected virtual void ProcessSpatialTypeReference(IEdmSpatialTypeReference reference)
		{
			this.ProcessPrimitiveTypeReference(reference);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00009375 File Offset: 0x00007575
		protected virtual void ProcessStringTypeReference(IEdmStringTypeReference reference)
		{
			this.ProcessPrimitiveTypeReference(reference);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000937E File Offset: 0x0000757E
		protected virtual void ProcessTemporalTypeReference(IEdmTemporalTypeReference reference)
		{
			this.ProcessPrimitiveTypeReference(reference);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00009387 File Offset: 0x00007587
		protected virtual void ProcessPrimitiveTypeReference(IEdmPrimitiveTypeReference reference)
		{
			this.ProcessTypeReference(reference);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00009390 File Offset: 0x00007590
		protected virtual void ProcessStructuredTypeReference(IEdmStructuredTypeReference reference)
		{
			this.ProcessTypeReference(reference);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00009399 File Offset: 0x00007599
		protected virtual void ProcessTypeReference(IEdmTypeReference element)
		{
			this.ProcessElement(element);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000093A2 File Offset: 0x000075A2
		protected virtual void ProcessTerm(IEdmTerm term)
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000093A4 File Offset: 0x000075A4
		protected virtual void ProcessValueTerm(IEdmValueTerm term)
		{
			this.ProcessSchemaElement(term);
			this.ProcessTerm(term);
			this.VisitTypeReference(term.Type);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000093C0 File Offset: 0x000075C0
		protected virtual void ProcessComplexType(IEdmComplexType definition)
		{
			this.ProcessSchemaElement(definition);
			this.ProcessStructuredType(definition);
			this.ProcessSchemaType(definition);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000093D7 File Offset: 0x000075D7
		protected virtual void ProcessEntityType(IEdmEntityType definition)
		{
			this.ProcessSchemaElement(definition);
			this.ProcessTerm(definition);
			this.ProcessStructuredType(definition);
			this.ProcessSchemaType(definition);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000093F5 File Offset: 0x000075F5
		protected virtual void ProcessRowType(IEdmRowType definition)
		{
			this.ProcessElement(definition);
			this.ProcessStructuredType(definition);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00009405 File Offset: 0x00007605
		protected virtual void ProcessCollectionType(IEdmCollectionType definition)
		{
			this.ProcessElement(definition);
			this.ProcessType(definition);
			this.VisitTypeReference(definition.ElementType);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00009421 File Offset: 0x00007621
		protected virtual void ProcessEnumType(IEdmEnumType definition)
		{
			this.ProcessSchemaElement(definition);
			this.ProcessType(definition);
			this.ProcessSchemaType(definition);
			this.VisitEnumMembers(definition.Members);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00009444 File Offset: 0x00007644
		protected virtual void ProcessEntityReferenceType(IEdmEntityReferenceType definition)
		{
			this.ProcessElement(definition);
			this.ProcessType(definition);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00009454 File Offset: 0x00007654
		protected virtual void ProcessStructuredType(IEdmStructuredType definition)
		{
			this.ProcessType(definition);
			this.VisitProperties(definition.DeclaredProperties);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00009469 File Offset: 0x00007669
		protected virtual void ProcessSchemaType(IEdmSchemaType type)
		{
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000946B File Offset: 0x0000766B
		protected virtual void ProcessType(IEdmType definition)
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000946D File Offset: 0x0000766D
		protected virtual void ProcessNavigationProperty(IEdmNavigationProperty property)
		{
			this.ProcessProperty(property);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00009476 File Offset: 0x00007676
		protected virtual void ProcessStructuralProperty(IEdmStructuralProperty property)
		{
			this.ProcessProperty(property);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000947F File Offset: 0x0000767F
		protected virtual void ProcessProperty(IEdmProperty property)
		{
			this.ProcessVocabularyAnnotatable(property);
			this.ProcessNamedElement(property);
			this.VisitTypeReference(property.Type);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000949B File Offset: 0x0000769B
		protected virtual void ProcessEnumMember(IEdmEnumMember enumMember)
		{
			this.ProcessNamedElement(enumMember);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000094A4 File Offset: 0x000076A4
		protected virtual void ProcessVocabularyAnnotation(IEdmVocabularyAnnotation annotation)
		{
			this.ProcessElement(annotation);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000094AD File Offset: 0x000076AD
		protected virtual void ProcessImmediateValueAnnotation(IEdmDirectValueAnnotation annotation)
		{
			this.ProcessNamedElement(annotation);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000094B6 File Offset: 0x000076B6
		protected virtual void ProcessValueAnnotation(IEdmValueAnnotation annotation)
		{
			this.ProcessVocabularyAnnotation(annotation);
			this.VisitExpression(annotation.Value);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000094CB File Offset: 0x000076CB
		protected virtual void ProcessTypeAnnotation(IEdmTypeAnnotation annotation)
		{
			this.ProcessVocabularyAnnotation(annotation);
			this.VisitPropertyValueBindings(annotation.PropertyValueBindings);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000094E0 File Offset: 0x000076E0
		protected virtual void ProcessPropertyValueBinding(IEdmPropertyValueBinding binding)
		{
			this.VisitExpression(binding.Value);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000094EE File Offset: 0x000076EE
		protected virtual void ProcessExpression(IEdmExpression expression)
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000094F0 File Offset: 0x000076F0
		protected virtual void ProcessStringConstantExpression(IEdmStringConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000094F9 File Offset: 0x000076F9
		protected virtual void ProcessBinaryConstantExpression(IEdmBinaryConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00009502 File Offset: 0x00007702
		protected virtual void ProcessRecordExpression(IEdmRecordExpression expression)
		{
			this.ProcessExpression(expression);
			if (expression.DeclaredType != null)
			{
				this.VisitTypeReference(expression.DeclaredType);
			}
			this.VisitPropertyConstructors(expression.Properties);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000952B File Offset: 0x0000772B
		protected virtual void ProcessPropertyReferenceExpression(IEdmPropertyReferenceExpression expression)
		{
			this.ProcessExpression(expression);
			if (expression.Base != null)
			{
				this.VisitExpression(expression.Base);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00009548 File Offset: 0x00007748
		protected virtual void ProcessPathExpression(IEdmPathExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00009551 File Offset: 0x00007751
		protected virtual void ProcessParameterReferenceExpression(IEdmParameterReferenceExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000955A File Offset: 0x0000775A
		protected virtual void ProcessCollectionExpression(IEdmCollectionExpression expression)
		{
			this.ProcessExpression(expression);
			this.VisitExpressions(expression.Elements);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000956F File Offset: 0x0000776F
		protected virtual void ProcessLabeledExpressionReferenceExpression(IEdmLabeledExpressionReferenceExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009578 File Offset: 0x00007778
		protected virtual void ProcessIsTypeExpression(IEdmIsTypeExpression expression)
		{
			this.ProcessExpression(expression);
			this.VisitTypeReference(expression.Type);
			this.VisitExpression(expression.Operand);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00009599 File Offset: 0x00007799
		protected virtual void ProcessIntegerConstantExpression(IEdmIntegerConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000095A2 File Offset: 0x000077A2
		protected virtual void ProcessIfExpression(IEdmIfExpression expression)
		{
			this.ProcessExpression(expression);
			this.VisitExpression(expression.TestExpression);
			this.VisitExpression(expression.TrueExpression);
			this.VisitExpression(expression.FalseExpression);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000095CF File Offset: 0x000077CF
		protected virtual void ProcessFunctionReferenceExpression(IEdmFunctionReferenceExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000095D8 File Offset: 0x000077D8
		protected virtual void ProcessFunctionApplicationExpression(IEdmApplyExpression expression)
		{
			this.ProcessExpression(expression);
			this.VisitExpression(expression.AppliedFunction);
			this.VisitExpressions(expression.Arguments);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000095F9 File Offset: 0x000077F9
		protected virtual void ProcessFloatingConstantExpression(IEdmFloatingConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00009602 File Offset: 0x00007802
		protected virtual void ProcessGuidConstantExpression(IEdmGuidConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000960B File Offset: 0x0000780B
		protected virtual void ProcessEnumMemberReferenceExpression(IEdmEnumMemberReferenceExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00009614 File Offset: 0x00007814
		protected virtual void ProcessEntitySetReferenceExpression(IEdmEntitySetReferenceExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000961D File Offset: 0x0000781D
		protected virtual void ProcessDecimalConstantExpression(IEdmDecimalConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00009626 File Offset: 0x00007826
		protected virtual void ProcessDateTimeConstantExpression(IEdmDateTimeConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000962F File Offset: 0x0000782F
		protected virtual void ProcessDateTimeOffsetConstantExpression(IEdmDateTimeOffsetConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00009638 File Offset: 0x00007838
		protected virtual void ProcessTimeConstantExpression(IEdmTimeConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00009641 File Offset: 0x00007841
		protected virtual void ProcessBooleanConstantExpression(IEdmBooleanConstantExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000964A File Offset: 0x0000784A
		protected virtual void ProcessAssertTypeExpression(IEdmAssertTypeExpression expression)
		{
			this.ProcessExpression(expression);
			this.VisitTypeReference(expression.Type);
			this.VisitExpression(expression.Operand);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000966B File Offset: 0x0000786B
		protected virtual void ProcessLabeledExpression(IEdmLabeledExpression element)
		{
			this.VisitExpression(element.Expression);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00009679 File Offset: 0x00007879
		protected virtual void ProcessPropertyConstructor(IEdmPropertyConstructor constructor)
		{
			this.VisitExpression(constructor.Value);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00009687 File Offset: 0x00007887
		protected virtual void ProcessNullConstantExpression(IEdmNullExpression expression)
		{
			this.ProcessExpression(expression);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00009690 File Offset: 0x00007890
		protected virtual void ProcessEntityContainer(IEdmEntityContainer container)
		{
			this.ProcessVocabularyAnnotatable(container);
			this.ProcessNamedElement(container);
			this.VisitEntityContainerElements(container.Elements);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x000096AC File Offset: 0x000078AC
		protected virtual void ProcessEntityContainerElement(IEdmEntityContainerElement element)
		{
			this.ProcessNamedElement(element);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000096B5 File Offset: 0x000078B5
		protected virtual void ProcessEntitySet(IEdmEntitySet set)
		{
			this.ProcessEntityContainerElement(set);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000096BE File Offset: 0x000078BE
		protected virtual void ProcessFunction(IEdmFunction function)
		{
			this.ProcessSchemaElement(function);
			this.ProcessFunctionBase(function);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000096CE File Offset: 0x000078CE
		protected virtual void ProcessFunctionImport(IEdmFunctionImport functionImport)
		{
			this.ProcessEntityContainerElement(functionImport);
			this.ProcessFunctionBase(functionImport);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000096DE File Offset: 0x000078DE
		protected virtual void ProcessFunctionBase(IEdmFunctionBase functionBase)
		{
			if (functionBase.ReturnType != null)
			{
				this.VisitTypeReference(functionBase.ReturnType);
			}
			this.VisitFunctionParameters(functionBase.Parameters);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00009700 File Offset: 0x00007900
		protected virtual void ProcessFunctionParameter(IEdmFunctionParameter parameter)
		{
			this.ProcessVocabularyAnnotatable(parameter);
			this.ProcessNamedElement(parameter);
			this.VisitTypeReference(parameter.Type);
		}

		// Token: 0x0400017A RID: 378
		protected readonly IEdmModel Model;
	}
}
