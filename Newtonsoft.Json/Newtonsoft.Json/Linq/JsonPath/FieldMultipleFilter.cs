using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006F RID: 111
	internal class FieldMultipleFilter : PathFilter
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x000173D7 File Offset: 0x000155D7
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x000173DF File Offset: 0x000155DF
		public List<string> Names { get; set; }

		// Token: 0x06000611 RID: 1553 RVA: 0x00017700 File Offset: 0x00015900
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				JObject o = t as JObject;
				if (o != null)
				{
					foreach (string name in this.Names)
					{
						JToken v = o[name];
						if (v != null)
						{
							yield return v;
						}
						if (errorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, name));
						}
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Properties {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", (from n in this.Names
					select "'" + n + "'").ToArray<string>()), t.GetType().Name));
				}
			}
			yield break;
		}
	}
}
