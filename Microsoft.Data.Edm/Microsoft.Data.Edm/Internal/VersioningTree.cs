using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x0200011F RID: 287
	internal class VersioningTree<TKey, TValue>
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x0000D659 File Offset: 0x0000B859
		public VersioningTree(TKey key, TValue value, VersioningTree<TKey, TValue> leftChild, VersioningTree<TKey, TValue> rightChild)
		{
			this.Key = key;
			this.Value = value;
			this.Height = VersioningTree<TKey, TValue>.Max(VersioningTree<TKey, TValue>.GetHeight(leftChild), VersioningTree<TKey, TValue>.GetHeight(rightChild)) + 1;
			this.LeftChild = leftChild;
			this.RightChild = rightChild;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0000D698 File Offset: 0x0000B898
		public TValue GetValue(TKey key, Func<TKey, TKey, int> compareFunction)
		{
			TValue result;
			if (this.TryGetValue(key, compareFunction, out result))
			{
				return result;
			}
			throw new KeyNotFoundException(key.ToString());
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public bool TryGetValue(TKey key, Func<TKey, TKey, int> compareFunction, out TValue value)
		{
			int num;
			for (VersioningTree<TKey, TValue> versioningTree = this; versioningTree != null; versioningTree = ((num < 0) ? versioningTree.LeftChild : versioningTree.RightChild))
			{
				num = compareFunction(key, versioningTree.Key);
				if (num == 0)
				{
					value = versioningTree.Value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000D718 File Offset: 0x0000B918
		public VersioningTree<TKey, TValue> SetKeyValue(TKey key, TValue value, Func<TKey, TKey, int> compareFunction)
		{
			VersioningTree<TKey, TValue> leftChild = this.LeftChild;
			VersioningTree<TKey, TValue> rightChild = this.RightChild;
			int num = compareFunction(key, this.Key);
			if (num < 0)
			{
				if (VersioningTree<TKey, TValue>.GetHeight(leftChild) > VersioningTree<TKey, TValue>.GetHeight(rightChild))
				{
					int num2 = compareFunction(key, leftChild.Key);
					VersioningTree<TKey, TValue> leftChild2 = (num2 < 0) ? VersioningTree<TKey, TValue>.SetKeyValue(leftChild.LeftChild, key, value, compareFunction) : leftChild.LeftChild;
					VersioningTree<TKey, TValue> rightChild2 = new VersioningTree<TKey, TValue>(this.Key, this.Value, (num2 > 0) ? VersioningTree<TKey, TValue>.SetKeyValue(leftChild.RightChild, key, value, compareFunction) : leftChild.RightChild, rightChild);
					return new VersioningTree<TKey, TValue>((num2 == 0) ? key : leftChild.Key, (num2 == 0) ? value : leftChild.Value, leftChild2, rightChild2);
				}
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, VersioningTree<TKey, TValue>.SetKeyValue(leftChild, key, value, compareFunction), rightChild);
			}
			else
			{
				if (num == 0)
				{
					return new VersioningTree<TKey, TValue>(key, value, leftChild, rightChild);
				}
				if (VersioningTree<TKey, TValue>.GetHeight(leftChild) < VersioningTree<TKey, TValue>.GetHeight(rightChild))
				{
					int num3 = compareFunction(key, rightChild.Key);
					VersioningTree<TKey, TValue> leftChild3 = new VersioningTree<TKey, TValue>(this.Key, this.Value, leftChild, (num3 < 0) ? VersioningTree<TKey, TValue>.SetKeyValue(rightChild.LeftChild, key, value, compareFunction) : rightChild.LeftChild);
					VersioningTree<TKey, TValue> rightChild3 = (num3 > 0) ? VersioningTree<TKey, TValue>.SetKeyValue(rightChild.RightChild, key, value, compareFunction) : rightChild.RightChild;
					return new VersioningTree<TKey, TValue>((num3 == 0) ? key : rightChild.Key, (num3 == 0) ? value : rightChild.Value, leftChild3, rightChild3);
				}
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, leftChild, VersioningTree<TKey, TValue>.SetKeyValue(rightChild, key, value, compareFunction));
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		public VersioningTree<TKey, TValue> Remove(TKey key, Func<TKey, TKey, int> compareFunction)
		{
			int num = compareFunction(key, this.Key);
			if (num < 0)
			{
				if (this.LeftChild == null)
				{
					throw new KeyNotFoundException(key.ToString());
				}
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, this.LeftChild.Remove(key, compareFunction), this.RightChild);
			}
			else if (num == 0)
			{
				if (this.LeftChild == null)
				{
					return this.RightChild;
				}
				if (this.RightChild == null)
				{
					return this.LeftChild;
				}
				if (this.LeftChild.Height < this.RightChild.Height)
				{
					return this.LeftChild.MakeRightmost(this.RightChild);
				}
				return this.RightChild.MakeLeftmost(this.LeftChild);
			}
			else
			{
				if (this.RightChild == null)
				{
					throw new KeyNotFoundException(key.ToString());
				}
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, this.LeftChild, this.RightChild.Remove(key, compareFunction));
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000D99F File Offset: 0x0000BB9F
		private static VersioningTree<TKey, TValue> SetKeyValue(VersioningTree<TKey, TValue> me, TKey key, TValue value, Func<TKey, TKey, int> compareFunction)
		{
			if (me == null)
			{
				return new VersioningTree<TKey, TValue>(key, value, null, null);
			}
			return me.SetKeyValue(key, value, compareFunction);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0000D9B7 File Offset: 0x0000BBB7
		private static int GetHeight(VersioningTree<TKey, TValue> tree)
		{
			if (tree != null)
			{
				return tree.Height;
			}
			return 0;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		private static int Max(int x, int y)
		{
			if (x <= y)
			{
				return y;
			}
			return x;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
		private VersioningTree<TKey, TValue> MakeLeftmost(VersioningTree<TKey, TValue> leftmost)
		{
			if (this.LeftChild == null)
			{
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, leftmost, this.RightChild);
			}
			return new VersioningTree<TKey, TValue>(this.Key, this.Value, this.LeftChild.MakeLeftmost(leftmost), this.RightChild);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000DA24 File Offset: 0x0000BC24
		private VersioningTree<TKey, TValue> MakeRightmost(VersioningTree<TKey, TValue> rightmost)
		{
			if (this.RightChild == null)
			{
				return new VersioningTree<TKey, TValue>(this.Key, this.Value, this.LeftChild, rightmost);
			}
			return new VersioningTree<TKey, TValue>(this.Key, this.Value, this.LeftChild, this.RightChild.MakeRightmost(rightmost));
		}

		// Token: 0x040001FB RID: 507
		public readonly TKey Key;

		// Token: 0x040001FC RID: 508
		public readonly TValue Value;

		// Token: 0x040001FD RID: 509
		public readonly int Height;

		// Token: 0x040001FE RID: 510
		public readonly VersioningTree<TKey, TValue> LeftChild;

		// Token: 0x040001FF RID: 511
		public readonly VersioningTree<TKey, TValue> RightChild;
	}
}
