using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ComboBox" /> types to UI Automation.</summary>
	// Token: 0x0200029C RID: 668
	public class ComboBoxAutomationPeer : SelectorAutomationPeer, IValueProvider, IExpandCollapseProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ComboBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />.</param>
		// Token: 0x0600254E RID: 9550 RVA: 0x000B3DF4 File Offset: 0x000B1FF4
		public ComboBoxAutomationPeer(ComboBox owner) : base(owner)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ComboBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />.</param>
		/// <returns>A new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</returns>
		// Token: 0x0600254F RID: 9551 RVA: 0x000B3DFD File Offset: 0x000B1FFD
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ListBoxItemAutomationPeer(item, this);
		}

		/// <summary>Gets the control type for this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ComboBox" /> enumeration value.</returns>
		// Token: 0x06002550 RID: 9552 RVA: 0x000800F2 File Offset: 0x0007E2F2
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ComboBox;
		}

		/// <summary>Gets the name of the class that defines the type that is associated with this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ComboBox".</returns>
		// Token: 0x06002551 RID: 9553 RVA: 0x000B3E06 File Offset: 0x000B2006
		protected override string GetClassNameCore()
		{
			return "ComboBox";
		}

		/// <summary>Gets the control pattern for this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />.</summary>
		/// <param name="pattern">One of the enumeration values.</param>
		/// <returns>If <paramref name="pattern" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Value" /> or <see cref="F:System.Windows.Automation.Peers.PatternInterface.ExpandCollapse" />, this method returns a reference to the current instance of the <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" />; otherwise, this method calls the base implementation on <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" />. </returns>
		// Token: 0x06002552 RID: 9554 RVA: 0x000B3E10 File Offset: 0x000B2010
		public override object GetPattern(PatternInterface pattern)
		{
			object result = null;
			if (pattern == PatternInterface.Value)
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				if (comboBox.IsEditable)
				{
					result = this;
				}
			}
			else if (pattern == PatternInterface.ExpandCollapse)
			{
				result = this;
			}
			else
			{
				result = base.GetPattern(pattern);
			}
			return result;
		}

		/// <summary>Gets a collection of child elements. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>A collection of child elements.</returns>
		// Token: 0x06002553 RID: 9555 RVA: 0x000B3E4C File Offset: 0x000B204C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			ComboBox comboBox = (ComboBox)base.Owner;
			TextBox editableTextBoxSite = comboBox.EditableTextBoxSite;
			if (editableTextBoxSite != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(editableTextBoxSite);
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>();
					}
					list.Insert(0, automationPeer);
				}
			}
			return list;
		}

		/// <summary>Sets the keyboard input focus on the <see cref="T:System.Windows.Controls.ComboBox" /> control that is associated with this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" /> object. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.SetFocus" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.ComboBox" /> control that is associated with this <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" /> object cannot receive focus.</exception>
		// Token: 0x06002554 RID: 9556 RVA: 0x000B3E94 File Offset: 0x000B2094
		protected override void SetFocusCore()
		{
			ComboBox comboBox = (ComboBox)base.Owner;
			if (comboBox.Focusable)
			{
				if (!comboBox.Focus())
				{
					if (!comboBox.IsEditable)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
					TextBox editableTextBoxSite = comboBox.EditableTextBoxSite;
					if (editableTextBoxSite == null || !editableTextBoxSite.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000B3F08 File Offset: 0x000B2108
		internal void ScrollItemIntoView(object item)
		{
			if (((IExpandCollapseProvider)this).ExpandCollapseState == ExpandCollapseState.Expanded)
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				if (comboBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
				{
					comboBox.OnBringItemIntoView(item);
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(comboBox.OnBringItemIntoView), item);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="val"> The string value of a control.</param>
		// Token: 0x06002556 RID: 9558 RVA: 0x000B3F5C File Offset: 0x000B215C
		void IValueProvider.SetValue(string val)
		{
			if (val == null)
			{
				throw new ArgumentNullException("val");
			}
			ComboBox comboBox = (ComboBox)base.Owner;
			if (!comboBox.IsEnabled)
			{
				throw new ElementNotEnabledException();
			}
			comboBox.SetCurrentValueInternal(ComboBox.TextProperty, val);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>A string value of the control.</returns>
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002557 RID: 9559 RVA: 0x000B3F9D File Offset: 0x000B219D
		string IValueProvider.Value
		{
			get
			{
				return ((ComboBox)base.Owner).Text;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified.</returns>
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x000B3FB0 File Offset: 0x000B21B0
		bool IValueProvider.IsReadOnly
		{
			get
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				return !comboBox.IsEnabled || comboBox.IsReadOnly;
			}
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000B3FD9 File Offset: 0x000B21D9
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600255A RID: 9562 RVA: 0x000B3FF4 File Offset: 0x000B21F4
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			ComboBox comboBox = (ComboBox)base.Owner;
			comboBox.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600255B RID: 9563 RVA: 0x000B402C File Offset: 0x000B222C
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			ComboBox comboBox = (ComboBox)base.Owner;
			comboBox.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The <see cref="T:System.Windows.Automation.ExpandCollapseState" /> for the current element.</returns>
		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000B4064 File Offset: 0x000B2264
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				ComboBox comboBox = (ComboBox)base.Owner;
				if (!comboBox.IsDropDownOpen)
				{
					return ExpandCollapseState.Collapsed;
				}
				return ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000B4088 File Offset: 0x000B2288
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}
	}
}
