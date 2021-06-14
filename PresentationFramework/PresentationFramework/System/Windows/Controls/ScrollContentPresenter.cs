using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Displays the content of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control.</summary>
	// Token: 0x0200051A RID: 1306
	public sealed class ScrollContentPresenter : ContentPresenter, IScrollInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> class. </summary>
		// Token: 0x06005438 RID: 21560 RVA: 0x0017548C File Offset: 0x0017368C
		public ScrollContentPresenter()
		{
			this._adornerLayer = new AdornerLayer();
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content upward by one line.</summary>
		// Token: 0x06005439 RID: 21561 RVA: 0x0017549F File Offset: 0x0017369F
		public void LineUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content downward by one line.</summary>
		// Token: 0x0600543A RID: 21562 RVA: 0x001754BF File Offset: 0x001736BF
		public void LineDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content to the left by a predetermined amount.</summary>
		// Token: 0x0600543B RID: 21563 RVA: 0x001754DF File Offset: 0x001736DF
		public void LineLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content to the right by a predetermined amount.</summary>
		// Token: 0x0600543C RID: 21564 RVA: 0x001754FF File Offset: 0x001736FF
		public void LineRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 16.0);
			}
		}

		/// <summary>Scrolls up within content by one page.</summary>
		// Token: 0x0600543D RID: 21565 RVA: 0x0017551F File Offset: 0x0017371F
		public void PageUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
			}
		}

		/// <summary>Scrolls down within content by one page.</summary>
		// Token: 0x0600543E RID: 21566 RVA: 0x0017553C File Offset: 0x0017373C
		public void PageDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
			}
		}

		/// <summary>Scrolls left within content by one page.</summary>
		// Token: 0x0600543F RID: 21567 RVA: 0x00175559 File Offset: 0x00173759
		public void PageLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
			}
		}

		/// <summary>Scrolls right within content by one page.</summary>
		// Token: 0x06005440 RID: 21568 RVA: 0x00175576 File Offset: 0x00173776
		public void PageRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
			}
		}

		/// <summary>Scrolls up within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005441 RID: 21569 RVA: 0x00175593 File Offset: 0x00173793
		public void MouseWheelUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 48.0);
			}
		}

		/// <summary>Scrolls down within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005442 RID: 21570 RVA: 0x001755B3 File Offset: 0x001737B3
		public void MouseWheelDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 48.0);
			}
		}

		/// <summary>Scrolls left within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005443 RID: 21571 RVA: 0x001755D3 File Offset: 0x001737D3
		public void MouseWheelLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 48.0);
			}
		}

		/// <summary>Scrolls right within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005444 RID: 21572 RVA: 0x001755F3 File Offset: 0x001737F3
		public void MouseWheelRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 48.0);
			}
		}

		/// <summary>Sets the amount of horizontal offset.</summary>
		/// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
		// Token: 0x06005445 RID: 21573 RVA: 0x00175614 File Offset: 0x00173814
		public void SetHorizontalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.X, num))
				{
					this._scrollData._offset.X = num;
					base.InvalidateArrange();
				}
			}
		}

		/// <summary>Sets the amount of vertical offset.</summary>
		/// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
		// Token: 0x06005446 RID: 21574 RVA: 0x00175664 File Offset: 0x00173864
		public void SetVerticalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.Y, num))
				{
					this._scrollData._offset.Y = num;
					base.InvalidateArrange();
				}
			}
		}

		/// <summary>Forces content to scroll until the coordinate space of a <see cref="T:System.Windows.Media.Visual" /> object is visible. </summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that becomes visible.</param>
		/// <param name="rectangle">The bounding rectangle that identifies the coordinate space to make visible.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that represents the visible region.</returns>
		// Token: 0x06005447 RID: 21575 RVA: 0x001756B4 File Offset: 0x001738B4
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return this.MakeVisible(visual, rectangle, true);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.AdornerLayer" /> on which adorners are rendered.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.AdornerLayer" /> on which adorners are rendered.</returns>
		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06005448 RID: 21576 RVA: 0x001756BF File Offset: 0x001738BF
		public AdornerLayer AdornerLayer
		{
			get
			{
				return this._adornerLayer;
			}
		}

		/// <summary>Indicates whether the content, if it supports <see cref="T:System.Windows.Controls.Primitives.IScrollInfo" />, should be allowed to control scrolling.   </summary>
		/// <returns>
		///     <see langword="true" /> if the content is allowed to scroll; otherwise, <see langword="false" />. A <see langword="false" /> value indicates that the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> acts as the scrolling client. This property has no default value.</returns>
		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06005449 RID: 21577 RVA: 0x001756C7 File Offset: 0x001738C7
		// (set) Token: 0x0600544A RID: 21578 RVA: 0x001756D9 File Offset: 0x001738D9
		public bool CanContentScroll
		{
			get
			{
				return (bool)base.GetValue(ScrollContentPresenter.CanContentScrollProperty);
			}
			set
			{
				base.SetValue(ScrollContentPresenter.CanContentScrollProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.</summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x0600544B RID: 21579 RVA: 0x001756E7 File Offset: 0x001738E7
		// (set) Token: 0x0600544C RID: 21580 RVA: 0x001756FE File Offset: 0x001738FE
		public bool CanHorizontallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll != value)
				{
					this._scrollData._canHorizontallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether scrolling on the vertical axis is possible.</summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x0600544D RID: 21581 RVA: 0x00175728 File Offset: 0x00173928
		// (set) Token: 0x0600544E RID: 21582 RVA: 0x0017573F File Offset: 0x0017393F
		public bool CanVerticallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll != value)
				{
					this._scrollData._canVerticallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets the horizontal size of the extent.</summary>
		/// <returns>The horizontal size of the extent. This property has no default value.</returns>
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x0600544F RID: 21583 RVA: 0x00175769 File Offset: 0x00173969
		public double ExtentWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Width;
			}
		}

		/// <summary>Gets the vertical size of the extent.</summary>
		/// <returns>The vertical size of the extent. This property has no default value.</returns>
		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06005450 RID: 21584 RVA: 0x0017578D File Offset: 0x0017398D
		public double ExtentHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Height;
			}
		}

		/// <summary>Gets the horizontal size of the viewport for this content.</summary>
		/// <returns>The horizontal size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06005451 RID: 21585 RVA: 0x001757B1 File Offset: 0x001739B1
		public double ViewportWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Width;
			}
		}

		/// <summary>Gets the vertical size of the viewport for this content.</summary>
		/// <returns>The vertical size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06005452 RID: 21586 RVA: 0x001757D5 File Offset: 0x001739D5
		public double ViewportHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Height;
			}
		}

		/// <summary>Gets the horizontal offset of the scrolled content.</summary>
		/// <returns>The horizontal offset. This property has no default value.</returns>
		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x06005453 RID: 21587 RVA: 0x001757F9 File Offset: 0x001739F9
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.X;
			}
		}

		/// <summary>Gets the vertical offset of the scrolled content.</summary>
		/// <returns>The vertical offset of the scrolled content. Valid values are between zero and the <see cref="P:System.Windows.Controls.ScrollContentPresenter.ExtentHeight" /> minus the <see cref="P:System.Windows.Controls.ScrollContentPresenter.ViewportHeight" />. This property has no default value.</returns>
		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x06005454 RID: 21588 RVA: 0x0017581D File Offset: 0x00173A1D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.Y;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior. This property has no default value.</returns>
		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x06005455 RID: 21589 RVA: 0x00175841 File Offset: 0x00173A41
		// (set) Token: 0x06005456 RID: 21590 RVA: 0x00175858 File Offset: 0x00173A58
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return null;
				}
				return this._scrollData._scrollOwner;
			}
			set
			{
				if (this.IsScrollClient)
				{
					this._scrollData._scrollOwner = value;
				}
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x06005457 RID: 21591 RVA: 0x0017586E File Offset: 0x00173A6E
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.TemplateChild != null)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x0017587C File Offset: 0x00173A7C
		protected override Visual GetVisualChild(int index)
		{
			if (base.TemplateChild == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.TemplateChild;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._adornerLayer;
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x06005459 RID: 21593 RVA: 0x001758DD File Offset: 0x00173ADD
		// (set) Token: 0x0600545A RID: 21594 RVA: 0x001758E8 File Offset: 0x00173AE8
		internal override UIElement TemplateChild
		{
			get
			{
				return base.TemplateChild;
			}
			set
			{
				UIElement templateChild = base.TemplateChild;
				if (value != templateChild)
				{
					if (templateChild != null && value == null)
					{
						base.RemoveVisualChild(this._adornerLayer);
					}
					base.TemplateChild = value;
					if (templateChild == null && value != null)
					{
						base.AddVisualChild(this._adornerLayer);
					}
				}
			}
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x0017592C File Offset: 0x00173B2C
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
			}
			try
			{
				int visualChildrenCount = this.VisualChildrenCount;
				if (visualChildrenCount > 0)
				{
					this._adornerLayer.Measure(constraint);
					if (!this.IsScrollClient)
					{
						size = base.MeasureOverride(constraint);
					}
					else
					{
						Size constraint2 = constraint;
						if (this._scrollData._canHorizontallyScroll)
						{
							constraint2.Width = double.PositiveInfinity;
						}
						if (this._scrollData._canVerticallyScroll)
						{
							constraint2.Height = double.PositiveInfinity;
						}
						size = base.MeasureOverride(constraint2);
					}
				}
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(constraint, size);
				}
				size.Width = Math.Min(constraint.Width, size.Width);
				size.Height = Math.Min(constraint.Height, size.Height);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
				}
			}
			return size;
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x00175A48 File Offset: 0x00173C48
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
			}
			try
			{
				int visualChildrenCount = this.VisualChildrenCount;
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(arrangeSize, this._scrollData._extent);
				}
				if (visualChildrenCount > 0)
				{
					this._adornerLayer.Arrange(new Rect(arrangeSize));
					UIElement uielement = this.GetVisualChild(0) as UIElement;
					if (uielement != null)
					{
						Rect finalRect = new Rect(uielement.DesiredSize);
						if (this.IsScrollClient)
						{
							finalRect.X = -this.HorizontalOffset;
							finalRect.Y = -this.VerticalOffset;
						}
						finalRect.Width = Math.Max(finalRect.Width, arrangeSize.Width);
						finalRect.Height = Math.Max(finalRect.Height, arrangeSize.Height);
						uielement.Arrange(finalRect);
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x00175B60 File Offset: 0x00173D60
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return new RectangleGeometry(new Rect(base.RenderSize));
		}

		/// <summary>Invoked when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />, which is used to build the visual tree of the current template.</summary>
		// Token: 0x0600545E RID: 21598 RVA: 0x00175B72 File Offset: 0x00173D72
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.HookupScrollingComponents();
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x00175B80 File Offset: 0x00173D80
		internal Rect MakeVisible(Visual visual, Rect rectangle, bool throwOnError)
		{
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(this);
			rectangle = generalTransform.TransformBounds(rectangle);
			if (!this.IsScrollClient || (!throwOnError && rectangle.IsEmpty))
			{
				return rectangle;
			}
			Rect rect = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(num);
			this.SetVerticalOffset(num2);
			rect.X = num;
			rect.Y = num2;
			rectangle.Intersect(rect);
			if (throwOnError)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			else if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x00175CEC File Offset: 0x00173EEC
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = false;
			bool flag2 = false;
			return ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(topView, bottomView, topChild, bottomChild, ref flag, ref flag2);
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x00175D0C File Offset: 0x00173F0C
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild, ref bool alignTop, ref bool alignBottom)
		{
			bool flag = DoubleUtil.LessThan(topChild, topView) && DoubleUtil.LessThan(bottomChild, bottomView);
			bool flag2 = DoubleUtil.GreaterThan(bottomChild, bottomView) && DoubleUtil.GreaterThan(topChild, topView);
			bool flag3 = bottomChild - topChild > bottomView - topView;
			if (((flag && !flag3) || (flag2 && flag3)) | alignTop)
			{
				alignTop = true;
				return topChild;
			}
			if ((flag || flag2) | alignBottom)
			{
				alignBottom = true;
				return bottomChild - (bottomView - topView);
			}
			return topView;
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x00175D76 File Offset: 0x00173F76
		internal static double ValidateInputOffset(double offset, string parameterName)
		{
			if (DoubleUtil.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException(parameterName, SR.Get("ScrollViewer_CannotBeNaN", new object[]
				{
					parameterName
				}));
			}
			return Math.Max(0.0, offset);
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00175DAA File Offset: 0x00173FAA
		private ScrollContentPresenter.ScrollData EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new ScrollContentPresenter.ScrollData();
			}
			return this._scrollData;
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x00175DC8 File Offset: 0x00173FC8
		internal void HookupScrollingComponents()
		{
			ScrollViewer scrollViewer = base.TemplatedParent as ScrollViewer;
			if (scrollViewer != null)
			{
				IScrollInfo scrollInfo = null;
				if (this.CanContentScroll)
				{
					scrollInfo = (base.Content as IScrollInfo);
					if (scrollInfo == null)
					{
						Visual visual = base.Content as Visual;
						if (visual != null)
						{
							ItemsPresenter itemsPresenter = visual as ItemsPresenter;
							if (itemsPresenter == null)
							{
								FrameworkElement frameworkElement = scrollViewer.TemplatedParent as FrameworkElement;
								if (frameworkElement != null)
								{
									itemsPresenter = (frameworkElement.GetTemplateChild("ItemsPresenter") as ItemsPresenter);
								}
							}
							if (itemsPresenter != null)
							{
								itemsPresenter.ApplyTemplate();
								int childrenCount = VisualTreeHelper.GetChildrenCount(itemsPresenter);
								if (childrenCount > 0)
								{
									scrollInfo = (VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo);
								}
							}
						}
					}
				}
				if (scrollInfo == null)
				{
					scrollInfo = this;
					this.EnsureScrollData();
				}
				if (scrollInfo != this._scrollInfo && this._scrollInfo != null)
				{
					if (this.IsScrollClient)
					{
						this._scrollData = null;
					}
					else
					{
						this._scrollInfo.ScrollOwner = null;
					}
				}
				if (scrollInfo != null)
				{
					this._scrollInfo = scrollInfo;
					scrollInfo.ScrollOwner = scrollViewer;
					scrollViewer.ScrollInfo = scrollInfo;
					return;
				}
			}
			else if (this._scrollInfo != null)
			{
				if (this._scrollInfo.ScrollOwner != null)
				{
					this._scrollInfo.ScrollOwner.ScrollInfo = null;
				}
				this._scrollInfo.ScrollOwner = null;
				this._scrollInfo = null;
				this._scrollData = null;
			}
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x00175EF4 File Offset: 0x001740F4
		private void VerifyScrollData(Size viewport, Size extent)
		{
			bool flag = true;
			if (double.IsInfinity(viewport.Width))
			{
				viewport.Width = extent.Width;
			}
			if (double.IsInfinity(viewport.Height))
			{
				viewport.Height = extent.Height;
			}
			flag &= DoubleUtil.AreClose(viewport, this._scrollData._viewport);
			flag &= DoubleUtil.AreClose(extent, this._scrollData._extent);
			this._scrollData._viewport = viewport;
			this._scrollData._extent = extent;
			if (!(flag & this.CoerceOffsets()))
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00175F92 File Offset: 0x00174192
		internal static double CoerceOffset(double offset, double extent, double viewport)
		{
			if (offset > extent - viewport)
			{
				offset = extent - viewport;
			}
			if (offset < 0.0)
			{
				offset = 0.0;
			}
			return offset;
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x00175FB8 File Offset: 0x001741B8
		private bool CoerceOffsets()
		{
			Vector vector = new Vector(ScrollContentPresenter.CoerceOffset(this._scrollData._offset.X, this._scrollData._extent.Width, this._scrollData._viewport.Width), ScrollContentPresenter.CoerceOffset(this._scrollData._offset.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height));
			bool result = DoubleUtil.AreClose(this._scrollData._computedOffset, vector);
			this._scrollData._computedOffset = vector;
			return result;
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x00176058 File Offset: 0x00174258
		private static void OnCanContentScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollContentPresenter scrollContentPresenter = (ScrollContentPresenter)d;
			if (scrollContentPresenter._scrollInfo == null)
			{
				return;
			}
			scrollContentPresenter.HookupScrollingComponents();
			scrollContentPresenter.InvalidateMeasure();
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06005469 RID: 21609 RVA: 0x00176081 File Offset: 0x00174281
		private bool IsScrollClient
		{
			get
			{
				return this._scrollInfo == this;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x0600546A RID: 21610 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollContentPresenter.CanContentScroll" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollContentPresenter.CanContentScroll" /> dependency property.</returns>
		// Token: 0x04002D8E RID: 11662
		public static readonly DependencyProperty CanContentScrollProperty = ScrollViewer.CanContentScrollProperty.AddOwner(typeof(ScrollContentPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollContentPresenter.OnCanContentScrollChanged)));

		// Token: 0x04002D8F RID: 11663
		private IScrollInfo _scrollInfo;

		// Token: 0x04002D90 RID: 11664
		private ScrollContentPresenter.ScrollData _scrollData;

		// Token: 0x04002D91 RID: 11665
		private readonly AdornerLayer _adornerLayer;

		// Token: 0x020009B0 RID: 2480
		private class ScrollData
		{
			// Token: 0x0400451C RID: 17692
			internal ScrollViewer _scrollOwner;

			// Token: 0x0400451D RID: 17693
			internal bool _canHorizontallyScroll;

			// Token: 0x0400451E RID: 17694
			internal bool _canVerticallyScroll;

			// Token: 0x0400451F RID: 17695
			internal Vector _offset;

			// Token: 0x04004520 RID: 17696
			internal Vector _computedOffset;

			// Token: 0x04004521 RID: 17697
			internal Size _viewport;

			// Token: 0x04004522 RID: 17698
			internal Size _extent;
		}
	}
}
