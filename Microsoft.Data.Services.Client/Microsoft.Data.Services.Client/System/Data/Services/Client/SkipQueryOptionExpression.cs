using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000D9 RID: 217
	[DebuggerDisplay("SkipQueryOptionExpression {SkipAmount}")]
	internal class SkipQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x0001CC2D File Offset: 0x0001AE2D
		internal SkipQueryOptionExpression(Type type, ConstantExpression skipAmount) : base(type)
		{
			this.skipAmount = skipAmount;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001CC3D File Offset: 0x0001AE3D
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10004;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001CC44 File Offset: 0x0001AE44
		internal ConstantExpression SkipAmount
		{
			get
			{
				return this.skipAmount;
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		internal override QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
		{
			int num = (int)this.skipAmount.Value;
			int num2 = (int)((SkipQueryOptionExpression)previous).skipAmount.Value;
			return new SkipQueryOptionExpression(this.Type, Expression.Constant(num + num2, typeof(int)));
		}

		// Token: 0x04000441 RID: 1089
		private ConstantExpression skipAmount;
	}
}
