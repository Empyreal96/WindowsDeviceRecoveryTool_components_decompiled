using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> types to UI Automation.</summary>
	// Token: 0x020002AC RID: 684
	public class DocumentPageViewAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" />.</param>
		// Token: 0x06002665 RID: 9829 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public DocumentPageViewAutomationPeer(DocumentPageView owner) : base(owner)
		{
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements of the <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" />.</returns>
		// Token: 0x06002666 RID: 9830 RVA: 0x0000C238 File Offset: 0x0000A438
		protected override List<AutomationPeer> GetChildrenCore()
		{
			return null;
		}

		/// <summary>Gets the string that uniquely identifies the <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationId" />.</summary>
		/// <returns>A string that contains the automation identifier.</returns>
		// Token: 0x06002667 RID: 9831 RVA: 0x000B70BC File Offset: 0x000B52BC
		protected override string GetAutomationIdCore()
		{
			string result = string.Empty;
			DocumentPageView documentPageView = (DocumentPageView)base.Owner;
			if (!string.IsNullOrEmpty(documentPageView.Name))
			{
				result = documentPageView.Name;
			}
			else if (documentPageView.PageNumber >= 0 && documentPageView.PageNumber < 2147483647)
			{
				result = string.Format(CultureInfo.InvariantCulture, "DocumentPage{0}", new object[]
				{
					documentPageView.PageNumber + 1
				});
			}
			return result;
		}
	}
}
