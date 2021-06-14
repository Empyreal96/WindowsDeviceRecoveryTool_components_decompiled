using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.CalendarButton" /> types to UI Automation.</summary>
	// Token: 0x0200029A RID: 666
	public sealed class CalendarButtonAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.CalendarButtonAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x06002546 RID: 9542 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public CalendarButtonAutomationPeer(Button owner) : base(owner)
		{
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x000B3DB5 File Offset: 0x000B1FB5
		private bool IsDayButton
		{
			get
			{
				return base.Owner is CalendarDayButton;
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000B3DC5 File Offset: 0x000B1FC5
		protected override string GetLocalizedControlTypeCore()
		{
			if (!this.IsDayButton)
			{
				return SR.Get("CalendarAutomationPeer_CalendarButtonLocalizedControlType");
			}
			return SR.Get("CalendarAutomationPeer_DayButtonLocalizedControlType");
		}
	}
}
