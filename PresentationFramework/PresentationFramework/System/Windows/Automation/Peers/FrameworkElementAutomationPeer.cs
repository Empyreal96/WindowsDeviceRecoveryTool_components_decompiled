using System;
using System.Windows.Data;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.FrameworkElement" /> types to UI Automation.</summary>
	// Token: 0x020002B6 RID: 694
	public class FrameworkElementAutomationPeer : UIElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.FrameworkElement" /> associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer" />.</param>
		// Token: 0x0600269B RID: 9883 RVA: 0x000B7946 File Offset: 0x000B5B46
		public FrameworkElementAutomationPeer(FrameworkElement owner) : base(owner)
		{
		}

		/// <summary>Gets the string that uniquely identifies the <see cref="T:System.Windows.FrameworkElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationId" />.</summary>
		/// <returns>The automation identifier for the element associated with the <see cref="T:System.Windows.Automation.Peers.FrameworkElementAutomationPeer" />, or <see cref="F:System.String.Empty" /> if there isn't an automation identifier.</returns>
		// Token: 0x0600269C RID: 9884 RVA: 0x000B7950 File Offset: 0x000B5B50
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text))
			{
				FrameworkElement frameworkElement = (FrameworkElement)base.Owner;
				text = base.Owner.Uid;
				if (string.IsNullOrEmpty(text))
				{
					text = frameworkElement.Name;
				}
			}
			return text ?? string.Empty;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The text label of the element that is associated with this automation peer.</returns>
		// Token: 0x0600269D RID: 9885 RVA: 0x000B79A0 File Offset: 0x000B5BA0
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				AutomationPeer labeledByCore = this.GetLabeledByCore();
				if (labeledByCore != null)
				{
					text = labeledByCore.GetName();
				}
				if (string.IsNullOrEmpty(text))
				{
					text = ((FrameworkElement)base.Owner).GetPlainText();
				}
			}
			return text ?? string.Empty;
		}

		/// <summary>Gets the string that describes the functionality of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetHelpText" />.</summary>
		/// <returns>The help text, usually from the <see cref="T:System.Windows.Controls.ToolTip" />, or <see cref="F:System.String.Empty" /> if there is no help text.</returns>
		// Token: 0x0600269E RID: 9886 RVA: 0x000B79F0 File Offset: 0x000B5BF0
		protected override string GetHelpTextCore()
		{
			string text = base.GetHelpTextCore();
			if (string.IsNullOrEmpty(text))
			{
				object toolTip = ((FrameworkElement)base.Owner).ToolTip;
				if (toolTip != null)
				{
					text = (toolTip as string);
					if (string.IsNullOrEmpty(text))
					{
						FrameworkElement frameworkElement = toolTip as FrameworkElement;
						if (frameworkElement != null)
						{
							text = frameworkElement.GetPlainText();
						}
					}
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000B7A4C File Offset: 0x000B5C4C
		internal override bool IgnoreUpdatePeer()
		{
			DependencyObject owner = base.Owner;
			return owner == null || owner.GetValue(FrameworkElement.DataContextProperty) == BindingOperations.DisconnectedSource || base.IgnoreUpdatePeer();
		}
	}
}
