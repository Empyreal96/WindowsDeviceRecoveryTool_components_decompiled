using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGrid" /> types to UI Automation.</summary>
	// Token: 0x0200029F RID: 671
	public sealed class DataGridAutomationPeer : ItemsControlAutomationPeer, IGridProvider, ISelectionProvider, ITableProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x06002566 RID: 9574 RVA: 0x000B4109 File Offset: 0x000B2309
		public DataGridAutomationPeer(DataGrid owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000962DF File Offset: 0x000944DF
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataGrid;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000B4120 File Offset: 0x000B2320
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			DataGridColumnHeadersPresenter columnHeadersPresenter = this.OwningDataGrid.ColumnHeadersPresenter;
			if (columnHeadersPresenter != null && columnHeadersPresenter.IsVisible)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(columnHeadersPresenter);
				if (automationPeer != null)
				{
					if (list == null)
					{
						list = new List<AutomationPeer>(1);
					}
					list.Insert(0, automationPeer);
				}
			}
			return list;
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Grid" />, <see cref="F:System.Windows.Automation.Peers.PatternInterface.Selection" />, or <see cref="F:System.Windows.Automation.Peers.PatternInterface.Table" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x0600256A RID: 9578 RVA: 0x000B4168 File Offset: 0x000B2368
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Scroll)
			{
				if (patternInterface != PatternInterface.Selection)
				{
					if (patternInterface != PatternInterface.Scroll)
					{
						goto IL_45;
					}
					ScrollViewer internalScrollHost = this.OwningDataGrid.InternalScrollHost;
					if (internalScrollHost == null)
					{
						goto IL_45;
					}
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(internalScrollHost);
					IScrollProvider scrollProvider = automationPeer as IScrollProvider;
					if (automationPeer != null && scrollProvider != null)
					{
						automationPeer.EventsSource = this;
						return scrollProvider;
					}
					goto IL_45;
				}
			}
			else if (patternInterface != PatternInterface.Grid && patternInterface != PatternInterface.Table)
			{
				goto IL_45;
			}
			return this;
			IL_45:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000B41C1 File Offset: 0x000B23C1
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new DataGridItemAutomationPeer(item, this);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000B41CA File Offset: 0x000B23CA
		internal override bool IsPropertySupportedByControlForFindItem(int id)
		{
			return SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000B41D2 File Offset: 0x000B23D2
		internal override object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return SelectorAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		/// <summary>Gets the total number of columns in a grid.</summary>
		/// <returns>The total number of columns in a grid.</returns>
		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x000B41DB File Offset: 0x000B23DB
		int IGridProvider.ColumnCount
		{
			get
			{
				return this.OwningDataGrid.Columns.Count;
			}
		}

		/// <summary>Gets the total number of rows in a grid.</summary>
		/// <returns>The total number of rows in a grid.</returns>
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000B41ED File Offset: 0x000B23ED
		int IGridProvider.RowCount
		{
			get
			{
				return this.OwningDataGrid.Items.Count;
			}
		}

		/// <summary>Retrieves the UI Automation provider for the specified cell.</summary>
		/// <param name="row">The ordinal number of the row of interest.</param>
		/// <param name="column">The ordinal number of the column of interest.</param>
		/// <returns>The UI Automation provider for the specified cell.</returns>
		// Token: 0x06002570 RID: 9584 RVA: 0x000B4200 File Offset: 0x000B2400
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (row >= 0 && row < this.OwningDataGrid.Items.Count && column >= 0 && column < this.OwningDataGrid.Columns.Count)
			{
				object item = this.OwningDataGrid.Items[row];
				DataGridColumn column2 = this.OwningDataGrid.Columns[column];
				this.OwningDataGrid.ScrollIntoView(item, column2);
				this.OwningDataGrid.UpdateLayout();
				DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(column2);
					if (orCreateCellItemPeer != null)
					{
						return base.ProviderFromPeer(orCreateCellItemPeer);
					}
				}
			}
			return null;
		}

		/// <summary>Retrieves a UI Automation provider for each child element that is selected.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x06002571 RID: 9585 RVA: 0x000B42A0 File Offset: 0x000B24A0
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
			switch (this.OwningDataGrid.SelectionUnit)
			{
			case DataGridSelectionUnit.Cell:
				this.AddSelectedCells(list);
				break;
			case DataGridSelectionUnit.FullRow:
				this.AddSelectedRows(list);
				break;
			case DataGridSelectionUnit.CellOrRowHeader:
				this.AddSelectedRows(list);
				this.AddSelectedCells(list);
				break;
			}
			return list.ToArray();
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider allows more than one child element to be selected concurrently.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selection is allowed; otherwise <see langword="false" />.</returns>
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x000B42F9 File Offset: 0x000B24F9
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Extended;
			}
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider requires at least one child element to be selected.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		/// <summary>Retrieves the primary direction of traversal for the table.</summary>
		/// <returns>The primary direction of traversal. </returns>
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x0000B02A File Offset: 0x0000922A
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		/// <summary>Gets a collection of UI Automation providers that represents all the column headers in a table.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x06002575 RID: 9589 RVA: 0x000B430C File Offset: 0x000B250C
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if ((this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				DataGridColumnHeadersPresenter columnHeadersPresenter = this.OwningDataGrid.ColumnHeadersPresenter;
				if (columnHeadersPresenter != null)
				{
					DataGridColumnHeadersPresenterAutomationPeer dataGridColumnHeadersPresenterAutomationPeer = columnHeadersPresenter.GetAutomationPeer() as DataGridColumnHeadersPresenterAutomationPeer;
					if (dataGridColumnHeadersPresenterAutomationPeer != null)
					{
						for (int i = 0; i < this.OwningDataGrid.Columns.Count; i++)
						{
							AutomationPeer automationPeer = dataGridColumnHeadersPresenterAutomationPeer.FindOrCreateItemAutomationPeer(this.OwningDataGrid.Columns[i]);
							if (automationPeer != null)
							{
								list.Add(base.ProviderFromPeer(automationPeer));
							}
						}
						if (list.Count > 0)
						{
							return list.ToArray();
						}
					}
				}
			}
			return null;
		}

		/// <summary>Retrieves a collection of UI Automation providers that represents all row headers in the table.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x06002576 RID: 9590 RVA: 0x000B43A4 File Offset: 0x000B25A4
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			if ((this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				foreach (object item in ((IEnumerable)this.OwningDataGrid.Items))
				{
					DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer;
					AutomationPeer rowHeaderAutomationPeer = dataGridItemAutomationPeer.RowHeaderAutomationPeer;
					if (rowHeaderAutomationPeer != null)
					{
						list.Add(base.ProviderFromPeer(rowHeaderAutomationPeer));
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x000B4448 File Offset: 0x000B2648
		private DataGrid OwningDataGrid
		{
			get
			{
				return (DataGrid)base.Owner;
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000B4458 File Offset: 0x000B2658
		private DataGridCellItemAutomationPeer GetCellItemPeer(DataGridCellInfo cellInfo)
		{
			if (cellInfo.IsValid)
			{
				DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(cellInfo.Item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					return dataGridItemAutomationPeer.GetOrCreateCellItemPeer(cellInfo.Column);
				}
			}
			return null;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000B4494 File Offset: 0x000B2694
		internal void RaiseAutomationCellSelectedEvent(SelectedCellsChangedEventArgs e)
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && this.OwningDataGrid.SelectedCells.Count == 1 && e.AddedCells.Count == 1)
			{
				DataGridCellItemAutomationPeer cellItemPeer = this.GetCellItemPeer(e.AddedCells[0]);
				if (cellItemPeer != null)
				{
					cellItemPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
				{
					for (int i = 0; i < e.AddedCells.Count; i++)
					{
						DataGridCellItemAutomationPeer cellItemPeer2 = this.GetCellItemPeer(e.AddedCells[i]);
						if (cellItemPeer2 != null)
						{
							cellItemPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
						}
					}
				}
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					for (int i = 0; i < e.RemovedCells.Count; i++)
					{
						DataGridCellItemAutomationPeer cellItemPeer3 = this.GetCellItemPeer(e.RemovedCells[i]);
						if (cellItemPeer3 != null)
						{
							cellItemPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
						}
					}
				}
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000B4560 File Offset: 0x000B2760
		internal void RaiseAutomationRowInvokeEvents(DataGridRow row)
		{
			DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(row.Item) as DataGridItemAutomationPeer;
			if (dataGridItemAutomationPeer != null)
			{
				dataGridItemAutomationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000B458C File Offset: 0x000B278C
		internal void RaiseAutomationCellInvokeEvents(DataGridColumn column, DataGridRow row)
		{
			DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(row.Item) as DataGridItemAutomationPeer;
			if (dataGridItemAutomationPeer != null)
			{
				DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(column);
				if (orCreateCellItemPeer != null)
				{
					orCreateCellItemPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000B45C0 File Offset: 0x000B27C0
		internal void RaiseAutomationSelectionEvents(SelectionChangedEventArgs e)
		{
			int count = this.OwningDataGrid.SelectedItems.Count;
			int count2 = e.AddedItems.Count;
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && count == 1 && count2 == 1)
			{
				ItemAutomationPeer itemAutomationPeer = this.FindOrCreateItemAutomationPeer(this.OwningDataGrid.SelectedItem);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					return;
				}
			}
			else
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
				{
					for (int i = 0; i < e.AddedItems.Count; i++)
					{
						ItemAutomationPeer itemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(e.AddedItems[i]);
						if (itemAutomationPeer2 != null)
						{
							itemAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
						}
					}
				}
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					for (int i = 0; i < e.RemovedItems.Count; i++)
					{
						ItemAutomationPeer itemAutomationPeer3 = this.FindOrCreateItemAutomationPeer(e.RemovedItems[i]);
						if (itemAutomationPeer3 != null)
						{
							itemAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
						}
					}
				}
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000B4698 File Offset: 0x000B2898
		private void AddSelectedCells(List<IRawElementProviderSimple> cellProviders)
		{
			if (cellProviders == null)
			{
				throw new ArgumentNullException("cellProviders");
			}
			if (this.OwningDataGrid.SelectedCells != null)
			{
				foreach (DataGridCellInfo dataGridCellInfo in this.OwningDataGrid.SelectedCells)
				{
					DataGridItemAutomationPeer dataGridItemAutomationPeer = this.FindOrCreateItemAutomationPeer(dataGridCellInfo.Item) as DataGridItemAutomationPeer;
					if (dataGridItemAutomationPeer != null)
					{
						IRawElementProviderSimple rawElementProviderSimple = base.ProviderFromPeer(dataGridItemAutomationPeer.GetOrCreateCellItemPeer(dataGridCellInfo.Column));
						if (rawElementProviderSimple != null)
						{
							cellProviders.Add(rawElementProviderSimple);
						}
					}
				}
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000B4734 File Offset: 0x000B2934
		private void AddSelectedRows(List<IRawElementProviderSimple> itemProviders)
		{
			if (itemProviders == null)
			{
				throw new ArgumentNullException("itemProviders");
			}
			if (this.OwningDataGrid.SelectedItems != null)
			{
				foreach (object item in this.OwningDataGrid.SelectedItems)
				{
					IRawElementProviderSimple rawElementProviderSimple = base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(item));
					if (rawElementProviderSimple != null)
					{
						itemProviders.Add(rawElementProviderSimple);
					}
				}
			}
		}
	}
}
