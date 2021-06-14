using System;
using System.Collections.Specialized;
using System.Windows.Automation.Peers;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that displays a list of data items.</summary>
	// Token: 0x020004FD RID: 1277
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListViewItem))]
	public class ListView : ListBox
	{
		// Token: 0x0600516A RID: 20842 RVA: 0x0016D458 File Offset: 0x0016B658
		static ListView()
		{
			ListBox.SelectionModeProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(SelectionMode.Extended));
			ControlsTraceLogger.AddControl(TelemetryControls.ListView);
		}

		/// <summary>Gets or sets an object that defines how the data is styled and organized in a <see cref="T:System.Windows.Controls.ListView" /> control.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ViewBase" /> object that specifies how to display information in the <see cref="T:System.Windows.Controls.ListView" />.</returns>
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x0600516B RID: 20843 RVA: 0x0016D4C3 File Offset: 0x0016B6C3
		// (set) Token: 0x0600516C RID: 20844 RVA: 0x0016D4D5 File Offset: 0x0016B6D5
		public ViewBase View
		{
			get
			{
				return (ViewBase)base.GetValue(ListView.ViewProperty);
			}
			set
			{
				base.SetValue(ListView.ViewProperty, value);
			}
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x0016D4E4 File Offset: 0x0016B6E4
		private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListView listView = (ListView)d;
			ViewBase viewBase = (ViewBase)e.OldValue;
			ViewBase viewBase2 = (ViewBase)e.NewValue;
			if (viewBase2 != null)
			{
				if (viewBase2.IsUsed)
				{
					throw new InvalidOperationException(SR.Get("ListView_ViewCannotBeShared"));
				}
				viewBase2.IsUsed = true;
			}
			listView._previousView = viewBase;
			listView.ApplyNewView();
			listView._previousView = viewBase2;
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(listView) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null)
			{
				if (listViewAutomationPeer.ViewAutomationPeer != null)
				{
					listViewAutomationPeer.ViewAutomationPeer.ViewDetached();
				}
				if (viewBase2 != null)
				{
					listViewAutomationPeer.ViewAutomationPeer = viewBase2.GetAutomationPeer(listView);
				}
				else
				{
					listViewAutomationPeer.ViewAutomationPeer = null;
				}
				listViewAutomationPeer.InvalidatePeer();
			}
			if (viewBase != null)
			{
				viewBase.IsUsed = false;
			}
		}

		/// <summary>Sets the styles, templates, and bindings for a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="element">An object that is a <see cref="T:System.Windows.Controls.ListViewItem" /> or that can be converted into one.</param>
		/// <param name="item">The object to use to create the <see cref="T:System.Windows.Controls.ListViewItem" />.</param>
		// Token: 0x0600516E RID: 20846 RVA: 0x0016D594 File Offset: 0x0016B794
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			ListViewItem listViewItem = element as ListViewItem;
			if (listViewItem != null)
			{
				ViewBase view = this.View;
				if (view != null)
				{
					listViewItem.SetDefaultStyleKey(view.ItemContainerDefaultStyleKey);
					view.PrepareItem(listViewItem);
					return;
				}
				listViewItem.ClearDefaultStyleKey();
			}
		}

		/// <summary>Removes all templates, styles, and bindings for the object that is displayed as a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.ListViewItem" /> container to clear.</param>
		/// <param name="item">The object that the <see cref="T:System.Windows.Controls.ListViewItem" /> contains.</param>
		// Token: 0x0600516F RID: 20847 RVA: 0x0016D5D7 File Offset: 0x0016B7D7
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
		}

		/// <summary>Determines whether an object is a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="item" /> is a <see cref="T:System.Windows.Controls.ListViewItem" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005170 RID: 20848 RVA: 0x0016D5E1 File Offset: 0x0016B7E1
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListViewItem;
		}

		/// <summary>Creates and returns a new <see cref="T:System.Windows.Controls.ListViewItem" /> container.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.ListViewItem" /> control.</returns>
		// Token: 0x06005171 RID: 20849 RVA: 0x0016D5EC File Offset: 0x0016B7EC
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListViewItem();
		}

		/// <summary>Responds to an <see cref="M:System.Windows.Controls.ItemsControl.OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs)" />. </summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06005172 RID: 20850 RVA: 0x0016D5F4 File Offset: 0x0016B7F4
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null && listViewAutomationPeer.ViewAutomationPeer != null)
			{
				listViewAutomationPeer.ViewAutomationPeer.ItemsChanged(e);
			}
		}

		/// <summary>Defines an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> object for the <see cref="T:System.Windows.Controls.ListView" /> control.</returns>
		// Token: 0x06005173 RID: 20851 RVA: 0x0016D62C File Offset: 0x0016B82C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			ListViewAutomationPeer listViewAutomationPeer = new ListViewAutomationPeer(this);
			if (listViewAutomationPeer != null && this.View != null)
			{
				listViewAutomationPeer.ViewAutomationPeer = this.View.GetAutomationPeer(this);
			}
			return listViewAutomationPeer;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x0016D660 File Offset: 0x0016B860
		private void ApplyNewView()
		{
			ViewBase view = this.View;
			if (view != null)
			{
				base.DefaultStyleKey = view.DefaultStyleKey;
			}
			else
			{
				base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
			}
			if (base.IsLoaded)
			{
				base.ItemContainerGenerator.Refresh();
			}
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x0016D6A3 File Offset: 0x0016B8A3
		internal override void OnThemeChanged()
		{
			if (!base.HasTemplateGeneratedSubTree && this.View != null)
			{
				this.View.OnThemeChanged();
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListView.View" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListView.View" /> dependency property.</returns>
		// Token: 0x04002C67 RID: 11367
		public static readonly DependencyProperty ViewProperty = DependencyProperty.Register("View", typeof(ViewBase), typeof(ListView), new PropertyMetadata(new PropertyChangedCallback(ListView.OnViewChanged)));

		// Token: 0x04002C68 RID: 11368
		private ViewBase _previousView;
	}
}
