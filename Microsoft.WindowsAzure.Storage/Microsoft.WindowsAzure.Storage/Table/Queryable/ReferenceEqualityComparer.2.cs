using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000125 RID: 293
	internal sealed class ReferenceEqualityComparer<T> : ReferenceEqualityComparer, IEqualityComparer<T>
	{
		// Token: 0x060013A8 RID: 5032 RVA: 0x00049C09 File Offset: 0x00047E09
		private ReferenceEqualityComparer()
		{
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00049C14 File Offset: 0x00047E14
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

		// Token: 0x060013AA RID: 5034 RVA: 0x00049C40 File Offset: 0x00047E40
		public bool Equals(T x, T y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00049C53 File Offset: 0x00047E53
		public int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x040005C9 RID: 1481
		private static ReferenceEqualityComparer<T> instance;
	}
}
