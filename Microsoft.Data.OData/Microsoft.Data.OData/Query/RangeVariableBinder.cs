using System;
using System.Linq;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000058 RID: 88
	internal sealed class RangeVariableBinder
	{
		// Token: 0x0600023B RID: 571 RVA: 0x0000866C File Offset: 0x0000686C
		internal static SingleValueNode BindRangeVariableToken(RangeVariableToken rangeVariableToken, BindingState state)
		{
			ExceptionUtils.CheckArgumentNotNull<RangeVariableToken>(rangeVariableToken, "rangeVariableToken");
			RangeVariable rangeVariable = state.RangeVariables.SingleOrDefault((RangeVariable p) => p.Name == rangeVariableToken.Name);
			if (rangeVariable == null)
			{
				throw new ODataException(Strings.MetadataBinder_ParameterNotInScope(rangeVariableToken.Name));
			}
			return NodeFactory.CreateRangeVariableReferenceNode(rangeVariable);
		}
	}
}
