using System;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000BA RID: 186
	internal abstract class QueryOptionExpression : Expression
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x000171AC File Offset: 0x000153AC
		internal QueryOptionExpression(Type type)
		{
			this.type = type;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x000171BB File Offset: 0x000153BB
		public override Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000171C3 File Offset: 0x000153C3
		internal virtual QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
		{
			return this;
		}

		// Token: 0x04000332 RID: 818
		private Type type;
	}
}
