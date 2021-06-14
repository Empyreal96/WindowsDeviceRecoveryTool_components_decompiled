using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200034E RID: 846
	internal sealed class FixedFlowMap
	{
		// Token: 0x06002D2A RID: 11562 RVA: 0x000CC100 File Offset: 0x000CA300
		internal FixedFlowMap()
		{
			this._Init();
		}

		// Token: 0x17000B3C RID: 2876
		internal FlowNode this[int fp]
		{
			get
			{
				return this._flowOrder[fp];
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000CC11C File Offset: 0x000CA31C
		internal void MappingReplace(FlowNode flowOld, List<FlowNode> flowNew)
		{
			int fp = flowOld.Fp;
			this._flowOrder.RemoveAt(fp);
			this._flowOrder.InsertRange(fp, flowNew);
			for (int i = fp; i < this._flowOrder.Count; i++)
			{
				this._flowOrder[i].SetFp(i);
			}
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000CC174 File Offset: 0x000CA374
		internal FixedSOMElement MappingGetFixedSOMElement(FixedNode fixedp, int offset)
		{
			List<FixedSOMElement> list = this._GetEntry(fixedp);
			if (list != null)
			{
				foreach (FixedSOMElement fixedSOMElement in list)
				{
					if (offset >= fixedSOMElement.StartIndex && offset <= fixedSOMElement.EndIndex)
					{
						return fixedSOMElement;
					}
				}
			}
			return null;
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000CC1E0 File Offset: 0x000CA3E0
		internal FlowNode FlowOrderInsertBefore(FlowNode nextFlow, FlowNode newFlow)
		{
			this._FlowOrderInsertBefore(nextFlow, newFlow);
			return newFlow;
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000CC1EB File Offset: 0x000CA3EB
		internal void AddFixedElement(FixedSOMElement element)
		{
			this._AddEntry(element);
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000CC1F4 File Offset: 0x000CA3F4
		internal FixedNode FixedStartEdge
		{
			get
			{
				return FixedFlowMap.s_FixedStart;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x000CC1FB File Offset: 0x000CA3FB
		internal FlowNode FlowStartEdge
		{
			get
			{
				return this._flowStart;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000CC203 File Offset: 0x000CA403
		internal FlowNode FlowEndEdge
		{
			get
			{
				return this._flowEnd;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000CC20B File Offset: 0x000CA40B
		internal int FlowCount
		{
			get
			{
				return this._flowOrder.Count;
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000CC218 File Offset: 0x000CA418
		private void _Init()
		{
			this._flowStart = new FlowNode(int.MinValue, FlowNodeType.Boundary, null);
			this._flowEnd = new FlowNode(int.MinValue, FlowNodeType.Boundary, null);
			this._flowOrder = new List<FlowNode>();
			this._flowOrder.Add(this._flowStart);
			this._flowStart.SetFp(0);
			this._flowOrder.Add(this._flowEnd);
			this._flowEnd.SetFp(1);
			this._mapping = new Hashtable();
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000CC29C File Offset: 0x000CA49C
		internal void _FlowOrderInsertBefore(FlowNode nextFlow, FlowNode newFlow)
		{
			newFlow.SetFp(nextFlow.Fp);
			this._flowOrder.Insert(newFlow.Fp, newFlow);
			int i = newFlow.Fp + 1;
			int count = this._flowOrder.Count;
			while (i < count)
			{
				this._flowOrder[i].IncreaseFp();
				i++;
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000CC2F8 File Offset: 0x000CA4F8
		private List<FixedSOMElement> _GetEntry(FixedNode node)
		{
			if (this._cachedEntry == null || node != this._cachedFixedNode)
			{
				this._cachedEntry = (List<FixedSOMElement>)this._mapping[node];
				this._cachedFixedNode = node;
			}
			return this._cachedEntry;
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000CC344 File Offset: 0x000CA544
		private void _AddEntry(FixedSOMElement element)
		{
			FixedNode fixedNode = element.FixedNode;
			List<FixedSOMElement> list;
			if (this._mapping.ContainsKey(fixedNode))
			{
				list = (List<FixedSOMElement>)this._mapping[fixedNode];
			}
			else
			{
				list = new List<FixedSOMElement>();
				this._mapping.Add(fixedNode, list);
			}
			list.Add(element);
		}

		// Token: 0x04001D8A RID: 7562
		internal const int FixedOrderStartPage = -2147483648;

		// Token: 0x04001D8B RID: 7563
		internal const int FixedOrderEndPage = 2147483647;

		// Token: 0x04001D8C RID: 7564
		internal const int FixedOrderStartVisual = -2147483648;

		// Token: 0x04001D8D RID: 7565
		internal const int FixedOrderEndVisual = 2147483647;

		// Token: 0x04001D8E RID: 7566
		internal const int FlowOrderBoundaryScopeId = -2147483648;

		// Token: 0x04001D8F RID: 7567
		internal const int FlowOrderVirtualScopeId = -1;

		// Token: 0x04001D90 RID: 7568
		internal const int FlowOrderScopeIdStart = 0;

		// Token: 0x04001D91 RID: 7569
		private List<FlowNode> _flowOrder;

		// Token: 0x04001D92 RID: 7570
		private FlowNode _flowStart;

		// Token: 0x04001D93 RID: 7571
		private FlowNode _flowEnd;

		// Token: 0x04001D94 RID: 7572
		private static readonly FixedNode s_FixedStart = FixedNode.Create(int.MinValue, 1, int.MinValue, -1, null);

		// Token: 0x04001D95 RID: 7573
		private static readonly FixedNode s_FixedEnd = FixedNode.Create(int.MaxValue, 1, int.MaxValue, -1, null);

		// Token: 0x04001D96 RID: 7574
		private Hashtable _mapping;

		// Token: 0x04001D97 RID: 7575
		private FixedNode _cachedFixedNode;

		// Token: 0x04001D98 RID: 7576
		private List<FixedSOMElement> _cachedEntry;
	}
}
