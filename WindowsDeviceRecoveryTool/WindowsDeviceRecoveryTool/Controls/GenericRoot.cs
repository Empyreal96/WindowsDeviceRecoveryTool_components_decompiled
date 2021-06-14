using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200000B RID: 11
	public sealed class GenericRoot : ContentControl
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00003420 File Offset: 0x00001620
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GenericRoot.CustomRootAutomationPeer(this);
		}

		// Token: 0x0200000C RID: 12
		private sealed class CustomRootAutomationPeer : GenericRootAutomationPeer
		{
			// Token: 0x06000054 RID: 84 RVA: 0x00003440 File Offset: 0x00001640
			public CustomRootAutomationPeer(UIElement owner) : base(owner)
			{
			}

			// Token: 0x06000055 RID: 85 RVA: 0x0000344C File Offset: 0x0000164C
			protected override List<AutomationPeer> GetChildrenCore()
			{
				List<AutomationPeer> childrenCore = base.GetChildrenCore();
				List<AutomationPeer> result;
				if (childrenCore == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < childrenCore.Count; i++)
					{
						AutomationPeer automationPeer = childrenCore[i];
						if (automationPeer.GetType() == typeof(PasswordBoxAutomationPeer))
						{
							PasswordBoxAutomationPeer passwordBoxAutomationPeer = (PasswordBoxAutomationPeer)automationPeer;
							PasswordBox passwordBox = (PasswordBox)passwordBoxAutomationPeer.Owner;
							AnnounceablePasswordBoxAutomationPeer announceablePasswordBoxAutomationPeer = new AnnounceablePasswordBoxAutomationPeer((PasswordBox)passwordBoxAutomationPeer.Owner);
							object obj = typeof(UIElement).InvokeMember("AutomationPeerField", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField, null, null, null);
							obj.GetType().InvokeMember("SetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, obj, new object[]
							{
								passwordBox,
								announceablePasswordBoxAutomationPeer
							});
							childrenCore[i] = announceablePasswordBoxAutomationPeer;
						}
					}
					result = childrenCore;
				}
				return result;
			}
		}
	}
}
