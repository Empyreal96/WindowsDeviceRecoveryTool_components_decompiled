using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that draws a set of tick marks for a <see cref="T:System.Windows.Controls.Slider" /> control.</summary>
	// Token: 0x020005AF RID: 1455
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class TickBar : FrameworkElement
	{
		// Token: 0x060060CA RID: 24778 RVA: 0x001B2078 File Offset: 0x001B0278
		static TickBar()
		{
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(TickBar), new FrameworkPropertyMetadata(true));
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that is used to draw the tick marks.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> to use to draw tick marks. The default value is <see langword="null" />.</returns>
		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x060060CC RID: 24780 RVA: 0x001B229F File Offset: 0x001B049F
		// (set) Token: 0x060060CD RID: 24781 RVA: 0x001B22B1 File Offset: 0x001B04B1
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(TickBar.FillProperty);
			}
			set
			{
				base.SetValue(TickBar.FillProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum value that is possible for a tick mark.  </summary>
		/// <returns>The smallest possible value for a tick mark. The default value is zero (0.0).</returns>
		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x060060CE RID: 24782 RVA: 0x001B22BF File Offset: 0x001B04BF
		// (set) Token: 0x060060CF RID: 24783 RVA: 0x001B22D1 File Offset: 0x001B04D1
		[Bindable(true)]
		[Category("Appearance")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(TickBar.MinimumProperty);
			}
			set
			{
				base.SetValue(TickBar.MinimumProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum value that is possible for a tick mark.  </summary>
		/// <returns>The largest possible value for a tick mark. The default value is 100.0.</returns>
		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x060060D0 RID: 24784 RVA: 0x001B22E4 File Offset: 0x001B04E4
		// (set) Token: 0x060060D1 RID: 24785 RVA: 0x001B22F6 File Offset: 0x001B04F6
		[Bindable(true)]
		[Category("Appearance")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(TickBar.MaximumProperty);
			}
			set
			{
				base.SetValue(TickBar.MaximumProperty, value);
			}
		}

		/// <summary>Gets or sets the start point of a selection.   </summary>
		/// <returns>The first value in a range of values for a selection. The default value is -1.0.</returns>
		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x060060D2 RID: 24786 RVA: 0x001B2309 File Offset: 0x001B0509
		// (set) Token: 0x060060D3 RID: 24787 RVA: 0x001B231B File Offset: 0x001B051B
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionStart
		{
			get
			{
				return (double)base.GetValue(TickBar.SelectionStartProperty);
			}
			set
			{
				base.SetValue(TickBar.SelectionStartProperty, value);
			}
		}

		/// <summary>Gets or sets the end point of a selection.  </summary>
		/// <returns>The last value in a range of values for a selection. The default value is -1.0.</returns>
		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x060060D4 RID: 24788 RVA: 0x001B232E File Offset: 0x001B052E
		// (set) Token: 0x060060D5 RID: 24789 RVA: 0x001B2340 File Offset: 0x001B0540
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionEnd
		{
			get
			{
				return (double)base.GetValue(TickBar.SelectionEndProperty);
			}
			set
			{
				base.SetValue(TickBar.SelectionEndProperty, value);
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Controls.Primitives.TickBar" /> displays a selection range.   </summary>
		/// <returns>
		///     <see langword="true" /> if a selection range is displayed; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x060060D6 RID: 24790 RVA: 0x001B2353 File Offset: 0x001B0553
		// (set) Token: 0x060060D7 RID: 24791 RVA: 0x001B2365 File Offset: 0x001B0565
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get
			{
				return (bool)base.GetValue(TickBar.IsSelectionRangeEnabledProperty);
			}
			set
			{
				base.SetValue(TickBar.IsSelectionRangeEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets the interval between tick marks.  </summary>
		/// <returns>The distance between tick marks. The default value is one (1.0).</returns>
		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x060060D8 RID: 24792 RVA: 0x001B2378 File Offset: 0x001B0578
		// (set) Token: 0x060060D9 RID: 24793 RVA: 0x001B238A File Offset: 0x001B058A
		[Bindable(true)]
		[Category("Appearance")]
		public double TickFrequency
		{
			get
			{
				return (double)base.GetValue(TickBar.TickFrequencyProperty);
			}
			set
			{
				base.SetValue(TickBar.TickFrequencyProperty, value);
			}
		}

		/// <summary>Gets or sets the positions of the tick marks.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.DoubleCollection" /> that specifies the locations of the tick marks. The default value is <see langword="null" />.</returns>
		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x060060DA RID: 24794 RVA: 0x001B239D File Offset: 0x001B059D
		// (set) Token: 0x060060DB RID: 24795 RVA: 0x001B23AF File Offset: 0x001B05AF
		[Bindable(true)]
		[Category("Appearance")]
		public DoubleCollection Ticks
		{
			get
			{
				return (DoubleCollection)base.GetValue(TickBar.TicksProperty);
			}
			set
			{
				base.SetValue(TickBar.TicksProperty, value);
			}
		}

		/// <summary>Gets or sets the direction of increasing value of the tick marks.  </summary>
		/// <returns>
		///     <see langword="true" /> if the direction of increasing value is to the left for a horizontal <see cref="T:System.Windows.Controls.Slider" /> or down for a vertical <see cref="T:System.Windows.Controls.Slider" />; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x060060DC RID: 24796 RVA: 0x001B23BD File Offset: 0x001B05BD
		// (set) Token: 0x060060DD RID: 24797 RVA: 0x001B23CF File Offset: 0x001B05CF
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(TickBar.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(TickBar.IsDirectionReversedProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets where tick marks appear  relative to a <see cref="T:System.Windows.Controls.Primitives.Track" /> of a <see cref="T:System.Windows.Controls.Slider" /> control.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Primitives.TickBarPlacement" /> enumeration value that identifies the position of the <see cref="T:System.Windows.Controls.Primitives.TickBar" /> in the <see cref="T:System.Windows.Style" /> layout of a <see cref="T:System.Windows.Controls.Slider" />. The default value is <see cref="F:System.Windows.Controls.Primitives.TickBarPlacement.Top" />.</returns>
		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x060060DE RID: 24798 RVA: 0x001B23E2 File Offset: 0x001B05E2
		// (set) Token: 0x060060DF RID: 24799 RVA: 0x001B23F4 File Offset: 0x001B05F4
		[Bindable(true)]
		[Category("Appearance")]
		public TickBarPlacement Placement
		{
			get
			{
				return (TickBarPlacement)base.GetValue(TickBar.PlacementProperty);
			}
			set
			{
				base.SetValue(TickBar.PlacementProperty, value);
			}
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x001B2408 File Offset: 0x001B0608
		private static bool IsValidTickBarPlacement(object o)
		{
			TickBarPlacement tickBarPlacement = (TickBarPlacement)o;
			return tickBarPlacement == TickBarPlacement.Left || tickBarPlacement == TickBarPlacement.Top || tickBarPlacement == TickBarPlacement.Right || tickBarPlacement == TickBarPlacement.Bottom;
		}

		/// <summary>Gets or sets a space buffer for the area that contains the tick marks that are specified for a <see cref="T:System.Windows.Controls.Primitives.TickBar" />.  </summary>
		/// <returns>A value that represents the total buffer area on either side of the row or column of tick marks. The default value is zero (0.0).</returns>
		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x060060E1 RID: 24801 RVA: 0x001B242D File Offset: 0x001B062D
		// (set) Token: 0x060060E2 RID: 24802 RVA: 0x001B243F File Offset: 0x001B063F
		[Bindable(true)]
		[Category("Appearance")]
		public double ReservedSpace
		{
			get
			{
				return (double)base.GetValue(TickBar.ReservedSpaceProperty);
			}
			set
			{
				base.SetValue(TickBar.ReservedSpaceProperty, value);
			}
		}

		/// <summary>Draws the tick marks for a <see cref="T:System.Windows.Controls.Slider" /> control. </summary>
		/// <param name="dc">The <see cref="T:System.Windows.Media.DrawingContext" /> that is used to draw the ticks.</param>
		// Token: 0x060060E3 RID: 24803 RVA: 0x001B2454 File Offset: 0x001B0654
		protected override void OnRender(DrawingContext dc)
		{
			Size size = new Size(base.ActualWidth, base.ActualHeight);
			double num = this.Maximum - this.Minimum;
			double num2 = 0.0;
			double num3 = 1.0;
			double num4 = 1.0;
			Point point = new Point(0.0, 0.0);
			Point point2 = new Point(0.0, 0.0);
			double num5 = this.ReservedSpace * 0.5;
			switch (this.Placement)
			{
			case TickBarPlacement.Left:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Height))
				{
					return;
				}
				size.Height -= this.ReservedSpace;
				num2 = -size.Width;
				point = new Point(size.Width, size.Height + num5);
				point2 = new Point(size.Width, num5);
				num3 = size.Height / num * -1.0;
				num4 = -1.0;
				break;
			case TickBarPlacement.Top:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Width))
				{
					return;
				}
				size.Width -= this.ReservedSpace;
				num2 = -size.Height;
				point = new Point(num5, size.Height);
				point2 = new Point(num5 + size.Width, size.Height);
				num3 = size.Width / num;
				num4 = 1.0;
				break;
			case TickBarPlacement.Right:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Height))
				{
					return;
				}
				size.Height -= this.ReservedSpace;
				num2 = size.Width;
				point = new Point(0.0, size.Height + num5);
				point2 = new Point(0.0, num5);
				num3 = size.Height / num * -1.0;
				num4 = -1.0;
				break;
			case TickBarPlacement.Bottom:
				if (DoubleUtil.GreaterThanOrClose(this.ReservedSpace, size.Width))
				{
					return;
				}
				size.Width -= this.ReservedSpace;
				num2 = size.Height;
				point = new Point(num5, 0.0);
				point2 = new Point(num5 + size.Width, 0.0);
				num3 = size.Width / num;
				num4 = 1.0;
				break;
			}
			double num6 = num2 * 0.75;
			if (this.IsDirectionReversed)
			{
				num4 = -num4;
				num3 *= -1.0;
				Point point3 = point;
				point = point2;
				point2 = point3;
			}
			Pen pen = new Pen(this.Fill, 1.0);
			bool snapsToDevicePixels = base.SnapsToDevicePixels;
			DoubleCollection doubleCollection = snapsToDevicePixels ? new DoubleCollection() : null;
			DoubleCollection doubleCollection2 = snapsToDevicePixels ? new DoubleCollection() : null;
			if (this.Placement == TickBarPlacement.Left || this.Placement == TickBarPlacement.Right)
			{
				double num7 = this.TickFrequency;
				if (num7 > 0.0)
				{
					double num8 = (this.Maximum - this.Minimum) / size.Height;
					if (num7 < num8)
					{
						num7 = num8;
					}
				}
				dc.DrawLine(pen, point, new Point(point.X + num2, point.Y));
				dc.DrawLine(pen, new Point(point.X, point2.Y), new Point(point.X + num2, point2.Y));
				if (snapsToDevicePixels)
				{
					doubleCollection.Add(point.X);
					doubleCollection2.Add(point.Y - 0.5);
					doubleCollection.Add(point.X + num2);
					doubleCollection2.Add(point2.Y - 0.5);
					doubleCollection.Add(point.X + num6);
				}
				DoubleCollection doubleCollection3 = null;
				bool flag;
				if (base.GetValueSource(TickBar.TicksProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection3 = this.Ticks;
				}
				if (doubleCollection3 != null && doubleCollection3.Count > 0)
				{
					for (int i = 0; i < doubleCollection3.Count; i++)
					{
						if (!DoubleUtil.LessThanOrClose(doubleCollection3[i], this.Minimum) && !DoubleUtil.GreaterThanOrClose(doubleCollection3[i], this.Maximum))
						{
							double num9 = doubleCollection3[i] - this.Minimum;
							double num10 = num9 * num3 + point.Y;
							dc.DrawLine(pen, new Point(point.X, num10), new Point(point.X + num6, num10));
							if (snapsToDevicePixels)
							{
								doubleCollection2.Add(num10 - 0.5);
							}
						}
					}
				}
				else if (num7 > 0.0)
				{
					for (double num11 = num7; num11 < num; num11 += num7)
					{
						double num12 = num11 * num3 + point.Y;
						dc.DrawLine(pen, new Point(point.X, num12), new Point(point.X + num6, num12));
						if (snapsToDevicePixels)
						{
							doubleCollection2.Add(num12 - 0.5);
						}
					}
				}
				if (this.IsSelectionRangeEnabled)
				{
					double num13 = (this.SelectionStart - this.Minimum) * num3 + point.Y;
					Point point4 = new Point(point.X, num13);
					Point start = new Point(point.X + num6, num13);
					Point point5 = new Point(point.X + num6, num13 + Math.Abs(num6) * num4);
					PathSegment[] segments = new PathSegment[]
					{
						new LineSegment(point5, true),
						new LineSegment(point4, true)
					};
					PathGeometry geometry = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start, segments, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry);
					num13 = (this.SelectionEnd - this.Minimum) * num3 + point.Y;
					point4 = new Point(point.X, num13);
					start = new Point(point.X + num6, num13);
					point5 = new Point(point.X + num6, num13 - Math.Abs(num6) * num4);
					segments = new PathSegment[]
					{
						new LineSegment(point5, true),
						new LineSegment(point4, true)
					};
					geometry = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start, segments, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry);
				}
			}
			else
			{
				double num14 = this.TickFrequency;
				if (num14 > 0.0)
				{
					double num15 = (this.Maximum - this.Minimum) / size.Width;
					if (num14 < num15)
					{
						num14 = num15;
					}
				}
				dc.DrawLine(pen, point, new Point(point.X, point.Y + num2));
				dc.DrawLine(pen, new Point(point2.X, point.Y), new Point(point2.X, point.Y + num2));
				if (snapsToDevicePixels)
				{
					doubleCollection.Add(point.X - 0.5);
					doubleCollection2.Add(point.Y);
					doubleCollection.Add(point.X - 0.5);
					doubleCollection2.Add(point2.Y + num2);
					doubleCollection2.Add(point2.Y + num6);
				}
				DoubleCollection doubleCollection4 = null;
				bool flag2;
				if (base.GetValueSource(TickBar.TicksProperty, null, out flag2) != BaseValueSourceInternal.Default || flag2)
				{
					doubleCollection4 = this.Ticks;
				}
				if (doubleCollection4 != null && doubleCollection4.Count > 0)
				{
					for (int j = 0; j < doubleCollection4.Count; j++)
					{
						if (!DoubleUtil.LessThanOrClose(doubleCollection4[j], this.Minimum) && !DoubleUtil.GreaterThanOrClose(doubleCollection4[j], this.Maximum))
						{
							double num16 = doubleCollection4[j] - this.Minimum;
							double num17 = num16 * num3 + point.X;
							dc.DrawLine(pen, new Point(num17, point.Y), new Point(num17, point.Y + num6));
							if (snapsToDevicePixels)
							{
								doubleCollection.Add(num17 - 0.5);
							}
						}
					}
				}
				else if (num14 > 0.0)
				{
					for (double num18 = num14; num18 < num; num18 += num14)
					{
						double num19 = num18 * num3 + point.X;
						dc.DrawLine(pen, new Point(num19, point.Y), new Point(num19, point.Y + num6));
						if (snapsToDevicePixels)
						{
							doubleCollection.Add(num19 - 0.5);
						}
					}
				}
				if (this.IsSelectionRangeEnabled)
				{
					double num20 = (this.SelectionStart - this.Minimum) * num3 + point.X;
					Point point6 = new Point(num20, point.Y);
					Point start2 = new Point(num20, point.Y + num6);
					Point point7 = new Point(num20 + Math.Abs(num6) * num4, point.Y + num6);
					PathSegment[] segments2 = new PathSegment[]
					{
						new LineSegment(point7, true),
						new LineSegment(point6, true)
					};
					PathGeometry geometry2 = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start2, segments2, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry2);
					num20 = (this.SelectionEnd - this.Minimum) * num3 + point.X;
					point6 = new Point(num20, point.Y);
					start2 = new Point(num20, point.Y + num6);
					point7 = new Point(num20 - Math.Abs(num6) * num4, point.Y + num6);
					segments2 = new PathSegment[]
					{
						new LineSegment(point7, true),
						new LineSegment(point6, true)
					};
					geometry2 = new PathGeometry(new PathFigure[]
					{
						new PathFigure(start2, segments2, true)
					});
					dc.DrawGeometry(this.Fill, pen, geometry2);
				}
			}
			if (snapsToDevicePixels)
			{
				doubleCollection.Add(base.ActualWidth);
				doubleCollection2.Add(base.ActualHeight);
				base.VisualXSnappingGuidelines = doubleCollection;
				base.VisualYSnappingGuidelines = doubleCollection2;
			}
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x001B2EA0 File Offset: 0x001B10A0
		private void BindToTemplatedParent(DependencyProperty target, DependencyProperty source)
		{
			if (!base.HasNonDefaultValue(target))
			{
				base.SetBinding(target, new Binding
				{
					RelativeSource = RelativeSource.TemplatedParent,
					Path = new PropertyPath(source)
				});
			}
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x001B2EDC File Offset: 0x001B10DC
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			Slider slider = base.TemplatedParent as Slider;
			if (slider != null)
			{
				this.BindToTemplatedParent(TickBar.TicksProperty, Slider.TicksProperty);
				this.BindToTemplatedParent(TickBar.TickFrequencyProperty, Slider.TickFrequencyProperty);
				this.BindToTemplatedParent(TickBar.IsSelectionRangeEnabledProperty, Slider.IsSelectionRangeEnabledProperty);
				this.BindToTemplatedParent(TickBar.SelectionStartProperty, Slider.SelectionStartProperty);
				this.BindToTemplatedParent(TickBar.SelectionEndProperty, Slider.SelectionEndProperty);
				this.BindToTemplatedParent(TickBar.MinimumProperty, RangeBase.MinimumProperty);
				this.BindToTemplatedParent(TickBar.MaximumProperty, RangeBase.MaximumProperty);
				this.BindToTemplatedParent(TickBar.IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);
				if (!base.HasNonDefaultValue(TickBar.ReservedSpaceProperty) && slider.Track != null)
				{
					Binding binding = new Binding();
					binding.Source = slider.Track.Thumb;
					if (slider.Orientation == Orientation.Horizontal)
					{
						binding.Path = new PropertyPath(FrameworkElement.ActualWidthProperty);
					}
					else
					{
						binding.Path = new PropertyPath(FrameworkElement.ActualHeightProperty);
					}
					base.SetBinding(TickBar.ReservedSpaceProperty, binding);
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.Fill" /> dependency property. This property is read-only.</summary>
		// Token: 0x0400311F RID: 12575
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(TickBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, null, null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.Minimum" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.Minimum" /> dependency property.</returns>
		// Token: 0x04003120 RID: 12576
		public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.Maximum" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.Maximum" /> dependency property.</returns>
		// Token: 0x04003121 RID: 12577
		public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.SelectionStart" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.SelectionStart" /> dependency property.</returns>
		// Token: 0x04003122 RID: 12578
		public static readonly DependencyProperty SelectionStartProperty = Slider.SelectionStartProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(-1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.SelectionEnd" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.SelectionEnd" /> dependency property.</returns>
		// Token: 0x04003123 RID: 12579
		public static readonly DependencyProperty SelectionEndProperty = Slider.SelectionEndProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(-1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.IsSelectionRangeEnabled" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Slider.IsSelectionRangeEnabled" /> dependency property.</returns>
		// Token: 0x04003124 RID: 12580
		public static readonly DependencyProperty IsSelectionRangeEnabledProperty = Slider.IsSelectionRangeEnabledProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.TickFrequency" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.TickFrequency" /> dependency property.</returns>
		// Token: 0x04003125 RID: 12581
		public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.Ticks" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.Ticks" /> dependency property.</returns>
		// Token: 0x04003126 RID: 12582
		public static readonly DependencyProperty TicksProperty = Slider.TicksProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.IsDirectionReversed" /> dependency property. This property is read-only.</summary>
		// Token: 0x04003127 RID: 12583
		public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(TickBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.Placement" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.Placement" /> dependency property.</returns>
		// Token: 0x04003128 RID: 12584
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(TickBarPlacement), typeof(TickBar), new FrameworkPropertyMetadata(TickBarPlacement.Top, FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TickBar.IsValidTickBarPlacement));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.TickBar.ReservedSpace" /> dependency property. This property is read-only.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.TickBar.ReservedSpace" /> dependency property.</returns>
		// Token: 0x04003129 RID: 12585
		public static readonly DependencyProperty ReservedSpaceProperty = DependencyProperty.Register("ReservedSpace", typeof(double), typeof(TickBar), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
	}
}
