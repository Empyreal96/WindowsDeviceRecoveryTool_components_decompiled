using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200007B RID: 123
	internal class BooleanQueryExpression : QueryExpression
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x0001A35C File Offset: 0x0001855C
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x0001A364 File Offset: 0x00018564
		public List<PathFilter> Path { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001A36D File Offset: 0x0001856D
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x0001A375 File Offset: 0x00018575
		public JValue Value { get; set; }

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001A380 File Offset: 0x00018580
		public override bool IsMatch(JToken t)
		{
			IEnumerable<JToken> enumerable = JPath.Evaluate(this.Path, t, false);
			foreach (JToken jtoken in enumerable)
			{
				JValue jvalue = jtoken as JValue;
				switch (base.Operator)
				{
				case QueryOperator.Equals:
					if (jvalue != null && jvalue.Equals(this.Value))
					{
						return true;
					}
					break;
				case QueryOperator.NotEquals:
					if (jvalue != null && !jvalue.Equals(this.Value))
					{
						return true;
					}
					break;
				case QueryOperator.Exists:
					return true;
				case QueryOperator.LessThan:
					if (jvalue != null && jvalue.CompareTo(this.Value) < 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThanOrEquals:
					if (jvalue != null && jvalue.CompareTo(this.Value) <= 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThan:
					if (jvalue != null && jvalue.CompareTo(this.Value) > 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThanOrEquals:
					if (jvalue != null && jvalue.CompareTo(this.Value) >= 0)
					{
						return true;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return false;
		}
	}
}
