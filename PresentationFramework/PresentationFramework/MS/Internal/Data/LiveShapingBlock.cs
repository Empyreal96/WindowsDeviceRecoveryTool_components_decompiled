using System;
using System.Collections.Generic;

namespace MS.Internal.Data
{
	// Token: 0x0200072C RID: 1836
	internal class LiveShapingBlock : RBNode<LiveShapingItem>
	{
		// Token: 0x0600757A RID: 30074 RVA: 0x00218E21 File Offset: 0x00217021
		internal LiveShapingBlock()
		{
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x00218E29 File Offset: 0x00217029
		internal LiveShapingBlock(bool b) : base(b)
		{
		}

		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x0600757C RID: 30076 RVA: 0x00218E32 File Offset: 0x00217032
		private LiveShapingBlock ParentBlock
		{
			get
			{
				return base.Parent as LiveShapingBlock;
			}
		}

		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x0600757D RID: 30077 RVA: 0x00218E3F File Offset: 0x0021703F
		private LiveShapingBlock LeftChildBlock
		{
			get
			{
				return (LiveShapingBlock)base.LeftChild;
			}
		}

		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x0600757E RID: 30078 RVA: 0x00218E4C File Offset: 0x0021704C
		private LiveShapingBlock RightChildBlock
		{
			get
			{
				return (LiveShapingBlock)base.RightChild;
			}
		}

		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x0600757F RID: 30079 RVA: 0x00218E59 File Offset: 0x00217059
		internal LiveShapingList List
		{
			get
			{
				return ((LiveShapingTree)base.GetRoot(this)).List;
			}
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x00218E6C File Offset: 0x0021706C
		public override LiveShapingItem SetItemAt(int offset, LiveShapingItem lsi)
		{
			base.SetItemAt(offset, lsi);
			if (lsi != null)
			{
				lsi.Block = this;
			}
			return lsi;
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x00218E84 File Offset: 0x00217084
		protected override void Copy(RBNode<LiveShapingItem> sourceNode, int sourceOffset, RBNode<LiveShapingItem> destNode, int destOffset, int count)
		{
			base.Copy(sourceNode, sourceOffset, destNode, destOffset, count);
			if (sourceNode != destNode)
			{
				LiveShapingBlock block = (LiveShapingBlock)destNode;
				int i = 0;
				while (i < count)
				{
					destNode.GetItemAt(destOffset).Block = block;
					i++;
					destOffset++;
				}
			}
		}

		// Token: 0x06007582 RID: 30082 RVA: 0x00218ECC File Offset: 0x002170CC
		internal RBFinger<LiveShapingItem> GetFinger(LiveShapingItem lsi)
		{
			int num = base.OffsetOf(lsi);
			int num2;
			base.GetRootAndIndex(this, out num2);
			return new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = num,
				Index = num2 + num,
				Found = true
			};
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x00218F18 File Offset: 0x00217118
		internal void FindPosition(LiveShapingItem item, out RBFinger<LiveShapingItem> oldFinger, out RBFinger<LiveShapingItem> newFinger, Comparison<LiveShapingItem> comparison)
		{
			int size = base.Size;
			int num = -1;
			int num2 = -1;
			int i;
			for (i = 0; i < size; i++)
			{
				LiveShapingItem itemAt = base.GetItemAt(i);
				if (item == itemAt)
				{
					break;
				}
				if (!itemAt.IsSortDirty)
				{
					num2 = i;
					if (num < 0)
					{
						num = i;
					}
				}
			}
			int j;
			for (j = i + 1; j < size; j++)
			{
				LiveShapingItem itemAt = base.GetItemAt(j);
				if (!itemAt.IsSortDirty)
				{
					break;
				}
			}
			int num3 = j;
			for (int k = size - 1; k > num3; k--)
			{
				LiveShapingItem itemAt = base.GetItemAt(k);
				if (!itemAt.IsSortDirty)
				{
					num3 = k;
				}
			}
			int num4;
			base.GetRootAndIndex(this, out num4);
			oldFinger = new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = i,
				Index = num4 + i,
				Found = true
			};
			LiveShapingItem liveShapingItem = (num2 >= 0) ? base.GetItemAt(num2) : null;
			LiveShapingItem liveShapingItem2 = (j < size) ? base.GetItemAt(j) : null;
			int num5;
			int num6;
			if (liveShapingItem != null && (num5 = comparison(item, liveShapingItem)) < 0)
			{
				if (num != num2)
				{
					num5 = comparison(item, base.GetItemAt(num));
				}
				if (num5 >= 0)
				{
					newFinger = this.LocalSearch(item, num + 1, num2, comparison);
					return;
				}
				newFinger = this.SearchLeft(item, num, comparison);
				return;
			}
			else if (liveShapingItem2 != null && (num6 = comparison(item, liveShapingItem2)) > 0)
			{
				if (num3 != j)
				{
					num6 = comparison(item, base.GetItemAt(num3));
				}
				if (num6 <= 0)
				{
					newFinger = this.LocalSearch(item, j + 1, num3, comparison);
					return;
				}
				newFinger = this.SearchRight(item, num3 + 1, comparison);
				return;
			}
			else if (liveShapingItem != null)
			{
				if (liveShapingItem2 != null)
				{
					newFinger = oldFinger;
					return;
				}
				newFinger = this.SearchRight(item, i, comparison);
				return;
			}
			else
			{
				if (liveShapingItem2 != null)
				{
					newFinger = this.SearchLeft(item, i, comparison);
					return;
				}
				newFinger = this.SearchLeft(item, i, comparison);
				if (newFinger.Node == this)
				{
					newFinger = this.SearchRight(item, i, comparison);
				}
				return;
			}
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x00219124 File Offset: 0x00217324
		private RBFinger<LiveShapingItem> LocalSearch(LiveShapingItem item, int left, int right, Comparison<LiveShapingItem> comparison)
		{
			int num2;
			while (right - left > 3)
			{
				int num = (left + right) / 2;
				num2 = num;
				while (num2 >= left && base.GetItemAt(num2).IsSortDirty)
				{
					num2--;
				}
				if (num2 < left || comparison(base.GetItemAt(num2), item) <= 0)
				{
					left = num + 1;
				}
				else
				{
					right = num2;
				}
			}
			num2 = left;
			while (num2 < right && (base.GetItemAt(num2).IsSortDirty || comparison(item, base.GetItemAt(num2)) > 0))
			{
				num2++;
			}
			int num3;
			base.GetRootAndIndex(this, out num3);
			return new RBFinger<LiveShapingItem>
			{
				Node = this,
				Offset = num2,
				Index = num3 + num2
			};
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x002191D4 File Offset: 0x002173D4
		private RBFinger<LiveShapingItem> SearchLeft(LiveShapingItem item, int offset, Comparison<LiveShapingItem> comparison)
		{
			LiveShapingBlock node = this;
			List<LiveShapingBlock> list = new List<LiveShapingBlock>();
			list.Add(this.LeftChildBlock);
			LiveShapingBlock liveShapingBlock = this;
			for (LiveShapingBlock parentBlock = liveShapingBlock.ParentBlock; parentBlock != null; parentBlock = liveShapingBlock.ParentBlock)
			{
				if (parentBlock.RightChildBlock == liveShapingBlock)
				{
					int num;
					int num2;
					int num3;
					parentBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						list.Add(parentBlock.LeftChildBlock);
					}
					else
					{
						if (comparison(item, parentBlock.GetItemAt(num2)) > 0)
						{
							break;
						}
						if (comparison(item, parentBlock.GetItemAt(num)) >= 0)
						{
							return parentBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						list.Clear();
						list.Add(parentBlock.LeftChildBlock);
						node = parentBlock;
						offset = num;
					}
				}
				liveShapingBlock = parentBlock;
			}
			Stack<LiveShapingBlock> stack = new Stack<LiveShapingBlock>(list);
			while (stack.Count > 0)
			{
				liveShapingBlock = stack.Pop();
				if (liveShapingBlock != null)
				{
					int num;
					int num2;
					int num3;
					liveShapingBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						stack.Push(liveShapingBlock.LeftChildBlock);
						stack.Push(liveShapingBlock.RightChildBlock);
					}
					else if (comparison(item, liveShapingBlock.GetItemAt(num2)) > 0)
					{
						stack.Clear();
						stack.Push(liveShapingBlock.RightChildBlock);
					}
					else
					{
						if (comparison(item, liveShapingBlock.GetItemAt(num)) >= 0)
						{
							return liveShapingBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						node = liveShapingBlock;
						offset = num;
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
				}
			}
			int num4;
			base.GetRootAndIndex(node, out num4);
			return new RBFinger<LiveShapingItem>
			{
				Node = node,
				Offset = offset,
				Index = num4 + offset
			};
		}

		// Token: 0x06007586 RID: 30086 RVA: 0x00219364 File Offset: 0x00217564
		private RBFinger<LiveShapingItem> SearchRight(LiveShapingItem item, int offset, Comparison<LiveShapingItem> comparison)
		{
			LiveShapingBlock node = this;
			List<LiveShapingBlock> list = new List<LiveShapingBlock>();
			list.Add(this.RightChildBlock);
			LiveShapingBlock liveShapingBlock = this;
			for (LiveShapingBlock parentBlock = liveShapingBlock.ParentBlock; parentBlock != null; parentBlock = liveShapingBlock.ParentBlock)
			{
				if (parentBlock.LeftChildBlock == liveShapingBlock)
				{
					int num;
					int num2;
					int num3;
					parentBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						list.Add(parentBlock.RightChildBlock);
					}
					else
					{
						if (comparison(item, parentBlock.GetItemAt(num)) < 0)
						{
							break;
						}
						if (comparison(item, parentBlock.GetItemAt(num2)) <= 0)
						{
							return parentBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						list.Clear();
						list.Add(parentBlock.RightChildBlock);
						node = parentBlock;
						offset = num2 + 1;
					}
				}
				liveShapingBlock = parentBlock;
			}
			Stack<LiveShapingBlock> stack = new Stack<LiveShapingBlock>(list);
			while (stack.Count > 0)
			{
				liveShapingBlock = stack.Pop();
				if (liveShapingBlock != null)
				{
					int num;
					int num2;
					int num3;
					liveShapingBlock.GetFirstAndLastCleanItems(out num, out num2, out num3);
					if (num >= num3)
					{
						stack.Push(liveShapingBlock.RightChildBlock);
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
					else if (comparison(item, liveShapingBlock.GetItemAt(num)) < 0)
					{
						stack.Clear();
						stack.Push(liveShapingBlock.LeftChildBlock);
					}
					else
					{
						if (comparison(item, liveShapingBlock.GetItemAt(num2)) <= 0)
						{
							return liveShapingBlock.LocalSearch(item, num + 1, num2, comparison);
						}
						node = liveShapingBlock;
						offset = num2 + 1;
						stack.Push(liveShapingBlock.RightChildBlock);
					}
				}
			}
			int num4;
			base.GetRootAndIndex(node, out num4);
			return new RBFinger<LiveShapingItem>
			{
				Node = node,
				Offset = offset,
				Index = num4 + offset
			};
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x002194F8 File Offset: 0x002176F8
		private void GetFirstAndLastCleanItems(out int first, out int last, out int size)
		{
			size = base.Size;
			first = 0;
			while (first < size && base.GetItemAt(first).IsSortDirty)
			{
				first++;
			}
			last = size - 1;
			while (last > first && base.GetItemAt(last).IsSortDirty)
			{
				last--;
			}
		}
	}
}
