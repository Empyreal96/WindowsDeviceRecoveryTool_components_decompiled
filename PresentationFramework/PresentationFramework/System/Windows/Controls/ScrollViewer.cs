using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Represents a scrollable area that can contain other visible elements. </summary>
	// Token: 0x02000525 RID: 1317
	[DefaultEvent("ScrollChangedEvent")]
	[Localizability(LocalizationCategory.Ignore)]
	[TemplatePart(Name = "PART_HorizontalScrollBar", Type = typeof(ScrollBar))]
	[TemplatePart(Name = "PART_VerticalScrollBar", Type = typeof(ScrollBar))]
	[TemplatePart(Name = "PART_ScrollContentPresenter", Type = typeof(ScrollContentPresenter))]
	public class ScrollViewer : ContentControl
	{
		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content upward by one line. </summary>
		// Token: 0x060054EA RID: 21738 RVA: 0x00177DF0 File Offset: 0x00175FF0
		public void LineUp()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineUp, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content downward by one line. </summary>
		// Token: 0x060054EB RID: 21739 RVA: 0x00177E03 File Offset: 0x00176003
		public void LineDown()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineDown, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content to the left by a predetermined amount. </summary>
		// Token: 0x060054EC RID: 21740 RVA: 0x00177E16 File Offset: 0x00176016
		public void LineLeft()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineLeft, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content to the right by a predetermined amount. </summary>
		// Token: 0x060054ED RID: 21741 RVA: 0x00177E29 File Offset: 0x00176029
		public void LineRight()
		{
			this.EnqueueCommand(ScrollViewer.Commands.LineRight, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content upward by one page. </summary>
		// Token: 0x060054EE RID: 21742 RVA: 0x00177E3C File Offset: 0x0017603C
		public void PageUp()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageUp, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content downward by one page. </summary>
		// Token: 0x060054EF RID: 21743 RVA: 0x00177E4F File Offset: 0x0017604F
		public void PageDown()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageDown, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content to the left by one page. </summary>
		// Token: 0x060054F0 RID: 21744 RVA: 0x00177E62 File Offset: 0x00176062
		public void PageLeft()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageLeft, 0.0, null);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollViewer" /> content to the right by one page. </summary>
		// Token: 0x060054F1 RID: 21745 RVA: 0x00177E75 File Offset: 0x00176075
		public void PageRight()
		{
			this.EnqueueCommand(ScrollViewer.Commands.PageRight, 0.0, null);
		}

		/// <summary>Scrolls horizontally to the beginning of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content. </summary>
		// Token: 0x060054F2 RID: 21746 RVA: 0x00177E88 File Offset: 0x00176088
		public void ScrollToLeftEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
		}

		/// <summary>Scrolls horizontally to the end of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content. </summary>
		// Token: 0x060054F3 RID: 21747 RVA: 0x00177E9C File Offset: 0x0017609C
		public void ScrollToRightEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.PositiveInfinity, null);
		}

		/// <summary>Scrolls vertically to the beginning of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content. </summary>
		// Token: 0x060054F4 RID: 21748 RVA: 0x00177EB0 File Offset: 0x001760B0
		public void ScrollToHome()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.NegativeInfinity, null);
		}

		/// <summary>Scrolls vertically to the end of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content. </summary>
		// Token: 0x060054F5 RID: 21749 RVA: 0x00177ED6 File Offset: 0x001760D6
		public void ScrollToEnd()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, double.NegativeInfinity, null);
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.PositiveInfinity, null);
		}

		/// <summary>Scrolls vertically to the beginning of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content. </summary>
		// Token: 0x060054F6 RID: 21750 RVA: 0x00177EFC File Offset: 0x001760FC
		public void ScrollToTop()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.NegativeInfinity, null);
		}

		/// <summary>Scrolls vertically to the end of the <see cref="T:System.Windows.Controls.ScrollViewer" /> content.</summary>
		// Token: 0x060054F7 RID: 21751 RVA: 0x00177F10 File Offset: 0x00176110
		public void ScrollToBottom()
		{
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, double.PositiveInfinity, null);
		}

		/// <summary>Scrolls the content within the <see cref="T:System.Windows.Controls.ScrollViewer" /> to the specified horizontal offset position.</summary>
		/// <param name="offset">The position that the content scrolls to.</param>
		// Token: 0x060054F8 RID: 21752 RVA: 0x00177F24 File Offset: 0x00176124
		public void ScrollToHorizontalOffset(double offset)
		{
			double param = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.EnqueueCommand(ScrollViewer.Commands.SetHorizontalOffset, param, null);
		}

		/// <summary>Scrolls the content within the <see cref="T:System.Windows.Controls.ScrollViewer" /> to the specified vertical offset position.</summary>
		/// <param name="offset">The position that the content scrolls to.</param>
		// Token: 0x060054F9 RID: 21753 RVA: 0x00177F48 File Offset: 0x00176148
		public void ScrollToVerticalOffset(double offset)
		{
			double param = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.EnqueueCommand(ScrollViewer.Commands.SetVerticalOffset, param, null);
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x00177F6C File Offset: 0x0017616C
		private void DeferScrollToHorizontalOffset(double offset)
		{
			double horizontalOffset = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.HorizontalOffset = horizontalOffset;
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x00177F8C File Offset: 0x0017618C
		private void DeferScrollToVerticalOffset(double offset)
		{
			double verticalOffset = ScrollContentPresenter.ValidateInputOffset(offset, "offset");
			this.VerticalOffset = verticalOffset;
		}

		// Token: 0x060054FC RID: 21756 RVA: 0x00177FAC File Offset: 0x001761AC
		internal void MakeVisible(Visual child, Rect rect)
		{
			ScrollViewer.MakeVisibleParams mvp = new ScrollViewer.MakeVisibleParams(child, rect);
			this.EnqueueCommand(ScrollViewer.Commands.MakeVisible, 0.0, mvp);
		}

		// Token: 0x060054FD RID: 21757 RVA: 0x00177FD3 File Offset: 0x001761D3
		private void EnsureLayoutUpdatedHandler()
		{
			if (this._layoutUpdatedHandler == null)
			{
				this._layoutUpdatedHandler = new EventHandler(this.OnLayoutUpdated);
				base.LayoutUpdated += this._layoutUpdatedHandler;
			}
			base.InvalidateArrange();
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x00178001 File Offset: 0x00176201
		private void ClearLayoutUpdatedHandler()
		{
			if (this._layoutUpdatedHandler != null && this._queue.IsEmpty())
			{
				base.LayoutUpdated -= this._layoutUpdatedHandler;
				this._layoutUpdatedHandler = null;
			}
		}

		/// <summary>Called by an <see cref="T:System.Windows.Controls.Primitives.IScrollInfo" /> interface that is attached to a <see cref="T:System.Windows.Controls.ScrollViewer" /> when the value of any scrolling property size changes. Scrolling properties include offset, extent, or viewport. </summary>
		// Token: 0x060054FF RID: 21759 RVA: 0x0017802C File Offset: 0x0017622C
		public void InvalidateScrollInfo()
		{
			if (this.ScrollInfo == null)
			{
				return;
			}
			if (!base.MeasureInProgress && (!base.ArrangeInProgress || !this.InvalidatedMeasureFromArrange))
			{
				double value = this.ScrollInfo.ExtentWidth;
				double value2 = this.ScrollInfo.ViewportWidth;
				if (this.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto && ((this._scrollVisibilityX == Visibility.Collapsed && DoubleUtil.GreaterThan(value, value2)) || (this._scrollVisibilityX == Visibility.Visible && DoubleUtil.LessThanOrClose(value, value2))))
				{
					base.InvalidateMeasure();
				}
				else
				{
					value = this.ScrollInfo.ExtentHeight;
					value2 = this.ScrollInfo.ViewportHeight;
					if (this.VerticalScrollBarVisibility == ScrollBarVisibility.Auto && ((this._scrollVisibilityY == Visibility.Collapsed && DoubleUtil.GreaterThan(value, value2)) || (this._scrollVisibilityY == Visibility.Visible && DoubleUtil.LessThanOrClose(value, value2))))
					{
						base.InvalidateMeasure();
					}
				}
			}
			if (!DoubleUtil.AreClose(this.HorizontalOffset, this.ScrollInfo.HorizontalOffset) || !DoubleUtil.AreClose(this.VerticalOffset, this.ScrollInfo.VerticalOffset) || !DoubleUtil.AreClose(this.ViewportWidth, this.ScrollInfo.ViewportWidth) || !DoubleUtil.AreClose(this.ViewportHeight, this.ScrollInfo.ViewportHeight) || !DoubleUtil.AreClose(this.ExtentWidth, this.ScrollInfo.ExtentWidth) || !DoubleUtil.AreClose(this.ExtentHeight, this.ScrollInfo.ExtentHeight))
			{
				this.EnsureLayoutUpdatedHandler();
			}
		}

		/// <summary>Gets or sets a value that indicates whether elements that support the <see cref="T:System.Windows.Controls.Primitives.IScrollInfo" /> interface are allowed to scroll.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls in terms of logical units; <see langword="false" /> if the <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls in terms of physical units. The default is <see langword="false" />.</returns>
		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06005500 RID: 21760 RVA: 0x0017818E File Offset: 0x0017638E
		// (set) Token: 0x06005501 RID: 21761 RVA: 0x001781A0 File Offset: 0x001763A0
		public bool CanContentScroll
		{
			get
			{
				return (bool)base.GetValue(ScrollViewer.CanContentScrollProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.CanContentScrollProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> should be displayed.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> value that indicates whether a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> should be displayed. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Hidden" />.</returns>
		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x06005502 RID: 21762 RVA: 0x001781AE File Offset: 0x001763AE
		// (set) Token: 0x06005503 RID: 21763 RVA: 0x001781C0 File Offset: 0x001763C0
		[Bindable(true)]
		[Category("Appearance")]
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> should be displayed.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> value that indicates whether a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> should be displayed. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Visible" />.</returns>
		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x06005504 RID: 21764 RVA: 0x001781D3 File Offset: 0x001763D3
		// (set) Token: 0x06005505 RID: 21765 RVA: 0x001781E5 File Offset: 0x001763E5
		[Bindable(true)]
		[Category("Appearance")]
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is visible.  </summary>
		/// <returns>A <see cref="T:System.Windows.Visibility" /> that indicates whether the horizontal scroll bar is visible. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Hidden" />.</returns>
		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x06005506 RID: 21766 RVA: 0x001781F8 File Offset: 0x001763F8
		public Visibility ComputedHorizontalScrollBarVisibility
		{
			get
			{
				return this._scrollVisibilityX;
			}
		}

		/// <summary>Gets a value that indicates whether the vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is visible.   </summary>
		/// <returns>A <see cref="T:System.Windows.Visibility" /> that indicates whether the vertical scroll bar is visible. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Visible" />.</returns>
		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x06005507 RID: 21767 RVA: 0x00178200 File Offset: 0x00176400
		public Visibility ComputedVerticalScrollBarVisibility
		{
			get
			{
				return this._scrollVisibilityY;
			}
		}

		/// <summary>Gets a value that contains the horizontal offset of the scrolled content.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the horizontal offset of the scrolled content. The default is 0.0.</returns>
		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x06005508 RID: 21768 RVA: 0x00178208 File Offset: 0x00176408
		// (set) Token: 0x06005509 RID: 21769 RVA: 0x00178210 File Offset: 0x00176410
		public double HorizontalOffset
		{
			get
			{
				return this._xPositionISI;
			}
			private set
			{
				base.SetValue(ScrollViewer.HorizontalOffsetPropertyKey, value);
			}
		}

		/// <summary>Gets a value that contains the vertical offset of the scrolled content.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical offset of the scrolled content. The default is 0.0.</returns>
		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x0600550A RID: 21770 RVA: 0x00178223 File Offset: 0x00176423
		// (set) Token: 0x0600550B RID: 21771 RVA: 0x0017822B File Offset: 0x0017642B
		public double VerticalOffset
		{
			get
			{
				return this._yPositionISI;
			}
			private set
			{
				base.SetValue(ScrollViewer.VerticalOffsetPropertyKey, value);
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the extent.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the horizontal size of the extent. The default is 0.0.</returns>
		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x0600550C RID: 21772 RVA: 0x0017823E File Offset: 0x0017643E
		[Category("Layout")]
		public double ExtentWidth
		{
			get
			{
				return this._xExtent;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the extent.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the extent. The default is 0.0.</returns>
		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x0600550D RID: 21773 RVA: 0x00178246 File Offset: 0x00176446
		[Category("Layout")]
		public double ExtentHeight
		{
			get
			{
				return this._yExtent;
			}
		}

		/// <summary>Gets a value that represents the horizontal size of the content element that can be scrolled.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the horizontal size of the content element that can be scrolled. This property has no default value.</returns>
		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x0600550E RID: 21774 RVA: 0x0017824E File Offset: 0x0017644E
		public double ScrollableWidth
		{
			get
			{
				return Math.Max(0.0, this.ExtentWidth - this.ViewportWidth);
			}
		}

		/// <summary>Gets a value that represents the vertical size of the content element that can be scrolled.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the content element that can be scrolled. This property has no default value.</returns>
		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x0600550F RID: 21775 RVA: 0x0017826B File Offset: 0x0017646B
		public double ScrollableHeight
		{
			get
			{
				return Math.Max(0.0, this.ExtentHeight - this.ViewportHeight);
			}
		}

		/// <summary>Gets a value that contains the horizontal size of the content's viewport.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the horizontal size of the content's viewport. The default is 0.0.</returns>
		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x06005510 RID: 21776 RVA: 0x00178288 File Offset: 0x00176488
		[Category("Layout")]
		public double ViewportWidth
		{
			get
			{
				return this._xSize;
			}
		}

		/// <summary>Gets a value that contains the vertical size of the content's viewport.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the vertical size of the content's viewport. This property has no default value.</returns>
		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x06005511 RID: 21777 RVA: 0x00178290 File Offset: 0x00176490
		[Category("Layout")]
		public double ViewportHeight
		{
			get
			{
				return this._ySize;
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.CanContentScroll" /> dependency property to a given element.</summary>
		/// <param name="element">The element on which to set the property value.</param>
		/// <param name="canContentScroll">The property value to set.</param>
		// Token: 0x06005512 RID: 21778 RVA: 0x00178298 File Offset: 0x00176498
		public static void SetCanContentScroll(DependencyObject element, bool canContentScroll)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.CanContentScrollProperty, canContentScroll);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.CanContentScroll" /> dependency property from a given element.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>
		///     <see langword="true" /> if this element can scroll; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005513 RID: 21779 RVA: 0x001782B4 File Offset: 0x001764B4
		public static bool GetCanContentScroll(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ScrollViewer.CanContentScrollProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility" /> dependency property to a given element.</summary>
		/// <param name="element">The element on which to set the property value.</param>
		/// <param name="horizontalScrollBarVisibility">The property value to set.</param>
		// Token: 0x06005514 RID: 21780 RVA: 0x001782D4 File Offset: 0x001764D4
		public static void SetHorizontalScrollBarVisibility(DependencyObject element, ScrollBarVisibility horizontalScrollBarVisibility)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, horizontalScrollBarVisibility);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility" /> dependency property from a given element.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x06005515 RID: 21781 RVA: 0x001782F5 File Offset: 0x001764F5
		public static ScrollBarVisibility GetHorizontalScrollBarVisibility(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollBarVisibility)element.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility" /> dependency property to a given element.</summary>
		/// <param name="element">The element on which to set the property value.</param>
		/// <param name="verticalScrollBarVisibility">The property value to set.</param>
		// Token: 0x06005516 RID: 21782 RVA: 0x00178315 File Offset: 0x00176515
		public static void SetVerticalScrollBarVisibility(DependencyObject element, ScrollBarVisibility verticalScrollBarVisibility)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, verticalScrollBarVisibility);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility" /> dependency property from a given element.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility" />  dependency property.</returns>
		// Token: 0x06005517 RID: 21783 RVA: 0x00178336 File Offset: 0x00176536
		public static ScrollBarVisibility GetVerticalScrollBarVisibility(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollBarVisibility)element.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
		}

		/// <summary>Gets the vertical offset of the visible content.</summary>
		/// <returns>The vertical offset of the visible content.</returns>
		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x06005518 RID: 21784 RVA: 0x00178356 File Offset: 0x00176556
		// (set) Token: 0x06005519 RID: 21785 RVA: 0x00178368 File Offset: 0x00176568
		public double ContentVerticalOffset
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.ContentVerticalOffsetProperty);
			}
			private set
			{
				base.SetValue(ScrollViewer.ContentVerticalOffsetPropertyKey, value);
			}
		}

		/// <summary>Gets the horizontal offset of the visible content.</summary>
		/// <returns>The horizontal offset of the visible content.</returns>
		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x0600551A RID: 21786 RVA: 0x0017837B File Offset: 0x0017657B
		// (set) Token: 0x0600551B RID: 21787 RVA: 0x0017838D File Offset: 0x0017658D
		public double ContentHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.ContentHorizontalOffsetProperty);
			}
			private set
			{
				base.SetValue(ScrollViewer.ContentHorizontalOffsetPropertyKey, value);
			}
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" /> property for the specified object.</summary>
		/// <param name="element">The object from which to get <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" />.</param>
		/// <returns>
		///     <see langword="true" /> if the content is stationary when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600551C RID: 21788 RVA: 0x001783A0 File Offset: 0x001765A0
		public static bool GetIsDeferredScrollingEnabled(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ScrollViewer.IsDeferredScrollingEnabledProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" /> property for the specified object.</summary>
		/// <param name="element">The object on which to set the <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" /> property.</param>
		/// <param name="value">
		///       <see langword="true" /> to have the content remain stationary when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />; otherwise, <see langword="false" />.</param>
		// Token: 0x0600551D RID: 21789 RVA: 0x001783C0 File Offset: 0x001765C0
		public static void SetIsDeferredScrollingEnabled(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, BooleanBoxes.Box(value));
		}

		/// <summary>Gets or sets a value that indicates whether the content is stationary when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the content is stationary when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170014B9 RID: 5305
		// (get) Token: 0x0600551E RID: 21790 RVA: 0x001783E1 File Offset: 0x001765E1
		// (set) Token: 0x0600551F RID: 21791 RVA: 0x001783F3 File Offset: 0x001765F3
		public bool IsDeferredScrollingEnabled
		{
			get
			{
				return (bool)base.GetValue(ScrollViewer.IsDeferredScrollingEnabledProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Occurs when changes are detected to the scroll position, extent, or viewport size.</summary>
		// Token: 0x14000107 RID: 263
		// (add) Token: 0x06005520 RID: 21792 RVA: 0x00178406 File Offset: 0x00176606
		// (remove) Token: 0x06005521 RID: 21793 RVA: 0x00178414 File Offset: 0x00176614
		[Category("Action")]
		public event ScrollChangedEventHandler ScrollChanged
		{
			add
			{
				base.AddHandler(ScrollViewer.ScrollChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ScrollViewer.ScrollChangedEvent, value);
			}
		}

		/// <summary>Called when a tap gesture initiated by a stylus is detected.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.StylusSystemGestureEventArgs" /> that contains information about the gesture.</param>
		// Token: 0x06005522 RID: 21794 RVA: 0x00178422 File Offset: 0x00176622
		protected override void OnStylusSystemGesture(StylusSystemGestureEventArgs e)
		{
			this._seenTapGesture = (e.SystemGesture == SystemGesture.Tap);
		}

		/// <summary>Called when a change in scrolling state is detected, such as a change in scroll position, extent, or viewport size.</summary>
		/// <param name="e">The <see cref="T:System.Windows.Controls.ScrollChangedEventArgs" /> that contain information about the change in the scrolling state.</param>
		// Token: 0x06005523 RID: 21795 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnScrollChanged(ScrollChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Performs a hit test to determine whether the specified points are within the bounds of this <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <param name="hitTestParameters">The parameters for hit testing within a visual object.</param>
		/// <returns>The result of the hit test.</returns>
		// Token: 0x06005524 RID: 21796 RVA: 0x00178434 File Offset: 0x00176634
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			Rect rect = new Rect(0.0, 0.0, base.ActualWidth, base.ActualHeight);
			if (rect.Contains(hitTestParameters.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParameters.HitPoint);
			}
			return null;
		}

		/// <summary>Gets a value that indicates that a control has a <see cref="T:System.Windows.Controls.ScrollViewer" /> defined in its style that defines custom keyboard scrolling behavior.</summary>
		/// <returns>
		///     <see langword="true" /> if this control defines custom keyboard scrolling behavior; otherwise, <see langword="false" />.</returns>
		// Token: 0x170014BA RID: 5306
		// (get) Token: 0x06005525 RID: 21797 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		/// <summary>Responds to specific keyboard input and invokes associated scrolling behavior.</summary>
		/// <param name="e">Required arguments for this event.</param>
		// Token: 0x06005526 RID: 21798 RVA: 0x00178484 File Offset: 0x00176684
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			Control control = base.TemplatedParent as Control;
			if (control != null && control.HandlesScrolling)
			{
				return;
			}
			if (e.OriginalSource == this)
			{
				this.ScrollInDirection(e);
				return;
			}
			if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
			{
				ScrollContentPresenter scrollContentPresenter = base.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (scrollContentPresenter == null)
				{
					this.ScrollInDirection(e);
					return;
				}
				FocusNavigationDirection direction = KeyboardNavigation.KeyToTraversalDirection(e.Key);
				DependencyObject dependencyObject = null;
				DependencyObject dependencyObject2 = Keyboard.FocusedElement as DependencyObject;
				bool flag = this.IsInViewport(scrollContentPresenter, dependencyObject2);
				if (flag)
				{
					UIElement uielement = dependencyObject2 as UIElement;
					if (uielement != null)
					{
						dependencyObject = uielement.PredictFocus(direction);
					}
					else
					{
						ContentElement contentElement = dependencyObject2 as ContentElement;
						if (contentElement != null)
						{
							dependencyObject = contentElement.PredictFocus(direction);
						}
						else
						{
							UIElement3D uielement3D = dependencyObject2 as UIElement3D;
							if (uielement3D != null)
							{
								dependencyObject = uielement3D.PredictFocus(direction);
							}
						}
					}
				}
				else
				{
					dependencyObject = scrollContentPresenter.PredictFocus(direction);
				}
				if (dependencyObject == null)
				{
					this.ScrollInDirection(e);
					return;
				}
				if (this.IsInViewport(scrollContentPresenter, dependencyObject))
				{
					((IInputElement)dependencyObject).Focus();
					e.Handled = true;
					return;
				}
				this.ScrollInDirection(e);
				base.UpdateLayout();
				if (this.IsInViewport(scrollContentPresenter, dependencyObject))
				{
					((IInputElement)dependencyObject).Focus();
					return;
				}
			}
			else
			{
				this.ScrollInDirection(e);
			}
		}

		// Token: 0x06005527 RID: 21799 RVA: 0x001785D8 File Offset: 0x001767D8
		private bool IsInViewport(ScrollContentPresenter scp, DependencyObject element)
		{
			Visual visualRoot = KeyboardNavigation.GetVisualRoot(scp);
			Visual visualRoot2 = KeyboardNavigation.GetVisualRoot(element);
			while (visualRoot != visualRoot2)
			{
				if (visualRoot2 == null)
				{
					return false;
				}
				FrameworkElement frameworkElement = visualRoot2 as FrameworkElement;
				if (frameworkElement == null)
				{
					return false;
				}
				element = frameworkElement.Parent;
				if (element == null)
				{
					return false;
				}
				visualRoot2 = KeyboardNavigation.GetVisualRoot(element);
			}
			Rect rectangle = KeyboardNavigation.GetRectangle(scp);
			Rect rectangle2 = KeyboardNavigation.GetRectangle(element);
			return rectangle.IntersectsWith(rectangle2);
		}

		// Token: 0x06005528 RID: 21800 RVA: 0x00178638 File Offset: 0x00176838
		internal void ScrollInDirection(KeyEventArgs e)
		{
			bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) > ModifierKeys.None;
			if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) <= ModifierKeys.None)
			{
				bool flag2 = base.FlowDirection == FlowDirection.RightToLeft;
				switch (e.Key)
				{
				case Key.Prior:
					this.PageUp();
					e.Handled = true;
					return;
				case Key.Next:
					this.PageDown();
					e.Handled = true;
					return;
				case Key.End:
					if (flag)
					{
						this.ScrollToBottom();
					}
					else
					{
						this.ScrollToRightEnd();
					}
					e.Handled = true;
					break;
				case Key.Home:
					if (flag)
					{
						this.ScrollToTop();
					}
					else
					{
						this.ScrollToLeftEnd();
					}
					e.Handled = true;
					return;
				case Key.Left:
					if (flag2)
					{
						this.LineRight();
					}
					else
					{
						this.LineLeft();
					}
					e.Handled = true;
					return;
				case Key.Up:
					this.LineUp();
					e.Handled = true;
					return;
				case Key.Right:
					if (flag2)
					{
						this.LineLeft();
					}
					else
					{
						this.LineRight();
					}
					e.Handled = true;
					return;
				case Key.Down:
					this.LineDown();
					e.Handled = true;
					return;
				default:
					return;
				}
			}
		}

		/// <summary>Responds to a click of the mouse wheel.</summary>
		/// <param name="e">Required arguments that describe this event.</param>
		// Token: 0x06005529 RID: 21801 RVA: 0x00178744 File Offset: 0x00176944
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (!this.HandlesMouseWheelScrolling)
			{
				return;
			}
			if (this.ScrollInfo != null)
			{
				if (e.Delta < 0)
				{
					this.ScrollInfo.MouseWheelDown();
				}
				else
				{
					this.ScrollInfo.MouseWheelUp();
				}
			}
			e.Handled = true;
		}

		/// <summary>Responds to a click of the left mouse button. </summary>
		/// <param name="e">Required arguments that describe this event.</param>
		// Token: 0x0600552A RID: 21802 RVA: 0x00178793 File Offset: 0x00176993
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (base.Focus())
			{
				e.Handled = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Measures the content of a <see cref="T:System.Windows.Controls.ScrollViewer" /> element.</summary>
		/// <param name="constraint">The upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The computed desired limit <see cref="T:System.Windows.Size" /> of the <see cref="T:System.Windows.Controls.ScrollViewer" /> element.</returns>
		// Token: 0x0600552B RID: 21803 RVA: 0x001787AC File Offset: 0x001769AC
		protected override Size MeasureOverride(Size constraint)
		{
			this.InChildInvalidateMeasure = false;
			IScrollInfo scrollInfo = this.ScrollInfo;
			int visualChildrenCount = this.VisualChildrenCount;
			UIElement uielement = (visualChildrenCount > 0) ? (this.GetVisualChild(0) as UIElement) : null;
			ScrollBarVisibility verticalScrollBarVisibility = this.VerticalScrollBarVisibility;
			ScrollBarVisibility horizontalScrollBarVisibility = this.HorizontalScrollBarVisibility;
			Size result = default(Size);
			if (uielement != null)
			{
				bool flag = EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLVIEWER:MeasureOverride");
				}
				try
				{
					bool flag2 = verticalScrollBarVisibility == ScrollBarVisibility.Auto;
					bool flag3 = horizontalScrollBarVisibility == ScrollBarVisibility.Auto;
					bool flag4 = verticalScrollBarVisibility == ScrollBarVisibility.Disabled;
					bool flag5 = horizontalScrollBarVisibility == ScrollBarVisibility.Disabled;
					Visibility visibility = (verticalScrollBarVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
					Visibility visibility2 = (horizontalScrollBarVisibility == ScrollBarVisibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
					if (this._scrollVisibilityY != visibility)
					{
						this._scrollVisibilityY = visibility;
						base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
					}
					if (this._scrollVisibilityX != visibility2)
					{
						this._scrollVisibilityX = visibility2;
						base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
					}
					if (scrollInfo != null)
					{
						scrollInfo.CanHorizontallyScroll = !flag5;
						scrollInfo.CanVerticallyScroll = !flag4;
					}
					try
					{
						this.InChildMeasurePass1 = true;
						uielement.Measure(constraint);
					}
					finally
					{
						this.InChildMeasurePass1 = false;
					}
					scrollInfo = this.ScrollInfo;
					if (scrollInfo != null && (flag3 || flag2))
					{
						bool flag6 = flag3 && DoubleUtil.GreaterThan(scrollInfo.ExtentWidth, scrollInfo.ViewportWidth);
						bool flag7 = flag2 && DoubleUtil.GreaterThan(scrollInfo.ExtentHeight, scrollInfo.ViewportHeight);
						if (flag6 && this._scrollVisibilityX != Visibility.Visible)
						{
							this._scrollVisibilityX = Visibility.Visible;
							base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
						}
						if (flag7 && this._scrollVisibilityY != Visibility.Visible)
						{
							this._scrollVisibilityY = Visibility.Visible;
							base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
						}
						if (flag6 || flag7)
						{
							this.InChildInvalidateMeasure = true;
							uielement.InvalidateMeasure();
							try
							{
								this.InChildMeasurePass2 = true;
								uielement.Measure(constraint);
							}
							finally
							{
								this.InChildMeasurePass2 = false;
							}
						}
						if (flag3 && flag2 && flag6 != flag7)
						{
							bool flag8 = !flag6 && DoubleUtil.GreaterThan(scrollInfo.ExtentWidth, scrollInfo.ViewportWidth);
							bool flag9 = !flag7 && DoubleUtil.GreaterThan(scrollInfo.ExtentHeight, scrollInfo.ViewportHeight);
							if (flag8)
							{
								if (this._scrollVisibilityX != Visibility.Visible)
								{
									this._scrollVisibilityX = Visibility.Visible;
									base.SetValue(ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey, this._scrollVisibilityX);
								}
							}
							else if (flag9 && this._scrollVisibilityY != Visibility.Visible)
							{
								this._scrollVisibilityY = Visibility.Visible;
								base.SetValue(ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey, this._scrollVisibilityY);
							}
							if (flag8 || flag9)
							{
								this.InChildInvalidateMeasure = true;
								uielement.InvalidateMeasure();
								try
								{
									this.InChildMeasurePass3 = true;
									uielement.Measure(constraint);
								}
								finally
								{
									this.InChildMeasurePass3 = false;
								}
							}
						}
					}
				}
				finally
				{
					if (flag)
					{
						EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLVIEWER:MeasureOverride");
					}
				}
				result = uielement.DesiredSize;
			}
			if (!base.ArrangeDirty && this.InvalidatedMeasureFromArrange)
			{
				this.InvalidatedMeasureFromArrange = false;
			}
			return result;
		}

		/// <summary>Arranges the content of the <see cref="T:System.Windows.Controls.ScrollViewer" />.</summary>
		/// <param name="arrangeSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		// Token: 0x0600552C RID: 21804 RVA: 0x00178B00 File Offset: 0x00176D00
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool invalidatedMeasureFromArrange = this.InvalidatedMeasureFromArrange;
			Size result = base.ArrangeOverride(arrangeSize);
			if (invalidatedMeasureFromArrange)
			{
				this.InvalidatedMeasureFromArrange = false;
			}
			else
			{
				this.InvalidatedMeasureFromArrange = base.MeasureDirty;
			}
			return result;
		}

		// Token: 0x0600552D RID: 21805 RVA: 0x00178B38 File Offset: 0x00176D38
		private void BindToTemplatedParent(DependencyProperty property)
		{
			if (!base.HasNonDefaultValue(property))
			{
				base.SetBinding(property, new Binding
				{
					RelativeSource = RelativeSource.TemplatedParent,
					Path = new PropertyPath(property)
				});
			}
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x00178B74 File Offset: 0x00176D74
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent != null)
			{
				this.BindToTemplatedParent(ScrollViewer.HorizontalScrollBarVisibilityProperty);
				this.BindToTemplatedParent(ScrollViewer.VerticalScrollBarVisibilityProperty);
				this.BindToTemplatedParent(ScrollViewer.CanContentScrollProperty);
				this.BindToTemplatedParent(ScrollViewer.IsDeferredScrollingEnabledProperty);
				this.BindToTemplatedParent(ScrollViewer.PanningModeProperty);
			}
		}

		/// <summary>Called when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />, which is used to build the current template's visual tree.</summary>
		// Token: 0x0600552F RID: 21807 RVA: 0x00178BC8 File Offset: 0x00176DC8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ScrollBar scrollBar = base.GetTemplateChild("PART_HorizontalScrollBar") as ScrollBar;
			if (scrollBar != null)
			{
				scrollBar.IsStandalone = false;
			}
			scrollBar = (base.GetTemplateChild("PART_VerticalScrollBar") as ScrollBar);
			if (scrollBar != null)
			{
				scrollBar.IsStandalone = false;
			}
			this.OnPanningModeChanged();
		}

		/// <summary>Gets or sets the element that implements the <see cref="T:System.Windows.Controls.Primitives.IScrollInfo" /> interface and provides values for scrolling properties of this <see cref="T:System.Windows.Controls.ScrollViewer" />. </summary>
		/// <returns>The element that controls scrolling properties, such as extent, offset, or viewport size. This property has no default value.</returns>
		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06005530 RID: 21808 RVA: 0x00178C17 File Offset: 0x00176E17
		// (set) Token: 0x06005531 RID: 21809 RVA: 0x00178C1F File Offset: 0x00176E1F
		protected internal IScrollInfo ScrollInfo
		{
			get
			{
				return this._scrollInfo;
			}
			set
			{
				this._scrollInfo = value;
				if (this._scrollInfo != null)
				{
					this._scrollInfo.CanHorizontallyScroll = (this.HorizontalScrollBarVisibility > ScrollBarVisibility.Disabled);
					this._scrollInfo.CanVerticallyScroll = (this.VerticalScrollBarVisibility > ScrollBarVisibility.Disabled);
					this.EnsureQueueProcessing();
				}
			}
		}

		/// <summary>Gets or sets the way <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to touch manipulation.</summary>
		/// <returns>A value that specifies how <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to touch manipulation.  The default is <see cref="F:System.Windows.Controls.PanningMode.None" />.</returns>
		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x06005532 RID: 21810 RVA: 0x00178C5E File Offset: 0x00176E5E
		// (set) Token: 0x06005533 RID: 21811 RVA: 0x00178C70 File Offset: 0x00176E70
		public PanningMode PanningMode
		{
			get
			{
				return (PanningMode)base.GetValue(ScrollViewer.PanningModeProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningModeProperty, value);
			}
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.ScrollViewer.PanningMode" /> property for the specified object.</summary>
		/// <param name="element">The object on which to set the <see cref="P:System.Windows.Controls.ScrollViewer.PanningMode" /> property.</param>
		/// <param name="panningMode">A value that specifies how <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to touch manipulation.</param>
		// Token: 0x06005534 RID: 21812 RVA: 0x00178C83 File Offset: 0x00176E83
		public static void SetPanningMode(DependencyObject element, PanningMode panningMode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningModeProperty, panningMode);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.ScrollViewer.PanningMode" /> property for the specified object.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>A value that specifies how <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to touch manipulation.</returns>
		// Token: 0x06005535 RID: 21813 RVA: 0x00178CA4 File Offset: 0x00176EA4
		public static PanningMode GetPanningMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PanningMode)element.GetValue(ScrollViewer.PanningModeProperty);
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x00178CC4 File Offset: 0x00176EC4
		private static void OnPanningModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = d as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.OnPanningModeChanged();
			}
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x00178CE4 File Offset: 0x00176EE4
		private void OnPanningModeChanged()
		{
			if (!base.HasTemplateGeneratedSubTree)
			{
				return;
			}
			PanningMode panningMode = this.PanningMode;
			base.InvalidateProperty(UIElement.IsManipulationEnabledProperty);
			if (panningMode != PanningMode.None)
			{
				base.SetCurrentValueInternal(UIElement.IsManipulationEnabledProperty, BooleanBoxes.TrueBox);
			}
		}

		/// <summary>Gets or sets the rate <see cref="T:System.Windows.Controls.ScrollViewer" /> slows in device-independent units (1/96th inch per unit) per squared millisecond when in inertia.</summary>
		/// <returns>The rate <see cref="T:System.Windows.Controls.ScrollViewer" /> slows in device-independent units (1/96th inch per unit) per squared millisecond.</returns>
		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x06005538 RID: 21816 RVA: 0x00178D1F File Offset: 0x00176F1F
		// (set) Token: 0x06005539 RID: 21817 RVA: 0x00178D31 File Offset: 0x00176F31
		public double PanningDeceleration
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.PanningDecelerationProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningDecelerationProperty, value);
			}
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.ScrollViewer.PanningDeceleration" /> property for the specified object.</summary>
		/// <param name="element">The object on which to set the <see cref="P:System.Windows.Controls.ScrollViewer.PanningDeceleration" /> property.</param>
		/// <param name="value">The rate <see cref="T:System.Windows.Controls.ScrollViewer" /> slows in device-independent units (1/96th inch per unit) per squared millisecond.</param>
		// Token: 0x0600553A RID: 21818 RVA: 0x00178D44 File Offset: 0x00176F44
		public static void SetPanningDeceleration(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningDecelerationProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.ScrollViewer.PanningDeceleration" /> property for the specified object.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The rate <see cref="T:System.Windows.Controls.ScrollViewer" /> slows in device-independent units (1/96th inch per unit) per squared millisecond.</returns>
		// Token: 0x0600553B RID: 21819 RVA: 0x00178D65 File Offset: 0x00176F65
		public static double GetPanningDeceleration(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ScrollViewer.PanningDecelerationProperty);
		}

		/// <summary>Gets or sets the ratio of scrolling offset to translate manipulation offset.</summary>
		/// <returns>The ratio of scrolling offset to translate manipulation offset. The default is 1.</returns>
		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x0600553C RID: 21820 RVA: 0x00178D85 File Offset: 0x00176F85
		// (set) Token: 0x0600553D RID: 21821 RVA: 0x00178D97 File Offset: 0x00176F97
		public double PanningRatio
		{
			get
			{
				return (double)base.GetValue(ScrollViewer.PanningRatioProperty);
			}
			set
			{
				base.SetValue(ScrollViewer.PanningRatioProperty, value);
			}
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.ScrollViewer.PanningRatio" /> property for the specified object.</summary>
		/// <param name="element">The object on which to set the <see cref="P:System.Windows.Controls.ScrollViewer.PanningRatio" /> property.</param>
		/// <param name="value">The ratio of scrolling offset to translate manipulation offset.</param>
		// Token: 0x0600553E RID: 21822 RVA: 0x00178DAA File Offset: 0x00176FAA
		public static void SetPanningRatio(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ScrollViewer.PanningRatioProperty, value);
		}

		/// <summary>Returns the value of the <see cref="P:System.Windows.Controls.ScrollViewer.PanningRatio" /> property for the specified object.</summary>
		/// <param name="element">The element from which the property value is read.</param>
		/// <returns>The ratio of scrolling offset to translate manipulation offset. </returns>
		// Token: 0x0600553F RID: 21823 RVA: 0x00178DCB File Offset: 0x00176FCB
		public static double GetPanningRatio(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(ScrollViewer.PanningRatioProperty);
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x00178DEC File Offset: 0x00176FEC
		private static bool CheckFiniteNonNegative(object value)
		{
			double num = (double)value;
			return DoubleUtil.GreaterThanOrClose(num, 0.0) && !double.IsInfinity(num);
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.ManipulationStarting" /> event occurs.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005541 RID: 21825 RVA: 0x00178E1C File Offset: 0x0017701C
		protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
		{
			this._panningInfo = null;
			this._seenTapGesture = false;
			PanningMode panningMode = this.PanningMode;
			if (panningMode != PanningMode.None)
			{
				this.CompleteScrollManipulation = false;
				ScrollContentPresenter scrollContentPresenter = base.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (this.ShouldManipulateScroll(e, scrollContentPresenter))
				{
					if (panningMode == PanningMode.HorizontalOnly)
					{
						e.Mode = ManipulationModes.TranslateX;
					}
					else if (panningMode == PanningMode.VerticalOnly)
					{
						e.Mode = ManipulationModes.TranslateY;
					}
					else
					{
						e.Mode = ManipulationModes.Translate;
					}
					e.ManipulationContainer = this;
					this._panningInfo = new ScrollViewer.PanningInfo
					{
						OriginalHorizontalOffset = this.HorizontalOffset,
						OriginalVerticalOffset = this.VerticalOffset,
						PanningMode = panningMode
					};
					double num = this.ViewportWidth + 1.0;
					double num2 = this.ViewportHeight + 1.0;
					if (scrollContentPresenter != null)
					{
						this._panningInfo.DeltaPerHorizontalOffet = (DoubleUtil.AreClose(num, 0.0) ? 0.0 : (scrollContentPresenter.ActualWidth / num));
						this._panningInfo.DeltaPerVerticalOffset = (DoubleUtil.AreClose(num2, 0.0) ? 0.0 : (scrollContentPresenter.ActualHeight / num2));
					}
					else
					{
						this._panningInfo.DeltaPerHorizontalOffet = (DoubleUtil.AreClose(num, 0.0) ? 0.0 : (base.ActualWidth / num));
						this._panningInfo.DeltaPerVerticalOffset = (DoubleUtil.AreClose(num2, 0.0) ? 0.0 : (base.ActualHeight / num2));
					}
					if (!this.ManipulationBindingsInitialized)
					{
						this.BindToTemplatedParent(ScrollViewer.PanningDecelerationProperty);
						this.BindToTemplatedParent(ScrollViewer.PanningRatioProperty);
						this.ManipulationBindingsInitialized = true;
					}
				}
				else
				{
					e.Cancel();
					this.ForceNextManipulationComplete = false;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x00178FDC File Offset: 0x001771DC
		private bool ShouldManipulateScroll(ManipulationStartingEventArgs e, ScrollContentPresenter viewport)
		{
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return false;
			}
			if (viewport == null)
			{
				return true;
			}
			GeneralTransform generalTransform = base.TransformToDescendant(viewport);
			double actualWidth = viewport.ActualWidth;
			double actualHeight = viewport.ActualHeight;
			foreach (IManipulator manipulator in e.Manipulators)
			{
				Point point = generalTransform.Transform(manipulator.GetPosition(this));
				if (DoubleUtil.LessThan(point.X, 0.0) || DoubleUtil.LessThan(point.Y, 0.0) || DoubleUtil.GreaterThan(point.X, actualWidth) || DoubleUtil.GreaterThan(point.Y, actualHeight))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.ManipulationDelta" /> event occurs.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005543 RID: 21827 RVA: 0x001790C4 File Offset: 0x001772C4
		protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (e.IsInertial && this.CompleteScrollManipulation)
				{
					e.Complete();
				}
				else
				{
					bool flag = false;
					if (this._seenTapGesture)
					{
						e.Cancel();
						this._panningInfo = null;
					}
					else if (this._panningInfo.IsPanning)
					{
						this.ManipulateScroll(e);
					}
					else if (this.CanStartScrollManipulation(e.CumulativeManipulation.Translation, out flag))
					{
						this._panningInfo.IsPanning = true;
						this.ManipulateScroll(e);
					}
					else if (flag)
					{
						e.Cancel();
						this._panningInfo = null;
					}
				}
				e.Handled = true;
			}
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x00179168 File Offset: 0x00177368
		private void ManipulateScroll(ManipulationDeltaEventArgs e)
		{
			PanningMode panningMode = this._panningInfo.PanningMode;
			if (panningMode != PanningMode.VerticalOnly)
			{
				this.ManipulateScroll(e.DeltaManipulation.Translation.X, e.CumulativeManipulation.Translation.X, true);
			}
			if (panningMode != PanningMode.HorizontalOnly)
			{
				this.ManipulateScroll(e.DeltaManipulation.Translation.Y, e.CumulativeManipulation.Translation.Y, false);
			}
			if (e.IsInertial && this.IsPastInertialLimit())
			{
				e.Complete();
				return;
			}
			double num = this._panningInfo.UnusedTranslation.X;
			if (!this._panningInfo.InHorizontalFeedback && DoubleUtil.LessThan(Math.Abs(num), 8.0))
			{
				num = 0.0;
			}
			this._panningInfo.InHorizontalFeedback = !DoubleUtil.AreClose(num, 0.0);
			double num2 = this._panningInfo.UnusedTranslation.Y;
			if (!this._panningInfo.InVerticalFeedback && DoubleUtil.LessThan(Math.Abs(num2), 5.0))
			{
				num2 = 0.0;
			}
			this._panningInfo.InVerticalFeedback = !DoubleUtil.AreClose(num2, 0.0);
			if (this._panningInfo.InHorizontalFeedback || this._panningInfo.InVerticalFeedback)
			{
				e.ReportBoundaryFeedback(new ManipulationDelta(new Vector(num, num2), 0.0, new Vector(1.0, 1.0), default(Vector)));
				if (e.IsInertial && this._panningInfo.InertiaBoundaryBeginTimestamp == 0)
				{
					this._panningInfo.InertiaBoundaryBeginTimestamp = Environment.TickCount;
				}
			}
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00179330 File Offset: 0x00177530
		private void ManipulateScroll(double delta, double cumulativeTranslation, bool isHorizontal)
		{
			double num = isHorizontal ? this._panningInfo.UnusedTranslation.X : this._panningInfo.UnusedTranslation.Y;
			double value = isHorizontal ? this.HorizontalOffset : this.VerticalOffset;
			double num2 = isHorizontal ? this.ScrollableWidth : this.ScrollableHeight;
			if (DoubleUtil.AreClose(num2, 0.0))
			{
				num = 0.0;
				delta = 0.0;
			}
			else if ((DoubleUtil.GreaterThan(delta, 0.0) && DoubleUtil.AreClose(value, 0.0)) || (DoubleUtil.LessThan(delta, 0.0) && DoubleUtil.AreClose(value, num2)))
			{
				num += delta;
				delta = 0.0;
			}
			else if (DoubleUtil.LessThan(delta, 0.0) && DoubleUtil.GreaterThan(num, 0.0))
			{
				double num3 = Math.Max(num + delta, 0.0);
				delta += num - num3;
				num = num3;
			}
			else if (DoubleUtil.GreaterThan(delta, 0.0) && DoubleUtil.LessThan(num, 0.0))
			{
				double num4 = Math.Min(num + delta, 0.0);
				delta += num - num4;
				num = num4;
			}
			if (isHorizontal)
			{
				if (!DoubleUtil.AreClose(delta, 0.0))
				{
					this.ScrollToHorizontalOffset(this._panningInfo.OriginalHorizontalOffset - Math.Round(this.PanningRatio * cumulativeTranslation / this._panningInfo.DeltaPerHorizontalOffet));
				}
				this._panningInfo.UnusedTranslation = new Vector(num, this._panningInfo.UnusedTranslation.Y);
				return;
			}
			if (!DoubleUtil.AreClose(delta, 0.0))
			{
				this.ScrollToVerticalOffset(this._panningInfo.OriginalVerticalOffset - Math.Round(this.PanningRatio * cumulativeTranslation / this._panningInfo.DeltaPerVerticalOffset));
			}
			this._panningInfo.UnusedTranslation = new Vector(this._panningInfo.UnusedTranslation.X, num);
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00179550 File Offset: 0x00177750
		private bool IsPastInertialLimit()
		{
			return Math.Abs(Environment.TickCount - this._panningInfo.InertiaBoundaryBeginTimestamp) >= 100 && (DoubleUtil.GreaterThanOrClose(Math.Abs(this._panningInfo.UnusedTranslation.X), 50.0) || DoubleUtil.GreaterThanOrClose(Math.Abs(this._panningInfo.UnusedTranslation.Y), 50.0));
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x001795CC File Offset: 0x001777CC
		private bool CanStartScrollManipulation(Vector translation, out bool cancelManipulation)
		{
			cancelManipulation = false;
			PanningMode panningMode = this._panningInfo.PanningMode;
			if (panningMode == PanningMode.None)
			{
				cancelManipulation = true;
				return false;
			}
			bool flag = DoubleUtil.GreaterThan(Math.Abs(translation.X), 3.0);
			bool flag2 = DoubleUtil.GreaterThan(Math.Abs(translation.Y), 3.0);
			if ((panningMode == PanningMode.Both && (flag || flag2)) || (panningMode == PanningMode.HorizontalOnly && flag) || (panningMode == PanningMode.VerticalOnly && flag2))
			{
				return true;
			}
			if (panningMode == PanningMode.HorizontalFirst)
			{
				bool flag3 = DoubleUtil.GreaterThanOrClose(Math.Abs(translation.X), Math.Abs(translation.Y));
				if (flag && flag3)
				{
					return true;
				}
				if (flag2)
				{
					cancelManipulation = true;
					return false;
				}
			}
			else if (panningMode == PanningMode.VerticalFirst)
			{
				bool flag4 = DoubleUtil.GreaterThanOrClose(Math.Abs(translation.Y), Math.Abs(translation.X));
				if (flag2 && flag4)
				{
					return true;
				}
				if (flag)
				{
					cancelManipulation = true;
					return false;
				}
			}
			return false;
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.ManipulationInertiaStarting" /> event occurs.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005548 RID: 21832 RVA: 0x001796A8 File Offset: 0x001778A8
		protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (!this._panningInfo.IsPanning && !this.ForceNextManipulationComplete)
				{
					e.Cancel();
					this._panningInfo = null;
				}
				else
				{
					e.TranslationBehavior.DesiredDeceleration = this.PanningDeceleration;
				}
				e.Handled = true;
			}
		}

		/// <summary>Called when the <see cref="E:System.Windows.UIElement.ManipulationCompleted" /> event occurs.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005549 RID: 21833 RVA: 0x001796FC File Offset: 0x001778FC
		protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
		{
			if (this._panningInfo != null)
			{
				if (!e.IsInertial || !this.CompleteScrollManipulation)
				{
					if (e.IsInertial && !DoubleUtil.AreClose(e.FinalVelocities.LinearVelocity, default(Vector)) && !this.IsPastInertialLimit())
					{
						this.ForceNextManipulationComplete = true;
					}
					else
					{
						if (!e.IsInertial && !this._panningInfo.IsPanning && !this.ForceNextManipulationComplete)
						{
							e.Cancel();
						}
						this.ForceNextManipulationComplete = false;
					}
				}
				this._panningInfo = null;
				this.CompleteScrollManipulation = false;
				e.Handled = true;
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x00179798 File Offset: 0x00177998
		// (set) Token: 0x0600554B RID: 21835 RVA: 0x001797A5 File Offset: 0x001779A5
		internal bool HandlesMouseWheelScrolling
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.HandlesMouseWheelScrolling) == ScrollViewer.Flags.HandlesMouseWheelScrolling;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.HandlesMouseWheelScrolling, value);
			}
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x001797AF File Offset: 0x001779AF
		// (set) Token: 0x0600554D RID: 21837 RVA: 0x001797BC File Offset: 0x001779BC
		internal bool InChildInvalidateMeasure
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildInvalidateMeasure) == ScrollViewer.Flags.InChildInvalidateMeasure;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildInvalidateMeasure, value);
			}
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x001797C8 File Offset: 0x001779C8
		private bool ExecuteNextCommand()
		{
			IScrollInfo scrollInfo = this.ScrollInfo;
			if (scrollInfo == null)
			{
				return false;
			}
			ScrollViewer.Command command = this._queue.Fetch();
			switch (command.Code)
			{
			case ScrollViewer.Commands.Invalid:
				return false;
			case ScrollViewer.Commands.LineUp:
				scrollInfo.LineUp();
				break;
			case ScrollViewer.Commands.LineDown:
				scrollInfo.LineDown();
				break;
			case ScrollViewer.Commands.LineLeft:
				scrollInfo.LineLeft();
				break;
			case ScrollViewer.Commands.LineRight:
				scrollInfo.LineRight();
				break;
			case ScrollViewer.Commands.PageUp:
				scrollInfo.PageUp();
				break;
			case ScrollViewer.Commands.PageDown:
				scrollInfo.PageDown();
				break;
			case ScrollViewer.Commands.PageLeft:
				scrollInfo.PageLeft();
				break;
			case ScrollViewer.Commands.PageRight:
				scrollInfo.PageRight();
				break;
			case ScrollViewer.Commands.SetHorizontalOffset:
				scrollInfo.SetHorizontalOffset(command.Param);
				break;
			case ScrollViewer.Commands.SetVerticalOffset:
				scrollInfo.SetVerticalOffset(command.Param);
				break;
			case ScrollViewer.Commands.MakeVisible:
			{
				Visual child = command.MakeVisibleParam.Child;
				Visual visual = scrollInfo as Visual;
				if (child != null && visual != null && (visual == child || visual.IsAncestorOf(child)) && base.IsAncestorOf(visual))
				{
					Rect rectangle = command.MakeVisibleParam.TargetRect;
					if (rectangle.IsEmpty)
					{
						UIElement uielement = child as UIElement;
						if (uielement != null)
						{
							rectangle = new Rect(uielement.RenderSize);
						}
						else
						{
							rectangle = default(Rect);
						}
					}
					Rect rect;
					if (scrollInfo.GetType() == typeof(ScrollContentPresenter))
					{
						rect = ((ScrollContentPresenter)scrollInfo).MakeVisible(child, rectangle, false);
					}
					else
					{
						rect = scrollInfo.MakeVisible(child, rectangle);
					}
					if (!rect.IsEmpty)
					{
						GeneralTransform generalTransform = visual.TransformToAncestor(this);
						rect = generalTransform.TransformBounds(rect);
					}
					base.BringIntoView(rect);
				}
				break;
			}
			}
			return true;
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x00179983 File Offset: 0x00177B83
		private void EnqueueCommand(ScrollViewer.Commands code, double param, ScrollViewer.MakeVisibleParams mvp)
		{
			this._queue.Enqueue(new ScrollViewer.Command(code, param, mvp));
			this.EnsureQueueProcessing();
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x0017999E File Offset: 0x00177B9E
		private void EnsureQueueProcessing()
		{
			if (!this._queue.IsEmpty())
			{
				this.EnsureLayoutUpdatedHandler();
			}
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x001799B4 File Offset: 0x00177BB4
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			if (this.ExecuteNextCommand())
			{
				base.InvalidateArrange();
				return;
			}
			double horizontalOffset = this.HorizontalOffset;
			double verticalOffset = this.VerticalOffset;
			double viewportWidth = this.ViewportWidth;
			double viewportHeight = this.ViewportHeight;
			double extentWidth = this.ExtentWidth;
			double extentHeight = this.ExtentHeight;
			double scrollableWidth = this.ScrollableWidth;
			double scrollableHeight = this.ScrollableHeight;
			bool flag = false;
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(horizontalOffset, this.ScrollInfo.HorizontalOffset))
			{
				this._xPositionISI = this.ScrollInfo.HorizontalOffset;
				this.HorizontalOffset = this._xPositionISI;
				this.ContentHorizontalOffset = this._xPositionISI;
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(verticalOffset, this.ScrollInfo.VerticalOffset))
			{
				this._yPositionISI = this.ScrollInfo.VerticalOffset;
				this.VerticalOffset = this._yPositionISI;
				this.ContentVerticalOffset = this._yPositionISI;
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(viewportWidth, this.ScrollInfo.ViewportWidth))
			{
				this._xSize = this.ScrollInfo.ViewportWidth;
				base.SetValue(ScrollViewer.ViewportWidthPropertyKey, this._xSize);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(viewportHeight, this.ScrollInfo.ViewportHeight))
			{
				this._ySize = this.ScrollInfo.ViewportHeight;
				base.SetValue(ScrollViewer.ViewportHeightPropertyKey, this._ySize);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(extentWidth, this.ScrollInfo.ExtentWidth))
			{
				this._xExtent = this.ScrollInfo.ExtentWidth;
				base.SetValue(ScrollViewer.ExtentWidthPropertyKey, this._xExtent);
				flag = true;
			}
			if (this.ScrollInfo != null && !DoubleUtil.AreClose(extentHeight, this.ScrollInfo.ExtentHeight))
			{
				this._yExtent = this.ScrollInfo.ExtentHeight;
				base.SetValue(ScrollViewer.ExtentHeightPropertyKey, this._yExtent);
				flag = true;
			}
			double scrollableWidth2 = this.ScrollableWidth;
			if (!DoubleUtil.AreClose(scrollableWidth, this.ScrollableWidth))
			{
				base.SetValue(ScrollViewer.ScrollableWidthPropertyKey, scrollableWidth2);
				flag = true;
			}
			double scrollableHeight2 = this.ScrollableHeight;
			if (!DoubleUtil.AreClose(scrollableHeight, this.ScrollableHeight))
			{
				base.SetValue(ScrollViewer.ScrollableHeightPropertyKey, scrollableHeight2);
				flag = true;
			}
			if (flag)
			{
				ScrollChangedEventArgs scrollChangedEventArgs = new ScrollChangedEventArgs(new Vector(this.HorizontalOffset, this.VerticalOffset), new Vector(this.HorizontalOffset - horizontalOffset, this.VerticalOffset - verticalOffset), new Size(this.ExtentWidth, this.ExtentHeight), new Vector(this.ExtentWidth - extentWidth, this.ExtentHeight - extentHeight), new Size(this.ViewportWidth, this.ViewportHeight), new Vector(this.ViewportWidth - viewportWidth, this.ViewportHeight - viewportHeight));
				scrollChangedEventArgs.RoutedEvent = ScrollViewer.ScrollChangedEvent;
				scrollChangedEventArgs.Source = this;
				try
				{
					this.OnScrollChanged(scrollChangedEventArgs);
					ScrollViewerAutomationPeer scrollViewerAutomationPeer = UIElementAutomationPeer.FromElement(this) as ScrollViewerAutomationPeer;
					if (scrollViewerAutomationPeer != null)
					{
						scrollViewerAutomationPeer.RaiseAutomationEvents(extentWidth, extentHeight, viewportWidth, viewportHeight, horizontalOffset, verticalOffset);
					}
				}
				finally
				{
					this.ClearLayoutUpdatedHandler();
				}
			}
			this.ClearLayoutUpdatedHandler();
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this control, as part of the Windows Presentation Foundation (WPF) automation infrastructure.</summary>
		/// <returns>The appropriate <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this control.</returns>
		// Token: 0x06005552 RID: 21842 RVA: 0x00179CE8 File Offset: 0x00177EE8
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ScrollViewerAutomationPeer(this);
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x00179CF0 File Offset: 0x00177EF0
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;
			Visual visual = e.TargetObject as Visual;
			if (visual != null)
			{
				if (visual != scrollViewer && visual.IsDescendantOf(scrollViewer))
				{
					e.Handled = true;
					scrollViewer.MakeVisible(visual, e.TargetRect);
					return;
				}
			}
			else
			{
				ContentElement contentElement = e.TargetObject as ContentElement;
				if (contentElement != null)
				{
					IContentHost contentHost = ContentHostHelper.FindContentHost(contentElement);
					visual = (contentHost as Visual);
					if (visual != null && visual.IsDescendantOf(scrollViewer))
					{
						ReadOnlyCollection<Rect> rectangles = contentHost.GetRectangles(contentElement);
						if (rectangles.Count > 0)
						{
							e.Handled = true;
							scrollViewer.MakeVisible(visual, rectangles[0]);
						}
					}
				}
			}
		}

		// Token: 0x06005554 RID: 21844 RVA: 0x00179D88 File Offset: 0x00177F88
		private static void OnScrollCommand(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Command == ScrollBar.DeferScrollToHorizontalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).DeferScrollToHorizontalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.DeferScrollToVerticalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).DeferScrollToVerticalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.LineLeftCommand)
			{
				((ScrollViewer)target).LineLeft();
			}
			else if (args.Command == ScrollBar.LineRightCommand)
			{
				((ScrollViewer)target).LineRight();
			}
			else if (args.Command == ScrollBar.PageLeftCommand)
			{
				((ScrollViewer)target).PageLeft();
			}
			else if (args.Command == ScrollBar.PageRightCommand)
			{
				((ScrollViewer)target).PageRight();
			}
			else if (args.Command == ScrollBar.LineUpCommand)
			{
				((ScrollViewer)target).LineUp();
			}
			else if (args.Command == ScrollBar.LineDownCommand)
			{
				((ScrollViewer)target).LineDown();
			}
			else if (args.Command == ScrollBar.PageUpCommand || args.Command == ComponentCommands.ScrollPageUp)
			{
				((ScrollViewer)target).PageUp();
			}
			else if (args.Command == ScrollBar.PageDownCommand || args.Command == ComponentCommands.ScrollPageDown)
			{
				((ScrollViewer)target).PageDown();
			}
			else if (args.Command == ScrollBar.ScrollToEndCommand)
			{
				((ScrollViewer)target).ScrollToEnd();
			}
			else if (args.Command == ScrollBar.ScrollToHomeCommand)
			{
				((ScrollViewer)target).ScrollToHome();
			}
			else if (args.Command == ScrollBar.ScrollToLeftEndCommand)
			{
				((ScrollViewer)target).ScrollToLeftEnd();
			}
			else if (args.Command == ScrollBar.ScrollToRightEndCommand)
			{
				((ScrollViewer)target).ScrollToRightEnd();
			}
			else if (args.Command == ScrollBar.ScrollToTopCommand)
			{
				((ScrollViewer)target).ScrollToTop();
			}
			else if (args.Command == ScrollBar.ScrollToBottomCommand)
			{
				((ScrollViewer)target).ScrollToBottom();
			}
			else if (args.Command == ScrollBar.ScrollToHorizontalOffsetCommand)
			{
				if (args.Parameter is double)
				{
					((ScrollViewer)target).ScrollToHorizontalOffset((double)args.Parameter);
				}
			}
			else if (args.Command == ScrollBar.ScrollToVerticalOffsetCommand && args.Parameter is double)
			{
				((ScrollViewer)target).ScrollToVerticalOffset((double)args.Parameter);
			}
			ScrollViewer scrollViewer = target as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.CompleteScrollManipulation = true;
			}
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x0017A024 File Offset: 0x00178224
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
			if (args.Command == ComponentCommands.ScrollPageUp || args.Command == ComponentCommands.ScrollPageDown)
			{
				ScrollViewer scrollViewer = target as ScrollViewer;
				Control control = (scrollViewer != null) ? (scrollViewer.TemplatedParent as Control) : null;
				if (control != null && control.HandlesScrolling)
				{
					args.CanExecute = false;
					args.ContinueRouting = true;
					args.Handled = true;
					return;
				}
			}
			else if (args.Command == ScrollBar.DeferScrollToHorizontalOffsetCommand || args.Command == ScrollBar.DeferScrollToVerticalOffsetCommand)
			{
				ScrollViewer scrollViewer2 = target as ScrollViewer;
				if (scrollViewer2 != null && !scrollViewer2.IsDeferredScrollingEnabled)
				{
					args.CanExecute = false;
					args.Handled = true;
				}
			}
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x0017A0C8 File Offset: 0x001782C8
		private static void InitializeCommands()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(ScrollViewer.OnScrollCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(ScrollViewer.OnQueryScrollCommand);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.LineDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.PageDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToLeftEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToRightEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToHomeCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToTopCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToBottomCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToHorizontalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.ScrollToVerticalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.DeferScrollToHorizontalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ScrollBar.DeferScrollToVerticalOffsetCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ComponentCommands.ScrollPageUp, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollViewer), ComponentCommands.ScrollPageDown, executedRoutedEventHandler, canExecuteRoutedEventHandler);
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x0017A2A8 File Offset: 0x001784A8
		private static ControlTemplate CreateDefaultControlTemplate()
		{
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Grid), "Grid");
			FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(ColumnDefinition), "ColumnDefinitionOne");
			FrameworkElementFactory frameworkElementFactory3 = new FrameworkElementFactory(typeof(ColumnDefinition), "ColumnDefinitionTwo");
			FrameworkElementFactory frameworkElementFactory4 = new FrameworkElementFactory(typeof(RowDefinition), "RowDefinitionOne");
			FrameworkElementFactory frameworkElementFactory5 = new FrameworkElementFactory(typeof(RowDefinition), "RowDefinitionTwo");
			FrameworkElementFactory frameworkElementFactory6 = new FrameworkElementFactory(typeof(ScrollBar), "PART_VerticalScrollBar");
			FrameworkElementFactory frameworkElementFactory7 = new FrameworkElementFactory(typeof(ScrollBar), "PART_HorizontalScrollBar");
			FrameworkElementFactory frameworkElementFactory8 = new FrameworkElementFactory(typeof(ScrollContentPresenter), "PART_ScrollContentPresenter");
			FrameworkElementFactory frameworkElementFactory9 = new FrameworkElementFactory(typeof(Rectangle), "Corner");
			Binding binding = new Binding("HorizontalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.RelativeSource = RelativeSource.TemplatedParent;
			Binding binding2 = new Binding("VerticalOffset");
			binding2.Mode = BindingMode.OneWay;
			binding2.RelativeSource = RelativeSource.TemplatedParent;
			frameworkElementFactory.SetValue(Panel.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
			frameworkElementFactory.AppendChild(frameworkElementFactory2);
			frameworkElementFactory.AppendChild(frameworkElementFactory3);
			frameworkElementFactory.AppendChild(frameworkElementFactory4);
			frameworkElementFactory.AppendChild(frameworkElementFactory5);
			frameworkElementFactory.AppendChild(frameworkElementFactory9);
			frameworkElementFactory.AppendChild(frameworkElementFactory8);
			frameworkElementFactory.AppendChild(frameworkElementFactory6);
			frameworkElementFactory.AppendChild(frameworkElementFactory7);
			frameworkElementFactory2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Star));
			frameworkElementFactory3.SetValue(ColumnDefinition.WidthProperty, new GridLength(1.0, GridUnitType.Auto));
			frameworkElementFactory4.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Star));
			frameworkElementFactory5.SetValue(RowDefinition.HeightProperty, new GridLength(1.0, GridUnitType.Auto));
			frameworkElementFactory8.SetValue(Grid.ColumnProperty, 0);
			frameworkElementFactory8.SetValue(Grid.RowProperty, 0);
			frameworkElementFactory8.SetValue(FrameworkElement.MarginProperty, new TemplateBindingExtension(Control.PaddingProperty));
			frameworkElementFactory8.SetValue(ContentControl.ContentProperty, new TemplateBindingExtension(ContentControl.ContentProperty));
			frameworkElementFactory8.SetValue(ContentControl.ContentTemplateProperty, new TemplateBindingExtension(ContentControl.ContentTemplateProperty));
			frameworkElementFactory8.SetValue(ScrollViewer.CanContentScrollProperty, new TemplateBindingExtension(ScrollViewer.CanContentScrollProperty));
			frameworkElementFactory7.SetValue(ScrollBar.OrientationProperty, Orientation.Horizontal);
			frameworkElementFactory7.SetValue(Grid.ColumnProperty, 0);
			frameworkElementFactory7.SetValue(Grid.RowProperty, 1);
			frameworkElementFactory7.SetValue(RangeBase.MinimumProperty, 0.0);
			frameworkElementFactory7.SetValue(RangeBase.MaximumProperty, new TemplateBindingExtension(ScrollViewer.ScrollableWidthProperty));
			frameworkElementFactory7.SetValue(ScrollBar.ViewportSizeProperty, new TemplateBindingExtension(ScrollViewer.ViewportWidthProperty));
			frameworkElementFactory7.SetBinding(RangeBase.ValueProperty, binding);
			frameworkElementFactory7.SetValue(UIElement.VisibilityProperty, new TemplateBindingExtension(ScrollViewer.ComputedHorizontalScrollBarVisibilityProperty));
			frameworkElementFactory7.SetValue(FrameworkElement.CursorProperty, Cursors.Arrow);
			frameworkElementFactory7.SetValue(AutomationProperties.AutomationIdProperty, "HorizontalScrollBar");
			frameworkElementFactory6.SetValue(Grid.ColumnProperty, 1);
			frameworkElementFactory6.SetValue(Grid.RowProperty, 0);
			frameworkElementFactory6.SetValue(RangeBase.MinimumProperty, 0.0);
			frameworkElementFactory6.SetValue(RangeBase.MaximumProperty, new TemplateBindingExtension(ScrollViewer.ScrollableHeightProperty));
			frameworkElementFactory6.SetValue(ScrollBar.ViewportSizeProperty, new TemplateBindingExtension(ScrollViewer.ViewportHeightProperty));
			frameworkElementFactory6.SetBinding(RangeBase.ValueProperty, binding2);
			frameworkElementFactory6.SetValue(UIElement.VisibilityProperty, new TemplateBindingExtension(ScrollViewer.ComputedVerticalScrollBarVisibilityProperty));
			frameworkElementFactory6.SetValue(FrameworkElement.CursorProperty, Cursors.Arrow);
			frameworkElementFactory6.SetValue(AutomationProperties.AutomationIdProperty, "VerticalScrollBar");
			frameworkElementFactory9.SetValue(Grid.ColumnProperty, 1);
			frameworkElementFactory9.SetValue(Grid.RowProperty, 1);
			frameworkElementFactory9.SetResourceReference(Shape.FillProperty, SystemColors.ControlBrushKey);
			ControlTemplate controlTemplate = new ControlTemplate(typeof(ScrollViewer));
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			return controlTemplate;
		}

		// Token: 0x06005558 RID: 21848 RVA: 0x0017A6CC File Offset: 0x001788CC
		private void SetFlagValue(ScrollViewer.Flags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x06005559 RID: 21849 RVA: 0x0017A6EF File Offset: 0x001788EF
		// (set) Token: 0x0600555A RID: 21850 RVA: 0x0017A6FC File Offset: 0x001788FC
		private bool InvalidatedMeasureFromArrange
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InvalidatedMeasureFromArrange) == ScrollViewer.Flags.InvalidatedMeasureFromArrange;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InvalidatedMeasureFromArrange, value);
			}
		}

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x0600555B RID: 21851 RVA: 0x0017A706 File Offset: 0x00178906
		// (set) Token: 0x0600555C RID: 21852 RVA: 0x0017A713 File Offset: 0x00178913
		private bool ForceNextManipulationComplete
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.ForceNextManipulationComplete) == ScrollViewer.Flags.ForceNextManipulationComplete;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.ForceNextManipulationComplete, value);
			}
		}

		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x0600555D RID: 21853 RVA: 0x0017A71D File Offset: 0x0017891D
		// (set) Token: 0x0600555E RID: 21854 RVA: 0x0017A72C File Offset: 0x0017892C
		private bool ManipulationBindingsInitialized
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.ManipulationBindingsInitialized) == ScrollViewer.Flags.ManipulationBindingsInitialized;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.ManipulationBindingsInitialized, value);
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x0600555F RID: 21855 RVA: 0x0017A737 File Offset: 0x00178937
		// (set) Token: 0x06005560 RID: 21856 RVA: 0x0017A746 File Offset: 0x00178946
		private bool CompleteScrollManipulation
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.CompleteScrollManipulation) == ScrollViewer.Flags.CompleteScrollManipulation;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.CompleteScrollManipulation, value);
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x06005561 RID: 21857 RVA: 0x0017A751 File Offset: 0x00178951
		// (set) Token: 0x06005562 RID: 21858 RVA: 0x0017A760 File Offset: 0x00178960
		internal bool InChildMeasurePass1
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass1) == ScrollViewer.Flags.InChildMeasurePass1;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass1, value);
			}
		}

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x06005563 RID: 21859 RVA: 0x0017A76B File Offset: 0x0017896B
		// (set) Token: 0x06005564 RID: 21860 RVA: 0x0017A780 File Offset: 0x00178980
		internal bool InChildMeasurePass2
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass2) == ScrollViewer.Flags.InChildMeasurePass2;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass2, value);
			}
		}

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x06005565 RID: 21861 RVA: 0x0017A78E File Offset: 0x0017898E
		// (set) Token: 0x06005566 RID: 21862 RVA: 0x0017A7A3 File Offset: 0x001789A3
		internal bool InChildMeasurePass3
		{
			get
			{
				return (this._flags & ScrollViewer.Flags.InChildMeasurePass3) == ScrollViewer.Flags.InChildMeasurePass3;
			}
			set
			{
				this.SetFlagValue(ScrollViewer.Flags.InChildMeasurePass3, value);
			}
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x0017A7B4 File Offset: 0x001789B4
		static ScrollViewer()
		{
			ScrollViewer.ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Bubble, typeof(ScrollChangedEventHandler), typeof(ScrollViewer));
			ScrollViewer.PanningModeProperty = DependencyProperty.RegisterAttached("PanningMode", typeof(PanningMode), typeof(ScrollViewer), new FrameworkPropertyMetadata(PanningMode.None, new PropertyChangedCallback(ScrollViewer.OnPanningModeChanged)));
			ScrollViewer.PanningDecelerationProperty = DependencyProperty.RegisterAttached("PanningDeceleration", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.001), new ValidateValueCallback(ScrollViewer.CheckFiniteNonNegative));
			ScrollViewer.PanningRatioProperty = DependencyProperty.RegisterAttached("PanningRatio", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(ScrollViewer.CheckFiniteNonNegative));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(typeof(ScrollViewer)));
			ScrollViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ScrollViewer));
			ScrollViewer.InitializeCommands();
			ControlTemplate defaultValue = ScrollViewer.CreateDefaultControlTemplate();
			Control.TemplateProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(defaultValue));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ScrollViewer), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			EventManager.RegisterClassHandler(typeof(ScrollViewer), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(ScrollViewer.OnRequestBringIntoView));
			ControlsTraceLogger.AddControl(TelemetryControls.ScrollViewer);
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x0017AD64 File Offset: 0x00178F64
		private static bool IsValidScrollBarVisibility(object o)
		{
			ScrollBarVisibility scrollBarVisibility = (ScrollBarVisibility)o;
			return scrollBarVisibility == ScrollBarVisibility.Disabled || scrollBarVisibility == ScrollBarVisibility.Auto || scrollBarVisibility == ScrollBarVisibility.Hidden || scrollBarVisibility == ScrollBarVisibility.Visible;
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x06005569 RID: 21865 RVA: 0x000962DF File Offset: 0x000944DF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x0017AD89 File Offset: 0x00178F89
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ScrollViewer._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.CanContentScroll" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.CanContentScroll" /> dependency property.</returns>
		// Token: 0x04002DC2 RID: 11714
		[CommonDependencyProperty]
		public static readonly DependencyProperty CanContentScrollProperty = DependencyProperty.RegisterAttached("CanContentScroll", typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002DC3 RID: 11715
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollViewer.IsValidScrollBarVisibility));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002DC4 RID: 11716
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.RegisterAttached("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(ScrollViewer.IsValidScrollBarVisibility));

		// Token: 0x04002DC5 RID: 11717
		private static readonly DependencyPropertyKey ComputedHorizontalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedHorizontalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(Visibility.Visible));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ComputedHorizontalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ComputedHorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002DC6 RID: 11718
		public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty = ScrollViewer.ComputedHorizontalScrollBarVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04002DC7 RID: 11719
		private static readonly DependencyPropertyKey ComputedVerticalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("ComputedVerticalScrollBarVisibility", typeof(Visibility), typeof(ScrollViewer), new FrameworkPropertyMetadata(Visibility.Visible));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ComputedVerticalScrollBarVisibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ComputedVerticalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002DC8 RID: 11720
		public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty = ScrollViewer.ComputedVerticalScrollBarVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04002DC9 RID: 11721
		private static readonly DependencyPropertyKey VerticalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("VerticalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.VerticalOffset" /> dependency property.</returns>
		// Token: 0x04002DCA RID: 11722
		public static readonly DependencyProperty VerticalOffsetProperty = ScrollViewer.VerticalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04002DCB RID: 11723
		private static readonly DependencyPropertyKey HorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("HorizontalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.HorizontalOffset" /> dependency property.</returns>
		// Token: 0x04002DCC RID: 11724
		public static readonly DependencyProperty HorizontalOffsetProperty = ScrollViewer.HorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04002DCD RID: 11725
		private static readonly DependencyPropertyKey ContentVerticalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("ContentVerticalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ContentVerticalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ContentVerticalOffset" /> dependency property.</returns>
		// Token: 0x04002DCE RID: 11726
		public static readonly DependencyProperty ContentVerticalOffsetProperty = ScrollViewer.ContentVerticalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04002DCF RID: 11727
		private static readonly DependencyPropertyKey ContentHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("ContentHorizontalOffset", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ContentHorizontalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ContentHorizontalOffset" /> dependency property.</returns>
		// Token: 0x04002DD0 RID: 11728
		public static readonly DependencyProperty ContentHorizontalOffsetProperty = ScrollViewer.ContentHorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04002DD1 RID: 11729
		private static readonly DependencyPropertyKey ExtentWidthPropertyKey = DependencyProperty.RegisterReadOnly("ExtentWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ExtentWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ExtentWidth" /> dependency property.</returns>
		// Token: 0x04002DD2 RID: 11730
		public static readonly DependencyProperty ExtentWidthProperty = ScrollViewer.ExtentWidthPropertyKey.DependencyProperty;

		// Token: 0x04002DD3 RID: 11731
		private static readonly DependencyPropertyKey ExtentHeightPropertyKey = DependencyProperty.RegisterReadOnly("ExtentHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ExtentHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ExtentHeight" /> dependency property.</returns>
		// Token: 0x04002DD4 RID: 11732
		public static readonly DependencyProperty ExtentHeightProperty = ScrollViewer.ExtentHeightPropertyKey.DependencyProperty;

		// Token: 0x04002DD5 RID: 11733
		private static readonly DependencyPropertyKey ScrollableWidthPropertyKey = DependencyProperty.RegisterReadOnly("ScrollableWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ScrollableWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ScrollableWidth" /> dependency property.</returns>
		// Token: 0x04002DD6 RID: 11734
		public static readonly DependencyProperty ScrollableWidthProperty = ScrollViewer.ScrollableWidthPropertyKey.DependencyProperty;

		// Token: 0x04002DD7 RID: 11735
		private static readonly DependencyPropertyKey ScrollableHeightPropertyKey = DependencyProperty.RegisterReadOnly("ScrollableHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ScrollableHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ScrollableHeight" /> dependency property.</returns>
		// Token: 0x04002DD8 RID: 11736
		public static readonly DependencyProperty ScrollableHeightProperty = ScrollViewer.ScrollableHeightPropertyKey.DependencyProperty;

		// Token: 0x04002DD9 RID: 11737
		private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterReadOnly("ViewportWidth", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ViewportWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ViewportWidth" /> dependency property.</returns>
		// Token: 0x04002DDA RID: 11738
		public static readonly DependencyProperty ViewportWidthProperty = ScrollViewer.ViewportWidthPropertyKey.DependencyProperty;

		// Token: 0x04002DDB RID: 11739
		internal static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterReadOnly("ViewportHeight", typeof(double), typeof(ScrollViewer), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.ViewportHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.ViewportHeight" /> dependency property.</returns>
		// Token: 0x04002DDC RID: 11740
		public static readonly DependencyProperty ViewportHeightProperty = ScrollViewer.ViewportHeightPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabled" /> dependency property.</returns>
		// Token: 0x04002DDD RID: 11741
		public static readonly DependencyProperty IsDeferredScrollingEnabledProperty = DependencyProperty.RegisterAttached("IsDeferredScrollingEnabled", typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.PanningMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.PanningMode" /> dependency property.</returns>
		// Token: 0x04002DDF RID: 11743
		public static readonly DependencyProperty PanningModeProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.PanningDeceleration" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.PanningDeceleration" /> dependency property.</returns>
		// Token: 0x04002DE0 RID: 11744
		public static readonly DependencyProperty PanningDecelerationProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollViewer.PanningRatio" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollViewer.PanningRatio" /> dependency property.</returns>
		// Token: 0x04002DE1 RID: 11745
		public static readonly DependencyProperty PanningRatioProperty;

		// Token: 0x04002DE2 RID: 11746
		private bool _seenTapGesture;

		// Token: 0x04002DE3 RID: 11747
		internal const double _scrollLineDelta = 16.0;

		// Token: 0x04002DE4 RID: 11748
		internal const double _mouseWheelDelta = 48.0;

		// Token: 0x04002DE5 RID: 11749
		private const string HorizontalScrollBarTemplateName = "PART_HorizontalScrollBar";

		// Token: 0x04002DE6 RID: 11750
		private const string VerticalScrollBarTemplateName = "PART_VerticalScrollBar";

		// Token: 0x04002DE7 RID: 11751
		internal const string ScrollContentPresenterTemplateName = "PART_ScrollContentPresenter";

		// Token: 0x04002DE8 RID: 11752
		private Visibility _scrollVisibilityX;

		// Token: 0x04002DE9 RID: 11753
		private Visibility _scrollVisibilityY;

		// Token: 0x04002DEA RID: 11754
		private double _xPositionISI;

		// Token: 0x04002DEB RID: 11755
		private double _yPositionISI;

		// Token: 0x04002DEC RID: 11756
		private double _xExtent;

		// Token: 0x04002DED RID: 11757
		private double _yExtent;

		// Token: 0x04002DEE RID: 11758
		private double _xSize;

		// Token: 0x04002DEF RID: 11759
		private double _ySize;

		// Token: 0x04002DF0 RID: 11760
		private EventHandler _layoutUpdatedHandler;

		// Token: 0x04002DF1 RID: 11761
		private IScrollInfo _scrollInfo;

		// Token: 0x04002DF2 RID: 11762
		private ScrollViewer.CommandQueue _queue;

		// Token: 0x04002DF3 RID: 11763
		private ScrollViewer.PanningInfo _panningInfo;

		// Token: 0x04002DF4 RID: 11764
		private ScrollViewer.Flags _flags = ScrollViewer.Flags.HandlesMouseWheelScrolling;

		// Token: 0x04002DF5 RID: 11765
		private static DependencyObjectType _dType;

		// Token: 0x020009B2 RID: 2482
		private class PanningInfo
		{
			// Token: 0x17001EC2 RID: 7874
			// (get) Token: 0x06008850 RID: 34896 RVA: 0x00252011 File Offset: 0x00250211
			// (set) Token: 0x06008851 RID: 34897 RVA: 0x00252019 File Offset: 0x00250219
			public PanningMode PanningMode { get; set; }

			// Token: 0x17001EC3 RID: 7875
			// (get) Token: 0x06008852 RID: 34898 RVA: 0x00252022 File Offset: 0x00250222
			// (set) Token: 0x06008853 RID: 34899 RVA: 0x0025202A File Offset: 0x0025022A
			public double OriginalHorizontalOffset { get; set; }

			// Token: 0x17001EC4 RID: 7876
			// (get) Token: 0x06008854 RID: 34900 RVA: 0x00252033 File Offset: 0x00250233
			// (set) Token: 0x06008855 RID: 34901 RVA: 0x0025203B File Offset: 0x0025023B
			public double OriginalVerticalOffset { get; set; }

			// Token: 0x17001EC5 RID: 7877
			// (get) Token: 0x06008856 RID: 34902 RVA: 0x00252044 File Offset: 0x00250244
			// (set) Token: 0x06008857 RID: 34903 RVA: 0x0025204C File Offset: 0x0025024C
			public double DeltaPerHorizontalOffet { get; set; }

			// Token: 0x17001EC6 RID: 7878
			// (get) Token: 0x06008858 RID: 34904 RVA: 0x00252055 File Offset: 0x00250255
			// (set) Token: 0x06008859 RID: 34905 RVA: 0x0025205D File Offset: 0x0025025D
			public double DeltaPerVerticalOffset { get; set; }

			// Token: 0x17001EC7 RID: 7879
			// (get) Token: 0x0600885A RID: 34906 RVA: 0x00252066 File Offset: 0x00250266
			// (set) Token: 0x0600885B RID: 34907 RVA: 0x0025206E File Offset: 0x0025026E
			public bool IsPanning { get; set; }

			// Token: 0x17001EC8 RID: 7880
			// (get) Token: 0x0600885C RID: 34908 RVA: 0x00252077 File Offset: 0x00250277
			// (set) Token: 0x0600885D RID: 34909 RVA: 0x0025207F File Offset: 0x0025027F
			public Vector UnusedTranslation { get; set; }

			// Token: 0x17001EC9 RID: 7881
			// (get) Token: 0x0600885E RID: 34910 RVA: 0x00252088 File Offset: 0x00250288
			// (set) Token: 0x0600885F RID: 34911 RVA: 0x00252090 File Offset: 0x00250290
			public bool InHorizontalFeedback { get; set; }

			// Token: 0x17001ECA RID: 7882
			// (get) Token: 0x06008860 RID: 34912 RVA: 0x00252099 File Offset: 0x00250299
			// (set) Token: 0x06008861 RID: 34913 RVA: 0x002520A1 File Offset: 0x002502A1
			public bool InVerticalFeedback { get; set; }

			// Token: 0x17001ECB RID: 7883
			// (get) Token: 0x06008862 RID: 34914 RVA: 0x002520AA File Offset: 0x002502AA
			// (set) Token: 0x06008863 RID: 34915 RVA: 0x002520B2 File Offset: 0x002502B2
			public int InertiaBoundaryBeginTimestamp { get; set; }

			// Token: 0x0400452E RID: 17710
			public const double PrePanTranslation = 3.0;

			// Token: 0x0400452F RID: 17711
			public const double MaxInertiaBoundaryTranslation = 50.0;

			// Token: 0x04004530 RID: 17712
			public const double PreFeedbackTranslationX = 8.0;

			// Token: 0x04004531 RID: 17713
			public const double PreFeedbackTranslationY = 5.0;

			// Token: 0x04004532 RID: 17714
			public const int InertiaBoundryMinimumTicks = 100;
		}

		// Token: 0x020009B3 RID: 2483
		private enum Commands
		{
			// Token: 0x04004534 RID: 17716
			Invalid,
			// Token: 0x04004535 RID: 17717
			LineUp,
			// Token: 0x04004536 RID: 17718
			LineDown,
			// Token: 0x04004537 RID: 17719
			LineLeft,
			// Token: 0x04004538 RID: 17720
			LineRight,
			// Token: 0x04004539 RID: 17721
			PageUp,
			// Token: 0x0400453A RID: 17722
			PageDown,
			// Token: 0x0400453B RID: 17723
			PageLeft,
			// Token: 0x0400453C RID: 17724
			PageRight,
			// Token: 0x0400453D RID: 17725
			SetHorizontalOffset,
			// Token: 0x0400453E RID: 17726
			SetVerticalOffset,
			// Token: 0x0400453F RID: 17727
			MakeVisible
		}

		// Token: 0x020009B4 RID: 2484
		private struct Command
		{
			// Token: 0x06008865 RID: 34917 RVA: 0x002520BB File Offset: 0x002502BB
			internal Command(ScrollViewer.Commands code, double param, ScrollViewer.MakeVisibleParams mvp)
			{
				this.Code = code;
				this.Param = param;
				this.MakeVisibleParam = mvp;
			}

			// Token: 0x04004540 RID: 17728
			internal ScrollViewer.Commands Code;

			// Token: 0x04004541 RID: 17729
			internal double Param;

			// Token: 0x04004542 RID: 17730
			internal ScrollViewer.MakeVisibleParams MakeVisibleParam;
		}

		// Token: 0x020009B5 RID: 2485
		private class MakeVisibleParams
		{
			// Token: 0x06008866 RID: 34918 RVA: 0x002520D2 File Offset: 0x002502D2
			internal MakeVisibleParams(Visual child, Rect targetRect)
			{
				this.Child = child;
				this.TargetRect = targetRect;
			}

			// Token: 0x04004543 RID: 17731
			internal Visual Child;

			// Token: 0x04004544 RID: 17732
			internal Rect TargetRect;
		}

		// Token: 0x020009B6 RID: 2486
		private struct CommandQueue
		{
			// Token: 0x06008867 RID: 34919 RVA: 0x002520E8 File Offset: 0x002502E8
			internal void Enqueue(ScrollViewer.Command command)
			{
				if (this._lastWritePosition == this._lastReadPosition)
				{
					this._array = new ScrollViewer.Command[32];
					this._lastWritePosition = (this._lastReadPosition = 0);
				}
				if (!this.OptimizeCommand(command))
				{
					this._lastWritePosition = (this._lastWritePosition + 1) % 32;
					if (this._lastWritePosition == this._lastReadPosition)
					{
						this._lastReadPosition = (this._lastReadPosition + 1) % 32;
					}
					this._array[this._lastWritePosition] = command;
				}
			}

			// Token: 0x06008868 RID: 34920 RVA: 0x0025216C File Offset: 0x0025036C
			private bool OptimizeCommand(ScrollViewer.Command command)
			{
				if (this._lastWritePosition != this._lastReadPosition && ((command.Code == ScrollViewer.Commands.SetHorizontalOffset && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.SetHorizontalOffset) || (command.Code == ScrollViewer.Commands.SetVerticalOffset && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.SetVerticalOffset) || (command.Code == ScrollViewer.Commands.MakeVisible && this._array[this._lastWritePosition].Code == ScrollViewer.Commands.MakeVisible)))
				{
					this._array[this._lastWritePosition].Param = command.Param;
					this._array[this._lastWritePosition].MakeVisibleParam = command.MakeVisibleParam;
					return true;
				}
				return false;
			}

			// Token: 0x06008869 RID: 34921 RVA: 0x00252234 File Offset: 0x00250434
			internal ScrollViewer.Command Fetch()
			{
				if (this._lastWritePosition == this._lastReadPosition)
				{
					return new ScrollViewer.Command(ScrollViewer.Commands.Invalid, 0.0, null);
				}
				this._lastReadPosition = (this._lastReadPosition + 1) % 32;
				ScrollViewer.Command result = this._array[this._lastReadPosition];
				this._array[this._lastReadPosition].MakeVisibleParam = null;
				if (this._lastWritePosition == this._lastReadPosition)
				{
					this._array = null;
				}
				return result;
			}

			// Token: 0x0600886A RID: 34922 RVA: 0x002522B0 File Offset: 0x002504B0
			internal bool IsEmpty()
			{
				return this._lastWritePosition == this._lastReadPosition;
			}

			// Token: 0x04004545 RID: 17733
			private const int _capacity = 32;

			// Token: 0x04004546 RID: 17734
			private int _lastWritePosition;

			// Token: 0x04004547 RID: 17735
			private int _lastReadPosition;

			// Token: 0x04004548 RID: 17736
			private ScrollViewer.Command[] _array;
		}

		// Token: 0x020009B7 RID: 2487
		[Flags]
		private enum Flags
		{
			// Token: 0x0400454A RID: 17738
			None = 0,
			// Token: 0x0400454B RID: 17739
			InvalidatedMeasureFromArrange = 1,
			// Token: 0x0400454C RID: 17740
			InChildInvalidateMeasure = 2,
			// Token: 0x0400454D RID: 17741
			HandlesMouseWheelScrolling = 4,
			// Token: 0x0400454E RID: 17742
			ForceNextManipulationComplete = 8,
			// Token: 0x0400454F RID: 17743
			ManipulationBindingsInitialized = 16,
			// Token: 0x04004550 RID: 17744
			CompleteScrollManipulation = 32,
			// Token: 0x04004551 RID: 17745
			InChildMeasurePass1 = 64,
			// Token: 0x04004552 RID: 17746
			InChildMeasurePass2 = 128,
			// Token: 0x04004553 RID: 17747
			InChildMeasurePass3 = 192
		}
	}
}
