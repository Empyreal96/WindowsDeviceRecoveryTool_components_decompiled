using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000488 RID: 1160
	internal class DetailsButtonAccessibleObject : Control.ControlAccessibleObject
	{
		// Token: 0x06004E36 RID: 20022 RVA: 0x00140F1E File Offset: 0x0013F11E
		public DetailsButtonAccessibleObject(DetailsButton owner) : base(owner)
		{
			this.ownerItem = owner;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool IsIAccessibleExSupported()
		{
			return true;
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x00140F2E File Offset: 0x0013F12E
		internal override object GetPropertyValue(int propertyID)
		{
			if (propertyID == 30003)
			{
				return 50000;
			}
			return base.GetPropertyValue(propertyID);
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x00140F4A File Offset: 0x0013F14A
		internal override bool IsPatternSupported(int patternId)
		{
			return patternId == 10005 || base.IsPatternSupported(patternId);
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x00140F5D File Offset: 0x0013F15D
		internal override UnsafeNativeMethods.ExpandCollapseState ExpandCollapseState
		{
			get
			{
				if (!this.ownerItem.Expanded)
				{
					return UnsafeNativeMethods.ExpandCollapseState.Collapsed;
				}
				return UnsafeNativeMethods.ExpandCollapseState.Expanded;
			}
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x00140F6F File Offset: 0x0013F16F
		internal override void Expand()
		{
			if (this.ownerItem != null && !this.ownerItem.Expanded)
			{
				this.DoDefaultAction();
			}
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x00140F8C File Offset: 0x0013F18C
		internal override void Collapse()
		{
			if (this.ownerItem != null && this.ownerItem.Expanded)
			{
				this.DoDefaultAction();
			}
		}

		// Token: 0x04003340 RID: 13120
		private DetailsButton ownerItem;
	}
}
