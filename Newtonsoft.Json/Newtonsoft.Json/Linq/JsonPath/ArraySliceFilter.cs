using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x0200006D RID: 109
	internal class ArraySliceFilter : PathFilter
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00016B0B File Offset: 0x00014D0B
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x00016B13 File Offset: 0x00014D13
		public int? Start { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00016B1C File Offset: 0x00014D1C
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x00016B24 File Offset: 0x00014D24
		public int? End { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00016B2D File Offset: 0x00014D2D
		// (set) Token: 0x06000607 RID: 1543 RVA: 0x00016B35 File Offset: 0x00014D35
		public int? Step { get; set; }

		// Token: 0x06000608 RID: 1544 RVA: 0x00017020 File Offset: 0x00015220
		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			if (this.Step == 0)
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken t in current)
			{
				JArray a = t as JArray;
				if (a != null)
				{
					int stepCount = this.Step ?? 1;
					int startIndex = this.Start ?? ((stepCount > 0) ? 0 : (a.Count - 1));
					int stopIndex = this.End ?? ((stepCount > 0) ? a.Count : -1);
					if (this.Start < 0)
					{
						startIndex = a.Count + startIndex;
					}
					if (this.End < 0)
					{
						stopIndex = a.Count + stopIndex;
					}
					startIndex = Math.Max(startIndex, (stepCount > 0) ? 0 : int.MinValue);
					startIndex = Math.Min(startIndex, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex = Math.Max(stopIndex, -1);
					stopIndex = Math.Min(stopIndex, a.Count);
					bool positiveStep = stepCount > 0;
					if (this.IsValid(startIndex, stopIndex, positiveStep))
					{
						int i = startIndex;
						while (this.IsValid(i, stopIndex, positiveStep))
						{
							yield return a[i];
							i += stepCount;
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, (this.Start != null) ? this.Start.Value.ToString(CultureInfo.InvariantCulture) : "*", (this.End != null) ? this.End.Value.ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
			yield break;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001704B File Offset: 0x0001524B
		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
}
