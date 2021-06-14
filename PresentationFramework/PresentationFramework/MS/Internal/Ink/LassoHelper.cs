using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x0200068D RID: 1677
	internal class LassoHelper
	{
		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x06006DB0 RID: 28080 RVA: 0x001F8194 File Offset: 0x001F6394
		public Visual Visual
		{
			get
			{
				this.EnsureVisual();
				return this._containerVisual;
			}
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x001F81A4 File Offset: 0x001F63A4
		public Point[] AddPoints(List<Point> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			this.EnsureReady();
			List<Point> list = new List<Point>();
			int count = points.Count;
			for (int i = 0; i < count; i++)
			{
				Point point = points[i];
				if (this._count == 0)
				{
					this.AddLassoPoint(point);
					list.Add(point);
					this._lasso.Add(point);
					this._boundingBox.Union(point);
					this._firstLassoPoint = point;
					this._lastLassoPoint = point;
					this._count++;
				}
				else
				{
					Vector vector = point - this._lastLassoPoint;
					double lengthSquared = vector.LengthSquared;
					if (DoubleUtil.AreClose(49.0, lengthSquared))
					{
						this.AddLassoPoint(point);
						list.Add(point);
						this._lasso.Add(point);
						this._boundingBox.Union(point);
						this._lastLassoPoint = point;
						this._count++;
					}
					else if (49.0 < lengthSquared)
					{
						double num = Math.Sqrt(49.0 / lengthSquared);
						Point lastLassoPoint = this._lastLassoPoint;
						for (double num2 = num; num2 < 1.0; num2 += num)
						{
							Point point2 = lastLassoPoint + vector * num2;
							this.AddLassoPoint(point2);
							list.Add(point2);
							this._lasso.Add(point2);
							this._boundingBox.Union(point2);
							this._lastLassoPoint = point2;
							this._count++;
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x001F8344 File Offset: 0x001F6544
		private void AddLassoPoint(Point lassoPoint)
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = null;
			try
			{
				drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawEllipse(this._brush, this._pen, lassoPoint, 2.5, 2.5);
			}
			finally
			{
				if (drawingContext != null)
				{
					drawingContext.Close();
				}
			}
			this._containerVisual.Children.Add(drawingVisual);
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x001F83B4 File Offset: 0x001F65B4
		public bool ArePointsInLasso(Point[] points, int percentIntersect)
		{
			int num = points.Length * percentIntersect / 100;
			if (num == 0 || 50 <= points.Length * percentIntersect % 100)
			{
				num++;
			}
			int num2 = 0;
			foreach (Point point in points)
			{
				if (this.Contains(point))
				{
					num2++;
					if (num2 == num)
					{
						break;
					}
				}
			}
			return num2 == num;
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x001F8410 File Offset: 0x001F6610
		private bool Contains(Point point)
		{
			if (!this._boundingBox.Contains(point))
			{
				return false;
			}
			bool flag = false;
			int num = this._lasso.Count;
			while (--num >= 0)
			{
				if (!DoubleUtil.AreClose(this._lasso[num].Y, point.Y))
				{
					flag = (point.Y < this._lasso[num].Y);
					break;
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			Point point2 = this._lasso[this._lasso.Count - 1];
			for (int i = 0; i < this._lasso.Count; i++)
			{
				Point point3 = this._lasso[i];
				if (DoubleUtil.AreClose(point3.Y, point.Y))
				{
					if (DoubleUtil.AreClose(point3.X, point.X))
					{
						flag2 = true;
						break;
					}
					if (i != 0 && DoubleUtil.AreClose(point2.Y, point.Y) && DoubleUtil.GreaterThanOrClose(point.X, Math.Min(point2.X, point3.X)) && DoubleUtil.LessThanOrClose(point.X, Math.Max(point2.X, point3.X)))
					{
						flag2 = true;
						break;
					}
				}
				else if (flag != point.Y < point3.Y)
				{
					flag = !flag;
					if (DoubleUtil.GreaterThanOrClose(point.X, Math.Max(point2.X, point3.X)))
					{
						flag2 = !flag2;
						if (i == 0 && DoubleUtil.AreClose(point.X, Math.Max(point2.X, point3.X)))
						{
							flag3 = true;
						}
					}
					else if (DoubleUtil.GreaterThanOrClose(point.X, Math.Min(point2.X, point3.X)))
					{
						Vector vector = point3 - point2;
						double value = point2.X + vector.X / vector.Y * (point.Y - point2.Y);
						if (DoubleUtil.GreaterThanOrClose(point.X, value))
						{
							flag2 = !flag2;
							if (i == 0 && DoubleUtil.AreClose(point.X, value))
							{
								flag3 = true;
							}
						}
					}
				}
				point2 = point3;
			}
			return flag2 && !flag3;
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x001F8678 File Offset: 0x001F6878
		private void EnsureVisual()
		{
			if (this._containerVisual == null)
			{
				this._containerVisual = new DrawingVisual();
			}
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x001F8690 File Offset: 0x001F6890
		private void EnsureReady()
		{
			if (!this._isActivated)
			{
				this._isActivated = true;
				this.EnsureVisual();
				this._brush = new SolidColorBrush(LassoHelper.DotColor);
				this._brush.Freeze();
				this._pen = new Pen(new SolidColorBrush(LassoHelper.DotCircumferenceColor), 0.5);
				this._pen.LineJoin = PenLineJoin.Round;
				this._pen.Freeze();
				this._lasso = new List<Point>(100);
				this._boundingBox = Rect.Empty;
				this._count = 0;
			}
		}

		// Token: 0x040035FD RID: 13821
		private DrawingVisual _containerVisual;

		// Token: 0x040035FE RID: 13822
		private Brush _brush;

		// Token: 0x040035FF RID: 13823
		private Pen _pen;

		// Token: 0x04003600 RID: 13824
		private bool _isActivated;

		// Token: 0x04003601 RID: 13825
		private Point _firstLassoPoint;

		// Token: 0x04003602 RID: 13826
		private Point _lastLassoPoint;

		// Token: 0x04003603 RID: 13827
		private int _count;

		// Token: 0x04003604 RID: 13828
		private List<Point> _lasso;

		// Token: 0x04003605 RID: 13829
		private Rect _boundingBox;

		// Token: 0x04003606 RID: 13830
		public const double MinDistanceSquared = 49.0;

		// Token: 0x04003607 RID: 13831
		private const double DotRadius = 2.5;

		// Token: 0x04003608 RID: 13832
		private const double DotCircumferenceThickness = 0.5;

		// Token: 0x04003609 RID: 13833
		private const double ConnectLineThickness = 0.75;

		// Token: 0x0400360A RID: 13834
		private const double ConnectLineOpacity = 0.75;

		// Token: 0x0400360B RID: 13835
		private static readonly Color DotColor = Colors.Orange;

		// Token: 0x0400360C RID: 13836
		private static readonly Color DotCircumferenceColor = Colors.White;
	}
}
