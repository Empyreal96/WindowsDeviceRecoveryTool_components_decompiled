using System;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control primitive that handles the positioning and sizing of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control and two <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> controls that are used to set a <see cref="P:System.Windows.Controls.Primitives.Track.Value" />. </summary>
	// Token: 0x020005B4 RID: 1460
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Track : FrameworkElement
	{
		// Token: 0x0600611C RID: 24860 RVA: 0x001B420C File Offset: 0x001B240C
		static Track()
		{
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Track), new UIPropertyMetadata(new PropertyChangedCallback(Track.OnIsEnabledChanged)));
		}

		/// <summary>Calculates the distance from the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> to a specified point along the <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
		/// <param name="pt">The specified point.</param>
		/// <returns>The distance between the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> and the specified <paramref name="pt" /> value.</returns>
		// Token: 0x0600611E RID: 24862 RVA: 0x001B439C File Offset: 0x001B259C
		public virtual double ValueFromPoint(Point pt)
		{
			double val;
			if (this.Orientation == Orientation.Horizontal)
			{
				val = this.Value + this.ValueFromDistance(pt.X - this.ThumbCenterOffset, pt.Y - base.RenderSize.Height * 0.5);
			}
			else
			{
				val = this.Value + this.ValueFromDistance(pt.X - base.RenderSize.Width * 0.5, pt.Y - this.ThumbCenterOffset);
			}
			return Math.Max(this.Minimum, Math.Min(this.Maximum, val));
		}

		/// <summary>Calculates the change in the <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" /> when the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> moves.</summary>
		/// <param name="horizontal">The horizontal displacement of the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" />.</param>
		/// <param name="vertical">The vertical displacement of the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" />.</param>
		/// <returns>The change in <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> that corresponds to the displacement of the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" />.</returns>
		// Token: 0x0600611F RID: 24863 RVA: 0x001B4444 File Offset: 0x001B2644
		public virtual double ValueFromDistance(double horizontal, double vertical)
		{
			double num = (double)(this.IsDirectionReversed ? -1 : 1);
			if (this.Orientation == Orientation.Horizontal)
			{
				return num * horizontal * this.Density;
			}
			return -1.0 * num * vertical * this.Density;
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x001B4488 File Offset: 0x001B2688
		private void UpdateComponent(Control oldValue, Control newValue)
		{
			if (oldValue != newValue)
			{
				if (this._visualChildren == null)
				{
					this._visualChildren = new Visual[3];
				}
				if (oldValue != null)
				{
					base.RemoveVisualChild(oldValue);
				}
				int i = 0;
				while (i < 3 && this._visualChildren[i] != null)
				{
					if (this._visualChildren[i] == oldValue)
					{
						while (i < 2)
						{
							if (this._visualChildren[i + 1] == null)
							{
								break;
							}
							this._visualChildren[i] = this._visualChildren[i + 1];
							i++;
						}
					}
					else
					{
						i++;
					}
				}
				this._visualChildren[i] = newValue;
				base.AddVisualChild(newValue);
				base.InvalidateMeasure();
				base.InvalidateArrange();
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that decreases the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property of the <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that reduces the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" /> control when the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> is pressed. The default is a <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> control that has default settings.</returns>
		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06006121 RID: 24865 RVA: 0x001B4521 File Offset: 0x001B2721
		// (set) Token: 0x06006122 RID: 24866 RVA: 0x001B4529 File Offset: 0x001B2729
		public RepeatButton DecreaseRepeatButton
		{
			get
			{
				return this._decreaseButton;
			}
			set
			{
				if (this._increaseButton == value)
				{
					throw new NotSupportedException(SR.Get("Track_SameButtons"));
				}
				this.UpdateComponent(this._decreaseButton, value);
				this._decreaseButton = value;
				if (this._decreaseButton != null)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control that is used to change the <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> of a <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control that is used with the <see cref="T:System.Windows.Controls.Primitives.Track" />. The default is a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control that has default settings.</returns>
		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06006123 RID: 24867 RVA: 0x001B4565 File Offset: 0x001B2765
		// (set) Token: 0x06006124 RID: 24868 RVA: 0x001B456D File Offset: 0x001B276D
		public Thumb Thumb
		{
			get
			{
				return this._thumb;
			}
			set
			{
				this.UpdateComponent(this._thumb, value);
				this._thumb = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that increases the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property of the <see cref="T:System.Windows.Controls.Primitives.Track" /> class.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> that increases the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" /> control when the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> is pressed. The default is a <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> control that has default settings.</returns>
		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06006125 RID: 24869 RVA: 0x001B4583 File Offset: 0x001B2783
		// (set) Token: 0x06006126 RID: 24870 RVA: 0x001B458B File Offset: 0x001B278B
		public RepeatButton IncreaseRepeatButton
		{
			get
			{
				return this._increaseButton;
			}
			set
			{
				if (this._decreaseButton == value)
				{
					throw new NotSupportedException(SR.Get("Track_SameButtons"));
				}
				this.UpdateComponent(this._increaseButton, value);
				this._increaseButton = value;
				if (this._increaseButton != null)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.Track" /> is displayed horizontally or vertically.  </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> enumeration value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.Track" /> is displayed horizontally or vertically. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06006127 RID: 24871 RVA: 0x001B45C7 File Offset: 0x001B27C7
		// (set) Token: 0x06006128 RID: 24872 RVA: 0x001B45D9 File Offset: 0x001B27D9
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(Track.OrientationProperty);
			}
			set
			{
				base.SetValue(Track.OrientationProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum possible <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" />.  </summary>
		/// <returns>The smallest allowable <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> for the <see cref="T:System.Windows.Controls.Primitives.Track" />. The default is 0.</returns>
		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06006129 RID: 24873 RVA: 0x001B45EC File Offset: 0x001B27EC
		// (set) Token: 0x0600612A RID: 24874 RVA: 0x001B45FE File Offset: 0x001B27FE
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(Track.MinimumProperty);
			}
			set
			{
				base.SetValue(Track.MinimumProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum possible <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.Track" />.  </summary>
		/// <returns>The largest allowable <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> for the <see cref="T:System.Windows.Controls.Primitives.Track" />. The default is 1.</returns>
		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x0600612B RID: 24875 RVA: 0x001B4611 File Offset: 0x001B2811
		// (set) Token: 0x0600612C RID: 24876 RVA: 0x001B4623 File Offset: 0x001B2823
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(Track.MaximumProperty);
			}
			set
			{
				base.SetValue(Track.MaximumProperty, value);
			}
		}

		/// <summary>Gets or sets the current value of the <see cref="T:System.Windows.Controls.Primitives.Track" /> as determined by the position of the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> control.  </summary>
		/// <returns>The current value of the <see cref="T:System.Windows.Controls.Primitives.Track" />. The default is 0.</returns>
		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600612D RID: 24877 RVA: 0x001B4636 File Offset: 0x001B2836
		// (set) Token: 0x0600612E RID: 24878 RVA: 0x001B4648 File Offset: 0x001B2848
		public double Value
		{
			get
			{
				return (double)base.GetValue(Track.ValueProperty);
			}
			set
			{
				base.SetValue(Track.ValueProperty, value);
			}
		}

		/// <summary>Gets or sets the size of the part of the scrollable content that is visible.  </summary>
		/// <returns>The size of the visible area of the scrollable content. The default is <see cref="F:System.Double.NaN" />, which means that the content size is not defined.</returns>
		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600612F RID: 24879 RVA: 0x001B465B File Offset: 0x001B285B
		// (set) Token: 0x06006130 RID: 24880 RVA: 0x001B466D File Offset: 0x001B286D
		public double ViewportSize
		{
			get
			{
				return (double)base.GetValue(Track.ViewportSizeProperty);
			}
			set
			{
				base.SetValue(Track.ViewportSizeProperty, value);
			}
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x001B4680 File Offset: 0x001B2880
		private static bool IsValidViewport(object o)
		{
			double num = (double)o;
			return num >= 0.0 || double.IsNaN(num);
		}

		/// <summary>Gets or sets whether the direction of increasing <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> is reversed from the default direction.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.Primitives.Track.IncreaseRepeatButton" /> and the <see cref="P:System.Windows.Controls.Primitives.Track.DecreaseRepeatButton" /> exchanged positions and the direction of increasing value is reversed; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06006132 RID: 24882 RVA: 0x001B46A8 File Offset: 0x001B28A8
		// (set) Token: 0x06006133 RID: 24883 RVA: 0x001B46BA File Offset: 0x001B28BA
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(Track.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(Track.IsDirectionReversedProperty, value);
			}
		}

		/// <summary>Gets the child of the <see cref="T:System.Windows.Controls.Primitives.Track" /> at the specified index.</summary>
		/// <param name="index">The index of the child.</param>
		/// <returns>Returns the object of the <see cref="T:System.Windows.Controls.Primitives.Track" /> at the specified index in the list of visual child elements. The index must be a number between zero (0) and the value of the <see cref="P:System.Windows.Controls.Primitives.Track.VisualChildrenCount" /> property minus one (1).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified index is greater than the value of the <see cref="P:System.Windows.Controls.Primitives.Track.VisualChildrenCount" /> property minus one (1).</exception>
		// Token: 0x06006134 RID: 24884 RVA: 0x001B46C8 File Offset: 0x001B28C8
		protected override Visual GetVisualChild(int index)
		{
			if (this._visualChildren == null || this._visualChildren[index] == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._visualChildren[index];
		}

		/// <summary>Gets the number of child elements of a <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
		/// <returns>An integer between 0 and 2 that specifies the number of child elements in the <see cref="T:System.Windows.Controls.Primitives.Track" />.</returns>
		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06006135 RID: 24885 RVA: 0x001B46FF File Offset: 0x001B28FF
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._visualChildren == null || this._visualChildren[0] == null)
				{
					return 0;
				}
				if (this._visualChildren[1] == null)
				{
					return 1;
				}
				if (this._visualChildren[2] != null)
				{
					return 3;
				}
				return 2;
			}
		}

		/// <summary>Measures the layout size that is required for the <see cref="T:System.Windows.Controls.Primitives.Track" /> and its components.</summary>
		/// <param name="availableSize">The size of the available space for the track.</param>
		/// <returns>The layout <see cref="T:System.Windows.Size" /> that is required by the <see cref="T:System.Windows.Controls.Primitives.Track" />.</returns>
		// Token: 0x06006136 RID: 24886 RVA: 0x001B4730 File Offset: 0x001B2930
		protected override Size MeasureOverride(Size availableSize)
		{
			Size desiredSize = new Size(0.0, 0.0);
			if (this.Thumb != null)
			{
				this.Thumb.Measure(availableSize);
				desiredSize = this.Thumb.DesiredSize;
			}
			if (!double.IsNaN(this.ViewportSize))
			{
				if (this.Orientation == Orientation.Vertical)
				{
					desiredSize.Height = 0.0;
				}
				else
				{
					desiredSize.Width = 0.0;
				}
			}
			return desiredSize;
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x001B47AF File Offset: 0x001B29AF
		private static void CoerceLength(ref double componentLength, double trackLength)
		{
			if (componentLength < 0.0)
			{
				componentLength = 0.0;
				return;
			}
			if (componentLength > trackLength || double.IsNaN(componentLength))
			{
				componentLength = trackLength;
			}
		}

		/// <summary>Creates the layout for the <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
		/// <param name="arrangeSize">The area that is provided for the <see cref="T:System.Windows.Controls.Primitives.Track" />.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> to use for the <see cref="T:System.Windows.Controls.Primitives.Track" /> content.</returns>
		// Token: 0x06006138 RID: 24888 RVA: 0x001B47DC File Offset: 0x001B29DC
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.Orientation == Orientation.Vertical;
			double num = Math.Max(0.0, this.ViewportSize);
			double num2;
			double num3;
			double num4;
			if (double.IsNaN(num))
			{
				this.ComputeSliderLengths(arrangeSize, flag, out num2, out num3, out num4);
			}
			else if (!this.ComputeScrollBarLengths(arrangeSize, num, flag, out num2, out num3, out num4))
			{
				return arrangeSize;
			}
			Point location = default(Point);
			Size size = arrangeSize;
			bool isDirectionReversed = this.IsDirectionReversed;
			if (flag)
			{
				Track.CoerceLength(ref num2, arrangeSize.Height);
				Track.CoerceLength(ref num4, arrangeSize.Height);
				Track.CoerceLength(ref num3, arrangeSize.Height);
				location.Y = (isDirectionReversed ? (num2 + num3) : 0.0);
				size.Height = num4;
				if (this.IncreaseRepeatButton != null)
				{
					this.IncreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.Y = (isDirectionReversed ? 0.0 : (num4 + num3));
				size.Height = num2;
				if (this.DecreaseRepeatButton != null)
				{
					this.DecreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.Y = (isDirectionReversed ? num2 : num4);
				size.Height = num3;
				if (this.Thumb != null)
				{
					this.Thumb.Arrange(new Rect(location, size));
				}
				this.ThumbCenterOffset = location.Y + num3 * 0.5;
			}
			else
			{
				Track.CoerceLength(ref num2, arrangeSize.Width);
				Track.CoerceLength(ref num4, arrangeSize.Width);
				Track.CoerceLength(ref num3, arrangeSize.Width);
				location.X = (isDirectionReversed ? (num4 + num3) : 0.0);
				size.Width = num2;
				if (this.DecreaseRepeatButton != null)
				{
					this.DecreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.X = (isDirectionReversed ? 0.0 : (num2 + num3));
				size.Width = num4;
				if (this.IncreaseRepeatButton != null)
				{
					this.IncreaseRepeatButton.Arrange(new Rect(location, size));
				}
				location.X = (isDirectionReversed ? num4 : num2);
				size.Width = num3;
				if (this.Thumb != null)
				{
					this.Thumb.Arrange(new Rect(location, size));
				}
				this.ThumbCenterOffset = location.X + num3 * 0.5;
			}
			return arrangeSize;
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x001B4A38 File Offset: 0x001B2C38
		private void ComputeSliderLengths(Size arrangeSize, bool isVertical, out double decreaseButtonLength, out double thumbLength, out double increaseButtonLength)
		{
			double minimum = this.Minimum;
			double num = Math.Max(0.0, this.Maximum - minimum);
			double num2 = Math.Min(num, this.Value - minimum);
			double num3;
			if (isVertical)
			{
				num3 = arrangeSize.Height;
				thumbLength = ((this.Thumb == null) ? 0.0 : this.Thumb.DesiredSize.Height);
			}
			else
			{
				num3 = arrangeSize.Width;
				thumbLength = ((this.Thumb == null) ? 0.0 : this.Thumb.DesiredSize.Width);
			}
			Track.CoerceLength(ref thumbLength, num3);
			double num4 = num3 - thumbLength;
			decreaseButtonLength = num4 * num2 / num;
			Track.CoerceLength(ref decreaseButtonLength, num4);
			increaseButtonLength = num4 - decreaseButtonLength;
			Track.CoerceLength(ref increaseButtonLength, num4);
			this.Density = num / num4;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x001B4B18 File Offset: 0x001B2D18
		private bool ComputeScrollBarLengths(Size arrangeSize, double viewportSize, bool isVertical, out double decreaseButtonLength, out double thumbLength, out double increaseButtonLength)
		{
			double minimum = this.Minimum;
			double num = Math.Max(0.0, this.Maximum - minimum);
			double num2 = Math.Min(num, this.Value - minimum);
			double num3 = Math.Max(0.0, num) + viewportSize;
			double num4;
			double val;
			if (isVertical)
			{
				num4 = arrangeSize.Height;
				object obj = base.TryFindResource(SystemParameters.VerticalScrollBarButtonHeightKey);
				double num5 = (obj is double) ? ((double)obj) : SystemParameters.VerticalScrollBarButtonHeight;
				val = Math.Floor(num5 * 0.5);
			}
			else
			{
				num4 = arrangeSize.Width;
				object obj2 = base.TryFindResource(SystemParameters.HorizontalScrollBarButtonWidthKey);
				double num6 = (obj2 is double) ? ((double)obj2) : SystemParameters.HorizontalScrollBarButtonWidth;
				val = Math.Floor(num6 * 0.5);
			}
			thumbLength = num4 * viewportSize / num3;
			Track.CoerceLength(ref thumbLength, num4);
			thumbLength = Math.Max(val, thumbLength);
			bool flag = DoubleUtil.LessThanOrClose(num, 0.0);
			bool flag2 = thumbLength > num4;
			if (flag || flag2)
			{
				if (base.Visibility != Visibility.Hidden)
				{
					base.Visibility = Visibility.Hidden;
				}
				this.ThumbCenterOffset = double.NaN;
				this.Density = double.NaN;
				decreaseButtonLength = 0.0;
				increaseButtonLength = 0.0;
				return false;
			}
			if (base.Visibility != Visibility.Visible)
			{
				base.Visibility = Visibility.Visible;
			}
			double num7 = num4 - thumbLength;
			decreaseButtonLength = num7 * num2 / num;
			Track.CoerceLength(ref decreaseButtonLength, num7);
			increaseButtonLength = num7 - decreaseButtonLength;
			Track.CoerceLength(ref increaseButtonLength, num7);
			this.Density = num / num7;
			return true;
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x001B4CBC File Offset: 0x001B2EBC
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

		// Token: 0x0600613C RID: 24892 RVA: 0x001B4CF8 File Offset: 0x001B2EF8
		private void BindChildToTemplatedParent(FrameworkElement element, DependencyProperty target, DependencyProperty source)
		{
			if (element != null && !element.HasNonDefaultValue(target))
			{
				element.SetBinding(target, new Binding
				{
					Source = base.TemplatedParent,
					Path = new PropertyPath(source)
				});
			}
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x001B4D38 File Offset: 0x001B2F38
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			RangeBase rangeBase = base.TemplatedParent as RangeBase;
			if (rangeBase != null)
			{
				this.BindToTemplatedParent(Track.MinimumProperty, RangeBase.MinimumProperty);
				this.BindToTemplatedParent(Track.MaximumProperty, RangeBase.MaximumProperty);
				this.BindToTemplatedParent(Track.ValueProperty, RangeBase.ValueProperty);
				ScrollBar scrollBar = rangeBase as ScrollBar;
				if (scrollBar != null)
				{
					this.BindToTemplatedParent(Track.ViewportSizeProperty, ScrollBar.ViewportSizeProperty);
					this.BindToTemplatedParent(Track.OrientationProperty, ScrollBar.OrientationProperty);
					return;
				}
				Slider slider = rangeBase as Slider;
				if (slider != null)
				{
					this.BindToTemplatedParent(Track.OrientationProperty, Slider.OrientationProperty);
					this.BindToTemplatedParent(Track.IsDirectionReversedProperty, Slider.IsDirectionReversedProperty);
					this.BindChildToTemplatedParent(this.DecreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
					this.BindChildToTemplatedParent(this.DecreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
					this.BindChildToTemplatedParent(this.IncreaseRepeatButton, RepeatButton.DelayProperty, Slider.DelayProperty);
					this.BindChildToTemplatedParent(this.IncreaseRepeatButton, RepeatButton.IntervalProperty, Slider.IntervalProperty);
				}
			}
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x001B4E3A File Offset: 0x001B303A
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				Mouse.Synchronize();
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x0600613F RID: 24895 RVA: 0x001B4E4F File Offset: 0x001B304F
		// (set) Token: 0x06006140 RID: 24896 RVA: 0x001B4E57 File Offset: 0x001B3057
		private double ThumbCenterOffset
		{
			get
			{
				return this._thumbCenterOffset;
			}
			set
			{
				this._thumbCenterOffset = value;
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06006141 RID: 24897 RVA: 0x001B4E60 File Offset: 0x001B3060
		// (set) Token: 0x06006142 RID: 24898 RVA: 0x001B4E68 File Offset: 0x001B3068
		private double Density
		{
			get
			{
				return this._density;
			}
			set
			{
				this._density = value;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06006143 RID: 24899 RVA: 0x000962DF File Offset: 0x000944DF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.Orientation" /> dependency property. </summary>
		// Token: 0x0400313B RID: 12603
		[CommonDependencyProperty]
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Track), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.Minimum" /> dependency property. </summary>
		// Token: 0x0400313C RID: 12604
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.Maximum" /> dependency property. </summary>
		// Token: 0x0400313D RID: 12605
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.Value" /> dependency property. </summary>
		// Token: 0x0400313E RID: 12606
		[CommonDependencyProperty]
		public static readonly DependencyProperty ValueProperty = RangeBase.ValueProperty.AddOwner(typeof(Track), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.ViewportSize" /> dependency property. </summary>
		// Token: 0x0400313F RID: 12607
		[CommonDependencyProperty]
		public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(double), typeof(Track), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(Track.IsValidViewport));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Track.IsDirectionReversed" /> dependency property. </summary>
		// Token: 0x04003140 RID: 12608
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(Track), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003141 RID: 12609
		private RepeatButton _increaseButton;

		// Token: 0x04003142 RID: 12610
		private RepeatButton _decreaseButton;

		// Token: 0x04003143 RID: 12611
		private Thumb _thumb;

		// Token: 0x04003144 RID: 12612
		private Visual[] _visualChildren;

		// Token: 0x04003145 RID: 12613
		private double _density = double.NaN;

		// Token: 0x04003146 RID: 12614
		private double _thumbCenterOffset = double.NaN;
	}
}
