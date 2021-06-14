using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x02000741 RID: 1857
	internal class RBTree<T> : RBNode<T>, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x060076AB RID: 30379 RVA: 0x0021EBD9 File Offset: 0x0021CDD9
		public RBTree() : base(false)
		{
			base.Size = 64;
		}

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x060076AC RID: 30380 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool HasData
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x060076AD RID: 30381 RVA: 0x0021EBEA File Offset: 0x0021CDEA
		// (set) Token: 0x060076AE RID: 30382 RVA: 0x0021EBF2 File Offset: 0x0021CDF2
		public Comparison<T> Comparison
		{
			get
			{
				return this._comparison;
			}
			set
			{
				this._comparison = value;
			}
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x0021EBFB File Offset: 0x0021CDFB
		public RBFinger<T> BoundedSearch(T x, int low, int high)
		{
			return base.BoundedSearch(x, low, high, this.Comparison);
		}

		// Token: 0x060076B0 RID: 30384 RVA: 0x0021EC0C File Offset: 0x0021CE0C
		public void Insert(T x)
		{
			RBFinger<T> finger = base.Find(x, this.Comparison);
			this.Insert(finger, x, true);
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x0021EC30 File Offset: 0x0021CE30
		private void Insert(RBFinger<T> finger, T x, bool checkSort = false)
		{
			RBNode<T> rbnode = finger.Node;
			if (rbnode == this)
			{
				rbnode = this.InsertNode(0);
				rbnode.InsertAt(0, x, null, null);
			}
			else if (rbnode.Size < 64)
			{
				rbnode.InsertAt(finger.Offset, x, null, null);
			}
			else
			{
				RBNode<T> rbnode2 = rbnode.GetSuccessor();
				RBNode<T> succsucc = null;
				if (rbnode2.Size >= 64)
				{
					if (rbnode2 != this)
					{
						succsucc = rbnode2;
					}
					rbnode2 = this.InsertNode(finger.Index + rbnode.Size - finger.Offset);
				}
				rbnode.InsertAt(finger.Offset, x, rbnode2, succsucc);
			}
			base.LeftChild.IsRed = false;
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x0021ECCC File Offset: 0x0021CECC
		public void Sort()
		{
			try
			{
				this.QuickSort();
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_IComparerFailed"), innerException);
			}
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x0021ED04 File Offset: 0x0021CF04
		public void QuickSort()
		{
			if (this.Count > 1)
			{
				RBFinger<T> low = base.FindIndex(0, false);
				RBFinger<T> high = base.FindIndex(this.Count, false);
				this.QuickSort3(low, high);
				this.InsertionSortImpl();
			}
		}

		// Token: 0x060076B4 RID: 30388 RVA: 0x0021ED3F File Offset: 0x0021CF3F
		public void InsertionSort()
		{
			if (this.Count > 1)
			{
				this.InsertionSortImpl();
			}
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x0021ED50 File Offset: 0x0021CF50
		private void QuickSort3(RBFinger<T> low, RBFinger<T> high)
		{
			while (high - low > 15)
			{
				RBFinger<T> rbfinger = low;
				RBFinger<T> rbfinger2 = low + 1;
				RBFinger<T> rbfinger3 = high - 1;
				RBFinger<T> rbfinger4 = high;
				RBFinger<T> rbfinger5 = base.FindIndex((low.Index + high.Index) / 2, true);
				int num = this.Comparison(low.Item, rbfinger5.Item);
				if (num < 0)
				{
					num = this.Comparison(rbfinger5.Item, rbfinger3.Item);
					if (num >= 0)
					{
						if (num == 0)
						{
							rbfinger4 = rbfinger3;
						}
						else
						{
							num = this.Comparison(low.Item, rbfinger3.Item);
							if (num < 0)
							{
								this.Exchange(rbfinger5, rbfinger3);
							}
							else if (num == 0)
							{
								this.Exchange(rbfinger5, rbfinger3);
								rbfinger = rbfinger2;
							}
							else
							{
								this.Exchange(low, rbfinger5);
								this.Exchange(low, rbfinger3);
							}
						}
					}
				}
				else if (num == 0)
				{
					num = this.Comparison(low.Item, rbfinger3.Item);
					if (num < 0)
					{
						rbfinger = rbfinger2;
					}
					else if (num == 0)
					{
						rbfinger = rbfinger2;
						rbfinger4 = rbfinger3;
					}
					else
					{
						this.Exchange(low, rbfinger3);
						rbfinger4 = rbfinger3;
					}
				}
				else
				{
					num = this.Comparison(low.Item, rbfinger3.Item);
					if (num < 0)
					{
						this.Exchange(low, rbfinger5);
					}
					else if (num == 0)
					{
						this.Exchange(low, rbfinger5);
						rbfinger4 = rbfinger3;
					}
					else
					{
						num = this.Comparison(rbfinger5.Item, rbfinger3.Item);
						if (num < 0)
						{
							this.Exchange(low, rbfinger5);
							this.Exchange(rbfinger5, rbfinger3);
						}
						else if (num == 0)
						{
							this.Exchange(low, rbfinger3);
							rbfinger = rbfinger2;
						}
						else
						{
							this.Exchange(low, rbfinger3);
						}
					}
				}
				T item = rbfinger5.Item;
				RBFinger<T> rbfinger6 = rbfinger2;
				RBFinger<T> rbfinger7 = rbfinger3;
				for (;;)
				{
					if (rbfinger6 < rbfinger7)
					{
						num = this.Comparison(rbfinger6.Item, item);
						if (num < 0)
						{
							this.Trade(rbfinger, rbfinger2, rbfinger6);
							rbfinger += rbfinger6 - rbfinger2;
							rbfinger6 = (rbfinger2 = ++rbfinger6);
							continue;
						}
						if (num == 0)
						{
							rbfinger6 = ++rbfinger6;
							continue;
						}
					}
					while (rbfinger6 < rbfinger7)
					{
						RBFinger<T> rbfinger8 = rbfinger7 - 1;
						num = this.Comparison(rbfinger8.Item, item);
						if (num < 0)
						{
							break;
						}
						if (num == 0)
						{
							rbfinger7 = --rbfinger7;
						}
						else
						{
							this.Trade(rbfinger7, rbfinger3, rbfinger4);
							rbfinger4 -= rbfinger3 - rbfinger7;
							rbfinger7 = (rbfinger3 = --rbfinger7);
						}
					}
					num = rbfinger7 - rbfinger6;
					if (num == 0)
					{
						goto Block_18;
					}
					if (num != 1)
					{
						if (num == 2)
						{
							goto Block_20;
						}
						this.Exchange(rbfinger6, rbfinger7 - 1);
						this.Trade(rbfinger, rbfinger2, rbfinger6);
						rbfinger += rbfinger6 - rbfinger2;
						rbfinger6 = (rbfinger2 = ++rbfinger6);
						this.Trade(rbfinger7, rbfinger3, rbfinger4);
						rbfinger4 -= rbfinger3 - rbfinger7;
						rbfinger7 = (rbfinger3 = --rbfinger7);
					}
				}
				IL_38B:
				if (rbfinger2 - low < high - rbfinger3)
				{
					this.QuickSort3(low, rbfinger2);
					low = rbfinger3;
					continue;
				}
				this.QuickSort3(rbfinger3, high);
				high = rbfinger2;
				continue;
				Block_18:
				this.Trade(low, rbfinger, rbfinger2);
				rbfinger2 += low - rbfinger;
				this.Trade(rbfinger3, rbfinger4, high);
				rbfinger3 += high - rbfinger4;
				goto IL_38B;
				Block_20:
				this.Trade(low, rbfinger, rbfinger2);
				rbfinger2 += low - rbfinger + 1;
				this.Exchange(rbfinger2 - 1, rbfinger6 + 1);
				if (rbfinger2 > rbfinger6)
				{
					rbfinger6 = ++rbfinger6;
				}
				this.Trade(rbfinger3, rbfinger4, high);
				rbfinger3 += high - rbfinger4 - 1;
				this.Exchange(rbfinger6, rbfinger3);
				goto IL_38B;
			}
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x0021F120 File Offset: 0x0021D320
		private void Trade(RBFinger<T> left, RBFinger<T> mid, RBFinger<T> right)
		{
			int num = Math.Min(mid - left, right - mid);
			for (int i = 0; i < num; i++)
			{
				right = --right;
				this.Exchange(left, right);
				left = ++left;
			}
		}

		// Token: 0x060076B7 RID: 30391 RVA: 0x0021F168 File Offset: 0x0021D368
		private void Exchange(RBFinger<T> f1, RBFinger<T> f2)
		{
			T item = f1.Item;
			f1.SetItem(f2.Item);
			f2.SetItem(item);
		}

		// Token: 0x060076B8 RID: 30392 RVA: 0x0021F194 File Offset: 0x0021D394
		private void InsertionSortImpl()
		{
			RBFinger<T> finger = base.FindIndex(1, true);
			while (finger.Node != this)
			{
				RBFinger<T> newFinger = base.LocateItem(finger, this.Comparison);
				base.ReInsert(ref finger, newFinger);
				finger = ++finger;
			}
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x0021F1D4 File Offset: 0x0021D3D4
		internal RBNode<T> InsertNode(int index)
		{
			RBNode<T> result;
			base.LeftChild = base.InsertNode(this, this, base.LeftChild, index, out result);
			return result;
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x0021F1F9 File Offset: 0x0021D3F9
		internal void RemoveNode(int index)
		{
			base.LeftChild = base.DeleteNode(this, base.LeftChild, index);
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x0021F223 File Offset: 0x0021D423
		internal virtual RBNode<T> NewNode()
		{
			return new RBNode<T>();
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x0021F22C File Offset: 0x0021D42C
		internal void ForEach(Action<T> action)
		{
			foreach (T obj in this)
			{
				action(obj);
			}
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x0021F274 File Offset: 0x0021D474
		internal void ForEachUntil(Func<T, bool> action)
		{
			foreach (T arg in this)
			{
				if (action(arg))
				{
					break;
				}
			}
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x0021F2C0 File Offset: 0x0021D4C0
		internal int IndexOf(T item, Func<T, T, bool> AreEqual)
		{
			if (this.Comparison != null)
			{
				RBFinger<T> finger = base.Find(item, this.Comparison);
				while (finger.Found && !AreEqual(finger.Item, item))
				{
					finger = ++finger;
					finger.Found = (finger.IsValid && this.Comparison(finger.Item, item) == 0);
				}
				if (!finger.Found)
				{
					return -1;
				}
				return finger.Index;
			}
			else
			{
				int result = 0;
				this.ForEachUntil(delegate(T x)
				{
					if (AreEqual(x, item))
					{
						return true;
					}
					int result;
					result++;
					result = result;
					return false;
				});
				if (result >= this.Count)
				{
					return -1;
				}
				return result;
			}
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x0021F3A8 File Offset: 0x0021D5A8
		public virtual int IndexOf(T item)
		{
			return this.IndexOf(item, (T x, T y) => ItemsControl.EqualsEx(x, y));
		}

		// Token: 0x060076C0 RID: 30400 RVA: 0x0021F3D0 File Offset: 0x0021D5D0
		public void Insert(int index, T item)
		{
			this.VerifyIndex(index, 1);
			RBFinger<T> finger = base.FindIndex(index, false);
			this.Insert(finger, item, false);
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x0021F3F8 File Offset: 0x0021D5F8
		public void RemoveAt(int index)
		{
			this.VerifyIndex(index, 0);
			this.SaveTree();
			int leftSize = base.LeftSize;
			RBFinger<T> rbfinger = base.FindIndex(index, true);
			base.RemoveAt(ref rbfinger);
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
			this.Verify(leftSize - 1, true);
		}

		// Token: 0x17001C3C RID: 7228
		public T this[int index]
		{
			get
			{
				this.VerifyIndex(index, 0);
				RBFinger<T> rbfinger = base.FindIndex(index, true);
				return rbfinger.Node.GetItemAt(rbfinger.Offset);
			}
			set
			{
				this.VerifyIndex(index, 0);
				RBFinger<T> rbfinger = base.FindIndex(index, true);
				rbfinger.Node.SetItemAt(rbfinger.Offset, value);
			}
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x0021F4B4 File Offset: 0x0021D6B4
		public void Add(T item)
		{
			this.SaveTree();
			int leftSize = base.LeftSize;
			RBNode<T> rbnode = base.LeftChild;
			if (rbnode == null)
			{
				rbnode = this.InsertNode(0);
				rbnode.InsertAt(0, item, null, null);
			}
			else
			{
				while (rbnode.RightChild != null)
				{
					rbnode = rbnode.RightChild;
				}
				if (rbnode.Size < 64)
				{
					rbnode.InsertAt(rbnode.Size, item, null, null);
				}
				else
				{
					rbnode = this.InsertNode(base.LeftSize);
					rbnode.InsertAt(0, item, null, null);
				}
			}
			base.LeftChild.IsRed = false;
			this.Verify(leftSize + 1, false);
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x0021F543 File Offset: 0x0021D743
		public void Clear()
		{
			base.LeftChild = null;
			base.LeftSize = 0;
		}

		// Token: 0x060076C6 RID: 30406 RVA: 0x0021F554 File Offset: 0x0021D754
		public bool Contains(T item)
		{
			return base.Find(item, this.Comparison).Found;
		}

		// Token: 0x060076C7 RID: 30407 RVA: 0x0021F578 File Offset: 0x0021D778
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex + this.Count > array.Length)
			{
				throw new ArgumentException(SR.Get("Argument_InvalidOffLen"));
			}
			foreach (T t in this)
			{
				array[arrayIndex] = t;
				arrayIndex++;
			}
		}

		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x060076C8 RID: 30408 RVA: 0x0021F600 File Offset: 0x0021D800
		public int Count
		{
			get
			{
				return base.LeftSize;
			}
		}

		// Token: 0x17001C3E RID: 7230
		// (get) Token: 0x060076C9 RID: 30409 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x0021F608 File Offset: 0x0021D808
		public bool Remove(T item)
		{
			RBFinger<T> rbfinger = base.Find(item, this.Comparison);
			if (rbfinger.Found)
			{
				base.RemoveAt(ref rbfinger);
			}
			if (base.LeftChild != null)
			{
				base.LeftChild.IsRed = false;
			}
			return rbfinger.Found;
		}

		// Token: 0x060076CB RID: 30411 RVA: 0x0021F64F File Offset: 0x0021D84F
		public IEnumerator<T> GetEnumerator()
		{
			RBFinger<T> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				yield return finger.Node.GetItemAt(finger.Offset);
				RBFinger<T> rbfinger = ++finger;
				finger = rbfinger;
			}
			yield break;
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x0021F65E File Offset: 0x0021D85E
		IEnumerator IEnumerable.GetEnumerator()
		{
			RBFinger<T> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				yield return finger.Node.GetItemAt(finger.Offset);
				RBFinger<T> rbfinger = ++finger;
				finger = rbfinger;
			}
			yield break;
		}

		// Token: 0x060076CD RID: 30413 RVA: 0x0021F66D File Offset: 0x0021D86D
		private void VerifyIndex(int index, int delta = 0)
		{
			if (index < 0 || index >= this.Count + delta)
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x00002137 File Offset: 0x00000337
		private void Verify(int expectedSize, bool checkSort = true)
		{
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x00002137 File Offset: 0x00000337
		private void SaveTree()
		{
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x00002137 File Offset: 0x00000337
		public void LoadTree(string s)
		{
		}

		// Token: 0x04003893 RID: 14483
		private const int QuickSortThreshold = 15;

		// Token: 0x04003894 RID: 14484
		private Comparison<T> _comparison;
	}
}
