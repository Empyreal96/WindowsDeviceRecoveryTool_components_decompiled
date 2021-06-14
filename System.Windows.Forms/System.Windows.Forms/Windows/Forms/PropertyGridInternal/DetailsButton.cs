using System;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x02000487 RID: 1159
	internal class DetailsButton : Button
	{
		// Token: 0x06004E33 RID: 20019 RVA: 0x00140EEC File Offset: 0x0013F0EC
		public DetailsButton(GridErrorDlg form)
		{
			this.parent = form;
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x00140EFB File Offset: 0x0013F0FB
		public bool Expanded
		{
			get
			{
				return this.parent.DetailsButtonExpanded;
			}
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00140F08 File Offset: 0x0013F108
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level1)
			{
				return new DetailsButtonAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x0400333F RID: 13119
		private GridErrorDlg parent;
	}
}
