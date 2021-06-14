using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C2 RID: 194
	internal sealed class WMICapabilities
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x000268A4 File Offset: 0x00024AA4
		static WMICapabilities()
		{
			WMICapabilities.wmiNetKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM\\.NET", false);
			WMICapabilities.wmiKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM", false);
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x000268DC File Offset: 0x00024ADC
		public static bool MultiIndicateSupported
		{
			get
			{
				if (-1 == WMICapabilities.multiIndicateSupported)
				{
					WMICapabilities.multiIndicateSupported = (WMICapabilities.MultiIndicatePossible() ? 1 : 0);
					if (WMICapabilities.wmiNetKey != null)
					{
						object value = WMICapabilities.wmiNetKey.GetValue("MultiIndicateSupported", WMICapabilities.multiIndicateSupported);
						if (value.GetType() == typeof(int) && (int)value == 1)
						{
							WMICapabilities.multiIndicateSupported = 1;
						}
					}
				}
				return WMICapabilities.multiIndicateSupported == 1;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00026950 File Offset: 0x00024B50
		public static void AddAutorecoverMof(string path)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\WBEM\\CIMOM", true);
			if (registryKey != null)
			{
				object value = registryKey.GetValue("Autorecover MOFs");
				string[] array = value as string[];
				if (array == null)
				{
					if (value != null)
					{
						return;
					}
					array = new string[0];
				}
				registryKey.SetValue("Autorecover MOFs timestamp", DateTime.Now.ToFileTime().ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))));
				foreach (string strA in array)
				{
					if (string.Compare(strA, path, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return;
					}
				}
				string[] array3 = new string[array.Length + 1];
				array.CopyTo(array3, 0);
				array3[array3.Length - 1] = path;
				registryKey.SetValue("Autorecover MOFs", array3);
				registryKey.SetValue("Autorecover MOFs timestamp", DateTime.Now.ToFileTime().ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))));
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00026A5B File Offset: 0x00024C5B
		public static string InstallationDirectory
		{
			get
			{
				if (WMICapabilities.installationDirectory == null && WMICapabilities.wmiKey != null)
				{
					WMICapabilities.installationDirectory = WMICapabilities.wmiKey.GetValue("Installation Directory").ToString();
				}
				return WMICapabilities.installationDirectory;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00026A89 File Offset: 0x00024C89
		public static string FrameworkDirectory
		{
			get
			{
				return Path.Combine(WMICapabilities.InstallationDirectory, "Framework");
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00026A9C File Offset: 0x00024C9C
		public static bool IsUserAdmin()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				return true;
			}
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
			return windowsPrincipal.Identity.IsAuthenticated && windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00026ADD File Offset: 0x00024CDD
		private static bool IsNovaFile(FileVersionInfo info)
		{
			return info.FileMajorPart == 1 && info.FileMinorPart == 50 && info.FileBuildPart == 1085;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00026B04 File Offset: 0x00024D04
		private static bool MultiIndicatePossible()
		{
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 1))
			{
				return true;
			}
			string fileName = Path.Combine(Environment.SystemDirectory, "wbem\\fastprox.dll");
			FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
			return WMICapabilities.IsNovaFile(versionInfo) && versionInfo.FilePrivatePart >= 56;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00026B64 File Offset: 0x00024D64
		public static bool IsWindowsXPOrHigher()
		{
			OperatingSystem osversion = Environment.OSVersion;
			return osversion.Platform == PlatformID.Win32NT && osversion.Version >= new Version(5, 1);
		}

		// Token: 0x0400052C RID: 1324
		private const string WMIKeyPath = "Software\\Microsoft\\WBEM";

		// Token: 0x0400052D RID: 1325
		private const string WMINetKeyPath = "Software\\Microsoft\\WBEM\\.NET";

		// Token: 0x0400052E RID: 1326
		private const string WMICIMOMKeyPath = "Software\\Microsoft\\WBEM\\CIMOM";

		// Token: 0x0400052F RID: 1327
		private const string MultiIndicateSupportedValueNameVal = "MultiIndicateSupported";

		// Token: 0x04000530 RID: 1328
		private const string AutoRecoverMofsVal = "Autorecover MOFs";

		// Token: 0x04000531 RID: 1329
		private const string AutoRecoverMofsTimestampVal = "Autorecover MOFs timestamp";

		// Token: 0x04000532 RID: 1330
		private const string InstallationDirectoryVal = "Installation Directory";

		// Token: 0x04000533 RID: 1331
		private const string FrameworkSubDirectory = "Framework";

		// Token: 0x04000534 RID: 1332
		private static RegistryKey wmiNetKey;

		// Token: 0x04000535 RID: 1333
		private static RegistryKey wmiKey;

		// Token: 0x04000536 RID: 1334
		private static int multiIndicateSupported = -1;

		// Token: 0x04000537 RID: 1335
		private static string installationDirectory = null;
	}
}
