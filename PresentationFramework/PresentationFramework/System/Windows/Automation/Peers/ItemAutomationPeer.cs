using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes a data item in an <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection to UI Automation. </summary>
	// Token: 0x020002C4 RID: 708
	public abstract class ItemAutomationPeer : AutomationPeer, IVirtualizedItemProvider
	{
		/// <summary>Provides initialization for base class values when called by the constructor of a derived class.</summary>
		/// <param name="item">The data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />.</param>
		/// <param name="itemsControlAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</param>
		// Token: 0x06002703 RID: 9987 RVA: 0x000B8B4E File Offset: 0x000B6D4E
		protected ItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
		{
			this.Item = item;
			this._itemsControlAutomationPeer = itemsControlAutomationPeer;
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x000B52C9 File Offset: 0x000B34C9
		// (set) Token: 0x06002705 RID: 9989 RVA: 0x000B8B64 File Offset: 0x000B6D64
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
				AutomationPeer wrapperPeer = this.GetWrapperPeer();
				if (wrapperPeer != null)
				{
					wrapperPeer.AncestorsInvalid = false;
				}
			}
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration value that specifies the control pattern.</param>
		/// <returns>An object that supports the control pattern if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. </returns>
		// Token: 0x06002706 RID: 9990 RVA: 0x000B8B90 File Offset: 0x000B6D90
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.VirtualizedItem)
			{
				if (VirtualizedItemPatternIdentifiers.Pattern != null)
				{
					if (this.GetWrapperPeer() == null)
					{
						return this;
					}
					if (this.ItemsControlAutomationPeer != null && !this.IsItemInAutomationTree())
					{
						return this;
					}
					if (this.ItemsControlAutomationPeer == null)
					{
						return this;
					}
				}
				return null;
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				UIElementAutomationPeer uielementAutomationPeer = this.GetWrapperPeer() as UIElementAutomationPeer;
				if (uielementAutomationPeer != null)
				{
					return uielementAutomationPeer.GetPattern(patternInterface);
				}
			}
			return null;
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000B8BF0 File Offset: 0x000B6DF0
		internal UIElement GetWrapper()
		{
			UIElement result = null;
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				ItemsControl itemsControl = (ItemsControl)itemsControlAutomationPeer.Owner;
				if (itemsControl != null)
				{
					object rawItem = this.RawItem;
					if (rawItem != DependencyProperty.UnsetValue)
					{
						if (((IGeneratorHost)itemsControl).IsItemItsOwnContainer(rawItem))
						{
							result = (rawItem as UIElement);
						}
						else
						{
							result = (itemsControl.ItemContainerGenerator.ContainerFromItem(rawItem) as UIElement);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000B8C4C File Offset: 0x000B6E4C
		internal virtual AutomationPeer GetWrapperPeer()
		{
			AutomationPeer automationPeer = null;
			UIElement wrapper = this.GetWrapper();
			if (wrapper != null)
			{
				automationPeer = UIElementAutomationPeer.CreatePeerForElement(wrapper);
				if (automationPeer == null)
				{
					if (wrapper is FrameworkElement)
					{
						automationPeer = new FrameworkElementAutomationPeer((FrameworkElement)wrapper);
					}
					else
					{
						automationPeer = new UIElementAutomationPeer(wrapper);
					}
				}
			}
			return automationPeer;
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000B8C8D File Offset: 0x000B6E8D
		internal void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null && !(this is GridViewItemAutomationPeer) && !this.IsItemInAutomationTree())
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000B8CB8 File Offset: 0x000B6EB8
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000B8D0C File Offset: 0x000B6F0C
		internal override void AddToParentProxyWeakRefCache()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				itemsControlAutomationPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000B8D30 File Offset: 0x000B6F30
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetVisibleBoundingRectCore();
			}
			return base.GetBoundingRectangle();
		}

		/// <summary>Gets a human-readable string that contains the type of item that the specified <see cref="T:System.Windows.UIElement" /> represents. </summary>
		/// <returns>The item type. An example includes "Mail Message" or "Contact".</returns>
		// Token: 0x0600270E RID: 9998 RVA: 0x000B8D54 File Offset: 0x000B6F54
		protected override string GetItemTypeCore()
		{
			return string.Empty;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x0600270F RID: 9999 RVA: 0x000B8D5C File Offset: 0x000B6F5C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.ForceEnsureChildren();
				return wrapperPeer.GetChildren();
			}
			return null;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Rect" /> that represents the bounding rectangle of the specified <see cref="T:System.Windows.UIElement" />. </summary>
		/// <returns>The bounding rectangle.</returns>
		// Token: 0x06002710 RID: 10000 RVA: 0x000B8D84 File Offset: 0x000B6F84
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> is off the screen. </summary>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.UIElement" /> is not on the screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002711 RID: 10001 RVA: 0x000B8DB4 File Offset: 0x000B6FB4
		protected override bool IsOffscreenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> is laid out in a particular direction. </summary>
		/// <returns>The direction of the specified <see cref="T:System.Windows.UIElement" />. Optionally, the method returns <see cref="F:System.Windows.Automation.Peers.AutomationOrientation.None" /> if the <see cref="T:System.Windows.UIElement" /> is not laid out in a particular direction.</returns>
		// Token: 0x06002712 RID: 10002 RVA: 0x000B8DDC File Offset: 0x000B6FDC
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000B8E04 File Offset: 0x000B7004
		protected override int GetPositionInSetCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				int num = wrapperPeer.GetPositionInSet();
				if (num == -1)
				{
					ItemsControl itemsControl = (ItemsControl)this.ItemsControlAutomationPeer.Owner;
					num = ItemAutomationPeer.GetPositionInSetFromItemsControl(itemsControl, this.Item);
				}
				return num;
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000B8E50 File Offset: 0x000B7050
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				int num = wrapperPeer.GetSizeOfSet();
				if (num == -1)
				{
					ItemsControl itemsControl = (ItemsControl)this.ItemsControlAutomationPeer.Owner;
					num = ItemAutomationPeer.GetSizeOfSetFromItemsControl(itemsControl, this.Item);
				}
				return num;
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000B8E9C File Offset: 0x000B709C
		internal static int GetPositionInSetFromItemsControl(ItemsControl itemsControl, object item)
		{
			ItemCollection items = itemsControl.Items;
			int num = items.IndexOf(item);
			if (itemsControl.IsGrouping)
			{
				int num2;
				num = ItemAutomationPeer.FindPositionInGroup(items.Groups, num, out num2);
			}
			return num + 1;
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000B8ED4 File Offset: 0x000B70D4
		internal static int GetSizeOfSetFromItemsControl(ItemsControl itemsControl, object item)
		{
			int result = -1;
			ItemCollection items = itemsControl.Items;
			if (itemsControl.IsGrouping)
			{
				int position = items.IndexOf(item);
				ItemAutomationPeer.FindPositionInGroup(items.Groups, position, out result);
			}
			else
			{
				result = items.Count;
			}
			return result;
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000B8F14 File Offset: 0x000B7114
		private static int FindPositionInGroup(ReadOnlyObservableCollection<object> collection, int position, out int sizeOfGroup)
		{
			ReadOnlyObservableCollection<object> readOnlyObservableCollection = null;
			sizeOfGroup = -1;
			do
			{
				readOnlyObservableCollection = null;
				foreach (object obj in collection)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = (CollectionViewGroupInternal)obj;
					if (position < collectionViewGroupInternal.ItemCount)
					{
						CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal;
						if (collectionViewGroupInternal2.IsBottomLevel)
						{
							readOnlyObservableCollection = null;
							sizeOfGroup = collectionViewGroupInternal.ItemCount;
							break;
						}
						readOnlyObservableCollection = collectionViewGroupInternal2.Items;
						break;
					}
					else
					{
						position -= collectionViewGroupInternal.ItemCount;
					}
				}
			}
			while ((collection = readOnlyObservableCollection) != null);
			return position;
		}

		/// <summary>Gets a string that conveys the visual status of the specified <see cref="T:System.Windows.UIElement" />. </summary>
		/// <returns>The status. Examples include "Busy" or "Online".</returns>
		// Token: 0x06002718 RID: 10008 RVA: 0x000B8F9C File Offset: 0x000B719C
		protected override string GetItemStatusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> is required to be completed on a form. </summary>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.UIElement" /> is required to be completed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002719 RID: 10009 RVA: 0x000B8FC8 File Offset: 0x000B71C8
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> can accept keyboard focus. </summary>
		/// <returns>
		///     <see langword="true" /> if the element can accept keyboard focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600271A RID: 10010 RVA: 0x000B8FF0 File Offset: 0x000B71F0
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> currently has keyboard input focus. </summary>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.UIElement" /> has keyboard input focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600271B RID: 10011 RVA: 0x000B9018 File Offset: 0x000B7218
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> can receive and send events. </summary>
		/// <returns>
		///     <see langword="true" /> if the UI Automation peer can receive and send events; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600271C RID: 10012 RVA: 0x000B9040 File Offset: 0x000B7240
		protected override bool IsEnabledCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> contains protected content. </summary>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.UIElement" /> contains protected content; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600271D RID: 10013 RVA: 0x000B9068 File Offset: 0x000B7268
		protected override bool IsPasswordCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		/// <summary>Gets the string that uniquely identifies the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>A string that contains the UI Automation identifier.</returns>
		// Token: 0x0600271E RID: 10014 RVA: 0x000B9090 File Offset: 0x000B7290
		protected override string GetAutomationIdCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			string result = null;
			object item;
			if (wrapperPeer != null)
			{
				result = wrapperPeer.GetAutomationId();
			}
			else if ((item = this.Item) != null)
			{
				using (RecyclableWrapper recyclableWrapperPeer = this.ItemsControlAutomationPeer.GetRecyclableWrapperPeer(item))
				{
					result = recyclableWrapperPeer.Peer.GetAutomationId();
				}
			}
			return result;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>The text label.</returns>
		// Token: 0x0600271F RID: 10015 RVA: 0x000B90F4 File Offset: 0x000B72F4
		protected override string GetNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			string text = null;
			object item = this.Item;
			if (wrapperPeer != null)
			{
				text = wrapperPeer.GetName();
			}
			else if (item != null)
			{
				using (RecyclableWrapper recyclableWrapperPeer = this.ItemsControlAutomationPeer.GetRecyclableWrapperPeer(item))
				{
					text = recyclableWrapperPeer.Peer.GetName();
				}
			}
			if (string.IsNullOrEmpty(text) && item != null)
			{
				FrameworkElement frameworkElement = item as FrameworkElement;
				if (frameworkElement != null)
				{
					text = frameworkElement.GetPlainText();
				}
				if (string.IsNullOrEmpty(text))
				{
					text = item.ToString();
				}
			}
			return text;
		}

		/// <summary>Gets a value that indicates whether the specified <see cref="T:System.Windows.UIElement" /> contains data that is presented to the user. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is a content element; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002720 RID: 10016 RVA: 0x000B9184 File Offset: 0x000B7384
		protected override bool IsContentElementCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			return wrapperPeer == null || wrapperPeer.IsContentElement();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.UIElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> is understood by the end user as interactive. </summary>
		/// <returns>
		///     <see langword="true" /> if the element is a control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002721 RID: 10017 RVA: 0x000B91A4 File Offset: 0x000B73A4
		protected override bool IsControlElementCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			return wrapperPeer == null || wrapperPeer.IsControlElement();
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Label" /> that is targeted to the specified <see cref="T:System.Windows.UIElement" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" /> for the element that is targeted by the <see cref="T:System.Windows.Controls.Label" />.</returns>
		// Token: 0x06002722 RID: 10018 RVA: 0x000B91C4 File Offset: 0x000B73C4
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		/// <summary>Gets the notification characteristics of the live region for the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> object. </summary>
		/// <returns>The notification characteristics of the live region. </returns>
		// Token: 0x06002723 RID: 10019 RVA: 0x000B91EC File Offset: 0x000B73EC
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLiveSetting();
			}
			this.ThrowElementNotAvailableException();
			return AutomationLiveSetting.Off;
		}

		/// <summary>Gets the string that describes the functionality of the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>The help text.</returns>
		// Token: 0x06002724 RID: 10020 RVA: 0x000B9214 File Offset: 0x000B7414
		protected override string GetHelpTextCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetHelpText();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Gets the accelerator key for the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>The accelerator key.</returns>
		// Token: 0x06002725 RID: 10021 RVA: 0x000B9240 File Offset: 0x000B7440
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Gets the access key for the <see cref="T:System.Windows.UIElement" /> that corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		/// <returns>The access key.</returns>
		// Token: 0x06002726 RID: 10022 RVA: 0x000B926C File Offset: 0x000B746C
		protected override string GetAccessKeyCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Point" /> that represents the clickable space that is on the specified <see cref="T:System.Windows.UIElement" />. </summary>
		/// <returns>The point that represents the clickable space that is on the specified <see cref="T:System.Windows.UIElement" />.</returns>
		// Token: 0x06002727 RID: 10023 RVA: 0x000B9298 File Offset: 0x000B7498
		protected override Point GetClickablePointCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		/// <summary>Sets the keyboard input focus on the specified <see cref="T:System.Windows.UIElement" />. The <see cref="T:System.Windows.UIElement" /> corresponds to the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />. </summary>
		// Token: 0x06002728 RID: 10024 RVA: 0x000B92D4 File Offset: 0x000B74D4
		protected override void SetFocusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000B92F8 File Offset: 0x000B74F8
		internal virtual ItemsControlAutomationPeer GetItemsControlAutomationPeer()
		{
			return this._itemsControlAutomationPeer;
		}

		/// <summary>Gets the data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />.</summary>
		/// <returns>The data item.</returns>
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000B9300 File Offset: 0x000B7500
		// (set) Token: 0x0600272B RID: 10027 RVA: 0x000B9329 File Offset: 0x000B7529
		public object Item
		{
			get
			{
				ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
				if (itemWeakReference == null)
				{
					return this._item;
				}
				return itemWeakReference.Target;
			}
			private set
			{
				if (value != null && !value.GetType().IsValueType && !FrameworkAppContextSwitches.ItemAutomationPeerKeepsItsItemAlive)
				{
					this._item = new ItemAutomationPeer.ItemWeakReference(value);
					return;
				}
				this._item = value;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000B9358 File Offset: 0x000B7558
		private object RawItem
		{
			get
			{
				ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
				if (itemWeakReference == null)
				{
					return this._item;
				}
				object target = itemWeakReference.Target;
				if (target != null)
				{
					return target;
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000B938C File Offset: 0x000B758C
		internal void ReuseForItem(object item)
		{
			ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
			if (itemWeakReference != null)
			{
				if (item != itemWeakReference.Target)
				{
					itemWeakReference.Target = item;
					return;
				}
			}
			else
			{
				this._item = item;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</summary>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> object.</returns>
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000B93C0 File Offset: 0x000B75C0
		// (set) Token: 0x0600272F RID: 10031 RVA: 0x000B93C8 File Offset: 0x000B75C8
		public ItemsControlAutomationPeer ItemsControlAutomationPeer
		{
			get
			{
				return this.GetItemsControlAutomationPeer();
			}
			internal set
			{
				this._itemsControlAutomationPeer = value;
			}
		}

		/// <summary>Makes the virtual item fully accessible as a UI Automation element.</summary>
		// Token: 0x06002730 RID: 10032 RVA: 0x000B93D1 File Offset: 0x000B75D1
		void IVirtualizedItemProvider.Realize()
		{
			this.RealizeCore();
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000B93DC File Offset: 0x000B75DC
		internal virtual void RealizeCore()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				ItemsControl parent = itemsControlAutomationPeer.Owner as ItemsControl;
				if (parent != null)
				{
					if (parent.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
					{
						if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parent))
						{
							itemsControlAutomationPeer.RecentlyRealizedPeers.Add(this);
						}
						parent.OnBringItemIntoView(this.Item);
						return;
					}
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(delegate(object arg)
					{
						if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parent))
						{
							itemsControlAutomationPeer.RecentlyRealizedPeers.Add(this);
						}
						parent.OnBringItemIntoView(arg);
						return null;
					}), this.Item);
				}
			}
		}

		// Token: 0x04001B83 RID: 7043
		private object _item;

		// Token: 0x04001B84 RID: 7044
		private ItemsControlAutomationPeer _itemsControlAutomationPeer;

		// Token: 0x020008B9 RID: 2233
		private class ItemWeakReference : WeakReference
		{
			// Token: 0x0600843F RID: 33855 RVA: 0x00235BB8 File Offset: 0x00233DB8
			public ItemWeakReference(object o) : base(o)
			{
			}
		}
	}
}
