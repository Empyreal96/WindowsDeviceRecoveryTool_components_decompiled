using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200017E RID: 382
	internal class DataGridToolTip : MarshalByRefObject
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x0004FB41 File Offset: 0x0004DD41
		public DataGridToolTip(DataGrid dataGrid)
		{
			this.dataGrid = dataGrid;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0004FB50 File Offset: 0x0004DD50
		public void CreateToolTipHandle()
		{
			if (this.tipWindow == null || this.tipWindow.Handle == IntPtr.Zero)
			{
				NativeMethods.INITCOMMONCONTROLSEX initcommoncontrolsex = new NativeMethods.INITCOMMONCONTROLSEX();
				initcommoncontrolsex.dwICC = 8;
				initcommoncontrolsex.dwSize = Marshal.SizeOf(initcommoncontrolsex);
				SafeNativeMethods.InitCommonControlsEx(initcommoncontrolsex);
				CreateParams createParams = new CreateParams();
				createParams.Parent = this.dataGrid.Handle;
				createParams.ClassName = "tooltips_class32";
				createParams.Style = 1;
				this.tipWindow = new NativeWindow();
				this.tipWindow.CreateHandle(createParams);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
				SafeNativeMethods.SetWindowPos(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.HWND_NOTOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1027, 3, 0);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0004FC58 File Offset: 0x0004DE58
		public void AddToolTip(string toolTipString, IntPtr toolTipId, Rectangle iconBounds)
		{
			if (toolTipString == null)
			{
				throw new ArgumentNullException("toolTipString");
			}
			if (iconBounds.IsEmpty)
			{
				throw new ArgumentNullException("iconBounds", SR.GetString("DataGridToolTipEmptyIcon"));
			}
			NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
			toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
			toolinfo_T.hwnd = this.dataGrid.Handle;
			toolinfo_T.uId = toolTipId;
			toolinfo_T.lpszText = toolTipString;
			toolinfo_T.rect = NativeMethods.RECT.FromXYWH(iconBounds.X, iconBounds.Y, iconBounds.Width, iconBounds.Height);
			toolinfo_T.uFlags = 16;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo_T);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0004FD14 File Offset: 0x0004DF14
		public void RemoveToolTip(IntPtr toolTipId)
		{
			NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
			toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
			toolinfo_T.hwnd = this.dataGrid.Handle;
			toolinfo_T.uId = toolTipId;
			UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_DELTOOL, 0, toolinfo_T);
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0004FD6E File Offset: 0x0004DF6E
		public void Destroy()
		{
			this.tipWindow.DestroyHandle();
			this.tipWindow = null;
		}

		// Token: 0x04000A21 RID: 2593
		private NativeWindow tipWindow;

		// Token: 0x04000A22 RID: 2594
		private DataGrid dataGrid;
	}
}
