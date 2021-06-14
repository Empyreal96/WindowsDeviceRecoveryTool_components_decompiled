using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.PasswordBox" /> types to UI Automation.</summary>
	// Token: 0x020002D4 RID: 724
	public class PasswordBoxAutomationPeer : TextAutomationPeer, IValueProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.PasswordBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" />.</param>
		// Token: 0x06002796 RID: 10134 RVA: 0x000BA6AB File Offset: 0x000B88AB
		public PasswordBoxAutomationPeer(PasswordBox owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.PasswordBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "PasswordBox".</returns>
		// Token: 0x06002797 RID: 10135 RVA: 0x000BA6B4 File Offset: 0x000B88B4
		protected override string GetClassNameCore()
		{
			return "PasswordBox";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.PasswordBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Edit" /> enumeration value.</returns>
		// Token: 0x06002798 RID: 10136 RVA: 0x00094CFC File Offset: 0x00092EFC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Edit;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.PasswordBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.PatternInterface.Value" /> enumeration value.</returns>
		// Token: 0x06002799 RID: 10137 RVA: 0x000BA6BC File Offset: 0x000B88BC
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			if (patternInterface == PatternInterface.Value)
			{
				obj = this;
			}
			else if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, ((PasswordBox)base.Owner).TextContainer);
				}
				obj = this._textPattern;
			}
			else if (patternInterface == PatternInterface.Scroll)
			{
				PasswordBox passwordBox = (PasswordBox)base.Owner;
				if (passwordBox.ScrollViewer != null)
				{
					obj = passwordBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			else
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.PasswordBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.PasswordBoxAutomationPeer" /> contains protected content. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsPassword" />.</summary>
		/// <returns>
		///     <see langword="true" />.</returns>
		// Token: 0x0600279A RID: 10138 RVA: 0x00016748 File Offset: 0x00014948
		protected override bool IsPasswordCore()
		{
			return true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified.</returns>
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x0600279B RID: 10139 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>A string value of the control.</returns>
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000BA740 File Offset: 0x000B8940
		string IValueProvider.Value
		{
			get
			{
				if (AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures)
				{
					throw new InvalidOperationException();
				}
				return string.Empty;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> The value of a control.</param>
		// Token: 0x0600279D RID: 10141 RVA: 0x000BA754 File Offset: 0x000B8954
		void IValueProvider.SetValue(string value)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			PasswordBox passwordBox = (PasswordBox)base.Owner;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			passwordBox.Password = value;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000B3FD9 File Offset: 0x000B21D9
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000BA790 File Offset: 0x000B8990
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseIsReadOnlyPropertyChangedEvent(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.IsReadOnlyProperty, oldValue, newValue);
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000BA7AD File Offset: 0x000B89AD
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			return new List<AutomationPeer>();
		}

		// Token: 0x04001B93 RID: 7059
		private TextAdaptor _textPattern;
	}
}
