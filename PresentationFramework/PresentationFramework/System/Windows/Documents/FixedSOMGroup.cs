using System;

namespace System.Windows.Documents
{
	// Token: 0x0200035A RID: 858
	internal class FixedSOMGroup : FixedSOMPageElement, IComparable
	{
		// Token: 0x06002DC2 RID: 11714 RVA: 0x000CDB53 File Offset: 0x000CBD53
		public FixedSOMGroup(FixedSOMPage page) : base(page)
		{
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000CDE94 File Offset: 0x000CC094
		int IComparable.CompareTo(object comparedObj)
		{
			int result = int.MinValue;
			FixedSOMGroup fixedSOMGroup = comparedObj as FixedSOMGroup;
			if (fixedSOMGroup == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					comparedObj.GetType(),
					typeof(FixedSOMGroup)
				}), "comparedObj");
			}
			bool rtl = this.IsRTL && fixedSOMGroup.IsRTL;
			FixedSOMSemanticBox.SpatialComparison spatialComparison = base._CompareHorizontal(fixedSOMGroup, rtl);
			switch (base._CompareVertical(fixedSOMGroup))
			{
			case FixedSOMSemanticBox.SpatialComparison.Before:
				result = -1;
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
				if (spatialComparison <= FixedSOMSemanticBox.SpatialComparison.Equal)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.Equal:
				switch (spatialComparison)
				{
				case FixedSOMSemanticBox.SpatialComparison.Before:
				case FixedSOMSemanticBox.SpatialComparison.OverlapBefore:
					result = -1;
					break;
				case FixedSOMSemanticBox.SpatialComparison.Equal:
					result = 0;
					break;
				case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				case FixedSOMSemanticBox.SpatialComparison.After:
					result = 1;
					break;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.OverlapAfter:
				if (spatialComparison >= FixedSOMSemanticBox.SpatialComparison.Equal)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
				break;
			case FixedSOMSemanticBox.SpatialComparison.After:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000CDF70 File Offset: 0x000CC170
		public void AddContainer(FixedSOMPageElement pageElement)
		{
			FixedSOMFixedBlock fixedSOMFixedBlock = pageElement as FixedSOMFixedBlock;
			if (fixedSOMFixedBlock == null || (!fixedSOMFixedBlock.IsFloatingImage && !fixedSOMFixedBlock.IsWhiteSpace))
			{
				if (pageElement.IsRTL)
				{
					this._RTLCount++;
				}
				else
				{
					this._LTRCount++;
				}
			}
			this._semanticBoxes.Add(pageElement);
			if (this._boundingRect.IsEmpty)
			{
				this._boundingRect = pageElement.BoundingRect;
				return;
			}
			this._boundingRect.Union(pageElement.BoundingRect);
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000CDFF4 File Offset: 0x000CC1F4
		public override bool IsRTL
		{
			get
			{
				return this._RTLCount > this._LTRCount;
			}
		}

		// Token: 0x04001DC5 RID: 7621
		private int _RTLCount;

		// Token: 0x04001DC6 RID: 7622
		private int _LTRCount;
	}
}
