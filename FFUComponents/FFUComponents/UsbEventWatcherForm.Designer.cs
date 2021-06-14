namespace FFUComponents
{
	// Token: 0x02000055 RID: 85
	internal partial class UsbEventWatcherForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00008260 File Offset: 0x00006460
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.ClientSize = new global::System.Drawing.Size(0, 0);
			base.WindowState = global::System.Windows.Forms.FormWindowState.Minimized;
			base.Visible = false;
			base.Name = "UsbEventWatcherForm";
			base.ShowInTaskbar = false;
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(1, 1);
			global::System.IntPtr value = global::FFUComponents.NativeMethods.SetParent(base.Handle, (global::System.IntPtr)(-3L));
			if (global::System.IntPtr.Zero == value)
			{
				throw new global::System.ComponentModel.Win32Exception(global::System.Runtime.InteropServices.Marshal.GetLastWin32Error());
			}
			base.ResumeLayout(false);
		}
	}
}
