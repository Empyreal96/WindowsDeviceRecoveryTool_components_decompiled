using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ProgressBar" /> types to UI Automation.</summary>
	// Token: 0x020002D6 RID: 726
	public class ProgressBarAutomationPeer : RangeBaseAutomationPeer, IRangeValueProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ProgressBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" />.</param>
		// Token: 0x060027A4 RID: 10148 RVA: 0x000BA7BB File Offset: 0x000B89BB
		public ProgressBarAutomationPeer(ProgressBar owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ProgressBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ProgressBar".</returns>
		// Token: 0x060027A5 RID: 10149 RVA: 0x000BA7C4 File Offset: 0x000B89C4
		protected override string GetClassNameCore()
		{
			return "ProgressBar";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ProgressBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ProgressBar" /> enumeration value.</returns>
		// Token: 0x060027A6 RID: 10150 RVA: 0x00095804 File Offset: 0x00093A04
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ProgressBar;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.ProgressBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.RangeValue" /> and <see cref="P:System.Windows.Controls.ProgressBar.IsIndeterminate" /> is <see langword="true" />, this method returns the current instance of the <see cref="T:System.Windows.Automation.Peers.ProgressBarAutomationPeer" />; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x060027A7 RID: 10151 RVA: 0x000BA7CB File Offset: 0x000B89CB
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.RangeValue && ((ProgressBar)base.Owner).IsIndeterminate)
			{
				return null;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="val"> The value to set.</param>
		// Token: 0x060027A8 RID: 10152 RVA: 0x000BA7EC File Offset: 0x000B89EC
		void IRangeValueProvider.SetValue(double val)
		{
			throw new InvalidOperationException(SR.Get("ProgressBarReadOnly"));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; otherwise <see langword="false" />.</returns>
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x00016748 File Offset: 0x00014948
		bool IRangeValueProvider.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The large-change value.</returns>
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x000BA7FD File Offset: 0x000B89FD
		double IRangeValueProvider.LargeChange
		{
			get
			{
				return double.NaN;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The small-change value.</returns>
		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000BA7FD File Offset: 0x000B89FD
		double IRangeValueProvider.SmallChange
		{
			get
			{
				return double.NaN;
			}
		}
	}
}
