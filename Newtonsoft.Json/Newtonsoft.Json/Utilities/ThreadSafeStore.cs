using System;
using System.Collections.Generic;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000103 RID: 259
	internal class ThreadSafeStore<TKey, TValue>
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x00031537 File Offset: 0x0002F737
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			this._creator = creator;
			this._store = new Dictionary<TKey, TValue>();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0003156C File Offset: 0x0002F76C
		public TValue Get(TKey key)
		{
			TValue result;
			if (!this._store.TryGetValue(key, out result))
			{
				return this.AddValue(key);
			}
			return result;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00031594 File Offset: 0x0002F794
		private TValue AddValue(TKey key)
		{
			TValue tvalue = this._creator(key);
			TValue result2;
			lock (this._lock)
			{
				if (this._store == null)
				{
					this._store = new Dictionary<TKey, TValue>();
					this._store[key] = tvalue;
				}
				else
				{
					TValue result;
					if (this._store.TryGetValue(key, out result))
					{
						return result;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._store);
					dictionary[key] = tvalue;
					Thread.MemoryBarrier();
					this._store = dictionary;
				}
				result2 = tvalue;
			}
			return result2;
		}

		// Token: 0x04000460 RID: 1120
		private readonly object _lock = new object();

		// Token: 0x04000461 RID: 1121
		private Dictionary<TKey, TValue> _store;

		// Token: 0x04000462 RID: 1122
		private readonly Func<TKey, TValue> _creator;
	}
}
