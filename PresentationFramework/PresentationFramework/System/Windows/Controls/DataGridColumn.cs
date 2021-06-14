using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column.</summary>
	// Token: 0x0200049F RID: 1183
	public abstract class DataGridColumn : DependencyObject
	{
		/// <summary>Gets or sets the content of the column header.</summary>
		/// <returns>The column header content. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600479E RID: 18334 RVA: 0x001455CA File Offset: 0x001437CA
		// (set) Token: 0x0600479F RID: 18335 RVA: 0x001455D7 File Offset: 0x001437D7
		public object Header
		{
			get
			{
				return base.GetValue(DataGridColumn.HeaderProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the column header.</summary>
		/// <returns>The style that is used to render the column header; or <see langword="null" />, to use the <see cref="P:System.Windows.Controls.DataGrid.ColumnHeaderStyle" /> setting. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x001455E5 File Offset: 0x001437E5
		// (set) Token: 0x060047A1 RID: 18337 RVA: 0x001455F7 File Offset: 0x001437F7
		public Style HeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.HeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderStyleProperty, value);
			}
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x00145608 File Offset: 0x00143808
		private static object OnCoerceHeaderStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.HeaderStyleProperty, dataGridColumn.DataGridOwner, DataGrid.ColumnHeaderStyleProperty);
		}

		/// <summary>Gets or sets the format pattern to apply to the content of the column header.</summary>
		/// <returns>A string value that represents the formatting pattern. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x060047A3 RID: 18339 RVA: 0x00145633 File Offset: 0x00143833
		// (set) Token: 0x060047A4 RID: 18340 RVA: 0x00145645 File Offset: 0x00143845
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(DataGridColumn.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderStringFormatProperty, value);
			}
		}

		/// <summary>Gets or sets the template that defines the visual representation of the column header.</summary>
		/// <returns>The object that defines the visual representation of the column header. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x060047A5 RID: 18341 RVA: 0x00145653 File Offset: 0x00143853
		// (set) Token: 0x060047A6 RID: 18342 RVA: 0x00145665 File Offset: 0x00143865
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridColumn.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the object that selects which template to use for the column header.</summary>
		/// <returns>The object that selects the template. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x060047A7 RID: 18343 RVA: 0x00145673 File Offset: 0x00143873
		// (set) Token: 0x060047A8 RID: 18344 RVA: 0x00145685 File Offset: 0x00143885
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridColumn.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.HeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is used to render cells in the column.</summary>
		/// <returns>The style that is used to render cells in the column. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x00145693 File Offset: 0x00143893
		// (set) Token: 0x060047AA RID: 18346 RVA: 0x001456A5 File Offset: 0x001438A5
		public Style CellStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.CellStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CellStyleProperty, value);
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x001456B4 File Offset: 0x001438B4
		private static object OnCoerceCellStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CellStyleProperty, dataGridColumn.DataGridOwner, DataGrid.CellStyleProperty);
		}

		/// <summary>Gets or sets a value that indicates whether cells in the column can be edited.</summary>
		/// <returns>
		///     <see langword="true" /> if cells in the column cannot be edited; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x001456DF File Offset: 0x001438DF
		// (set) Token: 0x060047AD RID: 18349 RVA: 0x001456F1 File Offset: 0x001438F1
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x00145700 File Offset: 0x00143900
		private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return dataGridColumn.OnCoerceIsReadOnly((bool)baseValue);
		}

		/// <summary>Determines the value of the <see cref="P:System.Windows.Controls.DataGridColumn.IsReadOnly" /> property based on the property rules of the <see cref="T:System.Windows.Controls.DataGrid" /> that contains this column.</summary>
		/// <param name="baseValue">The value that was passed to the delegate.</param>
		/// <returns>
		///     <see langword="true" /> if cells in the column cannot be edited based on rules from the <see cref="T:System.Windows.Controls.DataGrid" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060047AF RID: 18351 RVA: 0x00145725 File Offset: 0x00143925
		protected virtual bool OnCoerceIsReadOnly(bool baseValue)
		{
			return (bool)DataGridHelper.GetCoercedTransferPropertyValue(this, baseValue, DataGridColumn.IsReadOnlyProperty, this.DataGridOwner, DataGrid.IsReadOnlyProperty);
		}

		/// <summary>Gets or sets the column width or automatic sizing mode.</summary>
		/// <returns>A structure that represents the column width or automatic sizing mode. The registered default is <see cref="P:System.Windows.Controls.DataGridLength.Auto" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x00145748 File Offset: 0x00143948
		// (set) Token: 0x060047B1 RID: 18353 RVA: 0x0014575A File Offset: 0x0014395A
		public DataGridLength Width
		{
			get
			{
				return (DataGridLength)base.GetValue(DataGridColumn.WidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.WidthProperty, value);
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x00145770 File Offset: 0x00143970
		internal void SetWidthInternal(DataGridLength width)
		{
			bool ignoreRedistributionOnWidthChange = this._ignoreRedistributionOnWidthChange;
			this._ignoreRedistributionOnWidthChange = true;
			try
			{
				this.Width = width;
			}
			finally
			{
				this._ignoreRedistributionOnWidthChange = ignoreRedistributionOnWidthChange;
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x001457AC File Offset: 0x001439AC
		private static void OnWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGridLength dataGridLength = (DataGridLength)e.OldValue;
			DataGridLength dataGridLength2 = (DataGridLength)e.NewValue;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			if (dataGridOwner != null && !DoubleUtil.AreClose(dataGridLength.DisplayValue, dataGridLength2.DisplayValue))
			{
				dataGridOwner.InternalColumns.InvalidateAverageColumnWidth();
			}
			if (dataGridColumn._processingWidthChange)
			{
				dataGridColumn.CoerceValue(DataGridColumn.ActualWidthProperty);
				return;
			}
			dataGridColumn._processingWidthChange = true;
			if (dataGridLength.IsStar != dataGridLength2.IsStar)
			{
				dataGridColumn.CoerceValue(DataGridColumn.MaxWidthProperty);
			}
			try
			{
				if (dataGridOwner != null && (dataGridLength2.IsStar ^ dataGridLength.IsStar))
				{
					dataGridOwner.InternalColumns.InvalidateHasVisibleStarColumns();
				}
				dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
				if (dataGridOwner != null && !dataGridColumn._ignoreRedistributionOnWidthChange && dataGridColumn.IsVisible)
				{
					if (!dataGridLength2.IsStar && !dataGridLength2.IsAbsolute)
					{
						DataGridLength width = dataGridColumn.Width;
						double displayValue = DataGridHelper.CoerceToMinMax(width.DesiredValue, dataGridColumn.MinWidth, dataGridColumn.MaxWidth);
						dataGridColumn.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, width.DesiredValue, displayValue));
					}
					dataGridOwner.InternalColumns.RedistributeColumnWidthsOnWidthChangeOfColumn(dataGridColumn, (DataGridLength)e.OldValue);
				}
			}
			finally
			{
				dataGridColumn._processingWidthChange = false;
			}
		}

		/// <summary>Gets or sets the minimum width constraint of the column.</summary>
		/// <returns>The minimum column width, in device-independent units (1/96th inch per unit). The registered default is 20. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x00145900 File Offset: 0x00143B00
		// (set) Token: 0x060047B5 RID: 18357 RVA: 0x00145912 File Offset: 0x00143B12
		public double MinWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.MinWidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.MinWidthProperty, value);
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x00145928 File Offset: 0x00143B28
		private static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
			if (dataGridOwner != null && dataGridColumn.IsVisible)
			{
				dataGridOwner.InternalColumns.RedistributeColumnWidthsOnMinWidthChangeOfColumn(dataGridColumn, (double)e.OldValue);
			}
		}

		/// <summary>Gets or sets the maximum width constraint of the column.</summary>
		/// <returns>The maximum column width, in device-independent units (1/96th inch per unit). The registered default is <see cref="F:System.Double.PositiveInfinity" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x0014596F File Offset: 0x00143B6F
		// (set) Token: 0x060047B8 RID: 18360 RVA: 0x00145981 File Offset: 0x00143B81
		public double MaxWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.MaxWidthProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.MaxWidthProperty, value);
			}
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x00145994 File Offset: 0x00143B94
		private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			dataGridColumn.NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
			if (dataGridOwner != null && dataGridColumn.IsVisible)
			{
				dataGridOwner.InternalColumns.RedistributeColumnWidthsOnMaxWidthChangeOfColumn(dataGridColumn, (double)e.OldValue);
			}
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x001459DB File Offset: 0x00143BDB
		private static double CoerceDesiredOrDisplayWidthValue(double widthValue, double memberValue, DataGridLengthUnitType type)
		{
			if (DoubleUtil.IsNaN(memberValue))
			{
				if (type == DataGridLengthUnitType.Pixel)
				{
					memberValue = widthValue;
				}
				else if (type == DataGridLengthUnitType.Auto || type == DataGridLengthUnitType.SizeToCells || type == DataGridLengthUnitType.SizeToHeader)
				{
					memberValue = 0.0;
				}
			}
			return memberValue;
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x00145A08 File Offset: 0x00143C08
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			DataGridLength dataGridLength = (DataGridLength)DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.WidthProperty, dataGridColumn.DataGridOwner, DataGrid.ColumnWidthProperty);
			double desiredValue = DataGridColumn.CoerceDesiredOrDisplayWidthValue(dataGridLength.Value, dataGridLength.DesiredValue, dataGridLength.UnitType);
			double num = DataGridColumn.CoerceDesiredOrDisplayWidthValue(dataGridLength.Value, dataGridLength.DisplayValue, dataGridLength.UnitType);
			num = (DoubleUtil.IsNaN(num) ? num : DataGridHelper.CoerceToMinMax(num, dataGridColumn.MinWidth, dataGridColumn.MaxWidth));
			if (DoubleUtil.IsNaN(num) || DoubleUtil.AreClose(num, dataGridLength.DisplayValue))
			{
				return dataGridLength;
			}
			return new DataGridLength(dataGridLength.Value, dataGridLength.UnitType, desiredValue, num);
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x00145AC8 File Offset: 0x00143CC8
		private static object OnCoerceMinWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.MinWidthProperty, dataGridColumn.DataGridOwner, DataGrid.MinColumnWidthProperty);
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x00145AF4 File Offset: 0x00143CF4
		private static object OnCoerceMaxWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			double num = (double)DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.MaxWidthProperty, dataGridColumn.DataGridOwner, DataGrid.MaxColumnWidthProperty);
			if (double.IsPositiveInfinity(num) && dataGridColumn.Width.IsStar)
			{
				return 10000.0;
			}
			return num;
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x00145B54 File Offset: 0x00143D54
		private static bool ValidateMinWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num) && !double.IsPositiveInfinity(num);
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x00145B88 File Offset: 0x00143D88
		private static bool ValidateMaxWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num);
		}

		/// <summary>Gets the current width of the column, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The width of the column in device-independent units (1/96th inch per unit). The registered default is 0. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060047C0 RID: 18368 RVA: 0x00145BB3 File Offset: 0x00143DB3
		// (set) Token: 0x060047C1 RID: 18369 RVA: 0x00145BC5 File Offset: 0x00143DC5
		public double ActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGridColumn.ActualWidthProperty);
			}
			private set
			{
				base.SetValue(DataGridColumn.ActualWidthPropertyKey, value);
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x00145BD8 File Offset: 0x00143DD8
		private static object OnCoerceActualWidth(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			double num = (double)baseValue;
			double minWidth = dataGridColumn.MinWidth;
			double maxWidth = dataGridColumn.MaxWidth;
			DataGridLength width = dataGridColumn.Width;
			if (width.IsAbsolute)
			{
				num = width.DisplayValue;
			}
			if (num < minWidth)
			{
				num = minWidth;
			}
			else if (num > maxWidth)
			{
				num = maxWidth;
			}
			return num;
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x00145C30 File Offset: 0x00143E30
		internal double GetConstraintWidth(bool isHeader)
		{
			DataGridLength width = this.Width;
			if (!DoubleUtil.IsNaN(width.DisplayValue))
			{
				return width.DisplayValue;
			}
			if (width.IsAbsolute || width.IsStar || (width.IsSizeToCells && isHeader) || (width.IsSizeToHeader && !isHeader))
			{
				return this.ActualWidth;
			}
			return double.PositiveInfinity;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00145C94 File Offset: 0x00143E94
		internal void UpdateDesiredWidthForAutoColumn(bool isHeader, double pixelWidth)
		{
			DataGridLength width = this.Width;
			double minWidth = this.MinWidth;
			double maxWidth = this.MaxWidth;
			double num = DataGridHelper.CoerceToMinMax(pixelWidth, minWidth, maxWidth);
			if (width.IsAuto || (width.IsSizeToCells && !isHeader) || (width.IsSizeToHeader && isHeader))
			{
				if (DoubleUtil.IsNaN(width.DesiredValue) || DoubleUtil.LessThan(width.DesiredValue, pixelWidth))
				{
					if (DoubleUtil.IsNaN(width.DisplayValue))
					{
						this.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, pixelWidth, num));
					}
					else
					{
						double value = DataGridHelper.CoerceToMinMax(width.DesiredValue, minWidth, maxWidth);
						this.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, pixelWidth, width.DisplayValue));
						if (DoubleUtil.AreClose(value, width.DisplayValue))
						{
							this.DataGridOwner.InternalColumns.RecomputeColumnWidthsOnColumnResize(this, pixelWidth - width.DisplayValue, true);
						}
					}
					width = this.Width;
				}
				if (DoubleUtil.IsNaN(width.DisplayValue))
				{
					if (this.ActualWidth < num)
					{
						this.ActualWidth = num;
						return;
					}
				}
				else if (!DoubleUtil.AreClose(this.ActualWidth, width.DisplayValue))
				{
					this.ActualWidth = width.DisplayValue;
				}
			}
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x00145DD0 File Offset: 0x00143FD0
		internal void UpdateWidthForStarColumn(double displayWidth, double desiredWidth, double starValue)
		{
			DataGridLength width = this.Width;
			if (!DoubleUtil.AreClose(displayWidth, width.DisplayValue) || !DoubleUtil.AreClose(desiredWidth, width.DesiredValue) || !DoubleUtil.AreClose(width.Value, starValue))
			{
				this.SetWidthInternal(new DataGridLength(starValue, width.UnitType, desiredWidth, displayWidth));
				this.ActualWidth = displayWidth;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property value for the cell at the intersection of this column and the row that represents the specified data item.</summary>
		/// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
		/// <returns>The cell content; or <see langword="null" />, if the cell is not found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataItem" /> is <see langword="null" />.</exception>
		// Token: 0x060047C6 RID: 18374 RVA: 0x00145E30 File Offset: 0x00144030
		public FrameworkElement GetCellContent(object dataItem)
		{
			if (dataItem == null)
			{
				throw new ArgumentNullException("dataItem");
			}
			if (this._dataGridOwner != null)
			{
				DataGridRow dataGridRow = this._dataGridOwner.ItemContainerGenerator.ContainerFromItem(dataItem) as DataGridRow;
				if (dataGridRow != null)
				{
					return this.GetCellContent(dataGridRow);
				}
			}
			return null;
		}

		/// <summary>Retrieves the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property value for the cell at the intersection of this column and the specified row.</summary>
		/// <param name="dataGridRow">The row that contains the intended cell.</param>
		/// <returns>The cell content; or <see langword="null" />, if the cell is not found.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridRow" /> is <see langword="null" />.</exception>
		// Token: 0x060047C7 RID: 18375 RVA: 0x00145E78 File Offset: 0x00144078
		public FrameworkElement GetCellContent(DataGridRow dataGridRow)
		{
			if (dataGridRow == null)
			{
				throw new ArgumentNullException("dataGridRow");
			}
			if (this._dataGridOwner != null)
			{
				int num = this._dataGridOwner.Columns.IndexOf(this);
				if (num >= 0)
				{
					DataGridCell dataGridCell = dataGridRow.TryGetCell(num);
					if (dataGridCell != null)
					{
						return dataGridCell.Content as FrameworkElement;
					}
				}
			}
			return null;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x00145EC9 File Offset: 0x001440C9
		internal FrameworkElement BuildVisualTree(bool isEditing, object dataItem, DataGridCell cell)
		{
			if (isEditing)
			{
				return this.GenerateEditingElement(cell, dataItem);
			}
			return this.GenerateElement(cell, dataItem);
		}

		/// <summary>When overridden in a derived class, gets a read-only element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
		/// <returns>A new read-only element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.</returns>
		// Token: 0x060047C9 RID: 18377
		protected abstract FrameworkElement GenerateElement(DataGridCell cell, object dataItem);

		/// <summary>When overridden in a derived class, gets an editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
		/// <returns>A new editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.</returns>
		// Token: 0x060047CA RID: 18378
		protected abstract FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem);

		/// <summary>When overridden in a derived class, sets cell content as needed for editing.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
		/// <returns>When returned by a derived class, the unedited cell value. This implementation returns <see langword="null" /> in all cases.</returns>
		// Token: 0x060047CB RID: 18379 RVA: 0x0000C238 File Offset: 0x0000A438
		protected virtual object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			return null;
		}

		/// <summary>Causes the cell being edited to revert to the original, unedited value.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="uneditedValue">The original, unedited value in the cell being edited.</param>
		// Token: 0x060047CC RID: 18380 RVA: 0x00145EDF File Offset: 0x001440DF
		protected virtual void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.UpdateTarget(editingElement);
		}

		/// <summary>Performs any required validation before exiting cell editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <returns>
		///     <see langword="true" /> if no validation errors are found; otherwise, <see langword="false" />.</returns>
		// Token: 0x060047CD RID: 18381 RVA: 0x00145EE7 File Offset: 0x001440E7
		protected virtual bool CommitCellEdit(FrameworkElement editingElement)
		{
			return DataGridHelper.ValidateWithoutUpdate(editingElement);
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x00145EF0 File Offset: 0x001440F0
		internal void BeginEdit(FrameworkElement editingElement, RoutedEventArgs e)
		{
			if (editingElement != null)
			{
				editingElement.UpdateLayout();
				object value = this.PrepareCellForEdit(editingElement, e);
				DataGridColumn.SetOriginalValue(editingElement, value);
			}
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x00145F16 File Offset: 0x00144116
		internal void CancelEdit(FrameworkElement editingElement)
		{
			if (editingElement != null)
			{
				this.CancelCellEdit(editingElement, DataGridColumn.GetOriginalValue(editingElement));
				DataGridColumn.ClearOriginalValue(editingElement);
			}
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x00145F2E File Offset: 0x0014412E
		internal bool CommitEdit(FrameworkElement editingElement)
		{
			if (editingElement == null)
			{
				return true;
			}
			if (this.CommitCellEdit(editingElement))
			{
				DataGridColumn.ClearOriginalValue(editingElement);
				return true;
			}
			return false;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x00145F47 File Offset: 0x00144147
		private static object GetOriginalValue(DependencyObject obj)
		{
			return obj.GetValue(DataGridColumn.OriginalValueProperty);
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x00145F54 File Offset: 0x00144154
		private static void SetOriginalValue(DependencyObject obj, object value)
		{
			obj.SetValue(DataGridColumn.OriginalValueProperty, value);
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00145F62 File Offset: 0x00144162
		private static void ClearOriginalValue(DependencyObject obj)
		{
			obj.ClearValue(DataGridColumn.OriginalValueProperty);
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x00145F6F File Offset: 0x0014416F
		internal static void OnNotifyCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.Columns);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x00145F7F File Offset: 0x0014417F
		private static void OnNotifyColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x00145F90 File Offset: 0x00144190
		private static void OnNotifyColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x00145FA0 File Offset: 0x001441A0
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyColumns(target))
			{
				target &= ~DataGridNotificationTarget.Columns;
				if (e.Property == DataGrid.MaxColumnWidthProperty || e.Property == DataGridColumn.MaxWidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.MaxWidthProperty);
				}
				else if (e.Property == DataGrid.MinColumnWidthProperty || e.Property == DataGridColumn.MinWidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.MinWidthProperty);
				}
				else if (e.Property == DataGrid.ColumnWidthProperty || e.Property == DataGridColumn.WidthProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.WidthProperty);
				}
				else if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGridColumn.HeaderStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.HeaderStyleProperty);
				}
				else if (e.Property == DataGrid.CellStyleProperty || e.Property == DataGridColumn.CellStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CellStyleProperty);
				}
				else if (e.Property == DataGrid.IsReadOnlyProperty || e.Property == DataGridColumn.IsReadOnlyProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.IsReadOnlyProperty);
				}
				else if (e.Property == DataGrid.DragIndicatorStyleProperty || e.Property == DataGridColumn.DragIndicatorStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.DragIndicatorStyleProperty);
				}
				else if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					base.CoerceValue(DataGridColumn.IsFrozenProperty);
				}
				else if (e.Property == DataGrid.CanUserSortColumnsProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserSortProperty);
				}
				else if (e.Property == DataGrid.CanUserResizeColumnsProperty || e.Property == DataGridColumn.CanUserResizeProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserResizeProperty);
				}
				else if (e.Property == DataGrid.CanUserReorderColumnsProperty || e.Property == DataGridColumn.CanUserReorderProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridColumn.CanUserReorderProperty);
				}
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.MinWidthProperty || e.Property == DataGridColumn.MaxWidthProperty)
				{
					base.CoerceValue(DataGridColumn.ActualWidthProperty);
				}
			}
			if (target != DataGridNotificationTarget.None)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)d;
				DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.NotifyPropertyChanged(d, e, target);
				}
			}
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> that contains this column that a column property has changed.</summary>
		/// <param name="propertyName">The name of the column property that changed.</param>
		// Token: 0x060047D8 RID: 18392 RVA: 0x001461CC File Offset: 0x001443CC
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (this.DataGridOwner != null)
			{
				this.DataGridOwner.NotifyPropertyChanged(this, propertyName, default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.RefreshCellContent);
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x001461FC File Offset: 0x001443FC
		internal static void NotifyPropertyChangeForRefreshContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(e.Property.Name);
		}

		/// <summary>When overridden in a derived class, updates the contents of a cell in the column in response to a column property value that changed.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that changed.</param>
		// Token: 0x060047DA RID: 18394 RVA: 0x00002137 File Offset: 0x00000337
		protected internal virtual void RefreshCellContent(FrameworkElement element, string propertyName)
		{
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x00146218 File Offset: 0x00144418
		internal void SyncProperties()
		{
			DataGridHelper.TransferProperty(this, DataGridColumn.MinWidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.MaxWidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.WidthProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.HeaderStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CellStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.IsReadOnlyProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.DragIndicatorStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserSortProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserReorderProperty);
			DataGridHelper.TransferProperty(this, DataGridColumn.CanUserResizeProperty);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.DataGrid" /> control that contains this column.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataGrid" /> control that contains this column.</returns>
		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x00146293 File Offset: 0x00144493
		// (set) Token: 0x060047DD RID: 18397 RVA: 0x0014629B File Offset: 0x0014449B
		protected internal DataGrid DataGridOwner
		{
			get
			{
				return this._dataGridOwner;
			}
			internal set
			{
				this._dataGridOwner = value;
			}
		}

		/// <summary>Gets or sets the display position of the column relative to the other columns in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The zero-based position of the column, as it is displayed in the associated <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is -1. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060047DE RID: 18398 RVA: 0x001462A4 File Offset: 0x001444A4
		// (set) Token: 0x060047DF RID: 18399 RVA: 0x001462B6 File Offset: 0x001444B6
		public int DisplayIndex
		{
			get
			{
				return (int)base.GetValue(DataGridColumn.DisplayIndexProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.DisplayIndexProperty, value);
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x001462CC File Offset: 0x001444CC
		private static object OnCoerceDisplayIndex(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			if (dataGridColumn.DataGridOwner != null)
			{
				dataGridColumn.DataGridOwner.ValidateDisplayIndex(dataGridColumn, (int)baseValue);
			}
			return baseValue;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x001462FB File Offset: 0x001444FB
		private static void DisplayIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
		}

		/// <summary>Gets or sets a property name, or a period-delimited hierarchy of property names, that indicates the member to sort by.</summary>
		/// <returns>The path of the data-item member to sort by. The registered default is an empty string (""). For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060047E2 RID: 18402 RVA: 0x0014630C File Offset: 0x0014450C
		// (set) Token: 0x060047E3 RID: 18403 RVA: 0x0014631E File Offset: 0x0014451E
		public string SortMemberPath
		{
			get
			{
				return (string)base.GetValue(DataGridColumn.SortMemberPathProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.SortMemberPathProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can sort the column by clicking the column header.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can sort the column; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060047E4 RID: 18404 RVA: 0x0014632C File Offset: 0x0014452C
		// (set) Token: 0x060047E5 RID: 18405 RVA: 0x0014633E File Offset: 0x0014453E
		public bool CanUserSort
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserSortProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserSortProperty, value);
			}
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x0014634C File Offset: 0x0014454C
		internal static object OnCoerceCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			bool flag;
			BaseValueSourceInternal valueSource = dataGridColumn.GetValueSource(DataGridColumn.CanUserSortProperty, null, out flag);
			if (dataGridColumn.DataGridOwner != null)
			{
				bool flag2;
				BaseValueSourceInternal valueSource2 = dataGridColumn.DataGridOwner.GetValueSource(DataGrid.CanUserSortColumnsProperty, null, out flag2);
				if (valueSource2 == valueSource && !flag && flag2)
				{
					return dataGridColumn.DataGridOwner.GetValue(DataGrid.CanUserSortColumnsProperty);
				}
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserSortProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserSortColumnsProperty);
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x001463C4 File Offset: 0x001445C4
		private static void OnCanUserSortPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!DataGridHelper.IsPropertyTransferEnabled(d, DataGridColumn.CanUserSortProperty))
			{
				DataGridHelper.TransferProperty(d, DataGridColumn.CanUserSortProperty);
			}
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		/// <summary>Gets or sets the sort direction (ascending or descending) of the column.</summary>
		/// <returns>A value that represents the direction for sorting. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x060047E8 RID: 18408 RVA: 0x001463ED File Offset: 0x001445ED
		// (set) Token: 0x060047E9 RID: 18409 RVA: 0x001463FF File Offset: 0x001445FF
		public ListSortDirection? SortDirection
		{
			get
			{
				return (ListSortDirection?)base.GetValue(DataGridColumn.SortDirectionProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.SortDirectionProperty, value);
			}
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x00146412 File Offset: 0x00144612
		private static void OnNotifySortPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		/// <summary>Gets a value that indicates whether the column is auto-generated.</summary>
		/// <returns>
		///     <see langword="true" /> if the column is auto-generated; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060047EB RID: 18411 RVA: 0x00146423 File Offset: 0x00144623
		// (set) Token: 0x060047EC RID: 18412 RVA: 0x00146435 File Offset: 0x00144635
		public bool IsAutoGenerated
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsAutoGeneratedProperty);
			}
			internal set
			{
				base.SetValue(DataGridColumn.IsAutoGeneratedPropertyKey, value);
			}
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x00146444 File Offset: 0x00144644
		internal static DataGridColumn CreateDefaultColumn(ItemPropertyInfo itemProperty)
		{
			DataGridComboBoxColumn dataGridComboBoxColumn = null;
			Type propertyType = itemProperty.PropertyType;
			DataGridColumn dataGridColumn;
			if (propertyType.IsEnum)
			{
				dataGridComboBoxColumn = new DataGridComboBoxColumn();
				dataGridComboBoxColumn.ItemsSource = Enum.GetValues(propertyType);
				dataGridColumn = dataGridComboBoxColumn;
			}
			else if (typeof(string).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridTextColumn();
			}
			else if (typeof(bool).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridCheckBoxColumn();
			}
			else if (typeof(Uri).IsAssignableFrom(propertyType))
			{
				dataGridColumn = new DataGridHyperlinkColumn();
			}
			else
			{
				dataGridColumn = new DataGridTextColumn();
			}
			if (!typeof(IComparable).IsAssignableFrom(propertyType))
			{
				dataGridColumn.CanUserSort = false;
			}
			dataGridColumn.Header = itemProperty.Name;
			DataGridBoundColumn dataGridBoundColumn = dataGridColumn as DataGridBoundColumn;
			if (dataGridBoundColumn != null || dataGridComboBoxColumn != null)
			{
				Binding binding = new Binding(itemProperty.Name);
				if (dataGridComboBoxColumn != null)
				{
					dataGridComboBoxColumn.SelectedItemBinding = binding;
				}
				else
				{
					dataGridBoundColumn.Binding = binding;
				}
				PropertyDescriptor propertyDescriptor = itemProperty.Descriptor as PropertyDescriptor;
				if (propertyDescriptor != null)
				{
					if (propertyDescriptor.IsReadOnly)
					{
						binding.Mode = BindingMode.OneWay;
						dataGridColumn.IsReadOnly = true;
					}
				}
				else
				{
					PropertyInfo propertyInfo = itemProperty.Descriptor as PropertyInfo;
					if (propertyInfo != null && !propertyInfo.CanWrite)
					{
						binding.Mode = BindingMode.OneWay;
						dataGridColumn.IsReadOnly = true;
					}
				}
			}
			return dataGridColumn;
		}

		/// <summary>Gets a value that indicates whether the column is prevented from scrolling horizontally.</summary>
		/// <returns>
		///     <see langword="true" /> if the column cannot be scrolled horizontally; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060047EE RID: 18414 RVA: 0x0014657D File Offset: 0x0014477D
		// (set) Token: 0x060047EF RID: 18415 RVA: 0x0014658F File Offset: 0x0014478F
		public bool IsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.IsFrozenProperty);
			}
			internal set
			{
				base.SetValue(DataGridColumn.IsFrozenPropertyKey, value);
			}
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00146412 File Offset: 0x00144612
		private static void OnNotifyFrozenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumn)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x001465A0 File Offset: 0x001447A0
		private static object OnCoerceIsFrozen(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)d;
			DataGrid dataGridOwner = dataGridColumn.DataGridOwner;
			if (dataGridOwner == null)
			{
				return baseValue;
			}
			if (dataGridColumn.DisplayIndex < dataGridOwner.FrozenColumnCount)
			{
				return true;
			}
			return false;
		}

		/// <summary>Gets or sets a value that indicates whether the user can change the column display position by dragging the column header.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can drag the column header to a new position; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060047F2 RID: 18418 RVA: 0x001465DB File Offset: 0x001447DB
		// (set) Token: 0x060047F3 RID: 18419 RVA: 0x001465ED File Offset: 0x001447ED
		public bool CanUserReorder
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserReorderProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserReorderProperty, value);
			}
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x001465FC File Offset: 0x001447FC
		private static object OnCoerceCanUserReorder(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserReorderProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserReorderColumnsProperty);
		}

		/// <summary>Gets or sets the style object to apply to the column header during a drag operation.</summary>
		/// <returns>The style object to apply to the column header during a drag operation. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060047F5 RID: 18421 RVA: 0x00146627 File Offset: 0x00144827
		// (set) Token: 0x060047F6 RID: 18422 RVA: 0x00146639 File Offset: 0x00144839
		public Style DragIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridColumn.DragIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.DragIndicatorStyleProperty, value);
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x00146648 File Offset: 0x00144848
		private static object OnCoerceDragIndicatorStyle(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.DragIndicatorStyleProperty, dataGridColumn.DataGridOwner, DataGrid.DragIndicatorStyleProperty);
		}

		/// <summary>Gets or sets the binding object to use when getting or setting cell content for the clipboard.</summary>
		/// <returns>An object that represents the binding.</returns>
		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060047F8 RID: 18424 RVA: 0x00146673 File Offset: 0x00144873
		// (set) Token: 0x060047F9 RID: 18425 RVA: 0x0014667B File Offset: 0x0014487B
		public virtual BindingBase ClipboardContentBinding
		{
			get
			{
				return this._clipboardContentBinding;
			}
			set
			{
				this._clipboardContentBinding = value;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridColumn.CopyingCellClipboardContent" /> event.</summary>
		/// <param name="item">The data context for the selected element.</param>
		/// <returns>An object that represents the content of the cell.</returns>
		// Token: 0x060047FA RID: 18426 RVA: 0x00146684 File Offset: 0x00144884
		public virtual object OnCopyingCellClipboardContent(object item)
		{
			object obj = this.DataGridOwner.GetCellClipboardValue(item, this);
			if (this.CopyingCellClipboardContent != null)
			{
				DataGridCellClipboardEventArgs dataGridCellClipboardEventArgs = new DataGridCellClipboardEventArgs(item, this, obj);
				this.CopyingCellClipboardContent(this, dataGridCellClipboardEventArgs);
				obj = dataGridCellClipboardEventArgs.Content;
			}
			return obj;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridColumn.PastingCellClipboardContent" /> event.</summary>
		/// <param name="item">The data context for the selected element.</param>
		/// <param name="cellContent">The content to paste into the cell.</param>
		// Token: 0x060047FB RID: 18427 RVA: 0x001466C8 File Offset: 0x001448C8
		public virtual void OnPastingCellClipboardContent(object item, object cellContent)
		{
			BindingBase clipboardContentBinding = this.ClipboardContentBinding;
			if (clipboardContentBinding != null)
			{
				if (this.PastingCellClipboardContent != null)
				{
					DataGridCellClipboardEventArgs dataGridCellClipboardEventArgs = new DataGridCellClipboardEventArgs(item, this, cellContent);
					this.PastingCellClipboardContent(this, dataGridCellClipboardEventArgs);
					cellContent = dataGridCellClipboardEventArgs.Content;
				}
				if (cellContent != null)
				{
					this.DataGridOwner.SetCellClipboardValue(item, this, cellContent);
				}
			}
		}

		/// <summary>Occurs after the cell clipboard content is prepared.</summary>
		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x060047FC RID: 18428 RVA: 0x00146718 File Offset: 0x00144918
		// (remove) Token: 0x060047FD RID: 18429 RVA: 0x00146750 File Offset: 0x00144950
		public event EventHandler<DataGridCellClipboardEventArgs> CopyingCellClipboardContent;

		/// <summary>Occurs before the clipboard content is moved to the cell.</summary>
		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x060047FE RID: 18430 RVA: 0x00146788 File Offset: 0x00144988
		// (remove) Token: 0x060047FF RID: 18431 RVA: 0x001467C0 File Offset: 0x001449C0
		public event EventHandler<DataGridCellClipboardEventArgs> PastingCellClipboardContent;

		// Token: 0x06004800 RID: 18432 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnInput(InputEventArgs e)
		{
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x001467F8 File Offset: 0x001449F8
		internal void BeginEdit(InputEventArgs e, bool handled)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.BeginEdit(e))
			{
				e.Handled = (e.Handled || handled);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can adjust the column width by using the mouse.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can resize the column; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x06004802 RID: 18434 RVA: 0x00146826 File Offset: 0x00144A26
		// (set) Token: 0x06004803 RID: 18435 RVA: 0x00146838 File Offset: 0x00144A38
		public bool CanUserResize
		{
			get
			{
				return (bool)base.GetValue(DataGridColumn.CanUserResizeProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.CanUserResizeProperty, value);
			}
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x00146848 File Offset: 0x00144A48
		private static object OnCoerceCanUserResize(DependencyObject d, object baseValue)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumn, baseValue, DataGridColumn.CanUserResizeProperty, dataGridColumn.DataGridOwner, DataGrid.CanUserResizeColumnsProperty);
		}

		/// <summary>Gets or sets the visibility of the column.</summary>
		/// <returns>An enumeration value that specifies the column visibility. The registered default is <see cref="F:System.Windows.Visibility.Visible" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x06004805 RID: 18437 RVA: 0x00146873 File Offset: 0x00144A73
		// (set) Token: 0x06004806 RID: 18438 RVA: 0x00146885 File Offset: 0x00144A85
		public Visibility Visibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridColumn.VisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridColumn.VisibilityProperty, value);
			}
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x00146898 File Offset: 0x00144A98
		private static void OnVisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			Visibility visibility = (Visibility)eventArgs.OldValue;
			Visibility visibility2 = (Visibility)eventArgs.NewValue;
			if (visibility != Visibility.Visible && visibility2 != Visibility.Visible)
			{
				return;
			}
			((DataGridColumn)d).NotifyPropertyChanged(d, eventArgs, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.ColumnHeadersPresenter | DataGridNotificationTarget.DataGrid);
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06004808 RID: 18440 RVA: 0x001468D5 File Offset: 0x00144AD5
		internal bool IsVisible
		{
			get
			{
				return this.Visibility == Visibility.Visible;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.Header" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.Header" /> dependency property.</returns>
		// Token: 0x04002969 RID: 10601
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderStyle" /> dependency property.</returns>
		// Token: 0x0400296A RID: 10602
		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceHeaderStyle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderStringFormat" /> dependency property.</returns>
		// Token: 0x0400296B RID: 10603
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderTemplate" /> dependency property.</returns>
		// Token: 0x0400296C RID: 10604
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.HeaderTemplateSelector" /> dependency property.</returns>
		// Token: 0x0400296D RID: 10605
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.CellStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.CellStyle" /> dependency property.</returns>
		// Token: 0x0400296E RID: 10606
		public static readonly DependencyProperty CellStyleProperty = DependencyProperty.Register("CellStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyCellPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCellStyle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.IsReadOnly" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.IsReadOnly" /> dependency property.</returns>
		// Token: 0x0400296F RID: 10607
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.OnNotifyCellPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceIsReadOnly)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.Width" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.Width" /> dependency property.</returns>
		// Token: 0x04002970 RID: 10608
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(DataGridLength), typeof(DataGridColumn), new FrameworkPropertyMetadata(DataGridLength.Auto, new PropertyChangedCallback(DataGridColumn.OnWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceWidth)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.MinWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.MinWidth" /> dependency property.</returns>
		// Token: 0x04002971 RID: 10609
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(DataGridColumn.OnMinWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceMinWidth)), new ValidateValueCallback(DataGridColumn.ValidateMinWidth));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.MaxWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.MaxWidth" /> dependency property.</returns>
		// Token: 0x04002972 RID: 10610
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DataGridColumn.OnMaxWidthPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceMaxWidth)), new ValidateValueCallback(DataGridColumn.ValidateMaxWidth));

		// Token: 0x04002973 RID: 10611
		private static readonly DependencyPropertyKey ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), typeof(DataGridColumn), new FrameworkPropertyMetadata(0.0, null, new CoerceValueCallback(DataGridColumn.OnCoerceActualWidth)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.ActualWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.ActualWidth" /> dependency property.</returns>
		// Token: 0x04002974 RID: 10612
		public static readonly DependencyProperty ActualWidthProperty = DataGridColumn.ActualWidthPropertyKey.DependencyProperty;

		// Token: 0x04002975 RID: 10613
		private static readonly DependencyProperty OriginalValueProperty = DependencyProperty.RegisterAttached("OriginalValue", typeof(object), typeof(DataGridColumn), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> dependency property.</returns>
		// Token: 0x04002976 RID: 10614
		public static readonly DependencyProperty DisplayIndexProperty = DependencyProperty.Register("DisplayIndex", typeof(int), typeof(DataGridColumn), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(DataGridColumn.DisplayIndexChanged), new CoerceValueCallback(DataGridColumn.OnCoerceDisplayIndex)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.SortMemberPath" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.SortMemberPath" /> dependency property.</returns>
		// Token: 0x04002977 RID: 10615
		public static readonly DependencyProperty SortMemberPathProperty = DependencyProperty.Register("SortMemberPath", typeof(string), typeof(DataGridColumn), new FrameworkPropertyMetadata(string.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserSort" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserSort" /> dependency property.</returns>
		// Token: 0x04002978 RID: 10616
		public static readonly DependencyProperty CanUserSortProperty = DependencyProperty.Register("CanUserSort", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnCanUserSortPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserSort)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.SortDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.SortDirection" /> dependency property.</returns>
		// Token: 0x04002979 RID: 10617
		public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register("SortDirection", typeof(ListSortDirection?), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifySortPropertyChanged)));

		// Token: 0x0400297A RID: 10618
		private static readonly DependencyPropertyKey IsAutoGeneratedPropertyKey = DependencyProperty.RegisterReadOnly("IsAutoGenerated", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.IsAutoGenerated" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.IsAutoGenerated" /> dependency property.</returns>
		// Token: 0x0400297B RID: 10619
		public static readonly DependencyProperty IsAutoGeneratedProperty = DataGridColumn.IsAutoGeneratedPropertyKey.DependencyProperty;

		// Token: 0x0400297C RID: 10620
		private static readonly DependencyPropertyKey IsFrozenPropertyKey = DependencyProperty.RegisterReadOnly("IsFrozen", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.OnNotifyFrozenPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceIsFrozen)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.IsFrozen" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.IsFrozen" /> dependency property.</returns>
		// Token: 0x0400297D RID: 10621
		public static readonly DependencyProperty IsFrozenProperty = DataGridColumn.IsFrozenPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserReorder" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserReorder" /> dependency property.</returns>
		// Token: 0x0400297E RID: 10622
		public static readonly DependencyProperty CanUserReorderProperty = DependencyProperty.Register("CanUserReorder", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserReorder)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.DragIndicatorStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.DragIndicatorStyle" /> dependency property.</returns>
		// Token: 0x0400297F RID: 10623
		public static readonly DependencyProperty DragIndicatorStyleProperty = DependencyProperty.Register("DragIndicatorStyle", typeof(Style), typeof(DataGridColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceDragIndicatorStyle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserResize" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.CanUserResize" /> dependency property.</returns>
		// Token: 0x04002982 RID: 10626
		public static readonly DependencyProperty CanUserResizeProperty = DependencyProperty.Register("CanUserResize", typeof(bool), typeof(DataGridColumn), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGridColumn.OnNotifyColumnHeaderPropertyChanged), new CoerceValueCallback(DataGridColumn.OnCoerceCanUserResize)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridColumn.Visibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridColumn.Visibility" /> dependency property.</returns>
		// Token: 0x04002983 RID: 10627
		public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(DataGridColumn), new FrameworkPropertyMetadata(Visibility.Visible, new PropertyChangedCallback(DataGridColumn.OnVisibilityPropertyChanged)));

		// Token: 0x04002984 RID: 10628
		private DataGrid _dataGridOwner;

		// Token: 0x04002985 RID: 10629
		private BindingBase _clipboardContentBinding;

		// Token: 0x04002986 RID: 10630
		private bool _ignoreRedistributionOnWidthChange;

		// Token: 0x04002987 RID: 10631
		private bool _processingWidthChange;

		// Token: 0x04002988 RID: 10632
		private const double _starMaxWidth = 10000.0;
	}
}
