using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Data.OData
{
	// Token: 0x02000278 RID: 632
	internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		// Token: 0x060014D9 RID: 5337 RVA: 0x0004D610 File Offset: 0x0004B810
		private ReferenceEqualityComparer()
		{
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0004D618 File Offset: 0x0004B818
		internal static ReferenceEqualityComparer<T> Instance
		{
			get
			{
				if (ReferenceEqualityComparer<T>.instance == null)
				{
					ReferenceEqualityComparer<T> value = new ReferenceEqualityComparer<T>();
					Interlocked.CompareExchange<ReferenceEqualityComparer<T>>(ref ReferenceEqualityComparer<T>.instance, value, null);
				}
				return ReferenceEqualityComparer<T>.instance;
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004D644 File Offset: 0x0004B844
		public bool Equals(T x, T y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004D657 File Offset: 0x0004B857
		public int GetHashCode(T obj)
		{
			if (obj != null)
			{
				return obj.GetHashCode();
			}
			return 0;
		}

		// Token: 0x04000795 RID: 1941
		private static ReferenceEqualityComparer<T> instance;
	}
}
