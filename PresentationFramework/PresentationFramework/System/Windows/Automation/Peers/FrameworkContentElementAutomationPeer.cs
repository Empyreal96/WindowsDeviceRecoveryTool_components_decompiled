using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.FrameworkContentElement" /> types to UI Automation.</summary>
	// Token: 0x020002B5 RID: 693
	public class FrameworkContentElementAutomationPeer : ContentElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FrameworkContentElementAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkContentElementAutomationPeer" />.</param>
		// Token: 0x06002697 RID: 9879 RVA: 0x000B7872 File Offset: 0x000B5A72
		public FrameworkContentElementAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
		}

		/// <summary>Gets the string that uniquely identifies the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationId" />.</summary>
		/// <returns>The string that uniquely identifies the <see cref="T:System.Windows.FrameworkContentElement" />.</returns>
		// Token: 0x06002698 RID: 9880 RVA: 0x000B787C File Offset: 0x000B5A7C
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text))
			{
				text = ((FrameworkContentElement)base.Owner).Name;
			}
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		/// <summary>Gets the string that describes the functionality of the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetHelpText" />.</summary>
		/// <returns>The string describing the functionality of the element.</returns>
		// Token: 0x06002699 RID: 9881 RVA: 0x000B78BC File Offset: 0x000B5ABC
		protected override string GetHelpTextCore()
		{
			string text = base.GetHelpTextCore();
			if (string.IsNullOrEmpty(text))
			{
				object toolTip = ((FrameworkContentElement)base.Owner).ToolTip;
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

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Label" /> that is targeted to the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameworkContentElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetLabeledBy" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" /> for the element that is targeted by the <see cref="T:System.Windows.Controls.Label" />.</returns>
		// Token: 0x0600269A RID: 9882 RVA: 0x000B7918 File Offset: 0x000B5B18
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer labeledByCore = base.GetLabeledByCore();
			if (labeledByCore == null)
			{
				Label labeledBy = Label.GetLabeledBy(base.Owner);
				if (labeledBy != null)
				{
					return labeledBy.GetAutomationPeer();
				}
			}
			return labeledByCore;
		}
	}
}
