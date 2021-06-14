using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace System.Windows.Automation.Peers
{
	/// <summary>Represents a base class for exposing elements derived from <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> to UI Automation.</summary>
	// Token: 0x02000297 RID: 663
	public abstract class ButtonBaseAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Provides initialization for base class values when called by the constructor of a derived class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.ButtonBase" /> that is associated with this peer.</param>
		// Token: 0x06002521 RID: 9505 RVA: 0x000B30F9 File Offset: 0x000B12F9
		protected ButtonBaseAutomationPeer(ButtonBase owner) : base(owner)
		{
		}

		/// <summary>Gets the accelerator key for the element associated with this <see cref="T:System.Windows.Automation.Peers.ButtonBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAcceleratorKey" />.</summary>
		/// <returns>A string containing the accelerator key.</returns>
		// Token: 0x06002522 RID: 9506 RVA: 0x000B3104 File Offset: 0x000B1304
		protected override string GetAcceleratorKeyCore()
		{
			string text = base.GetAcceleratorKeyCore();
			if (text == string.Empty)
			{
				RoutedUICommand routedUICommand = ((ButtonBase)base.Owner).Command as RoutedUICommand;
				if (routedUICommand != null && !string.IsNullOrEmpty(routedUICommand.Text))
				{
					text = routedUICommand.Text;
				}
			}
			return text;
		}

		/// <summary>Gets the <see cref="P:System.Windows.Automation.AutomationProperties.AutomationId" /> for the element associated with this <see cref="T:System.Windows.Automation.Peers.ButtonBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationId" />.</summary>
		/// <returns>The string that contains the <see cref="P:System.Windows.Automation.AutomationProperties.AutomationId" />.</returns>
		// Token: 0x06002523 RID: 9507 RVA: 0x000B3154 File Offset: 0x000B1354
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text))
			{
				ButtonBase buttonBase = (ButtonBase)base.Owner;
				RoutedCommand routedCommand = buttonBase.Command as RoutedCommand;
				if (routedCommand != null)
				{
					string name = routedCommand.Name;
					if (!string.IsNullOrEmpty(name))
					{
						text = name;
					}
				}
			}
			return text ?? string.Empty;
		}

		/// <summary>Gets the name of the class of the element associated with this <see cref="T:System.Windows.Automation.Peers.ButtonBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>A string that contains the class name, minus the accelerator key.</returns>
		// Token: 0x06002524 RID: 9508 RVA: 0x000B31A8 File Offset: 0x000B13A8
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			ButtonBase buttonBase = (ButtonBase)base.Owner;
			if (!string.IsNullOrEmpty(text))
			{
				if (buttonBase.Content is string)
				{
					text = AccessText.RemoveAccessKeyMarker(text);
				}
			}
			else
			{
				RoutedUICommand routedUICommand = buttonBase.Command as RoutedUICommand;
				if (routedUICommand != null && !string.IsNullOrEmpty(routedUICommand.Text))
				{
					text = routedUICommand.Text;
				}
			}
			return text;
		}
	}
}
