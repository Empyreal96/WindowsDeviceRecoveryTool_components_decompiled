using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000042 RID: 66
	internal sealed class InnerPathTokenBinder
	{
		// Token: 0x0600019F RID: 415 RVA: 0x000072A8 File Offset: 0x000054A8
		internal InnerPathTokenBinder(MetadataBinder.QueryTokenVisitor bindMethod)
		{
			this.bindMethod = bindMethod;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000072B8 File Offset: 0x000054B8
		internal static SingleEntityNode EnsureParentIsEntityForNavProp(SingleValueNode parent)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(parent, "parent");
			SingleEntityNode singleEntityNode = parent as SingleEntityNode;
			if (singleEntityNode == null)
			{
				throw new ODataException(Strings.MetadataBinder_NavigationPropertyNotFollowingSingleEntityType);
			}
			return singleEntityNode;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000072E8 File Offset: 0x000054E8
		internal static IEdmProperty BindProperty(IEdmTypeReference parentReference, string propertyName)
		{
			IEdmStructuredTypeReference edmStructuredTypeReference = (parentReference == null) ? null : parentReference.AsStructuredOrNull();
			if (edmStructuredTypeReference != null)
			{
				return edmStructuredTypeReference.FindProperty(propertyName);
			}
			return null;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007310 File Offset: 0x00005510
		internal static QueryNode GetNavigationNode(IEdmNavigationProperty property, SingleEntityNode parent, IEnumerable<NamedValue> namedValues, BindingState state, KeyBinder keyBinder)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(property, "property");
			ExceptionUtils.CheckArgumentNotNull<SingleEntityNode>(parent, "parent");
			ExceptionUtils.CheckArgumentNotNull<BindingState>(state, "state");
			ExceptionUtils.CheckArgumentNotNull<KeyBinder>(keyBinder, "keyBinder");
			if (property.TargetMultiplicityTemporary() != EdmMultiplicity.Many)
			{
				return new SingleNavigationNode(property, parent);
			}
			CollectionNavigationNode collectionNavigationNode = new CollectionNavigationNode(property, parent);
			if (namedValues != null)
			{
				return keyBinder.BindKeyValues(collectionNavigationNode, namedValues);
			}
			return collectionNavigationNode;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007374 File Offset: 0x00005574
		internal QueryNode BindInnerPathSegment(InnerPathToken segmentToken, BindingState state)
		{
			FunctionCallBinder functionCallBinder = new FunctionCallBinder(this.bindMethod);
			QueryNode queryNode = this.DetermineParentNode(segmentToken, state);
			SingleValueNode singleValueNode = queryNode as SingleValueNode;
			if (singleValueNode == null)
			{
				QueryNode result;
				if (functionCallBinder.TryBindInnerPathAsFunctionCall(segmentToken, queryNode, state, out result))
				{
					return result;
				}
				throw new ODataException(Strings.MetadataBinder_PropertyAccessSourceNotSingleValue(segmentToken.Identifier));
			}
			else
			{
				IEdmProperty edmProperty = InnerPathTokenBinder.BindProperty(singleValueNode.TypeReference, segmentToken.Identifier);
				if (edmProperty == null)
				{
					QueryNode result2;
					if (functionCallBinder.TryBindInnerPathAsFunctionCall(segmentToken, queryNode, state, out result2))
					{
						return result2;
					}
					if (singleValueNode.TypeReference != null && !singleValueNode.TypeReference.Definition.IsOpenType())
					{
						throw new ODataException(Strings.MetadataBinder_PropertyNotDeclared(queryNode.GetEdmTypeReference().ODataFullName(), segmentToken.Identifier));
					}
					return new SingleValueOpenPropertyAccessNode(singleValueNode, segmentToken.Identifier);
				}
				else
				{
					if (edmProperty.Type.IsODataComplexTypeKind())
					{
						return new SingleValuePropertyAccessNode(singleValueNode, edmProperty);
					}
					if (edmProperty.Type.IsNonEntityCollectionType())
					{
						return new CollectionPropertyAccessNode(singleValueNode, edmProperty);
					}
					IEdmNavigationProperty edmNavigationProperty = edmProperty as IEdmNavigationProperty;
					if (edmNavigationProperty == null)
					{
						throw new ODataException(Strings.MetadataBinder_IllegalSegmentType(edmProperty.Name));
					}
					SingleEntityNode parent = InnerPathTokenBinder.EnsureParentIsEntityForNavProp(singleValueNode);
					return InnerPathTokenBinder.GetNavigationNode(edmNavigationProperty, parent, segmentToken.NamedValues, state, new KeyBinder(this.bindMethod));
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000749C File Offset: 0x0000569C
		private QueryNode DetermineParentNode(InnerPathToken segmentToken, BindingState state)
		{
			ExceptionUtils.CheckArgumentNotNull<InnerPathToken>(segmentToken, "segmentToken");
			ExceptionUtils.CheckArgumentNotNull<BindingState>(state, "state");
			if (segmentToken.NextToken != null)
			{
				return this.bindMethod(segmentToken.NextToken);
			}
			RangeVariable implicitRangeVariable = state.ImplicitRangeVariable;
			return NodeFactory.CreateRangeVariableReferenceNode(implicitRangeVariable);
		}

		// Token: 0x04000073 RID: 115
		private readonly MetadataBinder.QueryTokenVisitor bindMethod;
	}
}
