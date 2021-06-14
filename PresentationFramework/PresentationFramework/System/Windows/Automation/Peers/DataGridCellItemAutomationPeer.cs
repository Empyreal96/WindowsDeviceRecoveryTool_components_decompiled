using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridCell" /> types to UI Automation.</summary>
	// Token: 0x020002A1 RID: 673
	public sealed class DataGridCellItemAutomationPeer : AutomationPeer, IGridItemProvider, ITableItemProvider, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, IValueProvider, IVirtualizedItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> class. </summary>
		/// <param name="item">The element that is associated with this automation peer.</param>
		/// <param name="dataGridColumn">The <see cref="T:System.Windows.Controls.DataGrid" /> column that <paramref name="item" /> is in. </param>
		// Token: 0x06002582 RID: 9602 RVA: 0x000B47D3 File Offset: 0x000B29D3
		public DataGridCellItemAutomationPeer(object item, DataGridColumn dataGridColumn)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (dataGridColumn == null)
			{
				throw new ArgumentNullException("dataGridColumn");
			}
			this._item = new WeakReference(item);
			this._column = dataGridColumn;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000B480C File Offset: 0x000B2A0C
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000B4838 File Offset: 0x000B2A38
		protected override string GetAccessKeyCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000B4864 File Offset: 0x000B2A64
		protected override string GetAutomationIdCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetAutomationId();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000B4890 File Offset: 0x000B2A90
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000B48C0 File Offset: 0x000B2AC0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.ForceEnsureChildren();
				return owningCellPeer.GetChildren();
			}
			return null;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000B48E8 File Offset: 0x000B2AE8
		protected override string GetClassNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClassName();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000B4914 File Offset: 0x000B2B14
		protected override Point GetClickablePointCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000B4950 File Offset: 0x000B2B50
		protected override string GetHelpTextCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetHelpText();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x000B497C File Offset: 0x000B2B7C
		protected override string GetItemStatusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000B49A8 File Offset: 0x000B2BA8
		protected override string GetItemTypeCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetItemType();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000B49D4 File Offset: 0x000B2BD4
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000B49F9 File Offset: 0x000B2BF9
		protected override string GetLocalizedControlTypeCore()
		{
			if (!AccessibilitySwitches.UseNetFx47CompatibleAccessibilityFeatures)
			{
				return SR.Get("DataGridCellItemAutomationPeer_LocalizedControlType");
			}
			return base.GetLocalizedControlTypeCore();
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000B4A14 File Offset: 0x000B2C14
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			AutomationLiveSetting result = AutomationLiveSetting.Off;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetLiveSetting();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000B4A40 File Offset: 0x000B2C40
		protected override string GetNameCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			string text = null;
			if (owningCellPeer != null)
			{
				text = owningCellPeer.GetName();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = SR.Get("DataGridCellItemAutomationPeer_NameCoreFormat", new object[]
				{
					this.Item,
					this._column.DisplayIndex
				});
			}
			return text;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000B4A98 File Offset: 0x000B2C98
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration that specifies the control pattern.</param>
		/// <returns>The current <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> object, if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. For more information, see Remarks.</returns>
		// Token: 0x06002593 RID: 9619 RVA: 0x000B4AC0 File Offset: 0x000B2CC0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface != PatternInterface.Value)
					{
						if (patternInterface != PatternInterface.ScrollItem)
						{
							goto IL_8C;
						}
					}
					else
					{
						if (!this.IsNewItemPlaceholder)
						{
							return this;
						}
						goto IL_8C;
					}
				}
				else
				{
					if (!this.OwningDataGrid.IsReadOnly && !this._column.IsReadOnly)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface <= PatternInterface.SelectionItem)
			{
				if (patternInterface != PatternInterface.GridItem)
				{
					if (patternInterface != PatternInterface.SelectionItem)
					{
						goto IL_8C;
					}
					if (this.IsCellSelectionUnit)
					{
						return this;
					}
					goto IL_8C;
				}
			}
			else if (patternInterface != PatternInterface.TableItem)
			{
				if (patternInterface != PatternInterface.VirtualizedItem)
				{
					goto IL_8C;
				}
				if (VirtualizedItemPatternIdentifiers.Pattern == null)
				{
					goto IL_8C;
				}
				if (this.OwningCellPeer == null)
				{
					return this;
				}
				if (this.OwningItemPeer != null && !this.IsItemInAutomationTree())
				{
					return this;
				}
				if (this.OwningItemPeer == null)
				{
					return this;
				}
				goto IL_8C;
			}
			return this;
			IL_8C:
			return null;
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000B4B5C File Offset: 0x000B2D5C
		protected override int GetPositionInSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetPositionInSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000B4B88 File Offset: 0x000B2D88
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			int result = -1;
			if (owningCellPeer != null)
			{
				result = owningCellPeer.GetSizeOfSet();
			}
			else
			{
				this.ThrowElementNotAvailableException();
			}
			return result;
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000B4BB4 File Offset: 0x000B2DB4
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.GetVisibleBoundingRectCore();
			}
			return base.GetBoundingRectangle();
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000B4BD8 File Offset: 0x000B2DD8
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000B4C00 File Offset: 0x000B2E00
		protected override bool IsContentElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsContentElement();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000B4C20 File Offset: 0x000B2E20
		protected override bool IsControlElementCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			return owningCellPeer == null || owningCellPeer.IsControlElement();
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000B4C40 File Offset: 0x000B2E40
		protected override bool IsEnabledCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000B4C68 File Offset: 0x000B2E68
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000B4C90 File Offset: 0x000B2E90
		protected override bool IsOffscreenCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000B4CB8 File Offset: 0x000B2EB8
		protected override bool IsPasswordCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000B4CE0 File Offset: 0x000B2EE0
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				return owningCellPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x000B4D08 File Offset: 0x000B2F08
		protected override void SetFocusCore()
		{
			AutomationPeer owningCellPeer = this.OwningCellPeer;
			if (owningCellPeer != null)
			{
				owningCellPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000B4D2C File Offset: 0x000B2F2C
		internal override void AddToParentProxyWeakRefCache()
		{
			DataGridItemAutomationPeer owningItemPeer = this.OwningItemPeer;
			if (owningItemPeer != null)
			{
				owningItemPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		/// <summary>Gets the ordinal number of the column that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the column containing the cell or item.</returns>
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000B4D50 File Offset: 0x000B2F50
		int IGridItemProvider.Column
		{
			get
			{
				return this.OwningDataGrid.Columns.IndexOf(this._column);
			}
		}

		/// <summary>Gets the number of columns spanned by a cell or item.</summary>
		/// <returns>The number of columns spanned. </returns>
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets a UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.IGridProvider" /> and represents the container of the cell or item.</summary>
		/// <returns>A UI Automation provider that implements the <see cref="T:System.Windows.Automation.GridPattern" /> and represents the cell or item container. </returns>
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000B4D68 File Offset: 0x000B2F68
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		/// <summary>Gets the ordinal number of the row that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the row containing the cell or item. </returns>
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000B4D70 File Offset: 0x000B2F70
		int IGridItemProvider.Row
		{
			get
			{
				return this.OwningDataGrid.Items.IndexOf(this.Item);
			}
		}

		/// <summary>Gets the number of rows spanned by a cell or item.</summary>
		/// <returns>The number of rows spanned. </returns>
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.RowSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Retrieves a collection of UI Automation providers representing all the column headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x060025A7 RID: 9639 RVA: 0x000B4D88 File Offset: 0x000B2F88
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column && this.OwningDataGrid.ColumnHeadersPresenter != null)
			{
				DataGridColumnHeadersPresenterAutomationPeer dataGridColumnHeadersPresenterAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid.ColumnHeadersPresenter) as DataGridColumnHeadersPresenterAutomationPeer;
				if (dataGridColumnHeadersPresenterAutomationPeer != null)
				{
					AutomationPeer automationPeer = dataGridColumnHeadersPresenterAutomationPeer.FindOrCreateItemAutomationPeer(this._column);
					if (automationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(automationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		/// <summary>Retrieves a collection of UI Automation providers representing all the row headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x060025A8 RID: 9640 RVA: 0x000B4E00 File Offset: 0x000B3000
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			if (this.OwningDataGrid != null && (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
				DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(this.Item) as DataGridItemAutomationPeer;
				if (dataGridItemAutomationPeer != null)
				{
					AutomationPeer rowHeaderAutomationPeer = dataGridItemAutomationPeer.RowHeaderAutomationPeer;
					if (rowHeaderAutomationPeer != null)
					{
						return new List<IRawElementProviderSimple>(1)
						{
							base.ProviderFromPeer(rowHeaderAutomationPeer)
						}.ToArray();
					}
				}
			}
			return null;
		}

		/// <summary>Sends a request to activate a control and initiate its single, unambiguous action.</summary>
		// Token: 0x060025A9 RID: 9641 RVA: 0x000B4E74 File Offset: 0x000B3074
		void IInvokeProvider.Invoke()
		{
			if (this.OwningDataGrid.IsReadOnly || this._column.IsReadOnly)
			{
				return;
			}
			this.EnsureEnabled();
			bool flag = false;
			if (this.OwningCell == null)
			{
				this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
			}
			DataGridCell owningCell = this.OwningCell;
			if (owningCell != null)
			{
				if (!owningCell.IsEditing)
				{
					if (!owningCell.IsKeyboardFocusWithin)
					{
						owningCell.Focus();
					}
					this.OwningDataGrid.HandleSelectionForCellInput(owningCell, false, false, false);
					flag = this.OwningDataGrid.BeginEdit();
				}
				else
				{
					flag = true;
				}
			}
			if (!flag && !this.IsNewItemPlaceholder)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_AutomationInvokeFailed"));
			}
		}

		/// <summary>Scrolls the content area of a container object in order to display the control within the visible region (viewport) of the container.</summary>
		// Token: 0x060025AA RID: 9642 RVA: 0x000B4F1E File Offset: 0x000B311E
		void IScrollItemProvider.ScrollIntoView()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		/// <summary>Gets a value that indicates whether an item is selected. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000B4F37 File Offset: 0x000B3137
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.OwningDataGrid.SelectedCellsInternal.Contains(new DataGridCellInfo(this.Item, this._column));
			}
		}

		/// <summary>Gets the UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" /> and acts as the container for the calling object.</summary>
		/// <returns>The provider that supports <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" />. </returns>
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x000B4D68 File Offset: 0x000B2F68
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.ContainingGrid;
			}
		}

		/// <summary>Adds the current element to the collection of selected items.</summary>
		// Token: 0x060025AD RID: 9645 RVA: 0x000B4F5C File Offset: 0x000B315C
		void ISelectionItemProvider.AddToSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				return;
			}
			this.EnsureEnabled();
			if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single && this.OwningDataGrid.SelectedCells.Count > 0)
			{
				throw new InvalidOperationException();
			}
			this.OwningDataGrid.SelectedCellsInternal.Add(cell);
		}

		/// <summary>Removes the current element from the collection of selected items.</summary>
		// Token: 0x060025AE RID: 9646 RVA: 0x000B4FE8 File Offset: 0x000B31E8
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo cell = new DataGridCellInfo(this.Item, this._column);
			if (this.OwningDataGrid.SelectedCellsInternal.Contains(cell))
			{
				this.OwningDataGrid.SelectedCellsInternal.Remove(cell);
			}
		}

		/// <summary>Deselects any selected items and then selects the current element.</summary>
		// Token: 0x060025AF RID: 9647 RVA: 0x000B504C File Offset: 0x000B324C
		void ISelectionItemProvider.Select()
		{
			if (!this.IsCellSelectionUnit)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			this.EnsureEnabled();
			DataGridCellInfo currentCellInfo = new DataGridCellInfo(this.Item, this._column);
			this.OwningDataGrid.SelectOnlyThisCell(currentCellInfo);
		}

		/// <summary>Gets a value that specifies whether the value of a control is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified. </returns>
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000B5096 File Offset: 0x000B3296
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return this._column.IsReadOnly;
			}
		}

		/// <summary>Sets the value of a control.</summary>
		/// <param name="value">The value to set. The provider is responsible for converting the value to the appropriate data type.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.DataGridCell" /> object that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridCellItemAutomationPeer" /> object is read-only.</exception>
		// Token: 0x060025B1 RID: 9649 RVA: 0x000B50A3 File Offset: 0x000B32A3
		void IValueProvider.SetValue(string value)
		{
			if (this._column.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_ColumnIsReadOnly"));
			}
			if (this.OwningDataGrid != null)
			{
				this.OwningDataGrid.SetCellAutomationValue(this.Item, this._column, value);
			}
		}

		/// <summary>Gets the value of the control.</summary>
		/// <returns>The value of the control as a string. </returns>
		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000B50E2 File Offset: 0x000B32E2
		string IValueProvider.Value
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					return this.OwningDataGrid.GetCellAutomationValue(this.Item, this._column);
				}
				return null;
			}
		}

		/// <summary>Makes the virtual item fully accessible as a UI Automation element.</summary>
		// Token: 0x060025B3 RID: 9651 RVA: 0x000B4F1E File Offset: 0x000B311E
		void IVirtualizedItemProvider.Realize()
		{
			this.OwningDataGrid.ScrollIntoView(this.Item, this._column);
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000B5105 File Offset: 0x000B3305
		private void EnsureEnabled()
		{
			if (!this.OwningDataGrid.IsEnabled)
			{
				throw new ElementNotEnabledException();
			}
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000B511A File Offset: 0x000B331A
		private void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null && !this.IsItemInAutomationTree())
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000B513C File Offset: 0x000B333C
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000B518E File Offset: 0x000B338E
		private bool IsCellSelectionUnit
		{
			get
			{
				return this.OwningDataGrid != null && (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.Cell || this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader);
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000B51B8 File Offset: 0x000B33B8
		private bool IsNewItemPlaceholder
		{
			get
			{
				object item = this.Item;
				return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000B51DE File Offset: 0x000B33DE
		private DataGrid OwningDataGrid
		{
			get
			{
				return this._column.DataGridOwner;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000B51EC File Offset: 0x000B33EC
		private DataGridCell OwningCell
		{
			get
			{
				DataGrid owningDataGrid = this.OwningDataGrid;
				if (owningDataGrid == null)
				{
					return null;
				}
				return owningDataGrid.TryFindCell(this.Item, this._column);
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x000B5218 File Offset: 0x000B3418
		internal DataGridCellAutomationPeer OwningCellPeer
		{
			get
			{
				DataGridCellAutomationPeer dataGridCellAutomationPeer = null;
				DataGridCell owningCell = this.OwningCell;
				if (owningCell != null)
				{
					dataGridCellAutomationPeer = (UIElementAutomationPeer.CreatePeerForElement(owningCell) as DataGridCellAutomationPeer);
					dataGridCellAutomationPeer.EventsSource = this;
				}
				return dataGridCellAutomationPeer;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000B5248 File Offset: 0x000B3448
		private IRawElementProviderSimple ContainingGrid
		{
			get
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid);
				if (automationPeer != null)
				{
					return base.ProviderFromPeer(automationPeer);
				}
				return null;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x000B526D File Offset: 0x000B346D
		internal DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000B5275 File Offset: 0x000B3475
		internal object Item
		{
			get
			{
				if (this._item != null)
				{
					return this._item.Target;
				}
				return null;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000B528C File Offset: 0x000B348C
		private DataGridItemAutomationPeer OwningItemPeer
		{
			get
			{
				if (this.OwningDataGrid != null)
				{
					DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
					if (dataGridAutomationPeer != null)
					{
						return dataGridAutomationPeer.GetExistingPeerByItem(this.Item, true) as DataGridItemAutomationPeer;
					}
				}
				return null;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000B52C9 File Offset: 0x000B34C9
		// (set) Token: 0x060025C1 RID: 9665 RVA: 0x000B52D4 File Offset: 0x000B34D4
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer owningCellPeer = this.OwningCellPeer;
				if (owningCellPeer != null)
				{
					owningCellPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x04001B68 RID: 7016
		private WeakReference _item;

		// Token: 0x04001B69 RID: 7017
		private DataGridColumn _column;
	}
}
