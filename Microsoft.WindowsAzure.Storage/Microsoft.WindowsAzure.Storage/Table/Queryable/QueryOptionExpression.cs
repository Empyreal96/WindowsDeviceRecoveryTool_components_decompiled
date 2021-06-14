using System;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200010C RID: 268
	internal abstract class QueryOptionExpression : Expression
	{
		// Token: 0x060012D1 RID: 4817 RVA: 0x000465DD File Offset: 0x000447DD
		internal QueryOptionExpression(ExpressionType nodeType, Type type) : base(nodeType, type)
		{
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x000465E7 File Offset: 0x000447E7
		internal virtual QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
		{
			return this;
		}
	}
}
