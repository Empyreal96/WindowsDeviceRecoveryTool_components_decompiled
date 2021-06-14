using System;
using System.Collections;

namespace Microsoft.Data.OData
{
	// Token: 0x02000242 RID: 578
	internal class ReadOnlyEnumerable : IEnumerable
	{
		// Token: 0x0600128C RID: 4748 RVA: 0x00045B51 File Offset: 0x00043D51
		internal ReadOnlyEnumerable(IEnumerable sourceEnumerable)
		{
			this.sourceEnumerable = sourceEnumerable;
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x00045B60 File Offset: 0x00043D60
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.sourceEnumerable.GetEnumerator();
		}

		// Token: 0x040006AA RID: 1706
		private readonly IEnumerable sourceEnumerable;
	}
}
