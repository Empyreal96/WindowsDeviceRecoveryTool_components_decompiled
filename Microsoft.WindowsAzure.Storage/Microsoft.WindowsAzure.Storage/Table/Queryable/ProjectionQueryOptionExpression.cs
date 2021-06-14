using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000122 RID: 290
	internal class ProjectionQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x0600139D RID: 5021 RVA: 0x00049A3C File Offset: 0x00047C3C
		internal ProjectionQueryOptionExpression(Type type, LambdaExpression lambda, List<string> paths) : base((ExpressionType)10008, type)
		{
			this.lambda = lambda;
			this.paths = paths;
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00049A58 File Offset: 0x00047C58
		internal LambdaExpression Selector
		{
			get
			{
				return this.lambda;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00049A60 File Offset: 0x00047C60
		internal List<string> Paths
		{
			get
			{
				return this.paths;
			}
		}

		// Token: 0x040005C3 RID: 1475
		private readonly LambdaExpression lambda;

		// Token: 0x040005C4 RID: 1476
		private readonly List<string> paths;

		// Token: 0x040005C5 RID: 1477
		internal static readonly LambdaExpression DefaultLambda = Expression.Lambda(Expression.Constant(0), null);
	}
}
