using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Data.Services.Client
{
	// Token: 0x020000B2 RID: 178
	internal sealed class ReferenceEqualityComparer<T> : ReferenceEqualityComparer, IEqualityComparer<T>
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x000159B3 File Offset: 0x00013BB3
		private ReferenceEqualityComparer()
		{
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000159BC File Offset: 0x00013BBC
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

		// Token: 0x060005AF RID: 1455 RVA: 0x000159E8 File Offset: 0x00013BE8
		public bool Equals(T x, T y)
		{
			return object.ReferenceEquals(x, y);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000159FB File Offset: 0x00013BFB
		public int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x04000315 RID: 789
		private static ReferenceEqualityComparer<T> instance;
	}
}
