using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace MS.Internal
{
	// Token: 0x020005DC RID: 1500
	internal static class NetfxVersionHelper
	{
		// Token: 0x060063AA RID: 25514 RVA: 0x001C0A8C File Offset: 0x001BEC8C
		[SecuritySafeCritical]
		internal static int GetNetFXReleaseVersion()
		{
			int result = 0;
			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, NetfxVersionHelper._frameworkRegKeyFullPath);
			try
			{
				registryPermission.Assert();
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(NetfxVersionHelper._frameworkRegKey);
				if (registryKey != null)
				{
					object value = registryKey.GetValue("Release");
					if (value != null)
					{
						result = Convert.ToInt32(value);
					}
				}
			}
			catch (Exception ex) when (ex is SecurityException || ex is ObjectDisposedException || ex is IOException || ex is UnauthorizedAccessException || ex is FormatException || ex is OverflowException)
			{
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x060063AB RID: 25515 RVA: 0x001C0B4C File Offset: 0x001BED4C
		internal static string GetTargetFrameworkVersion()
		{
			string result = string.Empty;
			string targetFrameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
			if (!string.IsNullOrEmpty(targetFrameworkName))
			{
				try
				{
					FrameworkName frameworkName = new FrameworkName(targetFrameworkName);
					result = frameworkName.Version.ToString();
				}
				catch (Exception ex) when (ex is ArgumentException)
				{
				}
			}
			return result;
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x060063AC RID: 25516 RVA: 0x001C0BBC File Offset: 0x001BEDBC
		internal static string FrameworkRegKey
		{
			get
			{
				return NetfxVersionHelper._frameworkRegKey;
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x060063AD RID: 25517 RVA: 0x001C0BC3 File Offset: 0x001BEDC3
		internal static string FrameworkRegKeyFullPath
		{
			get
			{
				return NetfxVersionHelper._frameworkRegKeyFullPath;
			}
		}

		// Token: 0x040031ED RID: 12781
		private static readonly string _frameworkRegKey = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full";

		// Token: 0x040031EE RID: 12782
		private static readonly string _frameworkRegKeyFullPath = "HKEY_LOCAL_MACHINE\\" + NetfxVersionHelper._frameworkRegKey;
	}
}
