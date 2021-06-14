using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062F RID: 1583
	internal sealed class MarginCollapsingState : UnmanagedHandle
	{
		// Token: 0x060068B6 RID: 26806 RVA: 0x001D8F44 File Offset: 0x001D7144
		internal static void CollapseTopMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginTop);
			if (mcsCurrent != null)
			{
				mcsNew.Collapse(mcsCurrent);
			}
			if (mbp.BPTop != 0)
			{
				margin = mcsNew.Margin;
				mcsNew.Dispose();
				mcsNew = null;
				return;
			}
			if (mcsCurrent == null && DoubleUtil.IsZero(mbp.Margin.Top))
			{
				mcsNew.Dispose();
				mcsNew = null;
			}
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x001D8FB0 File Offset: 0x001D71B0
		internal static void CollapseBottomMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			if (!DoubleUtil.IsZero(mbp.Margin.Bottom))
			{
				mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginBottom);
			}
			if (mcsCurrent != null)
			{
				if (mbp.BPBottom != 0)
				{
					margin = mcsCurrent.Margin;
					return;
				}
				if (mcsNew == null)
				{
					mcsNew = new MarginCollapsingState(ptsContext, 0);
				}
				mcsNew.Collapse(mcsCurrent);
			}
		}

		// Token: 0x060068B8 RID: 26808 RVA: 0x001D9011 File Offset: 0x001D7211
		internal MarginCollapsingState(PtsContext ptsContext, int margin) : base(ptsContext)
		{
			this._maxPositive = ((margin >= 0) ? margin : 0);
			this._minNegative = ((margin < 0) ? margin : 0);
		}

		// Token: 0x060068B9 RID: 26809 RVA: 0x001D9036 File Offset: 0x001D7236
		private MarginCollapsingState(MarginCollapsingState mcs) : base(mcs.PtsContext)
		{
			this._maxPositive = mcs._maxPositive;
			this._minNegative = mcs._minNegative;
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x001D905C File Offset: 0x001D725C
		internal MarginCollapsingState Clone()
		{
			return new MarginCollapsingState(this);
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x001D9064 File Offset: 0x001D7264
		internal bool IsEqual(MarginCollapsingState mcs)
		{
			return this._maxPositive == mcs._maxPositive && this._minNegative == mcs._minNegative;
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x001D9084 File Offset: 0x001D7284
		internal void Collapse(MarginCollapsingState mcs)
		{
			this._maxPositive = Math.Max(this._maxPositive, mcs._maxPositive);
			this._minNegative = Math.Min(this._minNegative, mcs._minNegative);
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x060068BD RID: 26813 RVA: 0x001D90B4 File Offset: 0x001D72B4
		internal int Margin
		{
			get
			{
				return this._maxPositive + this._minNegative;
			}
		}

		// Token: 0x040033ED RID: 13293
		private int _maxPositive;

		// Token: 0x040033EE RID: 13294
		private int _minNegative;
	}
}
