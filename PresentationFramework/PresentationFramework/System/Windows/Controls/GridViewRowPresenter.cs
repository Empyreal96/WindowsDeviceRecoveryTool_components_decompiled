using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents an object that specifies the layout of a row of data.</summary>
	// Token: 0x020004DD RID: 1245
	public class GridViewRowPresenter : GridViewRowPresenterBase
	{
		/// <summary>Returns a string representation of the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />.</summary>
		/// <returns>A string that shows the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />.</returns>
		// Token: 0x06004DA5 RID: 19877 RVA: 0x0015E028 File Offset: 0x0015C228
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewRowPresenter", new object[]
			{
				base.GetType(),
				(this.Content != null) ? this.Content.ToString() : string.Empty,
				(base.Columns != null) ? base.Columns.Count : 0
			});
		}

		/// <summary>Gets or sets the data content to display in a row. </summary>
		/// <returns>The object that represents the content of a row.</returns>
		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06004DA6 RID: 19878 RVA: 0x0015E089 File Offset: 0x0015C289
		// (set) Token: 0x06004DA7 RID: 19879 RVA: 0x0015E096 File Offset: 0x0015C296
		public object Content
		{
			get
			{
				return base.GetValue(GridViewRowPresenter.ContentProperty);
			}
			set
			{
				base.SetValue(GridViewRowPresenter.ContentProperty, value);
			}
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0015E0A4 File Offset: 0x0015C2A4
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewRowPresenter gridViewRowPresenter = (GridViewRowPresenter)d;
			Type type = (e.OldValue != null) ? e.OldValue.GetType() : null;
			Type right = (e.NewValue != null) ? e.NewValue.GetType() : null;
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				gridViewRowPresenter._oldContentType = type;
				right = type;
			}
			else if (e.OldValue == BindingExpressionBase.DisconnectedItem)
			{
				type = gridViewRowPresenter._oldContentType;
			}
			if (type != right)
			{
				gridViewRowPresenter.NeedUpdateVisualTree = true;
				return;
			}
			gridViewRowPresenter.UpdateCells();
		}

		/// <summary>Determines the area that is required to display the row. </summary>
		/// <param name="constraint">The maximum area to use to display the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />. </param>
		/// <returns>The actual <see cref="T:System.Windows.Size" /> of the area that displays the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />.</returns>
		// Token: 0x06004DA9 RID: 19881 RVA: 0x0015E130 File Offset: 0x0015C330
		protected override Size MeasureOverride(Size constraint)
		{
			GridViewColumnCollection columns = base.Columns;
			if (columns == null)
			{
				return default(Size);
			}
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double height = constraint.Height;
			bool flag = false;
			foreach (GridViewColumn gridViewColumn in columns)
			{
				UIElement uielement = internalChildren[gridViewColumn.ActualIndex];
				if (uielement != null)
				{
					double num3 = Math.Max(0.0, constraint.Width - num2);
					if (gridViewColumn.State == ColumnMeasureState.Init || gridViewColumn.State == ColumnMeasureState.Headered)
					{
						if (!flag)
						{
							base.EnsureDesiredWidthList();
							base.LayoutUpdated += this.OnLayoutUpdated;
							flag = true;
						}
						uielement.Measure(new Size(num3, height));
						if (this.IsOnCurrentPage)
						{
							gridViewColumn.EnsureWidth(uielement.DesiredSize.Width);
						}
						base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.DesiredWidth;
						num2 += gridViewColumn.DesiredWidth;
					}
					else if (gridViewColumn.State == ColumnMeasureState.Data)
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
			this._isOnCurrentPageValid = false;
			num2 += 2.0;
			return new Size(num2, num);
		}

		/// <summary>Positions the content of a row according to the size of the corresponding <see cref="T:System.Windows.Controls.GridViewColumn" /> objects.</summary>
		/// <param name="arrangeSize">The area to use to display the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />.</param>
		/// <returns>The actual <see cref="T:System.Windows.Size" /> that is used to display the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" />.</returns>
		// Token: 0x06004DAA RID: 19882 RVA: 0x0015E30C File Offset: 0x0015C50C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			GridViewColumnCollection columns = base.Columns;
			if (columns == null)
			{
				return arrangeSize;
			}
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = arrangeSize.Width;
			foreach (GridViewColumn gridViewColumn in columns)
			{
				UIElement uielement = internalChildren[gridViewColumn.ActualIndex];
				if (uielement != null)
				{
					double num3 = Math.Min(num2, (gridViewColumn.State == ColumnMeasureState.SpecificWidth) ? gridViewColumn.Width : gridViewColumn.DesiredWidth);
					uielement.Arrange(new Rect(num, 0.0, num3, arrangeSize.Height));
					num2 -= num3;
					num += num3;
				}
			}
			return arrangeSize;
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0015E3D8 File Offset: 0x0015C5D8
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.NeedUpdateVisualTree)
			{
				base.InternalChildren.Clear();
				GridViewColumnCollection columns = base.Columns;
				if (columns != null)
				{
					foreach (GridViewColumn column in columns.ColumnCollection)
					{
						base.InternalChildren.AddInternal(this.CreateCell(column));
					}
				}
				base.NeedUpdateVisualTree = false;
			}
			this._viewPortValid = false;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0015E468 File Offset: 0x0015C668
		internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
		{
			if ("ActualWidth".Equals(propertyName))
			{
				return;
			}
			int actualIndex;
			if ((actualIndex = column.ActualIndex) >= 0 && actualIndex < base.InternalChildren.Count)
			{
				if (GridViewColumn.WidthProperty.Name.Equals(propertyName))
				{
					base.InvalidateMeasure();
					return;
				}
				if ("DisplayMemberBinding".Equals(propertyName))
				{
					FrameworkElement frameworkElement = base.InternalChildren[actualIndex] as FrameworkElement;
					if (frameworkElement != null)
					{
						BindingBase displayMemberBinding = column.DisplayMemberBinding;
						if (displayMemberBinding != null && frameworkElement is TextBlock)
						{
							frameworkElement.SetBinding(TextBlock.TextProperty, displayMemberBinding);
							return;
						}
						this.RenewCell(actualIndex, column);
						return;
					}
				}
				else
				{
					ContentPresenter contentPresenter = base.InternalChildren[actualIndex] as ContentPresenter;
					if (contentPresenter != null)
					{
						if (GridViewColumn.CellTemplateProperty.Name.Equals(propertyName))
						{
							DataTemplate cellTemplate;
							if ((cellTemplate = column.CellTemplate) == null)
							{
								contentPresenter.ClearValue(ContentControl.ContentTemplateProperty);
								return;
							}
							contentPresenter.ContentTemplate = cellTemplate;
							return;
						}
						else if (GridViewColumn.CellTemplateSelectorProperty.Name.Equals(propertyName))
						{
							DataTemplateSelector cellTemplateSelector;
							if ((cellTemplateSelector = column.CellTemplateSelector) == null)
							{
								contentPresenter.ClearValue(ContentControl.ContentTemplateSelectorProperty);
								return;
							}
							contentPresenter.ContentTemplateSelector = cellTemplateSelector;
						}
					}
				}
			}
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x0015E584 File Offset: 0x0015C784
		internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			base.OnColumnCollectionChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Move)
			{
				base.InvalidateArrange();
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				base.InternalChildren.AddInternal(this.CreateCell((GridViewColumn)e.NewItems[0]));
				break;
			case NotifyCollectionChangedAction.Remove:
				base.InternalChildren.RemoveAt(e.ActualIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				base.InternalChildren.RemoveAt(e.ActualIndex);
				base.InternalChildren.AddInternal(this.CreateCell((GridViewColumn)e.NewItems[0]));
				break;
			case NotifyCollectionChangedAction.Reset:
				base.InternalChildren.Clear();
				break;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x0015E64C File Offset: 0x0015C84C
		internal List<UIElement> ActualCells
		{
			get
			{
				List<UIElement> list = new List<UIElement>();
				GridViewColumnCollection columns = base.Columns;
				if (columns != null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					List<int> indexList = columns.IndexList;
					if (internalChildren.Count == columns.Count)
					{
						int i = 0;
						int count = columns.Count;
						while (i < count)
						{
							UIElement uielement = internalChildren[indexList[i]];
							if (uielement != null)
							{
								list.Add(uielement);
							}
							i++;
						}
					}
				}
				return list;
			}
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x0015E6BC File Offset: 0x0015C8BC
		private void FindViewPort()
		{
			this._viewItem = (base.TemplatedParent as FrameworkElement);
			if (this._viewItem != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this._viewItem);
				if (itemsControl != null)
				{
					ScrollViewer scrollHost = itemsControl.ScrollHost;
					if (scrollHost != null && itemsControl.ItemsHost is VirtualizingPanel && scrollHost.CanContentScroll)
					{
						this._viewPort = (scrollHost.GetTemplateChild("PART_ScrollContentPresenter") as FrameworkElement);
						if (this._viewPort == null)
						{
							this._viewPort = scrollHost;
						}
					}
				}
			}
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x0015E738 File Offset: 0x0015C938
		private bool CheckVisibleOnCurrentPage()
		{
			if (!this._viewPortValid)
			{
				this.FindViewPort();
			}
			bool result = true;
			if (this._viewItem != null && this._viewPort != null)
			{
				Rect container = new Rect(default(Point), this._viewPort.RenderSize);
				Rect rect = new Rect(default(Point), this._viewItem.RenderSize);
				rect = this._viewItem.TransformToAncestor(this._viewPort).TransformBounds(rect);
				result = this.CheckContains(container, rect);
			}
			return result;
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x0015E7C0 File Offset: 0x0015C9C0
		private bool CheckContains(Rect container, Rect element)
		{
			return (this.CheckIsPointBetween(container, element.Top) && this.CheckIsPointBetween(container, element.Bottom)) || this.CheckIsPointBetween(element, container.Top + 2.0) || this.CheckIsPointBetween(element, container.Bottom - 2.0);
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0015E821 File Offset: 0x0015CA21
		private bool CheckIsPointBetween(Rect rect, double pointY)
		{
			return DoubleUtil.LessThanOrClose(rect.Top, pointY) && DoubleUtil.LessThanOrClose(pointY, rect.Bottom);
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0015E844 File Offset: 0x0015CA44
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
						gridViewColumn.State = ColumnMeasureState.Data;
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

		// Token: 0x06004DB4 RID: 19892 RVA: 0x0015E918 File Offset: 0x0015CB18
		private FrameworkElement CreateCell(GridViewColumn column)
		{
			BindingBase displayMemberBinding;
			FrameworkElement frameworkElement;
			if ((displayMemberBinding = column.DisplayMemberBinding) != null)
			{
				frameworkElement = new TextBlock();
				frameworkElement.DataContext = this.Content;
				frameworkElement.SetBinding(TextBlock.TextProperty, displayMemberBinding);
			}
			else
			{
				ContentPresenter contentPresenter = new ContentPresenter();
				contentPresenter.Content = this.Content;
				DataTemplate cellTemplate;
				if ((cellTemplate = column.CellTemplate) != null)
				{
					contentPresenter.ContentTemplate = cellTemplate;
				}
				DataTemplateSelector cellTemplateSelector;
				if ((cellTemplateSelector = column.CellTemplateSelector) != null)
				{
					contentPresenter.ContentTemplateSelector = cellTemplateSelector;
				}
				frameworkElement = contentPresenter;
			}
			ContentControl contentControl;
			if ((contentControl = (base.TemplatedParent as ContentControl)) != null)
			{
				frameworkElement.VerticalAlignment = contentControl.VerticalContentAlignment;
				frameworkElement.HorizontalAlignment = contentControl.HorizontalContentAlignment;
			}
			frameworkElement.Margin = GridViewRowPresenter._defalutCellMargin;
			return frameworkElement;
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x0015E9BD File Offset: 0x0015CBBD
		private void RenewCell(int index, GridViewColumn column)
		{
			base.InternalChildren.RemoveAt(index);
			base.InternalChildren.Insert(index, this.CreateCell(column));
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0015E9E0 File Offset: 0x0015CBE0
		private void UpdateCells()
		{
			UIElementCollection internalChildren = base.InternalChildren;
			ContentControl contentControl = base.TemplatedParent as ContentControl;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				FrameworkElement frameworkElement = (FrameworkElement)internalChildren[i];
				ContentPresenter contentPresenter;
				if ((contentPresenter = (frameworkElement as ContentPresenter)) != null)
				{
					contentPresenter.Content = this.Content;
				}
				else
				{
					frameworkElement.DataContext = this.Content;
				}
				if (contentControl != null)
				{
					frameworkElement.VerticalAlignment = contentControl.VerticalContentAlignment;
					frameworkElement.HorizontalAlignment = contentControl.HorizontalContentAlignment;
				}
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x0015EA62 File Offset: 0x0015CC62
		private bool IsOnCurrentPage
		{
			get
			{
				if (!this._isOnCurrentPageValid)
				{
					this._isOnCurrentPage = (base.IsVisible && this.CheckVisibleOnCurrentPage());
					this._isOnCurrentPageValid = true;
				}
				return this._isOnCurrentPage;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridViewRowPresenter.Content" /> dependency property. </summary>
		// Token: 0x04002B85 RID: 11141
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(GridViewRowPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(GridViewRowPresenter.OnContentChanged)));

		// Token: 0x04002B86 RID: 11142
		private FrameworkElement _viewPort;

		// Token: 0x04002B87 RID: 11143
		private FrameworkElement _viewItem;

		// Token: 0x04002B88 RID: 11144
		private Type _oldContentType;

		// Token: 0x04002B89 RID: 11145
		private bool _viewPortValid;

		// Token: 0x04002B8A RID: 11146
		private bool _isOnCurrentPage;

		// Token: 0x04002B8B RID: 11147
		private bool _isOnCurrentPageValid;

		// Token: 0x04002B8C RID: 11148
		private static readonly Thickness _defalutCellMargin = new Thickness(6.0, 0.0, 6.0, 0.0);
	}
}
