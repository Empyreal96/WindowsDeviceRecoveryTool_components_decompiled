using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x02000759 RID: 1881
	internal class InkCanvasSelectionAdorner : Adorner
	{
		// Token: 0x060077BC RID: 30652 RVA: 0x00222B2C File Offset: 0x00220D2C
		internal InkCanvasSelectionAdorner(UIElement adornedElement) : base(adornedElement)
		{
			this._adornerBorderPen = new Pen(Brushes.Black, 1.0);
			DoubleCollection doubleCollection = new DoubleCollection();
			doubleCollection.Add(4.5);
			doubleCollection.Add(4.5);
			this._adornerBorderPen.DashStyle = new DashStyle(doubleCollection, 2.25);
			this._adornerBorderPen.DashCap = PenLineCap.Flat;
			this._adornerBorderPen.Freeze();
			this._adornerPenBrush = new Pen(new SolidColorBrush(Color.FromRgb(132, 146, 222)), 1.0);
			this._adornerPenBrush.Freeze();
			this._adornerFillBrush = new LinearGradientBrush(Color.FromRgb(240, 242, byte.MaxValue), Color.FromRgb(180, 207, 248), 45.0);
			this._adornerFillBrush.Freeze();
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingGroup.Open();
				drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0, 1.0, 1.0));
				drawingContext.DrawLine(new Pen(Brushes.Black, 0.16)
				{
					StartLineCap = PenLineCap.Square,
					EndLineCap = PenLineCap.Square
				}, new Point(1.0, 0.0), new Point(0.0, 1.0));
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			drawingGroup.Freeze();
			DrawingBrush drawingBrush = new DrawingBrush(drawingGroup);
			drawingBrush.TileMode = TileMode.Tile;
			drawingBrush.Viewport = new Rect(0.0, 0.0, 6.0, 6.0);
			drawingBrush.ViewportUnits = BrushMappingMode.Absolute;
			drawingBrush.Freeze();
			this._hatchPen = new Pen(drawingBrush, 6.0);
			this._hatchPen.Freeze();
			this._elementsBounds = new List<Rect>();
			this._strokesBounds = Rect.Empty;
		}

		// Token: 0x060077BD RID: 30653 RVA: 0x00222D70 File Offset: 0x00220F70
		internal InkCanvasSelectionHitResult SelectionHandleHitTest(Point point)
		{
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.None;
			Rect wireFrameRect = this.GetWireFrameRect();
			if (!wireFrameRect.IsEmpty)
			{
				for (InkCanvasSelectionHitResult inkCanvasSelectionHitResult2 = InkCanvasSelectionHitResult.TopLeft; inkCanvasSelectionHitResult2 <= InkCanvasSelectionHitResult.Left; inkCanvasSelectionHitResult2++)
				{
					Rect rect;
					Rect rect2;
					this.GetHandleRect(inkCanvasSelectionHitResult2, wireFrameRect, out rect, out rect2);
					if (rect2.Contains(point))
					{
						inkCanvasSelectionHitResult = inkCanvasSelectionHitResult2;
						break;
					}
				}
				if (inkCanvasSelectionHitResult == InkCanvasSelectionHitResult.None && Rect.Inflate(wireFrameRect, 4.0, 4.0).Contains(point))
				{
					inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.Selection;
				}
			}
			return inkCanvasSelectionHitResult;
		}

		// Token: 0x060077BE RID: 30654 RVA: 0x00222DE0 File Offset: 0x00220FE0
		internal void UpdateSelectionWireFrame(Rect strokesBounds, List<Rect> hatchBounds)
		{
			bool flag = false;
			bool flag2 = false;
			if (this._strokesBounds != strokesBounds)
			{
				this._strokesBounds = strokesBounds;
				flag = true;
			}
			int count = hatchBounds.Count;
			if (count != this._elementsBounds.Count)
			{
				flag2 = true;
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					if (this._elementsBounds[i] != hatchBounds[i])
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag || flag2)
			{
				if (flag2)
				{
					this._elementsBounds = hatchBounds;
				}
				base.InvalidateVisual();
			}
		}

		// Token: 0x060077BF RID: 30655 RVA: 0x00222E60 File Offset: 0x00221060
		protected override void OnRender(DrawingContext drawingContext)
		{
			this.DrawBackgound(drawingContext);
			Rect wireFrameRect = this.GetWireFrameRect();
			if (!wireFrameRect.IsEmpty)
			{
				drawingContext.DrawRectangle(null, this._adornerBorderPen, wireFrameRect);
				this.DrawHandles(drawingContext, wireFrameRect);
			}
		}

		// Token: 0x060077C0 RID: 30656 RVA: 0x00222E9C File Offset: 0x0022109C
		private void DrawHandles(DrawingContext drawingContext, Rect rectWireFrame)
		{
			for (InkCanvasSelectionHitResult inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.TopLeft; inkCanvasSelectionHitResult <= InkCanvasSelectionHitResult.Left; inkCanvasSelectionHitResult++)
			{
				Rect rectangle;
				Rect rect;
				this.GetHandleRect(inkCanvasSelectionHitResult, rectWireFrame, out rectangle, out rect);
				drawingContext.DrawRectangle(this._adornerFillBrush, this._adornerPenBrush, rectangle);
			}
		}

		// Token: 0x060077C1 RID: 30657 RVA: 0x00222ED4 File Offset: 0x002210D4
		private void DrawBackgound(DrawingContext drawingContext)
		{
			PathGeometry pathGeometry = null;
			int count = this._elementsBounds.Count;
			Geometry geometry;
			if (count != 0)
			{
				for (int i = 0; i < count; i++)
				{
					Rect rect = this._elementsBounds[i];
					if (!rect.IsEmpty)
					{
						rect.Inflate(3.0, 3.0);
						if (pathGeometry == null)
						{
							PathFigure pathFigure = new PathFigure();
							pathFigure.StartPoint = new Point(rect.Left, rect.Top);
							PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
							PathSegment pathSegment = new LineSegment(new Point(rect.Right, rect.Top), true);
							pathSegment.Freeze();
							pathSegmentCollection.Add(pathSegment);
							pathSegment = new LineSegment(new Point(rect.Right, rect.Bottom), true);
							pathSegment.Freeze();
							pathSegmentCollection.Add(pathSegment);
							pathSegment = new LineSegment(new Point(rect.Left, rect.Bottom), true);
							pathSegment.Freeze();
							pathSegmentCollection.Add(pathSegment);
							pathSegment = new LineSegment(new Point(rect.Left, rect.Top), true);
							pathSegment.Freeze();
							pathSegmentCollection.Add(pathSegment);
							pathSegmentCollection.Freeze();
							pathFigure.Segments = pathSegmentCollection;
							pathFigure.IsClosed = true;
							pathFigure.Freeze();
							pathGeometry = new PathGeometry();
							pathGeometry.Figures.Add(pathFigure);
						}
						else
						{
							geometry = new RectangleGeometry(rect);
							geometry.Freeze();
							pathGeometry = Geometry.Combine(pathGeometry, geometry, GeometryCombineMode.Union, null);
						}
					}
				}
			}
			GeometryGroup geometryGroup = new GeometryGroup();
			GeometryCollection geometryCollection = new GeometryCollection();
			geometry = new RectangleGeometry(new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			geometry.Freeze();
			geometryCollection.Add(geometry);
			Geometry geometry2 = null;
			if (pathGeometry != null)
			{
				pathGeometry.Freeze();
				geometry2 = pathGeometry.GetOutlinedPathGeometry();
				geometry2.Freeze();
				if (count == 1 && ((InkCanvasInnerCanvas)base.AdornedElement).InkCanvas.GetSelectedStrokes().Count == 0)
				{
					geometryCollection.Add(geometry2);
				}
			}
			geometryCollection.Freeze();
			geometryGroup.Children = geometryCollection;
			geometryGroup.Freeze();
			drawingContext.DrawGeometry(Brushes.Transparent, null, geometryGroup);
			if (geometry2 != null)
			{
				drawingContext.DrawGeometry(null, this._hatchPen, geometry2);
			}
		}

		// Token: 0x060077C2 RID: 30658 RVA: 0x00223138 File Offset: 0x00221338
		private void GetHandleRect(InkCanvasSelectionHitResult hitResult, Rect rectWireFrame, out Rect visibleRect, out Rect toleranceRect)
		{
			Point point = default(Point);
			double num = 0.0;
			double num2 = 3.0;
			switch (hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
				num = 8.0;
				point = new Point(rectWireFrame.Left, rectWireFrame.Top);
				break;
			case InkCanvasSelectionHitResult.Top:
				num = 6.0;
				point = new Point(rectWireFrame.Left + rectWireFrame.Width / 2.0, rectWireFrame.Top);
				num2 = 5.0;
				break;
			case InkCanvasSelectionHitResult.TopRight:
				num = 8.0;
				point = new Point(rectWireFrame.Right, rectWireFrame.Top);
				break;
			case InkCanvasSelectionHitResult.Right:
				num = 6.0;
				point = new Point(rectWireFrame.Right, rectWireFrame.Top + rectWireFrame.Height / 2.0);
				num2 = 5.0;
				break;
			case InkCanvasSelectionHitResult.BottomRight:
				num = 8.0;
				point = new Point(rectWireFrame.Right, rectWireFrame.Bottom);
				break;
			case InkCanvasSelectionHitResult.Bottom:
				num = 6.0;
				point = new Point(rectWireFrame.Left + rectWireFrame.Width / 2.0, rectWireFrame.Bottom);
				num2 = 5.0;
				break;
			case InkCanvasSelectionHitResult.BottomLeft:
				num = 8.0;
				point = new Point(rectWireFrame.Left, rectWireFrame.Bottom);
				break;
			case InkCanvasSelectionHitResult.Left:
				num = 6.0;
				point = new Point(rectWireFrame.Left, rectWireFrame.Top + rectWireFrame.Height / 2.0);
				num2 = 5.0;
				break;
			}
			visibleRect = new Rect(point.X - num / 2.0, point.Y - num / 2.0, num, num);
			toleranceRect = visibleRect;
			toleranceRect.Inflate(num2, num2);
		}

		// Token: 0x060077C3 RID: 30659 RVA: 0x0022335C File Offset: 0x0022155C
		private Rect GetWireFrameRect()
		{
			Rect result = Rect.Empty;
			Rect selectionBounds = ((InkCanvasInnerCanvas)base.AdornedElement).InkCanvas.GetSelectionBounds();
			if (!selectionBounds.IsEmpty)
			{
				result = Rect.Inflate(selectionBounds, 8.0, 8.0);
			}
			return result;
		}

		// Token: 0x040038D4 RID: 14548
		private Pen _adornerBorderPen;

		// Token: 0x040038D5 RID: 14549
		private Pen _adornerPenBrush;

		// Token: 0x040038D6 RID: 14550
		private Brush _adornerFillBrush;

		// Token: 0x040038D7 RID: 14551
		private Pen _hatchPen;

		// Token: 0x040038D8 RID: 14552
		private Rect _strokesBounds;

		// Token: 0x040038D9 RID: 14553
		private List<Rect> _elementsBounds;

		// Token: 0x040038DA RID: 14554
		private const double BorderMargin = 8.0;

		// Token: 0x040038DB RID: 14555
		private const double HatchBorderMargin = 6.0;

		// Token: 0x040038DC RID: 14556
		private const int CornerResizeHandleSize = 8;

		// Token: 0x040038DD RID: 14557
		private const int MiddleResizeHandleSize = 6;

		// Token: 0x040038DE RID: 14558
		private const double ResizeHandleTolerance = 3.0;

		// Token: 0x040038DF RID: 14559
		private const double LineThickness = 0.16;
	}
}
