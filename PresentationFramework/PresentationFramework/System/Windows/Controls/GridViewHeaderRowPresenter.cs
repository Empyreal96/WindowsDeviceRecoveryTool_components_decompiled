using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents an object that is used to define the layout of a row of column headers. </summary>
	// Token: 0x020004DC RID: 1244
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	public class GridViewHeaderRowPresenter : GridViewRowPresenterBase
	{
		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> to use for the column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> to use for the column header container. The default is <see langword="null" />.</returns>
		// Token: 0x170012DE RID: 4830
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x0015C6E4 File Offset: 0x0015A8E4
		// (set) Token: 0x06004D67 RID: 19815 RVA: 0x0015C6F6 File Offset: 0x0015A8F6
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the template to use to display the column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> that is used to display the column header content. The default is <see langword="null" />.</returns>
		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x0015C704 File Offset: 0x0015A904
		// (set) Token: 0x06004D69 RID: 19817 RVA: 0x0015C716 File Offset: 0x0015A916
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that provides logic that selects the data template to use to display a column header. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that chooses the <see cref="T:System.Windows.DataTemplate" /> to use to display each column header. The default is <see langword="null" />.</returns>
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06004D6A RID: 19818 RVA: 0x0015C724 File Offset: 0x0015A924
		// (set) Token: 0x06004D6B RID: 19819 RVA: 0x0015C736 File Offset: 0x0015A936
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets a composite string that specifies how to format the column headers if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the column headers if they are displayed as strings. The default is <see langword="null" />.</returns>
		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x0015C744 File Offset: 0x0015A944
		// (set) Token: 0x06004D6D RID: 19821 RVA: 0x0015C756 File Offset: 0x0015A956
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ColumnHeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether columns can change positions. </summary>
		/// <returns>
		///     <see langword="true" /> if columns can be moved by the drag-and-drop operation of a column header; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x0015C764 File Offset: 0x0015A964
		// (set) Token: 0x06004D6F RID: 19823 RVA: 0x0015C776 File Offset: 0x0015A976
		public bool AllowsColumnReorder
		{
			get
			{
				return (bool)base.GetValue(GridViewHeaderRowPresenter.AllowsColumnReorderProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.AllowsColumnReorderProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ContextMenu" /> for the column headers.   </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ContextMenu" /> for the column header row. The default is <see langword="null" />.</returns>
		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x0015C784 File Offset: 0x0015A984
		// (set) Token: 0x06004D71 RID: 19825 RVA: 0x0015C796 File Offset: 0x0015A996
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty, value);
			}
		}

		/// <summary>Gets or sets the content for a tooltip for the column header row. </summary>
		/// <returns>An object that represents the content of a tooltip for the column headers.</returns>
		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06004D72 RID: 19826 RVA: 0x0015C7A4 File Offset: 0x0015A9A4
		// (set) Token: 0x06004D73 RID: 19827 RVA: 0x0015C7B1 File Offset: 0x0015A9B1
		public object ColumnHeaderToolTip
		{
			get
			{
				return base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty, value);
			}
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0015C7C0 File Offset: 0x0015A9C0
		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewHeaderRowPresenter gridViewHeaderRowPresenter = (GridViewHeaderRowPresenter)d;
			if (e.Property == GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty || e.Property == GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty)
			{
				Helper.CheckTemplateAndTemplateSelector("GridViewHeaderRowPresenter", GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty, GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty, gridViewHeaderRowPresenter);
			}
			gridViewHeaderRowPresenter.UpdateAllHeaders(e.Property);
		}

		/// <summary>Determines the area that is required to display the column header row.</summary>
		/// <param name="constraint">The amount of area that is available to display the column header row.</param>
		/// <returns>The required <see cref="T:System.Windows.Size" /> for the column header row.</returns>
		// Token: 0x06004D75 RID: 19829 RVA: 0x0015C814 File Offset: 0x0015AA14
		protected override Size MeasureOverride(Size constraint)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double height = constraint.Height;
			bool flag = false;
			if (columns != null)
			{
				for (int i = 0; i < columns.Count; i++)
				{
					UIElement uielement = internalChildren[this.GetVisualIndex(i)];
					if (uielement != null)
					{
						double num3 = Math.Max(0.0, constraint.Width - num2);
						GridViewColumn gridViewColumn = columns[i];
						if (gridViewColumn.State == ColumnMeasureState.Init)
						{
							if (!flag)
							{
								base.EnsureDesiredWidthList();
								base.LayoutUpdated += this.OnLayoutUpdated;
								flag = true;
							}
							uielement.Measure(new Size(num3, height));
							base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.EnsureWidth(uielement.DesiredSize.Width);
							num2 += gridViewColumn.DesiredWidth;
						}
						else if (gridViewColumn.State == ColumnMeasureState.Headered || gridViewColumn.State == ColumnMeasureState.Data)
						{
							num3 = Math.Min(num3, gridViewColumn.DesiredWidth);
							uielement.Measure(new Size(num3, height));
							num2 += gridViewColumn.DesiredWidth;
						}
						else
						{
							num3 = Math.Min(num3, gridViewColumn.Width);
							uielement.Measure(new Size(num3, height));
							num2 += gridViewColumn.Width;
						}
						num = Math.Max(num, uielement.DesiredSize.Height);
					}
				}
			}
			this._paddingHeader.Measure(new Size(0.0, height));
			num = Math.Max(num, this._paddingHeader.DesiredSize.Height);
			num2 += 2.0;
			if (this._isHeaderDragging)
			{
				this._indicator.Measure(constraint);
				this._floatingHeader.Measure(constraint);
			}
			return new Size(num2, num);
		}

		/// <summary>Arranges the content of the header row elements, and computes the actual size of the header row.</summary>
		/// <param name="arrangeSize">The area that is available for the column header row.</param>
		/// <returns>The actual <see cref="T:System.Windows.Size" /> for the column header row.</returns>
		// Token: 0x06004D76 RID: 19830 RVA: 0x0015CA04 File Offset: 0x0015AC04
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = arrangeSize.Width;
			this.HeadersPositionList.Clear();
			Rect rect;
			if (columns != null)
			{
				for (int i = 0; i < columns.Count; i++)
				{
					UIElement uielement = internalChildren[this.GetVisualIndex(i)];
					if (uielement != null)
					{
						GridViewColumn gridViewColumn = columns[i];
						double num3 = Math.Min(num2, (gridViewColumn.State == ColumnMeasureState.SpecificWidth) ? gridViewColumn.Width : gridViewColumn.DesiredWidth);
						rect = new Rect(num, 0.0, num3, arrangeSize.Height);
						uielement.Arrange(rect);
						this.HeadersPositionList.Add(rect);
						num2 -= num3;
						num += num3;
					}
				}
				if (this._isColumnChangedOrCreated)
				{
					for (int j = 0; j < columns.Count; j++)
					{
						GridViewColumnHeader gridViewColumnHeader = internalChildren[this.GetVisualIndex(j)] as GridViewColumnHeader;
						gridViewColumnHeader.CheckWidthForPreviousHeaderGripper();
					}
					this._paddingHeader.CheckWidthForPreviousHeaderGripper();
					this._isColumnChangedOrCreated = false;
				}
			}
			rect = new Rect(num, 0.0, Math.Max(num2, 0.0), arrangeSize.Height);
			this._paddingHeader.Arrange(rect);
			this.HeadersPositionList.Add(rect);
			if (this._isHeaderDragging)
			{
				this._floatingHeader.Arrange(new Rect(new Point(this._currentPos.X - this._relativeStartPos.X, 0.0), this.HeadersPositionList[this._startColumnIndex].Size));
				Point location = this.FindPositionByIndex(this._desColumnIndex);
				this._indicator.Arrange(new Rect(location, new Size(this._indicator.DesiredSize.Width, arrangeSize.Height)));
			}
			return arrangeSize;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event that occurs when the user presses the left mouse button inside a <see cref="T:System.Windows.Controls.GridViewColumnHeader" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D77 RID: 19831 RVA: 0x0015CC00 File Offset: 0x0015AE00
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			GridViewColumnHeader gridViewColumnHeader = e.Source as GridViewColumnHeader;
			if (gridViewColumnHeader != null && this.AllowsColumnReorder)
			{
				this.PrepareHeaderDrag(gridViewColumnHeader, e.GetPosition(this), e.GetPosition(gridViewColumnHeader), false);
				this.MakeParentItemsControlGotFocus();
			}
			e.Handled = true;
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event that occurs when the user releases the left mouse button. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D78 RID: 19832 RVA: 0x0015CC4E File Offset: 0x0015AE4E
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			this._prepareDragging = false;
			if (this._isHeaderDragging)
			{
				this.FinishHeaderDrag(false);
			}
			e.Handled = true;
			base.OnMouseLeftButtonUp(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> event that occurs when the user moves the mouse.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D79 RID: 19833 RVA: 0x0015CC74 File Offset: 0x0015AE74
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.LeftButton == MouseButtonState.Pressed && this._prepareDragging)
			{
				this._currentPos = e.GetPosition(this);
				this._desColumnIndex = this.FindIndexByPosition(this._currentPos, true);
				if (!this._isHeaderDragging)
				{
					if (this.CheckStartHeaderDrag(this._currentPos, this._startPos))
					{
						this.StartHeaderDrag();
						base.InvalidateMeasure();
					}
				}
				else
				{
					bool flag = GridViewHeaderRowPresenter.IsMousePositionValid(this._floatingHeader, this._currentPos, 2.0);
					this._indicator.Visibility = (this._floatingHeader.Visibility = (flag ? Visibility.Visible : Visibility.Hidden));
					base.InvalidateArrange();
				}
			}
			e.Handled = true;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> event for the <see cref="T:System.Windows.Controls.GridViewColumnHeader" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06004D7A RID: 19834 RVA: 0x0015CD31 File Offset: 0x0015AF31
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (e.LeftButton == MouseButtonState.Pressed && this._isHeaderDragging)
			{
				this.FinishHeaderDrag(true);
			}
			this._prepareDragging = false;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0015CD5C File Offset: 0x0015AF5C
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.NeedUpdateVisualTree)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				GridViewColumnCollection columns = base.Columns;
				this.RenewEvents();
				if (internalChildren.Count == 0)
				{
					this.AddPaddingColumnHeader();
					this.AddIndicator();
					this.AddFloatingHeader(null);
				}
				else if (internalChildren.Count > 3)
				{
					int num = internalChildren.Count - 3;
					for (int i = 0; i < num; i++)
					{
						this.RemoveHeader(null, 1);
					}
				}
				this.UpdatePaddingHeader(this._paddingHeader);
				if (columns != null)
				{
					int num2 = 1;
					for (int j = columns.Count - 1; j >= 0; j--)
					{
						GridViewColumn column = columns[j];
						GridViewColumnHeader gridViewColumnHeader = this.CreateAndInsertHeader(column, num2++);
					}
				}
				this.BuildHeaderLinks();
				base.NeedUpdateVisualTree = false;
				this._isColumnChangedOrCreated = true;
			}
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0015CE2C File Offset: 0x0015B02C
		internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
		{
			if (column.ActualIndex >= 0)
			{
				GridViewColumnHeader gridViewColumnHeader = this.FindHeaderByColumn(column);
				if (gridViewColumnHeader != null)
				{
					if (GridViewColumn.WidthProperty.Name.Equals(propertyName) || "ActualWidth".Equals(propertyName))
					{
						base.InvalidateMeasure();
						return;
					}
					if (GridViewColumn.HeaderProperty.Name.Equals(propertyName))
					{
						if (!gridViewColumnHeader.IsInternalGenerated || column.Header is GridViewColumnHeader)
						{
							int index = base.InternalChildren.IndexOf(gridViewColumnHeader);
							this.RemoveHeader(gridViewColumnHeader, -1);
							GridViewColumnHeader gridViewColumnHeader2 = this.CreateAndInsertHeader(column, index);
							this.BuildHeaderLinks();
							return;
						}
						this.UpdateHeaderContent(gridViewColumnHeader);
						return;
					}
					else
					{
						DependencyProperty columnDPFromName = GridViewHeaderRowPresenter.GetColumnDPFromName(propertyName);
						if (columnDPFromName != null)
						{
							this.UpdateHeaderProperty(gridViewColumnHeader, columnDPFromName);
						}
					}
				}
			}
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0015CEE0 File Offset: 0x0015B0E0
		internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			base.OnColumnCollectionChanged(e);
			UIElementCollection internalChildren = base.InternalChildren;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				int visualIndex = this.GetVisualIndex(e.NewStartingIndex);
				GridViewColumn column = (GridViewColumn)e.NewItems[0];
				this.CreateAndInsertHeader(column, visualIndex + 1);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				this.RemoveHeader(null, this.GetVisualIndex(e.OldStartingIndex));
				break;
			case NotifyCollectionChangedAction.Replace:
			{
				int visualIndex = this.GetVisualIndex(e.OldStartingIndex);
				this.RemoveHeader(null, visualIndex);
				GridViewColumn column = (GridViewColumn)e.NewItems[0];
				this.CreateAndInsertHeader(column, visualIndex);
				break;
			}
			case NotifyCollectionChangedAction.Move:
			{
				int visualIndex2 = this.GetVisualIndex(e.OldStartingIndex);
				int visualIndex3 = this.GetVisualIndex(e.NewStartingIndex);
				GridViewColumnHeader element = (GridViewColumnHeader)internalChildren[visualIndex2];
				internalChildren.RemoveAt(visualIndex2);
				internalChildren.InsertInternal(visualIndex3, element);
				break;
			}
			case NotifyCollectionChangedAction.Reset:
			{
				int count = e.ClearedColumns.Count;
				for (int i = 0; i < count; i++)
				{
					this.RemoveHeader(null, 1);
				}
				break;
			}
			}
			this.BuildHeaderLinks();
			this._isColumnChangedOrCreated = true;
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0015D00C File Offset: 0x0015B20C
		internal void MakeParentItemsControlGotFocus()
		{
			if (this._itemsControl != null && !this._itemsControl.IsKeyboardFocusWithin)
			{
				ListBox listBox = this._itemsControl as ListBox;
				if (listBox != null && listBox.LastActionItem != null)
				{
					listBox.LastActionItem.Focus();
					return;
				}
				this._itemsControl.Focus();
			}
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0015D060 File Offset: 0x0015B260
		internal void UpdateHeaderProperty(GridViewColumnHeader header, DependencyProperty property)
		{
			DependencyProperty gvDP;
			DependencyProperty columnDP;
			DependencyProperty targetDP;
			GridViewHeaderRowPresenter.GetMatchingDPs(property, out gvDP, out columnDP, out targetDP);
			this.UpdateHeaderProperty(header, targetDP, columnDP, gvDP);
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for the column header row.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" /> object for this column header row. </returns>
		// Token: 0x06004D80 RID: 19840 RVA: 0x0015D083 File Offset: 0x0015B283
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridViewHeaderRowPresenterAutomationPeer(this);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0015D08C File Offset: 0x0015B28C
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			bool flag = false;
			GridViewColumnCollection columns = base.Columns;
			if (columns != null)
			{
				foreach (GridViewColumn gridViewColumn in columns)
				{
					if (gridViewColumn.State != ColumnMeasureState.SpecificWidth)
					{
						if (gridViewColumn.State == ColumnMeasureState.Init)
						{
							gridViewColumn.State = ColumnMeasureState.Headered;
						}
						if (base.DesiredWidthList == null || gridViewColumn.ActualIndex >= base.DesiredWidthList.Count)
						{
							flag = true;
							break;
						}
						if (!DoubleUtil.AreClose(gridViewColumn.DesiredWidth, base.DesiredWidthList[gridViewColumn.ActualIndex]))
						{
							base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.DesiredWidth;
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				base.InvalidateMeasure();
			}
			base.LayoutUpdated -= this.OnLayoutUpdated;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0015D168 File Offset: 0x0015B368
		private int GetVisualIndex(int columnIndex)
		{
			return base.InternalChildren.Count - 3 - columnIndex;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0015D188 File Offset: 0x0015B388
		private void BuildHeaderLinks()
		{
			GridViewColumnHeader previousVisualHeader = null;
			if (base.Columns != null)
			{
				for (int i = 0; i < base.Columns.Count; i++)
				{
					GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)base.InternalChildren[this.GetVisualIndex(i)];
					gridViewColumnHeader.PreviousVisualHeader = previousVisualHeader;
					previousVisualHeader = gridViewColumnHeader;
				}
			}
			if (this._paddingHeader != null)
			{
				this._paddingHeader.PreviousVisualHeader = previousVisualHeader;
			}
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0015D1EC File Offset: 0x0015B3EC
		private GridViewColumnHeader CreateAndInsertHeader(GridViewColumn column, int index)
		{
			object header = column.Header;
			GridViewColumnHeader gridViewColumnHeader = header as GridViewColumnHeader;
			if (header != null)
			{
				DependencyObject dependencyObject = header as DependencyObject;
				if (dependencyObject != null)
				{
					Visual visual = dependencyObject as Visual;
					if (visual != null)
					{
						Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual;
						if (visual2 != null)
						{
							if (gridViewColumnHeader != null)
							{
								GridViewHeaderRowPresenter gridViewHeaderRowPresenter = visual2 as GridViewHeaderRowPresenter;
								if (gridViewHeaderRowPresenter != null)
								{
									gridViewHeaderRowPresenter.InternalChildren.RemoveNoVerify(gridViewColumnHeader);
								}
							}
							else
							{
								GridViewColumnHeader gridViewColumnHeader2 = visual2 as GridViewColumnHeader;
								if (gridViewColumnHeader2 != null)
								{
									gridViewColumnHeader2.ClearValue(ContentControl.ContentProperty);
								}
							}
						}
					}
					DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
					if (parent != null)
					{
						LogicalTreeHelper.RemoveLogicalChild(parent, header);
					}
				}
			}
			if (gridViewColumnHeader == null)
			{
				gridViewColumnHeader = new GridViewColumnHeader();
				gridViewColumnHeader.IsInternalGenerated = true;
			}
			gridViewColumnHeader.SetValue(GridViewColumnHeader.ColumnPropertyKey, column);
			this.HookupItemsControlKeyboardEvent(gridViewColumnHeader);
			base.InternalChildren.InsertInternal(index, gridViewColumnHeader);
			this.UpdateHeader(gridViewColumnHeader);
			this._gvHeadersValid = false;
			return gridViewColumnHeader;
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0015D2BA File Offset: 0x0015B4BA
		private void RemoveHeader(GridViewColumnHeader header, int index)
		{
			this._gvHeadersValid = false;
			if (header != null)
			{
				base.InternalChildren.Remove(header);
			}
			else
			{
				header = (GridViewColumnHeader)base.InternalChildren[index];
				base.InternalChildren.RemoveAt(index);
			}
			this.UnhookItemsControlKeyboardEvent(header);
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0015D2FC File Offset: 0x0015B4FC
		private void RenewEvents()
		{
			ScrollViewer headerSV = this._headerSV;
			this._headerSV = (base.Parent as ScrollViewer);
			if (headerSV != this._headerSV)
			{
				if (headerSV != null)
				{
					headerSV.ScrollChanged -= this.OnHeaderScrollChanged;
				}
				if (this._headerSV != null)
				{
					this._headerSV.ScrollChanged += this.OnHeaderScrollChanged;
				}
			}
			ScrollViewer mainSV = this._mainSV;
			this._mainSV = (base.TemplatedParent as ScrollViewer);
			if (mainSV != this._mainSV)
			{
				if (mainSV != null)
				{
					mainSV.ScrollChanged -= this.OnMasterScrollChanged;
				}
				if (this._mainSV != null)
				{
					this._mainSV.ScrollChanged += this.OnMasterScrollChanged;
				}
			}
			ItemsControl itemsControl = this._itemsControl;
			this._itemsControl = GridViewHeaderRowPresenter.FindItemsControlThroughTemplatedParent(this);
			if (itemsControl != this._itemsControl)
			{
				if (itemsControl != null)
				{
					itemsControl.KeyDown -= this.OnColumnHeadersPresenterKeyDown;
				}
				if (this._itemsControl != null)
				{
					this._itemsControl.KeyDown += this.OnColumnHeadersPresenterKeyDown;
				}
			}
			ListView listView = this._itemsControl as ListView;
			if (listView != null && listView.View != null && listView.View is GridView)
			{
				((GridView)listView.View).HeaderRowPresenter = this;
			}
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0015D438 File Offset: 0x0015B638
		private void UnhookItemsControlKeyboardEvent(GridViewColumnHeader header)
		{
			if (this._itemsControl != null)
			{
				this._itemsControl.KeyDown -= header.OnColumnHeaderKeyDown;
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0015D459 File Offset: 0x0015B659
		private void HookupItemsControlKeyboardEvent(GridViewColumnHeader header)
		{
			if (this._itemsControl != null)
			{
				this._itemsControl.KeyDown += header.OnColumnHeaderKeyDown;
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0015D47A File Offset: 0x0015B67A
		private void OnMasterScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (this._headerSV != null && this._mainSV == e.OriginalSource)
			{
				this._headerSV.ScrollToHorizontalOffset(e.HorizontalOffset);
			}
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0015D4A3 File Offset: 0x0015B6A3
		private void OnHeaderScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (this._mainSV != null && this._headerSV == e.OriginalSource)
			{
				this._mainSV.ScrollToHorizontalOffset(e.HorizontalOffset);
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0015D4CC File Offset: 0x0015B6CC
		private void AddPaddingColumnHeader()
		{
			GridViewColumnHeader gridViewColumnHeader = new GridViewColumnHeader();
			gridViewColumnHeader.IsInternalGenerated = true;
			gridViewColumnHeader.SetValue(GridViewColumnHeader.RolePropertyKey, GridViewColumnHeaderRole.Padding);
			gridViewColumnHeader.Content = null;
			gridViewColumnHeader.ContentTemplate = null;
			gridViewColumnHeader.ContentTemplateSelector = null;
			gridViewColumnHeader.MinWidth = 0.0;
			gridViewColumnHeader.Padding = new Thickness(0.0);
			gridViewColumnHeader.Width = double.NaN;
			gridViewColumnHeader.HorizontalAlignment = HorizontalAlignment.Stretch;
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				gridViewColumnHeader.Focusable = false;
			}
			base.InternalChildren.AddInternal(gridViewColumnHeader);
			this._paddingHeader = gridViewColumnHeader;
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0015D568 File Offset: 0x0015B768
		private void AddIndicator()
		{
			Separator separator = new Separator();
			separator.Visibility = Visibility.Hidden;
			separator.Margin = new Thickness(0.0);
			separator.Width = 2.0;
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Border));
			frameworkElementFactory.SetValue(Border.BackgroundProperty, new SolidColorBrush(Color.FromUInt32(4278190208U)));
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Separator));
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			separator.Template = controlTemplate;
			base.InternalChildren.AddInternal(separator);
			this._indicator = separator;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0015D608 File Offset: 0x0015B808
		private void AddFloatingHeader(GridViewColumnHeader srcHeader)
		{
			Type type = (srcHeader != null) ? srcHeader.GetType() : typeof(GridViewColumnHeader);
			GridViewColumnHeader gridViewColumnHeader;
			try
			{
				gridViewColumnHeader = (Activator.CreateInstance(type) as GridViewColumnHeader);
			}
			catch (MissingMethodException innerException)
			{
				throw new ArgumentException(SR.Get("ListView_MissingParameterlessConstructor", new object[]
				{
					type
				}), innerException);
			}
			gridViewColumnHeader.IsInternalGenerated = true;
			gridViewColumnHeader.SetValue(GridViewColumnHeader.RolePropertyKey, GridViewColumnHeaderRole.Floating);
			gridViewColumnHeader.Visibility = Visibility.Hidden;
			base.InternalChildren.AddInternal(gridViewColumnHeader);
			this._floatingHeader = gridViewColumnHeader;
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0015D698 File Offset: 0x0015B898
		private void UpdateFloatingHeader(GridViewColumnHeader srcHeader)
		{
			this._floatingHeader.Style = srcHeader.Style;
			this._floatingHeader.FloatSourceHeader = srcHeader;
			this._floatingHeader.Width = srcHeader.ActualWidth;
			this._floatingHeader.Height = srcHeader.ActualHeight;
			this._floatingHeader.SetValue(GridViewColumnHeader.ColumnPropertyKey, srcHeader.Column);
			this._floatingHeader.Visibility = Visibility.Hidden;
			this._floatingHeader.MinWidth = srcHeader.MinWidth;
			this._floatingHeader.MinHeight = srcHeader.MinHeight;
			object obj = srcHeader.ReadLocalValue(ContentControl.ContentTemplateProperty);
			if (obj != DependencyProperty.UnsetValue && obj != null)
			{
				this._floatingHeader.ContentTemplate = srcHeader.ContentTemplate;
			}
			object obj2 = srcHeader.ReadLocalValue(ContentControl.ContentTemplateSelectorProperty);
			if (obj2 != DependencyProperty.UnsetValue && obj2 != null)
			{
				this._floatingHeader.ContentTemplateSelector = srcHeader.ContentTemplateSelector;
			}
			if (!(srcHeader.Content is Visual))
			{
				this._floatingHeader.Content = srcHeader.Content;
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0015D796 File Offset: 0x0015B996
		private bool CheckStartHeaderDrag(Point currentPos, Point originalPos)
		{
			return DoubleUtil.GreaterThan(Math.Abs(currentPos.X - originalPos.X), 4.0);
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0015D7BC File Offset: 0x0015B9BC
		private static ItemsControl FindItemsControlThroughTemplatedParent(GridViewHeaderRowPresenter presenter)
		{
			FrameworkElement frameworkElement = presenter.TemplatedParent as FrameworkElement;
			ItemsControl itemsControl = null;
			while (frameworkElement != null)
			{
				itemsControl = (frameworkElement as ItemsControl);
				if (itemsControl != null)
				{
					break;
				}
				frameworkElement = (frameworkElement.TemplatedParent as FrameworkElement);
			}
			return itemsControl;
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0015D7F4 File Offset: 0x0015B9F4
		private void OnColumnHeadersPresenterKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this._isHeaderDragging)
			{
				GridViewColumnHeader draggingSrcHeader = this._draggingSrcHeader;
				this.FinishHeaderDrag(true);
				this.PrepareHeaderDrag(draggingSrcHeader, this._currentPos, this._relativeStartPos, true);
				base.InvalidateArrange();
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0015D83C File Offset: 0x0015BA3C
		private GridViewColumnHeader FindHeaderByColumn(GridViewColumn column)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			if (columns != null && internalChildren.Count > columns.Count)
			{
				int num = columns.IndexOf(column);
				if (num != -1)
				{
					int visualIndex = this.GetVisualIndex(num);
					GridViewColumnHeader gridViewColumnHeader = internalChildren[visualIndex] as GridViewColumnHeader;
					if (gridViewColumnHeader.Column == column)
					{
						return gridViewColumnHeader;
					}
					for (int i = 1; i < internalChildren.Count; i++)
					{
						gridViewColumnHeader = (internalChildren[i] as GridViewColumnHeader);
						if (gridViewColumnHeader != null && gridViewColumnHeader.Column == column)
						{
							return gridViewColumnHeader;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0015D8D0 File Offset: 0x0015BAD0
		private int FindIndexByPosition(Point startPos, bool findNearestColumn)
		{
			int num = -1;
			if (startPos.X < 0.0)
			{
				return 0;
			}
			int i = 0;
			while (i < this.HeadersPositionList.Count)
			{
				num++;
				Rect rect = this.HeadersPositionList[i];
				double x = rect.X;
				double num2 = x + rect.Width;
				if (DoubleUtil.GreaterThanOrClose(startPos.X, x) && DoubleUtil.LessThanOrClose(startPos.X, num2))
				{
					if (!findNearestColumn)
					{
						break;
					}
					double value = (x + num2) * 0.5;
					if (DoubleUtil.GreaterThanOrClose(startPos.X, value) && i != this.HeadersPositionList.Count - 1)
					{
						num++;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			return num;
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0015D98C File Offset: 0x0015BB8C
		private Point FindPositionByIndex(int index)
		{
			return new Point(this.HeadersPositionList[index].X, 0.0);
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0015D9BC File Offset: 0x0015BBBC
		private void UpdateHeader(GridViewColumnHeader header)
		{
			this.UpdateHeaderContent(header);
			int i = 0;
			int num = GridViewHeaderRowPresenter.s_DPList[0].Length;
			while (i < num)
			{
				this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.s_DPList[2][i], GridViewHeaderRowPresenter.s_DPList[1][i], GridViewHeaderRowPresenter.s_DPList[0][i]);
				i++;
			}
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x0015DA08 File Offset: 0x0015BC08
		private void UpdateHeaderContent(GridViewColumnHeader header)
		{
			if (header != null && header.IsInternalGenerated)
			{
				GridViewColumn column = header.Column;
				if (column != null)
				{
					if (column.Header == null)
					{
						header.ClearValue(ContentControl.ContentProperty);
						return;
					}
					header.Content = column.Header;
				}
			}
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0015DA4A File Offset: 0x0015BC4A
		private void UpdatePaddingHeader(GridViewColumnHeader header)
		{
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty);
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty);
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty);
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0015DA70 File Offset: 0x0015BC70
		private void UpdateAllHeaders(DependencyProperty dp)
		{
			DependencyProperty gvDP;
			DependencyProperty columnDP;
			DependencyProperty targetDP;
			GridViewHeaderRowPresenter.GetMatchingDPs(dp, out gvDP, out columnDP, out targetDP);
			int num;
			int num2;
			this.GetIndexRange(dp, out num, out num2);
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = num; i <= num2; i++)
			{
				GridViewColumnHeader gridViewColumnHeader = internalChildren[i] as GridViewColumnHeader;
				if (gridViewColumnHeader != null)
				{
					this.UpdateHeaderProperty(gridViewColumnHeader, targetDP, columnDP, gvDP);
				}
			}
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0015DACC File Offset: 0x0015BCCC
		private void GetIndexRange(DependencyProperty dp, out int iStart, out int iEnd)
		{
			iStart = ((dp == GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty || dp == GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty || dp == GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty) ? 1 : 0);
			iEnd = base.InternalChildren.Count - 3;
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0015DAFC File Offset: 0x0015BCFC
		private void UpdateHeaderProperty(GridViewColumnHeader header, DependencyProperty targetDP, DependencyProperty columnDP, DependencyProperty gvDP)
		{
			if (gvDP == GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty && header.Role == GridViewColumnHeaderRole.Padding)
			{
				Style columnHeaderContainerStyle = this.ColumnHeaderContainerStyle;
				if (columnHeaderContainerStyle != null && !columnHeaderContainerStyle.TargetType.IsAssignableFrom(typeof(GridViewColumnHeader)))
				{
					header.Style = null;
					return;
				}
			}
			GridViewColumn column = header.Column;
			object obj = null;
			if (column != null && columnDP != null)
			{
				obj = column.GetValue(columnDP);
			}
			if (obj == null)
			{
				obj = base.GetValue(gvDP);
			}
			header.UpdateProperty(targetDP, obj);
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x0015DB70 File Offset: 0x0015BD70
		private void PrepareHeaderDrag(GridViewColumnHeader header, Point pos, Point relativePos, bool cancelInvoke)
		{
			if (header.Role == GridViewColumnHeaderRole.Normal)
			{
				this._prepareDragging = true;
				this._isHeaderDragging = false;
				this._draggingSrcHeader = header;
				this._startPos = pos;
				this._relativeStartPos = relativePos;
				if (!cancelInvoke)
				{
					this._startColumnIndex = this.FindIndexByPosition(this._startPos, false);
				}
			}
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0015DBC0 File Offset: 0x0015BDC0
		private void StartHeaderDrag()
		{
			this._startPos = this._currentPos;
			this._isHeaderDragging = true;
			this._draggingSrcHeader.SuppressClickEvent = true;
			if (base.Columns != null)
			{
				base.Columns.BlockWrite();
			}
			base.InternalChildren.Remove(this._floatingHeader);
			this.AddFloatingHeader(this._draggingSrcHeader);
			this.UpdateFloatingHeader(this._draggingSrcHeader);
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0015DC28 File Offset: 0x0015BE28
		private void FinishHeaderDrag(bool isCancel)
		{
			this._prepareDragging = false;
			this._isHeaderDragging = false;
			this._draggingSrcHeader.SuppressClickEvent = false;
			this._floatingHeader.Visibility = Visibility.Hidden;
			this._floatingHeader.ResetFloatingHeaderCanvasBackground();
			this._indicator.Visibility = Visibility.Hidden;
			if (base.Columns != null)
			{
				base.Columns.UnblockWrite();
			}
			if (!isCancel)
			{
				bool flag = GridViewHeaderRowPresenter.IsMousePositionValid(this._floatingHeader, this._currentPos, 2.0);
				int newIndex = (this._startColumnIndex >= this._desColumnIndex) ? this._desColumnIndex : (this._desColumnIndex - 1);
				if (flag)
				{
					base.Columns.Move(this._startColumnIndex, newIndex);
				}
			}
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x0015DCD7 File Offset: 0x0015BED7
		private static bool IsMousePositionValid(FrameworkElement floatingHeader, Point currentPos, double arrange)
		{
			return DoubleUtil.LessThanOrClose(-floatingHeader.Height * arrange, currentPos.Y) && DoubleUtil.LessThanOrClose(currentPos.Y, floatingHeader.Height * (arrange + 1.0));
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0015DD10 File Offset: 0x0015BF10
		internal List<GridViewColumnHeader> ActualColumnHeaders
		{
			get
			{
				if (this._gvHeaders == null || !this._gvHeadersValid)
				{
					this._gvHeadersValid = true;
					this._gvHeaders = new List<GridViewColumnHeader>();
					if (base.Columns != null)
					{
						UIElementCollection internalChildren = base.InternalChildren;
						int i = 0;
						int count = base.Columns.Count;
						while (i < count)
						{
							GridViewColumnHeader gridViewColumnHeader = internalChildren[this.GetVisualIndex(i)] as GridViewColumnHeader;
							if (gridViewColumnHeader != null)
							{
								this._gvHeaders.Add(gridViewColumnHeader);
							}
							i++;
						}
					}
				}
				return this._gvHeaders;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x0015DD8E File Offset: 0x0015BF8E
		private List<Rect> HeadersPositionList
		{
			get
			{
				if (this._headersPositionList == null)
				{
					this._headersPositionList = new List<Rect>();
				}
				return this._headersPositionList;
			}
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0015DDAC File Offset: 0x0015BFAC
		private static DependencyProperty GetColumnDPFromName(string dpName)
		{
			foreach (DependencyProperty dependencyProperty in GridViewHeaderRowPresenter.s_DPList[1])
			{
				if (dependencyProperty != null && dpName.Equals(dependencyProperty.Name))
				{
					return dependencyProperty;
				}
			}
			return null;
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0015DDE8 File Offset: 0x0015BFE8
		private static void GetMatchingDPs(DependencyProperty indexDP, out DependencyProperty gvDP, out DependencyProperty columnDP, out DependencyProperty headerDP)
		{
			for (int i = 0; i < GridViewHeaderRowPresenter.s_DPList.Length; i++)
			{
				for (int j = 0; j < GridViewHeaderRowPresenter.s_DPList[i].Length; j++)
				{
					if (indexDP == GridViewHeaderRowPresenter.s_DPList[i][j])
					{
						gvDP = GridViewHeaderRowPresenter.s_DPList[0][j];
						columnDP = GridViewHeaderRowPresenter.s_DPList[1][j];
						headerDP = GridViewHeaderRowPresenter.s_DPList[2][j];
						return;
					}
				}
			}
			DependencyProperty dependencyProperty;
			headerDP = (dependencyProperty = null);
			columnDP = (dependencyProperty = dependencyProperty);
			gvDP = dependencyProperty;
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0015DE60 File Offset: 0x0015C060
		// Note: this type is marked as 'beforefieldinit'.
		static GridViewHeaderRowPresenter()
		{
			DependencyProperty[][] array = new DependencyProperty[3][];
			array[0] = new DependencyProperty[]
			{
				GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty,
				GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty,
				GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty,
				GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty,
				GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty,
				GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty
			};
			int num = 1;
			DependencyProperty[] array2 = new DependencyProperty[6];
			array2[0] = GridViewColumn.HeaderContainerStyleProperty;
			array2[1] = GridViewColumn.HeaderTemplateProperty;
			array2[2] = GridViewColumn.HeaderTemplateSelectorProperty;
			array2[3] = GridViewColumn.HeaderStringFormatProperty;
			array[num] = array2;
			array[2] = new DependencyProperty[]
			{
				FrameworkElement.StyleProperty,
				ContentControl.ContentTemplateProperty,
				ContentControl.ContentTemplateSelectorProperty,
				ContentControl.ContentStringFormatProperty,
				FrameworkElement.ContextMenuProperty,
				FrameworkElement.ToolTipProperty
			};
			GridViewHeaderRowPresenter.s_DPList = array;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContainerStyle" /> dependency property. </summary>
		// Token: 0x04002B6A RID: 11114
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = GridView.ColumnHeaderContainerStyleProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplate" /> dependency property. </summary>
		// Token: 0x04002B6B RID: 11115
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = GridView.ColumnHeaderTemplateProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplateSelector" /> dependency property.</summary>
		// Token: 0x04002B6C RID: 11116
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = GridView.ColumnHeaderTemplateSelectorProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002B6D RID: 11117
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = GridView.ColumnHeaderStringFormatProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.AllowsColumnReorder" /> dependency property. </summary>
		// Token: 0x04002B6E RID: 11118
		public static readonly DependencyProperty AllowsColumnReorderProperty = GridView.AllowsColumnReorderProperty.AddOwner(typeof(GridViewHeaderRowPresenter));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContextMenu" /> dependency property. </summary>
		// Token: 0x04002B6F RID: 11119
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = GridView.ColumnHeaderContextMenuProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderToolTip" /> dependency property. </summary>
		// Token: 0x04002B70 RID: 11120
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = GridView.ColumnHeaderToolTipProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x04002B71 RID: 11121
		private bool _gvHeadersValid;

		// Token: 0x04002B72 RID: 11122
		private List<GridViewColumnHeader> _gvHeaders;

		// Token: 0x04002B73 RID: 11123
		private List<Rect> _headersPositionList;

		// Token: 0x04002B74 RID: 11124
		private ScrollViewer _mainSV;

		// Token: 0x04002B75 RID: 11125
		private ScrollViewer _headerSV;

		// Token: 0x04002B76 RID: 11126
		private GridViewColumnHeader _paddingHeader;

		// Token: 0x04002B77 RID: 11127
		private GridViewColumnHeader _floatingHeader;

		// Token: 0x04002B78 RID: 11128
		private Separator _indicator;

		// Token: 0x04002B79 RID: 11129
		private ItemsControl _itemsControl;

		// Token: 0x04002B7A RID: 11130
		private GridViewColumnHeader _draggingSrcHeader;

		// Token: 0x04002B7B RID: 11131
		private Point _startPos;

		// Token: 0x04002B7C RID: 11132
		private Point _relativeStartPos;

		// Token: 0x04002B7D RID: 11133
		private Point _currentPos;

		// Token: 0x04002B7E RID: 11134
		private int _startColumnIndex;

		// Token: 0x04002B7F RID: 11135
		private int _desColumnIndex;

		// Token: 0x04002B80 RID: 11136
		private bool _isHeaderDragging;

		// Token: 0x04002B81 RID: 11137
		private bool _isColumnChangedOrCreated;

		// Token: 0x04002B82 RID: 11138
		private bool _prepareDragging;

		// Token: 0x04002B83 RID: 11139
		private const double c_thresholdX = 4.0;

		// Token: 0x04002B84 RID: 11140
		private static readonly DependencyProperty[][] s_DPList;
	}
}
