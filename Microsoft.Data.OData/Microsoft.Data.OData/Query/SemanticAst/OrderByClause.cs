using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200007B RID: 123
	public sealed class OrderByClause
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000ADFA File Offset: 0x00008FFA
		public OrderByClause(OrderByClause thenBy, SingleValueNode expression, OrderByDirection direction, RangeVariable rangeVariable)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(expression, "expression");
			ExceptionUtils.CheckArgumentNotNull<RangeVariable>(rangeVariable, "parameter");
			this.thenBy = thenBy;
			this.expression = expression;
			this.direction = direction;
			this.rangeVariable = rangeVariable;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000AE36 File Offset: 0x00009036
		public OrderByClause ThenBy
		{
			get
			{
				return this.thenBy;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000AE3E File Offset: 0x0000903E
		public SingleValueNode Expression
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000AE46 File Offset: 0x00009046
		public OrderByDirection Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000AE4E File Offset: 0x0000904E
		public RangeVariable RangeVariable
		{
			get
			{
				return this.rangeVariable;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000AE56 File Offset: 0x00009056
		public IEdmTypeReference ItemType
		{
			get
			{
				return this.RangeVariable.TypeReference;
			}
		}

		// Token: 0x040000D2 RID: 210
		private readonly SingleValueNode expression;

		// Token: 0x040000D3 RID: 211
		private readonly OrderByDirection direction;

		// Token: 0x040000D4 RID: 212
		private readonly RangeVariable rangeVariable;

		// Token: 0x040000D5 RID: 213
		private readonly OrderByClause thenBy;
	}
}
