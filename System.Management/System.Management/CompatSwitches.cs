using System;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Management
{
	// Token: 0x0200003E RID: 62
	internal static class CompatSwitches
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000BF18 File Offset: 0x0000A118
		public static bool AllowIManagementObjectQI
		{
			get
			{
				if (CompatSwitches.s_allowManagementObjectQI == 0)
				{
					object obj = CompatSwitches.s_syncLock;
					lock (obj)
					{
						if (CompatSwitches.s_allowManagementObjectQI == 0)
						{
							CompatSwitches.s_allowManagementObjectQI = (CompatSwitches.GetSwitchValueFromRegistry() ? 1 : -1);
						}
					}
				}
				return CompatSwitches.s_allowManagementObjectQI == 1;
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000BF7C File Offset: 0x0000A17C
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool GetSwitchValueFromRegistry()
		{
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319");
				if (registryKey == null)
				{
					return false;
				}
				return (int)registryKey.GetValue("WMIDisableCOMSecurity", -1) == 1;
			}
			catch (Exception ex)
			{
				if (ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException || ex is AccessViolationException)
				{
					throw;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Dispose();
				}
			}
			return false;
		}

		// Token: 0x0400017E RID: 382
		private const string DotNetVersion = "v4.0.30319";

		// Token: 0x0400017F RID: 383
		private const string RegKeyLocation = "SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319";

		// Token: 0x04000180 RID: 384
		private static readonly object s_syncLock = new object();

		// Token: 0x04000181 RID: 385
		private static int s_allowManagementObjectQI;

		// Token: 0x04000182 RID: 386
		private const string c_WMIDisableCOMSecurity = "WMIDisableCOMSecurity";
	}
}
