using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200007C RID: 124
	internal class QueryFilter : PathFilter
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001A4B0 File Offset: 0x000186B0
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001A4B8 File Offset: 0x000186B8
		public QueryExpression Expression { get; set; }

		// Token: 0x060006BD RID: 1725 RVA: 0x0001A6F8 File Offset: 0x000188F8
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				foreach (JToken v in ((IEnumerable<JToken>)t))
				{
					if (this.Expression.IsMatch(v))
					{
						yield return v;
					}
				}
			}
			yield break;
		}
	}
}
