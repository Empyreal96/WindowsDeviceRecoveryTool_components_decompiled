using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> types to UI Automation.</summary>
	// Token: 0x020002DB RID: 731
	public class ScrollBarAutomationPeer : RangeBaseAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" />.</param>
		// Token: 0x060027CE RID: 10190 RVA: 0x000BA7BB File Offset: 0x000B89BB
		public ScrollBarAutomationPeer(ScrollBar owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains the word "ScrollBar".</returns>
		// Token: 0x060027CF RID: 10191 RVA: 0x000BABB2 File Offset: 0x000B8DB2
		protected override string GetClassNameCore()
		{
			return "ScrollBar";
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ScrollBar" /> enumeration value.</returns>
		// Token: 0x060027D0 RID: 10192 RVA: 0x00095808 File Offset: 0x00093A08
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ScrollBar;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x060027D1 RID: 10193 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}

		/// <summary>Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Point" /> that has values of <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" />; the only clickable points in a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> are the child elements.</returns>
		// Token: 0x060027D2 RID: 10194 RVA: 0x000BABB9 File Offset: 0x000B8DB9
		protected override Point GetClickablePointCore()
		{
			return new Point(double.NaN, double.NaN);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" /> is laid out in a specific direction. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetOrientation" />.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Automation.Peers.AutomationOrientation.Horizontal" /> or <see cref="F:System.Windows.Automation.Peers.AutomationOrientation.Vertical" />, depending on the orientation of the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />.</returns>
		// Token: 0x060027D3 RID: 10195 RVA: 0x000BABD2 File Offset: 0x000B8DD2
		protected override AutomationOrientation GetOrientationCore()
		{
			if (((ScrollBar)base.Owner).Orientation != Orientation.Horizontal)
			{
				return AutomationOrientation.Vertical;
			}
			return AutomationOrientation.Horizontal;
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000BABEC File Offset: 0x000B8DEC
		internal override void SetValueCore(double val)
		{
			double horizontalPercent = -1.0;
			double verticalPercent = -1.0;
			ScrollBar scrollBar = base.Owner as ScrollBar;
			ScrollViewer scrollViewer = scrollBar.TemplatedParent as ScrollViewer;
			if (scrollViewer == null)
			{
				base.SetValueCore(val);
				return;
			}
			if (scrollBar.Orientation == Orientation.Horizontal)
			{
				horizontalPercent = val / (scrollViewer.ExtentWidth - scrollViewer.ViewportWidth) * 100.0;
			}
			else
			{
				verticalPercent = val / (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight) * 100.0;
			}
			ScrollViewerAutomationPeer scrollViewerAutomationPeer = UIElementAutomationPeer.FromElement(scrollViewer) as ScrollViewerAutomationPeer;
			IScrollProvider scrollProvider = scrollViewerAutomationPeer;
			scrollProvider.SetScrollPercent(horizontalPercent, verticalPercent);
		}
	}
}
