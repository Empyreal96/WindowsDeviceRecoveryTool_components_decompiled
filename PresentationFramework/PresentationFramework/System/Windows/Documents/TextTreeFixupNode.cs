using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200041D RID: 1053
	internal class TextTreeFixupNode : TextTreeNode
	{
		// Token: 0x06003D19 RID: 15641 RVA: 0x0011BE5B File Offset: 0x0011A05B
		internal TextTreeFixupNode(TextTreeNode previousNode, ElementEdge previousEdge, TextTreeNode nextNode, ElementEdge nextEdge) : this(previousNode, previousEdge, nextNode, nextEdge, null, null)
		{
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0011BE6A File Offset: 0x0011A06A
		internal TextTreeFixupNode(TextTreeNode previousNode, ElementEdge previousEdge, TextTreeNode nextNode, ElementEdge nextEdge, TextTreeNode firstContainedNode, TextTreeNode lastContainedNode)
		{
			this._previousNode = previousNode;
			this._previousEdge = previousEdge;
			this._nextNode = nextNode;
			this._nextEdge = nextEdge;
			this._firstContainedNode = firstContainedNode;
			this._lastContainedNode = lastContainedNode;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0011BE9F File Offset: 0x0011A09F
		internal override TextTreeNode Clone()
		{
			Invariant.Assert(false, "Unexpected call to TextTreeFixupNode.Clone!");
			return null;
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x0011BEAD File Offset: 0x0011A0AD
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			Invariant.Assert(false, "Unexpected call to TextTreeFixupNode.GetPointerContext!");
			return TextPointerContext.None;
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D1E RID: 15646 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06003D1F RID: 15647 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D20 RID: 15648 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D22 RID: 15650 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override int LeftSymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D24 RID: 15652 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D26 RID: 15654 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06003D27 RID: 15655 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D28 RID: 15656 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06003D29 RID: 15657 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D2A RID: 15658 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06003D2B RID: 15659 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		// (set) Token: 0x06003D2C RID: 15660 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override int SymbolOffsetCache
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D2E RID: 15662 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override int SymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D30 RID: 15664 RVA: 0x0011BEBB File Offset: 0x0011A0BB
		internal override int IMECharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06003D31 RID: 15665 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D32 RID: 15666 RVA: 0x0011BECB File Offset: 0x0011A0CB
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06003D33 RID: 15667 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D34 RID: 15668 RVA: 0x0011BECB File Offset: 0x0011A0CB
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06003D35 RID: 15669 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D36 RID: 15670 RVA: 0x0011BECB File Offset: 0x0011A0CB
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06003D37 RID: 15671 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D38 RID: 15672 RVA: 0x0011BECB File Offset: 0x0011A0CB
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x0011BED8 File Offset: 0x0011A0D8
		internal TextTreeNode PreviousNode
		{
			get
			{
				return this._previousNode;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06003D3A RID: 15674 RVA: 0x0011BEE0 File Offset: 0x0011A0E0
		internal ElementEdge PreviousEdge
		{
			get
			{
				return this._previousEdge;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06003D3B RID: 15675 RVA: 0x0011BEE8 File Offset: 0x0011A0E8
		internal TextTreeNode NextNode
		{
			get
			{
				return this._nextNode;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06003D3C RID: 15676 RVA: 0x0011BEF0 File Offset: 0x0011A0F0
		internal ElementEdge NextEdge
		{
			get
			{
				return this._nextEdge;
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06003D3D RID: 15677 RVA: 0x0011BEF8 File Offset: 0x0011A0F8
		internal TextTreeNode FirstContainedNode
		{
			get
			{
				return this._firstContainedNode;
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06003D3E RID: 15678 RVA: 0x0011BF00 File Offset: 0x0011A100
		internal TextTreeNode LastContainedNode
		{
			get
			{
				return this._lastContainedNode;
			}
		}

		// Token: 0x04002658 RID: 9816
		private readonly TextTreeNode _previousNode;

		// Token: 0x04002659 RID: 9817
		private readonly ElementEdge _previousEdge;

		// Token: 0x0400265A RID: 9818
		private readonly TextTreeNode _nextNode;

		// Token: 0x0400265B RID: 9819
		private readonly ElementEdge _nextEdge;

		// Token: 0x0400265C RID: 9820
		private readonly TextTreeNode _firstContainedNode;

		// Token: 0x0400265D RID: 9821
		private readonly TextTreeNode _lastContainedNode;
	}
}
