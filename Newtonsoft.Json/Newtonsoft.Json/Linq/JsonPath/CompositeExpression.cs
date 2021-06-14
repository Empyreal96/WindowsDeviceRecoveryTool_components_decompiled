using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200007A RID: 122
	internal class CompositeExpression : QueryExpression
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001A26D File Offset: 0x0001846D
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001A275 File Offset: 0x00018475
		public List<QueryExpression> Expressions { get; set; }

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001A27E File Offset: 0x0001847E
		public CompositeExpression()
		{
			this.Expressions = new List<QueryExpression>();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001A294 File Offset: 0x00018494
		public override bool IsMatch(JToken t)
		{
			switch (base.Operator)
			{
			case QueryOperator.And:
				foreach (QueryExpression queryExpression in this.Expressions)
				{
					if (!queryExpression.IsMatch(t))
					{
						return false;
					}
				}
				return true;
			case QueryOperator.Or:
				foreach (QueryExpression queryExpression2 in this.Expressions)
				{
					if (queryExpression2.IsMatch(t))
					{
						return true;
					}
				}
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
			bool result;
			return result;
		}
	}
}
