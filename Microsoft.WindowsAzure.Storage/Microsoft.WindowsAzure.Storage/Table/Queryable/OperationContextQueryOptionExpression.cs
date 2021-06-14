using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011D RID: 285
	[DebuggerDisplay("OperationContextQueryOptionExpression {operationContext}")]
	internal class OperationContextQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x06001365 RID: 4965 RVA: 0x000488A1 File Offset: 0x00046AA1
		internal OperationContextQueryOptionExpression(Type type, ConstantExpression operationContext) : base((ExpressionType)10010, type)
		{
			this.operationContext = operationContext;
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x000488B6 File Offset: 0x00046AB6
		internal ConstantExpression OperationContext
		{
			get
			{
				return this.operationContext;
			}
		}

		// Token: 0x040005B3 RID: 1459
		private ConstantExpression operationContext;
	}
}
