using System;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000117 RID: 279
	internal class FilterQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x06001333 RID: 4915 RVA: 0x000480B0 File Offset: 0x000462B0
		internal FilterQueryOptionExpression(Type type, Expression predicate) : base((ExpressionType)10006, type)
		{
			this.predicate = predicate;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x000480C5 File Offset: 0x000462C5
		internal Expression Predicate
		{
			get
			{
				return this.predicate;
			}
		}

		// Token: 0x0400059E RID: 1438
		private Expression predicate;
	}
}
