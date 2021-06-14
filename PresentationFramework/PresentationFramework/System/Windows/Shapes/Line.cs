using System;
using System.ComponentModel;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	/// <summary>Draws a straight line between two points. </summary>
	// Token: 0x02000154 RID: 340
	public sealed class Line : Shape
	{
		/// <summary>Gets or sets the x-coordinate of the <see cref="T:System.Windows.Shapes.Line" /> start point.  </summary>
		/// <returns>The x-coordinate for the start point of the line. The default is 0.</returns>
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0003B417 File Offset: 0x00039617
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x0003B429 File Offset: 0x00039629
		[TypeConverter(typeof(LengthConverter))]
		public double X1
		{
			get
			{
				return (double)base.GetValue(Line.X1Property);
			}
			set
			{
				base.SetValue(Line.X1Property, value);
			}
		}

		/// <summary>Gets or sets the y-coordinate of the <see cref="T:System.Windows.Shapes.Line" /> start point.  </summary>
		/// <returns>The y-coordinate for the start point of the line. The default is 0.</returns>
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0003B43C File Offset: 0x0003963C
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x0003B44E File Offset: 0x0003964E
		[TypeConverter(typeof(LengthConverter))]
		public double Y1
		{
			get
			{
				return (double)base.GetValue(Line.Y1Property);
			}
			set
			{
				base.SetValue(Line.Y1Property, value);
			}
		}

		/// <summary>Gets or sets the x-coordinate of the <see cref="T:System.Windows.Shapes.Line" /> end point.  </summary>
		/// <returns>The x-coordinate for the end point of the line. The default is 0.</returns>
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0003B461 File Offset: 0x00039661
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0003B473 File Offset: 0x00039673
		[TypeConverter(typeof(LengthConverter))]
		public double X2
		{
			get
			{
				return (double)base.GetValue(Line.X2Property);
			}
			set
			{
				base.SetValue(Line.X2Property, value);
			}
		}

		/// <summary>Gets or sets the y-coordinate of the <see cref="T:System.Windows.Shapes.Line" /> end point.  </summary>
		/// <returns>The y-coordinate for the end point of the line. The default is 0.</returns>
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0003B486 File Offset: 0x00039686
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x0003B498 File Offset: 0x00039698
		[TypeConverter(typeof(LengthConverter))]
		public double Y2
		{
			get
			{
				return (double)base.GetValue(Line.Y2Property);
			}
			set
			{
				base.SetValue(Line.Y2Property, value);
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0003B4AB File Offset: 0x000396AB
		protected override Geometry DefiningGeometry
		{
			get
			{
				return this._lineGeometry;
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0003B4B4 File Offset: 0x000396B4
		internal override void CacheDefiningGeometry()
		{
			Point startPoint = new Point(this.X1, this.Y1);
			Point endPoint = new Point(this.X2, this.Y2);
			this._lineGeometry = new LineGeometry(startPoint, endPoint);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Line.X1" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Line.X1" /> dependency property.</returns>
		// Token: 0x0400117E RID: 4478
		public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Line.Y1" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Line.Y1" /> dependency property.</returns>
		// Token: 0x0400117F RID: 4479
		public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Line.X2" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Line.X2" /> dependency property.</returns>
		// Token: 0x04001180 RID: 4480
		public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Line.Y2" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Line.Y2" /> dependency property.</returns>
		// Token: 0x04001181 RID: 4481
		public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Shape.IsDoubleFinite));

		// Token: 0x04001182 RID: 4482
		private LineGeometry _lineGeometry;
	}
}
