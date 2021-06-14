using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the cells in a <see cref="T:System.Windows.Controls.GridView" /> to UI Automation.</summary>
	// Token: 0x020002BB RID: 699
	public class GridViewCellAutomationPeer : FrameworkElementAutomationPeer, ITableItemProvider, IGridItemProvider
	{
		// Token: 0x060026C9 RID: 9929 RVA: 0x000B8144 File Offset: 0x000B6344
		internal GridViewCellAutomationPeer(ContentPresenter owner, ListViewAutomationPeer parent) : base(owner)
		{
			Invariant.Assert(parent != null);
			this._listviewAP = parent;
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000B8144 File Offset: 0x000B6344
		internal GridViewCellAutomationPeer(TextBlock owner, ListViewAutomationPeer parent) : base(owner)
		{
			Invariant.Assert(parent != null);
			this._listviewAP = parent;
		}

		/// <summary>Gets the name of the element that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The name of the element.</returns>
		// Token: 0x060026CB RID: 9931 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		/// <summary>Gets the control type for the element that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>If this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" /> is associated with a <see cref="T:System.Windows.Controls.TextBlock" /> element, this method returns <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Text" />; otherwise, this method returns <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" />.</returns>
		// Token: 0x060026CC RID: 9932 RVA: 0x000B815D File Offset: 0x000B635D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if (base.Owner is TextBlock)
			{
				return AutomationControlType.Text;
			}
			return AutomationControlType.Custom;
		}

		/// <summary>Gets the control pattern for the element that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.GridItem" /> or <see cref="F:System.Windows.Automation.Peers.PatternInterface.TableItem" />, this method returns the current <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" />.</returns>
		// Token: 0x060026CD RID: 9933 RVA: 0x000B8171 File Offset: 0x000B6371
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.GridItem || patternInterface == PatternInterface.TableItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets or sets a value that indicates whether the element that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" /> is understood by the end user as interactive or the user might understand the element as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>If this <see cref="T:System.Windows.Automation.Peers.GridViewCellAutomationPeer" /> is associated with a <see cref="T:System.Windows.Controls.TextBlock" /> element, this method returns <see langword="true" />; otherwise, this method returns a list of child elements.</returns>
		// Token: 0x060026CE RID: 9934 RVA: 0x000B8188 File Offset: 0x000B6388
		protected override bool IsControlElementCore()
		{
			bool includeInvisibleElementsInControlView = base.IncludeInvisibleElementsInControlView;
			if (base.Owner is TextBlock)
			{
				return includeInvisibleElementsInControlView || base.Owner.IsVisible;
			}
			List<AutomationPeer> childrenAutomationPeer = this.GetChildrenAutomationPeer(base.Owner, includeInvisibleElementsInControlView);
			return childrenAutomationPeer != null && childrenAutomationPeer.Count >= 1;
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x000B81D9 File Offset: 0x000B63D9
		// (set) Token: 0x060026D0 RID: 9936 RVA: 0x000B81E1 File Offset: 0x000B63E1
		internal int Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000B81EA File Offset: 0x000B63EA
		// (set) Token: 0x060026D2 RID: 9938 RVA: 0x000B81F2 File Offset: 0x000B63F2
		internal int Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
			}
		}

		/// <summary>Retrieves a collection of UI Automation providers that represent all the row headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x060026D3 RID: 9939 RVA: 0x000B7D70 File Offset: 0x000B5F70
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			return new IRawElementProviderSimple[0];
		}

		/// <summary>Retrieves a collection of UI Automation providers that represent all the column headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x060026D4 RID: 9940 RVA: 0x000B81FC File Offset: 0x000B63FC
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			ListView listView = this._listviewAP.Owner as ListView;
			if (listView != null && listView.View is GridView)
			{
				GridView gridView = listView.View as GridView;
				if (gridView.HeaderRowPresenter != null && gridView.HeaderRowPresenter.ActualColumnHeaders.Count > this.Column)
				{
					GridViewColumnHeader element = gridView.HeaderRowPresenter.ActualColumnHeaders[this.Column];
					AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(element);
					if (automationPeer != null)
					{
						return new IRawElementProviderSimple[]
						{
							base.ProviderFromPeer(automationPeer)
						};
					}
				}
			}
			return new IRawElementProviderSimple[0];
		}

		/// <summary>Gets the ordinal number of the row that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the row containing the cell or item.</returns>
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000B828E File Offset: 0x000B648E
		int IGridItemProvider.Row
		{
			get
			{
				return this.Row;
			}
		}

		/// <summary>Gets the ordinal number of the column that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the column containing the cell or item.</returns>
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060026D6 RID: 9942 RVA: 0x000B8296 File Offset: 0x000B6496
		int IGridItemProvider.Column
		{
			get
			{
				return this.Column;
			}
		}

		/// <summary>Gets the number of rows spanned by a cell or item.</summary>
		/// <returns>The number of rows spanned.</returns>
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.RowSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets the number of columns spanned by a cell or item.</summary>
		/// <returns>The number of columns spanned.</returns>
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x00016748 File Offset: 0x00014948
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		/// <summary>Gets a UI Automation provider that implements <see cref="T:System.Windows.Automation.GridPattern" /> and represents the container of the cell or item.</summary>
		/// <returns>A UI Automation provider that implements the <see cref="T:System.Windows.Automation.GridPattern" /> and represents the cell or item container.</returns>
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x000B829E File Offset: 0x000B649E
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return base.ProviderFromPeer(this._listviewAP);
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000B82AC File Offset: 0x000B64AC
		private List<AutomationPeer> GetChildrenAutomationPeer(Visual parent, bool includeInvisibleItems)
		{
			Invariant.Assert(parent != null);
			List<AutomationPeer> children = null;
			GridViewCellAutomationPeer.iterate(parent, includeInvisibleItems, delegate(AutomationPeer peer)
			{
				if (children == null)
				{
					children = new List<AutomationPeer>();
				}
				children.Add(peer);
				return false;
			});
			return children;
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000B82EC File Offset: 0x000B64EC
		private static bool iterate(Visual parent, bool includeInvisibleItems, GridViewCellAutomationPeer.IteratorCallback callback)
		{
			bool flag = false;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int num = 0;
			while (num < internalVisualChildrenCount && !flag)
			{
				Visual visual = parent.InternalGetVisualChild(num);
				AutomationPeer peer;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (includeInvisibleItems || ((UIElement)visual).IsVisible) && (peer = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					flag = callback(peer);
				}
				else
				{
					flag = GridViewCellAutomationPeer.iterate(visual, includeInvisibleItems, callback);
				}
				num++;
			}
			return flag;
		}

		// Token: 0x04001B7C RID: 7036
		private ListViewAutomationPeer _listviewAP;

		// Token: 0x04001B7D RID: 7037
		private int _column;

		// Token: 0x04001B7E RID: 7038
		private int _row;

		// Token: 0x020008B7 RID: 2231
		// (Invoke) Token: 0x0600843A RID: 33850
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
