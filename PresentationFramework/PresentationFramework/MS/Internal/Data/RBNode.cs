using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x02000740 RID: 1856
	internal class RBNode<T> : INotifyPropertyChanged
	{
		// Token: 0x0600767C RID: 30332 RVA: 0x0021D977 File Offset: 0x0021BB77
		public RBNode()
		{
			this._data = new T[64];
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x0000326D File Offset: 0x0000146D
		protected RBNode(bool b)
		{
		}

		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x0600767E RID: 30334 RVA: 0x0021D98C File Offset: 0x0021BB8C
		// (set) Token: 0x0600767F RID: 30335 RVA: 0x0021D994 File Offset: 0x0021BB94
		public RBNode<T> LeftChild { get; set; }

		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x06007680 RID: 30336 RVA: 0x0021D99D File Offset: 0x0021BB9D
		// (set) Token: 0x06007681 RID: 30337 RVA: 0x0021D9A5 File Offset: 0x0021BBA5
		public RBNode<T> RightChild { get; set; }

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x06007682 RID: 30338 RVA: 0x0021D9AE File Offset: 0x0021BBAE
		// (set) Token: 0x06007683 RID: 30339 RVA: 0x0021D9B6 File Offset: 0x0021BBB6
		public RBNode<T> Parent { get; set; }

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x06007684 RID: 30340 RVA: 0x0021D9BF File Offset: 0x0021BBBF
		// (set) Token: 0x06007685 RID: 30341 RVA: 0x0021D9C7 File Offset: 0x0021BBC7
		public bool IsRed { get; set; }

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x06007686 RID: 30342 RVA: 0x00016748 File Offset: 0x00014948
		public virtual bool HasData
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001C38 RID: 7224
		// (get) Token: 0x06007687 RID: 30343 RVA: 0x0021D9D0 File Offset: 0x0021BBD0
		// (set) Token: 0x06007688 RID: 30344 RVA: 0x0021D9D8 File Offset: 0x0021BBD8
		public int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
				this.OnPropertyChanged("Size");
			}
		}

		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x06007689 RID: 30345 RVA: 0x0021D9EC File Offset: 0x0021BBEC
		// (set) Token: 0x0600768A RID: 30346 RVA: 0x0021D9F4 File Offset: 0x0021BBF4
		public int LeftSize
		{
			get
			{
				return this._leftSize;
			}
			set
			{
				this._leftSize = value;
				this.OnPropertyChanged("LeftSize");
			}
		}

		// Token: 0x0600768B RID: 30347 RVA: 0x0021DA08 File Offset: 0x0021BC08
		public T GetItemAt(int offset)
		{
			return this._data[offset];
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x0021DA16 File Offset: 0x0021BC16
		public virtual T SetItemAt(int offset, T x)
		{
			this._data[offset] = x;
			return x;
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x0021DA26 File Offset: 0x0021BC26
		public int OffsetOf(T x)
		{
			return Array.IndexOf<T>(this._data, x);
		}

		// Token: 0x0600768E RID: 30350 RVA: 0x0021DA34 File Offset: 0x0021BC34
		internal RBNode<T> GetSuccessor()
		{
			RBNode<T> rbnode2;
			if (this.RightChild == null)
			{
				RBNode<T> rbnode = this;
				rbnode2 = rbnode.Parent;
				while (rbnode2.RightChild == rbnode)
				{
					rbnode = rbnode2;
					rbnode2 = rbnode.Parent;
				}
				return rbnode2;
			}
			rbnode2 = this.RightChild;
			for (RBNode<T> rbnode = rbnode2.LeftChild; rbnode != null; rbnode = rbnode2.LeftChild)
			{
				rbnode2 = rbnode;
			}
			return rbnode2;
		}

		// Token: 0x0600768F RID: 30351 RVA: 0x0021DA88 File Offset: 0x0021BC88
		internal RBNode<T> GetPredecessor()
		{
			RBNode<T> rbnode2;
			if (this.LeftChild == null)
			{
				RBNode<T> rbnode = this;
				rbnode2 = rbnode.Parent;
				while (rbnode2 != null && rbnode2.LeftChild == rbnode)
				{
					rbnode = rbnode2;
					rbnode2 = rbnode.Parent;
				}
				return rbnode2;
			}
			rbnode2 = this.LeftChild;
			for (RBNode<T> rbnode = rbnode2.RightChild; rbnode != null; rbnode = rbnode2.RightChild)
			{
				rbnode2 = rbnode;
			}
			return rbnode2;
		}

		// Token: 0x06007690 RID: 30352 RVA: 0x0021DADC File Offset: 0x0021BCDC
		protected RBFinger<T> FindIndex(int index, bool exists = true)
		{
			int num = exists ? 1 : 0;
			RBFinger<T> result;
			if (index + num <= this.LeftSize)
			{
				if (this.LeftChild == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = 0,
						Index = 0,
						Found = false
					};
				}
				else
				{
					result = this.LeftChild.FindIndex(index, exists);
				}
			}
			else if (index < this.LeftSize + this.Size)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = index - this.LeftSize,
					Index = index,
					Found = true
				};
			}
			else if (this.RightChild == null)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = this.Size,
					Index = this.LeftSize + this.Size,
					Found = false
				};
			}
			else
			{
				result = this.RightChild.FindIndex(index - this.LeftSize - this.Size, exists);
				result.Index += this.LeftSize + this.Size;
			}
			return result;
		}

		// Token: 0x06007691 RID: 30353 RVA: 0x0021DC08 File Offset: 0x0021BE08
		protected RBFinger<T> Find(T x, Comparison<T> comparison)
		{
			int num = (this._data != null) ? comparison(x, this.GetItemAt(0)) : -1;
			RBFinger<T> result;
			int compHigh;
			if (num <= 0)
			{
				if (this.LeftChild == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = 0,
						Index = 0,
						Found = (num == 0)
					};
				}
				else
				{
					result = this.LeftChild.Find(x, comparison);
					if (num == 0 && !result.Found)
					{
						result = new RBFinger<T>
						{
							Node = this,
							Offset = 0,
							Index = this.LeftSize,
							Found = true
						};
					}
				}
			}
			else if ((compHigh = comparison(x, this.GetItemAt(this.Size - 1))) <= 0)
			{
				bool found;
				int num2 = this.BinarySearch(x, 1, this.Size - 1, comparison, compHigh, out found);
				result = new RBFinger<T>
				{
					Node = this,
					Offset = num2,
					Index = this.LeftSize + num2,
					Found = found
				};
			}
			else if (this.RightChild == null)
			{
				result = new RBFinger<T>
				{
					Node = this,
					Offset = this.Size,
					Index = this.LeftSize + this.Size
				};
			}
			else
			{
				result = this.RightChild.Find(x, comparison);
				result.Index += this.LeftSize + this.Size;
			}
			return result;
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x0021DD94 File Offset: 0x0021BF94
		protected RBFinger<T> BoundedSearch(T x, int low, int high, Comparison<T> comparison)
		{
			RBNode<T> rbnode = this.LeftChild;
			RBNode<T> rbnode2 = this.RightChild;
			int num = 0;
			int num2 = this.Size;
			int num3;
			if (high <= this.LeftSize)
			{
				num3 = -1;
			}
			else
			{
				if (low >= this.LeftSize)
				{
					rbnode = null;
					num = low - this.LeftSize;
				}
				num3 = ((num < this.Size) ? comparison(x, this.GetItemAt(num)) : 1);
			}
			RBFinger<T> result;
			if (num3 <= 0)
			{
				if (rbnode == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = num,
						Index = num,
						Found = (num3 == 0)
					};
				}
				else
				{
					result = rbnode.BoundedSearch(x, low, high, comparison);
					if (num3 == 0 && !result.Found)
					{
						result = new RBFinger<T>
						{
							Node = this,
							Offset = 0,
							Index = this.LeftSize,
							Found = true
						};
					}
				}
				return result;
			}
			int num4;
			if (this.LeftSize + this.Size <= low)
			{
				num4 = 1;
			}
			else
			{
				if (this.LeftSize + this.Size >= high)
				{
					rbnode2 = null;
					num2 = high - this.LeftSize;
				}
				num4 = comparison(x, this.GetItemAt(num2 - 1));
			}
			if (num4 > 0)
			{
				if (rbnode2 == null)
				{
					result = new RBFinger<T>
					{
						Node = this,
						Offset = num2,
						Index = this.LeftSize + num2,
						Found = false
					};
				}
				else
				{
					int num5 = this.LeftSize + this.Size;
					result = rbnode2.BoundedSearch(x, low - num5, high - num5, comparison);
					result.Index += num5;
				}
				return result;
			}
			bool found;
			int num6 = this.BinarySearch(x, num + 1, num2 - 1, comparison, num4, out found);
			result = new RBFinger<T>
			{
				Node = this,
				Offset = num6,
				Index = this.LeftSize + num6,
				Found = found
			};
			return result;
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x0021DF80 File Offset: 0x0021C180
		private int BinarySearch(T x, int low, int high, Comparison<T> comparison, int compHigh, out bool found)
		{
			while (high - low > 3)
			{
				int num = (high + low) / 2;
				int num2 = comparison(x, this.GetItemAt(num));
				if (num2 <= 0)
				{
					compHigh = num2;
					high = num;
				}
				else
				{
					low = num + 1;
				}
			}
			int num3 = 0;
			while (low < high)
			{
				num3 = comparison(x, this.GetItemAt(low));
				if (num3 <= 0)
				{
					break;
				}
				low++;
			}
			if (low == high)
			{
				num3 = compHigh;
			}
			found = (num3 == 0);
			return low;
		}

		// Token: 0x06007694 RID: 30356 RVA: 0x0021DFEC File Offset: 0x0021C1EC
		protected RBFinger<T> LocateItem(RBFinger<T> finger, Comparison<T> comparison)
		{
			RBNode<T> rbnode = finger.Node;
			int num = finger.Index - finger.Offset;
			T itemAt = rbnode.GetItemAt(finger.Offset);
			for (int i = finger.Offset - 1; i >= 0; i--)
			{
				if (comparison(itemAt, rbnode.GetItemAt(i)) >= 0)
				{
					return new RBFinger<T>
					{
						Node = rbnode,
						Offset = i + 1,
						Index = num + i + 1
					};
				}
			}
			RBNode<T> rbnode2 = rbnode;
			for (RBNode<T> parent = rbnode2.Parent; parent != null; parent = rbnode2.Parent)
			{
				while (parent != null && rbnode2 == parent.LeftChild)
				{
					rbnode2 = parent;
					parent = rbnode2.Parent;
				}
				if (parent == null || comparison(itemAt, parent.GetItemAt(parent.Size - 1)) >= 0)
				{
					break;
				}
				num = num - rbnode.LeftSize - parent.Size;
				if (comparison(itemAt, parent.GetItemAt(0)) >= 0)
				{
					bool flag;
					int num2 = parent.BinarySearch(itemAt, 1, parent.Size - 1, comparison, -1, out flag);
					return new RBFinger<T>
					{
						Node = parent,
						Offset = num2,
						Index = num + num2
					};
				}
				rbnode2 = (rbnode = parent);
			}
			if (rbnode.LeftChild != null)
			{
				RBFinger<T> result = rbnode.LeftChild.Find(itemAt, comparison);
				if (result.Offset == result.Node.Size)
				{
					result = new RBFinger<T>
					{
						Node = result.Node.GetSuccessor(),
						Offset = 0,
						Index = result.Index
					};
				}
				return result;
			}
			return new RBFinger<T>
			{
				Node = rbnode,
				Offset = 0,
				Index = num
			};
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x0021E1BA File Offset: 0x0021C3BA
		protected virtual void Copy(RBNode<T> sourceNode, int sourceOffset, RBNode<T> destNode, int destOffset, int count)
		{
			Array.Copy(sourceNode._data, sourceOffset, destNode._data, destOffset, count);
		}

		// Token: 0x06007696 RID: 30358 RVA: 0x0021E1D4 File Offset: 0x0021C3D4
		protected void ReInsert(ref RBFinger<T> oldFinger, RBFinger<T> newFinger)
		{
			RBNode<T> node = oldFinger.Node;
			RBNode<T> node2 = newFinger.Node;
			int offset = oldFinger.Offset;
			int offset2 = newFinger.Offset;
			T itemAt = node.GetItemAt(oldFinger.Offset);
			if (node == node2)
			{
				int num = offset - offset2;
				if (num != 0)
				{
					this.Copy(node, offset2, node, offset2 + 1, num);
					node.SetItemAt(offset2, itemAt);
					return;
				}
			}
			else
			{
				if (node2.Size < 64)
				{
					node2.InsertAt(offset2, itemAt, null, null);
					node.RemoveAt(ref oldFinger);
					return;
				}
				RBNode<T> rbnode = node2.GetSuccessor();
				if (rbnode == node)
				{
					T itemAt2 = node2.GetItemAt(63);
					this.Copy(node2, offset2, node2, offset2 + 1, 64 - offset2 - 1);
					node2.SetItemAt(offset2, itemAt);
					this.Copy(node, 0, node, 1, offset);
					node.SetItemAt(0, itemAt2);
					return;
				}
				if (rbnode.Size < 64)
				{
					node2.InsertAt(offset2, itemAt, rbnode, null);
				}
				else
				{
					RBNode<T> succsucc = rbnode;
					rbnode = this.InsertNodeAfter(node2);
					node2.InsertAt(offset2, itemAt, rbnode, succsucc);
				}
				node.RemoveAt(ref oldFinger);
			}
		}

		// Token: 0x06007697 RID: 30359 RVA: 0x0021E2D8 File Offset: 0x0021C4D8
		protected void RemoveAt(ref RBFinger<T> finger)
		{
			RBNode<T> node = finger.Node;
			int offset = finger.Offset;
			this.Copy(node, offset + 1, node, offset, node.Size - offset - 1);
			node.ChangeSize(-1);
			node.SetItemAt(node.Size, default(T));
			if (node.Size == 0)
			{
				finger.Node = node.GetSuccessor();
				finger.Offset = 0;
				int index;
				RBTree<T> rootAndIndex = node.GetRootAndIndex(node, out index);
				rootAndIndex.RemoveNode(index);
			}
			finger.Offset--;
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x0021E364 File Offset: 0x0021C564
		protected RBNode<T> InsertNodeAfter(RBNode<T> node)
		{
			int num;
			RBTree<T> rootAndIndex = this.GetRootAndIndex(node, out num);
			return rootAndIndex.InsertNode(num + node.Size);
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x0021E38C File Offset: 0x0021C58C
		protected RBTree<T> GetRoot(RBNode<T> node)
		{
			for (RBNode<T> parent = node.Parent; parent != null; parent = node.Parent)
			{
				node = parent;
			}
			return (RBTree<T>)node;
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x0021E3B8 File Offset: 0x0021C5B8
		protected RBTree<T> GetRootAndIndex(RBNode<T> node, out int index)
		{
			index = node.LeftSize;
			for (RBNode<T> parent = node.Parent; parent != null; parent = node.Parent)
			{
				if (node == parent.RightChild)
				{
					index += parent.LeftSize + parent.Size;
				}
				node = parent;
			}
			return (RBTree<T>)node;
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x0021E404 File Offset: 0x0021C604
		internal void InsertAt(int offset, T x, RBNode<T> successor = null, RBNode<T> succsucc = null)
		{
			if (this.Size < 64)
			{
				this.Copy(this, offset, this, offset + 1, this.Size - offset);
				this.SetItemAt(offset, x);
				this.ChangeSize(1);
				return;
			}
			if (successor.Size != 0)
			{
				int num = (this.Size + successor.Size + 1) / 2;
				if (offset < num)
				{
					this.Copy(successor, 0, successor, 64 - num + 1, successor.Size);
					this.Copy(this, num - 1, successor, 0, 64 - num + 1);
					this.Copy(this, offset, this, offset + 1, num - 1 - offset);
					this.SetItemAt(offset, x);
				}
				else
				{
					this.Copy(successor, 0, successor, 64 - num, successor.Size);
					this.Copy(this, num, successor, 0, 64 - num);
					this.Copy(successor, offset - num, successor, offset - num + 1, successor.Size + 64 - offset);
					successor.SetItemAt(offset - num, x);
				}
				this.ChangeSize(num - 64);
				successor.ChangeSize(64 - num + 1);
				return;
			}
			if (succsucc != null)
			{
				int num2 = 21;
				this.Copy(successor, 0, successor, num2, successor.Size);
				this.Copy(this, 64 - num2, successor, 0, num2);
				this.Copy(succsucc, 0, successor, num2 + successor.Size, num2);
				this.Copy(succsucc, num2, succsucc, 0, 64 - num2);
				if (offset <= 64 - num2)
				{
					this.Copy(this, offset, this, offset + 1, 64 - num2 - offset);
					this.SetItemAt(offset, x);
					this.ChangeSize(1 - num2);
					successor.ChangeSize(num2 + num2);
				}
				else
				{
					this.Copy(successor, offset - (64 - num2), successor, offset - (64 - num2) + 1, successor.Size + num2 + num2 - (offset - (64 - num2)));
					successor.SetItemAt(offset - (64 - num2), x);
					this.ChangeSize(-num2);
					successor.ChangeSize(num2 + num2 + 1);
				}
				succsucc.ChangeSize(-num2);
				return;
			}
			if (offset < 64)
			{
				successor.InsertAt(0, this.GetItemAt(63), null, null);
				this.Copy(this, offset, this, offset + 1, 64 - offset - 1);
				this.SetItemAt(offset, x);
				return;
			}
			successor.InsertAt(0, x, null, null);
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x0021E610 File Offset: 0x0021C810
		protected RBNode<T> InsertNode(RBTree<T> root, RBNode<T> parent, RBNode<T> node, int index, out RBNode<T> newNode)
		{
			if (node == null)
			{
				newNode = root.NewNode();
				newNode.Parent = parent;
				newNode.IsRed = true;
				return newNode;
			}
			if (index <= node.LeftSize)
			{
				node.LeftChild = this.InsertNode(root, node, node.LeftChild, index, out newNode);
			}
			else
			{
				node.RightChild = this.InsertNode(root, node, node.RightChild, index - node.LeftSize - node.Size, out newNode);
			}
			node = this.Fixup(node);
			return node;
		}

		// Token: 0x0600769D RID: 30365 RVA: 0x0021E694 File Offset: 0x0021C894
		protected void ChangeSize(int delta)
		{
			if (delta == 0)
			{
				return;
			}
			for (int i = this.Size + delta; i < this.Size; i++)
			{
				this._data[i] = default(T);
			}
			this.Size += delta;
			RBNode<T> rbnode = this;
			for (RBNode<T> parent = rbnode.Parent; parent != null; parent = rbnode.Parent)
			{
				if (parent.LeftChild == rbnode)
				{
					parent.LeftSize += delta;
				}
				rbnode = parent;
			}
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x0021E710 File Offset: 0x0021C910
		private RBNode<T> Substitute(RBNode<T> node, RBNode<T> sub, RBNode<T> parent)
		{
			sub.LeftChild = node.LeftChild;
			sub.RightChild = node.RightChild;
			sub.LeftSize = node.LeftSize;
			sub.Parent = node.Parent;
			sub.IsRed = node.IsRed;
			if (sub.LeftChild != null)
			{
				sub.LeftChild.Parent = sub;
			}
			if (sub.RightChild != null)
			{
				sub.RightChild.Parent = sub;
			}
			return sub;
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x0021E784 File Offset: 0x0021C984
		protected RBNode<T> DeleteNode(RBNode<T> parent, RBNode<T> node, int index)
		{
			if (index < node.LeftSize || (index == node.LeftSize && node.Size > 0))
			{
				if (!this.IsNodeRed(node.LeftChild) && !this.IsNodeRed(node.LeftChild.LeftChild))
				{
					node = this.MoveRedLeft(node);
				}
				node.LeftChild = this.DeleteNode(node, node.LeftChild, index);
			}
			else
			{
				bool flag = index == node.LeftSize;
				if (this.IsNodeRed(node.LeftChild))
				{
					node = node.RotateRight();
					flag = false;
				}
				if (flag && node.RightChild == null)
				{
					return null;
				}
				if (!this.IsNodeRed(node.RightChild) && !this.IsNodeRed(node.RightChild.LeftChild))
				{
					RBNode<T> rbnode = node;
					node = this.MoveRedRight(node);
					flag = (flag && rbnode == node);
				}
				if (flag)
				{
					RBNode<T> sub;
					node.RightChild = this.DeleteLeftmost(node.RightChild, out sub);
					node = this.Substitute(node, sub, parent);
				}
				else
				{
					node.RightChild = this.DeleteNode(node, node.RightChild, index - node.LeftSize - node.Size);
				}
			}
			return this.Fixup(node);
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x0021E8A4 File Offset: 0x0021CAA4
		private RBNode<T> DeleteLeftmost(RBNode<T> node, out RBNode<T> leftmost)
		{
			if (node.LeftChild == null)
			{
				leftmost = node;
				return null;
			}
			if (!this.IsNodeRed(node.LeftChild) && !this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = this.MoveRedLeft(node);
			}
			node.LeftChild = this.DeleteLeftmost(node.LeftChild, out leftmost);
			node.LeftSize -= leftmost.Size;
			return this.Fixup(node);
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x0021E916 File Offset: 0x0021CB16
		private bool IsNodeRed(RBNode<T> node)
		{
			return node != null && node.IsRed;
		}

		// Token: 0x060076A2 RID: 30370 RVA: 0x0021E924 File Offset: 0x0021CB24
		private RBNode<T> RotateLeft()
		{
			RBNode<T> rightChild = this.RightChild;
			rightChild.LeftSize += this.LeftSize + this.Size;
			rightChild.IsRed = this.IsRed;
			rightChild.Parent = this.Parent;
			this.RightChild = rightChild.LeftChild;
			if (this.RightChild != null)
			{
				this.RightChild.Parent = this;
			}
			rightChild.LeftChild = this;
			this.IsRed = true;
			this.Parent = rightChild;
			return rightChild;
		}

		// Token: 0x060076A3 RID: 30371 RVA: 0x0021E9A0 File Offset: 0x0021CBA0
		private RBNode<T> RotateRight()
		{
			RBNode<T> leftChild = this.LeftChild;
			this.LeftSize -= leftChild.LeftSize + leftChild.Size;
			leftChild.IsRed = this.IsRed;
			leftChild.Parent = this.Parent;
			this.LeftChild = leftChild.RightChild;
			if (this.LeftChild != null)
			{
				this.LeftChild.Parent = this;
			}
			leftChild.RightChild = this;
			this.IsRed = true;
			this.Parent = leftChild;
			return leftChild;
		}

		// Token: 0x060076A4 RID: 30372 RVA: 0x0021EA1C File Offset: 0x0021CC1C
		private void ColorFlip()
		{
			this.IsRed = !this.IsRed;
			this.LeftChild.IsRed = !this.LeftChild.IsRed;
			this.RightChild.IsRed = !this.RightChild.IsRed;
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x0021EA6C File Offset: 0x0021CC6C
		private RBNode<T> Fixup(RBNode<T> node)
		{
			if (!this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.RightChild))
			{
				node = node.RotateLeft();
			}
			if (this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = node.RotateRight();
			}
			if (this.IsNodeRed(node.LeftChild) && this.IsNodeRed(node.RightChild))
			{
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x060076A6 RID: 30374 RVA: 0x0021EAE9 File Offset: 0x0021CCE9
		private RBNode<T> MoveRedRight(RBNode<T> node)
		{
			node.ColorFlip();
			if (this.IsNodeRed(node.LeftChild.LeftChild))
			{
				node = node.RotateRight();
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x060076A7 RID: 30375 RVA: 0x0021EB13 File Offset: 0x0021CD13
		private RBNode<T> MoveRedLeft(RBNode<T> node)
		{
			node.ColorFlip();
			if (this.IsNodeRed(node.RightChild.LeftChild))
			{
				node.RightChild = node.RightChild.RotateRight();
				node = node.RotateLeft();
				node.ColorFlip();
			}
			return node;
		}

		// Token: 0x1400015C RID: 348
		// (add) Token: 0x060076A8 RID: 30376 RVA: 0x0021EB50 File Offset: 0x0021CD50
		// (remove) Token: 0x060076A9 RID: 30377 RVA: 0x0021EB88 File Offset: 0x0021CD88
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060076AA RID: 30378 RVA: 0x0021EBBD File Offset: 0x0021CDBD
		protected void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x04003889 RID: 14473
		protected const int MaxSize = 64;

		// Token: 0x0400388A RID: 14474
		protected const int BinarySearchThreshold = 3;

		// Token: 0x0400388F RID: 14479
		private int _size;

		// Token: 0x04003890 RID: 14480
		private int _leftSize;

		// Token: 0x04003892 RID: 14482
		private T[] _data;
	}
}
