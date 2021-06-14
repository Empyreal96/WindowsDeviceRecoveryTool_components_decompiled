using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x0200035F RID: 863
	internal sealed class GeometryWalker : CapacityStreamGeometryContext
	{
		// Token: 0x06002DE6 RID: 11750 RVA: 0x000CE5A1 File Offset: 0x000CC7A1
		public GeometryWalker(FixedSOMPageConstructor pageConstructor)
		{
			this._pageConstructor = pageConstructor;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000CE5B0 File Offset: 0x000CC7B0
		public void FindLines(StreamGeometry geometry, bool stroke, bool fill, Matrix trans)
		{
			this._transform = trans;
			this._fill = fill;
			this._stroke = stroke;
			PathGeometry.ParsePathGeometryData(geometry.GetPathGeometryData(), this);
			this.CheckCloseFigure();
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000CE5DC File Offset: 0x000CC7DC
		private void CheckCloseFigure()
		{
			if (this._needClose)
			{
				if (this._stroke && this._isClosed)
				{
					this._pageConstructor._AddLine(this._startPoint, this._lastPoint, this._transform);
				}
				if (this._fill && this._isFilled)
				{
					this._pageConstructor._ProcessFilledRect(this._transform, new Rect(this._xMin, this._yMin, this._xMax - this._xMin, this._yMax - this._yMin));
				}
				this._needClose = false;
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000CE678 File Offset: 0x000CC878
		private void GatherBounds(Point p)
		{
			if (p.X < this._xMin)
			{
				this._xMin = p.X;
			}
			else if (p.X > this._xMax)
			{
				this._xMax = p.X;
			}
			if (p.Y < this._yMin)
			{
				this._yMin = p.Y;
				return;
			}
			if (p.Y > this._yMax)
			{
				this._yMax = p.Y;
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000CE6F8 File Offset: 0x000CC8F8
		public override void BeginFigure(Point startPoint, bool isFilled, bool isClosed)
		{
			this.CheckCloseFigure();
			this._startPoint = startPoint;
			this._lastPoint = startPoint;
			this._isClosed = isClosed;
			this._isFilled = isFilled;
			if (this._isFilled && this._fill)
			{
				this._xMin = (this._xMax = startPoint.X);
				this._yMin = (this._yMax = startPoint.Y);
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000CE764 File Offset: 0x000CC964
		public override void LineTo(Point point, bool isStroked, bool isSmoothJoin)
		{
			if (isStroked && this._stroke)
			{
				this._pageConstructor._AddLine(this._lastPoint, point, this._transform);
			}
			if (this._isFilled && this._fill)
			{
				this.GatherBounds(point);
			}
			this._lastPoint = point;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000CE7B2 File Offset: 0x000CC9B2
		public override void QuadraticBezierTo(Point point1, Point point2, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point2;
			this._fill = false;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000CE7C2 File Offset: 0x000CC9C2
		public override void BezierTo(Point point1, Point point2, Point point3, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point3;
			this._fill = false;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000CE7D4 File Offset: 0x000CC9D4
		public override void PolyLineTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			if (isStroked && this._stroke)
			{
				for (int i = 0; i < points.Count; i++)
				{
					this._pageConstructor._AddLine(this._lastPoint, points[i], this._transform);
					this._lastPoint = points[i];
				}
			}
			else
			{
				this._lastPoint = points[points.Count - 1];
			}
			if (this._isFilled && this._fill)
			{
				for (int j = 0; j < points.Count; j++)
				{
					this.GatherBounds(points[j]);
				}
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000CE86C File Offset: 0x000CCA6C
		public override void PolyQuadraticBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = points[points.Count - 1];
			this._fill = false;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000CE86C File Offset: 0x000CCA6C
		public override void PolyBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = points[points.Count - 1];
			this._fill = false;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000CE889 File Offset: 0x000CCA89
		public override void ArcTo(Point point, Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection, bool isStroked, bool isSmoothJoin)
		{
			this._lastPoint = point;
			this._fill = false;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x00002137 File Offset: 0x00000337
		internal override void SetClosedState(bool closed)
		{
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x00002137 File Offset: 0x00000337
		internal override void SetFigureCount(int figureCount)
		{
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000CE899 File Offset: 0x000CCA99
		internal override void SetSegmentCount(int segmentCount)
		{
			if (segmentCount != 0)
			{
				this._needClose = true;
			}
		}

		// Token: 0x04001DD3 RID: 7635
		private FixedSOMPageConstructor _pageConstructor;

		// Token: 0x04001DD4 RID: 7636
		private Matrix _transform;

		// Token: 0x04001DD5 RID: 7637
		private bool _stroke;

		// Token: 0x04001DD6 RID: 7638
		private bool _fill;

		// Token: 0x04001DD7 RID: 7639
		private Point _startPoint;

		// Token: 0x04001DD8 RID: 7640
		private Point _lastPoint;

		// Token: 0x04001DD9 RID: 7641
		private bool _isClosed;

		// Token: 0x04001DDA RID: 7642
		private bool _isFilled;

		// Token: 0x04001DDB RID: 7643
		private double _xMin;

		// Token: 0x04001DDC RID: 7644
		private double _xMax;

		// Token: 0x04001DDD RID: 7645
		private double _yMin;

		// Token: 0x04001DDE RID: 7646
		private double _yMax;

		// Token: 0x04001DDF RID: 7647
		private bool _needClose;
	}
}
