using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x0200034B RID: 843
	internal sealed class FixedDSBuilder
	{
		// Token: 0x06002D14 RID: 11540 RVA: 0x000CB723 File Offset: 0x000C9923
		public FixedDSBuilder(FixedPage fp, StoryFragments sf)
		{
			this._nameHashTable = new Dictionary<string, FixedDSBuilder.NameHashFixedNode>();
			this._fixedPage = fp;
			this._storyFragments = sf;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000CB744 File Offset: 0x000C9944
		public void BuildNameHashTable(string Name, UIElement e, int indexToFixedNodes)
		{
			if (!this._nameHashTable.ContainsKey(Name))
			{
				this._nameHashTable.Add(Name, new FixedDSBuilder.NameHashFixedNode(e, indexToFixedNodes));
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000CB767 File Offset: 0x000C9967
		public StoryFragments StoryFragments
		{
			get
			{
				return this._storyFragments;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000CB770 File Offset: 0x000C9970
		public void ConstructFlowNodes(FixedTextBuilder.FlowModelBuilder flowBuilder, List<FixedNode> fixedNodes)
		{
			this._fixedNodes = fixedNodes;
			this._visitedArray = new BitArray(fixedNodes.Count);
			this._flowBuilder = flowBuilder;
			List<StoryFragment> storyFragmentList = this.StoryFragments.StoryFragmentList;
			foreach (StoryFragment storyFragment in storyFragmentList)
			{
				List<BlockElement> blockElementList = storyFragment.BlockElementList;
				foreach (BlockElement be in blockElementList)
				{
					this._CreateFlowNodes(be);
				}
			}
			this._flowBuilder.AddStartNode(FixedElement.ElementType.Paragraph);
			for (int i = 0; i < this._visitedArray.Count; i++)
			{
				if (!this._visitedArray[i])
				{
					this.AddFixedNodeInFlow(i, null);
				}
			}
			this._flowBuilder.AddEndNode();
			this._flowBuilder.AddLeftoverHyperlinks();
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000CB880 File Offset: 0x000C9A80
		private void AddFixedNodeInFlow(int index, UIElement e)
		{
			if (this._visitedArray[index])
			{
				return;
			}
			FixedNode fixedNode = this._fixedNodes[index];
			if (e == null)
			{
				e = (this._fixedPage.GetElement(fixedNode) as UIElement);
			}
			this._visitedArray[index] = true;
			FixedSOMElement fixedSOMElement = FixedSOMElement.CreateFixedSOMElement(this._fixedPage, e, fixedNode, -1, -1);
			if (fixedSOMElement != null)
			{
				this._flowBuilder.AddElement(fixedSOMElement);
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000CB8EC File Offset: 0x000C9AEC
		private void _CreateFlowNodes(BlockElement be)
		{
			NamedElement namedElement = be as NamedElement;
			if (namedElement != null)
			{
				this.ConstructSomElement(namedElement);
				return;
			}
			SemanticBasicElement semanticBasicElement = be as SemanticBasicElement;
			if (semanticBasicElement != null)
			{
				this._flowBuilder.AddStartNode(be.ElementType);
				XmlLanguage value = (XmlLanguage)this._fixedPage.GetValue(FrameworkElement.LanguageProperty);
				this._flowBuilder.FixedElement.SetValue(FixedElement.LanguageProperty, value);
				this.SpecialProcessing(semanticBasicElement);
				List<BlockElement> blockElementList = semanticBasicElement.BlockElementList;
				foreach (BlockElement be2 in blockElementList)
				{
					this._CreateFlowNodes(be2);
				}
				this._flowBuilder.AddEndNode();
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000CB9B4 File Offset: 0x000C9BB4
		private void AddChildofFixedNodeinFlow(int[] childIndex, NamedElement ne)
		{
			FixedNode item = FixedNode.Create(this._fixedNodes[0].Page, childIndex);
			int num = this._fixedNodes.BinarySearch(item);
			if (num >= 0)
			{
				int num2 = num - 1;
				while (num2 >= 0 && this._fixedNodes[num2].ComparetoIndex(childIndex) == 0)
				{
					num2--;
				}
				int num3 = num2 + 1;
				while (num3 < this._fixedNodes.Count && this._fixedNodes[num3].ComparetoIndex(childIndex) == 0)
				{
					this.AddFixedNodeInFlow(num3, null);
					num3++;
				}
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000CBA50 File Offset: 0x000C9C50
		private void SpecialProcessing(SemanticBasicElement sbe)
		{
			ListItemStructure listItemStructure = sbe as ListItemStructure;
			FixedDSBuilder.NameHashFixedNode nameHashFixedNode;
			if (listItemStructure != null && listItemStructure.Marker != null && this._nameHashTable.TryGetValue(listItemStructure.Marker, out nameHashFixedNode))
			{
				this._visitedArray[nameHashFixedNode.index] = true;
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000CBA98 File Offset: 0x000C9C98
		private void ConstructSomElement(NamedElement ne)
		{
			FixedDSBuilder.NameHashFixedNode nameHashFixedNode;
			if (this._nameHashTable.TryGetValue(ne.NameReference, out nameHashFixedNode))
			{
				if (nameHashFixedNode.uiElement is Glyphs || nameHashFixedNode.uiElement is Path || nameHashFixedNode.uiElement is Image)
				{
					this.AddFixedNodeInFlow(nameHashFixedNode.index, nameHashFixedNode.uiElement);
					return;
				}
				if (nameHashFixedNode.uiElement is Canvas)
				{
					int[] childIndex = this._fixedPage._CreateChildIndex(nameHashFixedNode.uiElement);
					this.AddChildofFixedNodeinFlow(childIndex, ne);
				}
			}
		}

		// Token: 0x04001D68 RID: 7528
		private StoryFragments _storyFragments;

		// Token: 0x04001D69 RID: 7529
		private FixedPage _fixedPage;

		// Token: 0x04001D6A RID: 7530
		private List<FixedNode> _fixedNodes;

		// Token: 0x04001D6B RID: 7531
		private BitArray _visitedArray;

		// Token: 0x04001D6C RID: 7532
		private Dictionary<string, FixedDSBuilder.NameHashFixedNode> _nameHashTable;

		// Token: 0x04001D6D RID: 7533
		private FixedTextBuilder.FlowModelBuilder _flowBuilder;

		// Token: 0x020008D0 RID: 2256
		private class NameHashFixedNode
		{
			// Token: 0x0600848E RID: 33934 RVA: 0x0024892D File Offset: 0x00246B2D
			internal NameHashFixedNode(UIElement e, int i)
			{
				this.uiElement = e;
				this.index = i;
			}

			// Token: 0x04004240 RID: 16960
			internal UIElement uiElement;

			// Token: 0x04004241 RID: 16961
			internal int index;
		}
	}
}
