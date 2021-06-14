using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x020000DA RID: 218
	[DebuggerDisplay("TakeQueryOptionExpression {TakeAmount}")]
	internal class TakeQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x0001CCA2 File Offset: 0x0001AEA2
		internal TakeQueryOptionExpression(Type type, ConstantExpression takeAmount) : base(type)
		{
			this.takeAmount = takeAmount;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001CCB2 File Offset: 0x0001AEB2
		public override ExpressionType NodeType
		{
			get
			{
				return (ExpressionType)10003;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001CCB9 File Offset: 0x0001AEB9
		internal ConstantExpression TakeAmount
		{
			get
			{
				return this.takeAmount;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001CCC4 File Offset: 0x0001AEC4
		internal override QueryOptionExpression ComposeMultipleSpecification(QueryOptionExpression previous)
		{
			int num = (int)this.takeAmount.Value;
			int num2 = (int)((TakeQueryOptionExpression)previous).takeAmount.Value;
			if (num >= num2)
			{
				return previous;
			}
			return this;
		}

		// Token: 0x04000442 RID: 1090
		private ConstantExpression takeAmount;
	}
}
