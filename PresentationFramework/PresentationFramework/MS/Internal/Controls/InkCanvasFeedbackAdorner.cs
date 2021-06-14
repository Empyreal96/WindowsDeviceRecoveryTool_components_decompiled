using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x02000757 RID: 1879
	internal class InkCanvasFeedbackAdorner : Adorner
	{
		// Token: 0x060077A9 RID: 30633 RVA: 0x002224EB File Offset: 0x002206EB
		private InkCanvasFeedbackAdorner() : base(null)
		{
		}

		// Token: 0x060077AA RID: 30634 RVA: 0x0022251C File Offset: 0x0022071C
		internal InkCanvasFeedbackAdorner(InkCanvas inkCanvas) : base((inkCanvas != null) ? inkCanvas.InnerCanvas : null)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._adornerBorderPen = new Pen(Brushes.Black, 1.0);
			DoubleCollection doubleCollection = new DoubleCollection();
			doubleCollection.Add(4.5);
			doubleCollection.Add(4.5);
			this._adornerBorderPen.DashStyle = new DashStyle(doubleCollection, 2.25);
			this._adornerBorderPen.DashCap = PenLineCap.Flat;
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x002225DC File Offset: 0x002207DC
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			base.VerifyAccess();
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(transform);
			if (!DoubleUtil.AreClose(this._offsetX, 0.0) || !DoubleUtil.AreClose(this._offsetY, 0.0))
			{
				generalTransformGroup.Children.Add(new TranslateTransform(this._offsetX, this._offsetY));
			}
			return generalTransformGroup;
		}

		// Token: 0x060077AC RID: 30636 RVA: 0x00222658 File Offset: 0x00220858
		private void OnBoundsUpdated(Rect rect)
		{
			base.VerifyAccess();
			if (rect != this._previousRect)
			{
				bool flag = false;
				Size size;
				double num2;
				double num3;
				if (!rect.IsEmpty)
				{
					double num = 12.0;
					Rect rect2 = Rect.Inflate(rect, num, num);
					size = new Size(rect2.Width, rect2.Height);
					num2 = rect2.Left;
					num3 = rect2.Top;
				}
				else
				{
					size = new Size(0.0, 0.0);
					num2 = 0.0;
					num3 = 0.0;
				}
				if (this._frameSize != size)
				{
					this._frameSize = size;
					flag = true;
				}
				if (!DoubleUtil.AreClose(this._offsetX, num2) || !DoubleUtil.AreClose(this._offsetY, num3))
				{
					this._offsetX = num2;
					this._offsetY = num3;
					flag = true;
				}
				if (flag)
				{
					base.InvalidateMeasure();
					base.InvalidateVisual();
					UIElement uielement = (UIElement)VisualTreeHelper.GetParent(this);
					if (uielement != null)
					{
						((UIElement)VisualTreeHelper.GetParent(this)).InvalidateArrange();
					}
				}
				this._previousRect = rect;
			}
		}

		// Token: 0x060077AD RID: 30637 RVA: 0x0022276C File Offset: 0x0022096C
		protected override Size MeasureOverride(Size constraint)
		{
			base.VerifyAccess();
			return this._frameSize;
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x0022277C File Offset: 0x0022097C
		protected override void OnRender(DrawingContext drawingContext)
		{
			drawingContext.DrawRectangle(null, this._adornerBorderPen, new Rect(4.0, 4.0, this._frameSize.Width - 8.0, this._frameSize.Height - 8.0));
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x002227D7 File Offset: 0x002209D7
		internal void UpdateBounds(Rect rect)
		{
			this.OnBoundsUpdated(rect);
		}

		// Token: 0x040038CB RID: 14539
		private InkCanvas _inkCanvas;

		// Token: 0x040038CC RID: 14540
		private Size _frameSize = new Size(0.0, 0.0);

		// Token: 0x040038CD RID: 14541
		private Rect _previousRect = Rect.Empty;

		// Token: 0x040038CE RID: 14542
		private double _offsetX;

		// Token: 0x040038CF RID: 14543
		private double _offsetY;

		// Token: 0x040038D0 RID: 14544
		private Pen _adornerBorderPen;

		// Token: 0x040038D1 RID: 14545
		private const int CornerResizeHandleSize = 8;

		// Token: 0x040038D2 RID: 14546
		private const double BorderMargin = 8.0;
	}
}
