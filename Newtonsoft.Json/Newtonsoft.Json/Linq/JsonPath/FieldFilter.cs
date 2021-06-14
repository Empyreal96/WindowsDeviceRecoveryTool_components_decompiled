using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006E RID: 110
	internal class FieldFilter : PathFilter
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00017061 File Offset: 0x00015261
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x00017069 File Offset: 0x00015269
		public string Name { get; set; }

		// Token: 0x0600060D RID: 1549 RVA: 0x000173A4 File Offset: 0x000155A4
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				JObject o = t as JObject;
				if (o != null)
				{
					if (this.Name != null)
					{
						JToken v = o[this.Name];
						if (v != null)
						{
							yield return v;
						}
						else if (errorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, this.Name));
						}
					}
					else
					{
						foreach (KeyValuePair<string, JToken> p in o)
						{
							KeyValuePair<string, JToken> keyValuePair = p;
							yield return keyValuePair.Value;
						}
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, this.Name ?? "*", t.GetType().Name));
				}
			}
			yield break;
		}
	}
}
