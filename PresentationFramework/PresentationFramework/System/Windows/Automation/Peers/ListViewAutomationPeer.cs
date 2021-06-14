using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ListView" /> types to UI Automation.</summary>
	// Token: 0x020002CF RID: 719
	public class ListViewAutomationPeer : ListBoxAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ListView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</param>
		// Token: 0x06002774 RID: 10100 RVA: 0x000BA189 File Offset: 0x000B8389
		public ListViewAutomationPeer(ListView owner) : base(owner)
		{
			Invariant.Assert(owner != null);
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ListView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.List" /> enumeration value.</returns>
		// Token: 0x06002775 RID: 10101 RVA: 0x000BA19B File Offset: 0x000B839B
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if (this._viewAutomationPeer != null)
			{
				return this._viewAutomationPeer.GetAutomationControlType();
			}
			return base.GetAutomationControlTypeCore();
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ListView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ListView".</returns>
		// Token: 0x06002776 RID: 10102 RVA: 0x000BA1B7 File Offset: 0x000B83B7
		protected override string GetClassNameCore()
		{
			return "ListView";
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.ListView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the view that this <see cref="T:System.Windows.Controls.ListView" /> is using. Default <see cref="T:System.Windows.Controls.ListView" /> implementation uses the <see cref="T:System.Windows.Controls.GridView" />, and this method returns <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" />.</returns>
		// Token: 0x06002777 RID: 10103 RVA: 0x000BA1C0 File Offset: 0x000B83C0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (this._viewAutomationPeer != null)
			{
				object pattern = this._viewAutomationPeer.GetPattern(patternInterface);
				if (pattern != null)
				{
					return pattern;
				}
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.ListView" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002778 RID: 10104 RVA: 0x000BA1F0 File Offset: 0x000B83F0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this._refreshItemPeers)
			{
				this._refreshItemPeers = false;
				base.ItemPeers.Clear();
			}
			List<AutomationPeer> list = base.GetChildrenCore();
			if (this._viewAutomationPeer != null)
			{
				list = this._viewAutomationPeer.GetChildren(list);
			}
			return list;
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</param>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> instance that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</returns>
		// Token: 0x06002779 RID: 10105 RVA: 0x000BA234 File Offset: 0x000B8434
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			if (this._viewAutomationPeer != null)
			{
				return this._viewAutomationPeer.CreateItemAutomationPeer(item);
			}
			return base.CreateItemAutomationPeer(item);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.IViewAutomationPeer" /> for this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />. </summary>
		/// <returns>The interface instance that is associated with this <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</returns>
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000BA252 File Offset: 0x000B8452
		// (set) Token: 0x0600277B RID: 10107 RVA: 0x000BA25A File Offset: 0x000B845A
		protected internal IViewAutomationPeer ViewAutomationPeer
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				return this._viewAutomationPeer;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				if (this._viewAutomationPeer != value)
				{
					this._refreshItemPeers = true;
				}
				this._viewAutomationPeer = value;
			}
		}

		// Token: 0x04001B91 RID: 7057
		private bool _refreshItemPeers;

		// Token: 0x04001B92 RID: 7058
		private IViewAutomationPeer _viewAutomationPeer;
	}
}
