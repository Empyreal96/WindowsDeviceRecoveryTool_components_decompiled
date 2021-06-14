using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridRow" /> types to UI Automation. The <see cref="T:System.Windows.Controls.DataGridRow" /> may or may not actually exist in memory.</summary>
	// Token: 0x020002A6 RID: 678
	public sealed class DataGridItemAutomationPeer : ItemAutomationPeer, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, ISelectionProvider, IItemContainerProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridItemAutomationPeer" /> class. </summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridItemAutomationPeer" />.</param>
		/// <param name="dataGridPeer">The <see cref="T:System.Windows.Automation.Peers.DataGridAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.DataGrid" /> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</param>
		// Token: 0x060025E4 RID: 9700 RVA: 0x000B5850 File Offset: 0x000B3A50
		public DataGridItemAutomationPeer(object item, DataGridAutomationPeer dataGridPeer) : base(item, dataGridPeer)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (dataGridPeer == null)
			{
				throw new ArgumentNullException("dataGridPeer");
			}
			this._dataGridAutomationPeer = dataGridPeer;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000964BA File Offset: 0x000946BA
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000B58A0 File Offset: 0x000B3AA0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.ForceEnsureChildren();
				return wrapperPeer.GetChildren();
			}
			return this.GetCellItemPeers();
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000B58CC File Offset: 0x000B3ACC
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			base.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration value that specifies the control pattern.</param>
		/// <returns>The current <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> object, if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. For more information, see Remarks.</returns>
		// Token: 0x060025E8 RID: 9704 RVA: 0x000B58F8 File Offset: 0x000B3AF8
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Selection)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface != PatternInterface.Selection)
					{
						goto IL_38;
					}
				}
				else
				{
					if (!this.OwningDataGrid.IsReadOnly)
					{
						return this;
					}
					goto IL_38;
				}
			}
			else if (patternInterface != PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.SelectionItem)
				{
					if (patternInterface != PatternInterface.ItemContainer)
					{
						goto IL_38;
					}
				}
				else
				{
					if (this.IsRowSelectionUnit)
					{
						return this;
					}
					goto IL_38;
				}
			}
			return this;
			IL_38:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000B5944 File Offset: 0x000B3B44
		protected override AutomationPeer GetPeerFromPointCore(Point point)
		{
			if (!base.IsOffscreen())
			{
				AutomationPeer rowHeaderAutomationPeer = this.RowHeaderAutomationPeer;
				if (rowHeaderAutomationPeer != null)
				{
					AutomationPeer peerFromPoint = rowHeaderAutomationPeer.GetPeerFromPoint(point);
					if (peerFromPoint != null)
					{
						return peerFromPoint;
					}
				}
			}
			return base.GetPeerFromPointCore(point);
		}

		/// <summary>Retrieves an element by the specified property value.</summary>
		/// <param name="startAfter">The item in the container after which to begin the search.</param>
		/// <param name="propertyId">The property that contains the value to retrieve.</param>
		/// <param name="value">The value to retrieve.</param>
		/// <returns>The first item that matches the search criterion; otherwise, <see langword="null" /> if no items match.</returns>
		// Token: 0x060025EA RID: 9706 RVA: 0x000B5978 File Offset: 0x000B3B78
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !SelectorAutomationPeer.IsPropertySupportedByControlForFindItemInternal(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			IList<DataGridColumn> columns = this.OwningDataGrid.Columns;
			if (columns != null && columns.Count > 0)
			{
				DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = null;
				if (startAfter != null)
				{
					dataGridCellItemAutomationPeer = (base.PeerFromProvider(startAfter) as DataGridCellItemAutomationPeer);
				}
				int num = 0;
				if (dataGridCellItemAutomationPeer != null)
				{
					if (dataGridCellItemAutomationPeer.Column == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = columns.IndexOf(dataGridCellItemAutomationPeer.Column) + 1;
					if (num == 0 || num == columns.Count)
					{
						return null;
					}
				}
				if (propertyId == 0 && num < columns.Count)
				{
					return base.ProviderFromPeer(this.GetOrCreateCellItemPeer(columns[num]));
				}
				object obj = null;
				for (int i = num; i < columns.Count; i++)
				{
					DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(columns[i]);
					if (orCreateCellItemPeer != null)
					{
						try
						{
							obj = SelectorAutomationPeer.GetSupportedPropertyValueInternal(orCreateCellItemPeer, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_108;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null)
							{
								return base.ProviderFromPeer(orCreateCellItemPeer);
							}
						}
						else if (value.Equals(obj))
						{
							return base.ProviderFromPeer(orCreateCellItemPeer);
						}
					}
					IL_108:;
				}
			}
			return null;
		}

		/// <summary>Sends a request to activate a control and initiate its single, unambiguous action.</summary>
		// Token: 0x060025EB RID: 9707 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		void IInvokeProvider.Invoke()
		{
			this.EnsureEnabled();
			object item = base.Item;
			if (this.GetWrapperPeer() == null)
			{
				this.OwningDataGrid.ScrollIntoView(item);
			}
			bool flag = false;
			UIElement wrapper = base.GetWrapper();
			if (wrapper != null)
			{
				IEditableCollectionView items = this.OwningDataGrid.Items;
				if (items.CurrentEditItem == item)
				{
					flag = this.OwningDataGrid.CommitEdit();
				}
				else if (this.OwningDataGrid.Columns.Count > 0)
				{
					DataGridCell dataGridCell = this.OwningDataGrid.TryFindCell(item, this.OwningDataGrid.Columns[0]);
					if (dataGridCell != null)
					{
						this.OwningDataGrid.UnselectAll();
						dataGridCell.Focus();
						flag = this.OwningDataGrid.BeginEdit();
					}
				}
			}
			if (!flag && !this.IsNewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_AutomationInvokeFailed"));
			}
		}

		/// <summary>Scrolls the content area of a container object to display the control within the visible region (viewport) of the container.</summary>
		// Token: 0x060025EC RID: 9708 RVA: 0x000B5B81 File Offset: 0x000B3D81
		void IScrollItemProvider.ScrollIntoView()
		{
			this.OwningDataGrid.ScrollIntoView(base.Item);
		}

		/// <summary>Gets a value that indicates whether an item is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x000B5B94 File Offset: 0x000B3D94
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.OwningDataGrid.SelectedItems.Contains(base.Item);
			}
		}

		/// <summary>Gets the UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" /> and acts as the container for the calling object.</summary>
		/// <returns>The provider for the <see cref="T:System.Windows.Controls.DataGrid" /> control. </returns>
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060025EE RID: 9710 RVA: 0x000B5BAC File Offset: 0x000B3DAC
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return base.ProviderFromPeer(this._dataGridAutomationPeer);
			}
		}

		/// <summary>Adds the current element to the collection of selected items.</summary>
		// Token: 0x060025EF RID: 9711 RVA: 0x000B5BBC File Offset: 0x000B3DBC
		void ISelectionItemProvider.AddToSelection()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			object item = base.Item;
			if (this.OwningDataGrid.SelectedItems.Contains(item))
			{
				return;
			}
			this.EnsureEnabled();
			if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single && this.OwningDataGrid.SelectedItems.Count > 0)
			{
				throw new InvalidOperationException();
			}
			if (this.OwningDataGrid.Items.Contains(item))
			{
				this.OwningDataGrid.SelectedItems.Add(item);
			}
		}

		/// <summary>Removes the current element from the collection of selected items.</summary>
		// Token: 0x060025F0 RID: 9712 RVA: 0x000B5C50 File Offset: 0x000B3E50
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			this.EnsureEnabled();
			object item = base.Item;
			if (this.OwningDataGrid.SelectedItems.Contains(item))
			{
				this.OwningDataGrid.SelectedItems.Remove(item);
			}
		}

		/// <summary>Clears any selected items and then selects the current element.</summary>
		// Token: 0x060025F1 RID: 9713 RVA: 0x000B5CA6 File Offset: 0x000B3EA6
		void ISelectionItemProvider.Select()
		{
			if (!this.IsRowSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGridRow_CannotSelectRowWhenCells"));
			}
			this.EnsureEnabled();
			this.OwningDataGrid.SelectedItem = base.Item;
		}

		/// <summary>Gets a value that indicates whether the UI Automation provider allows more than one child element to be selected concurrently.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selection is allowed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000B5CD7 File Offset: 0x000B3ED7
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
		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x060025F3 RID: 9715 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		/// <summary>Retrieves a UI Automation provider for each child element that is selected.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x060025F4 RID: 9716 RVA: 0x000B5CE8 File Offset: 0x000B3EE8
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			DataGrid owningDataGrid = this.OwningDataGrid;
			if (owningDataGrid == null)
			{
				return null;
			}
			int num = owningDataGrid.Items.IndexOf(base.Item);
			if (num > -1 && owningDataGrid.SelectedCellsInternal.Intersects(num))
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				for (int i = 0; i < this.OwningDataGrid.Columns.Count; i++)
				{
					if (owningDataGrid.SelectedCellsInternal.Contains(num, i))
					{
						DataGridColumn column = owningDataGrid.ColumnFromDisplayIndex(i);
						DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(column);
						if (orCreateCellItemPeer != null)
						{
							list.Add(base.ProviderFromPeer(orCreateCellItemPeer));
						}
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000B5D8C File Offset: 0x000B3F8C
		internal List<AutomationPeer> GetCellItemPeers()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<DataGridCellItemAutomationPeer> itemPeersStorage = new ItemPeersStorage<DataGridCellItemAutomationPeer>();
			IList list2 = null;
			bool flag = false;
			DataGridRow dataGridRow = base.GetWrapper() as DataGridRow;
			if (dataGridRow != null && dataGridRow.CellsPresenter != null)
			{
				Panel itemsHost = dataGridRow.CellsPresenter.ItemsHost;
				if (itemsHost != null)
				{
					list2 = itemsHost.Children;
					flag = true;
				}
			}
			if (!flag)
			{
				list2 = this.OwningDataGrid.Columns;
			}
			if (list2 != null)
			{
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					DataGridColumn dataGridColumn;
					if (flag)
					{
						dataGridColumn = (obj as DataGridCell).Column;
					}
					else
					{
						dataGridColumn = (obj as DataGridColumn);
					}
					if (dataGridColumn != null)
					{
						DataGridCellItemAutomationPeer orCreateCellItemPeer = this.GetOrCreateCellItemPeer(dataGridColumn, false);
						list.Add(orCreateCellItemPeer);
						itemPeersStorage[dataGridColumn] = orCreateCellItemPeer;
					}
				}
			}
			this.CellItemPeers = itemPeersStorage;
			return list;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000B5E84 File Offset: 0x000B4084
		internal DataGridCellItemAutomationPeer GetOrCreateCellItemPeer(DataGridColumn column)
		{
			return this.GetOrCreateCellItemPeer(column, true);
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000B5E90 File Offset: 0x000B4090
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private DataGridCellItemAutomationPeer GetOrCreateCellItemPeer(DataGridColumn column, bool addParentInfo)
		{
			DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = this.CellItemPeers[column];
			if (dataGridCellItemAutomationPeer == null)
			{
				dataGridCellItemAutomationPeer = this.GetPeerFromWeakRefStorage(column);
				if (dataGridCellItemAutomationPeer != null && !addParentInfo)
				{
					dataGridCellItemAutomationPeer.AncestorsInvalid = false;
					dataGridCellItemAutomationPeer.ChildrenValid = false;
				}
			}
			if (dataGridCellItemAutomationPeer == null)
			{
				dataGridCellItemAutomationPeer = new DataGridCellItemAutomationPeer(base.Item, column);
				if (addParentInfo && dataGridCellItemAutomationPeer != null)
				{
					dataGridCellItemAutomationPeer.TrySetParentInfo(this);
				}
			}
			AutomationPeer owningCellPeer = dataGridCellItemAutomationPeer.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.EventsSource = dataGridCellItemAutomationPeer;
			}
			return dataGridCellItemAutomationPeer;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000B5EFC File Offset: 0x000B40FC
		private DataGridCellItemAutomationPeer GetPeerFromWeakRefStorage(object column)
		{
			DataGridCellItemAutomationPeer dataGridCellItemAutomationPeer = null;
			WeakReference weakReference = this.WeakRefElementProxyStorage[column];
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					dataGridCellItemAutomationPeer = (base.PeerFromProvider(elementProxy) as DataGridCellItemAutomationPeer);
					if (dataGridCellItemAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(column);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(column);
				}
			}
			return dataGridCellItemAutomationPeer;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000B5F58 File Offset: 0x000B4158
		internal void AddProxyToWeakRefStorage(WeakReference wr, DataGridCellItemAutomationPeer cellItemPeer)
		{
			IList<DataGridColumn> columns = this.OwningDataGrid.Columns;
			if (columns != null && columns.Contains(cellItemPeer.Column) && this.GetPeerFromWeakRefStorage(cellItemPeer.Column) == null)
			{
				this.WeakRefElementProxyStorage[cellItemPeer.Column] = wr;
			}
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000B5FA2 File Offset: 0x000B41A2
		private void EnsureEnabled()
		{
			if (!this._dataGridAutomationPeer.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x000B5FB7 File Offset: 0x000B41B7
		private bool IsRowSelectionUnit
		{
			get
			{
				return this.OwningDataGrid != null && (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.FullRow || this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader);
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000B5FE4 File Offset: 0x000B41E4
		private bool IsNewItemPlaceholder
		{
			get
			{
				object item = base.Item;
				return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x000B600C File Offset: 0x000B420C
		internal AutomationPeer RowHeaderAutomationPeer
		{
			get
			{
				DataGridRowAutomationPeer dataGridRowAutomationPeer = this.GetWrapperPeer() as DataGridRowAutomationPeer;
				if (dataGridRowAutomationPeer == null)
				{
					return null;
				}
				return dataGridRowAutomationPeer.RowHeaderAutomationPeer;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x000B6030 File Offset: 0x000B4230
		private DataGrid OwningDataGrid
		{
			get
			{
				DataGridAutomationPeer dataGridAutomationPeer = this._dataGridAutomationPeer as DataGridAutomationPeer;
				return (DataGrid)dataGridAutomationPeer.Owner;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x000B6054 File Offset: 0x000B4254
		// (set) Token: 0x06002600 RID: 9728 RVA: 0x000B605C File Offset: 0x000B425C
		private ItemPeersStorage<DataGridCellItemAutomationPeer> CellItemPeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06002601 RID: 9729 RVA: 0x000B6065 File Offset: 0x000B4265
		private ItemPeersStorage<WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._weakRefElementProxyStorage;
			}
		}

		// Token: 0x04001B6B RID: 7019
		private AutomationPeer _dataGridAutomationPeer;

		// Token: 0x04001B6C RID: 7020
		private ItemPeersStorage<DataGridCellItemAutomationPeer> _dataChildren = new ItemPeersStorage<DataGridCellItemAutomationPeer>();

		// Token: 0x04001B6D RID: 7021
		private ItemPeersStorage<WeakReference> _weakRefElementProxyStorage = new ItemPeersStorage<WeakReference>();
	}
}
