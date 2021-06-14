using System;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Documents.TableCell" /> types to UI Automation.</summary>
	// Token: 0x020002E7 RID: 743
	public class TableCellAutomationPeer : TextElementAutomationPeer, IGridItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />.</param>
		// Token: 0x06002825 RID: 10277 RVA: 0x000B8A79 File Offset: 0x000B6C79
		public TableCellAutomationPeer(TableCell owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value from the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.GridItem" />, this method returns the current instance of the <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x06002826 RID: 10278 RVA: 0x000BBCFC File Offset: 0x000B9EFC
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.GridItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x06002827 RID: 10279 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		/// <summary>Gets the localized version of the control type for the <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />.</summary>
		/// <returns>A string that contains "cell".</returns>
		// Token: 0x06002828 RID: 10280 RVA: 0x000BBD0B File Offset: 0x000B9F0B
		protected override string GetLocalizedControlTypeCore()
		{
			return "cell";
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TableCell".</returns>
		// Token: 0x06002829 RID: 10281 RVA: 0x000BBD12 File Offset: 0x000B9F12
		protected override string GetClassNameCore()
		{
			return "TableCell";
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Documents.TableCell" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TableCellAutomationPeer" /> is understood by the end user as interactive or as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>
		///     <see langword="true" />.</returns>
		// Token: 0x0600282A RID: 10282 RVA: 0x000BBD1C File Offset: 0x000B9F1C
		protected override bool IsControlElementCore()
		{
			return base.IncludeInvisibleElementsInControlView || base.IsTextViewVisible == true;
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000BBD4F File Offset: 0x000B9F4F
		internal void OnColumnSpanChanged(int oldValue, int newValue)
		{
			base.RaisePropertyChangedEvent(GridItemPatternIdentifiers.ColumnSpanProperty, oldValue, newValue);
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000BBD68 File Offset: 0x000B9F68
		internal void OnRowSpanChanged(int oldValue, int newValue)
		{
			base.RaisePropertyChangedEvent(GridItemPatternIdentifiers.RowSpanProperty, oldValue, newValue);
		}

		/// <summary>Gets the ordinal number of the row that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the row containing the cell or item.</returns>
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000BBD81 File Offset: 0x000B9F81
		int IGridItemProvider.Row
		{
			get
			{
				return ((TableCell)base.Owner).RowIndex;
			}
		}

		/// <summary>Gets the ordinal number of the column that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the column containing the cell or item.</returns>
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000BBD93 File Offset: 0x000B9F93
		int IGridItemProvider.Column
		{
			get
			{
				return ((TableCell)base.Owner).ColumnIndex;
			}
		}

		/// <summary>Gets the number of rows spanned by a cell or item.</summary>
		/// <returns>The number of rows spanned.</returns>
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000BBDA5 File Offset: 0x000B9FA5
		int IGridItemProvider.RowSpan
		{
			get
			{
				return ((TableCell)base.Owner).RowSpan;
			}
		}

		/// <summary>Gets the number of columns spanned by a cell or item.</summary>
		/// <returns>The number of columns spanned.</returns>
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000BBDB7 File Offset: 0x000B9FB7
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return ((TableCell)base.Owner).ColumnSpan;
			}
		}

		/// <summary>Gets a UI Automation provider that implements <see cref="T:System.Windows.Automation.GridPattern" /> and represents the container of the cell or item.</summary>
		/// <returns>A UI Automation provider that implements the <see cref="T:System.Windows.Automation.GridPattern" /> and represents the cell or item container.</returns>
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000BBDC9 File Offset: 0x000B9FC9
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				if ((TableCell)base.Owner != null)
				{
					return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(((TableCell)base.Owner).Table));
				}
				return null;
			}
		}
	}
}
