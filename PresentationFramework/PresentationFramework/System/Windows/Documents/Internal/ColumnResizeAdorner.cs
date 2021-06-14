using System;
using System.Windows.Media;

namespace System.Windows.Documents.Internal
{
	// Token: 0x02000462 RID: 1122
	internal class ColumnResizeAdorner : Adorner
	{
		// Token: 0x060040B8 RID: 16568 RVA: 0x00127A28 File Offset: 0x00125C28
		internal ColumnResizeAdorner(UIElement scope) : base(scope)
		{
			this._pen = new Pen(new SolidColorBrush(Colors.LightSlateGray), 2.0);
			this._x = double.NaN;
			this._top = double.NaN;
			this._height = double.NaN;
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x00127A88 File Offset: 0x00125C88
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			TranslateTransform value = new TranslateTransform(this._x, this._top);
			generalTransformGroup.Children.Add(value);
			if (transform != null)
			{
				generalTransformGroup.Children.Add(transform);
			}
			return generalTransformGroup;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x00127AC9 File Offset: 0x00125CC9
		protected override void OnRender(DrawingContext drawingContext)
		{
			drawingContext.DrawLine(this._pen, new Point(0.0, 0.0), new Point(0.0, this._height));
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x00127B04 File Offset: 0x00125D04
		internal void Update(double newX)
		{
			if (this._x != newX)
			{
				this._x = newX;
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Update(base.AdornedElement);
					adornerLayer.InvalidateVisual();
				}
			}
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x00127B42 File Offset: 0x00125D42
		internal void Initialize(UIElement renderScope, double xPos, double yPos, double height)
		{
			this._adornerLayer = AdornerLayer.GetAdornerLayer(renderScope);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Add(this);
			}
			this._x = xPos;
			this._top = yPos;
			this._height = height;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00127B7A File Offset: 0x00125D7A
		internal void Uninitialize()
		{
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x04002773 RID: 10099
		private double _x;

		// Token: 0x04002774 RID: 10100
		private double _top;

		// Token: 0x04002775 RID: 10101
		private double _height;

		// Token: 0x04002776 RID: 10102
		private Pen _pen;

		// Token: 0x04002777 RID: 10103
		private AdornerLayer _adornerLayer;
	}
}
