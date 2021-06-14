using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006B RID: 107
	internal class ArrayIndexFilter : PathFilter
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00016532 File Offset: 0x00014732
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0001653A File Offset: 0x0001473A
		public int? Index { get; set; }

		// Token: 0x060005FC RID: 1532 RVA: 0x00016844 File Offset: 0x00014A44
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				if (this.Index != null)
				{
					JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, this.Index.Value);
					if (v != null)
					{
						yield return v;
					}
				}
				else if (t is JArray || t is JConstructor)
				{
					foreach (JToken v2 in ((IEnumerable<JToken>)t))
					{
						yield return v2;
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Index * not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
			yield break;
		}
	}
}
