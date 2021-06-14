using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shapes
{
	/// <summary>Draws a series of connected straight lines. </summary>
	// Token: 0x02000157 RID: 343
	public sealed class Polyline : Shape
	{
		/// <summary>Gets or sets a collection that contains the vertex points of the <see cref="T:System.Windows.Shapes.Polyline" />.  </summary>
		/// <returns>A collection of <see cref="T:System.Windows.Point" /> structures that describe the vertex points of the <see cref="T:System.Windows.Shapes.Polyline" />. The default is a null  reference (<see langword="Nothing" /> in Visual Basic).</returns>
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0003B809 File Offset: 0x00039A09
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0003B81B File Offset: 0x00039A1B
		public PointCollection Points
		{
			get
			{
				return (PointCollection)base.GetValue(Polyline.PointsProperty);
			}
			set
			{
				base.SetValue(Polyline.PointsProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.FillRule" /> enumeration that specifies how the interior fill of the shape is determined.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.FillRule" /> enumeration values. The default is <see cref="F:System.Windows.Media.FillRule.EvenOdd" />.</returns>
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0003B829 File Offset: 0x00039A29
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0003B83B File Offset: 0x00039A3B
		public FillRule FillRule
		{
			get
			{
				return (FillRule)base.GetValue(Polyline.FillRuleProperty);
			}
			set
			{
				base.SetValue(Polyline.FillRuleProperty, value);
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0003B84E File Offset: 0x00039A4E
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._polylineGeometry;
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0003B858 File Offset: 0x00039A58
		internal override void CacheDefiningGeometry()
		{
			PointCollection points = this.Points;
			PathFigure pathFigure = new PathFigure();
			if (points == null)
			{
				this._polylineGeometry = Geometry.Empty;
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
			}
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures.Add(pathFigure);
			pathGeometry.FillRule = this.FillRule;
			if (pathGeometry.Bounds == Rect.Empty)
			{
				this._polylineGeometry = Geometry.Empty;
				return;
			}
			this._polylineGeometry = pathGeometry;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Polyline.Points" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Polyline.Points" /> dependency property.</returns>
		// Token: 0x04001187 RID: 4487
		public static readonly DependencyProperty PointsProperty = DependencyProperty.Register("Points", typeof(PointCollection), typeof(Polyline), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(PointCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Polyline.FillRule" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Polyline.FillRule" /> dependency property.</returns>
		// Token: 0x04001188 RID: 4488
		public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register("FillRule", typeof(FillRule), typeof(Polyline), new FrameworkPropertyMetadata(FillRule.EvenOdd, FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(ValidateEnums.IsFillRuleValid));

		// Token: 0x04001189 RID: 4489
		private Geometry _polylineGeometry;
	}
}
