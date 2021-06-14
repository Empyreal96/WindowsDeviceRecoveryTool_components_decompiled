using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x02000363 RID: 867
	internal abstract class FixedSOMContainer : FixedSOMSemanticBox, IComparable
	{
		// Token: 0x06002E16 RID: 11798 RVA: 0x000D033C File Offset: 0x000CE53C
		protected FixedSOMContainer()
		{
			this._semanticBoxes = new List<FixedSOMSemanticBox>();
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000D0350 File Offset: 0x000CE550
		int IComparable.CompareTo(object comparedObj)
		{
			int num = int.MinValue;
			FixedSOMPageElement fixedSOMPageElement = comparedObj as FixedSOMPageElement;
			FixedSOMPageElement fixedSOMPageElement2 = this as FixedSOMPageElement;
			if (fixedSOMPageElement == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					comparedObj.GetType(),
					typeof(FixedSOMContainer)
				}), "comparedObj");
			}
			FixedSOMSemanticBox.SpatialComparison comparison = base._CompareHorizontal(fixedSOMPageElement, false);
			FixedSOMSemanticBox.SpatialComparison spatialComparison = base._CompareVertical(fixedSOMPageElement);
			switch (comparison)
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				if (spatialComparison != FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = -1;
				}
				else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = 1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				switch (spatialComparison)
				{
				case FixedSOMSemanticBox.SpatialComparison.Before:
				case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
					num = -1;
					break;
				case FixedSOMSemanticBox.SpatialComparison.Equal:
					num = 0;
					break;
				case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				case FixedSOMSemanticBox.SpatialComparison.After:
					num = 1;
					break;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.After)
				{
					num = 1;
				}
				else if (spatialComparison == FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				if (spatialComparison != FixedSOMSemanticBox.SpatialComparison.Before)
				{
					num = 1;
				}
				break;
			}
			if (num == -2147483648)
			{
				if (fixedSOMPageElement2.FixedNodes.Count == 0 || fixedSOMPageElement.FixedNodes.Count == 0)
				{
					num = 0;
				}
				else
				{
					FixedNode item = fixedSOMPageElement2.FixedNodes[0];
					FixedNode item2 = fixedSOMPageElement2.FixedNodes[fixedSOMPageElement2.FixedNodes.Count - 1];
					FixedNode item3 = fixedSOMPageElement.FixedNodes[0];
					FixedNode item4 = fixedSOMPageElement.FixedNodes[fixedSOMPageElement.FixedNodes.Count - 1];
					if (fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item3) - fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item2) == 1)
					{
						num = -1;
					}
					else if (fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item4) - fixedSOMPageElement2.FixedSOMPage.MarkupOrder.IndexOf(item) == 1)
					{
						num = 1;
					}
					else
					{
						int num2 = base._SpatialToAbsoluteComparison(spatialComparison);
						num = ((num2 != 0) ? num2 : base._SpatialToAbsoluteComparison(comparison));
					}
				}
			}
			return num;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000D0524 File Offset: 0x000CE724
		protected void AddSorted(FixedSOMSemanticBox box)
		{
			int num = this._semanticBoxes.Count - 1;
			while (num >= 0 && box.CompareTo(this._semanticBoxes[num]) != 1)
			{
				num--;
			}
			this._semanticBoxes.Insert(num + 1, box);
			this._UpdateBoundingRect(box.BoundingRect);
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000D0579 File Offset: 0x000CE779
		protected void Add(FixedSOMSemanticBox box)
		{
			this._semanticBoxes.Add(box);
			this._UpdateBoundingRect(box.BoundingRect);
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x000D0593 File Offset: 0x000CE793
		internal virtual FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[0];
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x000D059B File Offset: 0x000CE79B
		// (set) Token: 0x06002E1C RID: 11804 RVA: 0x000D05A3 File Offset: 0x000CE7A3
		public List<FixedSOMSemanticBox> SemanticBoxes
		{
			get
			{
				return this._semanticBoxes;
			}
			set
			{
				this._semanticBoxes = value;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000D05AC File Offset: 0x000CE7AC
		public List<FixedNode> FixedNodes
		{
			get
			{
				if (this._fixedNodes == null)
				{
					this._ConstructFixedNodes();
				}
				return this._fixedNodes;
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000D05C4 File Offset: 0x000CE7C4
		private void _ConstructFixedNodes()
		{
			this._fixedNodes = new List<FixedNode>();
			foreach (FixedSOMSemanticBox fixedSOMSemanticBox in this._semanticBoxes)
			{
				FixedSOMElement fixedSOMElement = fixedSOMSemanticBox as FixedSOMElement;
				if (fixedSOMElement != null)
				{
					this._fixedNodes.Add(fixedSOMElement.FixedNode);
				}
				else
				{
					FixedSOMContainer fixedSOMContainer = fixedSOMSemanticBox as FixedSOMContainer;
					List<FixedNode> fixedNodes = fixedSOMContainer.FixedNodes;
					foreach (FixedNode item in fixedNodes)
					{
						this._fixedNodes.Add(item);
					}
				}
			}
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000D0690 File Offset: 0x000CE890
		private void _UpdateBoundingRect(Rect rect)
		{
			if (this._boundingRect.IsEmpty)
			{
				this._boundingRect = rect;
				return;
			}
			this._boundingRect.Union(rect);
		}

		// Token: 0x04001DE9 RID: 7657
		protected List<FixedSOMSemanticBox> _semanticBoxes;

		// Token: 0x04001DEA RID: 7658
		protected List<FixedNode> _fixedNodes;
	}
}
