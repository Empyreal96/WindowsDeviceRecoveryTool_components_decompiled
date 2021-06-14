using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DataGridRowHeader" /> types to UI Automation.</summary>
	// Token: 0x020002A8 RID: 680
	public sealed class DataGridRowHeaderAutomationPeer : ButtonBaseAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridRowHeaderAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x06002609 RID: 9737 RVA: 0x000B309E File Offset: 0x000B129E
		public DataGridRowHeaderAutomationPeer(DataGridRowHeader owner) : base(owner)
		{
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x00094F9F File Offset: 0x0009319F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
