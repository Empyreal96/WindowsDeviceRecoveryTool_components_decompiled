using System;

namespace System.Windows.Forms
{
	// Token: 0x0200033F RID: 831
	internal sealed class RTLAwareMessageBox
	{
		// Token: 0x06003348 RID: 13128 RVA: 0x000EEDEF File Offset: 0x000ECFEF
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			if (RTLAwareMessageBox.IsRTLResources)
			{
				options |= (MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
			}
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x000EEE11 File Offset: 0x000ED011
		public static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}
	}
}
