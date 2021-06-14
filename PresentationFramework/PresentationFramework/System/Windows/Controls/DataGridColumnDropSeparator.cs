using System;
using System.Windows.Controls.Primitives;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004A1 RID: 1185
	internal class DataGridColumnDropSeparator : Separator
	{
		// Token: 0x06004872 RID: 18546 RVA: 0x00149968 File Offset: 0x00147B68
		static DataGridColumnDropSeparator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceHeight)));
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x001499E0 File Offset: 0x00147BE0
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			if (DoubleUtil.IsNaN(value))
			{
				return 2.0;
			}
			return baseValue;
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x00149A0C File Offset: 0x00147C0C
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnDropSeparator dataGridColumnDropSeparator = (DataGridColumnDropSeparator)d;
			if (dataGridColumnDropSeparator._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnDropSeparator._referenceHeader.ActualHeight;
			}
			return baseValue;
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x06004875 RID: 18549 RVA: 0x00149A49 File Offset: 0x00147C49
		// (set) Token: 0x06004876 RID: 18550 RVA: 0x00149A51 File Offset: 0x00147C51
		internal DataGridColumnHeader ReferenceHeader
		{
			get
			{
				return this._referenceHeader;
			}
			set
			{
				this._referenceHeader = value;
			}
		}

		// Token: 0x04002999 RID: 10649
		private DataGridColumnHeader _referenceHeader;
	}
}
