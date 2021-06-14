using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MS.Internal.AppModel;
using MS.Win32;

namespace MS.Internal.Ink
{
	// Token: 0x0200068F RID: 1679
	internal static class PenCursorManager
	{
		// Token: 0x06006DCC RID: 28108 RVA: 0x001F9054 File Offset: 0x001F7254
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static Cursor GetPenCursor(DrawingAttributes drawingAttributes, bool isHollow, bool isRightToLeft, double dpiScaleX, double dpiScaleY)
		{
			Drawing drawing = PenCursorManager.CreatePenDrawing(drawingAttributes, isHollow, isRightToLeft, dpiScaleX, dpiScaleY);
			return PenCursorManager.CreateCursorFromDrawing(drawing, new Point(0.0, 0.0));
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x001F908C File Offset: 0x001F728C
		internal static Cursor GetPointEraserCursor(StylusShape stylusShape, Matrix tranform, double dpiScaleX, double dpiScaleY)
		{
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			if (stylusShape.GetType() == typeof(RectangleStylusShape))
			{
				drawingAttributes.StylusTip = StylusTip.Rectangle;
			}
			else
			{
				drawingAttributes.StylusTip = StylusTip.Ellipse;
			}
			drawingAttributes.Height = stylusShape.Height;
			drawingAttributes.Width = stylusShape.Width;
			drawingAttributes.Color = Colors.Black;
			if (!tranform.IsIdentity)
			{
				drawingAttributes.StylusTipTransform *= tranform;
			}
			if (!DoubleUtil.IsZero(stylusShape.Rotation))
			{
				Matrix identity = Matrix.Identity;
				identity.Rotate(stylusShape.Rotation);
				drawingAttributes.StylusTipTransform *= identity;
			}
			return PenCursorManager.GetPenCursor(drawingAttributes, true, false, dpiScaleX, dpiScaleY);
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x001F9140 File Offset: 0x001F7340
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static Cursor GetStrokeEraserCursor()
		{
			if (PenCursorManager.s_StrokeEraserCursor == null)
			{
				Drawing drawing = PenCursorManager.CreateStrokeEraserDrawing();
				PenCursorManager.s_StrokeEraserCursor = PenCursorManager.CreateCursorFromDrawing(drawing, new Point(5.0, 5.0));
			}
			return PenCursorManager.s_StrokeEraserCursor;
		}

		// Token: 0x06006DCF RID: 28111 RVA: 0x001F9184 File Offset: 0x001F7384
		internal static Cursor GetSelectionCursor(InkCanvasSelectionHitResult hitResult, bool isRightToLeft)
		{
			Cursor result;
			switch (hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
			case InkCanvasSelectionHitResult.BottomRight:
				if (isRightToLeft)
				{
					result = Cursors.SizeNESW;
				}
				else
				{
					result = Cursors.SizeNWSE;
				}
				break;
			case InkCanvasSelectionHitResult.Top:
			case InkCanvasSelectionHitResult.Bottom:
				result = Cursors.SizeNS;
				break;
			case InkCanvasSelectionHitResult.TopRight:
			case InkCanvasSelectionHitResult.BottomLeft:
				if (isRightToLeft)
				{
					result = Cursors.SizeNWSE;
				}
				else
				{
					result = Cursors.SizeNESW;
				}
				break;
			case InkCanvasSelectionHitResult.Right:
			case InkCanvasSelectionHitResult.Left:
				result = Cursors.SizeWE;
				break;
			case InkCanvasSelectionHitResult.Selection:
				result = Cursors.SizeAll;
				break;
			default:
				result = Cursors.Cross;
				break;
			}
			return result;
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x001F9204 File Offset: 0x001F7404
		[SecurityCritical]
		private static Cursor CreateCursorFromDrawing(Drawing drawing, Point hotspot)
		{
			Cursor arrow = Cursors.Arrow;
			Rect bounds = drawing.Bounds;
			double width = bounds.Width;
			double height = bounds.Height;
			int num = IconHelper.AlignToBytes(bounds.Width, 1);
			int num2 = IconHelper.AlignToBytes(bounds.Height, 1);
			bounds.Inflate(((double)num - width) / 2.0, ((double)num2 - height) / 2.0);
			int xHotspot = (int)Math.Round(hotspot.X - bounds.Left);
			int yHotspot = (int)Math.Round(hotspot.Y - bounds.Top);
			DrawingVisual visual = PenCursorManager.CreateCursorDrawingVisual(drawing, num, num2);
			RenderTargetBitmap rtb = PenCursorManager.RenderVisualToBitmap(visual, num, num2);
			byte[] pixels = PenCursorManager.GetPixels(rtb, num, num2);
			NativeMethods.IconHandle iconHandle = IconHelper.CreateIconCursor(pixels, num, num2, xHotspot, yHotspot, false);
			if (iconHandle.IsInvalid)
			{
				return Cursors.Arrow;
			}
			return CursorInteropHelper.CriticalCreate(iconHandle);
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x001F92F0 File Offset: 0x001F74F0
		private static DrawingVisual CreateCursorDrawingVisual(Drawing drawing, int width, int height)
		{
			DrawingBrush drawingBrush = new DrawingBrush(drawing);
			drawingBrush.Stretch = Stretch.None;
			drawingBrush.AlignmentX = AlignmentX.Center;
			drawingBrush.AlignmentY = AlignmentY.Center;
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawRectangle(drawingBrush, null, new Rect(0.0, 0.0, (double)width, (double)height));
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingVisual;
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x001F9368 File Offset: 0x001F7568
		[SecurityCritical]
		private static RenderTargetBitmap RenderVisualToBitmap(Visual visual, int width, int height)
		{
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96.0, 96.0, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(visual);
			return renderTargetBitmap;
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x001F939C File Offset: 0x001F759C
		[SecurityCritical]
		private static byte[] GetPixels(RenderTargetBitmap rtb, int width, int height)
		{
			int num = width * 4;
			FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap();
			formatConvertedBitmap.BeginInit();
			formatConvertedBitmap.Source = rtb;
			formatConvertedBitmap.DestinationFormat = PixelFormats.Bgra32;
			formatConvertedBitmap.EndInit();
			byte[] array = new byte[num * height];
			formatConvertedBitmap.CriticalCopyPixels(Int32Rect.Empty, array, num, 0);
			return array;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x001F93EC File Offset: 0x001F75EC
		private static Drawing CreatePenDrawing(DrawingAttributes drawingAttributes, bool isHollow, bool isRightToLeft, double dpiScaleX, double dpiScaleY)
		{
			Stroke stroke = new Stroke(new StylusPointCollection
			{
				new StylusPoint(0.0, 0.0)
			}, new DrawingAttributes
			{
				Color = drawingAttributes.Color,
				Width = drawingAttributes.Width,
				Height = drawingAttributes.Height,
				StylusTipTransform = drawingAttributes.StylusTipTransform,
				IsHighlighter = drawingAttributes.IsHighlighter,
				StylusTip = drawingAttributes.StylusTip
			});
			stroke.DrawingAttributes.Width = PenCursorManager.ConvertToPixel(stroke.DrawingAttributes.Width, dpiScaleX);
			stroke.DrawingAttributes.Height = PenCursorManager.ConvertToPixel(stroke.DrawingAttributes.Height, dpiScaleY);
			double num = Math.Min(SystemParameters.PrimaryScreenWidth / 2.0, SystemParameters.PrimaryScreenHeight / 2.0);
			Rect bounds = stroke.GetBounds();
			bool flag = false;
			if (DoubleUtil.LessThan(bounds.Width, 1.0))
			{
				stroke.DrawingAttributes.Width = 1.0;
				flag = true;
			}
			else if (DoubleUtil.GreaterThan(bounds.Width, num))
			{
				stroke.DrawingAttributes.Width = num;
				flag = true;
			}
			if (DoubleUtil.LessThan(bounds.Height, 1.0))
			{
				stroke.DrawingAttributes.Height = 1.0;
				flag = true;
			}
			else if (DoubleUtil.GreaterThan(bounds.Height, num))
			{
				stroke.DrawingAttributes.Height = num;
				flag = true;
			}
			if (flag)
			{
				stroke.DrawingAttributes.StylusTipTransform = Matrix.Identity;
			}
			if (isRightToLeft)
			{
				Matrix stylusTipTransform = stroke.DrawingAttributes.StylusTipTransform;
				stylusTipTransform.Scale(-1.0, 1.0);
				if (stylusTipTransform.HasInverse)
				{
					stroke.DrawingAttributes.StylusTipTransform = stylusTipTransform;
				}
			}
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingGroup.Open();
				if (isHollow)
				{
					stroke.DrawInternal(drawingContext, stroke.DrawingAttributes, isHollow);
				}
				else
				{
					stroke.Draw(drawingContext, stroke.DrawingAttributes);
				}
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingGroup;
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x001F9620 File Offset: 0x001F7820
		private static Drawing CreateStrokeEraserDrawing()
		{
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingGroup.Open();
				LinearGradientBrush linearGradientBrush = new LinearGradientBrush(Color.FromRgb(240, 242, byte.MaxValue), Color.FromRgb(180, 207, 248), 45.0);
				linearGradientBrush.Freeze();
				SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(180, 207, 248));
				solidColorBrush.Freeze();
				Pen pen = new Pen(Brushes.Gray, 0.7);
				pen.Freeze();
				PathGeometry pathGeometry = new PathGeometry();
				PathFigure pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(5.0, 5.0);
				LineSegment lineSegment = new LineSegment(new Point(16.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(5.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry.Figures.Add(pathFigure);
				pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(5.0, 5.0);
				lineSegment = new LineSegment(new Point(5.0, 10.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(5.0, 5.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry.Figures.Add(pathFigure);
				pathGeometry.Freeze();
				PathGeometry pathGeometry2 = new PathGeometry();
				pathFigure = new PathFigure();
				pathFigure.StartPoint = new Point(15.0, 15.0);
				lineSegment = new LineSegment(new Point(15.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 19.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment = new LineSegment(new Point(26.0, 15.0), true);
				lineSegment.Freeze();
				pathFigure.Segments.Add(lineSegment);
				lineSegment.Freeze();
				lineSegment = new LineSegment(new Point(15.0, 15.0), true);
				pathFigure.Segments.Add(lineSegment);
				pathFigure.IsClosed = true;
				pathFigure.Freeze();
				pathGeometry2.Figures.Add(pathFigure);
				pathGeometry2.Freeze();
				drawingContext.DrawGeometry(linearGradientBrush, pen, pathGeometry);
				drawingContext.DrawGeometry(solidColorBrush, pen, pathGeometry2);
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(5.0, 0.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(0.0, 5.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(2.0, 2.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(8.0, 2.0));
				drawingContext.DrawLine(pen, new Point(5.0, 5.0), new Point(2.0, 8.0));
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			return drawingGroup;
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x001F9B54 File Offset: 0x001F7D54
		private static double ConvertToPixel(double value, double dpiScale)
		{
			if (dpiScale != 0.0)
			{
				return value * dpiScale;
			}
			return value;
		}

		// Token: 0x04003617 RID: 13847
		private static Cursor s_StrokeEraserCursor;
	}
}
