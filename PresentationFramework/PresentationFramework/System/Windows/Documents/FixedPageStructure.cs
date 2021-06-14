using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x02000356 RID: 854
	internal sealed class FixedPageStructure
	{
		// Token: 0x06002D94 RID: 11668 RVA: 0x000CD610 File Offset: 0x000CB810
		internal FixedPageStructure(int pageIndex)
		{
			this._pageIndex = pageIndex;
			this._flowStart = new FlowNode(-1, FlowNodeType.Virtual, pageIndex);
			this._flowEnd = this._flowStart;
			this._fixedStart = FixedNode.Create(pageIndex, 1, int.MinValue, -1, null);
			this._fixedEnd = FixedNode.Create(pageIndex, 1, int.MaxValue, -1, null);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000CD672 File Offset: 0x000CB872
		internal void SetupLineResults(FixedLineResult[] lineResults)
		{
			this._lineResults = lineResults;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000CD67C File Offset: 0x000CB87C
		internal FixedNode[] GetNextLine(int line, bool forward, ref int count)
		{
			if (forward)
			{
				while (line < this._lineResults.Length - 1)
				{
					if (count <= 0)
					{
						break;
					}
					line++;
					count--;
				}
			}
			else
			{
				while (line > 0 && count > 0)
				{
					line--;
					count--;
				}
			}
			if (count <= 0)
			{
				line = Math.Max(0, Math.Min(line, this._lineResults.Length - 1));
				return this._lineResults[line].Nodes;
			}
			return null;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000CD6F0 File Offset: 0x000CB8F0
		internal FixedNode[] FindSnapToLine(Point pt)
		{
			FixedLineResult fixedLineResult = null;
			FixedLineResult fixedLineResult2 = null;
			double num = double.MaxValue;
			double num2 = double.MaxValue;
			double num3 = double.MaxValue;
			foreach (FixedLineResult fixedLineResult3 in this._lineResults)
			{
				double num4 = Math.Max(0.0, (pt.Y > fixedLineResult3.LayoutBox.Y) ? (pt.Y - fixedLineResult3.LayoutBox.Bottom) : (fixedLineResult3.LayoutBox.Y - pt.Y));
				double num5 = Math.Max(0.0, (pt.X > fixedLineResult3.LayoutBox.X) ? (pt.X - fixedLineResult3.LayoutBox.Right) : (fixedLineResult3.LayoutBox.X - pt.X));
				if (num4 == 0.0 && num5 == 0.0)
				{
					return fixedLineResult3.Nodes;
				}
				if (num4 < num || (num4 == num && num5 < num2))
				{
					num = num4;
					num2 = num5;
					fixedLineResult = fixedLineResult3;
				}
				double num6 = 5.0 * num4 + num5;
				if (num6 < num3 && num4 < fixedLineResult3.LayoutBox.Height)
				{
					num3 = num6;
					fixedLineResult2 = fixedLineResult3;
				}
			}
			if (fixedLineResult == null)
			{
				return null;
			}
			if (fixedLineResult2 != null && (fixedLineResult2.LayoutBox.Left > fixedLineResult.LayoutBox.Right || fixedLineResult.LayoutBox.Left > fixedLineResult2.LayoutBox.Right))
			{
				return fixedLineResult2.Nodes;
			}
			return fixedLineResult.Nodes;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000CD8BF File Offset: 0x000CBABF
		internal void SetFlowBoundary(FlowNode flowStart, FlowNode flowEnd)
		{
			this._flowStart = flowStart;
			this._flowEnd = flowEnd;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000CD8CF File Offset: 0x000CBACF
		public void ConstructFixedSOMPage(List<FixedNode> fixedNodes)
		{
			this._fixedSOMPageConstructor.ConstructPageStructure(fixedNodes);
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x000CD8DE File Offset: 0x000CBADE
		internal FixedNode[] LastLine
		{
			get
			{
				if (this._lineResults.Length != 0)
				{
					return this._lineResults[this._lineResults.Length - 1].Nodes;
				}
				return null;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002D9B RID: 11675 RVA: 0x000CD901 File Offset: 0x000CBB01
		internal FixedNode[] FirstLine
		{
			get
			{
				if (this._lineResults.Length != 0)
				{
					return this._lineResults[0].Nodes;
				}
				return null;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x000CD91B File Offset: 0x000CBB1B
		internal int PageIndex
		{
			get
			{
				return this._pageIndex;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002D9D RID: 11677 RVA: 0x000CD923 File Offset: 0x000CBB23
		internal bool Loaded
		{
			get
			{
				return this._flowStart != null && this._flowStart.Type != FlowNodeType.Virtual;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000CD941 File Offset: 0x000CBB41
		internal FlowNode FlowStart
		{
			get
			{
				return this._flowStart;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x000CD949 File Offset: 0x000CBB49
		internal FlowNode FlowEnd
		{
			get
			{
				return this._flowEnd;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000CD951 File Offset: 0x000CBB51
		// (set) Token: 0x06002DA1 RID: 11681 RVA: 0x000CD959 File Offset: 0x000CBB59
		internal FixedSOMPage FixedSOMPage
		{
			get
			{
				return this._fixedSOMPage;
			}
			set
			{
				this._fixedSOMPage = value;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x000CD962 File Offset: 0x000CBB62
		// (set) Token: 0x06002DA3 RID: 11683 RVA: 0x000CD96A File Offset: 0x000CBB6A
		internal FixedDSBuilder FixedDSBuilder
		{
			get
			{
				return this._fixedDSBuilder;
			}
			set
			{
				this._fixedDSBuilder = value;
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002DA4 RID: 11684 RVA: 0x000CD973 File Offset: 0x000CBB73
		// (set) Token: 0x06002DA5 RID: 11685 RVA: 0x000CD97B File Offset: 0x000CBB7B
		internal FixedSOMPageConstructor PageConstructor
		{
			get
			{
				return this._fixedSOMPageConstructor;
			}
			set
			{
				this._fixedSOMPageConstructor = value;
			}
		}

		// Token: 0x04001DB1 RID: 7601
		private readonly int _pageIndex;

		// Token: 0x04001DB2 RID: 7602
		private FlowNode _flowStart;

		// Token: 0x04001DB3 RID: 7603
		private FlowNode _flowEnd;

		// Token: 0x04001DB4 RID: 7604
		private FixedNode _fixedStart;

		// Token: 0x04001DB5 RID: 7605
		private FixedNode _fixedEnd;

		// Token: 0x04001DB6 RID: 7606
		private FixedSOMPageConstructor _fixedSOMPageConstructor;

		// Token: 0x04001DB7 RID: 7607
		private FixedSOMPage _fixedSOMPage;

		// Token: 0x04001DB8 RID: 7608
		private FixedDSBuilder _fixedDSBuilder;

		// Token: 0x04001DB9 RID: 7609
		private FixedLineResult[] _lineResults;
	}
}
