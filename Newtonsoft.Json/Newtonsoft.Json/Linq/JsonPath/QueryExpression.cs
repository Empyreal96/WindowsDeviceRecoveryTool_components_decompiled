using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x02000079 RID: 121
	internal abstract class QueryExpression
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001A254 File Offset: 0x00018454
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001A25C File Offset: 0x0001845C
		public QueryOperator Operator { get; set; }

		// Token: 0x060006AF RID: 1711
		public abstract bool IsMatch(JToken t);
	}
}
