using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> row.</summary>
	// Token: 0x020004B4 RID: 1204
	public class DataGridRow : Control
	{
		// Token: 0x0600492E RID: 18734 RVA: 0x0014BD24 File Offset: 0x00149F24
		static DataGridRow()
		{
			DataGridRow.SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(DataGridRow));
			DataGridRow.UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(DataGridRow));
			DataGridRow.IsEditingPropertyKey = DependencyProperty.RegisterReadOnly("IsEditing", typeof(bool), typeof(DataGridRow), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));
			DataGridRow.IsEditingProperty = DataGridRow.IsEditingPropertyKey.DependencyProperty;
			DataGridRow.IsNewItemPropertyKey = DependencyProperty.RegisterReadOnly("IsNewItem", typeof(bool), typeof(DataGridRow), new FrameworkPropertyMetadata(false));
			DataGridRow.IsNewItemProperty = DataGridRow.IsNewItemPropertyKey.DependencyProperty;
			UIElement.VisibilityProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridRow.OnCoerceVisibility)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(typeof(DataGridRow)));
			DataGridRow.ItemsPanelProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DataGridCellsPanel)))));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(false));
			Control.BackgroundProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceBackground)));
			FrameworkElement.BindingGroupProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged)));
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(DataGridRow), new UIPropertyMetadata(new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));
			VirtualizingPanel.ShouldCacheContainerSizeProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridRow.OnCoerceShouldCacheContainerSize)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridRow), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRow" /> class. </summary>
		// Token: 0x0600492F RID: 18735 RVA: 0x0014C287 File Offset: 0x0014A487
		public DataGridRow()
		{
			this._tracker = new ContainerTracking<DataGridRow>(this);
		}

		/// <summary>Gets or sets the data item that the row represents. </summary>
		/// <returns>The data item that the row represents. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x0014C29B File Offset: 0x0014A49B
		// (set) Token: 0x06004931 RID: 18737 RVA: 0x0014C2A8 File Offset: 0x0014A4A8
		public object Item
		{
			get
			{
				return base.GetValue(DataGridRow.ItemProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ItemProperty, value);
			}
		}

		/// <summary>Updates the displayed cells when the <see cref="P:System.Windows.Controls.DataGridRow.Item" /> property value has changed. </summary>
		/// <param name="oldItem">The previous value of the <see cref="P:System.Windows.Controls.DataGridRow.Item" /> property.</param>
		/// <param name="newItem">The new value of the <see cref="P:System.Windows.Controls.DataGridRow.Item" /> property.</param>
		// Token: 0x06004932 RID: 18738 RVA: 0x0014C2B8 File Offset: 0x0014A4B8
		protected virtual void OnItemChanged(object oldItem, object newItem)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.Item = newItem;
			}
		}

		/// <summary>Gets or sets the template that defines the panel that controls the layout of cells in the row. </summary>
		/// <returns>The template that defines the panel to use for the layout of cells in the row. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x06004933 RID: 18739 RVA: 0x0014C2D6 File Offset: 0x0014A4D6
		// (set) Token: 0x06004934 RID: 18740 RVA: 0x0014C2E8 File Offset: 0x0014A4E8
		public ItemsPanelTemplate ItemsPanel
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(DataGridRow.ItemsPanelProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ItemsPanelProperty, value);
			}
		}

		/// <summary>Called whenever the control's template changes.</summary>
		/// <param name="oldTemplate">The old template.</param>
		/// <param name="newTemplate">The new template.</param>
		// Token: 0x06004935 RID: 18741 RVA: 0x0014C2F6 File Offset: 0x0014A4F6
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.CellsPresenter = null;
			this.DetailsPresenter = null;
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x06004936 RID: 18742 RVA: 0x0014C310 File Offset: 0x0014A510
		private bool IsDataGridKeyboardFocusWithin
		{
			get
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				return dataGridOwner != null && dataGridOwner.IsKeyboardFocusWithin;
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x0014C330 File Offset: 0x0014A530
		internal override void ChangeVisualState(bool useTransitions)
		{
			byte b = 0;
			if (this.IsSelected || this.IsEditing)
			{
				b += 8;
			}
			if (this.IsEditing)
			{
				b += 4;
			}
			if (base.IsMouseOver)
			{
				b += 2;
			}
			if (this.IsDataGridKeyboardFocusWithin)
			{
				b += 1;
			}
			for (byte b2 = DataGridRow._idealStateMapping[(int)b]; b2 != 255; b2 = DataGridRow._fallbackStateMapping[(int)b2])
			{
				string stateName;
				if (b2 == 5)
				{
					if (this.AlternationIndex % 2 == 1)
					{
						stateName = "Normal_AlternatingRow";
					}
					else
					{
						stateName = "Normal";
					}
				}
				else
				{
					stateName = DataGridRow._stateNames[(int)b2];
				}
				if (VisualStateManager.GoToState(this, stateName, useTransitions))
				{
					break;
				}
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Gets or sets an object that represents the row header contents. </summary>
		/// <returns>The row header contents. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x0014C3CD File Offset: 0x0014A5CD
		// (set) Token: 0x06004939 RID: 18745 RVA: 0x0014C3DA File Offset: 0x0014A5DA
		public object Header
		{
			get
			{
				return base.GetValue(DataGridRow.HeaderProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderProperty, value);
			}
		}

		/// <summary>Called when the value of the <see cref="P:System.Windows.Controls.DataGridRow.Header" /> property has changed.</summary>
		/// <param name="oldHeader">The previous value of the <see cref="P:System.Windows.Controls.DataGridRow.Header" /> property.</param>
		/// <param name="newHeader">The new value of the <see cref="P:System.Windows.Controls.DataGridRow.Header" /> property. </param>
		// Token: 0x0600493A RID: 18746 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
		}

		/// <summary>Gets or sets the style that is used when rendering the row header. </summary>
		/// <returns>The style that is used when rendering the row header. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x0014C3E8 File Offset: 0x0014A5E8
		// (set) Token: 0x0600493C RID: 18748 RVA: 0x0014C3FA File Offset: 0x0014A5FA
		public Style HeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridRow.HeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the template that is used to display the row header. </summary>
		/// <returns>The template that is used to display the row header or <see langword="null" /> to use the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderTemplate" /> setting. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x0600493D RID: 18749 RVA: 0x0014C408 File Offset: 0x0014A608
		// (set) Token: 0x0600493E RID: 18750 RVA: 0x0014C41A File Offset: 0x0014A61A
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridRow.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets a template selector that provides custom logic for choosing a row header template. </summary>
		/// <returns>A template selector for choosing the row header template. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x0600493F RID: 18751 RVA: 0x0014C428 File Offset: 0x0014A628
		// (set) Token: 0x06004940 RID: 18752 RVA: 0x0014C43A File Offset: 0x0014A63A
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridRow.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridRow.HeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets the template that is used to visually indicate an error in row validation. </summary>
		/// <returns>The template that is used to visually indicate an error in row validation, or <see langword="null" /> to use the <see cref="P:System.Windows.Controls.DataGrid.RowValidationErrorTemplate" /> setting. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06004941 RID: 18753 RVA: 0x0014C448 File Offset: 0x0014A648
		// (set) Token: 0x06004942 RID: 18754 RVA: 0x0014C45A File Offset: 0x0014A65A
		public ControlTemplate ValidationErrorTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(DataGridRow.ValidationErrorTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.ValidationErrorTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the template that is used to display the details section of the row. </summary>
		/// <returns>The template that is used to display the row details section or <see langword="null" /> to use the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsTemplate" /> setting. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06004943 RID: 18755 RVA: 0x0014C468 File Offset: 0x0014A668
		// (set) Token: 0x06004944 RID: 18756 RVA: 0x0014C47A File Offset: 0x0014A67A
		public DataTemplate DetailsTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridRow.DetailsTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets a template selector that provides custom logic for choosing a row details template. </summary>
		/// <returns>A template selector for choosing the row details template. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x06004945 RID: 18757 RVA: 0x0014C488 File Offset: 0x0014A688
		// (set) Token: 0x06004946 RID: 18758 RVA: 0x0014C49A File Offset: 0x0014A69A
		public DataTemplateSelector DetailsTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridRow.DetailsTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates when the details section of the row is displayed. </summary>
		/// <returns>A value that specifies the visibility of the row details. The registered default is <see cref="F:System.Windows.Visibility.Collapsed" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x06004947 RID: 18759 RVA: 0x0014C4A8 File Offset: 0x0014A6A8
		// (set) Token: 0x06004948 RID: 18760 RVA: 0x0014C4BA File Offset: 0x0014A6BA
		public Visibility DetailsVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridRow.DetailsVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridRow.DetailsVisibilityProperty, value);
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x06004949 RID: 18761 RVA: 0x0014C4CD File Offset: 0x0014A6CD
		// (set) Token: 0x0600494A RID: 18762 RVA: 0x0014C4D5 File Offset: 0x0014A6D5
		internal bool DetailsLoaded
		{
			get
			{
				return this._detailsLoaded;
			}
			set
			{
				this._detailsLoaded = value;
			}
		}

		/// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.Controls.DataGridRow" /> has been updated. </summary>
		/// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
		// Token: 0x0600494B RID: 18763 RVA: 0x0014C4DE File Offset: 0x0014A6DE
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DataGridRow.AlternationIndexProperty)
			{
				this.NotifyPropertyChanged(this, e, DataGridNotificationTarget.Rows);
			}
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x0014C504 File Offset: 0x0014A704
		internal void PrepareRow(object item, DataGrid owningDataGrid)
		{
			bool flag = this._owner != owningDataGrid;
			bool forcePrepareCells = false;
			this._owner = owningDataGrid;
			if (this != item)
			{
				if (this.Item != item)
				{
					this.Item = item;
				}
				else
				{
					forcePrepareCells = true;
				}
			}
			if (this.IsEditing)
			{
				this.IsEditing = false;
			}
			if (flag)
			{
				this.SyncProperties(forcePrepareCells);
			}
			base.CoerceValue(VirtualizingPanel.ShouldCacheContainerSizeProperty);
			base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.DelayedValidateWithoutUpdate), DispatcherPriority.DataBind, new object[]
			{
				base.BindingGroup
			});
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x0014C58C File Offset: 0x0014A78C
		internal void ClearRow(DataGrid owningDataGrid)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				this.PersistAttachedItemValue(cellsPresenter, FrameworkElement.HeightProperty);
			}
			this.PersistAttachedItemValue(this, DataGridRow.DetailsVisibilityProperty);
			this.Item = BindingExpressionBase.DisconnectedItem;
			DataGridDetailsPresenter detailsPresenter = this.DetailsPresenter;
			if (detailsPresenter != null)
			{
				detailsPresenter.Content = BindingExpressionBase.DisconnectedItem;
			}
			this._owner = null;
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x0014C5E4 File Offset: 0x0014A7E4
		private void PersistAttachedItemValue(DependencyObject objectWithProperty, DependencyProperty property)
		{
			if (DependencyPropertyHelper.GetValueSource(objectWithProperty, property).BaseValueSource == BaseValueSource.Local)
			{
				this._owner.ItemAttachedStorage.SetValue(this.Item, property, objectWithProperty.GetValue(property));
				objectWithProperty.ClearValue(property);
			}
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x0014C62C File Offset: 0x0014A82C
		private void RestoreAttachedItemValue(DependencyObject objectWithProperty, DependencyProperty property)
		{
			object value;
			if (this._owner.ItemAttachedStorage.TryGetValue(this.Item, property, out value))
			{
				objectWithProperty.SetValue(property, value);
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x06004950 RID: 18768 RVA: 0x0014C65C File Offset: 0x0014A85C
		internal ContainerTracking<DataGridRow> Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x0014C664 File Offset: 0x0014A864
		internal void OnRowResizeStarted()
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				this._cellsPresenterResizeHeight = cellsPresenter.Height;
			}
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x0014C688 File Offset: 0x0014A888
		internal void OnRowResize(double changeAmount)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				double num = cellsPresenter.ActualHeight + changeAmount;
				double num2 = Math.Max(this.RowHeader.DesiredSize.Height, base.MinHeight);
				if (DoubleUtil.LessThan(num, num2))
				{
					num = num2;
				}
				double maxHeight = base.MaxHeight;
				if (DoubleUtil.GreaterThan(num, maxHeight))
				{
					num = maxHeight;
				}
				cellsPresenter.Height = num;
			}
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0014C6EC File Offset: 0x0014A8EC
		internal void OnRowResizeCompleted(bool canceled)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null && canceled)
			{
				cellsPresenter.Height = this._cellsPresenterResizeHeight;
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x0014C714 File Offset: 0x0014A914
		internal void OnRowResizeReset()
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.ClearValue(FrameworkElement.HeightProperty);
				if (this._owner != null)
				{
					this._owner.ItemAttachedStorage.ClearValue(this.Item, FrameworkElement.HeightProperty);
				}
			}
		}

		/// <summary>Called to update the displayed cells when the <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection has changed. </summary>
		/// <param name="columns">The <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection.</param>
		/// <param name="e">The event data from the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event of the <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection.</param>
		// Token: 0x06004955 RID: 18773 RVA: 0x0014C75C File Offset: 0x0014A95C
		protected internal virtual void OnColumnsChanged(ObservableCollection<DataGridColumn> columns, NotifyCollectionChangedEventArgs e)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.OnColumnsChanged(columns, e);
			}
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x0014C77C File Offset: 0x0014A97C
		private static object OnCoerceHeaderStyle(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderStyleProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderStyleProperty);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0014C7A8 File Offset: 0x0014A9A8
		private static object OnCoerceHeaderTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderTemplateProperty);
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x0014C7D4 File Offset: 0x0014A9D4
		private static object OnCoerceHeaderTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.HeaderTemplateSelectorProperty, dataGridRow.DataGridOwner, DataGrid.RowHeaderTemplateSelectorProperty);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x0014C800 File Offset: 0x0014AA00
		private static object OnCoerceBackground(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			object result = baseValue;
			int alternationIndex = dataGridRow.AlternationIndex;
			if (alternationIndex != 0)
			{
				if (alternationIndex == 1)
				{
					result = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, Control.BackgroundProperty, dataGridRow.DataGridOwner, DataGrid.AlternatingRowBackgroundProperty);
				}
			}
			else
			{
				result = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, Control.BackgroundProperty, dataGridRow.DataGridOwner, DataGrid.RowBackgroundProperty);
			}
			return result;
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x0014C85C File Offset: 0x0014AA5C
		private static object OnCoerceValidationErrorTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.ValidationErrorTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowValidationErrorTemplateProperty);
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x0014C888 File Offset: 0x0014AA88
		private static object OnCoerceDetailsTemplate(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsTemplateProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsTemplateProperty);
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x0014C8B4 File Offset: 0x0014AAB4
		private static object OnCoerceDetailsTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsTemplateSelectorProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsTemplateSelectorProperty);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x0014C8E0 File Offset: 0x0014AAE0
		private static object OnCoerceDetailsVisibility(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			object obj = DataGridHelper.GetCoercedTransferPropertyValue(dataGridRow, baseValue, DataGridRow.DetailsVisibilityProperty, dataGridRow.DataGridOwner, DataGrid.RowDetailsVisibilityModeProperty);
			if (obj is DataGridRowDetailsVisibilityMode)
			{
				DataGridRowDetailsVisibilityMode dataGridRowDetailsVisibilityMode = (DataGridRowDetailsVisibilityMode)obj;
				bool flag = dataGridRow.DetailsTemplate != null || dataGridRow.DetailsTemplateSelector != null;
				bool flag2 = dataGridRow.Item != CollectionView.NewItemPlaceholder;
				switch (dataGridRowDetailsVisibilityMode)
				{
				case DataGridRowDetailsVisibilityMode.Collapsed:
					obj = Visibility.Collapsed;
					break;
				case DataGridRowDetailsVisibilityMode.Visible:
					obj = ((flag && flag2) ? Visibility.Visible : Visibility.Collapsed);
					break;
				case DataGridRowDetailsVisibilityMode.VisibleWhenSelected:
					obj = ((dataGridRow.IsSelected && flag && flag2) ? Visibility.Visible : Visibility.Collapsed);
					break;
				default:
					obj = Visibility.Collapsed;
					break;
				}
			}
			return obj;
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x0014C994 File Offset: 0x0014AB94
		private static object OnCoerceVisibility(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridRow.Item == CollectionView.NewItemPlaceholder && dataGridOwner != null)
			{
				return dataGridOwner.PlaceholderVisibility;
			}
			return baseValue;
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x0014C9CC File Offset: 0x0014ABCC
		private static object OnCoerceShouldCacheContainerSize(DependencyObject d, object baseValue)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			if (dataGridRow.Item == CollectionView.NewItemPlaceholder)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0014C9F5 File Offset: 0x0014ABF5
		private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0014CA09 File Offset: 0x0014AC09
		private static void OnNotifyRowAndRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DataGridRow).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x0014CA20 File Offset: 0x0014AC20
		private static void OnNotifyDetailsTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
			if (dataGridRow.DetailsLoaded && d.GetValue(e.Property) == e.NewValue)
			{
				if (dataGridRow.DataGridOwner != null)
				{
					dataGridRow.DataGridOwner.OnUnloadingRowDetailsWrapper(dataGridRow);
				}
				if (e.NewValue != null)
				{
					Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGrid.DelayedOnLoadingRowDetails), DispatcherPriority.Loaded, new object[]
					{
						dataGridRow
					});
				}
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x0014CAA0 File Offset: 0x0014ACA0
		private static void OnNotifyDetailsVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)d;
			Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGridRow.DelayedRowDetailsVisibilityChanged), DispatcherPriority.Loaded, new object[]
			{
				dataGridRow
			});
			dataGridRow.NotifyPropertyChanged(d, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x0014CAE4 File Offset: 0x0014ACE4
		private static object DelayedRowDetailsVisibilityChanged(object arg)
		{
			DataGridRow dataGridRow = (DataGridRow)arg;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			FrameworkElement detailsElement = (dataGridRow.DetailsPresenter != null) ? dataGridRow.DetailsPresenter.DetailsElement : null;
			if (dataGridOwner != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(dataGridRow, detailsElement);
				dataGridOwner.OnRowDetailsVisibilityChanged(e);
			}
			return null;
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x06004965 RID: 18789 RVA: 0x0014CB29 File Offset: 0x0014AD29
		// (set) Token: 0x06004966 RID: 18790 RVA: 0x0014CB31 File Offset: 0x0014AD31
		internal DataGridCellsPresenter CellsPresenter
		{
			get
			{
				return this._cellsPresenter;
			}
			set
			{
				this._cellsPresenter = value;
			}
		}

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06004967 RID: 18791 RVA: 0x0014CB3A File Offset: 0x0014AD3A
		// (set) Token: 0x06004968 RID: 18792 RVA: 0x0014CB42 File Offset: 0x0014AD42
		internal DataGridDetailsPresenter DetailsPresenter
		{
			get
			{
				return this._detailsPresenter;
			}
			set
			{
				this._detailsPresenter = value;
			}
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x06004969 RID: 18793 RVA: 0x0014CB4B File Offset: 0x0014AD4B
		// (set) Token: 0x0600496A RID: 18794 RVA: 0x0014CB53 File Offset: 0x0014AD53
		internal DataGridRowHeader RowHeader
		{
			get
			{
				return this._rowHeader;
			}
			set
			{
				this._rowHeader = value;
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x0014CB5C File Offset: 0x0014AD5C
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x0014CB6C File Offset: 0x0014AD6C
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyRows(target))
			{
				if (e.Property == DataGrid.RowBackgroundProperty || e.Property == DataGrid.AlternatingRowBackgroundProperty || e.Property == Control.BackgroundProperty || e.Property == DataGridRow.AlternationIndexProperty)
				{
					DataGridHelper.TransferProperty(this, Control.BackgroundProperty);
				}
				else if (e.Property == DataGrid.RowHeaderStyleProperty || e.Property == DataGridRow.HeaderStyleProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderStyleProperty);
				}
				else if (e.Property == DataGrid.RowHeaderTemplateProperty || e.Property == DataGridRow.HeaderTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateProperty);
				}
				else if (e.Property == DataGrid.RowHeaderTemplateSelectorProperty || e.Property == DataGridRow.HeaderTemplateSelectorProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateSelectorProperty);
				}
				else if (e.Property == DataGrid.RowValidationErrorTemplateProperty || e.Property == DataGridRow.ValidationErrorTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.ValidationErrorTemplateProperty);
				}
				else if (e.Property == DataGrid.RowDetailsTemplateProperty || e.Property == DataGridRow.DetailsTemplateProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateProperty);
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGrid.RowDetailsTemplateSelectorProperty || e.Property == DataGridRow.DetailsTemplateSelectorProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateSelectorProperty);
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGrid.RowDetailsVisibilityModeProperty || e.Property == DataGridRow.DetailsVisibilityProperty || e.Property == DataGridRow.IsSelectedProperty)
				{
					DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
				}
				else if (e.Property == DataGridRow.ItemProperty)
				{
					this.OnItemChanged(e.OldValue, e.NewValue);
				}
				else if (e.Property == DataGridRow.HeaderProperty)
				{
					this.OnHeaderChanged(e.OldValue, e.NewValue);
				}
				else if (e.Property == FrameworkElement.BindingGroupProperty)
				{
					base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.DelayedValidateWithoutUpdate), DispatcherPriority.DataBind, new object[]
					{
						e.NewValue
					});
				}
				else if (e.Property == DataGridRow.IsEditingProperty || e.Property == UIElement.IsMouseOverProperty || e.Property == UIElement.IsKeyboardFocusWithinProperty)
				{
					base.UpdateVisualState();
				}
			}
			if (DataGridHelper.ShouldNotifyDetailsPresenter(target) && this.DetailsPresenter != null)
			{
				this.DetailsPresenter.NotifyPropertyChanged(d, e);
			}
			if (DataGridHelper.ShouldNotifyCellsPresenter(target) || DataGridHelper.ShouldNotifyCells(target) || DataGridHelper.ShouldRefreshCellContent(target))
			{
				DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
				if (cellsPresenter != null)
				{
					cellsPresenter.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
			if (DataGridHelper.ShouldNotifyRowHeaders(target) && this.RowHeader != null)
			{
				this.RowHeader.NotifyPropertyChanged(d, e);
			}
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x0014CE48 File Offset: 0x0014B048
		private object DelayedValidateWithoutUpdate(object arg)
		{
			BindingGroup bindingGroup = (BindingGroup)arg;
			if (bindingGroup != null && bindingGroup.Items.Count > 0)
			{
				bindingGroup.ValidateWithoutUpdate();
			}
			return null;
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x0014CE78 File Offset: 0x0014B078
		private void SyncProperties(bool forcePrepareCells)
		{
			DataGridHelper.TransferProperty(this, Control.BackgroundProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderStyleProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.HeaderTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.ValidationErrorTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, DataGridRow.DetailsVisibilityProperty);
			base.CoerceValue(UIElement.VisibilityProperty);
			this.RestoreAttachedItemValue(this, DataGridRow.DetailsVisibilityProperty);
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.SyncProperties(forcePrepareCells);
				this.RestoreAttachedItemValue(cellsPresenter, FrameworkElement.HeightProperty);
			}
			if (this.DetailsPresenter != null)
			{
				this.DetailsPresenter.SyncProperties();
			}
			if (this.RowHeader != null)
			{
				this.RowHeader.SyncProperties();
			}
		}

		/// <summary>Gets the index of the row within a set of alternating rows.</summary>
		/// <returns>The index of the row within a set of alternating rows. The registered default is 0. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x0014CF37 File Offset: 0x0014B137
		public int AlternationIndex
		{
			get
			{
				return (int)base.GetValue(DataGridRow.AlternationIndexProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the row is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the row is selected; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06004970 RID: 18800 RVA: 0x0014CF49 File Offset: 0x0014B149
		// (set) Token: 0x06004971 RID: 18801 RVA: 0x0014CF5B File Offset: 0x0014B15B
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsSelectedProperty);
			}
			set
			{
				base.SetValue(DataGridRow.IsSelectedProperty, value);
			}
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x0014CF6C File Offset: 0x0014B16C
		private static void OnIsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = (DataGridRow)sender;
			bool flag = (bool)e.NewValue;
			if (flag && !dataGridRow.IsSelectable)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridOwner != null && dataGridRow.DataContext != null)
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(dataGridOwner) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(dataGridRow.DataContext) as DataGridItemAutomationPeer;
					if (dataGridItemAutomationPeer != null)
					{
						dataGridItemAutomationPeer.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, (bool)e.OldValue, flag);
					}
				}
			}
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
			dataGridRow.RaiseSelectionChangedEvent(flag);
			dataGridRow.UpdateVisualState();
			dataGridRow.NotifyPropertyChanged(dataGridRow, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0014D02A File Offset: 0x0014B22A
		private void RaiseSelectionChangedEvent(bool isSelected)
		{
			if (isSelected)
			{
				this.OnSelected(new RoutedEventArgs(DataGridRow.SelectedEvent, this));
				return;
			}
			this.OnUnselected(new RoutedEventArgs(DataGridRow.UnselectedEvent, this));
		}

		/// <summary>Occurs when the row is selected. </summary>
		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06004974 RID: 18804 RVA: 0x0014D052 File Offset: 0x0014B252
		// (remove) Token: 0x06004975 RID: 18805 RVA: 0x0014D060 File Offset: 0x0014B260
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(DataGridRow.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridRow.SelectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridRow.Selected" /> event when the <see cref="P:System.Windows.Controls.DataGridRow.IsSelected" /> property value changes to <see langword="true" />. </summary>
		/// <param name="e">The event data, which is empty when this method is called by the <see cref="T:System.Windows.Controls.DataGridRow" />.</param>
		// Token: 0x06004976 RID: 18806 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the row selection is cleared.</summary>
		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06004977 RID: 18807 RVA: 0x0014D06E File Offset: 0x0014B26E
		// (remove) Token: 0x06004978 RID: 18808 RVA: 0x0014D07C File Offset: 0x0014B27C
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(DataGridRow.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DataGridRow.UnselectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGridRow.Unselected" /> event when the <see cref="P:System.Windows.Controls.DataGridRow.IsSelected" /> property value changes to <see langword="false" />. </summary>
		/// <param name="e">The event data, which is empty when this method is called by the <see cref="T:System.Windows.Controls.DataGridRow" />.</param>
		// Token: 0x06004979 RID: 18809 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x0014D08C File Offset: 0x0014B28C
		private bool IsSelectable
		{
			get
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null)
				{
					DataGridSelectionUnit selectionUnit = dataGridOwner.SelectionUnit;
					return selectionUnit == DataGridSelectionUnit.FullRow || selectionUnit == DataGridSelectionUnit.CellOrRowHeader;
				}
				return true;
			}
		}

		/// <summary>Gets a value that indicates whether the row is in editing mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the row is in editing mode; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x0014D0B6 File Offset: 0x0014B2B6
		// (set) Token: 0x0600497C RID: 18812 RVA: 0x0014D0C8 File Offset: 0x0014B2C8
		public bool IsEditing
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsEditingProperty);
			}
			internal set
			{
				base.SetValue(DataGridRow.IsEditingPropertyKey, value);
			}
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Automation.Peers.DataGridRowAutomationPeer" /> for this row.</summary>
		/// <returns>A new automation peer for this row.</returns>
		// Token: 0x0600497D RID: 18813 RVA: 0x0014D0D6 File Offset: 0x0014B2D6
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridRowAutomationPeer(this);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x0014D0E0 File Offset: 0x0014B2E0
		internal void ScrollCellIntoView(int index)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				cellsPresenter.ScrollCellIntoView(index);
			}
		}

		/// <summary>Arranges the content of the row.</summary>
		/// <param name="arrangeBounds">The area that is available for the row. </param>
		/// <returns>The actual area used by the row.</returns>
		// Token: 0x0600497F RID: 18815 RVA: 0x0014D100 File Offset: 0x0014B300
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				dataGridOwner.QueueInvalidateCellsPanelHorizontalOffset();
			}
			return base.ArrangeOverride(arrangeBounds);
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.DataGridRow" /> is a placeholder for a new item or for an item that has not been committed.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.DataGridRow" /> is a placeholder for a new item or for an item that has not been committed; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x06004980 RID: 18816 RVA: 0x0014D124 File Offset: 0x0014B324
		// (set) Token: 0x06004981 RID: 18817 RVA: 0x0014D136 File Offset: 0x0014B336
		public bool IsNewItem
		{
			get
			{
				return (bool)base.GetValue(DataGridRow.IsNewItemProperty);
			}
			internal set
			{
				base.SetValue(DataGridRow.IsNewItemPropertyKey, value);
			}
		}

		/// <summary>Returns the index of the row's data item within the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of the <see cref="T:System.Windows.Controls.DataGrid" />. </summary>
		/// <returns>The index of the row's data item, or -1 if the item was not found. </returns>
		// Token: 0x06004982 RID: 18818 RVA: 0x0014D144 File Offset: 0x0014B344
		public int GetIndex()
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner != null)
			{
				return dataGridOwner.ItemContainerGenerator.IndexFromContainer(this);
			}
			return -1;
		}

		/// <summary>Returns the <see cref="T:System.Windows.Controls.DataGridRow" /> that contains the specified element. </summary>
		/// <param name="element">An element contained in a row to be found. </param>
		/// <returns>The <see cref="T:System.Windows.Controls.DataGridRow" /> that contains the specified element. </returns>
		// Token: 0x06004983 RID: 18819 RVA: 0x0014D169 File Offset: 0x0014B369
		public static DataGridRow GetRowContainingElement(FrameworkElement element)
		{
			return DataGridHelper.FindVisualParent<DataGridRow>(element);
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06004984 RID: 18820 RVA: 0x0014D171 File Offset: 0x0014B371
		internal DataGrid DataGridOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x0014D179 File Offset: 0x0014B379
		internal bool DetailsPresenterDrawsGridLines
		{
			get
			{
				return this._detailsPresenter != null && this._detailsPresenter.Visibility == Visibility.Visible;
			}
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x0014D194 File Offset: 0x0014B394
		internal DataGridCell TryGetCell(int index)
		{
			DataGridCellsPresenter cellsPresenter = this.CellsPresenter;
			if (cellsPresenter != null)
			{
				return cellsPresenter.ItemContainerGenerator.ContainerFromIndex(index) as DataGridCell;
			}
			return null;
		}

		// Token: 0x040029E6 RID: 10726
		private const byte DATAGRIDROW_stateMouseOverCode = 0;

		// Token: 0x040029E7 RID: 10727
		private const byte DATAGRIDROW_stateMouseOverEditingCode = 1;

		// Token: 0x040029E8 RID: 10728
		private const byte DATAGRIDROW_stateMouseOverEditingFocusedCode = 2;

		// Token: 0x040029E9 RID: 10729
		private const byte DATAGRIDROW_stateMouseOverSelectedCode = 3;

		// Token: 0x040029EA RID: 10730
		private const byte DATAGRIDROW_stateMouseOverSelectedFocusedCode = 4;

		// Token: 0x040029EB RID: 10731
		private const byte DATAGRIDROW_stateNormalCode = 5;

		// Token: 0x040029EC RID: 10732
		private const byte DATAGRIDROW_stateNormalEditingCode = 6;

		// Token: 0x040029ED RID: 10733
		private const byte DATAGRIDROW_stateNormalEditingFocusedCode = 7;

		// Token: 0x040029EE RID: 10734
		private const byte DATAGRIDROW_stateSelectedCode = 8;

		// Token: 0x040029EF RID: 10735
		private const byte DATAGRIDROW_stateSelectedFocusedCode = 9;

		// Token: 0x040029F0 RID: 10736
		private const byte DATAGRIDROW_stateNullCode = 255;

		// Token: 0x040029F1 RID: 10737
		private static byte[] _idealStateMapping = new byte[]
		{
			5,
			5,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			8,
			9,
			3,
			4,
			6,
			7,
			1,
			2
		};

		// Token: 0x040029F2 RID: 10738
		private static byte[] _fallbackStateMapping = new byte[]
		{
			5,
			2,
			7,
			4,
			9,
			byte.MaxValue,
			7,
			9,
			9,
			5
		};

		// Token: 0x040029F3 RID: 10739
		private static string[] _stateNames = new string[]
		{
			"MouseOver",
			"MouseOver_Unfocused_Editing",
			"MouseOver_Editing",
			"MouseOver_Unfocused_Selected",
			"MouseOver_Selected",
			"Normal",
			"Unfocused_Editing",
			"Normal_Editing",
			"Unfocused_Selected",
			"Normal_Selected"
		};

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.Item" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.Item" /> dependency property.</returns>
		// Token: 0x040029F4 RID: 10740
		public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(object), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.ItemsPanel" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.ItemsPanel" /> dependency property.</returns>
		// Token: 0x040029F5 RID: 10741
		public static readonly DependencyProperty ItemsPanelProperty = ItemsControl.ItemsPanelProperty.AddOwner(typeof(DataGridRow));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.Header" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.Header" /> dependency property.</returns>
		// Token: 0x040029F6 RID: 10742
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.HeaderStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.HeaderStyle" /> dependency property.</returns>
		// Token: 0x040029F7 RID: 10743
		public static readonly DependencyProperty HeaderStyleProperty = DependencyProperty.Register("HeaderStyle", typeof(Style), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderStyle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.HeaderTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.HeaderTemplate" /> dependency property.</returns>
		// Token: 0x040029F8 RID: 10744
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderTemplate)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.HeaderTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.HeaderTemplateSelector" /> dependency property.</returns>
		// Token: 0x040029F9 RID: 10745
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowAndRowHeaderPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceHeaderTemplateSelector)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.ValidationErrorTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.ValidationErrorTemplate" /> dependency property.</returns>
		// Token: 0x040029FA RID: 10746
		public static readonly DependencyProperty ValidationErrorTemplateProperty = DependencyProperty.Register("ValidationErrorTemplate", typeof(ControlTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyRowPropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceValidationErrorTemplate)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.DetailsTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.DetailsTemplate" /> dependency property.</returns>
		// Token: 0x040029FB RID: 10747
		public static readonly DependencyProperty DetailsTemplateProperty = DependencyProperty.Register("DetailsTemplate", typeof(DataTemplate), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsTemplatePropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsTemplate)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.DetailsTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.DetailsTemplateSelector" /> dependency property.</returns>
		// Token: 0x040029FC RID: 10748
		public static readonly DependencyProperty DetailsTemplateSelectorProperty = DependencyProperty.Register("DetailsTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridRow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsTemplatePropertyChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsTemplateSelector)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> dependency property.</returns>
		// Token: 0x040029FD RID: 10749
		public static readonly DependencyProperty DetailsVisibilityProperty = DependencyProperty.Register("DetailsVisibility", typeof(Visibility), typeof(DataGridRow), new FrameworkPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(DataGridRow.OnNotifyDetailsVisibilityChanged), new CoerceValueCallback(DataGridRow.OnCoerceDetailsVisibility)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.AlternationIndex" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.AlternationIndex" /> dependency property.</returns>
		// Token: 0x040029FE RID: 10750
		public static readonly DependencyProperty AlternationIndexProperty = ItemsControl.AlternationIndexProperty.AddOwner(typeof(DataGridRow));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.IsSelected" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.IsSelected" /> dependency property.</returns>
		// Token: 0x040029FF RID: 10751
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(DataGridRow), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(DataGridRow.OnIsSelectedChanged)));

		// Token: 0x04002A02 RID: 10754
		private static readonly DependencyPropertyKey IsEditingPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.IsEditing" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.IsEditing" /> dependency property.</returns>
		// Token: 0x04002A03 RID: 10755
		public static readonly DependencyProperty IsEditingProperty;

		// Token: 0x04002A04 RID: 10756
		internal static readonly DependencyPropertyKey IsNewItemPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridRow.IsNewItem" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridRow.IsNewItem" /> dependency property.</returns>
		// Token: 0x04002A05 RID: 10757
		public static readonly DependencyProperty IsNewItemProperty;

		// Token: 0x04002A06 RID: 10758
		internal bool _detailsLoaded;

		// Token: 0x04002A07 RID: 10759
		private DataGrid _owner;

		// Token: 0x04002A08 RID: 10760
		private DataGridCellsPresenter _cellsPresenter;

		// Token: 0x04002A09 RID: 10761
		private DataGridDetailsPresenter _detailsPresenter;

		// Token: 0x04002A0A RID: 10762
		private DataGridRowHeader _rowHeader;

		// Token: 0x04002A0B RID: 10763
		private ContainerTracking<DataGridRow> _tracker;

		// Token: 0x04002A0C RID: 10764
		private double _cellsPresenterResizeHeight;
	}
}
