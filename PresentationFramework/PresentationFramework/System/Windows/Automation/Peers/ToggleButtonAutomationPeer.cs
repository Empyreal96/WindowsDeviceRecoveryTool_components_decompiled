using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> types to UI Automation.</summary>
	// Token: 0x020002ED RID: 749
	public class ToggleButtonAutomationPeer : ButtonBaseAutomationPeer, IToggleProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />.</param>
		// Token: 0x06002852 RID: 10322 RVA: 0x000B309E File Offset: 0x000B129E
		public ToggleButtonAutomationPeer(ToggleButton owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Button".</returns>
		// Token: 0x06002853 RID: 10323 RVA: 0x000B30A7 File Offset: 0x000B12A7
		protected override string GetClassNameCore()
		{
			return "Button";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The control type for the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />.</returns>
		// Token: 0x06002854 RID: 10324 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Toggle" />, this method returns the current instance of the <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" />, otherwise <see langword="null" />.</returns>
		// Token: 0x06002855 RID: 10325 RVA: 0x000BC3C3 File Offset: 0x000BA5C3
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Toggle)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06002856 RID: 10326 RVA: 0x000BC3D4 File Offset: 0x000BA5D4
		void IToggleProvider.Toggle()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			ToggleButton toggleButton = (ToggleButton)base.Owner;
			toggleButton.OnToggle();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>the toggle state of the control.</returns>
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000BC404 File Offset: 0x000BA604
		ToggleState IToggleProvider.ToggleState
		{
			get
			{
				ToggleButton toggleButton = (ToggleButton)base.Owner;
				return ToggleButtonAutomationPeer.ConvertToToggleState(toggleButton.IsChecked);
			}
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000BC428 File Offset: 0x000BA628
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal virtual void RaiseToggleStatePropertyChangedEvent(bool? oldValue, bool? newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(TogglePatternIdentifiers.ToggleStateProperty, ToggleButtonAutomationPeer.ConvertToToggleState(oldValue), ToggleButtonAutomationPeer.ConvertToToggleState(newValue));
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000BC484 File Offset: 0x000BA684
		private static ToggleState ConvertToToggleState(bool? value)
		{
			bool? flag = value;
			if (flag != null)
			{
				bool valueOrDefault = flag.GetValueOrDefault();
				if (!valueOrDefault)
				{
					return ToggleState.Off;
				}
				if (valueOrDefault)
				{
					return ToggleState.On;
				}
			}
			return ToggleState.Indeterminate;
		}
	}
}
