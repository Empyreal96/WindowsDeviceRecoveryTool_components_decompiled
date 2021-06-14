using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200000A RID: 10
	internal sealed class BinaryOperatorBinder
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000027D6 File Offset: 0x000009D6
		internal BinaryOperatorBinder(Func<QueryToken, QueryNode> bindMethod)
		{
			this.bindMethod = bindMethod;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027E8 File Offset: 0x000009E8
		internal QueryNode BindBinaryOperator(BinaryOperatorToken binaryOperatorToken)
		{
			ExceptionUtils.CheckArgumentNotNull<BinaryOperatorToken>(binaryOperatorToken, "binaryOperatorToken");
			SingleValueNode operandFromToken = this.GetOperandFromToken(binaryOperatorToken.OperatorKind, binaryOperatorToken.Left);
			SingleValueNode operandFromToken2 = this.GetOperandFromToken(binaryOperatorToken.OperatorKind, binaryOperatorToken.Right);
			BinaryOperatorBinder.PromoteOperandTypes(binaryOperatorToken.OperatorKind, ref operandFromToken, ref operandFromToken2);
			return new BinaryOperatorNode(binaryOperatorToken.OperatorKind, operandFromToken, operandFromToken2);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002844 File Offset: 0x00000A44
		private static void PromoteOperandTypes(BinaryOperatorKind binaryOperatorKind, ref SingleValueNode left, ref SingleValueNode right)
		{
			IEdmTypeReference typeReference = left.TypeReference;
			IEdmTypeReference typeReference2 = right.TypeReference;
			if (!TypePromotionUtils.PromoteOperandTypes(binaryOperatorKind, ref typeReference, ref typeReference2))
			{
				string p = (left.TypeReference == null) ? "<null>" : left.TypeReference.ODataFullName();
				string p2 = (right.TypeReference == null) ? "<null>" : right.TypeReference.ODataFullName();
				throw new ODataException(Strings.MetadataBinder_IncompatibleOperandsError(p, p2, binaryOperatorKind));
			}
			left = MetadataBindingUtils.ConvertToTypeIfNeeded(left, typeReference);
			right = MetadataBindingUtils.ConvertToTypeIfNeeded(right, typeReference2);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028D0 File Offset: 0x00000AD0
		private SingleValueNode GetOperandFromToken(BinaryOperatorKind operatorKind, QueryToken queryToken)
		{
			SingleValueNode singleValueNode = this.bindMethod(queryToken) as SingleValueNode;
			if (singleValueNode == null)
			{
				throw new ODataException(Strings.MetadataBinder_BinaryOperatorOperandNotSingleValue(operatorKind.ToString()));
			}
			return singleValueNode;
		}

		// Token: 0x0400000D RID: 13
		private readonly Func<QueryToken, QueryNode> bindMethod;
	}
}
