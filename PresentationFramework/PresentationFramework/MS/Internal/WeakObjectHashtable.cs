using System;
using System.Collections;
using System.Security.Permissions;

namespace MS.Internal
{
	// Token: 0x020005F6 RID: 1526
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakObjectHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06006579 RID: 25977 RVA: 0x001C74BC File Offset: 0x001C56BC
		internal WeakObjectHashtable() : base(WeakObjectHashtable._comparer)
		{
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x001C74C9 File Offset: 0x001C56C9
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this.WrapKey(ref key);
			this[key] = value;
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x001C74E1 File Offset: 0x001C56E1
		private void WrapKey(ref object key)
		{
			if (key != null && !key.GetType().IsValueType)
			{
				key = new WeakObjectHashtable.EqualityWeakReference(key);
			}
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x001C7500 File Offset: 0x001C5700
		public object UnwrapKey(object key)
		{
			WeakObjectHashtable.EqualityWeakReference equalityWeakReference = key as WeakObjectHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return key;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x001C7520 File Offset: 0x001C5720
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
			long num = totalMemory - this._lastGlobalMem;
			long num2 = (long)(count - this._lastHashCount);
			if (num < 0L && num2 >= 0L)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakObjectHashtable.EqualityWeakReference equalityWeakReference = obj as WeakObjectHashtable.EqualityWeakReference;
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

		// Token: 0x040032C7 RID: 12999
		private static IEqualityComparer _comparer = new WeakObjectHashtable.WeakKeyComparer();

		// Token: 0x040032C8 RID: 13000
		private long _lastGlobalMem;

		// Token: 0x040032C9 RID: 13001
		private int _lastHashCount;

		// Token: 0x02000A11 RID: 2577
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06008A3F RID: 35391 RVA: 0x00256FC8 File Offset: 0x002551C8
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
				if (x == y)
				{
					return true;
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference;
				if ((equalityWeakReference = (x as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					x = equalityWeakReference.Target;
					if (x == null)
					{
						return false;
					}
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference2;
				if ((equalityWeakReference2 = (y as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					y = equalityWeakReference2.Target;
					if (y == null)
					{
						return false;
					}
				}
				return object.Equals(x, y);
			}

			// Token: 0x06008A40 RID: 35392 RVA: 0x0025681D File Offset: 0x00254A1D
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000A12 RID: 2578
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008A42 RID: 35394 RVA: 0x0025702B File Offset: 0x0025522B
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001F37 RID: 7991
			// (get) Token: 0x06008A43 RID: 35395 RVA: 0x0025704B File Offset: 0x0025524B
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001F38 RID: 7992
			// (get) Token: 0x06008A44 RID: 35396 RVA: 0x00257058 File Offset: 0x00255258
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x06008A45 RID: 35397 RVA: 0x00257065 File Offset: 0x00255265
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x06008A46 RID: 35398 RVA: 0x0025708C File Offset: 0x0025528C
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x040046C6 RID: 18118
			private int _hashCode;

			// Token: 0x040046C7 RID: 18119
			private WeakReference _weakRef;
		}
	}
}
