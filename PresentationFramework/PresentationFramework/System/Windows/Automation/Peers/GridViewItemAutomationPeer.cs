using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the data items in the collection of <see cref="P:System.Windows.Controls.ItemsControl.Items" /> in <see cref="T:System.Windows.Controls.GridView" /> types to UI Automation.</summary>
	// Token: 0x020002BC RID: 700
	public class GridViewItemAutomationPeer : ListBoxItemAutomationPeer
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The data item that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" />.</param>
		/// <param name="listviewAP">The <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> that is the parent of this <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" />.</param>
		// Token: 0x060026DC RID: 9948 RVA: 0x000B8360 File Offset: 0x000B6560
		public GridViewItemAutomationPeer(object owner, ListViewAutomationPeer listviewAP) : base(owner, listviewAP)
		{
			Invariant.Assert(listviewAP != null);
			this._listviewAP = listviewAP;
		}

		/// <summary>Gets the name of the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ListViewItem".</returns>
		// Token: 0x060026DD RID: 9949 RVA: 0x000B837A File Offset: 0x000B657A
		protected override string GetClassNameCore()
		{
			return "ListViewItem";
		}

		/// <summary>Gets the control type for the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.DataItem" /> enumeration value.</returns>
		// Token: 0x060026DE RID: 9950 RVA: 0x000964BA File Offset: 0x000946BA
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		/// <summary>Gets the collection of child elements of the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x060026DF RID: 9951 RVA: 0x000B8384 File Offset: 0x000B6584
		protected override List<AutomationPeer> GetChildrenCore()
		{
			ListView listView = this._listviewAP.Owner as ListView;
			Invariant.Assert(listView != null);
			object item = base.Item;
			ListViewItem listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
			if (listViewItem != null)
			{
				GridViewRowPresenter gridViewRowPresenter = GridViewAutomationPeer.FindVisualByType(listViewItem, typeof(GridViewRowPresenter)) as GridViewRowPresenter;
				if (gridViewRowPresenter != null)
				{
					Hashtable dataChildren = this._dataChildren;
					this._dataChildren = new Hashtable(gridViewRowPresenter.ActualCells.Count);
					List<AutomationPeer> list = new List<AutomationPeer>();
					int row = listView.Items.IndexOf(item);
					int num = 0;
					foreach (UIElement uielement in gridViewRowPresenter.ActualCells)
					{
						GridViewCellAutomationPeer gridViewCellAutomationPeer = (dataChildren == null) ? null : ((GridViewCellAutomationPeer)dataChildren[uielement]);
						if (gridViewCellAutomationPeer == null)
						{
							if (uielement is ContentPresenter)
							{
								gridViewCellAutomationPeer = new GridViewCellAutomationPeer((ContentPresenter)uielement, this._listviewAP);
							}
							else if (uielement is TextBlock)
							{
								gridViewCellAutomationPeer = new GridViewCellAutomationPeer((TextBlock)uielement, this._listviewAP);
							}
							else
							{
								Invariant.Assert(false, "Children of GridViewRowPresenter should be ContentPresenter or TextBlock");
							}
						}
						if (this._dataChildren[uielement] == null)
						{
							gridViewCellAutomationPeer.Column = num;
							gridViewCellAutomationPeer.Row = row;
							list.Add(gridViewCellAutomationPeer);
							this._dataChildren.Add(uielement, gridViewCellAutomationPeer);
							num++;
						}
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x04001B7F RID: 7039
		private ListViewAutomationPeer _listviewAP;

		// Token: 0x04001B80 RID: 7040
		private Hashtable _dataChildren;
	}
}
