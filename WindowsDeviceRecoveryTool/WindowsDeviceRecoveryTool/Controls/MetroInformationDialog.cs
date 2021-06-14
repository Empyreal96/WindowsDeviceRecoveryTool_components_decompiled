using System;
using System.Windows;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000038 RID: 56
	public class MetroInformationDialog : MetroDialog
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000DE05 File Offset: 0x0000C005
		public MetroInformationDialog()
		{
			base.YesButtonFocused = true;
			this.ButtonNo.Visibility = Visibility.Collapsed;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000DE28 File Offset: 0x0000C028
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000DE40 File Offset: 0x0000C040
		public string ButtonText
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
	}
}
