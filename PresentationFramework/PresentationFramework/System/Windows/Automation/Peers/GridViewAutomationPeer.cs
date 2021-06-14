using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GridView" /> types to UI Automation.</summary>
	// Token: 0x020002B8 RID: 696
	public class GridViewAutomationPeer : IViewAutomationPeer, ITableProvider, IGridProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GridView" /> associated with this <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" />.</param>
		/// <param name="listview">The <see cref="T:System.Windows.Controls.ListView" /> that the <see cref="T:System.Windows.Controls.GridView" /> is using as a view mode.</param>
		// Token: 0x060026A9 RID: 9897 RVA: 0x000B7B18 File Offset: 0x000B5D18
		public GridViewAutomationPeer(GridView owner, ListView listview)
		{
			Invariant.Assert(owner != null);
			Invariant.Assert(listview != null);
			this._owner = owner;
			this._listview = listview;
			this._oldItemsCount = this._listview.Items.Count;
			this._oldColumnsCount = this._owner.Columns.Count;
			((INotifyCollectionChanged)this._owner.Columns).CollectionChanged += this.OnColumnCollectionChanged;
		}

		/// <summary>Gets the control type for the element that is associated with this <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" />.</summary>
		/// <returns>A value in the enumeration.</returns>
		// Token: 0x060026AA RID: 9898 RVA: 0x000962DF File Offset: 0x000944DF
		AutomationControlType IViewAutomationPeer.GetAutomationControlType()
		{
			return AutomationControlType.DataGrid;
		}

		/// <summary>Gets the control pattern that is associated with the specified <paramref name="patternInterface" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Grid" /> or <see cref="F:System.Windows.Automation.Peers.PatternInterface.Table" />, this method returns an object that implements the control pattern, otherwise this method returns <see langword="null" />.</returns>
		// Token: 0x060026AB RID: 9899 RVA: 0x000B7B94 File Offset: 0x000B5D94
		object IViewAutomationPeer.GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Grid || patternInterface == PatternInterface.Table)
			{
				result = this;
			}
			return result;
		}

		/// <summary>Gets the collection of immediate child elements of the specified UI Automation peer.</summary>
		/// <param name="children">Collection of child objects you want to get.</param>
		/// <returns>Collection of child objects.</returns>
		// Token: 0x060026AC RID: 9900 RVA: 0x000B7BB0 File Offset: 0x000B5DB0
		List<AutomationPeer> IViewAutomationPeer.GetChildren(List<AutomationPeer> children)
		{
			if (this._owner.HeaderRowPresenter != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this._owner.HeaderRowPresenter);
				if (automationPeer != null)
				{
					if (children == null)
					{
						children = new List<AutomationPeer>();
					}
					children.Insert(0, automationPeer);
				}
			}
			return children;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</summary>
		/// <param name="item">Item to create.</param>
		/// <returns>The item created.</returns>
		// Token: 0x060026AD RID: 9901 RVA: 0x000B7BF4 File Offset: 0x000B5DF4
		ItemAutomationPeer IViewAutomationPeer.CreateItemAutomationPeer(object item)
		{
			ListViewAutomationPeer listviewAP = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
			return new GridViewItemAutomationPeer(item, listviewAP);
		}

		/// <summary>Called when the collection of items changes.</summary>
		/// <param name="e">Data associated with the event.</param>
		// Token: 0x060026AE RID: 9902 RVA: 0x000B7C1C File Offset: 0x000B5E1C
		void IViewAutomationPeer.ItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null)
			{
				if (this._oldItemsCount != this._listview.Items.Count)
				{
					listViewAutomationPeer.RaisePropertyChangedEvent(GridPatternIdentifiers.RowCountProperty, this._oldItemsCount, this._listview.Items.Count);
				}
				this._oldItemsCount = this._listview.Items.Count;
			}
		}

		/// <summary>Called when the custom view is no longer applied to the control.</summary>
		// Token: 0x060026AF RID: 9903 RVA: 0x000B7C96 File Offset: 0x000B5E96
		[MethodImpl(MethodImplOptions.NoInlining)]
		void IViewAutomationPeer.ViewDetached()
		{
			((INotifyCollectionChanged)this._owner.Columns).CollectionChanged -= this.OnColumnCollectionChanged;
		}

		/// <summary>Gets the primary direction of traversal for the table.</summary>
		/// <returns>The primary direction of traversal.  </returns>
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x0000B02A File Offset: 0x0000922A
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		/// <summary>Returns a collection of UI Automation providers that represents all the column headers in a table.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x060026B1 RID: 9905 RVA: 0x000B7CB4 File Offset: 0x000B5EB4
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if (this._owner.HeaderRowPresenter != null)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>(this._owner.HeaderRowPresenter.ActualColumnHeaders.Count);
				ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
				if (listViewAutomationPeer != null)
				{
					foreach (UIElement element in this._owner.HeaderRowPresenter.ActualColumnHeaders)
					{
						AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(element);
						if (automationPeer != null)
						{
							list.Add(ElementProxy.StaticWrap(automationPeer, listViewAutomationPeer));
						}
					}
				}
				return list.ToArray();
			}
			return new IRawElementProviderSimple[0];
		}

		/// <summary>Returns a collection of UI Automation providers that represents all row headers in the table.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x060026B2 RID: 9906 RVA: 0x000B7D70 File Offset: 0x000B5F70
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			return new IRawElementProviderSimple[0];
		}

		/// <summary>Gets the total number of columns in a grid.</summary>
		/// <returns>The total number of columns in a grid.</returns>
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000B7D78 File Offset: 0x000B5F78
		int IGridProvider.ColumnCount
		{
			get
			{
				if (this._owner.HeaderRowPresenter != null)
				{
					return this._owner.HeaderRowPresenter.ActualColumnHeaders.Count;
				}
				return this._owner.Columns.Count;
			}
		}

		/// <summary>Gets the total number of rows in a grid.</summary>
		/// <returns>The total number of rows in a grid.</returns>
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000B7DAD File Offset: 0x000B5FAD
		int IGridProvider.RowCount
		{
			get
			{
				return this._listview.Items.Count;
			}
		}

		/// <summary>Returns the UI Automation provider for the specified cell.</summary>
		/// <param name="row">The ordinal number of the row of interest.</param>
		/// <param name="column">The ordinal number of the column of interest.</param>
		/// <returns>The UI Automation provider for the specified cell.</returns>
		// Token: 0x060026B5 RID: 9909 RVA: 0x000B7DC0 File Offset: 0x000B5FC0
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
			ListViewItem listViewItem = this._listview.ItemContainerGenerator.ContainerFromIndex(row) as ListViewItem;
			if (listViewItem == null)
			{
				VirtualizingPanel virtualizingPanel = this._listview.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(row);
				}
				listViewItem = (this._listview.ItemContainerGenerator.ContainerFromIndex(row) as ListViewItem);
				if (listViewItem != null)
				{
					this._listview.Dispatcher.Invoke(DispatcherPriority.Loaded, new DispatcherOperationCallback((object arg) => null), null);
				}
			}
			if (listViewItem != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this._listview);
				if (automationPeer != null)
				{
					AutomationPeer automationPeer2 = UIElementAutomationPeer.FromElement(listViewItem);
					if (automationPeer2 != null)
					{
						AutomationPeer eventsSource = automationPeer2.EventsSource;
						if (eventsSource != null)
						{
							automationPeer2 = eventsSource;
						}
						List<AutomationPeer> children = automationPeer2.GetChildren();
						if (children.Count > column)
						{
							return ElementProxy.StaticWrap(children[column], automationPeer);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000B7ECC File Offset: 0x000B60CC
		private void OnColumnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._oldColumnsCount != this._owner.Columns.Count)
			{
				ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
				Invariant.Assert(listViewAutomationPeer != null);
				if (listViewAutomationPeer != null)
				{
					listViewAutomationPeer.RaisePropertyChangedEvent(GridPatternIdentifiers.ColumnCountProperty, this._oldColumnsCount, this._owner.Columns.Count);
				}
			}
			this._oldColumnsCount = this._owner.Columns.Count;
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this._listview);
			if (automationPeer != null)
			{
				List<AutomationPeer> children = automationPeer.GetChildren();
				if (children != null)
				{
					foreach (AutomationPeer automationPeer2 in children)
					{
						automationPeer2.InvalidatePeer();
					}
				}
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000B7FAC File Offset: 0x000B61AC
		internal static Visual FindVisualByType(Visual parent, Type type)
		{
			if (parent != null)
			{
				int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
				for (int i = 0; i < internalVisualChildrenCount; i++)
				{
					Visual visual = parent.InternalGetVisualChild(i);
					if (!type.IsInstanceOfType(visual))
					{
						visual = GridViewAutomationPeer.FindVisualByType(visual, type);
					}
					if (visual != null)
					{
						return visual;
					}
				}
			}
			return null;
		}

		// Token: 0x04001B78 RID: 7032
		private GridView _owner;

		// Token: 0x04001B79 RID: 7033
		private ListView _listview;

		// Token: 0x04001B7A RID: 7034
		private int _oldItemsCount;

		// Token: 0x04001B7B RID: 7035
		private int _oldColumnsCount;
	}
}
