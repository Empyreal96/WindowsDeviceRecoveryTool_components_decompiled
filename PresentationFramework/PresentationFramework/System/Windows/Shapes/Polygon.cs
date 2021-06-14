using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shapes
{
	/// <summary>Draws a polygon, which is a connected series of lines that form a closed shape.</summary>
	// Token: 0x02000156 RID: 342
	public sealed class Polygon : Shape
	{
		/// <summary>Gets or sets a collection that contains the vertex points of the polygon.   </summary>
		/// <returns>A collection of <see cref="T:System.Windows.Point" /> structures that describe the vertex points of the polygon. The default is a null reference (<see langword="Nothing" /> in Visual Basic).</returns>
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0003B680 File Offset: 0x00039880
		// (set) Token: 0x06000F3A RID: 3898 RVA: 0x0003B692 File Offset: 0x00039892
		public PointCollection Points
		{
			get
			{
				return (PointCollection)base.GetValue(Polygon.PointsProperty);
			}
			set
			{
				base.SetValue(Polygon.PointsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.FillRule" /> enumeration that specifies how the interior fill of the shape is determined.   </summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.FillRule" /> enumeration values. The default is <see cref="F:System.Windows.Media.FillRule.EvenOdd" />.</returns>
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0003B6A0 File Offset: 0x000398A0
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x0003B6B2 File Offset: 0x000398B2
		public FillRule FillRule
		{
			get
			{
				return (FillRule)base.GetValue(Polygon.FillRuleProperty);
			}
			set
			{
				base.SetValue(Polygon.FillRuleProperty, value);
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0003B6C5 File Offset: 0x000398C5
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._polygonGeometry;
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0003B6D0 File Offset: 0x000398D0
		internal override void CacheDefiningGeometry()
		{
			PointCollection points = this.Points;
			PathFigure pathFigure = new PathFigure();
			if (points == null)
			{
				this._polygonGeometry = Geometry.Empty;
				return;
			}
			if (points.Count > 0)
			{
				pathFigure.StartPoint = points[0];
				if (points.Count > 1)
				{
					Point[] array = new Point[points.Count - 1];
					for (int i = 1; i < points.Count; i++)
					{
						array[i - 1] = points[i];
					}
					pathFigure.Segments.Add(new PolyLineSegment(array, true));
				}
				pathFigure.IsClosed = true;
			}
			this._polygonGeometry = new PathGeometry
			{
				Figures = 
				{
					pathFigure
				},
				FillRule = this.FillRule
			};
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Polygon.Points" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Polygon.Points" /> dependency property.</returns>
		// Token: 0x04001184 RID: 4484
		public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(Polygon), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(PointCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Polygon.FillRule" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Polygon.FillRule" /> dependency property.</returns>
		// Token: 0x04001185 RID: 4485
		public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register("FillRule", typeof(FillRule), typeof(Polygon), new FrameworkPropertyMetadata(FillRule.EvenOdd, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(ValidateEnums.IsFillRuleValid));

		// Token: 0x04001186 RID: 4486
		private Geometry _polygonGeometry;
	}
}
