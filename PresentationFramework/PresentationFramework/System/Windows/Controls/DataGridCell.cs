using System;
using System.Collections.ObjectModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
	/// <summary>Represents a cell of a <see cref="T:System.Windows.Controls.DataGrid" /> control.</summary>
	// Token: 0x02000496 RID: 1174
	public class DataGridCell : ContentControl, IProvideDataGridColumn
	{
		// Token: 0x060046E2 RID: 18146 RVA: 0x00141854 File Offset: 0x0013FA54
		static DataGridCell()
		{
			DataGridCell.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridCell));
			DataGridCell.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridCell));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(typeof(DataGridCell)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridCell.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridCell.OnCoerceStyle)));
			UIElement.ClipProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridCell.OnCoerceClip)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridCell), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DataGridCell.OnAnyMouseLeftButtonDownThunk), true);
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(DataGridCell), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.LostFocusEvent, new RoutedEventHandler(DataGridCell.OnAnyLostFocus), true);
			EventManager.RegisterClassHandler(typeof(DataGridCell), UIElement.GotFocusEvent, new RoutedEventHandler(DataGridCell.OnAnyGotFocus), true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCell" /> class.</summary>
		// Token: 0x060046E3 RID: 18147 RVA: 0x00141B17 File Offset: 0x0013FD17
		public DataGridCell()
		{
			this._tracker = new ContainerTracking<DataGridCell>(this);
		}

		/// <summary>Returns the automation peer for this <see cref="T:System.Windows.Controls.DataGridCell" />.</summary>
		/// <returns>The automation peer for this <see cref="T:System.Windows.Controls.DataGridCell" />.</returns>
		// Token: 0x060046E4 RID: 18148 RVA: 0x00141B2B File Offset: 0x0013FD2B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridCellAutomationPeer(this);
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x00141B33 File Offset: 0x0013FD33
		internal void PrepareCell(object item, ItemsControl cellsPresenter, DataGridRow ownerRow)
		{
			this.PrepareCell(item, ownerRow, cellsPresenter.ItemContainerGenerator.IndexFromContainer(this));
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00141B4C File Offset: 0x0013FD4C
		internal void PrepareCell(object item, DataGridRow ownerRow, int index)
		{
			this._owner = ownerRow;
			DataGrid dataGridOwner = this._owner.DataGridOwner;
			if (dataGridOwner != null)
			{
				if (index >= 0 && index < dataGridOwner.Columns.Count)
				{
					DataGridColumn dataGridColumn = dataGridOwner.Columns[index];
					this.Column = dataGridColumn;
					base.TabIndex = dataGridColumn.DisplayIndex;
				}
				if (this.IsEditing)
				{
					this.IsEditing = false;
				}
				else if (!(base.Content is FrameworkElement))
				{
					this.BuildVisualTree();
					if (!this.NeedsVisualTree)
					{
						base.Content = item;
					}
				}
				bool isSelected = dataGridOwner.SelectedCellsInternal.Contains(this);
				this.SyncIsSelected(isSelected);
			}
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
			base.CoerceValue(UIElement.ClipProperty);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00141C09 File Offset: 0x0013FE09
		internal void ClearCell(DataGridRow ownerRow)
		{
			this._owner = null;
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x00141C12 File Offset: 0x0013FE12
		internal ContainerTracking<DataGridCell> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		/// <summary>Gets or sets the column that the cell is in.</summary>
		/// <returns>The column that the cell is in. </returns>
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x060046E9 RID: 18153 RVA: 0x00141C1A File Offset: 0x0013FE1A
		// (set) Token: 0x060046EA RID: 18154 RVA: 0x00141C2C File Offset: 0x0013FE2C
		public DataGridColumn Column
		{
			get
			{
				return (DataGridColumn)base.GetValue(DataGridCell.ColumnProperty);
			}
			internal set
			{
				base.SetValue(DataGridCell.ColumnPropertyKey, value);
			}
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x00141C3C File Offset: 0x0013FE3C
		private static void OnColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = sender as DataGridCell;
			if (dataGridCell != null)
			{
				dataGridCell.OnColumnChanged((DataGridColumn)e.OldValue, (DataGridColumn)e.NewValue);
			}
		}

		/// <summary>Called when the cell's <see cref="P:System.Windows.Controls.DataGridCell.Column" /> property changes. </summary>
		/// <param name="oldColumn">The old column definition.</param>
		/// <param name="newColumn">The new column definition.</param>
		// Token: 0x060046EC RID: 18156 RVA: 0x00141C71 File Offset: 0x0013FE71
		protected virtual void OnColumnChanged(DataGridColumn oldColumn, DataGridColumn newColumn)
		{
			base.Content = null;
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00141C90 File Offset: 0x0013FE90
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCell)d).NotifyPropertyChanged(d, string.Empty, e, DataGridNotificationTarget.Cells);
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x00141CA8 File Offset: 0x0013FEA8
		private static void OnNotifyIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)d;
			DataGrid dataGridOwner = dataGridCell.DataGridOwner;
			if ((bool)e.NewValue && dataGridOwner != null)
			{
				dataGridOwner.CancelEdit(dataGridCell);
			}
			CommandManager.InvalidateRequerySuggested();
			dataGridCell.NotifyPropertyChanged(d, string.Empty, e, DataGridNotificationTarget.Cells);
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00141CF0 File Offset: 0x0013FEF0
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			DataGridColumn dataGridColumn = d as DataGridColumn;
			if (dataGridColumn != null && dataGridColumn != this.Column)
			{
				return;
			}
			if (DataGridHelper.ShouldNotifyCells(target))
			{
				if (e.Property == DataGridColumn.WidthProperty)
				{
					DataGridHelper.OnColumnWidthChanged(this, e);
				}
				else if (e.Property == DataGrid.CellStyleProperty || e.Property == DataGridColumn.CellStyleProperty || e.Property == FrameworkElement.StyleProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				}
				else if (e.Property == DataGrid.IsReadOnlyProperty || e.Property == DataGridColumn.IsReadOnlyProperty || e.Property == DataGridCell.IsReadOnlyProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridCell.IsReadOnlyProperty);
				}
				else if (e.Property == DataGridColumn.DisplayIndexProperty)
				{
					base.TabIndex = dataGridColumn.DisplayIndex;
				}
				else if (e.Property == UIElement.IsKeyboardFocusWithinProperty)
				{
					base.UpdateVisualState();
				}
			}
			if (DataGridHelper.ShouldRefreshCellContent(target) && dataGridColumn != null && this.NeedsVisualTree)
			{
				if (!string.IsNullOrEmpty(propertyName))
				{
					dataGridColumn.RefreshCellContent(this, propertyName);
					return;
				}
				if (e.Property != null)
				{
					dataGridColumn.RefreshCellContent(this, e.Property.Name);
				}
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00141E18 File Offset: 0x00140018
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridCell dataGridCell = d as DataGridCell;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCell, baseValue, FrameworkElement.StyleProperty, dataGridCell.Column, DataGridColumn.CellStyleProperty, dataGridCell.DataGridOwner, DataGrid.CellStyleProperty);
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x00141E50 File Offset: 0x00140050
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.DataGridOwner == null)
			{
				return;
			}
			if (base.IsMouseOver)
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
			if (this.IsSelected)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unselected"
				});
			}
			if (this.DataGridOwner.IsKeyboardFocusWithin)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			if (this.IsCurrent)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Current",
					"Regular"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Regular", useTransitions);
			}
			if (this.IsEditing)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Editing",
					"Display"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Display", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00141F80 File Offset: 0x00140180
		internal void BuildVisualTree()
		{
			if (this.NeedsVisualTree)
			{
				DataGridColumn column = this.Column;
				if (column != null)
				{
					DataGridRow rowOwner = this.RowOwner;
					if (rowOwner != null)
					{
						BindingGroup bindingGroup = rowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							this.RemoveBindingExpressions(bindingGroup, base.Content as DependencyObject);
						}
					}
					FrameworkElement frameworkElement = column.BuildVisualTree(this.IsEditing, this.RowDataItem, this);
					FrameworkElement frameworkElement2 = base.Content as FrameworkElement;
					if (frameworkElement2 != null && frameworkElement2 != frameworkElement)
					{
						ContentPresenter contentPresenter = frameworkElement2 as ContentPresenter;
						if (contentPresenter == null)
						{
							frameworkElement2.SetValue(FrameworkElement.DataContextProperty, BindingExpressionBase.DisconnectedItem);
						}
						else
						{
							contentPresenter.Content = BindingExpressionBase.DisconnectedItem;
						}
					}
					base.Content = frameworkElement;
				}
			}
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x00142028 File Offset: 0x00140228
		private void RemoveBindingExpressions(BindingGroup bindingGroup, DependencyObject element)
		{
			if (element == null)
			{
				return;
			}
			Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
			BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
			bindingExpressions.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (DataGridHelper.BindingExpressionBelongsToElement<DataGridCell>(array[i], this))
				{
					bindingExpressions.Remove(array[i]);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the cell is in edit mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell is in edit mode; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x060046F4 RID: 18164 RVA: 0x00142077 File Offset: 0x00140277
		// (set) Token: 0x060046F5 RID: 18165 RVA: 0x00142089 File Offset: 0x00140289
		public bool IsEditing
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsEditingProperty);
			}
			set
			{
				base.SetValue(DataGridCell.IsEditingProperty, value);
			}
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00142097 File Offset: 0x00140297
		private static void OnIsEditingChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCell)sender).OnIsEditingChanged((bool)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.DataGridCell.IsEditing" /> property changes.</summary>
		/// <param name="isEditing">The new value of the <see cref="P:System.Windows.Controls.DataGridCell.IsEditing" /> property.</param>
		// Token: 0x060046F7 RID: 18167 RVA: 0x001420B0 File Offset: 0x001402B0
		protected virtual void OnIsEditingChanged(bool isEditing)
		{
			if (base.IsKeyboardFocusWithin && !base.IsKeyboardFocused)
			{
				base.Focus();
			}
			this.BuildVisualTree();
			base.UpdateVisualState();
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x001420D5 File Offset: 0x001402D5
		internal void NotifyCurrentCellContainerChanged()
		{
			base.UpdateVisualState();
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x001420E0 File Offset: 0x001402E0
		private bool IsCurrent
		{
			get
			{
				DataGridRow rowOwner = this.RowOwner;
				DataGridColumn column = this.Column;
				if (rowOwner != null && column != null)
				{
					DataGrid dataGridOwner = rowOwner.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsCurrent(rowOwner, column);
					}
				}
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the cell can be put in edit mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell cannot be put in edit mode; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />. </returns>
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x00142115 File Offset: 0x00140315
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsReadOnlyProperty);
			}
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00142128 File Offset: 0x00140328
		private static object OnCoerceIsReadOnly(DependencyObject d, object baseValue)
		{
			DataGridCell dataGridCell = d as DataGridCell;
			DataGridColumn column = dataGridCell.Column;
			DataGrid dataGridOwner = dataGridCell.DataGridOwner;
			return DataGridHelper.GetCoercedTransferPropertyValue(column, column.IsReadOnly, DataGridColumn.IsReadOnlyProperty, dataGridOwner, DataGrid.IsReadOnlyProperty);
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00142168 File Offset: 0x00140368
		private static void OnAnyLostFocus(object sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(e.OriginalSource as UIElement);
			if (dataGridCell != null && dataGridCell == sender)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null && !dataGridCell.IsKeyboardFocusWithin && dataGridOwner.FocusedCell == dataGridCell)
				{
					dataGridOwner.FocusedCell = null;
				}
			}
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x001421B0 File Offset: 0x001403B0
		private static void OnAnyGotFocus(object sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(e.OriginalSource as UIElement);
			if (dataGridCell != null && dataGridCell == sender)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.FocusedCell = dataGridCell;
				}
			}
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x001421E8 File Offset: 0x001403E8
		internal void BeginEdit(RoutedEventArgs e)
		{
			this.IsEditing = true;
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.BeginEdit(base.Content as FrameworkElement, e);
			}
			this.RaisePreparingCellForEdit(e);
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00142220 File Offset: 0x00140420
		internal void CancelEdit()
		{
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.CancelEdit(base.Content as FrameworkElement);
			}
			this.IsEditing = false;
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00142250 File Offset: 0x00140450
		internal bool CommitEdit()
		{
			bool flag = true;
			DataGridColumn column = this.Column;
			if (column != null)
			{
				flag = column.CommitEdit(base.Content as FrameworkElement);
			}
			if (flag)
			{
				this.IsEditing = false;
			}
			return flag;
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00142288 File Offset: 0x00140488
		private void RaisePreparingCellForEdit(RoutedEventArgs editingEventArgs)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				FrameworkElement editingElement = this.EditingElement;
				DataGridPreparingCellForEditEventArgs e = new DataGridPreparingCellForEditEventArgs(this.Column, this.RowOwner, editingEventArgs, editingElement);
				dataGridOwner.OnPreparingCellForEdit(e);
			}
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06004702 RID: 18178 RVA: 0x001422C1 File Offset: 0x001404C1
		internal FrameworkElement EditingElement
		{
			get
			{
				return base.Content as FrameworkElement;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the cell is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the cell is selected; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06004703 RID: 18179 RVA: 0x001422CE File Offset: 0x001404CE
		// (set) Token: 0x06004704 RID: 18180 RVA: 0x001422E0 File Offset: 0x001404E0
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridCell.IsSelectedProperty);
			}
			set
			{
				base.SetValue(DataGridCell.IsSelectedProperty, value);
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x001422F0 File Offset: 0x001404F0
		private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)sender;
			bool isSelected = (bool)e.NewValue;
			if (!dataGridCell._syncingIsSelected)
			{
				DataGrid dataGridOwner = dataGridCell.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.CellIsSelectedChanged(dataGridCell, isSelected);
				}
			}
			dataGridCell.RaiseSelectionChangedEvent(isSelected);
			dataGridCell.UpdateVisualState();
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x00142338 File Offset: 0x00140538
		internal void SyncIsSelected(bool isSelected)
		{
			bool syncingIsSelected = this._syncingIsSelected;
			this._syncingIsSelected = true;
			try
			{
				this.IsSelected = isSelected;
			}
			finally
			{
				this._syncingIsSelected = syncingIsSelected;
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x00142374 File Offset: 0x00140574
		private void RaiseSelectionChangedEvent(bool isSelected)
		{
			if (isSelected)
			{
				this.OnSelected(new RoutedEventArgs(DataGridCell.SelectedEvent, this));
				return;
			}
			this.OnUnselected(new RoutedEventArgs(DataGridCell.UnselectedEvent, this));
		}

		/// <summary>Occurs when the cell is selected.</summary>
		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06004708 RID: 18184 RVA: 0x0014239C File Offset: 0x0014059C
		// (remove) Token: 0x06004709 RID: 18185 RVA: 0x001423AA File Offset: 0x001405AA
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(DataGridCell.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridCell.SelectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridCell.Selected" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600470A RID: 18186 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the cell selection is cleared.</summary>
		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x0600470B RID: 18187 RVA: 0x001423B8 File Offset: 0x001405B8
		// (remove) Token: 0x0600470C RID: 18188 RVA: 0x001423C6 File Offset: 0x001405C6
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(DataGridCell.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridCell.UnselectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridCell.Unselected" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600470D RID: 18189 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Determines the desired size of the <see cref="T:System.Windows.Controls.DataGridCell" />. </summary>
		/// <param name="constraint">The maximum size that the cell can occupy.</param>
		/// <returns>The desired size of the <see cref="T:System.Windows.Controls.DataGridCell" />.</returns>
		// Token: 0x0600470E RID: 18190 RVA: 0x001423D4 File Offset: 0x001405D4
		protected override Size MeasureOverride(Size constraint)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			bool flag = DataGridHelper.IsGridLineVisible(dataGridOwner, true);
			bool flag2 = DataGridHelper.IsGridLineVisible(dataGridOwner, false);
			double num = 0.0;
			double num2 = 0.0;
			if (flag)
			{
				num = dataGridOwner.HorizontalGridLineThickness;
				constraint = DataGridHelper.SubtractFromSize(constraint, num, true);
			}
			if (flag2)
			{
				num2 = dataGridOwner.VerticalGridLineThickness;
				constraint = DataGridHelper.SubtractFromSize(constraint, num2, false);
			}
			Size result = base.MeasureOverride(constraint);
			if (flag)
			{
				result.Height += num;
			}
			if (flag2)
			{
				result.Width += num2;
			}
			return result;
		}

		/// <summary>Determines the final size and placement of the cell content.</summary>
		/// <param name="arrangeSize">The maximum size that the cell can occupy.</param>
		/// <returns>The final size of the control.</returns>
		// Token: 0x0600470F RID: 18191 RVA: 0x00142468 File Offset: 0x00140668
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			bool flag = DataGridHelper.IsGridLineVisible(dataGridOwner, true);
			bool flag2 = DataGridHelper.IsGridLineVisible(dataGridOwner, false);
			double num = 0.0;
			double num2 = 0.0;
			if (flag)
			{
				num = dataGridOwner.HorizontalGridLineThickness;
				arrangeSize = DataGridHelper.SubtractFromSize(arrangeSize, num, true);
			}
			if (flag2)
			{
				num2 = dataGridOwner.VerticalGridLineThickness;
				arrangeSize = DataGridHelper.SubtractFromSize(arrangeSize, num2, false);
			}
			Size result = base.ArrangeOverride(arrangeSize);
			if (flag)
			{
				result.Height += num;
			}
			if (flag2)
			{
				result.Width += num2;
			}
			return result;
		}

		/// <summary>Draws the cell and the right side gridline.</summary>
		/// <param name="drawingContext">The drawing instructions for the cell.</param>
		// Token: 0x06004710 RID: 18192 RVA: 0x001424FC File Offset: 0x001406FC
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGrid dataGridOwner = this.DataGridOwner;
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, false))
			{
				double verticalGridLineThickness = this.DataGridOwner.VerticalGridLineThickness;
				Rect rectangle = new Rect(new Size(verticalGridLineThickness, base.RenderSize.Height));
				rectangle.X = base.RenderSize.Width - verticalGridLineThickness;
				drawingContext.DrawRectangle(this.DataGridOwner.VerticalGridLinesBrush, null, rectangle);
			}
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle2 = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle2.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle2);
			}
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x001425C5 File Offset: 0x001407C5
		private static void OnAnyMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGridCell)sender).OnAnyMouseLeftButtonDown(e);
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x001425D4 File Offset: 0x001407D4
		private void OnAnyMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
			bool flag = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			if (isKeyboardFocusWithin && !flag && !e.Handled && this.IsSelected)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null)
				{
					dataGridOwner.HandleSelectionForCellInput(this, false, true, false);
					if (!this.IsEditing && !this.IsReadOnly)
					{
						dataGridOwner.BeginEdit(e);
						e.Handled = true;
						return;
					}
				}
			}
			else if (!isKeyboardFocusWithin || !this.IsSelected || flag)
			{
				if (!isKeyboardFocusWithin)
				{
					base.Focus();
				}
				DataGrid dataGridOwner2 = this.DataGridOwner;
				if (dataGridOwner2 != null)
				{
					dataGridOwner2.HandleSelectionForCellInput(this, Mouse.Captured == null, true, true);
				}
				e.Handled = true;
			}
		}

		/// <summary>Reports text composition.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004713 RID: 18195 RVA: 0x0014267C File Offset: 0x0014087C
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		/// <summary>Reports that a key was pressed.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004714 RID: 18196 RVA: 0x0014267C File Offset: 0x0014087C
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		/// <summary>Reports that a key was pressed.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004715 RID: 18197 RVA: 0x0014267C File Offset: 0x0014087C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.SendInputToColumn(e);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00142688 File Offset: 0x00140888
		private void SendInputToColumn(InputEventArgs e)
		{
			DataGridColumn column = this.Column;
			if (column != null)
			{
				column.OnInput(e);
			}
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x001426A8 File Offset: 0x001408A8
		private static object OnCoerceClip(DependencyObject d, object baseValue)
		{
			DataGridCell cell = (DataGridCell)d;
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

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x06004718 RID: 18200 RVA: 0x001426DC File Offset: 0x001408DC
		internal DataGrid DataGridOwner
		{
			get
			{
				if (this._owner != null)
				{
					DataGrid dataGrid = this._owner.DataGridOwner;
					if (dataGrid == null)
					{
						dataGrid = (ItemsControl.ItemsControlFromItemContainer(this._owner) as DataGrid);
					}
					return dataGrid;
				}
				return null;
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x06004719 RID: 18201 RVA: 0x00142714 File Offset: 0x00140914
		private Panel ParentPanel
		{
			get
			{
				return base.VisualParent as Panel;
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x00142721 File Offset: 0x00140921
		internal DataGridRow RowOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x0600471B RID: 18203 RVA: 0x0014272C File Offset: 0x0014092C
		internal object RowDataItem
		{
			get
			{
				DataGridRow rowOwner = this.RowOwner;
				if (rowOwner != null)
				{
					return rowOwner.Item;
				}
				return base.DataContext;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x00142750 File Offset: 0x00140950
		private DataGridCellsPresenter CellsPresenter
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as DataGridCellsPresenter;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x0600471D RID: 18205 RVA: 0x0014275D File Offset: 0x0014095D
		private bool NeedsVisualTree
		{
			get
			{
				return base.ContentTemplate == null && base.ContentTemplateSelector == null;
			}
		}

		// Token: 0x04002940 RID: 10560
		private static readonly DependencyPropertyKey ColumnPropertyKey = DependencyProperty.RegisterReadOnly("Column", typeof(DataGridColumn), typeof(DataGridCell), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridCell.OnColumnChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridCell.Column" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridCell.Column" /> dependency property.</returns>
		// Token: 0x04002941 RID: 10561
		public static readonly DependencyProperty ColumnProperty = DataGridCell.ColumnPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridCell.IsEditing" /> dependency property.</summary>
		/// <returns>Identifier for the <see cref="P:System.Windows.Controls.DataGridCell.IsEditing" /> dependency property.</returns>
		// Token: 0x04002942 RID: 10562
		public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnIsEditingChanged)));

		// Token: 0x04002943 RID: 10563
		private static readonly DependencyPropertyKey IsReadOnlyPropertyKey = DependencyProperty.RegisterReadOnly("IsReadOnly", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnNotifyIsReadOnlyChanged), new CoerceValueCallback(DataGridCell.OnCoerceIsReadOnly)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridCell.IsReadOnly" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridCell.IsReadOnly" /> dependency property.</returns>
		// Token: 0x04002944 RID: 10564
		public static readonly DependencyProperty IsReadOnlyProperty = DataGridCell.IsReadOnlyPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridCell.IsSelected" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridCell.IsSelected" /> dependency property.</returns>
		// Token: 0x04002945 RID: 10565
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DataGridCell), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCell.OnIsSelectedChanged)));

		// Token: 0x04002948 RID: 10568
		private DataGridRow _owner;

		// Token: 0x04002949 RID: 10569
		private ContainerTracking<DataGridCell> _tracker;

		// Token: 0x0400294A RID: 10570
		private bool _syncingIsSelected;
	}
}
