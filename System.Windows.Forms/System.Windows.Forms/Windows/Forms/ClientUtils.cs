using System;
using System.Collections;
using System.Security;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x02000100 RID: 256
	internal static class ClientUtils
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x0000CDC6 File Offset: 0x0000AFC6
		public static bool IsCriticalException(Exception ex)
		{
			return ex is NullReferenceException || ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is ExecutionEngineException || ex is IndexOutOfRangeException || ex is AccessViolationException;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000CE03 File Offset: 0x0000B003
		public static bool IsSecurityOrCriticalException(Exception ex)
		{
			return ex is SecurityException || ClientUtils.IsCriticalException(ex);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000CE18 File Offset: 0x0000B018
		public static int GetBitCount(uint x)
		{
			int num = 0;
			while (x > 0U)
			{
				x &= x - 1U;
				num++;
			}
			return num;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue, int maxNumberOfBitsOn)
		{
			bool flag = value >= minValue && value <= maxValue;
			return flag && ClientUtils.GetBitCount((uint)value) <= maxNumberOfBitsOn;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000CE90 File Offset: 0x0000B090
		public static bool IsEnumValid_Masked(Enum enumValue, int value, uint mask)
		{
			return ((long)value & (long)((ulong)mask)) == (long)value;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		public static bool IsEnumValid_NotSequential(Enum enumValue, int value, params int[] enumValues)
		{
			for (int i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] == value)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0200053F RID: 1343
		internal class WeakRefCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060054CA RID: 21706 RVA: 0x00164284 File Offset: 0x00162484
			internal WeakRefCollection()
			{
				this._innerList = new ArrayList(4);
			}

			// Token: 0x060054CB RID: 21707 RVA: 0x001642A3 File Offset: 0x001624A3
			internal WeakRefCollection(int size)
			{
				this._innerList = new ArrayList(size);
			}

			// Token: 0x17001459 RID: 5209
			// (get) Token: 0x060054CC RID: 21708 RVA: 0x001642C2 File Offset: 0x001624C2
			internal ArrayList InnerList
			{
				get
				{
					return this._innerList;
				}
			}

			// Token: 0x1700145A RID: 5210
			// (get) Token: 0x060054CD RID: 21709 RVA: 0x001642CA File Offset: 0x001624CA
			// (set) Token: 0x060054CE RID: 21710 RVA: 0x001642D2 File Offset: 0x001624D2
			public int RefCheckThreshold
			{
				get
				{
					return this.refCheckThreshold;
				}
				set
				{
					this.refCheckThreshold = value;
				}
			}

			// Token: 0x1700145B RID: 5211
			public object this[int index]
			{
				get
				{
					ClientUtils.WeakRefCollection.WeakRefObject weakRefObject = this.InnerList[index] as ClientUtils.WeakRefCollection.WeakRefObject;
					if (weakRefObject != null && weakRefObject.IsAlive)
					{
						return weakRefObject.Target;
					}
					return null;
				}
				set
				{
					this.InnerList[index] = this.CreateWeakRefObject(value);
				}
			}

			// Token: 0x060054D1 RID: 21713 RVA: 0x00164324 File Offset: 0x00162524
			public void ScavengeReferences()
			{
				int num = 0;
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					if (this[num] == null)
					{
						this.InnerList.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}

			// Token: 0x060054D2 RID: 21714 RVA: 0x00164364 File Offset: 0x00162564
			public override bool Equals(object obj)
			{
				ClientUtils.WeakRefCollection weakRefCollection = obj as ClientUtils.WeakRefCollection;
				if (weakRefCollection == this)
				{
					return true;
				}
				if (weakRefCollection == null || this.Count != weakRefCollection.Count)
				{
					return false;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this.InnerList[i] != weakRefCollection.InnerList[i] && (this.InnerList[i] == null || !this.InnerList[i].Equals(weakRefCollection.InnerList[i])))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060054D3 RID: 21715 RVA: 0x001572D5 File Offset: 0x001554D5
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060054D4 RID: 21716 RVA: 0x001643EC File Offset: 0x001625EC
			private ClientUtils.WeakRefCollection.WeakRefObject CreateWeakRefObject(object value)
			{
				if (value == null)
				{
					return null;
				}
				return new ClientUtils.WeakRefCollection.WeakRefObject(value);
			}

			// Token: 0x060054D5 RID: 21717 RVA: 0x001643FC File Offset: 0x001625FC
			private static void Copy(ClientUtils.WeakRefCollection sourceList, int sourceIndex, ClientUtils.WeakRefCollection destinationList, int destinationIndex, int length)
			{
				if (sourceIndex < destinationIndex)
				{
					sourceIndex += length;
					destinationIndex += length;
					while (length > 0)
					{
						destinationList.InnerList[--destinationIndex] = sourceList.InnerList[--sourceIndex];
						length--;
					}
					return;
				}
				while (length > 0)
				{
					destinationList.InnerList[destinationIndex++] = sourceList.InnerList[sourceIndex++];
					length--;
				}
			}

			// Token: 0x060054D6 RID: 21718 RVA: 0x00164478 File Offset: 0x00162678
			public void RemoveByHashCode(object value)
			{
				if (value == null)
				{
					return;
				}
				int hashCode = value.GetHashCode();
				for (int i = 0; i < this.InnerList.Count; i++)
				{
					if (this.InnerList[i] != null && this.InnerList[i].GetHashCode() == hashCode)
					{
						this.RemoveAt(i);
						return;
					}
				}
			}

			// Token: 0x060054D7 RID: 21719 RVA: 0x001644D0 File Offset: 0x001626D0
			public void Clear()
			{
				this.InnerList.Clear();
			}

			// Token: 0x1700145C RID: 5212
			// (get) Token: 0x060054D8 RID: 21720 RVA: 0x001644DD File Offset: 0x001626DD
			public bool IsFixedSize
			{
				get
				{
					return this.InnerList.IsFixedSize;
				}
			}

			// Token: 0x060054D9 RID: 21721 RVA: 0x001644EA File Offset: 0x001626EA
			public bool Contains(object value)
			{
				return this.InnerList.Contains(this.CreateWeakRefObject(value));
			}

			// Token: 0x060054DA RID: 21722 RVA: 0x001644FE File Offset: 0x001626FE
			public void RemoveAt(int index)
			{
				this.InnerList.RemoveAt(index);
			}

			// Token: 0x060054DB RID: 21723 RVA: 0x0016450C File Offset: 0x0016270C
			public void Remove(object value)
			{
				this.InnerList.Remove(this.CreateWeakRefObject(value));
			}

			// Token: 0x060054DC RID: 21724 RVA: 0x00164520 File Offset: 0x00162720
			public int IndexOf(object value)
			{
				return this.InnerList.IndexOf(this.CreateWeakRefObject(value));
			}

			// Token: 0x060054DD RID: 21725 RVA: 0x00164534 File Offset: 0x00162734
			public void Insert(int index, object value)
			{
				this.InnerList.Insert(index, this.CreateWeakRefObject(value));
			}

			// Token: 0x060054DE RID: 21726 RVA: 0x00164549 File Offset: 0x00162749
			public int Add(object value)
			{
				if (this.Count > this.RefCheckThreshold)
				{
					this.ScavengeReferences();
				}
				return this.InnerList.Add(this.CreateWeakRefObject(value));
			}

			// Token: 0x1700145D RID: 5213
			// (get) Token: 0x060054DF RID: 21727 RVA: 0x00164571 File Offset: 0x00162771
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			// Token: 0x1700145E RID: 5214
			// (get) Token: 0x060054E0 RID: 21728 RVA: 0x0016457E File Offset: 0x0016277E
			object ICollection.SyncRoot
			{
				get
				{
					return this.InnerList.SyncRoot;
				}
			}

			// Token: 0x1700145F RID: 5215
			// (get) Token: 0x060054E1 RID: 21729 RVA: 0x0016458B File Offset: 0x0016278B
			public bool IsReadOnly
			{
				get
				{
					return this.InnerList.IsReadOnly;
				}
			}

			// Token: 0x060054E2 RID: 21730 RVA: 0x00164598 File Offset: 0x00162798
			public void CopyTo(Array array, int index)
			{
				this.InnerList.CopyTo(array, index);
			}

			// Token: 0x17001460 RID: 5216
			// (get) Token: 0x060054E3 RID: 21731 RVA: 0x001645A7 File Offset: 0x001627A7
			bool ICollection.IsSynchronized
			{
				get
				{
					return this.InnerList.IsSynchronized;
				}
			}

			// Token: 0x060054E4 RID: 21732 RVA: 0x001645B4 File Offset: 0x001627B4
			public IEnumerator GetEnumerator()
			{
				return this.InnerList.GetEnumerator();
			}

			// Token: 0x0400375F RID: 14175
			private int refCheckThreshold = int.MaxValue;

			// Token: 0x04003760 RID: 14176
			private ArrayList _innerList;

			// Token: 0x02000887 RID: 2183
			internal class WeakRefObject
			{
				// Token: 0x06007061 RID: 28769 RVA: 0x0019B60B File Offset: 0x0019980B
				internal WeakRefObject(object obj)
				{
					this.weakHolder = new WeakReference(obj);
					this.hash = obj.GetHashCode();
				}

				// Token: 0x17001866 RID: 6246
				// (get) Token: 0x06007062 RID: 28770 RVA: 0x0019B62B File Offset: 0x0019982B
				internal bool IsAlive
				{
					get
					{
						return this.weakHolder.IsAlive;
					}
				}

				// Token: 0x17001867 RID: 6247
				// (get) Token: 0x06007063 RID: 28771 RVA: 0x0019B638 File Offset: 0x00199838
				internal object Target
				{
					get
					{
						return this.weakHolder.Target;
					}
				}

				// Token: 0x06007064 RID: 28772 RVA: 0x0019B645 File Offset: 0x00199845
				public override int GetHashCode()
				{
					return this.hash;
				}

				// Token: 0x06007065 RID: 28773 RVA: 0x0019B650 File Offset: 0x00199850
				public override bool Equals(object obj)
				{
					ClientUtils.WeakRefCollection.WeakRefObject weakRefObject = obj as ClientUtils.WeakRefCollection.WeakRefObject;
					return weakRefObject == this || (weakRefObject != null && (weakRefObject.Target == this.Target || (this.Target != null && this.Target.Equals(weakRefObject.Target))));
				}

				// Token: 0x040043DC RID: 17372
				private int hash;

				// Token: 0x040043DD RID: 17373
				private WeakReference weakHolder;
			}
		}
	}
}
