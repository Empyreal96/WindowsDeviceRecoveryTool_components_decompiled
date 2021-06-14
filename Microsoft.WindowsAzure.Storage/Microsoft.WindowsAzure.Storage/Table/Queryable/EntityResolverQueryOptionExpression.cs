using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200010D RID: 269
	[DebuggerDisplay("EntityResolverQueryOptionExpression {requestOptions}")]
	internal class EntityResolverQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x000465EA File Offset: 0x000447EA
		internal EntityResolverQueryOptionExpression(Type type, ConstantExpression resolver) : base((ExpressionType)10011, type)
		{
			this.resolver = resolver;
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x000465FF File Offset: 0x000447FF
		internal ConstantExpression Resolver
		{
			get
			{
				return this.resolver;
			}
		}

		// Token: 0x04000586 RID: 1414
		private ConstantExpression resolver;
	}
}
