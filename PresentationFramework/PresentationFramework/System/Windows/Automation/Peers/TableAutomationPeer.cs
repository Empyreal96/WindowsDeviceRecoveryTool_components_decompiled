using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Documents.Table" /> types to UI Automation.</summary>
	// Token: 0x020002E6 RID: 742
	public class TableAutomationPeer : TextElementAutomationPeer, IGridProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Documents.Table" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" />.</param>
		// Token: 0x0600281A RID: 10266 RVA: 0x000BB9B3 File Offset: 0x000B9BB3
		public TableAutomationPeer(Table owner) : base(owner)
		{
			this._rowCount = this.GetRowCount();
			this._columnCount = this.GetColumnCount();
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Documents.Table" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value from the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Grid" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x0600281B RID: 10267 RVA: 0x000BB9D4 File Offset: 0x000B9BD4
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Grid)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Documents.Table" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Table" /> enumeration value.</returns>
		// Token: 0x0600281C RID: 10268 RVA: 0x00094DDC File Offset: 0x00092FDC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Table;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Documents.Table" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Table".</returns>
		// Token: 0x0600281D RID: 10269 RVA: 0x000BB9E3 File Offset: 0x000B9BE3
		protected override string GetClassNameCore()
		{
			return "Table";
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Documents.Table" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableAutomationPeer" /> is understood by the end user as interactive or as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>
		///     <see langword="true" />.</returns>
		// Token: 0x0600281E RID: 10270 RVA: 0x000BB9EC File Offset: 0x000B9BEC
		protected override bool IsControlElementCore()
		{
			return base.IncludeInvisibleElementsInControlView || base.IsTextViewVisible == true;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000BBA20 File Offset: 0x000B9C20
		internal void OnStructureInvalidated()
		{
			int rowCount = this.GetRowCount();
			if (rowCount != this._rowCount)
			{
				base.RaisePropertyChangedEvent(GridPatternIdentifiers.RowCountProperty, this._rowCount, rowCount);
				this._rowCount = rowCount;
			}
			int columnCount = this.GetColumnCount();
			if (columnCount != this._columnCount)
			{
				base.RaisePropertyChangedEvent(GridPatternIdentifiers.ColumnCountProperty, this._columnCount, columnCount);
				this._columnCount = columnCount;
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000BBA94 File Offset: 0x000B9C94
		private int GetRowCount()
		{
			int num = 0;
			foreach (TableRowGroup tableRowGroup in ((IEnumerable<TableRowGroup>)((Table)base.Owner).RowGroups))
			{
				num += tableRowGroup.Rows.Count;
			}
			return num;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000BBAF8 File Offset: 0x000B9CF8
		private int GetColumnCount()
		{
			return ((Table)base.Owner).ColumnCount;
		}

		/// <summary>Retrieves the UI Automation provider for the specified cell.</summary>
		/// <param name="row"> The ordinal number of the row of interest.</param>
		/// <param name="column"> The ordinal number of the column of interest.</param>
		/// <returns>The UI Automation provider for the specified cell.</returns>
		// Token: 0x06002822 RID: 10274 RVA: 0x000BBB0C File Offset: 0x000B9D0C
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (row < 0 || row >= ((IGridProvider)this).RowCount)
			{
				throw new ArgumentOutOfRangeException("row");
			}
			if (column < 0 || column >= ((IGridProvider)this).ColumnCount)
			{
				throw new ArgumentOutOfRangeException("column");
			}
			int num = 0;
			Table table = (Table)base.Owner;
			foreach (TableRowGroup tableRowGroup in ((IEnumerable<TableRowGroup>)table.RowGroups))
			{
				if (num + tableRowGroup.Rows.Count < row)
				{
					num += tableRowGroup.Rows.Count;
				}
				else
				{
					foreach (TableRow tableRow in ((IEnumerable<TableRow>)tableRowGroup.Rows))
					{
						if (num == row)
						{
							foreach (TableCell tableCell in ((IEnumerable<TableCell>)tableRow.Cells))
							{
								if (tableCell.ColumnIndex <= column && tableCell.ColumnIndex + tableCell.ColumnSpan > column)
								{
									return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(tableCell));
								}
							}
							foreach (TableCell tableCell2 in tableRow.SpannedCells)
							{
								if (tableCell2.ColumnIndex <= column && tableCell2.ColumnIndex + tableCell2.ColumnSpan > column)
								{
									return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(tableCell2));
								}
							}
						}
						else
						{
							num++;
						}
					}
				}
			}
			return null;
		}

		/// <summary>Gets the total number of rows in a grid.</summary>
		/// <returns>The total number of rows in a grid.</returns>
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000BBCEC File Offset: 0x000B9EEC
		int IGridProvider.RowCount
		{
			get
			{
				return this._rowCount;
			}
		}

		/// <summary>Gets the total number of columns in a grid.</summary>
		/// <returns>The total number of columns in a grid.</returns>
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x000BBCF4 File Offset: 0x000B9EF4
		int IGridProvider.ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x04001B95 RID: 7061
		private int _rowCount;

		// Token: 0x04001B96 RID: 7062
		private int _columnCount;
	}
}
