using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Expander" /> types to UI Automation.</summary>
	// Token: 0x020002AF RID: 687
	public class ExpanderAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Expander" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />.</param>
		// Token: 0x06002671 RID: 9841 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ExpanderAutomationPeer(Expander owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Expander" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Expander".</returns>
		// Token: 0x06002672 RID: 9842 RVA: 0x000B7282 File Offset: 0x000B5482
		protected override string GetClassNameCore()
		{
			return "Expander";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Expander" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Group" /> enumeration value.</returns>
		// Token: 0x06002673 RID: 9843 RVA: 0x000B7289 File Offset: 0x000B5489
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.Expander" /> control that is associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />, and sets the event source of the templated toggle button to this <see cref="M:System.Windows.Automation.Peers.ExpanderAutomationPeer.GetChildrenCore" /> instance. </summary>
		/// <returns>The collection of child elements of the <see cref="T:System.Windows.Controls.Expander" /> control associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />. </returns>
		// Token: 0x06002674 RID: 9844 RVA: 0x000B7290 File Offset: 0x000B5490
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			ToggleButton expanderToggleButton = ((Expander)base.Owner).ExpanderToggleButton;
			if (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures && childrenCore != null)
			{
				foreach (AutomationPeer automationPeer in childrenCore)
				{
					UIElementAutomationPeer uielementAutomationPeer = (UIElementAutomationPeer)automationPeer;
					if (uielementAutomationPeer.Owner == expanderToggleButton)
					{
						uielementAutomationPeer.EventsSource = ((!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && base.EventsSource != null) ? base.EventsSource : this);
						break;
					}
				}
			}
			return childrenCore;
		}

		/// <summary>Indicates whether the expander has automation keyboard focus. </summary>
		/// <returns>
		///   <see langword="true" /> if the expander has automation keyboard focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002675 RID: 9845 RVA: 0x000B7328 File Offset: 0x000B5528
		protected override bool HasKeyboardFocusCore()
		{
			return (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures && ((Expander)base.Owner).IsExpanderToggleButtonFocused) || base.HasKeyboardFocusCore();
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.Expander" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />. </summary>
		/// <param name="pattern">One of the enumeration values.</param>
		/// <returns>If <paramref name="pattern" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.ExpandCollapse" />, this method returns a reference to the current instance of the <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" />, otherwise this method calls the base implementation on <see cref="T:System.Windows.Automation.Peers.UIElementAutomationPeer" /> which returns <see langword="null" />.</returns>
		// Token: 0x06002676 RID: 9846 RVA: 0x000B734C File Offset: 0x000B554C
		public override object GetPattern(PatternInterface pattern)
		{
			object result;
			if (pattern == PatternInterface.ExpandCollapse)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(pattern);
			}
			return result;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002677 RID: 9847 RVA: 0x000B736C File Offset: 0x000B556C
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Expander expander = (Expander)base.Owner;
			expander.IsExpanded = true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002678 RID: 9848 RVA: 0x000B739C File Offset: 0x000B559C
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Expander expander = (Expander)base.Owner;
			expander.IsExpanded = false;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The state (expanded or collapsed) of the control.</returns>
		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x000B73CC File Offset: 0x000B55CC
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				Expander expander = (Expander)base.Owner;
				if (!expander.IsExpanded)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000B4088 File Offset: 0x000B2288
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}
	}
}
