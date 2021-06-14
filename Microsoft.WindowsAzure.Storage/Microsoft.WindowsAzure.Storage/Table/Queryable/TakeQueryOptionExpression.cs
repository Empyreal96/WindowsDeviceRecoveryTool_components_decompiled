using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000135 RID: 309
	[DebuggerDisplay("TakeQueryOptionExpression {TakeAmount}")]
	internal class TakeQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x06001431 RID: 5169 RVA: 0x0004D0AB File Offset: 0x0004B2AB
		internal TakeQueryOptionExpression(Type type, ConstantExpression takeAmount) : base((ExpressionType)10003, type)
		{
			this.takeAmount = takeAmount;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0004D0C0 File Offset: 0x0004B2C0
		internal ConstantExpression TakeAmount
		{
			get
			{
				return this.takeAmount;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0004D0C8 File Offset: 0x0004B2C8
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

		// Token: 0x040006AF RID: 1711
		private ConstantExpression takeAmount;
	}
}
