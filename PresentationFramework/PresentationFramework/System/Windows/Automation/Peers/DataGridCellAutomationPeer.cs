using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridCell" /> types to UI Automation.</summary>
	// Token: 0x020002A0 RID: 672
	public sealed class DataGridCellAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridCellAutomationPeer" /> class. </summary>
		/// <param name="owner">An enumeration value that specifies the control pattern.</param>
		// Token: 0x0600257F RID: 9599 RVA: 0x000B47BC File Offset: 0x000B29BC
		public DataGridCellAutomationPeer(DataGridCell owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}
	}
}
