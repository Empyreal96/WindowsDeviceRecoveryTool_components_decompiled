using System;
using System.ComponentModel;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;

namespace System.Windows.Shapes
{
	/// <summary>Provides a base class for shape elements, such as <see cref="T:System.Windows.Shapes.Ellipse" />, <see cref="T:System.Windows.Shapes.Polygon" />, and <see cref="T:System.Windows.Shapes.Rectangle" />.</summary>
	// Token: 0x02000159 RID: 345
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public abstract class Shape : FrameworkElement
	{
		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Stretch" /> enumeration value that describes how the shape fills its allocated space.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.Stretch" /> enumeration values.</returns>
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0003BD16 File Offset: 0x00039F16
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0003BD28 File Offset: 0x00039F28
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Shape.StretchProperty);
			}
			set
			{
				base.SetValue(Shape.StretchProperty, value);
			}
		}

		/// <summary>Gets a value that represents the final rendered <see cref="T:System.Windows.Media.Geometry" /> of a <see cref="T:System.Windows.Shapes.Shape" />.</summary>
		/// <returns>The final rendered <see cref="T:System.Windows.Media.Geometry" /> of a <see cref="T:System.Windows.Shapes.Shape" />.</returns>
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0003BD3C File Offset: 0x00039F3C
		public virtual Geometry RenderedGeometry
		{
			get
			{
				this.EnsureRenderedGeometry();
				Geometry geometry = this._renderedGeometry.CloneCurrentValue();
				if (geometry == null || geometry == Geometry.Empty)
				{
					return Geometry.Empty;
				}
				if (geometry == this._renderedGeometry)
				{
					geometry = geometry.Clone();
					geometry.Freeze();
				}
				return geometry;
			}
		}

		/// <summary>Gets a value that represents a <see cref="T:System.Windows.Media.Transform" /> that is applied to the geometry of a <see cref="T:System.Windows.Shapes.Shape" /> prior to when it is drawn.</summary>
		/// <returns>A <see cref="T:System.Windows.Media.Transform" /> that is applied to the geometry of a <see cref="T:System.Windows.Shapes.Shape" /> prior to when it is drawn.</returns>
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0003BD84 File Offset: 0x00039F84
		public virtual Transform GeometryTransform
		{
			get
			{
				BoxedMatrix value = Shape.StretchMatrixField.GetValue(this);
				if (value == null)
				{
					return Transform.Identity;
				}
				return new MatrixTransform(value.Value);
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0003BDB1 File Offset: 0x00039FB1
		private static void OnPenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Shape)d)._pen = null;
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how the shape's interior is painted. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> that describes how the shape's interior is painted. The default is <see langword="null" />.</returns>
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0003BDBF File Offset: 0x00039FBF
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0003BDD1 File Offset: 0x00039FD1
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(Shape.FillProperty);
			}
			set
			{
				base.SetValue(Shape.FillProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted. </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted. The default is <see langword="null" />.</returns>
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0003BDDF File Offset: 0x00039FDF
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0003BDF1 File Offset: 0x00039FF1
		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(Shape.StrokeProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeProperty, value);
			}
		}

		/// <summary>Gets or sets the width of the <see cref="T:System.Windows.Shapes.Shape" /> outline. </summary>
		/// <returns>The width of the <see cref="T:System.Windows.Shapes.Shape" /> outline.</returns>
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0003BDFF File Offset: 0x00039FFF
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x0003BE11 File Offset: 0x0003A011
		[TypeConverter(typeof(LengthConverter))]
		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that describes the <see cref="T:System.Windows.Shapes.Shape" /> at the start of a <see cref="P:System.Windows.Shapes.Shape.Stroke" />. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.PenLineCap" /> enumeration values. The default is <see cref="F:System.Windows.Media.PenLineCap.Flat" />.</returns>
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000F64 RID: 3940 RVA: 0x0003BE24 File Offset: 0x0003A024
		// (set) Token: 0x06000F65 RID: 3941 RVA: 0x0003BE36 File Offset: 0x0003A036
		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeStartLineCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that describes the <see cref="T:System.Windows.Shapes.Shape" /> at the end of a line. </summary>
		/// <returns>One of the enumeration values for <see cref="T:System.Windows.Media.PenLineCap" />. The default is <see cref="F:System.Windows.Media.PenLineCap.Flat" />.</returns>
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0003BE49 File Offset: 0x0003A049
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0003BE5B File Offset: 0x0003A05B
		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeEndLineCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that specifies how the ends of a dash are drawn. </summary>
		/// <returns>One of the enumeration values for <see cref="T:System.Windows.Media.PenLineCap" />. The default is <see cref="F:System.Windows.Media.PenLineCap.Flat" />. </returns>
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003BE6E File Offset: 0x0003A06E
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x0003BE80 File Offset: 0x0003A080
		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(Shape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineJoin" /> enumeration value that specifies the type of join that is used at the vertices of a <see cref="T:System.Windows.Shapes.Shape" />.</summary>
		/// <returns>One of the enumeration values for <see cref="T:System.Windows.Media.PenLineJoin" /></returns>
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003BE93 File Offset: 0x0003A093
		// (set) Token: 0x06000F6B RID: 3947 RVA: 0x0003BEA5 File Offset: 0x0003A0A5
		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(Shape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeLineJoinProperty, value);
			}
		}

		/// <summary>Gets or sets a limit on the ratio of the miter length to half the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> of a <see cref="T:System.Windows.Shapes.Shape" /> element. </summary>
		/// <returns>The limit on the ratio of the miter length to the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> of a <see cref="T:System.Windows.Shapes.Shape" /> element. This value is always a positive number that is greater than or equal to 1.</returns>
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x0003BEB8 File Offset: 0x0003A0B8
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x0003BECA File Offset: 0x0003A0CA
		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeMiterLimitProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Double" /> that specifies the distance within the dash pattern where a dash begins.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the distance within the dash pattern where a dash begins.</returns>
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0003BEDD File Offset: 0x0003A0DD
		// (set) Token: 0x06000F6F RID: 3951 RVA: 0x0003BEEF File Offset: 0x0003A0EF
		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(Shape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets a collection of <see cref="T:System.Double" /> values that indicate the pattern of dashes and gaps that is used to outline shapes. </summary>
		/// <returns>A collection of <see cref="T:System.Double" /> values that specify the pattern of dashes and gaps. </returns>
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0003BF02 File Offset: 0x0003A102
		// (set) Token: 0x06000F71 RID: 3953 RVA: 0x0003BF14 File Offset: 0x0003A114
		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(Shape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(Shape.StrokeDashArrayProperty, value);
			}
		}

		/// <summary>Measures a <see cref="T:System.Windows.Shapes.Shape" /> during the first layout pass prior to arranging it.</summary>
		/// <param name="constraint">A maximum <see cref="T:System.Windows.Size" /> to not exceed.</param>
		/// <returns>The maximum <see cref="T:System.Windows.Size" /> for the <see cref="T:System.Windows.Shapes.Shape" />.</returns>
		// Token: 0x06000F72 RID: 3954 RVA: 0x0003BF24 File Offset: 0x0003A124
		protected override Size MeasureOverride(Size constraint)
		{
			this.CacheDefiningGeometry();
			Stretch stretch = this.Stretch;
			Size size;
			if (stretch == Stretch.None)
			{
				size = this.GetNaturalSize();
			}
			else
			{
				size = this.GetStretchedRenderSize(stretch, this.GetStrokeThickness(), constraint, this.GetDefiningGeometryBounds());
			}
			if (this.SizeIsInvalidOrEmpty(size))
			{
				size = new Size(0.0, 0.0);
				this._renderedGeometry = Geometry.Empty;
			}
			return size;
		}

		/// <summary>Arranges a <see cref="T:System.Windows.Shapes.Shape" /> by evaluating its <see cref="P:System.Windows.Shapes.Shape.RenderedGeometry" /> and <see cref="P:System.Windows.Shapes.Shape.Stretch" /> properties.</summary>
		/// <param name="finalSize">The final evaluated size of the <see cref="T:System.Windows.Shapes.Shape" />.</param>
		/// <returns>The final size of the arranged <see cref="T:System.Windows.Shapes.Shape" /> element.</returns>
		// Token: 0x06000F73 RID: 3955 RVA: 0x0003BF90 File Offset: 0x0003A190
		protected override Size ArrangeOverride(Size finalSize)
		{
			Stretch stretch = this.Stretch;
			Size size;
			if (stretch == Stretch.None)
			{
				Shape.StretchMatrixField.ClearValue(this);
				this.ResetRenderedGeometry();
				size = finalSize;
			}
			else
			{
				size = this.GetStretchedRenderSizeAndSetStretchMatrix(stretch, this.GetStrokeThickness(), finalSize, this.GetDefiningGeometryBounds());
			}
			if (this.SizeIsInvalidOrEmpty(size))
			{
				size = new Size(0.0, 0.0);
				this._renderedGeometry = Geometry.Empty;
			}
			return size;
		}

		/// <summary>Provides a means to change the default appearance of a <see cref="T:System.Windows.Shapes.Shape" /> element.</summary>
		/// <param name="drawingContext">A <see cref="T:System.Windows.Media.DrawingContext" /> object that is drawn during the rendering pass of this <see cref="T:System.Windows.Shapes.Shape" />.</param>
		// Token: 0x06000F74 RID: 3956 RVA: 0x0003BFFF File Offset: 0x0003A1FF
		protected override void OnRender(DrawingContext drawingContext)
		{
			this.EnsureRenderedGeometry();
			if (this._renderedGeometry != Geometry.Empty)
			{
				drawingContext.DrawGeometry(this.Fill, this.GetPen(), this._renderedGeometry);
			}
		}

		/// <summary>Gets a value that represents the <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="T:System.Windows.Shapes.Shape" />.</returns>
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06000F75 RID: 3957
		protected abstract Geometry DefiningGeometry { get; }

		// Token: 0x06000F76 RID: 3958 RVA: 0x0003C02C File Offset: 0x0003A22C
		internal bool SizeIsInvalidOrEmpty(Size size)
		{
			return DoubleUtil.IsNaN(size.Width) || DoubleUtil.IsNaN(size.Height) || size.IsEmpty;
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0003C054 File Offset: 0x0003A254
		internal bool IsPenNoOp
		{
			get
			{
				double strokeThickness = this.StrokeThickness;
				return this.Stroke == null || DoubleUtil.IsNaN(strokeThickness) || DoubleUtil.IsZero(strokeThickness);
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0003C080 File Offset: 0x0003A280
		internal double GetStrokeThickness()
		{
			if (this.IsPenNoOp)
			{
				return 0.0;
			}
			return Math.Abs(this.StrokeThickness);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0003C0A0 File Offset: 0x0003A2A0
		internal Pen GetPen()
		{
			if (this.IsPenNoOp)
			{
				return null;
			}
			if (this._pen == null)
			{
				double strokeThickness = this.StrokeThickness;
				double thickness = Math.Abs(strokeThickness);
				this._pen = new Pen();
				this._pen.CanBeInheritanceContext = false;
				this._pen.Thickness = thickness;
				this._pen.Brush = this.Stroke;
				this._pen.StartLineCap = this.StrokeStartLineCap;
				this._pen.EndLineCap = this.StrokeEndLineCap;
				this._pen.DashCap = this.StrokeDashCap;
				this._pen.LineJoin = this.StrokeLineJoin;
				this._pen.MiterLimit = this.StrokeMiterLimit;
				DoubleCollection doubleCollection = null;
				bool flag;
				if (base.GetValueSource(Shape.StrokeDashArrayProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection = this.StrokeDashArray;
				}
				double strokeDashOffset = this.StrokeDashOffset;
				if (doubleCollection != null || strokeDashOffset != 0.0)
				{
					this._pen.DashStyle = new DashStyle(doubleCollection, strokeDashOffset);
				}
			}
			return this._pen;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0003C1B8 File Offset: 0x0003A3B8
		internal static bool IsDoubleFiniteNonNegative(object o)
		{
			double num = (double)o;
			return !double.IsInfinity(num) && !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0003C1F0 File Offset: 0x0003A3F0
		internal static bool IsDoubleFinite(object o)
		{
			double num = (double)o;
			return !double.IsInfinity(num) && !DoubleUtil.IsNaN(num);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0003C218 File Offset: 0x0003A418
		internal static bool IsDoubleFiniteOrNaN(object o)
		{
			double d = (double)o;
			return !double.IsInfinity(d);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void CacheDefiningGeometry()
		{
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0003C238 File Offset: 0x0003A438
		internal Size GetStretchedRenderSize(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
		{
			double num;
			double num2;
			double num3;
			double num4;
			Size result;
			this.GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out num, out num2, out num3, out num4, out result);
			return result;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0003C25C File Offset: 0x0003A45C
		internal Size GetStretchedRenderSizeAndSetStretchMatrix(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds)
		{
			double scaleX;
			double scaleY;
			double offsetX;
			double offsetY;
			Size result;
			this.GetStretchMetrics(mode, strokeThickness, availableSize, geometryBounds, out scaleX, out scaleY, out offsetX, out offsetY, out result);
			Matrix identity = Matrix.Identity;
			identity.ScaleAt(scaleX, scaleY, geometryBounds.Location.X, geometryBounds.Location.Y);
			identity.Translate(offsetX, offsetY);
			Shape.StretchMatrixField.SetValue(this, new BoxedMatrix(identity));
			this.ResetRenderedGeometry();
			return result;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003C2D1 File Offset: 0x0003A4D1
		internal void ResetRenderedGeometry()
		{
			this._renderedGeometry = null;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0003C2DC File Offset: 0x0003A4DC
		internal void GetStretchMetrics(Stretch mode, double strokeThickness, Size availableSize, Rect geometryBounds, out double xScale, out double yScale, out double dX, out double dY, out Size stretchedSize)
		{
			if (!geometryBounds.IsEmpty)
			{
				double num = strokeThickness / 2.0;
				bool flag = false;
				xScale = Math.Max(availableSize.Width - strokeThickness, 0.0);
				yScale = Math.Max(availableSize.Height - strokeThickness, 0.0);
				dX = num - geometryBounds.Left;
				dY = num - geometryBounds.Top;
				if (geometryBounds.Width > xScale * 5E-324)
				{
					xScale /= geometryBounds.Width;
				}
				else
				{
					xScale = 1.0;
					if (geometryBounds.Width == 0.0)
					{
						flag = true;
					}
				}
				if (geometryBounds.Height > yScale * 5E-324)
				{
					yScale /= geometryBounds.Height;
				}
				else
				{
					yScale = 1.0;
					if (geometryBounds.Height == 0.0)
					{
						flag = true;
					}
				}
				if (mode != Stretch.Fill && !flag)
				{
					if (mode == Stretch.Uniform)
					{
						if (yScale > xScale)
						{
							yScale = xScale;
						}
						else
						{
							xScale = yScale;
						}
					}
					else if (xScale > yScale)
					{
						yScale = xScale;
					}
					else
					{
						xScale = yScale;
					}
				}
				stretchedSize = new Size(geometryBounds.Width * xScale + strokeThickness, geometryBounds.Height * yScale + strokeThickness);
				return;
			}
			xScale = (yScale = 1.0);
			dX = (dY = 0.0);
			stretchedSize = new Size(0.0, 0.0);
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0003C480 File Offset: 0x0003A680
		internal virtual Size GetNaturalSize()
		{
			Geometry definingGeometry = this.DefiningGeometry;
			Pen pen = this.GetPen();
			DashStyle dashStyle = null;
			if (pen != null)
			{
				dashStyle = pen.DashStyle;
				if (dashStyle != null)
				{
					pen.DashStyle = null;
				}
			}
			Rect renderBounds = definingGeometry.GetRenderBounds(pen);
			if (dashStyle != null)
			{
				pen.DashStyle = dashStyle;
			}
			return new Size(Math.Max(renderBounds.Right, 0.0), Math.Max(renderBounds.Bottom, 0.0));
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0003C4F4 File Offset: 0x0003A6F4
		internal virtual Rect GetDefiningGeometryBounds()
		{
			Geometry definingGeometry = this.DefiningGeometry;
			return definingGeometry.Bounds;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0003C510 File Offset: 0x0003A710
		internal void EnsureRenderedGeometry()
		{
			if (this._renderedGeometry == null)
			{
				this._renderedGeometry = this.DefiningGeometry;
				if (this.Stretch != Stretch.None)
				{
					Geometry geometry = this._renderedGeometry.CloneCurrentValue();
					if (this._renderedGeometry == geometry)
					{
						this._renderedGeometry = geometry.Clone();
					}
					else
					{
						this._renderedGeometry = geometry;
					}
					Transform transform = this._renderedGeometry.Transform;
					BoxedMatrix value = Shape.StretchMatrixField.GetValue(this);
					Matrix matrix = (value == null) ? Matrix.Identity : value.Value;
					if (transform == null || transform.IsIdentity)
					{
						this._renderedGeometry.Transform = new MatrixTransform(matrix);
						return;
					}
					this._renderedGeometry.Transform = new MatrixTransform(transform.Value * matrix);
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.Stretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.Stretch" /> dependency property.</returns>
		// Token: 0x0400118D RID: 4493
		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Shape), new FrameworkPropertyMetadata(Stretch.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.Fill" /> dependency property. This field is read-only. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.Fill" /> dependency property.</returns>
		// Token: 0x0400118E RID: 4494
		[CommonDependencyProperty]
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Shape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.Stroke" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.Stroke" /> dependency property.</returns>
		// Token: 0x0400118F RID: 4495
		[CommonDependencyProperty]
		public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(Shape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> dependency property.</returns>
		// Token: 0x04001190 RID: 4496
		[CommonDependencyProperty]
		public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeStartLineCap" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeStartLineCap" /> dependency property.</returns>
		// Token: 0x04001191 RID: 4497
		public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeEndLineCap" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeEndLineCap" /> dependency property.</returns>
		// Token: 0x04001192 RID: 4498
		public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeDashCap" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeDashCap" /> dependency property.</returns>
		// Token: 0x04001193 RID: 4499
		public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(Shape), new FrameworkPropertyMetadata(PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineCapValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeLineJoin" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeLineJoin" /> dependency property.</returns>
		// Token: 0x04001194 RID: 4500
		public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(Shape), new FrameworkPropertyMetadata(PenLineJoin.Miter, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)), new ValidateValueCallback(ValidateEnums.IsPenLineJoinValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeMiterLimit" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeMiterLimit" /> dependency property.</returns>
		// Token: 0x04001195 RID: 4501
		public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeDashOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeDashOffset" /> dependency property.</returns>
		// Token: 0x04001196 RID: 4502
		public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(Shape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Shape.StrokeDashArray" /> dependency property.  </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Shape.StrokeDashArray" /> dependency property.</returns>
		// Token: 0x04001197 RID: 4503
		public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(Shape), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Shape.OnPenChanged)));

		// Token: 0x04001198 RID: 4504
		private Pen _pen;

		// Token: 0x04001199 RID: 4505
		private Geometry _renderedGeometry = Geometry.Empty;

		// Token: 0x0400119A RID: 4506
		private static UncommonField<BoxedMatrix> StretchMatrixField = new UncommonField<BoxedMatrix>(null);
	}
}
