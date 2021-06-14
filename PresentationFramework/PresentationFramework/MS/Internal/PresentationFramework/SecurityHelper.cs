using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using MS.Internal.Permissions;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x020007FD RID: 2045
	internal static class SecurityHelper
	{
		// Token: 0x06007D82 RID: 32130 RVA: 0x00234230 File Offset: 0x00232430
		[SecuritySafeCritical]
		internal static bool CheckUnmanagedCodePermission()
		{
			try
			{
				SecurityHelper.DemandUnmanagedCode();
			}
			catch (SecurityException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007D83 RID: 32131 RVA: 0x0023425C File Offset: 0x0023245C
		[SecurityCritical]
		internal static void DemandUnmanagedCode()
		{
			if (SecurityHelper._unmanagedCodePermission == null)
			{
				SecurityHelper._unmanagedCodePermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			}
			SecurityHelper._unmanagedCodePermission.Demand();
		}

		// Token: 0x06007D84 RID: 32132 RVA: 0x0023427A File Offset: 0x0023247A
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ThrowExceptionIfSettingTrueInPartialTrust(ref bool value)
		{
			if (value && !SecurityHelper.CheckUnmanagedCodePermission())
			{
				value = false;
				throw new SecurityException(SR.Get("SecurityExceptionForSettingSandboxExternalToTrue"));
			}
		}

		// Token: 0x06007D85 RID: 32133 RVA: 0x0023429A File Offset: 0x0023249A
		[SecurityCritical]
		internal static void DemandWebBrowserPermission()
		{
			SecurityHelper.CachedWebBrowserPermission.Demand();
		}

		// Token: 0x17001D2C RID: 7468
		// (get) Token: 0x06007D86 RID: 32134 RVA: 0x002342A6 File Offset: 0x002324A6
		internal static WebBrowserPermission CachedWebBrowserPermission
		{
			[SecurityCritical]
			get
			{
				if (SecurityHelper._webBrowserPermission == null)
				{
					SecurityHelper._webBrowserPermission = new WebBrowserPermission(PermissionState.Unrestricted);
				}
				return SecurityHelper._webBrowserPermission;
			}
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x002342C0 File Offset: 0x002324C0
		[SecuritySafeCritical]
		internal static bool CallerAndAppDomainHaveUnrestrictedWebBrowserPermission()
		{
			if (!SecurityHelper.AppDomainHasPermission(SecurityHelper.CachedWebBrowserPermission))
			{
				return false;
			}
			try
			{
				SecurityHelper.DemandWebBrowserPermission();
			}
			catch (SecurityException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x002342FC File Offset: 0x002324FC
		[SecuritySafeCritical]
		internal static bool CallerHasUserInitiatedNavigationPermission()
		{
			try
			{
				SecurityHelper.CreateUserInitiatedNavigationPermission();
				SecurityHelper._userInitiatedNavigationPermission.Demand();
			}
			catch (SecurityException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x00234334 File Offset: 0x00232534
		[SecurityCritical]
		internal static CodeAccessPermission CreateUserInitiatedNavigationPermission()
		{
			if (SecurityHelper._userInitiatedNavigationPermission == null)
			{
				SecurityHelper._userInitiatedNavigationPermission = new UserInitiatedNavigationPermission();
			}
			return SecurityHelper._userInitiatedNavigationPermission;
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x0023434C File Offset: 0x0023254C
		[SecurityCritical]
		internal static void DemandPrintDialogPermissions()
		{
			if (SecurityHelper._defaultPrintingPermission == null)
			{
				SecurityHelper._defaultPrintingPermission = SystemDrawingHelper.NewDefaultPrintingPermission();
			}
			SecurityHelper._defaultPrintingPermission.Demand();
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x0023436C File Offset: 0x0023256C
		[SecuritySafeCritical]
		internal static bool CallerHasMemberAccessReflectionPermission()
		{
			try
			{
				if (SecurityHelper._reflectionPermission == null)
				{
					SecurityHelper._reflectionPermission = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);
				}
				SecurityHelper._reflectionPermission.Demand();
			}
			catch (SecurityException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x002343B0 File Offset: 0x002325B0
		[SecuritySafeCritical]
		internal static Exception GetExceptionForHR(int hr)
		{
			return Marshal.GetExceptionForHR(hr, new IntPtr(-1));
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x002343BE File Offset: 0x002325BE
		[SecurityCritical]
		internal static void DemandUIWindowPermission()
		{
			if (SecurityHelper._allWindowsUIPermission == null)
			{
				SecurityHelper._allWindowsUIPermission = new UIPermission(UIPermissionWindow.AllWindows);
			}
			SecurityHelper._allWindowsUIPermission.Demand();
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x002343DC File Offset: 0x002325DC
		[SecurityCritical]
		internal static void DemandUnrestrictedUIPermission()
		{
			if (SecurityHelper._unrestrictedUIPermission == null)
			{
				SecurityHelper._unrestrictedUIPermission = new UIPermission(PermissionState.Unrestricted);
			}
			SecurityHelper._unrestrictedUIPermission.Demand();
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x002343FA File Offset: 0x002325FA
		[SecurityCritical]
		internal static void DemandUnrestrictedFileIOPermission()
		{
			if (SecurityHelper._unrestrictedFileIOPermission == null)
			{
				SecurityHelper._unrestrictedFileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			}
			SecurityHelper._unrestrictedFileIOPermission.Demand();
		}

		// Token: 0x06007D90 RID: 32144 RVA: 0x00234418 File Offset: 0x00232618
		[SecurityCritical]
		internal static void DemandFileDialogOpenPermission()
		{
			if (SecurityHelper._fileDialogOpenPermission == null)
			{
				SecurityHelper._fileDialogOpenPermission = new FileDialogPermission(FileDialogPermissionAccess.Open);
			}
			SecurityHelper._fileDialogOpenPermission.Demand();
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x00234438 File Offset: 0x00232638
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ShowMessageBoxHelper(Window parent, string text, string title, MessageBoxButton buttons, MessageBoxImage image)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				if (parent != null)
				{
					MessageBox.Show(parent, text, title, buttons, image);
				}
				else
				{
					MessageBox.Show(text, title, buttons, image);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x00234484 File Offset: 0x00232684
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ShowMessageBoxHelper(IntPtr parentHwnd, string text, string title, MessageBoxButton buttons, MessageBoxImage image)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				MessageBox.ShowCore(parentHwnd, text, title, buttons, image, MessageBoxResult.None, MessageBoxOptions.None);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06007D93 RID: 32147 RVA: 0x002344C4 File Offset: 0x002326C4
		[SecurityCritical]
		internal static void DemandPlugInSerializerPermissions()
		{
			if (SecurityHelper._plugInSerializerPermissions == null)
			{
				SecurityHelper._plugInSerializerPermissions = new PermissionSet(PermissionState.Unrestricted);
			}
			SecurityHelper._plugInSerializerPermissions.Demand();
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x002344E2 File Offset: 0x002326E2
		internal static bool AreStringTypesEqual(string m1, string m2)
		{
			return string.Compare(m1, m2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x04003B3A RID: 15162
		private static SecurityPermission _unmanagedCodePermission;

		// Token: 0x04003B3B RID: 15163
		private static WebBrowserPermission _webBrowserPermission;

		// Token: 0x04003B3C RID: 15164
		private static UserInitiatedNavigationPermission _userInitiatedNavigationPermission;

		// Token: 0x04003B3D RID: 15165
		private static CodeAccessPermission _defaultPrintingPermission;

		// Token: 0x04003B3E RID: 15166
		private static ReflectionPermission _reflectionPermission;

		// Token: 0x04003B3F RID: 15167
		private static UIPermission _allWindowsUIPermission;

		// Token: 0x04003B40 RID: 15168
		private static UIPermission _unrestrictedUIPermission;

		// Token: 0x04003B41 RID: 15169
		private static FileIOPermission _unrestrictedFileIOPermission;

		// Token: 0x04003B42 RID: 15170
		private static FileDialogPermission _fileDialogOpenPermission;

		// Token: 0x04003B43 RID: 15171
		private static PermissionSet _plugInSerializerPermissions;
	}
}
