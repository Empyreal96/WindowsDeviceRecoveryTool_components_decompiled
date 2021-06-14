using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.Serialization
{
	// Token: 0x020001B1 RID: 433
	internal sealed class EdmModelCsdlSerializationVisitor : EdmModelVisitor
	{
		// Token: 0x060009DE RID: 2526 RVA: 0x0001B834 File Offset: 0x00019A34
		internal EdmModelCsdlSerializationVisitor(IEdmModel model, XmlWriter xmlWriter, Version edmVersion) : base(model)
		{
			this.edmVersion = edmVersion;
			this.namespaceAliasMappings = model.GetNamespaceAliases();
			this.schemaWriter = new EdmModelCsdlSchemaWriter(model, this.namespaceAliasMappings, xmlWriter, this.edmVersion);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001B8B4 File Offset: 0x00019AB4
		internal void VisitEdmSchema(EdmSchema element, IEnumerable<KeyValuePair<string, string>> mappings)
		{
			string alias = null;
			if (this.namespaceAliasMappings != null)
			{
				this.namespaceAliasMappings.TryGetValue(element.Namespace, out alias);
			}
			this.schemaWriter.WriteSchemaElementHeader(element, alias, mappings);
			foreach (string text in element.NamespaceUsings)
			{
				if (text != element.Namespace && this.namespaceAliasMappings != null && this.namespaceAliasMappings.TryGetValue(text, out alias))
				{
					this.schemaWriter.WriteNamespaceUsingElement(text, alias);
				}
			}
			base.VisitSchemaElements(element.SchemaElements);
			using (List<IEdmNavigationProperty>.Enumerator enumerator2 = element.AssociationNavigationProperties.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IEdmNavigationProperty navigationProperty = enumerator2.Current;
					string associationFullName = this.Model.GetAssociationFullName(navigationProperty);
					List<IEdmNavigationProperty> list;
					if (!this.associations.TryGetValue(associationFullName, out list))
					{
						list = new List<IEdmNavigationProperty>();
						this.associations.Add(associationFullName, list);
					}
					if (!list.Any((IEdmNavigationProperty np) => this.SharesAssociation(np, navigationProperty)))
					{
						list.Add(navigationProperty);
						list.Add(navigationProperty.Partner);
						this.ProcessAssociation(navigationProperty);
					}
				}
			}
			EdmModelVisitor.VisitCollection<IEdmEntityContainer>(element.EntityContainers, new Action<IEdmEntityContainer>(this.ProcessEntityContainer));
			foreach (KeyValuePair<string, List<IEdmVocabularyAnnotation>> keyValuePair in element.OutOfLineAnnotations)
			{
				this.schemaWriter.WriteAnnotationsElementHeader(keyValuePair.Key);
				base.VisitVocabularyAnnotations(keyValuePair.Value);
				this.schemaWriter.WriteEndElement();
			}
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001BB0C File Offset: 0x00019D0C
		protected override void ProcessEntityContainer(IEdmEntityContainer element)
		{
			this.BeginElement<IEdmEntityContainer>(element, new Action<IEdmEntityContainer>(this.schemaWriter.WriteEntityContainerElementHeader), new Action<IEdmEntityContainer>[0]);
			base.ProcessEntityContainer(element);
			using (IEnumerator<IEdmEntitySet> enumerator = element.EntitySets().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IEdmEntitySet entitySet = enumerator.Current;
					using (IEnumerator<IEdmNavigationTargetMapping> enumerator2 = entitySet.NavigationTargets.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IEdmNavigationTargetMapping mapping = enumerator2.Current;
							string associationFullName = this.Model.GetAssociationFullName(mapping.NavigationProperty);
							List<TupleInternal<IEdmEntitySet, IEdmNavigationProperty>> list;
							if (!this.associationSets.TryGetValue(associationFullName, out list))
							{
								list = new List<TupleInternal<IEdmEntitySet, IEdmNavigationProperty>>();
								this.associationSets[associationFullName] = list;
							}
							if (!list.Any((TupleInternal<IEdmEntitySet, IEdmNavigationProperty> set) => this.SharesAssociationSet(set.Item1, set.Item2, entitySet, mapping.NavigationProperty)))
							{
								list.Add(new TupleInternal<IEdmEntitySet, IEdmNavigationProperty>(entitySet, mapping.NavigationProperty));
								list.Add(new TupleInternal<IEdmEntitySet, IEdmNavigationProperty>(mapping.TargetEntitySet, mapping.NavigationProperty.Partner));
								this.ProcessAssociationSet(entitySet, mapping.NavigationProperty);
							}
						}
					}
				}
			}
			this.associationSets.Clear();
			this.EndElement(element);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001BCCC File Offset: 0x00019ECC
		protected override void ProcessEntitySet(IEdmEntitySet element)
		{
			this.BeginElement<IEdmEntitySet>(element, new Action<IEdmEntitySet>(this.schemaWriter.WriteEntitySetElementHeader), new Action<IEdmEntitySet>[0]);
			base.ProcessEntitySet(element);
			this.EndElement(element);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001BCFC File Offset: 0x00019EFC
		protected override void ProcessEntityType(IEdmEntityType element)
		{
			this.BeginElement<IEdmEntityType>(element, new Action<IEdmEntityType>(this.schemaWriter.WriteEntityTypeElementHeader), new Action<IEdmEntityType>[0]);
			if (element.DeclaredKey != null && element.DeclaredKey.Count<IEdmStructuralProperty>() > 0 && element.BaseType == null)
			{
				this.VisitEntityTypeDeclaredKey(element.DeclaredKey);
			}
			base.VisitProperties(element.DeclaredStructuralProperties().Cast<IEdmProperty>());
			base.VisitProperties(element.DeclaredNavigationProperties().Cast<IEdmProperty>());
			this.EndElement(element);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		protected override void ProcessStructuralProperty(IEdmStructuralProperty element)
		{
			bool inlineType = EdmModelCsdlSerializationVisitor.IsInlineType(element.Type);
			this.BeginElement<IEdmStructuralProperty>(element, delegate(IEdmStructuralProperty t)
			{
				this.schemaWriter.WriteStructuralPropertyElementHeader(t, inlineType);
			}, new Action<IEdmStructuralProperty>[]
			{
				delegate(IEdmStructuralProperty e)
				{
					this.ProcessFacets(e.Type, inlineType);
				}
			});
			if (!inlineType)
			{
				base.VisitTypeReference(element.Type);
			}
			this.EndElement(element);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001BE24 File Offset: 0x0001A024
		protected override void ProcessBinaryTypeReference(IEdmBinaryTypeReference element)
		{
			this.schemaWriter.WriteBinaryTypeAttributes(element);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001BE32 File Offset: 0x0001A032
		protected override void ProcessDecimalTypeReference(IEdmDecimalTypeReference element)
		{
			this.schemaWriter.WriteDecimalTypeAttributes(element);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001BE40 File Offset: 0x0001A040
		protected override void ProcessSpatialTypeReference(IEdmSpatialTypeReference element)
		{
			this.schemaWriter.WriteSpatialTypeAttributes(element);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0001BE4E File Offset: 0x0001A04E
		protected override void ProcessStringTypeReference(IEdmStringTypeReference element)
		{
			this.schemaWriter.WriteStringTypeAttributes(element);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001BE5C File Offset: 0x0001A05C
		protected override void ProcessTemporalTypeReference(IEdmTemporalTypeReference element)
		{
			this.schemaWriter.WriteTemporalTypeAttributes(element);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0001BE6A File Offset: 0x0001A06A
		protected override void ProcessNavigationProperty(IEdmNavigationProperty element)
		{
			this.BeginElement<IEdmNavigationProperty>(element, new Action<IEdmNavigationProperty>(this.schemaWriter.WriteNavigationPropertyElementHeader), new Action<IEdmNavigationProperty>[0]);
			this.EndElement(element);
			this.navigationProperties.Add(element);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001BE9D File Offset: 0x0001A09D
		protected override void ProcessComplexType(IEdmComplexType element)
		{
			this.BeginElement<IEdmComplexType>(element, new Action<IEdmComplexType>(this.schemaWriter.WriteComplexTypeElementHeader), new Action<IEdmComplexType>[0]);
			base.ProcessComplexType(element);
			this.EndElement(element);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001BECB File Offset: 0x0001A0CB
		protected override void ProcessEnumType(IEdmEnumType element)
		{
			this.BeginElement<IEdmEnumType>(element, new Action<IEdmEnumType>(this.schemaWriter.WriteEnumTypeElementHeader), new Action<IEdmEnumType>[0]);
			base.ProcessEnumType(element);
			this.EndElement(element);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001BEF9 File Offset: 0x0001A0F9
		protected override void ProcessEnumMember(IEdmEnumMember element)
		{
			this.BeginElement<IEdmEnumMember>(element, new Action<IEdmEnumMember>(this.schemaWriter.WriteEnumMemberElementHeader), new Action<IEdmEnumMember>[0]);
			this.EndElement(element);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0001BF5C File Offset: 0x0001A15C
		protected override void ProcessValueTerm(IEdmValueTerm term)
		{
			bool inlineType = term.Type != null && EdmModelCsdlSerializationVisitor.IsInlineType(term.Type);
			this.BeginElement<IEdmValueTerm>(term, delegate(IEdmValueTerm t)
			{
				this.schemaWriter.WriteValueTermElementHeader(t, inlineType);
			}, new Action<IEdmValueTerm>[]
			{
				delegate(IEdmValueTerm e)
				{
					this.ProcessFacets(e.Type, inlineType);
				}
			});
			if (!inlineType && term.Type != null)
			{
				base.VisitTypeReference(term.Type);
			}
			this.EndElement(term);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0001C028 File Offset: 0x0001A228
		protected override void ProcessFunction(IEdmFunction element)
		{
			if (element.ReturnType != null)
			{
				bool inlineReturnType = EdmModelCsdlSerializationVisitor.IsInlineType(element.ReturnType);
				this.BeginElement<IEdmFunction>(element, delegate(IEdmFunction f)
				{
					this.schemaWriter.WriteFunctionElementHeader(f, inlineReturnType);
				}, new Action<IEdmFunction>[]
				{
					delegate(IEdmFunction f)
					{
						this.ProcessFacets(f.ReturnType, inlineReturnType);
					}
				});
				if (!inlineReturnType)
				{
					this.schemaWriter.WriteReturnTypeElementHeader();
					base.VisitTypeReference(element.ReturnType);
					this.schemaWriter.WriteEndElement();
				}
			}
			else
			{
				this.BeginElement<IEdmFunction>(element, delegate(IEdmFunction t)
				{
					this.schemaWriter.WriteFunctionElementHeader(t, false);
				}, new Action<IEdmFunction>[0]);
			}
			if (element.DefiningExpression != null)
			{
				this.schemaWriter.WriteDefiningExpressionElement(element.DefiningExpression);
			}
			base.VisitFunctionParameters(element.Parameters);
			this.EndElement(element);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0001C138 File Offset: 0x0001A338
		protected override void ProcessFunctionParameter(IEdmFunctionParameter element)
		{
			bool inlineType = EdmModelCsdlSerializationVisitor.IsInlineType(element.Type);
			this.BeginElement<IEdmFunctionParameter>(element, delegate(IEdmFunctionParameter t)
			{
				this.schemaWriter.WriteFunctionParameterElementHeader(t, inlineType);
			}, new Action<IEdmFunctionParameter>[]
			{
				delegate(IEdmFunctionParameter e)
				{
					this.ProcessFacets(e.Type, inlineType);
				}
			});
			if (!inlineType)
			{
				base.VisitTypeReference(element.Type);
			}
			this.EndElement(element);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
		protected override void ProcessCollectionType(IEdmCollectionType element)
		{
			bool inlineType = EdmModelCsdlSerializationVisitor.IsInlineType(element.ElementType);
			this.BeginElement<IEdmCollectionType>(element, delegate(IEdmCollectionType t)
			{
				this.schemaWriter.WriteCollectionTypeElementHeader(t, inlineType);
			}, new Action<IEdmCollectionType>[]
			{
				delegate(IEdmCollectionType e)
				{
					this.ProcessFacets(e.ElementType, inlineType);
				}
			});
			if (!inlineType)
			{
				base.VisitTypeReference(element.ElementType);
			}
			this.EndElement(element);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001C254 File Offset: 0x0001A454
		protected override void ProcessRowType(IEdmRowType element)
		{
			this.schemaWriter.WriteRowTypeElementHeader();
			base.ProcessRowType(element);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001C274 File Offset: 0x0001A474
		protected override void ProcessFunctionImport(IEdmFunctionImport functionImport)
		{
			if (functionImport.ReturnType != null && !EdmModelCsdlSerializationVisitor.IsInlineType(functionImport.ReturnType))
			{
				throw new InvalidOperationException(Strings.Serializer_NonInlineFunctionImportReturnType(functionImport.Container.FullName() + "/" + functionImport.Name));
			}
			this.BeginElement<IEdmFunctionImport>(functionImport, new Action<IEdmFunctionImport>(this.schemaWriter.WriteFunctionImportElementHeader), new Action<IEdmFunctionImport>[0]);
			base.VisitFunctionParameters(functionImport.Parameters);
			this.EndElement(functionImport);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001C310 File Offset: 0x0001A510
		protected override void ProcessValueAnnotation(IEdmValueAnnotation annotation)
		{
			bool isInline = EdmModelCsdlSerializationVisitor.IsInlineExpression(annotation.Value);
			this.BeginElement<IEdmValueAnnotation>(annotation, delegate(IEdmValueAnnotation t)
			{
				this.schemaWriter.WriteValueAnnotationElementHeader(t, isInline);
			}, new Action<IEdmValueAnnotation>[0]);
			if (!isInline)
			{
				base.ProcessValueAnnotation(annotation);
			}
			this.EndElement(annotation);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001C36A File Offset: 0x0001A56A
		protected override void ProcessTypeAnnotation(IEdmTypeAnnotation annotation)
		{
			this.BeginElement<IEdmTypeAnnotation>(annotation, new Action<IEdmTypeAnnotation>(this.schemaWriter.WriteTypeAnnotationElementHeader), new Action<IEdmTypeAnnotation>[0]);
			base.ProcessTypeAnnotation(annotation);
			this.EndElement(annotation);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001C3BC File Offset: 0x0001A5BC
		protected override void ProcessPropertyValueBinding(IEdmPropertyValueBinding binding)
		{
			bool isInline = EdmModelCsdlSerializationVisitor.IsInlineExpression(binding.Value);
			this.BeginElement<IEdmPropertyValueBinding>(binding, delegate(IEdmPropertyValueBinding t)
			{
				this.schemaWriter.WritePropertyValueElementHeader(t, isInline);
			}, new Action<IEdmPropertyValueBinding>[0]);
			if (!isInline)
			{
				base.ProcessPropertyValueBinding(binding);
			}
			this.EndElement(binding);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001C416 File Offset: 0x0001A616
		protected override void ProcessStringConstantExpression(IEdmStringConstantExpression expression)
		{
			this.schemaWriter.WriteStringConstantExpressionElement(expression);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001C424 File Offset: 0x0001A624
		protected override void ProcessBinaryConstantExpression(IEdmBinaryConstantExpression expression)
		{
			this.schemaWriter.WriteBinaryConstantExpressionElement(expression);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0001C432 File Offset: 0x0001A632
		protected override void ProcessRecordExpression(IEdmRecordExpression expression)
		{
			this.BeginElement<IEdmRecordExpression>(expression, new Action<IEdmRecordExpression>(this.schemaWriter.WriteRecordExpressionElementHeader), new Action<IEdmRecordExpression>[0]);
			base.VisitPropertyConstructors(expression.Properties);
			this.EndElement(expression);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0001C465 File Offset: 0x0001A665
		protected override void ProcessLabeledExpression(IEdmLabeledExpression element)
		{
			if (element.Name == null)
			{
				base.ProcessLabeledExpression(element);
				return;
			}
			this.BeginElement<IEdmLabeledExpression>(element, new Action<IEdmLabeledExpression>(this.schemaWriter.WriteLabeledElementHeader), new Action<IEdmLabeledExpression>[0]);
			base.ProcessLabeledExpression(element);
			this.EndElement(element);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		protected override void ProcessPropertyConstructor(IEdmPropertyConstructor constructor)
		{
			bool isInline = EdmModelCsdlSerializationVisitor.IsInlineExpression(constructor.Value);
			this.BeginElement<IEdmPropertyConstructor>(constructor, delegate(IEdmPropertyConstructor t)
			{
				this.schemaWriter.WritePropertyConstructorElementHeader(t, isInline);
			}, new Action<IEdmPropertyConstructor>[0]);
			if (!isInline)
			{
				base.ProcessPropertyConstructor(constructor);
			}
			this.EndElement(constructor);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0001C51E File Offset: 0x0001A71E
		protected override void ProcessPropertyReferenceExpression(IEdmPropertyReferenceExpression expression)
		{
			this.BeginElement<IEdmPropertyReferenceExpression>(expression, new Action<IEdmPropertyReferenceExpression>(this.schemaWriter.WritePropertyReferenceExpressionElementHeader), new Action<IEdmPropertyReferenceExpression>[0]);
			if (expression.Base != null)
			{
				base.VisitExpression(expression.Base);
			}
			this.EndElement(expression);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0001C559 File Offset: 0x0001A759
		protected override void ProcessPathExpression(IEdmPathExpression expression)
		{
			this.schemaWriter.WritePathExpressionElement(expression);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001C567 File Offset: 0x0001A767
		protected override void ProcessParameterReferenceExpression(IEdmParameterReferenceExpression expression)
		{
			this.schemaWriter.WriteParameterReferenceExpressionElement(expression);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001C575 File Offset: 0x0001A775
		protected override void ProcessCollectionExpression(IEdmCollectionExpression expression)
		{
			this.BeginElement<IEdmCollectionExpression>(expression, new Action<IEdmCollectionExpression>(this.schemaWriter.WriteCollectionExpressionElementHeader), new Action<IEdmCollectionExpression>[0]);
			base.VisitExpressions(expression.Elements);
			this.EndElement(expression);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001C5E4 File Offset: 0x0001A7E4
		protected override void ProcessIsTypeExpression(IEdmIsTypeExpression expression)
		{
			bool inlineType = EdmModelCsdlSerializationVisitor.IsInlineType(expression.Type);
			this.BeginElement<IEdmIsTypeExpression>(expression, delegate(IEdmIsTypeExpression t)
			{
				this.schemaWriter.WriteIsTypeExpressionElementHeader(t, inlineType);
			}, new Action<IEdmIsTypeExpression>[]
			{
				delegate(IEdmIsTypeExpression e)
				{
					this.ProcessFacets(e.Type, inlineType);
				}
			});
			if (!inlineType)
			{
				base.VisitTypeReference(expression.Type);
			}
			base.VisitExpression(expression.Operand);
			this.EndElement(expression);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001C660 File Offset: 0x0001A860
		protected override void ProcessIntegerConstantExpression(IEdmIntegerConstantExpression expression)
		{
			this.schemaWriter.WriteIntegerConstantExpressionElement(expression);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001C66E File Offset: 0x0001A86E
		protected override void ProcessIfExpression(IEdmIfExpression expression)
		{
			this.BeginElement<IEdmIfExpression>(expression, new Action<IEdmIfExpression>(this.schemaWriter.WriteIfExpressionElementHeader), new Action<IEdmIfExpression>[0]);
			base.ProcessIfExpression(expression);
			this.EndElement(expression);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001C69C File Offset: 0x0001A89C
		protected override void ProcessFunctionReferenceExpression(IEdmFunctionReferenceExpression expression)
		{
			this.schemaWriter.WriteFunctionReferenceExpressionElement(expression);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001C6CC File Offset: 0x0001A8CC
		protected override void ProcessFunctionApplicationExpression(IEdmApplyExpression expression)
		{
			bool isFunction = expression.AppliedFunction.ExpressionKind == EdmExpressionKind.FunctionReference;
			this.BeginElement<IEdmApplyExpression>(expression, delegate(IEdmApplyExpression e)
			{
				this.schemaWriter.WriteFunctionApplicationElementHeader(e, isFunction);
			}, new Action<IEdmApplyExpression>[0]);
			if (!isFunction)
			{
				base.VisitExpression(expression.AppliedFunction);
			}
			base.VisitExpressions(expression.Arguments);
			this.EndElement(expression);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001C73B File Offset: 0x0001A93B
		protected override void ProcessFloatingConstantExpression(IEdmFloatingConstantExpression expression)
		{
			this.schemaWriter.WriteFloatingConstantExpressionElement(expression);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001C749 File Offset: 0x0001A949
		protected override void ProcessGuidConstantExpression(IEdmGuidConstantExpression expression)
		{
			this.schemaWriter.WriteGuidConstantExpressionElement(expression);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001C757 File Offset: 0x0001A957
		protected override void ProcessEnumMemberReferenceExpression(IEdmEnumMemberReferenceExpression expression)
		{
			this.schemaWriter.WriteEnumMemberReferenceExpressionElement(expression);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0001C765 File Offset: 0x0001A965
		protected override void ProcessEntitySetReferenceExpression(IEdmEntitySetReferenceExpression expression)
		{
			this.schemaWriter.WriteEntitySetReferenceExpressionElement(expression);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001C773 File Offset: 0x0001A973
		protected override void ProcessDecimalConstantExpression(IEdmDecimalConstantExpression expression)
		{
			this.schemaWriter.WriteDecimalConstantExpressionElement(expression);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001C781 File Offset: 0x0001A981
		protected override void ProcessDateTimeConstantExpression(IEdmDateTimeConstantExpression expression)
		{
			this.schemaWriter.WriteDateTimeConstantExpressionElement(expression);
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001C78F File Offset: 0x0001A98F
		protected override void ProcessDateTimeOffsetConstantExpression(IEdmDateTimeOffsetConstantExpression expression)
		{
			this.schemaWriter.WriteDateTimeOffsetConstantExpressionElement(expression);
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001C79D File Offset: 0x0001A99D
		protected override void ProcessBooleanConstantExpression(IEdmBooleanConstantExpression expression)
		{
			this.schemaWriter.WriteBooleanConstantExpressionElement(expression);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001C7AB File Offset: 0x0001A9AB
		protected override void ProcessNullConstantExpression(IEdmNullExpression expression)
		{
			this.schemaWriter.WriteNullConstantExpressionElement(expression);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
		protected override void ProcessAssertTypeExpression(IEdmAssertTypeExpression expression)
		{
			bool inlineType = EdmModelCsdlSerializationVisitor.IsInlineType(expression.Type);
			this.BeginElement<IEdmAssertTypeExpression>(expression, delegate(IEdmAssertTypeExpression t)
			{
				this.schemaWriter.WriteAssertTypeExpressionElementHeader(t, inlineType);
			}, new Action<IEdmAssertTypeExpression>[]
			{
				delegate(IEdmAssertTypeExpression e)
				{
					this.ProcessFacets(e.Type, inlineType);
				}
			});
			if (!inlineType)
			{
				base.VisitTypeReference(expression.Type);
			}
			base.VisitExpression(expression.Operand);
			this.EndElement(expression);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0001C870 File Offset: 0x0001AA70
		private static bool IsInlineType(IEdmTypeReference reference)
		{
			return reference.Definition is IEdmSchemaElement || reference.IsEntityReference() || (reference.IsCollection() && reference.AsCollection().CollectionDefinition().ElementType.Definition is IEdmSchemaElement);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
		private static bool IsInlineExpression(IEdmExpression expression)
		{
			switch (expression.ExpressionKind)
			{
			case EdmExpressionKind.BinaryConstant:
			case EdmExpressionKind.BooleanConstant:
			case EdmExpressionKind.DateTimeConstant:
			case EdmExpressionKind.DateTimeOffsetConstant:
			case EdmExpressionKind.DecimalConstant:
			case EdmExpressionKind.FloatingConstant:
			case EdmExpressionKind.GuidConstant:
			case EdmExpressionKind.IntegerConstant:
			case EdmExpressionKind.StringConstant:
			case EdmExpressionKind.TimeConstant:
			case EdmExpressionKind.Path:
				return true;
			}
			return false;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0001C90C File Offset: 0x0001AB0C
		private void ProcessAnnotations(IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			this.VisitAttributeAnnotations(annotations);
			foreach (IEdmDirectValueAnnotation edmDirectValueAnnotation in annotations)
			{
				if (edmDirectValueAnnotation.NamespaceUri == "http://schemas.microsoft.com/ado/2011/04/edm/documentation" && edmDirectValueAnnotation.Name == "Documentation")
				{
					this.ProcessEdmDocumentation((IEdmDocumentation)edmDirectValueAnnotation.Value);
				}
			}
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0001C98C File Offset: 0x0001AB8C
		private void ProcessAssociation(IEdmNavigationProperty element)
		{
			IEdmNavigationProperty primary = element.GetPrimary();
			IEdmNavigationProperty partner = primary.Partner;
			IEnumerable<IEdmDirectValueAnnotation> annotations;
			IEnumerable<IEdmDirectValueAnnotation> enumerable;
			IEnumerable<IEdmDirectValueAnnotation> enumerable2;
			IEnumerable<IEdmDirectValueAnnotation> annotations2;
			this.Model.GetAssociationAnnotations(element, out annotations, out enumerable, out enumerable2, out annotations2);
			this.schemaWriter.WriteAssociationElementHeader(primary);
			this.ProcessAnnotations(annotations);
			this.ProcessAssociationEnd(primary, (primary == element) ? enumerable : enumerable2);
			this.ProcessAssociationEnd(partner, (primary == element) ? enumerable2 : enumerable);
			this.ProcessReferentialConstraint(primary, annotations2);
			this.VisitPrimitiveElementAnnotations(annotations);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001CA0C File Offset: 0x0001AC0C
		private void ProcessAssociationEnd(IEdmNavigationProperty element, IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			this.schemaWriter.WriteAssociationEndElementHeader(element);
			this.ProcessAnnotations(annotations);
			if (element.OnDelete != EdmOnDeleteAction.None)
			{
				this.schemaWriter.WriteOperationActionElement("OnDelete", element.OnDelete);
			}
			this.VisitPrimitiveElementAnnotations(annotations);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		private void ProcessReferentialConstraint(IEdmNavigationProperty element, IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			IEdmNavigationProperty edmNavigationProperty;
			if (element.DependentProperties != null)
			{
				edmNavigationProperty = element.Partner;
			}
			else
			{
				if (element.Partner.DependentProperties == null)
				{
					return;
				}
				edmNavigationProperty = element;
			}
			this.schemaWriter.WriteReferentialConstraintElementHeader(edmNavigationProperty);
			this.ProcessAnnotations(annotations);
			this.schemaWriter.WriteReferentialConstraintPrincipalEndElementHeader(edmNavigationProperty);
			this.VisitPropertyRefs(((IEdmEntityType)edmNavigationProperty.DeclaringType).Key());
			this.schemaWriter.WriteEndElement();
			this.schemaWriter.WriteReferentialConstraintDependentEndElementHeader(edmNavigationProperty.Partner);
			this.VisitPropertyRefs(edmNavigationProperty.Partner.DependentProperties);
			this.schemaWriter.WriteEndElement();
			this.VisitPrimitiveElementAnnotations(annotations);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		private void ProcessAssociationSet(IEdmEntitySet entitySet, IEdmNavigationProperty property)
		{
			IEnumerable<IEdmDirectValueAnnotation> annotations;
			IEnumerable<IEdmDirectValueAnnotation> annotations2;
			IEnumerable<IEdmDirectValueAnnotation> annotations3;
			this.Model.GetAssociationSetAnnotations(entitySet, property, out annotations, out annotations2, out annotations3);
			this.schemaWriter.WriteAssociationSetElementHeader(entitySet, property);
			this.ProcessAnnotations(annotations);
			this.ProcessAssociationSetEnd(entitySet, property, annotations2);
			IEdmEntitySet edmEntitySet = entitySet.FindNavigationTarget(property);
			if (edmEntitySet != null)
			{
				this.ProcessAssociationSetEnd(edmEntitySet, property.Partner, annotations3);
			}
			this.VisitPrimitiveElementAnnotations(annotations);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0001CB74 File Offset: 0x0001AD74
		private void ProcessAssociationSetEnd(IEdmEntitySet entitySet, IEdmNavigationProperty property, IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			this.schemaWriter.WriteAssociationSetEndElementHeader(entitySet, property);
			this.ProcessAnnotations(annotations);
			this.VisitPrimitiveElementAnnotations(annotations);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		private void ProcessFacets(IEdmTypeReference element, bool inlineType)
		{
			if (element != null)
			{
				if (element.IsEntityReference())
				{
					return;
				}
				if (inlineType)
				{
					if (element.TypeKind() == EdmTypeKind.Collection)
					{
						IEdmCollectionTypeReference type = element.AsCollection();
						this.schemaWriter.WriteNullableAttribute(type.CollectionDefinition().ElementType);
						base.VisitTypeReference(type.CollectionDefinition().ElementType);
						return;
					}
					this.schemaWriter.WriteNullableAttribute(element);
					base.VisitTypeReference(element);
				}
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0001CC03 File Offset: 0x0001AE03
		private void VisitEntityTypeDeclaredKey(IEnumerable<IEdmStructuralProperty> keyProperties)
		{
			this.schemaWriter.WriteDelaredKeyPropertiesElementHeader();
			this.VisitPropertyRefs(keyProperties);
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0001CC24 File Offset: 0x0001AE24
		private void VisitPropertyRefs(IEnumerable<IEdmStructuralProperty> properties)
		{
			foreach (IEdmStructuralProperty property in properties)
			{
				this.schemaWriter.WritePropertyRefElement(property);
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0001CC74 File Offset: 0x0001AE74
		private void VisitAttributeAnnotations(IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			foreach (IEdmDirectValueAnnotation edmDirectValueAnnotation in annotations)
			{
				if (edmDirectValueAnnotation.NamespaceUri != "http://schemas.microsoft.com/ado/2011/04/edm/internal")
				{
					IEdmValue edmValue = edmDirectValueAnnotation.Value as IEdmValue;
					if (edmValue != null && !edmValue.IsSerializedAsElement(this.Model) && edmValue.Type.TypeKind() == EdmTypeKind.Primitive)
					{
						this.ProcessAttributeAnnotation(edmDirectValueAnnotation);
					}
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001CCFC File Offset: 0x0001AEFC
		private void VisitPrimitiveElementAnnotations(IEnumerable<IEdmDirectValueAnnotation> annotations)
		{
			foreach (IEdmDirectValueAnnotation edmDirectValueAnnotation in annotations)
			{
				if (edmDirectValueAnnotation.NamespaceUri != "http://schemas.microsoft.com/ado/2011/04/edm/internal")
				{
					IEdmValue edmValue = edmDirectValueAnnotation.Value as IEdmValue;
					if (edmValue != null && edmValue.IsSerializedAsElement(this.Model) && edmValue.Type.TypeKind() == EdmTypeKind.Primitive)
					{
						this.ProcessElementAnnotation(edmDirectValueAnnotation);
					}
				}
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001CD84 File Offset: 0x0001AF84
		private void ProcessAttributeAnnotation(IEdmDirectValueAnnotation annotation)
		{
			this.schemaWriter.WriteAnnotationStringAttribute(annotation);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001CD92 File Offset: 0x0001AF92
		private void ProcessElementAnnotation(IEdmDirectValueAnnotation annotation)
		{
			this.schemaWriter.WriteAnnotationStringElement(annotation);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001CDA0 File Offset: 0x0001AFA0
		private void VisitElementVocabularyAnnotations(IEnumerable<IEdmVocabularyAnnotation> annotations)
		{
			foreach (IEdmVocabularyAnnotation edmVocabularyAnnotation in annotations)
			{
				switch (edmVocabularyAnnotation.Term.TermKind)
				{
				case EdmTermKind.None:
					this.ProcessVocabularyAnnotation(edmVocabularyAnnotation);
					break;
				case EdmTermKind.Type:
					this.ProcessTypeAnnotation((IEdmTypeAnnotation)edmVocabularyAnnotation);
					break;
				case EdmTermKind.Value:
					this.ProcessValueAnnotation((IEdmValueAnnotation)edmVocabularyAnnotation);
					break;
				default:
					throw new InvalidOperationException(Strings.UnknownEnumVal_TermKind(edmVocabularyAnnotation.Term.TermKind));
				}
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001CE44 File Offset: 0x0001B044
		private void BeginElement<TElement>(TElement element, Action<TElement> elementHeaderWriter, params Action<TElement>[] additionalAttributeWriters) where TElement : IEdmElement
		{
			elementHeaderWriter(element);
			if (additionalAttributeWriters != null)
			{
				foreach (Action<TElement> action in additionalAttributeWriters)
				{
					action(element);
				}
			}
			this.VisitAttributeAnnotations(this.Model.DirectValueAnnotations(element));
			IEdmDocumentation documentation = this.Model.GetDocumentation(element);
			if (documentation != null)
			{
				this.ProcessEdmDocumentation(documentation);
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001CEB8 File Offset: 0x0001B0B8
		private void EndElement(IEdmElement element)
		{
			this.VisitPrimitiveElementAnnotations(this.Model.DirectValueAnnotations(element));
			IEdmVocabularyAnnotatable edmVocabularyAnnotatable = element as IEdmVocabularyAnnotatable;
			if (edmVocabularyAnnotatable != null)
			{
				this.VisitElementVocabularyAnnotations(from a in this.Model.FindDeclaredVocabularyAnnotations(edmVocabularyAnnotatable)
				where a.IsInline(this.Model)
				select a);
			}
			this.schemaWriter.WriteEndElement();
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001CF16 File Offset: 0x0001B116
		private void ProcessEdmDocumentation(IEdmDocumentation element)
		{
			this.schemaWriter.WriteDocumentationElement(element);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001CF24 File Offset: 0x0001B124
		private bool SharesAssociation(IEdmNavigationProperty thisNavprop, IEdmNavigationProperty thatNavprop)
		{
			if (thisNavprop == thatNavprop)
			{
				return true;
			}
			if (this.Model.GetAssociationName(thisNavprop) != this.Model.GetAssociationName(thatNavprop))
			{
				return false;
			}
			IEdmNavigationProperty primary = thisNavprop.GetPrimary();
			IEdmNavigationProperty primary2 = thatNavprop.GetPrimary();
			if (!this.SharesEnd(primary, primary2))
			{
				return false;
			}
			IEdmNavigationProperty partner = primary.Partner;
			IEdmNavigationProperty partner2 = primary2.Partner;
			if (!this.SharesEnd(partner, partner2))
			{
				return false;
			}
			IEnumerable<IEdmStructuralProperty> theseProperties = ((IEdmEntityType)primary.DeclaringType).Key();
			IEnumerable<IEdmStructuralProperty> thoseProperties = ((IEdmEntityType)primary2.DeclaringType).Key();
			if (!this.SharesReferentialConstraintEnd(theseProperties, thoseProperties))
			{
				return false;
			}
			IEnumerable<IEdmStructuralProperty> dependentProperties = partner.DependentProperties;
			IEnumerable<IEdmStructuralProperty> dependentProperties2 = partner.DependentProperties;
			if (dependentProperties != null && dependentProperties2 != null && !this.SharesReferentialConstraintEnd(dependentProperties, dependentProperties2))
			{
				return false;
			}
			IEnumerable<IEdmDirectValueAnnotation> enumerable;
			IEnumerable<IEdmDirectValueAnnotation> enumerable2;
			IEnumerable<IEdmDirectValueAnnotation> enumerable3;
			IEnumerable<IEdmDirectValueAnnotation> enumerable4;
			this.Model.GetAssociationAnnotations(primary, out enumerable, out enumerable2, out enumerable3, out enumerable4);
			IEnumerable<IEdmDirectValueAnnotation> enumerable5;
			IEnumerable<IEdmDirectValueAnnotation> enumerable6;
			IEnumerable<IEdmDirectValueAnnotation> enumerable7;
			IEnumerable<IEdmDirectValueAnnotation> enumerable8;
			this.Model.GetAssociationAnnotations(primary2, out enumerable5, out enumerable6, out enumerable7, out enumerable8);
			return enumerable == enumerable5 && enumerable2 == enumerable6 && enumerable3 == enumerable7 && enumerable4 == enumerable8;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001D028 File Offset: 0x0001B228
		private bool SharesEnd(IEdmNavigationProperty end1, IEdmNavigationProperty end2)
		{
			return ((IEdmEntityType)end1.DeclaringType).FullName() == ((IEdmEntityType)end2.DeclaringType).FullName() && this.Model.GetAssociationEndName(end1) == this.Model.GetAssociationEndName(end2) && end1.Multiplicity() == end2.Multiplicity() && end1.OnDelete == end2.OnDelete;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001D09C File Offset: 0x0001B29C
		private bool SharesReferentialConstraintEnd(IEnumerable<IEdmStructuralProperty> theseProperties, IEnumerable<IEdmStructuralProperty> thoseProperties)
		{
			if (theseProperties.Count<IEdmStructuralProperty>() != thoseProperties.Count<IEdmStructuralProperty>())
			{
				return false;
			}
			IEnumerator<IEdmStructuralProperty> enumerator = theseProperties.GetEnumerator();
			foreach (IEdmStructuralProperty edmStructuralProperty in thoseProperties)
			{
				enumerator.MoveNext();
				if (!(enumerator.Current.Name == edmStructuralProperty.Name))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001D11C File Offset: 0x0001B31C
		private bool SharesAssociationSet(IEdmEntitySet thisEntitySet, IEdmNavigationProperty thisNavprop, IEdmEntitySet thatEntitySet, IEdmNavigationProperty thatNavprop)
		{
			if (thisEntitySet == thatEntitySet && thisNavprop == thatNavprop)
			{
				return true;
			}
			if (!(this.Model.GetAssociationSetName(thisEntitySet, thisNavprop) == this.Model.GetAssociationSetName(thatEntitySet, thatNavprop)) || !(this.Model.GetAssociationFullName(thisNavprop) == this.Model.GetAssociationFullName(thatNavprop)))
			{
				return false;
			}
			if (!(this.Model.GetAssociationEndName(thisNavprop) == this.Model.GetAssociationEndName(thatNavprop)) || !(thisEntitySet.Name == thatEntitySet.Name))
			{
				return false;
			}
			IEdmEntitySet edmEntitySet = thisEntitySet.FindNavigationTarget(thisNavprop);
			IEdmEntitySet edmEntitySet2 = thatEntitySet.FindNavigationTarget(thatNavprop);
			if (edmEntitySet == null)
			{
				if (edmEntitySet2 != null)
				{
					return false;
				}
			}
			else
			{
				if (edmEntitySet2 == null)
				{
					return false;
				}
				if (!(this.Model.GetAssociationEndName(thisNavprop.Partner) == this.Model.GetAssociationEndName(thatNavprop.Partner)) || !(edmEntitySet.Name == edmEntitySet2.Name))
				{
					return false;
				}
			}
			IEnumerable<IEdmDirectValueAnnotation> enumerable;
			IEnumerable<IEdmDirectValueAnnotation> enumerable2;
			IEnumerable<IEdmDirectValueAnnotation> enumerable3;
			this.Model.GetAssociationSetAnnotations(thisEntitySet, thisNavprop, out enumerable, out enumerable2, out enumerable3);
			IEnumerable<IEdmDirectValueAnnotation> enumerable4;
			IEnumerable<IEdmDirectValueAnnotation> enumerable5;
			IEnumerable<IEdmDirectValueAnnotation> enumerable6;
			this.Model.GetAssociationSetAnnotations(thatEntitySet, thatNavprop, out enumerable4, out enumerable5, out enumerable6);
			return enumerable == enumerable4 && enumerable2 == enumerable5 && enumerable3 == enumerable6;
		}

		// Token: 0x040004A9 RID: 1193
		private readonly Version edmVersion;

		// Token: 0x040004AA RID: 1194
		private readonly EdmModelCsdlSchemaWriter schemaWriter;

		// Token: 0x040004AB RID: 1195
		private readonly List<IEdmNavigationProperty> navigationProperties = new List<IEdmNavigationProperty>();

		// Token: 0x040004AC RID: 1196
		private readonly Dictionary<string, List<IEdmNavigationProperty>> associations = new Dictionary<string, List<IEdmNavigationProperty>>();

		// Token: 0x040004AD RID: 1197
		private readonly Dictionary<string, List<TupleInternal<IEdmEntitySet, IEdmNavigationProperty>>> associationSets = new Dictionary<string, List<TupleInternal<IEdmEntitySet, IEdmNavigationProperty>>>();

		// Token: 0x040004AE RID: 1198
		private readonly VersioningDictionary<string, string> namespaceAliasMappings;
	}
}
