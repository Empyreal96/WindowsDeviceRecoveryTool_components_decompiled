using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000E1 RID: 225
	internal sealed class UnaryOperatorBinder
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x000135DC File Offset: 0x000117DC
		internal UnaryOperatorBinder(Func<QueryToken, QueryNode> bindMethod)
		{
			this.bindMethod = bindMethod;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000135EC File Offset: 0x000117EC
		internal QueryNode BindUnaryOperator(UnaryOperatorToken unaryOperatorToken)
		{
			ExceptionUtils.CheckArgumentNotNull<UnaryOperatorToken>(unaryOperatorToken, "unaryOperatorToken");
			SingleValueNode singleValueNode = this.GetOperandFromToken(unaryOperatorToken);
			IEdmTypeReference targetTypeReference = UnaryOperatorBinder.PromoteOperandType(singleValueNode, unaryOperatorToken.OperatorKind);
			singleValueNode = MetadataBindingUtils.ConvertToTypeIfNeeded(singleValueNode, targetTypeReference);
			return new UnaryOperatorNode(unaryOperatorToken.OperatorKind, singleValueNode);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00013630 File Offset: 0x00011830
		private static IEdmTypeReference PromoteOperandType(SingleValueNode operand, UnaryOperatorKind unaryOperatorKind)
		{
			IEdmTypeReference typeReference = operand.TypeReference;
			if (!TypePromotionUtils.PromoteOperandType(unaryOperatorKind, ref typeReference))
			{
				string p = (operand.TypeReference == null) ? "<null>" : operand.TypeReference.ODataFullName();
				throw new ODataException(Strings.MetadataBinder_IncompatibleOperandError(p, unaryOperatorKind));
			}
			return typeReference;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001367C File Offset: 0x0001187C
		private SingleValueNode GetOperandFromToken(UnaryOperatorToken unaryOperatorToken)
		{
			SingleValueNode singleValueNode = this.bindMethod(unaryOperatorToken.Operand) as SingleValueNode;
			if (singleValueNode == null)
			{
				throw new ODataException(Strings.MetadataBinder_UnaryOperatorOperandNotSingleValue(unaryOperatorToken.OperatorKind.ToString()));
			}
			return singleValueNode;
		}

		// Token: 0x04000256 RID: 598
		private readonly Func<QueryToken, QueryNode> bindMethod;
	}
}
