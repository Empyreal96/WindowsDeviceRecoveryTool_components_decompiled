using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000428 RID: 1064
	internal class TextTreeTextNode : TextTreeNode
	{
		// Token: 0x06003E07 RID: 15879 RVA: 0x0011D208 File Offset: 0x0011B408
		internal TextTreeTextNode()
		{
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0011D218 File Offset: 0x0011B418
		internal override TextTreeNode Clone()
		{
			TextTreeTextNode textTreeTextNode = null;
			if (this._symbolCount > 0)
			{
				textTreeTextNode = new TextTreeTextNode();
				textTreeTextNode._symbolCount = this._symbolCount;
			}
			return textTreeTextNode;
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00016748 File Offset: 0x00014948
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.Text;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0011D244 File Offset: 0x0011B444
		internal override TextTreeNode IncrementReferenceCount(ElementEdge edge, int delta)
		{
			Invariant.Assert(delta >= 0);
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Bad edge ref to TextTreeTextNode!");
			if (delta == 0)
			{
				return this;
			}
			TextTreeTextNode textTreeTextNode;
			if (this._positionRefCount > 0 && edge != this._referencedEdge)
			{
				textTreeTextNode = this.Split((edge == ElementEdge.BeforeStart) ? 0 : this._symbolCount, edge);
				textTreeTextNode._referencedEdge = edge;
				textTreeTextNode._positionRefCount += delta;
				TextTreeTextNode textTreeTextNode2;
				if (edge == ElementEdge.BeforeStart)
				{
					textTreeTextNode2 = (textTreeTextNode.GetPreviousNode() as TextTreeTextNode);
				}
				else
				{
					textTreeTextNode2 = (textTreeTextNode.GetNextNode() as TextTreeTextNode);
				}
				if (textTreeTextNode2 != null && textTreeTextNode2._positionRefCount == 0)
				{
					textTreeTextNode2.Merge();
				}
			}
			else
			{
				textTreeTextNode = this;
				this._referencedEdge = edge;
				this._positionRefCount += delta;
			}
			return textTreeTextNode;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0011D2FC File Offset: 0x0011B4FC
		internal override void DecrementReferenceCount(ElementEdge edge)
		{
			Invariant.Assert(edge == this._referencedEdge, "Bad edge decrement!");
			this._positionRefCount--;
			Invariant.Assert(this._positionRefCount >= 0, "Bogus PositionRefCount! ");
			if (this._positionRefCount == 0)
			{
				this.Merge();
			}
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0011D350 File Offset: 0x0011B550
		internal TextTreeTextNode Split(int localOffset, ElementEdge edge)
		{
			Invariant.Assert(this._symbolCount > 0, "Splitting a zero-width TextNode!");
			Invariant.Assert(localOffset >= 0 && localOffset <= this._symbolCount, "Bad localOffset!");
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Bad edge parameter!");
			TextTreeTextNode textTreeTextNode = new TextTreeTextNode();
			textTreeTextNode._generation = this._generation;
			base.Splay();
			ElementEdge edge2;
			TextTreeTextNode result;
			if (this._positionRefCount > 0 && this._referencedEdge == ElementEdge.BeforeStart)
			{
				textTreeTextNode._symbolOffsetCache = ((this._symbolOffsetCache == -1) ? -1 : (this._symbolOffsetCache + localOffset));
				textTreeTextNode._symbolCount = this._symbolCount - localOffset;
				this._symbolCount = localOffset;
				edge2 = ElementEdge.AfterEnd;
				result = ((edge == ElementEdge.BeforeStart) ? this : textTreeTextNode);
			}
			else
			{
				textTreeTextNode._symbolOffsetCache = this._symbolOffsetCache;
				textTreeTextNode._symbolCount = localOffset;
				this._symbolOffsetCache = ((this._symbolOffsetCache == -1) ? -1 : (this._symbolOffsetCache + localOffset));
				this._symbolCount -= localOffset;
				edge2 = ElementEdge.BeforeStart;
				result = ((edge == ElementEdge.BeforeStart) ? textTreeTextNode : this);
			}
			Invariant.Assert(this._symbolCount >= 0);
			Invariant.Assert(textTreeTextNode._symbolCount >= 0);
			textTreeTextNode.InsertAtNode(this, edge2);
			return result;
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x0011D479 File Offset: 0x0011B679
		// (set) Token: 0x06003E0E RID: 15886 RVA: 0x0011D481 File Offset: 0x0011B681
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06003E0F RID: 15887 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003E10 RID: 15888 RVA: 0x0011D48F File Offset: 0x0011B68F
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set child on a TextTreeTextNode!");
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x0011D49C File Offset: 0x0011B69C
		// (set) Token: 0x06003E12 RID: 15890 RVA: 0x0011D4A4 File Offset: 0x0011B6A4
		internal override int LeftSymbolCount
		{
			get
			{
				return this._leftSymbolCount;
			}
			set
			{
				this._leftSymbolCount = value;
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x0011D4AD File Offset: 0x0011B6AD
		// (set) Token: 0x06003E14 RID: 15892 RVA: 0x0011D4B5 File Offset: 0x0011B6B5
		internal override int LeftCharCount
		{
			get
			{
				return this._leftCharCount;
			}
			set
			{
				this._leftCharCount = value;
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x0011D4BE File Offset: 0x0011B6BE
		// (set) Token: 0x06003E16 RID: 15894 RVA: 0x0011D4C6 File Offset: 0x0011B6C6
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x0011D4D4 File Offset: 0x0011B6D4
		// (set) Token: 0x06003E18 RID: 15896 RVA: 0x0011D4DC File Offset: 0x0011B6DC
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x0011D4EA File Offset: 0x0011B6EA
		// (set) Token: 0x06003E1A RID: 15898 RVA: 0x0011D4F2 File Offset: 0x0011B6F2
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

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06003E1B RID: 15899 RVA: 0x0011D4FB File Offset: 0x0011B6FB
		// (set) Token: 0x06003E1C RID: 15900 RVA: 0x0011D503 File Offset: 0x0011B703
		internal override int SymbolOffsetCache
		{
			get
			{
				return this._symbolOffsetCache;
			}
			set
			{
				this._symbolOffsetCache = value;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x0011D50C File Offset: 0x0011B70C
		// (set) Token: 0x06003E1E RID: 15902 RVA: 0x0011D514 File Offset: 0x0011B714
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				this._symbolCount = value;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x0011D51D File Offset: 0x0011B71D
		// (set) Token: 0x06003E20 RID: 15904 RVA: 0x00002137 File Offset: 0x00000337
		internal override int IMECharCount
		{
			get
			{
				return this.SymbolCount;
			}
			set
			{
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06003E21 RID: 15905 RVA: 0x0011D525 File Offset: 0x0011B725
		// (set) Token: 0x06003E22 RID: 15906 RVA: 0x0011D53B File Offset: 0x0011B73B
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return this._referencedEdge == ElementEdge.BeforeStart && this._positionRefCount > 0;
			}
			set
			{
				Invariant.Assert(false, "Can't set TextTreeTextNode ref counts directly!");
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06003E23 RID: 15907 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003E24 RID: 15908 RVA: 0x0011D548 File Offset: 0x0011B748
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Text nodes don't have an AfterStart edge!");
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06003E25 RID: 15909 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003E26 RID: 15910 RVA: 0x0011D555 File Offset: 0x0011B755
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Text nodes don't have a BeforeEnd edge!");
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x0011D562 File Offset: 0x0011B762
		// (set) Token: 0x06003E28 RID: 15912 RVA: 0x0011D53B File Offset: 0x0011B73B
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return this._referencedEdge == ElementEdge.AfterEnd && this._positionRefCount > 0;
			}
			set
			{
				Invariant.Assert(false, "Can't set TextTreeTextNode ref counts directly!");
			}
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0011D578 File Offset: 0x0011B778
		private void Merge()
		{
			Invariant.Assert(this._positionRefCount == 0, "Inappropriate Merge call!");
			TextTreeTextNode textTreeTextNode = base.GetPreviousNode() as TextTreeTextNode;
			if (textTreeTextNode != null && (textTreeTextNode._positionRefCount == 0 || textTreeTextNode._referencedEdge == ElementEdge.BeforeStart))
			{
				base.Remove();
				this._parentNode = null;
				textTreeTextNode.Splay();
				textTreeTextNode._symbolCount += this._symbolCount;
			}
			else
			{
				textTreeTextNode = this;
			}
			TextTreeTextNode textTreeTextNode2 = textTreeTextNode.GetNextNode() as TextTreeTextNode;
			if (textTreeTextNode2 != null)
			{
				if (textTreeTextNode._positionRefCount == 0 && (textTreeTextNode2._positionRefCount == 0 || textTreeTextNode2._referencedEdge == ElementEdge.AfterEnd))
				{
					textTreeTextNode.Remove();
					textTreeTextNode._parentNode = null;
					textTreeTextNode2.Splay();
					if (textTreeTextNode2._symbolOffsetCache != -1)
					{
						textTreeTextNode2._symbolOffsetCache -= textTreeTextNode._symbolCount;
					}
					textTreeTextNode2._symbolCount += textTreeTextNode._symbolCount;
					return;
				}
				if ((textTreeTextNode._positionRefCount == 0 || textTreeTextNode._referencedEdge == ElementEdge.BeforeStart) && textTreeTextNode2._positionRefCount == 0)
				{
					textTreeTextNode2.Remove();
					textTreeTextNode2._parentNode = null;
					textTreeTextNode.Splay();
					textTreeTextNode._symbolCount += textTreeTextNode2._symbolCount;
				}
			}
		}

		// Token: 0x0400268A RID: 9866
		private int _leftSymbolCount;

		// Token: 0x0400268B RID: 9867
		private int _leftCharCount;

		// Token: 0x0400268C RID: 9868
		private TextTreeNode _parentNode;

		// Token: 0x0400268D RID: 9869
		private TextTreeNode _leftChildNode;

		// Token: 0x0400268E RID: 9870
		private TextTreeNode _rightChildNode;

		// Token: 0x0400268F RID: 9871
		private uint _generation;

		// Token: 0x04002690 RID: 9872
		private int _symbolOffsetCache;

		// Token: 0x04002691 RID: 9873
		private int _symbolCount;

		// Token: 0x04002692 RID: 9874
		private int _positionRefCount;

		// Token: 0x04002693 RID: 9875
		private ElementEdge _referencedEdge;
	}
}
