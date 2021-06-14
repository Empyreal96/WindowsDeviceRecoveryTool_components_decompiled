using System;
using System.Windows.Data;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	/// <summary>A panel that can hold specified cells in place when the view is scrolled.</summary>
	// Token: 0x020005A3 RID: 1443
	public class SelectiveScrollingGrid : Grid
	{
		/// <summary>Gets the direction that cells can scroll for a specified object.</summary>
		/// <param name="obj">The object whose scrolling orientation is retrieved.</param>
		/// <returns>The direction that cells can scroll.</returns>
		// Token: 0x06005F78 RID: 24440 RVA: 0x001AC15B File Offset: 0x001AA35B
		public static SelectiveScrollingOrientation GetSelectiveScrollingOrientation(DependencyObject obj)
		{
			return (SelectiveScrollingOrientation)obj.GetValue(SelectiveScrollingGrid.SelectiveScrollingOrientationProperty);
		}

		/// <summary>Sets the direction that cells can scroll for a specified object.</summary>
		/// <param name="obj">The object whose scrolling orientation is set.</param>
		/// <param name="value">The scrolling orientation.</param>
		// Token: 0x06005F79 RID: 24441 RVA: 0x001AC16D File Offset: 0x001AA36D
		public static void SetSelectiveScrollingOrientation(DependencyObject obj, SelectiveScrollingOrientation value)
		{
			obj.SetValue(SelectiveScrollingGrid.SelectiveScrollingOrientationProperty, value);
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x001AC180 File Offset: 0x001AA380
		private static void OnSelectiveScrollingOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			SelectiveScrollingOrientation selectiveScrollingOrientation = (SelectiveScrollingOrientation)e.NewValue;
			ScrollViewer scrollViewer = DataGridHelper.FindVisualParent<ScrollViewer>(uielement);
			if (scrollViewer != null && uielement != null)
			{
				Transform renderTransform = uielement.RenderTransform;
				if (renderTransform != null)
				{
					BindingOperations.ClearBinding(renderTransform, TranslateTransform.XProperty);
					BindingOperations.ClearBinding(renderTransform, TranslateTransform.YProperty);
				}
				if (selectiveScrollingOrientation == SelectiveScrollingOrientation.Both)
				{
					uielement.RenderTransform = null;
					return;
				}
				TranslateTransform translateTransform = new TranslateTransform();
				if (selectiveScrollingOrientation != SelectiveScrollingOrientation.Horizontal)
				{
					Binding binding = new Binding("ContentHorizontalOffset");
					binding.Source = scrollViewer;
					BindingOperations.SetBinding(translateTransform, TranslateTransform.XProperty, binding);
				}
				if (selectiveScrollingOrientation != SelectiveScrollingOrientation.Vertical)
				{
					Binding binding2 = new Binding("ContentVerticalOffset");
					binding2.Source = scrollViewer;
					BindingOperations.SetBinding(translateTransform, TranslateTransform.YProperty, binding2);
				}
				uielement.RenderTransform = translateTransform;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.SelectiveScrollingGrid.SelectiveScrollingOrientation" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.SelectiveScrollingGrid.SelectiveScrollingOrientation" /> dependency property.</returns>
		// Token: 0x040030A6 RID: 12454
		public static readonly DependencyProperty SelectiveScrollingOrientationProperty = DependencyProperty.RegisterAttached("SelectiveScrollingOrientation", typeof(SelectiveScrollingOrientation), typeof(SelectiveScrollingGrid), new FrameworkPropertyMetadata(SelectiveScrollingOrientation.Both, new PropertyChangedCallback(SelectiveScrollingGrid.OnSelectiveScrollingOrientationChanged)));
	}
}
