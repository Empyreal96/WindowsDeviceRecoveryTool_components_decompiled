using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Arranges child elements into a single line that can be oriented horizontally or vertically. </summary>
	// Token: 0x02000538 RID: 1336
	public class StackPanel : Panel, IScrollInfo, IStackMeasure
	{
		// Token: 0x0600567B RID: 22139 RVA: 0x0017ECF4 File Offset: 0x0017CEF4
		static StackPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.StackPanel);
		}

		/// <summary>Scrolls content by one logical unit upward.</summary>
		// Token: 0x0600567D RID: 22141 RVA: 0x0017ED53 File Offset: 0x0017CF53
		public void LineUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
		}

		/// <summary>Scrolls content downward by one logical unit.</summary>
		// Token: 0x0600567E RID: 22142 RVA: 0x0017ED7F File Offset: 0x0017CF7F
		public void LineDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
		}

		/// <summary>Scrolls content by one logical unit to the left.</summary>
		// Token: 0x0600567F RID: 22143 RVA: 0x0017EDAB File Offset: 0x0017CFAB
		public void LineLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		/// <summary>Scrolls content by one logical unit to the right.</summary>
		// Token: 0x06005680 RID: 22144 RVA: 0x0017EDD6 File Offset: 0x0017CFD6
		public void LineRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		/// <summary>Scrolls content logically upward by one page.</summary>
		// Token: 0x06005681 RID: 22145 RVA: 0x0017EE01 File Offset: 0x0017D001
		public void PageUp()
		{
			this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
		}

		/// <summary>Scrolls content logically downward by one page.</summary>
		// Token: 0x06005682 RID: 22146 RVA: 0x0017EE16 File Offset: 0x0017D016
		public void PageDown()
		{
			this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
		}

		/// <summary>Scrolls content logically to the left by one page.</summary>
		// Token: 0x06005683 RID: 22147 RVA: 0x0017EE2B File Offset: 0x0017D02B
		public void PageLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
		}

		/// <summary>Scrolls content logically to the right by one page.</summary>
		// Token: 0x06005684 RID: 22148 RVA: 0x0017EE40 File Offset: 0x0017D040
		public void PageRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
		}

		/// <summary>Scrolls content logically upward in response to a click of the mouse wheel button.</summary>
		// Token: 0x06005685 RID: 22149 RVA: 0x0017EE58 File Offset: 0x0017D058
		public void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffset(this.VerticalOffset - (double)SystemParameters.WheelScrollLines * ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
				return;
			}
			this.PageUp();
		}

		/// <summary>Scrolls content logically downward in response to a click of the mouse wheel button.</summary>
		// Token: 0x06005686 RID: 22150 RVA: 0x0017EEA8 File Offset: 0x0017D0A8
		public void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffset(this.VerticalOffset + (double)SystemParameters.WheelScrollLines * ((this.Orientation == Orientation.Vertical) ? 1.0 : 16.0));
				return;
			}
			this.PageDown();
		}

		/// <summary>Scrolls content logically to the left in response to a click of the mouse wheel button.</summary>
		// Token: 0x06005687 RID: 22151 RVA: 0x0017EEF5 File Offset: 0x0017D0F5
		public void MouseWheelLeft()
		{
			this.SetHorizontalOffset(this.HorizontalOffset - 3.0 * ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		/// <summary>Scrolls content logically to the right in response to a click of the mouse wheel button.</summary>
		// Token: 0x06005688 RID: 22152 RVA: 0x0017EF2A File Offset: 0x0017D12A
		public void MouseWheelRight()
		{
			this.SetHorizontalOffset(this.HorizontalOffset + 3.0 * ((this.Orientation == Orientation.Horizontal) ? 1.0 : 16.0));
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.StackPanel.HorizontalOffset" /> property.</summary>
		/// <param name="offset">The value of the <see cref="P:System.Windows.Controls.StackPanel.HorizontalOffset" /> property.</param>
		// Token: 0x06005689 RID: 22153 RVA: 0x0017EF60 File Offset: 0x0017D160
		public void SetHorizontalOffset(double offset)
		{
			this.EnsureScrollData();
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.X))
			{
				this._scrollData._offset.X = num;
				base.InvalidateMeasure();
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.StackPanel.VerticalOffset" /> property.</summary>
		/// <param name="offset">The value of the <see cref="P:System.Windows.Controls.StackPanel.VerticalOffset" /> property.</param>
		// Token: 0x0600568A RID: 22154 RVA: 0x0017EFB0 File Offset: 0x0017D1B0
		public void SetVerticalOffset(double offset)
		{
			this.EnsureScrollData();
			double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
			if (!DoubleUtil.AreClose(num, this._scrollData._offset.Y))
			{
				this._scrollData._offset.Y = num;
				base.InvalidateMeasure();
			}
		}

		/// <summary>Scrolls to the specified coordinates and makes that part of a <see cref="T:System.Windows.Media.Visual" /> visible. </summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that becomes visible.</param>
		/// <param name="rectangle">The <see cref="T:System.Windows.Rect" /> that represents coordinate space within a visual.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> in the coordinate space that is made visible.</returns>
		// Token: 0x0600568B RID: 22155 RVA: 0x0017F000 File Offset: 0x0017D200
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			Vector vector = default(Vector);
			Rect result = default(Rect);
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(this);
			rectangle = generalTransform.TransformBounds(rectangle);
			if (!this.IsScrolling)
			{
				return rectangle;
			}
			this.MakeVisiblePhysicalHelper(rectangle, ref vector, ref result);
			int childIndex = this.FindChildIndexThatParentsVisual(visual);
			this.MakeVisibleLogicalHelper(childIndex, ref vector, ref result);
			vector.X = ScrollContentPresenter.CoerceOffset(vector.X, this._scrollData._extent.Width, this._scrollData._viewport.Width);
			vector.Y = ScrollContentPresenter.CoerceOffset(vector.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height);
			if (!DoubleUtil.AreClose(vector, this._scrollData._offset))
			{
				this._scrollData._offset = vector;
				base.InvalidateMeasure();
				this.OnScrollChange();
			}
			return result;
		}

		/// <summary>Gets or sets a value that indicates the dimension by which child elements are stacked.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Orientation" /> of child content.</returns>
		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x0600568C RID: 22156 RVA: 0x0017F107 File Offset: 0x0017D307
		// (set) Token: 0x0600568D RID: 22157 RVA: 0x0017F119 File Offset: 0x0017D319
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(StackPanel.OrientationProperty);
			}
			set
			{
				base.SetValue(StackPanel.OrientationProperty, value);
			}
		}

		/// <summary>Gets a value that indicates if this <see cref="T:System.Windows.Controls.StackPanel" /> has vertical or horizontal orientation.</summary>
		/// <returns>This property always returns <see langword="true" />.</returns>
		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x0600568E RID: 22158 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HasLogicalOrientation
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value that represents the <see cref="T:System.Windows.Controls.Orientation" /> of the <see cref="T:System.Windows.Controls.StackPanel" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> value.</returns>
		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x0600568F RID: 22159 RVA: 0x0017F12C File Offset: 0x0017D32C
		protected internal override Orientation LogicalOrientation
		{
			get
			{
				return this.Orientation;
			}
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.StackPanel" /> can scroll in the horizontal dimension. </summary>
		/// <returns>
		///     <see langword="true" /> if content can scroll in the horizontal dimension; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x06005690 RID: 22160 RVA: 0x0017F134 File Offset: 0x0017D334
		// (set) Token: 0x06005691 RID: 22161 RVA: 0x0017F14B File Offset: 0x0017D34B
		[DefaultValue(false)]
		public bool CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowHorizontal;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowHorizontal != value)
				{
					this._scrollData._allowHorizontal = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether content can scroll in the vertical dimension. </summary>
		/// <returns>
		///     <see langword="true" /> if content can scroll in the vertical dimension; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x06005692 RID: 22162 RVA: 0x0017F173 File Offset: 0x0017D373
		// (set) Token: 0x06005693 RID: 22163 RVA: 0x0017F18A File Offset: 0x0017D38A
		[DefaultValue(false)]
		public bool CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData._allowVertical;
			}
			set
			{
				this.EnsureScrollData();
				if (this._scrollData._allowVertical != value)
				{
					this._scrollData._allowVertical = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the extent.</summary>
		/// <returns>
		///     <see cref="T:System.Double" /> that represents the horizontal size of the extent. The default value is 0.0.</returns>
		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x0017F1B2 File Offset: 0x0017D3B2
		public double ExtentWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Width;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the extent.</summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the vertical size of the extent. The default value is 0.0.</returns>
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x0017F1D6 File Offset: 0x0017D3D6
		public double ExtentHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._extent.Height;
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the content's viewport.</summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the vertical size of the content's viewport. The default value is 0.0.</returns>
		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x06005696 RID: 22166 RVA: 0x0017F1FA File Offset: 0x0017D3FA
		public double ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Width;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the content's viewport.</summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the vertical size of the content's viewport. The default value is 0.0.</returns>
		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x06005697 RID: 22167 RVA: 0x0017F21E File Offset: 0x0017D41E
		public double ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._viewport.Height;
			}
		}

		/// <summary>Gets a value that contains the horizontal offset of the scrolled content.</summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the horizontal offset of the scrolled content. The default value is 0.0.</returns>
		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x06005698 RID: 22168 RVA: 0x0017F242 File Offset: 0x0017D442
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.X;
			}
		}

		/// <summary>Gets a value that contains the vertical offset of the scrolled content.</summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the vertical offset of the scrolled content. The default value is 0.0.</returns>
		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x06005699 RID: 22169 RVA: 0x0017F266 File Offset: 0x0017D466
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData._computedOffset.Y;
			}
		}

		/// <summary>Gets or sets a value that identifies the container that controls scrolling behavior in this <see cref="T:System.Windows.Controls.StackPanel" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ScrollViewer" /> that owns scrolling for this <see cref="T:System.Windows.Controls.StackPanel" />. This property has no default value.</returns>
		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x0600569A RID: 22170 RVA: 0x0017F28A File Offset: 0x0017D48A
		// (set) Token: 0x0600569B RID: 22171 RVA: 0x0017F29D File Offset: 0x0017D49D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				this.EnsureScrollData();
				return this._scrollData._scrollOwner;
			}
			set
			{
				this.EnsureScrollData();
				if (value != this._scrollData._scrollOwner)
				{
					StackPanel.ResetScrolling(this);
					this._scrollData._scrollOwner = value;
				}
			}
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.StackPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.StackPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		// Token: 0x0600569C RID: 22172 RVA: 0x0017F2C8 File Offset: 0x0017D4C8
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:MeasureOverride");
			}
			try
			{
				result = StackPanel.StackMeasureHelper(this, this._scrollData, constraint);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:MeasureOverride");
				}
			}
			return result;
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x0017F340 File Offset: 0x0017D540
		internal static Size StackMeasureHelper(IStackMeasure measureElement, IStackMeasureScrollData scrollData, Size constraint)
		{
			Size size = default(Size);
			UIElementCollection internalChildren = measureElement.InternalChildren;
			Size availableSize = constraint;
			bool flag = measureElement.Orientation == Orientation.Horizontal;
			int num = -1;
			int i;
			double num2;
			if (flag)
			{
				availableSize.Width = double.PositiveInfinity;
				if (measureElement.IsScrolling && measureElement.CanVerticallyScroll)
				{
					availableSize.Height = double.PositiveInfinity;
				}
				i = (measureElement.IsScrolling ? StackPanel.CoerceOffsetToInteger(scrollData.Offset.X, internalChildren.Count) : 0);
				num2 = constraint.Width;
			}
			else
			{
				availableSize.Height = double.PositiveInfinity;
				if (measureElement.IsScrolling && measureElement.CanHorizontallyScroll)
				{
					availableSize.Width = double.PositiveInfinity;
				}
				i = (measureElement.IsScrolling ? StackPanel.CoerceOffsetToInteger(scrollData.Offset.Y, internalChildren.Count) : 0);
				num2 = constraint.Height;
			}
			int j = 0;
			int count = internalChildren.Count;
			while (j < count)
			{
				UIElement uielement = internalChildren[j];
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					Size desiredSize = uielement.DesiredSize;
					double num3;
					if (flag)
					{
						size.Width += desiredSize.Width;
						size.Height = Math.Max(size.Height, desiredSize.Height);
						num3 = desiredSize.Width;
					}
					else
					{
						size.Width = Math.Max(size.Width, desiredSize.Width);
						size.Height += desiredSize.Height;
						num3 = desiredSize.Height;
					}
					if (measureElement.IsScrolling && num == -1 && j >= i)
					{
						num2 -= num3;
						if (DoubleUtil.LessThanOrClose(num2, 0.0))
						{
							num = j;
						}
					}
				}
				j++;
			}
			if (measureElement.IsScrolling)
			{
				Size viewport = constraint;
				Size extent = size;
				Vector offset = scrollData.Offset;
				if (num == -1)
				{
					num = internalChildren.Count - 1;
				}
				while (i > 0)
				{
					double num4 = num2;
					if (flag)
					{
						num4 -= internalChildren[i - 1].DesiredSize.Width;
					}
					else
					{
						num4 -= internalChildren[i - 1].DesiredSize.Height;
					}
					if (DoubleUtil.LessThan(num4, 0.0))
					{
						break;
					}
					i--;
					num2 = num4;
				}
				int count2 = internalChildren.Count;
				int num5 = num - i;
				if (num5 == 0 || DoubleUtil.GreaterThanOrClose(num2, 0.0))
				{
					num5++;
				}
				if (flag)
				{
					scrollData.SetPhysicalViewport(viewport.Width);
					viewport.Width = (double)num5;
					extent.Width = (double)count2;
					offset.X = (double)i;
					offset.Y = Math.Max(0.0, Math.Min(offset.Y, extent.Height - viewport.Height));
				}
				else
				{
					scrollData.SetPhysicalViewport(viewport.Height);
					viewport.Height = (double)num5;
					extent.Height = (double)count2;
					offset.Y = (double)i;
					offset.X = Math.Max(0.0, Math.Min(offset.X, extent.Width - viewport.Width));
				}
				size.Width = Math.Min(size.Width, constraint.Width);
				size.Height = Math.Min(size.Height, constraint.Height);
				StackPanel.VerifyScrollingData(measureElement, scrollData, viewport, extent, offset);
			}
			return size;
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.StackPanel" /> element.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> that this element should use to arrange its child elements.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.StackPanel" /> element and its child elements.</returns>
		// Token: 0x0600569E RID: 22174 RVA: 0x0017F6CC File Offset: 0x0017D8CC
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrolling && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:ArrangeOverride");
			}
			try
			{
				StackPanel.StackArrangeHelper(this, this._scrollData, arrangeSize);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "STACK:ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x0017F73C File Offset: 0x0017D93C
		internal static Size StackArrangeHelper(IStackMeasure arrangeElement, IStackMeasureScrollData scrollData, Size arrangeSize)
		{
			UIElementCollection internalChildren = arrangeElement.InternalChildren;
			bool flag = arrangeElement.Orientation == Orientation.Horizontal;
			Rect finalRect = new Rect(arrangeSize);
			double num = 0.0;
			if (arrangeElement.IsScrolling)
			{
				if (flag)
				{
					finalRect.X = StackPanel.ComputePhysicalFromLogicalOffset(arrangeElement, scrollData.ComputedOffset.X, true);
					finalRect.Y = -1.0 * scrollData.ComputedOffset.Y;
				}
				else
				{
					finalRect.X = -1.0 * scrollData.ComputedOffset.X;
					finalRect.Y = StackPanel.ComputePhysicalFromLogicalOffset(arrangeElement, scrollData.ComputedOffset.Y, false);
				}
			}
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					if (flag)
					{
						finalRect.X += num;
						num = uielement.DesiredSize.Width;
						finalRect.Width = num;
						finalRect.Height = Math.Max(arrangeSize.Height, uielement.DesiredSize.Height);
					}
					else
					{
						finalRect.Y += num;
						num = uielement.DesiredSize.Height;
						finalRect.Height = num;
						finalRect.Width = Math.Max(arrangeSize.Width, uielement.DesiredSize.Width);
					}
					uielement.Arrange(finalRect);
				}
				i++;
			}
			return arrangeSize;
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0017F8C7 File Offset: 0x0017DAC7
		private void EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new StackPanel.ScrollData();
			}
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0017F8DC File Offset: 0x0017DADC
		private static void ResetScrolling(StackPanel element)
		{
			element.InvalidateMeasure();
			if (element.IsScrolling)
			{
				element._scrollData.ClearLayout();
			}
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x0017F8F7 File Offset: 0x0017DAF7
		private void OnScrollChange()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x0017F90C File Offset: 0x0017DB0C
		private static void VerifyScrollingData(IStackMeasure measureElement, IStackMeasureScrollData scrollData, Size viewport, Size extent, Vector offset)
		{
			bool flag = true;
			flag &= DoubleUtil.AreClose(viewport, scrollData.Viewport);
			flag &= DoubleUtil.AreClose(extent, scrollData.Extent);
			flag &= DoubleUtil.AreClose(offset, scrollData.ComputedOffset);
			scrollData.Offset = offset;
			if (!flag)
			{
				scrollData.Viewport = viewport;
				scrollData.Extent = extent;
				scrollData.ComputedOffset = offset;
				measureElement.OnScrollChange();
			}
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x0017F970 File Offset: 0x0017DB70
		private static double ComputePhysicalFromLogicalOffset(IStackMeasure arrangeElement, double logicalOffset, bool fHorizontal)
		{
			double num = 0.0;
			UIElementCollection internalChildren = arrangeElement.InternalChildren;
			int num2 = 0;
			while ((double)num2 < logicalOffset)
			{
				num -= (fHorizontal ? internalChildren[num2].DesiredSize.Width : internalChildren[num2].DesiredSize.Height);
				num2++;
			}
			return num;
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x0017F9CC File Offset: 0x0017DBCC
		private int FindChildIndexThatParentsVisual(Visual child)
		{
			DependencyObject dependencyObject = child;
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			while (parent != this)
			{
				dependencyObject = parent;
				parent = VisualTreeHelper.GetParent(dependencyObject);
				if (parent == null)
				{
					throw new ArgumentException(SR.Get("Stack_VisualInDifferentSubTree"), "child");
				}
			}
			UIElementCollection children = base.Children;
			return children.IndexOf((UIElement)dependencyObject);
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x0017FA1C File Offset: 0x0017DC1C
		private void MakeVisiblePhysicalHelper(Rect r, ref Vector newOffset, ref Rect newRect)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num;
			double num2;
			double num3;
			double num4;
			if (flag)
			{
				num = this._scrollData._computedOffset.Y;
				num2 = this.ViewportHeight;
				num3 = r.Y;
				num4 = r.Height;
			}
			else
			{
				num = this._scrollData._computedOffset.X;
				num2 = this.ViewportWidth;
				num3 = r.X;
				num4 = r.Width;
			}
			num3 += num;
			double num5 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(num, num + num2, num3, num3 + num4);
			double num6 = Math.Max(num3, num5);
			num4 = Math.Max(Math.Min(num4 + num3, num5 + num2) - num6, 0.0);
			num3 = num6;
			num3 -= num;
			if (flag)
			{
				newOffset.Y = num5;
				newRect.Y = num3;
				newRect.Height = num4;
				return;
			}
			newOffset.X = num5;
			newRect.X = num3;
			newRect.Width = num4;
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x0017FB00 File Offset: 0x0017DD00
		private void MakeVisibleLogicalHelper(int childIndex, ref Vector newOffset, ref Rect newRect)
		{
			bool flag = this.Orientation == Orientation.Horizontal;
			double num = 0.0;
			int num2;
			int num3;
			if (flag)
			{
				num2 = (int)this._scrollData._computedOffset.X;
				num3 = (int)this._scrollData._viewport.Width;
			}
			else
			{
				num2 = (int)this._scrollData._computedOffset.Y;
				num3 = (int)this._scrollData._viewport.Height;
			}
			int num4 = num2;
			if (childIndex < num2)
			{
				num4 = childIndex;
			}
			else if (childIndex > num2 + num3 - 1)
			{
				Size desiredSize = base.InternalChildren[childIndex].DesiredSize;
				double num5 = flag ? desiredSize.Width : desiredSize.Height;
				double num6 = this._scrollData._physicalViewport - num5;
				int num7 = childIndex;
				while (num7 > 0 && DoubleUtil.GreaterThanOrClose(num6, 0.0))
				{
					num7--;
					desiredSize = base.InternalChildren[num7].DesiredSize;
					num5 = (flag ? desiredSize.Width : desiredSize.Height);
					num += num5;
					num6 -= num5;
				}
				if (num7 != childIndex && DoubleUtil.LessThan(num6, 0.0))
				{
					num -= num5;
					num7++;
				}
				num4 = num7;
			}
			if (flag)
			{
				newOffset.X = (double)num4;
				newRect.X = num;
				newRect.Width = base.InternalChildren[childIndex].DesiredSize.Width;
				return;
			}
			newOffset.Y = (double)num4;
			newRect.Y = num;
			newRect.Height = base.InternalChildren[childIndex].DesiredSize.Height;
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x0017FCA4 File Offset: 0x0017DEA4
		private static int CoerceOffsetToInteger(double offset, int numberOfItems)
		{
			int num;
			if (double.IsNegativeInfinity(offset))
			{
				num = 0;
			}
			else if (double.IsPositiveInfinity(offset))
			{
				num = numberOfItems - 1;
			}
			else
			{
				num = (int)offset;
				num = Math.Max(Math.Min(numberOfItems - 1, num), 0);
			}
			return num;
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0017FCDF File Offset: 0x0017DEDF
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StackPanel.ResetScrolling(d as StackPanel);
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x0017FCEC File Offset: 0x0017DEEC
		private bool IsScrolling
		{
			get
			{
				return this._scrollData != null && this._scrollData._scrollOwner != null;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0009580F File Offset: 0x00093A0F
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x0017FD06 File Offset: 0x0017DF06
		bool IStackMeasure.IsScrolling
		{
			get
			{
				return this.IsScrolling;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x060056AD RID: 22189 RVA: 0x00171716 File Offset: 0x0016F916
		UIElementCollection IStackMeasure.InternalChildren
		{
			get
			{
				return base.InternalChildren;
			}
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x0017FD0E File Offset: 0x0017DF0E
		void IStackMeasure.OnScrollChange()
		{
			this.OnScrollChange();
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x0017FD16 File Offset: 0x0017DF16
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return SystemParameters.WheelScrollLines > 0;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StackPanel.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StackPanel.Orientation" /> dependency property.</returns>
		// Token: 0x04002E4D RID: 11853
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(StackPanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(StackPanel.OnOrientationChanged)), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		// Token: 0x04002E4E RID: 11854
		private StackPanel.ScrollData _scrollData;

		// Token: 0x020009BE RID: 2494
		private class ScrollData : IStackMeasureScrollData
		{
			// Token: 0x06008880 RID: 34944 RVA: 0x002524E0 File Offset: 0x002506E0
			internal void ClearLayout()
			{
				this._offset = default(Vector);
				this._viewport = (this._extent = default(Size));
				this._physicalViewport = 0.0;
			}

			// Token: 0x17001ECF RID: 7887
			// (get) Token: 0x06008881 RID: 34945 RVA: 0x00252520 File Offset: 0x00250720
			// (set) Token: 0x06008882 RID: 34946 RVA: 0x00252528 File Offset: 0x00250728
			public Vector Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001ED0 RID: 7888
			// (get) Token: 0x06008883 RID: 34947 RVA: 0x00252531 File Offset: 0x00250731
			// (set) Token: 0x06008884 RID: 34948 RVA: 0x00252539 File Offset: 0x00250739
			public Size Viewport
			{
				get
				{
					return this._viewport;
				}
				set
				{
					this._viewport = value;
				}
			}

			// Token: 0x17001ED1 RID: 7889
			// (get) Token: 0x06008885 RID: 34949 RVA: 0x00252542 File Offset: 0x00250742
			// (set) Token: 0x06008886 RID: 34950 RVA: 0x0025254A File Offset: 0x0025074A
			public Size Extent
			{
				get
				{
					return this._extent;
				}
				set
				{
					this._extent = value;
				}
			}

			// Token: 0x17001ED2 RID: 7890
			// (get) Token: 0x06008887 RID: 34951 RVA: 0x00252553 File Offset: 0x00250753
			// (set) Token: 0x06008888 RID: 34952 RVA: 0x0025255B File Offset: 0x0025075B
			public Vector ComputedOffset
			{
				get
				{
					return this._computedOffset;
				}
				set
				{
					this._computedOffset = value;
				}
			}

			// Token: 0x06008889 RID: 34953 RVA: 0x00252564 File Offset: 0x00250764
			public void SetPhysicalViewport(double value)
			{
				this._physicalViewport = value;
			}

			// Token: 0x04004564 RID: 17764
			internal bool _allowHorizontal;

			// Token: 0x04004565 RID: 17765
			internal bool _allowVertical;

			// Token: 0x04004566 RID: 17766
			internal Vector _offset;

			// Token: 0x04004567 RID: 17767
			internal Vector _computedOffset = new Vector(0.0, 0.0);

			// Token: 0x04004568 RID: 17768
			internal Size _viewport;

			// Token: 0x04004569 RID: 17769
			internal Size _extent;

			// Token: 0x0400456A RID: 17770
			internal double _physicalViewport;

			// Token: 0x0400456B RID: 17771
			internal ScrollViewer _scrollOwner;
		}
	}
}
