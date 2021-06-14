using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C9 RID: 713
	internal class ItemsControlWrapperAutomationPeer : ItemsControlAutomationPeer
	{
		// Token: 0x0600275A RID: 10074 RVA: 0x000B54DC File Offset: 0x000B36DC
		public ItemsControlWrapperAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000BA07A File Offset: 0x000B827A
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ItemsControlItemAutomationPeer(item, this);
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000BA083 File Offset: 0x000B8283
		protected override string GetClassNameCore()
		{
			return "ItemsControl";
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x0009580C File Offset: 0x00093A0C
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}
	}
}
