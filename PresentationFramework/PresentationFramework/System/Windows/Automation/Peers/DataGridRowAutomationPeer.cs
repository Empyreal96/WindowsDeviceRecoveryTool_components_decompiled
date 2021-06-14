using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridRow" /> types to UI Automation.</summary>
	// Token: 0x020002A7 RID: 679
	public sealed class DataGridRowAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridRowAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x06002602 RID: 9730 RVA: 0x000B47BC File Offset: 0x000B29BC
		public DataGridRowAutomationPeer(DataGridRow owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000964BA File Offset: 0x000946BA
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000B6070 File Offset: 0x000B4270
		protected override List<AutomationPeer> GetChildrenCore()
		{
			DataGridCellsPresenter cellsPresenter = this.OwningDataGridRow.CellsPresenter;
			if (cellsPresenter != null && cellsPresenter.ItemsHost != null)
			{
				List<AutomationPeer> list = new List<AutomationPeer>(3);
				AutomationPeer rowHeaderAutomationPeer = this.RowHeaderAutomationPeer;
				if (rowHeaderAutomationPeer != null)
				{
					list.Add(rowHeaderAutomationPeer);
				}
				DataGridItemAutomationPeer dataGridItemAutomationPeer = base.EventsSource as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					list.AddRange(dataGridItemAutomationPeer.GetCellItemPeers());
				}
				AutomationPeer detailsPresenterAutomationPeer = this.DetailsPresenterAutomationPeer;
				if (detailsPresenterAutomationPeer != null)
				{
					list.Add(detailsPresenterAutomationPeer);
				}
				return list;
			}
			return base.GetChildrenCore();
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000B60E4 File Offset: 0x000B42E4
		internal AutomationPeer RowHeaderAutomationPeer
		{
			get
			{
				DataGridRowHeader rowHeader = this.OwningDataGridRow.RowHeader;
				if (rowHeader != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(rowHeader);
				}
				return null;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x000B6108 File Offset: 0x000B4308
		private AutomationPeer DetailsPresenterAutomationPeer
		{
			get
			{
				DataGridDetailsPresenter detailsPresenter = this.OwningDataGridRow.DetailsPresenter;
				if (detailsPresenter != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(detailsPresenter);
				}
				return null;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x000B612C File Offset: 0x000B432C
		private DataGridRow OwningDataGridRow
		{
			get
			{
				return (DataGridRow)base.Owner;
			}
		}
	}
}
