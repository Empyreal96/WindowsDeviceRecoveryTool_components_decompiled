using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an individual <see cref="T:System.Windows.Controls.DataGrid" /> row header. </summary>
	// Token: 0x02000580 RID: 1408
	[TemplatePart(Name = "PART_TopHeaderGripper", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_BottomHeaderGripper", Type = typeof(Thumb))]
	public class DataGridRowHeader : ButtonBase
	{
		// Token: 0x06005D31 RID: 23857 RVA: 0x001A3B3C File Offset: 0x001A1D3C
		static DataGridRowHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(typeof(DataGridRowHeader)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContent)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContentTemplate)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContentTemplateSelector)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceStyle)));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceWidth)));
			ButtonBase.ClickModeProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(ClickMode.Press));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(false));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Automation.Peers.DataGridRowHeaderAutomationPeer" /> for this row header.</summary>
		/// <returns>A new automation peer for this row header.</returns>
		// Token: 0x06005D32 RID: 23858 RVA: 0x001A3E2B File Offset: 0x001A202B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridRowHeaderAutomationPeer(this);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> used to paint the row header separator lines. </summary>
		/// <returns>The brush used to paint row header separator lines. </returns>
		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06005D33 RID: 23859 RVA: 0x001A3E33 File Offset: 0x001A2033
		// (set) Token: 0x06005D34 RID: 23860 RVA: 0x001A3E45 File Offset: 0x001A2045
		public Brush SeparatorBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridRowHeader.SeparatorBrushProperty);
			}
			set
			{
				base.SetValue(DataGridRowHeader.SeparatorBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the user interface (UI) visibility of the row header separator lines. </summary>
		/// <returns>The UI visibility of the row header separator lines. The default is <see cref="F:System.Windows.Visibility.Visible" />.</returns>
		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x06005D35 RID: 23861 RVA: 0x001A3E53 File Offset: 0x001A2053
		// (set) Token: 0x06005D36 RID: 23862 RVA: 0x001A3E65 File Offset: 0x001A2065
		public Visibility SeparatorVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridRowHeader.SeparatorVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridRowHeader.SeparatorVisibilityProperty, value);
			}
		}

		/// <summary>Measures the children of a <see cref="T:System.Windows.Controls.Primitives.DataGridRowHeader" /> to prepare for arranging them during the <see cref="M:System.Windows.Controls.Control.ArrangeOverride(System.Windows.Size)" /> pass. </summary>
		/// <param name="availableSize">The available size that this element can give to child elements. Indicates an upper limit that child elements should not exceed. </param>
		/// <returns>The size that the <see cref="T:System.Windows.Controls.Primitives.DataGridRowHeader" /> determines it needs during layout, based on its calculations of child object allocated sizes. </returns>
		// Token: 0x06005D37 RID: 23863 RVA: 0x001A3E78 File Offset: 0x001A2078
		protected override Size MeasureOverride(Size availableSize)
		{
			Size result = base.MeasureOverride(availableSize);
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner == null)
			{
				return result;
			}
			if (DoubleUtil.IsNaN(dataGridOwner.RowHeaderWidth) && result.Width > dataGridOwner.RowHeaderActualWidth)
			{
				dataGridOwner.RowHeaderActualWidth = result.Width;
			}
			return new Size(dataGridOwner.RowHeaderActualWidth, result.Height);
		}

		/// <summary>Builds the visual tree for the row header when a new template is applied. </summary>
		// Token: 0x06005D38 RID: 23864 RVA: 0x001A3ED4 File Offset: 0x001A20D4
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			DataGridRow parentRow = this.ParentRow;
			if (parentRow != null)
			{
				parentRow.RowHeader = this;
				this.SyncProperties();
			}
			this.HookupGripperEvents();
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x001A3F04 File Offset: 0x001A2104
		internal void SyncProperties()
		{
			DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.WidthProperty);
			base.CoerceValue(DataGridRowHeader.IsRowSelectedProperty);
			this.OnCanUserResizeRowsChanged();
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x001A3F59 File Offset: 0x001A2159
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridRowHeader)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06005D3B RID: 23867 RVA: 0x001A3F68 File Offset: 0x001A2168
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property == DataGridRow.HeaderProperty || e.Property == ContentControl.ContentProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderStyleProperty || e.Property == DataGridRow.HeaderStyleProperty || e.Property == FrameworkElement.StyleProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderTemplateProperty || e.Property == DataGridRow.HeaderTemplateProperty || e.Property == ContentControl.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderTemplateSelectorProperty || e.Property == DataGridRow.HeaderTemplateSelectorProperty || e.Property == ContentControl.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderWidthProperty || e.Property == FrameworkElement.WidthProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.WidthProperty);
				return;
			}
			if (e.Property == DataGridRow.IsSelectedProperty)
			{
				base.CoerceValue(DataGridRowHeader.IsRowSelectedProperty);
				return;
			}
			if (e.Property == DataGrid.CanUserResizeRowsProperty)
			{
				this.OnCanUserResizeRowsChanged();
				return;
			}
			if (e.Property == DataGrid.RowHeaderActualWidthProperty)
			{
				base.InvalidateMeasure();
				base.InvalidateArrange();
				UIElement uielement = base.Parent as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
					uielement.InvalidateArrange();
					return;
				}
			}
			else if (e.Property == DataGrid.CurrentItemProperty || e.Property == DataGridRow.IsEditingProperty || e.Property == UIElement.IsMouseOverProperty || e.Property == UIElement.IsKeyboardFocusWithinProperty)
			{
				base.UpdateVisualState();
			}
		}

		// Token: 0x06005D3C RID: 23868 RVA: 0x001A410C File Offset: 0x001A230C
		private static object OnCoerceContent(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentProperty, dataGridRowHeader.ParentRow, DataGridRow.HeaderProperty);
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x001A4138 File Offset: 0x001A2338
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			DataGridRow parentRow = dataGridRowHeader.ParentRow;
			DataGrid grandParentObject = (parentRow != null) ? parentRow.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentTemplateProperty, parentRow, DataGridRow.HeaderTemplateProperty, grandParentObject, DataGrid.RowHeaderTemplateProperty);
		}

		// Token: 0x06005D3E RID: 23870 RVA: 0x001A4178 File Offset: 0x001A2378
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			DataGridRow parentRow = dataGridRowHeader.ParentRow;
			DataGrid grandParentObject = (parentRow != null) ? parentRow.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentTemplateSelectorProperty, parentRow, DataGridRow.HeaderTemplateSelectorProperty, grandParentObject, DataGrid.RowHeaderTemplateSelectorProperty);
		}

		// Token: 0x06005D3F RID: 23871 RVA: 0x001A41B8 File Offset: 0x001A23B8
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, FrameworkElement.StyleProperty, dataGridRowHeader.ParentRow, DataGridRow.HeaderStyleProperty, dataGridRowHeader.DataGridOwner, DataGrid.RowHeaderStyleProperty);
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x001A41F0 File Offset: 0x001A23F0
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, FrameworkElement.WidthProperty, dataGridRowHeader.DataGridOwner, DataGrid.RowHeaderWidthProperty);
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x06005D41 RID: 23873 RVA: 0x001A421C File Offset: 0x001A241C
		private bool IsRowCurrent
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsCurrent(parentRow, null);
					}
				}
				return false;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x06005D42 RID: 23874 RVA: 0x001A4248 File Offset: 0x001A2448
		private bool IsRowEditing
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				return parentRow != null && parentRow.IsEditing;
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x06005D43 RID: 23875 RVA: 0x001A4268 File Offset: 0x001A2468
		private bool IsRowMouseOver
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				return parentRow != null && parentRow.IsMouseOver;
			}
		}

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x06005D44 RID: 23876 RVA: 0x001A4288 File Offset: 0x001A2488
		private bool IsDataGridKeyboardFocusWithin
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsKeyboardFocusWithin;
					}
				}
				return false;
			}
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x001A42B4 File Offset: 0x001A24B4
		internal override void ChangeVisualState(bool useTransitions)
		{
			byte b = 0;
			if (this.IsRowCurrent)
			{
				b += 16;
			}
			if (this.IsRowSelected || this.IsRowEditing)
			{
				b += 8;
			}
			if (this.IsRowEditing)
			{
				b += 4;
			}
			if (this.IsRowMouseOver)
			{
				b += 2;
			}
			if (this.IsDataGridKeyboardFocusWithin)
			{
				b += 1;
			}
			for (byte b2 = DataGridRowHeader._idealStateMapping[(int)b]; b2 != 255; b2 = DataGridRowHeader._fallbackStateMapping[(int)b2])
			{
				string stateName = DataGridRowHeader._stateNames[(int)b2];
				if (VisualStateManager.GoToState(this, stateName, useTransitions))
				{
					break;
				}
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		/// <summary>Gets a value that indicates whether the row is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the row is selected; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06005D46 RID: 23878 RVA: 0x001A4340 File Offset: 0x001A2540
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsRowSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridRowHeader.IsRowSelectedProperty);
			}
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x001A4354 File Offset: 0x001A2554
		private static object OnCoerceIsRowSelected(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = (DataGridRowHeader)d;
			DataGridRow parentRow = dataGridRowHeader.ParentRow;
			if (parentRow != null)
			{
				return parentRow.IsSelected;
			}
			return baseValue;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event and initiates row selection or drag operations. </summary>
		// Token: 0x06005D48 RID: 23880 RVA: 0x001A4380 File Offset: 0x001A2580
		protected override void OnClick()
		{
			base.OnClick();
			if (Mouse.Captured == this)
			{
				base.ReleaseMouseCapture();
			}
			DataGrid dataGridOwner = this.DataGridOwner;
			DataGridRow parentRow = this.ParentRow;
			if (dataGridOwner != null && parentRow != null)
			{
				dataGridOwner.HandleSelectionForRowHeaderAndDetailsInput(parentRow, true);
			}
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x001A43C0 File Offset: 0x001A25C0
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._topGripper = (base.GetTemplateChild("PART_TopHeaderGripper") as Thumb);
			this._bottomGripper = (base.GetTemplateChild("PART_BottomHeaderGripper") as Thumb);
			if (this._topGripper != null)
			{
				this._topGripper.DragStarted += this.OnRowHeaderGripperDragStarted;
				this._topGripper.DragDelta += this.OnRowHeaderResize;
				this._topGripper.DragCompleted += this.OnRowHeaderGripperDragCompleted;
				this._topGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetTopGripperVisibility();
			}
			if (this._bottomGripper != null)
			{
				this._bottomGripper.DragStarted += this.OnRowHeaderGripperDragStarted;
				this._bottomGripper.DragDelta += this.OnRowHeaderResize;
				this._bottomGripper.DragCompleted += this.OnRowHeaderGripperDragCompleted;
				this._bottomGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetBottomGripperVisibility();
			}
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x001A44D4 File Offset: 0x001A26D4
		private void UnhookGripperEvents()
		{
			if (this._topGripper != null)
			{
				this._topGripper.DragStarted -= this.OnRowHeaderGripperDragStarted;
				this._topGripper.DragDelta -= this.OnRowHeaderResize;
				this._topGripper.DragCompleted -= this.OnRowHeaderGripperDragCompleted;
				this._topGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._topGripper = null;
			}
			if (this._bottomGripper != null)
			{
				this._bottomGripper.DragStarted -= this.OnRowHeaderGripperDragStarted;
				this._bottomGripper.DragDelta -= this.OnRowHeaderResize;
				this._bottomGripper.DragCompleted -= this.OnRowHeaderGripperDragCompleted;
				this._bottomGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._bottomGripper = null;
			}
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x001A45B8 File Offset: 0x001A27B8
		private void SetTopGripperVisibility()
		{
			if (this._topGripper != null)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				DataGridRow parentRow = this.ParentRow;
				if (dataGridOwner != null && parentRow != null && dataGridOwner.CanUserResizeRows && dataGridOwner.Items.Count > 1 && parentRow.Item != dataGridOwner.Items[0])
				{
					this._topGripper.Visibility = Visibility.Visible;
					return;
				}
				this._topGripper.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x001A4624 File Offset: 0x001A2824
		private void SetBottomGripperVisibility()
		{
			if (this._bottomGripper != null)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null && dataGridOwner.CanUserResizeRows)
				{
					this._bottomGripper.Visibility = Visibility.Visible;
					return;
				}
				this._bottomGripper.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06005D4D RID: 23885 RVA: 0x001A4664 File Offset: 0x001A2864
		private DataGridRow PreviousRow
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						int num = dataGridOwner.ItemContainerGenerator.IndexFromContainer(parentRow);
						if (num > 0)
						{
							return (DataGridRow)dataGridOwner.ItemContainerGenerator.ContainerFromIndex(num - 1);
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x001A46AB File Offset: 0x001A28AB
		private DataGridRow RowToResize(object gripper)
		{
			if (gripper != this._bottomGripper)
			{
				return this.PreviousRow;
			}
			return this.ParentRow;
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x001A46C4 File Offset: 0x001A28C4
		private void OnRowHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeStarted();
				e.Handled = true;
			}
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x001A46EC File Offset: 0x001A28EC
		private void OnRowHeaderResize(object sender, DragDeltaEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResize(e.VerticalChange);
				e.Handled = true;
			}
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x001A4718 File Offset: 0x001A2918
		private void OnRowHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeCompleted(e.Canceled);
				e.Handled = true;
			}
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x001A4744 File Offset: 0x001A2944
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeReset();
				e.Handled = true;
			}
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x001A4769 File Offset: 0x001A2969
		private void OnCanUserResizeRowsChanged()
		{
			this.SetTopGripperVisibility();
			this.SetBottomGripperVisibility();
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06005D54 RID: 23892 RVA: 0x001A13CB File Offset: 0x0019F5CB
		internal DataGridRow ParentRow
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06005D55 RID: 23893 RVA: 0x001A4778 File Offset: 0x001A2978
		private DataGrid DataGridOwner
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					return parentRow.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x04002FF7 RID: 12279
		private const byte DATAGRIDROWHEADER_stateMouseOverCode = 0;

		// Token: 0x04002FF8 RID: 12280
		private const byte DATAGRIDROWHEADER_stateMouseOverCurrentRowCode = 1;

		// Token: 0x04002FF9 RID: 12281
		private const byte DATAGRIDROWHEADER_stateMouseOverEditingRowCode = 2;

		// Token: 0x04002FFA RID: 12282
		private const byte DATAGRIDROWHEADER_stateMouseOverEditingRowFocusedCode = 3;

		// Token: 0x04002FFB RID: 12283
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCode = 4;

		// Token: 0x04002FFC RID: 12284
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowCode = 5;

		// Token: 0x04002FFD RID: 12285
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowFocusedCode = 6;

		// Token: 0x04002FFE RID: 12286
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedFocusedCode = 7;

		// Token: 0x04002FFF RID: 12287
		private const byte DATAGRIDROWHEADER_stateNormalCode = 8;

		// Token: 0x04003000 RID: 12288
		private const byte DATAGRIDROWHEADER_stateNormalCurrentRowCode = 9;

		// Token: 0x04003001 RID: 12289
		private const byte DATAGRIDROWHEADER_stateNormalEditingRowCode = 10;

		// Token: 0x04003002 RID: 12290
		private const byte DATAGRIDROWHEADER_stateNormalEditingRowFocusedCode = 11;

		// Token: 0x04003003 RID: 12291
		private const byte DATAGRIDROWHEADER_stateSelectedCode = 12;

		// Token: 0x04003004 RID: 12292
		private const byte DATAGRIDROWHEADER_stateSelectedCurrentRowCode = 13;

		// Token: 0x04003005 RID: 12293
		private const byte DATAGRIDROWHEADER_stateSelectedCurrentRowFocusedCode = 14;

		// Token: 0x04003006 RID: 12294
		private const byte DATAGRIDROWHEADER_stateSelectedFocusedCode = 15;

		// Token: 0x04003007 RID: 12295
		private const byte DATAGRIDROWHEADER_stateNullCode = 255;

		// Token: 0x04003008 RID: 12296
		private static byte[] _fallbackStateMapping = new byte[]
		{
			8,
			9,
			3,
			11,
			7,
			6,
			15,
			15,
			byte.MaxValue,
			8,
			11,
			14,
			15,
			14,
			9,
			8
		};

		// Token: 0x04003009 RID: 12297
		private static byte[] _idealStateMapping = new byte[]
		{
			8,
			8,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			12,
			15,
			4,
			7,
			10,
			11,
			2,
			3,
			9,
			9,
			1,
			1,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			13,
			14,
			5,
			6,
			10,
			11,
			2,
			3
		};

		// Token: 0x0400300A RID: 12298
		private static string[] _stateNames = new string[]
		{
			"MouseOver",
			"MouseOver_CurrentRow",
			"MouseOver_Unfocused_EditingRow",
			"MouseOver_EditingRow",
			"MouseOver_Unfocused_Selected",
			"MouseOver_Unfocused_CurrentRow_Selected",
			"MouseOver_CurrentRow_Selected",
			"MouseOver_Selected",
			"Normal",
			"Normal_CurrentRow",
			"Unfocused_EditingRow",
			"Normal_EditingRow",
			"Unfocused_Selected",
			"Unfocused_CurrentRow_Selected",
			"Normal_CurrentRow_Selected",
			"Normal_Selected"
		};

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorBrush" /> dependency property.</returns>
		// Token: 0x0400300B RID: 12299
		public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorVisibility" /> dependency property.</returns>
		// Token: 0x0400300C RID: 12300
		public static readonly DependencyProperty SeparatorVisibilityProperty = DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x0400300D RID: 12301
		private static readonly DependencyPropertyKey IsRowSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsRowSelected", typeof(bool), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceIsRowSelected)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.IsRowSelected" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DataGridRowHeader.IsRowSelected" /> dependency property.</returns>
		// Token: 0x0400300E RID: 12302
		public static readonly DependencyProperty IsRowSelectedProperty = DataGridRowHeader.IsRowSelectedPropertyKey.DependencyProperty;

		// Token: 0x0400300F RID: 12303
		private Thumb _topGripper;

		// Token: 0x04003010 RID: 12304
		private Thumb _bottomGripper;

		// Token: 0x04003011 RID: 12305
		private const string TopHeaderGripperTemplateName = "PART_TopHeaderGripper";

		// Token: 0x04003012 RID: 12306
		private const string BottomHeaderGripperTemplateName = "PART_BottomHeaderGripper";
	}
}
