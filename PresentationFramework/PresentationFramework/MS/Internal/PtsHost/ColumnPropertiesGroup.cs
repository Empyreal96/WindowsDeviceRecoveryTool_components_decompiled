using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000613 RID: 1555
	internal sealed class ColumnPropertiesGroup
	{
		// Token: 0x06006783 RID: 26499 RVA: 0x001CF28C File Offset: 0x001CD48C
		internal ColumnPropertiesGroup(DependencyObject o)
		{
			this._columnWidth = (double)o.GetValue(FlowDocument.ColumnWidthProperty);
			this._columnGap = (double)o.GetValue(FlowDocument.ColumnGapProperty);
			this._columnRuleWidth = (double)o.GetValue(FlowDocument.ColumnRuleWidthProperty);
			this._columnRuleBrush = (Brush)o.GetValue(FlowDocument.ColumnRuleBrushProperty);
			this._isColumnWidthFlexible = (bool)o.GetValue(FlowDocument.IsColumnWidthFlexibleProperty);
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x06006784 RID: 26500 RVA: 0x001CF30D File Offset: 0x001CD50D
		internal double ColumnWidth
		{
			get
			{
				return this._columnWidth;
			}
		}

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x001CF315 File Offset: 0x001CD515
		internal bool IsColumnWidthFlexible
		{
			get
			{
				return this._isColumnWidthFlexible;
			}
		}

		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x06006786 RID: 26502 RVA: 0x00094C44 File Offset: 0x00092E44
		internal ColumnSpaceDistribution ColumnSpaceDistribution
		{
			get
			{
				return ColumnSpaceDistribution.Between;
			}
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x06006787 RID: 26503 RVA: 0x001CF31D File Offset: 0x001CD51D
		internal double ColumnGap
		{
			get
			{
				Invariant.Assert(!double.IsNaN(this._columnGap));
				return this._columnGap;
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x06006788 RID: 26504 RVA: 0x001CF338 File Offset: 0x001CD538
		internal Brush ColumnRuleBrush
		{
			get
			{
				return this._columnRuleBrush;
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x06006789 RID: 26505 RVA: 0x001CF340 File Offset: 0x001CD540
		internal double ColumnRuleWidth
		{
			get
			{
				return this._columnRuleWidth;
			}
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x0600678A RID: 26506 RVA: 0x001CF348 File Offset: 0x001CD548
		internal bool ColumnWidthAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnWidth);
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x0600678B RID: 26507 RVA: 0x001CF355 File Offset: 0x001CD555
		internal bool ColumnGapAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnGap);
			}
		}

		// Token: 0x04003375 RID: 13173
		private double _columnWidth;

		// Token: 0x04003376 RID: 13174
		private bool _isColumnWidthFlexible;

		// Token: 0x04003377 RID: 13175
		private double _columnGap;

		// Token: 0x04003378 RID: 13176
		private Brush _columnRuleBrush;

		// Token: 0x04003379 RID: 13177
		private double _columnRuleWidth;
	}
}
