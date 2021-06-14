using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020005E5 RID: 1509
	internal class AssemblyFilter
	{
		// Token: 0x0600647C RID: 25724 RVA: 0x001C32E6 File Offset: 0x001C14E6
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static AssemblyFilter()
		{
			AssemblyFilter._disallowedListExtracted = new SecurityCriticalDataForSet<bool>(false);
			AssemblyFilter._assemblyList = new SecurityCriticalDataForSet<List<string>>(new List<string>());
		}

		// Token: 0x0600647D RID: 25725 RVA: 0x001C330C File Offset: 0x001C150C
		[SecurityCritical]
		internal void FilterCallback(object sender, AssemblyLoadEventArgs args)
		{
			object @lock = AssemblyFilter._lock;
			lock (@lock)
			{
				Assembly loadedAssembly = args.LoadedAssembly;
				if (!loadedAssembly.ReflectionOnly && loadedAssembly.GlobalAssemblyCache)
				{
					object[] customAttributes = loadedAssembly.GetCustomAttributes(typeof(AllowPartiallyTrustedCallersAttribute), false);
					if (customAttributes.Length != 0 && customAttributes[0] is AllowPartiallyTrustedCallersAttribute)
					{
						string text = this.AssemblyNameWithFileVersion(loadedAssembly);
						if (this.AssemblyOnDisallowedList(text))
						{
							UnsafeNativeMethods.ProcessUnhandledException_DLL(SR.Get("KillBitEnforcedShutdown") + text);
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							try
							{
								Environment.Exit(-1);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600647E RID: 25726 RVA: 0x001C33CC File Offset: 0x001C15CC
		[SecurityCritical]
		private string AssemblyNameWithFileVersion(Assembly a)
		{
			StringBuilder stringBuilder = new StringBuilder(a.FullName);
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			FileVersionInfo versionInfo;
			try
			{
				versionInfo = FileVersionInfo.GetVersionInfo(a.Location);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (versionInfo != null && versionInfo.ProductVersion != null)
			{
				stringBuilder.Append(", FileVersion=" + versionInfo.ProductVersion);
			}
			return stringBuilder.ToString().ToLower(CultureInfo.InvariantCulture).Trim();
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x001C344C File Offset: 0x001C164C
		[SecurityCritical]
		private bool AssemblyOnDisallowedList(string assemblyToCheck)
		{
			bool result = false;
			if (!AssemblyFilter._disallowedListExtracted.Value)
			{
				this.ExtractDisallowedRegistryList();
				AssemblyFilter._disallowedListExtracted.Value = true;
			}
			if (AssemblyFilter._assemblyList.Value.Contains(assemblyToCheck))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x001C3490 File Offset: 0x001C1690
		[SecurityCritical]
		private void ExtractDisallowedRegistryList()
		{
			new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NetFramework\\policy\\APTCA").Assert();
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA");
				if (registryKey != null)
				{
					string[] subKeyNames = registryKey.GetSubKeyNames();
					foreach (string text in subKeyNames)
					{
						registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA\\" + text);
						object value = registryKey.GetValue("APTCA_FLAG");
						if (value != null && (int)value == 1 && !AssemblyFilter._assemblyList.Value.Contains(text))
						{
							AssemblyFilter._assemblyList.Value.Add(text.ToLower(CultureInfo.InvariantCulture).Trim());
						}
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x040032AC RID: 12972
		private static SecurityCriticalDataForSet<List<string>> _assemblyList;

		// Token: 0x040032AD RID: 12973
		private static SecurityCriticalDataForSet<bool> _disallowedListExtracted;

		// Token: 0x040032AE RID: 12974
		private static object _lock = new object();

		// Token: 0x040032AF RID: 12975
		private const string FILEVERSION_STRING = ", FileVersion=";

		// Token: 0x040032B0 RID: 12976
		private const string KILL_BIT_REGISTRY_HIVE = "HKEY_LOCAL_MACHINE\\";

		// Token: 0x040032B1 RID: 12977
		private const string KILL_BIT_REGISTRY_LOCATION = "Software\\Microsoft\\.NetFramework\\policy\\APTCA";

		// Token: 0x040032B2 RID: 12978
		private const string SUBKEY_VALUE = "APTCA_FLAG";
	}
}
