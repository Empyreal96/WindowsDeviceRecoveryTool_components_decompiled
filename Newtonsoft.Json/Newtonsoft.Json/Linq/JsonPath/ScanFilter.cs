using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200007D RID: 125
	internal class ScanFilter : PathFilter
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0001A724 File Offset: 0x00018924
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x0001A72C File Offset: 0x0001892C
		public string Name { get; set; }

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001AA44 File Offset: 0x00018C44
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken root in current)
			{
				if (this.Name == null)
				{
					yield return root;
				}
				JToken value = root;
				JToken container = root;
				for (;;)
				{
					if (container != null && container.HasValues)
					{
						value = container.First;
					}
					else
					{
						while (value != null && value != root && value == value.Parent.Last)
						{
							value = value.Parent;
						}
						if (value == null || value == root)
						{
							break;
						}
						value = value.Next;
					}
					JProperty e = value as JProperty;
					if (e != null)
					{
						if (e.Name == this.Name)
						{
							yield return e.Value;
						}
					}
					else if (this.Name == null)
					{
						yield return value;
					}
					container = (value as JContainer);
				}
			}
			yield break;
		}
	}
}
