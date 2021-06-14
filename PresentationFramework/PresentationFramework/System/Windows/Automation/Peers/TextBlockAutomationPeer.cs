using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TextBlock" /> types to UI Automation.</summary>
	// Token: 0x020002E9 RID: 745
	public class TextBlockAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TextBlock" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" />.</param>
		// Token: 0x06002837 RID: 10295 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public TextBlockAutomationPeer(TextBlock owner) : base(owner)
		{
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.TextBlock" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>A collection of child elements, or <see langword="null" /> if the <see cref="T:System.Windows.Controls.TextBlock" /> is empty.</returns>
		// Token: 0x06002838 RID: 10296 RVA: 0x000BBE78 File Offset: 0x000BA078
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> result = null;
			TextBlock textBlock = (TextBlock)base.Owner;
			if (textBlock.HasComplexContent)
			{
				result = TextContainerHelper.GetAutomationPeersFromRange(textBlock.TextContainer.Start, textBlock.TextContainer.End, null);
			}
			return result;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.TextBlock" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Text" /> enumeration value.</returns>
		// Token: 0x06002839 RID: 10297 RVA: 0x0009444F File Offset: 0x0009264F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TextBlock" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains the word "TextBlock".</returns>
		// Token: 0x0600283A RID: 10298 RVA: 0x000BBEB9 File Offset: 0x000BA0B9
		protected override string GetClassNameCore()
		{
			return "TextBlock";
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.TextBlock" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBlockAutomationPeer" /> is something that the end user would understand as being interactive or as contributing to the logical structure of the control in the GUI. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> if the element is part of a template; otherwise <see langword="true" />.</returns>
		// Token: 0x0600283B RID: 10299 RVA: 0x000BBEC0 File Offset: 0x000BA0C0
		protected override bool IsControlElementCore()
		{
			TextBlock textBlock = (TextBlock)base.Owner;
			DependencyObject templatedParent = textBlock.TemplatedParent;
			return (templatedParent == null || templatedParent is ContentPresenter) && base.IsControlElementCore();
		}
	}
}
