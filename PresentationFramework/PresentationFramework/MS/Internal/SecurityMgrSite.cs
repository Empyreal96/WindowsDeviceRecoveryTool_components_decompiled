using System;
using System.Security;
using System.Windows;
using MS.Internal.AppModel;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020005E4 RID: 1508
	internal class SecurityMgrSite : NativeMethods.IInternetSecurityMgrSite
	{
		// Token: 0x06006479 RID: 25721 RVA: 0x0000326D File Offset: 0x0000146D
		internal SecurityMgrSite()
		{
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x001C3298 File Offset: 0x001C1498
		[SecurityCritical]
		public void GetWindow(ref IntPtr phwnd)
		{
			phwnd = IntPtr.Zero;
			if (Application.Current != null)
			{
				Window mainWindow = Application.Current.MainWindow;
				Invariant.Assert(Application.Current.BrowserCallbackServices == null || mainWindow is RootBrowserWindow);
				if (mainWindow != null)
				{
					phwnd = mainWindow.CriticalHandle;
				}
			}
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x00002137 File Offset: 0x00000337
		public void EnableModeless(bool fEnable)
		{
		}
	}
}
