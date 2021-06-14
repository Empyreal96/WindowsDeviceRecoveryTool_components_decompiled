using System;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C8 RID: 712
	internal class ItemsControlItemAutomationPeer : ItemAutomationPeer
	{
		// Token: 0x06002757 RID: 10071 RVA: 0x000BA069 File Offset: 0x000B8269
		public ItemsControlItemAutomationPeer(object item, ItemsControlWrapperAutomationPeer parent) : base(item, parent)
		{
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000964BA File Offset: 0x000946BA
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000BA073 File Offset: 0x000B8273
		protected override string GetClassNameCore()
		{
			return "ItemsControlItem";
		}
	}
}
