using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeadersPresenter" /> types to UI Automation.</summary>
	// Token: 0x020002A4 RID: 676
	public sealed class DataGridColumnHeadersPresenterAutomationPeer : ItemsControlAutomationPeer, IItemContainerProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeadersPresenterAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x060025DA RID: 9690 RVA: 0x000B54DC File Offset: 0x000B36DC
		public DataGridColumnHeadersPresenterAutomationPeer(DataGridColumnHeadersPresenter owner) : base(owner)
		{
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00094D63 File Offset: 0x00092F63
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Header;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000B54E8 File Offset: 0x000B36E8
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			DataGrid owningDataGrid = this.OwningDataGrid;
			if (owningDataGrid != null && owningDataGrid.Columns.Count > 0)
			{
				Panel itemsHost = itemsControl.ItemsHost;
				IList list2;
				if (this.IsVirtualized)
				{
					if (itemsHost == null)
					{
						return null;
					}
					list2 = itemsHost.Children;
				}
				else
				{
					list2 = this.OwningDataGrid.Columns;
				}
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					DataGridColumn dataGridColumn;
					if (obj is DataGridColumnHeader)
					{
						dataGridColumn = ((DataGridColumnHeader)obj).Column;
					}
					else
					{
						dataGridColumn = (obj as DataGridColumn);
					}
					ItemAutomationPeer itemAutomationPeer = itemPeers[dataGridColumn];
					if (itemAutomationPeer == null)
					{
						itemAutomationPeer = base.GetPeerFromWeakRefStorage(dataGridColumn);
						if (itemAutomationPeer != null)
						{
							itemAutomationPeer.AncestorsInvalid = false;
							itemAutomationPeer.ChildrenValid = false;
						}
					}
					object o = (dataGridColumn == null) ? null : dataGridColumn.Header;
					if (itemAutomationPeer == null || !ItemsControl.EqualsEx(itemAutomationPeer.Item, o))
					{
						itemAutomationPeer = this.CreateItemAutomationPeer(dataGridColumn);
					}
					if (itemAutomationPeer != null)
					{
						AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
						if (wrapperPeer != null)
						{
							wrapperPeer.EventsSource = itemAutomationPeer;
						}
					}
					if (itemAutomationPeer != null && base.ItemPeers[dataGridColumn] == null)
					{
						list.Add(itemAutomationPeer);
						base.ItemPeers[dataGridColumn] = itemAutomationPeer;
					}
				}
				return list;
			}
			return null;
		}

		/// <summary>Retrieves an element with the specified property value.</summary>
		/// <param name="startAfter">The item in the container after which to begin the search.</param>
		/// <param name="propertyId">The property that contains the value to retrieve.</param>
		/// <param name="value">The value to retrieve.</param>
		/// <returns>The first item that matches the search criterion; otherwise, <see langword="null" /> if no items match.</returns>
		// Token: 0x060025DE RID: 9694 RVA: 0x000B567C File Offset: 0x000B387C
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !this.IsPropertySupportedByControlForFindItem(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			IList list = null;
			if (itemsControl != null)
			{
				list = this.OwningDataGrid.Columns;
			}
			if (list != null && list.Count > 0)
			{
				DataGridColumnHeaderItemAutomationPeer dataGridColumnHeaderItemAutomationPeer = null;
				if (startAfter != null)
				{
					dataGridColumnHeaderItemAutomationPeer = (base.PeerFromProvider(startAfter) as DataGridColumnHeaderItemAutomationPeer);
					if (dataGridColumnHeaderItemAutomationPeer == null)
					{
						return null;
					}
				}
				int num = 0;
				if (dataGridColumnHeaderItemAutomationPeer != null)
				{
					if (dataGridColumnHeaderItemAutomationPeer.Item == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = list.IndexOf(dataGridColumnHeaderItemAutomationPeer.Column) + 1;
					if (num == 0 || num == list.Count)
					{
						return null;
					}
				}
				if (propertyId == 0)
				{
					for (int i = num; i < list.Count; i++)
					{
						if (list.IndexOf(list[i]) == i)
						{
							return base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(list[i]));
						}
					}
				}
				object obj = null;
				for (int j = num; j < list.Count; j++)
				{
					ItemAutomationPeer itemAutomationPeer = this.FindOrCreateItemAutomationPeer(list[j]);
					if (itemAutomationPeer != null)
					{
						try
						{
							obj = this.GetSupportedPropertyValue(itemAutomationPeer, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_16B;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null && list.IndexOf(list[j]) == j)
							{
								return base.ProviderFromPeer(itemAutomationPeer);
							}
						}
						else if (value.Equals(obj) && list.IndexOf(list[j]) == j)
						{
							return base.ProviderFromPeer(itemAutomationPeer);
						}
					}
					IL_16B:;
				}
			}
			return null;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000B5818 File Offset: 0x000B3A18
		protected override ItemAutomationPeer CreateItemAutomationPeer(object column)
		{
			DataGridColumn dataGridColumn = column as DataGridColumn;
			if (column != null)
			{
				return new DataGridColumnHeaderItemAutomationPeer(dataGridColumn.Header, dataGridColumn, this);
			}
			return null;
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000B583E File Offset: 0x000B3A3E
		private DataGrid OwningDataGrid
		{
			get
			{
				return ((DataGridColumnHeadersPresenter)base.Owner).ParentDataGrid;
			}
		}
	}
}
