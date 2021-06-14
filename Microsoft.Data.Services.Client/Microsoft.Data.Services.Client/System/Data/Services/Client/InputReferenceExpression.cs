using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000BF RID: 191
	[DebuggerDisplay("InputReferenceExpression -> {Type}")]
	internal sealed class InputReferenceExpression : Expression
	{
		// Token: 0x0600061E RID: 1566 RVA: 0x0001868E File Offset: 0x0001688E
		internal InputReferenceExpression(ResourceExpression target)
		{
			this.target = target;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001869D File Offset: 0x0001689D
		public override Type Type
		{
			get
			{
				return this.target.ResourceType;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000186AA File Offset: 0x000168AA
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10007;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000186B1 File Offset: 0x000168B1
		internal ResourceExpression Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000186B9 File Offset: 0x000168B9
		internal void OverrideTarget(ResourceSetExpression newTarget)
		{
			this.target = newTarget;
		}

		// Token: 0x040003EE RID: 1006
		private ResourceExpression target;
	}
}
