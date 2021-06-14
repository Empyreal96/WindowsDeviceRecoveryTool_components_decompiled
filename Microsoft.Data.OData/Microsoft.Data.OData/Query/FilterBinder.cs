using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200002A RID: 42
	internal sealed class FilterBinder
	{
		// Token: 0x06000110 RID: 272 RVA: 0x000053E0 File Offset: 0x000035E0
		internal FilterBinder(MetadataBinder.QueryTokenVisitor bindMethod, BindingState state)
		{
			this.bindMethod = bindMethod;
			this.state = state;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000053F8 File Offset: 0x000035F8
		internal FilterClause BindFilter(QueryToken filter)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryToken>(filter, "filter");
			QueryNode queryNode = this.bindMethod(filter);
			SingleValueNode singleValueNode = queryNode as SingleValueNode;
			if (singleValueNode == null || (singleValueNode.TypeReference != null && !singleValueNode.TypeReference.IsODataPrimitiveTypeKind()))
			{
				throw new ODataException(Strings.MetadataBinder_FilterExpressionNotSingleValue);
			}
			IEdmTypeReference typeReference = singleValueNode.TypeReference;
			if (typeReference != null)
			{
				IEdmPrimitiveTypeReference edmPrimitiveTypeReference = typeReference.AsPrimitiveOrNull();
				if (edmPrimitiveTypeReference == null || edmPrimitiveTypeReference.PrimitiveKind() != EdmPrimitiveTypeKind.Boolean)
				{
					throw new ODataException(Strings.MetadataBinder_FilterExpressionNotSingleValue);
				}
			}
			return new FilterClause(singleValueNode, this.state.ImplicitRangeVariable);
		}

		// Token: 0x04000057 RID: 87
		private readonly MetadataBinder.QueryTokenVisitor bindMethod;

		// Token: 0x04000058 RID: 88
		private readonly BindingState state;
	}
}
