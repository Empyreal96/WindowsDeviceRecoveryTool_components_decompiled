using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000420 RID: 1056
	internal abstract class TextTreeNode : SplayTreeNode
	{
		// Token: 0x06003D43 RID: 15683
		internal abstract TextTreeNode Clone();

		// Token: 0x06003D44 RID: 15684 RVA: 0x0011C000 File Offset: 0x0011A200
		internal TextContainer GetTextTree()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					break;
				}
				splayTreeNode = containingNode;
			}
			return ((TextTreeRootNode)splayTreeNode).TextContainer;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x0011C028 File Offset: 0x0011A228
		internal DependencyObject GetDependencyParent()
		{
			SplayTreeNode splayTreeNode = this;
			TextTreeTextElementNode textTreeTextElementNode;
			for (;;)
			{
				textTreeTextElementNode = (splayTreeNode as TextTreeTextElementNode);
				if (textTreeTextElementNode != null)
				{
					break;
				}
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					goto Block_2;
				}
				splayTreeNode = containingNode;
			}
			DependencyObject dependencyObject = textTreeTextElementNode.TextElement;
			Invariant.Assert(dependencyObject != null, "TextElementNode has null TextElement!");
			return dependencyObject;
			Block_2:
			dependencyObject = ((TextTreeRootNode)splayTreeNode).TextContainer.Parent;
			return dependencyObject;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x0011C07C File Offset: 0x0011A27C
		internal DependencyObject GetLogicalTreeNode()
		{
			TextTreeObjectNode textTreeObjectNode = this as TextTreeObjectNode;
			if (textTreeObjectNode != null && textTreeObjectNode.EmbeddedElement is FrameworkElement)
			{
				return textTreeObjectNode.EmbeddedElement;
			}
			SplayTreeNode splayTreeNode = this;
			TextTreeTextElementNode textTreeTextElementNode;
			for (;;)
			{
				textTreeTextElementNode = (splayTreeNode as TextTreeTextElementNode);
				if (textTreeTextElementNode != null)
				{
					break;
				}
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					goto Block_4;
				}
				splayTreeNode = containingNode;
			}
			return textTreeTextElementNode.TextElement;
			Block_4:
			return ((TextTreeRootNode)splayTreeNode).TextContainer.Parent;
		}

		// Token: 0x06003D47 RID: 15687
		internal abstract TextPointerContext GetPointerContext(LogicalDirection direction);

		// Token: 0x06003D48 RID: 15688 RVA: 0x0011C0E1 File Offset: 0x0011A2E1
		internal TextTreeNode IncrementReferenceCount(ElementEdge edge)
		{
			return this.IncrementReferenceCount(edge, 1);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0011C0EB File Offset: 0x0011A2EB
		internal virtual TextTreeNode IncrementReferenceCount(ElementEdge edge, bool delta)
		{
			return this.IncrementReferenceCount(edge, delta ? 1 : 0);
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x0011C0FC File Offset: 0x0011A2FC
		internal virtual TextTreeNode IncrementReferenceCount(ElementEdge edge, int delta)
		{
			Invariant.Assert(delta >= 0);
			if (delta > 0)
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					this.BeforeStartReferenceCount = true;
					return this;
				case ElementEdge.AfterStart:
					this.AfterStartReferenceCount = true;
					return this;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					this.BeforeEndReferenceCount = true;
					return this;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						this.AfterEndReferenceCount = true;
						return this;
					}
					break;
				}
				Invariant.Assert(false, "Bad ElementEdge value!");
			}
			return this;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void DecrementReferenceCount(ElementEdge edge)
		{
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x0011C167 File Offset: 0x0011A367
		internal void InsertAtPosition(TextPointer position)
		{
			base.InsertAtNode(position.Node, position.Edge);
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x0011C17B File Offset: 0x0011A37B
		internal ElementEdge GetEdgeFromOffsetNoBias(int nodeOffset)
		{
			return this.GetEdgeFromOffset(nodeOffset, LogicalDirection.Forward);
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0011C188 File Offset: 0x0011A388
		internal ElementEdge GetEdgeFromOffset(int nodeOffset, LogicalDirection bias)
		{
			ElementEdge result;
			if (this.SymbolCount == 0)
			{
				result = ((bias == LogicalDirection.Forward) ? ElementEdge.AfterEnd : ElementEdge.BeforeStart);
			}
			else if (nodeOffset == 0)
			{
				result = ElementEdge.BeforeStart;
			}
			else if (nodeOffset == this.SymbolCount)
			{
				result = ElementEdge.AfterEnd;
			}
			else if (nodeOffset == 1)
			{
				result = ElementEdge.AfterStart;
			}
			else
			{
				Invariant.Assert(nodeOffset == this.SymbolCount - 1);
				result = ElementEdge.BeforeEnd;
			}
			return result;
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x0011C1D8 File Offset: 0x0011A3D8
		internal int GetOffsetFromEdge(ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return 0;
			case ElementEdge.AfterStart:
				return 1;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				return this.SymbolCount - 1;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					return this.SymbolCount;
				}
				break;
			}
			int result = 0;
			Invariant.Assert(false, "Bad ElementEdge value!");
			return result;
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06003D50 RID: 15696
		// (set) Token: 0x06003D51 RID: 15697
		internal abstract bool BeforeStartReferenceCount { get; set; }

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06003D52 RID: 15698
		// (set) Token: 0x06003D53 RID: 15699
		internal abstract bool AfterStartReferenceCount { get; set; }

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06003D54 RID: 15700
		// (set) Token: 0x06003D55 RID: 15701
		internal abstract bool BeforeEndReferenceCount { get; set; }

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06003D56 RID: 15702
		// (set) Token: 0x06003D57 RID: 15703
		internal abstract bool AfterEndReferenceCount { get; set; }
	}
}
