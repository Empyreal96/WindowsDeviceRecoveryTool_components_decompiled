using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x02000120 RID: 288
	internal sealed class HttpHeaderValue : Dictionary<string, HttpHeaderValueElement>
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x00019E39 File Offset: 0x00018039
		internal HttpHeaderValue() : base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00019E50 File Offset: 0x00018050
		public override string ToString()
		{
			if (base.Count != 0)
			{
				return string.Join(",", (from element in base.Values
				select element.ToString()).ToArray<string>());
			}
			return null;
		}
	}
}
