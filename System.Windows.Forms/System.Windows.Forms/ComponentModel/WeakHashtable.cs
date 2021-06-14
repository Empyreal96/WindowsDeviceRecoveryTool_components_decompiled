using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000F8 RID: 248
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable
	{
		// Token: 0x060003DE RID: 990 RVA: 0x0000C2A5 File Offset: 0x0000A4A5
		internal WeakHashtable() : base(WeakHashtable._comparer)
		{
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000C2B2 File Offset: 0x0000A4B2
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000C2BA File Offset: 0x0000A4BA
		public override void Remove(object key)
		{
			base.Remove(key);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000C2C3 File Offset: 0x0000A4C3
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
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
					WeakReference weakReference = obj as WeakReference;
					if (weakReference != null && !weakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(weakReference);
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

		// Token: 0x04000422 RID: 1058
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x04000423 RID: 1059
		private long _lastGlobalMem;

		// Token: 0x04000424 RID: 1060
		private int _lastHashCount;

		// Token: 0x0200053B RID: 1339
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x060054B9 RID: 21689 RVA: 0x00163D14 File Offset: 0x00161F14
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					return y == null;
				}
				if (y != null && x.GetHashCode() == y.GetHashCode())
				{
					WeakReference weakReference = x as WeakReference;
					WeakReference weakReference2 = y as WeakReference;
					if (weakReference != null)
					{
						if (!weakReference.IsAlive)
						{
							return false;
						}
						x = weakReference.Target;
					}
					if (weakReference2 != null)
					{
						if (!weakReference2.IsAlive)
						{
							return false;
						}
						y = weakReference2.Target;
					}
					return x == y;
				}
				return false;
			}

			// Token: 0x060054BA RID: 21690 RVA: 0x00163D78 File Offset: 0x00161F78
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x0200053C RID: 1340
		private sealed class EqualityWeakReference : WeakReference
		{
			// Token: 0x060054BC RID: 21692 RVA: 0x00163D80 File Offset: 0x00161F80
			internal EqualityWeakReference(object o) : base(o)
			{
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x060054BD RID: 21693 RVA: 0x00163D95 File Offset: 0x00161F95
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || (this.IsAlive && o == this.Target));
			}

			// Token: 0x060054BE RID: 21694 RVA: 0x00163DC4 File Offset: 0x00161FC4
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04003755 RID: 14165
			private int _hashCode;
		}
	}
}
