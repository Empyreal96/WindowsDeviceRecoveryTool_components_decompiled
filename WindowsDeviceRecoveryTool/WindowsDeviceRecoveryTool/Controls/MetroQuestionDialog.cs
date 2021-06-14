using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000039 RID: 57
	public class MetroQuestionDialog : MetroDialog
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000DE4C File Offset: 0x0000C04C
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000DE64 File Offset: 0x0000C064
		public new string NoButtonText
		{
			get
			{
				return base.NoButtonText;
			}
			set
			{
				base.NoButtonText = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000DE70 File Offset: 0x0000C070
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000DE88 File Offset: 0x0000C088
		public new string YesButtonText
		{
			get
			{
				return base.YesButtonText;
			}
			set
			{
				base.YesButtonText = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000DE94 File Offset: 0x0000C094
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		public new bool YesButtonFocused
		{
			get
			{
				return base.YesButtonFocused;
			}
			set
			{
				base.YesButtonFocused = value;
			}
		}
	}
}
