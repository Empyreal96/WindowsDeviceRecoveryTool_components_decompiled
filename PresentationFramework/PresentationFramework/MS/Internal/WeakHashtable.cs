using System;
using System.Collections;
using System.Security.Permissions;

namespace MS.Internal
{
	// Token: 0x020005F4 RID: 1524
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06006567 RID: 25959 RVA: 0x001C7187 File Offset: 0x001C5387
		internal WeakHashtable() : base(WeakHashtable._comparer)
		{
		}

		// Token: 0x06006568 RID: 25960 RVA: 0x001C7194 File Offset: 0x001C5394
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x06006569 RID: 25961 RVA: 0x001C719C File Offset: 0x001C539C
		public override void Remove(object key)
		{
			base.Remove(key);
		}

		// Token: 0x0600656A RID: 25962 RVA: 0x001C71A5 File Offset: 0x001C53A5
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x001C71BC File Offset: 0x001C53BC
		public object UnwrapKey(object key)
		{
			WeakHashtable.EqualityWeakReference equalityWeakReference = key as WeakHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return null;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x001C71DC File Offset: 0x001C53DC
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakHashtable.EqualityWeakReference equalityWeakReference = obj as WeakHashtable.EqualityWeakReference;
					if (equalityWeakReference != null && !equalityWeakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(equalityWeakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object key in arrayList)
					{
						this.Remove(key);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x001C7328 File Offset: 0x001C5528
		public static IWeakHashtable FromKeyType(Type tKey)
		{
			if (tKey == typeof(object) || tKey.IsValueType)
			{
				return new WeakObjectHashtable();
			}
			return new WeakHashtable();
		}

		// Token: 0x040032C3 RID: 12995
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x040032C4 RID: 12996
		private long _lastGlobalMem;

		// Token: 0x040032C5 RID: 12997
		private int _lastHashCount;

		// Token: 0x02000A0E RID: 2574
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06008A30 RID: 35376 RVA: 0x00256D98 File Offset: 0x00254F98
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					return y == null;
				}
				if (y == null || x.GetHashCode() != y.GetHashCode())
				{
					return false;
				}
				WeakHashtable.EqualityWeakReference equalityWeakReference = x as WeakHashtable.EqualityWeakReference;
				WeakHashtable.EqualityWeakReference equalityWeakReference2 = y as WeakHashtable.EqualityWeakReference;
				if (equalityWeakReference != null && equalityWeakReference2 != null && !equalityWeakReference2.IsAlive && !equalityWeakReference.IsAlive)
				{
					return true;
				}
				if (equalityWeakReference != null)
				{
					x = equalityWeakReference.Target;
				}
				if (equalityWeakReference2 != null)
				{
					y = equalityWeakReference2.Target;
				}
				return x == y;
			}

			// Token: 0x06008A31 RID: 35377 RVA: 0x0025681D File Offset: 0x00254A1D
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000A0F RID: 2575
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008A33 RID: 35379 RVA: 0x00256E00 File Offset: 0x00255000
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001F33 RID: 7987
			// (get) Token: 0x06008A34 RID: 35380 RVA: 0x00256E20 File Offset: 0x00255020
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001F34 RID: 7988
			// (get) Token: 0x06008A35 RID: 35381 RVA: 0x00256E2D File Offset: 0x0025502D
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x06008A36 RID: 35382 RVA: 0x00256E3A File Offset: 0x0025503A
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x06008A37 RID: 35383 RVA: 0x00256E61 File Offset: 0x00255061
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x040046C0 RID: 18112
			private int _hashCode;

			// Token: 0x040046C1 RID: 18113
			private WeakReference _weakRef;
		}
	}
}
