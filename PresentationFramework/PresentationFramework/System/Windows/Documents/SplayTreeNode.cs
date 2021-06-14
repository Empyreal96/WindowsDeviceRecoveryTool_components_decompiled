using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003DF RID: 991
	internal abstract class SplayTreeNode
	{
		// Token: 0x060035BB RID: 13755 RVA: 0x000F4320 File Offset: 0x000F2520
		internal SplayTreeNode GetSiblingAtOffset(int offset, out int nodeOffset)
		{
			SplayTreeNode splayTreeNode = this;
			nodeOffset = 0;
			int leftSymbolCount;
			for (;;)
			{
				leftSymbolCount = splayTreeNode.LeftSymbolCount;
				if (offset < nodeOffset + leftSymbolCount)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else
				{
					int symbolCount = splayTreeNode.SymbolCount;
					if (offset <= nodeOffset + leftSymbolCount + symbolCount)
					{
						break;
					}
					nodeOffset += leftSymbolCount + symbolCount;
					splayTreeNode = splayTreeNode.RightChildNode;
				}
			}
			nodeOffset += leftSymbolCount;
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000F437C File Offset: 0x000F257C
		internal SplayTreeNode GetSiblingAtCharOffset(int charOffset, out int nodeCharOffset)
		{
			SplayTreeNode splayTreeNode = this;
			nodeCharOffset = 0;
			int leftCharCount;
			for (;;)
			{
				leftCharCount = splayTreeNode.LeftCharCount;
				if (charOffset < nodeCharOffset + leftCharCount)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else if (charOffset == nodeCharOffset + leftCharCount && charOffset > 0)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else
				{
					int imecharCount = splayTreeNode.IMECharCount;
					if (imecharCount > 0 && charOffset <= nodeCharOffset + leftCharCount + imecharCount)
					{
						break;
					}
					nodeCharOffset += leftCharCount + imecharCount;
					splayTreeNode = splayTreeNode.RightChildNode;
				}
			}
			nodeCharOffset += leftCharCount;
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000F43F0 File Offset: 0x000F25F0
		internal SplayTreeNode GetFirstContainedNode()
		{
			SplayTreeNode containedNode = this.ContainedNode;
			if (containedNode == null)
			{
				return null;
			}
			return containedNode.GetMinSibling();
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000F4410 File Offset: 0x000F2610
		internal SplayTreeNode GetLastContainedNode()
		{
			SplayTreeNode containedNode = this.ContainedNode;
			if (containedNode == null)
			{
				return null;
			}
			return containedNode.GetMaxSibling();
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000F442F File Offset: 0x000F262F
		internal SplayTreeNode GetContainingNode()
		{
			this.Splay();
			return this.ParentNode;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000F4440 File Offset: 0x000F2640
		internal SplayTreeNode GetPreviousNode()
		{
			SplayTreeNode splayTreeNode = this.LeftChildNode;
			if (splayTreeNode != null)
			{
				for (;;)
				{
					SplayTreeNode rightChildNode = splayTreeNode.RightChildNode;
					if (rightChildNode == null)
					{
						break;
					}
					splayTreeNode = rightChildNode;
				}
			}
			else
			{
				SplayTreeNodeRole role = this.Role;
				splayTreeNode = this.ParentNode;
				while (role != SplayTreeNodeRole.LocalRoot)
				{
					if (role == SplayTreeNodeRole.RightChild)
					{
						goto IL_41;
					}
					role = splayTreeNode.Role;
					splayTreeNode = splayTreeNode.ParentNode;
				}
				splayTreeNode = null;
			}
			IL_41:
			if (splayTreeNode != null)
			{
				splayTreeNode.Splay();
			}
			return splayTreeNode;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000F4498 File Offset: 0x000F2698
		internal SplayTreeNode GetNextNode()
		{
			SplayTreeNode splayTreeNode = this.RightChildNode;
			if (splayTreeNode != null)
			{
				for (;;)
				{
					SplayTreeNode leftChildNode = splayTreeNode.LeftChildNode;
					if (leftChildNode == null)
					{
						break;
					}
					splayTreeNode = leftChildNode;
				}
			}
			else
			{
				SplayTreeNodeRole role = this.Role;
				splayTreeNode = this.ParentNode;
				while (role != SplayTreeNodeRole.LocalRoot)
				{
					if (role == SplayTreeNodeRole.LeftChild)
					{
						goto IL_41;
					}
					role = splayTreeNode.Role;
					splayTreeNode = splayTreeNode.ParentNode;
				}
				splayTreeNode = null;
			}
			IL_41:
			if (splayTreeNode != null)
			{
				splayTreeNode.Splay();
			}
			return splayTreeNode;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000F44F0 File Offset: 0x000F26F0
		internal int GetSymbolOffset(uint treeGeneration)
		{
			int num = 0;
			SplayTreeNode splayTreeNode = this;
			while (splayTreeNode.Generation != treeGeneration || splayTreeNode.SymbolOffsetCache < 0)
			{
				splayTreeNode.Splay();
				num += splayTreeNode.LeftSymbolCount;
				num++;
				splayTreeNode = splayTreeNode.ParentNode;
			}
			num += splayTreeNode.SymbolOffsetCache;
			this.Generation = treeGeneration;
			this.SymbolOffsetCache = num;
			return num;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000F454C File Offset: 0x000F274C
		internal int GetIMECharOffset()
		{
			int num = 0;
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				splayTreeNode.Splay();
				num += splayTreeNode.LeftCharCount;
				splayTreeNode = splayTreeNode.ParentNode;
				if (splayTreeNode == null)
				{
					break;
				}
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					num += textTreeTextElementNode.IMELeftEdgeCharCount;
				}
			}
			return num;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000F458C File Offset: 0x000F278C
		internal void InsertAtNode(SplayTreeNode positionNode, ElementEdge edge)
		{
			if (edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd)
			{
				this.InsertAtNode(positionNode, edge == ElementEdge.BeforeStart);
				return;
			}
			SplayTreeNode splayTreeNode;
			bool insertBefore;
			if (edge == ElementEdge.AfterStart)
			{
				splayTreeNode = positionNode.GetFirstContainedNode();
				insertBefore = true;
			}
			else
			{
				splayTreeNode = positionNode.GetLastContainedNode();
				insertBefore = false;
			}
			if (splayTreeNode == null)
			{
				positionNode.ContainedNode = this;
				this.ParentNode = positionNode;
				Invariant.Assert(this.LeftChildNode == null);
				Invariant.Assert(this.RightChildNode == null);
				Invariant.Assert(this.LeftSymbolCount == 0);
				return;
			}
			this.InsertAtNode(splayTreeNode, insertBefore);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000F460C File Offset: 0x000F280C
		internal void InsertAtNode(SplayTreeNode location, bool insertBefore)
		{
			Invariant.Assert(this.ParentNode == null, "Can't insert child node!");
			Invariant.Assert(this.LeftChildNode == null, "Can't insert node with left children!");
			Invariant.Assert(this.RightChildNode == null, "Can't insert node with right children!");
			SplayTreeNode splayTreeNode = insertBefore ? location.GetPreviousNode() : location;
			SplayTreeNode rightSubTree;
			SplayTreeNode parentNode;
			if (splayTreeNode != null)
			{
				rightSubTree = splayTreeNode.Split();
				parentNode = splayTreeNode.ParentNode;
			}
			else
			{
				rightSubTree = location;
				location.Splay();
				Invariant.Assert(location.Role == SplayTreeNodeRole.LocalRoot, "location should be local root!");
				parentNode = location.ParentNode;
			}
			SplayTreeNode.Join(this, splayTreeNode, rightSubTree);
			this.ParentNode = parentNode;
			if (parentNode != null)
			{
				parentNode.ContainedNode = this;
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000F46B0 File Offset: 0x000F28B0
		internal void Remove()
		{
			this.Splay();
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot);
			SplayTreeNode parentNode = this.ParentNode;
			SplayTreeNode leftChildNode = this.LeftChildNode;
			SplayTreeNode rightChildNode = this.RightChildNode;
			if (leftChildNode != null)
			{
				leftChildNode.ParentNode = null;
			}
			if (rightChildNode != null)
			{
				rightChildNode.ParentNode = null;
			}
			SplayTreeNode splayTreeNode = SplayTreeNode.Join(leftChildNode, rightChildNode);
			if (parentNode != null)
			{
				parentNode.ContainedNode = splayTreeNode;
			}
			if (splayTreeNode != null)
			{
				splayTreeNode.ParentNode = parentNode;
			}
			this.ParentNode = null;
			this.LeftChildNode = null;
			this.RightChildNode = null;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000F472C File Offset: 0x000F292C
		internal static void Join(SplayTreeNode root, SplayTreeNode leftSubTree, SplayTreeNode rightSubTree)
		{
			root.LeftChildNode = leftSubTree;
			root.RightChildNode = rightSubTree;
			Invariant.Assert(root.Role == SplayTreeNodeRole.LocalRoot);
			if (leftSubTree != null)
			{
				leftSubTree.ParentNode = root;
				root.LeftSymbolCount = leftSubTree.LeftSymbolCount + leftSubTree.SymbolCount;
				root.LeftCharCount = leftSubTree.LeftCharCount + leftSubTree.IMECharCount;
			}
			else
			{
				root.LeftSymbolCount = 0;
				root.LeftCharCount = 0;
			}
			if (rightSubTree != null)
			{
				rightSubTree.ParentNode = root;
			}
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000F47A0 File Offset: 0x000F29A0
		internal static SplayTreeNode Join(SplayTreeNode leftSubTree, SplayTreeNode rightSubTree)
		{
			Invariant.Assert(leftSubTree == null || leftSubTree.ParentNode == null);
			Invariant.Assert(rightSubTree == null || rightSubTree.ParentNode == null);
			SplayTreeNode splayTreeNode;
			if (leftSubTree != null)
			{
				splayTreeNode = leftSubTree.GetMaxSibling();
				splayTreeNode.Splay();
				Invariant.Assert(splayTreeNode.Role == SplayTreeNodeRole.LocalRoot);
				Invariant.Assert(splayTreeNode.RightChildNode == null);
				splayTreeNode.RightChildNode = rightSubTree;
				if (rightSubTree != null)
				{
					rightSubTree.ParentNode = splayTreeNode;
				}
			}
			else if (rightSubTree != null)
			{
				splayTreeNode = rightSubTree;
				Invariant.Assert(splayTreeNode.Role == SplayTreeNodeRole.LocalRoot);
			}
			else
			{
				splayTreeNode = null;
			}
			return splayTreeNode;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000F482C File Offset: 0x000F2A2C
		internal SplayTreeNode Split()
		{
			this.Splay();
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot, "location should be local root!");
			SplayTreeNode rightChildNode = this.RightChildNode;
			if (rightChildNode != null)
			{
				rightChildNode.ParentNode = null;
				this.RightChildNode = null;
			}
			return rightChildNode;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000F486C File Offset: 0x000F2A6C
		internal SplayTreeNode GetMinSibling()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode leftChildNode = splayTreeNode.LeftChildNode;
				if (leftChildNode == null)
				{
					break;
				}
				splayTreeNode = leftChildNode;
			}
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000F4890 File Offset: 0x000F2A90
		internal SplayTreeNode GetMaxSibling()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode rightChildNode = splayTreeNode.RightChildNode;
				if (rightChildNode == null)
				{
					break;
				}
				splayTreeNode = rightChildNode;
			}
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000F48B4 File Offset: 0x000F2AB4
		internal void Splay()
		{
			SplayTreeNodeRole role;
			SplayTreeNode parentNode;
			for (;;)
			{
				role = this.Role;
				if (role == SplayTreeNodeRole.LocalRoot)
				{
					goto IL_7F;
				}
				parentNode = this.ParentNode;
				SplayTreeNodeRole role2 = parentNode.Role;
				if (role2 == SplayTreeNodeRole.LocalRoot)
				{
					break;
				}
				SplayTreeNode parentNode2 = parentNode.ParentNode;
				if (role == role2)
				{
					if (role == SplayTreeNodeRole.LeftChild)
					{
						parentNode2.RotateRight();
						parentNode.RotateRight();
					}
					else
					{
						parentNode2.RotateLeft();
						parentNode.RotateLeft();
					}
				}
				else if (role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.RotateRight();
					parentNode2.RotateLeft();
				}
				else
				{
					parentNode.RotateLeft();
					parentNode2.RotateRight();
				}
			}
			if (role == SplayTreeNodeRole.LeftChild)
			{
				parentNode.RotateRight();
			}
			else
			{
				parentNode.RotateLeft();
			}
			IL_7F:
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot, "Splay didn't move node to root!");
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000F4953 File Offset: 0x000F2B53
		internal bool IsChildOfNode(SplayTreeNode parentNode)
		{
			return parentNode.LeftChildNode == this || parentNode.RightChildNode == this || parentNode.ContainedNode == this;
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x060035CE RID: 13774
		// (set) Token: 0x060035CF RID: 13775
		internal abstract SplayTreeNode ParentNode { get; set; }

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x060035D0 RID: 13776
		// (set) Token: 0x060035D1 RID: 13777
		internal abstract SplayTreeNode ContainedNode { get; set; }

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x060035D2 RID: 13778
		// (set) Token: 0x060035D3 RID: 13779
		internal abstract SplayTreeNode LeftChildNode { get; set; }

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x060035D4 RID: 13780
		// (set) Token: 0x060035D5 RID: 13781
		internal abstract SplayTreeNode RightChildNode { get; set; }

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x060035D6 RID: 13782
		// (set) Token: 0x060035D7 RID: 13783
		internal abstract int SymbolCount { get; set; }

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x060035D8 RID: 13784
		// (set) Token: 0x060035D9 RID: 13785
		internal abstract int IMECharCount { get; set; }

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x060035DA RID: 13786
		// (set) Token: 0x060035DB RID: 13787
		internal abstract int LeftSymbolCount { get; set; }

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x060035DC RID: 13788
		// (set) Token: 0x060035DD RID: 13789
		internal abstract int LeftCharCount { get; set; }

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x060035DE RID: 13790
		// (set) Token: 0x060035DF RID: 13791
		internal abstract uint Generation { get; set; }

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x060035E0 RID: 13792
		// (set) Token: 0x060035E1 RID: 13793
		internal abstract int SymbolOffsetCache { get; set; }

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x060035E2 RID: 13794 RVA: 0x000F4974 File Offset: 0x000F2B74
		internal SplayTreeNodeRole Role
		{
			get
			{
				SplayTreeNode parentNode = this.ParentNode;
				SplayTreeNodeRole result;
				if (parentNode == null || parentNode.ContainedNode == this)
				{
					result = SplayTreeNodeRole.LocalRoot;
				}
				else if (parentNode.LeftChildNode == this)
				{
					result = SplayTreeNodeRole.LeftChild;
				}
				else
				{
					Invariant.Assert(parentNode.RightChildNode == this, "Node has no relation to parent!");
					result = SplayTreeNodeRole.RightChild;
				}
				return result;
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000F49BC File Offset: 0x000F2BBC
		private void RotateLeft()
		{
			Invariant.Assert(this.RightChildNode != null, "Can't rotate left with null right child!");
			SplayTreeNode rightChildNode = this.RightChildNode;
			this.RightChildNode = rightChildNode.LeftChildNode;
			if (rightChildNode.LeftChildNode != null)
			{
				rightChildNode.LeftChildNode.ParentNode = this;
			}
			SplayTreeNode parentNode = this.ParentNode;
			rightChildNode.ParentNode = parentNode;
			if (parentNode != null)
			{
				if (parentNode.ContainedNode == this)
				{
					parentNode.ContainedNode = rightChildNode;
				}
				else if (this.Role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.LeftChildNode = rightChildNode;
				}
				else
				{
					parentNode.RightChildNode = rightChildNode;
				}
			}
			rightChildNode.LeftChildNode = this;
			this.ParentNode = rightChildNode;
			SplayTreeNode rightChildNode2 = this.RightChildNode;
			rightChildNode.LeftSymbolCount += this.LeftSymbolCount + this.SymbolCount;
			rightChildNode.LeftCharCount += this.LeftCharCount + this.IMECharCount;
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000F4A88 File Offset: 0x000F2C88
		private void RotateRight()
		{
			Invariant.Assert(this.LeftChildNode != null, "Can't rotate right with null left child!");
			SplayTreeNode leftChildNode = this.LeftChildNode;
			this.LeftChildNode = leftChildNode.RightChildNode;
			if (leftChildNode.RightChildNode != null)
			{
				leftChildNode.RightChildNode.ParentNode = this;
			}
			SplayTreeNode parentNode = this.ParentNode;
			leftChildNode.ParentNode = parentNode;
			if (parentNode != null)
			{
				if (parentNode.ContainedNode == this)
				{
					parentNode.ContainedNode = leftChildNode;
				}
				else if (this.Role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.LeftChildNode = leftChildNode;
				}
				else
				{
					parentNode.RightChildNode = leftChildNode;
				}
			}
			leftChildNode.RightChildNode = this;
			this.ParentNode = leftChildNode;
			SplayTreeNode leftChildNode2 = this.LeftChildNode;
			this.LeftSymbolCount -= leftChildNode.LeftSymbolCount + leftChildNode.SymbolCount;
			this.LeftCharCount -= leftChildNode.LeftCharCount + leftChildNode.IMECharCount;
		}
	}
}
