using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006C RID: 108
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x00016877 File Offset: 0x00014A77
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x0001687F File Offset: 0x00014A7F
		public List<int> Indexes { get; set; }

		// Token: 0x06000600 RID: 1536 RVA: 0x00016AD8 File Offset: 0x00014CD8
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				foreach (int i in this.Indexes)
				{
					JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, i);
					if (v != null)
					{
						yield return v;
					}
				}
			}
			yield break;
		}
	}
}
