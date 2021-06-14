using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used within the template of a <see cref="T:System.Windows.Controls.DataGrid" /> to specify the location in the control's visual tree where the column headers are to be added.</summary>
	// Token: 0x0200057E RID: 1406
	[TemplatePart(Name = "PART_FillerColumnHeader", Type = typeof(DataGridColumnHeader))]
	public class DataGridColumnHeadersPresenter : ItemsControl
	{
		// Token: 0x06005CF2 RID: 23794 RVA: 0x001A2618 File Offset: 0x001A0818
		static DataGridColumnHeadersPresenter()
		{
			Type typeFromHandle = typeof(DataGridColumnHeadersPresenter);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeFromHandle));
			UIElement.FocusableProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			FrameworkElementFactory root = new FrameworkElementFactory(typeof(DataGridCellsPanel));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ItemsPanelTemplate(root)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumnHeadersPresenter.OnIsVirtualizingPropertyChanged), new CoerceValueCallback(DataGridColumnHeadersPresenter.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
		}

		/// <summary>It's invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
		// Token: 0x06005CF3 RID: 23795 RVA: 0x001A26C4 File Offset: 0x001A08C4
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			base.OnApplyTemplate();
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				base.ItemsSource = new DataGridColumnHeaderCollection(parentDataGrid.Columns);
				parentDataGrid.ColumnHeadersPresenter = this;
				DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				DataGridColumnHeader dataGridColumnHeader = base.GetTemplateChild("PART_FillerColumnHeader") as DataGridColumnHeader;
				if (dataGridColumnHeader != null)
				{
					DataGridHelper.TransferProperty(dataGridColumnHeader, FrameworkElement.StyleProperty);
					DataGridHelper.TransferProperty(dataGridColumnHeader, FrameworkElement.HeightProperty);
					return;
				}
			}
			else
			{
				base.ItemsSource = null;
			}
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeadersPresenterAutomationPeer" /> for this presenter.</summary>
		/// <returns>A new automation peer for this presenter.</returns>
		// Token: 0x06005CF4 RID: 23796 RVA: 0x001A2753 File Offset: 0x001A0953
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridColumnHeadersPresenterAutomationPeer(this);
		}

		/// <summary>Called to remeasure a control.</summary>
		/// <param name="availableSize">The available size that this element can give to child elements.</param>
		/// <returns>The size of the control, up to the maximum specified by constraint.</returns>
		// Token: 0x06005CF5 RID: 23797 RVA: 0x001A275C File Offset: 0x001A095C
		protected override Size MeasureOverride(Size availableSize)
		{
			Size size = availableSize;
			size.Width = double.PositiveInfinity;
			Size result = base.MeasureOverride(size);
			if (this._columnHeaderDragIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDragIndicator.Measure(size);
				Size desiredSize = this._columnHeaderDragIndicator.DesiredSize;
				result.Width = Math.Max(result.Width, desiredSize.Width);
				result.Height = Math.Max(result.Height, desiredSize.Height);
			}
			if (this._columnHeaderDropLocationIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDropLocationIndicator.Measure(availableSize);
				Size desiredSize = this._columnHeaderDropLocationIndicator.DesiredSize;
				result.Width = Math.Max(result.Width, desiredSize.Width);
				result.Height = Math.Max(result.Height, desiredSize.Height);
			}
			result.Width = Math.Min(availableSize.Width, result.Width);
			return result;
		}

		/// <summary>Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control" /> object.</summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x06005CF6 RID: 23798 RVA: 0x001A2858 File Offset: 0x001A0A58
		protected override Size ArrangeOverride(Size finalSize)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(this) > 0) ? (VisualTreeHelper.GetChild(this, 0) as UIElement) : null;
			if (uielement != null)
			{
				Rect finalRect = new Rect(finalSize);
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					finalRect.X = -parentDataGrid.HorizontalScrollOffset;
					finalRect.Width = Math.Max(finalSize.Width, parentDataGrid.CellsPanelActualWidth);
				}
				uielement.Arrange(finalRect);
			}
			if (this._columnHeaderDragIndicator != null && this._isColumnHeaderDragging)
			{
				this._columnHeaderDragIndicator.Arrange(new Rect(new Point(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartRelativePosition.X, 0.0), new Size(this._columnHeaderDragIndicator.Width, this._columnHeaderDragIndicator.Height)));
			}
			if (this._columnHeaderDropLocationIndicator != null && this._isColumnHeaderDragging)
			{
				Point location = this.FindColumnHeaderPositionByCurrentPosition(this._columnHeaderDragCurrentPosition, true);
				double width = this._columnHeaderDropLocationIndicator.Width;
				location.X -= width * 0.5;
				this._columnHeaderDropLocationIndicator.Arrange(new Rect(location, new Size(width, this._columnHeaderDropLocationIndicator.Height)));
			}
			return finalSize;
		}

		/// <summary>Returns a geometry for a clipping mask. The mask applies if the layout system attempts to arrange an element that is larger than the available display space.</summary>
		/// <param name="layoutSlotSize">The rendered size of the column header.</param>
		/// <returns>The clipping geometry.</returns>
		// Token: 0x06005CF7 RID: 23799 RVA: 0x001A298C File Offset: 0x001A0B8C
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(base.RenderSize));
			rectangleGeometry.Freeze();
			return rectangleGeometry;
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeader" /> instance to contain a column header value.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeader" /> instance. </returns>
		// Token: 0x06005CF8 RID: 23800 RVA: 0x001A29B1 File Offset: 0x001A0BB1
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridColumnHeader();
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item is (or is eligible to be) its own container; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005CF9 RID: 23801 RVA: 0x001A29B8 File Offset: 0x001A0BB8
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridColumnHeader;
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x0016992D File Offset: 0x00167B2D
		internal bool IsItemItsOwnContainerInternal(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x06005CFB RID: 23803 RVA: 0x001A29C4 File Offset: 0x001A0BC4
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridColumnHeader dataGridColumnHeader = element as DataGridColumnHeader;
			if (dataGridColumnHeader != null)
			{
				DataGridColumn column = this.ColumnFromContainer(dataGridColumnHeader);
				if (dataGridColumnHeader.Column == null)
				{
					dataGridColumnHeader.Tracker.StartTracking(ref this._headerTrackingRoot);
				}
				dataGridColumnHeader.PrepareColumnHeader(item, column);
			}
		}

		/// <summary>When overridden in a derived class, undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)" /> method.</summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		// Token: 0x06005CFC RID: 23804 RVA: 0x001A2A04 File Offset: 0x001A0C04
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridColumnHeader dataGridColumnHeader = element as DataGridColumnHeader;
			base.ClearContainerForItemOverride(element, item);
			if (dataGridColumnHeader != null)
			{
				dataGridColumnHeader.Tracker.StopTracking(ref this._headerTrackingRoot);
				dataGridColumnHeader.ClearHeader();
			}
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x001A2A3C File Offset: 0x001A0C3C
		private DataGridColumn ColumnFromContainer(DataGridColumnHeader container)
		{
			int index = base.ItemContainerGenerator.IndexFromContainer(container);
			return this.HeaderCollection.ColumnFromIndex(index);
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x001A2A62 File Offset: 0x001A0C62
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x001A2A74 File Offset: 0x001A0C74
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (DataGridHelper.ShouldNotifyColumnHeadersPresenter(target))
			{
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					if (dataGridColumn.IsVisible)
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrange();
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty || e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGrid.CellsPanelHorizontalOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "DelayedColumnWidthComputation", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange();
				}
				else if (e.Property == DataGrid.HorizontalScrollOffsetProperty)
				{
					base.InvalidateArrange();
					this.InvalidateDataGridCellsPanelMeasureAndArrange();
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForNonVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(false);
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(true);
				}
				else if (e.Property == DataGrid.CellsPanelActualWidthProperty)
				{
					base.InvalidateArrange();
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				}
			}
			if (DataGridHelper.ShouldNotifyColumnHeaders(target))
			{
				if (e.Property == DataGridColumn.HeaderProperty)
				{
					if (this.HeaderCollection != null)
					{
						this.HeaderCollection.NotifyHeaderPropertyChanged(dataGridColumn, e);
						return;
					}
				}
				else
				{
					for (ContainerTracking<DataGridColumnHeader> containerTracking = this._headerTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
					{
						containerTracking.Container.NotifyPropertyChanged(d, e);
					}
					if (d is DataGrid && (e.Property == DataGrid.ColumnHeaderStyleProperty || e.Property == DataGrid.ColumnHeaderHeightProperty))
					{
						DataGridColumnHeader dataGridColumnHeader = base.GetTemplateChild("PART_FillerColumnHeader") as DataGridColumnHeader;
						if (dataGridColumnHeader != null)
						{
							dataGridColumnHeader.NotifyPropertyChanged(d, e);
						}
					}
				}
			}
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x001A2C20 File Offset: 0x001A0E20
		private static void OnIsVirtualizingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = (DataGridColumnHeadersPresenter)d;
			DataGridHelper.TransferProperty(dataGridColumnHeadersPresenter, VirtualizingPanel.IsVirtualizingProperty);
			if (e.OldValue != dataGridColumnHeadersPresenter.GetValue(VirtualizingPanel.IsVirtualizingProperty))
			{
				dataGridColumnHeadersPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x001A2C5C File Offset: 0x001A0E5C
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = d as DataGridColumnHeadersPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridColumnHeadersPresenter, baseValue, VirtualizingPanel.IsVirtualizingProperty, dataGridColumnHeadersPresenter.ParentDataGrid, DataGrid.EnableColumnVirtualizationProperty);
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x001A2C87 File Offset: 0x001A0E87
		private void InvalidateDataGridCellsPanelMeasureAndArrange()
		{
			if (this._internalItemsHost != null)
			{
				this._internalItemsHost.InvalidateMeasure();
				this._internalItemsHost.InvalidateArrange();
			}
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x001A2CA7 File Offset: 0x001A0EA7
		private void InvalidateDataGridCellsPanelMeasureAndArrange(bool withColumnVirtualization)
		{
			if (withColumnVirtualization == VirtualizingPanel.GetIsVirtualizing(this))
			{
				this.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x06005D04 RID: 23812 RVA: 0x001A2CB8 File Offset: 0x001A0EB8
		// (set) Token: 0x06005D05 RID: 23813 RVA: 0x001A2CC0 File Offset: 0x001A0EC0
		internal Panel InternalItemsHost
		{
			get
			{
				return this._internalItemsHost;
			}
			set
			{
				this._internalItemsHost = value;
			}
		}

		/// <summary>Gets the number of <see cref="T:System.Windows.Media.Visual" /> children in this presenter.</summary>
		/// <returns>The number of visual children. </returns>
		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06005D06 RID: 23814 RVA: 0x001A2CCC File Offset: 0x001A0ECC
		protected override int VisualChildrenCount
		{
			get
			{
				int num = base.VisualChildrenCount;
				if (this._columnHeaderDragIndicator != null)
				{
					num++;
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					num++;
				}
				return num;
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Media.Visual" /> child at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Media.Visual" /> child to return.</param>
		/// <returns>The child. </returns>
		// Token: 0x06005D07 RID: 23815 RVA: 0x001A2CFC File Offset: 0x001A0EFC
		protected override Visual GetVisualChild(int index)
		{
			int visualChildrenCount = base.VisualChildrenCount;
			if (index == visualChildrenCount)
			{
				if (this._columnHeaderDragIndicator != null)
				{
					return this._columnHeaderDragIndicator;
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					return this._columnHeaderDropLocationIndicator;
				}
			}
			if (index == visualChildrenCount + 1 && this._columnHeaderDragIndicator != null && this._columnHeaderDropLocationIndicator != null)
			{
				return this._columnHeaderDropLocationIndicator;
			}
			return base.GetVisualChild(index);
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x001A2D58 File Offset: 0x001A0F58
		internal void OnHeaderMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.ParentDataGrid == null)
			{
				return;
			}
			if (this._columnHeaderDragIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
				this._columnHeaderDragIndicator = null;
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
				this._columnHeaderDropLocationIndicator = null;
			}
			Point position = e.GetPosition(this);
			DataGridColumnHeader dataGridColumnHeader = this.FindColumnHeaderByPosition(position);
			if (dataGridColumnHeader != null)
			{
				DataGridColumn column = dataGridColumnHeader.Column;
				if (this.ParentDataGrid.CanUserReorderColumns && column.CanUserReorder)
				{
					this.PrepareColumnHeaderDrag(dataGridColumnHeader, e.GetPosition(this), e.GetPosition(dataGridColumnHeader));
					return;
				}
			}
			else
			{
				this._isColumnHeaderDragging = false;
				this._prepareColumnHeaderDragging = false;
				this._draggingSrcColumnHeader = null;
				base.InvalidateArrange();
			}
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x001A2E04 File Offset: 0x001A1004
		internal void OnHeaderMouseMove(MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && this._prepareColumnHeaderDragging)
			{
				this._columnHeaderDragCurrentPosition = e.GetPosition(this);
				if (!this._isColumnHeaderDragging)
				{
					if (DataGridColumnHeadersPresenter.CheckStartColumnHeaderDrag(this._columnHeaderDragCurrentPosition, this._columnHeaderDragStartPosition))
					{
						this.StartColumnHeaderDrag();
						return;
					}
				}
				else
				{
					Visibility visibility = this.IsMousePositionValidForColumnDrag(2.0) ? Visibility.Visible : Visibility.Collapsed;
					if (this._columnHeaderDragIndicator != null)
					{
						this._columnHeaderDragIndicator.Visibility = visibility;
					}
					if (this._columnHeaderDropLocationIndicator != null)
					{
						this._columnHeaderDropLocationIndicator.Visibility = visibility;
					}
					base.InvalidateArrange();
					DragDeltaEventArgs e2 = new DragDeltaEventArgs(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartPosition.X, this._columnHeaderDragCurrentPosition.Y - this._columnHeaderDragStartPosition.Y);
					this._columnHeaderDragStartPosition = this._columnHeaderDragCurrentPosition;
					this.ParentDataGrid.OnColumnHeaderDragDelta(e2);
				}
			}
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x001A2EEC File Offset: 0x001A10EC
		internal void OnHeaderMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this._isColumnHeaderDragging)
			{
				this._columnHeaderDragCurrentPosition = e.GetPosition(this);
				this.FinishColumnHeaderDrag(false);
				return;
			}
			this.ClearColumnHeaderDragInfo();
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x001A2F11 File Offset: 0x001A1111
		internal void OnHeaderLostMouseCapture(MouseEventArgs e)
		{
			if (this._isColumnHeaderDragging && Mouse.LeftButton == MouseButtonState.Pressed)
			{
				this.FinishColumnHeaderDrag(true);
			}
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x001A2F2C File Offset: 0x001A112C
		private void ClearColumnHeaderDragInfo()
		{
			this._isColumnHeaderDragging = false;
			this._prepareColumnHeaderDragging = false;
			this._draggingSrcColumnHeader = null;
			if (this._columnHeaderDragIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
				this._columnHeaderDragIndicator = null;
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
				this._columnHeaderDropLocationIndicator = null;
			}
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x001A2F84 File Offset: 0x001A1184
		private void PrepareColumnHeaderDrag(DataGridColumnHeader header, Point pos, Point relativePos)
		{
			this._prepareColumnHeaderDragging = true;
			this._isColumnHeaderDragging = false;
			this._draggingSrcColumnHeader = header;
			this._columnHeaderDragStartPosition = pos;
			this._columnHeaderDragStartRelativePosition = relativePos;
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x001A2FA9 File Offset: 0x001A11A9
		private static bool CheckStartColumnHeaderDrag(Point currentPos, Point originalPos)
		{
			return DoubleUtil.GreaterThan(Math.Abs(currentPos.X - originalPos.X), SystemParameters.MinimumHorizontalDragDistance);
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x001A2FCC File Offset: 0x001A11CC
		private bool IsMousePositionValidForColumnDrag(double dragFactor)
		{
			int num = -1;
			return this.IsMousePositionValidForColumnDrag(dragFactor, out num);
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x001A2FE4 File Offset: 0x001A11E4
		private bool IsMousePositionValidForColumnDrag(double dragFactor, out int nearestDisplayIndex)
		{
			nearestDisplayIndex = -1;
			bool flag = false;
			if (this._draggingSrcColumnHeader.Column != null)
			{
				flag = this._draggingSrcColumnHeader.Column.IsFrozen;
			}
			int num = 0;
			if (this.ParentDataGrid != null)
			{
				num = this.ParentDataGrid.FrozenColumnCount;
			}
			nearestDisplayIndex = this.FindDisplayIndexByPosition(this._columnHeaderDragCurrentPosition, true);
			if (flag && nearestDisplayIndex >= num)
			{
				return false;
			}
			if (!flag && nearestDisplayIndex < num)
			{
				return false;
			}
			double num2;
			if (this._columnHeaderDragIndicator == null)
			{
				num2 = this._draggingSrcColumnHeader.RenderSize.Height;
			}
			else
			{
				num2 = Math.Max(this._draggingSrcColumnHeader.RenderSize.Height, this._columnHeaderDragIndicator.Height);
			}
			return DoubleUtil.LessThanOrClose(-num2 * dragFactor, this._columnHeaderDragCurrentPosition.Y) && DoubleUtil.LessThanOrClose(this._columnHeaderDragCurrentPosition.Y, num2 * (dragFactor + 1.0));
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x001A30D0 File Offset: 0x001A12D0
		private void StartColumnHeaderDrag()
		{
			this._columnHeaderDragStartPosition = this._columnHeaderDragCurrentPosition;
			DragStartedEventArgs e = new DragStartedEventArgs(this._columnHeaderDragStartPosition.X, this._columnHeaderDragStartPosition.Y);
			this.ParentDataGrid.OnColumnHeaderDragStarted(e);
			DataGridColumnReorderingEventArgs dataGridColumnReorderingEventArgs = new DataGridColumnReorderingEventArgs(this._draggingSrcColumnHeader.Column);
			this._columnHeaderDragIndicator = this.CreateColumnHeaderDragIndicator();
			this._columnHeaderDropLocationIndicator = this.CreateColumnHeaderDropIndicator();
			dataGridColumnReorderingEventArgs.DragIndicator = this._columnHeaderDragIndicator;
			dataGridColumnReorderingEventArgs.DropLocationIndicator = this._columnHeaderDropLocationIndicator;
			this.ParentDataGrid.OnColumnReordering(dataGridColumnReorderingEventArgs);
			if (!dataGridColumnReorderingEventArgs.Cancel)
			{
				this._isColumnHeaderDragging = true;
				this._columnHeaderDragIndicator = dataGridColumnReorderingEventArgs.DragIndicator;
				this._columnHeaderDropLocationIndicator = dataGridColumnReorderingEventArgs.DropLocationIndicator;
				if (this._columnHeaderDragIndicator != null)
				{
					this.SetDefaultsOnDragIndicator();
					base.AddVisualChild(this._columnHeaderDragIndicator);
				}
				if (this._columnHeaderDropLocationIndicator != null)
				{
					this.SetDefaultsOnDropIndicator();
					base.AddVisualChild(this._columnHeaderDropLocationIndicator);
				}
				this._draggingSrcColumnHeader.SuppressClickEvent = true;
				base.InvalidateMeasure();
				return;
			}
			this.FinishColumnHeaderDrag(true);
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x001A31D4 File Offset: 0x001A13D4
		private Control CreateColumnHeaderDragIndicator()
		{
			return new DataGridColumnFloatingHeader
			{
				ReferenceHeader = this._draggingSrcColumnHeader
			};
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x001A31F4 File Offset: 0x001A13F4
		private void SetDefaultsOnDragIndicator()
		{
			DataGridColumn column = this._draggingSrcColumnHeader.Column;
			Style style = null;
			if (column != null)
			{
				style = column.DragIndicatorStyle;
			}
			this._columnHeaderDragIndicator.Style = style;
			this._columnHeaderDragIndicator.CoerceValue(FrameworkElement.WidthProperty);
			this._columnHeaderDragIndicator.CoerceValue(FrameworkElement.HeightProperty);
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x001A3248 File Offset: 0x001A1448
		private Control CreateColumnHeaderDropIndicator()
		{
			return new DataGridColumnDropSeparator
			{
				ReferenceHeader = this._draggingSrcColumnHeader
			};
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x001A3268 File Offset: 0x001A1468
		private void SetDefaultsOnDropIndicator()
		{
			Style style = null;
			if (this.ParentDataGrid != null)
			{
				style = this.ParentDataGrid.DropLocationIndicatorStyle;
			}
			this._columnHeaderDropLocationIndicator.Style = style;
			this._columnHeaderDropLocationIndicator.CoerceValue(FrameworkElement.WidthProperty);
			this._columnHeaderDropLocationIndicator.CoerceValue(FrameworkElement.HeightProperty);
		}

		// Token: 0x06005D16 RID: 23830 RVA: 0x001A32B8 File Offset: 0x001A14B8
		private void FinishColumnHeaderDrag(bool isCancel)
		{
			this._prepareColumnHeaderDragging = false;
			this._isColumnHeaderDragging = false;
			this._draggingSrcColumnHeader.SuppressClickEvent = false;
			if (this._columnHeaderDragIndicator != null)
			{
				this._columnHeaderDragIndicator.Visibility = Visibility.Collapsed;
				DataGridColumnFloatingHeader dataGridColumnFloatingHeader = this._columnHeaderDragIndicator as DataGridColumnFloatingHeader;
				if (dataGridColumnFloatingHeader != null)
				{
					dataGridColumnFloatingHeader.ClearHeader();
				}
				base.RemoveVisualChild(this._columnHeaderDragIndicator);
			}
			if (this._columnHeaderDropLocationIndicator != null)
			{
				this._columnHeaderDropLocationIndicator.Visibility = Visibility.Collapsed;
				DataGridColumnDropSeparator dataGridColumnDropSeparator = this._columnHeaderDropLocationIndicator as DataGridColumnDropSeparator;
				if (dataGridColumnDropSeparator != null)
				{
					dataGridColumnDropSeparator.ReferenceHeader = null;
				}
				base.RemoveVisualChild(this._columnHeaderDropLocationIndicator);
			}
			DragCompletedEventArgs e = new DragCompletedEventArgs(this._columnHeaderDragCurrentPosition.X - this._columnHeaderDragStartPosition.X, this._columnHeaderDragCurrentPosition.Y - this._columnHeaderDragStartPosition.Y, isCancel);
			this.ParentDataGrid.OnColumnHeaderDragCompleted(e);
			this._draggingSrcColumnHeader.InvalidateArrange();
			if (!isCancel)
			{
				int num = -1;
				bool flag = this.IsMousePositionValidForColumnDrag(2.0, out num);
				DataGridColumn column = this._draggingSrcColumnHeader.Column;
				if (column != null && flag && num != column.DisplayIndex)
				{
					column.DisplayIndex = num;
					DataGridColumnEventArgs e2 = new DataGridColumnEventArgs(this._draggingSrcColumnHeader.Column);
					this.ParentDataGrid.OnColumnReordered(e2);
				}
			}
			this._draggingSrcColumnHeader = null;
			this._columnHeaderDragIndicator = null;
			this._columnHeaderDropLocationIndicator = null;
		}

		// Token: 0x06005D17 RID: 23831 RVA: 0x001A340C File Offset: 0x001A160C
		private int FindDisplayIndexByPosition(Point startPos, bool findNearestColumn)
		{
			int result;
			Point point;
			DataGridColumnHeader dataGridColumnHeader;
			this.FindDisplayIndexAndHeaderPosition(startPos, findNearestColumn, out result, out point, out dataGridColumnHeader);
			return result;
		}

		// Token: 0x06005D18 RID: 23832 RVA: 0x001A3428 File Offset: 0x001A1628
		private DataGridColumnHeader FindColumnHeaderByPosition(Point startPos)
		{
			int num;
			Point point;
			DataGridColumnHeader result;
			this.FindDisplayIndexAndHeaderPosition(startPos, false, out num, out point, out result);
			return result;
		}

		// Token: 0x06005D19 RID: 23833 RVA: 0x001A3444 File Offset: 0x001A1644
		private Point FindColumnHeaderPositionByCurrentPosition(Point startPos, bool findNearestColumn)
		{
			int num;
			Point result;
			DataGridColumnHeader dataGridColumnHeader;
			this.FindDisplayIndexAndHeaderPosition(startPos, findNearestColumn, out num, out result, out dataGridColumnHeader);
			return result;
		}

		// Token: 0x06005D1A RID: 23834 RVA: 0x001A3460 File Offset: 0x001A1660
		private static double GetColumnEstimatedWidth(DataGridColumn column, double averageColumnWidth)
		{
			double num = column.Width.DisplayValue;
			if (DoubleUtil.IsNaN(num))
			{
				num = Math.Max(averageColumnWidth, column.MinWidth);
				num = Math.Min(num, column.MaxWidth);
			}
			return num;
		}

		// Token: 0x06005D1B RID: 23835 RVA: 0x001A34A0 File Offset: 0x001A16A0
		private void FindDisplayIndexAndHeaderPosition(Point startPos, bool findNearestColumn, out int displayIndex, out Point headerPos, out DataGridColumnHeader header)
		{
			Point point = new Point(0.0, 0.0);
			headerPos = point;
			displayIndex = -1;
			header = null;
			if (startPos.X < 0.0)
			{
				if (findNearestColumn)
				{
					displayIndex = 0;
				}
				return;
			}
			double num = 0.0;
			double num2 = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			bool flag = false;
			int i = 0;
			while (i < parentDataGrid.Columns.Count)
			{
				displayIndex++;
				DataGridColumnHeader dataGridColumnHeader = parentDataGrid.ColumnHeaderFromDisplayIndex(i);
				if (dataGridColumnHeader != null)
				{
					GeneralTransform generalTransform = dataGridColumnHeader.TransformToAncestor(this);
					num = generalTransform.Transform(point).X;
					num2 = num + dataGridColumnHeader.RenderSize.Width;
					goto IL_FB;
				}
				DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					num = num2;
					if (i >= parentDataGrid.FrozenColumnCount && !flag)
					{
						num -= parentDataGrid.HorizontalScrollOffset;
						flag = true;
					}
					num2 = num + DataGridColumnHeadersPresenter.GetColumnEstimatedWidth(dataGridColumn, averageColumnWidth);
					goto IL_FB;
				}
				IL_18D:
				i++;
				continue;
				IL_FB:
				if (DoubleUtil.LessThanOrClose(startPos.X, num))
				{
					break;
				}
				if (!DoubleUtil.GreaterThanOrClose(startPos.X, num) || !DoubleUtil.LessThanOrClose(startPos.X, num2))
				{
					goto IL_18D;
				}
				if (!findNearestColumn)
				{
					header = dataGridColumnHeader;
					break;
				}
				double value = (num + num2) * 0.5;
				if (DoubleUtil.GreaterThanOrClose(startPos.X, value))
				{
					num = num2;
					displayIndex++;
				}
				if (this._draggingSrcColumnHeader != null && this._draggingSrcColumnHeader.Column != null && this._draggingSrcColumnHeader.Column.DisplayIndex < displayIndex)
				{
					displayIndex--;
					break;
				}
				break;
			}
			if (i == parentDataGrid.Columns.Count)
			{
				displayIndex = parentDataGrid.Columns.Count - 1;
				num = num2;
			}
			headerPos.X = num;
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06005D1C RID: 23836 RVA: 0x001A3679 File Offset: 0x001A1879
		private DataGridColumnHeaderCollection HeaderCollection
		{
			get
			{
				return base.ItemsSource as DataGridColumnHeaderCollection;
			}
		}

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x001A3686 File Offset: 0x001A1886
		internal DataGrid ParentDataGrid
		{
			get
			{
				if (this._parentDataGrid == null)
				{
					this._parentDataGrid = DataGridHelper.FindParent<DataGrid>(this);
				}
				return this._parentDataGrid;
			}
		}

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06005D1E RID: 23838 RVA: 0x001A36A2 File Offset: 0x001A18A2
		internal ContainerTracking<DataGridColumnHeader> HeaderTrackingRoot
		{
			get
			{
				return this._headerTrackingRoot;
			}
		}

		// Token: 0x04002FEB RID: 12267
		private const string ElementFillerColumnHeader = "PART_FillerColumnHeader";

		// Token: 0x04002FEC RID: 12268
		private ContainerTracking<DataGridColumnHeader> _headerTrackingRoot;

		// Token: 0x04002FED RID: 12269
		private DataGrid _parentDataGrid;

		// Token: 0x04002FEE RID: 12270
		private bool _prepareColumnHeaderDragging;

		// Token: 0x04002FEF RID: 12271
		private bool _isColumnHeaderDragging;

		// Token: 0x04002FF0 RID: 12272
		private DataGridColumnHeader _draggingSrcColumnHeader;

		// Token: 0x04002FF1 RID: 12273
		private Point _columnHeaderDragStartPosition;

		// Token: 0x04002FF2 RID: 12274
		private Point _columnHeaderDragStartRelativePosition;

		// Token: 0x04002FF3 RID: 12275
		private Point _columnHeaderDragCurrentPosition;

		// Token: 0x04002FF4 RID: 12276
		private Control _columnHeaderDropLocationIndicator;

		// Token: 0x04002FF5 RID: 12277
		private Control _columnHeaderDragIndicator;

		// Token: 0x04002FF6 RID: 12278
		private Panel _internalItemsHost;
	}
}
