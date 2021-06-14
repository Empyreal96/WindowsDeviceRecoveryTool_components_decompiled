using System;
using System.Collections;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000124 RID: 292
	internal class ReferenceEqualityComparer : IEqualityComparer
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x00049BEB File Offset: 0x00047DEB
		protected ReferenceEqualityComparer()
		{
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00049BF3 File Offset: 0x00047DF3
		bool IEqualityComparer.Equals(object x, object y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00049BFC File Offset: 0x00047DFC
		int IEqualityComparer.GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}
	}
}
