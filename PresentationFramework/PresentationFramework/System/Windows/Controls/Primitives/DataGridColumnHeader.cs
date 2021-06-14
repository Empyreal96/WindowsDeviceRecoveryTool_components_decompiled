using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an individual <see cref="T:System.Windows.Controls.DataGrid" /> column header.</summary>
	// Token: 0x0200057D RID: 1405
	[TemplatePart(Name = "PART_LeftHeaderGripper", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_RightHeaderGripper", Type = typeof(Thumb))]
	public class DataGridColumnHeader : ButtonBase, IProvideDataGridColumn
	{
		// Token: 0x06005CB4 RID: 23732 RVA: 0x001A1408 File Offset: 0x0019F608
		static DataGridColumnHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(typeof(DataGridColumnHeader)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContent)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContentTemplate)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceContentTemplateSelector)));
			ContentControl.ContentStringFormatProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceStringFormat)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceStyle)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceHeight)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(false));
			UIElement.ClipProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceClip)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeader" /> class. </summary>
		// Token: 0x06005CB5 RID: 23733 RVA: 0x001A1756 File Offset: 0x0019F956
		public DataGridColumnHeader()
		{
			this._tracker = new ContainerTracking<DataGridColumnHeader>(this);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.DataGridColumn" /> associated with this column header.</summary>
		/// <returns>The column associated with this column header.</returns>
		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06005CB6 RID: 23734 RVA: 0x001A176A File Offset: 0x0019F96A
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to paint the column header separator lines. </summary>
		/// <returns>The brush used to paint column header separator lines. </returns>
		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06005CB7 RID: 23735 RVA: 0x001A1772 File Offset: 0x0019F972
		// (set) Token: 0x06005CB8 RID: 23736 RVA: 0x001A1784 File Offset: 0x0019F984
		public Brush SeparatorBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridColumnHeader.SeparatorBrushProperty);
			}
			set
			{
				base.SetValue(DataGridColumnHeader.SeparatorBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the user interface (UI) visibility of the column header separator lines. </summary>
		/// <returns>The UI visibility of the column header separator lines. The default is <see cref="F:System.Windows.Visibility.Visible" />.</returns>
		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x001A1792 File Offset: 0x0019F992
		// (set) Token: 0x06005CBA RID: 23738 RVA: 0x001A17A4 File Offset: 0x0019F9A4
		public Visibility SeparatorVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridColumnHeader.SeparatorVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridColumnHeader.SeparatorVisibilityProperty, value);
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x001A17B8 File Offset: 0x0019F9B8
		internal void PrepareColumnHeader(object item, DataGridColumn column)
		{
			this._column = column;
			base.TabIndex = column.DisplayIndex;
			DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentStringFormatProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
			base.CoerceValue(DataGridColumnHeader.CanUserSortProperty);
			base.CoerceValue(DataGridColumnHeader.SortDirectionProperty);
			base.CoerceValue(DataGridColumnHeader.IsFrozenProperty);
			base.CoerceValue(UIElement.ClipProperty);
			base.CoerceValue(DataGridColumnHeader.DisplayIndexProperty);
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x001A1851 File Offset: 0x0019FA51
		internal void ClearHeader()
		{
			this._column = null;
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06005CBD RID: 23741 RVA: 0x001A185A File Offset: 0x0019FA5A
		internal ContainerTracking<DataGridColumnHeader> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		/// <summary>Gets the display position of the column associated with this column header relative to the other columns in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The display position of associated column relative to the other columns in the <see cref="T:System.Windows.Controls.DataGrid" />.</returns>
		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06005CBE RID: 23742 RVA: 0x001A1862 File Offset: 0x0019FA62
		public int DisplayIndex
		{
			get
			{
				return (int)base.GetValue(DataGridColumnHeader.DisplayIndexProperty);
			}
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x001A1874 File Offset: 0x0019FA74
		private static object OnCoerceDisplayIndex(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				return column.DisplayIndex;
			}
			return -1;
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x001A18A4 File Offset: 0x0019FAA4
		private static void OnDisplayIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				DataGrid dataGridOwner = column.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridColumnHeader.SetLeftGripperVisibility();
					DataGridColumnHeader dataGridColumnHeader2 = dataGridOwner.ColumnHeaderFromDisplayIndex(dataGridColumnHeader.DisplayIndex + 1);
					if (dataGridColumnHeader2 != null)
					{
						dataGridColumnHeader2.SetLeftGripperVisibility(column.CanUserResize);
					}
				}
			}
		}

		/// <summary>Builds the visual tree for the column header when a new template is applied. </summary>
		// Token: 0x06005CC1 RID: 23745 RVA: 0x001A18F0 File Offset: 0x0019FAF0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.HookupGripperEvents();
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x001A1900 File Offset: 0x0019FB00
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._leftGripper = (base.GetTemplateChild("PART_LeftHeaderGripper") as Thumb);
			this._rightGripper = (base.GetTemplateChild("PART_RightHeaderGripper") as Thumb);
			if (this._leftGripper != null)
			{
				this._leftGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._leftGripper.DragDelta += this.OnColumnHeaderResize;
				this._leftGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._leftGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetLeftGripperVisibility();
			}
			if (this._rightGripper != null)
			{
				this._rightGripper.DragStarted += this.OnColumnHeaderGripperDragStarted;
				this._rightGripper.DragDelta += this.OnColumnHeaderResize;
				this._rightGripper.DragCompleted += this.OnColumnHeaderGripperDragCompleted;
				this._rightGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x001A1A14 File Offset: 0x0019FC14
		private void UnhookGripperEvents()
		{
			if (this._leftGripper != null)
			{
				this._leftGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._leftGripper.DragDelta -= this.OnColumnHeaderResize;
				this._leftGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._leftGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._leftGripper = null;
			}
			if (this._rightGripper != null)
			{
				this._rightGripper.DragStarted -= this.OnColumnHeaderGripperDragStarted;
				this._rightGripper.DragDelta -= this.OnColumnHeaderResize;
				this._rightGripper.DragCompleted -= this.OnColumnHeaderGripperDragCompleted;
				this._rightGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._rightGripper = null;
			}
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x001A1AF7 File Offset: 0x0019FCF7
		private DataGridColumnHeader HeaderToResize(object gripper)
		{
			if (gripper != this._rightGripper)
			{
				return this.PreviousVisibleHeader;
			}
			return this;
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x001A1B0C File Offset: 0x0019FD0C
		private void OnColumnHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				if (dataGridColumnHeader.Column != null)
				{
					DataGrid dataGridOwner = dataGridColumnHeader.Column.DataGridOwner;
					if (dataGridOwner != null)
					{
						dataGridOwner.InternalColumns.OnColumnResizeStarted();
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x001A1B50 File Offset: 0x0019FD50
		private void OnColumnHeaderResize(object sender, DragDeltaEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				DataGridColumnHeader.RecomputeColumnWidthsOnColumnResize(dataGridColumnHeader, e.HorizontalChange);
				e.Handled = true;
			}
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x001A1B7C File Offset: 0x0019FD7C
		private static void RecomputeColumnWidthsOnColumnResize(DataGridColumnHeader header, double horizontalChange)
		{
			DataGridColumn column = header.Column;
			if (column == null)
			{
				return;
			}
			DataGrid dataGridOwner = column.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			dataGridOwner.InternalColumns.RecomputeColumnWidthsOnColumnResize(column, horizontalChange, false);
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x001A1BB0 File Offset: 0x0019FDB0
		private void OnColumnHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null)
			{
				if (dataGridColumnHeader.Column != null)
				{
					DataGrid dataGridOwner = dataGridColumnHeader.Column.DataGridOwner;
					if (dataGridOwner != null)
					{
						dataGridOwner.InternalColumns.OnColumnResizeCompleted(e.Canceled);
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x001A1BF8 File Offset: 0x0019FDF8
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			DataGridColumnHeader dataGridColumnHeader = this.HeaderToResize(sender);
			if (dataGridColumnHeader != null && dataGridColumnHeader.Column != null)
			{
				dataGridColumnHeader.Column.Width = DataGridLength.Auto;
				e.Handled = true;
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06005CCA RID: 23754 RVA: 0x001A1C2F File Offset: 0x0019FE2F
		private DataGridLength ColumnWidth
		{
			get
			{
				if (this.Column == null)
				{
					return DataGridLength.Auto;
				}
				return this.Column.Width;
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06005CCB RID: 23755 RVA: 0x001A1C4A File Offset: 0x0019FE4A
		private double ColumnActualWidth
		{
			get
			{
				if (this.Column == null)
				{
					return base.ActualWidth;
				}
				return this.Column.ActualWidth;
			}
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x001A1C66 File Offset: 0x0019FE66
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridColumnHeader)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x001A1C78 File Offset: 0x0019FE78
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (dataGridColumn != null && dataGridColumn != this.Column)
			{
				return;
			}
			if (e.Property == DataGridColumn.WidthProperty)
			{
				DataGridHelper.OnColumnWidthChanged(this, e);
				return;
			}
			if (e.Property == DataGridColumn.HeaderProperty || e.Property == ContentControl.ContentProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderTemplateProperty || e.Property == ContentControl.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderTemplateSelectorProperty || e.Property == ContentControl.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
				return;
			}
			if (e.Property == DataGridColumn.HeaderStringFormatProperty || e.Property == ContentControl.ContentStringFormatProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentStringFormatProperty);
				return;
			}
			if (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGridColumn.HeaderStyleProperty || e.Property == FrameworkElement.StyleProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				return;
			}
			if (e.Property == DataGrid.ColumnHeaderHeightProperty || e.Property == FrameworkElement.HeightProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
				return;
			}
			if (e.Property == DataGridColumn.DisplayIndexProperty)
			{
				base.CoerceValue(DataGridColumnHeader.DisplayIndexProperty);
				base.TabIndex = dataGridColumn.DisplayIndex;
				return;
			}
			if (e.Property == DataGrid.CanUserResizeColumnsProperty)
			{
				this.OnCanUserResizeColumnsChanged();
				return;
			}
			if (e.Property == DataGridColumn.CanUserSortProperty)
			{
				base.CoerceValue(DataGridColumnHeader.CanUserSortProperty);
				return;
			}
			if (e.Property == DataGridColumn.SortDirectionProperty)
			{
				base.CoerceValue(DataGridColumnHeader.SortDirectionProperty);
				return;
			}
			if (e.Property == DataGridColumn.IsFrozenProperty)
			{
				base.CoerceValue(DataGridColumnHeader.IsFrozenProperty);
				return;
			}
			if (e.Property == DataGridColumn.CanUserResizeProperty)
			{
				this.OnCanUserResizeChanged();
				return;
			}
			if (e.Property == DataGridColumn.VisibilityProperty)
			{
				this.OnColumnVisibilityChanged(e);
			}
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x001A1E60 File Offset: 0x001A0060
		private void OnCanUserResizeColumnsChanged()
		{
			if (this.Column.DataGridOwner != null)
			{
				this.SetLeftGripperVisibility();
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x001A1E7C File Offset: 0x001A007C
		private void OnCanUserResizeChanged()
		{
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null)
			{
				this.SetNextHeaderLeftGripperVisibility(this.Column.CanUserResize);
				this.SetRightGripperVisibility();
			}
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x001A1EB0 File Offset: 0x001A00B0
		private void SetLeftGripperVisibility()
		{
			if (this._leftGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			bool leftGripperVisibility = false;
			for (int i = this.DisplayIndex - 1; i >= 0; i--)
			{
				DataGridColumn dataGridColumn = dataGridOwner.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					leftGripperVisibility = dataGridColumn.CanUserResize;
					break;
				}
			}
			this.SetLeftGripperVisibility(leftGripperVisibility);
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x001A1F10 File Offset: 0x001A0110
		private void SetLeftGripperVisibility(bool canPreviousColumnResize)
		{
			if (this._leftGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.CanUserResizeColumns && canPreviousColumnResize)
			{
				this._leftGripper.Visibility = Visibility.Visible;
				return;
			}
			this._leftGripper.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x001A1F64 File Offset: 0x001A0164
		private void SetRightGripperVisibility()
		{
			if (this._rightGripper == null || this.Column == null)
			{
				return;
			}
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null && dataGridOwner.CanUserResizeColumns && this.Column.CanUserResize)
			{
				this._rightGripper.Visibility = Visibility.Visible;
				return;
			}
			this._rightGripper.Visibility = Visibility.Collapsed;
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x001A1FC0 File Offset: 0x001A01C0
		private void SetNextHeaderLeftGripperVisibility(bool canUserResize)
		{
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			int count = dataGridOwner.Columns.Count;
			int i = this.DisplayIndex + 1;
			while (i < count)
			{
				if (dataGridOwner.ColumnFromDisplayIndex(i).IsVisible)
				{
					DataGridColumnHeader dataGridColumnHeader = dataGridOwner.ColumnHeaderFromDisplayIndex(i);
					if (dataGridColumnHeader != null)
					{
						dataGridColumnHeader.SetLeftGripperVisibility(canUserResize);
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x001A201C File Offset: 0x001A021C
		private void OnColumnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGridOwner = this.Column.DataGridOwner;
			if (dataGridOwner != null)
			{
				bool flag = (Visibility)e.OldValue == Visibility.Visible;
				bool flag2 = (Visibility)e.NewValue == Visibility.Visible;
				if (flag != flag2)
				{
					if (flag2)
					{
						this.SetLeftGripperVisibility();
						this.SetRightGripperVisibility();
						this.SetNextHeaderLeftGripperVisibility(this.Column.CanUserResize);
						return;
					}
					bool nextHeaderLeftGripperVisibility = false;
					for (int i = this.DisplayIndex - 1; i >= 0; i--)
					{
						DataGridColumn dataGridColumn = dataGridOwner.ColumnFromDisplayIndex(i);
						if (dataGridColumn.IsVisible)
						{
							nextHeaderLeftGripperVisibility = dataGridColumn.CanUserResize;
							break;
						}
					}
					this.SetNextHeaderLeftGripperVisibility(nextHeaderLeftGripperVisibility);
				}
			}
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x001A20C0 File Offset: 0x001A02C0
		private static object OnCoerceContent(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			object coercedTransferPropertyValue = DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderProperty);
			FrameworkObject frameworkObject = new FrameworkObject(coercedTransferPropertyValue as DependencyObject);
			if (frameworkObject.Parent != null && frameworkObject.Parent != dataGridColumnHeader)
			{
				frameworkObject.ChangeLogicalParent(null);
			}
			return coercedTransferPropertyValue;
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x001A2118 File Offset: 0x001A0318
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentTemplateProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderTemplateProperty);
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x001A2144 File Offset: 0x001A0344
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentTemplateSelectorProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderTemplateSelectorProperty);
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001A2170 File Offset: 0x001A0370
		private static object OnCoerceStringFormat(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = d as DataGridColumnHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, ContentControl.ContentStringFormatProperty, dataGridColumnHeader.Column, DataGridColumn.HeaderStringFormatProperty);
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x001A219C File Offset: 0x001A039C
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			DataGrid grandParentObject = null;
			if (column == null)
			{
				DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = dataGridColumnHeader.TemplatedParent as DataGridColumnHeadersPresenter;
				if (dataGridColumnHeadersPresenter != null)
				{
					grandParentObject = dataGridColumnHeadersPresenter.ParentDataGrid;
				}
			}
			else
			{
				grandParentObject = column.DataGridOwner;
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, FrameworkElement.StyleProperty, column, DataGridColumn.HeaderStyleProperty, grandParentObject, DataGrid.ColumnHeaderStyleProperty);
		}

		/// <summary>Gets a value that indicates whether the user can click this column header to sort the <see cref="T:System.Windows.Controls.DataGrid" /> by the associated column.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can click this column header to sort the <see cref="T:System.Windows.Controls.DataGrid" /> by the associated column; otherwise, <see langword="false" />. </returns>
		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x06005CDA RID: 23770 RVA: 0x001A21F3 File Offset: 0x001A03F3
		public bool CanUserSort
		{
			get
			{
				return (bool)base.GetValue(DataGridColumnHeader.CanUserSortProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DataGrid" /> is sorted by the associated column and whether the column values are in ascending or descending order.</summary>
		/// <returns>The sort direction of the column or <see langword="null" /> if unsorted.</returns>
		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06005CDB RID: 23771 RVA: 0x001A2205 File Offset: 0x001A0405
		public ListSortDirection? SortDirection
		{
			get
			{
				return (ListSortDirection?)base.GetValue(DataGridColumnHeader.SortDirectionProperty);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event and initiates sorting. </summary>
		// Token: 0x06005CDC RID: 23772 RVA: 0x001A2218 File Offset: 0x001A0418
		protected override void OnClick()
		{
			if (!this.SuppressClickEvent)
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
					if (automationPeer != null)
					{
						automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
					}
				}
				base.OnClick();
				if (this.Column != null && this.Column.DataGridOwner != null)
				{
					this.Column.DataGridOwner.PerformSort(this.Column);
				}
			}
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x001A2278 File Offset: 0x001A0478
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			DataGrid parentObject = null;
			if (column == null)
			{
				DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = dataGridColumnHeader.TemplatedParent as DataGridColumnHeadersPresenter;
				if (dataGridColumnHeadersPresenter != null)
				{
					parentObject = dataGridColumnHeadersPresenter.ParentDataGrid;
				}
			}
			else
			{
				parentObject = column.DataGridOwner;
			}
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeader, baseValue, FrameworkElement.HeightProperty, parentObject, DataGrid.ColumnHeaderHeightProperty);
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x001A22CC File Offset: 0x001A04CC
		private static object OnCoerceCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				return column.CanUserSort;
			}
			return baseValue;
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x001A22F8 File Offset: 0x001A04F8
		private static object OnCoerceSortDirection(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				return column.SortDirection;
			}
			return baseValue;
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderAutomationPeer" /> for this column header.</summary>
		/// <returns>A new automation peer for this column header.</returns>
		// Token: 0x06005CE0 RID: 23776 RVA: 0x001A2323 File Offset: 0x001A0523
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridColumnHeaderAutomationPeer(this);
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x0019E20B File Offset: 0x0019C40B
		internal void Invoke()
		{
			this.OnClick();
		}

		/// <summary>Gets a value that indicates whether the column associated with this column header is prevented from scrolling horizontally.</summary>
		/// <returns>
		///     <see langword="true" /> if the associated column is prevented from scrolling horizontally; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x06005CE2 RID: 23778 RVA: 0x001A232B File Offset: 0x001A052B
		public bool IsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGridColumnHeader.IsFrozenProperty);
			}
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x001A2340 File Offset: 0x001A0540
		private static object OnCoerceIsFrozen(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader dataGridColumnHeader = (DataGridColumnHeader)d;
			DataGridColumn column = dataGridColumnHeader.Column;
			if (column != null)
			{
				return column.IsFrozen;
			}
			return baseValue;
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x001A236C File Offset: 0x001A056C
		private static object OnCoerceClip(DependencyObject d, object baseValue)
		{
			DataGridColumnHeader cell = (DataGridColumnHeader)d;
			Geometry geometry = baseValue as Geometry;
			Geometry frozenClipForCell = DataGridHelper.GetFrozenClipForCell(cell);
			if (frozenClipForCell != null)
			{
				if (geometry == null)
				{
					return frozenClipForCell;
				}
				geometry = new CombinedGeometry(GeometryCombineMode.Intersect, geometry, frozenClipForCell);
			}
			return geometry;
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06005CE5 RID: 23781 RVA: 0x001A23A0 File Offset: 0x001A05A0
		internal DataGridColumnHeadersPresenter ParentPresenter
		{
			get
			{
				if (this._parentPresenter == null)
				{
					this._parentPresenter = (ItemsControl.ItemsControlFromItemContainer(this) as DataGridColumnHeadersPresenter);
				}
				return this._parentPresenter;
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06005CE6 RID: 23782 RVA: 0x001A23C1 File Offset: 0x001A05C1
		// (set) Token: 0x06005CE7 RID: 23783 RVA: 0x001A23C9 File Offset: 0x001A05C9
		internal bool SuppressClickEvent
		{
			get
			{
				return this._suppressClickEvent;
			}
			set
			{
				this._suppressClickEvent = value;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event that occurs when the left mouse button is pressed while the mouse pointer is over this control.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005CE8 RID: 23784 RVA: 0x001A23D4 File Offset: 0x001A05D4
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				if (base.ClickMode == ClickMode.Hover && e.ButtonState == MouseButtonState.Pressed)
				{
					base.CaptureMouse();
				}
				parentPresenter.OnHeaderMouseLeftButtonDown(e);
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> routed event that occurs when the mouse pointer moves while over this element.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005CE9 RID: 23785 RVA: 0x001A241C File Offset: 0x001A061C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				parentPresenter.OnHeaderMouseMove(e);
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> routed event that occurs when the left mouse button is released while the mouse pointer is over this control.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005CEA RID: 23786 RVA: 0x001A2448 File Offset: 0x001A0648
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				if (base.ClickMode == ClickMode.Hover && base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				parentPresenter.OnHeaderMouseLeftButtonUp(e);
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when this control is no longer receiving mouse event messages.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.LostMouseCapture" /> event.</param>
		// Token: 0x06005CEB RID: 23787 RVA: 0x001A248C File Offset: 0x001A068C
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			DataGridColumnHeadersPresenter parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				parentPresenter.OnHeaderLostMouseCapture(e);
				e.Handled = true;
			}
		}

		/// <summary>Gets the key that references the style for the drop location indicator during a header drag operation. </summary>
		/// <returns>The style key for the drop location indicator. </returns>
		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06005CEC RID: 23788 RVA: 0x001A24B8 File Offset: 0x001A06B8
		public static ComponentResourceKey ColumnHeaderDropSeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey;
			}
		}

		/// <summary>Gets the key that references the style for displaying column headers during a header drag operation. </summary>
		/// <returns>The style key for floating column headers. </returns>
		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06005CED RID: 23789 RVA: 0x001A24BF File Offset: 0x001A06BF
		public static ComponentResourceKey ColumnFloatingHeaderStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridColumnHeaderColumnFloatingHeaderStyleKey;
			}
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x001A24C8 File Offset: 0x001A06C8
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (base.IsPressed)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Pressed",
					"MouseOver",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			ListSortDirection? sortDirection = this.SortDirection;
			if (sortDirection != null)
			{
				if (sortDirection == ListSortDirection.Ascending)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SortAscending",
						"Unsorted"
					});
				}
				if (sortDirection == ListSortDirection.Descending)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SortDescending",
						"Unsorted"
					});
				}
			}
			else
			{
				VisualStateManager.GoToState(this, "Unsorted", useTransitions);
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06005CEF RID: 23791 RVA: 0x001A176A File Offset: 0x0019F96A
		DataGridColumn IProvideDataGridColumn.Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x00142714 File Offset: 0x00140914
		private Panel ParentPanel
		{
			get
			{
				return base.VisualParent as Panel;
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x06005CF1 RID: 23793 RVA: 0x001A25CC File Offset: 0x001A07CC
		private DataGridColumnHeader PreviousVisibleHeader
		{
			get
			{
				DataGridColumn column = this.Column;
				if (column != null)
				{
					DataGrid dataGridOwner = column.DataGridOwner;
					if (dataGridOwner != null)
					{
						for (int i = this.DisplayIndex - 1; i >= 0; i--)
						{
							if (dataGridOwner.ColumnFromDisplayIndex(i).IsVisible)
							{
								return dataGridOwner.ColumnHeaderFromDisplayIndex(i);
							}
						}
					}
				}
				return null;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorBrush" /> dependency property.</returns>
		// Token: 0x04002FD9 RID: 12249
		public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorVisibility" /> dependency property.</returns>
		// Token: 0x04002FDA RID: 12250
		public static readonly DependencyProperty SeparatorVisibilityProperty = DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x04002FDB RID: 12251
		private static readonly DependencyPropertyKey DisplayIndexPropertyKey = DependencyProperty.RegisterReadOnly("DisplayIndex", typeof(int), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(DataGridColumnHeader.OnDisplayIndexChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceDisplayIndex)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.DisplayIndex" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.DisplayIndex" /> dependency property.</returns>
		// Token: 0x04002FDC RID: 12252
		public static readonly DependencyProperty DisplayIndexProperty = DataGridColumnHeader.DisplayIndexPropertyKey.DependencyProperty;

		// Token: 0x04002FDD RID: 12253
		private static readonly DependencyPropertyKey CanUserSortPropertyKey = DependencyProperty.RegisterReadOnly("CanUserSort", typeof(bool), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceCanUserSort)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.CanUserSort" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.CanUserSort" /> dependency property.</returns>
		// Token: 0x04002FDE RID: 12254
		public static readonly DependencyProperty CanUserSortProperty = DataGridColumnHeader.CanUserSortPropertyKey.DependencyProperty;

		// Token: 0x04002FDF RID: 12255
		private static readonly DependencyPropertyKey SortDirectionPropertyKey = DependencyProperty.RegisterReadOnly("SortDirection", typeof(ListSortDirection?), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged), new CoerceValueCallback(DataGridColumnHeader.OnCoerceSortDirection)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SortDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.SortDirection" /> dependency property.</returns>
		// Token: 0x04002FE0 RID: 12256
		public static readonly DependencyProperty SortDirectionProperty = DataGridColumnHeader.SortDirectionPropertyKey.DependencyProperty;

		// Token: 0x04002FE1 RID: 12257
		private static readonly DependencyPropertyKey IsFrozenPropertyKey = DependencyProperty.RegisterReadOnly("IsFrozen", typeof(bool), typeof(DataGridColumnHeader), new FrameworkPropertyMetadata(false, null, new CoerceValueCallback(DataGridColumnHeader.OnCoerceIsFrozen)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.IsFrozen" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridColumnHeader.IsFrozen" /> dependency property.</returns>
		// Token: 0x04002FE2 RID: 12258
		public static readonly DependencyProperty IsFrozenProperty = DataGridColumnHeader.IsFrozenPropertyKey.DependencyProperty;

		// Token: 0x04002FE3 RID: 12259
		private DataGridColumn _column;

		// Token: 0x04002FE4 RID: 12260
		private ContainerTracking<DataGridColumnHeader> _tracker;

		// Token: 0x04002FE5 RID: 12261
		private DataGridColumnHeadersPresenter _parentPresenter;

		// Token: 0x04002FE6 RID: 12262
		private Thumb _leftGripper;

		// Token: 0x04002FE7 RID: 12263
		private Thumb _rightGripper;

		// Token: 0x04002FE8 RID: 12264
		private bool _suppressClickEvent;

		// Token: 0x04002FE9 RID: 12265
		private const string LeftHeaderGripperTemplateName = "PART_LeftHeaderGripper";

		// Token: 0x04002FEA RID: 12266
		private const string RightHeaderGripperTemplateName = "PART_RightHeaderGripper";
	}
}
