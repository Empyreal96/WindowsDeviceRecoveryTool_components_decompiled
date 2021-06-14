using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used within the template of a <see cref="T:System.Windows.Controls.DataGrid" /> to specify the location in the control's visual tree where the row details are to be added.</summary>
	// Token: 0x0200057F RID: 1407
	public class DataGridDetailsPresenter : ContentPresenter
	{
		// Token: 0x06005D20 RID: 23840 RVA: 0x001A36AC File Offset: 0x001A18AC
		static DataGridDetailsPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(typeof(DataGridDetailsPresenter)));
			ContentPresenter.ContentTemplateProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridDetailsPresenter.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridDetailsPresenter.OnCoerceContentTemplate)));
			ContentPresenter.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridDetailsPresenter.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridDetailsPresenter.OnCoerceContentTemplateSelector)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
			EventManager.RegisterClassHandler(typeof(DataGridDetailsPresenter), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DataGridDetailsPresenter.OnAnyMouseLeftButtonDownThunk), true);
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Automation.Peers.DataGridDetailsPresenterAutomationPeer" /> for this presenter.</summary>
		/// <returns>A new automation peer for this presenter.</returns>
		// Token: 0x06005D22 RID: 23842 RVA: 0x001A3786 File Offset: 0x001A1986
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridDetailsPresenterAutomationPeer(this);
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x001A3790 File Offset: 0x001A1990
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridDetailsPresenter dataGridDetailsPresenter = d as DataGridDetailsPresenter;
			DataGridRow dataGridRowOwner = dataGridDetailsPresenter.DataGridRowOwner;
			DataGrid grandParentObject = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridDetailsPresenter, baseValue, ContentPresenter.ContentTemplateProperty, dataGridRowOwner, DataGridRow.DetailsTemplateProperty, grandParentObject, DataGrid.RowDetailsTemplateProperty);
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x001A37D0 File Offset: 0x001A19D0
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridDetailsPresenter dataGridDetailsPresenter = d as DataGridDetailsPresenter;
			DataGridRow dataGridRowOwner = dataGridDetailsPresenter.DataGridRowOwner;
			DataGrid grandParentObject = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridDetailsPresenter, baseValue, ContentPresenter.ContentTemplateSelectorProperty, dataGridRowOwner, DataGridRow.DetailsTemplateSelectorProperty, grandParentObject, DataGrid.RowDetailsTemplateSelectorProperty);
		}

		/// <summary>Invoked when the parent of this element in the visual tree is changed. Overrides <see cref="M:System.Windows.UIElement.OnVisualParentChanged(System.Windows.DependencyObject)" />.</summary>
		/// <param name="oldParent">The old parent element. May be <see langword="null" /> to indicate that the element did not have a visual parent previously.</param>
		// Token: 0x06005D25 RID: 23845 RVA: 0x001A3810 File Offset: 0x001A1A10
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner != null)
			{
				dataGridRowOwner.DetailsPresenter = this;
				this.SyncProperties();
			}
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x001A383B File Offset: 0x001A1A3B
		private static void OnAnyMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGridDetailsPresenter)sender).OnAnyMouseLeftButtonDown(e);
		}

		// Token: 0x06005D27 RID: 23847 RVA: 0x001A384C File Offset: 0x001A1A4C
		private void OnAnyMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return;
			}
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			DataGrid dataGrid = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			if (dataGrid != null && dataGridRowOwner != null)
			{
				if (dataGrid.CurrentCell.Item != dataGridRowOwner.Item)
				{
					dataGrid.ScrollIntoView(dataGridRowOwner.Item, dataGrid.ColumnFromDisplayIndex(0));
				}
				dataGrid.HandleSelectionForRowHeaderAndDetailsInput(dataGridRowOwner, Mouse.Captured == null);
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06005D28 RID: 23848 RVA: 0x001A38CC File Offset: 0x001A1ACC
		internal FrameworkElement DetailsElement
		{
			get
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(this);
				if (childrenCount > 0)
				{
					return VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
				}
				return null;
			}
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x001A38F4 File Offset: 0x001A1AF4
		internal void SyncProperties()
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			base.Content = ((dataGridRowOwner != null) ? dataGridRowOwner.Item : null);
			DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateSelectorProperty);
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x001A3930 File Offset: 0x001A1B30
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridDetailsPresenter)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x001A3940 File Offset: 0x001A1B40
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property == DataGrid.RowDetailsTemplateProperty || e.Property == DataGridRow.DetailsTemplateProperty || e.Property == ContentPresenter.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGrid.RowDetailsTemplateSelectorProperty || e.Property == DataGridRow.DetailsTemplateSelectorProperty || e.Property == ContentPresenter.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateSelectorProperty);
			}
		}

		/// <summary>Determines the size of the <see cref="T:System.Windows.Controls.ContentPresenter" /> object based on the sizing properties, margin, and requested size of the child content.</summary>
		/// <param name="availableSize">The available size that this element can give to child elements.</param>
		/// <returns>The size that is required to arrange child content.</returns>
		// Token: 0x06005D2C RID: 23852 RVA: 0x001A39B8 File Offset: 0x001A1BB8
		protected override Size MeasureOverride(Size availableSize)
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return base.MeasureOverride(availableSize);
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return base.MeasureOverride(availableSize);
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Size result = base.MeasureOverride(DataGridHelper.SubtractFromSize(availableSize, horizontalGridLineThickness, true));
				result.Height += horizontalGridLineThickness;
				return result;
			}
			return base.MeasureOverride(availableSize);
		}

		/// <summary>Positions the single child element and determines the content of a <see cref="T:System.Windows.Controls.ContentPresenter" /> object.</summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The actual size needed by the element.</returns>
		// Token: 0x06005D2D RID: 23853 RVA: 0x001A3A28 File Offset: 0x001A1C28
		protected override Size ArrangeOverride(Size finalSize)
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return base.ArrangeOverride(finalSize);
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return base.ArrangeOverride(finalSize);
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Size result = base.ArrangeOverride(DataGridHelper.SubtractFromSize(finalSize, horizontalGridLineThickness, true));
				result.Height += horizontalGridLineThickness;
				return result;
			}
			return base.ArrangeOverride(finalSize);
		}

		/// <summary>Called by the layout system to draw a horizontal line below the row details section if horizontal grid lines are visible.</summary>
		/// <param name="drawingContext">The drawing instructions for the row details. This context is provided to the layout system.</param>
		// Token: 0x06005D2E RID: 23854 RVA: 0x001A3A98 File Offset: 0x001A1C98
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return;
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle);
			}
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06005D2F RID: 23855 RVA: 0x001A3B1C File Offset: 0x001A1D1C
		private DataGrid DataGridOwner
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				if (dataGridRowOwner != null)
				{
					return dataGridRowOwner.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06005D30 RID: 23856 RVA: 0x001A13CB File Offset: 0x0019F5CB
		internal DataGridRow DataGridRowOwner
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}
	}
}
