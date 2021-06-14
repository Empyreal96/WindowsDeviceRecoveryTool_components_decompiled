using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200007A RID: 122
	public sealed class FilterClause
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000ADB1 File Offset: 0x00008FB1
		public FilterClause(SingleValueNode expression, RangeVariable rangeVariable)
		{
			ExceptionUtils.CheckArgumentNotNull<SingleValueNode>(expression, "expression");
			ExceptionUtils.CheckArgumentNotNull<RangeVariable>(rangeVariable, "parameter");
			this.expression = expression;
			this.rangeVariable = rangeVariable;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000ADDD File Offset: 0x00008FDD
		public SingleValueNode Expression
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000ADE5 File Offset: 0x00008FE5
		public RangeVariable RangeVariable
		{
			get
			{
				return this.rangeVariable;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000ADED File Offset: 0x00008FED
		public IEdmTypeReference ItemType
		{
			get
			{
				return this.RangeVariable.TypeReference;
			}
		}

		// Token: 0x040000D0 RID: 208
		private readonly SingleValueNode expression;

		// Token: 0x040000D1 RID: 209
		private readonly RangeVariable rangeVariable;
	}
}
