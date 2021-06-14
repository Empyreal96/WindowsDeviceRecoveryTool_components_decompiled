using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> types to UI Automation.</summary>
	// Token: 0x020002E1 RID: 737
	public class StatusBarAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" />.</param>
		// Token: 0x06002803 RID: 10243 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public StatusBarAutomationPeer(StatusBar owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "StatusBar".</returns>
		// Token: 0x06002804 RID: 10244 RVA: 0x000BB68F File Offset: 0x000B988F
		protected override string GetClassNameCore()
		{
			return "StatusBar";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.StatusBar" /> enumeration value.</returns>
		// Token: 0x06002805 RID: 10245 RVA: 0x00096074 File Offset: 0x00094274
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.StatusBar;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>A list of child elements.</returns>
		// Token: 0x06002806 RID: 10246 RVA: 0x000BB698 File Offset: 0x000B9898
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			ItemsControl itemsControl = base.Owner as ItemsControl;
			if (itemsControl != null)
			{
				foreach (object obj in ((IEnumerable)itemsControl.Items))
				{
					if (obj is Separator)
					{
						Separator element = obj as Separator;
						list.Add(UIElementAutomationPeer.CreatePeerForElement(element));
					}
					else
					{
						StatusBarItem statusBarItem = itemsControl.ItemContainerGenerator.ContainerFromItem(obj) as StatusBarItem;
						if (statusBarItem != null)
						{
							if (obj is string || obj is TextBlock || (obj is StatusBarItem && ((StatusBarItem)obj).Content is string))
							{
								list.Add(UIElementAutomationPeer.CreatePeerForElement(statusBarItem));
							}
							else
							{
								List<AutomationPeer> childrenAutomationPeer = this.GetChildrenAutomationPeer(statusBarItem);
								if (childrenAutomationPeer != null)
								{
									foreach (AutomationPeer item in childrenAutomationPeer)
									{
										list.Add(item);
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000BB7C8 File Offset: 0x000B99C8
		private List<AutomationPeer> GetChildrenAutomationPeer(Visual parent)
		{
			Invariant.Assert(parent != null);
			List<AutomationPeer> children = null;
			StatusBarAutomationPeer.iterate(parent, delegate(AutomationPeer peer)
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

		// Token: 0x06002808 RID: 10248 RVA: 0x000BB804 File Offset: 0x000B9A04
		private static bool iterate(Visual parent, StatusBarAutomationPeer.IteratorCallback callback)
		{
			bool flag = false;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int num = 0;
			while (num < internalVisualChildrenCount && !flag)
			{
				Visual visual = parent.InternalGetVisualChild(num);
				AutomationPeer peer;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (peer = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					flag = callback(peer);
				}
				else
				{
					flag = StatusBarAutomationPeer.iterate(visual, callback);
				}
				num++;
			}
			return flag;
		}

		// Token: 0x020008BD RID: 2237
		// (Invoke) Token: 0x06008446 RID: 33862
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
