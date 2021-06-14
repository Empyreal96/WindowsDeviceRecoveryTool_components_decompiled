using System;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000423 RID: 1059
	internal class TextTreeRootNode : TextTreeNode
	{
		// Token: 0x06003D7B RID: 15739 RVA: 0x0011C41D File Offset: 0x0011A61D
		internal TextTreeRootNode(TextContainer tree)
		{
			this._tree = tree;
			this._symbolCount = 2;
			this._caretUnitBoundaryCacheOffset = -1;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x0011C43A File Offset: 0x0011A63A
		internal override TextTreeNode Clone()
		{
			Invariant.Assert(false, "Unexpected call to TextTreeRootNode.Clone!");
			return null;
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.None;
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D7F RID: 15743 RVA: 0x0011C448 File Offset: 0x0011A648
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ParentNode on TextContainer root!");
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06003D80 RID: 15744 RVA: 0x0011C455 File Offset: 0x0011A655
		// (set) Token: 0x06003D81 RID: 15745 RVA: 0x0011C45D File Offset: 0x0011A65D
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D83 RID: 15747 RVA: 0x0011C46B File Offset: 0x0011A66B
		internal override int LeftSymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root is never a sibling!");
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D85 RID: 15749 RVA: 0x0011C46B File Offset: 0x0011A66B
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root is never a sibling!");
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D87 RID: 15751 RVA: 0x0011C478 File Offset: 0x0011A678
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root never has sibling nodes!");
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D89 RID: 15753 RVA: 0x0011C478 File Offset: 0x0011A678
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root never has sibling nodes!");
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0011C485 File Offset: 0x0011A685
		// (set) Token: 0x06003D8B RID: 15755 RVA: 0x0011C48D File Offset: 0x0011A68D
		internal override uint Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0011C496 File Offset: 0x0011A696
		// (set) Token: 0x06003D8D RID: 15757 RVA: 0x0011C49E File Offset: 0x0011A69E
		internal uint PositionGeneration
		{
			get
			{
				return this._positionGeneration;
			}
			set
			{
				this._positionGeneration = value;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x0011C4A7 File Offset: 0x0011A6A7
		// (set) Token: 0x06003D8F RID: 15759 RVA: 0x0011C4AF File Offset: 0x0011A6AF
		internal uint LayoutGeneration
		{
			get
			{
				return this._layoutGeneration;
			}
			set
			{
				this._layoutGeneration = value;
				this._caretUnitBoundaryCacheOffset = -1;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D91 RID: 15761 RVA: 0x0011C4BF File Offset: 0x0011A6BF
		internal override int SymbolOffsetCache
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0, "Bad SymbolOffsetCache on TextContainer root!");
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x0011C4CF File Offset: 0x0011A6CF
		// (set) Token: 0x06003D93 RID: 15763 RVA: 0x0011C4D7 File Offset: 0x0011A6D7
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				Invariant.Assert(value >= 2, "Bad _symbolCount on TextContainer root!");
				this._symbolCount = value;
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x0011C4F1 File Offset: 0x0011A6F1
		// (set) Token: 0x06003D95 RID: 15765 RVA: 0x0011C4F9 File Offset: 0x0011A6F9
		internal override int IMECharCount
		{
			get
			{
				return this._imeCharCount;
			}
			set
			{
				Invariant.Assert(value >= 0, "IMECharCount may never be negative!");
				this._imeCharCount = value;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D97 RID: 15767 RVA: 0x0011C513 File Offset: 0x0011A713
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(!value, "Root node BeforeStart edge can never be referenced!");
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D99 RID: 15769 RVA: 0x00002137 File Offset: 0x00000337
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06003D9A RID: 15770 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D9B RID: 15771 RVA: 0x00002137 File Offset: 0x00000337
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06003D9C RID: 15772 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D9D RID: 15773 RVA: 0x0011C523 File Offset: 0x0011A723
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(!value, "Root node AfterEnd edge can never be referenced!");
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06003D9E RID: 15774 RVA: 0x0011C533 File Offset: 0x0011A733
		internal TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x0011C53B File Offset: 0x0011A73B
		// (set) Token: 0x06003DA0 RID: 15776 RVA: 0x0011C543 File Offset: 0x0011A743
		internal TextTreeRootTextBlock RootTextBlock
		{
			get
			{
				return this._rootTextBlock;
			}
			set
			{
				this._rootTextBlock = value;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0011C54C File Offset: 0x0011A74C
		// (set) Token: 0x06003DA2 RID: 15778 RVA: 0x0011C554 File Offset: 0x0011A754
		internal DispatcherProcessingDisabled DispatcherProcessingDisabled
		{
			get
			{
				return this._processingDisabled;
			}
			set
			{
				this._processingDisabled = value;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x0011C55D File Offset: 0x0011A75D
		// (set) Token: 0x06003DA4 RID: 15780 RVA: 0x0011C565 File Offset: 0x0011A765
		internal bool CaretUnitBoundaryCache
		{
			get
			{
				return this._caretUnitBoundaryCache;
			}
			set
			{
				this._caretUnitBoundaryCache = value;
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x0011C56E File Offset: 0x0011A76E
		// (set) Token: 0x06003DA6 RID: 15782 RVA: 0x0011C576 File Offset: 0x0011A776
		internal int CaretUnitBoundaryCacheOffset
		{
			get
			{
				return this._caretUnitBoundaryCacheOffset;
			}
			set
			{
				this._caretUnitBoundaryCacheOffset = value;
			}
		}

		// Token: 0x0400266A RID: 9834
		private readonly TextContainer _tree;

		// Token: 0x0400266B RID: 9835
		private TextTreeNode _containedNode;

		// Token: 0x0400266C RID: 9836
		private int _symbolCount;

		// Token: 0x0400266D RID: 9837
		private int _imeCharCount;

		// Token: 0x0400266E RID: 9838
		private uint _generation;

		// Token: 0x0400266F RID: 9839
		private uint _positionGeneration;

		// Token: 0x04002670 RID: 9840
		private uint _layoutGeneration;

		// Token: 0x04002671 RID: 9841
		private TextTreeRootTextBlock _rootTextBlock;

		// Token: 0x04002672 RID: 9842
		private bool _caretUnitBoundaryCache;

		// Token: 0x04002673 RID: 9843
		private int _caretUnitBoundaryCacheOffset;

		// Token: 0x04002674 RID: 9844
		private DispatcherProcessingDisabled _processingDisabled;
	}
}
