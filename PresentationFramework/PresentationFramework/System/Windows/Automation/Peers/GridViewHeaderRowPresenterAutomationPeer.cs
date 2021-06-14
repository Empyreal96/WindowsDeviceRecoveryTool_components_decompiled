using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GridViewHeaderRowPresenter" /> types to UI Automation.</summary>
	// Token: 0x020002BA RID: 698
	public class GridViewHeaderRowPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GridViewHeaderRowPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" />.</param>
		// Token: 0x060026C4 RID: 9924 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public GridViewHeaderRowPresenterAutomationPeer(GridViewHeaderRowPresenter owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.GridViewHeaderRowPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "GridViewHeaderRowPresenter".</returns>
		// Token: 0x060026C5 RID: 9925 RVA: 0x000B80AB File Offset: 0x000B62AB
		protected override string GetClassNameCore()
		{
			return "GridViewHeaderRowPresenter";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.GridViewHeaderRowPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Header" /> enumeration value.</returns>
		// Token: 0x060026C6 RID: 9926 RVA: 0x00094D63 File Offset: 0x00092F63
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Header;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x060026C7 RID: 9927 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.GridViewHeaderRowPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewHeaderRowPresenterAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x060026C8 RID: 9928 RVA: 0x000B80B4 File Offset: 0x000B62B4
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			List<AutomationPeer> list = null;
			if (childrenCore != null)
			{
				list = new List<AutomationPeer>(childrenCore.Count);
				foreach (AutomationPeer automationPeer in childrenCore)
				{
					if (automationPeer is UIElementAutomationPeer)
					{
						GridViewColumnHeader gridViewColumnHeader = ((UIElementAutomationPeer)automationPeer).Owner as GridViewColumnHeader;
						if (gridViewColumnHeader != null && gridViewColumnHeader.Role == GridViewColumnHeaderRole.Normal)
						{
							list.Insert(0, automationPeer);
						}
					}
				}
			}
			return list;
		}
	}
}
