using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000252 RID: 594
	internal static class Utils
	{
		// Token: 0x06001387 RID: 4999 RVA: 0x00049420 File Offset: 0x00047620
		internal static bool TryDispose(object o)
		{
			IDisposable disposable = o as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00049440 File Offset: 0x00047640
		internal static Task FlushAsync(this Stream stream)
		{
			return Task.Factory.StartNew(new Action(stream.Flush));
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0004945C File Offset: 0x0004765C
		internal static KeyValuePair<int, T>[] StableSort<T>(this T[] array, Comparison<T> comparison)
		{
			ExceptionUtils.CheckArgumentNotNull<T[]>(array, "array");
			ExceptionUtils.CheckArgumentNotNull<Comparison<T>>(comparison, "comparison");
			KeyValuePair<int, T>[] array2 = new KeyValuePair<int, T>[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new KeyValuePair<int, T>(i, array[i]);
			}
			Array.Sort<KeyValuePair<int, T>>(array2, new Utils.StableComparer<T>(comparison));
			return array2;
		}

		// Token: 0x02000253 RID: 595
		private sealed class StableComparer<T> : IComparer<KeyValuePair<int, T>>
		{
			// Token: 0x0600138A RID: 5002 RVA: 0x000494BC File Offset: 0x000476BC
			public StableComparer(Comparison<T> innerComparer)
			{
				this.innerComparer = innerComparer;
			}

			// Token: 0x0600138B RID: 5003 RVA: 0x000494CC File Offset: 0x000476CC
			public int Compare(KeyValuePair<int, T> x, KeyValuePair<int, T> y)
			{
				int num = this.innerComparer(x.Value, y.Value);
				if (num == 0)
				{
					num = x.Key - y.Key;
				}
				return num;
			}

			// Token: 0x040006FD RID: 1789
			private readonly Comparison<T> innerComparer;
		}
	}
}
