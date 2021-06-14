using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000BB RID: 187
	internal class ProjectionQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x000171C6 File Offset: 0x000153C6
		internal ProjectionQueryOptionExpression(Type type, LambdaExpression lambda, List<string> paths) : base(type)
		{
			this.lambda = lambda;
			this.paths = paths;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x000171DD File Offset: 0x000153DD
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10008;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x000171E4 File Offset: 0x000153E4
		internal LambdaExpression Selector
		{
			get
			{
				return this.lambda;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x000171EC File Offset: 0x000153EC
		internal List<string> Paths
		{
			get
			{
				return this.paths;
			}
		}

		// Token: 0x04000333 RID: 819
		private readonly LambdaExpression lambda;

		// Token: 0x04000334 RID: 820
		private readonly List<string> paths;
	}
}
