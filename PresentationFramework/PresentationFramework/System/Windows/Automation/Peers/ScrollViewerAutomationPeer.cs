using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ScrollViewer" /> types to UI Automation.</summary>
	// Token: 0x020002DC RID: 732
	public class ScrollViewerAutomationPeer : FrameworkElementAutomationPeer, IScrollProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" />.</param>
		// Token: 0x060027D5 RID: 10197 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ScrollViewerAutomationPeer(ScrollViewer owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ScrollViewer".</returns>
		// Token: 0x060027D6 RID: 10198 RVA: 0x000BAC88 File Offset: 0x000B8E88
		protected override string GetClassNameCore()
		{
			return "ScrollViewer";
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Pane" /> enumeration value.</returns>
		// Token: 0x060027D7 RID: 10199 RVA: 0x00094CE7 File Offset: 0x00092EE7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" /> is understood by the end user as interactive or as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>If the <see cref="P:System.Windows.FrameworkElement.TemplatedParent" /> is <see langword="null" />, this method returns <see langword="true" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027D8 RID: 10200 RVA: 0x000BAC90 File Offset: 0x000B8E90
		protected override bool IsControlElementCore()
		{
			ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
			DependencyObject templatedParent = scrollViewer.TemplatedParent;
			return (templatedParent == null || templatedParent is ContentPresenter) && base.IsControlElementCore();
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.ScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Scroll" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x060027D9 RID: 10201 RVA: 0x000BACC3 File Offset: 0x000B8EC3
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Scroll)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="horizontalAmount"> The horizontal increment specific to the control.</param>
		/// <param name="verticalAmount"> The vertical increment specific to the control.</param>
		// Token: 0x060027DA RID: 10202 RVA: 0x000BACD4 File Offset: 0x000B8ED4
		void IScrollProvider.Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			bool flag = horizontalAmount != ScrollAmount.NoAmount;
			bool flag2 = verticalAmount != ScrollAmount.NoAmount;
			ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
			if ((flag && !this.HorizontallyScrollable) || (flag2 && !this.VerticallyScrollable))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			switch (horizontalAmount)
			{
			case ScrollAmount.LargeDecrement:
				scrollViewer.PageLeft();
				break;
			case ScrollAmount.SmallDecrement:
				scrollViewer.LineLeft();
				break;
			case ScrollAmount.NoAmount:
				break;
			case ScrollAmount.LargeIncrement:
				scrollViewer.PageRight();
				break;
			case ScrollAmount.SmallIncrement:
				scrollViewer.LineRight();
				break;
			default:
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			switch (verticalAmount)
			{
			case ScrollAmount.LargeDecrement:
				scrollViewer.PageUp();
				return;
			case ScrollAmount.SmallDecrement:
				scrollViewer.LineUp();
				return;
			case ScrollAmount.NoAmount:
				return;
			case ScrollAmount.LargeIncrement:
				scrollViewer.PageDown();
				return;
			case ScrollAmount.SmallIncrement:
				scrollViewer.LineDown();
				return;
			default:
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="horizontalPercent"> Percent scrolled horizontally.</param>
		/// <param name="verticalPercent"> Percent scrolled vertically.</param>
		// Token: 0x060027DB RID: 10203 RVA: 0x000BADC8 File Offset: 0x000B8FC8
		void IScrollProvider.SetScrollPercent(double horizontalPercent, double verticalPercent)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			bool flag = horizontalPercent != -1.0;
			bool flag2 = verticalPercent != -1.0;
			ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
			if ((flag && !this.HorizontallyScrollable) || (flag2 && !this.VerticallyScrollable))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			if ((flag && horizontalPercent < 0.0) || horizontalPercent > 100.0)
			{
				throw new ArgumentOutOfRangeException("horizontalPercent", SR.Get("ScrollViewer_OutOfRange", new object[]
				{
					"horizontalPercent",
					horizontalPercent.ToString(CultureInfo.InvariantCulture),
					"0",
					"100"
				}));
			}
			if ((flag2 && verticalPercent < 0.0) || verticalPercent > 100.0)
			{
				throw new ArgumentOutOfRangeException("verticalPercent", SR.Get("ScrollViewer_OutOfRange", new object[]
				{
					"verticalPercent",
					verticalPercent.ToString(CultureInfo.InvariantCulture),
					"0",
					"100"
				}));
			}
			if (flag)
			{
				scrollViewer.ScrollToHorizontalOffset((scrollViewer.ExtentWidth - scrollViewer.ViewportWidth) * horizontalPercent * 0.01);
			}
			if (flag2)
			{
				scrollViewer.ScrollToVerticalOffset((scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) * verticalPercent * 0.01);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The horizontal scroll position as a percentage of the total content area within the control.</returns>
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x000BAF38 File Offset: 0x000B9138
		double IScrollProvider.HorizontalScrollPercent
		{
			get
			{
				if (!this.HorizontallyScrollable)
				{
					return -1.0;
				}
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.HorizontalOffset * 100.0 / (scrollViewer.ExtentWidth - scrollViewer.ViewportWidth);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The vertical scroll position as a percentage of the total content area within the control.</returns>
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x000BAF84 File Offset: 0x000B9184
		double IScrollProvider.VerticalScrollPercent
		{
			get
			{
				if (!this.VerticallyScrollable)
				{
					return -1.0;
				}
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.VerticalOffset * 100.0 / (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Returns <see langword="S_OK" /> if successful, or an error value otherwise.</returns>
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x000BAFD0 File Offset: 0x000B91D0
		double IScrollProvider.HorizontalViewSize
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				if (scrollViewer.ScrollInfo == null || DoubleUtil.IsZero(scrollViewer.ExtentWidth))
				{
					return 100.0;
				}
				return Math.Min(100.0, scrollViewer.ViewportWidth * 100.0 / scrollViewer.ExtentWidth);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Returns <see langword="S_OK" /> if successful, or an error value otherwise.</returns>
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000BB030 File Offset: 0x000B9230
		double IScrollProvider.VerticalViewSize
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				if (scrollViewer.ScrollInfo == null || DoubleUtil.IsZero(scrollViewer.ExtentHeight))
				{
					return 100.0;
				}
				return Math.Min(100.0, scrollViewer.ViewportHeight * 100.0 / scrollViewer.ExtentHeight);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>Value indicating whether control is can be scrolled in the horizontal direction.</returns>
		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060027E0 RID: 10208 RVA: 0x000BB08E File Offset: 0x000B928E
		bool IScrollProvider.HorizontallyScrollable
		{
			get
			{
				return this.HorizontallyScrollable;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the control can scroll vertically; otherwise <see langword="false" />.</returns>
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x000BB096 File Offset: 0x000B9296
		bool IScrollProvider.VerticallyScrollable
		{
			get
			{
				return this.VerticallyScrollable;
			}
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000BB09E File Offset: 0x000B929E
		private static bool AutomationIsScrollable(double extent, double viewport)
		{
			return DoubleUtil.GreaterThan(extent, viewport);
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000BB0A7 File Offset: 0x000B92A7
		private static double AutomationGetScrollPercent(double extent, double viewport, double actualOffset)
		{
			if (!ScrollViewerAutomationPeer.AutomationIsScrollable(extent, viewport))
			{
				return -1.0;
			}
			return actualOffset * 100.0 / (extent - viewport);
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000BB0CC File Offset: 0x000B92CC
		private static double AutomationGetViewSize(double extent, double viewport)
		{
			if (DoubleUtil.IsZero(extent))
			{
				return 100.0;
			}
			return Math.Min(100.0, viewport * 100.0 / extent);
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060027E5 RID: 10213 RVA: 0x000BB0FC File Offset: 0x000B92FC
		private bool HorizontallyScrollable
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.ScrollInfo != null && DoubleUtil.GreaterThan(scrollViewer.ExtentWidth, scrollViewer.ViewportWidth);
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000BB130 File Offset: 0x000B9330
		private bool VerticallyScrollable
		{
			get
			{
				ScrollViewer scrollViewer = (ScrollViewer)base.Owner;
				return scrollViewer.ScrollInfo != null && DoubleUtil.GreaterThan(scrollViewer.ExtentHeight, scrollViewer.ViewportHeight);
			}
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x000BB164 File Offset: 0x000B9364
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationEvents(double extentX, double extentY, double viewportX, double viewportY, double offsetX, double offsetY)
		{
			if (ScrollViewerAutomationPeer.AutomationIsScrollable(extentX, viewportX) != ((IScrollProvider)this).HorizontallyScrollable)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontallyScrollableProperty, ScrollViewerAutomationPeer.AutomationIsScrollable(extentX, viewportX), ((IScrollProvider)this).HorizontallyScrollable);
			}
			if (ScrollViewerAutomationPeer.AutomationIsScrollable(extentY, viewportY) != ((IScrollProvider)this).VerticallyScrollable)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticallyScrollableProperty, ScrollViewerAutomationPeer.AutomationIsScrollable(extentY, viewportY), ((IScrollProvider)this).VerticallyScrollable);
			}
			if (ScrollViewerAutomationPeer.AutomationGetViewSize(extentX, viewportX) != ((IScrollProvider)this).HorizontalViewSize)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontalViewSizeProperty, ScrollViewerAutomationPeer.AutomationGetViewSize(extentX, viewportX), ((IScrollProvider)this).HorizontalViewSize);
			}
			if (ScrollViewerAutomationPeer.AutomationGetViewSize(extentY, viewportY) != ((IScrollProvider)this).VerticalViewSize)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticalViewSizeProperty, ScrollViewerAutomationPeer.AutomationGetViewSize(extentY, viewportY), ((IScrollProvider)this).VerticalViewSize);
			}
			if (ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentX, viewportX, offsetX) != ((IScrollProvider)this).HorizontalScrollPercent)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.HorizontalScrollPercentProperty, ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentX, viewportX, offsetX), ((IScrollProvider)this).HorizontalScrollPercent);
			}
			if (ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentY, viewportY, offsetY) != ((IScrollProvider)this).VerticalScrollPercent)
			{
				base.RaisePropertyChangedEvent(ScrollPatternIdentifiers.VerticalScrollPercentProperty, ScrollViewerAutomationPeer.AutomationGetScrollPercent(extentY, viewportY, offsetY), ((IScrollProvider)this).VerticalScrollPercent);
			}
		}
	}
}
