using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GridSplitter" /> types to UI Automation.</summary>
	// Token: 0x020002B7 RID: 695
	public class GridSplitterAutomationPeer : ThumbAutomationPeer, ITransformProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GridSplitterAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GridSplitter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridSplitterAutomationPeer" />.</param>
		// Token: 0x060026A0 RID: 9888 RVA: 0x000B7A7D File Offset: 0x000B5C7D
		public GridSplitterAutomationPeer(GridSplitter owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.GridSplitter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridSplitterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "GridSplitter".</returns>
		// Token: 0x060026A1 RID: 9889 RVA: 0x000B7A86 File Offset: 0x000B5C86
		protected override string GetClassNameCore()
		{
			return "GridSplitter";
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Automation.Peers.GridSplitterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />. </summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Transform" />, this method returns a <see langword="this" /> pointer; otherwise this method returns <see langword="null" />.</returns>
		// Token: 0x060026A2 RID: 9890 RVA: 0x000B7A8D File Offset: 0x000B5C8D
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Transform)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can be moved; otherwise <see langword="false" />.</returns>
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060026A3 RID: 9891 RVA: 0x00016748 File Offset: 0x00014948
		bool ITransformProvider.CanMove
		{
			get
			{
				return true;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can be resized; otherwise <see langword="false" />.</returns>
		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060026A4 RID: 9892 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanResize
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can rotate; otherwise <see langword="false" />.</returns>
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060026A5 RID: 9893 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="x"> Absolute screen coordinates of the left side of the control.</param>
		/// <param name="y"> Absolute screen coordinates of the top of the control.</param>
		// Token: 0x060026A6 RID: 9894 RVA: 0x000B7AA0 File Offset: 0x000B5CA0
		void ITransformProvider.Move(double x, double y)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			if (double.IsInfinity(x) || double.IsNaN(x))
			{
				throw new ArgumentOutOfRangeException("x");
			}
			if (double.IsInfinity(y) || double.IsNaN(y))
			{
				throw new ArgumentOutOfRangeException("y");
			}
			((GridSplitter)base.Owner).KeyboardMoveSplitter(x, y);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="width"> The new width of the window, in pixels.</param>
		/// <param name="height"> The new height of the window, in pixels.</param>
		// Token: 0x060026A7 RID: 9895 RVA: 0x000B7B04 File Offset: 0x000B5D04
		void ITransformProvider.Resize(double width, double height)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="degrees">The number of degrees to rotate the control.</param>
		// Token: 0x060026A8 RID: 9896 RVA: 0x000B7B04 File Offset: 0x000B5D04
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
		}
	}
}
