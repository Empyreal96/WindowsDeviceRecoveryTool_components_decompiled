using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000034 RID: 52
	internal sealed class LambdaBinder
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00006544 File Offset: 0x00004744
		internal LambdaBinder(MetadataBinder.QueryTokenVisitor bindMethod)
		{
			ExceptionUtils.CheckArgumentNotNull<MetadataBinder.QueryTokenVisitor>(bindMethod, "bindMethod");
			this.bindMethod = bindMethod;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006560 File Offset: 0x00004760
		internal LambdaNode BindLambdaToken(LambdaToken lambdaToken, BindingState state)
		{
			ExceptionUtils.CheckArgumentNotNull<LambdaToken>(lambdaToken, "LambdaToken");
			ExceptionUtils.CheckArgumentNotNull<BindingState>(state, "state");
			CollectionNode collectionNode = this.BindParentToken(lambdaToken.Parent);
			RangeVariable rangeVariable = null;
			if (lambdaToken.Parameter != null)
			{
				rangeVariable = NodeFactory.CreateParameterNode(lambdaToken.Parameter, collectionNode);
				state.RangeVariables.Push(rangeVariable);
			}
			SingleValueNode lambdaExpression = this.BindExpressionToken(lambdaToken.Expression);
			LambdaNode result = NodeFactory.CreateLambdaNode(state, collectionNode, lambdaExpression, rangeVariable, lambdaToken.Kind);
			if (rangeVariable != null)
			{
				state.RangeVariables.Pop();
			}
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000065E0 File Offset: 0x000047E0
		private CollectionNode BindParentToken(QueryToken queryToken)
		{
			QueryNode queryNode = this.bindMethod(queryToken);
			CollectionNode collectionNode = queryNode as CollectionNode;
			if (collectionNode != null)
			{
				return collectionNode;
			}
			if (!(queryNode is SingleValueOpenPropertyAccessNode))
			{
				throw new ODataException(Strings.MetadataBinder_LambdaParentMustBeCollection);
			}
			throw new ODataException(Strings.MetadataBinder_CollectionOpenPropertiesNotSupportedInThisRelease);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006628 File Offset: 0x00004828
		private SingleValueNode BindExpressionToken(QueryToken queryToken)
		{
			SingleValueNode singleValueNode = this.bindMethod(queryToken) as SingleValueNode;
			if (singleValueNode == null)
			{
				throw new ODataException(Strings.MetadataBinder_AnyAllExpressionNotSingleValue);
			}
			IEdmTypeReference edmTypeReference = singleValueNode.GetEdmTypeReference();
			if (edmTypeReference != null && !edmTypeReference.AsPrimitive().IsBoolean())
			{
				throw new ODataException(Strings.MetadataBinder_AnyAllExpressionNotSingleValue);
			}
			return singleValueNode;
		}

		// Token: 0x04000068 RID: 104
		private readonly MetadataBinder.QueryTokenVisitor bindMethod;
	}
}
