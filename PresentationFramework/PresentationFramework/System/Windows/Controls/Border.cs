using System;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Draws a border, background, or both around another element. </summary>
	// Token: 0x02000472 RID: 1138
	public class Border : Decorator
	{
		// Token: 0x0600425A RID: 16986 RVA: 0x0012F808 File Offset: 0x0012DA08
		static Border()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.Border);
		}

		/// <summary>Gets or sets the relative <see cref="T:System.Windows.Thickness" /> of a <see cref="T:System.Windows.Controls.Border" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Thickness" /> that describes the width of the boundaries of the <see cref="T:System.Windows.Controls.Border" />. This property has no default value.</returns>
		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x0012F996 File Offset: 0x0012DB96
		// (set) Token: 0x0600425D RID: 16989 RVA: 0x0012F9A8 File Offset: 0x0012DBA8
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Border.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Border.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Thickness" /> value that describes the amount of space between a <see cref="T:System.Windows.Controls.Border" /> and its child element.  </summary>
		/// <returns>The <see cref="T:System.Windows.Thickness" /> that describes the amount of space between a <see cref="T:System.Windows.Controls.Border" /> and its single child element. This property has no default value.</returns>
		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x0012F9BB File Offset: 0x0012DBBB
		// (set) Token: 0x0600425F RID: 16991 RVA: 0x0012F9CD File Offset: 0x0012DBCD
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Border.PaddingProperty);
			}
			set
			{
				base.SetValue(Border.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the degree to which the corners of a <see cref="T:System.Windows.Controls.Border" /> are rounded.  </summary>
		/// <returns>The <see cref="T:System.Windows.CornerRadius" /> that describes the degree to which corners are rounded. This property has no default value.</returns>
		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x0012F9E0 File Offset: 0x0012DBE0
		// (set) Token: 0x06004261 RID: 16993 RVA: 0x0012F9F2 File Offset: 0x0012DBF2
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(Border.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(Border.CornerRadiusProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that draws the outer border color.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that draws the outer border color. This property has no default value.</returns>
		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06004262 RID: 16994 RVA: 0x0012FA05 File Offset: 0x0012DC05
		// (set) Token: 0x06004263 RID: 16995 RVA: 0x0012FA17 File Offset: 0x0012DC17
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Border.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Border.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that fills the area between the bounds of a <see cref="T:System.Windows.Controls.Border" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that draws the background. This property has no default value.</returns>
		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x0012FA25 File Offset: 0x0012DC25
		// (set) Token: 0x06004265 RID: 16997 RVA: 0x0012FA37 File Offset: 0x0012DC37
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Border.BackgroundProperty);
			}
			set
			{
				base.SetValue(Border.BackgroundProperty, value);
			}
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0012FA48 File Offset: 0x0012DC48
		private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Border border = (Border)d;
			border.LeftPenCache = null;
			border.RightPenCache = null;
			border.TopPenCache = null;
			border.BottomPenCache = null;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0012FA78 File Offset: 0x0012DC78
		private static bool IsThicknessValid(object value)
		{
			return ((Thickness)value).IsValid(false, false, false, false);
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x0012FA98 File Offset: 0x0012DC98
		private static bool IsCornerRadiusValid(object value)
		{
			return ((CornerRadius)value).IsValid(false, false, false, false);
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.Border" /> before they are arranged during the <see cref="M:System.Windows.Controls.Border.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper <see cref="T:System.Windows.Size" /> limit that cannot be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the upper size limit of the element.</returns>
		// Token: 0x06004269 RID: 17001 RVA: 0x0012FAB8 File Offset: 0x0012DCB8
		protected override Size MeasureOverride(Size constraint)
		{
			UIElement child = this.Child;
			Size result = default(Size);
			Thickness borderThickness = this.BorderThickness;
			if (base.UseLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				DpiScale dpi = base.GetDpi();
				borderThickness = new Thickness(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY), UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
			}
			Size size = Border.HelperCollapseThickness(borderThickness);
			Size size2 = Border.HelperCollapseThickness(this.Padding);
			if (child != null)
			{
				Size size3 = new Size(size.Width + size2.Width, size.Height + size2.Height);
				Size availableSize = new Size(Math.Max(0.0, constraint.Width - size3.Width), Math.Max(0.0, constraint.Height - size3.Height));
				child.Measure(availableSize);
				Size desiredSize = child.DesiredSize;
				result.Width = desiredSize.Width + size3.Width;
				result.Height = desiredSize.Height + size3.Height;
			}
			else
			{
				result = new Size(size.Width + size2.Width, size.Height + size2.Height);
			}
			return result;
		}

		/// <summary>Arranges the contents of a <see cref="T:System.Windows.Controls.Border" /> element.</summary>
		/// <param name="finalSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child element.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Border" /> element and its child element.</returns>
		// Token: 0x0600426A RID: 17002 RVA: 0x0012FC2C File Offset: 0x0012DE2C
		protected override Size ArrangeOverride(Size finalSize)
		{
			Thickness borderThickness = this.BorderThickness;
			if (base.UseLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				DpiScale dpi = base.GetDpi();
				borderThickness = new Thickness(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY), UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
			}
			Rect rect = new Rect(finalSize);
			Rect rect2 = Border.HelperDeflateRect(rect, borderThickness);
			UIElement child = this.Child;
			if (child != null)
			{
				Rect finalRect = Border.HelperDeflateRect(rect2, this.Padding);
				child.Arrange(finalRect);
			}
			CornerRadius cornerRadius = this.CornerRadius;
			Brush borderBrush = this.BorderBrush;
			bool flag = Border.AreUniformCorners(cornerRadius);
			this._useComplexRenderCodePath = !flag;
			if (!this._useComplexRenderCodePath && borderBrush != null)
			{
				SolidColorBrush solidColorBrush = borderBrush as SolidColorBrush;
				bool isUniform = borderThickness.IsUniform;
				this._useComplexRenderCodePath = (solidColorBrush == null || (solidColorBrush.Color.A < byte.MaxValue && !isUniform) || (!DoubleUtil.IsZero(cornerRadius.TopLeft) && !isUniform));
			}
			if (this._useComplexRenderCodePath)
			{
				Border.Radii radii = new Border.Radii(cornerRadius, borderThickness, false);
				StreamGeometry streamGeometry = null;
				if (!DoubleUtil.IsZero(rect2.Width) && !DoubleUtil.IsZero(rect2.Height))
				{
					streamGeometry = new StreamGeometry();
					using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
					{
						Border.GenerateGeometry(streamGeometryContext, rect2, radii);
					}
					streamGeometry.Freeze();
					this.BackgroundGeometryCache = streamGeometry;
				}
				else
				{
					this.BackgroundGeometryCache = null;
				}
				if (!DoubleUtil.IsZero(rect.Width) && !DoubleUtil.IsZero(rect.Height))
				{
					Border.Radii radii2 = new Border.Radii(cornerRadius, borderThickness, true);
					StreamGeometry streamGeometry2 = new StreamGeometry();
					using (StreamGeometryContext streamGeometryContext2 = streamGeometry2.Open())
					{
						Border.GenerateGeometry(streamGeometryContext2, rect, radii2);
						if (streamGeometry != null)
						{
							Border.GenerateGeometry(streamGeometryContext2, rect2, radii);
						}
					}
					streamGeometry2.Freeze();
					this.BorderGeometryCache = streamGeometry2;
				}
				else
				{
					this.BorderGeometryCache = null;
				}
			}
			else
			{
				this.BackgroundGeometryCache = null;
				this.BorderGeometryCache = null;
			}
			return finalSize;
		}

		/// <summary>Draws the contents of a <see cref="T:System.Windows.Media.DrawingContext" /> object during the render pass of a <see cref="T:System.Windows.Controls.Border" />. </summary>
		/// <param name="dc">The <see cref="T:System.Windows.Media.DrawingContext" /> that defines the object to be drawn.</param>
		// Token: 0x0600426B RID: 17003 RVA: 0x0012FE74 File Offset: 0x0012E074
		protected override void OnRender(DrawingContext dc)
		{
			bool useLayoutRounding = base.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (this._useComplexRenderCodePath)
			{
				StreamGeometry borderGeometryCache = this.BorderGeometryCache;
				Brush brush;
				if (borderGeometryCache != null && (brush = this.BorderBrush) != null)
				{
					dc.DrawGeometry(brush, null, borderGeometryCache);
				}
				StreamGeometry backgroundGeometryCache = this.BackgroundGeometryCache;
				if (backgroundGeometryCache != null && (brush = this.Background) != null)
				{
					dc.DrawGeometry(brush, null, backgroundGeometryCache);
					return;
				}
			}
			else
			{
				Thickness borderThickness = this.BorderThickness;
				CornerRadius cornerRadius = this.CornerRadius;
				double topLeft = cornerRadius.TopLeft;
				bool flag = !DoubleUtil.IsZero(topLeft);
				Brush borderBrush;
				if (!borderThickness.IsZero && (borderBrush = this.BorderBrush) != null)
				{
					Pen pen = this.LeftPenCache;
					if (pen == null)
					{
						pen = new Pen();
						pen.Brush = borderBrush;
						if (useLayoutRounding)
						{
							pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX);
						}
						else
						{
							pen.Thickness = borderThickness.Left;
						}
						if (borderBrush.IsFrozen)
						{
							pen.Freeze();
						}
						this.LeftPenCache = pen;
					}
					if (borderThickness.IsUniform)
					{
						double num = pen.Thickness * 0.5;
						Rect rectangle = new Rect(new Point(num, num), new Point(base.RenderSize.Width - num, base.RenderSize.Height - num));
						if (flag)
						{
							dc.DrawRoundedRectangle(null, pen, rectangle, topLeft, topLeft);
						}
						else
						{
							dc.DrawRectangle(null, pen, rectangle);
						}
					}
					else
					{
						if (DoubleUtil.GreaterThan(borderThickness.Left, 0.0))
						{
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(num, 0.0), new Point(num, base.RenderSize.Height));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Right, 0.0))
						{
							pen = this.RightPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX);
								}
								else
								{
									pen.Thickness = borderThickness.Right;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.RightPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(base.RenderSize.Width - num, 0.0), new Point(base.RenderSize.Width - num, base.RenderSize.Height));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Top, 0.0))
						{
							pen = this.TopPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY);
								}
								else
								{
									pen.Thickness = borderThickness.Top;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.TopPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(0.0, num), new Point(base.RenderSize.Width, num));
						}
						if (DoubleUtil.GreaterThan(borderThickness.Bottom, 0.0))
						{
							pen = this.BottomPenCache;
							if (pen == null)
							{
								pen = new Pen();
								pen.Brush = borderBrush;
								if (useLayoutRounding)
								{
									pen.Thickness = UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY);
								}
								else
								{
									pen.Thickness = borderThickness.Bottom;
								}
								if (borderBrush.IsFrozen)
								{
									pen.Freeze();
								}
								this.BottomPenCache = pen;
							}
							double num = pen.Thickness * 0.5;
							dc.DrawLine(pen, new Point(0.0, base.RenderSize.Height - num), new Point(base.RenderSize.Width, base.RenderSize.Height - num));
						}
					}
				}
				Brush background = this.Background;
				if (background != null)
				{
					Point point;
					Point point2;
					if (useLayoutRounding)
					{
						point = new Point(UIElement.RoundLayoutValue(borderThickness.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(borderThickness.Top, dpi.DpiScaleY));
						if (FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
						{
							point2 = new Point(UIElement.RoundLayoutValue(base.RenderSize.Width - borderThickness.Right, dpi.DpiScaleX), UIElement.RoundLayoutValue(base.RenderSize.Height - borderThickness.Bottom, dpi.DpiScaleY));
						}
						else
						{
							point2 = new Point(base.RenderSize.Width - UIElement.RoundLayoutValue(borderThickness.Right, dpi.DpiScaleX), base.RenderSize.Height - UIElement.RoundLayoutValue(borderThickness.Bottom, dpi.DpiScaleY));
						}
					}
					else
					{
						point = new Point(borderThickness.Left, borderThickness.Top);
						point2 = new Point(base.RenderSize.Width - borderThickness.Right, base.RenderSize.Height - borderThickness.Bottom);
					}
					if (point2.X > point.X && point2.Y > point.Y)
					{
						if (flag)
						{
							Border.Radii radii = new Border.Radii(cornerRadius, borderThickness, false);
							double topLeft2 = radii.TopLeft;
							dc.DrawRoundedRectangle(background, null, new Rect(point, point2), topLeft2, topLeft2);
							return;
						}
						dc.DrawRectangle(background, null, new Rect(point, point2));
					}
				}
			}
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0013045A File Offset: 0x0012E65A
		private static Size HelperCollapseThickness(Thickness th)
		{
			return new Size(th.Left + th.Right, th.Top + th.Bottom);
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x00130480 File Offset: 0x0012E680
		private static bool AreUniformCorners(CornerRadius borderRadii)
		{
			double topLeft = borderRadii.TopLeft;
			return DoubleUtil.AreClose(topLeft, borderRadii.TopRight) && DoubleUtil.AreClose(topLeft, borderRadii.BottomLeft) && DoubleUtil.AreClose(topLeft, borderRadii.BottomRight);
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x001304C4 File Offset: 0x0012E6C4
		private static Rect HelperDeflateRect(Rect rt, Thickness thick)
		{
			return new Rect(rt.Left + thick.Left, rt.Top + thick.Top, Math.Max(0.0, rt.Width - thick.Left - thick.Right), Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00130540 File Offset: 0x0012E740
		private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, Border.Radii radii)
		{
			Point point = new Point(radii.LeftTop, 0.0);
			Point point2 = new Point(rect.Width - radii.RightTop, 0.0);
			Point point3 = new Point(rect.Width, radii.TopRight);
			Point point4 = new Point(rect.Width, rect.Height - radii.BottomRight);
			Point point5 = new Point(rect.Width - radii.RightBottom, rect.Height);
			Point point6 = new Point(radii.LeftBottom, rect.Height);
			Point point7 = new Point(0.0, rect.Height - radii.BottomLeft);
			Point point8 = new Point(0.0, radii.TopLeft);
			if (point.X > point2.X)
			{
				double x = radii.LeftTop / (radii.LeftTop + radii.RightTop) * rect.Width;
				point.X = x;
				point2.X = x;
			}
			if (point3.Y > point4.Y)
			{
				double y = radii.TopRight / (radii.TopRight + radii.BottomRight) * rect.Height;
				point3.Y = y;
				point4.Y = y;
			}
			if (point5.X < point6.X)
			{
				double x2 = radii.LeftBottom / (radii.LeftBottom + radii.RightBottom) * rect.Width;
				point5.X = x2;
				point6.X = x2;
			}
			if (point7.Y < point8.Y)
			{
				double y2 = radii.TopLeft / (radii.TopLeft + radii.BottomLeft) * rect.Height;
				point7.Y = y2;
				point8.Y = y2;
			}
			Vector vector = new Vector(rect.TopLeft.X, rect.TopLeft.Y);
			point += vector;
			point2 += vector;
			point3 += vector;
			point4 += vector;
			point5 += vector;
			point6 += vector;
			point7 += vector;
			point8 += vector;
			ctx.BeginFigure(point, true, true);
			ctx.LineTo(point2, true, false);
			double num = rect.TopRight.X - point2.X;
			double num2 = point3.Y - rect.TopRight.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point3, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point4, true, false);
			num = rect.BottomRight.X - point5.X;
			num2 = rect.BottomRight.Y - point4.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point5, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point6, true, false);
			num = point6.X - rect.BottomLeft.X;
			num2 = rect.BottomLeft.Y - point7.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point7, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
			ctx.LineTo(point8, true, false);
			num = point.X - rect.TopLeft.X;
			num2 = point8.Y - rect.TopLeft.Y;
			if (!DoubleUtil.IsZero(num) || !DoubleUtil.IsZero(num2))
			{
				ctx.ArcTo(point, new Size(num, num2), 0.0, false, SweepDirection.Clockwise, true, false);
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x0009580F File Offset: 0x00093A0F
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06004271 RID: 17009 RVA: 0x00130949 File Offset: 0x0012EB49
		// (set) Token: 0x06004272 RID: 17010 RVA: 0x00130956 File Offset: 0x0012EB56
		private StreamGeometry BorderGeometryCache
		{
			get
			{
				return Border.BorderGeometryField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BorderGeometryField.ClearValue(this);
					return;
				}
				Border.BorderGeometryField.SetValue(this, value);
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06004273 RID: 17011 RVA: 0x00130973 File Offset: 0x0012EB73
		// (set) Token: 0x06004274 RID: 17012 RVA: 0x00130980 File Offset: 0x0012EB80
		private StreamGeometry BackgroundGeometryCache
		{
			get
			{
				return Border.BackgroundGeometryField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BackgroundGeometryField.ClearValue(this);
					return;
				}
				Border.BackgroundGeometryField.SetValue(this, value);
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06004275 RID: 17013 RVA: 0x0013099D File Offset: 0x0012EB9D
		// (set) Token: 0x06004276 RID: 17014 RVA: 0x001309AA File Offset: 0x0012EBAA
		private Pen LeftPenCache
		{
			get
			{
				return Border.LeftPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.LeftPenField.ClearValue(this);
					return;
				}
				Border.LeftPenField.SetValue(this, value);
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06004277 RID: 17015 RVA: 0x001309C7 File Offset: 0x0012EBC7
		// (set) Token: 0x06004278 RID: 17016 RVA: 0x001309D4 File Offset: 0x0012EBD4
		private Pen RightPenCache
		{
			get
			{
				return Border.RightPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.RightPenField.ClearValue(this);
					return;
				}
				Border.RightPenField.SetValue(this, value);
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06004279 RID: 17017 RVA: 0x001309F1 File Offset: 0x0012EBF1
		// (set) Token: 0x0600427A RID: 17018 RVA: 0x001309FE File Offset: 0x0012EBFE
		private Pen TopPenCache
		{
			get
			{
				return Border.TopPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.TopPenField.ClearValue(this);
					return;
				}
				Border.TopPenField.SetValue(this, value);
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x0600427B RID: 17019 RVA: 0x00130A1B File Offset: 0x0012EC1B
		// (set) Token: 0x0600427C RID: 17020 RVA: 0x00130A28 File Offset: 0x0012EC28
		private Pen BottomPenCache
		{
			get
			{
				return Border.BottomPenField.GetValue(this);
			}
			set
			{
				if (value == null)
				{
					Border.BottomPenField.ClearValue(this);
					return;
				}
				Border.BottomPenField.SetValue(this, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Border.BorderThickness" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Border.BorderThickness" /> dependency property.</returns>
		// Token: 0x040027EC RID: 10220
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Border.OnClearPenCache)), new ValidateValueCallback(Border.IsThicknessValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Border.Padding" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Border.Padding" /> dependency property.</returns>
		// Token: 0x040027ED RID: 10221
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Border), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Border.IsThicknessValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Border.CornerRadius" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Border.CornerRadius" /> dependency property.</returns>
		// Token: 0x040027EE RID: 10222
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Border), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(Border.IsCornerRadiusValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Border.BorderBrush" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Border.BorderBrush" /> dependency property.</returns>
		// Token: 0x040027EF RID: 10223
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Border), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(Border.OnClearPenCache)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Border.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Border.Background" /> dependency property.</returns>
		// Token: 0x040027F0 RID: 10224
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Border), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		// Token: 0x040027F1 RID: 10225
		private bool _useComplexRenderCodePath;

		// Token: 0x040027F2 RID: 10226
		private static readonly UncommonField<StreamGeometry> BorderGeometryField = new UncommonField<StreamGeometry>();

		// Token: 0x040027F3 RID: 10227
		private static readonly UncommonField<StreamGeometry> BackgroundGeometryField = new UncommonField<StreamGeometry>();

		// Token: 0x040027F4 RID: 10228
		private static readonly UncommonField<Pen> LeftPenField = new UncommonField<Pen>();

		// Token: 0x040027F5 RID: 10229
		private static readonly UncommonField<Pen> RightPenField = new UncommonField<Pen>();

		// Token: 0x040027F6 RID: 10230
		private static readonly UncommonField<Pen> TopPenField = new UncommonField<Pen>();

		// Token: 0x040027F7 RID: 10231
		private static readonly UncommonField<Pen> BottomPenField = new UncommonField<Pen>();

		// Token: 0x0200095D RID: 2397
		private struct Radii
		{
			// Token: 0x06008730 RID: 34608 RVA: 0x0024F060 File Offset: 0x0024D260
			internal Radii(CornerRadius radii, Thickness borders, bool outer)
			{
				double num = 0.5 * borders.Left;
				double num2 = 0.5 * borders.Top;
				double num3 = 0.5 * borders.Right;
				double num4 = 0.5 * borders.Bottom;
				if (!outer)
				{
					this.LeftTop = Math.Max(0.0, radii.TopLeft - num);
					this.TopLeft = Math.Max(0.0, radii.TopLeft - num2);
					this.TopRight = Math.Max(0.0, radii.TopRight - num2);
					this.RightTop = Math.Max(0.0, radii.TopRight - num3);
					this.RightBottom = Math.Max(0.0, radii.BottomRight - num3);
					this.BottomRight = Math.Max(0.0, radii.BottomRight - num4);
					this.BottomLeft = Math.Max(0.0, radii.BottomLeft - num4);
					this.LeftBottom = Math.Max(0.0, radii.BottomLeft - num);
					return;
				}
				if (DoubleUtil.IsZero(radii.TopLeft))
				{
					this.LeftTop = (this.TopLeft = 0.0);
				}
				else
				{
					this.LeftTop = radii.TopLeft + num;
					this.TopLeft = radii.TopLeft + num2;
				}
				if (DoubleUtil.IsZero(radii.TopRight))
				{
					this.TopRight = (this.RightTop = 0.0);
				}
				else
				{
					this.TopRight = radii.TopRight + num2;
					this.RightTop = radii.TopRight + num3;
				}
				if (DoubleUtil.IsZero(radii.BottomRight))
				{
					this.RightBottom = (this.BottomRight = 0.0);
				}
				else
				{
					this.RightBottom = radii.BottomRight + num3;
					this.BottomRight = radii.BottomRight + num4;
				}
				if (DoubleUtil.IsZero(radii.BottomLeft))
				{
					this.BottomLeft = (this.LeftBottom = 0.0);
					return;
				}
				this.BottomLeft = radii.BottomLeft + num4;
				this.LeftBottom = radii.BottomLeft + num;
			}

			// Token: 0x040043FE RID: 17406
			internal double LeftTop;

			// Token: 0x040043FF RID: 17407
			internal double TopLeft;

			// Token: 0x04004400 RID: 17408
			internal double TopRight;

			// Token: 0x04004401 RID: 17409
			internal double RightTop;

			// Token: 0x04004402 RID: 17410
			internal double RightBottom;

			// Token: 0x04004403 RID: 17411
			internal double BottomRight;

			// Token: 0x04004404 RID: 17412
			internal double BottomLeft;

			// Token: 0x04004405 RID: 17413
			internal double LeftBottom;
		}
	}
}
