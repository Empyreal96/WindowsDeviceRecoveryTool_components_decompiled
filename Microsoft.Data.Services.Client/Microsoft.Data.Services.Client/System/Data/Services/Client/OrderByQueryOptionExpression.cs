using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000CB RID: 203
	internal class OrderByQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x00019AA5 File Offset: 0x00017CA5
		internal OrderByQueryOptionExpression(Type type, List<OrderByQueryOptionExpression.Selector> selectors) : base(type)
		{
			this.selectors = selectors;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00019AB5 File Offset: 0x00017CB5
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10005;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00019ABC File Offset: 0x00017CBC
		internal List<OrderByQueryOptionExpression.Selector> Selectors
		{
			get
			{
				return this.selectors;
			}
		}

		// Token: 0x0400040D RID: 1037
		private List<OrderByQueryOptionExpression.Selector> selectors;

		// Token: 0x020000CC RID: 204
		internal struct Selector
		{
			// Token: 0x06000668 RID: 1640 RVA: 0x00019AC4 File Offset: 0x00017CC4
			internal Selector(Expression e, bool descending)
			{
				this.Expression = e;
				this.Descending = descending;
			}

			// Token: 0x0400040E RID: 1038
			internal readonly Expression Expression;

			// Token: 0x0400040F RID: 1039
			internal readonly bool Descending;
		}
	}
}
