using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000069 RID: 105
	public class JsonMergeSettings
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001644A File Offset: 0x0001464A
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00016452 File Offset: 0x00014652
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x040001C1 RID: 449
		private MergeArrayHandling _mergeArrayHandling;
	}
}
